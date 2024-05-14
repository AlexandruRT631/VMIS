using AutoMapper;
using listing_backend.DTOs;
using listing_backend.Entities;
using listing_backend.Exceptions;
using listing_backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace listing_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoorTypeController(IDoorTypeService doorTypeService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllDoorTypes()
    {
        var doorTypeDtos = doorTypeService.GetAllDoorTypes().Select(mapper.Map<DoorTypeDto>).ToList();
        return Ok(doorTypeDtos);
    }
    
    [HttpGet("{id:int}")]
    public IActionResult GetDoorTypeById(int id)
    {
        try
        {
            var doorTypeDto = mapper.Map<DoorTypeDto>(doorTypeService.GetDoorTypeById(id));
            return Ok(doorTypeDto);
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
    public IActionResult CreateDoorType(DoorTypeDto doorTypeDto)
    {
        try
        {
            var inputDoorType = mapper.Map<DoorType>(doorTypeDto);
            var doorType = doorTypeService.CreateDoorType(inputDoorType);
            var outputDoorType = mapper.Map<DoorTypeDto>(doorType);
            return Ok(outputDoorType);
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
    public IActionResult UpdateDoorType(DoorTypeDto doorTypeDto)
    {
        try
        {
            var inputDoorType = mapper.Map<DoorType>(doorTypeDto);
            var doorType = doorTypeService.UpdateDoorType(inputDoorType);
            var outputDoorType = mapper.Map<DoorTypeDto>(doorType);
            return Ok(outputDoorType);
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
    public IActionResult DeleteDoorType(int id)
    {
        try
        {
            return Ok(doorTypeService.DeleteDoorType(id));
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