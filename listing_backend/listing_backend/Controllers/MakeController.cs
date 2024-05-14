using AutoMapper;
using listing_backend.DTOs;
using listing_backend.Entities;
using listing_backend.Exceptions;
using listing_backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace listing_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MakeController(IMakeService makeService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllMakes()
    {
        var makeDtos = makeService.GetAllMakes().Select(mapper.Map<MakeDto>).ToList();
        return Ok(makeDtos);
    }
    
    [HttpGet("{id:int}")]
    public IActionResult GetMakeById(int id)
    {
        try
        {
            var makeDto = mapper.Map<MakeDto>(makeService.GetMakeById(id));
            return Ok(makeDto);
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
    public IActionResult CreateMake(MakeDto makeDto)
    {
        try
        {
            var inputMake = mapper.Map<Make>(makeDto);
            var make = makeService.CreateMake(inputMake);
            var outputMake = mapper.Map<MakeDto>(make);
            return Ok(outputMake);
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
    public IActionResult UpdateMake(MakeDto makeDto)
    {
        try
        {
            var inputMake = mapper.Map<Make>(makeDto);
            var make = makeService.UpdateMake(inputMake);
            var outputMake = mapper.Map<MakeDto>(make);
            return Ok(outputMake);
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
    public IActionResult DeleteMake(int id)
    {
        try
        {
            return Ok(makeService.DeleteMake(id));
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