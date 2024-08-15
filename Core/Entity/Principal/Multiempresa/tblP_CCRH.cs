using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Principal.Multiempresa
{
    public class tblP_CCRH
    {
        public int id { get; set; }
        public string cc { get; set; }
        public string area { get; set; }
        public string cuenta { get; set; }
        public string areaCuenta { get; set; }
        public string descripcion { get; set; }
        public bool estatus { get; set; }
    }
}