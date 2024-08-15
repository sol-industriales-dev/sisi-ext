using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura.conciliacion
{
    public class ConciliacionHorometrosDTO
    {
        public int id { get; set; }
        public int idMaquinaria { get; set; }
        public string economico { get; set; }
        public string descripcion { get; set; }
        public string modelo { get; set; }
        public decimal HI { get; set; }
        public decimal HF { get; set; }
        public decimal HE { get; set; }
        public int unidad { get; set; }
        public string costo { get; set; }
        public int cargo { get; set; }
        public string comentario { get; set; }
        public decimal costoTotal { get; set; }
        public int moneda { get; set; }
        public decimal overhaul { get; set; }

    }
}
