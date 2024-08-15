using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Capacitacion
{
    public class AspectosCumplidosDTO
    {
        public int totalCiclos { get; set; }
        public int mes { get; set; }
        public string mesDesc { get; set; }
        public int tecnicaOperacional { get; set; }
        public string tecnicaOperacionalDesc { get; set; }
        public int comportamientoSeguro { get; set; }
        public string comportamientoSeguroDesc { get; set; }
        public int mantenimientoTecnico { get; set; }
        public string mantenimientoTecnicoDesc { get; set; }
    }
}
