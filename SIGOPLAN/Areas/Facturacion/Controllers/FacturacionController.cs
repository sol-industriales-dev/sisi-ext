using Core.DTO.Facturacion;
using Data.Factory.Facturacion;
using Data.Factory.Facturacion.Prefacturacion;
using Data.Factory.Maquinaria.Catalogos;
using Infrastructure.Utils;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Facturacion.Controllers
{
    public class FacturacionController : BaseController
    {
        CentroCostosFactoryServices centroCostosFactoryServices;
        FacturaFactoryService FacturaFactoryService;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            centroCostosFactoryServices = new CentroCostosFactoryServices();
            FacturaFactoryService = new FacturaFactoryService();
            base.OnActionExecuting(filterContext);
        }
        // GET: Facturacion/Facturacion
        #region Facturacion
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult BuscaInsumo(string insumo, string descripcion)
        {
            var restabla = FacturaFactoryService.getFacturaService().lstInsumo(insumo, descripcion)
                .Select(x => new
                {
                    id = x.insumo,
                    Insumo = x.insumo,
                    Descripcion = x.descripcion,
                    Unidad = x.unidad,
                    Precio = x.tipo == 9 || x.PRECIO_INSUMO == 0 ? "0" : String.Format("{0:C2}", x.PRECIO_INSUMO),
                    Tipo = x.tipo,
                    Grupo = x.grupo,
                    Consecutivo = x.consecutivo_bit
                });
            return Json(restabla, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SetObjInsumo(int consecutivo)
        {
            var obj = FacturaFactoryService.getFacturaService().objInsumo(consecutivo);
            var response = new
            {
                id = obj.consecutivo_bit,
                Partida = "",
                Insumo = obj.insumo,
                Descripcion = obj.descripcion,
                Unidad = obj.unidad,
                Cantidad = 1,
                Precio = obj.PRECIO_INSUMO,
                Descuento = 0,
                DescuentoDinero = 0,
                Importe =  obj.PRECIO_INSUMO,
                FE_PRECIO = obj.FE_PRECIO.ToString("yyyy-MM-dd")
            };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SetObjRentencion(int insumo)
        {
            var obj = FacturaFactoryService.getFacturaService().objRentencion(insumo);
            var response = new
            {
                Insumo = obj.insumo,
                Descripcion = obj.descripcion,
                Unidad = obj.unidad,
                Descuento = 0,
                DescuentoDinero = 0,
                Importe = 0,
                Cantidad = 1,
                Precio = 0
            };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetObjCliente(int numcte)
        {
            var objCliente = FacturaFactoryService.getFacturaService().objCliente(numcte);
            return Json(objCliente, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SetObjPedidio(int pedido)
        {
            var result = new Dictionary<string, object>();
            try
            {
                int numcte = 0, cia_surcusal;
                var objPedido = FacturaFactoryService.getFacturaService().objPedido(pedido, out numcte);
                result.Add("objPedido", objPedido);

                var objCliente = FacturaFactoryService.getFacturaService().objCliente(numcte);
                result.Add("objCliente", objCliente);

                var objRemision = FacturaFactoryService.getFacturaService().objRemision(pedido);
                result.Add("objRemision", objRemision);

                var objFactura = FacturaFactoryService.getFacturaService().objFactura(pedido, out cia_surcusal);
                result.Add("objFactura", objFactura);

                var objCdfParametros = FacturaFactoryService.getFacturaService().objCdfParametros(cia_surcusal);
                result.Add("objCdfParametros", objCdfParametros);

                var lstPartida = FacturaFactoryService.getFacturaService().GetlstInsumoFactura(pedido);
                result.Add("lstPartida", lstPartida);

                var lstRentencion = FacturaFactoryService.getFacturaService().GetlstInsumoRentencion(pedido);
                result.Add("lstRentencion", lstRentencion);

                var objUsuario = getUsuario();
                result.Add("idUsuario", objUsuario.id);
                result.Add("NomUsuario", objUsuario.nombre);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveBigFactura(BigFacturaDTO obj, List<PartidaDTO> lst, List<PartidaDTO> lstRentencion)
        {
            var result = FacturaFactoryService.getFacturaService().UpdateFacura(obj, lst, lstRentencion);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getNew()
        {
            var result = FacturaFactoryService.getFacturaService().getNew();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region FillCombo
        public ActionResult FillComboCC()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var list = FacturaFactoryService.getFacturaService().getListaCC();
                result.Add(ITEMS, list.OrderBy(x => x.Text));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboEmpleado()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var list = FacturaFactoryService.getFacturaService().getListaEmpleado();
                result.Add(ITEMS, list.Select(x => new { Text = x.Text.PadLeft(3, '0'), Value = x.Value }).OrderBy(x => x.Text));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillComboCiaSuc()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var list = FacturaFactoryService.getFacturaService().FillComboCiaSuc();
                result.Add(ITEMS, list.OrderBy(x => x.Value));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillComboSurcusal(int numcte)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var list = FacturaFactoryService.getFacturaService().FillComboSurcusal(numcte)
                    .Select(x => new { Text = x.Text.PadLeft(3, '0'), Value = x.Value });
                result.Add(ITEMS, list.OrderBy(x => x.Text));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillComboCliente()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var list = FacturaFactoryService.getFacturaService().FillComboCliente()
                    .Select(x => new { Text = x.Text.PadLeft(3, '0'), Value = x.Value });
                result.Add(ITEMS, list.OrderBy(x => x.Text));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboClienteNombre()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var list = FacturaFactoryService.getFacturaService().FillComboClienteNombre()
                    .Select(x => new { Text = x.Text.PadLeft(3, '0'), Value = x.Value });
                result.Add(ITEMS, list.OrderBy(x => x.Text));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillComboClienteNombresFiltro(int moneda)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var list = FacturaFactoryService.getFacturaService().FillComboClienteNombreMoneda(moneda)
                    .Select(x => new { Text =x.Text.PadLeft(3, '0') + '-' + x.Value, Value = x.Text.PadLeft(3, '0') });
                result.Add(ITEMS, list.OrderBy(x => x.Text));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillCboCliente()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var list = FacturaFactoryService.getFacturaService().FillComboClienteNombre();
                result.Add(ITEMS, list);
                result.Add(SUCCESS, list.Count > 0);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillComboRegFiscal()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var list = FacturaFactoryService.getFacturaService().FillComboRegFiscal();
                result.Add(ITEMS, list.OrderBy(x => x.Value));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillComboMetodoPago()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var list = FacturaFactoryService.getFacturaService().FillComboMetodoPago();
                result.Add(ITEMS, list.OrderBy(x => x.Value));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillComboClaveSat()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var list = FacturaFactoryService.getFacturaService().FillComboClaveSat();
                result.Add(ITEMS, list.OrderBy(x => x.Value));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboTm()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var list = FacturaFactoryService.getFacturaService().FillComboTm();
                result.Add(ITEMS, list.OrderBy(x => x.Value));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboZonas()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var list = FacturaFactoryService.getFacturaService().FillComboZonas();
                result.Add(ITEMS, list.OrderBy(x => x.Value));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion 
        
        #endregion
        #region Gestión
        public ActionResult Gestion()
        {
            return View();
        }

        public ActionResult GetTblGestion(DateTime inicio, DateTime fin, string cliente)
        {
            var restabla = FacturaFactoryService.getFacturaService().GetTblGestion(inicio, fin, cliente)
                .Select(x => new
                {
                    pedido = x.pedido,
                    remision = FacturaFactoryService.getFacturaService().GetRemisionFromPedido(x.pedido),
                    factura = FacturaFactoryService.getFacturaService().GetFacturaFromPedido(x.pedido),
                    numcte = x.numcte,
                    nombre = FacturaFactoryService.getFacturaService().GetClienteNombre(x.numcte),
                    fecha = x.fecha.ToString("yyyy-MM-dd"),
                    Accion = "",
                    Estatus = ""
                });
            return Json(restabla, JsonRequestBehavior.AllowGet);
        }
        #endregion


    }
}