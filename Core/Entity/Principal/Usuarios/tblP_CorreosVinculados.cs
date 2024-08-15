using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Principal.Usuarios
{
    public class tblP_CorreosVinculados
    {
        public int id { get; set; }
        public int usuarioPrincipalID { get; set; }
        public virtual tblP_Usuario usuarioPrincipal { get; set; }
        public int usuarioVinculadoID { get; set; }
        public virtual tblP_Usuario usuarioVinculado { get; set; }
    }
}
