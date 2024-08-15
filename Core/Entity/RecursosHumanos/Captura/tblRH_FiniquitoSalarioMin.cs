using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Captura
{
    public class tblRH_FiniquitoSalarioMin
    {
        public int id { get; set; }
        public decimal salarioMinimo { get; set; }
        public DateTime fecha { get; set; }
        public bool estatus { get; set; }
    }
}
