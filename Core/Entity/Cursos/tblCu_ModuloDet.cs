using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Cursos
{
    public class tblCU_ModuloDet
    {
        public int id { get; set; }
        public int pagina { get; set; }
        public string descripcion { get; set; }
        public string contenido { get; set; }
        public int idModulo { get; set; }
        //estado eliminado
        public  bool estado { get; set; }
    }
}