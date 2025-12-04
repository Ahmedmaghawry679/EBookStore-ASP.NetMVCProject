using BookStoreMVCUI.Models;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreMVCUI.Controllers
{
    [Authorize(Roles =nameof(Roles.Admin))]
    public class GenreController : Controller
    {
        private readonly IGenreRepository _genreRepository;

        public GenreController(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public async Task<IActionResult> Index()
        {
            var genres = await _genreRepository.GetGenres();
            return View(genres);
        }

        public IActionResult AddGenre()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddGenre(GenreDTO genreDTO)
        {
            if(!ModelState.IsValid)
            {
                return View(genreDTO);
            }
            try
            {
                var genre = new Genre { GenreName = genreDTO.GenreName, Id = genreDTO.Id };
                await _genreRepository.AddGenre(genre);
                TempData["successMessage"] = "Genre added successfully";
                return RedirectToAction(nameof(AddGenre));
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Genre could not added!";
                return View(genreDTO);
            }
        }

        public async Task<IActionResult> UpdateGenre(int id)
        {
            var genre = await _genreRepository.GetGenreById(id);
            if (genre is null)
                throw new InvalidOperationException($"Genre with id: {id} does not found");
            var genreDTO = new GenreDTO
            {
                Id = genre.Id,
                GenreName = genre.GenreName
            };
            return View(genreDTO);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateGenre(GenreDTO genreDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(genreDTO);
            }
            try
            {
                var genre = new Genre { GenreName = genreDTO.GenreName, Id = genreDTO.Id };
                await _genreRepository.UpdateGenre(genre);
                TempData["successMessage"] = "Genre is updated successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Genre could not updated!";
                return View(genreDTO);
            }

        }

        public async Task<IActionResult> DeleteGenre(int id)
        {
            var genre = await _genreRepository.GetGenreById(id);
            if (genre is null)
                throw new InvalidOperationException($"Genre with id: {id} does not found");
            await _genreRepository.DeleteGenre(genre);
            return RedirectToAction(nameof(Index));

        }
    }
}
