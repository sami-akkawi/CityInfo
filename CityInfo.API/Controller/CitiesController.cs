using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace CityInfo.API.Controller;

[ApiController]
[Route("api/cities")]
public class CitiesController(ICityInfoRepository repository, IMapper mapper) : ControllerBase
{
    private const int maxCitiesPageSize = 20;
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities(
        [FromQuery(Name = "name")] string? name,
        [FromQuery(Name = "searchQuery")] string? searchQuery,
        int pageNumber = 1,
        int pageSize = 10
        )
    {
        if (pageSize > maxCitiesPageSize)
        {
            pageSize = maxCitiesPageSize;
        }

        (IEnumerable<City> cities, PaginationMetaData paginationMetaData) = await repository.GetCitiesAsync(name, searchQuery, pageNumber, pageSize);
        
        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetaData));
        
        return Ok(mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cities));
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCity(int id, bool includePointsOfInterest = false)
    {
        City? city = await repository.GetCityAsync(id, includePointsOfInterest);
        if (city == null)
        {
            return NotFound();
        }

        if (includePointsOfInterest)
        {
            return Ok(mapper.Map<CityDto>(city));
        }
        
        return Ok(mapper.Map<CityWithoutPointsOfInterestDto>(city));
    }
}