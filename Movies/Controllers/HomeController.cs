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

        public async Task<IActionResult> Index()
        {
            int numberOfObjectsPerPage = 8;
            int pageNumber = 0;

            int total = (from m in _context.Movies select m).Count();
            ViewBag.Total = total;

            var queryPageResult = _context.Movies
            .Skip(numberOfObjectsPerPage * pageNumber)
            .Take(numberOfObjectsPerPage);

          
            return View(await queryPageResult.ToListAsync());


            //return View(await _context.Movies.ToListAsync());
        }

        public async Task<IActionResult> AdvancedSearchPage()
        {
           

            return View("AdvancedSearch", await _context.Movies.ToListAsync());
        }

        [HttpPost]
        public IActionResult AdvancedSearch(string title, string genre, string director)
        {
            if(director != null)
            {
                //var model = _context.Movies.Where(m => m.Director.ToLower().Contains(director.ToLower()));

                var model = from x in _context.Movies
                            where x.Director.ToLower().Contains(director.ToLower())
                            select x;
                return View(model);
            }
            return View();
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

        public async Task<ActionResult> GetMoviesPerPage(int page = 0)
        {
            if (page < 0)
            {
                ViewBag.Page = 0;
                return View();
            }

            int numberOfObjectsPerPage = 8;
            int pageNumber = page;
            int total = (from m in _context.Movies select m).Count();
            ViewBag.Total = total;
            ViewBag.PageNumber = pageNumber;

            var queryPageResult = _context.Movies
            .Skip(numberOfObjectsPerPage * pageNumber)
            .Take(numberOfObjectsPerPage);

            ViewBag.Page = page;

            return View("Index", await queryPageResult.ToListAsync());
        }

        public int GetTotalNumberOfMovies()
        {
            return _context.Movies.Count();
        }
    }
}
