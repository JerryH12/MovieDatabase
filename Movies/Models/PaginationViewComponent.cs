using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies.Data;

namespace Movies.Models
{
    public class PaginationViewComponent : ViewComponent
    {
        private readonly MoviesContext _context;

        public PaginationViewComponent(MoviesContext context) 
        {
            _context = context;
        }
        
        [HttpPost]
        public async Task<IViewComponentResult> InvokeAsync(int page = 0)
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

            return View(await queryPageResult.ToListAsync());
        }
    }
}


