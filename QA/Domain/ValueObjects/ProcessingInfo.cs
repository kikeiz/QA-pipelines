namespace QA.Domain.ValueObjects 
{
    using System.Text.Json.Serialization;
    using QA.Domain.Enums;

    public class RemarketingProcessingInfo
    {
        public BackgroundType BackgroundType { get; set ;}
        public CroppingProvider ExteriorCroppingProvider { get; set ;}
        public CroppingProvider InteriorCroppingProvider { get; set ;}
        public required string BackgroundUrl { get; set ;}
        public required string LogoUrl { get; set ;}
        public required string StampUrl { get; set ;}
        public required string LpcUrl { get; set ;}
        public required string FloorUrl { get; set ;}
        public required OutputDimensions OutputDimensions { get; set ;}
        public double BlurringValue { get; set ;}
    }

    [JsonDerivedType(typeof(ImageProcessingInfo), typeDiscriminator: "imageProcessingInfo")]
    [JsonDerivedType(typeof(VideoProcessingInfo), typeDiscriminator: "videoProcessingInfo")]
    public class MediaProcessingInfo;

    public class ImageProcessingInfo(
        bool Crop,
        bool DesaturateYellow,
        bool Enhance,
        bool AddLogo,
        bool AddLpc,
        bool AddStamp,
        CroppingProvider CroppingProvider,
        bool AddShadow,
        bool CorrectRotation,
        bool Center,
        double ZoomPercentage,
        double? HorizontalLineHeight = null,
        Margins? Margins = null) : MediaProcessingInfo
    {
        [JsonInclude] public bool Crop { get; } = Crop;
        [JsonInclude] public bool DesaturateYellow { get; } = DesaturateYellow;
        [JsonInclude] public bool Enhance { get; } = Enhance;
        [JsonInclude] public bool AddLogo { get; } = AddLogo;
        [JsonInclude] public bool AddLpc { get; } = AddLpc;
        [JsonInclude] public bool AddStamp { get; } = AddStamp;
        [JsonInclude] public CroppingProvider CroppingProvider { get; } = CroppingProvider;
        [JsonInclude] public bool AddShadow { get; } = AddShadow;
        [JsonInclude] public bool CorrectRotation { get; } = CorrectRotation;
        [JsonInclude] public bool Center { get; } = Center;
        [JsonInclude] public double ZoomPercentage { get; } = ZoomPercentage;
        [JsonInclude] public double? HorizontalLineHeight { get; } = HorizontalLineHeight;
        [JsonInclude] public Margins? Margins { get; } = Margins;

    }

    public class VideoProcessingInfo : MediaProcessingInfo
    {
        public bool Crop { get; }
    }

}

