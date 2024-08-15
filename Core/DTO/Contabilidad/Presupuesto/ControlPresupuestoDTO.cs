using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Presupuesto
{
    public class ControlPresupuestoDTO
    {
        public string AreaCuenta { get; set; }
        public string Tipo { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
    }
}
