using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Propuesta.Proveedores
{
    public class tblC_SaldosCondensados
    {
        public int id { get; set; }
        public int idGiro { get; set; }
        public string numpro { get; set; }
        public string cc { get; set; }
        public string factura { get; set; }
        public DateTime fechaFactura { get; set; }
        public DateTime fechaVence { get; set; }
        public int tm { get; set; }
        public decimal total { get; set; }
        public int moneda { get; set; }
        public DateTime fechaPropuesta { get; set; }
        public bool esPropuesta { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaCaptura { get; set; }
    }
}
