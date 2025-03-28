using QA.Domain.Entities;

namespace QA.Application.Ports.Inbound
{
    public interface IQAProcessFinder
    {
        Task<QAProcess> Find(Guid processId);
    }
}
