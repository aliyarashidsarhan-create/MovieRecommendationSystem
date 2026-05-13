using System;
using System.Collections.Generic;
using System.Text;

namespace MovieRecommendationSystem.Models
{
    // User inherits from Person.
    // This shows inheritance in OOP.
    public class User : Person
    {
        // List of genres the user likes.
        public List<string> FavoriteGenres { get; set; }

        // List of watched movie IDs.
        public List<int> WatchHistory { get; set; }

        public User(int id, string username, string password, List<string> favoriteGenres)
            : base(id, username, password)
        {
            FavoriteGenres = favoriteGenres;
            WatchHistory = new List<int>();
        }
    }
}