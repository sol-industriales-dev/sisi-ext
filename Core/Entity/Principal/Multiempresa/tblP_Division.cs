
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Principal.Multiempresa
{
    public class tblP_Division
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public string abreviacion { get; set; }
        public bool activo { get; set; }
        public virtual List<tblP_Subdivision> subdivisiones { get; set; }
    }
}
