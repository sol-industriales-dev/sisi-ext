using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Reclutamientos
{
    public class tblRH_REC_MotivoSueldo
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public bool registroActivo { get; set; }
    }
}
