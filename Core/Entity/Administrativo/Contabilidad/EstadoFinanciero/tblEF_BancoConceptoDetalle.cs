using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.EstadoFinanciero
{
    public class tblEF_BancoConceptoDetalle
    {
        public int id { get; set; }
        public int tm { get; set; }
        public int conceptoID { get; set; }
        public bool consolidado { get; set; }
        public bool registroActivo { get; set; }
    }
}
