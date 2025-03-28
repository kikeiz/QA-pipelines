using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace IntegrationTests.Fixtures.AWS
{
    class DynamoDbFixturesGenerator(IAmazonDynamoDB dynamoDbClient)
    {
        public async Task CreateTable(
            string tableName,
            List<KeySchemaElement> keySchema,
            List<AttributeDefinition> attributeDefinitions,
            ProvisionedThroughput provisionedThroughput,
            List<GlobalSecondaryIndex>? gsi = null)
        {

            ListTablesResponse existingTables = await dynamoDbClient.ListTablesAsync();
            if (existingTables.TableNames.Contains(tableName))
            {
                Console.WriteLine($"✅ Table '{tableName}' already exists.");
                return;
            }
            CreateTableRequest request = new ()
            {
                TableName = tableName,
                KeySchema = keySchema,
                AttributeDefinitions = attributeDefinitions,
                ProvisionedThroughput = provisionedThroughput, 
                GlobalSecondaryIndexes = gsi?.Count > 0 ? gsi : null
            };

            await dynamoDbClient.CreateTableAsync(request);
        }

        public async Task SeedTableAsync(string tableName, List<Dictionary<string, AttributeValue>> items)
        {
            var tasks = items.Select(item => dynamoDbClient.PutItemAsync(
                new PutItemRequest
                {
                    TableName = tableName,
                    Item = item
                }
            ));
            
            await Task.WhenAll(tasks);
        }
    }
}
