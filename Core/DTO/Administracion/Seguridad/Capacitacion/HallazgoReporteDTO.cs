using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Capacitacion
{
    public class HallazgoReporteDTO
    {
        public string deteccion { get; set; }
        public string recomendacion { get; set; }
        public string clasificacion { get; set; }
        public string lideres { get; set; }
        public Byte[] evidencia { get; set; }
    }
}
