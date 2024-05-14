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
    public IActionResult CreateListing(ListingDto listingDto)
    {
        try
        {
            var inputListing = mapper.Map<Listing>(listingDto);
            var listing = listingService.CreateListing(inputListing);
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
    public IActionResult UpdateListing(ListingDto listingDto)
    {
        try
        {
            var inputListing = mapper.Map<Listing>(listingDto);
            var listing = listingService.UpdateListing(inputListing);
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