using AutoMapper;
using GrpcWithDbService.Entities;
using GrpcWithDbService.Extensions.Constants;
using GrpcWithDbService.Infrastructure.ApacheKafKa;
using GrpcWithDbService.Service.Abstractions;
using Service.Contracts;

namespace GrpcWithDbService.Services.Implementations;

internal sealed class ApacheKafkaConsumerService : BackgroundService
{
    private readonly ICalcResultRepository _repo;
    private readonly ILogger<ApacheKafkaConsumerService> _logger;
    private readonly IMapper _mapper;
    public ApacheKafkaConsumerService(ICalcResultRepository repo, ILogger<ApacheKafkaConsumerService> logger, IMapper mapper)
    {
        _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper= mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    /// <summary>
    /// запускаем в фоновой задаче слушателя кафки (отправку запросов в топик для получения данных)
    /// </summary>
    /// <param name="stoppingToken"></param>
    /// <returns></returns>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            IConfiguration configuration = new ConfigurationBuilder()
              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
              .Build();
            Console.WriteLine($"Consumer of groupId {KafkaConstants.GroupId} was started");
            List<SumResultDto> dtoList = new();
            ApacheKafkaConsumer consumer = new ApacheKafkaConsumer(configuration, KafkaConstants.GroupId, KafkaConstants.TopicName);

            //запуск цикла в отдельной задаче,чтобы не заблокировать весь хост
            Task task1 = Task.Run(async () =>
            {
                while (true)
                    await ChekDataHelperAsync(consumer, dtoList);
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Path: {ex.StackTrace}  Time:{DateTime.Now.ToLongTimeString()} " +
                             $"Detail :{ex.Message}");
        }
    }
    private async Task ChekDataHelperAsync(ApacheKafkaConsumer consumer, List<SumResultDto> dtoList)
    {
        dtoList = consumer.Consume(dtoList);
        if (dtoList.Count > 0)
        {
            CalcResultEntity[] array = _mapper.Map<CalcResultEntity[]>(dtoList.ToArray());
            await _repo.CreateRangeAsync(array);
        }
    }
}
