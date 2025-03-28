using Amazon.SQS;
using QA.Application.DTOs;
using QA.Application.Ports.Inbound;
using QA.ClientInterface.Interfaces;
using QA.ClientInterface.Listeners;

namespace QA.ClientInterface
{
    public class MaestroQueueAdapter(
        IAmazonSQS sqsClient,
        ILogger<MaestroQueueAdapter> logger,
        IServiceProvider serviceProvider // Inject IServiceProvider for scoped resolution
    ) : SqsListener<CreateQAProcessDTO>(
        sqsClient,
        logger,
        Environment.GetEnvironmentVariable("QA_SQS_QUEUE_URL")!,
        waitTimeSeconds: 5,
        maxNumberOfMessages: 10
    ), IProcessMessage<CreateQAProcessDTO>
    {
        public override async Task ProcessMessage(CreateQAProcessDTO body, CancellationToken stoppingToken)
        {
            // Create a new scope to resolve scoped services
            using IServiceScope scope = serviceProvider.CreateScope();
            var createQAProcess = scope.ServiceProvider.GetRequiredService<IQAProcessCreator<CreateQAProcessDTO>>();

            Logger.LogInformation("Processing message in Adapter: {MessageBody}", body);
            await createQAProcess.CreateProcess(body);
        }
    }
}
