using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Captura
{
    public class tblRH_BN_Plantilla_Cuadrado_Det
    {
        public int id { get; set; }
        public int plantillaID { get; set; }
        public virtual tblRH_BN_Plantilla_Cuadrado plantilla { get; set; }
        public int puesto { get; set; }
        public string puestoNombre { get; set; }
        public int periodicidad { get; set; }
        public decimal monto { get; set; }
        public int tipoNominaCve { get; set; }
        public int depto { get; set; }
        public string deptoNombre { get; set; }
        public int empleado { get; set; }
        public string empleadoNombre { get; set; }
    }
}
