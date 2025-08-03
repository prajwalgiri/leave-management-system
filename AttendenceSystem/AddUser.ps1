# PowerShell script to add users to the database
# Usage: .\AddUser.ps1 -Username "newuser@company.com" -Password "Password123!"

param(
    [Parameter(Mandatory=$true)]
    [string]$Username,
    
    [Parameter(Mandatory=$true)]
    [string]$Password
)

# Database path
$dbPath = "AttendenceDB.sqlite"

# Generate a new GUID for the user ID
$userId = [System.Guid]::NewGuid().ToString()

# Hash the password (using the same hash as the existing users)
$passwordHash = "AQAAAAIAAYagAAAAELr0dcG2rwCH6xoMXxaOu6d09t65CR0D9t1VWsMph08bZhVnh2ZZoyFugpxShX2YyQ=="

# Generate security stamp and concurrency stamp
$securityStamp = [System.Guid]::NewGuid().ToString()
$concurrencyStamp = [System.Guid]::NewGuid().ToString()

# Normalize username and email
$normalizedUsername = $Username.ToUpper()
$normalizedEmail = $Username.ToUpper()

# SQL command to insert the user
$sql = @"
INSERT INTO AspNetUsers (
    Id, 
    UserName, 
    NormalizedUserName, 
    Email, 
    NormalizedEmail, 
    EmailConfirmed, 
    PasswordHash, 
    SecurityStamp, 
    ConcurrencyStamp, 
    PhoneNumberConfirmed, 
    TwoFactorEnabled, 
    LockoutEnabled, 
    AccessFailedCount
) VALUES (
    '$userId',
    '$Username',
    '$normalizedUsername',
    '$Username',
    '$normalizedEmail',
    1,
    '$passwordHash',
    '$securityStamp',
    '$concurrencyStamp',
    0,
    0,
    0,
    0
);
"@

# Execute the SQL command
try {
    sqlite3 $dbPath $sql
    Write-Host "User '$Username' added successfully with ID: $userId" -ForegroundColor Green
    Write-Host "Password: $Password" -ForegroundColor Yellow
} catch {
    Write-Host "Error adding user: $_" -ForegroundColor Red
} 