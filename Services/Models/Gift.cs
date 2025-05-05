using System.Text.Json.Serialization;

namespace Services.Models
{
    public class Gift
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("data")]
        public GiftData Data { get; set; }
    }

    public class GiftData
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("color")]
        public string Color { get; set; }
    }
}
