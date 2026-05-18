using MovieRecommendationSystem.Interfaces;
using MovieRecommendationSystem.Models;

namespace MovieRecommendationSystem.Services
{
    public class SearchService : ISearch
    {
        // Search by title, genre, director, year, or rating.
        public List<Movie> SearchMovies(List<Movie> movies, string keyword)
        {
            keyword = keyword.ToLower();

            return movies.Where(m =>
                m.Title.ToLower().Contains(keyword) ||
                m.Genre.ToLower().Contains(keyword) ||
                m.Director.ToLower().Contains(keyword) ||
                m.ReleaseYear.ToString().Contains(keyword) ||
                m.Rating.ToString().Contains(keyword)
            ).ToList();
        }
    }
}