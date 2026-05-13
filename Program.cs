using MovieRecommendationSystem.Models;
using MovieRecommendationSystem.Services;
using MovieRecommendationSystem.Utilities;

namespace MovieRecommendationSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // File paths for storing JSON data.
            string basePath = AppDomain.CurrentDomain.BaseDirectory;

            string usersFile = Path.Combine(basePath, @"..\..\..\Data\users.json");
            string moviesFile = Path.Combine(basePath, @"..\..\..\Data\movies.json");
            string ratingsFile = Path.Combine(basePath, @"..\..\..\Data\ratings.json");

            // Create file managers for users, movies, and ratings.
            FileManager<User> userFileManager = new FileManager<User>();
            FileManager<Movie> movieFileManager = new FileManager<Movie>();
            FileManager<Rating> ratingFileManager = new FileManager<Rating>();

            

            // Load data from JSON files.
            List<User> users = userFileManager.LoadData(usersFile);
            List<Movie> movies = movieFileManager.LoadData(moviesFile);

            List<Rating> ratings = ratingFileManager.LoadData(ratingsFile);

            
            // Create service objects.
            AuthenticationService authService = new AuthenticationService(users);
            MovieService movieService = new MovieService(movies, ratings);
            SearchService searchService = new SearchService();
            RecommendationService recommendationService = new RecommendationService();

            // Main menu loop.
            while (true)
            {
                Console.Clear();

                // Display main menu.
                Console.WriteLine("===== Movie Recommendation System =====");
                Console.WriteLine("1. Register");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. Exit");
                Console.Write("Choose option: ");

                string choice = Console.ReadLine() ?? "";

                // Handle menu options.
                switch (choice)
                {
                    case "1":
                        Register(authService, userFileManager, usersFile, users);
                        break;

                    case "2":
                        Login(
                            authService,
                            movieService,
                            searchService,
                            recommendationService,
                            movies,
                            ratings,
                            users,
                            userFileManager,
                            ratingFileManager,
                            usersFile,
                            ratingsFile
                        );
                        break;

                    case "3":
                        Console.WriteLine("Goodbye!");
                        return;

                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        // Register a new user.
        static void Register(
            AuthenticationService authService,
            FileManager<User> userFileManager,
            string usersFile,
            List<User> users)
        {
            Console.Clear();

            Console.WriteLine("===== Register =====");

            string username;
            string password;

            // Validate username.
            do
            {
                Console.Write("Enter username: ");
                username = Console.ReadLine() ?? "";

                if (!ValidationHelper.IsValidUsername(username))
                {
                    Console.WriteLine("Username must be at least 3 characters.");
                }

            } while (!ValidationHelper.IsValidUsername(username));

            // Validate password.
            do
            {
                Console.Write("Enter password: ");
                password = Console.ReadLine() ?? "";

                if (!ValidationHelper.IsValidPassword(password))
                {
                    Console.WriteLine("Password must be at least 4 characters.");
                }

            } while (!ValidationHelper.IsValidPassword(password));

            // Get favorite genres from the user.
            Console.Write("Enter favorite genres separated by comma: ");
            string genresInput = Console.ReadLine() ?? "";

            // Convert genres string into a list.
            List<string> favoriteGenres = genresInput
                .Split(',')
                .Select(g => g.Trim())
                .Where(g => g != "")
                .ToList();

            // Register the user.
            User? user = authService.Register(username, password, favoriteGenres);

            if (user != null)
            {
                // Save updated users list.
                userFileManager.SaveData(usersFile, users);

                Console.WriteLine("Registration successful!");
            }
            else
            {
                Console.WriteLine("Registration failed.");
            }
        }

        // Login existing user.
        static void Login(
            AuthenticationService authService,
            MovieService movieService,
            SearchService searchService,
            RecommendationService recommendationService,
            List<Movie> movies,
            List<Rating> ratings,
            List<User> users,
            FileManager<User> userFileManager,
            FileManager<Rating> ratingFileManager,
            string usersFile,
            string ratingsFile)
        {
            Console.Clear();

            Console.WriteLine("===== Login =====");

            // Get login information.
            Console.Write("Enter username: ");
            string username = Console.ReadLine() ?? "";

            Console.Write("Enter password: ");
            string password = Console.ReadLine() ?? "";

            // Check if user exists.
            User? user = authService.Login(username, password);

            if (user != null)
            {
                Console.WriteLine($"Welcome {user.Username}!");

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();

                // Open dashboard after successful login.
                UserDashboard(
                    user,
                    movieService,
                    searchService,
                    recommendationService,
                    movies,
                    ratings,
                    users,
                    userFileManager,
                    ratingFileManager,
                    usersFile,
                    ratingsFile
                );
            }
            else
            {
                Console.WriteLine("Invalid username or password.");
            }
        }

        // User dashboard after login.
        static void UserDashboard(
            User user,
            MovieService movieService,
            SearchService searchService,
            RecommendationService recommendationService,
            List<Movie> movies,
            List<Rating> ratings,
            List<User> users,
            FileManager<User> userFileManager,
            FileManager<Rating> ratingFileManager,
            string usersFile,
            string ratingsFile)
        {
            while (true)
            {
                Console.Clear();

                // Dashboard menu.
                Console.WriteLine($"===== Welcome {user.Username} =====");
                Console.WriteLine("1. Browse Movies");
                Console.WriteLine("2. Search Movies");
                Console.WriteLine("3. Rate Movie");
                Console.WriteLine("4. View Recommendations");
                Console.WriteLine("5. Watch History");
                Console.WriteLine("6. Logout");
                Console.Write("Choose option: ");

                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    // Display all movies.
                    case "1":
                        movieService.DisplayMovies();
                        break;

                    // Search movies by keyword.
                    case "2":

                        Console.Write("Enter search keyword: ");
                        string keyword = Console.ReadLine() ?? "";

                        List<Movie> results = searchService.SearchMovies(movies, keyword);

                        if (results.Count == 0)
                        {
                            Console.WriteLine("No movies found.");
                        }
                        else
                        {
                            Console.WriteLine("\nSearch Results:");

                            foreach (Movie movie in results)
                            {
                                Console.WriteLine($"{movie.Id}. {movie.Title} | {movie.Genre} | {movie.ReleaseYear} | Rating: {movie.Rating}");
                            }
                        }

                        break;

                    // Rate a movie.
                    case "3":

                        movieService.DisplayMovies();

                        Console.Write("\nEnter movie ID: ");

                        bool validMovieId = int.TryParse(Console.ReadLine(), out int movieId);

                        Console.Write("Enter rating from 1 to 5: ");

                        bool validScore = int.TryParse(Console.ReadLine(), out int score);

                        if (!validMovieId || !validScore)
                        {
                            Console.WriteLine("Invalid input. Please enter numbers only.");
                        }
                        else
                        {
                            movieService.RateMovie(user, movieId, score);

                            // Save updated ratings and users.
                            ratingFileManager.SaveData(ratingsFile, ratings);
                            userFileManager.SaveData(usersFile, users);
                        }

                        break;

                    // Show movie recommendations.
                    case "4":

                        List<Movie> recommendations =
                            recommendationService.GetRecommendations(user, movies, ratings, users);

                        Console.WriteLine("\nTop Recommendations:");

                        if (recommendations.Count == 0)
                        {
                            Console.WriteLine("No recommendations yet. Rate more movies first.");
                        }
                        else
                        {
                            foreach (Movie movie in recommendations)
                            {
                                Console.WriteLine($"{movie.Title} | {movie.Genre} | Rating: {movie.Rating}");
                            }
                        }

                        break;

                    // Display watch history.
                    case "5":

                        Console.WriteLine("\nWatch History:");

                        if (user.WatchHistory.Count == 0)
                        {
                            Console.WriteLine("No watched movies yet.");
                        }
                        else
                        {
                            foreach (int id in user.WatchHistory)
                            {
                                Movie? movie = movies.FirstOrDefault(m => m.Id == id);

                                Rating? rating = ratings.FirstOrDefault(r =>
                                r.UserId == user.Id && r.MovieId == id);

                                if (movie != null)
                                {
                                    if (rating != null)
                                    {
                                        Console.WriteLine($"{movie.Title} | Your Rating: {rating.Score}");
                                    }
                                    else
                                    {
                                        Console.WriteLine(movie.Title);
                                    }
                                }
                            }
                        }
                        break;

                    // Logout from dashboard.
                    case "6":

                        Console.WriteLine("Logged out.");
                        return;

                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }
    }
}