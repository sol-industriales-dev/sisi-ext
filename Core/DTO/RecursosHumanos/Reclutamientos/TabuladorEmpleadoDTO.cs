using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Reclutamientos
{
    public class TabuladorEmpleadoDTO
    {
        public DateTime? fecha_cambio { get; set; }
        public DateTime fecha_real { get; set; }
        public decimal salario_base { get; set; }
        public decimal complemento { get; set; }
        public decimal bono_de_zona { get; set; }
        public decimal total { get; set; }
        public int tipo_nomina { get; set; }
        public int tabuladorId { get; set; }
        public bool esNuevoTabulador { get; set; }
    }
}
