using LugxApp.Contexts;
using LugxApp.Models;
using LugxApp.ViewModels.GenreVMs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LugxApp.Areas.Admin.Controllers;

public class GenreController(LugxDbContext _context) : AdminBaseController
{
    public async Task<IActionResult> Index()
    {
        List<GenreGetVM> vms = await _context.Genres.Select(g => new GenreGetVM()
        {
            Id = g.Id,
            Name = g.Name,
        }).ToListAsync();

        return View(vms);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost, AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Create(GenreCreateVM model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var entity = new Genre() { Name = model.Name };

        await _context.Genres.AddAsync(entity);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Update(int id)
    {
        Genre? entity = await _context.Genres.FindAsync(id);
        if (entity == null)
            return NotFound();

        var model = new GenreUpdateVM()
        {
            Id = entity.Id,
            Name = entity.Name,
        };

        return View(model);
    }

    [HttpPost, AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Update(int id, GenreUpdateVM model)
    {
        Genre? entity = await _context.Genres.FindAsync(id);
        if (entity == null)
            return NotFound();

        if (!ModelState.IsValid)
            return View(model);

        entity.Name = model.Name;

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        Genre? entity = await _context.Genres.FindAsync(id);
        if (entity == null)
            return NotFound();

        _context.Genres.Remove(entity);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
