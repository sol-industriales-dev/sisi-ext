using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Inventario
{
    public class tblM_ControlEnvioMaquinaria_CorreosCC
    {
        public int id { get; set; }
        public int usuarioID { get; set; }
        public tblP_Usuario usuario { get; set; }
        public string cc { get; set; }
    }
}
