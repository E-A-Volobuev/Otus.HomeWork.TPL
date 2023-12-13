namespace GrpcMathService.Services.Abstractions;

internal interface IListResultReplyService
{
    /// <summary>
    /// получаем значения всех расчётов с указанием времени
    /// </summary>
    /// <param name="collection"></param>
    /// <returns></returns>
    ListResultReply GetListResults(IEnumerable<int> collection);
}
