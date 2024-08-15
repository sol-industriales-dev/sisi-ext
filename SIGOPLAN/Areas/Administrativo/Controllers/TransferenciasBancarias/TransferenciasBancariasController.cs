using Core.DAO.Administracion.TransferenciasBancarias;
using Core.DTO.Administracion.TransferenciasBancarias;
using Core.Enum.Administracion.TransferenciasBancarias;
using Data.Factory.Administracion.TransferenciasBancarias;
using Infrastructure.Utils;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Administrativo.Controllers.TransferenciasBancarias
{
    public class TransferenciasBancariasController : BaseController
    {
        private ITransferenciasBancariasDAO transferenciasBancariasService;
        Dictionary<string, object> resultado = new Dictionary<string, object>();
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            transferenciasBancariasService = new TransferenciasBancariasFactoryService().getTransferenciasBancariasService();
            resultado.Clear();
            base.OnActionExecuting(filterContext);
        }

        public ActionResult TransferenciasBancarias()
        {
            return View();
        }

        public ActionResult CargarMovimientosProveedorAutorizados(int proveedorInicial, int proveedorFinal, DateTime fechaInicial, DateTime fechaFinal)
        {
            var json = Json(transferenciasBancariasService.CargarMovimientosProveedorAutorizados(proveedorInicial, proveedorFinal, fechaInicial, fechaFinal), JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        public ActionResult CargarArchivoComprimido(List<RegistroArchivoDTO> registros)
        {
            try
            {
                var resultadoTupla = transferenciasBancariasService.CargarArchivoComprimido(registros);

                if (resultadoTupla != null)
                {
                    Session["archivoComprimido"] = resultadoTupla;
                }
                else
                {
                    return View("ErrorDescarga");
                }

                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DescargarArchivoComprimido()
        {
            try
            {
                var archivoComprimido = (Tuple<Stream, string>)Session["archivoComprimido"];

                string nombreArchivo = archivoComprimido.Item2;
                string tipo = MimeMapping.GetMimeMapping(nombreArchivo);

                var fileStreamResult = new FileStreamResult(archivoComprimido.Item1, tipo);
                fileStreamResult.FileDownloadName = nombreArchivo;

                return fileStreamResult;
            }
            catch (Exception)
            {
                return View("ErrorDescarga");
            }
        }

        public ActionResult GetOperacionEnum()
        {
            return Json(new { items = GlobalUtils.ParseEnumToCombo<OperacionEnum>() }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GenerarCheques(List<FacturaDTO> facturas, int cuentaBancaria)
        {
            return Json(transferenciasBancariasService.GenerarCheques(facturas, cuentaBancaria), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboCuentasBancarias()
        {
            return Json(transferenciasBancariasService.FillComboCuentasBancarias(), JsonRequestBehavior.AllowGet);
        }
    }
}