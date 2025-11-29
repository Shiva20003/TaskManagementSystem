using System;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Services
{
    /// <summary>
    /// Factory class for creating TaskItem objects
    /// Implements Factory pattern for centralized object creation
    /// Ensures consistent task creation with validation
    /// </summary>
    public class TaskFactory
    {
        /// <summary>
        /// Creates a new task with a unique ID
        /// </summary>
        public static TaskItem CreateTask(string title, string description, DateTime dueDate, 
                                         TaskPriority priority, string assignee)
        {
            // Generate unique task ID using GUID prefix
            string taskId = GenerateUniqueId();
            
            return new TaskItem(taskId, title, description, dueDate, priority, assignee);
        }

        /// <summary>
        /// Generates a unique task ID
        /// Format: TASK-XXXXXX (6 character alphanumeric)
        /// </summary>
        private static string GenerateUniqueId()
        {
            string guid = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();
            return $"TASK-{guid}";
        }

        /// <summary>
        /// Creates a task with default values for quick entry
        /// </summary>
        public static TaskItem CreateQuickTask(string title, DateTime dueDate)
        {
            return CreateTask(title, string.Empty, dueDate, TaskPriority.Medium, "Unassigned");
        }
    }
}
