using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Reclutamientos
{
    public class NuevoTabuladorDTO
    {
        public int tabulador { get; set; }
        public int puesto { get; set; }
        public decimal salario_base { get; set; }
        public decimal complemento { get; set; }
        public decimal bono_de_zona { get; set; }
        public int year { get; set; }
        public int? motivoCambio { get; set; }
        public DateTime? fechaAplicaCambio { get; set; }
    }
}
