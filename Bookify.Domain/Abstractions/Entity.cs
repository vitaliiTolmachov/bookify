namespace Bookify.Domain.Abstractions;

public abstract class Entity : IEquatable<Guid>
{
    private readonly List<IDomainEvent> _domainEvents = [];
    
    protected Entity(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id should not be empty for entity", nameof(id));
        
        Id = id;
    }
    public Guid Id { get; init; }

    public IReadOnlyCollection<IDomainEvent> GetDomainEvents()
    {
        return _domainEvents.AsReadOnly();
    }

    protected void RaiseDomainEvent(IDomainEvent @event)
    {
        _domainEvents.Add(@event);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

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