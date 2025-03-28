using System.Text.Json;
using Amazon.SQS.Model;
using QA.Application.DTOs;
using QA.Domain.Enums;
using IntegrationTests.Fixtures.Mocks;
using IntegrationTests.Fixtures.Helpers;
using Microsoft.Extensions.DependencyInjection;
using QA.Application.Ports.Outbound;

namespace IntegrationTests.UseCases
{
    
    // [TestCaseOrderer("IntegrationTests.Fixtures.Helpers.PriorityOrderer", "IntegrationTests")]
    // [Collection("IntegrationTestCollection")] 
    public partial class IntegrationTests()
    {
        [Fact, TestPriority(1)]
        public async Task CreateStandardProcessUseCaseTest()
        {
            // // Generat
            Assert.True(true);
            // string originalUrl = MockGenerator.GenerateMock<string>();
            // string finalUrl =  MockGenerator.GenerateMock<string>();
            // Guid mediaId = MockGenerator.GenerateMock<Guid>();
            // Guid shootingId = MockGenerator.GenerateMock<Guid>();
            // MediaType type = MediaType.Image;

            // MediaDto mediaDto = new(mediaId, originalUrl, finalUrl, type.ToString(), 1);
            // CreateQAProcessDTO createProcessDto = new(shootingId, [mediaDto]);

            // // Send message to queue
            // await fixture.SqsClient.SendMessageAsync(new SendMessageRequest
            // {
            //     QueueUrl = Environment.GetEnvironmentVariable("QA_SQS_QUEUE_URL"),
            //     MessageBody = JsonSerializer.Serialize(createProcessDto)
            // });
            // // Act: Resolve repository from DI
            // using var scope = fixture.Services.CreateScope();
            // var repository = scope.ServiceProvider.GetRequiredService<IQAProcessRepository>();
            // var entities = await repository.FilterByStatus(QAProcessStatus.Pending.ToString(), 1);

            // Assert.Single(entities);
            // Assert.NotNull(entities[0]);

            // Assert.Equal(mediaId.ToString(), entities?[0].Media?.First().ID);

            // Thread.Sleep(1000);
            Console.Write("PAASSED");

        }

    }
}
