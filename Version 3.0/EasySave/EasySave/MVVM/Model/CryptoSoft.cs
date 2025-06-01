using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace easysave_Crypto
{
    public static class CryptoManager
    {
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("1234567890ABCDEF1234567890ABCDEF"); // 32 bytes
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("ABCDEF1234567890"); // 16 bytes
        private const string MutexName = "Global\\EasySave_Crypto_Mutex";

        public static void EncryptFile(string inputPath, string outputPath)
        {
            if (!TryRunCryptoOperation(() =>
            {
                using FileStream inputFile = new(inputPath, FileMode.Open, FileAccess.Read);
                using FileStream outputFile = new(outputPath, FileMode.Create, FileAccess.Write);
                using Aes aes = Aes.Create();
                aes.Key = Key;
                aes.IV = IV;

                using CryptoStream cryptoStream = new(outputFile, aes.CreateEncryptor(), CryptoStreamMode.Write);
                inputFile.CopyTo(cryptoStream);
            }, "encryption", inputPath))
            {
                Console.WriteLine($"Le chiffrement de '{inputPath}' a été annulé car CryptoSoft est déjà en cours d’utilisation.");
            }
        }

        public static void DecryptFile(string inputPath, string outputPath)
        {
            if (!TryRunCryptoOperation(() =>
            {
                using FileStream inputFile = new(inputPath, FileMode.Open, FileAccess.Read);
                using FileStream outputFile = new(outputPath, FileMode.Create, FileAccess.Write);
                using Aes aes = Aes.Create();
                aes.Key = Key;
                aes.IV = IV;

                using CryptoStream cryptoStream = new(inputFile, aes.CreateDecryptor(), CryptoStreamMode.Read);
                cryptoStream.CopyTo(outputFile);
            }, "decryption", inputPath))
            {
                Console.WriteLine($"Le déchiffrement de '{inputPath}' a été annulé car CryptoSoft est déjà en cours d’utilisation.");
            }
        }

        /// <summary>
        /// Gère l'acquisition du mutex, l'exécution sécurisée et le release, avec gestion des exceptions.
        /// </summary>
        private static bool TryRunCryptoOperation(Action cryptoAction, string operationType, string targetFile)
        {
            using var mutex = new Mutex(false, MutexName);
            bool isOwned = false;

            try
            {
                // Attente jusqu'à 10 secondes pour obtenir le mutex
                isOwned = mutex.WaitOne(TimeSpan.FromSeconds(10), false);

                if (!isOwned)
                {
                    LogMutexRefused(operationType, targetFile);
                    return false;
                }

                cryptoAction.Invoke(); // Exécute l'action de chiffrement/déchiffrement
                return true;
            }
            catch (AbandonedMutexException)
            {
                // Un processus précédent a planté → mutex abandonné, on le récupère quand même
                Console.WriteLine("⚠️ Mutex abandonné récupéré. Une précédente instance de CryptoSoft s’est fermée brutalement.");
                cryptoAction.Invoke();
                return true;
            }
            catch (Exception ex)
            {
                LogCryptoError(operationType, targetFile, ex);
                return false;
            }
            finally
            {
                if (isOwned)
                {
                    try
                    {
                        mutex.ReleaseMutex();
                    }
                    catch (ApplicationException ex)
                    {
                        Console.WriteLine("Erreur lors du release du mutex : " + ex.Message);
                    }
                }
            }
        }

        private static void LogMutexRefused(string operationType, string targetFile)
        {
            Console.WriteLine($"[CryptoSoft Mono-Instance] Refusé : une autre instance est en cours. Opération : {operationType}, Fichier : {targetFile}");
            // Tu peux aussi logger ça dans ton logger XML/JSON ici
        }

        private static void LogCryptoError(string operationType, string targetFile, Exception ex)
        {
            Console.WriteLine($"Erreur pendant {operationType} de '{targetFile}' : {ex.Message}");
            // Log JSON/XML à ajouter ici si tu veux tracer dans les fichiers
        }
    }
}
