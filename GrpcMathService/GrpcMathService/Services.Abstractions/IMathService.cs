using GrpcMathService.Infrastructure.Extensions;
namespace GrpcMathService.Services.Abstractions;

internal interface IMathService
{
    /// <summary>
    /// вычисляем сумму элементов массива в зависимости от типа вычисления
    /// </summary>
    /// <param name="collection"></param>
    /// <param name="typeCalc"></param>
    /// <returns></returns>
    int GetSumByCollection(IEnumerable<int> collection, TypeCalc typeCalc);
}
