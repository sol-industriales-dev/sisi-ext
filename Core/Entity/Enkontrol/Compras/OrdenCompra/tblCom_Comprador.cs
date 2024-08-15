using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Enkontrol.Compras.OrdenCompra
{
    public class tblCom_Comprador
    {
        public int id { get; set; }
        public int empleado { get; set; }
        public string descripcion { get; set; }
        public int usuarioSIGOPLAN { get; set; }
        public string cveEmpleado { get; set; }
        public bool estatus { get; set; }
    }
}
