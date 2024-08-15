using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Almacen
{
    public class tblAlm_RelAreaCuentaXAlmacen
    {
        public int id { get; set; }
        public string Asignacion { get; set; }
        public string AreaCuenta { get; set; }
        public string usuarioCreacion { get; set; }
    }
}
