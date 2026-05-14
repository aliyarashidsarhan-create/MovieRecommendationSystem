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

            Console.WriteLine("\t\t\t\t╔═************═══════════════════════════════════════╗");
            Console.WriteLine("\t\t\t\t║                                                    ║");
            Console.WriteLine($"\t\t\t\t     {title.PadRight(36)}                       ");
            Console.WriteLine("\t\t\t\t║                                                    ║");
            Console.WriteLine("\t\t\t\t╚════════════════════════════════════*************═══╝\n\n");  
            Console.ResetColor();
        }

        public static string logo = @"

███╗   ███╗ ...
";

        public static string[] lines = logo.Split('\n');

        public static void DisplayLogo()
{
    int padding = (Console.WindowWidth - lines.Length) / 2;

    if (padding > 0)
    {
        Console.WriteLine(new string (' ', padding) + lines);
    }
    else
    {
        Console.WriteLine(lines);
    }

Thread.Sleep(15);
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
            Console.ForegroundColor = ConsoleColor.DarkRed;
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
            Console.ForegroundColor = ConsoleColor.Yellow;
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
