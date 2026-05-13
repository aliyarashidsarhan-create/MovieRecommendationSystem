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
            Console.ForegroundColor = ConsoleColor.Cyan;

            Console.WriteLine("\n==================================================================================================");
            Console.WriteLine($"{"ID",-5} {"TITLE",-35} {"GENRE",-15} {"YEAR",-10} {"RATING",-10}");
            Console.WriteLine("==================================================================================================");

            Console.ResetColor();

            foreach (var movie in _movies)
            {
                Console.WriteLine($"{movie.Id,-5} {movie.Title,-35} {movie.Genre,-15} {movie.ReleaseYear,-10} {movie.Rating,-10}");
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("==================================================================================================");
            Console.ResetColor();
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
                Console.WriteLine($"Movie ID: {movieId}");
                Console.WriteLine($"Score: {score}");
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
                Console.WriteLine("Rating saved!");
                Console.WriteLine($"Movie ID: {movieId}");
                Console.WriteLine($"Score: {score}");
            }

            if (!user.WatchHistory.Contains(movieId))
            {
                user.WatchHistory.Add(movieId);
            }
        }

        // Remove movie rating.
        public void RemoveRating(User user, int movieId)
        {
            var rating = _ratings.FirstOrDefault(r =>
            r.UserId == user.Id && r.MovieId == movieId);

            if (rating == null)
            {
                Console.WriteLine("Rating not found.");
                return;
            }

            _ratings.Remove(rating);

            Console.WriteLine("Rating removed successfully.");
        }
    }
}