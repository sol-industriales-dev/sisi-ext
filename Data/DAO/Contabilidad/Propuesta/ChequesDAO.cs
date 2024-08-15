using Core.DAO.Contabilidad.Propuesta;
using Core.DTO;
using Core.DTO.Contabilidad;
using Core.DTO.Contabilidad.Bancos;
using Core.DTO.Contabilidad.Propuesta;
using Core.DTO.Principal.Generales;
using Core.DTO.Utils.Data;
using Core.Entity.Administrativo.Contabilidad.Propuesta.Acomulado;
using Core.Enum.Administracion.Banco;
using Core.Enum.Administracion.Propuesta;
using Core.Enum.Multiempresa;
using Data.DAO.Contabilidad.Reportes;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Infrastructure.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Contabilidad.Propuesta
{
    public class ChequesDAO : GenericDAO<tblC_RelCuentaDivision>, IChequesDAO
    {
        #region Propuesta
        public List<ChequesDTO> getLstEdoCta(BusqConcentradoDTO busq)
        {
            try
            {
                var lstDiv = new List<int>();
                if (busq.esDivIndustrial)
                {
                    lstDiv.AddRange(new List<int>() 
                    { 
                        (int)TipoCCEnum.Industrial,
                        (int)TipoCCEnum.ObraCerradaIndustrial
                    });
                }
                else
                {
                    lstDiv.AddRange(new List<int>() 
                    { 
                        (int)TipoCCEnum.ConstruccionPesada,
                        (int)TipoCCEnum.Administración,
                        (int)TipoCCEnum.ObraCerradaGeneral
                    });
                }
                var lstCuenta = _context.tblC_RelCuentaDivision.ToList()
                    .Where(r => lstDiv.Any(d => d.Equals(r.division)))
                    .GroupBy(g => g.cuenta)
                    .Select(r => r.Key.ToString()).ToList();
                var odbc = new OdbcConsultaDTO()
                {
                    consulta = queryLstEdoCta(busq, lstCuenta),
                    parametros = parametrosLstCheques(busq, lstCuenta)
                };
                var lst = _contextEnkontrol.Select<ChequesDTO>(EnkontrolAmbienteEnum.Prod, odbc);
                return lst;
            }
            catch (Exception) { return new List<ChequesDTO>(); }
        }
        string queryLstEdoCta(BusqConcentradoDTO busq, List<string> lstCuenta)
        {
            var consulta = string.Format(@"SELECT edo.fecha_mov, edo.numero, edo.monto, edo.tc, edo.tm, edo.cc, edo.descripcion AS concepto, tm.descripcion, edo.ipoliza AS poliza, edo.itp AS tp
                                    FROM sb_edo_cta_chequera edo,sc_movpol mov, sb_tm tm
                                    WHERE edo.ipoliza = mov.poliza AND edo.itp = mov.tp AND edo.iyear = mov.year AND edo.imes = mov.mes AND mov.linea = edo.ilinea
                                        AND tm.clave = edo.tm
                                        AND edo.itp NOT IN ('04','05','06','08')--('01','02','03','07','0D','0S','0U','0B','0S','2H','2M')
                                        AND edo.tm NOT IN (5,8,11,14,64,67,68,69)
                                        AND edo.fecha_mov BETWEEN ? AND  ?
                                        AND edo.cuenta IN {0}
                                        AND edo.cc IN {1}"
                , lstCuenta.ToParamInValue()
                , busq.lstCC.ToParamInValue());
            return consulta;
        }
        List<OdbcParameterDTO> parametrosLstCheques(BusqConcentradoDTO busq, List<string> lstCuenta)
        {
            var parameters = new List<OdbcParameterDTO>();
            parameters.Add(new OdbcParameterDTO() { nombre = "fecha_mov", tipo = OdbcType.Date, valor = busq.min });
            parameters.Add(new OdbcParameterDTO() { nombre = "fecha_mov", tipo = OdbcType.Date, valor = busq.max });
            lstCuenta.ForEach(c =>
            {
                parameters.Add(new OdbcParameterDTO() { nombre = "cuenta", tipo = OdbcType.VarChar, valor = c });
            });
            busq.lstCC.ForEach(c =>
            {
                parameters.Add(new OdbcParameterDTO() { nombre = "cc", tipo = OdbcType.VarChar, valor = c });
            });
            return parameters;
        }
        public List<sbCuentaDTO> getLstCuenta(List<string> lstCta)
        {
            try
            {
                var odbc = new OdbcConsultaDTO()
                {
                    consulta = queryLstCuenta(lstCta),
                    parametros = paramLstCuenta(lstCta)
                };
                var lst = _contextEnkontrol.Select<sbCuentaDTO>(EnkontrolAmbienteEnum.Prod, odbc);
                return lst;
            }
            catch (Exception o_O) { return new List<sbCuentaDTO>(); }
        }
        string queryLstCuenta(List<string> lstCta)
        {
            return string.Format(@"SELECT cta.cuenta ,cta.descripcion ,banco ,moneda ,num_cta_banco ,(SELECT ban.descripcion FROM sb_bancos ban WHERE ban.banco = cta.banco) AS descBanco
                                    FROM sb_cuenta cta
                                    WHERE cta.cuenta IN {0}
                                    ORDER BY cta.cuenta"
                , lstCta.ToParamInValue());
        }
        List<OdbcParameterDTO> paramLstCuenta(List<string> lstCta)
        {
            var parameters = new List<OdbcParameterDTO>();
            lstCta.ForEach(cta => parameters.Add(new OdbcParameterDTO()
            {
                nombre = "cuenta",
                tipo = OdbcType.Int,
                valor = cta
            }));
            return parameters;
        }
        #endregion
        #region Consulta Concentrado
        public List<sbCuentaDTO> getConsultaConcentrado(BusqConsultaConcentradoDTO busq)
        {
            try
            {
                var odbc = new OdbcConsultaDTO()
                {
                    consulta = queryConsultaConcentrado(busq),
                    parametros = paramConsultaConcentrado(busq)
                };
                var lst = _contextEnkontrol.Select<sbCuentaDTO>(EnkontrolAmbienteEnum.Prod, odbc);
                return lst;
            }
            catch (Exception o_O) { return new List<sbCuentaDTO>(); }
        }
        string queryConsultaConcentrado(BusqConsultaConcentradoDTO busq)
        {
            return string.Format(@"SELECT edo.fecha_mov, edo.numero, edo.monto, edo.tc, edo.tm, edo.cc, edo.descripcion AS concepto, tm.descripcion, edo.ipoliza AS poliza, edo.itp AS tp
                                    FROM sb_edo_cta_chequera edo,sc_movpol mov, sb_tm tm
                                    WHERE edo.ipoliza = mov.poliza AND edo.itp = mov.tp AND edo.iyear = mov.year AND edo.imes = mov.mes AND mov.linea = edo.ilinea
                                        AND tm.clave = edo.tm
                                        AND edo.itp IN {0}
                                        AND edo.tm IN {1}
                                        AND edo.fecha_mov BETWEEN ? AND  ?
                                        AND edo.cuenta IN {2}
                                        AND edo.cc IN {3}"
                ,busq.lstTm.ToParamInValue()
                ,busq.lstTp.ToParamInValue()
                ,busq.lstCta.ToParamInValue()
                ,busq.lstCC.ToParamInValue());
        }
        List<OdbcParameterDTO> paramConsultaConcentrado(BusqConsultaConcentradoDTO busq)
        {
            var lst = new List<OdbcParameterDTO>();
            lst.AddRange(busq.lstTp.Select(s => new OdbcParameterDTO() { nombre = "itp", tipo = OdbcType.VarChar, valor = s }));
            lst.AddRange(busq.lstTm.Select(s => new OdbcParameterDTO() { nombre = "tm", tipo = OdbcType.Int, valor = s }));
            lst.Add(new OdbcParameterDTO() { nombre = "fecha_mov", tipo = OdbcType.Date, valor = busq.min });
            lst.Add(new OdbcParameterDTO() { nombre = "fecha_mov", tipo = OdbcType.Date, valor = busq.max });
            lst.AddRange(busq.lstCta.Select(s => new OdbcParameterDTO() { nombre = "cuenta", tipo = OdbcType.Int, valor = s }));
            lst.AddRange(busq.lstCC.Select(s => new OdbcParameterDTO() { nombre = "cc", tipo = OdbcType.VarChar, valor = s }));
            return lst;
        }
        #endregion
        #region Reporte Bancos
        public List<RptBanMovDTO> getLstBancosMov(BusqBancoMov busq)
        {
            var lstEdoCtaChe = getLstEdoCtaChequera(busq);
            var lstBanMov = setFormatEdoCtaToBanMov(lstEdoCtaChe, busq.formato);
            return lstBanMov;
        }
        List<sbEdoCtaChequeraDTO> getLstEdoCtaChequera(BusqBancoMov busq)
        {
            try
            {
                var odbc = new OdbcConsultaDTO()
                {
                    consulta = queryBancosMov(busq),
                    parametros = paramBancosMov(busq)
                };
                var lst = _contextEnkontrol.Select<sbEdoCtaChequeraDTO>(EnkontrolAmbienteEnum.Prod, odbc);
                return lst;
            }
            catch (Exception o_O) { return new List<sbEdoCtaChequeraDTO>(); }
        }
        string queryBancosMov(BusqBancoMov busq)
        {
            return string.Format(@"SELECT edo.cuenta, edo.fecha_mov, edo.tm, edo.numero, edo.cc, edo.monto, edo.descripcion ,edo.ipoliza, edo.itp, edo.ilinea, tm.naturaleza ,edo.banco ,edo.st_che
                                    ,(SELECT che.status_lp FROM sb_cheques che 
                                                    WHERE che.cuenta = edo.cuenta AND che.numero = edo.numero AND che.iyear = edo.iyear AND che.imes = edo.imes AND che.itp = edo.itp AND che.ipoliza = edo.ipoliza) AS status_lp
                                    FROM sb_edo_cta_chequera edo ,sb_tm tm 
                                    WHERE tm.clave = edo.tm 
                                        AND edo.fecha_mov BETWEEN ? AND ?
                                        AND edo.cc IN {0}
                                        AND edo.cuenta IN {1}
                                        AND edo.tm IN {2}
                                        AND edo.itp IN {3}"
                , busq.lstCc.ToParamInValue()
                , busq.lstCta.ToParamInValue()
                , busq.lstTm.ToParamInValue()
                , busq.lstTp.ToParamInValue());
        }
        List<OdbcParameterDTO> paramBancosMov(BusqBancoMov busq)
        {
            var lst = new List<OdbcParameterDTO>();
            lst.Add(new OdbcParameterDTO() { nombre = "fecha_mov", tipo = OdbcType.Date, valor = busq.min });
            lst.Add(new OdbcParameterDTO() { nombre = "fecha_mov", tipo = OdbcType.Date, valor = busq.max });
            lst.AddRange(busq.lstCc.Select(s => new OdbcParameterDTO() { nombre = "cc", tipo = OdbcType.VarChar, valor = s }));
            lst.AddRange(busq.lstCta.Select(s => new OdbcParameterDTO() { nombre = "cuenta", tipo = OdbcType.Numeric, valor = s }));
            lst.AddRange(busq.lstTm.Select(s => new OdbcParameterDTO() { nombre = "tm", tipo = OdbcType.Numeric, valor = s }));
            lst.AddRange(busq.lstTp.Select(s => new OdbcParameterDTO() { nombre = "itp", tipo = OdbcType.VarChar, valor = s }));
            return lst;
        }
        List<RptBanMovDTO> setFormatEdoCtaToBanMov(List<sbEdoCtaChequeraDTO> lstEdoCtaChe, int formato)
        {
            var polizaDAO = new PolizaDAO();
            var lstCC = new CadenaProductivaDAO().lstObra();
            var lstTp = polizaDAO.getComboTipoPoliza();
            var lstTm = polizaDAO.getComboTipoMovimiento("B");
            var lstCta = new ChequesDAO().cboCuentaBanco();
            var lstBan = getLstBancos();
            var lstBanMov = lstEdoCtaChe.Select(edo => new RptBanMovDTO()
            {
                cuenta = edo.cuenta,
                fecha_mov = edo.fecha_mov,
                tm = edo.tm,
                numero = edo.numero,
                cc = edo.cc,
                st_che = edo.st_che ?? string.Empty,
                status_lp = edo.status_lp ?? string.Empty,
                ipoliza = edo.ipoliza,
                itp = edo.itp,
                ilinea = edo.ilinea,
                banco = edo.banco,
                descripcion = edo.descripcion,
                cargo = edo.naturaleza.Equals(1) || edo.naturaleza.Equals(3) ? edo.monto : 0,
                abono = edo.naturaleza.Equals(2) || edo.naturaleza.Equals(4) ? -edo.monto : 0,
                ccDesc = lstCC.FirstOrDefault(cc => cc.cc.Equals(edo.cc)).descripcion,
                ctaDesc = lstCta.FirstOrDefault(cta => cta.Value.Equals(edo.cuenta)).Text,
                tmDesc = lstTm.FirstOrDefault(tm => tm.Value.Equals(edo.tm.ToString())).Text,
                tpDesc = lstTp.FirstOrDefault(tp => tp.Value.Equals(edo.itp)).Text,
                banDesc = lstBan.FirstOrDefault(ban => ban.banco.Equals(edo.banco)).descripcion,
                stDesc = setStDesc(edo)
            }).ToList();
            switch (formato)
            {
                case (int)formatoRptBanMovEnum.Detalle:
                    lstBanMov = lstBanMov.OrderBy(o => o.cuenta).ThenBy(o => o.numero).ThenBy(o => o.tm).ToList();
                    break;
                case (int)formatoRptBanMovEnum.Resumen:
                    lstBanMov = lstBanMov.OrderBy(o => o.cuenta).ThenBy(o => o.tm).ToList();
                    break;
                case (int)formatoRptBanMovEnum.DetallePorCc:
                    lstBanMov = lstBanMov.OrderBy(o => o.cc).ThenBy(o => o.tm).ToList();
                    break;
                case (int)formatoRptBanMovEnum.ResumenPorCc:
                    lstBanMov = lstBanMov.OrderBy(o => o.cc).ThenBy(o => o.tm).ToList();
                    break;
                default: break;
            }
            return lstBanMov;
        }
        string setStDesc(sbEdoCtaChequeraDTO che)
        {
            che.st_che = che.st_che ?? string.Empty;
            che.status_lp = che.status_lp ?? string.Empty;
            var desc = string.Empty;
            if (che.st_che.Equals("C"))
            {
                desc = "Cancelado"; 
            }
            else if (che.status_lp.Equals("I"))
	        {
             desc = "Impreso";
	         }
            else {
                desc = "No Impreso";
            }
            return desc;
        }
        #endregion
        List<sbBancoDTO> getLstBancos()
        {
            try
            {
                var lst = _contextEnkontrol.Select<sbBancoDTO>(EnkontrolAmbienteEnum.Prod, "SELECT banco ,descripcion FROM sb_bancos");
                return lst;
            }
            catch (Exception o_O) { return new List<sbBancoDTO>(); }
        }
        #region combobox
        public List<ComboDTO> cboCuentaBanco()
        {
            var consulta = "SELECT cuenta AS Value, (CONVERT(varchar(4), cuenta)+' - '+descripcion) AS Text FROM sb_cuenta ORDER BY Value";
            var cbo = _contextEnkontrol.Select<ComboDTO>(EnkontrolAmbienteEnum.Prod, consulta);
            var rel = _context.tblC_RelCuentaDivision.ToList();
            cbo.ForEach(cta =>
            {
                var div = rel.Where(r => cta.Value.ParseInt().Equals(r.cuenta)).ToList();
                cta.Prefijo = JsonConvert.SerializeObject(div);
            });
            return cbo;
        }
        #endregion
    }
}
