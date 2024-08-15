using Core.DTO.Maquinaria.Captura;
using Core.DTO.Maquinaria.Catalogos;
using Core.Entity.Maquinaria.Captura;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Principal.Multiempresa;
using Data.Factory.Maquinaria.Captura;
using Data.Factory.Maquinaria.Catalogos;
using Data.Factory.Maquinaria.Reporte;
using Data.Factory.RecursosHumanos.ReportesRH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Controllers.Maquinaria.Capturas.MaquinariaRentada
{
    public class MaquinariaRentadaController : BaseController
    {
        private CapturaHorometroFactoryServices HorometroFactory;
        private MaquinariaRentadaFactoryServices MaquinariaRentadaFactory;
        private RN_MaquinariaFactoryServices RN_MaquinariaFactory;
        private MaquinaFactoryServices MaquinaFactory;
        private RptIndicadorFactoryServices RptIndicadorFactoryServices;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HorometroFactory = new CapturaHorometroFactoryServices();
            MaquinariaRentadaFactory = new MaquinariaRentadaFactoryServices();
            RN_MaquinariaFactory = new RN_MaquinariaFactoryServices();
            MaquinaFactory = new MaquinaFactoryServices();
            RptIndicadorFactoryServices = new RptIndicadorFactoryServices();
            base.OnActionExecuting(filterContext);
        }
        // GET: MaquinariaRentada
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Captura()
        {
            return View();
        }

        public ActionResult RepInidicador()
        {
            return View();
        }

        //Actions nueva renta
        [HttpPost]
        public JsonResult GetAreasCuentaPorUsuario()
        {
            var respuesta = RN_MaquinariaFactory.getRN_MaquinariaServices().GetAreasCuentaPorUsuario();
            return Json(respuesta);
        }

        [HttpPost]
        public JsonResult GetCentrosCostoRentados(List<int> idAreasCuenta)
        {
            var respuesta = RN_MaquinariaFactory.getRN_MaquinariaServices().GetCentrosCostoRentados(idAreasCuenta);
            return Json(respuesta);
        }

        [HttpPost]
        public JsonResult GetMaquinasRentadas(List<int> idAreaCuenta, List<int> idCentroCosto, DateTime periodoDel, DateTime periodoA)
        {
            var respuesta = RN_MaquinariaFactory.getRN_MaquinariaServices().GetMaquinasRentadas(idAreaCuenta, idCentroCosto, periodoDel, periodoA);
            return Json(respuesta);
        }

        [HttpGet]
        public JsonResult GetInfoMaquinaRentada(int idMaquinaRentada)
        {
            var respuesta = RN_MaquinariaFactory.getRN_MaquinariaServices().GetInfoMaquinaRentada(idMaquinaRentada);
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult TerminarRentaMaquina(int idRentaMaquina)
        {
            var respuesta = RN_MaquinariaFactory.getRN_MaquinariaServices().TerminarRentaMaquina(idRentaMaquina);
            return Json(respuesta);
        }

        [HttpPost]
        public JsonResult GetCentrosCosto()
        {
            var respuesta = RN_MaquinariaFactory.getRN_MaquinariaServices().GetCentrosCosto();
            return Json(respuesta);

        }

        [HttpGet]
        public JsonResult GetInformacionCentroCosto(int idCentroCosto)
        {
            var respuesta = RN_MaquinariaFactory.getRN_MaquinariaServices().GetInformacionCentroCosto(idCentroCosto);
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RegistrarRenta(tblM_RN_Maquinaria informacionRenta, string tipoRenta)
        {
            var respuesta = RN_MaquinariaFactory.getRN_MaquinariaServices().RegistrarRenta(informacionRenta, tipoRenta);
            return Json(respuesta);
        }

        [HttpGet]
        public JsonResult GetHorometroPorPeriodoYCentroCosto(DateTime periodoInicio, DateTime periodoFinal, string Cc, int horometroInicial = 0, int horometroFinal = 0)
        {
            var respuesta = RN_MaquinariaFactory.getRN_MaquinariaServices().GetHorometroPorPeriodoYCentroCosto(periodoInicio, periodoFinal, Cc, horometroInicial, horometroFinal);
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }
        //Actions nueva renta fin
    }
}