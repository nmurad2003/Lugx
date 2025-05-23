using System.ComponentModel.DataAnnotations;

namespace LugxApp.Models;

public class Genre : BaseEntity
{
    [MaxLength(64)]
    public string Name { get; set; }
    IEnumerable<Genre> Genres { get; set; }
}
