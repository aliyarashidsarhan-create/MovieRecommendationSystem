using System;
using System.Collections.Generic;
using System.Text;

namespace MovieRecommendationSystem.Models
{
    public class Rating
    {
        // ID of the user who rated the movie.
        public int UserId { get; set; }

        // ID of the movie being rated.
        public int MovieId { get; set; }

        // Rating value from 1 to 5.
        public int Score { get; set; }
    }
}