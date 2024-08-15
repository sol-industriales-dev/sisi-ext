using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Principal.Multiempresa
{
    public class tblP_Subdivision
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public string abreviacion { get; set; }
        public int divisionID { get; set; }
        public bool activo { get; set; }
        public virtual tblP_Division division { get; set; }
    }
}