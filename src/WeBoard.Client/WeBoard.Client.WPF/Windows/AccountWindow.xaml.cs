using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using WeBoard.Client.WPF.Models;
using WeBoard.Client.WPF.Requests.Board;
using WeBoard.Client.WPF.Services;

namespace WeBoard.Client.WPF.Windows
{
    public partial class AccountWindow : MetroWindow
    {
        private readonly BoardApiService _boardService = new(new HttpClient());
        public ObservableCollection<BoardModel> Boards { get; } = new();
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
                    {
                        Boards.Add(board);
                    }
                    NoBoardsText.Visibility = Boards.Any() ? Visibility.Collapsed : Visibility.Visible;
                }
                else
                {
                    NoBoardsText.Visibility = Visibility.Visible;
                }
            });
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
            BoardTitleTextBox.Text = String.Empty;
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
                    BoardIdTextBox.Text = String.Empty;
                }
            }
        }
        
        private void CopyBoardIdClick(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem)
            {
                if (menuItem.DataContext is BoardModel board)
                {
                    Clipboard.SetText(board.Id.ToString());
                }
            }
        }

    }
}
