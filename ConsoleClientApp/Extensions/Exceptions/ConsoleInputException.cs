namespace ConsoleClientApp.Extensions.Exceptions;

internal sealed class ConsoleInputException : Exception
{
    public ConsoleInputException(string message)
        : base(message) { }
}
