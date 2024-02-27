using System.Text;

/// <summary>
/// Represents a namespace that contains classes for generating passwords and performing password-related operations.
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

            fastBruteForce.BruteForce();

            Console.WriteLine("Enter the length of the password: ");
            int length = Convert.ToInt32(Console.ReadLine());

            PasswordData._length = length;
            
            Console.WriteLine("Password: " + Password.Generate(length));
            Console.WriteLine("Strong Password: " + StrongPassword.Generate(length));
            Console.ReadLine();
            System.Console.WriteLine("Brute Force: ");
            PasswordUtils.BruteForce();
            Console.ReadLine();

            Vigener.CipherVigener(length);
            Console.ReadLine();
        }
    }

    /// <summary>
    /// Represents a class that performs fast brute force password cracking.
    /// </summary>
    public static class fastBruteForce 
    {
        static char[] Match = {'0','1','2','3','4','5','6','7','8','9','a','b','c','d','e','f','g','h','i','j' ,'k','l','m','n','o','p',
                        'q','r','s','t','u','v','w','x','y','z','A','B','C','D','E','F','G','H','I','J','C','L','M','N','O','P',
                        'Q','R','S','T','U','V','X','Y','Z','!','?',' ','*','-','+'};
        static string FindPassword;
        static int Combi;
        static string space = " ";
        static int Characters;

        /// <summary>
        /// Performs a brute force attack to find the password.
        /// </summary>
        public static void BruteForce()
        {
            int Count;
            FindPassword = Console.ReadLine();
            Characters = FindPassword.Length;

            DateTime start = DateTime.Now;
            start.ToString("yyyy-MM-dd_HH:mm:ss.fff");
            System.Console.WriteLine("Start Time: " + start);

            for (Count = 0; Count <= 15; Count++)
            {
                Recurse(Count, 0, "");
            }
        }

        /// <summary>
        /// Recursively generates combinations of characters and checks if the generated combination matches the desired password.
        /// </summary>
        /// <param name="Length">The length of the password.</param>
        /// <param name="Position">The current position in the password being generated.</param>
        /// <param name="BaseString">The base string representing the characters generated so far.</param>
        static void Recurse(int Length, int Position, string BaseString)
        {
            int Count = 0;

            for (Count = 0; Count < Match.Length; Count++)
            {
                Combi++;
                if (Position < Length - 1)
                {
                    Recurse(Length, Position + 1, BaseString + Match[Count]);
                }

                if (BaseString + Match[Count] == FindPassword)
                {
                    System.Console.WriteLine();
                    Console.WriteLine("Your password is: " + FindPassword);
                    Console.WriteLine("Your password is: " + Characters + " character(s) long");

                    DateTime end = DateTime.Now;
                    end.ToString("yyyy-MM-dd_HH:mm:ss");
                    Console.WriteLine(space);
                    Console.WriteLine("END:\t{0}\nCombi:\t{1}", end, Combi);
                    Console.ReadLine();
                }
            }           
        }
    }

    /// <summary>
    /// Represents the Vigener class for performing Vigener cipher decryption.
    /// </summary>
    public static class Vigener
    {
        static string dict = @"passwords.txt";
        static string output = @"output.txt";
        static List<string> dictionary = new List<string>();
        static Dictionary<string, int> countRefs = new Dictionary<string, int> {};

        /// <summary>
        /// Initializes the Vigener class by checking the existence of dictionary and output files.
        /// </summary>
        static Vigener()
        {
            if (!File.Exists(dict))
            {
                Console.WriteLine("Dictionary file not found");
                return;
            }
        
            if (!File.Exists(output))
            {
                Console.WriteLine("Output file not found");
                File.Create(output);
            }
        }

        /// <summary>
        /// Performs the Vigener cipher decryption with a given key length.
        /// </summary>
        /// <param name="keyLength">The length of the key to be used for decryption.</param>
        /// <param name="result">Optional parameter to store the result of the decryption.</param>
        public static void CipherVigener(int keyLength, string result = "")
        {
            string value = "YCUMLEMBAJJOPEEYSPETEOSGFJH";
            string attempt;
            int counter = 0;
            double progress = 0, total = 0;
            int limiter = 2;

            using (StreamReader sr = File.OpenText(dict))
            {
                string wordpre = "";
                while ((wordpre = sr.ReadLine()) != null)
                {
                    total += 1;
                    if (wordpre.Length > limiter)
                    {
                        dictionary.Add(wordpre);
                    }
                }
            }     

            using (StreamReader sr = File.OpenText(dict))
            {
                string word = "";
                while ((word = sr.ReadLine()) != null)
                {
                    progress += 1;
                    if (isAllLeters(word))
                    {
                        if (word.Length == keyLength)
                        {
                            attempt = decipherVigener(value, word);
                            if (dictionary.Any(attempt.Contains))
                            {
                                counter += 1;
                                System.Console.WriteLine("\r[S] " + word + "\t\t" + attempt);
                                using (StreamWriter sw = File.AppendText(output))
                                {
                                    sw.WriteLine("\r[S]" + word + "\t\t" + attempt);
                                }

                                countRefs.Add(word, dictionary.Count(s => attempt.Contains(s)));
                            }
                        }
                    }

                    System.Console.Write("\r>{0:n}%", (100/total)*progress);
                }
            }

            System.Console.WriteLine("  [Total: {0}]", total);
            System.Console.WriteLine("");

            System.Console.WriteLine("// ######## ######## ######## //");
            System.Console.WriteLine(" > MOST PROBABLE KEYS / VALUES");
            System.Console.WriteLine("// ######## ######## ######## //\n");

            foreach (var item in countRefs.OrderByDescending(r => r.Value).Take(10))
            {
                System.Console.WriteLine("KEY {0}, \"{1}\" [{2}]", item.Key, decipherVigener(value, item.Key), item.Value);
            }

            System.Console.WriteLine("");
            System.Console.WriteLine("");
        }

        /// <summary>
        /// Checks if a given string contains only letters.
        /// </summary>
        /// <param name="s">The string to be checked.</param>
        /// <returns>True if the string contains only letters, otherwise false.</returns>
        public static bool isAllLeters(string s) 
        {
            foreach(char c in s)
            {
                if (!char.IsLetter(c))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Deciphers the given text using the Vigenere cipher algorithm with the provided key.
        /// </summary>
        /// <param name="text">The text to be deciphered.</param>
        /// <param name="key">The key used for deciphering the text.</param>
        /// <returns>The deciphered text.</returns>
        static string decipherVigener(string text, string key)
        {
            StringBuilder result = new StringBuilder();
            int keyLength = key.Length;
            int diff;
            char decoded;

            text = text.Replace(" ", "").ToLower();

            for (int i = 0; i < text.Length; i++)
            {
                diff = text[i] - key[i%keyLength];

                if (diff < 0)
                {
                    diff += 26;
                }

                decoded = (char)(diff + 'a');
                result.Append(decoded);
            }

            return result.ToString();
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
    /// Represents the password data used for generating passwords.
    /// </summary>
    public static class PasswordData
    {
        public static char[] _password = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ~!@#$%^&*()-_+={}|}:;'<>?,./".ToCharArray();
        public static int _length;
    }


    /// <summary>
    /// Utility class for manipulating passwords.
    /// </summary>
    public static class PasswordUtils
    {
        /// <summary>
        /// Recursively generates all possible combinations of a password using a brute force approach.
        /// </summary>
        /// <param name="result">The current combination of characters being generated.</param>
        public static void BruteForce(string result = "")
        {
            if (result.Length == PasswordData._length)
            {
                System.Console.WriteLine(result);
            }  
            else
            {
                for (int i = 0; i < PasswordData._password.Length; i++)
                {
                    BruteForce(result + PasswordData._password[i]);
                }
            }
        }
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