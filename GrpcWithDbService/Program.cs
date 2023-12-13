using GrpcWithDbService.Infrastructure.ApacheKafKa;
using GrpcWithDbService.Infrastructure.EntityFramework;
using GrpcWithDbService.Infrastructure.Repositories;
using GrpcWithDbService.Service.Abstractions;
using GrpcWithDbService.Services.Implementations;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;
builder.Services.AddGrpc();
builder.Services.Configure<AMQPSettings>(configuration.GetSection("KafkaSettings"));
builder.Services.AddSingleton<ICalcResultRepository, CalcResultRepository>();
builder.Services.AddHostedService<ApacheKafkaConsumerService>();
builder.Services.AddDbContext<EFCoreDbContext>(options => options
                .UseNpgsql(configuration.GetConnectionString("DefaultConnection")),ServiceLifetime.Singleton);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<TranslatorService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
