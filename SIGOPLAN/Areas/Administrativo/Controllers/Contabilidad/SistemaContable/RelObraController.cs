using Core.DAO.Contabilidad.SistemaContable;
using Core.DTO.Contabilidad.SistemaContable.Cuentas;
using Core.DTO.Contabilidad.SistemaContable.Obra;
using Core.Entity.Administrativo.Contabilidad.Sistema_Contable.CentroCostos;
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

namespace SIGOPLAN.Areas.Administrativo.Controllers.Contabilidad.SistemaContable
{
    public class RelObraController : BaseController
    {
        #region init
        private Dictionary<string, object> resultado;
        private List<CentroCostoEmpresaDTO> catObras;
        private IObraDAO obraFS;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            resultado = new Dictionary<string, object>();
            catObras = new List<CentroCostoEmpresaDTO>();
            obraFS = new ObraFactoryServices().getObraServices();
            base.OnActionExecuting(filterContext);
        }
        #endregion
        #region RelacionObra
        [HttpGet]
        public ActionResult RelacionObra()
        {
            return View();
        }
        [HttpGet]
        public ActionResult _capturaRelObras()
        {
            return View();
        }
        public ActionResult RelObrasEmpresa(BusqAsignacionCuenta busq)
        {
            try
            {
                init();
                var relObra = obraFS.RelObras(busq);
                var gpoRelObra = from obra in relObra
                                 group obra by obra.PalObra into gpoPal
                                 select new
                                 {
                                     PalEmpresa = gpoPal.FirstOrDefault().PalEmpresa,
                                     PalObra = gpoPal.Key,
                                     PalDescripcion = catObras.FirstOrDefault(cat => cat.Empresa == busq.palEmpresa && cat.cc == gpoPal.Key).descripcion,
                                     lstSec = from cc in gpoPal
                                              select new
                                              {
                                                  cc.Id,
                                                  cc.SecEmpresa,
                                                  cc.SecObra,
                                                  SecDescripcion = catObras.FirstOrDefault(cat => cat.Empresa == busq.secEmpresa && cat.cc == cc.SecObra).descripcion,
                                              }
                                 };
                resultado.Add("lst", gpoRelObra);
                resultado.Add(SUCCESS, gpoRelObra.Any());
            }
            catch(Exception o_O)
            {
                resultado.Add("lst", new List<object>());
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, o_O.Message);
            }
            var json = Json(resultado, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }
        public ActionResult setRelObrasSession(List<tblC_CC_RelObras> relCtas)
        {
            try
            {
                var lstSession = (List<tblC_CC_RelObras>)Session["RelObras"];
                if(lstSession == null)
                {
                    lstSession = new List<tblC_CC_RelObras>();
                }
                lstSession.AddRange(relCtas);
                Session["RelObras"] = lstSession;
                var esSession = lstSession.Any();
                resultado.Add(SUCCESS, esSession);
                if(!esSession)
                {
                    Session["RelObras"] = null;
                }
            }
            catch(Exception o_O)
            {
                Session["RelObras"] = null;
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, o_O.Message);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarRelObras()
        {
            try
            {
                init();
                var lst = (List<tblC_CC_RelObras>)Session["RelObras"];
                var esSuccess = lst.Any() && lst.All(obra => validaCatObra(obra));
                if(esSuccess)
                {
                    esSuccess = obraFS.saveRelObras(lst);
                }
                Session["RelObras"] = null;
                resultado.Add(SUCCESS, esSuccess);
            }
            catch(Exception o_O)
            {
                Session["RelObras"] = null;
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, o_O.Message);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteObra(tblC_CC_RelObras obra)
        {
            try
            {
                var esEliminado = obraFS.DeleteObra(obra);
                resultado.Add(SUCCESS, esEliminado);
            }
            catch(Exception o_O)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, o_O.Message);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ObrasDescripcion(string term, EmpresaEnum empresa)
        {
            init();
            var items = (from obra in catObras
                         where obra.Empresa == empresa && string.Format("{0}-{1}", obra.cc, obra.descripcion).Contains(term)
                         select new
                         {
                             id = obra.cc,
                             value = string.Format("{0}-{1}", obra.cc, obra.descripcion)
                         }).Take(15);
            return Json(items, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Combobox
        [HttpGet]
        public ActionResult ItemsObraPorEmpresa()
        {
            try
            {
                init();
                var items = from cc in catObras
                            select new
                            {
                                Text = string.Format("{0}-{1}", cc.cc, cc.descripcion),
                                Value = cc.cc,
                                Empresa = cc.Empresa,
                            };
                resultado.Add(ITEMS, items);
                resultado.Add(SUCCESS, items.Any());
            }
            catch(Exception o_O)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, o_O.Message);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Auxiliares
        void init()
        {
            catObras = (List<CentroCostoEmpresaDTO>)Session["catObras"];
            if(catObras == null || !catObras.Any())
            {
                catObras = obraFS.CatObraEmpresa();
                Session["catObras"] = catObras;
            }
        }
        bool validaCatObra(tblC_CC_RelObras obra)
        {
            var mensaje = string.Empty;
            var obraPal = from cc in catObras
                          where cc.Empresa == obra.PalEmpresa && cc.cc == obra.PalObra
                          select cc;
            if(!obraPal.Any())
            {
                mensaje += string.Format("La obra {0} no pertenece a la empresa {1}.", obra.PalObra, obra.PalEmpresa.GetDescription());
            }
            var obraSec = from cc in catObras
                          where cc.Empresa == obra.SecEmpresa && cc.cc == obra.SecObra
                          select cc;
            if(!obraSec.Any())
            {
                mensaje += string.Format("La obra {0} no pertenece a la empresa {1}.", obra.PalObra, obra.SecEmpresa.GetDescription());
            }
            if(mensaje.Length > 0)
            {
                throw new System.InvalidOperationException(mensaje);
            }
            return mensaje.Length == 0;
        }
        #endregion
    }
}