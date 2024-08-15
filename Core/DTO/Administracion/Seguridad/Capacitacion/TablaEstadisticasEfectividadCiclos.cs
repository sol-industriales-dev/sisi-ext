using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Capacitacion
{
    public class TablaEstadisticasEfectividadCiclos
    {
        public string mes { get; set; }
        public decimal porcentajes { get; set; }
        public string proyectos { get; set; }
        public decimal promedioAnual { get; set; }
    }
}
