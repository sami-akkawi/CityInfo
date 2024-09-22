using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controller;

[ApiController]
[Route("api/cities")]
public class CitiesController(CitiesDataStore citiesDataStore) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<CityDto>> GetCities()
    {
        return Ok(citiesDataStore.Cities);
    }
    
    [HttpGet("{id}")]
    public ActionResult<CityDto> GetCity(int id)
    {
        CityDto? city = citiesDataStore.Cities.FirstOrDefault(c => c.Id == id);
        if (city == null)
        {
            return NotFound();
        }
        
        return Ok(city);
    }
}