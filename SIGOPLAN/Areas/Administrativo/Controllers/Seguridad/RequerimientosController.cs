using Core.DTO.Administracion.Seguridad.Requerimientos;
using Core.Entity.Administrativo.Seguridad.Requerimientos;
using Core.Enum.Administracion.Seguridad.Requerimientos;
using Data.Factory.Administracion.Seguridad.Requerimientos;
using Data.Factory.Principal.Usuarios;
using DotnetDaddy.DocumentViewer;
using Infrastructure.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SIGOPLAN.Controllers;
using System.IO;
using System.IO.Compression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
//using System.Web.Http;
using System.Web.Mvc;
using Core.DTO;
using Core.DTO.Administracion.Seguridad;

namespace SIGOPLAN.Areas.Administrativo.Controllers.Seguridad
{
    public class RequerimientosController : BaseController
    {
        private RequerimientosFactoryService RequerimientosServices;
        private UsuarioFactoryServices usuarioFactoryServices;
        Dictionary<string, object> result;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            RequerimientosServices = new RequerimientosFactoryService();
            usuarioFactoryServices = new UsuarioFactoryServices();
            result = new Dictionary<string, object>();
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

        #region Vistas
        public ActionResult Captura(int division)
        {
            vSesiones.sesionDivisionActual = division;

            return View();
        }

        public ActionResult Evaluacion(int division)
        {
            vSesiones.sesionDivisionActual = division;

            return View();
        }

        public ActionResult Asignacion(int division)
        {
            vSesiones.sesionDivisionActual = division;

            return View();
        }

        public ActionResult Requerimientos(int division)
        {
            vSesiones.sesionDivisionActual = division;

            return View();
        }

        public ActionResult Actividades(int division)
        {
            vSesiones.sesionDivisionActual = division;

            return View();
        }

        public ActionResult Condicionantes(int division)
        {
            vSesiones.sesionDivisionActual = division;

            return View();
        }

        public ActionResult Secciones(int division)
        {
            vSesiones.sesionDivisionActual = division;

            return View();
        }

        public ActionResult Dashboard(int division)
        {
            vSesiones.sesionDivisionActual = division;

            return View();
        }

        public ActionResult Auditoria(int division)
        {
            vSesiones.sesionDivisionActual = division;

            return View();
        }

        public ActionResult CentroCostoDivision(int division)
        {
            vSesiones.sesionDivisionActual = division;

            return View();
        }

        public ActionResult RelacionEmpleadoAreaCC(int division)
        {
            vSesiones.sesionDivisionActual = division;

            return View();
        }
        #endregion

