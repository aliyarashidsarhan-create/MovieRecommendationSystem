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
        private double CalculateSimilarity(int userId1, int userId2, List<Rating> ratings)
        {
            var ratings1 = ratings.Where(r => r.UserId == userId1).ToList();
            var ratings2 = ratings.Where(r => r.UserId == userId2).ToList();

            var commonMovies = ratings1
                .Select(r => r.MovieId)
                .Intersect(ratings2.Select(r => r.MovieId))
                .ToList();

            if (commonMovies.Count == 0)
            {
                return 0;
            }

            double similarity = 0;

            foreach (var movieId in commonMovies)
            {
                int score1 = ratings1.First(r => r.MovieId == movieId).Score;
                int score2 = ratings2.First(r => r.MovieId == movieId).Score;

                // Smaller difference means higher similarity.
                similarity += 5 - Math.Abs(score1 - score2);
            }

            return similarity / commonMovies.Count;
        }
    }
}