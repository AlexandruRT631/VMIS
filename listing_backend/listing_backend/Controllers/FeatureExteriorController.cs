using AutoMapper;
using listing_backend.DTOs;
using listing_backend.Entities;
using listing_backend.Exceptions;
using listing_backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace listing_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FeatureExteriorController(IFeatureExteriorService featureExteriorService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllFeaturesExterior()
    {
        var featureExteriorDtos = featureExteriorService.GetAllFeaturesExterior().Select(mapper.Map<FeatureExteriorDto>).ToList();
        return Ok(featureExteriorDtos);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetFeatureExteriorById(int id)
    {
        try
        {
            var featureExteriorDto = mapper.Map<FeatureExteriorDto>(featureExteriorService.GetFeatureExteriorById(id));
            return Ok(featureExteriorDto);
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
    public IActionResult CreateFeatureExterior(FeatureExteriorDto featureExteriorDto)
    {
        try
        {
            var inputFeatureExterior = mapper.Map<FeatureExterior>(featureExteriorDto);
            var featureExterior = featureExteriorService.CreateFeatureExterior(inputFeatureExterior);
            var outputFeatureExterior = mapper.Map<FeatureExteriorDto>(featureExterior);
            return Ok(outputFeatureExterior);
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
    public IActionResult UpdateFeatureExterior(FeatureExteriorDto featureExteriorDto)
    {
        try
        {
            var inputFeatureExterior = mapper.Map<FeatureExterior>(featureExteriorDto);
            var featureExterior = featureExteriorService.UpdateFeatureExterior(inputFeatureExterior);
            var outputFeatureExterior = mapper.Map<FeatureExteriorDto>(featureExterior);
            return Ok(outputFeatureExterior);
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
    public IActionResult DeleteFeatureExterior(int id)
    {
        try
        {
            return Ok(featureExteriorService.DeleteFeatureExterior(id));
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