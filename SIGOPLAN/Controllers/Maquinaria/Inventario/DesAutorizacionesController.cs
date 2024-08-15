using Core.DAO.Maquinaria.Catalogos;
using Core.DAO.Maquinaria.Inventario;
using Core.DAO.Principal.Usuarios;
using Data.Factory.Maquinaria.Captura;
using Data.Factory.Maquinaria.Catalogos;
using Data.Factory.Maquinaria.Inventario;
using Data.Factory.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Controllers.Maquinaria.Inventario
{
    public class DesAutorizacionesController : BaseController
    {

        private IAsignacionEquiposDAO asignacionEquipoFS;
        private IMaquinaDAO maquinaFS;
        private ICentroCostosDAO centroCostosFS;
        private ISolicitudEquipoDAO solicitudEquipoFS;
        private ISolicitudEquipoDetDAO solicitudEquipoDetFS;
        private IUsuarioDAO usuarioFS;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            usuarioFS = new UsuarioFactoryServices().getUsuarioService();
            centroCostosFS = new CentroCostosFactoryServices().getCentroCostosService();
            maquinaFS = new MaquinaFactoryServices().getMaquinaServices();
            asignacionEquipoFS = new AsignacionEquiposFactoryServices().getAsignacionEquiposFactoryServices();
            solicitudEquipoFS = new SolicitudEquipoFactoryServices().getSolicitudEquipoServices();
            solicitudEquipoDetFS = new SolicitudEquipoDetFactoryServices().getSolicitudEquipoDetServices();
            base.OnActionExecuting(filterContext);
        }

        // GET: DesAutorizaciones
        public ActionResult AutorizacionSolicitudes()
        {
            return View();
        }

        public ActionResult GetSolicitudes()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var Solicitudes = solicitudEquipoFS.GetEquipoNoMovimientos();
                result.Add("DataTables", Solicitudes.Select(x => new
                {
                    Folio = x.folio,
                    CCName = centroCostosFS.getNombreCCFix(x.CC),
                    UsuarioSolicitud = x.usuarioID != 0 ? usuarioFS.ListUsersById(x.usuarioID).FirstOrDefault().nombre : "",
                    Fecha = x.fechaElaboracion.ToShortDateString(),
                    cc = x.CC,
                    id = x.id
                }));

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SetDeshabilitada(int idSolicitud)
        {
            var result = new Dictionary<string, object>();
            try
            {
             var GetSolicitudObj = solicitudEquipoFS.loadSolicitudById(idSolicitud);

             GetSolicitudObj.EstatdoSolicitud = false;
             solicitudEquipoFS.Guardar(GetSolicitudObj);

             result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}