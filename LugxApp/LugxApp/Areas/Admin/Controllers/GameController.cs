using LugxApp.Contexts;
using LugxApp.Models;
using LugxApp.ViewModels.GameVMs;
using LugxApp.ViewModels.GenreVMs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LugxApp.Areas.Admin.Controllers;

public class GameController(LugxDbContext _context, IWebHostEnvironment _env) : AdminBaseController
{
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

    public async Task<IActionResult> Create()
    {
        await FillGenresToViewBagAsync();
        return View();
    }

    [HttpPost, AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Create(GameCreateVM model)
    {
        if (!ModelState.IsValid)
        {
            await FillGenresToViewBagAsync();
            return View(model);
        }

        Genre? genre = await _context.Genres.FindAsync(model.GenreId);
        if (genre == null)
        {
            ModelState.AddModelError("GenreId", "Invalid genre id!");
            await FillGenresToViewBagAsync();
            return View(model);
        }

        string? imagePath = null;
        if (model.Image != null)
        {
            if (!model.Image.ContentType.StartsWith("image"))
            {
                ModelState.AddModelError("Image", "Only image files are accepted!");
                await FillGenresToViewBagAsync();
                return View(model);
            }

            if (model.Image.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError("Image", "File size cannot exceed 2 MBs!");
                await FillGenresToViewBagAsync();
                return View(model);
            }

            imagePath = await CopyToNewImagePathAsync(model.Image);
        }

        var entity = new Game()
        {
            Name = model.Name,
            Price = model.Price,
            Discount = model.Discount,
            ImagePath = imagePath,
            GenreId = model.GenreId,
        };

        await _context.Games.AddAsync(entity);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Update(int id)
    {
        Game? entity = await _context.Games.FindAsync(id);
        if (entity == null)
            return NotFound();

        var model = new GameUpdateVM()
        {
            Id = entity.Id,
            Name = entity.Name,
            ImagePath = entity.ImagePath,
            Price = entity.Price,
            Discount = entity.Discount,
            GenreId = entity.GenreId,
        };

        await FillGenresToViewBagAsync();

        return View(model);
    }

    [HttpPost, AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Update(int id, GameUpdateVM model)
    {
        Game? entity = await _context.Games.FindAsync(id);
        if (entity == null)
            return NotFound();

        if (!ModelState.IsValid)
        {
            await FillGenresToViewBagAsync();
            return View(model);
        }

        Genre? genre = await _context.Genres.FindAsync(model.GenreId);
        if (genre == null)
        {
            ModelState.AddModelError("GenreId", "Invalid genre id!");
            await FillGenresToViewBagAsync();
            return View(model);
        }

        string? imagePath = entity.ImagePath;

        if (model.Image != null)
        {
            if (!model.Image.ContentType.StartsWith("image"))
            {
                ModelState.AddModelError("Image", "Only image files are accepted!");
                await FillGenresToViewBagAsync();
                return View(model);
            }

            if (model.Image.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError("Image", "File size cannot exceed 2 MBs!");
                await FillGenresToViewBagAsync();
                return View(model);
            }

            if (entity.ImagePath != null)
            {
                string fullPath = _env.WebRootPath + entity.ImagePath;
                await CopyToExistingImagePathAsync(model.Image, fullPath);
            }
            else
                imagePath = await CopyToNewImagePathAsync(model.Image);
        }

        entity.Name = model.Name;
        entity.ImagePath = imagePath;
        entity.Price = model.Price;
        entity.Discount = model.Discount;
        entity.GenreId = model.GenreId;

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        Game? entity = await _context.Games.FindAsync(id);
        if (entity == null)
            return NotFound();

        if (entity.ImagePath != null)
            System.IO.File.Delete(_env.WebRootPath + entity.ImagePath);

        _context.Games.Remove(entity);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }


    #region Utility Methods
    public async Task FillGenresToViewBagAsync()
    {
        ViewBag.Genres = await _context.Genres.Select(g => new GenreGetVM()
        {
            Id = g.Id,
            Name = g.Name,
        }).ToListAsync();
    }

    public async Task<string> CopyToNewImagePathAsync(IFormFile image)
    {
        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
        string fullPath = Path.Combine(_env.WebRootPath, "uploads", fileName);

        using var fs = new FileStream(fullPath, FileMode.Create);
        await image.CopyToAsync(fs);

        return "/uploads/" + fileName;
    }

    public async Task CopyToExistingImagePathAsync(IFormFile image, string imagePath)
    {
        string fullPath = _env.WebRootPath + imagePath;
        using var fs = new FileStream(fullPath, FileMode.Create);
        await image.CopyToAsync(fs);
    }
    #endregion
}
