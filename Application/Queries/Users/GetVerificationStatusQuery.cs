using MediatR;

namespace AuctionSystem.Application.Queries.Users
{
    public record GetVerificationStatusQuery(string UserId) : IRequest<VerificationStatusDto?>;

    public record VerificationStatusDto(
        string UserId,
        string Status, // "Pending", "Approved", "Rejected"
        List<DocumentDto> Documents,
        DateTime? VerificationDate
    );

    public record DocumentDto(
        int Id,
        string DocumentType,
        string Status,
        DateTime UploadedAt
    );
}