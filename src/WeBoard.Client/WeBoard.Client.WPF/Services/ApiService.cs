using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using WeBoard.Client.WPF.Requests;
using WeBoard.Client.WPF.Responses;

namespace WeBoard.Client.WPF.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        public string? Id { get; private set; }

        public ApiService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("http://3.98.122.179:5005") };
        }

        public async Task<ApiResponse> RegisterAsync(RegisterRequest registerRequest)
        {
            var content = new StringContent(JsonConvert.SerializeObject(registerRequest), Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync("users", content);

            if (response.IsSuccessStatusCode)
                return new ApiResponse();
            return new ApiResponse()
            {
                Success = false,
                Message = await response.Content.ReadAsStringAsync()
            };
        }

        public async Task<ApiResponse> LoginAsync(LoginRequest loginRequest)
        {
            var content = new StringContent(JsonConvert.SerializeObject(loginRequest), Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync("users/login", content);

            if (response.IsSuccessStatusCode)
            {
                return new ApiResponse()
                {
                    Id = await response.Content.ReadAsStringAsync()
                };
            }

            return new ApiResponse()
            {
                Success = false,
                Message = await response.Content.ReadAsStringAsync()
            };
        }
    }
}
