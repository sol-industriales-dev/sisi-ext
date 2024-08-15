using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Poliza
{
    public class tblC_SC_CuentasExcepcion
    {
        public int id { get; set; }
        public int cuenta { get; set; }
        public DateTime fechaCaptura { get; set; }
        public int usuarioCaptura { get; set; }
        public DateTime fechaModifica { get; set; }
        public int usuarioModifica { get; set; }
        public bool esActivo { get; set; }
    }
}
