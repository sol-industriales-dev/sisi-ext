using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Reclutamientos
{
    public class tblRH_REC_DiasPermitidos
    {
        public int id { get; set; }
        public int anteriores { get; set; }
        public int posteriores { get; set; }
        public bool registroActivo { get; set; }
        public int usuarioUltimaModificacion { get; set; }
        public DateTime fechaUltimaModificacion { get; set; }
    }
}
