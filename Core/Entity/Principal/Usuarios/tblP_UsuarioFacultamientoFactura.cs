using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.Multiempresa;
using Core.Enum.Principal.Usuario;

namespace Core.Entity.Principal.Usuarios
{
    public class tblP_UsuarioFacultamientoFactura
    {
        public int id { get; set; }
        public int usuario_id { get; set; }
        public TipoUsuarioFacultamientoFacturaEnum tipoUsuario { get; set; }
        public string cc { get; set; }
        public int area { get; set; }
        public int cuenta { get; set; }
        public EmpresaEnum empresa { get; set; }
        public bool registroActivo { get; set; }
    }
}
