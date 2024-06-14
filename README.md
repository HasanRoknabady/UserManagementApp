# User Management Application

## Description
This is a simple console-based user management application written in C#. It allows users to register, log in, change status, search for other users, and change their password.

## Features
- Register a new user
- Login
- Change status (available/not available)
- Search for users by username
- Change password
- Logout

## Requirements
- .NET Framework
- SQL Server

## Setup Instructions

1. **Clone the repository**:
    ```sh
    git clone https://github.com/yourusername/UserManagementApp.git
    ```

2. **Navigate to the project directory**:
    ```sh
    cd UserManagementApp
    ```

3. **Setup the Database**:
    - Execute the following SQL script to create the database and the required table:

    ```sql
    CREATE DATABASE UserManagement;
    GO

    USE UserManagement;
    GO

    CREATE TABLE [dbo].[Users] (
        [Id]       INT           IDENTITY (1, 1) NOT NULL,
        [Username] NVARCHAR (50) NOT NULL,
        [Password] NVARCHAR (64) NOT NULL, -- SHA-256 hash size
        [Status]   NVARCHAR (20) DEFAULT ('not available') NOT NULL,
        PRIMARY KEY CLUSTERED ([Id] ASC),
        UNIQUE NONCLUSTERED ([Username] ASC)
    );
    GO
    ```

4. **Update Connection String**:
    - In the `Program.cs` file, update the `connectionString` variable with your SQL Server details.

5. **Build and Run**:
    - Open the project in Visual Studio or your preferred C# IDE.
    - Build the solution.
    - Run the application.

## Usage
- **Register**: `register --username [username] --password [password]`
- **Login**: `login --username [username] --password [password]`
- **Change Status**: `change --status [available/not available]`
- **Search Users**: `search --username [username]`
- **Change Password**: `changepassword --old [oldPassword] --new [newPassword]`
- **Logout**: `logout`

## License
This project is licensed under the MIT License.
