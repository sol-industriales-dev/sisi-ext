using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Principal.Usuarios
{
    public class tblP_Usuario_Starsoft_Compradores
    {
        public int id { get; set; }
        public int sigoplan_usuario_id { get; set; }
        public string starsoft_usuario_id { get; set; }
        public bool registroActivo { get; set; }
    }
}
