using System.Diagnostics;

public class CryptoSoftWrapper
{
    private const string CryptoSoftPath = @"C:\Program Files\CryptoSoft\CryptoSoft.exe";

    public static bool EncryptFile(string inputFilePath)
    {
        try
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = CryptoSoftPath,
                Arguments = $"\"{inputFilePath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process proc = Process.Start(psi))
            {
                if (proc == null)
                    throw new InvalidOperationException("Impossible de démarrer CryptoSoft.");

                proc.WaitForExit();

                // Optionnel : récupération des logs
                string output = proc.StandardOutput.ReadToEnd();
                string error = proc.StandardError.ReadToEnd();

                if (proc.ExitCode != 0)
                {
                    Console.WriteLine($"Erreur CryptoSoft : {error}");
                    return false;
                }

                return true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception dans CryptoSoftWrapper : {ex.Message}");
            return false;
        }
    }
}
