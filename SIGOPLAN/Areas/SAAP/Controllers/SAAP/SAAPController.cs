using Core.DAO.SAAP;
using Core.DTO;
using Core.Entity.SAAP;
using Core.Enum.SAAP;
using Data.Factory.SAAP;
using DotnetDaddy.DocumentViewer;
using Infrastructure.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.SAAP
{
    public class SAAPController : BaseController
    {
        private ISAAPDAO saapService;
        Dictionary<string, object> result = new Dictionary<string, object>();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            saapService = new SAAPFactoryService().getSAAPService();

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
            return Json(saapService.GetAgrupacionCombo(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetActividades()
        {
            return Json(saapService.GetActividades(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarNuevaActividad(tblSAAP_Actividad actividad)
        {
            return Json(saapService.GuardarNuevaActividad(actividad), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarActividad(tblSAAP_Actividad actividad)
        {
            return Json(saapService.EditarActividad(actividad), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarActividad(tblSAAP_Actividad actividad)
        {
            return Json(saapService.EliminarActividad(actividad), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRelacionesEmpleadoAreaAgrupacion()
        {
            return Json(saapService.GetRelacionesEmpleadoAreaAgrupacion(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarNuevaRelacion(tblSAAP_RelacionEmpleadoAreaAgrupacion relacion)
        {
            return Json(saapService.GuardarNuevaRelacion(relacion), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarRelacion(tblSAAP_RelacionEmpleadoAreaAgrupacion relacion)
        {
            return Json(saapService.EditarRelacion(relacion), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarRelacion(tblSAAP_RelacionEmpleadoAreaAgrupacion relacion)
        {
            return Json(saapService.EliminarRelacion(relacion), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUsuarioPorClave(int claveEmpleado)
        {
            return Json(saapService.GetUsuarioPorClave(claveEmpleado), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAreaCombo()
        {
            return Json(saapService.GetAreaCombo(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetClasificacionCombo(List<int> areas)
        {
            return Json(saapService.GetClasificacionCombo(areas), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetActividadesAplicables(List<int> areas, List<ClasificacionActividadEnum> clasificaciones)
        {
            return Json(saapService.GetActividadesAplicables(areas, clasificaciones), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAsignacionActividades(int agrupacion_id)
        {
            return Json(saapService.GetAsignacionActividades(agrupacion_id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarAsignacionActividades(List<int> agrupaciones, DateTime fechaInicioEvaluacion, List<int> actividades)
        {
            return Json(saapService.GuardarAsignacionActividades(agrupaciones, fechaInicioEvaluacion, actividades), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarAsignacionesActividades(List<int> listaAsignaciones_id)
        {
            return Json(saapService.EliminarAsignacionesActividades(listaAsignaciones_id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAsignacionActividadesCaptura(int agrupacion_id, int area, EstatusEvidenciaEnum estatus)
        {
            return Json(saapService.GetAsignacionActividadesCaptura(agrupacion_id, area, estatus), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarEvidencia(List<HttpPostedFileBase> evidencias)
        {
            var captura = JsonConvert.DeserializeObject<tblSAAP_Evidencia>(Request.Form["captura"], new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            result = saapService.GuardarEvidencia(captura, evidencias);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarDatosArchivoEvidencia(int evidencia_id)
        {
            var resultado = new Dictionary<string, object>();

            resultado = saapService.CargarDatosArchivoEvidencia(evidencia_id);

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
            var resultadoTupla = saapService.DescargarArchivoEvidencia(evidencia_id);

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

        public ActionResult GetActividadesEvaluacion(int agrupacion_id, int estatus, int filtroArea)
        {
            return Json(saapService.GetActividadesEvaluacion(agrupacion_id, estatus, filtroArea), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEvidenciasActividad(int agrupacion_id, int area, int actividad_id)
        {
            return Json(saapService.GetEvidenciasActividad(agrupacion_id, area, actividad_id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarEvaluaciones(List<tblSAAP_Evidencia> evaluaciones)
        {
            return Json(saapService.GuardarEvaluaciones(evaluaciones), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarDashboard(List<int> listaAgrupaciones, int filtroArea)
        {
            return Json(saapService.CargarDashboard(listaAgrupaciones, filtroArea), JsonRequestBehavior.AllowGet);
        }
    }
}