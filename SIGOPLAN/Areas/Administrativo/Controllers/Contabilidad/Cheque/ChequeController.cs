using Core.DAO.Contabilidad.Cheque;
using Core.DAO.Contabilidad.SistemaContable;
using Core.DTO;
using Core.DTO.Administracion.Cheque;
using Core.DTO.Contabilidad.Cheque;
using Core.Entity.Administrativo.Contabilidad.Cheque;
using Core.Entity.Administrativo.Contabilidad.Cheques;
using Core.Enum.Principal;
using Data.DAO.Principal.Usuarios;
using Data.Factory.Administracion.Cheque;
using Data.Factory.Contabilidad.SistemaContable;
using SIGOPLAN.Controllers;
//using SIGOPLAN.Reportes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Administrativo.Controllers.Contabilidad.Cheque
{
    public class ChequeController : BaseController
    {

        ICapChequeDAO capChequeService;
        private IMesProcDAO mesFS;
        //private Vista _vista;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            capChequeService = new CapChequeFactoryServices().getChequeServices();
            mesFS = new MesProcFactoryServices().getMesPrcService();
            //_vista = new Vista();
            base.OnActionExecuting(filterContext);
        }

        public ViewResult CapturaCheques()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetCheques(filtroCheques filtros)
        {
            var ud = new UsuarioDAO();
            filtros.permiso = ud.getViewAction(vSesiones.sesionCurrentView, "ELIMINAR");
            return Json(capChequeService.GetCheques(filtros), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetListaProveedores(string term, bool porDesc)
        {
            return Json(capChequeService.GetListaProveedores(term, porDesc), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetListaCuentas(string term, bool porDesc)
        {
            return Json(capChequeService.GetListaCuentas(term, porDesc), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SetUltimoCheque(int cuenta)
        {
            return Json(capChequeService.SetUltimoCheque(cuenta), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetInfoCuenta(int cuenta)
        {
            return Json(capChequeService.GetInfoCheque(cuenta), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetTipoMovimiento()
        {
            return Json(capChequeService.GetTipoMovimientos(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetProveedores()
        {
            return Json(capChequeService.GetProveedores(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetCuentasBanco()
        {
            return Json(capChequeService.GetCuentasBanco(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveOrUpdateCheque(tblC_sb_cheques objSave, int ocID, List<tblC_sc_movpol> objMovPol, int tipoCheque)
        {
            return Json(capChequeService.GuardarCheque(objSave, ocID, objMovPol, tipoCheque), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ObtenerDatosPoliza(tblC_sb_cheques objSave, int ocID)
        {
            return Json(capChequeService.ObtenerDatosPoliza(objSave, ocID), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetPolizasCheque(int cheque, string fecha)
        {
            return Json(capChequeService.GetPolizasCheque(cheque, fecha), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetOrdenCompraAnticipo()
        {
            return Json(capChequeService.GetOrdenCompraAnticipo(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CboEconomico()
        {
            return Json(capChequeService.CboEconomico(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUsuarioCaptura()
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();
            var usuario = base.getUsuario();

            resultado.Add("usuarioID", usuario.id);
            resultado.Add("usuarioNombre", usuario.nombreUsuario);
            resultado.Add(SUCCESS, true);
            return Json(resultado, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetPeriodoContable()
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();
            try
            {
                var periodo = mesFS.getProcesosAbiertosPruebas(SistemasEnkontrolEnum.Bancos).LastOrDefault();
                var mes = periodo.fecha.Month;
                var year = periodo.fecha.Year;
                resultado.Add("mes", mes);
                resultado.Add("year", year);
                resultado.Add(SUCCESS, true);

            }
            catch (Exception)
            {

                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "No se cargo el periodo contable, ponerse en contacto con sistemas.");
            }

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveOrUpdatePoliza(List<tblC_sc_movpol> data)
        {
            return Json(capChequeService.SaveOrUpdatePoliza(data), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetListaCuentasInit()
        {
            return Json(capChequeService.GetListaCuentasInit(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ValidaPoliza(List<tblC_sc_movpol> listaMovpol)
        {
            return Json(capChequeService.ValidaPoliza(listaMovpol), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult printCheque()
        {
            //vista.printCheque();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BuscarCuenta(int cuenta, int subCuenta, int ssubCuenta)
        {
            return Json(capChequeService.BuscarCuenta(cuenta, subCuenta, ssubCuenta), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteCheque(int chequeID)
        {
            return Json(capChequeService.DeleteCheque(chequeID), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult BuscarOC(string cc, int numero)
        {
            return Json(capChequeService.BuscarOC(cc, numero), JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public ActionResult GetOCSeleccionado(string cc, int numero)
        {
            return Json(capChequeService.GetOCSeleccionado(cc, numero), JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult OpenEditCheques(int idCheque)
        {
            return Json(capChequeService.OpenEditCheques(idCheque), JsonRequestBehavior.AllowGet);
        }
    }
}