using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura
{
    public class KPINoFormatDTO
    {
        public int id { get; set; }
        public string economico { get; set; }
        public decimal horasIdealMensual { get; set; }
        public decimal pDisponibilidad { get; set; }
        public decimal horasTrabajado { get; set; }
        public decimal horasParo { get; set; }
        public decimal pMProgramadoTiempo { get; set; }
        public decimal pMProgramadoCantidad { get; set; }
        public decimal pPreventivoHoras { get; set; }
        public decimal pCorrectivoHoras { get; set; }
        public decimal pPredictivoHoras { get; set; }
        public decimal horasHombre { get; set; }
        public decimal MTBS { get; set; }
        public decimal MTTR { get; set; }
        public decimal pUtilizacion { get; set; }
    }
}
