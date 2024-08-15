using Core.DAO.Principal.Menus;
using Core.DTO;
using Core.DTO.Principal.Menus;
using Core.DTO.Sistemas;
using Core.Entity.Principal.Menus;
using Core.Entity.Principal.Usuarios;
using Core.Enum.Multiempresa;
using Core.Enum.Principal.Menus;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Principal.Menus
{
    public class MenuDAO : GenericDAO<tblP_Menu>, IMenuDAO
    {
        public IList<tblP_Menu> getAll()
        {
            var result = _context.tblP_Menu.Where(x => x.visible).ToList();
            foreach (var i in result)
            {

            }
            return result;
        }
        public IList<MenuDTO> getMenuTreeByCurrectSystem()
        {
            List<MenuDTO> result = new List<MenuDTO>();
            var _cs = vSesiones.sesionSistemaActual;
            var _user = vSesiones.sesionUsuarioDTO;

            int maxnivel = 0;
            var temp = _user.permisos.Where(x => x.visible == true && x.sistemaID == _cs);
            if (temp.Count() > 0)
            {
                maxnivel = temp == null ? 0 : temp.Max(x => x.nivel);
                for (int i = 1; i <= maxnivel; i++)
                {
                    var items = new List<tblP_Menu>();
                    items = _user.permisos.Where(x => x.nivel == i && x.visible == true && x.sistemaID == _cs).OrderBy(x => x.orden).ToList();


                    foreach (var item in items.OrderBy(x => x.nivel))
                    {

                        var tmi = new MenuDTO();
                        tmi.id = item.id;
                        tmi.padre = (int)item.padre;
                        tmi.nivel = (int)item.nivel;
                        tmi.orden = (int)item.orden;
                        tmi.text = item.descripcion;
                        tmi.state = "";
                        tmi.tipo = (int)item.tipo;
                        tmi.url = item.url;
                        tmi.icono = item.icono;
                        tmi.iconoFont = item.iconoFont;
                        tmi.activo = item.activo;
                        tmi.children = new List<MenuDTO>();
                        if (tmi.nivel == 1)
                        {
                            result.Add(tmi);
                        }
                        else
                        {
                            foreach (var item0 in result)
                            {
                                if (item0.id == item.padre)
                                {
                                    item0.children.Add(tmi);
                                }
                                else
                                {
                                    foreach (var item1 in item0.children)
                                    {
                                        if (item1.id == item.padre)
                                        {
                                            item1.children.Add(tmi);
                                        }
                                        else
                                        {
                                            foreach (var item2 in item1.children)
                                            {
                                                if (item2.id == item.padre)
                                                {
                                                    item2.children.Add(tmi);
                                                }
                                                else
                                                {
                                                    foreach (var item3 in item2.children)
                                                    {
                                                        if (item3.id == item.padre)
                                                        {
                                                            item3.children.Add(tmi);
                                                        }
                                                        else
                                                        {
                                                            foreach (var item4 in item3.children)
                                                            {
                                                                if (item4.id == item.padre)
                                                                {
                                                                    item4.children.Add(tmi);
                                                                }
                                                                else
                                                                {
                                                                    foreach (var item5 in item4.children)
                                                                    {
                                                                        if (item5.id == item.padre)
                                                                        {
                                                                            item5.children.Add(tmi);
                                                                        }
                                                                        else
                                                                        {
                                                                            foreach (var item6 in item5.children)
                                                                            {
                                                                                if (item6.id == item.padre)
                                                                                {
                                                                                    item6.children.Add(tmi);
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }
                }
            }

            var menusBloqueado = _context.tblP_MenuUsuarioBloqueado.Where(x => x.usuarioId == vSesiones.sesionUsuarioDTO.id && x.registroActivo).Select(x => x.menuId).ToList();
            result.RemoveAll(x => menusBloqueado.Contains(x.id));

            foreach (var item in result)
            {
                item.children.RemoveAll(x => menusBloqueado.Contains(x.id));
            }

            return result;
        }

        public string getMenuTreeByCurrectSystemString()
        {
            IList<MenuDTO> menu = getMenuTreeByCurrectSystem();
            string ms = "";

            foreach (var item in menu.ToList())
            {
                if (item.tipo == (int)TipoMenuEnum.VISTA)
                {
                    ms += @"<li class='child " + (!item.activo ? "disabled" : "") + "'><a href='" + item.url + "' data-MenuID='" + item.id + @"' data-Tipo='" + item.tipo + @"'><span class='icono'><img class='icono' src='" + item.icono + "'></span>" + item.text + @" </a></li>";
                }
                else if (item.tipo == (int)TipoMenuEnum.SUBMENU)
                {
                    ms += @"<li class='parent" + (!item.activo ? "disabled" : "") + @"'>
                             <a href='#'  data-MenuID='" + item.id + @"' data-Tipo='" + item.tipo + @"'><span class='icono'><img class='icono' src='" + item.icono + "'></span>" + item.text + @" <span class='caret'></span></a>
                             <ul class='nav-pills nav-stacked parent'>";
                    ms += getSubMenuString(item.children);
                    ms += @" </ul>
                           </li>";
                }
                else if (item.tipo == (int)TipoMenuEnum.EXTERNO)
                {
                    ms += @"<li class='child " + (!item.activo ? "disabled" : "") + "'><a href='" + item.url + "' data-MenuID='" + item.id + @"' data-Tipo='" + item.tipo + @"'><span class='icono'><img class='icono' src='" + item.icono + "'></span>" + item.text + @" </a></li>";
                }
            }
            return ms;
        }
        public string getSubMenuString(List<MenuDTO> menu)
        {
            string html = @"";
            foreach (var i in menu)
            {
                if (i.tipo == (int)TipoMenuEnum.VISTA)
                {
                    html += @"<li class='child " + (!i.activo ? "disabled" : "") + "'><a href='" + i.url + "' data-MenuID='" + i.id + @"' data-Tipo='" + i.tipo + @"'><span class='icono'><img class='icono' src='" + i.icono + "'></span>" + i.text + @" </a></li>";
                }
                else if (i.tipo == (int)TipoMenuEnum.SUBMENU)
                {
                    html += @"<li class='parent" + (!i.activo ? "disabled" : "") + @"'>
                             <a href='#' class=' " + (!i.activo ? "disabled" : "") + "' data-MenuID='" + i.id + @"' data-Tipo='" + i.tipo + @"'><span class='icono'><img class='icono' src='" + i.icono + "'></span>" + i.text + @" <span class='caret'></span></a>
                             <ul class='nav-pills nav-stacked'>";
                    html += getSubMenuString(i.children);
                    html += @" </ul>
                           </li>";
                }
                else if (i.tipo == (int)TipoMenuEnum.EXTERNO)
                {
                    html += @"<li class='child " + (!i.activo ? "disabled" : "") + "'><a href='" + i.url + "' data-MenuID='" + i.id + @"' data-Tipo='" + i.tipo + @"'><span class='icono'><img class='icono' src='" + i.icono + "'></span>" + i.text + @" </a></li>";
                }
            }
            return html;
        }
//        public string getMenuTreeByCurrectSystemStringNew()
//        {
//            IList<MenuDTO> menu = getMenuTreeByCurrectSystem();
//            string ms = "";

//            foreach (var item in menu.ToList())
//            {
//                if (item.tipo == (int)TipoMenuEnum.VISTA)
//                {
//                    ms += @"<li class='dropdown " + (!item.activo ? "disabled" : "") + " menu-border'><a href='" + item.url + "' data-MenuID='" + item.id + @"' data-Tipo='" + item.tipo + @"' class='dropdown-toggle' data-toggle='dropdown' role='button' aria-haspopup='true' aria-expanded='false'><i class='" + item.iconoFont + "' aria-hidden='true' ></i> " + item.text + @" </a></li>";
//                }
//                else if (item.tipo == (int)TipoMenuEnum.SUBMENU)
//                {
//                    ms += @"<li class='dropdown " + (!item.activo ? "disabled" : "") + @" menu-border'>
//                             <a href='#' data-MenuID='" + item.id + @"' data-Tipo='" + item.tipo + @"' class='dropdown-toggle' data-toggle='dropdown' role='button' aria-haspopup='true' aria-expanded='false'><i class='" + item.iconoFont + "' aria-hidden='true' ></i> " + item.text + @" <span class='caret'></span></a>
//                             <ul class='dropdown-menu multi-level'>";
//                    ms += getSubMenuStringNew(item.children);
//                    ms += @" </ul>
//                           </li>";
//                }
//                else if (item.tipo == (int)TipoMenuEnum.EXTERNO)
//                {
//                    ms += @"<li class='dropdown " + (!item.activo ? "disabled" : "") + "'><a href='" + item.url + "' data-MenuID='" + item.id + @"' data-Tipo='" + item.tipo + @"' class='dropdown-toggle' data-toggle='dropdown' role='button' aria-haspopup='true' aria-expanded='false'><i class='" + item.iconoFont + "' aria-hidden='true' ></i> " + item.text + @" </a></li>";
//                }
//            }
//            return ms;
//        }
//        public string getSubMenuStringNew(List<MenuDTO> menu)
//        {
//            string html = @"";
//            foreach (var i in menu)
//            {
//                if (i.tipo == (int)TipoMenuEnum.VISTA)
//                {
//                    html += @"<li class='menuOption " + (!i.activo ? "disabled" : "") + "'><a href='" + i.url + "' data-MenuID='" + i.id + @"' data-Tipo='" + i.tipo + @"'>" + i.text + @" </a></li>";
//                }
//                else if (i.tipo == (int)TipoMenuEnum.SUBMENU)
//                {
//                    html += @"<li class='dropdown-submenu " + (!i.activo ? "disabled" : "") + @"'>
//                             <a href='#' data-MenuID='" + i.id + @"' data-Tipo='" + i.tipo + @"' class='dropdown-toggle' data-toggle='dropdown' role='button' aria-haspopup='true' aria-expanded='false'>" + i.text + @" </a>
//                             <ul class='dropdown-menu'>";
//                    html += getSubMenuStringNew(i.children);
//                    html += @" </ul>
//                           </li>";
//                }
//                else if (i.tipo == (int)TipoMenuEnum.EXTERNO)
//                {
//                    html += @"<li class='menuOption " + (!i.activo ? "disabled" : "") + "'><a href='" + i.url + "' data-MenuID='" + i.id + @"' data-Tipo='" + i.tipo + @"'>" + i.text + @" </a></li>";
//                }
//            }
//            return html;
//        }
        public string getMenuTreeByCurrectSystemStringNew()
        {
            IList<MenuDTO> menu = getMenuTreeByCurrectSystem();
            StringBuilder ms = new StringBuilder();

            foreach (var item in menu)
            {
                if (item.tipo == (int)TipoMenuEnum.VISTA)
                {
                    ms.Append(string.Format("<li class='dropdown {0} menu-border'><a href='{1}' data-MenuID='{2}' data-Tipo='{3}' class='dropdown-toggle' data-toggle='dropdown' role='button' aria-haspopup='true' aria-expanded='false'><i class='{4}' aria-hidden='true' ></i> {5} </a></li>", (!item.activo ? "disabled" : ""), item.url, item.id, item.tipo, item.iconoFont, item.text));
                }
                else if (item.tipo == (int)TipoMenuEnum.SUBMENU)
                {
                    ms.Append(string.Format("<li class='dropdown {0} menu-border'><a href='#' data-MenuID='{1}' data-Tipo='{2}' class='dropdown-toggle' data-toggle='dropdown' role='button' aria-haspopup='true' aria-expanded='false'><i class='{3}' aria-hidden='true' ></i> {4} <span class='caret'></span></a><ul class='dropdown-menu multi-level'>", (!item.activo ? "disabled" : ""), item.id, item.tipo, item.iconoFont, item.text));
                    ms.Append(getSubMenuStringNew(item.children));
                    ms.Append(" </ul></li>");
                }
                else if (item.tipo == (int)TipoMenuEnum.EXTERNO)
                {
                    ms.Append(string.Format("<li class='dropdown {0}'><a href='{1}' data-MenuID='{2}' data-Tipo='{3}' class='dropdown-toggle' data-toggle='dropdown' role='button' aria-haspopup='true' aria-expanded='false'><i class='{4}' aria-hidden='true' ></i> {5} </a></li>", (!item.activo ? "disabled" : ""), item.url, item.id, item.tipo, item.iconoFont, item.text));
                }
            }
            return ms.ToString();
        }

        public string getSubMenuStringNew(List<MenuDTO> menu)
        {
            StringBuilder html = new StringBuilder();
            foreach (var i in menu)
            {
                if (i.tipo == (int)TipoMenuEnum.VISTA || i.tipo == (int)TipoMenuEnum.EXTERNO)
                {
                    html.Append(string.Format("<li class='menuOption {0}'><a href='{1}' data-MenuID='{2}' data-Tipo='{3}'>{4} </a></li>", (!i.activo ? "disabled" : ""), i.url, i.id, i.tipo, i.text));
                }
                else if (i.tipo == (int)TipoMenuEnum.SUBMENU)
                {
                    html.Append(string.Format("<li class='dropdown-submenu {0}'><a href='#' data-MenuID='{1}' data-Tipo='{2}' class='dropdown-toggle' data-toggle='dropdown' role='button' aria-haspopup='true' aria-expanded='false'>{3} </a><ul class='dropdown-menu'>", (!i.activo ? "disabled" : ""), i.id, i.tipo, i.text));
                    html.Append(getSubMenuStringNew(i.children));
                    html.Append(" </ul></li>");
                }
            }
            return html.ToString();
        }
        public string getBreadCrums()
        {
            var result = "";
            int? id = null;
            try
            {
                id = int.Parse(vSesiones.sesionCurrentMenu);
            }
            catch (Exception e)
            {
                id = null;
            }
            if (id != null)
            {
                var _user = vSesiones.sesionUsuarioDTO;
                var menu = _user.permisos;
                var p = menu.FirstOrDefault(x => x.id == id);
                var parent = menu.FirstOrDefault(x => x.id == p.padre);

                if (parent != null)
                {
                    result = "<li><span style='cursor:default;'>" + _context.tblP_Sistema.FirstOrDefault(x => x.id == vSesiones.sesionSistemaActual).nombre + "</span></li>" + recBreadCrums(parent) + "<li><span style='cursor:default;'>" + parent.descripcion + "</span></li>";
                }
                else
                {
                    result = "";
                }
            }

            return result;
        }

        public string recBreadCrums(tblP_Menu p)
        {
            var result = "";
            var _user = vSesiones.sesionUsuarioDTO;
            var menu = _user.permisos;
            var parent = menu.FirstOrDefault(x => x.id == p.padre);

            if (parent != null)
            {
                result = recBreadCrums(parent) + "<li><span style='cursor:default;'>" + parent.descripcion + "</span></li>";
            }
            else
            {
                result = "";
            }
            return result;
        }

        public bool checkURLExistence(string url)
        {
            return _context.tblP_Menu.Any(x => x.tipo == 2 && x.url.ToUpper().Equals(url.ToUpper()));
        }

        public IList<MenuDTO> getMenuTreeBySystem(int id)
        {
            List<MenuDTO> result = new List<MenuDTO>();
            var _cs = id;
            var _user = vSesiones.sesionUsuarioDTO;
            if (_cs == 0 || _user == null)
            {

            }
            int maxnivel = 0;
            var temp = _user.permisos.Where(x => x.visible == true && x.sistemaID == _cs);
            if (temp.Count() > 0)
            {
                maxnivel = temp == null ? 0 : temp.Max(x => x.nivel);
                for (int i = 1; i <= maxnivel; i++)
                {
                    var items = new List<tblP_Menu>();
                    items = _user.permisos.Where(x => x.nivel == i && x.visible == true && x.sistemaID == _cs).OrderBy(x => x.orden).ToList();


                    foreach (var item in items.OrderBy(x => x.nivel))
                    {

                        var tmi = new MenuDTO();
                        tmi.id = item.id;
                        tmi.padre = (int)item.padre;
                        tmi.nivel = (int)item.nivel;
                        tmi.orden = (int)item.orden;
                        tmi.text = item.descripcion;
                        tmi.state = "";
                        tmi.tipo = (int)item.tipo;
                        tmi.url = item.url;
                        tmi.icono = item.icono;
                        tmi.iconoFont = item.iconoFont;
                        tmi.activo = item.activo;
                        tmi.children = new List<MenuDTO>();
                        if (tmi.nivel == 1)
                        {
                            result.Add(tmi);
                        }
                        else
                        {
                            foreach (var item0 in result)
                            {
                                if (item0.id == item.padre)
                                {
                                    item0.children.Add(tmi);
                                }
                                else
                                {
                                    foreach (var item1 in item0.children)
                                    {
                                        if (item1.id == item.padre)
                                        {
                                            item1.children.Add(tmi);
                                        }
                                        else
                                        {
                                            foreach (var item2 in item1.children)
                                            {
                                                if (item2.id == item.padre)
                                                {
                                                    item2.children.Add(tmi);
                                                }
                                                else
                                                {
                                                    foreach (var item3 in item2.children)
                                                    {
                                                        if (item3.id == item.padre)
                                                        {
                                                            item3.children.Add(tmi);
                                                        }
                                                        else
                                                        {
                                                            foreach (var item4 in item3.children)
                                                            {
                                                                if (item4.id == item.padre)
                                                                {
                                                                    item4.children.Add(tmi);
                                                                }
                                                                else
                                                                {
                                                                    foreach (var item5 in item4.children)
                                                                    {
                                                                        if (item5.id == item.padre)
                                                                        {
                                                                            item5.children.Add(tmi);
                                                                        }
                                                                        else
                                                                        {
                                                                            foreach (var item6 in item5.children)
                                                                            {
                                                                                if (item6.id == item.padre)
                                                                                {
                                                                                    item6.children.Add(tmi);
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }
                }
            }
            return result;
        }
        public List<tblP_AccionesVista> getAccionesVista(int id)
        {
            return _context.tblP_AccionesVista.Where(x => x.vistaID == id).ToList();
        }
        public bool isLiberado(int id)
        {
            try
            {
                if(id.Equals(0))
                    return false;
                return _context.tblP_Menu.FirstOrDefault(w => w.id == id).liberado;
            }
            catch(Exception)
            {
                return false;
            }
        }
        public List<tblP_SistemaDTO> getSistemas()
        {
            var lista = new List<tblP_SistemaDTO>();
            var sistemas = _context.tblP_Sistema.ToList();
            foreach (var i in sistemas)
            {
                var o = new tblP_SistemaDTO();
                o.id = i.id;
                o.nombre = i.nombre;
                o.icono = i.icono;
                o.url = i.url;
                o.estatus = i.estatus;
                o.activo = i.activo;
                o.general = i.general;
                o.ext = i.ext;
                o.esVirtual = i.esVirtual;
                lista.Add(o);
            }
            return lista;
        }

        public void setSistemaActual(int vista)
        {
            var v = _context.tblP_Menu.FirstOrDefault(x=>x.id == vista);
            vSesiones.sesionSistemaActual = v.sistemaID;
        }
        public List<int> getVistasExcepcionPalabraCC()
        {
            try
            {
                var r = _context.tblP_VistasExcepcionPalabraCC.Select(x => x.vistaID).ToList();
                return r;
            }
            catch(Exception o_O)
            {
                return new List<int>();
            }
        }
        public MenuItemDTO getMenu(int id)
        {
            var data = new MenuItemDTO();
            var acciones = _context.tblP_AccionesVista.Where(x => x.vistaID == id).ToList();
            var menu = _context.tblP_Menu.FirstOrDefault(x => x.id == id);
            var permisos = _context.tblP_AccionesVistatblP_Usuario.Where(x => x.tblP_Usuario_id == vSesiones.sesionUsuarioDTO.id).ToList();
            data.menu = menu;
            data.acciones = acciones;
            data.permisos = permisos;
            
            return data;
        }
        public List<tblP_MenutblP_Usuario> getMenuItemsConPermisos(List<tblP_MenutblP_Usuario> dataTemp) {
            var data = new List<tblP_MenutblP_Usuario>();
            List<int> ids = dataTemp.Select(x=>x.tblP_Menu_id).Distinct().ToList();
            List<int> ids_permisos = _context.tblP_AccionesVista.Where(x=> ids.Contains(x.vistaID)).Select(x=>x.vistaID).Distinct().ToList();
            data = dataTemp.Where(x => ids_permisos.Contains(x.tblP_Menu_id)).Distinct().ToList();
            return data;
        }
    }
}
