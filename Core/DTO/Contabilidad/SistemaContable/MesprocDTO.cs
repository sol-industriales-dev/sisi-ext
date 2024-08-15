using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.SistemaContable
{
    public class MesProcDTO
    {
        public int year { get; set; }
        public int mes { get; set; }
        public string sco { get; set; }
        public string sbo { get; set; }
        public string scp { get; set; }
        public string scx { get; set; }
        public string snd { get; set; }
        public string sin { get; set; }
        public string soc { get; set; }
        public string sfa { get; set; }
        public string scv { get; set; }
        public string sac { get; set; }
        public string st_validacion { get; set; }
        public string st_codigo_agrupador { get; set; }
        public int? usuario_codigo_agrupador { get; set; }
        public string st_estatus_poliza { get; set; }
        public int? usuario_estatus_poliza { get; set; }
        public string st_saldo_mayor { get; set; }
        public int? usuario_saldo_mayor { get; set; }
        public string st_poliza_iva { get; set; }
        public int? usuario_poliza_iva { get; set; }
        public string st_sbo { get; set; }
        public int? usuario_sbo { get; set; }
        public string st_scx { get; set; }
        public int? usuario_sco { get; set; }
        public string st_scp { get; set; }
        public int? usuario_scp { get; set; }
        public string st_saldo_ini { get; set; }
        public int? usuario_saldo_ini { get; set; }
        public string st_tipo_contable { get; set; }
        public int? usuario_tipo_contable { get; set; }
        public Nullable<DateTime> fecha_autoriza { get; set; }
        public string st_cta_cfdi { get; set; }
        public int? usuario_cta_cfdi { get; set; }
    }
}
