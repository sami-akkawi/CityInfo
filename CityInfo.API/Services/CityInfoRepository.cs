﻿using CityInfo.API.DbContexts;
using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Services;

public class CityInfoRepository(CityInfoContext context): ICityInfoRepository
{
    public async Task<IEnumerable<City>> GetCitiesAsync()
    {
        return await context.Cities.OrderBy(c => c.Name).ToListAsync();
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

    public async Task<IEnumerable<PointOfInterest>> GetPointOfInterestsForCityAsync(int cityId)
    {
        return await context.PointOfInterests.Where(p => p.CityId == cityId).ToListAsync();
    }

    public async Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId)
    {
        return await context.PointOfInterests
            .FirstOrDefaultAsync(p => p.CityId == cityId && p.Id == pointOfInterestId);
    }
}