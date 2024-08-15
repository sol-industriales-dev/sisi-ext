using Core.DAO.Contabilidad.Poliza;
using Core.DTO;
using Core.DTO.Contabilidad;
using Core.DTO.Contabilidad.Poliza;
using Core.DTO.Contabilidad.Poliza.ConversionPoliza;
using Core.DTO.Utils.Data;
using Core.Entity.Administrativo.Contabilidad.Cheque;
using Core.Entity.Administrativo.Contabilidad.Poliza;
using Core.Entity.Administrativo.Contabilidad.Sistema_Contable.Cuentas;
using Core.Enum.Multiempresa;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTO.Maquinaria.Reporte.ActivoFijo.PolizaDepreciacion;
using Core.Entity.Administrativo.Contabilidad.Sistema_Contable.CentroCostos;
using Core.Entity.Administrativo.Contabilidad.Sistema_Contable.iTiposMovimientos;
using Data.DAO.Principal.Usuarios;
using Core.Enum.Principal;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using Core.Entity.Administrativo.Contabilidad;
using Core.DTO.Enkontrol.Tablas.Cuenta;

namespace Data.DAO.Contabilidad.Poliza
{
    public class ConversionPolizaDAO : GenericDAO<tblPo_Poliza>, IConversionPolizaDAO
    {
        private readonly Dictionary<string, object> resultado = new Dictionary<string, object>();
        private const string nombreControlador = "ConversionPolizaController";
        private decimal pesosMexicanos = 0;
        private decimal solesDollar = 0;
        private decimal solesPesos = 0;

        public DateTime fechaConversion2;
        
        private List<string> CuentasSistemasValidos = new List<string> { "B" };
        public ConversionPolizaDAO()
        {
            resultado.Clear();
        }
        public Dictionary<string, object> CargarPolizas(int poliza, int year, int mes)
        {
            try
            {
                var res = _context.tblC_sc_polizas.Where(r => poliza != 0 ? r.poliza == poliza : true && r.year == year && r.mes == mes).ToList();
                resultado.Add("resultado", res);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                LogError(0, 0, nombreControlador, "CargarPolizas", e, AccionEnum.CONSULTA, 0, null);
            }
            return resultado;
        }

