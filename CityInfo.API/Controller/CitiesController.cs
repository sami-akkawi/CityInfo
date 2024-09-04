using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controller;

[ApiController]
[Route("api/cities")]
public class CitiesController : ControllerBase
{
    [HttpGet]
    public JsonResult GetCities()
    {
        return new JsonResult(CitiesDataStore.Current.Cities);
    }
    
    [HttpGet("{id}")]
    public JsonResult GetCity(int id)
    {
        CityDto city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == id);
        if (city == null)
        {
            // todo...
        }
        
        return new JsonResult(city);
    }
}