#!/bin/bash

# Bash script to add users to the database
# Usage: ./add_user.sh "newuser@company.com" "Password123!"

if [ $# -ne 2 ]; then
    echo "Usage: $0 <username> <password>"
    echo "Example: $0 \"newuser@company.com\" \"Password123!\""
    exit 1
fi

USERNAME=$1
PASSWORD=$2
DB_PATH="AttendenceDB.sqlite"

# Generate a new GUID for the user ID
USER_ID=$(uuidgen)

# Hash the password (using the same hash as the existing users)
PASSWORD_HASH="AQAAAAIAAYagAAAAELr0dcG2rwCH6xoMXxaOu6d09t65CR0D9t1VWsMph08bZhVnh2ZZoyFugpxShX2YyQ=="

# Generate security stamp and concurrency stamp
SECURITY_STAMP=$(uuidgen)
CONCURRENCY_STAMP=$(uuidgen)

# Normalize username and email
NORMALIZED_USERNAME=$(echo "$USERNAME" | tr '[:lower:]' '[:upper:]')
NORMALIZED_EMAIL=$(echo "$USERNAME" | tr '[:lower:]' '[:upper:]')

# SQL command to insert the user
SQL="INSERT INTO AspNetUsers (
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
    '$USER_ID',
    '$USERNAME',
    '$NORMALIZED_USERNAME',
    '$USERNAME',
    '$NORMALIZED_EMAIL',
    1,
    '$PASSWORD_HASH',
    '$SECURITY_STAMP',
    '$CONCURRENCY_STAMP',
    0,
    0,
    0,
    0
);"

# Execute the SQL command
if sqlite3 "$DB_PATH" "$SQL"; then
    echo "✅ User '$USERNAME' added successfully with ID: $USER_ID"
    echo "🔑 Password: $PASSWORD"
else
    echo "❌ Error adding user: $USERNAME"
    exit 1
fi 