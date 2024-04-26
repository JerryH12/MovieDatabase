namespace Movies.Models
{
    public class FilmCrew
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set;}
        public string Role { get; set; }
    }
}
