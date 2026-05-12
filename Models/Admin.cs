using System;
using System.Collections.Generic;
using System.Text;

namespace MovieRecommendationSystem.Models
{
    // Admin also inherits from Person.
    public class Admin : Person
    {
        public Admin(int id, string username, string password)
            : base(id, username, password)
        {
        }
    }

}
