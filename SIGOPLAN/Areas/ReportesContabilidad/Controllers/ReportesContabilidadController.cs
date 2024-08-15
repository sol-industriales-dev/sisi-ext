using Core.DAO.ReportesContabilidad;
using Data.Factory.ReportesContabilidad;
using DotnetDaddy.DocumentViewer;
using Infrastructure.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.ReportesContabilidad
{
    public class ReportesContabilidadController : BaseController
    {
        IReportesContabilidadDAO reportesContabilidadFS;
        Dictionary<string, object> result = new Dictionary<string, object>();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            reportesContabilidadFS = new ReportesContabilidadFactoryService().getReportesContabilidad();
            base.OnActionExecuting(filterContext);
        }      

        public ActionResult ReporteAuxiliarEnkontrol()
        {
            return View();
        }

        public ActionResult CargarAuxiliarEnkontrol(string fechaInicio, string fechaFin, string ctaInicio, string ctaFin, string areaCuenta)
        {
            List<string> _areaCuenta = JsonConvert.DeserializeObject<List<string>>(areaCuenta);
            fechaInicio = fechaInicio.Replace('\"', ' ');
            fechaFin = fechaFin.Replace('\"', ' ');
            DateTime _fechaInicio = DateTime.Parse(fechaInicio);
            DateTime _fechaFin = DateTime.Parse(fechaFin);
            var json = Json(reportesContabilidadFS.cargarAuxiliarEnkontrol(_fechaInicio, _fechaFin, ctaInicio, ctaFin, _areaCuenta), JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = Int32.MaxValue;
            return json;

        }

        public ActionResult GetListaAC()
        {
            return Json(reportesContabilidadFS.getListaAC(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCuentas(string term)
        {
            var json = Json(reportesContabilidadFS.GetCuentas(term), JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = Int32.MaxValue;
            return json;
        }
    }
}
