using System.Collections.Generic;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Interfaces
{
    /// <summary>
    /// Interface for task repository operations
    /// Demonstrates interface-based programming and dependency injection
    /// Part of Repository pattern implementation
    /// </summary>
    public interface ITaskRepository
    {
        /// <summary>
        /// Adds a new task to the repository
        /// </summary>
        void Add(TaskItem task);

        /// <summary>
        /// Updates an existing task
        /// </summary>
        void Update(TaskItem task);

        /// <summary>
        /// Deletes a task by ID
        /// </summary>
        bool Delete(string taskId);

        /// <summary>
        /// Retrieves a task by ID
        /// </summary>
        TaskItem GetById(string taskId);

        /// <summary>
        /// Retrieves all tasks
        /// </summary>
        List<TaskItem> GetAll();

        /// <summary>
        /// Saves all tasks to persistent storage
        /// </summary>
        void SaveChanges();

        /// <summary>
        /// Loads tasks from persistent storage
        /// </summary>
        void LoadData();
    }
}
