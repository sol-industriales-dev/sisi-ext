using Core.Enum.Administracion.Seguridad.Capacitacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Capacitacion
{
    public class tblS_CapacitacionEmpleadoPrivilegio
    {
        public int id { get; set; }
        public int idUsuario { get; set; }
        public string nombre { get; set; }
        public string cc { get; set; }
        public int puesto_id { get; set; }
        public PrivilegioEnum idPrivilegio { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaRegistro { get; set; }
        public int division { get; set; }
    }
}
