using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Cursos
{
    public class tblCU_Modulo
    {
        public int id { get; set; }
        public  string nombreModulo { get; set; }
        public string descripcion { get; set; }
        public int idCurso { get; set; }
        public bool  estado { get; set; }
        public bool completo { get; set; }
    }
}
