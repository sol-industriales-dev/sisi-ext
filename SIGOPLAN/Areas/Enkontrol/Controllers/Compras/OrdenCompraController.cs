using Core.DTO.Enkontrol.OrdenCompra;
using Data.Factory.Enkontrol.Compras;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Core.DTO.Principal.Generales;
using Core.DTO.Enkontrol.Alamcen;
using System.IO;
using Core.DTO;
using Core.DAO.Enkontrol.Compras;
using Core.DTO.Enkontrol.OrdenCompra.CuadroComparativo;
using Core.DTO.Enkontrol.Requisicion;
using Infrastructure.Utils;
using Core.Enum.Multiempresa;
using Core.Enum.Enkontrol.Compras;

namespace SIGOPLAN.Areas.Enkontrol.Controllers.Compras
{
    public class OrdenCompraController : BaseController
    {
        OrdenCompraFactoryService ocfs;

        ICuadroComparativoDAO cuadroComparativoFS;
        Dictionary<string, object> result;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ocfs = new OrdenCompraFactoryService();
            cuadroComparativoFS = new CuadroComparativoFactoryService().getCCService();
            base.OnActionExecuting(filterContext);
        }

        #region Compras con cuadro comparativos calificados
        [HttpGet]
        public ActionResult CalificacionProvDashboard()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ConsultaDashboard(DateTime fechaInicio, DateTime fechaFin, List<int> proveedores, List<int> compradores)
        {
            var r = new Dictionary<string, object>();

            try
            {
                var resultado = cuadroComparativoFS.ConsultaDashboard(fechaInicio, fechaFin, proveedores, compradores);

                r.Add(SUCCESS, true);
                r.Add("seriesOptimoVsNoOptimo", resultado["seriesOptimoVsNoOptimo"]);
                //r.Add("pastel_noOptimoDetalle", resultado["pastel_noOptimoDetalle"]); // CONSULTA APARTE
                //r.Add("pastel_optimaDetalle", resultado["pastel_optimaDetalle"]); // CONSULTA APARTE
                r.Add("seriesTop10ProvOptimos", resultado["seriesTop10ProvOptimos"]);
                r.Add("seriesTop10ProvNoOptimos", resultado["seriesTop10ProvNoOptimos"]);

                #region Grafica de barras por calificaciones
                r.Add("lstComprasOptimas", resultado["lstComprasOptimas"]);
                r.Add("lstComprasMedias", resultado["lstComprasMedias"]);
                r.Add("lstComprasNoOptimas", resultado["lstComprasNoOptimas"]);
                #endregion

                #region Grafica de barras por compradores
                r.Add("lstGpx_Compradores", resultado["lstGpx_Compradores"]);
                #endregion

                #region Detalle grafica pastel top 10 proveedores optimos
                //r.Add("lstDetalleTop10ProveedoresOptimos", resultado["lstDetalleTop10ProveedoresOptimos"]); //CONSULTA APARTE
                #endregion

                #region Detalle grafica pastel top 10 proveedores no opimos
                //r.Add("lstDetalleTop10ProveedoresNoOptimos", resultado["lstDetalleTop10ProveedoresNoOptimos"]); //CONSULTA APARTE
                #endregion

                #region Detalle grafica pastel calificaciones
                //r.Add("lstDetalleCalificaciones", resultado["lstDetalleCalificaciones"]); //CONSULTA APARTE
                #endregion

                #region Grafica de barras proveedores
                r.Add("lstGpx_Proveedores", resultado["lstGpx_Proveedores"]);
                #endregion
            }
            catch (Exception ex)
            {
                r.Add(SUCCESS, false);
                r.Add(MESSAGE, ex.Message);
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetDetallesProveedoresOptimosVsNoOptimos(DateTime fechaInicio, DateTime fechaFin, List<int> proveedores, List<int> compradores)
        {
            var r = new Dictionary<string, object>();
            try
            {
                var resultado = cuadroComparativoFS.GetDetallesProveedoresOptimosVsNoOptimos(fechaInicio, fechaFin, proveedores, compradores);
                r.Add(SUCCESS, true);
                r.Add(ITEMS, resultado["pastel_optimaDetalle"]); // CONSULTA APARTE
            }
            catch (Exception ex)
            {
                r.Add(SUCCESS, false);
                r.Add(MESSAGE, ex.Message);
            }
            var json = Json(r, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        [HttpPost]
        public JsonResult GetDetallesTop10ProvNoOptimos(DateTime fechaInicio, DateTime fechaFin, List<int> proveedores, List<int> compradores)
        {
            var r = new Dictionary<string, object>();
            try
            {
                var resultado = cuadroComparativoFS.GetDetallesTop10ProvNoOptimos(fechaInicio, fechaFin, proveedores, compradores);
                r.Add(SUCCESS, true);
                r.Add(ITEMS, resultado["lstDetalleTop10ProveedoresNoOptimos"]); //CONSULTA APARTE
            }
            catch (Exception ex)
            {
                r.Add(SUCCESS, false);
                r.Add(MESSAGE, ex.Message);
            }
            var json = Json(r, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        [HttpPost]
        public JsonResult GetDetallesTop10ProvOptimos(DateTime fechaInicio, DateTime fechaFin, List<int> proveedores, List<int> compradores)
        {
            var r = new Dictionary<string, object>();
            try
            {
                var resultado = cuadroComparativoFS.GetDetallesTop10ProvOptimos(fechaInicio, fechaFin, proveedores, compradores);
                r.Add(SUCCESS, true);
                r.Add(ITEMS, resultado["lstDetalleTop10ProveedoresOptimos"]); //CONSULTA APARTE
            }
            catch (Exception ex)
            {
                r.Add(SUCCESS, false);
                r.Add(MESSAGE, ex.Message);
            }
            var json = Json(r, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        public JsonResult GetDetallesCalificaciones(DateTime fechaInicio, DateTime fechaFin, List<int> proveedores, List<int> compradores)
        {
            var r = new Dictionary<string, object>();
            try
            {
                var resultado = cuadroComparativoFS.GetDetallesCalificaciones(fechaInicio, fechaFin, proveedores, compradores);
                r.Add(SUCCESS, true);
                r.Add(ITEMS, resultado["lstDetalleCalificaciones"]); //CONSULTA APARTE
            }
            catch (Exception ex)
            {
                r.Add(SUCCESS, false);
                r.Add(MESSAGE, ex.Message);
            }
            var json = Json(r, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        public ActionResult FillCboProveedores()
        {
            result = cuadroComparativoFS.FillCboProveedores();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboCompradores()
        {
            result = cuadroComparativoFS.FillCboCompradores();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        // GET: Enkontrol/OrdenCompra
        public ActionResult Generar(int? id)
        {
            Session["servicioCompra"] = id.HasValue && id.Value == 2 ? true : false;
            ViewBag.tipoOrdenCompra = id.HasValue && id.Value == 2 ? true : false;

            return View();
        }
        public ActionResult GenerarInterna()
        {
            return View();
        }
        public ActionResult Editar()
        {
            return View();
        }
        public ActionResult Autorizar()
        {
            return View();
        }
        public ActionResult Traspasar()
        {
            return View();
        }
        public ActionResult Surtido()
        {
            Session["vistaActual"] = 7260;
            return View();
        }
        public ActionResult CuadroComparativo(int? id)
        {
            ViewBag.empresa = vSesiones.sesionEmpresaActual;

            Session["servicioCompraCuadroComparativo"] = id.HasValue && id.Value == 2 ? true : false;

            return View();
        }
        public ActionResult PendienteSurtir()
        {
            return View();
        }
        public ActionResult PendienteSurtirCompra()
        {
            return View();
        }
        public ActionResult Requisiciones()
        {
            return View();
        }
        public ActionResult EntradaNoInventariable()
        {
            Session["vistaActual"] = 7315;
            return View();
        }
        public ActionResult SeguimientoAutorizacion()
        {
            return View();
        }
        public ActionResult ComprasProveedor()
        {
            return View();
        }
        public ActionResult Trazabilidad()
        {
            return View();
        }

        public ActionResult ReporteUltimaCompraInsumo()
        {
            return View();
        }

        public ActionResult AutorizaBajasCH()
        {
            return View();
        }

        public ActionResult OCSinFactura()
        {
            return View();
        }

        public ActionResult OCRelacionarFactura()
        {
            return View();
        }

        #region Generar
        public ActionResult generarOC(List<GenOrdenCompraDTO> lstOC)
        {
            var result = new Dictionary<string, object>();
            try
            {
                if (lstOC.All(o => o.isValida() && o.lstPartida.All(p => p.isValida())))
                {
                    var numOc = ocfs.getOcService().generaNuevaOC(lstOC);
                    var isMultiple = numOc.Count > 1;
                    if (isMultiple)
                    {
                        result.Add("numOcMin", numOc.Min(o => o));
                        result.Add("numOcMax", numOc.Max(o => o));
                    }
                    else
                    {
                        result.Add("numOc", numOc.FirstOrDefault());
                    }
                    result.Add("isMultiple", isMultiple);
                    result.Add(SUCCESS, numOc.Count > 0);
                }
                else
                {
                    result.Add("mensaje", "Revise la información para poder guardarla correctamente");
                    result.Add(SUCCESS, false);
                }
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getComprador()
        {
            var result = new Dictionary<string, object>();
            try
            {

                result.Add("comprador", ocfs.getOcService().getComprador());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult busqReq(BusqReq busq)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add("lstReq", ocfs.getOcService().busqReq(busq));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult busqReqNum(BusqReq busq)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lst = ocfs.getOcService().busqReqNum(busq);
                result.Add("min", lst[0]);
                result.Add("max", lst[1]);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getPartidas(string cc, int num, int moneda)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add("lstPartida", ocfs.getOcService().getPartidas(cc, num, moneda));
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

        #region cbo
        public ActionResult FillComboCcReqComprador(BusqReq busq)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, ocfs.getOcService().FillComboCcReqComprador(busq));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillComboCcComComprador()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, ocfs.getOcService().FillComboCcComComprador());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillComboCcComCompradorModalEditar()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, ocfs.getOcService().FillComboCcComCompradorModalEditar());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillComboCcAut(bool isAuth)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, ocfs.getOcService().FillComboCcAut(isAuth));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillComboCcAutTodas()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, ocfs.getOcService().FillComboCcAutTodas());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillComboCcVoBo(bool isAuth, string cc)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var data = ocfs.getOcService().getListaCompras(isAuth, "", true);

                var lstComboBoxCC = data.Where(x => x.voboPendiente).GroupBy(w => w.cc).Select(y => new Core.DTO.Principal.Generales.ComboDTO
                {
                    Value = y.Key,
                    Text = y.FirstOrDefault().ccDesc
                }).ToList();

                result.Add(ITEMS, lstComboBoxCC);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillComboTipoReq()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, ocfs.getOcService().FillComboTipoReq());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillComboCcAsigComp()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, ocfs.getOcService().FillComboCcAsigComp());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillComboFolioCuadro(int cuadrosExistentes)
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> lstCombo = new List<ComboDTO>();

                for (var i = 1; i <= cuadrosExistentes; i++)
                {
                    lstCombo.Add(new ComboDTO
                    {
                        Value = i.ToString(),
                        Text = "0" + i
                    });
                }

                result.Add(ITEMS, lstCombo);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillComboCc()
        {
            var result = new Dictionary<string, object>();

            try
            {
                result.Add(ITEMS, ocfs.getOcService().FillComboCc());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillComboFamiliasInsumos()
        {
            var result = new Dictionary<string, object>();

            try
            {
                result.Add(ITEMS, ocfs.getOcService().FillComboFamiliasInsumos());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillComboCompradores()
        {
            var result = new Dictionary<string, object>();

            try
            {
                result.Add(ITEMS, ocfs.getOcService().FillComboCompradores());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillComboCompradoresCC(string cc)
        {
            var result = new Dictionary<string, object>();

            try
            {
                result.Add(ITEMS, ocfs.getOcService().FillComboCompradores(cc));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillComboProveedores()
        {
            var result = new Dictionary<string, object>();

            try
            {
                result.Add(ITEMS, ocfs.getOcService().FillComboProveedores());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillComboCcFiltroPorUsuario()
        {
            var result = new Dictionary<string, object>();

            try
            {
                result.Add(ITEMS, ocfs.getOcService().FillComboCcFiltroPorUsuario());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillComboEstatusCompras()
        {
            var result = new Dictionary<string, object>();

            try
            {
                var items = new List<ComboDTO> {
                    new ComboDTO{ Value = "Todas", Text = "Todas" },
                    new ComboDTO{ Value = "1", Text = "Sin Surtir" },
                    new ComboDTO{ Value = "2", Text = "Parciales" },
                    new ComboDTO{ Value = "3", Text = "Surtidas" }
                };

                result.Add(ITEMS, items);
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

        #region Autocompletado
        public ActionResult getProvFromNum(string term)
        {
            return Json(ocfs.getOcService().getProvFromNum(term), JsonRequestBehavior.AllowGet);
        }
        public ActionResult getProvFromNom(string term)
        {
            return Json(ocfs.getOcService().getProvFromNom(term), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetLABFromNum(int num)
        {
            return Json(ocfs.getOcService().getLABFromNum(num), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Update Compra
        public ActionResult GetCompra(string cc, int num, bool esOC_INTERNA = false, string PERU_tipoCompra = "")
        {
            var result = new Dictionary<string, object>();
            //try
            //{
                var data = ocfs.getOcService().getCompra(cc, num, esOC_INTERNA, PERU_tipoCompra);

                result.Add("info", data);
                result.Add("partidas", data.lstPartidas);
                result.Add("pagos", data.lstPagos);
                result.Add("retenciones", data.lstRetenciones);
                result.Add("ultimoMovimiento", data.ultimoMovimiento);

                result.Add(SUCCESS, true);
            //}
            //catch (Exception e)
            //{
            //    result.Add(MESSAGE, e.Message);
            //    result.Add(SUCCESS, false);
            //}
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCompraRelacionar(string cc, int num)
        {
            var result = new Dictionary<string, object>();
            //try
            //{
            var data = ocfs.getOcService().getCompraRelacionar(cc, num);

            result.Add("info", data);

            result.Add(SUCCESS, true);
            //}
            //catch (Exception e)
            //{
            //    result.Add(MESSAGE, e.Message);
            //    result.Add(SUCCESS, false);
            //}
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCompra_Interna(string cc, int num)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var data = ocfs.getOcService().getCompra_Interna(cc, num);

                result.Add("info", data);
                result.Add("partidas", data.lstPartidas);
                result.Add("pagos", data.lstPagos);
                result.Add("retenciones", data.lstRetenciones);
                result.Add("ultimoMovimiento", data.ultimoMovimiento);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateCompra(OrdenCompraDTO compra)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var data = ocfs.getOcService().updateCompra(compra);

                result.Add("info", data);
                result.Add("partidas", data.lstPartidas);
                result.Add("pagos", data.lstPagos);
                result.Add("retenciones", data.lstRetenciones);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateCompraInterna(OrdenCompraDTO compra)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var data = ocfs.getOcService().updateCompraInterna(compra);

                result.Add("info", data);
                result.Add("partidas", data.lstPartidas);
                result.Add("pagos", data.lstPagos);
                result.Add("retenciones", data.lstRetenciones);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateRetencionesCompra()
        {
            var retencion = JsonConvert.DeserializeObject<List<OrdenCompraRetencionesDTO>>(Request.Form["retencion"]);

            result = ocfs.getOcService().updateRetencionesCompra(retencion);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListaCompras(bool isAuth, string cc, bool pendientes, bool propias)
        {
            var result = new Dictionary<string, object>();

            //try
            //{
            if (cc == "--Todos--") cc = "";

            var data = ocfs.getOcService().getListaCompras(isAuth, cc, propias);

            if (data.Count > 0)
            {
                if (pendientes)
                {
                    data = data.Where(x => x.voboPendiente).ToList();
                }

                List<ComboDTO> combo = new List<ComboDTO>();

                foreach (var d in data)
                {
                    if (!combo.Any(x => x.Value == d.cc))
                    {
                        combo.Add(new ComboDTO
                        {
                            Value = d.cc,
                            Text = d.cc + "-" + d.ccDesc
                        });
                    }
                }

                result.Add("data", data);
                result.Add(ITEMS, combo.OrderBy(x => x.Value).ToList());
                result.Add(SUCCESS, true);
            }
            else
            {
                result.Add(SUCCESS, true);
            }

            result.Add("voboNoOptimo", ocfs.getOcService().puedeVerCheckBoxProvNoOptimo());
            //}
            //catch (Exception e)
            //{
            //    result.Add(MESSAGE, e.Message);
            //    result.Add(SUCCESS, false);
            //    result.Add("voboNoOptimo", false);
            //}

            //return Json(result, JsonRequestBehavior.AllowGet);
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }
        public ActionResult GetListaComprasTodas(string cc, bool pendientes, bool propias, int area, int cuenta)
        {
            var result = new Dictionary<string, object>();

            //try
            //{
                result.Add("data", ocfs.getOcService().getListaComprasTodas(cc, pendientes, propias, area, cuenta));
                result.Add(SUCCESS, true);
            //}
            //catch (Exception e)
            //{
            //    result.Add(MESSAGE, e.Message);
            //    result.Add(SUCCESS, false);
            //}

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListaComprasDes(string cc)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = ocfs.getOcService().getListaComprasDes(cc);

                result.Add("data", data);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetVobos(OrdenCompraDTO compra)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = ocfs.getOcService().getVobos(compra);

                List<dynamic> vobosCapturados = new List<dynamic>();
                List<VoBosCapturadosDTO> vobosCapturadosColombia = new List<VoBosCapturadosDTO>();

                if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Colombia)
                {
                    #region COLOMBIA
                    if (data.Count() > 0)
                    {
                        if (data[0].flagCompraSISUN && (data[0].vobo_aut != "S" || data[0].vobo_aut == null))
                        {
                            vobosCapturadosColombia.AddRange(new List<VoBosCapturadosDTO>
                            {
                                new VoBosCapturadosDTO(){ usu_numero = 0,usu_nombre = string.Empty },
                                new VoBosCapturadosDTO(){ usu_numero = 0,usu_nombre = string.Empty },
                                new VoBosCapturadosDTO(){ usu_numero = 0,usu_nombre = string.Empty }
                            });
                        }
                        else
                        {
                            vobosCapturadosColombia.AddRange(new List<VoBosCapturadosDTO>
                            {
                                new VoBosCapturadosDTO()
                                {
                                    usu_numero = compra.vobo,
                                    usu_nombre = compra.vobo != 0 ? ocfs.getOcService().getUsuarioEnKontrolCOLOMBIA(compra.vobo) : string.Empty
                                },
                                new VoBosCapturadosDTO()
                                {
                                    usu_numero = compra.vobo2,
                                    usu_nombre = compra.vobo2 != 0 ? ocfs.getOcService().getUsuarioEnKontrolCOLOMBIA(compra.vobo2) : string.Empty
                                },
                                new VoBosCapturadosDTO()
                                {
                                    usu_numero = compra.vobo3,
                                    usu_nombre = compra.vobo3 != 0 ? ocfs.getOcService().getUsuarioEnKontrolCOLOMBIA(compra.vobo3) : string.Empty
                                }
                            });
                        }
                    }
                    else
                    {
                        vobosCapturadosColombia.AddRange(new List<VoBosCapturadosDTO>
                        {
                            new VoBosCapturadosDTO(){ usu_numero = 0,usu_nombre = string.Empty },
                            new VoBosCapturadosDTO(){ usu_numero = 0,usu_nombre = string.Empty },
                            new VoBosCapturadosDTO(){ usu_numero = 0,usu_nombre = string.Empty }
                        });
                    }
                    #endregion
                }
                else
                {
                    vobosCapturados = new List<dynamic>
                    {
                        new {
                            usu_numero = compra.vobo,
                            usu_nombre = compra.vobo != 0 ? ocfs.getOcService().getUsuarioEnKontrol(compra.vobo)[0].descripcion.Value : ""
                        },
                        new {
                            usu_numero = compra.vobo2,
                            usu_nombre = compra.vobo2 != 0 ? ocfs.getOcService().getUsuarioEnKontrol(compra.vobo2)[0].descripcion.Value : ""
                        },
                        new {
                            usu_numero = compra.vobo3,
                            usu_nombre = compra.vobo3 != 0 ? ocfs.getOcService().getUsuarioEnKontrol(compra.vobo3)[0].descripcion.Value : ""
                        }
                    };
                }

                result.Add("data", data);

                if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Colombia)
                    result.Add("vobosCapturados", vobosCapturadosColombia);
                else
                    result.Add("vobosCapturados", vobosCapturados);

                result.Add("flagPuedeDarVobo", ocfs.getOcService().getFlagPuedeDarVobo(data.Select(x => x.usu_numero).ToList(), compra));

                string mensajeError = string.Empty;
                if (data.Count() <= 0) { mensajeError = "No se encontraron facultamientos de VoBo."; }
                result.Add("mensajeError", mensajeError);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAutorizaciones(OrdenCompraDTO compra)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = ocfs.getOcService().getAutorizaciones(compra);

                result.Add("data", data);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEmpresaLogueada()
        {
            return Json(ocfs.getOcService().GetEmpresaLogueada(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult AutorizarCompras(List<OrdenCompraDTO> listaVobos, List<OrdenCompraDTO> listaAutorizados, List<CheckProvNoOptimoDTO> listaNoOptimosVoBo, bool esOC_Interna = false)
        {
            if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan)
            {
                #region PERU
                return Json(ocfs.getOcService().RegistrarVoBoAutorizarOrdenCompra(listaVobos, listaAutorizados), JsonRequestBehavior.AllowGet);
                #endregion
            }
            else if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Colombia)
            {
                #region COLOMBIA
                Dictionary<string, object> result = new Dictionary<string, object>();
                try
                {
                    foreach (var com in listaVobos ?? Enumerable.Empty<OrdenCompraDTO>())
                    {
                        // SE VERIFICA SI LA OC EXISTE EN SIGOPLANCOLOMBIA
                        //bool existeOC = ocfs.getOcService().VerificarOC(com);
                        //if (!existeOC)
                        //{
                        //    string mensajeError = string.Format("La orden de compra con CC: {0} y número de compra: {1}, no existe en SIGOPLAN", com.cc, com.numero);
                        //    throw new Exception(mensajeError);
                        //}

                        var compra = ocfs.getOcService().getCompra(com.cc, com.numero, com.esOC_Interna);
                        var vobos = ocfs.getOcService().getVobos(compra);
                        var flagPuedeDarVobo = ocfs.getOcService().getFlagPuedeDarVobo(vobos.Select(x => x.usu_numero).ToList(), compra);

                        if (com.esOC_Interna)
                            flagPuedeDarVobo = true;

                        if (flagPuedeDarVobo)
                        {
                            string voboNumero = string.Empty;

                            if (compra.flagSISUN)
                            {
                                voboNumero = "vobo";
                            }
                            else
                            {
                                if (compra.vobo == 0)
                                    voboNumero = "vobo";
                                else if (compra.vobo2 == 0)
                                    voboNumero = "vobo2";
                                else if (compra.vobo3 == 0)
                                    voboNumero = "vobo3";
                            }
                            
                            ocfs.getOcService().voboCompra(compra, voboNumero, vobos, com.esOC_Interna);
                        }
                    }

                    foreach (var com in listaAutorizados ?? Enumerable.Empty<OrdenCompraDTO>())
                    {
                        ocfs.getOcService().autorizarCompra(com.cc, com.numero, true, com.esOC_Interna);
                    }

                    result.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    result.Add(SUCCESS, false);
                    result.Add(MESSAGE, e.Message);
                }
                return Json(result, JsonRequestBehavior.AllowGet);
                #endregion
            }
            else
            {
                #region DEMAS EMPRESAS
                var result = new Dictionary<string, object>();

                try
                {
                    foreach (var com in listaVobos ?? Enumerable.Empty<OrdenCompraDTO>())
                    {
                        var compra = ocfs.getOcService().getCompra(com.cc, com.numero, com.esOC_Interna);
                        var vobos = ocfs.getOcService().getVobos(compra);
                        var flagPuedeDarVobo = ocfs.getOcService().getFlagPuedeDarVobo(vobos.Select(x => x.usu_numero).ToList(), compra);

                        if (com.esOC_Interna)
                            flagPuedeDarVobo = true;

                        if (flagPuedeDarVobo)
                        {
                            string voboNumero = "";

                            if (compra.vobo == 0)
                            {
                                voboNumero = "vobo";
                            }
                            else if (compra.vobo2 == 0)
                            {
                                voboNumero = "vobo2";
                            }
                            else if (compra.vobo3 == 0)
                            {
                                voboNumero = "vobo3";
                            }

                            ocfs.getOcService().voboCompra(compra, voboNumero, vobos, com.esOC_Interna);
                        }
                    }

                    foreach (var com in listaAutorizados ?? Enumerable.Empty<OrdenCompraDTO>())
                    {
                        ocfs.getOcService().autorizarCompra(com.cc, com.numero, true, com.esOC_Interna);
                    }

                    result.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    result.Add(MESSAGE, e.Message);
                    result.Add(SUCCESS, false);
                }
                return Json(result, JsonRequestBehavior.AllowGet);
                #endregion
            }
        }
        public ActionResult DesautorizarCompras(List<OrdenCompraDTO> compras)
        {
            var result = new Dictionary<string, object>();
            try
            {
                foreach (var com in compras)
                {
                    ocfs.getOcService().desautorizarCompra(com.cc, com.numero);
                }

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult VoboCompra(OrdenCompraDTO compra, string voboNumero)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var vobos = ocfs.getOcService().getVobos(compra);

                ocfs.getOcService().voboCompra(compra, voboNumero, vobos, false);

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

        public ActionResult GuardarSurtido(OrdenCompraDTO compra, List<SurtidoCompraDTO> surtido)
        {
            var result = new Dictionary<string, object>();

            try
            {
                if (vSesiones.sesionEmpresaActual != (int)EmpresaEnum.Peru)
                {
                    ocfs.getOcService().checarUbicacionesValidas(compra, surtido);
                }

                List<entradasAlmacenDTO> entradas = ocfs.getOcService().guardarSurtido(compra, surtido);

                Session["entradasCompra"] = entradas;

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarSurtidoNoInventariable(OrdenCompraDTO compra, List<SurtidoCompraDTO> surtido)
        {
            var result = new Dictionary<string, object>();

            try
            {
                List<entradasAlmacenDTO> entradas = ocfs.getOcService().guardarSurtidoNoInventariable(compra, surtido);

                Session["entradasNoInventariables"] = entradas;

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BuscarCuadros(BusquedaCuadroDTO filtros)
        {
            var result = new Dictionary<string, object>();

            //try
            //{
                var data = ocfs.getOcService().buscarCuadros(filtros);

                result.Add("data", data);
                result.Add(SUCCESS, true);
            //}
            //catch (Exception e)
            //{
            //    result.Add(MESSAGE, e.Message);
            //    result.Add(SUCCESS, false);
            //}

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult requisicionesNumeros(string cc)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = ocfs.getOcService().requisicionesNumeros(cc);

                result.Add("data", data);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUsuarioEnKontrol(int numeroEmpleado)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = ocfs.getOcService().getUsuarioEnKontrol(numeroEmpleado);

                result.Add("data", data);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCuadroDet(CuadroComparativoDTO cuadro)
        {
            var result = new Dictionary<string, object>();

            try
            {
                //Session["servicioCompra"] = id.HasValue && id.Value == 2 ? true : false;
                var tipoCompra = "";

                if (Session["servicioCompra"] != null)
                {
                    var valor = (bool)Session["servicioCompra"];
                    if (valor)
                    {
                        tipoCompra = "RS";
                    }
                    else
                    {
                        tipoCompra = "RQ";
                    }
                }

                if (Session["servicioCompraCuadroComparativo"] != null)
                {
                    var valor = (bool)Session["servicioCompraCuadroComparativo"];
                    if (valor)
                    {
                        tipoCompra = "RS";
                    }
                    else
                    {
                        tipoCompra = "RQ";
                    }
                }

                if (tipoCompra != "")
                {
                    cuadro.PERU_tipoRequisicion = tipoCompra;
                }

                var data = ocfs.getOcService().getCuadroDet(cuadro);

                result.Add("data", data);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetProveedorInfo(string num, string PERU_tipoCambio)
        {
            return Json(ocfs.getOcService().getProveedorInfo(num, PERU_tipoCambio), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetEmpleadoUsuarioEK(int numEmpleado)
        {
            return Json(ocfs.getOcService().getNombreUsuarioEmpleado(numEmpleado), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAlmacenNombre(int numAlmacen)
        {
            return Json(ocfs.getOcService().getNombreAlmacen(numAlmacen), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarNuevoCuadro(CuadroComparativoDTO nuevoCuadro)
        {
            return Json(ocfs.getOcService().GuardarNuevoCuadro(nuevoCuadro), JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateCuadro(CuadroComparativoDTO cuadro)
        {
            return Json(ocfs.getOcService().UpdateCuadro(cuadro), JsonRequestBehavior.AllowGet);
        }
        public ActionResult BorrarCuadro(CuadroComparativoDTO cuadro)
        {
            return Json(ocfs.getOcService().BorrarCuadro(cuadro), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetRequisicion(string cc, int num)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var data = ocfs.getOcService().getRequisicion(cc, num);

                result.Add("info", data);
                result.Add("partidas", data.lstPartidas);
                result.Add("pagos", data.lstPagos);
                result.Add("retenciones", data.lstRetenciones);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add("info", null);
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarNuevaCompra(OrdenCompraDTO compra)
        {
            var tipoCompra = "";

            if (Session["servicioCompra"] != null)
            {
                var valor = (bool)Session["servicioCompra"];
                if (valor)
                {
                    tipoCompra = "RS";
                }
                else
                {
                    tipoCompra = "RQ";
                }
            }

            if (Session["servicioCompraCuadroComparativo"] != null)
            {
                var valor = (bool)Session["servicioCompraCuadroComparativo"];
                if (valor)
                {
                    tipoCompra = "RS";
                }
                else
                {
                    tipoCompra = "RQ";
                }
            }

            compra.PERU_tipoCompra = tipoCompra;

            return Json(ocfs.getOcService().guardarNuevaCompra(compra), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarNuevaCompraInterna(OrdenCompraDTO compra)
        {
            return Json(ocfs.getOcService().guardarNuevaCompraInterna(compra), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetRetencionInfo(int id_cpto)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var data = ocfs.getOcService().getRetencionInfo(id_cpto);

                result.Add("data", data);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetProveedorNumero(string term)
        {
            var items = ocfs.getOcService().getProveedorNumero(term);

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCompradorNumero(string term)
        {
            var items = ocfs.getOcService().getCompradorNumero(term);

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ObtenerComprasPendientes(string cc, int estatus, int proveedor, DateTime fechaInicial, DateTime fechaFinal, string idAreaCuenta, int idCompradorEK)
        {
            return new JsonResult
            {
                Data = ocfs.getOcService().ObtenerComprasPendientes(cc, estatus, proveedor, fechaInicial, fechaFinal, idAreaCuenta, idCompradorEK),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };
        }

        public ActionResult ObtenerComprasSinFactura(string cc, int estatus, int proveedor, DateTime fechaInicial, DateTime fechaFinal, string idAreaCuenta, int idCompradorEK)
        {
            return new JsonResult
            {
                Data = ocfs.getOcService().ObtenerComprasSinFactura(cc, estatus, proveedor, fechaInicial, fechaFinal, idAreaCuenta, idCompradorEK),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };
        }


        public ActionResult GetUltimaCompra(CuadroComparativoDetDTO partidaCuadro)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var tipoCompra = "";

                if (Session["servicioCompra"] != null)
                {
                    var valor = (bool)Session["servicioCompra"];
                    if (valor)
                    {
                        tipoCompra = "RS";
                    }
                    else
                    {
                        tipoCompra = "RQ";
                    }
                }

                if (Session["servicioCompraCuadroComparativo"] != null)
                {
                    var valor = (bool)Session["servicioCompraCuadroComparativo"];
                    if (valor)
                    {
                        tipoCompra = "RS";
                    }
                    else
                    {
                        tipoCompra = "RQ";
                    }
                }

                partidaCuadro.PERU_tipoCuadro = tipoCompra;

                var data = ocfs.getOcService().getUltimaCompra(partidaCuadro);

                result.Add("data", data);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult puedeCancelar()
        {
            return Json(ocfs.getOcService().puedeCancelar(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CancelarCompra(string cc, int numero)
        {
            return Json(ocfs.getOcService().cancelarCompra(cc, numero), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CancelarComprasMasivo()
        {
            var result = new Dictionary<string, object>();

            try
            {
                ocfs.getOcService().CancelarComprasMasivo();

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CancelarParcialCompra(OrdenCompraDTO compra)
        {
            return Json(ocfs.getOcService().cancelarParcialCompra(compra), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRequisicionesValidadas(List<string> listCC, List<string> listFamiliasInsumos, List<string> listCompradores, DateTime fechaInicio, DateTime fechaFin, int area = 0, int cuenta = 0, string noEconomico = "")
        {
            return Json(ocfs.getOcService().getRequisicionesValidadas(listCC, listFamiliasInsumos, listCompradores, fechaInicio, fechaFin, area, cuenta, noEconomico), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPreciosPorProveedor(string cc, int numeroRequisicion, long numeroProveedor)
        {
            return Json(ocfs.getOcService().getPreciosPorProveedor(cc, numeroRequisicion, numeroProveedor), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckEstatusOrdenCompraImpresa(string cc, int numero, string PERU_tipoCompra = "")
        {
            return Json(ocfs.getOcService().checkEstatusOrdenCompraImpresa(cc, numero, PERU_tipoCompra), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckEstatusOrdenCompraImpresaConsulta(string cc, int numero)
        {
            return Json(ocfs.getOcService().checkEstatusOrdenCompraImpresaConsulta(cc, numero), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetContadorRequisicionesPendientes()
        {
            return Json(ocfs.getOcService().getContadorRequisicionesPendientes(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult BorrarCompra(string cc, int numero)
        {
            var result = new Dictionary<string, object>();

            try
            {
                ocfs.getOcService().borrarCompra(cc, numero, false);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BorrarComprasMasivo()
        {
            var result = new Dictionary<string, object>();

            try
            {
                ocfs.getOcService().BorrarComprasMasivo();
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BorrarCompraAutorizante(string cc, int numero)
        {
            var result = new Dictionary<string, object>();

            try
            {
                ocfs.getOcService().borrarCompra(cc, numero, true);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCatalogoRetenciones()
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = ocfs.getOcService().getCatalogoRetenciones();

                result.Add("data", data);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetMovimientoNoInv(int almacenID, int remision)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = ocfs.getOcService().getMovimientoNoInv(almacenID, remision);

                result.Add("movimiento", data);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPresupuestoCC(string cc)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var data = ocfs.getOcService().getPresupuestoCC(cc);

                result.Add("data", data);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPeriodoContable()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var data = ocfs.getOcService().getPeriodoContable();

                result.Add("data", data);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDatosReporteEntradaOC(string cc, int? num, long numMovimiento)
        {
            var result = new Dictionary<string, object>();

            try
            {
                List<entradasAlmacenDTO> entradas = ocfs.getOcService().GetDatosReporteEntradaOC(cc, num, numMovimiento);

                if (entradas == null)
                {
                    result.Add(SUCCESS, false);
                    result.Add(MESSAGE, "No hay registros con ese número de movimiento o no tiene detalles.");
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                Session["entradasCompra"] = entradas;

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, "No hay registros con ese número de movimiento.");
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDatosReporteEntradaNoInvOC(string cc, int? num, long numMovimiento)
        {
            var result = new Dictionary<string, object>();

            try
            {
                List<entradasAlmacenDTO> entradas = ocfs.getOcService().GetDatosReporteEntradaNoInvOC(cc, num, numMovimiento);

                if (entradas == null)
                {
                    result.Add(SUCCESS, false);
                    result.Add(MESSAGE, "No hay registros con ese número de movimiento o no tiene detalles.");
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                Session["entradasNoInventariables"] = entradas;

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, "No hay registros con ese número de movimiento.");
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEntradas(string cc, int numero)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = ocfs.getOcService().getEntradas(cc, numero);

                result.Add("data", data);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ActualizarColocadaFechaYProveedor()
        {
            var result = new Dictionary<string, object>();

            try
            {
                ocfs.getOcService().actualizarColocadaFechaYProveedor();

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ActualizarImpresa()
        {
            var result = new Dictionary<string, object>();

            try
            {
                ocfs.getOcService().actualizarImpresa();

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult enviarOCProv()
        {
            var result = new Dictionary<string, object>();
            try
            {
                HttpPostedFileBase file = Request.Files["fuEvidencia"];
                var cc = (string)Request.Form["cc"];
                var numero = int.Parse(Request.Form["numero"]);
                var correo = (string)Request.Form["correo"];

                ocfs.getOcService().enviarOCProv(cc, numero, correo, file);

                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AuditoriaEliminarReqOC()
        {
            return Json(ocfs.getOcService().auditoriaEliminarReqOC(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAuditoriaRequisicionesComprasAfectadas()
        {
            return Json(ocfs.getOcService().getAuditoriaRequisicionesComprasAfectadas(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetComprasProveedor(string cc, DateTime fechaInicio, DateTime fechaFin, int proveedor, int area, int cuenta)
        {
            return Json(ocfs.getOcService().getComprasProveedor(cc, fechaInicio, fechaFin, proveedor, area, cuenta), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetProveedoresCC(string cc, DateTime fechaInicio, DateTime fechaFin)
        {
            return Json(ocfs.getOcService().getProveedoresCC(cc, fechaInicio, fechaFin), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAreasCuentasCCFechaProveedor(string cc, DateTime fechaInicio, DateTime fechaFin, int proveedor)
        {
            return Json(ocfs.getOcService().getAreasCuentasCCFechaProveedor(cc, fechaInicio, fechaFin, proveedor), JsonRequestBehavior.AllowGet);
        }

        public bool checarEmpresaArrendadora()
        {
            return vSesiones.sesionEmpresaActual == 2;
        }

        public ActionResult FillComboAreaCuentaTodas()
        {
            var result = new Dictionary<string, object>();

            try
            {
                result.Add(ITEMS, ocfs.getOcService().FillComboAreaCuentaTodas());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region Esconder boton enviar correo si no son compradores
        public ActionResult usuarioCompradorExiste()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var usuarioPermiso = ocfs.getOcService().usuarioCompradorExiste();
                result.Add("usuarioPermiso", usuarioPermiso);
                if (usuarioPermiso == false)
                {
                    result.Add(SUCCESS, false);
                }
                else
                {
                    if (usuarioPermiso == true)
                    {
                        result.Add(SUCCESS, true);
                    }
                }
            }
            catch (Exception e)
            {
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion


        public ActionResult ActualizarComprasDesautorizadas()
        {
            var result = new Dictionary<string, object>();

            try
            {
                ocfs.getOcService().actualizarComprasDesautorizadas();

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getTrazabilidadGeneral(trazabilidad_filtrosDTO filtro)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = ocfs.getOcService().getTrazabilidadGeneral(filtro);

                result.Add("data", data);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getTrazabilidadGeneralv2(trazabilidad_filtrosDTO filtro)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = ocfs.getOcService().getTrazabilidadGeneralv2(filtro);
                var jsonData = JsonConvert.SerializeObject(data);
                result.Add("data", jsonData);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(SUCCESS, false);
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        public ActionResult GetProveedoresInsumos(List<string> listaInsumos)
        {
            return Json(ocfs.getOcService().GetProveedoresInsumos(listaInsumos), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEmpleadosPendientesLiberacion()
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = ocfs.getOcService().getEmpleadosPendientesLiberacion();

                result.Add("data", data);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;

            return json;
        }

        public ActionResult GuardarBajas(List<EmpleadoPendienteLiberacionDTO> empleados)
        {
            var result = new Dictionary<string, object>();

            try
            {
                ocfs.getOcService().guardarBajas(empleados);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region GENERAR LINK
        public ActionResult FillCboProveedoresGenerarLink()
        {
            return Json(ocfs.getOcService().FillCboProveedoresGenerarLink(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CEProveedorLink(ProveedorLinkDTO objDTO)
        {
            return Json(ocfs.getOcService().CEProveedorLink(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarProveedorLink(ProveedorLinkDTO objDTO)
        {
            return Json(ocfs.getOcService().EliminarProveedorLink(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetProveedoresLink(ProveedorLinkDTO objDTO)
        {
            return Json(ocfs.getOcService().GetProveedoresLink(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboProveedoresGenerarLinkRegistrados(ProveedorLinkDTO objDTO)
        {
            return Json(ocfs.getOcService().FillCboProveedoresGenerarLinkRegistrados(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EnviarCorreoLinkProveedores(ProveedorLinkDTO objDTO)
        {
            return Json(ocfs.getOcService().EnviarCorreoLinkProveedores(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult IndicarEnvioCorreoExternamente(ProveedorLinkDTO objDTO)
        {
            return Json(ocfs.getOcService().IndicarEnvioCorreoExternamente(objDTO), JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult DescargarArchivoCotizacion(string ruta)
        {
            return new FileStreamResult(GlobalUtils.GetFileAsStream(ruta), MimeMapping.GetMimeMapping(Path.GetFileName(ruta)))
            {
                FileDownloadName = Path.GetFileName(ruta)
            };
        }

        public ActionResult ImpresionMasivaCompras()
        {
            return Json(ocfs.getOcService().ImpresionMasivaCompras(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboFormaPagoPeru()
        {
            return Json(ocfs.getOcService().FillComboFormaPagoPeru(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboTipoDocumentoPeru()
        {
            return Json(ocfs.getOcService().FillComboTipoDocumentoPeru(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTipoCambioPeru()
        {
            return Json(ocfs.getOcService().GetTipoCambioPeru(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GuardarFacturas(List<RelFacturaDTO> facturas)
        {
            foreach (var factura in facturas)
            {
                if (factura.XmlFile != null && factura.XmlFile.ContentLength > 0)
                {
                    // Obtener el nombre del archivo sin la ruta "facturas/"
                    string fileName = Path.GetFileName(factura.XmlFile.FileName);

                    // Obtener la fecha actual para crear las carpetas de año y mes
                    // Si la fecha viene en el XML, puedes usar esa fecha en lugar de DateTime.Now
                    DateTime currentDate = DateTime.Now;
                    string yearFolder = currentDate.Year.ToString();
                    string monthFolder = currentDate.ToString("MM");

                    // Crear la ruta completa: C:\Proyecto\SISI\Compras\Facturas\Año\Mes
                    string basePath = @"C:\Proyecto\SISI\Compras\Facturas";
                    string fullPath = Path.Combine(basePath, yearFolder, monthFolder);

                    // Crear las carpetas si no existen
                    if (!Directory.Exists(fullPath))
                    {
                        Directory.CreateDirectory(fullPath);
                    }

                    // Crear la ruta final del archivo (incluyendo el nombre)
                    string filePath = Path.Combine(fullPath, fileName);
                    factura.Ruta = filePath;
                    // Guardar el archivo en la ruta especificada
                    factura.XmlFile.SaveAs(filePath);
                    ocfs.getOcService().UpdateOCFactura(facturas);
                    // Aquí puedes continuar con la lógica para procesar la factura
                }
            }

            return Json(new { success = true, message = "Facturas guardadas con éxito." });
        }

    }
}