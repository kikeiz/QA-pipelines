using QA.Domain.Entities;
using QA.Domain.Enums;
using QA.Domain.ValueObjects;

namespace QA.Application.Ports.Outbound
{
    public class ViewConfig
    {
        public int Position { get; set; }
        public bool Crop { get; set; }
        public bool AddShadow { get; set; }
        public bool DesaturateYellow { get; set; }
        public bool Enhance { get; set; }
        public bool AddLpc { get; set; }
        public bool AddLogo { get; set; }
        public bool AddStamp { get; set; }
        public bool CorrectRotation { get; set; }
        public bool Center { get; set; }
        public double? ZoomPercentage { get; set; }
        public double? HorizontalLineHeight { get; set; }
        public Margins? Margins { get; set; }
        public VehicleViews View { get; set; }
        public ImageTypes ImageType { get; set; }
        public List<HotSpot>? Hotspots { get; set; }
    }

    public class ProcessingInfoResponse
    {
        public required RemarketingProcessingInfo GeneralConfiguration { get; set; }
        public required List<ViewConfig> ViewConfiguration { get; set; }
    }

    public interface IProcessingInfoAdater
    {
        Task<ProcessingInfoResponse> GetInfo(Guid shootingId);
    }
}
