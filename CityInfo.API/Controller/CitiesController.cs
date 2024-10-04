using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controller;

[ApiController]
[Route("api/cities")]
public class CitiesController(ICityInfoRepository repository, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities()
    {
        IEnumerable<City> cities = await repository.GetCitiesAsync();
        
        return Ok(mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cities));
    }
    
    // [HttpGet("{id}")]
    // public ActionResult<CityDto> GetCity(int id)
    // {
    //     CityDto? city = citiesDataStore.Cities.FirstOrDefault(c => c.Id == id);
    //     if (city == null)
    //     {
    //         return NotFound();
    //     }
    //     
    //     return Ok(city);
    // }
}