using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskManagementSystem.Algorithms;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Tests
{
    /// <summary>
    /// Unit tests for sorting algorithms
    /// Tests QuickSort and MergeSort implementations
    /// </summary>
    [TestClass]
    public class SortingAlgorithmsTests
    {
        [TestMethod]
        public void QuickSortByPriority_ShouldSortTasksInDescendingOrder()
        {
            // Arrange
            var tasks = new List<TaskItem>
            {
                new TaskItem("TASK-001", "Low Priority Task", "", DateTime.Now, TaskPriority.Low, "Alice"),
                new TaskItem("TASK-002", "High Priority Task", "", DateTime.Now, TaskPriority.High, "Bob"),
                new TaskItem("TASK-003", "Medium Priority Task", "", DateTime.Now, TaskPriority.Medium, "Charlie"),
                new TaskItem("TASK-004", "Another High Priority", "", DateTime.Now, TaskPriority.High, "Dave")
            };

            // Act
            SortingAlgorithms.QuickSortByPriority(tasks);

            // Assert
            Assert.AreEqual(TaskPriority.High, tasks[0].Priority);
            Assert.AreEqual(TaskPriority.High, tasks[1].Priority);
            Assert.AreEqual(TaskPriority.Medium, tasks[2].Priority);
            Assert.AreEqual(TaskPriority.Low, tasks[3].Priority);
        }

        [TestMethod]
        public void MergeSortByDueDate_ShouldSortTasksInAscendingOrder()
        {
            // Arrange
            DateTime baseDate = DateTime.Now;
            var tasks = new List<TaskItem>
            {
                new TaskItem("TASK-001", "Task Due in 10 days", "", baseDate.AddDays(10), TaskPriority.Medium, "Alice"),
                new TaskItem("TASK-002", "Task Due in 2 days", "", baseDate.AddDays(2), TaskPriority.High, "Bob"),
                new TaskItem("TASK-003", "Task Due in 5 days", "", baseDate.AddDays(5), TaskPriority.Low, "Charlie"),
                new TaskItem("TASK-004", "Task Due in 1 day", "", baseDate.AddDays(1), TaskPriority.Medium, "Dave")
            };

            // Act
            SortingAlgorithms.MergeSortByDueDate(tasks);

            // Assert
            Assert.AreEqual("Task Due in 1 day", tasks[0].Title);
            Assert.AreEqual("Task Due in 2 days", tasks[1].Title);
            Assert.AreEqual("Task Due in 5 days", tasks[2].Title);
            Assert.AreEqual("Task Due in 10 days", tasks[3].Title);
        }

        [TestMethod]
        public void QuickSortByPriority_ShouldHandleEmptyList()
        {
            // Arrange
            var tasks = new List<TaskItem>();

            // Act & Assert (should not throw exception)
            SortingAlgorithms.QuickSortByPriority(tasks);
            Assert.AreEqual(0, tasks.Count);
        }

        [TestMethod]
        public void MergeSortByDueDate_ShouldHandleSingleElement()
        {
            // Arrange
            var tasks = new List<TaskItem>
            {
                new TaskItem("TASK-001", "Single Task", "", DateTime.Now, TaskPriority.High, "Alice")
            };

            // Act
            SortingAlgorithms.MergeSortByDueDate(tasks);

            // Assert
            Assert.AreEqual(1, tasks.Count);
            Assert.AreEqual("Single Task", tasks[0].Title);
        }

        [TestMethod]
        public void BuiltInSortByPriority_ShouldMatchQuickSortResults()
        {
            // Arrange
            var tasks1 = new List<TaskItem>
            {
                new TaskItem("TASK-001", "Low", "", DateTime.Now, TaskPriority.Low, "A"),
                new TaskItem("TASK-002", "High", "", DateTime.Now, TaskPriority.High, "B"),
                new TaskItem("TASK-003", "Medium", "", DateTime.Now, TaskPriority.Medium, "C")
            };
            var tasks2 = new List<TaskItem>(tasks1);

            // Act
            SortingAlgorithms.QuickSortByPriority(tasks1);
            SortingAlgorithms.BuiltInSortByPriority(tasks2);

            // Assert
            for (int i = 0; i < tasks1.Count; i++)
            {
                Assert.AreEqual(tasks1[i].Priority, tasks2[i].Priority);
            }
        }

        [TestMethod]
        public void BuiltInSortByDueDate_ShouldMatchMergeSortResults()
        {
            // Arrange
            DateTime baseDate = DateTime.Now;
            var tasks1 = new List<TaskItem>
            {
                new TaskItem("TASK-001", "T1", "", baseDate.AddDays(10), TaskPriority.Low, "A"),
                new TaskItem("TASK-002", "T2", "", baseDate.AddDays(2), TaskPriority.High, "B"),
                new TaskItem("TASK-003", "T3", "", baseDate.AddDays(5), TaskPriority.Medium, "C")
            };
            var tasks2 = new List<TaskItem>(tasks1);

            // Act
            SortingAlgorithms.MergeSortByDueDate(tasks1);
            SortingAlgorithms.BuiltInSortByDueDate(tasks2);

            // Assert
            for (int i = 0; i < tasks1.Count; i++)
            {
                Assert.AreEqual(tasks1[i].DueDate, tasks2[i].DueDate);
            }
        }
    }
}
