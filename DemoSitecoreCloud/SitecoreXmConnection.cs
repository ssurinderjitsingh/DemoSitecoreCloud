using Newtonsoft.Json;

namespace DemoSitecoreCloud
{
    /// <summary>
    /// Model for Sitecore token response.
    /// </summary>
    public class SitecoreToken
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonProperty("expires_in")]
        public string ExpiresIn { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }
    }

    /// <summary>
    /// Handles authentication with Sitecore XM Cloud to retrieve an access token.
    /// </summary>
    public class SitecoreXmConnection
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://hostname-sitecorecloud.io";
        private const string TokenEndpoint = "/api/authoring/graphql/v1/oauth/token";

        private readonly string _clientId = "543k3jl534oi5lk3j54";
        private readonly string _clientSecret = "89dsfgfdg9gfdsfdgs0fd897bgsdfgslkrw8e7r9er8we7";
        private readonly string _audience = "http://api/.sitecorcloud.io";

        public SitecoreXmConnection(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<string> GetAuthTokenAsync()
        {
            var requestUrl = $"{BaseUrl}{TokenEndpoint}";
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