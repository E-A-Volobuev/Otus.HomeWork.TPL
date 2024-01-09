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
        resultList.Results.Add(GetObjWithTrackTime(collection, TypeCalc.Thread));

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
         ResultReply noneCalc = new();
        ///вычисляем время для 100 000 , 1000 000 и 10 000 000 итераций
        (ResultReply reply, Stopwatch watchByOneHundredThousand) = TimeHelper(noneCalc, 10, collection, typeCalc, GetResultByTypeCalc);
        (ResultReply replyTwo, Stopwatch watchByMillion) = TimeHelper(noneCalc, 100, collection, typeCalc, GetResultByTypeCalc);
        (ResultReply replyThree, Stopwatch watchByTenMillion) = TimeHelper(noneCalc, 200, collection, typeCalc, GetResultByTypeCalc);

        noneCalc = replyThree;
        noneCalc.DurationByOneHundredThousand = Duration.FromTimeSpan(watchByOneHundredThousand.Elapsed);
        noneCalc.DurationByMillion = Duration.FromTimeSpan(watchByMillion.Elapsed);
        noneCalc.DurationByTenMillion = Duration.FromTimeSpan(watchByTenMillion.Elapsed);

        return noneCalc;
    }
    private (ResultReply reply, Stopwatch watch) TimeHelper(ResultReply noneCalc,int count, IEnumerable<int> collection, TypeCalc typeCalc,Func<IEnumerable<int>, TypeCalc, ResultReply> operation)
    {
        Stopwatch watch = new();
        watch.Start();
        Parallel.For(0, count, i =>noneCalc = operation(collection, typeCalc));
        watch.Stop();

        return (noneCalc,watch);
    }
}
