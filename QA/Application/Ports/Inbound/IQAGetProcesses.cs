using QA.Application.DTOs;
using QA.Domain.Entities;
using QA.Domain.Enums;

namespace QA.Application.Ports.Inbound
{
    public interface IQAGetProcesses
    {
        Task<GetQAProcessesResultDto> GetProcesses(Guid userId);
    }
}