        #region Catálogos
        public ActionResult GetRequerimientos()
        {
            result = RequerimientosServices.GetRequerimientosService().getRequerimientos();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRequerimientosCombo()
        {
            var data = RequerimientosServices.GetRequerimientosService().getRequerimientos();
            result.Add(ITEMS, data["dataCombo"]);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPuntos()
        {
            result = RequerimientosServices.GetRequerimientosService().getPuntos();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPuntosRequerimiento(int requerimientoID)
        {
            result = RequerimientosServices.GetRequerimientosService().getPuntosRequerimiento(requerimientoID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarNuevoRequerimiento(tblS_Req_Requerimiento requerimiento, List<PuntoDTO> puntos)
        {
            result = RequerimientosServices.GetRequerimientosService().guardarNuevoRequerimiento(requerimiento, puntos);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarRequerimiento(tblS_Req_Requerimiento requerimiento, List<PuntoDTO> puntos)
        {
            result = RequerimientosServices.GetRequerimientosService().editarRequerimiento(requerimiento, puntos);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarRequerimiento(tblS_Req_Requerimiento requerimiento)
        {
            result = RequerimientosServices.GetRequerimientosService().eliminarRequerimiento(requerimiento);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetClasificacionCombo()
        {
            //Se quitan las clasificaciones que están después de "Requerimientos del Cliente" ya que éstas se utilizan en el módulo de SAAP.
            result.Add(ITEMS, (GlobalUtils.ParseEnumToCombo<ClasificacionEnum>()).Where(x => x.Value <= 7).ToList());
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetActividades()
        {
            result = RequerimientosServices.GetRequerimientosService().getActividades();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetActividadesCombo()
        {
            var data = RequerimientosServices.GetRequerimientosService().getActividades();
            result.Add(ITEMS, data["dataCombo"]);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarNuevaActividad(tblS_Req_Actividad actividad)
        {
            result = RequerimientosServices.GetRequerimientosService().guardarNuevaActividad(actividad);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarActividad(tblS_Req_Actividad actividad)
        {
            result = RequerimientosServices.GetRequerimientosService().editarActividad(actividad);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarActividad(tblS_Req_Actividad actividad)
        {
            result = RequerimientosServices.GetRequerimientosService().eliminarActividad(actividad);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCondicionantes()
        {
            result = RequerimientosServices.GetRequerimientosService().getCondicionantes();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCondicionantesCombo()
        {
            var data = RequerimientosServices.GetRequerimientosService().getCondicionantes();
            result.Add(ITEMS, data["dataCombo"]);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarNuevaCondicionante(tblS_Req_Condicionante condicionante)
        {
            result = RequerimientosServices.GetRequerimientosService().guardarNuevaCondicionante(condicionante);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarCondicionante(tblS_Req_Condicionante condicionante)
        {
            result = RequerimientosServices.GetRequerimientosService().editarCondicionante(condicionante);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarCondicionante(tblS_Req_Condicionante condicionante)
        {
            result = RequerimientosServices.GetRequerimientosService().eliminarCondicionante(condicionante);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSecciones()
        {
            result = RequerimientosServices.GetRequerimientosService().getSecciones();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSeccionesCombo()
        {
            var data = RequerimientosServices.GetRequerimientosService().getSecciones();
            result.Add(ITEMS, data["dataCombo"]);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarNuevaSeccion(tblS_Req_Seccion seccion)
        {
            result = RequerimientosServices.GetRequerimientosService().guardarNuevaSeccion(seccion);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarSeccion(tblS_Req_Seccion seccion)
        {
            result = RequerimientosServices.GetRequerimientosService().editarSeccion(seccion);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarSeccion(tblS_Req_Seccion seccion)
        {
            result = RequerimientosServices.GetRequerimientosService().eliminarSeccion(seccion);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRelacionCentroCostoDivision()
        {
            result = RequerimientosServices.GetRequerimientosService().getRelacionCentroCostoDivision();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDivisionesCombo()
        {
            var data = RequerimientosServices.GetRequerimientosService().getDivisiones();
            result.Add(ITEMS, data["dataCombo"]);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetLineaNegocioCombo(int division)
        {
            return Json(RequerimientosServices.GetRequerimientosService().GetLineaNegocioCombo(division), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarRelacionCentroCostoDivision(int grupo, int empresa, int division, int lineaNegocio_id)
        {
            result = RequerimientosServices.GetRequerimientosService().guardarRelacionCentroCostoDivision(empresa, grupo, division, lineaNegocio_id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRelacionesEmpleadoAreaCC()
        {
            result = RequerimientosServices.GetRequerimientosService().getRelacionesEmpleadoAreaCC();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarNuevaRelacion(tblS_Req_EmpleadoAreaCC relacion, bool esContratista)
        {
            result = RequerimientosServices.GetRequerimientosService().guardarNuevaRelacion(relacion, esContratista);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarRelacion(tblS_Req_EmpleadoAreaCC relacion, bool esContratista)
        {
            result = RequerimientosServices.GetRequerimientosService().editarRelacion(relacion, esContratista);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarRelacion(tblS_Req_EmpleadoAreaCC relacion)
        {
            result = RequerimientosServices.GetRequerimientosService().eliminarRelacion(relacion);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult GetAsignacion(int idEmpresa, int idAgrupacion)
        {
            result = RequerimientosServices.GetRequerimientosService().getAsignacion(idEmpresa,idAgrupacion);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetNormasAsignacionCombo(int idEmpresa, int idAgrupacion)
        {
            var data = RequerimientosServices.GetRequerimientosService().getAsignacion(idEmpresa, idAgrupacion);
            result.Add(ITEMS, data["dataCombo"]);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPeriodicidadCombo()
        {
            result.Add(ITEMS, GlobalUtils.ParseEnumToCombo<PeriodicidadRequerimientoEnum>());
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetVerificacionCombo()
        {
            result.Add(ITEMS, GlobalUtils.ParseEnumToCombo<VerificacionEnum>());
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarAsignacion(AsignacionDTO asignacion)
        {
            result = RequerimientosServices.GetRequerimientosService().guardarAsignacion(asignacion);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarAsignacionPunto(int asignacionID)
        {
            result = RequerimientosServices.GetRequerimientosService().eliminarAsignacionPunto(asignacionID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAsignacionCaptura(FiltrosAsignacionCapturaDTO filtros)
        {
            result = RequerimientosServices.GetRequerimientosService().getAsignacionCaptura(filtros);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEvidencias(string cc, int requerimientoID)
        {
            result = RequerimientosServices.GetRequerimientosService().getEvidencias(cc, requerimientoID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarEvidencia(List<HttpPostedFileBase> evidencias)
        {
            var captura = (JsonConvert.DeserializeObject<EvidenciaDTO[]>(Request.Form["captura"], new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" })).ToList();
            result = RequerimientosServices.GetRequerimientosService().guardarEvidencia(captura, evidencias);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CargarDatosArchivoEvidencia(int evidenciaID)
        {
            var resultado = new Dictionary<string, object>();

            resultado = RequerimientosServices.GetRequerimientosService().cargarDatosArchivoEvidencia(evidenciaID);

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

        public ActionResult DescargarArchivoEvidencia(int evidenciaID)
        {
            var resultadoTupla = RequerimientosServices.GetRequerimientosService().descargarArchivoEvidencia(evidenciaID);

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

        public ActionResult GetEvidenciasEvaluacion(int clasificacion, int idEmpresa, int idAgrupacion, int requerimientoID, DateTime fechaInicio, DateTime fechaFin, int estatus)
        {
            return new JsonResult
            {
                Data = RequerimientosServices.GetRequerimientosService().getEvidenciasEvaluacion(clasificacion, idEmpresa, idAgrupacion, requerimientoID, fechaInicio, fechaFin, estatus),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };
        }

        public ActionResult GuardarEvaluacion(List<EvidenciaDTO> evaluacion)
        {
            result = RequerimientosServices.GetRequerimientosService().guardarEvaluacion(evaluacion);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarExcelRequerimientosMasivo()
        {
            var result = new Dictionary<string, object>();

            try
            {
                if (Request.Files.Count > 0)
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileBase archivo = Request.Files[i];

                        RequerimientosServices.GetRequerimientosService().cargarExcelRequerimientosMasivo(archivo);
                    }

                    result.Add(SUCCESS, true);
                }
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarDashboard(List<int> listaDivisiones, List<int> listaLineasNegocio, List<MultiSegDTO> arrGrupos, List<int> listaRequerimientos, DateTime fechaInicio, DateTime fechaFin)
        {
            result = RequerimientosServices.GetRequerimientosService().cargarDashboard(listaDivisiones, listaLineasNegocio, arrGrupos, listaRequerimientos, fechaInicio, fechaFin);

            Session["dashboardChartGeneral"] = result["chartGeneral"];
            Session["dashboardChartRequerimientos"] = result["chartRequerimientos"];
            Session["dashboardChartSecciones"] = result["chartSecciones"];

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarDashboardClasificacion(List<int> listaDivisiones, List<int> listaLineasNegocio, List<MultiSegDTO> arrGrupos, List<ClasificacionEnum> listaClasificaciones, DateTime fechaInicio, DateTime fechaFin)
        {
            result = RequerimientosServices.GetRequerimientosService().cargarDashboardClasificacion(listaDivisiones, listaLineasNegocio, arrGrupos, listaClasificaciones, fechaInicio, fechaFin);

            Session["dashboardChartClasificacion"] = result["chartClasificacion"];

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAsignacionCapturaAuditoria(int idEmpresa, int idAgrupacion, ClasificacionEnum clasificacion)
        {
            result = RequerimientosServices.GetRequerimientosService().getAsignacionCapturaAuditoria(idEmpresa, idAgrupacion, clasificacion);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarEvidenciaAuditoria(List<HttpPostedFileBase> evidencias)
        {
            var captura = (JsonConvert.DeserializeObject<EvidenciaDTO[]>(Request.Form["captura"], new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" })).ToList();
            result = RequerimientosServices.GetRequerimientosService().guardarEvidenciaAuditoria(captura, evidencias);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarEvidenciaCargaMasiva(int mes, int anio)
        {
            DateTime fechaPuntos = new DateTime(anio, mes, 15);

            if (Request.Files.Count > 0)
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase archivo = Request.Files[i];
                    result = RequerimientosServices.GetRequerimientosService().guardarEvidenciaCargaMasiva(archivo, fechaPuntos);
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRequerimientosAsignacionCombo(List<int> clasificaciones)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = RequerimientosServices.GetRequerimientosService().getRequerimientosAsignacionCombo(clasificaciones);

                result.Add(ITEMS, data);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetActividadesAsignacionCombo(List<int> requerimientos)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = RequerimientosServices.GetRequerimientosService().getActividadesAsignacionCombo(requerimientos);

                result.Add(ITEMS, data);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCondicionantesAsignacionCombo(List<int> requerimientos, List<int> actividades)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = RequerimientosServices.GetRequerimientosService().getCondicionantesAsignacionCombo(requerimientos, actividades);

                result.Add(ITEMS, data);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSeccionesAsignacionCombo(List<int> requerimientos, List<int> actividades, List<int> condicionantes)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = RequerimientosServices.GetRequerimientosService().getSeccionesAsignacionCombo(requerimientos, actividades, condicionantes);

                result.Add(ITEMS, data);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboDivision()
        {
            var data = RequerimientosServices.GetRequerimientosService().FillComboDivision();
            result.Add(ITEMS, data);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboRequerimientosDashboard(int division, List<string> listaCC)
        {
            var data = RequerimientosServices.GetRequerimientosService().FillComboRequerimientosDashboard(division, listaCC);
            result.Add(ITEMS, data);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAreaCombo()
        {
            //result.Add(ITEMS, GlobalUtils.ParseEnumToCombo<AreaEnum>());
            //return Json(result, JsonRequestBehavior.AllowGet);

            return Json(RequerimientosServices.GetRequerimientosService().GetAreaCombo(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetResponsableCombo()
        {
            var data = RequerimientosServices.GetRequerimientosService().getResponsables();
            result.Add(ITEMS, data["dataCombo"]);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboCcPorDivision(int division)
        {
            var data = RequerimientosServices.GetRequerimientosService().FillComboCcPorDivision(division);
            result.Add(ITEMS, data);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}