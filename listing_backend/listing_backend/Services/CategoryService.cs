using listing_backend.Constants;
using listing_backend.Entities;
using listing_backend.Exceptions;
using listing_backend.Repositories;

namespace listing_backend.Services;

public class CategoryService(ICategoryRepository categoryRepository) : ICategoryService
{
    public List<Category> GetAllCategories()
    {
        return categoryRepository.GetAllCategories();
    }

    public Category? GetCategoryById(int id)
    {
        if (id <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (!categoryRepository.DoesCategoryExist(id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.CategoryNotFound);
        }
        
        return categoryRepository.GetCategoryById(id);
    }

    public Category CreateCategory(Category category)
    {
        if (category == null)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidCategory);
        }
        if (category.Id < 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (categoryRepository.DoesCategoryExist(category.Id))
        {
            throw new ObjectAlreadyExistsException(ExceptionMessages.CategoryAlreadyExists);
        }
        if (string.IsNullOrWhiteSpace(category.Name))
        {
            throw new InvalidArgumentException(ExceptionMessages.RequiredName);
        }
        if (categoryRepository.DoesCategoryExist(category.Name))
        {
            throw new ObjectAlreadyExistsException(ExceptionMessages.CategoryAlreadyExists);
        }
        
        return categoryRepository.CreateCategory(category);
    }
    
    public Category UpdateCategory(Category category)
    {
        
        if (category == null)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidCategory);
        }
        if (category.Id <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (!categoryRepository.DoesCategoryExist(category.Id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.CategoryNotFound);
        }
        if (string.IsNullOrWhiteSpace(category.Name))
        {
            throw new InvalidArgumentException(ExceptionMessages.RequiredName);
        }
        if (categoryRepository.DoesCategoryExist(category.Name))
        {
            throw new ObjectAlreadyExistsException(ExceptionMessages.CategoryAlreadyExists);
        }
        
        return categoryRepository.UpdateCategory(category);
    }
    
    public bool DeleteCategory(int id)
    {
        if (id < 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (!categoryRepository.DoesCategoryExist(id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.CategoryNotFound);
        }
        
        var category = categoryRepository.GetCategoryById(id);
        return categoryRepository.DeleteCategory(category!);
    }
}