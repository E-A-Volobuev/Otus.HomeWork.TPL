using ConsoleClientApp.Services.Abstractions;
namespace ConsoleClientApp.Services.Implementations;

/// <summary>
/// класс работы с консолью
/// </summary>
internal sealed class ConsoleActionService:IConsoleActionService
{
    public void PrintMessage(string message)
    {
        Console.WriteLine(message);
    }
    public string GetConsoleInput()
    {
       string text= Console.ReadLine();
        return text;
    }
    public void PressKey()
    {
        Console.ReadLine();
    }
}
