using Core.DAO.Contabilidad.Poliza;
using Core.DAO.Contabilidad.SistemaContable;
using Core.DTO.Contabilidad.Poliza;
using Core.DTO.Contabilidad.SistemaContable.Cuentas;
using Core.Entity.Administrativo.Contabilidad.Poliza;
using Core.Entity.Administrativo.Contabilidad.Sistema_Contable.Cuentas;
using Core.Enum.Multiempresa;
using Data.Factory.Contabilidad.Poliza;
using Data.Factory.Contabilidad.SistemaContable;
using Infrastructure.Utils;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Administrativo.Controllers.Contabilidad.Poliza
{
    public class ConversionPolizaController : BaseController
    {
        private Dictionary<string, object> resultado;
        IConversionPolizaDAO conPolizaFS;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            resultado = new Dictionary<string, object>();
            conPolizaFS = new ConversionPolizaFactoryServices().getPolizaService();
            base.OnActionExecuting(filterContext);
        }
        public ActionResult ConversionPoliza()
        {
            return View();
        }
        public ActionResult CargarPolizas(int poliza, int year, int mes)
        {
            return Json(conPolizaFS.CargarPolizas(poliza, year, mes), JsonRequestBehavior.AllowGet);
        }
        public ActionResult BuscarCaptura(DateTime fechaConversion)
        {
            return Json(conPolizaFS.BuscarCaptura(fechaConversion), JsonRequestBehavior.AllowGet);
        }
        public ActionResult CargarConversionPolizas(int year, int mes, string tp = "", int poliza = 0, string monto = "")
        {
            decimal _monto = 0;
            Decimal.TryParse(monto, out _monto);
            return Json(conPolizaFS.CargarConversionPolizas(year, mes, tp, poliza, _monto), JsonRequestBehavior.AllowGet);
        }
        public ActionResult ObtenerPolizaDetalle(int year, int mes, string tp, int empresa, int poliza = 0)
        {
            return Json(conPolizaFS.ObtenerPolizaDetalle(year, mes, tp, poliza, empresa), JsonRequestBehavior.AllowGet);
        }
        public ActionResult ObtenerPolizaDetallePeru(int year, int mes, string tp, int empresa, int poliza = 0)
        {
            return Json(conPolizaFS.ObtenerPolizaDetallePeru(year, mes, tp, poliza, empresa), JsonRequestBehavior.AllowGet);
        }
        public ActionResult saveOrUpdatePolizaSession(List<tblC_SC_ConversionPoliza> relCtas)
        {
            try
            {
                var lstSession = (List<tblC_SC_ConversionPoliza>)Session["ConversionPoliza"];
                if(lstSession == null)
                {
                    lstSession = new List<tblC_SC_ConversionPoliza>();
                }
                lstSession.AddRange(relCtas);
                Session["ConversionPoliza"] = lstSession;
                var esSession = lstSession.Any();
                resultado.Add(SUCCESS, esSession);
                if(!esSession)
                {
                    Session["ConversionPoliza"] = null;
                }
            }
            catch(Exception o_O)
            {
                Session["ConversionPoliza"] = null;
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, o_O.Message);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        public ActionResult saveOrUpdatePoliza(DateTime fechaConversion, bool aplicaFechaPoliza)
        {
            var listaPolizas = (List<tblC_SC_ConversionPoliza>)Session["ConversionPoliza"];
            var resultado = conPolizaFS.saveOrUpdatePoliza(listaPolizas, fechaConversion, aplicaFechaPoliza);
            Session["ConversionPoliza"] = null;
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ObtenerPolizaEnkontrol(PolizasDTO poliza, EmpresaEnum empresa)
        {
            return Json(conPolizaFS.ObtenerPolizaEnkontrol(poliza, empresa), JsonRequestBehavior.AllowGet);
        }
    }
}