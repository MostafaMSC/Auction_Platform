using MediatR;
using AuctionSystem.Domain.Repositories;
using AuctionSystem.Application.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AuctionSystem.Application.Queries.Auctions
{
    public class GetBidsQueryHandler : IRequestHandler<GetBidsQuery, IEnumerable<BidDto>>
    {
        private readonly IBidRepository _bidRepository;

        public GetBidsQueryHandler(IBidRepository bidRepository)
        {
            _bidRepository = bidRepository;
        }

        public async Task<IEnumerable<BidDto>> Handle(GetBidsQuery request, CancellationToken cancellationToken)
        {
            var bids = await _bidRepository.GetBidsAsync(request.AuctionId);

return bids.Select(b => new BidDto(
    b.Id,
    b.SellerId,
    b.Amount,
    b.CreatedAt
));

        }
    }
}
