using Bookify.Domain.Abstractions;
using Bookify.Domain.Users.Events;

namespace Bookify.Domain.Users;

public sealed class User : Entity
{
    private User(Guid id, FirstName firstName, LastName lastName, Email email)
        : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    private User()
    {
        
    }

    public FirstName FirstName { get; private set; }
    public LastName LastName { get; private set; }
    public Email Email { get; private set; }
    
    /*Since it's not initialized from the constructor
    should have initial value*/
    public string IdentityId { get; private set; } = String.Empty;

    public static User Create(FirstName firstName, LastName lastName, Email email)
    {
        var user = new User(Guid.NewGuid(), firstName, lastName, email);
        user.RaiseDomainEvent(new UserCreatedEvent(user.Id));
        return user;
    }

    public void SetIdentityId(string identityId)
    {
        if (string.IsNullOrEmpty(identityId))
            throw new ArgumentNullException(nameof(identityId));

        IdentityId = identityId;
    }
}