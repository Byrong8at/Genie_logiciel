using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Save_work
{
    public string Name { get; set; }
    public string Source_repertory { get; set; }
    public string Cible_repertory { get; set; }
    public string Save_type { get; set; }

    public Save_work(string name,string source_repertory,string cible_repertory,string save_type)
        {   
            Name = name;
            Source_repertory = source_repertory;
            Cible_repertory= cible_repertory;
            Save_type = save_type;
        }
    }
