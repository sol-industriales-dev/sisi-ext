using Newtonsoft.Json;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Converters;
using Core.Entity.Administrativo.ReservacionVehiculo;
using Data.Factory.Administracion.ReservacionVehiculo;
using Core.DAO.Administracion.ReservacionVehiculo;
using Core.DAO.Principal.Usuarios;
using Data.Factory.Principal.Usuarios;

namespace SIGOPLAN.Areas.Administrativo.Controllers.ReservacionVehiculo
{
    public class ReservacionVehiculoController : BaseController
    {
        IUsuarioDAO usuarioService;
        private ReservacionVehiculoFactoryService ReservacionVehiculoFactoryService;
        Dictionary<string, object> result;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            usuarioService = new UsuarioFactoryServices().getUsuarioService();
            ReservacionVehiculoFactoryService = new ReservacionVehiculoFactoryService();
            result = new Dictionary<string, object>();
            base.OnActionExecuting(filterContext);
        }

        // GET: Administrativo/ReservacionVehiculo
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Solicitud()
        {
            return View();
        }
        public ActionResult Autorizar()
        {
            return View();
        }
        public ActionResult Calendario()
        {
            return View();
        }

        #region SOLICITAR
        public ActionResult LlenarComboUsuarios(string cc)
        {
            var usuarios = usuarioService.ListUsersAll().Where(x => x.estatus == true).Select(y => new
            {
                Value = y.id,
                Text = y.nombre + ' ' + y.apellidoPaterno + ' ' + y.apellidoMaterno
            }).ToList();

            if (usuarios.Count > 0)
            {
                result.Add("items", usuarios);
                result.Add(SUCCESS, true);
            }
            else
            {
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarSolicitud(tblRV_Solicitudes solicitud)
        {
            result = ReservacionVehiculoFactoryService.getControlObraService().guardarSolicitud(solicitud);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EnviarCorreoSolicitud(int id)
        {
            result = ReservacionVehiculoFactoryService.getControlObraService().enviarCorreoSolicitud(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EnviarCorreoReemplazo(int solicitudAnterior_id, int solicitudNueva_id)
        {
            result = ReservacionVehiculoFactoryService.getControlObraService().enviarCorreoReemplazo(solicitudAnterior_id, solicitudNueva_id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EnviarCorreoAutorizacion(int solicitudAutorizada_id)
        {
            result = ReservacionVehiculoFactoryService.getControlObraService().enviarCorreoAutorizacion(solicitudAutorizada_id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EnviarCorreoRechazo(int solicitudRechazada_id)
        {
            result = ReservacionVehiculoFactoryService.getControlObraService().enviarCorreoRechazo(solicitudRechazada_id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EnviarCorreoCancelacion(int solicitudCancelada_id)
        {
            result = ReservacionVehiculoFactoryService.getControlObraService().enviarCorreoCancelacion(solicitudCancelada_id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AutorizarSolicitud(int solicitud_id)
        {
            result = ReservacionVehiculoFactoryService.getControlObraService().autorizarSolicitud(solicitud_id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EliminarSolicitud(int solicitud_id)
        {
            result = ReservacionVehiculoFactoryService.getControlObraService().eliminarSolicitud(solicitud_id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSolicitudes()
        {
            result = ReservacionVehiculoFactoryService.getControlObraService().getSolicitudes();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSolicitudFecha(DateTime fechaSalida, DateTime fechaEntrega)
        {
            result = ReservacionVehiculoFactoryService.getControlObraService().getSolicitudFecha(fechaSalida, fechaEntrega);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}