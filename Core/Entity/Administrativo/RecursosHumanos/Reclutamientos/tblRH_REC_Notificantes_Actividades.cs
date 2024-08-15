using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.RecursosHumanos.Enkontrol
{
    public class tblRH_REC_Notificantes_Actividades
    {
        public int id { get; set; }
        public int idUsuario { get; set; }
        public int cveUsuario { get; set; }
        public int idActividad { get; set; }
        public bool esActivo { get; set; }
    }
}
