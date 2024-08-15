using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Cursos
{
    public class tblCU_Asignacion
    {
        public int id { get; set; }
        public int idCurso { get; set; }
        public string claveUsuario { get; set; }
        public bool estado { get; set; }
    }
}
