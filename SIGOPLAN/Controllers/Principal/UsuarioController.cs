using Core.DAO.Principal.Alertas;
using Core.DTO;
using Core.DTO.Principal.Usuarios;
using Core.Entity.Principal.Usuarios;
using Core.Enum.Principal;
using Data.Factory.Maquinaria.Catalogos;
using Data.Factory.Principal.Alertas;
using Data.Factory.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;


namespace SIGOPLAN.Controllers.Principal
{
    public class UsuarioController : Controller
    {
        private readonly string SUCCESS = "success";
        private readonly string MESSAGE = "message";
        private const string PAGE = "page";
        private const string TOTAL_PAGE = "total";
        private const string ROWS = "rows";
        private const string ITEMS = "items";
        private readonly string OBJECT = "object";
        private UsuarioFactoryServices usuarioFactoryServices;
        private TipoMaquinariaFactoryServices tipoMaquinariaFactoryServices;
        private IAlertaMantenimientoDAO alertaMantenimientoService;

        public ActionResult Login()
        {
            return View();
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            usuarioFactoryServices = new UsuarioFactoryServices();
            tipoMaquinariaFactoryServices = new TipoMaquinariaFactoryServices();
            alertaMantenimientoService = new AlertaMantenimientoFactoryServices().getAlertaMantenimientoService();
        }

