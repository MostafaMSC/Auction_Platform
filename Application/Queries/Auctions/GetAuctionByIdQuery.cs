using AuctionSystem.Application.DTOs;
using MediatR;

namespace AuctionSystem.Application.Queries.Auctions
{
    public record GetAuctionByIdQuery(int AuctionId) : IRequest<AuctionDto?>;
}
