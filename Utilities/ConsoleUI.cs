using System;
using System.Collections.Generic;
using System.Text;

namespace MovieRecommendationSystem.Utilities
{
    public static class ConsoleUI
    {
        public static void Header(string title)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;

            Console.WriteLine("╔════════════════════════════════════════════════════╗");
            Console.WriteLine("║                                                    ║");
            Console.WriteLine($"║        {title.PadRight(36)}║");
            Console.WriteLine("║                                                    ║");
            Console.WriteLine("╚════════════════════════════════════════════════════╝");

            Console.ResetColor();
        }

        public static void Section(string title)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n========== {title} ==========\n");
            Console.ResetColor();
        }

        public static void Success(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void Info(string message)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void Loading(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write(message);

            for (int i = 0; i < 3; i++)
            {
                Thread.Sleep(700);
                Console.Write(".");
            }

            Console.WriteLine();
            Console.ResetColor();
        }

        public static void Wait()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\nPress any key to continue...");
            Console.ResetColor();
            Console.ReadKey();
        }
    }
}
