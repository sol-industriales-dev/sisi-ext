using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Reclutamientos
{
    public class tblRH_UsuariosCorreoAutorizacionAlta
    {
        public int id { get; set; }
        public int usuario_id { get; set; }
        public bool registroActivo { get; set; }
    }
}
