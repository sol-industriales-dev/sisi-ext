using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.DocumentosXPagar
{
    public class tblAF_DxP_ProgramacionPagos
    {

        public int id { get; set; }
        public int contratoid { get; set; }
        public string rfc { get; set; }
        public string noEconomico { get; set; }
        public string cc { get; set; }
        public string mensualidad { get; set; }
        public string financiamiento { get; set; }
        public DateTime fechaVencimiento { get; set; }
        public string contrato { get; set; }
        [NotMapped]
        public string areaCuenta { get; set; }
        public string ac { get; set; }
        public decimal capital { get; set; }
        public decimal intereses { get; set; }
        public decimal iva { get; set; }
        public decimal importe { get; set; }
        public decimal porcentaje { get; set; }
        public decimal importeDLLS { get; set; }
        public decimal tipoCambio { get; set; }
        public decimal total { get; set; }

        public int aplicado { get; set; }
        public int usuarioCaptura { get; set; }
        public DateTime fechaCaptura { get; set; }
        public int parcialidad { get; set; }
        public decimal ivaInteres { get; set; }
        public int empresa { get; set; }
        public bool aplicaPropuesta { get; set; }
        public int moneda { get; set; }

        public bool liquidar { get; set; }
        public decimal? penaConvencional { get; set; }
        public bool opcionCompra { get; set; }
        public decimal? montoOpcionCompra { get; set; }
        public int? maquinaId { get; set; }
    }
}
