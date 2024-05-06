using listing_backend.Entities;

namespace listing_backend.Services;

public interface ICategoryService
{
    public List<Category> GetAllCategories();
    public Category? GetCategoryById(int id);
    public Category CreateCategory(Category category);
    public Category UpdateCategory(Category category);
    public bool DeleteCategory(int id);
}