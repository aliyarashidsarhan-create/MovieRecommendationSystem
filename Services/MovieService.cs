using MovieRecommendationSystem.Models;
using MovieRecommendationSystem.Utilities;

namespace MovieRecommendationSystem.Services
{
    public class MovieService
    {
        private List<Movie> _movies;
        private List<Rating> _ratings;

        public MovieService(List<Movie> movies, List<Rating> ratings)
        {
            _movies = movies;
            _ratings = ratings;
        }

        // Display all movies.
        public void DisplayMovies()
        {
            Console.WriteLine("\nMovies List:");
            Console.WriteLine("--------------------------------------");

            foreach (var movie in _movies)
            {
                Console.WriteLine($"{movie.Id}. {movie.Title} | {movie.Genre} | {movie.ReleaseYear} | Rating: {movie.Rating}");
            }
        }

        // Add or update rating.
        public void RateMovie(User user, int movieId, int score)
        {
            if (!ValidationHelper.IsValidRating(score))
            {
                Console.WriteLine("Rating must be from 1 to 5.");
                return;
            }

            var movie = _movies.FirstOrDefault(m => m.Id == movieId);

            if (movie == null)
            {
                Console.WriteLine("Movie not found.");
                return;
            }

            var existingRating = _ratings.FirstOrDefault(r =>
                r.UserId == user.Id && r.MovieId == movieId);

            if (existingRating != null)
            {
                existingRating.Score = score;
                Console.WriteLine("Rating updated successfully.");
            }
            else
            {
                _ratings.Add(new Rating
                {
                    UserId = user.Id,
                    MovieId = movieId,
                    Score = score
                });

                Console.WriteLine("Rating added successfully.");
            }

            if (!user.WatchHistory.Contains(movieId))
            {
                user.WatchHistory.Add(movieId);
            }
        }
    }
}