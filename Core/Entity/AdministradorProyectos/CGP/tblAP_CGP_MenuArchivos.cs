using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.AdministradorProyectos.CGP
{
    public class tblAP_CGP_MenuArchivos
    {
        public int Id { get; set; }
        public int IdMenu { get; set; }
        public string DirArchivos { get; set; }
        public bool esActivo { get; set; }
    }
}
