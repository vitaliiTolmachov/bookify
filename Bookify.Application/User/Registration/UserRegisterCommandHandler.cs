using Bookify.Application.Abstractions.Authentication;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Shared;
using Bookify.Domain.Users;

namespace Bookify.Application.User.Registration;

public sealed class UserRegisterCommandHandler : ICommandHandler<UserRegisterCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBookifyAuthenticationService _bookifyAuthenticationService;

    public UserRegisterCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IBookifyAuthenticationService bookifyAuthenticationService)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _bookifyAuthenticationService = bookifyAuthenticationService;
    }
    
    public async Task<Result<Guid>> Handle(UserRegisterCommand request, CancellationToken cancellationToken)
    {
        var user = Domain.Users.User.Create(
            new FirstName(request.FirstName),
            new LastName(request.LastName),
            new Email(request.Email));

        var identityId = await _bookifyAuthenticationService.AuthenticateAsync(user, request.Password, cancellationToken);

        user.SetIdentityId(identityId);
        
        _userRepository.Add(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}