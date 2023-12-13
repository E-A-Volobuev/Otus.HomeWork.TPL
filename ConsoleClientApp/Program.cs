using ConsoleClientApp;
using ConsoleClientApp.Extensions.Exceptions;
using ConsoleClientApp.Infrastructure.ApacheKafka;
using ConsoleClientApp.Services.Abstractions;
using ConsoleClientApp.Services.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
ConfigurationManager configuration = builder.Configuration;
builder.Services.AddHostedService<StartApp>();
builder.Services.AddScoped<IConsoleActionService, ConsoleActionService>();
builder.Services.AddScoped<IGrpcService, GrpcService>();
builder.Services.AddScoped<IApacheKafkaProducerService, ApacheKafkaProducerService>();
builder.Services.AddScoped<ICalculationResultService, CalculationResultService>();
builder.Services.AddLogging(loggerBuilder =>
{
    loggerBuilder.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt"));
});
builder.Services.Configure<AMQPSettings>(configuration.GetSection("KafkaSettings"));

using IHost host = builder.Build();

await host.RunAsync();