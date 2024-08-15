using Core.DTO.Contabilidad.Propuesta;
using Core.DTO.Contabilidad;
using Core.DTO.Administracion;
using Core.Entity.Administrativo.Contabilidad;
using Core.Enum.Administracion.Propuesta;
using Data.Factory.Contabilidad.Reportes;
using Infrastructure.Utils;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data.Factory.Contabilidad.Propuesta;
using Core.Entity.Administrativo.Contabilidad.Propuesta.Acomulado;
using Data.Factory.Contabilidad;
using Core.DTO.Principal.Generales;
using Core.DTO;
using Data.Factory.Facturacion;
using Core.DTO.Utils.Excel;
using System.IO;
using System.Drawing;
using Core.Enum.Administracion.Propuesta.Nomina;
using Core.Entity.Administrativo.Contabilidad.Poliza;
using Core.Entity.Administrativo.Contabilidad.Propuesta.Nomina;
using Core.DTO.Contabilidad.Facturacion;
using Data.Factory.Maquinaria.Catalogos;
using Core.DTO.Contabilidad.Poliza;
using Core.Enum.Administracion.Cotizaciones;
using Core.Entity.Administrativo.Contabilidad.Propuesta.Proveedores;
using Core.Entity.Facturacion.Estimacion;
using Core.DTO.Contabilidad.Propuesta.Nomina;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Data.Factory.Encuestas;
using Core.Enum.Maquinaria.Reportes;
using Data.Factory.Maquinaria.Reporte;
using Core.DTO.Maquinaria.Reporte.Kubrix;
using Data.Factory.CuentasPorCobrar;
using Core.Entity.CuentasPorCobrar;
using Core.Enum.Multiempresa;
using Core.DTO.Contabilidad.Propuesta.Validacion;

