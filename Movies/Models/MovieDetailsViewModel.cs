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

        public DateTime ReleaseDate { get; set; }

        public string GenreName { get; set; }
       
        public double PriceAmount { get; set; }
        
        
    }
}
