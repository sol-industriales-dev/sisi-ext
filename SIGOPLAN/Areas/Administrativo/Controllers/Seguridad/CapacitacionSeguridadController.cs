using Core.DAO.Administracion.Seguridad;
using Core.DAO.RecursosHumanos.Captura;
using Core.DTO;
using Core.DTO.Administracion.Seguridad.CapacitacionSeguridad;
using Core.DTO.RecursosHumanos;
using Core.DTO.RecursosHumanos.Capacitacion;
using Core.Entity.Administrativo.Seguridad.CapacitacionSeguridad;
using Core.Entity.RecursosHumanos.Captura;
using Core.Enum.Administracion.Seguridad.CapacitacionSeguridad;
using Core.Enum.Multiempresa;
using Core.Enum.Principal;
using Data.DAO.Principal.Usuarios;
using Data.Factory.Administracion.Seguridad.CapacitacionSeguridad;
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
    public class CapacitacionSeguridadController : BaseController
    {
        private ICapacitacionSeguridadDAO capacitacionSeguridadService;
        IAutorizacionFormatoCambio authFormatoCambioFS;
        const string LISTA_EXAMENES_ASISTENTES = @"listaExamenes";

        Dictionary<string, object> result = new Dictionary<string, object>();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            vSesiones.sesionCapacitacionOperativa = vSesiones.sesionSistemaActual == 18 ? true : false;

            capacitacionSeguridadService = new CapacitacionSeguridadFactoryService().GetCapacitacionSeguridadService();
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

            return View();
        }
        public ActionResult GetCursos(List<int> clasificaciones, List<int> puestos, int estatus)
        {
            result = capacitacionSeguridadService.getCursos(clasificaciones, puestos, estatus);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCursoById(int id)
        {
            result = capacitacionSeguridadService.getCursoById(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetExamenesCursoById(int id)
        {
            result = capacitacionSeguridadService.getExamenesCursoById(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DescargarArchivo(long examen_id)
        {
            var array = capacitacionSeguridadService.descargarArchivo(examen_id);
            string pathExamen = capacitacionSeguridadService.getFileName(examen_id);

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
            result = capacitacionSeguridadService.getTipoExamen(curso_id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetClasificacionCursos()
        {
            result = capacitacionSeguridadService.getClasificacionCursos();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPuestos()
        {
            result = capacitacionSeguridadService.getPuestosEnkontrol();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarCurso(tblS_CapacitacionSeguridadCursos curso)
        {
            result = capacitacionSeguridadService.guardarCurso(curso);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarExamenes()
        {
            var examenes = JsonConvert.DeserializeObject<List<tblS_CapacitacionSeguridadCursosExamen>>(Request.Form["examenes"]);
            List<HttpPostedFileBase> archivos = new List<HttpPostedFileBase>();

            foreach (string fileName in Request.Files)
            {
                archivos.Add(Request.Files[fileName]);
            }

            result = capacitacionSeguridadService.guardarExamenes(examenes, archivos);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ActualizarCurso(tblS_CapacitacionSeguridadCursos curso, List<tblS_CapacitacionSeguridadCursosMando> mandos, List<tblS_CapacitacionSeguridadCursosPuestos> puestosNuevos, List<tblS_CapacitacionSeguridadCursosPuestosAutorizacion> puestosAutorizacionNuevos, List<tblS_CapacitacionSeguridadCursosCC> centrosCosto)
        {
            result = capacitacionSeguridadService.actualizarCurso(curso, mandos, puestosNuevos, puestosAutorizacionNuevos, centrosCosto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EliminarExamen(int examen_id)
        {
            result = capacitacionSeguridadService.eliminarExamen(examen_id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetEstatusCursos()
        {
            result = capacitacionSeguridadService.GetEstatusCursos();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EliminarCurso(int cursoID)
        {
            result = capacitacionSeguridadService.EliminarCurso(cursoID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RelacionCCAutorizante(int division)
        {
            vSesiones.sesionDivisionActual = division;

            return View();
        }

        public ActionResult RelacionCCDepartamentoRazonSocial(int division)
        {
            vSesiones.sesionDivisionActual = division;

            return View();
        }

        public ActionResult GetMandosEnum()
        {
            return Json(new { items = GlobalUtils.ParseEnumToCombo<MandoEnum>() }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPuestosMandos(List<int> mandos)
        {
            result = capacitacionSeguridadService.getPuestosEnkontrolMandos(mandos);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Control de Asistencia
        // GET: Administrativo/Capacitacion/ControlAsistencia
        [HttpGet]
        public ActionResult ControlAsistencia(int division)
        {
            vSesiones.sesionDivisionActual = division;

            return View();
        }

        [HttpGet]
        public ActionResult ErrorDescarga()
        {
            return View();
        }

        public ActionResult ObtenerComboCC()
        {
            return Json(capacitacionSeguridadService.ObtenerComboCC(), JsonRequestBehavior.AllowGet);
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
                return Json(capacitacionSeguridadService.ObtenerListaControlesAsistencia(cc, estado, fInicio, fFin), JsonRequestBehavior.AllowGet);
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
            return Json(capacitacionSeguridadService.GetCursosAutocomplete(term, porClave), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetUsuariosAutocomplete(string term, bool porClave)
        {
            return Json(capacitacionSeguridadService.GetUsuariosAutocomplete(term, porClave), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetLugarCursoAutocomplete(string term)
        {
            return Json(capacitacionSeguridadService.GetLugarCursoAutocomplete(term), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetEmpleadoEnKontrolAutocomplete(string term, bool porClave)
        {
            return Json(capacitacionSeguridadService.GetEmpleadoEnKontrolAutocomplete(term, porClave), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CrearControlAsistencia(tblS_CapacitacionSeguridadControlAsistencia controlAsistencia)
        {
            return Json(capacitacionSeguridadService.CrearControlAsistencia(controlAsistencia), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SubirArchivoControlAsistencia(HttpPostedFileBase archivo, int controlAsistenciaID)
        {
            return Json(capacitacionSeguridadService.SubirArchivoControlAsistencia(archivo, controlAsistenciaID), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DescargarListaControlAsistencia(int controlAsistenciaID)
        {
            var resultadoTupla = capacitacionSeguridadService.DescargarListaControlAsistencia(controlAsistenciaID);

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
            return Json(capacitacionSeguridadService.CargarDatosControlAsistencia(controlAsistenciaID), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CargarAsistentesCapacitacion(int controlAsistenciaID)
        {
            return Json(capacitacionSeguridadService.CargarAsistentesCapacitacion(controlAsistenciaID), JsonRequestBehavior.AllowGet);
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
            return Json(capacitacionSeguridadService.GuardarEvaluacionAsistentes(listaAsistentes), JsonRequestBehavior.AllowGet);
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
                resultado = capacitacionSeguridadService.GuardarExamenesAsistentes(listaExamenes, jefeID, coordinadorID, secretarioID, gerenteID, rfc, razonSocial);
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
            var resultadoTupla = capacitacionSeguridadService.DescargarExamenAsistente(controlAsistenciaDetalleID, tipoExamen);

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
            result = capacitacionSeguridadService.EliminarControlAsistencia(controlAsistenciaID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GuardarArchivosDC3(HttpPostedFileBase archivoDC3, int controlAsistenciaDetalleID)
        {
            result = capacitacionSeguridadService.guardarArchivosDC3(archivoDC3, controlAsistenciaDetalleID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DescargarDC3(int controlAsistenciaDetalleID)
        {
            var resultadoTupla = capacitacionSeguridadService.DescargarDC3(controlAsistenciaDetalleID);

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
        #endregion

        #region Autorización Capacitación

        // GET: Administrativo/Capacitacion/AutorizacionCapacitacion
        [HttpGet]
        public ActionResult AutorizacionCapacitacion(int division)
        {
            vSesiones.sesionDivisionActual = division;

            return View();
        }

        [HttpGet]
        public ActionResult ObtenerComboEstatusAutorizacionCapacitacion()
        {
            return Json(capacitacionSeguridadService.ObtenerComboEstatusAutorizacionCapacitacion(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ObtenerAutorizaciones(string cc, int curso, int estatus)
        {
            var data = capacitacionSeguridadService.ObtenerAutorizaciones(cc, curso, estatus);

            Session["reporteAutorizacionGeneral"] = data["reporteAutorizacionGeneral"];

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ObtenerAutorizantes(int capacitacionID)
        {
            return Json(capacitacionSeguridadService.ObtenerAutorizantes(capacitacionID), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AutorizarControlAsistencia(int controlAsistenciaID)
        {
            return Json(capacitacionSeguridadService.AutorizarControlAsistencia(controlAsistenciaID), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RechazarControlAsistencia(int controlAsistenciaID, string comentario)
        {
            return Json(capacitacionSeguridadService.RechazarControlAsistencia(controlAsistenciaID, comentario), JsonRequestBehavior.AllowGet);
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
                return Json(capacitacionSeguridadService.EnviarCorreoRechazo(controlAsistenciaID, pdf), JsonRequestBehavior.AllowGet);
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
                return Json(capacitacionSeguridadService.EnviarCorreoAutorizacion(controlAsistenciaID, pdf), JsonRequestBehavior.AllowGet);
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
                return Json(capacitacionSeguridadService.EnviarCorreoAutorizacionCompleta(controlAsistenciaID, pdf), JsonRequestBehavior.AllowGet);
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
                    result = capacitacionSeguridadService.guardarCargaMasiva(archivo);
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

            return View();
        }

        [HttpPost]
        public ActionResult ObtenerEmpleados(List<string> ccsCplan, List<string> ccsArr, List<string> puestos)
        {
            return Json(capacitacionSeguridadService.ObtenerEmpleados(ccsCplan, ccsArr, puestos), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ObtenerComboCCEnKontrol(EmpresaEnum empresa)
        {
            return Json(capacitacionSeguridadService.ObtenerComboCCEnKontrol(empresa), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ObtenerCursosEmpleado(int claveEmpleado, int puestoID)
        {
            return Json(capacitacionSeguridadService.ObtenerCursosEmpleado(claveEmpleado, puestoID), JsonRequestBehavior.AllowGet);
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
                var resultadoTupla = capacitacionSeguridadService.DescargarExpedienteEmpleado(claveEmpleado, licencia[0]);

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
            return Json(capacitacionSeguridadService.ObtenerExtracurricularesEmpleado(claveEmpleado), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SubirEvidenciaExtracurricular(int claveEmpleado, string nombre, decimal duracion, string fecha, string fechaFin = "", HttpPostedFileBase evidencia = null)
        {
            return Json(capacitacionSeguridadService.SubirEvidenciaExtracurricular(claveEmpleado, nombre, duracion, fecha, fechaFin, evidencia), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DescargarEvidenciaExtracurricular(int extracurricularID)
        {
            var resultadoTupla = capacitacionSeguridadService.DescargarEvidenciaExtracurricular(extracurricularID);

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
            return Json(capacitacionSeguridadService.EliminarEvidenciaExtracurricular(extracurricularID), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Dashboard

        // GET: Administrativo/Capacitacion/Dashboard
        public ActionResult Dashboard(int division)
        {
            vSesiones.sesionDivisionActual = division;

            return View();
        }

        [HttpPost]
        public ActionResult CargarDatosGeneralesDashboard(List<string> ccsCplan, List<string> ccsArr, DateTime fechaInicio, DateTime fechaFin, List<string> clasificacion)
        {
            return Json(capacitacionSeguridadService.CargarDatosGeneralesDashboard(ccsCplan, ccsArr, fechaInicio, fechaFin, clasificacion), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Matriz de Necesidades
        // GET: Administrativo/Capacitacion/MatrizNecesidades
        public ActionResult MatrizNecesidades(int division)
        {
            vSesiones.sesionDivisionActual = division;

            return View();
        }

        [HttpPost]
        public ActionResult ObtenerAreasPorCC(List<string> ccsCplan, List<string> ccsArr)
        {
            return Json(capacitacionSeguridadService.ObtenerAreasPorCC(ccsCplan, ccsArr), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CargarDatosMatrizNecesidades(List<string> ccsCplan, List<string> ccsArr, List<string> departamentosIDs, List<ClasificacionCursoEnum> clasificaciones)
        {
            var jsonResponse = new JsonResult
            {
                Data = capacitacionSeguridadService.CargarDatosMatrizNecesidades(ccsCplan, ccsArr, departamentosIDs, clasificaciones),
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
                Data = capacitacionSeguridadService.CargarDatosSeccionMatriz(ccsCplan, ccsArr, departamentosIDs, clasificaciones, seccion),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };

            return jsonResponse;
        }

        [HttpGet]
        public MemoryStream DescargarExcelPersonalActivo()
        {
            var resultadoTupla = capacitacionSeguridadService.DescargarExcelPersonalActivo();

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

        #endregion

        #region Privilegios
        // GET: Administrativo/Capacitacion/Privilegios
        public ActionResult Privilegios(int division)
        {
            vSesiones.sesionDivisionActual = division;

            return View();
        }
        public ActionResult _privilegios()
        {
            return PartialView();
        }
        tblS_CapacitacionSeguridadEmpleadoPrivilegio getPrivilegioActual()
        {
            if (capacitacionSeguridadService == null)
            {
                capacitacionSeguridadService = new CapacitacionSeguridadFactoryService().GetCapacitacionSeguridadService();
            }
            return capacitacionSeguridadService.getPrivilegioActual();
        }
        public bool esCreadorCurso()
        {
            var privilegio = getPrivilegioActual();
            var lstCreador = new List<PrivilegioEnum>() 
            {
                PrivilegioEnum.Administrador,
                PrivilegioEnum.ControlDocumentos
            };
            return lstCreador.Any(p => p == (PrivilegioEnum)privilegio.idPrivilegio);
        }
        public bool PuedeEliminarCursos()
        {
            var privilegio = getPrivilegioActual();
            var lstCreador = new List<PrivilegioEnum> { PrivilegioEnum.Administrador };
            return lstCreador.Any(p => p == (PrivilegioEnum)privilegio.idPrivilegio);
        }
        public bool PuedeEliminarControlAsistencia()
        {
            var privilegio = getPrivilegioActual();
            var lstCreador = new List<PrivilegioEnum> { PrivilegioEnum.Administrador };
            return lstCreador.Any(p => p == (PrivilegioEnum)privilegio.idPrivilegio);
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
            return lstCtrAsistencia.Any(p => p == (PrivilegioEnum)privilegio.idPrivilegio);
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
            return lstDashboard.Any(p => p == (PrivilegioEnum)privilegio.idPrivilegio);
        }
        public bool esAdministrador()
        {
            var privilegio = getPrivilegioActual();
            return (PrivilegioEnum)privilegio.idPrivilegio == PrivilegioEnum.Administrador;
        }
        public ActionResult ObtenerEmpleadosPrivilegios(BusqPrivilegiosDTO busq)
        {
            var result = capacitacionSeguridadService.ObtenerEmpleadosPrivilegios(busq);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult guardarEmpleadosPrivilegios(List<tblS_CapacitacionSeguridadEmpleadoPrivilegio> lst)
        {
            var result = new Dictionary<string, object>();
            try
            {
                if (lst != null && lst.All(cve => cve.idUsuario > 0))
                {
                    result = capacitacionSeguridadService.guardarEmpleadosPrivilegios(lst);
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
            result = capacitacionSeguridadService.getRelacionesCCAutorizantes();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarNuevaRelacionCCAutorizante(RelacionCCAutorizanteDTO relacion)
        {
            result = capacitacionSeguridadService.guardarNuevaRelacionCCAutorizante(relacion);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarRelacionCCAutorizante(RelacionCCAutorizanteDTO relacion)
        {
            result = capacitacionSeguridadService.editarRelacionCCAutorizante(relacion);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarRelacionCCAutorizante(RelacionCCAutorizanteDTO relacion)
        {
            result = capacitacionSeguridadService.eliminarRelacionCCAutorizante(relacion);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUsuarioPorClave(int claveEmpleado)
        {
            result = capacitacionSeguridadService.getUsuarioPorClave(claveEmpleado);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRelacionesCCDepartamentoRazonSocial()
        {
            result = capacitacionSeguridadService.getRelacionesCCDepartamentoRazonSocial();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarNuevaRelacionCCDepartamentoRazonSocial(List<RelacionCCDepartamentoRazonSocialDTO> relaciones)
        {
            result = capacitacionSeguridadService.guardarNuevaRelacionCCDepartamentoRazonSocial(relaciones);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarRelacionCCDepartamentoRazonSocial(RelacionCCDepartamentoRazonSocialDTO relacion)
        {
            result = capacitacionSeguridadService.editarRelacionCCDepartamentoRazonSocial(relacion);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarRelacionCCDepartamentoRazonSocial(RelacionCCDepartamentoRazonSocialDTO relacion)
        {
            result = capacitacionSeguridadService.eliminarRelacionCCDepartamentoRazonSocial(relacion);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDepartamentosCombo()
        {
            result = capacitacionSeguridadService.getDepartamentosCombo();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult AutorizadosPersonalAutorizado(int division)
        {
            vSesiones.sesionDivisionActual = division;

            return View();
        }

        public ActionResult DashboardPersonalAutorizado(int division)
        {
            vSesiones.sesionDivisionActual = division;

            return View();
        }

        public ActionResult CrearPersonalAutorizado(int division)
        {
            vSesiones.sesionDivisionActual = division;

            return View();
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

            return View();
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

            return View();
        }

        public ActionResult CompetenciasOperativas(int division)
        {
            vSesiones.sesionDivisionActual = division;

            return View();
        }

        public ActionResult IndicadorHorasHombre(int division)
        {
            vSesiones.sesionDivisionActual = division;

            return View();
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

        public ActionResult GetRazonSocialCombo()
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = capacitacionSeguridadService.getRazonSocialCombo();

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
            var stream = capacitacionSeguridadService.crearExcelRelacionCursosPuestos();

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
                var data = capacitacionSeguridadService.obtenerComboCursos();

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
                    result = capacitacionSeguridadService.guardarCargaMasivaControlAsistencia(archivo);
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
                    result = capacitacionSeguridadService.guardarCargaMasivaRelacionCursosPuestosAutorizacion(archivo);
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
            return Json(capacitacionSeguridadService.ObtenerComboCCAmbasEmpresas(), JsonRequestBehavior.AllowGet);
        }

        public int privilegioCapacitacion()
        {
            var privilegio = getPrivilegioActual();

            return privilegio != null ? (int)privilegio.idPrivilegio : 0;
        }

        #region Personal Autorizado
        public ActionResult GetListasAutorizacion(List<int> listaCursos, List<tblS_CapacitacionSeguridadListaAutorizacionCC> listaCC)
        {
            return Json(capacitacionSeguridadService.getListasAutorizacion(listaCursos, listaCC), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListasAutorizacionCombo()
        {
            return Json(capacitacionSeguridadService.getListasAutorizacionCombo(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarListaAutorizacion(tblS_CapacitacionSeguridadListaAutorizacion listaAutorizacion, List<tblS_CapacitacionSeguridadListaAutorizacionRFC> listaRFC, List<tblS_CapacitacionSeguridadListaAutorizacionCC> listaCentrosCosto)
        {
            return Json(capacitacionSeguridadService.guardarListaAutorizacion(listaAutorizacion, listaRFC, listaCentrosCosto), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarListaAutorizacion(tblS_CapacitacionSeguridadListaAutorizacion listaAutorizacion, List<tblS_CapacitacionSeguridadListaAutorizacionRFC> listaRFC, List<tblS_CapacitacionSeguridadListaAutorizacionCC> listaCentrosCosto)
        {
            return Json(capacitacionSeguridadService.editarListaAutorizacion(listaAutorizacion, listaRFC, listaCentrosCosto), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarListaAutorizacion(int listaAutorizacionID)
        {
            return Json(capacitacionSeguridadService.eliminarListaAutorizacion(listaAutorizacionID), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListaAutorizacionByID(int listaAutorizacionID)
        {
            var json = Json(capacitacionSeguridadService.getListaAutorizacionByID(listaAutorizacionID), JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        public ActionResult GetAutorizanteEnkontrolAutocomplete(string term)
        {
            return Json(capacitacionSeguridadService.getAutorizanteEnkontrolAutocomplete(term), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarInformacionAutorizados(ListaAutorizacionDTO listaAutorizacion)
        {
            return Json(capacitacionSeguridadService.guardarInformacionAutorizados(listaAutorizacion), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarDashboardPersonalAutorizado(FiltrosDashboardPersonalAutorizadoDTO filtros)
        {
            return Json(capacitacionSeguridadService.cargarDashboardPersonalAutorizado(filtros), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCorreosListaAutorizacion(int listaAutorizacionID)
        {
            return Json(capacitacionSeguridadService.getCorreosListaAutorizacion(listaAutorizacionID), JsonRequestBehavior.AllowGet);
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
                var data = capacitacionSeguridadService.getCiclosTrabajoCombo();

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

        public ActionResult GuardarNuevoCiclo(tblS_CapacitacionSeguridadDNCicloTrabajo ciclo, List<tblS_CapacitacionSeguridadDNCicloTrabajoAreas> listaAreas, List<tblS_CapacitacionSeguridadDNCicloTrabajoCriterio> listaCriterios)
        {
            return Json(capacitacionSeguridadService.guardarNuevoCiclo(ciclo, listaAreas, listaCriterios), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCicloByID(int cicloID)
        {
            return Json(capacitacionSeguridadService.getCicloByID(cicloID), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarRegistroCiclo(tblS_CapacitacionSeguridadDNCicloTrabajoRegistro registroCiclo, List<tblS_CapacitacionSeguridadDNCicloTrabajoRegistroRevisiones> listaRevisiones, List<tblS_CapacitacionSeguridadDNCicloTrabajoRegistroPropuestas> listaPropuestas, List<tblS_CapacitacionSeguridadDNCicloTrabajoRegistroAreas> listaAreas)
        {
            return Json(capacitacionSeguridadService.guardarRegistroCiclo(registroCiclo, listaRevisiones, listaPropuestas, listaAreas), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRegistrosCiclos(FiltrosRegistrosCiclo filtros)
        {
            return Json(capacitacionSeguridadService.getRegistrosCiclos(filtros), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListaSeguimiento(List<string> listaCC, TipoSeguimientoEnum tipoSeguimiento, DateTime fechaInicio, DateTime fechaFin)
        {
            return Json(capacitacionSeguridadService.getListaSeguimiento(listaCC, tipoSeguimiento, fechaInicio, fechaFin), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarSeguimientoAcciones(List<HttpPostedFileBase> evidencias)
        {
            var capturaEvidencias = (JsonConvert.DeserializeObject<tblS_CapacitacionSeguridadDNCicloTrabajoRegistro[]>(Request.Form["capturaEvidencias"], new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" })).ToList();
            var capturaEvaluaciones = (JsonConvert.DeserializeObject<tblS_CapacitacionSeguridadDNCicloTrabajoRegistro[]>(Request.Form["capturaEvaluaciones"], new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" })).ToList();

            return Json(capacitacionSeguridadService.guardarSeguimientoAcciones(capturaEvidencias, evidencias, capturaEvaluaciones), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarSeguimientoPropuestas(List<HttpPostedFileBase> evidencias)
        {
            var capturaEvidencias = (JsonConvert.DeserializeObject<tblS_CapacitacionSeguridadDNCicloTrabajoRegistroPropuestas[]>(Request.Form["capturaEvidencias"], new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" })).ToList();
            var capturaEvaluaciones = (JsonConvert.DeserializeObject<tblS_CapacitacionSeguridadDNCicloTrabajoRegistroPropuestas[]>(Request.Form["capturaEvaluaciones"], new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" })).ToList();

            return Json(capacitacionSeguridadService.guardarSeguimientoPropuestas(capturaEvidencias, evidencias, capturaEvaluaciones), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarDatosArchivoEvidenciaSeguimientoAcciones(int id)
        {
            var resultado = new Dictionary<string, object>();

            resultado = capacitacionSeguridadService.cargarDatosArchivoEvidenciaSeguimientoAcciones(id);

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
            var resultadoTupla = capacitacionSeguridadService.descargarArchivoEvidenciaAccion(id);

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

            resultado = capacitacionSeguridadService.cargarDatosArchivoEvidenciaSeguimientoPropuestas(id);

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
            var resultadoTupla = capacitacionSeguridadService.descargarArchivoEvidenciaPropuesta(id);

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
            return Json(capacitacionSeguridadService.cargarDashboardCiclos(listaCC, listaAreas, fechaInicio, fechaFin), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRegistroCicloTrabajoByID(int id)
        {
            return Json(capacitacionSeguridadService.getRegistroCicloTrabajoByID(id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTablaCicloTrabajo()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstCicloTrabajo = capacitacionSeguridadService.GetTablaCicloTrabajo();
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
                var lstCicloTrabajoCriterio = capacitacionSeguridadService.GetTablaCriterioTrabajo(id);
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
                var EliminarCicloTrabajo = capacitacionSeguridadService.EliminarCicloTrabajo(id);
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


        public ActionResult EditarCicloTrabajo(CicloTrabajoDTO parametros, List<tblS_CapacitacionSeguridadDNCicloTrabajoCriterio> criterio, List<tblS_CapacitacionSeguridadDNCicloTrabajoAreas> lstAreass)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var EditarCicloTrabajo = capacitacionSeguridadService.EditarCicloTrabajo(parametros, criterio, lstAreass);
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
                var obj = capacitacionSeguridadService.llenarCorreos(_IdUsuario);
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

                var obj = capacitacionSeguridadService.enviarCorreoRecorrido(recorridoID, usuarios, downloadPDF);
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
            return Json(capacitacionSeguridadService.getListaDepartamientos(listaAutorizacionID), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Detecciones Primarias
        public ActionResult GetRegistrosDeteccionesPrimarias(List<string> listaCC, List<AreaDTO> listaAreas, DateTime fecha)
        {
            return Json(capacitacionSeguridadService.getRegistrosDeteccionesPrimarias(listaCC, listaAreas, fecha), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarDeteccionPrimaria(tblS_CapacitacionSeguridadDNDeteccionPrimaria deteccionPrimaria, List<tblS_CapacitacionSeguridadDNDeteccionPrimariaNecesidad> listaNecesidades, List<tblS_CapacitacionSeguridadDNDeteccionPrimariaAreas> listaAreas)
        {
            return Json(capacitacionSeguridadService.guardarDeteccionPrimaria(deteccionPrimaria, listaNecesidades, listaAreas), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Recorridos
        public ActionResult GetRegistrosRecorridos(List<string> listaCC, List<AreaDTO> listaAreas, DateTime fecha, int realizador)
        {
            return Json(capacitacionSeguridadService.getRegistrosRecorridos(listaCC, listaAreas, fecha, realizador), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarNuevoRecorrido(List<HttpPostedFileBase> evidencias)
        {
            var recorrido = JsonConvert.DeserializeObject<tblS_CapacitacionSeguridadDNRecorrido>(Request.Form["recorrido"], new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            var listaHallazgos = (JsonConvert.DeserializeObject<RecorridoHallazgoDTO[]>(Request.Form["listaHallazgos"], new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" })).ToList();
            var listaAreas = (JsonConvert.DeserializeObject<tblS_CapacitacionSeguridadDNRecorridoAreas[]>(Request.Form["listaAreas"], new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" })).ToList();

            return Json(capacitacionSeguridadService.guardarNuevoRecorrido(recorrido, listaHallazgos, listaAreas, evidencias), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarRecorrido(List<HttpPostedFileBase> evidencias)
        {
            var recorrido = JsonConvert.DeserializeObject<tblS_CapacitacionSeguridadDNRecorrido>(Request.Form["recorrido"], new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            var listaHallazgos = (JsonConvert.DeserializeObject<RecorridoHallazgoDTO[]>(Request.Form["listaHallazgos"], new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" })).ToList();
            var listaAreas = (JsonConvert.DeserializeObject<tblS_CapacitacionSeguridadDNRecorridoAreas[]>(Request.Form["listaAreas"], new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" })).ToList();

            return Json(capacitacionSeguridadService.editarRecorrido(recorrido, listaHallazgos, listaAreas, evidencias), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRecorridoByID(int recorridoID)
        {
            return Json(capacitacionSeguridadService.getRecorridoByID(recorridoID), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarSeguimientoRecorrido(List<tblS_CapacitacionSeguridadDNRecorridoHallazgo> listaSeguimiento)
        {
            return Json(capacitacionSeguridadService.guardarSeguimientoRecorrido(listaSeguimiento), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarDashboardRecorridos(List<string> listaCC, List<AreaDTO> listaAreas, DateTime fecha, int realizador)
        {
            return Json(capacitacionSeguridadService.cargarDashboardRecorridos(listaCC, listaAreas, fecha, realizador), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region Competencias Operativas
        public ActionResult GetPromedioEvaluaciones(List<string> listaCC, List<AreaDTO> listaAreas, DateTime fecha)
        {
            return Json(capacitacionSeguridadService.getPromedioEvaluaciones(listaCC, listaAreas, fecha), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Indicador Hrs-Hombre
        public ActionResult GetEquiposCombo()
        {
            result = capacitacionSeguridadService.getEquiposCombo();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEmpleadoPorClave(int claveEmpleado)
        {
            return Json(capacitacionSeguridadService.getEmpleadoPorClave(claveEmpleado), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarHorasAdiestramiento(List<string> listaCC, DateTime fechaInicial, DateTime fechaFinal)
        {
            return Json(capacitacionSeguridadService.cargarHorasAdiestramiento(listaCC, fechaInicial, fechaFinal), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarNuevoColaboradorCapacitacion(tblS_CapacitacionSeguridadIHHColaboradorCapacitacion colaboradorCapacitacion)
        {
            return Json(capacitacionSeguridadService.guardarNuevoColaboradorCapacitacion(colaboradorCapacitacion), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetInfoColaboradorCapacitacion(int colaboradorCapacitacionID)
        {
            return Json(capacitacionSeguridadService.getInfoColaboradorCapacitacion(colaboradorCapacitacionID), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarNuevoControlHoras(List<tblS_CapacitacionSeguridadIHHControlHoras> listaControlHoras)
        {
            return Json(capacitacionSeguridadService.guardarNuevoControlHoras(listaControlHoras), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarLiberados(List<HttpPostedFileBase> archivos)
        {
            var captura = (JsonConvert.DeserializeObject<tblS_CapacitacionSeguridadIHHColaboradorCapacitacion[]>(Request.Form["captura"], new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" })).ToList();

            return Json(capacitacionSeguridadService.guardarLiberados(captura, archivos), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarDatosArchivoSoporteAdiestramiento(int id)
        {
            var resultado = new Dictionary<string, object>();

            resultado = capacitacionSeguridadService.cargarDatosArchivoSoporteAdiestramiento(id);

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
            var resultadoTupla = capacitacionSeguridadService.descargarArchivoSoporteAdiestramiento(id);

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
        public ActionResult postObtenerTablaHorasHombre(HorasHombreDTO parametros)
        {
            try
            {
                result.Add(ITEMS, capacitacionSeguridadService.postObtenerTablaHorasHombre(parametros, true));
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
                result.Add(ITEMS, capacitacionSeguridadService.obtenerInputPromedios(parametros));
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
                Session["objHorasHombreCapacitacion"] = capacitacionSeguridadService.obtenerInputPromedios(parametros);
                var lstHorasHombre = capacitacionSeguridadService.postObtenerTablaHorasHombre(parametros, false);
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
            var stream = capacitacionSeguridadService.crearExcelHorasHombreCapacitacion(parametros);

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
                lstObjEmpleadoCambio = capacitacionSeguridadService.TablaFormatosPendientes(parametros);
                int usuarioLog = 3367;
                var listaMostrar = new List<resultCapacitacionDTO>();
                var ud = new UsuarioDAO();
                var isVerTodo = ud.getViewAction(vSesiones.sesionCurrentView, "TodoFormatoCambio");
                var rh = ud.getViewAction(vSesiones.sesionCurrentView, "VerTodoFormato");
                var rhEditar = ud.getViewAction(vSesiones.sesionCurrentView, "Editar");

                foreach (var formatoCambio in lstObjEmpleadoCambio)
                {
                    var listAp = capacitacionSeguridadService.getAutorizacion(formatoCambio.id);

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

            result = capacitacionSeguridadService.postSubirArchivos(id, empresa, archivo);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DescargarArchivoCO(int id)
        {
            var array = capacitacionSeguridadService.descargarArchivoCO(id);
            string pathExamen = capacitacionSeguridadService.getFileNameCO(id);

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

                result = capacitacionSeguridadService.obtenerArchivoCODescargas(idFormatoCambio);


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






        #endregion

        #region ADMINISTRACION INSTRUCTORES

        public ActionResult GetInstructores()
        {
            try
            {
                result.Add(ITEMS, capacitacionSeguridadService.GetInstructores());
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
            result.Add(ITEMS, capacitacionSeguridadService.GetRolesCombo());
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PostGuardarInstructor(tblS_CapacitacionSeguridad_PCAdministracionInstructores parametros, bool AddEdit)
        {
            parametros.division = vSesiones.sesionDivisionActual;
            result.Add(ITEMS, capacitacionSeguridadService.PostGuardarInstructor(parametros, AddEdit));
            result.Add(SUCCESS, "SUCCESS");
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetInstructoresCombo(string cc)
        {
            result.Add(ITEMS, capacitacionSeguridadService.GetInstructoresCombo(cc));
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getFechaInicio(string cveEmpleado)
        {
            result.Add(ITEMS, capacitacionSeguridadService.getFechaInicio(cveEmpleado));
            result.Add(SUCCESS, "SUCCESS");
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCC()
        {
            result.Add(ITEMS, capacitacionSeguridadService.GetCC());
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
            result.Add(ITEMS, capacitacionSeguridadService.EliminarInstructor(id));
            result.Add(SUCCESS, "SUCCESS");
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ObtenerCCUnico(string cveEmpleado)
        {
            try
            {
                result.Add(ITEMS, capacitacionSeguridadService.ObtenerCCUnico(cveEmpleado));
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
            return Json(capacitacionSeguridadService.cargarCalendarioPlanCapacitacion(cc, listaTematicas, empresa, mesCalendario), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTematicaCombo()
        {
            result.Add(ITEMS, (GlobalUtils.ParseEnumToCombo<TematicaCursoEnum>()).Where(x => x.Value > 0).ToList());
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult GetEmpleadoCursosActos(int claveEmpleado)
        {
            return Json(capacitacionSeguridadService.GetEmpleadoCursosActos(claveEmpleado), JsonRequestBehavior.AllowGet);
        }
    }
}