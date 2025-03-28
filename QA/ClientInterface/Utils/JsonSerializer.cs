using System.Text.Json;

namespace QA.ClientInterface.Utils
{
    class Serializer
    {
        public static T Deserialize<T>(string info)
        {
           return JsonSerializer.Deserialize<T>(info) 
           ?? throw new InvalidOperationException($"Failed to deserialize message body: {info}");
        }

        public static string Serialize<T>(T info)
        {
           return JsonSerializer.Serialize(info) 
           ?? throw new InvalidOperationException($"Failed to deserialize message body: {info}");
        }
    }
}