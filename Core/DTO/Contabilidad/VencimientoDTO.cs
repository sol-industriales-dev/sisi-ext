using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad
{
    public class VencimientoDTO
    {
        public int id { get; set; }
        public int idProveedor { get; set; }
        public string proveedor { get; set; }
        public int factura { get; set; }
        public DateTime fecha { get; set; }
        public DateTime fechaVencimiento { get; set; }
        public DateTime fechaTimbrado { get; set; }
        public string fechaS { get; set; }
        public string fechaVencimientoS { get; set; }
        public string fechaTimbradoS { get; set; }
        public string centro_costos { get; set; }
        public string area_cuenta { get; set; }
        public string orden_compra { get; set; }
        public decimal monto { get; set; }
        public decimal IVA { get; set; }
        public decimal tipoCambio { get; set; }
        public string concepto { get; set; }
        public int numProveedor { get; set; }
        public decimal total { get; set; }
        public string saldoFactura { get; set; }
        public string nombCC { get; set; }
        public int tipoMoneda { get; set; }
        public string factoraje { get; set; }
        public int? cif { get; set; }
        public string banco { get; set; }
        public string numNafin { get; set; }
        public bool pagado { get; set; }
        public decimal abono { get; set; }
        public decimal diff { get; set; }

        public bool bloqueado { get; set; }
        public string descripcionBloqueo { get; set; }
    }
}
