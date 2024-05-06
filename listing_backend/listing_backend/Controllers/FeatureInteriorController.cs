using AutoMapper;
using listing_backend.DTOs;
using listing_backend.Entities;
using listing_backend.Exceptions;
using listing_backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace listing_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FeatureInteriorController(IFeatureInteriorService featureInteriorService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllFeaturesInterior()
    {
        var featureInteriorDtos = featureInteriorService.GetAllFeaturesInterior().Select(mapper.Map<FeatureInteriorDto>).ToList();
        return Ok(featureInteriorDtos);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetFeatureInteriorById(int id)
    {
        try
        {
            var featureInteriorDto = mapper.Map<FeatureInteriorDto>(featureInteriorService.GetFeatureInteriorById(id));
            return Ok(featureInteriorDto);
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
    public IActionResult CreateFeatureInterior(FeatureInteriorDto featureInteriorDto)
    {
        try
        {
            var inputFeatureInterior = mapper.Map<FeatureInterior>(featureInteriorDto);
            var featureInterior = featureInteriorService.CreateFeatureInterior(inputFeatureInterior);
            var outputFeatureInterior = mapper.Map<FeatureInteriorDto>(featureInterior);
            return Ok(outputFeatureInterior);
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
    public IActionResult UpdateFeatureInterior(FeatureInteriorDto featureInteriorDto)
    {
        try
        {
            var inputFeatureInterior = mapper.Map<FeatureInterior>(featureInteriorDto);
            var featureInterior = featureInteriorService.UpdateFeatureInterior(inputFeatureInterior);
            var outputFeatureInterior = mapper.Map<FeatureInteriorDto>(featureInterior);
            return Ok(outputFeatureInterior);
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
    public IActionResult DeleteFeatureInterior(int id)
    {
        try
        {
            return Ok(featureInteriorService.DeleteFeatureInterior(id));
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