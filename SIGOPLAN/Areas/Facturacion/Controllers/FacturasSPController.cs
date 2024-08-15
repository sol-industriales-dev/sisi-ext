using Core.DAO.Facturacion.Enkontrol;
using Core.Entity.Facturacion.Prefacturacion;
using Core.Service.Facturacion.Enkontrol;
using Data.Factory.Facturacion.Enkontrol;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Facturacion.Controllers
{
    public class FacturasSPController : BaseController
    {
        IFacturasSPDAO facturasSPInterfaz;
        IFacturasSPDAO facturasEKInterfaz;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //centroCostosFactoryServices = new CentroCostosFactoryServices();
            //FacturaFactoryService = new FacturaFactoryService();
            facturasSPInterfaz = new FacturasSPFactoryService().getFacturasSPFactoryService();
            facturasEKInterfaz = new FacturasSPFactoryService().getFacturasEKFactoryService();
            base.OnActionExecuting(filterContext);
        }

        #region Views

        public ActionResult FacturaEK()
        {
            return View();
        }

        #endregion


        #region PEDIDOS
        public ActionResult GuardarPedidos(tblF_RepPrefactura obj, List<tblF_CapPrefactura> lst, List<tblF_CapImporte> lstImpuesto, int idSPPedido)
        {
            return Json(facturasSPInterfaz.GuardarPedidos(obj, lst, lstImpuesto, idSPPedido), JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region REMISION
        public ActionResult GuardarRemision(tblF_RepPrefactura obj, List<tblF_CapPrefactura> lst, List<tblF_CapImporte> lstImpuesto, int idSPPedido, int idSPRemision)
        {
            return Json(facturasSPInterfaz.GuardarRemision(obj, lst, lstImpuesto, idSPPedido, idSPRemision), JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region FACTURAS
        public ActionResult GuardarFactura(tblF_RepPrefactura obj, List<tblF_CapPrefactura> lst, List<tblF_CapImporte> lstImpuesto, int idSPPedido, int idSPRemision, int idSPFactura)
        {
            return Json(facturasSPInterfaz.GuardarFactura(obj, lst, lstImpuesto, idSPPedido, idSPRemision, idSPFactura), JsonRequestBehavior.AllowGet);

        }
        #endregion
    }
}