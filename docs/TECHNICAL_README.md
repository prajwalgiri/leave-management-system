# Technical Documentation

This document provides a technical deep dive into the architecture, data models, and implementation details of the Leave Management System.

## Architecture

The application is built using **ASP.NET Core Razor Pages**, following a page-centric approach for UI development. It utilizes **Entity Framework Core (EF Core)** for data persistence and **ASP.NET Core Identity** for authentication.

### Key Components

*   **ApplicationDbContext**: Manages `ApplicationUser`, `Department`, and `LeaveModel`.
*   **ApplicationUser**: Custom identity user extending `IdentityUser` with `FullName`, `DepartmentId`, and `PhoneNumber`.
*   **DataSeeder**: Seeds roles (Admin, Supervisor, User) and initial departments.

## Database Schema

The system uses SQLite by default. The primary entities are:

### 1. ApplicationUser
Extends `IdentityUser`.
*   `FullName`: Display name.
*   `DepartmentId`: Foreign key to `Department`.

### 2. Department
Organizes employees and defines responsibility.
*   `Name`: Name of the department.
*   `DepartmentHeadId`: Link to an `ApplicationUser` acting as the supervisor.

### 3. LeaveModel
Stores leave request details.
*   `EmployeeId`: String ID mapping to the Identity User.
*   `StartDate` / `EndDate`: The duration of the leave.
*   `Reason`: Text description of the leave request.
*   `Status`: Enum (Requested, Approved, Rejected).
*   `ApprovedById`: Reference to the manager who approved the leave.

### 3. LeavePolicy
Defines the rules for leave allocations.
*   `AllowedLeaves`: Total number of days allowed.
*   `FromDate` / `ToDate`: The period for which the policy applies.

### 4. AttendenceHistoryModel
Records timestamps for attendance.
*   `EmployeeId`: Reference to the employee (Guid).
*   `Date`: Date of attendance (`DateOnly`).
*   `Time`: Time of recording (`TimeOnly`).

## Authentication & Authorization

The system integrates **ASP.NET Core Identity**:
*   **IdentityDbContext**: `ApplicationDbContext` inherits from `IdentityDbContext` to include standard User, Role, and Claim tables.
*   **Configuration**: Configured in `Program.cs` with `AddDefaultIdentity`.
*   **Security**: Password hashing and session management are handled by the framework.

## Configuration & Deployment

### Connection Strings
Managed in `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "DataSource=AttendenceDB.sqlite"
}
```

### Dependency Injection
Services like the database context and identity managers are registered in `Program.cs`:
*   `AddDbContext<ApplicationDbContext>`
*   `AddDefaultIdentity<IdentityUser>`
*   `AddRazorPages()`

### Database Migrations
Migrations are used to keep the database schema in sync with the models:
```bash
dotnet ef migrations add [MigrationName]
dotnet ef database update
```
Note: The application has `context.Database.EnsureCreated()` in `Program.cs` for automated setup on first run.

## Utility Scripts

The project includes scripts to interact directly with the database:
*   `AddUser.ps1`: Uses `sqlite3` CLI to insert a user record with a pre-calculated password hash.
*   `add_user.sh`: Linux/macOS equivalent of the user addition script.
