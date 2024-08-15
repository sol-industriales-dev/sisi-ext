using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.CuentasPorCobrar
{
    public class tblCXC_Permisos
    {
        public int id { get; set; }
        public int idUsuario { get; set; }
        public bool esAutorizar { get; set; }
        public bool esActivo { get; set; }
    }
}
