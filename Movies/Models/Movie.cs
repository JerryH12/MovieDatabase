﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Movies.Models
{
    public class Movie
    {
        public int Id { get; set; }

        public string Title { get; set; }

        [ValidateNever]
        [AllowNull]
        public byte[]? ImageFile { get; set; }

        public string Description { get; set; }

        [Display(Name = "Release Date")]
        public DateTime ReleaseDate { get; set; }

        public string Director { get; set; }

        public List<Actor> Actors { get; set; } = [];
        public List<Genre> Genres { get; set; } = [];
        public List<Price> Prices { get; set; } = [];
    }
}
