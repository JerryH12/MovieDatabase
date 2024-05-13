using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Movies.Data;
using Movies.Models;

namespace Movies.Repository
{
    public class MovieRepository : IMovieRepository
    {
        private readonly MoviesContext _context;

     

        public MovieRepository(MoviesContext context)
        {
            _context = context;
        }

        public DbSet<Movie> GetAll()
        {
            return _context.Movies;
        }

        public Movie GetById(int id)
        {
            return _context.Movies.Find(id);
        }

        public void Insert(Movie movie)
        {

        }

        public void Update(Movie movie)
        {
            _context.Entry(movie).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Movie movie = _context.Movies.Find(id);

            if (movie != null)
            {
                _context.Movies.Remove(movie);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}



