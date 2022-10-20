using System.Text.Json.Serialization;

namespace SolarBlaze.Services.MqttSqliteApi
{
    public struct DataPoint
    {
        [JsonPropertyName("timestamp")]
        public long TimeStamp { get; set; }

        [JsonPropertyName("payload")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public double Value { get; set; }
    }
}
