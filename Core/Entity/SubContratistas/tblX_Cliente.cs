using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.SubContratistas
{
    public class tblX_Cliente
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string direccion { get; set; }
        public string nombreCorto { get; set; }
        public string codigoPostal { get; set; }
        public string rfc { get; set; }
        public string correo { get; set; }
        public bool fisica { get; set; }
        public bool estatus { get; set; }
    }
}
