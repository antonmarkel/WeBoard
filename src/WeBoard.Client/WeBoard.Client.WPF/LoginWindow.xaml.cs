using System.Windows;
using MahApps.Metro.Controls;

namespace WeBoard.Client.WPF
{
    public partial class LoginWindow : MetroWindow
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void SignUpButtonClick(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
