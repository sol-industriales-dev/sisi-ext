using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad
{
    public class tblC_CatGiro
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaCaptura { get; set; }
    }
}
