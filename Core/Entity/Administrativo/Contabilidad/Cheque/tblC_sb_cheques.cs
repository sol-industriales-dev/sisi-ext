using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Cheques
{
    public class tblC_sb_cheques
    {
        public int id { get; set; }
        public decimal cuenta { get; set; }
        public DateTime fecha_mov { get; set; }
        public decimal tm { get; set; }
        public decimal numero { get; set; }
        public string tipocheque { get; set; }
        public string descripcion { get; set; }
        public string cc { get; set; }
        public decimal monto { get; set; }
        public string hecha_por { get; set; }
        public int usuariocapturaID { get; set; }
        public string status_bco { get; set; }
        public string status_lp { get; set; }
        public int num_pro_emp { get; set; }
        public string cpto1 { get; set; }
        public string cpto2 { get; set; }
        public string cpto3 { get; set; }
        public int iyear { get; set; }
        public int imes { get; set; }
        public int ipoliza { get; set; }
        public string itp { get; set; }
        public int ilinea { get; set; }
        public string tp { get; set; }
        public DateTime? fecha_reten { get; set; }
        public string desc1 { get; set; }
        public string status_transf_cash { get; set; }
        public int id_empleado_firma { get; set; }
        public int id_empleado_firma2 { get; set; }
        public DateTime? fecha_reten_fin { get; set; }
        public int firma1 { get; set; }
        public DateTime? fecha_firma1 { get; set; }
        public int firma2 { get; set; }
        public DateTime? fecha_firma2 { get; set; }
        public int firma3 { get; set; }
        public DateTime? fecha_firma3 { get; set; }
        public int clave_sub_tm { get; set; }
        public string ruta_comprobantebco_pdf { get; set; }

        public bool anticipo { get; set; }
        public int estatusCheque { get; set; }
    }
}
