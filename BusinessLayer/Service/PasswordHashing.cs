using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class PasswordHashing
    {

        private const int SaltSize = 16;  // Define the salt size (e.g., 16 bytes)
        private const int HashSize = 32;  // Define the hash size (e.g., 32 bytes)
        private const int Iterations = 10000;

        public  static string Hashing(string userPass)
        {
            try
            {
                byte[] salt = new byte[SaltSize];
                using var rng = RandomNumberGenerator.Create();
                rng.GetBytes(salt); // Generate a cryptographic salt

                using var pbkdf2 = new Rfc2898DeriveBytes(userPass, salt, Iterations, HashAlgorithmName.SHA256);
                byte[] hash = pbkdf2.GetBytes(HashSize);

                // Combine salt and hash
                byte[] hashByte = new byte[SaltSize + HashSize];
                Buffer.BlockCopy(salt, 0, hashByte, 0, SaltSize);
                Buffer.BlockCopy(hash, 0, hashByte, SaltSize, HashSize);

                return Convert.ToBase64String(hashByte); // Store as Base64 string
            }
            catch (Exception ex)
            {
                throw new Exception("Error hashing password.", ex);
            }
        }

        public static  bool VerifyPassword(string userPass, string storedHashPass)
        {
            try
            {
                byte[] hashBytes = Convert.FromBase64String(storedHashPass);
                byte[] salt = hashBytes[..SaltSize]; // Extract salt
                byte[] storedHash = hashBytes[SaltSize..]; // Extract stored hash

                using var pbkdf2 = new Rfc2898DeriveBytes(userPass, salt, Iterations, HashAlgorithmName.SHA256);
                byte[] computedHash = pbkdf2.GetBytes(HashSize);

                // Use SequenceEqual to prevent timing attacks
                return storedHash.AsSpan().SequenceEqual(computedHash);
            }
            catch (Exception ex)
            {
                throw new Exception("Error verifying password.", ex);
            }
        }

    }
}
