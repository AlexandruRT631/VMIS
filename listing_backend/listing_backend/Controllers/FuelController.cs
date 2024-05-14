using AutoMapper;
using listing_backend.DTOs;
using listing_backend.Entities;
using listing_backend.Exceptions;
using listing_backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace listing_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FuelController(IFuelService fuelService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllFuels()
    {
        var fuelDtos = fuelService.GetAllFuels().Select(mapper.Map<FuelDto>).ToList();
        return Ok(fuelDtos);
    }
    
    [HttpGet("{id:int}")]
    public IActionResult GetFuelById(int id)
    {
        try
        {
            var fuelDto = mapper.Map<FuelDto>(fuelService.GetFuelById(id));
            return Ok(fuelDto);
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
    public IActionResult CreateFuel(FuelDto fuelDto)
    {
        try
        {
            var inputFuel = mapper.Map<Fuel>(fuelDto);
            var fuel = fuelService.CreateFuel(inputFuel);
            var outputFuel = mapper.Map<FuelDto>(fuel);
            return Ok(outputFuel);
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
    public IActionResult UpdateFuel(FuelDto fuelDto)
    {
        try
        {
            var inputFuel = mapper.Map<Fuel>(fuelDto);
            var fuel = fuelService.UpdateFuel(inputFuel);
            var outputFuel = mapper.Map<FuelDto>(fuel);
            return Ok(outputFuel);
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
    public IActionResult DeleteFuel(int id)
    {
        try
        {
            return Ok(fuelService.DeleteFuel(id));
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