namespace Movies.Models
{
    public class GenreDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Movie> Movies { get; set; }
    }
}
