using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Facturas
{
    public class tblF_EK_TM
    {
        public int id { get; set; }
        public string clave { get; set; }
        public string descripcion { get; set; }
        public bool esActivo { get; set; }
    }
}
