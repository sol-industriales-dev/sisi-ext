using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Cheque
{
    public class tblC_sc_movpol
    {
        public int id { get; set; }
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
        public int iclave { get; set; }
        public int itm { get; set; }
        public string st_par { get; set; }
        public int orden_compra { get; set; }
        public int numpro { get; set; }
        public int socio_inversionista { get; set; }
        public int istm { get; set; }
        public int folio_imp { get; set; }
        public int linea_imp { get; set; }
        public int num_emp { get; set; }
        public int folio_gxc { get; set; }
        public string cfd_ruta_pdf { get; set; }
        public string cfd_ruta_xml { get; set; }
        public string UUID { get; set; }
        public string cfd_rfc { get; set; }
        public decimal cfd_tipocambio { get; set; }
        public decimal cfd_total { get; set; }
        public string cfd_moneda { get; set; }
        public int metodo_pago_sat { get; set; }
        public string ruta_comp_ext { get; set; }
        public string factura_comp_ext { get; set; }
        public string taxid { get; set; }
        public string forma_pago { get; set; }
        public string cfd_fecha_expedicion { get; set; }
        public string cfd_tipocomprobante { get; set; }
        public string cfd_metodo_pago_sat { get; set; }
        public int area { get; set; }
        public int cuenta_oc { get; set; }

        [NotMapped]
        public decimal montoUs { get; set; }
        [NotMapped]
        public string itmPeru { get; set; }
        [NotMapped]
        public string proveedorPeru { get; set; }
        
    }
}
