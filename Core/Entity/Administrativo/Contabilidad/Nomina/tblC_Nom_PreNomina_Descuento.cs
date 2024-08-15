using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Nomina
{
    public class tblC_Nom_PreNomina_Descuento
    {
        public int id { get; set; }
        public int empleadoCve { get; set; }
        public int tipoDescuento { get; set; }
        public int anio { get; set; }
        public int tipoNomina { get; set; }
        public int periodoInicial { get; set; }
        public int periodoFinal { get; set; }
        public decimal monto { get; set; }
        public bool estatus { get; set; }
    }
}
