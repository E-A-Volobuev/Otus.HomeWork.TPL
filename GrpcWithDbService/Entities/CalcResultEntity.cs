namespace GrpcWithDbService.Entities;

/// <summary>
/// результат вычислений суммы элементов массива, которые клиент ConsoleClientApp получил от микросервиса GrpcMathService
/// </summary>
public class CalcResultEntity:IEntityId
{
    public int Id { get; set; }
    /// <summary>
    /// полное описание для вывода пользователю в консоль
    /// </summary>
    public string Description { get; set; }
    public int CalculationType { get; set; }
    /// <summary>
    /// сумма элементов массива
    /// </summary>
    public int Sum { get; set; }
    /// <summary>
    /// время , потраченно на подсчёт суммы массива чисел для 100 000 операций
    /// </summary>
    public TimeSpan DurationByOneHundredThousand { get; set; }

    /// <summary>
    /// время , потраченно на подсчёт суммы массива чисел для 1000 000 операций
    /// </summary>
    public TimeSpan DurationByMillion { get; set; }

    // <summary>
    /// время , потраченно на подсчёт суммы массива чисел для 10 000 000 операций
    /// </summary>
    public TimeSpan DurationByTenMillion { get; set; }
    /// <summary>
    /// дата расчёта
    /// </summary>
    public DateTime CurrentDateTime { get; set; }
}
