using System.ComponentModel.DataAnnotations;

namespace LugxApp.ViewModels.GenreVMs;

public class GenreCreateVM
{
    [MaxLength(64)]
    public string Name { get; set; }
}
