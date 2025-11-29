using System;
using System.Collections.Generic;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Algorithms
{
    /// <summary>
    /// Implements sorting algorithms for task collections
    /// Manual implementations of QuickSort and MergeSort as required
    /// </summary>
    public static class SortingAlgorithms
    {
        #region QuickSort for Priority

        /// <summary>
        /// QuickSort implementation for sorting tasks by priority (High to Low)
        /// Time Complexity: O(n log n) average, O(n²) worst case
        /// Space Complexity: O(log n) due to recursion
        /// </summary>
        public static void QuickSortByPriority(List<TaskItem> tasks)
        {
            if (tasks == null)
                throw new ArgumentNullException(nameof(tasks));

            if (tasks.Count <= 1)
                return;

            QuickSortByPriorityRecursive(tasks, 0, tasks.Count - 1);
        }

        private static void QuickSortByPriorityRecursive(List<TaskItem> tasks, int low, int high)
        {
            if (low < high)
            {
                int pivotIndex = PartitionByPriority(tasks, low, high);
                QuickSortByPriorityRecursive(tasks, low, pivotIndex - 1);
                QuickSortByPriorityRecursive(tasks, pivotIndex + 1, high);
            }
        }

        private static int PartitionByPriority(List<TaskItem> tasks, int low, int high)
        {
            TaskPriority pivot = tasks[high].Priority;
            int i = low - 1;

            for (int j = low; j < high; j++)
            {
                // Sort in descending order (High priority first)
                if (tasks[j].Priority >= pivot)
                {
                    i++;
                    Swap(tasks, i, j);
                }
            }

            Swap(tasks, i + 1, high);
            return i + 1;
        }

        #endregion

        #region MergeSort for Due Date

        /// <summary>
        /// MergeSort implementation for sorting tasks by due date (earliest first)
        /// Time Complexity: O(n log n) guaranteed
        /// Space Complexity: O(n) for temporary arrays
        /// Stable sort - maintains relative order of equal elements
        /// </summary>
        public static void MergeSortByDueDate(List<TaskItem> tasks)
        {
            if (tasks == null)
                throw new ArgumentNullException(nameof(tasks));

            if (tasks.Count <= 1)
                return;

            MergeSortByDueDateRecursive(tasks, 0, tasks.Count - 1);
        }

        private static void MergeSortByDueDateRecursive(List<TaskItem> tasks, int left, int right)
        {
            if (left < right)
            {
                int mid = left + (right - left) / 2;

                MergeSortByDueDateRecursive(tasks, left, mid);
                MergeSortByDueDateRecursive(tasks, mid + 1, right);
                MergeByDueDate(tasks, left, mid, right);
            }
        }

        private static void MergeByDueDate(List<TaskItem> tasks, int left, int mid, int right)
        {
            int leftSize = mid - left + 1;
            int rightSize = right - mid;

            // Create temporary arrays
            TaskItem[] leftArray = new TaskItem[leftSize];
            TaskItem[] rightArray = new TaskItem[rightSize];

            // Copy data to temporary arrays
            for (int i = 0; i < leftSize; i++)
                leftArray[i] = tasks[left + i];
            for (int j = 0; j < rightSize; j++)
                rightArray[j] = tasks[mid + 1 + j];

            // Merge the temporary arrays back
            int iLeft = 0, iRight = 0, k = left;

            while (iLeft < leftSize && iRight < rightSize)
            {
                if (leftArray[iLeft].DueDate <= rightArray[iRight].DueDate)
                {
                    tasks[k] = leftArray[iLeft];
                    iLeft++;
                }
                else
                {
                    tasks[k] = rightArray[iRight];
                    iRight++;
                }
                k++;
            }

            // Copy remaining elements
            while (iLeft < leftSize)
            {
                tasks[k] = leftArray[iLeft];
                iLeft++;
                k++;
            }

            while (iRight < rightSize)
            {
                tasks[k] = rightArray[iRight];
                iRight++;
                k++;
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Swaps two elements in the list
        /// </summary>
        private static void Swap(List<TaskItem> tasks, int i, int j)
        {
            TaskItem temp = tasks[i];
            tasks[i] = tasks[j];
            tasks[j] = temp;
        }

        /// <summary>
        /// Sorts tasks by priority using built-in sort for comparison
        /// Used to validate manual implementation
        /// </summary>
        public static void BuiltInSortByPriority(List<TaskItem> tasks)
        {
            if (tasks == null)
                throw new ArgumentNullException(nameof(tasks));

            tasks.Sort((a, b) => b.Priority.CompareTo(a.Priority)); // Descending
        }

        /// <summary>
        /// Sorts tasks by due date using built-in sort for comparison
        /// Used to validate manual implementation
        /// </summary>
        public static void BuiltInSortByDueDate(List<TaskItem> tasks)
        {
            if (tasks == null)
                throw new ArgumentNullException(nameof(tasks));

            tasks.Sort((a, b) => a.DueDate.CompareTo(b.DueDate)); // Ascending
        }

        #endregion
    }
}
