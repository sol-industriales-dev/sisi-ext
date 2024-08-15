using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Cursos
{
    public  class tblCU_Curso
    {
        public int id { get; set; }
        public string nombreCurso { get; set; }
        public string descripcion { get; set; }
        public DateTime fecha { get; set; }
        public DateTime fechaCaptura { get; set; }
        public int usuarioCap { get; set; }
        public string nomUsuarioCap { get; set; }
        public string folio { get; set; }
        public bool editable { get; set; }
        public bool estado { get; set; }
        public bool completo { get; set; }
    }
}
