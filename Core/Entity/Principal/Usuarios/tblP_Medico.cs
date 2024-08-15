using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Principal.Usuarios
{
    public class tblP_Medico
    {
        public int id { get; set; }
        public int usuario_id { get; set; }
        public string titulo { get; set; }
        public string universidad { get; set; }
        public string cedulaProfesional { get; set; }
        public string cc { get; set; }
        public bool registroActivo { get; set; }
    }
}
