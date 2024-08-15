using Core.Enum.Contabilidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.EstadoFinanciero
{
    public class tblEF_BancoConcepto
    {
        public int id { get; set; }
        public string concepto { get; set; }
        public TipoOperacionEnum tipoOperacion { get; set; }
        public int ordenReporte { get; set; }
        public int grupoReporteID { get; set; }
        public bool consolidado { get; set; }
        public bool registroActivo { get; set; }
    }
}
