using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.EstadoFinanciero
{
    public class tblEF_ParteRelacionada
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public int tipo { get; set; }
        public int grupo { get; set; }
        public bool registroActivo { get; set; }
    }
}
