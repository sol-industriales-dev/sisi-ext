using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad
{
    public class tblC_FacturaParcial
    {
        public int id { get; set; }
        public int idCadena { get; set; }
        public int idPrincipal { get; set; }
        public string numProv { get; set; }
        public string factura { get; set; }
        public decimal total { get; set; }
        public decimal abonado { get; set; }
        public DateTime ultimoAbono { get; set; }
    }
}
