using System.ComponentModel.DataAnnotations;

namespace LugxApp.ViewModels.GenreVMs;

public class GenreUpdateVM
{
    public int Id { get; set; }
    [MaxLength(64)]
    public string Name { get; set; }
}
