using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace UserManagementApp
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Status { get; set; }

        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
