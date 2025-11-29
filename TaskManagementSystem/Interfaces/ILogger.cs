using System;

namespace TaskManagementSystem.Interfaces
{
    /// <summary>
    /// Interface for logging operations
    /// Abstracts logging implementation for flexibility and testability
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Logs an informational message
        /// </summary>
        void LogInfo(string message);

        /// <summary>
        /// Logs a warning message
        /// </summary>
        void LogWarning(string message);

        /// <summary>
        /// Logs an error message
        /// </summary>
        void LogError(string message);

        /// <summary>
        /// Logs an exception with details
        /// </summary>
        void LogException(Exception ex);
    }
}
