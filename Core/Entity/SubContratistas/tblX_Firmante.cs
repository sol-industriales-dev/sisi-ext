using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.SubContratistas
{
    public class tblX_Firmante
    {
        public int id { get; set; }
        public int? usuarioId { get; set; }
        public int orden { get; set; }
        public string puesto { get; set; }
        public string titulo { get; set; }
        public string nombre { get; set; }
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        public string correo { get; set; }
        public bool esSubcontratista { get; set; }
        public string cc { get; set; }
        public bool esActivo { get; set; }

        [ForeignKey("usuarioId")]
        public virtual tblP_Usuario usuario { get; set; }
    }
}
