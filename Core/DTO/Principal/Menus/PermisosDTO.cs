using Core.Entity.Principal.Menus;
using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Principal.Menus
{
    public class PermisosDTO
    {
        public tblP_Menu menu { get; set; }
        public List<tblP_AccionesVista> accion { get; set; }
    }
}
