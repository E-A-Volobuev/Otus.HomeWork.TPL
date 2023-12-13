
namespace ConsoleClientApp.Services.Abstractions;

internal interface IConsoleActionService
{
    void PrintMessage(string message);
    string GetConsoleInput();
    void PressKey();
}
