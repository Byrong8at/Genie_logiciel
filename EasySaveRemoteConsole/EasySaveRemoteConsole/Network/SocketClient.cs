using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;
using EasySaveRemoteConsole.MVVM.Model;

namespace EasySaveRemoteConsole.Network
{
    public class SocketClient
    {
        private TcpClient client;
        private StreamReader reader;

        public event Action<List<BackupStatus>> OnStatusReceived;

        public async Task ConnectAsync(string host = "localhost", int port = 8888)
        {
            try
            {
                client = new TcpClient();
                await client.ConnectAsync(host, port);
                reader = new StreamReader(client.GetStream());

                // Commence à écouter les messages
                _ = Task.Run(ListenLoop);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur de connexion : " + ex.Message);
            }
        }

        private async Task ListenLoop()
        {
            while (client.Connected)
            {
                try
                {
                    var jsonLine = await reader.ReadLineAsync();
                    if (jsonLine != null)
                    {
                        var doc = JsonDocument.Parse(jsonLine);
                        if (doc.RootElement.GetProperty("type").GetString() == "status")
                        {
                            var backups = new List<BackupStatus>();
                            foreach (var backup in doc.RootElement.GetProperty("backups").EnumerateArray())
                            {
                                backups.Add(new BackupStatus
                                {
                                    Name = backup.GetProperty("name").GetString(),
                                    Status = backup.GetProperty("status").GetString(),
                                    Progress = backup.GetProperty("progress").GetInt32()
                                });
                            }

                            OnStatusReceived?.Invoke(backups);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Erreur pendant lecture : " + e.Message);
                }
            }
        }

        public async Task SendCommand(string name, string action)
        {
            try
            {
                var json = JsonSerializer.Serialize(new
                {
                    type = "command",
                    name,
                    action
                });

                var writer = new StreamWriter(client.GetStream()) { AutoFlush = true };
                await writer.WriteLineAsync(json);
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur d’envoi : " + e.Message);
            }
        }
    }
}
