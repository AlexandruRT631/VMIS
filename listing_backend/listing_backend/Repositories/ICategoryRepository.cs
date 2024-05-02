using listing_backend.Entities;

namespace listing_backend.Repositories;

public interface ICategoryRepository
{
    public List<Category> GetAllCategories();
    public Category? GetCategoryById(int id);
    public Category CreateCategory(Category category);
    public Category UpdateCategory(Category category);
    public bool DeleteCategory(Category category);
    public bool DoesCategoryExist(int id);
}