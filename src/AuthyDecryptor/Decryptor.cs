using System.Security.Cryptography;
using System.Text;

namespace AuthyDecryptor;

public static class Decryptor
{
    public static string DecryptToken(int kdfIterations, string encryptedSeedBase64, string salt, string iv,
        string passphrase)
    {
        try
        {
            // Decode the base64-encoded encrypted seed
            var encryptedSeed = Convert.FromBase64String(encryptedSeedBase64);

            // Derive the encryption key using PBKDF2 with SHA-1
            using (var deriveBytes = new Rfc2898DeriveBytes(passphrase, Encoding.UTF8.GetBytes(salt), kdfIterations,
                       HashAlgorithmName.SHA1))
            {
                var key = deriveBytes.GetBytes(32); // AES-256 always requires a 32-byte key

                // Get the IV for AES decryption
                var ivBytes = string.IsNullOrEmpty(iv) ? new byte[16] : Convert.FromHexString(iv);

                // AES CBC
                using (var aes = Aes.Create())
                {
                    aes.Key = key;
                    aes.IV = ivBytes;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.None;

                    using (var decryptor = aes.CreateDecryptor())
                    {
                        // Decrypt the ciphertext
                        var decryptedData = decryptor.TransformFinalBlock(encryptedSeed, 0, encryptedSeed.Length);

                        // Remove PKCS7 padding
                        var padLen = decryptedData[^1];
                        var padStart = decryptedData.Length - padLen;

                        // Validate padding
                        if (padLen > 16 || padStart < 0)
                        {
                            throw new ArgumentException("Padding: Invalid length");
                        }

                        for (var i = padStart; i < decryptedData.Length; i++)
                        {
                            if (decryptedData[i] != padLen)
                            {
                                throw new ArgumentException("Padding: Invalid bytes");
                            }
                        }

                        return Encoding.UTF8.GetString(decryptedData, 0, padStart);
                    }
                }
            }
        }
        catch (Exception e)
        {
            return $"Decryption failed: {e.Message}";
        }
    }
}