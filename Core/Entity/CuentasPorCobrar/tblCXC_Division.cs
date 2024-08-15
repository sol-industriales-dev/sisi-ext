using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.CuentasPorCobrar
{
    public class tblCXC_Division
    {
        public int id { get; set; }
        public string division { get; set; }
        public bool estatus { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int usuarioCreacion { get; set; }
    }
}
