using MediatR;
using AuctionSystem.Application.DTOs;
using System.Collections.Generic;

namespace AuctionSystem.Application.Queries.Auctions
{
    public record GetBidsQuery(int AuctionId) : IRequest<IEnumerable<BidDto>>;
}
