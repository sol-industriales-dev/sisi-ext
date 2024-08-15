using Core.DTO.Kubrix;
using Core.Entity.Kubrix;
using Data.Factory.Contabilidad;
using Data.Factory.Contabilidad.Reportes;
using Data.Factory.Kubrix;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Kubrix.Controllers
{
    public class BaseDatosController : BaseController
    {
        #region Factory
        BaseDatosFactoryServices BDFS = new BaseDatosFactoryServices();
        CadenaProductivaFactoryServices CPFS = new CadenaProductivaFactoryServices();
        PolizaFactoryServices polizaFS = new PolizaFactoryServices();
        #endregion
        // GET: Kubrix/BaseDatos
        #region Vistas
        public ActionResult Vencimiento()
        {
            return View();
        }
        public ActionResult CXP()
        {
            return View();
        }
        public ActionResult CXPDetalle()
        {
            return View();
        }
        public ActionResult Maq()
        {
            return View();
        }
        public ActionResult Avance()
        {
            return View();
        }
        public ActionResult Saldos()
        {
            ViewBag.cc = "001";
            ViewBag.anio = DateTime.Now.Year;
            return View();
        }
        public ActionResult Bal12()
        {
            ViewBag.cc = "001";
            ViewBag.anio = DateTime.Now.Year;
            return View();
        }
        public ActionResult _tblVencimiento()
        {
            return View();
        }
        public ActionResult _tblCxp()
        {
            return View();
        }
        public ActionResult _tblCxpDet(string cc)
        {
            Session["cc"] = cc;
            return View();
        }
        public ActionResult _tblSaldos(string cc, int anio)
        {
            Session["cc"] = cc;
            Session["anio"] = anio;
            return View();
        }
        public ActionResult _tblBal12(string cc, int anio)
        {
            Session["cc"] = cc;
            Session["anio"] = anio;
            return View();
        }
        public ActionResult _tblMaq(DateTime fechaInicio, DateTime fechaFin)
        {
            Session["fechaInicio"] = fechaInicio;
            Session["fechaFin"] = fechaFin;
            return View();
        }
        public ActionResult _tblArchivos()
        {
            return View();
        }
        public ActionResult CapturaMaq()
        {
            return View();
        }
        #endregion
        public List<VencimientoDTO> lstVencimiento()
        {
            return BDFS.getBDServices().lstVencimiento();
        }
        public async Task<Dictionary<string, object>> lstSalContCC(string cc, int anio)
        {
            var result = new Dictionary<string, object>();
            var tlst = Task.Factory.StartNew(() => BDFS.getBDServices().lstSalContCC(anio).Where(w => w.cc.Equals(cc)).ToList());
            var tlstCC = Task.Factory.StartNew(() => CPFS.getCadenaProductivaService().lstObra());
            var tlstCta = Task.Factory.StartNew(() => polizaFS.getPolizaService().getCuenta());
            result.Add("lst", tlst.Result);
            result.Add("lstCC", tlstCC.Result);
            result.Add("lstCta", tlstCta.Result);
            result.Add(SUCCESS, true);
            await Task.WhenAll(tlst, tlstCC, tlstCta);
            return result;
        }
        public ActionResult lstAnioSaldos()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var hoy = DateTime.Now.Year;
                var lst = new List<int>() { 
                    hoy, 
                    hoy - 1
                };
                result.Add(ITEMS, lst.Select(x => new
                {
                    Text = x == hoy ? "Actual" : "Anterior",
                    Value = x,
                }));
                result.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public List<object> lstMaq(DateTime fechaInicio, DateTime fechaFin)
        {
            return BDFS.getBDServices().getInfoMaquinaria(fechaInicio, fechaFin);
        }
        public ActionResult lstCapturaMaq(string cc, string fechaInicio, string fechaFin)
        {
            var result = new Dictionary<string, object>();

            //var fechaInicioDate = DateTime.Now.Date;
            //var fechaFinDate = DateTime.Now.AddDays(-7).Date;

            DateTime fechaInicioDate;

            if (fechaInicio != "")
            {
                fechaInicioDate = DateTime.Parse(fechaInicio);
            }
            else
            {
                fechaInicioDate = DateTime.Now.Date;
            }

            DateTime fechaFinDate;
            if (fechaFin != "")
            {
                if (DateTime.Parse(fechaFin) < fechaInicioDate)
                {
                    fechaFinDate = DateTime.Parse(fechaFin);
                }
                else
                {
                    fechaFinDate = fechaInicioDate.AddDays(-7).Date;
                }
            }
            else
            {
                fechaFinDate = fechaInicioDate.AddDays(-7).Date;
            }

            try
            {
                var obj = BDFS.getBDServices().getInfoCapturaMaquinaria(fechaInicioDate, fechaFinDate, cc);
                result.Add("data", obj);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public void CapturarMaq(List<CapturaMaqDTO> arr)
        {
            try
            {
                BDFS.getBDServices().CapturarMaq(arr);
            }
            catch (Exception)
            {

            }
        }

        public List<tblK_CatAvance> lstArchivos()
        {
            return BDFS.getBDServices().lstArchivos();
        }
    }
}