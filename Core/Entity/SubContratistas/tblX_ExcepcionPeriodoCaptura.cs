using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.SubContratistas
{
    public class tblX_ExcepcionPeriodoCaptura
    {
        public int id { get; set; }
        public int subcontratistaID { get; set; }
        public bool estatus { get; set; }
    }
}
