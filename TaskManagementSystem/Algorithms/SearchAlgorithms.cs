using System;
using System.Collections.Generic;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Algorithms
{
    /// <summary>
    /// Implements search algorithms for task collections
    /// Demonstrates algorithm implementation as required
    /// </summary>
    public static class SearchAlgorithms
    {
        /// <summary>
        /// Linear search - searches for tasks by title (case-insensitive)
        /// Time Complexity: O(n)
        /// Best for unsorted data or small datasets
        /// </summary>
        public static List<TaskItem> LinearSearchByTitle(List<TaskItem> tasks, string searchTerm)
        {
            if (tasks == null)
                throw new ArgumentNullException(nameof(tasks));
            
            if (string.IsNullOrWhiteSpace(searchTerm))
                return new List<TaskItem>();

            List<TaskItem> results = new List<TaskItem>();
            searchTerm = searchTerm.ToLower();

            foreach (var task in tasks)
            {
                if (task.Title.ToLower().Contains(searchTerm))
                {
                    results.Add(task);
                }
            }

            return results;
        }

        /// <summary>
        /// Linear search - searches for tasks by assignee (case-insensitive)
        /// Time Complexity: O(n)
        /// </summary>
        public static List<TaskItem> LinearSearchByAssignee(List<TaskItem> tasks, string assignee)
        {
            if (tasks == null)
                throw new ArgumentNullException(nameof(tasks));
            
            if (string.IsNullOrWhiteSpace(assignee))
                return new List<TaskItem>();

            List<TaskItem> results = new List<TaskItem>();
            assignee = assignee.ToLower();

            foreach (var task in tasks)
            {
                if (task.Assignee.ToLower().Contains(assignee))
                {
                    results.Add(task);
                }
            }

            return results;
        }

        /// <summary>
        /// Binary search - searches for a task by exact Task ID
        /// Time Complexity: O(log n)
        /// Requires the list to be sorted by TaskId
        /// More efficient for large sorted datasets
        /// </summary>
        public static TaskItem BinarySearchById(List<TaskItem> tasks, string taskId)
        {
            if (tasks == null)
                throw new ArgumentNullException(nameof(tasks));
            
            if (string.IsNullOrWhiteSpace(taskId))
                return null;

            // First, sort the list by TaskId for binary search
            List<TaskItem> sortedTasks = new List<TaskItem>(tasks);
            sortedTasks.Sort((a, b) => string.Compare(a.TaskId, b.TaskId, StringComparison.Ordinal));

            int left = 0;
            int right = sortedTasks.Count - 1;

            while (left <= right)
            {
                int mid = left + (right - left) / 2;
                int comparison = string.Compare(sortedTasks[mid].TaskId, taskId, StringComparison.Ordinal);

                if (comparison == 0)
                {
                    return sortedTasks[mid]; // Found
                }
                else if (comparison < 0)
                {
                    left = mid + 1; // Search right half
                }
                else
                {
                    right = mid - 1; // Search left half
                }
            }

            return null; // Not found
        }

        /// <summary>
        /// Linear search by status
        /// </summary>
        public static List<TaskItem> SearchByStatus(List<TaskItem> tasks, TaskStatus status)
        {
            if (tasks == null)
                throw new ArgumentNullException(nameof(tasks));

            List<TaskItem> results = new List<TaskItem>();

            foreach (var task in tasks)
            {
                if (task.Status == status)
                {
                    results.Add(task);
                }
            }

            return results;
        }

        /// <summary>
        /// Linear search by priority
        /// </summary>
        public static List<TaskItem> SearchByPriority(List<TaskItem> tasks, TaskPriority priority)
        {
            if (tasks == null)
                throw new ArgumentNullException(nameof(tasks));

            List<TaskItem> results = new List<TaskItem>();

            foreach (var task in tasks)
            {
                if (task.Priority == priority)
                {
                    results.Add(task);
                }
            }

            return results;
        }
    }
}
