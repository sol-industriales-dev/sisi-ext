using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Capacitacion.CincoS
{
    public class TablaAccionesSeguimiento
    {
        public string mes { get; set; }
        public decimal total { get; set; }
        public decimal pendientes { get; set; }
        public decimal concluidas { get; set; }
    }
}
