using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Tests
{
    /// <summary>
    /// Unit tests for TaskItem class
    /// Tests task creation, validation, and business logic
    /// </summary>
    [TestClass]
    public class TaskItemTests
    {
        [TestMethod]
        public void TaskItem_Constructor_ShouldCreateValidTask()
        {
            // Arrange
            string taskId = "TASK-001";
            string title = "Complete Unit Tests";
            string description = "Write comprehensive unit tests";
            DateTime dueDate = DateTime.Now.AddDays(7);
            TaskPriority priority = TaskPriority.High;
            string assignee = "John Doe";

            // Act
            var task = new TaskItem(taskId, title, description, dueDate, priority, assignee);

            // Assert
            Assert.AreEqual(taskId, task.TaskId);
            Assert.AreEqual(title, task.Title);
            Assert.AreEqual(description, task.Description);
            Assert.AreEqual(dueDate, task.DueDate);
            Assert.AreEqual(priority, task.Priority);
            Assert.AreEqual(assignee, task.Assignee);
            Assert.AreEqual(TaskStatus.ToDo, task.Status);
            Assert.IsNull(task.CompletedDate);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TaskItem_Constructor_ShouldThrowException_WhenTitleIsEmpty()
        {
            // Arrange & Act & Assert
            var task = new TaskItem("TASK-001", "", "Description", DateTime.Now.AddDays(1), TaskPriority.Medium, "Assignee");
        }

        [TestMethod]
        public void TaskItem_IsOverdue_ShouldReturnTrue_WhenTaskIsPastDueDate()
        {
            // Arrange
            var task = new TaskItem("TASK-002", "Overdue Task", "Test", DateTime.Now.AddDays(-2), TaskPriority.High, "Jane");

            // Act
            bool isOverdue = task.IsOverdue();

            // Assert
            Assert.IsTrue(isOverdue);
        }

        [TestMethod]
        public void TaskItem_IsOverdue_ShouldReturnFalse_WhenTaskIsCompleted()
        {
            // Arrange
            var task = new TaskItem("TASK-003", "Completed Task", "Test", DateTime.Now.AddDays(-1), TaskPriority.Low, "Bob");
            task.UpdateStatus(TaskStatus.Done);

            // Act
            bool isOverdue = task.IsOverdue();

            // Assert
            Assert.IsFalse(isOverdue);
        }

        [TestMethod]
        public void TaskItem_IsUpcoming_ShouldReturnTrue_WhenTaskIsDueWithinSevenDays()
        {
            // Arrange
            var task = new TaskItem("TASK-004", "Upcoming Task", "Test", DateTime.Now.AddDays(5), TaskPriority.Medium, "Alice");

            // Act
            bool isUpcoming = task.IsUpcoming(7);

            // Assert
            Assert.IsTrue(isUpcoming);
        }

        [TestMethod]
        public void TaskItem_UpdateStatus_ShouldSetCompletedDate_WhenStatusIsDone()
        {
            // Arrange
            var task = new TaskItem("TASK-005", "Task to Complete", "Test", DateTime.Now.AddDays(1), TaskPriority.High, "Charlie");

            // Act
            task.UpdateStatus(TaskStatus.Done);

            // Assert
            Assert.AreEqual(TaskStatus.Done, task.Status);
            Assert.IsNotNull(task.CompletedDate);
            Assert.IsTrue(task.CompletedDate.Value <= DateTime.Now);
        }
    }
}
