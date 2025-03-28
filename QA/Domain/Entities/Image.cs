namespace QA.Domain.Entities
{
    using System.Text.Json.Serialization;
    using QA.Domain.Enums;
    using QA.Domain.ValueObjects;

    public class Image(
        Guid Id,
        string OriginalUrl,
        string FinalUrl,
        Guid ShootingId,
        int Position,
        VehicleViews View,
        List<HotSpot> Hotspots,
        ImageTypes Type,
        ImageProcessingInfo ProcessingInfo,
        bool ManuallyProcessed = false
    ) : Media(OriginalUrl, FinalUrl, ShootingId, Id, View, Position, MediaType.Image, ManuallyProcessed)
    {
        public List<HotSpot> Hotspots { get; private set; } = Hotspots;
        public int HorizontalLineHeight { get; private set; }
        public VehicleViews? ImageView { get; private set; } = View;
        public ImageTypes Type { get; private set; } = Type;
        [JsonInclude] public override ImageProcessingInfo ProcessingInfo { get; } = ProcessingInfo;

        public override string? RepushedUrl { get; protected set; }

        public void SetRepushedUrl(string repushedUrl)
        {
            RepushedUrl = repushedUrl;
        }

        public ImageTypes SetImageType(ImageTypes imageType)
        {
            Type = imageType;
            return Type;
        }

        public List<HotSpot> AddHotspots(List<HotSpot> hotspots)
        {
            Hotspots.AddRange(hotspots);
            return Hotspots;
        }

        public List<HotSpot> UpdateHotspot(string id, HotSpot newHotspot)
        {
            Hotspots = Hotspots.ConvertAll((HotSpot hotspot) => hotspot.Id == id ? newHotspot : hotspot);
            return Hotspots;
        }

        public List<HotSpot> RemoveHotspot(string id)
        {
            Hotspots = Hotspots.FindAll((HotSpot hotSpot) => hotSpot.Id != id);
            return Hotspots;
        }

        public void SetViewAndType(ImageTypes newImageType)
        {
            Type = newImageType;
            var exteriorImageTypes = new HashSet<ImageTypes>
            {
                ImageTypes.East,
                ImageTypes.North,
                ImageTypes.West,
                ImageTypes.South,
                ImageTypes.NorthEast,
                ImageTypes.NorthWest,
                ImageTypes.SouthWest,
                ImageTypes.SouthEast,
                ImageTypes.Wheel
            };

            ImageView = exteriorImageTypes.Contains(newImageType)
                ? VehicleViews.Exterior
                : VehicleViews.Interior;
        }
    }
}
