using CityInfo.API.Entities;

namespace CityInfo.API.Services;

public interface ICityInfoRepository
{
    Task<IEnumerable<City>> GetCitiesAsync();
    
    Task<City?> GetCityAsync(int cityId, bool includePointsOfInterests);

    Task<bool> CityExistsAsync(int cityId);
    
    Task<IEnumerable<PointOfInterest>> GetPointOfInterestsForCityAsync(int cityId);
    
    Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId);
    
    Task AddPointOfInterestAsync(int cityId, PointOfInterest pointOfInterest);

    Task<bool> SaveChangesAsync();
}