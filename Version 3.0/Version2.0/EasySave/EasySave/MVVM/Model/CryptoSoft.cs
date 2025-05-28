using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace easysave_Crypto
{
    public static class CryptoManager
    {
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("1234567890ABCDEF1234567890ABCDEF"); // 32 bytes (AES-256)
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("ABCDEF1234567890"); // 16 bytes (AES)

        public static void EncryptFile(string inputPath, string outputPath)
        {
            using FileStream inputFile = new FileStream(inputPath, FileMode.Open, FileAccess.Read);
            using FileStream outputFile = new FileStream(outputPath, FileMode.Create, FileAccess.Write);
            using Aes aes = Aes.Create();
            aes.Key = Key;
            aes.IV = IV;

            using CryptoStream cryptoStream = new CryptoStream(outputFile, aes.CreateEncryptor(), CryptoStreamMode.Write);
            inputFile.CopyTo(cryptoStream);
        }

        public static void DecryptFile(string inputPath, string outputPath)
        {
            using FileStream inputFile = new FileStream(inputPath, FileMode.Open, FileAccess.Read);
            using FileStream outputFile = new FileStream(outputPath, FileMode.Create, FileAccess.Write);
            using Aes aes = Aes.Create();
            aes.Key = Key;
            aes.IV = IV;

            using CryptoStream cryptoStream = new CryptoStream(inputFile, aes.CreateDecryptor(), CryptoStreamMode.Read);
            cryptoStream.CopyTo(outputFile);
        }
    }
}
