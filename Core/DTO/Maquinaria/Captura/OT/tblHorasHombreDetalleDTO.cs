using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura.OT
{
    public class tblHorasHombreDetalleDTO
    {
        public List<string> CC { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int grupo { get; set; }
        public int tipoParo { get; set; }
        public int empleadoID { get; set; }
        public int puestoID { get; set; }

    }
}
