namespace ConsoleClientApp.Extensions.Exceptions;

internal sealed class GrpcException : Exception
{
    public GrpcException(string message)
    : base(message) { }
}
