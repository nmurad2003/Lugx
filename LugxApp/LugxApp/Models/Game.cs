using System.ComponentModel.DataAnnotations;

namespace LugxApp.Models;

public class Game : BaseEntity
{
    [MaxLength(64)]
    public string Name { get; set; }
    public float Price { get; set; }
    [Range(0, 100)]
    public float? Discount { get; set; } = 0;
    public string? ImagePath { get; set; }

    public int GenreId { get; set; }
    public Genre Genre { get; set; }
}
