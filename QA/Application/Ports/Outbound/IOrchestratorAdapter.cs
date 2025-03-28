using QA.Domain.Entities;

namespace QA.Application.Ports.Outbound
{
    public interface IOrchestratorAdapter
    {
        Task Publish(QAProcess process);
    }
}
