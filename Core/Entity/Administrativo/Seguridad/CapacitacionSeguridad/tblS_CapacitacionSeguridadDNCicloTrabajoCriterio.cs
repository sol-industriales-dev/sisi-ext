using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.Administracion.Seguridad.CapacitacionSeguridad;

namespace Core.Entity.Administrativo.Seguridad.CapacitacionSeguridad
{
    public class tblS_CapacitacionSeguridadDNCicloTrabajoCriterio
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public int ponderacion { get; set; }
        public int cicloTrabajoID { get; set; }
        public bool estatus { get; set; }
    }
}
