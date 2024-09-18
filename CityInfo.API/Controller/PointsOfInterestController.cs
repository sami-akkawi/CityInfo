using CityInfo.API.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controller;

[Route("api/cities/{cityId}/pointsofinterest")]
[ApiController]
public class PointsOfInterestController(ILogger<PointsOfInterestController> logger) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterest(int cityId)
    {
        try
        {
            CityDto? city = CitiesDataStore.Current.Cities.FirstOrDefault(city => city.Id == cityId);
            if (city == null)
            {
                logger.LogInformation($"City with id {cityId} was not found");
                return NotFound();
            }

            return Ok(city.PointsOfInterest);
        }
        catch (Exception e)
        {
            logger.LogCritical(e, $"Exception occurred while getting points of interest for the city of id '{cityId}': {e.Message}");
            return StatusCode(500, "An exception occurred while getting points of interest for the city, it's been logged and our support team is on it.");
        }
        
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
    public ActionResult<PointOfInterestDto> UpdatePointOfInterest(int cityId, int pointOfInterestId, PointOfInterestForUpdateDto pointOfInterest)
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

    [HttpPatch("{pointOfInterestId}")]
    public ActionResult<PointOfInterestDto> PartiallyUpdatePointOfInterest(
        int cityId, 
        int pointOfInterestId, 
        JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
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

        PointOfInterestForUpdateDto pointOfInterestToPatch = new()
        {
            Name = pointOfInterestDto.Name,
            Description = pointOfInterestDto.Description,
        };
        patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

        if (!ModelState.IsValid || !TryValidateModel(pointOfInterestToPatch))
        {
            return BadRequest(ModelState);
        }
        
        pointOfInterestDto.Name = pointOfInterestToPatch.Name;
        pointOfInterestDto.Description = pointOfInterestToPatch.Description;
        
        return NoContent();
    }

    [HttpDelete("{pointOfInterestId}")]
    public ActionResult DeletePointOfInterest(int cityId, int pointOfInterestId)
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
        
        city.PointsOfInterest.Remove(pointOfInterestDto);
        
        return NoContent();
    }
}