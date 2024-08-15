using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura
{
    public class kpiInfoGeneralDTO
    {
        public string noEconomico { get; set; }
        public decimal horometroInicial { get; set; }
        public decimal horometroFinal { get; set; }
        public decimal horasTrabajadas { get; set; }
        public string disponibilidad { get; set; }
        public decimal MTBS { get; set; }
        public decimal MTTR { get; set; }
        public decimal horasHombre { get; set; }
        public decimal horasParo { get; set; }
        public decimal ratioMantenimiento { get; set; }
    }
}
