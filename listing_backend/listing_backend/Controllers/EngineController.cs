using AutoMapper;
using listing_backend.DTOs;
using listing_backend.Entities;
using listing_backend.Exceptions;
using listing_backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace listing_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EngineController(IEngineService engineService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllEngines()
    {
        var engineDtos = engineService.GetAllEngines().Select(mapper.Map<EngineDto>).ToList();
        return Ok(engineDtos);
    }
    
    [HttpGet("{id:int}")]
    public IActionResult GetEngineById(int id)
    {
        try
        {
            var engineDto = mapper.Map<EngineDto>(engineService.GetEngineById(id));
            return Ok(engineDto);
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
    public IActionResult CreateEngine(EngineDto engineDto)
    {
        try
        {
            var inputEngine = mapper.Map<Engine>(engineDto);
            var engine = engineService.CreateEngine(inputEngine);
            var outputEngine = mapper.Map<EngineDto>(engine);
            return Ok(outputEngine);
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
    public IActionResult UpdateEngine(EngineDto engineDto)
    {
        try
        {
            var inputEngine = mapper.Map<Engine>(engineDto);
            var engine = engineService.UpdateEngine(inputEngine);
            var outputEngine = mapper.Map<EngineDto>(engine);
            return Ok(outputEngine);
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
    public IActionResult DeleteEngine(int id)
    {
        try
        {
            return Ok(engineService.DeleteEngine(id));
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