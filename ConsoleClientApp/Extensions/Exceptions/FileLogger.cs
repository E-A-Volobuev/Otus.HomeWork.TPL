using Microsoft.Extensions.Logging;

namespace ConsoleClientApp.Extensions.Exceptions;

internal sealed class FileLogger : ILogger, IDisposable
{
    string filePath;
    static object _lock = new object();
    public FileLogger(string path)
    {
        filePath = path;
    }
    /// <summary>
    /// этот метод возвращает объект IDisposable, который представляет некоторую область видимости для логгера. В данном случае нам этот метод не важен, поэтому возвращаем значение this - ссылку на текущий объект класса, который реализует интерфейс IDisposable
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    /// <param name="state"></param>
    /// <returns></returns>
    public IDisposable BeginScope<TState>(TState state)
    {
        return this;
    }

    public void Dispose() { }

    /// <summary>
    ///  возвращает значения true или false, которые указыват, доступен ли логгер для использования
    /// </summary>
    /// <param name="logLevel"></param>
    /// <returns></returns>
    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId,
        TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        lock (_lock)
        {
            File.AppendAllText(filePath, formatter(state, exception) + Environment.NewLine);
        }
    }
}
