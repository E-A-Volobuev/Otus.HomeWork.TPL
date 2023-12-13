namespace ConsoleClientApp.Extensions.Constants;

internal sealed class ConsoleConstants
{
    public const string AppManual = "Программа предназначена для оценки оптимального способа реализации разных способов распараллеливания задач.";
    public const string Menu = "1 - Вычислить сумму элементов массива и замерить время выпонения.\n2 - Посмотреть историю замеров.\n3 - Выйти из приложения.";
    public const string StartProcess = "Введите все элементы массива чисел поочередно.\nЧтобы закончить ввод чилел введите симол «E». Для выхода введите символ «Q».";
    public const string InputNumber = "Введите число:";
    public const string SelectAction = "Выберите действие:";
    public const string SymbolForExit = "q";
    public const string SymbolForEndInput = "e";
    public const string UrlServerByMathAction = "https://localhost:7202"; //url сервера, отвечающего за подсчёт суммы элементов массива и замер времени
    public const string RepeatMessage = "Что-то пошло не так, попробуйте ещё раз";
    public const string SumElementsByArray = "Сумма элементов массива: ";
    public const string CalculationType = "Тип расчёта: ";
    public const string DurationCalc = "Время выполнения: ";
    public const string StartDateTime = "Дата: ";
    public const string SaveResultCalc = "Сохранить результаты расчёта?\n1 - Да.\n2 - Нет.";
    public const string TryAgain = "Что-то пошло не так... попробуйте ещё раз...";
    public const string SaveComplited = "Данные сохранены!";
    public const string UrlServerByDb = "https://localhost:7098"; //url сервера с бд
    public const string PrintHistory = "История замеров производительности:";
    public const string BorderHistory = "---------------------------------------------";
}
