using MovieRecommendationSystem.Interfaces;
using MovieRecommendationSystem.Models;

namespace MovieRecommendationSystem.Services
{
    public class ContentBasedService : IRecommendation
    {
        // Recommend movies based on user's favorite genres and movie tags.
        public List<Movie> Recommend(User user, List<Movie> movies, List<Rating> ratings, List<User> users)
        {
            var watchedMovieIds = user.WatchHistory;

            var recommendedMovies = movies
                .Where(movie => !watchedMovieIds.Contains(movie.Id))
                .Select(movie => new
                {
                    Movie = movie,

                    // Score increases if genre matches user favorite genres.
                    Score =
                        (user.FavoriteGenres.Contains(movie.Genre) ? 5 : 0) +
                        movie.Tags.Count(tag => user.FavoriteGenres.Contains(tag)) +
                        movie.Rating
                })
                .OrderByDescending(x => x.Score)
                .Take(5)
                .Select(x => x.Movie)
                .ToList();

            return recommendedMovies;
        }
    }
}