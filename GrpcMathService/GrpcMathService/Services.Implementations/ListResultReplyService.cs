using Google.Protobuf.WellKnownTypes;
using GrpcMathService.Infrastructure.Extensions;
using GrpcMathService.Services.Abstractions;
using System.Diagnostics;
namespace GrpcMathService.Services.Implementations;

/// <summary>
/// сервис получения объектов расчёта с указанием времени на каждый расчёт
/// </summary>
internal sealed class ListResultReplyService: IListResultReplyService
{
    private readonly IMathService _mathService;
    public ListResultReplyService(IMathService mathService) 
    {
        _mathService = mathService ?? throw new ArgumentNullException(nameof(mathService));
    }
    /// <summary>
    /// получаем значения всех расчётов с указанием времени
    /// </summary>
    /// <param name="collection"></param>
    /// <returns></returns>
    public ListResultReply GetListResults(IEnumerable<int> collection)
    {
        ListResultReply resultList = new();
        resultList.Results.Add(GetObjWithTrackTime(collection, TypeCalc.None));
        resultList.Results.Add(GetObjWithTrackTime(collection, TypeCalc.ParallelLocker));
        resultList.Results.Add(GetObjWithTrackTime(collection, TypeCalc.ParallelInterlocked));
        resultList.Results.Add(GetObjWithTrackTime(collection, TypeCalc.ParallelLINQ));

        return resultList;
    }
    /// <summary>
    /// получаем результат вычисления по типу расчёта
    /// </summary>
    /// <param name="collection"></param>
    /// <param name="typeCalc"></param>
    /// <returns></returns>
    private ResultReply GetResultByTypeCalc(IEnumerable<int> collection, TypeCalc typeCalc)
    {
        ResultReply reply= new ResultReply();
        reply.CalculationType = (int)typeCalc;
        reply.CurrentDateTime= Timestamp.FromDateTime(DateTime.UtcNow);
        reply.Sum=_mathService.GetSumByCollection(collection, typeCalc);

        return reply;
    }
    /// <summary>
    /// получаем результат вычисления суммы элементов массива со значением времени
    /// </summary>
    /// <param name="collection"></param>
    /// <param name="typeCalc"></param>
    /// <returns></returns>
    private ResultReply GetObjWithTrackTime(IEnumerable<int> collection, TypeCalc typeCalc)
    {
        Stopwatch watch = new();
        watch.Start();
        ResultReply noneCalc = GetResultByTypeCalc(collection, typeCalc);
        watch.Stop();
        noneCalc.Duration = Duration.FromTimeSpan(watch.Elapsed);
        return noneCalc;
    }
}
