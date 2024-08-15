using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Catalogo
{
    public class tblRH_FiniquitoConceptos
    {
        public int id { get; set; }
        public string concepto { get; set; }
        public string detalle { get; set; }
        public bool operador { get; set; }
        public bool estatus { get; set; }
    }
}
