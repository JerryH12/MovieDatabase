using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Movies.Models
{
    public class Price
    {
        public int Id { get; set; } 
        public double Amount { get; set; }
        public int MovieId {  get; set; }

       
        public Movie Movie { get; set; }
    }
}
