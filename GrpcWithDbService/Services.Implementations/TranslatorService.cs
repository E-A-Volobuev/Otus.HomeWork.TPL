using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcWithDbService.Entities;
using GrpcWithDbService.Service.Abstractions;
using Services.Extensions;

namespace GrpcWithDbService.Services.Implementations;

internal sealed class TranslatorService:Translator.TranslatorBase
{
    private readonly ILogger<TranslatorService> _logger;
    private readonly ICalcResultRepository _repo;
    public TranslatorService(ILogger<TranslatorService> logger,ICalcResultRepository repo)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _repo = repo ?? throw new ArgumentNullException(nameof(repo));
    }
    /// <summary>
    /// получаем от микросервиса ConsoleClientApp запрос через gRPC на получение всех данных из бд и отправляем данные также через gRPC
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<ListResultReply> TransleateMessageToDb(RequestMessage request, ServerCallContext context)
    {
        ListResultReply resultList = new();
        try
        {
            if (request.NameAction == GrpcConstants.GetAllDbAction)
                resultList=await GetResultListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Path: {ex.StackTrace}  Time:{DateTime.Now.ToLongTimeString()} " + $"Detail :{ex.Message}");
        }

        return resultList;
    }
    /// <summary>
    /// кастомный конвертер одного типа из protobuf файла, который автоматически создается gRPC, в другой
    /// </summary>
    /// <returns></returns>
    private async Task<ListResultReply> GetResultListAsync()
    {
        ListResultReply replyList = new();
        IEnumerable<CalcResultEntity> entityList = await _repo.GetAllAsync();
        foreach(var item in entityList)
        {
            ResultReply reply = new ResultReply();
            reply.CalculationType = item.CalculationType;
            reply.CurrentDateTime = Timestamp.FromDateTime(item.CurrentDateTime);
            reply.Sum = item.Sum;
            reply.DurationByOneHundredThousand= Duration.FromTimeSpan(item.DurationByOneHundredThousand);
            reply.DurationByMillion = Duration.FromTimeSpan(item.DurationByMillion);
            reply.DurationByTenMillion= Duration.FromTimeSpan(item.DurationByTenMillion);

            if (reply != null)
                replyList.Results.Add(reply);
        }

        return replyList;
    }
}