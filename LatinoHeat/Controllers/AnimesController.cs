using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LatinoHeat.Data;
using LatinoHeat.Models;
using Microsoft.AspNetCore.Hosting;
using System.Security.Claims;

namespace LatinoHeat.Controllers
{
    public class AnimesController : Controller
    {
        private readonly AnimeDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AnimesController(AnimeDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Animes.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var anime = await _context.Animes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (anime == null)
            {
                return NotFound();
            }

            return View(anime);
        }

        public IActionResult Create()
        {
            ViewBag.Genre = new List<string> { "Shonen", "Isekai", "Mecha", "Comedy", "Horror", "Fantasy", "Romance", "Shojo", "Adventure", "Kodomo" };
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Cover, CoverPath,Title,Description,Genre, ReleaseDate,EpisodeCount,CreatedBy,Rating")] Anime anime)
        {
            if (ModelState.IsValid)
            {
                if (anime.Cover != null)
                {
                    if (anime.Cover.ContentType != "image/jpeg" && anime.Cover.ContentType != "image/png")
                    {
                        ModelState.AddModelError("Cover", "PNG and JPEGS ALLOWED ONLY");
                    }
                    string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }

                    string fileName = Path.GetFileName(anime.Cover.FileName);
                    string fileSavePath = Path.Combine(uploadFolder, fileName);

                    using (FileStream stream = new FileStream(fileSavePath, FileMode.Create))
                    {
                        await anime.Cover.CopyToAsync(stream);
                    }
                    ViewBag.ImagePathAnime = "/uploads/" + fileName;
                    anime.CoverPath = ViewBag.ImagePathAnime;
                }

                _context.Add(anime);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = anime.Id });
            }

            ViewBag.Genre = new List<string> { "Shonen", "Isekai", "Mecha", "Comedy", "Horror", "Fantasy", "Romance", "Shojo", "Adventure", "Kodomo" };
            return View(anime);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var anime = await _context.Animes.FindAsync(id);
            if (anime == null)
            {
                return NotFound();
            }
            return View(anime);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CoverPath,Title,Description,Category,EpisodeCount,CreatedBy,Rating")] Anime anime)
        {
            if (id != anime.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(anime);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnimeExists(anime.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(anime);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var anime = await _context.Animes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (anime == null)
            {
                return NotFound();
            }

            return View(anime);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var anime = await _context.Animes.FindAsync(id);
            if (anime != null)
            {
                _context.Animes.Remove(anime);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnimeExists(int id)
        {
            return _context.Animes.Any(e => e.Id == id);
        }
    }
}
