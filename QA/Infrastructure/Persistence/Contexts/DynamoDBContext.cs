using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

namespace Qa.Infrastructure.Persistence.Contexts
{
    public class DynamoDbContext(IAmazonDynamoDB dynamoDbClient)
    {
        public static readonly Dictionary<string, string> GSI = new()
        {
            ["Status"] = "status-priorityDate-index",
            ["User"] = "userId-status-index" 
        };

        private readonly DynamoDBContext DbContext = new(dynamoDbClient);

        public DynamoDBContext GetContext() => DbContext;

        public IAmazonDynamoDB GetClient() => dynamoDbClient;
    }
}
