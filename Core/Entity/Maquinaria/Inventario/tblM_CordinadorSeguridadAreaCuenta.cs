using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Inventario
{
    public class tblM_CordinadorSeguridadAreaCuenta
    {
        public int id { get; set; }
        public string areaCuenta { get; set; }
        public int usuarioId { get; set; }
        public bool registroActivo { get; set; }

        public virtual tblP_Usuario usuario { get; set; }
    }
}
