using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Inventario
{
   public  class tblM_DocumentosResguardos
    {
       public int id { get; set; }
        public int ResguardoID { get; set; }
        public string nombreRuta { get; set; }
        public string nombreArchivo { get; set; }
        public int tipoArchivo { get; set; }
        public DateTime fechaSubido { get; set; }
        public int tipoResguardo { get; set; }
    }
}
