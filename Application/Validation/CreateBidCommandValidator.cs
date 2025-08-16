using FluentValidation;
using AuctionSystem.Domain.Repositories;

namespace AuctionSystem.Application.Commands.Auctions
{
    public class PlaceBidCommandValidator : AbstractValidator<PlaceBidCommand>
    {
        public PlaceBidCommandValidator(IAuctionRepository auctionRepository)
        {
            RuleFor(x => x.AuctionId)
                .GreaterThan(0).WithMessage("AuctionId must be greater than 0");

            RuleFor(x => x.BidAmount)
                .GreaterThan(0).WithMessage("Bid amount must be greater than 0");

            RuleFor(x => x)
                .MustAsync(async (command, cancellation) =>
                {
                    var auction = await auctionRepository.GetByIdAsync(command.AuctionId);
                    if (auction == null) return false; // invalid auction
                    if (!auction.IsActive) return false; // auction must be active
                    return command.BidAmount < auction.CurrentPrice.Amount; // bid must be less than current price
                })
                .WithMessage("Bid amount must be less than the current auction price and auction must be active");
        }
    }
}
