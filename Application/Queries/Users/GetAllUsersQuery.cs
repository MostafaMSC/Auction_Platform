using AuctionSystem.Domain.Constants;
using MediatR;

namespace AuctionSystem.Application.Queries.Users
{
    public record GetAllUsersQuery(
        string? SearchTerm = null,
        string? Status = null
    ) : IRequest<List<UserSummaryDto>>; // ← هكذا نحدد النوع المرجع



    public record PaginatedUsersResult(
        List<UserSummaryDto> Users,
        int TotalCount,
        int Page,
        int PageSize,
        int TotalPages
    );

    public record UserSummaryDto(
        string Id,
        string Email,
        string FullName,
        AccountType AccountType,
        string VerificationStatus

    );
}