using Core.DAO.Contabilidad.Reportes;
using Core.DTO;
using Core.DTO.Contabilidad.FlujoEfectivo;
using Core.DTO.Contabilidad.Poliza;
using Core.DTO.Contabilidad.Propuesta.Nomina;
using Core.DTO.Principal.Generales;
using Core.DTO.Utils.Data;
using Core.Entity.Administrativo.Contabilidad.FlujoEfectivo;
using Core.Enum.Administracion.FlujoEfectivo;
using Core.Enum.Administracion.Propuesta;
using Core.Enum.Administracion.Propuesta.Nomina;
using Core.Enum.Multiempresa;
using Core.Enum.Principal;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;

namespace Data.DAO.Contabilidad.Reportes
{
    public class FlujoEfectivoArrendadoraDAO : GenericDAO<tblC_FE_MovPol>, IFlujoEfectivoArrendadora
    {
        #region init
        private readonly Dictionary<string, object> resultado = new Dictionary<string, object>();
        private static HttpSessionState session { get { return HttpContext.Current.Session; } }
        private const string nombreControlador = "FlujoEfectivoController";
        private PolizaDAO polizaDAO = new PolizaDAO();
        private List<int> lstCptoInvertirSigno;
        private List<int> lstCptoOmitir;
        void init()
        {
            lstCptoInvertirSigno = new List<int> { 26 };
            lstCptoOmitir = new List<int>() { 25, 31, 27, 28, 33 };
            lstCptoOmitir.AddRange(lstCptoInvertirSigno);
        }
        #endregion
        #region Guardar
        public bool guardarCcVisto(tblC_FED_CcVisto cc)
        {
            var esGuardado = false;
            using (var dbTransaction = _context.Database.BeginTransaction())
                try
                {
                    var bd = _context.tblC_FED_CcVisto.FirstOrDefault(v => v.anio == cc.anio && v.semana == cc.semana && v.cc == cc.cc);
                    if (bd == null)
                    {
                        bd = new tblC_FED_CcVisto()
                        {
                            anio = cc.anio,
                            semana = cc.semana,
                            cc = cc.cc,
                        };
                    }
                    bd.esVisto = cc.esVisto;
                    _context.tblC_FED_CcVisto.AddOrUpdate(bd);
                    _context.SaveChanges();
                    esGuardado = bd.id > 0;
                    if (esGuardado)
                    {
                        dbTransaction.Commit();
                    }
                    else
                    {
                        dbTransaction.Rollback();
                    }
                }
                catch (Exception o_O)
                {
                    esGuardado = false;
                }
            return esGuardado;
        }
        public bool guardarConcepto(tblC_FE_CatConcepto obj, List<tblC_FE_RelConceptoTm> rel)
        {
            var esGuardado = false;
            using (var dbTransaction = _context.Database.BeginTransaction())
                try
                {
                    var _dbConcepto = _context.tblC_FE_CatConcepto.ToList().FirstOrDefault(c => c.id == obj.id);
                    var _dbRelTm = _context.tblC_FE_RelConceptoTm.ToList().Where(r => r.idConcepto == obj.id).ToList();
                    _dbRelTm.ForEach(r =>
                    {
                        _context.tblC_FE_RelConceptoTm.Remove(r);
                        _context.SaveChanges();
                    });
                    obj.fechaRegistro = DateTime.Now;
                    _context.tblC_FE_CatConcepto.AddOrUpdate(obj);
                    _context.SaveChanges();
                    rel.ForEach(r => r.idConcepto = obj.id);
                    _context.tblC_FE_RelConceptoTm.AddRange(rel);
                    _context.SaveChanges();
                    esGuardado = obj.id > 0 && rel.All(r => r.id > 0);
                    if (esGuardado)
                    {
                        dbTransaction.Commit();
                    }
                    else
                    {
                        dbTransaction.Rollback();
                    }
                }
                catch (Exception o_O)
                {
                    esGuardado = false;
                    dbTransaction.Rollback();
                    LogError(vSesiones.sesionSistemaActual, vSesiones.sesionCurrentView, nombreControlador, "guardarConcepto", o_O, AccionEnum.ACTUALIZAR, obj.id, obj);
                }
            return esGuardado;
        }
        public bool guardarConceptoDirecto(tblC_FED_CatConcepto obj, List<tblC_FED_RelConceptoTm> rel)
        {
            var esGuardado = false;
            using (var dbTransaction = _context.Database.BeginTransaction())
                try
                {
                    var _dbConcepto = _context.tblC_FE_CatConcepto.ToList().FirstOrDefault(c => c.id == obj.id);
                    var _dbRelTm = _context.tblC_FED_RelConceptoTm.ToList().Where(r => r.idConceptoDir == obj.id).ToList();
                    _dbRelTm.ForEach(r =>
                    {
                        _context.tblC_FED_RelConceptoTm.Remove(r);
                        _context.SaveChanges();
                    });
                    obj.fechaRegistro = DateTime.Now;
                    _context.tblC_FED_CatConcepto.AddOrUpdate(obj);
                    _context.SaveChanges();
                    rel.ForEach(r => r.idConceptoDir = obj.id);
                    _context.tblC_FED_RelConceptoTm.AddRange(rel);
                    _context.SaveChanges();
                    esGuardado = obj.id > 0 && rel.All(r => r.id > 0);
                    if (esGuardado)
                    {
                        dbTransaction.Commit();
                    }
                    else
                    {
                        dbTransaction.Rollback();
                    }
                }
                catch (Exception o_O)
                {
                    esGuardado = false;
                    dbTransaction.Rollback();
                    LogError(vSesiones.sesionSistemaActual, vSesiones.sesionCurrentView, nombreControlador, "guardarConceptoDirecto", o_O, AccionEnum.ACTUALIZAR, obj.id, obj);
                }
            return esGuardado;
        }
        public bool guardarMovPol(List<tblC_FE_MovPol> lst)
        {
            var esGuardado = false;
            using (var dbTransaction = _context.Database.BeginTransaction())
                try
                {
                    var ahora = DateTime.Now;
                    var lstUpTm = new List<tblC_FE_MovPol>();
                    var busq = new BusqFlujoEfectivoDTO()
                    {
                        max = new DateTime(lst.Min(m => m.year), lst.Min(m => m.mes), 1),
                        lstCC = lst.GroupBy(g => g.cc).Select(s => s.Key).ToList(),
                        lstTm = lst.GroupBy(g => g.itm).Select(s => s.Key).ToList(),
                    };
                    var lstDb = getAllMovPol(busq);
                    lst.ForEach(mov =>
                    {
                        var _bd = lstDb.FirstOrDefault(m => m.year == mov.year && m.mes == mov.mes && m.tp == mov.tp && m.poliza == mov.poliza && m.linea == mov.linea);
                        if (_bd == null)
                        {

                        }
                        else
                        {
                            mov.id = _bd.id;
                        }
                        if (mov.itm != mov.itmOri)
                        {
                            lstUpTm.Add(mov);
                        }
                        mov.esActivo = true;
                        mov.fechaRegistro = ahora;
                        _context.tblC_FE_MovPol.AddOrUpdate(mov);
                        _context.SaveChanges();
                    });
                    esGuardado = lst.All(r => r.id > 0);
                    if (esGuardado)
                    {
                        ActualizarITipoMovimiento(lstUpTm);
                        dbTransaction.Commit();
                    }
                    else
                    {
                        dbTransaction.Rollback();
                    }
                }
                catch (Exception o_O)
                {
                    var obj = lst.FirstOrDefault();
                    esGuardado = false;
                    dbTransaction.Rollback();
                    LogError(vSesiones.sesionSistemaActual, vSesiones.sesionCurrentView, nombreControlador, "guardarMovPol", o_O, AccionEnum.ACTUALIZAR, obj.id, obj);
                }
            return esGuardado;
        }
        public bool ActualizarITipoMovimiento(List<tblC_FE_MovPol> lst)
        {
            if (lst.Count == 0)
            {
                return false;
            }
            var lstTM = polizaDAO.getComboTipoMovimiento("B");
            var lstOdbc = lst.Select(mov => new OdbcConsultaDTO()
            {
                consulta = "UPDATE sc_movpol SET itm = ?, tm = ? WHERE year = ? AND mes = ? AND tp = ? AND poliza = ? AND linea = ?",
                parametros = new List<OdbcParameterDTO>() {
                        new OdbcParameterDTO() { nombre = "itm", tipo = OdbcType.Numeric, valor = mov.itm },
                        new OdbcParameterDTO() { nombre = "tm", tipo = OdbcType.Numeric, valor = lstTM.Any(tm => tm.Value.ParseInt() == mov.itm) ? lstTM.FirstOrDefault(tm => tm.Value.ParseInt() == mov.itm).Prefijo.ParseInt() : 0 },
                        new OdbcParameterDTO() { nombre = "year", tipo = OdbcType.Numeric, valor = mov.year },
                        new OdbcParameterDTO() { nombre = "mes", tipo = OdbcType.Numeric, valor = mov.mes },
                        new OdbcParameterDTO() { nombre = "tp", tipo = OdbcType.VarChar, valor = mov.tp },
                        new OdbcParameterDTO() { nombre = "poliza", tipo = OdbcType.Numeric, valor = mov.poliza },
                        new OdbcParameterDTO() { nombre = "linea", tipo = OdbcType.Numeric, valor = mov.linea }
                    }
            }).ToList();
            var res = _contextEnkontrol.Save(EnkontrolAmbienteEnum.Prueba, lstOdbc);
            return res.Count > 0 && res.All(r => r > 1);
        }
        public List<tblC_FED_SaldoInicial> getLstSaldoInicial(int anio)
        {
            try
            {
                return _context.tblC_FED_SaldoInicial.Where(w => w.anio == anio).ToList();
            }
            catch (Exception o_O)
            {
                return new List<tblC_FED_SaldoInicial>();
            }
        }
        public List<tblC_FE_SaldoInicial> getLstFE_SaldoInicial(int anio)
        {
            return _context.tblC_FE_SaldoInicial.Where(w => w.anio == anio).ToList();
        }
        int asigaIdCptoDir(MovpolDTO mov)
        {
            var idCostoProyecto = 6;
            var idGastosOperativo = 7;
            var lstAdmin = new List<int>() { (int)TipoCCEnum.Administración, (int)TipoCCEnum.GastosFininacierosYOtros };
            var relCatDir = (List<tblC_FED_RelConceptoTm>)session["relCatDir"];
            if (relCatDir == null)
            {
                relCatDir = _context.tblC_FED_RelConceptoTm.ToList();
                session["relCatDir"] = session["relCatDir"];
            }
            var idCpto = relCatDir.Any(r => r.tm == mov.itm) ? relCatDir.FirstOrDefault(r => r.tm == mov.itm).idConceptoDir : 0;
            if (idCpto != idCostoProyecto && idCpto != idGastosOperativo)
            {
                return idCpto;
            }
            var lstCC = (List<Core.DTO.Contabilidad.CcDTO>)session["lstCC"];
            if (lstCC == null)
            {
                lstCC = new CadenaProductivaDAO().lstObra();
                session["lstCC"] = lstCC;
            }
            var cc = lstCC.FirstOrDefault(c => c.cc == mov.cc);
            if (idCpto == idCostoProyecto || idCpto == idGastosOperativo)
            {
                if (cc != null && lstAdmin.Contains(cc.bit_area.ParseInt()))
                {
                    idCpto = idGastosOperativo;
                }
                else
                {
                    idCpto = idCostoProyecto;
                }
            }
            return idCpto;
        }
        public bool guardarCorte(BusqFlujoEfectivoDTO busq)
        {
            var esGuardado = false;
            using (var dbTransaction = _context.Database.BeginTransaction())
                try
                {
                    var relCptoOpe = _context.tblC_FE_RelConceptoTm.Select(s => s.tm);
                    var relCptoDir = _context.tblC_FED_RelConceptoTm.Select(s => s.tm).ToList();
                    relCptoDir.AddRange(relCptoOpe);
                    var lstVacio = new List<string> { null, string.Empty };
                    var anio = busq.max.Year;
                    var ahora = DateTime.Now;
                    var busqFlujo = new BusqFlujoEfectivoDTO()
                    {
                        idConcepto = busq.idConcepto,
                        lstAC = getComboAreaCuenta().Select(s => s.Value).ToList(),
                        lstTm = relCptoDir.GroupBy(g => g).Select(s => s.Key).ToList(),
                        max = busq.max,
                        min = new DateTime(busq.max.Year, 1, 1),
                        tipo = busq.tipo
                    };
                    busq.lstAC = busqFlujo.lstAC;
                    busq.lstTm = busqFlujo.lstTm;
                    var lstMovFlujo = getLstMovPolCCTM(busqFlujo).AsQueryable();
                    var gpoMovFlujo = (from g in lstMovFlujo
                                       group g by new
                                       {
                                           ac = g.area + "-" + g.cuenta_oc,
                                           idCpto = asigaIdCptoDir(g)
                                       } into s
                                       select new
                                       {
                                           ac = s.Key.ac,
                                           idCpto = s.Key.idCpto,
                                           monto = s.Sum(ss => ss.monto),
                                       }).ToList();
                    var lstMov = getLstMovPolActiva(busq);
                    var lstEnk = getLstMovPol(busq).AsQueryable();
                    busqFlujo.min = busqFlujo.min.AddDays(-1);
                    var lstSaldiInicial = getLstSaldoInicialACTM(busqFlujo).AsQueryable();
                    var gpoSaldoInicial = (from g in lstSaldiInicial
                                           group g by new
                                           {
                                               ac = g.cc,
                                               idCpto = asigaIdCptoDir(g)
                                           } into s
                                           select new
                                           {
                                               ac = s.Key.ac,
                                               idCpto = s.Key.idCpto,
                                               monto = s.Sum(ss => ss.monto),
                                           }).ToList();
                    var lstSaldoInicialTodos = (from ini in _context.tblC_FED_SaldoInicial.AsQueryable()
                                                where ini.anio == anio - 1 && ini.cc == "TODOS"
                                                select new
                                                {
                                                    ac = ini.cc,
                                                    idCpto = ini.idConceptoDir,
                                                    monto = ini.saldo,
                                                });
                    var semana = busq.max.noSemana();
                    var lstPLan = from w in _context.tblC_FED_CapPlaneacion.AsQueryable()
                                  where w.anio == anio && w.semana == semana
                                  select w;
                    var acumPlan = new List<tblC_FED_CapPlaneacion>();
                    var lstCpto = getCatConceptoDirActivo();
                    var relCpto = getRelConceptoDirTm();
                    var lstCptoHijo = lstCpto.Where(w => w.idPadre > 0 && w.idPadre < 30).ToList();
                    var lstMovAC = busq.lstAC;
                    lstMovAC.Insert(0, "TODOS");
                    #region Verificacion de polizas
                    for (int i = 0; i < lstMov.Count; i++)
                    {
                        var mov = lstMov[i];
                        mov.esFlujoEfectivo = true;
                        var enk = lstEnk.FirstOrDefault(e => e.year == mov.year && e.mes == mov.mes && e.tp == mov.tp && e.poliza == mov.poliza && e.itm == mov.itm && e.cc == mov.cc && e.area.ToString() + "-" + e.cuenta_oc.ToString() == mov.ac && e.monto == mov.monto);
                        if (enk == null)
                        {
                            mov.esFlujoEfectivo = false;
                            mov.esActivo = false;
                        }
                        else
                        {
                            mov.esFlujoEfectivo = true;
                        }
                        if (i % 100 == 0)
                        {
                            _context.SaveChanges();
                        }
                    }
                    _context.SaveChanges();
                    #endregion
                    #region Flujo Efectivo
                    for (int c = 0; c < lstMovAC.Count; c++)
                    {
                        var ac = lstMovAC[c];
                        var esTodos = ac == "TODOS";
                        var movAC = from w in lstMov where w.esFlujoEfectivo && (esTodos ? true : ac == w.ac) select w;
                        var flujoAC = from w in gpoMovFlujo where esTodos ? true : ac == w.ac select w;
                        var inicialAC = from w in gpoSaldoInicial where esTodos ? false : ac == w.ac select w;
                        if (esTodos)
                        {
                            inicialAC = from w in lstSaldoInicialTodos where ac == w.ac select w;
                        }
                        #region saldos
                        for (int h = 0; h < lstCptoHijo.Count; h++)
                        {
                            var cpto = lstCptoHijo[h];
                            var movCptoAC = (from w in movAC where w.idConceptoDir == cpto.id select w).Sum(s => s.monto);
                            var flujoCptoAC = (from w in flujoAC where w.idCpto == cpto.id select w).Sum(s => s.monto);
                            var inicialCptoAC = (from w in inicialAC where w.idCpto == cpto.id select w).Sum(s => (decimal?)s.monto).GetValueOrDefault();
                            var planCptoAC = lstPLan.FirstOrDefault(w => w.idConceptoDir == cpto.id && ac == w.ac);
                            if (planCptoAC != null && movCptoAC == planCptoAC.corte && flujoCptoAC == planCptoAC.strFlujoEfectivo.ParseDecimal() && inicialCptoAC == planCptoAC.strSaldoInicial.ParseDecimal())
                            {
                                acumPlan.Add(planCptoAC);
                                continue;
                            }
                            if (planCptoAC == null)
                            {
                                planCptoAC = new tblC_FED_CapPlaneacion()
                                {
                                    idConceptoDir = cpto.id,
                                    anio = anio,
                                    semana = semana,
                                    ac = ac,
                                    cc = "TODOS",
                                    fecha = busq.max,
                                    fechaRegistro = ahora,
                                };
                            }
                            planCptoAC.esActivo = true;
                            planCptoAC.corte = movCptoAC;
                            planCptoAC.flujoTotal = flujoCptoAC;
                            planCptoAC.strFlujoEfectivo = planCptoAC.flujoTotal.ToString();
                            planCptoAC.strSaldoInicial = inicialCptoAC.ToString();
                            _context.tblC_FED_CapPlaneacion.AddOrUpdate(planCptoAC);
                            if (acumPlan.Count % 100 == 0)
                            {
                                _context.SaveChanges();
                            }
                            acumPlan.Add(planCptoAC);
                        }
                        #endregion
                    }
                    _context.SaveChanges();
                    #endregion
                    dbTransaction.Commit();
                    esGuardado = true;
                }
                catch (Exception o_O)
                {
                    esGuardado = false;
                    dbTransaction.Rollback();
                    LogError(vSesiones.sesionSistemaActual, vSesiones.sesionCurrentView, nombreControlador, "guardarCorte", o_O, AccionEnum.ACTUALIZAR, 0, busq.min);
                }
            return esGuardado;
        }
        public bool cancelarCorte(BusqFlujoEfectivoDTO busq)
        {
            var esGuardado = false;
            var consulta = new StoreProcedureDTO()
            {
                nombre = "sp_C_FED_CancelarFlujoEfectivo",
                parametros = new List<OdbcParameterDTO>()
            };
            consulta.parametros.Add(new OdbcParameterDTO() { nombre = "@anio", valor = busq.max.Year });
            consulta.parametros.Add(new OdbcParameterDTO() { nombre = "@semana", valor = busq.max.noSemana() });
            consulta.parametros.Add(new OdbcParameterDTO() { nombre = "@min", valor = busq.min });
            consulta.parametros.Add(new OdbcParameterDTO() { nombre = "@max", valor = busq.max });
            esGuardado = _context.sp_SaveUpdate(consulta);
            return esGuardado;
        }
        public bool GuardarPlaneacion(List<tblC_FED_CapPlaneacion> lst)
        {
            var esGuardado = false;
            using (var dbTransaction = _context.Database.BeginTransaction())
                try
                {
                    var ahora = DateTime.Now;
                    var fecha = lst.FirstOrDefault().fecha;
                    var lstBd = getAllPlaneacion();
                    lst.ForEach(plan =>
                    {
                        var bd = lstBd.FirstOrDefault(p => p.idConceptoDir == plan.idConceptoDir && p.anio == plan.anio && p.semana == plan.semana && p.cc == plan.cc);
                        if (bd == null)
                        {

                        }
                        else
                        {
                            plan.id = bd.id;
                        }
                        plan.esActivo = true;
                        plan.fechaRegistro = ahora;
                        _context.tblC_FED_CapPlaneacion.AddOrUpdate(plan);
                        _context.SaveChanges();
                    });
                    esGuardado = lst.All(a => a.id > 0);
                    if (esGuardado)
                    {
                        dbTransaction.Commit();
                    }
                }
                catch (Exception o_O)
                {
                    esGuardado = false;
                    dbTransaction.Rollback();
                    LogError(vSesiones.sesionSistemaActual, vSesiones.sesionCurrentView, nombreControlador, "guardarPlaneacion", o_O, AccionEnum.ACTUALIZAR, 0, lst.FirstOrDefault());
                }
            return esGuardado;
        }
        public List<tblC_FED_DetProyeccionCierre> guardarDetProyeccionCierre(List<tblC_FED_DetProyeccionCierre> lst, bool esCC)
        {
            var esGuardado = false;
            using (var dbTransaction = _context.Database.BeginTransaction())
                try
                {
                    var ahora = DateTime.Now;
                    var busq = new BusqProyeccionCierreDTO()
                    {
                        max = lst.Max(m => m.fecha),
                        lstAC = lst.GroupBy(g => g.ac).Select(s => s.Key).ToList(),
                        lstCC = lst.GroupBy(g => g.cc).Select(s => s.Key).ToList(),
                        idConceptoDir = lst.FirstOrDefault().idConceptoDir,
                        tipo = lst.FirstOrDefault().tipo,
                        esCC = esCC
                    };
                    busq.min = busq.max.AddDays(-6);
                    if (busq.tipo != tipoProyeccionCierreEnum.Manual)
                    {
                        busq.lstAC.Insert(0, "TODOS");
                    }
                    var lstBd = getLstDetProyeccionCierre(busq);
                    lst.ForEach(plan =>
                    {
                        var bd = lstBd.FirstOrDefault(p => p.id == plan.id);
                        if (bd == null)
                        {

                        }
                        else
                        {
                            plan.id = bd.id;
                        }
                        plan.fechaRegistro = ahora;
                        _context.tblC_FED_DetProyeccionCierre.AddOrUpdate(plan);
                        _context.SaveChanges();
                        if (plan.tipo == tipoProyeccionCierreEnum.Manual && lst.Count == 2)
                        {
                            var primero = lst.FirstOrDefault();
                            primero.idDetProyGemelo = primero.id + 1;
                            lst.LastOrDefault().idDetProyGemelo = primero.id;
                        }
                    });
                    if (busq.tipo != tipoProyeccionCierreEnum.Manual)
                    {
                        var lstTodos = lst.Select(s => new tblC_FED_DetProyeccionCierre()
                        {
                            id = 0,
                            idDetProyGemelo = s.id,
                            idConceptoDir = s.idConceptoDir,
                            anio = s.anio,
                            semana = s.semana,
                            ac = "TODOS",
                            cc = s.cc,
                            descripcion = s.descripcion,
                            factura = s.factura,
                            fecha = s.fecha,
                            naturaleza = s.naturaleza,
                            monto = s.monto,
                            fechaFactura = s.fechaFactura,
                            numcte = s.numcte,
                            numpro = s.numpro,
                            tipo = s.tipo,
                            esActivo = s.esActivo,
                            fechaRegistro = s.fechaRegistro,
                        }).ToList();
                        lstTodos.ForEach(plan =>
                        {
                            var bd = lstBd.FirstOrDefault(p => (p.numpro == plan.numpro || p.numcte == plan.numcte) && p.factura == plan.factura && p.cc == plan.cc && p.ac == "TODOS");
                            if (bd == null)
                            {

                            }
                            else
                            {
                                plan.id = bd.id;
                            }
                            plan.fechaRegistro = ahora;
                            _context.tblC_FED_DetProyeccionCierre.AddOrUpdate(plan);
                            _context.SaveChanges();
                            if (plan.idDetProyGemelo > 0)
                            {
                                var gemelo = lst.FirstOrDefault(w => w.id == plan.idDetProyGemelo);
                                gemelo.idDetProyGemelo = plan.id;
                                _context.tblC_FED_DetProyeccionCierre.AddOrUpdate(plan);
                                _context.SaveChanges();
                            }
                        });
                    }
                    esGuardado = lst.All(a => a.id > 0);
                    if (esGuardado)
                    {
                        esGuardado = guardarCalculoProyeccionCierre(busq);
                        if (esGuardado)
                        {
                            dbTransaction.Commit();
                        }
                    }
                }
                catch (Exception o_O)
                {
                    esGuardado = false;
                    dbTransaction.Rollback();
                    LogError(vSesiones.sesionSistemaActual, vSesiones.sesionCurrentView, nombreControlador, "guardarDetProyeccionCierre", o_O, AccionEnum.ACTUALIZAR, 0, lst.FirstOrDefault());
                }
            return lst;
        }
        public bool eliminarDetProyeccionCierre(int id)
        {
            var esGuardado = false;
            var detalle = _context.tblC_FED_DetProyeccionCierre.FirstOrDefault(w => w.id == id);
            if (detalle == null)
            {
                return true;
            }
            using (var dbTransaction = _context.Database.BeginTransaction())
                try
                {
                    detalle.esActivo = false;
                    _context.tblC_FED_DetProyeccionCierre.AddOrUpdate(detalle);
                    _context.SaveChanges();
                    var busq = new BusqProyeccionCierreDTO()
                    {
                        min = detalle.fecha.AddDays(-6),
                        max = detalle.fecha,
                        idConceptoDir = detalle.idConceptoDir,
                        lstAC = new List<string>() { detalle.ac },
                        lstCC = new List<string>() { detalle.cc },
                    };
                    if (detalle.idDetProyGemelo > 0)
                    {
                        var gem = _context.tblC_FED_DetProyeccionCierre.FirstOrDefault(w => w.id == detalle.idDetProyGemelo);
                        gem.esActivo = false;
                        _context.tblC_FED_DetProyeccionCierre.AddOrUpdate(gem);
                        _context.SaveChanges();
                        busq.lstAC.Add(gem.ac);
                        busq.lstCC.Add(gem.cc);
                    }
                    esGuardado = guardarCalculoProyeccionCierre(busq);
                    if (esGuardado)
                    {
                        dbTransaction.Commit();
                    }
                }
                catch (Exception o_O)
                {
                    esGuardado = false;
                    dbTransaction.Rollback();
                    LogError(vSesiones.sesionSistemaActual, vSesiones.sesionCurrentView, nombreControlador, "eliminarDetProyeccionCierre", o_O, AccionEnum.ELIMINAR, id, detalle);
                }
            return esGuardado;
        }
        bool guardarCalculoProyeccionCierre(BusqProyeccionCierreDTO busq)
        {
            var esGuardado = false;
            for (int i = 0; i < 2; i++)
            {
                var lstBd = getLstDetProyeccionCierre(busq);
                var lstPlan = getPlaneacionCierre(busq).Where(w => w.idConceptoDir == busq.idConceptoDir).ToList();
                var lstObra = busq.esCC ? busq.lstCC : busq.lstAC;
                var semana = busq.max.noSemana();
                lstObra.ForEach(obra =>
                {
                    var plan = lstPlan.FirstOrDefault(p => busq.esCC ? p.cc == obra : p.ac == obra);
                    if (plan == null)
                    {
                        plan = new tblC_FED_CapPlaneacion()
                        {
                            idConceptoDir = busq.idConceptoDir,
                            anio = busq.max.Year,
                            fecha = busq.max,
                            semana = semana,
                            esActivo = true,
                        };
                        if (busq.esCC)
                        {
                            plan.cc = obra;
                            plan.ac = lstBd.Any(w => w.cc == obra) ? lstBd.FirstOrDefault(w => w.cc == obra).ac : "TODOS";
                        }
                        else
                        {
                            plan.ac = obra;
                            plan.cc = lstBd.Any(w => w.ac == obra) ? lstBd.FirstOrDefault(w => w.ac == obra).cc : "N/A";
                        }
                    }
                    plan.fechaRegistro = DateTime.Now;
                    plan.corte = lstBd.Where(w => busq.esCC ? w.cc == obra : w.ac == obra).Sum(s => s.monto);
                    _context.tblC_FED_CapPlaneacion.AddOrUpdate(p => p.id, plan);
                    _context.SaveChanges();
                    esGuardado = plan.id > 0;
                });
                busq.esCC = !busq.esCC;
            }
            return esGuardado;
        }
        public bool GuardarGpoReserva(tblC_FED_CatGrupoReserva gpo)
        {
            var esGuardado = false;
            using (var dbTransaction = _context.Database.BeginTransaction())
                try
                {
                    gpo.fechaRegistro = DateTime.Now;
                    _context.tblC_FED_CatGrupoReserva.AddOrUpdate(gpo);
                    _context.SaveChanges();
                    esGuardado = gpo.id > 0;
                    if (esGuardado)
                    {
                        dbTransaction.Commit();
                    }
                }
                catch (Exception o_O)
                {
                    esGuardado = false;
                    dbTransaction.Rollback();
                    LogError(vSesiones.sesionSistemaActual, vSesiones.sesionCurrentView, nombreControlador, "GuardarGpoReserva", o_O, AccionEnum.AGREGAR, gpo.id, gpo);
                }
            return esGuardado;
        }
        bool actualizarMovPol()
        {
            var esGuardado = false;
            using (var dbTransaction = _context.Database.BeginTransaction())
                try
                {
                    var ahora = DateTime.Now;
                    var idCostoProyecto = 6;
                    var idGastosOperativo = 7;
                    var lstCC = new CadenaProductivaDAO().lstObra();
                    var relCatDir = _context.tblC_FED_RelConceptoTm.Where(w => w.idConceptoDir == 10).ToList();
                    var relCat = _context.tblC_FE_RelConceptoTm.ToList();
                    var lstTM = relCatDir.GroupBy(g => g.tm).Select(s => s.Key).ToList();
                    var bdMovPol = _context.tblC_FE_MovPol.ToList();
                    var busq = new BusqFlujoEfectivoDTO()
                    {
                        tipo = tipoBusqueda.Todos,
                        lstCC = lstCC.Select(s => s.cc).ToList(),
                        lstTm = lstTM,
                        min = new DateTime(2020, 4, 12),
                        max = new DateTime(2020, 4, 18)
                    };
                    var lstMovPol = getLstMovPol(busq);
                    var sigMovPol = lstMovPol.Select(m => new tblC_FE_MovPol()
                    {
                        year = m.year,
                        mes = m.mes,
                        tp = m.tp,
                        poliza = m.poliza,
                        linea = m.linea,
                        tm = m.tm,
                        itm = m.itm,
                        itmOri = m.itm,
                        cta = m.cta,
                        scta = m.scta,
                        sscta = m.sscta,
                        fechapol = m.fechapol,
                        cc = m.cc,
                        concepto = m.concepto,
                        numpro = m.numpro.ParseInt(),
                        monto = m.monto,
                        idConceptoDir = relCatDir.FirstOrDefault(r => r.tm == m.itm).idConceptoDir,
                        idConcepto = relCat.Any(r => r.tm == m.itm) ? relCat.FirstOrDefault(r => r.tm == m.itm).idConcepto : 0,
                        fechaRegistro = ahora,
                        esFlujoEfectivo = true,
                        esActivo = true,
                    }).ToList();
                    sigMovPol.ForEach(m =>
                    {
                        var cc = lstCC.FirstOrDefault(c => c.cc == m.cc);
                        if (m.idConceptoDir == idCostoProyecto || m.idConceptoDir == idGastosOperativo)
                        {
                            if (cc != null && (int)TipoCCEnum.Administración == cc.bit_area.ParseInt())
                            {
                                m.idConceptoDir = idGastosOperativo;
                            }
                            else
                            {
                                m.idConceptoDir = idCostoProyecto;
                            }
                        }
                        var bd = bdMovPol.FirstOrDefault(b => b.year == m.year && b.mes == m.mes && b.tp == m.tp && b.poliza == m.poliza && b.linea == m.linea);
                        if (bd != null)
                        {
                            m.id = bd.id;
                        }
                    });
                    sigMovPol.ForEach(mov =>
                    {
                        _context.tblC_FE_MovPol.AddOrUpdate(mov);
                        _context.SaveChanges();
                    });
                    esGuardado = sigMovPol.All(m => m.id > 0);
                    if (esGuardado)
                    {
                        dbTransaction.Commit();
                    }
                    else
                    {
                        dbTransaction.Rollback();
                    }
                }
                catch (Exception o_O)
                {
                    esGuardado = false;
                    dbTransaction.Rollback();
                }
            return esGuardado;
        }
        string queryFlujoTotalCC(List<string> lstTm, int anio)
        {
            return string.Format(@"SELECT mov.area, mov.cuenta_oc, mov.cc, mov.itm, sum(mov.monto) AS monto
                                FROM sc_movpol mov
                                    WHERE mov.cta BETWEEN 1105 AND 1115 AND mov.year <= {1} AND mov.itm IN ({0})
                                GROUP BY mov.area, mov.cuenta_oc, mov.cc, mov.itm
                                ORDER BY mov.area, mov.cuenta_oc, mov.cc, mov.itm"
                , lstTm.ToLine(",")
                , anio);
        }
        string queryFlujoTotalMultiple(List<string> lstTm, int anio)
        {
            return string.Format(@"SELECT mov.area, mov.cuenta_oc, mov.cc, mov.itm, sum(mov.monto) AS monto
                                    FROM sc_movpol dd
                                    INNER JOIN sc_movpol mov ON mov.year = dd.year AND mov.mes = dd.mes AND mov.tp = dd.tp AND mov.poliza = dd.poliza AND mov.cc <> '*'
                                    WHERE dd.cta BETWEEN 1105 AND 1115 AND dd.cc = '*' AND mov.year <= {1} AND mov.itm IN ({0})
                                    GROUP BY mov.area, mov.cuenta_oc, mov.cc, mov.itm
                                    ORDER BY mov.area, mov.cuenta_oc, mov.cc, mov.itm"
                , lstTm.ToLine(",")
                , anio);
        }
        string queryFlujoTotalCC2019Semana54(List<string> lstTm)
        {
            return string.Format(@"SELECT mov.cc, mov.itm, sum(mov.monto) AS monto
                    FROM sc_movpol mov
                    INNER JOIN sc_polizas pol ON pol.year = mov.year AND pol.mes = mov.mes AND pol.tp = mov.tp AND pol.poliza = mov.poliza
                    WHERE mov.cta BETWEEN 1105 AND 1115 AND pol.fechapol BETWEEN '2019-12-29' AND '2019-12-31' AND CC NOT IN ('*') AND mov.itm IN ({0})
                    GROUP BY mov.cc, mov.itm"
                , lstTm.ToLine(","));
        }
        string queryFlujoTotalMultiple2019Semana54(List<string> lstTm)
        {
            return string.Format(@"SELECT mov.cc, mov.itm, sum(mov.monto) AS monto
                    FROM sc_movpol dd
                    INNER JOIN sc_polizas pol ON pol.year = dd.year AND pol.mes = dd.mes AND pol.tp = dd.tp AND pol.poliza = dd.poliza
                    INNER JOIN sc_movpol mov ON mov.year = dd.year AND mov.mes = dd.mes AND mov.tp = dd.tp AND mov.poliza = dd.poliza AND mov.cc <> '*'
                    WHERE dd.cta BETWEEN 1105 AND 1115 AND dd.cc = '*' AND pol.fechapol BETWEEN '2019-12-29' AND '2019-12-31' AND mov.itm IN ({0})
                    GROUP BY mov.cc, mov.itm"
                , lstTm.ToLine(","));
        }
        #endregion
        #region Concepto
        public List<tblC_FE_CatConcepto> getCatConcepto()
        {
            return _context.tblC_FE_CatConcepto.ToList();
        }
        public List<tblC_FED_CatConcepto> getCatConceptoDir()
        {
            return _context.tblC_FED_CatConcepto.ToList();
        }
        public List<tblC_FE_RelConceptoTm> getRelConceptoTm()
        {
            try
            {
                return _context.tblC_FE_RelConceptoTm.ToList();
            }
            catch (Exception)
            {

                return new List<tblC_FE_RelConceptoTm>();
            }

        }
        public List<tblC_FED_RelConceptoTm> getRelConceptoDirTm()
        {
            try
            {
                return _context.tblC_FED_RelConceptoTm.ToList();
            }
            catch (Exception o_O)
            {
                return new List<tblC_FED_RelConceptoTm>();
            }
        }
        #endregion
        #region ¿Que pasa si?
        public List<tblC_FED_CapPlaneacion> getPlaneacionCierre(BusqFlujoEfectivoDTO busq)
        {
            return getPlaneacionCierre(new BusqProyeccionCierreDTO()
            {
                grupo = "",
                lstCC = busq.lstCC,
                lstAC = busq.lstAC,
                max = busq.max,
                min = busq.min,
                tipo = 0,
                esCC = busq.esCC
            });
        }
        public List<tblC_FED_CapPlaneacion> getPlaneacionCierre(BusqProyeccionCierreDTO busq)
        {
            var semana = busq.max.noSemana();
            var idReserva = 29;
            var lstConceptoDir = _context.tblC_FED_CatConcepto.ToList();
            var lstPadreCierre = lstConceptoDir.Where(w => w.idPadre == -3).Select(s => s.id).ToList();
            var lstCptoCierre = lstConceptoDir.Where(w => lstPadreCierre.Contains(w.idPadre)).Select(s => s.id).ToList();
            var lst = _context.tblC_FED_CapPlaneacion.Where(w => w.esActivo && w.anio == busq.max.Year && w.semana == semana && (busq.esCC ? (busq.lstCC.Contains("TODOS") ? w.ac == "TODOS" : busq.lstCC.Contains(w.cc)) : (busq.lstAC.Contains("TODOS") ? w.ac == "TODOS" : busq.lstAC.Contains(w.ac))) && lstCptoCierre.Contains(w.idConceptoDir)).ToList();
            asignarFlujoPlaneacion(lst);
            var lstReserva = getLstDetProyeccionCierre(new BusqProyeccionCierreDTO()
            {
                idConceptoDir = idReserva,
                grupo = busq.grupo,
                lstCC = busq.lstCC,
                lstAC = busq.lstAC,
                max = busq.max,
                min = busq.min,
                tipo = busq.tipo,
                esCC = busq.esCC
            });
            if (lst.Any(w => w.idConceptoDir == idReserva))
            {
                lst.FirstOrDefault(w => w.idConceptoDir == idReserva).corte = lstReserva.Sum(s => s.monto);
            }
            else
            {
                lst.Add(new tblC_FED_CapPlaneacion()
                {
                    idConceptoDir = idReserva,
                    anio = busq.max.Year,
                    semana = busq.max.noSemana(),
                    fecha = busq.max,
                    cc = busq.lstCC.FirstOrDefault(),
                    ac = busq.lstAC.FirstOrDefault(),
                    corte = lstReserva.Sum(s => s.monto),
                });
            }
            return lst;
        }
        public List<tblC_FED_DetProyeccionCierre> getPlaneacionCierreDetalle(BusqProyeccionCierreDTO busq)
        {
            return getPlaneacionCierreDetalle(new BusqFlujoEfectivoDTO()
            {
                lstCC = busq.lstCC,
                lstAC = busq.lstAC,
                max = busq.max,
                min = busq.min,
                tipo = 0,
                esCC = busq.esCC
            });
        }
        public List<tblC_FED_DetProyeccionCierre> getPlaneacionCierreDetalle(BusqFlujoEfectivoDTO busq)
        {
            var semana = busq.max.noSemana();
            var idReserva = 29;
            var lstConceptoDir = _context.tblC_FED_CatConcepto.ToList();
            var lstPadreCierre = lstConceptoDir.Where(w => w.idPadre == -3).Select(s => s.id).ToList();
            var lstCptoCierre = lstConceptoDir.Where(w => lstPadreCierre.Contains(w.idPadre)).Select(s => s.id).ToList();
            var lst = _context.tblC_FED_DetProyeccionCierre.Where(w => w.esActivo && w.fecha >= busq.min && w.fecha <= busq.max && (busq.esCC ? (busq.lstCC.Contains("TODOS") ? w.ac == "TODOS" : busq.lstCC.Contains(w.cc)) : (busq.lstAC.Contains("TODOS") ? w.ac == "TODOS" : busq.lstAC.Contains(w.ac))) && lstCptoCierre.Contains(w.idConceptoDir)).ToList();
            var lstReserva = getLstDetProyeccionCierre(new BusqProyeccionCierreDTO()
            {
                idConceptoDir = idReserva,
                lstCC = busq.lstCC,
                lstAC = busq.lstAC,
                max = busq.max,
                min = busq.min,
                esCC = busq.esCC
            });
            if (lst.Any(w => w.idConceptoDir == idReserva))
            {
                lst.FirstOrDefault(w => w.idConceptoDir == idReserva).monto = lstReserva.Sum(s => s.monto);
            }
            else
            {
                lst.Add(new tblC_FED_DetProyeccionCierre()
                {
                    idConceptoDir = idReserva,
                    anio = busq.max.Year,
                    semana = busq.max.noSemana(),
                    fecha = busq.max,
                    cc = busq.lstCC.FirstOrDefault(),
                    ac = busq.lstAC.FirstOrDefault(),
                    monto = lstReserva.Sum(s => s.monto),
                });
            }
            return lst;
        }
        public List<tblC_FED_DetProyeccionCierre> getPlaneacionCierreDetalle_Optimizado(BusqProyeccionCierreDTO busq)
        {
            return getPlaneacionCierreDetalle_Optimizado(new BusqFlujoEfectivoDTO()
            {
                lstCC = busq.lstCC,
                lstAC = busq.lstAC,
                max = busq.max,
                min = busq.min,
                tipo = 0,
                esCC = busq.esCC
            });
        }
        public List<tblC_FED_DetProyeccionCierre> getPlaneacionCierreDetalle_Optimizado(BusqFlujoEfectivoDTO busq)
        {
            
            var idReserva = 29;
            var lstConceptoDir = _context.tblC_FED_CatConcepto.ToList();
            var lstPadreCierre = lstConceptoDir.Where(w => w.idPadre == -3).Select(s => s.id).ToList();
            var lstCptoCierre = lstConceptoDir.Where(w => lstPadreCierre.Contains(w.idPadre)).Select(s => s.id).ToList();
            var semana = _context.tblC_FED_Corte.OrderByDescending(x => x.id).Take(20).ToList();
            //var lst = _context.tblC_FED_DetProyeccionCierre.Where(w => w.esActivo && (semana.Any(y => y.anio == w.anio && y.semana == w.semana)) && (busq.esCC ? (busq.lstCC.Contains("TODOS") ? w.ac == "TODOS" : busq.lstCC.Contains(w.cc)) : (busq.lstAC.Contains("TODOS") ? w.ac == "TODOS" : busq.lstAC.Contains(w.ac))) && lstCptoCierre.Contains(w.idConceptoDir)).ToList();
            //var lst = _context.tblC_FED_DetProyeccionCierre.Where(w => w.esActivo && (semana.Any(y => y.anio == w.anio && y.semana == w.semana)) && (busq.lstAC.Contains("TODOS") ? w.ac == "TODOS" : busq.lstAC.Contains(w.ac)) && lstCptoCierre.Contains(w.idConceptoDir)).ToList();
            var lst = new List<tblC_FED_DetProyeccionCierre>();
            string ccs = busq.lstAC.Contains("TODOS") ? "TODOS" : string.Join("','",busq.lstAC);
            string conceptos = string.Join(",", lstCptoCierre);
            var conector = new DapperDTO
            {
                consulta = "SELECT a.idConceptoDir, anio, semana, SUM(a.monto) as monto FROM tblC_FED_DetProyeccionCierre a WHERE a.esActivo = 1 AND a.ac in ('" + ccs + "') AND a.idConceptoDir in (" + conceptos + ") and exists(select top 29 * from  tblC_FED_Corte b where (a.anio = b.anio and a.semana = b.semana) order by b.id desc) group by a.idConceptoDir, anio, semana"
            };
            lst = _context.Select<tblC_FED_DetProyeccionCierre>(conector);
            var lstReserva = getLstDetProyeccionCierre(new BusqProyeccionCierreDTO()
            {
                idConceptoDir = idReserva,
                lstCC = busq.lstCC,
                lstAC = busq.lstAC,
                max = busq.max,
                min = busq.min,
                esCC = busq.esCC
            });
            if (lst.Any(w => w.idConceptoDir == idReserva))
            {
                lst.FirstOrDefault(w => w.idConceptoDir == idReserva).monto = lstReserva.Sum(s => s.monto);
            }
            else
            {
                lst.Add(new tblC_FED_DetProyeccionCierre()
                {
                    idConceptoDir = idReserva,
                    anio = busq.max.Year,
                    semana = busq.max.noSemana(),
                    fecha = busq.max,
                    cc = busq.lstCC.FirstOrDefault(),
                    ac = busq.lstAC.FirstOrDefault(),
                    monto = lstReserva.Sum(s => s.monto),
                });
            }
            return lst;
        }
        public List<tblC_FED_DetProyeccionCierre> getLstDetProyeccionCierre(List<int> lstId)
        {
            return _context.tblC_FED_DetProyeccionCierre.Where(w => w.esActivo && lstId.Contains(w.id)).ToList();
        }
        public List<tblC_FED_DetProyeccionCierre> getLstDetProyeccionCierre(BusqProyeccionCierreDTO busq)
        {
            var idReserva = 29;
            var semana = busq.max.noSemana();
            var lstGrupoReserva = getLstGpoReserva().Select(s => s.id).ToList();
            var lst = from w in _context.tblC_FED_DetProyeccionCierre
                      where w.esActivo && w.idConceptoDir == busq.idConceptoDir && (busq.esCC ? busq.lstCC.Contains(w.cc) : busq.lstAC.Contains(w.ac)) && (w.idConceptoDir == idReserva ? lstGrupoReserva.Contains((int)w.grupoID) : w.anio == busq.max.Year && w.semana == semana)
                      select w;

            //List<tblC_FED_DetProyeccionCierre> lst = _context.Select<tblC_FED_DetProyeccionCierre>(new DapperDTO
            //{
            //    baseDatos = (int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? MainContextEnum.Construplan : MainContextEnum.Arrendadora,
            //    consulta = @"SELECT * FROM tblC_FED_DetProyeccionCierre where esActivo=1 and ac = 'TODOS' and idConceptoDir = " + busq.idConceptoDir + " and grupoID in (" + string.Join(",", lstGrupoReserva) + ")",
            //    parametros = new { registroActivo = true }
            //}).ToList();

            return lst.ToList();
        }
        public List<tblC_FED_DetProyeccionCierre> getLstDetProyeccionCierre(BusqFlujoEfectivoDTO busq)
        {
            var idReserva = 29;
            var semana = busq.max.noSemana();
            var lstGrupoReserva = getLstGpoReserva().Select(s => s.id).ToList();
            //var data = _context.tblC_FED_DetProyeccionCierre
            //    .Where(w => w.esActivo && w.idConceptoDir == busq.idConcepto)
            //    .Where(w => busq.esCC ? (busq.lstCC.Contains("TODOS") ? w.ac == "TODOS" : busq.lstCC.Contains(w.cc)) : (busq.lstAC.Contains("TODOS") ? w.ac == "TODOS" : busq.lstAC.Contains(w.ac)))
            //    .Where(w => w.idConceptoDir == idReserva ? lstGrupoReserva.Contains((int)w.grupoID) : w.anio == busq.max.Year && w.semana == semana).ToList();

            List<tblC_FED_DetProyeccionCierre> data = _context.Select<tblC_FED_DetProyeccionCierre>(new DapperDTO
            {
                baseDatos = (int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? MainContextEnum.Construplan : MainContextEnum.Arrendadora,
                consulta = @"SELECT * FROM tblC_FED_DetProyeccionCierre where esActivo=1 and ac = 'TODOS' and idConceptoDir = " + busq.idConcepto + " and grupoID in (" + string.Join(",", lstGrupoReserva) + ")",
                parametros = new { registroActivo = true }
            }).ToList();


            return data;
        }
        public List<tblC_FED_DetProyeccionCierre> initLstDetProyeccionCierre(BusqFlujoEfectivoDTO busq)
        {
            var lst = new List<tblC_FED_DetProyeccionCierre>();
            var idReserva = 29;
            var semana = busq.max.noSemana();
            var anio = busq.max.Year;
            var esCcTodos = busq.lstCC.Contains("TODOS");
            var esAcTodos = busq.lstAC.Contains("TODOS");
            var lstCompleta = (from det in _context.tblC_FED_DetProyeccionCierre.AsQueryable()
                               where det.esActivo && det.idConceptoDir != idReserva && det.anio == anio && det.semana == semana
                               select det).ToList();
            if (busq.esCC)
            {
                if (esCcTodos)
                {
                    lst = (from cierre in lstCompleta where cierre.ac == "TODOS" select cierre).ToList();
                    lst.Where(cierre => cierre.idDetProyGemelo > 0).ToList().ForEach(cierre =>
                    {
                        var gemeloCC = (from det in lstCompleta
                                        where det.idDetProyGemelo == cierre.id
                                        select det.cc).FirstOrDefault();
                        cierre.cc = gemeloCC;
                    });
                }
                else
                {
                    lst = (from cierre in lstCompleta where busq.lstCC.Contains(cierre.cc) && cierre.ac != "TODOS" select cierre).ToList();
                }
            }
            else
            {
                if (esAcTodos)
                {
                    lst = (from cierre in lstCompleta where cierre.ac == "TODOS" select cierre).ToList();
                    lst.Where(cierre => cierre.idDetProyGemelo > 0).ToList().ForEach(cierre =>
                    {
                        var gemeloAC = (from det in lstCompleta
                                        where det.idDetProyGemelo == cierre.id
                                        select det.ac).FirstOrDefault();
                        cierre.ac = gemeloAC;
                    });
                }
                else
                {
                    lst = (from cierre in lstCompleta where busq.lstAC.Contains(cierre.ac) select cierre).ToList();
                }
            }
            lst.AddRange(getLstDetProyeccionCierre(busq));
            return lst;
        }
        public List<tblC_FED_DetProyeccionCierre> initLstDetProyeccionCierre_Optimizado(BusqFlujoEfectivoDTO busq)
        {
            var lst = new List<tblC_FED_DetProyeccionCierre>();
            var idReserva = 29;
            var semana = busq.max.noSemana();
            var anio = busq.max.Year;
            var esCcTodos = busq.lstCC.Contains("TODOS");
            var esAcTodos = busq.lstAC.Contains("TODOS");


            var lstCompleta = (from det in _context.tblC_FED_DetProyeccionCierre.AsQueryable()
                               where det.esActivo && (busq.idConcepto == idReserva ? det.idConceptoDir == idReserva && det.anio <= anio && det.semana <= semana : det.idConceptoDir != idReserva && det.anio == anio && det.semana == semana) 
                               select det).ToList();
            if (busq.esCC)
            {
                if (esCcTodos)
                {
                    lst = (from cierre in lstCompleta where cierre.ac == "TODOS" select cierre).ToList();
                    lst.Where(cierre => cierre.idDetProyGemelo > 0).ToList().ForEach(cierre =>
                    {
                        var gemeloCC = (from det in lstCompleta
                                        where det.idDetProyGemelo == cierre.id
                                        select det.cc).FirstOrDefault();
                        cierre.cc = gemeloCC;
                    });
                }
                else
                {
                    lst = (from cierre in lstCompleta where busq.lstCC.Contains(cierre.cc) && cierre.ac != "TODOS" select cierre).ToList();
                }
            }
            else
            {
                if (esAcTodos)
                {
                    lst = (from cierre in lstCompleta where cierre.ac == "TODOS" select cierre).ToList();
                    lst.Where(cierre => cierre.idDetProyGemelo > 0).ToList().ForEach(cierre =>
                    {
                        var gemeloAC = (from det in lstCompleta
                                        where det.idDetProyGemelo == cierre.id
                                        select det.ac).FirstOrDefault();
                        cierre.ac = gemeloAC;
                    });
                }
                else
                {
                    lst = (from cierre in lstCompleta where busq.lstAC.Contains(cierre.ac) && cierre.ac!="TODOS" select cierre).ToList();
                }
            }
            //lst.AddRange(getLstDetProyeccionCierre(busq));
            return lst;
        }
        private void AjustarMontosCierre(List<tblC_FED_DetProyeccionCierre> lst)
        {
            init();
            var lstConceptosCierre = ConsultarConceptosCierre();
            lst.ForEach(cierre =>
            {
                if (lstCptoInvertirSigno.Contains(cierre.idConceptoDir))
                {
                    cierre.monto *= -1;
                }
            });
        }
        private List<tblC_FED_CatConcepto> ConsultarConceptosCierre()
        {
            return (from cpto in _context.tblC_FED_CatConcepto
                    where cpto.esActivo && cpto.idPadre == 30
                    select cpto).ToList();
        }
        public List<tblC_FED_CatGrupoReserva> getLstGpoReserva()
        {
            return _context.tblC_FED_CatGrupoReserva.Where(w => w.esActivo).ToList();
        }
        public List<tblC_FED_CatGrupoReserva> getAllGpoReserva()
        {
            return _context.tblC_FED_CatGrupoReserva.ToList();
        }
        #endregion
        #region Movimiento de Poliza
        /// <summary>
        /// Consulta el movimiento de polizas
        /// </summary>
        /// <param name="busq"></param>
        /// <returns></returns>
        public List<MovpolDTO> getLstMovPol(BusqFlujoEfectivoDTO busq)
        {
            var lst = new List<MovpolDTO>();
            var lstOdbc = new List<OdbcConsultaDTO>()
            {
                new OdbcConsultaDTO()
                {
                    consulta = queryLstMovPol(busq),
                    parametros = paramLstMovPolFlujoTotal(busq)
                }
                ,new OdbcConsultaDTO()
                {
                    consulta = queryLstMultiPol(busq),
                    parametros = paramLstMovPolFlujoTotal(busq)
                }
            };
            lstOdbc.ForEach(odbc =>
                lst.AddRange(
                    _contextEnkontrol.Select<MovpolDTO>(EnkontrolEnum.ArrenProd, odbc)
                )
            );
            for (int i = 0; i < lst.Count; i++)
            {
                var mov = lst[i];
                if (mov.area.ParseInt() == 0 || mov.cuenta_oc.ParseInt() == 0)
                {
                    mov.area = 0;
                    mov.cuenta_oc = 0;
                }
                mov.numpro = mov.numpro.ParseInt();
            }
            if (busq.esCC)
            {
                lst = lst.Where(w => busq.lstCC.Contains("TODOS") ? true : busq.lstCC.Contains(w.cc)).ToList();
            }
            else
            {
                lst = lst.Where(w => busq.lstAC.Contains("TODOS") ? true : busq.lstAC.Contains(w.area + "-" + w.cuenta_oc)).ToList();
            }
            return lst;
        }
        public List<tblC_FE_MovPol> getFE_LstMovPol(BusqFlujoEfectivoDTO busq)
        {
            var ahora = DateTime.Now;
            var lst = from mov in getLstMovPol(busq)
                      select new tblC_FE_MovPol
                      {
                          year = mov.year,
                          mes = mov.mes,
                          tp = mov.tp,
                          poliza = mov.poliza,
                          fechapol = mov.fechapol,
                          linea = mov.linea,
                          cta = mov.cta,
                          scta = mov.scta,
                          sscta = mov.sscta,
                          concepto = mov.concepto,
                          itm = mov.itm,
                          itmOri = mov.itm,
                          tm = mov.tm,
                          ac = mov.area + "-" + mov.cuenta_oc,
                          cc = mov.cc,
                          numpro = mov.numpro.ParseInt(),
                          monto = mov.monto,
                          esActivo = true,
                          esFlujoEfectivo = true,
                          idConcepto = 0,
                          idConceptoDir = 0,
                          fechaRegistro = ahora
                      };
            return lst.ToList();
        }
        List<OdbcParameterDTO> paramLstMovPol(BusqFlujoEfectivoDTO busq)
        {
            var lst = new List<OdbcParameterDTO>();
            lst.Add(new OdbcParameterDTO() { nombre = "cta", tipo = OdbcType.Numeric, valor = (int)CtaDeudorDivEnum.FondoFijoCaja });
            lst.Add(new OdbcParameterDTO() { nombre = "cta", tipo = OdbcType.Numeric, valor = (int)CtaDeudorDivEnum.Inversion });
            lst.Add(new OdbcParameterDTO() { nombre = "fechapol", tipo = OdbcType.Date, valor = busq.min });
            lst.Add(new OdbcParameterDTO() { nombre = "fechapol", tipo = OdbcType.Date, valor = busq.max });
            lst.AddRange(busq.lstTm.Select(s => new OdbcParameterDTO() { nombre = "itm", tipo = OdbcType.Numeric, valor = s }).ToList());
            return lst;
        }
        public bool verificaYActualizaAreaCuentaEnMovimientoPolizas()
        {
            var odbcVerifica = new OdbcConsultaDTO() { consulta = queryVerificaAreaCuentaEnMovimientoPolizas() };
            var lstVerifica = _contextEnkontrol.Select<MovpolDTO>(EnkontrolEnum.ArrenProd, odbcVerifica);
            var esGuardado = lstVerifica.Any();
            if (esGuardado)
            {
                var odbcActualiza = new OdbcConsultaDTO() { consulta = queryActualizaAreaCuentaEnMovimientoPolizas() };
                var updateMovPol = _contextEnkontrol.Save(EnkontrolEnum.ArrenProd, odbcActualiza);
                esGuardado = updateMovPol > 0;
            }
            return esGuardado;
        }
        string queryVerificaAreaCuentaEnMovimientoPolizas()
        {
            return @"SELECT * FROM (SELECT 
                         mov.area AS mov_area
                        ,mov.cuenta_oc AS mov_cuenta
                        ,(select top 1 a.area from so_orden_compra_det a
                                    where a.cc = mov.cc and a.numero = mov.orden_compra) AS oc_area
                        ,(select top 1 a.cuenta from so_orden_compra_det a
                                   where a.cc = mov.cc and a.numero = mov.orden_compra) AS oc_cuenta
                        FROM sc_movpol mov
                            INNER JOIN sc_polizas pol ON pol.year = mov.year AND pol.mes = mov.mes AND pol.tp = mov.tp AND pol.poliza = mov.poliza
                                WHERE (mov.area = 0 OR mov.area is null OR mov.cuenta_oc = 0 OR mov.cuenta_oc is null) AND (mov.cta BETWEEN 1105 AND 1115) and (mov.orden_compra!=0 and mov.orden_compra is not null)
                        ) x WHERE x.mov_area != x.oc_area OR x.mov_cuenta != x.oc_cuenta";
        }
        string queryActualizaAreaCuentaEnMovimientoPolizas()
        {
            return @"update sc_movpol 
                    set 
                         area = (select top 1 a.area from so_orden_compra_det a
                                    where a.cc=mov.cc and a.numero=mov.orden_compra)
                        ,cuenta_oc = (select top 1 a.cuenta from so_orden_compra_det a
                                        where a.cc=mov.cc and a.numero=mov.orden_compra)
                    FROM sc_movpol mov
                        INNER JOIN sc_polizas pol ON pol.year = mov.year AND pol.mes = mov.mes AND pol.tp = mov.tp AND pol.poliza = mov.poliza
                    WHERE (mov.area = 0 OR mov.area is null OR mov.cuenta_oc = 0 OR mov.cuenta_oc is null) AND (mov.cta BETWEEN 1105 AND 1115) and (mov.orden_compra!=0 and mov.orden_compra is not null)";
        }
        string lstAcToWhereAC(List<string> lstAC)
        {
            var whereAC = string.Empty;
            lstAC.ForEach(ac => whereAC += "(mov.area = ? AND mov.cuenta_oc = ?) OR ");
            whereAC = whereAC.Remove(whereAC.Length - 3, 3);
            return whereAC;
        }
        string queryLstMovPol(BusqFlujoEfectivoDTO busq)
        {
            return string.Format(@"SELECT mov.year ,mov.mes ,mov.poliza ,mov.tp ,mov.linea ,mov.cta ,mov.scta ,mov.sscta ,mov.digito ,mov.tm ,mov.referencia ,mov.cc, mov.area ,mov.cuenta_oc ,mov.concepto ,mov.monto ,mov.iclave ,mov.itm ,mov.st_par ,mov.orden_compra ,mov.numpro ,pol.fechapol
                                    FROM sc_movpol mov
                                  INNER JOIN sc_polizas pol ON pol.year = mov.year AND pol.mes = mov.mes AND pol.tp = mov.tp AND pol.poliza = mov.poliza
                                        WHERE  mov.cta BETWEEN ? AND ?
                                        AND pol.fechapol BETWEEN ? AND ?
                                        AND mov.itm IN {0}"
                , busq.lstTm.ToParamInValue());
        }
        string queryLstMultiPol(BusqFlujoEfectivoDTO busq)
        {
            return string.Format(@"SELECT dd.year ,dd.mes ,dd.poliza ,dd.tp ,mov.linea ,dd.cta ,dd.scta ,dd.sscta ,dd.digito ,dd.tm ,mov.referencia ,mov.cc ,mov.area ,mov.cuenta_oc ,mov.concepto ,mov.monto ,dd.iclave ,dd.itm ,dd.st_par ,mov.orden_compra ,mov.numpro ,pol.fechapol
                        FROM sc_movpol dd
                        INNER JOIN sc_polizas pol ON pol.year = dd.year AND pol.mes = dd.mes AND pol.tp = dd.tp AND pol.poliza = dd.poliza
                        INNER JOIN sc_movpol mov ON mov.year = dd.year AND mov.mes = dd.mes AND mov.tp = dd.tp AND mov.poliza = dd.poliza AND mov.cc <> '*'
                        WHERE dd.cta BETWEEN ? AND ?
                            AND pol.fechapol BETWEEN ? AND ?
                            AND dd.itm IN {0}
                            AND dd.cc = '*'"
                , busq.lstTm.ToParamInValue());
        }
        /// <summary>
        /// Consulta de cc y itm agrupados
        /// </summary>
        /// <param name="busq"></param>
        /// <returns>cc y itm agrupados</returns>
        public List<MovpolDTO> getLstMovPolCCTM(BusqFlujoEfectivoDTO busq)
        {
            var lst = new List<MovpolDTO>();
            var lstOdbc = new List<OdbcConsultaDTO>()
            {
                new OdbcConsultaDTO()
                {
                    consulta = queryFlujoCCTM(busq),
                    parametros = paramLstMovPol(busq)
                }
                ,new OdbcConsultaDTO()
                {
                    consulta = queryFlujoCCTMMultiple(busq),
                    parametros = paramLstMovPol(busq)
                }
            };
            lstOdbc.ForEach(odbc =>
                lst.AddRange(_contextEnkontrol.Select<MovpolDTO>(EnkontrolAmbienteEnum.Prod, odbc))
            );
            asignarACenCC(lst);
            return lst;
        }
        string queryFlujoCCTM(BusqFlujoEfectivoDTO busq)
        {
            return string.Format(@"SELECT mov.area, mov.cuenta_oc, mov.itm, sum(mov.monto) AS monto
                    FROM sc_movpol mov
                    INNER JOIN sc_polizas pol ON pol.year = mov.year AND pol.mes = mov.mes AND pol.tp = mov.tp AND pol.poliza = mov.poliza
                    WHERE mov.cta BETWEEN ? AND ? 
                        AND pol.fechapol BETWEEN ? AND ?
                        AND mov.itm IN {0}
                    GROUP BY mov.area, mov.cuenta_oc, mov.itm"
                , busq.lstTm.ToParamInValue());
        }
        string queryFlujoCCTMMultiple(BusqFlujoEfectivoDTO busq)
        {
            return string.Format(@"SELECT mov.area, mov.cuenta_oc, mov.itm, sum(mov.monto) AS monto
                    FROM sc_movpol dd
                    INNER JOIN sc_polizas pol ON pol.year = dd.year AND pol.mes = dd.mes AND pol.tp = dd.tp AND pol.poliza = dd.poliza
                    INNER JOIN sc_movpol mov ON mov.year = dd.year AND mov.mes = dd.mes AND mov.tp = dd.tp AND mov.poliza = dd.poliza AND mov.cc <> '*'
                    WHERE dd.cta BETWEEN ? AND ? 
                        AND dd.cc = '*' 
                        AND pol.fechapol BETWEEN ? AND ?
                        AND mov.itm IN {0}
                    GROUP BY mov.area, mov.cuenta_oc, mov.itm"
                , busq.lstTm.ToParamInValue());
        }
        public List<tblC_FED_CapPlaneacion> getSaldoInicial(int anio)
        {
            var ahora = DateTime.Now;
            var finAnio = new DateTime(anio, 12, 31);
            var acumPlan = new List<tblC_FED_CapPlaneacion>();
            var lstPlan = _context.tblC_FED_CapPlaneacion.Where(w => w.anio == anio && w.semana == 52).ToList();
            var lstSaldo = _context.tblC_FED_SaldoInicial.Where(w => w.esActivo && w.anio == anio).ToList();
            var lstCC = lstPlan.Select(s => s.cc).ToList();
            lstCC.AddRange(lstSaldo.Select(s => s.cc).ToList());
            lstCC = lstCC.GroupBy(g => g).Select(s => s.Key).ToList();
            var lstCpto = getCatConceptoDirActivo();
            lstCC.ForEach(cc =>
            {
                var lstSaldoCC = lstSaldo.Where(w => cc == "TODOS" ? true : w.cc == cc).ToList();
                lstCpto.Where(w => w.idPadre > 0).ToList().ForEach(cpto =>
                {
                    var plan = lstPlan.FirstOrDefault(p => p.cc == cc && p.idConceptoDir == cpto.id);
                    if (plan == null)
                    {
                        plan = new tblC_FED_CapPlaneacion()
                        {
                            anio = anio,
                            semana = finAnio.noSemana(),
                            cc = cc,
                            idConceptoDir = cpto.id,
                            fecha = finAnio,
                            esActivo = true,
                            fechaRegistro = ahora
                        };
                    }
                    plan.flujoTotal = lstSaldoCC.Where(w => w.idConceptoDir == cpto.id).Sum(s => s.saldo);
                    plan.strFlujoEfectivo = plan.flujoTotal.ToString();
                    acumPlan.Add(plan);
                });
            });
            return acumPlan;
        }
        public List<tblC_FE_CatConcepto> getCatConceptoActivo()
        {
            try
            {
                return _context.tblC_FE_CatConcepto.Where(w => w.esActivo).ToList();
            }
            catch (Exception o_O)
            {
                return new List<tblC_FE_CatConcepto>();
            }
        }
        public List<tblC_FED_CatConcepto> getCatConceptoDirActivo()
        {
            try
            {
                return _context.tblC_FED_CatConcepto.Where(w => w.esActivo).ToList();
            }
            catch (Exception o_O)
            {
                return new List<tblC_FED_CatConcepto>();
            }
        }
        public List<tblC_FE_MovPol> getLstMovPolActiva(BusqFlujoEfectivoDTO busq)
        {
            try
            {
                var min = busq.min;
                var max = busq.max;
                busq.lstTm = busq.lstTm == null ? new List<int>() : busq.lstTm;
                busq.lstCC = busq.lstCC == null ? new List<string>() : busq.lstCC;
                busq.lstAC = busq.lstAC == null ? new List<string>() : busq.lstAC;
                var esTmVacio = !busq.lstTm.Any();
                var esCcVacio = !busq.lstCC.Any();
                var esAcVacio = !busq.lstAC.Any();
                return (from w in _context.tblC_FE_MovPol
                        where w.esActivo && w.fechapol >= min && w.fechapol <= max
                        && (busq.esCC ? (esCcVacio ? true : busq.lstCC.Contains(w.cc)) : (esAcVacio ? true : busq.lstAC.Contains(w.ac)))
                        select w).ToList();
            }
            catch (Exception)
            {
                return new List<tblC_FE_MovPol>();
            }
        }
        public List<tblC_FE_MovPol> getLstMovPolActiva()
        {
            try
            {
                return _context.tblC_FE_MovPol.Where(w => w.esActivo).ToList();
            }
            catch (Exception o_O)
            {
                return new List<tblC_FE_MovPol>();
            }
        }
        public List<tblC_FE_MovPol> getLstMovPolFlujoEfectivo(BusqFlujoEfectivoDTO busq)
        {
            try
            {
                return _context.tblC_FE_MovPol
                .Where(w => w.esActivo)
                .Where(w => w.esFlujoEfectivo)
                .Where(w => busq.max.Year == 2000 ? true : w.year == busq.max.Year && w.mes <= busq.max.Month)
                .Where(w => busq.lstTm == null ? true : busq.lstTm.Any(a => a == w.itm))
                .Where(w => busq.lstCC == null ? true : busq.lstCC.Any(a => a == w.cc)).ToList();
            }
            catch (Exception o_O)
            {
                return new List<tblC_FE_MovPol>();
            }
        }
        public List<tblC_FE_MovPol> getLstMovPolFlujoEfectivoOperativo(BusqFlujoEfectivoDTO busq)
        {
            try
            {
                return (from w in _context.tblC_FE_MovPol
                        where w.esActivo && w.esFlujoEfectivo && w.idConcepto > 0 && (busq.lstTm.Any() ? true : busq.lstTm.Contains(w.itm)) &&
                        (busq.esCC ? (busq.lstCC.Contains("TODOS") ? true : busq.lstCC.Contains(w.cc)) : (busq.lstAC.Contains("TODOS") ? true : busq.lstAC.Contains(w.ac))) &&
                        (busq.max.Year == 2000 ? true : w.year == busq.max.Year && w.mes <= busq.max.Month)
                        select w).ToList();
            }
            catch (Exception)
            {
                return new List<tblC_FE_MovPol>();
            }
        }
        public List<tblC_FE_MovPol> getLstMovPolFlujoEfectivoDirecto(BusqFlujoEfectivoDTO busq)
        {
            try
            {
                var esTmVacio = busq.lstTm == null;
                var esCCvacio = busq.lstCC == null;
                var esACvacio = busq.lstAC == null;
                var lst = from w in _context.tblC_FE_MovPol
                          where w.esActivo && w.esFlujoEfectivo && w.idConceptoDir > 0
                          && (esTmVacio ? true : busq.lstTm.Contains(w.itm))
                          select w;
                if (busq.esCC)
                {
                    lst = from w in lst where esCCvacio ? true : busq.lstCC.Contains(w.cc) select w;
                }
                else
                {
                    lst = from w in lst where esACvacio ? true : busq.lstAC.Contains(w.ac) select w;
                }
                return lst.ToList();
            }
            catch (Exception o_O)
            {
                return new List<tblC_FE_MovPol>();
            }
        }
        List<tblC_FE_MovPol> getAllMovPol(BusqFlujoEfectivoDTO busq)
        {
            try
            {
                var year = busq.max.Year;
                var mes = busq.max.Month;
                busq.lstTm = busq.lstTm == null ? new List<int>() : busq.lstTm;
                busq.lstCC = busq.lstCC == null ? new List<string>() : busq.lstCC;
                busq.lstAC = busq.lstAC == null ? new List<string>() : busq.lstAC;
                var esTmVacio = !busq.lstTm.Any();
                var esCCVacio = !busq.lstCC.Any();
                var esACVacio = !busq.lstAC.Any();
                return (from w in _context.tblC_FE_MovPol
                        where w.year == year && w.mes == mes
                                && (esTmVacio ? true : busq.lstTm.Contains(w.itm))
                                && (busq.esCC ? (esCCVacio ? true : busq.lstCC.Contains(w.cc)) : (esACVacio ? true : busq.lstAC.Contains(w.ac)))
                        select w).ToList();
            }
            catch (Exception o_O)
            {
                return new List<tblC_FE_MovPol>();
            }
        }
        List<tblC_FE_MovPol> getAllMovPolDelAnio(BusqFlujoEfectivoDTO busq)
        {
            try
            {
                return _context.tblC_FE_MovPol
                .Where(w => w.esActivo)
                .Where(w => w.year == busq.max.Year)
                .Where(w => busq.lstTm == null ? true : busq.lstTm.Any(a => a == w.itm))
                .Where(w => busq.lstCC == null ? true : busq.lstCC.Any(a => a == w.cc)).ToList();
            }
            catch (Exception o_O)
            {
                return new List<tblC_FE_MovPol>();
            }
        }
        public List<MovpolDTO> getLstSaldoInicialACTM(BusqFlujoEfectivoDTO busq)
        {
            var lst = new List<MovpolDTO>();
            var odbc = new OdbcConsultaDTO()
            {
                consulta = querySaldoAnioACTM(busq),
                parametros = paramSaldoAnioACTM(busq)
            };
            lst.AddRange(_contextEnkontrol.Select<MovpolDTO>(EnkontrolAmbienteEnum.Prod, odbc));
            return lst;
        }
        string querySaldoAnioACTM(BusqFlujoEfectivoDTO busq)
        {
            return string.Format(@"SELECT CASE WHEN mov.area IS NULL OR mov.cuenta_oc IS NULL THEN '0-0' ELSE CAST(mov.area AS varchar)+ '-' + CAST(mov.cuenta_oc AS varchar) END cc
                            , mov.itm
                            , sum(mov.monto) AS monto
                    FROM sc_movpol mov
                    WHERE mov.cta BETWEEN 1105 AND 1115 AND mov.year <= ? AND mov.itm IN ({0})
                    GROUP BY cc, mov.itm"
                , busq.lstTm.Select(s => s.ToString()).ToList().ToLine(","));
        }
        List<OdbcParameterDTO> paramSaldoAnioACTM(BusqFlujoEfectivoDTO busq)
        {
            var lst = new List<OdbcParameterDTO>();
            lst.Add(new OdbcParameterDTO() { nombre = "year", tipo = OdbcType.Int, valor = busq.min.Year });
            lst.AddRange(from tm in busq.lstTm select new OdbcParameterDTO { nombre = "itm", tipo = OdbcType.Int, valor = tm });
            return lst;
        }
        #endregion
        #region Flujo Efectivo
        /// <summary>
        /// Consulta el movimiento de polizas del año
        /// </summary>
        /// <param name="busq"></param>
        /// <returns></returns>
        public List<MovpolDTO> getLstMovPolAcumulado(BusqFlujoEfectivoDTO busq)
        {
            var lst = new List<MovpolDTO>();
            var lstOdbc = new List<OdbcConsultaDTO>()
            {
                new OdbcConsultaDTO()
                {
                    consulta = queryLstMovPolAcumulado(busq),
                    parametros = paramLstMovPolAcumulado(busq)
                },
                new OdbcConsultaDTO()
                {
                    consulta = queryLstMultiPolAcumulado(busq),
                    parametros = paramLstMovPolAcumulado(busq)
                }
            };
            lstOdbc.ForEach(odbc =>
                lst.AddRange(
                    _contextEnkontrol.Select<MovpolDTO>(EnkontrolAmbienteEnum.Prod, odbc)
                )
            );
            return lst;
        }
        List<OdbcParameterDTO> paramLstMovPolAcumulado(BusqFlujoEfectivoDTO busq)
        {
            var lst = new List<OdbcParameterDTO>();
            lst.Add(new OdbcParameterDTO() { nombre = "cta", tipo = OdbcType.Numeric, valor = (int)CtaDeudorDivEnum.FondoFijoCaja });
            lst.Add(new OdbcParameterDTO() { nombre = "cta", tipo = OdbcType.Numeric, valor = (int)CtaDeudorDivEnum.Inversion });
            lst.Add(new OdbcParameterDTO() { nombre = "year", tipo = OdbcType.Numeric, valor = busq.max.Year });

            return lst;
        }
        string queryLstMovPolAcumulado(BusqFlujoEfectivoDTO busq)
        {
            return string.Format(@"SELECT * FROM sc_movpol
                                        WHERE cta BETWEEN ? AND ?
                                        AND year = ?");
        }
        string queryLstMultiPolAcumulado(BusqFlujoEfectivoDTO busq)
        {
            return string.Format(@"SELECT dd.year ,dd.mes ,dd.poliza ,dd.tp ,mov.linea ,dd.cta ,dd.scta ,dd.sscta ,dd.digito ,dd.tm ,mov.referencia ,mov.cc ,mov.concepto ,mov.monto ,dd.iclave ,dd.itm ,dd.st_par ,mov.orden_compra ,mov.numpro
                        FROM sc_movpol dd
                        INNER JOIN sc_movpol mov ON mov.year = dd.year AND mov.mes = dd.mes AND mov.tp = dd.tp AND mov.poliza = dd.poliza AND mov.cc <> '*'
                        WHERE dd.cta BETWEEN ? AND ?
                            AND dd.year = ?
                            AND dd.cc = '*'");
        }
        public List<CatctaDTO> getCatCtaDeudoresDiversios()
        {
            var odbc = new OdbcConsultaDTO()
            {
                consulta = "SELECT * FROM catcta WHERE cta BETWEEN ? AND ?",
                parametros = new List<OdbcParameterDTO>()
                {
                    new OdbcParameterDTO() { nombre = "cta", tipo = OdbcType.Numeric, valor = (int)CtaDeudorDivEnum.FondoFijoCaja },
                    new OdbcParameterDTO() { nombre = "cta", tipo = OdbcType.Numeric, valor = (int)CtaDeudorDivEnum.Inversion }
                }
            };
            var lst = _contextEnkontrol.Select<CatctaDTO>(EnkontrolAmbienteEnum.Prod, odbc);
            return lst;
        }
        public List<SalContCcDTO> getLstSalContCC(BusqFlujoEfectivoDTO busq)
        {
            var odbc = new OdbcConsultaDTO()
            {
                consulta = "SELECT * FROM sc_salcont_cc WHERE cta BETWEEN ? AND ? AND year = ?",
                parametros = paramSalContCC(busq)
            };
            var lst = _contextEnkontrol.Select<SalContCcDTO>(EnkontrolAmbienteEnum.Prod, odbc);
            return lst;
        }
        List<OdbcParameterDTO> paramSalContCC(BusqFlujoEfectivoDTO busq)
        {
            var lst = new List<OdbcParameterDTO>();
            lst.Add(new OdbcParameterDTO() { nombre = "cta", tipo = OdbcType.Numeric, valor = CtaDeudorDivEnum.FondoFijoCaja });
            lst.Add(new OdbcParameterDTO() { nombre = "cta", tipo = OdbcType.Numeric, valor = CtaDeudorDivEnum.Inversion });
            lst.Add(new OdbcParameterDTO() { nombre = "year", tipo = OdbcType.Numeric, valor = busq.max.Year });
            return lst;
        }
        public List<MovpolDTO> getLstMovPolFlujoTotal(BusqFlujoEfectivoDTO busq)
        {
            var lst = new List<MovpolDTO>();
            var lstOdbc = new List<OdbcConsultaDTO>()
            {
                new OdbcConsultaDTO()
                {
                    consulta = queryLstMovPolFlujoTotal(busq),
                    parametros = paramLstMovPolFlujoTotal(busq)
                },
                new OdbcConsultaDTO()
                {
                    consulta = queryLstMultiPolFlujoTotal(busq),
                    parametros = paramLstMovPolFlujoTotal(busq)
                }
            };
            lstOdbc.ForEach(odbc =>
                lst.AddRange(
                    _contextEnkontrol.Select<MovpolDTO>(EnkontrolAmbienteEnum.Prod, odbc)
                )
            );
            lst.ForEach(mov =>
            {
                mov.area = mov.area.ParseInt();
                mov.cuenta_oc = mov.cuenta_oc.ParseInt();
                if (mov.area == 0 || mov.cuenta_oc == 0)
                {
                    mov.area = 0;
                    mov.cuenta_oc = 0;
                }
            });
            if (busq.esCC)
            {
                lst = lst.Where(mov => busq.lstCC.Contains("TODOS") ? true : busq.lstCC.Contains(mov.cc)).ToList();
            }
            else
            {
                lst = lst.Where(mov => busq.lstAC.Contains("TODOS") ? true : busq.lstAC.Contains(mov.area + "-" + mov.cuenta_oc)).ToList();
            }
            return lst;
        }
        public Task<List<MovpolDTO>> taskLstMovPolFlujoTotal(BusqFlujoEfectivoDTO busq)
        {
            return Task.Run(() =>
            {
                var lstOdbc = new List<OdbcConsultaDTO>()
                {
                    new OdbcConsultaDTO()
                    {
                        consulta = queryLstMovPolFlujoTotal(busq),
                        parametros = paramLstMovPolFlujoTotal(busq)
                    },
                    new OdbcConsultaDTO()
                    {
                        consulta = queryLstMultiPolFlujoTotal(busq),
                        parametros = paramLstMovPolFlujoTotal(busq)
                    }
                };
                var lst = _contextEnkontrol.Select<MovpolDTO>(EnkontrolEnum.ArrenProd, lstOdbc);
                lst.ForEach(mov =>
                {
                    mov.area = mov.area.ParseInt();
                    mov.cuenta_oc = mov.cuenta_oc.ParseInt();
                    if (mov.area == 0 || mov.cuenta_oc == 0)
                    {
                        mov.area = 0;
                        mov.cuenta_oc = 0;
                    }
                });
                if (busq.esCC)
                {
                    lst = lst.Where(mov => busq.lstCC.Contains("TODOS") ? true : busq.lstCC.Contains(mov.cc)).ToList();
                }
                else
                {
                    lst = lst.Where(mov => busq.lstAC.Contains("TODOS") ? true : busq.lstAC.Contains(mov.area + "-" + mov.cuenta_oc)).ToList();
                }
                return lst;
            });
        }
        public List<MovpolDTO> taskLstMovPolFlujoTotal_Optimizado2(BusqFlujoEfectivoDTO busq)
        {
            var lstOdbc = new List<OdbcConsultaDTO>()
                {
                    new OdbcConsultaDTO()
                    {
                        consulta = queryLstMovPolFlujoTotal(busq),
                        parametros = paramLstMovPolFlujoTotal(busq)
                    },
                    new OdbcConsultaDTO()
                    {
                        consulta = queryLstMultiPolFlujoTotal(busq),
                        parametros = paramLstMovPolFlujoTotal(busq)
                    }
                };
            var lst = _contextEnkontrol.Select<MovpolDTO>(EnkontrolEnum.ArrenProd, lstOdbc);
            lst.ForEach(mov =>
            {
                mov.area = mov.area.ParseInt();
                mov.cuenta_oc = mov.cuenta_oc.ParseInt();
                if (mov.area == 0 || mov.cuenta_oc == 0)
                {
                    mov.area = 0;
                    mov.cuenta_oc = 0;
                }
            });
            if (busq.esCC)
            {
                lst = lst.Where(mov => busq.lstCC.Contains("TODOS") ? true : busq.lstCC.Contains(mov.cc)).ToList();
            }
            else
            {
                lst = lst.Where(mov => busq.lstAC.Contains("TODOS") ? true : busq.lstAC.Contains(mov.area + "-" + mov.cuenta_oc)).ToList();
            }
            return lst;
        }
        public List<MovpolDTO> taskLstMovPolFlujoTotal_Optimizado(FiltroPolizasDTO filtro)
        {
            var lstOdbc = new List<OdbcConsultaDTO>()
                {
                    new OdbcConsultaDTO()
                    {
                        consulta = queryLstMovPolFlujoTotal_Optimizado(filtro),
                        parametros = paramLstMovPolFlujoTotal_Optimizado(filtro)
                    },
                    new OdbcConsultaDTO()
                    {
                        consulta = queryLstMultiPolFlujoTotal_Optimizado(filtro),
                        parametros = paramLstMovPolFlujoTotal_Optimizado(filtro)
                    }
                };
            var lst = _contextEnkontrol.Select<MovpolDTO>(EnkontrolEnum.ArrenProd, lstOdbc);
            lst.ForEach(mov =>
            {
                mov.area = mov.area.ParseInt();
                mov.cuenta_oc = mov.cuenta_oc.ParseInt();
                if (mov.area == 0 || mov.cuenta_oc == 0)
                {
                    mov.area = 0;
                    mov.cuenta_oc = 0;
                }
            });
            if (filtro.busq.esCC)
            {
                lst = lst.Where(mov => filtro.busq.lstCC.Contains("TODOS") ? true : filtro.busq.lstCC.Contains(mov.cc)).ToList();
            }
            else
            {
                lst = lst.Where(mov => filtro.busq.lstAC.Contains("TODOS") ? true : filtro.busq.lstAC.Contains(mov.area + "-" + mov.cuenta_oc)).ToList();
            }
            return lst;
        }
        List<OdbcParameterDTO> paramLstMovPolFlujoTotal(BusqFlujoEfectivoDTO busq)
        {
            var lst = new List<OdbcParameterDTO>();
            lst.Add(new OdbcParameterDTO() { nombre = "cta", tipo = OdbcType.Numeric, valor = (int)CtaDeudorDivEnum.FondoFijoCaja });
            lst.Add(new OdbcParameterDTO() { nombre = "cta", tipo = OdbcType.Numeric, valor = (int)CtaDeudorDivEnum.Inversion });
            lst.Add(new OdbcParameterDTO() { nombre = "fechapol", tipo = OdbcType.Date, valor = busq.min });
            lst.Add(new OdbcParameterDTO() { nombre = "fechapol", tipo = OdbcType.Date, valor = busq.max });
            lst.AddRange(busq.lstTm.Select(s => new OdbcParameterDTO() { nombre = "itm", tipo = OdbcType.Numeric, valor = s }).ToList());
            return lst;
        }
        List<OdbcParameterDTO> paramLstMovPolFlujoTotal_Optimizado(FiltroPolizasDTO filtro)
        {
            var lst = new List<OdbcParameterDTO>();
            lst.Add(new OdbcParameterDTO() { nombre = "fechapol", tipo = OdbcType.Date, valor = filtro.busq.min });
            lst.Add(new OdbcParameterDTO() { nombre = "fechapol", tipo = OdbcType.Date, valor = filtro.busq.max });
            
            lst.AddRange(filtro.listTM.Select(s => new OdbcParameterDTO() { nombre = "itm", tipo = OdbcType.Numeric, valor = s }).ToList());

            if (filtro.busqDet.cta > 0)
            {
                lst.Add(new OdbcParameterDTO() { nombre = "cta", tipo = OdbcType.Numeric, valor = filtro.busqDet.cta });
                lst.Add(new OdbcParameterDTO() { nombre = "scta", tipo = OdbcType.Numeric, valor = filtro.busqDet.scta });
                lst.Add(new OdbcParameterDTO() { nombre = "sscta", tipo = OdbcType.Numeric, valor = filtro.busqDet.sscta });
            }
            else
            {
                lst.Add(new OdbcParameterDTO() { nombre = "cta", tipo = OdbcType.Numeric, valor = (int)CtaDeudorDivEnum.FondoFijoCaja });
                lst.Add(new OdbcParameterDTO() { nombre = "cta", tipo = OdbcType.Numeric, valor = (int)CtaDeudorDivEnum.Inversion });
            }

            if (filtro.area != 0 && filtro.cuenta != 0)
            {
                lst.Add(new OdbcParameterDTO() { nombre = "area", tipo = OdbcType.Numeric, valor = filtro.area });
                lst.Add(new OdbcParameterDTO() { nombre = "cuenta_oc", tipo = OdbcType.Numeric, valor = filtro.cuenta });
            }
            return lst;
        }
        string queryLstMovPolFlujoTotal(BusqFlujoEfectivoDTO busq)
        {
            return string.Format(@"SELECT mov.area, mov.cuenta_oc, mov.cc, mov.itm, mov.cta, mov.scta, mov.sscta, mov.numpro, mov.concepto, sum(mov.monto) AS monto
                                    FROM sc_movpol mov
                                  INNER JOIN sc_polizas pol ON pol.year = mov.year AND pol.mes = mov.mes AND pol.tp = mov.tp AND pol.poliza = mov.poliza
                                        WHERE mov.cta BETWEEN ? AND ?
                                        AND pol.fechapol BETWEEN ? AND ?
                                        AND mov.itm IN {0}
                                GROUP BY mov.area, mov.cuenta_oc, mov.cc, mov.itm, mov.cta, mov.scta, mov.sscta, mov.numpro, mov.concepto"
                , busq.lstTm.ToParamInValue());
        }
        string queryLstMovPolFlujoTotal_Optimizado(FiltroPolizasDTO filtro)
        {
            string cuentas = filtro.busqDet.cta > 0 ? " AND mov.cta = ? AND mov.scta = ? AND mov.sscta = ? " : " AND mov.cta BETWEEN ? AND ? ";
            string areaCuenta = filtro.area == 0 ? "" : "and mov.area = ? and mov.cuenta_oc = ?";
            return string.Format(@"SELECT mov.area, mov.cuenta_oc, mov.cc, mov.itm, mov.cta, mov.scta, mov.sscta, mov.numpro, mov.concepto, sum(mov.monto) AS monto
                                    FROM sc_movpol mov
                                  INNER JOIN sc_polizas pol ON pol.year = mov.year AND pol.mes = mov.mes AND pol.tp = mov.tp AND pol.poliza = mov.poliza
                                        WHERE pol.fechapol BETWEEN ? AND ?
                                        AND mov.itm IN {0} " + cuentas + @" " + areaCuenta + @"
                                GROUP BY mov.area, mov.cuenta_oc, mov.cc, mov.itm, mov.cta, mov.scta, mov.sscta, mov.numpro, mov.concepto"
                , filtro.listTM.ToParamInValue());
        }
        string queryLstMultiPolFlujoTotal(BusqFlujoEfectivoDTO busq)
        {
            return string.Format(@"SELECT mov.area, mov.cuenta_oc, mov.cc, mov.itm, dd.cta, dd.scta, dd.sscta, mov.numpro, mov.concepto, sum(mov.monto) AS monto
                        FROM sc_movpol dd
                        INNER JOIN sc_polizas pol ON pol.year = dd.year AND pol.mes = dd.mes AND pol.tp = dd.tp AND pol.poliza = dd.poliza
                        INNER JOIN sc_movpol mov ON mov.year = dd.year AND mov.mes = dd.mes AND mov.tp = dd.tp AND mov.poliza = dd.poliza AND mov.cc <> '*'
                        WHERE dd.cta BETWEEN ? AND ? 
                        AND dd.cc = '*' 
                        AND pol.fechapol BETWEEN ? AND ?
                        AND mov.itm IN {0}
                            AND dd.cc = '*'
                        GROUP BY mov.area, mov.cuenta_oc, mov.cc, mov.itm, dd.cta, dd.scta, dd.sscta, mov.numpro, mov.concepto"
                , busq.lstTm.ToParamInValue());
        }
        string queryLstMultiPolFlujoTotal_Optimizado(FiltroPolizasDTO filtro)
        {
            string cuentas = filtro.busqDet.cta > 0 ? " AND mov.cta = ? AND mov.scta = ? AND mov.sscta = ? " : " AND mov.cta BETWEEN ? AND ? ";
            string areaCuenta = filtro.area == 0 ? "" : "and mov.area = ? and mov.cuenta_oc = ?";
            return string.Format(@"SELECT mov.area, mov.cuenta_oc, mov.cc, mov.itm, dd.cta, dd.scta, dd.sscta, mov.numpro, mov.concepto, sum(mov.monto) AS monto
                        FROM sc_movpol dd
                        INNER JOIN sc_polizas pol ON pol.year = dd.year AND pol.mes = dd.mes AND pol.tp = dd.tp AND pol.poliza = dd.poliza
                        INNER JOIN sc_movpol mov ON mov.year = dd.year AND mov.mes = dd.mes AND mov.tp = dd.tp AND mov.poliza = dd.poliza AND mov.cc <> '*'
                        WHERE dd.cc = '*' 
                        AND pol.fechapol BETWEEN ? AND ?
                        AND mov.itm IN {0}
                            AND dd.cc = '*' " + cuentas + @" " + areaCuenta + @"
                        GROUP BY mov.area, mov.cuenta_oc, mov.cc, mov.itm, dd.cta, dd.scta, dd.sscta, mov.numpro, mov.concepto"
                , filtro.listTM.ToParamInValue());
        }
        public List<tblC_FED_CcVisto> getLstCCvistos(int anio, int semana)
        {
            try
            {
                return _context.tblC_FED_CcVisto.Where(w => w.anio == anio && w.semana == semana).ToList();
            }
            catch (Exception o_O)
            {
                return new List<tblC_FED_CcVisto>();
            }
        }
        public List<tblC_FED_RelObraUsuario> getRelObraUsuario()
        {
            return _context.tblC_FED_RelObraUsuario.Where(w => w.idUsuario == vSesiones.sesionUsuarioDTO.id).ToList();
        }
        public List<tblC_FED_CapPlaneacion> getLstSaldoInicialCCTM(BusqFlujoEfectivoDTO busq)
        {
            var lst = new List<MovpolDTO>();
            var odbc = new OdbcConsultaDTO()
            {
                consulta = querySaldoAnioCCTM(busq),
                parametros = paramSaldoAnioCCTM(busq)
            };
            lst.AddRange(_contextEnkontrol.Select<MovpolDTO>(EnkontrolAmbienteEnum.Prod, odbc));
            var lstSaldoInicial = from g in lst
                                  group g by new
                                  {
                                      cc = g.cc,
                                      idCpto = asigaIdCptoDir(g)
                                  } into s
                                  select new tblC_FED_CapPlaneacion
                                  {
                                      cc = s.Key.cc,
                                      idConceptoDir = s.Key.idCpto,
                                      strSaldoInicial = s.Sum(ss => ss.monto).ToString(),
                                  };
            return lstSaldoInicial.ToList();
        }
        string querySaldoAnioCCTM(BusqFlujoEfectivoDTO busq)
        {
            return string.Format(@"SELECT mov.cc, mov.itm, sum(mov.monto) AS monto
                    FROM sc_movpol mov
                    WHERE mov.cta BETWEEN 1105 AND 1115 AND mov.year <= ? AND mov.itm IN ({0}) AND mov.cc IN ({1})
                    GROUP BY mov.cc, mov.itm"
                , busq.lstTm.Select(s => s.ToString()).ToList().ToLine(",")
                , busq.lstCC.Select(s => s.ToString()).ToList().ToLine(","));
        }
        List<OdbcParameterDTO> paramSaldoAnioCCTM(BusqFlujoEfectivoDTO busq)
        {
            var anioAnterio = busq.max.Year - 1;
            var lst = new List<OdbcParameterDTO>();
            lst.Add(new OdbcParameterDTO() { nombre = "year", tipo = OdbcType.Int, valor = anioAnterio });
            lst.AddRange(from tm in busq.lstTm select new OdbcParameterDTO { nombre = "itm", tipo = OdbcType.Int, valor = tm });
            lst.AddRange(busq.lstCC.Select(cc => new OdbcParameterDTO() { nombre = "cc", tipo = OdbcType.NVarChar, valor = cc }).ToList());
            return lst;
        }
        #endregion
        #region Planeación
        void asignarFlujoPlaneacion(List<tblC_FED_CapPlaneacion> lst)
        {
            var lstCpto = _context.tblC_FED_CatConcepto.ToList();

            lst.ForEach(plan =>
            {
                plan.flujoTotal = plan.strFlujoEfectivo.ParseDecimal();
                var cpto = lstCpto.FirstOrDefault(c => c.id == plan.idConceptoDir);
                if (cpto.operador.Trim() == "-" && plan.planeado > 0)
                {
                    plan.planeado *= -1;
                }
            });
        }
        void asignarFlujoPlaneacion(List<tblC_FED_PlaneacionDet> lst)
        {
            var lstCpto = _context.tblC_FED_CatConcepto.ToList();

            lst.ForEach(plan =>
            {
                var cpto = lstCpto.FirstOrDefault(c => c.id == plan.concepto);
                if (cpto.operador.Trim() == "-" && plan.concepto > 0)
                {
                    plan.monto *= -1;
                }
            });
        }
        public List<tblC_FED_CapPlaneacion> getPlaneacion()
        {
            try
            {
                var lst = (from w in _context.tblC_FED_CapPlaneacion where w.esActivo select w).ToList();
                asignarFlujoPlaneacion(lst);
                return lst;
            }
            catch (Exception o_O)
            {
                return new List<tblC_FED_CapPlaneacion>();
            }
        }

        public IQueryable<tblC_FED_CapPlaneacion> getPlaneacionOptimizado(BusqFlujoEfectivoDTO busq)
        {
            try
            {
                //var lst = (from w in _context.tblC_FED_CapPlaneacion where w.esActivo select w).ToList();
                var lst = new List<tblC_FED_CapPlaneacion>();
                if (busq.esCC)
                {
                    //var lstMovCC = flujoFS.getFE_LstMovPol(busq);
                    //lstMovCC.ForEach(mov =>
                    //{
                    //    mov.idConcepto = RelConcepto.Any(r => r.tm == mov.itm) ? RelConcepto.FirstOrDefault(r => r.tm == mov.itm).idConcepto : 0;
                    //    mov.idConceptoDir = RelConceptoDir.Any(r => r.tm == mov.itm) ? RelConceptoDir.FirstOrDefault(r => r.tm == mov.itm).idConceptoDir : 0;
                    //});
                    //return lstMovCC;
                }
                else
                {
                    var ccs = busq.lstAC.Contains("TODOS") ? "'TODOS'" : "'" + string.Join("','", busq.lstAC) + "'";
                    var conector = new DapperDTO
                    {
                        consulta = "SELECT * FROM tblC_FED_CapPlaneacion a WHERE a.esActivo = 1 AND a.idConceptoDir > 0 AND a.ac in (" + ccs + ") and exists(select top 29 * from  tblC_FED_Corte b where (a.anio = b.anio and a.semana = b.semana) order by b.id desc)"
                    };
                    lst = _context.Select<tblC_FED_CapPlaneacion>(conector);

                }
                asignarFlujoPlaneacion(lst);
                return lst.AsQueryable();
            }
            catch (Exception o_O)
            {
                return new List<tblC_FED_CapPlaneacion>().AsQueryable();
            }
        }
        public List<tblC_FED_PlaneacionDet> getPlaneacionDetalles()
        {
            try
            {
                var lst = (from w in _context.tblC_FED_PlaneacionDet where w.estatus select w).ToList();
                return lst;
            }
            catch (Exception o_O)
            {
                return new List<tblC_FED_PlaneacionDet>();
            }
        }
        List<tblC_FED_CapPlaneacion> getAllPlaneacion()
        {
            try
            {
                var lst = _context.tblC_FED_CapPlaneacion.Where(w => w.idConceptoDir > 0).ToList();
                asignarFlujoPlaneacion(lst);
                return lst;
            }
            catch (Exception o_O)
            {
                return new List<tblC_FED_CapPlaneacion>();
            }
        }
        public List<tblC_FED_CapPlaneacion> getPlaneacion(BusqFlujoEfectivoDTO busq)
        {
            try
            {
                var anio = busq.max.Year;
                var semana = busq.max.noSemana();
                var lst = _context.tblC_FED_CapPlaneacion
                .Where(w => w.esActivo)
                .Where(w => w.idConceptoDir > 0)
                .Where(w => w.anio == anio && w.semana == semana).ToList();
                asignarFlujoPlaneacion(lst);
                return lst;
            }
            catch (Exception o_O)
            {
                return new List<tblC_FED_CapPlaneacion>();
            }
        }
        #endregion
        #region Combox

        public List<ComboDTO> getCboGrupoConcepto()
        {
            return _context.tblC_FE_CatConcepto.ToList()
                .Where(w => w.idpadre == 0)
                .Select(s => new ComboDTO()
                {
                    Value = s.id.ToString(),
                    Text = s.Concepto,
                    Prefijo = JsonUtils.convertNetObjectToJson(s)
                }).ToList();
        }
        public List<ComboGroupDTO> getCboGpoConcepto()
        {
            var lstConcepto = _context.tblC_FE_CatConcepto.ToList();
            return lstConcepto
                  .GroupBy(g => g.idpadre)
                  .Where(g => g.Key > 0)
                  .Select(cto => new ComboGroupDTO
                  {
                      label = lstConcepto.FirstOrDefault(pad => pad.id == cto.Key).Concepto,
                      options = cto.Select(opt => new ComboDTO()
                      {
                          Text = opt.Concepto,
                          Value = opt.id.ToString(),
                          Prefijo = JsonUtils.convertNetObjectToJson(opt)
                      }).ToList()
                  }).ToList();
        }
        public List<ComboDTO> getCboCCActivosSigoplan()
        {
            var cbo = getComboAreaCuenta();
            var lstAC = _context.tblP_CC.Where(x => x.estatus && x.area > 0 && x.cuenta > 0).ToList();
            cbo.ForEach(option =>
            {
                var ac = lstAC.FirstOrDefault(w => w.areaCuenta == option.Value);
                option.Prefijo = ac == null || !ac.fechaArranque.HasValue ? "Sin fecha" : ac.fechaArranque.Value.ToString("MMMM / yyyy").ToUpper();
            });
            return cbo;
        }

        public List<ComboDTO> getCboCCTodosSigoplan()
        {
            var idUsuario = vSesiones.sesionUsuarioDTO.id;
            var lstAC = from cc in _context.tblP_CC select cc;
            var relObraUsuario = getRelObraUsuario();
            var esTODO = relObraUsuario.Any(s => s.tipo == tipoObraUsuarioEnum.Todos);
            var relGpoObra = relObraUsuario.Where(s => s.tipo == tipoObraUsuarioEnum.Grupo).Select(s => s.obra).ToList();
            var relIndObra = relObraUsuario.Where(s => s.tipo == tipoObraUsuarioEnum.Obra).Select(s => s.obra).ToList();
            var fechaMinima = new DateTime(2018, 5, 30);
            var cbo = (from ac in getComboAreaCuenta()
                       where esTODO ? true : relIndObra.Contains(ac.Value)
                       select ac).ToList();
            foreach (var option in cbo)
            {
                var ac = lstAC.FirstOrDefault(w => w.areaCuenta == option.Value);
                option.Prefijo = (ac == null ? fechaMinima : ac.fechaArranque.GetValueOrDefault()).ToString("MMMM / yyyy").ToUpper();
                option.Id = (ac == null ? "0" : ac.cc);
            }
            if (esTODO)
            {
                cbo.Insert(0, new ComboDTO()
                {
                    Text = "TODOS",
                    Value = "TODOS",
                    Prefijo = "ENERO / 2020",
                    Id = ""
                });
            }
            return cbo;
        }
        public List<ComboDTO> getLstGrupoReserva()
        {
            return _context.tblC_FED_CatGrupoReserva
                .Where(x => x.esActivo)
                .Select(c => new ComboDTO()
                {
                    Value = ""+c.id,
                    Text = c.grupo
                }).ToList();
        }
        public List<ComboDTO> getComboAreaCuenta()
        {
            try
            {
                var odbcCplan = new OdbcConsultaDTO()
                {
                    consulta = @"SELECT DISTINCT CAST(area AS Varchar)+'-'+CAST(cuenta AS Varchar) AS Value, CAST(area AS Varchar)+'-'+CAST(cuenta AS Varchar)+' '+ MAX(descripcion) AS Text, area, cuenta
                                            FROM si_area_cuenta
                                            GROUP BY area, cuenta
                                            ORDER BY area ,cuenta"
                };
                var lst = _contextEnkontrol.Select<Core.DTO.Principal.Generales.ComboDTO>(EnkontrolEnum.ArrenProd, odbcCplan);
                var lstCC = _context.tblP_CC.ToList();
                lst = lst.OrderBy(o => o.Value.Split('-')[0].ParseInt()).ThenBy(o => o.Value.Split('-')[1].ParseInt()).ToList();
                return lst;
            }
            catch (Exception o_O) { return new List<Core.DTO.Principal.Generales.ComboDTO>(); }
        }
        public List<ComboDTO> getComboAreaCuentaConCentroCostos()
        {
            try
            {
                var odbcCplan = new OdbcConsultaDTO()
                {
                    consulta = @"SELECT DISTINCT CAST(ac.area AS Varchar)+'-'+CAST(ac.cuenta AS Varchar) AS Prefijo, ac.centro_costo AS Value, ac.centro_costo+'-'+ cc.descripcion AS Text
                                            FROM si_area_cuenta ac
                                            INNER JOIN cc cc ON cc.cc = ac.centro_costo
                                            ORDER BY ac.centro_costo"
                };
                var lst = _contextEnkontrol.Select<Core.DTO.Principal.Generales.ComboDTO>(EnkontrolAmbienteEnum.Prod, odbcCplan);
                lst = lst.GroupBy(g => g.Value).Select(s => new ComboDTO()
                {
                    Text = s.FirstOrDefault().Text,
                    Value = s.Key,
                    Prefijo = s.Select(p => p.Prefijo).ToList().ToLine(",").Replace("'", string.Empty)
                }).ToList();
                return lst;
            }
            catch (Exception o_O) { return new List<Core.DTO.Principal.Generales.ComboDTO>(); }
        }
        public List<ComboDTO> getComboAreaCuentaConCentroCostosPorUsuario()
        {
            try
            {
                var relObraUsuario = getRelObraUsuario();
                var esTODO = relObraUsuario.Any(s => s.tipo == tipoObraUsuarioEnum.Todos);
                var relGpoObra = relObraUsuario.Where(s => s.tipo == tipoObraUsuarioEnum.Grupo).Select(s => s.obra).ToList();
                var relIndObra = relObraUsuario.Where(s => s.tipo == tipoObraUsuarioEnum.Obra).Select(s => s.obra).ToList();
                var odbcCplan = new OdbcConsultaDTO()
                {
                    consulta = string.Format(@"SELECT DISTINCT CAST(ac.area AS Varchar)+'-'+CAST(ac.cuenta AS Varchar) AS Prefijo, ac.centro_costo AS Value, ac.centro_costo+'-'+ cc.descripcion AS Text
                                            FROM si_area_cuenta ac
                                            INNER JOIN cc cc ON cc.cc = ac.centro_costo
                                            {0}
                                            ORDER BY ac.centro_costo", esTODO ? string.Empty : "WHERE Prefijo IN " + relIndObra.ToParamInValue())
                };
                if (relIndObra.Any())
                {
                    odbcCplan.parametros = relIndObra.Select(s => new OdbcParameterDTO { nombre = "Prefijo", tipo = OdbcType.NVarChar, valor = s }).ToList();
                }
                var lst = _contextEnkontrol.Select<Core.DTO.Principal.Generales.ComboDTO>(EnkontrolAmbienteEnum.Prod, odbcCplan);
                if (esTODO)
                {
                    lst.Insert(0, new ComboDTO
                    {
                        Text = "TODOS",
                        Value = "TODOS",
                        Prefijo = "ENERO / 2020"
                    });
                }
                var cbo = (from g in lst.AsQueryable()
                           group g by g.Value into s
                           select new ComboDTO
                           {
                               Text = s.FirstOrDefault().Text,
                               Value = s.Key,
                               Prefijo = s.Select(p => p.Prefijo).ToList().ToLine(",").Replace("'", string.Empty)
                           }).Distinct().ToList();
                return cbo;
            }
            catch (Exception o_O) { return new List<Core.DTO.Principal.Generales.ComboDTO>(); }
        }
        #endregion
        /// <summary>
        /// Genra un lista de IdConceptos proyectados para sumatoria deñ cuadro
        /// </summary>
        /// <param name="idConceptoDir">idConceptoDir</param>
        /// <returns>lista de IdConceptos</returns>m
        List<int> getLstIdCptoDirDesdeIdCptoDir(int idConceptoDir)
        {
            var lstConceptoDir = getCatConceptoDirActivo().Where(w => w.idPadre >= 0).ToList();
            var lstIdCpto = new List<int>();
            if (lstConceptoDir.Any(c => c.idPadre == idConceptoDir))
            {
                lstIdCpto.AddRange(lstConceptoDir.Where(c => c.idPadre <= idConceptoDir).Select(s => s.id).ToList());
            }
            else
            {
                lstIdCpto.Add(idConceptoDir);
            }
            return lstIdCpto;
        }
        public List<Core.DTO.Contabilidad.CcDTO> lstObra()
        {
            try
            {
                var lst = _contextEnkontrol.Select<Core.DTO.Contabilidad.CcDTO>(EnkontrolAmbienteEnum.Prod, "SELECT * FROM cc");
                return lst.ToList();
            }
            catch (Exception)
            { return new List<Core.DTO.Contabilidad.CcDTO>(); }
        }
        /// <summary>
        /// Asignar area cuentas desde su centro costos
        /// </summary>
        /// <param name="lst">Movimientos polizas</param>
        void asignarACenCC(List<MovpolDTO> lst)
        {
            var lstAC = polizaDAO.getComboAreaCuenta();
            lst.ForEach(mov =>
            {
                var ac = lstAC.FirstOrDefault(a => a.Prefijo == mov.cc);
                if (ac != null)
                {
                    mov.cc = ac.Value;
                }
            });
        }
        public Dictionary<string, object> ObtenerInfoConceptos(string cc, string ac, int semana, int anio, bool esCC)
        {
            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;
                var lstConceptoDir = getCatConceptoDirActivo().Where(x => x.idPadre >= 0 && x.idPadre < 30);

                List<ConceptosPlaneacionDTO> listaConceptos = new List<ConceptosPlaneacionDTO>();
                bool editable = true;

                var existeCorte = _context.tblC_FED_CapPlaneacion.Where(x => (esCC ? x.cc.Equals(cc) : x.ac.Equals(ac)) && x.semana == semana && x.anio == anio).ToList();
                if (existeCorte != null)
                    editable = (existeCorte.Sum(x => x.corte) > 0 ? false : true);
                foreach (var item in lstConceptoDir.Where(x => x.idPadre != 0).Where(x => x.id != 9).Where(r => r.id != 14))
                {

                    ConceptosPlaneacionDTO obj = new ConceptosPlaneacionDTO();
                    var detPlan = _context.tblC_FED_PlaneacionDet.Where(x => (esCC ? x.cc.Equals(cc) : x.ac.Equals(ac)) && x.semana == semana && x.año == anio && x.concepto == item.id).ToList();
                    decimal total = detPlan.Sum(s => s.monto);
                    var padre = lstConceptoDir.First(x => x.id == item.idPadre);
                    obj.operador = item.operador.Trim();
                    obj.concepto = string.Format("({0}) {1}", item.operador.Trim(), item.Concepto);
                    obj.conceptoID = item.id;
                    obj.idPadre = "Conceptos"; //padre.Concepto;
                    obj.total = total;
                    obj.edit = editable;
                    listaConceptos.Add(obj);
                }

                resultado.Add("listaConceptos", listaConceptos);
                resultado.Add(SUCCESS, true);

                return resultado;
            }
            catch (Exception e)
            {
                LogError(0, 0, nombreControlador, "obtenerConceptos", e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al buscar los Conceptos..");
                return resultado;
            }
        }
        public Dictionary<string, object> getDescripcionesPlaneacion(int conceptoID, string cc, int semana, int anio)
        {
            try
            {
                List<PlaneacionDetDTO> listaResult = new List<PlaneacionDetDTO>();
                string query = "SELECT numcte AS value,nombre as Text FROM sx_clientes";
                var clientes = (List<ComboDTO>)ContextConstruplan.Where(query).ToObject<List<ComboDTO>>();
                var pro = polizaDAO.getProveedor();
                string inFacturas = "";
                var listaPlaneacionDet = _context.tblC_FED_PlaneacionDet.Where(x => x.cc.Equals(cc) && x.semana == semana && x.año == anio && x.concepto == conceptoID && x.estatus).ToList();
                var listaFacturas = listaPlaneacionDet.Where(x => !string.IsNullOrEmpty(x.factura) && x.estatus).Select(x => x.factura).ToList();
                var listaGastos = listaPlaneacionDet.Where(r => r.sp_gastos_provID != 0 && r.estatus).Select(x => x.sp_gastos_provID).ToList();
                var listaNomina = listaPlaneacionDet.Where(r => r.nominaID != 0 && r.estatus).Select(x => x.nominaID).ToList();
                var listaCadena = listaPlaneacionDet.Where(r => r.cadenaProductivaID != 0 && r.estatus).ToList(); //.Select(r => r.cadenaProductivaID).ToList();


                var rawGastosProv = _context.tblC_sp_gastos_prov.Where(r => listaGastos.Contains(r.idSigoplan)).Select(x => new
                {
                    x.idSigoplan,
                    x.numpro
                }).ToList();

                if (listaFacturas.Count > 0)
                {
                    listaFacturas.ForEach(r =>
                    {
                        inFacturas += r + ",";
                    });
                    string queryFacturas = string.Format(@"SELECT cte.nombre as text,cte.numcte AS prefijo,fac.factura as value FROM sf_facturas fac INNER JOIN sx_clientes cte ON cte.numcte = fac.numcte where fac.factura in ({0})", inFacturas.TrimEnd(','));
                    List<ComboDTO> rawListaFacturas = (List<ComboDTO>)ContextConstruplan.Where(queryFacturas).ToObject<List<ComboDTO>>();
                    foreach (var row in rawListaFacturas.GroupBy(x => new { x.Text, x.Prefijo }).Select(r => r))
                    {
                        PlaneacionDetDTO info = new PlaneacionDetDTO();
                        decimal total = 0;
                        var listaFacturasCliente = rawListaFacturas.Where(x => x.Prefijo == row.Key.Prefijo).Select(x => x.Value).ToList();
                        listaPlaneacionDet.Where(r => listaFacturasCliente.Contains(r.factura) && r.estatus).ToList().ForEach(t => { total += t.monto; });
                        info.descripcion = row.Key.Prefijo + " - " + row.Key.Text;
                        info.numcte = 0;
                        info.numProv = Convert.ToInt32(row.Key.Prefijo);
                        info.monto = total;
                        info.cc = cc;
                        info.detalle = true;
                        info.semana = semana;
                        info.listIDSigoplan = new List<int>();
                        info.listFacturas = listaFacturasCliente;
                        info.listaNomina = new List<int>();
                        info.listCadenasProductivas = new List<int>();
                        listaResult.Add(info);
                    }
                }



                foreach (var row in listaPlaneacionDet)
                {
                    PlaneacionDetDTO info = new PlaneacionDetDTO();
                    if (row.sp_gastos_provID == 0 && string.IsNullOrEmpty(row.factura) && row.cadenaProductivaID != 0)
                    {
                        info.descripcion = row.descripcion;
                        info.monto = row.monto;
                        info.numcte = 0;
                        info.numProv = 0;
                        info.cc = cc;
                        info.detalle = false;
                        info.semana = semana;
                        info.listIDSigoplan = new List<int>();
                        info.listFacturas = new List<string>();
                        info.listaNomina = new List<int>();
                        info.listCadenasProductivas = new List<int>();
                        listaResult.Add(info);
                    }
                }

                foreach (var row in rawGastosProv.GroupBy(x => x.numpro).Select(r => r))
                {
                    decimal total = 0;
                    PlaneacionDetDTO info = new PlaneacionDetDTO();
                    var listaGastosProv = rawGastosProv.Where(x => x.numpro == row.Key).Select(x => x.idSigoplan).ToList();
                    listaPlaneacionDet.Where(r => listaGastosProv.Contains(r.sp_gastos_provID) && r.estatus).ToList().ForEach(t => { total += t.monto; });
                    info.descripcion = getInfoProvedor(row.Key, pro);
                    info.numcte = 0;
                    info.numProv = row.Key;
                    info.monto = total;
                    info.cc = cc;
                    info.detalle = true;
                    info.semana = semana;
                    info.listIDSigoplan = listaGastosProv;
                    info.listFacturas = new List<string>();
                    info.listaNomina = new List<int>();
                    info.listCadenasProductivas = new List<int>();
                    listaResult.Add(info);
                }

                foreach (var row in listaCadena.GroupBy(x => x.numprov).Select(r => r))
                {
                    decimal total = 0;
                    PlaneacionDetDTO info = new PlaneacionDetDTO();
                    var listaCadenas = listaCadena.Where(x => x.numprov == row.Key).Select(x => x.id).ToList();
                    listaPlaneacionDet.Where(r => listaCadenas.Contains(r.id)).ToList().ForEach(t => { total += t.monto; });
                    info.descripcion = getInfoProvedor(row.Key, pro);
                    info.numcte = 0;
                    info.numProv = row.Key;
                    info.monto = total;
                    info.cc = cc;
                    info.detalle = true;
                    info.semana = semana;
                    info.listIDSigoplan = new List<int>();
                    info.listFacturas = new List<string>();
                    info.listaNomina = new List<int>();
                    info.listCadenasProductivas = listaCadena.Select(r => r.cadenaProductivaID).ToList();
                    listaResult.Add(info);
                }


                resultado.Add(SUCCESS, true);
                resultado.Add("planeacionDetalle", listaResult);

            }
            catch (Exception e)
            {
                LogError(0, 0, nombreControlador, "TablaDetallesConcepto", e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al buscar conceptos detalle planeacion.");
                return resultado;
            }
            return resultado;
        }
        #region Detalles de planeacion detalle para reporte de directo.
        List<tblC_FED_PlaneacionDet> busqPlaneacionDetParaDesglose(int conceptoID, int semana, int anio, bool esCC)
        {
            var conceptos = getLstIdCptoDirDesdeIdCptoDir(conceptoID);
            var busqSession = (BusqFlujoEfectivoDTO)session["busqFlujoEfectivo"];
            return _context.tblC_FED_PlaneacionDet
                    .Where(x => x.estatus && x.semana == semana && x.año == anio && conceptos.Contains(x.concepto))
                    .Where(x => esCC ? (busqSession.lstCC.Contains("TODOS") ? x.ac == "TODOS" : busqSession.lstCC.Contains(x.cc) && x.ac != "TODOS") : busqSession.lstAC.Contains("TODOS") ? x.ac == "TODOS" : busqSession.lstAC.Contains(x.ac) && x.ac != "TODOS").ToList();
        }
        /// <summary>
        /// Carga la información de la planeacion agrupado por los conceptos del enum tipoDetallePlaneacionEnum.cs
        /// </summary>
        /// <param name="conceptoID"></param>
        /// <param name="cc"></param>
        /// <param name="semana"></param>
        /// <returns></returns>
        public Dictionary<string, object> getDetallesPlaneacionPPal(int conceptoID, string ac, string cc, int semana, bool esCC, int anio)
        {
            try
            {
                var planeacionPpal = new List<planeacionPrincipalDTO>();
                decimal totalCadenaProductiva = 0;
                decimal totalNomina = 0;
                decimal totalGastosProv = 0;
                decimal totalEfectivoRecibido = 0;
                decimal totalNormal = 0;
                decimal totalContrato = 0;
                var listaPlaneacionDet = busqPlaneacionDetParaDesglose(conceptoID, semana, anio, esCC);
                var lstPlan = listaPlaneacionDet.GroupBy(g => new { tipo = asignarTipoDetalle(g) })
                    .Select(g => new
                    {
                        tipo = g.Key.tipo,
                        total = g.ToList().Sum(s => s.monto),
                        lst = g.ToList()
                    }).ToList();
                totalCadenaProductiva = lstPlan.Where(w => w.tipo == (int)tipoDetallePlaneacionEnum.cadenaProductiva).Sum(s => s.total);
                totalNomina = lstPlan.Where(w => w.tipo == (int)tipoDetallePlaneacionEnum.nomina).Sum(s => s.total);
                totalGastosProv = lstPlan.Where(w => w.tipo == (int)tipoDetallePlaneacionEnum.gastosProyecto).Sum(s => s.total);
                totalEfectivoRecibido = lstPlan.Where(w => w.tipo == (int)tipoDetallePlaneacionEnum.efectivoRecibido).Sum(s => s.total);
                totalNormal = lstPlan.Where(w => w.tipo == (int)tipoDetallePlaneacionEnum.manual).Sum(s => s.total);
                totalContrato = lstPlan.Where(w => w.tipo == (int)tipoDetallePlaneacionEnum.contratos).Sum(s => s.total);

                if (totalCadenaProductiva != 0)
                    planeacionPpal.Add(detalle(totalCadenaProductiva, "Cadena Productiva", conceptoID, (int)tipoDetallePlaneacionEnum.cadenaProductiva));
                if (totalNomina != 0)
                    planeacionPpal.Add(detalle(totalNomina, "Gastos Nómina", conceptoID, (int)tipoDetallePlaneacionEnum.nomina));
                if (totalGastosProv != 0)
                    planeacionPpal.Add(detalle(totalGastosProv, "Propuesta Pagó", conceptoID, (int)tipoDetallePlaneacionEnum.gastosProyecto));
                if (totalEfectivoRecibido != 0)
                    planeacionPpal.Add(detalle(totalEfectivoRecibido, "Estimaciones", conceptoID, (int)tipoDetallePlaneacionEnum.efectivoRecibido));
                if (totalNormal != 0)
                    planeacionPpal.Add(detalle(totalNormal, "Captura Manual", conceptoID, (int)tipoDetallePlaneacionEnum.manual));
                if (totalContrato != 0)
                    planeacionPpal.Add(detalle(totalContrato, "DOC X PAGAR", conceptoID, (int)tipoDetallePlaneacionEnum.contratos));

                resultado.Add("planeacionPpal", planeacionPpal);
                resultado.Add("conceptos", conceptoID);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, nombreControlador, "consultaPrincipalPlaneacion", e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al buscar los gatos provedores...");
                return resultado;
            }
            return resultado;
        }
        /// <summary>
        /// Carga la información para la planeacion segun la planeacion en base a los tipos de conceptos.
        /// </summary>
        /// <param name="conceptoID"></param>
        /// <param name="cc"></param>
        /// <param name="semana"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public Dictionary<string, object> getSubNivelDetallePlaneacion(int conceptoID, string ac, string cc, int semana, bool esCC, int anio, int tipo)
        {
            //try
            //{
                List<PlaneacionDetDTO> listaResult = new List<PlaneacionDetDTO>();
                string query = "SELECT numcte AS value,nombre as Text FROM sx_clientes";
                var clientes = (List<ComboDTO>)ContextConstruplan.Where(query).ToObject<List<ComboDTO>>();
                var pro = polizaDAO.getProveedor();
                string inFacturas = "";
                var listaPlaneacionDet = busqPlaneacionDetParaDesglose(conceptoID, semana, anio, esCC);
                var lstAC = getComboAreaCuenta();
                var lstGemId = listaPlaneacionDet.Select(s => s.idDetProyGemelo).Where(w => w > 0).ToList();
                var lstGem = _context.tblC_FED_PlaneacionDet.ToList().Where(w => lstGemId.Contains(w.id)).ToList();
                lstAC.Add(new ComboDTO { Value = "0-0", Text = "0-0 Sin asignar" });
                foreach (var plan in listaPlaneacionDet)
                {
                    var gem = lstGem.FirstOrDefault(g => g.id == plan.idDetProyGemelo);
                    if (gem != null)
                    {
                        if (plan.ac == "TODOS")
                        {
                            plan.ac = gem.ac;
                        }
                    }
                    if (plan.ac != "TODOS")
                    {
                        plan.ac = lstAC.FirstOrDefault(op => op.Value == plan.ac).Text;
                    }
                }
                //listaPlaneacionDet.ForEach(plan =>
                //{
                //    var gem = lstGem.FirstOrDefault(g => g.id == plan.idDetProyGemelo);
                //    if (gem != null)
                //    {
                //        if (plan.ac == "TODOS")
                //        {
                //            plan.ac = gem.ac;
                //        }
                //    }
                //    if (plan.ac != "TODOS")
                //    {
                //        plan.ac = lstAC.FirstOrDefault(op => op.Value == plan.ac).Text;
                //    }
                //});
                switch (tipo)
                {
                    case (int)tipoDetallePlaneacionEnum.cadenaProductiva:
                        {
                            var listaCadena = listaPlaneacionDet.Where(r => r.cadenaProductivaID != 0 && r.estatus).ToList();
                            listaResult = listaCadena.GroupBy(x => x.numprov).Select(r => new PlaneacionDetDTO()
                            {
                                descripcion = getInfoProvedor(r.Key, pro),
                                numcte = 0,
                                numProv = r.Key,
                                monto = r.Sum(s => s.monto),
                                ac = r.FirstOrDefault().ac,
                                cc = r.FirstOrDefault().cc,
                                detalle = true,
                                semana = semana,
                                listIDSigoplan = new List<int>(),
                                listFacturas = new List<string>(),
                                listaNomina = new List<int>(),
                                listCadenasProductivas = r.Select(s => s.cadenaProductivaID).ToList()
                            }).ToList();
                        }
                        break;
                    case (int)tipoDetallePlaneacionEnum.efectivoRecibido:
                        {
                            var lstFactura = listaPlaneacionDet.Where(x => !string.IsNullOrEmpty(x.factura) && x.estatus && x.cadenaProductivaID == 0 && x.nominaID == 0 && x.sp_gastos_provID == 0).ToList();
                            var listaFacturas = lstFactura.Select(x => x.factura).ToList();
                            if (listaFacturas.Count > 0)
                            {
                                listaFacturas.ForEach(r =>
                                {
                                    inFacturas += r + ",";
                                });
                                string queryFacturas = string.Format(@"SELECT cte.nombre as text,cte.numcte AS prefijo,fac.factura as value FROM sf_facturas fac INNER JOIN sx_clientes cte ON cte.numcte = fac.numcte where fac.factura in ({0})", inFacturas.TrimEnd(','));
                                List<ComboDTO> rawListaFacturas = (List<ComboDTO>)ContextConstruplan.Where(queryFacturas).ToObject<List<ComboDTO>>();
                                listaResult = rawListaFacturas.GroupBy(x => new { x.Text, x.Prefijo })
                                    .Select(row => new PlaneacionDetDTO()
                                    {
                                        descripcion = row.Key.Prefijo + " - " + row.Key.Text,
                                        numcte = Convert.ToInt32(row.Key.Prefijo),
                                        numProv = 0,
                                        monto = lstFactura.Where(w => w.numcte == row.Key.Prefijo.ParseInt()).Sum(s => s.monto),
                                        ac = lstFactura.Where(w => w.numcte == row.Key.Prefijo.ParseInt()).FirstOrDefault().ac,
                                        cc = lstFactura.Where(w => w.numcte == row.Key.Prefijo.ParseInt()).FirstOrDefault().cc,
                                        detalle = true,
                                        semana = semana,
                                        listIDSigoplan = new List<int>(),
                                        listFacturas = row.Select(s => s.Prefijo).ToList(),
                                        listaNomina = new List<int>(),
                                        listCadenasProductivas = new List<int>(),
                                    }).ToList();
                            }
                        }
                        break;
                    case (int)tipoDetallePlaneacionEnum.gastosProyecto:
                        {
                            listaPlaneacionDet = listaPlaneacionDet.Where(r => r.sp_gastos_provID != 0 && r.estatus).ToList();
                            listaPlaneacionDet = ajustarPlanDet(listaPlaneacionDet);
                            var listaGastos = listaPlaneacionDet.Select(x => x.sp_gastos_provID).ToList();
                            var rawGastosProv = _context.tblC_sp_gastos_prov.Where(r => listaGastos.Contains(r.idSigoplan)).ToList();
                            listaResult = rawGastosProv.GroupBy(x => x.numpro).OrderBy(o => o.Key)
                                .Select(row => new PlaneacionDetDTO()
                                {
                                    descripcion = getInfoProvedor(row.Key, pro),
                                    numcte = 0,
                                    numProv = row.Key,
                                    monto = listaPlaneacionDet.Where(w => w.numprov == row.Key).Distinct().Sum(s => s.monto),
                                    ac = "TODOS",
                                    cc = row.FirstOrDefault().cc,
                                    detalle = true,
                                    semana = semana,
                                    listIDSigoplan = row.Select(s => s.idSigoplan).ToList(),
                                    listFacturas = new List<string>(),
                                    listaNomina = new List<int>(),
                                    listCadenasProductivas = new List<int>(),
                                }).ToList();
                        }
                        break;
                    case (int)tipoDetallePlaneacionEnum.manual:
                        {
                            foreach (var row in listaPlaneacionDet)
                            {
                                PlaneacionDetDTO info = new PlaneacionDetDTO();
                                if (row.nominaID == 0 && row.cadenaProductivaID == 0 && row.sp_gastos_provID == 0 && string.IsNullOrEmpty(row.factura))
                                {
                                    info.descripcion = row.descripcion;
                                    info.monto = row.monto;
                                    info.numcte = 0;
                                    info.numProv = 0;
                                    info.ac = row.ac;
                                    info.cc = row.cc;
                                    info.detalle = false;
                                    info.semana = semana;
                                    info.listIDSigoplan = new List<int>();
                                    info.listFacturas = new List<string>();
                                    info.listaNomina = new List<int>();
                                    info.listCadenasProductivas = new List<int>();
                                    listaResult.Add(info);
                                }
                            }
                        }
                        break;
                    case (int)tipoDetallePlaneacionEnum.nomina:
                        {
                            var listaNomina = listaPlaneacionDet.Where(r => r.nominaID != 0 && r.estatus).Select(s => s.nominaID).ToList();
                            var lstDet = _context.tblC_FED_PlaneacionDet.ToList().Where(w => (esCC ? w.cc != "TODOS" : w.ac != "TODOS") && listaNomina.Contains(w.nominaID)).ToList();
                            foreach (var row in listaPlaneacionDet)
                            {
                                PlaneacionDetDTO info = new PlaneacionDetDTO();
                                if (row.nominaID != 0)
                                {
                                    info.descripcion = row.descripcion;
                                    info.monto = row.monto;
                                    info.numcte = 0;
                                    info.numProv = 0;
                                    info.ac = lstDet.FirstOrDefault(w => w.nominaID == row.nominaID).ac;
                                    info.cc = lstDet.FirstOrDefault(w => w.nominaID == row.nominaID).cc;
                                    info.detalle = false;
                                    info.semana = semana;
                                    info.listIDSigoplan = new List<int>();
                                    info.listFacturas = new List<string>();
                                    info.listaNomina = new List<int>();
                                    info.listCadenasProductivas = new List<int>();
                                    listaResult.Add(info);
                                }
                            }
                        }
                        break;
                    case (int)tipoDetallePlaneacionEnum.contratos:
                        {
                            var listaNomina = listaPlaneacionDet.Where(r => r.categoriaTipo == 6 && r.estatus).ToList();
                            var lstDet = _context.tblC_FED_PlaneacionDet.ToList().Where(w => (esCC ? w.cc != "TODOS" : w.ac != "TODOS")).ToList();
                            foreach (var row in listaPlaneacionDet)
                            {
                                PlaneacionDetDTO info = new PlaneacionDetDTO();

                                info.descripcion = row.descripcion;
                                info.monto = row.monto;
                                info.numcte = 0;
                                info.numProv = 0;
                                info.ac = row.ac;
                                info.cc = row.cc;
                                info.detalle = false;
                                info.semana = semana;
                                info.listIDSigoplan = new List<int>();
                                info.listFacturas = new List<string>();
                                info.listaNomina = new List<int>();
                                info.listCadenasProductivas = new List<int>();
                                listaResult.Add(info);

                            }
                        }
                        break;
                    default:
                        resultado.Add(SUCCESS, false);
                        return resultado;

                }
                decimal suma = 0;
                listaResult.ForEach(x =>
                {
                    x.tipo = tipo;
                    x.conceptoID = conceptoID;
                    suma += x.monto;
                });
                resultado.Add("suma", suma);
                resultado.Add(SUCCESS, true);
                resultado.Add("planeacionDetalle", listaResult);

            //}
            //catch (Exception e)
            //{
            //    LogError(0, 0, nombreControlador, "TablaDetallesConcepto", e, AccionEnum.CONSULTA, 0, null);
            //    resultado.Add(SUCCESS, false);
            //    resultado.Add(MESSAGE, "Ocurrió un error al buscar conceptos detalle planeacion.");
            //    return resultado;
            //}
            return resultado;
        }
        /// <summary>
        /// Muestra los detalles mas profundos por el proveedor o numero de cliente según sea su información
        /// </summary>
        /// <param name="cc"></param>
        /// <param name="semana"></param>
        /// <param name="conceptoID"></param>
        /// <param name="numProv"></param>
        /// <param name="numcte"></param>
        /// <returns></returns>
        public Dictionary<string, object> getSubDetalle(string ac, string cc, int semana, bool esCC, int anio, int conceptoID, int numProv, int numcte)
        {
            try
            {
                List<tblC_FED_PlaneacionDet> listaPlanTemp = new List<tblC_FED_PlaneacionDet>();
                var conceptos = getLstIdCptoDirDesdeIdCptoDir(conceptoID);
                var listaPlaneacionDet = busqPlaneacionDetParaDesglose(conceptoID, semana, anio, esCC);
                if (ac == "TODOS" || cc == "TODOS")
                {
                    listaPlanTemp = _context.tblC_FED_PlaneacionDet.Where(x => x.semana == semana && x.ac != ac && conceptos.Contains(x.concepto)).ToList();
                    foreach (var item in listaPlaneacionDet)
                    {
                        if (item.sp_gastos_provID != 0)
                            item.ac = listaPlanTemp.FirstOrDefault(x => x.sp_gastos_provID == item.sp_gastos_provID).ac;
                        if (item.cadenaProductivaID != 0)
                            item.ac = listaPlanTemp.FirstOrDefault(x => x.cadenaProductivaID == item.cadenaProductivaID).ac;
                        if (item.cadenaProductivaID == 0 && item.sp_gastos_provID == 0 && !string.IsNullOrEmpty(item.factura))
                            item.ac = listaPlanTemp.FirstOrDefault(x => x.factura == item.factura).ac;
                    }
                }
                if (numcte != 0)
                {
                    listaPlaneacionDet = listaPlaneacionDet.Where(r => r.numcte == numcte).ToList();

                }
                else if (numProv != 0)
                {
                    listaPlaneacionDet = listaPlaneacionDet.Where(r => r.numprov == numProv).ToList();
                }
                listaPlaneacionDet = ajustarPlanDet(listaPlaneacionDet);
                resultado.Add("planeacionDetalle", listaPlaneacionDet);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {

                LogError(0, 0, nombreControlador, "obtenerDealleDescripcionplanacionConcepto", e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al obtener los detalles del concepto...");
                return resultado;
            }
            return resultado;
        }
        List<tblC_FED_PlaneacionDet> ajustarPlanDet(List<tblC_FED_PlaneacionDet> lst)
        {
            return lst.GroupBy(g => new
            {
                g.numcte,
                g.numprov,
                g.factura,
                g.monto,
                g.cadenaProductivaID,
                g.nominaID,
                g.año,
                g.semana,
                g.cc,
                g.ac
            }).Select(s => s.LastOrDefault()).ToList();
        }
        #endregion
        #region Planeacion Detalle
        public Dictionary<string, object> getDetallePlaneacion(int concepto, string cc, string ac, int semana, int anio, int tipo, bool esCC)
        {
            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;
                var lstIdCpto = getLstIdCptoDirDesdeIdCptoDir(concepto);
                var listaConceptosDetalle = _context.tblC_FED_PlaneacionDet.Where(x => (esCC ? x.cc.Equals(cc) : x.ac.Equals(ac)) && x.semana == semana && x.año == anio && lstIdCpto.Contains(x.concepto) && x.estatus).ToList();
                if (!esCC && ac == "TODOS")
                {
                    listaConceptosDetalle.ForEach(dato =>
                    {
                        if (dato.sp_gastos_provID != 0)
                        {
                            var temporal = _context.tblC_FED_PlaneacionDet.FirstOrDefault(x => x.sp_gastos_provID == dato.sp_gastos_provID && x.estatus);
                            dato.ac = temporal.ac;
                        }
                        if (!string.IsNullOrEmpty(dato.factura))
                        {
                            var temporal = _context.tblC_FED_PlaneacionDet.FirstOrDefault(x => x.factura == dato.factura && x.estatus);
                            dato.ac = temporal.ac;
                        }
                        if (dato.nominaID != 0)
                        {
                            var temporal = _context.tblC_FED_PlaneacionDet.FirstOrDefault(x => x.nominaID == dato.nominaID && x.estatus);
                            dato.ac = temporal.ac;
                        }
                        if (dato.cadenaProductivaID != 0)
                        {
                            var temporal = _context.tblC_FED_PlaneacionDet.FirstOrDefault(x => x.cadenaProductivaID == dato.cadenaProductivaID && x.estatus);
                            dato.ac = temporal.ac;
                        }
                    });
                }
                switch (tipo)
                {
                    case (int)tipoDetallePlaneacionEnum.cadenaProductiva:
                        listaConceptosDetalle = listaConceptosDetalle.Where(x => x.cadenaProductivaID != 0).ToList();
                        break;
                    case (int)tipoDetallePlaneacionEnum.efectivoRecibido:
                        listaConceptosDetalle = listaConceptosDetalle.Where(x => !string.IsNullOrEmpty(x.factura)).ToList();
                        break;
                    case (int)tipoDetallePlaneacionEnum.gastosProyecto:
                        listaConceptosDetalle = listaConceptosDetalle.Where(x => x.sp_gastos_provID != 0).ToList();
                        break;
                    case (int)tipoDetallePlaneacionEnum.manual:
                        listaConceptosDetalle = listaConceptosDetalle.Where(x => x.cadenaProductivaID == 0 && string.IsNullOrEmpty(x.factura) && x.sp_gastos_provID == 0 && x.nominaID == 0).ToList();
                        break;
                    case (int)tipoDetallePlaneacionEnum.nomina:
                        listaConceptosDetalle = listaConceptosDetalle.Where(x => x.nominaID != 0).ToList();
                        break;
                    default:
                        break;
                }
                var lstId = listaConceptosDetalle.Select(s => s.idDetProyGemelo).Where(w => w > 0).ToList();
                var lstGemelos = _context.tblC_FED_PlaneacionDet.ToList().Where(w => lstId.Contains(w.id)).ToList();
                var lstRes = listaConceptosDetalle.Select(det => new
                {
                    id = det.id,
                    concepto = det.concepto,
                    descripcion = det.descripcion,
                    monto = det.monto,
                    cc = det.cc,
                    ac = det.ac,
                    semana = det.semana,
                    año = det.año,
                    estatus = det.estatus,
                    fechaCaptura = det.fechaCaptura,
                    usuarioCaptura = det.usuarioCaptura,
                    sp_gastos_provID = det.sp_gastos_provID,
                    factura = det.factura,
                    nominaID = det.nominaID,
                    cadenaProductivaID = det.cadenaProductivaID,
                    numcte = det.numcte,
                    numprov = det.numprov,
                    fechaFactura = det.fechaFactura,
                    idDetProyGemelo = det.idDetProyGemelo,
                    acDetProyGemelo = lstGemelos.Any(w => w.id == det.idDetProyGemelo) ? lstGemelos.FirstOrDefault(w => w.id == det.idDetProyGemelo).ac : "N/A",
                    tipo = asignarTipoDetalle(det),
                }).ToList();
                resultado.Add("listaConceptosDetalle", lstRes);
                resultado.Add(SUCCESS, true);
                return resultado;
            }
            catch (Exception e)
            {
                LogError(0, 0, nombreControlador, "ObtenerPlaneacion", e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al buscar copcentos de detalle.");
                return resultado;
            }
        }
        int asignarTipoDetalle(tblC_FED_PlaneacionDet plan)
        {
            var tipo = 0;
            if (plan.cadenaProductivaID == 0 && string.IsNullOrEmpty(plan.factura) && plan.sp_gastos_provID == 0 && plan.nominaID == 0 && plan.categoriaTipo == 0)
            {
                tipo = (int)tipoDetallePlaneacionEnum.manual;
            }
            else if (plan.cadenaProductivaID != 0)
            {
                tipo = (int)tipoDetallePlaneacionEnum.cadenaProductiva;
            }
            else if (plan.sp_gastos_provID != 0)
            {
                tipo = (int)tipoDetallePlaneacionEnum.gastosProyecto;
            }
            else if (!string.IsNullOrEmpty(plan.factura))
            {
                tipo = (int)tipoDetallePlaneacionEnum.efectivoRecibido;
            }
            else if (plan.nominaID != 0)
            {
                tipo = (int)tipoDetallePlaneacionEnum.nomina;
            }
            else if (plan.categoriaTipo > 0)
            {
                tipo = (int)tipoDetallePlaneacionEnum.contratos;
            }
            return tipo;
        }
        //Cargar lista de costos de proyecto.
        public Dictionary<string, object> getGastosProyecto(DateTime fechaInicio, DateTime fechaFin, string cc, int semana, int anio)
        {
            try
            {
                var pro = polizaDAO.getProveedor();
                var lstDivAdmin = new List<int> { (int)TipoCCEnum.Administración, (int)TipoCCEnum.GastosFininacierosYOtros };
                var ccAdministracion = polizaDAO.lstObra().Where(x => !lstDivAdmin.Contains(x.Prefijo.ParseInt())).Select(x => x.Value).ToList();
                var lstAC = polizaDAO.getComboAreaCuenta();

                var lstCC = lstAC.Where(x => (cc == "TODOS" ? true : x.Value == cc) && ccAdministracion.Contains(x.Value)).Select(s => s.Prefijo).ToList();
                var tms = getRelConceptoTm().Where(x => x.idConcepto == 6).Select(x => x.tm);

                var auxInfo = _context.tblC_sp_gastos_prov.Where(r => lstCC.Contains(r.cc) && r.fechaPropuesta != null && r.total > 0)
                    .Where(r => r.fechaPropuesta >= fechaInicio)
                    .Where(r => r.fechaPropuesta <= fechaFin).ToList();

                List<ComboDTO> lstACFactura = new List<ComboDTO>();
                if (auxInfo.Count() > 0) {
                    string query = "SELECT numpro as Value, factura as Text, (SELECT MAX(CAST(area AS varchar) + '-' + CAST(cuenta AS varchar)) FROM so_orden_compra_det WHERE cc = sp_gastos_prov.cc AND numero = sp_gastos_prov.referenciaoc ) AS Prefijo FROM sp_gastos_prov WHERE ";

                    foreach (var item in auxInfo)
                    {
                        query += string.Format("(numpro = {0} AND factura = {1})", item.numpro, item.factura);
                        if (auxInfo.LastOrDefault() != item) query += " OR ";
                    }
                    lstACFactura = _contextEnkontrol.Select<ComboDTO>(EnkontrolEnum.ArrenProd, query); 
                }

                var info = auxInfo.Select(x =>
                    {
                        //var auxAc = lstAC.FirstOrDefault(ac => ac.Prefijo.ToUpper() == x.cc.ToUpper());
                        var auxAc = lstACFactura.FirstOrDefault(y => y.Value == x.numpro.ToString() && y.Text == x.factura.ToString());
                        return new tblC_FED_PlaneacionDet
                        {
                            id = 0,
                            concepto = 6,
                            descripcion = getInfoProvedor(x.numpro, pro),
                            estatus = true,
                            fechaCaptura = DateTime.Now,
                            año = anio,
                            ac = auxAc == null ? "" : auxAc.Prefijo,
                            cc = x.cc,
                            monto = -Convert.ToDecimal(x.tipocambio != 0 ? x.tipocambio * x.total : x.total),
                            semana = semana,
                            sp_gastos_provID = x.idSigoplan,
                            factura = x.factura,
                            numprov = (int)x.numpro,
                            numcte = 0,
                            fechaFactura = x.fecha.ToShortDateString()
                        };
                    }).ToList();
                var gastosProv = _context.tblC_FED_PlaneacionDet.Where(x => x.cc == cc && x.año == anio && x.semana == semana && x.sp_gastos_provID != 0 && x.estatus).Select(r => r.sp_gastos_provID).ToList();
                resultado.Add("listaPlaneacion", info.Where(x => !gastosProv.Contains(x.sp_gastos_provID) && x.monto != 0).OrderBy(x => x.cc).ThenBy(x => x.descripcion).ToList());
                resultado.Add(SUCCESS, true);
                return resultado;
            }
            catch (Exception e)
            {
                LogError(0, 0, nombreControlador, "costosProyecto", e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al buscar los gatos provedores...");
                return resultado;
            }
        }
        //Carga pagos de nomina
        public Dictionary<string, object> getCargaNomina(List<PeriodosNominaDTO> lstPeriodo, string cc, int semana, int anio)
        {
            try
            {
                var fechaFin = lstPeriodo.Max(m => m.fecha_final);
                var economicos = _context.tblP_CC.Where(x => x.estatus && x.cc != "0").Select(x => new ComboDTO { Value = x.areaCuenta, Text = x.descripcion }).ToList();
                var lstAC = polizaDAO.getComboAreaCuenta();
                var lstCC = lstAC.Where(w => cc == "TODOS" ? true : cc == w.Value).Select(s => s.Prefijo).ToList();
                var lstCatalogoCCNomina = _context.tblC_Nom_CatalogoCC.ToList();
                var info = _context.tblC_NominaPoliza.ToList()
                    //.Where(r => r.tipoCuenta == (int)tipoCuentaNominaEnum.Arrendadora)
                    .Where(r => lstCC.Contains(r.cc) && r.esActivo)
                    .Where(r => lstPeriodo.Exists(p => r.fecha >= p.fecha_inicial && r.fecha <= p.fecha_final && r.tipoNomina == p.tipo_nomina)).ToList()
                    .GroupBy(r => new { r.cc, r.tipoNomina })
                    .Select(x =>
                    {
                        var auxAC = lstCatalogoCCNomina.FirstOrDefault(y => y.cc == x.Key.cc);
                        return new tblC_FED_PlaneacionDet
                        {
                            id = 0,
                            concepto = 7,
                            descripcion = string.Format("CARGO {0} - {1}", ((tipoNominaPropuestaEnum)x.Key.tipoNomina).GetDescription(), getDescripcionCC(lstAC.FirstOrDefault(ac => ac.Prefijo == x.Key.cc).Value, lstAC.FirstOrDefault(ac => ac.Prefijo == x.Key.cc), economicos)),
                            estatus = true,
                            fechaCaptura = DateTime.Now,
                            año = anio,
                            ac = auxAC == null ? "" : (auxAC.area ?? 0).ToString() + "-" + (auxAC.cuenta ?? 0).ToString(),
                            cc = x.Key.cc,
                            monto = -(x.Sum(y => y.cargo) - x.Sum(y => y.abono) + x.Sum(y => y.iva) - x.Sum(y => y.retencion)),
                            semana = semana,
                            sp_gastos_provID = 0,
                            nominaID = x.FirstOrDefault().id,
                            cadenaProductivaID = 0,
                            numcte = 0,
                            numprov = 0,
                            fechaFactura = ""
                        };
                    }).ToList();
                var gastosNomina = _context.tblC_FED_PlaneacionDet.Where(x => x.ac == cc && x.año == anio && x.semana == semana && x.nominaID != 0 && x.estatus).Select(r => r.nominaID).ToList();
                resultado.Add("listaPlaneacion", info.Where(x => !gastosNomina.Contains(x.nominaID)).OrderBy(x => x.cc).ThenBy(x => x.descripcion).ToList());
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, nombreControlador, "CargarNominas", e, AccionEnum.ACTUALIZAR, 0, null);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al guardar los operadores de detalle planeacion.");
            }
            return resultado;
        }
        //Carga lista de cadena productiva.
        public Dictionary<string, object> getCadenasProductivas(DateTime fechaInicio, DateTime fechaFin, string ac, string cc, int semana, int anio, bool esCC)
        {
            try
            {
                var info = _context.tblC_CadenaProductiva
                    .Where(r => esCC ? r.centro_costos == cc : ac == "TODOS" ? true : r.area_cuenta == ac)
                    .Where(r => r.saldoFactura > 0)
                    .Where(r => r.fechaVencimiento >= fechaInicio)
                    .Where(r => r.fechaVencimiento <= fechaFin).ToList()
                    .Select(x => new tblC_FED_PlaneacionDet
                    {
                        id = 0,
                        concepto = 6,
                        descripcion = x.proveedor,
                        estatus = true,
                        fechaCaptura = DateTime.Now,
                        año = anio,
                        ac = x.area_cuenta,
                        cc = x.centro_costos,
                        monto = -(x.tipoCambio != 0 ? x.saldoFactura * x.tipoCambio : x.saldoFactura),
                        semana = semana,
                        sp_gastos_provID = 0,
                        nominaID = 0,
                        cadenaProductivaID = x.id,
                        factura = x.factura,
                        numprov = Convert.ToInt32(x.numProveedor),
                        numcte = 0,
                        fechaFactura = x.fechaVencimiento.ToShortDateString()
                    }).ToList();
                var listaCadenas = _context.tblC_FED_PlaneacionDet.Where(x => (esCC ? x.cc == cc : ac == "TODOS" ? true : x.ac == ac) && x.año == anio && x.semana == semana && x.cadenaProductivaID != null && x.estatus).Select(r => r.cadenaProductivaID).ToList();
                resultado.Add("listaPlaneacion", info.Where(r => !listaCadenas.Contains(r.cadenaProductivaID)));
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {

                LogError(0, 0, nombreControlador, "CadenasProductivas", e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al buscar los gatos provedores...");
                return resultado;
            }
            return resultado;
        }
        //Cargar lista de Efectivo Recibido
        public Dictionary<string, object> getEfectivoRecibido(DateTime fechaInicio, DateTime fechaFin, string cc, string ac, int semana, int anio, bool esCC)
        {
            try
            {
                string query = string.Format(@"SELECT  numcte,factura,cc,area,total,nombre ,descripcion,DATEFORMAT( CAST ( fechafac AS DATE ), 'dd/mm/yyyy' ) as fechafac FROM (SELECT cte.nombre, mov.numcte, mov.factura, mov.cc,fac.area ,fac.cuenta, MAX(mov.fechavenc) AS fechavenc,fac.fecha as fechafac,
                                                SUM(CASE WHEN det.insumo IN (9010001 ,9010003 ,9010006 ,9010007) AND mov.fechavenc <= '{0}' THEN mov.total*mov.tipocambio ELSE 0 END) AS total,
                                                MAX(det.linea) AS linea,('ESTMIACION DE OBRA '+cte.nombre) as descripcion
                                                    FROM sx_movcltes mov
                                                    INNER JOIN sf_facturas fac ON fac.factura = mov.factura AND fac.numcte = mov.numcte AND fac.cc = mov.cc AND mov.referenciaoc = fac.numero_nc
                                                    INNER JOIN sf_facturas_det det ON  det.factura = fac.factura AND fac.numero_nc = det.numero_nc AND det.cia_sucursal = fac.cia_sucursal
                                                    INNER JOIN sx_clientes cte ON cte.numcte = mov.numcte
                                                    GROUP BY  mov.numcte ,mov.factura ,mov.cc,fac.area ,fac.cuenta, cte.nombre,fac.fecha
                                                    ORDER BY mov.cc,fac.area ,fac.cuenta ,mov.numcte ,mov.factura) x
                                                WHERE (x.total) NOT BETWEEN -1 AND 1 AND fechavenc <= '{0}' AND x.total >0
                                            ORDER BY nombre,cc"
                    , fechaFin.ToString("yyyyMMdd"));
                var efectivoRecibido = _contextEnkontrol.Select<efectivoRecibidoDTO>(EnkontrolEnum.ArrenProd, query);
                efectivoRecibido.ForEach(factura =>
                {
                    factura.area = factura.area.ParseInt();
                    factura.cuenta = factura.cuenta.ParseInt();
                    if (factura.area == 0 || factura.cuenta == 0)
                    {
                        factura.area = 0;
                        factura.cuenta = 0;
                    }
                });
                if (!esCC && ac != "TODOS")
                {
                    efectivoRecibido = efectivoRecibido.Where(w => ac == w.area.ToString() + "-" + w.cuenta.ToString()).ToList();
                }
                if (esCC && cc != "TODOS")
                {
                    efectivoRecibido = efectivoRecibido.Where(w => cc == w.cc).ToList();
                }
                var listaFacturas = _context.tblC_FED_PlaneacionDet.Where(x => x.cc == cc && x.año == anio && x.semana == semana && x.factura != null && x.estatus).Select(r => r.factura).ToList();
                efectivoRecibido = efectivoRecibido.Where(f => !listaFacturas.Contains(f.factura)).OrderBy(x => x.cc).ThenBy(x => x.nombre).ToList();
                resultado.Add("listaPlaneacion", efectivoRecibido);
                resultado.Add(SUCCESS, true);
                return resultado;
            }
            catch (Exception e)
            {
                LogError(0, 0, nombreControlador, "efectivoRecibido", e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al buscar Efectivo Recibido...");
                return resultado;
            }
        }
        //Cargar lista de Gastos Operativos
        public Dictionary<string, object> getGastosOperativos(DateTime fechaInicio, DateTime fechaFin, string cc, int semana, int anio)
        {
            try
            {
                var pro = polizaDAO.getProveedor();
                var lstDivAdmin = new List<int> { (int)TipoCCEnum.Administración, (int)TipoCCEnum.GastosFininacierosYOtros };
                var ccAdministracion = polizaDAO.lstObra().Where(x => lstDivAdmin.Contains(x.Prefijo.ParseInt())).Select(x => x.Value).ToList();
                var lstAC = polizaDAO.getComboAreaCuenta();
                var lstCC = lstAC.Where(x => (cc == "TODOS" ? true : x.Value == cc) && ccAdministracion.Contains(x.Value)).Select(s => s.Prefijo).ToList();
                var info = _context.tblC_sp_gastos_prov.Where(r => lstCC.Contains(r.cc) && r.fechaPropuesta != null && r.total > 0)
                    .Where(r => r.fechaPropuesta >= fechaInicio)
                    .Where(r => r.fechaPropuesta <= fechaFin).ToList()
                    .Select(x => new tblC_FED_PlaneacionDet
                    {
                        id = 0,
                        concepto = 7,
                        descripcion = getInfoProvedor(x.numpro, pro),
                        estatus = true,
                        fechaCaptura = DateTime.Now,
                        año = anio,
                        ac = lstAC.FirstOrDefault(ac => ac.Prefijo == x.cc).Value,
                        cc = x.cc,
                        monto = -(Convert.ToDecimal(x.tipocambio) != 0 ? x.total * Convert.ToDecimal(x.tipocambio) : x.total),
                        semana = semana,
                        sp_gastos_provID = x.idSigoplan,
                        numprov = (int)x.numpro,
                        numcte = 0,
                        fechaFactura = x.fecha.ToShortDateString(),
                        factura = x.factura
                    }).ToList();
                var gastosProv = _context.tblC_FED_PlaneacionDet.Where(x => x.cc == cc && x.año == anio && x.semana == semana && x.sp_gastos_provID != 0 && x.estatus).Select(r => r.sp_gastos_provID).ToList();
                resultado.Add("listaPlaneacion", info.Where(x => !gastosProv.Contains(x.sp_gastos_provID)).OrderBy(x => x.cc).ThenBy(x => x.descripcion).ToList());
                resultado.Add(SUCCESS, true);
                return resultado;
            }
            catch (Exception e)
            {
                LogError(0, 0, nombreControlador, "costosProyecto", e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al buscar los gatos provedores...");
                return resultado;
            }
        }
        //Guardado de información
        public Dictionary<string, object> saveOrUpdateDetalle(List<tblC_FED_PlaneacionDet> lst)
        {
            //using(var dbTransaction = _context.Database.BeginTransaction())
            //{
            try
            {
                var lstIds = lst.Select(x => x.id).ToList();
                var _lst = _context.tblC_FED_PlaneacionDet.Where(x => lstIds.Contains(x.id)).ToList();
                var _primero = _lst.FirstOrDefault();
                var primero = lst.FirstOrDefault();

                if (_primero != null && primero.ac != _primero.ac) primero.ac = _primero.ac;
                if (_primero != null && primero.cc != _primero.cc) primero.cc = _primero.cc;

                if (lst.Count == 1)
                {
                    if (_primero.idDetProyGemelo > 0)
                    {
                        var gem = _context.tblC_FED_PlaneacionDet.FirstOrDefault(w => w.id == _primero.idDetProyGemelo);
                        if (gem != null)
                        {
                            gem.estatus = primero.estatus;
                            lst.Add(gem);
                        }
                    }
                    else
                    {
                        var gem = _context.tblC_FED_PlaneacionDet.FirstOrDefault(x => x.semana == _primero.semana && x.año == _primero.año && x.estatus == _primero.estatus
                            && x.sp_gastos_provID == _primero.sp_gastos_provID && x.factura == _primero.factura && x.nominaID == _primero.nominaID && x.cadenaProductivaID == _primero.cadenaProductivaID
                            && x.numcte == _primero.numcte && x.numprov == _primero.numprov && x.fechaFactura == _primero.fechaFactura && x.ac != _primero.ac);
                        if (gem != null)
                        {
                            gem.estatus = primero.estatus;
                            lst.Add(gem);
                        }
                    }
                }
                if (_primero != null)
                {

                    var _ids = lst.Select(x => x.id).ToList();
                    var _listBD = _context.tblC_FED_PlaneacionDet.Where(x => _ids.Contains(x.id)).ToList();

                    var primeroBD = _listBD.FirstOrDefault(x => x.ac != "TODOS");
                    var primeroListaOriginal = lst.FirstOrDefault();

                    if ((primeroBD.concepto == 6 || primeroBD.concepto == 7) && primeroBD.sp_gastos_provID != 0 && !primeroListaOriginal.estatus)
                    {
                        var _facturasBorrarENKONTROL = _listBD.Where(x => x.ac != "TODOS").ToList();
                        bool tienePago = false;

                        foreach (var item in _facturasBorrarENKONTROL)
                        {
                            if (item.numprov > 0 && item.factura != null)
                            {
                                var odbcCplan = new OdbcConsultaDTO()
                                {
                                    consulta = @"SELECT * FROM sp_movprov WHERE numpro = ? AND factura = ? AND tm = 51",
                                    parametros = new List<OdbcParameterDTO>() {
                                new OdbcParameterDTO() { nombre = "numpro", tipo = OdbcType.Numeric, valor = item.numprov },
                                new OdbcParameterDTO() { nombre = "factura", tipo = OdbcType.VarChar, valor = item.factura },
                            }
                                };
                                var pagos = _contextEnkontrol.Select<Core.DTO.Principal.Generales.ComboDTO>(EnkontrolAmbienteEnum.Prod, odbcCplan);

                                tienePago = pagos.Count() > 0;

                                if (tienePago)
                                {
                                    throw new Exception("No se puede eliminar la factura solicitada puesto que ya cuenta con un pago en sistema.");
                                }

                                odbcCplan = new OdbcConsultaDTO()
                                {
                                    consulta = "UPDATE sp_movprov SET autorizapago = ' ' WHERE numpro = ? AND factura = ?",
                                    parametros = new List<OdbcParameterDTO>() {
                                new OdbcParameterDTO() { nombre = "numpro", tipo = OdbcType.Numeric, valor = item.numprov },
                                new OdbcParameterDTO() { nombre = "factura", tipo = OdbcType.VarChar, valor = item.factura },
                            }
                                };
                                var res = _contextEnkontrol.Save(EnkontrolAmbienteEnum.ProdCPLAN, odbcCplan);
                            }
                        }

                        var facturaBorrarSIGOPLAN = _context.tblC_sp_gastos_prov.FirstOrDefault(x => x.idSigoplan == _primero.sp_gastos_provID);
                        if (facturaBorrarSIGOPLAN != null)
                        {
                            _context.tblC_sp_gastos_prov.Remove(facturaBorrarSIGOPLAN);
                            _context.SaveChanges();
                        }
                    }
                }
                
                
                //var primero = lst.FirstOrDefault();
                //if (lst.Count == 1 && primero.idDetProyGemelo > 0)
                //{
                //    var gem = _context.tblC_FED_PlaneacionDet.FirstOrDefault(w => w.id == primero.idDetProyGemelo);
                //    gem.estatus = primero.estatus;
                //    lst.Add(gem);
                //}
                int usuarioCreadorID = vSesiones.sesionUsuarioDTO.id;
                lst.ForEach(nuevo =>
                {
                    if (nuevo.estatus)
                    {
                        if (nuevo.id > 0)
                        {
                            var bd = _context.tblC_FED_PlaneacionDet.FirstOrDefault(x => x.id == nuevo.id);
                            nuevo.id = bd.id;
                            _context.tblC_FED_PlaneacionDet.AddOrUpdate(nuevo);
                            _context.SaveChanges();
                        }
                        else
                        {
                            nuevo.usuarioCaptura = usuarioCreadorID;
                            nuevo.fechaCaptura = DateTime.Now;
                            _context.tblC_FED_PlaneacionDet.Add(nuevo);
                            _context.SaveChanges();
                        }
                        if (lst.Count > 1 && lst.LastOrDefault().id == 0)
                        {
                            var maxId = nuevo.id;
                            lst.LastOrDefault().idDetProyGemelo = maxId;
                            lst.FirstOrDefault().idDetProyGemelo = maxId + 1;
                        }
                        var fed_capPlaneacion = _context.tblC_FED_CapPlaneacion.FirstOrDefault(x => x.idConceptoDir == nuevo.concepto && x.anio == nuevo.año && x.semana == nuevo.semana && x.esActivo && x.ac == x.cc && x.cc == nuevo.cc);
                        if (fed_capPlaneacion != null)
                        {
                            var lstDet = _context.tblC_FED_PlaneacionDet.ToList().Where(w => w.estatus && w.concepto == nuevo.concepto && w.año == nuevo.año && w.semana == nuevo.semana && w.ac == nuevo.ac && w.cc == nuevo.cc);
                            fed_capPlaneacion.planeado = lstDet.Sum(s => s.monto);
                            _context.SaveChanges();
                        }
                        else
                        {
                            var fecha = Infrastructure.Utils.DatetimeUtils.primerDiaSemana(nuevo.año, nuevo.semana);
                            fecha = fecha.Siguiente(DayOfWeek.Saturday);
                            tblC_FED_CapPlaneacion nuevoRegistro = new tblC_FED_CapPlaneacion();
                            nuevoRegistro.id = 0;
                            nuevoRegistro.anio = nuevo.año;
                            nuevoRegistro.cc = nuevo.cc;
                            nuevoRegistro.ac = nuevo.ac;
                            nuevoRegistro.corte = 0;
                            nuevoRegistro.esActivo = true;
                            nuevoRegistro.fecha = fecha;
                            nuevoRegistro.fechaRegistro = DateTime.Now;
                            nuevoRegistro.flujoTotal = 0;
                            nuevoRegistro.idConceptoDir = nuevo.concepto;
                            nuevoRegistro.planeado = nuevo.monto;
                            nuevoRegistro.semana = nuevo.semana;
                            nuevoRegistro.strFlujoEfectivo = "0";
                            _context.tblC_FED_CapPlaneacion.Add(nuevoRegistro);
                            _context.SaveChanges();
                        }
                    }
                    else
                    {
                        var actualizacionBanco = _context.tblC_FED_PlaneacionDet.FirstOrDefault(x => x.id == nuevo.id);
                        if (actualizacionBanco != null)
                        {
                            actualizacionBanco.descripcion = actualizacionBanco.descripcion;
                            actualizacionBanco.monto = nuevo.monto;
                            actualizacionBanco.estatus = nuevo.estatus;
                            //_context.tblC_FED_PlaneacionDet.Remove(actualizacionBanco);
                            var lstDet = _context.tblC_FED_PlaneacionDet.Where(w => w.estatus && w.concepto == nuevo.concepto && w.año == nuevo.año && w.semana == nuevo.semana && w.ac == nuevo.ac && w.cc == nuevo.cc).ToList();
                            var fed_capPlaneacion = _context.tblC_FED_CapPlaneacion.FirstOrDefault(x => x.idConceptoDir == actualizacionBanco.concepto && x.anio == actualizacionBanco.año && x.semana == actualizacionBanco.semana && x.ac == nuevo.ac && x.esActivo && x.cc == actualizacionBanco.cc);
                            if (fed_capPlaneacion != null) fed_capPlaneacion.planeado = lstDet.Sum(s => s.monto);
                            _context.tblC_FED_PlaneacionDet.Remove(actualizacionBanco);
                            _context.SaveChanges();
                            if (!string.IsNullOrEmpty(actualizacionBanco.factura))
                            {
                                List<tblC_FED_PlaneacionDet> temporales = _context.tblC_FED_PlaneacionDet.Where(x => x.factura == actualizacionBanco.factura).ToList();
                                temporales.ForEach(r => { r.estatus = false; r.monto = 0; });
                                _context.tblC_FED_PlaneacionDet.RemoveRange(temporales);
                                _context.SaveChanges();
                            }
                            if (actualizacionBanco.sp_gastos_provID != 0)
                            {
                                List<tblC_FED_PlaneacionDet> temporales = _context.tblC_FED_PlaneacionDet.Where(x => x.sp_gastos_provID == actualizacionBanco.sp_gastos_provID).ToList();
                                temporales.ForEach(r =>
                                {
                                    r.estatus = false;
                                    r.monto = 0;
                                });
                                _context.tblC_FED_PlaneacionDet.RemoveRange(temporales);
                                _context.SaveChanges();
                            }
                            if (actualizacionBanco.cadenaProductivaID != 0)
                            {
                                List<tblC_FED_PlaneacionDet> temporales = _context.tblC_FED_PlaneacionDet.Where(x => x.cadenaProductivaID == actualizacionBanco.cadenaProductivaID).ToList();
                                temporales.ForEach(r =>
                                {
                                    r.estatus = false;
                                    r.monto = 0;
                                });
                                _context.tblC_FED_PlaneacionDet.RemoveRange(temporales);
                                _context.SaveChanges();
                            }
                            if (actualizacionBanco.nominaID != 0)
                            {
                                List<tblC_FED_PlaneacionDet> temporales = _context.tblC_FED_PlaneacionDet.Where(x => x.nominaID == actualizacionBanco.nominaID).ToList();
                                temporales.ForEach(r =>
                                {
                                    r.estatus = false;
                                    r.monto = 0;
                                });
                                _context.tblC_FED_PlaneacionDet.RemoveRange(temporales);
                                _context.SaveChanges();
                            }
                        }
                    }
                });
                resultado.Add("nuevo", lst.Select(det => new
                {
                    id = det.id,
                    concepto = det.concepto,
                    descripcion = det.descripcion,
                    monto = det.monto,
                    ac = det.ac,
                    cc = det.cc,
                    semana = det.semana,
                    año = det.año,
                    estatus = det.estatus,
                    fechaCaptura = det.fechaCaptura,
                    usuarioCaptura = det.usuarioCaptura,
                    sp_gastos_provID = det.sp_gastos_provID,
                    factura = det.factura,
                    nominaID = det.nominaID,
                    cadenaProductivaID = det.cadenaProductivaID,
                    numcte = det.numcte,
                    numprov = det.numprov,
                    fechaFactura = det.fechaFactura,
                    idDetProyGemelo = det.idDetProyGemelo,
                    ccDetProyGemelo = lst.Any(w => w.id == det.idDetProyGemelo) ? lst.FirstOrDefault(w => w.id == det.idDetProyGemelo).ac : "N/A",
                    tipo = asignarTipoDetalle(det),
                }).ToList());
                _context.SaveChanges();
                //dbTransaction.Commit();
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                //dbTransaction.Rollback();
                LogError(0, 0, nombreControlador, "GuardarDetallePlaneacion", e, AccionEnum.ACTUALIZAR, 0, lst.FirstOrDefault());
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al guardar los operadores de detalle planeacion.");
            }
            return resultado;
            //}
        }
        public Dictionary<string, object> saveDetallesMasivos(List<tblC_FED_PlaneacionDet> listaPlaneacionDet, int idConceptoDir, int anio, int semana, string ac, string cc)
        {
            //using(var dbTransaction = _context.Database.BeginTransaction())
            try
            {
                List<tblC_FED_PlaneacionDet> tempPlaneacion = new List<tblC_FED_PlaneacionDet>();
                decimal planeado = 0;
                int usuarioCreadorID = vSesiones.sesionUsuarioDTO.id;
                listaPlaneacionDet.ForEach(x =>
                {
                    x.usuarioCaptura = usuarioCreadorID;
                    x.fechaCaptura = DateTime.Now;
                    x.semana = semana;
                    x.año = anio;
                    x.concepto = idConceptoDir;
                    x.estatus = true;
                });
                _context.tblC_FED_PlaneacionDet.AddRange(listaPlaneacionDet);
                _context.SaveChanges();
                foreach (var temp in listaPlaneacionDet)
                {
                    tblC_FED_PlaneacionDet tempNe = new tblC_FED_PlaneacionDet();
                    tempNe.año = temp.año;
                    tempNe.cc = temp.cc;
                    tempNe.ac = "TODOS";
                    tempNe.concepto = temp.concepto;
                    tempNe.descripcion = temp.descripcion;
                    tempNe.estatus = temp.estatus;
                    tempNe.factura = temp.factura;
                    tempNe.fechaCaptura = temp.fechaCaptura;
                    tempNe.monto = temp.monto;
                    tempNe.semana = temp.semana;
                    tempNe.sp_gastos_provID = temp.sp_gastos_provID;
                    tempNe.usuarioCaptura = temp.usuarioCaptura;
                    tempNe.fechaFactura = temp.fechaFactura;
                    tempNe.cadenaProductivaID = temp.cadenaProductivaID;
                    tempNe.nominaID = temp.nominaID;
                    tempNe.numcte = temp.numcte;
                    tempNe.numprov = temp.numprov;
                    tempNe.categoriaTipo = temp.categoriaTipo;
                    tempPlaneacion.Add(tempNe);
                }
                _context.tblC_FED_PlaneacionDet.AddRange(tempPlaneacion);
                _context.SaveChanges();
                var lstAC = listaPlaneacionDet.GroupBy(x => x.ac).Select(r => r.Key).ToList();
                lstAC.Add("TODOS");
                foreach (var itemAC in lstAC)
                {
                    decimal motoTotalPlaneado = 0;
                    var listaTotales = _context.tblC_FED_PlaneacionDet.Where(x => x.concepto == idConceptoDir && x.año == anio && x.semana == semana && itemAC == x.ac && x.estatus).ToList();
                    motoTotalPlaneado = listaTotales.Sum(s => s.monto);
                    var centroCosto = listaTotales.LastOrDefault(p => p.ac == itemAC).cc;
                    var fed_capPlaneacionCC = _context.tblC_FED_CapPlaneacion.FirstOrDefault(x => x.idConceptoDir == idConceptoDir && x.anio == anio && x.semana == semana && x.esActivo && x.ac == itemAC && x.cc == centroCosto);
                    if (fed_capPlaneacionCC == null)
                    {
                        var fecha = Infrastructure.Utils.DatetimeUtils.primerDiaSemana(anio, semana);
                        fecha = fecha.Siguiente(DayOfWeek.Saturday);
                        tblC_FED_CapPlaneacion nuevoRegistro = new tblC_FED_CapPlaneacion();
                        nuevoRegistro.id = 0;
                        nuevoRegistro.anio = anio;
                        nuevoRegistro.ac = itemAC;
                        nuevoRegistro.cc = centroCosto;
                        nuevoRegistro.corte = 0;
                        nuevoRegistro.esActivo = true;
                        nuevoRegistro.fecha = fecha;
                        nuevoRegistro.fechaRegistro = DateTime.Now;
                        nuevoRegistro.flujoTotal = 0;
                        nuevoRegistro.idConceptoDir = idConceptoDir;
                        nuevoRegistro.planeado = motoTotalPlaneado;
                        nuevoRegistro.semana = semana;
                        nuevoRegistro.strFlujoEfectivo = "0";
                        _context.tblC_FED_CapPlaneacion.Add(nuevoRegistro);
                        _context.SaveChanges();
                    }
                    else
                    {
                        fed_capPlaneacionCC.planeado = motoTotalPlaneado;
                        _context.SaveChanges();
                    }
                }
                //dbTransaction.Commit();
                resultado.Add(SUCCESS, true);
                resultado.Add("concepto", idConceptoDir);
                resultado.Add("ac", ac);
                resultado.Add("cc", cc);
                resultado.Add("semana", semana);
            }
            catch (Exception e)
            {
                //dbTransaction.Rollback();
                LogError(0, 0, nombreControlador, "GuardarDetallePlaneacion", e, AccionEnum.ACTUALIZAR, 0, null);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al guardar los operadores de detalle planeacion.");
            }
            return resultado;
        }
        //fin
        private planeacionPrincipalDTO detalle(decimal monto, string descripcion, int conceptoID, int tipoConcepto)
        {
            return new planeacionPrincipalDTO
            {
                descripcion = descripcion,
                monto = monto,
                conceptoID = conceptoID,
                tipoConcepto = tipoConcepto
            };
        }
        private string getDescripcionCC(string cc, Core.DTO.Principal.Generales.ComboDTO combo, List<ComboDTO> obj)
        {
            var d = combo == null ? "-1" : combo.Value;
            var objCC = obj.FirstOrDefault(r => r.Value == d);
            return objCC != null ? cc + " " + objCC.Text : cc;
        }
        private string getInfoProvedor(Int64 numpro, List<ProveedorDTO> prov)
        {
            var result = prov.FirstOrDefault(x => x.numpro == numpro);
            return result != null ? numpro + " - " + result.nombre : "";
        }
        public Dictionary<string, object> getListaClientes()
        {
            try
            {
                string query = "SELECT numcte AS value,nombre as Text FROM sx_clientes";
                var comboDTO = (List<ComboDTO>)ContextConstruplan.Where(query).ToObject<List<ComboDTO>>();

                resultado.Add("comboDTO", comboDTO);
                resultado.Add(SUCCESS, true);
                return resultado;
            }
            catch (Exception e)
            {
                LogError(0, 0, nombreControlador, "listaClientes", e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al cargar los lientes...");
                return resultado;
            }
        }

        ///-1
        public Dictionary<string, object> getDetalleDescripcionPlaneacion(PlaneacionDetDTO planeacionDetDTO, string cc)
        {
            try
            {
                if (planeacionDetDTO.listFacturas != null)
                {
                    var resultFacturas = _context.tblC_FED_PlaneacionDet.Where(x => planeacionDetDTO.listFacturas.Contains(x.factura)).Where(x => x.estatus && cc == "TODOS" ? x.cc != "TODOS" : x.cc == cc).ToList();//.ToList();
                    resultado.Add("listaConceptosDetalle", resultFacturas);
                    resultado.Add(SUCCESS, true);
                }
                else if (planeacionDetDTO.listIDSigoplan.Count > 0)
                {
                    var resultGastos = _context.tblC_FED_PlaneacionDet.Where(x => planeacionDetDTO.listIDSigoplan.Contains(x.sp_gastos_provID)).Where(x => x.estatus && cc == "TODOS" ? x.cc != "TODOS" : x.cc == cc).ToList();
                    resultado.Add("listaConceptosDetalle", resultGastos);
                    resultado.Add(SUCCESS, true);
                }
                else
                {
                    resultado.Add(MESSAGE, "Ocurrió un error al obtener los detalles del concepto...");
                    resultado.Add(SUCCESS, false);
                }
            }
            catch (Exception e)
            {

                LogError(0, 0, nombreControlador, "obtenerDealleDescripcionplanacionConcepto", e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al obtener los detalles del concepto...");
                return resultado;
            }
            return resultado;
        }
        #endregion
        #region Solo Arrendadora
        public BusqFlujoEfectivoDTO setAreaCuentaPorCentroCostos(BusqFlujoEfectivoDTO busq)
        {
            using (var _ctxArrendadora = new MainContext(EmpresaEnum.Arrendadora))
            {
                var esTodos = busq.lstCC.Contains("TODOS");
                busq.lstAC = (from obra in _ctxArrendadora.tblP_CC
                              where obra.estatus && obra.areaCuenta != null && (esTodos ? true : busq.lstCC.Contains(obra.cc))
                              select obra.areaCuenta).ToList();
            }
            return busq;
        }
        public decimal getFlujoEfectivoTotalArrendadora(BusqFlujoEfectivoDTO busq)
        {
            using (var _ctxArrendadora = new MainContext(EmpresaEnum.Arrendadora))
            {
                var anio = busq.max.Year;
                var semana = busq.max.noSemana();
                var esTodo = busq.lstAC.Contains("TODOS");
                var lstCpto = _ctxArrendadora.tblC_FED_CatConcepto.AsQueryable();
                var lstCptoPadre = from padre in lstCpto where padre.idPadre == 0 select padre.id;
                var lstCptoFlujo = from cpto in lstCpto where lstCptoPadre.Contains(cpto.idPadre) select cpto.id;
                var lstFlujo = from flujo in _ctxArrendadora.tblC_FED_CapPlaneacion.AsQueryable()
                               where flujo.esActivo && flujo.anio == anio && flujo.semana == semana && lstCptoFlujo.Contains(flujo.idConceptoDir) && (esTodo ? flujo.ac == "TODOS" : busq.lstAC.Contains(flujo.ac))
                               select flujo;
                var total = lstFlujo.ToList().Sum(flujo => flujo.strFlujoEfectivo.ParseDecimal(0) + (esTodo ? flujo.strSaldoInicial.ParseDecimal(0) : (busq.esFlujo ? flujo.strSaldoInicial.ParseDecimal(0) : 0)));
                return total;
            }
        }
        public List<tblC_FED_DetProyeccionCierre> getPlaneacionCierreArrendadora(BusqProyeccionCierreDTO busq)
        {
            using (var _ctxArrendadora = new MainContext(EmpresaEnum.Arrendadora))
            {
                var idReserva = 29;
                var anio = busq.max.Year;
                var semana = busq.max.noSemana();
                var lst = new List<tblC_FED_DetProyeccionCierre>();
                var esTodos = busq.lstAC.Contains("TODOS");
                var lstConceptoDir = _ctxArrendadora.tblC_FED_CatConcepto.ToList();
                var lstCompleta = _ctxArrendadora.tblC_FED_DetProyeccionCierre.Where(w => w.esActivo && w.anio >= anio && w.semana == semana).ToList();
                if (esTodos)
                {
                    lst = lstCompleta.Where(cierre => cierre.ac == "TODOS").ToList();
                    lst.Where(cierre => cierre.idDetProyGemelo > 0).ToList().ForEach(cierre =>
                    {
                        var gemeloAC = (from det in lstCompleta
                                        where det.idDetProyGemelo == cierre.id
                                        select det.ac).FirstOrDefault();
                        cierre.ac = gemeloAC;
                    });
                }
                else
                {
                    lst = lstCompleta.Where(cierre => busq.lstAC.Contains(cierre.ac)).ToList();
                }
                var lstReserva = getLstDetProyeccionCierreArrendadora(new BusqProyeccionCierreDTO()
                {
                    idConceptoDir = idReserva,
                    grupo = busq.grupo,
                    lstCC = busq.lstCC,
                    lstAC = busq.lstAC,
                    max = busq.max,
                    min = busq.min,
                    tipo = busq.tipo,
                    esCC = busq.esCC
                });
                if (lst.Any(w => w.idConceptoDir == idReserva))
                {
                    lst.FirstOrDefault(w => w.idConceptoDir == idReserva).monto = lstReserva.Sum(s => s.monto);
                }
                else
                {
                    lst.Add(new tblC_FED_DetProyeccionCierre()
                    {
                        idConceptoDir = idReserva,
                        anio = busq.max.Year,
                        semana = busq.max.noSemana(),
                        fechaFactura = busq.max,
                        cc = busq.lstCC.FirstOrDefault(),
                        ac = busq.lstAC.FirstOrDefault(),
                        monto = lstReserva.Sum(s => s.monto),
                    });
                }
                return lst;
            }
        }
        List<tblC_FED_DetProyeccionCierre> getLstDetProyeccionCierreArrendadora(BusqProyeccionCierreDTO busq)
        {
            using (var _ctxArrendadora = new MainContext(EmpresaEnum.Arrendadora))
            {
                var idReserva = 29;
                var semana = busq.max.noSemana();
                var lstGrupoReserva = getLstGpoReserva().Select(s => s.grupo).ToList();
                var lst = (from w in _ctxArrendadora.tblC_FED_DetProyeccionCierre.AsQueryable()
                           where w.esActivo && w.idConceptoDir == busq.idConceptoDir && busq.lstCC.Contains(w.cc) && (w.idConceptoDir == idReserva ? lstGrupoReserva.Contains(w.grupo) : w.anio == busq.max.Year && w.semana == semana)
                           select w).ToList();
                return lst;
            }
        }
        public Task<List<tblC_FED_DetProyeccionCierre>> getLstDetProyeccionCierreArrendadora(BusqFlujoEfectivoDTO busq)
        {
            return Task.Run(() =>
            {
                using (var _ctxArrendadora = new MainContext(EmpresaEnum.Arrendadora))
                {
                    var idReserva = 29;
                    var semana = busq.max.noSemana();
                    var esTodos = busq.lstAC.Contains("TODOS");
                    var lst = new List<tblC_FED_DetProyeccionCierre>();
                    var lstGrupoReserva = from gpo in _ctxArrendadora.tblC_FED_CatGrupoReserva where gpo.esActivo select gpo.grupo;
                    var lstCompleta = (from w in _ctxArrendadora.tblC_FED_DetProyeccionCierre.AsQueryable()
                                       where w.esActivo && (w.idConceptoDir == idReserva ? lstGrupoReserva.Contains(w.grupo) : w.anio == busq.max.Year && w.semana == semana)
                                       select w).ToList();
                    if (esTodos)
                    {
                        lst = lstCompleta.Where(cierre => cierre.ac == "TODOS").ToList();
                        lst.Where(cierre => cierre.idDetProyGemelo > 0).ToList().ForEach(cierre =>
                        {
                            var gemeloAC = (from det in lstCompleta
                                            where det.idDetProyGemelo == cierre.id
                                            select det.ac).FirstOrDefault();
                            cierre.ac = gemeloAC;
                        });
                    }
                    else
                    {
                        lst = lstCompleta.Where(cierre => busq.lstAC.Contains(cierre.ac)).ToList();
                    }
                    AjustarMontosCierre(lst);
                    return lst;
                }
            });
        }
        public List<tblC_FED_CapPlaneacion> getFlujoDirectoArrendadora(BusqFlujoEfectivoDTO busq)
        {
            using (var _ctxArrendadora = new MainContext(EmpresaEnum.Arrendadora))
            {
                var esTodos = busq.lstCC.Contains("TODOS");
                var lstAc = busq.lstCC.Contains("TODOS") ? new string[] { "TODOS" } : busq.lstAC.Select(ac => ac).ToArray();
                var lstCpto = from cpto in _ctxArrendadora.tblC_FED_CatConcepto.AsQueryable() where cpto.esActivo select cpto;
                var lstCptoPadre = from padre in lstCpto where padre.idPadre == 0 select padre.id;
                #region Flujo
                var lstCptoFlujo = from cpto in lstCpto where lstCptoPadre.Contains(cpto.idPadre) select cpto.id;
                var lstFlujo = _context.Select<tblC_FED_CapPlaneacion>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Arrendadora,
                    consulta = busq.esConciliado ?
                        //@"SELECT *, 2 AS empresa
                        //                                 FROM tblC_FED_CapPlaneacion
                        //                                 WHERE esActivo = 1 AND idConceptoDir IN @idConceptoDir AND ac IN @ac"
                    @"SELECT 
	                    A.id, A.idConceptoDir, A.cc, A.ac, A.fecha, A.anio, A.semana,
	                    A.corte, A.esActivo, A.fechaRegistro, A.strFlujoEfectivo, A.strSaldoInicial, 2 AS empresa,
	                    A.planeado - ISNULL((select sum(monto) from tblC_FED_PlaneacionDet where concepto = A.idConceptoDir AND cc = A.cc AND ac = A.ac AND semana = A.semana AND año = A.anio AND estatus = 1 AND (numcte in (504, 9076) OR numprov in (5412) OR concepto like '%CONSTRUCCIONES PLANIFICADAS%')), 0) as planeado
                    FROM tblC_FED_CapPlaneacion A
                    WHERE A.esActivo = 1 AND A.idConceptoDir > 0 AND ac IN @ac"
                                :
                                @"SELECT *, 2 AS empresa
                                                FROM tblC_FED_CapPlaneacion
                                                WHERE esActivo = 1 AND idConceptoDir IN @idConceptoDir AND ac IN @ac"
                    ,
                    parametros = new { idConceptoDir = lstCptoFlujo, ac = lstAc }
                });
                #endregion
                #region Planeacion
                var sigFecha = busq.max.AddDays(7);
                var actAnio = busq.max.Year;
                var actSemana = busq.max.noSemana();
                var sigAnio = sigFecha.Year;
                var sigSemana = sigFecha.noSemana();
                var lstPlan = _context.Select<tblC_FED_CapPlaneacion>(new DapperDTO()
                {
                    baseDatos = MainContextEnum.Arrendadora,
                    consulta = busq.esConciliado ? @"SELECT año AS anio, semana, concepto AS idConceptoDir, ac, SUM(monto) AS Planeado
                                 FROM tblC_FED_PlaneacionDet
                                 WHERE estatus = 1 AND ac IN @ac AND
                                 ((año = @actAnio AND semana = @actSemana) OR
                                 (año = @sigAnio AND semana = @sigSemana))
                                 AND descripcion not like '%CONSTRUCCIONES PLANIFICADAS%'
                                 GROUP BY año, semana, concepto, ac"
                                :
                                @"SELECT año AS anio, semana, concepto AS idConceptoDir, ac, SUM(monto) AS Planeado
                                FROM tblC_FED_PlaneacionDet
                                WHERE estatus = 1 AND ac IN @ac AND
                                ((año = @actAnio AND semana = @actSemana) OR
                                (año = @sigAnio AND semana = @sigSemana))
                                GROUP BY año, semana, concepto, ac"
                    ,
                    parametros = new { ac = lstAc, actAnio, actSemana, sigAnio, sigSemana }
                });
                var lstConceptos = new List<tblC_FED_CapPlaneacion>();
                lstFlujo.Where(f => (f.anio == actAnio && f.semana == actSemana) || (f.anio == sigAnio && f.semana == sigSemana)).ToList().ForEach(flujo => flujo.planeado = 0);
                lstPlan.ForEach(plan =>
                {
                    var verifica = lstConceptos.FirstOrDefault(w => w.anio == plan.anio && w.semana == plan.semana && w.idConceptoDir == plan.idConceptoDir && w.ac == plan.ac);
                    if (verifica == null)
                    {
                        var planeado = plan.planeado;
                        var flujo = lstFlujo.FirstOrDefault(f => f.anio == plan.anio && f.semana == plan.semana && f.idConceptoDir == plan.idConceptoDir && f.ac == plan.ac);
                        if (flujo == null)
                        {
                            lstFlujo.Add(new tblC_FED_CapPlaneacion
                            {
                                anio = plan.anio,
                                semana = plan.semana,
                                idConceptoDir = plan.idConceptoDir,
                                ac = plan.ac,
                                cc = plan.cc,
                                planeado = planeado
                            });
                        }
                        else
                        {
                            flujo.planeado = planeado;
                        }
                        lstConceptos.Add(new tblC_FED_CapPlaneacion
                        {
                            anio = plan.anio,
                            semana = plan.semana,
                            idConceptoDir = plan.idConceptoDir,
                            ac = plan.ac,
                            cc = plan.cc,
                        });
                    }
                });
                #endregion
                #region WhatIf
                var idReserva = 29;
                var lstCierre = _context.Select<tblC_FED_CapPlaneacion>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Arrendadora,
                    consulta = @"SELECT idConceptoDir, anio, semana, MAX(fecha) AS fecha, ac, SUM(monto) AS corte, 'TODOS' AS cc, 2 AS empresa
                                 FROM tblC_FED_DetProyeccionCierre
                                 WHERE esActivo = 1 AND idConceptoDir <> @idReserva AND ac IN @ac
                                 GROUP BY anio, semana, idConceptoDir, ac",
                    parametros = new { idReserva, ac = lstAc }
                });
                lstFlujo.AddRange(lstCierre);
                #endregion
                #region Reservas
                var lstGrupo = from grupo in _ctxArrendadora.tblC_FED_CatGrupoReserva where grupo.esActivo select grupo.grupo;
                var lstReservas = _context.Select<tblC_FED_CapPlaneacion>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Arrendadora,
                    consulta = @"SELECT idConceptoDir, anio, semana, MAX(fecha) AS fecha, ac, 'TODOS' AS cc, SUM(monto) AS corte, 2 as empresa
                                 FROM tblC_FED_DetProyeccionCierre
                                 WHERE esActivo = 1 AND idConceptoDir = @idReserva AND grupo IN @grupo AND ac IN @ac
                                 GROUP BY anio, semana, idConceptoDir, ac, grupo",
                    parametros = new { idReserva, grupo = lstGrupo, ac = lstAc }
                });
                lstFlujo.AddRange(lstReservas);
                #endregion
                lstFlujo.ForEach(flujo =>
                {
                    flujo.flujoTotal = flujo.strFlujoEfectivo.ParseDecimal();
                    flujo.cc = "TODOS";
                });
                return lstFlujo;
            }
        }
        public List<tblC_FE_MovPol> getLstMovPolFlujoEfectivoDirectoArrendadora(BusqFlujoEfectivoDTO busq)
        {
            using (var _ctxArrendadora = new MainContext(EmpresaEnum.Arrendadora))
            {
                busq.lstTm = busq.lstTm == null ? new List<int>() : busq.lstTm;
                var esTmVacio = !busq.lstTm.Any();
                var lstMovPol = (from movpol in _ctxArrendadora.tblC_FE_MovPol
                                 where movpol.esActivo && movpol.esFlujoEfectivo && movpol.idConceptoDir > 0
                                 && (esTmVacio ? true : busq.lstTm.Contains(movpol.itm))
                                 select movpol).ToList();
                lstMovPol.ForEach(movpol => movpol.empresa = EmpresaEnum.Arrendadora);
                return lstMovPol;
            }
        }
        #endregion
        public bool actualizarFacturasIngresos()
        {
            var odbc = new OdbcConsultaDTO()
            {
                consulta = @"SELECT numcte, factura, (CAST(area AS varchar)+'-'+ CAST(cuenta AS varchar)) AS ac
                            FROM sf_facturas
                            ORDER BY numcte, factura",
            };
            var FacturasAC = _contextEnkontrol.Select<tblC_FED_DetProyeccionCierre>(EnkontrolEnum.ArrenProd, odbc);
            using (var _ctxArrendadora = new MainContext(EmpresaEnum.Arrendadora))
            {
                var Facturas = (from factura in _ctxArrendadora.tblC_FED_DetProyeccionCierre.AsQueryable()
                                where factura.idConceptoDir == 21 && factura.tipo == tipoProyeccionCierreEnum.FacturasClientes && factura.ac == "0-0"
                                orderby factura.numcte, factura.factura
                                select factura).ToList();
                var FacturaCount = Facturas.Count;
                for (int i = 0; i < FacturaCount; i++)
                {
                    var Factura = Facturas[i];
                    var facturaAC = FacturasAC.FirstOrDefault(f => f.numcte == Factura.numcte && f.factura == Factura.factura);
                    if (facturaAC == null)
                    {
                        Factura.ac = "0-0";
                    }
                    else if (facturaAC.ac == "-")
                    {
                        Factura.ac = "0-0";
                    }
                    else
                    {
                        Factura.ac = facturaAC.ac ?? "0-0";
                    }
                    _ctxArrendadora.tblC_FED_DetProyeccionCierre.AddOrUpdate(Factura);
                    if (i % 100 == 0)
                    {
                        _ctxArrendadora.SaveChanges();
                    }
                }
                _ctxArrendadora.SaveChanges();
            }
            return true;
        }

        #region Datos Kubrix

        public List<tblC_FED_DetProyeccionCierre> getIngresosEstimadosKubrix(BusqFlujoEfectivoDTO busq, string concepto, int semana)
        {
            List<tblC_FED_DetProyeccionCierre> data = new List<tblC_FED_DetProyeccionCierre>();

            //Tomar el ultimo corte anterior a la fecha de busqueda
            var fechaCorte = busq.max.AddDays(10);
            var corteKubrix = _context.tblM_KBCorte.Where(x => x.fechaCorte <= fechaCorte && x.tipo == 10).OrderByDescending(x => x.fechaCorte).FirstOrDefault();
            var fechaInicioAnio = new DateTime(busq.max.Year, 1, 1);
            if (corteKubrix != null)
            {
                _context.Database.CommandTimeout = 300;
                var auxdata = _context.tblM_KBCorteDet.Where(x =>
                    x.corteID == corteKubrix.id
                    && x.fechapol >= fechaInicioAnio
                        //&& x.fechapol <= busq.max
                    && (busq.esCC ? (busq.lstCC.Contains("TODOS") ? true : busq.lstCC.Contains(x.cc)) : (busq.lstAC.Contains("TODOS") ? true : busq.lstAC.Contains(x.areaCuenta)))
                        //&& ((concepto == "" || concepto == null) ? true : x.concepto == concepto)
                    && (x.cuenta == "1-1-0" || x.cuenta == "1-2-1" || x.cuenta == "1-2-2" || x.cuenta == "1-2-3" || x.cuenta == "1-3-1" || x.cuenta == "1-3-2")
                    ).ToList();
                if (auxdata.Count() > 0)
                {
                    data = auxdata.GroupBy(x => new { areaCuenta = x.areaCuenta }).Select(x => new tblC_FED_DetProyeccionCierre
                    {
                        ac = x.Key.areaCuenta,
                        cc = "N/A",
                        anio = corteKubrix.fechaCorte.Year,
                        descripcion = "INGRESO ESTIMADO",
                        monto = x.Sum(z => z.monto) * (decimal)1.16,
                        fecha = busq.max,
                        fechaRegistro = corteKubrix.fechaCorte,
                        factura = 0,
                        fechaFactura = corteKubrix.fechaCorte,
                        idConceptoDir = 23,
                        tipo = tipoProyeccionCierreEnum.KubrixIngresos,
                        semana = semana,
                        naturaleza = naturalezaEnum.Ingreso,
                        esActivo = true
                    }).ToList();
                }
            }
            return data;
        }

        public List<tblC_FED_DetProyeccionCierre> getCostosEstimadosKubrix(BusqFlujoEfectivoDTO busq, string concepto, int semana)
        {
            List<tblC_FED_DetProyeccionCierre> data = new List<tblC_FED_DetProyeccionCierre>();

            //Tomar el ultimo corte anterior a la fecha de busqueda
            var fechaCorte = busq.max.AddDays(10);

            var corteKubrix = _context.tblM_KBCorte.Where(x => x.fechaCorte <= fechaCorte && x.tipo == 10).OrderByDescending(x => x.fechaCorte).FirstOrDefault();
            var fechaInicioAnio = new DateTime(busq.max.Year, 1, 1);
            if (corteKubrix != null)
            {
                var auxdata = _context.tblM_KBCorteDet.Where(x =>
                    x.corteID == corteKubrix.id
                    && x.fechapol >= fechaInicioAnio
                        //&& x.fechapol <= busq.max
                    && (busq.esCC ? (busq.lstCC.Contains("TODOS") ? true : busq.lstCC.Contains(x.cc)) : (busq.lstAC.Contains("TODOS") ? true : busq.lstAC.Contains(x.areaCuenta)))
                        //&& ((concepto == "" || concepto == null) ? true : x.concepto == concepto)
                    && (x.cuenta == "1-4-0")
                ).ToList();
                if (auxdata.Count() > 0)
                {
                    data = auxdata.GroupBy(x => new { areaCuenta = x.areaCuenta/*, cc = x.cc*/ }).Select(x => new tblC_FED_DetProyeccionCierre
                    {
                        ac = x.Key.areaCuenta,
                        cc = "N/A",
                        anio = corteKubrix.fechaCorte.Year,
                        descripcion = "COSTO ESTIMADO",
                        monto = x.Sum(z => z.monto) * (decimal)1.16,
                        fecha = busq.max,
                        fechaRegistro = corteKubrix.fechaCorte,
                        factura = 0,
                        fechaFactura = corteKubrix.fechaCorte,
                        idConceptoDir = 26,
                        tipo = tipoProyeccionCierreEnum.KubrixCostos,
                        semana = semana,
                        naturaleza = naturalezaEnum.Egreso,
                        esActivo = true
                    }).ToList();
                }
            }
            return data;
        }

        #endregion

        public List<string> getACCerradas()
        {
            var lstTipoCCCerrada = new List<int>() {
                (int)TipoCCEnum.ObraCerradaGeneral,
                (int)TipoCCEnum.ObraCerradaIndustrial
            };
            var data = _context.tblC_CCDivision.Where(x => lstTipoCCCerrada.Contains(x.division)).Select(x => x.cc).ToList();
            return data;
        }
        public List<tblC_FED_Corte> getSemanasCorte()
        {
            List<tblC_FED_Corte> data = new List<tblC_FED_Corte>();
            data = _context.tblC_FED_Corte.ToList();
            return data;
        }
        public tblC_FED_Corte getCorteActaul()
        {
            tblC_FED_Corte data = new tblC_FED_Corte();
            data = _context.tblC_FED_Corte.FirstOrDefault(x => x.actual);
            return data;
        }
    }
}
