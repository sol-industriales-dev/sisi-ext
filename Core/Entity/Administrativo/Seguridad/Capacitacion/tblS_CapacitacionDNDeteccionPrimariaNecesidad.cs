using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.Administracion.Seguridad.Capacitacion;

namespace Core.Entity.Administrativo.Seguridad.Capacitacion
{
    public class tblS_CapacitacionDNDeteccionPrimariaNecesidad
    {
        public int id { get; set; }
        public MetodoEnum metodo { get; set; }
        public string detecciones { get; set; }
        public int accionesCursoID { get; set; }
        public string observaciones { get; set; }
        public int deteccionPrimariaID { get; set; }
        public bool estatus { get; set; }
    }
}
