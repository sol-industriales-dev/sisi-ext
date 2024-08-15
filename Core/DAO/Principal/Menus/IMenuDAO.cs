using Core.DTO.Principal.Menus;
using Core.DTO.Sistemas;
using Core.Entity.Principal.Menus;
using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Principal.Menus
{
    public interface IMenuDAO
    {
        IList<tblP_Menu> getAll();
        void setSistemaActual(int vista);
        List<tblP_SistemaDTO> getSistemas();
        IList<MenuDTO> getMenuTreeByCurrectSystem();
        string getMenuTreeByCurrectSystemString();
        string getMenuTreeByCurrectSystemStringNew();
        string getBreadCrums();
        bool checkURLExistence(string url);
        IList<MenuDTO> getMenuTreeBySystem(int id);
        List<tblP_AccionesVista> getAccionesVista(int id);
        bool isLiberado(int id);
        List<int> getVistasExcepcionPalabraCC();
        MenuItemDTO getMenu(int id);
        List<tblP_MenutblP_Usuario> getMenuItemsConPermisos(List<tblP_MenutblP_Usuario> dataTemp);
    }
}
