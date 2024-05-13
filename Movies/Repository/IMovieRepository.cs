using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Movies.Models;

namespace Movies.Repository
{
    public interface IMovieRepository
    {
        DbSet<Movie> GetAll();
        Movie GetById(int id);
        void Insert(Movie movie);
        void Update(Movie movie);
        void Delete(int id);
        void Save();

    }
}
