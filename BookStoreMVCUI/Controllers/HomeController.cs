using BookStoreMVCUI.Models;
using BookStoreMVCUI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BookStoreMVCUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeRepository homeRepository;

        public HomeController(ILogger<HomeController> logger, IHomeRepository homeRepository)
        {
            _logger = logger;
            this.homeRepository = homeRepository;
        }

        public async  Task<IActionResult> Index(string sTerm="", int genreId=0)
        {
            BookDisplayModel bookDisplayModel = new BookDisplayModel();
            bookDisplayModel.Books = await homeRepository.GetBooks(sTerm, genreId);
            bookDisplayModel.Genres = await homeRepository.GetGenres();
            bookDisplayModel.GenreId = genreId;
            bookDisplayModel.STerm = sTerm;
            return View(bookDisplayModel);
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
}
