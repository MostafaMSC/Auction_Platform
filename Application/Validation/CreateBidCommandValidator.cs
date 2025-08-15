using FluentValidation;

namespace AuctionSystem.Application.Commands.Auctions
{
    public class PlaceBidCommandValidator : AbstractValidator<PlaceBidCommand>
    {
        public PlaceBidCommandValidator()
        {
            RuleFor(x => x.AuctionId)
                .GreaterThan(0).WithMessage("AuctionId must be greater than 0");

            RuleFor(x => x.SellerId)
                .NotEmpty().WithMessage("SellerId is required");

            RuleFor(x => x.BidAmount)
                .GreaterThan(0).WithMessage("Bid amount must be greater than 0");
        }
    }
}
