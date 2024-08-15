using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.SubContratistas
{
    public class tblX_Cliente_ContratoPeriodo
    {
        public int id { get; set; }
        public int contratoID { get; set; }
        public int periodoID { get; set; }
        public DateTime fechaCaptura { get; set; }
        public bool actualizacion { get; set; }
        public bool validado { get; set; }
        public bool estatus { get; set; }
    }
}
