using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.FlujoEfectivo
{
    public class tblC_FED_CcVisto
    {
        public int id { get; set; }
        public int anio { get; set; }
        public int semana { get; set; }
        public string cc { get; set; }
        public bool esVisto { get; set; }
    }
}
