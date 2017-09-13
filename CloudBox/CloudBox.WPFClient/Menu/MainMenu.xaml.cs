using System;
using System.Windows;
using System.Windows.Controls;

namespace CloudBox.WPFClient.Menu
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : UserControl, ISwitchable
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        private void RegisterButton_OnClick(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new Register());
        }

        private void LoginButton_OnClick(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new Login());
        }
    }
}
