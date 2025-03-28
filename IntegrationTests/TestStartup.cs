using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Amazon.DynamoDBv2;
using Amazon.SQS;
using Qa.Infrastructure.Persistence.Contexts;
using QA.Application.Ports.Outbound;
using QA.Infrastructure.ExternalServices;
using QA.Infrastructure.Persistence.Repositories;
using QA.Application.Ports.Inbound;
using QA.Application.DTOs;
using Qa.Application.UseCases;
using QA.Infrastructure.Messaging;
using QA.ClientInterface;
using QA.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using QA.ClientInterface.Controllers;
using QA.Infrastructure.Persistence.Contexts.Resolvers.Interfaces;
using QA.Infrastructure.Persistence.Contexts.Resolvers;

namespace IntegrationTests
{
public class TestStartup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        Console.WriteLine("🔄 Registering test dependencies...");

        // ✅ JSON Serialization Configuration (same as Program.cs)
        services.Configure<JsonOptions>(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

            options.JsonSerializerOptions.TypeInfoResolver = new DefaultJsonTypeInfoResolver
            {
                Modifiers = { typeInfo =>
                {
                    if (typeInfo.Type == typeof(MediaProcessingInfo))
                    {
                        typeInfo.PolymorphismOptions = new JsonPolymorphismOptions
                        {
                            DerivedTypes =
                            {
                                new JsonDerivedType(typeof(ImageProcessingInfo)),
                                new JsonDerivedType(typeof(VideoProcessingInfo))
                            },
                            UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToBaseType
                        };
                    }
                }}
            };

            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        services.AddSingleton<ITableNameResolver, EnvironmentTableNameResolver>();


        // ✅ Register controllers
        services.AddControllers().AddApplicationPart(typeof(QAProcessController).Assembly);

        // ✅ AWS Services (Will be overridden by TestAppFixture)
        services.AddSingleton<DynamoDbContext>();

        // ✅ Register Repositories (same as Program.cs)
        services.AddScoped<IQAProcessRepository, QAProcessRepository>();

        // ✅ Register Application UseCases (same as Program.cs)
        services.AddTransient<IQAProcessCreator<CreateQAProcessDTO>, CreateQAProcessUseCase>();
        services.AddTransient<IQAProcessFinder, ProcessFindUseCase>();
        services.AddTransient<IQAGetProcesses, GetProcessUseCase>();

        // ✅ Register OrchestratorAdapter
        services.AddScoped<IOrchestratorAdapter, OrchestratorAdapter>();

        // ✅ Register Hosted Service
        services.AddHostedService<MaestroQueueAdapter>();

        Console.WriteLine("✅ Test dependencies registered.");
    }

    public static void Configure(IApplicationBuilder app, IWebHostEnvironment env, IActionDescriptorCollectionProvider actionProvider)
    {
        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers(); // Make sure this is still here

            Console.WriteLine("🧩 Registered Controller Endpoints:");
            foreach (var route in actionProvider.ActionDescriptors.Items)
            {
                if (route.AttributeRouteInfo != null)
                {
                    Console.WriteLine($"➡️  {route.AttributeRouteInfo.Template}");
                }
            }
        });
    }
}

}

