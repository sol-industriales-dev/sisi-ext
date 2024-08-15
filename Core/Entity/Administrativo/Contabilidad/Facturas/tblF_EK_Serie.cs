using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Facturas
{
    public class tblF_EK_Serie
    {
        public int id { get; set; }
        public int linea_folios { get; set; }
        public string serie { get; set; }
        public string usoSerie { get; set; }
        public int folio_ini { get; set; }
        public int folio_fin { get; set; }
        public int no_aprobacion { get; set; }
        public int ano_aprobacion { get; set; }
        public int ult_folio { get; set; }
        public int linea_cer { get; set; }
        public decimal porc_iva { get; set; }
        public string vigente { get; set; }
        public int usuario_alta_log { get; set; }
        public DateTime fecha_alta_log { get; set; }
        public int usuario_cambios_log { get; set; }
        public int usuario_vig_log { get; set; }
        public DateTime fecha_vig_log { get; set; }
        public bool esActivo { get; set; }
    }
}
