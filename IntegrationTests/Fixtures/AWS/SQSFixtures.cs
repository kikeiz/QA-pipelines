using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace IntegrationTests.Fixtures.AWS
{
    class SQSFixturesGenerator(IAmazonSQS sqsClient)
    {
       public  async Task<string> CreateQueue(
            string queueName
        )
        {
            var listQueuesResponse = await sqsClient.ListQueuesAsync(new ListQueuesRequest());
            if (listQueuesResponse.QueueUrls.Exists(url => url.EndsWith(queueName)))
            {
                Console.WriteLine($"✅ Queue '{queueName}' already exists.");
                return listQueuesResponse.QueueUrls.Find(url => url.EndsWith(queueName))!;
            }

            var createQueueResponse = await sqsClient.CreateQueueAsync(new CreateQueueRequest { QueueName = queueName });
            Console.WriteLine($"✅ SQS Queue '{queueName}' created successfully!");

            return createQueueResponse.QueueUrl;
        }
    }
}
