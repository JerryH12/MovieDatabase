using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Movies.Data;
using Movies.Models;
using Movies.Repository;

namespace Movies.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MoviesContext _context;
       // private readonly IMovieRepository _repository;

        public MoviesController(MoviesContext context)
        {
            //_repository = new MovieRepository(context);
            _context = context;
        }

        // GET: Movies
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            //return View(await _repository.GetAll().ToListAsync());
            return View(await _context.Movies.ToListAsync());
        }

        [Authorize(Roles = "Admin")]
        private void PopulateActorData(int? movieId)
        {
            /* var ActorData = from actor in _context.Actors
                             join movie in _context.Movies
                             on actor.Id equals movie.Id
                             where movie.Id == movieId
                             select actor;*/

            var ActorData = _context.Actors
                .Include(m => m.Movies)
                .ThenInclude(m => m.Actors);
               
                            
            var viewModel = new List<ActorDetailsViewModel>();

            foreach (var item in ActorData)
            {
                viewModel.Add(new ActorDetailsViewModel
                {
                    ActorId = item.Id,
                    FirstName = item.FirstName,
                    LastName = item.LastName
                   
                });

                
            }
            ViewBag.ActorPopulate = viewModel;
        }

        // GET: Movies/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

           // PopulateActorData(id);

            var movie = await _context.Movies
                .Include(m => m.Actors)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        private byte?[] ConvertToNullableBytes(byte[] nonNullableBytes)
        {     
            byte?[] nullableBytes = new byte?[nonNullableBytes.Length];

            for (int i = 0; i < nonNullableBytes.Length; i++)
            {
                nullableBytes[i] = nonNullableBytes[i];
            }
            return nullableBytes;
        }

        // GET: Movies/Create

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
           
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(IFormFile ImageFile, [Bind("Title,Exclude=ImageFile,Director,Description,ReleaseDate")] Movie movie)
        {
            if (ModelState.IsValid)
            {     
                using (var memoryStream = new MemoryStream())
                {
                    await ImageFile.CopyToAsync(memoryStream);

                    if (memoryStream.Length < 2097152)
                    {
                        movie.ImageFile = memoryStream.ToArray();
                        //movie.ImageFile = ConvertToNullableBytes(memoryStream.ToArray());


                        
                    }
                }
               
                    _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // Only for testing. Remove later.
                var errors = ModelState.Select(x => x.Value.Errors)
                               .Where(y => y.Count > 0)
                               .ToList();
            }
            return View(movie);
        }

        // GET: Movies/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, IFormFile file, [Bind("Id,Title,Director,Description,ReleaseDate")] Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            // TODO: Edit without uploading any file.
            if (ModelState.IsValid)
            {
                try
                {
                    if (file != null)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await file.CopyToAsync(memoryStream);

                            if (memoryStream.Length < 2097152)
                            {
                                movie.ImageFile = memoryStream.ToArray();
                               // movie.ImageFile = ConvertToNullableBytes(memoryStream.ToArray());
                            }
                        }
                    }

                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
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
            return View(movie);
        }

        // GET: Movies/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /*
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string uploads = Path.Combine(Environment.CurrentDirectory, "wwwroot/images");
                    if (file.Length > 0)
                    {
                        string filePath = Path.Combine(uploads, file.FileName);
                        using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                    }
                }
            }
            return View("Create");
        }*/

      
        public async Task<ActionResult> MovieInfo(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
               .Include(m => m.Actors)
               .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }
            return View("MovieInfo", movie);
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}
