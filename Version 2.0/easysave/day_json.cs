using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SaveLog
{
    public string Name_save { get; set; }
    public string FileSource { get; set; }
    public string FileTarget { get; set; }
    public long FileSize { get; set; }           
    public double FileTransferTime { get; set; } // en secondes
    public string Time { get; set; }             
}

