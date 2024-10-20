using Core.DAO.Enkontrol.Almacen;
using Core.DTO;
using Core.DTO.Almacen;
using Core.DTO.Enkontrol.Alamcen;
using Core.DTO.Enkontrol.Tablas;
using Data.Factory.Enkontrol.Almacen;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Core.Entity.Enkontrol.Compras.OrdenCompra;
using Core.DTO.Enkontrol.Tablas.Almacen;
using Newtonsoft.Json;
using Core.Enum.Enkontrol.Compras;
using Infrastructure.Utils;
using Core.Entity.Enkontrol.Compras;
using Core.DTO.Utils.DataTable;
using Data.Factory.Maquinaria.Catalogos;

namespace SIGOPLAN.Areas.Enkontrol.Controllers.Almacen
{
    public class AlmacenController : BaseController
    {
        IAlmacenDAO almService;
        Dictionary<string, object> result;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            almService = new AlmacenFactoryService().getAlmService();
            base.OnActionExecuting(filterContext);
        }

        #region Vistas
        public ActionResult SolicitudTraspaso()
        {
            return View();
        }

        public ActionResult GestionTraspaso()
        {
            return View();
        }

        public ActionResult ActualizacionDeUbicaciones()
        {
            return View();
        }
        public ActionResult Insumos()
        {
            ViewBag.empresa = vSesiones.sesionEmpresaActual;

            return View();
        }
        public ActionResult DevolucionEntrada()
        {
            return View();
        }
        public ActionResult DevolucionSalida()
        {
            return View();
        }
        public ActionResult InventarioFisicoEntrada()
        {
            return View();
        }
        public ActionResult InventarioFisicoSalida()
        {
            return View();
        }
        public ActionResult SalidaConsumoSinOrigen()
        {
            return View();
        }
        public ActionResult AutorizaBajasRH()
        {
            return View();
        }
        public ActionResult EntradaTraspasoSinOrigen()
        {
            return View();
        }
        public ActionResult ConsultaTraspaso()
        {
            return View();
        }
        public ActionResult RegistroInventarioFisico()
        {
            return View();
        }
        public ActionResult ComparativoExistenciasInventarioFisico()
        {
            return View();
        }
        public ActionResult ValuacionInventarioFisico()
        {
            return View();
        }
        public ActionResult CatalogoAreaAlmacen()
        {
            return View();
        }

        public ActionResult TraspasoDirecto()
        {
            return View();
        }

        public ActionResult ABCAlmacen()
        {
            return View();
        }

        public ActionResult Remanentes()
        {
            return View();
        }

        public ActionResult Ubicaciones()
        {
            return View();
        }

        public ActionResult CatInsumo()
        {
            return View();
        }

        public ActionResult CatFamilia()
        {
            return View();
        }

        public ActionResult CatTipo()
        {
            return View();
        }

        public ActionResult CatGrupo()
        {
            return View();
        }
        #endregion

