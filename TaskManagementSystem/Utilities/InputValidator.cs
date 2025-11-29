using System;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Utilities
{
    /// <summary>
    /// Utility class for validating and parsing user input
    /// Reduces code duplication and improves maintainability
    /// </summary>
    public static class InputValidator
    {
        /// <summary>
        /// Validates and reads a non-empty string
        /// </summary>
        public static string ReadNonEmptyString(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine()?.Trim();
                
                if (!string.IsNullOrWhiteSpace(input))
                {
                    return input;
                }
                
                Console.WriteLine("Input cannot be empty. Please try again.");
            }
        }

        /// <summary>
        /// Validates and reads a date
        /// </summary>
        public static DateTime ReadDate(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine()?.Trim();
                
                if (DateTime.TryParse(input, out DateTime date))
                {
                    return date;
                }
                
                Console.WriteLine("Invalid date format. Please use format: MM/DD/YYYY or YYYY-MM-DD");
            }
        }

        /// <summary>
        /// Validates and reads an integer within a range
        /// </summary>
        public static int ReadInt(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine()?.Trim();
                
                if (int.TryParse(input, out int value) && value >= min && value <= max)
                {
                    return value;
                }
                
                Console.WriteLine($"Please enter a number between {min} and {max}.");
            }
        }

        /// <summary>
        /// Validates and reads a TaskPriority enum
        /// </summary>
        public static TaskPriority ReadPriority(string prompt)
        {
            Console.WriteLine(prompt);
            Console.WriteLine("1. Low");
            Console.WriteLine("2. Medium");
            Console.WriteLine("3. High");
            
            int choice = ReadInt("Select priority (1-3): ", 1, 3);
            return (TaskPriority)choice;
        }

        /// <summary>
        /// Validates and reads a TaskStatus enum
        /// </summary>
        public static TaskStatus ReadStatus(string prompt)
        {
            Console.WriteLine(prompt);
            Console.WriteLine("1. To-Do");
            Console.WriteLine("2. In Progress");
            Console.WriteLine("3. Done");
            
            int choice = ReadInt("Select status (1-3): ", 1, 3);
            return (TaskStatus)choice;
        }

        /// <summary>
        /// Reads an optional string (allows empty input)
        /// </summary>
        public static string ReadOptionalString(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine()?.Trim() ?? string.Empty;
        }

        /// <summary>
        /// Prompts for yes/no confirmation
        /// </summary>
        public static bool ReadConfirmation(string prompt)
        {
            while (true)
            {
                Console.Write($"{prompt} (y/n): ");
                string input = Console.ReadLine()?.Trim().ToLower();
                
                if (input == "y" || input == "yes")
                    return true;
                if (input == "n" || input == "no")
                    return false;
                
                Console.WriteLine("Please enter 'y' or 'n'.");
            }
        }

        /// <summary>
        /// Pauses execution until user presses a key
        /// </summary>
        public static void PressAnyKeyToContinue()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey(true);
        }
    }
}
