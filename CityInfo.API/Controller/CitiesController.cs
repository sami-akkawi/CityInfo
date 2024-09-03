using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controller;

[ApiController]
[Route("api/cities")]
public class CitiesController : ControllerBase
{
    [HttpGet]
    public JsonResult GetCities()
    {
        return new JsonResult(new List<object>
        {
            new {id = 1, Name = "Zurich"},
            new {id = 2, Name = "Berlin"}
        });
    }
}