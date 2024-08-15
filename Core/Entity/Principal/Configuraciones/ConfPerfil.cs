using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Principal.Configuraciones
{
    public class ConfPerfil
    {
        public int id { get; set; }
        public tblP_Perfil perfil { get; set; }
        public DateTime fechaRegistro { get; set; }
        public bool permisoBitacora { get; set; }
    }
}
