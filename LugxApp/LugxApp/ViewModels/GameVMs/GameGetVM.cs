using LugxApp.ViewModels.GenreVMs;
using System.ComponentModel.DataAnnotations;

namespace LugxApp.ViewModels.GameVMs;

public class GameGetVM
{
    public int Id { get; set; }
    public string Name { get; set; }
    public float Price { get; set; }
    public float? Discount { get; set; } = 0;
    public string? ImagePath { get; set; }
    public GenreGetVM Genre { get; set; }
}
