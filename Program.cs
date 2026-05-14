using MovieRecommendationSystem.Models;
using MovieRecommendationSystem.Services;
using MovieRecommendationSystem.Utilities;
using System.Threading;


namespace MovieRecommendationSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ConsoleUI.Header("AI MOVIE RECOMMENDATION SYSTEM\n");

            Console.ForegroundColor = ConsoleColor.Blue;

            string logo = @"
███╗   ███╗ ██████╗ ██╗   ██╗██╗███████╗
████╗ ████║██╔═══██╗██║   ██║██║██╔════╝
██╔████╔██║██║   ██║██║   ██║██║█████╗
██║╚██╔╝██║██║   ██║╚██╗ ██╔╝██║██╔══╝
██║ ╚═╝ ██║╚██████╔╝ ╚████╔╝ ██║███████╗
╚═╝     ╚═╝ ╚═════╝   ╚═══╝  ╚═╝╚══════╝
";


            string[] lines = logo.Split('\n');

            foreach (string line in lines)
            {
                int padding = (Console.WindowWidth - line.Length) / 2;

                if (padding > 0)
                {
                    Console.WriteLine(new string(' ', padding) + line);
                }
                else
                {
                    Console.WriteLine(line);
                }

                Thread.Sleep(80);
            }

            Console.ResetColor();

            ConsoleUI.Loading("\n\t\t\t\t\t\tStarting system");

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
                // Display professional main menu.
                ConsoleUI.Header("AI MOVIE RECOMMENDATION SYSTEM");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\t\t\t\t  ╔══════════════════════════════════════════════╗");
                Console.WriteLine("\t\t\t\t  ║                  MAIN MENU                   ║");
                Console.WriteLine("\t\t\t\t  ║                                              ║");
                Console.WriteLine("\t\t\t\t  ╠══════════════════════════════════════════════╣");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\t\t\t\t  ║  [1]    Create New Account                   ║");
                Console.WriteLine("\t\t\t\t  ║                                              ║");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\t\t\t\t  ║  [2]  Login to Your Account                  ║");
                Console.WriteLine("\t\t\t\t  ║                                              ║");
                Console.ForegroundColor = ConsoleColor.White;
                    ;
                Console.WriteLine("\t\t\t\t  ║  [3]  Exit System                            ║");
                Console.WriteLine("\t\t\t\t  ║                                              ║");
                Console.ForegroundColor = ConsoleColor.Yellow; 
                Console.WriteLine("\t\t\t\t  ╚══════════════════════════════════════════════╝");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("\n\n\t\t\t\t\t Select an option: ");
                Console.ResetColor();


                string choice = Console.ReadLine() ?? "";

                // Handle menu options.
                switch (choice)
                {
                    case "1":
                        ConsoleUI.Loading("\n\t\t\t\t\t\tOpening registration page");
                        Register(authService, userFileManager, usersFile, users);
                        break;

                    case "2":
                        ConsoleUI.Loading("\n\t\t\t\t\tOpening login page");
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
                        ConsoleUI.Loading("\n\t\t\t\t\t\tClosing system");

                        Console.ForegroundColor = ConsoleColor.Yellow;

                        Console.WriteLine("\n\n\t\t\t\t\t\tThank you for using our AI Movie Recommendation System!");

                        Console.ResetColor();
                        return;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;

                        Console.WriteLine("╔════════════════════════════╗");
                        Console.WriteLine("║      Invalid Option!       ║");
                        Console.WriteLine("╚════════════════════════════╝");

                        Console.ResetColor();
                        break;
                }

                Console.WriteLine("\n\n\t\t\t\t\t\tPress any key to continue...");
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

            Console.WriteLine("\n\t\t\t\t\t\t===== Register =====");

            string username;
            string password;

            // Validate username.
            do
            {
                Console.Write("\n\t\t\t\t\t\tEnter username: ");
                username = Console.ReadLine() ?? "";

                if (!ValidationHelper.IsValidUsername(username))
                {
                    Console.WriteLine("\n\t\t\t\t\t\tUsername must be at least 3 characters.");
                }

            } while (!ValidationHelper.IsValidUsername(username));

            // Validate password.
            do
            {
                Console.Write("\n\t\t\t\t\t\tEnter password: ");
                password = Console.ReadLine() ?? "";

                if (!ValidationHelper.IsValidPassword(password))
                {
                    Console.WriteLine("\n\t\t\t\t\t\tPassword must be at least 4 characters.");
                }

            } while (!ValidationHelper.IsValidPassword(password));

            // Get favorite genres from the user.
            Console.Write("\n\t\t\t\t\t\tEnter favorite genres separated by comma: ");
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

                ConsoleUI.Success("\n\t\t\t\t\t\tRegistration successful!");
            }
            else
            {
                Console.WriteLine("\n\t\t\t\t\t\tRegistration failed.");
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

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("\t\t\t\t╔══════════════════════════════════════════════╗");
                Console.WriteLine("\t\t\t\t║                USER DASHBOARD                ║");
                Console.WriteLine("\t\t\t\t╠══════════════════════════════════════════════╣");

                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine($"\t\t\t\t║ Favorite Genres: {string.Join(", ", user.FavoriteGenres).PadRight(26)}  ║");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("\t\t\t\t╠══════════════════════════════════════════════╣");

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\t\t\t\t║  [1]  Browse Movies                          ║");
                Console.WriteLine("\t\t\t\t║  [2]  Search Movies                          ║");
                Console.WriteLine("\t\t\t\t║  [3]  Rate Movie                             ║");
                Console.WriteLine("\t\t\t\t║  [4]  Remove Rating                          ║");
                Console.WriteLine("\t\t\t\t║  [5]  View AI Recommendations                ║");
                Console.WriteLine("\t\t\t\t║  [6]  Watch History                          ║");
                Console.WriteLine("\t\t\t\t║  [7]  Top Rated Movies                       ║");
                Console.WriteLine("\t\t\t\t║  [8]  System Statistics                      ║");
                Console.WriteLine("\t\t\t\t║  [9]  Trending Movies                        ║");
                Console.WriteLine("\t\t\t\t║  [10] Favorite Movies                        ║");
                Console.WriteLine("\t\t\t\t║  [11] Movie Details                          ║");
                Console.WriteLine("\t\t\t\t║  [12] Logout                                 ║");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("\t\t\t\t╚══════════════════════════════════════════════╝");

                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\n \t\t\t\tSelect an option: ");
                Console.ResetColor();

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

                            Console.ForegroundColor = ConsoleColor.DarkGray;

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

                        string[] steps =
 {
"Analyzing your ratings...",
"Matching favorite genres...",
"Comparing similar users...",
"Calculating recommendation scores..."
};

                        foreach (string step in steps)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;

                            Console.Write($"\r{step.PadRight(50)}");

                            Thread.Sleep(900);
                        }

                        Console.Clear();

                        Thread.Sleep(5000);

                        List<Movie> recommendations =
                        recommendationService.GetRecommendations(user, movies, ratings, users);

                        Console.ForegroundColor = ConsoleColor.Green;

                        Console.WriteLine("  ╔════════════════════════════════════════════╗");
                        Console.WriteLine("          AI TOP RECOMMENDATIONS                ");
                        Console.WriteLine("  ╚════════════════════════════════════════════╝");

                        Console.ResetColor();

                        if (recommendations.Count == 0)
                        {
                            ConsoleUI.Error("No recommendations yet. Rate more movies first.");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGray;
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

                        Console.WriteLine("  ╔══════════════════════════════════════════════╗");
                        Console.WriteLine("  ║              TOP RATED MOVIES                ║");
                        Console.WriteLine("  ╚══════════════════════════════════════════════╝");

                        Console.ResetColor();

                        Console.ForegroundColor = ConsoleColor.DarkGray;

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

                        ConsoleUI.Loading("\n\t\t\t\tLoading system statistics");

                        Console.ForegroundColor = ConsoleColor.White;

                        Console.WriteLine("\t\t\t\t  ╔════════════════════════════════════════════╗");
                        Console.WriteLine("\t\t\t\t  ║             SYSTEM STATISTICS              ║");
                        Console.WriteLine("\t\t\t\t  ╚════════════════════════════════════════════╝");

                        Console.ResetColor();

                        Console.ForegroundColor = ConsoleColor.DarkGray;

                        Console.WriteLine($"\t\t\t\t\tTotal Movies      : {movies.Count}");
                        Console.WriteLine($"\t\t\t\t\tTotal Users       : {users.Count}");
                        Console.WriteLine($"\t\t\t\t\tTotal Ratings     : {ratings.Count}");

                        double averageRating = ratings.Count > 0
                        ? ratings.Average(r => r.Score)
                        : 0;

                        Console.WriteLine($"\t\t\t\t\tAverage Rating    : {averageRating:F1}");

                        var mostWatchedGenre = movies
                        .GroupBy(m => m.Genre)
                        .OrderByDescending(g => g.Count())
                        .FirstOrDefault();

                        if (mostWatchedGenre != null)
                        {
                            Console.WriteLine($"\t\t\t\t\tTop Genre         : {mostWatchedGenre.Key}");
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

                        Console.ForegroundColor = ConsoleColor.DarkGray;

                        Console.WriteLine("  ╔════════════════════════════════════════════╗");
                        Console.WriteLine("  ║              TRENDING MOVIES               ║");
                        Console.WriteLine("  ╚════════════════════════════════════════════╝");

                        Console.ResetColor();

                        if (trendingMovies.Count == 0)
                        {
                            ConsoleUI.Error("No trending movies yet.");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGray;

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

                        Console.Clear();

                        ConsoleUI.Loading("Loading favorite movies");

                        var favoriteMovieIds = ratings
                        .Where(r => r.UserId == user.Id && r.Score == 5)
                        .Select(r => r.MovieId)
                        .ToList();

                        var favoriteMovies = movies
                        .Where(m => favoriteMovieIds.Contains(m.Id))
                        .ToList();

                        Console.ForegroundColor = ConsoleColor.DarkYellow;

                        Console.WriteLine("\n==============================================================");
                        Console.WriteLine("                     FAVORITE MOVIES                          ");
                        Console.WriteLine("==============================================================");

                        Console.ResetColor();

                        if (favoriteMovies.Count == 0)
                        {
                            ConsoleUI.Error("No favorite movies yet.");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Gray;

                            foreach (Movie movie in favoriteMovies)
                            {
                                Console.WriteLine($"\n:clapper: {movie.Title}");
                                Console.WriteLine($"   Genre  : {movie.Genre}");
                                Console.WriteLine($"   Rating : {movie.Rating}");
                            }

                            Console.ResetColor();
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

                                Console.WriteLine("  ╔════════════════════════════════════════════╗");
                                Console.WriteLine("  ║               MOVIE DETAILS                ║");
                                Console.WriteLine("  ╚════════════════════════════════════════════╝");

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
                        Console.WriteLine("║      Invalid Option!       ║");
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

    
