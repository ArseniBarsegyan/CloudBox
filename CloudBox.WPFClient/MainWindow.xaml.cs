﻿using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using CloudBox.WPFClient.Helpers;
using CloudBox.WPFClient.Models;
using CloudBox.WPFClient.ServiceReference1;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using ListViewItem = System.Windows.Controls.ListViewItem;
using MessageBox = System.Windows.MessageBox;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace CloudBox.WPFClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string _userName;
        private FileSystemWatcher _systemWatcher;
        private readonly NotifyIcon _notifyIcon;

        public MainWindow(string username)
        {
            InitializeComponent();
            CenterWindowOnScreen();
            _userName = username;

            _notifyIcon = new NotifyIcon
            {
                Icon = new System.Drawing.Icon(ConstantHelper.CloudBoxIcon),
                Visible = true
            };
            _notifyIcon.DoubleClick += NotifyIcon_OnDoubleClick;
            _notifyIcon.MouseDown += NotifyIcon_OnMouseDown;

            CurrentPath.Text = _userName;
            UserNameField.Text = "Hello, " + username + "!";
            CreateUserDirectoryIfNotExists();
            ShowAllContentByCurrentPath();

            //Initialize watching after user's directory after log in
            _systemWatcher =
                new FileSystemWatcher(AppDomain.CurrentDomain.BaseDirectory + @"\Accounts\" + _userName)
                {
                    EnableRaisingEvents = true,
                    IncludeSubdirectories = true
                };
            _systemWatcher.Created += SystemWatcher_OnCreated;
            _systemWatcher.Deleted += SystemWatcher_OnDeleted;

            Closing += MainWindow_OnClosing;
        }

        //------------------------------------------------------------------------
        //---------------------------Window events--------------------------------
        //------------------------------------------------------------------------

        //Center this window on screen
        private void CenterWindowOnScreen()
        {
            var screenWidth = SystemParameters.PrimaryScreenWidth;
            var screenHeight = SystemParameters.PrimaryScreenHeight;
            var windowWidth = Width;
            var windowHeight = Height;
            Left = (screenWidth / 2) - (windowWidth / 2);
            Top = (screenHeight / 2) - (windowHeight / 2);
        }

        //When 'Escape' key pressed close application
        private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }

        //Double click at icon in tray, and context menu item 'open' selected will show this window
        private void NotifyIcon_OnDoubleClick(object sender, EventArgs eventArgs)
        {
            Show();
            WindowState = WindowState.Normal;
        }

        //Right click at icon in tray will show context menu
        private void NotifyIcon_OnMouseDown(object sender, MouseEventArgs mouseEventArgs)
        {
            if (mouseEventArgs.Button == MouseButtons.Right)
            {
                var menu = (System.Windows.Controls.ContextMenu)FindResource("NotifierContextMenu");
                menu.IsOpen = true;
            }
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
                Hide();

            base.OnStateChanged(e);
        }

        //Confirm before closing app
        private void MainWindow_OnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            var messageBoxResult = MessageBox.Show(ConstantHelper.CloseApplicationQuestion, 
                ConstantHelper.QuitConfirmation, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.No)
            {
                cancelEventArgs.Cancel = true;
            }
        }

        private void Menu_Open(object sender, RoutedEventArgs e)
        {
            Show();
            WindowState = WindowState.Normal;
        }

        private void Menu_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CreateUserDirectoryIfNotExists()
        {
            var fullPath = AppDomain.CurrentDomain.BaseDirectory + @"\Accounts\" + _userName;
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }
        }

        //Check if user folder exists. If not - create it. Shows all folders and files
        //by path
        public void ShowAllContentByCurrentPath()
        {
            //Clear list before displaying items
            ((ArrayList)ListView.Resources["Items"]).Clear();
            using (var serviceClient = new CloudServiceClient())
            {
                serviceClient.CheckIfDirectoryWithUserNameExists(_userName);
                var allDirectories = serviceClient.GetAllDirectoriesByPath(CurrentPath.Text);
                var allFiles = serviceClient.GetAllFilesByPath(CurrentPath.Text);

                foreach (var directory in allDirectories)
                {
                    var directoryName = directory.Split('[')[0];
                    var directoryCreationTime = directory.Split('[')[1];
                    var directoryModel = new DirectoryModel { Name = directoryName, CreationTime = directoryCreationTime };

                    ((ArrayList)ListView.Resources["Items"]).Add(directoryModel);
                }
                foreach (var file in allFiles)
                {
                    var fileName = file.Split('[')[0];
                    var extension = fileName.Split('.').Last();
                    var fileCreationTime = file.Split('[')[1];
                    var fileModel = new FileModel { Name = fileName, Extension = extension, CreationTime = fileCreationTime };

                    ((ArrayList)ListView.Resources["Items"]).Add(fileModel);
                }
            }
            ListView.Items.Refresh();
        }

        //Double click on listViewItem open this item
        private void ListView_OnItemDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem item)
            {
                var directory = item.Content as DirectoryModel;
                var file = item.Content as FileModel;

                if (directory != null)
                {
                    CurrentPath.Text += @"\" + directory.Name;
                    ShowAllContentByCurrentPath();
                }
                else if (file != null)
                {
                    using (var serviceClient = new CloudServiceClient())
                    {
                        var fileLink = serviceClient.GetFileLink(CurrentPath.Text + @"\" + file.Name);
                        Process.Start(fileLink);
                    }
                }
            }
            ListView.Items.Refresh();
        }

        //Clicking on delete context menu - remove element from list and refresh view
        private void DeleteMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            DeleteListViewItem();
        }

        //When list item selected pressing 'del' will delete item
        private void ListView_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                DeleteListViewItem();
            }
        }

        private void DeleteListViewItem()
        {
            //asking. If user confirm proceed
            var messageBoxResult = MessageBox.Show(ConstantHelper.AreYouSure, 
                ConstantHelper.DeleteConfirmation, MessageBoxButton.YesNo);
            if (messageBoxResult != MessageBoxResult.Yes) return;

            var index = ListView.SelectedIndex;
            var selectedItem = ListView.Items.GetItemAt(index);

            if (selectedItem != null)
            {
                var directory = selectedItem as DirectoryModel;
                var file = selectedItem as FileModel;

                if (directory != null)
                {
                    using (var serviceClient = new CloudServiceClient())
                    {
                        serviceClient.RemoveElement(CurrentPath.Text + @"\" + directory.Name);
                    }
                    ((ArrayList)ListView.Resources["Items"]).Remove(directory);
                }
                else if (file != null)
                {
                    using (var serviceClient = new CloudServiceClient())
                    {
                        serviceClient.RemoveElement(CurrentPath.Text + @"\" + file.Name);
                    }
                    ((ArrayList)ListView.Resources["Items"]).Remove(file);
                }
                ListView.Items.Refresh();
            }
        }

        //When back clicked return to one level
        private void BackButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!CurrentPath.Text.Equals(_userName))
            {
                var currentPath = CurrentPath.Text;
                var newPath = currentPath.Substring(0, currentPath.LastIndexOf('\\'));
                CurrentPath.Text = newPath;
                ShowAllContentByCurrentPath();
            }
        }

        //Click on CreateFolderButton creating new directory
        private void CreateDirectoryButton_OnClick(object sender, RoutedEventArgs e)
        {
            var createDirectoryDialog = new CreateDirectoryWindow(CurrentPath.Text, this);
            createDirectoryDialog.Show();
        }

        //Open dialog box where user can select file and upload it to server
        private void UploadButton_OnClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() != true) return;
            foreach (var fullFileName in openFileDialog.FileNames)
            {
                var fileContent = File.ReadAllBytes(fullFileName);
                var savePath = CurrentPath.Text + @"\" + new FileInfo(fullFileName).Name;

                using (var serviceClient = new CloudServiceClient())
                {
                    var result = serviceClient.Upload(fileContent, savePath);
                    if (result.Equals("file uploaded"))
                    {
                        MessageBox.Show(ConstantHelper.UploadSuccessful, 
                            ConstantHelper.UploadResult, MessageBoxButton.OK);
                    }
                }
            }
            ShowAllContentByCurrentPath();
        }

        //------------------------------------------------------------------------
        //---------------------FileSystemWatcher events---------------------------
        //------------------------------------------------------------------------
        private void SystemWatcher_OnCreated(object sender, FileSystemEventArgs fileSystemEventArgs)
        {
            _notifyIcon.Icon = new System.Drawing.Icon(ConstantHelper.CloudBoxSyncIcon);
            //running new task
            var task = new Task(() =>
            {
                //Finding relative to user's directory path to object that cause event
                var regex = new Regex(_userName + ".+");
                var relativePath = regex.Matches(fileSystemEventArgs.FullPath)[0].ToString();
                //Split relative path by '\\' and we get array consists of directories names, where last element
                //can be a file
                var splittedPath = relativePath.Split('\\');

                //Path relative to Accounts directory
                var currentRelativePath = new StringBuilder(splittedPath[0]);
                //Full path
                var currentFullPath = new StringBuilder(AppDomain.CurrentDomain.BaseDirectory + @"\Accounts\" + _userName);

                //Walk through path. If current directory doesn't exists on server, create it.
                //If element in path - file, upload it
                for (var i = 1; i < splittedPath.Length; i++)
                {
                    currentRelativePath.Append(@"\" + splittedPath[i]);
                    currentFullPath.Append(@"\" + splittedPath[i]);

                    using (var serviceClient = new CloudServiceClient())
                    {
                        //If current element is a directory, create it in server
                        if (Directory.Exists(currentFullPath.ToString()))
                        {
                            serviceClient.CreateDirectoryIfNotExists(currentRelativePath.ToString());
                        }
                        //Else it's a file
                        else
                        {
                            //Convert file into byte[] and copy to server
                            var fileContent = File.ReadAllBytes(fileSystemEventArgs.FullPath);
                            serviceClient.Upload(fileContent, currentRelativePath.ToString());
                        }
                    }
                }
            });
            task.Start();
            task.Wait();
            _notifyIcon.Icon = new System.Drawing.Icon(ConstantHelper.CloudBoxIcon);
        }

        private void SystemWatcher_OnDeleted(object sender, FileSystemEventArgs fileSystemEventArgs)
        {
            _notifyIcon.Icon = new System.Drawing.Icon(ConstantHelper.CloudBoxSyncIcon);
            var task = new Task(() =>
            {
                //Finding relative to user's directory path to object that cause event
                var regex = new Regex(_userName + ".+");
                var relativePath = regex.Matches(fileSystemEventArgs.FullPath)[0].ToString();

                //Delelte this object at server
                using (var serviceClient = new CloudServiceClient())
                {
                    serviceClient.RemoveElement(relativePath);
                }
            });
            task.Start();
            task.Wait();
            _notifyIcon.Icon = new System.Drawing.Icon(ConstantHelper.CloudBoxIcon);
        }
    }
}
