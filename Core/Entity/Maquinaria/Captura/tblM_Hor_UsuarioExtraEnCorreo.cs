using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Captura
{
    public class tblM_Hor_UsuarioExtraEnCorreo
    {
        public int id { get; set; }
        public int usuarioId { get; set; }
        public string cc { get; set; }
        public bool registroActivo { get; set; }
        public DateTime fechaRegistro { get; set; }
        public DateTime fechaUltimaModificacion { get; set; }
        public int usuarioRegistro { get; set; }
        public int usuarioUltimaModificacion { get; set; }

        public virtual tblP_Usuario usuario { get; set; }
    }
}
