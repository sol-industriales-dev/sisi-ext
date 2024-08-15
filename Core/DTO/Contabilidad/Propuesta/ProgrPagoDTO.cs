using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Propuesta
{
    public class ProgrPagoDTO
    {
        public string pdf { get; set; }
        public string xml { get; set; }
        public string proveedorID { get; set; }
        public string proveedor { get; set; }
        public string factura { get; set; }
        public DateTime fecha { get; set; }
        public DateTime vence { get; set; }
        public int tm { get; set; }
        public string cc { get; set; }
        public string oc { get; set; }
        public string concepto { get; set; }
        public decimal saldo { get; set; }
        public string tipoMoneda { get; set; }
        public Nullable<decimal> tipocambio { get; set; }
        public decimal monto { get; set; }
        public int tmb { get; set; }
        public int tmp { get; set; }
        public string tmbDescripcion { get; set; }
        public string tmpDescripcion { get; set; }
        public decimal facturado { get; set; }
        public decimal solicitado { get; set; }
        public decimal pagado { get; set; }
        public decimal recibido { get; set; }
        public decimal maxPago { get; set; }
        public int id { get; set; }
        public string status { get; set; }
        public bool esPagado { get; set; }
        public decimal monto_plan { get; set; }
        public string ac { get; set; }
        public string acDesc { get; set; }
        public bool activo_fijo { get; set; }
        public decimal iva { get; set; }
        public string numproPeru { get; set; }
        public string req { get; set; }
        public string estatus { get; set; }
    }
}