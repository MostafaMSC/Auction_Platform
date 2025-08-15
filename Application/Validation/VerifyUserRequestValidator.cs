    using AuctionSystem.Application.Commands.Users;
    using FluentValidation;


    namespace AuctionSystem.Application.Validation.Users
    {
        public class VerifyUserRequestValidator : AbstractValidator<VerifyUserCommand>
        {
            public VerifyUserRequestValidator()
            {
                RuleFor(x => x.DocumentType)
                    .NotEmpty().WithMessage("DocumentType is required");

                RuleFor(x => x.DocumentFile)
                    .NotNull().WithMessage("Document file is required")
                    .Must(file => file.Length > 0).WithMessage("File cannot be empty");
            }
        }
    }
