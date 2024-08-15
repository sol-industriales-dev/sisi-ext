using Core.DAO.Principal.Menus;
using Core.DTO.Principal.Menus;
using Core.DTO.Sistemas;
using Core.Entity.Principal.Menus;
using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Principal.Menus
{
    public class MenuService : IMenuDAO
    {
        #region Atributos
        private IMenuDAO m_menuDAO;
        #endregion

        #region Propiedades
        public IMenuDAO MenuDAO
        {
            get { return m_menuDAO; }
            set { m_menuDAO = value; }
        }
        #endregion

        #region Constructores
        public MenuService(IMenuDAO menuDAO)
        {
            this.MenuDAO = menuDAO;
        }
        #endregion
        public IList<tblP_Menu> getAll()
        {
            return MenuDAO.getAll();
        }

        public void setSistemaActual(int vista)
        {
            MenuDAO.setSistemaActual(vista);
        }
        public List<tblP_SistemaDTO> getSistemas()
        {
            return MenuDAO.getSistemas();
        }
        public IList<MenuDTO> getMenuTreeByCurrectSystem()
        {
            return MenuDAO.getMenuTreeByCurrectSystem();
        }
        public string getMenuTreeByCurrectSystemString()
        {
            return MenuDAO.getMenuTreeByCurrectSystemString();
        }
        public string getMenuTreeByCurrectSystemStringNew()
        {
            return MenuDAO.getMenuTreeByCurrectSystemStringNew();
        }
        public string getBreadCrums()
        {
            return MenuDAO.getBreadCrums();
        }
        public bool checkURLExistence(string url) 
        {
            return MenuDAO.checkURLExistence(url);
        }


        public IList<MenuDTO> getMenuTreeBySystem(int id)
        {
            return MenuDAO.getMenuTreeBySystem(id);
        }


        public List<tblP_AccionesVista> getAccionesVista(int id)
        {
            return MenuDAO.getAccionesVista(id);
        }
        public bool isLiberado(int id)
        {
            return MenuDAO.isLiberado(id);
        }
        public List<int> getVistasExcepcionPalabraCC()
        {
            return MenuDAO.getVistasExcepcionPalabraCC();
        }
        public MenuItemDTO getMenu(int id)
        {
            return MenuDAO.getMenu(id);
        }
        public List<tblP_MenutblP_Usuario> getMenuItemsConPermisos(List<tblP_MenutblP_Usuario> dataTemp)
        {
            return MenuDAO.getMenuItemsConPermisos(dataTemp);
        }
    }
}
