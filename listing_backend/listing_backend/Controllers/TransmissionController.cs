using AutoMapper;
using listing_backend.DTOs;
using listing_backend.Entities;
using listing_backend.Exceptions;
using listing_backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace listing_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransmissionController(ITransmissionService transmissionService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllTransmissions()
    {
        var transmissionDtos = transmissionService.GetAllTransmissions().Select(mapper.Map<TransmissionDto>).ToList();
        return Ok(transmissionDtos);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetTransmissionById(int id)
    {
        try
        {
            var transmissionDto = mapper.Map<TransmissionDto>(transmissionService.GetTransmissionById(id));
            return Ok(transmissionDto);
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
    public IActionResult CreateTransmission(TransmissionDto transmissionDto)
    {
        try
        {
            var inputTransmission = mapper.Map<Transmission>(transmissionDto);
            var transmission = transmissionService.CreateTransmission(inputTransmission);
            var outputTransmission = mapper.Map<TransmissionDto>(transmission);
            return Ok(outputTransmission);
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
    public IActionResult UpdateTransmission(TransmissionDto transmissionDto)
    {
        try
        {
            var inputTransmission = mapper.Map<Transmission>(transmissionDto);
            var transmission = transmissionService.UpdateTransmission(inputTransmission);
            var outputTransmission = mapper.Map<TransmissionDto>(transmission);
            return Ok(outputTransmission);
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
    public IActionResult DeleteTransmission(int id)
    {
        try
        {
            return Ok(transmissionService.DeleteTransmission(id));
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