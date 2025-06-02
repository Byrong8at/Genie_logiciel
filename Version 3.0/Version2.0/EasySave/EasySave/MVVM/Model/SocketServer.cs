// Fichier : SocketServer.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using EasySave.MVVM.Model;

namespace EasySave.MVVM.Model
{
    public class SocketServer
    {
        private TcpListener server;
        private Thread listenerThread;
        private bool isRunning = false;
        private readonly int port = 8888;

        public void Start()
        {
            server = new TcpListener(IPAddress.Any, port);
            server.Start();
            isRunning = true;

            listenerThread = new Thread(ListenForClients);
            listenerThread.Start();

            Console.WriteLine($"[SocketServer] Serveur démarré sur le port {port}");
        }

        public void Stop()
        {
            isRunning = false;
            server.Stop();
            listenerThread?.Join();
        }

        private void ListenForClients()
        {
            while (isRunning)
            {
                try
                {
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("[SocketServer] Client connecté.");

                    Thread clientThread = new Thread(() => HandleClient(client));
                    clientThread.Start();
                }
                catch (SocketException)
                {
                    if (isRunning)
                        Console.WriteLine("[SocketServer] Erreur socket.");
                }
            }
        }

        private void HandleClient(TcpClient client)
        {
            using NetworkStream stream = client.GetStream();
            using StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            using StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

            try
            {
                while (client.Connected)
                {
                    // 1. Envoyer l’état des sauvegardes
                    var status = GetBackupStatus();
                    string jsonStatus = JsonSerializer.Serialize(new
                    {
                        type = "status",
                        backups = status
                    });
                    writer.WriteLine(jsonStatus);

                    // 2. Attendre commande (non bloquant dans cette version)
                    if (stream.DataAvailable)
                    {
                        string incoming = reader.ReadLine();
                        HandleCommand(incoming);
                    }

                    Thread.Sleep(1000); // 1 seconde entre chaque envoi
                }
            }
            catch (IOException)
            {
                Console.WriteLine("[SocketServer] Client déconnecté.");
            }
        }

        private List<object> GetBackupStatus()
        {
            var list = Controller.Display_save();
            List<object> result = new();

            foreach (var save in list)
            {
                result.Add(new
                {
                    name = save.Name,
                    status = "running", 
                    progress = 0        
                });
            }

            return result;
        }

        private void HandleCommand(string json)
        {
            try
            {
                var doc = JsonDocument.Parse(json);
                string action = doc.RootElement.GetProperty("action").GetString();
                string name = doc.RootElement.GetProperty("name").GetString();

                switch (action.ToLower())
                {
                    case "pause":
                        Saver.State_Save = false;
                        break;
                    case "play":
                        Saver.State_Save = true;
                        break;
                    case "stop":
                        Saver.Break_Save = false;
                        break;
                }

                Console.WriteLine($"[SocketServer] Commande reçue : {action} sur {name}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"[SocketServer] Erreur de commande : {e.Message}");
            }
        }
    }
}
