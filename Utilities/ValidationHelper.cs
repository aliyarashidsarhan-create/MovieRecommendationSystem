using System;
using System.Collections.Generic;
using System.Text;

namespace MovieRecommendationSystem.Utilities
{
    public static class ValidationHelper
    {
        // Check if username is valid.
        public static bool IsValidUsername(string username)
        {
            return !string.IsNullOrWhiteSpace(username) && username.Length >= 3;
        }

        // Check if password is valid.
        public static bool IsValidPassword(string password)
        {
            return !string.IsNullOrWhiteSpace(password) && password.Length >= 4;
        }

        // Check rating between 1 and 5.
        public static bool IsValidRating(int rating)
        {
            return rating >= 1 && rating <= 5;
        }
    }
}