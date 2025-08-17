using MediatR;
using AuctionSystem.Application.Commands.Auctions;
using AuctionSystem.Domain.Repositories;
using AuctionSystem.Domain.ValueObjects;
using AuctionSystem.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;


namespace AuctionSystem.Application.Handlers.AuctionHandlers
{
    public class PlaceBidCommandHandler : IRequestHandler<PlaceBidCommand, PlaceBidResult>
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PlaceBidCommandHandler(
            IAuctionRepository auctionRepository,
            IUserRepository userRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _auctionRepository = auctionRepository;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<PlaceBidResult> Handle(PlaceBidCommand request, CancellationToken cancellationToken)
        {
         try
    {
        // 1. Get auction
        var auction = await _auctionRepository.GetByIdAsync(request.AuctionId);
        if (auction == null)
            return new PlaceBidResult(false, "Auction not found");

        // 2. Determine seller ID
            string sellerId = request.SellerId;
            if (string.IsNullOrEmpty(sellerId))
            {
                sellerId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(sellerId))
                    return new PlaceBidResult(false, "Seller not specified and no user logged in");
            }


        // 3. Verify seller exists
        var seller = await _userRepository.GetByIdAsync(sellerId);
        if (seller == null)
            return new PlaceBidResult(false, "Seller not found");

        // 4. Let domain handle the business logic
        var bidAmount = new Money(request.BidAmount);
        auction.PlaceBid(sellerId, bidAmount);

        // 5. Save changes
        await _auctionRepository.UpdateAsync(auction);

        // 6. Get the newly created bid ID
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
