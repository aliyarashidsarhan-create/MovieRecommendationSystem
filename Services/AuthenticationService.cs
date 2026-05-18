using MovieRecommendationSystem.Models;
using MovieRecommendationSystem.Utilities;

namespace MovieRecommendationSystem.Services
{
    public class AuthenticationService
    {
        private List<User> _users;

        public AuthenticationService(List<User> users)
        {
            _users = users;
        }

        // Register new user.
        public User? Register(string username, string password, List<string> favoriteGenres)
        {
            if (!ValidationHelper.IsValidUsername(username))
            {
                ConsoleUI.Error("Username must be at least 3 characters.");
                return null;
            }

            if (!ValidationHelper.IsValidPassword(password))
            {
                ConsoleUI.Error("Password must be at least 4 characters.");
                return null;
            }

            // Prevent duplicate username.
            if (_users.Any(u => u.Username.ToLower() == username.ToLower()))
            {
                ConsoleUI.Error("Username already exists.");
                return null;
            }

            int newId = _users.Count == 0 ? 1 : _users.Max(u => u.Id) + 1;

            User newUser = new User(newId, username, password, favoriteGenres);
            _users.Add(newUser);

            return newUser;
        }

        // Login existing user.
        public User? Login(string username, string password)
        {
            return _users.FirstOrDefault(u =>
            u.Username.ToLower() == username.ToLower() &&
            u.Password == password);
        }
    }
}