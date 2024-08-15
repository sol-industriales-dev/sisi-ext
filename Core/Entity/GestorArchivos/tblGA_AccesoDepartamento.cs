using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.GestorArchivos
{
    public class tblGA_AccesoDepartamento
    {
        public int id { get; set; }
        public int usuarioID { get; set; }
        public virtual tblP_Usuario usuario { get; set; }
        public int departamentoID { get; set; }
        public virtual tblP_Departamento departamento { get; set; }
    }
}
