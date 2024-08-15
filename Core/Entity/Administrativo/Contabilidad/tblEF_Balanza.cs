using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad
{
    public class tblEF_Balanza
    {
        public int id { get; set; }
        public int cta { get; set; }
        public int scta { get; set; }
        public int sscta { get; set; }
        public decimal saldoInicial { get; set; }
        public decimal cargos { get; set; }
        public decimal abonos { get; set; }
        public decimal saldoActual { get; set; }
        public int corteMesID { get; set; }
        public bool estatus { get; set; }
    }
}
