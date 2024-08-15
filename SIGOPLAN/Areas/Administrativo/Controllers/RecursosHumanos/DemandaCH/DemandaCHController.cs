using Core.DAO.RecursosHumanos.Demandas;
using Core.DTO.RecursosHumanos.Demandas;
using Data.Factory.RecursosHumanos.Demandas;
using DotnetDaddy.DocumentViewer;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Administrativo.Controllers.RecursosHumanos.Demandas
{
    public class DemandaCHController : BaseController
    {
        #region CONSTRUCTOR
        IDemandaCHDAO iDemandaDAO;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            iDemandaDAO = new DemandaCHFactoryService().GetDemandaDAO();
            base.OnActionExecuting(filterContext);
        }
        #endregion

        #region VISTAS
        public ActionResult Captura()
        {
            return View();
        }

        public ActionResult Seguimiento()
        {
            return View();
        }

        public ActionResult Historico()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            return View();
        }
        #endregion

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

        #region CAPTURAS
        public ActionResult GetCapturas(CapturaDTO objDTO)
        {
            return Json(iDemandaDAO.GetCapturas(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetInformacionEmpleado(CapturaDTO objDTO)
        {
            return Json(iDemandaDAO.GetInformacionEmpleado(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CECaptura(CapturaDTO objDTO)
        {
            return Json(iDemandaDAO.CECaptura(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarCaptura(CapturaDTO objDTO)
        {
            return Json(iDemandaDAO.EliminarCaptura(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboSemaforo()
        {
            return Json(iDemandaDAO.FillCboSemaforo(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDatosActualizarCaptura(CapturaDTO objDTO)
        {
            return Json(iDemandaDAO.GetDatosActualizarCaptura(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboJuzgados()
        {
            return Json(iDemandaDAO.FillCboJuzgados(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboTipoDemandas()
        {
            return Json(iDemandaDAO.FillCboTipoDemandas(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CerrarDemanda(int idCaptura)
        {
            return Json(iDemandaDAO.CerrarDemanda(idCaptura), JsonRequestBehavior.AllowGet);
        }

        #region ARCHIVOS ADJUNTOS
        public ActionResult GuardarArchivoAdjunto(List<HttpPostedFileBase> lstArchivos, int FK_Captura, int tipoArchivo)
        {
            return Json(iDemandaDAO.GuardarArchivoAdjunto(lstArchivos, FK_Captura, tipoArchivo), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetArchivosAdjuntos(int FK_Captura)
        {
            return Json(iDemandaDAO.GetArchivosAdjuntos(FK_Captura), JsonRequestBehavior.AllowGet);
        }

        public ActionResult VisualizarArchivoAdjunto(int idArchivo)
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();
            resultado = iDemandaDAO.VisualizarArchivoAdjunto(idArchivo);

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

        public ActionResult EliminarArchivoAdjunto(int idArchivo)
        {
            return Json(iDemandaDAO.EliminarArchivoAdjunto(idArchivo), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboTipoArchivos()
        {
            return Json(iDemandaDAO.FillCboTipoArchivos(), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region SEGUIMIENTOS
        public ActionResult CrearSeguimiento(SeguimientoDTO objDTO)
        {
            return Json(iDemandaDAO.CrearSeguimiento(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEstatusFiniquitoEmpleadoDemanda(int claveEmpleado)
        {
            return Json(iDemandaDAO.GetEstatusFiniquitoEmpleadoDemanda(claveEmpleado), JsonRequestBehavior.AllowGet);
        }

        public ActionResult DescargarArchivoAdjunto(string rutaArchivo)
        {
            return File(rutaArchivo, "multipart/form-data", Path.GetFileName(rutaArchivo));
        }
        #endregion

        #region HISTORICO
        public ActionResult GetHistorico(HistoricoDTO objDTO)
        {
            return Json(iDemandaDAO.GetHistorico(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetLstSeguimientos(int FK_Captura)
        {
            return Json(iDemandaDAO.GetLstSeguimientos(FK_Captura), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region DASHBOARD
        public ActionResult GetDashboard(DashboardDTO objFiltroDTO)
        {
            return Json(iDemandaDAO.GetDashboard(objFiltroDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboAnios()
        {
            return Json(iDemandaDAO.FillCboAnios(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboCCDemandasRegistradas()
        {
            return Json(iDemandaDAO.FillCboCCDemandasRegistradas(), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region GENERALES
        public ActionResult FillCboCC()
        {
            return Json(iDemandaDAO.FillCboCC(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboEstados()
        {
            return Json(iDemandaDAO.FillCboEstados(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboEstatus()
        {
            return Json(iDemandaDAO.FillCboEstatus(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboEmpleados()
        {
            return Json(iDemandaDAO.FillCboEmpleados(), JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}