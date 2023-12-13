using Microsoft.Extensions.Logging;

namespace ConsoleClientApp.Extensions.Exceptions;

/// <summary>
/// добавляем метод расширения для возможности установки конфигурации записи в файл в классе program
/// </summary>
internal static class FileLoggerExtensions
{
    public static ILoggingBuilder AddFile(this ILoggingBuilder builder, string filePath)
    {
        builder.AddProvider(new FileLoggerProvider(filePath));
        return builder;
    }
}
