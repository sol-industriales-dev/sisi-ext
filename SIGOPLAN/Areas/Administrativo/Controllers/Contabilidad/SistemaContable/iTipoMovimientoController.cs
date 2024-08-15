using Core.DAO.Contabilidad.SistemaContable;
using Core.DTO.Contabilidad.SistemaContable.Cuentas;
using Core.DTO.Contabilidad.SistemaContable.iTipoMovimiento;
using Core.Entity.Administrativo.Contabilidad.Sistema_Contable.iTiposMovimientos;
using Data.Factory.Contabilidad.SistemaContable;
using Infrastructure.Utils;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
//using System.Web.Http;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Administrativo.Controllers.Contabilidad.SistemaContable
{
    public class iTipoMovimientoController : BaseController
    {
        #region init
        private Dictionary<string, object> resultado;
        private IiTipoMovimientoDAO iTipoMovimientoFS;
        private List<string> iSistemas;
        private List<iTmEmpresaDTO> catiTm;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            resultado = new Dictionary<string, object>();
            iTipoMovimientoFS = new iTipoMovimientoFactoryServices().getiTmServices();
            iSistemas = new List<string>();
            catiTm = new List<iTmEmpresaDTO>();
            base.OnActionExecuting(filterContext);
        }
        #endregion
        #region Relacion iTm entre empresas
        public ActionResult RelacioniTm()
        {
            return View();
        }
        public ActionResult _RelacioniTm()
        {
            return PartialView();
        }
        public ActionResult ReliTmEmpresas(BusqAsignacionCuenta busq)
        {
            try
            {
                var relItm = iTipoMovimientoFS.ReliTmEmpresas(busq);
                resultado.Add("lst", relItm);
                resultado.Add(SUCCESS, relItm.Any());
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
        public ActionResult setRelitmSession(List<tblC_TM_Relitm> relCtas)
        {
            try
            {
                var lstSession = (List<tblC_TM_Relitm>)Session["Relitm"];
                if(lstSession == null)
                {
                    lstSession = new List<tblC_TM_Relitm>();
                }
                lstSession.AddRange(relCtas);
                Session["Relitm"] = lstSession;
                var esSession = lstSession.Any();
                resultado.Add(SUCCESS, esSession);
                if(!esSession)
                {
                    Session["Relitm"] = null;
                }
            }
            catch(Exception o_O)
            {
                Session["Relitm"] = null;
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, o_O.Message);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarRelitm()
        {
            try
            {
                init();
                var lst = (List<tblC_TM_Relitm>)Session["Relitm"];
                var esSuccess = lst.Any() && lst.All(itm => validaCatItm(itm));
                if(esSuccess)
                {
                    esSuccess = iTipoMovimientoFS.saveRelitm(lst);
                }
                Session["Relitm"] = null;
                resultado.Add(SUCCESS, true);
            }
            catch(Exception o_O)
            {
                Session["Relitm"] = null;
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, o_O.Message);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region Combobox
        public ActionResult ItemsiTmPorEmpresa()
        {
            try
            {
                init();
                var optISistema = from iSistema in iSistemas
                                  select new
                                  {
                                      Text = iSistema,
                                      Value = iSistema
                                  };
                resultado.Add("optISistema", optISistema);
                resultado.Add("optiTm", catiTm);
                resultado.Add(SUCCESS, catiTm.Any());
            }
            catch(Exception o_O)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, o_O.Message);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region auxiliares
        void init()
        {
            iSistemas.Add("B");
            catiTm = (List<iTmEmpresaDTO>)Session["catiTm"];
            if(catiTm == null || !catiTm.Any())
            {
                catiTm = iTipoMovimientoFS.ITipoMovimientoEmpresa(iSistemas);
                Session["catiTm"] = catiTm;
            }
        }
        private bool validaCatItm(tblC_TM_Relitm itm)
        {
            var mensaje = string.Empty;
            var itmPal = from _itm in catiTm
                         where _itm.Empresa == itm.PalEmpresa && _itm.Prefijo.Contains(itm.PalSistema) && _itm.Value.ParseInt() == itm.PaliTm
                         select _itm;
            if(!itmPal.Any())
            {
                mensaje += string.Format("El movimiento {0} no pertenece a la empresa {1}.", itm.PaliTm, itm.PalEmpresa.GetDescription());
            }
            var itmSec = from _itm in catiTm
                         where _itm.Empresa == itm.SecEmpresa && _itm.Prefijo.Contains(itm.SecSistema) && _itm.Value.ParseInt() == itm.SeciTm
                         select _itm;
            if(!itmPal.Any())
            {
                mensaje += string.Format("El movimiento {0} no pertenece a la empresa {1}.", itm.SeciTm, itm.PalEmpresa.GetDescription());
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