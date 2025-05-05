using System.Text.Json.Serialization;

namespace Services.Models
{
    public class Gift
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("data")]
        public GiftData Data { get; set; } = new GiftData();
    }

    public class GiftData
    {
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("color")]
        public string Color { get; set; } = string.Empty;
    }
}