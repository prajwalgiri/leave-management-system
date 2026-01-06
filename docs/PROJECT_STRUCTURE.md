# Project Structure

Overview of the directory structure and file organization for the Leave Management System.

## Root Directory

| File/Folder | Description |
| :--- | :--- |
| `docs/` | Documentation files (you are here). |
| `AttendenceSystem/` | The main ASP.NET Core web application project. |
| `AttendenceSystem.sln` | Visual Studio Solution file. |

## AttendenceSystem Project

### `/Areas`
Contains ASP.NET Core Areas.
*   `Identity/`: Default area for Identity management (Pages, Logic).

### `/Data`
Data layer of the application.
*   `Models/`: Domain entities.
    *   `Attendence/`: Models related to attendance tracking.
    *   `Leave/`: Models related to leave requests and policies.
*   `ApplicationDbContext.cs`: EF Core context definition.
*   `DataSeeder.cs`: Database initialization logic.

### `/Migrations`
Entity Framework Core migration files used for database schema versioning.

### `/Pages`
Razor Pages for the Web UI.
*   `Index.cshtml`: Main dashboard.
*   `Employees.cshtml`: Employee lookup.
*   `AddEmployee.cshtml`: Form to register new employees.
*   `RequestLeave.cshtml`: Form for submitting leave requests.
*   `ViewLeaves.cshtml`: Table of leave requests and status.
*   `ManageLeavePolicy.cshtml`: Admin view for leave rules.
*   `Shared/`: Layouts and partial views (e.g., `_Layout.cshtml`, `_LoginPartial.cshtml`).

### `/Properties`
Development-time settings like `launchSettings.json`.

### `/wwwroot`
Static assets served to the browser.
*   `css/`: Custom styles.
*   `js/`: Client-side scripting.
*   `lib/`: Third-party libraries (Bootstrap, jQuery, etc.).

### Root Files
*   `Program.cs`: Application entry point, service configuration, and middleware pipeline.
*   `appsettings.json`: Configuration settings (Connection strings, logging levels).
*   `AttendenceDB.sqlite`: SQLite database file (generated at runtime).
*   `AddUser.ps1` / `add_user.sh`: Scripted user management tools.
