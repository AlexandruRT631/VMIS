using listing_backend.DataAccess;
using listing_backend.Entities;

namespace listing_backend.Repositories;

public class CategoryRepository(ListingDbContext context) : ICategoryRepository
{
    public List<Category> GetAllCategories()
    {
        return context.Categories.ToList();
    }

    public Category? GetCategoryById(int id)
    {
        return context.Categories.Find(id);
    }

    public Category CreateCategory(Category category)
    {
        context.Categories.Add(category);
        context.SaveChanges();
        return category;
    }

    public Category UpdateCategory(Category category)
    {
        context.Categories.Update(category);
        context.SaveChanges();
        return category;
    }

    public bool DeleteCategory(Category category)
    {
        context.Categories.Remove(category);
        context.SaveChanges();
        return true;
    }

    public bool DoesCategoryExist(int id)
    {
        return context.Categories.Any(e => e.Id == id);
    }
}