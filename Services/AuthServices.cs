using System.Text;
using System.Security.Cryptography; // This allows us to use SHA256 hashing.
using System.Text; // This helps with text encoding (converting text into bytes).


namespace TaskManagement.Services
{
    public class AuthServices
    {

        //Hashing the password. This method takes a string (our password we enter) and runs it though .NET's sha256 hashing algo.
        public static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }


        //checking to see if the password entered matches the hash that we have stored
        public static bool VerifyPassword(string enteredPassword, string storedHash)
        {
            return HashPassword(enteredPassword) == storedHash;
        }


        /*  User Registration

A user signs up with password "mypassword".
We hash "mypassword" into "XohImNooBHFR0OVq3SRZ6Af98WMebQ4OtDxhJ6YqQbw=".
We store "XohImNooBHFR0OVq3SRZ6Af98WMebQ4OtDxhJ6YqQbw=" in the database (not "mypassword").
2️⃣ User Login

The user enters "mypassword" to log in.
We hash "mypassword" again (which gives the same hash).
We compare the newly hashed password with the stored one.
If they match, the login is successful! */

    }
}
