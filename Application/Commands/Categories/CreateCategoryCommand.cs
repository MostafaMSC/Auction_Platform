using MediatR;

namespace AuctionSystem.Application.Commands.Category
{
    public record CreateCategoryCommand(
        string CategoryName,
        string CategoryDescription
    ) : IRequest<int>;  // returns new Id
}