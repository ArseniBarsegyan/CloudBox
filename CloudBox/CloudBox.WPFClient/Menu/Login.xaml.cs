using System.Windows;
using System.Windows.Controls;
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

        //If validation successfull go to main page, else show message with validation error
        private void SubmitButton_OnClick(object sender, RoutedEventArgs e)
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
