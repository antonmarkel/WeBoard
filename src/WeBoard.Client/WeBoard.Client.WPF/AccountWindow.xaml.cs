using System.Windows;
using MahApps.Metro.Controls;

namespace WeBoard.Client.WPF
{
    public partial class AccountWindow : MetroWindow
    {
        public AccountWindow()
        {
            InitializeComponent();
        }

        private void AccountButtonClick(object sender, RoutedEventArgs e)
        {
            AccountMenu.IsOpen = !AccountMenu.IsOpen;
        }

        private void LogOutButtonClick(object sender, RoutedEventArgs e)
        {
        }

        private void CreateNewBoardButtonClick(object sender, RoutedEventArgs e)
        {
            CreateBoardMenu.IsOpen = true;
        }

        private void CancelNewBoardButtonClick(object sender, RoutedEventArgs e)
        {
            CreateBoardMenu.IsOpen = false;
        }

        private void ConfirmNewBoardButtonClick(object sender, RoutedEventArgs e)
        {
            CreateBoardMenu.IsOpen = false;
        }

    }
}
