using System;
using System.Collections.Generic;
using System.Linq;
using TaskManagementSystem.Models;
using TaskManagementSystem.Services;
using TaskManagementSystem.Utilities;

namespace TaskManagementSystem.UI
{
    /// <summary>
    /// Console-based user interface for the Task Management System
    /// Demonstrates encapsulation by keeping UI logic separate from business logic
    /// </summary>
    public class ConsoleUI
    {
        private readonly TaskManager _taskManager;
        private bool _isRunning;

        public ConsoleUI(TaskManager taskManager)
        {
            _taskManager = taskManager ?? throw new ArgumentNullException(nameof(taskManager));
            _isRunning = true;
        }

        /// <summary>
        /// Starts the main application loop
        /// </summary>
        public void Run()
        {
            Console.Clear();
            DisplayWelcome();

            while (_isRunning)
            {
                try
                {
                    DisplayMainMenu();
                    int choice = InputValidator.ReadInt("Enter your choice: ", 0, 9);
                    Console.WriteLine();

                    ProcessMenuChoice(choice);
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\nError: {ex.Message}");
                    Console.ResetColor();
                    InputValidator.PressAnyKeyToContinue();
                }
            }
        }

        private void DisplayWelcome()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔════════════════════════════════════════════════════════╗");
            Console.WriteLine("║      TASK & PROJECT TRACKING SYSTEM                    ║");
            Console.WriteLine("║      Professional Task Management Solution             ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════╝");
            Console.ResetColor();
            Console.WriteLine();
        }

