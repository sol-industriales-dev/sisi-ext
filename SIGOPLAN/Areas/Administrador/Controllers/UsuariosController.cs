using Core.DTO;
using Core.DTO.Principal.Menus;
using Core.Entity.Principal.Menus;
using Core.Entity.Principal.Usuarios;
using Data.Factory.Principal.Menus;
using Data.Factory.Principal.Usuarios;
using Infrastructure.Utils;
using Newtonsoft.Json;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Administrador.Controllers
{
    public class UsuariosController : BaseController
    {
        private UsuarioFactoryServices usuarioFactoryServices = new UsuarioFactoryServices();
        private MenuFactoryServices menuFactoryServices = new MenuFactoryServices();
        private static Random random = new Random();
        // GET: Administrador/Usuarios
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult GetUsuarios()
        {
            var result = new Dictionary<string, object>();

            try
            {
                var lstUsuarios = usuarioFactoryServices.getUsuarioService().ListUsersAll().Select(x => new
                {
                    id = x.id.ToString(),
                    Nombre = GlobalUtils.ObtenerNombreCompletoUsuario(x),
                    Clave = x.cveEmpleado ?? "",
                    idEnkontrol = (x.idEnkontrol == 0 ?"":x.idEnkontrol.ToString()),
                    Correo = x.correo ?? "",
                    Usuario = x.nombreUsuario ?? "",
                    Estatus = x.estatus ? "Activo" : "Desactivado"
                });

                result.Add(ITEMS, lstUsuarios);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Clear();
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PerfilUsuario()
        {
            return View();
        }

        public ActionResult GetInfoUsuario(int id = 0)
        {
            object objUsuario = new object();
            if (id != 0)
            {
                objUsuario = usuarioFactoryServices.getUsuarioService().ListUsersById(id).Select(x => new
                {
                    x.id,
                    x.nombre,
                    paterno = x.apellidoPaterno,
                    materno = x.apellidoMaterno,
                    x.correo,
                    usuario = x.nombreUsuario,
                    contrasena = Encriptacion.desencriptar(HttpUtility.UrlDecode(x.contrasena)),
                    empresa = usuarioFactoryServices.getUsuarioService().getUsuarioEmpresa(x.id).Select(e => e.tblP_Empresas_id).ToList(),
                    departamentoID = x.puesto.departamentoID,
                    x.puestoID,
                    x.perfilID,
                    x.cveEmpleado,
                    x.estatus,
                    x.enviar,
                    auditor = x.esAuditor,
                    x.idEnkontrol,
                    gestorSeguridad = x.tipoSeguridad,
                    x.externoPatoos
                }).FirstOrDefault();
            }
            return Json(objUsuario, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetInfoUsuarioUsuarioActivos()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            List<tblP_Usuario> objUsuario = new List<tblP_Usuario>();

            objUsuario = usuarioFactoryServices.getUsuarioService().ListUsersActivos().ToList();

            foreach (var item in objUsuario) 
            {
                item.contrasena = Encriptacion.desencriptar(HttpUtility.UrlDecode(item.contrasena));
            }

            objUsuario = objUsuario.Where(x => x.contrasena.Contains("Proyecto")).ToList();

            foreach (var item in objUsuario)
            {
                var nuevaContrasena = new string(Enumerable.Repeat(chars, 8).Select(s => s[random.Next(s.Length)]).ToArray());
                item.contrasena = nuevaContrasena;
            }
            
            return Json(objUsuario, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getVistas(int sistemas = 0, int idUsuario = 0)
        {

            List<string> objResultSistemas = new List<string>();
            var result = new Dictionary<string, object>();


            var objMenu = menuFactoryServices.getMenuService().getMenuTreeBySystem(sistemas);

            var exist = usuarioFactoryServices.getUsuarioService().getVistasUsuario(idUsuario, 0);

            var objResult = objMenu.Select(x => new
            {
                title = x.text,
                selected = exist.Where(y => y.tblP_Menu_id == x.id).ToList().Count > 0 ? true : false,
                expanded = exist.Where(y => y.tblP_Menu_id == x.id).ToList().Count > 0 ? true : false,
                id = exist.Where(y => y.tblP_Menu_id == x.id).ToList().Count > 0 ? exist.Where(y => y.tblP_Menu_id == x.id).FirstOrDefault().id : 0,
                key = x.id,
                folder = x.children.Count > 0 ? true : false,
                children = x.children.Count > 0 ? this.ReturnChildrens(x.children, idUsuario, exist) : null
            }).ToList();

            result.Add("menuCompleto", objResult);
            result.Add("lstSistemas", objResultSistemas.Distinct());


            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getPermisosVistas(List<tblP_MenutblP_Usuario> lstVistas, int idUsuario = 0)
        {
            List<object> lstResult = new List<object>();

            if (lstVistas != null)
            {
                var temp = menuFactoryServices.getMenuService().getMenuItemsConPermisos(lstVistas);
                foreach (var intVista in temp)
                {
                    var menu = menuFactoryServices.getMenuService().getMenu(intVista.tblP_Menu_id);
                    if (menu.acciones.Count > 0)
                    {
                        var objTree = new
                        {
                            title = menu.menu.descripcion,
                            expanded = true,
                            key = intVista,
                            folder = menu.acciones.Count > 0 ? true : false,
                            children = menu.acciones.Select(x => new
                            {
                                title = x.Accion,
                                selected = menu.permisos.Where(y => y.tblP_AccionesVista_id == x.id).Count() > 0 ? true : false,
                                id = menu.permisos.Where(y => y.tblP_AccionesVista_id == x.id).ToList().Count > 0 ? menu.permisos.Where(y => y.tblP_AccionesVista_id == x.id).FirstOrDefault().id : 0,
                                key = x.id,
                                folder = false
                            }).ToList()
                        };
                        lstResult.Add(objTree);
                    }
                }
            }
            return Json(lstResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getCCsUsuario(int id)
        {
            var lstList = usuarioFactoryServices.getUsuarioService().getCCsUsuario(id).Select(x => x.cc);

            return Json(lstList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getUsuariosPermisos()
        {
            var result = new Dictionary<string, object>();

            try
            {
                var usuarioObj = getUsuario();
                bool Autorizador = false;
                var usuario = usuarioFactoryServices.getUsuarioService().ListUsersById(usuarioObj.id).FirstOrDefault();
                if (usuario != null)
                {
                    Autorizador = usuario.usuarioAuditor;
                }

                result.Add("Autorizador", Autorizador);
                // result.Add(ITEMS, lstUsuarios);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }


        public ActionResult SaveUsuario(tblP_Usuario usuario, List<tblP_MenutblP_Usuario> permisos, List<string> ccs, List<tblP_AccionesVistatblP_Usuario> accVistas, int sistema, List<int> empresa)
        {
            bool result = true;
            try
            {

                usuario.usuarioSGC = "visitante";
                usuario.tipoSGC = false;
                usuario.empresa = string.Empty;
                var objResult = usuarioFactoryServices.getUsuarioService().SaveUsuario(usuario);
                List<tblP_MenutblP_Usuario> lstPermisos = new List<tblP_MenutblP_Usuario>();
                List<tblP_AccionesVistatblP_Usuario> lstAcciones = new List<tblP_AccionesVistatblP_Usuario>();
                if (permisos != null)
                {
                    //var lstPermisosFiltrados = permisos.GroupBy(x => x.tblP_Menu_id).Select(g => g.First()).ToList();
                    //foreach (tblP_MenutblP_Usuario i in lstPermisosFiltrados)
                    //{
                    //    i.tblP_Usuario_id = objResult.id;
                    //    lstPermisos.Add(i);
                    //}
                    permisos.ForEach(x => x.tblP_Usuario_id = objResult.id);

                    usuarioFactoryServices.getUsuarioService().DeletePermisos(sistema, objResult.id, permisos, lstAcciones);
                    usuarioFactoryServices.getUsuarioService().SavePermisos(permisos);
                }
                if (accVistas != null)
                {
                    accVistas.ForEach(x => x.tblP_Usuario_id = objResult.id);

                    usuarioFactoryServices.getUsuarioService().SavePermisosVista(accVistas);
                }


                // Se actualizan los cc
                if (ccs == null)
                {
                    ccs = new List<string>();
                }
                var ccsIDs = ccs.Select(x => Convert.ToInt32(x)).ToList();
                usuarioFactoryServices.getUsuarioService().ActualizarCCsUsuario(ccsIDs, objResult.id);

                if (empresa != null)
                {
                    usuarioFactoryServices.getUsuarioService().DeleteUsuarioEmpresa(objResult.id);
                    usuarioFactoryServices.getUsuarioService().saveUsuarioEmpresa(objResult.id, empresa);
                }
            }
            catch (Exception e)
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveUsuarioNuevaFuncion(tblP_Usuario usuario, List<tblP_MenutblP_Usuario> permisos, List<string> ccs, List<tblP_AccionesVistatblP_Usuario> accVistas, int sistema, List<int> empresa)
        {
            return Json(usuarioFactoryServices.getUsuarioService().GuardarUsuario(usuario, permisos, ccs, accVistas, sistema, empresa), JsonRequestBehavior.AllowGet);
        }

        public ActionResult saveUsuarioEnkontrol(tblP_Usuario_Enkontrol usuario)
        {
            var result = new Dictionary<string, object>();
            try
            {
                usuario.idUsuario = vSesiones.sesionUsuarioDTO.id;
                result.Add(SUCCESS, usuarioFactoryServices.getUsuarioService().saveUsuarioEnkontrol(usuario));
            }
            catch (Exception e)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, e.Message);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        private List<object> ReturnChildrens(List<MenuDTO> lstChildrens, int idUsuario, List<tblP_MenutblP_Usuario> exist)
        {
            var objResult = lstChildrens.Select(x => new
            {
                title = x.text,
                selected = exist.Where(y => y.tblP_Menu_id == x.id).ToList().Count > 0 ? true : false,
                expanded = exist.Where(y => y.tblP_Menu_id == x.id).ToList().Count > 0 ? true : false,
                id = exist.Where(y => y.tblP_Menu_id == x.id).ToList().Count > 0 ? exist.Where(y => y.tblP_Menu_id == x.id).FirstOrDefault().id : 0,
                key = x.id,
                folder = x.children.Count > 0 ? true : false,
                children = this.ReturnChildrens(x.children, idUsuario, exist)
            }).ToList();

            List<object> lstobj = new List<object>();

            foreach (var aux in objResult)
            {
                lstobj.Add(aux);
            }

            return lstobj;

        }
        public ActionResult getLstEmpresasActivas()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, usuarioFactoryServices.getUsuarioService().getLstEmpresasActivas().Select(d => new
                {
                    Text = d.nombre,
                    Value = d.id
                }));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getLstPerfilActivo()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, usuarioFactoryServices.getUsuarioService().getLstPerfilActivo().Select(d => new
                {
                    Text = d.nombre,
                    Value = d.id
                }));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getLstDept()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, usuarioFactoryServices.getUsuarioService().getLstDept().Select(d => new
                {
                    Text = d.descripcion,
                    Value = d.id
                }));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getLstPuesto(int idDept)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, usuarioFactoryServices.getUsuarioService().getLstPuesto(idDept).Select(d => new
                {
                    Text = d.descripcion,
                    Value = d.id
                }));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getAllPuesto()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, usuarioFactoryServices.getUsuarioService().getAllPuesto().Select(d => new
                {
                    Text = d.descripcion,
                    Value = d.id
                }));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getIdCCsUsuario(int id)
        {
            return Json(usuarioFactoryServices.getUsuarioService().getIdCCsUsuario(id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ObtenerIDsCCsUsuarioAutoriza(int id)
        {
            return Json(usuarioFactoryServices.getUsuarioService().ObtenerIDsCCsUsuarioAutoriza(id), JsonRequestBehavior.AllowGet);
        }

        public ViewResult FormularioDatosEnKontrol()
        {
            return View();
        }

        [HttpGet]
        public ActionResult NecesitaIngresarDatosEnKontrol()
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                bool noNecesitaVerificar = usuarioFactoryServices.getUsuarioService().NecesitaIngresarDatosEnKontrol() == false;
                resultado.Add("noNecesitaVerificar", noNecesitaVerificar);
                resultado.Add("claveEmpleado", vSesiones.sesionUsuarioDTO.cveEmpleado);
                resultado.Add("nombreCompleto", GlobalUtils.ObtenerNombreCompletoUsuarioActual());
                resultado.Add("redireccionURl", "/SISTEMA/SISTEMA");
                resultado.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ValidarDatosUsuarioEnKontrol(string password, int claveEmpleado)
        {
            return Json(usuarioFactoryServices.getUsuarioService().ValidarDatosUsuarioEnKontrol(password, claveEmpleado), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetUsuarioActivo()
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                resultado.Add("usuarioActivo", getUsuario().id);
                resultado.Add(SUCCESS, true);

            }
            catch (Exception)
            {

                resultado.Add(SUCCESS, false);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
    }
}