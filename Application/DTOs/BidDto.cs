using AuctionSystem.Domain.ValueObjects;

namespace AuctionSystem.Application.DTOs
{
    public record BidDto(
        int Id,
        string SellerId,
        Money Amount,
        DateTime CreatedAt
    );

}