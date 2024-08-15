using Core.DAO.Contabilidad.SistemaContable;
using Core.DTO.Contabilidad.SistemaContable.Cuentas;
using Core.Entity.Administrativo.Contabilidad.Sistema_Contable.Cuentas;
using Core.Enum.Multiempresa;
using Data.Factory.Contabilidad.SistemaContable;
using Infrastructure.Utils;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Administrativo.Controllers.Contabilidad.Poliza
{
    public class CuentaController : BaseController
    {
        private Dictionary<string, object> resultado;
        private ICuentaDAO cuentaFS;
        private List<CatCtaEmpresa> catCta;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            resultado = new Dictionary<string, object>();
            cuentaFS = new CuentaFactoryServices().getCuentaService();
            base.OnActionExecuting(filterContext);
        }
        #region Asignación de cuentas
        public ActionResult Conciliacion()
        {
            return View();
        }
        public ActionResult _Conciliacion()
        {
            return View();
        }
        public ActionResult getRelCuentas(BusqAsignacionCuenta busq)
        {
            try
            {
                init();
                var lst = cuentaFS.getRelCuentas(busq);
                var gpo = from cta in lst
                          group cta by new { cta.palEmpresa, cta.palCta, cta.palScta, cta.palSscta }
                              into gpoCta
                              select new
                              {
                                  palEmpresa = gpoCta.Key.palEmpresa,
                                  palCta = gpoCta.Key.palCta,
                                  palScta = gpoCta.Key.palScta,
                                  palSscta = gpoCta.Key.palSscta,
                                  palDescripcion = getCuentaDescripcion(gpoCta.Key.palEmpresa, gpoCta.Key.palCta, gpoCta.Key.palScta, gpoCta.Key.palSscta),
                                  lstSec = from cta in gpoCta
                                           select new
                                           {
                                               id = cta.id,
                                               secEmpresa = cta.secEmpresa,
                                               secCta = cta.secCta,
                                               secScta = cta.secScta,
                                               secSscta = cta.secSscta,
                                               secDescripcion = getCuentaDescripcion(cta.secEmpresa, cta.secCta, cta.secScta, cta.secSscta),
                                           }
                              };
                resultado.Add(SUCCESS, gpo.Any());
                resultado.Add("lst", gpo);
            }
            catch(Exception o_O)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, o_O.Message);
            }
            var json = Json(resultado, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }
        public ActionResult setRelCuentasSession(List<tblC_Cta_RelCuentas> relCtas)
        {
            try
            {
                init();
                var lstSession = (List<tblC_Cta_RelCuentas>)Session["relCtas"];
                if(lstSession == null)
                {
                    lstSession = new List<tblC_Cta_RelCuentas>();
                }
                lstSession.AddRange(relCtas);
                Session["relCtas"] = lstSession;
                var esSession = lstSession.Any() && relCtas.All(cuenta => validaCatCta(cuenta));
                resultado.Add(SUCCESS, esSession);
                if(!esSession)
                {
                    Session["relCtas"] = null;
                }
            }
            catch(Exception o_O)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, o_O.Message);
            }
            resultado.Add("dataRequest", relCtas);
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarRelCuentas()
        {
            try
            {
                var lst = (List<tblC_Cta_RelCuentas>)Session["relCtas"];
                var esSuccess = cuentaFS.saveRelCuentas(lst);
                Session["relCtas"] = null;
                resultado.Add(SUCCESS, esSuccess);
            }
            catch(Exception o_O)
            {
                Session["relCtas"] = null;
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, o_O.Message);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteCuenta(tblC_Cta_RelCuentas cuenta)
        {
            try
            {
                var esEliminado = cuentaFS.DeleteCuenta(cuenta);
                resultado.Add(SUCCESS, true);
            }
            catch(Exception o_O)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, o_O.Message);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region combobox
        public ActionResult getCboCuentasEmpresas()
        {
            try
            {
                init();
                var items = from cta in catCta
                            group cta by new { cta.empresa, cta.cta } into gpo
                            select new
                            {
                                Text = string.Format("{0}-{1}-{2} {3}", gpo.Key.cta, 0, 0, gpo.FirstOrDefault().descripcion),
                                Value = string.Format("{0}-{1}-{2}", gpo.Key.cta, 0, 0),
                                Selectable = true,
                                isGroup = true,
                                options = from opt in gpo
                                          where opt.scta != 0 && opt.sscta != 0
                                          select new Core.DTO.Principal.Generales.ComboDTO
                                          {
                                              Text = string.Format("{0}-{1}-{2} {3}", opt.cta, opt.scta, opt.sscta, opt.descripcion),
                                              Value = string.Format("{0}-{1}-{2}", opt.cta, opt.scta, opt.sscta)
                                          },
                                empresa = gpo.Key.empresa
                            };
                resultado.Add(ITEMS, items);
                resultado.Add(SUCCESS, catCta.Any());

            }
            catch(Exception o_O)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, o_O.Message);
            }
            var json = Json(resultado, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }
        #endregion
        #region Auxiliares
        void init()
        {
            catCta = (List<CatCtaEmpresa>)Session["catCta"];
            if(catCta == null || catCta.Count() < 1)
            {
                catCta = cuentaFS.getCatCta();
                Session["catCta"] = catCta;
            }
        }
        public ActionResult getCuentaDesc(string term, EmpresaEnum empresa)
        {
            init();
            var items = (from cta in catCta
                         where cta.empresa == empresa && string.Format("{0}-{1}-{2} {3}", cta.cta, cta.scta, cta.sscta, cta.descripcion).Contains(term)
                         select new
                         {
                             id = string.Format("{0}-{1}-{2}", cta.cta, cta.scta, cta.sscta),
                             value = string.Format("{0}-{1}-{2} {3}", cta.cta, cta.scta, cta.sscta, cta.descripcion)
                         }).Take(15);
            return Json(items, JsonRequestBehavior.AllowGet);
        }
        bool validaCatCta(tblC_Cta_RelCuentas cuenta)
        {
            var mensaje = string.Empty;
            var ctaPal = from cta in catCta
                         where cta.empresa == cuenta.palEmpresa && cta.cta == cuenta.palCta && cta.scta == cuenta.palScta && cta.sscta == cuenta.palSscta
                         select cta;
            if(!ctaPal.Any())
            {
                var ctaDesc = string.Format("{0}-{1}-{2}", cuenta.palCta, cuenta.palScta, cuenta.palSscta);
                mensaje += string.Format("La cuenta {0} no pertenece a la empresa {1}. ", ctaDesc, cuenta.palEmpresa.GetDescription());
            }
            var ctaSec = from cta in catCta
                         where cta.empresa == cuenta.secEmpresa && cta.cta == cuenta.secCta && cta.scta == cuenta.secScta && cta.sscta == cuenta.secSscta
                         select cta;
            if(!ctaSec.Any())
            {
                var ctaDesc = string.Format("{0}-{1}-{2}", cuenta.secCta, cuenta.secScta, cuenta.secSscta);
                mensaje += string.Format("La cuenta {0} no pertenece a la empresa {1}. ", ctaDesc, cuenta.secEmpresa.GetDescription());
            }
            if(mensaje.Length > 0)
            {
                throw new System.InvalidOperationException(mensaje);
            }
            return mensaje.Length == 0;
        }
        string getCuentaDescripcion(EmpresaEnum empresa, int cta, int scta, int sscta)
        {
            var cuenta = catCta.FirstOrDefault(cat => cat.empresa == empresa && cat.cta == cta && cat.scta == scta && cat.sscta == sscta);
            if(cuenta == null)
            {
                return string.Empty;
            }
            else
            {
                return cuenta.descripcion;
            }
        }
        #endregion
    }
}