using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Bogus;
using IntegrationTests.Fixtures.AWS;
using QA.Application.DTOs;
using QA.Domain.Enums;

namespace IntegrationTests.Fixtures.Custom
{
    public class CustomInfrastructureConfigManager(IAmazonDynamoDB dynamoDbClient)
    {
        public async Task GenerateQaProcessTable()
        {
            DynamoDbFixturesGenerator dynamoFixturesGenerator = new (dynamoDbClient);

            await dynamoFixturesGenerator.CreateTable(
                "QaProcess",
                [
                    new ("processId", KeyType.HASH)
                ],
                [
                    new ("processId", ScalarAttributeType.S), 
                    new AttributeDefinition("userId", ScalarAttributeType.S),
                    new AttributeDefinition("priorityDate", ScalarAttributeType.N),
                    new AttributeDefinition("status", ScalarAttributeType.S),
                ],
                new (5,5),
                [
                    new GlobalSecondaryIndex
                    {
                        IndexName = "status-priorityDate-Index",
                        KeySchema = 
                        {
                            new ("status", KeyType.HASH),
                            new ("priorityDate", KeyType.RANGE)
                        },
                        Projection = new Projection { ProjectionType = ProjectionType.ALL },
                        ProvisionedThroughput = new ProvisionedThroughput(5, 5) 
                    },
                    new ()
                    {
                        IndexName = "userId-status-Index",
                        KeySchema = 
                        {
                            new KeySchemaElement("userId", KeyType.HASH),
                            new KeySchemaElement("status", KeyType.RANGE)
                        },
                        Projection = new Projection { ProjectionType = ProjectionType.ALL },
                        ProvisionedThroughput = new ProvisionedThroughput(5, 5)
                    }
                ]
            );
        }
    }
}
