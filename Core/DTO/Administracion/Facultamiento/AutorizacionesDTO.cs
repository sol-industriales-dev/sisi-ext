using Core.Entity.Administrativo.Facultamiento;
using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Facultamiento
{
    public class AutorizacionesDTO
    {
        public tblFa_CatAutorizacion autorizaciones { get; set; }
        public tblP_Usuario usuario { get; set; }
    }
}