        private void DisplayMainMenu()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("═══════════════ MAIN MENU ═══════════════");
            Console.ResetColor();
            Console.WriteLine("1. Add New Task");
            Console.WriteLine("2. Update Task Status");
            Console.WriteLine("3. Search Tasks");
            Console.WriteLine("4. Sort Tasks");
            Console.WriteLine("5. View All Tasks");
            Console.WriteLine("6. Generate Reports");
            Console.WriteLine("7. Delete Task");
            Console.WriteLine("8. View Statistics");
            Console.WriteLine("9. About (Design Patterns)");
            Console.WriteLine("0. Exit");
            Console.WriteLine("═════════════════════════════════════════");
        }

        private void ProcessMenuChoice(int choice)
        {
            switch (choice)
            {
                case 1:
                    AddNewTask();
                    break;
                case 2:
                    UpdateTaskStatus();
                    break;
                case 3:
                    SearchTasks();
                    break;
                case 4:
                    SortTasks();
                    break;
                case 5:
                    ViewAllTasks();
                    break;
                case 6:
                    GenerateReports();
                    break;
                case 7:
                    DeleteTask();
                    break;
                case 8:
                    ViewStatistics();
                    break;
                case 9:
                    ShowAbout();
                    break;
                case 0:
                    Exit();
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }

        private void AddNewTask()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("═══ ADD NEW TASK ═══");
            Console.ResetColor();
            Console.WriteLine();

            string title = InputValidator.ReadNonEmptyString("Task Title: ");
            string description = InputValidator.ReadOptionalString("Description (optional): ");
            DateTime dueDate = InputValidator.ReadDate("Due Date (MM/DD/YYYY): ");
            TaskPriority priority = InputValidator.ReadPriority("\nSelect Priority:");
            string assignee = InputValidator.ReadOptionalString("Assignee (optional): ");

            if (string.IsNullOrWhiteSpace(assignee))
                assignee = "Unassigned";

            var task = _taskManager.CreateTask(title, description, dueDate, priority, assignee);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n✓ Task created successfully!");
            Console.WriteLine($"Task ID: {task.TaskId}");
            Console.ResetColor();

            InputValidator.PressAnyKeyToContinue();
        }

        private void UpdateTaskStatus()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("═══ UPDATE TASK STATUS ═══");
            Console.ResetColor();
            Console.WriteLine();

            string taskId = InputValidator.ReadNonEmptyString("Enter Task ID: ");
            
            var task = _taskManager.SearchTaskById(taskId);
            if (task == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n✗ Task with ID '{taskId}' not found.");
                Console.ResetColor();
                InputValidator.PressAnyKeyToContinue();
                return;
            }

            Console.WriteLine($"\nCurrent Task: {task.Title}");
            Console.WriteLine($"Current Status: {task.Status}");
            Console.WriteLine();

            TaskStatus newStatus = InputValidator.ReadStatus("Select New Status:");

            _taskManager.UpdateTaskStatus(taskId, newStatus);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n✓ Task status updated successfully to: {newStatus}");
            Console.ResetColor();

            InputValidator.PressAnyKeyToContinue();
        }

        private void SearchTasks()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("═══ SEARCH TASKS ═══");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("1. Search by Title (Linear Search)");
            Console.WriteLine("2. Search by Assignee (Linear Search)");
            Console.WriteLine("3. Search by ID (Binary Search)");
            Console.WriteLine();

            int choice = InputValidator.ReadInt("Select search type (1-3): ", 1, 3);
            Console.WriteLine();

            List<TaskItem> results = new List<TaskItem>();

            switch (choice)
            {
                case 1:
                    string titleSearch = InputValidator.ReadNonEmptyString("Enter title to search: ");
                    results = _taskManager.SearchTasksByTitle(titleSearch);
                    Console.WriteLine($"\nUsing: Linear Search Algorithm O(n)");
                    break;
                case 2:
                    string assigneeSearch = InputValidator.ReadNonEmptyString("Enter assignee to search: ");
                    results = _taskManager.SearchTasksByAssignee(assigneeSearch);
                    Console.WriteLine($"\nUsing: Linear Search Algorithm O(n)");
                    break;
                case 3:
                    string idSearch = InputValidator.ReadNonEmptyString("Enter Task ID: ");
                    var task = _taskManager.SearchTaskById(idSearch);
                    if (task != null)
                        results.Add(task);
                    Console.WriteLine($"\nUsing: Binary Search Algorithm O(log n)");
                    break;
            }

            Console.WriteLine();
            DisplayTaskList(results, "Search Results");
            InputValidator.PressAnyKeyToContinue();
        }

        private void SortTasks()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("═══ SORT TASKS ═══");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("1. Sort by Priority (High to Low) - QuickSort");
            Console.WriteLine("2. Sort by Due Date (Earliest First) - MergeSort");
            Console.WriteLine();

            int choice = InputValidator.ReadInt("Select sort type (1-2): ", 1, 2);
            Console.WriteLine();

            List<TaskItem> sortedTasks;

            switch (choice)
            {
                case 1:
                    sortedTasks = _taskManager.GetTasksSortedByPriority();
                    Console.WriteLine("Using: QuickSort Algorithm - O(n log n) average");
                    DisplayTaskList(sortedTasks, "Tasks Sorted by Priority");
                    break;
                case 2:
                    sortedTasks = _taskManager.GetTasksSortedByDueDate();
                    Console.WriteLine("Using: MergeSort Algorithm - O(n log n) guaranteed");
                    DisplayTaskList(sortedTasks, "Tasks Sorted by Due Date");
                    break;
            }

            InputValidator.PressAnyKeyToContinue();
        }

        private void ViewAllTasks()
        {
            Console.Clear();
            var tasks = _taskManager.GetAllTasks();
            DisplayTaskList(tasks, "All Tasks");
            InputValidator.PressAnyKeyToContinue();
        }

        private void GenerateReports()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("═══ GENERATE REPORTS ═══");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("1. Overdue Tasks Report");
            Console.WriteLine("2. Upcoming Tasks Report (Next 7 Days)");
            Console.WriteLine();

            int choice = InputValidator.ReadInt("Select report type (1-2): ", 1, 2);
            Console.WriteLine();

            switch (choice)
            {
                case 1:
                    var overdueTasks = _taskManager.GetOverdueTasks();
                    DisplayTaskList(overdueTasks, "OVERDUE TASKS REPORT");
                    break;
                case 2:
                    var upcomingTasks = _taskManager.GetUpcomingTasks(7);
                    DisplayTaskList(upcomingTasks, "UPCOMING TASKS REPORT (Next 7 Days)");
                    break;
            }

            InputValidator.PressAnyKeyToContinue();
        }

        private void DeleteTask()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("═══ DELETE TASK ═══");
            Console.ResetColor();
            Console.WriteLine();

            string taskId = InputValidator.ReadNonEmptyString("Enter Task ID to delete: ");
            
            var task = _taskManager.SearchTaskById(taskId);
            if (task == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n✗ Task with ID '{taskId}' not found.");
                Console.ResetColor();
                InputValidator.PressAnyKeyToContinue();
                return;
            }

            Console.WriteLine($"\nTask to delete: {task.Title}");
            bool confirm = InputValidator.ReadConfirmation("Are you sure you want to delete this task?");

            if (confirm)
            {
                _taskManager.DeleteTask(taskId);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n✓ Task deleted successfully.");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("\nDeletion cancelled.");
            }

            InputValidator.PressAnyKeyToContinue();
        }

        private void ViewStatistics()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("═══ TASK STATISTICS ═══");
            Console.ResetColor();
            Console.WriteLine();

            var stats = _taskManager.GetTaskStatistics();

            Console.WriteLine($"Total Tasks:        {stats["Total"]}");
            Console.WriteLine($"To-Do:              {stats["ToDo"]}");
            Console.WriteLine($"In Progress:        {stats["InProgress"]}");
            Console.WriteLine($"Done:               {stats["Done"]}");
            Console.WriteLine($"Overdue:            {stats["Overdue"]}");
            Console.WriteLine($"High Priority:      {stats["High Priority"]}");

            InputValidator.PressAnyKeyToContinue();
        }

        private void ShowAbout()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("═══ DESIGN PATTERNS IMPLEMENTED ═══");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("1. SINGLETON PATTERN");
            Console.WriteLine("   - FileLogger class ensures single instance");
            Console.WriteLine("   - Thread-safe implementation with double-check locking");
            Console.WriteLine();
            Console.WriteLine("2. REPOSITORY PATTERN");
            Console.WriteLine("   - TaskRepository separates data access from business logic");
            Console.WriteLine("   - Implements ITaskRepository interface for testability");
            Console.WriteLine();
            Console.WriteLine("3. FACTORY PATTERN");
            Console.WriteLine("   - TaskFactory centralizes task creation");
            Console.WriteLine("   - Generates unique IDs and validates input");
            Console.WriteLine();
            Console.WriteLine("ALGORITHMS IMPLEMENTED:");
            Console.WriteLine("   - Linear Search: O(n) - for title/assignee search");
            Console.WriteLine("   - Binary Search: O(log n) - for ID search");
            Console.WriteLine("   - QuickSort: O(n log n) - for priority sorting");
            Console.WriteLine("   - MergeSort: O(n log n) - for date sorting");
            Console.WriteLine();
            InputValidator.PressAnyKeyToContinue();
        }

        private void DisplayTaskList(List<TaskItem> tasks, string title)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"═══ {title.ToUpper()} ═══");
            Console.ResetColor();
            Console.WriteLine();

            if (tasks == null || tasks.Count == 0)
            {
                Console.WriteLine("No tasks found.");
                return;
            }

            Console.WriteLine($"Total: {tasks.Count} task(s)\n");

            foreach (var task in tasks)
            {
                // Color code by status - C# 7.3 compatible
                ConsoleColor color;
                if (task.Status == TaskStatus.Done)
                {
                    color = ConsoleColor.Green;
                }
                else if (task.Status == TaskStatus.InProgress)
                {
                    color = ConsoleColor.Yellow;
                }
                else
                {
                    color = ConsoleColor.White;
                }

                if (task.IsOverdue())
                    color = ConsoleColor.Red;

                Console.ForegroundColor = color;
                Console.WriteLine(task.ToString());
                Console.ResetColor();

                if (!string.IsNullOrWhiteSpace(task.Description))
                {
                    Console.WriteLine($"   Description: {task.Description}");
                }
                Console.WriteLine();
            }
        }

        private void Exit()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Thank you for using Task & Project Tracking System!");
            Console.WriteLine("All data has been saved. Goodbye!");
            Console.ResetColor();
            _isRunning = false;
        }
    }
}
