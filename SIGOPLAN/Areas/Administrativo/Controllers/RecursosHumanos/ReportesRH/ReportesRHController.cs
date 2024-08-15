using Core.DTO.Principal.Generales;
using Data.Factory.Principal.Usuarios;
using Data.Factory.Principal.Alertas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.DTO;
using Newtonsoft.Json;
using SIGOPLAN.Controllers;
using Core.Enum.Maquinaria.Reportes;
using Data.Factory.RecursosHumanos.Captura;
using Core.DTO.RecursosHumanos;
using System.IO;
using Core.DTO.RecursosHumanos.Constancias;
using Core.Entity.RecursosHumanos.Reportes;
using Core.DAO.RecursosHumanos.ReportesRH;
using Core.Entity.RecursosHumanos.Reclutamientos;
using DotnetDaddy.DocumentViewer;



namespace SIGOPLAN.Areas.Administrativo.Controllers.RecursosHumanos.ReportesRH
{
    public class ReportesRHController : BaseController
    {
        public IReportesRHDAO ReportesInterfaz;

        private readonly string SUCCESS = "success";
        private readonly string MESSAGE = "message";
        private const string PAGE = "page";
        private const string TOTAL_PAGE = "total";
        private const string ROWS = "rows";
        private const string ITEMS = "items";
        private UsuarioFactoryServices usuarioFactoryServices;
        private FormatoCambioFactoryService capturaFormatoCambioFactoryServices = new FormatoCambioFactoryService();
        private Data.Factory.RecursosHumanos.ReportesRH.ReportesRHFactoryServices reportesRHFactoryServices;

        Dictionary<string, object> result;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            usuarioFactoryServices = new UsuarioFactoryServices();
            reportesRHFactoryServices = new Data.Factory.RecursosHumanos.ReportesRH.ReportesRHFactoryServices();
            base.OnActionExecuting(filterContext);
        }

        #region VISOR
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
        #endregion

        #region VISTAS

        public ActionResult Altas()
        {
            return View();
        }
        public ActionResult Activos()
        {
            return View();
        }
        public ActionResult Bajas()
        {
            return View();
        }
        public ActionResult ReporteBajas()
        {
            return View();
        }
        public ActionResult Cambios()
        {
            return View();
        }
        public ActionResult Modificaciones()
        {
            return View();
        }
        public ActionResult Staffing()
        {
            return View();
        }
        public ActionResult Dashboard()
        {
            return View();
        }
        public ActionResult Constancias()
        {
            return View();
        }
        public ActionResult CapturaPrestamo()
        {
            return PartialView();
        }
        public ActionResult ConsultaPrestamos()
        {
            return PartialView();
        }
        public ActionResult GestionPrestamo(string idEmpleado, string ccEmpleado, string tipoDePrestamo, string statusPrestamo)
        {
            if(idEmpleado!=null)
            {
                ViewBag.idEmpleado = idEmpleado; 
            }
            else
            {
                ViewBag.idEmpleado = "";
            }
         
            ViewBag.ccEmpleado = ccEmpleado;
            ViewBag.tipoDePrestamo = tipoDePrestamo;
            ViewBag.statusPrestamo = statusPrestamo;

            return PartialView();
        }
        public ActionResult ConfiguracionPrestamo()
        {
            return PartialView();
        }
        public ActionResult Expediciones()
        {
            return View();
        }
        public ActionResult DashboardPrestamos()
        {
            return View();
        }
        public ActionResult Generales()
        {
            return View();
        }
        #endregion

        public ActionResult FillComboCC()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var list = reportesRHFactoryServices.getReportesRHService().getListaCCRH();
                result.Add(ITEMS, list.OrderBy(x => x.Value));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getListaCCRHBajas()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var list = reportesRHFactoryServices.getReportesRHService().getListaCCRHBajas();
                result.Add(ITEMS, list.OrderBy(x => x.Value));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillComboConceptosBaja()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var list = reportesRHFactoryServices.getReportesRHService().getListaConceptosBaja().ToList();
                result.Add(ITEMS, list.OrderBy(x => x.Value));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillGridRepAltas(List<string> cc, DateTime fechaInicio, DateTime fechaFin)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var listResult = reportesRHFactoryServices.getReportesRHService().getListaAltas(cc, fechaInicio, fechaFin).ToList();

