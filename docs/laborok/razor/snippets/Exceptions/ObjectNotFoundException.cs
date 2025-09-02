using System.Runtime.Serialization;

namespace MovieCatalog.Exceptions;

public class ObjectNotFoundException : Exception
{
    public object? Id { get; }
    public ObjectNotFoundException(object? id = null, string? message = null, Exception? innerException = null) : 
        base(message ?? $"Object not found{(id == null ? "" : $" ({id})")}).", innerException) 
            => Id = id;
}
public sealed class ObjectNotFoundException<TEntity> : ObjectNotFoundException
{
    public Type EntityType => typeof(TEntity);
    public ObjectNotFoundException(object? id = null, string? message = null, Exception? innerException = null) 
        : base(id, message ?? $"{typeof(TEntity).Name} not found{(id == null ? "" : $" ({id})")}.", innerException) { }
}

