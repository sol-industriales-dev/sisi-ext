using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura
{
    public class EconomicosHrsDTO
    {
        public string fechaInicio { get; set; }
        public string fechaFin { get; set; }
        public string noEconomico { get; set; }
        public string modelo { get; set; }
        public decimal horometroInicial { get; set; }
        public decimal horometroFinal { get; set; }
        public string areaCuenta { get; set; }
        public string cc { get; set; }
        public string nombreObra { get; set; }
        public decimal efectivo { get; set; }
    }
}
