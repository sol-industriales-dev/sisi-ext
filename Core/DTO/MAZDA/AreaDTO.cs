using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.MAZDA
{
    public class AreaDTO
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public int cuadrillaID { get; set; }
        public string cuadrilla { get; set; }

        public List<ActividadDTO> actividades { get; set; }
    }
}
