using Amazon.DynamoDBv2.DataModel;
using QA.Domain.Enums;
using QA.Domain.ValueObjects;
using MediaType = QA.Domain.Enums.MediaType;

namespace QA.Infrastructure.Persistence.Models
{
    public class RemarketingProcessingInfoModel
    {
        [DynamoDBProperty("backgroundType")]
        public string BackgroundTypeString
        {
            get => BackgroundType.ToString();
            set => BackgroundType = Enum.Parse<BackgroundType>(value);
        }

        [DynamoDBIgnore]
        public BackgroundType BackgroundType { get; set; }

        [DynamoDBProperty("exteriorCroppingProvider")]
        public string ExteriorCroppingProviderString
        {
            get => ExteriorCroppingProvider.ToString();
            set => ExteriorCroppingProvider = Enum.Parse<CroppingProvider>(value);
        }

        [DynamoDBIgnore]
        public CroppingProvider ExteriorCroppingProvider { get; set; }

        [DynamoDBProperty("interiorCroppingProvider")]
        public string InteriorCroppingProviderString
        {
            get => InteriorCroppingProvider.ToString();
            set => InteriorCroppingProvider = Enum.Parse<CroppingProvider>(value);
        }

        [DynamoDBIgnore]
        public CroppingProvider InteriorCroppingProvider { get; set; }

        [DynamoDBProperty("backgroundUrl")]
        public string? BackgroundUrl { get; set; }

        [DynamoDBProperty("logoUrl")]
        public string? LogoUrl { get; set; }

        [DynamoDBProperty("stampUrl")]
        public string? StamUrl { get; set; }

        [DynamoDBProperty("lpcUrl")]
        public string? LpcUrl { get; set; }

        [DynamoDBProperty("floorUrl")]
        public string? FloorUrl { get; set; }

        [DynamoDBProperty("blurringValue")]
        public int? BlurringVBalue { get; set; }

        [DynamoDBProperty("outputDimensions")]
        public OutputDimensionsModel? OutputDimensions { get; set; }
    }

    public class OutputDimensionsModel
    {
        [DynamoDBProperty("outputWidth")]
        public int? OutputWidth { get; set; }

        [DynamoDBProperty("outputHeight")]
        public int? OutputHeight { get; set; }
    }

}
