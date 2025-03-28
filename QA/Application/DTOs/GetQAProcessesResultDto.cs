using System.Text.Json.Serialization;

namespace QA.Application.DTOs
{
    public record GetQAProcessesResultDto
    {
        [JsonPropertyName("processes")]
        public required List<QAProcessDto> Processes { get; set; }

        [JsonPropertyName("totalCount")]
        public int TotalCount { get; set; }
    }

    public record QAProcessDto(
        Guid ProcessId,
        Guid RemarketingId,
        Guid ShootingId,
        Guid CorporationId,
        string Status,
        bool SelfQA,
        string Priority,
        Guid? UserId,
        string BillingPlan,
        string Type
    );
}
