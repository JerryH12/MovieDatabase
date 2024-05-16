using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Movies.Data;
using Movies.Models;
using System.Diagnostics;
using System.Text;
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
            .Take(numberOfObjectsPerPage)
            .ToList();

            var viewModel = PopulateMovieData(queryPageResult);

            return View(viewModel);


            //return View(await _context.Movies.ToListAsync());
        }

        public async Task<IActionResult> AdvancedSearchPage()
        {
            //var model = _context.Movies.Include(m => m.Genres).Take(10);
            PopulateGenreData();
            return View("AdvancedSearch");
        }

        [HttpPost]
        public IActionResult AdvancedSearch(string title, string genre, string director, string[] selectedGenres)
        {
            List<Movie> model = null;
            
            if (title != null && director != null)
            {
               /* model = (from m in _context.Movies    
                             where m.Director.ToLower().Contains(director.ToLower())
                                && m.Title.ToLower().Contains(title.ToLower())
                                select m);*/

                model = _context.Movies.Include(m=>m.Genres)
                                        .Where(m => m.Director.ToLower().Contains(director.ToLower()))
                                        .Where(m => m.Title.ToLower().Contains(m.Title.ToLower())).ToList();  
                
               
            }
            else if(title != null)
            {
                model = _context.Movies.Include(m => m.Genres)
                                   .Where(m=>m.Title.ToLower().Contains(title.ToLower())).ToList();
            }
            else if(director != null) 
            {
                model = _context.Movies.Include(m => m.Genres)
                                   .Where(m=>m.Director.ToLower().Contains(director.ToLower())).ToList();
            }

            PopulateGenreData();

            if (selectedGenres.Count() > 0 && model != null)
            {
                List<Movie> filteredModel = new List<Movie>();

                foreach (var movie in model)
                {
                    foreach (var gen in movie.Genres)
                    {
                        if (selectedGenres.Any(x => x.Equals(gen.Name)))
                        {
                            filteredModel.Add(movie);
                        }
                    }
                }

                return View(filteredModel);
            }
           
            return View(model);
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

            var model = from m in _context.Movies
                         where m.Title.ToLower().Contains(word.ToLower())
                         select m;

            var viewModel = PopulateMovieData(model.ToList());

            return View("index", viewModel);
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

            var viewModel = PopulateMovieData(queryPageResult.ToList());

            return View("Index", viewModel);
        }

        public int GetTotalNumberOfMovies()
        {
            return _context.Movies.Count();
        }

        private byte[] ConvertToNonNullableBytes(byte?[] nullableBytes)
        {
            byte[] nonNullableBytes = new byte[nullableBytes.Length];

            for (int i = 0; i < nullableBytes.Length; i++)
            {
                nonNullableBytes[i] = (byte)nullableBytes[i];
            }
            return nonNullableBytes;
        }

        private void PopulateGenreData()
        {
            var genreData = _context.Genre
                .Include(m => m.Movies);

            var viewModel = new List<GenreDetailsViewModel>();

            foreach (var item in genreData)
            {
                viewModel.Add(new GenreDetailsViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Movies = item.Movies
                });
            }
            ViewBag.GenrePopulate = viewModel;
        }

        private List<MovieDetailsViewModel> PopulateMovieData(List<Movie> queryResult)
        {
            var viewModel = new List<MovieDetailsViewModel>();

            foreach (var item in queryResult)
            {
                viewModel.Add(new MovieDetailsViewModel
                {
                    MovieId = item.Id,
                    Title = item.Title,
                    Description = item.Description,
                    ReleaseDate = item.ReleaseDate,
                    Director = item.Director,
                    Actors = item.Actors,
                    ImageFile = item.ImageFile
                });
            }
            return viewModel;
        }
    }
}
