using Amazon.SQS;
using Amazon.Extensions.NETCore.Setup;
using QA.ClientInterface;
using Qa.Infrastructure.Persistence.Contexts;
using QA.Application.Ports.Inbound;
using QA.Application.DTOs;
using Qa.Application.UseCases;
using QA.Infrastructure.ExternalServices;
using QA.Application.Ports.Outbound;
using QA.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using QA.Domain.ValueObjects;
using Amazon.DynamoDBv2;
using Amazon;
using QA.Infrastructure.Messaging;
using QA.Infrastructure.Persistence.Contexts.Resolvers.Interfaces;
using QA.Infrastructure.Persistence.Contexts.Resolvers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JsonOptions>(options =>
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
                    UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToBaseType // ✅ Prevents `$type`
                };
            }
        }}
    };

    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

    // ✅ Automatically serialize enums as strings
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});


// Configure AWS Services
builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonSQS>();


builder.Services.AddSingleton<IAmazonDynamoDB>(provider =>
{
    var regionName = Environment.GetEnvironmentVariable("AWS_REGION") ?? "eu-west-2";
    Console.WriteLine($"🛰️ Loaded AWS_REGION directly: {regionName}");

    var region = RegionEndpoint.GetBySystemName(regionName);

    return new AmazonDynamoDBClient(region);
});

builder.Services.AddSingleton<ITableNameResolver, EnvironmentTableNameResolver>();


// ✅ Correct: Register DynamoDB Context using IAmazonDynamoDB
builder.Services.AddSingleton<DynamoDbContext>();


// Register Repositories
builder.Services.AddScoped<IQAProcessRepository, QAProcessRepository>();

// Register Application UseCases
builder.Services.AddTransient<IQAProcessCreator<CreateQAProcessDTO>, CreateQAProcessUseCase>();
builder.Services.AddTransient<IQAProcessFinder, ProcessFindUseCase>();
builder.Services.AddTransient<IQAGetProcesses, GetProcessUseCase>();


// Register OrchestratorAdapter
builder.Services.AddScoped<IOrchestratorAdapter, OrchestratorAdapter>();

// Register the Adapter with HttpClient
builder.Services.AddHttpClient<ICorporationInfoAdapter, CorporationInfoAdapter>(client =>
{
    // Configure HttpClient (e.g., base address, headers)
    client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("ACCOUNT_SERVICE_URL")!); // Replace with your actual base URL
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

// Register the Adapter with HttpClient
builder.Services.AddHttpClient<IProcessingInfoAdater, ProcessingInfoAdapter>(client =>
{
    // Configure HttpClient (e.g., base address, headers)
    client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("MEDIA_SERVICE_URL")!); // Replace with your actual base URL
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

// Register the Adapter as a Hosted Service
builder.Services.AddHostedService<MaestroQueueAdapter>();

builder.Services.AddOpenApi();
builder.Services.AddControllers();

var app = builder.Build();
app.MapControllers();

app.Run();


