using Core.DAO.Contabilidad.SistemaContable;
using Core.DAO.Principal.Usuarios;
using Core.DTO.Contabilidad.SistemaContable.TipoCambio;
using Core.DTO.Principal.Generales;
using Core.Entity.Administrativo.Contabilidad.Sistema_Contable.Moneda;
using Data.Factory.Contabilidad.SistemaContable;
using Data.Factory.Principal.Usuarios;
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
    public class TipoCambioController : BaseController
    {
        #region init
        private Dictionary<string, object> resultado;
        private ITipoCambioDAO tipoCambioFS;
        private IUsuarioDAO usuarioFS;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            resultado = new Dictionary<string, object>();
            tipoCambioFS = new TipoCambioFactoryServices().getTCService();
            usuarioFS = new UsuarioFactoryServices().getUsuarioService();
            base.OnActionExecuting(filterContext);
        }
        #endregion
        #region Captura de tipo de cambio
        public ActionResult Gestion()
        {
            return View();
        }
        public ActionResult _capturaTipoCambio()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ValidaTipoCambioDelDia()
        {
            try
            {
                var tipoCambio = tipoCambioFS.TipoCambioDelDia();
                var puedeCapturarMoneda = usuarioFS.getLstAccionesActual().Any(permiso => permiso.id == tipoCambio.Divisa.idAccion);
                var HistoricoTipoCambio = tipoCambioFS.HistoricoTipoCambio();
                resultado.Add(SUCCESS, puedeCapturarMoneda && !tipoCambio.esActivo);
                resultado.Add("tipoCambio", tipoCambio);
                resultado.Add("HistoricoTipoCambio", HistoricoTipoCambio);
            }
            catch (Exception o_O)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, o_O.Message);
                resultado.Add("HistoricoTipoCambio", new List<TipoCambioDTO>());
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ExisteTipoCambio(DateTime fechaActual)
        {
            try
            {
                var HistoricoTipoCambio = tipoCambioFS.HistoricoTipoCambio().Where(r => r.Fecha.Date == fechaActual.Date);
                resultado.Add(SUCCESS, HistoricoTipoCambio.Any());
                resultado.Add("HistoricoTipoCambio", HistoricoTipoCambio);
            }
            catch (Exception o_O)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, o_O.Message);
                resultado.Add("HistoricoTipoCambio", new List<TipoCambioDTO>());
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GuardarTipoCambio(tblC_SC_TipoCambio tipoCambio)
        {
            try
            {
                var esSuccess = esTipoCambioValido(tipoCambio);
                if (esSuccess)
                {
                    esSuccess = tipoCambioFS.GuardarTipoCambio(tipoCambio);
                }
                resultado.Add(SUCCESS, esSuccess);
            }
            catch (Exception o_O)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, o_O.Message);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region combobox
        [HttpGet]
        public ActionResult GetComboMonedas()
        {
            try
            {
                var catalogo = tipoCambioFS.CatMoneda();
                var items = from moneda in catalogo
                            select new ComboDTO
                            {
                                Text = moneda.Moneda,
                                Value = moneda.Moneda,
                                Prefijo = Infrastructure.Utils.JsonUtils.convertNetObjectToJson(moneda)
                            };
                resultado.Add(ITEMS, items);
                resultado.Add(SUCCESS, items.Any());
            }
            catch (Exception o_O)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, o_O.Message);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Auxiliares
        bool esTipoCambioValido(tblC_SC_TipoCambio tipoCambio)
        {
            var mensaje = string.Empty;
            if (tipoCambio.TipoCambio <= 0)
            {
                mensaje += "El tipo de cambio no es correcto. ";
            }
            if (tipoCambio.Fecha.Date > DateTime.Now.Date)
            {
                mensaje += "Seleccione una fecha valida. ";
            }
            if (tipoCambio.Divisa.Clave <= 0)
            {
                mensaje += "Seleccione una moneda correcta. ";
            }
            var tieneError = mensaje.Any();
            if (tieneError)
            {
                throw new Exception(mensaje);
            }
            return !tieneError;
        }
        #endregion

    }
}