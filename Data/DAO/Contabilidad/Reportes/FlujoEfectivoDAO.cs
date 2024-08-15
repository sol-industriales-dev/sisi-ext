using Core.DAO.Contabilidad.Reportes;
using Core.DTO.Contabilidad.FlujoEfectivo;
using Core.DTO.Contabilidad.Poliza;
using Core.DTO.Principal.Generales;
using Core.DTO.Utils.Data;
using Core.Entity.Administrativo.Contabilidad.FlujoEfectivo;
using Core.Enum.Administracion.FlujoEfectivo;
using Core.Enum.Multiempresa;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.Principal.Bitacoras;
using Core.DTO;
using Core.Enum.Administracion.Propuesta;
using Core.Entity.Principal.Multiempresa;
using System.Web;
using System.Web.SessionState;
using Core.Enum.Administracion.Propuesta.Nomina;
using Core.DTO.Contabilidad.Propuesta.Nomina;
using Core.Enum.Principal;
using Core.Entity.Administrativo.Contabilidad.Propuesta.Acomulado;


namespace Data.DAO.Contabilidad.Reportes
{
    public class FlujoEfectivoDAO : GenericDAO<tblC_FE_MovPol>, IFlujoEfectivoDAO
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
            using(var dbTransaction = _context.Database.BeginTransaction())
                try
                {
                    var bd = _context.tblC_FED_CcVisto.FirstOrDefault(v => v.anio == cc.anio && v.semana == cc.semana && v.cc == cc.cc);
                    if(bd == null)
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
                    if(esGuardado)
                    {
                        dbTransaction.Commit();
                    }
                    else
                    {
                        dbTransaction.Rollback();
                    }
                }
                catch(Exception o_O)
                {
                    esGuardado = false;
                    dbTransaction.Rollback();
                    LogError(vSesiones.sesionSistemaActual, vSesiones.sesionCurrentView, nombreControlador, "guardarCcVisto", o_O, AccionEnum.ACTUALIZAR, cc.id, cc);
                }
            return esGuardado;
        }
        public bool guardarConcepto(tblC_FE_CatConcepto obj, List<tblC_FE_RelConceptoTm> rel)
        {
            var esGuardado = false;
            using(var dbTransaction = _context.Database.BeginTransaction())
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
                    if(esGuardado)
                    {
                        dbTransaction.Commit();
                    }
                    else
                    {
                        dbTransaction.Rollback();
                    }
                }
                catch(Exception o_O)
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
            using(var dbTransaction = _context.Database.BeginTransaction())
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
                    if(esGuardado)
                    {
                        dbTransaction.Commit();
                    }
                    else
                    {
                        dbTransaction.Rollback();
                    }
                }
                catch(Exception o_O)
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
            using(var dbTransaction = _context.Database.BeginTransaction())
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
                        if(_bd == null)
                        {

                        }
                        else
                        {
                            mov.id = _bd.id;
                        }
                        if(mov.itm != mov.itmOri)
                        {
                            lstUpTm.Add(mov);
                        }
                        mov.esActivo = true;
                        mov.fechaRegistro = ahora;
                        _context.tblC_FE_MovPol.AddOrUpdate(mov);
                        _context.SaveChanges();
                    });
                    esGuardado = lst.All(r => r.id > 0);
                    if(esGuardado)
                    {
                        ActualizarITipoMovimiento(lstUpTm);
                        dbTransaction.Commit();
                    }
                    else
                    {
                        dbTransaction.Rollback();
                    }
                }
                catch(Exception o_O)
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
            if(lst.Count == 0)
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
            return _context.tblC_FED_SaldoInicial.Where(w => w.anio == anio).ToList();
        }
        public List<tblC_FE_SaldoInicial> getLstFE_SaldoInicial(int anio)
        {
            return _context.tblC_FE_SaldoInicial.Where(w => w.anio == anio).ToList();
        }
        int asigaIdCptoDir(MovpolDTO mov)
        {
            var idCostoProyecto = 6;
            var idGastosOperativo = 7;
            var lstDivAdmin = new List<int>()
            {
                (int)TipoCCEnum.Administración,
                (int)TipoCCEnum.GastosFininacierosYOtros
            };
            var relCatDir = (List<tblC_FED_RelConceptoTm>)session["relCptoDir"];
            if(relCatDir == null)
            {
                relCatDir = _context.tblC_FED_RelConceptoTm.ToList();
                session["relCptoDir"] = relCatDir;
            }
            var idCpto = relCatDir.Any(r => r.tm == mov.itm) ? relCatDir.FirstOrDefault(r => r.tm == mov.itm).idConceptoDir : 0;
            if(idCpto != idCostoProyecto && idCpto != idGastosOperativo)
            {
                return idCpto;
            }
            var lstCC = (List<Core.DTO.Contabilidad.CcDTO>)session["lstCC"];
            if(lstCC == null)
            {
                lstCC = new CadenaProductivaDAO().lstObra();
                session["lstCC"] = lstCC;
            }
            var cc = lstCC.FirstOrDefault(c => c.cc == mov.cc);
            if(idCpto == idCostoProyecto || idCpto == idGastosOperativo)
            {
                if(cc != null && lstDivAdmin.Contains(cc.bit_area.ParseInt()))
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
            return lst;
        }
        string queryFlujoCCTM(BusqFlujoEfectivoDTO busq)
        {
            return string.Format(@"SELECT mov.cc, mov.itm, sum(mov.monto) AS monto
                    FROM sc_movpol mov
                    INNER JOIN sc_polizas pol ON pol.year = mov.year AND pol.mes = mov.mes AND pol.tp = mov.tp AND pol.poliza = mov.poliza
                    WHERE mov.cc IN {0}
                        AND mov.cta BETWEEN ? AND ? 
                        AND pol.fechapol BETWEEN ? AND ?
                        AND mov.itm IN {1}
                    GROUP BY mov.cc, mov.itm"
                , busq.lstCC.ToParamInValue()
                , busq.lstTm.ToParamInValue());
        }
        string queryFlujoCCTMMultiple(BusqFlujoEfectivoDTO busq)
        {
            return string.Format(@"SELECT mov.cc, mov.itm, sum(mov.monto) AS monto
                    FROM sc_movpol dd
                    INNER JOIN sc_polizas pol ON pol.year = dd.year AND pol.mes = dd.mes AND pol.tp = dd.tp AND pol.poliza = dd.poliza
                    INNER JOIN sc_movpol mov ON mov.year = dd.year AND mov.mes = dd.mes AND mov.tp = dd.tp AND mov.poliza = dd.poliza AND mov.cc <> '*' AND mov.cc IN {0}
                    WHERE dd.cta BETWEEN ? AND ? 
                        AND dd.cc = '*' 
                        AND pol.fechapol BETWEEN ? AND ?
                        AND mov.itm IN {1}
                    GROUP BY mov.cc, mov.itm"
                , busq.lstCC.ToParamInValue()
                , busq.lstTm.ToParamInValue());
        }
        public bool guardarCorte(BusqFlujoEfectivoDTO busq)
        {
            var esGuardado = false;
            using(var dbTransaction = _context.Database.BeginTransaction())
                try
                {
                    var relCptoOpe = _context.tblC_FE_RelConceptoTm.Select(s => s.tm);
                    var relCptoDir = _context.tblC_FED_RelConceptoTm.Select(s => s.tm).ToList();
                    relCptoDir.AddRange(relCptoOpe);
                    var ahora = DateTime.Now;
                    var anio = busq.max.Year;
                    var busqFlujo = new BusqFlujoEfectivoDTO()
                    {
                        idConcepto = busq.idConcepto,
                        lstCC = new CadenaProductivaDAO().lstObra().Select(s => s.cc).ToList(),
                        lstTm = relCptoDir.GroupBy(g => g).Select(s => s.Key).ToList(),
                        max = busq.max,
                        min = new DateTime(busq.max.Year, 1, 1),
                        tipo = busq.tipo
                    };
                    busq.lstCC = busqFlujo.lstCC;
                    busq.lstTm = busqFlujo.lstTm;
                    var lstMovFlujo = getLstMovPolCCTM(busqFlujo).AsQueryable();
                    var gpoMovFlujo = from g in lstMovFlujo
                                      group g by new
                                      {
                                          cc = g.cc,
                                          idCpto = asigaIdCptoDir(g)
                                      } into s
                                      select new
                                      {
                                          cc = s.Key.cc,
                                          idCpto = s.Key.idCpto,
                                          monto = s.Sum(ss => ss.monto),
                                      };
                    var lstMov = getLstMovPolActiva(busq);
                    var lstEnk = getLstMovPol(busq).AsQueryable();
                    busqFlujo.min = busqFlujo.min.AddDays(-1);
                    var lstSaldiInicial = getLstSaldoInicialCCTM(busqFlujo).AsQueryable();
                    var gpoSaldoInicial = from g in lstSaldiInicial
                                          group g by new
                                          {
                                              cc = g.cc,
                                              idCpto = asigaIdCptoDir(g)
                                          } into s
                                          select new
                                          {
                                              cc = s.Key.cc,
                                              idCpto = s.Key.idCpto,
                                              monto = s.Sum(ss => ss.monto),
                                          };
                    var lstSaldoInicialTodos = (from ini in _context.tblC_FED_SaldoInicial.AsQueryable()
                                                where ini.anio == anio - 1 && ini.cc == "TODOS"
                                                select new
                                                {
                                                    cc = ini.cc,
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
                    var lstMovCC = busqFlujo.lstCC;
                    lstMovCC.Insert(0, "TODOS");
                    lstMovCC = lstMovCC.GroupBy(g => g).Select(s => s.Key).ToList();
                    #region Verificacion de polizas
                    for(int i = 0; i < lstMov.Count(); i++)
                    {
                        var mov = lstMov[i];
                        mov.esActivo = true;
                        var enk = (from e in lstEnk
                                   where e.year == mov.year && e.mes == mov.mes && e.tp == mov.tp && e.poliza == mov.poliza && e.linea == mov.linea && e.itm == mov.itm && e.cc == mov.cc && e.monto == mov.monto
                                   select e).FirstOrDefault();
                        if(enk == null)
                        {
                            mov.esFlujoEfectivo = false;
                            mov.esActivo = false;
                        }
                        else
                        {
                            mov.esFlujoEfectivo = true;
                        }
                        if(i % 100 == 0)
                        {
                            _context.SaveChanges();
                        }
                    }
                    _context.SaveChanges();
                    #endregion
                    #region flujoEfectivo
                    for(int c = 0; c < lstMovCC.Count; c++)
                    {
                        var cc = lstMovCC[c];
                        var esTodos = cc == "TODOS";
                        var movCC = from w in lstMov where w.esFlujoEfectivo && (esTodos ? true : cc == w.cc) select w;
                        var flujoCC = from w in gpoMovFlujo where esTodos ? true : cc == w.cc select w;
                        var inicialCC = from w in gpoSaldoInicial where esTodos ? false : cc == w.cc select w;
                        if(esTodos)
                        {
                            inicialCC = from w in lstSaldoInicialTodos where cc == w.cc select w;
                        }
                        #region saldos
                        for(int h = 0; h < lstCptoHijo.Count; h++)
                        {
                            var cpto = lstCptoHijo[h];
                            var movCptoCC = (from w in movCC where w.idConceptoDir == cpto.id select w).Sum(s => s.monto);
                            var flujoCptoCC = (from w in flujoCC where w.idCpto == cpto.id select w).Sum(s => s.monto);
                            var inicialCptoCC = (from w in inicialCC where w.idCpto == cpto.id select w).Sum(s => (decimal?)s.monto).GetValueOrDefault();
                            var planCptoCC = lstPLan.FirstOrDefault(w => w.idConceptoDir == cpto.id && cc == w.cc);
                            if(planCptoCC != null && movCptoCC == planCptoCC.corte && flujoCptoCC == planCptoCC.strFlujoEfectivo.ParseDecimal() && inicialCptoCC == planCptoCC.strSaldoInicial.ParseDecimal())
                            {
                                continue;
                            }
                            if(planCptoCC == null)
                            {
                                planCptoCC = new tblC_FED_CapPlaneacion()
                                {
                                    idConceptoDir = cpto.id,
                                    anio = anio,
                                    semana = semana,
                                    cc = cc,
                                    fecha = busq.max,
                                    fechaRegistro = ahora,
                                };
                            }
                            planCptoCC.esActivo = true;
                            planCptoCC.corte = movCptoCC;
                            planCptoCC.flujoTotal = flujoCptoCC;
                            planCptoCC.strFlujoEfectivo = planCptoCC.flujoTotal.ToString();
                            planCptoCC.strSaldoInicial = inicialCptoCC.ToString();
                            _context.tblC_FED_CapPlaneacion.AddOrUpdate(planCptoCC);
                            if(acumPlan.Count % 100 == 0)
                            {
                                _context.SaveChanges();
                            }
                            acumPlan.Add(planCptoCC);
                        }
                        #endregion
                    }
                    _context.SaveChanges();
                    #endregion
                    dbTransaction.Commit();
                    esGuardado = true;
                }
                catch(Exception o_O)
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
            using(var dbTransaction = _context.Database.BeginTransaction())
                try
                {
                    var ahora = DateTime.Now;
                    var fecha = lst.FirstOrDefault().fecha;
                    var lstBd = getAllPlaneacion();
                    lst.ForEach(plan =>
                    {
                        var bd = lstBd.FirstOrDefault(p => p.idConceptoDir == plan.idConceptoDir && p.anio == plan.anio && p.semana == plan.semana && p.cc == plan.cc);
                        if(bd == null)
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
                    if(esGuardado)
                    {
                        dbTransaction.Commit();
                    }
                }
                catch(Exception o_O)
                {
                    esGuardado = false;
                    dbTransaction.Rollback();
                    LogError(vSesiones.sesionSistemaActual, vSesiones.sesionCurrentView, nombreControlador, "guardarPlaneacion", o_O, AccionEnum.ACTUALIZAR, 0, lst.FirstOrDefault());
                }
            return esGuardado;
        }
        public List<tblC_FED_DetProyeccionCierre> guardarDetProyeccionCierre(List<tblC_FED_DetProyeccionCierre> lst)
        {
            var esGuardado = false;
            using(var dbTransaction = _context.Database.BeginTransaction())
                try
                {
                    var ahora = DateTime.Now;
                    var busq = new BusqProyeccionCierreDTO()
                    {
                        max = lst.Max(m => m.fecha),
                        lstCC = lst.GroupBy(g => g.cc).Select(s => s.Key).ToList(),
                        idConceptoDir = lst.FirstOrDefault().idConceptoDir,
                        tipo = lst.FirstOrDefault().tipo
                    };
                    busq.min = busq.max.AddDays(-6);
                    if(busq.tipo != tipoProyeccionCierreEnum.Manual)
                    {
                        busq.lstCC.Insert(0, "TODOS");
                    }
                    var lstBd = getLstDetProyeccionCierre(busq);
                    lst.ForEach(plan =>
                    {
                        var bd = lstBd.FirstOrDefault(p => p.id == plan.id);
                        if(bd == null)
                        {

                        }
                        else
                        {
                            plan.id = bd.id;
                        }
                        plan.fechaRegistro = ahora;
                        _context.tblC_FED_DetProyeccionCierre.AddOrUpdate(plan);
                        _context.SaveChanges();
                        if(plan.tipo == tipoProyeccionCierreEnum.Manual && lst.Count == 2)
                        {
                            var primero = lst.FirstOrDefault();
                            primero.idDetProyGemelo = primero.id + 1;
                            lst.LastOrDefault().idDetProyGemelo = primero.id;
                        }
                    });
                    if(busq.tipo != tipoProyeccionCierreEnum.Manual)
                    {
                        var lstTodos = lst.Select(s => new tblC_FED_DetProyeccionCierre()
                        {
                            id = 0,
                            idDetProyGemelo = s.id,
                            idConceptoDir = s.idConceptoDir,
                            anio = s.anio,
                            semana = s.semana,
                            cc = "TODOS",
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
                            var bd = lstBd.FirstOrDefault(p => p.monto == plan.monto && p.descripcion == plan.descripcion && p.fechaFactura == plan.fechaFactura && p.numpro == plan.numpro && p.numcte == plan.numcte && p.factura == plan.factura && p.cc == "TODOS");
                            if(bd == null)
                            {

                            }
                            else
                            {
                                plan.id = bd.id;
                            }
                            plan.fechaRegistro = ahora;
                            _context.tblC_FED_DetProyeccionCierre.AddOrUpdate(plan);
                            _context.SaveChanges();
                            if(plan.idDetProyGemelo > 0)
                            {
                                var gemelo = lst.FirstOrDefault(w => w.id == plan.idDetProyGemelo);
                                gemelo.idDetProyGemelo = plan.id;
                                _context.tblC_FED_DetProyeccionCierre.AddOrUpdate(plan);
                                _context.SaveChanges();
                            }
                        });
                    }
                    esGuardado = lst.All(a => a.id > 0);
                    if(esGuardado)
                    {
                        esGuardado = guardarCalculoProyeccionCierre(busq);
                        if(esGuardado)
                        {
                            dbTransaction.Commit();
                        }
                    }
                }
                catch(Exception o_O)
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
            if(detalle == null)
            {
                return true;
            }
            using(var dbTransaction = _context.Database.BeginTransaction())
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
                        lstCC = new List<string>()
                    };
                    busq.lstCC.Add(detalle.cc);
                    if(detalle.idDetProyGemelo > 0)
                    {
                        var gem = _context.tblC_FED_DetProyeccionCierre.FirstOrDefault(w => w.id == detalle.idDetProyGemelo);
                        gem.esActivo = false;
                        _context.tblC_FED_DetProyeccionCierre.AddOrUpdate(gem);
                        _context.SaveChanges();
                        busq.lstCC.Add(gem.cc);
                    }
                    esGuardado = guardarCalculoProyeccionCierre(busq);
                    if(esGuardado)
                    {
                        dbTransaction.Commit();
                    }
                }
                catch(Exception o_O)
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
            var lstBd = getLstDetProyeccionCierre(busq);
            var lstPlan = getPlaneacionCierre(busq).Where(w => w.idConceptoDir == busq.idConceptoDir).ToList();
            busq.lstCC.ForEach(cc =>
            {
                var plan = lstPlan.FirstOrDefault(p => p.cc == cc);
                if(plan == null)
                {
                    var semana = busq.max.noSemana();
                    plan = new tblC_FED_CapPlaneacion()
                    {
                        idConceptoDir = busq.idConceptoDir,
                        cc = cc,
                        anio = busq.max.Year,
                        fecha = busq.max,
                        semana = semana,
                        esActivo = true,
                    };
                }
                plan.fechaRegistro = DateTime.Now;
                plan.corte = lstBd.Where(w => w.cc == cc).Sum(s => s.monto);
                _context.tblC_FED_CapPlaneacion.AddOrUpdate(p => p.id, plan);
                _context.SaveChanges();
                esGuardado = plan.id > 0;
            });
            return esGuardado;
        }
        public bool GuardarGpoReserva(tblC_FED_CatGrupoReserva gpo)
        {
            var esGuardado = false;
            using(var dbTransaction = _context.Database.BeginTransaction())
                try
                {
                    gpo.fechaRegistro = DateTime.Now;
                    _context.tblC_FED_CatGrupoReserva.AddOrUpdate(gpo);
                    _context.SaveChanges();
                    esGuardado = gpo.id > 0;
                    if(esGuardado)
                    {
                        dbTransaction.Commit();
                    }
                }
                catch(Exception o_O)
                {
                    esGuardado = false;
                    dbTransaction.Rollback();
                    LogError(vSesiones.sesionSistemaActual, vSesiones.sesionCurrentView, nombreControlador, "GuardarGpoReserva", o_O, AccionEnum.AGREGAR, gpo.id, gpo);
                }
            return esGuardado;
        }
        bool actualizarFlujoTotal2020()
        {
            var esGuardado = false;
            using(var dbTransaction = _context.Database.BeginTransaction())
                try
                {
                    var idCostoProyecto = 6;
                    var idGastosOperativo = 7;
                    var ahora = DateTime.Now;
                    var lstMovpol = new List<MovpolDTO>();
                    var acumIni = new List<tblC_FED_SaldoInicial>();
                    var lstCC = new CadenaProductivaDAO().lstObra();
                    var lstIni = _context.tblC_FED_SaldoInicial.ToList();
                    var relCpto = _context.tblC_FED_RelConceptoTm.ToList();
                    var lstTm = relCpto.GroupBy(g => g.tm).Select(s => s.Key.ToString()).ToList();
                    var lstOdbc = new List<OdbcConsultaDTO>()
                    {
                        new OdbcConsultaDTO() { consulta = queryFlujoTotalCC(lstTm) }
                       ,new OdbcConsultaDTO() { consulta = queryFlujoTotalMultiple(lstTm) }
                    };
                    lstOdbc.ForEach(odbc => lstMovpol.AddRange(_contextEnkontrol.Select<MovpolDTO>(EnkontrolAmbienteEnum.Prod, odbc)));
                    #region acumulado historico
                    var gpoCCItm = (from g in lstMovpol
                                    group g by new
                                    {
                                        cc = g.cc,
                                        idCpto = asigaIdCptoDir(g)
                                    } into s
                                    select new
                                    {
                                        cc = s.Key.cc,
                                        idCpto = s.Key.idCpto,
                                        monto = s.Sum(ss => ss.monto),
                                    }).ToList();
                    var lstAdmin = new List<int>
                    {
                        (int)TipoCCEnum.Administración,
                        (int)TipoCCEnum.GastosFininacierosYOtros
                    };
                    gpoCCItm.ForEach(ccEnk =>
                    {
                        var idCpto = ccEnk.idCpto;
                        var plan = lstIni.FirstOrDefault(p => p.idConceptoDir == idCpto && p.cc == ccEnk.cc);
                        if(plan == null)
                        {
                            plan = new tblC_FED_SaldoInicial()
                            {
                                cc = ccEnk.cc,
                                anio = 2019,
                                idConceptoDir = idCpto,
                                esActivo = true,
                                fechaRegistro = ahora
                            };
                        }
                        plan.saldo = ccEnk.monto;
                        acumIni.Add(plan);
                    });
                    acumIni.ForEach(plan =>
                    {
                        _context.tblC_FED_SaldoInicial.AddOrUpdate(plan);
                        _context.SaveChanges();
                    });
                    esGuardado = true;
                    dbTransaction.Commit();
                    #endregion
                }
                catch(Exception e)
                {
                    dbTransaction.Rollback();
                }
            return esGuardado;
        }
        string queryFlujoTotalCC(List<string> lstTm)
        {
            return string.Format(@"SELECT mov.cc, mov.itm, sum(mov.monto) AS monto
                    FROM sc_movpol mov
                    INNER JOIN sc_polizas pol ON pol.year = mov.year AND pol.mes = mov.mes AND pol.tp = mov.tp AND pol.poliza = mov.poliza
                    WHERE mov.cta BETWEEN 1105 AND 1115 AND pol.fechapol <= '2019-12-31' AND CC NOT IN ('*') AND mov.itm IN ({0})
                    GROUP BY mov.cc, mov.itm"
                , lstTm.ToLine(","));
        }
        string queryFlujoTotalMultiple(List<string> lstTm)
        {
            return string.Format(@"SELECT mov.cc, mov.itm, sum(mov.monto) AS monto
                    FROM sc_movpol dd
                    INNER JOIN sc_polizas pol ON pol.year = dd.year AND pol.mes = dd.mes AND pol.tp = dd.tp AND pol.poliza = dd.poliza
                    INNER JOIN sc_movpol mov ON mov.year = dd.year AND mov.mes = dd.mes AND mov.tp = dd.tp AND mov.poliza = dd.poliza AND mov.cc <> '*'
                    WHERE dd.cta BETWEEN 1105 AND 1115 AND dd.cc = '*' AND pol.fechapol <= '2019-12-31' AND mov.itm IN ({0})
                    GROUP BY mov.cc, mov.itm"
                , lstTm.ToLine(","));
        }
        public List<MovpolDTO> getLstSaldoInicialCCTM(BusqFlujoEfectivoDTO busq)
        {
            var lst = new List<MovpolDTO>();
            var lstOdbc = new List<OdbcConsultaDTO>()
            {
                new OdbcConsultaDTO()
                {
                    consulta = querySaldoAnio(busq),
                    parametros = paramSaldoAnio(busq)
                }
                ,new OdbcConsultaDTO()
                {
                    consulta = querySaldoAniolMultiple(busq),
                    parametros = paramSaldoAnio(busq)
                }
            };
            lstOdbc.ForEach(odbc =>
                lst.AddRange(_contextEnkontrol.Select<MovpolDTO>(EnkontrolAmbienteEnum.Prod, odbc))
            );
            return lst;
        }
        string querySaldoAnio(BusqFlujoEfectivoDTO busq)
        {
            return string.Format(@"SELECT mov.cc, mov.itm, sum(mov.monto) AS monto
                    FROM sc_movpol mov
                    WHERE mov.cta BETWEEN 1105 AND 1115 AND mov.year <= ? AND CC NOT IN ('*') AND mov.itm IN ({0})
                    GROUP BY mov.cc, mov.itm"
                , busq.lstTm.Select(s => s.ToString()).ToList().ToLine(","));
        }
        string querySaldoAniolMultiple(BusqFlujoEfectivoDTO busq)
        {
            return string.Format(@"SELECT mov.cc, mov.itm, sum(mov.monto) AS monto
                    FROM sc_movpol dd
                    INNER JOIN sc_movpol mov ON mov.year = dd.year AND mov.mes = dd.mes AND mov.tp = dd.tp AND mov.poliza = dd.poliza AND mov.cc <> '*'
                    WHERE dd.cta BETWEEN 1105 AND 1115 AND dd.cc = '*' AND mov.year <= ? AND mov.itm IN ({0})
                    GROUP BY mov.cc, mov.itm"
                , busq.lstTm.Select(s => s.ToString()).ToList().ToLine(","));
        }
        List<OdbcParameterDTO> paramSaldoAnio(BusqFlujoEfectivoDTO busq)
        {
            var lst = new List<OdbcParameterDTO>();
            lst.Add(new OdbcParameterDTO() { nombre = "year", tipo = OdbcType.Int, valor = busq.min.Year });
            lst.AddRange(from tm in busq.lstTm select new OdbcParameterDTO { nombre = "itm", tipo = OdbcType.Int, valor = tm });
            return lst;
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
            catch(Exception)
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
            catch(Exception o_O)
            {
                return new List<tblC_FED_RelConceptoTm>();
            }
        }
        public List<tblC_FED_RelConceptoTm> getRelConceptoDirTmArr()
        {
            try
            {
                List<tblC_FED_RelConceptoTm> data = new List<tblC_FED_RelConceptoTm>();
                using (var _ctxArrendadora = new MainContext(EmpresaEnum.Arrendadora)) {
                    data = _ctxArrendadora.tblC_FED_RelConceptoTm.ToList();
                }
                return data;
            }
            catch (Exception o_O)
            {
                return new List<tblC_FED_RelConceptoTm>();
            }
        }
        #endregion
        #region ¿Que pasa si?
        public List<tblC_FED_CapPlaneacion> getPlaneacionCierre(BusqProyeccionCierreDTO busq)
        {
            var semana = busq.max.noSemana();
            var idReserva = 29;
            var lstConceptoDir = _context.tblC_FED_CatConcepto;
            var lstPadreCierre = lstConceptoDir.Where(w => w.idPadre == -3).Select(s => s.id);
            var lstCptoCierre = lstConceptoDir.Where(w => lstPadreCierre.Contains(w.idPadre)).Select(s => s.id);
            var lst = _context.tblC_FED_CapPlaneacion.Where(w => w.esActivo && w.anio == busq.max.Year && w.semana == semana && busq.lstCC.Contains(w.cc) && lstCptoCierre.Contains(w.idConceptoDir)).ToList();
            asignarFlujoPlaneacion(lst);
            var lstReserva = getLstDetProyeccionCierre(new BusqProyeccionCierreDTO()
            {
                idConceptoDir = idReserva,
                grupo = busq.grupo,
                lstCC = busq.lstCC,
                max = busq.max,
                min = busq.min,
                tipo = busq.tipo
            });
            if(lst.Any(w => w.idConceptoDir == idReserva))
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
            var anio = busq.max.Year;
            var idReserva = 29;
            var lstConceptoDir = _context.tblC_FED_CatConcepto.ToList();
            var lstPadreCierre = lstConceptoDir.Where(w => w.idPadre == -3).Select(s => s.id).ToList();
            var lstCptoCierre = lstConceptoDir.Where(w => lstPadreCierre.Contains(w.idPadre)).Select(s => s.id).ToList();
            var lst = _context.tblC_FED_DetProyeccionCierre.Where(w => w.esActivo && w.anio == anio && w.semana == semana && busq.lstCC.Contains("TODOS") ? w.cc == "TODOS" : busq.lstCC.Contains(w.cc) && lstCptoCierre.Contains(w.idConceptoDir)).ToList();
            var lstReserva = getLstDetProyeccionCierre(new BusqProyeccionCierreDTO()
            {
                idConceptoDir = idReserva,
                lstCC = busq.lstCC,
                lstAC = busq.lstAC,
                max = busq.max,
                min = busq.min,
                esCC = busq.esCC
            });
            if(lst.Any(w => w.idConceptoDir == idReserva))
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
            return _context.tblC_FED_DetProyeccionCierre
                .Where(w => w.esActivo && lstId.Contains(w.id)).ToList();
        }
        public List<tblC_FED_DetProyeccionCierre> getLstDetProyeccionCierre(BusqProyeccionCierreDTO busq)
        {
            var idReserva = 29;
            var semana = busq.max.noSemana();
            var lstGrupoReserva = getLstGpoReserva().Select(s => s.grupo);
            return _context.tblC_FED_DetProyeccionCierre
                .Where(w => w.esActivo && busq.lstCC.Contains(w.cc) && w.idConceptoDir == busq.idConceptoDir)
                .Where(w => w.idConceptoDir == idReserva ? lstGrupoReserva.Contains(w.grupo) : w.anio == busq.max.Year && w.semana == semana).ToList();
        }
        public List<tblC_FED_DetProyeccionCierre> getLstDetProyeccionCierre(BusqFlujoEfectivoDTO busq)
        {
            var idReserva = 29;
            var semana = busq.max.noSemana();
            var anio = busq.max.Year;
            var lstGrupoReserva = getLstGpoReserva().Select(s => s.grupo).ToList();
            List<int> proveedoresArr = Enum.GetValues(typeof(numproArrendadoraEnum)).Cast<int>().ToList();
            List<int> clientesArr = Enum.GetValues(typeof(numcteArrendadoraEnum)).Cast<int>().ToList();
            var lst = (from det in _context.tblC_FED_DetProyeccionCierre
                       where det.esActivo && det.idConceptoDir == busq.idConcepto && det.idConceptoDir == idReserva && lstGrupoReserva.Contains(det.grupo) &&
                       (busq.esConciliado ? !proveedoresArr.Contains(det.numpro) && !clientesArr.Contains(det.numcte) : true)
                       select det).Distinct().ToList();
            return lst;
        }
        public Task<List<tblC_FED_DetProyeccionCierre>> initLstDetProyeccionCierre(BusqFlujoEfectivoDTO busq)
        {
            return Task.Run(() =>
                {
                    init();
                    var lst = new List<tblC_FED_DetProyeccionCierre>();
                    var idReserva = 29;
                    var anio = busq.max.Year;
                    var semana = busq.max.noSemana();
                    var esTodos = busq.lstCC.Contains("TODOS");
                    #region Construplan
                    //var lstCompleta = (from det in _context.tblC_FED_DetProyeccionCierre.AsQueryable()
                    //                   where det.esActivo && det.idConceptoDir != idReserva && det.anio == anio && det.semana == semana
                    //                   select det).ToList();
                    var lstCompleta = _context.Select<tblC_FED_DetProyeccionCierre>(new DapperDTO 
                    {
                        baseDatos = MainContextEnum.Construplan,
                        consulta = busq.esConciliado ?
                                    @"SELECT *, 1 AS empresa
                                     FROM tblC_FED_DetProyeccionCierre
                                     WHERE esActivo = 1 AND idConceptoDir <> @idReserva AND anio = @anio AND semana = @semana
                                     AND numpro not in (9867) AND numcte not in (487, 9084)
                                     AND descripcion not like '%ARRENDADORA%'":
                                    @"SELECT *, 1 AS empresa
                                     FROM tblC_FED_DetProyeccionCierre
                                     WHERE esActivo = 1 AND idConceptoDir <> @idReserva AND anio = @anio AND semana = @semana"
                                    ,
                        parametros = new
                        {
                            idReserva,
                            anio,
                            semana
                        }
                    });
                    var lstCompletaReserva = _context.Select<tblC_FED_DetProyeccionCierre>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Construplan,
                        consulta = busq.esConciliado ?
                                    @"SELECT *, 1 AS empresa
                                     FROM tblC_FED_DetProyeccionCierre
                                     WHERE esActivo = 1 AND idConceptoDir = @idReserva
                                     AND numpro not in (9867) AND numcte not in (487, 9084) AND descripcion not like '%ARRENDADORA%'" :
                                    @"SELECT *, 1 AS empresa
                                     FROM tblC_FED_DetProyeccionCierre
                                     WHERE esActivo = 1 AND idConceptoDir = @idReserva",
                        parametros = new
                        {
                            idReserva
                        }
                    });
                    lstCompleta.AddRange(lstCompletaReserva);
                    if(esTodos)
                    {
                        lst = (from cierre in lstCompleta where cierre.cc == "TODOS" select cierre).ToList();
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
                        lst = (from cierre in lstCompleta where busq.lstCC.Contains(cierre.cc) select cierre).ToList();
                    }
                    lst.ForEach(cierre => cierre.empresa = EmpresaEnum.Construplan);
                    #endregion
                    #region Arrendadora
                    if(busq.esConciliado)
                    {
                        using(var _ctxArrendadora = new MainContext(EmpresaEnum.Arrendadora))
                        {
                            //var lstArrendadora = (from det in _ctxArrendadora.tblC_FED_DetProyeccionCierre.AsQueryable()
                            //                      where det.esActivo && det.idConceptoDir != idReserva && det.anio == anio && det.semana == semana
                            //                      select det).ToList();
                            var lstArrendadora = _context.Select<tblC_FED_DetProyeccionCierre>(new DapperDTO
                            {
                                baseDatos = MainContextEnum.Arrendadora,
                                consulta = @"SELECT *, 2 AS empresa
                                            FROM tblC_FED_DetProyeccionCierre
                                            WHERE esActivo = 1 AND idConceptoDir <> @idReserva AND anio = @anio AND semana = @semana
                                            AND numpro not in (5412) AND numcte not in (504, 9076) AND descripcion not like '%CONSTRUCCIONES PLANIFICADAS%",
                                parametros = new
                                {
                                    idReserva,
                                    anio,
                                    semana
                                }
                            });
                            var lstArrendadoraReserva = _context.Select<tblC_FED_DetProyeccionCierre>(new DapperDTO
                            {
                                baseDatos = MainContextEnum.Arrendadora,
                                consulta = @"SELECT *, 1 AS empresa
                                    FROM tblC_FED_DetProyeccionCierre
                                    WHERE esActivo = 1 AND idConceptoDir = @idReserva
                                    AND numpro not in (5412) AND numcte not in (504, 9076) AND descripcion not like '%CONSTRUCCIONES PLANIFICADAS%'",
                                parametros = new
                                {
                                    idReserva
                                }
                            });
                            lstArrendadora.AddRange(lstArrendadoraReserva);
                            if(esTodos)
                            {
                                var lstArrendTodo = (from det in lstArrendadora where det.ac == "TODOS" select det).ToList();
                                lstArrendTodo.Where(cierre => cierre.idDetProyGemelo > 0).ToList().ForEach(cierre =>
                                {
                                    var gemeloAC = (from det in lstArrendadora
                                                    where det.idDetProyGemelo == cierre.id
                                                    select det.ac).FirstOrDefault();
                                    cierre.ac = gemeloAC;
                                });
                                lstArrendTodo.ForEach(cierre => cierre.empresa = EmpresaEnum.Arrendadora);
                                lst.AddRange(lstArrendTodo);
                            }
                            else
                            {
                                var lstArrenAC = (from det in lstArrendadora where busq.lstAC.Contains(det.ac) select det).ToList();
                                lst.AddRange(lstArrenAC);
                            }
                        }
                    }
                    #endregion
                    AjustarMontosCierre(lst);
                    lst.AddRange(getLstDetProyeccionCierre(busq));
                    return lst;
                });
        }
        public List<tblC_FED_DetProyeccionCierre> initLstDetProyeccionCierre_Optimizado(BusqFlujoEfectivoDTO busq)
        {
            init();
            var lst = new List<tblC_FED_DetProyeccionCierre>();
            var idReserva = 29;
            var anio = busq.max.Year;
            var semana = busq.max.noSemana();
            var esTodos = busq.lstCC.Contains("TODOS");
            #region Construplan
            //var lstCompleta = (from det in _context.tblC_FED_DetProyeccionCierre.AsQueryable()
            //                   where det.esActivo && det.idConceptoDir != idReserva && det.anio == anio && det.semana == semana
            //                   select det).ToList();
            var lstCompleta = _context.Select<tblC_FED_DetProyeccionCierre>(new DapperDTO
            {
                baseDatos = MainContextEnum.Construplan,
                consulta = busq.esConciliado ?
                            @"SELECT *, 1 AS empresa
                                     FROM tblC_FED_DetProyeccionCierre
                                     WHERE esActivo = 1 AND idConceptoDir <> @idReserva AND anio = @anio AND semana = @semana
                                     AND numpro not in (9867) AND numcte not in (487, 9084)
                                     AND descripcion not like '%ARRENDADORA%'" :
                            @"SELECT *, 1 AS empresa
                                     FROM tblC_FED_DetProyeccionCierre
                                     WHERE esActivo = 1 AND idConceptoDir <> @idReserva AND anio = @anio AND semana = @semana"
                            ,
                parametros = new
                {
                    idReserva,
                    anio,
                    semana
                }
            });
            var lstCompletaReserva = _context.Select<tblC_FED_DetProyeccionCierre>(new DapperDTO
            {
                baseDatos = MainContextEnum.Construplan,
                consulta = busq.esConciliado ?
                            @"SELECT *, 1 AS empresa
                                     FROM tblC_FED_DetProyeccionCierre
                                     WHERE esActivo = 1 AND idConceptoDir = @idReserva
                                     AND numpro not in (9867) AND numcte not in (487, 9084) AND descripcion not like '%ARRENDADORA%'" :
                            @"SELECT *, 1 AS empresa
                                     FROM tblC_FED_DetProyeccionCierre
                                     WHERE esActivo = 1 AND idConceptoDir = @idReserva",
                parametros = new
                {
                    idReserva
                }
            });
            lstCompleta.AddRange(lstCompletaReserva);
            if (esTodos)
            {
                lst = (from cierre in lstCompleta where cierre.cc == "TODOS" select cierre).ToList();
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
                lst = (from cierre in lstCompleta where busq.lstCC.Contains(cierre.cc) select cierre).ToList();
            }
            lst.ForEach(cierre => cierre.empresa = EmpresaEnum.Construplan);
            #endregion
            #region Arrendadora
            if (busq.esConciliado)
            {
                using (var _ctxArrendadora = new MainContext(EmpresaEnum.Arrendadora))
                {
                    //var lstArrendadora = (from det in _ctxArrendadora.tblC_FED_DetProyeccionCierre.AsQueryable()
                    //                      where det.esActivo && det.idConceptoDir != idReserva && det.anio == anio && det.semana == semana
                    //                      select det).ToList();
                    var lstArrendadora = _context.Select<tblC_FED_DetProyeccionCierre>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Arrendadora,
                        consulta = @"SELECT *, 2 AS empresa
                                            FROM tblC_FED_DetProyeccionCierre
                                            WHERE esActivo = 1 AND idConceptoDir <> @idReserva AND anio = @anio AND semana = @semana
                                            AND numpro not in (5412) AND numcte not in (504, 9076) AND descripcion not like '%CONSTRUCCIONES PLANIFICADAS%",
                        parametros = new
                        {
                            idReserva,
                            anio,
                            semana
                        }
                    });
                    var lstArrendadoraReserva = _context.Select<tblC_FED_DetProyeccionCierre>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Arrendadora,
                        consulta = @"SELECT *, 1 AS empresa
                                    FROM tblC_FED_DetProyeccionCierre
                                    WHERE esActivo = 1 AND idConceptoDir = @idReserva
                                    AND numpro not in (5412) AND numcte not in (504, 9076) AND descripcion not like '%CONSTRUCCIONES PLANIFICADAS%'",
                        parametros = new
                        {
                            idReserva
                        }
                    });
                    lstArrendadora.AddRange(lstArrendadoraReserva);
                    if (esTodos)
                    {
                        var lstArrendTodo = (from det in lstArrendadora where det.ac == "TODOS" select det).ToList();
                        lstArrendTodo.Where(cierre => cierre.idDetProyGemelo > 0).ToList().ForEach(cierre =>
                        {
                            var gemeloAC = (from det in lstArrendadora
                                            where det.idDetProyGemelo == cierre.id
                                            select det.ac).FirstOrDefault();
                            cierre.ac = gemeloAC;
                        });
                        lstArrendTodo.ForEach(cierre => cierre.empresa = EmpresaEnum.Arrendadora);
                        lst.AddRange(lstArrendTodo);
                    }
                    else
                    {
                        var lstArrenAC = (from det in lstArrendadora where busq.lstAC.Contains(det.ac) select det).ToList();
                        lst.AddRange(lstArrenAC);
                    }
                }
            }
            #endregion
            AjustarMontosCierre(lst);
            lst.AddRange(getLstDetProyeccionCierre(busq));
            return lst;
        }
        private void AjustarMontosCierre(List<tblC_FED_DetProyeccionCierre> lst)
        {
            var lstConceptosCierre = ConsultarConceptosCierre();
            lst.ForEach(cierre =>
            {
                if(lstCptoInvertirSigno.Contains(cierre.idConceptoDir))
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
        public List<tblC_FED_DetProyeccionCierre> getLstDetReservaDelAnio(BusqFlujoEfectivoDTO busq)
        {
            var idReserva = 29;
            var semana = busq.max.noSemana();
            var lstGrupoReserva = getLstGpoReserva().Select(s => s.grupo);
            return _context.tblC_FED_DetProyeccionCierre
                .Where(w => w.esActivo && w.idConceptoDir == busq.idConcepto)
                .Where(w => busq.lstCC.Contains("TODOS") ? w.cc == "TODOS" : busq.lstCC.Contains(w.cc))
                .Where(w => w.idConceptoDir == idReserva ? lstGrupoReserva.Contains(w.grupo) : w.anio == busq.max.Year && w.semana <= semana).ToList();
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
        /// Consulta el movimiento de polizas del mes
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
                    _contextEnkontrol.Select<MovpolDTO>(EnkontrolAmbienteEnum.Prod, odbc)
                )
            );
            for(int i = 0; i < lst.Count; i++)
            {
                var mov = lst[i];
                if(mov.area.ParseInt() == 0 || mov.cuenta_oc.ParseInt() == 0)
                {
                    mov.area = 0;
                    mov.cuenta_oc = 0;
                }
                if(mov.numpro.ParseInt() == 0)
                {
                    mov.numpro = 0;
                }
            }
            return lst;
        }
        List<OdbcParameterDTO> paramLstMovPol(BusqFlujoEfectivoDTO busq)
        {
            var lst = new List<OdbcParameterDTO>();
            lst.AddRange(busq.lstCC.Select(s => new OdbcParameterDTO() { nombre = "cc", tipo = OdbcType.VarChar, valor = s }));
            lst.Add(new OdbcParameterDTO() { nombre = "cta", tipo = OdbcType.Numeric, valor = (int)CtaDeudorDivEnum.FondoFijoCaja });
            lst.Add(new OdbcParameterDTO() { nombre = "cta", tipo = OdbcType.Numeric, valor = (int)CtaDeudorDivEnum.Inversion });
            lst.Add(new OdbcParameterDTO() { nombre = "fechapol", tipo = OdbcType.Date, valor = busq.min });
            lst.Add(new OdbcParameterDTO() { nombre = "fechapol", tipo = OdbcType.Date, valor = busq.max });
            lst.AddRange(busq.lstTm.Select(s => new OdbcParameterDTO() { nombre = "itm", tipo = OdbcType.Numeric, valor = s }));
            return lst;
        }
        string queryLstMovPol(BusqFlujoEfectivoDTO busq)
        {
            return string.Format(@"SELECT mov.year ,mov.mes ,mov.poliza ,mov.tp ,mov.linea ,mov.cta ,mov.scta ,mov.sscta ,mov.digito ,mov.tm ,mov.referencia ,mov.cc ,mov.concepto ,mov.monto ,mov.iclave ,mov.itm ,mov.st_par ,mov.orden_compra ,mov.numpro ,pol.fechapol
                                    FROM sc_movpol mov
                                  INNER JOIN sc_polizas pol ON pol.year = mov.year AND pol.mes = mov.mes AND pol.tp = mov.tp AND pol.poliza = mov.poliza
                                        WHERE mov.cc IN {0}
                                        AND mov.cta BETWEEN ? AND ?
                                        AND pol.fechapol BETWEEN ? AND ?
                                        AND mov.itm IN {1}"
                , busq.lstCC.ToParamInValue()
                , busq.lstTm.ToParamInValue());
        }
        string queryLstMultiPol(BusqFlujoEfectivoDTO busq)
        {
            return string.Format(@"SELECT dd.year ,dd.mes ,dd.poliza ,dd.tp ,mov.linea ,dd.cta ,dd.scta ,dd.sscta ,dd.digito ,dd.tm ,mov.referencia ,mov.cc ,mov.concepto ,mov.monto ,dd.iclave ,dd.itm ,dd.st_par ,mov.orden_compra ,mov.numpro ,pol.fechapol
                        FROM sc_movpol dd
                        INNER JOIN sc_polizas pol ON pol.year = dd.year AND pol.mes = dd.mes AND pol.tp = dd.tp AND pol.poliza = dd.poliza
                        INNER JOIN sc_movpol mov ON mov.year = dd.year AND mov.mes = dd.mes AND mov.tp = dd.tp AND mov.poliza = dd.poliza AND mov.cc <> '*' AND mov.cc IN {0}
                        WHERE dd.cta BETWEEN ? AND ?
                            AND pol.fechapol BETWEEN ? AND ?
                            AND dd.itm IN {1}
                            AND dd.cc = '*'"
                , busq.lstCC.ToParamInValue()
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
                    if(plan == null)
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
            catch(Exception o_O)
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
            catch(Exception o_O)
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
                return (from w in _context.tblC_FE_MovPol
                        where w.esActivo && w.fechapol >= min && w.fechapol <= max
                        && (esCcVacio ? true : busq.lstCC.Contains(w.cc))
                        select w).ToList();
            }
            catch(Exception)
            {
                return new List<tblC_FE_MovPol>();
            }
        }
        public List<tblC_FED_Corte> getLstCortes()
        {
            try
            {
                return _context.tblC_FED_Corte.ToList();
            }
            catch (Exception)
            {
                return new List<tblC_FED_Corte>();
            }
        }
        public List<tblC_FE_MovPol> getLstMovPolActiva()
        {
            try
            {
                return _context.tblC_FE_MovPol.Where(w => w.esActivo).ToList();
            }
            catch(Exception o_O)
            {
                return new List<tblC_FE_MovPol>();
            }
        }
        public List<tblC_FE_MovPol> getLstMovPolFlujoEfectivo(BusqFlujoEfectivoDTO busq)
        {
            try
            {
                var mes = busq.max.Month;
                var anio = busq.max.Year;
                busq.lstTm = busq.lstTm == null ? new List<int>() : busq.lstTm;
                busq.lstCC = busq.lstCC == null ? new List<string>() : busq.lstCC;
                var esTmVacio = !busq.lstTm.Any();
                var esCcVacio = !busq.lstCC.Any();
                return _context.tblC_FE_MovPol
                .Where(w => w.esActivo)
                .Where(w => w.esFlujoEfectivo)
                .Where(w => anio == 2000 ? true : w.year == anio && w.mes <= mes)
                .Where(w => esTmVacio ? true : busq.lstTm.Contains(w.itm))
                .Where(w => esCcVacio ? true : busq.lstCC.Contains(w.cc)).ToList();
            }
            catch(Exception o_O)
            {
                return new List<tblC_FE_MovPol>();
            }
        }
        public List<tblC_FE_MovPol> getLstMovPolFlujoEfectivoOperativo(BusqFlujoEfectivoDTO busq)
        {
            try
            {
                var esAnioInicial = busq.max.Year == 2000;
                var esTmVacio = busq.lstTm == null;
                var esCCvacio = busq.lstCC == null;
                return (from mov in _context.tblC_FE_MovPol
                        where mov.esActivo && mov.esFlujoEfectivo && mov.idConcepto > 0
                        && (esAnioInicial ? true : mov.year == busq.max.Year && mov.mes <= busq.max.Month)
                        && (esTmVacio ? true : busq.lstTm.Contains(mov.itm))
                        && (esCCvacio ? true : busq.lstCC.Contains(mov.cc))
                        select mov).ToList();
            }
            catch(Exception)
            {
                return new List<tblC_FE_MovPol>();
            }
        }
        public Task<List<tblC_FE_MovPol>> getLstMovPolFlujoEfectivoDirecto(BusqFlujoEfectivoDTO busq)
        {
            return Task.Run(() => 
            {
                var esTodos = busq.lstCC.Contains("TODOS");
                var movPolCplan = new DapperDTO()
                {
                    baseDatos = MainContextEnum.Construplan,
                    consulta = string.Format(@"SELECT *, 1 AS empresa
                                                FROM tblC_FE_MovPol
                                                	WHERE esActivo = 1 AND esFlujoEfectivo = 1 and idConceptoDir > 0
                                                    AND fechapol BETWEEN @min AND @max
                                                	AND itm in @itms
                                                    {0} {1}"
                    , esTodos ? string.Empty : "AND cc in @cc"
                    //se eliminan registros relacionados con arrendadora para evitar duplicidad de informacion
                    , busq.esConciliado ? "AND (numpro is NULL OR numpro not in (9867, 4835)) AND concepto not like '%ARRENDADORA%'" : string.Empty),
                    parametros = new
                    {
                        min = busq.min,
                        max = busq.max,
                        itms = busq.lstTm.Select(itm => itm).ToArray(),
                        cc = busq.lstCC.Select(cc => cc).ToArray()
                    }
                };
                var lstMovPol = _context.Select<tblC_FE_MovPol>(movPolCplan);
                lstMovPol.ForEach(movpol => movpol.empresa = EmpresaEnum.Construplan);
                if(busq.esConciliado)
                {
                    using(var _ctxArrendadora = new MainContext(EmpresaEnum.Arrendadora))
                    {
                        var movPolArren = new DapperDTO
                        {
                            baseDatos = MainContextEnum.Arrendadora,
                            consulta = string.Format(@"SELECT *, 2 as empresa
                                         FROM tblC_FE_MovPol
                                            WHERE esActivo = 1 AND esFlujoEfectivo = 1 and idConceptoDir > 0
                                            AND fechapol BETWEEN @min AND @max
                                            AND itm in @itms
                                            {0} {1}"
                            , esTodos ? string.Empty : "AND ac in @ac"
                            , busq.esConciliado ? "AND (numpro is NULL OR numpro not in (5412))  AND descripcion not like '%CONSTRUCCIONES PLANIFICADAS%" : string.Empty),
                            parametros = new
                            {
                                min = busq.min,
                                max = busq.max,
                                itms = busq.lstTm.Select(itm => itm).ToArray(),
                                ac = busq.lstAC.Select(ac => ac).ToArray()
                            }
                        };
                        var lstMovPolArrendadora = _context.Select<tblC_FE_MovPol>(movPolArren);
                        lstMovPol.AddRange(lstMovPolArrendadora);
                    }
                }
                return lstMovPol;
            });            
        }
        public List<tblC_FE_MovPol> getLstMovPolFlujoEfectivoDirecto_Optimizado(BusqFlujoEfectivoDTO busq)
        {
            var esTodos = busq.lstCC.Contains("TODOS");
            var movPolCplan = new DapperDTO()
            {
                baseDatos = MainContextEnum.Construplan,
                consulta = string.Format(@"SELECT *, 1 AS empresa
                                                FROM tblC_FE_MovPol
                                                	WHERE esActivo = 1 AND esFlujoEfectivo = 1 and idConceptoDir > 0
                                                    AND fechapol BETWEEN @min AND @max
                                                	AND itm in @itms
                                                    {0} {1}"
                , esTodos ? string.Empty : "AND cc in @cc"
                    //se eliminan registros relacionados con arrendadora para evitar duplicidad de informacion
                , busq.esConciliado ? "AND (numpro is NULL OR numpro not in (9867, 4835)) AND concepto not like '%ARRENDADORA%'" : string.Empty),
                parametros = new
                {
                    min = busq.min,
                    max = busq.max,
                    itms = busq.lstTm.Select(itm => itm).ToArray(),
                    cc = busq.lstCC.Select(cc => cc).ToArray()
                }
            };
            var lstMovPol = _context.Select<tblC_FE_MovPol>(movPolCplan);
            lstMovPol.ForEach(movpol => movpol.empresa = EmpresaEnum.Construplan);
            if (busq.esConciliado)
            {
                using (var _ctxArrendadora = new MainContext(EmpresaEnum.Arrendadora))
                {
                    var movPolArren = new DapperDTO
                    {
                        baseDatos = MainContextEnum.Arrendadora,
                        consulta = string.Format(@"SELECT *, 2 as empresa
                                         FROM tblC_FE_MovPol
                                            WHERE esActivo = 1 AND esFlujoEfectivo = 1 and idConceptoDir > 0
                                            AND fechapol BETWEEN @min AND @max
                                            AND itm in @itms
                                            {0} {1}"
                        , esTodos ? string.Empty : "AND ac in @ac"
                        , busq.esConciliado ? "AND (numpro is NULL OR numpro not in (5412))  AND descripcion not like '%CONSTRUCCIONES PLANIFICADAS%" : string.Empty),
                        parametros = new
                        {
                            min = busq.min,
                            max = busq.max,
                            itms = busq.lstTm.Select(itm => itm).ToArray(),
                            ac = busq.lstAC.Select(ac => ac).ToArray()
                        }
                    };
                    var lstMovPolArrendadora = _context.Select<tblC_FE_MovPol>(movPolArren);
                    lstMovPol.AddRange(lstMovPolArrendadora);
                }
            }
            return lstMovPol;
        }
        public List<tblC_FE_MovPol> getLstMovPolFlujoEfectivoDirecto_Optimizado(tipoDetalleEnum tipo,BusqFlujoEfectivoDTO busq)
        {
            var esTodos = busq.lstCC.Contains("TODOS");
            string Query = "";
            switch(tipo)
            {
                case tipoDetalleEnum.FlujoTotalCuenta:
                    {
                        Query = string.Format(@"SELECT cta,scta,sscta,SUM(monto), 1 AS empresa
                                                FROM tblC_FE_MovPol
                                                	WHERE esActivo = 1 AND esFlujoEfectivo = 1 and idConceptoDir > 0
                                                    AND fechapol BETWEEN @min AND @max
                                                	AND itm in @itms
                                                    {0} {1} group by cta,scta,sscta"
                        , esTodos ? string.Empty : "AND cc in @cc"
                            //se eliminan registros relacionados con arrendadora para evitar duplicidad de informacion
                        , busq.esConciliado ? "AND (numpro is NULL OR numpro not in (9867, 4835)) AND concepto not like '%ARRENDADORA%'" : string.Empty);
                    }
                    break;
                default:
                    break;
            }
            var movPolCplan = new DapperDTO()
            {
                baseDatos = MainContextEnum.Construplan,

                consulta = Query,
                parametros = new
                {
                    min = busq.min,
                    max = busq.max,
                    itms = busq.lstTm.Select(itm => itm).ToArray(),
                    cc = busq.lstCC.Select(cc => cc).ToArray()
                }
            };
            var lstMovPol = _context.Select<tblC_FE_MovPol>(movPolCplan);
            lstMovPol.ForEach(movpol => movpol.empresa = EmpresaEnum.Construplan);
            if (busq.esConciliado)
            {
                using (var _ctxArrendadora = new MainContext(EmpresaEnum.Arrendadora))
                {
                    var movPolArren = new DapperDTO
                    {
                        baseDatos = MainContextEnum.Arrendadora,
                        consulta = string.Format(@"SELECT *, 2 as empresa
                                         FROM tblC_FE_MovPol
                                            WHERE esActivo = 1 AND esFlujoEfectivo = 1 and idConceptoDir > 0
                                            AND fechapol BETWEEN @min AND @max
                                            AND itm in @itms
                                            {0} {1}"
                        , esTodos ? string.Empty : "AND ac in @ac"
                        , busq.esConciliado ? "AND (numpro is NULL OR numpro not in (5412))  AND descripcion not like '%CONSTRUCCIONES PLANIFICADAS%" : string.Empty),
                        parametros = new
                        {
                            min = busq.min,
                            max = busq.max,
                            itms = busq.lstTm.Select(itm => itm).ToArray(),
                            ac = busq.lstAC.Select(ac => ac).ToArray()
                        }
                    };
                    var lstMovPolArrendadora = _context.Select<tblC_FE_MovPol>(movPolArren);
                    lstMovPol.AddRange(lstMovPolArrendadora);
                }
            }
            return lstMovPol;
        }
        public Task<List<tblC_FED_PlaneacionDet>> getLstDetPlaneacionDirecto(BusqFlujoEfectivoDTO busq)
        {
            return Task.Run(() =>
                {
                    var detPlaneaciones = new List<tblC_FED_PlaneacionDet>();
                    var max = busq.max.AddDays(7);
                    var minAnio = busq.min.Year;
                    var minSemana = busq.min.noSemana();
                    var maxAnio = max.Year;
                    var maxSemana = max.noSemana();
                    var esTodos = busq.lstCC.Contains("TODOS");
                    List<int> proveedoresArr = Enum.GetValues(typeof(numproArrendadoraEnum)).Cast<int>().ToList();
                    List<int> clientesArr = Enum.GetValues(typeof(numcteArrendadoraEnum)).Cast<int>().ToList();
                    #region Construplan
                    var error2 = new DapperDTO
                        {
                            baseDatos = MainContextEnum.Construplan,
                            //se eliminan registros relacionados con arrendadora para evitar duplicidad de informacion
                            consulta = busq.esConciliado ? @"SELECT *, 1 AS empresa
                                        FROM tblC_FED_PlaneacionDet
                                        WHERE (estatus = 1 AND                                        
                                        (año = @minAnio AND semana = @minSemana) OR
                                        (año = @maxAnio AND semana = @maxSemana))
                                        AND descripcion not like '%ARRENDADORA%'
                                        AND ((numprov is NULL OR numprov not in (9867,4835)) AND (numcte is NULL OR numcte not in (487,9084)))" :
                                        @"SELECT *, 1 AS empresa
                                        FROM tblC_FED_PlaneacionDet
                                        WHERE estatus = 1 AND                                        
                                        (año = @minAnio AND semana = @minSemana) OR
                                        (año = @maxAnio AND semana = @maxSemana)",
                            parametros = new
                            {
                                minAnio,
                                minSemana,
                                maxAnio,
                                maxSemana,
                                proveedoresArr = string.Join(",", proveedoresArr.Select(n => n.ToString()).ToArray()),
                                clientesArr = string.Join(",", clientesArr.Select(n => n.ToString()).ToArray())
                            }
                        };

                    var detPlanCompleta = _context.Select<tblC_FED_PlaneacionDet>(error2);
                    var error = detPlanCompleta.Where(x => proveedoresArr.Contains(x.numprov) || clientesArr.Contains(x.numcte)).ToList();
                    if(esTodos)
                    {
                        detPlaneaciones = (from plan in detPlanCompleta where plan.cc != "TODOS" select plan).ToList();
                    }
                    else
                    {
                        detPlaneaciones = (from plan in detPlanCompleta where busq.lstCC.Contains(plan.cc) select plan).ToList();
                    }
                    #endregion
                    #region Arrendadora
                    if(busq.esConciliado)
                    {
                        using(var _ctxArrendadora = new MainContext(EmpresaEnum.Arrendadora))
                        {
                            var detPlanArrendadora = _context.Select<tblC_FED_PlaneacionDet>(new DapperDTO 
                            {
                                baseDatos = MainContextEnum.Arrendadora,
                                consulta = @"SELECT *, 2 AS empresa
                                        FROM tblC_FED_PlaneacionDet
                                        WHERE (estatus = 1 AND
                                        (año = @minAnio AND semana = @minSemana) OR
                                        (año = @maxAnio AND semana = @maxSemana))
                                        AND ((numprov is NULL OR numprov not in (5412)) AND (numcte is NULL OR numcte not in (504, 9076)))
                                        AND descripcion not like '%CONSTRUCCIONES PLANIFICADAS%'",
                                parametros = new
                                {
                                    minAnio,
                                    minSemana,
                                    maxAnio,
                                    maxSemana
                                }
                            });
                            if(esTodos)
                            {
                                var detPlanArrenTodo = (from det in detPlanArrendadora where det.ac == "TODOS" select det).ToList();
                                detPlanArrenTodo.Where(plan => plan.idDetProyGemelo > 0).ToList().ForEach(plan =>
                                {
                                    var gemeloAC = (from det in detPlanArrendadora
                                                    where det.id == plan.idDetProyGemelo
                                                    select det.ac).FirstOrDefault();
                                    plan.ac = gemeloAC;
                                });
                                detPlaneaciones.AddRange(detPlanArrenTodo);
                            }
                            else
                            {
                                var detPlanAC = (from det in detPlanArrendadora where busq.lstAC.Contains(det.ac) select det).ToList();
                                detPlaneaciones.AddRange(detPlanAC);
                            }
                        }
                    }
                    #endregion
                    #region Asignar tipo
                    detPlaneaciones.ForEach(plan =>
                    {
                        if(plan.cadenaProductivaID != 0)
                        {
                            plan.tipo = tipoDetallePlaneacionEnum.cadenaProductiva;
                        }
                        else if(plan.nominaID != 0)
                        {
                            plan.tipo = tipoDetallePlaneacionEnum.nomina;
                        }
                        else if(plan.sp_gastos_provID != 0)
                        {
                            plan.tipo = tipoDetallePlaneacionEnum.gastosProyecto;
                        }
                        else if(!string.IsNullOrEmpty(plan.factura))
                        {
                            plan.tipo = tipoDetallePlaneacionEnum.efectivoRecibido;
                        }
                        else
                        {
                            plan.tipo = tipoDetallePlaneacionEnum.manual;
                        }
                    });
                    #endregion
                    return detPlaneaciones;
                });
        }
        public List<tblC_FED_PlaneacionDet> getLstDetPlaneacionDirecto_Optimizado(BusqFlujoEfectivoDTO busq)
        {
            var detPlaneaciones = new List<tblC_FED_PlaneacionDet>();
            var max = busq.max.AddDays(7);
            var minAnio = busq.min.Year;
            var minSemana = busq.min.noSemana();
            var maxAnio = max.Year;
            var maxSemana = max.noSemana();
            var esTodos = busq.lstCC.Contains("TODOS");
            List<int> proveedoresArr = Enum.GetValues(typeof(numproArrendadoraEnum)).Cast<int>().ToList();
            List<int> clientesArr = Enum.GetValues(typeof(numcteArrendadoraEnum)).Cast<int>().ToList();
            #region Construplan
            var error2 = new DapperDTO
            {
                baseDatos = MainContextEnum.Construplan,
                //se eliminan registros relacionados con arrendadora para evitar duplicidad de informacion
                consulta = busq.esConciliado ? @"SELECT *, 1 AS empresa
                                        FROM tblC_FED_PlaneacionDet
                                        WHERE (estatus = 1 AND                                        
                                        (año = @minAnio AND semana = @minSemana) OR
                                        (año = @maxAnio AND semana = @maxSemana))
                                        AND descripcion not like '%ARRENDADORA%'
                                        AND ((numprov is NULL OR numprov not in (9867,4835)) AND (numcte is NULL OR numcte not in (487,9084)))" :
                            @"SELECT *, 1 AS empresa
                                        FROM tblC_FED_PlaneacionDet
                                        WHERE estatus = 1 AND                                        
                                        (año = @minAnio AND semana = @minSemana) OR
                                        (año = @maxAnio AND semana = @maxSemana)",
                parametros = new
                {
                    minAnio,
                    minSemana,
                    maxAnio,
                    maxSemana,
                    proveedoresArr = string.Join(",", proveedoresArr.Select(n => n.ToString()).ToArray()),
                    clientesArr = string.Join(",", clientesArr.Select(n => n.ToString()).ToArray())
                }
            };

            var detPlanCompleta = _context.Select<tblC_FED_PlaneacionDet>(error2);
            var error = detPlanCompleta.Where(x => proveedoresArr.Contains(x.numprov) || clientesArr.Contains(x.numcte)).ToList();
            if (esTodos)
            {
                detPlaneaciones = (from plan in detPlanCompleta where plan.cc != "TODOS" select plan).ToList();
            }
            else
            {
                detPlaneaciones = (from plan in detPlanCompleta where busq.lstCC.Contains(plan.cc) select plan).ToList();
            }
            #endregion
            #region Arrendadora
            if (busq.esConciliado)
            {
                using (var _ctxArrendadora = new MainContext(EmpresaEnum.Arrendadora))
                {
                    var detPlanArrendadora = _context.Select<tblC_FED_PlaneacionDet>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Arrendadora,
                        consulta = @"SELECT *, 2 AS empresa
                                        FROM tblC_FED_PlaneacionDet
                                        WHERE (estatus = 1 AND
                                        (año = @minAnio AND semana = @minSemana) OR
                                        (año = @maxAnio AND semana = @maxSemana))
                                        AND ((numprov is NULL OR numprov not in (5412)) AND (numcte is NULL OR numcte not in (504, 9076)))
                                        AND descripcion not like '%CONSTRUCCIONES PLANIFICADAS%'",
                        parametros = new
                        {
                            minAnio,
                            minSemana,
                            maxAnio,
                            maxSemana
                        }
                    });
                    if (esTodos)
                    {
                        var detPlanArrenTodo = (from det in detPlanArrendadora where det.ac == "TODOS" select det).ToList();
                        detPlanArrenTodo.Where(plan => plan.idDetProyGemelo > 0).ToList().ForEach(plan =>
                        {
                            var gemeloAC = (from det in detPlanArrendadora
                                            where det.id == plan.idDetProyGemelo
                                            select det.ac).FirstOrDefault();
                            plan.ac = gemeloAC;
                        });
                        detPlaneaciones.AddRange(detPlanArrenTodo);
                    }
                    else
                    {
                        var detPlanAC = (from det in detPlanArrendadora where busq.lstAC.Contains(det.ac) select det).ToList();
                        detPlaneaciones.AddRange(detPlanAC);
                    }
                }
            }
            #endregion
            #region Asignar tipo
            detPlaneaciones.ForEach(plan =>
            {
                if (plan.cadenaProductivaID != 0)
                {
                    plan.tipo = tipoDetallePlaneacionEnum.cadenaProductiva;
                }
                else if (plan.nominaID != 0)
                {
                    plan.tipo = tipoDetallePlaneacionEnum.nomina;
                }
                else if (plan.sp_gastos_provID != 0)
                {
                    plan.tipo = tipoDetallePlaneacionEnum.gastosProyecto;
                }
                else if (!string.IsNullOrEmpty(plan.factura))
                {
                    plan.tipo = tipoDetallePlaneacionEnum.efectivoRecibido;
                }
                else
                {
                    plan.tipo = tipoDetallePlaneacionEnum.manual;
                }
            });
            #endregion
            return detPlaneaciones;
        }
        List<tblC_FE_MovPol> getAllMovPol(BusqFlujoEfectivoDTO busq)
        {
            try
            {
                busq.lstTm = busq.lstTm == null ? new List<int>() : busq.lstTm;
                busq.lstCC = busq.lstCC == null ? new List<string>() : busq.lstCC;
                var esTmVacio = !busq.lstTm.Any();
                var esCcVacio = !busq.lstCC.Any();
                return _context.tblC_FE_MovPol
                .Where(w => w.year == busq.max.Year && w.mes == busq.max.Month)
                .Where(w => esTmVacio ? true : busq.lstTm.Contains(w.itm))
                .Where(w => esCcVacio ? true : busq.lstCC.Contains(w.cc)).ToList();
            }
            catch(Exception o_O)
            {
                return new List<tblC_FE_MovPol>();
            }
        }
        List<tblC_FE_MovPol> getAllMovPolDelAnio(BusqFlujoEfectivoDTO busq)
        {
            try
            {
                busq.lstTm = busq.lstTm == null ? new List<int>() : busq.lstTm;
                busq.lstCC = busq.lstCC == null ? new List<string>() : busq.lstCC;
                var esTmVacio = !busq.lstTm.Any();
                var esCcVacio = !busq.lstCC.Any();
                return _context.tblC_FE_MovPol
                .Where(w => w.esActivo)
                .Where(w => w.year == busq.max.Year)
                .Where(w => esTmVacio ? true : busq.lstTm.Contains(w.itm))
                .Where(w => esCcVacio ? true : busq.lstCC.Contains(w.cc)).ToList();
            }
            catch(Exception o_O)
            {
                return new List<tblC_FE_MovPol>();
            }
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
            return lst;
        }
        #region taskLstMovPolFlujoTotal
        public Task<List<MovpolDTO>> taskLstMovPolFlujoTotal(BusqFlujoEfectivoDTO busq)
        {
            return Task.Run(() =>
            {
                #region Construplan
                var lstOdbcCplan = new List<OdbcConsultaDTO>()
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
                var movPolCplan = _contextEnkontrol.Select<MovpolDTO>(EnkontrolEnum.CplanProd, lstOdbcCplan);
                #endregion
                #region Arrendadora
                if(busq.esConciliado)
                {
                    var esTodos = busq.lstCC.Contains("TODOS");
                    var lstOdbcArrendadora = new List<OdbcConsultaDTO>()
                    {
                        new OdbcConsultaDTO()
                        {
                            consulta = queryLstMovPolFlujoTotalArrendadora(busq),
                            parametros = paramLstMovPolFlujoTotalArrendadora(busq)
                        },
                        new OdbcConsultaDTO()
                        {
                            consulta = queryLstMultiPolFlujoTotalArrendadora(busq),
                            parametros = paramLstMovPolFlujoTotalArrendadora(busq)
                        }
                    };
                    var movPolArrend = (from movpol in _contextEnkontrol.Select<MovpolDTO>(EnkontrolEnum.ArrenProd, lstOdbcArrendadora)
                                        where esTodos ? true : busq.lstAC.Contains(movpol.area + "-" + movpol.cuenta_oc)
                                        select movpol).ToList();
                    movPolArrend.ForEach(mov =>
                    {
                        mov.area = mov.area.ParseInt();
                        mov.cuenta_oc = mov.cuenta_oc.ParseInt();
                        if(mov.area == 0 || mov.cuenta_oc == 0)
                        {
                            mov.area = 0;
                            mov.cuenta_oc = 0;
                        }
                    });
                    movPolCplan.AddRange(movPolArrend);
                }
                #endregion
                return movPolCplan;
            });
        }
        public List<MovpolDTO> taskLstMovPolFlujoTotal_Optimizado(FiltroPolizasDTO filtro)
        {
            #region Construplan
            var lstOdbcCplan = new List<OdbcConsultaDTO>()
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
            var movPolCplan = _contextEnkontrol.Select<MovpolDTO>(EnkontrolEnum.CplanProd, lstOdbcCplan);
            #endregion
            #region Arrendadora
            if (filtro.busq.esConciliado)
            {
                var esTodos = filtro.busq.lstCC.Contains("TODOS");
                var lstOdbcArrendadora = new List<OdbcConsultaDTO>()
                    {
                        new OdbcConsultaDTO()
                        {
                            consulta = queryLstMovPolFlujoTotalArrendadora_Optimizado(filtro),
                            parametros = paramLstMovPolFlujoTotalArrendadora_Optimizado(filtro)
                        },
                        new OdbcConsultaDTO()
                        {
                            consulta = queryLstMultiPolFlujoTotalArrendadora_Optimizado(filtro),
                            parametros = paramLstMovPolFlujoTotalArrendadora_Optimizado(filtro)
                        }
                    };
                var movPolArrend = _contextEnkontrol.Select<MovpolDTO>(EnkontrolEnum.ArrenProd, lstOdbcArrendadora);

                //movPolArrend.ForEach(mov =>
                //{
                //    mov.area = mov.area.ParseInt();
                //    mov.cuenta_oc = mov.cuenta_oc.ParseInt();
                //    if (mov.area == 0 || mov.cuenta_oc == 0)
                //    {
                //        mov.area = 0;
                //        mov.cuenta_oc = 0;
                //    }
                //});
                movPolCplan.AddRange(movPolArrend);
            }
            #endregion
            return movPolCplan;
        }
        #region Construplan
        List<OdbcParameterDTO> paramLstMovPolFlujoTotal(BusqFlujoEfectivoDTO busq)
        {
            var lst = new List<OdbcParameterDTO>();
            lst.AddRange(busq.lstCC.Select(s => new OdbcParameterDTO() { nombre = "cc", tipo = OdbcType.VarChar, valor = s }).ToList());
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
            lst.AddRange(filtro.lstCC.Select(s => new OdbcParameterDTO() { nombre = "cc", tipo = OdbcType.VarChar, valor = s }).ToList());
            
            if (filtro.busqDet.cta > 0)
            {
                lst.Add(new OdbcParameterDTO() { nombre = "cta", tipo = OdbcType.Numeric, valor = filtro.busqDet.cta });
                lst.Add(new OdbcParameterDTO() { nombre = "scta", tipo = OdbcType.Numeric, valor = filtro.busqDet.scta });
                lst.Add(new OdbcParameterDTO() { nombre = "sscta", tipo = OdbcType.Numeric, valor = filtro.busqDet.sscta });
            }
            else {
                lst.Add(new OdbcParameterDTO() { nombre = "cta", tipo = OdbcType.Numeric, valor = (int)CtaDeudorDivEnum.FondoFijoCaja });
                lst.Add(new OdbcParameterDTO() { nombre = "cta", tipo = OdbcType.Numeric, valor = (int)CtaDeudorDivEnum.Inversion });
            }
            lst.Add(new OdbcParameterDTO() { nombre = "fechapol", tipo = OdbcType.Date, valor = filtro.busq.min });
            lst.Add(new OdbcParameterDTO() { nombre = "fechapol", tipo = OdbcType.Date, valor = filtro.busq.max });
            lst.AddRange(filtro.listTM.Select(s => new OdbcParameterDTO() { nombre = "itm", tipo = OdbcType.Numeric, valor = s }).ToList());
            return lst;
        }
        List<OdbcParameterDTO> paramLstMovPolFlujoTotalArrendadora_Optimizado(FiltroPolizasDTO filtro)
        {
            var lst = new List<OdbcParameterDTO>();
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
            lst.Add(new OdbcParameterDTO() { nombre = "fechapol", tipo = OdbcType.Date, valor = filtro.busq.min });
            lst.Add(new OdbcParameterDTO() { nombre = "fechapol", tipo = OdbcType.Date, valor = filtro.busq.max });
            lst.AddRange(filtro.busq.lstAC.Select(s => new OdbcParameterDTO() { nombre = "ac", tipo = OdbcType.VarChar, valor = s }).ToList());
            return lst;
        }
        string queryLstMovPolFlujoTotal(BusqFlujoEfectivoDTO busq)
        {
            if (busq.esConciliado)
            {
                //se eliminan registros relacionados con arrendadora para evitar duplicidad de informacion
                return string.Format(@"SELECT mov.cc, mov.itm, mov.cta, mov.scta, mov.sscta, mov.numpro, mov.concepto, sum(mov.monto) AS monto, max(pol.fechapol) AS fechapol, 1 AS empresa
                                    FROM sc_movpol mov
                                  INNER JOIN sc_polizas pol ON pol.year = mov.year AND pol.mes = mov.mes AND pol.tp = mov.tp AND pol.poliza = mov.poliza
                                        WHERE mov.cc IN {0}
                                        AND mov.cta BETWEEN ? AND ?
                                        AND pol.fechapol BETWEEN ? AND ?
                                        AND mov.itm IN {1}
                                        AND (mov.numpro is NULL OR mov.numpro not in (9867, 4835))
                                        
                                GROUP BY mov.cc, mov.itm, mov.cta, mov.scta, mov.sscta, mov.numpro, mov.concepto"
                , busq.lstCC.ToParamInValue()
                , busq.lstTm.ToParamInValue());
            }
            else 
            {
                return string.Format(@"SELECT mov.cc, mov.itm, mov.cta, mov.scta, mov.sscta, mov.numpro, mov.concepto, sum(mov.monto) AS monto, max(pol.fechapol) AS fechapol, 1 AS empresa
                                    FROM sc_movpol mov
                                  INNER JOIN sc_polizas pol ON pol.year = mov.year AND pol.mes = mov.mes AND pol.tp = mov.tp AND pol.poliza = mov.poliza
                                        WHERE mov.cc IN {0}
                                        AND mov.cta BETWEEN ? AND ?
                                        AND pol.fechapol BETWEEN ? AND ?
                                        AND mov.itm IN {1}
                                GROUP BY mov.cc, mov.itm, mov.cta, mov.scta, mov.sscta, mov.numpro, mov.concepto"
                , busq.lstCC.ToParamInValue()
                , busq.lstTm.ToParamInValue());
            }
        }
        string queryLstMultiPolFlujoTotal(BusqFlujoEfectivoDTO busq)
        {
            if (busq.esConciliado)
            {
                //se eliminan registros relacionados con arrendadora para evitar duplicidad de informacion
                return string.Format(@"SELECT mov.cc, mov.itm, dd.cta, dd.scta, dd.sscta, mov.numpro, mov.concepto, sum(mov.monto) AS monto, max(pol.fechapol) AS fechapol, 1 AS empresa
                        FROM sc_movpol dd
                        INNER JOIN sc_polizas pol ON pol.year = dd.year AND pol.mes = dd.mes AND pol.tp = dd.tp AND pol.poliza = dd.poliza
                        INNER JOIN sc_movpol mov ON mov.year = dd.year AND mov.mes = dd.mes AND mov.tp = dd.tp AND mov.poliza = dd.poliza AND mov.cc <> '*' AND mov.cc IN {0}
                        WHERE dd.cta BETWEEN ? AND ? 
                        AND dd.cc = '*' 
                        AND pol.fechapol BETWEEN ? AND ?
                        AND mov.itm IN {1}
                        AND (mov.numpro is NULL OR mov.numpro not in (9867, 4835))
                        AND dd.cc = '*'
                        GROUP BY mov.cc, mov.itm, dd.cta, dd.scta, dd.sscta, mov.numpro, mov.concepto"
                , busq.lstCC.ToParamInValue()
                , busq.lstTm.ToParamInValue());
            }
            else 
            {
                return string.Format(@"SELECT mov.cc, mov.itm, dd.cta, dd.scta, dd.sscta, mov.numpro, mov.concepto, sum(mov.monto) AS monto, max(pol.fechapol) AS fechapol, 1 AS empresa
                        FROM sc_movpol dd
                        INNER JOIN sc_polizas pol ON pol.year = dd.year AND pol.mes = dd.mes AND pol.tp = dd.tp AND pol.poliza = dd.poliza
                        INNER JOIN sc_movpol mov ON mov.year = dd.year AND mov.mes = dd.mes AND mov.tp = dd.tp AND mov.poliza = dd.poliza AND mov.cc <> '*' AND mov.cc IN {0}
                        WHERE dd.cta BETWEEN ? AND ? 
                        AND dd.cc = '*' 
                        AND pol.fechapol BETWEEN ? AND ?
                        AND mov.itm IN {1}
                            AND dd.cc = '*'
                        GROUP BY mov.cc, mov.itm, dd.cta, dd.scta, dd.sscta, mov.numpro, mov.concepto"
                , busq.lstCC.ToParamInValue()
                , busq.lstTm.ToParamInValue());
            }

        }
        string queryLstMovPolFlujoTotal_Optimizado(FiltroPolizasDTO filtro)
        {
            string cuentas = filtro.busqDet.cta > 0 ? " AND mov.cta = ? AND mov.scta = ? AND mov.sscta = ? " : " AND mov.cta BETWEEN ? AND ? ";
            if (filtro.busq.esConciliado)
            {
                
                //se eliminan registros relacionados con arrendadora para evitar duplicidad de informacion
                return string.Format(@"SELECT mov.cc, mov.itm, mov.cta, mov.scta, mov.sscta, mov.numpro, mov.concepto, sum(mov.monto) AS monto, max(pol.fechapol) AS fechapol, 1 AS empresa
                                    FROM sc_movpol mov
                                  INNER JOIN sc_polizas pol ON pol.year = mov.year AND pol.mes = mov.mes AND pol.tp = mov.tp AND pol.poliza = mov.poliza
                                        WHERE mov.cc IN {0} " + cuentas + @"
                                        AND pol.fechapol BETWEEN ? AND ?
                                        AND mov.itm IN {1}
                                        AND (mov.numpro is NULL OR mov.numpro not in (9867, 4835))
                                        
                                GROUP BY mov.cc, mov.itm, mov.cta, mov.scta, mov.sscta, mov.numpro, mov.concepto"
                , filtro.lstCC.ToParamInValue()
                , filtro.listTM.ToParamInValue());
            }
            else
            {
                return string.Format(@"SELECT mov.cc, mov.itm, mov.cta, mov.scta, mov.sscta, mov.numpro, mov.concepto, sum(mov.monto) AS monto, max(pol.fechapol) AS fechapol, 1 AS empresa
                                    FROM sc_movpol mov
                                  INNER JOIN sc_polizas pol ON pol.year = mov.year AND pol.mes = mov.mes AND pol.tp = mov.tp AND pol.poliza = mov.poliza
                                        WHERE mov.cc IN {0} " + cuentas + @"
                                        AND pol.fechapol BETWEEN ? AND ?
                                        AND mov.itm IN {1}
                                GROUP BY mov.cc, mov.itm, mov.cta, mov.scta, mov.sscta, mov.numpro, mov.concepto"
                , filtro.lstCC.ToParamInValue()
                , filtro.listTM.ToParamInValue());
            }
        }
        string queryLstMultiPolFlujoTotal_Optimizado(FiltroPolizasDTO filtro)
        {
            string cuentas = filtro.busqDet.cta > 0 ? " AND mov.cta = ? AND mov.scta = ? AND mov.sscta = ? " : " AND mov.cta BETWEEN ? AND ? ";
            if (filtro.busq.esConciliado)
            {
                //se eliminan registros relacionados con arrendadora para evitar duplicidad de informacion
                return string.Format(@"SELECT mov.cc, mov.itm, dd.cta, dd.scta, dd.sscta, mov.numpro, mov.concepto, sum(mov.monto) AS monto, max(pol.fechapol) AS fechapol, 1 AS empresa
                        FROM sc_movpol dd
                        INNER JOIN sc_polizas pol ON pol.year = dd.year AND pol.mes = dd.mes AND pol.tp = dd.tp AND pol.poliza = dd.poliza
                        INNER JOIN sc_movpol mov ON mov.year = dd.year AND mov.mes = dd.mes AND mov.tp = dd.tp AND mov.poliza = dd.poliza AND mov.cc <> '*' AND mov.cc IN {0}
                        WHERE dd.cc = '*' 
                        " + cuentas + @"
                        AND pol.fechapol BETWEEN ? AND ?
                        AND mov.itm IN {1}
                        AND (mov.numpro is NULL OR mov.numpro not in (9867, 4835))
                        AND dd.cc = '*'
                        GROUP BY mov.cc, mov.itm, dd.cta, dd.scta, dd.sscta, mov.numpro, mov.concepto"
                , filtro.lstCC.ToParamInValue()
                , filtro.listTM.ToParamInValue());
            }
            else
            {
                return string.Format(@"SELECT mov.cc, mov.itm, dd.cta, dd.scta, dd.sscta, mov.numpro, mov.concepto, sum(mov.monto) AS monto, max(pol.fechapol) AS fechapol, 1 AS empresa
                        FROM sc_movpol dd
                        INNER JOIN sc_polizas pol ON pol.year = dd.year AND pol.mes = dd.mes AND pol.tp = dd.tp AND pol.poliza = dd.poliza
                        INNER JOIN sc_movpol mov ON mov.year = dd.year AND mov.mes = dd.mes AND mov.tp = dd.tp AND mov.poliza = dd.poliza AND mov.cc <> '*' AND mov.cc IN {0}
                        WHERE dd.cc = '*' 
                        " + cuentas + @"
                        AND pol.fechapol BETWEEN ? AND ?
                        AND mov.itm IN {1}
                        GROUP BY mov.cc, mov.itm, dd.cta, dd.scta, dd.sscta, mov.numpro, mov.concepto"
                , filtro.lstCC.ToParamInValue()
                , filtro.listTM.ToParamInValue());
            }

        }
        string queryLstMovPolFlujoTotalArrendadora_Optimizado(FiltroPolizasDTO filtro)
        {
            string cuentas = filtro.busqDet.cta > 0 ? " mov.cta = ? AND mov.scta = ? AND mov.sscta = ? " : " mov.cta BETWEEN ? AND ? ";
            if (filtro.busq.lstCC.Contains("TODOS"))
            {
                return string.Format(@"SELECT mov.area, mov.cuenta_oc, mov.cc, mov.itm, mov.cta, mov.scta, mov.sscta, mov.numpro, mov.concepto, sum(mov.monto) AS monto, max(pol.fechapol) AS fechapol, 2 AS empresa
                                    FROM sc_movpol mov
                                  INNER JOIN sc_polizas pol ON pol.year = mov.year AND pol.mes = mov.mes AND pol.tp = mov.tp AND pol.poliza = mov.poliza
                                        WHERE " + cuentas + @"
                                        AND pol.fechapol BETWEEN ? AND ?
                                        AND mov.itm IN (0)
                                        AND (mov.numpro is NULL OR mov.numpro not in (5412))
                                GROUP BY mov.area, mov.cuenta_oc, mov.cc, mov.itm, mov.cta, mov.scta, mov.sscta, mov.numpro, mov.concepto"
                , filtro.listTMArr.ToParamInValue());
            }
            else
            {
                return string.Format(@"SELECT mov.area, mov.cuenta_oc, mov.cc, mov.itm, mov.cta, mov.scta, mov.sscta, mov.numpro, mov.concepto, sum(mov.monto) AS monto, max(pol.fechapol) AS fechapol, 2 AS empresa
                                    FROM sc_movpol mov
                                  INNER JOIN sc_polizas pol ON pol.year = mov.year AND pol.mes = mov.mes AND pol.tp = mov.tp AND pol.poliza = mov.poliza
                                        WHERE " + cuentas + @"
                                        AND pol.fechapol BETWEEN ? AND ?
                                        AND mov.itm IN (0)
                                        AND (mov.numpro is NULL OR mov.numpro not in (5412))
                                        AND (CAST(mov.area as varchar) + '-' + CAST(mov.cuenta_oc as varchar)) IN {0}
                               GROUP BY mov.area, mov.cuenta_oc, mov.cc, mov.itm, mov.cta, mov.scta, mov.sscta, mov.numpro, mov.concepto"
                , filtro.listTMArr.ToParamInValue());
            } 
        }
        string queryLstMultiPolFlujoTotalArrendadora_Optimizado(FiltroPolizasDTO filtro)
        {
            string cuentas = filtro.busqDet.cta > 0 ? " mov.cta = ? AND mov.scta = ? AND mov.sscta = ? " : " mov.cta BETWEEN ? AND ? ";
            return string.Format(@"SELECT mov.area, mov.cuenta_oc, mov.cc, mov.itm, dd.cta, dd.scta, dd.sscta, mov.numpro, mov.concepto, sum(mov.monto) AS monto, max(pol.fechapol) AS fechapol, 2 AS empresa
                        FROM sc_movpol dd
                        INNER JOIN sc_polizas pol ON pol.year = dd.year AND pol.mes = dd.mes AND pol.tp = dd.tp AND pol.poliza = dd.poliza
                        INNER JOIN sc_movpol mov ON mov.year = dd.year AND mov.mes = dd.mes AND mov.tp = dd.tp AND mov.poliza = dd.poliza AND mov.cc <> '*'
                        WHERE " + cuentas + @"
                        AND dd.cc = '*' 
                        AND pol.fechapol BETWEEN ? AND ?
                        AND mov.itm IN (0)
                        AND (mov.numpro is NULL OR mov.numpro not in (5412))
                            AND dd.cc = '*'
                        GROUP BY mov.area, mov.cuenta_oc, mov.cc, mov.itm, dd.cta, dd.scta, dd.sscta, mov.numpro, mov.concepto"
                , filtro.listTMArr.ToParamInValue());
        }
        #endregion
        #region Arrendadora
        List<OdbcParameterDTO> paramLstMovPolFlujoTotalArrendadora(BusqFlujoEfectivoDTO busq)
        {
            //List<int> lstTmArrendadora = new List<int>();
            //using (var _ctxArrendadora = new MainContext(EmpresaEnum.Arrendadora))
            //{
            //    lstTmArrendadora = _ctxArrendadora.tblC_FE_RelConceptoTm.GroupBy(g => g.tm).Select(s => s.Key).ToList();
            //}
            //_context.tblC_FE_RelConceptoTm.ToList();
            var lst = new List<OdbcParameterDTO>();
            lst.Add(new OdbcParameterDTO() { nombre = "cta", tipo = OdbcType.Numeric, valor = (int)CtaDeudorDivEnum.FondoFijoCaja });
            lst.Add(new OdbcParameterDTO() { nombre = "cta", tipo = OdbcType.Numeric, valor = (int)CtaDeudorDivEnum.Inversion });
            lst.Add(new OdbcParameterDTO() { nombre = "fechapol", tipo = OdbcType.Date, valor = busq.min });
            lst.Add(new OdbcParameterDTO() { nombre = "fechapol", tipo = OdbcType.Date, valor = busq.max });
            lst.AddRange(busq.lstAC.Select(s => new OdbcParameterDTO() { nombre = "ac", tipo = OdbcType.VarChar, valor = s }).ToList());
            return lst;
        }
        string queryLstMovPolFlujoTotalArrendadora(BusqFlujoEfectivoDTO busq)
        {
            List<int> lstTmArrendadora = new List<int>();
            using (var _ctxArrendadora = new MainContext(EmpresaEnum.Arrendadora))
            {
                lstTmArrendadora = _ctxArrendadora.tblC_FE_RelConceptoTm.GroupBy(g => g.tm).Select(s => s.Key).ToList();
            }
            if (busq.lstCC.Contains("TODOS"))
            {
                return string.Format(@"SELECT mov.area, mov.cuenta_oc, mov.cc, mov.itm, mov.cta, mov.scta, mov.sscta, mov.numpro, mov.concepto, sum(mov.monto) AS monto, max(pol.fechapol) AS fechapol, 2 AS empresa
                                    FROM sc_movpol mov
                                  INNER JOIN sc_polizas pol ON pol.year = mov.year AND pol.mes = mov.mes AND pol.tp = mov.tp AND pol.poliza = mov.poliza
                                        WHERE mov.cta BETWEEN ? AND ?
                                        AND pol.fechapol BETWEEN ? AND ?
                                        AND mov.itm IN (2,4,5,6,7,8,9,10,12,16,51,53,54,55,56,57,58,59,60,61,62,63,65,66,68,71,73,74)
                                        AND (mov.numpro is NULL OR mov.numpro not in (5412))
                                GROUP BY mov.area, mov.cuenta_oc, mov.cc, mov.itm, mov.cta, mov.scta, mov.sscta, mov.numpro, mov.concepto"
                    /*, lstTmArrendadora.ToParamInValue()*/);
            }
            else
            {
                return string.Format(@"SELECT mov.area, mov.cuenta_oc, mov.cc, mov.itm, mov.cta, mov.scta, mov.sscta, mov.numpro, mov.concepto, sum(mov.monto) AS monto, max(pol.fechapol) AS fechapol, 2 AS empresa
                                    FROM sc_movpol mov
                                  INNER JOIN sc_polizas pol ON pol.year = mov.year AND pol.mes = mov.mes AND pol.tp = mov.tp AND pol.poliza = mov.poliza
                                        WHERE mov.cta BETWEEN ? AND ?
                                        AND pol.fechapol BETWEEN ? AND ?
                                        AND mov.itm IN (2,4,5,6,7,8,9,10,12,16,51,53,54,55,56,57,58,59,60,61,62,63,65,66,68,71,73,74)
                                        AND (mov.numpro is NULL OR mov.numpro not in (5412))
                                        AND (CAST(mov.area as varchar) + '-' + CAST(mov.cuenta_oc as varchar)) IN {0}
                                GROUP BY mov.area, mov.cuenta_oc, mov.cc, mov.itm, mov.cta, mov.scta, mov.sscta, mov.numpro, mov.concepto"
                    /*, lstTmArrendadora.ToParamInValue()*/
                    , busq.lstAC.ToParamInValue());
            }
        }
        string queryLstMultiPolFlujoTotalArrendadora(BusqFlujoEfectivoDTO busq)
        {
            List<int> lstTmArrendadora = new List<int>();
            using (var _ctxArrendadora = new MainContext(EmpresaEnum.Arrendadora))
            {
                lstTmArrendadora = _ctxArrendadora.tblC_FE_RelConceptoTm.GroupBy(g => g.tm).Select(s => s.Key).ToList();
            }
            return string.Format(@"SELECT mov.area, mov.cuenta_oc, mov.cc, mov.itm, dd.cta, dd.scta, dd.sscta, mov.numpro, mov.concepto, sum(mov.monto) AS monto, max(pol.fechapol) AS fechapol, 2 AS empresa
                        FROM sc_movpol dd
                        INNER JOIN sc_polizas pol ON pol.year = dd.year AND pol.mes = dd.mes AND pol.tp = dd.tp AND pol.poliza = dd.poliza
                        INNER JOIN sc_movpol mov ON mov.year = dd.year AND mov.mes = dd.mes AND mov.tp = dd.tp AND mov.poliza = dd.poliza AND mov.cc <> '*'
                        WHERE dd.cta BETWEEN ? AND ?
                        AND dd.cc = '*' 
                        AND pol.fechapol BETWEEN ? AND ?
                        AND mov.itm IN (2,4,5,6,7,8,9,10,12,16,51,53,54,55,56,57,58,59,60,61,62,63,65,66,68,71,73,74)
                        AND (mov.numpro is NULL OR mov.numpro not in (5412))
                            AND dd.cc = '*'
                        GROUP BY mov.area, mov.cuenta_oc, mov.cc, mov.itm, dd.cta, dd.scta, dd.sscta, mov.numpro, mov.concepto"
                , lstTmArrendadora.ToParamInValue());
        }
        #endregion
        #endregion
        public List<tblC_FED_CcVisto> getLstCCvistos(int anio, int semana)
        {
            try
            {
                return _context.tblC_FED_CcVisto.ToList().Where(w => w.anio == anio && w.semana == semana).ToList();
            }
            catch(Exception o_O)
            {
                return new List<tblC_FED_CcVisto>();
            }
        }
        public List<tblC_FED_RelObraUsuario> getRelObraUsuario()
        {
            return _context.tblC_FED_RelObraUsuario.Where(w => w.idUsuario == vSesiones.sesionUsuarioDTO.id).ToList();
        }
        public List<tblC_FED_CapPlaneacion> getFlujoTodoEfctivoYCostos(int anio, int noSemana)
        {
            var lstCptoDir = new List<int>() { 7, 17 };
            return _context.tblC_FED_CapPlaneacion.Where(w => w.esActivo && w.cc == "TODOS" && lstCptoDir.Contains(w.idConceptoDir) && w.anio == anio && w.semana == noSemana).ToList();
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
                if(cpto.operador.Trim() == "-" && plan.planeado > 0)
                {
                    plan.planeado *= -1;
                }
                plan.empresa = EmpresaEnum.Construplan;
            });
        }
        public List<tblC_FED_CapPlaneacion> getPlaneacion(bool esConciliado)
        {
            try
            {
                var conector = new DapperDTO
                {
                    //se eliminan registros relacionados con arrendadora para evitar duplicidad de informacion
                    consulta = esConciliado ?
                    @"SELECT 
	                    A.id, A.idConceptoDir, A.cc, A.ac, A.fecha, A.anio, A.semana,
	                    A.corte, A.esActivo, A.fechaRegistro, A.strFlujoEfectivo, A.strSaldoInicial,
	                    A.planeado - ISNULL((select sum(monto) from tblC_FED_PlaneacionDet where concepto = A.idConceptoDir AND cc = A.cc AND semana = A.semana AND año = A.anio AND estatus = 1 AND (numcte in (487, 9084) OR numprov in (4835,9867) OR descripcion like '%arrendadora%')), 0) as planeado
                    FROM tblC_FED_CapPlaneacion A
                    WHERE A.esActivo = 1 AND A.idConceptoDir > 0"
                    :
                    "SELECT * FROM tblC_FED_CapPlaneacion WHERE esActivo = 1 AND idConceptoDir > 0"
                };
                var lst = _context.Select<tblC_FED_CapPlaneacion>(conector);
                asignarFlujoPlaneacion(lst);
                
                return lst;
            }
            catch(Exception o_O)
            {
                return new List<tblC_FED_CapPlaneacion>();
            }
        }
        public IQueryable<tblC_FED_CapPlaneacion> getPlaneacionOptimizado(BusqFlujoEfectivoDTO busq)
        {
            try
            {
                var ccs = busq.lstCC.Contains("TODOS") ? "'TODOS'" : "'" + string.Join("','", busq.lstCC) + "'";
                var conector = new DapperDTO
                {
                    //se eliminan registros relacionados con arrendadora para evitar duplicidad de informacion
                    consulta = busq.esConciliado ?
                    @"SELECT 
	                    A.id, A.idConceptoDir, A.cc, A.ac, A.fecha, A.anio, A.semana,
	                    A.corte, A.esActivo, A.fechaRegistro, A.strFlujoEfectivo, A.strSaldoInicial,
	                    A.planeado - ISNULL((select sum(monto) from tblC_FED_PlaneacionDet where concepto = A.idConceptoDir AND cc = A.cc AND semana = A.semana AND año = A.anio AND estatus = 1 AND (numcte in (487, 9084) OR numprov in (4835,9867) OR descripcion like '%arrendadora%')), 0) as planeado
                    FROM tblC_FED_CapPlaneacion A
                    WHERE A.esActivo = 1 AND A.idConceptoDir > 0 and A.cc in ("+ccs+")"
                    :
                    "SELECT * FROM tblC_FED_CapPlaneacion WHERE esActivo = 1 AND idConceptoDir > 0 and cc in (" + ccs + ")"
                };
                var lst = _context.Select<tblC_FED_CapPlaneacion>(conector);
                asignarFlujoPlaneacion(lst);

                return lst.AsQueryable();
            }
            catch (Exception o_O)
            {
                return new List<tblC_FED_CapPlaneacion>().AsQueryable();
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
            catch(Exception o_O)
            {
                return new List<tblC_FED_CapPlaneacion>();
            }
        }
        public List<tblC_FED_CapPlaneacion> getPlaneacion(BusqFlujoEfectivoDTO busq)
        {
            try
            {
                if (!busq.esConciliado)
                {
                    var lst = _context.tblC_FED_CapPlaneacion
                        .Where(w => w.esActivo)
                        .Where(w => w.idConceptoDir > 0)
                        .Where(w => w.anio == busq.max.Year && w.semana == busq.max.noSemana()).ToList();
                    asignarFlujoPlaneacion(lst);
                    return lst;
                }
                else 
                {
                    var auxLstDetallesPlan = _context.tblC_FED_PlaneacionDet
                        .Where(x => x.concepto > 0 
                            && x.semana == busq.max.Year 
                            && x.semana == busq.max.noSemana() 
                            && x.estatus
                            && (x.numprov == 4835 || x.numprov == 9867)
                            && x.descripcion.Contains("ARRENDADORA")
                            && (x.numcte == 487 || x.numcte == 9084)).ToList();
                    
                    var lst = _context.tblC_FED_CapPlaneacion
                        .Where(w => w.esActivo)
                        .Where(w => w.idConceptoDir > 0)
                        .Where(w => w.anio == busq.max.Year && w.semana == busq.max.noSemana()).ToList();
                    foreach (var item in lst) 
                    {
                        var auxRestaPlaneado = auxLstDetallesPlan.Where(x => x.cc == item.cc && x.concepto == item.idConceptoDir).Sum(x => x.monto);
                        if (auxRestaPlaneado != 0) 
                        {
                            item.planeado -= auxRestaPlaneado;
                        }
                    }
                    asignarFlujoPlaneacion(lst);
                    return lst;
                }         
            }
            catch(Exception o_O)
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
            return _context.tblP_CC
                .OrderBy(o => o.ordernFlujoEfectivo).ThenBy(o => o.cc)
                .ToList()
                .Select(c => new ComboDTO()
                {
                    Value = c.cc,
                    Text = c.cc + "-" + c.descripcion,
                    Prefijo = c.fechaArranque.HasValue ? c.fechaArranque.Value.ToString("MMMM / yyyy").ToUpper() : "ENERO / 2011",
                    Orden = c.ordernFlujoEfectivo
                }).ToList();
        }
        public List<ComboDTO> getLstGrupoReserva()
        {
            return _context.tblC_FED_CatGrupoReserva
                .Where(x => x.esActivo)
                .Select(c => new ComboDTO()
                {
                    Value = c.grupo,
                    Text = c.grupo
                }).ToList();
        }
        public List<ComboDTO> getComboAreaCuenta()
        {
            try
            {
                var odbcCplan = new OdbcConsultaDTO()
                {
                    consulta = @"SELECT DISTINCT CAST(area AS Varchar)+'-'+CAST(cuenta AS Varchar) AS Value, CAST(area AS Varchar)+'-'+CAST(cuenta AS Varchar)+' '+descripcion AS Text, area, cuenta
                                            FROM si_area_cuenta 
                                            ORDER BY area, cuenta"
                };
                var lst = _contextEnkontrol.Select<Core.DTO.Principal.Generales.ComboDTO>(EnkontrolAmbienteEnum.Prod, odbcCplan);
                return lst;
            }
            catch(Exception o_O) { return new List<Core.DTO.Principal.Generales.ComboDTO>(); }
        }
        #endregion
        /// <summary>
        /// Genra un lista de IdConceptos proyectados para sumatoria deñ cuadro
        /// </summary>
        /// <param name="idConceptoDir">idConceptoDir</param>
        /// <returns>lista de IdConceptos</returns>m
        List<int> getLstIdCptoDirDesdeIdCptoDir(int idConceptoDir)
        {
            var lstConceptoDir = getCatConceptoDirActivo().Where(w => w.idPadre >= 0);
            var lstIdCpto = new List<int>();
            if(lstConceptoDir.Any(c => c.idPadre == idConceptoDir))
            {
                lstIdCpto.AddRange(lstConceptoDir.Where(c => c.idPadre <= idConceptoDir).Select(s => s.id).ToList());
            }
            else
            {
                lstIdCpto.Add(idConceptoDir);
            }
            return lstIdCpto;
        }
        public Dictionary<string, object> ObtenerInfoConceptos(string cc, int semana, int anio)
        {
            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;
                var lstConceptoDir = getCatConceptoDirActivo().Where(x => x.idPadre >= 0 && x.idPadre < 30);

                List<ConceptosPlaneacionDTO> listaConceptos = new List<ConceptosPlaneacionDTO>();
                bool editable = true;

                var existeCorte = _context.tblC_FED_CapPlaneacion.Where(x => x.cc.Equals(cc) && x.semana == semana && x.anio == anio).ToList();
                if(existeCorte != null)
                    editable = (existeCorte.Sum(x => x.corte) > 0 ? false : true);
                foreach(var item in lstConceptoDir.Where(x => x.idPadre != 0).Where(x => x.id != 9).Where(r => r.id != 14))
                {

                    ConceptosPlaneacionDTO obj = new ConceptosPlaneacionDTO();
                    decimal total = 0;
                    var detPlan = _context.tblC_FED_PlaneacionDet.Where(x => x.cc.Equals(cc) && x.semana == semana && x.año == anio && x.concepto == item.id);
                    foreach(var dplan in detPlan)
                    {
                        total += dplan.monto;
                    }
                    var padre = lstConceptoDir.FirstOrDefault(x => x.id == item.idPadre);
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
            catch(Exception e)
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

                if(listaFacturas.Count > 0)
                {
                    listaFacturas.ForEach(r =>
                    {
                        inFacturas += r + ",";
                    });
                    string queryFacturas = string.Format(@"SELECT cte.nombre as text,cte.numcte AS prefijo,fac.factura as value FROM sf_facturas fac INNER JOIN sx_clientes cte ON cte.numcte = fac.numcte where fac.factura in ({0})", inFacturas.TrimEnd(','));
                    List<ComboDTO> rawListaFacturas = (List<ComboDTO>)ContextConstruplan.Where(queryFacturas).ToObject<List<ComboDTO>>();
                    foreach(var row in rawListaFacturas.GroupBy(x => new { x.Text, x.Prefijo }).Select(r => r))
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



                foreach(var row in listaPlaneacionDet)
                {
                    PlaneacionDetDTO info = new PlaneacionDetDTO();
                    if(row.sp_gastos_provID == 0 && string.IsNullOrEmpty(row.factura) && row.cadenaProductivaID != 0)
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

                foreach(var row in rawGastosProv.GroupBy(x => x.numpro).Select(r => r))
                {
                    decimal total = 0;
                    PlaneacionDetDTO info = new PlaneacionDetDTO();
                    var listaGastosProv = rawGastosProv.Where(x => x.numpro == row.Key).Select(x => x.idSigoplan).ToList();
                    listaPlaneacionDet.Where(r => listaGastosProv.Contains(r.sp_gastos_provID) && r.estatus).ToList().ForEach(t => { total += t.monto; });
                    info.descripcion = getInfoProvedor((int)row.Key, pro);
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

                foreach(var row in listaCadena.GroupBy(x => x.numprov).Select(r => r))
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
            catch(Exception e)
            {
                LogError(0, 0, nombreControlador, "TablaDetallesConcepto", e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al buscar conceptos detalle planeacion.");
                return resultado;
            }
            return resultado;
        }
        #region Detalles de planeacion detalle para reporte de directo.
        /// <summary>
        /// Carga la información de la planeacion agrupado por los conceptos del enum tipoDetallePlaneacionEnum.cs
        /// </summary>
        /// <param name="conceptoID"></param>
        /// <param name="cc"></param>
        /// <param name="semana"></param>
        /// <param name="anio"></param>
        /// <returns></returns>
        public Dictionary<string, object> getDetallesPlaneacionPPal(int conceptoID, string cc, int semana, int anio, bool esConciliado)
        {
            try
            {
                var conceptos = getLstIdCptoDirDesdeIdCptoDir(conceptoID);
                var lstConceptos = getCatConceptoDir();
                var busqSession = (BusqFlujoEfectivoDTO)session["busqFlujoEfectivo"]; //Es la información según los parametros seleccionados en el boton de busqueda general en la interfaz
                List<tblC_FED_PlaneacionDet> listaPlaneacionDet = new List<tblC_FED_PlaneacionDet>();
                List<planeacionPrincipalDTO> planeacionPpal = new List<planeacionPrincipalDTO>();
                //variables de totales.
                decimal totalCadenaProductiva = 0;
                decimal totalNomina = 0;
                decimal totalGastosProv = 0;
                decimal totalEfectivoRecibido = 0;
                decimal totalNormal = 0;
                bool existeAgrupado = false;
                decimal totalContrato = 0;

                List<int> proveedoresArr = Enum.GetValues(typeof(numproArrendadoraEnum)).Cast<int>().ToList();
                List<int> clientesArr = Enum.GetValues(typeof(numcteArrendadoraEnum)).Cast<int>().ToList();


                if(cc != "TODOS") //Verifica si es diferente a todos los centros de costos, en caso de que no sea de todos filtrar el centro de costos y ver si se encuentra en la lista de agrupados de centros de costos.
                    existeAgrupado = true;

                //Si no se encuentra en la lista de centros de costos la busqueda procede normalmente.
                if(!existeAgrupado)
                {
                    listaPlaneacionDet = _context.tblC_FED_PlaneacionDet.Where(x => x.cc.Equals(cc) && x.semana == semana && x.año == anio && conceptos.Contains(x.concepto) && x.estatus && (esConciliado ? (!proveedoresArr.Contains(x.numprov) && !clientesArr.Contains(x.numcte)) : true)).ToList();
                    var error = listaPlaneacionDet.Where(x => proveedoresArr.Contains(x.numprov) && clientesArr.Contains(x.numcte)).ToList();
                }
                else
                {
                    var listaCCxDivision = busqSession.lstCC;
                    listaPlaneacionDet = _context.tblC_FED_PlaneacionDet.Where(x => listaCCxDivision.Contains(x.cc) && x.semana == semana && x.año == anio && conceptos.Contains(x.concepto) && x.estatus && (esConciliado ? (!proveedoresArr.Contains(x.numprov) && !clientesArr.Contains(x.numcte)) : true)).ToList();
                }
                foreach(var item in listaPlaneacionDet)
                {
                    if(item.cadenaProductivaID != 0)
                    {
                        totalCadenaProductiva += item.monto;
                    }
                    else if(item.nominaID != 0)
                    {
                        totalNomina += item.monto;
                    }
                    else if(item.sp_gastos_provID != 0)
                    {
                        totalGastosProv += item.monto;
                    }
                    else if(!string.IsNullOrEmpty(item.factura))
                    {
                        totalEfectivoRecibido += item.monto;
                    }
                    else if(item.categoriaTipo==6)
                    {
                        totalContrato += item.monto;
                    }
                    else
                    {
                        totalNormal += item.monto;
                    }
                }

                if(totalCadenaProductiva != 0)
                    planeacionPpal.Add(detalle(totalCadenaProductiva, "Cadena Productiva", conceptoID, (int)tipoDetallePlaneacionEnum.cadenaProductiva));
                if(totalNomina != 0)
                    planeacionPpal.Add(detalle(totalNomina, "Gastos Nómina", conceptoID, (int)tipoDetallePlaneacionEnum.nomina));
                if(totalGastosProv != 0)
                    planeacionPpal.Add(detalle(totalGastosProv, "Propuesta Pagó", conceptoID, (int)tipoDetallePlaneacionEnum.gastosProyecto));
                if(totalEfectivoRecibido != 0)
                    planeacionPpal.Add(detalle(totalEfectivoRecibido, "Estimaciones", conceptoID, (int)tipoDetallePlaneacionEnum.efectivoRecibido));
                if(totalNormal != 0)
                    planeacionPpal.Add(detalle(totalNormal, "Captura Manual", conceptoID, (int)tipoDetallePlaneacionEnum.manual));
                if (totalContrato != 0)
                    planeacionPpal.Add(detalle(totalContrato, "DOC X PAGAR", conceptoID, (int)tipoDetallePlaneacionEnum.contratos));

                resultado.Add("planeacionPpal", planeacionPpal);
                resultado.Add("conceptos", conceptos.FirstOrDefault());
                resultado.Add(SUCCESS, true);
            }
            catch(Exception e)
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
        public Dictionary<string, object> getSubNivelDetallePlaneacion(int conceptoID, string cc, int semana, int anio, int tipo)
        {
            try
            {
                List<PlaneacionDetDTO> listaResult = new List<PlaneacionDetDTO>();
                string query = "SELECT numcte AS value,nombre as Text FROM sx_clientes";
                var clientes = (List<ComboDTO>)ContextConstruplan.Where(query).ToObject<List<ComboDTO>>();
                var pro = polizaDAO.getProveedor();
                string inFacturas = "";
                List<tblC_FED_PlaneacionDet> listaPlaneacionDet = new List<tblC_FED_PlaneacionDet>();
                bool existeAgrupado = false;
                var busqSession = (BusqFlujoEfectivoDTO)session["busqFlujoEfectivo"];
                var conceptos = getLstIdCptoDirDesdeIdCptoDir(conceptoID);
                if(cc != "TODOS") //Verifica si es diferente a todos los centros de costos, en caso de que no sea de todos filtrar el centro de costos y ver si se encuentra en la lista de agrupados de centros de costos.
                    existeAgrupado = true;

                //Si no se encuentra en la lista de centros de costos la busqueda procede normalmente.
                if(!existeAgrupado)
                {
                    listaPlaneacionDet = _context.tblC_FED_PlaneacionDet.Where(x => x.cc.Equals(cc) && x.semana == semana && x.año == anio && conceptos.Contains(x.concepto) && x.estatus).ToList();
                }
                else
                {
                    var listaCCxDivision = busqSession.lstCC.ToList();
                    listaPlaneacionDet = _context.tblC_FED_PlaneacionDet.Where(x => listaCCxDivision.Contains(x.cc) && x.semana == semana && x.año == anio && conceptos.Contains(x.concepto) && x.estatus).ToList();
                }
                var lstCC = polizaDAO.getComboCentroCostos();
                var lstCCValue = lstCC.Select(s => s.Value);
                var lstGemId = listaPlaneacionDet.Where(w => w.idDetProyGemelo > 0).Select(s => s.idDetProyGemelo).ToList();
                var lstGem = _context.tblC_FED_PlaneacionDet.ToList().Where(w => lstGemId.Contains(w.id)).ToList();
                listaPlaneacionDet.ForEach(plan =>
                {
                    var gem = lstGem.FirstOrDefault(g => g.id == plan.idDetProyGemelo);
                    if(gem != null)
                    {
                        if(plan.cc == "TODOS")
                        {
                            plan.cc = gem.cc;
                        }
                    }
                    if(lstCCValue.Contains(plan.cc))
                    {
                        plan.cc = lstCC.FirstOrDefault(ac => ac.Value == plan.cc).Text;
                    }
                });
                switch(tipo)
                {
                    case (int)tipoDetallePlaneacionEnum.cadenaProductiva:
                        {
                            var listaCadena = listaPlaneacionDet.Where(r => r.cadenaProductivaID != 0 && r.estatus).ToList();
                            foreach(var row in listaCadena.GroupBy(x => x.numprov).Select(r => r))
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
                        }
                        break;
                    case (int)tipoDetallePlaneacionEnum.efectivoRecibido:
                        {
                            var listaFacturas = listaPlaneacionDet.Where(x => !string.IsNullOrEmpty(x.factura) && x.estatus && x.cadenaProductivaID == 0 && x.nominaID == 0 && x.sp_gastos_provID == 0).Select(x => x.factura).ToList();
                            if(listaFacturas.Count > 0)
                            {
                                listaFacturas.ForEach(r =>
                                {
                                    inFacturas += r + ",";
                                });
                                string queryFacturas = string.Format(@"SELECT cte.nombre as text,cte.numcte AS prefijo,fac.factura as value FROM sf_facturas fac INNER JOIN sx_clientes cte ON cte.numcte = fac.numcte where fac.factura in ({0})", inFacturas.TrimEnd(','));
                                List<ComboDTO> rawListaFacturas = (List<ComboDTO>)ContextConstruplan.Where(queryFacturas).ToObject<List<ComboDTO>>();
                                foreach(var row in rawListaFacturas.GroupBy(x => new { x.Text, x.Prefijo }).Select(r => r))
                                {
                                    PlaneacionDetDTO info = new PlaneacionDetDTO();
                                    decimal total = 0;
                                    var listaFacturasCliente = rawListaFacturas.Where(x => x.Prefijo == row.Key.Prefijo).Select(x => x.Value).ToList();
                                    listaPlaneacionDet.Where(r => listaFacturasCliente.Contains(r.factura) && r.estatus).ToList().ForEach(t => { total += t.monto; });
                                    info.descripcion = row.Key.Prefijo + " - " + row.Key.Text;
                                    info.numcte = Convert.ToInt32(row.Key.Prefijo);
                                    info.numProv = 0;
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
                        }
                        break;
                    case (int)tipoDetallePlaneacionEnum.gastosProyecto:
                        {
                            var listaGastos = listaPlaneacionDet.Where(r => r.sp_gastos_provID != 0 && r.estatus).Select(x => x.sp_gastos_provID).ToList();
                            var rawGastosProv = _context.tblC_sp_gastos_prov.Where(r => listaGastos.Contains(r.idSigoplan)).Select(x => new
                            {
                                x.idSigoplan,
                                x.numpro
                            }).ToList();

                            foreach(var row in rawGastosProv.GroupBy(x => x.numpro).Select(r => r).OrderBy(o => o.Key))
                            {
                                decimal total = 0;
                                PlaneacionDetDTO info = new PlaneacionDetDTO();
                                var listaGastosProv = rawGastosProv.Where(x => x.numpro == row.Key).Select(x => x.idSigoplan).ToList();
                                listaPlaneacionDet.Where(r => listaGastosProv.Contains(r.sp_gastos_provID) && r.estatus).ToList().ForEach(t => { total += t.monto; });
                                info.descripcion = getInfoProvedor((int)row.Key, pro);
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
                        }
                        break;
                    case (int)tipoDetallePlaneacionEnum.manual:
                        {
                            foreach(var row in listaPlaneacionDet)
                            {
                                PlaneacionDetDTO info = new PlaneacionDetDTO();
                                if(row.nominaID == 0 && row.cadenaProductivaID == 0 && row.sp_gastos_provID == 0 && string.IsNullOrEmpty(row.factura))
                                {
                                    info.descripcion = row.descripcion;
                                    info.monto = row.monto;
                                    info.numcte = 0;
                                    info.numProv = 0;
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
                            var listaNomina = listaPlaneacionDet.Where(r => r.nominaID != 0 && r.estatus).Select(x => x.nominaID).ToList();

                            foreach(var row in listaPlaneacionDet)
                            {
                                PlaneacionDetDTO info = new PlaneacionDetDTO();
                                if(row.nominaID != 0)
                                {
                                    info.descripcion = row.descripcion;
                                    info.monto = row.monto;
                                    info.numcte = 0;
                                    info.numProv = 0;
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
                    case (int)tipoDetallePlaneacionEnum.contratos:
                        {
                            //var listaNomina = listaPlaneacionDet.Where(r => r.categoriaTipo == 6 && r.estatus).ToList();
                         //   var lstDet = _context.tblC_FED_PlaneacionDet.ToList().Where(w => (esCC ? w.cc != "TODOS" : w.ac != "TODOS")).ToList();
                            foreach (var row in listaPlaneacionDet)
                            {
                                PlaneacionDetDTO info = new PlaneacionDetDTO();

                                info.descripcion = row.descripcion;
                                info.monto = row.monto;
                                info.numcte = 0;
                                info.numProv = 0;
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
                listaResult.ForEach(x => { x.tipo = tipo; x.conceptoID = conceptoID; suma += x.monto; });
                resultado.Add("suma", suma);
                resultado.Add(SUCCESS, true);
                resultado.Add("planeacionDetalle", listaResult);

            }
            catch(Exception e)
            {
                LogError(0, 0, nombreControlador, "TablaDetallesConcepto", e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al buscar conceptos detalle planeacion.");
                return resultado;
            }
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
        public Dictionary<string, object> getSubDetalle(string cc, int semana, int anio, int conceptoID, int numProv, int numcte)
        {
            try
            {
                List<tblC_FED_PlaneacionDet> listaPlanTemp = new List<tblC_FED_PlaneacionDet>();
                List<tblC_FED_PlaneacionDet> listaPlaneacionDet = new List<tblC_FED_PlaneacionDet>();
                bool existeAgrupado = false;
                var busqSession = (BusqFlujoEfectivoDTO)session["busqFlujoEfectivo"];
                var conceptos = getLstIdCptoDirDesdeIdCptoDir(conceptoID);
                if(cc != "TODOS") //Verifica si es diferente a todos los centros de costos, en caso de que no sea de todos filtrar el centro de costos y ver si se encuentra en la lista de agrupados de centros de costos.
                    existeAgrupado = true;

                //Si no se encuentra en la lista de centros de costos la busqueda procede normalmente.
                if(!existeAgrupado)
                {
                    listaPlaneacionDet = _context.tblC_FED_PlaneacionDet.Where(x => x.cc.Equals(cc) && x.semana == semana && x.año == anio && conceptos.Contains(x.concepto) && x.estatus).ToList();
                }
                else
                {
                    var listaCCxDivision = busqSession.lstCC.ToList();
                    listaPlaneacionDet = _context.tblC_FED_PlaneacionDet.Where(x => listaCCxDivision.Contains(x.cc) && x.semana == semana && x.año == anio && conceptos.Contains(x.concepto) && x.estatus).ToList();
                }

                if(cc == "TODOS")
                {
                    listaPlanTemp = _context.tblC_FED_PlaneacionDet.Where(x => x.semana == semana && x.año == anio && x.cc != cc && conceptos.Contains(x.concepto)).ToList();
                    foreach(var item in listaPlaneacionDet)
                    {
                        if(item.sp_gastos_provID != 0)
                            item.cc = listaPlanTemp.FirstOrDefault(x => x.sp_gastos_provID == item.sp_gastos_provID).cc;
                        if(item.cadenaProductivaID != 0)
                            item.cc = listaPlanTemp.FirstOrDefault(x => x.cadenaProductivaID == item.cadenaProductivaID).cc;
                        if(item.cadenaProductivaID == 0 && item.sp_gastos_provID == 0 && !string.IsNullOrEmpty(item.factura))
                            item.cc = listaPlanTemp.FirstOrDefault(x => x.factura == item.factura).cc;
                    }
                }

                if(numcte != 0)
                {
                    resultado.Add("planeacionDetalle", listaPlaneacionDet.Where(r => r.numcte == numcte));
                }
                else if(numProv != 0)
                {
                    resultado.Add("planeacionDetalle", listaPlaneacionDet.Where(r => r.numprov == numProv));
                }
                resultado.Add(SUCCESS, true);
            }
            catch(Exception e)
            {

                LogError(0, 0, nombreControlador, "obtenerDealleDescripcionplanacionConcepto", e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al obtener los detalles del concepto...");
                return resultado;
            }
            return resultado;
        }
        #endregion
        #region Planeacion Detalle
        public Dictionary<string, object> getDetallePlaneacion(int concepto, string cc, int semana, int anio, int tipo)
        {
            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;
                var lstIdCpto = getLstIdCptoDirDesdeIdCptoDir(concepto);
                var listaConceptosDetalle = _context.tblC_FED_PlaneacionDet.Where(x => x.cc.Equals(cc) && x.semana == semana && x.año == anio && lstIdCpto.Contains(x.concepto) && x.estatus).ToList();

                if(cc == "TODOS")
                {
                    listaConceptosDetalle.ForEach(dato =>
                    {
                        if(dato.sp_gastos_provID != 0)
                        {
                            var temporal = _context.tblC_FED_PlaneacionDet.FirstOrDefault(x => x.sp_gastos_provID == dato.sp_gastos_provID && x.estatus);
                            dato.cc = temporal.cc;
                        }
                        else if(!string.IsNullOrEmpty(dato.factura))
                        {
                            if (dato.numprov != 0)
                            {
                                var temporal = _context.tblC_FED_PlaneacionDet.FirstOrDefault(x => x.factura == dato.factura && x.numprov == dato.numprov && x.estatus);
                                dato.cc = temporal.cc;
                            }
                            else if (dato.numcte != 0)
                            {
                                var temporal = _context.tblC_FED_PlaneacionDet.FirstOrDefault(x => x.factura == dato.factura && x.numcte == dato.numcte && x.estatus);
                                dato.cc = temporal.cc;
                            }
                        }
                        else if(dato.nominaID != 0)
                        {
                            var temporal = _context.tblC_FED_PlaneacionDet.FirstOrDefault(x => x.nominaID == dato.nominaID && x.estatus);
                            dato.cc = temporal.cc;
                        }
                        else if(dato.cadenaProductivaID != 0)
                        {
                            var temporal = _context.tblC_FED_PlaneacionDet.FirstOrDefault(x => x.cadenaProductivaID == dato.cadenaProductivaID && x.estatus);
                            dato.cc = temporal.cc;
                        }
                    });
                }
                switch(tipo)
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
                    ccDetProyGemelo = lstGemelos.Any(w => w.id == det.idDetProyGemelo) ? lstGemelos.FirstOrDefault(w => w.id == det.idDetProyGemelo).cc : "N/A",
                    tipo = asignarTipoDetalle(det),
                }).ToList();
                resultado.Add("listaConceptosDetalle", lstRes);
                resultado.Add(SUCCESS, true);
                return resultado;
            }
            catch(Exception e)
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
            if(plan.cadenaProductivaID == 0 && string.IsNullOrEmpty(plan.factura) && plan.sp_gastos_provID == 0 && plan.nominaID == 0)
            {
                tipo = (int)tipoDetallePlaneacionEnum.manual;
            }
            else if(plan.cadenaProductivaID != 0)
            {
                tipo = (int)tipoDetallePlaneacionEnum.cadenaProductiva;
            }
            else if(!string.IsNullOrEmpty(plan.factura))
            {
                tipo = (int)tipoDetallePlaneacionEnum.efectivoRecibido;
            }
            else if(plan.nominaID != 0)
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
                var lstDivAdmin = new List<int>()
                {
                    (int)TipoCCEnum.Administración,
                    (int)TipoCCEnum.GastosFininacierosYOtros
                };
                var pro = polizaDAO.getProveedor();
                var ccAdministracion = polizaDAO.lstObra().Where(x => !lstDivAdmin.Contains(x.Prefijo.ParseInt())).Select(x => x.Value).ToList();
                var tms = getRelConceptoTm().Where(x => x.idConcepto == 6).Select(x => x.tm);

                List<tblC_FED_PlaneacionDet> info = new List<tblC_FED_PlaneacionDet>();
                List<int> gastosProv = new List<int>();

                if (vSesiones.sesionEmpresaActual != (int)EmpresaEnum.Peru)
                {
                    info = _context.tblC_sp_gastos_prov
                    .Where(r => (cc == "TODOS" ? ccAdministracion.Contains(r.cc) : r.cc == cc) && r.fechaPropuesta != null && r.total > 0)

                    .Where(r => r.fechaPropuesta >= fechaInicio)
                    .Where(r => r.fechaPropuesta <= fechaFin).ToList()
                    .Select(x => new tblC_FED_PlaneacionDet
                    {
                        id = 0,
                        concepto = 6,
                        descripcion = getInfoProvedor((int)x.numpro, pro),
                        estatus = true,
                        fechaCaptura = DateTime.Now,
                        año = fechaFin.Year,
                        cc = x.cc,
                        monto = -Convert.ToDecimal(x.tipocambio != 0 ? x.tipocambio * x.total : x.total),
                        semana = semana,
                        sp_gastos_provID = x.idSigoplan,
                        factura = x.factura,
                        numprov = (int)x.numpro,
                        numcte = 0,
                        fechaFactura = x.fecha.ToShortDateString()
                    }).ToList();
                    gastosProv = _context.tblC_FED_PlaneacionDet.Where(x => x.cc == cc && x.año == anio && x.semana == semana && x.sp_gastos_provID != 0 && x.estatus).Select(r => r.sp_gastos_provID).ToList();
                }
                else 
                {
                    List<tblP_CC> ccs = new List<tblP_CC>();
                    var fecha_especial = new DateTime(2023, 03, 30);
                    using (var ctxConstruplan = new MainContext(EmpresaEnum.Construplan))
                    {
                        ccs = ctxConstruplan.tblP_CC.ToList();
                    }

                    var tipoCambio = _context.tblC_SC_TipoCambio.Where(x => x.Moneda == 3 && x.esActivo).ToList();
                    
                    info = _context.tblC_sp_gastos_prov
                    .Where(r => (cc == "TODOS" ? ccAdministracion.Contains(r.cc) : r.cc == cc) && r.fechaPropuesta != null && r.total > 0)
                    .Where(r => r.fechaPropuesta >= fecha_especial).ToList()
                    //.Where(r => r.fechaPropuesta <= fechaFin).ToList()
                    .Select(x =>
                    {
                        var _proveedor = pro.FirstOrDefault(y => y.numproPeru == x.numproPeru);
                        var _cc = ccs.FirstOrDefault(y => y.ccRH == x.cc);
                        var _tc = tipoCambio.FirstOrDefault(y => y.Fecha == x.fecha);
                        return new tblC_FED_PlaneacionDet
                        {
                            id = 0,
                            concepto = 6,
                            descripcion = _proveedor == null ? x.numproPeru : _proveedor.numproPeru + "-" + _proveedor.nombre,
                            estatus = true,
                            fechaCaptura = DateTime.Now,
                            año = fechaFin.Year,
                            cc = _cc == null ? x.cc : _cc.cc,
                            monto = (-Convert.ToDecimal(x.tipocambio != 0 ? x.tipocambio * x.total : x.total)) * (_tc == null ? 4.991530M : _tc.TipoCambio),
                            semana = semana,
                            sp_gastos_provID = x.idSigoplan,
                            factura = x.factura,
                            numprov = Int32.Parse(x.numproPeru.Substring(2,9)),
                            numcte = 0,
                            fechaFactura = x.fecha.ToShortDateString()
                        };
                    }).ToList();
                    gastosProv = _context.tblC_FED_PlaneacionDet.Where(x => x.cc == cc && x.año == anio && x.semana == semana && x.sp_gastos_provID != 0 && x.estatus).Select(r => r.sp_gastos_provID).ToList();

                }

                resultado.Add("listaPlaneacion", info.Where(x => !gastosProv.Contains(x.sp_gastos_provID) && x.monto != 0).ToList());
                resultado.Add(SUCCESS, true);
                return resultado;
            }
            catch(Exception e)
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
                var economicos = _context.tblP_CC.Where(x => x.estatus && x.cc != "0").Select(x => new ComboDTO { Value = x.cc, Text = x.descripcion }).ToList();
                var info = _context.tblC_NominaResumen.ToList()
                    .Where(r => r.tipoCuenta != (int)tipoCuentaNominaEnum.Arrendadora)
                     .Where(r => (cc == "TODOS" ? true : r.cc == cc))
                     .Where(r => lstPeriodo.Exists(p => r.fecha_final >= p.fecha_inicial && r.fecha_final <= p.fecha_final && r.tipoNomina == p.tipo_nomina)).ToList()
                    .Select(x => new tblC_FED_PlaneacionDet
                    {
                        id = 0,
                        concepto = 7,
                        descripcion = string.Format("CARGO {0} - {1}", ((tipoNominaPropuestaEnum)x.tipoNomina).GetDescription(), getDescripcionCC(x.cc, economicos)),
                        estatus = true,
                        fechaCaptura = DateTime.Now,
                        año = anio,
                        cc = x.cc,
                        monto = -x.total,
                        semana = semana,
                        sp_gastos_provID = 0,
                        nominaID = x.id,
                        cadenaProductivaID = 0,
                        numcte = 0,
                        numprov = 0,
                        fechaFactura = ""
                    }).ToList();
                var gastosNomina = _context.tblC_FED_PlaneacionDet.Where(x => x.cc == cc && x.año == anio && x.semana == semana && x.nominaID != 0 && x.estatus).Select(r => r.nominaID).ToList();
                resultado.Add("listaPlaneacion", info.Where(x => !gastosNomina.Contains(x.nominaID)).OrderBy(x => x.cc).ThenBy(x => x.descripcion).ToList());
                resultado.Add(SUCCESS, true);
            }
            catch(Exception e)
            {
                LogError(0, 0, nombreControlador, "CargarNominas", e, AccionEnum.ACTUALIZAR, 0, null);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al guardar los operadores de detalle planeacion.");
            }
            return resultado;
        }
        //Carga lista de cadena productiva.
        public Dictionary<string, object> getCadenasProductivas(DateTime fechaInicio, DateTime fechaFin, string cc, int semana, int anio)
        {
            try
            {
                var info = _context.tblC_CadenaProductiva
                    .Where(r => (cc == "TODOS" ? true : r.centro_costos == cc) && r.saldoFactura > 0)
                    .Where(r => r.fechaVencimiento >= fechaInicio)
                    .Where(r => r.fechaVencimiento <= fechaFin).ToList()
                    .Select(x => new tblC_FED_PlaneacionDet
                    {
                        id = 0,
                        concepto = 6,
                        descripcion = x.proveedor,
                        estatus = true,
                        fechaCaptura = DateTime.Now,
                        año = fechaFin.Year,
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
                var listaCadenas = _context.tblC_FED_PlaneacionDet.Where(x => x.cc == cc && x.año == anio && x.semana == semana && x.cadenaProductivaID != null && x.estatus).Select(r => r.cadenaProductivaID).ToList();
                resultado.Add("listaPlaneacion", info.Where(r => !listaCadenas.Contains(r.cadenaProductivaID)));
                resultado.Add(SUCCESS, true);
            }
            catch(Exception e)
            {

                LogError(0, 0, nombreControlador, "CadenasProductivas", e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al buscar los gatos provedores...");
                return resultado;
            }
            return resultado;
        }
        //Cargar lista de Efectivo Recibido
        public Dictionary<string, object> getEfectivoRecibido(DateTime fechaInicio, DateTime fechaFin, string cc, int semana, int anio)
        {
            try
            {
                string query = string.Format(@"SELECT  numcte,factura,cc,total,nombre ,descripcion,DATEFORMAT( CAST ( fechafac AS DATE ), 'dd/mm/yyyy' ) as fechafac FROM (SELECT cte.nombre, mov.numcte, mov.factura, mov.cc, MAX(mov.fechavenc) AS fechavenc,fac.fecha as fechafac,
                                                SUM(CASE WHEN det.insumo IN (9010001 ,9010003 ,9010006 ,9010007) AND mov.fechavenc <= '{0}' THEN mov.total*mov.tipocambio ELSE 0 END) AS total,
                                                MAX(det.linea) AS linea,('ESTMIACION DE OBRA '+cte.nombre) as descripcion
                                                    FROM sx_movcltes mov
                                                    INNER JOIN sf_facturas fac ON fac.factura = mov.factura AND fac.numcte = mov.numcte AND fac.cc = mov.cc AND mov.referenciaoc = fac.numero_nc
                                                    INNER JOIN sf_facturas_det det ON  det.factura = fac.factura AND fac.numero_nc = det.numero_nc AND det.cia_sucursal = fac.cia_sucursal
                                                    INNER JOIN sx_clientes cte ON cte.numcte = mov.numcte
                                                    GROUP BY  mov.numcte ,mov.factura ,mov.cc, cte.nombre,fac.fecha
                                                    ORDER BY mov.cc ,mov.numcte ,mov.factura) x
                                                WHERE (x.total) NOT BETWEEN -1 AND 1 AND fechavenc <= '{0}' AND x.total >0 {1}
                                            ORDER BY nombre,cc", fechaFin.ToString("yyyyMMdd"), cc != "TODOS" ? "AND CC='" + cc + "'" : "");
                var listaFacturas = _context.tblC_FED_PlaneacionDet.Where(x => x.cc == cc && x.año == anio && x.semana == semana && x.factura != null && x.estatus).Select(r => r.factura).ToList();
                var efectivoRecibido = (List<efectivoRecibidoDTO>)ContextConstruplan.Where(query).ToObject<List<efectivoRecibidoDTO>>();
                resultado.Add("listaPlaneacion", efectivoRecibido.Where(f => !listaFacturas.Contains(f.factura)).OrderBy(x => x.cc).ThenBy(x => x.nombre).ToList());
                resultado.Add(SUCCESS, true);
                return resultado;
            }
            catch(Exception e)
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
                var lstDivAdmin = new List<int>()
                {
                    (int)TipoCCEnum.Administración,
                    (int)TipoCCEnum.GastosFininacierosYOtros
                };
                var pro = polizaDAO.getProveedor();
                var _ccAdministracion = polizaDAO.lstObra().ToList();
                var ccAdministracion = _ccAdministracion.Where(x => lstDivAdmin.Contains(x.Prefijo.ParseInt())).Select(x => x.Value).ToList();
                var tms = getRelConceptoTm().Where(x => x.idConcepto == 6).Select(x => x.tm);                
                var info = _context.tblC_sp_gastos_prov
                    .Where(r => (cc == "TODOS" ? ccAdministracion.Contains(r.cc) : r.cc == cc) && r.fechaPropuesta != null)

                    .Where(r => r.fechaPropuesta >= fechaInicio)
                    .Where(r => r.fechaPropuesta <= fechaFin).ToList()
                    .Select(x => new tblC_FED_PlaneacionDet
                    {
                        id = 0,
                        concepto = 7,
                        descripcion = getInfoProvedor((int)x.numpro, pro),
                        estatus = true,
                        fechaCaptura = DateTime.Now,
                        año = fechaFin.Year,
                        cc = x.cc,
                        monto = -(Convert.ToDecimal(x.tipocambio) != 0 ? x.total * Convert.ToDecimal(x.tipocambio) : x.total),
                        semana = semana,
                        sp_gastos_provID = x.idSigoplan,
                        numprov = (int)x.numpro,
                        numcte = 0,
                        fechaFactura = x.fecha.ToShortDateString(),
                        factura = x.factura

                    }).ToList();

                if((int) vSesiones.sesionEmpresaActual == (int) EmpresaEnum.Colombia)
                {
                    var centrosCosto = _context.tblP_CC.ToList();
                    foreach (var item in info) 
                    {
                        var _centroCosto = centrosCosto.FirstOrDefault(x => x.cc == item.cc);
                        var tipocambio = _context.tblC_SC_TipoCambio.FirstOrDefault(x => x.Fecha == DateTime.Today);
                        if (tipocambio == null) tipocambio = _context.tblC_SC_TipoCambio.OrderByDescending(x => x.Fecha).FirstOrDefault();
                        if (_centroCosto != null) 
                        {
                            item.cc = _centroCosto.ccRH == null ? item.cc : _centroCosto.ccRH;
                            item.monto = item.monto / (tipocambio.TipoCambio != 0 ? tipocambio.TipoCambio : 1);
                        }
                    }
                }
                var gastosProv = _context.tblC_FED_PlaneacionDet.Where(x => x.cc == cc && x.año == anio && x.semana == semana && x.sp_gastos_provID != 0 && x.estatus).Select(r => r.sp_gastos_provID).ToList();

                resultado.Add("listaPlaneacion", info.Where(x => !gastosProv.Contains(x.sp_gastos_provID)).OrderBy(x => x.cc).ThenBy(x => x.descripcion).ToList());
                resultado.Add(SUCCESS, true);
                return resultado;
            }
            catch(Exception e)
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
                            && x.numcte == _primero.numcte && x.numprov == _primero.numprov && x.fechaFactura == _primero.fechaFactura && x.cc != _primero.cc);
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

                    var primeroBD = _listBD.FirstOrDefault(x => x.cc != "TODOS");
                    var primeroListaOriginal = lst.FirstOrDefault();

                    if ((primeroBD.concepto == 6 || primeroBD.concepto == 7) && primeroBD.sp_gastos_provID != 0 && !primeroListaOriginal.estatus)
                    {
                        var _facturasBorrarENKONTROL = _listBD.Where(x => x.cc != "TODOS").ToList();
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

                int usuarioCreadorID = vSesiones.sesionUsuarioDTO.id;

                lst.ToList().ForEach(nuevo =>
                {
                    if(nuevo.estatus)
                    {
                        if(nuevo.id > 0)
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
                        if(lst.Count > 1 && lst.LastOrDefault().id == 0)
                        {
                            var maxId = nuevo.id;
                            lst.LastOrDefault().idDetProyGemelo = maxId;
                            lst.FirstOrDefault().idDetProyGemelo = maxId + 1;
                        }
                        var fed_capPlaneacion = _context.tblC_FED_CapPlaneacion.FirstOrDefault(x => x.idConceptoDir == nuevo.concepto && x.anio == nuevo.año && x.semana == nuevo.semana && x.esActivo && x.cc == nuevo.cc);
                        if(fed_capPlaneacion != null)
                        {
                            var lstDet = _context.tblC_FED_PlaneacionDet.ToList().Where(w => w.estatus && w.concepto == nuevo.concepto && w.año == nuevo.año && w.semana == nuevo.semana && w.cc == nuevo.cc);
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
                            _context.tblC_FED_PlaneacionDet.Remove(actualizacionBanco);
                            var lstDet = _context.tblC_FED_PlaneacionDet.ToList().Where(w => w.estatus && w.concepto == nuevo.concepto && w.año == nuevo.año && w.semana == nuevo.semana && w.cc == nuevo.cc).ToList();
                            var fed_capPlaneacion = _context.tblC_FED_CapPlaneacion.FirstOrDefault(x => x.idConceptoDir == actualizacionBanco.concepto && x.anio == actualizacionBanco.año && x.semana == actualizacionBanco.semana && x.esActivo && x.cc == actualizacionBanco.cc);
                            fed_capPlaneacion.planeado = lstDet.Sum(s => s.monto);
                            _context.SaveChanges();
                            if (!string.IsNullOrEmpty(actualizacionBanco.factura))
                            {
                                if (actualizacionBanco.numprov != 0)
                                {

                                    List<tblC_FED_PlaneacionDet> temporales = _context.tblC_FED_PlaneacionDet.Where(x => x.factura == actualizacionBanco.factura && x.numprov == actualizacionBanco.numprov).ToList();
                                    temporales.ForEach(r => { r.estatus = false; r.monto = 0; });
                                    _context.tblC_FED_PlaneacionDet.RemoveRange(temporales);
                                    _context.SaveChanges();
                                }
                                else if (actualizacionBanco.numcte != 0)
                                {
                                    List<tblC_FED_PlaneacionDet> temporales = _context.tblC_FED_PlaneacionDet.Where(x => x.factura == actualizacionBanco.factura && x.numcte == actualizacionBanco.numcte).ToList();
                                    temporales.ForEach(r => { r.estatus = false; r.monto = 0; });
                                    _context.tblC_FED_PlaneacionDet.RemoveRange(temporales);
                                    _context.SaveChanges();
                                }

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
                    ccDetProyGemelo = lst.Any(w => w.id == det.idDetProyGemelo) ? lst.FirstOrDefault(w => w.id == det.idDetProyGemelo).cc : "N/A",
                    tipo = asignarTipoDetalle(det),
                }).ToList());
                _context.SaveChanges();


                //dbTransaction.Commit();
                resultado.Add(SUCCESS, true);
            }
            catch(Exception e)
            {
                //dbTransaction.Rollback();
                LogError(0, 0, nombreControlador, "GuardarDetallePlaneacion", e, AccionEnum.ACTUALIZAR, 0, lst.FirstOrDefault());
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al guardar los operadores de detalle planeacion." + e.Message);
            }
            return resultado;
            //}
        }
        public Dictionary<string, object> saveDetallesMasivos(List<tblC_FED_PlaneacionDet> listaPlaneacionDet, int idConceptoDir, int anio, int semana, string cc)
        {
            //using(var dbTransaction = _context.Database.BeginTransaction())
            try
            {
                resultado.Clear();
                List<tblC_FED_PlaneacionDet> tempPlaneacion = new List<tblC_FED_PlaneacionDet>();
                decimal planeado = 0;
                int usuarioCreadorID = vSesiones.sesionUsuarioDTO.id;
                List<string> lstCC = new List<string>();

                switch (vSesiones.sesionEmpresaActual) 
                {
                    case (int)EmpresaEnum.Colombia:
                        using (var _ctxConstruplan = new MainContext(EmpresaEnum.Construplan))
                        {
                            listaPlaneacionDet.ForEach(x =>
                            {
                                x.usuarioCaptura = usuarioCreadorID;
                                x.fechaCaptura = DateTime.Now;
                                x.semana = semana;
                                x.año = anio;
                                x.concepto = idConceptoDir;
                                x.estatus = true;
                            });

                            _ctxConstruplan.tblC_FED_PlaneacionDet.AddRange(listaPlaneacionDet);
                            _ctxConstruplan.SaveChanges();
                            foreach (var temp in listaPlaneacionDet)
                            {
                                tblC_FED_PlaneacionDet tempNe = new tblC_FED_PlaneacionDet();

                                tempNe.año = temp.año;
                                tempNe.cc = "TODOS";
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

                            _ctxConstruplan.tblC_FED_PlaneacionDet.AddRange(tempPlaneacion);
                            _ctxConstruplan.SaveChanges();
                            lstCC = listaPlaneacionDet.GroupBy(x => x.cc).Select(r => r.Key).ToList();
                            lstCC.Add("TODOS");
                            foreach (var itemCC in lstCC)
                            {

                                decimal motoTotalPlaneado = 0;
                                var listaTotales = _ctxConstruplan.tblC_FED_PlaneacionDet.Where(x => x.concepto == idConceptoDir && x.año == anio && x.semana == semana && itemCC == x.cc && x.estatus).ToList();
                                motoTotalPlaneado = listaTotales.Sum(s => s.monto);
                                var fed_capPlaneacionCC = _ctxConstruplan.tblC_FED_CapPlaneacion.FirstOrDefault(x => x.idConceptoDir == idConceptoDir && x.anio == anio && x.semana == semana && x.esActivo && x.cc == itemCC);

                                if (fed_capPlaneacionCC == null)
                                {
                                    var fecha = Infrastructure.Utils.DatetimeUtils.primerDiaSemana(anio, semana);
                                    fecha = fecha.Siguiente(DayOfWeek.Saturday);
                                    tblC_FED_CapPlaneacion nuevoRegistro = new tblC_FED_CapPlaneacion();
                                    nuevoRegistro.id = 0;
                                    nuevoRegistro.anio = anio;
                                    nuevoRegistro.cc = itemCC;
                                    nuevoRegistro.corte = 0;
                                    nuevoRegistro.esActivo = true;
                                    nuevoRegistro.fecha = fecha;
                                    nuevoRegistro.fechaRegistro = DateTime.Now;
                                    nuevoRegistro.flujoTotal = 0;
                                    nuevoRegistro.idConceptoDir = idConceptoDir;
                                    nuevoRegistro.planeado = motoTotalPlaneado;
                                    nuevoRegistro.semana = semana;
                                    nuevoRegistro.strFlujoEfectivo = "0";
                                    _ctxConstruplan.tblC_FED_CapPlaneacion.Add(nuevoRegistro);
                                    _ctxConstruplan.SaveChanges();
                                }
                                else
                                {
                                    fed_capPlaneacionCC.planeado = motoTotalPlaneado;
                                    _ctxConstruplan.SaveChanges();
                                }
                                var sumaPlaneacion = _ctxConstruplan.tblC_FED_PlaneacionDet.Where(x => x.concepto == idConceptoDir && x.año == anio && x.semana == semana && itemCC == x.cc && x.estatus).ToList();
                                planeado = sumaPlaneacion.Sum(s => s.monto);
                                var fed_capPlaneacion = _ctxConstruplan.tblC_FED_CapPlaneacion.FirstOrDefault(x => x.idConceptoDir == idConceptoDir && x.anio == anio && x.semana == semana && x.esActivo && x.cc == itemCC);
                                if (fed_capPlaneacion == null)
                                {
                                    tblC_FED_CapPlaneacion nuevoRegistro = new tblC_FED_CapPlaneacion();
                                    nuevoRegistro.id = 0;
                                    nuevoRegistro.anio = anio;
                                    nuevoRegistro.cc = cc;
                                    nuevoRegistro.corte = 0;
                                    nuevoRegistro.esActivo = true;
                                    nuevoRegistro.fecha = DateTime.Now;
                                    nuevoRegistro.fechaRegistro = DateTime.Now;
                                    nuevoRegistro.flujoTotal = 0;
                                    nuevoRegistro.idConceptoDir = idConceptoDir;
                                    nuevoRegistro.planeado = planeado;
                                    nuevoRegistro.semana = semana;
                                    nuevoRegistro.strFlujoEfectivo = "0";
                                    _ctxConstruplan.tblC_FED_CapPlaneacion.Add(nuevoRegistro);
                                    _ctxConstruplan.SaveChanges();
                                }
                                else
                                {
                                    fed_capPlaneacion.planeado = planeado;
                                    _ctxConstruplan.SaveChanges();
                                }
                            }
                            //dbTransaction.Commit();
                            resultado.Add(SUCCESS, true);
                            resultado.Add("concepto", idConceptoDir);
                            resultado.Add("cc", cc);
                            resultado.Add("semana", semana);
                        }
                        break;
                    case (int)EmpresaEnum.Peru:
                        using (var _ctxConstruplan = new MainContext(EmpresaEnum.Construplan))
                        {
                            var lstCCSIGOPLAN = _ctxConstruplan.tblP_CC.ToList();

                            listaPlaneacionDet.ForEach(x =>
                            {
                                var ccEnk = lstCCSIGOPLAN.FirstOrDefault(y => y.ccRH == x.cc);
                                x.cc = ccEnk == null ? x.cc : ccEnk.ccRH;
                                x.usuarioCaptura = usuarioCreadorID;
                                x.fechaCaptura = DateTime.Now;
                                x.semana = semana;
                                x.año = anio;
                                x.concepto = idConceptoDir;
                                x.estatus = true;
                            });

                            _ctxConstruplan.tblC_FED_PlaneacionDet.AddRange(listaPlaneacionDet);
                            _ctxConstruplan.SaveChanges();
                            foreach (var temp in listaPlaneacionDet)
                            {
                                tblC_FED_PlaneacionDet tempNe = new tblC_FED_PlaneacionDet();

                                tempNe.año = temp.año;
                                tempNe.cc = "TODOS";
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

                            _ctxConstruplan.tblC_FED_PlaneacionDet.AddRange(tempPlaneacion);
                            _ctxConstruplan.SaveChanges();
                            lstCC = listaPlaneacionDet.GroupBy(x => x.cc).Select(r => r.Key).ToList();
                            lstCC.Add("TODOS");
                            foreach (var itemCC in lstCC)
                            {
                                decimal motoTotalPlaneado = 0;
                                var listaTotales = _ctxConstruplan.tblC_FED_PlaneacionDet.Where(x => x.concepto == idConceptoDir && x.año == anio && x.semana == semana && itemCC == x.cc && x.estatus).ToList();
                                motoTotalPlaneado = listaTotales.Sum(s => s.monto);
                                var fed_capPlaneacionCC = _ctxConstruplan.tblC_FED_CapPlaneacion.FirstOrDefault(x => x.idConceptoDir == idConceptoDir && x.anio == anio && x.semana == semana && x.esActivo && x.cc == itemCC);

                                if (fed_capPlaneacionCC == null)
                                {
                                    var fecha = Infrastructure.Utils.DatetimeUtils.primerDiaSemana(anio, semana);
                                    fecha = fecha.Siguiente(DayOfWeek.Saturday);
                                    tblC_FED_CapPlaneacion nuevoRegistro = new tblC_FED_CapPlaneacion();
                                    nuevoRegistro.id = 0;
                                    nuevoRegistro.anio = anio;
                                    nuevoRegistro.cc = itemCC;
                                    nuevoRegistro.corte = 0;
                                    nuevoRegistro.esActivo = true;
                                    nuevoRegistro.fecha = fecha;
                                    nuevoRegistro.fechaRegistro = DateTime.Now;
                                    nuevoRegistro.flujoTotal = 0;
                                    nuevoRegistro.idConceptoDir = idConceptoDir;
                                    nuevoRegistro.planeado = motoTotalPlaneado;
                                    nuevoRegistro.semana = semana;
                                    nuevoRegistro.strFlujoEfectivo = "0";
                                    _ctxConstruplan.tblC_FED_CapPlaneacion.Add(nuevoRegistro);
                                    _ctxConstruplan.SaveChanges();
                                }
                                else
                                {
                                    fed_capPlaneacionCC.planeado = motoTotalPlaneado;
                                    _ctxConstruplan.SaveChanges();
                                }
                                var sumaPlaneacion = _ctxConstruplan.tblC_FED_PlaneacionDet.Where(x => x.concepto == idConceptoDir && x.año == anio && x.semana == semana && itemCC == x.cc && x.estatus).ToList();
                                planeado = sumaPlaneacion.Sum(s => s.monto);
                                var fed_capPlaneacion = _ctxConstruplan.tblC_FED_CapPlaneacion.FirstOrDefault(x => x.idConceptoDir == idConceptoDir && x.anio == anio && x.semana == semana && x.esActivo && x.cc == itemCC);
                                if (fed_capPlaneacion == null)
                                {
                                    tblC_FED_CapPlaneacion nuevoRegistro = new tblC_FED_CapPlaneacion();
                                    nuevoRegistro.id = 0;
                                    nuevoRegistro.anio = anio;
                                    nuevoRegistro.cc = cc;
                                    nuevoRegistro.corte = 0;
                                    nuevoRegistro.esActivo = true;
                                    nuevoRegistro.fecha = DateTime.Now;
                                    nuevoRegistro.fechaRegistro = DateTime.Now;
                                    nuevoRegistro.flujoTotal = 0;
                                    nuevoRegistro.idConceptoDir = idConceptoDir;
                                    nuevoRegistro.planeado = planeado;
                                    nuevoRegistro.semana = semana;
                                    nuevoRegistro.strFlujoEfectivo = "0";
                                    _ctxConstruplan.tblC_FED_CapPlaneacion.Add(nuevoRegistro);
                                    _ctxConstruplan.SaveChanges();
                                }
                                else
                                {
                                    fed_capPlaneacion.planeado = planeado;
                                    _ctxConstruplan.SaveChanges();
                                }
                            }
                            //dbTransaction.Commit();
                            resultado.Add(SUCCESS, true);
                            resultado.Add("concepto", idConceptoDir);
                            resultado.Add("cc", cc);
                            resultado.Add("semana", semana);
                        }
                        break;
                    default:                        
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
                            tempNe.cc = "TODOS";
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
                        lstCC = listaPlaneacionDet.GroupBy(x => x.cc).Select(r => r.Key).ToList();
                        lstCC.Add("TODOS");
                        foreach (var itemCC in lstCC)
                        {

                            decimal motoTotalPlaneado = 0;
                            var listaTotales = _context.tblC_FED_PlaneacionDet.Where(x => x.concepto == idConceptoDir && x.año == anio && x.semana == semana && itemCC == x.cc && x.estatus).ToList();
                            motoTotalPlaneado = listaTotales.Sum(s => s.monto);
                            var fed_capPlaneacionCC = _context.tblC_FED_CapPlaneacion.FirstOrDefault(x => x.idConceptoDir == idConceptoDir && x.anio == anio && x.semana == semana && x.esActivo && x.cc == itemCC);

                            if (fed_capPlaneacionCC == null)
                            {
                                var fecha = Infrastructure.Utils.DatetimeUtils.primerDiaSemana(anio, semana);
                                fecha = fecha.Siguiente(DayOfWeek.Saturday);
                                tblC_FED_CapPlaneacion nuevoRegistro = new tblC_FED_CapPlaneacion();
                                nuevoRegistro.id = 0;
                                nuevoRegistro.anio = anio;
                                nuevoRegistro.cc = itemCC;
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
                            var sumaPlaneacion = _context.tblC_FED_PlaneacionDet.Where(x => x.concepto == idConceptoDir && x.año == anio && x.semana == semana && itemCC == x.cc && x.estatus).ToList();
                            planeado = sumaPlaneacion.Sum(s => s.monto);
                            var fed_capPlaneacion = _context.tblC_FED_CapPlaneacion.FirstOrDefault(x => x.idConceptoDir == idConceptoDir && x.anio == anio && x.semana == semana && x.esActivo && x.cc == itemCC);
                            if (fed_capPlaneacion == null)
                            {
                                tblC_FED_CapPlaneacion nuevoRegistro = new tblC_FED_CapPlaneacion();
                                nuevoRegistro.id = 0;
                                nuevoRegistro.anio = anio;
                                nuevoRegistro.cc = cc;
                                nuevoRegistro.corte = 0;
                                nuevoRegistro.esActivo = true;
                                nuevoRegistro.fecha = DateTime.Now;
                                nuevoRegistro.fechaRegistro = DateTime.Now;
                                nuevoRegistro.flujoTotal = 0;
                                nuevoRegistro.idConceptoDir = idConceptoDir;
                                nuevoRegistro.planeado = planeado;
                                nuevoRegistro.semana = semana;
                                nuevoRegistro.strFlujoEfectivo = "0";
                                _context.tblC_FED_CapPlaneacion.Add(nuevoRegistro);
                                _context.SaveChanges();
                            }
                            else
                            {
                                fed_capPlaneacion.planeado = planeado;
                                _context.SaveChanges();
                            }
                        }
                        //dbTransaction.Commit();
                        resultado.Add(SUCCESS, true);
                        resultado.Add("concepto", idConceptoDir);
                        resultado.Add("cc", cc);
                        resultado.Add("semana", semana);
                        break;
                }             
            }
            catch(Exception e)
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
        private string getDescripcionCC(string d, List<ComboDTO> obj)
        {
            var objCC = obj.FirstOrDefault(r => r.Value == d);
            return objCC != null ? objCC.Value + " " + objCC.Text : d;
        }
        private string getInfoProvedor(int numpro, List<ProveedorDTO> prov)
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
            catch(Exception e)
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
                if(planeacionDetDTO.listFacturas != null)
                {
                    var resultFacturas = _context.tblC_FED_PlaneacionDet.Where(x => planeacionDetDTO.listFacturas.Contains(x.factura)).Where(x => x.estatus && cc == "TODOS" ? x.cc != "TODOS" : x.cc == cc).ToList();//.ToList();
                    resultado.Add("listaConceptosDetalle", resultFacturas);
                    resultado.Add(SUCCESS, true);
                }
                else if(planeacionDetDTO.listIDSigoplan.Count > 0)
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
            catch(Exception e)
            {

                LogError(0, 0, nombreControlador, "obtenerDealleDescripcionplanacionConcepto", e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al obtener los detalles del concepto...");
                return resultado;
            }
            return resultado;
        }
        #endregion

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
