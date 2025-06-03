using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MahApps.Metro.Controls;
using WeBoard.Client.WPF.Models;
using WeBoard.Client.WPF.Requests.Board;
using WeBoard.Client.WPF.Services;

namespace WeBoard.Client.WPF.Windows
{
    public partial class AccountWindow : MetroWindow, INotifyPropertyChanged
    {
        private readonly BoardApiService _boardService = new(new System.Net.Http.HttpClient());

        public ObservableCollection<BoardModel> Boards { get; } = new();

        private bool _showFavorites = false;
        public bool ShowFavorites
        {
            get => _showFavorites;
            set
            {
                if (_showFavorites != value)
                {
                    _showFavorites = value;
                    OnPropertyChanged(nameof(ShowFavorites));
                    OnPropertyChanged(nameof(DisplayedBoards));
                }
            }
        }

        public IEnumerable<BoardModel> DisplayedBoards =>
            ShowFavorites ? Boards.Where(b => b.IsFavorite) : Boards;

        private readonly Guid _id;

        public AccountWindow(Guid id)
        {
            InitializeComponent();
            _id = id;
            DataContext = this;

            LoadBoardsAsync();
        }

        private async void LoadBoardsAsync()
        {
            var response = await _boardService.GetUserBoardsAsync(_id);
            Dispatcher.Invoke(() =>
            {
                Boards.Clear();

                if (response.Success && response.Boards != null)
                {
                    foreach (var board in response.Boards)
                        Boards.Add(board);

                    OnPropertyChanged(nameof(DisplayedBoards));
                    NoBoardsText.Visibility = Boards.Any() ? Visibility.Collapsed : Visibility.Visible;
                }
                else
                {
                    NoBoardsText.Visibility = Visibility.Visible;
                }
            });
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

        private void CopyBoardIdClick(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem && menuItem.DataContext is BoardModel board)
            {
                Clipboard.SetText(board.Id.ToString());
            }
        }

        private void ToggleFavoriteClick(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem && menuItem.DataContext is BoardModel board)
            {
                board.IsFavorite = !board.IsFavorite;
                OnPropertyChanged(nameof(DisplayedBoards));
            }
        }

        private void HomeButtonClick(object sender, RoutedEventArgs e)
        {
            ShowFavorites = false;
        }

        private void FavoritesButtonClick(object sender, RoutedEventArgs e)
        {
            ShowFavorites = true;
        }

        private void AccountButtonClick(object sender, RoutedEventArgs e)
        {
            AccountMenu.IsOpen = !AccountMenu.IsOpen;
        }

        private void LogOutButtonClick(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            Close();
            loginWindow.Show();
        }

        private void CreateBoardButtonClick(object sender, RoutedEventArgs e)
        {
            CreateBoardMenu.IsOpen = true;
        }

        private void CancelCreateBoardButtonClick(object sender, RoutedEventArgs e)
        {
            CreateBoardMenu.IsOpen = false;
        }

        private async void ConfirmCreateBoardButtonClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(BoardTitleTextBox.Text))
            {
                var request = new CreateBoardRequest()
                {
                    Name = BoardTitleTextBox.Text,
                };

                var response = await _boardService.CreateBoardAsync(request, _id);
                if (response.Success)
                {
                    LoadBoardsAsync();
                    CreateBoardMenu.IsOpen = false;
                }
            }
            CreateBoardMenu.IsOpen = false;
            BoardTitleTextBox.Text = string.Empty;
        }

        private void AddBoardButtonClick(object sender, RoutedEventArgs e)
        {
            AddBoardMenu.IsOpen = true;
        }

        private void CancelAddBoardButtonClick(object sender, RoutedEventArgs e)
        {
            AddBoardMenu.IsOpen = false;
        }

        private async void ConfirmAddBoardButtonClick(object sender, RoutedEventArgs e)
        {
            if (Guid.TryParse(BoardIdTextBox.Text, out var boardId))
            {
                var response = await _boardService.AddBoardAsync(boardId, _id);

                if (response.Success)
                {
                    LoadBoardsAsync();
                    AddBoardMenu.IsOpen = false;
                    BoardIdTextBox.Text = string.Empty;
                }
            }
        }

        private void BoardMouseLeftButtonDownClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.DataContext is BoardModel board)
            {
                string exePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WeBoard.Client.exe");

                Hide();

                var processInfo = new ProcessStartInfo
                {
                    FileName = exePath,
                    Arguments = $"{_id} {board.Id}",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WorkingDirectory = Path.GetDirectoryName(exePath)
                };

                var process = new Process { StartInfo = processInfo };
                process.EnableRaisingEvents = true;
                process.Exited += (s, args) => Dispatcher.Invoke(() => Show());
                process.Start();
            }
        }
    }
}
