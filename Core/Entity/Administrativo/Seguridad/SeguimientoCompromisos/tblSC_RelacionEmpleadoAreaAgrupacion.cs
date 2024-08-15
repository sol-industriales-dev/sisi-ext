using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.SeguimientoCompromisos;

namespace Core.Entity.SeguimientoCompromisos
{
    public class tblSC_RelacionEmpleadoAreaAgrupacion
    {
        public int id { get; set; }
        public int usuario_id { get; set; }
        public int area { get; set; }
        public int agrupacion_id { get; set; }
        public TipoUsuarioSCEnum tipoUsuario { get; set; }
        public int usuarioCreacion_id { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int usuarioModificacion_id { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
    }
}
