using System.ComponentModel.DataAnnotations;

namespace LugxApp.ViewModels.GameVMs;

public class GameUpdateVM
{
    public int Id { get; set; }
    [MaxLength(64)]
    public string Name { get; set; }
    public float Price { get; set; }
    [Range(0, 100)]
    public float? Discount { get; set; } = 0;
    public IFormFile? Image { get; set; }
    public string? ImagePath { get; set; }
    public int GenreId { get; set; }
}
