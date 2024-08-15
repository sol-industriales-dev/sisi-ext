using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad
{
    public class tblEF_TipoMovimientoFlujo
    {
        public int id { get; set; }
        public int tipoMovimiento { get; set; }
        public int conceptoID { get; set; }
        public bool estatus { get; set; }
    }
}
