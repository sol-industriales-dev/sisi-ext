using Core.DAO.Administracion.Seguridad;
using Core.DAO.RecursosHumanos.Captura;
using Core.DTO;
using Core.DTO.Administracion.Seguridad.Capacitacion;
using Core.DTO.RecursosHumanos;
using Core.DTO.RecursosHumanos.Capacitacion;
using Core.Entity.Administrativo.Seguridad.Capacitacion;
using Core.Entity.RecursosHumanos.Captura;
using Core.Enum.Administracion.Seguridad.Capacitacion;
using Core.Enum.Multiempresa;
using Core.Enum.Principal;
using Data.DAO.Principal.Usuarios;
using Data.Factory.Administracion.Seguridad.Capacitacion;
using Data.Factory.Principal.Archivos;
using Data.Factory.RecursosHumanos.Captura;
using DotnetDaddy.DocumentViewer;
using Infrastructure.DTO;
using Infrastructure.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Administrativo.Controllers.Seguridad
{
    public class CapacitacionController : BaseController
    {
        private ICapacitacionDAO capacitacionService;
        IAutorizacionFormatoCambio authFormatoCambioFS;
        const string LISTA_EXAMENES_ASISTENTES = @"listaExamenes";

        Dictionary<string, object> result = new Dictionary<string, object>();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            vSesiones.sesionCapacitacionOperativa = vSesiones.sesionSistemaActual == 18 ? true : false;

            capacitacionService = new CapacitacionFactoryService().GetCapacitacionService();
            authFormatoCambioFS = new AutorizacionFormatoCambioFactoryService().getAutorizacionFormatoCambioService();

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

        #region Cursos
        // GET: Administrativo/Capacitacion/CapturaCursos
        public ActionResult CapturaCursos(int division)
        {
            vSesiones.sesionDivisionActual = division;

            if (capacitacionService.AccesoPermitidoPrivilegioDivision(division))
            {
                return View();
            }
            else
            {
                return View("ErrorPermisoVista");
            }
        }
        public ActionResult GetCursos(List<int> clasificaciones, List<int> puestos, int estatus)
        {
            result = capacitacionService.getCursos(clasificaciones, puestos, estatus);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCursoById(int id)
        {
            result = capacitacionService.getCursoById(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetExamenesCursoById(int id)
        {
            result = capacitacionService.getExamenesCursoById(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DescargarArchivo(long examen_id)
        {
            var array = capacitacionService.descargarArchivo(examen_id);
            string pathExamen = capacitacionService.getFileName(examen_id);

            if (array != null)
            {
                return File(array, System.Net.Mime.MediaTypeNames.Application.Octet, pathExamen);
            }
            else
            {
                return View("ErrorDescarga");
            }

        }
        public ActionResult GetTipoExamen(int curso_id)
        {
            result = capacitacionService.getTipoExamen(curso_id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetClasificacionCursos()
        {
            result = capacitacionService.getClasificacionCursos();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPuestos()
        {
            result = capacitacionService.getPuestosEnkontrol();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarCurso(tblS_CapacitacionCursos curso)
        {
            result = capacitacionService.guardarCurso(curso);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarExamenes()
        {
            var examenes = JsonConvert.DeserializeObject<List<tblS_CapacitacionCursosExamen>>(Request.Form["examenes"]);
            List<HttpPostedFileBase> archivos = new List<HttpPostedFileBase>();

            foreach (string fileName in Request.Files)
            {
                archivos.Add(Request.Files[fileName]);
            }

            result = capacitacionService.guardarExamenes(examenes, archivos);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ActualizarCurso(tblS_CapacitacionCursos curso, List<tblS_CapacitacionCursosMando> mandos, List<tblS_CapacitacionCursosPuestos> puestosNuevos, List<tblS_CapacitacionCursosPuestosAutorizacion> puestosAutorizacionNuevos, List<tblS_CapacitacionCursosCC> centrosCosto)
        {
            result = capacitacionService.actualizarCurso(curso, mandos, puestosNuevos, puestosAutorizacionNuevos, centrosCosto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EliminarExamen(int examen_id)
        {
            result = capacitacionService.eliminarExamen(examen_id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetEstatusCursos()
        {
            result = capacitacionService.GetEstatusCursos();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EliminarCurso(int cursoID)
        {
            result = capacitacionService.EliminarCurso(cursoID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RelacionCCAutorizante(int division)
        {
            vSesiones.sesionDivisionActual = division;

            if (capacitacionService.AccesoPermitidoPrivilegioDivision(division))
            {
                return View();
            }
            else
            {
                return View("ErrorPermisoVista");
            }
        }

        public ActionResult RelacionCCDepartamentoRazonSocial(int division)
        {
            vSesiones.sesionDivisionActual = division;

            if (capacitacionService.AccesoPermitidoPrivilegioDivision(division))
            {
                return View();
            }
            else
            {
                return View("ErrorPermisoVista");
            }
        }

        public ActionResult GetMandosEnum()
        {
            return Json(new { items = GlobalUtils.ParseEnumToCombo<MandoEnum>() }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPuestosMandos(List<int> mandos)
        {
            result = capacitacionService.getPuestosEnkontrolMandos(mandos);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Control de Asistencia
        // GET: Administrativo/Capacitacion/ControlAsistencia
        [HttpGet]
        public ActionResult ControlAsistencia(int division)
        {
            vSesiones.sesionDivisionActual = division;

            if (capacitacionService.AccesoPermitidoPrivilegioDivision(division))
            {
                return View();
            }
            else
            {
                return View("ErrorPermisoVista");
            }
        }

        [HttpGet]
        public ActionResult ErrorDescarga()
        {
            return View();
        }

        public ActionResult ObtenerComboCC()
        {
            return Json(capacitacionService.ObtenerComboCC(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ObtenerEstatusControlAsistencia()
        {
            var listaEstatus = GlobalUtils.ParseEnumToCombo<EstatusControlAsistenciaEnum>();

            if (vSesiones.sesionCapacitacionOperativa)
            {
                listaEstatus.Remove(listaEstatus.FirstOrDefault(x => x.Value == (int)EstatusControlAsistenciaEnum.PendienteAutorizacion));
            }

            return Json(listaEstatus, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ObtenerListaControlesAsistencia(string cc, int estado, string fechaInicio, string fechaFin)
        {
            try
            {
                var fInicio = DateTime.Parse(fechaInicio);
                var fFin = DateTime.Parse(fechaFin);
                return Json(capacitacionService.ObtenerListaControlesAsistencia(cc, estado, fInicio, fFin), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                var resultado = new Dictionary<string, object>();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al obtener los parámetros de búsqueda.");
                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetCursosAutocomplete(string term, bool porClave)
        {
            return Json(capacitacionService.GetCursosAutocomplete(term, porClave), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetUsuariosAutocomplete(string term, bool porClave)
        {
            return Json(capacitacionService.GetUsuariosAutocomplete(term, porClave), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetLugarCursoAutocomplete(string term)
        {
            return Json(capacitacionService.GetLugarCursoAutocomplete(term), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetEmpleadoEnKontrolAutocomplete(string term, bool porClave)
        {
            return Json(capacitacionService.GetEmpleadoEnKontrolAutocomplete(term, porClave), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CrearControlAsistencia(tblS_CapacitacionControlAsistencia controlAsistencia)
        {
            return Json(capacitacionService.CrearControlAsistencia(controlAsistencia), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SubirArchivoControlAsistencia(HttpPostedFileBase archivo, int controlAsistenciaID)
        {
            return Json(capacitacionService.SubirArchivoControlAsistencia(archivo, controlAsistenciaID), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DescargarListaControlAsistencia(int controlAsistenciaID)
        {
            var resultadoTupla = capacitacionService.DescargarListaControlAsistencia(controlAsistenciaID);

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
                return View("ErrorDescarga");
            }
        }

        [HttpGet]
        public ActionResult CargarDatosControlAsistencia(int controlAsistenciaID)
        {
            return Json(capacitacionService.CargarDatosControlAsistencia(controlAsistenciaID), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CargarAsistentesCapacitacion(int controlAsistenciaID)
        {
            return Json(capacitacionService.CargarAsistentesCapacitacion(controlAsistenciaID), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CargarExamenesAsistente(HttpPostedFileBase examenDiagnostico, HttpPostedFileBase examenFinal, int id, int claveEmpleado, bool aprobado, decimal calificacion)
        {
            var resultado = new Dictionary<string, bool>();
            try
            {
                // Checa si ya existe una lista en sesión.
                var listaExamenes = Session[LISTA_EXAMENES_ASISTENTES] as List<ExamenesAsistenteDTO>;

                // Si está nulo, la crea
                if (listaExamenes == null)
                {
                    listaExamenes = new List<ExamenesAsistenteDTO>();
                }

                var examenesAsistenteDTO = new ExamenesAsistenteDTO(examenDiagnostico, examenFinal, id, claveEmpleado, aprobado, calificacion);

                listaExamenes.Add(examenesAsistenteDTO);

                Session[LISTA_EXAMENES_ASISTENTES] = listaExamenes;
                resultado.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                Session[LISTA_EXAMENES_ASISTENTES] = null;
                resultado.Add(SUCCESS, false);
            }


            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GuardarEvaluacionAsistentes(List<AsistenteCursoDTO> listaAsistentes)
        {
            return Json(capacitacionService.GuardarEvaluacionAsistentes(listaAsistentes), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GuardarExamenesAsistentes(int jefeID, int coordinadorID, int secretarioID, int gerenteID, string rfc = "", string razonSocial = "")
        {

            var resultado = new Dictionary<string, object>();
            var listaExamenes = new List<ExamenesAsistenteDTO>();
            try
            {
                listaExamenes = Session[LISTA_EXAMENES_ASISTENTES] as List<ExamenesAsistenteDTO>;

                if (listaExamenes == null || listaExamenes.Count == 0)
                {
                    throw new Exception("No se pudieron cargar los exámenes de la sesión al intentar guardar los exámenes de los asistentes.");
                }
                resultado = capacitacionService.GuardarExamenesAsistentes(listaExamenes, jefeID, coordinadorID, secretarioID, gerenteID, rfc, razonSocial);
                Session[LISTA_EXAMENES_ASISTENTES] = null;
            }
            catch (Exception)
            {
                Session[LISTA_EXAMENES_ASISTENTES] = null;
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "No se pudo cargar correctamente los exámenes.");
                return Json(resultado, JsonRequestBehavior.AllowGet);
            }

            return Json(resultado, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult DescargarExamenAsistente(int controlAsistenciaDetalleID, int tipoExamen)
        {
            var resultadoTupla = capacitacionService.DescargarExamenAsistente(controlAsistenciaDetalleID, tipoExamen);

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
                return View("ErrorDescarga");
            }
        }

        [HttpPost]
        public ActionResult EliminarControlAsistencia(int controlAsistenciaID)
        {
            result = capacitacionService.EliminarControlAsistencia(controlAsistenciaID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GuardarArchivosDC3(HttpPostedFileBase archivoDC3, int controlAsistenciaDetalleID)
        {
            result = capacitacionService.guardarArchivosDC3(archivoDC3, controlAsistenciaDetalleID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DescargarDC3(int controlAsistenciaDetalleID)
        {
            var resultadoTupla = capacitacionService.DescargarDC3(controlAsistenciaDetalleID);

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
                return View("ErrorDescarga");
            }
        }

        public ActionResult GuardarMigracionEmpleado(int claveEmpleado, string cc, int empresa)
        {
            return Json(capacitacionService.GuardarMigracionEmpleado(claveEmpleado, cc, empresa), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Autorización Capacitación

        // GET: Administrativo/Capacitacion/AutorizacionCapacitacion
        [HttpGet]
        public ActionResult AutorizacionCapacitacion(int division)
        {
            vSesiones.sesionDivisionActual = division;

            if (capacitacionService.AccesoPermitidoPrivilegioDivision(division))
            {
                return View();
            }
            else
            {
                return View("ErrorPermisoVista");
            }
        }

        [HttpGet]
        public ActionResult ObtenerComboEstatusAutorizacionCapacitacion()
        {
            return Json(capacitacionService.ObtenerComboEstatusAutorizacionCapacitacion(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ObtenerAutorizaciones(string cc, int curso, int estatus)
        {
            var data = capacitacionService.ObtenerAutorizaciones(cc, curso, estatus);

            Session["reporteAutorizacionGeneral"] = data["reporteAutorizacionGeneral"];

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ObtenerAutorizantes(int capacitacionID)
        {
            return Json(capacitacionService.ObtenerAutorizantes(capacitacionID), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AutorizarControlAsistencia(int controlAsistenciaID)
        {
            return Json(capacitacionService.AutorizarControlAsistencia(controlAsistenciaID), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RechazarControlAsistencia(int controlAsistenciaID, string comentario)
        {
            return Json(capacitacionService.RechazarControlAsistencia(controlAsistenciaID, comentario), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EnviarCorreoRechazo(int controlAsistenciaID)
        {
            List<Byte[]> pdf;
            try
            {
                pdf = (List<Byte[]>)Session["downloadPDF"];
            }
            catch (Exception)
            {
                pdf = null;
            }
            finally
            {
                Session["downloadPDF"] = null;
            }

            if (pdf == null)
            {
                result.Add(SUCCESS, false);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(capacitacionService.EnviarCorreoRechazo(controlAsistenciaID, pdf), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EnviarCorreoAutorizacion(int controlAsistenciaID)
        {
            List<Byte[]> pdf;
            try
            {
                pdf = (List<Byte[]>)Session["downloadPDF"];
            }
            catch (Exception)
            {
                pdf = null;
            }
            finally
            {
                Session["downloadPDF"] = null;
            }

            if (pdf == null)
            {
                result.Add(SUCCESS, false);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(capacitacionService.EnviarCorreoAutorizacion(controlAsistenciaID, pdf), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EnviarCorreoAutorizacionCompleta(int controlAsistenciaID)
        {
            List<Byte[]> pdf;
            try
            {
                pdf = (List<Byte[]>)Session["downloadPDF"];
            }
            catch (Exception)
            {
                pdf = null;
            }
            finally
            {
                Session["downloadPDF"] = null;
            }

            if (pdf == null)
            {
                result.Add(SUCCESS, false);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(capacitacionService.EnviarCorreoAutorizacionCompleta(controlAsistenciaID, pdf), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult DescargarFormatoAutorizacion(int controlAsistenciaID)
        {
            //var resultadoTupla = capacitacionService.DescargarFormatoAutorizacion(controlAsistenciaID);

            //if (resultadoTupla != null)
            //{
            //    string nombreArchivo = resultadoTupla.Item2;
            //    string tipo = MimeMapping.GetMimeMapping(nombreArchivo);

            //    var fileStreamResult = new FileStreamResult(resultadoTupla.Item1, tipo);
            //    fileStreamResult.FileDownloadName = nombreArchivo;

            //    return fileStreamResult;
            //}
            //else
            //{
            //    return View("ErrorDescarga");
            //}
            try
            {
                List<byte[]> archivo = (List<byte[]>)Session["downloadPDF"];
                if (archivo[0] != null)
                {
                    return File(archivo[0], "multipart/form-data", "Formado de Autorizacion.pdf");
                }
                else
                {
                    return View("ErrorDescarga");
                }
            }
            catch (Exception e)
            {
                return View("ErrorDescarga");
            }
        }

        public ActionResult GuardarCargaMasiva()
        {
            if (Request.Files.Count > 0)
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase archivo = Request.Files[i];
                    result = capacitacionService.guardarCargaMasiva(archivo);
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Matriz de empleados
        // GET: Administrativo/Capacitacion/MatrizEmpleados
        public ActionResult MatrizEmpleados(int division)
        {
            vSesiones.sesionDivisionActual = division;

            if (capacitacionService.AccesoPermitidoPrivilegioDivision(division))
            {
                return View();
            }
            else
            {
                return View("ErrorPermisoVista");
            }
        }

        [HttpPost]
        public ActionResult ObtenerEmpleados(List<string> ccsCplan, List<string> ccsArr, List<string> puestos)
        {
            return Json(capacitacionService.ObtenerEmpleados(ccsCplan, ccsArr, puestos), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ObtenerComboCCEnKontrol(EmpresaEnum empresa)
        {
            return Json(capacitacionService.ObtenerComboCCEnKontrol(empresa), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ObtenerCursosEmpleado(int claveEmpleado, int puestoID)
        {
            return Json(capacitacionService.ObtenerCursosEmpleado(claveEmpleado, puestoID), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DescargarExpedienteEmpleado(int claveEmpleado)
        {
            List<Byte[]> licencia;
            try
            {
                licencia = (List<Byte[]>)Session["downloadPDF"];
            }
            catch (Exception)
            {
                licencia = null;
            }
            finally
            {
                Session["downloadPDF"] = null;
            }

            if (licencia == null || licencia.Count == 0)
            {
                return View("ErrorDescarga");
            }
            else
            {
                var resultadoTupla = capacitacionService.DescargarExpedienteEmpleado(claveEmpleado, licencia[0]);

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
                    return View("ErrorDescarga");
                }
            }
        }

        [HttpGet]
        public ActionResult ObtenerExtracurricularesEmpleado(int claveEmpleado)
        {
            return Json(capacitacionService.ObtenerExtracurricularesEmpleado(claveEmpleado), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SubirEvidenciaExtracurricular(int claveEmpleado, string nombre, decimal duracion, string fecha, string fechaFin = "", HttpPostedFileBase evidencia = null)
        {
            return Json(capacitacionService.SubirEvidenciaExtracurricular(claveEmpleado, nombre, duracion, fecha, fechaFin, evidencia), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DescargarEvidenciaExtracurricular(int extracurricularID)
        {
            var resultadoTupla = capacitacionService.DescargarEvidenciaExtracurricular(extracurricularID);

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
                return View("ErrorDescarga");
            }
        }

        [HttpPost]
        public ActionResult EliminarEvidenciaExtracurricular(int extracurricularID)
        {
            return Json(capacitacionService.EliminarEvidenciaExtracurricular(extracurricularID), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetArchivosMatrizEmpleados(int claveEmpleado)
        {
            return Json(capacitacionService.GetArchivosMatrizEmpleados(claveEmpleado), JsonRequestBehavior.AllowGet);
        }

        public FileResult DescargarArchivoEmpleado(string ruta)
        {
            try
            {
                return File(ruta, "multipart/form-data", Path.GetFileName(ruta));
            }
            catch (Exception e)
            {
                return null;
            }
        }
        #endregion

        #region Dashboard

        // GET: Administrativo/Capacitacion/Dashboard
        public ActionResult Dashboard(int division)
        {
            vSesiones.sesionDivisionActual = division;

            if (capacitacionService.AccesoPermitidoPrivilegioDivision(division))
            {
                return View();
            }
            else
            {
                return View("ErrorPermisoVista");
            }
        }

        [HttpPost]
        public ActionResult CargarDatosGeneralesDashboard(List<string> ccsCplan, List<string> ccsArr, List<string> departamentosIDs, DateTime fechaInicio, DateTime fechaFin, List<string> clasificacion)
        {
            return Json(capacitacionService.CargarDatosGeneralesDashboard(ccsCplan, ccsArr, departamentosIDs, fechaInicio, fechaFin, clasificacion), JsonRequestBehavior.AllowGet);
        }

        public MemoryStream DescargarExcelExpirados()
        {
            var stream = capacitacionService.DescargarExcelExpirados();

            if (stream != null)
            {
                this.Response.Clear();
                this.Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                this.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", "Reporte Expirados.xlsx"));
                this.Response.BinaryWrite(stream.ToArray());
                this.Response.End();

                return stream;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region Matriz de Necesidades
        // GET: Administrativo/Capacitacion/MatrizNecesidades
        public ActionResult MatrizNecesidades(int division)
        {
            vSesiones.sesionDivisionActual = division;

            if (capacitacionService.AccesoPermitidoPrivilegioDivision(division))
            {
                return View();
            }
            else
            {
                return View("ErrorPermisoVista");
            }
        }

        [HttpPost]
        public ActionResult ObtenerAreasPorCC(List<string> ccsCplan, List<string> ccsArr)
        {
            return Json(capacitacionService.ObtenerAreasPorCC(ccsCplan, ccsArr), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CargarDatosMatrizNecesidades(List<string> ccsCplan, List<string> ccsArr, List<string> departamentosIDs, List<ClasificacionCursoEnum> clasificaciones)
        {
            var jsonResponse = new JsonResult
            {
                Data = capacitacionService.CargarDatosMatrizNecesidades(ccsCplan, ccsArr, departamentosIDs, clasificaciones),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };

            return jsonResponse;
        }

        [HttpPost]
        public ActionResult CargarDatosSeccionMatriz(List<string> ccsCplan, List<string> ccsArr, List<string> departamentosIDs, List<ClasificacionCursoEnum> clasificaciones, SeccionMatrizEnum seccion)
        {
            var jsonResponse = new JsonResult
            {
                Data = capacitacionService.CargarDatosSeccionMatriz(ccsCplan, ccsArr, departamentosIDs, clasificaciones, seccion),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };

            return jsonResponse;
        }

        [HttpGet]
        public MemoryStream DescargarExcelPersonalActivo()
        {
            var resultadoTupla = capacitacionService.DescargarExcelPersonalActivo();

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
                //return View(RUTA_VISTA_ERROR_DESCARGA);
                return null;
            }
        }

        public ActionResult GetFormatoCambioID(int claveEmpleado)
        {
            return Json(capacitacionService.GetFormatoCambioID(claveEmpleado), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Privilegios
        // GET: Administrativo/Capacitacion/Privilegios
        public ActionResult Privilegios(int division)
        {
            vSesiones.sesionDivisionActual = division;

            if (capacitacionService.AccesoPermitidoPrivilegioDivision(division))
            {
                return View();
            }
            else
            {
                return View("ErrorPermisoVista");
            }
        }
        public ActionResult _privilegios()
        {
            return PartialView();
        }
        tblS_CapacitacionEmpleadoPrivilegio getPrivilegioActual()
        {
            if (capacitacionService == null)
            {
                capacitacionService = new CapacitacionFactoryService().GetCapacitacionService();
            }
            return capacitacionService.getPrivilegioActual();
        }
        public bool esCreadorCurso()
        {
            var privilegio = getPrivilegioActual();
            var lstCreador = new List<PrivilegioEnum>() 
            {
                PrivilegioEnum.Administrador,
                PrivilegioEnum.ControlDocumentos
            };
            return lstCreador.Any(p => p == privilegio.idPrivilegio);
        }
        public bool PuedeEliminarCursos()
        {
            var privilegio = getPrivilegioActual();
            var lstCreador = new List<PrivilegioEnum> { PrivilegioEnum.Administrador };
            return lstCreador.Any(p => p == privilegio.idPrivilegio);
        }
        public bool PuedeEliminarControlAsistencia()
        {
            var privilegio = getPrivilegioActual();
            var lstCreador = new List<PrivilegioEnum> { PrivilegioEnum.Administrador };
            return lstCreador.Any(p => p == privilegio.idPrivilegio);
        }
        public bool esControlAsistencia()
        {
            var privilegio = getPrivilegioActual();
            var lstCtrAsistencia = new List<PrivilegioEnum>() 
            {
                PrivilegioEnum.Administrador,
                PrivilegioEnum.ControlDocumentos,
                PrivilegioEnum.Instructor
            };
            return lstCtrAsistencia.Any(p => p == privilegio.idPrivilegio);
        }
        public bool checarModuloCapacitacionOperativa()
        {
            return vSesiones.sesionSistemaActual == 18;
        }
        public bool esDashboard()
        {
            var privilegio = getPrivilegioActual();
            var lstDashboard = new List<PrivilegioEnum>() 
            {
                PrivilegioEnum.Administrador,
                PrivilegioEnum.Visor,
                PrivilegioEnum.ControlDocumentos,
                PrivilegioEnum.Instructor
            };
            return lstDashboard.Any(p => p == privilegio.idPrivilegio);
        }
        public bool esAdministrador()
        {
            var privilegio = getPrivilegioActual();
            return privilegio.idPrivilegio == PrivilegioEnum.Administrador;
        }
        public ActionResult ObtenerEmpleadosPrivilegios(BusqPrivilegiosDTO busq)
        {
            var result = capacitacionService.ObtenerEmpleadosPrivilegios(busq);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult guardarEmpleadosPrivilegios(List<tblS_CapacitacionEmpleadoPrivilegio> lst)
        {
            var result = new Dictionary<string, object>();
            try
            {
                if (lst != null && lst.All(cve => cve.idUsuario > 0))
                {
                    result = capacitacionService.guardarEmpleadosPrivilegios(lst);
                }
            }
            catch (Exception o_O)
            {
                result.Add(MESSAGE, o_O.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getCboPrivilegios()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cbo = EnumExtensions.ToCombo<PrivilegioEnum>();
                var esSuccess = cbo.Count > 0;
                if (esSuccess)
                {
                    result.Add(ITEMS, cbo);
                }
                result.Add(SUCCESS, esSuccess);
            }
            catch (Exception o_O)
            {
                result.Add(MESSAGE, o_O.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Catálogos
        public ActionResult GetTipoPuestoCombo()
        {
            result.Add(ITEMS, GlobalUtils.ParseEnumToCombo<PuestoAutorizanteEnum>());
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRelacionesCCAutorizantes()
        {
            result = capacitacionService.getRelacionesCCAutorizantes();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarNuevaRelacionCCAutorizante(RelacionCCAutorizanteDTO relacion)
        {
            result = capacitacionService.guardarNuevaRelacionCCAutorizante(relacion);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarRelacionCCAutorizante(RelacionCCAutorizanteDTO relacion)
        {
            result = capacitacionService.editarRelacionCCAutorizante(relacion);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarRelacionCCAutorizante(RelacionCCAutorizanteDTO relacion)
        {
            result = capacitacionService.eliminarRelacionCCAutorizante(relacion);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUsuarioPorClave(int claveEmpleado)
        {
            result = capacitacionService.getUsuarioPorClave(claveEmpleado);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRelacionesCCDepartamentoRazonSocial()
        {
            result = capacitacionService.getRelacionesCCDepartamentoRazonSocial();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarNuevaRelacionCCDepartamentoRazonSocial(List<RelacionCCDepartamentoRazonSocialDTO> relaciones)
        {
            result = capacitacionService.guardarNuevaRelacionCCDepartamentoRazonSocial(relaciones);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarRelacionCCDepartamentoRazonSocial(RelacionCCDepartamentoRazonSocialDTO relacion)
        {
            result = capacitacionService.editarRelacionCCDepartamentoRazonSocial(relacion);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarRelacionCCDepartamentoRazonSocial(RelacionCCDepartamentoRazonSocialDTO relacion)
        {
            result = capacitacionService.eliminarRelacionCCDepartamentoRazonSocial(relacion);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDepartamentosCombo()
        {
            result = capacitacionService.getDepartamentosCombo();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult AutorizadosPersonalAutorizado(int division)
        {
            vSesiones.sesionDivisionActual = division;

            if (capacitacionService.AccesoPermitidoPrivilegioDivision(division))
            {
                return View();
            }
            else
            {
                return View("ErrorPermisoVista");
            }
        }

        public ActionResult DashboardPersonalAutorizado(int division)
        {
            vSesiones.sesionDivisionActual = division;

            if (capacitacionService.AccesoPermitidoPrivilegioDivision(division))
            {
                return View();
            }
            else
            {
                return View("ErrorPermisoVista");
            }
        }

        public ActionResult CrearPersonalAutorizado(int division)
        {
            vSesiones.sesionDivisionActual = division;

            if (capacitacionService.AccesoPermitidoPrivilegioDivision(division))
            {
                return View();
            }
            else
            {
                return View("ErrorPermisoVista");
            }
        }

        public ActionResult PromedioEvaluaciones()
        {
            return View();
        }

        public ActionResult CambiosCategoria()
        {
            return View();
        }

        public ActionResult HorasAdiestramiento()
        {
            return View();
        }

        public ActionResult HorasHombreCapacitacion()
        {
            return View();
        }

        public ActionResult PlanCapacitacionMenu(int division)
        {
            vSesiones.sesionDivisionActual = division;

            if (capacitacionService.AccesoPermitidoPrivilegioDivision(division))
            {
                return View();
            }
            else
            {
                return View("ErrorPermisoVista");
            }
        }

        public ActionResult AdministracionInstructores()
        {
            return View();
        }

        public ActionResult PlanCapacitacion()
        {
            return View();
        }

        public ActionResult DeteccionNecesidades(int division)
        {
            vSesiones.sesionDivisionActual = division;

            if (capacitacionService.AccesoPermitidoPrivilegioDivision(division))
            {
                return View();
            }
            else
            {
                return View("ErrorPermisoVista");
            }
        }

        public ActionResult CompetenciasOperativas(int division)
        {
            vSesiones.sesionDivisionActual = division;

            if (capacitacionService.AccesoPermitidoPrivilegioDivision(division))
            {
                return View();
            }
            else
            {
                return View("ErrorPermisoVista");
            }
        }

        public ActionResult IndicadorHorasHombre(int division)
        {
            vSesiones.sesionDivisionActual = division;

            if (capacitacionService.AccesoPermitidoPrivilegioDivision(division))
            {
                return View();
            }
            else
            {
                return View("ErrorPermisoVista");
            }
        }

        public ActionResult CiclosTrabajo()
        {
            return View();
        }

        public ActionResult DeteccionesPrimarias()
        {
            return View();
        }

        public ActionResult Recorridos()
        {
            return View();
        }

        public ActionResult FactorCapacitacion()
        {
            return View();
        }

        public ActionResult GetRazonSocialCombo()
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = capacitacionService.getRazonSocialCombo();

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

        public MemoryStream CrearExcelRelacionCursosPuestos()
        {
            var stream = capacitacionService.crearExcelRelacionCursosPuestos();

            if (stream != null)
            {
                this.Response.Clear();
                this.Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                this.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", "Reporte Relacion Cursos Puestos.xlsx"));
                this.Response.BinaryWrite(stream.ToArray());
                this.Response.End();

                return stream;
            }
            else
            {
                return null;
            }
        }

        public ActionResult ObtenerComboCursos()
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = capacitacionService.obtenerComboCursos();

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

        public ActionResult GuardarCargaMasivaControlAsistencia()
        {
            if (Request.Files.Count > 0)
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase archivo = Request.Files[i];
                    result = capacitacionService.guardarCargaMasivaControlAsistencia(archivo);
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarCargaMasivaRelacionCursosPuestosAutorizacion()
        {
            if (Request.Files.Count > 0)
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase archivo = Request.Files[i];
                    result = capacitacionService.guardarCargaMasivaRelacionCursosPuestosAutorizacion(archivo);
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult ImprimirReporteGeneralAutorizacion(string cc, int curso, int estatus)
        //{
        //    var result = new Dictionary<string, object>();

        //    try
        //    {
        //        var data = capacitacionService.imprimirReporteGeneralAutorizacion(cc, curso, estatus);

        //        result.Add(ITEMS, data);
        //        result.Add(SUCCESS, true);
        //    }
        //    catch (Exception e)
        //    {
        //        result.Add(MESSAGE, e.Message);
        //        result.Add(SUCCESS, false);
        //    }

        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult ObtenerComboCCAmbasEmpresas()
        {
            return Json(capacitacionService.ObtenerComboCCAmbasEmpresas(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ObtenerComboCCMigracion()
        {
            return Json(capacitacionService.ObtenerComboCCMigracion(), JsonRequestBehavior.AllowGet);
        }

        public int privilegioCapacitacion()
        {
            var privilegio = getPrivilegioActual();

            return privilegio != null ? (int)privilegio.idPrivilegio : 0;
        }

        #region Personal Autorizado
        public ActionResult GetListasAutorizacion(List<int> listaCursos, List<tblS_CapacitacionListaAutorizacionCC> listaCC)
        {
            return Json(capacitacionService.getListasAutorizacion(listaCursos, listaCC), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListasAutorizacionCombo()
        {
            return Json(capacitacionService.getListasAutorizacionCombo(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarListaAutorizacion(tblS_CapacitacionListaAutorizacion listaAutorizacion, List<tblS_CapacitacionListaAutorizacionRFC> listaRFC, List<tblS_CapacitacionListaAutorizacionCC> listaCentrosCosto)
        {
            return Json(capacitacionService.guardarListaAutorizacion(listaAutorizacion, listaRFC, listaCentrosCosto), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarListaAutorizacion(tblS_CapacitacionListaAutorizacion listaAutorizacion, List<tblS_CapacitacionListaAutorizacionRFC> listaRFC, List<tblS_CapacitacionListaAutorizacionCC> listaCentrosCosto)
        {
            return Json(capacitacionService.editarListaAutorizacion(listaAutorizacion, listaRFC, listaCentrosCosto), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarListaAutorizacion(int listaAutorizacionID)
        {
            return Json(capacitacionService.eliminarListaAutorizacion(listaAutorizacionID), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListaAutorizacionByID(int listaAutorizacionID)
        {
            var json = Json(capacitacionService.getListaAutorizacionByID(listaAutorizacionID), JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        public ActionResult GetAutorizanteEnkontrolAutocomplete(string term)
        {
            return Json(capacitacionService.getAutorizanteEnkontrolAutocomplete(term), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarInformacionAutorizados(ListaAutorizacionDTO listaAutorizacion)
        {
            return Json(capacitacionService.guardarInformacionAutorizados(listaAutorizacion), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarDashboardPersonalAutorizado(FiltrosDashboardPersonalAutorizadoDTO filtros)
        {
            return Json(capacitacionService.cargarDashboardPersonalAutorizado(filtros), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCorreosListaAutorizacion(int listaAutorizacionID)
        {
            return Json(capacitacionService.getCorreosListaAutorizacion(listaAutorizacionID), JsonRequestBehavior.AllowGet);
        }

        public ActionResult SetCorreosListaAutorizacion(List<string> listaCorreos)
        {
            var result = new Dictionary<string, object>();

            try
            {
#if DEBUG
                listaCorreos = new List<string> { "oscar.valencia@construplan.com.mx" };
#endif

                Session["listaCorreosListaAutorizacion"] = listaCorreos;

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Detección de Necesidades
        #region Ciclos de Trabajo
        public ActionResult GetCiclosTrabajoCombo()
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = capacitacionService.getCiclosTrabajoCombo();

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

        public ActionResult GuardarNuevoCiclo(tblS_CapacitacionDNCicloTrabajo ciclo, List<tblS_CapacitacionDNCicloTrabajoAreas> listaAreas, List<tblS_CapacitacionDNCicloTrabajoCriterio> listaCriterios)
        {
            return Json(capacitacionService.guardarNuevoCiclo(ciclo, listaAreas, listaCriterios), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCicloByID(int cicloID)
        {
            return Json(capacitacionService.getCicloByID(cicloID), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarRegistroCiclo(tblS_CapacitacionDNCicloTrabajoRegistro registroCiclo, List<tblS_CapacitacionDNCicloTrabajoRegistroRevisiones> listaRevisiones, List<tblS_CapacitacionDNCicloTrabajoRegistroPropuestas> listaPropuestas, List<tblS_CapacitacionDNCicloTrabajoRegistroAreas> listaAreas, List<int> areasSeguimiento, List<int> interesados)
        {
            return Json(capacitacionService.guardarRegistroCiclo(registroCiclo, listaRevisiones, listaPropuestas, listaAreas, areasSeguimiento, interesados), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRegistrosCiclos(FiltrosRegistrosCiclo filtros)
        {
            return Json(capacitacionService.getRegistrosCiclos(filtros), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListaSeguimiento(List<string> listaCC, TipoSeguimientoEnum tipoSeguimiento, DateTime fechaInicio, DateTime fechaFin)
        {
            return Json(capacitacionService.getListaSeguimiento(listaCC, tipoSeguimiento, fechaInicio, fechaFin), JsonRequestBehavior.AllowGet);
        }

        public MemoryStream DescargarExcelSeguimientoAcciones()
        {
            var stream = capacitacionService.DescargarExcelSeguimientoAcciones();

            if (stream != null)
            {
                this.Response.Clear();
                this.Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                this.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", "Reporte Seguimiento Acciones.xlsx"));
                this.Response.BinaryWrite(stream.ToArray());
                this.Response.End();

                return stream;
            }
            else
            {
                return null;
            }
        }

        public ActionResult GuardarSeguimientoAcciones(List<HttpPostedFileBase> evidencias)
        {
            var capturaEvidencias = (JsonConvert.DeserializeObject<tblS_CapacitacionDNCicloTrabajoRegistro[]>(Request.Form["capturaEvidencias"], new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" })).ToList();
            var capturaEvaluaciones = (JsonConvert.DeserializeObject<tblS_CapacitacionDNCicloTrabajoRegistro[]>(Request.Form["capturaEvaluaciones"], new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" })).ToList();

            return Json(capacitacionService.guardarSeguimientoAcciones(capturaEvidencias, evidencias, capturaEvaluaciones), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarSeguimientoPropuestas(List<HttpPostedFileBase> evidencias)
        {
            var capturaEvidencias = (JsonConvert.DeserializeObject<tblS_CapacitacionDNCicloTrabajoRegistroPropuestas[]>(Request.Form["capturaEvidencias"], new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" })).ToList();
            var capturaEvaluaciones = (JsonConvert.DeserializeObject<tblS_CapacitacionDNCicloTrabajoRegistroPropuestas[]>(Request.Form["capturaEvaluaciones"], new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" })).ToList();

            return Json(capacitacionService.guardarSeguimientoPropuestas(capturaEvidencias, evidencias, capturaEvaluaciones), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarDatosArchivoEvidenciaSeguimientoAcciones(int id)
        {
            var resultado = new Dictionary<string, object>();

            resultado = capacitacionService.cargarDatosArchivoEvidenciaSeguimientoAcciones(id);

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

        public ActionResult DescargarArchivoEvidenciaAccion(int id)
        {
            var resultadoTupla = capacitacionService.descargarArchivoEvidenciaAccion(id);

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

        public ActionResult CargarDatosArchivoEvidenciaSeguimientoPropuestas(int id)
        {
            var resultado = new Dictionary<string, object>();

            resultado = capacitacionService.cargarDatosArchivoEvidenciaSeguimientoPropuestas(id);

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

        public ActionResult DescargarArchivoEvidenciaPropuesta(int id)
        {
            var resultadoTupla = capacitacionService.descargarArchivoEvidenciaPropuesta(id);

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

        public ActionResult CargarDashboardCiclos(List<string> listaCC, List<AreaDTO> listaAreas, DateTime fechaInicio, DateTime fechaFin)
        {
            return Json(capacitacionService.cargarDashboardCiclos(listaCC, listaAreas, fechaInicio, fechaFin), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRegistroCicloTrabajoByID(int id)
        {
            return Json(capacitacionService.getRegistroCicloTrabajoByID(id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTablaCicloTrabajo()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstCicloTrabajo = capacitacionService.GetTablaCicloTrabajo();
                result.Add("lstCicloTrabajo", lstCicloTrabajo);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetTablaCriterioTrabajo(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstCicloTrabajoCriterio = capacitacionService.GetTablaCriterioTrabajo(id);
                result.Add("lstCicloTrabajoCriterio", lstCicloTrabajoCriterio);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarCicloTrabajo(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var EliminarCicloTrabajo = capacitacionService.EliminarCicloTrabajo(id);
                result.Add("EliminarCicloTrabajo", EliminarCicloTrabajo);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EliminarRegistroCicloTrabajo(tblS_CapacitacionDNCicloTrabajoRegistro ciclo)
        {
            return Json(capacitacionService.EliminarRegistroCicloTrabajo(ciclo), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarCicloTrabajo(CicloTrabajoDTO parametros, List<tblS_CapacitacionDNCicloTrabajoCriterio> criterio, List<tblS_CapacitacionDNCicloTrabajoAreas> lstAreass)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var EditarCicloTrabajo = capacitacionService.EditarCicloTrabajo(parametros, criterio, lstAreass);
                result.Add("EditarCicloTrabajo", EditarCicloTrabajo);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult llenarCorreos(int _IdUsuario)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var obj = capacitacionService.llenarCorreos(_IdUsuario);
                result.Add(ITEMS, obj.Select(x => new { Value = x.id, Text = x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno }).OrderBy(x => x.Text));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult enviarCorreoRecorrido(int recorridoID, List<int> usuarios)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var downloadPDF = (List<Byte[]>)Session["downloadPDF"];

                var obj = capacitacionService.enviarCorreoRecorrido(recorridoID, usuarios, downloadPDF);
                if (obj.Count() == 0)
                {
                    result.Add(SUCCESS, true);
                }
                else
                {
                    result.Add("obj", string.Join("<br/>", obj.ToList()));
                    result.Add(SUCCESS, false);
                }

                Session["downloadPDF"] = null;


            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getListaDepartamientos(int listaAutorizacionID)
        {
            return Json(capacitacionService.getListaDepartamientos(listaAutorizacionID), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAreaSeguimiento()
        {
            return Json(capacitacionService.GetAreaSeguimiento(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarDatosSeccionSeguimientoAcciones(List<string> listaCC, TipoSeguimientoEnum tipoSeguimiento, DateTime fechaInicio, DateTime fechaFin, List<int> listaAreasSeguimientoHistorial, SeccionSeguimientoAccionesEnum seccion)
        {
            var jsonResponse = new JsonResult
            {
                Data = capacitacionService.CargarDatosSeccionSeguimientoAcciones(listaCC, tipoSeguimiento, fechaInicio, fechaFin, listaAreasSeguimientoHistorial, seccion),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };

            return jsonResponse;
        }
        
        #region CARGAR EVIDENCIAS EN RECORRIDOS
        public ActionResult GuardarEvidenciaRecorrido(List<HttpPostedFileBase> lstArchivos, int FK_Recorrido)
        {
            return Json(capacitacionService.GuardarEvidenciaRecorrido(lstArchivos, FK_Recorrido), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEvidenciasRecorrido(int FK_Recorrido)
        {
            return Json(capacitacionService.GetEvidenciasRecorrido(FK_Recorrido), JsonRequestBehavior.AllowGet);
        }

        public ActionResult VisualizarEvidenciaRecorrido(int FK_Recorrido)
        {
            var json = Json(capacitacionService.VisualizarEvidenciaRecorrido(FK_Recorrido), JsonRequestBehavior.AllowGet);

            json.MaxJsonLength = int.MaxValue;

            return json;
        }

        public ActionResult EliminarEvidenciaRecorrido(int idArchivo)
        {
            return Json(capacitacionService.EliminarEvidenciaRecorrido(idArchivo), JsonRequestBehavior.AllowGet);
        }
        #endregion

        public JsonResult CargarCiclosRequeridos()
        {
            return Json(capacitacionService.CargarCiclosRequeridos(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GuardarCiclosRequeridos(List<tblS_CapacitacionDNCiclosRequeridos> listaCiclosRequeridos)
        {
            return Json(capacitacionService.GuardarCiclosRequeridos(listaCiclosRequeridos), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Detecciones Primarias
        public ActionResult GetRegistrosDeteccionesPrimarias(List<string> listaCC, List<AreaDTO> listaAreas, DateTime fecha)
        {
            return Json(capacitacionService.getRegistrosDeteccionesPrimarias(listaCC, listaAreas, fecha), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarDeteccionPrimaria(tblS_CapacitacionDNDeteccionPrimaria deteccionPrimaria, List<tblS_CapacitacionDNDeteccionPrimariaNecesidad> listaNecesidades, List<tblS_CapacitacionDNDeteccionPrimariaAreas> listaAreas)
        {
            return Json(capacitacionService.guardarDeteccionPrimaria(deteccionPrimaria, listaNecesidades, listaAreas), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Recorridos
        public ActionResult GetRegistrosRecorridos(List<string> listaCC, List<AreaDTO> listaAreas, DateTime fechaInicio, DateTime fechaFin, int realizador)
        {
            return Json(capacitacionService.getRegistrosRecorridos(listaCC, listaAreas, fechaInicio, fechaFin, realizador), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarNuevoRecorrido(List<HttpPostedFileBase> evidencias)
        {
            var recorrido = JsonConvert.DeserializeObject<tblS_CapacitacionDNRecorrido>(Request.Form["recorrido"], new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            var listaHallazgos = (JsonConvert.DeserializeObject<RecorridoHallazgoDTO[]>(Request.Form["listaHallazgos"], new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" })).ToList();
            var listaAreas = (JsonConvert.DeserializeObject<tblS_CapacitacionDNRecorridoAreas[]>(Request.Form["listaAreas"], new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" })).ToList();

            return Json(capacitacionService.guardarNuevoRecorrido(recorrido, listaHallazgos, listaAreas, evidencias), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarRecorrido(List<HttpPostedFileBase> evidencias)
        {
            var recorrido = JsonConvert.DeserializeObject<tblS_CapacitacionDNRecorrido>(Request.Form["recorrido"], new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            var listaHallazgos = (JsonConvert.DeserializeObject<RecorridoHallazgoDTO[]>(Request.Form["listaHallazgos"], new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" })).ToList();
            var listaAreas = (JsonConvert.DeserializeObject<tblS_CapacitacionDNRecorridoAreas[]>(Request.Form["listaAreas"], new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" })).ToList();

            return Json(capacitacionService.editarRecorrido(recorrido, listaHallazgos, listaAreas, evidencias), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarRegistroRecorrido(int recorrido_id)
        {
            return Json(capacitacionService.EliminarRegistroRecorrido(recorrido_id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRecorridoByID(int recorridoID)
        {
            return Json(capacitacionService.getRecorridoByID(recorridoID), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarSeguimientoRecorrido(List<tblS_CapacitacionDNRecorridoHallazgo> listaSeguimiento)
        {
            return Json(capacitacionService.guardarSeguimientoRecorrido(listaSeguimiento), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarDashboardRecorridos(List<string> listaCC, List<AreaDTO> listaAreas, DateTime fechaInicio, DateTime fechaFin, int realizador)
        {
            return Json(capacitacionService.cargarDashboardRecorridos(listaCC, listaAreas, fechaInicio, fechaFin, realizador), JsonRequestBehavior.AllowGet);
        }

        public ActionResult VerificarCorreosValidos(List<int> listaUsuariosID)
        {
            return Json(capacitacionService.VerificarCorreosValidos(listaUsuariosID), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region Competencias Operativas
        public ActionResult GetPromedioEvaluaciones(List<string> listaCC, List<AreaDTO> listaAreas, DateTime fecha)
        {
            return Json(capacitacionService.getPromedioEvaluaciones(listaCC, listaAreas, fecha), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Indicador Hrs-Hombre
        public ActionResult GetEquiposCombo()
        {
            result = capacitacionService.getEquiposCombo();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEquipoAdiestramientoCombo()
        {
            return Json(capacitacionService.GetEquipoAdiestramientoCombo(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEmpleadoPorClave(int claveEmpleado)
        {
            return Json(capacitacionService.getEmpleadoPorClave(claveEmpleado), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarHorasAdiestramiento(List<string> listaCC, DateTime fechaInicial, DateTime fechaFinal, List<int> equipos, List<int> equiposGrafica, List<int> actividades)
        {
            return Json(capacitacionService.cargarHorasAdiestramiento(listaCC, fechaInicial, fechaFinal, equipos, equiposGrafica, actividades), JsonRequestBehavior.AllowGet);
        }

        public ActionResult cargarHorasAdiestramientoColaborador(List<string> listaCC, DateTime fechaInicial, DateTime fechaFinal, List<int> equipos, List<int> actividades, int colaborador)
        {
            return Json(capacitacionService.cargarHorasAdiestramientoColaborador(listaCC, fechaInicial, fechaFinal, equipos, actividades, colaborador), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarNuevoColaboradorCapacitacion(tblS_CapacitacionIHHColaboradorCapacitacion colaboradorCapacitacion, List<int> interesados)
        {
            return Json(capacitacionService.guardarNuevoColaboradorCapacitacion(colaboradorCapacitacion, interesados), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarColaboradorCapacitacion(tblS_CapacitacionIHHColaboradorCapacitacion colaboradorCapacitacion)
        {
            return Json(capacitacionService.EditarColaboradorCapacitacion(colaboradorCapacitacion), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarColaboradorCapacitacion(tblS_CapacitacionIHHColaboradorCapacitacion colaboradorCapacitacion)
        {
            return Json(capacitacionService.EliminarColaboradorCapacitacion(colaboradorCapacitacion), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetInfoColaboradorCapacitacion(int colaboradorCapacitacionID)
        {
            return Json(capacitacionService.getInfoColaboradorCapacitacion(colaboradorCapacitacionID), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetInfoColaboradorCapacitacionActividad(int colaboradorCapacitacionID)
        {
            return Json(capacitacionService.GetInfoColaboradorCapacitacionActividad(colaboradorCapacitacionID), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarNuevoControlHoras(List<tblS_CapacitacionIHHControlHoras> listaControlHoras)
        {
            return Json(capacitacionService.guardarNuevoControlHoras(listaControlHoras), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GuardarNuevoControlActividad(List<tblS_CapacitacionIHHControlActividad> listaControlActividad)
        {
            return Json(capacitacionService.GuardarNuevoControlActividad(listaControlActividad));
        }

        public ActionResult GuardarLiberados(List<HttpPostedFileBase> archivos)
        {
            var captura = (JsonConvert.DeserializeObject<tblS_CapacitacionIHHColaboradorCapacitacion[]>(Request.Form["captura"], new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" })).ToList();

            return Json(capacitacionService.guardarLiberados(captura, archivos), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarLiberacionAdministrador(List<tblS_CapacitacionIHHColaboradorCapacitacion> captura)
        {
            return Json(capacitacionService.GuardarLiberacionAdministrador(captura), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarDatosArchivoSoporteAdiestramiento(int id)
        {
            var resultado = new Dictionary<string, object>();

            resultado = capacitacionService.cargarDatosArchivoSoporteAdiestramiento(id);

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

        public ActionResult DescargarArchivoSoporteAdiestramiento(int id)
        {
            var resultadoTupla = capacitacionService.descargarArchivoSoporteAdiestramiento(id);

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

        public ActionResult descargarGlobal(int id)
        {

            var resultadoTupla = capacitacionService.descargarGlobal(id, (string)Session["img1"], (string)Session["img2"]);
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
                return null;
            }

            //var resultadoTupla = capacitacionService.DescargarExpedienteEmpleado(claveEmpleado, licencia[0]);

            //if (resultadoTupla != null)
            //{
            //    string nombreArchivo = resultadoTupla.Item2;
            //    string tipo = MimeMapping.GetMimeMapping(nombreArchivo);

            //    var fileStreamResult = new FileStreamResult(resultadoTupla.Item1, tipo);
            //    fileStreamResult.FileDownloadName = nombreArchivo;

            //    return fileStreamResult;
            //}
            //else
            //{
            //    return View("ErrorDescarga");
            //}
        }

        public ActionResult cargarImgTemp1(string gfx1, string gfx2)
        {
            Session["img1"] = gfx1;
            Session["img2"] = gfx2;

            result.Add(SUCCESS, true);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CargarInformacionHorasHombre(HorasHombreDTO filtros)
        {
            return Json(capacitacionService.CargarInformacionHorasHombre(filtros), JsonRequestBehavior.AllowGet);
        }

        public ActionResult postObtenerTablaHorasHombre(HorasHombreDTO parametros)
        {
            try
            {
                result.Add(ITEMS, capacitacionService.postObtenerTablaHorasHombre(parametros, true));
                result.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, false);
                throw;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult obtenerInputPromedios(HorasHombreDTO parametros)
        {
            try
            {
                result.Add(ITEMS, capacitacionService.obtenerInputPromedios(parametros));
                result.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, false);
                throw;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearReporteHorasHombres(HorasHombreDTO parametros)
        {
            try
            {
                Session["objHorasHombreCapacitacion"] = capacitacionService.obtenerInputPromedios(parametros);
                var lstHorasHombre = capacitacionService.postObtenerTablaHorasHombre(parametros, false);
                List<HorasHombreReporteDTO> lstHorasCapacitacion = new List<HorasHombreReporteDTO>();
                HorasHombreReporteDTO obj = new HorasHombreReporteDTO();

                foreach (var item in lstHorasHombre)
                {
                    obj = new HorasHombreReporteDTO();
                    obj.cc = item.centrocosto;
                    obj.Descripcion = item.descripcion;
                    obj.TotalPersonal = item.totalPersonal;
                    obj.objGlobalCap = item.lstGlobal.HrsCap;
                    obj.objEneroCap = item.lstEnero.HrsCap;
                    obj.objFebreroCap = item.lstFebrero.HrsCap;
                    obj.objMarzoCap = item.lstMarzo.HrsCap;
                    obj.objAbrilCap = item.lstAbril.HrsCap;
                    obj.objMayoCap = item.lstMayo.HrsCap;
                    obj.objJunioCap = item.lstJunio.HrsCap;
                    obj.objJulioCap = item.lstJulio.HrsCap;
                    obj.objAgostoCap = item.lstAgosto.HrsCap;
                    obj.objSeptiembreCap = item.lstSeptiembre.HrsCap;
                    obj.objOctubreCap = item.lstOctubre.HrsCap;
                    obj.objNoviembreCap = item.lstNoviembre.HrsCap;
                    obj.objDiciembreCap = item.lstDiciembre.HrsCap;
                    obj.objGlobalTrab = item.lstGlobal.HrsTrab;
                    obj.objEneroTrab = item.lstEnero.HrsTrab;
                    obj.objFebreroTrab = item.lstFebrero.HrsTrab;
                    obj.objMarzoTrab = item.lstMarzo.HrsTrab;
                    obj.objAbrilTrab = item.lstAbril.HrsTrab;
                    obj.objMayoTrab = item.lstMayo.HrsTrab;
                    obj.objJunioTrab = item.lstJunio.HrsTrab;
                    obj.objJulioTrab = item.lstJulio.HrsTrab;
                    obj.objAgostoTrab = item.lstAgosto.HrsTrab;
                    obj.objSeptiembreTrab = item.lstSeptiembre.HrsTrab;
                    obj.objOctubreTrab = item.lstOctubre.HrsTrab;
                    obj.objNoviembreTrab = item.lstNoviembre.HrsTrab;
                    obj.objDiciembreTrab = item.lstDiciembre.HrsTrab;

                    lstHorasCapacitacion.Add(obj);
                }
                Session["lstHorasHombreCapacitacion"] = lstHorasCapacitacion;
                result.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, false);
                throw;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult creaExcelito(HorasHombreDTO parametros)
        {
            Session["parametros"] = parametros;
            result.Add(SUCCESS, true);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public MemoryStream crearExcelHorasHombreCapacitacion()
        {
            HorasHombreDTO parametros = (HorasHombreDTO)Session["parametros"];
            var stream = capacitacionService.crearExcelHorasHombreCapacitacion(parametros);

            if (stream != null)
            {
                this.Response.Clear();
                this.Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                this.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", "Reporte Insumos.xlsx"));
                this.Response.BinaryWrite(stream.ToArray());
                this.Response.End();

                Session["parametros"] = null;
                return stream;
            }
            else
            {
                return null;
            }
        }

        public JsonResult GetActividades()
        {
            return Json(capacitacionService.GetActividades(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetInteresados()
        {
            return Json(capacitacionService.GetInteresados(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMtto(int colaboradorCapacitacionId)
        {
            return Json(capacitacionService.GetMtto(colaboradorCapacitacionId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMttoDetalle(int clave_empleado)
        {
            return Json(capacitacionService.GetMttoDetalle(clave_empleado), JsonRequestBehavior.AllowGet);
        }

        public MemoryStream DescargarExcelAdiestramientoActividades()
        {
            var stream = capacitacionService.DescargarExcelAdiestramientoActividades();

            if (stream != null)
            {
                this.Response.Clear();
                this.Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                this.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", "Reporte Colaboradores Adiestramiento Actividad.xlsx"));
                this.Response.BinaryWrite(stream.ToArray());
                this.Response.End();

                return stream;
            }
            else
            {
                return null;
            }
        }

        public MemoryStream DescargarExcelAdiestramientoHoras()
        {
            var stream = capacitacionService.DescargarExcelAdiestramientoHoras();

            if (stream != null)
            {
                this.Response.Clear();
                this.Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                this.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", "Reporte Colaboradores Adiestramiento Horas.xlsx"));
                this.Response.BinaryWrite(stream.ToArray());
                this.Response.End();

                return stream;
            }
            else
            {
                return null;
            }
        }
        #endregion


        #region CAMBIOS DE CATEGORIA
        private delegate bool EsMismoUsuarioCC(int usuarioCapturoID);
        public ActionResult TablaFormatosPendientes(FiltrosCapacitacionDTO parametros)
        {
            var result = new Dictionary<string, object>();
            var lstObjFormatosPendientes = new List<CatFormatoCambioDTO>();
            var lstObjEmpleadoCambio = new List<resultCapacitacionDTO>();
            try
            {
                if (parametros.CC == null)
                    parametros.CC = "";
                if (parametros.tipo == null)
                    parametros.tipo = "";
                //switch (parametros.estado)
                //{
                //    case "Todos":
                //        intEstado = 1;
                //        break;
                //    case "Pendiente":
                //        intEstado = 2;
                //        break;
                //    case "Aprobado":
                //        intEstado = 3;
                //        break;
                //    case "Rechazado":
                //        intEstado = 4;
                //        break;
                //}
                lstObjEmpleadoCambio = capacitacionService.TablaFormatosPendientes(parametros);
                int usuarioLog = 3367;
                var listaMostrar = new List<resultCapacitacionDTO>();
                var ud = new UsuarioDAO();
                var isVerTodo = ud.getViewAction(vSesiones.sesionCurrentView, "TodoFormatoCambio");
                var rh = ud.getViewAction(vSesiones.sesionCurrentView, "VerTodoFormato");
                var rhEditar = ud.getViewAction(vSesiones.sesionCurrentView, "Editar");

                foreach (var formatoCambio in lstObjEmpleadoCambio)
                {
                    var listAp = capacitacionService.getAutorizacion(formatoCambio.id);

                    EsMismoUsuarioCC esMismoUsuarioCC = authFormatoCambioFS.EsUsuarioMismoCC;

                    if ((formatoCambio.usuarioCap == usuarioLog) || (isVerTodo) || rh || esMismoUsuarioCC(formatoCambio.usuarioCap))
                    {
                        formatoCambio.editable = true;
                        var isEditarTodo = ud.getViewAction(vSesiones.sesionCurrentView, "EditarTodo");
                        if (isEditarTodo)
                        {
                            formatoCambio.editable = true;
                        }
                        else
                        {
                            foreach (var objAp in listAp)
                            {
                                if (objAp.Estatus || formatoCambio.Rechazado)
                                {
                                    formatoCambio.editable = false;
                                    break;
                                }
                                else if (rhEditar)
                                {
                                    formatoCambio.editable = true;
                                    break;
                                }
                            }
                        }
                        listaMostrar.Add(formatoCambio);
                    }
                    else
                    {
                        var isEditarTodo = ud.getViewAction(vSesiones.sesionCurrentView, "EditarTodo");
                        if (isEditarTodo)
                        {
                            formatoCambio.editable = true;
                            foreach (var objAp in listAp)
                            {
                                if (objAp.Clave_Aprobador == usuarioLog /*&& objAp.Estatus*/)
                                {
                                    listaMostrar.Add(formatoCambio);
                                }
                            }
                        }
                        else
                        {

                            switch (parametros.estado)
                            {
                                case 1:
                                    {
                                        listaMostrar.Add(formatoCambio);
                                    }
                                    break;
                                case 2:
                                    {
                                        var objAp = listAp.Where(x => x.Autorizando && x.Clave_Aprobador.Equals(usuarioLog)).FirstOrDefault();
                                        if (objAp != null)
                                        {
                                            var claseBandera = string.Empty;
                                            setEstado(objAp, out claseBandera);

                                            listaMostrar.Add(formatoCambio);
                                        }
                                    }
                                    break;
                                case 3:
                                    {
                                        var objAp = listAp.Where(x => x.Clave_Aprobador.Equals(usuarioLog)).OrderByDescending(w => w.Orden).FirstOrDefault();

                                        var claseBandera = string.Empty;
                                        setEstado(objAp, out claseBandera);

                                        listaMostrar.Add(formatoCambio);
                                    }
                                    break;
                                case 4:
                                    {
                                        var objAp = listAp.Where(x => x.Clave_Aprobador.Equals(usuarioLog)).OrderByDescending(w => w.Orden).FirstOrDefault();

                                        var claseBandera = string.Empty;
                                        setEstado(objAp, out claseBandera);

                                        listaMostrar.Add(formatoCambio);
                                    }
                                    break;
                            }
                        }
                    }

                }
                result.Add("data", listaMostrar);
                result.Add(SUCCESS, lstObjEmpleadoCambio.Count > 0);
            }
            catch (Exception o_O)
            {
                result.Add("message", o_O.Message);
                result.Add(SUCCESS, false);
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        authEstadoEnum setEstado(tblRH_AutorizacionFormatoCambio auth, out string clase)
        {
            var estado = authEstadoEnum.EnEspera;
            if (auth.Estatus)
            {
                estado = authEstadoEnum.Autorizado;
            }
            else if (auth.Rechazado)
            {
                estado = authEstadoEnum.Rechazado;
            }
            else if (auth.Autorizando && auth.Clave_Aprobador.Equals(vSesiones.sesionUsuarioDTO.id))
            {
                estado = authEstadoEnum.EnTurno;
            }
            clase = ((authEstadoEnum)estado).GetDescription();
            return estado;
        }

        public ActionResult postSubirArchivos(List<HttpPostedFileBase> archivo)
        {
            int id = (JsonConvert.DeserializeObject<int>(Request.Form["id"], new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" })).ParseInt();
            EmpresaEnum empresa = JsonConvert.DeserializeObject<EmpresaEnum>(Request.Form["empresa"], new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });

            result = capacitacionService.postSubirArchivos(id, empresa, archivo);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DescargarArchivoCO(int id)
        {
            var array = capacitacionService.descargarArchivoCO(id);
            string pathExamen = capacitacionService.getFileNameCO(id);

            if (array != null)
            {
                return File(array, System.Net.Mime.MediaTypeNames.Application.Octet, pathExamen);
            }
            else
            {
                return View("ErrorDescarga");
            }

        }

        public ActionResult obtenerArchivoCODescargas(int idFormatoCambio)
        {
            var result = new Dictionary<string, object>();
            try
            {

                result = capacitacionService.obtenerArchivoCODescargas(idFormatoCambio);


                result.Add(SUCCESS, result.Count > 0);
            }
            catch (Exception o_O)
            {
                result.Add("message", o_O.Message);
                result.Add(SUCCESS, false);
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        public ActionResult EliminarArchivosFormatoCambio(int formatoCambio_id)
        {
            return Json(capacitacionService.EliminarArchivosFormatoCambio(formatoCambio_id), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ADMINISTRACION INSTRUCTORES

        public ActionResult GetInstructores()
        {
            try
            {
                result.Add(ITEMS, capacitacionService.GetInstructores());
                result.Add(SUCCESS, "SUCCESS");

            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, "ERROR");
                throw;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetRolesCombo()
        {
            result.Add(ITEMS, capacitacionService.GetRolesCombo());
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PostGuardarInstructor(tblS_Capacitacion_PCAdministracionInstructores parametros, bool AddEdit)
        {
            parametros.division = vSesiones.sesionDivisionActual;
            result.Add(ITEMS, capacitacionService.PostGuardarInstructor(parametros, AddEdit));
            result.Add(SUCCESS, "SUCCESS");
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetInstructoresCombo(string cc)
        {
            result.Add(ITEMS, capacitacionService.GetInstructoresCombo(cc));
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getFechaInicio(string cveEmpleado)
        {
            result.Add(ITEMS, capacitacionService.getFechaInicio(cveEmpleado));
            result.Add(SUCCESS, "SUCCESS");
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCC()
        {
            result.Add(ITEMS, capacitacionService.GetCC());
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ComboGetEnumerables()
        {
            var tresult = GlobalUtils.ParseEnumToCombo<TematicaCursoEnum>();
            result.Add(ITEMS, tresult);
            result.Add(SUCCESS, "SUCCESS");
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EliminarInstructor(int id)
        {
            result.Add(ITEMS, capacitacionService.EliminarInstructor(id));
            result.Add(SUCCESS, "SUCCESS");
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ObtenerCCUnico(string cveEmpleado)
        {
            try
            {
                result.Add(ITEMS, capacitacionService.ObtenerCCUnico(cveEmpleado));
                result.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, false);
                throw;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        //public ActionResult GetEmpleadoPorClave(string claveEmpleado)
        //{
        //    try
        //    {
        //        result.Add(ITEMS, capacitacionService.ObtenerCCUnico(claveEmpleado));
        //        result.Add(SUCCESS, true);
        //    }
        //    catch (Exception ex)
        //    {
        //        result.Add(SUCCESS, false);
        //        throw;
        //    }
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        #endregion

        #region Plan de Capacitación
        public ActionResult CargarCalendarioPlanCapacitacion(string cc, List<TematicaCursoEnum> listaTematicas, int empresa, DateTime mesCalendario)
        {
            return Json(capacitacionService.cargarCalendarioPlanCapacitacion(cc, listaTematicas, empresa, mesCalendario), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTematicaCombo()
        {
            result.Add(ITEMS, (GlobalUtils.ParseEnumToCombo<TematicaCursoEnum>()).Where(x => x.Value > 0).ToList());
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult GetEmpleadoCursosActos(int claveEmpleado)
        {
            return Json(capacitacionService.GetEmpleadoCursosActos(claveEmpleado), JsonRequestBehavior.AllowGet);
        }

        #region Factor Capacitación
        public ActionResult CargarFactorCapacitacion(int division, List<string> listaCentroCosto, DateTime fechaInicial, DateTime fechaFinal)
        {
            return Json(capacitacionService.CargarFactorCapacitacion(division, listaCentroCosto, fechaInicial, fechaFinal), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarFactorCapacitacionDetalle(int division, List<string> listaCentroCosto, DateTime fechaInicial, DateTime fechaFinal)
        {
            return Json(capacitacionService.CargarFactorCapacitacionDetalle(division, listaCentroCosto, fechaInicial, fechaFinal), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarEstadisticas(int division, List<string> listaCentroCosto, int anio, SeccionEstadisticaEnum seccion)
        {
            return Json(capacitacionService.CargarEstadisticas(division, listaCentroCosto, anio, seccion), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarEfectividadCiclosDetalle(int division, List<string> listaCentroCosto, int anio)
        {
            return Json(capacitacionService.CargarEfectividadCiclosDetalle(division, listaCentroCosto, anio), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAniosFactorCapacitacion()
        {
            var anioActual = DateTime.Now.Year;
            var listaAnios = new List<ComboDTO> { 
                new ComboDTO { Value = anioActual, Text = anioActual.ToString() }
            };

            for (int i = 1; i < 4; i++)
            {
                listaAnios.Add(new ComboDTO
                {
                    Value = DateTime.Now.AddYears(i * -1).Year,
                    Text = DateTime.Now.AddYears(i * -1).Year.ToString()
                });
            }

            return Json(new { items = listaAnios }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCentrosCostoDivision(int division)
        {
            return Json(capacitacionService.GetCentrosCostoDivision(division), JsonRequestBehavior.AllowGet);
        }
        public ActionResult fillComboDivision()
        {
            return Json(capacitacionService.fillComboDivision(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillAnios()
        {
            return Json(capacitacionService.FillAnios(), JsonRequestBehavior.AllowGet);
        }
        
        #endregion

        #region CATALOGO FIRMAS INSTRUCTORES
        public ActionResult FirmasInstructores()
        {
            if (capacitacionService.AccesoPermitidoPrivilegioDivision(1))
            {
                return View();
            }
            else
            {
                return View("ErrorPermisoVista");
            }
        }

        public ActionResult GetFirmasInstructores()
        {
            return Json(capacitacionService.GetFirmasInstructores(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CEFirmaInstructor(FirmaInstructorDTO objParamDTO, List<HttpPostedFileBase> lstArchivos)
        {
            return Json(capacitacionService.CEFirmaInstructor(objParamDTO, lstArchivos));
        }

        public ActionResult EliminarFirmaInstructor(FirmaInstructorDTO objParamDTO)
        {
            return Json(capacitacionService.EliminarFirmaInstructor(objParamDTO));
        }

        public ActionResult FillCboUsuarios()
        {
            return Json(capacitacionService.FillCboUsuarios(), JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}