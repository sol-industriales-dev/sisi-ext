using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad
{
    public class tblC_CadenaProductiva
    {
        public int id { get; set; }
        public int idPrincipal { get; set; }
        public string proveedor { get; set; }
        public string numProveedor { get; set; }
        public string numNafin { get; set; }
        public decimal saldoFactura { get; set; }
        public decimal IVA { get; set; }
        public decimal tipoCambio { get; set; }
        public decimal monto { get; set; }
        public string concepto { get; set; }
        public string factoraje { get; set; }
        public string cif { get; set; }
        public string banco { get; set; }
        public string factura { get; set; }
        public bool pagado { get; set; }
        public DateTime fecha { get; set; }
        public DateTime fechaVencimiento { get; set; }
        public bool reasignado { get; set; }
        public bool estatus { get; set; }
        public int tipoMoneda { get; set; }
        public string centro_costos { get; set; }
        public string nombCC { get; set; }
        public string area_cuenta { get; set; }
        public string orden_compra { get; set; }
    }
}
