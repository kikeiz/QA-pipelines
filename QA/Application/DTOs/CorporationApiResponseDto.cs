using System.Text.Json.Serialization;

namespace QA.Application.DTOs
{
    public class CorporationApiResponse
    {
        [JsonPropertyName("corporationId")]
        public Guid CorporationId { get; set; }

        [JsonPropertyName("selfQA")]
        public bool SelfQA { get; set; }
        
        [JsonPropertyName("billingPlan")]
        public required string BillingPlan { get; set; }
    }
}

