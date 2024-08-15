using Core.Enum.Multiempresa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Poliza
{
    public class MovpolDTO
    {
        public int year { get; set; }
        public int mes { get; set; }
        public int poliza { get; set; }
        public string tp { get; set; }
        public int linea { get; set; }
        public int cta { get; set; }
        public int scta { get; set; }
        public int sscta { get; set; }
        public int digito { get; set; }
        public int tm { get; set; }
        public string referencia { get; set; }
        public string cc { get; set; }
        public string concepto { get; set; }
        public decimal monto { get; set; }
        public decimal montoUs { get; set; }
        public int iclave { get; set; }
        public int itm { get; set; }
        public string orden_compra { get; set; }
        public int? numpro { get; set; }
        public string forma_pago { get; set; }
        public DateTime fechapol { get; set; }
        public int? area { get; set; }
        public int? cuenta_oc { get; set; }
        public EmpresaEnum empresa { get; set; }
        public DateTime fecha { get; set; }
        public string proveedorPeru { get; set; }
    }
}
