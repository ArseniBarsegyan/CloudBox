using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CloudBox.WPFClient.Models;
using CloudBox.WPFClient.ServiceReference1;
using Microsoft.Win32;
using ListViewItem = System.Windows.Controls.ListViewItem;
using MessageBox = System.Windows.MessageBox;
using UserControl = System.Windows.Controls.UserControl;

namespace CloudBox.WPFClient.Menu
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : UserControl
    {
        private static string _userName;

        public MainWindow(string userName)
        {
            InitializeComponent();

            _userName = userName;
            CurrentPath.Text = _userName;
            UserNameField.Text = "Hello, " + userName + "!";
            ShowAllContentByCurrentPath();
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
                    var directoryModel = new DirectoryModel {Name = directoryName, CreationTime = directoryCreationTime};

                    ((ArrayList)ListView.Resources["Items"]).Add(directoryModel);
                }
                foreach (var file in allFiles)
                {
                    var fileName = file.Split('[')[0];
                    var extension = fileName.Split('.').Last();
                    var fileCreationTime = file.Split('[')[1];
                    var fileModel = new FileModel { Name = fileName, Extension = extension, CreationTime = fileCreationTime};

                    ((ArrayList) ListView.Resources["Items"]).Add(fileModel);
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
                        MessageBox.Show("Upload result", result, MessageBoxButton.OK);
                    }
                }
            }
            ShowAllContentByCurrentPath();
        }
    }
}
