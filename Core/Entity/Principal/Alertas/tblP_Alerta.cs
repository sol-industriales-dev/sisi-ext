using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Principal.Alertas
{
    public class tblP_Alerta
    {
        public int id { get; set; }
        public int userEnviaID { get; set; }
        public int userRecibeID { get; set; }
        public int tipoAlerta { get; set; }
        public int sistemaID { get; set; }
        public bool visto { get; set; }
        public string url { get; set; }
        public int objID { get; set; }
        public string obj { get; set; }
        public string msj { get; set; }
        public int? documentoID { get; set; }
        public int moduloID { get; set; }
    }
}
