/// <summary>
/// Provides classes and methods for generating passwords.
/// </summary>
namespace PasswordGenerator
{
    /// <summary>
    /// Represents the entry point of the program.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The main method that is called when the program starts.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        public static void Main(string[] args)
        {
            Console.WriteLine("Password Generator");
            Console.WriteLine("Enter the length of the password: ");
            int length = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Password: " + Password.Generate(length));
            Console.WriteLine("Strong Password: " + StrongPassword.Generate(length));
            Console.ReadLine();
        }
    }

    /// <summary>
    /// Represents a utility class for generating passwords.
    /// </summary>
    public static class Password
    {
        private static readonly string _password = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// Generates a random password of the specified length.
        /// </summary>
        /// <param name="length">The length of the password to generate.</param>
        /// <returns>A randomly generated password.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the specified length is greater than the available characters in the password pool.</exception>
        public static string Generate(int length)
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThan(length, _password.Length, nameof(length));

            Random _randomPassword = new Random();
            string password = string.Empty;
            for (int i = 0; i < length; i++)
            {
                password += _password[_randomPassword.Next(_password.Length)];
            }

            return password;
        }
    }
    
    /// <summary>
    /// Represents a utility class for generating strong passwords.
    /// </summary>
    public static class StrongPassword
    {
    private static readonly string _password = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ~!@#$%^&*()-_=+{}|}:;'<>?,./";

        /// <summary>
        /// Generates a strong password of the specified length.
        /// </summary>
        /// <param name="length">The length of the password to generate.</param>
        /// <returns>A randomly generated strong password.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the specified length is greater than the available characters in the password pool.</exception>
        public static string Generate(int length)
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThan(length, _password.Length, nameof(length));

            Random _randomPassword = new Random();
            string password = string.Empty;
            for (int i = 0; i < length; i++)
            {
                password += _password[_randomPassword.Next(_password.Length)];
            }

            return password;
        }
    }

    /// <summary>
    /// Utility class for manipulating passwords.
    /// </summary>
    public static class PasswordUtils
    {
        /// <summary>
        /// Converts the given password to uppercase.
        /// </summary>
        /// <param name="password">The password to convert.</param>
        /// <returns>The converted password in uppercase.</returns>
        public static string ToUpperPassword(string password)
        {
            return password.ToUpper();
        }

        /// <summary>
        /// Converts the given password to lowercase.
        /// </summary>
        /// <param name="password">The password to convert.</param>
        /// <returns>The converted password in lowercase.</returns>
        public static string ToLowerPassword(string password)
        {
            return password.ToLower();
        }
    }
}