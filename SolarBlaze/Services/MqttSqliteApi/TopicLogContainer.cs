using System.Text.Json.Serialization;

namespace SolarBlaze.Services.MqttSqliteApi
{
    public class TopicLogContainer
    {
        [JsonPropertyName("topics")]
        public Dictionary<string, DataPoint[]> Topics { get; set; } = null!;

        [JsonPropertyName("limit")]
        public int Limit { get; set; }

        public DataPoint[]? TryGetTopic(Topic topic)
        {
            var names = topic.TopicNames().ToList();
            if (names.Count != 1)
                throw new ArgumentException("Must request exactly one topic", nameof(topic));

            Topics.TryGetValue(names[0], out var data);
            return data;
        }
    }
}
