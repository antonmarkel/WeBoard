using System.Windows;
using MahApps.Metro.Controls;

namespace WeBoard.Client.WPF.Windows
{
    public partial class AccountWindow : MetroWindow
    {
        private readonly string _token;
        public AccountWindow(string token)
        {
            InitializeComponent();
            _token = token;  
        }

        private void AccountButtonClick(object sender, RoutedEventArgs e)
        {
            AccountMenu.IsOpen = !AccountMenu.IsOpen;
        }

        private void LogOutButtonClick(object sender, RoutedEventArgs e)
        {
        }
        private void CreateBoardButtonClick(object sender, RoutedEventArgs e)
        {
            CreateBoardMenu.IsOpen = true;
        }
        private void CancelCreateBoardButtonClick(object sender, RoutedEventArgs e)
        {
            CreateBoardMenu.IsOpen = false;
        }

        private void ConfirmCreateBoardButtonClick(object sender, RoutedEventArgs e)
        {
            CreateBoardMenu.IsOpen = false;
        }

        private void AddBoardButtonClick(object sender, RoutedEventArgs e)
        {
            AddBoardMenu.IsOpen = true;
        }

        private void CancelAddBoardButtonClick(object sender, RoutedEventArgs e)
        {
            AddBoardMenu.IsOpen = false;
        }

        private void ConfirmAddBoardButtonClick(object sender, RoutedEventArgs e)
        {
            AddBoardMenu.IsOpen = false;
        }

    }
}
