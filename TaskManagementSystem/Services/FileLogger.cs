using System;
using System.IO;
using TaskManagementSystem.Interfaces;

namespace TaskManagementSystem.Services
{
    /// <summary>
    /// File-based logger implementation using Singleton pattern
    /// Ensures only one instance exists throughout the application lifecycle
    /// Thread-safe implementation for concurrent access
    /// </summary>
    public sealed class FileLogger : ILogger
    {
        private static readonly object _lock = new object();
        private static FileLogger _instance;
        private readonly string _logFilePath;

        /// <summary>
        /// Private constructor to prevent external instantiation
        /// </summary>
        private FileLogger()
        {
            _logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "system.log");
        }

        /// <summary>
        /// Gets the singleton instance of FileLogger
        /// Thread-safe lazy initialization
        /// </summary>
        public static FileLogger Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new FileLogger();
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Writes a log entry to the file with timestamp
        /// </summary>
        private void WriteLog(string level, string message)
        {
            lock (_lock)
            {
                try
                {
                    string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}";
                    File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
                }
                catch (Exception ex)
                {
                    // Fallback to console if file writing fails
                    Console.WriteLine($"Failed to write to log file: {ex.Message}");
                }
            }
        }

        public void LogInfo(string message)
        {
            WriteLog("INFO", message);
        }

        public void LogWarning(string message)
        {
            WriteLog("WARNING", message);
        }

        public void LogError(string message)
        {
            WriteLog("ERROR", message);
        }

        public void LogException(Exception ex)
        {
            WriteLog("EXCEPTION", $"{ex.GetType().Name}: {ex.Message}\nStack Trace: {ex.StackTrace}");
        }
    }
}
