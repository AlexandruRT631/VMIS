using AutoMapper;
using listing_backend.DTOs;
using listing_backend.Entities;
using listing_backend.Exceptions;
using listing_backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace listing_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ColorController(IColorService colorService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllColors()
    {
        var colorDtOs = colorService.GetAllColors().Select(mapper.Map<ColorDto>).ToList();
        return Ok(colorDtOs);
    }
    
    [HttpGet("{id:int}")]
    public IActionResult GetColorById(int id)
    {
        try
        {
            var colorDto = mapper.Map<ColorDto>(colorService.GetColorById(id));
            return Ok(colorDto);
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
    public IActionResult CreateColor(ColorDto colorDto)
    {
        try
        {
            var inputColor = mapper.Map<Color>(colorDto);
            var color = colorService.CreateColor(inputColor);
            var outputColor = mapper.Map<ColorDto>(color);
            return Ok(outputColor);
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
    public IActionResult UpdateColor(ColorDto colorDto)
    {
        try
        {
            var inputColor = mapper.Map<Color>(colorDto);
            var color = colorService.UpdateColor(inputColor);
            var outputColor = mapper.Map<ColorDto>(color);
            return Ok(outputColor);
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
    public IActionResult DeleteColor(int id)
    {
        try
        {
            return Ok(colorService.DeleteColor(id));
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