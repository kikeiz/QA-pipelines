using Amazon.DynamoDBv2.DataModel;
using QA.Domain.Enums;
using QA.Domain.ValueObjects;
using MediaType = QA.Domain.Enums.MediaType;

namespace QA.Infrastructure.Persistence.Models
{

    public class MediaProcessingInfoModel
    {
        [DynamoDBProperty("crop")]
        public required bool Crop { get; set; }

        [DynamoDBProperty("desaturateYellow")]
        public required bool DesaturateYellow { get; set; }

        [DynamoDBProperty("enhance")]
        public required bool Enhance { get; set; }

        [DynamoDBProperty("addLogo")]
        public required bool AddLogo { get; set; }

        [DynamoDBProperty("addLpc")]
        public required bool AddLpc { get; set; }

        [DynamoDBProperty("addStamp")]
        public bool AddStamp { get; set; }

        [DynamoDBProperty("addShadow")]
        public bool AddShadow { get; set; }

        [DynamoDBProperty("correctRotation")]
        public bool CorrectRotation { get; set; }

        [DynamoDBProperty("center")]
        public bool Center { get; set; }

        [DynamoDBProperty("zoomPercentage")]
        public double ZoomPercentage { get; set; }

        [DynamoDBProperty("horizontalLineHeight")]
        public double HorizontalLineHeight { get; set; }

        [DynamoDBProperty("croppingProvider")]
        public string CroppingProviderString
        {
            get => CroppingProvider.ToString();
            set => CroppingProvider = Enum.Parse<CroppingProvider>(value);
        }

        [DynamoDBIgnore]
        public CroppingProvider CroppingProvider { get; set; }

        [DynamoDBProperty("margins")]
        public MarginsModel? Margins { get; set; }

    }
}
