
using System.ComponentModel.DataAnnotations;

namespace Service.Contracts;

public sealed class SumResultDto
{
    /// <summary>
    /// полное описание для вывода пользователю в консоль
    /// </summary>
    public string Description { get; set; }

    public CalculationType CalculationType { get; set; }
    /// <summary>
    /// сумма элементов массива
    /// </summary>
    public int Sum { get; set; }
    /// <summary>
    /// время , потраченно на подсчёт суммы массива чисел
    /// </summary>
    public TimeSpan Duration { get; set; }
    /// <summary>
    /// дата расчёта
    /// </summary>
    public DateTime CurrentDateTime { get; set; }

}
/// <summary>
/// тип расчёта
/// </summary>
public enum CalculationType
{
    [Display(Name = "Последовательынй перебор")]
    None,
    [Display(Name = "Параллельное (locker)")]
    ParallelLocker,
    [Display(Name = "Параллельное (interlocked)")]
    ParallelInterlocked,
    [Display(Name = "Параллельное (LINQ)")]
    ParallelLINQ
}

public static class CalculationTypeExtender
{
    public static string DisplayName(this CalculationType method)
    {
        return method switch
        {
            CalculationType.None => "Последовательынй перебор",
            CalculationType.ParallelLocker => "Параллельное (locker)",
            CalculationType.ParallelInterlocked => "Параллельное (interlocked)",
            _ => "Параллельное (LINQ)"
        };
    }
}
