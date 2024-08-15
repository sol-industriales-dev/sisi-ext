using Core.Enum.Administracion.Seguridad.Requerimientos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Requerimientos
{
    public class tblS_Req_EmpleadoAreaCC
    {
        public int id { get; set; }
        public int idUsuario { get; set; }
        public int empleado { get; set; }
        public int area { get; set; }
        public string cc { get; set; }
        public int division { get; set; }
        public bool estatus { get; set; }
        public int idEmpresa { get; set; }
        public int? idAgrupacion { get; set; }
        public bool esContratista { get; set; }
    }
}
