using QA.Application.DTOs;
using QA.Domain.Enums;

namespace QA.Application.Ports.Outbound
{
    public class CorporationInfoResponse
    {
        public Guid CorporationId  { get; set; }
        public bool SelfQA { get; set; }
        public BillingPlan BillingPlan { get; set; }

    }
    public interface ICorporationInfoAdapter
    {
        Task<CorporationInfoResponse> GetInfo(Guid shootingId);
    }
}
