using System.Text.Json.Serialization;

namespace DaddyJokes.Data
{
    public class DaddyJokesOut
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("joke")]
        public string Joke { get; set; }
    }
}
