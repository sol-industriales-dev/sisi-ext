using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.RecursosHumanos.Reclutamientos
{
    public class tblRH_REC_PermisoTabuladorLibre
    {
        public int id { get; set; }
        public int usuario_id { get; set; }
        public bool registroActivo { get; set; }
    }
}
