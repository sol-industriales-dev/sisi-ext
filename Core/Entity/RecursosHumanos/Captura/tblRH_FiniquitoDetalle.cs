using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Captura
{
    public class tblRH_FiniquitoDetalle
    {
        public int id { get; set; }
        public int conceptoID { get; set; }
        public string conceptoInfo { get; set; }
        public decimal operacion1 { get; set; }
        public decimal operacion2 { get; set; }
        public decimal operacion3 { get; set; }
        public decimal operacion4 { get; set; }
        public string conceptoDetalle { get; set; }
        public decimal resultado { get; set; }
        public bool estatus { get; set; }
        public int finiquitoID { get; set; }
    }
}
