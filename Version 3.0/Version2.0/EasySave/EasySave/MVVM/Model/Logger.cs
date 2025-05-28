using LogLibrary;
using System;
using System.Security.Cryptography.X509Certificates;
using Xml_logger;

namespace EasySave_Logger
{
    public static class Logger
    {
        //singleton?
        public static void Log_State(string namePath, string sourceFile, string targetDestination, int totalFiles, long totalSize, int filesCopied, string logType)
        {
            int filesLeft = totalFiles - filesCopied;
            int progression = (int)((filesCopied / (double)totalFiles) * 100);

            if (logType.Equals("json", StringComparison.OrdinalIgnoreCase))
            {
                LogGenerator.GenerateLogState(
                    name: namePath,
                    srcPath: sourceFile,
                    dstPath: targetDestination,
                    state: "ACTIVE",
                    totalFiles: totalFiles,
                    totalSize: totalSize,
                    filesLeft: filesLeft,
                    progression: progression
                );
            }
            else
            {
                LogGestionnary.GenerateLogState(
                    namePath,
                    sourceFile,
                    targetDestination,
                    "ACTIVE",
                    totalFiles,
                    totalSize,
                    filesLeft,
                    progression
                );
            }
        }

        public static void Log_end(string type_log, string name_save, string sourceDir, string backupFolder, long totalSize, double totalDuration)
        {
            if (type_log.ToLower() == "json")
            {
                DailyLogGenerator.GenerateLogDay(name_save, sourceDir, backupFolder, totalSize, totalDuration);

                LogGenerator.GenerateLogState(
                    name: name_save,
                    srcPath: "",
                    dstPath: "",
                    state: "END",
                    totalFiles: 0,
                    totalSize: 0,
                    filesLeft: 0,
                    progression: 0
                );
            }
            else
            {
                DailyLogGestionnary.GenerateXMLDay(name_save, sourceDir, backupFolder, totalSize, totalDuration);
                LogGestionnary.GenerateLogState(name_save, "", "", "END", 0, 0, 0, 0);
            }
        }

        public static void Log_break(string type_log, string save)
        {
            if (type_log?.ToLower() == "json")
            {
                LogGenerator.GenerateLogState(
                    name: save,
                    srcPath: "",
                    dstPath: "",
                    state: "STOP",
                    totalFiles: 0,
                    totalSize: 0,
                    filesLeft: 0,
                    progression: 0
                );
            }
            else
            {
                LogGestionnary.GenerateLogState(save, "", "", "STOP", 0, 0, 0, 0);
            }
        }
    }
}
