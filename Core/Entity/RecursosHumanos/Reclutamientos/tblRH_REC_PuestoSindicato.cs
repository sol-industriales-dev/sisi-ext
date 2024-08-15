using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Reclutamientos
{
    public class tblRH_REC_PuestoSindicato
    {
        public int id { get; set; }
        public int puesto { get; set; }
        public string descripcion { get; set; }
        public string sindicalizado { get; set; }
    }
}
