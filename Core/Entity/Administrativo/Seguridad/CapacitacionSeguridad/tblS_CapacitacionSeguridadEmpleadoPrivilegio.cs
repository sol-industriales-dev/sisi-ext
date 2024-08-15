using Core.Enum.Administracion.Seguridad.CapacitacionSeguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.CapacitacionSeguridad
{
    public class tblS_CapacitacionSeguridadEmpleadoPrivilegio
    {
        public int id { get; set; }
        public int idUsuario { get; set; }
        public string nombre { get; set; }
        public string cc { get; set; }
        public int puesto_id { get; set; }
        public int idPrivilegio { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaRegistro { get; set; }
        public int division { get; set; }
    }
}
