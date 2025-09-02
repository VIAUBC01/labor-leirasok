using System.Runtime.Serialization;

namespace MovieCatalogApi.Exceptions;

public class ConflictException : Exception
{
    public ConflictException()
    {
    }

    protected ConflictException(SerializationInfo info, StreamingContext context) 
        : base(info, context)
    {
    }

    public ConflictException(string? message) 
        : base(message)
    {
    }

    public ConflictException(string? message, Exception? innerException) 
        : base(message, innerException)
    {
    }

    public ConflictException(int id, string typeName, Exception? innerException = null)
        : this($" The entity {typeName}({id}) has conflicting values.", innerException)
    {
    }
}

