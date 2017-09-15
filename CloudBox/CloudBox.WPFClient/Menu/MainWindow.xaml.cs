using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace CloudBox.WPFClient.Menu
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : UserControl
    {
        public MainWindow(string userName)
        {
            InitializeComponent();
            UserNameField.Text = "Hello, " + userName + "!";
            CreateUserDirectoryIfNotExists(userName);
            ShowTree(userName);
        }

        //Create user's folder in 'Accounts\' if not exists and show it in TreeView
        private void CreateUserDirectoryIfNotExists(string userName)
        {
            var userDirectory = CreateAccountsDirectoryIfNotExists() + @"\" + userName;
            if (!Directory.Exists(userDirectory))
            {
                Directory.CreateDirectory(userDirectory);
            }
        }

        private void ShowTree(string userName)
        {
            var userDirectory = CreateAccountsDirectoryIfNotExists() + @"\" + userName;

            DirectoryInfo info = new DirectoryInfo(userDirectory);
            foreach (var directory in info.GetDirectories())
            {
                var item = new TreeViewItem
                {
                    Header = directory.ToString(),
                    Tag = directory
                };
                item.Items.Add("*");
                TreeView.Items.Add(item);
            }
        }

        //Create folder 'Accounts' if not exists
        private string CreateAccountsDirectoryIfNotExists()
        {
            var accountsPath = System.AppDomain.CurrentDomain.BaseDirectory + @"\Accounts";
            if (!Directory.Exists(accountsPath))
            {
                Directory.CreateDirectory(accountsPath);
            }
            return accountsPath;
        }

        private void TreeView_OnExpanded(object sender, RoutedEventArgs e)
        {
            var item = (TreeViewItem) e.OriginalSource;
            item.Items.Clear();

            var directories = ((DirectoryInfo)item.Tag).GetDirectories();
            var files = ((DirectoryInfo) item.Tag).GetFiles();
            
            foreach (var directory in directories)
            {
                var sItem = new TreeViewItem
                {
                    Header = directory.ToString(),
                    Tag = directory
                };
                sItem.Items.Add("*");
                item.Items.Add(sItem);
            }
            foreach (var file in files)
            {
                item.Items.Add(file);
            }
        }
    }
}
