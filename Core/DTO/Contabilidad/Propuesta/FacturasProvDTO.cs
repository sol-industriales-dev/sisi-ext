using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Propuesta
{
    public class FacturasProvDTO
    {
        public int id { get; set; }
        public int numpro { get; set; }
        public string numproPeru { get; set; }
        public string proveedor { get; set; }
        public string referenciaoc { get; set; }
        public string cc { get; set; }
        public string centroCostos { get; set; }
        public int tm { get; set; }
        public string tmDesc { get; set; }
        public string vence { get; set; }
        public string factura { get; set; }
        public decimal saldo { get; set; }
        public decimal monto_plan { get; set; }
        public string concepto { get; set; }
        public string moneda { get; set; }
        public string autorizado { get; set; }
        public decimal? tipocambio { get; set; }
        public int idGiro { get; set; }

        public bool bloqueado { get; set; }
        public string descripcionBloqueo { get; set; }
        public bool activo_fijo { get; set; }

        public string fechaTimbrado { get; set; }
        public string fechaValidacion { get; set; }

        public string tp { get; set; }
    }
}
