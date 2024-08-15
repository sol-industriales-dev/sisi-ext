using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Proyecciones
{
    public class FiltrosGeneralDTO
    {
        public int escenario { get; set; }
        public decimal divisor { get; set; }
        public int mes { get; set; }
        public int anio { get; set; }
        public int inclCM { get; set; }
        public decimal reduccion { get; set; }
    }
}
