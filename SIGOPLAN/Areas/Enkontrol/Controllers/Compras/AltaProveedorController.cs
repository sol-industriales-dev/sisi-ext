using Core.DAO.Enkontrol.Compras;
using Core.DTO.Enkontrol.OrdenCompra;
using Core.Entity.Enkontrol.Compras;
using Data.Factory.Enkontrol.Compras;
using Infrastructure.Utils;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Enkontrol.Controllers.Compras
{
    public class AltaProveedorController : BaseController
    {

        AltaProveedorFactoryService GetAltaProveedorFS;
        IAltaProveedorDAO IAltaProveedorDAO;
        Dictionary<string, object> resultado = new Dictionary<string, object>();
        public const string RUTA_VISTA_ERROR_DESCARGA = "~/Views/Shared/ErrorDescarga.cshtml";
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            GetAltaProveedorFS = new AltaProveedorFactoryService();
            IAltaProveedorDAO = new AltaProveedorFactoryService().GetAltaProveedorFactoryService();

        }

        // GET: Enkontrol/AltaProveedor
        public ActionResult AltaProveedor(string numpro)
        {
            ViewBag.numpro = numpro;
           return PartialView();
        }
        public ActionResult AltaProveedorColombia(string numpro)
        {
            ViewBag.numpro = numpro;
            return PartialView();
        }

        public ActionResult getProveedores()
        {
            var json = Json(GetAltaProveedorFS.GetAltaProveedorFactoryService().getProveedores(), JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }
        public ActionResult obtenerDatosProveedores(int id, int numpro)
        {
            return Json(GetAltaProveedorFS.GetAltaProveedorFactoryService().obtenerDatosProveedores(id, numpro), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarProveedor(HttpPostedFileBase objFile)
        {
            tblCom_sp_proveedoresDTO objProveedor = JsonUtils.convertJsonToNetObject<tblCom_sp_proveedoresDTO>(Request.Form["objProveedor"], "es-MX");
            return Json(GetAltaProveedorFS.GetAltaProveedorFactoryService().GuardarProveedor(objProveedor, objFile), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarEditarProveedorColombia(HttpPostedFileBase objFile)
        {
            tblCom_sp_proveedoresColombiaDTO objProveedor = JsonUtils.convertJsonToNetObject<tblCom_sp_proveedoresColombiaDTO>(Request.Form["objProveedor"], "es-MX");
            return Json(GetAltaProveedorFS.GetAltaProveedorFactoryService().GuardarEditarProveedorColombia(objProveedor, objFile), JsonRequestBehavior.AllowGet);
        }
        public ActionResult AutorizarProveedor(int id)
        {
            return Json(GetAltaProveedorFS.GetAltaProveedorFactoryService().AutorizarProveedor(id), JsonRequestBehavior.AllowGet);
        }
        public ActionResult NotificarAltaProveedor(int id)
        {
            return Json(GetAltaProveedorFS.GetAltaProveedorFactoryService().NotificarAltaProveedor(id), JsonRequestBehavior.AllowGet);
        }
        public ActionResult eliminarProveedor(int id)
        {
            return Json(GetAltaProveedorFS.GetAltaProveedorFactoryService().eliminarProveedor(id), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetArchivosAdjuntos(int idArchivo)
        {
            return Json(GetAltaProveedorFS.GetAltaProveedorFactoryService().GetArchivosAdjuntos(idArchivo), JsonRequestBehavior.AllowGet);
        }
        public ActionResult VisualizarArchivoAdjunto(int idArchivo)
        {
            return Json(GetAltaProveedorFS.GetAltaProveedorFactoryService().VisualizarArchivoAdjunto(idArchivo), JsonRequestBehavior.AllowGet);
        }
        public ActionResult EliminarArchivoAdjunto(int idArchivo)
        {
            return Json(GetAltaProveedorFS.GetAltaProveedorFactoryService().EliminarArchivoAdjunto(idArchivo), JsonRequestBehavior.AllowGet);
        }
        public ActionResult DescargarArchivo(int idArchivo)
        {
            var resultadoTupla = GetAltaProveedorFS.GetAltaProveedorFactoryService().descargarArchivo(idArchivo);

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

        #region GENERALES
        public ActionResult GetLastProveedor(int tipoProveedor)
        {
            resultado.Clear();

            try
            {
                resultado.Add("lastNumPro", GetAltaProveedorFS.GetAltaProveedorFactoryService().GetLastProveedor(tipoProveedor));
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        
        #endregion

        #region FillCombos
        public ActionResult FillComboCiudad()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, GetAltaProveedorFS.GetAltaProveedorFactoryService().FillComboCiudad());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillComboTipoProveedor()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, GetAltaProveedorFS.GetAltaProveedorFactoryService().FillComboTipoProveedor());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillComboTipoTercero()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, GetAltaProveedorFS.GetAltaProveedorFactoryService().FillComboTipoTercero());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillComboTipoOperacion()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, GetAltaProveedorFS.GetAltaProveedorFactoryService().FillComboTipoOperacion());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillComboTipoPagoTerceroTrans()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, GetAltaProveedorFS.GetAltaProveedorFactoryService().FillComboTipoPagoTerceroTrans());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillComboTipoMovBase()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, GetAltaProveedorFS.GetAltaProveedorFactoryService().FillComboTipoMovBase());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillComboTipoMoneda()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, GetAltaProveedorFS.GetAltaProveedorFactoryService().FillComboTipoMoneda());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillComboBancos()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, GetAltaProveedorFS.GetAltaProveedorFactoryService().FillComboBancos());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillComboTipoRegimen()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, GetAltaProveedorFS.GetAltaProveedorFactoryService().FillComboTipoRegimen());
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
    }
}