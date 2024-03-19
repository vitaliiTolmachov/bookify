namespace Bookify.Domain.Abstractions;

public abstract class Entity : IEquatable<Guid>
{
    protected Entity(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException(nameof(id));
        
        Id = id;
    }
    public Guid Id { get; init; }

    public bool Equals(Guid otherId)
    {
        return Id.Equals(otherId);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Entity) obj);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
    
    protected bool Equals(Entity other)
    {
        return Id.Equals(other.Id);
    }
}