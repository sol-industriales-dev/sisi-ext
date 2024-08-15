using Data.Factory.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data.DAO.Contabilidad.Poliza;
using Data.Factory.Contabilidad;
using Core.Entity.Administrativo.Contabilidad;
using Core.DTO.Principal.Generales;
using Core.DTO.Contabilidad;

namespace SIGOPLAN.Areas.Administrativo.Controllers.Contabilidad.Poliza
{
    public class ConciliacionCCController : Controller
    {
        private UsuarioFactoryServices usuarioFactoryServices;
        private ConciliacionCCFactoryServices conciliacionCCFactoryServices = new ConciliacionCCFactoryServices();
        Dictionary<string, object> result;
 
        // GET: Administrativo/ConciliacionCC
        public ActionResult conciliacionCC()
        {
            return View();
        }
        public ActionResult FillCCPrincipal()
        {
            result = conciliacionCCFactoryServices.GetConciliacionCCService().FillCCPrincipal();
            return Json(result, JsonRequestBehavior.AllowGet); ;
        }
        public ActionResult FillCCSecundario()
        {
            result = conciliacionCCFactoryServices.GetConciliacionCCService().FillCCSecundario();
            return Json(result, JsonRequestBehavior.AllowGet); ;
        }

        public ActionResult GuardarEditarConciliacionCC(tblC_Cta_RelCC data)
        {
            result = conciliacionCCFactoryServices.GetConciliacionCCService().GuardarEditarConciliacionCC(data);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EliminarConciliacionCC(int id)
        {
            result = conciliacionCCFactoryServices.GetConciliacionCCService().EliminarConciliacionCC(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetBuscarConciliacionCC(List<string> palEmpresaCC)
        {
            result = conciliacionCCFactoryServices.GetConciliacionCCService().GetBuscarConciliacionCC(palEmpresaCC);
            return Json(result, JsonRequestBehavior.AllowGet);
        }





    }
}