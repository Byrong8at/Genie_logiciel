using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class SaveWork
{
    public string Name { get; set; }
    public string Source_repertory { get; set; }
    public string Cible_repertory { get; set; }

    public string Log_type { get; set; }

    public SaveWork(string name, string source_repertory, string cible_repertory, string log_type)
    {
        Name = name;
        Source_repertory = source_repertory;
        Cible_repertory = cible_repertory;
        Log_type = log_type;
    }


}

