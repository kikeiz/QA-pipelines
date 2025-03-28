using Amazon.DynamoDBv2.Model;
using QA.Application.DTOs;
using QA.Application.Mappers.Persistence;
using QA.Application.Ports.Inbound;
using QA.Application.Ports.Outbound;
using QA.Domain.Enums;


namespace Qa.Application.UseCases
{
    public class GetProcessUseCase(
            IQAProcessRepository repository
             ) : IQAGetProcesses
    {
        public async Task<GetQAProcessesResultDto> GetProcesses(Guid userId)
        {
            try
            {
                int maxNumProcesses = int.Parse(Environment.GetEnvironmentVariable("MAX_NUM_PROCESSES")!);
                Console.WriteLine($"MAX_NUM_PROCESSES: {maxNumProcesses}");
                var userProcesses = await repository.FilterByUserId(userId, maxNumProcesses);
                var pendingProcesses = (userProcesses.Count < maxNumProcesses)
                    ? await repository.FilterByStatus(QAProcessStatus.Pending.ToString(), maxNumProcesses - userProcesses.Count)
                    : [];
                int allProcessesCount = await repository.GetTotalAmountOfProcesses(QAProcessStatus.Pending.ToString());

                var allProcesses = userProcesses
                        .Concat(pendingProcesses)
                        .Select(QAProcessMapper.FromDbModel)
                        .Select(QAProcessMapper.ToDto)
                        .ToList();

                return new GetQAProcessesResultDto {
                    Processes = allProcesses,
                    TotalCount = allProcessesCount
                };
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
