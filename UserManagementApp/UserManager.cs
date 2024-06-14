using System;
using System.Data.SqlClient;

namespace UserManagementApp
{
    public class UserManager
    {
        private readonly string connectionString;

        public UserManager(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void Register(User user)
        {
            user.Password = User.HashPassword(user.Password);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("INSERT INTO Users (Username, Password) VALUES (@Username, @Password)", connection);
                command.Parameters.AddWithValue("@Username", user.Username);
                command.Parameters.AddWithValue("@Password", user.Password);

                try
                {
                    command.ExecuteNonQuery();
                    Program.ShowMessage("Registration successful!", ConsoleColor.Green);
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627) // Unique constraint error
                    {
                        Program.ShowMessage("Registration failed! Username already exists.", ConsoleColor.Red);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        public User Login(string username, string password)
        {
            string hashedPassword = User.HashPassword(password);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Users WHERE Username = @Username AND Password = @Password", connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", hashedPassword);

                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new User
                    {
                        Username = reader["Username"].ToString(),
                        Password = reader["Password"].ToString(),
                        Status = reader["Status"].ToString()
                    };
                }
                else
                {
                    return null;
                }
            }
        }

        public void ChangeStatus(string username, string status)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("UPDATE Users SET Status = @Status WHERE Username = @Username", connection);
                command.Parameters.AddWithValue("@Status", status);
                command.Parameters.AddWithValue("@Username", username);

                command.ExecuteNonQuery();
                Program.ShowMessage("Status changed successfully!", ConsoleColor.Green);
            }
        }

        public void Search(string searchUsername)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT Username, Status FROM Users WHERE Username LIKE @Username", connection);
                command.Parameters.AddWithValue("@Username", searchUsername + "%");

                var reader = command.ExecuteReader();
                int count = 1;
                Console.ForegroundColor = ConsoleColor.Yellow;
                while (reader.Read())
                {
                    Console.WriteLine($"{count}- {reader["Username"]} status: {reader["Status"]}");
                    count++;
                }

                if (count == 1)
                {
                    Program.ShowMessage("No users found.", ConsoleColor.Red);
                }
                else
                {
                    Console.ResetColor();
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                }
            }
        }

        public void ChangePassword(string username, string oldPassword, string newPassword)
        {
            string hashedOldPassword = User.HashPassword(oldPassword);
            string hashedNewPassword = User.HashPassword(newPassword);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("UPDATE Users SET Password = @NewPassword WHERE Username = @Username AND Password = @OldPassword", connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@OldPassword", hashedOldPassword);
                command.Parameters.AddWithValue("@NewPassword", hashedNewPassword);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Program.ShowMessage("Password changed successfully!", ConsoleColor.Green);
                }
                else
                {
                    Program.ShowMessage("Password change failed! Incorrect old password.", ConsoleColor.Red);
                }
            }
        }
    }
}
