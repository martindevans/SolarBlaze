using System.Text.Json.Serialization;

namespace SolarBlaze.Services.MqttSqliteApi
{
    public class TopicContainer
    {
        public Dictionary<string, DataPoint> Topics { get; set; } = null!;

        public DataPoint? TryGetTopic(Topic topic)
        {
            var names = topic.TopicNames().ToList();
            if (names.Count != 1)
                throw new ArgumentException("Must request exactly one topic", nameof(topic));

            if (!Topics.TryGetValue(names[0], out var data))
                return null;
            return data;
        }
    }
}
