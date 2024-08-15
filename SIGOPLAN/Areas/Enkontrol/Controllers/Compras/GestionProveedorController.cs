using Core.DAO.Enkontrol.Compras;
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
    public class GestionProveedorController :  BaseController
    {
        GestionProveedorFactoryService GetGestionProveedorFS;
        IGestionProveedorDAO IGestionProveedorDAO;
        Dictionary<string, object> resultado = new Dictionary<string, object>();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            GetGestionProveedorFS = new GestionProveedorFactoryService();
            IGestionProveedorDAO = new GestionProveedorFactoryService().GetGestionProveedorFS();

        }
        // GET: Enkontrol/GestionProveedores
        public ActionResult GestionProveedores(string id)
        {
            ViewBag.idProv = id;
            return View();
        }

        public ActionResult getProveedores()
        {
            return Json(GetGestionProveedorFS.GetGestionProveedorFS().getProveedores(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult getDatosProveedores(int id)
        {
            return Json(GetGestionProveedorFS.GetGestionProveedorFS().getDatosProveedores(id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarEditarProveedor(HttpPostedFileBase objFile)
        {
            tblCom_MAEPROV proveedor = JsonUtils.convertJsonToNetObject<tblCom_MAEPROV>(Request.Form["proveedor"], "es-MX");
            resultado = GetGestionProveedorFS.GetGestionProveedorFS().GuardarEditarProveedor(proveedor, objFile);
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardadoCajaChica(HttpPostedFileBase objFile)
        {
            tblCom_MAEPROV proveedor = JsonUtils.convertJsonToNetObject<tblCom_MAEPROV>(Request.Form["proveedor"], "es-MX");
            resultado = GetGestionProveedorFS.GetGestionProveedorFS().GuardadoCajaChica(proveedor, objFile);
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        

        public ActionResult eliminarProveedor(int id)
        {
            resultado = GetGestionProveedorFS.GetGestionProveedorFS().eliminarProveedor(id);
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AutorizarProveedor(int id)
        {
            resultado = GetGestionProveedorFS.GetGestionProveedorFS().AutorizarProveedor(id);
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getCuentasBancosProveedores(string anexo)
        {
            return Json(GetGestionProveedorFS.GetGestionProveedorFS().getCuentasBancosProveedores(anexo), JsonRequestBehavior.AllowGet);
        }
        public ActionResult getDatosCuentasBancoProveedor(int id)
        {
            return Json(GetGestionProveedorFS.GetGestionProveedorFS().getDatosCuentasBancoProveedor(id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult guardarEditarCuentaBancoProveedor(tblCom_CuentasBancosProveedor CuentasBancosProveedor)
        {
            resultado = GetGestionProveedorFS.GetGestionProveedorFS().guardarEditarCuentaBancoProveedor(CuentasBancosProveedor);
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult eliminarCuentaBancoProveedor(int id)
        {
            resultado = GetGestionProveedorFS.GetGestionProveedorFS().eliminarCuentaBancoProveedor(id);
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult NotificarAltaProveedor(int id)
        {
            resultado = GetGestionProveedorFS.GetGestionProveedorFS().NotificarAltaProveedor(id);
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        #region ARCHIVOS
        public ActionResult RemoveArchivos(int idArchivo)
        {
            return Json(GetGestionProveedorFS.GetGestionProveedorFS().RemoveArchivos(idArchivo), JsonRequestBehavior.AllowGet);
        }

        public ActionResult DescargarArchivo(int idArchivo)
        {
            var resultadoTupla = GetGestionProveedorFS.GetGestionProveedorFS().descargarArchivo(idArchivo);

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
    }
}