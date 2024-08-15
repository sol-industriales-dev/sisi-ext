using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Nomina
{
    public class tblC_Nom_ColumnaRaya
    {
        public int id { get; set; }
        public int clave { get; set; }
        public string concepto { get; set; }
        public string conceptoGeneral { get; set; }
        public string nombreColumnaUnidades { get; set; }
        public string nombreColumnaImportes { get; set; }
        public string nombreColumnaFechas { get; set; }
        public string tipoColumna { get; set; }
        public bool estatus { get; set; }
    }
}
