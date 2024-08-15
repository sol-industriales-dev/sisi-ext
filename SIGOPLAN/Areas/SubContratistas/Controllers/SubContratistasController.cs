using Core.DAO.Subcontratistas;
using Core.DTO;
using Core.Entity.SubContratistas;
using Data.Factory.Subcontratistas;
using DotnetDaddy.DocumentViewer;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.SubContratistas.Controllers
{
    public class SubContratistasController : BaseController
    {
        private ISubcontratistasDAO subcontratistasService;

        Dictionary<string, object> result = new Dictionary<string, object>();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            subcontratistasService = new SubcontratistasFactoryService().getSubService();

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

        public ActionResult _menuClick()
        {
            return View();
        }

        public ActionResult SubContratistas()
        {
            return View();
        }

        public ActionResult ConsultaArchivos()
        {
            return View();
        }

        public ActionResult CapturaArchivos()
        {
            return View();
        }

        public ActionResult Contratos()
        {
            return View();
        }

        public ActionResult Proyectos()
        {
            return View();
        }

        public ActionResult Clientes()
        {
            return View();
        }

        #region Catálogos
        #region Subcontratistas
        public ActionResult GetSubcontratistas()
        {
            result = subcontratistasService.getSubcontratistas();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarNuevoSubcontratista(tblX_SubContratista subcontratista)
        {
            result = subcontratistasService.guardarNuevoSubcontratista(subcontratista);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarSubcontratista(tblX_SubContratista subcontratista)
        {
            result = subcontratistasService.editarSubcontratista(subcontratista);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarSubcontratista(tblX_SubContratista subcontratista)
        {
            result = subcontratistasService.eliminarSubcontratista(subcontratista);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Contratos
        public ActionResult GetContratos()
        {
            result = subcontratistasService.getContratos();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarNuevoContrato(tblX_Contrato contrato)
        {
            result = subcontratistasService.guardarNuevoContrato(contrato);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarContrato(tblX_Contrato contrato)
        {
            result = subcontratistasService.editarContrato(contrato);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarContrato(tblX_Contrato contrato)
        {
            result = subcontratistasService.eliminarContrato(contrato);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Proyectos
        public ActionResult GetProyectos()
        {
            result = subcontratistasService.getProyectos();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarNuevoProyecto(tblX_Proyecto proyecto)
        {
            result = subcontratistasService.guardarNuevoProyecto(proyecto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarProyecto(tblX_Proyecto proyecto)
        {
            result = subcontratistasService.editarProyecto(proyecto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarProyecto(tblX_Proyecto proyecto)
        {
            result = subcontratistasService.eliminarProyecto(proyecto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Clientes
        public ActionResult GetClientes()
        {
            result = subcontratistasService.getClientes();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarNuevoCliente(tblX_Cliente cliente)
        {
            result = subcontratistasService.guardarNuevoCliente(cliente);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarCliente(tblX_Cliente cliente)
        {
            result = subcontratistasService.editarCliente(cliente);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarCliente(tblX_Cliente cliente)
        {
            result = subcontratistasService.eliminarCliente(cliente);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        public ActionResult GetSubcontratistasArchivos(int filtroCarga)
        {
            result = subcontratistasService.getSubcontratistasArchivos(filtroCarga);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSubcontratistaByID(int id)
        {
            result = subcontratistasService.getSubcontratistaByID(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarArchivoSubcontratista(HttpPostedFileBase archivo)
        {
            var documentacionID = Convert.ToInt32(Request.Form["documentacionID"]);
            var justificacion = Convert.ToString(Request.Form["justificacion"]);
            DateTime? fechaVencimiento = null;

            if (Request.Form["fechaVencimiento"] != "undefined")
            {
                fechaVencimiento = Convert.ToDateTime(Request.Form["fechaVencimiento"]);
            }

            return Json(subcontratistasService.guardarArchivoSubcontratista(archivo, documentacionID, justificacion, fechaVencimiento), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarArchivoEditadoSubcontratista(HttpPostedFileBase archivo)
        {
            var archivoCargadoID = Convert.ToInt32(Request.Form["archivoCargadoID"]);
            DateTime? fechaVencimiento = null;

            if (Request.Form["fechaVencimiento"] != "undefined")
            {
                fechaVencimiento = Convert.ToDateTime(Request.Form["fechaVencimiento"]);
            }

            return Json(subcontratistasService.guardarArchivoEditadoSubcontratista(archivo, archivoCargadoID, fechaVencimiento), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarArchivoRenovadoSubcontratista(HttpPostedFileBase archivo)
        {
            var archivoCargadoID = Convert.ToInt32(Request.Form["archivoCargadoID"]);
            DateTime? fechaVencimiento = null;

            if (Request.Form["fechaVencimiento"] != "undefined")
            {
                fechaVencimiento = Convert.ToDateTime(Request.Form["fechaVencimiento"]);
            }

            return Json(subcontratistasService.guardarArchivoRenovadoSubcontratista(archivo, archivoCargadoID, fechaVencimiento), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDocumentacionPendiente(int subcontratistaID)
        {
            result = subcontratistasService.getDocumentacionPendiente(subcontratistaID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetHistorialRechazado(int subcontratistaID)
        {
            result = subcontratistasService.getHistorialRechazado(subcontratistaID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarValidacion(List<HttpPostedFileBase> archivos)
        {
            var listaValidacion = (JsonConvert.DeserializeObject<tblX_RelacionSubContratistaDocumentacion[]>(Request.Form["listaValidacion"], new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" })).ToList();

            return Json(subcontratistasService.guardarValidacion(listaValidacion, archivos), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetJustificacionOpcional(int archivoID)
        {
            result = subcontratistasService.getJustificacionOpcional(archivoID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public FileResult getFileDownload()
        {
            try
            {
                int id = Convert.ToInt32(Request.QueryString["id"]);
                var archivo = subcontratistasService.getArchivoSubcontratista(id);

                return File(archivo.rutaArchivo, "multipart/form-data", Path.GetFileName(archivo.rutaArchivo));
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult getFileRuta(int id)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var archivo = subcontratistasService.getArchivoSubcontratista(id);

                result.Add("ruta", archivo.rutaArchivo);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetProveedor(int numeroProveedor)
        {
            result = subcontratistasService.getProveedor(numeroProveedor);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarArchivosFijos()
        {
            result = subcontratistasService.CargarArchivosFijos();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
