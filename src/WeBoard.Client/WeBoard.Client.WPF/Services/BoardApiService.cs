using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using WeBoard.Client.WPF.Models;
using WeBoard.Client.WPF.Requests.Board;
using WeBoard.Client.WPF.Responses;

namespace WeBoard.Client.WPF.Services;
public class BoardApiService
{
    private readonly HttpClient _httpClient;

    public BoardApiService(HttpClient httpClient)
    {
        _httpClient = new HttpClient { BaseAddress = new Uri("http://3.98.122.179:5005") };
    }

    public async Task<BoardApiResponse> CreateBoardAsync(CreateBoardRequest boardRequest, Guid token)
    {
        var content = new StringContent(JsonConvert.SerializeObject(boardRequest),
            Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"boards/{token}", content);

        if (response.IsSuccessStatusCode)
        {
            var idString = await response.Content.ReadAsStringAsync();
            idString = idString.Trim('"');
            return new BoardApiResponse
            {
                BoardId = Guid.TryParse(idString, out var id) ? id : Guid.Empty
            };
        }

        return new BoardApiResponse
        {
            Success = false,
            Message = await response.Content.ReadAsStringAsync()
        };
    }

    public async Task<BoardApiResponse> AddBoardAsync(Guid boardId, Guid token)
    {
        var response = await _httpClient.PostAsync($"boards/add/{boardId}/{token}", null);

        if (response.IsSuccessStatusCode)
            return new BoardApiResponse();

        return new BoardApiResponse
        {
            Success = false,
            Message = await response.Content.ReadAsStringAsync()
        };
    }

    public async Task<BoardApiResponse> GetUserBoardsAsync(Guid token)
    {
        var response = await _httpClient.GetAsync($"boards/boards/{token}");

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return new BoardApiResponse
            {
                Boards = JsonConvert.DeserializeObject<List<BoardModel>>(content)!
            };
        }

        return new BoardApiResponse
        {
            Success = false,
            Message = await response.Content.ReadAsStringAsync()
        };
    }
}