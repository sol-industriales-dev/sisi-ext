using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Barrenacion
{
   public  class PiezasBarrenadoraDTO
    {
        public int id { get; set; }
        public string serieAutomatica { get; set; }
        public string serieManual { get; set; }
        public decimal precio { get; set; }
        public decimal horasAcumuladas { get; set; }
    }
}
