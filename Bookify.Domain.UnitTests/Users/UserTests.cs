using Bookify.Domain.Users;
using Bookify.Domain.Users.Events;
using FluentAssertions;

namespace Bookify.Domain.UnitTests.Users;

public class UserTests : BaseTest
{

    [Fact]
    public void Create_Should_SetProperties()
    {
        // Act
        var user = User.Create(UserData.FirstName, UserData.LastName, UserData.Email);

        // Assert
        user.FirstName.Should().Be(UserData.FirstName);
        user.LastName.Should().Be(UserData.LastName);
        user.Email.Should().Be(UserData.Email);
        user.IdentityId.Should().BeEmpty();
    }

    [Fact]
    public void SetIdentityId_Should_SetIdentityId()
    {
        //Arrange
        var identityId = Guid.NewGuid().ToString();
        
        // Act
        var user = User.Create(UserData.FirstName, UserData.LastName, UserData.Email);
        user.SetIdentityId(identityId);

        // Assert
        user.FirstName.Should().Be(UserData.FirstName);
        user.LastName.Should().Be(UserData.LastName);
        user.Email.Should().Be(UserData.Email);
        user.IdentityId.Should().Be(identityId);
    }
    
    [Fact]
    public void SetIdentityId_Should_RaiseException_WhenNull()
    {
        // Act
        var user = User.Create(UserData.FirstName, UserData.LastName, UserData.Email);
        var act = () => user.SetIdentityId(null);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }
    
    [Fact]
    public void Create_Should_RaiseUserCreatedDomainEvent()
    {
        // Act
        var user = User.Create(UserData.FirstName, UserData.LastName, UserData.Email);

        // Assert
        var userCreatedDomainEvent = AssertDomainEventWasPublished<UserCreatedEvent>(user);

        userCreatedDomainEvent.UserId.Should().Be(user.Id);
    }
    
    [Fact]
    public void Create_Should_AddRegisteredRoleToUser()
    {
        // Act
        var user = User.Create(UserData.FirstName, UserData.LastName, UserData.Email);

        // Assert
        user.Roles.Should().Contain(Role.Registered);
    }
}