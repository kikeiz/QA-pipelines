using Amazon.DynamoDBv2.DataModel;
using QA.Infrastructure.Persistence.Models;
using Qa.Infrastructure.Persistence.Contexts;
using QA.Application.Ports.Outbound;
using Amazon.DynamoDBv2.DocumentModel;
using QA.Infrastructure.Persistence.Contexts.Resolvers.Interfaces;
using Amazon.DynamoDBv2.Model;

namespace QA.Infrastructure.Persistence.Repositories
{
    public class QAProcessRepository(DynamoDbContext context, ITableNameResolver tableNameResolver) : IQAProcessRepository
    {
        private readonly DynamoDBContext _dbContext = context.GetContext();

        private DynamoDBOperationConfig OperationConfig => new()
        {
            OverrideTableName = tableNameResolver.Resolve<QAProcessModel>()
        };

        public async Task Save(QAProcessModel qaProcessModel)
        {
            try
            {
                await _dbContext.SaveAsync(qaProcessModel, OperationConfig);
                Console.WriteLine("QA Process saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving QA Process: {ex.Message}");
                throw;
            }
        }

        public async Task<QAProcessModel?> GetById(Guid processId)
        {
            try
            {
                return await _dbContext.LoadAsync<QAProcessModel>(processId, OperationConfig);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving QA Process: {ex.Message}");
                throw;
            }
        }

        public async Task Delete(Guid processId)
        {
            try
            {
                await _dbContext.DeleteAsync<QAProcessModel>(processId, OperationConfig);
                Console.WriteLine("QA Process deleted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting QA Process: {ex.Message}");
                throw;
            }
        }
        public async Task<List<QAProcessModel>> FilterByUserId(Guid userId, int limit)
        {
            try
            {
                var query = new QueryOperationConfig
                {
                    IndexName = DynamoDbContext.GSI["User"],
                    KeyExpression = new Expression
                    {
                        ExpressionStatement = "#userId = :userIdVal",
                        ExpressionAttributeNames = new()
                        {
                            { "#userId", "userId" }
                        },
                        ExpressionAttributeValues = new()
                        {
                            { ":userIdVal", userId }
                        }
                    },
                    BackwardSearch = false,
                    ConsistentRead = false,
                    Limit = limit
                };
                var search = _dbContext.FromQueryAsync<QAProcessModel>(query, OperationConfig);
                var results = await search.GetNextSetAsync();

                return [.. results.Take(limit)];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error querying QA Process by user: {ex.Message}");
                throw;
            }
        }

        public async Task<List<QAProcessModel>> FilterByStatus(string status, int limit)
        {
            try
            {
                var query = new QueryOperationConfig
                {
                    IndexName = DynamoDbContext.GSI["Status"],
                    KeyExpression = new Expression
                    {
                        ExpressionStatement = "#status = :statusVal AND priorityDate > :minPriorityDate",
                        ExpressionAttributeNames = new()
                        {
                            { "#status", "status" }
                        },
                        ExpressionAttributeValues = new()
                        {
                            { ":statusVal", status },
                            { ":minPriorityDate", 0 }
                        }
                    },
                    BackwardSearch = false,
                    ConsistentRead = false,
                    Limit = limit
                };

                var search = _dbContext.FromQueryAsync<QAProcessModel>(query, OperationConfig);
                var results = await search.GetNextSetAsync();

                return [.. results.Take(limit)];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error querying QA Process by status: {ex.Message}");
                throw;
            }
        }

        public async Task<int> GetTotalAmountOfProcesses(string status)
        {
            try
            {
                var tableName = tableNameResolver.Resolve<QAProcessModel>();


                var request = new QueryRequest
                {
                    TableName = tableName,
                    IndexName = DynamoDbContext.GSI["Status"],
                    KeyConditionExpression = "#status = :pending",
                    ExpressionAttributeNames = new Dictionary<string, string>
                    {
                        { "#status", "status" }
                    },
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                    {
                        { ":pending", new AttributeValue { S = "Pending" } }
                    },
                    Select = "COUNT",
                    ConsistentRead = false
                };

                int totalCount = 0;
                Dictionary<string, AttributeValue>? lastKey = null;

                do
                {
                    request.ExclusiveStartKey = lastKey;
                    var response = await context.GetClient().QueryAsync(request);
                    totalCount += response.Count;
                    lastKey = response.LastEvaluatedKey;
                } while (lastKey != null && lastKey.Count > 0);

                return totalCount;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error counting Pending QA Processes: {ex.Message}");
                throw;
            }
        }
    }
}
