using System;

namespace TaskManagementSystem.Models
{
    /// <summary>
    /// Represents a task item in the tracking system
    /// Demonstrates encapsulation and OOP principles
    /// </summary>
    public class TaskItem
    {
        public string TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public TaskPriority Priority { get; set; }
        public TaskStatus Status { get; set; }
        public string Assignee { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? CompletedDate { get; set; }

        /// <summary>
        /// Default constructor for JSON deserialization
        /// </summary>
        public TaskItem()
        {
            CreatedDate = DateTime.Now;
            Status = TaskStatus.ToDo;
        }

        /// <summary>
        /// Constructor with parameters for creating new tasks
        /// </summary>
        public TaskItem(string taskId, string title, string description, DateTime dueDate, 
                       TaskPriority priority, string assignee)
        {
            if (string.IsNullOrWhiteSpace(taskId))
                throw new ArgumentException("Task ID cannot be empty", nameof(taskId));
            
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty", nameof(title));

            TaskId = taskId;
            Title = title;
            Description = description ?? string.Empty;
            DueDate = dueDate;
            Priority = priority;
            Assignee = assignee ?? "Unassigned";
            Status = TaskStatus.ToDo;
            CreatedDate = DateTime.Now;
            CompletedDate = null;
        }

        /// <summary>
        /// Checks if the task is overdue
        /// </summary>
        public bool IsOverdue()
        {
            return Status != TaskStatus.Done && DateTime.Now > DueDate;
        }

        /// <summary>
        /// Checks if the task is upcoming (due within specified days)
        /// </summary>
        public bool IsUpcoming(int days)
        {
            if (Status == TaskStatus.Done)
                return false;

            TimeSpan timeUntilDue = DueDate - DateTime.Now;
            return timeUntilDue.TotalDays >= 0 && timeUntilDue.TotalDays <= days;
        }

        /// <summary>
        /// Updates the task status and sets completion date if done
        /// </summary>
        public void UpdateStatus(TaskStatus newStatus)
        {
            Status = newStatus;
            if (newStatus == TaskStatus.Done && !CompletedDate.HasValue)
            {
                CompletedDate = DateTime.Now;
            }
        }

        /// <summary>
        /// Returns a formatted string representation of the task
        /// </summary>
        public override string ToString()
        {
            string overdueIndicator = IsOverdue() ? " [OVERDUE]" : "";
            return $"[{TaskId}] {Title} | Priority: {Priority} | Status: {Status} | " +
                   $"Due: {DueDate:yyyy-MM-dd} | Assignee: {Assignee}{overdueIndicator}";
        }
    }
}
