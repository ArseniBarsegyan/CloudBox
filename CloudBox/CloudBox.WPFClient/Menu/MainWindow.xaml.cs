using System.Windows.Controls;

namespace CloudBox.WPFClient.Menu
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : UserControl
    {
        public MainWindow(string username)
        {
            InitializeComponent();
            UserPanelComboBox.Text = username;
        }
    }
}
