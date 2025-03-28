using PactNet.Verifier;
using PactNet.Infrastructure.Outputters;
using PactNet;
using IntegrationTests.Fixtures.Helpers;
using IntegrationTests.Fixtures.Mocks;
using Microsoft.Extensions.DependencyInjection;
using QA.Application.Ports.Outbound;

namespace IntegrationTests.ContractTesting
{
    [TestCaseOrderer("IntegrationTests.Fixtures.Helpers.PriorityOrderer", "IntegrationTests")]
    [Collection("IntegrationTestCollection")]
    public partial class IntegrationTests(TestAppFixture fixture)
        {
        [Fact, TestPriority(2)]
        public async Task Verify_MyService_Pact_Is_Honored()
        {
            Console.WriteLine("🔄 Running Pact Provider Verification against Pact Broker...");


            using var scope = fixture.Services.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IQAProcessRepository>();
            
            var process = QaProcessMockGenerator.GenerateMockProcess("123e4567-e89b-12d3-a456-426614174000");
            await repository.Save(process);

            var foundProcess = await repository.GetById(process.ProcessId) ?? throw new Exception("❌ Process not found!");
            Console.WriteLine($"✅ Process found: {foundProcess.ProcessId}");

            var config = new PactVerifierConfig
            {
                LogLevel = PactLogLevel.Error,
                Outputters = [new ConsoleOutput()]
            };

            using var pactVerifier = new PactVerifier("QaServiceBack", config);

            pactVerifier
                .WithHttpEndpoint(fixture.ApiBaseUri)
                .WithPactBrokerSource(fixture.PactBrokerUri)
                .Verify();

            Console.WriteLine("✅ Pact verification completed!");
        }
    }
}
