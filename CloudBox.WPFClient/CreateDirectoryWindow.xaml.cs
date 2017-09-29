using System.Windows;
using System.Windows.Input;
using CloudBox.WPFClient.ServiceReference1;

namespace CloudBox.WPFClient
{
    /// <summary>
    /// Interaction logic for CreateDirectoryWindow.xaml
    /// </summary>
    public partial class CreateDirectoryWindow : Window
    {
        private string _currentPath;
        private MainWindow _mainWindow;

        public CreateDirectoryWindow(string currentPath, MainWindow mainWindow)
        {
            InitializeComponent();
            CenterWindowOnScreen();
            _currentPath = currentPath;
            _mainWindow = mainWindow;
        }

        //Press enter - submit directory name and create it, escape - cancel directory creating
        private void CreateDirectoryWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                CreateDirectoryIfNotExists();
                Close();
            }
            else if (e.Key == Key.Escape)
            {
                Close();
            }
        }

        //Pressing Create button will create directory at server and after that close this window
        private void CreateButton_OnClick(object sender, RoutedEventArgs e)
        {
            CreateDirectoryIfNotExists();
            Close();
        }

        //Pressing cancel close this window
        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //Create directory if not exists on server and refresh main window
        private void CreateDirectoryIfNotExists()
        {
            using (var serviceClient = new CloudServiceClient())
            {
                var result = serviceClient.CreateDirectoryIfNotExists(_currentPath + @"\" + DirectoryName.Text);
                MessageBox.Show(result);
                _mainWindow.ShowAllContentByCurrentPath();
            }
        }

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
    }
}
