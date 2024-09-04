using CityInfo.API.Models;

namespace CityInfo.API;

public class CitiesDataStore
{
    public List<CityDto> Cities { get; set; } =
    [
        new CityDto()
        {
            Id = 1,
            Name = "New York City",
            Description = "The one with that big park.",
        },

        new CityDto()
        {
            Id = 2,
            Name = "Antwerp",
            Description = "The one with the cathedral that was never really finished.",
        },

        new CityDto()
        {
            Id = 3,
            Name = "Paris",
            Description = "The one with that big tower.",
        }
    ];

    public static CitiesDataStore Current { get; } = new CitiesDataStore();
}