                result.Add("current", 1);
                result.Add("rowCount", 1);
                result.Add("total", listResult.Count());
                var temp = listResult;
                result.Add("rows", temp);
                Session["cr" + ReportesEnum.RHREPALTAS] = JsonConvert.SerializeObject(temp);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);

            }
            //return Json(result, JsonRequestBehavior.AllowGet);
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;

            return json;
        }
        public ActionResult FillGridRepActivos(List<string> cc)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var listResult = reportesRHFactoryServices.getReportesRHService().getListaActivos(cc).ToList();

                result.Add("current", 1);
                result.Add("rowCount", 1);
                result.Add("total", listResult.Count());
                var temp = listResult.OrderBy(x => x.cC).ThenBy(x => x.empleado).ThenBy(x => x.fechaAlta);
                result.Add("rows", temp);
                Session["cr" + ReportesEnum.RHREPACTIVOS] = JsonConvert.SerializeObject(temp);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRptActivos(List<string> cc)
        {
            var result = reportesRHFactoryServices.getReportesRHService().GetRptActivos(cc);
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;

            return json;
            //return Json(reportesRHFactoryServices.getReportesRHService().GetRptActivos(cc), JsonRequestBehavior.AllowGet);
        }

        public ActionResult getLayoutBajas(List<string> cc, List<string> concepto, DateTime fechaInicio, DateTime fechaFin, bool tipo, List<int> estatus, DateTime? fechaContaInicio, DateTime? fechaContaFin)
        {
            var result = new Dictionary<string, object>();

            var listResult = reportesRHFactoryServices.getReportesRHService().getLayoutBajas(cc, concepto, fechaInicio, fechaFin, tipo, estatus, fechaContaInicio, fechaContaFin).ToList();
            Session["ListaLayoutBajasRHDTO"] = listResult.ToList();
            result.Add("current", 1);
            result.Add("rowCount", 1);
            result.Add("total", listResult.Count());
            var temp = listResult.OrderBy(x => x.cC).ThenBy(x => x.empleado).ThenBy(x => x.fechaBaja);
            result.Add("rows", listResult);
            Session["cr" + ReportesEnum.RHREPBAJAS] = JsonConvert.SerializeObject(temp);
            result.Add(SUCCESS, true);

            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;

            return json;
        }

        public ActionResult FillGridRepBajas(List<string> cc, List<string> concepto, DateTime fechaInicio, DateTime fechaFin, bool tipo, List<int> estatus, DateTime? fechaContaInicio, DateTime? fechaContaFin, int? tipoBajas)
        {
            var result = new Dictionary<string, object>();
            var json = Json(result, JsonRequestBehavior.AllowGet);
            //try
            //{
            try
            {
                var listResult = reportesRHFactoryServices.getReportesRHService().getListaBajas(cc, concepto, fechaInicio, fechaFin, tipo, estatus, fechaContaInicio, fechaContaFin, tipoBajas).ToList();
                Session["ListaLayoutBajasRHDTO"] = listResult.ToList();
                result.Add("current", 1);
                result.Add("rowCount", 1);
                result.Add("total", listResult.Count());
                var temp = listResult.OrderBy(x => x.cC).ThenBy(x => x.empleado).ThenBy(x => x.fechaBaja);
                result.Add("rows", listResult);
                Session["cr" + ReportesEnum.RHREPBAJAS] = JsonConvert.SerializeObject(temp);
                result.Add(SUCCESS, true);
                //}
                //catch (Exception e)
                //{
                //    result.Add(MESSAGE, e.Message);
                //    result.Add(SUCCESS, false);

                //}
                //return Json(result, JsonRequestBehavior.AllowGet);
                json.MaxJsonLength = int.MaxValue;
            }
            catch (Exception e)
            {
                
                throw e;
            }
            

            return json;
        }
        public ActionResult FillComboCambio()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var list = new List<ComboDTO>();
                list.Add(new ComboDTO { Value = "Puesto", Text = "Puesto" });
                list.Add(new ComboDTO { Value = "Sueldo", Text = "Sueldo" });
                list.Add(new ComboDTO { Value = "CC", Text = "CC" });
                list.Add(new ComboDTO { Value = "Jefe", Text = "Jefe Inmediato" });
                list.Add(new ComboDTO { Value = "Patronal", Text = "Registro Patronal" });
                list.Add(new ComboDTO { Value = "Nomina", Text = "Tipo de Nomina" });
                result.Add(ITEMS, list.OrderBy(x => x.Value));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillComboEmpleado(List<string> cc)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var list = reportesRHFactoryServices.getReportesRHService().getListaEmpleadosByCC(cc);
                result.Add(ITEMS, list.OrderBy(x => x.Value));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillGridRepCambios(FiltrosCambiosDTO obj)//(List<string> cc, List<string> concepto, List<string> empleado, DateTime fechaInicio, DateTime fechaFin)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var listResult = reportesRHFactoryServices.getReportesRHService().getListaCambios(obj.cc, obj.concepto, obj.empleado, obj.fechaInicio, obj.fechaFin).ToList();
                foreach (var lstCa in listResult)
                {
                    var lstdescc = capturaFormatoCambioFactoryServices.getFormatoCambioService().getCCList().Where(x => x.cc == lstCa.cC).First();
                    lstCa.cC = lstdescc.cc + " - " + lstdescc.descripcion;
                }
                result.Add("current", 1);
                result.Add("rowCount", 1);
                result.Add("total", listResult.Count());
                var temp = listResult.OrderBy(x => x.cC).ThenBy(x => x.fechaCambio).ThenBy(x => x.empleadoID);
                result.Add("rows", temp);
                Session["cr" + ReportesEnum.RHREPCAMBIOS] = JsonConvert.SerializeObject(temp);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillGridRepModificaciones(List<string> cc, List<string> concepto, DateTime fechaInicio, DateTime fechaFin)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var listResult = reportesRHFactoryServices.getReportesRHService().getListaModificaciones(cc, concepto, fechaInicio, fechaFin).ToList();

                result.Add("current", 1);
                result.Add("rowCount", 1);
                result.Add("total", listResult.Count());
                var temp = listResult.OrderBy(x => x.cC).ThenBy(x => x.fecha).ThenBy(x => x.concepto);
                result.Add("rows", temp);
                Session["cr" + ReportesEnum.RHREPMODIFICACIONES] = JsonConvert.SerializeObject(temp);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CatIncidencia()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var list = reportesRHFactoryServices.getReportesRHService().CatIncidencia();
                result.Add(ITEMS, list.OrderBy(x => x.Value));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
            //return Json(reportesRHFactoryServices.getReportesRHService().CatIncidencia(), JsonRequestBehavior.AllowGet);
        }

        #region REPORTE GENERAL
        public ActionResult GetRptGeneral(List<string> cc)
        {
            var json = Json(reportesRHFactoryServices.getReportesRHService().GetRptGeneral(cc), JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;

            return json;
        }
        #endregion

        #region RPT STAFFING GUIDE
        public ActionResult GetPuestosCategoriasRelPuesto(string _cc, string _strPuesto)
        {
            return Json(reportesRHFactoryServices.getReportesRHService().GetPuestosCategoriasRelPuesto(_cc, _strPuesto), JsonRequestBehavior.AllowGet);
        }
        public ActionResult creaVariableDeSesion(string cc)
        {
            var result = new Dictionary<string, object>();
            Session["Documento"] = reportesRHFactoryServices.getReportesRHService().crearReporte(cc);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public MemoryStream crearReporte()
        {
            var result = new Dictionary<string, object>();
            MemoryStream stream = (MemoryStream)Session["Documento"];

            if (stream != null)
            {
                this.Response.Clear();
                this.Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                this.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", "Reporte Staffing Guide.xlsx"));
                this.Response.BinaryWrite(stream.ToArray());
                this.Response.End();

                Session["Documento"] = null;
                return stream;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region DASHBOARD
        public ActionResult GetDashboard(List<string> ccs, DateTime fechaInicio, DateTime fechaFin)
        {
            var json = Json(reportesRHFactoryServices.getReportesRHService().GetDashboard(ccs, fechaInicio, fechaFin), JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;

            return json;

        }

        #endregion

        #region Constancias
        public ActionResult GetEmpleadosPrestamos(List<string> cc)
        {
            return Json(reportesRHFactoryServices.getReportesRHService().GetEmpleadosPrestamos(cc), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetConsultaCC(List<string> cc, string estatus)
        {
            var json = Json(reportesRHFactoryServices.getReportesRHService().GetConsultaCC(cc, estatus), JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;

            return json;
        }

        public ActionResult GetInfoPrestamos(int clave_empleado)
        {
            return Json(reportesRHFactoryServices.getReportesRHService().GetInfoPrestamos(clave_empleado), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPrestamos(int clave_empleado)
        {
            return Json(reportesRHFactoryServices.getReportesRHService().GetPrestamos(clave_empleado), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetConsultaPrestamos(int clave_empleado)
        {
            return Json(reportesRHFactoryServices.getReportesRHService().GetConsultaPrestamos(clave_empleado), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPrestamosFiltro(FiltroPrestamosDTO objFiltro)
        {
            return Json(reportesRHFactoryServices.getReportesRHService().GetPrestamosFiltro(objFiltro), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPrestamosFiltroGestion(List<string> cc3, string estatus, string tipoPrestamo)
        {
            return Json(reportesRHFactoryServices.getReportesRHService().GetPrestamosFiltroGestion(cc3, estatus, tipoPrestamo), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSolicitudPrestamos(int clave_empleado)
        {
            return Json(reportesRHFactoryServices.getReportesRHService().GetSolicitudPrestamos(clave_empleado), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSolicitudLactancia(int clave_empleado)
        {
            return Json(reportesRHFactoryServices.getReportesRHService().GetSolicitudLactancia(clave_empleado), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSolicitudFonacot(int clave_empleado)
        {
            return Json(reportesRHFactoryServices.getReportesRHService().GetSolicitudFonacot(clave_empleado), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSolicitudGuarderia(int clave_empleado)
        {
            return Json(reportesRHFactoryServices.getReportesRHService().GetSolicitudGuarderia(clave_empleado), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetHijos(int clave_empleado)
        {
            return Json(reportesRHFactoryServices.getReportesRHService().GetHijos(clave_empleado), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSolicitudLaboral(int clave_empleado)
        {
            return Json(reportesRHFactoryServices.getReportesRHService().GetSolicitudLaboral(clave_empleado), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetConfiguracionPrestamos()
        {
            return Json(reportesRHFactoryServices.getReportesRHService().GetConfiguracionPrestamos(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarConfiguracionPrestamo(tblRH_Prestamos_ConfiguracionPrestamo data)
        {
            result = reportesRHFactoryServices.getReportesRHService().GuardarConfiguracionPrestamo(data);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetFechasPeriodos()
        {
            result = reportesRHFactoryServices.getReportesRHService().GetFechasPeriodos();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarEditarPrestamos(tblRH_EK_Prestamos data)
        {
            return Json(reportesRHFactoryServices.getReportesRHService().GuardarEditarPrestamos(data), JsonRequestBehavior.AllowGet);
        }
        public ActionResult EliminarPrestamo(int prestamo_id)
        {
            result = reportesRHFactoryServices.getReportesRHService().EliminarPrestamo(prestamo_id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EliminarConfiguracion(int id)
        {
            result = reportesRHFactoryServices.getReportesRHService().EliminarConfiguracion(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult AutorizarRechazarPrestamo(string estatus)
        //{
        //    result = reportesRHFactoryServices.getReportesRHService().AutorizarRechazarPrestamo(estatus);
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult ActivarDesactivarPeriodo(int id, string estatus)
        {
            result = reportesRHFactoryServices.getReportesRHService().ActivarDesactivarPeriodo(id, estatus);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetInfoLaboral(int clave_empleado)
        {
            return Json(reportesRHFactoryServices.getReportesRHService().GetInfoLaboral(clave_empleado), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetInfoLiberacion(int clave_empleado)
        {
            return Json(reportesRHFactoryServices.getReportesRHService().GetInfoLiberacion(clave_empleado), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetInfoPagare(int clave_empleado)
        {
            return Json(reportesRHFactoryServices.getReportesRHService().GetInfoPagare(clave_empleado), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetInfoFonacot(int clave_empleado)
        {
            return Json(reportesRHFactoryServices.getReportesRHService().GetInfoFonacot(clave_empleado), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetInfoGuarderia(int clave_empleado)
        {
            return Json(reportesRHFactoryServices.getReportesRHService().GetInfoGuarderia(clave_empleado), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetInfoLactancia(int clave_empleado)
        {
            return Json(reportesRHFactoryServices.getReportesRHService().GetInfoLactancia(clave_empleado), JsonRequestBehavior.AllowGet);
        }
        public ActionResult SetInformacionGuarderia(repGuarderiaDTO guarderia)
        {
            result = new Dictionary<string, object>();

            Session["guarderiaInformacion"] = guarderia;

            result.Add(SUCCESS, true);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SetInformacionLaboral(repLaboralDTO laboral)
        {
            result = new Dictionary<string, object>();

            Session["laboralInformacion"] = laboral;

            result.Add(SUCCESS, true);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SetInformacionLactancia(repLactanciaDTO lactancia)
        {
            result = new Dictionary<string, object>();

            Session["lactanciaInformacion"] = lactancia;

            result.Add(SUCCESS, true);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SetInformacionFonacot(repFonacotDTO fonacot)
        {
            result = new Dictionary<string, object>();

            Session["fonacotInformacion"] = fonacot;

            result.Add(SUCCESS, true);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region cboFirmasPrestamos
        public ActionResult GetResponsableCC(int clave_empleado)
        {
            return Json(reportesRHFactoryServices.getReportesRHService().GetResponsableCC(clave_empleado), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDirectorGeneral(int clave_empleado)
        {
            return Json(reportesRHFactoryServices.getReportesRHService().GetDirectorGeneral(clave_empleado), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboAutorizantesPrestamos(int clave_empleado, string tipoPrestamo)
        {
            return Json(reportesRHFactoryServices.getReportesRHService().FillComboAutorizantesPrestamos(clave_empleado, tipoPrestamo), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboCapitalHumano()
        {
            return Json(reportesRHFactoryServices.getReportesRHService().FillCboCapitalHumano(), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        #region EXPEDICIONES
        public ActionResult GetExpediciones(string cc, int? tipoReporte, int? claveEmpleado, string nombreEmpleado)
        {
            return Json(reportesRHFactoryServices.getReportesRHService().GetExpediciones(cc, tipoReporte, claveEmpleado, nombreEmpleado), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarExpediciones(tblRH_REC_Expediciones objExpedicion)
        {
            return Json(reportesRHFactoryServices.getReportesRHService().GuardarExpediciones(objExpedicion), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarArchivoExpedicion(int idExp, Byte[] archivo)
        {
            return Json(reportesRHFactoryServices.getReportesRHService().GuardarArchivoExpedicion(idExp, archivo), JsonRequestBehavior.AllowGet);
        }
        public FileResult GetArchivoExpedicion()
        {
            try
            {
                int file_id = Convert.ToInt32(Request.QueryString["file_id"]);

                var ruta = reportesRHFactoryServices.getReportesRHService().GetArchivoExpedicion(file_id).rutaArchivo;

                return File(ruta, "multipart/form-data", Path.GetFileName(ruta));
            }
            catch (Exception e)
            {

                return null;
            }

        }
        #endregion

        #region MODULO DE PRESTAMOS
        #region CAPTURA DE PRESTAMOS

        #endregion

        #region CONSULTA DE PRESTAMOS
        public ActionResult NotificarPrestamo(int FK_Prestamo)
        {
            return Json(reportesRHFactoryServices.getReportesRHService().NotificarPrestamo(FK_Prestamo), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region CARGA DE ARCHIVOS
        public ActionResult GuardarArchivoAdjunto(List<HttpPostedFileBase> lstArchivos, int FK_Prestamo)
        {
            return Json(reportesRHFactoryServices.getReportesRHService().GuardarArchivoAdjunto(lstArchivos, FK_Prestamo), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarArchivoAdjuntoEnCaptura(List<HttpPostedFileBase> lstArchivos)
        {
            return Json(reportesRHFactoryServices.getReportesRHService().GuardarArchivoAdjuntoEnCaptura(lstArchivos), JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult GetArchivosAdjuntos(int FK_Prestamo)
        {
            return Json(reportesRHFactoryServices.getReportesRHService().GetArchivosAdjuntos(FK_Prestamo), JsonRequestBehavior.AllowGet);
        }

        public ActionResult VisualizarArchivoAdjunto(int idArchivo)
        {
            var result = reportesRHFactoryServices.getReportesRHService().VisualizarArchivoAdjunto(idArchivo);
            string ruta = result["ruta"] as string;

            var fileData = Tuple.Create(System.IO.File.ReadAllBytes(ruta), Path.GetExtension(ruta));

            Session["archivoVisor"] = fileData;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarArchivoAdjunto(int idArchivo)
        {
            return Json(reportesRHFactoryServices.getReportesRHService().EliminarArchivoAdjunto(idArchivo), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region GESTIÓN DE PRESTAMOS (AUTORIZACIONES)
        //public ActionResult AutorizarRechazarPrestamo(AutorizarRechazarPrestamoDTO objFiltroDTO)
        //{
        //    return Json(reportesRHFactoryServices.getReportesRHService().AutorizarRechazarPrestamo(objFiltroDTO), JsonRequestBehavior.AllowGet);
        //}

        public ActionResult GetListadoAutorizantes(int idPrestamo)
        {
            return Json(reportesRHFactoryServices.getReportesRHService().GetListadoAutorizantes(idPrestamo), JsonRequestBehavior.AllowGet);
        }

        public ActionResult AutorizarRechazarPrestamo(repPrestamosDTO objFiltroDTO)
        {
            return Json(reportesRHFactoryServices.getReportesRHService().AutorizarRechazarPrestamo(objFiltroDTO), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region DASHBOARD PRESTAMOS
        public ActionResult GetDashboardPrestamos(FiltroPrestamosDTO objFiltroDTO)
        {
            return Json(reportesRHFactoryServices.getReportesRHService().GetDashboardPrestamos(objFiltroDTO), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion
    }
}