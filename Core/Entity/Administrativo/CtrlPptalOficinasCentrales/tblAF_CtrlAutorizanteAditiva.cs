using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.CtrlPptalOficinasCentrales
{
    public class tblAF_CtrlAutorizanteAditiva
    {
        public int id { get; set; }
        public int idRow { get; set; }
        public int idAutorizante { get; set; }
        public bool registroActivo { get; set; }

        public virtual tblP_Usuario autorizante { get; set; }
    }
}
