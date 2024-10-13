using CityInfo.API.Entities;

namespace CityInfo.API.Services;

public interface ICityInfoRepository
{
    Task<(IEnumerable<City>, PaginationMetaData)> GetCitiesAsync(string? name, string? searchQuery, int pageNumber, int pageSize);
    
    Task<City?> GetCityAsync(int cityId, bool includePointsOfInterests);

    Task<bool> CityExistsAsync(int cityId);
    
    Task<IEnumerable<PointOfInterest>> GetPointOfInterestsForCityAsync(int cityId);
    
    Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId);
    
    void DeletePointOfInterest(PointOfInterest pointOfInterest);
    
    Task AddPointOfInterestAsync(int cityId, PointOfInterest pointOfInterest);

    Task<bool> SaveChangesAsync();
}