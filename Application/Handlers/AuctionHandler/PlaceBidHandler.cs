using MediatR;
using AuctionSystem.Application.Commands.Auctions;
using AuctionSystem.Domain.Repositories;
using AuctionSystem.Domain.ValueObjects;
using AuctionSystem.Domain.Exceptions;

namespace AuctionSystem.Application.Handlers.AuctionHandlers
{
    public class PlaceBidCommandHandler : IRequestHandler<PlaceBidCommand, PlaceBidResult>
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IUserRepository _userRepository;

        public PlaceBidCommandHandler(
            IAuctionRepository auctionRepository,
            IUserRepository userRepository)
        {
            _auctionRepository = auctionRepository;
            _userRepository = userRepository;
        }

        public async Task<PlaceBidResult> Handle(PlaceBidCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // 1. Get auction
                var auction = await _auctionRepository.GetByIdAsync(request.AuctionId);
                if (auction == null)
                    return new PlaceBidResult(false, "Auction not found");

                // 2. Verify seller exists
                var seller = await _userRepository.GetByIdAsync(request.SellerId);
                if (seller == null)
                    return new PlaceBidResult(false, "Seller not found");

                // 3. Let domain handle the business logic
                var bidAmount = new Money(request.BidAmount);
                auction.PlaceBid(request.SellerId, bidAmount);

                // 4. Save changes
                await _auctionRepository.UpdateAsync(auction);

                // 5. Get the newly created bid ID
                var newBid = auction.Bids.OrderByDescending(b => b.CreatedAt).First();
                
                return new PlaceBidResult(true, null, newBid.Id);
            }
            catch (DomainException ex)
            {
                return new PlaceBidResult(false, ex.Message);
            }
            catch (Exception ex)
            {
                return new PlaceBidResult(false, "An error occurred while placing the bid");
            }
        }
    }
}
