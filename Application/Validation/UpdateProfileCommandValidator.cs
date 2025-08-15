// File: Application/Commands/Users/Validators/UpdateProfileCommandValidator.cs
using FluentValidation;

namespace AuctionSystem.Application.Commands.Users
{
    public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
    {
        public UpdateProfileCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.UserName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.AccountType)
                .IsInEnum();
        }
    }
}
