using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Facturas
{
    public class tblF_EK_FormaPagoSat
    {
        public int id { get; set; }
        public string clave_sat { get; set; }
        public string descripcion_sat { get; set; }
        public string Bancarizado { get; set; }
        public string NumOperacion { get; set; }
        public string RFCEmisorOrdenante { get; set; }
        public string CuentaOrdenante { get; set; }
        public string PatronCtaordenante { get; set; }
        public string RFCEmisorBeneficiario { get; set; }
        public string CuentaBeneficiario { get; set; }
        public string PatronCtaBeneficiario { get; set; }
        public string TipoCadenaPago { get; set; }
        public string BancoExtranjero { get; set; }
        public string patron_cta_ordenante { get; set; }
        public string patron_cta_beneficiario { get; set; }
        public int id_enkontrol { get; set; }
    }
}
