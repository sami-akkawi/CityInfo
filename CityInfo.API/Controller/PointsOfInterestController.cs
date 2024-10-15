using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controller;

[Route("api/cities/{cityId}/pointsofinterest")]
[Authorize]
[ApiController]
public class PointsOfInterestController(
    ILogger<PointsOfInterestController> logger, 
    IMailService mailService,
    ICityInfoRepository repository,
    IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointsOfInterest(int cityId)
    {
        try
        {
            IEnumerable<PointOfInterest> pointOfInterests = await repository.GetPointOfInterestsForCityAsync(cityId);
            if (pointOfInterests.Any() && !await repository.CityExistsAsync(cityId))
            {
                logger.LogInformation($"City with id {cityId} was not found");
                return NotFound();
            }

            return Ok(mapper.Map<IEnumerable<PointOfInterestDto>>(pointOfInterests));
        }
        catch (Exception e)
        {
            logger.LogCritical(e, $"Exception occurred while getting points of interest for the city of id '{cityId}': {e.Message}");
            return StatusCode(500, "An exception occurred while getting points of interest for the city, it's been logged and our support team is on it.");
        }
        
    }

    [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
    public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterest(int cityId, int pointOfInterestId)
    {
        if (!await repository.CityExistsAsync(cityId))
        {
            return NotFound();
        }
        
        PointOfInterest? pointOfInterest = await repository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
        if (pointOfInterest == null)
        {
            return NotFound();
        }

        return Ok(mapper.Map<PointOfInterestDto>(pointOfInterest));
    }

    [HttpPost]
    public async Task<ActionResult<PointOfInterestDto>> CreatePointOfInterest(int cityId, PointOfInterestForCreationDto pointOfInterestForCreation)
    {
        if (!await repository.CityExistsAsync(cityId))
        {
            return NotFound();
        }
        
        PointOfInterest pointOfInterest = mapper.Map<PointOfInterest>(pointOfInterestForCreation);

        await repository.AddPointOfInterestAsync(cityId, pointOfInterest);
        await repository.SaveChangesAsync();
        
        PointOfInterestDto pointOfInterestDto = mapper.Map<PointOfInterestDto>(pointOfInterest);
        
        return CreatedAtRoute("GetPointOfInterest", 
            new {cityId = cityId, pointOfInterestId = pointOfInterestDto.Id}, // created the location header value which includes the url to call the newly created resource
            pointOfInterestDto
            );
    }
    
    [HttpPut("{pointOfInterestId}")]
    public async Task<ActionResult> UpdatePointOfInterest(int cityId, int pointOfInterestId, PointOfInterestForUpdateDto pointOfInterestForUpdate)
    {
        if (!await repository.CityExistsAsync(cityId))
        {
            return NotFound();
        }
        
        PointOfInterest? pointOfInterest = await repository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
        if (pointOfInterest == null)
        {
            return NotFound();
        }
        
        mapper.Map(pointOfInterestForUpdate, pointOfInterest);
        await repository.SaveChangesAsync();
        
        return NoContent();
    }
    
    [HttpPatch("{pointOfInterestId}")]
    public async Task<ActionResult> PartiallyUpdatePointOfInterest(
        int cityId, 
        int pointOfInterestId, 
        JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
    {
        if (!await repository.CityExistsAsync(cityId))
        {
            return NotFound();
        }
        
        PointOfInterest? pointOfInterest = await repository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
        if (pointOfInterest == null)
        {
            return NotFound();
        }
    
        PointOfInterestForUpdateDto pointOfInterestToPatch = mapper.Map<PointOfInterestForUpdateDto>(pointOfInterest);
        patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);
    
        if (!ModelState.IsValid || !TryValidateModel(pointOfInterestToPatch))
        {
            return BadRequest(ModelState);
        }

        mapper.Map(pointOfInterestToPatch, pointOfInterest);
        await repository.SaveChangesAsync();
        
        return NoContent();
    }
    
    [HttpDelete("{pointOfInterestId}")]
    public async Task<ActionResult> DeletePointOfInterest(int cityId, int pointOfInterestId)
    {
        if (!await repository.CityExistsAsync(cityId))
        {
            return NotFound();
        }
        
        PointOfInterest? pointOfInterest = await repository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
        if (pointOfInterest == null)
        {
            return NotFound();
        }
        
        repository.DeletePointOfInterest(pointOfInterest);
        await repository.SaveChangesAsync();
        
        mailService.Send("Point of interest deleted", $"{pointOfInterest.Name} was deleted...");
        
        return NoContent();
    }
}