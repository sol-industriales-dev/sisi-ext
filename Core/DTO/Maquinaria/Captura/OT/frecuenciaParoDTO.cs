using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura.OT
{
    public class frecuenciaParoDTO
    {
        public int no { get; set; }
        public int motivoParoID { get; set; }
        public string motivoParo { get; set; }
        public int frecuenciaParo { get; set; }
        public decimal tiempoOT { get; set; }
        public decimal horasHombre { get; set; }
        public string detalleFrecuencia { get; set; }

    }
}
