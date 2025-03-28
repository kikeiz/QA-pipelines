using QA.Application.DTOs;
using QA.Domain.Enums;
using QA.Infrastructure.Persistence.Models;

namespace QA.Application.Ports.Outbound
{
    public interface IQAProcessRepository
    {
        Task Save(QAProcessModel qaProcessModel);
        Task<QAProcessModel?> GetById(Guid processId);
        Task<List<QAProcessModel>> FilterByStatus(string status, int limit);
        Task Delete(Guid processId);
        Task<List<QAProcessModel>> FilterByUserId(Guid userId, int limit);
        Task<int> GetTotalAmountOfProcesses(string status);
    }
}
