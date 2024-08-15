using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Principal.Usuarios
{
    public class tblP_Autoriza
    {
        public int id { get; set; }
        public int usuarioID { get; set; }
        public int perfilAutorizaID { get; set; }
        public int cc_usuario_ID { get; set; }
        public virtual tblP_Usuario usuario { get; set; }
        //public virtual List<tblP_PerfilAutoriza> perfilAutoriza { get; set; }
        // public virtual List<tblP_CC_Usuario> CC_Usuario { get; set; }
    }
}
