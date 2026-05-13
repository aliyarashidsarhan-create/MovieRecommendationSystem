using MovieRecommendationSystem.Models;
using MovieRecommendationSystem.Services;
using MovieRecommendationSystem.Utilities;


namespace MovieRecommendationSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ConsoleUI.Header("AI MOVIE RECOMMENDATION SYSTEM");

            Console.ForegroundColor = ConsoleColor.Magenta;

            Console.WriteLine(@"
      ███╗   ███╗ ██████╗ ██╗   ██╗██╗███████╗
      ████╗ ████║██╔═══██╗██║   ██║██║██╔════╝
      ██╔████╔██║██║   ██║██║   ██║██║█████╗
      ██║╚██╔╝██║██║   ██║╚██╗ ██╔╝██║██╔══╝
      ██║ ╚═╝ ██║╚██████╔╝ ╚████╔╝ ██║███████╗
      ╚═╝     ╚═╝ ╚═════╝   ╚═══╝  ╚═╝╚══════╝
");

            Console.ResetColor();

            ConsoleUI.Loading("Starting system");

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
                ConsoleUI.Header("AI MOVIE RECOMMENDATION SYSTEM");
                ConsoleUI.Section("MAIN MENU");
                Console.WriteLine("1. Register");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. Exit");
                Console.Write("\nChoose option: ");

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
                        Console.ForegroundColor = ConsoleColor.Red;

                        Console.WriteLine("╔════════════════════════════╗");
                        Console.WriteLine("║      Invalid Option!      ║");
                        Console.WriteLine("╚════════════════════════════╝");

                        Console.ResetColor();
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

                ConsoleUI.Success("Registration successful!");
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
                ConsoleUI.Success($"Welcome {user.Username}!");

                ConsoleUI.Loading($"Loading profile for {user.Username}");

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
                ConsoleUI.Header($"WELCOME {user.Username.ToUpper()}");
                ConsoleUI.Section("USER DASHBOARD");
                Console.ForegroundColor = ConsoleColor.DarkYellow;

                Console.WriteLine($"Favorite Genres: {string.Join(", ", user.FavoriteGenres)}");

                Console.ResetColor();
                Console.WriteLine("1. Browse Movies");
                Console.WriteLine("2. Search Movies");
                Console.WriteLine("3. Rate Movie");
                Console.WriteLine("4. Remove Rating");
                Console.WriteLine("5. View Recommendations");
                Console.WriteLine("6. Watch History");
                Console.WriteLine("7. Top Rated Movies");
                Console.WriteLine("8. System Statistics");
                Console.WriteLine("9. Trending Movies");
                Console.WriteLine("10. Favorite Movies");
                Console.WriteLine("11. Movie Details");
                Console.WriteLine("12.Logout");
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

                        ConsoleUI.Loading("Searching movies");

                        List<Movie> results = searchService.SearchMovies(movies, keyword);

                        if (results.Count == 0)
                        {
                            Console.WriteLine("No movies found.");
                        }
                        else
                        {
                            Console.WriteLine("\nSearch Results:\n");

                            Console.ForegroundColor = ConsoleColor.Cyan;

                            Console.WriteLine("--------------------------------------------------------------------------------");
                            Console.WriteLine($"{"ID",-5} {"TITLE",-30} {"GENRE",-15} {"YEAR",-10} {"RATING",-10}");
                            Console.WriteLine("--------------------------------------------------------------------------------");

                            Console.ResetColor();

                            foreach (Movie movie in results)
                            {
                                Console.WriteLine($"{movie.Id,-5} {movie.Title,-30} {movie.Genre,-15} {movie.ReleaseYear,-10} {movie.Rating,-10}");
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
                            Console.ForegroundColor = ConsoleColor.Red;

                            Console.WriteLine("╔══════════════════════════════════════╗");
                            Console.WriteLine("║  Invalid Input! Numbers only.        ║");
                            Console.WriteLine("╚══════════════════════════════════════╝");

                            Console.ResetColor();
                        }
                        else
                        {
                            movieService.RateMovie(user, movieId, score);

                            ratingFileManager.SaveData(ratingsFile, ratings);
                            userFileManager.SaveData(usersFile, users);
                        }

                        break;

                    // Remove rating.
                    case "4":
                        movieService.DisplayMovies();

                        Console.Write("\nEnter movie ID to remove rating: ");
                        bool validRemoveId = int.TryParse(Console.ReadLine(), out int removeMovieId);

                        if (!validRemoveId)
                        {
                            Console.WriteLine("Invalid movie ID.");
                        }
                        else
                        {
                            movieService.RemoveRating(user, removeMovieId);

                            ratingFileManager.SaveData(ratingsFile, ratings);
                        }

                        break;

                    // Show movie recommendations.
                    case "5":

                        ConsoleUI.Loading("Loading AI recommendations");
                        Console.ForegroundColor = ConsoleColor.DarkCyan;

                        Console.WriteLine("\nAI Engine Analysis:");
                        Console.WriteLine("- Analyzing your ratings");
                        Console.WriteLine("- Matching favorite genres");
                        Console.WriteLine("- Comparing similar users");
                        Console.WriteLine("- Calculating recommendation scores");

                        Console.ResetColor();

                        Thread.Sleep(5000);

                        List<Movie> recommendations =
                        recommendationService.GetRecommendations(user, movies, ratings, users);

                        Console.ForegroundColor = ConsoleColor.Green;

                        Console.WriteLine("\n╔════════════════════════════════════════════╗");
                        Console.WriteLine("║          AI TOP RECOMMENDATIONS           ║");
                        Console.WriteLine("╚════════════════════════════════════════════╝");

                        Console.ResetColor();

                        if (recommendations.Count == 0)
                        {
                            ConsoleUI.Error("No recommendations yet. Rate more movies first.");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("\n----------------------------------------------------------------------------");
                            Console.WriteLine($"{"NO",-5} {"TITLE",-35} {"GENRE",-15} {"RATING",-10}");
                            Console.WriteLine("----------------------------------------------------------------------------");
                            Console.ResetColor();

                            int number = 1;

                            foreach (Movie movie in recommendations)
                            {
                                Console.WriteLine($"{number,-5} {movie.Title,-35} {movie.Genre,-15} {movie.Rating,-10}");
                                number++;
                            }

                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine("\nRecommendations are based on your ratings, favorite genres, and similar users.");
                            Console.ResetColor();
                        }

                        break;

                    // Display watch history.
                    case "6":
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

                    case "7":

                        ConsoleUI.Loading("Loading top rated movies");

                        var topMovies = movies
                        .OrderByDescending(m => m.Rating)
                        .Take(10)
                        .ToList();

                        Console.ForegroundColor = ConsoleColor.Yellow;

                        Console.WriteLine("\n╔════════════════════════════════════════════╗");
                        Console.WriteLine("║              TOP RATED MOVIES             ║");
                        Console.WriteLine("╚════════════════════════════════════════════╝");

                        Console.ResetColor();

                        Console.ForegroundColor = ConsoleColor.Cyan;

                        Console.WriteLine("\n--------------------------------------------------------------------------------");
                        Console.WriteLine($"{"NO",-5} {"TITLE",-35} {"GENRE",-15} {"RATING",-10}");
                        Console.WriteLine("--------------------------------------------------------------------------------");

                        Console.ResetColor();

                        int rank = 1;

                        foreach (var movie in topMovies)
                        {
                            Console.WriteLine($"{rank,-5} {movie.Title,-35} {movie.Genre,-15} {movie.Rating,-10}");
                            rank++;
                        }

                        break;
                        // System statistics.
                    case "8":

                        ConsoleUI.Loading("Loading system statistics");

                        Console.ForegroundColor = ConsoleColor.Magenta;

                        Console.WriteLine("\n╔════════════════════════════════════════════╗");
                        Console.WriteLine("║             SYSTEM STATISTICS             ║");
                        Console.WriteLine("╚════════════════════════════════════════════╝");

                        Console.ResetColor();

                        Console.ForegroundColor = ConsoleColor.Cyan;

                        Console.WriteLine($"\nTotal Movies      : {movies.Count}");
                        Console.WriteLine($"Total Users       : {users.Count}");
                        Console.WriteLine($"Total Ratings     : {ratings.Count}");

                        double averageRating = ratings.Count > 0
                        ? ratings.Average(r => r.Score)
                        : 0;

                        Console.WriteLine($"Average Rating    : {averageRating:F1}");

                        var mostWatchedGenre = movies
                        .GroupBy(m => m.Genre)
                        .OrderByDescending(g => g.Count())
                        .FirstOrDefault();

                        if (mostWatchedGenre != null)
                        {
                            Console.WriteLine($"Top Genre         : {mostWatchedGenre.Key}");
                        }

                        Console.ResetColor();

                        break;
                    // Trending movies based on number of ratings.
                    case "9":

                        ConsoleUI.Loading("Finding trending movies");

                        var trendingMovies = ratings
                        .GroupBy(r => r.MovieId)
                        .Select(g => new
                        {
                            MovieId = g.Key,
                            RatingCount = g.Count(),
                            AverageScore = g.Average(r => r.Score)
                        })
                        .OrderByDescending(x => x.RatingCount)
                        .ThenByDescending(x => x.AverageScore)
                        .Take(10)
                        .ToList();

                        Console.ForegroundColor = ConsoleColor.Magenta;

                        Console.WriteLine("\n╔════════════════════════════════════════════╗");
                        Console.WriteLine("║              TRENDING MOVIES                 ║");
                        Console.WriteLine("╚════════════════════════════════════════════╝");

                        Console.ResetColor();

                        if (trendingMovies.Count == 0)
                        {
                            ConsoleUI.Error("No trending movies yet.");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;

                            Console.WriteLine("\n--------------------------------------------------------------------------------");
                            Console.WriteLine($"{"NO",-5} {"TITLE",-35} {"RATINGS",-10} {"AVG SCORE",-10}");
                            Console.WriteLine("--------------------------------------------------------------------------------");

                            Console.ResetColor();

                            int trank = 1;

                            foreach (var item in trendingMovies)
                            {
                                Movie? movie = movies.FirstOrDefault(m => m.Id == item.MovieId);

                                if (movie != null)
                                {
                                    Console.WriteLine($"{trank,-5} {movie.Title,-35} {item.RatingCount,-10} {item.AverageScore,-10:F1}");
                                    trank++;
                                }
                            }
                        }

                        break;
                    // Favorite movies based on rating 5.
                    case "10":

                        ConsoleUI.Loading("Loading favorite movies");

                        var favoriteMovieIds = ratings
                        .Where(r => r.UserId == user.Id && r.Score == 5)
                        .Select(r => r.MovieId)
                        .ToList();

                        var favoriteMovies = movies
                        .Where(m => favoriteMovieIds.Contains(m.Id))
                        .ToList();

                        Console.ForegroundColor = ConsoleColor.Yellow;

                        Console.WriteLine("\n╔════════════════════════════════════════════╗");
                        Console.WriteLine("  ║              FAVORITE MOVIES               ║");
                        Console.WriteLine("  ╚════════════════════════════════════════════ ╝");

                        Console.ResetColor();

                        if (favoriteMovies.Count == 0)
                        {
                            ConsoleUI.Error("No favorite movies yet. Rate movies with 5 stars first.");
                        }
                        else
                        {
                            foreach (Movie movie in favoriteMovies)
                            {
                                Console.WriteLine($"{movie.Title} | {movie.Genre} | Rating: {movie.Rating}");
                            }
                        }

                        break;
                    // Display detailed movie information.
                    case "11":

                        movieService.DisplayMovies();

                        Console.Write("\nEnter movie ID: ");

                        bool validDetailsId =
                        int.TryParse(Console.ReadLine(), out int detailsMovieId);

                        if (!validDetailsId)
                        {
                            ConsoleUI.Error("Invalid movie ID.");
                        }
                        else
                        {
                            Movie? selectedMovie =
                            movies.FirstOrDefault(m => m.Id == detailsMovieId);

                            if (selectedMovie == null)
                            {
                                ConsoleUI.Error("Movie not found.");
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Green;

                                Console.WriteLine("\n╔════════════════════════════════════════════╗");
                                Console.WriteLine("║               MOVIE DETAILS               ║");
                                Console.WriteLine("╚════════════════════════════════════════════╝");

                                Console.ResetColor();

                                Console.WriteLine($"\nTitle        : {selectedMovie.Title}");
                                Console.WriteLine($"Genre        : {selectedMovie.Genre}");
                                Console.WriteLine($"Release Year : {selectedMovie.ReleaseYear}");
                                Console.WriteLine($"Rating       : {selectedMovie.Rating}");
                                Console.WriteLine($"Director     : {selectedMovie.Director}");

                                Console.WriteLine($"\nDescription:");
                                Console.WriteLine(selectedMovie.Description);

                                Console.WriteLine($"\nCast:");
                                foreach (string actor in selectedMovie.Cast)
                                {
                                    Console.WriteLine($"- {actor}");
                                }

                                Console.WriteLine($"\nTags:");
                                foreach (string tag in selectedMovie.Tags)
                                {
                                    Console.WriteLine($"# {tag}");
                                }
                            }
                        }

                        break;
                    // Logout from dashboard.
                    case "12":
                        ConsoleUI.Loading("Logging out");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Thank you for using our AI Movie System!");
                        Console.ResetColor();
                        return;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;

                        Console.WriteLine("╔════════════════════════════╗");
                        Console.WriteLine("║      Invalid Option!      ║");
                        Console.WriteLine("╚════════════════════════════╝");

                        Console.ResetColor();
                        break;
                }
                Console.WriteLine("\nPress any key to return to dashboard...");
                Console.ReadKey();



            }
        }
    }
}

    
