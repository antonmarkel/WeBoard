using System.Windows;
using MahApps.Metro.Controls;

namespace WeBoard.Client.WPF
{
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LogInButtonClick(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show(); 
            this.Close(); 
        }
    }
}