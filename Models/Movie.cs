using System;
using System.Collections.Generic;
using System.Text;

namespace MovieRecommendationSystem.Models
{
    public class Movie
    {
        // Movie information required by the project.
        public int Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public int ReleaseYear { get; set; }
        public double Rating { get; set; }
        public string Director { get; set; }
        public List<string> Cast { get; set; }
        public List<string> Tags { get; set; }

        public Movie()
        {
            Cast = new List<string>();
            Tags = new List<string>();
        }
    }
}