using System;
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
using CloudBox.WPFClient.Models;
using CloudBox.WPFClient.ServiceReference1;
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

        public MainWindow(string username)
        {
            InitializeComponent();
            _userName = username;

            var notifyIcon = new NotifyIcon
            {
                Icon = new System.Drawing.Icon("CloudBox.ico"),
                Visible = true
            };
            notifyIcon.DoubleClick += NotifyIcon_OnDoubleClick;
            notifyIcon.MouseDown += NotifyIcon_OnMouseDown;

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
            var messageBoxResult = MessageBox.Show("Close application?", "Quit confirmation", MessageBoxButton.YesNo, 
                MessageBoxImage.Warning);
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
            var messageBoxResult = MessageBox.Show("Are you sure?", "Delete Confirmation", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                Close();
            }
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
        private void ShowAllContentByCurrentPath()
        {
            //Clear list before displaying items
            ((ArrayList)ListView.Resources["Items"]).Clear();
            using (var serviceClient = new CloudBoxServiceClient())
            {
                serviceClient.CheckIfDirectoryWithUserNameExists(_userName);
                var allDirectories = serviceClient.GetAllDirectoriesByPath(_userName, CurrentPath.Text);
                var allFiles = serviceClient.GetAllFilesByPath(_userName, CurrentPath.Text);

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
            var item = sender as ListViewItem;
            if (item != null)
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
                    using (var serviceClient = new CloudBoxServiceClient())
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
            //asking. If user confirm proceed
            var messageBoxResult = MessageBox.Show("Are you sure?", "Delete Confirmation", MessageBoxButton.YesNo);
            if (messageBoxResult != MessageBoxResult.Yes) return;

            var index = ListView.SelectedIndex;
            var selectedItem = ListView.Items.GetItemAt(index);

            if (selectedItem != null)
            {
                var directory = selectedItem as DirectoryModel;
                var file = selectedItem as FileModel;

                if (directory != null)
                {
                    using (var serviceClient = new CloudBoxServiceClient())
                    {
                        serviceClient.RemoveElement(CurrentPath.Text + @"\" + directory.Name);
                    }
                    ((ArrayList)ListView.Resources["Items"]).Remove(directory);
                }
                else if (file != null)
                {
                    using (var serviceClient = new CloudBoxServiceClient())
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
            if (CurrentPath.Text.Equals(_userName))
            {
            }
            else
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

                using (var serviceClient = new CloudBoxServiceClient())
                {
                    var result = serviceClient.Upload(fileContent, savePath);
                    if (result.Equals("file uploaded"))
                    {
                        MessageBox.Show("Upload result", "Uploaded successfull", MessageBoxButton.OK);
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
                var currentFullPath = new StringBuilder(System.AppDomain.CurrentDomain.BaseDirectory + @"\Accounts\" + _userName);

                //Walk through path. If current directory doesn't exists on server, create it.
                //If element in path - file, upload it
                for (var i = 1; i < splittedPath.Length; i++)
                {
                    currentRelativePath.Append(@"\" + splittedPath[i]);
                    currentFullPath.Append(@"\" + splittedPath[i]);

                    using (var serviceClient = new CloudBoxServiceClient())
                    {
                        //If current element is a directory, create it in server
                        if (Directory.Exists(currentFullPath.ToString()))
                        {
                            serviceClient.CreateFolderIfNotExists(currentRelativePath.ToString());
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
        }

        private void SystemWatcher_OnDeleted(object sender, FileSystemEventArgs fileSystemEventArgs)
        {
            var task = new Task(() =>
            {
                //Finding relative to user's directory path to object that cause event
                var regex = new Regex(_userName + ".+");
                var relativePath = regex.Matches(fileSystemEventArgs.FullPath)[0].ToString();

                //Delelte this object at server
                using (var serviceClient = new CloudBoxServiceClient())
                {
                    serviceClient.RemoveElement(relativePath);
                }
            });
            task.Start();
            task.Wait();
        }
    }
}
