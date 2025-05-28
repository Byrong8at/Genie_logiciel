using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave_Logiciel
{
    public static class Logiciel
    {

        public static event Action ProJobChanged;
        public static List<string> logicielMetierProcessName = new List<string> { "notepad.exe" }; // Liste des logiciels métier 

        public static void AddLogicielMetier(string processName)
        {
            if (!string.IsNullOrEmpty(processName) && !logicielMetierProcessName.Contains(processName))
            {
                logicielMetierProcessName.Add(processName);
                ProJobChanged?.Invoke(); // Notifie les abonnés que la liste a changé
            }
        }

        public static void RemoveLogicielMetier(string processName)
        {
            logicielMetierProcessName.Remove(processName);
            ProJobChanged?.Invoke(); // Notifie les abonnés que la liste a changé
        }

        public static bool IsLogicielMetier()
        {
            if (logicielMetierProcessName == null || logicielMetierProcessName.Count == 0) return false;

            foreach (string processName in logicielMetierProcessName)
            {
                if (string.IsNullOrEmpty(processName)) continue;

                Process[] processes = Process.GetProcessesByName(processName.Replace(".exe", ""));
                if (processes.Length > 0)
                {
                    return true; // Au moins un logiciel métier est en cours
                }
            }
            return false; // Aucun logiciel métier n'est en cours
        }

        public static bool LogicielHandler()
        {
            if (IsLogicielMetier())
            {
                Console.WriteLine("Error Running");
                return false;
            }
            return true;
        }
    }
}
