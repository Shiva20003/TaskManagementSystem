using System;
using System.Collections.Generic;
using System.Linq;
using TaskManagementSystem.Algorithms;
using TaskManagementSystem.Interfaces;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Services
{
    /// <summary>
    /// Core business logic for task management operations
    /// Orchestrates repository, algorithms, and logging
    /// </summary>
    public class TaskManager
    {
        private readonly ITaskRepository _repository;
        private readonly ILogger _logger;

        public TaskManager(ITaskRepository repository, ILogger logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Creates and adds a new task
        /// </summary>
        public TaskItem CreateTask(string title, string description, DateTime dueDate, 
                                   TaskPriority priority, string assignee)
        {
            try
            {
                var task = TaskFactory.CreateTask(title, description, dueDate, priority, assignee);
                _repository.Add(task);
                _logger.LogInfo($"Task created successfully: {task.TaskId}");
                return task;
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw;
            }
        }

        /// <summary>
        /// Updates the status of a task
        /// </summary>
        public void UpdateTaskStatus(string taskId, TaskStatus newStatus)
        {
            try
            {
                var task = _repository.GetById(taskId);
                if (task == null)
                {
                    _logger.LogWarning($"Task not found: {taskId}");
                    throw new InvalidOperationException($"Task with ID '{taskId}' not found.");
                }

                var oldStatus = task.Status;
                task.UpdateStatus(newStatus);
                _repository.Update(task);
                _logger.LogInfo($"Task {taskId} status updated: {oldStatus} -> {newStatus}");
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw;
            }
        }

        /// <summary>
        /// Gets all tasks
        /// </summary>
        public List<TaskItem> GetAllTasks()
        {
            return _repository.GetAll();
        }

        /// <summary>
        /// Searches tasks by title using linear search
        /// </summary>
        public List<TaskItem> SearchTasksByTitle(string searchTerm)
        {
            var allTasks = _repository.GetAll();
            var results = SearchAlgorithms.LinearSearchByTitle(allTasks, searchTerm);
            _logger.LogInfo($"Search by title '{searchTerm}': {results.Count} results found");
            return results;
        }

        /// <summary>
        /// Searches tasks by assignee using linear search
        /// </summary>
        public List<TaskItem> SearchTasksByAssignee(string assignee)
        {
            var allTasks = _repository.GetAll();
            var results = SearchAlgorithms.LinearSearchByAssignee(allTasks, assignee);
            _logger.LogInfo($"Search by assignee '{assignee}': {results.Count} results found");
            return results;
        }

        /// <summary>
        /// Searches for a task by ID using binary search
        /// </summary>
        public TaskItem SearchTaskById(string taskId)
        {
            var allTasks = _repository.GetAll();
            var result = SearchAlgorithms.BinarySearchById(allTasks, taskId);
            _logger.LogInfo($"Binary search by ID '{taskId}': {(result != null ? "Found" : "Not found")}");
            return result;
        }

        /// <summary>
        /// Sorts tasks by priority (High to Low) using QuickSort
        /// </summary>
        public List<TaskItem> GetTasksSortedByPriority()
        {
            var tasks = _repository.GetAll();
            SortingAlgorithms.QuickSortByPriority(tasks);
            _logger.LogInfo($"Tasks sorted by priority (QuickSort): {tasks.Count} tasks");
            return tasks;
        }

        /// <summary>
        /// Sorts tasks by due date (earliest first) using MergeSort
        /// </summary>
        public List<TaskItem> GetTasksSortedByDueDate()
        {
            var tasks = _repository.GetAll();
            SortingAlgorithms.MergeSortByDueDate(tasks);
            _logger.LogInfo($"Tasks sorted by due date (MergeSort): {tasks.Count} tasks");
            return tasks;
        }

        /// <summary>
        /// Generates a report of overdue tasks
        /// </summary>
        public List<TaskItem> GetOverdueTasks()
        {
            var allTasks = _repository.GetAll();
            var overdueTasks = allTasks.Where(t => t.IsOverdue()).ToList();
            _logger.LogInfo($"Overdue tasks report generated: {overdueTasks.Count} tasks");
            return overdueTasks;
        }

        /// <summary>
        /// Generates a report of upcoming tasks (due within specified days)
        /// </summary>
        public List<TaskItem> GetUpcomingTasks(int days = 7)
        {
            var allTasks = _repository.GetAll();
            var upcomingTasks = allTasks.Where(t => t.IsUpcoming(days)).ToList();
            _logger.LogInfo($"Upcoming tasks report generated (next {days} days): {upcomingTasks.Count} tasks");
            return upcomingTasks;
        }

        /// <summary>
        /// Deletes a task
        /// </summary>
        public bool DeleteTask(string taskId)
        {
            try
            {
                bool deleted = _repository.Delete(taskId);
                if (deleted)
                {
                    _logger.LogInfo($"Task deleted: {taskId}");
                }
                else
                {
                    _logger.LogWarning($"Task not found for deletion: {taskId}");
                }
                return deleted;
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw;
            }
        }

        /// <summary>
        /// Gets task statistics
        /// </summary>
        public Dictionary<string, int> GetTaskStatistics()
        {
            var allTasks = _repository.GetAll();
            return new Dictionary<string, int>
            {
                { "Total", allTasks.Count },
                { "ToDo", allTasks.Count(t => t.Status == TaskStatus.ToDo) },
                { "InProgress", allTasks.Count(t => t.Status == TaskStatus.InProgress) },
                { "Done", allTasks.Count(t => t.Status == TaskStatus.Done) },
                { "Overdue", allTasks.Count(t => t.IsOverdue()) },
                { "High Priority", allTasks.Count(t => t.Priority == TaskPriority.High) }
            };
        }
    }
}
