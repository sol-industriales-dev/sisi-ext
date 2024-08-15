using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.DocumentosXPagar.Reportes
{
    public class PropuestaArrendadoraDTO
    {
        public string noEconomico { get; set; }
        public string cc { get; set; }
        public string mensualidad { get; set; }
        public string financiamiento { get; set; }
        public DateTime fechaVencimiento { get; set; }
        public string noContrato { get; set; }
        public string areaCuenta { get; set; }
        public decimal capital { get; set; }
        public decimal ivaIntereses { get; set; }
        public decimal intereses { get; set; }
        public decimal iva { get; set; }
        public decimal importe { get; set; }
        public decimal porcentaje { get; set; }
        public decimal importeDLLS { get; set; }
        public decimal tipoCambio { get; set; }
        public decimal total { get; set; }
        public string rfc { get; set; }

    }
}
