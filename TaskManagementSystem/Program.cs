using System;
using TaskManagementSystem.Interfaces;
using TaskManagementSystem.Services;
using TaskManagementSystem.UI;

namespace TaskManagementSystem
{
    /// <summary>
    /// Main entry point for the Task Management System
    /// Demonstrates encapsulation by keeping Program.cs minimal
    /// Initializes dependencies and starts the application
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Initialize services using design patterns
                ILogger logger = FileLogger.Instance; // Singleton pattern
                ITaskRepository repository = new TaskRepository(logger); // Repository pattern
                TaskManager taskManager = new TaskManager(repository, logger);

                // Log application start
                logger.LogInfo("=== Task Management System Started ===");

                // Start the console UI
                ConsoleUI ui = new ConsoleUI(taskManager);
                ui.Run();

                // Log application exit
                logger.LogInfo("=== Task Management System Exited ===");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Fatal Error: {ex.Message}");
                Console.WriteLine("Please check the system.log file for details.");
                Console.ResetColor();
                
                FileLogger.Instance.LogException(ex);
                
                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
            }
        }
    }
}

