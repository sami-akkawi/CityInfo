using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.DbContexts;

public class CityInfoContext: DbContext
{
    public DbSet<City> Cities { get; set; }
    
    public DbSet<PointOfInterest> PointOfInterests { get; set; }
}