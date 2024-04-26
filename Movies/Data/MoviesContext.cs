using Microsoft.EntityFrameworkCore;
using Movies.Models;

namespace Movies.Data
{
    public class MoviesContext : DbContext
    {
        public MoviesContext(DbContextOptions<MoviesContext> options) : base(options)  
        { }

        public DbSet<Actor> Actors { get; set; }
        public DbSet<FilmCrew> FilmCrew {  get; set; }
        public DbSet<Genre> Genre { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Price> Price { get; set; }

        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // We tell the app to use the connectionstring.
            //optionsBuilder.UseSqlServer(connectionString);

            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot config = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();
                string connString = config.GetConnectionString("MoviesConnectionString");
                optionsBuilder.UseSqlServer(connString); //Or whatever DB you are using
            }
        }
    }
}
