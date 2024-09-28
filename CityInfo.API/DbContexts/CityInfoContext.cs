using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.DbContexts;

public class CityInfoContext(DbContextOptions options): DbContext(options: options)
{
    public DbSet<City> Cities { get; set; }
    
    public DbSet<PointOfInterest> PointOfInterests { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>()
            .HasData(
                new("New York City")
                {
                    Id = 1,
                    Description = "The one with that big park."
                },
                new("Antwerp")
                {
                    Id = 2,
                    Description = "The one with the cathedral that was never really finished."
                },
                new("Paris")
                {
                    Id = 3,
                    Description = "The one with that big tower."
                });

            modelBuilder.Entity<PointOfInterest>()
                .HasData(
                    new("Central Park") 
                    { 
                        Id = 1, 
                        CityId = 1, 
                        Description = "The most visited urban park in the United States." 
                    }, 
                    new("Empire State Building") 
                    { 
                        Id = 2,
                        CityId = 1,
                        Description = "A 102-story skyscraper located in Midtown Manhattan."
                    },
                    new("Cathedral")
                    {
                        Id = 3,
                        CityId = 2,
                        Description = "A Gothic style cathedral, conceived by architects Jan and Pieter Appelmans."
                    },
                    new("Antwerp Central Station")
                    {
                        Id = 4,
                        CityId = 2,
                        Description = "The the finest example of railway architecture in Belgium."
                    },
                    new("Eiffel Tower") 
                    {
                        Id = 5,
                        CityId = 3,
                        Description = "A wrought iron lattice tower on the Champ de Mars, named after engineer Gustave Eiffel."
                    },
                    new("The Louvre")
                    { 
                        Id = 6,
                        CityId = 3,
                        Description = "The world's largest museum."
                    }
               );
        
        base.OnModelCreating(modelBuilder);
    }
}