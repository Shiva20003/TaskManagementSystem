# Task & Project Tracking System

## Overview
A comprehensive console-based Task & Project Tracking System built in C# (.NET Framework 4.7.2) that demonstrates professional software engineering practices including OOP design, design patterns, algorithms, data persistence, and unit testing.

## Features
- ✅ **Task Management**: Create, update, search, sort, and delete tasks
- ✅ **Task Properties**: Unique IDs, due dates, priorities (Low/Medium/High), status (ToDo/InProgress/Done), assignees
- ✅ **Search Algorithms**: Linear search (by title/assignee) and Binary search (by ID)
- ✅ **Sorting Algorithms**: QuickSort (by priority) and MergeSort (by due date)
- ✅ **Reports**: Overdue tasks and upcoming tasks reports
- ✅ **Data Persistence**: JSON file storage for tasks
- ✅ **Logging**: Text file logging with timestamps
- ✅ **Error Handling**: Comprehensive exception handling throughout

## Design Patterns Implemented
1. **Singleton Pattern** - FileLogger ensures single instance for centralized logging
2. **Repository Pattern** - TaskRepository separates data access from business logic
3. **Factory Pattern** - TaskFactory centralizes task creation with unique ID generation

## Project Structure
```
TaskManagementSystem/
├── Models/
│   ├── TaskItem.cs          # Task domain model
│   ├── TaskPriority.cs      # Priority enumeration
│   └── TaskStatus.cs        # Status enumeration
├── Interfaces/
│   ├── ITaskRepository.cs   # Repository contract
│   └── ILogger.cs           # Logger contract
├── Services/
│   ├── TaskRepository.cs    # JSON persistence implementation
│   ├── FileLogger.cs        # Singleton logger implementation
│   ├── TaskFactory.cs       # Factory for task creation
│   └── TaskManager.cs       # Business logic orchestration
├── Algorithms/
│   ├── SearchAlgorithms.cs  # Linear & Binary search
│   └── SortingAlgorithms.cs # QuickSort & MergeSort
├── UI/
│   └── ConsoleUI.cs         # Menu-driven interface
├── Utilities/
│   └── InputValidator.cs    # Input validation helpers
└── Program.cs               # Minimal entry point

TaskManagementSystem.Tests/
├── TaskItemTests.cs         # 6 unit tests
├── SearchAlgorithmsTests.cs # 7 unit tests
├── SortingAlgorithmsTests.cs# 6 unit tests
├── TaskManagerTests.cs      # 7 unit tests
└── TaskRepositoryTests.cs   # 6 unit tests
Total: 32 comprehensive unit tests
```

## Building the Project

### Option 1: Visual Studio
1. Open `TaskManagementSystem.slnx` in Visual Studio
2. Right-click on solution → **Restore NuGet Packages**
3. Build → **Build Solution** (Ctrl+Shift+B)
4. Set `TaskManagementSystem` as startup project
5. Press F5 to run

### Option 2: Command Line (Visual Studio Developer Command Prompt)
```bash
# Open Developer Command Prompt for VS
# Navigate to project directory
cd "d:\WORKS\Pj 822 Shiva\TaskManagementSystem\TaskManagementSystem"

# Restore packages and build
msbuild TaskManagementSystem\TaskManagementSystem.csproj /t:Restore,Build /p:Configuration=Debug

# Run the application
TaskManagementSystem\bin\Debug\TaskManagementSystem.exe
```

## Running Unit Tests
1. In Visual Studio: Test → Run All Tests
2. View results in Test Explorer
3. All 32 tests should pass

## Dependencies
- .NET Framework 4.7.2
- Newtonsoft.Json 13.0.3 (for JSON serialization)
- MSTest Framework 2.2.10 (for unit testing)

## Data Files
- `tasks.json` - Stores all tasks (created automatically)
- `system.log` - Application log file (created automatically)

## Usage
1. Run the application
2. Use the menu to:
   - Add new tasks with details
   - Update task status
   - Search tasks (by title, assignee, or ID)
   - Sort tasks (by priority or due date)
   - View all tasks
   - Generate reports (overdue/upcoming)
   - Delete tasks
   - View statistics

## Algorithm Complexity
- **Linear Search**: O(n) - Used for text-based searches
- **Binary Search**: O(log n) - Used for ID lookup
- **QuickSort**: O(n log n) average - Used for priority sorting
- **MergeSort**: O(n log n) guaranteed - Used for date sorting

## OOP Principles Demonstrated
- **Encapsulation**: Properties with validation, private fields
- **Inheritance**: Not explicitly used but architecture supports it
- **Polymorphism**: Interface-based programming (ILogger, ITaskRepository)
- **Abstraction**: Interfaces abstract implementation details

## Error Handling
- Input validation for all user inputs
- Exception handling for file I/O operations
- Graceful handling of malformed JSON data
- Defensive programming throughout

## Author
Software Engineering Assignment - Scenario 3: Task & Project Tracker

## License
Educational Project