        public ActionResult getLogin(tblP_Usuario obj)
        {
            var result = new Dictionary<string, object>();

            try
            {
                vSesiones.reset();
                if (vSesiones.sesionEmpresaActual == 0)
                {
                    vSesiones.sesionEmpresaActual = 1;
                }
                UsuarioDTO usuarioDTO = usuarioFactoryServices.getUsuarioService().IniciarSesion(obj.nombreUsuario, obj.contrasena);
                if (!string.IsNullOrEmpty(usuarioDTO.id.ToString()))
                {
                    vSesiones.sesionUsuarioDTO = usuarioDTO;

                    if (usuarioDTO.empresas.Count > 1)
                    {
                        result.Add("sistemas", usuarioDTO.empresas);
                        result.Add("setReedireccion", true);
                        result.Add("VistaProcedimientos", false);
                    }
                    else
                    {
                        result.Add("sistemas", usuarioDTO.sistemas);
                        result.Add("empresaActual", usuarioDTO.empresas.FirstOrDefault());
                        result.Add("setReedireccion", false);
                        result.Add("VistaProcedimientos", usuarioDTO.sistemas.FirstOrDefault().id == (int)SistemasEnum.PROCESOS);
                    }

                    result.Add("externoSeguridad", usuarioDTO.externoSeguridad);

                    result.Add("VistaCalendario", vSesiones.sesionUsuarioDTO.VistaCalendario);
                    result.Add("VistaGestorCorporativo", vSesiones.sesionUsuarioDTO.externoGestor);

                    result.Add(SUCCESS, true);
                }
                else
                {
                    result.Add(SUCCESS, false);
                }
            }
            catch (Exception e)
            {
                result.Add(SUCCESS, false);
                if (e is ApplicationException)
                {
                    result.Add(MESSAGE, e.Message);
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public void getLoginNP(string u, string p, int empresa)
        {
            if (Session != null)
                clearAllSessions();
            vSesiones.sesionEmpresaActual = empresa;
            UsuarioFactoryServices usuarioFactoryServices = new UsuarioFactoryServices();
            UsuarioDTO usuarioDTO = usuarioFactoryServices.getUsuarioService().IniciarSesion(u, p);
            vSesiones.sesionEmpresaActual = empresa;
            vSesiones.sesionUsuarioDTO = usuarioDTO;

            List<Core.DTO.Sistemas.tblP_EmpresasDTO> lstEmpresas = new List<Core.DTO.Sistemas.tblP_EmpresasDTO>();
            Core.DTO.Sistemas.tblP_EmpresasDTO obj = new Core.DTO.Sistemas.tblP_EmpresasDTO();
            lstEmpresas.Add(obj);
            vSesiones.sesionUsuarioDTO.empresas = usuarioDTO.empresas;
        }
        public void getLoginNPCE(string u, string p, int empresa, int routing)
        {
            if (Session != null)
                clearAllSessions();
            vSesiones.sesionEmpresaActual = empresa;
            vSesiones.sesionBestRouting = routing;
            UsuarioFactoryServices usuarioFactoryServices = new UsuarioFactoryServices();
            UsuarioDTO usuarioDTO = usuarioFactoryServices.getUsuarioService().setCambioEmpresa(u, p);
            vSesiones.sesionUsuarioDTO = usuarioDTO;
            vSesiones.sesionEmpresaActual = empresa;
            vSesiones.sesionBestRouting = routing;
        }

        public void getLoginSISI(string u, string p, int empresa, int routing, int vista)
        {
            if (Session != null)
                clearAllSessions();
            vSesiones.sesionEmpresaActual = empresa;
            vSesiones.sesionBestRouting = routing;
            UsuarioFactoryServices usuarioFactoryServices = new UsuarioFactoryServices();
            UsuarioDTO usuarioDTO = usuarioFactoryServices.getUsuarioService().setCambioEmpresa(u, p);
            vSesiones.sesionUsuarioDTO = usuarioDTO;
            vSesiones.sesionEmpresaActual = empresa;
            vSesiones.sesionBestRouting = routing;
            vSesiones.sesionCurrentView = vista;
        }

        public ActionResult removerAlerta(int idAlerta)
        {
            var result = new Dictionary<string, object>();

            try
            {
                usuarioFactoryServices.getUsuarioService().removerAlerta(idAlerta);

            }
            catch (Exception e)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, e.Message);
            }

            return RedirectToAction("SISTEMA", "SISTEMA");
        }

        public ActionResult getSistemas()
        {
            var result = new Dictionary<string, object>();

            try
            {
                result.Add("facultamientoFacturas", vSesiones.sesionUsuarioDTO.facultamientoFacturas);
                result.Add("sistemas", vSesiones.sesionUsuarioDTO.sistemas);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, e.Message);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CerrarSesion()
        {
            var result = new Dictionary<string, object>();
            try
            {
                clearAllSessions();
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, e.Message);
            }
            return RedirectToAction("Login", "Usuario");
        }
        public ActionResult CerrarSesionNR()
        {
            var result = new Dictionary<string, object>();

            try
            {
                clearAllSessions();
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, e.Message);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult enviarAccesos()
        {
            var result = new Dictionary<string, object>();
            try
            {
                usuarioFactoryServices.getUsuarioService().enviarAcceso();
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, e.Message);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public void clearAllSessions()
        {
            Session.Abandon();
            Session.RemoveAll();
            Session.Clear();
            vSesiones.clear();
        }
        //Sesion
        #region ComprobarSession
        public ActionResult ComprobarSession()
        {
            var result = new Dictionary<string, object>();

            DateTime updateSession;
            DateTime serverTime;
            int minutes;
            int timeout;
            int timeremaining;

            updateSession = ((DateTime)Session["updateSession"]);
            serverTime = ((DateTime)Session["serverTime"]);

            minutes = ((TimeSpan)(serverTime - updateSession)).Minutes;
            timeout = Session.Timeout;
            timeremaining = Session.Timeout - minutes;

            result.Add(OBJECT, new TiempoSesionDTO()
            {
                UpdateSession = updateSession.ToString("dd/MM/yyyy hh:mm:ss"),
                ServerTime = serverTime.ToString("dd/MM/yyyy hh:mm:ss"),
                Minutos = minutes,
                Timeout = timeout,
                Restante = timeremaining
            });

            Session["serverTime"] = updateSession;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region ContinuarSession
        public ActionResult ContinuarSession()
        {
            var result = new Dictionary<string, object>();

            DateTime updateSession = DateTime.Now;
            DateTime serverTime = DateTime.Now;

            int minutes = 0;
            int timeout = Session.Timeout;

            result.Add(OBJECT, new TiempoSesionDTO()
            {
                UpdateSession = updateSession.ToString("dd/MM/yyyy hh:mm:ss"),
                ServerTime = serverTime.ToString("dd/MM/yyyy hh:mm:ss"),
                Minutos = minutes,
                Timeout = timeout,
                Restante = timeout
            });

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AbandonarSession()
        {
            var result = new Dictionary<string, object>();

            clearAllSessions();

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region AlarmaMantenimiento
        public ActionResult VerificarAlarmaMantenimiento()
        {
            return Json(alertaMantenimientoService.VerificarAlertaMantenimiento(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult replacePass()
        {
            var result = new Dictionary<string, object>();
            try
            {
                usuarioFactoryServices.getUsuarioService().replacePass();
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
