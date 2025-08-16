using AuctionSystem.Application.DTOs;

public record AuctionDto(
    int Id,
    int ProjectId,
    decimal StartingPrice,
    decimal CurrentPrice,
    decimal MinPrice,
    decimal TargetPrice,
    DateTime? StartAt,
    DateTime? EndAt,
    string Status,
    bool IsActive,
    int? WinningBidId,
    string? WinningSellerId,
    List<BidDto> Bids
);
