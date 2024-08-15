using Core.DTO.Maquinaria.Reporte;
using Data.Factory.Maquinaria.Captura;
using Data.Factory.Maquinaria.Catalogos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Controllers.Maquinaria.Reportes
{
    public class RepProvisionMaquinaController : BaseController
    {
        private MaquinariaRentadaFactoryServices MaquinariaRentadaFactoryServices;
        private CentroCostosFactoryServices centroCostosFactoryServices;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            MaquinariaRentadaFactoryServices = new MaquinariaRentadaFactoryServices();
            centroCostosFactoryServices = new CentroCostosFactoryServices();
            base.OnActionExecuting(filterContext);
        }
        // GET: RepProivionMaquina
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult fnLoadTable(int cc, DateTime fechaCorte, decimal TC, bool TodoReporte)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var centroCosto = centroCostosFactoryServices.getCentroCostosService().getNombreCC(cc);
                var data = MaquinariaRentadaFactoryServices.getMaquinariaRentadaServices().getRptProvisionalInfo(cc, fechaCorte, TC, TodoReporte);

                result.Add("data", data);
                result.Add("centroCosto", cc + "-" + centroCosto);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}