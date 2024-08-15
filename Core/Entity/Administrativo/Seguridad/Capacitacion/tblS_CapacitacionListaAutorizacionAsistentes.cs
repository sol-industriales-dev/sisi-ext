using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Capacitacion
{
    public class tblS_CapacitacionListaAutorizacionAsistentes
    {
        public int id { get; set; }
        public int claveEmpleado { get; set; }
        public DateTime? fechaVencimiento { get; set; }
        public DateTime? fechaAsistencia { get; set; }
        public int listaAutorizacionID { get; set; }
        public bool estatus { get; set; }
    }
}
