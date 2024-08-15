using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Tabuladores
{
    public class RepTabuladoresDTO
    {
        public string id { get; set; }
        public string puesto { get; set; }
        public string categoria { get; set; }
        public string lineaNegocios { get; set; }
        public string sueldoBase { get; set; }
        public string sueldoComplemento { get; set; }
        public string sueldoTotal { get; set; }
        public string sueldoMensual { get; set; }
        public string nomina { get; set; }
        public string personal { get; set; }
        public string descAreaDepartamento { get; set; }
        public string descSindicato { get; set; }
    }
}
