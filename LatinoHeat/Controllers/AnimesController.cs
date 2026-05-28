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
using LatinoHeat.Interface;

namespace LatinoHeat.Controllers
{
    public class AnimesController : Controller
    {
        //private readonly AnimeDbContext _context;
        private readonly IDataAccessLayer _dAL;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AnimesController(IDataAccessLayer dAL, IWebHostEnvironment webHostEnvironment)
        {
            _dAL = dAL;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View(_dAL.GetAnime());
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var anime = _dAL.GetAnime(id.Value);
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

                _dAL.CreateAnime(anime);
                return RedirectToAction("Details", new { id = anime.Id });
            }

            ViewBag.Genre = new List<string> { "Shonen", "Isekai", "Mecha", "Comedy", "Horror", "Fantasy", "Romance", "Shojo", "Adventure", "Kodomo" };
            return View(anime);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var anime = _dAL.GetAnime(id.Value);
            if (anime == null)
            {
                return NotFound();
            }
            ViewBag.Genre = new List<string> { "Shonen", "Isekai", "Mecha", "Comedy", "Horror", "Fantasy", "Romance", "Shojo", "Adventure", "Kodomo" };
            return View(anime);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,CoverPath,Title,Description,Genre,EpisodeCount,CreatedBy,Rating")] Anime anime)
        {
            if (id != anime.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dAL.UpdateAnime(anime);
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
            ViewBag.Genre = new List<string> { "Shonen", "Isekai", "Mecha", "Comedy", "Horror", "Fantasy", "Romance", "Shojo", "Adventure", "Kodomo" };
            return View(anime);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var anime = _dAL.GetAnime(id.Value);
            if (anime == null)
            {
                return NotFound();
            }

            return View(anime);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var anime = _dAL.GetAnime(id);
            if (anime != null)
            {
                _dAL.DeleteAnime(anime);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool AnimeExists(int id)
        {
            return _dAL.AnimeExists(id);
        }
    }
}
