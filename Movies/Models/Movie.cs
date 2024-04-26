﻿namespace Movies.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; } 
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set;}
        public List<Actor> Actors { get; set; }
        public List<Genre> Genres { get; set; }   
        public List<Price> Prices { get; set; }
        public List<FilmCrew> FilmCrewList { get; set; }
    }
}
