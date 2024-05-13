using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Movies.Data;
using Movies.Models;

namespace Movies.Controllers
{
    public class ActorsController : Controller
    {
        private readonly MoviesContext _context;

        public ActorsController(MoviesContext context)
        {
            _context = context;
        }

        // GET: Actors
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Actors.OrderBy(a => a.LastName).ToListAsync());
        }

        // GET: Actors/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actor = await _context.Actors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actor == null)
            {
                return NotFound();
            }

            return View(actor);
        }

        // GET: Actors/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            //ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title");
            PopulateMovieData();
            return View();
        }

        [Authorize(Roles = "Admin")]
        private void PopulateMovieData()
        {
            var MovieData = _context.Movies
                             .Include(m => m.Actors);
            var viewModel = new List<MovieDetailsViewModel>();
            foreach(var item in MovieData)
            {
                viewModel.Add(new MovieDetailsViewModel
                {
                    MovieId = item.Id,
                    Title = item.Title,
                    Actors = item.Actors
                });
            }
            ViewBag.MoviePopulate = viewModel;
        }


        // POST: Actors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        // public async Task<IActionResult> Create([Bind("FirstName,LastName")] Actor actor)

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Actor actor, string[] selectedMovie)
        {
            if (ModelState.IsValid)
            {

                _context.Attach(actor);
                _context.Add(actor);
              

                foreach (var movieId in selectedMovie)
                {
                    var movie = _context.Movies.Find(int.Parse(movieId));
                    actor.Movies.Add(movie);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(actor);
        }

        // GET: Actors/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var actor = await _context.Actors.FindAsync(id);
            var actor = _context.Actors.Include(a=>a.Movies)
                                       .FirstOrDefault(a => a.Id == id);
            if (actor == null)
            {
                return NotFound();
            }

            PopulateMovieData();
         
            return View(actor);
        }

        // POST: Actors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, string[] selectedMovie, [Bind("Id,FirstName,LastName")] Actor actor)
        {
            if (id != actor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                   // _context.Attach(actor);
                    _context.Update(actor);


                    /*
                     * // Retrieve the entities
                        var entity1 = context.Entity1s.Include(e => e.Entity2s).FirstOrDefault(e => e.Id == entity1Id);
                        var entity2 = context.Entity2s.Include(e => e.Entity1s).FirstOrDefault(e => e.Id == entity2Id);

                        if (entity1 != null && entity2 != null)
                        {
                            // Remove the relationship
                            entity1.Entity2s.Remove(entity2);
                            entity2.Entity1s.Remove(entity1);

                            // Save changes
                            context.SaveChanges();
                        }
                     */

                    var entities = _context.Actors.Include(a => a.Movies).FirstOrDefault(a => a.Id == id);
                   // var entities2 = _context.Movies.Include(m => m.Actors).FirstOrDefault(m => m.Id == movie.Id);

                    foreach (var movie in entities.Movies)
                    {
                       movie.Actors.Remove(actor);
                    }

                    entities.Movies.Clear();
                    _context.SaveChanges();

                    foreach (var movieId in selectedMovie)
                    {
                        var movie = _context.Movies.Find(int.Parse(movieId));  
                        entities.Movies.Add(movie);
                    }
     
                  _context.SaveChanges();
                  
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActorExists(actor.Id))
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
            return View(actor);
        }

        // GET: Actors/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actor = await _context.Actors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actor == null)
            {
                return NotFound();
            }

            return View(actor);
        }

        // POST: Actors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var actor = await _context.Actors.FindAsync(id);
            if (actor != null)
            {
                _context.Actors.Remove(actor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        private bool ActorExists(int id)
        {
            return _context.Actors.Any(e => e.Id == id);
        }
    }
}
