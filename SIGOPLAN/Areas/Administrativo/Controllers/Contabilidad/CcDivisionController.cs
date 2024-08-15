using Core.Entity.Administrativo.Contabilidad.Propuesta.Acomulado;
using Core.Enum.Administracion.Propuesta;
using Data.Factory.Contabilidad;
using Data.Factory.Contabilidad.Propuesta;
using Infrastructure.Utils;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Administrativo.Controllers.Contabilidad
{
    public class CcDivisionController : BaseController
    {
        #region Factory
        private CCDivisionFactoryServices divFS;
        private ChequeFactoryServices chequeFS;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            divFS = new CCDivisionFactoryServices();
            chequeFS = new ChequeFactoryServices();
            base.OnActionExecuting(filterContext);
        }
        #endregion
        // GET: Administrativo/CcDivision
        public ActionResult _divCcDivisiones()
        {
            return PartialView();
        }
        public ActionResult _RelCuentaDivision()
        {
            return PartialView();
        }
        public ActionResult getLstCuenta()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstDiv = EnumExtensions.ToCombo<TipoCCEnum>();
                var lstRel = divFS.getCcDivisionService().getLstRelCtaDiv();
                var lstCta = chequeFS.getChequeService().getLstCuenta(lstRel.GroupBy(s => s.cuenta.ToString()).Select(s => s.Key).ToList());
                lstCta.ForEach(cta => {
                    cta.descDivision = lstDiv.Where(d => lstRel.Any(r => r.division.Equals(d.Value.ParseInt()) && cta.cuenta.Equals(r.cuenta))).Select(d => d.Text).ToList().ToLine(" ");
                    cta.esActivo = lstRel.Where(r => r.cuenta.Equals(cta.cuenta)).All(r => r.esActivo);
                });
                result.Add("lst", lstCta);
                result.Add(SUCCESS, lstCta.Count > 0);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult setRelDivCta(List<tblC_RelCuentaDivision> lst)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var esGuardado = false;
                if (lst.Any(r => r.cuenta > 0 && r.division > 0))
                {
                    esGuardado = divFS.getCcDivisionService().Guardar(lst);   
                }
                result.Add(SUCCESS, esGuardado);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult cboCuentaBanco()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cbo = chequeFS.getChequeService().cboCuentaBanco();
                result.Add(ITEMS, cbo);
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