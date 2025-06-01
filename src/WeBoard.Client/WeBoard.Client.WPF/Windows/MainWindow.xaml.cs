using System.Windows;
using MahApps.Metro.Controls;
using WeBoard.Client.WPF.Requests;
using WeBoard.Client.WPF.Responses;
using WeBoard.Client.WPF.Services;

namespace WeBoard.Client.WPF.Windows
{
    public partial class MainWindow : MetroWindow
    {
        private readonly ApiService _apiService;
        public MainWindow()
        {
            InitializeComponent();
            _apiService = new ApiService();
        }

        private void LogInButtonClick(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            Close();
            loginWindow.Show(); 
        }

        private async void SignUpButtonClick(object sender, RoutedEventArgs e)
        {
            var username = UsernameTextBox.Text;
            var password = PasswordBox.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ErrorTextBlock.Text = "Username and password cannot be empty.";
                ErrorTextBlock.Visibility = Visibility.Visible;
                return;
            }

            var registerRequest = new RegisterRequest
            {
                Name = username,
                Password = password
            };

            ApiResponse registerResponse = await _apiService.RegisterAsync(registerRequest);

            if (registerResponse.Success)
            {
                LoginWindow loginWindow = new LoginWindow();
                Close();
                loginWindow.Show();
            }
            else
            {
                ErrorTextBlock.Text = registerResponse.Message;
                ErrorTextBlock.Visibility = Visibility.Visible;
            }
        }
    }
}