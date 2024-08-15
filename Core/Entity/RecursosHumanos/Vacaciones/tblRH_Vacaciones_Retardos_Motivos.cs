using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Vacaciones
{
    public class tblRH_Vacaciones_Retardos_Motivos
    {
        public int id { get; set; }
        public int tipoRetardo { get; set; }
        public string descripcion { get; set; }
        public bool esActivo { get; set; }
    }
}