namespace SIGOPLAN.Areas.Administrativo.Controllers.Contabilidad
{
    public class PropuestaController : BaseController
    {
        // GET: Administrativo/Propuesta
        #region init
        private CadenaProductivaFactoryServices cadenaFS;
        private ReservaFactoryServer reservaFS;
        private ChequeFactoryServices chequeFS;
        private PolizaFactoryServices polizaFS;
        private SaldoConciliadoFactoryServices saldoFS;
        private FacturaFactoryService facturaFS;
        private NominaResumenFactoryServices nominaFS;
        private CentroCostosFactoryServices ccFS;
        private CatGiroProvFactoryServices giroProvFS;
        private EstimacioProveedorFactoryServices estProvFS;
        private PropuestaProgramacionFactoryServices propPrpgFs;
        private EstimacionCobranzaFactoryServices estCobFS;
        private CostoEstimadoFactoryServices costoEstFS;
        private RentabilidadFactoryServices rentabilidadFS;
        private CuentasPorCobrarFactoryService cxcFS;
        private List<CcDTO> lstCC;
        private List<int> lstTipoChCta;
        private List<Infrastructure.DTO.ComboDTO> lstOverhaul;
        private List<int> lstTipoRitchieBros;
        private string ccRitchieBros;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            cadenaFS = new CadenaProductivaFactoryServices();
            reservaFS = new ReservaFactoryServer();
            chequeFS = new ChequeFactoryServices();
            polizaFS = new PolizaFactoryServices();
            saldoFS = new SaldoConciliadoFactoryServices();
            facturaFS = new FacturaFactoryService();
            nominaFS = new NominaResumenFactoryServices();
            ccFS = new CentroCostosFactoryServices();
            giroProvFS = new CatGiroProvFactoryServices();
            estProvFS = new EstimacioProveedorFactoryServices();
            propPrpgFs = new PropuestaProgramacionFactoryServices();
            estCobFS = new EstimacionCobranzaFactoryServices();
            costoEstFS = new CostoEstimadoFactoryServices();
            rentabilidadFS = new RentabilidadFactoryServices();
            cxcFS = new CuentasPorCobrarFactoryService();
            init();
            base.OnActionExecuting(filterContext);
        }
        #endregion
        #region Concentrado
        public ActionResult Concentrado()
        {
            return View();
        }
        public ActionResult ConsultaConcentrado()
        {
            return View();
        }
        public ActionResult getConcentrado(BusqConcentradoDTO busq)
        {
            var result = new Dictionary<string, object>();
            try
            {
                busq = generarBusqDTO(busq);
                var lstConcentrado = getLstConcentrado(busq);
                Session["lstConcentrado"] = lstConcentrado;
                result.Add("lstConcentrado", lstConcentrado);
                result.Add(SUCCESS, true);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getConsultaConcentrado(BusqConsultaConcentradoDTO busq)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstConcentrado = getConsultaConcentrado(busq);
                result.Add("lstConcentrado", lstConcentrado);
                result.Add(SUCCESS, true);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getBusqDTO()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var busq = generarBusqDTO();
                result.Add("busq", busq);
                result.Add(SUCCESS, true);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult acctionToProrrateo(string accion, List<ConcentradoDTO> lst)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var tipo = Regex.Match(accion, @"\d+").Value.ParseInt();
                var lstRes = new List<ReservaDTO>();
                var tipoCondicion = 0;
                var esPorcentaje = false;
                switch(accion[0])
                {
                    case 'a':
                        tipoCondicion = (int)TipoProrrateoReservaEnum.Automatico;
                        lst = (List<ConcentradoDTO>)Session["lstConcentrado"];
                        break;
                    case 's':
                        tipoCondicion = (int)TipoProrrateoReservaEnum.Seleccionado;
                        break;
                    default:
                        break;
                }
                var catRes = reservaFS.getReservasService().getLstCatReservasActivas().Where(w => w.esAutomatico).FirstOrDefault(w => w.id.Equals(tipo));
                var catCalc = reservaFS.getReservasService().getRelCatReservaCalculoActivo(tipo, tipoCondicion);
                var relCc = reservaFS.getReservasService().getRelCatReservaCcActivas(tipo, tipoCondicion);
                var relTm = reservaFS.getReservasService().getRelCatReservaTmActivas(tipo, tipoCondicion);
                var relTp = reservaFS.getReservasService().getRelCatReservaTpActivas(tipo, tipoCondicion);
                var lstCond = lst.Where(w => relCc.Any(a => a.cc.Equals(w.cc)))
                                .Where(w => relTm.Any(a => a.tm.Equals(w.tm)))
                                .Where(w => relTp.Any(a => a.tp.Equals(w.tp))).ToList();
                lstCond.ForEach(e => { e.tipoReserva = e.tipoReservaAutomatica = tipo; });
                Session["lstReservaDetalle"] = lstCond;
                switch(catCalc.idTipoCalculo)
                {
                    #region Suma por porcentaje
                    case 1:
                        var porcentaje = catCalc.porcentaje;
                        esPorcentaje = true;
                        lstRes.AddRange(lstCond.GroupBy(g => g.cc).Select(res => new ReservaDTO()
                        {
                            tipo = tipo,
                            fecha = lstCond.Max(s => s.fecha),
                            cc = setCC(res.Key),
                            cargo = res.Sum(s => s.abono),
                            abono = res.Sum(s => s.cargo),
                            porcentaje = porcentaje
                        }));
                        if(lstRes.Count > 0)
                        {
                            lstRes.Add(new ReservaDTO()
                            {
                                tipo = tipo,
                                fecha = lstCond.Max(s => s.fecha),
                                cc = string.Format("R{0:00}", tipo),
                                cargo = lstRes.Sum(s => s.abono),
                                abono = lstRes.Sum(s => s.cargo),
                                porcentaje = 100
                            });
                        }
                        break;
                    #endregion
                    #region Suma por Centro Costos
                    case 2:
                        esPorcentaje = false;
                        lstRes.AddRange(lstCond.GroupBy(g => g.cc).Select(res => new ReservaDTO()
                        {
                            tipo = tipo,
                            fecha = lstCond.Max(s => s.fecha),
                            cc = setCC(res.Key),
                            cargo = res.Sum(s => s.abono),
                            abono = res.Sum(s => s.cargo),
                            porcentaje = 1
                        }));
                        if(lstRes.Count > 0)
                        {
                            lstRes.Add(new ReservaDTO()
                            {
                                tipo = tipo,
                                fecha = lstCond.Max(s => s.fecha),
                                cc = string.Format("R{0:00}", tipo),
                                cargo = lstRes.Sum(s => s.abono),
                                abono = lstRes.Sum(s => s.cargo),
                                porcentaje = 1
                            });
                        }
                        break;
                    #endregion
                    #region Suma prorrateo
                    case 3:
                        esPorcentaje = false;
                        var count = lstCond.Count;
                        var cargoProlateado = lstCond.Sum(s => s.cargo) / count;
                        var abonoProlateado = lstCond.Sum(s => s.abono) / count;
                        lstRes.AddRange(lstCond.GroupBy(g => g.cc).Select(res => new ReservaDTO()
                        {
                            tipo = tipo,
                            fecha = lstCond.Max(s => s.fecha),
                            cc = setCC(res.Key),
                            cargo = cargoProlateado,
                            abono = abonoProlateado,
                            porcentaje = 1
                        }));
                        if(lstRes.Count > 0)
                        {
                            lstRes.Add(new ReservaDTO()
                            {
                                tipo = tipo,
                                fecha = lstCond.Max(s => s.fecha),
                                cc = string.Format("R{0:00}", tipo),
                                cargo = lstCond.Sum(s => s.abono),
                                abono = lstCond.Sum(s => s.cargo),
                                porcentaje = 1
                            });
                        }
                        break;
                    #endregion
                    default:
                        break;
                }
                var esSuccess = lstRes.Count > 0;
                result.Add(SUCCESS, esSuccess);
                if(esSuccess)
                {
                    result.Add("lst", lstRes);
                    result.Add("esPorcentaje", esPorcentaje);
                }
            }
            catch(Exception e)
            {
                Session["lstReservaDetalle"] = new List<ConcentradoDTO>();
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region catálogo reservas
        public ActionResult Catalogo()
        {
            return View();
        }
        public ActionResult _divCatalogo()
        {
            return PartialView();
        }
        public ActionResult _divCatReservas()
        {
            return PartialView();
        }
        public ActionResult getLstCatReserva()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lst = reservaFS.getReservasService().getLstCatReserva();
                var relCal = reservaFS.getReservasService().getRelCatResercaCalActiva();
                var relCc = reservaFS.getReservasService().getRelCatReservaCcActivas();
                var relTm = reservaFS.getReservasService().getRelCatReservaTmActivas();
                var relTp = reservaFS.getReservasService().getRelCatReservaTpActivas();
                var lstCatRes = lst.Select(catReserva => new CatReservaDTO()
                {
                    catReserva = catReserva,
                    lstCalc = relCal.Where(c => c.idCatReserva.Equals(catReserva.id)).ToList(),
                    lstCc = relCc.Where(c => c.idCatReserva.Equals(catReserva.id)).ToList(),
                    lstTm = relTm.Where(c => c.idCatReserva.Equals(catReserva.id)).ToList(),
                    lstTp = relTp.Where(c => c.idCatReserva.Equals(catReserva.id)).ToList(),
                });
                result.Add("lst", lstCatRes);
                result.Add(SUCCESS, lst.Count > 0);
            }
            catch(Exception o_O)
            {
                result.Add(MESSAGE, o_O.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult guardarCatReserva(CatReservaDTO obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var esGuardado = false;
                if(obj.catReserva.descripcion.Length > 0)
                {
                    esGuardado = reservaFS.getReservasService().guardarCatReserva(obj);
                }
                result.Add(SUCCESS, esGuardado);
            }
            catch(Exception o_O)
            {
                result.Add(MESSAGE, o_O.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Reserva
        // GET: Administrativo/Propuesta
        public ActionResult Reserva()
        {
            return View();
        }
        public ActionResult _mdlReservas()
        {
            return PartialView();
        }
        public ActionResult _tblReservasTotales()
        {
            return PartialView();
        }
        public ActionResult guardarReservaImpIva(List<ReservaDTO> lst)
        {
            return guardarReserva(lst);
        }
        public ActionResult guardarReserva(List<ReservaDTO> lst)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var esGuardado = false;
                var esValida = lst.All(r => r.esValida());
                var primero = lst.FirstOrDefault();
                var lstCatRes = reservaFS.getReservasService().getLstCatReservasActivas();
                var lstReservaDetalle = (List<ConcentradoDTO>)Session["lstReservaDetalle"];
                if(esValida)
                {
                    esGuardado = reservaFS.getReservasService().guardarReserva(lst);
                    if(esGuardado && lstReservaDetalle != null && primero.tipo.Equals(lstReservaDetalle.FirstOrDefault().tipoReserva) && lstCatRes.Any(cr => cr.id.Equals(primero.tipo) && (cr.esAutomatico || cr.esSeleccionado)))
                    {
                        lstReservaDetalle.ForEach(d =>
                        {
                            d.fecha = primero.fecha;
                        });
                        esGuardado = reservaFS.getReservasService().guardarReservaDetalle(lstReservaDetalle);
                        Session["lstReservaDetalle"] = new List<ConcentradoDTO>();
                    }
                }
                result.Add(SUCCESS, esGuardado);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ElimnarReserva(List<int> lst)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var esEliminado = false;
                var esValida = lst.All(r => r > 0);
                if(esValida)
                    esEliminado = reservaFS.getReservasService().ElimnarReserva(lst);
                result.Add(SUCCESS, esEliminado);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getLstReservas(BusqReservaDTO busq)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstReserva = getBusqReserva(busq);
                result.Add("lstReservas", lstReserva);
                result.Add(SUCCESS, lstReserva.Count > 0);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getTotalReservas(BusqReservaDTO busq)
        {
            var result = new Dictionary<string, object>();
            try
            {
                busq.fecha = busq.fecha.Equals(default(DateTime)) ? DateTime.Now : busq.fecha;
                var lstRes = reservaFS.getReservasService().getLstReservas(busq);
                var lstAnt = reservaFS.getReservasService().getLstReservasAnteriores(busq);
                lstRes.AddRange(lstAnt);
                var lstReservaTotal = lstRes.GroupBy(g => g.cc).Select(t => new
                {
                    cc = lstCC.FirstOrDefault(c => c.cc.Equals(t.Key)).descripcion,
                    total = t.Sum(s => s.cargo - s.abono).ToString("C2")
                }).ToList();
                result.Add("totalReservas", lstAnt.Sum(s => s.cargo - s.abono));
                result.Add("lstReservaTotal", lstReservaTotal);
                result.Add(SUCCESS, lstAnt.Count > 0);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        List<ReservaDTO> getBusqReserva(BusqReservaDTO busq)
        {
            var lstReserva = reservaFS.getReservasService().getLstReservas(busq);
            var lstAnt = reservaFS.getReservasService().getLstReservasAnteriores(busq);
            var res = lstReserva.Select(reserva => new ReservaDTO
            {
                id = reserva.id,
                fecha = reserva.fecha,
                tipo = reserva.tipo,
                cc = reserva.cc,
                anterior = lstAnt.Where(ant => ant.cc.Equals(reserva.cc)).Sum(s => s.cargo - s.abono),
                cargo = reserva.cargo,
                abono = reserva.abono,
                global = reserva.cargo - reserva.abono
            }).ToList();
            return res;
        }
        #endregion
        #region Saldo Globales
        public ActionResult SaldoGlobales()
        {
            return View();
        }
        public ActionResult _mdlSaldosGlobales()
        {
            return PartialView();
        }
        List<ConcentradoDTO> generarSaldos(BusqConcentradoDTO busq)
        {
            var lstConcentrado = new List<ConcentradoDTO>();
            lstConcentrado.AddRange(saldoConciliadoToConcentrado(busq));
            lstConcentrado.AddRange(edoCtaToConcentrado(busq));
            lstConcentrado.AddRange(cadenaToConcentrado(busq));
            lstConcentrado.AddRange(interesesNafinToConcentrado(busq));
            lstConcentrado.AddRange(reservaSinDetalleToConcentrado(busq));
            lstConcentrado.AddRange(anticipoPagadoToConcentrado(busq));
            lstConcentrado = setReservasAutomatica(lstConcentrado, busq);
            lstConcentrado = setObra(lstConcentrado);
            return lstConcentrado;
        }
        List<ConcentradoDTO> getDetConciliar(BusqConcentradoDTO busq)
        {
            var lstConcentrado = new List<ConcentradoDTO>();
            lstConcentrado.AddRange(edoCtaToConcentrado(busq));
            return lstConcentrado;
        }
        public ActionResult setConciliacion(BusqConcentradoDTO busq)
        {
            var result = new Dictionary<string, object>();
            try
            {
                busq = generarBusqDTO(busq);
                var esGuardado = false;
                var lstSaldos = getDetConciliar(busq);
                var esValida = lstSaldos.All(r => r.esValida());
                if(esValida)
                {
                    esGuardado = saldoFS.getSaldoConciliadoService().setConciliacion(lstSaldos);
                }
                result.Add("lstSaldos", lstSaldos);
                result.Add(SUCCESS, esGuardado && lstSaldos.Count > 0);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getSaldosActuales(BusqConcentradoDTO busq)
        {
            var result = new Dictionary<string, object>();
            try
            {
                busq = generarBusqDTO(busq);
                var lstSaldos = getLstSaldosGeneralesActuales(busq);
                Session["lstSaldosGlobales"] = lstSaldos;
                Session["busqConcentrado"] = busq;
                result.Add("lstSaldos", lstSaldos);
                result.Add(SUCCESS, lstSaldos.Count > 0);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        List<SaldoGlobalDTO> getLstSaldosGeneralesActuales(BusqConcentradoDTO busq)
        {
            var i = 0;
            var lstConcentrado = generarSaldos(busq);
            lstConcentrado = setObraPropuesta(lstConcentrado);
            var lstCatReservas = reservaFS.getReservasService().getLstCatReservasActivas();
            var lstReserva = reservaFS.getReservasService().getLstReservasCCR(busq);
            #region Estimaciones
            var lstCcEstimacion = reservaFS.getReservasService().getLstCCEstimacion();
            var lstEstimado = lstConcentrado
                .Where(w => lstCcEstimacion.Any(e => w.cc.Equals(e) && lstCC.Any(c => c.cc.Equals(w.cc) && !c.bit_area.Equals(TipoCCEnum.ObraCerradaGeneral))))
                .GroupBy(g => g.cc)
                .Where(w => w.Sum(s => s.cargo - s.abono) != 0)
                .Select(c => new SaldoGlobalDTO()
                {
                    clase = EnumExtensions.GetDescription(TipoSaldoGlobalEnum.Saldo),
                    orden = ++i,
                    descripcion = lstCC.FirstOrDefault(w => w.cc.Equals(c.Key)).descripcion,
                    saldo = c.Sum(s => s.cargo - s.abono)
                }).ToList();
            var sumEstimaciones = lstEstimado.Sum(s => s.saldo);
            lstEstimado.AddRange(new List<SaldoGlobalDTO>()
                {
                    new SaldoGlobalDTO(){
                    clase = EnumExtensions.GetDescription(TipoSaldoGlobalEnum.SaldoTotal),
                    descripcion = "SALDO EN BANCOS POR ESTIMACIONES",
                    saldo = sumEstimaciones
                    }, new SaldoGlobalDTO()
                });
            #endregion
            #region No estimados
            var lstNoEstimados = lstConcentrado
                .Where(w => !lstCcEstimacion.Any(e => w.cc.Equals(e)))
                .GroupBy(g => g.cc)
                .Where(w => w.Sum(s => s.cargo - s.abono) != 0)
                .Select(c => new SaldoGlobalDTO()
                {
                    clase = EnumExtensions.GetDescription(TipoSaldoGlobalEnum.Saldo),
                    orden = ++i,
                    descripcion = lstCC.FirstOrDefault(w => w.cc.Equals(c.Key)).descripcion,
                    saldo = c.Sum(s => s.cargo - s.abono)
                }).ToList();
            lstNoEstimados.Insert(0, new SaldoGlobalDTO()
            {
                clase = EnumExtensions.GetDescription(TipoSaldoGlobalEnum.Descripcion),
                descripcion = "OBRAS EJECUTADAS SIN ANTICIPOS"
            });
            var sumNoEstimaciones = lstNoEstimados.Sum(s => s.saldo);
            lstNoEstimados.AddRange(new List<SaldoGlobalDTO>()
            {
                new SaldoGlobalDTO()
                {
                    clase = EnumExtensions.GetDescription(TipoSaldoGlobalEnum.SaldoTotal),
                    descripcion = "SALDOS DE OBRAS SIN ANTICIPO",
                    saldo = sumNoEstimaciones
                },
                new SaldoGlobalDTO()
            });
            #endregion
            #region Sumas estimaciones
            var sumEstimacionesTotal = sumEstimaciones + sumNoEstimaciones;
            var lstSumEstimacion = new List<SaldoGlobalDTO>()
            {
                new SaldoGlobalDTO()
                {
                    clase = EnumExtensions.GetDescription(TipoSaldoGlobalEnum.Total),
                    descripcion = "SALDOS POR ESTIMACIONES Y OBRAS SIN ANTICIPO",
                    total = sumEstimacionesTotal
                },
                new SaldoGlobalDTO()
            };
            #endregion
            #region Anticipos
            var lstResAnticipo = reservaFS.getReservasService().getLstReservas(busq).Where(r => lstCatReservas.Any(c => c.id.Equals(r.tipo) && c.tipoReservaSaldoGlobal.Equals((int)TipoReservaSaldoGlobalEnum.Anticipos))).ToList();
            var lstAnticipos = new List<SaldoGlobalDTO>()
            {
                new SaldoGlobalDTO()
                {
                    clase = EnumExtensions.GetDescription(TipoSaldoGlobalEnum.Descripcion),
                    descripcion = "ANTICIPOS EN M.N. DE OBRAS CONSTRUPLAN"
                },
                new SaldoGlobalDTO(),
                new SaldoGlobalDTO()
                {
                    clase = EnumExtensions.GetDescription(TipoSaldoGlobalEnum.Descripcion),
                    descripcion = "ANTICIPOS"
                },
                new SaldoGlobalDTO(),
            };
            lstAnticipos.AddRange(lstResAnticipo
                .Select(a => new SaldoGlobalDTO()
                {
                    clase = EnumExtensions.GetDescription(TipoSaldoGlobalEnum.Saldo),
                    orden = ++i,
                    descripcion = string.Format("{0} {1}", lstCatReservas.FirstOrDefault(w => w.id.Equals(a.tipo)).descripcion.ToUpper(), lstCC.FirstOrDefault(c => c.cc.Equals(a.cc)).descripcion).ToUpper(),
                    saldo = a.cargo - a.abono
                }).OrderByDescending(o => o.saldo).ToList());
            var sumAnticipos = lstAnticipos.Sum(s => s.saldo);
            lstAnticipos.AddRange(new List<SaldoGlobalDTO>() 
            { 
                new SaldoGlobalDTO(){
                    clase = EnumExtensions.GetDescription(TipoSaldoGlobalEnum.SaldoTotal),
                    descripcion = "SALDO DE ANTICIPOS POR AMORTIZAR",
                    saldo = sumAnticipos
                },
                new SaldoGlobalDTO(),
            });
            #endregion
            #region Otros
            var lstTipoReservas = lstReserva.Where(r => lstCatReservas.Any(a => a.id.Equals(r.tipo) && a.tipoReservaSaldoGlobal.Equals((int)TipoReservaSaldoGlobalEnum.Otros))).ToList();
            var lstOtros = new List<SaldoGlobalDTO>()
            {
                new SaldoGlobalDTO(){
                    clase = EnumExtensions.GetDescription(TipoSaldoGlobalEnum.Descripcion),
                    descripcion = "OTROS Y RESERVAS EN M.N. DE OBRAS CONSTRUPLAN"
                },
                new SaldoGlobalDTO(),
                new SaldoGlobalDTO(){
                    clase = EnumExtensions.GetDescription(TipoSaldoGlobalEnum.Descripcion),
                    descripcion = "O T R O S"
                }
            };
            var saldoIndustrial = getSaldoRestante(busq);
            saldoIndustrial.orden = ++i;
            lstOtros.Add(saldoIndustrial);
            var lstResCtrl = lstTipoReservas.GroupBy(g => g.tipo).Select(s => s.Key).ToList();
            lstResCtrl.ForEach(ctrl =>
            {
                var res = lstReserva.Where(r => r.tipo.Equals(ctrl)).ToList();
                var saldo = res.Where(r => r.cc.Equals(string.Format("R{0:00}", ctrl))).Sum(s => s.cargo - s.abono);
                if(saldo > 0)
                {
                    lstOtros.Add(new SaldoGlobalDTO()
                    {
                        clase = EnumExtensions.GetDescription(TipoSaldoGlobalEnum.Saldo),
                        orden = ++i,
                        descripcion = string.Format("RESERVA PARA {0}", lstCatReservas.FirstOrDefault(cr => ctrl.Equals(cr.id)).descripcion).ToUpper(),
                        saldo = saldo
                    });
                }
            });
            var sumOtros = lstOtros.Sum(s => s.saldo);
            lstOtros.AddRange(new List<SaldoGlobalDTO>()
            {
                    new SaldoGlobalDTO()
                    {
                        clase = EnumExtensions.GetDescription(TipoSaldoGlobalEnum.SaldoTotal),
                        descripcion = "SALDO TOTAL OTROS",
                        saldo = sumOtros
                    },
                new SaldoGlobalDTO()
            });
            #endregion
            #region Reservas
            var lstVta = new List<SaldoGlobalDTO>()
            {
                new SaldoGlobalDTO(){
                    clase = EnumExtensions.GetDescription(TipoSaldoGlobalEnum.Descripcion),
                    descripcion = "R E S E R V A S "
                }
            };
            lstVta.AddRange(lstReserva
                .Where(rb => rb.tipo.Equals((int)tipoReservaEnum.RitchieBros))
                .GroupBy(g => g.tipo)
                .Select(rb => new SaldoGlobalDTO()
                {
                    clase = EnumExtensions.GetDescription(TipoSaldoGlobalEnum.Saldo),
                    descripcion = lstCatReservas.FirstOrDefault(w => w.id.Equals(rb.Key)).descripcion.ToUpper(),
                    orden = ++i,
                    saldo = rb.Sum(s => s.cargo - s.abono)
                }).OrderByDescending(o => o.saldo).ToList());
            var sumVta = lstVta.Sum(s => s.saldo);
            lstVta.AddRange(new List<SaldoGlobalDTO>()
                {
                    new SaldoGlobalDTO(){
                    clase = EnumExtensions.GetDescription(TipoSaldoGlobalEnum.SaldoTotal),
                    descripcion = "SALDO TOTAL DE RESERVAS",
                    saldo = sumVta
                },
                new SaldoGlobalDTO()
                });
            #endregion
            #region Impuestos
            var lstReservaImpuesto = new List<SaldoGlobalDTO>()
            {
                new SaldoGlobalDTO(){
                    clase = EnumExtensions.GetDescription(TipoSaldoGlobalEnum.Descripcion),
                    descripcion = "RESERVAS PARA IMPUESTOS M.N."
                }
            };
            lstReservaImpuesto.AddRange(lstReserva
                .Where(a => lstCatReservas.Any(cr => a.tipo.Equals(cr.id) && cr.tipoReservaSaldoGlobal.Equals((int)TipoReservaSaldoGlobalEnum.Impuestos)))
                .GroupBy(g => g.tipo)
                .Select(r => new SaldoGlobalDTO()
                {
                    clase = EnumExtensions.GetDescription(TipoSaldoGlobalEnum.Saldo),
                    orden = ++i,
                    descripcion = string.Format("{0} {1}", lstCatReservas.FirstOrDefault(w => w.id.Equals(r.Key)).descripcion.ToUpper(), busq.max.Year).ToUpper(),
                    saldo = r.Sum(s => s.cargo - s.abono)
                }).ToList());
            var sumImpuestos = lstReservaImpuesto.Sum(s => s.saldo);
            lstReservaImpuesto.AddRange(new List<SaldoGlobalDTO>() 
            {
                new SaldoGlobalDTO(){
                    clase = EnumExtensions.GetDescription(TipoSaldoGlobalEnum.SaldoTotal),
                    descripcion = "SALDO TOTAL DE RESERVAS IMPUESTOS",
                    saldo = lstReservaImpuesto.Sum(s => s.saldo)
                },
                new SaldoGlobalDTO()
            });
            #endregion
            #region saldos
            var lstSaldosGlobales = new List<SaldoGlobalDTO>();
            var sumAntResv = sumAnticipos + sumOtros + sumVta + sumImpuestos;
            var sumBancos = sumEstimacionesTotal + sumAntResv;
            var sumGranTotal = sumBancos;
            lstSaldosGlobales.AddRange(lstEstimado);
            lstSaldosGlobales.AddRange(lstNoEstimados);
            lstSaldosGlobales.AddRange(lstSumEstimacion);
            lstSaldosGlobales.AddRange(lstAnticipos);
            lstSaldosGlobales.AddRange(lstOtros);
            lstSaldosGlobales.AddRange(lstVta);
            lstSaldosGlobales.AddRange(lstReservaImpuesto);
            lstSaldosGlobales.Add(new SaldoGlobalDTO()
            {
                clase = EnumExtensions.GetDescription(TipoSaldoGlobalEnum.Total),
                descripcion = "SALDOS TOTAL DE ANTICIPOS OTROS Y RESERVAS EN M.N. DE OBRAS",
                total = sumAntResv
            });
            lstSaldosGlobales.Add(new SaldoGlobalDTO()
            {
                clase = EnumExtensions.GetDescription(TipoSaldoGlobalEnum.GranTotal),
                descripcion = "SALDO EN BANCOS POR ESTIMACIONES, RESERVAS DE IMPUESTOS Y ANTICIPOS",
                global = sumBancos
            });
            lstSaldosGlobales.Add(new SaldoGlobalDTO());
            lstSaldosGlobales.Add(new SaldoGlobalDTO()
            {
                clase = EnumExtensions.GetDescription(TipoSaldoGlobalEnum.GranTotal),
                descripcion = "GRAN TOTAL DISPONIBLE",
                global = sumGranTotal
            });
            #endregion
            return lstSaldosGlobales;
        }
        SaldoGlobalDTO getSaldoRestante(BusqConcentradoDTO busq)
        {
            var lstDiv = new List<int>();
            if(busq.esDivIndustrial)
            {
                lstDiv.Add((int)TipoCCEnum.ConstruccionPesada);
                lstDiv.Add((int)TipoCCEnum.Administración);
                lstDiv.Add((int)TipoCCEnum.ObraCerradaGeneral);
            }
            else
            {
                lstDiv.Add((int)TipoCCEnum.Industrial);
                lstDiv.Add((int)TipoCCEnum.ObraCerradaIndustrial);
            }
            busq.lstCC = lstCC.Where(c => lstDiv.Any(i => c.bit_area.ParseInt().Equals(i))).Select(c => c.cc).ToList();
            var lst = saldoConciliadoToConcentrado(busq);
            var sum = lst.Sum(s => s.cargo - s.abono);
            var obj = new SaldoGlobalDTO()
            {
                clase = EnumExtensions.GetDescription(TipoSaldoGlobalEnum.Saldo),
                descripcion = string.Format("SALDO AREA {0}", busq.esDivIndustrial ? "GENERAL" : "INDUSTRIAL"),
                saldo = sum
            };
            return obj;
        }
        #endregion
        #region Resumen de Nomina
        public ActionResult NominaPoliza()
        {
            return View();
        }
        public ActionResult NominaResumen()
        {
            return View();
        }

        [HttpPost]
        public ActionResult getLstPolizasNominas(DateTime fecha_inicial, DateTime fecha_final)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lst = nominaFS.getNominaServices().getLstPolizaNomina(fecha_inicial, fecha_final).Where(pol => pol.sscta > 0).ToList();
                fecha_inicial = fecha_inicial.AddDays(-6);
                var lstActiva = nominaFS.getNominaServices().getLstPolizaNominaActiva(fecha_inicial, fecha_final);
                var lstCcArren = ccFS.getCentroCostosService().getLstCcArrendadoraProd();
                var lstCta = EnumExtensions.ToCombo<tipoCuentaNominaEnum>();
                var tbl = from g in lst
                          group g by new { g.year, g.mes, g.tp, g.poliza, g.cc, g.linea, g.sscta } into pol
                          orderby pol.FirstOrDefault().fechapol, pol.Key.poliza, pol.Key.cc
                          select new
                          {
                              ccDesc = (pol.Key.sscta.Equals((int)tipoCuentaNominaEnum.Arrendadora) || pol.Key.sscta.Equals((int)tipoCuentaNominaEnum.TRONSET))? lstCcArren.FirstOrDefault(w => w.Value.Equals(pol.Key.cc)).Text.Split('-')[1] : lstCC.FirstOrDefault(c => c.cc.Equals(pol.Key.cc)).descripcion.ToUpper(),
                              cc = pol.Key.cc,
                              fechapol = pol.FirstOrDefault().fechapol.ToShortDateString(),
                              year = pol.FirstOrDefault().fechapol.Year,
                              mes = pol.FirstOrDefault().fechapol.Month,
                              concepto = pol.Key.sscta == 2710 ? string.Format("{0} {1}", "Finiquito Ref. ", pol.FirstOrDefault().referencia) : string.Format("{0} {1}", pol.FirstOrDefault().concepto, pol.FirstOrDefault().referencia),
                              poliza = pol.Key.poliza,
                              cargo = 0,
                              //cargo = getCargo(pol.ToList()),
                              abono = getAbono(pol.ToList()),
                              //iva = getIva(pol.ToList()),
                              iva = 0,
                              retencion = getRetencion(pol.ToList()),
                              tipoNomina = lstActiva.Any(act => act.year == pol.Key.year && act.mes == pol.Key.mes && act.poliza == pol.Key.poliza) ? lstActiva.FirstOrDefault(act => act.year == pol.Key.year && act.mes == pol.Key.mes && act.poliza == pol.Key.poliza).tipoNomina : (int)tipoNominaPropuestaEnum.NA,
                              tipoCuenta = pol.Key.sscta,
                              tipoCuentaNombre = lstCta.FirstOrDefault(c => c.Value.Equals(pol.Key.sscta)) != null ? lstCta.FirstOrDefault(c => c.Value.Equals(pol.Key.sscta)).Text : "N/A"
                          };
                tbl = tbl.OrderBy(x => x.fechapol).ThenBy(x => x.poliza).ThenBy(x => x.tipoCuentaNombre).ToList();
                result.Add("polizas", tbl);
                result.Add(SUCCESS, tbl.Any());
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        decimal getCargo(List<NominaPolizaDTO> pol)
        {
            var tipoCuenta = (tipoCuentaNominaEnum)pol.FirstOrDefault().sscta;
            var cargo = 0m;
            switch(tipoCuenta)
            {
                case tipoCuentaNominaEnum.Arrendadora:
                case tipoCuentaNominaEnum.TRONSET:
                    cargo = pol.Where(w => w.tm == 1 || w.tm == 3).Sum(s => (decimal)(s.monto));
                    cargo -= getIva(pol);
                    break;
                default:
                    cargo = pol.Where(w => w.tm == 1 || w.tm == 3).Sum(s => s.monto);
                    break;
            }
            if (cargo >= 157779 && cargo < 157780) {
                var a = 0;
            }
            return cargo;
        }
        decimal getAbono(List<NominaPolizaDTO> pol)
        {
            var tipoCuenta = (tipoCuentaNominaEnum)pol.FirstOrDefault().sscta;
            var abono = 0m;
            switch(tipoCuenta)
            {
                //case tipoCuentaNominaEnum.TRONSET:
                //    abono = pol.Where(w => w.tm == 2 || w.tm == 4).Sum(s => (decimal)(s.monto + s.retencion));
                //    break;
                default:
                    abono = pol.Where(w => w.tm == 2 || w.tm == 4).Sum(s => s.monto);
                    break;
            }
            return abono;
        }
        decimal getIva(List<NominaPolizaDTO> pol)
        {
            var tipoCuenta = (tipoCuentaNominaEnum)pol.FirstOrDefault().sscta;
            var iva = 0m;
            switch(tipoCuenta)
            {
                case tipoCuentaNominaEnum.Arrendadora:
                case tipoCuentaNominaEnum.TRONSET:
                    var total = pol.Sum(s => s.tm == 2 || s.tm == 4 ? (decimal)((-s.monto) - s.retencion) : (decimal)(s.monto + s.retencion));
                    var sub = (total / 1.16m);
                    iva = total - sub;
                    break;
                case tipoCuentaNominaEnum.CONSTRUPLAN:
                    iva = 0;
                    break;
                default:
                    iva = (pol.Sum(s => s.tm == 2 || s.tm == 4 ? -s.monto : s.monto) * .16m);
                    break;
            }
            return iva;
        }
        decimal getRetencion(List<NominaPolizaDTO> pol)
        {
            var retencion = -pol.LastOrDefault().retencion.GetValueOrDefault();       
            return retencion;
        }
        [HttpPost]
        public ActionResult setNominaPolizaSession(List<tblC_NominaPoliza> relCtas)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstSession = (List<tblC_NominaPoliza>)Session["listaNominaPoliza"];
                if(lstSession == null)
                {
                    lstSession = new List<tblC_NominaPoliza>();
                }
                lstSession.AddRange(relCtas);
                Session["listaNominaPoliza"] = lstSession;
                var esSession = lstSession.Any();
                result.Add(SUCCESS, esSession);
                if(!esSession)
                {
                    Session["listaNominaPoliza"] = null;
                }
            }
            catch(Exception o_O)
            {
                Session["listaNominaPoliza"] = null;
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, o_O.Message);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult guardarNominaPoliza()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var esGuardado = false;
                var listaNominaPoliza = (List<tblC_NominaPoliza>)Session["listaNominaPoliza"];
                if(listaNominaPoliza.All(pol => pol.year > 2000 && pol.mes > 0 && pol.poliza > 0 && pol.tipoCuenta > 0 && pol.cc.Length > 0))
                {
                    var periodos = nominaFS.getNominaServices().getLstPeriodoNomina();
                    listaNominaPoliza.Where(w => w.tipoNomina.Equals((int)tipoNominaPropuestaEnum.Semanal)).ToList().ForEach(nomina =>
                        {
                            nomina.fecha = nomina.fecha.AddDays(-6);
                        });
                    esGuardado = nominaFS.getNominaServices().guardarNominaPoliza(listaNominaPoliza);
                }
                result.Add(SUCCESS, esGuardado);
            }
            catch(Exception e)
            {
                Session["listaNominaPoliza"] = null;
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetNominasQuincenalesSemanales(DateTime fechaInicio, DateTime fechaFin, tipoNominaPropuestaEnum tipoNomina)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var polizas = nominaFS.getNominaServices().getLstPolizaNominaActiva(fechaInicio, fechaFin).Where(nomina => nomina.tipoNomina == (int)tipoNomina).ToList();
                var resumenes = nominaFS.getNominaServices().getLstResumenNominaActiva(fechaInicio, fechaFin, (int)tipoNomina);
                var lstResumen = getResumenCplan(polizas, resumenes, fechaInicio, fechaFin);
                lstResumen.AddRange(getResumenArrendadora(polizas, resumenes, fechaInicio, fechaFin));
                lstResumen.Add(new NominaResumenDTO(lstResumen, fechaInicio, fechaFin));
                resultado.Add(SUCCESS, lstResumen.Any());
                resultado.Add("listaNominas", lstResumen);
            }
            catch(Exception o_O)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, o_O.Message);
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        List<NominaResumenDTO> getResumenCplan(List<tblC_NominaPoliza> lstPoliza, List<tblC_NominaResumen> lstResumen, DateTime fechaInicio, DateTime fechaFin)
        {
            var lst = new List<NominaResumenDTO>();
            var vacio = new NominaResumenDTO
                        {
                            cc = "ZZZ",
                            descripcion = string.Empty,
                            clase = "vacio"
                        };
            var lstCuentas = new List<tipoCuentaNominaEnum>
                {
                    tipoCuentaNominaEnum.ConstructoraRavelio,
                    tipoCuentaNominaEnum.SONMONT,
                    tipoCuentaNominaEnum.REGFORTE,
                    tipoCuentaNominaEnum.CONSTRUPLAN,
                };
            var lstDivisionCC = CplanDivisionesCC();
            var lstDivisionesOmitir = new List<TipoCCEnum>
            {
                TipoCCEnum.Administración,
                TipoCCEnum.GastosFininacierosYOtros,
                TipoCCEnum.ObraCerradaGeneral,
                TipoCCEnum.Industrial,
                TipoCCEnum.ObraCerradaIndustrial,  
                TipoCCEnum.AlimentosYBebidas,
                TipoCCEnum.Automotriz,
                TipoCCEnum.Energía,
            };
            var listaObras = cadenaFS.getCadenaProductivaService().lstObra().OrderBy(o => o.cc.Trim());
            lstCuentas.ForEach(cuenta =>
            {
                var lstNomina = new List<NominaResumenDTO>();
                lstDivisionCC.ForEach(division =>
                    {
                        var obra = Enumerable.Empty<string>();
                        var polizas = Enumerable.Empty<tblC_NominaPoliza>();
                        var resumen = Enumerable.Empty<tblC_NominaResumen>();
                        var existDivision = EnumExtensions.Any<TipoCCEnum>(division);
                        var lstDivisionBusq = new List<int>() { (int)division };
                        switch(division)
                        {
                            case TipoCCEnum.Administración:
                                lstDivisionBusq.Add((int)TipoCCEnum.GastosFininacierosYOtros);
                                break;
                            case TipoCCEnum.Industrial:
                                lstDivisionBusq.Add((int)TipoCCEnum.AlimentosYBebidas);
                                lstDivisionBusq.Add((int)TipoCCEnum.Automotriz);
                                lstDivisionBusq.Add((int)TipoCCEnum.Energía);
                                break;
                            default:
                                break;
                        }
                        if(existDivision)
                        {
                            obra = from cc in listaObras
                                   where lstDivisionBusq.Contains(cc.bit_area.ParseInt())
                                   select cc.cc;
                        }
                        else
                        {
                            obra = from cc in listaObras
                                   where !lstDivisionesOmitir.Select(s => s.ParseInt()).Contains(cc.bit_area.ParseInt())
                                   select cc.cc;
                        }
                        var lstCuentaBusq = new List<int>() { (int)cuenta };
                        if(cuenta == tipoCuentaNominaEnum.CONSTRUPLAN)
                        {
                            lstCuentaBusq.Add((int)tipoCuentaNominaEnum.ProvisionServicioAdministrativos);
                            lstCuentaBusq.Add((int)tipoCuentaNominaEnum.ServiciosAdministrativosComplementaria);
                        }
                        polizas = from pol in lstPoliza
                                  where lstCuentaBusq.Contains(pol.tipoCuenta) && obra.Contains(pol.cc)
                                  select pol;
                        switch(division)
                        {
                            case TipoCCEnum.Administración:
                            case TipoCCEnum.Industrial:
                            case 0:
                                resumen = from res in lstResumen
                                          where lstCuentaBusq.Contains(res.tipoCuenta) && obra.Contains(res.cc)
                                          select res;
                                var esUltimoDiv = division == 0;
                                var clase = "cplan " + (existDivision ? "normal" : "obraIndividual");
                                if(polizas.Any())
                                {
                                    lstNomina.Add(new NominaResumenDTO
                                    {
                                        cc = "C.C.",
                                        descripcion = "AREA " + (existDivision ? division.GetDescription() : "Individual").ToUpper(),
                                        clase = "encabezadoTabla",
                                        descripcionCuenta = cuenta.GetDescription(),
                                    });
                                }
                                lstNomina.AddRange((from nomina in polizas
                                                    group nomina by nomina.cc into nomina
                                                    select new NominaResumenDTO
                                                    {
                                                        id = resumen.Any(r => r.tipoCuenta == (int)cuenta && r.cc == nomina.Key) ? resumen.FirstOrDefault(r => r.tipoCuenta == (int)cuenta && r.cc == nomina.Key).id : 0,
                                                        cc = nomina.Key,
                                                        descripcion = listaObras.FirstOrDefault(c => c.cc == nomina.Key).descripcion,
                                                        nomina = nomina.Sum(s => s.cargo - s.abono),
                                                        iva = nomina.Sum(s => s.iva),
                                                        retencion = nomina.Sum(s => s.retencion),
                                                        total = nomina.Sum(s => s.cargo - s.abono + s.iva - s.retencion),
                                                        noEmpleado = resumen.Where(w => w.cc == nomina.Key).Sum(s => s.noEmpleado),
                                                        noPracticante = resumen.Where(w => w.cc == nomina.Key).Sum(s => s.noPracticante),
                                                        clase = clase,
                                                        tipoCuenta = cuenta,
                                                        fecha_inicial = fechaInicio,
                                                        fecha_final = fechaFin,
                                                        tipoNomina = (tipoNominaPropuestaEnum)nomina.FirstOrDefault().tipoNomina,
                                                        division = division.GetDescription()
                                                    }).ToList());
                                var lstNominaDiv = from cuentaDiv in lstNomina
                                                   where cuentaDiv.tipoCuenta == cuenta && cuentaDiv.division == division.GetDescription()
                                                   select cuentaDiv;
                                if(lstNominaDiv.Any())
                                {
                                    lstNomina.Add(new NominaResumenDTO
                                    {
                                        cc = "ZZZ",
                                        descripcion = "TOTAL",
                                        nomina = lstNominaDiv.Sum(s => s.nomina),
                                        iva = lstNominaDiv.Sum(s => s.iva),
                                        retencion = lstNominaDiv.Sum(s => s.retencion),
                                        total = lstNominaDiv.Sum(s => s.total),
                                        noEmpleado = lstNominaDiv.Sum(s => s.noEmpleado),
                                        noPracticante = lstNominaDiv.Sum(s => s.noPracticante),
                                        tipoCuenta = cuenta,
                                        clase = "cplan totalCuadro",
                                        division = division.GetDescription()
                                    });
                                    lstNomina.Add(vacio);
                                }
                                if(esUltimoDiv)
                                {
                                    if(lstNomina.Any())
                                    {
                                        lstNomina.Insert(0, new NominaResumenDTO
                                        {
                                            cc = "ZZ",
                                            descripcion = cuenta.GetDescription().ToUpper(),
                                            clase = "encabezadoEmpresa " + cuenta.GetDescription().ToUpper(),
                                            descripcionCuenta = cuenta.GetDescription(),
                                        });
                                    }
                                    var lstNominaPolizas = from pol in lstPoliza
                                                           where lstCuentaBusq.Contains(pol.tipoCuenta)
                                                           select pol;
                                    var lstNominaCuenta = from cuentaDiv in lstResumen
                                                          where cuentaDiv.tipoCuenta == (int)cuenta
                                                          select cuentaDiv;
                                    lstNomina.Add(new NominaResumenDTO
                                    {
                                        cc = "ZZZ",
                                        descripcion = "TOTAL TRANSFERENCIA A " + cuenta.GetDescription().ToUpper(),
                                        nomina = lstNominaPolizas.Sum(s => s.cargo - s.abono),
                                        iva = lstNominaPolizas.Sum(s => s.iva),
                                        retencion = lstNominaPolizas.Sum(s => s.retencion),
                                        total = lstNominaPolizas.Sum(s => s.cargo - s.abono + s.iva - s.retencion),
                                        noEmpleado = lstNominaCuenta.Sum(s => s.noEmpleado),
                                        noPracticante = lstNominaCuenta.Sum(s => s.noPracticante),
                                        tipoCuenta = cuenta,
                                        clase = string.Format("cplan totalCuenta {0} {1}", cuenta.GetDescription().ToUpper(), division.GetDescription()),
                                        division = division.GetDescription()
                                    });
                                    lstNomina.Add(vacio);
                                }
                                break;
                            case TipoCCEnum.ObraCerradaGeneral:
                            case TipoCCEnum.ObraCerradaIndustrial:
                                if(polizas.Any())
                                {
                                    var ccCerrada = "0" + division.ParseInt();
                                    resumen = from res in lstResumen
                                              where lstCuentaBusq.Contains(res.tipoCuenta) && ccCerrada == res.cc
                                              select res;
                                    var divDesc = division.GetDescription().Replace(" ", string.Empty);
                                    lstNomina.Add(new NominaResumenDTO
                                    {
                                        cc = "C.C.",
                                        descripcion = "AREA " + (existDivision ? division.GetDescription() : "Individual").ToUpper(),
                                        clase = "encabezadoTabla",
                                        descripcionCuenta = cuenta.GetDescription(),
                                    });
                                    lstNomina.Add(new NominaResumenDTO
                                    {
                                        id = resumen.Any() ? resumen.FirstOrDefault().id : 0,
                                        cc = ccCerrada,
                                        descripcion = "OBRA CERRADA",
                                        nomina = polizas.Sum(s => s.cargo - s.abono),
                                        iva = polizas.Sum(s => s.iva),
                                        retencion = polizas.Sum(s => s.retencion),
                                        total = polizas.Sum(s => s.cargo - s.abono + s.iva - s.retencion),
                                        noEmpleado = resumen.Sum(s => s.noEmpleado),
                                        noPracticante = resumen.Sum(s => s.noPracticante),
                                        clase = "cplan normal",
                                        tipoCuenta = cuenta,
                                        fecha_inicial = fechaInicio,
                                        fecha_final = fechaFin,
                                        tipoNomina = (tipoNominaPropuestaEnum)polizas.FirstOrDefault().tipoNomina,
                                        division = divDesc
                                    });
                                    lstNomina.Add(new NominaResumenDTO
                                    {
                                        cc = "ZZZ",
                                        descripcion = "TOTAL",
                                        nomina = polizas.Sum(s => s.cargo - s.abono),
                                        iva = polizas.Sum(s => s.iva),
                                        retencion = polizas.Sum(s => s.retencion),
                                        total = polizas.Sum(s => s.cargo - s.abono + s.iva - s.retencion),
                                        noEmpleado = resumen.Sum(s => s.noEmpleado),
                                        noPracticante = resumen.Sum(s => s.noPracticante),
                                        tipoCuenta = cuenta,
                                        clase = "cplan totalCuadro",
                                        division = divDesc
                                    });
                                    lstNomina.Add(vacio);
                                }
                                break;
                            default:
                                break;
                        }
                    });
                lst.AddRange(lstNomina);
            });
            return lst;
        }
        List<NominaResumenDTO> getResumenArrendadora(List<tblC_NominaPoliza> lstPoliza, List<tblC_NominaResumen> lstResumen, DateTime fechaInicio, DateTime fechaFin)
        {
            var lst = new List<NominaResumenDTO>();
            var obrasArrendadora = ccFS.getCentroCostosService().getLstCcArrendadoraProd().OrderBy(o => o.Value);
            var lstCuentas = new List<tipoCuentaNominaEnum>()
                {
                    tipoCuentaNominaEnum.TRONSET,
                    tipoCuentaNominaEnum.Arrendadora
                };
            lstCuentas.ForEach(cuenta =>
                {
                    var lstCuentaBusq = new List<int>() { (int)cuenta };
                    var polizas = from pol in lstPoliza
                                  where lstCuentaBusq.Contains(pol.tipoCuenta) && obrasArrendadora.Any(a => a.Value == pol.cc)
                                  select pol;
                    var resumen = from res in lstResumen
                                  where lstCuentaBusq.Contains(res.tipoCuenta) && obrasArrendadora.Any(a => a.Value == res.cc)
                                  select res;
                    if(polizas.Any())
                    {
                        lst.Add(new NominaResumenDTO
                        {
                            cc = "ZZ",
                            descripcion = cuenta.GetDescription().ToUpper(),
                            clase = "encabezadoEmpresa " + cuenta.GetDescription().ToUpper(),
                            descripcionCuenta = cuenta.GetDescription(),
                        });
                        lst.Add(new NominaResumenDTO
                        {
                            cc = "C.C.",
                            descripcion = string.Empty,
                            clase = "encabezadoTabla",
                            descripcionCuenta = cuenta.GetDescription(),
                            descripcionNomina = ((tipoNominaPropuestaEnum)(polizas.FirstOrDefault().tipoNomina)).GetDescription()
                        });
                        lst.AddRange((from nomina in polizas
                                      group nomina by nomina.cc into nomina
                                      select new NominaResumenDTO
                                      {
                                          id = resumen.Any(r => r.tipoCuenta == (int)cuenta && r.cc == nomina.Key) ? resumen.FirstOrDefault(r => r.tipoCuenta == (int)cuenta && r.cc == nomina.Key).id : 0,
                                          cc = nomina.Key,
                                          descripcion = obrasArrendadora.FirstOrDefault(c => c.Value == nomina.Key).Text.Split('-')[1],
                                          nomina = nomina.Sum(s => s.cargo - s.abono),
                                          iva = nomina.Sum(s => s.iva),
                                          retencion = nomina.Sum(s => s.retencion),
                                          total = nomina.Sum(s => s.cargo - s.abono + s.iva - s.retencion),
                                          noEmpleado = resumen.Where(w => w.cc == nomina.Key).Sum(s => s.noEmpleado),
                                          noPracticante = resumen.Where(w => w.cc == nomina.Key).Sum(s => s.noPracticante),
                                          clase = "arrend normal",
                                          tipoCuenta = cuenta,
                                          fecha_inicial = fechaInicio,
                                          fecha_final = fechaFin,
                                          tipoNomina = (tipoNominaPropuestaEnum)nomina.FirstOrDefault().tipoNomina,
                                          division = "0 " + cuenta.GetDescription()
                                      }).ToList());
                    }
                    var lstResumenCuenta = from nomina in lst
                                           where nomina.tipoCuenta == cuenta
                                           select nomina;
                    lst.Add(new NominaResumenDTO
                    {
                        cc = "ZZZ",
                        descripcion = "TOTAL TRANSFERENCIA A " + cuenta.GetDescription().ToUpper(),
                        nomina = lstResumenCuenta.Sum(s => s.nomina),
                        iva = lstResumenCuenta.Sum(s => s.iva),
                        retencion = lstResumenCuenta.Sum(s => s.retencion),
                        total = lstResumenCuenta.Sum(s => s.total),
                        noEmpleado = lstResumenCuenta.Sum(s => s.noEmpleado),
                        noPracticante = lstResumenCuenta.Sum(s => s.noPracticante),
                        tipoCuenta = cuenta,
                        clase = "arrend totalCuenta " + cuenta.GetDescription().ToUpper(),
                        division = "0 " + cuenta.GetDescription()
                    });
                    lst.Add(new NominaResumenDTO
                    {
                        cc = "ZZZ",
                        descripcion = string.Empty,
                        clase = "vacio"
                    });
                });
            var lstTotalCuadro = from clase in lst
                                 where clase.clase.Contains("totalCuadro")
                                 select clase;
            return lst;
        }
        [HttpPost]
        public ActionResult GetNominasOtros(DateTime fechaInicio, DateTime fechaFin)
        {
            var resultado = nominaFS.getNominaServices().GetNominasOtros(fechaInicio, fechaFin);
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult guardarNominaResumen(List<tblC_NominaResumen> listaNominaResumen, bool otros)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var esGuardado = listaNominaResumen.Count > 0 && listaNominaResumen.All(nom =>
                        nom.cc.Length > 0 &&
                        (otros ? true : (nom.noEmpleado > 0 || nom.noPracticante > 0)) &&
                        nom.tipoCuenta > 0);
                if(esGuardado)
                {
                    esGuardado = nominaFS.getNominaServices().guardarNominaResumen(listaNominaResumen);
                }
                result.Add(SUCCESS, esGuardado);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Resumen Estimaciones
        public ViewResult EstimacionesCaptura()
        {
            return View();
        }
        public PartialViewResult _tblEstimacionesCaptura()
        {
            return PartialView();
        }
        public ViewResult EstimacionesResumen()
        {
            return View();
        }
        public ActionResult getLstFacturasEstimadas(DateTime fechaInicial, DateTime fechaFinal)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var auth = facturaFS.getFacturaService().getAuthResumenEstimacion(fechaInicial, fechaFinal);
                var lstFactura = facturaFS.getFacturaService().GetAnaliticoClientes(fechaFinal);
                var lstSigoplan = facturaFS.getFacturaService().getAlltEstimadoo(fechaInicial, fechaFinal);
                lstFactura.AddRange(lstSigoplan);
                var lst = lstFactura
                    .GroupBy(o => new { cc = o.cc.Trim(), numcte = o.numcte.ParseInt(), factura = o.factura.ParseInt() })
                    .OrderBy(g => g.Key.cc).ThenBy(g => g.Key.numcte).ThenBy(g => g.Key.factura)
                    .Select(s => s.OrderByDescending(o => o.id).ToList().FirstOrDefault())
                    .Where(w => w.esActivo)
                    .ToList();
                result.Add("stAuth", auth.stAuth);
                result.Add("lst", lst);
                result.Add(SUCCESS, lst.Count > 0);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }
        public ActionResult GetAnaliticoClientes(DateTime fechaInicial, DateTime fechaFinal)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lst = new List<EstClieFacturaDTO>();
                var auth = facturaFS.getFacturaService().getAuthResumenEstimacion(fechaInicial, fechaFinal);
                var facturas = facturaFS.getFacturaService().getlstEstimadoActivo(fechaInicial, fechaFinal)
                    .Select(fact => new EstClieFacturaDTO()
                    {
                        cc = fact.cc,
                        numcte = fact.numcte,
                        factura = fact.factura,
                        fecha = fact.fecha,
                        fechavenc = fact.fechavenc,
                        estimacion = fact.estimacion,
                        anticipo = fact.anticipo,
                        vencido = fact.vencido,
                        cobrado = fact.cobrado,
                        pronostico = fact.pronostico,
                        linea = fact.linea,
                        clase = "normal"
                    }).ToList()
                    .OrderBy(o => o.numcte).ThenBy(o => o.cc).ToList();
                var lstCliente = facturaFS.getFacturaService().getLstCliente();
                var i = 0;
                var lstInds = new List<int>()
                {
                    (int)TipoCCEnum.Mineria,
                    (int)TipoCCEnum.Industrial,
                    (int)TipoCCEnum.AlimentosYBebidas,
                    (int)TipoCCEnum.Automotriz,
                    (int)TipoCCEnum.Energía,
                    (int)TipoCCEnum.ObraCerradaIndustrial,
                };
                var lstFacuraClientes = facturas.Where(w => lstCC.Any(c => c.cc.Equals(w.cc) && !lstInds.Contains(c.bit_area.ParseInt()))).GroupBy(g => g.numcte);
                var lstFacturaObra = facturas.Where(w => lstCC.Any(c => c.cc.Equals(w.cc) && lstInds.Contains(c.bit_area.ParseInt()))).GroupBy(g => new
                {
                    g.cc,
                    division = lstCC.FirstOrDefault(c => c.cc == g.cc).bit_area.ParseInt()
                }).Select(e => new
                {
                    cc = e.Key.cc,
                    division = lstCC.FirstOrDefault(c => c.cc == e.Key.cc).bit_area.ParseInt(),
                    lstFacturas = e
                });
                lstFacuraClientes.ToList().ForEach(c =>
                    {
                        var cliente = lstCliente.FirstOrDefault(cli => cli.numcte == c.Key.ParseInt()).nombre;
                        lst.Add(new EstClieFacturaDTO()
                        {
                            numcte = c.Key,
                            no = ++i,
                            descripcion = cliente,
                            estimacion = c.Sum(s => s.estimacion),
                            anticipo = c.Sum(s => s.anticipo),
                            vencido = c.Sum(s => s.vencido),
                            cobrado = c.Sum(s => s.cobrado),
                            pronostico = c.Sum(s => s.pronostico),
                            clase = "suma",
                        });
                        c.ToList().ForEach(f =>
                        {
                            f.no = ++i;
                            f.descripcion = string.Format("{0} {1}", lstCC.FirstOrDefault(x => x.cc.Equals(f.cc)).descripcion, f.factura);
                            f.grupo = string.Format("{0}: {1}", c.Key, cliente);
                            lst.Add(f);
                        });
                    });
                lst.Add(new EstClieFacturaDTO()
                {
                    no = ++i,
                    descripcion = "SUBTOTAL",
                    estimacion = lstFacuraClientes.Sum(s => s.Sum(ss => ss.estimacion)),
                    anticipo = lstFacuraClientes.Sum(s => s.Sum(ss => ss.anticipo)),
                    vencido = lstFacuraClientes.Sum(s => s.Sum(ss => ss.vencido)),
                    cobrado = lstFacuraClientes.Sum(s => s.Sum(ss => ss.cobrado)),
                    pronostico = lstFacuraClientes.Sum(s => s.Sum(ss => ss.pronostico)),
                    clase = "subtotal",
                });
                lstInds.ForEach(inds =>
                {
                    lst.Add(new EstClieFacturaDTO()
                    {
                        no = ++i,
                        descripcion = EnumExtensions.GetDescription((TipoCCEnum)inds).ToUpper(),
                        clase = "encabezado",
                    });
                    var lstFacturaIndustrial = lstFacturaObra.Where(w => inds == w.division);
                    lstFacturaIndustrial.ToList().ForEach(c =>
                    {
                        var cc = lstCC.FirstOrDefault(x => x.cc.Equals(c.cc));
                        c.lstFacturas.GroupBy(f => f.numcte).ToList().ForEach(f =>
                        {
                            var cliente = lstCliente.FirstOrDefault(cli => cli.numcte == f.Key.ParseInt()).nombre;
                            lst.Add(new EstClieFacturaDTO()
                            {
                                no = ++i,
                                descripcion = cliente,
                                numcte = f.Key,
                                grupo = cc.cc + "-" + cc.descripcion,
                                grupoCC = EnumExtensions.GetDescription((TipoCCEnum)inds),
                                cc = cc.cc,
                                estimacion = f.Sum(s => s.estimacion),
                                anticipo = f.Sum(s => s.anticipo),
                                vencido = f.Sum(s => s.vencido),
                                pronostico = f.Sum(s => s.pronostico),
                                cobrado = f.Sum(s => s.cobrado),
                                clase = "normal"
                            });

                            foreach (var item in f)
                            {
                                lst.Add(new EstClieFacturaDTO()
                                {
                                    no = ++i,
                                    descripcion = cliente,
                                    numcte = item.factura,
                                    grupo = cc.cc + "-" + cc.descripcion,
                                    grupoCC = EnumExtensions.GetDescription((TipoCCEnum)inds),
                                    cc = cc.cc,
                                    estimacion = item.estimacion,
                                    anticipo = item.anticipo,
                                    vencido = item.vencido,
                                    pronostico = item.pronostico,
                                    cobrado = item.cobrado,
                                    clase = "normal"
                                });
                            }
                        });
                    });
                    lst.Add(new EstClieFacturaDTO()
                    {
                        no = ++i,
                        descripcion = "SUBTOTAL",
                        estimacion = lstFacturaIndustrial.Sum(s => s.lstFacturas.Sum(ss => ss.estimacion)),
                        anticipo = lstFacturaIndustrial.Sum(s => s.lstFacturas.Sum(ss => ss.anticipo)),
                        vencido = lstFacturaIndustrial.Sum(s => s.lstFacturas.Sum(ss => ss.vencido)),
                        cobrado = lstFacturaIndustrial.Sum(s => s.lstFacturas.Sum(ss => ss.cobrado)),
                        pronostico = lstFacturaIndustrial.Sum(s => s.lstFacturas.Sum(ss => ss.pronostico)),
                        clase = "subtotal",
                    });
                });
                lst.Add(new EstClieFacturaDTO()
                {
                    no = ++i,
                    descripcion = "SUBTOTAL",
                    estimacion = lstFacturaObra.Sum(s => s.lstFacturas.Sum(ss => ss.estimacion)),
                    anticipo = lstFacturaObra.Sum(s => s.lstFacturas.Sum(ss => ss.anticipo)),
                    vencido = lstFacturaObra.Sum(s => s.lstFacturas.Sum(ss => ss.vencido)),
                    cobrado = lstFacturaObra.Sum(s => s.lstFacturas.Sum(ss => ss.cobrado)),
                    pronostico = lstFacturaObra.Sum(s => s.lstFacturas.Sum(ss => ss.pronostico)),
                    clase = "subtotal",
                });
                lst.Add(new EstClieFacturaDTO()
                {
                    no = ++i,
                    descripcion = "GRAN TOTAL",
                    estimacion = facturas.Sum(s => s.estimacion),
                    anticipo = facturas.Sum(s => s.anticipo),
                    vencido = facturas.Sum(s => s.vencido),
                    cobrado = facturas.Sum(s => s.cobrado),
                    pronostico = facturas.Sum(s => s.pronostico),
                    clase = "subtotal",
                });
                Session["lstEstimacionResumen"] = lst;
                Session["EstfechaFinal"] = fechaFinal;
                Session["stAuth"] = ((stAuthEnum)auth.stAuth).GetDescription();
                result.Add("facturas", lst);
                result.Add("auth", auth);
                result.Add("stAuth", ((stAuthEnum)auth.stAuth).GetDescription());
                result.Add(SUCCESS, lst.Count > 0);
            }
            catch(Exception e)
            {
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAnaliticoClientesCXC(DateTime fechaInicial, DateTime fechaFinal)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lst = new List<EstClieFacturaDTO>();

                Dictionary<string,object> dictAuth = cxcFS.getCuentasPorCobrarService().VerificarCXC(fechaInicial, fechaFinal);
                bool? esAuth = dictAuth["esAuth"] as bool?;

                var facturas = new List<EstClieFacturaDTO>();

                if (esAuth.Value)
                {
                    List<tblCXC_CuentasPorCobrar> facturasSave = dictAuth["items"] as List<tblCXC_CuentasPorCobrar>;
                    facturas = facturasSave.Select(fact => new EstClieFacturaDTO()
                    {
                        cc = fact.cc,
                        numcte = fact.numcte.ToString(),
                        nombreCliente = fact.nombreCliente,
                        factura = fact.factura.ToString(),
                        //fecha = fact.fechaOrig,
                        //fechavenc = fact.fechaVenc,
                        estimacion = 0,
                        anticipo = 0,
                        vencido = fact.total,
                        cobrado = 0,
                        pronostico = fact.pronosticado,
                        linea = "",
                        clase = "normal"
                    }).ToList()
                        .OrderBy(o => o.numcte).ThenBy(o => o.cc).ToList(); ;
                }
                else
                {
                    var lstFacturas = rentabilidadFS.getRentabilidadDAO().getLstCXC(new BusqKubrixDTO() { fechaFin = fechaInicial }).Where(e => e.esCorte && e.montoPronosticado > 0).ToList();
                    facturas = lstFacturas.Where(e => e.fechaCorte.Date >= fechaInicial.Date && e.fechaCorte.Date <= fechaFinal.Date)
                        .Select(fact => new EstClieFacturaDTO()
                        {
                            cc = fact.areaCuenta,
                            numcte = fact.numcte,
                            nombreCliente = fact.responsable,
                            factura = fact.factura.ToString(),
                            fecha = fact.fecha,
                            fechavenc = fact.fechaCorte,
                            estimacion = 0,
                            anticipo = 0,
                            vencido = fact.monto,
                            cobrado = 0,
                            pronostico = fact.montoPronosticado,
                            linea = fact.concepto,
                            clase = "normal"
                        }).ToList()
                        .OrderBy(o => o.numcte).ThenBy(o => o.cc).ToList();
                }

                //var auth = facturaFS.getFacturaService().getAuthResumenEstimacion(fechaInicial, fechaFinal);

                //var lstCliente = facturaFS.getFacturaService().getLstCliente();
                var i = 0;
                var lstInds = new List<int>()
                {
                    (int)TipoCCEnum.Mineria,
                    //(int)TipoCCEnum.Industrial,
                    (int)TipoCCEnum.ConstruccionPesada,
                    (int)TipoCCEnum.Administración,
                    (int)TipoCCEnum.GastosFininacierosYOtros,
                    (int)TipoCCEnum.AlimentosYBebidas,
                    (int)TipoCCEnum.Automotriz,
                    (int)TipoCCEnum.Energía,
                    //(int)TipoCCEnum.ObraCerradaIndustrial,
                };
                var lstFacuraClientes = facturas.Where(w => lstCC.Any(c => c.cc.Equals(w.cc) && !lstInds.Contains(c.bit_area.ParseInt()))).GroupBy(g => g.nombreCliente);
                var lstFacturaObra = facturas.Where(w => lstCC.Any(c => c.cc.Equals(w.cc) && lstInds.Contains(c.bit_area.ParseInt()))).GroupBy(g => new
                {
                    g.cc,
                    division = lstCC.FirstOrDefault(c => c.cc == g.cc).bit_area.ParseInt()
                }).Select(e => new
                {
                    cc = e.Key.cc,
                    division = lstCC.FirstOrDefault(c => c.cc == e.Key.cc).bit_area.ParseInt(),
                    lstFacturas = e
                });

                lstFacuraClientes.ToList().ForEach(c =>
                {
                    //var cliente = lstCliente.FirstOrDefault(cli => cli.numcte == c.Key.ParseInt()).nombre;
                    lst.Add(new EstClieFacturaDTO()
                    {
                        numcte = c.Key,
                        no = ++i,
                        descripcion = c.Key,
                        estimacion = c.Sum(s => s.estimacion),
                        anticipo = c.Sum(s => s.anticipo),
                        vencido = c.Sum(s => s.vencido),
                        cobrado = c.Sum(s => s.cobrado),
                        pronostico = c.Sum(s => s.pronostico),
                        clase = "suma",
                    });
                    c.ToList().ForEach(f =>
                    {
                        f.no = ++i;
                        f.descripcion = string.Format("{0} {1}", lstCC.FirstOrDefault(x => x.cc.Equals(f.cc)).descripcion, f.factura);
                        f.grupo = string.Format("{0}: {1}", c.Key, c.Key);
                        lst.Add(f);
                    });
                });
                //lst.Add(new EstClieFacturaDTO()
                //{
                //    no = ++i,
                //    descripcion = "SUBTOTAL",
                //    estimacion = lstFacuraClientes.Sum(s => s.Sum(ss => ss.estimacion)),
                //    anticipo = lstFacuraClientes.Sum(s => s.Sum(ss => ss.anticipo)),
                //    vencido = lstFacuraClientes.Sum(s => s.Sum(ss => ss.vencido)),
                //    cobrado = lstFacuraClientes.Sum(s => s.Sum(ss => ss.cobrado)),
                //    pronostico = lstFacuraClientes.Sum(s => s.Sum(ss => ss.pronostico)),
                //    clase = "subtotal",
                //});
                lstInds.ForEach(inds =>
                {
                    string descDivision = EnumExtensions.GetDescription((TipoCCEnum)inds).ToUpper();
                    var lstFacturaIndustrial = lstFacturaObra.Where(w => inds == w.division);
                    lst.Add(new EstClieFacturaDTO()
                    {
                        no = ++i,
                        vencido = lstFacturaIndustrial.Sum(s => s.lstFacturas.Sum(ss => ss.vencido)),
                        pronostico = lstFacturaIndustrial.Sum(s => s.lstFacturas.Sum(ss => ss.pronostico)),
                        descripcion = descDivision,
                        clase = "encabezado",
                    });
                    lstFacturaIndustrial.ToList().ForEach(c =>
                    {
                        var facturasCC = c.lstFacturas.GroupBy(e => e.cc).ToList();

                        foreach (var agrupacionCC in facturasCC)
                        {

                            var cc = lstCC.FirstOrDefault(x => x.cc.Equals(agrupacionCC.Key));

                            lst.Add(new EstClieFacturaDTO()
                            {
                                no = ++i,
                                descripcion = "[" + agrupacionCC.Key + "] " + cc.descripcion,
                                numcte = agrupacionCC.Key,
                                grupo = cc.cc + "-" + cc.descripcion,
                                grupoCC = EnumExtensions.GetDescription((TipoCCEnum)inds),
                                cc = cc.cc,
                                estimacion = agrupacionCC.Sum(s => s.estimacion),
                                anticipo = agrupacionCC.Sum(s => s.anticipo),
                                vencido = agrupacionCC.Sum(s => s.vencido),
                                pronostico = agrupacionCC.Sum(s => s.pronostico),
                                cobrado = agrupacionCC.Sum(s => s.cobrado),
                                clase = "normalCC"
                            });

                            agrupacionCC.GroupBy(f => f.nombreCliente).ToList().ForEach(f =>
                            {
                                //var cliente = lstCliente.FirstOrDefault(cli => cli.numcte == f.Key.ParseInt()).nombre;
                                lst.Add(new EstClieFacturaDTO()
                                {
                                    no = ++i,
                                    descripcion = f.Key,
                                    numcte = f.Key,
                                    grupo = cc.cc + "-" + cc.descripcion,
                                    grupoCC = EnumExtensions.GetDescription((TipoCCEnum)inds),
                                    cc = cc.cc,
                                    estimacion = f.Sum(s => s.estimacion),
                                    anticipo = f.Sum(s => s.anticipo),
                                    vencido = f.Sum(s => s.vencido),
                                    pronostico = f.Sum(s => s.pronostico),
                                    cobrado = f.Sum(s => s.cobrado),
                                    clase = "normalCliente",
                                });

                                var lstFacturasCC = f.GroupBy(e => e.cc + " - " + e.descripcion).ToList();

                                foreach (var item in f)
                                {
                                    lst.Add(new EstClieFacturaDTO()
                                    {
                                        no = ++i,
                                        descripcion = item.factura,
                                        numcte = "",
                                        grupo = cc.cc + "-" + cc.descripcion,
                                        grupoCC = EnumExtensions.GetDescription((TipoCCEnum)inds),
                                        cc = cc.cc,
                                        estimacion = item.estimacion,
                                        anticipo = item.anticipo,
                                        vencido = item.vencido,
                                        pronostico = item.pronostico,
                                        cobrado = item.cobrado,
                                        clase = "normalFactura",
                                        fecha = item.fecha,
                                        fechavenc = item.fechavenc,
                                    });
                                }

                                //lst.Add(new EstClieFacturaDTO()
                                //{
                                //    no = ++i,
                                //    descripcion = f.Key + " - CLIENTE SUBTOTAL",
                                //    estimacion = f.Sum(ss => ss.estimacion),
                                //    anticipo = f.Sum(ss => ss.anticipo),
                                //    vencido = f.Sum(ss => ss.vencido),
                                //    cobrado = f.Sum(ss => ss.cobrado),
                                //    pronostico = f.Sum(ss => ss.pronostico),
                                //    clase = "subtotalCliente",
                                //});
                            });

                            //lst.Add(new EstClieFacturaDTO()
                            //{
                            //    no = ++i,
                            //    descripcion = "[" + cc.cc + " ] " + cc.descripcion + " - CC SUBTOTAL",
                            //    estimacion = agrupacionCC.Sum(ss => ss.estimacion),
                            //    anticipo = agrupacionCC.Sum(ss => ss.anticipo),
                            //    vencido = agrupacionCC.Sum(ss => ss.vencido),
                            //    cobrado = agrupacionCC.Sum(ss => ss.cobrado),
                            //    pronostico = agrupacionCC.Sum(ss => ss.pronostico),
                            //    clase = "subtotalCC",
                            //});
                        }
                    });

                    //lst.Add(new EstClieFacturaDTO()
                    //{
                    //    no = ++i,
                    //    descripcion = descDivision + " - DIVISION SUBTOTAL",
                    //    estimacion = lstFacturaIndustrial.Sum(s => s.lstFacturas.Sum(ss => ss.estimacion)),
                    //    anticipo = lstFacturaIndustrial.Sum(s => s.lstFacturas.Sum(ss => ss.anticipo)),
                    //    vencido = lstFacturaIndustrial.Sum(s => s.lstFacturas.Sum(ss => ss.vencido)),
                    //    cobrado = lstFacturaIndustrial.Sum(s => s.lstFacturas.Sum(ss => ss.cobrado)),
                    //    pronostico = lstFacturaIndustrial.Sum(s => s.lstFacturas.Sum(ss => ss.pronostico)),
                    //    clase = "subtotalDivision",
                    //});

                });
                //lst.Add(new EstClieFacturaDTO()
                //{
                //    no = ++i,
                //    descripcion = "SUBTOTAL",
                //    estimacion = lstFacturaObra.Sum(s => s.lstFacturas.Sum(ss => ss.estimacion)),
                //    anticipo = lstFacturaObra.Sum(s => s.lstFacturas.Sum(ss => ss.anticipo)),
                //    vencido = lstFacturaObra.Sum(s => s.lstFacturas.Sum(ss => ss.vencido)),
                //    cobrado = lstFacturaObra.Sum(s => s.lstFacturas.Sum(ss => ss.cobrado)),
                //    pronostico = lstFacturaObra.Sum(s => s.lstFacturas.Sum(ss => ss.pronostico)),
                //    clase = "subtotal",
                //});
                lst.Add(new EstClieFacturaDTO()
                {
                    no = ++i,
                    descripcion = "GRAN TOTAL",
                    estimacion = facturas.Sum(s => s.estimacion),
                    anticipo = facturas.Sum(s => s.anticipo),
                    vencido = facturas.Sum(s => s.vencido),
                    cobrado = facturas.Sum(s => s.cobrado),
                    pronostico = facturas.Sum(s => s.pronostico),
                    clase = "subtotal",
                });
                Session["lstEstimacionResumen"] = lst;
                Session["EstfechaFinal"] = fechaFinal;
                Session["stAuth"] = esAuth.Value ? "Aceptado" : "Pendiente" ;
                result.Add("facturas", lst);
                result.Add("lstFacturas", facturas);
                result.Add("auth", esAuth.Value);
                result.Add("stAuth", esAuth.Value ? "Aceptado" : "Pendiente" );
                result.Add(SUCCESS, lst.Count > 0);
            }
            catch (Exception e)
            {
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult guardarLstEstimacionResumen(List<tblF_EstimacionResumen> lst)
        {
            var result = new Dictionary<string, object>();
            try
            {
                lst = lst.Where(est => est.numcte.Length > 0 && est.cc.Length == 3 && est.factura != null && est.factura.ParseInt() > 0
                    && !est.fecha.Equals(default(DateTime)) && !est.fechavenc.Equals(default(DateTime)) && !est.fechaResumen.Equals(default(DateTime))
                    && (est.estimacion != 0 || est.anticipo != 0 || est.vencido != 0 || est.pronostico != 0 || est.cobrado != 0)).ToList();
                var esGuardado = facturaFS.getFacturaService().guardarLstEstimacionResumen(lst);
                result.Add(SUCCESS, esGuardado);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult eliminarEstimacion(List<int> lstId)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var esEliminado = true;
                if(lstId.Count > 0 && lstId.All(id => id > 0))
                {
                    esEliminado = facturaFS.getFacturaService().eliminarEstimacion(lstId);
                }
                result.Add(SUCCESS, esEliminado);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult authResumenEstimacion(DateTime fecha)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var esAuth = false;
                if(!fecha.Equals(default(DateTime)))
                {
                    esAuth = facturaFS.getFacturaService().authResumenEstimacion(fecha);
                }
                result.Add(SUCCESS, esAuth);
            }
            catch(Exception o_O)
            {
                result.Add(MESSAGE, o_O.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Resumen Proveedores
        public ViewResult AnalitivoVencimiento6Col()
        {
            return View();
        }

        public ViewResult EstimacionProveedor()
        {
            return View();
        }
        [HttpPost]
        public ActionResult getLstAnaliticoVencimiento6Col(BusqAnaliticoDTO busq)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var thisCC = lstCC.Select(c => c.cc).ToList();
                var lstProv = polizaFS.getPolizaService().getProveedor();
                var lstTm = polizaFS.getPolizaService().getComboTipoMovimiento("P");
                var lstSigoplan = polizaFS.getPolizaService().getAllCondSaldosActivos().Where(w => w.fechaPropuesta.noSemana() == busq.fecha.noSemana() && busq.lstCC.Any(c => c.Equals(w.cc))).ToList();
                var lstVencido = polizaFS.getPolizaService().getLstAnaliticoVencimiento6Col(busq);
                var lst = lstVencido.Select(venc => new
                {
                    numpro = venc.numpro,
                    factura = venc.factura,
                    cc = venc.cc,
                    tm = venc.tm,
                    porVencer = venc.porVencer,
                    dias7 = venc.dias7,
                    dias14 = venc.dias14,
                    dias30 = venc.dias30,
                    dias45 = venc.dias45,
                    dias60 = venc.dias60,
                    dias61 = venc.dias61,
                    moneda = venc.numpro < 9000 ? 2 : 1,
                    descMon = venc.numpro < 9000 ? "DLL" : "MN",
                    fechaVence = venc.fechaVence.ToShortDateString(),
                    fechaFactura = venc.fechaFactura.ToShortDateString(),
                    descCC = lstCC.FirstOrDefault(c => c.cc.Equals(venc.cc)).descripcion,
                    tipoMov = lstTm.FirstOrDefault(t => t.Value.Equals(venc.tm.ToString())).Text,
                    proveedor = lstProv.FirstOrDefault(p => p.numpro.Equals(venc.numpro)).nombre,
                    total = venc.porVencer + venc.dias7 + venc.dias14 + venc.dias30 + venc.dias45 + venc.dias60 + venc.dias61,
                    idGiro = lstSigoplan.Any(s => s.numpro.Equals(venc.numpro.ToString()) && s.factura.Equals(venc.factura.ToString())) ? lstSigoplan.FirstOrDefault(s => s.numpro.Equals(venc.numpro.ToString()) && s.factura.Equals(venc.factura.ToString()) && s.cc.Equals(venc.cc)).idGiro : 0,
                    esPropuesta = lstSigoplan.Any(s => s.numpro.Equals(venc.numpro.ToString()) && s.factura.Equals(venc.factura.ToString())) ? lstSigoplan.FirstOrDefault(s => s.numpro.Equals(venc.numpro.ToString()) && s.factura.Equals(venc.factura.ToString()) && s.cc.Equals(venc.cc)).esPropuesta : false,
                }).ToList();
                var esSuccees = lst.Count > 0;
                if(esSuccees)
                {
                    result.Add("lst", lst);
                }

                result.Add(SUCCESS, esSuccees);
            }
            catch(Exception)
            {
                result.Add(SUCCESS, false);
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        public ActionResult guardarCondensadoSaldos(List<tblC_SaldosCondensados> listaProveedores)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var esGuardado = false;
                if(listaProveedores.All(cond => cond.cc.Length >= 3 && cond.total > 0 && cond.numpro.Length > 0 && cond.moneda > 0))
                {
                    esGuardado = polizaFS.getPolizaService().guardarLstSaldosCondensados(listaProveedores);
                }
                result.Add(SUCCESS, esGuardado);
            }
            catch(Exception)
            {
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getLstEstProv(DateTime min, DateTime max)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstProv = polizaFS.getPolizaService().getProveedor();
                var lstGiro = giroProvFS.getGiroProvServices().getAllGiro();
                var lstEst = EnumExtensions.ToCombo<tipoEstimacionProveedorEnum>();
                var lstTm = polizaFS.getPolizaService().getComboTipoMovimiento("P");
                var lst = estProvFS.getEstProvService().getLstEstProv(min, max);
                var res = lst.Select(ep => new
                {
                    id = ep.id,
                    idGiro = ep.idGiro,
                    idEst = ep.idEst,
                    fecha = ep.fecha,
                    numpro = ep.numpro,
                    cc = ep.cc,
                    tm = ep.tm,
                    total = ep.total,
                    comentarios = ep.comentarios,
                    descCC = lstCC.FirstOrDefault(c => c.cc.Equals(ep.cc)).descripcion,
                    descEst = lstEst.FirstOrDefault(e => e.Value.Equals(ep.idEst)).Text.ToUpper(),
                    descProv = lstProv.FirstOrDefault(p => ep.numpro.Equals(p.numpro.ToString())).nombre,
                    descGiro = string.Format("{0}", lstGiro.FirstOrDefault(g => g.id.Equals(ep.idGiro)).descripcion),
                    descTm = lstTm.FirstOrDefault(t => t.Value.ParseInt() == ep.tm).Text
                }).ToList();
                result.Add("lst", res);
                result.Add(SUCCESS, lst.Count > 0);
            }
            catch(Exception)
            {
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult guardarEstProv(tblC_EstimacionProveedor estProv)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var esGuardado = false;
                if(estProv.cc.Length.Equals(3) && estProv.total != 0 && estProv.idGiro > 0 && estProv.tm > 0 && estProv.idEst > 0 && estProv.numpro.Length > 0 && !estProv.fecha.Equals(default(DateTime)))
                {
                    estProv.comentarios = estProv.comentarios ?? string.Empty;
                    esGuardado = estProvFS.getEstProvService().guardarEstProv(estProv);
                }
                result.Add(SUCCESS, esGuardado);
            }
            catch(Exception)
            {
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Pronostico Cobranza
        public ViewResult PronosticoCobranza()
        {
            return View();
        }
        public ActionResult getLstEstimacionCobranza(DateTime fecha)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lst = estCobFS.getEstCobService().getLstEstimacionCobranza(fecha);
                var esSucces = lst.Count > 0;
                if(esSucces)
                {
                    result.Add("lst", lst);
                }
                result.Add(SUCCESS, esSucces);
            }
            catch(Exception)
            {
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult guardarEstimacionCobro(List<tblC_EstimacionCobranza> lst)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var esGuardado = lst.All(a => a.cc.Count() == 3 && (a.estimado > 0 || a.semana1 > 0 || a.semana2 > 0 || a.semana3 > 0));
                if(esGuardado)
                {
                    esGuardado = estCobFS.getEstCobService().guardarEstimacionCobro(lst);
                }
                result.Add(SUCCESS, esGuardado);
            }
            catch(Exception)
            {
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region CostoEstimado
        public ViewResult CostoEstimado()
        {
            return View();
        }
        public ActionResult getLstCostoEstimado(DateTime fecha)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lst = costoEstFS.getCostoEstimadoServices().getLstCostoEstimado(fecha);
                var esSucces = lst.Count > 0;
                if(esSucces)
                {
                    result.Add("lst", lst);
                }
                result.Add(SUCCESS, esSucces);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult guardarLstCostoEstimado(List<tblC_CostoEstimado> lst)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var esGuardado = false;
                esGuardado = lst.All(a => a.cc.Count() == 3 && a.estimacion > 0);
                if(esGuardado)
                {
                    esGuardado = costoEstFS.getCostoEstimadoServices().guardarLstCostoEstimado(lst);
                }
                result.Add(SUCCESS, esGuardado);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Propuesta pago
        public ActionResult Pago()
        {
            return View();
        }
        public ActionResult _propuestaPago()
        {
            return PartialView();
        }
        public ActionResult getPropuestaPago(BusqConcentradoDTO busq)
        {
            var result = new Dictionary<string, object>();
            //try
            //{
            busq = generarBusqDTO(busq);
            var thisCC = getObra().GroupBy(g => g.descripcion).Select(s => s.OrderBy(o => o.cc).FirstOrDefault()).OrderBy(o => o.cc).ToList();
            var lst = getLstPropuesta(busq);
            result.Add("lst", lst);
            result.Add("lstCC", thisCC);
            result.Add(SUCCESS, lst.Count > 0);
            //}
            //catch (Exception e)
            //{
            //    result.Add(MESSAGE, e.Message);
            //    result.Add(SUCCESS, false);
            //}
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Conversiones
        bool esUsuarioDivIndustrial()
        {
            return vSesiones.sesionUsuarioDTO.perfil.Equals("Administrador") ? false : vSesiones.sesionUsuarioDTO.permisos2.Any(p => p.accion.Any(a => a.vistaID.Equals(vSesiones.sesionCurrentView)));
        }
        BusqConcentradoDTO generarBusqDTO(BusqConcentradoDTO busq)
        {

            if(busq == null || default(DateTime).Equals(busq.min))
            {
                busq = generarBusqDTO();
            }
            else
            {

                var ahora = busq.max;
                var ant = ahora.AddDays(-7);
                var min = ant.Siguiente(DayOfWeek.Sunday);
                var max = min.Siguiente(DayOfWeek.Saturday);
                var esDivIndustrial = esUsuarioDivIndustrial();
                var thisCC = getObra();
                var lstDiv = getLstDiv();
                busq.lstCC = cadenaFS.getCadenaProductivaService().lstObra().Where(c => lstDiv.Any(d => d == c.bit_area.ParseInt())).Select(s => s.cc).ToList();
                thisCC = thisCC
                    .GroupBy(g => setCC(g.cc))
                    .Select(s => s.OrderBy(o => o.cc).FirstOrDefault()).ToList();
                if(esDivIndustrial)
                {
                    thisCC.Add(new CcDTO()
                    {
                        bit_area = "1",
                        cc = "CIV",
                        descripcion = "CIVIL"
                    });
                }
                else
                {
                    thisCC.Add(new CcDTO()
                    {
                        bit_area = "2",
                        cc = "IND",
                        descripcion = "INDUSTRIAL"
                    });

                }
                busq.ctrlCC = thisCC.Select(s => s.cc).ToList();
                busq.esDivIndustrial = esDivIndustrial;
            }
            return busq;
        }
        public BusqConcentradoDTO generarBusqDTO()
        {
            var ahora = DateTime.Now;
            var ant = ahora.AddDays(-7);
            var min = ant.Siguiente(DayOfWeek.Sunday);
            var max = min.Siguiente(DayOfWeek.Saturday);
            var esDivIndustrial = esUsuarioDivIndustrial();
            var thisCC = getObra().Select(c => c.cc).ToList();
            var ctrlCC = thisCC.GroupBy(g => setCC(g)).Select(s => s.Key).OrderBy(o => o).ToList();
            var busq = new BusqConcentradoDTO()
            {
                lstCC = thisCC,
                ctrlCC = ctrlCC,
                min = min,
                max = max,
                esDivIndustrial = esDivIndustrial
            };
            return busq;
        }

        #region Propuesta pago
        List<PropuestaPagoDTO> getLstPropuesta(BusqConcentradoDTO busq)
        {
            var lst = new List<PropuestaPagoDTO>();
            var blanco = getPropuestaBlanco(busq);
            lst.Add(getOrigen(busq.ctrlCC));
            lst.AddRange(getSumaRecursos(busq));
            lst.AddRange(getRemanente(busq));
            lst.AddRange(getResumenCondensadoSaldosProveedor(busq));
            lst.AddRange(getAplicaciones(busq));
            lst.AddRange(getAnaliticoVencimiento(busq));
            lst.Add(getSumaSemana(busq));
            lst.AddRange(getCobroSigSemanas(busq));
            lst.AddRange(getSemanaSiguiente(busq));
            return lst;
        }
        #region List propuesta
        PropuestaPagoDTO getOrigen(List<string> lstCC)
        {
            return new PropuestaPagoDTO()
            {
                clase = EnumExtensions.GetDescription(tipoPropuestaEnum.SinSaldo),
                grupo = string.Empty,
                lstGrupoConca = new List<string>(),
                nivelSuma = 0,
                desc = "O R I G E N",
                esEscondido = false,
                cc = lstCC.Select(c => new PropuestaCCDTO() { cc = c }).ToList()
            };
        }
        PropuestaPagoDTO getPropuestaBlanco(BusqConcentradoDTO busq)
        {
            return new PropuestaPagoDTO()
            {
                clase = EnumExtensions.GetDescription(tipoPropuestaEnum.SinSaldo),
                grupo = string.Empty,
                lstGrupoConca = new List<string>(),
                nivelSuma = 0,
                esEscondido = false,
                desc = string.Empty,
                cc = busq.ctrlCC.Select(c => new PropuestaCCDTO() { cc = c }).ToList()
            };
        }
        #endregion
        List<PropuestaPagoDTO> getSumaRecursos(BusqConcentradoDTO busq)
        {
            var lst = new List<PropuestaPagoDTO>();
            var grupo = EnumExtensions.GetDescription(GrupoPropuestaEnum.sumaRecursos);
            var authEstClte = facturaFS.getFacturaService().getAuthResumenEstimacion(busq.min, busq.max);
            var lstEstimaciones = authEstClte.stAuth.Equals((int)stAuthEnum.Aceptado) ? facturaFS.getFacturaService().getlstEstimadoActivo(busq.min, busq.max) : new List<tblF_EstimacionResumen>();
            lstEstimaciones.ForEach(est => est.cc = setCC(est.cc));
            #region getSaldoBancos
            var lstSaldo = generarSaldos(busq)
                            .GroupBy(g => setCC(g.cc))
                            .Select(s => new
                            {
                                cc = s.Key,
                                total = s.Sum(ss => ss.cargo - ss.abono)
                            }).ToList();
            var saldoBancos = new PropuestaPagoDTO()
            {
                clase = EnumExtensions.GetDescription(tipoPropuestaEnum.Saldo),
                grupo = grupo,
                lstGrupoConca = new List<string>(),
                nivelSuma = 1,
                esEscondido = false,
                desc = string.Format("SALDO EN BANCOS AL {0} DE {1} DEL {2}", busq.max.Day, busq.max.ToString("MMMM"), busq.max.Year).ToUpper(),
                cc = busq.ctrlCC.Select(c => new PropuestaCCDTO()
                {
                    cc = c,
                    saldo = lstSaldo.Where(w => w.cc.Equals(c)).Sum(s => s.total)
                }).ToList()
            };
            lst.Add(saldoBancos);
            #endregion
            lst.Add(new PropuestaPagoDTO()
            {
                clase = EnumExtensions.GetDescription(tipoPropuestaEnum.Saldo),
                grupo = grupo,
                lstGrupoConca = new List<string>() { grupo },
                nivelSuma = 1,
                esEscondido = false,
                desc = "PRONOSTICO DE COBRANZA SIN IVA",
                cc = busq.ctrlCC.Select(c => new PropuestaCCDTO()
                {
                    cc = c,
                    saldo = lstEstimaciones.Where(w => w.cc.Equals(c)).Sum(s => s.estimacion)
                }).ToList()
            });
            lst.Add(new PropuestaPagoDTO()
            {
                clase = EnumExtensions.GetDescription(tipoPropuestaEnum.Saldo),
                grupo = grupo,
                lstGrupoConca = new List<string>() { grupo },
                nivelSuma = 1,
                esEscondido = false,
                desc = "RETIRO DE RESERVA Y/O ANTICIPO",
                cc = busq.ctrlCC.Select(c => new PropuestaCCDTO()
                {
                    cc = c,
                    saldo = lstEstimaciones.Where(w => w.cc.Equals(c)).Sum(s => s.anticipo)
                }).ToList()
            });
            lst.Add(new PropuestaPagoDTO()
            {
                clase = EnumExtensions.GetDescription(tipoPropuestaEnum.Suma),
                grupo = grupo,
                lstGrupoConca = new List<string>() { 
                    grupo,
                    EnumExtensions.GetDescription(GrupoPropuestaEnum.remanente)
                },
                nivelSuma = 2,
                esEscondido = false,
                desc = "SUMA DE RECURSOS",
                cc = busq.ctrlCC.Select(c => new PropuestaCCDTO()
                {
                    cc = c,
                    saldo = lstEstimaciones.Where(w => w.cc.Equals(c)).Sum(s => s.estimacion + s.anticipo) + lstSaldo.Where(w => w.cc.Equals(c)).Sum(s => s.total)
                }).ToList()
            });
            return lst;
        }
        List<PropuestaPagoDTO> getRemanente(BusqConcentradoDTO busq)
        {
            var grupo = EnumExtensions.GetDescription(GrupoPropuestaEnum.remanente);
            var lst = new List<PropuestaPagoDTO>();
            var blanco = getPropuestaBlanco(busq);
            var lstReservaControl = reservaFS.getReservasService().getLstCatReservasActivas()
                .Where(w => w.tipoReservaSaldoGlobal.Equals((int)TipoReservaSaldoGlobalEnum.Otros)).ToList();
            var lstReserva = reservaFS.getReservasService().getLstReservas(busq)
                                .Where(r => lstReservaControl.Any(pp => pp.id.Equals(r.tipo)))
                                .GroupBy(g => new { g.tipo, g.cc })
                                .Select(r => new
                                {
                                    cc = r.Key.cc,
                                    tipo = r.Key.tipo,
                                    total = r.Sum(s => s.cargo - s.abono)
                                }).ToList();
            blanco.grupo = grupo;
            var lstProv = polizaFS.getPolizaService().getAllCondSaldosActivos();
            lstProv.ForEach(prov => prov.cc = setCC(prov.cc));
            var lstEstimaciones = facturaFS.getFacturaService().getlstEstimadoActivo(busq.min, busq.max);
            lstEstimaciones.ForEach(est => est.cc = setCC(est.cc));
            var lstSaldo = generarSaldos(busq)
                            .GroupBy(g => setCC(g.cc))
                            .Select(s => new
                            {
                                cc = s.Key,
                                total = s.Sum(ss => ss.cargo - ss.abono)
                            }).ToList();
            lst.Add(new PropuestaPagoDTO()
            {
                clase = EnumExtensions.GetDescription(tipoPropuestaEnum.Suma),
                grupo = grupo,
                lstGrupoConca = new List<string>(),
                nivelSuma = 1,
                esEscondido = false,
                desc = "TOTAL DE COSTOS Y GASTOS",
                cc = busq.ctrlCC.Select(c => new PropuestaCCDTO()
                {
                    cc = c,
                    saldo = lstProv.Where(w => w.cc.Equals(c)).Sum(s => s.total)
                }).ToList()
            });
            lstReservaControl.ForEach(idRes =>
            {
                var objProp = new List<PropuestaCCDTO>();
                var lstTipo = lstReserva.Where(r => r.tipo.Equals(idRes.id)).ToList();
                busq.ctrlCC.ForEach(cc =>
                {
                    objProp.Add(new PropuestaCCDTO()
                    {
                        cc = cc,
                        saldo = lstTipo.Where(w => w.tipo.Equals(idRes.id) && cc.Equals(w.cc)).Sum(s => s.total)
                    });
                });
                lst.Add(new PropuestaPagoDTO()
                {
                    clase = EnumExtensions.GetDescription(tipoPropuestaEnum.Saldo),
                    grupo = grupo,
                    lstGrupoConca = new List<string>(),
                    nivelSuma = 1,
                    esEscondido = false,
                    desc = string.Format("RESERVA PARA {0}", idRes.descripcion).ToUpper(),
                    cc = objProp
                });
            });
            lst.Add(blanco);
            lst.Add(new PropuestaPagoDTO()
            {
                clase = EnumExtensions.GetDescription(tipoPropuestaEnum.Suma),
                grupo = grupo,
                lstGrupoConca = new List<string>() 
                { 
                    EnumExtensions.GetDescription(GrupoPropuestaEnum.sumaRecursos)
                },
                nivelSuma = 2,
                esEscondido = false,
                desc = "REMANENTE A FAVOR ( CONTRA )",
                cc = busq.ctrlCC.Select(c => new PropuestaCCDTO()
                {
                    cc = c,
                    saldo = lstEstimaciones.Where(w => w.cc.Equals(c)).Sum(s => s.estimacion + s.anticipo) + lstSaldo.Where(w => w.cc.Equals(c)).Sum(s => s.total) + lstProv.Where(w => w.cc.Equals(c)).Sum(s => s.total) + lstReserva.Where(w => w.cc.Equals(c)).Sum(s => s.total)
                }).ToList()
            });
            lst.Add(blanco);
            return lst;
        }
        List<PropuestaPagoDTO> getResumenCondensadoSaldosProveedor(BusqConcentradoDTO busq)
        {
            var lst = new List<PropuestaPagoDTO>();
            var grupo = EnumExtensions.GetDescription(GrupoPropuestaEnum.proveedor);
            var blanco = getPropuestaBlanco(busq);
            blanco.grupo = grupo;
            var lstBanco = EnumExtensions.ToCombo<NoProvBanco>();
            var lstCondensado = polizaFS.getPolizaService().getLstCondSaldosBancos(busq);
            lstCondensado.ForEach(c =>
            {
                c.cc = setCC(c.cc);
                c.total = c.saldo + c.vencido;
            });
            var lstSaldos = lstCondensado.Where(c => !lstBanco.Any(a => a.Value.ToString().Equals(c.numpro))).ToList();
            var lstCondensadoBanco = lstCondensado.Where(c => lstBanco.Any(a => a.Value.ToString().Equals(c.numpro))).ToList();
            lst.Add(new PropuestaPagoDTO()
            {
                clase = EnumExtensions.GetDescription(tipoPropuestaEnum.Saldo),
                grupo = grupo,
                lstGrupoConca = new List<string>() { grupo },
                nivelSuma = 1,
                esEscondido = true,
                desc = "SALDO DE PROVEEDORES",
                cc = (from cc in busq.ctrlCC
                      join cond in lstSaldos on cc equals cond.cc into ccCond
                      from cond in ccCond.DefaultIfEmpty()
                      select new PropuestaCCDTO
                      {
                          cc = cc,
                          saldo = cond == null ? 0 : cond.total
                      }).GroupBy(g => g.cc).Select(q => new PropuestaCCDTO()
                      {
                          cc = q.Key,
                          saldo = q.Sum(r => r.saldo) + lstCondensadoBanco.Where(w => w.cc.Equals(q.Key)).Sum(s => s.total)
                      }).ToList()
            });
            lst.Add(blanco);
            lst.AddRange(lstCondensadoBanco.GroupBy(g => g.numpro).Select(s => new PropuestaPagoDTO()
            {
                clase = EnumExtensions.GetDescription(tipoPropuestaEnum.Saldo),
                grupo = grupo,
                lstGrupoConca = new List<string>(),
                nivelSuma = 1,
                esEscondido = true,
                desc = EnumExtensions.GetDescription((NoProvBanco)s.Key.ParseInt()),
                cc = (from cc in busq.ctrlCC
                      join cond in s on cc equals cond.cc into ccCond
                      from cond in ccCond.DefaultIfEmpty()
                      select new PropuestaCCDTO
                      {
                          cc = cc,
                          saldo = cond == null ? 0 : cond.total
                      }).GroupBy(g => g.cc).Select(q => new PropuestaCCDTO()
                      {
                          cc = q.Key,
                          saldo = q.Sum(r => r.saldo)
                      }).ToList()
            }).ToList());
            lst.Add(new PropuestaPagoDTO()
            {
                clase = EnumExtensions.GetDescription(tipoPropuestaEnum.Suma),
                grupo = grupo,
                lstGrupoConca = new List<string>(),
                nivelSuma = 2,
                esEscondido = true,
                desc = "TOTAL PROVEEDORES",
                cc = busq.ctrlCC.Select(c => new PropuestaCCDTO()
                {
                    cc = c,
                    saldo = lstCondensado.Where(w => w.cc.Equals(c)).Sum(s => s.total) + lstCondensadoBanco.Where(w => w.cc.Equals(c)).Sum(s => s.total)
                }).ToList()
            });
            lst.Add(new PropuestaPagoDTO()
            {
                clase = EnumExtensions.GetDescription(tipoPropuestaEnum.Suma),
                grupo = grupo,
                lstGrupoConca = new List<string>(),
                nivelSuma = 2,
                esEscondido = false,
                desc = "TOTAL PROVEEDORES CADENAS",
                cc = busq.ctrlCC.Select(c => new PropuestaCCDTO()
                {
                    cc = c,
                    saldo = lstCondensadoBanco.Where(w => w.cc.Equals(c)).Sum(s => s.total)
                }).ToList()
            });
            lst.Add(new PropuestaPagoDTO()
            {
                clase = EnumExtensions.GetDescription(tipoPropuestaEnum.Suma),
                grupo = grupo,
                lstGrupoConca = new List<string>(),
                nivelSuma = 2,
                esEscondido = false,
                desc = "TOTAL PROVEEDORES OTROS",
                cc = busq.ctrlCC.Select(c => new PropuestaCCDTO()
                {
                    cc = c,
                    saldo = lstCondensadoBanco.Where(w => w.cc.Equals(c)).Sum(s => s.total) + lstSaldos.Where(w => w.cc.Equals(c)).Sum(s => s.total)
                }).ToList()
            });
            return lst;
        }
        List<PropuestaPagoDTO> getAplicaciones(BusqConcentradoDTO busq)
        {
            var lst = new List<PropuestaPagoDTO>();
            var grupo = EnumExtensions.GetDescription(GrupoPropuestaEnum.aplicaciones);
            var blanco = getPropuestaBlanco(busq);
            var lstNomina = nominaFS.getNominaServices().getLstResumenNominaActiva(busq.min, busq.max);
            lstNomina.ForEach(nom => nom.cc = setCC(nom.cc));
            var lstAportaciones = reservaFS.getReservasService().getLstReservasCC(busq)
                .Where(w => w.tipo.Equals((int)tipoReservaEnum.AportacionAdmin)).ToList();
            lstAportaciones.ForEach(apor => apor.cc = setCC(apor.cc));
            var lstControl = new List<int>() 
            {
                (int)tipoNominaPropuestaEnum.Semanal,
                (int)tipoNominaPropuestaEnum.Quincenal,
                0// resto
            };
            blanco.grupo = grupo;
            lst.Add(blanco);
            lst.Add(new PropuestaPagoDTO()
            {
                clase = EnumExtensions.GetDescription(tipoPropuestaEnum.SinSaldo),
                grupo = grupo,
                lstGrupoConca = new List<string>(),
                nivelSuma = 0,
                desc = "A  P  L  I  C  A  C  I  O  N  E  S  :",
                esEscondido = false,
                cc = busq.ctrlCC.Select(c => new PropuestaCCDTO() { cc = c }).ToList()
            });
            lst.Add(blanco);
            lstControl.ForEach(tipoNomina =>
            {
                if(tipoNomina.Equals(0))
                {
                    lst.Add(new PropuestaPagoDTO()
                    {
                        clase = EnumExtensions.GetDescription(tipoPropuestaEnum.Saldo),
                        grupo = grupo,
                        lstGrupoConca = new List<string>(),
                        nivelSuma = 1,
                        esEscondido = false,
                        desc = "PAGO DE NOMINA OTROS",
                        cc = busq.ctrlCC.Select(c => new PropuestaCCDTO()
                        {
                            cc = c,
                            saldo = lstNomina.Where(w => w.cc.Equals(c) && w.tipoNomina.Equals(tipoNomina)).Sum(s => s.total)
                        }).ToList()
                    });
                }
                else
                {
                    lst.Add(new PropuestaPagoDTO()
                    {
                        clase = EnumExtensions.GetDescription(tipoPropuestaEnum.SaldoEncabezado),
                        grupo = grupo,
                        lstGrupoConca = new List<string>(),
                        nivelSuma = 1,
                        esEscondido = false,
                        desc = string.Format("PAGO DE NOMINA {0}", EnumExtensions.GetDescription((tipoNominaPropuestaEnum)tipoNomina)).ToUpper(),
                        cc = busq.ctrlCC.Select(c => new PropuestaCCDTO()
                        {
                            cc = c,
                            saldo = lstNomina.Where(w => w.cc.Equals(c) && w.tipoNomina.Equals(tipoNomina)).Sum(s => s.total)
                        }).ToList()
                    });
                    lst.Add(new PropuestaPagoDTO()
                    {
                        clase = EnumExtensions.GetDescription(tipoPropuestaEnum.Saldo),
                        grupo = grupo,
                        lstGrupoConca = new List<string>(),
                        nivelSuma = 1,
                        esEscondido = false,
                        desc = "# DE EMPLEADOS",
                        cc = busq.ctrlCC.Select(c => new PropuestaCCDTO()
                        {
                            cc = c,
                            saldo = lstNomina.Where(w => w.cc.Equals(c) && w.tipoNomina.Equals(tipoNomina)).Sum(s => s.noEmpleado)
                        }).ToList()
                    });
                }
            });
            lst.Add(new PropuestaPagoDTO()
            {
                clase = EnumExtensions.GetDescription(tipoPropuestaEnum.SaldoEncabezado),
                grupo = grupo,
                lstGrupoConca = new List<string>(),
                nivelSuma = 1,
                esEscondido = false,
                desc = "APORTACIONES ADMON",
                cc = busq.ctrlCC.Select(c => new PropuestaCCDTO()
                {
                    cc = c,
                    saldo = lstAportaciones.Where(w => w.cc.Equals(c)).Sum(s => s.cargo - s.abono)
                }).ToList()
            });
            return lst;
        }
        List<PropuestaPagoDTO> getAnaliticoVencimiento(BusqConcentradoDTO busq)
        {
            var lst = new List<PropuestaPagoDTO>();
            var grupoReq = EnumExtensions.GetDescription(GrupoPropuestaEnum.requerimientoObra);
            var proveedores = polizaFS.getPolizaService().getProveedor();
            var lstGiro = giroProvFS.getGiroProvServices().getLstGiro();
            var lstTm = polizaFS.getPolizaService().getComboTipoMovimiento("P");
            var lstProv = polizaFS.getPolizaService().getAllCondSaldosActivos();
            var lstProp = propPrpgFs.getPropProgService().getLstGastosProv(busq).Select(est => new tblC_SaldosCondensados()
            {
                numpro = est.numpro.ToString(),
                idGiro = est.idGiro,
                cc = est.cc,
                total = est.total,
                fechaFactura = est.fecha,
                tm = est.tm
            }).ToList();
            var lstEstProv = estProvFS.getEstProvService().getLstEstProv(busq.min, busq.max).Select(est => new tblC_SaldosCondensados()
            {
                numpro = est.numpro,
                idGiro = est.idGiro,
                cc = est.cc,
                total = est.total,
                fechaFactura = est.fecha,
                tm = est.tm
            }).ToList();
            lstProv.AddRange(lstProp);
            lstProv.AddRange(lstEstProv);
            lstProv = lstProv.Where(p => p.total > 0).GroupBy(g => new { g.numpro, g.cc, g.idGiro, g.tm }).Select(est => new tblC_SaldosCondensados()
            {
                numpro = est.Key.numpro,
                idGiro = est.Key.idGiro,
                cc = est.Key.cc,
                total = est.Sum(s => s.total),
                tm = est.Key.tm
            }).ToList();
            lstProv.ForEach(prov => prov.cc = setCC(prov.cc));
            var lstNomina = nominaFS.getNominaServices().getLstResumenNominaActiva(busq.min, busq.max);
            lstNomina.ForEach(nom => nom.cc = setCC(nom.cc));
            var blancoReq = getPropuestaBlanco(busq);
            blancoReq.grupo = grupoReq;
            lst.Add(blancoReq);
            lstGiro.ToList().ForEach(padre =>
            {
                var lstProvPadre = lstProv.Where(p => p.idGiro == padre.id).ToList();
                lst.Add(new PropuestaPagoDTO()
                {
                    clase = EnumExtensions.GetDescription(tipoPropuestaEnum.Encabezado),
                    grupo = grupoReq,
                    lstGrupoConca = new List<string>(),
                    nivelSuma = 0,
                    desc = padre.descripcion,
                    esEscondido = false,
                    cc = busq.ctrlCC.Select(c => new PropuestaCCDTO() { cc = c }).ToList()
                });
                lst.Add(blancoReq);
                lstTm.ToList().ForEach(giro =>
                {
                    var lstProvTm = lstProvPadre.Where(w => w.tm == giro.Value.ParseInt()).ToList();
                    if(lstProvTm.Sum(s => s.total) > 0)
                    {
                        lst.Add(new PropuestaPagoDTO()
                        {
                            clase = EnumExtensions.GetDescription(tipoPropuestaEnum.Suma),
                            grupo = string.Format("{0}Padre{1}Giro{2}", grupoReq, padre.id, giro.Value),
                            lstGrupoConca = new List<string>(),
                            nivelSuma = 2,
                            desc = Regex.Replace(giro.Text, @"[\d-]", string.Empty),
                            esEscondido = true,
                            cc = busq.ctrlCC.Select(c => new PropuestaCCDTO()
                            {
                                cc = c,
                                saldo = lstProvTm.Where(p => c == p.cc).Sum(s => s.total)
                            }).ToList()
                        });
                        lstProvTm.Where(w => w.tm == giro.Value.ParseInt()).ToList().ForEach(giroProv =>
                        {
                            lst.Add(new PropuestaPagoDTO()
                            {
                                clase = EnumExtensions.GetDescription(tipoPropuestaEnum.Saldo),
                                grupo = string.Format("{0}Padre{1}Giro{2}", grupoReq, padre.id, giro.Value.ParseInt()),
                                lstGrupoConca = new List<string>(),
                                nivelSuma = 1,
                                desc = proveedores.FirstOrDefault(p => p.numpro.Equals(giroProv.numpro.ParseInt())).nombre,
                                esEscondido = true,
                                cc = busq.ctrlCC.Select(c => new PropuestaCCDTO()
                                {
                                    cc = c,
                                    saldo = giroProv.cc.Equals(c) ? giroProv.total : 0
                                }).ToList()
                            });
                        });
                    }
                });
                lst.Add(new PropuestaPagoDTO()
                {
                    clase = EnumExtensions.GetDescription(tipoPropuestaEnum.Suma),
                    grupo = grupoReq,
                    lstGrupoConca = new List<string>(),
                    nivelSuma = 0,
                    desc = "SUMA",
                    esEscondido = false,
                    cc = busq.ctrlCC.Select(c => new PropuestaCCDTO()
                    {
                        cc = c,
                        saldo = lstProvPadre.Where(p => c == p.cc).Sum(s => s.total)
                    }).ToList()
                });
                lst.Add(blancoReq);
            });
            lst.Add(new PropuestaPagoDTO()
            {
                clase = EnumExtensions.GetDescription(tipoPropuestaEnum.Suma),
                grupo = grupoReq,
                lstGrupoConca = new List<string>(),
                nivelSuma = 0,
                desc = "SUMA PROVEEDORES",
                esEscondido = false,
                cc = busq.ctrlCC.Select(c => new PropuestaCCDTO()
                {
                    cc = c,
                    saldo = lstProv.Where(p => c.Equals(p.cc)).Sum(s => s.total)
                }).ToList()
            });
            lst.Add(blancoReq);
            return lst;
        }
        PropuestaPagoDTO getSumaSemana(BusqConcentradoDTO busq)
        {
            var lstReservaControl = reservaFS.getReservasService().getLstCatReservasActivas()
                .Where(w => w.tipoReservaSaldoGlobal.Equals((int)TipoReservaSaldoGlobalEnum.Otros)).ToList();
            var lstSaldo = generarSaldos(busq)
                            .GroupBy(g => g.cc)
                            .Select(s => new
                            {
                                cc = s.Key,
                                total = s.Sum(ss => ss.cargo - ss.abono)
                            }).ToList();
            var lstReserva = reservaFS.getReservasService().getLstReservas(busq)
                                .Where(r => lstReservaControl.Any(pp => pp.id.Equals(r.tipo)))
                                .GroupBy(g => new { g.tipo, g.cc })
                                .Select(r => new
                                {
                                    cc = r.Key.cc,
                                    tipo = r.Key.tipo,
                                    total = r.Sum(s => s.cargo - s.abono)
                                }).ToList();
            var lstEstimaciones = facturaFS.getFacturaService().getlstEstimadoActivo(busq.min, busq.max, busq.lstCC);
            lstEstimaciones.ForEach(est => est.cc = setCC(est.cc));
            var lstProv = polizaFS.getPolizaService().getAllCondSaldosActivos();
            lstProv.ForEach(prov => prov.cc = setCC(prov.cc));
            return new PropuestaPagoDTO()
            {
                clase = EnumExtensions.GetDescription(tipoPropuestaEnum.Suma),
                grupo = EnumExtensions.GetDescription(GrupoPropuestaEnum.saldoFinSemana),
                lstGrupoConca = new List<string>() 
                {
                    EnumExtensions.GetDescription(GrupoPropuestaEnum.remanente),
                    EnumExtensions.GetDescription(GrupoPropuestaEnum.aplicaciones),
                    EnumExtensions.GetDescription(GrupoPropuestaEnum.proveedor)
                },
                nivelSuma = 2,
                esEscondido = false,
                desc = "TOTAL DE COSTOS Y GASTOS PARA LA SEMANA",
                cc = busq.ctrlCC.Select(c => new PropuestaCCDTO()
                {
                    cc = c,
                    saldo = lstEstimaciones.Where(w => w.cc.Equals(c)).Sum(s => s.estimacion + s.anticipo) + lstSaldo.Where(w => w.cc.Equals(c)).Sum(s => s.total) + lstProv.Where(w => w.cc.Equals(c)).Sum(s => s.total) + lstReserva.Where(w => w.cc.Equals(c)).Sum(s => s.total)
                }).ToList()
            };
        }
        List<PropuestaPagoDTO> getCobroSigSemanas(BusqConcentradoDTO busq)
        {
            var lst = new List<PropuestaPagoDTO>();
            var grupo = EnumExtensions.GetDescription(GrupoPropuestaEnum.cobroProxSemanas);
            var lstPeriodos = nominaFS.getNominaServices().getLstSig4Semanas(busq.min);
            var lstCobros = estCobFS.getEstCobService().getLstEstimacionCobranza(busq.max);
            lstCobros.ForEach(cob => cob.cc = setCC(cob.cc));
            lst.Add(new PropuestaPagoDTO()
            {
                clase = EnumExtensions.GetDescription(tipoPropuestaEnum.Saldo),
                grupo = grupo,
                lstGrupoConca = new List<string>(),
                nivelSuma = 1,
                esEscondido = true,
                desc = string.Format("FACTURADO SIN FECHA DE COBRO").ToUpper(),
                cc = busq.ctrlCC.Select(c => new PropuestaCCDTO()
                {
                    cc = c,
                    saldo = lstCobros.Where(w => w.cc.Equals(c)).Sum(s => s.estimado)
                }).ToList()
            });
            lst.Add(new PropuestaPagoDTO()
            {
                clase = EnumExtensions.GetDescription(tipoPropuestaEnum.Saldo),
                grupo = grupo,
                lstGrupoConca = new List<string>(),
                nivelSuma = 1,
                esEscondido = true,
                desc = string.Format("PRO.COBR.SEM DEL {0:dd} AL {1:dd} {1:MMMM} {1:yyyy}", lstPeriodos[0].fecha_inicial, lstPeriodos[0].fecha_final).ToUpper(),
                cc = busq.ctrlCC.Select(c => new PropuestaCCDTO()
                {
                    cc = c,
                    saldo = lstCobros.Where(w => w.cc.Equals(c)).Sum(s => s.semana1)
                }).ToList()
            });
            lst.Add(new PropuestaPagoDTO()
            {
                clase = EnumExtensions.GetDescription(tipoPropuestaEnum.Saldo),
                grupo = grupo,
                lstGrupoConca = new List<string>(),
                nivelSuma = 1,
                esEscondido = true,
                desc = string.Format("PRO.COBR.SEM DEL {0:dd} AL {1:dd} {1:MMMM} {1:yyyy}", lstPeriodos[1].fecha_inicial, lstPeriodos[1].fecha_final).ToUpper(),
                cc = busq.ctrlCC.Select(c => new PropuestaCCDTO()
                {
                    cc = c,
                    saldo = lstCobros.Where(w => w.cc.Equals(c)).Sum(s => s.semana2)
                }).ToList()
            });
            lst.Add(new PropuestaPagoDTO()
            {
                clase = EnumExtensions.GetDescription(tipoPropuestaEnum.Saldo),
                grupo = grupo,
                lstGrupoConca = new List<string>(),
                nivelSuma = 1,
                esEscondido = true,
                desc = string.Format("PRO.COBR.SEM DEL {0:dd} AL {1:dd} {1:MMMM} {1:yyyy}", lstPeriodos[2].fecha_inicial, lstPeriodos[2].fecha_final).ToUpper(),
                cc = busq.ctrlCC.Select(c => new PropuestaCCDTO()
                {
                    cc = c,
                    saldo = lstCobros.Where(w => w.cc.Equals(c)).Sum(s => s.semana3)
                }).ToList()
            });
            lst.Add(new PropuestaPagoDTO()
            {
                clase = EnumExtensions.GetDescription(tipoPropuestaEnum.Suma),
                grupo = grupo,
                lstGrupoConca = new List<string>(),
                nivelSuma = 2,
                esEscondido = true,
                desc = "PORCENTAJE (%)".ToUpper(),
                cc = busq.ctrlCC.Select(c => new PropuestaCCDTO()
                {
                    cc = c,
                    saldo = lstCobros.Where(s => s.cc.Equals(c)).Sum(s => s.estimado + s.semana1 + s.semana2 + s.semana3).DivideA(lstCobros.Sum(s => s.estimado)),
                }).ToList()
            });
            return lst;
        }
        List<PropuestaPagoDTO> getSemanaSiguiente(BusqConcentradoDTO busq)
        {
            var lst = new List<PropuestaPagoDTO>();
            var blancoReq = getPropuestaBlanco(busq);
            var lstBanco = EnumExtensions.ToCombo<NoProvBanco>();
            var grupo = EnumExtensions.GetDescription(GrupoPropuestaEnum.saldoFinSemana);
            var lstCondensado = polizaFS.getPolizaService().getLstCondSaldosBancos(busq).OrderBy(o => o.numpro).ToList();
            lstCondensado.ForEach(cond => { cond.cc = setCC(cond.cc); });
            var lstSaldos = lstCondensado.Where(c => !lstBanco.Any(a => a.Value.Equals(c.numpro))).ToList();
            var lstCondensadoBanco = lstCondensado.Where(c => lstBanco.Any(a => a.Value.Equals(c.numpro))).ToList();
            var lstEstimaciones = facturaFS.getFacturaService().getlstEstimadoActivo(busq.min, busq.max, busq.lstCC);
            lstEstimaciones.ForEach(est => { est.cc = setCC(est.cc); });
            var lstCondCplan = polizaFS.getPolizaService().getLstCondSaldosCplan(busq);
            var lstCondArren = polizaFS.getPolizaService().getLstAnaliticoVencimiento6ColArrendadora(busq);
            var lstProv = polizaFS.getPolizaService().getAllCondSaldosActivos();
            var lstCosEst = costoEstFS.getCostoEstimadoServices().getLstCostoEstimado(busq.max);
            var lstCobEst = estCobFS.getEstCobService().getLstEstimacionCobranza(busq.max);
            var lstRes = reservaFS.getReservasService().getLstReservas(busq).Where(w => w.tipo.Equals(tipoReservaEnum.AportacionAdmin) || w.tipo.Equals(tipoReservaEnum.AportacionCdiAlimentos) || w.tipo.Equals(tipoReservaEnum.AportacionCdiAutomotrizBajio) || w.tipo.Equals(tipoReservaEnum.AportacionCdiAutomotrizNoroeste)).ToList();
            #region Obras Cplan Arren
            var lstObra = cadenaFS.getCadenaProductivaService().lstObra();
            lstObra.AddRange(new List<CcDTO>() { 
                new CcDTO(){
                    cc = "010",
                    descripcion = "TALLER DE REPARACION LLANTAS OTR"
                },
                new CcDTO(){
                    cc = "0-1",
                    descripcion = "MAQUINARIA NO ASIGNADA A OBRA (OBRAS CERRADAS)"
                },
                new CcDTO(){
                    cc = "044",
                    descripcion = "WAREHOUSE GRUPO MODELO"
                },
                new CcDTO(){
                    cc = "160",
                    descripcion = "COMPONENTES LA HERRADURA"
                },
                new CcDTO(){
                    cc = "165",
                    descripcion = "CAMINO CERRO PELON"
                },
                new CcDTO(){
                    cc = "351",
                    descripcion = "CAPUFE TECATE"
                },
                new CcDTO(){
                    cc = "987",
                    descripcion = "componentes Construcción"
                },
                new CcDTO(){
                    cc = "C72",
                    descripcion = "SECTOR AUTOMOTRIZ NOROESTE"
                },
                new CcDTO(){
                    cc = "010",
                    descripcion = "TALLER MECANICO CENTRAL (ADMINISTRACION)"
                },
                new CcDTO(){
                    cc = "557",
                    descripcion = "ADMINISTRACION TAMPICO"
                },
                new CcDTO(){
                    cc = "046",
                    descripcion = "PEPSICO PLANTA MAIZORO"
                },
                new CcDTO(){
                    cc = "046",
                    descripcion = "PEPSICO PLANTA MAIZORO"
                },
                new CcDTO(){
                    cc = "996",
                    descripcion = "MAQUINARIA PARA VENTA"
                },
                new CcDTO(){
                    cc = "010",
                    descripcion = "TALLER OVERHAUL CONSTRUCCION (ADMINISTRACION)"
                },
                new CcDTO(){
                    cc = "074",
                    descripcion = "SILOS GRUPO MODELO"
                },
                new CcDTO(){
                    cc = "159",
                    descripcion = "PATIO DE MAQUINARIA (ADMINISTRACION)"
                },
                new CcDTO(){
                    cc = "988",
                    descripcion = "REPARACION DE COMPONENTES-OVERHAUL"
                }
            });
            #endregion
            lstCondCplan.ForEach(cond => cond.cc = setCC(cond.cc));
            lstCondArren.ForEach(cond =>
            {
                if(lstObra.Any(c => c.descripcion.Trim().ToUpper().Contains(cond.cc.Trim().ToUpper())))
                {
                    cond.cc = lstObra.FirstOrDefault(c => c.descripcion.Trim().ToUpper().Contains(cond.cc.Trim().ToUpper())).cc;
                    cond.cc = setCC(cond.cc);
                }
            });
            lstCosEst.ForEach(cost => cost.cc = setCC(cost.cc));
            lstCobEst.ForEach(cost => cost.cc = setCC(cost.cc));
            var lstProp = propPrpgFs.getPropProgService().getLstGastosProv(busq).Select(est => new tblC_SaldosCondensados()
            {
                numpro = est.numpro.ToString(),
                idGiro = est.idGiro,
                cc = est.cc,
                total = est.total,
                fechaFactura = est.fecha,
            }).ToList();
            var lstEstProv = estProvFS.getEstProvService().getLstEstProv(busq.min, busq.max).Select(est => new tblC_SaldosCondensados()
            {
                numpro = est.numpro,
                idGiro = est.idGiro,
                cc = est.cc,
                total = est.total,
                fechaFactura = est.fecha,
            }).ToList();
            lstProv.AddRange(lstProp);
            lstProv.AddRange(lstEstProv);
            lstProv.ForEach(prov => { prov.cc = setCC(prov.cc); });
            var lstReservaCadena = reservaFS.getReservasService().getLstReservaCadenasAnteriores(busq.min, busq.max);
            var lstReservaControl = reservaFS.getReservasService().getLstCatReservasActivas()
                .Where(w => w.tipoReservaSaldoGlobal.Equals((int)TipoReservaSaldoGlobalEnum.Otros)).ToList();
            var lstReserva = reservaFS.getReservasService().getLstReservas(busq)
                                .Where(r => lstReservaControl.Any(pp => pp.id.Equals(r.tipo)))
                                .GroupBy(g => new { g.tipo, g.cc })
                                .Select(r => new
                                {
                                    cc = r.Key.cc,
                                    tipo = r.Key.tipo,
                                    total = r.Sum(s => s.cargo - s.abono)
                                }).ToList();
            lst.Add(new PropuestaPagoDTO()
            {
                clase = EnumExtensions.GetDescription(tipoPropuestaEnum.Saldo),
                grupo = grupo,
                lstGrupoConca = new List<string>(),
                nivelSuma = 1,
                esEscondido = false,
                desc = "4.0",
                cc = busq.ctrlCC.Select(c => new PropuestaCCDTO()
                {
                    cc = c,
                    saldo = lstCondensadoBanco.Where(w => w.cc.Equals(c)).Sum(s => s.total) + lstSaldos.Where(w => w.cc.Equals(c)).Sum(s => s.total)
                }).ToList()
            });
            lst.Add(new PropuestaPagoDTO()
            {
                clase = EnumExtensions.GetDescription(tipoPropuestaEnum.Saldo),
                grupo = grupo,
                lstGrupoConca = new List<string>(),
                nivelSuma = 1,
                esEscondido = false,
                desc = "ABONO A PROVEEDORES SEM",
                cc = busq.ctrlCC.Select(c => new PropuestaCCDTO()
                {
                    cc = c,
                    saldo = lstSaldos.Where(w => w.cc.Equals(c)).Sum(s => s.total)
                }).ToList()
            });
            lst.Add(new PropuestaPagoDTO()
            {
                clase = EnumExtensions.GetDescription(tipoPropuestaEnum.Saldo),
                grupo = grupo,
                lstGrupoConca = new List<string>(),
                nivelSuma = 1,
                esEscondido = false,
                desc = "SALDO DE PROV + CADENAS FINAL",
                cc = busq.ctrlCC.Select(c => new PropuestaCCDTO()
                {
                    cc = c,
                    saldo = lstCondensadoBanco.Where(w => w.cc.Equals(c)).Sum(s => s.total) + lstSaldos.Where(w => w.cc.Equals(c)).Sum(s => s.total)
                }).ToList()
            });
            lst.Add(new PropuestaPagoDTO()
            {
                clase = EnumExtensions.GetDescription(tipoPropuestaEnum.Suma),
                grupo = grupo,
                lstGrupoConca = new List<string>(),
                nivelSuma = 1,
                esEscondido = false,
                desc = "SALDO FIN DE SEMANA",
                cc = busq.ctrlCC.Select(c => new PropuestaCCDTO()
                {
                    cc = c,
                    saldo = lstEstimaciones.Where(w => w.cc.Equals(c)).Sum(s => s.estimacion + s.anticipo) + lstSaldos.Where(w => w.cc.Equals(c)).Sum(s => s.total) + lstProv.Where(w => w.cc.Equals(c)).Sum(s => s.total) + lstReserva.Where(w => w.cc.Equals(c)).Sum(s => s.total)
                }).ToList()
            });
            lst.Add(new PropuestaPagoDTO()
            {
                clase = EnumExtensions.GetDescription(tipoPropuestaEnum.Saldo),
                grupo = grupo,
                lstGrupoConca = new List<string>(),
                nivelSuma = 1,
                esEscondido = false,
                desc = string.Empty,
                cc = busq.ctrlCC.Select(c => new PropuestaCCDTO()
                {
                    cc = c,
                    saldo = lstEstimaciones.Where(w => w.cc.Equals(c)).Sum(s => s.estimacion + s.anticipo) + lstProv.Where(w => w.cc.Equals(c)).Sum(s => s.total) + lstReserva.Where(w => w.cc.Equals(c)).Sum(s => s.total) - lstCondensadoBanco.Where(w => w.cc.Equals(c)).Sum(s => s.total)
                }).ToList()
            });
            lst.Add(blancoReq);
            lst.Add(new PropuestaPagoDTO()
            {
                clase = EnumExtensions.GetDescription(tipoPropuestaEnum.Saldo),
                grupo = grupo,
                lstGrupoConca = new List<string>(),
                nivelSuma = 1,
                esEscondido = false,
                desc = "SALDO DE CONSTRUPLAN A ARRENDADORA",
                cc = busq.ctrlCC.Select(c => new PropuestaCCDTO()
                {
                    cc = c,
                    saldo = lstCondCplan.Where(w => w.cc.Equals(c)).Sum(s => s.porVencer)
                }).ToList()
            });
            lst.Add(blancoReq);
            lst.Add(new PropuestaPagoDTO()
            {
                clase = EnumExtensions.GetDescription(tipoPropuestaEnum.Saldo),
                grupo = grupo,
                lstGrupoConca = new List<string>(),
                nivelSuma = 1,
                esEscondido = false,
                desc = "SALDO DE ARRENDADORA A PROVEEDORES",
                cc = busq.ctrlCC.Select(c => new PropuestaCCDTO()
                {
                    cc = c,
                    saldo = lstCondArren.Where(w => w.cc.Equals(c)).Sum(s => s.porVencer)
                }).ToList()
            });
            lst.Add(blancoReq);
            lstReservaCadena.GroupBy(g => g.fecha.noSemana()).ToList().ForEach(rc =>
            {
                lst.Add(new PropuestaPagoDTO()
                {
                    clase = EnumExtensions.GetDescription(tipoPropuestaEnum.Saldo),
                    grupo = grupo,
                    lstGrupoConca = new List<string>(),
                    nivelSuma = 1,
                    esEscondido = false,
                    desc = string.Format("Reserva en cadenas al {0:dd} {0:MMMM} {0:yyyy}", rc.Max(m => m.fecha)).ToUpper(),
                    cc = busq.ctrlCC.Select(c => new PropuestaCCDTO()
                    {
                        cc = c,
                        saldo = rc.Where(w => w.cc.Equals(c)).Sum(s => s.cargo - s.abono)
                    }).ToList()
                });
            });
            lst.Add(new PropuestaPagoDTO()
            {
                clase = EnumExtensions.GetDescription(tipoPropuestaEnum.Saldo),
                grupo = grupo,
                lstGrupoConca = new List<string>(),
                nivelSuma = 1,
                esEscondido = false,
                desc = "RECURSO",
                cc = busq.ctrlCC.Select(c => new PropuestaCCDTO()
                {
                    cc = c,
                    saldo = lstEstimaciones.Where(e => e.cc.Equals(c)).Sum(s => s.estimacion)
                }).ToList()
            });
            lst.Add(new PropuestaPagoDTO()
            {
                clase = EnumExtensions.GetDescription(tipoPropuestaEnum.Saldo),
                grupo = grupo,
                lstGrupoConca = new List<string>(),
                nivelSuma = 1,
                esEscondido = false,
                desc = "ADEUDO",
                cc = busq.ctrlCC.Select(c => new PropuestaCCDTO()
                {
                    cc = c,
                    saldo = lstEstimaciones.Where(w => w.cc.Equals(c)).Sum(s => s.estimacion + s.anticipo) + lstProv.Where(w => w.cc.Equals(c)).Sum(s => s.total) + lstReserva.Where(w => w.cc.Equals(c)).Sum(s => s.total) - lstCondensadoBanco.Where(w => w.cc.Equals(c)).Sum(s => s.total) + lstReservaCadena.GroupBy(g => g.fecha.noSemana()).Where(w => w.Max(m => m.fecha.noSemana()).Equals(w.Key)).Sum(s => s.Sum(ss => ss.cc.Equals(c) ? ss.cargo - ss.abono : 0))
                }).ToList()
            });
            lst.Add(new PropuestaPagoDTO()
            {
                clase = EnumExtensions.GetDescription(tipoPropuestaEnum.Saldo),
                grupo = grupo,
                lstGrupoConca = new List<string>(),
                nivelSuma = 1,
                esEscondido = false,
                desc = "COSTO ESTIMADO",
                cc = busq.ctrlCC.Select(c => new PropuestaCCDTO()
                {
                    cc = c,
                    saldo = lstCobEst.Where(w => w.cc.Equals(c)).Sum(s => s.estimado + s.semana1 + s.semana2 + s.semana3)
                }).ToList()
            });
            lst.Add(new PropuestaPagoDTO()
            {
                clase = EnumExtensions.GetDescription(tipoPropuestaEnum.Saldo),
                grupo = grupo,
                lstGrupoConca = new List<string>(),
                nivelSuma = 1,
                esEscondido = false,
                desc = "PRESTAMOS BANCARIOS",
                cc = busq.ctrlCC.Select(c => new PropuestaCCDTO()
                {
                    cc = c,
                    saldo = 0
                }).ToList()
            });
            lst.Add(new PropuestaPagoDTO()
            {
                clase = EnumExtensions.GetDescription(tipoPropuestaEnum.Saldo),
                grupo = grupo,
                lstGrupoConca = new List<string>(),
                nivelSuma = 1,
                esEscondido = false,
                desc = "PRONOSTICO",
                cc = busq.ctrlCC.Select(c => new PropuestaCCDTO()
                {
                    cc = c,
                    saldo = lstEstimaciones.Where(e => e.cc.Equals(c)).Sum(s => s.estimacion) + lstEstimaciones.Where(w => w.cc.Equals(c)).Sum(s => s.estimacion + s.anticipo) + lstProv.Where(w => w.cc.Equals(c)).Sum(s => s.total) + lstReserva.Where(w => w.cc.Equals(c)).Sum(s => s.total) - lstCondensadoBanco.Where(w => w.cc.Equals(c)).Sum(s => s.total) + lstReservaCadena.GroupBy(g => g.fecha.noSemana()).Where(w => w.Max(m => m.fecha.noSemana()).Equals(w.Key)).Sum(s => s.Sum(ss => ss.cc.Equals(c) ? ss.cargo - ss.abono : 0)) + lstEstimaciones.Where(w => w.cc.Equals(c)).Sum(s => s.estimacion) + lstCobEst.Where(w => w.cc.Equals(c)).Sum(s => s.estimado + s.semana1 + s.semana2 + s.semana3)
                }).ToList()
            });
            lst.Add(new PropuestaPagoDTO()
            {
                clase = EnumExtensions.GetDescription(tipoPropuestaEnum.Saldo),
                grupo = grupo,
                lstGrupoConca = new List<string>(),
                nivelSuma = 1,
                esEscondido = false,
                desc = "UTILIDADES RETENIDAS",
                cc = busq.ctrlCC.Select(c => new PropuestaCCDTO()
                {
                    cc = c,
                    saldo = lstEstimaciones.Where(w => w.cc.Equals(c)).Sum(s => s.cobrado)
                }).ToList()
            });
            lst.Add(new PropuestaPagoDTO()
            {
                clase = EnumExtensions.GetDescription(tipoPropuestaEnum.Saldo),
                grupo = grupo,
                lstGrupoConca = new List<string>(),
                nivelSuma = 1,
                esEscondido = false,
                desc = "SUB TOTAL",
                cc = busq.ctrlCC.Select(c => new PropuestaCCDTO()
                {
                    cc = c,
                    saldo = lstEstimaciones.Where(e => e.cc.Equals(c)).Sum(s => s.estimacion) + lstEstimaciones.Where(w => w.cc.Equals(c)).Sum(s => s.estimacion + s.anticipo) + lstProv.Where(w => w.cc.Equals(c)).Sum(s => s.total) + lstReserva.Where(w => w.cc.Equals(c)).Sum(s => s.total) - lstCondensadoBanco.Where(w => w.cc.Equals(c)).Sum(s => s.total) + lstReservaCadena.GroupBy(g => g.fecha.noSemana()).Where(w => w.Max(m => m.fecha.noSemana()).Equals(w.Key)).Sum(s => s.Sum(ss => ss.cc.Equals(c) ? ss.cargo - ss.abono : 0)) + lstEstimaciones.Where(w => w.cc.Equals(c)).Sum(s => s.estimacion) + lstCobEst.Where(w => w.cc.Equals(c)).Sum(s => s.estimado + s.semana1 + s.semana2 + s.semana3)
                }).ToList()
            });
            lst.Add(new PropuestaPagoDTO()
            {
                clase = EnumExtensions.GetDescription(tipoPropuestaEnum.Saldo),
                grupo = grupo,
                lstGrupoConca = new List<string>(),
                nivelSuma = 1,
                esEscondido = false,
                desc = "BANCOS ANT",
                cc = busq.ctrlCC.Select(c => new PropuestaCCDTO()
                {
                    cc = c,
                    saldo = 0
                }).ToList()
            });
            lst.Add(new PropuestaPagoDTO()
            {
                clase = EnumExtensions.GetDescription(tipoPropuestaEnum.Saldo),
                grupo = grupo,
                lstGrupoConca = new List<string>(),
                nivelSuma = 1,
                esEscondido = false,
                desc = "OBRAS EJECUTADAS SIN ANTICIPO",
                cc = busq.ctrlCC.Select(c => new PropuestaCCDTO()
                {
                    cc = c,
                    saldo = lstEstimaciones.Where(w => w.cc.Equals(c)).Sum(s => s.anticipo)
                }).ToList()
            });
            lst.Add(new PropuestaPagoDTO()
            {
                clase = EnumExtensions.GetDescription(tipoPropuestaEnum.Saldo),
                grupo = grupo,
                lstGrupoConca = new List<string>(),
                nivelSuma = 1,
                esEscondido = false,
                desc = "SUBTOTAL DISPONIBLE",
                cc = busq.ctrlCC.Select(c => new PropuestaCCDTO()
                {
                    cc = c,
                    saldo = lstEstimaciones.Where(e => e.cc.Equals(c)).Sum(s => s.estimacion) + lstEstimaciones.Where(w => w.cc.Equals(c)).Sum(s => s.estimacion + s.anticipo) + lstProv.Where(w => w.cc.Equals(c)).Sum(s => s.total) + lstReserva.Where(w => w.cc.Equals(c)).Sum(s => s.total) - lstCondensadoBanco.Where(w => w.cc.Equals(c)).Sum(s => s.total) + lstReservaCadena.GroupBy(g => g.fecha.noSemana()).Where(w => w.Max(m => m.fecha.noSemana()).Equals(w.Key)).Sum(s => s.Sum(ss => ss.cc.Equals(c) ? ss.cargo - ss.abono : 0)) + lstEstimaciones.Where(w => w.cc.Equals(c)).Sum(s => s.estimacion)
                }).ToList()
            });
            lst.Add(new PropuestaPagoDTO()
            {
                clase = EnumExtensions.GetDescription(tipoPropuestaEnum.Saldo),
                grupo = grupo,
                lstGrupoConca = new List<string>(),
                nivelSuma = 1,
                esEscondido = false,
                desc = "Acero en Almacen",
                cc = busq.ctrlCC.Select(c => new PropuestaCCDTO()
                {
                    cc = c,
                    saldo = 0
                }).ToList()
            });
            lst.Add(new PropuestaPagoDTO()
            {
                clase = EnumExtensions.GetDescription(tipoPropuestaEnum.SaldoEncabezado),
                grupo = grupo,
                lstGrupoConca = new List<string>(),
                nivelSuma = 1,
                esEscondido = false,
                desc = " TOTAL ( 1 ) ",
                cc = busq.ctrlCC.Select(c => new PropuestaCCDTO()
                {
                    cc = c,
                    saldo = lstEstimaciones.Where(e => e.cc.Equals(c)).Sum(s => s.estimacion) + lstEstimaciones.Where(w => w.cc.Equals(c)).Sum(s => s.estimacion + s.anticipo) + lstProv.Where(w => w.cc.Equals(c)).Sum(s => s.total) + lstReserva.Where(w => w.cc.Equals(c)).Sum(s => s.total) - lstCondensadoBanco.Where(w => w.cc.Equals(c)).Sum(s => s.total) + lstReservaCadena.GroupBy(g => g.fecha.noSemana()).Where(w => w.Max(m => m.fecha.noSemana()).Equals(w.Key)).Sum(s => s.Sum(ss => ss.cc.Equals(c) ? ss.cargo - ss.abono : 0)) + lstEstimaciones.Where(w => w.cc.Equals(c)).Sum(s => s.estimacion)
                }).ToList()
            });
            lst.Add(blancoReq);
            lst.Add(new PropuestaPagoDTO()
            {
                clase = EnumExtensions.GetDescription(tipoPropuestaEnum.Saldo),
                grupo = grupo,
                lstGrupoConca = new List<string>(),
                nivelSuma = 1,
                esEscondido = false,
                desc = "APORTACIONES P/ADMON",
                cc = busq.ctrlCC.Select(c => new PropuestaCCDTO()
                {
                    cc = c,
                    saldo = lstRes.Where(w => w.cc.Equals(c)).Sum(s => s.cargo - s.abono)
                }).ToList()
            });
            lst.Add(new PropuestaPagoDTO()
            {
                clase = EnumExtensions.GetDescription(tipoPropuestaEnum.Saldo),
                grupo = grupo,
                lstGrupoConca = new List<string>(),
                nivelSuma = 1,
                esEscondido = false,
                desc = "RETENCIONES X RECUPERAR",
                cc = busq.ctrlCC.Select(c => new PropuestaCCDTO()
                {
                    cc = c,
                    saldo = lstEstimaciones.Where(w => w.cc.Equals(c)).Sum(s => s.estimacion)
                }).ToList()
            });
            lst.Add(new PropuestaPagoDTO()
            {
                clase = EnumExtensions.GetDescription(tipoPropuestaEnum.Saldo),
                grupo = grupo,
                lstGrupoConca = new List<string>(),
                nivelSuma = 1,
                esEscondido = false,
                desc = "ANTICIPO A CONTRATISTAS",
                cc = busq.ctrlCC.Select(c => new PropuestaCCDTO()
                {
                    cc = c,
                    saldo = lstEstimaciones.Where(w => w.cc.Equals(c)).Sum(s => s.anticipo)
                }).ToList()
            });
            lst.Add(new PropuestaPagoDTO()
            {
                clase = EnumExtensions.GetDescription(tipoPropuestaEnum.Saldo),
                grupo = grupo,
                lstGrupoConca = new List<string>(),
                nivelSuma = 1,
                esEscondido = false,
                desc = "ANTICIPO X AMORTIZAR",
                cc = busq.ctrlCC.Select(c => new PropuestaCCDTO()
                {
                    cc = c,
                    saldo = lstEstimaciones.Where(w => w.cc.Equals(c)).Sum(s => s.cobrado)
                }).ToList()
            });
            lst.Add(new PropuestaPagoDTO()
            {
                clase = EnumExtensions.GetDescription(tipoPropuestaEnum.SaldoEncabezado),
                grupo = grupo,
                lstGrupoConca = new List<string>(),
                nivelSuma = 2,
                esEscondido = false,
                desc = "TOTAL ( 2 )",
                cc = busq.ctrlCC.Select(c => new PropuestaCCDTO()
                {
                    cc = c,
                    saldo = lstEstimaciones.Where(e => e.cc.Equals(c)).Sum(s => s.estimacion) + lstEstimaciones.Where(w => w.cc.Equals(c)).Sum(s => s.estimacion + s.anticipo) + lstProv.Where(w => w.cc.Equals(c)).Sum(s => s.total) + lstReserva.Where(w => w.cc.Equals(c)).Sum(s => s.total) - lstCondensadoBanco.Where(w => w.cc.Equals(c)).Sum(s => s.total) + lstReservaCadena.GroupBy(g => g.fecha.noSemana()).Where(w => w.Max(m => m.fecha.noSemana()).Equals(w.Key)).Sum(s => s.Sum(ss => ss.cc.Equals(c) ? ss.cargo - ss.abono : 0)) + lstEstimaciones.Where(w => w.cc.Equals(c)).Sum(s => s.estimacion) + lstReserva.Where(w => w.cc.Equals(c)).Sum(s => s.total) + lstEstimaciones.Where(w => w.cc.Equals(c)).Sum(s => s.estimacion + s.cobrado + s.anticipo)
                }).ToList()
            });
            return lst;
        }
        #endregion
        #region Concentrado
        List<ConcentradoDTO> getLstConcentrado(BusqConcentradoDTO busq)
        {
            var lstConcentrado = new List<ConcentradoDTO>();
            lstConcentrado.AddRange(cadenaToConcentrado(busq));
            lstConcentrado.AddRange(reservaSinDetalleToConcentrado(busq));
            lstConcentrado.AddRange(interesesNafinToConcentrado(busq));
            lstConcentrado.AddRange(impuestosIvaToConcentrado(busq));
            lstConcentrado.AddRange(edoCtaToConcentrado(busq));
            lstConcentrado = setReservasAutomatica(lstConcentrado, busq);
            lstConcentrado = setObra(lstConcentrado);
            lstConcentrado = calculoSaldoConciliado(lstConcentrado, busq);
            return lstConcentrado;
        }
        List<ConcentradoDTO> calculoSaldoConciliado(List<ConcentradoDTO> lst, BusqConcentradoDTO busq)
        {
            var lstConciliado = saldoFS.getSaldoConciliadoService().getLstSaldosConciliadosAnterior(busq);
            var saldo = lstConciliado.Sum(s => s.saldo);
            var iniConciliado = new ConcentradoDTO()
            {
                tipo = (int)TipoConcentradoEnum.SaldoConciliado,
                tipoReserva = 0,
                fecha = busq.min,
                beneficiario = "SALDO INICIAL CONCILIADO",
                concepto = string.Empty,
                cc = string.Empty,
                obra = string.Empty,
                noCheque = string.Empty,
                cargo = 0,
                abono = 0,
                saldo = saldo,
                sonDolares = false
            };
            lst.ForEach(c =>
            {
                saldo += c.cargo - c.abono;
                c.saldo = saldo;
            });
            var finConciliado = new ConcentradoDTO()
            {
                tipo = (int)TipoConcentradoEnum.SaldoConciliado,
                tipoReserva = 0,
                fecha = busq.max,
                beneficiario = "SALDO CORTE CONCILIADO",
                concepto = string.Empty,
                cc = string.Empty,
                obra = string.Empty,
                noCheque = string.Empty,
                cargo = 0,
                abono = 0,
                saldo = saldo,
                sonDolares = false
            };
            lst.Insert(0, iniConciliado);
            lst.Add(finConciliado);
            return lst;
        }
        List<ConcentradoDTO> saldoConciliadoToConcentrado(BusqConcentradoDTO busq)
        {
            var lstConciliado = saldoFS.getSaldoConciliadoService().getLstSaldosConciliadosAnterior(busq)
                .Select(c => new ConcentradoDTO()
                {
                    tipo = (int)TipoConcentradoEnum.SaldoConciliado,
                    fecha = c.fecha,
                    cc = c.cc,
                    obra = c.cc,
                    cargo = c.saldo
                }).ToList();
            return lstConciliado;
        }
        List<ConcentradoDTO> cadenaToConcentrado(BusqConcentradoDTO busq)
        {
            var lstConcentrado = cadenaFS.getCadenaProductivaService().getLstCadenasPagadas(busq)
                .Select(c => new ConcentradoDTO()
                {
                    tipo = (int)TipoConcentradoEnum.CadenaProductiva,
                    tipoReserva = 0,
                    fecha = c.fechaVencimiento,
                    beneficiario = "CARGO NAFIN",
                    concepto = c.proveedor.ToUpper(),
                    cc = c.centro_costos,
                    obra = c.centro_costos,
                    noCheque = string.Empty,
                    cargo = c.saldoFactura,
                    abono = c.saldoFactura,
                    saldo = 0,
                    sonDolares = c.numProveedor.ParseInt() >= 9000
                }).ToList();
            return lstConcentrado;
        }
        List<ConcentradoDTO> interesesNafinToConcentrado(BusqConcentradoDTO busq)
        {
            var lstConcentrado = cadenaFS.getCadenaProductivaService().getlstInteresesNafinDetalle(busq)
                .Select(c => new ConcentradoDTO()
                {
                    tipo = (int)TipoConcentradoEnum.InteresesFactoraje,
                    tipoReserva = 0,
                    fecha = c.fecha,
                    beneficiario = "CARGO BANCARIO",
                    concepto = "INTERESES POR FACTORAJE VENCIDO",
                    cc = c.cc,
                    obra = c.cc,
                    noCheque = string.Empty,
                    cargo = 0,
                    abono = c.interesFactoraje,
                    saldo = 0,
                    sonDolares = c.divisa.Equals(2)
                }).ToList();
            return lstConcentrado;
        }
        List<ConcentradoDTO> impuestosIvaToConcentrado(BusqConcentradoDTO busq)
        {
            var lstCarRes = reservaFS.getReservasService().getLstCatReservasActivas();
            var lstConcentrado = reservaFS.getReservasService().getLstReservaImpuestoIva(busq)
                .Where(r => !r.cc.Contains("R"))
                .Select(c => new ConcentradoDTO()
                {
                    tipo = (int)TipoConcentradoEnum.Reserva,
                    tipoReserva = c.tipo,
                    fecha = c.fecha,
                    beneficiario = c.cargo > c.abono ? "RETIRO RESERVA" : "RESERVA",
                    concepto = string.Format("{0}", lstCarRes.FirstOrDefault(r => r.id.Equals(c.tipo)).descripcion.ToUpper()),
                    cc = c.cc,
                    obra = c.cc,
                    noCheque = string.Empty,
                    cargo = c.cargo,
                    abono = c.abono,
                    saldo = 0,
                    sonDolares = false
                }).ToList();
            return lstConcentrado;
        }
        List<ConcentradoDTO> anticipoPagadoToConcentrado(BusqConcentradoDTO busq)
        {
            var lstConcentrado = cadenaFS.getCadenaProductivaService().getLstAnticipo(busq.lstCC)
                .Select(c => new ConcentradoDTO()
                {
                    tipo = (int)TipoConcentradoEnum.Anticipos,
                    tipoReserva = 0,
                    fecha = c.fechaVencimiento,
                    beneficiario = "ANTICIPO",
                    concepto = string.Format("CARGO NAFIN {0}", c.proveedor.ToUpper()),
                    cc = c.centro_costos,
                    obra = c.centro_costos,
                    noCheque = string.Empty,
                    cargo = c.anticipo,
                    abono = c.anticipo,
                    saldo = 0,
                    sonDolares = c.numProveedor.ParseInt() >= 9000
                }).ToList();
            return lstConcentrado;
        }
        List<ConcentradoDTO> reservaToConcentrado(BusqConcentradoDTO busq)
        {
            var lstCarRes = reservaFS.getReservasService().getLstCatReservasActivas();
            var lstConcentrado = reservaFS.getReservasService().getLstReservas(busq)
                .Select(c => new ConcentradoDTO()
                {
                    tipo = (int)TipoConcentradoEnum.Reserva,
                    tipoReserva = c.tipo,
                    fecha = c.fecha,
                    beneficiario = c.cargo > c.abono ? "RETIRO RESERVA" : "RESERVA",
                    concepto = string.Format("{0}", lstCarRes.FirstOrDefault(r => r.id.Equals(c.tipo)).descripcion.ToUpper()),
                    cc = c.cc,
                    obra = c.cc,
                    noCheque = string.Empty,
                    cargo = c.cargo,
                    abono = c.abono,
                    saldo = 0,
                    sonDolares = false
                }).ToList();
            return lstConcentrado;
        }
        List<ConcentradoDTO> reservaSinDetalleToConcentrado(BusqConcentradoDTO busq)
        {
            var lstCarRes = reservaFS.getReservasService().getLstCatReservasActivas();
            var lstConcentrado = reservaFS.getReservasService().getLstReservasSinDetalle(busq)
                .Select(c => new ConcentradoDTO()
                {
                    tipo = (int)TipoConcentradoEnum.Reserva,
                    tipoReserva = c.tipo,
                    fecha = c.fecha,
                    beneficiario = c.cargo > c.abono ? "RETIRO RESERVA" : "RESERVA",
                    concepto = string.Format("{0}", lstCarRes.FirstOrDefault(r => r.id.Equals(c.tipo)).descripcion.ToUpper()),
                    cc = c.cc,
                    obra = c.cc,
                    noCheque = string.Empty,
                    cargo = c.cargo * c.porcentaje,
                    abono = c.abono * c.porcentaje,
                    saldo = 0,
                    sonDolares = false
                }).ToList();
            return lstConcentrado;
        }
        List<ConcentradoDTO> edoCtaToConcentrado(BusqConcentradoDTO busq)
        {
            var lstConcentrado = chequeFS.getChequeService().getLstEdoCta(busq)
                .GroupBy(g => new { g.fecha_mov, g.numero, g.cc })
                .Select(c => new ConcentradoDTO()
                {
                    tipo = (int)TipoConcentradoEnum.EstadoCuenta,
                    tipoReserva = 0,
                    fecha = c.Key.fecha_mov,
                    beneficiario = c.FirstOrDefault().descripcion,
                    concepto = string.IsNullOrEmpty(c.FirstOrDefault().concepto) ? string.Empty : c.FirstOrDefault().concepto.ToUpper(),
                    cc = c.Key.cc,
                    obra = c.Key.cc,
                    noCheque = c.Key.numero.ToString(),
                    cargo = c.Where(w => w.tm < 49).Sum(s => s.monto * s.tc),
                    abono = -c.Where(w => w.tm > 49).Sum(s => s.monto * s.tc),
                    saldo = 0,
                    sonDolares = c.Any(a => a.tc > 1),
                    poliza = c.FirstOrDefault().poliza,
                    tp = c.FirstOrDefault().tp,
                    tm = c.FirstOrDefault().tm
                }).ToList();
            return lstConcentrado;
        }
        List<ConcentradoDTO> setReservasAutomatica(List<ConcentradoDTO> lst, BusqConcentradoDTO busq)
        {
            var tipoProrrateo = (int)TipoProrrateoReservaEnum.Seleccionado;
            var lstDet = reservaFS.getReservasService().getLstReservasDetalle(busq)
                .Where(w => busq.lstCC.Any(c => c.Equals(w.cc))).ToList();
            var lstCatRes = reservaFS.getReservasService().getLstCatReservasActivas().Where(w => w.esSeleccionado).ToList();
            var relCalculo = reservaFS.getReservasService().getRelCatResercaCalActiva()
                .Where(w => w.idTipoProrrateo.Equals(tipoProrrateo))
                .Where(w => lstCatRes.Any(a => a.id.Equals(w.idCatReserva)))
                .ToList();
            var relCc = reservaFS.getReservasService().getRelCatReservaCcActivas()
                .Where(w => w.idTipoProrrateo.Equals(tipoProrrateo))
                .Where(w => lstCatRes.Any(a => a.id.Equals(w.idCatReserva))).ToList();
            var relTm = reservaFS.getReservasService().getRelCatReservaTmActivas()
                .Where(w => w.idTipoProrrateo.Equals(tipoProrrateo))
                .Where(w => lstCatRes.Any(a => a.id.Equals(w.idCatReserva))).ToList();
            var relTp = reservaFS.getReservasService().getRelCatReservaTpActivas()
                .Where(w => w.idTipoProrrateo.Equals(tipoProrrateo))
                .Where(w => lstCatRes.Any(a => a.id.Equals(w.idCatReserva))).ToList();
            lst.Where(w => esChequeEdoCta(w)).ToList().ForEach(c =>
            {
                c.items = new List<ComboDTO>();
                var det = lstDet
                    .Where(w => w.fecha >= busq.min)
                    .Where(w => w.fecha <= busq.max)
                    .Where(w => w.tp.Equals(c.tp))
                    .Where(w => w.poliza.Equals(c.poliza))
                    .Where(w => w.cc.Equals(c.cc)).ToList();
                var lstRelCC = relCc.Where(rc => c.cc.Equals(rc.cc)).ToList();
                var lstRelTm = relTm.Where(rm => c.tm.Equals(rm.tm)).ToList();
                var lstRelTp = relTp.Where(rp => c.tp.Equals(rp.tp)).ToList();
                var lsrRelCalculo = relCalculo
                    .Where(rc => lstRelCC.Any(lc => lc.idCatReserva.Equals(rc.idCatReserva) && lc.idTipoProrrateo.Equals(rc.idTipoProrrateo)))
                    .Where(rc => lstRelTm.Any(lc => lc.idCatReserva.Equals(rc.idCatReserva) && lc.idTipoProrrateo.Equals(rc.idTipoProrrateo)))
                    .Where(rc => lstRelTp.Any(lc => lc.idCatReserva.Equals(rc.idCatReserva) && lc.idTipoProrrateo.Equals(rc.idTipoProrrateo))).ToList();
                if(lstRelCC.Count > 0 && lstRelTm.Count > 0 && lstRelTp.Count > 0 && lsrRelCalculo.Count > 0)
                {
                    c.items.AddRange(lstCatRes
                        .Where(w => lsrRelCalculo.Any(rc => rc.idCatReserva.Equals(w.id)))
                        .Where(w => lstRelCC.Any(rc => rc.idCatReserva.Equals(w.id) && rc.cc.Equals(c.cc)))
                        .Where(w => lstRelTm.Any(rc => rc.idCatReserva.Equals(w.id) && rc.tm.Equals(c.tm)))
                        .Where(w => lstRelTp.Any(rc => rc.idCatReserva.Equals(w.id) && rc.tp.Equals(c.tp)))
                        .Select(s => new ComboDTO()
                    {
                        Value = s.id.ToString(),
                        Text = s.descripcion,
                        Prefijo = JsonConvert.SerializeObject(s)
                    }));
                }
                if(det.Count > 0)
                {
                    var d = det.FirstOrDefault(w => w.numero.ToString().Equals(c.noCheque));
                    if(d != null)
                    {
                        if(lstRelTm.Any(rm => rm.tm.Equals(c.tm) && rm.idTipoProrrateo.Equals(TipoProrrateoReservaEnum.Seleccionado)))
                        {
                            c.tipoReservaAutomatica = d.tipo;
                        }
                        c.tipoReserva = d.tipo;
                    }
                    else
                    {
                        lst.Add(new ConcentradoDTO()
                        {
                            fecha = c.fecha,
                            cc = c.cc,
                            obra = c.cc,
                            noCheque = c.poliza.ToString(),
                            poliza = c.poliza,
                            tipo = (int)TipoConcentradoEnum.Reserva,
                            tipoReserva = c.tipoReserva,
                        });
                    }
                }
            });
            var lstNomina = lstDet
                .Where(d => !lst.Where(c => !string.IsNullOrEmpty(c.tp)).Any(c => c.tp.Equals("03") && c.poliza.Equals(d.poliza) && c.cc.Equals(d.cc)))
                .Select(s => new ConcentradoDTO()
                {
                    fecha = s.fecha,
                    cc = s.cc,
                    obra = s.cc,
                    noCheque = s.poliza.ToString(),
                    poliza = s.poliza,
                    cargo = s.cargo,
                    abono = s.abono,
                    tipo = (int)TipoConcentradoEnum.Reserva,
                    tipoReserva = s.tipo,
                    tipoReservaAutomatica = 0,
                    concepto = string.Format("PD-{0} {1}", s.poliza, lstCatRes.FirstOrDefault(r => r.id.Equals(s.tipo)).descripcion.ToUpper()),
                    beneficiario = lstCatRes.FirstOrDefault(r => r.id.Equals(s.tipo)).descripcion.ToUpper(),
                    saldo = s.cargo - s.abono,
                    sonDolares = false,
                    tp = s.tp
                }).ToList();
            lst.AddRange(lstNomina);
            return lst;
        }
        #endregion
        List<CcDTO> getObra()
        {
            var lst = new List<CcDTO>();
            if(lstCC == null || lstCC.Count < 10 || lst.Count < 10)
            {
                cadenaFS = new CadenaProductivaFactoryServices();
                lst = cadenaFS.getCadenaProductivaService().lstObra();
                lstCC = cadenaFS.getCadenaProductivaService().lstObra();
            }
            lst.ForEach(obra =>
            {
                var area = (TipoCCEnum)Enum.Parse(typeof(TipoCCEnum), obra.bit_area);
                var esInd = esUsuarioDivIndustrial();
                if(esInd)
                {
                    switch(area)
                    {
                        case TipoCCEnum.ObraCerradaIndustrial:
                            obra.cc = "0-2";
                            obra.descripcion = "Obra Cerrada";
                            break;
                        case TipoCCEnum.Administración:
                            obra.cc = "CIV";
                            obra.descripcion = "Civil";
                            break;
                        case TipoCCEnum.ObraCerradaGeneral:
                            obra.cc = "CIV";
                            obra.descripcion = "Civil";
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch(area)
                    {
                        case TipoCCEnum.Administración:
                            obra.cc = "000";
                            obra.descripcion = "Administración";
                            break;
                        case TipoCCEnum.ObraCerradaGeneral:
                            obra.cc = "0-1";
                            obra.descripcion = "Obra Cerrada";
                            break;
                        case TipoCCEnum.Industrial:
                            obra.cc = "IND";
                            obra.descripcion = "Industrial";
                            break;
                        case TipoCCEnum.ObraCerradaIndustrial:
                            obra.cc = "IND";
                            obra.descripcion = "Industrial";
                            break;
                        default:
                            break;
                    }
                }
                obra.descripcion = obra.descripcion.ToUpper();
            });
            return lst;
        }
        List<ConcentradoDTO> setObra(List<ConcentradoDTO> lst)
        {
            var lstRelCc = polizaFS.getPolizaService().getRelCCPropuesta();
            lst.ForEach(c =>
            {
                var cc = lstCC.FirstOrDefault(w => w.cc.Equals(c.obra));
                var area = cc.bit_area.ParseInt();
                if(area.Equals(0) ||
                    area.Equals((int)TipoCCEnum.ConstruccionPesada) ||
                    area.Equals((int)TipoCCEnum.Industrial))
                {
                    c.obra = string.Format("{0}-{1}", cc.cc, cc.descripcion);
                }
                else
                {
                    c.obra = string.Format("{0}-{1}", cc.cc, c.obra = EnumExtensions.GetDescription((TipoCCEnum)cc.bit_area.ParseInt()));
                }
                var relCc = lstRelCc.FirstOrDefault(r => r.ccSecundario.Equals(c.cc));
                if(relCc != null)
                {
                    c.cc = relCc.ccPrincipal;
                }
                c.obra = c.obra.ToUpper();
            });
            return lst.OrderBy(o => o.fecha)
                    .ThenBy(o => o.noCheque)
                    .ToList();
        }
        List<ConcentradoDTO> setObraPropuesta(List<ConcentradoDTO> lst)
        {
            var lstRelCc = polizaFS.getPolizaService().getRelCCPropuesta();
            lst.ForEach(c =>
            {
                var cc = lstCC.FirstOrDefault(w => w.cc.Equals(c.cc));
                var area = (TipoCCEnum)Enum.Parse(typeof(TipoCCEnum), cc.bit_area);
                switch(area)
                {
                    case TipoCCEnum.Administración:
                        c.cc = "000";
                        break;
                    case TipoCCEnum.ObraCerradaGeneral:
                        c.cc = "0-1";
                        break;
                    case TipoCCEnum.ObraCerradaIndustrial:
                        c.cc = "0-2";
                        break;
                    default:
                        break;
                }
                var relCc = lstRelCc.FirstOrDefault(r => r.ccSecundario.Equals(c.obra));
                if(relCc != null)
                {
                    c.cc = relCc.ccPrincipal;
                }
                c.obra = c.obra.ToUpper();
            });
            return lst.OrderBy(o => o.fecha)
                    .ThenBy(o => o.noCheque)
                    .ToList();
        }
        /// <summary>
        /// Reglas de division de CC
        /// </summary>
        /// <param name="cc">centro costos</param>
        /// <returns>nombre de la obra</returns>
        string setCC(string cc)
        {
            var esInd = esUsuarioDivIndustrial();
            var obra = lstCC.FirstOrDefault(c => c.cc.Equals(cc));
            var area = TipoCCEnum.ConstruccionPesada;
            if(obra == null)
            {
                area = esInd ? TipoCCEnum.Administración : TipoCCEnum.Industrial;
            }
            else
            {
                area = (TipoCCEnum)Enum.Parse(typeof(TipoCCEnum), obra.bit_area);
            }
            if(esInd)
            {
                switch(area)
                {
                    case TipoCCEnum.ObraCerradaIndustrial:
                        cc = "0-2";
                        break;
                    case TipoCCEnum.Administración:
                        cc = "CIV";
                        break;
                    case TipoCCEnum.ObraCerradaGeneral:
                        cc = "CIV";
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch(area)
                {
                    case TipoCCEnum.Administración:
                        cc = "000";
                        break;
                    case TipoCCEnum.ObraCerradaGeneral:
                        cc = "0-1";
                        break;
                    case TipoCCEnum.Industrial:
                        cc = "IND";
                        break;
                    case TipoCCEnum.ObraCerradaIndustrial:
                        cc = "IND";
                        break;
                    default:
                        break;
                }
            }
            return cc;
        }
        #endregion
        #region Reglas de validacion
        void init()
        {
            lstCC = cadenaFS.getCadenaProductivaService().lstObra();
            lstCC.AddRange(new List<CcDTO>()
            {
                new CcDTO()
                {
                    cc = "000",
                    bit_area = "3",
                    descripcion = "ADMINISTRACIÓN"
                },
                new CcDTO()
                {
                    cc = "0-1",
                    bit_area = "-1",
                    descripcion = "OBRA CERRADA"
                },
                new CcDTO()
                {
                    cc = "0-2",
                    bit_area = "-2",
                    descripcion = "OBRA CERRADA"
                },
            });
            lstTipoChCta = new List<int>()
            {
                (int)TipoConcentradoEnum.Cheques,
                (int)TipoConcentradoEnum.EstadoCuenta,
                (int)TipoConcentradoEnum.Nomina
            };
            lstOverhaul = EnumExtensions.ToCombo<CcOverhaul>();
            ccRitchieBros = "996";
            lstTipoRitchieBros = new List<int>()
            {
                (int)TipoConcentradoEnum.CadenaProductiva,
                (int)TipoConcentradoEnum.Cheques,
                (int)TipoConcentradoEnum.EstadoCuenta,
                (int)TipoConcentradoEnum.InteresesFactoraje,
                (int)TipoConcentradoEnum.Anticipos,
            };
        }
        bool esChequeEdoCta(ConcentradoDTO con)
        {
            return lstTipoChCta.Any(t => t.Equals(con.tipo));
        }
        bool esConcentradoOverhaul(ConcentradoDTO obj)
        {
            var esChCta = lstTipoChCta.Any(c => c.Equals(obj.tipo));
            return esChCta ? lstOverhaul.Any(o => o.Value.Equals(obj.cc.ParseInt())) : false;
        }
        bool esRitchieBros(ConcentradoDTO obj)
        {
            var esRB = lstTipoRitchieBros.Where(t => t.Equals(obj.tipo)).Any(t => obj.cc.Equals(ccRitchieBros));
            return esRB;
        }
        int setTipoNominasAutomaticas(int tipoNomina)
        {
            switch(tipoNomina)
            {
                case (int)tipoNominaPropuestaEnum.Prestamo:
                    tipoNomina = (int)tipoReservaEnum.Impuestos;
                    break;
                case (int)tipoNominaPropuestaEnum.Finiquito:
                    tipoNomina = (int)tipoReservaEnum.Impuestos;
                    break;
                case (int)tipoNominaPropuestaEnum.Imss:
                    tipoNomina = (int)tipoReservaEnum.Impuestos;
                    break;
                case (int)tipoNominaPropuestaEnum.Bono:
                    tipoNomina = (int)tipoReservaEnum.Bono;
                    break;
                case (int)tipoNominaPropuestaEnum.Aguinaldo:
                    tipoNomina = (int)tipoReservaEnum.Aguinaldo;
                    break;
                case (int)tipoNominaPropuestaEnum.ISR:
                    tipoNomina = (int)tipoReservaEnum.DeIsr;
                    break;
                case (int)tipoNominaPropuestaEnum.ISN:
                    tipoNomina = (int)tipoReservaEnum.Impuestos;
                    break;
                default:
                    tipoNomina = 0;
                    break;
            }
            return tipoNomina;
        }
        #endregion
        #region Combobox
        public ActionResult getTipoAcomulado()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, EnumExtensions.ToCombo<TipoConcentradoEnum>());
                result.Add(SUCCESS, true);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getTipoEstimacionProveedor()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, EnumExtensions.ToCombo<tipoEstimacionProveedorEnum>());
                result.Add(SUCCESS, true);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getTipoReserva()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, reservaFS.getReservasService().cboCatReserva());
                result.Add(SUCCESS, true);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getCCReservaGlobal(BusqReservaDTO busq)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstAnterior = reservaFS.getReservasService().getLstReservasAnteriores(busq);
                var catReserva = reservaFS.getReservasService().getCatReservaActiva(busq.tipo);
                var lst = filtraTipoCCDivUsuario().Select(combo => new ComboDTO()
                {
                    Text = string.Format("{0}-{1}", combo.cc, combo.descripcion),
                    Value = combo.cc,
                    Prefijo = lstAnterior.Where(anterior => anterior.cc.Equals(combo.cc)).Sum(anterior => anterior.cargo - anterior.abono).ToString()
                }).ToList();
                lst = setObra(lst);
                lst.Insert(0, new ComboDTO()
                {
                    Text = catReserva.descripcion.ToUpper(),
                    Value = string.Format("R{0:00}", busq.tipo),
                    Prefijo = lstAnterior.Sum(anterior => anterior.cargo - anterior.abono).ToString()
                });
                result.Add(ITEMS, lst);
                result.Add(SUCCESS, lst.Count > 0);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getCC()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lst = filtraTipoCCDivUsuario().Select(combo => new ComboDTO()
                {
                    Text = string.Format("{0}-{1}", combo.cc, combo.descripcion),
                    Value = combo.cc,
                }).ToList();
                lst = setObra(lst);
                result.Add(ITEMS, lst);
                result.Add(SUCCESS, lst.Count > 0);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        List<CcDTO> filtraTipoCCDivUsuario()
        {
            var lstDiv = getLstDiv();
            if(lstCC == null || lstCC.Count < 10)
            {
                cadenaFS = new CadenaProductivaFactoryServices();
                lstCC = cadenaFS.getCadenaProductivaService().lstObra();
            }
            return lstCC.Where(c => lstDiv.Any(d => c.bit_area.ParseInt().Equals(d))).ToList();
        }
        List<int> getLstDiv()
        {
            var esIndustrial = esUsuarioDivIndustrial();
            var lstDiv = new List<int>();
            if(esIndustrial)
            {
                lstDiv = new List<int>() {
                    (int)TipoCCEnum.Industrial,
                    (int)TipoCCEnum.ObraCerradaIndustrial,
                };
            }
            else
            {
                lstDiv = new List<int>() {
                    0,//para no asignados
                    (int)TipoCCEnum.Administración,
                    (int)TipoCCEnum.ConstruccionPesada,
                    (int)TipoCCEnum.ObraCerradaGeneral,
                };
            }
            return lstDiv;
        }
        List<ComboDTO> setObra(List<ComboDTO> lst)
        {
            var lstCcSobrecribir = new List<int>() 
            {
                (int)TipoCCEnum.Administración,
                (int)TipoCCEnum.ObraCerradaGeneral,
                (int)TipoCCEnum.ObraCerradaIndustrial
            };
            lst.ForEach(c =>
            {
                var cc = lstCC.FirstOrDefault(w => w.cc.Equals(c.Value));
                if(cc != null)
                {
                    var area = cc.bit_area.ParseInt();
                    if(lstCcSobrecribir.Any(s => s.Equals(area)))
                    {
                        c.Text = string.Format("{1}", area, EnumExtensions.GetDescription((TipoCCEnum)area)).ToUpper();
                        switch(area)
                        {
                            case (int)TipoCCEnum.Administración:
                                c.Value = "000";
                                break;
                            case (int)TipoCCEnum.ObraCerradaGeneral:
                                c.Value = "0-1";
                                break;
                            case (int)TipoCCEnum.ObraCerradaIndustrial:
                                c.Value = "0-2";
                                break;
                            default:
                                break;
                        }
                    }
                }
            });
            var grupo = lst
                .GroupBy(g => g.Value)
                .Select(c => new ComboDTO()
                {
                    Text = c.FirstOrDefault().Text,
                    Value = c.FirstOrDefault().Value,
                    Prefijo = c.Sum(s => s.Prefijo.ParseDecimal()).ToString()
                })
                .OrderBy(o => o.Value).ToList();
            return grupo;
        }
        public ActionResult getCbotPeriodoNomina()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lst = nominaFS.getNominaServices().getLstPeriodoNomina();
                lst.AddRange(nominaFS.getNominaServices().getLstPeriodoNominaAnt());
                lst = lst.GroupBy(x => new { x.fecha_final, x.fecha_finalStr, x.fecha_inicial, x.fecha_inicialStr, x.fecha_pago, x.fecha_pagoStr, x.mes_cc, x.periodo, x.tipo_nomina, x.tipo_periodo }).Select(y => y.First()).ToList();
                var cbo = new List<ComboGroupDTO>();
                var orden = lst.OrderByDescending(x => x.fecha_inicial).ThenByDescending(o => o.periodo).ThenBy(o => o.tipo_nomina).ToList();
                cbo.AddRange(setCboNominas(orden));
                cbo.Add(setCboNominasOtros(orden));
                result.Add(ITEMS, cbo);
                result.Add(SUCCESS, cbo.Count > 0);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getCbotPeriodoNominaAguinaldo()
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<PeriodosNominaDTO> lst = new List<PeriodosNominaDTO>();
                var anioActual = DateTime.Today.Year;
                for (int i = 2021; i <= anioActual + 1; i++)
                {
                    PeriodosNominaDTO auxLst = new PeriodosNominaDTO
                    {
                        tipo_nomina = 10,
                        periodo = 1,
                        tipo_periodo = 1,
                        fecha_inicial = new DateTime(i, 1, 1),
                        fecha_final = new DateTime(i, 12, 31),
                        fecha_pago = new DateTime(i, 12, 15),
                        mes_cc = 12,
                        fecha_inicialStr = (new DateTime(i, 1, 1)).ToShortDateString(),
                        fecha_finalStr = (new DateTime(i, 12, 31)).ToShortDateString(),
                        fecha_pagoStr = (new DateTime(i, 12, 15)).ToShortDateString(),
                    };
                    lst.Add(auxLst);
                }
                lst = lst.GroupBy(x => new { x.fecha_final, x.fecha_finalStr, x.fecha_inicial, x.fecha_inicialStr, x.fecha_pago, x.fecha_pagoStr, x.mes_cc, x.periodo, x.tipo_nomina, x.tipo_periodo }).Select(y => y.First()).ToList();
                var cbo = new List<ComboGroupDTO>();
                var orden = lst.OrderByDescending(x => x.fecha_inicial).ThenByDescending(o => o.periodo).ThenBy(o => o.tipo_nomina).ToList();
                cbo.AddRange(setCboNominas(orden));
                cbo.Add(setCboNominasOtros(orden));
                result.Add(ITEMS, cbo);
                result.Add(SUCCESS, cbo.Count > 0);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getCbotSoloPeriodoNomina()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lst = nominaFS.getNominaServices().getLstPeriodoNomina();
                var cbo = lst.OrderBy(o => o.tipo_nomina).ThenBy(o => o.fecha_inicial).ToList()
                    .Select(nom => new ComboDTO()
                    {
                        Value = string.Format("{0}-{1}-{2}-NORMAL",
                    nom.fecha_inicial.ToShortDateString(),
                    nom.fecha_final.ToShortDateString(),
                    nom.tipo_nomina,
                    "normal"),
                        Text = string.Format("{0} #{1:00} NÓMINA DEL {2:00} AL {3:00} {4} {5}",
                            EnumExtensions.GetDescription((tipoNominaPropuestaEnum)nom.tipo_nomina),
                            nom.periodo,
                            nom.fecha_inicial.Day,
                            nom.fecha_final.Day,
                            nom.fecha_final.ToString("MMMMM").ToUpper(),
                            nom.fecha_final.Year),
                        Prefijo = string.Empty,
                    }).ToList();
                result.Add(ITEMS, cbo);
                result.Add(SUCCESS, cbo.Count > 0);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        List<ComboGroupDTO> setCboNominas(List<PeriodosNominaDTO> lst)
        {
            return lst.Where(w => w.tipo_nomina < 5).GroupBy(g => g.tipo_nomina).Select(per => new ComboGroupDTO()
            {
                label = EnumExtensions.GetDescription((tipoNominaPropuestaEnum)per.Key),
                options = per.Select(nom => new ComboDTO()
                {
                    Value = string.Format("{0}-{1}-{2}-NORMAL",
                    nom.fecha_inicial.ToShortDateString(),
                    nom.fecha_final.ToShortDateString(),
                    nom.tipo_nomina,
                    "normal"),
                    Text = string.Format("{0} #{1:00} NÓMINA DEL {2:00} AL {3:00} {4} {5}",
                        EnumExtensions.GetDescription((tipoNominaPropuestaEnum)per.Key),
                        nom.periodo,
                        nom.fecha_inicial.Day,
                        nom.fecha_final.Day,
                        nom.fecha_final.ToString("MMMMM").ToUpper(),
                        nom.fecha_final.Year),
                    Prefijo = string.Empty,
                }).ToList()
            }).ToList();
        }
        ComboGroupDTO setCboNominasOtros(List<PeriodosNominaDTO> lst)
        {
            return new ComboGroupDTO()
            {
                label = "OTROS",
                options = lst.Where(w => w.tipo_nomina.Equals((int)tipoNominaPropuestaEnum.Semanal)).Select(per => new ComboDTO()
                {
                    Value = string.Format("{0}-{1}-{2}-OTROS",
                            per.fecha_inicial.ToShortDateString(),
                            per.fecha_final.ToShortDateString(),
                            5),
                    Text = string.Format("SEMANA #{0:00} OTROS DEL {1:00} AL {2:00} {3} {4}",
                        per.periodo,
                        per.fecha_inicial.Day,
                        per.fecha_final.Day,
                        per.fecha_final.ToString("MMMMM").ToUpper(),
                        per.fecha_final.Year),
                    Prefijo = string.Empty
                }).ToList()
            };
        }
        public ActionResult CargarTiposNomina()
        {
            return Json(GlobalUtils.ParseEnumToCombo<tipoNominaPropuestaEnum>(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult LlenarComboCC()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lstAc = lstCC.Select(s => new { Text = string.Format("{0}-{1}", s.cc, s.descripcion), Value = s.cc }).OrderBy(o => o.Value).ToList();
                result.Add(SUCCESS, lstAc.Count() > 0);
                result.Add(ITEMS, lstAc);
            }
            catch(Exception)
            {
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getLstAccionReservas()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cbo = new List<ComboDTO>();
                var busq = generarBusqDTO();
                var lst = reservaFS.getReservasService().getLstCatReservasActivas();
                var relCC = reservaFS.getReservasService().getRelCatReservaCcActivas();
                lst = lst.Where(w => relCC.Any(a => a.idCatReserva.Equals(w.id) && busq.lstCC.Any(c => c.Equals(a.cc)))).ToList();
                cbo.AddRange(lst.Where(w => w.esSeleccionado).Select(res => new ComboDTO
                {
                    Value = string.Format("s{0:00}", res.id),
                    Text = string.Format("Seleccionado {0}", res.descripcion),
                    Prefijo = JsonConvert.SerializeObject(res)
                }).ToList());
                cbo.AddRange(lst.Where(w => w.esAutomatico).Select(res => new ComboDTO
                {
                    Value = string.Format("a{0:00}", res.id),
                    Text = string.Format("Automático {0}", res.descripcion),
                    Prefijo = JsonConvert.SerializeObject(res)
                }).ToList());
                result.Add(ITEMS, cbo);
                result.Add(SUCCESS, cbo.Count > 0);
            }
            catch(Exception)
            {
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult cboCatReserva()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cbo = reservaFS.getReservasService().cboCatReserva();
                result.Add(ITEMS, cbo);
                result.Add(SUCCESS, cbo.Count() > 0);
            }
            catch(Exception)
            {
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult cboCatCalculoRes()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cbo = reservaFS.getReservasService().cboCatCalculoRes();
                result.Add(ITEMS, cbo);
                result.Add(SUCCESS, cbo.Count() > 0);
            }
            catch(Exception)
            {
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult cboCatResTipoSaldoGlobal()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cbo = EnumExtensions.ToCombo<TipoReservaSaldoGlobalEnum>();
                result.Add(ITEMS, cbo);
                result.Add(SUCCESS, cbo.Count() > 0);
            }
            catch(Exception)
            {
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult cboTipoOperacion()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cbo = EnumExtensions.ToCombo<tipoOperacionEnum>();
                var esSucces = cbo.Count() > 0;
                if(esSucces)
                {
                    result.Add(ITEMS, cbo);
                }
                result.Add(SUCCESS, esSucces);
            }
            catch(Exception)
            {
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getCbotipoCuentaNomina()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cbo = EnumExtensions.ToCombo<tipoCuentaNominaEnum>();
                var lstDiv = CplanDivisionesCC().Select(s => s.GetDescription().Replace(" ", string.Empty));
                var esSucces = cbo.Count() > 0;
                if(esSucces)
                {
                    result.Add(ITEMS, cbo);
                    result.Add("lstDivCC", lstDiv);
                }
                result.Add(SUCCESS, esSucces);
            }
            catch(Exception)
            {
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        List<TipoCCEnum> CplanDivisionesCC()
        {
            return new List<TipoCCEnum>
            {
                TipoCCEnum.Administración,
                TipoCCEnum.ObraCerradaGeneral,
                TipoCCEnum.Industrial,
                TipoCCEnum.ObraCerradaIndustrial,
                0
            };
        }
        #endregion
        #region Export archivos
        public MemoryStream exportConcentrado()
        {
            #region Tab Concentrado
            using(var package = new ExcelPackage())
            {
                var lstConcentrado = ((List<ConcentradoDTO>)Session["lstConcentrado"]).ToArray();
                var maxFecha = lstConcentrado.Max(c => c.fecha).ToString("yy-MM-dd");
                var titulo = string.Format("{0} {1}", Infrastructure.Utils.ExcelUtilities.NombreValidoArchivo("BANCOS AL"), maxFecha);
                var lstCatReserva = reservaFS.getReservasService().getLstCatReserva();
                var concentrado = package.Workbook.Worksheets.Add(titulo);
                var i = 4;
                concentrado.Cells["A4"].LoadFromCollection(lstConcentrado.Select(s => s.fecha.ToShortDateString()));
                concentrado.Cells["B4"].LoadFromCollection(lstConcentrado.Select(s => s.beneficiario));
                concentrado.Cells["C4"].LoadFromCollection(lstConcentrado.Select(s => s.concepto));
                concentrado.Cells["D4"].LoadFromCollection(lstConcentrado.Select(s => s.obra));
                concentrado.Cells["E4"].LoadFromCollection(lstConcentrado.Select(s => s.noCheque));
                lstConcentrado.ToList().ForEach(s => concentrado.Cells[string.Format("F{0}", i++)].Value = s.cargo);
                i = 4;
                lstConcentrado.ToList().ForEach(s => concentrado.Cells[string.Format("G{0}", i++)].Value = s.abono);
                i = 4;
                lstConcentrado.ToList().ForEach(s => concentrado.Cells[string.Format("H{0}", i++)].Value = s.saldo);

                concentrado.InsertRow(1, 1);
                concentrado.Cells["B1"].Value = getEmpresaNombre().ToUpper();
                concentrado.Cells["B2"].Value = "LIBRO DE BANCOS DE LA OBRA";
                concentrado.Cells["C2"].Value = string.Format("CONCENTRADO {0} DE LAS OBRAS", generarBusqDTO().esDivIndustrial ? "INDUSTRIAL" : "GENERAL");
                concentrado.Cells["F2"].Value = string.Format("CORTE AL {0}", maxFecha);
                concentrado.Cells["A4"].Value = "FECHA";
                concentrado.Cells["B4"].Value = "BENEFICIARIO";
                concentrado.Cells["C4"].Value = "CONCEPTO";
                concentrado.Cells["D4"].Value = "OBRA";
                concentrado.Cells["E4"].Value = "NO. CHEQUE";
                concentrado.Cells["F4"].Value = "CARGO";
                concentrado.Cells["G4"].Value = "ABONO";
                concentrado.Cells["H4"].Value = "SALDO";
                using(var rng = concentrado.Cells["B1:C2"])
                {
                    rng.Style.Font.Bold = true;
                    rng.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }
                using(var rng = concentrado.Cells["F2"])
                {
                    rng.Style.Font.Bold = true;
                    rng.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rng.Style.Fill.BackgroundColor.SetColor(0, 255, 255, 0);
                }
                using(var rng = concentrado.Cells["A4:H4"])
                {
                    rng.Style.Font.Bold = true;
                    rng.Style.Font.Color.SetColor(Color.White);
                    rng.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rng.Style.Fill.BackgroundColor.SetColor(0, 51, 51, 51);
                }
                i = 4;
                lstConcentrado.ToList().ForEach(tipo =>
                {
                    using(var rng = concentrado.Cells[string.Format("A{0}:H{0}", ++i)])
                    {
                        rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        switch(tipo.tipo)
                        {
                            case (int)TipoConcentradoEnum.SaldoConciliado:
                                rng.Style.Font.Color.SetColor(Color.Red);
                                rng.Style.Fill.BackgroundColor.SetColor(0, 255, 255, 0);
                                break;
                            case (int)TipoConcentradoEnum.CadenaProductiva:
                                rng.Style.Fill.BackgroundColor.SetColor(0, 255, 192, 0);
                                break;
                            case (int)TipoConcentradoEnum.Cheques:
                                rng.Style.Fill.BackgroundColor.SetColor(0, 175, 204, 111);
                                break;
                            case (int)TipoConcentradoEnum.InteresesFactoraje:
                                rng.Style.Fill.BackgroundColor.SetColor(0, 240, 193, 19);
                                break;
                            case (int)TipoConcentradoEnum.PolizasDiario:
                                rng.Style.Fill.BackgroundColor.SetColor(0, 216, 228, 188);
                                break;
                            case (int)TipoConcentradoEnum.EstadoCuenta:
                                rng.Style.Fill.BackgroundColor.SetColor(0, 147, 181, 245);
                                break;
                            case (int)TipoConcentradoEnum.MovimientoCliente:
                                rng.Style.Fill.BackgroundColor.SetColor(0, 181, 221, 89);
                                break;
                            case (int)TipoConcentradoEnum.Reserva:
                                var reserva = lstCatReserva.FirstOrDefault(cr => cr.id.Equals(tipo.tipoReserva));
                                var color = ColorTranslator.FromHtml(reserva.hexColor);
                                rng.Style.Fill.BackgroundColor.SetColor(color);
                                break;
                            default:
                                break;
                        }
                    }
                    if(tipo.sonDolares)
                    {
                        using(var rng = concentrado.Cells[string.Format("E{0}", i)])
                        {
                            rng.Style.Font.Color.SetColor(Color.Red);
                        }
                    }
                    using(var rng = concentrado.Cells[string.Format("E{0}:H{0}", i)])
                    {
                        rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        rng.Style.Numberformat.Format = "0.00";
                    }
                });
                concentrado.Cells[concentrado.Dimension.Address].AutoFitColumns();
                package.Compression = CompressionLevel.BestSpeed;
                List<byte[]> lista = new List<byte[]>();
                using(var exportData = new MemoryStream())
                {
                    this.Response.Clear();
                    package.SaveAs(exportData);
                    lista.Add(exportData.ToArray());
                    this.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    this.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", package.Workbook.Worksheets.FirstOrDefault().Name + ".xlsx"));
                    this.Response.BinaryWrite(exportData.ToArray());
                    this.Response.End();
                    return exportData;
                }
            }
            #endregion
        }
        public MemoryStream exportSaldosglobales()
        {
            #region Tab Saldos Globales
            using(var package = new ExcelPackage())
            {
                var lstSaldosGlobales = ((List<SaldoGlobalDTO>)Session["lstSaldosGlobales"]).ToArray();
                var busq = (BusqConcentradoDTO)Session["busqConcentrado"];
                var fecha = busq.max;
                var saldosGlobales = package.Workbook.Worksheets.Add("RESUMEN AL " + fecha.ToString("yyMMdd"));
                var i = 4;
                saldosGlobales.Cells["A4"].LoadFromCollection(lstSaldosGlobales.Select(s => s.orden > 0 ? s.orden.ToString() : string.Empty));
                saldosGlobales.Cells["B4"].LoadFromCollection(lstSaldosGlobales.Select(s => s.descripcion ?? string.Empty));
                lstSaldosGlobales.ToList().ForEach(s => saldosGlobales.Cells[string.Format("C{0}", i++)].Value = s.saldo);
                i = 4;
                lstSaldosGlobales.ToList().ForEach(s => saldosGlobales.Cells[string.Format("D{0}", i++)].Value = s.total);
                i = 4;
                lstSaldosGlobales.ToList().ForEach(s => saldosGlobales.Cells[string.Format("E{0}", i++)].Value = s.global);
                i = 4;

                saldosGlobales.InsertRow(1, 1);
                saldosGlobales.Cells["B1"].Value = getEmpresaNombre().ToUpper();
                saldosGlobales.Cells["B2"].Value = Infrastructure.Utils.ExcelUtilities.NombreValidoArchivo(string.Format("RESUMEN DE SALDO EN LIBROS DE "));
                saldosGlobales.Cells["B3"].Value = string.Format("AL {0}", fecha.ToLongDateString().ToUpper());

                using(var rng = saldosGlobales.Cells["B1:B3"])
                {
                    rng.Style.Font.Bold = true;
                    rng.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }
                i = 4;
                lstSaldosGlobales.ToList().ForEach(tipo =>
                {
                    ++i;
                    using(var rng = saldosGlobales.Cells[string.Format("C{0}:E{0}", i)])
                    {
                        rng.ToList().ForEach(celda =>
                        {
                            if(celda.GetValue<decimal>() == decimal.Zero)
                            {
                                celda.Style.Font.Color.SetColor(Color.White);
                            }
                            else
                            {
                                celda.Style.Font.Color.SetColor(Color.Black);
                            }

                        });
                    }
                    if(!string.IsNullOrEmpty(tipo.clase))
                    {
                        var clase = (TipoSaldoGlobalEnum)Enum.Parse(typeof(TipoSaldoGlobalEnum), tipo.clase);
                        using(var rng = saldosGlobales.Cells[string.Format("B{0}", i)])
                        {
                            switch(clase)
                            {
                                case TipoSaldoGlobalEnum.Descripcion:
                                    rng.Style.Font.Bold = true;
                                    break;
                                case TipoSaldoGlobalEnum.Saldo:
                                    rng.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    break;
                                case TipoSaldoGlobalEnum.SaldoTotal:
                                    rng.Style.Font.Bold = true;
                                    break;
                                case TipoSaldoGlobalEnum.Total:
                                    rng.Style.Font.Bold = true;
                                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    rng.Style.Font.Color.SetColor(Color.White);
                                    rng.Style.Fill.BackgroundColor.SetColor(Color.Black);
                                    break;
                                case TipoSaldoGlobalEnum.GranTotal:
                                    rng.Style.Font.Bold = true;
                                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    rng.Style.Font.Color.SetColor(Color.White);
                                    rng.Style.Fill.BackgroundColor.SetColor(Color.Black);
                                    break;
                                default:
                                    break;
                            }
                        }
                        using(var rng = saldosGlobales.Cells[string.Format("A{0}", i)])
                        {
                            rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        }
                        using(var rng = saldosGlobales.Cells[string.Format("C{0}:E{0}", i)])
                        {
                            rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            rng.Style.Numberformat.Format = "0.00";
                        }
                        if(clase.Equals(TipoSaldoGlobalEnum.Saldo))
                            using(var rng = saldosGlobales.Cells[string.Format("C{0}", i)])
                            {
                                rng.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            }
                        if(clase.Equals(TipoSaldoGlobalEnum.SaldoTotal))
                            using(var rng = saldosGlobales.Cells[string.Format("C{0}", i)])
                            {
                                rng.Style.Font.Bold = true;
                                rng.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                            }
                        if(clase.Equals(TipoSaldoGlobalEnum.Total))
                            using(var rng = saldosGlobales.Cells[string.Format("D{0}", i)])
                            {
                                rng.Style.Font.Bold = true;
                                rng.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                            }
                        if(clase.Equals(TipoSaldoGlobalEnum.GranTotal))
                            using(var rng = saldosGlobales.Cells[string.Format("E{0}", i)])
                            {
                                rng.Style.Font.Bold = true;
                                rng.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                            }
                    }
                });
                saldosGlobales.Cells[saldosGlobales.Dimension.Address].AutoFitColumns();
                package.Compression = CompressionLevel.BestSpeed;
                List<byte[]> lista = new List<byte[]>();
                using(var exportData = new MemoryStream())
                {
                    this.Response.Clear();
                    package.SaveAs(exportData);
                    lista.Add(exportData.ToArray());
                    this.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    this.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", package.Workbook.Worksheets.FirstOrDefault().Name + ".xlsx"));
                    this.Response.BinaryWrite(exportData.ToArray());
                    this.Response.End();
                    return exportData;
                }
            }
            #endregion
        }
        public MemoryStream exportEstimacionesResumen()
        {
            #region Tab Resumen
            using(var package = new ExcelPackage())
            {
                var lstEstRes = ((List<EstClieFacturaDTO>)Session["lstEstimacionResumen"]).ToArray();
                var fecha = (DateTime)Session["EstfechaFinal"];
                var i = 7;
                var titulo = "PRONOSTICO DE COBRANZA AL " + fecha.ToString("dd MMMM yyyy").ToUpper();
                var estimacionesResumen = package.Workbook.Worksheets.Add(ExcelUtilities.NombreValidoArchivo(titulo));

                estimacionesResumen.Cells["B2:H2"].Merge = true;
                estimacionesResumen.Cells["B3:H3"].Merge = true;
                estimacionesResumen.Cells["B4:H4"].Merge = true;
                estimacionesResumen.Cells["B5:H5"].Merge = true;

                estimacionesResumen.Cells["B2"].Value = getEmpresaNombre().ToUpper();
                estimacionesResumen.Cells["B3"].Value = string.Format("PERIFÉRICO PONIENTE 770 COL. PALO VERDE C.P. 83280");
                estimacionesResumen.Cells["B4"].Value = string.Format("HERMOSILLO, SON");
                estimacionesResumen.Cells["B5"].Value = titulo;

                estimacionesResumen.Cells["A6"].Value = string.Empty;
                estimacionesResumen.Cells["B6"].Value = "CUENTAS POR COBRAR";
                estimacionesResumen.Cells["E6"].Value = "TOTAL";
                //estimacionesResumen.Cells["F6"].Value = string.Empty;
                estimacionesResumen.Cells["G6"].Value = "PRONÓSTICOS COBRANZA";

                estimacionesResumen.Cells["A7"].LoadFromCollection(lstEstRes.Select(s => s.no > 0 ? s.no.ToString() : string.Empty));
                estimacionesResumen.Cells["B7"].LoadFromCollection(lstEstRes.Select(s => s.descripcion ?? string.Empty));
                //lstEstRes.ToList().ForEach(s => estimacionesResumen.Cells[string.Format("C{0}", i++)].Value = s.estimacion);
                //i = 7;
                //lstEstRes.ToList().ForEach(s => estimacionesResumen.Cells[string.Format("D{0}", i++)].Value = s.anticipo);
                //i = 7;
                lstEstRes.ToList().ForEach(s => estimacionesResumen.Cells[string.Format("E{0}", i++)].Value = s.vencido);
                i = 7;
                //estimacionesResumen.Cells["F7"].LoadFromCollection(lstEstRes.Select(s => string.Empty));
                lstEstRes.ToList().ForEach(s => estimacionesResumen.Cells[string.Format("G{0}", i++)].Value = s.pronostico);
                i = 7;
                //lstEstRes.ToList().ForEach(s => estimacionesResumen.Cells[string.Format("H{0}", i++)].Value = s.cobrado);
                //i = 7;

                using(var rng = estimacionesResumen.Cells["A1:H6"])
                {
                    rng.Style.Font.Bold = true;
                    rng.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                using(var rng = estimacionesResumen.Cells["A6:H6"])
                {
                    rng.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                }

                var index = 7;

                Color gray2 = Color.FromArgb(230, 230, 230);

                estimacionesResumen.Column(5).Style.Numberformat.Format = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
                estimacionesResumen.Column(7).Style.Numberformat.Format = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";

                lstEstRes.ToList().ForEach(tipo =>
                {
                    using(var rng = estimacionesResumen.Cells[string.Format("B{0}:H{0}", index)])
                    {
                        switch(tipo.clase)
                        {
                            case "suma":
                                rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                rng.Style.Font.Bold = true;
                                rng.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                                rng.Style.Font.Color.SetColor(Color.Black);
                                break;
                            case "encabezado":
                                rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                rng.Style.Border.BorderAround(ExcelBorderStyle.Medium);
                                rng.Style.Font.Bold = true;
                                rng.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                                rng.Style.Font.Color.SetColor(Color.Black);
                                using(var abc = estimacionesResumen.Cells[string.Format("B{0}", index)])
                                {
                                    abc.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                }
                                //using(var abc = estimacionesResumen.Cells[string.Format("C{0}:H{0}", index)])
                                //{
                                //    abc.Value = string.Empty;
                                //}
                                break;
                            case "subtotal":
                                rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                rng.Style.Font.Bold = true;
                                rng.Style.Border.BorderAround(ExcelBorderStyle.Medium);
                                rng.Style.Font.Color.SetColor(Color.Black);
                                rng.Style.Fill.BackgroundColor.SetColor(Color.White);
                                break;
                            case "normalCC":
                                rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                rng.Style.Font.Bold = true;
                                rng.Style.Border.BorderAround(ExcelBorderStyle.Medium);
                                rng.Style.Font.Color.SetColor(Color.Black);
                                rng.Style.Fill.BackgroundColor.SetColor(gray2);
                                break;
                            case "normalCliente":
                                rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                rng.Style.Font.Bold = true;
                                rng.Style.Border.BorderAround(ExcelBorderStyle.Medium);
                                rng.Style.Font.Color.SetColor(Color.Black);
                                rng.Style.Fill.BackgroundColor.SetColor(Color.White);
                                break;
                        }
                    }
                    index++;
                });
                using(var rng = estimacionesResumen.Cells["A6:H" + (index - 1)])
                {
                    rng.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                }
                var index2 = 7;
                lstEstRes.ToList().ForEach(tipo =>
                {
                    using(var rng = estimacionesResumen.Cells[string.Format("C{0}:H{0}", index2)])
                    {
                        rng.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    }
                    index2++;
                });

                estimacionesResumen.Cells[estimacionesResumen.Dimension.Address].AutoFitColumns();
                package.Compression = CompressionLevel.BestSpeed;
                List<byte[]> lista = new List<byte[]>();
                using(var exportData = new MemoryStream())
                {
                    this.Response.Clear();
                    package.SaveAs(exportData);
                    lista.Add(exportData.ToArray());
                    this.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    this.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", package.Workbook.Worksheets.FirstOrDefault().Name + ".xlsx"));
                    this.Response.BinaryWrite(exportData.ToArray());
                    this.Response.End();
                    return exportData;
                }
            }
            #endregion
        }
        #endregion
        #region Propuesta Proveedor
        public ActionResult ResumeProveedor()
        {
            ViewBag.Empresa = vSesiones.sesionEmpresaActual;
            return View();
        }
        public ActionResult ResumeProveedorAF()
        {
            ViewBag.Empresa = vSesiones.sesionEmpresaActual;
            return View();
        }
        public ActionResult ProgramacionPagos()
        {
            if(base.getAction("Guardar"))
            {
                ViewBag.Guardar = 1;
            }
            else
            {
                ViewBag.Guardar = 0;
            }
            ViewBag.Empresa = vSesiones.sesionEmpresaActual;
            return View();
        }
        public ActionResult ValidacionFacturas()
        {
            if (base.getAction("Guardar"))
            {
                ViewBag.Guardar = 1;
            }
            else
            {
                ViewBag.Guardar = 0;
            }
            ViewBag.Empresa = vSesiones.sesionEmpresaActual;
            return View();
        }
        public ActionResult SaldosMenores()
        {
            ViewBag.Empresa = vSesiones.sesionEmpresaActual;
            return View();
        }
        public ActionResult ReporteProgramacionPago()
        {
            ViewBag.Empresa = vSesiones.sesionEmpresaActual;
            return View();
        }
        public ActionResult _divReporteProgramacionPago()
        {
            return PartialView();
        }
        public ActionResult getLstFacturasProv(BusqPropEkDTO busq)
        {
            var result = new Dictionary<string, object>();

            Session["busq"] = busq;
            List<FacturasProvDTO> lst = new List<FacturasProvDTO>();

            if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Peru)
            {
                var lstGastosPeru = propPrpgFs.getPropProgService().getLstFacturasProvPeru(busq);
                if (lstGastosPeru.Count() > 0)
                {
                    foreach (var item in lstGastosPeru)
                    {
                        item.fechaValidacion = item.vence;
                        item.fechaTimbrado = item.vence;
                    }
                }
                var pagosFacturasPeru = propPrpgFs.getPropProgService().getFacturasPendientesPeru(lstGastosPeru);
                lst.AddRange(pagosFacturasPeru); 
            }
            else 
            {
                var lstProv = polizaFS.getPolizaService().getProveedor();
                var lstTm = polizaFS.getPolizaService().getComboTipoMovimiento("P");
                var lstGastos = propPrpgFs.getPropProgService().getLstGastosProv(busq);
                var lstPagos = propPrpgFs.getPropProgService().getLstFacturasProv(busq);
                lst = lstPagos.Select(p => new FacturasProvDTO
                {
                    id = p.id,
                    numpro = p.numpro,
                    proveedor = lstProv.FirstOrDefault(prov => prov.numpro.Equals(p.numpro)).nombre,
                    referenciaoc = p.referenciaoc,
                    cc = p.cc,
                    centroCostos = (vSesiones.sesionEmpresaActual == 1 ? lstCC.FirstOrDefault(c => c.cc.Equals(p.cc)).descripcion : p.cc),
                    tm = p.tm,
                    tmDesc = lstTm.FirstOrDefault(m => m.Value.Equals(p.tm.ToString())).Text,
                    vence = p.fecha.ToShortDateString(),
                    factura = p.factura,
                    saldo = p.total,
                    monto_plan = p.total,
                    concepto = p.concepto,
                    moneda = p.numpro < 9000 ? "MN" : "DLL",
                    autorizado = p.estatus,
                    tipocambio = p.numpro < 9000 ? 1 : p.tipocambio,
                    idGiro = lstGastos.Any(g => g.numpro.Equals(p.numpro) && g.cc.Equals(p.cc) && g.factura.Equals(p.factura)) ? lstGastos.Where(g => g.numpro.Equals(p.numpro) && g.cc.Equals(p.cc) && g.factura.Equals(p.factura)).FirstOrDefault().idGiro : 0,
                    activo_fijo = (bool)p.activo_fijo,
                    fechaTimbrado = p.fechaTimbrado.HasValue ? p.fechaTimbrado.Value.ToShortDateString() : "",
                    fechaValidacion = p.fechaValidacion.HasValue ? p.fechaValidacion.Value.ToShortDateString() : "",
                }).OrderBy(o => o.numpro).ToList();
            }
            var esSucces = lst.Count() > 0;
            if(esSucces)
            {
                //var dll = cadenaFS.getCadenaProductivaService().getDolarDelDia(busq.fechaCorte);
                var total = lst.Where(x => x.moneda.Equals("MN")).Sum(a => a.saldo);
                var totaldll = lst.Where(x => x.moneda.Equals("DLL")).Sum(a => a.saldo);

                #region Verificar bloqueo del proveedor
                if (polizaFS.getPolizaService().aplicarBloqueo())
                {
                    var subcontratistasBloqueados = polizaFS.getPolizaService().subcontratistasBloqueados();

                    if (subcontratistasBloqueados.Count > 0)
                    {
                        foreach (var gbProv in lst.GroupBy(g => g.numpro))
                        {
                            var subBloqueado = subcontratistasBloqueados.FirstOrDefault(x => x.numeroProveedor == gbProv.Key);
                            if (subBloqueado != null)
                            {
                                foreach (var item in gbProv)
                                {
                                    item.bloqueado = true;
                                    item.descripcionBloqueo = subBloqueado.descripcionTipoBloqueo;
                                }
                            }
                        }
                    }
                }
                #endregion

                result.Add("lst", lst);
                result.Add("total", total);
                result.Add("totaldll", totaldll);
            }
            result.Add(SUCCESS, esSucces);

            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }
        public ActionResult getLstFacturasProv_activofijo(BusqPropEkDTO busq)
        {
            var result = new Dictionary<string, object>();
            //try
            //{
            Session["busq"] = busq;
            var lstProv = polizaFS.getPolizaService().getProveedor();
            var lstTm = polizaFS.getPolizaService().getComboTipoMovimiento("P");
            var lstGastos = propPrpgFs.getPropProgService().getLstGastosProv(busq);
            var lstPagos = propPrpgFs.getPropProgService().getLstFacturasProv_activofijo(busq);
            var lst = lstPagos.Select(p => new FacturasProvDTO
            {
                id = p.id,
                numpro = p.numpro,
                proveedor = lstProv.FirstOrDefault(prov => prov.numpro.Equals(p.numpro)).nombre,
                referenciaoc = p.referenciaoc,
                cc = p.cc,
                centroCostos = (vSesiones.sesionEmpresaActual == 1 ? lstCC.FirstOrDefault(c => c.cc.Equals(p.cc)).descripcion : p.cc),
                tm = p.tm,
                tmDesc = lstTm.FirstOrDefault(m => m.Value.Equals(p.tm.ToString())).Text,
                vence = p.fecha.ToShortDateString(),
                factura = p.factura,
                saldo = p.total,
                monto_plan = p.monto_plan,
                concepto = p.concepto,
                moneda = p.numpro < 9000 ? "MN" : "DLL",
                autorizado = p.estatus,
                tipocambio = p.numpro < 9000 ? 1 : p.tipocambio,
                activo_fijo = true
            }).OrderBy(o => o.numpro).ToList();
            var esSucces = lst.Count() > 0;
            if (esSucces)
            {
                //var dll = cadenaFS.getCadenaProductivaService().getDolarDelDia(busq.fechaCorte);
                var total = lst.Where(x => x.moneda.Equals("MN")).Sum(a => a.monto_plan);
                var totaldll = lst.Where(x => x.moneda.Equals("DLL")).Sum(a => a.monto_plan);

                result.Add("lst", lst);
                result.Add("total", total);
                result.Add("totaldll", totaldll);
            }
            result.Add(SUCCESS, esSucces);
            //}
            //catch(Exception)
            //{
            //    result.Add(SUCCESS, false);
            //}
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }
        
        public ActionResult guardarGastosProv(List<tblC_sp_gastos_prov> lst, bool manual)
        {
            return Json(propPrpgFs.getPropProgService().guardarGastosProv(lst, manual), JsonRequestBehavior.AllowGet);
        }
        public ActionResult guardarGastosProv_AF(List<tblC_sp_gastos_prov> lst, bool manual)
        {
            var result = new Dictionary<string, object>();
            try
            {
                bool esSucces = false;
                esSucces = propPrpgFs.getPropProgService().guardarGastosProv_ActivoFijo(lst, manual);
                result.Add(SUCCESS, esSucces);
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getProgPagoRptId()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var idReporte = (int)ReportesEnum.programacionPagos;
                var esSucces = idReporte > 0;
                if(esSucces)
                {
                    result.Add("idReporte", idReporte);
                }
                result.Add(SUCCESS, esSucces);
            }
            catch(Exception)
            {
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CargarTblProgrPagos(string min, string max, List<string> cc, DateTime fecha)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lst = propPrpgFs.getPropProgService().GetListProgrPagos(min, max, cc, fecha);
                var esSucees = lst.Count > 0;
                if(esSucees)
                {
                    //var dll = cadenaFS.getCadenaProductivaService().getDolarDelDia(fecha);
                    var total = lst.Where(x => !x.tipoMoneda.Equals("DLL")).Sum(x => x.monto);
                    var totaldll = lst.Where(x => x.tipoMoneda.Equals("DLL")).Sum(x => x.monto);
                    result.Add("lst", lst);
                    result.Add("total", total);
                    result.Add("iva", lst.Where(x => !x.tipoMoneda.Equals("DLL")).Sum(x => x.iva));
                    result.Add("ivaDLLS", lst.Where(x => x.tipoMoneda.Equals("DLL")).Sum(x => x.iva));
                    result.Add("totalDll", totaldll);
                }
                result.Add(SUCCESS, esSucees);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getSaldosMenores(BusqPropEkDTO busq)
        {
            var result = new Dictionary<string, object>();
            try
            {
                Session["busq"] = busq;
                var lstProv = polizaFS.getPolizaService().getProveedor();
                var lstTm = polizaFS.getPolizaService().getComboTipoMovimiento("P");
                var lstPagos = propPrpgFs.getPropProgService().getLstFacturasSaldosMenores(busq);
                var lst = lstPagos.Select(p => new
                {
                    id = p.id,
                    numpro = p.numpro,
                    proveedor = lstProv.FirstOrDefault(prov => prov.numpro.Equals(p.numpro)).nombre,
                    referenciaoc = p.referenciaoc,
                    cc = p.cc,
                    ac = p.ac,
                    acDesc = p.acDesc,
                    centroCostos = (vSesiones.sesionEmpresaActual == 1 ? lstCC.FirstOrDefault(c => c.cc.Equals(p.cc)).descripcion : p.cc),
                    tm = p.tm,
                    tmDesc = lstTm.FirstOrDefault(m => m.Value.Equals(p.tm.ToString())).Text,
                    vence = p.fecha.ToShortDateString(),
                    factura = p.factura,
                    saldo = (busq.tipo ? p.totalMN : p.total),
                    monto_plan = (busq.tipo ? p.totalMN : p.total),
                    concepto = p.concepto,
                    moneda = (busq.tipo ? "MN":p.numpro < 9000 ? "MN" : "DLL"),
                    autorizado = p.estatus,
                    tipocambio = p.numpro < 9000 ? 1 : p.tipocambio,
                    total = (busq.tipo ? p.totalMN : p.total),
                    totalMN = p.totalMN,
                    totalCompleto = p.total + p.totalMN
                }).OrderBy(o => o.numpro).ToList();
                var esSucces = lst.Count() > 0;
                if (esSucces)
                {
                    //var dll = cadenaFS.getCadenaProductivaService().getDolarDelDia(busq.fechaCorte);
                    var total = lst.Where(x => x.moneda.Equals("MN")).Sum(a => a.saldo);
                    var totaldll = lst.Where(x => x.moneda.Equals("DLL")).Sum(a => a.saldo);
                    result.Add("lst", lst);
                    result.Add("total", total);
                    result.Add("totaldll", totaldll);
                }
                result.Add(SUCCESS, esSucces);
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }
        public ActionResult getComboProveedores()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lst = propPrpgFs.getPropProgService().getComboProveedores();
                result.Add(ITEMS, lst);
                result.Add(SUCCESS, true);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarMontosProgrPagos(List<MontoPropPagoDTO> lst, DateTime pago)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var exito = false;
                exito = propPrpgFs.getPropProgService().GuardarMontosProgrPagos(lst, pago);
                result.Add(SUCCESS, exito);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaldosMenores_GenerarPolizas(DateTime pago,string moneda,decimal total,bool tipo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var polizaID = 0;
                polizaID = propPrpgFs.getPropProgService().SaldosMenores_GenerarPolizas(pago,moneda,total,tipo);
                string poliza = pago.Year + "-" + pago.Month + "-" + polizaID + "-03";
                result.Add("poliza", poliza);
                result.Add("polizaID", polizaID);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaldosMenores_GenerarPolizas_det(List<MontoPropPagoDTO> lst, DateTime pago, int polizaID, bool tipo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var poliza = "";
                poliza = propPrpgFs.getPropProgService().SaldosMenores_GenerarPolizas_det(lst, pago, polizaID,tipo);
                result.Add("poliza", poliza);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getLstGenMovProv(BusqGenMovProvDTO busq)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lst = propPrpgFs.getPropProgService().getLstGenMovProv(busq);
                var esSucces = lst.Count > 0;
                if(esSucces)
                {
                    Session["lstRptGenMovProv"] = lst;
                    Session["busqRptGenMovProv"] = busq;
                    result.Add("lst", lst);
                }
                result.Add(SUCCESS, esSucces);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }
        public ActionResult getLimitNoProveedores()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var limit = propPrpgFs.getPropProgService().getLimitNoProveedores();
                result.Add("limit", limit);
                result.Add(SUCCESS, true);
            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public FileResult fnDownloadFile()
        {
            try
            {
                string archivo = Request.QueryString["descargar"];
                var nombre = archivo.Split('\\').Last();
                var ext = archivo.Split('.').Last();
                if (archivo.Contains("10.1.0.100"))
                {
                    archivo = "\\\\10.1.0.100\\Portal\\xml\\" + nombre;
                }
                else 
                {
                    archivo = archivo.Replace("10.1.0.125", "CONSTRUPLAN20");
                }                
                return File(archivo, "multipart/form-data", nombre);
            }
            catch(Exception)
            {

                return null;
            }

        }
        public ActionResult AbrirArchivo(string url)
        {
            var result = new Dictionary<string, object>();
            try
            {
                byte[] buffer = new byte[16 * 1024];
                byte[] aux;
                var stream = System.IO.File.Open(url, FileMode.Open);
                using(MemoryStream ms = new MemoryStream())
                {
                    int read;
                    while((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }
                    aux = ms.ToArray();
                }
                result.Add("file", aux);
                result.Add(SUCCESS, true);

            }
            catch(Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public List<tblC_sp_gastos_prov> getReportesPropuesta(DateTime fechaInicio, DateTime fechaFin, bool autorizada)
        {
            var data = propPrpgFs.getPropProgService().getReportes(fechaInicio, fechaFin, autorizada);
            return data;
        }
        #endregion
        #region Facturas duplicadas
        public ActionResult FacturaDuplicada()
        {
            return View();
        }

        [HttpGet]
        public JsonResult getDuplicados()
        {
            var r = new Dictionary<string, object>();

            try
            {
                var facturas = propPrpgFs.getPropProgService().getFacturasDuplicadas();

                var facturasAgrupadas = facturas.GroupBy(g => new { g.numpro,g.factura,g.tm, g.cc,g.referenciaoc, g.cfd_serie });

                var facturasDuplicadas = facturasAgrupadas.Where(w => w.Count() > 1);

                var duplicadas = new List<FacturaDuplicadaDTO>();

                foreach (var fd in facturasDuplicadas)
                {
                    foreach (var factura in fd)
                    {
                        var duplicada = new FacturaDuplicadaDTO();

                        duplicada.factura = factura.factura;
                        duplicada.linea = factura.linea;
                        duplicada.mes = factura.mes;
                        duplicada.numpro = factura.numpro;
                        duplicada.cc = factura.cc;
                        duplicada.referenciaoc = factura.referenciaoc;
                        duplicada.poliza = factura.poliza;
                        duplicada.serie = factura.cfd_serie;
                        duplicada.tp = factura.tp;
                        duplicada.year = factura.year;
                        duplicada.monto = factura.monto;
                        duplicada.tipoCambio = factura.tipocambio;
                        duplicada.concepto = factura.concepto;
                        duplicada.tm = factura.tm;
    
                        duplicadas.Add(duplicada);
                    }
                }

                r.Add(SUCCESS, true);
                r.Add(ITEMS, duplicadas);
            }
            catch (Exception ex)
            {
                r.Add(SUCCESS, false);
                r.Add(MESSAGE, ex.Message);
            }

            return Json(r, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Autorizacion Facturas
        public ActionResult CargarFacturasPendientes(string min, string max, List<string> cc, int tipo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lst = propPrpgFs.getPropProgService().GetFacturasPendientes(min, max, cc, tipo);
                var esSucees = lst.Count > 0;
                if (esSucees)
                {
                    //var dll = cadenaFS.getCadenaProductivaService().getDolarDelDia(fecha);
                    var total = lst.Where(x => !x.tipoMoneda.Equals("DLL")).Sum(x => x.monto);
                    var totaldll = lst.Where(x => x.tipoMoneda.Equals("DLL")).Sum(x => x.monto);
                    result.Add("lst", lst);
                    result.Add("total", total);
                    result.Add("iva", lst.Where(x => !x.tipoMoneda.Equals("DLL")).Sum(x => x.iva));
                    result.Add("ivaDLLS", lst.Where(x => x.tipoMoneda.Equals("DLL")).Sum(x => x.iva));
                    result.Add("totalDll", totaldll);
                }
                result.Add(SUCCESS, esSucees);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public FileResult GetArchivoPortal(string nombreArchivo)
        {
            try
            {

                string rutaArchivo = "\\\\10.1.0.100\\Portal\\xml\\FILECOPYPRUEBAS\\" + nombreArchivo;

#if DEBUG
                rutaArchivo = "C:\\Portal\\xml\\FILECOPYPRUEBAS\\" + nombreArchivo;
#endif

                return File(rutaArchivo, "multipart/form-data", nombreArchivo);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public FileResult GetArchivoPortalReq(string nombreArchivo)
        {
            try
            {

                string rutaArchivo = "\\\\10.1.0.100\\Portal\\Requisitos\\" + nombreArchivo;

#if DEBUG
                rutaArchivo = "C:\\Portal\\xml\\FILECOPYPRUEBAS\\" + nombreArchivo;
#endif

                return File(rutaArchivo, "multipart/form-data", nombreArchivo);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult CargarDatosArchivoPortal(string nombreArchivo)
        {
            var resultado = new Dictionary<string, object>();

            string rutaArchivo = "\\\\10.1.0.100\\Portal\\xml\\FILECOPYPRUEBAS\\" + nombreArchivo;

#if DEBUG
            rutaArchivo = "C:\\Portal\\xml\\FILECOPYPRUEBAS\\" + nombreArchivo;
#endif

            var fileData = Tuple.Create(System.IO.File.ReadAllBytes(rutaArchivo), Path.GetExtension(nombreArchivo));

            Session["archivoVisor"] = fileData;

            resultado.Add(SUCCESS, true);

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public FileResult GetRutaRequerimiento(string cc, int numero)
        {
            try
            {
                string nombreArchivo = propPrpgFs.getPropProgService().GetRutaRequerimiento(cc, numero);
                string rutaArchivo = "\\\\10.1.0.100\\Portal\\Requisitos\\" + nombreArchivo;

#if DEBUG
                rutaArchivo = "C:\\Portal\\xml\\FILECOPYPRUEBAS\\" + nombreArchivo;
#endif

                return File(rutaArchivo, "multipart/form-data", nombreArchivo);
            }
            catch (Exception)
            {
                
                return null;
            }
        }

        public ActionResult ValidarFactura(List<FiltroValidacionDTO> lstFiltro)
        {
            return Json(propPrpgFs.getPropProgService().ValidarFactura(lstFiltro), JsonRequestBehavior.AllowGet);
        }

        public ActionResult AutorizarFactura(List<FiltroValidacionDTO> lstFiltro)
        {
            return Json(propPrpgFs.getPropProgService().AutorizarFactura(lstFiltro), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDescripcionTM(int tm)
        {
            return Json(propPrpgFs.getPropProgService().GetDescripcionTM(tm), JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}