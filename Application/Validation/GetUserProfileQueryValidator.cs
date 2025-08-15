using AuctionSystem.Application.Queries.Users;
using FluentValidation;

namespace AuctionSystem.Application.Validation
{
    public class GetUserProfileQueryValidator : AbstractValidator<GetUserProfileQuery>
    {
        public GetUserProfileQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required");
        }
    }
}
