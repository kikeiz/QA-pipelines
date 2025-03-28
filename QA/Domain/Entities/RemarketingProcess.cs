namespace QA.Domain.Entities
{
    using System.Text.Json.Serialization;
    using QA.Domain.Enums;
    using QA.Domain.ValueObjects;

    public class RemarketingProcess(
        Shooting shooting,
        RemarketingProcessingInfo processingInfo,
        Guid? ID = null
    )
    {
        public Guid ID { get; set;} = ID ?? Guid.NewGuid();
        [JsonInclude] public Shooting Shooting { get; set;} = shooting;
        [JsonInclude] public RemarketingProcessingInfo ProcessingInfo { get; set;} = processingInfo;

    }
}
