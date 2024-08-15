using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Barrenacion
{
    public class ReporteGeneralCapturaDTO
    {
        public string fechaCaptura { get; set; }
        public string equipo { get; set; }
        public decimal horasTrabajo { get; set; }
        public int turno { get; set; }
        public int barrenos { get; set; }
        public decimal metrosLineales { get; set; }
        public decimal metrosLinealesHora { get; set; }
        public decimal toneladas { get; set; }
        public string areaCuenta { get; set; }
        public decimal toneladasHora { get; set; }
    }
}
