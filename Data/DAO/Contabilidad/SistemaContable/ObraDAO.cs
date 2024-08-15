using Core.DAO.Contabilidad.SistemaContable;
using Core.DTO.Contabilidad.SistemaContable.Cuentas;
using Core.DTO.Contabilidad.SistemaContable.Obra;
using Core.DTO.Utils.Data;
using Core.Entity.Administrativo.Contabilidad.Sistema_Contable.CentroCostos;
using Core.Enum.Multiempresa;
using Core.Enum.Principal;
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

namespace Data.DAO.Contabilidad.SistemaContable
{
    public class ObraDAO : GenericDAO<tblC_CC_RelObras>, IObraDAO
    {
        private const string nombreControlador = "Obra";
        public bool saveRelObras(List<tblC_CC_RelObras> lst)
        {
            var esGuardado = false;
            using(var _ctxCplan = new MainContext(EmpresaEnum.Construplan))
            using(var _traCplan = _ctxCplan.Database.BeginTransaction())
                try
                {
                    var primero = lst.FirstOrDefault();
                    var _bd = from bd in RelObras(_ctxCplan)
                              where bd.PalEmpresa == primero.PalEmpresa && bd.SecEmpresa == primero.SecEmpresa
                              select bd;
                    #region Desactivar obras
                    for(int i = 0; i < _bd.Count(); i++)
                    {
                        var _cuenta = _bd.ElementAtOrDefault(i);
                        _cuenta.esActivo = false;
                        if(i % 100 == 0)
                        {
                            _ctxCplan.SaveChanges();
                        }
                    }
                    _ctxCplan.SaveChanges();
                    #endregion
                    #region guarda o actualiza
                    for(int i = 0; i < lst.Count; i++)
                    {
                        var obra = lst[i];
                        var _obra = (from cc in _bd
                                       where cc.PalEmpresa == obra.PalEmpresa && cc.PalObra == obra.PalObra && cc.SecEmpresa == obra.SecEmpresa && cc.SecObra == obra.SecObra
                                       select cc).FirstOrDefault();
                        if(_obra == null)
                        {
                            obra.registrar();
                        }
                        else
                        {
                            obra.Id = _obra.Id;
                            obra.esActivo = true;
                            obra.idUsuarioRegistro = _obra.idUsuarioRegistro;
                            obra.fechaRegistro = _obra.fechaRegistro;
                        }
                        _ctxCplan.tblC_CC_RelObras.AddOrUpdate(obra);
                        if(i % 100 == 0)
                        {
                            _ctxCplan.SaveChanges();
                        }
                    }
                    _ctxCplan.SaveChanges();
                    #endregion
                    #region Actualizar en todas las empresas
                    if(lst.All(obra => obra.Id > 0))
                    {
                        _traCplan.Commit();
                        var spRelCtas = new StoreProcedureDTO()
                        {
                            baseDatos = MainContextEnum.Construplan,
                            nombre = "spC_CC_TruncateInsertRelObras"
                        };
                        esGuardado = _ctxCplan.sp_SaveUpdate(spRelCtas);   
                    }
                    #endregion
                }
                catch(Exception o_O)
                {
                    throw;
                }
            return esGuardado;
        }
        public bool DeleteObra(tblC_CC_RelObras obra)
        {
            var esEliminado = false;
            using(var _ctxCplan = new MainContext(EmpresaEnum.Construplan))
            using(var _traCplan = _ctxCplan.Database.BeginTransaction())
                try
                {
                    var _bd = RelObras(_ctxCplan);
                    var _obra = (from cc in _bd
                                 where cc.PalEmpresa == obra.PalEmpresa && cc.PalObra == obra.PalObra && cc.SecEmpresa == obra.SecEmpresa && cc.SecObra == obra.SecObra
                                 select cc).FirstOrDefault();
                    #region actualizar en todas las empresas
                    var spRelCtas = new StoreProcedureDTO()
                    {
                        baseDatos = MainContextEnum.Construplan,
                        nombre = "spC_CC_TruncateInsertRelObras"
                    };
                    var esSpComplete = _ctxCplan.sp_SaveUpdate(spRelCtas);
                    #endregion
                    esEliminado = true;
                }
                catch(Exception o_O)
                {

                    throw;
                }
            return esEliminado;
        }
        IEnumerable<tblC_CC_RelObras> RelObras(MainContext _ctx)
        {
            return from obra in _ctx.tblC_CC_RelObras select obra;
        }
        public List<tblC_CC_RelObras> RelObras(BusqAsignacionCuenta busq)
        {
            return (from cc in _context.tblC_CC_RelObras
                    where cc.esActivo && cc.PalEmpresa == busq.palEmpresa && cc.SecEmpresa == busq.secEmpresa
                    select cc).ToList();
        }
        public List<CentroCostoEmpresaDTO> CatObraEmpresa()
        {
            var catObras = new List<CentroCostoEmpresaDTO>();
            var lstEmpresa = (from empresa in EnumExtensions.ToCombo<EmpresaEnum>()
                              select (EmpresaEnum)empresa.Value).ToList();
            lstEmpresa.ForEach(empresa =>
            {
                var baseDatos = _contextEnkontrol.BaseDatosProductivoDesdeEmpresa((EmpresaEnum)empresa);
                var odbc = new OdbcConsultaDTO()
                {
                    consulta = "SELECT * ,? AS empresa FROM \"DBA\".\"cc\" ORDER BY cc",
                    parametros = new List<OdbcParameterDTO>()
                };
                odbc.parametros.Add(new OdbcParameterDTO() { nombre = "empresa", tipo = OdbcType.Int, valor = (int)empresa });
                var catObrasEnk = _contextEnkontrol.Select<CentroCostoEmpresaDTO>(baseDatos, odbc);
                catObras.AddRange(catObrasEnk);
            });
            return catObras;
        }
    }
}
