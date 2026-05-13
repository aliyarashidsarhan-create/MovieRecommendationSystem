using MovieRecommendationSystem.Models;

namespace MovieRecommendationSystem.Interfaces
{
    // Interface for recommendation strategies.
    public interface IRecommendation
    {
        List<Movie> Recommend(User user, List<Movie> movies, List<Rating> ratings, List<User> users);
    }
}