using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.EstadoFinanciero
{
    public class tblEF_InversionDetalle
    {
        public int id { get; set; }
        public int cta { get; set; }
        public int scta { get; set; }
        public int sscta { get; set; }
        public int operacion { get; set; }
        public int inversionID { get; set; }
        public bool registroActivo { get; set; }
    }
}
