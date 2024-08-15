using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.CapacitacionSeguridad
{
    public class RecorridoReporteDTO
    {
        public string proyectoObra { get; set; }
        public string area { get; set; }
        public string fecha { get; set; }
        public string realizador { get; set; }
        public List<HallazgoReporteDTO> listaHallazgos { get; set; }
    }
}
