using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CloudBox.WPFClient.Models;
using CloudBox.WPFClient.ServiceReference1;

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
            ShowAllFoldersInRootFolder();
        }

        //Check if user folder exists. If not - create it
        private void ShowAllFoldersInRootFolder()
        {
            using (CloudBoxServiceClient serviceClient = new CloudBoxServiceClient())
            {
                serviceClient.CheckIfDirectoryWithUserNameExists(_userName);
                var allDirectories = serviceClient.GetAllDirectoriesByPath(_userName, CurrentPath.Text);
                var allFiles = serviceClient.GetAllFilesByPath(_userName, CurrentPath.Text);
                foreach (var directory in allDirectories)
                {
                    var directoryName = directory.Split('[')[0];
                    var directoryCreationTime = directory.Split('[')[1];
                    var directoryModel = new DirectoryModel {Name = directoryName, CreationTime = directoryCreationTime, ActionName = "Delete"};

                    ((ArrayList)ListView.Resources["Items"]).Add(directoryModel);
                }
                foreach (var file in allFiles)
                {
                    var fileName = file.Split('[')[0];
                    var extension = fileName.Split('.').Last();
                    var fileCreationTime = file.Split('[')[1];
                    var fileModel = new FileModel { Name = fileName, Extension = extension, CreationTime = fileCreationTime, ActionName = "Delete"};

                    ((ArrayList) ListView.Resources["Items"]).Add(fileModel);
                }
            }
        }

        private void EventSetter_OnHandler(object sender, MouseButtonEventArgs e)
        {
            ListViewItem item = sender as ListViewItem;
            if (item != null)
            {
                var directory = item.Content as DirectoryModel;
                var file = item.Content as FileModel;

                if (directory != null)
                {
                    CurrentPath.Text += @"\" + directory.Name;
                }
            }
        }
    }
}
