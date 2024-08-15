using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.CapacitacionSeguridad
{
    public class tblS_CapacitacionSeguridadListaAutorizacionAsistentes
    {
        public int id { get; set; }
        public int claveEmpleado { get; set; }
        public DateTime? fechaVencimiento { get; set; }
        public int listaAutorizacionID { get; set; }
        public bool estatus { get; set; }
    }
}
