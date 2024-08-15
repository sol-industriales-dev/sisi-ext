using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.Administracion.Seguridad.CapacitacionSeguridad;
using Core.Entity.Principal.Usuarios;

namespace Core.Entity.Administrativo.Seguridad.CapacitacionSeguridad
{
    public class tblS_CapacitacionSeguridadAutorizacion
    {
        public int id { get; set; }
        public int controlAsistenciaID { get; set; }
        public virtual tblS_CapacitacionSeguridadControlAsistencia controlAsistencia { get; set; }
        public int usuarioID { get; set; }
        public virtual tblP_Usuario usuario { get; set; }
        public int estatus { get; set; }
        public int tipoPuesto { get; set; }
        public int orden { get; set; }
        public string firma { get; set; }
        public DateTime? fecha { get; set; }
        public int division { get; set; }
    }
}
