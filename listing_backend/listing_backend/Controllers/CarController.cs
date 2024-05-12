using AutoMapper;
using listing_backend.DTOs;
using listing_backend.Entities;
using listing_backend.Exceptions;
using listing_backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace listing_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarController(ICarService carService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllCars()
    {
        var carDtos = carService.GetAllCars().Select(mapper.Map<CarDto>).ToList();
        return Ok(carDtos);
    }
    
    [HttpGet("{id:int}")]
    public IActionResult GetCarById(int id)
    {
        try
        {
            var carDto = mapper.Map<CarDto>(carService.GetCarById(id));
            return Ok(carDto);
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
    public IActionResult CreateCar(CarDto carDto)
    {
        try
        {
            var inputCar = mapper.Map<Car>(carDto);
            var car = carService.CreateCar(inputCar);
            var outputCar = mapper.Map<CarDto>(car);
            return Ok(outputCar);
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
    public IActionResult UpdateCar(CarDto carDto)
    {
        try
        {
            var inputCar = mapper.Map<Car>(carDto);
            var car = carService.UpdateCar(inputCar);
            var outputCar = mapper.Map<CarDto>(car);
            return Ok(outputCar);
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
    public IActionResult DeleteCar(int id)
    {
        try
        {
            return Ok(carService.DeleteCar(id));
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