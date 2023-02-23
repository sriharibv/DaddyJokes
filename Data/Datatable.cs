using System;
using System.Text.Json.Serialization;

namespace DaddyJokes.Data
{
    public class Datatable<T>
    {

        [JsonPropertyName("current_page")]
        public int PageNumber { get; set; }

        [JsonPropertyName("results")]
        public IList<T> Items { get; set; }

        [JsonPropertyName("total_jokes")]
        public int TotalItems { get; set; }

        [JsonPropertyName("total_pages")]
        public int TotalPages { get; set; }
    }
}

