using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Movies.Data;
using Movies.Models;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Movies.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly MoviesContext _context;

        public HomeController(ILogger<HomeController> logger, MoviesContext context)
        {
            _context = context;
            _logger = logger;
        }

        /*
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }*/


        public async Task<IActionResult> Index()
        {
            return View(await _context.Movies.ToListAsync());
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

        [HttpPost]
        public IActionResult SearchByTitle(string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                return NotFound();
            }

            var model = from x in _context.Movies
                         where x.Title.ToLower().Contains(word.ToLower())
                         select x;
            return View("index", model);
        }
    }
}
