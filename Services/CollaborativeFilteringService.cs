using MovieRecommendationSystem.Interfaces;
using MovieRecommendationSystem.Models;

namespace MovieRecommendationSystem.Services
{
    public class CollaborativeFilteringService : IRecommendation
    {
        // Recommend movies based on similar users.
        public List<Movie> Recommend(User user, List<Movie> movies, List<Rating> ratings, List<User> users)
        {
            var userRatings = ratings.Where(r => r.UserId == user.Id).ToList();

            var similarUsers = users
                .Where(u => u.Id != user.Id)
                .Select(otherUser => new
                {
                    User = otherUser,
                    Similarity = CalculateSimilarity(user.Id, otherUser.Id, ratings)
                })
                .OrderByDescending(x => x.Similarity)
                .Take(3)
                .ToList();

            var watchedMovieIds = user.WatchHistory;

            var recommendedMovieIds = similarUsers
                .SelectMany(su => ratings.Where(r => r.UserId == su.User.Id && r.Score >= 4))
                .Where(r => !watchedMovieIds.Contains(r.MovieId))
                .GroupBy(r => r.MovieId)
                .OrderByDescending(g => g.Average(r => r.Score))
                .Select(g => g.Key)
                .Take(5)
                .ToList();

            return movies.Where(m => recommendedMovieIds.Contains(m.Id)).ToList();
        }

        // Simple similarity calculation between two users.
        // Calculate similarity between two users using Cosine Similarity.
        private double CalculateSimilarity(int userId1, int userId2, List<Rating> ratings)
        {
            var ratings1 = ratings.Where(r => r.UserId == userId1).ToList();
            var ratings2 = ratings.Where(r => r.UserId == userId2).ToList();

            // Get movies rated by both users.
            var commonMovies = ratings1
        .Select(r => r.MovieId)
        .Intersect(ratings2.Select(r => r.MovieId))
        .ToList();

            // If there are no common movies, similarity = 0.
            if (commonMovies.Count == 0)
            {
                return 0;
            }

            double dotProduct = 0;
            double magnitude1 = 0;
            double magnitude2 = 0;

            foreach (var movieId in commonMovies)
            {
                int score1 = ratings1.First(r => r.MovieId == movieId).Score;
                int score2 = ratings2.First(r => r.MovieId == movieId).Score;

                // Multiply ratings together.
                dotProduct += score1 * score2;

                // Square ratings.
                magnitude1 += Math.Pow(score1, 2);
                magnitude2 += Math.Pow(score2, 2);
            }

            // Avoid division by zero.
            if (magnitude1 == 0 || magnitude2 == 0)
            {
                return 0;
            }

            // Cosine Similarity formula.
            return dotProduct / (Math.Sqrt(magnitude1) * Math.Sqrt(magnitude2));
        }
    }
}