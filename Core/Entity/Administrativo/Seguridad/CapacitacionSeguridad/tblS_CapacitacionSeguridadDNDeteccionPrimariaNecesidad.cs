using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.Administracion.Seguridad.CapacitacionSeguridad;

namespace Core.Entity.Administrativo.Seguridad.CapacitacionSeguridad
{
    public class tblS_CapacitacionSeguridadDNDeteccionPrimariaNecesidad
    {
        public int id { get; set; }
        public int metodo { get; set; }
        public string detecciones { get; set; }
        public int accionesCursoID { get; set; }
        public string observaciones { get; set; }
        public int deteccionPrimariaID { get; set; }
        public bool estatus { get; set; }
    }
}
