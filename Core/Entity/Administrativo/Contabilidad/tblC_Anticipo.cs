using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad
{
    public class tblC_Anticipo
    {
        public int id { get; set; }
        public string proveedor { get; set; }
        public string numProveedor { get; set; }
        public string numNafin { get; set; }
        public decimal anticipo { get; set; }
        public decimal IVA { get; set; }
        public decimal tipoCambio { get; set; }
        public decimal monto { get; set; }
        public string concepto { get; set; }
        public string factoraje { get; set; }
        public string cif { get; set; }
        public string banco { get; set; }
        public DateTime fecha { get; set; }
        public DateTime fechaVencimiento { get; set; }
        public bool estatus { get; set; }
        public int tipoMoneda { get; set; }
        public string centro_costos { get; set; }
        public string nombCC { get; set; }
    }
}
