using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Reclutamientos
{
    public class tblRH_REC_TipoDocumento
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public int tipo { get; set; }
        public bool estatus { get; set; }
        public int orden { get; set; }
    }
}
