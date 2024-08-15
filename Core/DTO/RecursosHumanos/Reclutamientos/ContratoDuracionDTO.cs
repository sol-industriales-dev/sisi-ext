using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Reclutamientos
{
    public class ContratoDuracionDTO
    {
        public int clave_duracion { get; set; }
        public string nombre { get; set; }
        public int? duracion_meses { get; set; }
        public int? duracion_dias { get; set; }
        public string indefinido { get; set; }
        public int tipo_contrato { get; set; }
    }
}
