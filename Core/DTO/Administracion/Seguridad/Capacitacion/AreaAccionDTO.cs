using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Capacitacion
{
    public class AreaAccionDTO
    {
        public string cicloDesc { get; set; }
        public string areaDesc { get; set; }
        public int accionesRegistradas { get; set; }
        public int proceso { get; set; }
        public int solventadas { get; set; }
        public decimal porcentajeSolventadas { get; set; }

        public string accionDesc { get; set; }
        public string responsableNombre { get; set; }
        public string fechaDeteccionString { get; set; }
        public string tiempoTranscurrido { get; set; }
    }
}
