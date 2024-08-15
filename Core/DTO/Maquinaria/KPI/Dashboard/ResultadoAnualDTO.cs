using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.KPI.Dashboard
{
    public class ResultadoAnualDTO
    {
        public decimal disponible { get; set; }
        public decimal operacion { get; set; }
        public decimal trabajo { get; set; }
        public int horas { get; set; }
    }
}
