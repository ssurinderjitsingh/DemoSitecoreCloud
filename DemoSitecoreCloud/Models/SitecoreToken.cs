using Newtonsoft.Json;

namespace DemoSitecoreCloud.Models
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
}