        public Dictionary<string, object> saveOrUpdatePoliza(List<tblC_SC_ConversionPoliza> listaPolizas, DateTime fechaConversion, bool aplicaFechaPoliza)
        {
            using (var dbTransaction = _context.Database.BeginTransaction())
                try
                {
                    fechaConversion2 = fechaConversion;
                    var ahora = DateTime.Now;
                    var tiposPolizas = new List<PolizasDTO>();
                    var lstGuardar = new List<tblC_SC_ConversionPoliza>();
                    var PolizasGuardadas = new List<tblC_sc_polizas>();
                    var palEmpresa = listaPolizas.FirstOrDefault().PalEmpresa;
                    var secEmpresa = listaPolizas.FirstOrDefault().SecEmpresa;
                    var conSec = _contextEnkontrol.BaseDatosProductivoDesdeEmpresa(secEmpresa);
                    var secYear = fechaConversion.Year;
                    var secMes = fechaConversion.Month;
                    asignarPesosColombianos(fechaConversion);
                    var cuentasColombia = CuentasColombiaCplan();
                    var obras = ObrasColombiaCplan();
                    var obrasPeru = ObrasPeruCplan();
                    var iTiposMov = iTiposMovimientoColombiaCplan();
                    var cuentasSistema = CuentasSistemasBancario(secEmpresa);
                    if (vSesiones.sesionEmpresaActual == 6)
                    {
                        cuentasColombia = CuentasPeruCplan();
                        iTiposMov = iTiposMovimientoPeruCplan();

                    }

                    var polizasPal = listaPolizas.Select(polPal => new PolizasDTO()
                    {
                        year = polPal.PalYear,
                        mes = polPal.PalMes,
                        tp = polPal.PalTP,
                        poliza = polPal.PalPoliza


                    }).ToList();
                    polizasPal = VerificacionRelaciones(polizasPal);
                    for (int j = 0; j < polizasPal.Count; j++)
                    {
                        var polizaPal = polizasPal[j];
                        var conexionODBC = new List<OdbcConsultaDTO>();
                        var polizaPalEnk = PolizaEnkontrol(polizaPal, palEmpresa);
                        var movimientoPolPalEnk = MovimientosPolizasEnkontrol(polizaPal, palEmpresa, fechaConversion);                      


                        #region Asignar nuevo numero poliza
                        var tipoPoliza = tiposPolizas.FirstOrDefault(numPol => numPol.tp == polizaPalEnk.tp) ?? new PolizasDTO();

                        if (vSesiones.sesionEmpresaActual == 6)
                        {
                            var tPoliza = _context.tblC_PolizaTP.FirstOrDefault(x => x.esActivo && x.PalTP == polizaPalEnk.tp);
                         
                                if (tPoliza != null)
                                {
                                    polizaPalEnk.tp = tPoliza.SecTP;
                                    tipoPoliza.poliza = UltimoNumPolizaCplan(polizaPalEnk.tp, fechaConversion);
                                    tipoPoliza.tp = polizaPalEnk.tp;
                                    tiposPolizas.Add(tipoPoliza);
                                }
                                else
                                {
                                    tipoPoliza.poliza++;
                                }                                                    
                        }
                        else
                        {

                            if (tipoPoliza.tp != polizaPalEnk.tp)
                            {
                                tipoPoliza.poliza = UltimoNumPolizaCplan(polizaPalEnk.tp, fechaConversion);
                                tipoPoliza.tp = polizaPalEnk.tp;
                                tiposPolizas.Add(tipoPoliza);
                            }
                            else
                            {
                                tipoPoliza.poliza++;
                            }
                        }

                        polizaPalEnk.poliza = tipoPoliza.poliza;
                        polizaPalEnk.year = secYear;
                        polizaPalEnk.mes = secMes;
                        polizaPalEnk.fechapol = fechaConversion;
                        polizaPalEnk.cargos =  convertirPesosColAPesosMX(polizaPalEnk.cargos);
                        polizaPalEnk.abonos = convertirPesosColAPesosMX(polizaPalEnk.abonos);
                        polizaPalEnk.fecha_hora_crea = ahora;
                        polizaPalEnk.fec_hora_movto = ahora;
                        polizaPalEnk.usuario_crea = new UsuarioDAO().getUserEk(vSesiones.sesionUsuarioDTO.id).empleado.ToString();
                        polizaPalEnk.usuario_movto = polizaPalEnk.usuario_movto == 0 ? 0 : polizaPalEnk.usuario_crea.ParseInt();
                        polizaPalEnk.concepto = string.IsNullOrEmpty(polizaPalEnk.concepto) ? string.Empty : polizaPalEnk.concepto;
                        #endregion

                        #region Asignacion cuentas
                        try
                        {
                            for (int i = 0; i < movimientoPolPalEnk.Count; i++)
                            {
                                var mov = movimientoPolPalEnk[i];
                                if (vSesiones.sesionEmpresaActual != 6)
                                {
                                    var cuentaColombia = cuentasColombia.FirstOrDefault(cta => cta.palCta == mov.cta && cta.palScta == mov.scta && cta.palSscta == mov.sscta);

                                    var cuentaSistema = cuentasSistema.FirstOrDefault(cta => cta.cuenta == cuentaColombia.secCta && CuentasSistemasValidos.Contains(cta.sistema)) ?? new CtaintDTO();

                                        var obra = string.IsNullOrEmpty(mov.cc) ? string.Empty : obras.FirstOrDefault(cc => cc.PalObra == mov.cc).SecObra;
                                        mov.cc = obra;


                                    var itmSec = iTiposMov.Any(tm => tm.PaliTm == mov.itm) ? iTiposMov.FirstOrDefault(tm => tm.PaliTm == mov.itm).SeciTm : 0;
                                    var itm = cuentaSistema.cuenta > 0 && itmSec > 0 ? itmSec : 0;

                                    mov.cta = cuentaColombia.secCta;
                                    mov.scta = cuentaColombia.secScta;
                                    mov.sscta = cuentaColombia.secSscta;

                                    mov.itm = itm;
                                    mov.poliza = tipoPoliza.poliza;
                                    mov.year = secYear;
                                    mov.mes = secMes;
                                    mov.numpro = 0;
                                    mov.monto = convertirPesosColAPesosMX(mov.monto);
                                }
                                else
                                {
                                    //var obraPeru = obrasPeru.FirstOrDefault(cc => cc.ccPrincipal == mov.cc) ?? new tblC_Cta_RelCC();
                                    //mov.cc = obraPeru.ToString();
                                    mov.tp = polizaPalEnk.tp;
                                    mov.monto = mov.monto;
                                }
                                mov.poliza = tipoPoliza.poliza;
                                mov.numpro = 0;

                            }                    


                            //for (int i = 0; i < movimientoPolPalEnk.Count; i++)
                            //{
                            //    var mov = movimientoPolPalEnk[i];                 

                            //    if (vSesiones.sesionEmpresaActual != 6)
                            //    {
                            //        var cuenta = cuentasColombia.FirstOrDefault(cta => cta.palCta == mov.cta && cta.palScta == mov.scta && cta.palSscta == mov.sscta);
                            //        var cuentaSistema = cuentasSistema.FirstOrDefault(cta => cta.cuenta == cuenta.secCta && CuentasSistemasValidos.Contains(cta.sistema)) ?? new CtaintDTO();

                            //        var obra = string.IsNullOrEmpty(mov.cc) ? string.Empty : obras.FirstOrDefault(cc => cc.PalObra == mov.cc).SecObra;
                            //        mov.cc = mov.cc; 

                            //        var itmSec = iTiposMov.Any(tm => tm.PaliTm == mov.itm) ? iTiposMov.FirstOrDefault(tm => tm.PaliTm == mov.itm).SeciTm : 0;
                            //        //if(vSesiones.sesionEmpresaActual==6)
                            //        //{
                            //        //    itmSec = _context.tblC_PolizaITM.FirstOrDefault(x => x.esActivo && x.PaliTm == mov.itm).SeciTm;
                            //        //}
                            //        var itm = cuentaSistema.cuenta > 0 && itmSec > 0 ? itmSec : 0;

                            //        mov.cta = cuenta.secCta;
                            //        mov.scta = cuenta.secScta;
                            //        mov.sscta = cuenta.secSscta;
                            //        mov.itm = itm;
                            //        mov.year = secYear;
                            //        mov.mes = secMes;
                            //    }
                            //    else
                            //    {
                            //        mov.tp = polizaPalEnk.tp;
                            //    }
                            //    mov.poliza = tipoPoliza.poliza;
                            //    mov.numpro = 0;

                            //    if (vSesiones.sesionEmpresaActual == 6)
                            //    {
                            //        mov.monto = mov.monto;
                            //    }
                            //    else
                            //    {
                            //        mov.monto = convertirPesosColAPesosMX(mov.monto);
                            //    }
                              
                            //}
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                        #endregion
                        var diff = polizaPalEnk.cargos + polizaPalEnk.abonos;
                        if (diff != 0)
                        {
                            polizaPalEnk.cargos -= diff;
                        }
                        var movCargos = movimientoPolPalEnk.Where(w => w.tm == 1 || w.tm == 3).Sum(s => s.monto);
                        var movAbonos = movimientoPolPalEnk.Where(w => w.tm == 2 || w.tm == 4).Sum(s => s.monto);
                        var diffMov = movCargos + movAbonos;
                        if (diffMov != 0)
                        {
                            var movAjuste = movimientoPolPalEnk.FirstOrDefault(mov => mov.monto == movimientoPolPalEnk.Max(m => m.monto));
                            movAjuste.monto -= diffMov;
                            movCargos = movimientoPolPalEnk.Where(w => w.tm == 1 || w.tm == 3).Sum(s => s.monto);
                            movAbonos = movimientoPolPalEnk.Where(w => w.tm == 2 || w.tm == 4).Sum(s => s.monto);
                            diffMov = movCargos + movAbonos;
                        }
                        conexionODBC.Add(consultaInsertPoliza(polizaPalEnk));
                        conexionODBC.AddRange(consultaInsertMovPoliza(movimientoPolPalEnk));
                        if (diffMov == 0)
                        {
                            var guardado = _contextEnkontrol.Save(conSec, conexionODBC);
                            //var guardado = _contextEnkontrol.Save(EnkontrolEnum.PruebaCplanProd, conexionODBC);
                            if (guardado.All(g => g == 1))
                            {
                                lstGuardar.Add(listaPolizas[j]);
                                PolizasGuardadas.Add(polizaPalEnk);
                            }
                        }
                        else
                        {

                        }
                    }
                    //var tipoMovPeru = "03";

                    //SetDefault Conversion peru Construplan
                    //var tPoliza = _context.tblC_PolizaTP.FirstOrDefault(x => x.esActivo && x.PalTP == polizasPal.ToString()).SecTP;
                    for (int i = 0; i < PolizasGuardadas.Count; i++)
                    {
                        var conv = lstGuardar[i];
                        var _bd = _context.tblC_SC_ConversionPoliza.FirstOrDefault(cp => cp.esActivo && cp.PalEmpresa == conv.PalEmpresa && cp.PalYear == conv.PalYear && cp.PalMes == conv.PalMes && cp.PalTP == conv.PalTP && cp.PalPoliza == conv.PalPoliza) ?? new tblC_SC_ConversionPoliza();


                        if (_bd.Id == 0)
                        {
                            var polizaSec = PolizasGuardadas[i];
                            _bd = new tblC_SC_ConversionPoliza
                            {
                                PalEmpresa = conv.PalEmpresa,
                                PalYear = conv.PalYear,
                                PalMes = conv.PalMes,
                                PalTP = conv.PalTP,
                                PalPoliza = conv.PalPoliza,
                                PalCargo = conv.PalCargo,
                                PalAbono = conv.PalAbono,
                                SecEmpresa = conv.SecEmpresa,
                                SecYear = polizaSec.year,
                                SecMes = polizaSec.mes,
                                SecTP = polizaSec.tp,
                                SecPoliza = polizaSec.poliza,
                                SecCargo = polizaSec.cargos,
                                SecAbono = polizaSec.abonos
                            };
                            _bd.registrar();
                            _context.tblC_SC_ConversionPoliza.Add(_bd);
                            _context.SaveChanges();
                        }
                    }
                    dbTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    LogError(0, 0, nombreControlador, "saveOrUpdatePoliza", e, AccionEnum.AGREGAR, 0, null);
                }
            return resultado;
        }

        private OdbcConsultaDTO consultaInsertPoliza(tblC_sc_polizas poliza)
        {
            var polConsulta = new OdbcConsultaDTO
            {
                consulta = "INSERT INTO sc_polizas (year ,mes ,poliza ,tp ,fechapol ,cargos ,abonos ,generada ,status ,status_lock ,fec_hora_movto ,usuario_movto ,fecha_hora_crea ,usuario_crea ,concepto ,error) VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)"
            };

            polConsulta.parametros.Add(new OdbcParameterDTO { nombre = "year", tipo = OdbcType.Numeric, valor = poliza.year });
            polConsulta.parametros.Add(new OdbcParameterDTO { nombre = "mes", tipo = OdbcType.Numeric, valor = poliza.mes });
            polConsulta.parametros.Add(new OdbcParameterDTO { nombre = "poliza", tipo = OdbcType.Numeric, valor = poliza.poliza });
            polConsulta.parametros.Add(new OdbcParameterDTO { nombre = "tp", tipo = OdbcType.Char, valor = poliza.tp });
            polConsulta.parametros.Add(new OdbcParameterDTO { nombre = "fechapol", tipo = OdbcType.Date, valor = poliza.fechapol });
            polConsulta.parametros.Add(new OdbcParameterDTO { nombre = "cargos", tipo = OdbcType.Numeric, valor = poliza.cargos });
            polConsulta.parametros.Add(new OdbcParameterDTO { nombre = "abonos", tipo = OdbcType.Numeric, valor = poliza.abonos });
            polConsulta.parametros.Add(new OdbcParameterDTO { nombre = "generada", tipo = OdbcType.Char, valor = poliza.generada });
            polConsulta.parametros.Add(new OdbcParameterDTO { nombre = "status", tipo = OdbcType.Char, valor = poliza.status ?? string.Empty });
            polConsulta.parametros.Add(new OdbcParameterDTO { nombre = "status_lock", tipo = OdbcType.Char, valor = poliza.status_lock });
            polConsulta.parametros.Add(new OdbcParameterDTO { nombre = "fec_hora_movto", tipo = OdbcType.DateTime, valor = DateTime.Now });
            polConsulta.parametros.Add(new OdbcParameterDTO { nombre = "usuario_movto", tipo = OdbcType.Char, valor = poliza.usuario_movto });
            polConsulta.parametros.Add(new OdbcParameterDTO { nombre = "fecha_hora_crea", tipo = OdbcType.DateTime, valor = DateTime.Now });
            polConsulta.parametros.Add(new OdbcParameterDTO { nombre = "usuario_crea", tipo = OdbcType.Char, valor = poliza.usuario_crea });
            polConsulta.parametros.Add(new OdbcParameterDTO { nombre = "concepto", tipo = OdbcType.VarChar, valor = poliza.concepto });
            polConsulta.parametros.Add(new OdbcParameterDTO { nombre = "error", tipo = OdbcType.VarChar, valor = string.Empty });
            polConsulta.parametros.Add(new OdbcParameterDTO { nombre = "status_carga_pol", tipo = OdbcType.VarChar, valor = DBNull.Value });

            return polConsulta;
        }
        private List<OdbcConsultaDTO> consultaInsertMovPoliza(List<tblC_sc_movpol> lstMovPol)
        {
            var lstmovConsulta = new List<OdbcConsultaDTO>();
            lstMovPol.ForEach(mov =>
            {
                var movConsulta = new OdbcConsultaDTO
                {
                    consulta = @"INSERT INTO sc_movpol 
                                 (year, mes, poliza, tp, linea, cta, scta, sscta, digito, tm, referencia, cc, concepto, monto, iclave, itm, st_par, orden_compra, numpro, area,cuenta_oc)
                                                VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)"
                };
                movConsulta.parametros.Add(new OdbcParameterDTO { nombre = "year", tipo = OdbcType.Numeric, valor = mov.year });
                movConsulta.parametros.Add(new OdbcParameterDTO { nombre = "mes", tipo = OdbcType.Numeric, valor = mov.mes });
                movConsulta.parametros.Add(new OdbcParameterDTO { nombre = "poliza", tipo = OdbcType.Numeric, valor = mov.poliza });
                movConsulta.parametros.Add(new OdbcParameterDTO { nombre = "tp", tipo = OdbcType.Char, valor = mov.tp });
                movConsulta.parametros.Add(new OdbcParameterDTO { nombre = "linea", tipo = OdbcType.Numeric, valor = mov.linea });
                movConsulta.parametros.Add(new OdbcParameterDTO { nombre = "cta", tipo = OdbcType.Numeric, valor = mov.cta });
                movConsulta.parametros.Add(new OdbcParameterDTO { nombre = "scta", tipo = OdbcType.Numeric, valor = mov.scta });
                movConsulta.parametros.Add(new OdbcParameterDTO { nombre = "sscta", tipo = OdbcType.Numeric, valor = mov.sscta });
                movConsulta.parametros.Add(new OdbcParameterDTO { nombre = "digito", tipo = OdbcType.Numeric, valor = mov.digito });
                movConsulta.parametros.Add(new OdbcParameterDTO { nombre = "tm", tipo = OdbcType.Numeric, valor = mov.tm });
                movConsulta.parametros.Add(new OdbcParameterDTO { nombre = "referencia", tipo = OdbcType.Char, valor = mov.referencia });
                movConsulta.parametros.Add(new OdbcParameterDTO { nombre = "cc", tipo = OdbcType.Char, valor = mov.cc });
                movConsulta.parametros.Add(new OdbcParameterDTO { nombre = "concepto", tipo = OdbcType.Char, valor = mov.concepto });
                movConsulta.parametros.Add(new OdbcParameterDTO { nombre = "monto", tipo = OdbcType.Numeric, valor = mov.monto });
                movConsulta.parametros.Add(new OdbcParameterDTO { nombre = "iclave", tipo = OdbcType.Numeric, valor = 0 });
                movConsulta.parametros.Add(new OdbcParameterDTO { nombre = "itm", tipo = OdbcType.Numeric, valor = mov.itm });
                movConsulta.parametros.Add(new OdbcParameterDTO { nombre = "st_par", tipo = OdbcType.Char, valor = string.Empty });
                movConsulta.parametros.Add(new OdbcParameterDTO { nombre = "orden_compra", tipo = OdbcType.Numeric, valor = mov.orden_compra });
                movConsulta.parametros.Add(new OdbcParameterDTO { nombre = "numpro", tipo = OdbcType.Numeric, valor = mov.numpro });
                movConsulta.parametros.Add(new OdbcParameterDTO { nombre = "area", tipo = OdbcType.Numeric, valor = mov.area });
                movConsulta.parametros.Add(new OdbcParameterDTO { nombre = "cuenta_oc", tipo = OdbcType.Numeric, valor = mov.cuenta_oc });
                lstmovConsulta.Add(movConsulta);
            });
            return lstmovConsulta;
        }
        private tblC_sc_polizas PolizaEnkontrol(PolizasDTO item, EmpresaEnum empresa)
        {
            //var conexion = empresa == EmpresaEnum.Construplan ? EnkontrolEnum.PruebaCplanProd : _contextEnkontrol.BaseDatosProductivoDesdeEmpresa(empresa);
            //var conexion = _contextEnkontrol.BaseDatosProductivoDesdeEmpresa(empresa);
            //var odbc = new OdbcConsultaDTO();
            //if (vSesiones.sesionEmpresaActual == 6)
            //{
            //    var tPoliza = _context.tblC_PolizaTP.FirstOrDefault(x => x.esActivo && x.PalTP == item.tp).SecTP;
            //    item.tp = tPoliza;
            //}

            if (vSesiones.sesionEmpresaActual == 6)
            {
                DynamicParameters lstParametros = new DynamicParameters();
                var obrasPeru = ObrasPeruCplan();
                var cuentas = CuentasPeruCplan();
                var cuentasSistema = CuentasSistemasBancario(EmpresaEnum.Construplan);
                var iTiposMovPeru = iTiposMovimientoPeruCplan();
                var anioPoliza = item.year;
//                                var sql = @"
//                             SELECT
//                                2022 AS year,
//                                CAST(12 as int) AS mes,
//                                CAST(DMOV_C_COMPR AS int) AS poliza,
//                                SUBDIAR_CODIGO AS tp,    
//                                Convert(datetime,(DMOV_FECHA)) AS fechapol,
//                                SUM(DMOV_DEBE) AS cargos,
//                                SUM(DMOV_HABER) AS abonos,
//                                '' as generada,
//                                'C' AS status,
//                                'N' AS status_lock,
//                                MAX(DMOV_GLOSA) AS concepto,         
//                                '' as status_carga_pol,
//                                '' as usuario_crea         
//                            FROM [003BDCONT2022].dbo.DETMOV12 GROUP BY SUBDIAR_CODIGO, DMOV_C_COMPR,DMOV_CUENT,DMOV_FECHA";


                // bd cMov_Banco  
                var sql = string.Format(@"              			  
							SELECT
                                {0} AS year,
                                CAST(01 as int) AS mes,
                                CAST(DMOV_C_COMPR AS int) AS poliza,
                                SUBDIAR_CODIGO AS tp,    
                                Convert(datetime,(DMOV_FECHA)) AS fechapol,
                                SUM(DMOV_DEBE) AS cargos,
                                SUM(DMOV_HABER) AS abonos,
                                '' as generada,
                                'C' AS status,
                                'N' AS status_lock,
                                MAX(DMOV_GLOSA) AS concepto,         
                                '' as status_carga_pol,
                                '' as usuario_crea         
                            FROM [003BDCONT{0}].dbo.DETMOV01 GROUP BY SUBDIAR_CODIGO, DMOV_C_COMPR,DMOV_CUENT,DMOV_FECHA
						    UNION
						   	SELECT
                                {0} AS year,
                                CAST(02 as int) AS mes,
                                CAST(DMOV_C_COMPR AS int) AS poliza,
                                SUBDIAR_CODIGO AS tp,    
                                Convert(datetime,(DMOV_FECHA)) AS fechapol,
                                SUM(DMOV_DEBE) AS cargos,
                                SUM(DMOV_HABER) AS abonos,
                                '' as generada,
                                'C' AS status,
                                'N' AS status_lock,
                                MAX(DMOV_GLOSA) AS concepto,         
                                '' as status_carga_pol,
                                '' as usuario_crea         
                             FROM [003BDCONT{0}].dbo.DETMOV02 GROUP BY SUBDIAR_CODIGO, DMOV_C_COMPR,DMOV_CUENT,DMOV_FECHA
						     UNION
						   	 SELECT
                                {0} AS year,
                                CAST(03 as int) AS mes,
                                CAST(DMOV_C_COMPR AS int) AS poliza,
                                SUBDIAR_CODIGO AS tp,    
                                Convert(datetime,(DMOV_FECHA)) AS fechapol,
                                SUM(DMOV_DEBE) AS cargos,
                                SUM(DMOV_HABER) AS abonos,
                                '' as generada,
                                'C' AS status,
                                'N' AS status_lock,
                                MAX(DMOV_GLOSA) AS concepto,         
                                '' as status_carga_pol,
                                '' as usuario_crea         
                            FROM [003BDCONT{0}].dbo.DETMOV03 GROUP BY SUBDIAR_CODIGO, DMOV_C_COMPR,DMOV_CUENT,DMOV_FECHA
						  	UNION
						    SELECT
                                {0} AS year,
                                CAST(04 as int) AS mes,
                                CAST(DMOV_C_COMPR AS int) AS poliza,
                                SUBDIAR_CODIGO AS tp,    
                                Convert(datetime,(DMOV_FECHA)) AS fechapol,
                                SUM(DMOV_DEBE) AS cargos,
                                SUM(DMOV_HABER) AS abonos,
                                '' as generada,
                                'C' AS status,
                                'N' AS status_lock,
                                MAX(DMOV_GLOSA) AS concepto,         
                                '' as status_carga_pol,
                                '' as usuario_crea         
                             FROM [003BDCONT{0}].dbo.DETMOV04 GROUP BY SUBDIAR_CODIGO, DMOV_C_COMPR,DMOV_CUENT,DMOV_FECHA
						  	 UNION
						   	 SELECT
                                {0} AS year,
                                CAST(05 as int) AS mes,
                                CAST(DMOV_C_COMPR AS int) AS poliza,
                                SUBDIAR_CODIGO AS tp,    
                                Convert(datetime,(DMOV_FECHA)) AS fechapol,
                                SUM(DMOV_DEBE) AS cargos,
                                SUM(DMOV_HABER) AS abonos,
                                '' as generada,
                                'C' AS status,
                                'N' AS status_lock,
                                MAX(DMOV_GLOSA) AS concepto,         
                                '' as status_carga_pol,
                                '' as usuario_crea         
                             FROM [003BDCONT{0}].dbo.DETMOV05 GROUP BY SUBDIAR_CODIGO, DMOV_C_COMPR,DMOV_CUENT,DMOV_FECHA
						  	 UNION
						     SELECT
                                {0} AS year,
                                CAST(06 as int) AS mes,
                                CAST(DMOV_C_COMPR AS int) AS poliza,
                                SUBDIAR_CODIGO AS tp,    
                                Convert(datetime,(DMOV_FECHA)) AS fechapol,
                                SUM(DMOV_DEBE) AS cargos,
                                SUM(DMOV_HABER) AS abonos,
                                '' as generada,
                                'C' AS status,
                                'N' AS status_lock,
                                MAX(DMOV_GLOSA) AS concepto,         
                                '' as status_carga_pol,
                                '' as usuario_crea         
                             FROM [003BDCONT{0}].dbo.DETMOV06 GROUP BY SUBDIAR_CODIGO, DMOV_C_COMPR,DMOV_CUENT,DMOV_FECHA
						  	 UNION
						   	 SELECT
                                {0} AS year,
                                CAST(07 as int) AS mes,
                                CAST(DMOV_C_COMPR AS int) AS poliza,
                                SUBDIAR_CODIGO AS tp,    
                                Convert(datetime,(DMOV_FECHA)) AS fechapol,
                                SUM(DMOV_DEBE) AS cargos,
                                SUM(DMOV_HABER) AS abonos,
                                '' as generada,
                                'C' AS status,
                                'N' AS status_lock,
                                MAX(DMOV_GLOSA) AS concepto,         
                                '' as status_carga_pol,
                                '' as usuario_crea         
                             FROM [003BDCONT{0}].dbo.DETMOV07 GROUP BY SUBDIAR_CODIGO, DMOV_C_COMPR,DMOV_CUENT,DMOV_FECHA
						  	 UNION
						   	 SELECT
                                {0} AS year,
                                CAST(08 as int) AS mes,
                                CAST(DMOV_C_COMPR AS int) AS poliza,
                                SUBDIAR_CODIGO AS tp,    
                                Convert(datetime,(DMOV_FECHA)) AS fechapol,
                                SUM(DMOV_DEBE) AS cargos,
                                SUM(DMOV_HABER) AS abonos,
                                '' as generada,
                                'C' AS status,
                                'N' AS status_lock,
                                MAX(DMOV_GLOSA) AS concepto,         
                                '' as status_carga_pol,
                                '' as usuario_crea         
                            FROM [003BDCONT{0}].dbo.DETMOV08 GROUP BY SUBDIAR_CODIGO, DMOV_C_COMPR,DMOV_CUENT,DMOV_FECHA
						  	UNION
						   	SELECT
                                {0} AS year,
                                CAST(09 as int) AS mes,
                                CAST(DMOV_C_COMPR AS int) AS poliza,
                                SUBDIAR_CODIGO AS tp,    
                                Convert(datetime,(DMOV_FECHA)) AS fechapol,
                                SUM(DMOV_DEBE) AS cargos,
                                SUM(DMOV_HABER) AS abonos,
                                '' as generada,
                                'C' AS status,
                                'N' AS status_lock,
                                MAX(DMOV_GLOSA) AS concepto,         
                                '' as status_carga_pol,
                                '' as usuario_crea         
                            FROM [003BDCONT{0}].dbo.DETMOV09 GROUP BY SUBDIAR_CODIGO, DMOV_C_COMPR,DMOV_CUENT,DMOV_FECHA
						  	UNION
						    SELECT
                                {0} AS year,
                                CAST(10 as int) AS mes,
                                CAST(DMOV_C_COMPR AS int) AS poliza,
                                SUBDIAR_CODIGO AS tp,    
                                Convert(datetime,(DMOV_FECHA)) AS fechapol,
                                SUM(DMOV_DEBE) AS cargos,
                                SUM(DMOV_HABER) AS abonos,
                                '' as generada,
                                'C' AS status,
                                'N' AS status_lock,
                                MAX(DMOV_GLOSA) AS concepto,         
                                '' as status_carga_pol,
                                '' as usuario_crea         
                             FROM [003BDCONT{0}].dbo.DETMOV10 GROUP BY SUBDIAR_CODIGO, DMOV_C_COMPR,DMOV_CUENT,DMOV_FECHA
						  	 UNION
						     SELECT
                                {0} AS year,
                                CAST(11 as int) AS mes,
                                CAST(DMOV_C_COMPR AS int) AS poliza,
                                SUBDIAR_CODIGO AS tp,    
                                Convert(datetime,(DMOV_FECHA)) AS fechapol,
                                SUM(DMOV_DEBE) AS cargos,
                                SUM(DMOV_HABER) AS abonos,
                                '' as generada,
                                'C' AS status,
                                'N' AS status_lock,
                                MAX(DMOV_GLOSA) AS concepto,         
                                '' as status_carga_pol,
                                '' as usuario_crea         
                            FROM [003BDCONT{0}].dbo.DETMOV11 GROUP BY SUBDIAR_CODIGO, DMOV_C_COMPR,DMOV_CUENT,DMOV_FECHA
						  	UNION
						   	SELECT
                                {0} AS year,
                                CAST(12 as int) AS mes,
                                CAST(DMOV_C_COMPR AS int) AS poliza,
                                SUBDIAR_CODIGO AS tp,    
                                Convert(datetime,(DMOV_FECHA)) AS fechapol,
                                SUM(DMOV_DEBE) AS cargos,
                                SUM(DMOV_HABER) AS abonos,
                                '' as generada,
                                'C' AS status,
                                'N' AS status_lock,
                                MAX(DMOV_GLOSA) AS concepto,         
                                '' as status_carga_pol,
                                '' as usuario_crea         
                          FROM [003BDCONT{0}].dbo.DETMOV12 GROUP BY SUBDIAR_CODIGO, DMOV_C_COMPR,DMOV_CUENT,DMOV_FECHA", anioPoliza);
                


                List<tblC_sc_polizas> lstMovPoliza = new List<tblC_sc_polizas>();
                using (var conexion = new SqlConnection(ConextSigoDapper.conexionStarsoftBancos()))
                {
                    conexion.Open();
                    lstMovPoliza = conexion.Query<tblC_sc_polizas>(sql, lstParametros, null, true, 300, commandType: CommandType.Text).Where(x => x.year == item.year && x.mes == item.mes && x.poliza == item.poliza && x.tp == item.tp).ToList();
                    conexion.Close();
                }
                return lstMovPoliza.FirstOrDefault();
            }
            else
            {
                var conexion = _contextEnkontrol.BaseDatosProductivoDesdeEmpresa(empresa);
                var odbc = new OdbcConsultaDTO();
                odbc.consulta = @"SELECT * FROM ""DBA"".""sc_polizas"" WHERE year = ? AND mes = ? AND poliza = ? AND tp = ?";
                odbc.parametros.Add(new OdbcParameterDTO()
                {
                    nombre = "year",
                    tipo = OdbcType.VarChar,
                    valor = item.year
                });
                odbc.parametros.Add(new OdbcParameterDTO()
                {
                    nombre = "mes",
                    tipo = OdbcType.VarChar,
                    valor = item.mes
                });
                odbc.parametros.Add(new OdbcParameterDTO()
                {
                    nombre = "poliza",
                    tipo = OdbcType.VarChar,
                    valor = item.poliza
                });
                odbc.parametros.Add(new OdbcParameterDTO()
                {
                    nombre = "tp",
                    tipo = OdbcType.VarChar,
                    valor = item.tp
                });

                var resultado = _contextEnkontrol.Select<tblC_sc_polizas>(conexion, odbc);
                return resultado.FirstOrDefault();
            }
        }

        private string queryPolizaDetallePeru(int anio) 
        {
            return string.Format(@"                           
						 SELECT
	                        {0} AS year,
	                        1 AS mes,	                       
	                        CAST(DMOV_C_COMPR AS int) AS poliza,
	                        CAST(DMOV_SECUE AS int) AS linea,
                            SUBDIAR_CODIGO AS tp,
	                        DMOV_CUENT AS cta,
	                        0 AS sscta,
	                        0 AS sscta,
	                        0 AS digito,
                            CASE WHEN DMOV_DEBE <> 0 THEN (CASE WHEN DMOV_DEBE > 0 THEN 1 ELSE 3 END) ELSE (CASE WHEN DMOV_HABER <> 0 THEN (CASE WHEN DMOV_HABER > 0 THEN 2 ELSE 4 END) ELSE 1 END) END AS tm,
	                        CASE WHEN (SUBDIAR_CODIGO = '09' AND DMOV_GLOSA like 'Consumo - %' AND DMOV_CENCO IS NOT NULL) THEN DMOV_CENCO ELSE DMOV_GLOSA END AS cc,
	                        DMOV_GLOSA AS concepto,
                            DMOV_DOCUM as referencia,
	                        DMOV_DEBE - DMOV_HABER AS monto,
                            DMOV_DEBUS - DMOV_HABUS as montoUs,
                            'C' AS status,
	                        0 AS iclave,
	                        0 AS itm,
	                        NULL AS numpro,
	                        Convert(datetime,(DMOV_FECHA)) AS fechapol,
	                        NULL AS area,
	                        NULL AS cuenta_oc,
	                        6 AS empresa,
	                        GETDATE() as fecha,
	                        LEFT(DMOV_DOCUM, 2) AS itmPeru,
                            RIGHT('00000000000'+ RTRIM(ISNULL(DMOV_ANEXO, '0')), 11) AS proveedorPeru
                        FROM [003BDCONT{0}].dbo.DETMOV01
						UNION
						SELECT
	                        {0} AS year,
	                        2 AS mes,	                       
	                        CAST(DMOV_C_COMPR AS int) AS poliza,
	                        CAST(DMOV_SECUE AS int) AS linea,
                            SUBDIAR_CODIGO AS tp,
	                        DMOV_CUENT AS cta,
	                        0 AS sscta,
	                        0 AS sscta,
	                        0 AS digito,
                            CASE WHEN DMOV_DEBE <> 0 THEN (CASE WHEN DMOV_DEBE > 0 THEN 1 ELSE 3 END) ELSE (CASE WHEN DMOV_HABER <> 0 THEN (CASE WHEN DMOV_HABER > 0 THEN 2 ELSE 4 END) ELSE 1 END) END AS tm,
	                        CASE WHEN (SUBDIAR_CODIGO = '09' AND DMOV_GLOSA like 'Consumo - %' AND DMOV_CENCO IS NOT NULL) THEN DMOV_CENCO ELSE DMOV_GLOSA END AS cc,
	                        DMOV_GLOSA AS concepto,
                            DMOV_DOCUM as referencia,
	                        DMOV_DEBE - DMOV_HABER AS monto,
                            DMOV_DEBUS - DMOV_HABUS as montoUs,
                            'C' AS status,
	                        0 AS iclave,
	                        0 AS itm,
	                        NULL AS numpro,
	                        Convert(datetime,(DMOV_FECHA)) AS fechapol,
	                        NULL AS area,
	                        NULL AS cuenta_oc,
	                        6 AS empresa,
	                        GETDATE() as fecha,
	                        LEFT(DMOV_DOCUM, 2) AS itmPeru,
                            RIGHT('00000000000'+ RTRIM(ISNULL(DMOV_ANEXO, '0')), 11) AS proveedorPeru
                        FROM [003BDCONT{0}].dbo.DETMOV02
						UNION
						SELECT
	                        {0} AS year,
	                        3 AS mes,	                       
	                        CAST(DMOV_C_COMPR AS int) AS poliza,
	                        CAST(DMOV_SECUE AS int) AS linea,
                            SUBDIAR_CODIGO AS tp,
	                        DMOV_CUENT AS cta,
	                        0 AS sscta,
	                        0 AS sscta,
	                        0 AS digito,
                            CASE WHEN DMOV_DEBE <> 0 THEN (CASE WHEN DMOV_DEBE > 0 THEN 1 ELSE 3 END) ELSE (CASE WHEN DMOV_HABER <> 0 THEN (CASE WHEN DMOV_HABER > 0 THEN 2 ELSE 4 END) ELSE 1 END) END AS tm,
	                        CASE WHEN (SUBDIAR_CODIGO = '09' AND DMOV_GLOSA like 'Consumo - %' AND DMOV_CENCO IS NOT NULL) THEN DMOV_CENCO ELSE DMOV_GLOSA END AS cc,
	                        DMOV_GLOSA AS concepto,
                            DMOV_DOCUM as referencia,
	                        DMOV_DEBE - DMOV_HABER AS monto,
                            DMOV_DEBUS - DMOV_HABUS as montoUs,
                            'C' AS status,
	                        0 AS iclave,
	                        0 AS itm,
	                        NULL AS numpro,
	                        Convert(datetime,(DMOV_FECHA)) AS fechapol,
	                        NULL AS area,
	                        NULL AS cuenta_oc,
	                        6 AS empresa,
	                        GETDATE() as fecha,
	                        LEFT(DMOV_DOCUM, 2) AS itmPeru,
                            RIGHT('00000000000'+ RTRIM(ISNULL(DMOV_ANEXO, '0')), 11) AS proveedorPeru
                        FROM [003BDCONT{0}].dbo.DETMOV03
						UNION
						SELECT
	                        {0} AS year,
	                        4 AS mes,	                       
	                        CAST(DMOV_C_COMPR AS int) AS poliza,
	                        CAST(DMOV_SECUE AS int) AS linea,
                            SUBDIAR_CODIGO AS tp,
	                        DMOV_CUENT AS cta,
	                        0 AS sscta,
	                        0 AS sscta,
	                        0 AS digito,
                            CASE WHEN DMOV_DEBE <> 0 THEN (CASE WHEN DMOV_DEBE > 0 THEN 1 ELSE 3 END) ELSE (CASE WHEN DMOV_HABER <> 0 THEN (CASE WHEN DMOV_HABER > 0 THEN 2 ELSE 4 END) ELSE 1 END) END AS tm,
	                        CASE WHEN (SUBDIAR_CODIGO = '09' AND DMOV_GLOSA like 'Consumo - %' AND DMOV_CENCO IS NOT NULL) THEN DMOV_CENCO ELSE DMOV_GLOSA END AS cc,
	                        DMOV_GLOSA AS concepto,
                            DMOV_DOCUM as referencia,
	                        DMOV_DEBE - DMOV_HABER AS monto,
                            DMOV_DEBUS - DMOV_HABUS as montoUs,
                            'C' AS status,
	                        0 AS iclave,
	                        0 AS itm,
	                        NULL AS numpro,
	                        Convert(datetime,(DMOV_FECHA)) AS fechapol,
	                        NULL AS area,
	                        NULL AS cuenta_oc,
	                        6 AS empresa,
	                        GETDATE() as fecha,
	                        LEFT(DMOV_DOCUM, 2) AS itmPeru,
                            RIGHT('00000000000'+ RTRIM(ISNULL(DMOV_ANEXO, '0')), 11) AS proveedorPeru
                        FROM [003BDCONT{0}].dbo.DETMOV04
						UNION
						SELECT
	                        {0} AS year,
	                        5 AS mes,	                       
	                        CAST(DMOV_C_COMPR AS int) AS poliza,
	                        CAST(DMOV_SECUE AS int) AS linea,
                            SUBDIAR_CODIGO AS tp,
	                        DMOV_CUENT AS cta,
	                        0 AS sscta,
	                        0 AS sscta,
	                        0 AS digito,
                            CASE WHEN DMOV_DEBE <> 0 THEN (CASE WHEN DMOV_DEBE > 0 THEN 1 ELSE 3 END) ELSE (CASE WHEN DMOV_HABER <> 0 THEN (CASE WHEN DMOV_HABER > 0 THEN 2 ELSE 4 END) ELSE 1 END) END AS tm,
	                        CASE WHEN (SUBDIAR_CODIGO = '09' AND DMOV_GLOSA like 'Consumo - %' AND DMOV_CENCO IS NOT NULL) THEN DMOV_CENCO ELSE DMOV_GLOSA END AS cc,
	                        DMOV_GLOSA AS concepto,
                            DMOV_DOCUM as referencia,
	                        DMOV_DEBE - DMOV_HABER AS monto,
                            DMOV_DEBUS - DMOV_HABUS as montoUs,
                            'C' AS status,
	                        0 AS iclave,
	                        0 AS itm,
	                        NULL AS numpro,
	                        Convert(datetime,(DMOV_FECHA)) AS fechapol,
	                        NULL AS area,
	                        NULL AS cuenta_oc,
	                        6 AS empresa,
	                        GETDATE() as fecha,
	                        LEFT(DMOV_DOCUM, 2) AS itmPeru,
                            RIGHT('00000000000'+ RTRIM(ISNULL(DMOV_ANEXO, '0')), 11) AS proveedorPeru
                        FROM [003BDCONT{0}].dbo.DETMOV05
						UNION
						SELECT
	                        {0} AS year,
	                        6 AS mes,	                       
	                        CAST(DMOV_C_COMPR AS int) AS poliza,
	                        CAST(DMOV_SECUE AS int) AS linea,
                            SUBDIAR_CODIGO AS tp,
	                        DMOV_CUENT AS cta,
	                        0 AS sscta,
	                        0 AS sscta,
	                        0 AS digito,
                            CASE WHEN DMOV_DEBE <> 0 THEN (CASE WHEN DMOV_DEBE > 0 THEN 1 ELSE 3 END) ELSE (CASE WHEN DMOV_HABER <> 0 THEN (CASE WHEN DMOV_HABER > 0 THEN 2 ELSE 4 END) ELSE 1 END) END AS tm,
	                        CASE WHEN (SUBDIAR_CODIGO = '09' AND DMOV_GLOSA like 'Consumo - %' AND DMOV_CENCO IS NOT NULL) THEN DMOV_CENCO ELSE DMOV_GLOSA END AS cc,
	                        DMOV_GLOSA AS concepto,
                            DMOV_DOCUM as referencia,
	                        DMOV_DEBE - DMOV_HABER AS monto,
                            DMOV_DEBUS - DMOV_HABUS as montoUs,
                            'C' AS status,
	                        0 AS iclave,
	                        0 AS itm,
	                        NULL AS numpro,
	                        Convert(datetime,(DMOV_FECHA)) AS fechapol,
	                        NULL AS area,
	                        NULL AS cuenta_oc,
	                        6 AS empresa,
	                        GETDATE() as fecha,
	                        LEFT(DMOV_DOCUM, 2) AS itmPeru,
                            RIGHT('00000000000'+ RTRIM(ISNULL(DMOV_ANEXO, '0')), 11) AS proveedorPeru
                        FROM [003BDCONT{0}].dbo.DETMOV06
						UNION
						SELECT
	                        {0} AS year,
	                        7 AS mes,	                       
	                        CAST(DMOV_C_COMPR AS int) AS poliza,
	                        CAST(DMOV_SECUE AS int) AS linea,
                            SUBDIAR_CODIGO AS tp,
	                        DMOV_CUENT AS cta,
	                        0 AS sscta,
	                        0 AS sscta,
	                        0 AS digito,
                            CASE WHEN DMOV_DEBE <> 0 THEN (CASE WHEN DMOV_DEBE > 0 THEN 1 ELSE 3 END) ELSE (CASE WHEN DMOV_HABER <> 0 THEN (CASE WHEN DMOV_HABER > 0 THEN 2 ELSE 4 END) ELSE 1 END) END AS tm,
	                        CASE WHEN (SUBDIAR_CODIGO = '09' AND DMOV_GLOSA like 'Consumo - %' AND DMOV_CENCO IS NOT NULL) THEN DMOV_CENCO ELSE DMOV_GLOSA END AS cc,
	                        DMOV_GLOSA AS concepto,
                            DMOV_DOCUM as referencia,
	                        DMOV_DEBE - DMOV_HABER AS monto,
                            DMOV_DEBUS - DMOV_HABUS as montoUs,
                            'C' AS status,
	                        0 AS iclave,
	                        0 AS itm,
	                        NULL AS numpro,
	                        Convert(datetime,(DMOV_FECHA)) AS fechapol,
	                        NULL AS area,
	                        NULL AS cuenta_oc,
	                        6 AS empresa,
	                        GETDATE() as fecha,
	                        LEFT(DMOV_DOCUM, 2) AS itmPeru,
                            RIGHT('00000000000'+ RTRIM(ISNULL(DMOV_ANEXO, '0')), 11) AS proveedorPeru
                        FROM [003BDCONT{0}].dbo.DETMOV07
						UNION
						SELECT
	                        {0} AS year,
	                        8 AS mes,	                       
	                        CAST(DMOV_C_COMPR AS int) AS poliza,
	                        CAST(DMOV_SECUE AS int) AS linea,
                            SUBDIAR_CODIGO AS tp,
	                        DMOV_CUENT AS cta,
	                        0 AS sscta,
	                        0 AS sscta,
	                        0 AS digito,
                            CASE WHEN DMOV_DEBE <> 0 THEN (CASE WHEN DMOV_DEBE > 0 THEN 1 ELSE 3 END) ELSE (CASE WHEN DMOV_HABER <> 0 THEN (CASE WHEN DMOV_HABER > 0 THEN 2 ELSE 4 END) ELSE 1 END) END AS tm,
	                        CASE WHEN (SUBDIAR_CODIGO = '09' AND DMOV_GLOSA like 'Consumo - %' AND DMOV_CENCO IS NOT NULL) THEN DMOV_CENCO ELSE DMOV_GLOSA END AS cc,
	                        DMOV_GLOSA AS concepto,
                            DMOV_DOCUM as referencia,
	                        DMOV_DEBE - DMOV_HABER AS monto,
                            DMOV_DEBUS - DMOV_HABUS as montoUs,
                            'C' AS status,
	                        0 AS iclave,
	                        0 AS itm,
	                        NULL AS numpro,
	                        Convert(datetime,(DMOV_FECHA)) AS fechapol,
	                        NULL AS area,
	                        NULL AS cuenta_oc,
	                        6 AS empresa,
	                        GETDATE() as fecha,
	                        LEFT(DMOV_DOCUM, 2) AS itmPeru,
                            RIGHT('00000000000'+ RTRIM(ISNULL(DMOV_ANEXO, '0')), 11) AS proveedorPeru
                        FROM [003BDCONT{0}].dbo.DETMOV08
						UNION
						SELECT
	                        {0} AS year,
	                        9 AS mes,	                       
	                        CAST(DMOV_C_COMPR AS int) AS poliza,
	                        CAST(DMOV_SECUE AS int) AS linea,
                            SUBDIAR_CODIGO AS tp,
	                        DMOV_CUENT AS cta,
	                        0 AS sscta,
	                        0 AS sscta,
	                        0 AS digito,
                            CASE WHEN DMOV_DEBE <> 0 THEN (CASE WHEN DMOV_DEBE > 0 THEN 1 ELSE 3 END) ELSE (CASE WHEN DMOV_HABER <> 0 THEN (CASE WHEN DMOV_HABER > 0 THEN 2 ELSE 4 END) ELSE 1 END) END AS tm,
	                        CASE WHEN (SUBDIAR_CODIGO = '09' AND DMOV_GLOSA like 'Consumo - %' AND DMOV_CENCO IS NOT NULL) THEN DMOV_CENCO ELSE DMOV_GLOSA END AS cc,
	                        DMOV_GLOSA AS concepto,
                            DMOV_DOCUM as referencia,
	                        DMOV_DEBE - DMOV_HABER AS monto,
                            DMOV_DEBUS - DMOV_HABUS as montoUs,
                            'C' AS status,
	                        0 AS iclave,
	                        0 AS itm,
	                        NULL AS numpro,
	                        Convert(datetime,(DMOV_FECHA)) AS fechapol,
	                        NULL AS area,
	                        NULL AS cuenta_oc,
	                        6 AS empresa,
	                        GETDATE() as fecha,
	                        LEFT(DMOV_DOCUM, 2) AS itmPeru,
                            RIGHT('00000000000'+ RTRIM(ISNULL(DMOV_ANEXO, '0')), 11) AS proveedorPeru
                        FROM [003BDCONT{0}].dbo.DETMOV09
						UNION
						SELECT
	                        {0} AS year,
	                        10 AS mes,	                       
	                        CAST(DMOV_C_COMPR AS int) AS poliza,
	                        CAST(DMOV_SECUE AS int) AS linea,
                            SUBDIAR_CODIGO AS tp,
	                        DMOV_CUENT AS cta,
	                        0 AS sscta,
	                        0 AS sscta,
	                        0 AS digito,
                            CASE WHEN DMOV_DEBE <> 0 THEN (CASE WHEN DMOV_DEBE > 0 THEN 1 ELSE 3 END) ELSE (CASE WHEN DMOV_HABER <> 0 THEN (CASE WHEN DMOV_HABER > 0 THEN 2 ELSE 4 END) ELSE 1 END) END AS tm,
	                        CASE WHEN (SUBDIAR_CODIGO = '09' AND DMOV_GLOSA like 'Consumo - %' AND DMOV_CENCO IS NOT NULL) THEN DMOV_CENCO ELSE DMOV_GLOSA END AS cc,
	                        DMOV_GLOSA AS concepto,
                            DMOV_DOCUM as referencia,
	                        DMOV_DEBE - DMOV_HABER AS monto,
                            DMOV_DEBUS - DMOV_HABUS as montoUs,
                            'C' AS status,
	                        0 AS iclave,
	                        0 AS itm,
	                        NULL AS numpro,
	                        Convert(datetime,(DMOV_FECHA)) AS fechapol,
	                        NULL AS area,
	                        NULL AS cuenta_oc,
	                        6 AS empresa,
	                        GETDATE() as fecha,
	                        LEFT(DMOV_DOCUM, 2) AS itmPeru,
                            RIGHT('00000000000'+ RTRIM(ISNULL(DMOV_ANEXO, '0')), 11) AS proveedorPeru
                        FROM [003BDCONT{0}].dbo.DETMOV10
						UNION
						SELECT
	                        {0} AS year,
	                        11 AS mes,	                       
	                        CAST(DMOV_C_COMPR AS int) AS poliza,
	                        CAST(DMOV_SECUE AS int) AS linea,
                            SUBDIAR_CODIGO AS tp,
	                        DMOV_CUENT AS cta,
	                        0 AS sscta,
	                        0 AS sscta,
	                        0 AS digito,
                            CASE WHEN DMOV_DEBE <> 0 THEN (CASE WHEN DMOV_DEBE > 0 THEN 1 ELSE 3 END) ELSE (CASE WHEN DMOV_HABER <> 0 THEN (CASE WHEN DMOV_HABER > 0 THEN 2 ELSE 4 END) ELSE 1 END) END AS tm,
	                        CASE WHEN (SUBDIAR_CODIGO = '09' AND DMOV_GLOSA like 'Consumo - %' AND DMOV_CENCO IS NOT NULL) THEN DMOV_CENCO ELSE DMOV_GLOSA END AS cc,
	                        DMOV_GLOSA AS concepto,
                            DMOV_DOCUM as referencia,
	                        DMOV_DEBE - DMOV_HABER AS monto,
                            DMOV_DEBUS - DMOV_HABUS as montoUs,
                            'C' AS status,
	                        0 AS iclave,
	                        0 AS itm,
	                        NULL AS numpro,
	                        Convert(datetime,(DMOV_FECHA)) AS fechapol,
	                        NULL AS area,
	                        NULL AS cuenta_oc,
	                        6 AS empresa,
	                        GETDATE() as fecha,
	                        LEFT(DMOV_DOCUM, 2) AS itmPeru,
                            RIGHT('00000000000'+ RTRIM(ISNULL(DMOV_ANEXO, '0')), 11) AS proveedorPeru
                        FROM [003BDCONT{0}].dbo.DETMOV11
						UNION
						SELECT
	                        {0} AS year,
	                        12 AS mes,	                       
	                        CAST(DMOV_C_COMPR AS int) AS poliza,
	                        CAST(DMOV_SECUE AS int) AS linea,
                            SUBDIAR_CODIGO AS tp,
	                        DMOV_CUENT AS cta,
	                        0 AS sscta,
	                        0 AS sscta,
	                        0 AS digito,
                            CASE WHEN DMOV_DEBE <> 0 THEN (CASE WHEN DMOV_DEBE > 0 THEN 1 ELSE 3 END) ELSE (CASE WHEN DMOV_HABER <> 0 THEN (CASE WHEN DMOV_HABER > 0 THEN 2 ELSE 4 END) ELSE 1 END) END AS tm,
	                        CASE WHEN (SUBDIAR_CODIGO = '09' AND DMOV_GLOSA like 'Consumo - %' AND DMOV_CENCO IS NOT NULL) THEN DMOV_CENCO ELSE DMOV_GLOSA END AS cc,
	                        DMOV_GLOSA AS concepto,
                            DMOV_DOCUM as referencia,
	                        DMOV_DEBE - DMOV_HABER AS monto,
                            DMOV_DEBUS - DMOV_HABUS as montoUs,
                            'C' AS status,
	                        0 AS iclave,
	                        0 AS itm,
	                        NULL AS numpro,
	                        Convert(datetime,(DMOV_FECHA)) AS fechapol,
	                        NULL AS area,
	                        NULL AS cuenta_oc,
	                        6 AS empresa,
	                        GETDATE() as fecha,
	                        LEFT(DMOV_DOCUM, 2) AS itmPeru,
                            RIGHT('00000000000'+ RTRIM(ISNULL(DMOV_ANEXO, '0')), 11) AS proveedorPeru
                        FROM [003BDCONT{0}].dbo.DETMOV12 ", anio);
        }

        private List<tblC_sc_movpol> MovimientosPolizasEnkontrol(PolizasDTO item, EmpresaEnum empresa, DateTime fechaConversion)
        {
            if (vSesiones.sesionEmpresaActual == 6)
            {
                DynamicParameters lstParametros = new DynamicParameters();
                DynamicParameters lstParametrosDollar = new DynamicParameters();
                var obrasPeru = ObrasPeruCplan();
                var cuentas = CuentasPeruCplan();
                var cuentasSistema = CuentasSistemasBancario(EmpresaEnum.Construplan);
                var iTiposMovPeru = iTiposMovimientoPeruCplan();
                var anioActual = item.year;
                List<int> cuentasAplicaITM = new List<int> { 1110, 1115, 1125, 1135, 1146, 1155, 1210, 1220, 1225, 1316, 1321, 2105, 2106, 2115, 2117, 2130, 4000, 4900, 5000, 5280 };

                var sql = queryPolizaDetallePeru(anioActual);

                List<tblC_sc_movpol> lstMovPol = new List<tblC_sc_movpol>();
                List<tblC_sc_movpol> lstMovPolDollar = new List<tblC_sc_movpol>();

                var cuentasExcepcion = _context.tblC_SC_CuentasExcepcion.Where(x => x.esActivo).Select(x => x.cuenta).ToList();
                using (var conexion = new SqlConnection(ConextSigoDapper.conexionStarsoftBancos()))
                {
                    conexion.Open();
                    lstMovPol = conexion.Query<tblC_sc_movpol>(sql, lstParametros, null, true, 300, commandType: CommandType.Text).Where(x => x.year == item.year && x.mes == item.mes && x.poliza == item.poliza && x.tp == item.tp).ToList();
                    var tPoliza = _context.tblC_PolizaTP.FirstOrDefault(x => x.esActivo && x.PalTP == item.tp).SecTP;
                    List<tblC_sc_movpol> lstMovPolConComplementaria = new List<tblC_sc_movpol>();
                    List<tblC_sc_movpol> lstEliminados = new List<tblC_sc_movpol>();


                    var lineaTotal = lstMovPol.Count();
                    var tipoCambioDollar = _context.tblC_SC_TipoCambio.FirstOrDefault(tc => tc.Moneda == 2 && tc.Fecha == fechaConversion2);
                    var tipoCambioSol = _context.tblC_SC_TipoCambio.FirstOrDefault(tc => tc.Moneda == 4 && tc.Fecha == fechaConversion2);
                    var tipoCambioMx = _context.tblC_SC_TipoCambio.FirstOrDefault(tc => tc.Moneda == 3 && tc.Fecha == fechaConversion2);
                    bool esComplementaria = false;                    

                    foreach (var mov in lstMovPol)
                    {
                        if (!cuentasExcepcion.Contains(mov.cta))
                        {
                            mov.year = fechaConversion.Year;
                            mov.mes = fechaConversion.Month;

                            var cuentasProveedores = _context.tblC_CP_CuentasProveedores.Where(x => x.esActivo).ToList();
                            var cuentasProveedoresIDs = cuentasProveedores.Select(x => x.cta).ToList();
                            var obraCaptura = mov.cc;

                            if (mov.tp == "09")
                            {
                                var _centroCosto = obrasPeru.FirstOrDefault(x => x.ccPrincipal == mov.cc);
                                if (_centroCosto != null) obraCaptura = _centroCosto.ccSecundario;
                            }

                            else 
                            {
                                if (mov.tp != "04") obraCaptura = (obraCaptura.Length > 0 && obraCaptura.Contains('/')) ? obraCaptura.Split('/')[1] : obraCaptura;
                                obraCaptura = obraCaptura.Length > 0 ? obraCaptura.Split(' ')[0] : "";
                            }                            

                            tblC_Cta_RelCC obra = obrasPeru.FirstOrDefault(cc => cc.ccPrincipal == obraCaptura);
                            if (obra == null) obra = obrasPeru.FirstOrDefault(cc => cc.ccSecundario == obraCaptura) ?? new tblC_Cta_RelCC();
                            var cuenta = cuentas.FirstOrDefault(cta => cta.palCta == mov.cta && cta.palScta == mov.scta && cta.palSscta == mov.sscta) ?? new tblC_Cta_RelCuentas();
                            
                            var _cta = cuenta.secCta;
                            var _scta = cuenta.secScta;
                            var _sscta = cuenta.secSscta;

                            if (cuentasProveedoresIDs.Contains(mov.cta))
                            {
                                var conexionEnkontrol = _contextEnkontrol.BaseDatosProductivoDesdeEmpresa(EmpresaEnum.Construplan);
                                var odbc = new OdbcConsultaDTO();
                                odbc.consulta = @"SELECT * FROM catcta WHERE cta = 2105 AND scta = 500";
                                var cuentasEnkontrol = _contextEnkontrol.Select<catctaDTO>(conexionEnkontrol, odbc);
                                var cuentaEnkontrol = cuentasEnkontrol.FirstOrDefault(x => x.descripcion.Contains(mov.proveedorPeru));
                                _cta = 2105;
                                _scta = 500;
                                if (cuentaEnkontrol != null) _sscta = cuentaEnkontrol.sscta;
                                else _sscta = 0;
                            }
                            var cuentaSistema = cuentasSistema.FirstOrDefault(cta => cta.cuenta == _cta && CuentasSistemasValidos.Contains(cta.sistema)) ?? new CtaintDTO();

                            var tipoMov = iTiposMovPeru.FirstOrDefault(itm => itm.PaliTmPeru == mov.itmPeru && itm.PaliTm == mov.tm);

                            var ctaInstitucionComplementaria = _context.tblAF_DxP_RelInstitucionCta
                            .FirstOrDefault(f =>
                                f.institucionID == cuenta.palCta &&
                                f.activo &&
                                f.complementaria &&
                                f.moneda == 2
                            );
                            var lstMovPolComplementaria = new tblC_sc_movpol();

                            if (ctaInstitucionComplementaria != null)
                            {
                                esComplementaria = true;
                            }

                            if (esComplementaria == true)
                            {
                                if (cuentasProveedoresIDs.Contains(mov.cta)) 
                                {
                                    var _cuentaProveedores = cuentasProveedores.FirstOrDefault(x => x.cta == mov.cta);
                                    mov.cc = (obra.ccSecundario == null || obra.ccSecundario.Trim() == String.Empty) ? "032" : obra.ccSecundario;
                                    mov.cta = _cta;
                                    mov.scta = _scta;
                                    mov.sscta = _sscta;
                                    mov.itm = (tipoMov != null && cuentasAplicaITM.Contains(_cta)) ? tipoMov.SeciTm : 0;
                                    mov.tm = mov.tm;
                                    mov.monto = _cuentaProveedores.moneda == 1 ? convertirPesosColAPesosMX(mov.monto) :  mov.montoUs * tipoCambioDollar.TipoCambio;
                                    mov.linea = mov.linea;
                                    mov.referencia = mov.referencia == null ? "" : (mov.referencia.Length <= 8 ? mov.referencia : mov.referencia.Substring(mov.referencia.Length - 8, 8));
                                    mov.tp = tPoliza;
                                }
                                else
                                {
                                    if (ctaInstitucionComplementaria != null)
                                    {
                                        lineaTotal++;
                                        lstMovPolComplementaria.year = mov.year;
                                        lstMovPolComplementaria.mes = mov.mes;
                                        lstMovPolComplementaria.cc = (obra.ccSecundario == null || obra.ccSecundario.Trim() == String.Empty) ? "032" : obra.ccSecundario;
                                        lstMovPolComplementaria.cta = cuenta.secCta;
                                        lstMovPolComplementaria.scta = ctaInstitucionComplementaria.scta;
                                        lstMovPolComplementaria.sscta = cuenta.secSscta;
                                        lstMovPolComplementaria.itm = (tipoMov != null && cuentasAplicaITM.Contains(_cta)) ? tipoMov.SeciTm : 0;
                                        lstMovPolComplementaria.tm = mov.tm;
                                        lstMovPolComplementaria.concepto = mov.concepto;
                                        //aqui poner el calculo de la complementaria
                                        lstMovPolComplementaria.linea = lineaTotal;
                                        lstMovPolComplementaria.monto = ((mov.montoUs * tipoCambioDollar.TipoCambio) - mov.montoUs);

                                        lstMovPolComplementaria.referencia = mov.referencia == null ? "" : (mov.referencia.Length <= 8 ? mov.referencia : mov.referencia.Substring(mov.referencia.Length - 8, 8));
                                        lstMovPolComplementaria.tp = tPoliza;
                                        lstMovPolConComplementaria.Add(lstMovPolComplementaria);

                                        mov.cc = (obra.ccSecundario == null || obra.ccSecundario.Trim() == String.Empty) ? "032" : obra.ccSecundario;
                                        mov.cta = _cta;
                                        mov.scta = _scta;
                                        mov.sscta = _sscta;
                                        mov.itm = (tipoMov != null && cuentasAplicaITM.Contains(_cta)) ? tipoMov.SeciTm : 0;
                                        mov.tm = mov.tm;
                                        mov.monto = mov.montoUs;
                                        mov.linea = mov.linea;
                                        mov.referencia = mov.referencia == null ? "" : (mov.referencia.Length <= 8 ? mov.referencia : mov.referencia.Substring(mov.referencia.Length - 8, 8));
                                        mov.tp = tPoliza;
                                    }
                                    else
                                    {
                                        mov.cc = (obra.ccSecundario == null || obra.ccSecundario.Trim() == String.Empty) ? "032" : obra.ccSecundario;
                                        mov.cta = _cta;
                                        mov.scta = _scta;
                                        mov.sscta = _sscta;
                                        mov.itm = (tipoMov != null && cuentasAplicaITM.Contains(_cta)) ? tipoMov.SeciTm : 0;
                                        mov.tm = mov.tm;
                                        mov.monto = (mov.montoUs * tipoCambioDollar.TipoCambio);
                                        mov.linea = mov.linea;
                                        mov.referencia = mov.referencia == null ? "" : (mov.referencia.Length <= 8 ? mov.referencia : mov.referencia.Substring(mov.referencia.Length - 8, 8));
                                        mov.tp = tPoliza;
                                    }
                                }
                            }
                            else
                            {
                                mov.cc = (obra.ccSecundario == null || obra.ccSecundario.Trim() == String.Empty) ? "032" : obra.ccSecundario;
                                mov.cta = _cta;
                                mov.scta = _scta;
                                mov.sscta = _sscta;
                                mov.itm = (tipoMov != null && cuentasAplicaITM.Contains(_cta)) ? tipoMov.SeciTm : 0;
                                mov.tm = mov.tm;
                                mov.monto = convertirPesosColAPesosMX(mov.monto);
                                mov.linea = mov.linea;
                                mov.referencia = mov.referencia == null ? "" : (mov.referencia.Length <= 8 ? mov.referencia : mov.referencia.Substring(mov.referencia.Length - 8, 8));
                                mov.tp = tPoliza;
                            }
                        }
                        else {
                            lstEliminados.Add(mov);
                        }
                        
                    }
                    lstMovPol.AddRange(lstMovPolConComplementaria);
                    foreach(var itemEliminados in lstEliminados)
                    {
                        lstMovPol.Remove(itemEliminados);
                    }
                    conexion.Close();
                }
                return lstMovPol;
            }
            else
            {

                //var conexion = empresa == EmpresaEnum.Construplan ? EnkontrolEnum.PruebaCplanProd : _contextEnkontrol.BaseDatosProductivoDesdeEmpresa(empresa);
                var conexion = _contextEnkontrol.BaseDatosProductivoDesdeEmpresa(empresa);
                var odbc = new OdbcConsultaDTO();
                odbc.consulta = @"SELECT * FROM ""DBA"".""sc_movpol"" WHERE year = ? AND mes = ? AND poliza = ? AND tp = ?";
                odbc.parametros.Add(new OdbcParameterDTO()
                {
                    nombre = "year",
                    tipo = OdbcType.VarChar,
                    valor = item.year
                });
                odbc.parametros.Add(new OdbcParameterDTO()
                {
                    nombre = "mes",
                    tipo = OdbcType.VarChar,
                    valor = item.mes
                });
                odbc.parametros.Add(new OdbcParameterDTO()
                {
                    nombre = "poliza",
                    tipo = OdbcType.VarChar,
                    valor = item.poliza
                });
                odbc.parametros.Add(new OdbcParameterDTO()
                {
                    nombre = "tp",
                    tipo = OdbcType.VarChar,
                    valor = item.tp
                });
                var resultado = _contextEnkontrol.Select<tblC_sc_movpol>(conexion, odbc);
                return resultado;
            }
        }
        private void asignarPesosColombianos(DateTime fechaPoliza)
        {
            pesosMexicanos = getPesosMXDesdeColombia(fechaPoliza);
        }
        private void asignarPesosSolesDollar(DateTime fechaPoliza)
        {
            solesDollar = getPesosMXDesdeSolesADollar(fechaPoliza);
        }

        private void asignarPesosSolesPesosMX(DateTime fechaPoliza)
        {
            solesPesos = getPesosMXDesdeSolesAPesosMX(fechaPoliza);
        }

        private decimal convertirPesosColAPesosMX(decimal monto)
        {
            if (vSesiones.sesionEmpresaActual == 6)
            {
                var conversion = (monto * pesosMexicanos).ParseDecimal();
                return decimal.Round(conversion, 6);
            }
            else
            {
                var conversion = (monto / pesosMexicanos).ParseDecimal();
                return decimal.Round(conversion, 6);
            }    
        }

        private decimal convertirSolesAPesosMXDollar(decimal monto)
        {
            var conversionUs = (monto * solesDollar).ParseDecimal();
            return decimal.Round(conversionUs, 6);
        }

        private decimal convertirSolesADollar(decimal monto)
        {
            var conversion = (monto / solesDollar).ParseDecimal();
            return decimal.Round(conversion, 6);
        }
        private decimal convertirPesosDollarMX(decimal monto)
        {            
                var conversion = (monto * pesosMexicanos).ParseDecimal();
                return decimal.Round(conversion, 6);         
        }
        public decimal getPesosMXDesdeSolesAPesosMX(DateTime dia)
        {
            try
            {
                var pesosMx = _context.tblC_SC_TipoCambio.FirstOrDefault(tc => tc.Moneda == 3 && tc.Fecha == dia);

                return pesosMx.TipoCambio;
            }
            catch (Exception) { return 0; }
        }

        public decimal getPesosMXDesdeSolesADollar(DateTime dia)
        {
            try
            {
                var pesosUs = _context.tblC_SC_TipoCambio.FirstOrDefault(tc => tc.Moneda == 2 && tc.Fecha == dia);

                return pesosUs.TipoCambio;
            }
            catch (Exception) { return 0; }
        }
        public decimal getPesosMXDesdeColombia(DateTime dia)
        {
            try
            {
                var pesosMx = _context.tblC_SC_TipoCambio.FirstOrDefault(tc => tc.Moneda == 3 && tc.Fecha == dia);

                return pesosMx.TipoCambio;
            }
            catch (Exception) { return 0; }
        }
        public Dictionary<string, object> ObtenerPolizaDetalle(int year, int mes, string tp, int poliza, int empresa)
        {
            var r = new Dictionary<string, object>();
            var conexion = _contextEnkontrol.BaseDatosProductivoDesdeEmpresa((EmpresaEnum)empresa);
            //var conexion = empresa == 1 ? EnkontrolEnum.PruebaCplanProd : _contextEnkontrol.BaseDatosProductivoDesdeEmpresa((EmpresaEnum)empresa);
            try
            {
                OdbcConsultaDTO odbcPoliza = new OdbcConsultaDTO();
                odbcPoliza.consulta = string.Format(@"
                    SELECT
                        POL.fechapol AS FechaPoliza,
                        POL.year AS Año,
                        POL.mes AS Mes,
                        POL.tp AS TipoPoliza,
                        POL.poliza AS Poliza,
                        MOV.linea AS Linea,
                        MOV.cta As Cuenta,
                        MOV.scta AS Subcuenta,
                        MOV.sscta AS SubSubcuenta,
                        (CAST(MOV.cta AS varchar(10)) + '-' + CAST(MOV.scta AS varchar(10)) + '-' + CAST(MOV.sscta AS varchar(10))) AS DescripcionCuenta,
                        MOV.tm AS TipoMovimiento,
                        MOV.itm AS iTipoMovimiento,
                        MOV.referencia AS Referencia,
                        MOV.cc AS CC,
                        MOV.Concepto AS Concepto,
                        MOV.monto AS Monto,
                        MOV.area AS Area,
                        MOV.cuenta_oc AS Cuenta_OC
                    FROM DBA.sc_movpol AS MOV
                    INNER JOIN DBA.sc_polizas AS POL ON POL.year = MOV.year AND POL.mes = MOV.mes AND POL.tp = MOV.tp AND POL.poliza = MOV.poliza
                        WHERE MOV.year = ? AND MOV.mes = ? AND MOV.tp = ? AND MOV.poliza = ?");
                odbcPoliza.parametros.Add(new OdbcParameterDTO()
                {
                    nombre = "year",
                    tipo = OdbcType.Int,
                    valor = year
                });
                odbcPoliza.parametros.Add(new OdbcParameterDTO()
                {
                    nombre = "mes",
                    tipo = OdbcType.Int,
                    valor = mes
                });
                odbcPoliza.parametros.Add(new OdbcParameterDTO()
                {
                    nombre = "tp",
                    tipo = OdbcType.Char,
                    valor = tp
                });
                odbcPoliza.parametros.Add(new OdbcParameterDTO()
                {
                    nombre = "poliza",
                    tipo = OdbcType.Int,
                    valor = poliza
                });

                List<PolizaDetalleDTO> polizaDetalle = _contextEnkontrol.Select<PolizaDetalleDTO>(conexion, odbcPoliza);

                r.Add(SUCCESS, true);
                r.Add("Value", polizaDetalle);

            }
            catch (Exception o_O)
            {
                r.Add(MESSAGE, o_O.Message);
                LogError(0, 0, nombreControlador, "ObtenerPolizaDetalle", o_O, AccionEnum.CONSULTA, 0, null);
            }

            return r;
        }

        public Dictionary<string, object> ObtenerPolizaDetallePeru(int year, int mes, string tp, int poliza, int empresa)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                DynamicParameters lstParametros = new DynamicParameters();
                // para pruebas diciembre
//                                var sql = @"
//             	 SELECT
//                                        Convert(datetime,(DMOV_FECHA)) AS FechaPoliza,
//	                                    2022 as Año,
//	                                    CAST(12 AS int) AS Mes,
//	                                    SUBDIAR_CODIGO AS TipoPoliza,
//	                                    CAST(DMOV_C_COMPR AS int) AS Poliza,
//	                                    CAST(DMOV_SECUE AS int) AS Linea,
//	                                    DMOV_CUENT AS Cuenta,
//	                                    0 AS Subcuenta,
//	                                    0 AS SubSubcuenta,
//	                                    (CAST(DMOV_CUENT AS varchar(10)) + '-' + CAST(0 AS varchar(10)) + '-' + CAST(0 AS varchar(10))) AS DescripcionCuenta,
//	                                    CASE WHEN DMOV_DEBE <> 0 THEN (CASE WHEN DMOV_DEBE > 0 THEN 1 ELSE 3 END) ELSE (CASE WHEN DMOV_HABER <> 0 THEN (CASE WHEN DMOV_HABER > 0 THEN 2 ELSE 4 END) ELSE 1 END) END AS TipoMovimiento,
//                                        DMOV_DOCUM as referencia,	                                    
//                                        DMOV_CENCO AS CC,
//	                                    DMOV_GLOSA AS Concepto,
//	                                    DMOV_DEBE - DMOV_HABER AS Monto,
//	                                    1 AS iTipoMovimiento,                         
//	                                    NULL AS Area,
//	                                    NULL AS Cuenta_OC,
//	                                    6 AS empresa,
//	                                    GETDATE() as fecha
//                                    FROM [003BDCONT2022].dbo.DETMOV12 ";

                                var sql = string.Format(@"
                					 SELECT
                                        Convert(datetime,(DMOV_FECHA)) AS FechaPoliza,
	                                    {0} as Año,
	                                    CAST(01 AS int) AS Mes,
	                                    SUBDIAR_CODIGO AS TipoPoliza,
	                                    CAST(DMOV_C_COMPR AS int) AS Poliza,
	                                    CAST(DMOV_SECUE AS int) AS Linea,
	                                    DMOV_CUENT AS Cuenta,
	                                    0 AS Subcuenta,
	                                    0 AS SubSubcuenta,
	                                    (CAST(DMOV_CUENT AS varchar(10)) + '-' + CAST(0 AS varchar(10)) + '-' + CAST(0 AS varchar(10))) AS DescripcionCuenta,
                                        CASE WHEN DMOV_DEBE <> 0 THEN (CASE WHEN DMOV_DEBE > 0 THEN 1 ELSE 3 END) ELSE (CASE WHEN DMOV_HABER <> 0 THEN (CASE WHEN DMOV_HABER > 0 THEN 2 ELSE 4 END) ELSE 1 END) END AS TipoMovimiento,
                                        '' AS Referencia,	                                    
                                        DMOV_CENCO AS CC,
	                                    DMOV_GLOSA AS Concepto,
	                                    DMOV_DEBE - DMOV_HABER AS Monto,
                                        DMOV_DEBUS - DMOV_HABUS AS montoUs,
	                                    1 AS iTipoMovimiento,                         
	                                    NULL AS Area,
	                                    NULL AS Cuenta_OC,
	                                    6 AS empresa,
	                                    GETDATE() as fecha
                                    FROM [003BDCONT{0}].dbo.DETMOV01 
                                    UNION
	                                SELECT
                                        Convert(datetime,(DMOV_FECHA)) AS FechaPoliza,
	                                    {0} as Año,
	                                    CAST(02 AS int) AS Mes,
	                                    SUBDIAR_CODIGO AS TipoPoliza,
	                                    CAST(DMOV_C_COMPR AS int) AS Poliza,
	                                    CAST(DMOV_SECUE AS int) AS Linea,
	                                    DMOV_CUENT AS Cuenta,
	                                    0 AS Subcuenta,
	                                    0 AS SubSubcuenta,
	                                    (CAST(DMOV_CUENT AS varchar(10)) + '-' + CAST(0 AS varchar(10)) + '-' + CAST(0 AS varchar(10))) AS DescripcionCuenta,
                                        CASE WHEN DMOV_DEBE <> 0 THEN (CASE WHEN DMOV_DEBE > 0 THEN 1 ELSE 3 END) ELSE (CASE WHEN DMOV_HABER <> 0 THEN (CASE WHEN DMOV_HABER > 0 THEN 2 ELSE 4 END) ELSE 1 END) END AS TipoMovimiento,
                                        '' AS Referencia,	                                    
                                        DMOV_CENCO AS CC,
	                                    DMOV_GLOSA AS Concepto,
	                                    DMOV_DEBE - DMOV_HABER AS Monto,
                                        DMOV_DEBUS - DMOV_HABUS AS montoUs,
	                                    1 AS iTipoMovimiento,                         
	                                    NULL AS Area,
	                                    NULL AS Cuenta_OC,
	                                    6 AS empresa,
	                                    GETDATE() as fecha
                                    FROM [003BDCONT{0}].dbo.DETMOV02
                                    UNION
	                                SELECT
                                        Convert(datetime,(DMOV_FECHA)) AS FechaPoliza,
	                                    {0} as Año,
	                                    CAST(03 AS int) AS Mes,
	                                    SUBDIAR_CODIGO AS TipoPoliza,
	                                    CAST(DMOV_C_COMPR AS int) AS Poliza,
	                                    CAST(DMOV_SECUE AS int) AS Linea,
	                                    DMOV_CUENT AS Cuenta,
	                                    0 AS Subcuenta,
	                                    0 AS SubSubcuenta,
	                                    (CAST(DMOV_CUENT AS varchar(10)) + '-' + CAST(0 AS varchar(10)) + '-' + CAST(0 AS varchar(10))) AS DescripcionCuenta,
                                        CASE WHEN DMOV_DEBE <> 0 THEN (CASE WHEN DMOV_DEBE > 0 THEN 1 ELSE 3 END) ELSE (CASE WHEN DMOV_HABER <> 0 THEN (CASE WHEN DMOV_HABER > 0 THEN 2 ELSE 4 END) ELSE 1 END) END AS TipoMovimiento,
                                        '' AS Referencia,	                                    
                                        DMOV_CENCO AS CC,
	                                    DMOV_GLOSA AS Concepto,
	                                    DMOV_DEBE - DMOV_HABER AS Monto,
                                        DMOV_DEBUS - DMOV_HABUS AS montoUs,
	                                    1 AS iTipoMovimiento,                         
	                                    NULL AS Area,
	                                    NULL AS Cuenta_OC,
	                                    6 AS empresa,
	                                    GETDATE() as fecha
                                    FROM [003BDCONT{0}].dbo.DETMOV03
                                    UNION
	                                SELECT
                                        Convert(datetime,(DMOV_FECHA)) AS FechaPoliza,
	                                    {0} as Año,
	                                    CAST(04 AS int) AS Mes,
	                                    SUBDIAR_CODIGO AS TipoPoliza,
	                                    CAST(DMOV_C_COMPR AS int) AS Poliza,
	                                    CAST(DMOV_SECUE AS int) AS Linea,
	                                    DMOV_CUENT AS Cuenta,
	                                    0 AS Subcuenta,
	                                    0 AS SubSubcuenta,
	                                    (CAST(DMOV_CUENT AS varchar(10)) + '-' + CAST(0 AS varchar(10)) + '-' + CAST(0 AS varchar(10))) AS DescripcionCuenta,
                                        CASE WHEN DMOV_DEBE <> 0 THEN (CASE WHEN DMOV_DEBE > 0 THEN 1 ELSE 3 END) ELSE (CASE WHEN DMOV_HABER <> 0 THEN (CASE WHEN DMOV_HABER > 0 THEN 2 ELSE 4 END) ELSE 1 END) END AS TipoMovimiento,
                                        '' AS Referencia,	                                    
                                        DMOV_CENCO AS CC,
	                                    DMOV_GLOSA AS Concepto,
	                                    DMOV_DEBE - DMOV_HABER AS Monto,
                                        DMOV_DEBUS - DMOV_HABUS AS montoUs,
	                                    1 AS iTipoMovimiento,                         
	                                    NULL AS Area,
	                                    NULL AS Cuenta_OC,
	                                    6 AS empresa,
	                                    GETDATE() as fecha
                                    FROM [003BDCONT{0}].dbo.DETMOV04
                                    UNION
	                                SELECT
                                        Convert(datetime,(DMOV_FECHA)) AS FechaPoliza,
	                                    {0} as Año,
	                                    CAST(05 AS int) AS Mes,
	                                    SUBDIAR_CODIGO AS TipoPoliza,
	                                    CAST(DMOV_C_COMPR AS int) AS Poliza,
	                                    CAST(DMOV_SECUE AS int) AS Linea,
	                                    DMOV_CUENT AS Cuenta,
	                                    0 AS Subcuenta,
	                                    0 AS SubSubcuenta,
	                                    (CAST(DMOV_CUENT AS varchar(10)) + '-' + CAST(0 AS varchar(10)) + '-' + CAST(0 AS varchar(10))) AS DescripcionCuenta,
                                        CASE WHEN DMOV_DEBE <> 0 THEN (CASE WHEN DMOV_DEBE > 0 THEN 1 ELSE 3 END) ELSE (CASE WHEN DMOV_HABER <> 0 THEN (CASE WHEN DMOV_HABER > 0 THEN 2 ELSE 4 END) ELSE 1 END) END AS TipoMovimiento,
                                        '' AS Referencia,	                                    
                                        DMOV_CENCO AS CC,
	                                    DMOV_GLOSA AS Concepto,
	                                    DMOV_DEBE - DMOV_HABER AS Monto,
                                        DMOV_DEBUS - DMOV_HABUS AS montoUs,
	                                    1 AS iTipoMovimiento,                         
	                                    NULL AS Area,
	                                    NULL AS Cuenta_OC,
	                                    6 AS empresa,
	                                    GETDATE() as fecha
                                    FROM [003BDCONT{0}].dbo.DETMOV05
                                    UNION
	                                SELECT
                                        Convert(datetime,(DMOV_FECHA)) AS FechaPoliza,
	                                    {0} as Año,
	                                    CAST(06 AS int) AS Mes,
	                                    SUBDIAR_CODIGO AS TipoPoliza,
	                                    CAST(DMOV_C_COMPR AS int) AS Poliza,
	                                    CAST(DMOV_SECUE AS int) AS Linea,
	                                    DMOV_CUENT AS Cuenta,
	                                    0 AS Subcuenta,
	                                    0 AS SubSubcuenta,
	                                    (CAST(DMOV_CUENT AS varchar(10)) + '-' + CAST(0 AS varchar(10)) + '-' + CAST(0 AS varchar(10))) AS DescripcionCuenta,
                                        CASE WHEN DMOV_DEBE <> 0 THEN (CASE WHEN DMOV_DEBE > 0 THEN 1 ELSE 3 END) ELSE (CASE WHEN DMOV_HABER <> 0 THEN (CASE WHEN DMOV_HABER > 0 THEN 2 ELSE 4 END) ELSE 1 END) END AS TipoMovimiento,
                                        '' AS Referencia,	                                    
                                        DMOV_CENCO AS CC,
	                                    DMOV_GLOSA AS Concepto,
	                                    DMOV_DEBE - DMOV_HABER AS Monto,
                                        DMOV_DEBUS - DMOV_HABUS AS montoUs,
	                                    1 AS iTipoMovimiento,                         
	                                    NULL AS Area,
	                                    NULL AS Cuenta_OC,
	                                    6 AS empresa,
	                                    GETDATE() as fecha
                                    FROM [003BDCONT{0}].dbo.DETMOV06
                                    UNION
	                                SELECT
                                        Convert(datetime,(DMOV_FECHA)) AS FechaPoliza,
	                                    {0} as Año,
	                                    CAST(07 AS int) AS Mes,
	                                    SUBDIAR_CODIGO AS TipoPoliza,
	                                    CAST(DMOV_C_COMPR AS int) AS Poliza,
	                                    CAST(DMOV_SECUE AS int) AS Linea,
	                                    DMOV_CUENT AS Cuenta,
	                                    0 AS Subcuenta,
	                                    0 AS SubSubcuenta,
	                                    (CAST(DMOV_CUENT AS varchar(10)) + '-' + CAST(0 AS varchar(10)) + '-' + CAST(0 AS varchar(10))) AS DescripcionCuenta,
                                        CASE WHEN DMOV_DEBE <> 0 THEN (CASE WHEN DMOV_DEBE > 0 THEN 1 ELSE 3 END) ELSE (CASE WHEN DMOV_HABER <> 0 THEN (CASE WHEN DMOV_HABER > 0 THEN 2 ELSE 4 END) ELSE 1 END) END AS TipoMovimiento,
                                        '' AS Referencia,	                                    
                                        DMOV_CENCO AS CC,
	                                    DMOV_GLOSA AS Concepto,
	                                    DMOV_DEBE - DMOV_HABER AS Monto,
                                        DMOV_DEBUS - DMOV_HABUS AS montoUs,
	                                    1 AS iTipoMovimiento,                         
	                                    NULL AS Area,
	                                    NULL AS Cuenta_OC,
	                                    6 AS empresa,
	                                    GETDATE() as fecha
                                    FROM [003BDCONT{0}].dbo.DETMOV07
                                    UNION
	                                SELECT
                                        Convert(datetime,(DMOV_FECHA)) AS FechaPoliza,
	                                    {0} as Año,
	                                    CAST(08 AS int) AS Mes,
	                                    SUBDIAR_CODIGO AS TipoPoliza,
	                                    CAST(DMOV_C_COMPR AS int) AS Poliza,
	                                    CAST(DMOV_SECUE AS int) AS Linea,
	                                    DMOV_CUENT AS Cuenta,
	                                    0 AS Subcuenta,
	                                    0 AS SubSubcuenta,
	                                    (CAST(DMOV_CUENT AS varchar(10)) + '-' + CAST(0 AS varchar(10)) + '-' + CAST(0 AS varchar(10))) AS DescripcionCuenta,
                                        CASE WHEN DMOV_DEBE <> 0 THEN (CASE WHEN DMOV_DEBE > 0 THEN 1 ELSE 3 END) ELSE (CASE WHEN DMOV_HABER <> 0 THEN (CASE WHEN DMOV_HABER > 0 THEN 2 ELSE 4 END) ELSE 1 END) END AS TipoMovimiento,
                                        '' AS Referencia,	                                    
                                        DMOV_CENCO AS CC,
	                                    DMOV_GLOSA AS Concepto,
	                                    DMOV_DEBE - DMOV_HABER AS Monto,
                                        DMOV_DEBUS - DMOV_HABUS AS montoUs,
	                                    1 AS iTipoMovimiento,                         
	                                    NULL AS Area,
	                                    NULL AS Cuenta_OC,
	                                    6 AS empresa,
	                                    GETDATE() as fecha
                                    FROM [003BDCONT{0}].dbo.DETMOV08
                                    UNION
	                                SELECT
                                        Convert(datetime,(DMOV_FECHA)) AS FechaPoliza,
	                                    {0} as Año,
	                                    CAST(09 AS int) AS Mes,
	                                    SUBDIAR_CODIGO AS TipoPoliza,
	                                    CAST(DMOV_C_COMPR AS int) AS Poliza,
	                                    CAST(DMOV_SECUE AS int) AS Linea,
	                                    DMOV_CUENT AS Cuenta,
	                                    0 AS Subcuenta,
	                                    0 AS SubSubcuenta,
	                                    (CAST(DMOV_CUENT AS varchar(10)) + '-' + CAST(0 AS varchar(10)) + '-' + CAST(0 AS varchar(10))) AS DescripcionCuenta,
                                        CASE WHEN DMOV_DEBE <> 0 THEN (CASE WHEN DMOV_DEBE > 0 THEN 1 ELSE 3 END) ELSE (CASE WHEN DMOV_HABER <> 0 THEN (CASE WHEN DMOV_HABER > 0 THEN 2 ELSE 4 END) ELSE 1 END) END AS TipoMovimiento,
                                        '' AS Referencia,	                                    
                                        DMOV_CENCO AS CC,
	                                    DMOV_GLOSA AS Concepto,
	                                    DMOV_DEBE - DMOV_HABER AS Monto,
                                        DMOV_DEBUS - DMOV_HABUS AS montoUs,
	                                    1 AS iTipoMovimiento,                         
	                                    NULL AS Area,
	                                    NULL AS Cuenta_OC,
	                                    6 AS empresa,
	                                    GETDATE() as fecha
                                    FROM [003BDCONT{0}].dbo.DETMOV09
                                    UNION
	                                SELECT
                                        Convert(datetime,(DMOV_FECHA)) AS FechaPoliza,
	                                    {0} as Año,
	                                    CAST(10 AS int) AS Mes,
	                                    SUBDIAR_CODIGO AS TipoPoliza,
	                                    CAST(DMOV_C_COMPR AS int) AS Poliza,
	                                    CAST(DMOV_SECUE AS int) AS Linea,
	                                    DMOV_CUENT AS Cuenta,
	                                    0 AS Subcuenta,
	                                    0 AS SubSubcuenta,
	                                    (CAST(DMOV_CUENT AS varchar(10)) + '-' + CAST(0 AS varchar(10)) + '-' + CAST(0 AS varchar(10))) AS DescripcionCuenta,
                                        CASE WHEN DMOV_DEBE <> 0 THEN (CASE WHEN DMOV_DEBE > 0 THEN 1 ELSE 3 END) ELSE (CASE WHEN DMOV_HABER <> 0 THEN (CASE WHEN DMOV_HABER > 0 THEN 2 ELSE 4 END) ELSE 1 END) END AS TipoMovimiento,
                                        '' AS Referencia,	                                    
                                        DMOV_CENCO AS CC,
	                                    DMOV_GLOSA AS Concepto,
	                                    DMOV_DEBE - DMOV_HABER AS Monto,
                                        DMOV_DEBUS - DMOV_HABUS AS montoUs,
	                                    1 AS iTipoMovimiento,                         
	                                    NULL AS Area,
	                                    NULL AS Cuenta_OC,
	                                    6 AS empresa,
	                                    GETDATE() as fecha
                                    FROM [003BDCONT{0}].dbo.DETMOV10
                                    UNION
	                                SELECT
                                        Convert(datetime,(DMOV_FECHA)) AS FechaPoliza,
	                                    {0} as Año,
	                                    CAST(11 AS int) AS Mes,
	                                    SUBDIAR_CODIGO AS TipoPoliza,
	                                    CAST(DMOV_C_COMPR AS int) AS Poliza,
	                                    CAST(DMOV_SECUE AS int) AS Linea,
	                                    DMOV_CUENT AS Cuenta,
	                                    0 AS Subcuenta,
	                                    0 AS SubSubcuenta,
	                                    (CAST(DMOV_CUENT AS varchar(10)) + '-' + CAST(0 AS varchar(10)) + '-' + CAST(0 AS varchar(10))) AS DescripcionCuenta,
                                        CASE WHEN DMOV_DEBE <> 0 THEN (CASE WHEN DMOV_DEBE > 0 THEN 1 ELSE 3 END) ELSE (CASE WHEN DMOV_HABER <> 0 THEN (CASE WHEN DMOV_HABER > 0 THEN 2 ELSE 4 END) ELSE 1 END) END AS TipoMovimiento,
                                        '' AS Referencia,	                                    
                                        DMOV_CENCO AS CC,
	                                    DMOV_GLOSA AS Concepto,
	                                    DMOV_DEBE - DMOV_HABER AS Monto,
                                        DMOV_DEBUS - DMOV_HABUS AS montoUs,
	                                    1 AS iTipoMovimiento,                         
	                                    NULL AS Area,
	                                    NULL AS Cuenta_OC,
	                                    6 AS empresa,
	                                    GETDATE() as fecha
                                    FROM [003BDCONT{0}].dbo.DETMOV11
                                    UNION
	                                SELECT
                                        Convert(datetime,(DMOV_FECHA)) AS FechaPoliza,
	                                    {0} as Año,
	                                    CAST(12 AS int) AS Mes,
	                                    SUBDIAR_CODIGO AS TipoPoliza,
	                                    CAST(DMOV_C_COMPR AS int) AS Poliza,
	                                    CAST(DMOV_SECUE AS int) AS Linea,
	                                    DMOV_CUENT AS Cuenta,
	                                    0 AS Subcuenta,
	                                    0 AS SubSubcuenta,
	                                    (CAST(DMOV_CUENT AS varchar(10)) + '-' + CAST(0 AS varchar(10)) + '-' + CAST(0 AS varchar(10))) AS DescripcionCuenta,
                                        CASE WHEN DMOV_DEBE <> 0 THEN (CASE WHEN DMOV_DEBE > 0 THEN 1 ELSE 3 END) ELSE (CASE WHEN DMOV_HABER <> 0 THEN (CASE WHEN DMOV_HABER > 0 THEN 2 ELSE 4 END) ELSE 1 END) END AS TipoMovimiento,
                                        '' AS Referencia,	                                    
                                        DMOV_CENCO AS CC,
	                                    DMOV_GLOSA AS Concepto,
	                                    DMOV_DEBE - DMOV_HABER AS Monto,
                                        DMOV_DEBUS - DMOV_HABUS AS montoUs,
	                                    1 AS iTipoMovimiento,                         
	                                    NULL AS Area,
	                                    NULL AS Cuenta_OC,
	                                    6 AS empresa,
	                                    GETDATE() as fecha
                                    FROM [003BDCONT{0}].dbo.DETMOV12", year);

                List<PolizaDetalleDTO> detallePoliza = new List<PolizaDetalleDTO>();

                using (var conexion2 = new SqlConnection(ConextSigoDapper.conexionStarsoftBancos()))
                {
                    conexion2.Open();
                    detallePoliza = conexion2.Query<PolizaDetalleDTO>(sql, lstParametros, null, true, 300, commandType: CommandType.Text).Where(x => x.Año == year && x.Mes == mes && x.Poliza == poliza && x.TipoPoliza == tp).ToList();
                    conexion2.Close();
                }      

                resultado.Add(SUCCESS, true);
                resultado.Add("Value", detallePoliza);

            }
            catch (Exception o_O)
            {
                resultado.Add(MESSAGE, o_O.Message);
                LogError(0, 0, nombreControlador, "ObtenerPolizaDetallePeru", o_O, AccionEnum.CONSULTA, 0, null);
            }

            return resultado;

        }

        public Dictionary<string, object> CargarConversionPolizas(int year, int mes, string tp, int poliza, decimal monto)
        {
            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;
                var res = from r in _context.tblC_SC_ConversionPoliza
                          where r.esActivo && r.SecEmpresa == EmpresaEnum.Construplan && (r.PalYear == year || r.SecYear == year) && (r.PalMes == mes || r.SecMes == mes) && (tp == "" ? true : (r.PalTP == tp || r.SecTP == tp)) && (poliza == 0 ? true : (r.PalPoliza == poliza || r.SecPoliza == poliza)) && (monto == 0 ? true : (Math.Abs(r.PalAbono) == Math.Abs(monto) || Math.Abs(r.SecAbono) == Math.Abs(monto)))
                          orderby r.SecYear descending, r.SecMes descending, r.SecPoliza descending
                          select r;

                List<ConversionPolizaDTO> polizasConvertidas = res.Select(x => new ConversionPolizaDTO {
                    PalEmpresa = x.PalEmpresa,
                    PalYear = x.PalYear,
                    PalMes = x.PalMes,
                    PalTP = x.PalTP,
                    PalPoliza = x.PalPoliza,
                    PalCargo = x.PalCargo,
                    PalAbono  = x.PalAbono,
                    SecEmpresa = x.SecEmpresa,
                    SecYear = x.SecYear,
                    SecMes = x.SecMes,
                    SecTP = x.SecTP,
                    SecPoliza = x.SecPoliza,
                    SecCargo  = x.SecCargo,
                    SecAbono = x.SecAbono,
                    estatusRegistro = 1,
                }).ToList();

                List<tblC_sc_movpol> lstMovPol = new List<tblC_sc_movpol>();
                int empresa = vSesiones.sesionEmpresaActual;

                switch (empresa) 
                {
                    case (int)EmpresaEnum.Peru:
                        var sql = queryPolizaDetallePeru(year);                
                        DynamicParameters lstParametros = new DynamicParameters();

                        var cuentasExcepcion = _context.tblC_SC_CuentasExcepcion.Where(x => x.esActivo).Select(x => x.cuenta).ToList();

                        using (var conexion = new SqlConnection(ConextSigoDapper.conexionStarsoftBancos()))
                        {
                            conexion.Open();
                            lstMovPol = conexion.Query<tblC_sc_movpol>(sql, lstParametros, null, true, 300, commandType: CommandType.Text).ToList();
                            conexion.Close();
                        }
                        lstMovPol = lstMovPol.Where(x => !cuentasExcepcion.Contains(x.cta)).ToList();
                        break;
                    default:
                        var conexionBD = _contextEnkontrol.BaseDatosProductivoDesdeEmpresa((EmpresaEnum)empresa);
                        var odbc = new OdbcConsultaDTO();
                        odbc.consulta = @"SELECT * FROM ""DBA"".""sc_movpol"" WHERE year = ? AND mes = ?";
                        odbc.parametros.Add(new OdbcParameterDTO()
                        {
                            nombre = "year",
                            tipo = OdbcType.VarChar,
                            valor = year
                        });
                        odbc.parametros.Add(new OdbcParameterDTO()
                        {
                            nombre = "mes",
                            tipo = OdbcType.VarChar,
                            valor = mes
                        });
                        lstMovPol = _contextEnkontrol.Select<tblC_sc_movpol>(conexionBD, odbc);
                        break;
                }

                foreach (var item in polizasConvertidas) 
                {
                    var _auxDetalle = lstMovPol.Where(x => x.year == item.PalYear && x.mes == item.PalMes && x.tp == item.PalTP && x.poliza == item.PalPoliza).ToList();
                    if (_auxDetalle.Count() < 1) 
                    {
                        item.estatusRegistro = 3;
                    }
                    else {
                        var cargoTotal = _auxDetalle.Where(x => x.tm == 1 || x.tm == 3).Sum(x => x.monto);
                        var abonoTotal = _auxDetalle.Where(x => x.tm == 2 || x.tm == 4).Sum(x => x.monto);
                        if (cargoTotal != item.PalCargo && (abonoTotal * (-1)) != item.PalAbono) 
                        {
                            item.estatusRegistro = 2;
                        }
                    }
                }

                resultado.Add("resultado", polizasConvertidas);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception o_O)
            {
                resultado.Add(SUCCESS, false);
                LogError(0, 0, nombreControlador, "CargarConversionPolizas", o_O, AccionEnum.CONSULTA, 0, null);
            }
            return resultado;
        }
        public Dictionary<string, object> BuscarCaptura(DateTime fechaConversion)
        {
            try
            {
                fechaConversion2 = fechaConversion;
                _context.Configuration.AutoDetectChangesEnabled = false;
                var numPolizaCplan = new List<PolizasDTO>();
                var polizasColCplan = new List<ConversionPolizaDTO>();
                asignarPesosColombianos(fechaConversion);
                List<ConversionPolizaDTO> polizasConvertidas = new List<ConversionPolizaDTO>();

                if (vSesiones.sesionEmpresaActual == 6)
                {
                    asignarPesosSolesDollar(fechaConversion);
                    var polizasPeru = PolizasEnkontrolPeru(fechaConversion.Year);
                    polizasConvertidas = PolizasConversionEstatusPeru(polizasPeru);
                    resultado.Add("lst", polizasConvertidas);
                    resultado.Add(SUCCESS, polizasConvertidas.Any());
                }
                else
                {
                    var polizasEnkontrolColombia = PolizasEnkontrolColombia();
                    polizasConvertidas = PolizasConversionEstatus(polizasEnkontrolColombia);
                    resultado.Add("lst", polizasConvertidas);
                    resultado.Add(SUCCESS, polizasConvertidas.Any());
                }

            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                LogError(0, 0, nombreControlador, "BuscarCaptura", e, AccionEnum.CONSULTA, 0, null);
            }
            return resultado;
        }
        List<tblC_SC_ConversionPoliza> ConversionPolizasColombiaCplan()
        {
            return _context.tblC_SC_ConversionPoliza.Where(r => r.esActivo && r.PalEmpresa == EmpresaEnum.Colombia && r.SecEmpresa == EmpresaEnum.Construplan).ToList();
        }
        List<tblC_SC_ConversionPoliza> ConversionPolizasPeruCplan()
        {
            return _context.tblC_SC_ConversionPoliza.Where(r => r.esActivo && r.PalEmpresa == EmpresaEnum.Peru && r.SecEmpresa == EmpresaEnum.Construplan).ToList();
        }
        List<tblC_Cta_RelCuentas> CuentasColombiaCplan()
        {
            return _context.tblC_Cta_RelCuentas.Where(w => w.esActivo && w.palEmpresa == EmpresaEnum.Colombia && w.secEmpresa == EmpresaEnum.Construplan).ToList();
        }
        List<tblC_Cta_RelCuentas> CuentasPeruCplan()
        {
            return _context.tblC_Cta_RelCuentas.Where(w => w.esActivo && w.palEmpresa == EmpresaEnum.Peru && w.secEmpresa == EmpresaEnum.Construplan).ToList();
        }
        List<tblC_CC_RelObras> ObrasColombiaCplan()
        {
            return _context.tblC_CC_RelObras.Where(w => w.esActivo && w.PalEmpresa == EmpresaEnum.Colombia && w.SecEmpresa == EmpresaEnum.Construplan).ToList();
        }

        List<tblC_Cta_RelCC> ObrasPeruCplan()
        {
            return _context.tblC_Cta_RelCC.Where(w => w.esActivo).ToList();
        }
        List<tblC_TM_Relitm> iTiposMovimientoColombiaCplan()
        {
            return _context.tblC_TM_Relitm.Where(w => w.esActivo && w.PalEmpresa == EmpresaEnum.Colombia && w.SecEmpresa == EmpresaEnum.Construplan).ToList();
        }
        List<tblC_TM_Relitm> iTiposMovimientoPeruCplan()
        {
            return _context.tblC_TM_Relitm.Where(w => w.esActivo && w.PalEmpresa == EmpresaEnum.Peru && w.SecEmpresa == EmpresaEnum.Construplan).ToList();
        }
        List<CtaintDTO> CuentasSistemas(EmpresaEnum empresa)
        {
            var consulta = new OdbcConsultaDTO()
            {
                consulta = "SELECT * FROM \"DBA\".\"ctaint\"",
            };
            //var baseDatos = empresa == EmpresaEnum.Construplan ? EnkontrolEnum.PruebaCplanProd : _contextEnkontrol.BaseDatosProductivoDesdeEmpresa(empresa);
            var baseDatos = _contextEnkontrol.BaseDatosProductivoDesdeEmpresa(empresa);
            return _contextEnkontrol.Select<CtaintDTO>(baseDatos, consulta);
        }
        List<CtaintDTO> CuentasSistemasBancario(EmpresaEnum empresa)
        {
            var consulta = new OdbcConsultaDTO()
            {
                consulta = "SELECT * FROM \"DBA\".\"ctaint\" WHERE sistema = 'B'",
            };
            //var baseDatos = empresa == EmpresaEnum.Construplan ? EnkontrolEnum.PruebaCplanProd : _contextEnkontrol.BaseDatosProductivoDesdeEmpresa(empresa);
            var baseDatos = _contextEnkontrol.BaseDatosProductivoDesdeEmpresa(empresa);
            return _contextEnkontrol.Select<CtaintDTO>(baseDatos, consulta);
        }
        List<PolizasDTO> PolizasEnkontrolPeru(int year)
        {
            List<PolizasDTO> modResultado = new List<PolizasDTO>(); ;
            DynamicParameters lstParametros = new DynamicParameters();

            try
            {
                var polizasCol = ConversionPolizasPeruCplan();
//                                var sql = @"
//                                     SELECT
//                                            2022 AS year,     
//                                            CAST(12 as int) AS mes,                 										  
//						                    SUBDIAR_CODIGO as tp,
//						                    CAST(DMOV_C_COMPR as int) as poliza,	
//						                    Convert(datetime, Max(DMOV_FECHA),103) AS fechapol,
//		                                    SUM(DMOV_DEBE) AS cargos,	
//					                        SUM(DMOV_HABER) AS abonos,							
//					                        '' as generada,
//                                            'C' AS status,
//                                            'N' AS status_lock,
//						                    max(DMOV_GLOSA) AS concepto
//	                                        from [003BDCONT2022].[dbo].[DETMOV12]
//				                        GROUP BY SUBDIAR_CODIGO,DMOV_C_COMPR";


                                var sql = string.Format(@"
                     SELECT
                        {0} AS year,     
                        CAST(01 as int) AS mes,                 										  
						SUBDIAR_CODIGO as tp,
						CAST(DMOV_C_COMPR as int) as poliza,							
						Convert(datetime, Max(DMOV_FECHA),103) AS fechapol,
		                SUM(DMOV_DEBE) AS cargos,	
					    SUM(DMOV_HABER) AS abonos,	
                         SUM(DMOV_DEBUS) AS cargosUs,	
					    SUM(DMOV_HABUS) AS abonosUs,							
					    '' as generada,
                        'C' AS status,
                        'N' AS status_lock,
						max(DMOV_GLOSA) AS concepto
	                FROM [003BDCONT{0}].[dbo].[DETMOV01]
				    GROUP BY SUBDIAR_CODIGO,DMOV_C_COMPR
                    UNION
                    SELECT
                        {0} AS year,     
                        CAST(02 as int) AS mes,                 										  
						SUBDIAR_CODIGO as tp,
						CAST(DMOV_C_COMPR as int) as poliza,							
						Convert(datetime, Max(DMOV_FECHA),103) AS fechapol,
		                SUM(DMOV_DEBE) AS cargos,	
					    SUM(DMOV_HABER) AS abonos,	
                     SUM(DMOV_DEBUS) AS cargosUs,	
					    SUM(DMOV_HABUS) AS abonosUs,						
					    '' as generada,
                        'C' AS status,
                        'N' AS status_lock,
						max(DMOV_GLOSA) AS concepto
	                FROM [003BDCONT{0}].[dbo].[DETMOV02]
				    GROUP BY SUBDIAR_CODIGO,DMOV_C_COMPR
                    UNION
                    SELECT
                        {0} AS year,     
                        CAST(03 as int) AS mes,                 										  
						SUBDIAR_CODIGO as tp,
						CAST(DMOV_C_COMPR as int) as poliza,							
						Convert(datetime, Max(DMOV_FECHA),103) AS fechapol,
		                SUM(DMOV_DEBE) AS cargos,	
					    SUM(DMOV_HABER) AS abonos,	
                         SUM(DMOV_DEBUS) AS cargosUs,	
					    SUM(DMOV_HABUS) AS abonosUs,
					    '' as generada,
                        'C' AS status,
                        'N' AS status_lock,
						max(DMOV_GLOSA) AS concepto
	                FROM [003BDCONT{0}].[dbo].[DETMOV03]
				    GROUP BY SUBDIAR_CODIGO,DMOV_C_COMPR
                    UNION
                    SELECT
                        {0} AS year,     
                        CAST(04 as int) AS mes,                 										  
						SUBDIAR_CODIGO as tp,
						CAST(DMOV_C_COMPR as int) as poliza,							
						Convert(datetime, Max(DMOV_FECHA),103) AS fechapol,
		                SUM(DMOV_DEBE) AS cargos,	
					    SUM(DMOV_HABER) AS abonos,	
                         SUM(DMOV_DEBUS) AS cargosUs,	
					    SUM(DMOV_HABUS) AS abonosUs,					
					    '' as generada,
                        'C' AS status,
                        'N' AS status_lock,
						max(DMOV_GLOSA) AS concepto
	                FROM [003BDCONT{0}].[dbo].[DETMOV04]
				    GROUP BY SUBDIAR_CODIGO,DMOV_C_COMPR
                    UNION
                    SELECT
                        {0} AS year,     
                        CAST(05 as int) AS mes,                 										  
						SUBDIAR_CODIGO as tp,
						CAST(DMOV_C_COMPR as int) as poliza,							
						Convert(datetime, Max(DMOV_FECHA),103) AS fechapol,
		                SUM(DMOV_DEBE) AS cargos,	
					    SUM(DMOV_HABER) AS abonos,
                         SUM(DMOV_DEBUS) AS cargosUs,	
					    SUM(DMOV_HABUS) AS abonosUs,
					    '' as generada,
                        'C' AS status,
                        'N' AS status_lock,
						max(DMOV_GLOSA) AS concepto
	                FROM [003BDCONT{0}].[dbo].[DETMOV05]
				    GROUP BY SUBDIAR_CODIGO,DMOV_C_COMPR
                    UNION
                    SELECT
                        {0} AS year,     
                        CAST(06 as int) AS mes,                 										  
						SUBDIAR_CODIGO as tp,
						CAST(DMOV_C_COMPR as int) as poliza,							
						Convert(datetime, Max(DMOV_FECHA),103) AS fechapol,
		                SUM(DMOV_DEBE) AS cargos,	
					    SUM(DMOV_HABER) AS abonos,
                         SUM(DMOV_DEBUS) AS cargosUs,	
					    SUM(DMOV_HABUS) AS abonosUs,														
					    '' as generada,
                        'C' AS status,
                        'N' AS status_lock,
						max(DMOV_GLOSA) AS concepto
	                FROM [003BDCONT{0}].[dbo].[DETMOV06]
				    GROUP BY SUBDIAR_CODIGO,DMOV_C_COMPR
                    UNION
                    SELECT
                        {0} AS year,     
                        CAST(07 as int) AS mes,                 										  
						SUBDIAR_CODIGO as tp,
						CAST(DMOV_C_COMPR as int) as poliza,							
						Convert(datetime, Max(DMOV_FECHA),103) AS fechapol,
		                SUM(DMOV_DEBE) AS cargos,	
					    SUM(DMOV_HABER) AS abonos,
                         SUM(DMOV_DEBUS) AS cargosUs,	
					    SUM(DMOV_HABUS) AS abonosUs,														
					    '' as generada,
                        'C' AS status,
                        'N' AS status_lock,
						max(DMOV_GLOSA) AS concepto
	                FROM [003BDCONT{0}].[dbo].[DETMOV07]
				    GROUP BY SUBDIAR_CODIGO,DMOV_C_COMPR
                    UNION
                    SELECT
                        {0} AS year,     
                        CAST(08 as int) AS mes,                 										  
						SUBDIAR_CODIGO as tp,
						CAST(DMOV_C_COMPR as int) as poliza,							
						Convert(datetime, Max(DMOV_FECHA),103) AS fechapol,
		                SUM(DMOV_DEBE) AS cargos,	
					    SUM(DMOV_HABER) AS abonos,		
                         SUM(DMOV_DEBUS) AS cargosUs,	
					    SUM(DMOV_HABUS) AS abonosUs,												
					    '' as generada,
                        'C' AS status,
                        'N' AS status_lock,
						max(DMOV_GLOSA) AS concepto
	                FROM [003BDCONT{0}].[dbo].[DETMOV08]
				    GROUP BY SUBDIAR_CODIGO,DMOV_C_COMPR
                    UNION
                    SELECT
                        {0} AS year,     
                        CAST(09 as int) AS mes,                 										  
						SUBDIAR_CODIGO as tp,
						CAST(DMOV_C_COMPR as int) as poliza,							
						Convert(datetime, Max(DMOV_FECHA),103) AS fechapol,
		                SUM(DMOV_DEBE) AS cargos,	
					    SUM(DMOV_HABER) AS abonos,	
                         SUM(DMOV_DEBUS) AS cargosUs,	
					    SUM(DMOV_HABUS) AS abonosUs,													
					    '' as generada,
                        'C' AS status,
                        'N' AS status_lock,
						max(DMOV_GLOSA) AS concepto
	                FROM [003BDCONT{0}].[dbo].[DETMOV09]
				    GROUP BY SUBDIAR_CODIGO,DMOV_C_COMPR
                    UNION
                    SELECT
                        {0} AS year,     
                        CAST(10 as int) AS mes,                 										  
						SUBDIAR_CODIGO as tp,
						CAST(DMOV_C_COMPR as int) as poliza,							
						Convert(datetime, Max(DMOV_FECHA),103) AS fechapol,
		                SUM(DMOV_DEBE) AS cargos,	
					    SUM(DMOV_HABER) AS abonos,
                         SUM(DMOV_DEBUS) AS cargosUs,	
					    SUM(DMOV_HABUS) AS abonosUs,														
					    '' as generada,
                        'C' AS status,
                        'N' AS status_lock,
						max(DMOV_GLOSA) AS concepto
	                FROM [003BDCONT{0}].[dbo].[DETMOV10]
				    GROUP BY SUBDIAR_CODIGO,DMOV_C_COMPR
                    UNION
                    SELECT
                        {0} AS year,     
                        CAST(11 as int) AS mes,                 										  
						SUBDIAR_CODIGO as tp,
						CAST(DMOV_C_COMPR as int) as poliza,							
						Convert(datetime, Max(DMOV_FECHA),103) AS fechapol,
		                SUM(DMOV_DEBE) AS cargos,	
					    SUM(DMOV_HABER) AS abonos,	
                         SUM(DMOV_DEBUS) AS cargosUs,	
					    SUM(DMOV_HABUS) AS abonosUs,													
					    '' as generada,
                        'C' AS status,
                        'N' AS status_lock,
						max(DMOV_GLOSA) AS concepto
	                FROM [003BDCONT{0}].[dbo].[DETMOV11]
				    GROUP BY SUBDIAR_CODIGO,DMOV_C_COMPR
                    UNION
                    SELECT
                        {0} AS year,     
                        CAST(12 as int) AS mes,                 										  
						SUBDIAR_CODIGO as tp,
						CAST(DMOV_C_COMPR as int) as poliza,							
						Convert(datetime, Max(DMOV_FECHA),103) AS fechapol,
		                SUM(DMOV_DEBE) AS cargos,	
					    SUM(DMOV_HABER) AS abonos,	
                         SUM(DMOV_DEBUS) AS cargosUs,	
					    SUM(DMOV_HABUS) AS abonosUs,													
					    '' as generada,
                        'C' AS status,
                        'N' AS status_lock,
						max(DMOV_GLOSA) AS concepto
	                FROM [003BDCONT{0}].[dbo].[DETMOV12]                    
				    GROUP BY SUBDIAR_CODIGO,DMOV_C_COMPR", year);



                using (var conexion = new SqlConnection(ConextSigoDapper.conexionStarsoftBancos()))
                {
                    conexion.Open();
                    //modResultado = conexion.Query<PolizasDTO>(sql, lstParametros, null, true, 300, commandType: CommandType.Text).Where(x => x.year == 2023 && x.mes == 1).ToList();
                    modResultado = conexion.Query<PolizasDTO>(sql, lstParametros, null, true, 300, commandType: CommandType.Text).Where(pol => !polizasCol.Any(col => col.PalYear == pol.year && col.PalMes == pol.mes && col.PalTP == pol.tp && col.PalPoliza == pol.poliza)).ToList();
                    conexion.Close();
                }
            }
            catch (Exception ex)
            {
                LogError(0, 0, "RentabilidadController", "PolizasEnkontrolPeru", ex, AccionEnum.CONSULTA, 0, modResultado);
            }
            return modResultado;
        }
        List<PolizasDTO> PolizasEnkontrolColombia()
        {
            var polizasCol = ConversionPolizasColombiaCplan();
            var consulta = new OdbcConsultaDTO()
            {
                consulta = string.Format(@"SELECT DISTINCT pol.year, pol.mes, pol.tp, pol.poliza, MAX(pol.fechapol) AS fechapol, MAX(pol.cargos) AS cargos, MAX(pol.abonos) AS abonos
                            FROM ""DBA"".""sc_polizas"" pol
                            WHERE (pol.year >= 2020 AND pol.mes >= 11) or  pol.year >= 2021
                            GROUP BY pol.year, pol.mes, pol.tp, pol.poliza
                            ORDER BY pol.year DESC, pol.mes DESC, pol.poliza DESC")
            };


            var polizas = _contextEnkontrol.Select<PolizasDTO>(EnkontrolEnum.ColombiaProductivo, consulta);
            polizas = polizas.Where(pol => !polizasCol.Any(col => col.PalYear == pol.year && col.PalMes == pol.mes && col.PalTP == pol.tp && col.PalPoliza == pol.poliza)).ToList();
            return polizas;
        }
        List<PolizasDTO> VerificacionRelaciones(List<PolizasDTO> lst)
        {

            var polizas = new List<PolizasDTO>();
            var cuentas = CuentasColombiaCplan();
            var cuentasPeru = CuentasPeruCplan();
            var obras = ObrasColombiaCplan();
            var obrasPeru = ObrasPeruCplan();
            var iTiposMov = iTiposMovimientoColombiaCplan();
            var iTiposMovPeru = iTiposMovimientoPeruCplan();
            var cuentasSistema = CuentasSistemasBancario(EmpresaEnum.Construplan);

            DynamicParameters lstParametros = new DynamicParameters();

            if (vSesiones.sesionEmpresaActual == 3)
            {
                lst.ForEach(poliza =>
                {
                    var lstValida = new List<bool>();
                    var odbc = new OdbcConsultaDTO
                    {
                        consulta = "SELECT * FROM \"DBA\".\"sc_movpol\" WHERE year = ? AND mes = ? AND tp = ? AND poliza = ?"
                    };
                    odbc.parametros.Add(new OdbcParameterDTO { nombre = "year", tipo = OdbcType.Int, valor = poliza.year });
                    odbc.parametros.Add(new OdbcParameterDTO { nombre = "mes", tipo = OdbcType.Int, valor = poliza.mes });
                    odbc.parametros.Add(new OdbcParameterDTO { nombre = "tp", tipo = OdbcType.VarChar, valor = poliza.tp });
                    odbc.parametros.Add(new OdbcParameterDTO { nombre = "poliza", tipo = OdbcType.Int, valor = poliza.poliza });
                    var movpol = _contextEnkontrol.Select<MovpolDTO>(EnkontrolEnum.ColombiaProductivo, odbc);

                    movpol.ForEach(mov =>
                    {
                        var cuenta = cuentas.FirstOrDefault(cta => cta.palCta == mov.cta && cta.palScta == mov.scta && cta.palSscta == mov.sscta) ?? new tblC_Cta_RelCuentas();
                        var cuentaSistema = cuentasSistema.FirstOrDefault(cta => cta.cuenta == cuenta.secCta && CuentasSistemasValidos.Contains(cta.sistema)) ?? new CtaintDTO();
                        var tipoMov = iTiposMov.FirstOrDefault(itm => itm.PaliTm == mov.itm && cuentaSistema.cuenta > 0) ?? new tblC_TM_Relitm();
                        var obra = obras.FirstOrDefault(cc => cc.PalObra == mov.cc) ?? new tblC_CC_RelObras();
                        var puedeSincCta = cuenta.id > 0;
                        var puedeSincItm = cuentaSistema.cuenta > 0 && tipoMov.Id > 0 ? true : cuentaSistema.cuenta == 0;
                        var puedeSincObra = mov.cc == string.Empty || obra.Id > 0;
                        if (puedeSincCta && puedeSincItm && puedeSincObra)
                        {
                            lstValida.Add(true);
                        }
                    });
                    var sonTodasConvertibles = movpol.Count == lstValida.Count;
                    if (movpol.Any() && sonTodasConvertibles)
                    {
                        polizas.Add(poliza);
                    }
                });
            }
            else
            {
                lst.ForEach(poliza =>
                {
                    var lstValidaPeru = new List<bool>();
                    var anioPoliza = poliza.year;
//                                        var sql = @"
//                                                SELECT
//		                                                2022 AS year,
//		                                                CAST(12 as int) AS mes,
//		                                                SUBDIAR_CODIGO AS tp,
//		                                                CAST(DMOV_C_COMPR AS int) AS poliza,
//		                                                SUM(DMOV_DEBE) AS cargos,
//		                                                SUM(DMOV_HABER) AS abonos,
//		                                                'C' AS status,
//		                                                'N' AS status_lock,
//		                                                MAX(DMOV_GLOSA) AS concepto,
//                                                        DMOV_CENCO as cc,
//                                                        DMOV_CUENT as cta,
//                                                        0 as scta,
//                                                        0 as sscta
//	                                                FROM [003BDCONT2022].dbo.DETMOV12 GROUP BY SUBDIAR_CODIGO,DMOV_C_COMPR,DMOV_CUENT,DMOV_CENCO ";

                    var sql = string.Format(@"
                   	SELECT
		                {0} AS year,
		                CAST(01 as int) AS mes,
		                SUBDIAR_CODIGO AS tp,
		                CAST(DMOV_C_COMPR AS int) AS poliza,
		                SUM(DMOV_DEBE) AS cargos,
		                SUM(DMOV_HABER) AS abonos,
                        SUM(DMOV_DEBUS) AS cargosUs,
		                SUM(DMOV_HABUS) AS abonosUs,
		                'C' AS status,
		                'N' AS status_lock,
		                MAX(DMOV_GLOSA) AS concepto,
                        DMOV_CENCO as cc,
                        DMOV_CUENT as cta,
                        0 as scta,
                        0 as sscta
	                FROM [003BDCONT{0}].dbo.DETMOV01 
                    GROUP BY SUBDIAR_CODIGO,DMOV_C_COMPR,DMOV_CUENT,DMOV_CENCO
                    UNION
 	                SELECT
		                {0} AS year,
		                CAST(02 as int) AS mes,
		                SUBDIAR_CODIGO AS tp,
		                CAST(DMOV_C_COMPR AS int) AS poliza,
		                SUM(DMOV_DEBE) AS cargos,
		                SUM(DMOV_HABER) AS abonos,
                        SUM(DMOV_DEBUS) AS cargosUs,	
					    SUM(DMOV_HABUS) AS abonosUs,
		                'C' AS status,
		                'N' AS status_lock,
		                MAX(DMOV_GLOSA) AS concepto,
                        DMOV_CENCO as cc,
                        DMOV_CUENT as cta,
                        0 as scta,
                        0 as sscta
	                FROM [003BDCONT{0}].dbo.DETMOV02 
                    GROUP BY SUBDIAR_CODIGO,DMOV_C_COMPR,DMOV_CUENT,DMOV_CENCO
                    UNION
 	                SELECT
		                {0} AS year,
		                CAST(03 as int) AS mes,
		                SUBDIAR_CODIGO AS tp,
		                CAST(DMOV_C_COMPR AS int) AS poliza,
		                SUM(DMOV_DEBE) AS cargos,
		                SUM(DMOV_HABER) AS abonos,
                        SUM(DMOV_DEBUS) AS cargosUs,	
					    SUM(DMOV_HABUS) AS abonosUs,
		                'C' AS status,
		                'N' AS status_lock,
		                MAX(DMOV_GLOSA) AS concepto,
                        DMOV_CENCO as cc,
                        DMOV_CUENT as cta,
                        0 as scta,
                        0 as sscta
	                FROM [003BDCONT{0}].dbo.DETMOV03 
                    GROUP BY SUBDIAR_CODIGO,DMOV_C_COMPR,DMOV_CUENT,DMOV_CENCO
                    UNION
 	                SELECT
		                {0} AS year,
		                CAST(04 as int) AS mes,
		                SUBDIAR_CODIGO AS tp,
		                CAST(DMOV_C_COMPR AS int) AS poliza,
		                SUM(DMOV_DEBE) AS cargos,
		                SUM(DMOV_HABER) AS abonos,
                        SUM(DMOV_DEBUS) AS cargosUs,	
					    SUM(DMOV_HABUS) AS abonosUs,
		                'C' AS status,
		                'N' AS status_lock,
		                MAX(DMOV_GLOSA) AS concepto,
                        DMOV_CENCO as cc,
                        DMOV_CUENT as cta,
                        0 as scta,
                        0 as sscta
	                FROM [003BDCONT{0}].dbo.DETMOV04 
                    GROUP BY SUBDIAR_CODIGO,DMOV_C_COMPR,DMOV_CUENT,DMOV_CENCO
                    UNION
 	                SELECT
		                {0} AS year,
		                CAST(05 as int) AS mes,
		                SUBDIAR_CODIGO AS tp,
		                CAST(DMOV_C_COMPR AS int) AS poliza,
		                SUM(DMOV_DEBE) AS cargos,
		                SUM(DMOV_HABER) AS abonos,
                        SUM(DMOV_DEBUS) AS cargosUs,	
					    SUM(DMOV_HABUS) AS abonosUs,
		                'C' AS status,
		                'N' AS status_lock,
		                MAX(DMOV_GLOSA) AS concepto,
                        DMOV_CENCO as cc,
                        DMOV_CUENT as cta,
                        0 as scta,
                        0 as sscta
	                FROM [003BDCONT{0}].dbo.DETMOV05
                    GROUP BY SUBDIAR_CODIGO,DMOV_C_COMPR,DMOV_CUENT,DMOV_CENCO
                    UNION
 	                SELECT
		                {0} AS year,
		                CAST(06 as int) AS mes,
		                SUBDIAR_CODIGO AS tp,
		                CAST(DMOV_C_COMPR AS int) AS poliza,
		                SUM(DMOV_DEBE) AS cargos,
		                SUM(DMOV_HABER) AS abonos,
                        SUM(DMOV_DEBUS) AS cargosUs,	
					    SUM(DMOV_HABUS) AS abonosUs,
		                'C' AS status,
		                'N' AS status_lock,
		                MAX(DMOV_GLOSA) AS concepto,
                        DMOV_CENCO as cc,
                        DMOV_CUENT as cta,
                        0 as scta,
                        0 as sscta
	                FROM [003BDCONT{0}].dbo.DETMOV06 
                    GROUP BY SUBDIAR_CODIGO,DMOV_C_COMPR,DMOV_CUENT,DMOV_CENCO
                    UNION
 	                SELECT
		                {0} AS year,
		                CAST(07 as int) AS mes,
		                SUBDIAR_CODIGO AS tp,
		                CAST(DMOV_C_COMPR AS int) AS poliza,
		                SUM(DMOV_DEBE) AS cargos,
		                SUM(DMOV_HABER) AS abonos,
                        SUM(DMOV_DEBUS) AS cargosUs,	
					    SUM(DMOV_HABUS) AS abonosUs,
		                'C' AS status,
		                'N' AS status_lock,
		                MAX(DMOV_GLOSA) AS concepto,
                        DMOV_CENCO as cc,
                        DMOV_CUENT as cta,
                        0 as scta,
                        0 as sscta
	                FROM [003BDCONT{0}].dbo.DETMOV07
                    GROUP BY SUBDIAR_CODIGO,DMOV_C_COMPR,DMOV_CUENT,DMOV_CENCO
                    UNION
 	                SELECT
		                {0} AS year,
		                CAST(08 as int) AS mes,
		                SUBDIAR_CODIGO AS tp,
		                CAST(DMOV_C_COMPR AS int) AS poliza,
		                SUM(DMOV_DEBE) AS cargos,
		                SUM(DMOV_HABER) AS abonos,
                        SUM(DMOV_DEBUS) AS cargosUs,	
					    SUM(DMOV_HABUS) AS abonosUs,
		                'C' AS status,
		                'N' AS status_lock,
		                MAX(DMOV_GLOSA) AS concepto,
                        DMOV_CENCO as cc,
                        DMOV_CUENT as cta,
                        0 as scta,
                        0 as sscta
	                FROM [003BDCONT{0}].dbo.DETMOV08 
                    GROUP BY SUBDIAR_CODIGO,DMOV_C_COMPR,DMOV_CUENT,DMOV_CENCO
                    UNION
 	                SELECT
		                {0} AS year,
		                CAST(09 as int) AS mes,
		                SUBDIAR_CODIGO AS tp,
		                CAST(DMOV_C_COMPR AS int) AS poliza,
		                SUM(DMOV_DEBE) AS cargos,
		                SUM(DMOV_HABER) AS abonos,
                        SUM(DMOV_DEBUS) AS cargosUs,	
					    SUM(DMOV_HABUS) AS abonosUs,
		                'C' AS status,
		                'N' AS status_lock,
		                MAX(DMOV_GLOSA) AS concepto,
                        DMOV_CENCO as cc,
                        DMOV_CUENT as cta,
                        0 as scta,
                        0 as sscta
	                FROM [003BDCONT{0}].dbo.DETMOV09 
                    GROUP BY SUBDIAR_CODIGO,DMOV_C_COMPR,DMOV_CUENT,DMOV_CENCO
                    UNION
 	                SELECT
		                {0} AS year,
		                CAST(10 as int) AS mes,
		                SUBDIAR_CODIGO AS tp,
		                CAST(DMOV_C_COMPR AS int) AS poliza,
		                SUM(DMOV_DEBE) AS cargos,
		                SUM(DMOV_HABER) AS abonos,
                        SUM(DMOV_DEBUS) AS cargosUs,	
					    SUM(DMOV_HABUS) AS abonosUs,
		                'C' AS status,
		                'N' AS status_lock,
		                MAX(DMOV_GLOSA) AS concepto,
                        DMOV_CENCO as cc,
                        DMOV_CUENT as cta,
                        0 as scta,
                        0 as sscta
	                FROM [003BDCONT{0}].dbo.DETMOV10 
                    GROUP BY SUBDIAR_CODIGO,DMOV_C_COMPR,DMOV_CUENT,DMOV_CENCO
                    UNION
 	                SELECT
		                {0} AS year,
		                CAST(11 as int) AS mes,
		                SUBDIAR_CODIGO AS tp,
		                CAST(DMOV_C_COMPR AS int) AS poliza,
		                SUM(DMOV_DEBE) AS cargos,
		                SUM(DMOV_HABER) AS abonos,
                        SUM(DMOV_DEBUS) AS cargosUs,	
					    SUM(DMOV_HABUS) AS abonosUs,
		                'C' AS status,
		                'N' AS status_lock,
		                MAX(DMOV_GLOSA) AS concepto,
                        DMOV_CENCO as cc,
                        DMOV_CUENT as cta,
                        0 as scta,
                        0 as sscta
	                FROM [003BDCONT{0}].dbo.DETMOV11 
                    GROUP BY SUBDIAR_CODIGO,DMOV_C_COMPR,DMOV_CUENT,DMOV_CENCO
                    UNION
 	                SELECT
		                {0} AS year,
		                CAST(12 as int) AS mes,
		                SUBDIAR_CODIGO AS tp,
		                CAST(DMOV_C_COMPR AS int) AS poliza,
		                SUM(DMOV_DEBE) AS cargos,
		                SUM(DMOV_HABER) AS abonos,
                        SUM(DMOV_DEBUS) AS cargosUs,	
					    SUM(DMOV_HABUS) AS abonosUs,
		                'C' AS status,
		                'N' AS status_lock,
		                MAX(DMOV_GLOSA) AS concepto,
                        DMOV_CENCO as cc,
                        DMOV_CUENT as cta,
                        0 as scta,
                        0 as sscta
	                FROM [003BDCONT{0}].dbo.DETMOV12 
                    GROUP BY SUBDIAR_CODIGO,DMOV_C_COMPR,DMOV_CUENT,DMOV_CENCO ", anioPoliza);

                    List<MovpolDTO> lstMovPol = new List<MovpolDTO>(); ;
                    using (var conexion = new SqlConnection(ConextSigoDapper.conexionStarsoftBancos()))
                    {
                        conexion.Open();
                        lstMovPol = conexion.Query<MovpolDTO>(sql, lstParametros, null, true, 300, commandType: CommandType.Text).ToList();

                        conexion.Close();
                    }
                    lstMovPol.ForEach(mov =>
                    {

                        var cuenta = cuentasPeru.FirstOrDefault(cta => cta.palCta == mov.cta && cta.palScta == mov.scta && cta.palSscta == mov.sscta) ?? new tblC_Cta_RelCuentas();
                        var cuentaSistema = cuentasSistema.FirstOrDefault(cta => cta.cuenta == cuenta.secCta && CuentasSistemasValidos.Contains(cta.sistema)) ?? new CtaintDTO();
                        var tipoMov = iTiposMovPeru.FirstOrDefault(itm => itm.PaliTm == mov.itm && cuentaSistema.cuenta > 0) ?? new tblC_TM_Relitm();

                        //var puedeSincCta = cuenta.id > 0;
                        //var puedeSincItm = cuentaSistema.cuenta > 0 && tipoMov.Id > 0 ? true : cuentaSistema.cuenta == 0;
                        //var puedeSincObra = mov.cc == string.Empty || obra.id > 0;
                        //if (puedeSincCta && puedeSincItm && puedeSincObra)
                        //{
                        //    lstValidaPeru.Add(true);
                        //}
                        var puedeSincObra = mov.cc == string.Empty;
                        if (vSesiones.sesionEmpresaActual == 6)
                        {
                            var obra = obrasPeru.FirstOrDefault(cc => cc.ccSecundario == mov.cc);
                            if (obra == null) mov.cc = "032";
                            else mov.cc = mov.cc; 
                            puedeSincObra = mov.cc == string.Empty;
                        }
                        var puedeSincCta = cuenta.id > 0;
                        var puedeSincItm = cuentaSistema.cuenta > 0 && tipoMov.Id > 0 ? true : cuentaSistema.cuenta == 0;

                        if (vSesiones.sesionEmpresaActual == 6)
                        {
                            puedeSincCta = true;
                            puedeSincItm = true;
                            puedeSincObra = true;
                        }

                        if (puedeSincCta && puedeSincItm && puedeSincObra)
                        {
                            lstValidaPeru.Add(true);
                        }
                    });

                    var sonTodasConvertiblesPeru = lstMovPol.Count == lstValidaPeru.Count;
                    if (lstMovPol.Any() && sonTodasConvertiblesPeru)
                    {
                        polizas.Add(poliza);
                    }
                });
            }
            return polizas;
        }
        List<ConversionPolizaDTO> PolizasConversionEstatus(List<PolizasDTO> lst)
        {
            var polizas = new List<ConversionPolizaDTO>();
            var cuentas = CuentasColombiaCplan();
            var obras = ObrasColombiaCplan();
            var iTiposMov = iTiposMovimientoColombiaCplan();
            var cuentasSistema = CuentasSistemasBancario(EmpresaEnum.Construplan);
            var lstConsulta = new List<OdbcConsultaDTO>();
            lst.ForEach(poliza =>
            {
                var odbc = new OdbcConsultaDTO
                {
                    consulta = "SELECT * FROM \"DBA\".\"sc_movpol\" WHERE year = ? AND mes = ? AND tp = ? AND poliza = ?"
                };
                odbc.parametros.Add(new OdbcParameterDTO { nombre = "year", tipo = OdbcType.Int, valor = poliza.year });
                odbc.parametros.Add(new OdbcParameterDTO { nombre = "mes", tipo = OdbcType.Int, valor = poliza.mes });
                odbc.parametros.Add(new OdbcParameterDTO { nombre = "tp", tipo = OdbcType.VarChar, valor = poliza.tp });
                odbc.parametros.Add(new OdbcParameterDTO { nombre = "poliza", tipo = OdbcType.Int, valor = poliza.poliza });
                lstConsulta.Add(odbc);
            });
            var lstMovPol = _contextEnkontrol.Select<MovpolDTO>(EnkontrolEnum.ColombiaProductivo, lstConsulta);
            lst.ForEach(poliza =>
            {
                var estatus = string.Empty;
                var lstValida = new List<bool>();
                var puedeSincCta = true;
                var puedeSincItm = true;
                var puedeSincObra = true;
                var movpol = lstMovPol.Where(mov => mov.year == poliza.year && mov.mes == poliza.mes && mov.tp == poliza.tp && mov.poliza == poliza.poliza).ToList();

                var mensaje2 = new List<string>();
                for (int i = 0; i < movpol.Count; i++)
                {
                    string CtasFaltantes = "";
                    var mov = movpol[i];
                    var relCuenta = cuentas.FirstOrDefault(cta => cta.palCta == mov.cta && cta.palScta == mov.scta && cta.palSscta == mov.sscta) ?? new tblC_Cta_RelCuentas();
                    var cuentaSistema = cuentasSistema.FirstOrDefault(cta => cta.cuenta == relCuenta.secCta && CuentasSistemasValidos.Contains(cta.sistema)) ?? new CtaintDTO();
                    var tipoMov = iTiposMov.FirstOrDefault(itm => itm.PaliTm == mov.itm && cuentaSistema.cuenta > 0) ?? new tblC_TM_Relitm();
                    var obra = obras.FirstOrDefault(cc => cc.PalObra == mov.cc) ?? new tblC_CC_RelObras();
                    var movPuedeSincCta = relCuenta.id > 0;
                    //Cuentas secundarias validas y relacionadas o movimientos principales sin cuentas validas
                    var movPuedeSincItm = cuentaSistema.cuenta > 0 && tipoMov.Id > 0 ? true : cuentaSistema.cuenta == 0;
                    var movPuedeSincObra = mov.cc == string.Empty || obra.Id > 0;

                    if (!movPuedeSincCta && !movPuedeSincItm && !movPuedeSincObra)
                    {

                        puedeSincCta = false;
                        puedeSincItm = false;
                        puedeSincObra = false;
                        continue;
                    }
                    else
                    {
                        var mensaje = new List<string>();
                        if (!movPuedeSincCta)
                        {
                            CtasFaltantes = "[" + mov.cta + "-" + mov.scta + "-" + mov.sscta + "]";
                            if (!mensaje2.Exists(r => r == CtasFaltantes))
                                mensaje2.Add(CtasFaltantes);
                            puedeSincCta = false;
                        }
                        if (!movPuedeSincObra)
                        {
                            puedeSincObra = false;
                        }
                        if (!movPuedeSincItm)
                        {
                            puedeSincItm = false;
                        }
                        estatus = string.Format("Faltan {0} por relacionar", mensaje.ToLine(","));
                    }
                }

                if (puedeSincCta && puedeSincItm && puedeSincObra)
                {
                    estatus = "OK";
                }
                else if (!puedeSincCta && !puedeSincItm && !puedeSincObra)
                {
                    estatus = "No se puede convertir";
                }
                else
                {
                    var mensaje = new List<string>();
                    if (!puedeSincCta)
                    {
                        mensaje.Add("Cuentas ");
                        mensaje.AddRange(mensaje2);
                    }
                    if (!puedeSincObra)
                    {
                        mensaje.Add("Centro Costos ");
                    }
                    if (!puedeSincItm)
                    {
                        mensaje.Add("Tipos Movimientos ");
                    }
                    estatus = string.Format("Faltan {0} por relacionar", mensaje.ToLine(",").Replace("'"[0], ' '));
                }
                polizas.Add(new ConversionPolizaDTO
                {
                    PalEmpresa = EmpresaEnum.Colombia,
                    PalYear = poliza.year,
                    PalMes = poliza.mes,
                    PalTP = poliza.tp,
                    PalPoliza = poliza.poliza,
                    PalCargo = poliza.cargos,
                    PalAbono = poliza.abonos,
                    PalFechapol = poliza.fechapol,
                    fechaPoliza = poliza.fechapol.ToShortDateString(),
                    SecEmpresa = EmpresaEnum.Construplan,
                    SecCargo = convertirPesosColAPesosMX(poliza.cargos),
                    SecAbono = convertirPesosColAPesosMX(poliza.abonos),
                    Estatus = estatus
                });
            });
            return polizas;
        }
        List<ConversionPolizaDTO> PolizasConversionEstatusPeru(List<PolizasDTO> lst)
        {
            var polizas = new List<ConversionPolizaDTO>();
            var cuentas = CuentasPeruCplan();
            var obras = ObrasPeruCplan();

            var iTiposMov = iTiposMovimientoPeruCplan();
            var cuentasSistema = CuentasSistemasBancario(EmpresaEnum.Construplan);
            var lstConsulta = new List<OdbcConsultaDTO>();


            List<MovpolDTO> lstMovPol = new List<MovpolDTO>(); ;
            DynamicParameters lstParametros = new DynamicParameters();
//            string consultaGeneral = @"
//                                      SELECT
//                                        2022 AS year,
//                                        CAST(12 as int) AS mes,										  
//						                SUBDIAR_CODIGO as tp,
//						                CAST(DMOV_C_COMPR as int) as poliza,
//						                CAST(DMOV_SECUE as int) as linea,	
//						                DMOV_CUENT as cta,
//						                0 AS sscta,
//						                0 AS sscta,
//	                                    0 AS digito,
//						                CASE WHEN DMOV_DEBE <> 0 THEN (CASE WHEN DMOV_DEBE > 0 THEN 1 ELSE 3 END) ELSE (CASE WHEN DMOV_HABER <> 0 THEN (CASE WHEN DMOV_HABER > 0 THEN 2 ELSE 4 END) ELSE 1 END) END AS tm,		            
//						                DMOV_CENCO as cc,
//					                    DMOV_GLOSA AS concepto,
//						                DMOV_DEBE - DMOV_HABER AS monto,
//						                0 AS iclave,
//						                1 AS itm,
//						                '' AS orden_compra,
//						                NULL AS numpro,
//						                '' AS forma_pago,	          	
//					                    Convert(datetime,(DMOV_FECHA)) AS fechapol,
//						                NULL AS area,
//						                NULL AS cuenta_oc,
//						                6 AS empresa,
//						                GETDATE() as fecha
//					                 FROM [003BDCONT2022].dbo.DETMOV12 WHERE ";

            string consultaGeneral = @"
                         			SELECT
                                        2023 AS year,
                                        CAST(01 as int) AS mes,										  
						                SUBDIAR_CODIGO as tp,
						                CAST(DMOV_C_COMPR as int) as poliza,
						                CAST(DMOV_SECUE as int) as linea,	
						                DMOV_CUENT as cta,
						                0 AS sscta,
						                0 AS sscta,
	                                    0 AS digito,
                                        CASE WHEN DMOV_DEBE <> 0 THEN (CASE WHEN DMOV_DEBE > 0 THEN 1 ELSE 3 END) ELSE (CASE WHEN DMOV_HABER <> 0 THEN (CASE WHEN DMOV_HABER > 0 THEN 2 ELSE 4 END) ELSE 1 END) END AS tm,		            
						                DMOV_CENCO as cc,
					                    DMOV_GLOSA AS concepto,
						                DMOV_DEBE - DMOV_HABER AS monto,
                                        DMOV_DEBUS - DMOV_HABUS AS montoUs,
						                0 AS iclave,
						                1 AS itm,
						                '' AS orden_compra,
						                NULL AS numpro,
						                '' AS forma_pago,	          	
					                    Convert(datetime,(DMOV_FECHA)) AS fechapol,
						                NULL AS area,
						                NULL AS cuenta_oc,
						                6 AS empresa,
						                GETDATE() as fecha,
                                        RIGHT('00000000000'+ RTRIM(ISNULL(DMOV_ANEXO, '0')), 11) AS proveedorPeru
					                 FROM [003BDCONT2023].dbo.DETMOV01
                                     UNION
	                                 SELECT
                                        2023 AS year,
                                        CAST(02 as int) AS mes,										  
						                SUBDIAR_CODIGO as tp,
						                CAST(DMOV_C_COMPR as int) as poliza,
						                CAST(DMOV_SECUE as int) as linea,	
						                DMOV_CUENT as cta,
						                0 AS sscta,
						                0 AS sscta,
	                                    0 AS digito,
                                        CASE WHEN DMOV_DEBE <> 0 THEN (CASE WHEN DMOV_DEBE > 0 THEN 1 ELSE 3 END) ELSE (CASE WHEN DMOV_HABER <> 0 THEN (CASE WHEN DMOV_HABER > 0 THEN 2 ELSE 4 END) ELSE 1 END) END AS tm,		            
						                DMOV_CENCO as cc,
					                    DMOV_GLOSA AS concepto,
						                DMOV_DEBE - DMOV_HABER AS monto,
                                        DMOV_DEBUS - DMOV_HABUS AS montoUs,
						                0 AS iclave,
						                1 AS itm,
						                '' AS orden_compra,
						                NULL AS numpro,
						                '' AS forma_pago,	          	
					                    Convert(datetime,(DMOV_FECHA)) AS fechapol,
						                NULL AS area,
						                NULL AS cuenta_oc,
						                6 AS empresa,
						                GETDATE() as fecha,
                                        RIGHT('00000000000'+ RTRIM(ISNULL(DMOV_ANEXO, '0')), 11) AS proveedorPeru
					                 FROM [003BDCONT2023].dbo.DETMOV02
                                     UNION
	                                 SELECT
                                        2023 AS year,
                                        CAST(03 as int) AS mes,										  
						                SUBDIAR_CODIGO as tp,
						                CAST(DMOV_C_COMPR as int) as poliza,
						                CAST(DMOV_SECUE as int) as linea,	
						                DMOV_CUENT as cta,
						                0 AS sscta,
						                0 AS sscta,
	                                    0 AS digito,
                                        CASE WHEN DMOV_DEBE <> 0 THEN (CASE WHEN DMOV_DEBE > 0 THEN 1 ELSE 3 END) ELSE (CASE WHEN DMOV_HABER <> 0 THEN (CASE WHEN DMOV_HABER > 0 THEN 2 ELSE 4 END) ELSE 1 END) END AS tm,		            
						                DMOV_CENCO as cc,
					                    DMOV_GLOSA AS concepto,
						                DMOV_DEBE - DMOV_HABER AS monto,
                                        DMOV_DEBUS - DMOV_HABUS AS montoUs,
						                0 AS iclave,
						                1 AS itm,
						                '' AS orden_compra,
						                NULL AS numpro,
						                '' AS forma_pago,	          	
					                    Convert(datetime,(DMOV_FECHA)) AS fechapol,
						                NULL AS area,
						                NULL AS cuenta_oc,
						                6 AS empresa,
						                GETDATE() as fecha,
                                        RIGHT('00000000000'+ RTRIM(ISNULL(DMOV_ANEXO, '0')), 11) AS proveedorPeru
					                 FROM [003BDCONT2023].dbo.DETMOV03
                                     UNION
	                                 SELECT
                                        2023 AS year,
                                        CAST(04 as int) AS mes,										  
						                SUBDIAR_CODIGO as tp,
						                CAST(DMOV_C_COMPR as int) as poliza,
						                CAST(DMOV_SECUE as int) as linea,	
						                DMOV_CUENT as cta,
						                0 AS sscta,
						                0 AS sscta,
	                                    0 AS digito,
                                        CASE WHEN DMOV_DEBE <> 0 THEN (CASE WHEN DMOV_DEBE > 0 THEN 1 ELSE 3 END) ELSE (CASE WHEN DMOV_HABER <> 0 THEN (CASE WHEN DMOV_HABER > 0 THEN 2 ELSE 4 END) ELSE 1 END) END AS tm,		            
						                DMOV_CENCO as cc,
					                    DMOV_GLOSA AS concepto,
						                DMOV_DEBE - DMOV_HABER AS monto,
                                        DMOV_DEBUS - DMOV_HABUS AS montoUs,
						                0 AS iclave,
						                1 AS itm,
						                '' AS orden_compra,
						                NULL AS numpro,
						                '' AS forma_pago,	          	
					                    Convert(datetime,(DMOV_FECHA)) AS fechapol,
						                NULL AS area,
						                NULL AS cuenta_oc,
						                6 AS empresa,
						                GETDATE() as fecha,
                                        RIGHT('00000000000'+ RTRIM(ISNULL(DMOV_ANEXO, '0')), 11) AS proveedorPeru
					                 FROM [003BDCONT2023].dbo.DETMOV04
                                     UNION
	                                 SELECT
                                        2023 AS year,
                                        CAST(05 as int) AS mes,										  
						                SUBDIAR_CODIGO as tp,
						                CAST(DMOV_C_COMPR as int) as poliza,
						                CAST(DMOV_SECUE as int) as linea,	
						                DMOV_CUENT as cta,
						                0 AS sscta,
						                0 AS sscta,
	                                    0 AS digito,
                                        CASE WHEN DMOV_DEBE <> 0 THEN (CASE WHEN DMOV_DEBE > 0 THEN 1 ELSE 3 END) ELSE (CASE WHEN DMOV_HABER <> 0 THEN (CASE WHEN DMOV_HABER > 0 THEN 2 ELSE 4 END) ELSE 1 END) END AS tm,		            
						                DMOV_CENCO as cc,
					                    DMOV_GLOSA AS concepto,
						                DMOV_DEBE - DMOV_HABER AS monto,
                                        DMOV_DEBUS - DMOV_HABUS AS montoUs,
						                0 AS iclave,
						                1 AS itm,
						                '' AS orden_compra,
						                NULL AS numpro,
						                '' AS forma_pago,	          	
					                    Convert(datetime,(DMOV_FECHA)) AS fechapol,
						                NULL AS area,
						                NULL AS cuenta_oc,
						                6 AS empresa,
						                GETDATE() as fecha,
                                        RIGHT('00000000000'+ RTRIM(ISNULL(DMOV_ANEXO, '0')), 11) AS proveedorPeru
					                 FROM [003BDCONT2023].dbo.DETMOV05
                                     UNION
	                                 SELECT
                                        2023 AS year,
                                        CAST(06 as int) AS mes,										  
						                SUBDIAR_CODIGO as tp,
						                CAST(DMOV_C_COMPR as int) as poliza,
						                CAST(DMOV_SECUE as int) as linea,	
						                DMOV_CUENT as cta,
						                0 AS sscta,
						                0 AS sscta,
	                                    0 AS digito,
                                        CASE WHEN DMOV_DEBE <> 0 THEN (CASE WHEN DMOV_DEBE > 0 THEN 1 ELSE 3 END) ELSE (CASE WHEN DMOV_HABER <> 0 THEN (CASE WHEN DMOV_HABER > 0 THEN 2 ELSE 4 END) ELSE 1 END) END AS tm,		            
						                DMOV_CENCO as cc,
					                    DMOV_GLOSA AS concepto,
						                DMOV_DEBE - DMOV_HABER AS monto,
                                        DMOV_DEBUS - DMOV_HABUS AS montoUs,
						                0 AS iclave,
						                1 AS itm,
						                '' AS orden_compra,
						                NULL AS numpro,
						                '' AS forma_pago,	          	
					                    Convert(datetime,(DMOV_FECHA)) AS fechapol,
						                NULL AS area,
						                NULL AS cuenta_oc,
						                6 AS empresa,
						                GETDATE() as fecha,
                                        RIGHT('00000000000'+ RTRIM(ISNULL(DMOV_ANEXO, '0')), 11) AS proveedorPeru
					                 FROM [003BDCONT2023].dbo.DETMOV06
                                     UNION
	                                 SELECT
                                        2023 AS year,
                                        CAST(07 as int) AS mes,										  
						                SUBDIAR_CODIGO as tp,
						                CAST(DMOV_C_COMPR as int) as poliza,
						                CAST(DMOV_SECUE as int) as linea,	
						                DMOV_CUENT as cta,
						                0 AS sscta,
						                0 AS sscta,
	                                    0 AS digito,
                                        CASE WHEN DMOV_DEBE <> 0 THEN (CASE WHEN DMOV_DEBE > 0 THEN 1 ELSE 3 END) ELSE (CASE WHEN DMOV_HABER <> 0 THEN (CASE WHEN DMOV_HABER > 0 THEN 2 ELSE 4 END) ELSE 1 END) END AS tm,		            
						                DMOV_CENCO as cc,
					                    DMOV_GLOSA AS concepto,
						                DMOV_DEBE - DMOV_HABER AS monto,
                                        DMOV_DEBUS - DMOV_HABUS AS montoUs,
						                0 AS iclave,
						                1 AS itm,
						                '' AS orden_compra,
						                NULL AS numpro,
						                '' AS forma_pago,	          	
					                    Convert(datetime,(DMOV_FECHA)) AS fechapol,
						                NULL AS area,
						                NULL AS cuenta_oc,
						                6 AS empresa,
						                GETDATE() as fecha,
                                        RIGHT('00000000000'+ RTRIM(ISNULL(DMOV_ANEXO, '0')), 11) AS proveedorPeru
					                 FROM [003BDCONT2023].dbo.DETMOV07
                                     UNION
	                                 SELECT
                                        2023 AS year,
                                        CAST(08 as int) AS mes,										  
						                SUBDIAR_CODIGO as tp,
						                CAST(DMOV_C_COMPR as int) as poliza,
						                CAST(DMOV_SECUE as int) as linea,	
						                DMOV_CUENT as cta,
						                0 AS sscta,
						                0 AS sscta,
	                                    0 AS digito,
                                       CASE WHEN DMOV_DEBE <> 0 THEN (CASE WHEN DMOV_DEBE > 0 THEN 1 ELSE 3 END) ELSE (CASE WHEN DMOV_HABER <> 0 THEN (CASE WHEN DMOV_HABER > 0 THEN 2 ELSE 4 END) ELSE 1 END) END AS tm,		            
						                DMOV_CENCO as cc,
					                    DMOV_GLOSA AS concepto,
						                DMOV_DEBE - DMOV_HABER AS monto,
                                        DMOV_DEBUS - DMOV_HABUS AS montoUs,
						                0 AS iclave,
						                1 AS itm,
						                '' AS orden_compra,
						                NULL AS numpro,
						                '' AS forma_pago,	          	
					                    Convert(datetime,(DMOV_FECHA)) AS fechapol,
						                NULL AS area,
						                NULL AS cuenta_oc,
						                6 AS empresa,
						                GETDATE() as fecha,
                                        RIGHT('00000000000'+ RTRIM(ISNULL(DMOV_ANEXO, '0')), 11) AS proveedorPeru
					                 FROM [003BDCONT2023].dbo.DETMOV08
                                     UNION
	                                 SELECT
                                        2023 AS year,
                                        CAST(09 as int) AS mes,										  
						                SUBDIAR_CODIGO as tp,
						                CAST(DMOV_C_COMPR as int) as poliza,
						                CAST(DMOV_SECUE as int) as linea,	
						                DMOV_CUENT as cta,
						                0 AS sscta,
						                0 AS sscta,
	                                    0 AS digito,
                                        CASE WHEN DMOV_DEBE <> 0 THEN (CASE WHEN DMOV_DEBE > 0 THEN 1 ELSE 3 END) ELSE (CASE WHEN DMOV_HABER <> 0 THEN (CASE WHEN DMOV_HABER > 0 THEN 2 ELSE 4 END) ELSE 1 END) END AS tm,		            
						                DMOV_CENCO as cc,
					                    DMOV_GLOSA AS concepto,
						                DMOV_DEBE - DMOV_HABER AS monto,
                                        DMOV_DEBUS - DMOV_HABUS AS montoUs,
						                0 AS iclave,
						                1 AS itm,
						                '' AS orden_compra,
						                NULL AS numpro,
						                '' AS forma_pago,	          	
					                    Convert(datetime,(DMOV_FECHA)) AS fechapol,
						                NULL AS area,
						                NULL AS cuenta_oc,
						                6 AS empresa,
						                GETDATE() as fecha,
                                        RIGHT('00000000000'+ RTRIM(ISNULL(DMOV_ANEXO, '0')), 11) AS proveedorPeru
					                 FROM [003BDCONT2023].dbo.DETMOV09
                                     UNION
	                                 SELECT
                                        2023 AS year,
                                        CAST(10 as int) AS mes,										  
						                SUBDIAR_CODIGO as tp,
						                CAST(DMOV_C_COMPR as int) as poliza,
						                CAST(DMOV_SECUE as int) as linea,	
						                DMOV_CUENT as cta,
						                0 AS sscta,
						                0 AS sscta,
	                                    0 AS digito,
                                        CASE WHEN DMOV_DEBE <> 0 THEN (CASE WHEN DMOV_DEBE > 0 THEN 1 ELSE 3 END) ELSE (CASE WHEN DMOV_HABER <> 0 THEN (CASE WHEN DMOV_HABER > 0 THEN 2 ELSE 4 END) ELSE 1 END) END AS tm,		            
						                DMOV_CENCO as cc,
					                    DMOV_GLOSA AS concepto,
						                DMOV_DEBE - DMOV_HABER AS monto,
                                        DMOV_DEBUS - DMOV_HABUS AS montoUs,
						                0 AS iclave,
						                1 AS itm,
						                '' AS orden_compra,
						                NULL AS numpro,
						                '' AS forma_pago,	          	
					                    Convert(datetime,(DMOV_FECHA)) AS fechapol,
						                NULL AS area,
						                NULL AS cuenta_oc,
						                6 AS empresa,
						                GETDATE() as fecha,
                                        RIGHT('00000000000'+ RTRIM(ISNULL(DMOV_ANEXO, '0')), 11) AS proveedorPeru
					                 FROM [003BDCONT2023].dbo.DETMOV10
                                     UNION
	                                 SELECT
                                        2023 AS year,
                                        CAST(11 as int) AS mes,										  
						                SUBDIAR_CODIGO as tp,
						                CAST(DMOV_C_COMPR as int) as poliza,
						                CAST(DMOV_SECUE as int) as linea,	
						                DMOV_CUENT as cta,
						                0 AS sscta,
						                0 AS sscta,
	                                    0 AS digito,
                                        CASE WHEN DMOV_DEBE <> 0 THEN (CASE WHEN DMOV_DEBE > 0 THEN 1 ELSE 3 END) ELSE (CASE WHEN DMOV_HABER <> 0 THEN (CASE WHEN DMOV_HABER > 0 THEN 2 ELSE 4 END) ELSE 1 END) END AS tm,		            
						                DMOV_CENCO as cc,
					                    DMOV_GLOSA AS concepto,
						                DMOV_DEBE - DMOV_HABER AS monto,
                                        DMOV_DEBUS - DMOV_HABUS AS montoUs,
						                0 AS iclave,
						                1 AS itm,
						                '' AS orden_compra,
						                NULL AS numpro,
						                '' AS forma_pago,	          	
					                    Convert(datetime,(DMOV_FECHA)) AS fechapol,
						                NULL AS area,
						                NULL AS cuenta_oc,
						                6 AS empresa,
						                GETDATE() as fecha,
                                        RIGHT('00000000000'+ RTRIM(ISNULL(DMOV_ANEXO, '0')), 11) AS proveedorPeru
					                 FROM [003BDCONT2023].dbo.DETMOV11
                                     UNION
	                                 SELECT
                                        2023 AS year,
                                        CAST(12 as int) AS mes,										  
						                SUBDIAR_CODIGO as tp,
						                CAST(DMOV_C_COMPR as int) as poliza,
						                CAST(DMOV_SECUE as int) as linea,	
						                DMOV_CUENT as cta,
						                0 AS sscta,
						                0 AS sscta,
	                                    0 AS digito,
                                        CASE WHEN DMOV_DEBE <> 0 THEN (CASE WHEN DMOV_DEBE > 0 THEN 1 ELSE 3 END) ELSE (CASE WHEN DMOV_HABER <> 0 THEN (CASE WHEN DMOV_HABER > 0 THEN 2 ELSE 4 END) ELSE 1 END) END AS tm,		            
						                DMOV_CENCO as cc,
					                    DMOV_GLOSA AS concepto,
						                DMOV_DEBE - DMOV_HABER AS monto,
                                        DMOV_DEBUS - DMOV_HABUS AS montoUs,
						                0 AS iclave,
						                1 AS itm,
						                '' AS orden_compra,
						                NULL AS numpro,
						                '' AS forma_pago,	          	
					                    Convert(datetime,(DMOV_FECHA)) AS fechapol,
						                NULL AS area,
						                NULL AS cuenta_oc,
						                6 AS empresa,
						                GETDATE() as fecha,
                                        RIGHT('00000000000'+ RTRIM(ISNULL(DMOV_ANEXO, '0')), 11) AS proveedorPeru
					                 FROM [003BDCONT2023].dbo.DETMOV12 WHERE ";


            lst.ForEach(poliza =>
            {

                //consultaGeneral += String.Format(@"(CB_C_MES = {0} AND CB_C_CODIG = {1} AND CB_C_SECUE = {2}) OR ", poliza.mes.ToString().PadLeft(3, '0'), poliza.tp, poliza.poliza.ToString().PadLeft(4, '0'));
                consultaGeneral += String.Format(@"(01 = {0} AND SUBDIAR_CODIGO = {1} AND DMOV_C_COMPR = {2}) OR ", poliza.mes.ToString().PadLeft(3, '0'), poliza.tp, poliza.poliza.ToString().PadLeft(4, '0'));
                //modResultado = conexion.Query<PolizasDTO>(sql, lstParametros, null, true, 300, commandType: CommandType.Text).Where(pol => !polizasCol.Any(col => col.PalYear == pol.year && col.PalMes == pol.mes && col.PalTP == pol.tp && col.PalPoliza == pol.poliza)).ToList();
            });
            consultaGeneral = consultaGeneral.Substring(0, consultaGeneral.Length - 3);
            using (var conexion = new SqlConnection(ConextSigoDapper.conexionStarsoftBancos()))
            {
                conexion.Open();
                lstMovPol = conexion.Query<MovpolDTO>(consultaGeneral, lstParametros, null, true, 300, commandType: CommandType.Text).ToList();

                conexion.Close();
            }

            var conexionEnkontrol = _contextEnkontrol.BaseDatosProductivoDesdeEmpresa(EmpresaEnum.Construplan);
            var odbc = new OdbcConsultaDTO();
            odbc.consulta = @"SELECT * FROM catcta WHERE cta = 2105 AND scta = 500";
            var cuentasProveedoresEnk = _contextEnkontrol.Select<catctaDTO>(conexionEnkontrol, odbc);

            var cuentasProveedoresSIGOPLAN = _context.tblC_CP_CuentasProveedores.Where(x => x.esActivo).ToList();

            lst.ForEach(poliza =>
            {
                var estatus = string.Empty;
                var lstValida = new List<bool>();
                var puedeSincCta = true;
                var puedeSincItm = true;
                var puedeSincObra = true;
                var puedeSincProveedorPeru = true;
                var movpol = lstMovPol.Where(mov => mov.year == poliza.year && mov.mes == poliza.mes && mov.tp == poliza.tp && mov.poliza == poliza.poliza).ToList();

                var mensaje2 = new List<string>();
                for (int i = 0; i < movpol.Count; i++)
                {
                    string CtasFaltantes = "";
                    var mov = movpol[i];
                    var relCuenta = cuentas.FirstOrDefault(cta => cta.palCta == mov.cta && cta.palScta == mov.scta && cta.palSscta == mov.sscta) ?? new tblC_Cta_RelCuentas();
                    var cuentaSistema = cuentasSistema.FirstOrDefault(cta => cta.cuenta == relCuenta.secCta && CuentasSistemasValidos.Contains(cta.sistema)) ?? new CtaintDTO();
                    var tipoMov = iTiposMov.FirstOrDefault(itm => itm.PaliTm == mov.itm && cuentaSistema.cuenta > 0) ?? new tblC_TM_Relitm();
                    var obra = obras.FirstOrDefault(cc => cc.ccSecundario == mov.cc) ?? new tblC_Cta_RelCC();


                    var movPuedeSincCta = relCuenta.id > 0;
                    ////Cuentas secundarias validas y relacionadas o movimientos principales sin cuentas validas
                    //var movPuedeSincItm = cuentaSistema.cuenta > 0 && tipoMov.Id > 0 ? true : cuentaSistema.cuenta == 0;
                    //var movPuedeSincObra = mov.cc == string.Empty || obra.Id > 0;
                    var movPuedeSincItm = true;
                    var movPuedeSincObra = true;
                    
                    var _cuentaProveedoresSIGOPLAN = cuentasProveedoresSIGOPLAN.FirstOrDefault(x => x.cta == movpol[i].cta);
                    if (_cuentaProveedoresSIGOPLAN != null) 
                    {
                        var _cuentaProveedoresEnk = cuentasProveedoresEnk.FirstOrDefault(x => x.descripcion.Contains(mov.proveedorPeru));
                        puedeSincProveedorPeru = _cuentaProveedoresEnk != null;
                    }

                    if (!movPuedeSincCta && !movPuedeSincItm && !movPuedeSincObra)
                    {

                        puedeSincCta = false;
                        puedeSincItm = false;
                        puedeSincObra = false;
                        continue;
                    }
                    else
                    {
                        var mensaje = new List<string>();
                        if (!movPuedeSincCta)
                        {
                            CtasFaltantes = "[" + mov.cta + "-" + mov.scta + "-" + mov.sscta + "]";
                            if (!mensaje2.Exists(r => r == CtasFaltantes))
                                mensaje2.Add(CtasFaltantes);
                            puedeSincCta = false;
                        }
                        if (!movPuedeSincObra)
                        {
                            puedeSincObra = false;
                        }
                        if (!movPuedeSincItm)
                        {
                            puedeSincItm = false;
                        } if (!puedeSincProveedorPeru)
                        {
                            mensaje2.Add("Proveedor " + mov.proveedorPeru);
                        }                        
                        estatus = string.Format("Faltan {0} por relacionar", mensaje.ToLine(","));
                    }
                    
                }

                if (puedeSincCta && puedeSincItm && puedeSincObra && puedeSincProveedorPeru)
                {
                    estatus = "OK";
                }
                else if (!puedeSincCta && !puedeSincItm && !puedeSincObra && !puedeSincProveedorPeru)
                {
                    estatus = "No se puede convertir";
                }
                else
                {
                    var mensaje = new List<string>();
                    if (!puedeSincCta)
                    {
                        mensaje.Add("Cuentas ");
                        mensaje.AddRange(mensaje2);
                    }
                    if (!puedeSincObra)
                    {
                        mensaje.Add("Centro Costos ");
                    }
                    if (!puedeSincItm)
                    {
                        mensaje.Add("Tipos Movimientos ");
                    }
                    if (!puedeSincProveedorPeru)
                    {
                        mensaje.Add("Proveedores ");
                    }
                    estatus = string.Format("Faltan {0} por relacionar", mensaje.ToLine(",").Replace("'"[0], ' '));
                }

                if (vSesiones.sesionEmpresaActual == 6)
                {
                    polizas.Add(new ConversionPolizaDTO
                    {
                        PalEmpresa = EmpresaEnum.Peru,
                        PalYear = poliza.year,
                        PalMes = poliza.mes,
                        PalTP = poliza.tp,
                        PalPoliza = poliza.poliza,
                        PalCargo = poliza.cargos,
                        PalCargoUs = poliza.cargosUs,
                        PalAbono = poliza.abonos,
                        PalAbonoUs = poliza.abonosUs,
                        PalFechapol = poliza.fechapol,
                        fechaPoliza = poliza.fechapol.ToShortDateString(),
                        SecEmpresa = EmpresaEnum.Construplan,
                        SecCargo = convertirPesosColAPesosMX(poliza.cargos),
                        SecAbono = convertirPesosColAPesosMX(poliza.abonos),
                        Estatus = estatus
                    });
                }
                else
                {
                    polizas.Add(new ConversionPolizaDTO
                    {
                        PalEmpresa = EmpresaEnum.Peru,
                        PalYear = poliza.year,
                        PalMes = poliza.mes,
                        PalTP = poliza.tp,
                        PalPoliza = poliza.poliza,
                        PalCargo = poliza.cargos,
                        PalAbono = poliza.abonos,
                        PalFechapol = poliza.fechapol,
                        fechaPoliza = poliza.fechapol.ToShortDateString(),
                        SecEmpresa = EmpresaEnum.Construplan,
                        SecCargo = convertirPesosColAPesosMX(poliza.cargos),
                        SecAbono = convertirPesosColAPesosMX(poliza.abonos),
                        Estatus = estatus
                    });
                }

            });
            return polizas;
        }

        private int UltimoNumPolizaCplan(string tp, DateTime fecha)
        {
            try
            {

                string consulta = string.Format(@"SELECT top 1 (poliza + 1) As poliza FROM sc_polizas
                                                WHERE year = {0}
                                                    AND mes = {1}
                                                    AND tp = '{2}'
                                                    ORDER BY poliza DESC", fecha.Year, fecha.Month, tp);
                var lstEkP = _contextEnkontrol.Select<PolizasDTO>(EnkontrolEnum.CplanProd, consulta);
                //var lstEkP = _contextEnkontrol.Select<PolizasDTO>(EnkontrolEnum.PruebaCplanProd, consulta);
                return lstEkP.FirstOrDefault().poliza;


            }
            catch (Exception ex) { return 1; }
        }
        public Dictionary<string, object> ObtenerPolizaEnkontrol(PolizasDTO poliza, EmpresaEnum empresa)
        {
            try
            {
                var polizaEnk = PolizaEnkontrol(poliza, empresa);
                resultado.Add("poliza", polizaEnk);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                LogError(0, 0, nombreControlador, "ObtenerPolizaEnkontrol", e, AccionEnum.CONSULTA, 0, null);
            }
            return resultado;
        }
    }
}
