using Core.Entity.Principal.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Principal.Usuarios
{
    public class UsuarioExtDTO
    {
        public int id { get; set; }
        public string contrasena { get; set; }
        public string nombreUsuario { get; set; }
        public string nombreCompleto { get; set; }
        public List<tblP_Sistema> sistemas { get; set; }
        public bool tipoSGC { get; set; }
        public string usuarioSGC { get; set; }
    }
}
