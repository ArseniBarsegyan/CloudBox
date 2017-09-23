using System.Windows;
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
    }
}
