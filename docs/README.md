# Leave Management System

A comprehensive web-based application built with .NET 8 for managing employee attendance and leave requests. This system provides a streamlined workflow for employees to request leaves and for managers to approve them, while keeping track of attendance history.

## Features

### 👤 User Authentication & Identity
*   **Secure Login/Logout**: Built using ASP.NET Core Identity with a custom `ApplicationUser`.
*   **Role-based Access**: Support for **Admin**, **Supervisor**, and **User** roles.
*   **Departmental Context**: Users are assigned to departments for localized management.

### 📅 Leave Management
*   **Request Leave**: Employees can submit leave requests.
*   **Approval Workflow**: Supervisors can review and approve/reject requests from their own department. Admins can manage all requests.
*   **Leave Policies**: Define rules for allowed leaves.

### 📊 Dashboards & Reporting
*   **Admin Dashboard**: Overview of organizational metrics and department stats.
*   **Department Dashboard**: View leaves for all members of your department.
*   **Weekly Leaves**: Calendar-style view for staff availability by week.
*   **Today's Leaves**: Real-time view of who is currently out.

### ⏱️ Attendance Tracking
*   **Logging**: Automatically or manually record employee attendance.
*   **History**: Maintain a detailed log of attendance (date and time) for each employee.

### 👥 Employee Management
*   **Directory**: View a list of all employees in the system.
*   **Onboarding**: Add new employees to the platform.

## Technology Stack

*   **Framework**: .NET 8.0 (ASP.NET Core Razor Pages)
*   **Database**: SQLite (Default)
*   **ORM**: Entity Framework Core
*   **Security**: ASP.NET Core Identity
*   **Frontend**: Razor Pages, HTML/CSS, JavaScript

## Quick Start

1.  **Clone the repository**:
    ```bash
    git clone [repository-url]
    cd leave-management-system
    ```

2.  **Restore dependencies**:
    ```bash
    dotnet restore
    ```

3.  **Run the application**:
    ```bash
    dotnet run --project AttendenceSystem
    ```

4.  **Access the app**:
    Navigate to `https://localhost:5001` or the port shown in the console.
    *   **Default Admin**: `admin@company.com` / `Admin123!`

## Administrative Tools

*   **Add User Script**: Use `AttendenceSystem/AddUser.ps1` (PowerShell) or `AttendenceSystem/add_user.sh` (Shell) to manually inject users into the database.
