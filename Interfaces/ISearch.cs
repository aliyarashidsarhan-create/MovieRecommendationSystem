using MovieRecommendationSystem.Models;

namespace MovieRecommendationSystem.Interfaces
{
    public interface ISearch
    {
        List<Movie> SearchMovies(List<Movie> movies, string keyword);
    }
}