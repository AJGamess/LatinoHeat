using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using LatinoHeat.Models;
using LatinoHeat.Interface;

namespace LatinoHeat.Controllers
{
    public class AnimesController : Controller
    {
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

        public IActionResult Browse(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;
            return View(_dAL.GetAnimeWithReactions(User.Identity.Name, searchString));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Like(int id)
        {
            _dAL.ToggleReaction(id, User.Identity.Name, true);
            return RedirectToAction(nameof(Browse));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Dislike(int id)
        {
            _dAL.ToggleReaction(id, User.Identity.Name, false);
            return RedirectToAction(nameof(Browse));
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
        public async Task<IActionResult> Create([Bind("Id,Cover, CoverPath,Title,Description,Genre,ReleaseDate,EpisodeCount,CreatedBy,Rating")] Anime anime)
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
                anime.CreatedBy = User.Identity.Name;
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Cover, CoverPath,Title,Description,Genre, ReleaseDate,EpisodeCount,CreatedBy,Rating")] Anime anime)
        {
            if (id != anime.Id)
            {
                return NotFound();
            }

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

                try
                {
                    _dAL.UpdateAnime(anime);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_dAL.AnimeExists(anime.Id))
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
    }
}
