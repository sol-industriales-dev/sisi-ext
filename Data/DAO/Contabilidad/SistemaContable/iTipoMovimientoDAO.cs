using Core.DAO.Contabilidad.SistemaContable;
using Core.DTO.Contabilidad.SistemaContable.Cuentas;
using Core.DTO.Contabilidad.SistemaContable.iTipoMovimiento;
using Core.DTO.Utils.Data;
using Core.Entity.Administrativo.Contabilidad.Sistema_Contable.iTiposMovimientos;
using Core.Enum.Multiempresa;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Infrastructure.DTO;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.Principal;

namespace Data.DAO.Contabilidad.SistemaContable
{
    public class iTipoMovimientoDAO : GenericDAO<tblC_TM_Relitm>, IiTipoMovimientoDAO
    {
        private const string nombreControlador = "iTipoMovimiento";
        #region Guardar
        public bool saveRelitm(List<tblC_TM_Relitm> lst)
        {
            var esGuardado = false;
            using(var _ctxCplan = new MainContext(EmpresaEnum.Construplan))
            using(var _traCplan = _ctxCplan.Database.BeginTransaction())
                try
                {
                    var primero = lst.FirstOrDefault();
                    var _bd = from bd in Relitm(_ctxCplan)
                                  where bd.PalEmpresa == primero.PalEmpresa && bd.SecEmpresa == primero.SecEmpresa
                                  select bd;
                    #region Desactivar itm
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
                        var itm = lst[i];
                        var _itm = (from rel in _bd
                                    where rel.PalEmpresa == itm.PalEmpresa && rel.PalSistema.Trim() == itm.PalSistema && rel.PaliTm == itm.PaliTm && rel.SecEmpresa == itm.SecEmpresa && rel.SecSistema.Trim() == itm.SecSistema && rel.SeciTm == itm.SeciTm
                                    select rel).FirstOrDefault();
                        if(_itm == null)
                        {
                            itm.registrar();
                        }
                        else
                        {
                            itm.Id = _itm.Id;
                            itm.esActivo = true;
                            itm.idUsuarioRegistro = _itm.idUsuarioRegistro;
                            itm.fechaRegistro = _itm.fechaRegistro;
                        }
                        _ctxCplan.tblC_TM_Relitm.AddOrUpdate(itm);
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
                            nombre = "spC_TM_TruncateInsertRelItm"
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
        #endregion
        #region Relacion iTipo Movimiento
        public List<tblC_TM_Relitm> ReliTmEmpresas(BusqAsignacionCuenta busq)
        {
            return _context.tblC_TM_Relitm.Where(rel => rel.esActivo && rel.PalEmpresa == busq.palEmpresa && rel.SecEmpresa == busq.secEmpresa).ToList();
        }
        IEnumerable<tblC_TM_Relitm> Relitm(MainContext _ctx)
        {
            return from rel in _ctx.tblC_TM_Relitm select rel;
        }
        public List<iTmEmpresaDTO> ITipoMovimientoEmpresa(List<string> iSistemas)
        {
            try
            {
                var catItm = new List<iTmEmpresaDTO>();
                var Empresas = (from empresa in EnumExtensions.ToCombo<EmpresaEnum>()
                                select (EmpresaEnum)empresa.Value).ToList();
                Empresas.ForEach(empresa =>
                {
                    var baseDatos = _contextEnkontrol.BaseDatosProductivoDesdeEmpresa((EmpresaEnum)empresa);
                    iSistemas.ForEach(iSistema =>
                    {
                        var conector = new OdbcConsultaDTO();
                        conector.parametros.Add(new OdbcParameterDTO { nombre = "Empresa", tipo = OdbcType.Int, valor = (int)empresa });
                        switch(iSistema)
                        {
                            case "B":
                                conector.consulta = "SELECT (CONVERT(char(2), clave) + '  ' + descripcion) AS Text, clave AS Value, 'B' AS Prefijo, ? AS Empresa FROM \"DBA\".\"sb_tm\" ORDER BY Value";
                                break;
                            case "P":
                                conector.consulta = "SELECT (CONVERT(char(2), tm) + '  ' + descripcion) AS Text, tm AS Value, 'P' AS Prefijo, ? AS Empresa FROM \"DBA\".\"sp_tm\" ORDER BY Value";
                                break;
                            case "X":
                                conector.consulta = "SELECT (CONVERT(char(2), tm) + '  ' + descripcion) AS Text, tm as Value, 'X' AS Prefijo, ? AS Empresa FROM \"DBA\".\"sx_tm\" ORDER BY Value";
                                break;
                            default:
                                conector.consulta = string.Empty;
                                break;
                        }
                        if(conector.consulta.Any())
                        {
                            catItm.AddRange(_contextEnkontrol.Select<iTmEmpresaDTO>(baseDatos, conector));
                        }
                    });
                });
                return catItm;
            }
            catch(Exception)
            {
                return new List<iTmEmpresaDTO>();

            }
        }
        #endregion
    }
}
