using AutoMapper;
using listing_backend.DTOs;
using listing_backend.Entities;
using listing_backend.Exceptions;
using listing_backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace listing_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ModelController(IModelService modelService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllModels()
    {
        var modelDtos = modelService.GetAllModels().Select(mapper.Map<ModelDto>).ToList();
        return Ok(modelDtos);
    }
    
    [HttpGet("{id:int}")]
    public IActionResult GetModelById(int id)
    {
        try
        {
            var modelDto = mapper.Map<ModelDto>(modelService.GetModelById(id));
            return Ok(modelDto);
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
    public IActionResult CreateModel(ModelDto modelDto)
    {
        try
        {
            var inputModel = mapper.Map<Model>(modelDto);
            var model = modelService.CreateModel(inputModel);
            var outputModel = mapper.Map<ModelDto>(model);
            return Ok(outputModel);
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
    public IActionResult UpdateModel(ModelDto modelDto)
    {
        try
        {
            var inputModel = mapper.Map<Model>(modelDto);
            var model = modelService.UpdateModel(inputModel);
            var outputModel = mapper.Map<ModelDto>(model);
            return Ok(outputModel);
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
    
    [HttpDelete("{id:int}")]
    public IActionResult DeleteModel(int id)
    {
        try
        {
            return Ok(modelService.DeleteModel(id));
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