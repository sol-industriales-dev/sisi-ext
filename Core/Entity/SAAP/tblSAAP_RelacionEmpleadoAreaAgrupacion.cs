using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.SAAP;

namespace Core.Entity.SAAP
{
    public class tblSAAP_RelacionEmpleadoAreaAgrupacion
    {
        public int id { get; set; }
        public int usuario_id { get; set; }
        public int area { get; set; }
        public int agrupacion_id { get; set; }
        public TipoUsuarioEnum tipoUsuario { get; set; }
        public int usuarioCreacion_id { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int usuarioModificacion_id { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
    }
}
