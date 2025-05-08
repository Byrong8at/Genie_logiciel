using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class state_log
{
    public string name { get; set; }
    public string SourceFilePath { get; set; }
    public string TargetFilePath { get; set; }
    public string State { get; set; }
    public int TotalFilesToCopy { get; set; } // en secondes
    public long TotalFilesSize { get; set; } // en secondes
    public int NbFilesLeftToDo { get; set; } // en secondes
    public int Progression { get; set; }
}

