using System.Runtime.Serialization;

namespace GrpcWithDbService.Extensions.Exceptions;

internal sealed class DataOperationException : Exception
{
    public DataOperationException()
    {
    }

    public DataOperationException(string message)
        : base(message)
    {
    }

    public DataOperationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    protected DataOperationException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
