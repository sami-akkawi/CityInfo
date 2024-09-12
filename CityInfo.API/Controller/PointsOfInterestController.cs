using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controller;

[Route("api/cities/{cityId}/pointsofinterest")]
[ApiController]
public class PointsOfInterestController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterest(int cityId)
    {
        CityDto? city = CitiesDataStore.Current.Cities.FirstOrDefault(city => city.Id == cityId);
        if (city == null)
        {
            return NotFound();
        }
        
        return Ok(city.PointsOfInterest);
    }

    [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
    public ActionResult<PointOfInterestDto> GetPointOfInterest(int cityId, int pointOfInterestId)
    {
        CityDto? city = CitiesDataStore.Current.Cities.FirstOrDefault(city => city.Id == cityId);
        if (city == null)
        {
            return NotFound();
        }
        
        PointOfInterestDto? pointOfInterest = city.PointsOfInterest.FirstOrDefault(point => point.Id == pointOfInterestId);
        if (pointOfInterest == null)
        {
            return NotFound();
        }

        return Ok(pointOfInterest);
    }

    [HttpPost]
    public ActionResult<PointOfInterestDto> CreatePointOfInterest(int cityId, PointOfInterestForCreationDto pointOfInterest)
    {
        CityDto? city = CitiesDataStore.Current.Cities.FirstOrDefault(city => city.Id == cityId);
        if (city == null)
        {
            return NotFound();
        }
        
        int maxPointOfInterestId = CitiesDataStore.Current.Cities.SelectMany(c => c.PointsOfInterest).Max(p => p.Id);

        PointOfInterestDto pointOfInterestDto = new()
        {
            Id = maxPointOfInterestId + 1,
            Name = pointOfInterest.Name,
            Description = pointOfInterest.Description,
        };
        
        city.PointsOfInterest.Add(pointOfInterestDto);

        return CreatedAtRoute("GetPointOfInterest", 
            new {cityId = cityId, pointOfInterestId = pointOfInterestDto.Id}, // created the location header value which includes the url to call the newly created resource
            pointOfInterestDto
            );
    }

    [HttpPut("{pointOfInterestId}")]
    public ActionResult<PointOfInterestDto> UpdatePointOfInterest(int cityId, int pointOfInterestId, PointOfInterestForCreationDto pointOfInterest)
    {
        CityDto? city = CitiesDataStore.Current.Cities.FirstOrDefault(city => city.Id == cityId);
        if (city == null)
        {
            return NotFound();
        }
        
        PointOfInterestDto? pointOfInterestDto = city.PointsOfInterest.FirstOrDefault(point => point.Id == pointOfInterestId);
        if (pointOfInterestDto == null)
        {
            return NotFound();
        }
        
        pointOfInterestDto.Name = pointOfInterest.Name;
        pointOfInterestDto.Description = pointOfInterest.Description;
        
        return NoContent();
    }
}