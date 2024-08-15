using Core.DAO.Principal.Menus;
using Core.Service.Principal.Menus;
using Data.DAO.Principal.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Principal.Menus
{
    public class MenuFactoryServices
    {
        public IMenuDAO getMenuService()
        {
            return new MenuService(new MenuDAO());
        }
    }
}
