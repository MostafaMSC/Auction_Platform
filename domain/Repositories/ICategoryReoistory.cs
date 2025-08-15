public interface ICategoryRepository
{
    Task CreateAsync(Category category);
    Task<IEnumerable<Category>> GetAllAsync();
    Task<bool> DeleteAsync(int id);
}
