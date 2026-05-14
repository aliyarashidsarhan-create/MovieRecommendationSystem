using MovieRecommendationSystem.Models;
using MovieRecommendationSystem.Utilities;
using System.Threading;

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
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n======================== MOVIE GALLERY ========================\n");
            Console.ResetColor();

            int columns = 3;
            int cardWidth = 34;

            for (int i = 0; i < _movies.Count; i += columns)
            {
                var rowMovies = _movies.Skip(i).Take(columns).ToList();

                var cards = rowMovies
                .Select(movie => BuildMovieCard(movie, cardWidth))
                .ToList();

                for (int line = 0; line < cards[0].Count; line++)
                {
                    if (line == 0 || line == 2 || line == 5)
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    else if (line == 1)
                        Console.ForegroundColor = ConsoleColor.White;
                    else if (line == 4)
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    else
                        Console.ForegroundColor = ConsoleColor.Gray;

                    foreach (var card in cards)
                    {
                        Console.Write(card[line] + "  ");
                    }

                    Console.WriteLine();
                    Console.ResetColor();
                }

                Console.WriteLine();
            }
        }

        private List<string> BuildMovieCard(Movie movie, int width)
        {
            string border = "+" + new string('-', width - 2) + "+";

            int starsCount = (int)Math.Round(movie.Rating / 2);
            starsCount = Math.Clamp(starsCount, 0, 5);

            string stars = new string('*', starsCount) + new string('-', 5 - starsCount);

            return new List<string>
{
border,
FormatCardLine($"[{movie.Id}] {movie.Title}", width),
border,
FormatCardLine($"Genre: {movie.Genre}", width),
FormatCardLine($"Rating: {stars} ({movie.Rating:0.0})", width),
border
};
        }

        private string FormatCardLine(string text, int width)
        {
            int contentWidth = width - 4;

            if (text.Length > contentWidth)
            {
                text = text.Substring(0, contentWidth - 3) + "...";
            }

            return "| " + text.PadRight(contentWidth) + " |";
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