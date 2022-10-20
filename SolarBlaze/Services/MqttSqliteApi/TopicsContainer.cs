using System.Text.Json.Serialization;

namespace SolarBlaze.Services.MqttSqliteApi
{
    public class TopicsContainer
    {
        [JsonPropertyName("topics")]
        public Dictionary<string, DataPoint[]> Topics { get; set; } = null!;

        [JsonPropertyName("limit")]
        public int Limit { get; set; }
    }
}
