using AutoMapper;
using listing_backend.DTOs;
using listing_backend.Entities;
using listing_backend.Exceptions;
using listing_backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace listing_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FeatureController(IFeatureService featureService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllFeatures()
    {
        var featureDtos = featureService.GetAllFeatures().Select(mapper.Map<FeatureDto>).ToList();
        return Ok(featureDtos);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetFeatureById(int id)
    {
        try
        {
            var featureDto = mapper.Map<FeatureDto>(featureService.GetFeatureById(id));
            return Ok(featureDto);
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
    public IActionResult CreateFeature(FeatureDto featureDto)
    {
        try
        {
            var inputFeature = mapper.Map<Feature>(featureDto);
            var feature = featureService.CreateFeature(inputFeature);
            var outputFeature = mapper.Map<FeatureDto>(feature);
            return Ok(outputFeature);
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
    public IActionResult UpdateFeature(FeatureDto featureDto)
    {
        try
        {
            var inputFeature = mapper.Map<Feature>(featureDto);
            var feature = featureService.UpdateFeature(inputFeature);
            var outputFeature = mapper.Map<FeatureDto>(feature);
            return Ok(outputFeature);
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
    public IActionResult DeleteFeature(int id)
    {
        try
        {
            return Ok(featureService.DeleteFeature(id));
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