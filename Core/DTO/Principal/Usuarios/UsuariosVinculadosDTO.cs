using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Principal.Usuarios
{
    public class UsuariosVinculadosDTO
    {
        public int principal { get; set; }
        public string principalCorreo { get; set; }
        public int vinculado { get; set; }
        public string vinculadoCorreo { get; set; }
    }
}
