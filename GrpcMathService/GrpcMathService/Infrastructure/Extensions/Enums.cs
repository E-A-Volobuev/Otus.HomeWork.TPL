namespace GrpcMathService.Infrastructure.Extensions;

/// <summary>
/// тип вычисления суммы массива
/// </summary>
internal enum TypeCalc
{
    /// <summary>
    /// последовательынй перебор
    /// </summary>
    None,
    /// <summary>
    /// параллельно + locker
    /// </summary>
    ParallelLocker,
    /// <summary>
    /// параллельно + interlocked
    /// </summary>
    ParallelInterlocked,
    /// <summary>
    /// параллельно через LINQ
    /// </summary>
    ParallelLINQ
}
