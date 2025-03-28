using System.Text.Json.Serialization;

namespace QA.Domain.Entities
{

    public class Shooting(
        Guid ID,
        List<Media> media
    )
    {
        public Guid ID { get; } = ID;
        [JsonInclude] public List<Media> Media { get; } = media;
        private int MediaCount { get; } = media.Count;

        public int GetLength()
        {
            return MediaCount;
        }

    }
}
