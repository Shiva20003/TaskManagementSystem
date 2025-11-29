using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskManagementSystem.Interfaces;
using TaskManagementSystem.Models;
using TaskManagementSystem.Services;

namespace TaskManagementSystem.Tests
{
    /// <summary>
    /// Unit tests for TaskRepository
    /// Tests JSON persistence and data operations
    /// </summary>
    [TestClass]
    public class TaskRepositoryTests
    {
        private ILogger _logger;
        private string _testDataPath;

        [TestInitialize]
        public void Setup()
        {
            _logger = FileLogger.Instance;
            _testDataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tasks.json");
            
            // Clean up test data file if it exists
            if (File.Exists(_testDataPath))
            {
                File.Delete(_testDataPath);
            }
        }

        [TestMethod]
        public void Add_ShouldPersistTaskToJsonFile()
        {
            // Arrange
            var repository = new TaskRepository(_logger);
            var task = new TaskItem("TEST-001", "Test Task", "Description", DateTime.Now.AddDays(1), TaskPriority.High, "Tester");

            // Act
            repository.Add(task);

            // Assert
            Assert.IsTrue(File.Exists(_testDataPath));
            var retrieved = repository.GetById("TEST-001");
            Assert.IsNotNull(retrieved);
            Assert.AreEqual("Test Task", retrieved.Title);
        }

        [TestMethod]
        public void LoadData_ShouldRestoreTasksFromJsonFile()
        {
            // Arrange
            var repository1 = new TaskRepository(_logger);
            var task = new TaskItem("TEST-002", "Persistent Task", "Test", DateTime.Now.AddDays(2), TaskPriority.Medium, "User");
            repository1.Add(task);

            // Act - Create new repository instance (should load from file)
            var repository2 = new TaskRepository(_logger);
            var loadedTask = repository2.GetById("TEST-002");

            // Assert
            Assert.IsNotNull(loadedTask);
            Assert.AreEqual("Persistent Task", loadedTask.Title);
            Assert.AreEqual(TaskPriority.Medium, loadedTask.Priority);
        }

        [TestMethod]
        public void Update_ShouldModifyExistingTask()
        {
            // Arrange
            var repository = new TaskRepository(_logger);
            var task = new TaskItem("TEST-003", "Original Title", "Desc", DateTime.Now.AddDays(1), TaskPriority.Low, "User");
            repository.Add(task);

            // Act
            task.Title = "Updated Title";
            task.UpdateStatus(TaskStatus.InProgress);
            repository.Update(task);

            // Assert
            var updated = repository.GetById("TEST-003");
            Assert.AreEqual("Updated Title", updated.Title);
            Assert.AreEqual(TaskStatus.InProgress, updated.Status);
        }

        [TestMethod]
        public void Delete_ShouldRemoveTaskAndPersistChanges()
        {
            // Arrange
            var repository = new TaskRepository(_logger);
            var task = new TaskItem("TEST-004", "Task to Delete", "Test", DateTime.Now, TaskPriority.High, "User");
            repository.Add(task);

            // Act
            bool deleted = repository.Delete("TEST-004");

            // Assert
            Assert.IsTrue(deleted);
            var retrieved = repository.GetById("TEST-004");
            Assert.IsNull(retrieved);

            // Verify persistence
            var repository2 = new TaskRepository(_logger);
            var stillDeleted = repository2.GetById("TEST-004");
            Assert.IsNull(stillDeleted);
        }

        [TestMethod]
        public void GetAll_ShouldReturnAllTasks()
        {
            // Arrange
            var repository = new TaskRepository(_logger);
            repository.Add(new TaskItem("TEST-005", "Task 1", "", DateTime.Now, TaskPriority.High, "User1"));
            repository.Add(new TaskItem("TEST-006", "Task 2", "", DateTime.Now, TaskPriority.Low, "User2"));

            // Act
            var allTasks = repository.GetAll();

            // Assert
            Assert.IsTrue(allTasks.Count >= 2);
            Assert.IsTrue(allTasks.Exists(t => t.TaskId == "TEST-005"));
            Assert.IsTrue(allTasks.Exists(t => t.TaskId == "TEST-006"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Add_ShouldThrowException_WhenDuplicateIdExists()
        {
            // Arrange
            var repository = new TaskRepository(_logger);
            var task1 = new TaskItem("TEST-007", "Task 1", "", DateTime.Now, TaskPriority.High, "User");
            var task2 = new TaskItem("TEST-007", "Task 2", "", DateTime.Now, TaskPriority.Low, "User");

            // Act
            repository.Add(task1);
            repository.Add(task2); // Should throw exception

            // Assert is handled by ExpectedException
        }
    }
}
