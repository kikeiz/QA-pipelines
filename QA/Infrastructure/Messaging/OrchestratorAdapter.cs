using Amazon.SQS;
using Amazon.SQS.Model;
using QA.Application.Ports.Outbound;
using QA.Domain.Entities;
using System.Text.Json;

namespace QA.Infrastructure.Messaging
{
    public class OrchestratorAdapter(IAmazonSQS sqsClient) : IOrchestratorAdapter
    {
        private readonly string _queueUrl = Environment.GetEnvironmentVariable("REMARKETING_SERVICE_QUEUE_URL")!;

        public async Task Publish(QAProcess process)
        {
            var messageBody = JsonSerializer.Serialize(new { Status = process.Status.ToString() });
            var request = new SendMessageRequest
            {
                QueueUrl = _queueUrl,
                MessageBody = messageBody
            };

            await sqsClient.SendMessageAsync(request);
        }
    }
}
