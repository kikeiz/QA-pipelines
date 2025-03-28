namespace QA.Domain.Entities
{
    using System.Text.Json.Serialization;
    using QA.Domain.Enums;
    using QA.Domain.ValueObjects;

    public abstract class Media(
        string OriginalUrl,
        string FinalUrl,
        Guid ShootingId,
        Guid Id,
        VehicleViews View,
        int Position,
        MediaType MediaType,
        bool ManuallyProcessed
    )
    {
        public int Position { get; private set; } = Position;
        public string OriginalUrl { get; protected set; } = OriginalUrl;
        public string FinalUrl { get; protected set; } = FinalUrl;
        public Guid ShootingId { get; } = ShootingId;
        public bool ManuallyProcessed { get; } = ManuallyProcessed;
        public Guid Id { get; } = Id;
        [JsonInclude] public abstract MediaProcessingInfo ProcessingInfo { get; }
        public abstract string? RepushedUrl { get; protected set; }

        public VehicleViews View { get; } = View;

        public MediaType MediaType { get; } = MediaType;

        public void UpdateFinalUrl(string url)
        {
            FinalUrl = url;
        }

        // public void MarkAsProcessed()
        // {
        //     MediaStatus = MediaStatuses.Processed;
        // }

        public void ConvertRepushedUrlToOptimized(string repushedUrl)
        {
            FinalUrl = repushedUrl;
        }

        // public abstract void FinishImageProcessing();
    
    }
}
