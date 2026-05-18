using MovieRecommendationSystem.Interfaces;
using MovieRecommendationSystem.Models;

namespace MovieRecommendationSystem.Services
{
    public class RecommendationService
    {
        private List<IRecommendation> _strategies;

        public RecommendationService()
        {
            // Polymorphism: same interface, different recommendation strategies.
            _strategies = new List<IRecommendation>
{
new ContentBasedService(),
new CollaborativeFilteringService()
};
        }

        public List<Movie> GetRecommendations(User user, List<Movie> movies, List<Rating> ratings, List<User> users)
        {
            List<Movie> finalRecommendations = new List<Movie>();

            // Get recommendations from each strategy.
            foreach (var strategy in _strategies)
            {
                var result = strategy.Recommend(user, movies, ratings, users);
                finalRecommendations.AddRange(result);
            }

            // Remove duplicate movies, calculate weighted score, and rank movies.
            return finalRecommendations
.GroupBy(m => m.Id)
.Select(g => g.First())
.Select(movie => new
{
    Movie = movie,
    Score = CalculateWeightedScore(user, movie, ratings)
})
.OrderByDescending(x => x.Score)
.Take(5)
.Select(x => x.Movie)
.ToList();
        }

        // Calculate weighted recommendation score for each movie.
        private double CalculateWeightedScore(User user, Movie movie, List<Rating> ratings)
        {
            double genreScore = user.FavoriteGenres.Contains(movie.Genre) ? 3.0 : 0.0;

            double tagScore = movie.Tags
            .Count(tag => user.FavoriteGenres.Contains(tag)) * 1.5;

            double popularityScore = ratings
            .Where(r => r.MovieId == movie.Id)
            .Select(r => r.Score)
            .DefaultIfEmpty(0)
            .Average();

            double movieRatingScore = movie.Rating / 2.0;

            double finalScore =
            genreScore +
            tagScore +
            popularityScore +
            movieRatingScore;

            return finalScore;
        }
    }
}