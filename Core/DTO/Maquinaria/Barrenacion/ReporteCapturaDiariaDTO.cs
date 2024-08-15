using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Barrenacion
{
    public class ReporteCapturaDiariaDTO
    {
        public int id { get; set; }
        public DateTime fechaCapturaDate { get; set; }
        public DateTime fechaCaptura { get; set; }
        public string noEconomico { get; set; }
        public decimal horasTrabajadas { get; set; }
        public int turno { get; set; }
        public string operador { get; set; }
        public string ayudante { get; set; }
        public string tipoCaptura { get; set; }
        public decimal metrosLineales { get; set; }
        public decimal metrosLinealesHora { get; set; }
        public decimal toneladas { get; set; }
        public decimal toneladasHora { get; set; }
        public int barrenos { get; set; }
        public string areaCuenta { get; set; }

    }
}
