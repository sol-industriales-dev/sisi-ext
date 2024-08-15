using Core.DAO.RecursosHumanos.ActoCondicion;
using Core.DTO.RecursosHumanos.ActoCondicion;
using Core.DTO.RecursosHumanos.ActoCondicion.Graficas;
using Core.Enum.RecursosHumanos.ActoCondicion;
using Data.Factory.RecursosHumanos;
using DotnetDaddy.DocumentViewer;
using Infrastructure.Utils;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Administrativo.Controllers.RecursosHumanos.ActoCondicion
{
    public class ActoCondicionCHController : BaseController
    {
        #region CONSTRUCTOR
        private IActoCondicionCHDAO actoCondicionService;

        Dictionary<string, object> result = new Dictionary<string, object>();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            actoCondicionService = new ActoCondicionCHFactoryService().GetActoCondicionCHService();
            result.Clear();

            base.OnActionExecuting(filterContext);
        }
        #endregion

        #region ACTO Y CONDICIONES
        // GET: Administrativo/ActoCondicion
        public ActionResult Index(int? id)
        {
            if (id.HasValue)
            {
                var resultado = actoCondicionService.VistoAlerta(id.Value);
            }

            return View();
        }

        [HttpPost]
        public ActionResult ObtenerCentrosCostos()
        {
            return Json(actoCondicionService.ObtenerCentrosCostos(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ObtenerSupervisores()
        {
            return Json(actoCondicionService.ObtenerSupervisores(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ObtenerDepartamentos()
        {
            return Json(actoCondicionService.ObtenerDepartamentos(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboSubclasificacionesDepartamentos(int? idDepartamento)
        {
            if (!idDepartamento.HasValue)
            {
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, "");
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(actoCondicionService.FillCboSubclasificacionesDepartamentos(idDepartamento.Value), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult FillCboSubclasificaciones()
        {
            return Json(actoCondicionService.FillCboSubclasificaciones(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ObtenerAcciones()
        {
            return Json(actoCondicionService.ObtenerAcciones(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ObtenerClasificaciones()
        {
            return Json(actoCondicionService.ObtenerClasificaciones(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ObtenerEstatusActoCondicion()
        {
            var resultado = new Dictionary<string, object>();
            result.Add(ITEMS, GlobalUtils.ParseEnumToCombo<EstatusActoCondicionCH>());
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CargarActosCondiciones(FiltroActoCondicionDTO filtro)
        {
            return Json(actoCondicionService.CargarActosCondiciones(filtro), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GuardarActo(ActoDTO acto)
        {
            var causas = JsonUtils.convertJsonToNetObject<List<CausaAccionDTO>>(Request.Form["causas"], "es-MX");
            var acciones = JsonUtils.convertJsonToNetObject<List<CausaAccionDTO>>(Request.Form["acciones"], "es-MX");

            acto.causas = causas;
            acto.acciones = acciones;

            return Json(actoCondicionService.GuardarActo(acto), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GuardarCondicion(CondicionDTO condicion)
        {
            return Json(actoCondicionService.GuardarCondicion(condicion), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ObtenerActoCondicion(TipoRiesgoCH tipoRiesgo, int id)
        {
            return Json(actoCondicionService.ObtenerActoCondicion(tipoRiesgo, id), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EliminarActoCondicion(TipoRiesgoCH tipoRiesgo, int id)
        {
            return Json(actoCondicionService.EliminarActoCondicion(tipoRiesgo, id), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DescargarArchivo(int sucesoID, TipoRiesgoCH tipoRiesgo, TipoArchivo tipoArchivo)
        {
            var resultadoTupla = actoCondicionService.DescargarArchivo(sucesoID, tipoRiesgo, tipoArchivo);

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

        [HttpPost]
        public ActionResult CargarDatosArchivo(int sucesoID, TipoRiesgoCH tipoRiesgo, TipoArchivo tipoArchivo)
        {
            var resultado = new Dictionary<string, object>();

            resultado = actoCondicionService.CargarDatosArchivo(sucesoID, tipoRiesgo, tipoArchivo);

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

        [HttpGet]
        public JsonResult ObtenerInformacionInfraccion(int numeroInfraccion, int claveEmpleado, DateTime fechaSuceso)
        {
            return Json(actoCondicionService.ObtenerInformacionInfraccion(numeroInfraccion, claveEmpleado, fechaSuceso), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenerInformacionInfraccionContratista(int numeroInfraccion, int claveEmpleado, DateTime fechaSuceso)
        {
            return Json(actoCondicionService.ObtenerInformacionInfraccionContratista(numeroInfraccion, claveEmpleado, fechaSuceso), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerAccionReaccion(int tipo)
        {
            return Json(actoCondicionService.ObtenerAccionReaccion(tipo), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ObtenerPrioridades()
        {
            return Json(actoCondicionService.ObtenerPrioridades());
        }

        [HttpPost]
        public ActionResult ObtenerReporteActoCondicion(int id, int tipo)
        {
            result = new Dictionary<string, object>();
            try
            {
                var resultado = actoCondicionService.ObtenerReporteActoCondicion(id, tipo);
                //result.Add(ITEMS, resultado);
                result.Add(SUCCESS, true);
                Session["rptActoCondicion"] = resultado;
            }
            catch (Exception)
            {
                result.Add(ITEMS, null);
                result.Add(SUCCESS, true);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarFirma(GuardarFirmaDTO data)
        {
            var resultado = actoCondicionService.GuardarFirma(data);

            return Json(resultado);
        }

        [HttpPost]
        public JsonResult CargarActa(HttpPostedFileBase acta, int id)
        {
            var resultado = actoCondicionService.CargarActa(acta, id);

            return Json(resultado);
        }

        [HttpPost]
        public JsonResult CargarComprimido(HttpPostedFileBase archivoComprimido)
        {
            var resultado = actoCondicionService.CargarComprimido(archivoComprimido);

            return Json(resultado);
        }

        [HttpGet]
        public ActionResult DescargarFormato()
        {
            var resultado = actoCondicionService.DescargarFormato();

            if (resultado != null)
            {
                string nombreArchivo = resultado["name"] as string;
                string tipo = MimeMapping.GetMimeMapping(nombreArchivo);

                var fileStreamResult = new FileStreamResult(resultado["archivo"] as Stream, tipo);

                return fileStreamResult;
            }
            else
            {
                return View(RUTA_VISTA_ERROR_DESCARGA);
            }
        }

        [HttpGet]
        public JsonResult GetInfoEmpleado(string term)
        {
            var resultado = actoCondicionService.GetInfoEmpleado(term);

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetInfoEmpleadoInternoContratista(string term, bool esContratista, int idEmpresaContratista)
        {
            var resultado = actoCondicionService.GetInfoEmpleadoInternoContratista(term, esContratista, idEmpresaContratista);

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DescargarReporteExcel()
        {
            var filtro = JsonUtils.convertJsonToNetObject<FiltroActoCondicionDTO>(Request.Form["filtro"], "es-MX");
            var resultado = actoCondicionService.DescargarReporteExcel(filtro);

            if ((bool)resultado[SUCCESS])
            {
                var stream = (MemoryStream)resultado[ITEMS];
                Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment; filename=Reporte.xlsx");
                Response.BinaryWrite(stream.ToArray());
                Response.End();
            }

            return null;
        }

        [HttpPost]
        public JsonResult ObtenerClasificacionesGenerales()
        {
            return Json(actoCondicionService.ObtenerClasificacionesGenerales());
        }

        public ActionResult FillCboProcedimientos(int FK_Clasificacion)
        {
            return Json(actoCondicionService.FillCboProcedimientos(FK_Clasificacion), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarArchivoAdjunto(List<HttpPostedFileBase> lstArchivos, int idActo)
        {
            return Json(actoCondicionService.GuardarArchivoAdjunto(lstArchivos, idActo), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetArchivosAdjuntos(int idActo)
        {
            return Json(actoCondicionService.GetArchivosAdjuntos(idActo), JsonRequestBehavior.AllowGet);
        }

        public ActionResult VisualizarArchivoAdjunto(int idArchivo)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();
            resultado = actoCondicionService.VisualizarArchivoAdjunto(idArchivo);

            if (Convert.ToBoolean(resultado["success"]))
            {
                var bytesArchivo = resultado["archivo"] as byte[];
                var extension = resultado["extension"] as string;
                var fileData = Tuple.Create(bytesArchivo, extension);
                Session["archivoVisor"] = fileData;
            }
            else
                Session["archivoVisor"] = null;

            resultado.Remove("archivo");
            resultado.Remove("extension");

            return Json(resultado, JsonRequestBehavior.AllowGet);
            //return Json(actoCondicionService.VisualizarArchivoAdjunto(idArchivo), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarArchivoAdjunto(int idArchivo)
        {
            return Json(actoCondicionService.EliminarArchivoAdjunto(idArchivo), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSiguienteInfraccion(int claveEmpleado, int procedimientoID)
        {
            return Json(actoCondicionService.GetSiguienteInfraccion(claveEmpleado, procedimientoID), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPermisos()
        {
            return Json(actoCondicionService.GetPermisos(), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region DASHBOARD

        public ActionResult Dashboard()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CargarDatosDashboard(FiltroDashboardDTO filtro)
        {
            return Json(actoCondicionService.CargarDatosDashboard(filtro), JsonRequestBehavior.AllowGet);
        }

        public ActionResult obtenerGraficaTotalDep(FiltroDashboardDTO filtro)
        {
            return Json(actoCondicionService.obtenerGraficaTotalDep(filtro), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDashboard(int anio)
        {
            return Json(actoCondicionService.GetDashboard(anio), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region HISTORIAL

        public ActionResult Historial()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ObtenerHistorialEmpleado(int claveEmpleado)
        {
            return Json(actoCondicionService.ObtenerHistorialEmpleado(claveEmpleado), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DescargarActo(int actoID)
        {
            var resultadoTupla = actoCondicionService.DescargarActo(actoID);

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

        [HttpPost]
        public ActionResult ObtenerMatrices(FiltroDashboardDTO filtro)
        {
            return Json(actoCondicionService.ObtenerMatrices(filtro), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public MemoryStream DescargarExcelMatrizActos()
        {
            var resultadoTupla = actoCondicionService.DescargarExcelMatrizActos();

            if (resultadoTupla != null)
            {
                string nombreArchivo = resultadoTupla.Item2;

                this.Response.Clear();
                this.Response.ContentType = MimeMapping.GetMimeMapping(nombreArchivo);

                this.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", nombreArchivo));
                this.Response.BinaryWrite(resultadoTupla.Item1.ToArray());
                this.Response.End();
                return resultadoTupla.Item1;
            }
            else
            {
                return null;
            }
        }

        [HttpGet]
        public MemoryStream DescargarExcelMatrizCondiciones()
        {
            var resultadoTupla = actoCondicionService.DescargarExcelMatrizCondiciones();

            if (resultadoTupla != null)
            {
                string nombreArchivo = resultadoTupla.Item2;

                this.Response.Clear();
                this.Response.ContentType = MimeMapping.GetMimeMapping(nombreArchivo);

                this.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", nombreArchivo));
                this.Response.BinaryWrite(resultadoTupla.Item1.ToArray());
                this.Response.End();
                return resultadoTupla.Item1;
            }
            else
            {
                return null;
            }
        }

        public ActionResult GetListadoArchivosAdjuntos(int FK_Acto)
        {
            return Json(actoCondicionService.GetListadoArchivosAdjuntos(FK_Acto), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region MATRIZ
        public ActionResult Matriz()
        {
            return View();
        }

        public ActionResult GetMatriz()
        {
            return Json(actoCondicionService.GetMatriz(), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region REPORTES
        public ActionResult GenerarReporte(ReporteActoCondicionCH objParametros)
        {
            return Json(actoCondicionService.GenerarReporte(objParametros), JsonRequestBehavior.AllowGet);
        }

        public ActionResult DescargarActa(int idActo, string ciudad, string articulos)
        {
            return Json(actoCondicionService.DescargarActa(idActo, ciudad, articulos), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region GENERALES
        public ActionResult GetContenidoActaAdministrativa(int idActo)
        {
            return Json(actoCondicionService.GetContenidoActaAdministrativa(idActo), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPuestoEmpleado(int idActo)
        {
            return Json(actoCondicionService.GetPuestoEmpleado(idActo), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ObtenerComboCCAmbasEmpresas(bool incContratista, int? division)
        {
            return Json(actoCondicionService.ObtenerComboCCAmbasEmpresas(incContratista, division), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboCC()
        {
            return Json(actoCondicionService.FillCboCC(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboDepartamentos(string cc)
        {
            return Json(actoCondicionService.FillCboDepartamentos(cc), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboTipoReportes()
        {
            return Json(actoCondicionService.FillCboTipoReportes(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult InfEmpleado(int claveEmpleado)
        {
            return Json(actoCondicionService.InfEmpleado(claveEmpleado), JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}