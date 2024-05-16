using System.Reflection;

namespace Movies.Models
{
    public class MovieDetailsViewModel
    {
        // TODO: Combine data from tables for the view.
        /*
        public Movie Movie { get; set; }
        public Genre Genre { get; set; }
        public Actor Actor { get; set; }
        public Price Price { get; set; }*/

        public int MovieId { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }
        public string Director { get; set; } 
        public byte[] ImageFile { get; set; }

        public DateTime ReleaseDate { get; set; }
       
        public double PriceAmount { get; set; }
        
        public List<Actor> Actors { get; set; }

        public List<Genre> Genres { get; set; }
    }
}
