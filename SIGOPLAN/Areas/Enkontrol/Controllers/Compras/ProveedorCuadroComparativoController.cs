using Core.DAO.Enkontrol.Compras;
using Core.DTO;
using Core.DTO.Enkontrol.OrdenCompra;
using Data.Factory.Enkontrol.Compras;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Enkontrol.Controllers.Compras
{
    public class ProveedorCuadroComparativoController : BaseController
    {
        ProveedorCuadroComparativoFactoryService GetProvCuadroComprativoFS;
        IProveedorCuadroComparativoDAO IProveedorCuadroComparativoDAO;
        Dictionary<string, object> resultado = new Dictionary<string, object>();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            GetProvCuadroComprativoFS = new ProveedorCuadroComparativoFactoryService();
            IProveedorCuadroComparativoDAO = new ProveedorCuadroComparativoFactoryService().GetProvCuadroComprativoFS();

            //Core.DTO.Principal.Usuarios.UsuarioDTO usuarioDTO = new Core.DTO.Principal.Usuarios.UsuarioDTO();
            //usuarioDTO.esCliente = false;
            //vSesiones.sesionUsuarioDTO = usuarioDTO;

            //List<Core.DTO.Sistemas.tblP_EmpresasDTO> lstEmpresas = new List<Core.DTO.Sistemas.tblP_EmpresasDTO>();
            //Core.DTO.Sistemas.tblP_EmpresasDTO objEmpresa = new Core.DTO.Sistemas.tblP_EmpresasDTO();
            //objEmpresa.url = string.Empty;
            //lstEmpresas.Add(objEmpresa);
            //vSesiones.sesionUsuarioDTO.empresas = lstEmpresas;
        }

        #region VIEWS
        public ActionResult ProveedorCuadroComparativo()
        {
            return View();
        }

        public ActionResult ExitoCapturaProveedorCuadroComparativo()
        {
            return View();
        }

        public ActionResult ErrorProveedorCuadroComparativo()
        {
            return View();
        }
        #endregion

        #region
        public ActionResult VerificarProveedorRelHash(string hash)
        {
            return Json(GetProvCuadroComprativoFS.GetProvCuadroComprativoFS().VerificarProveedorRelHash(hash), JsonRequestBehavior.AllowGet);

            //var resultados = new Dictionary<string, object>();
            //var resultado = GetProvCuadroComprativoFS.GetProvCuadroComprativoFS().VerificarProveedorRelHash(hash);
            //resultados.Add("SUCCESS", resultado[SUCCESS]);
            //bool error = Convert.ToBoolean(resultados["SUCCESS"]);

            //}
            //else
            //{
            //    return Json(resultado, JsonRequestBehavior.AllowGet);
            //}
            //return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDatosProveedor(string hash)
        {
            return Json(GetProvCuadroComprativoFS.GetProvCuadroComprativoFS().GetDatosProveedor(hash), JsonRequestBehavior.AllowGet);

            //var resultados = new Dictionary<string, object>();
            //var resultado = GetProvCuadroComprativoFS.GetProvCuadroComprativoFS().GetDatosProveedor(hash);
            //resultados.Add("SUCCESS", resultado[SUCCESS]);
            //bool error = Convert.ToBoolean(resultados["SUCCESS"]);
            //if (!error)
            //{
            //    //return View("ErrorProveedorCuadroComparativo");
            //}
            //else
            //{
            //    return Json(resultado, JsonRequestBehavior.AllowGet);
            //}
            //return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarCuadroComparativo(HttpPostedFileBase archivo)
        {
            var cuadro = JsonConvert.DeserializeObject<CuadroComparativoDTO>(Request.Form["cuadro"], new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });

            return Json(GetProvCuadroComprativoFS.GetProvCuadroComprativoFS().GuardarCuadroComparativo(cuadro, archivo), JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}