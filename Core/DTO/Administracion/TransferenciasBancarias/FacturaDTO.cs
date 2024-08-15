using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.Administracion.TransferenciasBancarias;

namespace Core.DTO.Administracion.TransferenciasBancarias
{
    public class FacturaDTO
    {
        public int factura { get; set; }
        public string estado { get; set; }
        public int tm { get; set; }
        public string cc { get; set; }
        public decimal monto { get; set; }
        public decimal monto_plan { get; set; }
        public string programo { get; set; }
        public string autorizo { get; set; }
        public string proveedor { get; set; }
        public string cuenta { get; set; }
        public string clabe { get; set; }
        public int banco { get; set; }
        public string bancoDesc { get; set; }
        public int numpro { get; set; }
        public string cuentaOrigen { get; set; }
        public int bancoOrigen { get; set; }
        public OperacionEnum operacion { get; set; }
        public string referenciaoc { get; set; }

        public decimal iva { get; set; }
        public string referencia { get; set; }
        public decimal saldo { get; set; }
        public bool chequeGenerado { get; set; }
        public int poliza { get; set; }
        public int cta { get; set; }
        public int scta { get; set; }
        public int sscta { get; set; }
        public string cuentaBanorte { get; set; }
        public string cuentaSantander { get; set; }
        public string fecha_timbrado { get; set; }
        public string fecha_validacion { get; set; }
        public DateTime fecha { get; set; }
    }
}
