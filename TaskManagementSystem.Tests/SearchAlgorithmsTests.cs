using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskManagementSystem.Algorithms;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Tests
{
    /// <summary>
    /// Unit tests for search algorithms
    /// Tests linear and binary search implementations
    /// </summary>
    [TestClass]
    public class SearchAlgorithmsTests
    {
        private List<TaskItem> _testTasks;

        [TestInitialize]
        public void Setup()
        {
            _testTasks = new List<TaskItem>
            {
                new TaskItem("TASK-001", "Implement Login", "Create login page", DateTime.Now.AddDays(5), TaskPriority.High, "Alice"),
                new TaskItem("TASK-002", "Design Database", "Design schema", DateTime.Now.AddDays(10), TaskPriority.Medium, "Bob"),
                new TaskItem("TASK-003", "Write Tests", "Unit tests", DateTime.Now.AddDays(3), TaskPriority.High, "Alice"),
                new TaskItem("TASK-004", "Deploy Application", "Production deployment", DateTime.Now.AddDays(15), TaskPriority.Low, "Charlie"),
                new TaskItem("TASK-005", "Code Review", "Review pull requests", DateTime.Now.AddDays(2), TaskPriority.Medium, "Bob")
            };
        }

        [TestMethod]
        public void LinearSearchByTitle_ShouldFindMatchingTasks()
        {
            // Act
            var results = SearchAlgorithms.LinearSearchByTitle(_testTasks, "Design");

            // Assert
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Design Database", results[0].Title);
        }

        [TestMethod]
        public void LinearSearchByTitle_ShouldBeCaseInsensitive()
        {
            // Act
            var results = SearchAlgorithms.LinearSearchByTitle(_testTasks, "design");

            // Assert
            Assert.AreEqual(1, results.Count);
        }

        [TestMethod]
        public void LinearSearchByAssignee_ShouldFindAllTasksForAssignee()
        {
            // Act
            var results = SearchAlgorithms.LinearSearchByAssignee(_testTasks, "Alice");

            // Assert
            Assert.AreEqual(2, results.Count);
            Assert.IsTrue(results.TrueForAll(t => t.Assignee == "Alice"));
        }

        [TestMethod]
        public void BinarySearchById_ShouldFindExactTask()
        {
            // Act
            var result = SearchAlgorithms.BinarySearchById(_testTasks, "TASK-003");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("TASK-003", result.TaskId);
            Assert.AreEqual("Write Tests", result.Title);
        }

        [TestMethod]
        public void BinarySearchById_ShouldReturnNull_WhenTaskNotFound()
        {
            // Act
            var result = SearchAlgorithms.BinarySearchById(_testTasks, "TASK-999");

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void SearchByStatus_ShouldFindAllTasksWithStatus()
        {
            // Arrange
            _testTasks[0].UpdateStatus(TaskStatus.InProgress);
            _testTasks[2].UpdateStatus(TaskStatus.InProgress);

            // Act
            var results = SearchAlgorithms.SearchByStatus(_testTasks, TaskStatus.InProgress);

            // Assert
            Assert.AreEqual(2, results.Count);
            Assert.IsTrue(results.TrueForAll(t => t.Status == TaskStatus.InProgress));
        }

        [TestMethod]
        public void SearchByPriority_ShouldFindAllHighPriorityTasks()
        {
            // Act
            var results = SearchAlgorithms.SearchByPriority(_testTasks, TaskPriority.High);

            // Assert
            Assert.AreEqual(2, results.Count);
            Assert.IsTrue(results.TrueForAll(t => t.Priority == TaskPriority.High));
        }
    }
}
