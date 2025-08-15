using MediatR;

namespace AuctionSystem.Application.Commands.Categories
{
    public record DeleteCategoryCommand(int CategoryId) : IRequest<bool>;
}
