using Amazon.DynamoDBv2.DataModel;
using QA.Domain.Enums;
using QA.Domain.ValueObjects;
using MediaType = QA.Domain.Enums.MediaType;

namespace QA.Infrastructure.Persistence.Models
{
    public class MediaModel
    {
        [DynamoDBProperty("id")]
        public required string ID { get; set; }

        [DynamoDBProperty("url")]
        public required string Url { get; set; }

        [DynamoDBProperty("optimizedUrl")]
        public required string OptimizedUrl { get; set; }

        [DynamoDBProperty("repushedUrl")]
        public required string? RepushedUrl { get; set; }

        [DynamoDBProperty("position")]
        public required int Position { get; set; }

        [DynamoDBProperty("manuallyProcessed")]
        public required bool ManuallyProcessed { get; set; }

        [DynamoDBProperty("processingInfo")]
        public required MediaProcessingInfoModel ProcessingInfo { get; set; }

        [DynamoDBProperty("type")]
        public string TypeString
        {
            get => Type.ToString();
            set => Type = Enum.Parse<MediaType>(value);
        }

        [DynamoDBIgnore]
        public MediaType Type { get; set; }

        [DynamoDBProperty("vehicleView")]
        public string VehicleViewString
        {
            get => VehicleView.ToString();
            set => VehicleView = Enum.Parse<VehicleViews>(value);
        }

        [DynamoDBIgnore]
        public VehicleViews VehicleView { get; set; }

        [DynamoDBProperty("classification")]
        public string MediaClassificationString
        {
            get => MediaClassification.ToString();
            set => MediaClassification = Enum.Parse<ImageTypes>(value);
        }

        [DynamoDBIgnore]
        public ImageTypes MediaClassification { get; set; }
    }

    public class MarginsModel
    {
        [DynamoDBProperty("top")]
        public int? Top { get; set; }

        [DynamoDBProperty("left")]
        public int? Left { get; set; }

        [DynamoDBProperty("bottom")]
        public int? Bottom { get; set; }

        [DynamoDBProperty("right")]
        public int? Right { get; set; }
    }
}
