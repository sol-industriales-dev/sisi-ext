using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.Administracion.Seguridad.CapacitacionSeguridad;

namespace Core.Entity.Administrativo.Seguridad.CapacitacionSeguridad
{
    public class tblS_CapacitacionSeguridadDNCicloTrabajo
    {
        public int id { get; set; }
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public int tipoCiclo { get; set; }
        public DateTime? fechaCiclo { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public int division { get; set; }
        public bool estatus { get; set; }
    }
}
