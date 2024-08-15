using Core.DAO.Administracion.Seguridad.MedioAmbiente;
using Core.DTO;
using Core.DTO.Administracion.Seguridad.MedioAmbiente;
using Core.Entity.Administrativo.Seguridad.Indicadores;
using Core.Entity.Administrativo.Seguridad.MedioAmbiente;
using Core.Enum.Administracion.Seguridad.MedioAmbiente;
using Data.Factory.Administracion.Seguridad.MedioAmbiente;
using DotnetDaddy.DocumentViewer;
using Infrastructure.Utils;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Administrativo.Controllers.Seguridad
{
    public class MedioAmbienteController : BaseController
    {
        #region VARIABLES GLOBALES
        Dictionary<string, object> resultado = new Dictionary<string, object>();
        #endregion

        #region INIT
        private IMedioAmbienteDAO medioAmbienteService;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            medioAmbienteService = new MedioAmbienteFactoryService().GetMedioAmbienteService();
            resultado.Clear();
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
        #endregion

        #region VIEWS
        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult Captura()
        {
            return View();
        }

        public ActionResult AspectosAmbientales()
        {
            return View();
        }

        public ActionResult Transportistas()
        {
            return View();
        }

        public ActionResult ClasificacionesTransportistas()
        {
            return View();
        }
        #endregion

        #region ASPECTOS AMBIENTALES
        public ActionResult GetAspectosAmbientales(int tipoCaptura = 0)
        {
            return Json(medioAmbienteService.getAspectosAmbientales(tipoCaptura), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUnidadCombo()
        {
            resultado.Add(ITEMS, GlobalUtils.ParseEnumToCombo<UnidadEnum>());
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFactorPeligroCombo()
        {
            resultado.Add(ITEMS, GlobalUtils.ParseEnumToCombo<FactorPeligroEnum>());
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarNuevoAspectoAmbiental(tblS_MedioAmbienteAspectoAmbiental aspectoAmbiental)
        {
            return Json(medioAmbienteService.guardarNuevoAspectoAmbiental(aspectoAmbiental), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarAspectoAmbiental(tblS_MedioAmbienteAspectoAmbiental aspectoAmbiental)
        {
            return Json(medioAmbienteService.editarAspectoAmbiental(aspectoAmbiental), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarAspectoAmbiental(tblS_MedioAmbienteAspectoAmbiental aspectoAmbiental)
        {
            return Json(medioAmbienteService.eliminarAspectoAmbiental(aspectoAmbiental), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetClasificacionCombo()
        {
            return Json(medioAmbienteService.getClasificacionCombo(), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region CAPTURAS
        #region ACOPIO
        public ActionResult GetCapturas(CapturaDTO _objFiltroDTO)
        {
            return Json(medioAmbienteService.GetCapturas(_objFiltroDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearEditarCaptura(HttpPostedFileBase _objFile)
        {
            CapturaDTO _objCEDTO = JsonUtils.convertJsonToNetObject<CapturaDTO>(Request.Form["_objCEDTO"], "es-MX");
            return Json(medioAmbienteService.CrearEditarCaptura(_objCEDTO, _objFile), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarCaptura(int _idCaptura)
        {
            return Json(medioAmbienteService.EliminarCaptura(_idCaptura), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDatosActualizarCaptura(int _idCaptura)
        {
            return Json(medioAmbienteService.GetDatosActualizarCaptura(_idCaptura), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUltimoConsecutivoCodContenedor(int idAgrupacion, int idAspectoAmbiental, 
                                                                                List<tblS_IncidentesAgrupacionCC> lstIndicentesAgrupacionCC,
                                                                                List<tblS_MedioAmbienteAspectoAmbiental> lstMedioAmbienteAspectoAmbiental,
                                                                                List<tblS_MedioAmbienteCapturaDet> lstMedioAmbienteCapturaDet)
        {
            return Json(medioAmbienteService.GetUltimoConsecutivoCodContenedor(idAgrupacion, idAspectoAmbiental, lstIndicentesAgrupacionCC, lstMedioAmbienteAspectoAmbiental, lstMedioAmbienteCapturaDet), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region TRAYECTOS
        public ActionResult CrearEditarTrayecto(HttpPostedFileBase _objFile)
        {
            CapturaDTO _objCEDTO = JsonUtils.convertJsonToNetObject<CapturaDTO>(Request.Form["_objCEDTO"], "es-MX");
            return Json(medioAmbienteService.CrearEditarTrayecto(_objCEDTO, _objFile), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarTrayecto(int _idCaptura)
        {
            return Json(medioAmbienteService.EliminarTrayecto(_idCaptura), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDatosActualizarTrayecto(int _idCaptura)
        {
            return Json(medioAmbienteService.GetDatosActualizarTrayecto(_idCaptura), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAspectosAmbientalesToTrayectos(int idAgrupacion, string consecutivo)
        {
            return Json(medioAmbienteService.GetAspectosAmbientalesToTrayectos(idAgrupacion, consecutivo), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearAspectoAmbientalAcopioToTrayecto(HttpPostedFileBase objFile)
        {
            AspectosAmbientalesToTrayectosDTO obj = JsonUtils.convertJsonToNetObject<AspectosAmbientalesToTrayectosDTO>(Request.Form["obj"], "es-MX");
            return Json(medioAmbienteService.CrearAspectoAmbientalAcopioToTrayecto(obj, objFile), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region DESTINO FINAL
        public ActionResult CrearEditarDestinoFinal(HttpPostedFileBase _objFile)
        {
            CapturaDTO _objCEDTO = JsonUtils.convertJsonToNetObject<CapturaDTO>(Request.Form["_objCEDTO"], "es-MX");
            return Json(medioAmbienteService.CrearEditarDestinoFinal(_objCEDTO, _objFile), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarDestinoFinal(int _idCaptura)
        {
            return Json(medioAmbienteService.EliminarDestinoFinal(_idCaptura), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDatosActualizarDestinoFinal(int _idCaptura)
        {
            return Json(medioAmbienteService.GetDatosActualizarDestinoFinal(_idCaptura), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAspectosAmbientalesToDestinoFinal(int idAgrupacion, string consecutivo)
        {
            return Json(medioAmbienteService.GetAspectosAmbientalesToDestinoFinal(idAgrupacion, consecutivo), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearDestinoFinal(HttpPostedFileBase objFile)
        {
            AspectosAmbientalesToDestinoFinalDTO objCDTO = JsonUtils.convertJsonToNetObject<AspectosAmbientalesToDestinoFinalDTO>(Request.Form["objCDTO"], "es-MX");
            return Json(medioAmbienteService.CrearDestinoFinal(objCDTO, objFile), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ARCHIVOS REL CAPTURAS
        public ActionResult GetArchivosRelCapturas(CapturaDTO objParamDTO)
        {
            return Json(medioAmbienteService.GetArchivosRelCapturas(objParamDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult VisualizarArchivo(int idArchivo)
        {
            resultado = new Dictionary<string, object>();
            resultado = medioAmbienteService.VisualizarArchivo(idArchivo);

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
        #endregion
        #endregion

        #region TRANSPORTISTAS
        public ActionResult GetTransportistas(TransportistasDTO _objFiltroDTO)
        {
            return Json(medioAmbienteService.GetTransportistas(_objFiltroDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearEditarTransportista(TransportistasDTO _objTransportistaDTO)
        {
            return Json(medioAmbienteService.CrearEditarTransportista(_objTransportistaDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarTransportista(int _idTransportista)
        {
            return Json(medioAmbienteService.EliminarTransportista(_idTransportista), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboClasificacionesTransportistas()
        {
            return Json(medioAmbienteService.FillCboClasificacionesTransportistas(), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region CLASIFICACIÓN TRANSPORTISTAS
        public ActionResult GetClasificacionesTransportistas(ClasificacionTransportistaDTO _objFiltroDTO)
        {
            return Json(medioAmbienteService.GetClasificacionesTransportistas(_objFiltroDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearEditarClasificacionTransportista(ClasificacionTransportistaDTO _objClasificacionTransportistaDTO)
        {
            return Json(medioAmbienteService.CrearEditarClasificacionTransportista(_objClasificacionTransportistaDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarClasificacionTransportista(int _idClasificacionTransportista)
        {
            return Json(medioAmbienteService.EliminarClasificacionTransportista(_idClasificacionTransportista), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region DASHBOARD
        public ActionResult GetGraficas(FiltroDTO objFiltroDTO)
        {
            return Json(medioAmbienteService.GetGraficas(objFiltroDTO), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region GENERALES
        [HttpPost]
        public ActionResult DescargarArchivo()
        {
            CapturaDTO filtro = JsonUtils.convertJsonToNetObject<CapturaDTO>(Request.Form["filtro"], "es-MX");
            var resultadoTupla = medioAmbienteService.DescargarArchivo(filtro._idCaptura, filtro._tipoArchivo);

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
        #endregion

        #region FILL COMBOS
        public ActionResult FillCboAgrupaciones()
        {
            return Json(medioAmbienteService.FillCboAgrupaciones(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboUsuarios()
        {
            return Json(medioAmbienteService.FillCboUsuarios(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboAspectosAmbientales()
        {
            return Json(medioAmbienteService.FillCboAspectosAmbientales(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboTransportistas()
        {
            return Json(medioAmbienteService.FillCboTransportistas(), JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}