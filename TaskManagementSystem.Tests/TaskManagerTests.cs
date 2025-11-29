using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskManagementSystem.Interfaces;
using TaskManagementSystem.Models;
using TaskManagementSystem.Services;

namespace TaskManagementSystem.Tests
{
    /// <summary>
    /// Unit tests for TaskManager business logic
    /// Tests task operations and report generation
    /// </summary>
    [TestClass]
    public class TaskManagerTests
    {
        private TaskManager _taskManager;
        private ILogger _logger;
        private ITaskRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            _logger = FileLogger.Instance;
            _repository = new TaskRepository(_logger);
            _taskManager = new TaskManager(_repository, _logger);
        }

        [TestMethod]
        public void CreateTask_ShouldAddTaskToRepository()
        {
            // Arrange
            string title = "Test Task";
            string description = "Test Description";
            DateTime dueDate = DateTime.Now.AddDays(7);
            TaskPriority priority = TaskPriority.High;
            string assignee = "Test User";

            // Act
            var task = _taskManager.CreateTask(title, description, dueDate, priority, assignee);

            // Assert
            Assert.IsNotNull(task);
            Assert.IsNotNull(task.TaskId);
            Assert.AreEqual(title, task.Title);
            Assert.AreEqual(description, task.Description);

            // Verify task was added to repository
            var retrievedTask = _repository.GetById(task.TaskId);
            Assert.IsNotNull(retrievedTask);
            Assert.AreEqual(task.TaskId, retrievedTask.TaskId);
        }

        [TestMethod]
        public void UpdateTaskStatus_ShouldChangeTaskStatus()
        {
            // Arrange
            var task = _taskManager.CreateTask("Update Test", "Test", DateTime.Now.AddDays(1), TaskPriority.Medium, "User");
            
            // Act
            _taskManager.UpdateTaskStatus(task.TaskId, TaskStatus.InProgress);

            // Assert
            var updatedTask = _repository.GetById(task.TaskId);
            Assert.AreEqual(TaskStatus.InProgress, updatedTask.Status);
        }

        [TestMethod]
        public void GetOverdueTasks_ShouldReturnOnlyOverdueTasks()
        {
            // Arrange - Clean up any existing tasks first
            var allTasks = _repository.GetAll();
            foreach (var t in allTasks)
            {
                _repository.Delete(t.TaskId);
            }

            // Create test tasks
            var overdueTask = _taskManager.CreateTask("Overdue Task", "", DateTime.Now.AddDays(-5), TaskPriority.High, "User");
            var futureTask = _taskManager.CreateTask("Future Task", "", DateTime.Now.AddDays(5), TaskPriority.Low, "User");

            // Act
            var overdueTasks = _taskManager.GetOverdueTasks();

            // Assert
            Assert.IsTrue(overdueTasks.Count >= 1);
            Assert.IsTrue(overdueTasks.Exists(t => t.TaskId == overdueTask.TaskId));
            Assert.IsFalse(overdueTasks.Exists(t => t.TaskId == futureTask.TaskId));
        }

        [TestMethod]
        public void GetUpcomingTasks_ShouldReturnTasksDueWithinSevenDays()
        {
            // Arrange - Clean up
            var allTasks = _repository.GetAll();
            foreach (var t in allTasks)
            {
                _repository.Delete(t.TaskId);
            }

            var upcomingTask = _taskManager.CreateTask("Upcoming Task", "", DateTime.Now.AddDays(3), TaskPriority.Medium, "User");
            var farFutureTask = _taskManager.CreateTask("Far Future Task", "", DateTime.Now.AddDays(30), TaskPriority.Low, "User");

            // Act
            var upcomingTasks = _taskManager.GetUpcomingTasks(7);

            // Assert
            Assert.IsTrue(upcomingTasks.Count >= 1);
            Assert.IsTrue(upcomingTasks.Exists(t => t.TaskId == upcomingTask.TaskId));
            Assert.IsFalse(upcomingTasks.Exists(t => t.TaskId == farFutureTask.TaskId));
        }

        [TestMethod]
        public void DeleteTask_ShouldRemoveTaskFromRepository()
        {
            // Arrange
            var task = _taskManager.CreateTask("Task to Delete", "", DateTime.Now.AddDays(1), TaskPriority.Low, "User");
            string taskId = task.TaskId;

            // Act
            bool deleted = _taskManager.DeleteTask(taskId);

            // Assert
            Assert.IsTrue(deleted);
            var retrievedTask = _repository.GetById(taskId);
            Assert.IsNull(retrievedTask);
        }

        [TestMethod]
        public void GetTaskStatistics_ShouldReturnCorrectCounts()
        {
            // Arrange - Clean up
            var allTasks = _repository.GetAll();
            foreach (var t in allTasks)
            {
                _repository.Delete(t.TaskId);
            }

            _taskManager.CreateTask("Task 1", "", DateTime.Now.AddDays(1), TaskPriority.High, "User");
            var task2 = _taskManager.CreateTask("Task 2", "", DateTime.Now.AddDays(2), TaskPriority.Low, "User");
            _taskManager.UpdateTaskStatus(task2.TaskId, TaskStatus.Done);

            // Act
            var stats = _taskManager.GetTaskStatistics();

            // Assert
            Assert.AreEqual(2, stats["Total"]);
            Assert.AreEqual(1, stats["Done"]);
            Assert.AreEqual(1, stats["High Priority"]);
        }
    }
}
