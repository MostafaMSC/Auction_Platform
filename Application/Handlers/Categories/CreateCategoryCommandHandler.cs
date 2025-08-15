// Handler for CreateCategory
using AuctionSystem.Application.Commands.Categories;
using AuctionSystem.Application.Commands.Category;
using AuctionSystem.Application.DTOs;
using AuctionSystem.Application.Queries.Category;
using AuctionSystem.Domain.Repositories;
using MediatR;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, int>
{
    private readonly ICategoryRepository _categoryRepo;

    public CreateCategoryCommandHandler(ICategoryRepository categoryRepo)
    {
        _categoryRepo = categoryRepo;
    }

    public async Task<int> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = new Category
        {
            CategoryName = request.CategoryName,
            CategoryDescription = request.CategoryDescription
        };

        await _categoryRepo.CreateAsync(category);
        return category.Id;
    }
}

// Handler for GetAllCategories
public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, IEnumerable<CategoryDto>>
{
    private readonly ICategoryRepository _categoryRepo;

    public GetAllCategoriesQueryHandler(ICategoryRepository categoryRepo)
    {
        _categoryRepo = categoryRepo;
    }

    public async Task<IEnumerable<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var list = await _categoryRepo.GetAllAsync();
        return list.Select(c => new CategoryDto
        {
            Id = c.Id,
            CategoryName = c.CategoryName,
            CategoryDescription = c.CategoryDescription
        });
    }
}

// Handler for DeleteCategory
public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, bool>
{
    private readonly ICategoryRepository _categoryRepo;

    public DeleteCategoryCommandHandler(ICategoryRepository categoryRepo)
    {
        _categoryRepo = categoryRepo;
    }

    public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        return await _categoryRepo.DeleteAsync(request.CategoryId);
    }
}
