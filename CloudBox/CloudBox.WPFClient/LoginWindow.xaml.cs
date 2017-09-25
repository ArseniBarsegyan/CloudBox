using System.Windows;
using System.Windows.Input;
using CloudBox.WPFClient.ServiceReference1;

namespace CloudBox.WPFClient
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            CenterWindowOnScreen();
        }

        //Back to main page
        private void BackButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //Log in to service
        private void SubmitButton_OnClick(object sender, RoutedEventArgs e)
        {
            LoginConfirmed();
        }

        //If validation successfull opens main page, else show message with validation error
        private void LoginConfirmed()
        {
            using (CloudBoxServiceClient serviceClient = new CloudBoxServiceClient())
            {
                var userName = UserNameField.Text;
                var userPassword = UserPasswordField.Password;
                if (userName != string.Empty && userPassword != string.Empty)
                {
                    bool result = serviceClient.ValidateUser(userName, userPassword);
                    if (result)
                    {
                        var window = new MainWindow(userName);
                        window.Show();
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Incorrect login or password");
                    }
                }
            }
        }

        //When pressed Enter try to log in, escape - exit application
        private void LoginWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LoginConfirmed();
            }
            else if (e.Key == Key.Escape)
            {
                Close();
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
