using FluentValidation;

namespace AuctionSystem.Application.Commands.Auctions
{
    public class CreateAuctionCommandValidator : AbstractValidator<CreateAuctionCommand>
    {
        public CreateAuctionCommandValidator()
        {
            RuleFor(x => x.ProjectId)
                .GreaterThan(0).WithMessage("ProjectId is required");

            RuleFor(x => x.StartingPrice)
                .GreaterThan(0).WithMessage("StartingPrice must be greater than 0");

            RuleFor(x => x.MinPrice)
                .GreaterThan(0).WithMessage("MinPrice must be greater than 0");

            RuleFor(x => x.MaxDurationHours)
                .GreaterThan(0).WithMessage("MaxDuration must be positive");

            RuleFor(x => x.PriceDropIntervalMinutes)
                .GreaterThan(0).WithMessage("PriceDropInterval must be positive");

            RuleFor(x => x.PriceDropAmount)
                .GreaterThan(0).WithMessage("PriceDropAmount must be positive");

            RuleFor(x => x.StartingPrice)
                .GreaterThanOrEqualTo(x => x.MinPrice)
                .WithMessage("StartingPrice must be greater than or equal to MinPrice");
        }
    }
}
