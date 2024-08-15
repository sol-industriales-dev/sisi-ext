using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Principal.Menus;
using Core.Entity.Principal.Usuarios;
namespace Core.DTO.Sistemas
{
    public class MenuItemDTO
    {
        public tblP_Menu menu { get; set; }
        public List<tblP_AccionesVista> acciones { get; set; }
        public List<tblP_AccionesVistatblP_Usuario> permisos { get; set; }
    }
}
