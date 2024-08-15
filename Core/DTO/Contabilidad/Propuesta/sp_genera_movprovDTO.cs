using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Propuesta
{
    public class sp_genera_movprovDTO
    {
        public int id { get; set; }
        public int numpro { get; set; }
        public string factura { get; set; }
        public DateTime fecha { get; set; }
        public DateTime vence { get; set; }
        public int tm { get; set; }
        public decimal monto { get; set; }
        public int tm_bancario { get; set; }
        public int tm_prov { get; set; }
        public DateTime fecha_movto { get; set; }
        public string cc { get; set; }
        public string oc { get; set; }
        public decimal monto_plan { get; set; }
        public Nullable<decimal> tipocambio { get; set; }
        public string status { get; set; }
        public Nullable<int> clave_sub_tm { get; set; }
        public Nullable<int> id_cta_dep { get; set; }
        public Nullable<int> id_batch { get; set; }
        public string concepto { get; set; }
        public string ruta_rec_pdf { get; set; }
        public string ruta_rec_xml { get; set; }
        public string descTm { get; set; }
        public string descTmp { get; set; }
        public string descTmb { get; set; }
        public string proveedor { get; set; }
        public string descStatus { get; set; }
        public string genEstado { get; set; }
        public Nullable<decimal> genMonto { get; set; }
        public string programo { get; set; }
        public string autorizo { get; set; }
        public string ac { get; set; }
        public string acDesc { get; set; }
        public string autorizapago { get; set; }
        public string cuenta { get; set; }
        public string banco { get; set; }
        public bool activo_fijo { get; set; }
        public string areaCuenta { get; set; }
        public decimal iva { get; set; }
        public DateTime fecha_timbrado { get; set; }
        public DateTime fecha_validacion { get; set; }
        public string numproPeru { get; set; }
        public string moneda { get; set; }
    }
}
