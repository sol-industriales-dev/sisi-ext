using Core.DAO.Contabilidad.SistemaContable;
using Core.DTO;
using Core.DTO.Contabilidad.SistemaContable.Cuentas;
using Core.DTO.Utils.Data;
using Core.Entity.Administrativo.Contabilidad.Sistema_Contable.Cuentas;
using Core.Enum.Multiempresa;
using Core.Enum.Principal;
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
using System.Data.SqlClient;
using Dapper;
using System.Data;

namespace Data.DAO.Contabilidad.SistemaContable
{
    public class CuentaDAO : GenericDAO<tblC_Cta_RelCuentas>, ICuentaDAO
    {
        #region Asignación de ctas
        public bool saveRelCuentas(List<tblC_Cta_RelCuentas> lst)
        {
            var esGuardado = false;
           
           
            using(var _ctxCplan = new MainContext(EmpresaEnum.Construplan))
            using(var _traCplan = _ctxCplan.Database.BeginTransaction())
                try
                {
                 
                        var primero = lst.FirstOrDefault();
                        var _bdCuentas = from bd in getRelCuentas(_ctxCplan)
                                         where bd.palEmpresa == primero.palEmpresa && bd.secEmpresa == primero.secEmpresa
                                         select bd;



                        #region desactivar cuentas
                        for (int i = 0; i < _bdCuentas.Count(); i++)
                        {
                            var _cuenta = _bdCuentas.ElementAtOrDefault(i);
                            _cuenta.esActivo = false;
                            if (i % 100 == 0)
                            {
                                _ctxCplan.SaveChanges();
                            }
                        }
                        _ctxCplan.SaveChanges();
                        #endregion

                        #region guarda o actualiza
                        for (int i = 0; i < lst.Count; i++)
                        {
                            var cuenta = lst[i];
                            var _cuenta = (from cta in _bdCuentas
                                           where cta.palEmpresa == cuenta.palEmpresa && cta.palCta == cuenta.palCta && cta.palScta == cuenta.palScta && cta.palSscta == cuenta.palSscta
                                           && cta.secEmpresa == cuenta.secEmpresa && cta.secCta == cuenta.secCta && cta.secScta == cuenta.secScta && cta.secSscta == cuenta.secSscta
                                           select cta).FirstOrDefault();


                            if (_cuenta == null)
                            {
                                cuenta.registrar();
                            }
                            else
                            {
                                cuenta.id = _cuenta.id;
                                cuenta.esActivo = true;
                                cuenta.idUsuarioRegistro = _cuenta.idUsuarioRegistro;
                                cuenta.fechaRegistro = _cuenta.fechaRegistro;
                            }
                            _ctxCplan.tblC_Cta_RelCuentas.AddOrUpdate(cuenta);
                            if (i % 100 == 0)
                            {
                                _ctxCplan.SaveChanges();
                            }
                        }
                        _ctxCplan.SaveChanges();
                        #endregion

                        #region actualizar en todas las empresas
                        _traCplan.Commit();
                        var spRelCtas = new StoreProcedureDTO()
                        {
                            baseDatos = MainContextEnum.Construplan,
                            nombre = "spC_Cta_TruncateInsertRelCuentas"
                        };
                    if(vSesiones.sesionEmpresaActual==6)
                    {
                        spRelCtas = new StoreProcedureDTO()
                        {
                            baseDatos = MainContextEnum.PERU,
                            nombre = "spC_Cta_TruncateInsertRelCuentas"
                        };
                    }

                        var esSpComplete = _ctxCplan.sp_SaveUpdate(spRelCtas);
                        #endregion

                        esGuardado = lst.All(cta => cta.id > 0) && esSpComplete;   

                  
                }
                catch(Exception o_O)
                {

                    throw;
                }
            return esGuardado;
        }
        public bool DeleteCuenta(tblC_Cta_RelCuentas cuenta)
        {
            var esEliminado = false;
            using(var _ctxCplan = new MainContext(EmpresaEnum.Construplan))
            using(var _traCplan = _ctxCplan.Database.BeginTransaction())
                try
                {
                    var _bdCuentas = getRelCuentas(_ctxCplan);
                    var _cuenta = (from cta in _bdCuentas
                                   where cta.palEmpresa == cuenta.palEmpresa && cta.palCta == cuenta.palCta && cta.palScta == cuenta.palScta && cta.palSscta == cuenta.palSscta
                                   && cta.secEmpresa == cuenta.secEmpresa && cta.secCta == cuenta.secCta && cta.secScta == cuenta.secScta && cta.secSscta == cuenta.secSscta
                                   select cta).FirstOrDefault();
                    if(_cuenta != null)
                    {
                        _cuenta.esActivo = false;
                        _ctxCplan.SaveChanges();
                        #region actualizar en todas las empresas
                        var spRelCtas = new StoreProcedureDTO()
                        {
                            baseDatos = MainContextEnum.Construplan,
                            nombre = "spC_Cta_TruncateInsertRelCuentas"
                        };
                       
                        var esSpComplete = _ctxCplan.sp_SaveUpdate(spRelCtas);
                        #endregion
                    }
                    esEliminado = true;
                }
                catch(Exception o_O)
                {

                    throw;
                }
            return esEliminado;
        }
        IEnumerable<tblC_Cta_RelCuentas> getRelCuentas(MainContext _ctx)
        {
            return from cta in _ctx.tblC_Cta_RelCuentas select cta;
        }
        public List<tblC_Cta_RelCuentas> getRelCuentas(BusqAsignacionCuenta busq)
        {
            return (from cta in _context.tblC_Cta_RelCuentas
                    where cta.esActivo && cta.palEmpresa == busq.palEmpresa && cta.secEmpresa == busq.secEmpresa
                    select cta).ToList();
        }
        public List<CatCtaEmpresa> getCatCta()
        {
            try
            {
                var lst = new List<CatCtaEmpresa>();
                var baseDatos = _contextEnkontrol.BaseDatosProductivoDesdeEmpresa(EmpresaEnum.Construplan);
                var odbc = new OdbcConsultaDTO()
                {
                    consulta = "SELECT * ,? AS empresa FROM DBA.catcta ORDER BY cta, scta, sscta",
                    parametros = new List<OdbcParameterDTO>()
                };
                odbc.parametros.Add(new OdbcParameterDTO() { nombre = "empresa", tipo = OdbcType.Int, valor = 1 });
                var lstCta = _contextEnkontrol.Select<CatCtaEmpresa>(baseDatos, odbc);
                lst.AddRange(lstCta);

                if (vSesiones.sesionEmpresaActual == 6)
                {
                    using (var dbStartSoft = new MainContextPeruStarSoft003BDCONTABILIDAD())
                    {
                        var lstCuentasPeru = dbStartSoft.PLAN_CUENTA_NACIONAL.Where(e => e.PLANCTA_NIVEL == 5).ToList().Select(e =>
                        {
                            var auxCta = e.PLANCTA_CODIGO.ParseInt();
                            return new CatCtaEmpresa
                            {
                                empresa = (EmpresaEnum)6,
                                cta = auxCta,
                                scta = 0,
                                sscta = 0,
                                descripcion = e.PLANCTA_DESCRIPCION
                            };
                        }).ToList();

                        lst.AddRange(lstCuentasPeru);

                    }
                }
                else 
                {
                    baseDatos = _contextEnkontrol.BaseDatosProductivoDesdeEmpresa(EmpresaEnum.Colombia);
                    odbc = new OdbcConsultaDTO()
                    {
                        consulta = "SELECT * ,? AS empresa FROM DBA.catcta ORDER BY cta, scta, sscta",
                        parametros = new List<OdbcParameterDTO>()
                    };
                    odbc.parametros.Add(new OdbcParameterDTO() { nombre = "empresa", tipo = OdbcType.Int, valor = 3 });
                    lstCta = _contextEnkontrol.Select<CatCtaEmpresa>(baseDatos, odbc);
                    lst.AddRange(lstCta);
                }
                return lst;
            }
            catch(Exception) { return new List<CatCtaEmpresa>(); }
        }
        #endregion
    }
}
