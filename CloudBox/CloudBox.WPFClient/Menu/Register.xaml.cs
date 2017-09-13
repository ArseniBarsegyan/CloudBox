using System;
using System.Windows;
using System.Windows.Controls;

namespace CloudBox.WPFClient.Menu
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : UserControl, ISwitchable
    {
        public Register()
        {
            InitializeComponent();
        }

        //Back to main page
        private void BackButton_OnClick(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new MainMenu());
        }

        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        //Register User if not exists
        private void SubmitButton_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
