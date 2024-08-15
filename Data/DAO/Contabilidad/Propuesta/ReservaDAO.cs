using Core.DAO.Contabilidad.Propuesta;
using Core.DTO;
using Core.DTO.Contabilidad.Poliza;
using Core.DTO.Contabilidad.Propuesta;
using Core.DTO.Principal.Generales;
using Core.DTO.Utils.Data;
using Core.Entity.Administrativo.Contabilidad;
using Core.Entity.Administrativo.Contabilidad.Poliza;
using Core.Entity.Administrativo.Contabilidad.Propuesta.Acomulado;
using Core.Enum.Administracion.Propuesta;
using Core.Enum.Administracion.Propuesta.Nomina;
using Core.Enum.Multiempresa;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Data.Factory.Principal.Menus;
using Infrastructure.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Contabilidad.Propuesta
{
    public class ReservaDAO : GenericDAO<tblC_Reserva>, IReservaDAO
    {
        string nombreControlador = "Reserva";
        MenuFactoryServices mfs = new MenuFactoryServices();
        #region Guardar
        /// <summary>
        /// Guarda o actualiza las reserva
        /// </summary>
        /// <param name="lst">Reservas a guardar</param>
        /// <returns>Bandera de realizado</returns>
        public bool guardarReserva(List<ReservaDTO> lst)
        {
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                var ultimoId = 0;
                var ahora = DateTime.Now;
                var primero = lst.FirstOrDefault();
                var lstReserva = _context.tblC_Reserva.ToList()
                    .Where(r => r.fecha.Year.Equals(primero.fecha.Year))
                    .Where(r => r.fecha.noSemana().Equals(primero.fecha.noSemana()))
                    .Where(r => r.tipo.Equals(primero.tipo))
                    .ToList();
                lstReserva.ForEach(r => 
                {
                    r.esActivo = false;
                    _context.tblC_Reserva.AddOrUpdate(r);
                });
                lst.ForEach(r =>
                {
                    var guardar = lstReserva.FirstOrDefault(g => g.cc.Equals(r.cc));
                    var esNuevo = guardar == null;
                    if(esNuevo)
                        guardar = new tblC_Reserva()
                        {
                            cc = r.cc,
                            tipo = r.tipo
                        };
                    guardar.fecha = r.fecha;
                    guardar.esActivo = true;
                    guardar.ultimoCambio = ahora;
                    guardar.cargo = r.cargo;
                    guardar.abono = r.abono;
                    guardar.porcentaje = r.global;
                    _context.tblC_Reserva.AddOrUpdate(guardar);
                    SaveChanges();
                    ultimoId = guardar.id;
                });
                if(mfs.getMenuService().isLiberado(vSesiones.sesionCurrentView))
                {
                    SaveBitacora((int)BitacoraEnum.PropuestaReserva, (int)AccionEnum.AGREGAR, ultimoId, JsonUtils.convertNetObjectToJson(primero));
                    SaveChanges();
                }
                dbTransaction.Commit();
                return ultimoId > 0;
            }
        }
        /// <summary>
        /// Guarda o actualiza los detalles de ciertas reservas
        /// </summary>
        /// <param name="lst">Detalles de reservas a guardar</param>
        /// <returns>Bandera de realizado</returns>
        public bool guardarReservaDetalle(List<ConcentradoDTO> lst)
        {
            try
            {
                var ultimoId = 0;
                var primero = lst.FirstOrDefault();
                var lstReserva = _context.tblC_Reserva.ToList()
                    .Where(r => r.fecha.Year.Equals(primero.fecha.Year))
                    .Where(r => r.fecha.noSemana().Equals(primero.fecha.noSemana()))
                    .Where(r => r.tipo.Equals(primero.tipoReservaAutomatica))
                    .ToList();
                var lstDet = _context.tblC_ReservaDetalle.ToList()
                    .Where(r => r.fecha.Year.Equals(primero.fecha.Year))
                    .Where(r => r.fecha.noSemana().Equals(primero.fecha.noSemana()))
                    .Where(r => r.tipo.Equals(primero.tipoReservaAutomatica))
                    .ToList();
                lstDet.ForEach(r =>
                {
                    r.esActivo = false;
                    _context.tblC_ReservaDetalle.AddOrUpdate(r);
                });
                lst.ForEach(g =>
                {
                    var guardar = lstDet
                        .Where(d => d.tp.Equals(g.tp))
                        .Where(d => d.poliza.Equals(g.poliza))
                        .Where(d => d.numero.Equals(g.noCheque))
                        .FirstOrDefault(d => d.cc.Equals(g.cc));
                    var esNuevo = guardar == null;
                    if (esNuevo)
                        guardar = new tblC_ReservaDetalle()
                        {
                            tipo = g.tipoReserva,
                            tp = g.tp,
                            poliza = g.poliza,
                            numero = g.noCheque.ParseInt(),
                            cc = g.cc
                        };
                    var res = lstReserva.FirstOrDefault(r => r.cc.Equals(g.cc));
                    guardar.idReserva = res.id;
                    guardar.cargo = g.cargo;
                    guardar.abono = g.abono;
                    //guardar.porcentaje = res.porcentaje;
                    guardar.fecha = g.fecha;
                    guardar.numero = g.noCheque.ParseInt();
                    guardar.esActivo = true;
                    guardar.ultimoCambio = res.ultimoCambio;
                    _context.tblC_ReservaDetalle.AddOrUpdate(guardar);
                    SaveChanges();
                    ultimoId = guardar.id;
                });
                return ultimoId > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool guardarReservaNomima(List<tblC_NominaPoliza> lst)
        {
            var lstGuardado = new List<bool>();
            var lstEmpresa = EnumExtensions.ToCombo<EmpresaEnum>();
            var dicCtrl = new Dictionary<int, List<tblC_NominaPoliza>>();
            lst = lst.Where(n => n.tipoNomina > 4)
                .OrderBy(o => o.tipoCuenta).ToList();
            lstEmpresa.ForEach(emp =>
            {
                var empresa = emp.Value.ParseInt();
                var lstNomEmpresa = new List<tblC_NominaPoliza>();
                switch (empresa)
                {
                    case (int)EmpresaEnum.Construplan:
                        lstNomEmpresa = lst.Where(n => !n.tipoCuenta.Equals((int)tipoCuentaNominaEnum.Arrendadora)).ToList();
                        break;
                    case (int)EmpresaEnum.Arrendadora:
                        lstNomEmpresa = lst.Where(n => n.tipoCuenta.Equals((int)tipoCuentaNominaEnum.Arrendadora)).ToList();
                        break;
                    case (int)EmpresaEnum.Colombia:
                        lstNomEmpresa = new List<tblC_NominaPoliza>(); //Sin cuentas definidas para colombia
                        break;
                    default:
                        lstNomEmpresa = new List<tblC_NominaPoliza>();
                        break;
                }
                dicCtrl.Add(empresa, lstNomEmpresa);
            });
            dicCtrl.ToList().ForEach(emp =>
            {
                using (var _ctx = new MainContext(emp.Key))
                using (var dbTransaction = _ctx.Database.BeginTransaction())
                {
                    try
                    {
                        var lstGuardadoEmpresa = new List<bool>();
                        var ahora = DateTime.Now;
                        var primero = emp.Value.FirstOrDefault();
                        if (primero != null)
                        {
                            var min = emp.Value.Min(m => m.fecha);
                            var max = emp.Value.Max(m => m.fecha);
                            var lstReserva = _ctx.tblC_Reserva.ToList()
                            .Where(r => r.esActivo)
                            .Where(r => r.fecha >= min)
                            .Where(r => r.fecha <= max)
                            .ToList();
                            lstReserva.ForEach(r =>
                            {
                                r.esActivo = false;
                                _ctx.tblC_Reserva.AddOrUpdate(r);
                                _ctx.SaveChanges();
                            });
                            var lstGrupo = emp.Value.GroupBy(g => new { g.cc, noSemana = g.fecha.noSemana(), g.tipoNomina }).ToList();
                            lstGrupo.ForEach(nom =>
                            {
                                var tipoNomina = setTipoNominasAutomaticas(nom.Key.tipoNomina);
                                var res = _ctx.tblC_Reserva.ToList().FirstOrDefault(r => r.cc.Equals(nom.Key.cc) && r.fecha.noSemana().Equals(nom.Key.noSemana) && r.tipo.Equals(tipoNomina));
                                var esRes = res == null;
                                if (esRes)
                                {
                                    res = new tblC_Reserva()
                                    {
                                        cc = nom.Key.cc,
                                        tipo = tipoNomina,
                                        fecha = nom.Min(m => m.fecha)
                                    };
                                }
                                res.cargo = nom.Sum(s => s.cargo);
                                res.abono = nom.Sum(s => s.abono);
                                res.porcentaje = 100;
                                res.ultimoCambio = ahora;
                                res.esActivo = true;
                                _ctx.tblC_Reserva.AddOrUpdate(res);
                                _ctx.SaveChanges();
                                var esGuardado = res.id > 0;
                                lstGuardadoEmpresa.Add(esGuardado);
                                if (esGuardado)
                                {
                                    var lstDet = _ctx.tblC_ReservaDetalle.ToList()
                                            .Where(r => r.tp.Equals("03"))
                                            .Where(r => r.fecha >= min)
                                            .Where(r => r.fecha <= max)
                                            .Where(r => r.cc.Equals(nom.Key.cc))
                                            .Where(r => r.tipo.Equals(tipoNomina))
                                            .ToList();
                                    lstDet.ForEach(r =>
                                    {
                                        r.esActivo = false;
                                        _ctx.tblC_ReservaDetalle.AddOrUpdate(r);
                                        _ctx.SaveChanges();
                                    });
                                    nom.ToList().ForEach(nomDet =>
                                    {
                                        var detalle = _ctx.tblC_ReservaDetalle.ToList().FirstOrDefault(d => d.poliza.Equals(nomDet.poliza) && d.fecha.noSemana().Equals(nomDet.fecha.noSemana()));
                                        var esDet = detalle == null;
                                        if (esDet)
                                        {
                                            detalle = new tblC_ReservaDetalle()
                                            {
                                                cc = nom.Key.cc,
                                                tipo = tipoNomina,
                                                fecha = nom.Min(m => m.fecha),
                                                poliza = nomDet.poliza,
                                                numero = nomDet.poliza,
                                                tp = "03",
                                            };
                                        }
                                        detalle.idReserva = res.id;
                                        detalle.cargo = nomDet.cargo;
                                        detalle.abono = nomDet.abono;
                                        detalle.esActivo = true;
                                        detalle.ultimoCambio = ahora;
                                        _ctx.tblC_ReservaDetalle.AddOrUpdate(detalle);
                                        _ctx.SaveChanges();
                                    });
                                }
                            });
                            if (lstGuardadoEmpresa.Count > 0 && lstGuardadoEmpresa.All(a => a))
                            {
                                lstGuardado.AddRange(lstGuardadoEmpresa);
                                dbTransaction.Commit();
                            }
                            else
                            {
                                dbTransaction.Rollback();
                            }
                        }
                    }
                    catch (Exception o_O)
                    {
                        var accion = lstGuardado.Count > 0 && lstGuardado.All(a => a) ? AccionEnum.ACTUALIZAR : AccionEnum.AGREGAR;
                        var nombreFuncion = System.Reflection.MethodBase.GetCurrentMethod().Name;
                        var primero = lst.FirstOrDefault();
                        dbTransaction.Rollback();
                        LogError(vSesiones.sesionSistemaActual, vSesiones.sesionCurrentView, nombreControlador, nombreFuncion, o_O, accion, 0, primero);
                        dbTransaction.Rollback();
                    }
                }
            });
            var esSave = lstGuardado.Count > 0 && lstGuardado.All(a => a);
            return esSave;
        }
        public bool guardarCatReserva(CatReservaDTO obj)
        {
            var esGuardado = false;
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                var ahora = DateTime.Now;
                var lstCalc = _context.tblC_RelCatReservaCalculo.ToList()
                    .Where(c => c.idCatReserva.Equals(obj.catReserva.id)).ToList();
                var lstCc = _context.tblC_RelCatReservaCc.ToList()
                    .Where(c => c.idCatReserva.Equals(obj.catReserva.id)).ToList();
                var lstTm = _context.tblC_RelCatReservaTm.ToList()
                    .Where(c => c.idCatReserva.Equals(obj.catReserva.id))
                    .Where(c => obj.lstTm.Any(a => a.generado.Equals(c.generado))).ToList();
                var lstTp = _context.tblC_RelCatReservaTp.ToList()
                    .Where(c => c.idCatReserva.Equals(obj.catReserva.id)).ToList();
                lstCalc.ForEach(c =>
                    {
                        c.esActivo = false;
                        _context.tblC_RelCatReservaCalculo.AddOrUpdate(c);
                        _context.SaveChanges();
                    });
                lstCc.ForEach(c => 
                {
                    c.esActivo = false;
                    _context.tblC_RelCatReservaCc.AddOrUpdate(c);
                    _context.SaveChanges();
                });
                lstTm.ForEach(c =>
                {
                    c.esActivo = false;
                    _context.tblC_RelCatReservaTm.AddOrUpdate(c);
                    _context.SaveChanges();
                });
                lstTp.ForEach(c =>
                {
                    c.esActivo = false;
                    _context.tblC_RelCatReservaTp.AddOrUpdate(c);
                    _context.SaveChanges();
                });
                obj.catReserva.fechaRegistro = ahora;
                _context.tblC_CatReserva.AddOrUpdate(obj.catReserva);
                _context.SaveChanges();
                esGuardado = obj.catReserva.id > 0;
                if (esGuardado)
                {
                    obj.lstCalc.Where(w => w.idTipoCalculo > 0).ToList().ForEach(c =>
                    {
                        var bd = lstCalc.FirstOrDefault(b => b.idTipoCalculo.Equals(c.idTipoCalculo) && b.idTipoProrrateo.Equals(c.idTipoProrrateo));
                        if (bd == null)
                        {
                            bd = new tblC_RelCatReservaCalculo()
                            {
                                idTipoCalculo = c.idTipoCalculo,
                                idTipoProrrateo = c.idTipoProrrateo,
                                idCatReserva = obj.catReserva.id,
                                esActivo = c.esActivo
                            };
                        }
                        bd.porcentaje = c.porcentaje;
                        bd.esActivo = true;
                        bd.fechaRegistro = ahora;
                        _context.tblC_RelCatReservaCalculo.AddOrUpdate(bd);
                        _context.SaveChanges();
                    });
                    if (obj.lstCc != null)
                    obj.lstCc.Where(w => !string.IsNullOrEmpty(w.cc)).ToList().ForEach(c =>
                    {
                        var bd = lstCc.FirstOrDefault(b => b.cc.Equals(c.cc) && b.idTipoProrrateo.Equals(c.idTipoProrrateo));
                        if (bd == null)
                        {
                            bd = new tblC_RelCatReservaCc()
                            {
                                cc = c.cc,
                                idTipoProrrateo = c.idTipoProrrateo,
                                idCatReserva = obj.catReserva.id,
                                esActivo = c.esActivo
                            };
                        }
                        bd.esActivo = true;
                        bd.fechaRegistro = ahora;
                        _context.tblC_RelCatReservaCc.AddOrUpdate(bd);
                        _context.SaveChanges();
                    });
                    if (obj.lstTm != null)
                    obj.lstTm.Where(w => w.tm > 0).ToList().ForEach(c =>
                    {
                        var bd = lstTm.FirstOrDefault(b => c.tm.Equals(b.tm) && b.idTipoProrrateo.Equals(c.idTipoProrrateo));
                        if (bd == null)
                        {
                            bd = new tblC_RelCatReservaTm()
                            {
                                tm = c.tm,
                                generado = c.generado,
                                idTipoProrrateo = c.idTipoProrrateo,
                                idCatReserva = obj.catReserva.id,
                                esActivo = c.esActivo
                            };
                        }
                        bd.esActivo = true;
                        bd.fechaRegistro = ahora;
                        _context.tblC_RelCatReservaTm.AddOrUpdate(bd);
                        _context.SaveChanges();
                    });
                    if (obj.lstTp != null)
                    obj.lstTp.Where(w => !string.IsNullOrEmpty(w.tp)).ToList().ForEach(c =>
                    {
                        var bd = lstTp.FirstOrDefault(b => b.tp.Equals(c.tp) && b.idTipoProrrateo.Equals(c.idTipoProrrateo));
                        if (bd == null)
                        {
                            bd = new tblC_RelCatReservaTp()
                            {
                                tp = c.tp,
                                idTipoProrrateo = c.idTipoProrrateo,
                                idCatReserva = obj.catReserva.id,
                                esActivo = c.esActivo
                            };
                        }
                        bd.esActivo = true;
                        bd.fechaRegistro = ahora;
                        _context.tblC_RelCatReservaTp.AddOrUpdate(bd);
                        _context.SaveChanges();
                    });
                }
                dbTransaction.Commit();
            }
            return esGuardado;
        }
        #endregion
        #region Eliminar
        /// <summary>
        /// Desactiva la reservas selecionadas
        /// </summary>
        /// <param name="lst">Reservas a desactivar</param>
        /// <returns>Bandera de desactivado</returns>
        public bool ElimnarReserva(List<int> lst)
        {
            try
            {
                lst.ForEach(i =>
                {
                    var desactivar = _context.tblC_Reserva.FirstOrDefault(r => r.id.Equals(i) && r.esActivo);
                    if(desactivar != null)
                    {
                        desactivar.esActivo = false;
                        desactivar.ultimoCambio = DateTime.Now;
                        _context.tblC_Reserva.AddOrUpdate(desactivar);
                        SaveChanges();
                    }
                });
                if(mfs.getMenuService().isLiberado(vSesiones.sesionCurrentView))
                {
                    var ultimo = lst.OrderByDescending(r => r).FirstOrDefault();
                    SaveBitacora((int)BitacoraEnum.Requisicion, (int)AccionEnum.ELIMINAR, ultimo, JsonUtils.convertNetObjectToJson(ultimo));
                    SaveChanges();
                }
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
        #endregion
        #region Cadena Obra
        /// <summary>
        /// Consulta las reservas de cadena de la semana
        /// </summary>
        /// <param name="fecha">fecha de la consulta</param>
        /// <returns>Reservas de cadena</returns>
        public List<tblC_Reserva> getLstReservasCadena(DateTime fecha)
        {
            return _context.tblC_Reserva.ToList()
                .Where(r => r.esActivo)
                .Where(r => r.tipo.Equals((int)tipoReservaEnum.PagoEnCadenas))
                .Where(r => r.fecha.Year.Equals(fecha.Year))
                .Where(r => r.fecha.noSemana().Equals(fecha.noSemana()))
                .ToList();
        }
        /// <summary>
        /// Consulta las reservas de cadena de la semana
        /// </summary>
        /// <param name="fecha">fecha de la consulta</param>
        /// <returns>Reservas de cadena</returns>
        public List<tblC_Reserva> getLstReservasCadenaAnterior(DateTime fecha)
        {
            var semAntrior = new DateTime(fecha.Year, fecha.Month, fecha.Day).AddDays(-7);
            return _context.tblC_Reserva.ToList()
                .Where(r => r.esActivo)
                .Where(r => r.tipo.Equals((int)tipoReservaEnum.PagoEnCadenas))
                .Where(r => r.fecha.Year.Equals(semAntrior.Year))
                .Where(r => r.fecha.noSemana().Equals(semAntrior.noSemana()))
                .ToList();
        }
        #endregion
        /// <summary>
        /// Consulta de reservas de la semana de su tipo
        /// </summary>
        /// <param name="busq">Busqueda por reserva</param>
        /// <returns>Reservas</returns>
        public List<tblC_Reserva> getLstReservas(BusqReservaDTO busq)
        {
            return _context.tblC_Reserva.ToList()
                .Where(r => r.esActivo)
                .Where(r => r.fecha.Year.Equals(busq.fecha.Year))
                .Where(r => r.fecha.noSemana().Equals(busq.fecha.noSemana()))
                .Where(r => busq.tipo.Equals(0) ? true : r.tipo.Equals(busq.tipo))
                .ToList();
        }
        /// <summary>
        /// Consulta de reservas con los CC-R
        /// </summary>
        /// <param name="busq">Busqueda por concentrado</param>
        /// <returns>Reservas</returns>
        public List<tblC_Reserva> getLstReservasCCR(BusqConcentradoDTO busq)
        {
            return _context.tblC_Reserva.ToList()
                .Where(r => r.esActivo)
                .Where(r => r.fecha >= busq.min)
                .Where(r => r.fecha <= busq.max)
                .Where(r => r.cc.Contains("R"))
                .ToList();
        }
        /// <summary>
        /// Consulta de reservas
        /// </summary>
        /// <param name="busq">Busqueda por concentrado</param>
        /// <returns>Reservas</returns>
        public List<tblC_Reserva> getLstReservas(BusqConcentradoDTO busq)
        {
            return _context.tblC_Reserva.ToList()
                .Where(r => r.esActivo)
                .Where(r => r.fecha >= busq.min)
                .Where(r => r.fecha <= busq.max)
                .Where(r => busq.lstCC.Any(c => c.Equals(r.cc)))
                .ToList();
        }
        public List<tblC_Reserva> getLstReservasCC(BusqConcentradoDTO busq)
        {
            return _context.tblC_Reserva.ToList()
                .Where(r => r.esActivo)
                .Where(r => r.fecha >= busq.min)
                .Where(r => r.fecha <= busq.max)
                .ToList();
        }
        /// <summary>
        /// Consulta de reservas sin detalles relacionads
        /// </summary>
        /// <param name="busq">Busqueda por concentrado</param>
        /// <returns>Reservas</returns>
        public List<tblC_Reserva> getLstReservasSinDetalle(BusqConcentradoDTO busq)
        {
            var lst = getLstReservas(busq);
            var det = getLstReservasDetalle(busq);
            var lstRes = lst.Where(r => !det.Any(d => d.idReserva.Equals(r.id))).ToList();
            return lstRes;
        }
        /// <summary>
        /// Consulta de reservas de semanas anteriores
        /// </summary>
        /// <param name="busq">Busqueda de concentrado</param>
        /// <returns>Lista de reservas</returns>
        public List<tblC_Reserva> getLstReservasAnteriores(BusqConcentradoDTO busq)
        {
            var semAntrior = new DateTime(busq.min.Year, busq.min.Month, busq.min.Day);
            semAntrior = semAntrior.AddDays(-7);
            return _context.tblC_Reserva.ToList()
                .Where(r => r.esActivo)
                .Where(r => r.fecha.Year.Equals(busq.min.Year))
                .Where(r => r.fecha.noSemana().Equals(semAntrior.noSemana()))
                .Where(r => busq.lstCC.Any(c => c.Equals(r.cc)))
                .ToList();
        }
        /// <summary>
        /// Consulta de reservas de semanas anteriores
        /// </summary>
        /// <param name="busq">Busqueda de concentrado</param>
        /// <returns>Lista de reservas</returns>
        public List<tblC_Reserva> getLstReservasAnteriores(BusqReservaDTO busq)
        {
            var semAntrior = new DateTime(busq.fecha.Year, busq.fecha.Month, busq.fecha.Day);
            semAntrior = semAntrior.AddDays(-7);
            return _context.tblC_Reserva.ToList()
                .Where(r => r.esActivo)
                .Where(r => r.fecha.Year.Equals(semAntrior.Year))
                .Where(r => r.fecha.noSemana().Equals(semAntrior.noSemana()))
                .Where(r => busq.tipo.Equals(0) ? true : r.tipo.Equals(busq.tipo))
                .ToList();
        }
        /// <summary>
        /// Consulta de reservas de la semana, excepto las de su tipo
        /// </summary>
        /// <param name="busq">Busqueda de reserva</param>
        /// <returns>Lista de reservas</returns>
        public List<tblC_Reserva> getLstOtrasReservas(BusqReservaDTO busq)
        {
            return _context.tblC_Reserva.ToList()
                .Where(r => r.esActivo)
                .Where(r => r.fecha.Year.Equals(busq.fecha.Year))
                .Where(r => r.fecha.noSemana().Equals(busq.fecha.noSemana()))
                .Where(r => !r.tipo.Equals(busq.tipo)).ToList();
        }
        /// <summary>
        /// Consulta de otras reservas de semanas anteriores
        /// </summary>
        /// <param name="busq">Busqueda de reserva</param>
        /// <returns>Lista de reservas</returns>
        public List<tblC_Reserva> getLstOtrasReservasAnteriores(BusqReservaDTO busq)
        {
            return _context.tblC_Reserva.ToList()
                .Where(r => r.esActivo)
                .Where(r => r.fecha.Year <= busq.fecha.Year)
                .Where(r => r.fecha.noSemana() < busq.fecha.noSemana())
                .Where(r => !r.tipo.Equals(busq.tipo)).ToList();
        }
        public List<tblC_Reserva> getLstReservaCadenasAnteriores(DateTime min, DateTime max)
        {
            return _context.tblC_Reserva.ToList()
                .Where(r => r.esActivo)
                .Where(r => r.tipo.Equals((int)tipoReservaEnum.PagoEnCadenas))
                .Where(r => r.fecha >= min.AddDays(-7))
                .Where(r => r.fecha <= max)
                .ToList();
        }
        /// <summary>
        /// Consulta de Reservas de impuestos de iva
        /// </summary>
        /// <param name="busq">Busqueda de reservas</param>
        /// <returns>reservas de impuestos de iva</returns>
        public List<tblC_Reserva> getLstReservaImpuestoIva(BusqConcentradoDTO busq)
        {
            return _context.tblC_Reserva.ToList()
                .Where(r => r.esActivo)
                .Where(r => r.tipo.Equals((int)tipoReservaEnum.Impuestos))
                .Where(r => busq.lstCC.Any(c => c.Equals(r.cc)))
                .Where(b => b.fecha >= busq.min)
                .Where(b => b.fecha <= busq.max)
                .ToList();
        }
        /// <summary>
        /// Consulta los detalles de la semana
        /// </summary>
        /// <param name="busq">Busqueda de detalles de reservas</param>
        /// <returns>detalles de reservas</returns>
        public List<tblC_ReservaDetalle> getLstReservasDetalle(BusqConcentradoDTO busq)
        {
            return _context.tblC_ReservaDetalle.ToList()
                .Where(b => b.esActivo)
                .Where(b => b.fecha >= busq.min)
                .Where(b => b.fecha <= busq.max)
                .ToList();
        }
        public tblC_CatReserva getCatReservaActiva(int idCatReserva)
        {
            return getLstCatReservasActivas().FirstOrDefault(w => w.id.Equals(idCatReserva));
        }
        public List<tblC_CatReserva> getLstCatReserva()
        {
            return _context.tblC_CatReserva.ToList();
        }
        public List<tblC_CatReserva> getLstCatReservasActivas()
        {
            return _context.tblC_CatReserva.ToList().Where(w => w.esActivo).ToList();
        }
        public List<tblC_RelCatReservaCalculo> getRelCatResercaCalActiva()
        {
            return _context.tblC_RelCatReservaCalculo.ToList().Where(c => c.esActivo).ToList();
        }
        public List<tblC_RelCatReservaCc> getRelCatReservaCcActivas()
        {
            return _context.tblC_RelCatReservaCc.ToList().Where(c => c.esActivo).ToList();
        }
        public List<tblC_RelCatReservaTm> getRelCatReservaTmActivas()
        {
            return _context.tblC_RelCatReservaTm.ToList().Where(c => c.esActivo).ToList();
        }
        public List<tblC_RelCatReservaTp> getRelCatReservaTpActivas()
        {
            return _context.tblC_RelCatReservaTp.ToList().Where(c => c.esActivo).ToList();
        }
        public tblC_RelCatReservaCalculo getRelCatReservaCalculoActivo(int idCatReserva, int idTipoProrrateo)
        {
            return getRelCatResercaCalActiva().FirstOrDefault(w => w.idCatReserva.Equals(idCatReserva) && w.idTipoProrrateo.Equals(idTipoProrrateo));
        }
        public List<tblC_RelCatReservaCc> getRelCatReservaCcActivas(int idCatReserva, int idTipoProrrateo)
        {
            return getRelCatReservaCcActivas().Where(w => w.idCatReserva.Equals(idCatReserva) && w.idTipoProrrateo.Equals(idTipoProrrateo)).ToList();
        }
        public List<tblC_RelCatReservaTm> getRelCatReservaTmActivas(int idCatReserva, int idTipoProrrateo)
        {
            return getRelCatReservaTmActivas().Where(w => w.idCatReserva.Equals(idCatReserva) && w.idTipoProrrateo.Equals(idTipoProrrateo)).ToList();
        }
        public List<tblC_RelCatReservaTp> getRelCatReservaTpActivas(int idCatReserva, int idTipoProrrateo)
        {
            return getRelCatReservaTpActivas().Where(w => w.idCatReserva.Equals(idCatReserva) && w.idTipoProrrateo.Equals(idTipoProrrateo)).ToList();
        }
        public List<string> getLstCCEstimacion()
        {
            return _context.tblC_Reserva.ToList()
                .Where(b => b.esActivo)
                .Where(b => b.tipo.Equals((int)tipoReservaEnum.Estimacion))
                .Select(s => s.cc)
                .ToList();
        }
        /// <summary>
        /// Consulta los úlimos porcentajes activos de los cc
        /// </summary>
        /// <returns>porcentajes de los cc</returns>
        public Dictionary<string, decimal> getLstPorcentajeImpIva()
        {
            var lst = _context.tblC_Reserva.ToList()
                .Where(b => b.esActivo)
                .Where(b => b.tipo.Equals((int)tipoReservaEnum.Impuestos))
                .GroupBy(g => g.cc)
                .ToDictionary(res => res.Key, res => res.FirstOrDefault(r => r.id.Equals(res.Max(m => m.id))).porcentaje);
            return lst;
        }
        public List<ComboDTO> cboCatReserva()
        {
            var lst = getLstCatReservasActivas();
            var cbo = lst.Select(cr => new ComboDTO() 
            {
                Text = cr.descripcion,
                Value = cr.id.ToString(),
                Prefijo = JsonConvert.SerializeObject(cr)
            }).ToList();
            return cbo;
        }
        public List<ComboDTO> cboCatCalculoRes()
        {
            var lst = _context.tblC_CatCalculoCatReserva.ToList().Where(w => w.esActivo).ToList();
            var cbo = lst.Select(cr => new ComboDTO()
            {
                Value = cr.id.ToString(),
                Text = cr.descripcion
            }).ToList();
            return cbo;
        }
        int setTipoNominasAutomaticas(int tipoNomina)
        {
            switch (tipoNomina)
            {
                case (int)tipoNominaPropuestaEnum.Prestamo: tipoNomina = (int)tipoReservaEnum.Impuestos; break;
                case (int)tipoNominaPropuestaEnum.Finiquito: tipoNomina = (int)tipoReservaEnum.Impuestos; break;
                case (int)tipoNominaPropuestaEnum.Imss: tipoNomina = (int)tipoReservaEnum.Impuestos; break;
                case (int)tipoNominaPropuestaEnum.Bono: tipoNomina = (int)tipoReservaEnum.Bono; break;
                case (int)tipoNominaPropuestaEnum.Aguinaldo: tipoNomina = (int)tipoReservaEnum.Aguinaldo; break;
                case (int)tipoNominaPropuestaEnum.ISR: tipoNomina = (int)tipoReservaEnum.DeIsr; break;
                case (int)tipoNominaPropuestaEnum.ISN: tipoNomina = (int)tipoReservaEnum.Impuestos; break;
                default: tipoNomina = 0; break;
            }
            return tipoNomina;
        }
    }
}
