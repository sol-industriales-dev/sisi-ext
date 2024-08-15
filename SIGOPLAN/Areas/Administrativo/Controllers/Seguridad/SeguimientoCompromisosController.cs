using Core.DAO.Administracion.Seguridad.SeguimientoCompromisos;
using Core.Entity.SeguimientoCompromisos;
using Core.Enum.SeguimientoCompromisos;
using Data.Factory.Administracion.Seguridad.SeguimientoCompromisos;
using DotnetDaddy.DocumentViewer;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Administrativo.Controllers.Seguridad
{
    public class SeguimientoCompromisosController : BaseController
    {
        private ISeguimientoCompromisosDAO seguimientoCompromisosService;
        Dictionary<string, object> result = new Dictionary<string, object>();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            seguimientoCompromisosService = new SeguimientoCompromisosFactoryService().getSeguimientoCompromisosService();

            result.Clear();

            base.OnActionExecuting(filterContext);
        }

        public PartialViewResult _visorGrid()
        {
            var viewer = new DocViewer { ID = "ctlDoc", IncludeJQuery = false, DebugMode = false, BasePath = "/", ResourcePath = "/", FitType = "", Zoom = 40, TimeOut = 20 };

            ViewBag.ViewerScripts = viewer.ReferenceScripts();
            ViewBag.ViewerCSS = viewer.ReferenceCss();
            ViewBag.ViewerID = viewer.ClientID;
            ViewBag.ViewerObject = viewer.JsObject;
            ViewBag.ViewerInit = viewer.GetAjaxInitArguments("");

            return PartialView();
        }

        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult Captura()
        {
            return View();
        }

        public ActionResult Evaluacion()
        {
            return View();
        }

        public ActionResult Asignacion()
        {
            return View();
        }

        public ActionResult Actividades()
        {
            return View();
        }

        public ActionResult RelacionEmpleadoAreaAgrupacion()
        {
            return View();
        }

        public ActionResult GetAgrupacionCombo()
        {
            return Json(seguimientoCompromisosService.GetAgrupacionCombo(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetActividades()
        {
            return Json(seguimientoCompromisosService.GetActividades(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarNuevaActividad(tblSC_Actividad actividad)
        {
            return Json(seguimientoCompromisosService.GuardarNuevaActividad(actividad), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarActividad(tblSC_Actividad actividad)
        {
            return Json(seguimientoCompromisosService.EditarActividad(actividad), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarActividad(tblSC_Actividad actividad)
        {
            return Json(seguimientoCompromisosService.EliminarActividad(actividad), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRelacionesEmpleadoAreaAgrupacion()
        {
            return Json(seguimientoCompromisosService.GetRelacionesEmpleadoAreaAgrupacion(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarNuevaRelacion(tblSC_RelacionEmpleadoAreaAgrupacion relacion)
        {
            return Json(seguimientoCompromisosService.GuardarNuevaRelacion(relacion), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarRelacion(tblSC_RelacionEmpleadoAreaAgrupacion relacion)
        {
            return Json(seguimientoCompromisosService.EditarRelacion(relacion), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarRelacion(tblSC_RelacionEmpleadoAreaAgrupacion relacion)
        {
            return Json(seguimientoCompromisosService.EliminarRelacion(relacion), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUsuarioPorClave(int claveEmpleado)
        {
            return Json(seguimientoCompromisosService.GetUsuarioPorClave(claveEmpleado), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAreaCombo()
        {
            return Json(seguimientoCompromisosService.GetAreaCombo(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetClasificacionCombo(List<int> areas)
        {
            return Json(seguimientoCompromisosService.GetClasificacionCombo(areas), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetActividadesAplicables(List<int> areas, List<ClasificacionActividadSCEnum> clasificaciones)
        {
            return Json(seguimientoCompromisosService.GetActividadesAplicables(areas, clasificaciones), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAsignacionActividades(int agrupacion_id)
        {
            return Json(seguimientoCompromisosService.GetAsignacionActividades(agrupacion_id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarAsignacionActividades(List<int> agrupaciones, DateTime fechaInicioEvaluacion, List<int> actividades)
        {
            return Json(seguimientoCompromisosService.GuardarAsignacionActividades(agrupaciones, fechaInicioEvaluacion, actividades), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarAsignacionActividad(int asignacion_id)
        {
            return Json(seguimientoCompromisosService.EliminarAsignacionActividad(asignacion_id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAsignacionActividadesCaptura(int agrupacion_id)
        {
            return Json(seguimientoCompromisosService.GetAsignacionActividadesCaptura(agrupacion_id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarEvidencia(List<HttpPostedFileBase> evidencias)
        {
            var captura = JsonConvert.DeserializeObject<tblSC_Evidencia>(Request.Form["captura"], new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            result = seguimientoCompromisosService.GuardarEvidencia(captura, evidencias);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarDatosArchivoEvidencia(int evidencia_id)
        {
            var resultado = new Dictionary<string, object>();

            resultado = seguimientoCompromisosService.CargarDatosArchivoEvidencia(evidencia_id);

            if (Convert.ToBoolean(resultado["success"]))
            {
                var bytesArchivo = resultado["archivo"] as byte[];
                var extension = resultado["extension"] as string;

                var fileData = Tuple.Create(bytesArchivo, extension);

                Session["archivoVisor"] = fileData;
            }
            else
            {
                Session["archivoVisor"] = null;
            }

            resultado.Remove("archivo");
            resultado.Remove("extension");

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DescargarArchivoEvidencia(int evidencia_id)
        {
            var resultadoTupla = seguimientoCompromisosService.DescargarArchivoEvidencia(evidencia_id);

            if (resultadoTupla != null)
            {
                string nombreArchivo = resultadoTupla.Item2;
                string tipo = MimeMapping.GetMimeMapping(nombreArchivo);

                var fileStreamResult = new FileStreamResult(resultadoTupla.Item1, tipo);
                fileStreamResult.FileDownloadName = nombreArchivo;

                return fileStreamResult;
            }
            else
            {
                return View(RUTA_VISTA_ERROR_DESCARGA);
            }
        }

        public ActionResult GetActividadesEvaluacion(int agrupacion_id, int estatus)
        {
            return Json(seguimientoCompromisosService.GetActividadesEvaluacion(agrupacion_id, estatus), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEvidenciasActividad(int agrupacion_id, int area, int actividad_id)
        {
            return Json(seguimientoCompromisosService.GetEvidenciasActividad(agrupacion_id, area, actividad_id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarEvaluaciones(List<tblSC_Evidencia> evaluaciones)
        {
            return Json(seguimientoCompromisosService.GuardarEvaluaciones(evaluaciones), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarDashboard(List<int> listaAgrupaciones)
        {
            return Json(seguimientoCompromisosService.CargarDashboard(listaAgrupaciones), JsonRequestBehavior.AllowGet);
        }
    }
}