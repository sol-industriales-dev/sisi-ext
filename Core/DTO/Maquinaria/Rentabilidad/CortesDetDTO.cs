using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Rentabilidad
{
    public class CortesDetDTO
    {
        public int semana { get; set; }
        public string areaCuenta { get; set; }
        public decimal montoActual { get; set; }
        public decimal montoAcumulado { get; set; }
        public string cuenta { get; set; }
    }
}
