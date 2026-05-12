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

            foreach (var strategy in _strategies)
            {
                var result = strategy.Recommend(user, movies, ratings, users);
                finalRecommendations.AddRange(result);
            }

            // Remove duplicate movies and sort by movie rating.
            return finalRecommendations
                .GroupBy(m => m.Id)
                .Select(g => g.First())
                .OrderByDescending(m => m.Rating)
                .Take(5)
                .ToList();
        }
    }
}