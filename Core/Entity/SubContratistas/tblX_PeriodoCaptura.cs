using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.SubContratistas
{
    public class tblX_PeriodoCaptura
    {
        public int id { get; set; }
        public int anio { get; set; }
        public int mes { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public bool estatus { get; set; }
    }
}
