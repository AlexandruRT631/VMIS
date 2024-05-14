using AutoMapper;
using listing_backend.DTOs;
using listing_backend.Entities;
using listing_backend.Exceptions;
using listing_backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace listing_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TractionController(ITractionService tractionService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllTractions()
    {
        var tractionDtos = tractionService.GetAllTractions().Select(mapper.Map<TractionDto>).ToList();
        return Ok(tractionDtos);
    }
    
    [HttpGet("{id:int}")]
    public IActionResult GetTractionById(int id)
    {
        try
        {
            var tractionDto = mapper.Map<TractionDto>(tractionService.GetTractionById(id));
            return Ok(tractionDto);
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
    public IActionResult CreateTraction(TractionDto tractionDto)
    {
        try
        {
            var inputTraction = mapper.Map<Traction>(tractionDto);
            var traction = tractionService.CreateTraction(inputTraction);
            var outputTraction = mapper.Map<TractionDto>(traction);
            return Ok(outputTraction);
        }
        catch (InvalidArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (ObjectAlreadyExistsException e)
        {
            return Conflict(e.Message);
        }
        catch (ObjectNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
    
    [HttpPut]
    public IActionResult UpdateTraction(TractionDto tractionDto)
    {
        try
        {
            var inputTraction = mapper.Map<Traction>(tractionDto);
            var traction = tractionService.UpdateTraction(inputTraction);
            var outputTraction = mapper.Map<TractionDto>(traction);
            return Ok(outputTraction);
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
    public IActionResult DeleteTraction(int id)
    {
        try
        {
            return Ok(tractionService.DeleteTraction(id));
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