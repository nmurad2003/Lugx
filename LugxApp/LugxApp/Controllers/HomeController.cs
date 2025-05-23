using LugxApp.Contexts;
using LugxApp.Models;
using LugxApp.ViewModels.GameVMs;
using LugxApp.ViewModels.GenreVMs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace LugxApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    public readonly LugxDbContext _context;

    public HomeController(ILogger<HomeController> logger, LugxDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        List<GameGetVM> vms = await _context.Games.Select(g => new GameGetVM()
        {
            Id = g.Id,
            ImagePath = g.ImagePath,
            Name = g.Name,
            Genre = new GenreGetVM() { Name = g.Genre.Name },
            Price = g.Price,
            Discount = g.Discount,
        }).ToListAsync();

        return View(vms);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
