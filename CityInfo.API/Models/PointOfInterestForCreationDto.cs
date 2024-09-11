using System.ComponentModel.DataAnnotations;

namespace CityInfo.API.Models;

public class PointOfInterestForCreationDto
{
    [Required(ErrorMessage = "field_is_required")]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(200)]
    public string? Description { get; set; }
}