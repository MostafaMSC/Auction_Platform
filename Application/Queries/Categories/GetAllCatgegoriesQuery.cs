using MediatR;
using AuctionSystem.Application.DTOs;

namespace AuctionSystem.Application.Queries.Category
{
    public record GetAllCategoriesQuery : IRequest<IEnumerable<CategoryDto>>;
}