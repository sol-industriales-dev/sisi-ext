using Core.DAO.Administracion.Seguridad.Capacitacion;
using Core.DTO.Administracion.Seguridad.Capacitacion.CincoS;
using Core.Enum.Administracion.Seguridad.Capacitacion.CincoS;
using Core.Service.Administracion.Seguridad.Capacitacion;
using Data.Factory.Administracion.Seguridad.Capacitacion;
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
    public class CincoSController : BaseController
    {
        ICincoSDAO _cincoSFS;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _cincoSFS = new CincoSFactoryService().GetCincoSService();

            base.OnActionExecuting(filterContext);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult VistaMenuAuditorias5s()
        {
            return View();
        }

        public ActionResult Calendario()
        {
            return View();
        }

        public ActionResult Auditorias5s()
        {
            if (_cincoSFS.AccesoPermitido(PrivilegioEnum.Administrador) || _cincoSFS.AccesoPermitido(PrivilegioEnum.Auditor))
            {
                return View();
            }
            else
            {
                return View("ErrorPermisoVista");
            }
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

        #region CHECKLIST
        public ViewResult CheckList()
        {
            if (_cincoSFS.AccesoPermitido(PrivilegioEnum.Administrador))
            {
                return View();
            }
            else
            {
                return View("ErrorPermisoVista");
            }
        }

        public JsonResult GetCCs(ConsultaCCsEnum consulta, int? checkListId)
        {
            return Json(_cincoSFS.GetCCs(consulta, checkListId));
        }

        public JsonResult GetCheckLists(List<string> ccs)
        {
            return Json(_cincoSFS.GetCheckLists(ccs));
        }

        public JsonResult GetAreas()
        {
            return Json(_cincoSFS.GetAreas());
        }

        public JsonResult GetLideres()
        {
            return Json(_cincoSFS.GetLideres());
        }

        public JsonResult GetSubAreas()
        {
            return Json(_cincoSFS.GetSubAreas());
        }

        public JsonResult GetCheckList(int checkListId)
        {
            return Json(_cincoSFS.GetCheckList(checkListId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GuardarCheckList(CheckListGuardarDTO checkList)
        {
            return Json(_cincoSFS.GuardarCheckList(checkList));
        }

        public JsonResult EliminarCheckList(int checkListId)
        {
            return Json(_cincoSFS.EliminarCheckList(checkListId));
        }

        public JsonResult EditarCheckList(CheckListGuardarDTO checkList)
        {
            return Json(_cincoSFS.EditarCheckList(checkList));
        }

        public JsonResult GetCalendarioCheckList(int checkListId)
        {
            return Json(_cincoSFS.GetCalendarioCheckList(checkListId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GuardarCalendarioCheckList(CalendarioCheckListDTO calendario)
        {
            return Json(_cincoSFS.GuardarCalendarioCheckList(calendario), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region CALENDARIO
        public ActionResult GetCalendarios(List<string> ccsFiltro, int añoFiltro)
        {
            return Json(_cincoSFS.GetCalendarios(ccsFiltro, añoFiltro));
        }

        public ActionResult SaveImgSessionCalendario(string img){

            var dictResult = new Dictionary<string,object>();
            dictResult.Add(SUCCESS, true);

            Session["imgCalendarioCincoS"] = img;

            return Json(dictResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveImg5sReporteEjecutivo(string img)
        {

            var dictResult = new Dictionary<string, object>();
            dictResult.Add(SUCCESS, true);

            Session["img5sReporteEjecutivo"] = img;

            return Json(dictResult, JsonRequestBehavior.AllowGet);
        }
        
        #endregion

        #region PlanAccion
        public ActionResult Seguimientos()
        {
            return View();
        }

        public ActionResult llenarTablaPlanAccion(AuditoriaDTO objParamDTO, DateTime fechaInicio, DateTime fechaFinal)
        {
            return Json(_cincoSFS.llenarTablaPlanAccion(objParamDTO, fechaInicio, fechaFinal), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region REPORTE EJECUTIVO

        public ActionResult VistaMenuReporteEjecutivo()
        {
            return View();
        }

        public ActionResult Estadisticas()
        {
            return View();
        }

        public ActionResult ReporteEjecutivo()
        {
            return View();
        }


        #endregion    

        #region AUDITORIAS 5'S
        public ActionResult GetAuditorias(AuditoriaDTO objParamDTO)
        {
            return Json(_cincoSFS.GetAuditorias(objParamDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearEditarAuditoria(List<HttpPostedFileBase> lstDetecciones, List<HttpPostedFileBase> lstMedidas)
        {
            var objParamDTO = JsonConvert.DeserializeObject<AuditoriaDTO>(Request.Form["objParamDTO"], new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            List<int> lstIndice_Detecciones = JsonConvert.DeserializeObject<List<int>>(Request.Form["lstIndice_Detecciones"], new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            List<int> lstIndice_Medidas = JsonConvert.DeserializeObject<List<int>>(Request.Form["lstIndice_Medidas"], new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            return Json(_cincoSFS.CrearEditarAuditoria(objParamDTO, lstDetecciones, lstMedidas, lstIndice_Detecciones, lstIndice_Medidas), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetInspeccionesRelCheckList(AuditoriaDTO objParamDTO)
        {
            return Json(_cincoSFS.GetInspeccionesRelCheckList(objParamDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboAuditores(AuditoriaDTO objParamDTO)
        {
            return Json(_cincoSFS.FillCboAuditores(objParamDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDatosActualizarAuditoria(AuditoriaDTO objParamDTO)
        {
            return Json(_cincoSFS.GetDatosActualizarAuditoria(objParamDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult NotificarAuditoria(int idAuditoria)
        {
            return Json(_cincoSFS.NotificarAuditoria(idAuditoria));
        }
        #endregion

        #region SEGUIMIENTOS
        public ActionResult GetSeguimientos(AuditoriaDTO objParamDTO)
        {
            return Json(_cincoSFS.GetSeguimientos(objParamDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult RegistrarArchivoSeguimiento(HttpPostedFileBase objArchivoSeguimiento)
        {
            var objParamDTO = JsonConvert.DeserializeObject<AuditoriaDTO>(Request.Form["objParamDTO"], new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            return Json(_cincoSFS.RegistrarArchivoSeguimiento(objParamDTO, objArchivoSeguimiento), JsonRequestBehavior.AllowGet);
        }

        public ActionResult AutorizarRechazarArchivoSeguimiento(AuditoriaDetDTO objParamDTO)
        {
            return Json(_cincoSFS.AutorizarRechazarArchivoSeguimiento(objParamDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult DescargarArchivo(AuditoriaDetDTO objParamDTO)
        {
            var resultadoTupla = _cincoSFS.DescargarArchivo(objParamDTO);

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

        public ActionResult VisualizarArchivo(AuditoriaDetDTO objParamDTO)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();
            resultado = _cincoSFS.VisualizarArchivo(objParamDTO);

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
        }

        public ActionResult GuardarComentarioLider(int idAuditDet, string comentario)
        {
            return Json(_cincoSFS.GuardarComentarioLider(idAuditDet, comentario), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region GENERALES
        public ActionResult FillCboCheckList()
        {
            return Json(_cincoSFS.FillCboCheckList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboProyectos(AuditoriaDTO objParamDTO)
        {
            return Json(_cincoSFS.FillCboProyectos(objParamDTO), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region FACULTAMIENTOS
        public ViewResult Usuario()
        {
            if (_cincoSFS.AccesoPermitido(PrivilegioEnum.Administrador))
            {
                return View();
            }
            else
            {
                return View("ErrorPermisoVista");
            }
        }

        [HttpGet]
        public JsonResult GetUsuario(string term)
        {
            return Json(_cincoSFS.GetUsuario(term), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetInfoUsuario(int idUsuario)
        {
            return Json(_cincoSFS.GetInfoUsuario(idUsuario), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAuditores(List<string> ccs)
        {
            return Json(_cincoSFS.GetAuditores(ccs));
        }

        public JsonResult GuardarAuditor(GuardarUsuarioDTO usuario)
        {
            return Json(_cincoSFS.GuardarAuditor(usuario));
        }

        public JsonResult EliminarAuditor(int idAuditor)
        {
            return Json(_cincoSFS.EliminarAuditor(idAuditor));
        }

        public JsonResult GetAuditor(int idAuditor)
        {
            return Json(_cincoSFS.GetAuditor(idAuditor), JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditarAuditor(AuditorInfoDTO info)
        {
            return Json(_cincoSFS.EditarAuditor(info));
        }

        public JsonResult GetFacultamientos(List<string> ccs, int? privilegioId)
        {
            return Json(_cincoSFS.GetFacultamientos(ccs, privilegioId));
        }

        public JsonResult GetPrivilegios()
        {
            return Json(_cincoSFS.GetPrivilegios());
        }

        public JsonResult GetAuditorPrivilegio(int idAuditor)
        {
            return Json(_cincoSFS.GetAuditorPrivilegio(idAuditor), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GuardarAuditorPrivilegio(AuditorPrivilegioDTO privilegio)
        {
            return Json(_cincoSFS.GuardarAuditorPrivilegio(privilegio));
        }

        public JsonResult EliminarAuditorPrivilegio(int idAuditor)
        {
            return Json(_cincoSFS.EliminarAuditorPrivilegio(idAuditor));
        }

        public JsonResult GetTablaLideres(List<string> ccs)
        {
            return Json(_cincoSFS.GetTablaLideres(ccs));
        }

        public JsonResult GetLider(int idLider)
        {
            return Json(_cincoSFS.GetLider(idLider), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GuardarLider(LiderInfoDTO info)
        {
            return Json(_cincoSFS.GuardarLider(info));
        }

        public JsonResult EliminarLider(int idLider)
        {
            return Json(_cincoSFS.EliminarLider(idLider));
        }

        public JsonResult GetAreaOperativaLider()
        {
            return Json(_cincoSFS.GetAreaOperativaLider());
        }

        public JsonResult GetTablaSubAreas()
        {
            return Json(_cincoSFS.GetTablaSubAreas(), JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetSubArea(int id)
        {
            return Json(_cincoSFS.GetSubArea(id), JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditarSubArea(int id, string nombre)
        {
            return Json(_cincoSFS.EditarSubArea(id, nombre));
        }

        public JsonResult EliminarSubArea(int id)
        {
            return Json(_cincoSFS.EliminarSubArea(id));
        }

        public JsonResult GuardarSubArea(string nombre)
        {
            return Json(_cincoSFS.GuardarSubArea(nombre));
        }
        #endregion

        #region REPORTES

        public JsonResult GetEstadisticasTendencias(List<string> CCs, List<int> areas, DateTime fechaInicio, DateTime fechaFin)
        {
            if (CCs == null) CCs = new List<string>();
            if (areas == null) areas = new List<int>();
            return Json(_cincoSFS.GetEstadisticasTendencias(CCs, areas, fechaInicio, fechaFin));
        }
        public JsonResult GetReporteEjecutivo(List<string> CCs, List<int> areas, DateTime fechaInicio, DateTime fechaFin)
        {
            if (CCs == null) CCs = new List<string>();
            if (areas == null) areas = new List<int>();
            return Json(_cincoSFS.GetReporteEjecutivo(CCs, areas, fechaInicio, fechaFin));
        }

        #endregion
    }
}