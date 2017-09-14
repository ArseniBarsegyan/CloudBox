using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CloudBox.WPFClient.ServiceReference1;

namespace CloudBox.WPFClient.Menu
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : UserControl, ISwitchable
    {
        public Login()
        {
            InitializeComponent();
        }

        public void UtilizeState(object state)
        {
            throw new System.NotImplementedException();
        }

        //Back to main page
        private void BackButton_OnClick(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new MainMenu());
        }
        
        private void SubmitButton_OnClick(object sender, RoutedEventArgs e)
        {
            LoginConfirmed();
        }
        
        //Enter call login method, escape returns back to MainMenu
        private void Login_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LoginConfirmed();
            }
            else if (e.Key == Key.Escape)
            {
                Switcher.Switch(new MainMenu());
            }
        }

        //If validation successfull go to main page, else show message with validation error
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
                        Switcher.Switch(new MainWindow(userName));
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
