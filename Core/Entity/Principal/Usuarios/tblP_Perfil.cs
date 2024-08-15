using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Principal.Usuarios
{
    public class tblP_Perfil
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public bool estatus { get; set; }
        public DateTime fechaCreacion { get; set; }
    }
}
