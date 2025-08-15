using MediatR;
using AuctionSystem.Application.Commands.Auth;
using AuctionSystem.Application.Interfaces;

namespace AuctionSystem.Application.Handlers
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterUserResult>
    {
        private readonly IUserRegistrationService _registrationService;

        public RegisterUserCommandHandler(IUserRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        public async Task<RegisterUserResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            if (request.Password != request.ConfirmPassword)
                return new RegisterUserResult(false, "Passwords do not match");

            var (Succeeded, ErrorMessage, UserId) = await _registrationService.RegisterUserAsync(
                request.Username,
                request.Email,
                request.Password,
                request.AccountType
            );

            return Succeeded
                ? new RegisterUserResult(true, null, UserId, null)
                : new RegisterUserResult(false, ErrorMessage);
        }
    }
}
