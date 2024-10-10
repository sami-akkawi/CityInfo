using CityInfo.API.DbContexts;
using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Services;

public class CityInfoRepository(CityInfoContext context): ICityInfoRepository
{
    public async Task<IEnumerable<City>> GetCitiesAsync()
    {
        return await context.Cities.OrderBy(c => c.Name).ToListAsync();
    }

    public async Task<IEnumerable<City>> GetCitiesAsync(string? name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return await GetCitiesAsync();
        }

        name = name.Trim();
        return await context.Cities
            .Where(c => c.Name == name)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<City?> GetCityAsync(int cityId, bool includePointsOfInterests)
    {
        if (includePointsOfInterests)
        {
            return await context.Cities.Include(c => c.PointOfInterests)
                .FirstOrDefaultAsync(c => c.Id == cityId);
        }
        
        return await context.Cities.FirstOrDefaultAsync(c => c.Id == cityId);
    }

    public async Task<bool> CityExistsAsync(int cityId)
    {
        return await context.Cities.AnyAsync(c => c.Id == cityId);
    }

    public async Task<IEnumerable<PointOfInterest>> GetPointOfInterestsForCityAsync(int cityId)
    {
        return await context.PointOfInterests.Where(p => p.CityId == cityId).ToListAsync();
    }

    public async Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId)
    {
        return await context.PointOfInterests
            .FirstOrDefaultAsync(p => p.CityId == cityId && p.Id == pointOfInterestId);
    }

    public void DeletePointOfInterest(PointOfInterest pointOfInterest)
    {
        context.PointOfInterests.Remove(pointOfInterest);
    }

    public async Task AddPointOfInterestAsync(int cityId, PointOfInterest pointOfInterest)
    {
        City city = await GetCityAsync(cityId, false);
        if (city != null)
        {
            city.PointOfInterests.Add(pointOfInterest);
        }
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await context.SaveChangesAsync() >= 0;
    }
}