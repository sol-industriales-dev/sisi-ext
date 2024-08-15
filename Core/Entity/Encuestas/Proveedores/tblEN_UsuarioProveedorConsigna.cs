using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Encuestas.Proveedores
{
    public class tblEN_UsuarioProveedorConsigna
    {
        public int id { get; set; }
        public int usuarioCcId { get; set; }
        public int proveedor { get; set; }
        public bool registroActivo { get; set; }
    }
}
