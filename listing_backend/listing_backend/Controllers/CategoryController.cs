using AutoMapper;
using listing_backend.DTOs;
using listing_backend.Entities;
using listing_backend.Exceptions;
using listing_backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace listing_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController(ICategoryService categoryService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllCategories()
    {
        var categoryDtos = categoryService.GetAllCategories().Select(mapper.Map<CategoryDto>).ToList();
        return Ok(categoryDtos);
    }
    
    [HttpGet("{id:int}")]
    public IActionResult GetCategoryById(int id)
    {
        try
        {
            var categoryDto = mapper.Map<CategoryDto>(categoryService.GetCategoryById(id));
            return Ok(categoryDto);
        }
        catch (InvalidArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (ObjectNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
    
    [HttpPost]
    public IActionResult CreateCategory(CategoryDto categoryDto)
    {
        try
        {
            var inputCategory = mapper.Map<Category>(categoryDto);
            var category = categoryService.CreateCategory(inputCategory);
            var outputCategory = mapper.Map<CategoryDto>(category);
            return Ok(outputCategory);
        }
        catch (InvalidArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (ObjectAlreadyExistsException e)
        {
            return Conflict(e.Message);
        }
    }
    
    [HttpPut]
    public IActionResult UpdateCategory(CategoryDto categoryDto)
    {
        try
        {
            var inputCategory = mapper.Map<Category>(categoryDto);
            var category = categoryService.UpdateCategory(inputCategory);
            var outputCategory = mapper.Map<CategoryDto>(category);
            return Ok(outputCategory);
        }
        catch (InvalidArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (ObjectNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (ObjectAlreadyExistsException e)
        {
            return Conflict(e.Message);
        }
    }
    
    [HttpDelete("{id:int}")]
    public IActionResult DeleteCategory(int id)
    {
        try
        {
            return Ok(categoryService.DeleteCategory(id));
        }
        catch (InvalidArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (ObjectNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}