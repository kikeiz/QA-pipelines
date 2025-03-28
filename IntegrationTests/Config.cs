using Amazon.DynamoDBv2;
using Amazon.SQS;
using IntegrationTestingBase.Containers.AWS;
using IntegrationTestingBase.Containers.API;
using IntegrationTestingBase.Containers.Registry;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QA.Infrastructure.ExternalServices;
using IntegrationTestingBase.Core.Interfaces;
using QA.Application.Ports.Outbound;
using IntegrationTestingBase.Containers;
using System.Text.Json;
using IntegrationTests.Fixtures.Custom;
using IntegrationTests.Fixtures.Mocks;
using IntegrationTests.Fixtures.AWS;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace IntegrationTests
{
    [CollectionDefinition("IntegrationTestCollection", DisableParallelization = true)]
    public class SharedTestCollection : ICollectionFixture<TestAppFixture> { }

    public class TestAppFixture : IDisposable
    {
        private readonly IHost _server;
        private readonly IContainerRegistry _registry;
        private bool _disposed = false;
        
        public Uri ApiBaseUri { get; } = new("http://localhost:26404");
        public Uri PactBrokerUri { get; } = new("http://ec2-63-177-102-205.eu-central-1.compute.amazonaws.com");

        // AWS & External Services
        public IAmazonSQS SqsClient { get; private set; } = default!;
        public IAmazonDynamoDB DynamoDbClient { get; private set; } = default!;
        public AwsContainer AwsContainer { get; private set; } = default!;
        public MockApiContainer AccountApiContainer { get; private set; } = default!;
        public MockApiContainer MediaApiContainer { get; private set; } = default!;

        public IServiceProvider Services => _server.Services;


        public TestAppFixture()
        {
            Console.WriteLine("🔄 Initializing TestAppFixture...");

            // Initialize TestContainers
            _registry = TestContainerRegistry.GetInstance;
            InitializeContainers().GetAwaiter().GetResult();


            // Setup API server
            _server = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(builder =>
                {
                    builder.UseUrls(ApiBaseUri.ToString());
                    builder.UseStartup<TestStartup>(); // Ensure you have a TestStartup class for DI overrides
                    builder.ConfigureServices(services =>
                    {
                        Console.WriteLine("🔄 Overriding DI for Testing...");

                        // Remove existing services and replace with test dependencies
                        
                        services.AddSingleton(_ => SqsClient);
                        services.AddSingleton(_ => DynamoDbClient);
                        
                        services.AddHttpClient<ICorporationInfoAdapter, CorporationInfoAdapter>(client =>
                        {
                            client.BaseAddress = new Uri(AccountApiContainer.GetUrl());
                            client.DefaultRequestHeaders.Add("Accept", "application/json");
                        });

                        services.AddHttpClient<IProcessingInfoAdater, ProcessingInfoAdapter>(client =>
                        {
                            client.BaseAddress = new Uri(MediaApiContainer.GetUrl());
                            client.DefaultRequestHeaders.Add("Accept", "application/json");
                        });
                    });
                })
                .Build();

            _server.Start();
            Console.WriteLine($"✅ API Server started at {ApiBaseUri}");

            // ✅ Ensure the endpoints are registered correctly
            PrintRegisteredRoutes(_server.Services);
        }
        private static void PrintRegisteredRoutes(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var actionProvider = scope.ServiceProvider.GetRequiredService<IActionDescriptorCollectionProvider>();
            foreach (var route in actionProvider.ActionDescriptors.Items)
            {
                if (route.AttributeRouteInfo != null)
                    Console.WriteLine($"🔹 Registered Route: {route.AttributeRouteInfo.Template}");
            }
        }

        private async Task InitializeContainers()
        {
            Console.WriteLine("🔄 Starting required containers...");

            var config = new Dictionary<string, BaseConfig>
            {
                ["AWS_CONTAINER"] = new AwsConfig
                {
                    Services = [AwsServices.SQS, AwsServices.DynamoDB],
                    Credentials = new AwsCredentials
                    {
                        AccessKey = MockGenerator.GenerateMock<string>(),
                        AccessSecret = MockGenerator.GenerateMock<string>()
                    }
                },
                ["ACCOUNT_API"] = new MockApiConfig
                {
                    Mappings = [
                        new Mapping
                        {
                            Request = new Request
                            {
                                Method = "GET",
                                UrlPattern = "/corporation/.*"
                            },
                            Response = new Response
                            {
                                Status = 200,
                                Body = JsonSerializer.Serialize(FakeDataManager.GenerateCorporationData()),
                                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                            }
                        }
                    ]
                },
                ["MEDIA_API"] = new MockApiConfig
                {
                    Mappings = [
                        new Mapping
                        {
                            Request = new Request
                            {
                                Method = "GET",
                                UrlPattern = "/configuration/.*"
                            },
                            Response = new Response
                            {
                                Status = 200,
                                Body = JsonSerializer.Serialize(new
                                {
                                    stamp = FakeDataManager.GenerateStampData(),
                                    views = FakeDataManager.GenerateViewData()
                                }),
                                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                            }
                        }
                    ]
                }
            };

            await _registry.InitializeContainers(config);

            // Retrieve TestContainers
            AwsContainer = _registry.GetContainer<AwsContainer>("AWS_CONTAINER");
            AccountApiContainer = _registry.GetContainer<MockApiContainer>("ACCOUNT_API");
            MediaApiContainer = _registry.GetContainer<MockApiContainer>("MEDIA_API");

            // Initialize AWS Clients
            SqsClient = AwsContainer.GetClient<AmazonSQSClient>(new AmazonSQSConfig
            {
                ServiceURL = AwsContainer.GetUrl(),
                AuthenticationRegion = AwsContainer.GetRegion()
            });

            DynamoDbClient = AwsContainer.GetClient<AmazonDynamoDBClient>(new AmazonDynamoDBConfig
            {
                ServiceURL = AwsContainer.GetUrl(),
                AuthenticationRegion = AwsContainer.GetRegion()
            });

            Console.WriteLine("Configuring DynamoDB Tables...");
            CustomInfrastructureConfigManager infrastructurerConfigManager = new(DynamoDbClient);
            await infrastructurerConfigManager.GenerateQaProcessTable();
            Console.WriteLine("DynamoDB Tables Configured.");

            Console.WriteLine("Configuring SQS Queues...");
            SQSFixturesGenerator sqsFixturesGenerator = new(SqsClient);
            string queueUrl = await sqsFixturesGenerator.CreateQueue("QaProcessQueue");
            Console.WriteLine($"SQS Queue Created: {queueUrl}");

            SetEnvironments(new Dictionary<string, string>
            {
                { "ACCOUNT_SERVICE_URL", AccountApiContainer.GetUrl() },
                { "MEDIA_SERVICE_URL", MediaApiContainer.GetUrl() },
                { "QA_SQS_QUEUE_URL", queueUrl },
                { "QA_PROCESS_TABLE", "QaProcess" },
                { "MAX_NUM_PROCESSES", "3"}
            });

            Console.WriteLine("✅ Containers initialized successfully!");
        }

        private static void SetEnvironments(Dictionary<string, string> variables)
        {
            foreach (var (name, value) in variables)
                Environment.SetEnvironmentVariable(name, value);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                Console.WriteLine("🛑 Shutting down TestAppFixture...");
                
                _server?.Dispose();

                _registry.StopContainers().GetAwaiter().GetResult();

                SqsClient = null!;
                DynamoDbClient = null!;
                AwsContainer = null!;
                AccountApiContainer = null!;
                MediaApiContainer = null!;
            }

            _disposed = true;
        }
    }

}
