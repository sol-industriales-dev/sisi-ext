using Core.DAO.Contabilidad.SistemaContable;
using Core.DTO;
using Core.DTO.Contabilidad.SistemaContable.TipoCambio;
using Core.Entity.Administrativo.Contabilidad.Sistema_Contable.Moneda;
using Core.Enum.Administracion.Cotizaciones;
using Core.Enum.Multiempresa;
using Core.Enum.Principal.Bitacoras;
using Data.DAO.Principal.Usuarios;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using System.Data.Entity.Migrations;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTO.Utils.Data;
using System.Data.Odbc;
using System.Data.Entity.Core.Objects;
using System.Data.Entity;
using System.Data.Entity.Core;

namespace Data.DAO.Contabilidad.SistemaContable
{
    public class TipoCambioDAO : GenericDAO<tblC_SC_TipoCambio>, ITipoCambioDAO
    {
        #region Guardar
        private const string nombreControlador = "TipoCambioDAO";
        public bool GuardarTipoCambio(tblC_SC_TipoCambio tipoCambio)
        {
            var esGuardado = false;
            using(var dbTransaction = _context.Database.BeginTransaction())
                try
                {
                    esGuardado = GuardarTipoCambioSigoplan(tipoCambio);
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
                    LogError(vSesiones.sesionSistemaActual, vSesiones.sesionCurrentView, nombreControlador, "GuardarTipoCambio", o_O, AccionEnum.ACTUALIZAR, tipoCambio.Id, tipoCambio);
                }
            return esGuardado;
        }
        bool GuardarTipoCambioSigoplan(tblC_SC_TipoCambio tipoCambio)
        {
            var ahora = DateTime.Now;
            var _bd = _context.tblC_SC_TipoCambio.ToList().FirstOrDefault(tc => tc.Moneda == tipoCambio.Moneda && tc.Fecha.Date == tipoCambio.Fecha.Date) ?? new  tblC_SC_TipoCambio();
            if(_bd.Id > 0)
            {
                tipoCambio.Id = _bd.Id;
                tipoCambio.esActivo = _bd.esActivo;
                tipoCambio.idUsuarioRegistro = _bd.idUsuarioRegistro;
                tipoCambio.fechaRegistro = _bd.fechaRegistro;
            }

            if(vSesiones.sesionEmpresaActual==6)
            {
                _bd = _context.tblC_SC_TipoCambio.ToList().FirstOrDefault(tc => tc.Moneda == tipoCambio.Moneda && tc.Fecha.Date == tipoCambio.Fecha.Date) ?? new tblC_SC_TipoCambio();
                if (_bd.Id > 0)
                {
                    tipoCambio.Id = _bd.Id;
                    tipoCambio.esActivo = _bd.esActivo;
                    tipoCambio.idUsuarioRegistro = _bd.idUsuarioRegistro;
                    tipoCambio.fechaRegistro = _bd.fechaRegistro;
                }
            }
            if(tipoCambio.Id == 0)
            {
                tipoCambio.registrar();
            }
            tipoCambio.Empleado_modifica = new UsuarioDAO().getUserEk(vSesiones.sesionUsuarioDTO.id).empleado;
            tipoCambio.FechaModifica = ahora;
            tipoCambio.HoraModifica = ahora;
            tipoCambio.Divisa = null;
            _context.tblC_SC_TipoCambio.AddOrUpdate(tipoCambio);
            _context.SaveChanges();
            return tipoCambio.Id > 0;
        }
        public bool GuardarTipoCambioEnkontrol(tblC_SC_TipoCambio tipoCambio)
        {
            var consulta = new OdbcConsultaDTO()
            {
                consulta = @"INSERT INTO ""DBA"".""tipo_cambio""
                            (moneda, fecha, tipo_cambio ,empleado_modifica ,fecha_modifica ,hora_modifica ,tc_anterior)
                            VALUES (?,?,?,?,?,?,?)",
                parametros = new List<OdbcParameterDTO>
                {
                    new OdbcParameterDTO { nombre = "moneda", tipo = OdbcType.Char, valor = tipoCambio.Moneda },
                    new OdbcParameterDTO { nombre = "fecha", tipo = OdbcType.DateTime, valor = tipoCambio.Fecha },
                    new OdbcParameterDTO { nombre = "tipo_cambio", tipo = OdbcType.Numeric, valor = tipoCambio.TipoCambio },
                    new OdbcParameterDTO { nombre = "empleado_modifica", tipo = OdbcType.Numeric, valor = tipoCambio.Empleado_modifica },
                    new OdbcParameterDTO { nombre = "fecha_modifica", tipo = OdbcType.DateTime, valor = tipoCambio.FechaModifica },
                    new OdbcParameterDTO { nombre = "hora_modifica", tipo = OdbcType.DateTime, valor = tipoCambio.HoraModifica },
                    new OdbcParameterDTO { nombre = "tc_anterior", tipo = OdbcType.Numeric, valor = tipoCambio.TcAnterior ?? 0 },
                }
            };
            var enkTipoCambio = _contextEnkontrol.Save(EnkontrolEnum.PruebaCplanProd, consulta);
            return enkTipoCambio > 0;
        }
        public bool ActualizarTipoCambioEnkontrol(tblC_SC_TipoCambio tipoCambio)
        {
            var consulta = new OdbcConsultaDTO()
            {
                consulta = @"UPDATE ""DBA"".""tipo_cambio""
                              SET tipo_cambio = ?
                                 ,empleado_modifica = ?
                                 ,fecha_modifica = ?
                                 ,hora_modifica = ?
                                 ,tc_anterior = ?
                            WHERE moneda = ? AND fecha = ?",
                parametros = new List<OdbcParameterDTO>
                {
                    new OdbcParameterDTO { nombre = "tipo_cambio", tipo = OdbcType.Numeric, valor = tipoCambio.TipoCambio },
                    new OdbcParameterDTO { nombre = "empleado_modifica", tipo = OdbcType.Numeric, valor = tipoCambio.Empleado_modifica },
                    new OdbcParameterDTO { nombre = "fecha_modifica", tipo = OdbcType.DateTime, valor = tipoCambio.FechaModifica },
                    new OdbcParameterDTO { nombre = "hora_modifica", tipo = OdbcType.DateTime, valor = tipoCambio.HoraModifica },
                    new OdbcParameterDTO { nombre = "tc_anterior", tipo = OdbcType.Numeric, valor = tipoCambio.TcAnterior ?? 0},
                    new OdbcParameterDTO { nombre = "moneda", tipo = OdbcType.Char, valor = tipoCambio.Moneda },
                    new OdbcParameterDTO { nombre = "fecha", tipo = OdbcType.DateTime, valor = tipoCambio.Fecha },
                }
            };
            var enkTipoCambio = _contextEnkontrol.Save(EnkontrolEnum.PruebaCplanProd, consulta);
            return enkTipoCambio > 0;
        }
        #endregion
        #region Captura Tipo Cambio
        public tblC_SC_TipoCambio TipoCambioDelDia()
        {
            var ahora = DateTime.Now;
            var moneda = MonedaPrincipal();
            var tipoCambio = _context.tblC_SC_TipoCambio.FirstOrDefault(tc => tc.esActivo && tc.Moneda == moneda.Clave && tc.Fecha.Year == ahora.Year && tc.Fecha.Month == ahora.Month && tc.Fecha.Day == ahora.Day) ?? new tblC_SC_TipoCambio();
            if(tipoCambio.Id == 0)
            {
                tipoCambio.Moneda = moneda.Clave;
                tipoCambio.Fecha = ahora;
                var Divisa = moneda ?? new tblC_SC_CatMoneda();
                tipoCambio.Divisa = Divisa;
            }

            if(vSesiones.sesionEmpresaActual==6)
            {
                tipoCambio = _context.tblC_SC_TipoCambio.FirstOrDefault(tc => tc.esActivo && tc.Moneda == moneda.Clave && tc.Fecha.Year == ahora.Year && tc.Fecha.Month == ahora.Month && tc.Fecha.Day == ahora.Day) ?? new tblC_SC_TipoCambio();
                if (tipoCambio.Id == 0)
                {
                    tipoCambio.Moneda = moneda.Clave;
                    tipoCambio.Fecha = ahora;
                    var Divisa = moneda ?? new tblC_SC_CatMoneda();
                    tipoCambio.Divisa = Divisa;
                }
            }
            return tipoCambio;
        }
        public List<TipoCambioDTO> HistoricoTipoCambio()
        {
            var Monedas = CatMoneda();
            var tiposCambioSigoplan = TiposCambioSigoplan();
            var tiposCambio = (from tc in tiposCambioSigoplan.AsQueryable()
                              orderby tc.Fecha descending
                              select tc).ToList();
            tiposCambio.ForEach(tc => tc.Divisa = Monedas.FirstOrDefault(mon => mon.Clave == tc.Moneda));

            if(vSesiones.sesionEmpresaActual==6)
            {
                tiposCambio = (from tc in tiposCambioSigoplan.AsQueryable()
                               orderby tc.Fecha descending
                               select tc).ToList();
                tiposCambio.ForEach(tc => tc.Divisa = Monedas.FirstOrDefault(mon => mon.Clave == tc.Moneda));

            }
            return tiposCambio.ToList();
        }
        List<TipoCambioDTO> TiposCambioSigoplan()
        {
            if(vSesiones.sesionEmpresaActual==3)
            {
                return (from tc in _context.tblC_SC_TipoCambio
                        where tc.esActivo
                        select new TipoCambioDTO
                        {
                            Id = tc.Id,
                            Moneda = tc.Moneda,
                            Fecha = tc.Fecha,
                            TipoCambio = tc.TipoCambio,
                            Empleado_modifica = tc.Empleado_modifica,
                            Empleado_modifica_nombre = string.Empty,
                            FechaModifica = tc.FechaModifica,
                            HoraModifica = tc.HoraModifica,
                            TcAnterior = tc.TcAnterior,
                            Divisa = tc.Divisa,

                        }).ToList();
            }else if (vSesiones.sesionEmpresaActual == 6)
            {
                return (from tc in _context.tblC_SC_TipoCambio
                        where tc.esActivo
                        select new TipoCambioDTO
                        {
                            Id = tc.Id,
                            Moneda = tc.Moneda,
                            Fecha = tc.Fecha,
                            TipoCambio = tc.TipoCambio,
                            Empleado_modifica = tc.Empleado_modifica,
                            Empleado_modifica_nombre = string.Empty,
                            FechaModifica = tc.FechaModifica,
                            HoraModifica = tc.HoraModifica,
                            TcAnterior = tc.TcAnterior,
                            Divisa = tc.Divisa,

                        }).ToList();
            }
            else
            {
                return (from tc in _context.tblC_SC_TipoCambio
                        where tc.esActivo
                        select new TipoCambioDTO
                        {
                            Id = tc.Id,
                            Moneda = tc.Moneda,
                            Fecha = tc.Fecha,
                            TipoCambio = tc.TipoCambio,
                            Empleado_modifica = tc.Empleado_modifica,
                            Empleado_modifica_nombre = string.Empty,
                            FechaModifica = tc.FechaModifica,
                            HoraModifica = tc.HoraModifica,
                            TcAnterior = tc.TcAnterior,
                            Divisa = tc.Divisa,

                        }).ToList();
            }

        }
        List<TipoCambioDTO> TiposCambioEnkontrol()
        {
            var consulta = @"SELECT tc.moneda AS Moneda, tc.fecha AS Fecha, tc.tipo_cambio AS TipoCambio, tc.empleado_modifica AS Empleado_modifica, fecha_modifica AS FechaModifica, CAST(hora_modifica AS Timestamp) AS HoraModifica, tc_anterior AS TcAnterior, u.nom AS Empleado_modifica_nombre
                            FROM ""DBA"".""tipo_cambio"" tc
                            INNER JOIN ""DBA"".""ek010ab"" u ON u.num = tc.empleado_modifica";
            return _contextEnkontrol.Select<TipoCambioDTO>(EnkontrolAmbienteEnum.Prod, consulta);
        }
        bool existeTipoCambioEnkontrol(tblC_SC_TipoCambio tipoCambio)
        {
            var consulta = new OdbcConsultaDTO()
            {
                consulta = @"SELECT tc.moneda AS Moneda, tc.fecha AS Fecha, tc.tipo_cambio AS TipoCambio, tc.empleado_modifica AS Empleado_modifica, fecha_modifica AS FechaModifica, CAST(hora_modifica AS Timestamp) AS HoraModifica, tc_anterior AS TcAnterior
                            FROM ""DBA"".""tipo_cambio"" tc
                            WHERE tc.moneda = ? AND tc.fecha = ?"
            };
            consulta.parametros.Add(new OdbcParameterDTO { nombre = "moneda", tipo = OdbcType.Char, valor = tipoCambio.Moneda });
            consulta.parametros.Add(new OdbcParameterDTO { nombre = "fecha", tipo = OdbcType.Date, valor = tipoCambio.Fecha });
            var enkTipoCambio = _contextEnkontrol.Select<TipoCambioDTO>(EnkontrolEnum.PruebaCplanProd, consulta);
            return enkTipoCambio.Any();
        }
        public List<tblC_SC_CatMoneda> CatMoneda()
        {            

            if(vSesiones.sesionEmpresaActual==6)
            {
                return _context.tblC_SC_CatMoneda.ToList();
            }
            else
            {
                return _context.tblC_SC_CatMoneda.ToList();
            }
        }
        public List<tblC_SC_CatMoneda> CatMonedaEnkontrol()
        {
            var consulta = "SELECT num AS Id, moneda AS Moneda, clave AS Clave, denom AS Denominacion, cve_moneda_sat AS Codigo, idioma AS idioma FROM \"DBA\".\"moneda\"";
            return _contextEnkontrol.Select<tblC_SC_CatMoneda>(EnkontrolAmbienteEnum.Prod, consulta);
        }
        #endregion
        #region combobox
        #endregion
        #region Auxiliares
        tblC_SC_CatMoneda MonedaPrincipal()
        {
            var moneda = CatMoneda().FirstOrDefault(m => m.Clave == 3);

            if (vSesiones.sesionEmpresaActual == 6)
            {
                moneda = CatMoneda().FirstOrDefault(m => m.Clave == 4);
            }
                
            return moneda;
        }
        #endregion
    }
}
