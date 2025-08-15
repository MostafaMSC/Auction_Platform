namespace AuctionSystem.Application.DTOs
{
    public record ProjectDto(
        int Id,
        string Title,
        string Description,
        string? OwnerId,
        string Status,
        decimal Budget
    );
}
