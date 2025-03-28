using Amazon.DynamoDBv2.Model;
using QA.Application.Mappers.Persistence;
using QA.Application.Ports.Inbound;
using QA.Application.Ports.Outbound;
using QA.Domain.Entities;
using QA.Infrastructure.Persistence.Models;

namespace Qa.Application.UseCases
{
    public class ProcessFindUseCase(
        IQAProcessRepository repository
         ) : IQAProcessFinder
    {
        public async Task<QAProcess> Find(Guid processId)
        {
            try
            {
                
                QAProcessModel qaProcessModel = await repository.GetById(processId) 
                ?? throw new ResourceNotFoundException("Resource with id " + processId + "not found in DB");

                return QAProcessMapper.FromDbModel(qaProcessModel);

            }
            catch (Exception)
            {
                throw;
            }
           
        }
    }
}
