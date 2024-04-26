using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Movies.Data;
using Movies.Models;

namespace Movies.Controllers
{
    public class FilmCrewsController : Controller
    {
        private readonly MoviesContext _context;

        public FilmCrewsController(MoviesContext context)
        {
            _context = context;
        }

        // GET: FilmCrews
        public async Task<IActionResult> Index()
        {
            var moviesContext = _context.FilmCrew.Include(f => f.Movie);
            return View(await moviesContext.ToListAsync());
        }

        // GET: FilmCrews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var filmCrew = await _context.FilmCrew
                .Include(f => f.Movie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (filmCrew == null)
            {
                return NotFound();
            }

            return View(filmCrew);
        }

        // GET: FilmCrews/Create
        public IActionResult Create()
        {
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Id");
            return View();
        }

        // POST: FilmCrews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,MovieId,Role")] FilmCrew filmCrew)
        {
            if (ModelState.IsValid)
            {
                _context.Add(filmCrew);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Id", filmCrew.MovieId);
            return View(filmCrew);
        }

        // GET: FilmCrews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var filmCrew = await _context.FilmCrew.FindAsync(id);
            if (filmCrew == null)
            {
                return NotFound();
            }
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Id", filmCrew.MovieId);
            return View(filmCrew);
        }

        // POST: FilmCrews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,MovieId,Role")] FilmCrew filmCrew)
        {
            if (id != filmCrew.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(filmCrew);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilmCrewExists(filmCrew.Id))
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
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Id", filmCrew.MovieId);
            return View(filmCrew);
        }

        // GET: FilmCrews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var filmCrew = await _context.FilmCrew
                .Include(f => f.Movie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (filmCrew == null)
            {
                return NotFound();
            }

            return View(filmCrew);
        }

        // POST: FilmCrews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var filmCrew = await _context.FilmCrew.FindAsync(id);
            if (filmCrew != null)
            {
                _context.FilmCrew.Remove(filmCrew);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FilmCrewExists(int id)
        {
            return _context.FilmCrew.Any(e => e.Id == id);
        }
    }
}
