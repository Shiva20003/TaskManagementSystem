using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TaskManagementSystem.Interfaces;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Services
{
    /// <summary>
    /// Repository implementation for task data persistence
    /// Implements Repository pattern for separation of concerns
    /// Handles JSON serialization/deserialization with error handling
    /// </summary>
    public class TaskRepository : ITaskRepository
    {
        private readonly string _dataFilePath;
        private readonly ILogger _logger;
        private List<TaskItem> _tasks;

        public TaskRepository(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dataFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tasks.json");
            _tasks = new List<TaskItem>();
            LoadData();
        }

        public void Add(TaskItem task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            if (_tasks.Any(t => t.TaskId == task.TaskId))
                throw new InvalidOperationException($"Task with ID '{task.TaskId}' already exists.");

            _tasks.Add(task);
            SaveChanges();
            _logger.LogInfo($"Task added: {task.TaskId} - {task.Title}");
        }

        public void Update(TaskItem task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            var existingTask = _tasks.FirstOrDefault(t => t.TaskId == task.TaskId);
            if (existingTask == null)
                throw new InvalidOperationException($"Task with ID '{task.TaskId}' not found.");

            // Update the existing task
            int index = _tasks.IndexOf(existingTask);
            _tasks[index] = task;
            SaveChanges();
            _logger.LogInfo($"Task updated: {task.TaskId} - {task.Title}");
        }

        public bool Delete(string taskId)
        {
            if (string.IsNullOrWhiteSpace(taskId))
                throw new ArgumentException("Task ID cannot be empty", nameof(taskId));

            var task = _tasks.FirstOrDefault(t => t.TaskId == taskId);
            if (task != null)
            {
                _tasks.Remove(task);
                SaveChanges();
                _logger.LogInfo($"Task deleted: {taskId}");
                return true;
            }
            return false;
        }

        public TaskItem GetById(string taskId)
        {
            if (string.IsNullOrWhiteSpace(taskId))
                throw new ArgumentException("Task ID cannot be empty", nameof(taskId));

            return _tasks.FirstOrDefault(t => t.TaskId == taskId);
        }

        public List<TaskItem> GetAll()
        {
            return new List<TaskItem>(_tasks); // Return a copy to prevent external modification
        }

        public void SaveChanges()
        {
            try
            {
                string json = JsonConvert.SerializeObject(_tasks, Formatting.Indented);
                File.WriteAllText(_dataFilePath, json);
                _logger.LogInfo($"Data saved successfully. Total tasks: {_tasks.Count}");
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw new IOException("Failed to save tasks to file.", ex);
            }
        }

        public void LoadData()
        {
            try
            {
                if (File.Exists(_dataFilePath))
                {
                    string json = File.ReadAllText(_dataFilePath);
                    
                    if (string.IsNullOrWhiteSpace(json))
                    {
                        _tasks = new List<TaskItem>();
                        _logger.LogWarning("Data file is empty. Starting with empty task list.");
                        return;
                    }

                    _tasks = JsonConvert.DeserializeObject<List<TaskItem>>(json) ?? new List<TaskItem>();
                    _logger.LogInfo($"Data loaded successfully. Total tasks: {_tasks.Count}");
                }
                else
                {
                    _tasks = new List<TaskItem>();
                    _logger.LogInfo("Data file not found. Starting with empty task list.");
                }
            }
            catch (JsonException ex)
            {
                _logger.LogException(ex);
                _logger.LogWarning("Failed to parse JSON data. Starting with empty task list.");
                _tasks = new List<TaskItem>();
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw new IOException("Failed to load tasks from file.", ex);
            }
        }
    }
}