        public ActionResult FillComboCC()
        {
            var result = new Dictionary<string, object>();

            try
            {
                result.Add(ITEMS, almService.FillComboCC());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillComboCCTodos()
        {
            var result = new Dictionary<string, object>();

            try
            {
                result.Add(ITEMS, almService.FillComboCCTodos());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetInsumos(int almacenID, string cc)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = almService.getInsumos(almacenID, cc);

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

        public ActionResult GuardarTraspasos(string ccOrigen, int almacenOrigen, string ccDestino, int almacenDestino, string comentarios, List<ValuacionDTO> insumos)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = almService.guardarTraspasos(ccOrigen, almacenOrigen, ccDestino, almacenDestino, comentarios, insumos);

                result.Add(SUCCESS, data);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTraspasosPendientes(string ccOrigen, int almacenOrigen, string ccDestino, int almacenDestino, int folioInterno)
        {
            return Json(almService.getTraspasosPendientes(ccOrigen, almacenOrigen, ccDestino, almacenDestino, folioInterno), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTraspasosPendientesOrigen(string ccOrigen, int almacenOrigen, string ccDestino, int almacenDestino, int numeroRequisicion)
        {
            return Json(almService.getTraspasosPendientesOrigen(ccOrigen, almacenOrigen, ccDestino, almacenDestino, numeroRequisicion), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTraspasosRechazados(string ccOrigen, int almacenOrigen, string ccDestino, int almacenDestino, int folioInterno)
        {
            return Json(almService.getTraspasosRechazados(ccOrigen, almacenOrigen, ccDestino, almacenDestino, folioInterno), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarAutorizacionesTraspasosDirectos(List<TraspasoDTO> listaAutorizados, int numReq)
        {
            var r = almService.guardarAutorizacionesTraspasos(listaAutorizados, false, true);
            if ((bool)r[SUCCESS])
            {
                var data = almService.getSalidaConsultaTraspaso(listaAutorizados.First().almacenOrigen, numReq);

                Session["salidaConsultaTraspaso"] = data.Item2;
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarAutorizacionesTraspasos(List<TraspasoDTO> listaAutorizados)
        {
            return Json(almService.guardarAutorizacionesTraspasos(listaAutorizados, false, false), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarAutorizacionesTraspasosOrigen(List<TraspasoDTO> listaAutorizados)
        {
            return Json(almService.guardarAutorizacionesTraspasosOrigen(listaAutorizados), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetInformacionInsumo(FiltrosExistenciaInsumoDTO filtros)
        {
            return Json(almService.getInformacionInsumo(filtros), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetInsumosCatalogo(DataTablesParam param, InsumoCatalogoDTO filtros)
        {
            var json = Json(almService.getInsumosCatalogo(param, filtros)["datos"], JsonRequestBehavior.AllowGet);

            json.MaxJsonLength = int.MaxValue;

            return json;
        }

        public ActionResult GetInformacionInsumoCatalogo(int insumo)
        {
            return Json(almService.getInformacionInsumoCatalogo(insumo), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarNuevoInsumo(InsumoCatalogoDTO insumo)
        {
            return Json(almService.guardarNuevoInsumo(insumo), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTipoGrupo(int tipo, int grupo)
        {
            return Json(almService.getTipoGrupo(tipo, grupo), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTipoInsumoPeru(int tipo)
        {
            return Json(almService.GetTipoInsumoPeru(tipo), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboTipoInsumoPeru()
        {
            return Json(almService.FillComboTipoInsumoPeru(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboUnidadPeru()
        {
            return Json(almService.FillComboUnidadPeru(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUnidades(string term)
        {
            var items = almService.getUnidades(term);

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public MemoryStream CrearExcelInsumos()
        {
            var stream = almService.crearExcelInsumos();

            if (stream != null)
            {
                this.Response.Clear();
                this.Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                this.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", "Reporte Insumos.xlsx"));
                this.Response.BinaryWrite(stream.ToArray());
                this.Response.End();

                return stream;
            }
            else
            {
                return null;
            }
        }

        public ActionResult CargarExcel()
        {
            return Json(almService.cargarExcel(Request.Files), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCentroCosto(string cc)
        {
            return Json(almService.getCentroCosto(cc), JsonRequestBehavior.AllowGet);
        }

        #region Devolución Entrada
        public ActionResult GetNuevaDevolucionEntrada(int almacenID)
        {
            return Json(almService.getNuevaDevolucionEntrada(almacenID), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDevolucionEntrada(int almacenID, int numero)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = almService.getDevolucionEntrada(almacenID, numero);

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

        public ActionResult GuardarDevolucionEntrada(MovimientoEnkontrolDTO movimiento)
        {
            var result = new Dictionary<string, object>();

            try
            {
                almService.checarUbicacionesValidas(movimiento.detalle);
                List<entradasAlmacenDTO> entradas = almService.guardarDevolucionEntrada(movimiento);

                Session["entradasDevolucion"] = entradas;

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

        #region Devolución Salida
        public ActionResult GetNuevaDevolucionSalida(int almacenID)
        {
            return Json(almService.getNuevaDevolucionSalida(almacenID), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDevolucionSalida(int almacenID, int numero)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = almService.getDevolucionSalida(almacenID, numero);

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

        public ActionResult GuardarDevolucionSalida(MovimientoEnkontrolDTO movimiento)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var flagMaquinaStandBy = false;
                //if (vSesiones.sesionEmpresaActual == 2)
                //{
                //    flagMaquinaStandBy = almService.checkMaquinaStandBy(movimiento.cc);
                //}

                List<salidasAlmacenDTO> salidas = almService.guardarDevolucionSalida(movimiento);

                Session["salidasDevolucion"] = salidas;

                result.Add(SUCCESS, true);
                result.Add("flagMaquinaStandBy", flagMaquinaStandBy);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEntradasCompra(int almacen, string cc, int numeroOrdenCompra)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = almService.getEntradasCompra(almacen, cc, numeroOrdenCompra);

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

        public ActionResult ImprimirMovimientoSalidaDevolucion(int almacen, int numero)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = almService.imprimirMovimientoSalidaDevolucion(almacen, numero);

                Session["salidasDevolucion"] = data;

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
        #endregion

        #region Inventario Físico Entrada
        public ActionResult GetNuevaEntradaInventarioFisico(int almacenID)
        {
            return Json(almService.getNuevaEntradaInventarioFisico(almacenID), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEntradaInventarioFisico(int almacenID, int numero)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = almService.getEntradaInventarioFisico(almacenID, numero);

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

        public ActionResult GuardarEntradaInventarioFisico(MovimientoEnkontrolDTO movimiento)
        {
            var result = new Dictionary<string, object>();

            try
            {
                almService.checarUbicacionesValidas(movimiento.detalle);
                List<entradasAlmacenDTO> entradas = almService.guardarEntradaInventarioFisico(movimiento);

                Session["entradasFisico"] = entradas;

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetReporteEntradaFisico(int almacen, int numero)
        {
            var result = new Dictionary<string, object>();

            try
            {
                List<entradasAlmacenDTO> entradas = almService.getReporteEntradaFisico(almacen, numero);

                if (entradas == null)
                {
                    result.Add(SUCCESS, false);
                    result.Add(MESSAGE, "No hay registros con ese número de movimiento o no tiene detalles.");
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                Session["entradasFisico"] = entradas;

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, "No hay registros con ese número de movimiento.");
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Inventario Físico Salida
        public ActionResult GetNuevaSalidaInventarioFisico(int almacenID)
        {
            return Json(almService.getNuevaSalidaInventarioFisico(almacenID), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSalidaInventarioFisico(int almacenID, int numero)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = almService.getSalidaInventarioFisico(almacenID, numero);

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

        public ActionResult GuardarSalidaInventarioFisico(MovimientoEnkontrolDTO movimiento)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var flagMaquinaStandBy = false;
                //if (vSesiones.sesionEmpresaActual == 2)
                //{
                //    flagMaquinaStandBy = almService.checkMaquinaStandBy(movimiento.cc);
                //}

                List<salidasAlmacenDTO> salidas = almService.guardarSalidaInventarioFisico(movimiento, false);

                Session["salidasFisico"] = salidas;

                result.Add(SUCCESS, true);
                result.Add("flagMaquinaStandBy", flagMaquinaStandBy);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ImprimirMovimientoSalidaInventarioFisico(int almacen, int numero)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = almService.ImprimirMovimientoSalidaInventarioFisico(almacen, numero);

                Session["salidasFisico"] = data;

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

        public ActionResult CargarExcelSalidaFisico()
        {
            return Json(almService.CargarExcelSalidaFisico(Request.Files), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Salida Por Consumo Sin Origen
        public ActionResult GetNuevaSalidaConsumo(int almacenID)
        {
            return Json(almService.getNuevaSalidaConsumo(almacenID), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSalidaConsumo(int almacenID, int numero)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = almService.getSalidaConsumo(almacenID, numero);

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

        public ActionResult GuardarSalidaConsumo(MovimientoEnkontrolDTO movimiento)
        {
            var data = almService.guardarSalidaConsumo(movimiento);

            Session["salidasConsumo"] = data["salidas"];

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ImprimirMovimientoSalidaConsumo(int almacen, int numero)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = almService.imprimirMovimientoSalidaConsumo(almacen, numero);

                Session["salidasConsumo"] = data;

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
        #endregion

        #region Salida Por Consumo Con Origen
        public ActionResult GuardarSalidaConsumoOrigen(MovimientoEnkontrolDTO movimiento)
        {
            var data = almService.guardarSalidaConsumoOrigen(movimiento);

            Session["salidasConsumo"] = data["salidas"];

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult GetHistorialInsumo(int almacen, int insumo)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = almService.getHistorialInsumo(almacen, insumo);

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

        public ActionResult GetEmpleadosPendientesLiberacion()
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = almService.getEmpleadosPendientesLiberacion();

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
                almService.guardarBajas(empleados);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCatalogoUbicaciones(int almacenID)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = almService.getCatalogoUbicaciones(almacenID);

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

        public ActionResult FillComboTipoMovimiento()
        {
            var result = new Dictionary<string, object>();

            try
            {
                result.Add(ITEMS, almService.FillComboTipoMovimiento());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarMovimiento(int almacen, int tipo_mov, int numero)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = almService.cargarMovimiento(almacen, tipo_mov, numero);

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

        public ActionResult ChecarAccesoAlmacenista(int almacen)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = almService.checarAccesoAlmacenista(almacen);

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

        public ActionResult InsertTraspasosPendientes()
        {
            var result = new Dictionary<string, object>();

            try
            {
                almService.insertTraspasosPendientes();

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetExistencias(int empresa, int insumo, int almacen)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = almService.getExistencias(empresa, insumo, almacen);

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

        public ActionResult CargarExcelTraspasoMasivo()
        {
            var result = new Dictionary<string, object>();

            try
            {
                if (Request.Files.Count > 0)
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileBase archivo = Request.Files[i];

                        almService.cargarExcelTraspasoMasivo(archivo);
                    }

                    result.Add(SUCCESS, true);
                }
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckPermisoTraspasoMasivo()
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = almService.checkPermisoTraspasoMasivo();

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

        #region Re-impresión de Traspasos
        public ActionResult ImprimirMovimientoEntradaTraspaso(int almacen, int numero)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = almService.imprimirMovimientoEntradaTraspaso(almacen, numero);

                if (data == null)
                {
                    Session["entradaConsultaTraspaso"] = null;
                }
                else
                {
                    Session["entradaConsultaTraspaso"] = data;
                }

                //Session["foliosEntradas"] = data;

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

        public ActionResult ImprimirMovimientoSalidaTraspaso(int almacen, int numero)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = almService.imprimirMovimientoSalidaTraspaso(almacen, numero);

                Session["foliosSalidas"] = data;

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
        #endregion

        public ActionResult GetCentrosCostoModal()
        {
            var result = new Dictionary<string, object>();

            try
            {
                result.Add("data", almService.getCentrosCosto());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetNuevaSalidaConsultaTraspaso(int almacenID)
        {
            return Json(almService.getNuevaSalidaConsultaTraspaso(almacenID), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetNuevaEntradaConsultaTraspaso(int almacenID)
        {
            return Json(almService.getNuevaEntradaConsultaTraspaso(almacenID), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSalidaConsultaTraspasoDirecto(int almacenID, int numero)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = almService.getSalidaConsultaTraspaso(almacenID, numero);

                if (data == null)
                {
                    Session["salidaConsultaTraspaso"] = null;
                    result.Add("noExiste", true);
                }
                else
                {
                    Session["salidaConsultaTraspaso"] = data.Item2;
                    result.Add("data", data.Item1);
                    result.Add("dataReporte", data.Item2);
                    result.Add("noExiste", false);
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

        public ActionResult GetSalidaConsultaTraspaso(int almacenID, int numero)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = almService.getSalidaConsultaTraspaso(almacenID, numero);

                if (data == null)
                {
                    Session["salidaConsultaTraspaso"] = null;
                    throw new Exception("No se encuentra el movimiento.");
                }
                else
                {
                    Session["salidaConsultaTraspaso"] = data.Item2;
                }

                result.Add("data", data.Item1);
                result.Add("dataReporte", data.Item2);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEntradaConsultaTraspasoDirecto(int almacenID, int numero)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = almService.getEntradaConsultaTraspaso(almacenID, numero);

                if (data == null)
                {
                    Session["entradaConsultaTraspaso"] = null;
                    result.Add("noExiste", true);
                }
                else
                {
                    Session["entradaConsultaTraspaso"] = data.Item2;
                    result.Add("data", data.Item1);
                    result.Add("dataReporte", data.Item2);
                    result.Add("noExiste", false);
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

        public ActionResult GetEntradaConsultaTraspaso(int almacenID, int numero)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var data = almService.getEntradaConsultaTraspaso(almacenID, numero);

                if (data == null)
                {
                    Session["entradaConsultaTraspaso"] = null;
                    throw new Exception("No se encuentra el movimiento.");
                }
                else
                {
                    Session["entradaConsultaTraspaso"] = data.Item2;
                }

                result.Add("data", data.Item1);
                result.Add("dataReporte", data.Item2);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChecarPermisosFamilias(int almacen, List<int> insumos)
        {
            var result = new Dictionary<string, object>();

            try
            {
                result.Add("flagPermiso", almService.checarPermisosFamilias(almacen, insumos));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChecarPermisoAreaCuenta()
        {
            var result = new Dictionary<string, object>();

            try
            {
                var flag = almService.checarPermisoAreaCuenta();

                result.Add("flagPermiso_14_1_14_2", flag);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ObtenerComprasPendientes(string strlistaCC, int estatus, List<int> listaAlmacenes, DateTime fechaInicio, DateTime fechaFin)
        {
            var result = new Dictionary<string, object>();

            try
            {
                List<string> listaCC = JsonConvert.DeserializeObject<List<string>>(strlistaCC);
                var data = almService.ObtenerComprasPendientes(listaCC, estatus, listaAlmacenes, fechaInicio, fechaFin);

                result.Add(SUCCESS, true);
                result.Add("data", data);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CorregirUbicacionesSalidas()
        {
            var result = new Dictionary<string, object>();

            try
            {
                var corregido = almService.corregirUbicacionesSalidas();

                result.Add(SUCCESS, corregido);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAlmacenes()
        {
            result = new Dictionary<string, object>();
            try
            {
                var resultado = almService.GetAlmacenes();
                var r = almService.ObtenerMovimientos();
                var a = r.Where(n => n.almacen == 914).ToList().Count();
                var lst = resultado.Select(y => new si_almacenDTO
                {
                    almacen = y.almacen,
                    descripcion = y.descripcion,
                    direccion = y.direccion,
                    responsable = y.responsable,
                    telefono = y.telefono,
                    valida_almacen_cc = y.valida_almacen_cc,
                    bit_pt = y.bit_pt,
                    bit_mp = y.bit_mp,
                    cc = y.cc,
                    almacen_virtual = y.almacen_virtual,
                    botonEliminar = r.Where(n => n.almacen == y.almacen).ToList().Count() != 0 ? true : false,
                }).ToList();

                result.Add(ITEMS, lst);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {

                result.Add(ITEMS, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarAlmacen(si_almacenDTO datos)
        {
            result = almService.GuardarAlmacen(datos);

            return Json(result);
        }
        
        public ActionResult GetUsuarioEnkontrolByID(int empleado)
        {
            return Json(almService.getUsuarioEnkontrolByID(empleado), JsonRequestBehavior.AllowGet);
        }

        #region Inventario Físico
        public ActionResult CargarExistenciasAlmacen(int almacen, DateTime fecha, bool existentes)
        {
            Session["reporteInventarioFisicoAlmacen"] = almacen;
            Session["reporteInventarioFisicoFecha"] = fecha.ToShortDateString();
            Session["reporteInventarioFisicoExistentes"] = existentes;

            return Json(almService.cargarExistenciasAlmacen(almacen, fecha, existentes), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarInventarioFisico(int almacen, DateTime fecha)
        {
            return Json(almService.cargarInventarioFisico(almacen, fecha), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarInventarioFisicoInsumo(string cc, int almacen, DateTime fecha, int insumoInicio, int insumoFin, bool soloConDiferencia)
        {
            Session["reporteValuacionInventarioFisicoCC"] = cc;
            Session["reporteValuacionInventarioFisicoAlmacen"] = almacen;
            Session["reporteValuacionInventarioFisicoFecha"] = fecha.ToShortDateString();
            Session["reporteValuacionInventarioFisicoInsumIni"] = insumoInicio;
            Session["reporteValuacionInventarioFisicoInsumFin"] = insumoFin;
            Session["reporteValuacionInventarioFisicoDiferencia"] = soloConDiferencia;
            
            return Json(almService.cargarInventarioFisico(cc, almacen, fecha, insumoInicio, insumoFin, soloConDiferencia), JsonRequestBehavior.AllowGet);
        }

        public ActionResult cargarIntervaloInsumos()
        {
            return Json(almService.cargarIntervaloInsumos(), JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult GuardarInventarioFisico(string partidasString)
        {
            var partidas = JsonUtils.convertJsonToNetObject<List<FisicoDetalleDTO>>(partidasString, "es-MX");

            return Json(almService.guardarInventarioFisico(partidas), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarPartidaInventarioFisico(FisicoDetalleDTO partida)
        {
            return Json(almService.eliminarPartidaInventarioFisico(partida), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CongelarAlmacenInventarioFisico(int almacen, DateTime fecha)
        {
            return Json(almService.congelarAlmacenInventarioFisico(almacen, fecha), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CerrarInventarioFisico(int almacen, DateTime fecha)
        {
            return Json(almService.cerrarInventarioFisico(almacen, fecha), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarDescripcionAlmacen(int almacen)
        {
            return Json(almService.CargarDescripcionAlmacen(almacen), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarDescripcionCC(string cc)
        {
            return Json(almService.CargarDescripcionCC(cc), JsonRequestBehavior.AllowGet);
        }

        public bool PermisoCierreInventario()
        {
            if (almService == null)
            {
                almService = new AlmacenFactoryService().getAlmService();
            }

            var permiso = almService.getPermisoCierreInventario();

            return permiso != null;
        }
        #endregion
        [HttpPost]
        public JsonResult EliminarAlmacen(int almacen)
        {
            result = new Dictionary<string, object>();
            var re = almService.EliminarAlmacen(almacen);
            result.Add(SUCCESS, re);
            return Json(result);
        }

        [HttpPost]
        public JsonResult ObtenerAlmacenEditaroAgregar(int almacen)
        {
            result = new Dictionary<string, object>();
            var re = almService.ObtenerAlmacenEditaroAgregar(almacen);
            result.Add("data", re);
            return Json(result);
        }

        [HttpPost]
        public JsonResult EditarAlmacen(si_almacenDTO datos)
        {
            result = new Dictionary<string, object>();
            var a = almService.EditarAlmacen(datos);
            result.Add(SUCCESS, a);
            return Json(result);
        }

        [HttpPost]
        public JsonResult obtenerAlmacenes()
        {
            result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, almService.GetAlmacenes());
                result.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                result.Add(ITEMS, null);
                result.Add(SUCCESS, false);
                throw;
            }
            return Json(result);
        }
        [HttpPost]
        public JsonResult ObtenerExistenciaInventario(int almacen, DateTime fecha, bool existentes, bool ultimoPrecio, int insumoInicio, int insumoFin, bool soloConDiferencia)
        {
            result = new Dictionary<string, object>();
            try
            {
                var data = almService.obtenerExistenciasvsInventario(almacen, fecha, existentes, ultimoPrecio, insumoInicio, insumoFin, soloConDiferencia);
                int totalNumero = data.Count();
                int exactitudNumero = data.Where(x => x.diferencia == 0).Count();
                int difNumero = totalNumero - exactitudNumero;
                decimal exactitudPorcentaje = totalNumero != 0 ? ((decimal)exactitudNumero * (decimal)100) / (decimal)totalNumero : 0;
                decimal difPorcentaje = totalNumero != 0 ? ((decimal)difNumero * (decimal)100) / (decimal)totalNumero : 0;

                decimal difAbono = data.Where(x => x.diferencia != 0).Sum(x => Decimal.Parse(x.abono));
                decimal difCargo = data.Where(x => x.diferencia != 0).Sum(x => Decimal.Parse(x.cargos));
                decimal difTotal = difAbono + difCargo;

                result.Add(ITEMS, data);
                result.Add("difNumero", difNumero);
                result.Add("difPorcentaje", difPorcentaje);
                result.Add("exactitudNumero", exactitudNumero);
                result.Add("exactitudPorcentaje", exactitudPorcentaje);
                result.Add("totalNumero", totalNumero);
                result.Add("totalPorcentaje", 100);

                result.Add("difAbono", difAbono);
                result.Add("difCargo", difCargo);
                result.Add("difTotal", difTotal);

                result.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                result.Add(ITEMS, null);
                result.Add(SUCCESS, false);
                throw;
            }
            return Json(result);
        }
        [HttpPost]
        public JsonResult ObtenerExistenciaInventarioreporte(int almacen, DateTime fecha, bool existentes, bool ultimoPrecio, int insumoInicio, int insumoFin, bool soloConDiferencia)
        {
            result = new Dictionary<string, object>();
            try
            {
                Session["rptObtener"] = almService.obtenerExistenciasvsInventario(almacen, fecha, existentes, ultimoPrecio, insumoInicio, insumoFin, soloConDiferencia);
                result.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                result.Add(ITEMS, null);
                result.Add(SUCCESS, false);
                throw;
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult ObtenerAlmacenID(int almacen)
        {
            result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, almService.ObtenerAlmacenID(almacen));
                result.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                result.Add(ITEMS, null);
                result.Add(SUCCESS, false);
                throw;
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult consultarPrimerInsumo(int insumo)
        {
            result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, almService.consultarPrimerInsumo(insumo));
                result.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                result.Add(ITEMS, null);
                result.Add(SUCCESS, false);
                throw;
            }
            return Json(result);
        }
        [HttpPost]
        public JsonResult consultarUltimoInsumo(int insumo)
        {
            result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, almService.consultarUltimoInsumo(insumo));
                result.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                result.Add(ITEMS, null);
                result.Add(SUCCESS, false);
                throw;
            }
            return Json(result);
        }

        [HttpPost]
        public ActionResult ObtenerInsumos(int Almacen, int pagina, int registros)
        {
            var json = Json(almService.Obtenerinsumos(Almacen,pagina,registros), JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        #region Actualización de Ubicaciones

        public ActionResult CargarExistenciasAlmacenPorInsumo(int almacen, int insumo, bool existentes = true)
        {
            return Json(almService.cargarExistenciasAlmacen(almacen, insumo, existentes), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ActualizacionUbicacionInsumo(int almacen, int insumo, string cc, tblAlm_Movimientos movimiento, List<tblAlm_MovimientosDet> detallesMovimientoSalida, List<tblAlm_MovimientosDet> detallesMovimientoEntrada)
        {
            return Json(almService.ActualizacionUbicacionInsumo(almacen, insumo, cc, movimiento, detallesMovimientoSalida, detallesMovimientoEntrada), JsonRequestBehavior.AllowGet);
        }

        #endregion

        public ActionResult CargarExcelSalidaTraspaso()
        {
            return Json(almService.CargarExcelSalidaTraspaso(Request.Files), JsonRequestBehavior.AllowGet);
        }

        #region NUEVO CATALOGO PARA RELACION DE AREA ALMACEN
        [HttpPost]
        public JsonResult getAreaAlmacen(string AreaCuenta)
        {
            result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, almService.getAreaAlmacen(AreaCuenta));
                result.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                result.Add(ITEMS, null);
                result.Add(SUCCESS, false);
                throw;
            }
            return Json(result);
        }
        [HttpPost]
        public JsonResult GuardarEditarAreaAlmacen(AreaAlmacenDTO parametros)
        {
            result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, almService.GuardarEditarAreaAlmacen(parametros));
                result.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                result.Add(ITEMS, null);
                result.Add(SUCCESS, false);
                throw;
            }
            return Json(result);
        }
        [HttpPost]
        public JsonResult EliminarAreaAlmacen(int id)
        {
            result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, almService.EliminarAreaAlmacen(id));
                result.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                result.Add(ITEMS, null);
                result.Add(SUCCESS, false);
                throw;
            }
            return Json(result);
        }
        [HttpPost]
        public JsonResult getAlmacenesAreaDisponibles(int idRelacion)
        {
            result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, almService.getAlmacenesAreaDisponibles(idRelacion));
                result.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                result.Add(ITEMS, null);
                result.Add(SUCCESS, false);
                throw;
            }
            return Json(result);
        }
            [HttpPost]
        public JsonResult getAreaCuentas(int idRelacion)
        {
            result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, almService.getAreaCuentas(idRelacion));
                result.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                result.Add(ITEMS, null);
                result.Add(SUCCESS, false);
                throw;
            }
            return Json(result);
        }
            public JsonResult getTodasAreaCuentas()
            {
                result = new Dictionary<string, object>();
                try
                {
                    result.Add(ITEMS, almService.getTodasAreaCuentas());
                    result.Add(SUCCESS, true);
                }
                catch (Exception)
                {
                    result.Add(ITEMS, null);
                    result.Add(SUCCESS, false);
                    throw;
                }
                return Json(result);
            }
            public JsonResult getDetalleAreaAlmacen(int idRelacion)
            {
                result = new Dictionary<string, object>();
                try
                {
                    result.Add(ITEMS, almService.getDetalleAreaAlmacen(idRelacion));
                    result.Add(SUCCESS, true);
                }
                catch (Exception)
                {
                    result.Add(ITEMS, null);
                    result.Add(SUCCESS, false);
                    throw;
                }
                return Json(result);
            }
        
        #endregion

        public ActionResult CrearRequisicionCompraConciliacion()
            {
                var cc = "012";
                var comentario = "Comentario Prueba";
                var tipoCC = TipoCentroCostoEnum.ADMINISTRATIVO;
                var precio = 123;
                var tipoMoneda = 1;
                var precioDolar = 20;
                var porcentajeIVA = 16;

                return Json(almService.crearRequisicionCompraConciliacion(cc, comentario, tipoCC, precio, tipoMoneda, precioDolar, porcentajeIVA), JsonRequestBehavior.AllowGet);
            }

        #region Remanentes
        public JsonResult CargarRemanentes(List<int> listaAlmacenes, DateTime fechaInicio, DateTime fechaFin, int solicitante)
        {
            var json = Json(almService.CargarRemanentes(listaAlmacenes, fechaInicio, fechaFin, solicitante), JsonRequestBehavior.AllowGet);

            json.MaxJsonLength = int.MaxValue;

            return json;
        }

        public ActionResult FillComboAlmacenesFisicos()
        {
            var result = new Dictionary<string, object>();

            try
            {
                result.Add("items", almService.FillComboAlmacenesFisicos());
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarRegistroRemanente(int remanente_id)
        {
            return Json(almService.EliminarRegistroRemanente(remanente_id), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Catálogo Ubicaciones
        public ActionResult GetUbicaciones()
        {
            return Json(almService.GetUbicaciones(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarNuevaUbicacion(tblAlm_Ubicacion ubicacion)
        {
            return Json(almService.GuardarNuevaUbicacion(ubicacion), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarUbicacion(tblAlm_Ubicacion ubicacion)
        {
            return Json(almService.EditarUbicacion(ubicacion), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarUbicacion(tblAlm_Ubicacion ubicacion)
        {
            return Json(almService.EliminarUbicacion(ubicacion), JsonRequestBehavior.AllowGet);
        }
        #endregion
    
        #region Catalogos Insumos
        public ActionResult FillGrid_InsumoTipo(tblAlm_Insumo_Tipo obj)
        {
            var result = new Dictionary<string, object>();
            var listResult = almService.FillGrid_InsumoTipo(obj).Select(x => new { id = x.id, tipo = x.tipo, descripcion = x.descripcion, estatus = (x.estatus == true) ? "ACTIVO" : "INACTIVO" });
            result.Add("current", 1);
            result.Add("rowCount", 1);
            result.Add("total", listResult.Count());
            result.Add("rows", listResult);
            result.Add(SUCCESS, true);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveOrUpdate_InsumoTipo(tblAlm_Insumo_Tipo obj, int Actualizacion)
        {
            var result = new Dictionary<string, object>();
            try
            {
                almService.SaveOrUpdate_InsumoTipo(obj);
                result.Add(MESSAGE, GlobalUtils.getMensaje(Actualizacion));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillGrid_InsumoGrupo(tblAlm_Insumo_Grupo obj)
        {
            var result = new Dictionary<string, object>();
            var listResult = almService.FillGrid_InsumoGrupo(obj).Select(x => new { id = x.id, grupo = x.grupo, descripcion = x.descripcion, estatus = (x.estatus == true) ? "ACTIVO" : "INACTIVO" });
            result.Add("current", 1);
            result.Add("rowCount", 1);
            result.Add("total", listResult.Count());
            result.Add("rows", listResult);
            result.Add(SUCCESS, true);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveOrUpdate_InsumoGrupo(tblAlm_Insumo_Grupo obj, int Actualizacion)
        {
            var result = new Dictionary<string, object>();
            try
            {
                almService.SaveOrUpdate_InsumoGrupo(obj);
                result.Add(MESSAGE, GlobalUtils.getMensaje(Actualizacion));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillGrid_InsumoFamilia(tblAlm_Grupos_Insumo obj)
        {
            var result = new Dictionary<string, object>();
            var listResult = almService.FillGrid_InsumoFamilia(obj).Select(x => new { id = x.id, familia = x.familia, tipo = x.tipo_insumo, grupo = x.grupo_insumo, descripcion = x.descripcion, estatus = (x.estatus == true) ? "ACTIVO" : "INACTIVO" });
            result.Add("current", 1);
            result.Add("rowCount", 1);
            result.Add("total", listResult.Count());
            result.Add("rows", listResult);
            result.Add(SUCCESS, true);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveOrUpdate_InsumoFamilia(tblAlm_Grupos_Insumo obj, int Actualizacion)
        {
            var result = new Dictionary<string, object>();
            try
            {
                almService.SaveOrUpdate_InsumoFamilia(obj);
                result.Add(MESSAGE, GlobalUtils.getMensaje(Actualizacion));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboInsumoTipo(bool estatus)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, almService.FillCboInsumoTipo(estatus).Select(x => new { Value = x.tipo, Text = "[" + x.tipo + "] - " + x.descripcion }));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboInsumoGrupo(bool estatus)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, almService.FillCboInsumoGrupo(estatus).Select(x => new { Value = x.grupo, Text = "["+x.grupo + "] - " + x.descripcion }));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillGrid_Insumo(tblAlm_Insumo obj)
        {
            var result = new Dictionary<string, object>();
            var listResult = almService.FillGrid_Insumo(obj).Select(x => new { id = x.id, insumo = x.insumo, tipo = x.tipo, grupo = x.grupo, descripcion = x.descripcion, estatus = (x.estatus == true) ? "ACTIVO" : "INACTIVO" });
            result.Add("current", 1);
            result.Add("rowCount", 1);
            result.Add("total", listResult.Count());
            result.Add("rows", listResult);
            result.Add(SUCCESS, true);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveOrUpdate_Insumo(tblAlm_Insumo obj, int Actualizacion)
        {
            var result = new Dictionary<string, object>();
            try
            {
                almService.SaveOrUpdate_Insumo(obj);
                result.Add(MESSAGE, GlobalUtils.getMensaje(Actualizacion));
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
    }
}