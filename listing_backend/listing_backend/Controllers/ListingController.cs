using System.Text.Json;
using AutoMapper;
using listing_backend.DTOs;
using listing_backend.Entities;
using listing_backend.Exceptions;
using listing_backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace listing_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ListingController(IListingService listingService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllListings()
    {
        var listingDtos = listingService.GetAllListings().Select(mapper.Map<ListingDto>).ToList();
        return Ok(listingDtos);
    }
    
    [HttpGet("{id:int}")]
    public IActionResult GetListingById(int id)
    {
        try
        {
            var listingDto = mapper.Map<ListingDto>(listingService.GetListingById(id));
            return Ok(listingDto);
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
    public IActionResult CreateListing([FromForm] string listingDto, [FromForm] List<IFormFile> images)
    {
        try
        {
            var listingDtoObject = JsonSerializer.Deserialize<ListingDto>(listingDto);
            var inputListing = mapper.Map<Listing>(listingDtoObject);
            var listing = listingService.CreateListing(inputListing, images);
            var outputListing = mapper.Map<ListingDto>(listing);
            return Ok(outputListing);
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
    
    [HttpPut]
    public IActionResult UpdateListing([FromForm] ListingDto listingDto, [FromForm] List<IFormFile> images)
    {
        try
        {
            var inputListing = mapper.Map<Listing>(listingDto);
            var listing = listingService.UpdateListing(inputListing, images);
            var outputListing = mapper.Map<ListingDto>(listing);
            return Ok(outputListing);
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
    public IActionResult DeleteListing(int id)
    {
        try
        {
            return Ok(listingService.DeleteListing(id));
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