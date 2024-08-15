using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Captura
{
    public class tblRH_FiniquitoFirmas
    {
        public int id { get; set; }
        public int finiquitoID { get; set; }
        public DateTime fecha { get; set; }
        public int usuarioID { get; set; }
        public string nombre { get; set; }
        public string ape_paterno { get; set; }
        public string ape_materno { get; set; }
        public bool estatus { get; set; }
        public bool autorizando { get; set; }
        public bool rechazado { get; set; }
        public int orden { get; set; }
    }
}
