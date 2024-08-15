using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.MAZDA
{
    public class ActividadDTO
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public string detalle { get; set; }
        public int areaID { get; set; }
        public string area { get; set; }
        public int periodo { get; set; }
        public string periodoDesc { get; set; }
        public int cuadrillaID { get; set; }
        public string cuadrilla { get; set; }
    }
}
