using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.Tablas.RH.Plantilla
{
    public class sn_plantilla_aditivaDTO
    {
        public int id { get; set; }
        public int id_plantilla { get; set; }
        public string cc { get; set; }
        public int puesto { get; set; }
        public string tipo { get; set; }
        public int cantidad { get; set; }
        public int solicita { get; set; }
        public DateTime fecha_solicita { get; set; }
        public int autoriza { get; set; }
        public DateTime fecha_autoriza { get; set; }
        public int visto_bueno { get; set; }
        public DateTime? fecha_visto_bueno { get; set; }
        public string estatus { get; set; }
        public DateTime fecha { get; set; }
        public string observaciones { get; set; }
    }
}
