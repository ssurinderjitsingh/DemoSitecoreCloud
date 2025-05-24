using DemoSitecoreCloud.Interface;
using DemoSitecoreCloud.Models;
using Newtonsoft.Json;

namespace DemoSitecoreCloud
{

    /// <summary>
    /// Handles authentication with Sitecore XM Cloud to retrieve an access token.
    /// </summary>
    public class SitecoreConnection : IConnection
    {
        public string EndPoint { get; private set; }

        private readonly string _clientId, _clientSecret, _tokenUrl, _audience;

        private readonly HttpClient _httpClient;
        private string TokenEndPoint {  get; set; }

        public SitecoreConnection(string endpoint, string clientId, string clientSecret, string tokenUrl, string audience)
        {
            EndPoint = endpoint;
            _clientId = clientId;
            _clientSecret = clientSecret;
            _tokenUrl = tokenUrl;
            _audience = audience;
            _httpClient = new HttpClient();
        }

        public async Task<string> GetAccessTokenAsync()
        {
            var requestUrl = $"{EndPoint}{TokenEndPoint}";
            var formData = new Dictionary<string, string>
            {
                ["grant_type"] = "client_credentials",
                ["client_id"] = _clientId,
                ["client_secret"] = _clientSecret,
                ["audience"] = _audience
            };

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, requestUrl)
                {
                    Content = new FormUrlEncodedContent(formData)
                };

                var response = await _httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    Console.Error.WriteLine($"Failed to retrieve token. Status: {response.StatusCode}");
                    return string.Empty;
                }

                var json = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonConvert.DeserializeObject<SitecoreToken>(json);
                return tokenResponse?.AccessToken ?? string.Empty;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Exception occurred while fetching token: {ex.Message}");
                return string.Empty;
            }
        }
    }
}