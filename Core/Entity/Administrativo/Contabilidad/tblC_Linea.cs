using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad
{
    public class tblC_Linea
    {
        public int id { get; set; }
        public string banco { get; set; }
        public string factoraje { get; set; }
        public int moneda { get; set; }
        public decimal linea { get; set; }
        public DateTime fecha { get; set; }
    }
}
