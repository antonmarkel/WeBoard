using System.Windows;
using MahApps.Metro.Controls;
using WeBoard.Client.WPF.Requests;
using WeBoard.Client.WPF.Responses;
using WeBoard.Client.WPF.Services;

namespace WeBoard.Client.WPF.Windows
{
    public partial class LoginWindow : MetroWindow
    {
        private readonly AuthenticationApiService _apiService;

        public LoginWindow()
        {
            InitializeComponent();
            _apiService = new AuthenticationApiService();
        }

        private async void LogInButtonClick(object sender, RoutedEventArgs e)
        {
            var username = UsernameTextBox.Text;
            var password = PasswordBox.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ErrorTextBlock.Text = "Username and password cannot be empty.";
                ErrorTextBlock.Visibility = Visibility.Visible;
                return;
            }

            var loginRequest = new LoginRequest
            {
                Name = username,
                Password = password
            };

            AuthenticationApiResponse loginResponse = await _apiService.LoginAsync(loginRequest);

            if (loginResponse.Success)
            {
                AccountWindow accountWindow = new AccountWindow(loginResponse.Id); 
                Close();
                accountWindow.Show();
            }
            else
            {
                ErrorTextBlock.Text = loginResponse.Message;
                ErrorTextBlock.Visibility = Visibility.Visible;
            }
        }

        private void SignUpButtonClick(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            Close();
            mainWindow.Show();
        }
    }
}