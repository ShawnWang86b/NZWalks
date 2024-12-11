using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO;

public class UpdateRegionDto
{   
    [Required]
    [MinLength(3,ErrorMessage = "Code has to be a minimum length of 3")]
    [MaxLength(3, ErrorMessage = "Code has to be a maximum length of 3")]
    public string Code { get; set; }
    
    [Required]
    [MaxLength(100, ErrorMessage = "Name has to be a maximum length of 100")]
    public string Name { get; set; }
    
    public string? RegionImageUrl { get; set; }
}