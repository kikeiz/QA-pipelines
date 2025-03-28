using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using QA.ClientInterface.Utils;
using System.Text.Json;

namespace QA.ClientInterface.Listeners
{
    public abstract class SqsListener<T>(
        IAmazonSQS sqsClient,
        ILogger<SqsListener<T>> logger,
        string queueUrl,
        int waitTimeSeconds = 5,
        int maxNumberOfMessages = 10
    ) : BackgroundService
    {
        protected readonly IAmazonSQS SqsClient = sqsClient;
        protected readonly ILogger<SqsListener<T>> Logger = logger;
        protected readonly string QueueUrl = queueUrl;
        protected readonly int WaitTimeSeconds = waitTimeSeconds;
        protected readonly int MaxNumberOfMessages = maxNumberOfMessages;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Logger.LogInformation("Starting SQS Listener for queue: {QueueUrl}", QueueUrl);

            while (!stoppingToken.IsCancellationRequested)
            {
                await ProcessBatchAsync(stoppingToken);
            }
        }

        private async Task ProcessBatchAsync(CancellationToken stoppingToken)
        {
            try
            {
                // Fetch messages from the queue
                var response = await ReceiveMessagesAsync(stoppingToken);

                if (response.Messages.Count == 0)
                {
                    Logger.LogInformation("No messages received. Retrying in {WaitTimeSeconds} seconds.", WaitTimeSeconds);
                    return;
                }
                Logger.LogInformation("Count of received {Count}", response.Messages.Count);

                // Process all messages in parallel
                var processingTasks = response.Messages.Select(message => ProcessMessageWithDeletionAsync(message, stoppingToken)).ToArray();
                await Task.WhenAll(processingTasks);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An error occurred while processing SQS messages.");
                await Task.Delay(5000, stoppingToken);
            }
        }

        private async Task<ReceiveMessageResponse> ReceiveMessagesAsync(CancellationToken stoppingToken)
        {
            return await SqsClient.ReceiveMessageAsync(new ReceiveMessageRequest
            {
                QueueUrl = QueueUrl,
                MaxNumberOfMessages = MaxNumberOfMessages,
                WaitTimeSeconds = WaitTimeSeconds
            }, stoppingToken);
        }

        private async Task ProcessMessageWithDeletionAsync(Message message, CancellationToken stoppingToken)
        {
            try
            {
                T deserializedMessage = Serializer.Deserialize<T>(message.Body);
                await ProcessMessage(deserializedMessage, stoppingToken);
                await DeleteMessageAsync(message, stoppingToken);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to process message: {MessageId}", message.MessageId);
            }
        }

        private async Task DeleteMessageAsync(Message message, CancellationToken stoppingToken)
        {
            try
            {
                await SqsClient.DeleteMessageAsync(QueueUrl, message.ReceiptHandle, stoppingToken);
                Logger.LogInformation("Message deleted: {MessageId}", message.MessageId);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to delete message: {MessageId}", message.MessageId);
            }
        }

        // Abstract method to be implemented by child classes
        public abstract Task ProcessMessage(T body, CancellationToken stoppingToken);
    }
}
