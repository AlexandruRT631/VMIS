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
    public IActionResult GetAllListings(int pageIndex = 1, int pageSize = 1)
    {
        var (listings, totalPages) = listingService.GetAllListings(pageIndex, pageSize);
        var listingDtos = listings
            .Select(mapper.Map<ListingDto>)
            .ToList();
        return Ok(new
        {
            Listings = listingDtos,
            TotalPages = totalPages
        });
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
    public IActionResult UpdateListing([FromForm] string listingDto, [FromForm] List<IFormFile>? images)
    {
        try
        {
            var listingDtoObject = JsonSerializer.Deserialize<ListingDto>(listingDto);
            var inputListing = mapper.Map<Listing>(listingDtoObject);
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
    
    [HttpPost("search")]
    public IActionResult GetListingsBySearch([FromForm] ListingSearchDto listingSearchDto,[FromForm] int pageIndex = 1,[FromForm] int pageSize = 1)
    {
        try
        {
            var (listings, totalPages) = listingService
                .GetListingsBySearch(listingSearchDto, pageIndex, pageSize);
            var listingDtos = listings
                .Select(mapper.Map<ListingDto>)
                .ToList();
            return Ok(new
            {
                Listings = listingDtos,
                TotalPages = totalPages
            });
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
    
    [HttpGet("active/{sellerId:int}")]
    public IActionResult GetActiveListingsByUserId(int sellerId, int pageIndex = 1, int pageSize = 1)
    {
        try
        {
            var (listings, totalPages) = listingService
                .GetActiveListingsByUserId(sellerId, pageIndex, pageSize);
            var listingDtos = listings
                .Select(mapper.Map<ListingDto>)
                .ToList();
            return Ok(new
            {
                Listings = listingDtos,
                TotalPages = totalPages
            });
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
    
    [HttpGet("inactive/{sellerId:int}")]
    public IActionResult GetInactiveListingsByUserId(int sellerId, int pageIndex = 1, int pageSize = 1)
    {
        try
        {
            var (listings, totalPages) = listingService
                .GetInactiveListingsByUserId(sellerId, pageIndex, pageSize);
            var listingDtos = listings
                .Select(mapper.Map<ListingDto>)
                .ToList();
            return Ok(new
            {
                Listings = listingDtos,
                TotalPages = totalPages
            });
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
    
    [HttpPost("ids")]
    public IActionResult GetListingsByIds(List<int> ids, int pageIndex = 1, int pageSize = 1)
    {
        try
        {
            var (listings, totalPages) = listingService
                .GetListingsByIds(ids, pageIndex, pageSize);
            var listingDtos = listings
                .Select(mapper.Map<ListingDto>)
                .ToList();
            return Ok(new
            {
                Listings = listingDtos,
                TotalPages = totalPages
            });
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