using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Reclutamientos
{
    public class tblRH_REC_Notificantes_Altas
    {
        public int id { get; set; }
        public int idUsuario { get; set; }
        public string cc { get; set; }
        public bool esAuth { get; set; }
        public bool notificarBaja { get; set; }
        public bool esActivo { get; set; }
    }
}
