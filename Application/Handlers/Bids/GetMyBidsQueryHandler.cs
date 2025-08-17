using MediatR;
using AuctionSystem.Domain.Repositories;
using AuctionSystem.Application.DTOs;

public class GetMyBidsQueryHandler : IRequestHandler<GetMyBidsQuery, IEnumerable<BidDto>>
{
    private readonly IBidRepository _bidRepository;

    public GetMyBidsQueryHandler(IBidRepository bidRepository)
    {
        _bidRepository = bidRepository;
    }

    public async Task<IEnumerable<BidDto>> Handle(GetMyBidsQuery request, CancellationToken cancellationToken)
    {
        var bids = await _bidRepository.GetBidsBySellerIdAsync(request.UserId);
return bids.Select(b => new BidDto(
    b.Id,
    b.SellerId,
    b.Amount,
    b.CreatedAt
));

    }
}
