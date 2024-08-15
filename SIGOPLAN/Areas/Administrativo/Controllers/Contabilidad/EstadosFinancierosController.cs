using Core.DAO.Contabilidad;
using Core.DTO;
using Core.Entity.Administrativo.Contabilidad;
using Core.Enum.Contabilidad.EstadoFinanciero;
using Core.Enum.Multiempresa;
using Data.Factory.Contabilidad;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Administrativo.Controllers.Contabilidad
{
    public class EstadosFinancierosController : BaseController
    {
        private IEstadosFinancierosDAO estadosFinancierosService;

        Dictionary<string, object> result = new Dictionary<string, object>();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            estadosFinancierosService = new EstadosFinancierosFactoryService().GetEstadosFinancierosService();

            result.Clear();

            base.OnActionExecuting(filterContext);
        }

        public ActionResult DeterminacionResultado()
        {
            return View();
        }

        public ActionResult GuardarBalanza()
        {
            return View();
        }

        public ActionResult EstadoResultados()
        {
            return View();
        }

        public ActionResult CalcularBalanza(DateTime fechaAnioMes)
        {
            return Json(estadosFinancierosService.calcularBalanza(fechaAnioMes), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarBalanzaCorte(DateTime fechaAnioMes)
        {
            return Json(estadosFinancierosService.guardarBalanzaCorte(fechaAnioMes), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboCC()
        {
            return Json(estadosFinancierosService.FillComboCC(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CalcularEstadoResultados(List<EmpresaEnum> listaEmpresas, DateTime fechaAnioMes, List<string> listaCC)
        {
            var respuesta = estadosFinancierosService.calcularEstadoResultados(listaEmpresas, fechaAnioMes, listaCC);

            if ((bool)respuesta[SUCCESS])
            {
                var configuracinReporte = new EstadoResultadoConfiguracionDTO();
                configuracinReporte.fecha = fechaAnioMes;
                configuracinReporte.listaEmpresas = listaEmpresas;
                configuracinReporte.listaColumnas = respuesta["listaColumnas"] as List<Tuple<string, string, int>>;
                configuracinReporte.listaDatos = respuesta["listaDatosDataTable"] as List<Dictionary<string, object>>;
                
                Session["rptEstadoResultados"] = configuracinReporte;
            }

            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEstadoResultadoDetalle(List<EmpresaEnum> listaEmpresas, DateTime fechaAnioMes, List<string> listaCC, int tipoBusqueda)
        {
            var respuesta = estadosFinancierosService.GetEstadoResultadoDetalle(listaEmpresas, fechaAnioMes, listaCC, tipoBusqueda);
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EliminarBalanza(DateTime fechaCorte)
        {
            return Json(estadosFinancierosService.EliminarBalanza(fechaCorte));
        }

        #region Balance
        public ActionResult BalanceGeneral()
        {
            return View();
        }

        [HttpPost]
        public JsonResult CalcularBalanceGeneral(List<EmpresaEnum> listaEmpresas, DateTime fechaAnioMes, List<string> listaCC, TipoBalanceEnum tipoBalance)
        {
            #region SE CREA VARIABLES DE SESION PARA REPORTE ESTADO POSICION FINANCIERO
            if (listaEmpresas != null)
                Session["listaEmpresas"] = listaEmpresas.ToList();
            else
                Session["listaEmpresas"] = null;

            if (listaCC != null)
                Session["listaCC"] = listaCC.ToList();
            else
                Session["listaCC"] = null;

            Session["fechaAnioMes"] = fechaAnioMes;
            #endregion

            var respuesta = estadosFinancierosService.CalcularBalanceGeneral(listaEmpresas, fechaAnioMes, listaCC, tipoBalance);

            return Json(respuesta);
        }

        public ActionResult GetBalanceDetalle(List<EmpresaEnum> listaEmpresas, DateTime fechaMesCorte, List<string> listaCC, TipoDetalleEnum tipoDetalle, int tipoTablaGeneral)
        {
            return Json(estadosFinancierosService.GetBalanceDetalle(listaEmpresas, fechaMesCorte, listaCC, tipoDetalle, tipoTablaGeneral), JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
