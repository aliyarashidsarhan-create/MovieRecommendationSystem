using System;
using System.Collections.Generic;
using System.Text;

namespace MovieRecommendationSystem.Models
{
    // Abstract class because Person is a general idea.
    // User and Admin will inherit from it.
    public abstract class Person
    {
        // Common properties for all people in the system.
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        // Constructor to initialize common data.
        public Person(int id, string username, string password)
        {
            Id = id;
            Username = username;
            Password = password;
        }
    }
}
