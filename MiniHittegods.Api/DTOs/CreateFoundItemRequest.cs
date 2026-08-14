using System.ComponentModel.DataAnnotations;

namespace MiniHittegods.Api.DTOs;

public class CreateFoundItemRequest
{
    [Required]
    [MaxLength(80)]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string Category { get; set; } = string.Empty;

    [Required]
    public string FoundLocation { get; set; } = string.Empty;
}