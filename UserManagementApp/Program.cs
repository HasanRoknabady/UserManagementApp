using System;

namespace UserManagementApp
{
    internal class Program
    {
        static string connectionString = "Data Source=HASAN;Initial Catalog=UserManagement;Integrated Security=True";
        static string loggedInUser = null;
        static UserManager userManager;

        static void Main(string[] args)
        {
            userManager = new UserManager(connectionString);

            ShowWelcomeMessage();

            while (true)
            {
                try
                {
                    ShowMainMenu();

                    string input = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(input))
                    {
                        ShowMessage("Invalid command", ConsoleColor.Red);
                        continue;
                    }

                    var commandParts = input.Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
                    var command = commandParts[0].ToLower();
                    var parameters = commandParts.Length > 1 ? commandParts[1] : string.Empty;

                    switch (command)
                    {
                        case "register":
                            Register(parameters);
                            break;
                        case "login":
                            Login(parameters);
                            break;
                        case "change":
                            Change(parameters);
                            break;
                        case "search":
                            Search(parameters);
                            break;
                        case "changepassword":
                            ChangePassword(parameters);
                            break;
                        case "logout":
                            Logout();
                            break;
                        default:
                            ShowMessage("Invalid command", ConsoleColor.Red);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    ShowMessage($"Error: {ex.Message}", ConsoleColor.Red);
                }
            }
        }

        static void ShowWelcomeMessage()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("======================================");
            Console.WriteLine("    Welcome to the User Management    ");
            Console.WriteLine("======================================");
            Console.ResetColor();
            if (loggedInUser != null)
            {
                Console.WriteLine($"Logged in as: {loggedInUser}");
            }
            Console.ResetColor();
        }

        static void ShowMainMenu()
        {
            Console.Clear();
            ShowWelcomeMessage();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nAvailable Commands:");
            Console.WriteLine("1. Register --username [username] --password [password]");
            Console.WriteLine("2. Login --username [username] --password [password]");
            if (loggedInUser != null)
            {
                Console.WriteLine("3. Change --status [available/not available]");
                Console.WriteLine("4. Search --username [username]");
                Console.WriteLine("5. ChangePassword --old [oldPassword] --new [newPassword]");
                Console.WriteLine("6. Logout");
            }
            Console.ResetColor();
            Console.Write("Enter command: ");
        }

        public static void ShowMessage(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        static void Register(string parameters)
        {
            var username = GetParameterValue(parameters, "username");
            var password = GetParameterValue(parameters, "password");

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ShowMessage("Username and password cannot be empty.", ConsoleColor.Red);
                return;
            }

            User user = new User
            {
                Username = username,
                Password = password
            };

            userManager.Register(user);
        }

        static void Login(string parameters)
        {
            var username = GetParameterValue(parameters, "username");
            var password = GetParameterValue(parameters, "password");

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ShowMessage("Username and password cannot be empty.", ConsoleColor.Red);
                return;
            }

            var user = userManager.Login(username, password);
            if (user != null)
            {
                loggedInUser = user.Username;
                ShowMessage($"Login successful! Welcome, {loggedInUser}.", ConsoleColor.Green);
            }
            else
            {
                ShowMessage("Login failed! Invalid username or password.", ConsoleColor.Red);
            }
        }

        static void Change(string parameters)
        {
            if (loggedInUser == null)
            {
                ShowMessage("Please login first.", ConsoleColor.Red);
                return;
            }

            var status = GetParameterValue(parameters, "status");

            if (status != "available" && status != "not available")
            {
                ShowMessage("Invalid status. Status must be 'available' or 'not available'.", ConsoleColor.Red);
                return;
            }

            userManager.ChangeStatus(loggedInUser, status);
        }

        static void Search(string parameters)
        {
            if (loggedInUser == null)
            {
                ShowMessage("Please login first.", ConsoleColor.Red);
                return;
            }

            var searchUsername = GetParameterValue(parameters, "username");

            if (string.IsNullOrWhiteSpace(searchUsername))
            {
                ShowMessage("Search username cannot be empty.", ConsoleColor.Red);
                return;
            }

            userManager.Search(searchUsername);
        }

        static void ChangePassword(string parameters)
        {
            if (loggedInUser == null)
            {
                ShowMessage("Please login first.", ConsoleColor.Red);
                return;
            }

            var oldPassword = GetParameterValue(parameters, "old");
            var newPassword = GetParameterValue(parameters, "new");

            if (string.IsNullOrWhiteSpace(oldPassword) || string.IsNullOrWhiteSpace(newPassword))
            {
                ShowMessage("Old password and new password cannot be empty.", ConsoleColor.Red);
                return;
            }

            userManager.ChangePassword(loggedInUser, oldPassword, newPassword);
        }

        static void Logout()
        {
            if (loggedInUser == null)
            {
                ShowMessage("You are not logged in.", ConsoleColor.Red);
            }
            else
            {
                loggedInUser = null;
                ShowMessage("Logged out successfully!", ConsoleColor.Green);
                ShowMainMenu();
            }
        }

        static string GetParameterValue(string parameters, string key)
        {
            var keyValuePair = parameters.Split(new[] { $"--{key} " }, StringSplitOptions.None);
            if (keyValuePair.Length < 2)
            {
                throw new ArgumentException($"Parameter {key} not found");
            }
            return keyValuePair[1].Split(new[] { ' ' }, 2)[0];
        }
    }
}
