using System.Text.Json.Serialization;

namespace BookStoreWebApp.DTOs
{
    public class RegisterRequest
    {
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("fullname")]
        public string Fullname { get; set; } = string.Empty;

        [JsonPropertyName("phone")]
        public string Phone { get; set; } = string.Empty;

        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;

        [JsonPropertyName("gender")]
        public bool Gender { get; set; }
    }
}
