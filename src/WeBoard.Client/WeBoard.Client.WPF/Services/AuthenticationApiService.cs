using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using WeBoard.Client.WPF.Requests;
using WeBoard.Client.WPF.Requests.Authentication;
using WeBoard.Client.WPF.Responses;

namespace WeBoard.Client.WPF.Services
{
    public class AuthenticationApiService
    {
        private readonly HttpClient _httpClient;
        public string? Id { get; private set; }

        public AuthenticationApiService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("http://3.98.122.179:5005") };
        }

        public async Task<AuthenticationApiResponse> RegisterAsync(RegisterRequest registerRequest)
        {
            var content = new StringContent(JsonConvert.SerializeObject(registerRequest), Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync("users", content);

            if (response.IsSuccessStatusCode)
                return new AuthenticationApiResponse();
            return new AuthenticationApiResponse()
            {
                Success = false,
                Message = await response.Content.ReadAsStringAsync()
            };
        }

        public async Task<AuthenticationApiResponse> LoginAsync(LoginRequest loginRequest)
        {
            var content = new StringContent(JsonConvert.SerializeObject(loginRequest), Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync("users/login", content);

            if (response.IsSuccessStatusCode)
            {
                var idString = await response.Content.ReadAsStringAsync();
                idString = idString.Trim('"');
                if (Guid.TryParse(idString, out var id))
                {
                    return new AuthenticationApiResponse()
                    {
                        Id = id
                    };
                }

            }

            return new AuthenticationApiResponse()
            {
                Success = false,
                Message = await response.Content.ReadAsStringAsync()
            };
        }
    }
}
