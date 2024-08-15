using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura.OT
{
    public class detFrecuenciaParoDTO
    {
        public string folio { get; set; }
        public int economicoID { get; set; }
        public string economico { get; set; }
        public decimal tiempoUtil { get; set; }
        public decimal tiempoMuerto { get; set; }
        public string comentariosSolucion { get; set; }
        public decimal cantidadPersonas { get; set; }
        public decimal horashombre { get; set; }

    }
}
