using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Cheque
{
    public class PolizaCheque
    {
        public int verCFDI { get; set; }
        public int no { get; set; }
        public int cuenta { get; set; }
        public int scta { get; set; }
        public string descripcionScta { get; set; }
        public int sscta { get; set; }
        public string  descripcionSscta { get; set; }
        public string d { get; set; }
        public int mov { get; set; }
        public string descripcionMov { get; set; }
        public int subMov { get; set; }
        public string descripcionSubMov { get; set; }
        public int numPro { get; set; }
        public int proveedor { get; set; }
        public int referencia { get; set; }
        public string cc { get; set; }
        public string oc { get; set; }
        public string concepto { get; set; }
        public int tipoMovimiento { get; set; }
        public decimal monto { get; set; }
        public string CargaCFDI { get; set; }
        public string st { get; set; }
        public string uuid { get; set; }
        public string cfdiRFC { get; set; }
        public string metodoPado { get; set; }
        public string facturaComprobanteExtranjero { get; set; }
        public int taxID { get; set; }
    }
}
