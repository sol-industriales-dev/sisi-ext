using Core.DAO.Maquinaria.Reporte;
using Core.DTO.Maquinaria.Reporte.Rentabilidad;
using Core.DTO.Principal.Generales;
using Core.DTO.Utils.Data;
using Core.Enum.Multiempresa;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Maquinaria.Captura;
using Core.DTO;
using Core.DTO.Maquinaria.Reporte.Analisis;
using Core.DTO.Maquinaria.Reporte.Kubrix;
using Core.DTO.Maquinaria.Rentabilidad;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Maquinaria.Rentabilidad;
using System.Data;
using System.Data.SqlClient;
using Core.Entity.Maquinaria;
using Core.Entity.Principal.Multiempresa;
using Newtonsoft.Json;
using Core.Enum.Principal.Bitacoras;
using System.Text.RegularExpressions;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System.IO;
using OfficeOpenXml.Table;
using Core.DTO.Maquinaria.Reporte.ActivoFijo.Generales.Enkontrol;
using Dapper;
using Core.DTO.Utils;
using Core.Enum.CuentasPorCobrar;
using System.Drawing;
using System.Web;

namespace Data.DAO.Maquinaria.Reporte
{
    public class RentabilidadDAO : GenericDAO<tblM_KBCatCuenta>, IRentabilidadDAO
    {
        #region variables y constructor
        private readonly Dictionary<string, object> resultado = new Dictionary<string, object>();
        private string NombreControlador = "RentabilidadController";

        private const string nombreControlador = "RentabilidadController";
        public MainContext _context = new MainContext();
        //public ContextSigoDapper _kcon = new ContextSigoDapper();

        #endregion
        public RentabilidadDAO()
        {
            _context = new MainContext();
        }

        public List<ComboDTO> getListaCC()
        {
            try
            {
                var lstSIGOPLAN = _context.tblP_CC.Select(x => new ComboDTO
                {
                    Value = x.areaCuenta,
                    Text = x.areaCuenta + " " + x.descripcion,
                    Prefijo = x.descripcion
                }).ToList();
                var obj = new OdbcConsultaDTO()
                {
                    consulta = string.Format(@"SELECT (CAST(area as varchar(10)) + '-' + CAST(cuenta as varchar(10))) as Value, UPPER(CAST(area as varchar(10)) + '-' + CAST(cuenta as varchar(10)) + ' ' + min(descripcion)) as Text, min(descripcion) AS Prefijo FROM si_area_cuenta group by area, cuenta order by area, cuenta")
                };
                var lstEkP = _contextEnkontrol.Select<ComboDTO>(EnkontrolEnum.ArrenProd, obj);
                var lstValues = lstSIGOPLAN.Select(x => x.Value).ToList();
                var lst = lstEkP.Where(x => !lstValues.Contains(x.Value));
                lstSIGOPLAN.AddRange(lst);
                int parse = 0;
                return lstSIGOPLAN
                    .OrderBy(x => Int32.TryParse(x.Value.Split('-')[0], out parse) ? Int32.Parse(x.Value.Split('-')[0]) : Int32.MaxValue)
                    .ThenBy(x => x.Value.Split('-').Count() > 1 ?
                        (Int32.TryParse(x.Value.Split('-')[1], out parse) ? Int32.Parse(x.Value.Split('-')[1]) : Int32.MaxValue) :
                        (Int32.TryParse(x.Value, out parse) ? Int32.Parse(x.Value) : Int32.MaxValue)).ToList();
            }
            catch (Exception) { return new List<ComboDTO>(); }
        }

        private List<ComboDTO> getListaCCCPlan()
        {
            try
            {
                var lstSIGOPLAN = _context.tblP_CC.Select(x => new { x.cc, x.areaCuenta }).ToList();
                var obj = new OdbcConsultaDTO()
                {
                    consulta = string.Format(@"SELECT cc as Value, UPPER(cc) + ' ' + descripcion as Text FROM cc order by cc")
                };
                var lstEkP = _contextEnkontrol.Select<ComboDTO>(EnkontrolEnum.CplanProd, obj);
                foreach (var item in lstEkP)
                {
                    var auxLstSIGOPLAN = lstSIGOPLAN.FirstOrDefault(x => x.cc == item.Value);
                    if (auxLstSIGOPLAN != null) { item.Prefijo = auxLstSIGOPLAN.areaCuenta; }
                }
                var lst = lstEkP.ToList();
                return lst.OrderBy(x => x.Value).ToList();
            }
            catch (Exception) { return new List<ComboDTO>(); }
        }


        public List<ComboDTO> getListaCCByUsuario(int usuarioID, int tipo)
        {
            try
            {
                var empresa = vSesiones.sesionEmpresaActual;
                List<ComboDTO> lstSIGOPLAN = new List<ComboDTO>();
                List<ComboDTO> lstEkP = new List<ComboDTO>();
                List<string> lstValues = new List<string>();

                if (empresa == 1)
                {
                    lstSIGOPLAN = _context.tblP_CC.Select(x => new ComboDTO
                    {
                        Value = (x.areaCuenta == "0" || x.areaCuenta == "0-0") ? x.cc : x.areaCuenta,
                        Text = x.cc + " " + x.descripcion,
                        Prefijo = x.cc,
                        TextoOpcional = x.estatus.ToString()
                    }).ToList();

                    var obj = new OdbcConsultaDTO()
                    {
                        consulta = string.Format(@"SELECT cc as Value, cc + '-' + descripcion as Text, cc as Prefijo, CASE WHEN st_ppto = 'T' THEN 'false' ELSE 'true' END as TextoOpcional FROM cc order by cc")
                    };
                    lstEkP = _contextEnkontrol.Select<ComboDTO>(EnkontrolEnum.CplanProd, obj);
                    lstValues = lstSIGOPLAN.Select(x => x.Prefijo).ToList();
                }
                else
                {
                    lstSIGOPLAN = _context.tblP_CC.Select(x => new ComboDTO
                    {
                        Value = (x.areaCuenta == "0" || x.areaCuenta == "0-0") ? x.cc : x.areaCuenta,
                        Text = x.areaCuenta + " " + x.descripcion,
                        Prefijo = x.cc
                    }).ToList();

                    var obj = new OdbcConsultaDTO()
                    {
                        consulta = string.Format(@"SELECT (CAST(area as varchar(10)) + '-' + CAST(cuenta as varchar(10))) as Value, UPPER(CAST(area as varchar(10)) + '-' + CAST(cuenta as varchar(10)) + ' ' + min(descripcion)) as Text FROM si_area_cuenta group by area, cuenta order by area, cuenta")
                    };
                    lstEkP = _contextEnkontrol.Select<ComboDTO>(EnkontrolEnum.ArrenProd, obj);
                    lstValues = lstSIGOPLAN.Select(x => x.Value).ToList();
                }

                var lst = lstEkP.Where(x => !lstValues.Contains(x.Value)).ToList();
                lstSIGOPLAN.AddRange(lst);
                var responsable = checkResponsable(-1, usuarioID);
                var centrosGuardados = _context.tblM_KBCatCC.Select(x => x.areaCuenta).ToList();
                var remover = new List<ComboDTO>();
                foreach (var item in lstSIGOPLAN)
                {
                    if (!centrosGuardados.Contains(item.Value))
                    {
                        if (tipo == 0)
                        {
                            item.Prefijo = "0";
                        }
                        else
                        {
                            remover.Add(item);
                        }
                    }
                    else
                    {
                        if (tipo == 0)
                        {
                            item.Prefijo = "1";
                        }
                        else
                        {
                            var ccCPlan = _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == item.Value);
                            if (ccCPlan != null) item.Prefijo = ccCPlan.cc;
                            else item.Prefijo = "N/A";
                        }
                    }
                }
                foreach (var item in remover) { lstSIGOPLAN.Remove(item); }
                if (responsable)
                {
                    var responsableACs = getACResponsable(usuarioID);
                    lstSIGOPLAN = lstSIGOPLAN.Where(x => responsableACs.Contains(x.Value)).ToList();
                }
                int parse = 0;
                if (empresa == 1)
                {
                    return lstSIGOPLAN
                    .OrderByDescending(x => x.TextoOpcional).ThenBy(x => x.Prefijo).ToList();
                }
                else
                {
                    return lstSIGOPLAN
                        .OrderBy(x => Int32.TryParse(x.Value.Split('-')[0], out parse) ? Int32.Parse(x.Value.Split('-')[0]) : Int32.MaxValue)
                        .ThenBy(x => x.Value.Split('-').Count() > 1 ?
                            (Int32.TryParse(x.Value.Split('-')[1], out parse) ? Int32.Parse(x.Value.Split('-')[1]) : Int32.MaxValue) :
                            (Int32.TryParse(x.Value, out parse) ? Int32.Parse(x.Value) : Int32.MaxValue)).ToList();
                }

            }
            catch (Exception) { return new List<ComboDTO>(); }
        }

        public List<RentabilidadDTO> getLstRentabilidad(BusqRentabilidadDTO busq)
        {
            try
            {
                busq = verificaBusqueda(busq);
                var odbc = new OdbcConsultaDTO()
                {
                    consulta = queryRentabilidadFinal(busq),
                    parametros = parametrosRentabilidad(busq)
                };
                var lst = _contextEnkontrol.Select<RentabilidadDTO>(EnkontrolAmbienteEnum.Prod, odbc);
                return lst;
            }
            catch (Exception o_O)
            {
                return new List<RentabilidadDTO>();
            }
        }

        public List<RentabilidadDTO> getLstRentabilidadDetalle(BusqRentabilidadDTO busq)
        {
            try
            {
                busq = verificaBusqueda(busq);
                var odbc = new OdbcConsultaDTO()
                {
                    consulta = queryRentabilidadDetalle(busq),
                    parametros = parametrosRentabilidad(busq)
                };
                var lst = _contextEnkontrol.Select<RentabilidadDTO>(EnkontrolAmbienteEnum.Prod, odbc);
                return lst;
            }
            catch (Exception o_O)
            {
                return new List<RentabilidadDTO>();
            }
        }

        string queryRentabilidadFinal(BusqRentabilidadDTO busq)
        {
            string query = "";
            switch (busq.tipoReporte)
            {
                case Core.Enum.Maquinaria.Reportes.Rentabilidad.TipoReporteEnum.Ingresos:
                    query = string.Format(
                        @"SELECT noEco, cta, tipoInsumo, tipoInsumo_Desc, tipo_mov, sum(importe) as importe
                            FROM 
                            (
                                SELECT d.descripcion as noEco, a.cta as cta, a.scta as tipoInsumo, c.descripcion as tipoInsumo_Desc, a.tm as tipo_mov, SUM(a.monto) as importe, e.fechapol as fecha
                                FROM (SELECT cta, poliza, concepto, (CAST(cta as varchar(10)) + '-' + CAST(scta as varchar(10))) as scta, tm, monto, tp, year, mes, (CAST(scta as varchar(10))+ '-' + CAST(sscta as varchar(10))) as sscta, cc from sc_movpol WHERE cta = 4000 and scta != '4000-3' and tm in (2, 4)) a 
                                LEFT JOIN (SELECT poliza, tp, year, mes, fechapol FROM sc_polizas) e
                                ON a.poliza = e.poliza and a.tp = e.tp and a.year = e.year and a.mes = e.mes
                                LEFT JOIN (SELECT (CAST(cta as varchar(10)) + '-' + CAST(scta as varchar(10))) as scta, descripcion from catcta WHERE sscta = 0 and cta = 4000 and scta != '4000-3') c
                                ON a.scta = c.scta
                                LEFT JOIN (SELECT cc, descripcion FROM cc) as d
                                ON a.cc = d.cc AND d.descripcion in {0}
                                WHERE fecha BETWEEN ? AND ?
                                GROUP BY d.descripcion, a.cta, a.poliza, a.concepto, a.scta, c.descripcion, a.tm, e.fechapol
                            ) x
                            WHERE noEco LIKE '%-%'    
                            GROUP BY noEco, x.cta, x.tipoInsumo, x.tipoInsumo_Desc, x.tipo_mov"
                        , busq.lstMaquina.ToParamInValue()
                    );
                    break;
                case Core.Enum.Maquinaria.Reportes.Rentabilidad.TipoReporteEnum.Rentabilidad:
                    query = string.Format(
                        @"SELECT noEco, cta, tipo_mov, sum(importe) as importe
                            FROM 
                            (
                                SELECT d.descripcion as noEco, a.cta as cta, a.poliza AS insumo, a.concepto as insumo_Desc, a.tm as tipo_mov, SUM(a.monto) as importe, e.fechapol as fecha
                                FROM (SELECT cta, poliza, concepto, (CAST(cta as varchar(10)) + '-' + CAST(scta as varchar(10))) as scta, tm, monto, tp, year, mes, (CAST(scta as varchar(10))+ '-' + CAST(sscta as varchar(10))) as sscta, cc from sc_movpol WHERE ((cta = 5000 and tm in (1, 3)) or (cta = 4000 and tm in (2, 4)) or (scta = '5900-3' and tm in (1, 3))) and scta != '4000-3') a 
                                LEFT JOIN (SELECT poliza, tp, year, mes, fechapol FROM sc_polizas) e
                                ON a.poliza = e.poliza and a.tp = e.tp and a.year = e.year and a.mes = e.mes
                                LEFT JOIN (SELECT cc, descripcion FROM cc) as d
                                ON a.cc = d.cc AND d.descripcion in {0}
                                WHERE fecha BETWEEN ? AND ?
                                GROUP BY d.descripcion, a.cta, a.poliza, a.concepto, a.tm, e.fechapol
                            ) x
                            WHERE noEco LIKE '%-%'    
                            GROUP BY noEco, x.cta, x.tipo_mov"
                        , busq.lstMaquina.ToParamInValue()
                    );
                    break;
                case Core.Enum.Maquinaria.Reportes.Rentabilidad.TipoReporteEnum.Costos:
                case Core.Enum.Maquinaria.Reportes.Rentabilidad.TipoReporteEnum.CostoHora:
                    query = string.Format(
                        @"SELECT noEco, cta, tipoInsumo, tipoInsumo_Desc, tipo_mov, sum(importe) as importe
                            FROM 
                            (
                                SELECT d.descripcion as noEco, a.cta as cta, a.scta as tipoInsumo, c.descripcion as tipoInsumo_Desc, a.tm as tipo_mov, SUM(a.monto) as importe, e.fechapol as fecha
                                FROM (SELECT cta, poliza, concepto, (CAST(cta as varchar(10)) + '-' + CAST(scta as varchar(10))) as scta, tm, monto, tp, year, mes, (CAST(scta as varchar(10))+ '-' + CAST(sscta as varchar(10))) as sscta, cc from sc_movpol WHERE (cta = 5000 and tm in (1, 3)) or (scta = '5900-3' and tm in (1, 3))) a 
                                LEFT JOIN (SELECT poliza, tp, year, mes, fechapol FROM sc_polizas) e
                                ON a.poliza = e.poliza and a.tp = e.tp and a.year = e.year and a.mes = e.mes
                                LEFT JOIN (SELECT (CAST(cta as varchar(10)) + '-' + CAST(scta as varchar(10))) as scta, descripcion from catcta WHERE sscta = 0 and cta = 5000 or scta = '5900-3') c
                                ON a.scta = c.scta
                                LEFT JOIN (SELECT cc, descripcion FROM cc) as d
                                ON a.cc = d.cc AND d.descripcion in {0}
                                WHERE fecha BETWEEN ? AND ?
                                GROUP BY d.descripcion, a.cta, a.poliza, a.concepto, a.scta, c.descripcion, a.tm, e.fechapol
                            ) x
                            WHERE noEco LIKE '%-%'    
                            GROUP BY noEco, x.cta, x.tipoInsumo, x.tipoInsumo_Desc, x.tipo_mov"
                        , busq.lstMaquina.ToParamInValue()
                    );
                    break;
                default:
                    break;
            }
            return query;
        }
        string queryRentabilidadDetalle(BusqRentabilidadDTO busq)
        {
            string query = "";
            if (busq.tipoReporte == Core.Enum.Maquinaria.Reportes.Rentabilidad.TipoReporteEnum.Rentabilidad)
            {
                query = string.Format(
                @"SELECT noEco, cta, tipoInsumo, tipoInsumo_Desc, grupoInsumo, grupoInsumo_Desc, insumo, insumo_Desc, tipo_mov, sum(importe) as importe, fecha
                    FROM 
                    (
                        SELECT d.descripcion as noEco, a.cta as cta, a.poliza AS insumo, a.concepto as insumo_Desc, a.scta as tipoInsumo, a.sscta as grupoInsumo, c.descripcion as tipoInsumo_Desc, b.descripcion as grupoInsumo_Desc, a.tm as tipo_mov, SUM(a.monto) as importe, e.fechapol as fecha
                        FROM (SELECT cta, poliza, concepto, (CAST(cta as varchar(10)) + '-' + CAST(scta as varchar(10))) as scta, tm, monto, tp, year, mes, (CAST(scta as varchar(10))+ '-' + CAST(sscta as varchar(10))) as sscta, cc from sc_movpol WHERE ((cta=5000 and tm in (1, 3)) or (cta=4000 and scta!='4000-3' and tm in (2, 4)) or (scta='5900-3' and tm in (1, 3))) and tm in {0}) a 
                        LEFT JOIN (SELECT poliza, tp, year, mes, fechapol FROM sc_polizas) e
                        ON a.poliza = e.poliza and a.tp = e.tp and a.year = e.year and a.mes = e.mes
                        LEFT JOIN (SELECT (CAST(cta as varchar(10)) + '-' + CAST(scta as varchar(10))) as scta, descripcion from catcta WHERE sscta = 0 and (cta=5000 or (cta=4000 and scta!='4000-3') or scta='5900-3')) c
                        ON a.scta = c.scta
                        LEFT JOIN (SELECT (CAST(cta as varchar(10)) + '-' + CAST(scta as varchar(10))) as scta, (CAST(scta as varchar(10))+ '-' + CAST(sscta as varchar(10))) as sscta, descripcion from catcta WHERE (cta=5000 or (cta=4000 and scta!='4000-3') or scta='5900-3')) b
                        ON a.sscta = b.sscta AND a.scta = b.scta
                        LEFT JOIN (SELECT cc, descripcion FROM cc) as d
                        ON a.cc = d.cc AND d.descripcion in {1}
                        WHERE fecha BETWEEN ? AND ?
                        GROUP BY d.descripcion, a.cta, a.poliza ,a.concepto , a.scta, a.sscta , c.descripcion, b.descripcion, a.tm, e.fechapol
                    ) x
                    WHERE noEco LIKE '%-%'    
                    GROUP BY noEco, x.cta, x.insumo, x.insumo_Desc, x.tipoInsumo, x.grupoInsumo, x.tipoInsumo_Desc, x.grupoInsumo_Desc, x.tipo_mov, x.fecha"
                    , busq.tm.ToParamInValue()
                    , busq.lstMaquina.ToParamInValue()
                );
            }
            else
            {
                string auxTM = busq.cta >= 5000 ? "(1, 3)" : "(2, 4)";
                query = string.Format(
                    @"SELECT noEco, cta, tipoInsumo, tipoInsumo_Desc, grupoInsumo, grupoInsumo_Desc, insumo, insumo_Desc, tipo_mov, sum(importe) as importe, fecha
                        FROM 
                        (
                            SELECT d.descripcion as noEco, a.cta as cta, a.poliza AS insumo, a.concepto as insumo_Desc, a.scta as tipoInsumo, a.sscta as grupoInsumo, c.descripcion as tipoInsumo_Desc, b.descripcion as grupoInsumo_Desc, a.tm as tipo_mov, SUM(a.monto) as importe, e.fechapol as fecha
                            FROM (SELECT cta, poliza, concepto, (CAST(cta as varchar(10)) + '-' + CAST(scta as varchar(10))) as scta, tm, monto, tp, year, mes, (CAST(scta as varchar(10))+ '-' + CAST(sscta as varchar(10))) as sscta, cc from sc_movpol WHERE cta = {0} and tm in {2} and scta = (CAST({0} as varchar(10)) + '-' + CAST({1} as varchar(10)))) a 
                            LEFT JOIN (SELECT poliza, tp, year, mes, fechapol FROM sc_polizas) e
                            ON a.poliza = e.poliza and a.tp = e.tp and a.year = e.year and a.mes = e.mes
                            LEFT JOIN (SELECT (CAST(cta as varchar(10)) + '-' + CAST(scta as varchar(10))) as scta, descripcion from catcta WHERE sscta = 0 and cta = {0} and scta = (CAST({0} as varchar(10)) + '-' + CAST({1} as varchar(10)))) c
                            ON a.scta = c.scta
                            LEFT JOIN (SELECT (CAST(cta as varchar(10)) + '-' + CAST(scta as varchar(10))) as scta, (CAST(scta as varchar(10))+ '-' + CAST(sscta as varchar(10))) as sscta, descripcion from catcta WHERE cta = {0} and scta = (CAST({0} as varchar(10)) + '-' + CAST({1} as varchar(10)))) b
                            ON a.sscta = b.sscta AND a.scta = b.scta
                            LEFT JOIN (SELECT cc, descripcion FROM cc) as d
                            ON a.cc = d.cc AND d.descripcion in {3}
                            WHERE fecha BETWEEN ? AND ?
                            GROUP BY d.descripcion, a.cta, a.poliza ,a.concepto , a.scta, a.sscta , c.descripcion, b.descripcion, a.tm, e.fechapol
                        ) x
                        WHERE noEco LIKE '%-%'    
                        GROUP BY noEco, x.cta, x.insumo, x.insumo_Desc, x.tipoInsumo, x.grupoInsumo, x.tipoInsumo_Desc, x.grupoInsumo_Desc, x.tipo_mov, x.fecha"
                    , busq.cta
                    , busq.scta
                    , auxTM
                    , busq.lstMaquina.ToParamInValue()
                );
            }

            return query;
        }
        public string ObtenerModeloEconomico(string noEconomico)
        {
            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;

                var economico = _context.tblM_CatMaquina.FirstOrDefault(x => x.noEconomico == noEconomico);

                return economico != null ? economico.modeloEquipo.descripcion : string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        public decimal ObtenerObraCostoHorario(string noEconomico)
        {
            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;

                var economico = _context.tblM_CatMaquina.FirstOrDefault(x => x.noEconomico == noEconomico);
                if (economico != null)
                {
                    var obra = _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == economico.centro_costos).id;
                    var caratula = _context.tblM_EncCaratula.Where(x => x.ccID == obra && x.isActivo).OrderByDescending(x => x.id).FirstOrDefault();
                    var costoHorario = _context.tblM_CapCaratula.FirstOrDefault(x => x.idCaratula == caratula.id && x.idModelo == economico.modeloEquipoID).costo;
                    if (caratula.moneda == 2)
                    {
                        costoHorario = costoHorario * getDolarDelDia(DateTime.Today);
                    }
                    return costoHorario;
                }
                else { return 0; }

            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// Consulta de dolar al día anterior
        /// </summary>
        /// <param name="dia">fecha de consulta</param>
        /// <returns>tipo de cambio del dolar</returns>
        public decimal getDolarDelDia(DateTime dia)
        {
            try
            {
                string consulta = string.Format("SELECT tipo_cambio FROM tipo_cambio WHERE fecha = '{0}'", dia.ToString("yyyyMMdd"));
                var dolar = (List<Core.DTO.Contabilidad.TipoCambioDllDTO>)_contextEnkontrol.Where(consulta).ToObject<List<Core.DTO.Contabilidad.TipoCambioDllDTO>>();
                return dolar.FirstOrDefault().tipo_cambio;
            }
            catch (Exception) { return 0; }
        }

        BusqRentabilidadDTO verificaBusqueda(BusqRentabilidadDTO busq)
        {
            _context.Configuration.AutoDetectChangesEnabled = false;
            if (busq.lstGrupo == null || busq.lstGrupo.Count == 0)
            {
                busq.lstGrupo = _context.tblM_CatGrupoMaquinaria.ToList()
                    .Where(w => w.estatus)
                    .Where(w => busq.tipo == 0 ? true : w.tipoEquipoID == busq.tipo)
                    .Select(s => s.id).ToList();
            }
            if (busq.lstModelo == null || busq.lstModelo.Count == 0)
            {
                busq.lstModelo = _context.tblM_CatModeloEquipo.ToList()
                    .Where(w => w.estatus)
                    .Where(w => busq.lstGrupo == null || busq.lstGrupo.Count == 0 ? true : busq.lstGrupo.Any(g => g == w.idGrupo))
                    .Select(s => s.id).ToList();
            }
            if (busq.lstMaquina == null || busq.lstMaquina.Count == 0)
            {
                busq.lstMaquina = _context.tblM_CatMaquina.ToList()
                    //.Where(w => w.estatus == 1)
                    .Where(w => busq.lstModelo == null || busq.lstModelo.Count == 0 ? true : busq.lstModelo.Any(m => m == w.modeloEquipoID))
                    .Select(s => s.noEconomico).ToList();
            }
            return busq;
        }

        List<OdbcParameterDTO> parametrosRentabilidad(BusqRentabilidadDTO busq)
        {
            var lst = new List<OdbcParameterDTO>();
            var max = new DateTime(busq.max.Year, busq.max.Month, DateTime.DaysInMonth(busq.max.Year, busq.max.Month));
            if (busq.tm != null) lst.AddRange(busq.tm.Select(s => new OdbcParameterDTO() { nombre = "tm", tipo = OdbcType.Int, valor = s }));
            lst.AddRange(busq.lstMaquina.Select(s => new OdbcParameterDTO() { nombre = "descripcion", tipo = OdbcType.VarChar, valor = s }));
            lst.Add(new OdbcParameterDTO() { nombre = "fecha", tipo = OdbcType.DateTime, valor = busq.min });
            lst.Add(new OdbcParameterDTO() { nombre = "fecha", tipo = OdbcType.DateTime, valor = max });
            if (busq.obra != "TODOS")
            {
                lst.Add(new OdbcParameterDTO() { nombre = "areaCuenta", tipo = OdbcType.VarChar, valor = busq.obra });
            }
            return lst;
        }



        #region combobox
        public List<ComboDTO> cboTipo()
        {
            _context.Configuration.AutoDetectChangesEnabled = false;
            var lstTipo = _context.tblM_CatTipoMaquinaria.Where(w => w.estatus).ToList();
            var cbo = lstTipo.Select(s => new ComboDTO()
                {
                    Text = s.descripcion,
                    Value = s.id.ToString(),
                }).ToList();
            return cbo;
        }
        public List<ComboDTO> cboGrupo(BusqRentabilidadDTO busq)
        {
            _context.Configuration.AutoDetectChangesEnabled = false;
            var lstGrupo = _context.tblM_CatGrupoMaquinaria.Where(w => busq.tipo == 0 ? true : w.tipoEquipoID == busq.tipo).ToList();
            var cbo = lstGrupo.Select(s => new ComboDTO()
                {
                    Text = s.descripcion,
                    Value = s.id.ToString(),
                }).OrderBy(x => x.Text).ToList();
            return cbo;
        }
        public List<ComboDTO> cboModelo(BusqRentabilidadDTO busq)
        {
            _context.Configuration.AutoDetectChangesEnabled = false;
            var lstModelo = _context.tblM_CatModeloEquipo.Where(w => busq.lstGrupo.Any(g => g == w.idGrupo)).ToList();
            var cbo = lstModelo.Select(s => new ComboDTO()
                {
                    Text = s.descripcion,
                    Value = s.id.ToString(),
                }).ToList();
            return cbo;
        }
        public List<ComboDTO> cboMaquina(BusqRentabilidadDTO busq)
        {
            _context.Configuration.AutoDetectChangesEnabled = false;

            return _context.tblM_CatMaquina
                .Where(w => (busq.obra.Equals("TODOS") ? true : busq.obra == w.centro_costos) && w.estatus == 1)
                    .ToList()
                    .Where(w => busq.lstGrupo == null ? true : busq.lstGrupo.Contains(w.grupoMaquinariaID))
                    .Where(w => busq.lstModelo == null ? true : busq.lstModelo.Contains(w.modeloEquipoID))
                    .Select(s => new ComboDTO
                    {
                        Text = s.noEconomico,
                        Value = s.noEconomico
                    }).ToList();
        }
        #endregion

        #region Analisis
        public List<RentabilidadDTO> getLstAnalisis(BusqAnalisisDTO busq)
        {
            try
            {
                busq = verificaBusquedaAnalisis(busq);
                var odbc = new OdbcConsultaDTO()
                {
                    consulta = queryAnalisisOpti(busq),
                    parametros = parametrosAnalisis(busq)
                };
                var lst = _contextEnkontrol.Select<RentabilidadDTO>(EnkontrolAmbienteEnum.Prod, odbc);
                return lst;
            }
            catch (Exception o_O)
            {
                return new List<RentabilidadDTO>();
            }
        }

        BusqAnalisisDTO verificaBusquedaAnalisis(BusqAnalisisDTO busq)
        {
            _context.Configuration.AutoDetectChangesEnabled = false;
            busq.lstMaquina = _context.tblM_CatMaquina.ToList()
                .Where(w => ((busq.tipo > 0 && busq.tipo < 4) ? w.grupoMaquinaria.tipoEquipoID == busq.tipo : true) && w.noEconomico != null)
                .Select(s => s.noEconomico).ToList();
            var auxLista = _context.tblM_CatMaquina.ToList()
                .Where(w => ((busq.tipo > 0 && busq.tipo < 4) ? w.grupoMaquinaria.tipoEquipoID == busq.tipo : true) && w.noEconomico == null)
                .ToList();
            return busq;
        }

        string queryAnalisisOpti(BusqAnalisisDTO busq)
        {
            var auxCCTodos = string.Format(@"WHERE d.descripcion IN {0}", busq.lstMaquina.ToParamInValue());
            switch (busq.tipo)
            {
                case 4:
                    auxCCTodos = string.Format(@"WHERE d.descripcion NOT IN {0} AND d.descripcion NOT IN ('FLETES DE MAQUINARIA Y EQUIPO','TALLER DE REPARACION DE LLANTAS OTR')", busq.lstMaquina.ToParamInValue());
                    break;
                case 5:
                    auxCCTodos = string.Format(@"WHERE d.descripcion = 'FLETES DE MAQUINARIA Y EQUIPO'");
                    break;
                case 6:
                    auxCCTodos = string.Format(@"WHERE d.descripcion = 'TALLER DE REPARACION DE LLANTAS OTR'");
                    break;
                default:
                    break;
            }

            return string.Format(
                @"SELECT noEco, cta, tipoInsumo, tipoInsumo_Desc, tipo_mov, sum(importe) as importe, fecha
                    FROM 
                    (
                        SELECT d.descripcion as noEco, a.cta as cta, a.poliza AS insumo, a.concepto as insumo_Desc, a.scta as tipoInsumo, c.descripcion as tipoInsumo_Desc, a.sscta as grupoInsumo, a.tm as tipo_mov, SUM(a.monto) as importe, e.fechapol as fecha
                        FROM (SELECT cta, poliza, concepto, (CAST(cta as varchar(10)) + '-' + CAST(scta as varchar(10))) as scta, tm, monto, tp, year, mes, (CAST(scta as varchar(10))+ '-' + CAST(sscta as varchar(10))) as sscta, cc from sc_movpol WHERE cta in (5000, 4000, 5280, 5900, 4901) and scta != '4000-3') a 
                        LEFT JOIN (SELECT poliza, tp, year, mes, fechapol FROM sc_polizas) e
                        ON a.poliza = e.poliza and a.tp = e.tp and a.year = e.year and a.mes = e.mes
                        LEFT JOIN (SELECT (CAST(cta as varchar(10)) + '-' + CAST(scta as varchar(10))) as scta, descripcion from catcta WHERE sscta = 0 and cta in (5000, 4000, 5280, 5900, 4901) and scta != '4000-3') c
                        ON a.scta = c.scta
                        LEFT JOIN (SELECT cc, descripcion FROM cc) as d
                        ON a.cc = d.cc {0}
                        GROUP BY d.descripcion, a.cta, a.poliza ,a.concepto , a.scta, c.descripcion, a.sscta, a.tm, e.fechapol
                    ) x  
                    GROUP BY noEco, x.cta, x.tipoInsumo, x.tipoInsumo_Desc, x.tipo_mov, x.fecha"
                , busq.tipo == 0 ? "" : auxCCTodos
            );
        }

        List<OdbcParameterDTO> parametrosAnalisis(BusqAnalisisDTO busq)
        {
            var lst = new List<OdbcParameterDTO>();
            var min = busq.fecha.AddDays(-35);
            //if (busq.tipo > 0 && busq.tipo < 5) { lst.AddRange(busq.lstMaquina.Select(s => new OdbcParameterDTO() { nombre = "descripcion", tipo = OdbcType.VarChar, valor = s })); }
            if (busq.tipo > 0 && busq.tipo < 5) { lst.AddRange(busq.lstMaquina.Select(s => new OdbcParameterDTO() { nombre = "descripcion", tipo = OdbcType.VarChar, valor = s })); }
            if (busq.obra != "TODOS")
            {
                lst.Add(new OdbcParameterDTO() { nombre = "areaCuenta", tipo = OdbcType.VarChar, valor = busq.obra });
            }
            return lst;
        }
        #endregion

        #region Kubrix
        public List<RentabilidadDTO> getLstKubrix(BusqKubrixDTO busq)
        {
            try
            {
                if (busq.lstModelo != null) busq.maquinas = getMaquinasKubrix(busq);
                //-->
                List<tblP_CC> centroCostos = new List<tblP_CC>();
                List<int> centroCostosID = new List<int>();

                if (busq.obra != null)
                {
                    centroCostos = _context.tblP_CC.Where(x => busq.obra.Contains(x.areaCuenta)).ToList();
                    if (busq.obra.Contains("S/A") || centroCostos.Count() < 1) centroCostosID.Add(-1);
                }

                if (centroCostos.Count() > 0) centroCostosID.AddRange(centroCostos.Select(x => x.id));
                DateTime auxFechaLimite = busq.fechaInicio.AddMonths(-1);

                var auxFacturasConcEntrada = _context.tblM_CapEncConciliacionHorometros.Where(x => x.estatus == 1 && x.facturado && x.fechaFin <= busq.fechaFin && x.fechaFin >= busq.fechaInicio && (centroCostosID.Count() == 0 ? true : centroCostosID.Contains(x.centroCostosID))).ToList();
                var auxFacturasConcSalida = _context.tblM_CapEncConciliacionHorometros.Where(x => x.estatus == 1 && x.facturado && x.fechaFin.Month == 12 && x.fechaFin < busq.fechaInicio).ToList();

                List<string> auxFacturasEntrada = new List<string>();
                foreach (var item in auxFacturasConcEntrada) { auxFacturasEntrada.AddRange(JsonConvert.DeserializeObject<List<string>>(item.factura)); }
                auxFacturasEntrada = auxFacturasEntrada.Distinct().ToList();
                auxFacturasEntrada.Remove("0");
                if (auxFacturasEntrada.Count() < 1) auxFacturasEntrada.Add("-1");

                List<string> auxFacturasSalida = new List<string>();
                foreach (var item in auxFacturasConcSalida) { auxFacturasSalida.AddRange(JsonConvert.DeserializeObject<List<string>>(item.factura)); }
                auxFacturasSalida = auxFacturasSalida.Distinct().ToList();
                auxFacturasSalida.Remove("0");
                if (auxFacturasSalida.Count() < 1) auxFacturasSalida.Add("-1");

                //var auxParametros = "";
                //foreach (var item in auxFacturasSalida) auxParametros += "'" + item + "',";
                //<--
                var odbcArrendadora = new OdbcConsultaDTO()
                {
                    consulta = queryKubrixArrendadora(busq, auxFacturasEntrada),
                    parametros = parametrosKubrix(busq, auxFacturasEntrada)
                };
                var odbcConstruplan = new OdbcConsultaDTO()
                {
                    consulta = queryKubrixCplanDpcn(busq),
                    parametros = parametrosKubrix(busq, auxFacturasEntrada)
                };
                var auxParametros2 = "";
                foreach (var item in odbcArrendadora.parametros)
                {
                    auxParametros2 += "'" + item.valor.ToString() + "',";
                }
                var lstArrendadora = _contextEnkontrolRentabilidad.SelectKubrix(EnkontrolEnum.ArrenProd, odbcArrendadora);
                var lstConstruplan = _contextEnkontrolRentabilidad.SelectKubrix(EnkontrolEnum.CplanProd, odbcConstruplan);

                var EncConciliaciones = _context.tblM_CapEncConciliacionHorometros.Select(x => new
                {
                    factura = x.factura,
                    centroCostosID = x.centroCostosID,
                    fechaInicio = x.fechaInicio,
                    fechaFin = x.fechaFin,
                    estatus = x.estatus
                }).ToList();
                var EncConciliacionesIDs = EncConciliaciones.Select(x => x.factura).ToList();
                var centrosCostosEnk = getListaCC();
                List<tblM_KBDivisionDetalle> divisiones = getDivisionesDetalle();

                foreach (var item in lstArrendadora)
                {
                    var auxCC = centrosCostosEnk.FirstOrDefault(x => x.Value == item.areaCuenta);
                    var divisionDetalle = divisiones.FirstOrDefault(y => (y.ac.areaCuenta) == item.areaCuenta);
                    if (divisionDetalle != null && divisionDetalle.division.division == "ENERGIA")
                    {
                        int a = 0;
                    }
                    item.areaCuenta = auxCC == null ? item.areaCuenta : auxCC.Text;
                    item.division = divisionDetalle == null ? "SIN DIVISION" : divisionDetalle.division.division;

                    var auxFacturaString = string.Format(@"""{0}""", item.referencia);
                    if (item.cta == 4000)
                    {
                        if (auxFacturasSalida.Contains(item.referencia))
                        {
                            auxFacturaString = string.Format(@"""{0}""", item.referencia);
                            var auxFactura = auxFacturasConcSalida.FirstOrDefault(x => x.factura.Contains(auxFacturaString) && x.estatus == 1);
                            if (auxFactura != null) item.fecha = auxFactura.fechaFin;
                        }
                        //if (item.fecha > busq.fechaFin)
                        //{
                        //    var conciliacionAux = auxFacturasConcEntrada.FirstOrDefault(x => x.factura.Contains("\"" + item.referencia + "\""));
                        //    item.cta = 1;
                        //    item.tipoInsumo = "1-3";
                        //    item.tipo_mov = 2;
                        //    item.tipoInsumo_Desc = "MOVIMIENTOS DE ENTRADA TARDÍA";
                        //    if (conciliacionAux != null) { item.fecha = conciliacionAux.fechaFin; }
                        //    else { item.fecha = busq.fechaFin; }
                        //    item.importe = item.importe * (-1);
                        //}
                    }
                    if (item.cta < 5000)
                    {
                        if (EncConciliacionesIDs.Any(x => x.Contains(auxFacturaString)))
                        {
                            var caratula = EncConciliaciones.FirstOrDefault(x => x.factura.Contains(auxFacturaString) && x.estatus == 1);
                            var cc = _context.tblP_CC.FirstOrDefault(x => x.id == caratula.centroCostosID);
                            item.referencia = "CONCILIACION " + (cc == null ? "S/AC" : cc.descripcion) + " " + caratula.fechaInicio.ToString("dd/MM/yyyy") + " - " + caratula.fechaFin.ToString("dd/MM/yyyy");
                        }
                        else { item.referencia = "SIN CONCILIACIÓN"; }
                    }
                }
                foreach (var item in lstConstruplan)
                {
                    var auxCC = centrosCostosEnk.FirstOrDefault(x => x.Prefijo.ToUpper() == item.areaCuenta.ToUpper());
                    var divisionDetalle = divisiones.FirstOrDefault(y => (auxCC != null ? y.ac.areaCuenta == auxCC.Value : false));

                    item.areaCuenta = auxCC == null ? item.areaCuenta : auxCC.Text;
                    item.division = divisionDetalle == null ? "SIN DIVISION" : divisionDetalle.division.division;
                }
                lstArrendadora.AddRange(lstConstruplan);
                return lstArrendadora.Where(x => x.fecha >= busq.fechaInicio).ToList();
            }
            catch (Exception o_O)
            {
                return new List<RentabilidadDTO>();
            }
        }

        public List<RentabilidadDTO> getLstKubrixConstruplan(BusqKubrixDTO busq)
        {
            try
            {
                if (busq.lstModelo != null) busq.maquinas = getMaquinasKubrix(busq);
                //-->
                List<tblP_CC> centroCostos = new List<tblP_CC>();
                List<int> centroCostosID = new List<int>();

                if (busq.obra != null)
                {
                    centroCostos = _context.tblP_CC.Where(x => busq.obra.Contains(x.areaCuenta)).ToList();
                    if (busq.obra.Contains("S/A") || centroCostos.Count() < 1) centroCostosID.Add(-1);
                }

                if (centroCostos.Count() > 0) centroCostosID.AddRange(centroCostos.Select(x => x.id));
                DateTime auxFechaLimite = busq.fechaInicio.AddMonths(-1);

                //var auxFacturasConcEntrada = _context.tblM_CapEncConciliacionHorometros.Where(x => x.estatus == 1 && x.facturado && x.fechaFin <= busq.fechaFin && x.fechaFin >= busq.fechaInicio && (centroCostosID.Count() == 0 ? true : centroCostosID.Contains(x.centroCostosID))).ToList();
                //var auxFacturasConcSalida = _context.tblM_CapEncConciliacionHorometros.Where(x => x.estatus == 1 && x.facturado && x.fechaFin.Month == 12 && x.fechaFin < busq.fechaInicio).ToList();

                //List<string> auxFacturasEntrada = new List<string>();
                //foreach (var item in auxFacturasConcEntrada) { auxFacturasEntrada.AddRange(JsonConvert.DeserializeObject<List<string>>(item.factura)); }
                //auxFacturasEntrada = auxFacturasEntrada.Distinct().ToList();
                //auxFacturasEntrada.Remove("0");
                //if (auxFacturasEntrada.Count() < 1) auxFacturasEntrada.Add("-1");

                //List<string> auxFacturasSalida = new List<string>();
                //foreach (var item in auxFacturasConcSalida) { auxFacturasSalida.AddRange(JsonConvert.DeserializeObject<List<string>>(item.factura)); }
                //auxFacturasSalida = auxFacturasSalida.Distinct().ToList();
                //auxFacturasSalida.Remove("0");
                //if (auxFacturasSalida.Count() < 1) auxFacturasSalida.Add("-1");

                //var auxParametros = "";
                //foreach (var item in auxFacturasSalida) auxParametros += "'" + item + "',";
                //<--
                //var odbcArrendadora = new OdbcConsultaDTO()
                //{
                //    consulta = queryKubrixArrendadora(busq, auxFacturasEntrada),
                //    parametros = parametrosKubrix(busq, auxFacturasEntrada)
                //};
                var odbcConstruplan = new OdbcConsultaDTO()
                {
                    consulta = queryKubrixCplan(busq),
                    parametros = parametrosKubrixCplan(busq)
                };
                var lstConstruplan = _contextEnkontrolRentabilidad.SelectKubrix(EnkontrolEnum.CplanProd, odbcConstruplan);

                var EncConciliaciones = _context.tblM_CapEncConciliacionHorometros.Select(x => new
                {
                    factura = x.factura,
                    centroCostosID = x.centroCostosID,
                    fechaInicio = x.fechaInicio,
                    fechaFin = x.fechaFin,
                    estatus = x.estatus
                }).ToList();
                var EncConciliacionesIDs = EncConciliaciones.Select(x => x.factura).ToList();
                var centrosCostosEnk = getListaCC();
                List<tblM_KBDivisionDetalle> divisiones = getDivisionesDetalle();

                foreach (var item in lstConstruplan)
                {
                    var auxCC = centrosCostosEnk.FirstOrDefault(x => x.Value == item.areaCuenta);
                    var divisionDetalle = divisiones.FirstOrDefault(y => (y.ac.areaCuenta) == item.areaCuenta);
                    if (divisionDetalle != null && divisionDetalle.division.division == "ENERGIA")
                    {
                        int a = 0;
                    }
                    item.areaCuenta = auxCC == null ? item.areaCuenta : auxCC.Text;
                    item.division = divisionDetalle == null ? "SIN DIVISION" : divisionDetalle.division.division;

                    var auxFacturaString = string.Format(@"""{0}""", item.referencia);
                    //if (item.cta == 4000)
                    //{
                    //    if (auxFacturasSalida.Contains(item.referencia))
                    //    {
                    //        auxFacturaString = string.Format(@"""{0}""", item.referencia);
                    //        var auxFactura = auxFacturasConcSalida.FirstOrDefault(x => x.factura.Contains(auxFacturaString) && x.estatus == 1);
                    //        if (auxFactura != null) item.fecha = auxFactura.fechaFin;
                    //    }
                    //    if (item.fecha > busq.fechaFin)
                    //    {
                    //        var conciliacionAux = auxFacturasConcEntrada.FirstOrDefault(x => x.factura.Contains("\"" + item.referencia + "\""));
                    //        item.cta = 1;
                    //        item.tipoInsumo = "1-3";
                    //        item.tipo_mov = 2;
                    //        item.tipoInsumo_Desc = "MOVIMIENTOS DE ENTRADA TARDÍA";
                    //        if (conciliacionAux != null) { item.fecha = conciliacionAux.fechaFin; }
                    //        else { item.fecha = busq.fechaFin; }
                    //        item.importe = item.importe * (-1);
                    //    }
                    //}
                    if (item.cta < 5000)
                    {
                        if (EncConciliacionesIDs.Any(x => x.Contains(auxFacturaString)))
                        {
                            var caratula = EncConciliaciones.FirstOrDefault(x => x.factura.Contains(auxFacturaString) && x.estatus == 1);
                            var cc = _context.tblP_CC.FirstOrDefault(x => x.id == caratula.centroCostosID);
                            item.referencia = "CONCILIACION " + (cc == null ? "S/AC" : cc.descripcion) + " " + caratula.fechaInicio.ToString("dd/MM/yyyy") + " - " + caratula.fechaFin.ToString("dd/MM/yyyy");
                        }
                        else { item.referencia = "SIN CONCILIACIÓN"; }
                    }
                }
                foreach (var item in lstConstruplan)
                {
                    var auxCC = centrosCostosEnk.FirstOrDefault(x => x.Prefijo.ToUpper() == item.areaCuenta.ToUpper());
                    var divisionDetalle = divisiones.FirstOrDefault(y => (auxCC != null ? y.ac.areaCuenta == auxCC.Value : false));

                    item.areaCuenta = auxCC == null ? item.areaCuenta : auxCC.Text;
                    item.division = divisionDetalle == null ? "SIN DIVISION" : divisionDetalle.division.division;
                }
                return lstConstruplan.Where(x => x.fecha >= busq.fechaInicio).ToList();
            }
            catch (Exception o_O)
            {
                return new List<RentabilidadDTO>();
            }
        }

        string queryKubrixCplan(BusqKubrixDTO busq)
        {
            //var auxMaquina = busq.maquinas == null ? (busq.economico == null ? "" : string.Format(@"AND TRIM(d.descripcion) = '{0}'", busq.economico)) : (busq.maquinas.Count() > 0 ? string.Format(@"AND TRIM(d.descripcion) IN {0}", busq.maquinas.ToParamInValue()) : string.Format(@"AND TRIM(d.descripcion) = ''"));
            string auxAreaCuenta = "AND (";
            List<string> auxObras = new List<string>();
            if (busq.ccEnkontrol != null) auxObras = busq.ccEnkontrol;
            foreach (var item in auxObras)
            {
                if (item == "-1") auxAreaCuenta += string.Format(@"TRIM(d.descripcion) NOT IN (SELECT (CAST(area as varchar(10)) + '-' + CAST(cuenta as varchar(10))) as Value FROM si_area_cuenta group by area, cuenta order by area, cuenta) OR ");
                else auxAreaCuenta += string.Format(@"TRIM(d.descripcion) = '{0}' OR ", item);
            }
            auxAreaCuenta = auxAreaCuenta.Substring(0, auxAreaCuenta.Length - 3);
            auxAreaCuenta += ")";
            // fecha, (poliza + '/' + referencia) as poliza, referencia, (CAST(area as varchar(3)) + '-' + CAST(cuenta_oc as varchar (3))) as areaCuenta
            return string.Format(
                @"SELECT
                    noEco, ISNULL(cta, 0) as cta, tipoInsumo, tipoInsumo_Desc, grupoInsumo, grupoInsumo_Desc, insumo, insumo_Desc, ISNULL(tipo_mov, 0) as tipo_mov, ISNULL(sum(importe), 0) as importe, 
                    fecha, poliza as poliza, referencia, (CAST(area as varchar(3)) + '-' + CAST(cuenta_oc as varchar (3))) as areaCuenta, linea
                FROM 
                (
		            SELECT 
                        ISNULL(d.descripcion, 'INDEFINIDO') as noEco, a.cta as cta, a.poliza AS insumo, a.concepto as insumo_Desc, a.scta as tipoInsumo, a.sscta as grupoInsumo, c.descripcion as tipoInsumo_Desc, 
                        b.descripcion as grupoInsumo_Desc, a.tm as tipo_mov, SUM(a.monto) as importe, e.fechapol as fecha, a.cc as cc, a.area as area, a.cuenta_oc as cuenta_oc,
                        (CAST(a.year as varchar(10)) + '-' +  CAST(a.mes as varchar(10)) + '-' +  CAST(a.tp as varchar(10)) + '-' +  CAST(a.poliza as varchar(10))) as poliza,
                        (CAST(a.year as varchar(10)) + '-' +  CAST(a.mes as varchar(10)) + '-' +  CAST(a.tp as varchar(10)) + '-' +  CAST(a.poliza as varchar(10)) + '-' +  CAST(a.linea as varchar(10))) as polizaAux, a.referencia as referencia, a.linea as linea
                    FROM 
                        (SELECT 
                            referencia, cta, poliza, concepto, (CAST(cta as varchar(10)) + '-' + CAST(scta as varchar(10))) as scta, tm, monto, tp, linea, year, mes, 
                            (CAST(scta as varchar(10))+ '-' + CAST(sscta as varchar(10))) as sscta, cc, area, cuenta_oc, (CAST(area as varchar(10)) + '-' +  CAST(cuenta_oc as varchar(10))) as areaCuenta 
                        FROM 
                            sc_movpol 
                        WHERE 
                            cta in (5000, 4000, 5280, 5900, 4901, 4900, 5901)
                        ) a 
                    LEFT JOIN 
                        (SELECT 
                            poliza, tp, year, mes, fechapol 
                        FROM 
                            sc_polizas
                        ) e
                    ON 
                        a.poliza = e.poliza and a.tp = e.tp and a.year = e.year and a.mes = e.mes
                    LEFT JOIN 
                        (SELECT 
                            (CAST(cta as varchar(10)) + '-' + CAST(scta as varchar(10))) as scta, descripcion 
                        FROM 
                            catcta 
                        WHERE 
                            sscta = 0 and cta in (5000, 4000, 5280, 5900, 4901, 4900, 5901)
                        ) c
                    ON 
                        a.scta = c.scta
                    LEFT JOIN 
                        (SELECT 
                            (CAST(cta as varchar(10)) + '-' + CAST(scta as varchar(10))) as scta, (CAST(scta as varchar(10))+ '-' + CAST(sscta as varchar(10))) as sscta, descripcion 
                        FROM 
                            catcta 
                        WHERE 
                            cta in (5000, 4000, 5280, 5900, 4901, 4900, 5901)
                        ) b
                    ON 
                        a.sscta = b.sscta AND a.scta = b.scta
                    LEFT JOIN 
                        (SELECT 
                            cc, descripcion 
                        FROM 
                            cc
                        ) d
                    ON 
                        a.cc = d.cc                              
                    WHERE 
                        (fecha >= ? AND fecha < ? {0})
                    GROUP BY 
                        d.descripcion, a.cta, a.poliza ,a.concepto , a.scta, a.sscta, a.tm, e.fechapol, poliza, a.area, a.cuenta_oc, a.linea, a.referencia, c.descripcion, b.descripcion, a.cc, a.linea
                ) x 
                GROUP BY 
                    noEco, x.cta, x.insumo, x.insumo_Desc, x.tipoInsumo, tipoInsumo_Desc, x.grupoInsumo, grupoInsumo_Desc, x.tipo_mov, x.fecha, x.poliza, x.area, x.cuenta_oc, x.referencia, x.linea"
                //, auxMaquina
                , auxObras.Count() > 0 ? auxAreaCuenta : ""
            );
        }

        string queryKubrixCplanDpcn(BusqKubrixDTO busq)
        {
            //var auxMaquina = busq.maquinas == null ? (busq.economico == null ? "" : string.Format(@"AND TRIM(d.descripcion) = '{0}'", busq.economico)) : (busq.maquinas.Count() > 0 ? string.Format(@"AND TRIM(d.descripcion) IN {0}", busq.maquinas.ToParamInValue()) : string.Format(@"AND TRIM(d.descripcion) = ''"));
            string auxAreaCuenta = "AND (";
            List<string> auxObras = new List<string>();
            if (busq.ccEnkontrol != null) auxObras = busq.ccEnkontrol;
            foreach (var item in auxObras)
            {
                if (item == "-1") auxAreaCuenta += string.Format(@"TRIM(d.descripcion) NOT IN (SELECT (CAST(area as varchar(10)) + '-' + CAST(cuenta as varchar(10))) as Value FROM si_area_cuenta group by area, cuenta order by area, cuenta) OR ");
                else auxAreaCuenta += string.Format(@"TRIM(d.descripcion) = '{0}' OR ", item);
            }
            auxAreaCuenta = auxAreaCuenta.Substring(0, auxAreaCuenta.Length - 3);
            auxAreaCuenta += ")";
            // fecha, (poliza + '/' + referencia) as poliza, referencia, (CAST(area as varchar(3)) + '-' + CAST(cuenta_oc as varchar (3))) as areaCuenta
            return string.Format(
                @"SELECT
                    noEco, ISNULL(cta, 0) as cta, tipoInsumo, tipoInsumo_Desc, grupoInsumo, grupoInsumo_Desc, insumo, insumo_Desc, ISNULL(tipo_mov, 0) as tipo_mov, ISNULL(sum(importe), 0) as importe, 
                    fecha, poliza as poliza, referencia, (CAST(area as varchar(3)) + '-' + CAST(cuenta_oc as varchar (3))) as areaCuenta, linea
                FROM 
                (
		            SELECT 
                        ISNULL(d.descripcion, 'INDEFINIDO') as noEco, a.cta as cta, a.poliza AS insumo, a.concepto as insumo_Desc, a.scta as tipoInsumo, a.sscta as grupoInsumo, c.descripcion as tipoInsumo_Desc, 
                        b.descripcion as grupoInsumo_Desc, a.tm as tipo_mov, SUM(a.monto) as importe, e.fechapol as fecha, a.cc as cc, a.area as area, a.cuenta_oc as cuenta_oc,
                        (CAST(a.year as varchar(10)) + '-' +  CAST(a.mes as varchar(10)) + '-' +  CAST(a.tp as varchar(10)) + '-' +  CAST(a.poliza as varchar(10))) as poliza,
                        (CAST(a.year as varchar(10)) + '-' +  CAST(a.mes as varchar(10)) + '-' +  CAST(a.tp as varchar(10)) + '-' +  CAST(a.poliza as varchar(10)) + '-' +  CAST(a.linea as varchar(10))) as polizaAux, a.referencia as referencia, a.linea as linea
                    FROM 
                        (SELECT 
                            referencia, cta, poliza, concepto, (CAST(cta as varchar(10)) + '-' + CAST(scta as varchar(10))) as scta, tm, monto, tp, linea, year, mes, 
                            (CAST(scta as varchar(10))+ '-' + CAST(sscta as varchar(10))) as sscta, cc, area, cuenta_oc, (CAST(area as varchar(10)) + '-' +  CAST(cuenta_oc as varchar(10))) as areaCuenta 
                        FROM 
                            sc_movpol 
                        WHERE 
                            scta = '5000-10' OR scta = '5900-3'
                        ) a 
                    LEFT JOIN 
                        (SELECT 
                            poliza, tp, year, mes, fechapol 
                        FROM 
                            sc_polizas
                        ) e
                    ON 
                        a.poliza = e.poliza and a.tp = e.tp and a.year = e.year and a.mes = e.mes
                    LEFT JOIN 
                        (SELECT 
                            (CAST(cta as varchar(10)) + '-' + CAST(scta as varchar(10))) as scta, descripcion 
                        FROM 
                            catcta 
                        WHERE 
                            sscta = 0 and (scta = '5000-10' OR scta = '5900-3')
                        ) c
                    ON 
                        a.scta = c.scta
                    LEFT JOIN 
                        (SELECT 
                            (CAST(cta as varchar(10)) + '-' + CAST(scta as varchar(10))) as scta, (CAST(scta as varchar(10))+ '-' + CAST(sscta as varchar(10))) as sscta, descripcion 
                        FROM 
                            catcta 
                        WHERE 
                            scta = '5000-10' OR scta = '5900-3'
                        ) b
                    ON 
                        a.sscta = b.sscta AND a.scta = b.scta
                    LEFT JOIN 
                        (SELECT 
                            cc, descripcion 
                        FROM 
                            cc
                        ) d
                    ON 
                        a.cc = d.cc                              
                    WHERE 
                        (fecha >= ? AND fecha < ? {0})
                    GROUP BY 
                        d.descripcion, a.cta, a.poliza ,a.concepto , a.scta, a.sscta, a.tm, e.fechapol, poliza, a.area, a.cuenta_oc, a.linea, a.referencia, c.descripcion, b.descripcion, a.cc, a.linea
                ) x 
                GROUP BY 
                    noEco, x.cta, x.insumo, x.insumo_Desc, x.tipoInsumo, tipoInsumo_Desc, x.grupoInsumo, grupoInsumo_Desc, x.tipo_mov, x.fecha, x.poliza, x.area, x.cuenta_oc, x.referencia, x.linea"
                //, auxMaquina
                , auxObras.Count() > 0 ? auxAreaCuenta : ""
            );
        }

        string queryKubrixArrendadora(BusqKubrixDTO busq, List<string> auxFacturasEntrada)
        {
            var auxMaquina = busq.maquinas == null ? (busq.economico == null ? "" : string.Format(@"AND TRIM(d.descripcion) = '{0}'", busq.economico)) : (busq.maquinas.Count() > 0 ? string.Format(@"AND TRIM(d.descripcion) IN {0}", busq.maquinas.ToParamInValue()) : string.Format(@"AND TRIM(d.descripcion) = ''"));
            string auxAreaCuenta = "AND (";
            List<string> auxObras = new List<string>();
            if (busq.obra != null) auxObras = busq.obra;
            foreach (var item in auxObras)
            {
                if (item == "S/A") auxAreaCuenta += string.Format(@"a.areaCuenta NOT IN (SELECT (CAST(area as varchar(10)) + '-' + CAST(cuenta as varchar(10))) as Value FROM si_area_cuenta group by area, cuenta order by area, cuenta) OR ");
                else auxAreaCuenta += string.Format(@"a.areaCuenta = '{0}' OR ", item);
            }
            auxAreaCuenta = auxAreaCuenta.Substring(0, auxAreaCuenta.Length - 3);
            auxAreaCuenta += ")";

            return string.Format(
                @"SELECT
                    noEco, ISNULL(cta, 0) as cta, tipoInsumo, tipoInsumo_Desc, grupoInsumo, grupoInsumo_Desc, insumo, insumo_Desc, ISNULL(tipo_mov, 0) as tipo_mov, ISNULL(sum(importe), 0) as importe, 
                    fecha,  poliza, referencia, (CAST(area as varchar(3)) + '-' + CAST(cuenta_oc as varchar (3))) as areaCuenta, linea
                FROM 
                (
		            SELECT 
                        ISNULL(d.descripcion, 'INDEFINIDO') as noEco, a.cta as cta, a.poliza AS insumo, a.concepto as insumo_Desc, a.scta as tipoInsumo, a.sscta as grupoInsumo, c.descripcion as tipoInsumo_Desc, 
                        b.descripcion as grupoInsumo_Desc, a.tm as tipo_mov, SUM(a.monto) as importe, e.fechapol as fecha, a.cc as cc, a.area as area, a.cuenta_oc as cuenta_oc,
                        (CAST(a.year as varchar(10)) + '-' +  CAST(a.mes as varchar(10)) + '-' +  CAST(a.tp as varchar(10)) + '-' +  CAST(a.poliza as varchar(10))) as poliza,
                        (CAST(a.year as varchar(10)) + '-' +  CAST(a.mes as varchar(10)) + '-' +  CAST(a.tp as varchar(10)) + '-' +  CAST(a.poliza as varchar(10)) + '-' +  CAST(a.linea as varchar(10))) as polizaAux, a.referencia as referencia, a.linea as linea
                    FROM 
                        (SELECT 
                            referencia, cta, poliza, concepto, (CAST(cta as varchar(10)) + '-' + CAST(scta as varchar(10))) as scta, tm, monto, tp, linea, year, mes, 
                            (CAST(scta as varchar(10))+ '-' + CAST(sscta as varchar(10))) as sscta, cc, area, cuenta_oc, (CAST(area as varchar(10)) + '-' +  CAST(cuenta_oc as varchar(10))) as areaCuenta 
                        FROM 
                            sc_movpol 
                        WHERE 
                            cta in (5000, 4000, 5280, 5900, 4901, 4900, 5901)
                        ) a 
                    LEFT JOIN 
                        (SELECT 
                            poliza, tp, year, mes, fechapol 
                        FROM 
                            sc_polizas
                        ) e
                    ON 
                        a.poliza = e.poliza and a.tp = e.tp and a.year = e.year and a.mes = e.mes
                    LEFT JOIN 
                        (SELECT 
                            (CAST(cta as varchar(10)) + '-' + CAST(scta as varchar(10))) as scta, descripcion 
                        FROM 
                            catcta 
                        WHERE 
                            sscta = 0 and cta in (5000, 4000, 5280, 5900, 4901, 4900, 5901)
                        ) c
                    ON 
                        a.scta = c.scta
                    LEFT JOIN 
                        (SELECT 
                            (CAST(cta as varchar(10)) + '-' + CAST(scta as varchar(10))) as scta, (CAST(scta as varchar(10))+ '-' + CAST(sscta as varchar(10))) as sscta, descripcion 
                        FROM 
                            catcta 
                        WHERE 
                            cta in (5000, 4000, 5280, 5900, 4901, 4900, 5901)
                        ) b
                    ON 
                        a.sscta = b.sscta AND a.scta = b.scta
                    LEFT JOIN 
                        (SELECT 
                            cc, descripcion 
                        FROM 
                            cc
                        ) d
                    ON 
                        a.cc = d.cc                              
                    WHERE 
                        (fecha >= ? AND fecha < ? {0} {1}) OR (a.cta = 4000 AND referencia in {2} )
                    GROUP BY 
                        d.descripcion, a.cta, a.poliza ,a.concepto , a.scta, a.sscta, a.tm, e.fechapol, poliza, a.area, a.cuenta_oc, a.linea, a.referencia, c.descripcion, b.descripcion, a.cc, a.linea
                ) x 
                GROUP BY 
                    noEco, x.cta, x.insumo, x.insumo_Desc, x.tipoInsumo, tipoInsumo_Desc, x.grupoInsumo, grupoInsumo_Desc, x.tipo_mov, x.fecha, x.poliza, x.area, x.cuenta_oc, x.referencia, x.linea"
                , auxMaquina
                , auxObras.Count() > 0 ? auxAreaCuenta : ""
                , auxFacturasEntrada.ToParamInValue()
            );
        }


        List<OdbcParameterDTO> parametrosKubrix(BusqKubrixDTO busq, List<string> auxFacturasEntrada)
        {
            var lst = new List<OdbcParameterDTO>();
            lst.Add(new OdbcParameterDTO() { nombre = "fecha", tipo = OdbcType.DateTime, valor = busq.fechaInicio });
            lst.Add(new OdbcParameterDTO() { nombre = "fecha", tipo = OdbcType.DateTime, valor = busq.fechaFin.AddDays(1) });
            if (busq.maquinas != null) { lst.AddRange(busq.maquinas.Select(s => new OdbcParameterDTO() { nombre = "descripcion", tipo = OdbcType.VarChar, valor = s })); }
            //if (busq.obra != "TODOS" && busq.obra != "S/A")
            //{
            //    var obra = _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == busq.obra);
            //    lst.Add(new OdbcParameterDTO() { nombre = "area", tipo = OdbcType.Numeric, valor = obra.area });
            //    lst.Add(new OdbcParameterDTO() { nombre = "cuenta", tipo = OdbcType.Numeric, valor = obra.cuenta });
            //}
            lst.AddRange(auxFacturasEntrada.Select(s => new OdbcParameterDTO() { nombre = "referencia", tipo = OdbcType.VarChar, valor = s }));
            return lst;
        }

        List<OdbcParameterDTO> parametrosKubrixCplan(BusqKubrixDTO busq)
        {
            var lst = new List<OdbcParameterDTO>();
            lst.Add(new OdbcParameterDTO() { nombre = "fecha", tipo = OdbcType.DateTime, valor = busq.fechaInicio });
            lst.Add(new OdbcParameterDTO() { nombre = "fecha", tipo = OdbcType.DateTime, valor = busq.fechaFin.AddDays(1) });
            if (busq.maquinas != null) { lst.AddRange(busq.maquinas.Select(s => new OdbcParameterDTO() { nombre = "descripcion", tipo = OdbcType.VarChar, valor = s })); }

            return lst;
        }

        public List<Tuple<int, string>> getRelacionEcoTipo(List<string> economicos)
        {
            List<Tuple<int, string>> data = new List<Tuple<int, string>>();
            var tipos = _context.tblM_CatMaquina.Where(x => economicos.Contains(x.noEconomico)).ToList();
            foreach (var item in tipos)
            {
                if (item.grupoMaquinaria.tipoEquipoID == 3 && item.CargoEmpresa == 2)
                    data.Add(new Tuple<int, string>(8, item.noEconomico));
                else
                    data.Add(new Tuple<int, string>(item.grupoMaquinaria.tipoEquipoID, item.noEconomico));
            }
            return data;
        }

        public List<RentabilidadDTO> getLstKubrixIngresosPendientes(BusqKubrixDTO busq)
        {
            try
            {
                List<RentabilidadDTO> lst = new List<RentabilidadDTO>();
                var conciliacionesFiltradas = _context.tblM_CapEncConciliacionHorometros.Where(x => x.fechaInicio <= busq.fechaFin && x.fechaFin >= busq.fechaFin).ToList();
                var areaCuenta = conciliacionesFiltradas.Select(x => x.centroCostosID);
                var tablaCC = _context.tblP_CC.Where(x => areaCuenta.Contains(x.id)).Select(x => new
                {
                    id = x.id,
                    areaCuenta = x.areaCuenta,
                    descripcion = x.descripcion
                }).ToList();
                var idConciliacionesFiltradas = conciliacionesFiltradas.Select(x => x.id).ToList();
                lst = _context.tblM_CapConciliacionHorometros.Where(x => busq.maquinas.Contains(x.economico) && idConciliacionesFiltradas.Contains(x.idEncCaratula) && x.idEmpresa == 2).ToList()
                .Select(x =>
                {
                    var caratula = conciliacionesFiltradas.FirstOrDefault(y => y.id == x.idEncCaratula);
                    //var auxTipoCambio = tipoCambio.FirstOrDefault(y => y.fecha == caratula.fechaFin);
                    var centroCostos = tablaCC.FirstOrDefault(y => y.id == caratula.centroCostosID);
                    return new RentabilidadDTO
                    {
                        noEco = x.economico,
                        cta = 4000,
                        tipoInsumo = "4000-1",
                        tipoInsumo_Desc = "POR RENTA DE EQUIPO",
                        grupoInsumo = "4000-1-1",
                        grupoInsumo_Desc = "POR RENTA DE EQUIPO",
                        insumo = x.id,
                        insumo_Desc = x.descripcion,
                        tipo_mov = 2,
                        importe = x.total,
                        fecha = caratula.fechaFin,
                        tipo = _context.tblM_CatMaquina.FirstOrDefault(y => y.noEconomico == x.economico).grupoMaquinaria.tipoEquipoID,
                        referencia = "CONCILIACION " + (centroCostos == null ? "S/AC" : centroCostos.descripcion) + " " + caratula.fechaInicio.ToString("dd/MM/yyyy") + " - " + caratula.fechaFin.ToString("dd/MM/yyyy")
                    };
                }).ToList();

                return lst;
            }
            catch (Exception o_O)
            {
                return new List<RentabilidadDTO>();
            }
        }

        public List<RentabilidadDTO> getLstKubrixDetalle(BusqKubrixDTO busq)
        {
            try
            {
                if (busq.lstModelo != null || busq.tipoEquipo > 0) busq.maquinas = getMaquinasKubrix(busq);
                if (busq.cta == 1)
                {
                    List<RentabilidadDTO> data = new List<RentabilidadDTO>();
                    if (busq.tipoEquipo != 7)
                    {
                        switch (busq.scta)
                        {
                            case 1:
                                data = getLstKubrixIngresosEstimacion(busq, false).Where(x => (busq.maquinas == null ? true : busq.maquinas.Contains(x.noEco))).ToList();
                                break;
                            case 2:
                                data = getLstKubrixIngresosPendientesGenerar(busq, -1, false).Where(x =>
                                    (busq.maquinas == null ? true : busq.maquinas.Contains(x.noEco))
                                    && (busq.sscta > 0 ? (busq.sscta < 3 ? (busq.sscta == 1 ? x.grupoInsumo == "1-2-1" : x.grupoInsumo == "1-2-2") : true) : true)).ToList();
                                break;
                        }
                    }
                    return data;
                }
                else
                {
                    List<string> auxObras = new List<string>();
                    if (busq.obra != null) auxObras = busq.obra;
                    var centroCostos = _context.tblP_CC.Where(x => auxObras.Contains(x.areaCuenta)).ToList();
                    List<int> centroCostosID = new List<int>(); ;
                    if (centroCostos.Count() > 0) centroCostosID = centroCostos.Select(x => x.id).ToList();

                    //var centroCostos = _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == busq.obra);
                    //var centroCostosID = -1;
                    //if (centroCostos != null) centroCostosID = centroCostos.id;
                    DateTime auxFechaLimite = busq.fechaFin.AddMonths(-1);

                    var auxFacturasConcEntrada = _context.tblM_CapEncConciliacionHorometros.Where(x => x.facturado && x.fechaFin <= busq.fechaFin && x.fechaFin >= busq.fechaInicio && (auxObras.Count() == 0 ? true : centroCostosID.Contains(x.centroCostosID))).ToList();
                    var auxFacturasConcSalida = _context.tblM_CapEncConciliacionHorometros.Where(x => x.facturado && x.fechaFin <= busq.fechaFin && x.fechaFin >= auxFechaLimite && (auxObras.Count() == 0 ? true : centroCostosID.Contains(x.centroCostosID))).ToList();

                    List<string> auxFacturasEntrada = new List<string>();
                    foreach (var item in auxFacturasConcEntrada) { auxFacturasEntrada.AddRange(JsonConvert.DeserializeObject<List<string>>(item.factura)); }
                    auxFacturasEntrada = auxFacturasEntrada.Distinct().ToList();
                    if (auxFacturasEntrada.Count() < 1) auxFacturasEntrada.Add("0");

                    List<string> auxFacturasSalida = new List<string>();
                    foreach (var item in auxFacturasConcSalida) { auxFacturasSalida.AddRange(JsonConvert.DeserializeObject<List<string>>(item.factura)); }
                    auxFacturasSalida = auxFacturasSalida.Distinct().ToList();
                    if (auxFacturasSalida.Count() < 1) auxFacturasSalida.Add("0");


                    var odbc = new OdbcConsultaDTO()
                    {
                        consulta = queryKubrixDetalle(busq, auxFacturasEntrada, auxFacturasSalida),
                        parametros = parametrosKubrixDetalle(busq, auxFacturasEntrada, auxFacturasSalida)
                    };
                    var auxParametros = "";
                    foreach (var item in odbc.parametros)
                    {
                        auxParametros += "'" + item.valor.ToString() + "',";
                    }

                    var lst = _contextEnkontrol.Select<RentabilidadDTO>(EnkontrolAmbienteEnum.Prod, odbc);
                    foreach (var item in lst)
                    {
                        if (auxFacturasEntrada.Contains(item.referencia))
                        {
                            var auxFacturaString = string.Format(@"""{0}""", item.referencia);
                            var auxFecha = auxFacturasConcEntrada.FirstOrDefault(x => x.factura.Contains(auxFacturaString)).fechaFin;
                            item.fecha = auxFecha;
                        }
                    }
                    return lst;
                }
            }
            catch (Exception o_O)
            {
                return new List<RentabilidadDTO>();
            }
        }

        string queryKubrixDetalle(BusqKubrixDTO busq, List<string> auxFacturasEntrada, List<string> auxFacturasSalida)
        {
            string query = "";
            var auxFletes = getFletesActivos();
            auxFletes.Add("FLETES DE MAQUINARIA Y EQUIPO");
            var auxMaquinas = busq.maquinas == null ? (busq.economico == null ? "" : string.Format(@"AND d.descripcion = '{0}'", busq.economico)) : (busq.maquinas.Count() > 0 ? (busq.tipoEquipo == 7 ? string.Format(@"AND d.descripcion NOT IN {0}", busq.maquinas.ToParamInValue()) : string.Format(@"AND d.descripcion IN {0}", busq.maquinas.ToParamInValue())) : string.Format(@"AND d.descripcion = ''"));
            string auxAreaCuenta = "AND (";
            List<string> auxObras = new List<string>();
            if (busq.obra != null) auxObras = busq.obra;
            foreach (var item in auxObras)
            {
                if (item == "S/A") auxAreaCuenta += string.Format(@"a.areaCuenta NOT IN (SELECT (CAST(area as varchar(10)) + '-' + CAST(cuenta as varchar(10))) as Value FROM si_area_cuenta group by area, cuenta order by area, cuenta) OR ");
                else auxAreaCuenta += string.Format(@"a.areaCuenta = {0} OR ", item);
            }
            auxAreaCuenta = auxAreaCuenta.Substring(0, auxAreaCuenta.Length - 3);

            //if (busq.maquinas != null) {
            query = string.Format(
                @"SELECT noEco, cta, tipoInsumo, grupoInsumo, insumo, insumo_Desc, tipo_mov, sum(importe) as importe, fecha, poliza, referencia
                    FROM 
                    (
                        SELECT 
                            d.descripcion as noEco, a.cta as cta, a.poliza AS insumo, a.concepto as insumo_Desc, a.scta as tipoInsumo, a.sscta as grupoInsumo, a.tm as tipo_mov, SUM(a.monto) as importe, 
                            a.referencia as referencia,
                            e.fechapol as fecha, (CAST(a.year as varchar(10)) + '-' +  CAST(a.mes as varchar(10)) + '-' +  CAST(a.tp as varchar(10)) + '-' +  CAST(a.poliza as varchar(10))) as poliza, 
                            a.area as area, a.cuenta_oc as cuenta,
                            (CAST(a.year as varchar(10)) + '-' +  CAST(a.mes as varchar(10)) + '-' +  CAST(a.tp as varchar(10)) + '-' +  CAST(a.poliza as varchar(10)) + '-' +  CAST(a.linea as varchar(10))) as polizaAux
                        FROM 
                            (SELECT 
                                referencia, cta, poliza, linea, concepto, scta, tm, monto, tp, year, mes, sscta, cc, area, cuenta_oc 
                            FROM 
                                sc_movpol 
                            WHERE 
                                cta = {0} and scta = {1} and sscta = {2}
                            ) a 
                        LEFT JOIN 
                            (SELECT 
                                poliza, tp, year, mes, fechapol 
                            FROM 
                                sc_polizas
                            ) e
                        ON 
                            a.poliza = e.poliza and a.tp = e.tp and a.year = e.year and a.mes = e.mes
                        LEFT JOIN 
                            (SELECT 
                                cc, descripcion 
                            FROM 
                                cc
                            ) as d
                        ON 
                            a.cc = d.cc                                
                        WHERE 
                            (fecha >= ? AND fecha < ? and cta != 4000 {3} {4}) 
                            OR
                            (fecha >= ? AND fecha < ? and cta = 4000 AND referencia NOT IN {6} {3} {4}) 
                            OR
                            (cta = 4000 AND referencia IN {5} {3} {4})
                        GROUP BY d.descripcion, a.cta, a.poliza ,a.concepto , a.scta, a.sscta, a.tm, e.fechapol, poliza, a.area, a.cuenta_oc, a.linea, a.referencia
                    ) x 
                    GROUP BY noEco, x.cta, x.insumo, x.insumo_Desc, x.tipoInsumo, x.grupoInsumo, x.tipo_mov, x.fecha, x.poliza, x.area, x.cuenta, x.referencia"
                , busq.cta
                , busq.scta
                , busq.sscta
                , auxMaquinas
                , auxObras.Count() > 0 ? auxAreaCuenta : ""
                , auxFacturasEntrada.ToParamInValue()
                , auxFacturasSalida.ToParamInValue()
            );
            return query;
        }

        List<OdbcParameterDTO> parametrosKubrixDetalle(BusqKubrixDTO busq, List<string> auxFacturasEntrada, List<string> auxFacturasSalida)
        {
            var lst = new List<OdbcParameterDTO>();
            //List<string> lstMaquinas = getMaquinasKubrix(busq);
            lst.Add(new OdbcParameterDTO() { nombre = "fecha", tipo = OdbcType.DateTime, valor = busq.fechaInicio.AddDays(1) });
            lst.Add(new OdbcParameterDTO() { nombre = "fecha", tipo = OdbcType.DateTime, valor = busq.fechaFin.AddDays(1) });
            if (busq.maquinas != null) { lst.AddRange(busq.maquinas.Select(s => new OdbcParameterDTO() { nombre = "descripcion", tipo = OdbcType.VarChar, valor = s })); }
            //if (busq.obra != "TODOS" && busq.obra != "S/A")
            //{
            //    var obra = _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == busq.obra);
            //    lst.Add(new OdbcParameterDTO() { nombre = "area", tipo = OdbcType.Numeric, valor = obra.area });
            //    lst.Add(new OdbcParameterDTO() { nombre = "cuenta", tipo = OdbcType.Numeric, valor = obra.cuenta });
            //}
            //
            lst.Add(new OdbcParameterDTO() { nombre = "fecha", tipo = OdbcType.DateTime, valor = busq.fechaInicio.AddDays(1) });
            lst.Add(new OdbcParameterDTO() { nombre = "fecha", tipo = OdbcType.DateTime, valor = busq.fechaFin.AddDays(1) });
            lst.AddRange(auxFacturasSalida.Select(s => new OdbcParameterDTO() { nombre = "referencia", tipo = OdbcType.VarChar, valor = s }));
            if (busq.maquinas != null) { lst.AddRange(busq.maquinas.Select(s => new OdbcParameterDTO() { nombre = "descripcion", tipo = OdbcType.VarChar, valor = s })); }
            //if (busq.obra != "TODOS" && busq.obra != "S/A")
            //{
            //    var obra = _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == busq.obra);
            //    lst.Add(new OdbcParameterDTO() { nombre = "area", tipo = OdbcType.Numeric, valor = obra.area });
            //    lst.Add(new OdbcParameterDTO() { nombre = "cuenta", tipo = OdbcType.Numeric, valor = obra.cuenta });
            //}
            //
            lst.AddRange(auxFacturasEntrada.Select(s => new OdbcParameterDTO() { nombre = "referencia", tipo = OdbcType.VarChar, valor = s }));
            if (busq.maquinas != null) { lst.AddRange(busq.maquinas.Select(s => new OdbcParameterDTO() { nombre = "descripcion", tipo = OdbcType.VarChar, valor = s })); }
            //if (busq.obra != "TODOS" && busq.obra != "S/A")
            //{
            //    var obra = _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == busq.obra);
            //    lst.Add(new OdbcParameterDTO() { nombre = "area", tipo = OdbcType.Numeric, valor = obra.area });
            //    lst.Add(new OdbcParameterDTO() { nombre = "cuenta", tipo = OdbcType.Numeric, valor = obra.cuenta });
            //}
            return lst;
        }

        List<string> getMaquinasKubrix(BusqKubrixDTO busq)
        {
            _context.Configuration.AutoDetectChangesEnabled = false;
            List<string> auxTipo = (new string[] { "CFC", "CF-", "MC-", "PR-", "TC-", "CAR-", "EX-", "HDT-" }).ToList();
            var auxFletes = getFletesActivos();
            List<string> auxAdminCentral = (new string[] { "ADMINISTRACIÓN CENTRAL ARRENDADORA  ", "ALMACEN Y TALLER EN PLANTA DE ASFALTO", "TALLER  HERMOSILLO NOMINA", "ALM Y TALLER EN PLANTA ASFALTO NOMINA", "GASTO TMC Y PATIO DE MAQ", "COMPRA HERRAMIENTA Y EQUIPO MENOR TMC" }).ToList();

            List<string> auxAdminProyectos = (new string[] { "ADQUISICION DE COMBUSTIBLES","MINADO LA COLORADA NOMINA","MINADO NOCHEBUENA II NOMINA","PROYECTO LA YAQUI NOMINA","PRESA DE JALES HERRADURA III NOMINA",
                "PATIOS NOCHE BUENA VII NOMINA","HERRADURA XIII NOMINA","PRESA SAN JULIAN ETAPA II NOMINA","MINADO SAN AGUSTIN NOMINA","PAVIMENTACION QUINTERO ARCE NOMINA","PLANTA DE ASFALTO NOMINA","CAPUFE HERMOSILLO-SANTA ANA NOMINA",
                "TALLER OVERHAUL EQ CONSTRUCCION NOMINA","CAPUFE - SANTANA NOMINA","PERSONAL EN DESARROLLO NÓMINA","TALLER CAPUFE TECATE NOMINA","TALLER DE REPARACION DE LLANTAS OTR NOMI","REHABILITACION COLECTOR 60\" NOMINA",
                "REHABILITACION COLECTOR DE 36” SAHUARO N","CAMINO CERRO PELON NOMINA","TERRACERIAS DESALADORA EMPALME NOMINA","MINADO CERRO PELON NOMINA","CONSERVACION CARRET CABOR-SONOYTA NOMINA","CONSTRUCCION DEL ACUEDUCTO DE 22\" NOMINA",
                "DEPOSITO DE JALES V NOMINA","NOMINA MINADO NOCHEBUENA III","NOMINA  EDIFICIO DE PROCESOS","NOMINA PAVIMENTACION ESTAC CENTRO","NOMINA REHABILITACION DE COLECTOR 60” ET","NOMINA PATIO 4B LA COLORADA",
                "NOMINA ESTACION DE COMPRESION PITIQUITO","NOMINA RECARPETEO BLVD SOLIDARIDAD","DEPRESIACIONES VARIAS","CONTROL DE MAQUINARIA","COMPRA COMPONENTES HERRADURA","COMPRA COMPONENTES LA COLORADA","COMPONENTES EQ CONSTRUCCION",
                "COMPONENTES SAN AGUSTIN","COMPRA DE HERAMIENTA Y EQUIPO MENOR","GASTOS TALLER MINADO LA COLORADA","GASTOS TALLER MINADO NOCHEBUENA II","GASTOS TALLER PROYECTO LA YAQUI","GASTOS TALL PRESA DE JALES HERRADURA III",
                "GASTOS TALLER PRESA SAN JULIAN ETAPA II","GASTOS TALLER PATIOS NOCHEBUENA VII","GASTOS TALLER HERRADURA XIII","GASTOS TALLER SAN AGUSTIN","CARRILERIA","COMPRA Y REPARACION DE CUCHARONES Y CAJA","GASTOS TALLER QUINTERO ARCE",
                "GASTOS TALLER VARIAS CALLES","GASTOS TALLER CAPUFE HERMOSILLO-SANTANA","GASTOS TALLER PLANTA DE ASFALTO","GASTOS TALLER PRESA DE AGUA SAN JULIAN","GASTOS TALLER CAPUFE-SANTA","GASTOS TALLER CAPUFE TECATE",
                "MINA SUBTERRANEA SAN JULIAN","GASTOS TALLER REHABILITACION DE COLECTOR","GASTOS TALLER MINADO LA HERRADURA","GASTOS TALLER CAMINO CERRO PELON","GASTOS TALLER TERRACERIAS DESALADORA EMP","GASTOS TALLER RENTA DE MAQUINARIA MULATO",
                "GASTOS TALLER MINADO CERRO PELON","GASTOS TALLER CONSERVA CARRET CAB-SONOYT","COMPRA DE COMPONENTES CERRO PELÓN","GASTOS TALLER CONSTRUCCION DEL ACUEDUCTO","GASTOS TALLER DEPOSITO DE JALES V","CRC Construplan",
                "GASTOS TALLER MMINADO NOCHEBUENA III","GASTOS TALLER EDIFICIO DE PROCESOS","GASTOS TALLER PAVIMENTACION ESTAC CENTRO","Gastos CRC Construplan","GASTOS TALLER GM SILAO MISCELANEOS ENSAM","GASTOS TALLER REHABILITACION DE COLECTOR",
                "Gastos Taller Overhaul Construccion","Compra Herramienta Overhaul Construcción","GASTOS TALLER PATIO 4B LA COLORADA","GASTOS TALLER ESTACION DE COMPRESION PIT","GASTOS TALLER ESTACION DE COMPRESION PITIQUITO","GASTOS TALLER AMPLIACION OFICINAS CENTRALES",
                "GASTOS TALLER RECARPETEO BLVD SOLIDARIDAD","GASTOS TALLER RECARPETEO BLVD SOLIDARIDA","GASTOS TALLER DEPOSITO DE JALES SECOS VI","GASTOS TALLER COMERCIALIZACION AUTOMOTRIZ","MAQUINARIA Y EQUIPO EN RENTA PURA" }).ToList();
            auxFletes.Add("FLETES DE MAQUINARIA Y EQUIPO");
            List<string> auxMaquinas = new List<string>();
            if (busq.tipoEquipo == 1 && busq.tipoEquipoMayor > 0 && busq.tipoEquipoMayor < 6) { auxTipo = auxTipo.GetRange((busq.tipoEquipoMayor), 1); }
            List<string> maquinas = new List<string>();
            switch (busq.tipoEquipo)
            {
                case 0:
                case 1:
                case 2:
                    maquinas = _context.tblM_CatMaquina.ToList()
                    .Where(w => w.noEconomico != null && !auxFletes.Contains(w.noEconomico.Trim())
                        && ((busq.economico == null || busq.economico == "") ? true : w.noEconomico == busq.economico)
                        && (busq.tipoEquipo > 0 ? ((busq.tipoEquipo > 3 && busq.tipoEquipo < 5) ? auxMaquinas.Contains(w.noEconomico) : w.grupoMaquinaria.tipoEquipoID == busq.tipoEquipo) : true)
                        && ((busq.lstModelo == null || busq.lstModelo.Count() < 1) ? true : busq.lstModelo.Contains(w.modeloEquipoID))
                        && (busq.tipoEquipo == 1 ? (busq.tipoEquipoMayor > 0 ? (busq.tipoEquipoMayor == 6 ? !auxTipo.Any(s => w.noEconomico.Contains(s)) : auxTipo.Any(s => w.noEconomico.Contains(s))) : true) : true))
                    .Select(s => s.noEconomico).ToList();
                    break;
                case 3:
                    maquinas = _context.tblM_CatMaquina.ToList()
                    .Where(w => w.noEconomico != null && !auxFletes.Contains(w.noEconomico.Trim())
                        && ((busq.economico == null || busq.economico == "") ? true : w.noEconomico == busq.economico)
                        && (busq.tipoEquipo > 0 ? ((busq.tipoEquipo > 3 && busq.tipoEquipo < 5) ? auxMaquinas.Contains(w.noEconomico) : w.grupoMaquinaria.tipoEquipoID == busq.tipoEquipo) : true)
                        && ((busq.lstModelo == null || busq.lstModelo.Count() < 1) ? true : busq.lstModelo.Contains(w.modeloEquipoID))
                        && (busq.tipoEquipo == 1 ? (busq.tipoEquipoMayor > 0 ? (busq.tipoEquipoMayor == 6 ? !auxTipo.Any(s => w.noEconomico.Contains(s)) : auxTipo.Any(s => w.noEconomico.Contains(s))) : true) : true)
                        && w.CargoEmpresa == 1)
                    .Select(s => s.noEconomico).ToList();
                    break;
                case 4:
                    maquinas.AddRange(auxFletes);
                    break;
                case 5:
                    maquinas.Add("TALLER DE REPARACION DE LLANTAS OTR");
                    break;
                case 6:
                    maquinas.AddRange(auxAdminCentral);
                    break;
                case 7:
                    maquinas = _context.tblM_CatMaquina.Where(x => x.noEconomico != null).Select(s => s.noEconomico).ToList();
                    maquinas.AddRange(auxFletes);
                    maquinas.Add("TALLER DE REPARACION DE LLANTAS OTR");
                    maquinas.AddRange(auxAdminCentral);
                    maquinas.AddRange(auxAdminProyectos);
                    break;
                case 8:
                    maquinas = _context.tblM_CatMaquina.ToList()
                    .Where(w => w.noEconomico != null && !auxFletes.Contains(w.noEconomico.Trim())
                        && ((busq.economico == null || busq.economico == "") ? true : w.noEconomico == busq.economico)
                        && (busq.tipoEquipo > 0 ? ((busq.tipoEquipo > 3 && busq.tipoEquipo < 5) ? auxMaquinas.Contains(w.noEconomico) : w.grupoMaquinaria.tipoEquipoID == busq.tipoEquipo) : true)
                        && ((busq.lstModelo == null || busq.lstModelo.Count() < 1) ? true : busq.lstModelo.Contains(w.modeloEquipoID))
                        && (busq.tipoEquipo == 1 ? (busq.tipoEquipoMayor > 0 ? (busq.tipoEquipoMayor == 6 ? !auxTipo.Any(s => w.noEconomico.Contains(s)) : auxTipo.Any(s => w.noEconomico.Contains(s))) : true) : true)
                        && w.CargoEmpresa == 2)
                    .Select(s => s.noEconomico).ToList();
                    break;
                case 9:
                    maquinas.AddRange(auxAdminProyectos);
                    break;
            }
            return maquinas;
        }

        public List<tblM_CatMaquina> fillComboMaquinaria(int grupoID, List<int> modeloID)
        {
            if (modeloID == null) { modeloID = new List<int>(); }
            return _context.tblM_CatMaquina.Where(x => x.estatus != 0 && (grupoID == -1 ? true : x.grupoMaquinariaID == grupoID) && (modeloID.Count() > 0 ? modeloID.Contains(x.modeloEquipoID) : true)).ToList();
        }

        public List<tblM_CatGrupoMaquinaria> FillGrupoEquipo()
        {
            using (var _db = new MainContext((int)EmpresaEnum.Arrendadora))
            {
                return _db.tblM_CatGrupoMaquinaria.Where(x => x.estatus == true).OrderBy(x => x.descripcion).ToList();
            }
        }

        public List<tblM_CatModeloEquipo> FillModeloEquipo(int grupoID)
        {
            using (var _db = new MainContext((int)EmpresaEnum.Arrendadora))
            {
                return _db.tblM_CatModeloEquipo.Where(x => x.estatus == true && x.marcaEquipo.estatus == true && (grupoID == -1 ? true : x.idGrupo == grupoID)).OrderBy(x => x.descripcion).ToList();
            }
        }

        public List<RentabilidadDTO> getLstKubrixIngresosEstimacion(BusqKubrixDTO busq, bool corte)
        {
            var tablaCC = _context.tblP_CC.Select(x => new
            {
                id = x.id,
                areaCuenta = x.areaCuenta,
                descripcion = x.descripcion
            }).ToList();
            List<string> auxObras = new List<string>();
            List<int> ccID = new List<int>();
            List<tblM_KBDivisionDetalle> divisiones = getDivisionesDetalle();

            if (busq.obra != null)
            {
                auxObras = busq.obra;
                if (busq.obra.Contains("S/A")) ccID.Add(-1);
            }

            var cc = tablaCC.Where(x => auxObras.Contains(x.areaCuenta)).ToList();

            if (cc.Count() > 0) ccID.AddRange(cc.Select(x => x.id));
            else { if (auxObras.Count > 0) { ccID.Add(-1); } }

            //var cc = tablaCC.FirstOrDefault(x => x.areaCuenta == busq.obra);
            //var ccID = 0;
            var economicoBusqueda = busq.economico == null ? "" : busq.economico;
            List<string> maquinasBusqueda = busq.maquinas == null ? new List<string>() : busq.maquinas;
            //if (cc != null) ccID = cc.id;
            //if (busq.obra == "S/A") ccID = -1;
            var auxConciliacionesAutorizadas = _context.tblM_AutorizaConciliacionHorometros.Where(x => x.pendienteAdmin == 1 && x.pendienteDirector == 1 && x.pendienteGerente == 1)
                .Select(x => new { conciliacionID = x.conciliacionID, firmaDirector = x.firmaDirector }).ToList();
            var auxConciliacionesAutorizadasID = auxConciliacionesAutorizadas.Select(x => x.conciliacionID);
            var conciliacionesAutorizadas = _context.tblM_CapEncConciliacionHorometros
            .Where(x =>
                (ccID.Count() > 0 ? ccID.Contains(x.centroCostosID) : true)
                && auxConciliacionesAutorizadasID.Contains(x.id)
                && x.fechaFin >= busq.fechaInicio
                && x.fechaFin <= busq.fechaFin
                && !x.facturado
            ).ToList();
            var conciliacionesAutorizadasID = conciliacionesAutorizadas.Select(x => x.id).ToList();
            var fechasConciliaciones = conciliacionesAutorizadas.Where(x => x.fechaFin != null)
            .Select(x =>
            {
                var firmaDirector = auxConciliacionesAutorizadas.FirstOrDefault(y => y.conciliacionID == x.id);
                var auxFechaAutorizacion = "";
                if (firmaDirector != null)
                {
                    var auxSplit = firmaDirector.firmaDirector.Split('|').ToList();
                    if (auxSplit.Count() > 2) { auxFechaAutorizacion = auxSplit[1]; }
                }
                var fechaAutorizacion = x.fechaFin;
                try { fechaAutorizacion = auxFechaAutorizacion.Length == 8 ? new DateTime(Int32.Parse(auxFechaAutorizacion.Substring(4, 4)), Int32.Parse(auxFechaAutorizacion.Substring(0, 2)), Int32.Parse(auxFechaAutorizacion.Substring(2, 2))) : x.fechaFin; }
                catch (Exception e) { fechaAutorizacion = x.fechaFin; }
                return fechaAutorizacion;
            }).Distinct().ToList();
            var tipoCambio = getDolarDias(fechasConciliaciones);
            int numMaquinas = maquinasBusqueda.Count();

            var conciliacionesAutorizadasDet = _context.tblM_CapConciliacionHorometros
            .Where(x =>
                conciliacionesAutorizadasID.Contains(x.idEncCaratula)
                && (economicoBusqueda == "" ? (numMaquinas > 0 ? maquinasBusqueda.Contains(x.economico) : true) : x.economico == economicoBusqueda) && x.total != 0
            ).ToList()
            .Select(x =>
            {
                var caratula = conciliacionesAutorizadas.FirstOrDefault(y => y.id == x.idEncCaratula);

                var centroCostos = tablaCC.FirstOrDefault(y => y.id == caratula.centroCostosID);
                var firmaDirector = auxConciliacionesAutorizadas.FirstOrDefault(y => y.conciliacionID == x.idEncCaratula);
                var auxFechaAutorizacion = "";
                if (firmaDirector != null)
                {
                    var auxSplit = firmaDirector.firmaDirector.Split('|').ToList();
                    if (auxSplit.Count() > 2) { auxFechaAutorizacion = auxSplit[1]; }
                }
                var fechaAutorizacion = caratula.fechaFin;
                try { fechaAutorizacion = auxFechaAutorizacion.Length == 8 ? new DateTime(Int32.Parse(auxFechaAutorizacion.Substring(4, 4)), Int32.Parse(auxFechaAutorizacion.Substring(0, 2)), Int32.Parse(auxFechaAutorizacion.Substring(2, 2))) : caratula.fechaFin; }
                catch (Exception e) { fechaAutorizacion = caratula.fechaFin; }
                var auxTipoCambio = tipoCambio.FirstOrDefault(y => y.fecha == fechaAutorizacion);
                var centroDeCostos = centroCostos == null ? "" : centroCostos.areaCuenta;
                var divisionDetalle = divisiones.FirstOrDefault(y => (y.ac.areaCuenta) == centroDeCostos);
                //var auxCC = centrosCostosEnk.FirstOrDefault(x => x.Value == item.areaCuenta);
                //var divisionDetalle = divisiones.FirstOrDefault(y => (y.ac.areaCuenta) == item.areaCuenta);

                //item.areaCuenta = auxCC == null ? item.areaCuenta : auxCC.Text;
                //item.division = divisionDetalle == null ? "SIN DIVISION" : divisionDetalle.division.division;

                return new RentabilidadDTO
                {
                    noEco = x.economico == null ? "INDEFINIDO" : x.economico,
                    cta = 1,
                    tipoInsumo = "1-1",
                    tipoInsumo_Desc = "CONCILIACIONES",
                    grupoInsumo = "1-1-0",
                    grupoInsumo_Desc = "CONCILIACIONES",
                    tipo_mov = 2,
                    importe = (x.moneda == 1 ? x.total : (auxTipoCambio == null ? x.total * (decimal)19.80 : x.total * auxTipoCambio.tipo_cambio)),
                    fecha = caratula.fechaFin,
                    insumo_Desc = "CONCILIACIÓN",
                    poliza = x.idEncCaratula.ToString().PadLeft(10, '0'),
                    areaCuenta = corte ? centroCostos.areaCuenta : centroCostos.areaCuenta + " " + centroCostos.descripcion,
                    referencia = "CONCILIACION " + (centroCostos == null ? "S/AC" : centroCostos.descripcion) + " " + caratula.fechaInicio.ToString("dd/MM/yyyy") + " - " + caratula.fechaFin.ToString("dd/MM/yyyy"),
                    division = divisionDetalle == null ? "SIN DIVISION" : divisionDetalle.division.division
                };
            }).ToList();
            return conciliacionesAutorizadasDet;
        }

        //public List<RentabilidadDTO> getLstKubrixIngresosPendientesGenerar(BusqKubrixDTO busq, int usuarioID, bool corte)
        //{
        //    List<RentabilidadDTO> pendientesGenerar = new List<RentabilidadDTO>();
        //    List<DateTime> diasTipoDeCambio = new List<DateTime>();
        //    List<tblM_KBDivisionDetalle> divisiones = getDivisionesDetalle();
        //    var auxConciliacionesAutorizadasID = _context.tblM_AutorizaConciliacionHorometros.Where(x => x.pendienteAdmin == 1 && x.pendienteDirector == 1 && x.pendienteGerente == 1).Select(x => x.conciliacionID);
        //    var centrosCostosEnkontrol = getListaCCByUsuario(usuarioID, 1).Select(x => x.Value).ToList();
        //    int numDiaActual = DateTime.Today.Day;
        //    DateTime /*diaMinimo = numDiaActual >= 15 ? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 16) : new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        //    if (diaMinimo < busq.fechaInicio)*/ diaMinimo = busq.fechaInicio;
        //    var diaInicioTC = diaMinimo;
        //    while (diaInicioTC <= busq.fechaFin)
        //    {
        //        diasTipoDeCambio.Add(diaInicioTC);
        //        diaInicioTC = diaInicioTC.AddDays(1);
        //    }
        //    var tipoCambio = getDolarDias(diasTipoDeCambio);
        //    List<int> auxModelos = new List<int>();
        //    if (busq.lstModelo != null) auxModelos = busq.lstModelo;

        //    List<string> auxObras = new List<string>();
        //    if (busq.obra != null) auxObras = busq.obra;
        //    var ccAConsiderar = new List<string>(auxObras);
        //    var ccSIGOPLAN = _context.tblP_CC.Select(x => x.areaCuenta).ToList();
        //    ccAConsiderar.AddRange(centrosCostosEnkontrol.Where(x => !ccSIGOPLAN.Contains(x)));

        //    var maquinasPPG = _context.tblM_CatMaquina
        //    .Where(x => x.CargoEmpresa == 1
        //        && x.noEconomico != null
        //        && (auxObras.Count() > 0 ? (ccAConsiderar.Contains(x.centro_costos)) : true)
        //        && (busq.economico == null ? true : x.noEconomico == busq.economico)
        //        && (auxModelos.Count() > 0 ? auxModelos.Contains(x.modeloEquipoID) : true))
        //    .GroupBy(x => x.noEconomico, (key, g) => g.OrderByDescending(e => e.id).FirstOrDefault()).Select(x => new
        //    {
        //        id = x.id,
        //        modeloEquipoID = x.modeloEquipoID,
        //        centro_costos = x.centro_costos,
        //        noEconomico = x.noEconomico,
        //        fechaAdquisicion = x.fechaAdquisicion
        //    }).ToList();
        //    List<string> strMaquinasPPG = maquinasPPG.Select(x => x.noEconomico).Distinct().ToList();
        //    List<int> modelosMaquinasPPG = maquinasPPG.Select(x => x.modeloEquipoID).Distinct().ToList();
        //    List<string> ccString = maquinasPPG.Select(x => x.centro_costos).Distinct().ToList();
        //    var ccsPPG = _context.tblP_CC.Where(x => ccString.Contains(x.areaCuenta) && x.estatus).Select(x => new
        //    {
        //        id = x.id,
        //        areaCuenta = x.areaCuenta,
        //        descripcion = x.descripcion
        //    }).ToList();
        //    List<int> ccIDsPPG = ccsPPG.Select(x => x.id).ToList();

        //    var encabezadoCaratula = _context.tblM_Caratula.Where(x => x.autorizada == 1).OrderByDescending(x => x.fechaAutorizacion).FirstOrDefault();
        //    var encabezadoCaratulaID = 0;
        //    if (encabezadoCaratula != null) encabezadoCaratulaID = encabezadoCaratula.id;

        //    if (encabezadoCaratulaID > 0)
        //    {
        //        //var encabezadosCaratula = _context.tblM_EncCaratula.Where(x => ccIDsPPG.Contains(x.ccID)).GroupBy(x => x.ccID, (key, g) => g.OrderByDescending(e => e.id).FirstOrDefault()).Select(x => new
        //        //{
        //        //    id = x.id,
        //        //    ccID = x.ccID,
        //        //    isActivo = x.isActivo,
        //        //    moneda = x.moneda
        //        //}).ToList();
        //        //List<int> encabezadosIDs = encabezadosCaratula.Select(x => x.id).ToList();

        //        var caratulasDetalles = _context.tblM_CaratulaDet.Where(x => x.caratula == encabezadoCaratulaID && modelosMaquinasPPG.Contains(x.idModelo)).Select(x => new
        //        {
        //            id = x.id,
        //            idModelo = x.idModelo,
        //            unidad = x.tipoHoraDia,
        //            costo = x.costoMXN
        //        }).ToList(); ;

        //        //var caratulasDetalles = _context.tblM_CapCaratula.Where(x =>
        //        //    encabezadosIDs.Contains(x.idCaratula)
        //        //    && x.activo && modelosMaquinasPPG.Contains(x.idModelo)
        //        //    ).Select(x => new
        //        //{
        //        //    id = x.id,
        //        //    idCaratula = x.idCaratula,
        //        //    idModelo = x.idModelo,
        //        //    unidad = x.unidad,
        //        //    costo = x.costo
        //        //}).ToList();
        //        var modelosCostoHora = caratulasDetalles.Where(x => x.unidad == 1).Select(x => x.idModelo).Distinct().ToList();
        //        var modelosCostoDiaAux = caratulasDetalles.Where(x => x.unidad == 2).Select(x => x.idModelo).Distinct().ToList();
        //        var maquinasCostoHora = maquinasPPG.Where(x => modelosCostoHora.Contains(x.modeloEquipoID) && x.noEconomico != null).ToList();
        //        var maquinasCostoDia = maquinasPPG.Where(x => modelosCostoDiaAux.Contains(x.modeloEquipoID) && x.noEconomico != null).ToList();
        //        var maquinasCostoHoraID = maquinasCostoHora.Select(x => x.noEconomico).ToList();
        //        var maquinasCostoDiaID = maquinasCostoDia.Select(x => x.noEconomico).ToList();
        //        var auxHorometros = busq.sscta == 2 ? null : _context.tblM_CapHorometro.Where(x => maquinasCostoHoraID.Contains(x.Economico) && x.Fecha >= diaMinimo).Select(x => new
        //        {
        //            id = x.id,
        //            Economico = x.Economico,
        //            Fecha = x.Fecha,
        //            HorasTrabajo = x.HorasTrabajo
        //        }).ToList();


        //        foreach (var maquina in maquinasCostoHora)
        //        {
        //            var ccPPG = ccsPPG.Where(x => x.areaCuenta == maquina.centro_costos).OrderByDescending(x => x.id).FirstOrDefault();
        //            if (ccPPG != null)
        //            {
        //                var caratulaPPG = caratulasDetalles.Where(x => x.idModelo == maquina.modeloEquipoID).OrderByDescending(x => x.id).FirstOrDefault();
        //                if (caratulaPPG != null && caratulaPPG.costo != 0)
        //                {
        //                    if (busq.sscta != 2)
        //                    {
        //                        var auxFechaInicio = busq.fechaInicio;
        //                        if (auxFechaInicio < maquina.fechaAdquisicion) auxFechaInicio = maquina.fechaAdquisicion;
        //                        var conciliacionDet = _context.tblM_CapConciliacionHorometros.Where(x => x.economico == maquina.noEconomico).Select(x => x.idEncCaratula).ToList();
        //                        var conciliacion = _context.tblM_CapEncConciliacionHorometros.Where(x => conciliacionDet.Contains(x.id) && auxConciliacionesAutorizadasID.Contains(x.id)).OrderByDescending(x => x.fechaFin).FirstOrDefault();
        //                        var divisionDetalle = divisiones.FirstOrDefault(x => (x.ac.areaCuenta) == ccPPG.areaCuenta);

        //                        var auxRentabilidadPPG = auxHorometros.Where(x => x.Economico == maquina.noEconomico && (conciliacion == null ? x.Fecha >= diaMinimo : (conciliacion.fechaFin.AddDays(1) >= auxFechaInicio ? x.Fecha > conciliacion.fechaFin : x.Fecha >= auxFechaInicio)) && x.Fecha <= busq.fechaFin).ToList()
        //                        .Select(x =>
        //                        {
        //                            var auxTipoCambio = tipoCambio.FirstOrDefault(y => y.fecha.Date == x.Fecha.Date);
        //                            return new RentabilidadDTO
        //                            {
        //                                noEco = x.Economico,
        //                                cta = 1,
        //                                tipoInsumo = "1-2",
        //                                tipoInsumo_Desc = "CONCILIACIONES POR GENERAR",
        //                                grupoInsumo = "1-2-1",
        //                                grupoInsumo_Desc = "HORAS TRABAJADAS",
        //                                tipo_mov = 3,
        //                                //importe = (auxEncCaratula.moneda == 1 ? x.HorasTrabajo * caratulaPPG.costo : (auxTipoCambio == null ? x.HorasTrabajo * caratulaPPG.costo * (decimal)19.80 : x.HorasTrabajo * caratulaPPG.costo * auxTipoCambio.tipo_cambio)),
        //                                importe = x.HorasTrabajo * caratulaPPG.costo,
        //                                fecha = x.Fecha.Date > busq.fechaFin ? conciliacion.fechaFin : x.Fecha,
        //                                insumo_Desc = "HORAS TRABAJADAS",
        //                                poliza = x.id.ToString().PadLeft(10, '0'),
        //                                areaCuenta = corte ? ccPPG.areaCuenta : ccPPG.areaCuenta + " " + ccPPG.descripcion,
        //                                referencia = ccPPG.descripcion,
        //                                division = divisionDetalle == null ? "SIN DIVISION" : divisionDetalle.division.division
        //                            };
        //                        }).ToList();
        //                        pendientesGenerar.AddRange(auxRentabilidadPPG);
        //                    }
        //                }
        //            }
        //        }

        //        foreach (var maquina in maquinasCostoDia)
        //        {
        //            var ccPPG = ccsPPG.Where(x => x.areaCuenta == maquina.centro_costos).OrderByDescending(x => x.id).FirstOrDefault();
        //            if (ccPPG != null)
        //            {
        //                var caratulaPPG = caratulasDetalles.Where(x => x.idModelo == maquina.modeloEquipoID).OrderByDescending(x => x.id).FirstOrDefault();
        //                if (caratulaPPG != null && caratulaPPG.costo != 0)
        //                {
        //                    if (busq.sscta != 1)
        //                    {
        //                        var conciliacionDet = _context.tblM_CapConciliacionHorometros.Where(x => x.economico == maquina.noEconomico).Select(x => x.idEncCaratula).ToList();
        //                        var conciliacion = _context.tblM_CapEncConciliacionHorometros.Where(x => conciliacionDet.Contains(x.id) && auxConciliacionesAutorizadasID.Contains(x.id)).OrderByDescending(x => x.fechaFin).FirstOrDefault();

        //                        var auxFechaInicio = busq.fechaInicio;
        //                        if (auxFechaInicio < maquina.fechaAdquisicion) auxFechaInicio = maquina.fechaAdquisicion;
        //                        List<DateTime> auxDiasTrabajados = new List<DateTime>();
        //                        DateTime auxDiaMinimoTrabajado = conciliacion == null ? auxFechaInicio : (conciliacion.fechaFin.AddDays(1) >= auxFechaInicio ? conciliacion.fechaFin.AddDays(1) : auxFechaInicio);
        //                        while (auxDiaMinimoTrabajado.Date <= busq.fechaFin.Date)
        //                        {
        //                            auxDiasTrabajados.Add(auxDiaMinimoTrabajado);
        //                            auxDiaMinimoTrabajado = auxDiaMinimoTrabajado.AddDays(1);
        //                        }
        //                        foreach (var fecha in auxDiasTrabajados)
        //                        {
        //                            var auxTipoCambio = tipoCambio.FirstOrDefault(y => y.fecha.Date == fecha.Date);
        //                            var divisionDetalle = divisiones.FirstOrDefault(x => (x.ac.areaCuenta) == ccPPG.areaCuenta);
        //                            RentabilidadDTO auxItemRentabilidadPPG = new RentabilidadDTO
        //                            {
        //                                noEco = maquina.noEconomico,
        //                                cta = 1,
        //                                tipoInsumo = "1-2",
        //                                tipoInsumo_Desc = "CONCILIACIONES POR GENERAR",
        //                                grupoInsumo = "1-2-2",
        //                                grupoInsumo_Desc = "DIAS TRABAJADOS",
        //                                tipo_mov = 3,
        //                                //importe = (auxEncCaratula.moneda == 1 ? caratulaPPG.costo : (auxTipoCambio == null ? caratulaPPG.costo * (decimal)19.80 : caratulaPPG.costo * auxTipoCambio.tipo_cambio)),
        //                                importe = caratulaPPG.costo,
        //                                fecha = fecha,
        //                                insumo_Desc = "DÍA TRABAJADO",
        //                                poliza = "--",
        //                                areaCuenta = corte ? ccPPG.areaCuenta : ccPPG.areaCuenta + " " + ccPPG.descripcion,
        //                                referencia = ccPPG.descripcion,
        //                                division = divisionDetalle == null ? "SIN DIVISION" : divisionDetalle.division.division
        //                            };
        //                            pendientesGenerar.Add(auxItemRentabilidadPPG);
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        pendientesGenerar = pendientesGenerar.GroupBy(x => new { x.noEco, x.areaCuenta, x.fecha, x.division }).Select(x => new RentabilidadDTO
        //        {
        //            noEco = x.Key.noEco,
        //            cta = 1,
        //            tipoInsumo = x.Max(y => y.tipoInsumo),
        //            tipoInsumo_Desc = x.Max(y => y.tipoInsumo_Desc),
        //            grupoInsumo = x.Max(y => y.grupoInsumo),
        //            grupoInsumo_Desc = x.Max(y => y.grupoInsumo_Desc),
        //            tipo_mov = 3,
        //            importe = x.Sum(y => y.importe),
        //            fecha = x.Key.fecha,
        //            insumo_Desc = x.Max(y => y.insumo_Desc),
        //            poliza = "--",
        //            areaCuenta = x.Max(y => y.areaCuenta),
        //            referencia = x.Max(y => y.referencia),
        //            division = x.Key.division
        //        }).ToList();
        //        //var dias = pendientesGenerar.GroupBy(x => x.fecha).Select(x =>  new{ fecha = x.Key, importe = x.Sum(y => y.importe)}).ToList();
        //    }
        //    return pendientesGenerar;
        //}

        List<RentabilidadDTO> getLstKubrixIngresosPendientesGenerarEspecial(BusqKubrixDTO busq, int usuarioID, bool corte)
        {
            List<RentabilidadDTO> pendientesGenerar = new List<RentabilidadDTO>();
            List<DateTime> diasTipoDeCambio = new List<DateTime>();
            List<tblM_KBDivisionDetalle> divisiones = getDivisionesDetalle();
            var auxConciliacionesAutorizadasID = _context.tblM_AutorizaConciliacionHorometros.Where(x => x.pendienteAdmin == 1 && x.pendienteDirector == 1 && x.pendienteGerente == 1).Select(x => x.conciliacionID);
            var centrosCostosEnkontrol = getListaCCByUsuario(usuarioID, 1).Select(x => x.Value).ToList();
            int numDiaActual = DateTime.Today.Day;
            DateTime /*diaMinimo = numDiaActual >= 15 ? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 16) : new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            if (diaMinimo < busq.fechaInicio)*/ diaMinimo = busq.fechaInicio;
            var diaInicioTC = diaMinimo;
            while (diaInicioTC <= busq.fechaFin)
            {
                diasTipoDeCambio.Add(diaInicioTC);
                diaInicioTC = diaInicioTC.AddDays(1);
            }
            var tipoCambio = getDolarDias(diasTipoDeCambio);
            List<int> auxModelos = new List<int>();
            if (busq.lstModelo != null) auxModelos = busq.lstModelo;

            List<string> auxObras = new List<string>();
            if (busq.obra != null) auxObras = busq.obra;
            var ccAConsiderar = new List<string>(auxObras);
            var ccSIGOPLAN = _context.tblP_CC.Select(x => x.areaCuenta).ToList();
            ccAConsiderar.AddRange(centrosCostosEnkontrol.Where(x => !ccSIGOPLAN.Contains(x)));

            var maquinasPPG = _context.tblM_CatMaquina
            .Where(x => x.CargoEmpresa == 1
                && x.noEconomico != null
                && (auxObras.Count() > 0 ? (ccAConsiderar.Contains(x.centro_costos)) : true)
                && (busq.economico == null ? true : x.noEconomico == busq.economico)
                && (auxModelos.Count() > 0 ? auxModelos.Contains(x.modeloEquipoID) : true)
                /*&& (x.centro_costos == "18-2" || x.centro_costos == "8-9" || x.centro_costos == "50-1" || x.centro_costos == "51-1" || x.centro_costos == "7-17")*/)
            .GroupBy(x => x.noEconomico, (key, g) => g.OrderByDescending(e => e.id).FirstOrDefault()).Select(x => new
            {
                id = x.id,
                modeloEquipoID = x.modeloEquipoID,
                centro_costos = x.centro_costos,
                noEconomico = x.noEconomico,
                fechaAdquisicion = x.fechaAdquisicion
            }).ToList();
            List<string> strMaquinasPPG = maquinasPPG.Select(x => x.noEconomico).Distinct().ToList();
            List<int> modelosMaquinasPPG = maquinasPPG.Select(x => x.modeloEquipoID).Distinct().ToList();
            List<string> ccString = maquinasPPG.Select(x => x.centro_costos).Distinct().ToList();
            var ccsPPG = _context.tblP_CC.Where(x => ccString.Contains(x.areaCuenta) && x.estatus).Select(x => new
            {
                id = x.id,
                areaCuenta = x.areaCuenta,
                descripcion = x.descripcion
            }).ToList();
            List<int> ccIDsPPG = ccsPPG.Select(x => x.id).ToList();
            //var encabezadosCaratula = _context.tblM_EncCaratula.Where(x => ccIDsPPG.Contains(x.ccID)).GroupBy(x => x.ccID, (key, g) => g.OrderByDescending(e => e.id).FirstOrDefault()).Select(x => new
            //{
            //    id = x.id,
            //    ccID = x.ccID,
            //    isActivo = x.isActivo,
            //    moneda = x.moneda
            ////}).ToList();
            //List<int> encabezadosIDs = encabezadosCaratula.Select(x => x.id).ToList();

            var centrosCosto = _context.tblP_CC.ToList();

            var caratulasObras = _context.tblM_IndicadoresCaratula.Select(x => new ComboDTO
            {
                Orden = x.idCC,
                Value = "",
                Text = x.moneda.ToString()
            }).ToList();

            foreach (var item in caratulasObras) 
            {
                item.Value = centrosCosto.FirstOrDefault(y => y.id == item.Orden).areaCuenta;
            }

            caratulasObras = caratulasObras.Where(y => y.Value.Contains("-")).ToList();

            var caratulasDetalles = _context.tblM_CaratulaDet.Where(x =>
                x.caratula == 1
                && modelosMaquinasPPG.Contains(x.idModelo)
                ).Select(x => new
                {
                    id = x.id,
                    idCaratula = x.caratula,
                    idModelo = x.idModelo,
                    unidad = x.tipoHoraDia,
                    costoDLLS = x.costoDLLS,
                    costoMXN = x.costoMXN
                }).ToList();
            var modelosCostoHora = caratulasDetalles.Where(x => x.unidad == 1).Select(x => x.idModelo).Distinct().ToList();
            var modelosCostoDiaAux = caratulasDetalles.Where(x => x.unidad == 2).Select(x => x.idModelo).Distinct().ToList();
            var maquinasCostoHora = maquinasPPG.Where(x => modelosCostoHora.Contains(x.modeloEquipoID) && x.noEconomico != null).Select(x => x.noEconomico).ToList();
            var maquinasCostoDia = maquinasPPG.Where(x => modelosCostoDiaAux.Contains(x.modeloEquipoID) && x.noEconomico != null).Select(x => x.noEconomico).ToList();
            var auxHorometros = busq.sscta == 2 ? null : _context.tblM_CapHorometro.Where(x => maquinasCostoHora.Contains(x.Economico) && x.Fecha >= diaMinimo).Select(x => new
            {
                id = x.id,
                Economico = x.Economico,
                Fecha = x.Fecha,
                HorasTrabajo = x.HorasTrabajo
            }).ToList();

            foreach (var maquina in maquinasPPG)
            {
                var ccPPG = ccsPPG.Where(x => x.areaCuenta == maquina.centro_costos).OrderByDescending(x => x.id).FirstOrDefault();
                var caratulaObra = caratulasObras.FirstOrDefault(x => x.Value == maquina.centro_costos);                
                if (caratulaObra != null) 
                if (ccPPG != null && caratulasObras != null)
                {
                    var moneda = 1;
                    moneda = caratulaObra.Text == "True" ? 2 : 1;
                    //var auxEncCaratula = encabezadosCaratula.Where(x => x.ccID == ccPPG.id && x.isActivo).OrderByDescending(x => x.id).FirstOrDefault();
                    //if (auxEncCaratula != null)
                    //{
                    var caratulaPPG = caratulasDetalles.Where(x => x.idCaratula == 1 && x.idModelo == maquina.modeloEquipoID).OrderByDescending(x => x.id).FirstOrDefault();

                    if (caratulaPPG != null && caratulaPPG.costoDLLS != 0)
                    {
                        switch (caratulaPPG.unidad)
                        {
                            case 1:
                                if (busq.sscta != 2)
                                {
                                    var auxFechaInicio = busq.fechaInicio;
                                    if (auxFechaInicio < maquina.fechaAdquisicion) auxFechaInicio = maquina.fechaAdquisicion;
                                    var conciliacionDet = _context.tblM_CapConciliacionHorometros.Where(x => x.economico == maquina.noEconomico).Select(x => x.idEncCaratula).ToList();
                                    var conciliacion = _context.tblM_CapEncConciliacionHorometros.Where(x => conciliacionDet.Contains(x.id) && auxConciliacionesAutorizadasID.Contains(x.id)).OrderByDescending(x => x.fechaFin).FirstOrDefault();
                                    var divisionDetalle = divisiones.FirstOrDefault(x => (x.ac.areaCuenta) == ccPPG.areaCuenta);

                                    var auxRentabilidadPPG = auxHorometros.Where(x => x.Economico == maquina.noEconomico && (conciliacion == null ? x.Fecha >= diaMinimo : (conciliacion.fechaFin.AddDays(1) >= auxFechaInicio ? x.Fecha > conciliacion.fechaFin : x.Fecha >= auxFechaInicio)) && x.Fecha <= busq.fechaFin).ToList()
                                    .Select(x =>
                                    {
                                        var auxTipoCambio = tipoCambio.FirstOrDefault(y => y.fecha.Date == x.Fecha.Date);
                                        return new RentabilidadDTO
                                        {
                                            noEco = x.Economico,
                                            cta = 1,
                                            tipoInsumo = "1-2",
                                            tipoInsumo_Desc = "CONCILIACIONES POR GENERAR",
                                            grupoInsumo = "1-2-1",
                                            grupoInsumo_Desc = "HORAS TRABAJADAS",
                                            tipo_mov = 3,
                                            importe = (moneda == 1 ? x.HorasTrabajo * caratulaPPG.costoMXN : (auxTipoCambio == null ? x.HorasTrabajo * caratulaPPG.costoDLLS * (decimal)17.50 : x.HorasTrabajo * caratulaPPG.costoDLLS * auxTipoCambio.tipo_cambio)),
                                            fecha = x.Fecha.Date > busq.fechaFin ? conciliacion.fechaFin : x.Fecha,
                                            insumo_Desc = "HORAS TRABAJADAS",
                                            poliza = x.id.ToString().PadLeft(10, '0'),
                                            areaCuenta = corte ? ccPPG.areaCuenta : ccPPG.areaCuenta + " " + ccPPG.descripcion,
                                            referencia = ccPPG.descripcion,
                                            division = divisionDetalle == null ? "SIN DIVISION" : divisionDetalle.division.division
                                        };
                                    }).ToList();
                                    pendientesGenerar.AddRange(auxRentabilidadPPG);
                                }
                                break;
                            case 2:
                                if (busq.sscta != 1)
                                {
                                    var conciliacionDet = _context.tblM_CapConciliacionHorometros.Where(x => x.economico == maquina.noEconomico).Select(x => x.idEncCaratula).ToList();
                                    var conciliacion = _context.tblM_CapEncConciliacionHorometros.Where(x => conciliacionDet.Contains(x.id) && auxConciliacionesAutorizadasID.Contains(x.id)).OrderByDescending(x => x.fechaFin).FirstOrDefault();

                                    var auxFechaInicio = busq.fechaInicio;
                                    if (auxFechaInicio < maquina.fechaAdquisicion) auxFechaInicio = maquina.fechaAdquisicion;
                                    List<DateTime> auxDiasTrabajados = new List<DateTime>();
                                    DateTime auxDiaMinimoTrabajado = conciliacion == null ? auxFechaInicio : (conciliacion.fechaFin.AddDays(1) >= auxFechaInicio ? conciliacion.fechaFin.AddDays(1) : auxFechaInicio);
                                    while (auxDiaMinimoTrabajado.Date <= busq.fechaFin.Date)
                                    {
                                        auxDiasTrabajados.Add(auxDiaMinimoTrabajado);
                                        auxDiaMinimoTrabajado = auxDiaMinimoTrabajado.AddDays(1);
                                    }
                                    foreach (var fecha in auxDiasTrabajados)
                                    {
                                        var auxTipoCambio = tipoCambio.FirstOrDefault(y => y.fecha.Date == fecha.Date);
                                        var divisionDetalle = divisiones.FirstOrDefault(x => (x.ac.areaCuenta) == ccPPG.areaCuenta);
                                        RentabilidadDTO auxItemRentabilidadPPG = new RentabilidadDTO
                                        {
                                            noEco = maquina.noEconomico,
                                            cta = 1,
                                            tipoInsumo = "1-2",
                                            tipoInsumo_Desc = "CONCILIACIONES POR GENERAR",
                                            grupoInsumo = "1-2-2",
                                            grupoInsumo_Desc = "DIAS TRABAJADOS",
                                            tipo_mov = 3,
                                            importe = (moneda == 1 ? caratulaPPG.costoMXN : (auxTipoCambio == null ? caratulaPPG.costoDLLS * (decimal)19.80 : caratulaPPG.costoDLLS * auxTipoCambio.tipo_cambio)),
                                            fecha = fecha,
                                            insumo_Desc = "DÍA TRABAJADO",
                                            poliza = "--",
                                            areaCuenta = corte ? ccPPG.areaCuenta : ccPPG.areaCuenta + " " + ccPPG.descripcion,
                                            referencia = ccPPG.descripcion,
                                            division = divisionDetalle == null ? "SIN DIVISION" : divisionDetalle.division.division
                                        };
                                        pendientesGenerar.Add(auxItemRentabilidadPPG);
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                        //}
                    }
                }
            }
            pendientesGenerar = pendientesGenerar.GroupBy(x => new { x.noEco, x.areaCuenta, x.fecha, x.division }).Select(x => new RentabilidadDTO
            {
                noEco = x.Key.noEco,
                cta = 1,
                tipoInsumo = x.Max(y => y.tipoInsumo),
                tipoInsumo_Desc = x.Max(y => y.tipoInsumo_Desc),
                grupoInsumo = x.Max(y => y.grupoInsumo),
                grupoInsumo_Desc = x.Max(y => y.grupoInsumo_Desc),
                tipo_mov = 3,
                importe = x.Sum(y => y.importe),
                fecha = x.Key.fecha,
                insumo_Desc = x.Max(y => y.insumo_Desc),
                poliza = "--",
                areaCuenta = x.Max(y => y.areaCuenta),
                referencia = x.Max(y => y.referencia),
                division = x.Key.division
            }).ToList();
            //var dias = pendientesGenerar.GroupBy(x => x.fecha).Select(x =>  new{ fecha = x.Key, importe = x.Sum(y => y.importe)}).ToList();
            return pendientesGenerar;
        }

        public List<RentabilidadDTO> getLstKubrixIngresosPendientesGenerar(BusqKubrixDTO busq, int usuarioID, bool corte)
        {
            List<RentabilidadDTO> pendientesGenerar = new List<RentabilidadDTO>();
            List<DateTime> diasTipoDeCambio = new List<DateTime>();
            List<tblM_KBDivisionDetalle> divisiones = getDivisionesDetalle();
            var auxConciliacionesAutorizadasID = _context.tblM_AutorizaConciliacionHorometros.Where(x => x.pendienteAdmin == 1 && x.pendienteDirector == 1 && x.pendienteGerente == 1).Select(x => x.conciliacionID);
            var centrosCostosEnkontrol = getListaCCByUsuario(usuarioID, 1).Select(x => x.Value).ToList();
            int numDiaActual = DateTime.Today.Day;
            DateTime /*diaMinimo = numDiaActual >= 15 ? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 16) : new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            if (diaMinimo < busq.fechaInicio)*/ diaMinimo = busq.fechaInicio;
            var diaInicioTC = diaMinimo;
            while (diaInicioTC <= busq.fechaFin)
            {
                diasTipoDeCambio.Add(diaInicioTC);
                diaInicioTC = diaInicioTC.AddDays(1);
            }
            var tipoCambio = getDolarDias(diasTipoDeCambio);
            List<int> auxModelos = new List<int>();
            if (busq.lstModelo != null) auxModelos = busq.lstModelo;

            List<string> auxObras = new List<string>();
            if (busq.obra != null) auxObras = busq.obra;
            var ccAConsiderar = new List<string>(auxObras);
            var ccSIGOPLAN = _context.tblP_CC.Select(x => x.areaCuenta).ToList();
            ccAConsiderar.AddRange(centrosCostosEnkontrol.Where(x => !ccSIGOPLAN.Contains(x)));

            var maquinasPPG = _context.tblM_CatMaquina
            .Where(x => x.CargoEmpresa == 1
                && x.noEconomico != null
                && (auxObras.Count() > 0 ? (ccAConsiderar.Contains(x.centro_costos)) : true)
                && (busq.economico == null ? true : x.noEconomico == busq.economico)
                && (auxModelos.Count() > 0 ? auxModelos.Contains(x.modeloEquipoID) : true))
            .GroupBy(x => x.noEconomico, (key, g) => g.OrderByDescending(e => e.id).FirstOrDefault()).Select(x => new
            {
                id = x.id,
                modeloEquipoID = x.modeloEquipoID,
                centro_costos = x.centro_costos,
                noEconomico = x.noEconomico,
                fechaAdquisicion = x.fechaAdquisicion
            }).ToList();
            List<string> strMaquinasPPG = maquinasPPG.Select(x => x.noEconomico).Distinct().ToList();
            List<int> modelosMaquinasPPG = maquinasPPG.Select(x => x.modeloEquipoID).Distinct().ToList();
            List<string> ccString = maquinasPPG.Select(x => x.centro_costos).Distinct().ToList();
            var ccsPPG = _context.tblP_CC.Where(x => ccString.Contains(x.areaCuenta) && x.estatus).Select(x => new
            {
                id = x.id,
                areaCuenta = x.areaCuenta,
                descripcion = x.descripcion
            }).ToList();
            List<int> ccIDsPPG = ccsPPG.Select(x => x.id).ToList();
            var encabezadosCaratula = _context.tblM_EncCaratula.Where(x => ccIDsPPG.Contains(x.ccID)).GroupBy(x => x.ccID, (key, g) => g.OrderByDescending(e => e.id).FirstOrDefault()).Select(x => new
            {
                id = x.id,
                ccID = x.ccID,
                isActivo = x.isActivo,
                moneda = x.moneda
            }).ToList();
            List<int> encabezadosIDs = encabezadosCaratula.Select(x => x.id).ToList();
            var caratulasDetalles = _context.tblM_CapCaratula.Where(x =>
                encabezadosIDs.Contains(x.idCaratula)
                && x.activo && modelosMaquinasPPG.Contains(x.idModelo)
                ).Select(x => new
                {
                    id = x.id,
                    idCaratula = x.idCaratula,
                    idModelo = x.idModelo,
                    unidad = x.unidad,
                    costo = x.costo
                }).ToList();
            var modelosCostoHora = caratulasDetalles.Where(x => x.unidad == 1).Select(x => x.idModelo).Distinct().ToList();
            var modelosCostoDiaAux = caratulasDetalles.Where(x => x.unidad == 2).Select(x => x.idModelo).Distinct().ToList();
            var maquinasCostoHora = maquinasPPG.Where(x => modelosCostoHora.Contains(x.modeloEquipoID) && x.noEconomico != null).Select(x => x.noEconomico).ToList();
            var maquinasCostoDia = maquinasPPG.Where(x => modelosCostoDiaAux.Contains(x.modeloEquipoID) && x.noEconomico != null).Select(x => x.noEconomico).ToList();
            var auxHorometros = busq.sscta == 2 ? null : _context.tblM_CapHorometro.Where(x => maquinasCostoHora.Contains(x.Economico) && x.Fecha >= diaMinimo).Select(x => new
            {
                id = x.id,
                Economico = x.Economico,
                Fecha = x.Fecha,
                HorasTrabajo = x.HorasTrabajo
            }).ToList();

            foreach (var maquina in maquinasPPG)
            {
                var ccPPG = ccsPPG.Where(x => x.areaCuenta == maquina.centro_costos).OrderByDescending(x => x.id).FirstOrDefault();
                if (ccPPG != null)
                {
                    var auxEncCaratula = encabezadosCaratula.Where(x => x.ccID == ccPPG.id && x.isActivo).OrderByDescending(x => x.id).FirstOrDefault();
                    if (auxEncCaratula != null)
                    {
                        var caratulaPPG = caratulasDetalles.Where(x => x.idCaratula == auxEncCaratula.id && x.idModelo == maquina.modeloEquipoID).OrderByDescending(x => x.id).FirstOrDefault();

                        if (caratulaPPG != null && caratulaPPG.costo != 0)
                        {
                            switch (caratulaPPG.unidad)
                            {
                                case 1:
                                    if (busq.sscta != 2)
                                    {
                                        var auxFechaInicio = busq.fechaInicio;
                                        if (auxFechaInicio < maquina.fechaAdquisicion) auxFechaInicio = maquina.fechaAdquisicion;
                                        var conciliacionDet = _context.tblM_CapConciliacionHorometros.Where(x => x.economico == maquina.noEconomico).Select(x => x.idEncCaratula).ToList();
                                        var conciliacion = _context.tblM_CapEncConciliacionHorometros.Where(x => conciliacionDet.Contains(x.id) && auxConciliacionesAutorizadasID.Contains(x.id)).OrderByDescending(x => x.fechaFin).FirstOrDefault();
                                        var divisionDetalle = divisiones.FirstOrDefault(x => (x.ac.areaCuenta) == ccPPG.areaCuenta);

                                        var auxRentabilidadPPG = auxHorometros.Where(x => x.Economico == maquina.noEconomico && (conciliacion == null ? x.Fecha >= diaMinimo : (conciliacion.fechaFin.AddDays(1) >= auxFechaInicio ? x.Fecha > conciliacion.fechaFin : x.Fecha >= auxFechaInicio)) && x.Fecha <= busq.fechaFin).ToList()
                                        .Select(x =>
                                        {
                                            var auxTipoCambio = tipoCambio.FirstOrDefault(y => y.fecha.Date == x.Fecha.Date);
                                            return new RentabilidadDTO
                                            {
                                                noEco = x.Economico,
                                                cta = 1,
                                                tipoInsumo = "1-2",
                                                tipoInsumo_Desc = "CONCILIACIONES POR GENERAR",
                                                grupoInsumo = "1-2-1",
                                                grupoInsumo_Desc = "HORAS TRABAJADAS",
                                                tipo_mov = 3,
                                                importe = (auxEncCaratula.moneda == 1 ? x.HorasTrabajo * caratulaPPG.costo : (auxTipoCambio == null ? x.HorasTrabajo * caratulaPPG.costo * (decimal)19.80 : x.HorasTrabajo * caratulaPPG.costo * auxTipoCambio.tipo_cambio)),
                                                fecha = x.Fecha.Date > busq.fechaFin ? conciliacion.fechaFin : x.Fecha,
                                                insumo_Desc = "HORAS TRABAJADAS",
                                                poliza = x.id.ToString().PadLeft(10, '0'),
                                                areaCuenta = corte ? ccPPG.areaCuenta : ccPPG.areaCuenta + " " + ccPPG.descripcion,
                                                referencia = ccPPG.descripcion,
                                                division = divisionDetalle == null ? "SIN DIVISION" : divisionDetalle.division.division
                                            };
                                        }).ToList();
                                        pendientesGenerar.AddRange(auxRentabilidadPPG);
                                    }
                                    break;
                                case 2:
                                    if (busq.sscta != 1)
                                    {
                                        var conciliacionDet = _context.tblM_CapConciliacionHorometros.Where(x => x.economico == maquina.noEconomico).Select(x => x.idEncCaratula).ToList();
                                        var conciliacion = _context.tblM_CapEncConciliacionHorometros.Where(x => conciliacionDet.Contains(x.id) && auxConciliacionesAutorizadasID.Contains(x.id)).OrderByDescending(x => x.fechaFin).FirstOrDefault();

                                        var auxFechaInicio = busq.fechaInicio;
                                        if (auxFechaInicio < maquina.fechaAdquisicion) auxFechaInicio = maquina.fechaAdquisicion;
                                        List<DateTime> auxDiasTrabajados = new List<DateTime>();
                                        DateTime auxDiaMinimoTrabajado = conciliacion == null ? auxFechaInicio : (conciliacion.fechaFin.AddDays(1) >= auxFechaInicio ? conciliacion.fechaFin.AddDays(1) : auxFechaInicio);
                                        while (auxDiaMinimoTrabajado.Date <= busq.fechaFin.Date)
                                        {
                                            auxDiasTrabajados.Add(auxDiaMinimoTrabajado);
                                            auxDiaMinimoTrabajado = auxDiaMinimoTrabajado.AddDays(1);
                                        }
                                        foreach (var fecha in auxDiasTrabajados)
                                        {
                                            var auxTipoCambio = tipoCambio.FirstOrDefault(y => y.fecha.Date == fecha.Date);
                                            var divisionDetalle = divisiones.FirstOrDefault(x => (x.ac.areaCuenta) == ccPPG.areaCuenta);
                                            RentabilidadDTO auxItemRentabilidadPPG = new RentabilidadDTO
                                            {
                                                noEco = maquina.noEconomico,
                                                cta = 1,
                                                tipoInsumo = "1-2",
                                                tipoInsumo_Desc = "CONCILIACIONES POR GENERAR",
                                                grupoInsumo = "1-2-2",
                                                grupoInsumo_Desc = "DIAS TRABAJADOS",
                                                tipo_mov = 3,
                                                importe = (auxEncCaratula.moneda == 1 ? caratulaPPG.costo : (auxTipoCambio == null ? caratulaPPG.costo * (decimal)19.80 : caratulaPPG.costo * auxTipoCambio.tipo_cambio)),
                                                fecha = fecha,
                                                insumo_Desc = "DÍA TRABAJADO",
                                                poliza = "--",
                                                areaCuenta = corte ? ccPPG.areaCuenta : ccPPG.areaCuenta + " " + ccPPG.descripcion,
                                                referencia = ccPPG.descripcion,
                                                division = divisionDetalle == null ? "SIN DIVISION" : divisionDetalle.division.division
                                            };
                                            pendientesGenerar.Add(auxItemRentabilidadPPG);
                                        }
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
            pendientesGenerar = pendientesGenerar.GroupBy(x => new { x.noEco, x.areaCuenta, x.fecha, x.division }).Select(x => new RentabilidadDTO
            {
                noEco = x.Key.noEco,
                cta = 1,
                tipoInsumo = x.Max(y => y.tipoInsumo),
                tipoInsumo_Desc = x.Max(y => y.tipoInsumo_Desc),
                grupoInsumo = x.Max(y => y.grupoInsumo),
                grupoInsumo_Desc = x.Max(y => y.grupoInsumo_Desc),
                tipo_mov = 3,
                importe = x.Sum(y => y.importe),
                fecha = x.Key.fecha,
                insumo_Desc = x.Max(y => y.insumo_Desc),
                poliza = "--",
                areaCuenta = x.Max(y => y.areaCuenta),
                referencia = x.Max(y => y.referencia),
                division = x.Key.division
            }).ToList();
            //var dias = pendientesGenerar.GroupBy(x => x.fecha).Select(x =>  new{ fecha = x.Key, importe = x.Sum(y => y.importe)}).ToList();
            return pendientesGenerar;
        }

        private List<Core.DTO.Contabilidad.TipoCambioDllDTO> getDolarDias(List<DateTime> dias)
        {
            try
            {
                var lst = new List<OdbcParameterDTO>();
                if (dias.Count() > 0)
                {
                    lst.AddRange(dias.Select(s => new OdbcParameterDTO() { nombre = "fecha", tipo = OdbcType.DateTime, valor = s }));
                    var odbc = new OdbcConsultaDTO()
                    {
                        consulta = string.Format("SELECT fecha, tipo_cambio FROM tipo_cambio WHERE fecha in {0}", dias.ToParamInValue()),
                        parametros = lst
                    };
                    var tipoCambio = _contextEnkontrol.Select<Core.DTO.Contabilidad.TipoCambioDllDTO>(EnkontrolAmbienteEnum.Prod, odbc);
                    return tipoCambio;
                }
                else { return new List<Core.DTO.Contabilidad.TipoCambioDllDTO>(); }
            }
            catch (Exception) { return new List<Core.DTO.Contabilidad.TipoCambioDllDTO>(); }
        }

        public List<string> getFletesActivos()
        {
            List<string> data = new List<string>();
            var fletes = _context.tblM_KBFletes.Where(x => x.estatus && x.noEconomico != null).ToList();
            if (fletes.Count() > 0) data = fletes.Select(x => x.noEconomico).ToList();
            return data;
        }

        public List<tblM_KBDivision> getDivisiones()
        {
            List<tblM_KBDivision> data = new List<tblM_KBDivision>();
            data = _context.tblM_KBDivision.Where(x => x.estatus).ToList();
            return data;
        }

        public List<tblM_KBUsuarioResponsable> getResponsabilesAC(int usuarioID)
        {
            var responsable = checkResponsable(-1, usuarioID);
            List<tblM_KBUsuarioResponsable> data = new List<tblM_KBUsuarioResponsable>();
            data = _context.tblM_KBUsuarioResponsable.Where(x => x.estatus && (responsable ? x.usuarioResponsableID == usuarioID : true)).ToList();
            return data;
        }

        public List<string> getACDivision(int divisionID)
        {
            List<string> data = new List<string>();
            var detalles = _context.tblM_KBDivisionDetalle.Where(x => x.divisionID == divisionID && x.estatus && x.ac.areaCuenta != null).ToList();
            if (detalles.Count() > 0) data = detalles.Select(x => x.ac.areaCuenta).ToList();
            return data;
        }

        public List<string> getACResponsable(int responsableID)
        {
            List<string> data = new List<string>();
            var usuarioResponsable = _context.tblM_KBUsuarioResponsable.FirstOrDefault(x => x.usuarioResponsableID == responsableID);
            if (usuarioResponsable != null)
            {
                var detalles = _context.tblM_KBAreaCuentaResponsable.Where(x => x.usuarioResponsableID == usuarioResponsable.id && x.estatus).ToList();
                if (detalles.Count() > 0) data = detalles.Select(x => x.areaCuenta.areaCuenta).ToList();
            }
            return data;
        }

        public bool checkResponsable(int usuarioID, int responsableID)
        {
            var responsables = _context.tblM_KBUsuarioResponsable.Where(x => x.estatus).Select(x => x.usuarioResponsableID).ToList();
            if (responsables.Contains(usuarioID) || responsables.Contains(responsableID)) return true;
            else return false;
        }

        public bool guardarLstCC(List<string> listaCC)
        {
            try
            {
                List<tblM_KBCatCC> guardar = new List<tblM_KBCatCC>();
                List<tblM_KBCatCC> borrar = _context.tblM_KBCatCC.ToList();
                List<string> borrarAC = borrar.Select(x => x.areaCuenta).ToList();
                tblM_KBCatCC auxCC = new tblM_KBCatCC();

                foreach (var item in listaCC)
                {
                    auxCC = new tblM_KBCatCC();
                    auxCC.areaCuenta = item;
                    guardar.Add(auxCC);
                }
                borrar = borrar.Where(x => !listaCC.Contains(x.areaCuenta)).ToList();
                guardar = guardar.Where(x => !borrarAC.Contains(x.areaCuenta)).ToList();
                _context.tblM_KBCatCC.RemoveRange(borrar);
                _context.tblM_KBCatCC.AddRange(guardar);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        #endregion

        #region Corte

        public int guardarCorteArrendadora(int usuario, int tipoCorte)
        {
            try
            {
                var tablaCC = _context.tblP_CC.Where(x => x.id > 0).Select(x => new
                {
                    id = x.id,
                    descripcion = x.descripcion
                }).ToList();  
                //-->Lista final para datos de corte
                List<tblM_KBCorteDet> lst = new List<tblM_KBCorteDet>();
                //-->Objetos para creación de consulta
                var lstParametros = new List<OdbcParameterDTO>();
                var odbc = new OdbcConsultaDTO(); //-->Consulta principal
                var odbcVirtual = new OdbcConsultaDTO(); //-->Consulta a empresa virtual
                tblM_KBCorte corteAnterior = new tblM_KBCorte();
                //-->Identificación de fecha de corte
                DateTime fechaCorte = DateTime.Today.AddDays(1);
                //-->tipo Corte: [0 -> Semanal], [1 - Mensual], [2 - Anual], [10 - Flujo]
                if (tipoCorte == 0)
                {
                    while (fechaCorte.DayOfWeek != DayOfWeek.Wednesday)
                        fechaCorte = fechaCorte.AddDays(-1);
                }
                else
                {
                    if (tipoCorte == 10)
                    {
                        while (fechaCorte.DayOfWeek != DayOfWeek.Tuesday)
                            fechaCorte = fechaCorte.AddDays(-1);
                    }
                    else
                    {
                        fechaCorte = new DateTime(fechaCorte.Year, fechaCorte.Month, 1);
                        fechaCorte = fechaCorte.AddDays(-1);
                    }
                }
                //--> Fecha especial de guardado
                //fechaCorte = new DateTime(2023, 08, 31);
                //-->Identificación de corte anterior para jalado de estimados
                var cortesAnteriores = _context.tblM_KBCorte.Where(x => x.fechaCorte < fechaCorte && (tipoCorte == 10 ? x.tipo == 0 : x.tipo == tipoCorte)).ToList();
                if (cortesAnteriores.Count > 0) corteAnterior = cortesAnteriores.OrderByDescending(x => x.fechaCorte).FirstOrDefault();
                //->Verificar si ya existe el corte especificado
                var corteCerrado = _context.tblM_KBCorte.Where(x => x.fechaCorte == fechaCorte && x.tipo == tipoCorte && x.guardadoArrendadora).FirstOrDefault();
                if (corteCerrado == null)
                {
                    //-->Determinación de facturas de conciliación que aplican
                    var auxFacturasConcEntrada = _context.tblM_CapEncConciliacionHorometros.Where(x => x.estatus == 1 && x.facturado && x.fechaFin <= fechaCorte).ToList();
                    List<string> auxFacturasEntrada = new List<string>();
                    foreach (var item in auxFacturasConcEntrada) { auxFacturasEntrada.AddRange(JsonConvert.DeserializeObject<List<string>>(item.factura)); }
                    auxFacturasEntrada = auxFacturasEntrada.Distinct().ToList();
                    auxFacturasEntrada.Remove("0");
                    if (auxFacturasEntrada.Count() < 1) auxFacturasEntrada.Add("-1");
                    //-->Definición de parametros
                    lstParametros = new List<OdbcParameterDTO>();
                    lstParametros.Add(new OdbcParameterDTO() { nombre = "fechapol", tipo = OdbcType.DateTime, valor = fechaCorte });
                    lstParametros.AddRange(auxFacturasEntrada.Select(s => new OdbcParameterDTO() { nombre = "referencia", tipo = OdbcType.VarChar, valor = s }));
                    //-->Query principal arrendadora
                    odbc = new OdbcConsultaDTO()
                    {
                        consulta = queryKubrixCorteArrendadora(auxFacturasEntrada),
                        parametros = lstParametros
                    };
                    //-->Query empresa virtual
                    odbcVirtual = new OdbcConsultaDTO()
                    {
                        consulta = queryKubrixCorteVirtual(),
                        parametros = lstParametros
                    };

                    //-->Definición de conciliaciones con factura para adecuación de ingesos
                    var conciliaciones = _context.tblM_CapEncConciliacionHorometros.ToList().Select(x =>
                    {
                        var auxCC = tablaCC.FirstOrDefault(y => y.id == x.centroCostosID);
                        return new ConciliacionKubrixDTO
                        {
                            id = x.id,
                            cc = auxCC == null ? "S/AC" : auxCC.descripcion,
                            fechaInicio = x.fechaInicio,
                            fechaFin = x.fechaFin,
                            estatus = x.estatus,
                            factura = x.factura
                        };
                    }).ToList();

                    //-->Ingresos estimados manuales
                    List<tblM_KBCorteDet> ingresosEstimadosManuales = new List<tblM_KBCorteDet>();
                    if (corteAnterior != null)
                    {
                        ingresosEstimadosManuales = _context.tblM_KBCorteDet.Where(x => x.corteID == corteAnterior.id && x.cuenta == "1-2-3").ToList();
                    }
                    //-->Ingresos pendientes por general manuales
                    List<tblM_KBCorteDet> pendientesGenerarManuales = new List<tblM_KBCorteDet>();
                    if (corteAnterior != null)
                    {
                        pendientesGenerarManuales = _context.tblM_KBCorteDet.Where(x => x.corteID == corteAnterior.id && x.cuenta == "1-3-2").ToList();
                    }
                    //-->Costos Estimados
                    List<tblM_KBCorteDet> costoEstimado = new List<tblM_KBCorteDet>();
                    if (corteAnterior != null)
                    {
                        costoEstimado = _context.tblM_KBCorteDet.Where(x => x.corteID == corteAnterior.id && x.cuenta == "1-4-0").ToList();
                    }
                    //-->Select principal
                    lst = _contextEnkontrolRentabilidad.SelectCorte(EnkontrolEnum.ArrenProd, odbc, conciliaciones);
                    //-->Select empresa virtual
                    List<tblM_KBCorteDet> lstVirtual = _contextEnkontrolRentabilidad.SelectCorte(EnkontrolEnum.ArrenVirtual, odbcVirtual, conciliaciones);

                    lstVirtual.ForEach(x => x.empresa = 12);

                    lst.AddRange(lstVirtual);
                    //-->Definición de objeto búsqueda para calculo de estimados
                    BusqKubrixDTO busq = new BusqKubrixDTO();
                    busq.fechaInicio = new DateTime();
                    busq.fechaFin = fechaCorte;
                    busq.obra = new List<string>();
                    //-->Ingresos Estimados
                    var IngresosEstimados = getLstKubrixIngresosEstimacion(busq, true).Select(x => new tblM_KBCorteDet
                    {
                        poliza = x.poliza,
                        cuenta = x.grupoInsumo,
                        concepto = x.insumo_Desc,
                        monto = x.importe,
                        cc = x.noEco,
                        areaCuenta = x.areaCuenta,
                        fechapol = x.fecha,
                        referencia = x.referencia,
                        linea = 0
                    }).ToList();

                    //-->Ingresos pendientes por generar
                    //var pendientesGenerar = getLstKubrixIngresosPendientesGenerar(busq, -1, true).Select(x => new tblM_KBCorteDet
                    //{
                    //    poliza = x.poliza,
                    //    cuenta = x.grupoInsumo,
                    //    concepto = x.insumo_Desc,
                    //    monto = x.importe,
                    //    cc = x.noEco,
                    //    areaCuenta = x.areaCuenta,
                    //    fechapol = x.fecha,
                    //    referencia = x.referencia,
                    //    linea = 0
                    //}).ToList();

                    var pendientesGenerarEspecial = getLstKubrixIngresosPendientesGenerarEspecial(busq, -1, true).Select(x => new tblM_KBCorteDet
                    {
                        poliza = x.poliza,
                        cuenta = x.grupoInsumo,
                        concepto = x.insumo_Desc,
                        monto = x.importe,
                        cc = x.noEco,
                        areaCuenta = x.areaCuenta,
                        fechapol = x.fecha,
                        referencia = x.referencia,
                        linea = 0
                    }).ToList();

                    //-->Se agregan a la lista principal
                    lst.AddRange(IngresosEstimados);
                    lst.AddRange(ingresosEstimadosManuales);
                    //lst.AddRange(pendientesGenerar);
                    lst.AddRange(pendientesGenerarManuales);
                    lst.AddRange(costoEstimado);
                    lst.AddRange(pendientesGenerarEspecial);
                    //-->Especificación de tipo de maquinaria
                    GetTipoMaquinaCorte(lst);
                    //-->Guardado(SQLBulk)
                    var corteID = saveCorte(lst, usuario, fechaCorte, tipoCorte, 2);
                    return corteID;
                }
                else { return -1; }
            }
            catch (Exception o_O)
            {
                return 0;
            }
        }

        public int guardarCorteConstruplan(int usuario, int tipoCorte)
        {
            try
            {
                List<tblM_KBCorteDet> lst = new List<tblM_KBCorteDet>();
                List<tblM_KBCorteDet> lstVirtual = new List<tblM_KBCorteDet>();
                List<tblM_KBCorteDet> lstEICI = new List<tblM_KBCorteDet>();
                var lstParametros = new List<OdbcParameterDTO>();
                //List<OdbcConsultaDTO> lstOdbc = new List<OdbcConsultaDTO>();
                tblM_KBCorte corteAnterior = new tblM_KBCorte();

                var odbc = new OdbcConsultaDTO();
                var odbcVirtual = new OdbcConsultaDTO();
                var odbcVirtualeici = new OdbcConsultaDTO();
                var fechaInicio = new DateTime(2019, 1, 1);
                var fechaFin = new DateTime(2020, 1, 1);

                DateTime fechaCorte = DateTime.Today.AddDays(1);
                //DateTime fechaCorte = DateTime.Today.AddMonths(-1);
                if (tipoCorte == 0)
                {
                    while (fechaCorte.DayOfWeek != DayOfWeek.Wednesday)
                        fechaCorte = fechaCorte.AddDays(-1);
                }
                else
                {
                    if (tipoCorte == 10)
                    {
                        while (fechaCorte.DayOfWeek != DayOfWeek.Tuesday)
                            fechaCorte = fechaCorte.AddDays(-1);
                    }
                    else
                    {
                        fechaCorte = new DateTime(fechaCorte.Year, fechaCorte.Month, 1);
                        fechaCorte = fechaCorte.AddDays(-1);
                    }
                }
                //--> Fecha especial de guardado
                //fechaCorte = new DateTime(2023, 01, 31);

                var cortesAnteriores = tipoCorte == 1 ?
                    _context.tblM_KBCorte.Where(x => x.fechaCorte <= fechaCorte && x.tipo == tipoCorte).ToList()
                    : _context.tblM_KBCorte.Where(x => x.fechaCorte <= fechaCorte && x.tipo == 0).ToList();
                if (cortesAnteriores.Count > 0) corteAnterior = cortesAnteriores.OrderByDescending(x => x.fechaCorte).FirstOrDefault();

                var corteCerrado = _context.tblM_KBCorte.Where(x => x.fechaCorte == fechaCorte && x.tipo == tipoCorte && x.guardadoConstruplan).FirstOrDefault();
                if (corteCerrado == null)
                {
                    //while (fechaFin < fechaCorte) 
                    //{
                    //    lstParametros = new List<OdbcParameterDTO>();
                    //    lstParametros.Add(new OdbcParameterDTO() { nombre = "fechapol", tipo = OdbcType.DateTime, valor = fechaInicio });
                    //    lstParametros.Add(new OdbcParameterDTO() { nombre = "fechapol", tipo = OdbcType.DateTime, valor = fechaFin });

                    //    odbc = new OdbcConsultaDTO()
                    //    {
                    //        consulta = queryKubrixCorteConstruplan(true),
                    //        parametros = lstParametros
                    //    };
                    //    fechaInicio = fechaInicio.AddYears(1);
                    //    fechaFin = fechaInicio.AddYears(1);
                    //    lstOdbc.Add(odbc);
                    //}
                    lstParametros = new List<OdbcParameterDTO>();
                    lstParametros.Add(new OdbcParameterDTO() { nombre = "fechapol", tipo = OdbcType.DateTime, valor = fechaInicio });
                    lstParametros.Add(new OdbcParameterDTO() { nombre = "fechapol", tipo = OdbcType.DateTime, valor = fechaCorte });

                    odbc = new OdbcConsultaDTO()
                    {
                        consulta = queryKubrixCorteConstruplan(true),
                        parametros = lstParametros
                    };
                    odbcVirtual = new OdbcConsultaDTO()
                    {
                        consulta = queryKubrixCorteCplanVirtual(true),
                        parametros = lstParametros
                    };
                    odbcVirtualeici = new OdbcConsultaDTO()
                    {
                        consulta = queryKubrixCorteEICIVirtual(true),
                        parametros = lstParametros
                    };

                    fechaInicio = fechaInicio.AddYears(1);
                    fechaFin = fechaInicio.AddYears(1);
                    //lstOdbc.Add(odbc);


                    lst = _contextEnkontrolRentabilidad.SelectCorte(EnkontrolEnum.CplanProd, odbc, new List<ConciliacionKubrixDTO>());
                    lstVirtual = _contextEnkontrolRentabilidad.SelectCorte(EnkontrolEnum.CplanVirtual, odbcVirtual, new List<ConciliacionKubrixDTO>());
                    lstEICI = _contextEnkontrolRentabilidad.SelectCorte(EnkontrolEnum.CplanEici, odbcVirtualeici, new List<ConciliacionKubrixDTO>());

                    lstVirtual.ForEach(x => x.empresa = 10);
                    lstEICI.ForEach(x => x.empresa = 4);

                    List<tblM_KBCorteDet> ingresosEstimadosManuales = new List<tblM_KBCorteDet>();
                    if (corteAnterior != null)
                    {
                        ingresosEstimadosManuales = _context.tblM_KBCorteDet.Where(x => x.corteID == corteAnterior.id && x.cuenta == "1-1-0").ToList();
                    }
                    List<tblM_KBCorteDet> pendientesGenerarManuales = new List<tblM_KBCorteDet>();
                    if (corteAnterior != null)
                    {
                        pendientesGenerarManuales = _context.tblM_KBCorteDet.Where(x => x.corteID == corteAnterior.id && x.cuenta == "1-2-3").ToList();
                    }
                    List<tblM_KBCorteDet> costoEstimado = new List<tblM_KBCorteDet>();
                    if (corteAnterior != null)
                    {
                        costoEstimado = _context.tblM_KBCorteDet.Where(x => x.corteID == corteAnterior.id && x.cuenta == "1-4-0").ToList();
                    }

                    lst.AddRange(lstEICI);
                    lst.AddRange(lstVirtual);
                    lst.AddRange(ingresosEstimadosManuales);
                    lst.AddRange(pendientesGenerarManuales);
                    lst.AddRange(costoEstimado);

                    var tablaCC = _context.tblP_CC.ToList();

                    foreach (var item in lst)
                    {
                        if (item.areaCuenta.Length > 7) item.areaCuenta = "-1";
                        var auxAreaCuenta = tablaCC.FirstOrDefault(x => x.cc.Trim().ToUpper() == item.areaCuenta.Trim().ToUpper());
                        if (auxAreaCuenta != null) item.areaCuenta = auxAreaCuenta.areaCuenta;
                    }


                    var corteID = saveCorte(lst, usuario, fechaCorte, tipoCorte, 1);

                    return corteID;
                }
                else { return -1; }
            }
            catch (Exception o_O)
            {
                return 0;
            }
        }

        string queryKubrixCorteArrendadora(List<string> auxFacturasEntrada)
        {
            return string.Format(
                        @"SELECT 
                            CAST(year as varchar(4)) + '-' + CAST(mes as varchar(2)) + '-' + tp + '-' + CAST(poliza as varchar(6)) as poliza, CAST(cta as varchar(6))  + '-' + CAST(scta as varchar(6)) + '-' + CAST(sscta as varchar(6)) as cuenta, concepto, sum(monto) as monto, ISNULL(cc, 'INDEFINIDO') as cc, CAST(area as varchar(6)) + '-' + CAST(cuenta_oc as varchar(6)) as areaCuenta, fechapol, referencia, linea
                        FROM 
                        (	
                            SELECT 
                                a.year, a.mes, a.tp, a.poliza,
                                a.cta as cta, a.scta as scta, a.sscta as sscta, a.concepto as concepto, SUM(a.monto) as monto,
                                d.descripcion as cc, a.area as area, a.cuenta_oc as cuenta_oc, e.fechapol as fechapol, a.referencia as referencia, a.linea as linea
                            FROM 
                                (SELECT 
                                    referencia, cta, poliza, linea, concepto, scta, monto, tp, year, mes,  sscta, cc, area, cuenta_oc 
                                FROM 
                                    sc_movpol 
                                WHERE 
                                    cta in (5000, 4000, 5280, 5900, 4901, 4900, 5901)
                                ) a 
                            LEFT JOIN 
                                (SELECT 
                                    poliza, tp, year, mes, fechapol 
                                FROM 
                                    sc_polizas
                                WHERE
                                    status = 'A'
                                ) e
                            ON 
                                a.poliza = e.poliza and a.tp = e.tp and a.year = e.year and a.mes = e.mes
                            LEFT JOIN 
                                (SELECT 
                                    cc, descripcion 
                                FROM 
                                    cc
                                ) as d
                            ON 
                                a.cc = d.cc
                            WHERE
                                fechapol is not null AND (fechapol <= ? OR (cta = 4000 AND referencia IN {0}))
                            GROUP BY 
                                a.year, a.mes, a.tp, a.poliza, a.cta, a.scta, a.sscta, a.concepto, d.descripcion, a.area, a.cuenta_oc, e.fechapol, a.referencia, a.linea
                        ) x  
                        GROUP BY 
                            x.year, x.mes, x.tp, x.poliza, x.cta, x.scta, x.sscta, x.concepto, x.cc, x.fechapol, x.area, x.cuenta_oc, x.referencia, x.linea"
                        , auxFacturasEntrada.ToParamInValue()
                    );
        }

        string queryKubrixCorteConstruplan(bool banderaLimiteFecha)
        {
            return string.Format(
                        @"SELECT 
                            CAST(year as varchar(4)) + '-' + CAST(mes as varchar(2)) + '-' + tp + '-' + CAST(poliza as varchar(6)) as poliza, CAST(cta as varchar(6))  + '-' + CAST(scta as varchar(6)) + '-' + CAST(sscta as varchar(6)) as cuenta, concepto, sum(monto) as monto, referencia as cc, areaCuenta as areaCuenta, fechapol, referencia, linea
                        FROM 
                        (	
                            SELECT 
                                a.year, a.mes, a.tp, a.poliza,
                                a.cta as cta, a.scta as scta, a.sscta as sscta, a.concepto as concepto, SUM(a.monto) as monto,
                                a.cc as areaCuenta, e.fechapol as fechapol, a.referencia as referencia, a.linea as linea
                            FROM 
                                (SELECT 
                                    referencia, cta, poliza, linea, concepto, scta, monto, tp, year, mes,  sscta, cc, area, cuenta_oc 
                                FROM 
                                    sc_movpol 
                                WHERE 
                                    cta in (5000, 4000, 5280, 5900, 4901, 4900, 5901)                                    
                                ) a 
                            LEFT JOIN 
                                (SELECT 
                                    poliza, tp, year, mes, fechapol 
                                FROM 
                                    sc_polizas
                                WHERE
                                    status = 'A'
                                ) e
                            ON 
                                a.poliza = e.poliza and a.tp = e.tp and a.year = e.year and a.mes = e.mes
                            WHERE
                                fechapol is not null AND fechapol >= ? {0}
                            GROUP BY 
                                a.year, a.mes, a.tp, a.poliza, a.cta, a.scta, a.sscta, a.concepto, a.cc, a.area, a.cuenta_oc, e.fechapol, a.referencia, a.linea
                        ) x  
                        GROUP BY 
                            x.year, x.mes, x.tp, x.poliza, x.cta, x.scta, x.sscta, x.concepto, x.areaCuenta, x.fechapol, x.referencia, x.linea", (banderaLimiteFecha ? "AND fechapol < ?" : "")
                    );
            //(cta = 5000 AND scta = 10 AND cc != '990') OR (cta = 5900 AND scta = 3) OR (cta = 5280 AND scta = 10 AND cc != '990')
        }

        string queryKubrixCorteVirtual()
        {
            return string.Format(
                        @"SELECT 
                            CAST(year as varchar(4)) + '-' + CAST(mes as varchar(2)) + '-' + tp + '-' + CAST(poliza as varchar(6)) as poliza, CAST(cta as varchar(6))  + '-' + CAST(scta as varchar(6)) + '-' + CAST(sscta as varchar(6)) as cuenta, concepto, sum(monto) as monto, ISNULL(cc, 'INDEFINIDO') as cc, areaCuenta, fechapol, referencia, linea
                        FROM 
                        (	
                            SELECT 
                                a.year, a.mes, a.tp, a.poliza,
                                a.cta as cta, a.scta as scta, a.sscta as sscta, a.concepto as concepto, SUM(a.monto) as monto,
                                d.descripcion as cc, e.fechapol as fechapol, a.referencia as referencia, a.linea as linea, (CASE WHEN area = 1 AND cuenta_oc = 9 THEN '1-9' ELSE '6-5' END) as areaCuenta
                            FROM 
                                (SELECT 
                                    referencia, cta, poliza, linea, concepto, scta, monto, tp, year, mes,  sscta, cc, area, cuenta_oc
                                FROM 
                                    sc_movpol 
                                WHERE 
                                    (cta IN (5000, 4000, 5280, 5900, 4901, 4900, 5901) and ((area!= 1 or cuenta_oc != 9) or (area is null or cuenta_oc is null)))
                                    OR (cta IN (4000, 4901, 4900) and (area= 1 and cuenta_oc = 9) )
                                ) a 
                            LEFT JOIN 
                                (SELECT 
                                    poliza, tp, year, mes, fechapol 
                                FROM 
                                    sc_polizas
                                WHERE
                                    status = 'A'
                                ) e
                            ON 
                                a.poliza = e.poliza and a.tp = e.tp and a.year = e.year and a.mes = e.mes
                            LEFT JOIN 
                                (SELECT 
                                    cc, descripcion 
                                FROM 
                                    cc
                                ) as d
                            ON 
                                a.cc = d.cc
                            WHERE
                                fechapol is not null AND fechapol <= ?
                            GROUP BY 
                                a.year, a.mes, a.tp, a.poliza, a.cta, a.scta, a.sscta, a.concepto, d.descripcion, e.fechapol, a.referencia, a.linea, a.area, a.cuenta_oc
                        ) x  
                        GROUP BY 
                            x.year, x.mes, x.tp, x.poliza, x.cta, x.scta, x.sscta, x.concepto, x.cc, x.fechapol, x.referencia, x.linea, x.areaCuenta"
                    );
        }

        string queryKubrixCorteCplanVirtual(bool banderaLimiteFecha)
        {
            return string.Format(
                        @"SELECT 
                            CAST(year as varchar(4)) + '-' + CAST(mes as varchar(2)) + '-' + tp + '-' + CAST(poliza as varchar(6)) as poliza, CAST(cta as varchar(6))  + '-' + CAST(scta as varchar(6)) + '-' + CAST(sscta as varchar(6)) as cuenta, concepto, sum(monto) as monto, 'N/A' as cc, areaCuenta as areaCuenta, fechapol, referencia, linea
                        FROM 
                        (	
                            SELECT 
                                a.year, a.mes, a.tp, a.poliza,
                                a.cta as cta, a.scta as scta, a.sscta as sscta, a.concepto as concepto, SUM(a.monto) as monto,
                                a.cc as areaCuenta, e.fechapol as fechapol, a.referencia as referencia, a.linea as linea
                            FROM 
                                (SELECT 
                                    referencia, cta, poliza, linea, concepto, scta, monto, tp, year, mes,  sscta, cc
                                FROM 
                                    sc_movpol 
                                WHERE 
                                    cta in (5000, 4000, 5280, 5900, 4901, 4900, 5901)                                    
                                ) a 
                            LEFT JOIN 
                                (SELECT 
                                    poliza, tp, year, mes, fechapol 
                                FROM 
                                    sc_polizas
                                ) e
                            ON 
                                a.poliza = e.poliza and a.tp = e.tp and a.year = e.year and a.mes = e.mes
                            WHERE
                                fechapol >= '2019-01-01'
                            GROUP BY 
                                a.year, a.mes, a.tp, a.poliza, a.cta, a.scta, a.sscta, a.concepto, a.cc, e.fechapol, a.referencia, a.linea
                        ) x  
                        GROUP BY 
                            x.year, x.mes, x.tp, x.poliza, x.cta, x.scta, x.sscta, x.concepto, x.areaCuenta, x.fechapol, x.referencia, x.linea", (banderaLimiteFecha ? "AND fechapol < ?" : "")
                    );
        }
        string queryKubrixCorteEICIVirtual(bool banderaLimiteFecha)
        {
            return string.Format(
                        @"SELECT 
                            CAST(year as varchar(4)) + '-' + CAST(mes as varchar(2)) + '-' + tp + '-' + CAST(poliza as varchar(6)) as poliza, CAST(cta as varchar(6))  + '-' + CAST(scta as varchar(6)) + '-' + CAST(sscta as varchar(6)) as cuenta, concepto, sum(monto) as monto, 'N/A' as cc, areaCuenta as areaCuenta, fechapol, referencia, linea
                        FROM 
                        (	
                            SELECT 
                                a.year, a.mes, a.tp, a.poliza,
                                a.cta as cta, a.scta as scta, a.sscta as sscta, a.concepto as concepto, SUM(a.monto) as monto,
                                a.cc as areaCuenta, e.fechapol as fechapol, a.referencia as referencia, a.linea as linea
                            FROM 
                                (SELECT 
                                    referencia, cta, poliza, linea, concepto, scta, monto, tp, year, mes,  sscta, cc
                                FROM 
                                    sc_movpol 
                                WHERE 
                                    cta in (5000, 4000, 5280, 5900, 4901, 4900, 5901)                                    
                                ) a 
                            LEFT JOIN 
                                (SELECT 
                                    poliza, tp, year, mes, fechapol 
                                FROM 
                                    sc_polizas
                                ) e
                            ON 
                                a.poliza = e.poliza and a.tp = e.tp and a.year = e.year and a.mes = e.mes
                            WHERE
                                fechapol >= '2019-01-01'
                            GROUP BY 
                                a.year, a.mes, a.tp, a.poliza, a.cta, a.scta, a.sscta, a.concepto, a.cc, e.fechapol, a.referencia, a.linea
                        ) x  
                        GROUP BY 
                            x.year, x.mes, x.tp, x.poliza, x.cta, x.scta, x.sscta, x.concepto, x.areaCuenta, x.fechapol, x.referencia, x.linea", (banderaLimiteFecha ? "AND fechapol < ?" : "")
                    );
        }

        
        private int saveCorte(List<tblM_KBCorteDet> lista, int usuario, DateTime fechaCorte, int tipo, int empresa)
        {
            try
            {
                var empresaBD = vSesiones.sesionEmpresaActual;
                int corteID = 0;

                var corte = new tblM_KBCorte();
                corte.id = 0;
                corte.fecha = DateTime.Now;
                corte.fechaCorte = fechaCorte;
                corte.usuarioID = usuario;
                corte.tipo = tipo;
                corte.anio = fechaCorte.Year;

                using (var _db = new MainContext(empresa == 1 ? (int)EmpresaEnum.Arrendadora : (int)EmpresaEnum.Construplan))
                {
                    var corteExistenteOtraEmpresa = _db.tblM_KBCorte.Where(x => x.fechaCorte == fechaCorte && x.tipo == tipo && (empresa == 1 ? !x.guardadoConstruplan : !x.guardadoArrendadora)).OrderByDescending(x => x.fecha).FirstOrDefault();
                    if (corteExistenteOtraEmpresa != null)
                    {
                        if (empresa == 1) { corte.guardadoArrendadora = true; }
                        else { corte.guardadoConstruplan = true; }
                    }

                    _db.SaveChanges();


                    ////var corteExistente = _context.tblM_KBCorte.Where(x => x.fechaCorte == fechaCorte && x.tipo == tipo && (empresa == 1 ? !x.guardadoConstruplan : !x.guardadoArrendadora)).OrderByDescending(x => x.fecha).FirstOrDefault();
                    //if (corteExistente == null)
                    //{
                    _context.tblM_KBCorte.Add(corte);
                    _context.SaveChanges();
                    corteID = corte.id;
                    //}
                    //else { corteID = corteExistente.id; }
                    string stringCon = "";
                    switch (empresaBD)
                    {
                        case 1:
                            stringCon = System.Configuration.ConfigurationManager.ConnectionStrings["MainContext"].ConnectionString;
                            break;
                        case 2:
                            stringCon = System.Configuration.ConfigurationManager.ConnectionStrings["MainContextArrendadora"].ConnectionString;
                            break;
                        default:
                            stringCon = System.Configuration.ConfigurationManager.ConnectionStrings["MainContextColombia"].ConnectionString;
                            break;
                    }
                    DataTable dt = new DataTable();
                    dt.Columns.Add("id", System.Type.GetType("System.Int32"));
                    dt.Columns.Add("corteID", System.Type.GetType("System.Int32"));
                    dt.Columns.Add("poliza", System.Type.GetType("System.String"));
                    dt.Columns.Add("cuenta", System.Type.GetType("System.String"));
                    dt.Columns.Add("concepto", System.Type.GetType("System.String"));
                    dt.Columns.Add("monto", System.Type.GetType("System.Decimal"));
                    dt.Columns.Add("cc", System.Type.GetType("System.String"));
                    dt.Columns.Add("areaCuenta", System.Type.GetType("System.String"));
                    dt.Columns.Add("fechapol", System.Type.GetType("System.DateTime"));
                    dt.Columns.Add("tipoEquipo", System.Type.GetType("System.Int32"));
                    dt.Columns.Add("referencia", System.Type.GetType("System.String"));
                    dt.Columns.Add("empresa", System.Type.GetType("System.Int32"));
                    dt.Columns.Add("linea", System.Type.GetType("System.Int32"));
                    foreach (var itm in lista)
                    {
                        DataRow row = dt.NewRow();
                        row["id"] = itm.id;
                        row["corteID"] = corteID;
                        row["poliza"] = (itm.poliza == null || itm.poliza == "") ? " " : itm.poliza;
                        row["cuenta"] = (itm.cuenta == null || itm.cuenta == "") ? " " : itm.cuenta;
                        row["concepto"] = (itm.concepto == null || itm.concepto == "" || itm.concepto.Length > 40) ? " " : itm.concepto;
                        row["monto"] = itm.monto;
                        row["cc"] = (itm.cc == null || itm.cc == "") ? " " : itm.cc;
                        row["areaCuenta"] = (itm.areaCuenta == null || itm.areaCuenta == "") ? " " : itm.areaCuenta;
                        row["fechapol"] = itm.fechapol == null ? fechaCorte : itm.fechapol;
                        row["tipoEquipo"] = itm.tipoEquipo;
                        row["referencia"] = (itm.referencia == null || itm.referencia == "") ? " " : itm.referencia;
                        row["empresa"] = (itm.empresa == null || itm.empresa < 1) ? empresa : itm.empresa;
                        row["linea"] = itm.linea;
                        dt.Rows.Add(row);
                    }
                    using (SqlConnection cn = new SqlConnection(stringCon))
                    {
                        cn.Open();
                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(cn))
                        {
                            bulkCopy.DestinationTableName = "dbo.tblM_KBCorteDet";
                            bulkCopy.WriteToServer(dt);
                        }
                        cn.Close();
                    }
                    var corteGuardado = _context.tblM_KBCorte.FirstOrDefault(x => x.id == corteID);
                    switch (empresa)
                    {
                        case 1:
                            corteGuardado.guardadoConstruplan = true;
                            if (corteExistenteOtraEmpresa != null) { corteExistenteOtraEmpresa.guardadoConstruplan = true; }

                            break;
                        case 2:
                            corteGuardado.guardadoArrendadora = true;
                            if (corteExistenteOtraEmpresa != null) { corteExistenteOtraEmpresa.guardadoArrendadora = true; }
                            break;
                    }
                    _context.SaveChanges();
                    return corteID;
                }
            }
            catch (Exception e)
            {
                return -1;
            }

        }

        private int GetTipoMaquina(string economico)
        {
            int data = 0;
            var auxFletes = getFletesActivos();
            auxFletes.Add("FLETES DE MAQUINARIA Y EQUIPO");
            var auxOTR = (new string[] { "TALLER DE REPARACION DE LLANTAS OTR" }).ToList();
            //var auxOtros = (new string[] { "MAQUINARIA Y EQUIPO EN RENTA PURA", "PROYECTO LA YAQUI NOMINA" }).ToList();
            List<string> auxAdminCentral = (new string[] { "ADMINISTRACIÓN CENTRAL ARRENDADORA  ", "ADMINISTRACIÓN CENTRAL ARRENDADORA", "ALMACEN Y TALLER EN PLANTA DE ASFALTO", "TALLER  HERMOSILLO NOMINA", "ALM Y TALLER EN PLANTA ASFALTO NOMINA", "GASTO TMC Y PATIO DE MAQ", "COMPRA HERRAMIENTA Y EQUIPO MENOR TMC" }).ToList();

            List<string> auxAdminProyectos = (new string[] { "ADQUISICION DE COMBUSTIBLES","MINADO LA COLORADA NOMINA","MINADO NOCHEBUENA II NOMINA","PROYECTO LA YAQUI NOMINA","PRESA DE JALES HERRADURA III NOMINA",
                "PATIOS NOCHE BUENA VII NOMINA","HERRADURA XIII NOMINA","PRESA SAN JULIAN ETAPA II NOMINA","MINADO SAN AGUSTIN NOMINA","PAVIMENTACION QUINTERO ARCE NOMINA","PLANTA DE ASFALTO NOMINA","CAPUFE HERMOSILLO-SANTA ANA NOMINA",
                "TALLER OVERHAUL EQ CONSTRUCCION NOMINA","CAPUFE - SANTANA NOMINA","PERSONAL EN DESARROLLO NÓMINA","TALLER CAPUFE TECATE NOMINA","TALLER DE REPARACION DE LLANTAS OTR NOMI","REHABILITACION COLECTOR 60\" NOMINA",
                "REHABILITACION COLECTOR DE 36” SAHUARO N","CAMINO CERRO PELON NOMINA","TERRACERIAS DESALADORA EMPALME NOMINA","MINADO CERRO PELON NOMINA","CONSERVACION CARRET CABOR-SONOYTA NOMINA","CONSTRUCCION DEL ACUEDUCTO DE 22\" NOMINA",
                "DEPOSITO DE JALES V NOMINA","NOMINA MINADO NOCHEBUENA III","NOMINA  EDIFICIO DE PROCESOS","NOMINA PAVIMENTACION ESTAC CENTRO","NOMINA REHABILITACION DE COLECTOR 60” ET","NOMINA PATIO 4B LA COLORADA",
                "NOMINA ESTACION DE COMPRESION PITIQUITO","NOMINA RECARPETEO BLVD SOLIDARIDAD","DEPRESIACIONES VARIAS","CONTROL DE MAQUINARIA","COMPRA COMPONENTES HERRADURA","COMPRA COMPONENTES LA COLORADA","COMPONENTES EQ CONSTRUCCION",
                "COMPONENTES SAN AGUSTIN","COMPRA DE HERAMIENTA Y EQUIPO MENOR","GASTOS TALLER MINADO LA COLORADA","GASTOS TALLER MINADO NOCHEBUENA II","GASTOS TALLER PROYECTO LA YAQUI","GASTOS TALL PRESA DE JALES HERRADURA III",
                "GASTOS TALLER PRESA SAN JULIAN ETAPA II","GASTOS TALLER PATIOS NOCHEBUENA VII","GASTOS TALLER HERRADURA XIII","GASTOS TALLER SAN AGUSTIN","CARRILERIA","COMPRA Y REPARACION DE CUCHARONES Y CAJA","GASTOS TALLER QUINTERO ARCE",
                "GASTOS TALLER VARIAS CALLES","GASTOS TALLER CAPUFE HERMOSILLO-SANTANA","GASTOS TALLER PLANTA DE ASFALTO","GASTOS TALLER PRESA DE AGUA SAN JULIAN","GASTOS TALLER CAPUFE-SANTA","GASTOS TALLER CAPUFE TECATE",
                "MINA SUBTERRANEA SAN JULIAN","GASTOS TALLER REHABILITACION DE COLECTOR","GASTOS TALLER MINADO LA HERRADURA","GASTOS TALLER CAMINO CERRO PELON","GASTOS TALLER TERRACERIAS DESALADORA EMP","GASTOS TALLER RENTA DE MAQUINARIA MULATO",
                "GASTOS TALLER MINADO CERRO PELON","GASTOS TALLER CONSERVA CARRET CAB-SONOYT","COMPRA DE COMPONENTES CERRO PELÓN","GASTOS TALLER CONSTRUCCION DEL ACUEDUCTO","GASTOS TALLER DEPOSITO DE JALES V","CRC Construplan",
                "GASTOS TALLER MMINADO NOCHEBUENA III","GASTOS TALLER EDIFICIO DE PROCESOS","GASTOS TALLER PAVIMENTACION ESTAC CENTRO","Gastos CRC Construplan","GASTOS TALLER GM SILAO MISCELANEOS ENSAM","GASTOS TALLER REHABILITACION DE COLECTOR",
                "Gastos Taller Overhaul Construccion","Compra Herramienta Overhaul Construcción","GASTOS TALLER PATIO 4B LA COLORADA","GASTOS TALLER ESTACION DE COMPRESION PIT","GASTOS TALLER ESTACION DE COMPRESION PITIQUITO","GASTOS TALLER AMPLIACION OFICINAS CENTRALES",
                "GASTOS TALLER RECARPETEO BLVD SOLIDARIDAD","GASTOS TALLER RECARPETEO BLVD SOLIDARIDA","GASTOS TALLER DEPOSITO DE JALES SECOS VI","GASTOS TALLER COMERCIALIZACION AUTOMOTRIZ","MAQUINARIA Y EQUIPO EN RENTA PURA" }).ToList();


            if (auxFletes.Contains(economico.Trim())) { data = 4; }
            else
            {
                if (auxOTR.Contains(economico.Trim())) { data = 5; }
                else
                {
                    if (auxAdminCentral.Contains(economico.Trim())) { data = 6; }
                    else
                    {
                        if (auxAdminProyectos.Contains(economico.Trim())) { data = 9; }
                        else
                        {
                            var maquina = _context.tblM_CatMaquina.FirstOrDefault(x => x.noEconomico == economico.Trim());
                            if (maquina == null) { data = 7; }
                            else
                            {
                                if (maquina.grupoMaquinaria.tipoEquipoID == 3 && maquina.CargoEmpresa == 2) { data = 8; }
                                else data = maquina.grupoMaquinaria.tipoEquipoID;
                            }
                        }
                    }
                }
            }
            return data;
        }

        private void GetTipoMaquinaCorte(List<tblM_KBCorteDet> listaCorte)
        {
            var maquinasCorte = listaCorte.Select(x => x.cc).Distinct().ToList();
            List<tblM_CatMaquina> auxMaquinas = new List<tblM_CatMaquina>();

            using (var _db = new MainContext((int)EmpresaEnum.Arrendadora))
            {
                auxMaquinas = _db.tblM_CatMaquina.Where(x => maquinasCorte.Contains(x.noEconomico)).ToList();

                var auxFletes = getFletesActivos();
                auxFletes.Add("FLETES DE MAQUINARIA Y EQUIPO");
                var auxOTR = (new string[] { "TALLER DE REPARACION DE LLANTAS OTR" }).ToList();
                //var auxOtros = (new string[] { "MAQUINARIA Y EQUIPO EN RENTA PURA", "PROYECTO LA YAQUI NOMINA" }).ToList();
                List<string> auxAdminCentral = (new string[] { "ADMINISTRACIÓN CENTRAL ARRENDADORA  ", "ADMINISTRACIÓN CENTRAL ARRENDADORA", "ALMACEN Y TALLER EN PLANTA DE ASFALTO", "TALLER  HERMOSILLO NOMINA", "ALM Y TALLER EN PLANTA ASFALTO NOMINA", "GASTO TMC Y PATIO DE MAQ", "COMPRA HERRAMIENTA Y EQUIPO MENOR TMC" }).ToList();

                List<string> auxAdminProyectos = (new string[] { "ADQUISICION DE COMBUSTIBLES","MINADO LA COLORADA NOMINA","MINADO NOCHEBUENA II NOMINA","PROYECTO LA YAQUI NOMINA","PRESA DE JALES HERRADURA III NOMINA",
                    "PATIOS NOCHE BUENA VII NOMINA","HERRADURA XIII NOMINA","PRESA SAN JULIAN ETAPA II NOMINA","MINADO SAN AGUSTIN NOMINA","PAVIMENTACION QUINTERO ARCE NOMINA","PLANTA DE ASFALTO NOMINA","CAPUFE HERMOSILLO-SANTA ANA NOMINA",
                    "TALLER OVERHAUL EQ CONSTRUCCION NOMINA","CAPUFE - SANTANA NOMINA","PERSONAL EN DESARROLLO NÓMINA","TALLER CAPUFE TECATE NOMINA","TALLER DE REPARACION DE LLANTAS OTR NOMI","REHABILITACION COLECTOR 60\" NOMINA",
                    "REHABILITACION COLECTOR DE 36” SAHUARO N","CAMINO CERRO PELON NOMINA","TERRACERIAS DESALADORA EMPALME NOMINA","MINADO CERRO PELON NOMINA","CONSERVACION CARRET CABOR-SONOYTA NOMINA","CONSTRUCCION DEL ACUEDUCTO DE 22\" NOMINA",
                    "DEPOSITO DE JALES V NOMINA","NOMINA MINADO NOCHEBUENA III","NOMINA  EDIFICIO DE PROCESOS","NOMINA PAVIMENTACION ESTAC CENTRO","NOMINA REHABILITACION DE COLECTOR 60” ET","NOMINA PATIO 4B LA COLORADA",
                    "NOMINA ESTACION DE COMPRESION PITIQUITO","NOMINA RECARPETEO BLVD SOLIDARIDAD","DEPRESIACIONES VARIAS","CONTROL DE MAQUINARIA","COMPRA COMPONENTES HERRADURA","COMPRA COMPONENTES LA COLORADA","COMPONENTES EQ CONSTRUCCION",
                    "COMPONENTES SAN AGUSTIN","COMPRA DE HERAMIENTA Y EQUIPO MENOR","GASTOS TALLER MINADO LA COLORADA","GASTOS TALLER MINADO NOCHEBUENA II","GASTOS TALLER PROYECTO LA YAQUI","GASTOS TALL PRESA DE JALES HERRADURA III",
                    "GASTOS TALLER PRESA SAN JULIAN ETAPA II","GASTOS TALLER PATIOS NOCHEBUENA VII","GASTOS TALLER HERRADURA XIII","GASTOS TALLER SAN AGUSTIN","CARRILERIA","COMPRA Y REPARACION DE CUCHARONES Y CAJA","GASTOS TALLER QUINTERO ARCE",
                    "GASTOS TALLER VARIAS CALLES","GASTOS TALLER CAPUFE HERMOSILLO-SANTANA","GASTOS TALLER PLANTA DE ASFALTO","GASTOS TALLER PRESA DE AGUA SAN JULIAN","GASTOS TALLER CAPUFE-SANTA","GASTOS TALLER CAPUFE TECATE",
                    "MINA SUBTERRANEA SAN JULIAN","GASTOS TALLER REHABILITACION DE COLECTOR","GASTOS TALLER MINADO LA HERRADURA","GASTOS TALLER CAMINO CERRO PELON","GASTOS TALLER TERRACERIAS DESALADORA EMP","GASTOS TALLER RENTA DE MAQUINARIA MULATO",
                    "GASTOS TALLER MINADO CERRO PELON","GASTOS TALLER CONSERVA CARRET CAB-SONOYT","COMPRA DE COMPONENTES CERRO PELÓN","GASTOS TALLER CONSTRUCCION DEL ACUEDUCTO","GASTOS TALLER DEPOSITO DE JALES V","CRC Construplan",
                    "GASTOS TALLER MMINADO NOCHEBUENA III","GASTOS TALLER EDIFICIO DE PROCESOS","GASTOS TALLER PAVIMENTACION ESTAC CENTRO","Gastos CRC Construplan","GASTOS TALLER GM SILAO MISCELANEOS ENSAM","GASTOS TALLER REHABILITACION DE COLECTOR",
                    "Gastos Taller Overhaul Construccion","Compra Herramienta Overhaul Construcción","GASTOS TALLER PATIO 4B LA COLORADA","GASTOS TALLER ESTACION DE COMPRESION PIT","GASTOS TALLER ESTACION DE COMPRESION PITIQUITO","GASTOS TALLER AMPLIACION OFICINAS CENTRALES",
                    "GASTOS TALLER RECARPETEO BLVD SOLIDARIDAD","GASTOS TALLER RECARPETEO BLVD SOLIDARIDA","GASTOS TALLER DEPOSITO DE JALES SECOS VI","GASTOS TALLER COMERCIALIZACION AUTOMOTRIZ","MAQUINARIA Y EQUIPO EN RENTA PURA" }).ToList();

                foreach (var item in listaCorte)
                {
                    if (auxFletes.Contains(item.cc)) { item.tipoEquipo = 4; }
                    else
                    {
                        if (auxOTR.Contains(item.cc)) { item.tipoEquipo = 5; }
                        else
                        {
                            if (auxAdminCentral.Contains(item.cc)) { item.tipoEquipo = 6; }
                            else
                            {
                                if (auxAdminProyectos.Contains(item.cc)) { item.tipoEquipo = 9; }
                                else
                                {
                                    var maquina = auxMaquinas.FirstOrDefault(x => x.noEconomico == item.cc);
                                    if (maquina == null) { item.tipoEquipo = 7; }
                                    else
                                    {
                                        if (maquina.grupoMaquinaria.tipoEquipoID == 3 && maquina.CargoEmpresa == 2) { item.tipoEquipo = 8; }
                                        else item.tipoEquipo = maquina.grupoMaquinaria.tipoEquipoID;
                                    }
                                }
                            }

                        }
                    }
                }
            }
        }

        public List<DateTime> getLstFechasCortes(int tipoCorte)
        {
            List<DateTime> data = new List<DateTime>();
            var cortes = _context.tblM_KBCorte.Where(x => x.tipo == tipoCorte /*&& x.guardadoArrendadora && x.guardadoConstruplan*/).ToList();
            if (cortes.Count() > 0)
            {
                //Caso especial 26 de febrero
                var especial = cortes.FirstOrDefault(x => x.id == 17);
                if (especial != null)
                {
                    var auxEspecial = _context.tblM_KBCorte.FirstOrDefault(x => x.id == 20);
                    if (auxEspecial != null)
                    {
                        cortes.Remove(especial);
                        cortes.Add(auxEspecial);
                    }
                }
                data = cortes.Select(x => x.fechaCorte).ToList();
            }
            return data;
        }

        public List<tblM_KBCorte> getCortesPorFecha(DateTime fecha, int tipo)
        {
            var diaSiguiente = fecha.AddDays(1);
            var auxFechaEspecial = new DateTime(2020, 2, 29);
            var data = _context.tblM_KBCorte.Where(x => x.fechaCorte >= fecha && x.fechaCorte < diaSiguiente && (auxFechaEspecial.Date == fecha.Date ? true : x.tipo == tipo) /*&& x.guardadoArrendadora && x.guardadoConstruplan*/).OrderByDescending(x => x.fecha).ToList();
            if (fecha.Date == auxFechaEspecial.Date)
            {
                data = _context.tblM_KBCorte.Where(x => x.id == 20 && x.guardadoArrendadora).ToList();
            }

            //var especial = data.FirstOrDefault(x => x.id == 17);
            //if (especial != null) 
            //{
            //    //Caso especial 29 de enero
            //    var auxEspecial = _context.tblM_KBCorte.FirstOrDefault(x => x.id == 20);
            //    if (auxEspecial != null) 
            //    {
            //        data.Remove(especial);
            //        data.Add(auxEspecial);
            //    }
            //}
            return data;
        }

        public List<tblM_KBCorte> getCortesAnt(DateTime fecha, int tipo)
        {
            var data = _context.tblM_KBCorte.Where(x => x.fechaCorte < fecha && x.tipo == tipo).OrderByDescending(x => x.fechaCorte).Take(5).ToList();
            return data;
        }

        public List<tblM_KBCorte> getCortesPorMes(DateTime fecha)
        {
            var fechaInicio = new DateTime(fecha.Year, fecha.Month, 1);
            var fechaFin = fechaInicio.AddMonths(1);
            var data = _context.tblM_KBCorte.Where(x => x.fechaCorte >= fechaInicio && x.fechaCorte < fechaFin && x.tipo == 1).OrderByDescending(x => x.fecha).ToList();
            return data;
        }

        //List<OdbcParameterDTO> parametrosConsultaArrendadora(int corteID, DateTime fechaInicio, DateTime fechaFin, List<string> areaCuenta, List<string> maquinas, string economico)
        //{
        //    var lst = new List<OdbcParameterDTO>();
        //    lst.Add(new OdbcParameterDTO() { nombre = "corteID", tipo = OdbcType.Int, valor = corteID });
        //    lst.Add(new OdbcParameterDTO() { nombre = "fecha", tipo = OdbcType.DateTime, valor = fechaInicio });
        //    lst.Add(new OdbcParameterDTO() { nombre = "fecha", tipo = OdbcType.DateTime, valor = fechaFin.AddDays(1) });
        //    if (areaCuenta.Count() > 0) { lst.AddRange(areaCuenta.Select(s => new OdbcParameterDTO() { nombre = "areaCuenta", tipo = OdbcType.VarChar, valor = s })); }

        //    if (maquinas.Count() > 0) { lst.AddRange(maquinas.Select(s => new OdbcParameterDTO() { nombre = "cc", tipo = OdbcType.VarChar, valor = s })); }
        //    if ((economico == null || economico == "")) lst.Add(new OdbcParameterDTO() { nombre = "cc", tipo = OdbcType.Int, valor = corteID });
        //    //if (busq.obra != "TODOS" && busq.obra != "S/A")
        //    //{
        //    //    var obra = _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == busq.obra);
        //    //    lst.Add(new OdbcParameterDTO() { nombre = "area", tipo = OdbcType.Numeric, valor = obra.area });
        //    //    lst.Add(new OdbcParameterDTO() { nombre = "cuenta", tipo = OdbcType.Numeric, valor = obra.cuenta });
        //    //}
        //    lst.AddRange(auxFacturasEntrada.Select(s => new OdbcParameterDTO() { nombre = "referencia", tipo = OdbcType.VarChar, valor = s }));
        //    return lst;
        //}

        public List<CortePpalDTO> getLstKubrixCorte(int corteID, List<string> areaCuenta, List<int> modelos, string economico, DateTime fechaInicio, DateTime fechaFin, int semana, int usuarioID, bool reporteCostos)
        {
            var empresa = (int)EmpresaEnum.Arrendadora;
            if (vSesiones.sesionEmpresaActual == 2) { empresa = (int)EmpresaEnum.Construplan; }
            var corte = _context.tblM_KBCorte.FirstOrDefault(x => x.id == corteID);
            var centrosCostosEnkontrolArr = getListaCC().ToList();
            var centrosCostosEnkontrolCplan = getListaCCCPlan();

            using (var _db = new MainContext(empresa))
            {
                _context.Database.CommandTimeout = 300;
                _db.Database.CommandTimeout = 300;
                List<tblM_CatMaquina> maquinas = new List<tblM_CatMaquina>();
                if (vSesiones.sesionEmpresaActual == 2) { maquinas = _context.tblM_CatMaquina.ToList(); }
                else { maquinas = _db.tblM_CatMaquina.ToList(); }

                List<string> auxObras = new List<string>();
                if (areaCuenta != null) auxObras = areaCuenta;
                if (corteID == 20) { fechaFin = new DateTime(2020, 2, 29); }

                if (vSesiones.sesionEmpresaActual == 2)
                {
                    var auxFacturasConcSalida = _context.tblM_CapEncConciliacionHorometros.Where(x => x.estatus == 1 && x.facturado && x.fechaFin.Month == 12 && x.fechaFin < fechaInicio).ToList();
                    List<string> auxFacturasSalida = new List<string>();
                    foreach (var item in auxFacturasConcSalida) { auxFacturasSalida.AddRange(JsonConvert.DeserializeObject<List<string>>(item.factura)); }
                    auxFacturasSalida = auxFacturasSalida.Distinct().ToList();
                    auxFacturasSalida.Remove("0");
                    if (auxFacturasSalida.Count() < 1) auxFacturasSalida.Add("-1");
                }
                var maquinasModelo = modelos == null ? new List<string>() : (vSesiones.sesionEmpresaActual == 2 ? _context.tblM_CatMaquina.Where(x => modelos.Contains(x.modeloEquipoID)) : _db.tblM_CatMaquina.Where(x => modelos.Contains(x.modeloEquipoID))).Select(x => x.noEconomico).ToList();
                List<CortePpalDTO> detalles = new List<CortePpalDTO>();

                List<CortePpalDTO> auxDetalles;

                //                //if (semana == 1)
                //                //{
                //                var consultaDapper = new DapperDTO();
                //                consultaDapper.baseDatos = Core.Enum.Principal.MainContextEnum.Arrendadora;
                //                consultaDapper.consulta = String.Format(@"SELECT * FROM tblM_KBCorteDet where
                //                    cc != null 
                //                    {0}
                //                    AND corteID = ?
                //                    AND fechapol <= ? 
                //                    AND fechapol >= ?
                //                    AND {1}
                //                    AND {2}
                //                    AND {3}
                //                    AND (referencia = null OR referencia NOT LIKE {4})
                //                    AND YEAR(fechapol) = ?
                //                    AND cuenta NOT LIKE '4000-5-%' 
                //                    AND x.cuenta NOT LIKE '4000-6-%'"
                //                    , (reporteCostos ? " cuenta like '5000-%' OR cuenta like 5280-%' OR cuenta = '1-4-0' " : "")
                //                    , (auxObras.Count() > 0 ? (" areaCuenta in " + auxObras.ToParamInValue() + " ") : " ")
                //                    , (maquinasModelo.Count() > 0 ? " cc in  " + maquinasModelo.ToParamInValue() + " " : " ")
                //                    , ((economico == null || economico == "") ? " cc = " + economico + " " : " ")
                //                    , "/" + (fechaInicio.Year - 1).ToString() + " ");
                //                consultaDapper.parametros =;

                //List<tblM_KBCorteDet> auxDetallesGrouped = _context.Select<tblM_KBCorteDet>(consultaDapper);
                var auxDetallesGrouped = from x in _context.tblM_KBCorteDet.AsQueryable()
                                         where x.cc != null && (reporteCostos ? (x.cuenta.Contains("5000-") || x.cuenta.Contains("5280-") || x.cuenta == "1-4-0") : true) && (x.corteID == corteID) && x.fechapol <= fechaFin && x.fechapol >= fechaInicio
                                             && (auxObras.Count() > 0 ? auxObras.Contains(x.areaCuenta) : true) && (maquinasModelo.Count() > 0 ? maquinasModelo.Contains(x.cc) : true)
                                             && ((economico == null || economico == "") ? true : x.cc == economico) && !(x.referencia != null && x.referencia.Contains("/" + (fechaInicio.Year - 1).ToString())
                                             && x.fechapol.Year == fechaInicio.Year) && !x.cuenta.Contains("4000-5-") && !x.cuenta.Contains("4000-6-")
                                         select x;
                auxDetalles = (from x in auxDetallesGrouped
                               group x by new { x.areaCuenta, x.cuenta, x.cc, x.tipoEquipo, x.referencia, x.empresa } into grouped
                               select new CortePpalDTO
                               {
                                   //corteID = corteID,
                                   cuenta = grouped.Key.cuenta,
                                   monto = grouped.Sum(y => y.monto) * (grouped.Key.cuenta != "1-1-0" && grouped.Key.cuenta != "1-2-1" && grouped.Key.cuenta != "1-2-2" && grouped.Key.cuenta != "1-2-3" && grouped.Key.cuenta != "1-3-1" && grouped.Key.cuenta != "1-3-2" && !reporteCostos ? (-1) : (1)),
                                   tipoEquipo = grouped.Key.tipoEquipo,
                                   cc = grouped.Key.cc,
                                   semana = semana,
                                   areaCuenta = grouped.Key.areaCuenta,
                                   referencia = grouped.Key.referencia,
                                   empresa = grouped.Key.empresa,
                                   tipoMov = 0,
                               }).ToList();

                detalles = auxDetalles.ToList();

                if (corte != null && vSesiones.sesionEmpresaActual == 2)
                {
                    var corteOtraEmpresa = _db.tblM_KBCorte.FirstOrDefault(x => x.fechaCorte == corte.fechaCorte);
                    if (corteOtraEmpresa != null)
                    {
                        var auxDetallesCplanGrouped = from x in _db.tblM_KBCorteDet.AsQueryable()
                                                      where x.cc != null && (reporteCostos ? (x.cuenta.Contains("5000-") || x.cuenta.Contains("5280-") || x.cuenta == "1-4-0") : true) && (x.corteID == corteOtraEmpresa.id) && x.fechapol <= fechaFin && x.fechapol >= fechaInicio
                                                      && (auxObras.Count() > 0 ? auxObras.Contains(x.areaCuenta) : true) && (maquinasModelo.Count() > 0 ? maquinasModelo.Contains(x.cc) : true)
                                                      && ((economico == null || economico == "") ? true : x.cc == economico) && !(x.referencia != null && x.referencia.Contains("/" + (fechaInicio.Year - 1).ToString())
                                                      && x.fechapol.Year == fechaInicio.Year) && !x.cuenta.Contains("4000-5-") && !x.cuenta.Contains("4000-6-") && (x.cuenta.Contains("5000-10-") || x.cuenta.Contains("5900-3-") || x.cuenta.Contains("5280-10-"))
                                                      select x;

                        var auxDetallesCplan = from x in auxDetallesCplanGrouped
                                               group x by new { x.areaCuenta, x.cuenta, x.cc, x.tipoEquipo, x.referencia, x.empresa } into grouped
                                               select new CortePpalDTO
                                               {
                                                   //corteID = corteID,
                                                   cuenta = grouped.Key.cuenta,
                                                   monto = grouped.Sum(y => y.monto) * (grouped.Key.cuenta != "1-1-0" && grouped.Key.cuenta != "1-2-1" && grouped.Key.cuenta != "1-2-2" && grouped.Key.cuenta != "1-2-3" && grouped.Key.cuenta != "1-3-1" && grouped.Key.cuenta != "1-3-2" && !reporteCostos ? (-1) : (1)),
                                                   tipoEquipo = grouped.Key.tipoEquipo,
                                                   cc = grouped.Key.cc,
                                                   semana = semana,
                                                   areaCuenta = grouped.Key.areaCuenta,
                                                   referencia = grouped.Key.referencia,
                                                   empresa = grouped.Key.empresa,
                                                   tipoMov = 0,
                                               };
                        var DetallesCplan = auxDetallesCplan.ToList();
                        detalles.AddRange(DetallesCplan);
                    }
                }

                if (auxObras.Count() <= 0 || auxObras.Contains("6-5"))
                {
                    var auxFletesGrouped = from x in _context.tblM_KBCorteDet.AsQueryable()
                                           where x.cc != null && x.corteID == corteID && x.cuenta.Contains("4000-6-") && x.fechapol <= fechaFin && x.fechapol >= fechaInicio
                                               && (maquinasModelo.Count() > 0 ? maquinasModelo.Contains(x.cc) : true) && ((economico == null || economico == "" || economico == "TODOS") ? true : x.cc == economico) && !(x.referencia != null
                                               && x.referencia.Contains("/" + (fechaInicio.Year - 1).ToString()) && x.fechapol.Year == fechaInicio.Year)
                                           select x;
                    var auxFletes = from x in auxFletesGrouped
                                    group x by new { x.cuenta, x.cc, x.tipoEquipo, x.referencia, x.empresa } into grouped
                                    select new CortePpalDTO
                                    {
                                        //corteID = corteID,
                                        cuenta = grouped.Key.cuenta,
                                        monto = grouped.Sum(y => y.monto) * (grouped.Key.cuenta != "1-1-0" && grouped.Key.cuenta != "1-2-1" && grouped.Key.cuenta != "1-2-2" && grouped.Key.cuenta != "1-2-3" && grouped.Key.cuenta != "1-3-1" && grouped.Key.cuenta != "1-3-2" && !reporteCostos ? (-1) : (1)),
                                        tipoEquipo = grouped.Key.tipoEquipo,
                                        cc = grouped.Key.cc,
                                        semana = semana,
                                        areaCuenta = "6-5",
                                        referencia = grouped.Key.referencia,
                                        empresa = grouped.Key.empresa,
                                        tipoMov = 0,
                                    };
                    var fletes = auxFletes.ToList();
                    detalles.AddRange(fletes);
                }
                if (auxObras.Count() <= 0 || auxObras.Contains("9-27"))
                {
                    var auxOtrGropued = from x in _context.tblM_KBCorteDet.AsQueryable()
                                        where x.cc != null && x.corteID == corteID && x.cuenta.Contains("4000-5-") && x.fechapol <= fechaFin && x.fechapol >= fechaInicio
                                            && (maquinasModelo.Count() > 0 ? maquinasModelo.Contains(x.cc) : true) && ((economico == null || economico == "" || economico == "TODOS") ? true : x.cc == economico) && !(x.referencia != null
                                            && x.referencia.Contains("/" + (fechaInicio.Year - 1).ToString()) && x.fechapol.Year == fechaInicio.Year)
                                        select x;
                    var auxOtr = from x in auxOtrGropued
                                 group x by new { x.cuenta, x.cc, x.tipoEquipo, x.referencia, x.empresa } into grouped
                                 select new CortePpalDTO
                                 {
                                     //corteID = corteID,
                                     cuenta = grouped.Key.cuenta,
                                     monto = grouped.Sum(y => y.monto) * (grouped.Key.cuenta != "1-1-0" && grouped.Key.cuenta != "1-2-1" && grouped.Key.cuenta != "1-2-2" && grouped.Key.cuenta != "1-2-3" && grouped.Key.cuenta != "1-3-1" && grouped.Key.cuenta != "1-3-2" && !reporteCostos ? (-1) : (1)),
                                     tipoEquipo = grouped.Key.tipoEquipo,
                                     cc = grouped.Key.cc,
                                     semana = semana,
                                     areaCuenta = "9-27",
                                     referencia = grouped.Key.referencia,
                                     empresa = grouped.Key.empresa,
                                     tipoMov = 0,
                                 };
                    var otr = auxOtr.ToList();
                    detalles.AddRange(otr);
                }
                #region COMENTADO
                //}
                //else
                //{
                //    auxDetalles = from x in _context.tblM_KBCorteDet.AsQueryable()
                //        where x.cc != null && (reporteCostos ? (x.cuenta.Contains("5000-") || x.cuenta.Contains("5280-") || x.cuenta == "1-4-0") : true) && (x.corteID == corteID) && x.fechapol <= fechaFin && x.fechapol >= fechaInicio
                //            && (auxObras.Count() > 0 ? auxObras.Contains(x.areaCuenta) : true) && (maquinasModelo.Count() > 0 ? maquinasModelo.Contains(x.cc) : true)
                //            && ((economico == null || economico == "") ? true : x.cc == economico) && !(x.referencia != null && x.referencia.Contains("/" + (fechaInicio.Year - 1).ToString())
                //            && x.fechapol.Year == fechaInicio.Year) && !x.cuenta.Contains("4000-5-") && !x.cuenta.Contains("4000-6-")
                //        group x by new { x.areaCuenta, x.cuenta } into grouped
                //        select new CorteDTO
                //        {
                //            corteID = corteID,
                //            cuenta = grouped.Key.cuenta,
                //            monto = grouped.Sum(y => y.monto) * (grouped.Key.cuenta != "1-1-0" && grouped.Key.cuenta != "1-2-1" && grouped.Key.cuenta != "1-2-2" && grouped.Key.cuenta != "1-2-3" && grouped.Key.cuenta != "1-3-1" && grouped.Key.cuenta != "1-3-2" && !reporteCostos ? (-1) : (1)),
                //            semana = semana,
                //            areaCuenta = grouped.Key.areaCuenta,
                //            tipoMov = 0,
                //        };
                //    detalles = auxDetalles.ToList();

                //    if (corte != null && vSesiones.sesionEmpresaActual == 2)
                //    {
                //        var corteOtraEmpresa = _db.tblM_KBCorte.FirstOrDefault(x => x.fechaCorte == corte.fechaCorte);
                //        if (corteOtraEmpresa != null)
                //        {
                //            var auxDetallesOtraEmpresaRaw = from x in _db.tblM_KBCorteDet.AsQueryable()
                //                where x.cc != null && (reporteCostos ? (x.cuenta.Contains("5000-") || x.cuenta.Contains("5280-") || x.cuenta == "1-4-0") : true) && (x.corteID == corteOtraEmpresa.id) && x.fechapol <= fechaFin && x.fechapol >= fechaInicio
                //                    && (auxObras.Count() > 0 ? auxObras.Contains(x.areaCuenta) : true) && (maquinasModelo.Count() > 0 ? maquinasModelo.Contains(x.cc) : true)
                //                    && ((economico == null || economico == "") ? true : x.cc == economico) && !(x.referencia != null && x.referencia.Contains("/" + (fechaInicio.Year - 1).ToString())
                //                    && x.fechapol.Year == fechaInicio.Year) && !x.cuenta.Contains("4000-5-") && !x.cuenta.Contains("4000-6-") && (x.cuenta.Contains("5000-10-") || x.cuenta.Contains("5900-3-") || x.cuenta.Contains("5280-10-"))
                //                group x by new { x.areaCuenta, x.cuenta } into grouped
                //                select new CorteDTO
                //                {
                //                    corteID = corteID,
                //                    cuenta = grouped.Key.cuenta,
                //                    monto = grouped.Sum(y => y.monto) * (grouped.Key.cuenta != "1-1-0" && grouped.Key.cuenta != "1-2-1" && grouped.Key.cuenta != "1-2-2" && grouped.Key.cuenta != "1-2-3" && grouped.Key.cuenta != "1-3-1" && grouped.Key.cuenta != "1-3-2" && !reporteCostos ? (-1) : (1)),
                //                    semana = semana,
                //                    areaCuenta = grouped.Key.areaCuenta,
                //                    tipoMov = 0,
                //                };
                //            detalles.AddRange(auxDetallesOtraEmpresaRaw.ToList());
                //        }
                //    } 

                //    if (auxObras.Count() <= 0 || auxObras.Contains("6-5"))
                //    {
                //        var fletesAux = from x in _context.tblM_KBCorteDet.AsQueryable()
                //            where x.cc != null && x.corteID == corteID && x.cuenta.Contains("4000-6-") && x.fechapol <= fechaFin && x.fechapol >= fechaInicio
                //                && (maquinasModelo.Count() > 0 ? maquinasModelo.Contains(x.cc) : true) && ((economico == null || economico == "" || economico == "TODOS") ? true : x.cc == economico) && !(x.referencia != null
                //                && x.referencia.Contains("/" + (fechaInicio.Year - 1).ToString()) && x.fechapol.Year == fechaInicio.Year)
                //            group x by new { x.cuenta } into grouped
                //            select new CorteDTO
                //            {
                //                corteID = corteID,
                //                cuenta = grouped.Key.cuenta,
                //                monto = grouped.Sum(y => y.monto) * (grouped.Key.cuenta != "1-1-0" && grouped.Key.cuenta != "1-2-1" && grouped.Key.cuenta != "1-2-2" && grouped.Key.cuenta != "1-2-3" && grouped.Key.cuenta != "1-3-1" && grouped.Key.cuenta != "1-3-2" && !reporteCostos ? (-1) : (1)),
                //                semana = semana,
                //                areaCuenta = "6-5",
                //                tipoMov = 0,
                //            };
                //        detalles.AddRange(fletesAux.ToList());
                //    }
                //    if (auxObras.Count() <= 0 || auxObras.Contains("9-27"))
                //    {
                //        var otrAux = from x in _context.tblM_KBCorteDet.AsQueryable()
                //            where x.cc != null && x.corteID == corteID && x.cuenta.Contains("4000-5-") && x.fechapol <= fechaFin && x.fechapol >= fechaInicio
                //                && (maquinasModelo.Count() > 0 ? maquinasModelo.Contains(x.cc) : true) && ((economico == null || economico == "" || economico == "TODOS") ? true : x.cc == economico) && !(x.referencia != null
                //                && x.referencia.Contains("/" + (fechaInicio.Year - 1).ToString()) && x.fechapol.Year == fechaInicio.Year)
                //            group x by new { x.cuenta } into grouped
                //            select new CorteDTO
                //            {
                //                corteID = corteID,
                //                cuenta = grouped.Key.cuenta,
                //                monto = grouped.Sum(y => y.monto) * (grouped.Key.cuenta != "1-1-0" && grouped.Key.cuenta != "1-2-1" && grouped.Key.cuenta != "1-2-2" && grouped.Key.cuenta != "1-2-3" && grouped.Key.cuenta != "1-3-1" && grouped.Key.cuenta != "1-3-2" && !reporteCostos ? (-1) : (1)),
                //                semana = semana,
                //                areaCuenta = "9-27",
                //                tipoMov = 0,
                //            };
                //        detalles.AddRange(otrAux.ToList());
                //    }
                //}
                #endregion
                if (vSesiones.sesionEmpresaActual == 1)
                {
                    foreach (var item in detalles)
                    {
                        var auxAreaCuentaCplan = centrosCostosEnkontrolCplan.FirstOrDefault(x => x.Prefijo == null ? x.Value == item.areaCuenta : x.Prefijo == item.areaCuenta);
                        if (auxAreaCuentaCplan == null)
                        {
                            var auxAreaCuentaArr = centrosCostosEnkontrolArr.FirstOrDefault(x => x.Value == item.areaCuenta);
                            item.areaCuenta = auxAreaCuentaArr == null ? item.areaCuenta : auxAreaCuentaArr.Text;
                        }
                        else { item.areaCuenta = auxAreaCuentaCplan.Text; }
                    }
                }
                else
                {
                    foreach (var item in detalles)
                    {
                        var auxAreaCuentaArr = centrosCostosEnkontrolArr.FirstOrDefault(x => x.Value == item.areaCuenta);
                        if (auxAreaCuentaArr == null)
                        {
                            var auxAreaCuentaCplan = centrosCostosEnkontrolCplan.FirstOrDefault(x => x.Value == item.areaCuenta);
                            item.areaCuenta = auxAreaCuentaCplan == null ? item.areaCuenta : auxAreaCuentaCplan.Text;
                        }
                        else { item.areaCuenta = auxAreaCuentaArr.Text; }
                    }
                }
                return detalles;
            }
        }

        public List<CortePpalDTO> getLstKubrixCortes(List<int> cortes, List<string> areaCuenta, List<int> modelos, string economico, DateTime fechaInicio, List<DateTime> fechasFin, int usuarioID, bool reporteCostos)
        {
            var empresa = (int)EmpresaEnum.Arrendadora;
            if (vSesiones.sesionEmpresaActual == 2) { empresa = (int)EmpresaEnum.Construplan; }
            var centrosCostosEnkontrolArr = getListaCC().ToList();
            var centrosCostosEnkontrolCplan = getListaCCCPlan();
            IQueryable<tblM_KBCorteDet> dataSemAnterior = null;
            List<tblM_KBCorteDet> dataSemAntOtraEmp = null;
            DateTime fechaInicioAnio = new DateTime(fechasFin[fechasFin.Count() - 1].Year, 1, 1);
            List<string> estimados = (new string[] { "1-1-0", "1-2-1", "1-2-2", "1-2-3", "1-3-1", "1-3-2", "1-4-0" }).ToList();

            List<string> ccAcumulado2020 = (new string[] { "10-2", "10-3", "10-6", "10-7", "10-8", "10-9", "10-10", "11-1", "11-2", "10-12", "11-9", "10-24" }).ToList();
            DateTime fechaInicioAcumulado2020 = new DateTime(2020, 1, 1);


            List<string> ccActual = (new string[] { "009", "017", "018", "019", "028", "029", "557", "558", "562", "C65", "C66", "C69", "C71" }).ToList();
            DateTime fechaInicioActual = new DateTime(DateTime.Now.Year, 1, 1);


            using (var _db = new MainContext(empresa))
            {
                _context.Database.CommandTimeout = 300;
                _db.Database.CommandTimeout = 300;
                List<tblM_CatMaquina> maquinas = new List<tblM_CatMaquina>();
                if (vSesiones.sesionEmpresaActual == 2) { maquinas = _context.tblM_CatMaquina.ToList(); }
                else { maquinas = _db.tblM_CatMaquina.ToList(); }

                List<string> auxObras = new List<string>();
                if (areaCuenta != null) auxObras = areaCuenta;

                if (vSesiones.sesionEmpresaActual == 2)
                {
                    var auxFacturasConcSalida = _context.tblM_CapEncConciliacionHorometros.Where(x => x.estatus == 1 && x.facturado && x.fechaFin.Month == 12 && x.fechaFin < fechaInicio).ToList();
                    List<string> auxFacturasSalida = new List<string>();
                    foreach (var item in auxFacturasConcSalida) { auxFacturasSalida.AddRange(JsonConvert.DeserializeObject<List<string>>(item.factura)); }
                    auxFacturasSalida = auxFacturasSalida.Distinct().ToList();
                    auxFacturasSalida.Remove("0");
                    if (auxFacturasSalida.Count() < 1) auxFacturasSalida.Add("-1");
                }
                var maquinasModelo = modelos == null ? new List<string>() : (vSesiones.sesionEmpresaActual == 2 ? _context.tblM_CatMaquina.Where(x => modelos.Contains(x.modeloEquipoID)) : _db.tblM_CatMaquina.Where(x => modelos.Contains(x.modeloEquipoID))).Select(x => x.noEconomico).ToList();
                List<CortePpalDTO> detalles = new List<CortePpalDTO>();

                IQueryable<CortePpalDTO> auxDetalles;

                for (int i = 0; i < cortes.Count(); i++)
                {
                    var corteID = cortes[i];
                    var corte = _context.tblM_KBCorte.FirstOrDefault(x => x.id == corteID);
                    var fechaFin = fechasFin[i];
                    IQueryable<tblM_KBCorteDet> auxDataSemAnterior = null;

                    IQueryable<tblM_KBCorteDet> auxDetallesGrouped = from x in _context.tblM_KBCorteDet.AsQueryable()
                                                                     where x.cc != null && (reporteCostos ? (x.cuenta.Contains("5000-") || x.cuenta.Contains("5280-") || x.cuenta == "1-4-0") : true) && (x.corteID == corteID) && x.fechapol <= fechaFin && x.fechapol >= fechaInicio
                                                                         && (auxObras.Count() > 0 ? vSesiones.sesionEmpresaActual == 2 ? (((auxObras.Contains("6-5")) ? (auxObras.Contains(x.areaCuenta) || x.cuenta.Contains("4000-6-")) : (auxObras.Contains("9-27") ? (auxObras.Contains(x.areaCuenta) || x.cuenta.Contains("4000-5-")) : (auxObras.Contains(x.areaCuenta) && !x.cuenta.Contains("4000-5-") && !x.cuenta.Contains("4000-6-"))))) : (auxObras.Contains(x.areaCuenta)) : true)
                                                                         && (maquinasModelo.Count() > 0 ? maquinasModelo.Contains(x.cc) : true) && ((economico == null || economico == "") ? true : x.cc == economico) && !(x.referencia != null && x.referencia.Contains("/" + (fechaInicio.Year - 1).ToString())
                                                                         && x.fechapol.Year == fechaInicio.Year) /*&& (vSesiones.sesionEmpresaActual == 1 ? !estimados.Contains(x.cuenta) : true)*/// && !x.cuenta.Contains("4000-5-") && !x.cuenta.Contains("4000-6-")
                                                                     select x;
                    #region EMPRESA ACTUAL
                    if (vSesiones.sesionEmpresaActual == 1) { auxDetallesGrouped = auxDetallesGrouped.Where(x => !(ccAcumulado2020.Contains(x.areaCuenta) && x.fechapol <= fechaInicioAcumulado2020)); }
                    if (vSesiones.sesionEmpresaActual == 1) { auxDetallesGrouped = auxDetallesGrouped.Where(x => !(ccActual.Contains(x.areaCuenta) && x.fechapol <= fechaInicioActual)); }

                    auxDataSemAnterior = auxDetallesGrouped;
                    #endregion

                    #region ENTRA EN EL IF

                    if (i > 0 && dataSemAnterior != null)
                    {
                        auxDetalles = auxDetallesGrouped.GroupJoin(dataSemAnterior, x => new { x.poliza, x.linea, x.monto, x.areaCuenta }, y => new { y.poliza, y.linea, y.monto, y.areaCuenta }, (x, y) => new { x, y })
                            .Select(z => new CorteDTO
                            {
                                id = z.y.Count() > 0 ? 0 : 1,
                                corteID = z.x.corteID,
                                poliza = z.x.poliza,
                                cuenta = z.x.cuenta,
                                concepto = z.x.concepto,
                                monto = z.x.monto,
                                cc = z.x.cc,
                                areaCuenta = z.x.cuenta.Contains("4000-6-") ? "6-5" : (z.x.cuenta.Contains("4000-5-") ? "9-27" : z.x.areaCuenta),
                                fechapol = z.x.fechapol,
                                tipoEquipo = z.x.tipoEquipo,
                                referencia = z.x.referencia,
                                empresa = z.x.empresa,
                                linea = z.x.linea
                            }).GroupBy(x => new { x.areaCuenta, x.cuenta, x.cc, x.tipoEquipo, x.referencia, x.empresa, x.id, x.fechapol })
                            .Select(grouped => new CortePpalDTO
                            {
                                cuenta = grouped.Key.cuenta,
                                monto = grouped.Sum(y => y.monto) * (grouped.Key.cuenta != "1-1-0" && grouped.Key.cuenta != "1-2-1" && grouped.Key.cuenta != "1-2-2" && grouped.Key.cuenta != "1-2-3" && grouped.Key.cuenta != "1-3-1" && grouped.Key.cuenta != "1-3-2" && !reporteCostos ? (-1) : (1)),
                                tipoEquipo = grouped.Key.tipoEquipo,
                                cc = grouped.Key.cc,
                                semana = 6 - i,
                                areaCuenta = grouped.Key.areaCuenta,
                                referencia = grouped.Key.referencia,
                                empresa = grouped.Key.empresa,
                                tipoMov = grouped.Key.id,
                                acumulado = grouped.Key.fechapol <= fechaInicioAnio
                            }).GroupBy(x => new { x.cuenta, x.tipoEquipo, x.cc, x.semana, x.areaCuenta, x.referencia, x.empresa, x.tipoMov, x.acumulado })
                            .Select(grouped => new CortePpalDTO
                            {
                                cuenta = grouped.Key.cuenta,
                                monto = grouped.Sum(y => y.monto),
                                tipoEquipo = grouped.Key.tipoEquipo,
                                cc = grouped.Key.cc,
                                semana = grouped.Key.semana,
                                areaCuenta = grouped.Key.areaCuenta,
                                referencia = grouped.Key.referencia,
                                empresa = grouped.Key.empresa,
                                tipoMov = grouped.Key.tipoMov,
                                acumulado = grouped.Key.acumulado
                            });


                        detalles.AddRange(auxDetalles);

                        auxDetalles = dataSemAnterior.GroupJoin(auxDetallesGrouped, x => new { x.poliza, x.linea, x.monto, x.areaCuenta }, y => new { y.poliza, y.linea, y.monto, y.areaCuenta }, (x, y) => new { x, y }).Where(e => e.y.Count() < 1)
                            .Select(z => new CorteDTO
                            {
                                corteID = z.x.corteID,
                                poliza = z.x.poliza,
                                cuenta = z.x.cuenta,
                                concepto = z.x.concepto,
                                monto = (z.x.monto) * (-1),
                                cc = z.x.cc,
                                areaCuenta = z.x.cuenta.Contains("4000-6-") ? "6-5" : (z.x.cuenta.Contains("4000-5-") ? "9-27" : z.x.areaCuenta),
                                fechapol = z.x.fechapol,
                                tipoEquipo = z.x.tipoEquipo,
                                referencia = z.x.referencia,
                                empresa = z.x.empresa,
                                linea = z.x.linea
                            }).GroupBy(x => new { x.areaCuenta, x.cuenta, x.cc, x.tipoEquipo, x.referencia, x.empresa, x.fechapol })
                            .Select(grouped => new CortePpalDTO
                            {
                                cuenta = grouped.Key.cuenta,
                                monto = grouped.Sum(y => y.monto) * (grouped.Key.cuenta != "1-1-0" && grouped.Key.cuenta != "1-2-1" && grouped.Key.cuenta != "1-2-2" && grouped.Key.cuenta != "1-2-3" && grouped.Key.cuenta != "1-3-1" && grouped.Key.cuenta != "1-3-2" && !reporteCostos ? (-1) : (1)),
                                tipoEquipo = grouped.Key.tipoEquipo,
                                cc = grouped.Key.cc,
                                semana = 6 - i,
                                areaCuenta = grouped.Key.areaCuenta,
                                referencia = grouped.Key.referencia,
                                empresa = grouped.Key.empresa,
                                tipoMov = 2,
                                acumulado = grouped.Key.fechapol < fechaInicioAnio
                            }).GroupBy(x => new { x.cuenta, x.tipoEquipo, x.cc, x.semana, x.areaCuenta, x.referencia, x.empresa, x.tipoMov, x.acumulado })
                            .Select(grouped => new CortePpalDTO
                            {
                                cuenta = grouped.Key.cuenta,
                                monto = grouped.Sum(y => y.monto),
                                tipoEquipo = grouped.Key.tipoEquipo,
                                cc = grouped.Key.cc,
                                semana = grouped.Key.semana,
                                areaCuenta = grouped.Key.areaCuenta,
                                referencia = grouped.Key.referencia,
                                empresa = grouped.Key.empresa,
                                tipoMov = grouped.Key.tipoMov,
                                acumulado = grouped.Key.acumulado
                            });

                        detalles.AddRange(auxDetalles);
                    }
                    #endregion

                    #region ELSE
                    else
                    {
                        auxDetalles = auxDetallesGrouped.Select(z => new CorteDTO
                        {
                            corteID = z.corteID,
                            poliza = z.poliza,
                            cuenta = z.cuenta,
                            concepto = z.concepto,
                            monto = (z.monto) * (-1),
                            cc = z.cc,
                            areaCuenta = z.cuenta.Contains("4000-6-") ? "6-5" : (z.cuenta.Contains("4000-5-") ? "9-27" : z.areaCuenta),
                            fechapol = z.fechapol,
                            tipoEquipo = z.tipoEquipo,
                            referencia = z.referencia,
                            empresa = z.empresa,
                            linea = z.linea
                        }).GroupBy(x => new { x.areaCuenta, x.cuenta, x.cc, x.tipoEquipo, x.referencia, x.empresa, x.fechapol })
                            .Select(grouped => new CortePpalDTO
                            {
                                cuenta = grouped.Key.cuenta,
                                monto = grouped.Sum(y => y.monto) * (grouped.Key.cuenta != "1-1-0" && grouped.Key.cuenta != "1-2-1" && grouped.Key.cuenta != "1-2-2" && grouped.Key.cuenta != "1-2-3" && grouped.Key.cuenta != "1-3-1" && grouped.Key.cuenta != "1-3-2" && !reporteCostos ? (-1) : (1)),
                                tipoEquipo = grouped.Key.tipoEquipo,
                                cc = grouped.Key.cc,
                                semana = 6 - i,
                                areaCuenta = grouped.Key.areaCuenta,
                                referencia = grouped.Key.referencia,
                                empresa = grouped.Key.empresa,
                                tipoMov = 0,
                                acumulado = grouped.Key.fechapol < fechaInicioAnio
                            }).GroupBy(x => new { x.cuenta, x.tipoEquipo, x.cc, x.semana, x.areaCuenta, x.referencia, x.empresa, x.tipoMov, x.acumulado })
                            .Select(grouped => new CortePpalDTO
                            {
                                cuenta = grouped.Key.cuenta,
                                monto = grouped.Sum(y => y.monto),
                                tipoEquipo = grouped.Key.tipoEquipo,
                                cc = grouped.Key.cc,
                                semana = grouped.Key.semana,
                                areaCuenta = grouped.Key.areaCuenta,
                                referencia = grouped.Key.referencia,
                                empresa = grouped.Key.empresa,
                                tipoMov = grouped.Key.tipoMov,
                                acumulado = grouped.Key.acumulado
                            });
                        if (i > 0) detalles.AddRange(auxDetalles.ToList());
                    }

                    #endregion

                    #region ARRENDADORA
                    if (corte != null && vSesiones.sesionEmpresaActual == 2)
                    {
                        var corteOtraEmpresa = _db.tblM_KBCorte.FirstOrDefault(x => x.fechaCorte == corte.fechaCorte && x.tipo == corte.tipo);
                        List<tblM_KBCorteDet> auxDetallesCplanGrouped = new List<tblM_KBCorteDet>();/* (from x in _context.tblM_KBCorteDet.AsQueryable()
                                                                         where x.cc != null && (reporteCostos ? (x.cuenta.Contains("5000-") || x.cuenta.Contains("5280-") || x.cuenta == "1-4-0") : true) && (x.corteID == corteID) && x.fechapol <= fechaFin && x.fechapol >= fechaInicio
                                                                             && (auxObras.Count() > 0 ? vSesiones.sesionEmpresaActual == 2 ? (((auxObras.Contains("6-5")) ? (auxObras.Contains(x.areaCuenta) || x.cuenta.Contains("4000-6-")) : (auxObras.Contains("9-27") ? (auxObras.Contains(x.areaCuenta) || x.cuenta.Contains("4000-5-")) : (auxObras.Contains(x.areaCuenta) && !x.cuenta.Contains("4000-5-") && !x.cuenta.Contains("4000-6-"))))) : (auxObras.Contains(x.areaCuenta)) : true)
                                                                             && (maquinasModelo.Count() > 0 ? maquinasModelo.Contains(x.cc) : true) && ((economico == null || economico == "") ? true : x.cc == economico) && !(x.referencia != null && x.referencia.Contains("/" + (fechaInicio.Year - 1).ToString())
                                                                             && x.fechapol.Year == fechaInicio.Year) && x.empresa == 1// && !x.cuenta.Contains("4000-5-") && !x.cuenta.Contains("4000-6-")
                                                                         select x).ToList();*/


                        if (corteOtraEmpresa != null)
                        {
                            List<tblM_KBCorteDet> auxDetallesBSCplan = (from x in _db.tblM_KBCorteDet.AsQueryable()
                                                                        where x.cc != null && (reporteCostos ? (x.cuenta.Contains("5000-") || x.cuenta.Contains("5280-") || x.cuenta == "1-4-0") : true) && (x.corteID == corteOtraEmpresa.id) && x.fechapol <= fechaFin && x.fechapol >= fechaInicio
                                                                        && (auxObras.Count() > 0 ? auxObras.Contains(x.areaCuenta) : true) && (maquinasModelo.Count() > 0 ? maquinasModelo.Contains(x.cc) : true)
                                                                        && ((economico == null || economico == "") ? true : x.cc == economico) && !(x.referencia != null && x.referencia.Contains("/" + (fechaInicio.Year - 1).ToString())
                                                                        && x.fechapol.Year == fechaInicio.Year) && !x.cuenta.Contains("4000-5-") && !x.cuenta.Contains("4000-6-") && (x.cuenta.Contains("5000-10-") || x.cuenta.Contains("5900-3-") || x.cuenta.Contains("5280-10-"))
                                                                        select x).ToList();
                            auxDetallesCplanGrouped.AddRange(auxDetallesBSCplan);
                        }
                        if (i > 0 && dataSemAntOtraEmp != null)
                        {
                            var auxDetallesCplanF = auxDetallesCplanGrouped.GroupJoin(dataSemAntOtraEmp, x => new { x.poliza, x.linea, x.monto }, y => new { y.poliza, y.linea, y.monto }, (x, y) => new { x, y })
                            .Select(z => new CorteDTO
                            {
                                id = z.y.Count() > 0 ? 0 : 1,
                                corteID = z.x.corteID,
                                poliza = z.x.poliza,
                                cuenta = z.x.cuenta,
                                concepto = z.x.concepto,
                                monto = z.x.monto,
                                cc = z.x.cc,
                                areaCuenta = z.x.areaCuenta,
                                fechapol = z.x.fechapol,
                                tipoEquipo = z.x.tipoEquipo,
                                referencia = z.x.referencia,
                                empresa = z.x.empresa,
                                linea = z.x.linea
                            }).GroupBy(x => new { x.areaCuenta, x.cuenta, x.cc, x.tipoEquipo, x.referencia, x.empresa, x.id, x.fechapol })
                            .Select(grouped => new CortePpalDTO
                            {
                                cuenta = grouped.Key.cuenta,
                                monto = grouped.Sum(y => y.monto) * (grouped.Key.cuenta != "1-1-0" && grouped.Key.cuenta != "1-2-1" && grouped.Key.cuenta != "1-2-2" && grouped.Key.cuenta != "1-2-3" && grouped.Key.cuenta != "1-3-1" && grouped.Key.cuenta != "1-3-2" && !reporteCostos ? (-1) : (1)),
                                tipoEquipo = grouped.Key.tipoEquipo,
                                cc = grouped.Key.cc,
                                semana = 6 - i,
                                areaCuenta = grouped.Key.areaCuenta,
                                referencia = grouped.Key.referencia,
                                empresa = grouped.Key.empresa,
                                tipoMov = grouped.Key.id,
                                acumulado = grouped.Key.fechapol < fechaInicioAnio
                            }).GroupBy(x => new { x.cuenta, x.tipoEquipo, x.cc, x.semana, x.areaCuenta, x.referencia, x.empresa, x.tipoMov, x.acumulado })
                            .Select(grouped => new CortePpalDTO
                            {
                                cuenta = grouped.Key.cuenta,
                                monto = grouped.Sum(y => y.monto),
                                tipoEquipo = grouped.Key.tipoEquipo,
                                cc = grouped.Key.cc,
                                semana = grouped.Key.semana,
                                areaCuenta = grouped.Key.areaCuenta,
                                referencia = grouped.Key.referencia,
                                empresa = grouped.Key.empresa,
                                tipoMov = grouped.Key.tipoMov,
                                acumulado = grouped.Key.acumulado
                            });

                            detalles.AddRange(auxDetallesCplanF);

                            auxDetallesCplanF = dataSemAntOtraEmp.GroupJoin(auxDetallesCplanGrouped, x => new { x.poliza, x.linea, x.monto }, y => new { y.poliza, y.linea, y.monto }, (x, y) => new { x, y }).Where(e => e.y.Count() < 1)
                            .Select(z => new CorteDTO
                            {
                                corteID = z.x.corteID,
                                poliza = z.x.poliza,
                                cuenta = z.x.cuenta,
                                concepto = z.x.concepto,
                                monto = (z.x.monto) * (-1),
                                cc = z.x.cc,
                                areaCuenta = z.x.areaCuenta,
                                fechapol = z.x.fechapol,
                                tipoEquipo = z.x.tipoEquipo,
                                referencia = z.x.referencia,
                                empresa = z.x.empresa,
                                linea = z.x.linea
                            }).GroupBy(x => new { x.areaCuenta, x.cuenta, x.cc, x.tipoEquipo, x.referencia, x.empresa, x.fechapol })
                            .Select(grouped => new CortePpalDTO
                            {
                                cuenta = grouped.Key.cuenta,
                                monto = grouped.Sum(y => y.monto) * (grouped.Key.cuenta != "1-1-0" && grouped.Key.cuenta != "1-2-1" && grouped.Key.cuenta != "1-2-2" && grouped.Key.cuenta != "1-2-3" && grouped.Key.cuenta != "1-3-1" && grouped.Key.cuenta != "1-3-2" && !reporteCostos ? (-1) : (1)),
                                tipoEquipo = grouped.Key.tipoEquipo,
                                cc = grouped.Key.cc,
                                semana = 6 - i,
                                areaCuenta = grouped.Key.areaCuenta,
                                referencia = grouped.Key.referencia,
                                empresa = grouped.Key.empresa,
                                tipoMov = 2,
                                acumulado = grouped.Key.fechapol < fechaInicioAnio
                            }).GroupBy(x => new { x.cuenta, x.tipoEquipo, x.cc, x.semana, x.areaCuenta, x.referencia, x.empresa, x.tipoMov, x.acumulado })
                            .Select(grouped => new CortePpalDTO
                            {
                                cuenta = grouped.Key.cuenta,
                                monto = grouped.Sum(y => y.monto),
                                tipoEquipo = grouped.Key.tipoEquipo,
                                cc = grouped.Key.cc,
                                semana = grouped.Key.semana,
                                areaCuenta = grouped.Key.areaCuenta,
                                referencia = grouped.Key.referencia,
                                empresa = grouped.Key.empresa,
                                tipoMov = grouped.Key.tipoMov,
                                acumulado = grouped.Key.acumulado
                            });

                            detalles.AddRange(auxDetallesCplanF);

                            if (auxDetallesCplanGrouped != null) dataSemAntOtraEmp = auxDetallesCplanGrouped;
                        }
                        else
                        {
                            var auxDetallesCplan = auxDetallesCplanGrouped.Select(z => new CorteDTO
                            {
                                corteID = z.corteID,
                                poliza = z.poliza,
                                cuenta = z.cuenta,
                                concepto = z.concepto,
                                monto = (z.monto) * (-1),
                                cc = z.cc,
                                areaCuenta = (z.cuenta.Contains("4000-6-") && vSesiones.sesionEmpresaActual == 2) ? "6-5" : ((z.cuenta.Contains("4000-5-") && vSesiones.sesionEmpresaActual == 2) ? "9-27" : z.areaCuenta),
                                fechapol = z.fechapol,
                                tipoEquipo = z.tipoEquipo,
                                referencia = z.referencia,
                                empresa = z.empresa,
                                linea = z.linea
                            }).GroupBy(x => new { x.areaCuenta, x.cuenta, x.cc, x.tipoEquipo, x.referencia, x.empresa, x.fechapol })
                            .Select(grouped => new CortePpalDTO
                            {
                                cuenta = grouped.Key.cuenta,
                                monto = grouped.Sum(y => y.monto) * (grouped.Key.cuenta != "1-1-0" && grouped.Key.cuenta != "1-2-1" && grouped.Key.cuenta != "1-2-2" && grouped.Key.cuenta != "1-2-3" && grouped.Key.cuenta != "1-3-1" && grouped.Key.cuenta != "1-3-2" && !reporteCostos ? (-1) : (1)),
                                tipoEquipo = grouped.Key.tipoEquipo,
                                cc = grouped.Key.cc,
                                semana = 6 - i,
                                areaCuenta = grouped.Key.areaCuenta,
                                referencia = grouped.Key.referencia,
                                empresa = grouped.Key.empresa,
                                tipoMov = 0,
                                acumulado = grouped.Key.fechapol < fechaInicioAnio
                            }).GroupBy(x => new { x.cuenta, x.tipoEquipo, x.cc, x.semana, x.areaCuenta, x.referencia, x.empresa, x.tipoMov, x.acumulado })
                            .Select(grouped => new CortePpalDTO
                            {
                                cuenta = grouped.Key.cuenta,
                                monto = grouped.Sum(y => y.monto),
                                tipoEquipo = grouped.Key.tipoEquipo,
                                cc = grouped.Key.cc,
                                semana = grouped.Key.semana,
                                areaCuenta = grouped.Key.areaCuenta,
                                referencia = grouped.Key.referencia,
                                empresa = grouped.Key.empresa,
                                tipoMov = grouped.Key.tipoMov,
                                acumulado = grouped.Key.acumulado
                            });
                            var DetallesCplan = auxDetallesCplan;
                            if (i > 0) detalles.AddRange(DetallesCplan.ToList());
                        }
                        dataSemAntOtraEmp = auxDetallesCplanGrouped;
                    }
                    #endregion
                    dataSemAnterior = auxDataSemAnterior;
                }
                if (vSesiones.sesionEmpresaActual == 1)
                {
                    foreach (var item in detalles)
                    {
                        var auxAreaCuentaCplan = centrosCostosEnkontrolCplan.FirstOrDefault(x => x.Prefijo == null ? x.Value == item.areaCuenta : x.Prefijo == item.areaCuenta);
                        if (auxAreaCuentaCplan == null)
                        {
                            var auxAreaCuentaArr = centrosCostosEnkontrolArr.FirstOrDefault(x => x.Value == item.areaCuenta);
                            item.areaCuenta = auxAreaCuentaArr == null ? item.areaCuenta : auxAreaCuentaArr.Text;
                        }
                        else { item.areaCuenta = auxAreaCuentaCplan.Text; }
                    }
                }
                else
                {
                    foreach (var item in detalles)
                    {
                        var auxAreaCuentaArr = centrosCostosEnkontrolArr.FirstOrDefault(x => x.Value == item.areaCuenta);
                        if (auxAreaCuentaArr == null)
                        {
                            var auxAreaCuentaCplan = centrosCostosEnkontrolCplan.FirstOrDefault(x => x.Value == item.areaCuenta);
                            item.areaCuenta = auxAreaCuentaCplan == null ? item.areaCuenta : auxAreaCuentaCplan.Text;
                        }
                        else { item.areaCuenta = auxAreaCuentaArr.Text; }
                    }
                }
                return detalles;
            }
        }

        public List<CortePpalDTO> getLstKubrixCortesAnteriores(int anio, List<string> areaCuenta, List<int> modelos, string economico, int usuarioID, bool reporteCostos)
        {
            var empresa = (int)EmpresaEnum.Arrendadora;
            if (vSesiones.sesionEmpresaActual == 2) { empresa = (int)EmpresaEnum.Construplan; }
            var centrosCostosEnkontrolArr = getListaCC().ToList();
            var centrosCostosEnkontrolCplan = getListaCCCPlan();
            List<string> estimados = (new string[] { "1-1-0", "1-2-1", "1-2-2", "1-2-3", "1-3-1", "1-3-2", "1-4-0" }).ToList();

            List<string> ccAcumulado2020 = (new string[] { "10-2", "10-3", "10-6", "10-7", "10-8", "10-9", "10-10", "11-1", "11-2", "10-12", "11-9", "10-24" }).ToList();
            DateTime fechaInicioAcumulado2020 = new DateTime(2020, 1, 1);


            List<string> ccActual = (new string[] { "009", "017", "018", "019", "028", "029", "557", "558", "562", "C65", "C66", "C69", "C71" }).ToList();
            DateTime fechaInicioActual = new DateTime(DateTime.Now.Year, 1, 1);

            using (var _db = new MainContext(empresa))
            {
                _context.Database.CommandTimeout = 300;
                _db.Database.CommandTimeout = 300;
                List<tblM_CatMaquina> maquinas = new List<tblM_CatMaquina>();
                if (vSesiones.sesionEmpresaActual == 2) { maquinas = _context.tblM_CatMaquina.ToList(); }
                else { maquinas = _db.tblM_CatMaquina.ToList(); }

                List<string> auxObras = new List<string>();
                if (areaCuenta != null) auxObras = areaCuenta;

                var maquinasModelo = modelos == null ? new List<string>() : (vSesiones.sesionEmpresaActual == 2 ? _context.tblM_CatMaquina.Where(x => modelos.Contains(x.modeloEquipoID)) : _db.tblM_CatMaquina.Where(x => modelos.Contains(x.modeloEquipoID))).Select(x => x.noEconomico).ToList();
                List<CortePpalDTO> detalles = new List<CortePpalDTO>();

                IQueryable<CortePpalDTO> auxDetalles;

                var cortes = _context.tblM_KBCorte.Where(x => x.anio <= anio && x.tipo == 2).Select(x => x.id).ToList();


                var auxDetallesGrouped = from x in _context.tblM_KBCorteDet.AsQueryable()
                                         where x.cc != null && (reporteCostos ? (x.cuenta.Contains("5000-") || x.cuenta.Contains("5280-") || x.cuenta == "1-4-0") : true) && (cortes.Contains(x.corteID))
                                             && (auxObras.Count() > 0 ? vSesiones.sesionEmpresaActual == 2 ? (((auxObras.Contains("6-5")) ? (auxObras.Contains(x.areaCuenta) || x.cuenta.Contains("4000-6-")) : (auxObras.Contains("9-27") ? (auxObras.Contains(x.areaCuenta) || x.cuenta.Contains("4000-5-")) : (auxObras.Contains(x.areaCuenta) && !x.cuenta.Contains("4000-5-") && !x.cuenta.Contains("4000-6-"))))) : (auxObras.Contains(x.areaCuenta)) : true)
                                             && (maquinasModelo.Count() > 0 ? maquinasModelo.Contains(x.cc) : true) && ((economico == null || economico == "") ? true : x.cc == economico) && x.empresa == vSesiones.sesionEmpresaActual// && !x.cuenta.Contains("4000-5-") && !x.cuenta.Contains("4000-6-")

                                         select x;
                if (vSesiones.sesionEmpresaActual == 1) { auxDetallesGrouped = auxDetallesGrouped.Where(x => !(ccAcumulado2020.Contains(x.areaCuenta) && x.fechapol <= fechaInicioAcumulado2020)); }
                if (vSesiones.sesionEmpresaActual == 1) { auxDetallesGrouped = auxDetallesGrouped.Where(x => !(ccActual.Contains(x.areaCuenta) && x.fechapol <= fechaInicioActual)); }
                auxDetalles = auxDetallesGrouped
                .Select(z => new CorteDTO
                {
                    corteID = z.corteID,
                    poliza = z.poliza,
                    cuenta = z.cuenta,
                    concepto = z.concepto,
                    monto = z.monto,
                    cc = z.cc,
                    areaCuenta = (z.cuenta.Contains("4000-6-") && vSesiones.sesionEmpresaActual == 2) ? "6-5" : ((z.cuenta.Contains("4000-5-") && vSesiones.sesionEmpresaActual == 2) ? "9-27" : z.areaCuenta),
                    fechapol = z.fechapol,
                    tipoEquipo = z.tipoEquipo,
                    referencia = z.referencia,
                    empresa = z.empresa,
                    linea = z.linea
                }).GroupBy(x => new { x.areaCuenta, x.cuenta, x.cc, x.tipoEquipo, x.referencia, x.empresa })
                .Select(grouped => new CortePpalDTO
                {
                    cuenta = grouped.Key.cuenta,
                    monto = grouped.Sum(y => y.monto) * (grouped.Key.cuenta != "1-1-0" && grouped.Key.cuenta != "1-2-1" && grouped.Key.cuenta != "1-2-2" && grouped.Key.cuenta != "1-2-3" && grouped.Key.cuenta != "1-3-1" && grouped.Key.cuenta != "1-3-2" && !reporteCostos ? (-1) : (1)),
                    tipoEquipo = grouped.Key.tipoEquipo,
                    cc = grouped.Key.cc,
                    semana = 6,
                    areaCuenta = grouped.Key.areaCuenta,
                    referencia = grouped.Key.referencia,
                    empresa = grouped.Key.empresa,
                    tipoMov = 0
                });

                detalles.AddRange(auxDetalles.ToList());

                if (vSesiones.sesionEmpresaActual == 1)
                {
                    foreach (var item in detalles)
                    {
                        var auxAreaCuentaCplan = centrosCostosEnkontrolCplan.FirstOrDefault(x => x.Prefijo == null ? x.Value == item.areaCuenta : x.Prefijo == item.areaCuenta);
                        if (auxAreaCuentaCplan == null)
                        {
                            var auxAreaCuentaArr = centrosCostosEnkontrolArr.FirstOrDefault(x => x.Value == item.areaCuenta);
                            item.areaCuenta = auxAreaCuentaArr == null ? item.areaCuenta : auxAreaCuentaArr.Text;
                        }
                        else { item.areaCuenta = auxAreaCuentaCplan.Text; }
                    }
                }
                else
                {
                    foreach (var item in detalles)
                    {
                        var auxAreaCuentaArr = centrosCostosEnkontrolArr.FirstOrDefault(x => x.Value == item.areaCuenta);
                        if (auxAreaCuentaArr == null)
                        {
                            var auxAreaCuentaCplan = centrosCostosEnkontrolCplan.FirstOrDefault(x => x.Value == item.areaCuenta);
                            item.areaCuenta = auxAreaCuentaCplan == null ? item.areaCuenta : auxAreaCuentaCplan.Text;
                        }
                        else { item.areaCuenta = auxAreaCuentaArr.Text; }
                    }
                }
                return detalles;
            }
        }

        public List<ComboDTO> getEconomicoEstatus(List<string> economicos)
        {
            var maquinas = _context.tblM_CatMaquina.Where(x => economicos.Contains(x.noEconomico)).Select(x => new ComboDTO
            {
                Value = x.noEconomico,
                Text = x.estatus.ToString(),
                Prefijo = x.centro_costos
            }).ToList();
            return maquinas;
        }

        public List<CorteDTO> getLstKubrixCorteDet(int corteID, int tipo, int columna, int renglon, List<int> modelos, string economico, DateTime fechaInicio, DateTime fechaFin, List<string> areaCuenta, int semana, int usuarioID, string divisionCol, string areaCuentaCol, string economicoCol, bool reporteCostos)
        {
            var empresa = (int)EmpresaEnum.Arrendadora;
            if (vSesiones.sesionEmpresaActual == 2) { empresa = (int)EmpresaEnum.Construplan; }
            var corte = _context.tblM_KBCorte.FirstOrDefault(x => x.id == corteID);
            List<string> cuentasIngreso = (new string[] { "4000-", "4900-", "4901-", "1-1-0", "1-3-1", "1-3-2", "1-2-1", "1-2-2", "1-2-3" }).ToList();
            List<string> cuentasIngresoEstimado = (new string[] { "1-1-0", "1-3-1", "1-3-2" }).ToList();
            List<string> cuentasPorGenerar = (new string[] { "1-2-1", "1-2-2", "1-2-3" }).ToList();


            var centrosCostosEnkontrolArr = getListaCC().ToList();
            var centrosCostosEnkontrolCplan = getListaCCCPlan();

            using (var _db = new MainContext(empresa))
            {
                List<tblM_CatMaquina> maquinas = new List<tblM_CatMaquina>();
                if (vSesiones.sesionEmpresaActual == 2) { maquinas = _context.tblM_CatMaquina.ToList(); }
                else { maquinas = _db.tblM_CatMaquina.ToList(); }

                List<string> auxObras = new List<string>();
                if (areaCuenta != null)
                {
                    auxObras = areaCuenta;
                    areaCuenta.Remove("");
                }
                if (corteID == 20) { fechaFin = new DateTime(2020, 2, 29); }
                var auxFacturasConcSalida = _context.tblM_CapEncConciliacionHorometros.Where(x => x.estatus == 1 && x.facturado && x.fechaFin.Month == 12 && x.fechaFin < fechaInicio).ToList();
                List<string> auxFacturasSalida = new List<string>();
                foreach (var item in auxFacturasConcSalida) { auxFacturasSalida.AddRange(JsonConvert.DeserializeObject<List<string>>(item.factura)); }
                auxFacturasSalida = auxFacturasSalida.Distinct().ToList();
                auxFacturasSalida.Remove("0");
                if (auxFacturasSalida.Count() < 1) auxFacturasSalida.Add("-1");

                var maquinasModelo = modelos == null ? new List<string>() : (vSesiones.sesionEmpresaActual == 2 ? _context.tblM_CatMaquina.Where(x => modelos.Contains(x.modeloEquipoID)) : _db.tblM_CatMaquina.Where(x => modelos.Contains(x.modeloEquipoID))).Select(x => x.noEconomico).ToList();

                var auxDetallesRaw = from x in _context.tblM_KBCorteDet.AsQueryable()
                                     where x.cc != null && (x.corteID == corteID) && x.fechapol <= fechaFin && x.fechapol >= fechaInicio
                                         && (auxObras.Count() > 0 ? auxObras.Contains(x.areaCuenta) : true) && (maquinasModelo.Count() > 0 ? maquinasModelo.Contains(x.cc) : true)
                                         && ((economico == null || economico == "" || economico == "TODOS") ? true : x.cc == economico) && !(x.referencia != null && x.referencia.Contains("/" + (fechaInicio.Year - 1).ToString()) && x.fechapol.Year == fechaInicio.Year)
                                         && (vSesiones.sesionEmpresaActual == 2 ? !x.cuenta.Contains("4000-5-") && !x.cuenta.Contains("4000-6-") : true)
                                     select x;


                IQueryable<tblM_KBCorteDet> auxDetallesOtraEmpresaRaw = null;

                if (corte != null && vSesiones.sesionEmpresaActual == 2 && (renglon == -1 || renglon == 5 || renglon == 8 || renglon == 10))
                {
                    var corteOtraEmpresa = _db.tblM_KBCorte.FirstOrDefault(x => x.fechaCorte == corte.fechaCorte);
                    if (corteOtraEmpresa != null)
                    {
                        auxDetallesOtraEmpresaRaw = from x in _db.tblM_KBCorteDet.AsQueryable()
                                                    where x.cc != null && (x.corteID == corteOtraEmpresa.id) && x.fechapol <= fechaFin && x.fechapol >= fechaInicio
                                                        && (auxObras.Count() > 0 ? auxObras.Contains(x.areaCuenta) : true) && (maquinasModelo.Count() > 0 ? maquinasModelo.Contains(x.cc) : true)
                                                        && ((economico == null || economico == "" || economico == "TODOS") ? true : x.cc == economico) && !(x.referencia != null && x.referencia.Contains("/" + (fechaInicio.Year - 1).ToString()) && x.fechapol.Year == fechaInicio.Year)
                                                        && (vSesiones.sesionEmpresaActual == 2 ? !x.cuenta.Contains("4000-5-") && !x.cuenta.Contains("4000-6-") : true) && (x.cuenta.Contains("5000-10-") || x.cuenta.Contains("5900-3-") || x.cuenta.Contains("5280-10-"))
                                                    select x;
                    }
                }

                switch (renglon)
                {
                    case -2: auxDetallesRaw = auxDetallesRaw.Where(x => cuentasIngreso.Any(y => x.cuenta.Contains(y))); break;
                    case -1: auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta.Contains("5000-") || x.cuenta == "1-4-0" || x.cuenta.Contains("5280-") || x.cuenta.Contains("5900-") || x.cuenta.Contains("5901-")); break;
                    case 0: auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta.Contains("4000-")); break;
                    case 1: auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta == "1-1-0" || x.cuenta == "1-3-1" || x.cuenta == "1-3-2"); break;
                    case 2: auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta == "1-2-1" || x.cuenta == "1-2-2" || x.cuenta == "1-2-3"); break;
                    case 4: auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta.Contains("5000-") && !x.cuenta.Contains("5000-10-")); break;
                    case 5:
                        auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta.Contains("5000-10-"));
                        if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => x.cuenta.Contains("5000-10-")); }
                        break;
                    case 6: auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta == "1-4-0"); break;
                    case 8:
                        auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta.Contains("5280-"));
                        if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => x.cuenta.Contains("5280-10-")); }
                        break;
                    case 10:
                        auxDetallesRaw = auxDetallesRaw.Where(x => (reporteCostos ? x.cuenta.Contains("5900-") : x.cuenta.Contains("4900-")) || x.cuenta.Contains("5900-"));
                        if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => x.cuenta.Contains("5900-3-")); }
                        break;
                    case 12: auxDetallesRaw = auxDetallesRaw.Where(x => (reporteCostos ? x.cuenta.Contains("5901-") : x.cuenta.Contains("4901-")) || x.cuenta.Contains("5901-")); break;
                }

                if (divisionCol != "" && divisionCol != "ARRENDADORA")
                {
                    List<string> acAdministracion = (new string[] { "003", "D01", "0", "A05", "026", "524", "979", "990" }).ToList();
                    List<string> acAdministracionArr = (new string[] { "9-30", "14-1", "14-2", "15-1", "16-1", "16-2", "16-3", "988", "994", "997" }).ToList();
                    if (divisionCol == "SIN DIVISION")
                    {
                        var divisiones = _context.tblM_KBDivision.Where(x => x.estatus).Select(x => x.divisionDetalle).ToList();
                        List<string> areasCuentaDivision = new List<string>();
                        foreach (var item in divisiones)
                        {
                            List<string> auxAC = item.Where(x => x.estatus).Select(x => x.ac.areaCuenta).ToList();
                            areasCuentaDivision.AddRange(auxAC);
                        }
                        areasCuentaDivision = areasCuentaDivision.Distinct().ToList();

                        areasCuentaDivision.AddRange(acAdministracion);
                        areasCuentaDivision.AddRange(acAdministracionArr);
                        if (areasCuentaDivision.Count() > 0)
                        {
                            auxDetallesRaw = auxDetallesRaw.Where(x => !areasCuentaDivision.Contains(x.areaCuenta));
                            if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => !areasCuentaDivision.Contains(x.areaCuenta)); }
                        }
                    }
                    else
                    {
                        var division = _context.tblM_KBDivision.FirstOrDefault(x => x.division == divisionCol);
                        if (division != null)
                        {
                            var areasCuentaDivision = division.divisionDetalle.Where(x => x.ac != null && x.estatus).Select(x => x.ac.areaCuenta).ToList();
                            //Administración
                            if (division.id == 7) { areasCuentaDivision.AddRange(acAdministracion); }
                            //Administracion arrendadora
                            if (division.id == 13) { areasCuentaDivision.AddRange(acAdministracionArr); }
                            if (areasCuentaDivision.Count() > 0)
                            {
                                auxDetallesRaw = auxDetallesRaw.Where(x => areasCuentaDivision.Contains(x.areaCuenta));
                                if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => areasCuentaDivision.Contains(x.areaCuenta)); }
                            }
                        }
                    }
                }
                if (areaCuentaCol != "" && areaCuentaCol != "ARRENDADORA" && areaCuentaCol != divisionCol)
                {
                    string areaCuentaSplit = areaCuentaCol.Split(' ')[0];
                    auxDetallesRaw = auxDetallesRaw.Where(x => x.areaCuenta == areaCuentaSplit);
                    if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => x.areaCuenta == areaCuentaSplit); }
                }
                if (economicoCol != "")
                {
                    auxDetallesRaw = auxDetallesRaw.Where(x => x.cc == economicoCol);
                    if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => x.cc == economicoCol); }
                }
                if (tipo == 4)
                {
                    auxDetallesRaw = auxDetallesRaw.Where(x => x.tipoEquipo == columna);
                    if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => x.tipoEquipo == columna); }
                }
                //
                List<CorteDTO> detalles = new List<CorteDTO>();
                var auxDetalles = auxDetallesRaw.GroupBy(x => new { x.cuenta, x.areaCuenta, x.referencia, x.cc, x.empresa }).Select(x => new CorteDTO()
                {
                    corteID = corteID,
                    cuenta = x.Key.cuenta,
                    monto = x.Sum(y => y.monto) * (x.Key.cuenta != "1-1-0" && x.Key.cuenta != "1-2-1" && x.Key.cuenta != "1-2-2" && x.Key.cuenta != "1-2-3" && x.Key.cuenta != "1-3-1" && x.Key.cuenta != "1-3-2" && !reporteCostos ? (-1) : (1)),
                    cc = x.Key.cc,
                    referencia = x.Key.referencia,
                    semana = semana,
                    empresa = x.Key.empresa,
                    areaCuenta = x.Key.areaCuenta,
                    tipoMov = 0
                });
                if (auxDetallesOtraEmpresaRaw != null)
                {
                    var auxDetallesOtraEmpresa = auxDetallesOtraEmpresaRaw.GroupBy(x => new { x.cuenta, x.areaCuenta, x.referencia, x.cc, x.empresa }).Select(x => new CorteDTO()
                    {
                        corteID = corteID,
                        cuenta = x.Key.cuenta,
                        monto = x.Sum(y => y.monto) * (x.Key.cuenta != "1-1-0" && x.Key.cuenta != "1-2-1" && x.Key.cuenta != "1-2-2" && x.Key.cuenta != "1-2-3" && x.Key.cuenta != "1-3-1" && x.Key.cuenta != "1-3-2" && !reporteCostos ? (-1) : (1)),
                        cc = x.Key.cc,
                        referencia = x.Key.referencia,
                        semana = semana,
                        empresa = x.Key.empresa,
                        areaCuenta = x.Key.areaCuenta,
                        tipoMov = 0
                    });
                    auxDetalles.Concat(auxDetallesOtraEmpresa);
                }
                //Agregar fletes y otr como areas cuenta
                if (vSesiones.sesionEmpresaActual == 2 && renglon == 0)
                {
                    if ((auxObras.Count() <= 0 && (divisionCol == "ARRENDADORA" || areaCuentaCol == "ARRENDADORA" || (divisionCol == "" && areaCuentaCol == "")))
                        || (auxObras.Contains("6-5") && (divisionCol == "ARRENDADORA" || areaCuentaCol == "ARRENDADORA" || (divisionCol == "" && areaCuentaCol == "")))
                        || divisionCol == "FLETES" || areaCuentaCol == "6-5 DEPARTAMENTO DE FLETES")
                    {
                        var fletesAux = from x in _context.tblM_KBCorteDet.AsQueryable()
                                        where x.cc != null && x.corteID == corteID && x.cuenta.Contains("4000-6-") && x.fechapol <= fechaFin && x.fechapol >= fechaInicio
                                            && (maquinasModelo.Count() > 0 ? maquinasModelo.Contains(x.cc) : true) && ((economico == null || economico == "" || economico == "TODOS") ? true : x.cc == economico) && !(x.referencia != null
                                            && x.referencia.Contains("/" + (fechaInicio.Year - 1).ToString()) && x.fechapol.Year == fechaInicio.Year)
                                        select x;
                        if (tipo == 4) { fletesAux = fletesAux.Where(x => x.tipoEquipo == columna); }
                        if (economicoCol != "") { fletesAux = fletesAux.Where(x => x.cc == economicoCol); }
                        var fletes = fletesAux.GroupBy(x => new { x.cuenta, x.areaCuenta, x.referencia, x.cc, x.empresa }).Select(x => new CorteDTO()
                        {
                            corteID = corteID,
                            cuenta = x.Key.cuenta,
                            monto = x.Sum(y => y.monto) * (x.Key.cuenta != "1-1-0" && x.Key.cuenta != "1-2-1" && x.Key.cuenta != "1-2-2" && x.Key.cuenta != "1-2-3" && x.Key.cuenta != "1-3-1" && x.Key.cuenta != "1-3-2" && !reporteCostos ? (-1) : (1)),
                            cc = x.Key.cc,
                            referencia = x.Key.referencia,
                            semana = semana,
                            empresa = x.Key.empresa,
                            areaCuenta = "6-5",
                            tipoMov = 0
                        });
                        auxDetalles.Concat(fletes);
                    }
                    if ((auxObras.Count() <= 0 && (divisionCol == "ARRENDADORA" || areaCuentaCol == "ARRENDADORA" || (divisionCol == "" && areaCuentaCol == "")))
                        || (auxObras.Contains("9-27") && (divisionCol == "ARRENDADORA" || areaCuentaCol == "ARRENDADORA" || (divisionCol == "" && areaCuentaCol == "")))
                        || divisionCol == "LLANTAS OTR" || areaCuentaCol == "9-27 LLANTAS OTR")
                    {
                        var otrAux = from x in _context.tblM_KBCorteDet.AsQueryable()
                                     where x.cc != null && x.corteID == corteID && x.cuenta.Contains("4000-5-") && x.fechapol <= fechaFin && x.fechapol >= fechaInicio
                                         && (maquinasModelo.Count() > 0 ? maquinasModelo.Contains(x.cc) : true) && ((economico == null || economico == "" || economico == "TODOS") ? true : x.cc == economico) && !(x.referencia != null
                                         && x.referencia.Contains("/" + (fechaInicio.Year - 1).ToString()) && x.fechapol.Year == fechaInicio.Year)
                                     select x;
                        if (tipo == 4) { otrAux = otrAux.Where(x => x.tipoEquipo == columna); }
                        if (economicoCol != "") { otrAux = otrAux.Where(x => x.cc == economicoCol); }
                        var otr = otrAux.GroupBy(x => new { x.cuenta, x.areaCuenta, x.referencia, x.cc, x.empresa }).Select(x => new CorteDTO()
                        {
                            corteID = corteID,
                            cuenta = x.Key.cuenta,
                            monto = x.Sum(y => y.monto) * (x.Key.cuenta != "1-1-0" && x.Key.cuenta != "1-2-1" && x.Key.cuenta != "1-2-2" && x.Key.cuenta != "1-2-3" && x.Key.cuenta != "1-3-1" && x.Key.cuenta != "1-3-2" && !reporteCostos ? (-1) : (1)),
                            cc = x.Key.cc,
                            referencia = x.Key.referencia,
                            semana = semana,
                            empresa = x.Key.empresa,
                            areaCuenta = "9-27",
                            tipoMov = 0
                        });
                        auxDetalles.Concat(otr);
                    }
                }
                if (vSesiones.sesionEmpresaActual == 1)
                {
                    foreach (var item in detalles)
                    {
                        var auxAreaCuentaCplan = centrosCostosEnkontrolCplan.FirstOrDefault(x => x.Prefijo == null ? x.Value == item.areaCuenta : x.Prefijo == item.areaCuenta);
                        if (auxAreaCuentaCplan == null)
                        {
                            var auxAreaCuentaArr = centrosCostosEnkontrolArr.FirstOrDefault(x => x.Value == item.areaCuenta);
                            item.areaCuenta = auxAreaCuentaArr == null ? item.areaCuenta : auxAreaCuentaArr.Text;
                        }
                        else { item.areaCuenta = auxAreaCuentaCplan.Text; }
                    }
                }
                else
                {
                    foreach (var item in detalles)
                    {
                        var auxAreaCuentaArr = centrosCostosEnkontrolArr.FirstOrDefault(x => x.Value == item.areaCuenta);
                        if (auxAreaCuentaArr == null)
                        {
                            var auxAreaCuentaCplan = centrosCostosEnkontrolCplan.FirstOrDefault(x => x.Value == item.areaCuenta);
                            item.areaCuenta = auxAreaCuentaCplan == null ? item.areaCuenta : auxAreaCuentaCplan.Text;
                        }
                        else { item.areaCuenta = auxAreaCuentaArr.Text; }
                    }
                }
                return auxDetalles.ToList();
            }
        }

        public List<CorteDTO> getLstKubrixCorteActualDet(int corteID, int tipo, int columna, int renglon, List<int> modelos, string economico, DateTime fechaInicio, DateTime fechaFin, List<string> areaCuenta, int semana, int usuarioID, List<CorteDTO> lstAnterior, string divisionCol, string areaCuentaCol, string economicoCol, bool reporteCostos)
        {
            var empresa = (int)EmpresaEnum.Arrendadora;
            if (vSesiones.sesionEmpresaActual == 2) { empresa = (int)EmpresaEnum.Construplan; }
            var corte = _context.tblM_KBCorte.FirstOrDefault(x => x.id == corteID);
            List<tblM_KBCorteDet> detallesTemporal = new List<tblM_KBCorteDet>();

            var centrosCostosEnkontrolArr = getListaCC().ToList();
            var centrosCostosEnkontrolCplan = getListaCCCPlan();
            using (var _db = new MainContext(empresa))
            {
                List<tblM_CatMaquina> maquinas = new List<tblM_CatMaquina>();
                if (vSesiones.sesionEmpresaActual == 2) { maquinas = _context.tblM_CatMaquina.ToList(); }
                else { maquinas = _db.tblM_CatMaquina.ToList(); }
                List<string> auxObras = new List<string>();
                if (areaCuenta != null)
                {
                    auxObras = areaCuenta;
                    areaCuenta.Remove("");
                }
                if (corteID == 20) { fechaFin = new DateTime(2020, 2, 29); }
                if (vSesiones.sesionEmpresaActual == 2)
                {
                    var auxFacturasConcSalida = _context.tblM_CapEncConciliacionHorometros.Where(x => x.estatus == 1 && x.facturado && x.fechaFin.Month == 12 && x.fechaFin < fechaInicio).ToList();
                    List<string> auxFacturasSalida = new List<string>();
                    foreach (var item in auxFacturasConcSalida) { auxFacturasSalida.AddRange(JsonConvert.DeserializeObject<List<string>>(item.factura)); }
                    auxFacturasSalida = auxFacturasSalida.Distinct().ToList();
                    auxFacturasSalida.Remove("0");
                    if (auxFacturasSalida.Count() < 1) auxFacturasSalida.Add("-1");
                }
                var maquinasModelo = modelos == null ? new List<string>() : (vSesiones.sesionEmpresaActual == 2 ? _context.tblM_CatMaquina.Where(x => modelos.Contains(x.modeloEquipoID)) : _db.tblM_CatMaquina.Where(x => modelos.Contains(x.modeloEquipoID))).Select(x => x.noEconomico).ToList();

                //var auxDetallesRaw = _context.tblM_KBCorteDet.Where(x => x.cc != null && ( x.corteID == corteID) && x.fechapol <= fechaFin && x.fechapol >= fechaInicio
                //    && (auxObras.Count() > 0 ? auxObras.Contains(x.areaCuenta) : true) && (maquinasModelo.Count() > 0 ? maquinasModelo.Contains(x.cc) : true)
                //    && ((economico == null || economico == "" || economico == "TODOS") ? true : x.cc == economico) && !(x.referencia != null && x.referencia.Contains("/" + (fechaInicio.Year - 1).ToString()) && x.fechapol.Year == fechaInicio.Year)
                //    && !x.cuenta.Contains("4000-5-") && !x.cuenta.Contains("4000-6-"));

                var auxDetallesRaw = from x in _context.tblM_KBCorteDet.AsQueryable()
                                     where x.cc != null && (x.corteID == corteID) && x.fechapol <= fechaFin && x.fechapol >= fechaInicio
                                         && (auxObras.Count() > 0 ? auxObras.Contains(x.areaCuenta) : true) && (maquinasModelo.Count() > 0 ? maquinasModelo.Contains(x.cc) : true)
                                         && ((economico == null || economico == "" || economico == "TODOS") ? true : x.cc == economico) && !(x.referencia != null && x.referencia.Contains("/" + (fechaInicio.Year - 1).ToString()) && x.fechapol.Year == fechaInicio.Year)
                                         && (vSesiones.sesionEmpresaActual == 2 ? !x.cuenta.Contains("4000-5-") && !x.cuenta.Contains("4000-6-") : true)
                                     select x;

                IQueryable<tblM_KBCorteDet> auxDetallesOtraEmpresaRaw = null;

                if (corte != null && vSesiones.sesionEmpresaActual == 2 && (renglon == -1 || renglon == 5 || renglon == 8 || renglon == 10))
                {
                    var corteOtraEmpresa = _db.tblM_KBCorte.FirstOrDefault(x => x.fechaCorte == corte.fechaCorte);
                    if (corteOtraEmpresa != null)
                    {
                        //auxDetallesOtraEmpresaRaw = _db.tblM_KBCorteDet.Where(x => x.cc != null && (x.corteID == corteOtraEmpresa.id) && x.fechapol <= fechaFin && x.fechapol >= fechaInicio
                        //    && (auxObras.Count() > 0 ? auxObras.Contains(x.areaCuenta) : true) && (maquinasModelo.Count() > 0 ? maquinasModelo.Contains(x.cc) : true)
                        //    && ((economico == null || economico == "" || economico == "TODOS") ? true : x.cc == economico) && !(x.referencia != null && x.referencia.Contains("/" + (fechaInicio.Year - 1).ToString()) && x.fechapol.Year == fechaInicio.Year)
                        //    && !x.cuenta.Contains("4000-5-") && !x.cuenta.Contains("4000-6-")).Where(x => x.cuenta.Contains("5000-10-") || x.cuenta.Contains("5900-3-") || x.cuenta.Contains("5280-10-"));

                        auxDetallesOtraEmpresaRaw = from x in _db.tblM_KBCorteDet.AsQueryable()
                                                    where x.cc != null && (x.corteID == corteOtraEmpresa.id) && x.fechapol <= fechaFin && x.fechapol >= fechaInicio
                                                        && (auxObras.Count() > 0 ? auxObras.Contains(x.areaCuenta) : true) && (maquinasModelo.Count() > 0 ? maquinasModelo.Contains(x.cc) : true)
                                                        && ((economico == null || economico == "" || economico == "TODOS") ? true : x.cc == economico) && !(x.referencia != null && x.referencia.Contains("/" + (fechaInicio.Year - 1).ToString()) && x.fechapol.Year == fechaInicio.Year)
                                                        && (vSesiones.sesionEmpresaActual == 2 ? !x.cuenta.Contains("4000-5-") && !x.cuenta.Contains("4000-6-") : true) && (x.cuenta.Contains("5000-10-") || x.cuenta.Contains("5900-3-") || x.cuenta.Contains("5280-10-"))
                                                    select x;
                    }
                }


                switch (renglon)
                {
                    case -2: auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta.Contains("4000-") || x.cuenta == "1-1-0" || x.cuenta == "1-3-1" || x.cuenta == "1-2-1" || x.cuenta == "1-2-2" || x.cuenta.Contains("4900-") || x.cuenta.Contains("4901-")); break;
                    case -1: auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta.Contains("5000-") || x.cuenta == "1-4-0" || x.cuenta.Contains("5280-") || x.cuenta.Contains("5900-") || x.cuenta.Contains("5901-")); break;
                    case 0: auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta.Contains("4000-")); break;
                    case 1: auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta == "1-1-0" || x.cuenta == "1-3-1" || x.cuenta == "1-3-2"); break;
                    case 2: auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta == "1-2-1" || x.cuenta == "1-2-2" || x.cuenta == "1-2-3"); break;
                    case 4: auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta.Contains("5000-") && !x.cuenta.Contains("5000-10-")); break;
                    case 5:
                        auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta.Contains("5000-10-"));
                        if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => x.cuenta.Contains("5000-10-")); }
                        break;
                    case 6: auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta == "1-4-0"); break;
                    case 8:
                        auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta.Contains("5280-"));
                        if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => x.cuenta.Contains("5280-10-")); }
                        break;
                    case 10:
                        auxDetallesRaw = auxDetallesRaw.Where(x => (reporteCostos ? x.cuenta.Contains("5900-") : x.cuenta.Contains("4900-")) || x.cuenta.Contains("5900-"));
                        if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => x.cuenta.Contains("5900-3-")); }
                        break;
                    case 12: auxDetallesRaw = auxDetallesRaw.Where(x => (reporteCostos ? x.cuenta.Contains("5901-") : x.cuenta.Contains("4901-")) || x.cuenta.Contains("5901-")); break;
                }

                if (divisionCol != "" && divisionCol != "ARRENDADORA")
                {
                    List<string> acAdministracion = (new string[] { "003", "D01", "0", "A05", "026", "524", "979", "990" }).ToList();
                    List<string> acAdministracionArr = (new string[] { "9-30", "14-1", "14-2", "15-1", "16-1", "16-2", "16-3", "988", "994", "997" }).ToList();
                    if (divisionCol == "SIN DIVISION")
                    {
                        var divisiones = _context.tblM_KBDivision.Where(x => x.estatus).Select(x => x.divisionDetalle).ToList();
                        List<string> areasCuentaDivision = new List<string>();
                        foreach (var item in divisiones)
                        {
                            List<string> auxAC = item.Where(x => x.ac != null && x.estatus).Select(x => x.ac.areaCuenta).ToList();
                            areasCuentaDivision.AddRange(auxAC);
                        }
                        areasCuentaDivision = areasCuentaDivision.Distinct().ToList();

                        areasCuentaDivision.AddRange(acAdministracion);
                        areasCuentaDivision.AddRange(acAdministracionArr);

                        if (areasCuentaDivision.Count() > 0)
                        {
                            auxDetallesRaw = auxDetallesRaw.Where(x => !areasCuentaDivision.Contains(x.areaCuenta));
                            if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => !areasCuentaDivision.Contains(x.areaCuenta)); }
                        }
                    }
                    else
                    {
                        var division = _context.tblM_KBDivision.FirstOrDefault(x => x.division == divisionCol);
                        if (division != null)
                        {
                            var areasCuentaDivision = division.divisionDetalle.Where(x => x.ac != null && x.estatus).Select(x => x.ac.areaCuenta).ToList();
                            //Administración
                            if (division.id == 7) { areasCuentaDivision.AddRange(acAdministracion); }
                            //Administracion arrendadora
                            if (division.id == 13) { areasCuentaDivision.AddRange(acAdministracionArr); }
                            if (areasCuentaDivision.Count() > 0)
                            {
                                auxDetallesRaw = auxDetallesRaw.Where(x => areasCuentaDivision.Contains(x.areaCuenta));
                                if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => areasCuentaDivision.Contains(x.areaCuenta)); }
                            }
                        }
                    }
                }
                if (areaCuentaCol != "" && areaCuentaCol != "ARRENDADORA" && areaCuentaCol != divisionCol)
                {
                    string areaCuentaSplit = areaCuentaCol.Split(' ')[0];
                    auxDetallesRaw = auxDetallesRaw.Where(x => x.areaCuenta == areaCuentaSplit);
                    if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => x.areaCuenta == areaCuentaSplit); }
                }
                if (economicoCol != "")
                {
                    auxDetallesRaw = auxDetallesRaw.Where(x => x.cc == economicoCol);
                    if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => x.cc == economicoCol); }
                }
                if (tipo == 4)
                {
                    auxDetallesRaw = auxDetallesRaw.Where(x => x.tipoEquipo == columna);
                    if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => x.tipoEquipo == columna); }
                }
                detallesTemporal = auxDetallesRaw.ToList();
                //Agregar fletes y otr como areas cuenta
                IQueryable<tblM_KBCorteDet> auxFletes = null;
                IQueryable<tblM_KBCorteDet> auxOtr = null;
                List<tblM_KBCorteDet> auxDetallesActualRaw = new List<tblM_KBCorteDet>();
                List<tblM_KBCorteDet> auxDetallesAnteriorRaw = new List<tblM_KBCorteDet>();
                List<CorteDTO> auxDetallesAnteriorNoActualRaw = new List<CorteDTO>();
                if (vSesiones.sesionEmpresaActual == 2 && renglon == 0)
                {
                    if ((auxObras.Count() <= 0 && (divisionCol == "ARRENDADORA" || areaCuentaCol == "ARRENDADORA" || (divisionCol == "" && areaCuentaCol == "")))
                        || (auxObras.Contains("6-5") && (divisionCol == "ARRENDADORA" || areaCuentaCol == "ARRENDADORA" || (divisionCol == "" && areaCuentaCol == "")))
                        || divisionCol == "FLETES" || areaCuentaCol == "6-5 DEPARTAMENTO DE FLETES")
                    {
                        //fletes = _context.tblM_KBCorteDet.Where(x => x.cc != null && x.corteID == corteID && x.cuenta.Contains("4000-6-") && x.fechapol <= fechaFin && x.fechapol >= fechaInicio
                        //    && (maquinasModelo.Count() > 0 ? maquinasModelo.Contains(x.cc) : true) && ((economico == null || economico == "" || economico == "TODOS") ? true : x.cc == economico) && !(x.referencia != null
                        //    && x.referencia.Contains("/" + (fechaInicio.Year - 1).ToString()) && x.fechapol.Year == fechaInicio.Year));
                        auxFletes = from x in _context.tblM_KBCorteDet.AsQueryable()
                                    where x.cc != null && x.corteID == corteID && x.cuenta.Contains("4000-6-") && x.fechapol <= fechaFin && x.fechapol >= fechaInicio
                                        && (maquinasModelo.Count() > 0 ? maquinasModelo.Contains(x.cc) : true) && ((economico == null || economico == "" || economico == "TODOS") ? true : x.cc == economico) && !(x.referencia != null
                                        && x.referencia.Contains("/" + (fechaInicio.Year - 1).ToString()) && x.fechapol.Year == fechaInicio.Year)
                                    select x;

                        var fletes = auxFletes.ToList();
                        if (economicoCol != "") { fletes = fletes.Where(x => x.cc == economicoCol).ToList(); }
                        if (tipo == 4) { fletes = fletes.Where(x => x.tipoEquipo == columna).ToList(); }
                        foreach (var item in fletes)
                        {
                            item.areaCuenta = "6-5";
                            //var existe = lstAnterior.FirstOrDefault(x => x.linea == item.linea && x.poliza == item.poliza);
                            //if (existe != null) auxDetallesActualRaw.Add(item);
                            //else { auxDetallesAnteriorRaw.Add(item); }
                        }
                        detallesTemporal.AddRange(fletes);

                    }
                    if ((auxObras.Count() <= 0 && (divisionCol == "ARRENDADORA" || areaCuentaCol == "ARRENDADORA" || (divisionCol == "" && areaCuentaCol == "")))
                        || (auxObras.Contains("9-27") && (divisionCol == "ARRENDADORA" || areaCuentaCol == "ARRENDADORA" || (divisionCol == "" && areaCuentaCol == "")))
                        || divisionCol == "LLANTAS OTR" || areaCuentaCol == "9-27 LLANTAS OTR")
                    {
                        //otr = _context.tblM_KBCorteDet.Where(x => x.cc != null && x.corteID == corteID && x.cuenta.Contains("4000-5-") && x.fechapol <= fechaFin && x.fechapol >= fechaInicio
                        //    && (maquinasModelo.Count() > 0 ? maquinasModelo.Contains(x.cc) : true) && ((economico == null || economico == "" || economico == "TODOS") ? true : x.cc == economico) && !(x.referencia != null
                        //    && x.referencia.Contains("/" + (fechaInicio.Year - 1).ToString()) && x.fechapol.Year == fechaInicio.Year));

                        auxOtr = from x in _context.tblM_KBCorteDet.AsQueryable()
                                 where x.cc != null && x.corteID == corteID && x.cuenta.Contains("4000-5-") && x.fechapol <= fechaFin && x.fechapol >= fechaInicio
                                     && (maquinasModelo.Count() > 0 ? maquinasModelo.Contains(x.cc) : true) && ((economico == null || economico == "" || economico == "TODOS") ? true : x.cc == economico) && !(x.referencia != null
                                     && x.referencia.Contains("/" + (fechaInicio.Year - 1).ToString()) && x.fechapol.Year == fechaInicio.Year)
                                 select x;

                        var otr = auxOtr.ToList();
                        if (economicoCol != "") { otr = otr.Where(x => x.cc == economicoCol).ToList(); }
                        if (tipo == 4) { otr = otr.Where(x => x.tipoEquipo == columna).ToList(); }
                        foreach (var item in otr)
                        {
                            item.areaCuenta = "9-27";

                            //var existe = lstAnterior.FirstOrDefault(x => x.linea == item.linea && x.poliza == item.poliza);
                            //if (existe != null) auxDetallesActualRaw.Add(item);
                            //else { auxDetallesAnteriorRaw.Add(item); }
                        }
                        detallesTemporal.AddRange(otr);
                    }
                }

                //auxDetallesActualRaw = auxDetallesRaw.Where(x => lstAnteriorID.Contains(x.poliza + " " + x.linea.ToString() + " " + x.monto.ToString())).ToList();
                //auxDetallesAnteriorRaw = auxDetallesRaw.Where(x => !lstAnteriorID.Contains(x.poliza + " " + x.linea.ToString() + " " + x.monto.ToString())).ToList();



                auxDetallesActualRaw = detallesTemporal.GroupJoin(lstAnterior, x => new { x.poliza, x.linea, x.monto }, y => new { y.poliza, y.linea, y.monto }, (x, y) => new { x, y })
                .Where(e => e.y.Any())
                .Select(e => e.x).ToList();

                auxDetallesAnteriorRaw = detallesTemporal.GroupJoin(lstAnterior, x => new { x.poliza, x.linea, x.monto }, y => new { y.poliza, y.linea, y.monto }, (x, y) => new { x, y })
                .Where(e => !e.y.Any())
                .Select(e => e.x).ToList();

                //foreach (var item in auxDetallesRaw)
                //{
                //    //var existe = lstAnterior.Any(x => x.linea == item.linea && x.poliza == item.poliza && x.monto == item.monto);

                //    var existe2 = lstAnterior.FirstOrDefault(x => x.linea == item.linea && x.poliza == item.poliza && x.monto == item.monto && x.cc == item.cc && x.concepto == item.concepto);
                //    //var existe = from x in lstAnterior.AsQueryable()
                //    //             where x.linea == item.linea && x.poliza == item.poliza && x.monto == item.monto
                //    //             select x.id;

                //    if (existe2 != null) { 
                //        auxDetallesActualRaw.Add(item);
                //        lstAnterior.Remove(existe2);
                //    }
                //    else { auxDetallesAnteriorRaw.Add(item); }
                //}

                if (auxDetallesOtraEmpresaRaw != null)
                {
                    var auxDetallesActualOtraRaw = auxDetallesOtraEmpresaRaw.ToList().GroupJoin(lstAnterior, x => new { x.poliza, x.linea, x.monto }, y => new { y.poliza, y.linea, y.monto }, (x, y) => new { x, y })
                        .Where(e => e.y.Any())
                        .Select(e => e.x).ToList();

                    var auxDetallesAnteriorOtraRaw = auxDetallesOtraEmpresaRaw.ToList().GroupJoin(lstAnterior, x => new { x.poliza, x.linea, x.monto }, y => new { y.poliza, y.linea, y.monto }, (x, y) => new { x, y })
                        .Where(e => !e.y.Any())
                        .Select(e => e.x).ToList();

                    auxDetallesActualRaw.AddRange(auxDetallesActualOtraRaw);
                    auxDetallesAnteriorRaw.AddRange(auxDetallesAnteriorOtraRaw);

                    //foreach (var item in auxDetallesOtraEmpresaRaw)
                    //{
                    //    //var existe = lstAnterior.FirstOrDefault(x => x.linea == item.linea && x.poliza == item.poliza && x.monto == item.monto);
                    //    var existe = from x in lstAnterior.AsQueryable()
                    //                 where x.linea == item.linea && x.poliza == item.poliza && x.monto == item.monto
                    //                 select x.linea;
                    //    if (existe.Any()) { auxDetallesActualRaw.Add(item); }
                    //    else { auxDetallesAnteriorRaw.Add(item); }



                    //}
                }
                auxDetallesAnteriorNoActualRaw = lstAnterior.GroupJoin(detallesTemporal, x => new { x.poliza, x.linea, x.monto }, y => new { y.poliza, y.linea, y.monto }, (x, y) => new { x, y })
                    .Where(e => !e.y.Any())
                    .Select(e => e.x).ToList();
                //foreach (var item in lstAnterior)
                //{


                //    ////var existe = auxDetallesActualRaw.FirstOrDefault(x => x.linea == item.linea && x.poliza == item.poliza && x.monto == item.monto);
                //    //var existe = from x in auxDetallesActualRaw.AsQueryable()
                //    //             where x.linea == item.linea && x.poliza == item.poliza && x.monto == item.monto
                //    //             select x.id;

                //    ////var existe2 = auxDetallesAnteriorRaw.FirstOrDefault(x => x.linea == item.linea && x.poliza == item.poliza && x.monto == item.monto);
                //    //var existe2 = from x in auxDetallesAnteriorRaw.AsQueryable()
                //    //              where x.linea == item.linea && x.poliza == item.poliza && x.monto == item.monto
                //    //              select x.id;
                //    //if (!existe.Any() && !existe2.Any()) { }//auxDetallesAnteriorNoActualRaw.Add(item);
                //}

                //auxDetallesAnteriorNoActualRaw.ForEach(x => x.tipoMov = 2);
                //auxDetallesAnteriorNoActualRaw.ForEach(x => x.monto = x.monto * (-1));


                List<CorteDTO> detallesActual = new List<CorteDTO>();
                List<CorteDTO> detallesAnterior = new List<CorteDTO>();
                List<CorteDTO> detalles = new List<CorteDTO>();
                var auxDetalles = auxDetallesActualRaw.GroupBy(x => new { x.cuenta, x.areaCuenta, x.referencia, x.cc, x.empresa }).ToList();
                detallesActual = auxDetalles.ToList().Select(x =>
                {
                    return new CorteDTO
                    {
                        corteID = corteID,
                        cuenta = x.Key.cuenta,
                        monto = x.Sum(y => y.monto) * (x.Key.cuenta != "1-1-0" && x.Key.cuenta != "1-2-1" && x.Key.cuenta != "1-2-2" && x.Key.cuenta != "1-2-3" && x.Key.cuenta != "1-3-1" && x.Key.cuenta != "1-3-2" && !reporteCostos ? (-1) : (1)),
                        cc = x.Key.cc,
                        referencia = x.Key.referencia,
                        semana = semana,
                        empresa = x.Key.empresa,
                        areaCuenta = x.Key.areaCuenta,
                        tipoMov = 0
                    };
                }).ToList();

                auxDetalles = auxDetallesAnteriorRaw.GroupBy(x => new { x.cuenta, x.areaCuenta, x.referencia, x.cc, x.empresa }).ToList();
                detallesAnterior = auxDetalles.ToList().Select(x =>
                {
                    return new CorteDTO
                    {
                        corteID = corteID,
                        cuenta = x.Key.cuenta,
                        monto = x.Sum(y => y.monto) * (x.Key.cuenta != "1-1-0" && x.Key.cuenta != "1-2-1" && x.Key.cuenta != "1-2-2" && x.Key.cuenta != "1-2-3" && x.Key.cuenta != "1-3-1" && x.Key.cuenta != "1-3-2" && !reporteCostos ? (-1) : (1)),
                        cc = x.Key.cc,
                        referencia = x.Key.referencia,
                        semana = semana,
                        empresa = x.Key.empresa,
                        areaCuenta = x.Key.areaCuenta,
                        tipoMov = 1
                    };
                }).ToList();
                detalles.AddRange(detallesActual);
                detalles.AddRange(detallesAnterior);
                detalles.AddRange(auxDetallesAnteriorNoActualRaw);

                if (vSesiones.sesionEmpresaActual == 1)
                {
                    foreach (var item in detalles)
                    {
                        var auxAreaCuentaCplan = centrosCostosEnkontrolCplan.FirstOrDefault(x => x.Prefijo == null ? x.Value == item.areaCuenta : x.Prefijo == item.areaCuenta);
                        if (auxAreaCuentaCplan == null)
                        {
                            var auxAreaCuentaArr = centrosCostosEnkontrolArr.FirstOrDefault(x => x.Value == item.areaCuenta);
                            item.areaCuenta = auxAreaCuentaArr == null ? item.areaCuenta : auxAreaCuentaArr.Text;
                        }
                        else { item.areaCuenta = auxAreaCuentaCplan.Text; }
                    }
                }
                else
                {
                    foreach (var item in detalles)
                    {
                        var auxAreaCuentaArr = centrosCostosEnkontrolArr.FirstOrDefault(x => x.Value == item.areaCuenta);
                        if (auxAreaCuentaArr == null)
                        {
                            var auxAreaCuentaCplan = centrosCostosEnkontrolCplan.FirstOrDefault(x => x.Value == item.areaCuenta);
                            item.areaCuenta = auxAreaCuentaCplan == null ? item.areaCuenta : auxAreaCuentaCplan.Text;
                        }
                        else { item.areaCuenta = auxAreaCuentaArr.Text; }
                    }
                }


                return detalles;
            }
        }

        public List<CorteDTO> getLstKubrixCorteDetCompleto(int corteID, int tipo, int columna, int renglon, List<int> modelos, string economico, DateTime fechaInicio, DateTime fechaFin, List<string> areaCuenta, int semana, int usuarioID, string divisionCol, string areaCuentaCol, string economicoCol, bool reporteCostos)
        {
            var empresa = (int)EmpresaEnum.Arrendadora;
            if (vSesiones.sesionEmpresaActual == 2) { empresa = (int)EmpresaEnum.Construplan; }
            var corte = _context.tblM_KBCorte.FirstOrDefault(x => x.id == corteID);

            var centrosCostosEnkontrolArr = getListaCC().ToList();
            var centrosCostosEnkontrolCplan = getListaCCCPlan();
            using (var _db = new MainContext(empresa))
            {
                List<tblM_CatMaquina> maquinas = new List<tblM_CatMaquina>();
                if (vSesiones.sesionEmpresaActual == 2) { maquinas = _context.tblM_CatMaquina.ToList(); }
                else { maquinas = _db.tblM_CatMaquina.ToList(); }

                List<string> auxObras = new List<string>();
                if (areaCuenta != null)
                {
                    auxObras = areaCuenta;
                    areaCuenta.Remove("");
                }
                if (corteID == 20) { fechaFin = new DateTime(2020, 2, 29); }
                var auxFacturasConcSalida = _context.tblM_CapEncConciliacionHorometros.Where(x => x.estatus == 1 && x.facturado && x.fechaFin.Month == 12 && x.fechaFin < fechaInicio).ToList();
                List<string> auxFacturasSalida = new List<string>();
                foreach (var item in auxFacturasConcSalida) { auxFacturasSalida.AddRange(JsonConvert.DeserializeObject<List<string>>(item.factura)); }
                auxFacturasSalida = auxFacturasSalida.Distinct().ToList();
                auxFacturasSalida.Remove("0");
                if (auxFacturasSalida.Count() < 1) auxFacturasSalida.Add("-1");

                var maquinasModelo = modelos == null ? new List<string>() : (vSesiones.sesionEmpresaActual == 2 ? _context.tblM_CatMaquina.Where(x => modelos.Contains(x.modeloEquipoID)) : _db.tblM_CatMaquina.Where(x => modelos.Contains(x.modeloEquipoID))).Select(x => x.noEconomico).ToList();

                var auxDetallesRaw = _context.tblM_KBCorteDet.Where(x => x.cc != null && (x.corteID == corteID) && x.fechapol <= fechaFin && x.fechapol >= fechaInicio
                    && (auxObras.Count() > 0 ? auxObras.Contains(x.areaCuenta) : true) && (maquinasModelo.Count() > 0 ? maquinasModelo.Contains(x.cc) : true)
                    && ((economico == null || economico == "" || economico == "TODOS") ? true : x.cc == economico) && !(x.referencia != null && x.referencia.Contains("/" + (fechaInicio.Year - 1).ToString()) && x.fechapol.Year == fechaInicio.Year)
                    && (vSesiones.sesionEmpresaActual == 2 ? !x.cuenta.Contains("4000-5-") && !x.cuenta.Contains("4000-6-") : true));

                IQueryable<tblM_KBCorteDet> auxDetallesOtraEmpresaRaw = null;

                if (corte != null && vSesiones.sesionEmpresaActual == 2 && (renglon == -1 || renglon == 5 || renglon == 8 || renglon == 10))
                {
                    var corteOtraEmpresa = _db.tblM_KBCorte.FirstOrDefault(x => x.fechaCorte == corte.fechaCorte);
                    if (corteOtraEmpresa != null)
                    {
                        auxDetallesOtraEmpresaRaw = _db.tblM_KBCorteDet.Where(x => x.cc != null && (x.corteID == corteOtraEmpresa.id) && x.fechapol <= fechaFin && x.fechapol >= fechaInicio
                            && (auxObras.Count() > 0 ? auxObras.Contains(x.areaCuenta) : true) && (maquinasModelo.Count() > 0 ? maquinasModelo.Contains(x.cc) : true)
                            && ((economico == null || economico == "" || economico == "TODOS") ? true : x.cc == economico) && !(x.referencia != null && x.referencia.Contains("/" + (fechaInicio.Year - 1).ToString()) && x.fechapol.Year == fechaInicio.Year)
                            && (vSesiones.sesionEmpresaActual == 2 ? !x.cuenta.Contains("4000-5-") && !x.cuenta.Contains("4000-6-") : true)).Where(x => x.cuenta.Contains("5000-10-") || x.cuenta.Contains("5900-3-") || x.cuenta.Contains("5280-10-"));

                    }
                }

                switch (renglon)
                {
                    case -2: auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta.Contains("4000-") || x.cuenta == "1-1-0" || x.cuenta == "1-3-1" || x.cuenta == "1-3-2" || x.cuenta == "1-2-1" || x.cuenta == "1-2-2" || x.cuenta == "1-2-3" || x.cuenta.Contains("4900-") || x.cuenta.Contains("4901-")); break;
                    case -1: auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta.Contains("5000-") || x.cuenta == "1-4-0" || x.cuenta.Contains("5280-") || x.cuenta.Contains("5900-") || x.cuenta.Contains("5901-")); break;
                    case 0: auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta.Contains("4000-")); break;
                    case 1: auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta == "1-1-0" || x.cuenta == "1-3-1" || x.cuenta == "1-3-2"); break;
                    case 2: auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta == "1-2-1" || x.cuenta == "1-2-2" || x.cuenta == "1-2-3"); break;
                    case 4: auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta.Contains("5000-") && !x.cuenta.Contains("5000-10-")); break;
                    case 5:
                        auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta.Contains("5000-10-"));
                        if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => x.cuenta.Contains("5000-10-")); }
                        break;
                    case 6: auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta == "1-4-0"); break;
                    case 8:
                        auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta.Contains("5280-"));
                        if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => x.cuenta.Contains("5280-10-")); }
                        break;
                    case 10:
                        auxDetallesRaw = auxDetallesRaw.Where(x => (reporteCostos ? x.cuenta.Contains("5900-") : x.cuenta.Contains("4900-")) || x.cuenta.Contains("5900-"));
                        if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => x.cuenta.Contains("5900-3-")); }
                        break;
                    case 12: auxDetallesRaw = auxDetallesRaw.Where(x => (reporteCostos ? x.cuenta.Contains("5901-") : x.cuenta.Contains("4901-")) || x.cuenta.Contains("5901-")); break;
                }

                if (divisionCol != "" && divisionCol != "ARRENDADORA")
                {
                    List<string> acAdministracion = (new string[] { "003", "D01", "0", "A05", "026", "524", "979", "990" }).ToList();
                    List<string> acAdministracionArr = (new string[] { "9-30", "14-1", "14-2", "15-1", "16-1", "16-2", "16-3", "988", "994", "997" }).ToList();
                    if (divisionCol == "SIN DIVISION")
                    {
                        var divisiones = _context.tblM_KBDivision.Where(x => x.estatus).Select(x => x.divisionDetalle).ToList();
                        List<string> areasCuentaDivision = new List<string>();
                        foreach (var item in divisiones)
                        {
                            List<string> auxAC = item.Where(x => x.estatus).Select(x => x.ac.areaCuenta).ToList();
                            areasCuentaDivision.AddRange(auxAC);
                        }
                        areasCuentaDivision = areasCuentaDivision.Distinct().ToList();

                        areasCuentaDivision.AddRange(acAdministracion);
                        areasCuentaDivision.AddRange(acAdministracionArr);
                        if (areasCuentaDivision.Count() > 0)
                        {
                            auxDetallesRaw = auxDetallesRaw.Where(x => !areasCuentaDivision.Contains(x.areaCuenta));
                            if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => !areasCuentaDivision.Contains(x.areaCuenta)); }
                        }
                    }
                    else
                    {
                        var division = _context.tblM_KBDivision.FirstOrDefault(x => x.division == divisionCol);
                        if (division != null)
                        {
                            var areasCuentaDivision = division.divisionDetalle.Where(x => x.ac != null && x.estatus).Select(x => x.ac.areaCuenta).ToList();
                            //Administración
                            if (division.id == 7) { areasCuentaDivision.AddRange(acAdministracion); }
                            //Administracion arrendadora
                            if (division.id == 13) { areasCuentaDivision.AddRange(acAdministracionArr); }
                            if (areasCuentaDivision.Count() > 0)
                            {
                                auxDetallesRaw = auxDetallesRaw.Where(x => areasCuentaDivision.Contains(x.areaCuenta));
                                if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => areasCuentaDivision.Contains(x.areaCuenta)); }
                            }
                        }
                    }
                }
                if (areaCuentaCol != "" && areaCuentaCol != "ARRENDADORA" && areaCuentaCol != divisionCol)
                {
                    string areaCuentaSplit = areaCuentaCol.Split(' ')[0];
                    auxDetallesRaw = auxDetallesRaw.Where(x => x.areaCuenta == areaCuentaSplit);
                    if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => x.areaCuenta == areaCuentaSplit); }
                }
                if (economicoCol != "")
                {
                    auxDetallesRaw = auxDetallesRaw.Where(x => x.cc == economicoCol);
                    if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => x.cc == economicoCol); }
                }
                if (tipo == 4)
                {
                    auxDetallesRaw = auxDetallesRaw.Where(x => x.tipoEquipo == columna);
                    if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => x.tipoEquipo == columna); }
                }
                //
                List<CorteDTO> detalles = new List<CorteDTO>();
                var detallesRaw = auxDetallesRaw.ToList();
                if (auxDetallesOtraEmpresaRaw != null)
                {
                    var detallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.ToList();
                    detallesRaw.AddRange(detallesOtraEmpresaRaw);
                }
                //Agregar fletes y otr como areas cuenta
                if (vSesiones.sesionEmpresaActual == 2 && (renglon == 0 || renglon == -2))
                {
                    if ((auxObras.Count() <= 0 && (divisionCol == "ARRENDADORA" || areaCuentaCol == "ARRENDADORA" || (divisionCol == "" && areaCuentaCol == "")))
                        || (auxObras.Contains("6-5") && (divisionCol == "ARRENDADORA" || areaCuentaCol == "ARRENDADORA" || (divisionCol == "" && areaCuentaCol == "")))
                        || divisionCol == "FLETES" || areaCuentaCol == "6-5 DEPARTAMENTO DE FLETES")
                    {
                        var fletes = _context.tblM_KBCorteDet.Where(x => x.cc != null && x.corteID == corteID && x.cuenta.Contains("4000-6-") && x.fechapol <= fechaFin && x.fechapol >= fechaInicio
                            && (maquinasModelo.Count() > 0 ? maquinasModelo.Contains(x.cc) : true) && ((economico == null || economico == "" || economico == "TODOS") ? true : x.cc == economico) && !(x.referencia != null
                            && x.referencia.Contains("/" + (fechaInicio.Year - 1).ToString()) && x.fechapol.Year == fechaInicio.Year));
                        foreach (var item in fletes) { item.areaCuenta = "6-5"; }
                        if (economicoCol != "") { fletes = fletes.Where(x => x.cc == economicoCol); }
                        if (tipo == 4) { fletes = fletes.Where(x => x.tipoEquipo == columna); }
                        var lstFletes = fletes.ToList();
                        detallesRaw.AddRange(lstFletes);
                    }
                    if ((auxObras.Count() <= 0 && (divisionCol == "ARRENDADORA" || areaCuentaCol == "ARRENDADORA" || (divisionCol == "" && areaCuentaCol == "")))
                        || (auxObras.Contains("9-27") && (divisionCol == "ARRENDADORA" || areaCuentaCol == "ARRENDADORA" || (divisionCol == "" && areaCuentaCol == "")))
                        || divisionCol == "LLANTAS OTR" || areaCuentaCol == "9-27 LLANTAS OTR")
                    {
                        var otr = _context.tblM_KBCorteDet.Where(x => x.cc != null && x.corteID == corteID && x.cuenta.Contains("4000-5-") && x.fechapol <= fechaFin && x.fechapol >= fechaInicio
                            && (maquinasModelo.Count() > 0 ? maquinasModelo.Contains(x.cc) : true) && ((economico == null || economico == "" || economico == "TODOS") ? true : x.cc == economico) && !(x.referencia != null
                            && x.referencia.Contains("/" + (fechaInicio.Year - 1).ToString()) && x.fechapol.Year == fechaInicio.Year));
                        foreach (var item in otr) { item.areaCuenta = "9-27"; }
                        if (economicoCol != "") { otr = otr.Where(x => x.cc == economicoCol); }
                        if (tipo == 4) { otr = otr.Where(x => x.tipoEquipo == columna); }
                        var lstOtr = otr.ToList();
                        detallesRaw.AddRange(lstOtr);
                    }
                }
                detalles = detallesRaw.Select(x =>
                {
                    return new CorteDTO
                    {
                        id = x.id,
                        corteID = corteID,
                        cuenta = x.cuenta,
                        monto = x.monto * (x.cuenta != "1-1-0" && x.cuenta != "1-2-1" && x.cuenta != "1-2-2" && x.cuenta != "1-2-3" && x.cuenta != "1-3-1" && x.cuenta != "1-3-2" && !reporteCostos ? (-1) : (1)),
                        fechapol = x.fechapol,
                        tipoEquipo = x.tipoEquipo,
                        cc = x.cc,
                        poliza = x.poliza,
                        concepto = x.concepto,
                        referencia = x.referencia,
                        semana = semana,
                        empresa = x.empresa,
                        areaCuenta = x.areaCuenta,
                        linea = x.linea
                    };
                }).ToList();



                if (vSesiones.sesionEmpresaActual == 1)
                {
                    foreach (var item in detalles)
                    {
                        var auxAreaCuentaCplan = centrosCostosEnkontrolCplan.FirstOrDefault(x => x.Prefijo == null ? x.Value == item.areaCuenta : x.Prefijo == item.areaCuenta);
                        if (auxAreaCuentaCplan == null)
                        {
                            var auxAreaCuentaArr = centrosCostosEnkontrolArr.FirstOrDefault(x => x.Value == item.areaCuenta);
                            item.areaCuenta = auxAreaCuentaArr == null ? item.areaCuenta : auxAreaCuentaArr.Text;
                        }
                        else { item.areaCuenta = auxAreaCuentaCplan.Text; }
                    }
                }
                else
                {
                    foreach (var item in detalles)
                    {
                        var auxAreaCuentaArr = centrosCostosEnkontrolArr.FirstOrDefault(x => x.Value == item.areaCuenta);
                        if (auxAreaCuentaArr == null)
                        {
                            var auxAreaCuentaCplan = centrosCostosEnkontrolCplan.FirstOrDefault(x => x.Value == item.areaCuenta);
                            item.areaCuenta = auxAreaCuentaCplan == null ? item.areaCuenta : auxAreaCuentaCplan.Text;
                        }
                        else { item.areaCuenta = auxAreaCuentaArr.Text; }
                    }
                }
                return detalles;
            }
        }

        public List<CorteDTO> getLstKubrixCorteAnterior(int corteID, int tipo, int columna, int renglon, List<int> modelos, string economico, DateTime fechaInicio, DateTime fechaFin, List<string> areaCuenta, int usuarioID, string divisionCol, string areaCuentaCol, string economicoCol, bool reporteCostos)
        {
            var empresa = (int)EmpresaEnum.Arrendadora;
            if (vSesiones.sesionEmpresaActual == 2) { empresa = (int)EmpresaEnum.Construplan; }
            var corte = _context.tblM_KBCorte.FirstOrDefault(x => x.id == corteID);

            var centrosCostosEnkontrolArr = getListaCC().ToList();
            var centrosCostosEnkontrolCplan = getListaCCCPlan();
            using (var _db = new MainContext(empresa))
            {
                List<tblM_CatMaquina> maquinas = new List<tblM_CatMaquina>();
                if (vSesiones.sesionEmpresaActual == 2) { maquinas = _context.tblM_CatMaquina.ToList(); }
                else { maquinas = _db.tblM_CatMaquina.ToList(); }

                List<string> auxObras = new List<string>();
                if (areaCuenta != null)
                {
                    auxObras = areaCuenta;
                    areaCuenta.Remove("");
                }
                if (corteID == 20) { fechaFin = new DateTime(2020, 2, 29); }
                var auxFacturasConcSalida = _context.tblM_CapEncConciliacionHorometros.Where(x => x.estatus == 1 && x.facturado && x.fechaFin.Month == 12 && x.fechaFin < fechaInicio).ToList();
                List<string> auxFacturasSalida = new List<string>();
                foreach (var item in auxFacturasConcSalida) { auxFacturasSalida.AddRange(JsonConvert.DeserializeObject<List<string>>(item.factura)); }
                auxFacturasSalida = auxFacturasSalida.Distinct().ToList();
                auxFacturasSalida.Remove("0");
                if (auxFacturasSalida.Count() < 1) auxFacturasSalida.Add("-1");

                var maquinasModelo = modelos == null ? new List<string>() : (vSesiones.sesionEmpresaActual == 2 ? _context.tblM_CatMaquina.Where(x => modelos.Contains(x.modeloEquipoID)) : _db.tblM_CatMaquina.Where(x => modelos.Contains(x.modeloEquipoID))).Select(x => x.noEconomico).ToList();

                var auxDetallesRaw = _context.tblM_KBCorteDet.Where(x => x.cc != null && (x.corteID == corteID) && x.fechapol <= fechaFin && x.fechapol >= fechaInicio
                    && (auxObras.Count() > 0 ? auxObras.Contains(x.areaCuenta) : true) && (maquinasModelo.Count() > 0 ? maquinasModelo.Contains(x.cc) : true)
                    && ((economico == null || economico == "" || economico == "TODOS") ? true : x.cc == economico) && !(x.referencia != null && x.referencia.Contains("/" + (fechaInicio.Year - 1).ToString()) && x.fechapol.Year == fechaInicio.Year)
                    && (vSesiones.sesionEmpresaActual == 2 ? !x.cuenta.Contains("4000-5-") && !x.cuenta.Contains("4000-6-") : true));

                IQueryable<tblM_KBCorteDet> auxDetallesOtraEmpresaRaw = null;

                if (corte != null && vSesiones.sesionEmpresaActual == 2 && (renglon == -1 || renglon == 5 || renglon == 8 || renglon == 10))
                {
                    var corteOtraEmpresa = _db.tblM_KBCorte.FirstOrDefault(x => x.fechaCorte == corte.fechaCorte);
                    if (corteOtraEmpresa != null)
                    {
                        auxDetallesOtraEmpresaRaw = _db.tblM_KBCorteDet.Where(x => x.cc != null && (x.corteID == corteOtraEmpresa.id) && x.fechapol <= fechaFin && x.fechapol >= fechaInicio
                            && (auxObras.Count() > 0 ? auxObras.Contains(x.areaCuenta) : true) && (maquinasModelo.Count() > 0 ? maquinasModelo.Contains(x.cc) : true)
                            && ((economico == null || economico == "" || economico == "TODOS") ? true : x.cc == economico) && !(x.referencia != null && x.referencia.Contains("/" + (fechaInicio.Year - 1).ToString()) && x.fechapol.Year == fechaInicio.Year)
                            && (vSesiones.sesionEmpresaActual == 2 ? !x.cuenta.Contains("4000-5-") && !x.cuenta.Contains("4000-6-") : true)).Where(x => x.cuenta.Contains("5000-10-") || x.cuenta.Contains("5900-3-") || x.cuenta.Contains("5280-10-"));

                    }
                }

                switch (renglon)
                {
                    case -2: auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta.Contains("4000-") || x.cuenta == "1-1-0" || x.cuenta == "1-3-1" || x.cuenta == "1-3-2" || x.cuenta == "1-2-1" || x.cuenta == "1-2-2" || x.cuenta == "1-2-3" || x.cuenta.Contains("4900-") || x.cuenta.Contains("4901-")); break;
                    case -1: auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta.Contains("5000-") || x.cuenta == "1-4-0" || x.cuenta.Contains("5280-") || x.cuenta.Contains("5900-") || x.cuenta.Contains("5901-")); break;
                    case 0: auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta.Contains("4000-")); break;
                    case 1: auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta == "1-1-0" || x.cuenta == "1-3-1" || x.cuenta == "1-3-2"); break;
                    case 2: auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta == "1-2-1" || x.cuenta == "1-2-2" || x.cuenta == "1-2-3"); break;
                    case 4: auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta.Contains("5000-") && !x.cuenta.Contains("5000-10-")); break;
                    case 5:
                        auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta.Contains("5000-10-"));
                        if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => x.cuenta.Contains("5000-10-")); }
                        break;
                    case 6: auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta == "1-4-0"); break;
                    case 8:
                        auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta.Contains("5280-"));
                        if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => x.cuenta.Contains("5280-10-")); }
                        break;
                    case 10:
                        auxDetallesRaw = auxDetallesRaw.Where(x => (reporteCostos ? x.cuenta.Contains("5900-") : x.cuenta.Contains("4900-")) || x.cuenta.Contains("5900-"));
                        if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => x.cuenta.Contains("5900-3-")); }
                        break;
                    case 12: auxDetallesRaw = auxDetallesRaw.Where(x => (reporteCostos ? x.cuenta.Contains("5901-") : x.cuenta.Contains("4901-")) || x.cuenta.Contains("5901-")); break;
                }

                if (divisionCol != "" && divisionCol != "ARRENDADORA")
                {
                    List<string> acAdministracion = (new string[] { "003", "D01", "0", "A05", "026", "524", "979", "990" }).ToList();
                    List<string> acAdministracionArr = (new string[] { "9-30", "14-1", "14-2", "15-1", "16-1", "16-2", "16-3", "988", "994", "997" }).ToList();
                    if (divisionCol == "SIN DIVISION")
                    {
                        var divisiones = _context.tblM_KBDivision.Where(x => x.estatus).Select(x => x.divisionDetalle).ToList();
                        List<string> areasCuentaDivision = new List<string>();
                        foreach (var item in divisiones)
                        {
                            List<string> auxAC = item.Where(x => x.estatus).Select(x => x.ac.areaCuenta).ToList();
                            areasCuentaDivision.AddRange(auxAC);
                        }
                        areasCuentaDivision = areasCuentaDivision.Distinct().ToList();

                        areasCuentaDivision.AddRange(acAdministracion);
                        areasCuentaDivision.AddRange(acAdministracionArr);
                        if (areasCuentaDivision.Count() > 0)
                        {
                            auxDetallesRaw = auxDetallesRaw.Where(x => !areasCuentaDivision.Contains(x.areaCuenta));
                            if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => !areasCuentaDivision.Contains(x.areaCuenta)); }
                        }
                    }
                    else
                    {
                        var division = _context.tblM_KBDivision.FirstOrDefault(x => x.division == divisionCol);
                        if (division != null)
                        {
                            var areasCuentaDivision = division.divisionDetalle.Where(x => x.ac != null && x.estatus).Select(x => x.ac.areaCuenta).ToList();
                            //Administración
                            if (division.id == 7) { areasCuentaDivision.AddRange(acAdministracion); }
                            //Administracion arrendadora
                            if (division.id == 13) { areasCuentaDivision.AddRange(acAdministracionArr); }
                            if (areasCuentaDivision.Count() > 0)
                            {
                                auxDetallesRaw = auxDetallesRaw.Where(x => areasCuentaDivision.Contains(x.areaCuenta));
                                if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => areasCuentaDivision.Contains(x.areaCuenta)); }
                            }
                        }
                    }
                }
                if (areaCuentaCol != "" && areaCuentaCol != "ARRENDADORA" && areaCuentaCol != divisionCol)
                {
                    string areaCuentaSplit = areaCuentaCol.Split(' ')[0];
                    auxDetallesRaw = auxDetallesRaw.Where(x => x.areaCuenta == areaCuentaSplit);
                    if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => x.areaCuenta == areaCuentaSplit); }
                }
                if (economicoCol != "")
                {
                    auxDetallesRaw = auxDetallesRaw.Where(x => x.cc == economicoCol);
                    if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => x.cc == economicoCol); }
                }
                if (tipo == 4)
                {
                    auxDetallesRaw = auxDetallesRaw.Where(x => x.tipoEquipo == columna);
                    if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => x.tipoEquipo == columna); }
                }
                //
                List<CorteDTO> detalles = new List<CorteDTO>();
                var detallesRaw = auxDetallesRaw.ToList();
                if (auxDetallesOtraEmpresaRaw != null)
                {
                    var detallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.ToList();
                    detallesRaw.AddRange(detallesOtraEmpresaRaw);
                }
                //Agregar fletes y otr como areas cuenta
                if (vSesiones.sesionEmpresaActual == 2 && (renglon == 0 || renglon == -2))
                {
                    if ((auxObras.Count() <= 0 && (divisionCol == "ARRENDADORA" || areaCuentaCol == "ARRENDADORA" || (divisionCol == "" && areaCuentaCol == "")))
                        || (auxObras.Contains("6-5") && (divisionCol == "ARRENDADORA" || areaCuentaCol == "ARRENDADORA" || (divisionCol == "" && areaCuentaCol == "")))
                        || divisionCol == "FLETES" || areaCuentaCol == "6-5 DEPARTAMENTO DE FLETES")
                    {
                        var fletes = _context.tblM_KBCorteDet.Where(x => x.cc != null && x.corteID == corteID && x.cuenta.Contains("4000-6-") && x.fechapol <= fechaFin && x.fechapol >= fechaInicio
                            && (maquinasModelo.Count() > 0 ? maquinasModelo.Contains(x.cc) : true) && ((economico == null || economico == "" || economico == "TODOS") ? true : x.cc == economico) && !(x.referencia != null
                            && x.referencia.Contains("/" + (fechaInicio.Year - 1).ToString()) && x.fechapol.Year == fechaInicio.Year));
                        foreach (var item in fletes) { item.areaCuenta = "6-5"; }
                        if (economicoCol != "") { fletes = fletes.Where(x => x.cc == economicoCol); }
                        if (tipo == 4) { fletes = fletes.Where(x => x.tipoEquipo == columna); }
                        var lstFletes = fletes.ToList();
                        detallesRaw.AddRange(lstFletes);
                    }
                    if ((auxObras.Count() <= 0 && (divisionCol == "ARRENDADORA" || areaCuentaCol == "ARRENDADORA" || (divisionCol == "" && areaCuentaCol == "")))
                        || (auxObras.Contains("9-27") && (divisionCol == "ARRENDADORA" || areaCuentaCol == "ARRENDADORA" || (divisionCol == "" && areaCuentaCol == "")))
                        || divisionCol == "LLANTAS OTR" || areaCuentaCol == "9-27 LLANTAS OTR")
                    {
                        var otr = _context.tblM_KBCorteDet.Where(x => x.cc != null && x.corteID == corteID && x.cuenta.Contains("4000-5-") && x.fechapol <= fechaFin && x.fechapol >= fechaInicio
                            && (maquinasModelo.Count() > 0 ? maquinasModelo.Contains(x.cc) : true) && ((economico == null || economico == "" || economico == "TODOS") ? true : x.cc == economico) && !(x.referencia != null
                            && x.referencia.Contains("/" + (fechaInicio.Year - 1).ToString()) && x.fechapol.Year == fechaInicio.Year));
                        foreach (var item in otr) { item.areaCuenta = "9-27"; }
                        if (economicoCol != "") { otr = otr.Where(x => x.cc == economicoCol); }
                        if (tipo == 4) { otr = otr.Where(x => x.tipoEquipo == columna); }
                        var lstOtr = otr.ToList();
                        detallesRaw.AddRange(lstOtr);
                    }
                }
                detalles = detallesRaw.Select(x =>
                {
                    return new CorteDTO
                    {
                        cc = x.cc,
                        cuenta = x.cuenta,
                        areaCuenta = x.areaCuenta,
                        concepto = "**REGISTRO ELIMINADO**" + x.concepto,
                        tipoEquipo = x.tipoEquipo,
                        poliza = x.poliza,
                        semana = 2,
                        referencia = x.referencia,
                        empresa = x.empresa,
                        tipoMov = 2,
                        monto = (x.monto * (x.cuenta != "1-1-0" && x.cuenta != "1-2-1" && x.cuenta != "1-2-2" && x.cuenta != "1-2-3" && x.cuenta != "1-3-1" && x.cuenta != "1-3-2" && !reporteCostos ? (-1) : (1))) * (-1),
                    };
                }).ToList();

                if (vSesiones.sesionEmpresaActual == 1)
                {
                    foreach (var item in detalles)
                    {
                        var auxAreaCuentaCplan = centrosCostosEnkontrolCplan.FirstOrDefault(x => x.Prefijo == null ? x.Value == item.areaCuenta : x.Prefijo == item.areaCuenta);
                        if (auxAreaCuentaCplan == null)
                        {
                            var auxAreaCuentaArr = centrosCostosEnkontrolArr.FirstOrDefault(x => x.Value == item.areaCuenta);
                            item.areaCuenta = auxAreaCuentaArr == null ? item.areaCuenta : auxAreaCuentaArr.Text;
                        }
                        else { item.areaCuenta = auxAreaCuentaCplan.Text; }
                    }
                }
                else
                {
                    foreach (var item in detalles)
                    {
                        var auxAreaCuentaArr = centrosCostosEnkontrolArr.FirstOrDefault(x => x.Value == item.areaCuenta);
                        if (auxAreaCuentaArr == null)
                        {
                            var auxAreaCuentaCplan = centrosCostosEnkontrolCplan.FirstOrDefault(x => x.Value == item.areaCuenta);
                            item.areaCuenta = auxAreaCuentaCplan == null ? item.areaCuenta : auxAreaCuentaCplan.Text;
                        }
                        else { item.areaCuenta = auxAreaCuentaArr.Text; }
                    }
                }
                return detalles;
            }
        }


        public List<CorteDTO> getLstKubrixCorteDetCompleto(int corteID, int tipo, int columna, int renglon, List<int> modelos, string economico,
            DateTime fechaInicio, DateTime fechaFin, List<string> areaCuenta, int semana, int usuarioID, string divisionCol, string areaCuentaCol,
            string economicoCol, string subcuentaFiltro, string subsubcuentaFiltro, string divisionFiltro, string areaCuentaFiltro, string conciliacionFiltro,
            string economicoFiltro, int empresaID, bool reporteCostos)
        {
            var empresa = (int)EmpresaEnum.Arrendadora;
            if (vSesiones.sesionEmpresaActual == 2) { empresa = (int)EmpresaEnum.Construplan; }
            var corte = _context.tblM_KBCorte.FirstOrDefault(x => x.id == corteID);

            var centrosCostosEnkontrolArr = getListaCC().ToList();
            var centrosCostosEnkontrolCplan = getListaCCCPlan();

            List<string> acAdministracion = (new string[] { "003", "D01", "0", "A05", "026", "524", "979", "990" }).ToList();
            List<string> acAdministracionArr = (new string[] { "9-30", "14-1", "14-2", "15-1", "16-1", "16-2", "16-3", "988", "994", "997" }).ToList();

            List<string> ccAcumulado2020 = (new string[] { "10-2", "10-3", "10-6", "10-7", "10-8", "10-9", "10-10", "11-1", "11-2", "10-12", "11-9", "10-24" }).ToList();
            DateTime fechaInicioAcumulado2020 = new DateTime(2020, 1, 1);


            List<string> ccActual = (new string[] { "009", "017", "018", "019", "028", "029", "557", "558", "562", "C65", "C66", "C69", "C71" }).ToList();
            DateTime fechaInicioActual = new DateTime(DateTime.Now.Year, 1, 1);


            using (var _db = new MainContext(empresa))
            {
                List<tblM_CatMaquina> maquinas = new List<tblM_CatMaquina>();
                if (vSesiones.sesionEmpresaActual == 2) { maquinas = _context.tblM_CatMaquina.ToList(); }
                else { maquinas = _db.tblM_CatMaquina.ToList(); }

                List<string> auxObras = new List<string>();
                if (areaCuenta != null)
                {
                    auxObras = areaCuenta;
                    areaCuenta.Remove("");
                }
                if (corteID == 20) { fechaFin = new DateTime(2020, 2, 29); }
                if (vSesiones.sesionEmpresaActual == 2)
                {
                    var auxFacturasConcSalida = _context.tblM_CapEncConciliacionHorometros.Where(x => x.estatus == 1 && x.facturado && x.fechaFin.Month == 12 && x.fechaFin < fechaInicio).ToList();
                    List<string> auxFacturasSalida = new List<string>();
                    foreach (var item in auxFacturasConcSalida) { auxFacturasSalida.AddRange(JsonConvert.DeserializeObject<List<string>>(item.factura)); }
                    auxFacturasSalida = auxFacturasSalida.Distinct().ToList();
                    auxFacturasSalida.Remove("0");
                    if (auxFacturasSalida.Count() < 1) auxFacturasSalida.Add("-1");
                }
                var maquinasModelo = modelos == null ? new List<string>() : (vSesiones.sesionEmpresaActual == 2 ? _context.tblM_CatMaquina.Where(x => modelos.Contains(x.modeloEquipoID)) : _db.tblM_CatMaquina.Where(x => modelos.Contains(x.modeloEquipoID))).Select(x => x.noEconomico).ToList();

                var auxDetallesRaw = _context.tblM_KBCorteDet.Where(x => x.cc != null && (x.corteID == corteID) && x.fechapol <= fechaFin && x.fechapol >= fechaInicio
                    && (auxObras.Count() > 0 ? auxObras.Contains(x.areaCuenta) : true) && (maquinasModelo.Count() > 0 ? maquinasModelo.Contains(x.cc) : true)
                    && ((economico == null || economico == "" || economico == "TODOS") ? true : x.cc == economico) && !(x.referencia != null && x.referencia.Contains("/" + (fechaInicio.Year - 1).ToString()) && x.fechapol.Year == fechaInicio.Year)
                    && !x.cuenta.Contains("4000-5-") && !x.cuenta.Contains("4000-6-") /*&& (empresaID > 0 ? x.empresa == empresaID : true)*/);

                if (vSesiones.sesionEmpresaActual == 1) { auxDetallesRaw = auxDetallesRaw.Where(x => !(ccAcumulado2020.Contains(x.areaCuenta) && x.fechapol <= fechaInicioAcumulado2020)); }

                var cortesAnteriores = _context.tblM_KBCorte.Where(x => x.tipo == 2 && x.anio >= fechaInicio.Year && x.anio < (fechaFin.Year - 1)).Select(x => x.id).ToList();

                if (cortesAnteriores.Count() > 0)
                {
                    var auxDetallesAnterioresRaw = _context.tblM_KBCorteDet.Where(x => x.cc != null && cortesAnteriores.Contains(x.corteID) && x.fechapol <= fechaFin && x.fechapol >= fechaInicio
                    && (auxObras.Count() > 0 ? auxObras.Contains(x.areaCuenta) : true) && (maquinasModelo.Count() > 0 ? maquinasModelo.Contains(x.cc) : true)
                    && ((economico == null || economico == "" || economico == "TODOS") ? true : x.cc == economico) && !(x.referencia != null && x.referencia.Contains("/" + (fechaInicio.Year - 1).ToString()) && x.fechapol.Year == fechaInicio.Year)
                    && !x.cuenta.Contains("4000-5-") && !x.cuenta.Contains("4000-6-") /*&& (empresaID > 0 ? x.empresa == empresaID : true)*/);

                    if (vSesiones.sesionEmpresaActual == 1) { auxDetallesAnterioresRaw = auxDetallesAnterioresRaw.Where(x => !(ccAcumulado2020.Contains(x.areaCuenta) && x.fechapol <= fechaInicioAcumulado2020)); }
                    if (vSesiones.sesionEmpresaActual == 1) { auxDetallesAnterioresRaw = auxDetallesAnterioresRaw.Where(x => !(ccActual.Contains(x.areaCuenta) && x.fechapol <= fechaInicioActual)); }

                    auxDetallesRaw = auxDetallesRaw.Concat(auxDetallesAnterioresRaw);
                }

                IQueryable<tblM_KBCorteDet> auxDetallesOtraEmpresaRaw = null;

                if (corte != null && vSesiones.sesionEmpresaActual == 2 && (renglon == -1 || renglon == 5 || renglon == 8 || renglon == 10) && empresaID < 2)
                {
                    var corteOtraEmpresa = _db.tblM_KBCorte.FirstOrDefault(x => x.fechaCorte == corte.fechaCorte && x.tipo == corte.tipo);
                    if (corteOtraEmpresa != null)
                    {
                        auxDetallesOtraEmpresaRaw = _db.tblM_KBCorteDet.Where(x => x.cc != null && (x.corteID == corteOtraEmpresa.id) && x.fechapol <= fechaFin && x.fechapol >= fechaInicio
                            && (auxObras.Count() > 0 ? auxObras.Contains(x.areaCuenta) : true) && (maquinasModelo.Count() > 0 ? maquinasModelo.Contains(x.cc) : true)
                            && ((economico == null || economico == "" || economico == "TODOS") ? true : x.cc == economico) && !(x.referencia != null && x.referencia.Contains("/" + (fechaInicio.Year - 1).ToString()) && x.fechapol.Year == fechaInicio.Year)
                            && !x.cuenta.Contains("4000-5-") && !x.cuenta.Contains("4000-6-")).Where(x => x.cuenta.Contains("5000-10-") || x.cuenta.Contains("5900-3-") || x.cuenta.Contains("5280-10-"));

                    }
                }

                switch (renglon)
                {
                    case -2: auxDetallesRaw = auxDetallesRaw.Where(x => (x.cuenta.Contains("4000-") || x.cuenta == "1-1-0" || x.cuenta == "1-3-1" || x.cuenta == "1-3-2" || x.cuenta == "1-2-1" || x.cuenta == "1-2-2" || x.cuenta == "1-2-3" || x.cuenta.Contains("4900-") || x.cuenta.Contains("4901-")) && !x.cuenta.Contains("4000-8-")); break;
                    case -1: auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta.Contains("5000-") || x.cuenta == "1-4-0" || x.cuenta.Contains("5280-") || x.cuenta.Contains("5900-") || x.cuenta.Contains("5901-")); break;
                    case 0: auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta.Contains("4000-")); break;
                    case 1: auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta == "1-1-0" || x.cuenta == "1-3-1" || x.cuenta == "1-3-2"); break;
                    case 2: auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta == "1-2-1" || x.cuenta == "1-2-2" || x.cuenta == "1-2-3"); break;
                    case 4: auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta.Contains("5000-") && !x.cuenta.Contains("5000-10-")); break;
                    case 5:
                        auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta.Contains("5000-10-"));
                        if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => x.cuenta.Contains("5000-10-")); }
                        break;
                    case 6: auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta == "1-4-0"); break;
                    case 8:
                        auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta.Contains("5280-"));
                        if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => x.cuenta.Contains("5280-10-")); }
                        break;
                    case 10:
                        auxDetallesRaw = auxDetallesRaw.Where(x => (reporteCostos ? x.cuenta.Contains("5900-") : x.cuenta.Contains("4900-")) || x.cuenta.Contains("5900-"));
                        if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => x.cuenta.Contains("5900-3-")); }
                        break;
                    case 12: auxDetallesRaw = auxDetallesRaw.Where(x => (reporteCostos ? x.cuenta.Contains("5901-") : (x.cuenta.Contains("4901-") || x.cuenta.Contains("4000-8-"))) || x.cuenta.Contains("5901-")); break;
                }

                if (divisionCol != "" && divisionCol != "ARRENDADORA")
                {
                    if (divisionCol == "SIN DIVISION")
                    {
                        var divisiones = _context.tblM_KBDivision.Where(x => x.estatus).Select(x => x.divisionDetalle).ToList();
                        List<string> areasCuentaDivision = new List<string>();
                        foreach (var item in divisiones)
                        {
                            List<string> auxAC = item.Where(x => x.ac != null && x.estatus).Select(x => x.ac.areaCuenta).ToList();
                            areasCuentaDivision.AddRange(auxAC);
                        }
                        areasCuentaDivision = areasCuentaDivision.Distinct().ToList();
                        areasCuentaDivision.AddRange(acAdministracion);
                        areasCuentaDivision.AddRange(acAdministracionArr);
                        if (areasCuentaDivision.Count() > 0)
                        {
                            auxDetallesRaw = auxDetallesRaw.Where(x => !areasCuentaDivision.Contains(x.areaCuenta));
                            if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => !areasCuentaDivision.Contains(x.areaCuenta)); }
                        }
                    }
                    else
                    {
                        var division = _context.tblM_KBDivision.FirstOrDefault(x => x.division == divisionCol);
                        if (division != null)
                        {
                            var areasCuentaDivision = division.divisionDetalle.Where(x => x.ac != null && x.estatus).Select(x => x.ac.areaCuenta).ToList();
                            //Administración
                            if (division.id == 7) { areasCuentaDivision.AddRange(acAdministracion); }
                            //Administracion arrendadora
                            if (division.id == 13) { areasCuentaDivision.AddRange(acAdministracionArr); }
                            if (areasCuentaDivision.Count() > 0)
                            {
                                auxDetallesRaw = auxDetallesRaw.Where(x => areasCuentaDivision.Contains(x.areaCuenta));
                                if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => areasCuentaDivision.Contains(x.areaCuenta)); }
                            }
                        }
                    }
                }
                if (areaCuentaCol != "" && areaCuentaCol != "ARRENDADORA" && areaCuentaCol != divisionCol)
                {
                    string areaCuentaSplit = areaCuentaCol.Split(' ')[0];
                    auxDetallesRaw = auxDetallesRaw.Where(x => x.areaCuenta == areaCuentaSplit);
                    if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => x.areaCuenta == areaCuentaSplit); }
                }
                if (economicoCol != "")
                {
                    auxDetallesRaw = auxDetallesRaw.Where(x => x.cc == economicoCol);
                    if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => x.cc == economicoCol); }
                }
                if (tipo == 4)
                {
                    auxDetallesRaw = auxDetallesRaw.Where(x => x.tipoEquipo == columna);
                    if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => x.tipoEquipo == columna); }
                }
                //
                if (subcuentaFiltro != "")
                {
                    auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta.Contains(subcuentaFiltro + "-"));
                    if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => x.cuenta.Contains(subcuentaFiltro + "-")); }
                }
                if (subsubcuentaFiltro != "")
                {
                    auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta == subsubcuentaFiltro);
                    if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => x.cuenta == subsubcuentaFiltro); }
                }
                if (areaCuentaFiltro != "")
                {
                    string areaCuentaSplit = areaCuentaFiltro.Split(' ')[0];
                    auxDetallesRaw = auxDetallesRaw.Where(x => x.areaCuenta == areaCuentaSplit);
                    if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => x.areaCuenta == areaCuentaSplit); }
                }

                if (divisionFiltro != "" && divisionFiltro != "ARRENDADORA")
                {

                    if (divisionFiltro == "SIN DIVISION")
                    {
                        var divisiones = _context.tblM_KBDivision.Where(x => x.estatus).Select(x => x.divisionDetalle).ToList();
                        List<string> areasCuentaDivision = new List<string>();
                        foreach (var item in divisiones)
                        {
                            List<string> auxAC = item.Where(x => x.ac != null && x.estatus).Select(x => x.ac.areaCuenta).ToList();
                            areasCuentaDivision.AddRange(auxAC);
                        }
                        areasCuentaDivision = areasCuentaDivision.Distinct().ToList();
                        areasCuentaDivision.AddRange(acAdministracion);
                        areasCuentaDivision.AddRange(acAdministracionArr);
                        if (areasCuentaDivision.Count() > 0)
                        {
                            auxDetallesRaw = auxDetallesRaw.Where(x => !areasCuentaDivision.Contains(x.areaCuenta));
                            if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => !areasCuentaDivision.Contains(x.areaCuenta)); }
                        }
                    }
                    else
                    {
                        var division = _context.tblM_KBDivision.FirstOrDefault(x => x.division == divisionFiltro);
                        if (division != null)
                        {
                            var areasCuentaDivision = division.divisionDetalle.Where(x => x.ac != null && x.estatus).Select(x => x.ac.areaCuenta).ToList();
                            //Administración
                            if (division.id == 7) { areasCuentaDivision.AddRange(acAdministracion); }
                            //Administracion arrendadora
                            if (division.id == 13) { areasCuentaDivision.AddRange(acAdministracionArr); }
                            if (areasCuentaDivision.Count() > 0)
                            {
                                auxDetallesRaw = auxDetallesRaw.Where(x => areasCuentaDivision.Contains(x.areaCuenta));
                                if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => areasCuentaDivision.Contains(x.areaCuenta)); }
                            }
                        }
                    }
                }
                if (conciliacionFiltro != "")
                {
                    auxDetallesRaw = auxDetallesRaw.Where(x => x.referencia == conciliacionFiltro);
                    if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => x.referencia == conciliacionFiltro); }
                }
                if (economicoFiltro != "")
                {
                    auxDetallesRaw = auxDetallesRaw.Where(x => x.cc == economicoFiltro);
                    if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => x.cc == economicoFiltro); }
                }

                List<CorteDTO> detalles = new List<CorteDTO>();
                //var detallesRaw = auxDetallesRaw.ToList();
                //if (auxDetallesOtraEmpresaRaw != null)
                //{
                //    var detallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.ToList();
                //    detallesRaw.AddRange(detallesOtraEmpresaRaw);
                //}
                //Agregar fletes y otr como areas cuenta
                if (renglon == 0 || renglon == -2)
                {
                    if ((auxObras.Count() <= 0 && (divisionCol == "ARRENDADORA" || areaCuentaCol == "ARRENDADORA" || (divisionCol == "" && areaCuentaCol == "")))
                        || (auxObras.Contains("6-5") && (divisionCol == "ARRENDADORA" || areaCuentaCol == "ARRENDADORA" || (divisionCol == "" && areaCuentaCol == "")))
                        || divisionCol == "FLETES" || areaCuentaCol == "6-5 DEPARTAMENTO DE FLETES")
                    {
                        var fletes = _context.tblM_KBCorteDet.Where(x => x.cc != null && x.corteID == corteID && x.cuenta.Contains("4000-6-") && x.fechapol <= fechaFin && x.fechapol >= fechaInicio
                            && (maquinasModelo.Count() > 0 ? maquinasModelo.Contains(x.cc) : true) && ((economico == null || economico == "" || economico == "TODOS") ? true : x.cc == economico) && !(x.referencia != null
                            && x.referencia.Contains("/" + (fechaInicio.Year - 1).ToString()) && x.fechapol.Year == fechaInicio.Year));
                        foreach (var item in fletes) { item.areaCuenta = "6-5"; }
                        //
                        if (economicoCol != "") { fletes = fletes.Where(x => x.cc == economicoCol); }
                        if (tipo == 4) { fletes = fletes.Where(x => x.tipoEquipo == columna); }
                        if (subcuentaFiltro != "") { fletes = fletes.Where(x => x.cuenta.Contains(subcuentaFiltro + "-")); }
                        if (subsubcuentaFiltro != "") { fletes = fletes.Where(x => x.cuenta == subsubcuentaFiltro); }
                        if (conciliacionFiltro != "") { fletes = fletes.Where(x => x.referencia == conciliacionFiltro); }
                        if (economicoFiltro != "") { fletes = fletes.Where(x => x.cc == economicoFiltro); }
                        //                    
                        //var lstFletes = fletes.ToList();
                        if (areaCuentaFiltro != "")
                        {
                            string areaCuentaSplit = areaCuentaFiltro.Split(' ')[0];
                            fletes = fletes.Where(x => x.areaCuenta == areaCuentaSplit);
                        }
                        auxDetallesRaw = auxDetallesRaw.Concat(fletes);
                    }
                    if ((auxObras.Count() <= 0 && (divisionCol == "ARRENDADORA" || areaCuentaCol == "ARRENDADORA" || (divisionCol == "" && areaCuentaCol == "")))
                        || (auxObras.Contains("9-27") && (divisionCol == "ARRENDADORA" || areaCuentaCol == "ARRENDADORA" || (divisionCol == "" && areaCuentaCol == "")))
                        || divisionCol == "LLANTAS OTR" || areaCuentaCol == "9-27 LLANTAS OTR")
                    {
                        var otr = _context.tblM_KBCorteDet.Where(x => x.cc != null && x.corteID == corteID && x.cuenta.Contains("4000-5-") && x.fechapol <= fechaFin && x.fechapol >= fechaInicio
                            && (maquinasModelo.Count() > 0 ? maquinasModelo.Contains(x.cc) : true) && ((economico == null || economico == "" || economico == "TODOS") ? true : x.cc == economico) && !(x.referencia != null
                            && x.referencia.Contains("/" + (fechaInicio.Year - 1).ToString()) && x.fechapol.Year == fechaInicio.Year));
                        foreach (var item in otr) { item.areaCuenta = "9-27"; }
                        //
                        if (economicoCol != "") { otr = otr.Where(x => x.cc == economicoCol); }
                        if (tipo == 4) { otr = otr.Where(x => x.tipoEquipo == columna); }
                        if (subcuentaFiltro != "") { otr = otr.Where(x => x.cuenta.Contains(subcuentaFiltro + "-")); }
                        if (subsubcuentaFiltro != "") { otr = otr.Where(x => x.cuenta == subsubcuentaFiltro); }
                        if (conciliacionFiltro != "") { otr = otr.Where(x => x.referencia == conciliacionFiltro); }
                        if (economicoFiltro != "") { otr = otr.Where(x => x.cc == economicoFiltro); }
                        //                    
                        //var lstOtr = otr.ToList();
                        if (areaCuentaFiltro != "")
                        {
                            string areaCuentaSplit = areaCuentaFiltro.Split(' ')[0];
                            //lstOtr = lstOtr.Where(x => x.areaCuenta == areaCuentaSplit).ToList();
                            otr = otr.Where(x => x.areaCuenta == areaCuentaSplit);
                        }
                        auxDetallesRaw = auxDetallesRaw.Concat(otr);
                    }
                }
                //
                if (auxDetallesRaw != null)
                {
                    detalles = auxDetallesRaw.ToList().Select(x =>
                    {
                        return new CorteDTO
                        {
                            id = x.id,
                            corteID = corteID,
                            cuenta = x.cuenta,
                            monto = x.monto * (x.cuenta != "1-1-0" && x.cuenta != "1-2-1" && x.cuenta != "1-2-2" && x.cuenta != "1-2-3" && x.cuenta != "1-3-1" && x.cuenta != "1-3-2" && !reporteCostos ? (-1) : (1)),
                            fechapol = x.fechapol,
                            tipoEquipo = x.tipoEquipo,
                            cc = x.cc,
                            poliza = x.poliza,
                            concepto = x.concepto,
                            referencia = x.referencia,
                            semana = semana,
                            empresa = x.empresa,
                            areaCuenta = x.areaCuenta,
                            linea = x.linea
                        };
                    }).ToList();
                }

                if (auxDetallesOtraEmpresaRaw != null)
                {
                    var auxDetallesCplan = auxDetallesOtraEmpresaRaw.ToList().Select(x =>
                    {
                        return new CorteDTO
                        {
                            id = x.id,
                            corteID = corteID,
                            cuenta = x.cuenta,
                            monto = x.monto * (x.cuenta != "1-1-0" && x.cuenta != "1-2-1" && x.cuenta != "1-2-2" && x.cuenta != "1-2-3" && x.cuenta != "1-3-1" && x.cuenta != "1-3-2" && !reporteCostos ? (-1) : (1)),
                            fechapol = x.fechapol,
                            tipoEquipo = x.tipoEquipo,
                            cc = x.cc,
                            poliza = x.poliza,
                            concepto = x.concepto,
                            referencia = x.referencia,
                            semana = semana,
                            empresa = x.empresa,
                            areaCuenta = x.areaCuenta,
                            linea = x.linea
                        };
                    }).ToList();

                    detalles.AddRange(auxDetallesCplan);
                }

                if (vSesiones.sesionEmpresaActual == 1)
                {
                    foreach (var item in detalles)
                    {
                        var auxAreaCuentaCplan = centrosCostosEnkontrolCplan.FirstOrDefault(x => x.Prefijo == null ? x.Value == item.areaCuenta : x.Prefijo == item.areaCuenta);
                        if (auxAreaCuentaCplan == null)
                        {
                            var auxAreaCuentaArr = centrosCostosEnkontrolArr.FirstOrDefault(x => x.Value == item.areaCuenta);
                            item.areaCuenta = auxAreaCuentaArr == null ? item.areaCuenta : auxAreaCuentaArr.Text;
                        }
                        else { item.areaCuenta = auxAreaCuentaCplan.Text; }
                    }
                }
                else
                {
                    foreach (var item in detalles)
                    {
                        var auxAreaCuentaArr = centrosCostosEnkontrolArr.FirstOrDefault(x => x.Value == item.areaCuenta);
                        if (auxAreaCuentaArr == null)
                        {
                            var auxAreaCuentaCplan = centrosCostosEnkontrolCplan.FirstOrDefault(x => x.Value == item.areaCuenta);
                            item.areaCuenta = auxAreaCuentaCplan == null ? item.areaCuenta : auxAreaCuentaCplan.Text;
                        }
                        else { item.areaCuenta = auxAreaCuentaArr.Text; }
                    }
                }
                return detalles;
            }
        }

        public List<CorteDTO> getLstKubrixCorteDetCompletoCplan(int corteID, int tipo, int columna, int renglon, List<int> modelos, string economico,
            DateTime fechaInicio, DateTime fechaFin, List<string> areaCuenta, int semana, int usuarioID, string divisionCol, string areaCuentaCol,
            string economicoCol, string subcuentaFiltro, string subsubcuentaFiltro, string divisionFiltro, string areaCuentaFiltro, string conciliacionFiltro,
            string economicoFiltro, int empresaID, bool reporteCostos, int acumulado)
        {
            //--> Casos especiales
            if (subcuentaFiltro == "4000-4" || subsubcuentaFiltro.Contains("4000-4-"))
            {
                subcuentaFiltro = "ERROR";
                subsubcuentaFiltro = "ERROR";
            }
            //<--


            var empresa = (int)EmpresaEnum.Construplan;
            if (vSesiones.sesionEmpresaActual == 1) { empresa = (int)EmpresaEnum.Arrendadora; }
            var corte = _context.tblM_KBCorte.FirstOrDefault(x => x.id == corteID);

            var centrosCostosEnkontrolArr = getListaCC().ToList();
            var centrosCostosEnkontrolCplan = getListaCCCPlan();

            List<string> acAdministracion = (new string[] { "003", "D01", "0", "A05", "026", "524", "979", "990" }).ToList();
            List<string> acAdministracionArr = (new string[] { "9-30", "14-1", "14-2", "15-1", "16-1", "16-2", "16-3", "988", "994", "997" }).ToList();

            List<string> ccAcumulado2020 = (new string[] { "10-2", "10-3", "10-6", "10-7", "10-8", "10-9", "10-10", "11-1", "11-2", "10-12", "11-9", "10-24" }).ToList();
            DateTime fechaInicioAcumulado2020 = new DateTime(2020, 1, 1);

            List<string> ccActual = (new string[] { "009", "017", "018", "019", "028", "029", "557", "558", "562", "C65", "C66", "C69", "C71" }).ToList();
            DateTime fechaInicioActual = new DateTime(DateTime.Now.Year, 1, 1);


            using (var _db = new MainContext(empresa))
            {
                List<tblM_CatMaquina> maquinas = _context.tblM_CatMaquina.ToList();
                //var corteEmpresaActual = _context.tblM_KBCorte.Where(x => corte.fechaCorte == x.fechaCorte && x.tipo == tipo).OrderByDescending(x => x.fecha).FirstOrDefault();

                List<string> auxObras = new List<string>();
                if (areaCuenta != null)
                {
                    auxObras = areaCuenta;
                    areaCuenta.Remove("");
                }

                var maquinasModelo = modelos == null ? new List<string>() : _context.tblM_CatMaquina.Where(x => modelos.Contains(x.modeloEquipoID)).Select(x => x.noEconomico).ToList();

                var auxDetallesRaw = _context.tblM_KBCorteDet.Where(x => x.cc != null && (x.corteID == corteID) && x.fechapol <= fechaFin && x.fechapol >= fechaInicio
                    && (auxObras.Count() > 0 ? auxObras.Contains(x.areaCuenta) : true) && (maquinasModelo.Count() > 0 ? maquinasModelo.Contains(x.cc) : true)
                    && ((economico == null || economico == "" || economico == "TODOS") ? true : x.cc == economico) && !(x.referencia != null && x.referencia.Contains("/" + (fechaInicio.Year - 1).ToString()) && x.fechapol.Year == fechaInicio.Year)
                    /*&& !x.cuenta.Contains("4000-5-") && !x.cuenta.Contains("4000-6-") && (empresaID > 0 ? x.empresa == empresaID : true)*/);

                if (vSesiones.sesionEmpresaActual == 1) { auxDetallesRaw = auxDetallesRaw.Where(x => !(ccAcumulado2020.Contains(x.areaCuenta) && x.fechapol <= fechaInicioAcumulado2020)); }
                if (vSesiones.sesionEmpresaActual == 1) { auxDetallesRaw = auxDetallesRaw.Where(x => !(ccActual.Contains(x.areaCuenta) && x.fechapol <= fechaInicioActual)); }

                List<int> cortesAnteriores = new List<int>();
                if (acumulado == 1) cortesAnteriores = _context.tblM_KBCorte.Where(x => x.tipo == 2).Select(x => x.id).ToList();

                if (cortesAnteriores.Count() > 0)
                {
                    var auxDetallesAnterioresRaw = _context.tblM_KBCorteDet.Where(x => x.cc != null && cortesAnteriores.Contains(x.corteID) && x.fechapol <= fechaFin && x.fechapol >= fechaInicio
                    && (auxObras.Count() > 0 ? auxObras.Contains(x.areaCuenta) : true) && (maquinasModelo.Count() > 0 ? maquinasModelo.Contains(x.cc) : true)
                    && ((economico == null || economico == "" || economico == "TODOS") ? true : x.cc == economico) && !(x.referencia != null && x.referencia.Contains("/" + (fechaInicio.Year - 1).ToString()) && x.fechapol.Year == fechaInicio.Year)
                        /*&& !x.cuenta.Contains("4000-5-") && !x.cuenta.Contains("4000-6-") && (empresaID > 0 ? x.empresa == empresaID : true)*/);

                    if (vSesiones.sesionEmpresaActual == 1) { auxDetallesAnterioresRaw = auxDetallesAnterioresRaw.Where(x => !(ccAcumulado2020.Contains(x.areaCuenta) && x.fechapol <= fechaInicioAcumulado2020)); }

                    auxDetallesRaw = auxDetallesRaw.Concat(auxDetallesAnterioresRaw);
                }

                IQueryable<tblM_KBCorteDet> auxDetallesOtraEmpresaRaw = null;

                switch (renglon)
                {
                    case -2: auxDetallesRaw = auxDetallesRaw.Where(x => (x.cuenta.Contains("4000-") || x.cuenta == "1-1-0" || x.cuenta == "1-3-1" || x.cuenta == "1-3-2" || x.cuenta == "1-2-1" || x.cuenta == "1-2-2" || x.cuenta == "1-2-3" || x.cuenta.Contains("4900-") || x.cuenta.Contains("4901-")) && !x.cuenta.Contains("4000-8-") && !x.cuenta.Contains("4000-4-")); break;
                    case -1: auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta.Contains("5000-") || x.cuenta == "1-4-0" || x.cuenta.Contains("5280-") || x.cuenta.Contains("5900-") || x.cuenta.Contains("5901-") && x.cuenta.Contains("4000-4-")); break;
                    case 0: auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta.Contains("4000-") && !x.cuenta.Contains("4000-4-")); break;
                    case 1: auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta == "1-1-0" || x.cuenta == "1-3-1" || x.cuenta == "1-3-2"); break;
                    case 2: auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta == "1-2-1" || x.cuenta == "1-2-2" || x.cuenta == "1-2-3"); break;
                    case 4: auxDetallesRaw = auxDetallesRaw.Where(x => (x.cuenta.Contains("5000-") && !x.cuenta.Contains("5000-10-")) || x.cuenta.Contains("4000-4-")); break;
                    case 5:
                        auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta.Contains("5000-10-"));
                        if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => x.cuenta.Contains("5000-10-")); }
                        break;
                    case 6: auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta.Contains("5900-") && !x.cuenta.Contains("5090-1-")); break;
                    case 7: auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta == "1-4-0"); break;
                    case 9:
                        auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta.Contains("5280-"));
                        if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => x.cuenta.Contains("5280-10-")); }
                        break;
                    case 11: auxDetallesRaw = auxDetallesRaw.Where(x => (reporteCostos ? x.cuenta.Contains("5900-1-") : x.cuenta.Contains("4900-1-")) || x.cuenta.Contains("5900-1-")); break;
                    case 13: auxDetallesRaw = auxDetallesRaw.Where(x => (reporteCostos ? x.cuenta.Contains("5901-") : (x.cuenta.Contains("4901-") || x.cuenta.Contains("4000-8-"))) || x.cuenta.Contains("5901-")); break;
                }

                if (divisionCol != "" && divisionCol != "ARRENDADORA")
                {
                    if (divisionCol == "SIN DIVISION")
                    {
                        var divisiones = _context.tblM_KBDivision.Where(x => x.estatus).Select(x => x.divisionDetalle).ToList();
                        List<string> areasCuentaDivision = new List<string>();
                        foreach (var item in divisiones)
                        {
                            List<string> auxAC = item.Where(x => x.ac != null && x.estatus).Select(x => x.ac.areaCuenta).ToList();
                            areasCuentaDivision.AddRange(auxAC);
                        }
                        areasCuentaDivision = areasCuentaDivision.Distinct().ToList();
                        areasCuentaDivision.AddRange(acAdministracion);
                        areasCuentaDivision.AddRange(acAdministracionArr);
                        if (areasCuentaDivision.Count() > 0)
                        {
                            auxDetallesRaw = auxDetallesRaw.Where(x => !areasCuentaDivision.Contains(x.areaCuenta));
                            if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => !areasCuentaDivision.Contains(x.areaCuenta)); }
                        }
                    }
                    else
                    {
                        var division = _context.tblM_KBDivision.FirstOrDefault(x => x.division == divisionCol);
                        if (division != null)
                        {
                            var areasCuentaDivision = division.divisionDetalle.Where(x => x.ac != null && x.estatus).Select(x => x.ac.areaCuenta == "0" || x.ac.areaCuenta == "0-0" ? x.ac.cc : x.ac.areaCuenta).ToList();
                            //Administración
                            if (division.id == 7) { areasCuentaDivision.AddRange(acAdministracion); }
                            //Administracion arrendadora
                            if (division.id == 13) { areasCuentaDivision.AddRange(acAdministracionArr); }
                            if (areasCuentaDivision.Count() > 0)
                            {
                                auxDetallesRaw = auxDetallesRaw.Where(x => areasCuentaDivision.Contains(x.areaCuenta));
                                if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => areasCuentaDivision.Contains(x.areaCuenta)); }
                            }
                        }
                    }
                }
                if (areaCuentaCol != "" && areaCuentaCol != "ARRENDADORA" && areaCuentaCol != divisionCol)
                {
                    var stringAC = areaCuentaCol.Split(' ')[0];
                    var auxAreaCuenta = _context.tblP_CC.FirstOrDefault(x => x.cc == stringAC);
                    if (auxAreaCuenta != null)
                    {
                        auxDetallesRaw = auxDetallesRaw.Where(x => x.areaCuenta == auxAreaCuenta.areaCuenta);
                        if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => x.areaCuenta == auxAreaCuenta.areaCuenta); }
                    }
                    else
                    {
                        auxDetallesRaw = auxDetallesRaw.Where(x => x.areaCuenta == stringAC);
                        if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => x.areaCuenta == stringAC); }
                    }
                }
                if (economicoCol != "")
                {
                    auxDetallesRaw = auxDetallesRaw.Where(x => x.cc == economicoCol);
                    if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => x.cc == economicoCol); }
                }
                if (tipo == 4)
                {
                    auxDetallesRaw = auxDetallesRaw.Where(x => x.tipoEquipo == columna);
                    if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => x.tipoEquipo == columna); }
                }
                //
                if (subcuentaFiltro != "")
                {
                    if (subcuentaFiltro == "5000-4")
                    {
                        auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta.Contains(subcuentaFiltro + "-") || x.cuenta.Contains("4000-4-"));
                        if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => x.cuenta.Contains(subcuentaFiltro + "-") || x.cuenta.Contains("4000-4-")); }
                    }
                    else
                    {
                        auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta.Contains(subcuentaFiltro + "-"));
                        if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => x.cuenta.Contains(subcuentaFiltro + "-")); }
                    }
                }
                if (subsubcuentaFiltro != "")
                {
                    if (subsubcuentaFiltro == "5000-4-2")
                    {
                        auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta == subsubcuentaFiltro || x.cuenta.Contains("4000-4-"));
                        if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => x.cuenta == subsubcuentaFiltro || x.cuenta.Contains("4000-4-")); }
                    }
                    else
                    {
                        auxDetallesRaw = auxDetallesRaw.Where(x => x.cuenta == subsubcuentaFiltro);
                        if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => x.cuenta == subsubcuentaFiltro); }
                    }
                }
                if (areaCuentaFiltro != "")
                {
                    var stringAC = areaCuentaFiltro.Split(' ')[0];
                    var auxAreaCuenta = _context.tblP_CC.FirstOrDefault(x => x.cc == stringAC);

                    if (auxAreaCuenta != null && auxAreaCuenta.areaCuenta != "0-0" && auxAreaCuenta.areaCuenta != "0-0")
                    {
                        auxDetallesRaw = auxDetallesRaw.Where(x => x.areaCuenta == auxAreaCuenta.areaCuenta);
                        if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => x.areaCuenta == auxAreaCuenta.areaCuenta); }
                    }
                    else
                    {
                        auxDetallesRaw = auxDetallesRaw.Where(x => x.areaCuenta == stringAC);
                        if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => x.areaCuenta == stringAC); }
                    }

                }

                if (divisionFiltro != "" && divisionFiltro != "ARRENDADORA")
                {

                    if (divisionFiltro == "SIN DIVISION")
                    {
                        var divisiones = _context.tblM_KBDivision.Where(x => x.estatus).Select(x => x.divisionDetalle).ToList();
                        List<string> areasCuentaDivision = new List<string>();
                        foreach (var item in divisiones)
                        {
                            List<string> auxAC = item.Where(x => x.ac != null && x.estatus).Select(x => x.ac.areaCuenta).ToList();
                            areasCuentaDivision.AddRange(auxAC);
                        }
                        areasCuentaDivision = areasCuentaDivision.Distinct().ToList();
                        areasCuentaDivision.AddRange(acAdministracion);
                        areasCuentaDivision.AddRange(acAdministracionArr);
                        if (areasCuentaDivision.Count() > 0)
                        {
                            auxDetallesRaw = auxDetallesRaw.Where(x => !areasCuentaDivision.Contains(x.areaCuenta));
                            if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => !areasCuentaDivision.Contains(x.areaCuenta)); }
                        }
                    }
                    else
                    {
                        var division = _context.tblM_KBDivision.FirstOrDefault(x => x.division == divisionFiltro);
                        if (division != null)
                        {
                            var areasCuentaDivision = division.divisionDetalle.Where(x => x.ac != null && x.estatus).Select(x => x.ac.areaCuenta).ToList();
                            //Administración
                            if (division.id == 7) { areasCuentaDivision.AddRange(acAdministracion); }
                            //Administracion arrendadora
                            if (division.id == 13) { areasCuentaDivision.AddRange(acAdministracionArr); }
                            if (areasCuentaDivision.Count() > 0)
                            {
                                auxDetallesRaw = auxDetallesRaw.Where(x => areasCuentaDivision.Contains(x.areaCuenta));
                                if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => areasCuentaDivision.Contains(x.areaCuenta)); }
                            }
                        }
                    }
                }
                if (conciliacionFiltro != "")
                {
                    auxDetallesRaw = auxDetallesRaw.Where(x => x.referencia == conciliacionFiltro);
                    if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => x.referencia == conciliacionFiltro); }
                }
                if (economicoFiltro != "")
                {
                    auxDetallesRaw = auxDetallesRaw.Where(x => x.cc == economicoFiltro);
                    if (auxDetallesOtraEmpresaRaw != null) { auxDetallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.Where(x => x.cc == economicoFiltro); }
                }

                List<CorteDTO> detalles = new List<CorteDTO>();
                //var detallesRaw = auxDetallesRaw.ToList();
                //if (auxDetallesOtraEmpresaRaw != null)
                //{
                //    var detallesOtraEmpresaRaw = auxDetallesOtraEmpresaRaw.ToList();
                //    detallesRaw.AddRange(detallesOtraEmpresaRaw);
                //}
                //Agregar fletes y otr como areas cuenta
                //if (renglon == 0 || renglon == -2)
                //{
                //    if ((auxObras.Count() <= 0 && (divisionCol == "ARRENDADORA" || areaCuentaCol == "ARRENDADORA" || (divisionCol == "" && areaCuentaCol == "")))
                //        || (auxObras.Contains("6-5") && (divisionCol == "ARRENDADORA" || areaCuentaCol == "ARRENDADORA" || (divisionCol == "" && areaCuentaCol == "")))
                //        || divisionCol == "FLETES" || areaCuentaCol == "6-5 DEPARTAMENTO DE FLETES")
                //    {
                //        var fletes = _context.tblM_KBCorteDet.Where(x => x.cc != null && x.corteID == corteID && x.cuenta.Contains("4000-6-") && x.fechapol <= fechaFin && x.fechapol >= fechaInicio
                //            && (maquinasModelo.Count() > 0 ? maquinasModelo.Contains(x.cc) : true) && ((economico == null || economico == "" || economico == "TODOS") ? true : x.cc == economico) && !(x.referencia != null
                //            && x.referencia.Contains("/" + (fechaInicio.Year - 1).ToString()) && x.fechapol.Year == fechaInicio.Year));
                //        foreach (var item in fletes) { item.areaCuenta = "6-5"; }
                //        //
                //        if (economicoCol != "") { fletes = fletes.Where(x => x.cc == economicoCol); }
                //        if (tipo == 4) { fletes = fletes.Where(x => x.tipoEquipo == columna); }
                //        if (subcuentaFiltro != "") { fletes = fletes.Where(x => x.cuenta.Contains(subcuentaFiltro + "-")); }
                //        if (subsubcuentaFiltro != "") { fletes = fletes.Where(x => x.cuenta == subsubcuentaFiltro); }
                //        if (conciliacionFiltro != "") { fletes = fletes.Where(x => x.referencia == conciliacionFiltro); }
                //        if (economicoFiltro != "") { fletes = fletes.Where(x => x.cc == economicoFiltro); }
                //        //                    
                //        //var lstFletes = fletes.ToList();
                //        if (areaCuentaFiltro != "")
                //        {
                //            string areaCuentaSplit = areaCuentaFiltro.Split(' ')[0];
                //            fletes = fletes.Where(x => x.areaCuenta == areaCuentaSplit);
                //        }
                //        auxDetallesRaw = auxDetallesRaw.Concat(fletes);
                //    }
                //    if ((auxObras.Count() <= 0 && (divisionCol == "ARRENDADORA" || areaCuentaCol == "ARRENDADORA" || (divisionCol == "" && areaCuentaCol == "")))
                //        || (auxObras.Contains("9-27") && (divisionCol == "ARRENDADORA" || areaCuentaCol == "ARRENDADORA" || (divisionCol == "" && areaCuentaCol == "")))
                //        || divisionCol == "LLANTAS OTR" || areaCuentaCol == "9-27 LLANTAS OTR")
                //    {
                //        var otr = _context.tblM_KBCorteDet.Where(x => x.cc != null && x.corteID == corteID && x.cuenta.Contains("4000-5-") && x.fechapol <= fechaFin && x.fechapol >= fechaInicio
                //            && (maquinasModelo.Count() > 0 ? maquinasModelo.Contains(x.cc) : true) && ((economico == null || economico == "" || economico == "TODOS") ? true : x.cc == economico) && !(x.referencia != null
                //            && x.referencia.Contains("/" + (fechaInicio.Year - 1).ToString()) && x.fechapol.Year == fechaInicio.Year));
                //        foreach (var item in otr) { item.areaCuenta = "9-27"; }
                //        //
                //        if (economicoCol != "") { otr = otr.Where(x => x.cc == economicoCol); }
                //        if (tipo == 4) { otr = otr.Where(x => x.tipoEquipo == columna); }
                //        if (subcuentaFiltro != "") { otr = otr.Where(x => x.cuenta.Contains(subcuentaFiltro + "-")); }
                //        if (subsubcuentaFiltro != "") { otr = otr.Where(x => x.cuenta == subsubcuentaFiltro); }
                //        if (conciliacionFiltro != "") { otr = otr.Where(x => x.referencia == conciliacionFiltro); }
                //        if (economicoFiltro != "") { otr = otr.Where(x => x.cc == economicoFiltro); }
                //        //                    
                //        //var lstOtr = otr.ToList();
                //        if (areaCuentaFiltro != "")
                //        {
                //            string areaCuentaSplit = areaCuentaFiltro.Split(' ')[0];
                //            //lstOtr = lstOtr.Where(x => x.areaCuenta == areaCuentaSplit).ToList();
                //            otr = otr.Where(x => x.areaCuenta == areaCuentaSplit);
                //        }
                //        auxDetallesRaw = auxDetallesRaw.Concat(otr);
                //    }
                //}
                //
                if (auxDetallesRaw != null)
                {
                    detalles = auxDetallesRaw.ToList().Select(x =>
                    {
                        return new CorteDTO
                        {
                            id = x.id,
                            corteID = corteID,
                            cuenta = x.cuenta,
                            monto = x.monto * (x.cuenta != "1-1-0" && x.cuenta != "1-2-1" && x.cuenta != "1-2-2" && x.cuenta != "1-2-3" && x.cuenta != "1-3-1" && x.cuenta != "1-3-2" && !reporteCostos ? (-1) : (1)),
                            fechapol = x.fechapol,
                            tipoEquipo = x.tipoEquipo,
                            cc = x.cc,
                            poliza = x.poliza,
                            concepto = x.concepto,
                            referencia = x.referencia,
                            semana = semana,
                            empresa = x.empresa,
                            areaCuenta = x.areaCuenta,
                            linea = x.linea
                        };
                    }).ToList();
                }

                if (auxDetallesOtraEmpresaRaw != null)
                {
                    var auxDetallesCplan = auxDetallesOtraEmpresaRaw.ToList().Select(x =>
                    {
                        return new CorteDTO
                        {
                            id = x.id,
                            corteID = corteID,
                            cuenta = x.cuenta,
                            monto = x.monto * (x.cuenta != "1-1-0" && x.cuenta != "1-2-1" && x.cuenta != "1-2-2" && x.cuenta != "1-2-3" && x.cuenta != "1-3-1" && x.cuenta != "1-3-2" && !reporteCostos ? (-1) : (1)),
                            fechapol = x.fechapol,
                            tipoEquipo = x.tipoEquipo,
                            cc = x.cc,
                            poliza = x.poliza,
                            concepto = x.concepto,
                            referencia = x.referencia,
                            semana = semana,
                            empresa = x.empresa,
                            areaCuenta = x.areaCuenta,
                            linea = x.linea
                        };
                    }).ToList();

                    detalles.AddRange(auxDetallesCplan);
                }

                if (vSesiones.sesionEmpresaActual == 1)
                {
                    foreach (var item in detalles)
                    {
                        var auxAreaCuentaCplan = centrosCostosEnkontrolCplan.FirstOrDefault(x => x.Prefijo == null ? x.Value == item.areaCuenta : x.Prefijo == item.areaCuenta);
                        if (auxAreaCuentaCplan == null)
                        {
                            var auxAreaCuentaArr = centrosCostosEnkontrolArr.FirstOrDefault(x => x.Value == item.areaCuenta);
                            item.areaCuenta = auxAreaCuentaArr == null ? item.areaCuenta : auxAreaCuentaArr.Text;
                        }
                        else { item.areaCuenta = auxAreaCuentaCplan.Text; }
                    }
                }
                else
                {
                    foreach (var item in detalles)
                    {
                        var auxAreaCuentaArr = centrosCostosEnkontrolArr.FirstOrDefault(x => x.Value == item.areaCuenta);
                        if (auxAreaCuentaArr == null)
                        {
                            var auxAreaCuentaCplan = centrosCostosEnkontrolCplan.FirstOrDefault(x => x.Value == item.areaCuenta);
                            item.areaCuenta = auxAreaCuentaCplan == null ? item.areaCuenta : auxAreaCuentaCplan.Text;
                        }
                        else { item.areaCuenta = auxAreaCuentaArr.Text; }
                    }
                }
                return detalles;
            }
        }

        public List<CorteDTO> getLstKubrixCorteCostoEstimado(int corteID, DateTime fechaInicio, DateTime fechaFin, int tipoGuardado)
        {
            var centrosCostosEnkontrolArr = getListaCC().ToList();
            var centrosCostosEnkontrolCplan = getListaCCCPlan();

            var auxDetallesRaw = _context.tblM_KBCorteDet.Where(x => x.cc != null && (tipoGuardado == 1 ? x.cuenta == "1-3-2" : (tipoGuardado == 2 ? x.cuenta == "1-2-3" : x.cuenta == "1-4-0")) && x.corteID == corteID &&
                 !(x.referencia != null && x.referencia.Contains("/" + (fechaInicio.Year - 1).ToString()) && x.fechapol.Year == fechaInicio.Year)).ToList();

            //var auxDetalles = auxDetallesRaw.GroupBy(x => new { x.cuenta, x.areaCuenta, x.referencia, x.cc, x.empresa, x.concepto, x.id, x.fechapol }).ToList();
            var detalles = auxDetallesRaw.ToList().Select(x =>
            {
                return new CorteDTO
                {
                    id = x.id,
                    corteID = corteID,
                    cuenta = x.cuenta,
                    monto = x.monto * (x.cuenta == "1-1-0" || x.cuenta == "1-2-1" || x.cuenta == "1-2-2" || x.cuenta == "1-2-3" || x.cuenta == "1-3-1" || x.cuenta == "1-3-2" ? (1) : (-1)),
                    fechapol = x.fechapol,
                    //tipoEquipo = x.tipoEquipo,
                    cc = x.cc,
                    //poliza = x.Key.poliza,
                    concepto = x.concepto,
                    referencia = x.referencia,
                    semana = 1,
                    empresa = x.empresa,
                    areaCuenta = x.areaCuenta,
                    //linea = x.linea
                    tipoMov = 0
                };
            }).ToList();
            if (vSesiones.sesionEmpresaActual == 1)
            {
                foreach (var item in detalles)
                {
                    var auxAreaCuentaCplan = centrosCostosEnkontrolCplan.FirstOrDefault(x => x.Prefijo == null ? x.Value == item.areaCuenta : x.Prefijo == item.areaCuenta);
                    if (auxAreaCuentaCplan == null)
                    {
                        var auxAreaCuentaArr = centrosCostosEnkontrolArr.FirstOrDefault(x => x.Value == item.areaCuenta);
                        item.areaCuenta = auxAreaCuentaArr == null ? item.areaCuenta : auxAreaCuentaArr.Text;
                    }
                    else { item.areaCuenta = auxAreaCuentaCplan.Text; }
                }
            }
            else
            {
                foreach (var item in detalles)
                {
                    var auxAreaCuentaArr = centrosCostosEnkontrolArr.FirstOrDefault(x => x.Value == item.areaCuenta);
                    if (auxAreaCuentaArr == null)
                    {
                        var auxAreaCuentaCplan = centrosCostosEnkontrolCplan.FirstOrDefault(x => x.Value == item.areaCuenta);
                        item.areaCuenta = auxAreaCuentaCplan == null ? item.areaCuenta : auxAreaCuentaCplan.Text;
                    }
                    else { item.areaCuenta = auxAreaCuentaArr.Text; }
                }
            }
            return detalles;
        }

        public List<ComboDTO> getCuentasDesc()
        {
            try
            {
                var obj = new OdbcConsultaDTO()
                {
                    consulta = string.Format(@"SELECT CAST(cta as varchar) + '-' + CAST(scta as varchar) + '-' + CAST(sscta as varchar) as Value, descripcion as Text FROM catcta where cta = 4000 OR cta = 4900 OR cta = 4901 OR cta = 5000 OR cta = 5280 OR cta = 5900 OR cta = 5901")
                };
                var lstEkP = _contextEnkontrol.Select<ComboDTO>(vSesiones.sesionEmpresaActual == 1 ? EnkontrolEnum.CplanProd : EnkontrolEnum.ArrenProd, obj);

                var cuentasSIGOPLAN = _context.tblM_KBCatCuenta.Where(x => x.cuenta.Contains("1-")).Select(x => new ComboDTO
                {
                    Value = x.cuenta,
                    Text = x.descripcion
                }).ToList();
                lstEkP.AddRange(cuentasSIGOPLAN);
                return lstEkP;
            }
            catch (Exception)
            {
                return new List<ComboDTO>();
            }
        }

        public tblM_KBCorte getCorteByID(int corteID)
        {
            var corte = _context.tblM_KBCorte.FirstOrDefault(x => x.id == corteID);
            return corte;
        }
        public tblM_KBCorte getCorteByIDArrendadora()
        {
            using (var _db = new MainContext((int)EmpresaEnum.Arrendadora))
            {
                var corte = _db.tblM_KBCorte.Where(r=>r.tipo == 0).OrderByDescending(r => r.id).FirstOrDefault();
                return corte;
            }
        }
        public List<tblM_KBCatCuenta> getCuentasDescripcion()
        {
            var cuentas = _context.tblM_KBCatCuenta.ToList();
            return cuentas;
        }

        public List<ComboDTO> getGrupoMaquinas()
        {
            var data = _context.tblM_CatMaquina.Select(x => new ComboDTO
            {
                Value = x.grupoMaquinaria.descripcion,
                Text = x.noEconomico
            }).ToList();
            return data;
        }

        //public List<RentabilidadDTO> getLstKubrixDetalleCorte(int corteID, List<string> areaCuenta, List<int> modelos, string economico, DateTime fechaInicio, DateTime fechaFin, string cuenta, int tipo, int tipoEquipoMayor)
        //{
        //    try
        //    {
        //        List<string> auxObras = new List<string>();
        //        List<tblM_CatMaquina> maquinas = _context.tblM_CatMaquina.ToList();
        //        areaCuenta.Remove("");
        //        if (areaCuenta != null) auxObras = areaCuenta;
        //        var maquinasModelo = modelos == null ? new List<string>() : _context.tblM_CatMaquina.Where(x => modelos.Contains(x.modeloEquipoID)).Select(x => x.noEconomico).ToList();
        //        List<string> auxTipo = (new string[] { "CFC", "CF-", "MC-", "PR-", "TC-", "CAR-", "EX-", "HDT-" }).ToList();
        //        string auxTipoSingle = "";
        //        if ((tipoEquipoMayor > 0 && tipoEquipoMayor < 7) || tipoEquipoMayor > 7) auxTipoSingle = auxTipo[tipoEquipoMayor - 1];



        //        var lst =  _context.tblM_KBCorteDet.Where(x => x.cc != null && x.corteID == corteID && x.fechapol <= fechaFin && x.fechapol >= fechaInicio
        //        && (auxObras.Count() > 0 ? auxObras.Contains(x.areaCuenta) : true) && (maquinasModelo.Count() > 0 ? maquinasModelo.Contains(x.cc) : true) && ((economico == null || economico == "") ? true : x.cc == economico)
        //        && x.cuenta == cuenta && (tipo > 0 ? x.tipoEquipo == tipo : true) && (tipoEquipoMayor > 0 ? (tipoEquipoMayor == 7 ? !auxTipo.Any(s => x.cc.Contains(s)) : ((tipoEquipoMayor < 7 || tipoEquipoMayor > 7) ? x.cc.Contains(auxTipoSingle) : true)) : true)).ToList();

        //        var listaFinal = lst.Select(x =>
        //        {
        //            var grupoMaquina = "OTROS";
        //            var maquina = maquinas.FirstOrDefault(y => y.noEconomico == x.cc);
        //            if (maquina != null) grupoMaquina = maquina.grupoMaquinaria.descripcion;
        //            return new RentabilidadDTO
        //            {
        //                noEco = x.cc,
        //                cta = 0,
        //                hrsTrabajadas = 0,
        //                tipoInsumo = "",
        //                tipoInsumo_Desc = "",
        //                grupoInsumo = "",
        //                grupoInsumo_Desc = "",
        //                insumo = 0,
        //                insumo_Desc = x.concepto,
        //                areaCuenta = "",
        //                tipo_mov = 0,
        //                importe = x.monto,
        //                fecha = x.fechapol,
        //                tipo = x.tipoEquipo,
        //                poliza = x.poliza,
        //                cc = "",
        //                area = 0,
        //                cuenta = 0,
        //            };
        //        }).ToList();


        //        return listaFinal;
        //    }
        //    catch (Exception o_O)
        //    {
        //        return new List<RentabilidadDTO>();
        //    }
        //}
        public bool GuardadoLineaCorte(int id, int corteID, string concepto, decimal monto, string cc, string ac, DateTime fecha, string conciliacion, int empresa, int tipoGuardado)
        {
            try
            {
                if (id > 0)
                {
                    var linea = _context.tblM_KBCorteDet.FirstOrDefault(x => x.id == id);
                    if (linea != null)
                    {
                        if (linea.areaCuenta != ac) linea.areaCuenta = ac;
                        if (linea.cc != cc) { linea.cc = cc; GetTipoMaquinaCorte(new List<tblM_KBCorteDet> { linea }); }
                        if (linea.concepto != concepto) linea.concepto = concepto;
                        if (linea.empresa != empresa) linea.empresa = empresa;
                        if (linea.fechapol != fecha) linea.fechapol = fecha;
                        if (linea.monto != monto) linea.monto = monto;
                        if (linea.referencia != conciliacion) linea.referencia = conciliacion;
                    }
                    else { return false; }
                }
                else
                {
                    tblM_KBCorteDet data = new tblM_KBCorteDet();
                    data.areaCuenta = ac;
                    data.cc = cc;
                    data.concepto = concepto;
                    data.corteID = corteID;
                    data.cuenta = tipoGuardado == 1 ? "1-3-2" : (tipoGuardado == 2 ? "1-2-3" : "1-4-0");
                    data.empresa = empresa;
                    data.fechapol = fecha;
                    data.id = 0;
                    data.linea = 0;
                    data.monto = monto;
                    data.poliza = "--";
                    data.referencia = conciliacion;
                    data.tipoEquipo = 0;
                    GetTipoMaquinaCorte(new List<tblM_KBCorteDet> { data });
                    _context.tblM_KBCorteDet.Add(data);
                }
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool EliminarLineaCorte(int id)
        {
            try
            {
                var linea = _context.tblM_KBCorteDet.FirstOrDefault(x => x.id == id);
                if (linea != null)
                {
                    _context.tblM_KBCorteDet.Remove(linea);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception e) { return false; }
        }

        public bool CheckCostoEstimadoCerrado(int corteID)
        {
            try
            {
                var data = _context.tblM_KBCorte.FirstOrDefault(x => x.id == corteID);
                if (data != null) return data.costoEstCerrado;
                else return false;
            }
            catch (Exception e) { return false; }
        }

        public bool CerrarCostoEst(int corteID)
        {
            try
            {
                var data = _context.tblM_KBCorte.FirstOrDefault(x => x.id == corteID);
                if (data != null)
                {
                    data.costoEstCerrado = true;
                    _context.SaveChanges();
                    return true;
                }
                else return false;
            }
            catch (Exception e) { return false; }
        }

        #endregion
        #region CXP
        public List<CuentasPendientesDTO> getLstCXP(BusqKubrixDTO busq)
        {
            try
            {
                var obj = new OdbcConsultaDTO()
                {
                    consulta = queryLstCXPCplan(),
                    parametros = new List<OdbcParameterDTO>() { 
                        new OdbcParameterDTO() { nombre = "fechavenc", tipo = OdbcType.Date, valor = busq.fechaFin }
                    }
                };
                var lstEkP = _contextEnkontrol.Select<CuentasPendientesDTO>(EnkontrolEnum.CplanProd, obj);
                var lst = lstEkP.ToList();
                return lst;
            }
            catch (Exception) { return new List<CuentasPendientesDTO>(); }
        }
        string queryLstCXP()
        {
            return string.Format(@"
            select 
                b.nombre as responsable, 
                x.factura as factura, 
                x.total as monto, 
                x.concepto as concepto, 
                fecha as fecha, 
                ( select top 1(CAST(area as varchar) + '-' + CAST(cuenta as varchar)) from so_orden_compra_det where x.cc = cc and x.numero = numero) as areaCuenta, 
                ISNULL((select top 1(descripcion) from si_area_cuenta where area = (select top 1(area) from so_orden_compra_det where x.cc = cc and x.numero = numero) and cuenta = (select top 1(cuenta) from so_orden_compra_det where x.cc = cc and x.numero = numero)), '--') as areaCuentaDesc 
            from
            (SELECT 
                numpro, factura, sum(total * tipocambio) as total, MAX(CASE WHEN (tm < 26 or tm = 99) THEN concepto ELSE '' END) as concepto, MAX(CASE WHEN (tm < 26 or tm = 99) THEN cc ELSE '' END) as cc, MAX(CASE WHEN (tm < 26 or tm = 99) THEN fechavenc ELSE '1900-01-01' END) as fecha
                , MAX(CASE WHEN (tm < 26 or tm = 99) THEN CAST(referenciaoc AS numeric) ELSE -1 END) as numero
            FROM 
                sp_movprov a
            group by numpro, factura) x
            LEFT JOIN sp_proveedores b ON x.numpro = b.numpro
            where x.total > 1"
            );
        }

        string queryLstCXPCplan() 
        {
            return string.Format(@"
                SELECT 
                    proveedor.nombre AS responsable, 
                    movimiento.factura AS factura, 
                    movimiento.total AS monto, 
                    movimiento.concepto AS concepto, 
                    movimiento.fecha AS fecha,
                    movimiento.cc AS areaCuenta,
                    (SELECT descripcion FROM cc tablaCC WHERE tablaCC.cc = movimiento.cc) as areaCuentaDesc
                FROM
                (
                    SELECT 
                        numpro, 
                        factura, sum(total * tipocambio) AS total,
                        MAX(CASE WHEN (tm < 26 or tm = 99) THEN concepto ELSE '' END) AS concepto, 
                        MAX(CASE WHEN (tm < 26 or tm = 99) THEN cc ELSE '' END) AS cc, 
                        MAX(CASE WHEN (tm < 26 or tm = 99) THEN fechavenc ELSE '1900-01-01' END) AS fecha,
                        MAX(CASE WHEN (tm < 26 or tm = 99) THEN CAST(referenciaoc AS numeric) ELSE -1 END) AS numero
                    FROM 
                        sp_movprov
                    GROUP BY 
                        numpro,
                        factura
                    ) movimiento
                LEFT JOIN 
                    sp_proveedores proveedor 
                ON 
                    movimiento.numpro = proveedor.numpro
                WHERE movimiento.total > 1
            ");
        }

        public List<CuentasPendientesDTO> getLstCXC(BusqKubrixDTO busq)
        {
            try
            {
                var lstParametros = new List<OdbcParameterDTO>();
                List<string> ccs = busq.ccEnkontrol;
                List<string> ccsTemp = ccs;

                if (busq.idDivision != null )
                {
                    //var lstCCsDivision = _context.tblCXC_DivisionDetalle.Where(e => e.estatus && e.divisionID == busq.idDivision).ToList();
                    var lstCCsDivision = _context.tblC_CCDivision.Where(e => e.division == busq.idDivision).ToList();
                    List<string> lstIdsCcs = lstCCsDivision.Select(e => e.cc).ToList();

                    if (ccs == null || ccs.Count() == 0)
                    {
                        ccs = new List<string>();
                        ccs.AddRange(lstIdsCcs);
                    }
                    else
                    {
                        foreach (var item in ccsTemp)
                        {
                            if (!lstIdsCcs.Contains(item))
                            {
                                ccs.Remove(item);
                            }
                        }
                    }

                }

                if (ccs == null) ccs = new List<string>();
                //lstParametros.Add(new OdbcParameterDTO() { nombre = "fechavenc", tipo = OdbcType.Date, valor = busq.fechaFin });
                lstParametros.AddRange(ccs.Select(s => new OdbcParameterDTO() { nombre = "cc", tipo = OdbcType.VarChar, valor = s }));
                var obj = new OdbcConsultaDTO()
                {
                    consulta = queryLstCXCCplan(ccs),
                    parametros = lstParametros
                };
                var lstEkP = _contextEnkontrol.Select<CuentasPendientesDTO>(EnkontrolEnum.CplanProd, obj);

                #region Colombia
                var objColombia = new OdbcConsultaDTO()
                {
                    consulta = queryLstCXCColombia(ccs),
                    parametros = lstParametros
                };
                var lstEkPColombia = _contextEnkontrol.Select<CuentasPendientesDTO>(EnkontrolEnum.CplanProd, objColombia);

                if (lstEkPColombia.Count() > 0)
                {
                    string consultaENKColombia = @"
                        SELECT 
                            B.nombre as responsable, A.numcte, A.factura, A.cc as areaCuenta, A.fecha, A.fechavenc as fechaOrig
                        FROM DBA.sx_movcltes A LEFT JOIN DBA.sx_clientes B ON A.numcte = B.numcte
                        WHERE ";
                    List<tblP_CC> ccsColombia = new List<tblP_CC>();
                    using (var _db = new MainContext((int)EmpresaEnum.Colombia))
                    {
                        ccsColombia = _db.tblP_CC.ToList();
                    }
                    foreach (var item in lstEkPColombia)
                    {
                        var _ccColombia = ccsColombia.FirstOrDefault(x => x.ccRH == item.areaCuenta);
                        var _ccColombiaStr = _ccColombia == null ? "001" : _ccColombia.cc;
                        consultaENKColombia += "(cc = '" + _ccColombiaStr + "' AND factura = '" + item.factura + "') OR";
                    }
                    consultaENKColombia = consultaENKColombia.Remove(consultaENKColombia.Length - 2);

                    objColombia = new OdbcConsultaDTO()
                    {
                        consulta = consultaENKColombia
                    };
                    var lstFacturasColombia = _contextEnkontrol.Select<CuentasPendientesDTO>(EnkontrolEnum.ColombiaProductivo, objColombia);
                    foreach (var item in lstEkPColombia)
                    {
                        var _ccColombia = ccsColombia.FirstOrDefault(x => x.ccRH == item.areaCuenta);
                        var _ccColombiaStr = _ccColombia == null ? "001" : _ccColombia.cc;
                        var facturaColombia = lstFacturasColombia.FirstOrDefault(x => x.areaCuenta == _ccColombiaStr && x.factura == item.factura);
                        if (facturaColombia != null) 
                        {
                            item.numcte = facturaColombia.numcte;
                            item.responsable = facturaColombia.responsable;
                            item.fecha = facturaColombia.fecha;
                            item.fechaOrig = facturaColombia.fechaOrig;
                        }
                    }
                    lstEkP.AddRange(lstEkPColombia);
                }
                #endregion

                #region Perú
                var objPeru = new OdbcConsultaDTO()
                {
                    consulta = queryLstCXCPeru(ccs),
                    parametros = lstParametros
                };
                var lstEkPPeru = _contextEnkontrol.Select<CuentasPendientesDTO>(EnkontrolEnum.CplanProd, objPeru);

                foreach (var item in lstEkPPeru)
                {
                    if (true)
                    {
                        item.numcte = "20100123500";
                        item.responsable = "NEXA RESOURCES ATACOCHA S.A.A.";
                    }
                }

                lstEkP.AddRange(lstEkPPeru);
                #endregion

                //List<>

                //var objCortes = _context.tblCXC_Corte.FirstOrDefault(e => e.fechaInicio.Date < busq.fechaFin.Date && e.fechaFin.Date > busq.fechaFin.Date);
                //var lstIdsCorte = lstCortes.Select(e => e.id).ToList();
                //var lstDetCorte = _context.tblCXC_Corte_Det.Where(e => e.esActivo && objCortes.id == e.idCorte).ToList();
                var lstDetCorte = _context.tblCXC_Corte_Det.Where(e => e.esActivo).ToList();
                var lstDetCorteRemovidos = _context.tblCXC_Corte_Det.Where(e => e.esRemoved).ToList();
                var lstConvenios = _context.tblCXC_Convenios.Where(e => e.esActivo).ToList();
                var lstIdsConvenios = lstConvenios.Select(e => e.id).ToList();
                var lstConveniosDet = _context.tblCXC_Convenios_Det.Where(e => e.esActivo && lstIdsConvenios.Contains(e.idAcuerdo)).ToList();
                var lstFacturasMod = _context.tblCXC_FacturasMod.Where(e => e.esActivo).ToList();
                var lstIdsFacturasMod = lstFacturasMod.Select(e => e.factura.ToString()).ToList();

                foreach (var item in lstEkP)
                {
                    item.montoPagado = 0;

                    var objDetCorte = lstDetCorte.FirstOrDefault(e => e.idFactura.ToString() == item.factura);
                    if (objDetCorte != null)
                    {
                        if (objDetCorte.esRemoved)
                        {

                            item.esCorte = false;
                            item.esRemoved = true;
                            item.comentarios = objDetCorte.comentariosRemove;

                        }
                        else
                        {
                            
                            var objConvenio = lstConvenios.FirstOrDefault(e => e.idFactura.ToString() == item.factura);
                            if (objConvenio != null)
                            {

                                //if (objConvenio.esAutorizar && objConvenio.estatus != EstatusConvenioEnum.APROBADO)
                                //{
                                //    item.montoPronosticado = 0;
                                //    item.idAcuerdo = objConvenio.id;
                                //}
                                //else
                                //{
                                //    var lstAbonosConvenio = lstConveniosDet.Where(e => e.idAcuerdo == objConvenio.id).OrderBy(e => e.fechaDet).ToList();

                                //    var objConvenioDet = lstAbonosConvenio.FirstOrDefault(e => e.fechaDet.Date >= busq.fechaFin.Date);

                                //    if (objConvenioDet != null)
                                //    {
                                //        item.fechaCorte = objConvenioDet.fechaDet;
                                //        item.montoPronosticado = objConvenioDet.abonoDet;
                                //        item.idAcuerdo = objConvenio.id;
                                //    }
                                //    else
                                //    {
                                //        item.montoPronosticado = 0;
                                //        item.idAcuerdo = objConvenio.id;
                                //    }

                                //}

                                var lstAbonosConvenio = lstConveniosDet.Where(e => e.idAcuerdo == objConvenio.id).OrderBy(e => e.fechaDet).ToList();

                                var objConvenioDet = lstAbonosConvenio.FirstOrDefault(e => e.fechaDet.Date >= busq.fechaFin.Date);

                                if (objConvenioDet != null)
                                {
                                    item.fechaCorte = objConvenioDet.fechaDet;
                                    item.montoPronosticado = objConvenioDet.abonoDet;
                                    item.idAcuerdo = objConvenio.id;
                                }
                                else
                                {
                                    item.montoPronosticado = 0;
                                    item.idAcuerdo = objConvenio.id;
                                }

                            }
                            else
                            {
                                

                                item.fechaCorte = item.fecha;
                                item.montoPronosticado = item.monto;

                            }


                            item.esCorte = true;
                            item.esRemoved = false;

                            
                        }
                    }
                    else
                    {
                        var objFacturaRemovida = lstDetCorteRemovidos.FirstOrDefault(e => e.idFactura.ToString() == item.factura);

                        if (objFacturaRemovida != null)
                        {
                            item.esCorte = false;
                            item.esRemoved = true;
                            item.comentarios = objFacturaRemovida.comentariosRemove;
                        }
                        else
                        {
                            item.esRemoved = false;
                            //SI ESTA VENCIDA AGREGAR AL CORTE
                            //if (item.fecha.Date < busq.fechaFin.Date)
                            //{
                            //    item.montoPronosticado = item.monto;

                            //    item.esCorte = true;
                            //}
                            //else
                            //{
                            //    item.esCorte = false;

                            //}
                        }
                    }

                    //ALMACENAR LA PRIMERA FECHA DE VENCIMIENTO DE LA FACTURA
                    item.fechaOGVenc = item.fecha;

                    if (lstIdsFacturasMod.Contains(item.factura))
                    {
                        var objFacturaMod = lstFacturasMod.FirstOrDefault(e => e.factura.ToString() == item.factura);

                        if (objFacturaMod != null)
                        {
                            item.fecha = objFacturaMod.fechaNueva;
                            item.fechaCorte = objFacturaMod.fechaNueva;

                        }
                    }
                }
                var lst = lstEkP.ToList();
                return lst;
            }
            catch (Exception e) { 
                return new List<CuentasPendientesDTO>(); 
            }
        }

        public MemoryStream DescargarExcelCXC(List<CuentasPendientesDTO> data, DateTime fechaCorte)
        {
            try
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    var hoja1 = excel.Workbook.Worksheets.Add("CCs");

                    #region HEADER
                    var imgLogo = Image.FromFile(HttpContext.Current.Server.MapPath("\\Content\\img\\logo\\logo.jpg"));
                    var logo = hoja1.Drawings.AddPicture("logo", imgLogo);
                    logo.SetPosition(1, 0, 1, 0);

                    hoja1.Cells[4, 3, 4, 6].Merge = true;
                    hoja1.Cells[hoja1.MergedCells[0]].Value = "Grupo Construcciones Planificadas S.A. de C.V.";
                    hoja1.Cells[hoja1.MergedCells[0]].Style.Font.Bold = true;
                    hoja1.Cells[hoja1.MergedCells[0]].Style.Font.Size = 20;
                    hoja1.Cells[hoja1.MergedCells[0]].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    hoja1.Cells[hoja1.MergedCells[0]].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    hoja1.Cells[5, 3, 5, 6].Merge = true;
                    hoja1.Cells[hoja1.MergedCells[1]].Value = "Saldos de Clientes";
                    hoja1.Cells[hoja1.MergedCells[1]].Style.Font.Bold = true;
                    hoja1.Cells[hoja1.MergedCells[1]].Style.Font.Size = 16;
                    hoja1.Cells[hoja1.MergedCells[1]].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    hoja1.Cells[hoja1.MergedCells[1]].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    hoja1.Cells[6, 3, 6, 6].Merge = true;
                    hoja1.Cells[hoja1.MergedCells[2]].Value = "Fecha de Corte: " + fechaCorte.ToShortDateString();
                    hoja1.Cells[hoja1.MergedCells[2]].Style.Font.Size = 12;
                    hoja1.Cells[hoja1.MergedCells[2]].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    hoja1.Cells[hoja1.MergedCells[2]].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    hoja1.Cells["B9:F9"].LoadFromArrays(new List<string[]>() { new string[] { "CC", "Descripción CC", "# Cliente", "Descripción Cliente", "Por Vencer" } });
                    hoja1.Cells["B9:F9"].Style.Font.Bold = true;
                    hoja1.Cells["B9:F9"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Cells["B9:F9"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    #endregion

                    #region DATOS
                    var cellData = new List<object[]>();

                    var datosAgrupados = data.GroupBy(x => new { x.areaCuenta, x.areaCuentaDesc, x.numcte, x.responsable }).Select(x => new
                    {
                        areaCuenta = x.Key.areaCuenta,
                        areaCuentaDesc = x.Key.areaCuentaDesc,
                        numcte = x.Key.numcte,
                        responsable = x.Key.responsable,
                        monto = x.Sum(y => y.monto)
                    }).OrderBy(x => x.areaCuenta).ThenBy(x => x.numcte).ToList();

                    int contadorRenglon = 10;

                    foreach (var d in datosAgrupados)
                    {
                        cellData.Add(new object[] { d.areaCuenta, d.areaCuentaDesc, d.numcte, d.responsable, d.monto });

                        hoja1.Cells[contadorRenglon, 6].Style.Numberformat.Format = "$###,###,##0.00";

                        contadorRenglon++;
                    }

                    hoja1.Cells[10, 2].LoadFromArrays(cellData);
                    #endregion

                    ExcelRange range = hoja1.Cells[9, 2, hoja1.Dimension.End.Row, hoja1.Dimension.End.Column];
                    ExcelTable tab = hoja1.Tables.Add(range, "Tabla1");
                    tab.TableStyle = TableStyles.Medium2;

                    excel.Compression = OfficeOpenXml.CompressionLevel.BestSpeed;

                    hoja1.Cells[hoja1.Dimension.Address].AutoFitColumns();

                    var bytes = new MemoryStream();

                    using (var exportData = new MemoryStream())
                    {
                        excel.SaveAs(exportData);
                        bytes = exportData;
                    }

                    return bytes;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public Dictionary<string,object> getLstCXCReporte(BusqKubrixDTO busq)
        {
            resultado.Clear();
            try
            {
                var lstParametros = new List<OdbcParameterDTO>();
                List<string> ccs = busq.ccEnkontrol;
                //List<string> ccsTemp = ccs;

                //if (busq.idDivision != null)
                //{
                //    var lstCCsDivision = _context.tblM_KBDivisionDetalle.Where(e => e.estatus && e.divisionID == busq.idDivision).ToList();
                //    List<string> lstIdsCcs = lstCCsDivision.Select(e => e.ac.cc).ToList();

                //    if (ccs == null || ccs.Count() == 0)
                //    {
                //        ccs = new List<string>();
                //        ccs.AddRange(lstIdsCcs);
                //    }
                //    else
                //    {
                //        foreach (var item in ccsTemp)
                //        {
                //            if (!lstIdsCcs.Contains(item))
                //            {
                //                ccs.Remove(item);
                //            }
                //        }
                //    }

                //}

                if (ccs == null) ccs = new List<string>();
                //lstParametros.Add(new OdbcParameterDTO() { nombre = "fechavenc", tipo = OdbcType.Date, valor = busq.fechaFin });
                lstParametros.AddRange(ccs.Select(s => new OdbcParameterDTO() { nombre = "cc", tipo = OdbcType.VarChar, valor = s }));
                var obj = new OdbcConsultaDTO()
                {
                    consulta = queryLstCXCCplan(ccs),
                    parametros = lstParametros
                };
                var lstEkP = _contextEnkontrol.Select<CuentasPendientesDTO>(EnkontrolEnum.CplanProd, obj);

                //var objCortes = _context.tblCXC_Corte.FirstOrDefault(e => e.fechaInicio.Date < busq.fechaFin.Date && e.fechaFin.Date > busq.fechaFin.Date);
                //var lstIdsCorte = lstCortes.Select(e => e.id).ToList();
                //var lstDetCorte = _context.tblCXC_Corte_Det.Where(e => e.esActivo && objCortes.id == e.idCorte).ToList();
                var lstDetCorte = _context.tblCXC_Corte_Det.Where(e => e.esActivo).ToList();
                var lstDetCorteRemovidos = _context.tblCXC_Corte_Det.Where(e => e.esRemoved).ToList();
                var lstConvenios = _context.tblCXC_Convenios.Where(e => e.esActivo).ToList();
                var lstIdsConvenios = lstConvenios.Select(e => e.id).ToList();
                var lstConveniosDet = _context.tblCXC_Convenios_Det.Where(e => e.esActivo && lstIdsConvenios.Contains(e.idAcuerdo)).ToList();

                var lstDivision = _context.tblM_KBDivision.Where(e => e.estatus).ToList();
                var lstCCsDivisionDet = _context.tblM_KBDivisionDetalle.Where(e => e.estatus).ToList();
                List<string> lstCcsDiv = lstCCsDivisionDet.Select(e => e.ac.cc).ToList();

                //decimal totalImporte = 0;
                //decimal totalPronostico = 0;
                //List<string> lstClientesCorte = new List<string>();
                //List<string> lstCCsCorte = new List<string>();
                //int numFacturas = 0;

                foreach (var item in lstEkP)
                {
                    item.montoPagado = 0;

                    

                    if (lstCcsDiv.Contains(item.areaCuenta))
                    {
                        var objDetDivision = lstCCsDivisionDet.FirstOrDefault(e => e.ac.cc == item.areaCuenta);
                        var objDivision = lstDivision.FirstOrDefault(e => e.id == objDetDivision.divisionID);
                        if (objDivision != null)
                        {
                            item.idDivision = objDivision.id;
                            item.descDivision = objDivision.division;
                        }
                        else
                        {
                            item.idDivision = 0;
                            item.descDivision = "SIN DIVISION";
                        }

                    }
                    else
                    {
                        item.idDivision = 0;
                        item.descDivision = "SIN DIVISION";
                    }

                    var objDetCorte = lstDetCorte.FirstOrDefault(e => e.idFactura.ToString() == item.factura);
                    if (objDetCorte != null)
                    {
                        if (!objDetCorte.esRemoved)
                        {
                            var objConvenio = lstConvenios.FirstOrDefault(e => e.idFactura.ToString() == item.factura);
                            if (objConvenio != null)
                            {
                                //if (objConvenio.esAutorizar && objConvenio.estatus != EstatusConvenioEnum.APROBADO)
                                //{
                                //    item.montoPronosticado = 0;
                                //    item.idAcuerdo = objConvenio.id;
                                //}
                                //else
                                //{
                                //    var lstAbonosConvenio = lstConveniosDet.Where(e => e.idAcuerdo == objConvenio.id).OrderBy(e => e.fechaDet).ToList();

                                //    var objConvenioDet = lstAbonosConvenio.FirstOrDefault(e => e.fechaDet.Date >= busq.fechaFin.Date);

                                //    if (objConvenioDet != null)
                                //    {

                                //        item.montoPronosticado = objConvenioDet.abonoDet;
                                //        item.idAcuerdo = objConvenio.id;
                                //    }
                                //    else
                                //    {
                                //        item.montoPronosticado = 0;
                                //        item.idAcuerdo = objConvenio.id;
                                //    }

                                //}

                                var lstAbonosConvenio = lstConveniosDet.Where(e => e.idAcuerdo == objConvenio.id).OrderBy(e => e.fechaDet).ToList();

                                var objConvenioDet = lstAbonosConvenio.FirstOrDefault(e => e.fechaDet.Date >= busq.fechaFin.Date);

                                if (objConvenioDet != null)
                                {

                                    item.montoPronosticado = objConvenioDet.abonoDet;
                                    item.idAcuerdo = objConvenio.id;
                                }
                                else
                                {
                                    item.montoPronosticado = 0;
                                    item.idAcuerdo = objConvenio.id;
                                }

                            }
                            else
                            {

                                item.montoPronosticado = item.monto;
                                //if (item.fecha.Date > busq.fechaFin.Date)
                                //{

                                //}

                            }
                            item.esCorte = true;
                            item.esRemoved = false;


                            //totalImporte += item.monto;
                            //totalPronostico += item.montoPronosticado;
                            //lstClientesCorte.Add(item.numcte);
                            //lstCCsCorte.Add(item.areaCuenta);
                        }
                        else
                        {
                            item.esCorte = false;
                        }
                    }
                }

                var lst = lstEkP.ToList();
                resultado.Add(ITEMS, lst);
                //resultado.Add("totalImporte", totalImporte);
                //resultado.Add("totalPronostico", totalPronostico);
                //resultado.Add("numClientes", lstClientesCorte.Distinct().Count());
                //resultado.Add("numCCs", lstCCsCorte.Distinct().Count());
                resultado.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);

            }
            return resultado;
        }


        string queryLstCXC()
        {
            return string.Format(@"
                select 
                    b.nombre as responsable, 
                    x.factura as factura, 
                    x.total as monto, 
                    x.concepto as concepto, 
                    x.fecha as fecha, 
                    x.fechaOrig as fechaOrig,
                    ( select top 1(CAST(area as varchar) + '-' + CAST(cuenta_oc as varchar)) from sc_movpol where x.year = year and x.mes = mes and tp = '08' and x.poliza = poliza) as areaCuenta, 
                    ISNULL((select top 1(descripcion) from si_area_cuenta where area = (select top 1(area) from sc_movpol where x.year = year and x.mes = mes and tp = '08' and x.poliza = poliza) and cuenta = (select top 1(cuenta_oc) from sc_movpol where x.year = year and x.mes = mes and tp = '08' and x.poliza = poliza)), '--') as areaCuentaDesc 
                from
                (SELECT 
                    a.numcte, a.factura, sum(a.total * a.tipocambio) as total, MAX(CASE WHEN (a.tm < 26 or a.tm = 99) THEN a.concepto ELSE '' END) as concepto, 
                    MAX(CASE WHEN (a.tm < 26 or a.tm = 99) THEN a.cc ELSE '' END) as cc, MAX(CASE WHEN (a.tm < 26 or a.tm = 99) THEN a.fechavenc ELSE '1900-01-01' END) as fecha,
                    MAX(CASE WHEN (a.tm < 26 or a.tm = 99) THEN a.cc ELSE '' END) as cc2, MAX(CASE WHEN (a.tm < 26 or a.tm = 99) THEN a.fecha ELSE '1900-01-01' END) as fechaOrig,
                    MAX(CASE WHEN (a.tm < 26 or a.tm = 99) THEN CAST(a.referenciaoc AS numeric) ELSE -1 END) as numero, MAX(CASE WHEN (a.tp = '08') THEN a.year ELSE '' END) as year, 
                    MAX(CASE WHEN (a.tp = '08') THEN a.mes ELSE '' END) as mes, MAX(CASE WHEN (a.tp = '08') THEN a.poliza ELSE '' END) as poliza
                FROM 
                    sx_movcltes a
                group by numcte, factura) x
                LEFT JOIN sx_clientes b ON x.numcte = b.numcte
                where x.total > 1"
                );
        }

        string queryLstCXCCplan(List<string> CCs) 
        {
            if (CCs.Count() > 0)
            {
                return string.Format(@"
            select 
                b.nombre as responsable, 
                x.numcte as numcte, 
                x.factura as factura, 
                x.total as monto, 
                x.concepto as concepto, 
                x.fecha as fecha, 
                x.cc as areaCuenta, 
                x.fechaOrig as fechaOrig,
                (SELECT descripcion FROM cc tablaCC WHERE tablaCC.cc = x.cc) as areaCuentaDesc
            from
            (SELECT 
                a.numcte, a.factura, sum(a.total * a.tipocambio) as total, MAX(CASE WHEN (a.tm < 26 or a.tm = 99) THEN a.concepto ELSE '' END) as concepto, 
                MAX(CASE WHEN (a.tm < 26 or a.tm = 99) THEN a.cc ELSE '' END) as cc, MAX(CASE WHEN (a.tm < 26 or a.tm = 99) THEN a.fechavenc ELSE '1900-01-01' END) as fecha,
                MAX(CASE WHEN (a.tm < 26 or a.tm = 99) THEN a.cc ELSE '' END) as cc2, MAX(CASE WHEN (a.tm < 26 or a.tm = 99) THEN a.fecha ELSE '1900-01-01' END) as fechaOrig,
                MAX(CASE WHEN (a.tm < 26 or a.tm = 99) THEN CAST(a.referenciaoc AS numeric) ELSE -1 END) as numero, MAX(CASE WHEN (a.tp = '08') THEN a.year ELSE '' END) as year, 
                MAX(CASE WHEN (a.tp = '08') THEN a.mes ELSE '' END) as mes, MAX(CASE WHEN (a.tp = '08') THEN a.poliza ELSE '' END) as poliza
            FROM 
                (select * from sx_movcltes where cc in {0}) a
            group by numcte, factura) x
            LEFT JOIN sx_clientes b ON x.numcte = b.numcte
            where x.total > 1
            ", CCs.ToParamInValue());
            }
            else {
                return string.Format(@"
            select 
                b.nombre as responsable, 
                x.numcte as numcte, 
                x.factura as factura, 
                x.total as monto, 
                x.concepto as concepto, 
                x.fecha as fecha, 
                x.cc as areaCuenta, 
                x.fechaOrig as fechaOrig,
                (SELECT descripcion FROM cc tablaCC WHERE tablaCC.cc = x.cc) as areaCuentaDesc
            from
            (SELECT 
                a.numcte, a.factura, sum(a.total * a.tipocambio) as total, MAX(CASE WHEN (a.tm < 26 or a.tm = 99) THEN a.concepto ELSE '' END) as concepto, 
                MAX(CASE WHEN (a.tm < 26 or a.tm = 99) THEN a.cc ELSE '' END) as cc, MAX(CASE WHEN (a.tm < 26 or a.tm = 99) THEN a.fechavenc ELSE '1900-01-01' END) as fecha,
                MAX(CASE WHEN (a.tm < 26 or a.tm = 99) THEN a.cc ELSE '' END) as cc2, MAX(CASE WHEN (a.tm < 26 or a.tm = 99) THEN a.fecha ELSE '1900-01-01' END) as fechaOrig,
                MAX(CASE WHEN (a.tm < 26 or a.tm = 99) THEN CAST(a.referenciaoc AS numeric) ELSE -1 END) as numero, MAX(CASE WHEN (a.tp = '08') THEN a.year ELSE '' END) as year, 
                MAX(CASE WHEN (a.tp = '08') THEN a.mes ELSE '' END) as mes, MAX(CASE WHEN (a.tp = '08') THEN a.poliza ELSE '' END) as poliza
            FROM 
                (select * from sx_movcltes) a
            group by numcte, factura) x
            LEFT JOIN sx_clientes b ON x.numcte = b.numcte
            where x.total > 1
            ");
            }
        }

        string queryLstCXCColombia(List<string> CCs)
        {
            if (CCs.Count() > 0)
            {
                return string.Format(@"
                SELECT * FROM (
                    SELECT
                        '' AS responsable, 
                        '' AS numcte, 
                        A.referencia AS factura, 
                        SUM(A.monto) as monto, 
                        MIN(A.concepto) as concepto, 
                        GETDATE() AS fecha, 
                        A.cc AS areaCuenta, 
                        GETDATE() AS fechaOrig, 
                        (SELECT tablaCC.descripcion FROM cc tablaCC WHERE tablaCC.cc = A.cc) AS areaCuentaDesc
                    FROM sc_movpol A WHERE A.cta = 1125 AND A.scta = 9998 AND A.year >= 2023 AND cc IN {0} group by A.cc, A.referencia) X
                WHERE X.monto NOT BETWEEN -1 AND 1
                ", CCs.ToParamInValue());
            }
            else
            {
                return string.Format(@"
                    SELECT * FROM (
                        SELECT
                            '' AS responsable, 
                            '' AS numcte, 
                            A.referencia AS factura, 
                            SUM(A.monto) as monto, 
                            MIN(A.concepto) as concepto, 
                            GETDATE() AS fecha, 
                            A.cc AS areaCuenta, 
                            GETDATE() AS fechaOrig, 
                            (SELECT tablaCC.descripcion FROM cc tablaCC WHERE tablaCC.cc = A.cc) AS areaCuentaDesc
                        FROM sc_movpol A WHERE A.cta = 1125 AND A.scta = 9998 AND A.year >= 2023 group by A.cc, A.referencia) X
                    WHERE X.monto NOT BETWEEN -1 AND 1

                ");
            }
        }

        string queryLstCXCPeru(List<string> CCs)
        {
            if (CCs.Count() > 0)
            {
                return string.Format(@"
                    SELECT * FROM (
                        SELECT 
                            '' AS responsable, 
                            '' AS numcte, 
                            X.factura AS factura, 
                            SUM(X.monto) as monto, 
                            MIN(X.concepto) as concepto, 
                            MIN(X.fecha) AS fecha, 
                            X.cc AS areaCuenta, 
                            MIN(X.fecha) AS fechaOrig, 
                            (SELECT tablaCC.descripcion FROM cc tablaCC WHERE tablaCC.cc = X.cc) AS areaCuentaDesc
                        FROM (
                            SELECT
                                '' AS responsable, 
                                '' AS numcte, 
                                A.referencia AS factura, 
                                A.monto, 
                                A.concepto, 
                                (SELECT TOP 1 fechapol FROM sc_polizas B WHERE A.year = B.year AND A.mes = B.mes AND A.tp = B.tp AND A.poliza = B.poliza) AS fecha, 
                                A.cc, 
                                (SELECT tablaCC.descripcion FROM cc tablaCC WHERE tablaCC.cc = A.cc) AS areaCuentaDesc
                            FROM sc_movpol A WHERE A.cta = 1125 AND A.scta = 9999 AND A.year >= 2023 AND cc IN {0}) X group by X.cc, X.factura
                        ) Y
                    WHERE Y.monto NOT BETWEEN -1 AND 1
                ", CCs.ToParamInValue());
            }
            else
            {
                return string.Format(@"
                    SELECT * FROM (
                        SELECT 
                            '' AS responsable, 
                            '' AS numcte, 
                            X.factura AS factura, 
                            SUM(X.monto) as monto, 
                            MIN(X.concepto) as concepto, 
                            MIN(X.fecha) AS fecha, 
                            X.cc AS areaCuenta, 
                            MIN(X.fecha) AS fechaOrig, 
                            (SELECT tablaCC.descripcion FROM cc tablaCC WHERE tablaCC.cc = X.cc) AS areaCuentaDesc
                        FROM (
                            SELECT
                                '' AS responsable, 
                                '' AS numcte, 
                                A.referencia AS factura, 
                                A.monto, 
                                A.concepto, 
                                (SELECT TOP 1 fechapol FROM sc_polizas B WHERE A.year = B.year AND A.mes = B.mes AND A.tp = B.tp AND A.poliza = B.poliza) AS fecha, 
                                A.cc, 
                                (SELECT tablaCC.descripcion FROM cc tablaCC WHERE tablaCC.cc = A.cc) AS areaCuentaDesc
                            FROM sc_movpol A WHERE A.cta = 1125 AND A.scta = 9999 AND A.year >= 2023) X group by X.cc, X.factura
                        ) Y
                    WHERE Y.monto NOT BETWEEN -1 AND 1
                ");
            }
        }

        #endregion

        #region Balanza

        public List<BalanzaKubrixDTO> getBalanza(DateTime fechaCorte, int empresa, int tipoCorte)
        {
            try
            {
                //fechaCorte = new DateTime(2022, 09, 30);
                List<CtaDescripcionEnkontrolDTO> descripcionCuenta = new List<CtaDescripcionEnkontrolDTO>();
                //Checar el último mes cerrado
                var lstParametros = new List<OdbcParameterDTO>();
                lstParametros.Add(new OdbcParameterDTO() { nombre = "year", tipo = OdbcType.Numeric, valor = fechaCorte.Year });
                lstParametros.Add(new OdbcParameterDTO() { nombre = "mes", tipo = OdbcType.Numeric, valor = fechaCorte.Month });
                var obj = new OdbcConsultaDTO()
                {
                    consulta = string.Format(@"SELECT TOP 1 year as anio, mes as mes FROM sc_mesproc WHERE sfa = 'S' AND year <= ? AND mes <= ? ORDER BY year DESC, mes DESC"),
                    parametros = lstParametros
                };
                AnioMesDTO UltimoMesFacturado = _contextEnkontrol.Select<AnioMesDTO>(empresa == 2 ? EnkontrolEnum.ArrenProd : EnkontrolEnum.CplanProd, obj).FirstOrDefault();
                //Buscar saldos antes de ultimo mes cerrado
                lstParametros = new List<OdbcParameterDTO>();
                lstParametros.Add(new OdbcParameterDTO() { nombre = "year", tipo = OdbcType.Numeric, valor = fechaCorte.Year });
                if (tipoCorte == 1) lstParametros.Add(new OdbcParameterDTO() { nombre = "fecha", tipo = OdbcType.DateTime, valor = fechaCorte });
                lstParametros.Add(new OdbcParameterDTO() { nombre = "year", tipo = OdbcType.Numeric, valor = fechaCorte.Year });

                lstParametros.Add(new OdbcParameterDTO() { nombre = "year", tipo = OdbcType.Numeric, valor = fechaCorte.Year });
                if (tipoCorte == 1) lstParametros.Add(new OdbcParameterDTO() { nombre = "fecha", tipo = OdbcType.DateTime, valor = fechaCorte });
                lstParametros.Add(new OdbcParameterDTO() { nombre = "year", tipo = OdbcType.Numeric, valor = fechaCorte.Year });

                lstParametros.Add(new OdbcParameterDTO() { nombre = "year", tipo = OdbcType.Numeric, valor = fechaCorte.Year });
                if (tipoCorte == 1) lstParametros.Add(new OdbcParameterDTO() { nombre = "fecha", tipo = OdbcType.DateTime, valor = fechaCorte });
                lstParametros.Add(new OdbcParameterDTO() { nombre = "year", tipo = OdbcType.Numeric, valor = fechaCorte.Year });

                lstParametros.Add(new OdbcParameterDTO() { nombre = "year", tipo = OdbcType.Numeric, valor = fechaCorte.Year });
                if (tipoCorte == 1) lstParametros.Add(new OdbcParameterDTO() { nombre = "fecha", tipo = OdbcType.DateTime, valor = fechaCorte });
                lstParametros.Add(new OdbcParameterDTO() { nombre = "year", tipo = OdbcType.Numeric, valor = fechaCorte.Year });

                //var auxMesesCargo = (new string[] { "enecargos", "febcargos", "marcargos", "abrcargos", "maycargos", "juncargos", "julcargos", "agocargos", "sepcargos", "octcargos", "novcargos", "diccargos" }).ToList();
                //var auxMesesAbono = (new string[] { "eneabonos", "febabonos", "marabonos", "abrabonos", "mayabonos", "junabonos", "julabonos", "agoabonos", "sepabonos", "octabonos", "novabonos", "dicabonos" }).ToList();

                //auxMesesCargo = auxMesesCargo.GetRange(0, UltimoMesFacturado.mes);
                //auxMesesAbono = auxMesesAbono.GetRange(0, UltimoMesFacturado.mes);
                //var auxMesesCargoStr = "";
                //foreach (var item in auxMesesCargo) { auxMesesCargoStr += item + " + "; }
                //auxMesesCargoStr = auxMesesCargoStr.Substring(0, auxMesesCargoStr.Length - 2);
                //var auxMesesAbonoStr = "";
                //foreach (var item in auxMesesAbono) { auxMesesAbonoStr += item + " + "; }
                //auxMesesAbonoStr = auxMesesAbonoStr.Substring(0, auxMesesAbonoStr.Length - 2);

                obj = new OdbcConsultaDTO()
                {
                    consulta = string.Format(@"                        
                        SELECT x.*, (x.saldoInicial + x.cargos + x.abonos) AS saldoActual FROM
                        (
                        SELECT 
                            0 as cta,
                            0 as scta,
                            0 as sscta, 
                            ISNULL((SELECT salini FROM sc_salcont WHERE year = ? AND cta = 0 AND scta = 0 AND sscta = 0), 0) AS saldoInicial,
                            SUM(CASE WHEN (movimiento.tm = 1 OR movimiento.tm = 3) THEN movimiento.monto ELSE 0 END) AS cargos,
                            SUM(CASE WHEN (movimiento.tm = 2 OR movimiento.tm = 4) THEN movimiento.monto ELSE 0 END) AS abonos
                        FROM sc_movpol movimiento
                        WHERE 
                            (SELECT status from sc_polizas WHERE year = movimiento.year AND mes = movimiento.mes AND tp = movimiento.tp AND poliza = movimiento.poliza) = 'A'
                            {0}
                            AND year = ?
                        UNION
                        SELECT 
                            movimiento.cta,
                            0 as scta,
                            0 as sscta, 
                            ISNULL((SELECT salini FROM sc_salcont WHERE year = ? AND cta = movimiento.cta AND scta = 0 AND sscta = 0), 0) AS saldoInicial,
                            SUM(CASE WHEN (movimiento.tm = 1 OR movimiento.tm = 3) THEN movimiento.monto ELSE 0 END) AS cargos,
                            SUM(CASE WHEN (movimiento.tm = 2 OR movimiento.tm = 4) THEN movimiento.monto ELSE 0 END) AS abonos
                        FROM sc_movpol movimiento
                        WHERE 
                            (SELECT status from sc_polizas WHERE year = movimiento.year AND mes = movimiento.mes AND tp = movimiento.tp AND poliza = movimiento.poliza) = 'A' 
                            {0}
                            AND year = ?
                        GROUP BY movimiento.cta
                        UNION
                        SELECT 
                            movimiento.cta,
                            movimiento.scta as scta,
                            0 as sscta, 
                            ISNULL((SELECT salini FROM sc_salcont WHERE year = ? AND cta = movimiento.cta AND scta = movimiento.scta AND sscta = 0), 0) AS saldoInicial,
                            SUM(CASE WHEN (movimiento.tm = 1 OR movimiento.tm = 3) THEN movimiento.monto ELSE 0 END) AS cargos,
                            SUM(CASE WHEN (movimiento.tm = 2 OR movimiento.tm = 4) THEN movimiento.monto ELSE 0 END) AS abonos
                        FROM sc_movpol movimiento
                        WHERE 
                            (SELECT status from sc_polizas WHERE year = movimiento.year AND mes = movimiento.mes AND tp = movimiento.tp AND poliza = movimiento.poliza) = 'A' 
                            {0}
                            AND year = ?
                        GROUP BY movimiento.cta, movimiento.scta
                        UNION
                        SELECT 
                            movimiento.cta,
                            movimiento.scta as scta,
                            movimiento.sscta as sscta, 
                            ISNULL((SELECT salini FROM sc_salcont WHERE year = ? AND cta = movimiento.cta AND scta = movimiento.scta AND sscta = movimiento.sscta), 0) AS saldoInicial,
                            SUM(CASE WHEN (movimiento.tm = 1 OR movimiento.tm = 3) THEN movimiento.monto ELSE 0 END) AS cargos,
                            SUM(CASE WHEN (movimiento.tm = 2 OR movimiento.tm = 4) THEN movimiento.monto ELSE 0 END) AS abonos
                        FROM sc_movpol movimiento
                        WHERE 
                            (SELECT status from sc_polizas WHERE year = movimiento.year AND mes = movimiento.mes AND tp = movimiento.tp AND poliza = movimiento.poliza) = 'A' 
                            {0}
                            AND year = ?
                        GROUP BY movimiento.cta, movimiento.scta, movimiento.sscta
                        ) x
                        ORDER BY x.cta, x.scta, x.sscta", tipoCorte == 1 ? "AND (SELECT fechapol from sc_polizas WHERE year = movimiento.year AND mes = movimiento.mes AND tp = movimiento.tp AND poliza = movimiento.poliza) <= ?" : ""),
                    parametros = lstParametros
                };

                List<BalanzaKubrixDTO> lstInicial = _contextEnkontrol.Select<BalanzaKubrixDTO>(empresa == 2 ? EnkontrolEnum.ArrenProd : EnkontrolEnum.CplanProd, obj);
                //Agregar movimientos polizas a partir de siguiente mes

                //DateTime fechaInicio = new DateTime(UltimoMesFacturado.anio, UltimoMesFacturado.mes + 1, 1).AddDays(-1);

                //lstParametros = new List<OdbcParameterDTO>();
                ////lstParametros.Add(new OdbcParameterDTO() { nombre = "fechapol", tipo = OdbcType.DateTime, valor = fechaInicio });
                //lstParametros.Add(new OdbcParameterDTO() { nombre = "fechapol", tipo = OdbcType.DateTime, valor = fechaInicio });
                ////lstParametros.Add(new OdbcParameterDTO() { nombre = "fechapol", tipo = OdbcType.DateTime, valor = fechaInicio });
                ////lstParametros.Add(new OdbcParameterDTO() { nombre = "fechapol", tipo = OdbcType.DateTime, valor = fechaInicio });
                //lstParametros.Add(new OdbcParameterDTO() { nombre = "fechapol", tipo = OdbcType.DateTime, valor = fechaCorte });
                //obj = new OdbcConsultaDTO()
                //{
                //    consulta = queryLstBalanza(),
                //    parametros = lstParametros
                //};
                //List<BalanzaKubrixDTO> lstEkP = _contextEnkontrolRentabilidad.SelectBalanza(empresa == 2 ? EnkontrolEnum.ArrenProd : EnkontrolEnum.CplanProd, obj);

                //var auxBalanzaTotal = new BalanzaKubrixDTO
                //{
                //    cta = 0,
                //    scta = 0,
                //    sscta = 0,
                //    abonos = lstEkP.Sum(x => x.abonos),
                //    cargos = lstEkP.Sum(x => x.cargos),
                //    saldoActual = lstEkP.Sum(x => x.saldoActual),
                //    saldoInicial = lstEkP.Sum(x => x.saldoInicial)
                //};

                //var auxBalanzaSctas = lstEkP.GroupBy(x => x.cta).Select(x => new BalanzaKubrixDTO
                //{
                //    cta = x.Key,
                //    scta = 0,
                //    sscta = 0,
                //    abonos = x.Sum(y => y.abonos),
                //    cargos = x.Sum(y => y.cargos),
                //    saldoActual = x.Sum(y => y.saldoActual),
                //    saldoInicial = x.Sum(y => y.saldoInicial)
                //}).ToList();

                //var auxBalanzaSSCtas = lstEkP.GroupBy(x => new { x.cta, x.scta }).Select(x => new BalanzaKubrixDTO
                //{
                //    cta = x.Key.cta,
                //    scta = x.Key.scta,
                //    sscta = 0,
                //    abonos = x.Sum(y => y.abonos),
                //    cargos = x.Sum(y => y.cargos),
                //    saldoActual = x.Sum(y => y.saldoActual),
                //    saldoInicial = x.Sum(y => y.saldoInicial)
                //}).ToList();

                //lstEkP = lstEkP.Where(x => x.scta != 0 && x.sscta != 0).ToList();
                //lstEkP.Add(auxBalanzaTotal);
                //lstEkP.AddRange(auxBalanzaSctas);
                //lstEkP.AddRange(auxBalanzaSSCtas);
                //lstEkP.AddRange(lstInicial);

                //lstEkP = lstEkP.GroupBy(x => new { x.cta, x.scta, x.sscta }).Select(x => new BalanzaKubrixDTO
                //{
                //    cta = x.Key.cta,
                //    scta = x.Key.scta,
                //    sscta = x.Key.sscta,
                //    abonos = x.Sum(y => y.abonos),
                //    cargos = x.Sum(y => y.cargos),
                //    saldoActual = x.Sum(y => y.saldoActual),
                //    saldoInicial = x.Sum(y => y.saldoInicial)
                //}).Distinct().Where(x => !(x.abonos == 0 && x.cargos == 0 && x.saldoInicial == 0 && x.saldoActual == 0)).ToList();

                List<string> ssCtas = lstInicial.Select(x => x.cta.ToString() + "-" + x.scta.ToString() + "-" + x.sscta.ToString()).ToList();
                List<string> sCtas = lstInicial.Select(x => x.cta.ToString() + "-" + x.scta.ToString() + "-0").Distinct().ToList();
                List<string> ctas = lstInicial.Select(x => x.cta.ToString() + "-0-0").Distinct().ToList();
                ctas.AddRange(sCtas);
                ctas.AddRange(ssCtas);
                lstParametros = new List<OdbcParameterDTO>();
                lstParametros.AddRange(ctas.Select(s => new OdbcParameterDTO() { nombre = "tm", tipo = OdbcType.VarChar, valor = s }));
                obj = new OdbcConsultaDTO()
                {
                    consulta = queryCuentas(ctas),
                    parametros = lstParametros
                };
                var lstCuentas = _contextEnkontrol.Select<CtaDescripcionEnkontrolDTO>(empresa == 2 ? EnkontrolEnum.ArrenProd : EnkontrolEnum.CplanProd, obj);

                foreach (var item in lstInicial)
                {
                    var catCta = lstCuentas.FirstOrDefault(x => x.cta == item.cta && x.scta == item.scta && x.sscta == item.sscta);
                    if (catCta == null) { item.descripcion = ""; }
                    else { item.descripcion = catCta.descripcion; }
                }

                return lstInicial;
            }
            catch (Exception e)
            {
                return new List<BalanzaKubrixDTO>();
            }
        }

        string queryLstBalanza()
        {
            return string.Format(
                @"SELECT 
                    polizas.cta AS cta, polizas.scta AS scta, polizas.sscta AS sscta, polizas.saldoInicial AS saldoInicial, 
                    polizas.cargos AS cargos, polizas.abonos AS abonos, polizas.monto AS saldoActual 
                FROM
                    (SELECT 
                        c.cta AS cta, c.scta AS scta, c.sscta AS sscta, 
                        0 AS saldoInicial,
                        sum(CASE WHEN (c.tm = 1 OR c.tm = 3) THEN c.monto ELSE 0 END) AS cargos, 
                        sum(CASE WHEN (c.tm = 2 OR c.tm = 4) THEN c.monto ELSE 0 END) AS abonos, 
                        sum(c.monto) AS monto
                    FROM 
                        (SELECT 
                            a.cta AS cta, a.scta AS scta, a.sscta AS sscta, a.monto AS monto, a.tm AS tm, b.fechapol AS fechapol
                        FROM 
                            (SELECT 
                                cta, scta, sscta, monto, tm, year, mes, poliza, tp 
                            FROM 
                                sc_movpol
                        ) a
                        LEFT JOIN
                            (SELECT 
                                year, mes, poliza, tp, fechapol 
                            FROM 
                                sc_polizas
                            ) b
                        ON 
                            a.year = b.year AND a.mes = b.mes AND a.poliza = b.poliza AND a.tp = b.tp
                        WHERE  fechapol > ? AND fechapol <= ?
                        ) c 
                    GROUP BY cta, scta, sscta
                    ) polizas
                WHERE
                    saldoInicial != 0 OR cargos != 0 OR abonos != 0 OR saldoActual != 0
                ORDER BY 
                    cta, scta, sscta"
                );
        }

        string queryCuentas(List<string> cuentas)
        {
            return string.Format(@"
                select cta, scta, sscta, descripcion from catcta
                where CAST(cta as varchar(5)) + '-' + CAST(scta as varchar(5)) + '-' + CAST(sscta as varchar(5)) in {0}"
                , cuentas.ToParamInValue()
            );
        }

        #endregion Balanza


        #region Catalógo de Divisiones

        public Dictionary<string, object> SaveOrUpdateDivision(tblM_KBDivision nuevaDivision)
        {
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    int usuarioCreadorID = vSesiones.sesionUsuarioDTO.id;
                    if (nuevaDivision.id == 0)
                    {
                        if (_context.tblM_KBDivision.Any(x => x.division == nuevaDivision.division))
                        {
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "Ya se encuentra una división con la misma descripción");
                            return resultado;
                        }
                        nuevaDivision.usuarioCreadorID = usuarioCreadorID;
                        nuevaDivision.fechaCreacion = DateTime.Now;
                        nuevaDivision.estatus = true;

                        _context.tblM_KBDivision.Add(nuevaDivision);

                        _context.SaveChanges();
                    }
                    else
                    {
                        var divisionTemporal = _context.tblM_KBDivision.FirstOrDefault(d => d.id == nuevaDivision.id);


                        divisionTemporal.division = nuevaDivision.division;
                        divisionTemporal.estatus = nuevaDivision.estatus;
                        divisionTemporal.divisionDetalle.ForEach(x => x.estatus = false);


                        foreach (var item in nuevaDivision.divisionDetalle)
                        {
                            var exist = divisionTemporal.divisionDetalle.FirstOrDefault(x => x.acID == item.acID);
                            if (exist == null)
                            {
                                tblM_KBDivisionDetalle nuevoDetalle = new tblM_KBDivisionDetalle();
                                nuevoDetalle.id = 0;
                                nuevoDetalle.acID = item.acID;
                                nuevoDetalle.divisionID = divisionTemporal.id;
                                nuevoDetalle.estatus = true;
                                //divisionTemporal.divisionDetalle.Add(nuevoDetalle);

                                _context.tblM_KBDivisionDetalle.Add(nuevoDetalle);

                            }
                            else
                            {
                                divisionTemporal.divisionDetalle.FirstOrDefault(x => x.id == exist.id).estatus = true;

                            }
                        }
                        _context.SaveChanges();
                    }

                    resultado.Add(SUCCESS, true);
                    dbTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    LogError(0, 0, nombreControlador, "GuardarDivision", e, AccionEnum.ACTUALIZAR, 0, nuevaDivision);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Ocurrió un error al guardar la divion favor de consultar con el departamento de sistemas.");
                }
                return resultado;
            }


        }

        public Dictionary<string, object> GetInfoDivision(int areaCuenta, bool estatus)
        {

            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;

                var empresa = vSesiones.sesionEmpresaActual;

                if (empresa == 1)
                {
                    var listaDivisiones = _context.tblM_KBDivision
                        .Where(x => x.estatus == estatus)
                        .Select(x => new
                        {
                            id = x.id,
                            division = x.division,
                            areaCuenta = x.divisionDetalle.Where(e => e.estatus).Select(r => new { id = r.id, areaCuenta = r.acID, descripcion = r.ac.cc + "-" + r.ac.descripcion }).ToList(),
                        })
                        .ToList();

                    resultado.Add("listaDivisiones", listaDivisiones);
                    resultado.Add(SUCCESS, true);

                }
                else
                {
                    var listaDivisiones = _context.tblM_KBDivision
                        .Where(x => x.estatus == estatus)
                        .Select(x => new
                        {
                            id = x.id,
                            division = x.division,
                            areaCuenta = x.divisionDetalle.Where(e => e.estatus).Select(r => new { id = r.id, areaCuenta = r.acID, descripcion = r.ac.areaCuenta + "-" + r.ac.descripcion }).ToList(),
                        })
                        .ToList();

                    resultado.Add("listaDivisiones", listaDivisiones);
                    resultado.Add(SUCCESS, true);
                }
                return resultado;
            }
            catch (Exception e)
            {
                LogError(0, 0, nombreControlador, "ObtenerDiviion", e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al buscar las divisiones.");
                return resultado;
            }
        }

        public Dictionary<string, object> getDivisionByID(int divisionID)
        {
            try
            {

                var division = _context.tblM_KBDivision.First(d => d.id == divisionID);

                resultado.Add("estatus", division.estatus.ToString());
                resultado.Add("division", division.division);
                resultado.Add("areaCuenta", division.divisionDetalle.Where(r => r.estatus).Select(a => a.acID).ToList());

                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {

                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public tblM_KBDivision getDivisionByNombre(string nombreDivision)
        {
            try
            {
                var division = _context.tblM_KBDivision.First(d => d.division == nombreDivision);
                return division;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<ComboDTO> getComboAreaCuenta()
        {
            var empresa = vSesiones.sesionEmpresaActual;

            if (empresa == 1)
            {
                return _context.tblP_CC.Select(x => new ComboDTO
                {
                    Value = x.id.ToString(),
                    Text = x.cc + " - " + x.descripcion
                }).ToList();
            }
            else
            {
                return _context.tblP_CC.Select(x => new ComboDTO
                {
                    Value = x.id.ToString(),
                    Text = x.areaCuenta + " - " + x.descripcion
                }).ToList();
            }

        }

        public Dictionary<string, object> bajaDivision(int divisionID)
        {

            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                var divisionTemporal = _context.tblM_KBDivision.FirstOrDefault(d => d.id == divisionID);
                try
                {
                    divisionTemporal.estatus = false;
                    _context.SaveChanges();

                    resultado.Add(SUCCESS, true);
                    dbTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    LogError(0, 0, nombreControlador, "GuardarDivision", e, AccionEnum.ACTUALIZAR, 0, divisionTemporal);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Ocurrió un error al guardar la divion favor de consultar con el departamento de sistemas.");
                }
                return resultado;
            }
        }

        public List<tblM_KBDivisionDetalle> getDivisionesDetalle()
        {
            var data = _context.tblM_KBDivisionDetalle.Where(x => x.division.estatus && x.estatus).ToList();
            return data;
        }

        #endregion

        #region Catalógo de Fletes

        public Dictionary<string, object> SaveOrUpdateFlete(tblM_KBFletes nuevo)
        {
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    int usuarioCreadorID = vSesiones.sesionUsuarioDTO.id;

                    if (nuevo.id == 0)
                    {
                        if (_context.tblM_KBFletes.Any(x => x.economicoID == nuevo.economicoID))
                        {
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "Ya se encuentra un equipo en fletes.");
                            return resultado;
                        }

                        nuevo.usuarioCreadorID = usuarioCreadorID;
                        nuevo.fechaCreacion = DateTime.Now;
                        nuevo.estatus = true;

                        _context.tblM_KBFletes.Add(nuevo);
                        _context.SaveChanges();


                    }
                    else
                    {
                        var actualizacion = _context.tblM_KBFletes.FirstOrDefault(x => x.id == nuevo.id);


                        actualizacion.estatus = nuevo.estatus;
                        actualizacion.areaCuenta = nuevo.areaCuenta;
                        _context.SaveChanges();
                    }

                    _context.SaveChanges();
                    resultado.Add(SUCCESS, true);
                    dbTransaction.Commit();

                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    LogError(0, 0, nombreControlador, "GuardarBanco", e, AccionEnum.ACTUALIZAR, 0, nuevo);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Ocurrió un error al guardar los operadores de la barrenadora.");
                }
                return resultado;
            }


        }

        public Dictionary<string, object> GetInfoFletes()
        {

            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;

                var listaFletes = _context.tblM_KBFletes
                    .Where(x => x.estatus)
                    .ToList();

                resultado.Add("listaFletes", listaFletes);
                resultado.Add(SUCCESS, true);

                return resultado;
            }
            catch (Exception e)
            {
                LogError(0, 0, nombreControlador, "GetFletes", e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al buscar los fletes..");
                return resultado;
            }
        }

        public Dictionary<string, object> CboEconomico()
        {

            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;

                var data = _context.tblM_CatMaquina
                    .Where(x => x.estatus == 1)
                    .Select(x => new
                    {
                        id = x.id,
                        text = x.noEconomico,
                        areaCuenta = x.centro_costos
                    }).ToList();

                resultado.Add("items", data);
                resultado.Add(SUCCESS, true);

                return resultado;
            }
            catch (Exception e)
            {
                LogError(0, 0, nombreControlador, "GetFletes", e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al buscar los fletes..");
                return resultado;
            }
        }
        #endregion

        #region Catalógo de Responsable Centro Costos

        public Dictionary<string, object> SaveOrUpdateAdministacionUsuarios(tblM_KBUsuarioResponsable nuevoUsuario)
        {
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    int usuarioCreadorID = vSesiones.sesionUsuarioDTO.id;
                    if (nuevoUsuario.id == 0)
                    {
                        if (_context.tblM_KBUsuarioResponsable.Any(x => x.usuarioResponsableID == nuevoUsuario.usuarioResponsableID))
                        {
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "Ya se encuentra dado de alta no es necesario agregar el usuario nuevamente");
                            return resultado;
                        }
                        nuevoUsuario.usuarioCreadorID = usuarioCreadorID;
                        nuevoUsuario.fechaCreacion = DateTime.Now;
                        nuevoUsuario.estatus = true;

                        _context.tblM_KBUsuarioResponsable.Add(nuevoUsuario);

                        _context.SaveChanges();
                    }
                    else
                    {
                        var temporal = _context.tblM_KBUsuarioResponsable.FirstOrDefault(d => d.id == nuevoUsuario.id);

                        temporal.estatus = nuevoUsuario.estatus;
                        temporal.areaCuentaResponsable.ForEach(x => x.estatus = false);

                        foreach (var item in nuevoUsuario.areaCuentaResponsable)
                        {
                            var exist = temporal.areaCuentaResponsable.FirstOrDefault(x => x.areaCuentaID == item.areaCuentaID);
                            if (exist == null)
                            {
                                tblM_KBAreaCuentaResponsable nuevoDetalle = new tblM_KBAreaCuentaResponsable();
                                nuevoDetalle.id = 0;
                                nuevoDetalle.areaCuentaID = item.areaCuentaID;
                                nuevoDetalle.estatus = true;
                                nuevoDetalle.usuarioResponsableID = nuevoUsuario.id;
                                _context.tblM_KBAreaCuentaResponsable.Add(nuevoDetalle);
                                _context.SaveChanges();
                            }
                            else
                            {
                                temporal.areaCuentaResponsable.FirstOrDefault(x => x.id == exist.id).estatus = true;
                                _context.SaveChanges();

                            }
                        }

                    }

                    resultado.Add(SUCCESS, true);
                    dbTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    LogError(0, 0, nombreControlador, "GuardarUsuarioAdministrador", e, AccionEnum.ACTUALIZAR, 0, nuevoUsuario);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Ocurrió un error al guardar el usuario favor de consultar con el departamento de sistemas.");
                }
                return resultado;
            }
        }

        public Dictionary<string, object> bajaUsuario(int id)
        {
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                var temporal = _context.tblM_KBUsuarioResponsable.FirstOrDefault(d => d.id == id);
                try
                {
                    temporal.estatus = false;
                    _context.SaveChanges();
                    resultado.Add(SUCCESS, true);
                    dbTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    LogError(0, 0, nombreControlador, "GuardarUsuarioAdministrador", e, AccionEnum.ACTUALIZAR, 0, temporal);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Ocurrió un error al guardar el usuario favor de consultar con el departamento de sistemas.");
                }
                return resultado;
            }
        }

        public Dictionary<string, object> GetInfoAdministracionUsuarios(int estatus)
        {

            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;

                var listaResposanbles = _context.tblM_KBUsuarioResponsable
                    .Where(x => estatus == 0 ? true : (estatus == 1 ? x.estatus : !x.estatus))
                    .Select(x => new
                    {
                        id = x.id,
                        usuarioNombre = x.usuarioResponsable.nombre + " " + x.usuarioResponsable.apellidoPaterno + " " + x.usuarioResponsable.apellidoMaterno,
                        usuarioResponsable = x.usuarioResponsableID,
                        areaCuenta = x.areaCuentaResponsable.Where(e => e.estatus).Select(r => new { id = r.id, areaCuenta = r.areaCuenta.areaCuenta, descripcion = r.areaCuenta.areaCuenta + "-" + r.areaCuenta.descripcion }).ToList(),
                        estatus = x.estatus
                    })
                    .ToList();

                resultado.Add("listaResposanbles", listaResposanbles);
                resultado.Add(SUCCESS, true);

                return resultado;
            }
            catch (Exception e)
            {
                LogError(0, 0, nombreControlador, "ObtenerDiviion", e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al buscar los usuarios.");
                return resultado;
            }
        }

        public Dictionary<string, object> getReponsableByID(int id)
        {
            try
            {

                var usuarioResposanble = _context.tblM_KBUsuarioResponsable.First(d => d.id == id);
                string estatus = usuarioResposanble.estatus.ToString();
                /*var usuario = _context.tblP_Usuario.Where(x => x.id == usuarioResposanble.usuarioResponsableID).Select(x => new
                {
                    id = x.id,
                    nombre = x.nombre,
                    apellidoPaterno = x.apellidoPaterno,
                    apellidoMaterno = x.apellidoPaterno
                }).First();*/


                resultado.Add("estatus", estatus);
                resultado.Add("reponsableUsuario", usuarioResposanble.usuarioResponsableID);
                resultado.Add("areaCuenta", usuarioResposanble.areaCuentaResponsable.Where(r => r.estatus).Select(a => a.areaCuentaID).ToList());

                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {

                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> getListaUsuarios()
        {
            try
            {

                var usuarioResposanble = _context.tblP_Usuario.Where(u => u.estatus && !u.cliente).
                                         Select(r => new
                                         {
                                             id = r.id,
                                             text = r.nombre + " " + r.apellidoPaterno + " " + r.apellidoMaterno
                                         }).ToList();

                resultado.Add("items", usuarioResposanble);

                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {

                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        #endregion

        #region Reportes autogenerados
        public MemoryStream crearExcelSaldosCplan(DateTime fechaCorte)
        {
            try
            {

                int anio = fechaCorte.Year;
                OdbcConsultaDTO odbc = new OdbcConsultaDTO()
                {
                    consulta = string.Format(@"SELECT year, cta, scta, sscta, salini, enecargos, eneabonos, febcargos, febabonos, marcargos, marabonos,
                        abrcargos, abrabonos, maycargos, mayabonos, juncargos, junabonos, julcargos, julabonos, agocargos, agoabonos,
                        sepcargos, sepabonos, octcargos, octabonos, novcargos, novabonos, diccargos, dicabonos, cc,
                        (select descripcion from catcta where cta = a.cta and scta = a.scta and sscta = a.sscta) as descripcion,
                        (select descripcion from cc where cc = a.cc) as descripcion2
                    FROM sc_salcont_cc a where (year = {0}) and cta > 0 
                        and (select count(cc) from cc where cc = a.cc) > 0
                        and (salini + enecargos + eneabonos  + febcargos + febabonos  + marcargos + marabonos  + abrcargos + abrabonos + maycargos 
                        + mayabonos + juncargos + junabonos + julcargos + julabonos + agocargos + agoabonos + sepcargos + sepabonos + octcargos + octabonos 
                        + novcargos + novabonos + diccargos + dicabonos) != 0
                        order by cc, cta, scta, sscta", anio),
                    parametros = new List<OdbcParameterDTO>()
                };
                List<SaldosKubrixDTO> lst = _contextEnkontrolRentabilidad.Select<SaldosKubrixDTO>(EnkontrolEnum.CplanProd, odbc);

                using (ExcelPackage excel = new ExcelPackage())
                {
                    var hoja1 = excel.Workbook.Worksheets.Add("Saldos");

                    List<string[]> headerRow = new List<string[]>() { new string[] { 
                        "cta", "scta", "sscta", "year", "salini", "enecargos", "eneabonos", "febcargos", "febabonos", "marcargos", "marabonos", "abrcargos", "abrabonos",
                        "maycargos", "mayabonos", "juncargos", "junabonos", "julcargos", "julabonos", "agocargos", "agoabonos", "sepcargos", "sepabonos",
                        "octcargos", "octabonos", "novcargos", "novabonos", "diccargos", "dicabonos", "cc", "descripcion", "descripcion2"
                    } };
                    string headerRange = "A1:AE1";

                    hoja1.Cells[headerRange].LoadFromArrays(headerRow);

                    var cellData = new List<object[]>();

                    foreach (var movimiento in lst)
                    {
                        cellData.Add(new object[]{
                        movimiento.cta,
                        movimiento.scta,
                        movimiento.sscta,
                        anio,
                        movimiento.salini,
                        movimiento.enecargos,
                        movimiento.eneabonos,
                        movimiento.febcargos,
                        movimiento.febabonos,
                        movimiento.marcargos,
                        movimiento.marabonos,
                        movimiento.abrcargos,
                        movimiento.abrabonos,
                        movimiento.maycargos,
                        movimiento.mayabonos,
                        movimiento.juncargos,
                        movimiento.junabonos,
                        movimiento.julcargos,
                        movimiento.julabonos,
                        movimiento.agocargos,
                        movimiento.agoabonos,
                        movimiento.sepcargos,
                        movimiento.sepabonos,
                        movimiento.octcargos,
                        movimiento.octabonos,
                        movimiento.novcargos,
                        movimiento.novabonos,
                        movimiento.diccargos,
                        movimiento.dicabonos,
                        movimiento.cc,
                        movimiento.descripcion,
                        movimiento.descripcion2
                        });
                    }

                    hoja1.Cells[2, 1].LoadFromArrays(cellData);

                    excel.Compression = CompressionLevel.BestSpeed;

                    var bytes = new MemoryStream();

                    using (var exportData = new MemoryStream())
                    {
                        excel.SaveAs(exportData);
                        bytes = exportData;
                    }

                    return bytes;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public MemoryStream crearExcelSaldosColombia(DateTime fechaCorte)
        {
            try
            {
                int anio = fechaCorte.Year;
                OdbcConsultaDTO odbc = new OdbcConsultaDTO()
                {
                    consulta = string.Format(@"SELECT year, cta, scta, sscta, salini, enecargos, eneabonos, febcargos, febabonos, marcargos, marabonos,
                        abrcargos, abrabonos, maycargos, mayabonos, juncargos, junabonos, julcargos, julabonos, agocargos, agoabonos,
                        sepcargos, sepabonos, octcargos, octabonos, novcargos, novabonos, diccargos, dicabonos, cc,
                        (select descripcion from DBA.catcta where cta = a.cta and scta = a.scta and sscta = a.sscta) as descripcion,
                        (select descripcion from DBA.cc where cc = a.cc) as descripcion2
                    FROM DBA.sc_salcont_cc a where (year = {0}) and cta > 0 
                        and (select count(cc) from DBA.cc where cc = a.cc) > 0
                        and (salini + enecargos + eneabonos  + febcargos + febabonos  + marcargos + marabonos  + abrcargos + abrabonos + maycargos 
                        + mayabonos + juncargos + junabonos + julcargos + julabonos + agocargos + agoabonos + sepcargos + sepabonos + octcargos + octabonos 
                        + novcargos + novabonos + diccargos + dicabonos) != 0
                        order by cc, cta, scta, sscta", anio),
                    parametros = new List<OdbcParameterDTO>()
                };
                List<SaldosKubrixDTO> lst = _contextEnkontrolRentabilidad.Select<SaldosKubrixDTO>(EnkontrolEnum.ColombiaProductivo, odbc);

                using (ExcelPackage excel = new ExcelPackage())
                {
                    var hoja1 = excel.Workbook.Worksheets.Add("Saldos");

                    List<string[]> headerRow = new List<string[]>() { new string[] { 
                        "cta", "scta", "sscta", "year", "salini", "enecargos", "eneabonos", "febcargos", "febabonos", "marcargos", "marabonos", "abrcargos", "abrabonos",
                        "maycargos", "mayabonos", "juncargos", "junabonos", "julcargos", "julabonos", "agocargos", "agoabonos", "sepcargos", "sepabonos",
                        "octcargos", "octabonos", "novcargos", "novabonos", "diccargos", "dicabonos", "cc", "descripcion", "descripcion2"
                    } };
                    string headerRange = "A1:AE1";

                    hoja1.Cells[headerRange].LoadFromArrays(headerRow);

                    var cellData = new List<object[]>();

                    foreach (var movimiento in lst)
                    {
                        cellData.Add(new object[]{
                        movimiento.cta,
                        movimiento.scta,
                        movimiento.sscta,
                        anio,
                        movimiento.salini,
                        movimiento.enecargos,
                        movimiento.eneabonos,
                        movimiento.febcargos,
                        movimiento.febabonos,
                        movimiento.marcargos,
                        movimiento.marabonos,
                        movimiento.abrcargos,
                        movimiento.abrabonos,
                        movimiento.maycargos,
                        movimiento.mayabonos,
                        movimiento.juncargos,
                        movimiento.junabonos,
                        movimiento.julcargos,
                        movimiento.julabonos,
                        movimiento.agocargos,
                        movimiento.agoabonos,
                        movimiento.sepcargos,
                        movimiento.sepabonos,
                        movimiento.octcargos,
                        movimiento.octabonos,
                        movimiento.novcargos,
                        movimiento.novabonos,
                        movimiento.diccargos,
                        movimiento.dicabonos,
                        movimiento.cc,
                        movimiento.descripcion,
                        movimiento.descripcion2
                        });
                    }

                    hoja1.Cells[2, 1].LoadFromArrays(cellData);

                    excel.Compression = CompressionLevel.BestSpeed;

                    var bytes = new MemoryStream();

                    using (var exportData = new MemoryStream())
                    {
                        excel.SaveAs(exportData);
                        bytes = exportData;
                    }

                    return bytes;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public MemoryStream crearExcelSaldosCplanEici(DateTime fechaCorte)
        {
            try
            {

                int anio = fechaCorte.Year;
                OdbcConsultaDTO odbc = new OdbcConsultaDTO()
                {
                    consulta = string.Format(@"SELECT year, cta, scta, sscta, salini, enecargos, eneabonos, febcargos, febabonos, marcargos, marabonos,
                        abrcargos, abrabonos, maycargos, mayabonos, juncargos, junabonos, julcargos, julabonos, agocargos, agoabonos,
                        sepcargos, sepabonos, octcargos, octabonos, novcargos, novabonos, diccargos, dicabonos, cc,
                        (select descripcion from catcta where cta = a.cta and scta = a.scta and sscta = a.sscta) as descripcion,
                        (select descripcion from cc where cc = a.cc) as descripcion2
                    FROM sc_salcont_cc a where (year = {0}) and cta > 0                         
                        and (select count(cc) from cc where cc = a.cc) > 0
                        and (salini + enecargos + eneabonos  + febcargos + febabonos  + marcargos + marabonos  + abrcargos + abrabonos + maycargos 
                        + mayabonos + juncargos + junabonos + julcargos + julabonos + agocargos + agoabonos + sepcargos + sepabonos + octcargos + octabonos 
                        + novcargos + novabonos + diccargos + dicabonos) != 0
                        order by cc, cta, scta, sscta", anio),
                    parametros = new List<OdbcParameterDTO>()
                };
                List<SaldosKubrixDTO> lst = _contextEnkontrolRentabilidad.Select<SaldosKubrixDTO>(EnkontrolEnum.CplanEici, odbc);

                using (ExcelPackage excel = new ExcelPackage())
                {
                    var hoja1 = excel.Workbook.Worksheets.Add("Saldos");

                    List<string[]> headerRow = new List<string[]>() { new string[] { 
                        "cta", "scta", "sscta", "year", "salini", "enecargos", "eneabonos", "febcargos", "febabonos", "marcargos", "marabonos", "abrcargos", "abrabonos",
                        "maycargos", "mayabonos", "juncargos", "junabonos", "julcargos", "julabonos", "agocargos", "agoabonos", "sepcargos", "sepabonos",
                        "octcargos", "octabonos", "novcargos", "novabonos", "diccargos", "dicabonos", "cc", "descripcion", "descripcion2"
                    } };
                    string headerRange = "A1:AE1";

                    hoja1.Cells[headerRange].LoadFromArrays(headerRow);

                    var cellData = new List<object[]>();

                    foreach (var movimiento in lst)
                    {
                        cellData.Add(new object[]{
                        movimiento.cta,
                        movimiento.scta,
                        movimiento.sscta,
                        anio,
                        movimiento.salini,
                        movimiento.enecargos,
                        movimiento.eneabonos,
                        movimiento.febcargos,
                        movimiento.febabonos,
                        movimiento.marcargos,
                        movimiento.marabonos,
                        movimiento.abrcargos,
                        movimiento.abrabonos,
                        movimiento.maycargos,
                        movimiento.mayabonos,
                        movimiento.juncargos,
                        movimiento.junabonos,
                        movimiento.julcargos,
                        movimiento.julabonos,
                        movimiento.agocargos,
                        movimiento.agoabonos,
                        movimiento.sepcargos,
                        movimiento.sepabonos,
                        movimiento.octcargos,
                        movimiento.octabonos,
                        movimiento.novcargos,
                        movimiento.novabonos,
                        movimiento.diccargos,
                        movimiento.dicabonos,
                        movimiento.cc,
                        movimiento.descripcion,
                        movimiento.descripcion2
                        });
                    }

                    hoja1.Cells[2, 1].LoadFromArrays(cellData);

                    excel.Compression = CompressionLevel.BestSpeed;

                    var bytes = new MemoryStream();

                    using (var exportData = new MemoryStream())
                    {
                        excel.SaveAs(exportData);
                        bytes = exportData;
                    }

                    return bytes;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public MemoryStream crearExcelClientesCplan(DateTime fechaCorte, int tipo)
        {
            try
            {
                OdbcConsultaDTO odbc = new OdbcConsultaDTO()
                {
                    consulta = string.Format(@"SELECT 
                            final.numcte, final.nombre, final.factura, final.tm, final.fecha, final.vencimiento, final.id_segmento, final.desc_segmento, final.tipo_cliente, final.moneda, final.tipocambio, final.cc, final.desc_cc, 
                            final.corto, final.corte, final.plazo1, final.plazo2, final.plazo3, final.dif1, final.dif2, final.dif3, final.movs_x_factura,
                            CASE WHEN final.sum_venc_x_fact > 0 THEN final.sum_venc_x_fact ELSE 0 END as sum_venc_x_fact, CASE WHEN final.sum_por_vencer > 0 THEN final.sum_por_vencer ELSE 0 END as sum_por_vencer,
                            CASE WHEN final.sum_vencido_1 > 0 THEN final.sum_vencido_1 ELSE 0 END as sum_vencido_1, CASE WHEN final.sum_vencido_2 > 0 THEN final.sum_vencido_2 ELSE 0 END as sum_vencido_2, 
                            CASE WHEN final.sum_vencido_3 > 0 THEN final.sum_vencido_3 ELSE 0 END as sum_vencido_3, CASE WHEN final.sum_vencido_4 > 0 THEN final.sum_vencido_4 ELSE 0 END as sum_vencido_4,
                            CASE WHEN final.sum_vencido > 0 THEN final.sum_vencido ELSE 0 END as sum_vencido, final.saldo_x_factura, final.monto_factura, final.descrip_tm, final.concep_mov, final.Is_telefono
                        FROM
                        (
                        SELECT 
                            movimiento.numcte as numcte, MIN(cliente.nombre) as nombre, movimiento.factura as factura, MIN(movimiento.tm) as tm, MIN(movimiento.fecha) as fecha, MIN(movimiento.fechavenc) as vencimiento,
                            1 as id_segmento, 'SEGMENTO DEFAULT' as desc_segmento, MIN(cliente.tipo_cliente) as tipo_cliente, MIN(movimiento.moneda) as moneda, MIN(movimiento.tipocambio) as tipocambio, movimiento.cc as cc,
                            MIN(centro_costos.descripcion) as desc_cc, MIN(centro_costos.corto) as corto, '{0}' as corte, 15 as plazo1, 30 as plazo2, 60 as plazo3, CAST(DATEADD(day, -15, '{0}') as date) AS dif1,
                            CAST(DATEADD(day, -30, '{0}') as date) AS dif2, CAST(DATEADD(day, -60, '{0}') as date) AS dif3, (select COUNT(factura) from sx_movcltes where movimiento.factura = factura and numcte = movimiento.numcte) as movs_x_factura,
                            SUM(CASE WHEN (movimiento.fechavenc < '{0}') THEN  movimiento.total ELSE 0 END) + ISNULL( (select SUM(total) from sx_movcltes where tm in (select tm from sx_tm where naturaleza in (2,3)) AND factura = movimiento.factura AND numcte = movimiento.numcte), 0) AS sum_venc_x_fact, 
                            SUM(CASE WHEN (movimiento.fechavenc BETWEEN '{0}' AND CAST(DATEADD(day, 15, '{0}') as date)) THEN  movimiento.total ELSE 0 END) + ISNULL( (select SUM(total) from sx_movcltes where tm in (select tm from sx_tm where naturaleza in (2,3)) AND factura = movimiento.factura AND numcte = movimiento.numcte), 0) AS sum_por_vencer, 
                            SUM(CASE WHEN (movimiento.fechavenc BETWEEN CAST(DATEADD(day, -15, '{0}') as date) AND '{0}') THEN  movimiento.total ELSE 0 END) + ISNULL( (select SUM(total) from sx_movcltes where tm in (select tm from sx_tm where naturaleza in (2,3)) AND factura = movimiento.factura AND numcte = movimiento.numcte), 0) AS sum_vencido_1,
                            SUM(CASE WHEN (movimiento.fechavenc BETWEEN CAST(DATEADD(day, -30, '{0}') as date) AND CAST(DATEADD(day, -15, '{0}') as date)) THEN  movimiento.total ELSE 0 END) + ISNULL( (select SUM(total) from sx_movcltes where tm in (select tm from sx_tm where naturaleza in (2,3)) AND factura = movimiento.factura AND numcte = movimiento.numcte), 0) AS sum_vencido_2,
                            SUM(CASE WHEN (movimiento.fechavenc BETWEEN CAST(DATEADD(day, -60, '{0}') as date) AND CAST(DATEADD(day, -30, '{0}') as date)) THEN  movimiento.total ELSE 0 END) + ISNULL( (select SUM(total) from sx_movcltes where tm in (select tm from sx_tm where naturaleza in (2,3)) AND factura = movimiento.factura AND numcte = movimiento.numcte), 0) AS sum_vencido_3,
                            SUM(CASE WHEN (movimiento.fechavenc < CAST(DATEADD(day, -60, '{0}') as date)) THEN  movimiento.total ELSE 0 END) + ISNULL( (select SUM(total) from sx_movcltes where tm in (select tm from sx_tm where naturaleza in (2,3)) AND factura = movimiento.factura AND numcte = movimiento.numcte), 0) AS sum_vencido_4,
                            SUM(CASE WHEN (movimiento.fechavenc < '{0}') THEN  movimiento.total ELSE 0 END) + ISNULL( (select SUM(total) from sx_movcltes where tm in (select tm from sx_tm where naturaleza in (2,3)) AND factura = movimiento.factura AND numcte = movimiento.numcte), 0) AS sum_vencido,
                            (select SUM(total) from sx_movcltes where factura = movimiento.factura and numcte = movimiento.numcte) AS saldo_x_factura,
                            SUM(movimiento.total) as monto_factura,
                            (select MIN(descripcion) from sx_tm where tm = MIN(movimiento.tm)) as descrip_tm,
                            MIN(movimiento.concepto) as concep_mov,
                            ISNULL(MIN(cliente.telefono1), '') AS Is_telefono,
                            (select SUM(total) from sx_movcltes where numcte = movimiento.numcte group by numcte) AS suma_cliente
                        FROM (select * from sx_movcltes where tm in (select tm from sx_tm where naturaleza in (1,4))) movimiento
                        INNER JOIN sx_clientes cliente ON movimiento.numcte = cliente.numcte
                        INNER JOIN cc centro_costos ON centro_costos.cc = movimiento.cc

                        GROUP BY movimiento.numcte, movimiento.factura, movimiento.cc
                        ) final WHERE final.suma_cliente != 0 ORDER BY final.numcte", fechaCorte.ToString("yyyy-MM-dd")),
                    parametros = new List<OdbcParameterDTO>()
                };
                List<ClientesKubrixDTO> lst = _contextEnkontrolRentabilidad.Select<ClientesKubrixDTO>(tipo == 0 ? EnkontrolEnum.CplanProd : EnkontrolEnum.CplanEici, odbc);

                using (ExcelPackage excel = new ExcelPackage())
                {
                    var hoja1 = excel.Workbook.Worksheets.Add("Clientes");

                    List<string[]> headerRow = new List<string[]>() { new string[] { 
                        "numcte", "nombre", "factura", "tm", "fecha", "vencimiento", "id_segmento", "desc_segmento", "tipo_cliente", "desc_tipo_cliente", 
                        "moneda", "tipocambio", "desc_moneda", "cc", "desc_cc", "corto", "corte", "plazo1", "plazo2", "plazo3", "dif1", "dif2", "dif3", 
                        "movs_x_factura", "sum_venc_x_fact", "sum_por_vencer", "sum_vencido_1", "sum_vencido_2", "sum_vencido_3", "sum_vencido_4", 
                        "sum_vencido", "saldo_x_factura", "monto_factura", "fact_abono_no_apli", "monto_fact_abono_no_apli", "fecha_max", 
                        "ultimo_tipocambio", "descrip_tm", "concep_mov", "ls_telefono"

                    } };
                    string headerRange = "A1:AN1";

                    hoja1.Cells[headerRange].LoadFromArrays(headerRow);

                    var cellData = new List<object[]>();

                    foreach (var movimiento in lst)
                    {
                        cellData.Add(new object[]{
                            movimiento.numcte,
                            movimiento.nombre,
                            movimiento.factura,
                            movimiento.tm,
                            movimiento.fecha.ToString("dd/MM/yyyy"),
                            movimiento.vencimiento.ToString("dd/MM/yyyy"),
                            movimiento.id_segmento,
                            movimiento.desc_segmento,
                            movimiento.tipo_cliente,
                            movimiento.desc_tipo_cliente,
                            movimiento.moneda,
                            movimiento.tipocambio,
                            movimiento.desc_moneda,
                            movimiento.cc,
                            movimiento.desc_cc,
                            movimiento.corto,
                            fechaCorte.ToString("dd/MM/yyyy"),
                            movimiento.plazo1,
                            movimiento.plazo2,
                            movimiento.plazo3,
                            movimiento.dif1.ToString("dd/MM/yyyy"),
                            movimiento.dif2.ToString("dd/MM/yyyy"),
                            movimiento.dif3.ToString("dd/MM/yyyy"),
                            movimiento.movs_x_factura,
                            movimiento.sum_venc_x_fact,
                            movimiento.sum_por_vencer,
                            movimiento.sum_vencido_1,
                            movimiento.sum_vencido_2,
                            movimiento.sum_vencido_3,
                            movimiento.sum_vencido_4,
                            movimiento.sum_vencido,
                            movimiento.saldo_x_factura,
                            movimiento.monto_factura,
                            movimiento.fact_abono_no_apli,
                            movimiento.monto_fact_abono_no_apli,
                            movimiento.fecha_max.ToString("dd/MM/yyyy"),
                            movimiento.ultimo_tipocambio,
                            movimiento.descrip_tm,
                            movimiento.concep_mov,
                            movimiento.ls_telefono
                        });
                    }

                    hoja1.Cells[2, 1].LoadFromArrays(cellData);

                    excel.Compression = CompressionLevel.BestSpeed;

                    var bytes = new MemoryStream();

                    using (var exportData = new MemoryStream())
                    {
                        excel.SaveAs(exportData);
                        bytes = exportData;
                    }

                    return bytes;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public MemoryStream crearExcelVencimientosCplan(DateTime fechaCorte)
        {
            try
            {
                OdbcConsultaDTO odbc = new OdbcConsultaDTO()
                {
                    consulta = string.Format(@"SELECT 
                            final.numpro, final.nombre, 
                            CASE WHEN final.por_vencer > 0 THEN final.por_vencer ELSE 0 END AS por_vencer, 
                            CASE WHEN final.plazo_1 > 0 THEN final.plazo_1 ELSE 0 END AS plazo_1, 
                            CASE WHEN final.plazo_2 > 0 THEN final.plazo_2 ELSE 0 END AS plazo_2, 
                            CASE WHEN final.plazo_3 > 0 THEN final.plazo_3 ELSE 0 END AS plazo_3, 
                            CASE WHEN final.plazo_4 > 0 THEN final.plazo_4 ELSE 0 END AS plazo_4,
                            final.factura, final.moneda, final.fecha, final.fechavenc, final.tm, final.autorizapago, 
                            final.saldo_factura, final.plazo1, final.plazo2, final.plazo3, final.plazo4, final.cc, final.descripcion,
                            final.tipocambio, final.nomcorto, final.corto, ISNULL(final.telefono1, '') as telefono1, 
                            CASE WHEN moneda_id = 1 THEN final.saldo_factura ELSE 0 END AS saldo_factura_dlls, final.concepto, final.moneda_id
                        FROM
                        (SELECT 
                            movimiento.numpro as numpro, MIN(proveedor.nombre) as nombre, 
                            SUM(CASE WHEN (movimiento.fechavenc BETWEEN '{0}' AND CAST(DATEADD(day, 7, '{0}') as date)) THEN  movimiento.total ELSE 0 END) 
                                + ISNULL( (select SUM(total) from sp_movprov where tm in (select tm from sp_tm where naturaleza in (2,3)) AND factura = movimiento.factura AND numpro = movimiento.numpro), 0) AS por_vencer,    
                            SUM(CASE WHEN (movimiento.fechavenc BETWEEN CAST(DATEADD(day, -7, '{0}') as date) AND '{0}') THEN  movimiento.total ELSE 0 END) + ISNULL( (select SUM(total) from sp_movprov where tm in (select tm from sp_tm where naturaleza in (2,3)) AND factura = movimiento.factura AND numpro = movimiento.numpro), 0) AS plazo_1,
                            SUM(CASE WHEN (movimiento.fechavenc BETWEEN CAST(DATEADD(day, -14, '{0}') as date) AND CAST(DATEADD(day, -7, '{0}') as date)) THEN  movimiento.total ELSE 0 END) + ISNULL( (select SUM(total) from sp_movprov where tm in (select tm from sp_tm where naturaleza in (2,3)) AND factura = movimiento.factura AND numpro = movimiento.numpro), 0) AS plazo_2,
                            SUM(CASE WHEN (movimiento.fechavenc BETWEEN CAST(DATEADD(day, -30, '{0}') as date) AND CAST(DATEADD(day, -14, '{0}') as date)) THEN  movimiento.total ELSE 0 END) + ISNULL( (select SUM(total) from sp_movprov where tm in (select tm from sp_tm where naturaleza in (2,3)) AND factura = movimiento.factura AND numpro = movimiento.numpro), 0) AS plazo_3,
                            SUM(CASE WHEN (movimiento.fechavenc < CAST(DATEADD(day, -30, '{0}') as date)) THEN  movimiento.total ELSE 0 END) + ISNULL( (select SUM(total) from sp_movprov where tm in (select tm from sp_tm where naturaleza in (2,3)) AND factura = movimiento.factura AND numpro = movimiento.numpro), 0) AS plazo_4,
                            movimiento.factura, (select moneda from moneda where clave = MIN(movimiento.moneda)) as moneda, MIN(movimiento.fecha) as fecha, MIN(movimiento.fechavenc) as fechavenc, MIN(movimiento.tm) as tm, MIN(movimiento.autorizapago) as autorizapago,
                            (select SUM(total) from sp_movprov where factura = movimiento.factura and numpro = movimiento.numpro) AS saldo_factura, 7 as plazo1, 14 as plazo2, 30 as plazo3, 30 as plazo4, movimiento.cc as cc, MIN(centro_costos.descripcion) as descripcion,
                            MIN(movimiento.tipocambio) as tipocambio, MIN(proveedor.nomcorto) as nomcorto, MIN(centro_costos.corto) as corto, MIN(proveedor.telefono1) as telefono1,
    
                            MIN(movimiento.concepto) as concepto, MIN(movimiento.moneda) as moneda_id
                        FROM 
                            (select * from sp_movprov where tm in (select tm from sp_tm where naturaleza in (1,4))) movimiento
                            INNER JOIN sp_proveedores proveedor ON movimiento.numpro = proveedor.numpro
                            INNER JOIN cc centro_costos ON centro_costos.cc = movimiento.cc
                        GROUP BY movimiento.numpro, movimiento.factura, movimiento.cc) final
                        WHERE final.saldo_factura != 0 ORDER BY final.numpro", fechaCorte.ToString("yyy-MM-dd")),
                    parametros = new List<OdbcParameterDTO>()
                };
                List<VencimientoKubrixDTO> lst = _contextEnkontrolRentabilidad.Select<VencimientoKubrixDTO>(EnkontrolEnum.CplanProd, odbc);

                using (ExcelPackage excel = new ExcelPackage())
                {
                    var hoja1 = excel.Workbook.Worksheets.Add("Vencimientos");

                    List<string[]> headerRow = new List<string[]>() { new string[] { 
                        "numpro", "nombre", "por_vencer", "plazo1", "plazo2", "plazo3", "plazo4", "factura", "moneda", "fecha", "fechavenc", "tm", 
                        "autorizapago", "saldo_factura", "plazo1", "plazo2", "plazo3", "plazo4", "cc", "descripcion", "tipocambio", "nomcorto", "corto", 
                        "telefono1", "saldo_factura_dlls", "concepto", "moneda"

                    } };
                    string headerRange = "A1:AA1";

                    hoja1.Cells[headerRange].LoadFromArrays(headerRow);

                    var cellData = new List<object[]>();

                    foreach (var movimiento in lst)
                    {
                        cellData.Add(new object[]{
                            movimiento.numpro,
                            movimiento.nombre,
                            movimiento.por_vencer,
                            movimiento.plazo_1,
                            movimiento.plazo_2,
                            movimiento.plazo_3,
                            movimiento.plazo_4,
                            movimiento.factura,
                            movimiento.moneda,
                            movimiento.fecha.ToString("dd/MM/yyyy"),
                            movimiento.fechavenc.ToString("dd/MM/yyyy"),
                            movimiento.tm,
                            movimiento.autorizapago,
                            movimiento.saldo_factura,
                            movimiento.plazo1,
                            movimiento.plazo2,
                            movimiento.plazo3,
                            movimiento.plazo4,
                            movimiento.cc,
                            movimiento.descripcion,
                            movimiento.tipocambio,
                            movimiento.nomcorto,
                            movimiento.corto,
                            movimiento.telefono1,
                            movimiento.saldo_factura_dlls,
                            movimiento.concepto,
                            movimiento.moneda
                        });
                    }

                    hoja1.Cells[2, 1].LoadFromArrays(cellData);

                    excel.Compression = CompressionLevel.BestSpeed;

                    var bytes = new MemoryStream();

                    using (var exportData = new MemoryStream())
                    {
                        excel.SaveAs(exportData);
                        bytes = exportData;
                    }

                    return bytes;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public MemoryStream crearExcelSaldosCplanIntegradora(DateTime fechaCorte)
        {
            try
            {

                int anio = fechaCorte.Year;
                OdbcConsultaDTO odbc = new OdbcConsultaDTO()
                {
                    consulta = string.Format(@"SELECT year, cta, scta, sscta, salini, enecargos, eneabonos, febcargos, febabonos, marcargos, marabonos,
                        abrcargos, abrabonos, maycargos, mayabonos, juncargos, junabonos, julcargos, julabonos, agocargos, agoabonos,
                        sepcargos, sepabonos, octcargos, octabonos, novcargos, novabonos, diccargos, dicabonos, cc,
                        (select descripcion from catcta where cta = a.cta and scta = a.scta and sscta = a.sscta) as descripcion,
                        (select descripcion from cc where cc = a.cc) as descripcion2
                    FROM sc_salcont_cc a where (year = {0}) and cta > 0 
                        and (select count(cc) from cc where cc = a.cc) > 0
                        and (salini + enecargos + eneabonos  + febcargos + febabonos  + marcargos + marabonos  + abrcargos + abrabonos + maycargos 
                        + mayabonos + juncargos + junabonos + julcargos + julabonos + agocargos + agoabonos + sepcargos + sepabonos + octcargos + octabonos 
                        + novcargos + novabonos + diccargos + dicabonos) != 0
                        order by cc, cta, scta, sscta", anio),
                    parametros = new List<OdbcParameterDTO>()
                };
                List<SaldosKubrixDTO> lst = _contextEnkontrolRentabilidad.Select<SaldosKubrixDTO>(EnkontrolEnum.CplanIntegradora, odbc);

                using (ExcelPackage excel = new ExcelPackage())
                {
                    var hoja1 = excel.Workbook.Worksheets.Add("Saldos");

                    List<string[]> headerRow = new List<string[]>() { new string[] { 
                        "cta", "scta", "sscta", "year", "salini", "enecargos", "eneabonos", "febcargos", "febabonos", "marcargos", "marabonos", "abrcargos", "abrabonos",
                        "maycargos", "mayabonos", "juncargos", "junabonos", "julcargos", "julabonos", "agocargos", "agoabonos", "sepcargos", "sepabonos",
                        "octcargos", "octabonos", "novcargos", "novabonos", "diccargos", "dicabonos", "cc", "descripcion", "descripcion2"
                    } };
                    string headerRange = "A1:AE1";

                    hoja1.Cells[headerRange].LoadFromArrays(headerRow);

                    var cellData = new List<object[]>();

                    foreach (var movimiento in lst)
                    {
                        cellData.Add(new object[]{
                        movimiento.cta,
                        movimiento.scta,
                        movimiento.sscta,
                        anio,
                        movimiento.salini,
                        movimiento.enecargos,
                        movimiento.eneabonos,
                        movimiento.febcargos,
                        movimiento.febabonos,
                        movimiento.marcargos,
                        movimiento.marabonos,
                        movimiento.abrcargos,
                        movimiento.abrabonos,
                        movimiento.maycargos,
                        movimiento.mayabonos,
                        movimiento.juncargos,
                        movimiento.junabonos,
                        movimiento.julcargos,
                        movimiento.julabonos,
                        movimiento.agocargos,
                        movimiento.agoabonos,
                        movimiento.sepcargos,
                        movimiento.sepabonos,
                        movimiento.octcargos,
                        movimiento.octabonos,
                        movimiento.novcargos,
                        movimiento.novabonos,
                        movimiento.diccargos,
                        movimiento.dicabonos,
                        movimiento.cc,
                        movimiento.descripcion,
                        movimiento.descripcion2
                        });
                    }

                    hoja1.Cells[2, 1].LoadFromArrays(cellData);

                    excel.Compression = CompressionLevel.BestSpeed;

                    var bytes = new MemoryStream();

                    using (var exportData = new MemoryStream())
                    {
                        excel.SaveAs(exportData);
                        bytes = exportData;
                    }

                    return bytes;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public MemoryStream crearExcelSemanalKubrix(int corteID, int tipo, List<string> areaCuenta)
        {
            try
            {
                var ccEnkontrol = getCCEnkontrol();
                var corte = _context.tblM_KBCorte.FirstOrDefault(x => x.id == corteID);
                var cortesAnteriores = _context.tblM_KBCorte.Where(x => x.fechaCorte < corte.fechaCorte && x.tipo == corte.tipo).OrderByDescending(x => x.fechaCorte).Take(5).ToList();
                List<int> cortesAnterioresID = new List<int>(5);
                List<int> cortesAnterioresCplanID = new List<int>(5);
                int corteAnteriorID = -1;
                List<ReporteSemanalKubrixDTO> auxDetallesCplan = new List<ReporteSemanalKubrixDTO>();

                var corte2ID = cortesAnteriores[0] == null ? -1 : cortesAnteriores[0].id;
                var corte3ID = cortesAnteriores[1] == null ? -1 : cortesAnteriores[1].id;
                var corte4ID = cortesAnteriores[2] == null ? -1 : cortesAnteriores[2].id;
                var corte5ID = cortesAnteriores[3] == null ? -1 : cortesAnteriores[3].id;
                var corte6ID = cortesAnteriores[4] == null ? -1 : cortesAnteriores[4].id;

                var corte2IDCplan = -1;
                var corte3IDCplan = -1;
                var corte4IDCplan = -1;
                var corte5IDCplan = -1;
                var corte6IDCplan = -1;
                for (int i = 0; i < 5; i++) { cortesAnterioresID.Add(cortesAnteriores[i] == null ? -1 : cortesAnteriores[i].id); }
                var anio = corte.fechaCorte.Year;
                if(tipo == 2) anio = 2020;
                List<ReporteSemanalKubrixDTO> auxDetalles = new List<ReporteSemanalKubrixDTO>();


                if (vSesiones.sesionEmpresaActual == 2)
                {
                    using (var _db = new MainContext((int)EmpresaEnum.Construplan))
                    {
                        var corteCplan = _db.tblM_KBCorte.Where(x => x.fechaCorte == corte.fechaCorte && x.tipo == corte.tipo).OrderByDescending(x => x.fecha).FirstOrDefault();
                        if (corteCplan != null)
                        {
                            corteAnteriorID = corteCplan.id;
                            var cortesAnterioresCplan = _db.tblM_KBCorte.Where(x => x.fechaCorte < corteCplan.fechaCorte && x.tipo == corteCplan.tipo).OrderByDescending(x => x.fechaCorte).Take(5).ToList();

                            corte2IDCplan = cortesAnterioresCplan.Count() > 0 ? cortesAnterioresCplan[0].id : -1;
                            corte3IDCplan = cortesAnterioresCplan.Count() > 1 ? cortesAnterioresCplan[1].id : -1;
                            corte4IDCplan = cortesAnterioresCplan.Count() > 2 ? cortesAnterioresCplan[2].id : -1;
                            corte5IDCplan = cortesAnterioresCplan.Count() > 3 ? cortesAnterioresCplan[3].id : -1;
                            corte6IDCplan = cortesAnterioresCplan.Count() > 4 ? cortesAnterioresCplan[4].id : -1;
                            for (int i = 0; i < cortesAnterioresCplan.Count(); i++) { cortesAnterioresCplanID.Add(cortesAnterioresCplan[i] == null ? -1 : cortesAnterioresCplan[i].id); }

                            auxDetallesCplan = _db.tblM_KBCorteDet.Where(x => (x.corteID == corteCplan.id || cortesAnterioresCplanID.Contains(x.corteID)) && x.fechapol >= new DateTime(anio, 1, 1)
                                 && !x.referencia.Contains("/" + (anio - 1).ToString()) && (x.cuenta.Contains("5000-10-") || x.cuenta.Contains("5280-10-") || x.cuenta.Contains("5900-3-"))).GroupBy(x => x.areaCuenta).Select(x => new ReporteSemanalKubrixDTO
                                 {
                                     cc = "",
                                     areaCuenta = x.Key,
                                     nombre = "",

                                     acumulado1 = (x.Where(y => (y.corteID == corte.id)).Count() > 0 ? x.Where(y => (y.corteID == corte.id)).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0)
                                             + (corteAnteriorID != -1 ? (x.Where(y => (y.corteID == corteAnteriorID) && y.empresa == 1).Count() > 0 ? x.Where(y => (y.corteID == corteAnteriorID) && y.empresa == 1).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0) : 0),
                                     acumulado2 = (x.Where(y => (y.corteID == corte2ID)).Count() > 0 ? x.Where(y => (y.corteID == corte2ID)).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0)
                                             + (corte2IDCplan != -1 ? (x.Where(y => (y.corteID == corte2IDCplan) && y.empresa == 1).Count() > 0 ? x.Where(y => (y.corteID == corte2IDCplan) && y.empresa == 1).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0) : 0),
                                     acumulado3 = (x.Where(y => (y.corteID == corte3ID)).Count() > 0 ? x.Where(y => (y.corteID == corte3ID)).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0)
                                             + (corte3IDCplan != -1 ? (x.Where(y => (y.corteID == corte3IDCplan) && y.empresa == 1).Count() > 0 ? x.Where(y => (y.corteID == corte3IDCplan) && y.empresa == 1).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0) : 0),
                                     acumulado4 = (x.Where(y => (y.corteID == corte4ID)).Count() > 0 ? x.Where(y => (y.corteID == corte4ID)).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0)
                                             + (corte4IDCplan != -1 ? (x.Where(y => (y.corteID == corte4IDCplan) && y.empresa == 1).Count() > 0 ? x.Where(y => (y.corteID == corte4IDCplan) && y.empresa == 1).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0) : 0),
                                     acumulado5 = (x.Where(y => (y.corteID == corte5ID)).Count() > 0 ? x.Where(y => (y.corteID == corte5ID)).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0)
                                             + (corte5IDCplan != -1 ? (x.Where(y => (y.corteID == corte5IDCplan) && y.empresa == 1).Count() > 0 ? x.Where(y => (y.corteID == corte5IDCplan) && y.empresa == 1).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0) : 0),
                                     acumulado6 = (x.Where(y => (y.corteID == corte6ID)).Count() > 0 ? x.Where(y => (y.corteID == corte6ID)).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0)
                                             + (corte6IDCplan != -1 ? (x.Where(y => (y.corteID == corte6IDCplan) && y.empresa == 1).Count() > 0 ? x.Where(y => (y.corteID == corte6IDCplan) && y.empresa == 1).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0) : 0),

                                 }).ToList();
                        }
                    }
                }

                var timeout = _context.Database.CommandTimeout;
                _context.Database.CommandTimeout = 500;

                auxDetalles = _context.tblM_KBCorteDet.Where(x => !x.cuenta.Contains("4000-5-") && !x.cuenta.Contains("4000-6-") && (x.corteID == corte.id || cortesAnterioresID.Contains(x.corteID)) && !x.cuenta.Contains("4000-9-")
                    && x.fechapol >= new DateTime(anio, 1, 1) && !x.referencia.Contains("/" + (anio - 1).ToString()) && x.areaCuenta != "6-5" && x.areaCuenta != "9-27").GroupBy(x => x.areaCuenta).Where(x => areaCuenta.Count() > 0 ? areaCuenta.Contains(x.Key) : true)
                    .Select(x => new ReporteSemanalKubrixDTO
                    {
                        cc = "",
                        areaCuenta = x.Key,
                        nombre = "",

                        acumulado1 = (x.Where(y => (y.corteID == corte.id)).Count() > 0 ? x.Where(y => (y.corteID == corte.id)).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0)
                                + (corteAnteriorID != -1 ? (x.Where(y => (y.corteID == corteAnteriorID) && y.empresa == 1).Count() > 0 ? x.Where(y => (y.corteID == corteAnteriorID) && y.empresa == 1).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0) : 0),
                        acumulado2 = (x.Where(y => (y.corteID == corte2ID)).Count() > 0 ? x.Where(y => (y.corteID == corte2ID)).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0)
                                + (corte2IDCplan != -1 ? (x.Where(y => (y.corteID == corte2IDCplan) && y.empresa == 1).Count() > 0 ? x.Where(y => (y.corteID == corte2IDCplan) && y.empresa == 1).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0) : 0),
                        acumulado3 = (x.Where(y => (y.corteID == corte3ID)).Count() > 0 ? x.Where(y => (y.corteID == corte3ID)).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0)
                                + (corte3IDCplan != -1 ? (x.Where(y => (y.corteID == corte3IDCplan) && y.empresa == 1).Count() > 0 ? x.Where(y => (y.corteID == corte3IDCplan) && y.empresa == 1).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0) : 0),
                        acumulado4 = (x.Where(y => (y.corteID == corte4ID)).Count() > 0 ? x.Where(y => (y.corteID == corte4ID)).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0)
                                + (corte4IDCplan != -1 ? (x.Where(y => (y.corteID == corte4IDCplan) && y.empresa == 1).Count() > 0 ? x.Where(y => (y.corteID == corte4IDCplan) && y.empresa == 1).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0) : 0),
                        acumulado5 = (x.Where(y => (y.corteID == corte5ID)).Count() > 0 ? x.Where(y => (y.corteID == corte5ID)).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0)
                                + (corte5IDCplan != -1 ? (x.Where(y => (y.corteID == corte5IDCplan) && y.empresa == 1).Count() > 0 ? x.Where(y => (y.corteID == corte5IDCplan) && y.empresa == 1).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0) : 0),

                    }).ToList();
                auxDetalles.AddRange(auxDetallesCplan);

                var auxDetallesEspeciales = _context.tblM_KBCorteDet.Where(x => (x.cuenta.Contains("4000-5-") || x.cuenta.Contains("4000-6-") || x.areaCuenta == "6-5" || x.areaCuenta == "9-27") && (x.corteID == corte.id || cortesAnterioresID.Contains(x.corteID))
                    && x.fechapol >= new DateTime(anio, 1, 1) && !x.referencia.Contains("/" + (anio - 1).ToString())).ToList();

                foreach (var item in auxDetallesEspeciales)
                {
                    if (item.cuenta.Contains("4000-6-")) item.areaCuenta = "6-5";
                    else if (item.cuenta.Contains("4000-5-")) item.areaCuenta = "9-27";
                }

                var auxDetallesGroup = auxDetallesEspeciales.GroupBy(x => x.areaCuenta).Where(x => areaCuenta.Count() > 0 ? areaCuenta.Contains(x.Key) : true)
                    .Select(x => new ReporteSemanalKubrixDTO
                    {
                        cc = "",
                        areaCuenta = x.Key,
                        nombre = "",

                        acumulado1 = (x.Where(y => (y.corteID == corte.id)).Count() > 0 ? x.Where(y => (y.corteID == corte.id)).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0)
                                + (corteAnteriorID != -1 ? (x.Where(y => (y.corteID == corteAnteriorID) && y.empresa == 1).Count() > 0 ? x.Where(y => (y.corteID == corteAnteriorID) && y.empresa == 1).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0) : 0),
                        acumulado2 = (x.Where(y => (y.corteID == corte2ID)).Count() > 0 ? x.Where(y => (y.corteID == corte2ID)).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0)
                                + (corte2IDCplan != -1 ? (x.Where(y => (y.corteID == corte2IDCplan) && y.empresa == 1).Count() > 0 ? x.Where(y => (y.corteID == corte2IDCplan) && y.empresa == 1).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0) : 0),
                        acumulado3 = (x.Where(y => (y.corteID == corte3ID)).Count() > 0 ? x.Where(y => (y.corteID == corte3ID)).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0)
                                + (corte3IDCplan != -1 ? (x.Where(y => (y.corteID == corte3IDCplan) && y.empresa == 1).Count() > 0 ? x.Where(y => (y.corteID == corte3IDCplan) && y.empresa == 1).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0) : 0),
                        acumulado4 = (x.Where(y => (y.corteID == corte4ID)).Count() > 0 ? x.Where(y => (y.corteID == corte4ID)).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0)
                                + (corte4IDCplan != -1 ? (x.Where(y => (y.corteID == corte4IDCplan) && y.empresa == 1).Count() > 0 ? x.Where(y => (y.corteID == corte4IDCplan) && y.empresa == 1).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0) : 0),
                        acumulado5 = (x.Where(y => (y.corteID == corte5ID)).Count() > 0 ? x.Where(y => (y.corteID == corte5ID)).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0)
                                + (corte5IDCplan != -1 ? (x.Where(y => (y.corteID == corte5IDCplan) && y.empresa == 1).Count() > 0 ? x.Where(y => (y.corteID == corte5IDCplan) && y.empresa == 1).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0) : 0),
                        acumulado6 = (x.Where(y => (y.corteID == corte6ID)).Count() > 0 ? x.Where(y => (y.corteID == corte6ID)).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0)
                        + (corte6IDCplan != -1 ? (x.Where(y => (y.corteID == corte6IDCplan) && y.empresa == 1).Count() > 0 ? x.Where(y => (y.corteID == corte6IDCplan) && y.empresa == 1).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0) : 0),

                    }).ToList();

                auxDetalles.AddRange(auxDetallesGroup.ToList());

                List<ReporteSemanalKubrixDTO> lst = auxDetalles.OrderBy(x => x.cc).GroupBy(x => x.areaCuenta).Select(x =>
                {
                    var AC = _context.tblP_CC.FirstOrDefault(y => y.areaCuenta == x.Key);
                    if (AC == null) AC = _context.tblP_CC.FirstOrDefault(y => y.cc == x.Key);
                    var ccEnkontrolActual = ccEnkontrol.FirstOrDefault(y => y.CC == (x.Key.Contains("-") ? "" : x.Key));
                    return new ReporteSemanalKubrixDTO
                    {
                        cc = AC == null ? (x.Key.Contains("-") ? "" : x.Key) : AC.cc,
                        areaCuenta = x.Key.Contains("-") ? x.Key : "",
                        nombre = AC == null ? (ccEnkontrolActual == null ? "" : ccEnkontrolActual.Descripcion) : AC.descripcion,
                        semana1 = x.Sum(y => y.acumulado1) - x.Sum(y => y.acumulado2),
                        semana2 = x.Sum(y => y.acumulado2) - x.Sum(y => y.acumulado3),
                        semana3 = x.Sum(y => y.acumulado3) - x.Sum(y => y.acumulado4),
                        semana4 = x.Sum(y => y.acumulado4) - x.Sum(y => y.acumulado5),
                        semana5 = x.Sum(y => y.acumulado5) - x.Sum(y => y.acumulado6),
                        acumulado1 = x.Sum(y => y.acumulado1),
                        acumulado2 = x.Sum(y => y.acumulado2),
                        acumulado3 = x.Sum(y => y.acumulado3),
                        acumulado4 = x.Sum(y => y.acumulado4),
                        acumulado5 = x.Sum(y => y.acumulado5),
                    };
                }).OrderBy(x => x.cc).ToList();


                using (ExcelPackage excel = new ExcelPackage())
                {
                    var hoja1 = excel.Workbook.Worksheets.Add("Semanal");

                    List<string[]> headerRow = new List<string[]>() { new string[] { 
                        "CC", "Area Cuenta", "Descripción", 
                        "Semana " + ((char)10).ToString() + corte.fechaCorte.ToString("dd/MM/yyy"), "Acumulado " + ((char)10).ToString() + corte.fechaCorte.ToString("dd/MM/yyy"), 
                        "Semana " + ((char)10).ToString() + cortesAnteriores[0].fechaCorte.ToString("dd/MM/yyy"), "Acumulado " + ((char)10).ToString() + cortesAnteriores[0].fechaCorte.ToString("dd/MM/yyy"), 
                        "Semana " + ((char)10).ToString() + cortesAnteriores[1].fechaCorte.ToString("dd/MM/yyy"), "Acumulado " + ((char)10).ToString() + cortesAnteriores[1].fechaCorte.ToString("dd/MM/yyy"), 
                        "Semana " + ((char)10).ToString() + cortesAnteriores[2].fechaCorte.ToString("dd/MM/yyy"), "Acumulado " + ((char)10).ToString() + cortesAnteriores[2].fechaCorte.ToString("dd/MM/yyy"), 
                        "Semana " + ((char)10).ToString() + cortesAnteriores[3].fechaCorte.ToString("dd/MM/yyy"), "Acumulado " + ((char)10).ToString() + cortesAnteriores[3].fechaCorte.ToString("dd/MM/yyy")
                    } };
                    string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

                    hoja1.Cells[headerRange].LoadFromArrays(headerRow);
                    hoja1.Cells[headerRange].Style.WrapText = true;

                    var cellData = new List<object[]>();

                    foreach (var linea in lst)
                    {
                        cellData.Add(new object[]{
                        linea.cc,
                        linea.areaCuenta,
                        linea.nombre,
                        linea.semana1,
                        linea.acumulado1,
                        linea.semana2,
                        linea.acumulado2,
                        linea.semana3,
                        linea.acumulado3,
                        linea.semana4,
                        linea.acumulado4,
                        linea.semana5,
                        linea.acumulado5
                        });
                    }

                    hoja1.Cells[2, 1].LoadFromArrays(cellData);
                    excel.Compression = CompressionLevel.BestSpeed;
                    hoja1.Cells[hoja1.Dimension.Address].AutoFitColumns(10, 50);

                    ExcelRange range = hoja1.Cells[1, 1, hoja1.Dimension.End.Row, hoja1.Dimension.End.Column];
                    ExcelTable tab = hoja1.Tables.Add(range, "Table1");
                    tab.TableStyle = TableStyles.Medium2;
                    hoja1.Row(1).CustomHeight = false;

                    var bytes = new MemoryStream();

                    using (var exportData = new MemoryStream())
                    {
                        excel.SaveAs(exportData);
                        bytes = exportData;
                    }
                    _context.Database.CommandTimeout = timeout;

                    return bytes;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public MemoryStream crearExcelPorSubcuentaKubrix(int corteID, int tipo, List<string> areaCuenta)
        {
            try
            {
                var ccEnkontrol = getCCEnkontrol();
                var corte = _context.tblM_KBCorte.FirstOrDefault(x => x.id == corteID);
                var corteAnterior = _context.tblM_KBCorte.Where(x => x.fechaCorte < corte.fechaCorte && x.tipo == corte.tipo).OrderByDescending(x => x.fechaCorte).FirstOrDefault();
                tblM_KBCorte corteCplan = new tblM_KBCorte();
                var corteCplanID = -1;
                tblM_KBCorte corteAnteriorCplan = new tblM_KBCorte(); ;
                var corteAnteriorCplanID = -1;

                var anio = corte.fechaCorte.Year;
                if (tipo == 2) anio = 2020;
                // var auxDetalles = _context.tblM_KBCorteDet.Where(x => (x.corteID == corte.id || x.corteID == corteAnterior.id)
                //     && x.fechapol >= new DateTime(anio, 1, 1) && !x.referencia.Contains("/" + (anio - 1).ToString())).ToList();

                var auxDetalles = _context.tblM_KBCorteDet.Where(x => (x.corteID == corte.id || x.corteID == corteAnterior.id)
                    && x.fechapol >= new DateTime(anio, 1, 1) && !x.referencia.Contains("/" + (anio - 1).ToString())).GroupBy(x => new { x.areaCuenta, x.cuenta, x.corteID }).Select(x => new ReporteSemanalDTO
                    {
                        areaCuenta = x.Key.areaCuenta,
                        cuenta = x.Key.cuenta,
                        corteID = x.Key.corteID,
                        monto = x.Sum(y => y.monto)
                    }).ToList();

                foreach (var item in auxDetalles)
                {
                    if (item.cuenta.Contains("4000-6-")) item.areaCuenta = "6-5";
                    else { if (item.cuenta.Contains("4000-5-")) item.areaCuenta = "9-27"; }
                }

                //var auxDetalles = auxDetallesRaw.GroupBy(x => new { x.areaCuenta }).ToList();

                if (vSesiones.sesionEmpresaActual == 2)
                {
                    using (var _db = new MainContext((int)EmpresaEnum.Construplan))
                    {
                        corteCplan = _db.tblM_KBCorte.FirstOrDefault(x => x.fechaCorte == corte.fechaCorte && x.tipo == corte.tipo);

                        if (corteCplan != null)
                        {
                            corteAnteriorCplan = _db.tblM_KBCorte.Where(x => x.fechaCorte < corteCplan.fechaCorte && x.tipo == corteCplan.tipo).OrderByDescending(x => x.fechaCorte).FirstOrDefault();
                            corteCplanID = corteCplan.id;
                            if (corteAnteriorCplan == null)
                            {
                                var auxDetallesCplan = _db.tblM_KBCorteDet.Where(x => (x.corteID == corteCplan.id) && x.fechapol >= new DateTime(anio, 1, 1) && (x.cuenta.Contains("5000-10-") || x.cuenta.Contains("5280-10-") || x.cuenta.Contains("5900-3-"))).GroupBy(x => new { x.areaCuenta, x.cuenta, x.corteID }).Select(x => new ReporteSemanalDTO
                                {
                                    areaCuenta = x.Key.areaCuenta,
                                    cuenta = x.Key.cuenta,
                                    corteID = x.Key.corteID,
                                    monto = x.Sum(y => y.monto)
                                }).ToList();
                                auxDetalles.AddRange(auxDetallesCplan);
                            }
                            else
                            {
                                corteAnteriorCplanID = corteAnteriorCplan.id;
                                var auxDetallesCplan = _db.tblM_KBCorteDet.Where(x => (x.corteID == corteCplan.id || x.corteID == corteAnteriorCplan.id) && x.fechapol >= new DateTime(anio, 1, 1) && (x.cuenta.Contains("5000-10-") || x.cuenta.Contains("5280-10-") || x.cuenta.Contains("5900-3-"))).GroupBy(x => new { x.areaCuenta, x.cuenta, x.corteID }).Select(x => new ReporteSemanalDTO
                                {
                                    areaCuenta = x.Key.areaCuenta,
                                    cuenta = x.Key.cuenta,
                                    corteID = x.Key.corteID,
                                    monto = x.Sum(y => y.monto)
                                }).ToList();
                                auxDetalles.AddRange(auxDetallesCplan);
                            }

                        }
                    }
                }

                var lst = auxDetalles.Where(x => areaCuenta.Count() > 0 ? areaCuenta.Contains(x.areaCuenta) : true).GroupBy(x => new { x.areaCuenta }).ToList();

                var cuentasCostos = _context.tblM_KBCatCuenta.Where(x => (x.cuenta.Contains("5000-") || x.cuenta.Contains("5280") || x.cuenta.Contains("5900") || x.cuenta.Contains("5901")) && x.cuenta.Contains("-0")).ToList();

                using (ExcelPackage excel = new ExcelPackage())
                {
                    var hoja1 = excel.Workbook.Worksheets.Add("Por Subcuenta");
                    List<string> auxHeaders = new List<string>(); ;
                    auxHeaders.Add("CC");
                    auxHeaders.Add("AREA CUENTA");
                    auxHeaders.Add("DESCRIPCIÓN");

                    foreach (var auxCuenta in cuentasCostos)
                    {
                        if (auxCuenta.cuenta != "5000-0-0")
                        {
                            auxHeaders.Add(auxCuenta.cuenta + ((char)10).ToString() + auxCuenta.descripcion + ((char)10).ToString() + "SEMANA");
                            auxHeaders.Add(auxCuenta.cuenta + ((char)10).ToString() + auxCuenta.descripcion + ((char)10).ToString() + "ACUMULADO");
                        }
                    }
                    auxHeaders.Add("COSTO ESTIMADO" + ((char)10).ToString() + "SEMANA");
                    auxHeaders.Add("COSTO ESTIMADO" + ((char)10).ToString() + "ACUMULADO");
                    auxHeaders.Add("4900-1-0" + ((char)10).ToString() + "GANANCIA CAMBIARIA" + ((char)10).ToString() + "SEMANA");
                    auxHeaders.Add("4900-1-0" + ((char)10).ToString() + "GANANCIA CAMBIARIA" + ((char)10).ToString() + "ACUMULADO");

                    auxHeaders.Add("INGRESOS" + ((char)10).ToString() + "SEMANA");
                    auxHeaders.Add("INGRESOS" + ((char)10).ToString() + "ACUMULADO");
                    auxHeaders.Add("4901-1-0" + ((char)10).ToString() + "INGRESO POR VENTA ACTIVO FIJO" + ((char)10).ToString() + "SEMANA");
                    auxHeaders.Add("4901-1-0" + ((char)10).ToString() + "INGRESO POR VENTA ACTIVO FIJO" + ((char)10).ToString() + "ACUMULADO");
                    auxHeaders.Add("4901-2-0" + ((char)10).ToString() + "INGRESO OCASIONAL" + ((char)10).ToString() + "SEMANA");
                    auxHeaders.Add("4901-2-0" + ((char)10).ToString() + "INGRESO OCASIONAL" + ((char)10).ToString() + "ACUMULADO");

                    List<string[]> headerRow = new List<string[]>() { auxHeaders.ToArray() };

                    //foreach (var auxCuenta in cuentasCostos) { headerRow[0] = headerRow[0].Concat(new string[] { auxCuenta.cuenta + ((char)10).ToString() + auxCuenta.descripcion }); }

                    string headerRange = "A1:CA1";
                    hoja1.Cells[headerRange].LoadFromArrays(headerRow);
                    hoja1.Cells[headerRange].Style.WrapText = true;

                    var cellData = new List<object[]>();
                    foreach (var linea in lst)
                    {
                        List<object> auxCell = new List<object>();
                        var AC = _context.tblP_CC.FirstOrDefault(y => y.areaCuenta == linea.Key.areaCuenta);
                        if (AC == null) AC = _context.tblP_CC.FirstOrDefault(y => y.cc == linea.Key.areaCuenta);
                        var ccEnkontrolActual = ccEnkontrol.FirstOrDefault(y => y.CC == (linea.Key.areaCuenta.Contains("-") ? "" : linea.Key.areaCuenta));
                        auxCell.Add(AC == null ? (linea.Key.areaCuenta.Contains("-") ? "" : linea.Key.areaCuenta) : AC.cc);
                        auxCell.Add(linea.Key.areaCuenta.Contains("-") ? linea.Key.areaCuenta : "");
                        auxCell.Add(AC == null ? (ccEnkontrolActual == null ? "" : ccEnkontrolActual.Descripcion) : AC.descripcion);
                        foreach (var auxCuenta in cuentasCostos)
                        {
                            string ac = "";
                            int idx = auxCuenta.cuenta.LastIndexOf('-');
                            if (idx != -1) { ac = auxCuenta.cuenta.Substring(0, idx + 1); }
                            if (ac != "5000-0-")
                            {
                                auxCell.Add(
                                    linea.Where(y => y.cuenta.Contains(ac) && (y.corteID == corte.id || y.corteID == corteCplanID)).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (-1) : (1)))
                                    - linea.Where(y => y.cuenta.Contains(ac) && (y.corteID == corteAnterior.id || y.corteID == corteAnteriorCplanID)).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (-1) : (1)))
                                );
                                auxCell.Add(linea.Where(y => y.cuenta.Contains(ac) && (y.corteID == corte.id || y.corteID == corteCplanID)).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (-1) : (1))));
                            }
                        }
                        auxCell.Add(
                            linea.Where(y => y.cuenta == "1-4-0" && y.corteID == corte.id).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (-1) : (1)))
                            - linea.Where(y => y.cuenta == "1-4-0" && y.corteID == corteAnterior.id).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (-1) : (1)))
                        );
                        auxCell.Add(linea.Where(y => y.cuenta == "1-4-0" && y.corteID == corte.id).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (-1) : (1))));

                        auxCell.Add(
                            linea.Where(y => y.cuenta.Contains("4900-1-") && y.corteID == corte.id).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1)))
                            - linea.Where(y => y.cuenta.Contains("4900-1-") && y.corteID == corteAnterior.id).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1)))
                        );
                        auxCell.Add(linea.Where(y => y.cuenta.Contains("4900-1-") && y.corteID == corte.id).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))));

                        auxCell.Add(
                            linea.Where(y => ((y.cuenta.Contains("4000-") && !y.cuenta.Contains("4000-9-")) || y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2") && y.corteID == corte.id).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1)))
                            - linea.Where(y => ((y.cuenta.Contains("4000-") && !y.cuenta.Contains("4000-9-")) || y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2") && y.corteID == corteAnterior.id).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1)))
                        );
                        auxCell.Add(linea.Where(y => ((y.cuenta.Contains("4000-") && !y.cuenta.Contains("4000-9-")) || y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2") && y.corteID == corte.id).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))));

                        auxCell.Add(
                            linea.Where(y => y.cuenta.Contains("4901-1-") && y.corteID == corte.id).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1)))
                            - linea.Where(y => y.cuenta.Contains("4901-1-") && y.corteID == corteAnterior.id).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1)))
                        );
                        auxCell.Add(linea.Where(y => y.cuenta.Contains("4901-1-") && y.corteID == corte.id).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))));

                        auxCell.Add(
                            linea.Where(y => y.cuenta.Contains("4901-2-") && y.corteID == corte.id).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1)))
                            - linea.Where(y => y.cuenta.Contains("4901-2-") && y.corteID == corteAnterior.id).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1)))
                        );
                        auxCell.Add(linea.Where(y => y.cuenta.Contains("4901-2-") && y.corteID == corte.id).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))));


                        cellData.Add(auxCell.ToArray());
                    }
                    hoja1.Cells[2, 1].LoadFromArrays(cellData);
                    excel.Compression = CompressionLevel.BestSpeed;
                    hoja1.Cells[hoja1.Dimension.Address].AutoFitColumns(10, 50);

                    ExcelRange range = hoja1.Cells[1, 1, hoja1.Dimension.End.Row, hoja1.Dimension.End.Column];
                    ExcelTable tab = hoja1.Tables.Add(range, "Table1");
                    tab.TableStyle = TableStyles.Medium2;
                    hoja1.Row(1).CustomHeight = false;

                    var s = tab.Address.Start;
                    var e = tab.Address.End;

                    hoja1.Cells[s.Row + 1, s.Column, e.Row, e.Column].Sort(new[] { 0, 1 });

                    var bytes = new MemoryStream();
                    using (var exportData = new MemoryStream())
                    {
                        excel.SaveAs(exportData);
                        bytes = exportData;
                    }

                    return bytes;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<byte[]> ExcelSaldosCplan(int anio, EnkontrolEnum empresa = EnkontrolEnum.CplanProd)
        {
            try
            {
                List<byte[]> archivos = new List<byte[]>();
                OdbcConsultaDTO odbc = new OdbcConsultaDTO()
                {
                    consulta = string.Format(@"SELECT year, cta, scta, sscta, salini, enecargos, eneabonos, febcargos, febabonos, marcargos, marabonos,
                        abrcargos, abrabonos, maycargos, mayabonos, juncargos, junabonos, julcargos, julabonos, agocargos, agoabonos,
                        sepcargos, sepabonos, octcargos, octabonos, novcargos, novabonos, diccargos, dicabonos, cc,
                        (select descripcion from DBA.catcta where cta = a.cta and scta = a.scta and sscta = a.sscta) as descripcion,
                        (select descripcion from DBA.cc where cc = a.cc) as descripcion2
                    FROM DBA.sc_salcont_cc a where (year = {0} OR year = {1}) and cta > 0 
                        and (select count(cc) from DBA.cc where cc = a.cc) > 0
                        and (salini + enecargos + eneabonos  + febcargos + febabonos  + marcargos + marabonos  + abrcargos + abrabonos + maycargos 
                        + mayabonos + juncargos + junabonos + julcargos + julabonos + agocargos + agoabonos + sepcargos + sepabonos + octcargos + octabonos 
                        + novcargos + novabonos + diccargos + dicabonos) != 0
                        order by cc, cta, scta, sscta", anio, (anio - 1)),
                    parametros = new List<OdbcParameterDTO>()
                };

                List<SaldosKubrixDTO> lstTotal = _contextEnkontrolRentabilidad.SelectSaldos(empresa, odbc);

                for (int i = 0; i < 2; i++)
                {
                    var lst = lstTotal.Where(x => x.year == anio - i);
                    using (ExcelPackage excel = new ExcelPackage())
                    {
                        var hoja1 = excel.Workbook.Worksheets.Add("Saldos");

                        List<string[]> headerRow = new List<string[]>() { new string[] { 
                        "cta", "scta", "sscta", "year", "salini", "enecargos", "eneabonos", "febcargos", "febabonos", "marcargos", "marabonos", "abrcargos", "abrabonos",
                        "maycargos", "mayabonos", "juncargos", "junabonos", "julcargos", "julabonos", "agocargos", "agoabonos", "sepcargos", "sepabonos",
                        "octcargos", "octabonos", "novcargos", "novabonos", "diccargos", "dicabonos", "cc", "descripcion", "descripcion2"
                    } };
                        string headerRange = "A1:AE1";

                        hoja1.Cells[headerRange].LoadFromArrays(headerRow);

                        var cellData = new List<object[]>();

                        foreach (var movimiento in lst)
                        {
                            cellData.Add(new object[]{
                                movimiento.cta,
                                movimiento.scta,
                                movimiento.sscta,
                                movimiento.year,
                                movimiento.salini,
                                movimiento.enecargos,
                                movimiento.eneabonos,
                                movimiento.febcargos,
                                movimiento.febabonos,
                                movimiento.marcargos,
                                movimiento.marabonos,
                                movimiento.abrcargos,
                                movimiento.abrabonos,
                                movimiento.maycargos,
                                movimiento.mayabonos,
                                movimiento.juncargos,
                                movimiento.junabonos,
                                movimiento.julcargos,
                                movimiento.julabonos,
                                movimiento.agocargos,
                                movimiento.agoabonos,
                                movimiento.sepcargos,
                                movimiento.sepabonos,
                                movimiento.octcargos,
                                movimiento.octabonos,
                                movimiento.novcargos,
                                movimiento.novabonos,
                                movimiento.diccargos,
                                movimiento.dicabonos,
                                movimiento.cc,
                                movimiento.descripcion,
                                movimiento.descripcion2
                            });
                        }

                        hoja1.Cells[2, 1].LoadFromArrays(cellData);

                        excel.Compression = CompressionLevel.BestSpeed;

                        byte[] binaryData = null;

                        using (var exportData = new MemoryStream())
                        {
                            excel.SaveAs(exportData);
                            binaryData = exportData.ToArray();
                        }

                        archivos.Add(binaryData);

                    }
                }
                return archivos;

            }
            catch (Exception e) { return null; }
        }

        public byte[] ExcelSaldosCplanVirtual(int anio)
        {
            try
            {
                OdbcConsultaDTO odbc = new OdbcConsultaDTO()
                {
                    consulta = string.Format(@"SELECT year, cta, scta, sscta, salini, enecargos, eneabonos, febcargos, febabonos, marcargos, marabonos,
                        abrcargos, abrabonos, maycargos, mayabonos, juncargos, junabonos, julcargos, julabonos, agocargos, agoabonos,
                        sepcargos, sepabonos, octcargos, octabonos, novcargos, novabonos, diccargos, dicabonos, cc,
                        (select descripcion from DBA.catcta where cta = a.cta and scta = a.scta and sscta = a.sscta) as descripcion,
                        (select descripcion from DBA.cc where cc = a.cc) as descripcion2
                    FROM DBA.sc_salcont_cc a where (year = {0}) and cta > 0 
                        and (select count(cc) from DBA.cc where cc = a.cc) > 0
                        and (salini + enecargos + eneabonos  + febcargos + febabonos  + marcargos + marabonos  + abrcargos + abrabonos + maycargos 
                        + mayabonos + juncargos + junabonos + julcargos + julabonos + agocargos + agoabonos + sepcargos + sepabonos + octcargos + octabonos 
                        + novcargos + novabonos + diccargos + dicabonos) != 0
                        order by cc, cta, scta, sscta", anio),
                    parametros = new List<OdbcParameterDTO>()
                };
                List<SaldosKubrixDTO> lst = _contextEnkontrolRentabilidad.SelectSaldos(EnkontrolEnum.CplanVirtual, odbc);

                using (ExcelPackage excel = new ExcelPackage())
                {
                    var hoja1 = excel.Workbook.Worksheets.Add("Saldos");

                    List<string[]> headerRow = new List<string[]>() { new string[] { 
                        "cta", "scta", "sscta", "year", "salini", "enecargos", "eneabonos", "febcargos", "febabonos", "marcargos", "marabonos", "abrcargos", "abrabonos",
                        "maycargos", "mayabonos", "juncargos", "junabonos", "julcargos", "julabonos", "agocargos", "agoabonos", "sepcargos", "sepabonos",
                        "octcargos", "octabonos", "novcargos", "novabonos", "diccargos", "dicabonos", "cc", "descripcion", "descripcion2"
                    } };
                    string headerRange = "A1:AE1";

                    hoja1.Cells[headerRange].LoadFromArrays(headerRow);

                    var cellData = new List<object[]>();

                    foreach (var movimiento in lst)
                    {
                        cellData.Add(new object[]{
                        movimiento.cta,
                        movimiento.scta,
                        movimiento.sscta,
                        anio,
                        movimiento.salini,
                        movimiento.enecargos,
                        movimiento.eneabonos,
                        movimiento.febcargos,
                        movimiento.febabonos,
                        movimiento.marcargos,
                        movimiento.marabonos,
                        movimiento.abrcargos,
                        movimiento.abrabonos,
                        movimiento.maycargos,
                        movimiento.mayabonos,
                        movimiento.juncargos,
                        movimiento.junabonos,
                        movimiento.julcargos,
                        movimiento.julabonos,
                        movimiento.agocargos,
                        movimiento.agoabonos,
                        movimiento.sepcargos,
                        movimiento.sepabonos,
                        movimiento.octcargos,
                        movimiento.octabonos,
                        movimiento.novcargos,
                        movimiento.novabonos,
                        movimiento.diccargos,
                        movimiento.dicabonos,
                        movimiento.cc,
                        movimiento.descripcion,
                        movimiento.descripcion2
                        });
                    }

                    hoja1.Cells[2, 1].LoadFromArrays(cellData);

                    excel.Compression = CompressionLevel.BestSpeed;

                    byte[] binaryData = null;

                    using (var exportData = new MemoryStream())
                    {
                        excel.SaveAs(exportData);
                        binaryData = exportData.ToArray();
                    }

                    return binaryData;

                }
            }
            catch (Exception e) { return null; }
        }

        public byte[] ExcelClientesCplan(EnkontrolEnum empresa = EnkontrolEnum.CplanProd)
        {
            try
            {
                OdbcConsultaDTO odbc = new OdbcConsultaDTO()
                {
                    consulta = string.Format(@"SELECT 
                            final.numcte, final.nombre, final.factura, final.tm, final.fecha, final.vencimiento, final.id_segmento, final.desc_segmento, final.tipo_cliente, final.moneda, final.tipocambio, final.cc, final.desc_cc, 
                            final.corto, final.corte, final.plazo1, final.plazo2, final.plazo3, final.dif1, final.dif2, final.dif3, final.movs_x_factura,
                            CASE WHEN final.sum_venc_x_fact > 0 THEN final.sum_venc_x_fact ELSE 0 END as sum_venc_x_fact, CASE WHEN final.sum_por_vencer > 0 THEN final.sum_por_vencer ELSE 0 END as sum_por_vencer,
                            CASE WHEN final.sum_vencido_1 > 0 THEN final.sum_vencido_1 ELSE 0 END as sum_vencido_1, CASE WHEN final.sum_vencido_2 > 0 THEN final.sum_vencido_2 ELSE 0 END as sum_vencido_2, 
                            CASE WHEN final.sum_vencido_3 > 0 THEN final.sum_vencido_3 ELSE 0 END as sum_vencido_3, CASE WHEN final.sum_vencido_4 > 0 THEN final.sum_vencido_4 ELSE 0 END as sum_vencido_4,
                            CASE WHEN final.sum_vencido > 0 THEN final.sum_vencido ELSE 0 END as sum_vencido, final.saldo_x_factura, final.monto_factura, final.descrip_tm, final.concep_mov, final.Is_telefono
                        FROM
                        (
                        SELECT 
                            movimiento.numcte as numcte, MIN(cliente.nombre) as nombre, movimiento.factura as factura, MIN(movimiento.tm) as tm, MIN(movimiento.fecha) as fecha, MIN(movimiento.fechavenc) as vencimiento,
                            1 as id_segmento, 'SEGMENTO DEFAULT' as desc_segmento, MIN(cliente.tipo_cliente) as tipo_cliente, MIN(movimiento.moneda) as moneda, MIN(movimiento.tipocambio) as tipocambio, movimiento.cc as cc,
                            MIN(centro_costos.descripcion) as desc_cc, MIN(centro_costos.corto) as corto, '{0}' as corte, 15 as plazo1, 30 as plazo2, 60 as plazo3, CAST(DATEADD(day, -15, '{0}') as date) AS dif1,
                            CAST(DATEADD(day, -30, '{0}') as date) AS dif2, CAST(DATEADD(day, -60, '{0}') as date) AS dif3, (select COUNT(factura) from sx_movcltes where movimiento.factura = factura and numcte = movimiento.numcte) as movs_x_factura,
                            SUM(CASE WHEN (movimiento.fechavenc < '{0}') THEN  movimiento.total ELSE 0 END) + ISNULL( (select SUM(total) from sx_movcltes where tm in (select tm from sx_tm where naturaleza in (2,3)) AND factura = movimiento.factura AND numcte = movimiento.numcte), 0) AS sum_venc_x_fact, 
                            SUM(CASE WHEN (movimiento.fechavenc BETWEEN '{0}' AND CAST(DATEADD(day, 15, '{0}') as date)) THEN  movimiento.total ELSE 0 END) + ISNULL( (select SUM(total) from sx_movcltes where tm in (select tm from sx_tm where naturaleza in (2,3)) AND factura = movimiento.factura AND numcte = movimiento.numcte), 0) AS sum_por_vencer, 
                            SUM(CASE WHEN (movimiento.fechavenc BETWEEN CAST(DATEADD(day, -15, '{0}') as date) AND '{0}') THEN  movimiento.total ELSE 0 END) + ISNULL( (select SUM(total) from sx_movcltes where tm in (select tm from sx_tm where naturaleza in (2,3)) AND factura = movimiento.factura AND numcte = movimiento.numcte), 0) AS sum_vencido_1,
                            SUM(CASE WHEN (movimiento.fechavenc BETWEEN CAST(DATEADD(day, -30, '{0}') as date) AND CAST(DATEADD(day, -15, '{0}') as date)) THEN  movimiento.total ELSE 0 END) + ISNULL( (select SUM(total) from sx_movcltes where tm in (select tm from sx_tm where naturaleza in (2,3)) AND factura = movimiento.factura AND numcte = movimiento.numcte), 0) AS sum_vencido_2,
                            SUM(CASE WHEN (movimiento.fechavenc BETWEEN CAST(DATEADD(day, -60, '{0}') as date) AND CAST(DATEADD(day, -30, '{0}') as date)) THEN  movimiento.total ELSE 0 END) + ISNULL( (select SUM(total) from sx_movcltes where tm in (select tm from sx_tm where naturaleza in (2,3)) AND factura = movimiento.factura AND numcte = movimiento.numcte), 0) AS sum_vencido_3,
                            SUM(CASE WHEN (movimiento.fechavenc < CAST(DATEADD(day, -60, '{0}') as date)) THEN  movimiento.total ELSE 0 END) + ISNULL( (select SUM(total) from sx_movcltes where tm in (select tm from sx_tm where naturaleza in (2,3)) AND factura = movimiento.factura AND numcte = movimiento.numcte), 0) AS sum_vencido_4,
                            SUM(CASE WHEN (movimiento.fechavenc < '{0}') THEN  movimiento.total ELSE 0 END) + ISNULL( (select SUM(total) from sx_movcltes where tm in (select tm from sx_tm where naturaleza in (2,3)) AND factura = movimiento.factura AND numcte = movimiento.numcte), 0) AS sum_vencido,
                            (select SUM(total) from sx_movcltes where factura = movimiento.factura and numcte = movimiento.numcte) AS saldo_x_factura,
                            SUM(movimiento.total) as monto_factura,
                            (select MIN(descripcion) from sx_tm where tm = MIN(movimiento.tm)) as descrip_tm,
                            MIN(movimiento.concepto) as concep_mov,
                            ISNULL(MIN(cliente.telefono1), '') AS Is_telefono,
                            (select SUM(total) from sx_movcltes where numcte = movimiento.numcte group by numcte) AS suma_cliente
                        FROM (select * from sx_movcltes where tm in (select tm from sx_tm where naturaleza in (1,4))) movimiento
                        INNER JOIN sx_clientes cliente ON movimiento.numcte = cliente.numcte
                        INNER JOIN cc centro_costos ON centro_costos.cc = movimiento.cc

                        GROUP BY movimiento.numcte, movimiento.factura, movimiento.cc
                        ) final WHERE final.suma_cliente != 0 ORDER BY final.numcte", DateTime.Today.ToString("yyyy-MM-dd")),
                    parametros = new List<OdbcParameterDTO>()
                };

                List<ClientesKubrixDTO> lst = _contextEnkontrolRentabilidad.Select<ClientesKubrixDTO>(empresa, odbc);

                using (ExcelPackage excel = new ExcelPackage())
                {
                    var hoja1 = excel.Workbook.Worksheets.Add("Clientes");

                    List<string[]> headerRow = new List<string[]>() { new string[] { 
                        "numcte", "nombre", "factura", "tm", "fecha", "vencimiento", "id_segmento", "desc_segmento", "tipo_cliente", "desc_tipo_cliente", 
                        "moneda", "tipocambio", "desc_moneda", "cc", "desc_cc", "corto", "corte", "plazo1", "plazo2", "plazo3", "dif1", "dif2", "dif3", 
                        "movs_x_factura", "sum_venc_x_fact", "sum_por_vencer", "sum_vencido_1", "sum_vencido_2", "sum_vencido_3", "sum_vencido_4", 
                        "sum_vencido", "saldo_x_factura", "monto_factura", "fact_abono_no_apli", "monto_fact_abono_no_apli", "fecha_max", 
                        "ultimo_tipocambio", "descrip_tm", "concep_mov", "ls_telefono"

                    } };
                    string headerRange = "A1:AN1";

                    hoja1.Cells[headerRange].LoadFromArrays(headerRow);

                    var cellData = new List<object[]>();

                    foreach (var movimiento in lst)
                    {
                        cellData.Add(new object[]{
                            movimiento.numcte,
                            movimiento.nombre,
                            movimiento.factura,
                            movimiento.tm,
                            movimiento.fecha.ToString("dd/MM/yyyy"),
                            movimiento.vencimiento.ToString("dd/MM/yyyy"),
                            movimiento.id_segmento,
                            movimiento.desc_segmento,
                            movimiento.tipo_cliente,
                            movimiento.desc_tipo_cliente,
                            movimiento.moneda,
                            movimiento.tipocambio,
                            movimiento.desc_moneda,
                            movimiento.cc,
                            movimiento.desc_cc,
                            movimiento.corto,
                            DateTime.Today.ToString("dd/MM/yyyy"),
                            movimiento.plazo1,
                            movimiento.plazo2,
                            movimiento.plazo3,
                            movimiento.dif1.ToString("dd/MM/yyyy"),
                            movimiento.dif2.ToString("dd/MM/yyyy"),
                            movimiento.dif3.ToString("dd/MM/yyyy"),
                            movimiento.movs_x_factura,
                            movimiento.sum_venc_x_fact,
                            movimiento.sum_por_vencer,
                            movimiento.sum_vencido_1,
                            movimiento.sum_vencido_2,
                            movimiento.sum_vencido_3,
                            movimiento.sum_vencido_4,
                            movimiento.sum_vencido,
                            movimiento.saldo_x_factura,
                            movimiento.monto_factura,
                            movimiento.fact_abono_no_apli,
                            movimiento.monto_fact_abono_no_apli,
                            movimiento.fecha_max.ToString("dd/MM/yyyy"),
                            movimiento.ultimo_tipocambio,
                            movimiento.descrip_tm,
                            movimiento.concep_mov,
                            movimiento.ls_telefono
                        });
                    }

                    hoja1.Cells[2, 1].LoadFromArrays(cellData);

                    excel.Compression = CompressionLevel.BestSpeed;

                    byte[] binaryData = null;

                    using (var exportData = new MemoryStream())
                    {
                        excel.SaveAs(exportData);
                        binaryData = exportData.ToArray();
                    }

                    return binaryData;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public byte[] ExcelVencimientosCplan(EnkontrolEnum empresa = EnkontrolEnum.CplanProd)
        {
            try
            {
                OdbcConsultaDTO odbc = new OdbcConsultaDTO()
                {
                    consulta = string.Format(@"SELECT 
                            final.numpro, final.nombre, 
                            CASE WHEN final.por_vencer > 0 THEN final.por_vencer ELSE 0 END AS por_vencer, 
                            CASE WHEN final.plazo_1 > 0 THEN final.plazo_1 ELSE 0 END AS plazo_1, 
                            CASE WHEN final.plazo_2 > 0 THEN final.plazo_2 ELSE 0 END AS plazo_2, 
                            CASE WHEN final.plazo_3 > 0 THEN final.plazo_3 ELSE 0 END AS plazo_3, 
                            CASE WHEN final.plazo_4 > 0 THEN final.plazo_4 ELSE 0 END AS plazo_4,
                            final.factura, final.moneda, final.fecha, final.fechavenc, final.tm, final.autorizapago, 
                            final.saldo_factura, final.plazo1, final.plazo2, final.plazo3, final.plazo4, final.cc, final.descripcion,
                            final.tipocambio, final.nomcorto, final.corto, ISNULL(final.telefono1, '') as telefono1, 
                            CASE WHEN moneda_id = 1 THEN final.saldo_factura ELSE 0 END AS saldo_factura_dlls, final.concepto, final.moneda_id
                        FROM
                        (SELECT 
                            movimiento.numpro as numpro, MIN(proveedor.nombre) as nombre, 
                            SUM(CASE WHEN (movimiento.fechavenc BETWEEN '{0}' AND CAST(DATEADD(day, 7, '{0}') as date)) THEN  movimiento.total ELSE 0 END) 
                                + ISNULL( (select SUM(total) from sp_movprov where tm in (select tm from sp_tm where naturaleza in (2,3)) AND factura = movimiento.factura AND numpro = movimiento.numpro), 0) AS por_vencer,    
                            SUM(CASE WHEN (movimiento.fechavenc BETWEEN CAST(DATEADD(day, -7, '{0}') as date) AND '{0}') THEN  movimiento.total ELSE 0 END) + ISNULL( (select SUM(total) from sp_movprov where tm in (select tm from sp_tm where naturaleza in (2,3)) AND factura = movimiento.factura AND numpro = movimiento.numpro), 0) AS plazo_1,
                            SUM(CASE WHEN (movimiento.fechavenc BETWEEN CAST(DATEADD(day, -14, '{0}') as date) AND CAST(DATEADD(day, -7, '{0}') as date)) THEN  movimiento.total ELSE 0 END) + ISNULL( (select SUM(total) from sp_movprov where tm in (select tm from sp_tm where naturaleza in (2,3)) AND factura = movimiento.factura AND numpro = movimiento.numpro), 0) AS plazo_2,
                            SUM(CASE WHEN (movimiento.fechavenc BETWEEN CAST(DATEADD(day, -30, '{0}') as date) AND CAST(DATEADD(day, -14, '{0}') as date)) THEN  movimiento.total ELSE 0 END) + ISNULL( (select SUM(total) from sp_movprov where tm in (select tm from sp_tm where naturaleza in (2,3)) AND factura = movimiento.factura AND numpro = movimiento.numpro), 0) AS plazo_3,
                            SUM(CASE WHEN (movimiento.fechavenc < CAST(DATEADD(day, -30, '{0}') as date)) THEN  movimiento.total ELSE 0 END) + ISNULL( (select SUM(total) from sp_movprov where tm in (select tm from sp_tm where naturaleza in (2,3)) AND factura = movimiento.factura AND numpro = movimiento.numpro), 0) AS plazo_4,
                            movimiento.factura, (select moneda from moneda where clave = MIN(movimiento.moneda)) as moneda, MIN(movimiento.fecha) as fecha, MIN(movimiento.fechavenc) as fechavenc, MIN(movimiento.tm) as tm, MIN(movimiento.autorizapago) as autorizapago,
                            (select SUM(total) from sp_movprov where factura = movimiento.factura and numpro = movimiento.numpro) AS saldo_factura, 7 as plazo1, 14 as plazo2, 30 as plazo3, 30 as plazo4, movimiento.cc as cc, MIN(centro_costos.descripcion) as descripcion,
                            MIN(movimiento.tipocambio) as tipocambio, MIN(proveedor.nomcorto) as nomcorto, MIN(centro_costos.corto) as corto, MIN(proveedor.telefono1) as telefono1,
    
                            MIN(movimiento.concepto) as concepto, MIN(movimiento.moneda) as moneda_id
                        FROM 
                            (select * from sp_movprov where tm in (select tm from sp_tm where naturaleza in (1,4))) movimiento
                            INNER JOIN sp_proveedores proveedor ON movimiento.numpro = proveedor.numpro
                            INNER JOIN cc centro_costos ON centro_costos.cc = movimiento.cc
                        GROUP BY movimiento.numpro, movimiento.factura, movimiento.cc) final
                        WHERE final.saldo_factura != 0 ORDER BY final.numpro", DateTime.Today.ToString("yyy-MM-dd")),
                    parametros = new List<OdbcParameterDTO>()
                };

                List<VencimientoKubrixDTO> lst = _contextEnkontrolRentabilidad.Select<VencimientoKubrixDTO>(empresa, odbc);

                using (ExcelPackage excel = new ExcelPackage())
                {
                    var hoja1 = excel.Workbook.Worksheets.Add("Vencimientos");

                    List<string[]> headerRow = new List<string[]>() { new string[] { 
                        "numpro", "nombre", "por_vencer", "plazo1", "plazo2", "plazo3", "plazo4", "factura", "moneda", "fecha", "fechavenc", "tm", 
                        "autorizapago", "saldo_factura", "plazo1", "plazo2", "plazo3", "plazo4", "cc", "descripcion", "tipocambio", "nomcorto", "corto", 
                        "telefono1", "saldo_factura_dlls", "concepto", "moneda"

                    } };
                    string headerRange = "A1:AA1";

                    hoja1.Cells[headerRange].LoadFromArrays(headerRow);

                    var cellData = new List<object[]>();

                    foreach (var movimiento in lst)
                    {
                        cellData.Add(new object[]{
                            movimiento.numpro,
                            movimiento.nombre,
                            movimiento.por_vencer,
                            movimiento.plazo_1,
                            movimiento.plazo_2,
                            movimiento.plazo_3,
                            movimiento.plazo_4,
                            movimiento.factura,
                            movimiento.moneda,
                            movimiento.fecha.ToString("dd/MM/yyyy"),
                            movimiento.fechavenc.ToString("dd/MM/yyyy"),
                            movimiento.tm,
                            movimiento.autorizapago,
                            movimiento.saldo_factura,
                            movimiento.plazo1,
                            movimiento.plazo2,
                            movimiento.plazo3,
                            movimiento.plazo4,
                            movimiento.cc,
                            movimiento.descripcion,
                            movimiento.tipocambio,
                            movimiento.nomcorto,
                            movimiento.corto,
                            movimiento.telefono1,
                            movimiento.saldo_factura_dlls,
                            movimiento.concepto,
                            movimiento.moneda
                        });
                    }

                    hoja1.Cells[2, 1].LoadFromArrays(cellData);

                    excel.Compression = CompressionLevel.BestSpeed;

                    byte[] binaryData = null;

                    using (var exportData = new MemoryStream())
                    {
                        excel.SaveAs(exportData);
                        binaryData = exportData.ToArray();
                    }

                    return binaryData;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public byte[] ExcelSemanalKubrix(int corteID)
        {
            try
            {
                tblM_KBCorte corte = new tblM_KBCorte();
                List<tblM_KBCorte> cortesAnteriores = new List<tblM_KBCorte>();

                List<ReporteSemanalKubrixDTO> lst = new List<ReporteSemanalKubrixDTO>();
                List<int> cortesAnterioresID = new List<int>(5);
                List<int> cortesAnterioresCplanID = new List<int>(5);
                int corteAnteriorID = -1;

                var corte2ID = -1;
                var corte3ID = -1;
                var corte4ID = -1;
                var corte5ID = -1;
                var corte6ID = -1;

                var corte2IDCplan = -1;
                var corte3IDCplan = -1;
                var corte4IDCplan = -1;
                var corte5IDCplan = -1;
                var corte6IDCplan = -1;

                using (var _dbArrendadora = new MainContext((int)EmpresaEnum.Arrendadora))
                {
                    var ccEnkontrol = getCCEnkontrol();
                    corte = _dbArrendadora.tblM_KBCorte.FirstOrDefault(x => x.id == corteID);
                    cortesAnteriores = _dbArrendadora.tblM_KBCorte.Where(x => x.fechaCorte < corte.fechaCorte && x.tipo == corte.tipo).OrderByDescending(x => x.fechaCorte).Take(5).ToList();
                    corte2ID = cortesAnteriores[0] == null ? -1 : cortesAnteriores[0].id;
                    corte3ID = cortesAnteriores[1] == null ? -1 : cortesAnteriores[1].id;
                    corte4ID = cortesAnteriores[2] == null ? -1 : cortesAnteriores[2].id;
                    corte5ID = cortesAnteriores[3] == null ? -1 : cortesAnteriores[3].id;
                    corte6ID = cortesAnteriores[4] == null ? -1 : cortesAnteriores[4].id;
                    for (int i = 0; i < 5; i++) { cortesAnterioresID.Add(cortesAnteriores[i] == null ? -1 : cortesAnteriores[i].id); }
                    var anio = corte.fechaCorte.Year;
                    List<ReporteSemanalKubrixDTO> auxDetalles = new List<ReporteSemanalKubrixDTO>();


                    using (var _dbCplan = new MainContext((int)EmpresaEnum.Construplan))
                    {
                        var corteCplan = _dbCplan.tblM_KBCorte.FirstOrDefault(x => x.fechaCorte == corte.fechaCorte && x.tipo == corte.tipo);
                        if (corteCplan != null)
                        {
                            corteAnteriorID = corteCplan.id;
                            var cortesAnterioresCplan = _dbCplan.tblM_KBCorte.Where(x => x.fechaCorte < corteCplan.fechaCorte && x.tipo == corteCplan.tipo).OrderByDescending(x => x.fechaCorte).Take(5).ToList();

                            corte2IDCplan = cortesAnterioresCplan.Count() > 0 ? cortesAnterioresCplan[0].id : -1;
                            corte3IDCplan = cortesAnterioresCplan.Count() > 1 ? cortesAnterioresCplan[1].id : -1;
                            corte4IDCplan = cortesAnterioresCplan.Count() > 2 ? cortesAnterioresCplan[2].id : -1;
                            corte5IDCplan = cortesAnterioresCplan.Count() > 3 ? cortesAnterioresCplan[3].id : -1;
                            corte6IDCplan = cortesAnterioresCplan.Count() > 4 ? cortesAnterioresCplan[4].id : -1;
                            for (int i = 0; i < cortesAnterioresCplan.Count(); i++) { cortesAnterioresCplanID.Add(cortesAnterioresCplan[i] == null ? -1 : cortesAnterioresCplan[i].id); }

                            var auxDetallesCplan = _dbCplan.tblM_KBCorteDet.Where(x => (x.corteID == corteCplan.id || cortesAnterioresCplanID.Contains(x.corteID)) && x.fechapol >= new DateTime(anio, 1, 1)
                                    && !x.referencia.Contains("/" + (anio - 1).ToString()) && (x.cuenta.Contains("5000-10-") || x.cuenta.Contains("5280-10-") || x.cuenta.Contains("5900-3-"))).GroupBy(x => x.areaCuenta).Select(x => new ReporteSemanalKubrixDTO
                                    {
                                        cc = "",
                                        areaCuenta = x.Key,
                                        nombre = "",

                                        acumulado1 = (x.Where(y => (y.corteID == corte.id)).Count() > 0 ? x.Where(y => (y.corteID == corte.id)).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0)
                                                + (corteAnteriorID != -1 ? (x.Where(y => (y.corteID == corteAnteriorID) && y.empresa == 1).Count() > 0 ? x.Where(y => (y.corteID == corteAnteriorID) && y.empresa == 1).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0) : 0),
                                        acumulado2 = (x.Where(y => (y.corteID == corte2ID)).Count() > 0 ? x.Where(y => (y.corteID == corte2ID)).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0)
                                                + (corte2IDCplan != -1 ? (x.Where(y => (y.corteID == corte2IDCplan) && y.empresa == 1).Count() > 0 ? x.Where(y => (y.corteID == corte2IDCplan) && y.empresa == 1).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0) : 0),
                                        acumulado3 = (x.Where(y => (y.corteID == corte3ID)).Count() > 0 ? x.Where(y => (y.corteID == corte3ID)).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0)
                                                + (corte3IDCplan != -1 ? (x.Where(y => (y.corteID == corte3IDCplan) && y.empresa == 1).Count() > 0 ? x.Where(y => (y.corteID == corte3IDCplan) && y.empresa == 1).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0) : 0),
                                        acumulado4 = (x.Where(y => (y.corteID == corte4ID)).Count() > 0 ? x.Where(y => (y.corteID == corte4ID)).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0)
                                                + (corte4IDCplan != -1 ? (x.Where(y => (y.corteID == corte4IDCplan) && y.empresa == 1).Count() > 0 ? x.Where(y => (y.corteID == corte4IDCplan) && y.empresa == 1).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0) : 0),
                                        acumulado5 = (x.Where(y => (y.corteID == corte5ID)).Count() > 0 ? x.Where(y => (y.corteID == corte5ID)).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0)
                                                + (corte5IDCplan != -1 ? (x.Where(y => (y.corteID == corte5IDCplan) && y.empresa == 1).Count() > 0 ? x.Where(y => (y.corteID == corte5IDCplan) && y.empresa == 1).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0) : 0),
                                        acumulado6 = (x.Where(y => (y.corteID == corte6ID)).Count() > 0 ? x.Where(y => (y.corteID == corte6ID)).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0)
                                        + (corte6IDCplan != -1 ? (x.Where(y => (y.corteID == corte6IDCplan) && y.empresa == 1).Count() > 0 ? x.Where(y => (y.corteID == corte6IDCplan) && y.empresa == 1).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0) : 0),

                                    }).ToList();

                            auxDetalles.AddRange(auxDetallesCplan);
                        }
                    }


                    var timeout = _dbArrendadora.Database.CommandTimeout;
                    _dbArrendadora.Database.CommandTimeout = 300;

                    var auxDetallesArrendadora = _dbArrendadora.tblM_KBCorteDet.Where(x => !x.cuenta.Contains("4000-5-") && !x.cuenta.Contains("4000-6-") && (x.corteID == corte.id || cortesAnterioresID.Contains(x.corteID))
                        && x.fechapol >= new DateTime(anio, 1, 1) && !x.referencia.Contains("/" + (anio - 1).ToString()) && x.areaCuenta != "6-5" && x.areaCuenta != "9-27").GroupBy(x => x.areaCuenta)
                        .Select(x => new ReporteSemanalKubrixDTO
                        {
                            cc = "",
                            areaCuenta = x.Key,
                            nombre = "",

                            acumulado1 = (x.Where(y => (y.corteID == corte.id)).Count() > 0 ? x.Where(y => (y.corteID == corte.id)).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0)
                                    + (corteAnteriorID != -1 ? (x.Where(y => (y.corteID == corteAnteriorID) && y.empresa == 1).Count() > 0 ? x.Where(y => (y.corteID == corteAnteriorID) && y.empresa == 1).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0) : 0),
                            acumulado2 = (x.Where(y => (y.corteID == corte2ID)).Count() > 0 ? x.Where(y => (y.corteID == corte2ID)).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0)
                                    + (corte2IDCplan != -1 ? (x.Where(y => (y.corteID == corte2IDCplan) && y.empresa == 1).Count() > 0 ? x.Where(y => (y.corteID == corte2IDCplan) && y.empresa == 1).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0) : 0),
                            acumulado3 = (x.Where(y => (y.corteID == corte3ID)).Count() > 0 ? x.Where(y => (y.corteID == corte3ID)).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0)
                                    + (corte3IDCplan != -1 ? (x.Where(y => (y.corteID == corte3IDCplan) && y.empresa == 1).Count() > 0 ? x.Where(y => (y.corteID == corte3IDCplan) && y.empresa == 1).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0) : 0),
                            acumulado4 = (x.Where(y => (y.corteID == corte4ID)).Count() > 0 ? x.Where(y => (y.corteID == corte4ID)).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0)
                                    + (corte4IDCplan != -1 ? (x.Where(y => (y.corteID == corte4IDCplan) && y.empresa == 1).Count() > 0 ? x.Where(y => (y.corteID == corte4IDCplan) && y.empresa == 1).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0) : 0),
                            acumulado5 = (x.Where(y => (y.corteID == corte5ID)).Count() > 0 ? x.Where(y => (y.corteID == corte5ID)).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0)
                                    + (corte5IDCplan != -1 ? (x.Where(y => (y.corteID == corte5IDCplan) && y.empresa == 1).Count() > 0 ? x.Where(y => (y.corteID == corte5IDCplan) && y.empresa == 1).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0) : 0),

                        }).ToList();

                    auxDetalles.AddRange(auxDetallesArrendadora);

                    var auxDetallesEspeciales = _dbArrendadora.tblM_KBCorteDet.Where(x => (x.cuenta.Contains("4000-5-") || x.cuenta.Contains("4000-6-") || x.areaCuenta == "6-5" || x.areaCuenta == "9-27") && (x.corteID == corte.id || cortesAnterioresID.Contains(x.corteID))
                        && x.fechapol >= new DateTime(anio, 1, 1) && !x.referencia.Contains("/" + (anio - 1).ToString())).ToList();

                    foreach (var item in auxDetallesEspeciales)
                    {
                        if (item.cuenta.Contains("4000-6-")) item.areaCuenta = "6-5";
                        else if (item.cuenta.Contains("4000-5-")) item.areaCuenta = "9-27";
                    }

                    var auxDetallesGroup = auxDetallesEspeciales.GroupBy(x => x.areaCuenta)
                        .Select(x => new ReporteSemanalKubrixDTO
                        {
                            cc = "",
                            areaCuenta = x.Key,
                            nombre = "",

                            acumulado1 = (x.Where(y => (y.corteID == corte.id)).Count() > 0 ? x.Where(y => (y.corteID == corte.id)).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0)
                                    + (corteAnteriorID != -1 ? (x.Where(y => (y.corteID == corteAnteriorID) && y.empresa == 1).Count() > 0 ? x.Where(y => (y.corteID == corteAnteriorID) && y.empresa == 1).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0) : 0),
                            acumulado2 = (x.Where(y => (y.corteID == corte2ID)).Count() > 0 ? x.Where(y => (y.corteID == corte2ID)).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0)
                                    + (corte2IDCplan != -1 ? (x.Where(y => (y.corteID == corte2IDCplan) && y.empresa == 1).Count() > 0 ? x.Where(y => (y.corteID == corte2IDCplan) && y.empresa == 1).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0) : 0),
                            acumulado3 = (x.Where(y => (y.corteID == corte3ID)).Count() > 0 ? x.Where(y => (y.corteID == corte3ID)).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0)
                                    + (corte3IDCplan != -1 ? (x.Where(y => (y.corteID == corte3IDCplan) && y.empresa == 1).Count() > 0 ? x.Where(y => (y.corteID == corte3IDCplan) && y.empresa == 1).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0) : 0),
                            acumulado4 = (x.Where(y => (y.corteID == corte4ID)).Count() > 0 ? x.Where(y => (y.corteID == corte4ID)).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0)
                                    + (corte4IDCplan != -1 ? (x.Where(y => (y.corteID == corte4IDCplan) && y.empresa == 1).Count() > 0 ? x.Where(y => (y.corteID == corte4IDCplan) && y.empresa == 1).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0) : 0),
                            acumulado5 = (x.Where(y => (y.corteID == corte5ID)).Count() > 0 ? x.Where(y => (y.corteID == corte5ID)).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0)
                                    + (corte5IDCplan != -1 ? (x.Where(y => (y.corteID == corte5IDCplan) && y.empresa == 1).Count() > 0 ? x.Where(y => (y.corteID == corte5IDCplan) && y.empresa == 1).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0) : 0),
                            acumulado6 = (x.Where(y => (y.corteID == corte6ID)).Count() > 0 ? x.Where(y => (y.corteID == corte6ID)).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0)
                            + (corte6IDCplan != -1 ? (x.Where(y => (y.corteID == corte6IDCplan) && y.empresa == 1).Count() > 0 ? x.Where(y => (y.corteID == corte6IDCplan) && y.empresa == 1).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))) : 0) : 0),

                        }).ToList();

                    auxDetalles.AddRange(auxDetallesGroup.ToList());

                    lst = auxDetalles.OrderBy(x => x.cc).GroupBy(x => x.areaCuenta).Select(x =>
                    {
                        var AC = _dbArrendadora.tblP_CC.FirstOrDefault(y => y.areaCuenta == x.Key);
                        if (AC == null) AC = _dbArrendadora.tblP_CC.FirstOrDefault(y => y.cc == x.Key);
                        var ccEnkontrolActual = ccEnkontrol.FirstOrDefault(y => y.CC == (x.Key.Contains("-") ? "" : x.Key));
                        return new ReporteSemanalKubrixDTO
                        {
                            cc = AC == null ? (x.Key.Contains("-") ? "" : x.Key) : AC.cc,
                            areaCuenta = x.Key.Contains("-") ? x.Key : "",
                            nombre = AC == null ? (ccEnkontrolActual == null ? "" : ccEnkontrolActual.Descripcion) : AC.descripcion,
                            semana1 = x.Sum(y => y.acumulado1) - x.Sum(y => y.acumulado2),
                            semana2 = x.Sum(y => y.acumulado2) - x.Sum(y => y.acumulado3),
                            semana3 = x.Sum(y => y.acumulado3) - x.Sum(y => y.acumulado4),
                            semana4 = x.Sum(y => y.acumulado4) - x.Sum(y => y.acumulado5),
                            semana5 = x.Sum(y => y.acumulado5) - x.Sum(y => y.acumulado6),
                            acumulado1 = x.Sum(y => y.acumulado1),
                            acumulado2 = x.Sum(y => y.acumulado2),
                            acumulado3 = x.Sum(y => y.acumulado3),
                            acumulado4 = x.Sum(y => y.acumulado4),
                            acumulado5 = x.Sum(y => y.acumulado5),
                        };
                    }).OrderBy(x => x.cc).ToList();
                }

                using (ExcelPackage excel = new ExcelPackage())
                {
                    var hoja1 = excel.Workbook.Worksheets.Add("Semanal");

                    List<string[]> headerRow = new List<string[]>() { new string[] { 
                        "CC", "Area Cuenta", "Descripción", 
                        "Semana " + ((char)10).ToString() + corte.fechaCorte.ToString("dd/MM/yyy"), "Acumulado " + ((char)10).ToString() + corte.fechaCorte.ToString("dd/MM/yyy"), 
                        "Semana " + ((char)10).ToString() + cortesAnteriores[0].fechaCorte.ToString("dd/MM/yyy"), "Acumulado " + ((char)10).ToString() + cortesAnteriores[0].fechaCorte.ToString("dd/MM/yyy"), 
                        "Semana " + ((char)10).ToString() + cortesAnteriores[1].fechaCorte.ToString("dd/MM/yyy"), "Acumulado " + ((char)10).ToString() + cortesAnteriores[1].fechaCorte.ToString("dd/MM/yyy"), 
                        "Semana " + ((char)10).ToString() + cortesAnteriores[2].fechaCorte.ToString("dd/MM/yyy"), "Acumulado " + ((char)10).ToString() + cortesAnteriores[2].fechaCorte.ToString("dd/MM/yyy"), 
                        "Semana " + ((char)10).ToString() + cortesAnteriores[3].fechaCorte.ToString("dd/MM/yyy"), "Acumulado " + ((char)10).ToString() + cortesAnteriores[3].fechaCorte.ToString("dd/MM/yyy")
                    } };
                    string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

                    hoja1.Cells[headerRange].LoadFromArrays(headerRow);
                    hoja1.Cells[headerRange].Style.WrapText = true;

                    var cellData = new List<object[]>();

                    foreach (var linea in lst)
                    {
                        cellData.Add(new object[]{
                        linea.cc,
                        linea.areaCuenta,
                        linea.nombre,
                        linea.semana1,
                        linea.acumulado1,
                        linea.semana2,
                        linea.acumulado2,
                        linea.semana3,
                        linea.acumulado3,
                        linea.semana4,
                        linea.acumulado4,
                        linea.semana5,
                        linea.acumulado5
                        });
                    }

                    hoja1.Cells[2, 1].LoadFromArrays(cellData);
                    excel.Compression = CompressionLevel.BestSpeed;
                    hoja1.Cells[hoja1.Dimension.Address].AutoFitColumns(10, 50);

                    ExcelRange range = hoja1.Cells[1, 1, hoja1.Dimension.End.Row, hoja1.Dimension.End.Column];
                    ExcelTable tab = hoja1.Tables.Add(range, "Table1");
                    tab.TableStyle = TableStyles.Medium2;
                    hoja1.Row(1).CustomHeight = false;

                    byte[] binaryData = null;

                    using (var exportData = new MemoryStream())
                    {
                        excel.SaveAs(exportData);
                        binaryData = exportData.ToArray();
                    }

                    return binaryData;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public byte[] ExcelPorSubcuentaKubrix(int corteID)
        {
            try
            {
                var ccEnkontrol = getCCEnkontrol();
                var corte = _context.tblM_KBCorte.FirstOrDefault(x => x.id == corteID);
                var corteAnterior = _context.tblM_KBCorte.Where(x => x.fechaCorte < corte.fechaCorte && x.tipo == corte.tipo).OrderByDescending(x => x.fechaCorte).FirstOrDefault();
                tblM_KBCorte corteCplan = new tblM_KBCorte();
                var corteCplanID = -1;
                tblM_KBCorte corteAnteriorCplan = new tblM_KBCorte(); ;
                var corteAnteriorCplanID = -1;

                var anio = corte.fechaCorte.Year;

                var auxDetalles = _context.tblM_KBCorteDet.Where(x => (x.corteID == corte.id || x.corteID == corteAnterior.id)
                    && x.fechapol >= new DateTime(anio, 1, 1) && !x.referencia.Contains("/" + (anio - 1).ToString())).ToList();

                if (vSesiones.sesionEmpresaActual == 2)
                {
                    using (var _db = new MainContext((int)EmpresaEnum.Construplan))
                    {
                        corteCplan = _db.tblM_KBCorte.FirstOrDefault(x => x.fechaCorte == corte.fechaCorte && x.tipo == corte.tipo);

                        if (corteCplan != null)
                        {
                            corteAnteriorCplan = _db.tblM_KBCorte.Where(x => x.fechaCorte < corteCplan.fechaCorte && x.tipo == corteCplan.tipo).OrderByDescending(x => x.fechaCorte).FirstOrDefault();
                            corteCplanID = corteCplan.id;
                            List<tblM_KBCorteDet> auxDetallesCplan = new List<tblM_KBCorteDet>();
                            if (corteAnteriorCplan == null) auxDetallesCplan = _db.tblM_KBCorteDet.Where(x => (x.corteID == corteCplan.id) && x.fechapol >= new DateTime(anio, 1, 1) && (x.cuenta.Contains("5000-10-") || x.cuenta.Contains("5280-10-") || x.cuenta.Contains("5900-3-"))).ToList();
                            else
                            {
                                corteAnteriorCplanID = corteAnteriorCplan.id;
                                auxDetallesCplan = _db.tblM_KBCorteDet.Where(x => (x.corteID == corteCplan.id || x.corteID == corteAnteriorCplan.id) && x.fechapol >= new DateTime(anio, 1, 1) && (x.cuenta.Contains("5000-10-") || x.cuenta.Contains("5280-10-") || x.cuenta.Contains("5900-3-"))).ToList();
                            }
                            auxDetalles.AddRange(auxDetallesCplan);
                        }
                    }
                }


                foreach (var item in auxDetalles)
                {
                    if (item.cuenta.Contains("4000-6-")) item.areaCuenta = "6-5";
                    else { if (item.cuenta.Contains("4000-5-")) item.areaCuenta = "9-27"; }
                }

                var lst = auxDetalles.GroupBy(x => new { x.areaCuenta }).ToList();

                var cuentasCostos = _context.tblM_KBCatCuenta.Where(x => (x.cuenta.Contains("5000-") || x.cuenta.Contains("5280") || x.cuenta.Contains("5900") || x.cuenta.Contains("5901")) && x.cuenta.Contains("-0")).ToList();

                using (ExcelPackage excel = new ExcelPackage())
                {
                    var hoja1 = excel.Workbook.Worksheets.Add("Por Subcuenta");
                    List<string> auxHeaders = new List<string>(); ;
                    auxHeaders.Add("CC");
                    auxHeaders.Add("AREA CUENTA");
                    auxHeaders.Add("DESCRIPCIÓN");

                    foreach (var auxCuenta in cuentasCostos)
                    {
                        if (auxCuenta.cuenta != "5000-0-0")
                        {
                            auxHeaders.Add(auxCuenta.cuenta + ((char)10).ToString() + auxCuenta.descripcion + ((char)10).ToString() + "SEMANA");
                            auxHeaders.Add(auxCuenta.cuenta + ((char)10).ToString() + auxCuenta.descripcion + ((char)10).ToString() + "ACUMULADO");
                        }
                    }
                    auxHeaders.Add("COSTO ESTIMADO" + ((char)10).ToString() + "SEMANA");
                    auxHeaders.Add("COSTO ESTIMADO" + ((char)10).ToString() + "ACUMULADO");
                    auxHeaders.Add("4900-1-0" + ((char)10).ToString() + "GANANCIA CAMBIARIA" + ((char)10).ToString() + "SEMANA");
                    auxHeaders.Add("4900-1-0" + ((char)10).ToString() + "GANANCIA CAMBIARIA" + ((char)10).ToString() + "ACUMULADO");

                    auxHeaders.Add("INGRESOS" + ((char)10).ToString() + "SEMANA");
                    auxHeaders.Add("INGRESOS" + ((char)10).ToString() + "ACUMULADO");
                    auxHeaders.Add("4901-1-0" + ((char)10).ToString() + "INGRESO POR VENTA ACTIVO FIJO" + ((char)10).ToString() + "SEMANA");
                    auxHeaders.Add("4901-1-0" + ((char)10).ToString() + "INGRESO POR VENTA ACTIVO FIJO" + ((char)10).ToString() + "ACUMULADO");
                    auxHeaders.Add("4901-2-0" + ((char)10).ToString() + "INGRESO OCASIONAL" + ((char)10).ToString() + "SEMANA");
                    auxHeaders.Add("4901-2-0" + ((char)10).ToString() + "INGRESO OCASIONAL" + ((char)10).ToString() + "ACUMULADO");

                    List<string[]> headerRow = new List<string[]>() { auxHeaders.ToArray() };

                    //foreach (var auxCuenta in cuentasCostos) { headerRow[0] = headerRow[0].Concat(new string[] { auxCuenta.cuenta + ((char)10).ToString() + auxCuenta.descripcion }); }

                    string headerRange = "A1:CA1";
                    hoja1.Cells[headerRange].LoadFromArrays(headerRow);
                    hoja1.Cells[headerRange].Style.WrapText = true;

                    var cellData = new List<object[]>();
                    foreach (var linea in lst)
                    {
                        List<object> auxCell = new List<object>();
                        var AC = _context.tblP_CC.FirstOrDefault(y => y.areaCuenta == linea.Key.areaCuenta);
                        if (AC == null) AC = _context.tblP_CC.FirstOrDefault(y => y.cc == linea.Key.areaCuenta);
                        var ccEnkontrolActual = ccEnkontrol.FirstOrDefault(y => y.CC == (linea.Key.areaCuenta.Contains("-") ? "" : linea.Key.areaCuenta));
                        auxCell.Add(AC == null ? (linea.Key.areaCuenta.Contains("-") ? "" : linea.Key.areaCuenta) : AC.cc);
                        auxCell.Add(linea.Key.areaCuenta.Contains("-") ? linea.Key.areaCuenta : "");
                        auxCell.Add(AC == null ? (ccEnkontrolActual == null ? "" : ccEnkontrolActual.Descripcion) : AC.descripcion);
                        foreach (var auxCuenta in cuentasCostos)
                        {
                            string ac = "";
                            int idx = auxCuenta.cuenta.LastIndexOf('-');
                            if (idx != -1) { ac = auxCuenta.cuenta.Substring(0, idx + 1); }
                            if (ac != "5000-0-")
                            {
                                auxCell.Add(
                                    linea.Where(y => y.cuenta.Contains(ac) && (y.corteID == corte.id || y.corteID == corteCplanID)).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (-1) : (1)))
                                    - linea.Where(y => y.cuenta.Contains(ac) && (y.corteID == corteAnterior.id || y.corteID == corteAnteriorCplanID)).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (-1) : (1)))
                                );
                                auxCell.Add(linea.Where(y => y.cuenta.Contains(ac) && (y.corteID == corte.id || y.corteID == corteCplanID)).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (-1) : (1))));
                            }
                        }
                        auxCell.Add(
                            linea.Where(y => y.cuenta == "1-4-0" && y.corteID == corte.id).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (-1) : (1)))
                            - linea.Where(y => y.cuenta == "1-4-0" && y.corteID == corteAnterior.id).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (-1) : (1)))
                        );
                        auxCell.Add(linea.Where(y => y.cuenta == "1-4-0" && y.corteID == corte.id).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (-1) : (1))));

                        auxCell.Add(
                            linea.Where(y => y.cuenta.Contains("4900-1-") && y.corteID == corte.id).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1)))
                            - linea.Where(y => y.cuenta.Contains("4900-1-") && y.corteID == corteAnterior.id).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1)))
                        );
                        auxCell.Add(linea.Where(y => y.cuenta.Contains("4900-1-") && y.corteID == corte.id).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))));

                        auxCell.Add(
                            linea.Where(y => (y.cuenta.Contains("4000-") || y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2") && y.corteID == corte.id).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1)))
                            - linea.Where(y => (y.cuenta.Contains("4000-") || y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2") && y.corteID == corteAnterior.id).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1)))
                        );
                        auxCell.Add(linea.Where(y => (y.cuenta.Contains("4000-") || y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2") && y.corteID == corte.id).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))));

                        auxCell.Add(
                            linea.Where(y => y.cuenta.Contains("4901-1-") && y.corteID == corte.id).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1)))
                            - linea.Where(y => y.cuenta.Contains("4901-1-") && y.corteID == corteAnterior.id).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1)))
                        );
                        auxCell.Add(linea.Where(y => y.cuenta.Contains("4901-1-") && y.corteID == corte.id).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))));

                        auxCell.Add(
                            linea.Where(y => y.cuenta.Contains("4901-2-") && y.corteID == corte.id).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1)))
                            - linea.Where(y => y.cuenta.Contains("4901-2-") && y.corteID == corteAnterior.id).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1)))
                        );
                        auxCell.Add(linea.Where(y => y.cuenta.Contains("4901-2-") && y.corteID == corte.id).Sum(y => y.monto * (y.cuenta == "1-1-0" || y.cuenta == "1-2-1" || y.cuenta == "1-2-2" || y.cuenta == "1-2-3" || y.cuenta == "1-3-1" || y.cuenta == "1-3-2" ? (1) : (-1))));


                        cellData.Add(auxCell.ToArray());
                    }
                    hoja1.Cells[2, 1].LoadFromArrays(cellData);
                    excel.Compression = CompressionLevel.BestSpeed;
                    hoja1.Cells[hoja1.Dimension.Address].AutoFitColumns(10, 50);

                    ExcelRange range = hoja1.Cells[1, 1, hoja1.Dimension.End.Row, hoja1.Dimension.End.Column];
                    ExcelTable tab = hoja1.Tables.Add(range, "Table1");
                    tab.TableStyle = TableStyles.Medium2;
                    hoja1.Row(1).CustomHeight = false;

                    var s = tab.Address.Start;
                    var e = tab.Address.End;

                    hoja1.Cells[s.Row + 1, s.Column, e.Row, e.Column].Sort(new[] { 0, 1 });

                    byte[] binaryData = null;

                    using (var exportData = new MemoryStream())
                    {
                        excel.SaveAs(exportData);
                        binaryData = exportData.ToArray();
                    }

                    return binaryData;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private List<CentroCostoDTO> getCCEnkontrol()
        {
            OdbcConsultaDTO odbcCCs = new OdbcConsultaDTO();
            odbcCCs.consulta = "SELECT cc, descripcion, corto FROM cc ORDER BY cc";
            List<CentroCostoDTO> CCs = _contextEnkontrol.Select<CentroCostoDTO>(EnkontrolEnum.CplanProd, odbcCCs);

            return CCs;
        }

        public List<DetallesSemanalDTO> GetMovimientosNuevos(int corteID, List<string> areasCuenta)
        {
            List<DetallesSemanalDTO> data = new List<DetallesSemanalDTO>();
            try
            {
                tblM_KBCorte corte = _context.tblM_KBCorte.FirstOrDefault(x => x.id == corteID);
                tblM_KBCorte corteAnterior = new tblM_KBCorte();
                if (corte.tipo == 1) corteAnterior = _context.tblM_KBCorte.Where(x => x.fechaCorte < corte.fechaCorte && x.tipo == 1).OrderByDescending(x => x.fechaCorte).FirstOrDefault();
                else corteAnterior = _context.tblM_KBCorte.Where(x => x.fechaCorte < corte.fechaCorte && x.tipo == 0).OrderByDescending(x => x.fechaCorte).FirstOrDefault();

                tblM_KBCorte corteCplan = new tblM_KBCorte();
                tblM_KBCorte corteAnteriorCplan = new tblM_KBCorte();

                List<DetallesKubrixPorCtaDTO> detallesEntrantes = new List<DetallesKubrixPorCtaDTO>();
                List<DetallesKubrixPorCtaDTO> detallesEliminados = new List<DetallesKubrixPorCtaDTO>();
                List<DetallesKubrixPorCtaDTO> detallesEntrantesCplan = new List<DetallesKubrixPorCtaDTO>();
                List<DetallesKubrixPorCtaDTO> detallesEliminadosCplan = new List<DetallesKubrixPorCtaDTO>();

                var auxDetalles = _context.tblM_KBCorteDet.Where(x => x.corteID == corte.id && areasCuenta.Contains(x.areaCuenta) && (x.cuenta.Contains("5000-") || x.cuenta.Contains("5900-") || x.cuenta.Contains("5901-") || /*x.cuenta.Contains("5280-") ||*/ x.cuenta.Contains("4900-1-") || x.cuenta.Contains("4000-4-")));
                var auxDetallesAnterior = _context.tblM_KBCorteDet.Where(x => x.corteID == corteAnterior.id && areasCuenta.Contains(x.areaCuenta) && (x.cuenta.Contains("5000-") || x.cuenta.Contains("5900-") || x.cuenta.Contains("5901-") || /*x.cuenta.Contains("5280-") ||*/ x.cuenta.Contains("4900-1-") || x.cuenta.Contains("4000-4-")));

                //var auxDetalles = _context.tblM_KBCorteDet.Where(x => x.corteID == corte.id && areasCuenta.Contains(x.areaCuenta) && x.cuenta.Contains("4000-1-"));
                //var auxDetallesAnterior = _context.tblM_KBCorteDet.Where(x => x.corteID == corteAnterior.id && areasCuenta.Contains(x.areaCuenta) && x.cuenta.Contains("4000-1-"));
                var cuentasDesc = getCuentasDesc();

                detallesEntrantes =
                    (from actual in auxDetalles
                     join anterior in auxDetallesAnterior
                     on
                         new
                         {
                             areaCuenta = actual.areaCuenta,
                             cc = actual.cc,
                             concepto = actual.concepto,
                             cuenta = actual.cuenta,
                             empresa = actual.empresa,
                             fechapol = actual.fechapol,
                             linea = actual.linea,
                             monto = actual.monto,
                             poliza = actual.referencia,
                             tipoEquipo = actual.tipoEquipo
                         }
                     equals
                         new
                         {
                             areaCuenta = anterior.areaCuenta,
                             cc = anterior.cc,
                             concepto = anterior.concepto,
                             cuenta = anterior.cuenta,
                             empresa = anterior.empresa,
                             fechapol = anterior.fechapol,
                             linea = anterior.linea,
                             monto = anterior.monto,
                             poliza = anterior.referencia,
                             tipoEquipo = anterior.tipoEquipo
                         }
                     into mezcla
                     from anterior in mezcla.DefaultIfEmpty()
                     where anterior == null
                     select actual).ToList()
                     .Select(x =>
                    {
                        bool banderaCtaDiesel = false;
                        if (x.cuenta.Contains("4000-4-")) banderaCtaDiesel = true;
                        return new DetallesKubrixPorCtaDTO
                            {
                                cta = banderaCtaDiesel ? "5000" : x.cuenta.Split('-').FirstOrDefault(),
                                scta = banderaCtaDiesel ? "4" : x.cuenta.Split('-').Skip(1).FirstOrDefault(),
                                sscta = banderaCtaDiesel ? "2" : x.cuenta.Split('-').Skip(1).LastOrDefault(),
                                descripcion = cuentasDesc.FirstOrDefault(y => y.Value == x.cuenta).Text,
                                anio = x.poliza.Split('-').FirstOrDefault(),
                                mes = x.poliza.Split('-').Skip(1).FirstOrDefault(),
                                poliza = x.poliza.Split('-').LastOrDefault(),
                                tp = x.poliza.Split('-').Skip(2).FirstOrDefault(),
                                fecha = x.fechapol.ToString("dd/MMM/yyyy"),
                                cc = x.areaCuenta,
                                referencia = x.referencia,
                                concepto = x.concepto,
                                cargos = x.monto,
                                abonos = 0,
                                saldo = x.monto,
                            };
                    }).ToList();

                detallesEliminados =
                    (from actual in auxDetallesAnterior
                     join anterior in auxDetalles
                     on
                         new
                         {
                             areaCuenta = actual.areaCuenta,
                             cc = actual.cc,
                             concepto = actual.concepto,
                             cuenta = actual.cuenta,
                             empresa = actual.empresa,
                             fechapol = actual.fechapol,
                             linea = actual.linea,
                             monto = actual.monto,
                             poliza = actual.referencia,
                             tipoEquipo = actual.tipoEquipo
                         }
                     equals
                         new
                         {
                             areaCuenta = anterior.areaCuenta,
                             cc = anterior.cc,
                             concepto = anterior.concepto,
                             cuenta = anterior.cuenta,
                             empresa = anterior.empresa,
                             fechapol = anterior.fechapol,
                             linea = anterior.linea,
                             monto = anterior.monto,
                             poliza = anterior.referencia,
                             tipoEquipo = anterior.tipoEquipo
                         }
                     into mezcla
                     from anterior in mezcla.DefaultIfEmpty()
                     where anterior == null
                     select actual).ToList()
                     .Select(x =>
                    {
                        bool banderaCtaDiesel = false;
                        if (x.cuenta.Contains("4000-4-")) banderaCtaDiesel = true;
                        return new DetallesKubrixPorCtaDTO
                            {
                                cta = banderaCtaDiesel ? "5000" : x.cuenta.Split('-').FirstOrDefault(),
                                scta = banderaCtaDiesel ? "4" : x.cuenta.Split('-').Skip(1).FirstOrDefault(),
                                sscta = banderaCtaDiesel ? "2" : x.cuenta.Split('-').Skip(1).LastOrDefault(),
                                descripcion = cuentasDesc.FirstOrDefault(y => y.Value == x.cuenta).Text,
                                anio = x.poliza.Split('-').FirstOrDefault(),
                                mes = x.poliza.Split('-').Skip(1).FirstOrDefault(),
                                poliza = x.poliza.Split('-').LastOrDefault(),
                                tp = x.poliza.Split('-').Skip(2).FirstOrDefault(),
                                fecha = x.fechapol.ToString("dd/MMM/yyyy"),
                                cc = x.areaCuenta,
                                referencia = x.referencia,
                                concepto = x.concepto,
                                cargos = x.monto,
                                abonos = 0,
                                saldo = x.monto,
                            };
                    }).ToList();

                if (vSesiones.sesionEmpresaActual == 2)
                {
                    using (var _db = new MainContext((int)EmpresaEnum.Construplan))
                    {
                        corteCplan = _db.tblM_KBCorte.FirstOrDefault(x => x.fechaCorte == corte.fechaCorte && x.tipo == corte.tipo);

                        if (corteCplan != null)
                        {
                            corteAnteriorCplan = _db.tblM_KBCorte.Where(x => x.fechaCorte < corteCplan.fechaCorte && x.tipo == corteCplan.tipo).OrderByDescending(x => x.fechaCorte).FirstOrDefault();

                            var auxDetallesCplan = _context.tblM_KBCorteDet.Where(x => x.corteID == corteCplan.id && areasCuenta.Contains(x.areaCuenta) && (x.cuenta.Contains("5000-") || x.cuenta.Contains("5900-") || x.cuenta.Contains("5901-") || /*x.cuenta.Contains("5280-") ||*/ x.cuenta.Contains("4900-1-") || x.cuenta.Contains("4000-4-")));
                            var auxDetallesAnteriorCplan = _context.tblM_KBCorteDet.Where(x => x.corteID == corteAnteriorCplan.id && areasCuenta.Contains(x.areaCuenta) && (x.cuenta.Contains("5000-") || x.cuenta.Contains("5900-") || x.cuenta.Contains("5901-") || /*x.cuenta.Contains("5280-") ||*/ x.cuenta.Contains("4900-1-") || x.cuenta.Contains("4000-4-")));

                            detallesEntrantesCplan =
                                (from actual in auxDetallesCplan
                                 join anterior in auxDetallesAnteriorCplan
                                on
                                    new
                                    {
                                        areaCuenta = actual.areaCuenta,
                                        cc = actual.cc,
                                        concepto = actual.concepto,
                                        cuenta = actual.cuenta,
                                        empresa = actual.empresa,
                                        fechapol = actual.fechapol,
                                        linea = actual.linea,
                                        monto = actual.monto,
                                        poliza = actual.referencia,
                                        tipoEquipo = actual.tipoEquipo
                                    }
                                equals
                                    new
                                    {
                                        areaCuenta = anterior.areaCuenta,
                                        cc = anterior.cc,
                                        concepto = anterior.concepto,
                                        cuenta = anterior.cuenta,
                                        empresa = anterior.empresa,
                                        fechapol = anterior.fechapol,
                                        linea = anterior.linea,
                                        monto = anterior.monto,
                                        poliza = anterior.referencia,
                                        tipoEquipo = anterior.tipoEquipo
                                    }
                                into mezcla
                                 from anterior in mezcla.DefaultIfEmpty()
                                 where anterior == null
                                 select actual).ToList()
                                .Select(x =>
                                {
                                    bool banderaCtaDiesel = false;
                                    if (x.cuenta.Contains("4000-4-")) banderaCtaDiesel = true;
                                    return new DetallesKubrixPorCtaDTO
                                        {
                                            cta = banderaCtaDiesel ? "5000" : x.cuenta.Split('-').FirstOrDefault(),
                                            scta = banderaCtaDiesel ? "4" : x.cuenta.Split('-').Skip(1).FirstOrDefault(),
                                            sscta = banderaCtaDiesel ? "2" : x.cuenta.Split('-').Skip(1).LastOrDefault(),
                                            descripcion = cuentasDesc.FirstOrDefault(y => y.Value == x.cuenta).Text,
                                            anio = x.poliza.Split('-').FirstOrDefault(),
                                            mes = x.poliza.Split('-').Skip(1).FirstOrDefault(),
                                            poliza = x.poliza.Split('-').LastOrDefault(),
                                            tp = x.poliza.Split('-').Skip(2).FirstOrDefault(),
                                            fecha = x.fechapol.ToString("dd/MMM/yyyy"),
                                            cc = x.areaCuenta,
                                            referencia = x.referencia,
                                            concepto = x.concepto,
                                            cargos = x.monto,
                                            abonos = 0,
                                            saldo = x.monto,
                                        };
                                }).ToList();

                            detallesEliminadosCplan =
                                (from actual in auxDetallesAnteriorCplan
                                 join anterior in auxDetallesCplan
                                on
                                    new
                                    {
                                        areaCuenta = actual.areaCuenta,
                                        cc = actual.cc,
                                        concepto = actual.concepto,
                                        cuenta = actual.cuenta,
                                        empresa = actual.empresa,
                                        fechapol = actual.fechapol,
                                        linea = actual.linea,
                                        monto = actual.monto,
                                        poliza = actual.referencia,
                                        tipoEquipo = actual.tipoEquipo
                                    }
                                equals
                                    new
                                    {
                                        areaCuenta = anterior.areaCuenta,
                                        cc = anterior.cc,
                                        concepto = anterior.concepto,
                                        cuenta = anterior.cuenta,
                                        empresa = anterior.empresa,
                                        fechapol = anterior.fechapol,
                                        linea = anterior.linea,
                                        monto = anterior.monto,
                                        poliza = anterior.referencia,
                                        tipoEquipo = anterior.tipoEquipo
                                    }
                                into mezcla
                                 from anterior in mezcla.DefaultIfEmpty()
                                 where anterior == null
                                 select actual).ToList()
                                .Select(x =>
                                {
                                    bool banderaCtaDiesel = false;
                                    if (x.cuenta.Contains("4000-4-")) banderaCtaDiesel = true;
                                    return new DetallesKubrixPorCtaDTO
                                        {
                                            cta = banderaCtaDiesel ? "5000" : x.cuenta.Split('-').FirstOrDefault(),
                                            scta = banderaCtaDiesel ? "4" : x.cuenta.Split('-').Skip(1).FirstOrDefault(),
                                            sscta = banderaCtaDiesel ? "2" : x.cuenta.Split('-').Skip(1).LastOrDefault(),
                                            descripcion = cuentasDesc.FirstOrDefault(y => y.Value == x.cuenta).Text,
                                            anio = x.poliza.Split('-').FirstOrDefault(),
                                            mes = x.poliza.Split('-').Skip(1).FirstOrDefault(),
                                            poliza = x.poliza.Split('-').LastOrDefault(),
                                            tp = x.poliza.Split('-').Skip(2).FirstOrDefault(),
                                            fecha = x.fechapol.ToString("dd/MMM/yyyy"),
                                            cc = x.areaCuenta,
                                            referencia = x.referencia,
                                            concepto = x.concepto,
                                            cargos = x.monto,
                                            abonos = 0,
                                            saldo = x.monto,
                                        };
                                }).ToList();
                        }
                    }
                }
                foreach (var item in areasCuenta)
                {
                    DetallesSemanalDTO auxData = new DetallesSemanalDTO();
                    auxData.cc = item;
                    auxData.entrantes = detallesEntrantes.Where(x => x.cc == item).ToList();
                    auxData.eliminados = detallesEliminados.Where(x => x.cc == item).ToList();
                    auxData.entrantes.AddRange(detallesEntrantesCplan);
                    auxData.eliminados.AddRange(detallesEliminadosCplan);
                    data.Add(auxData);
                }
            }
            catch (Exception e)
            {
                return data;
            }
            return data;
        }

        public byte[] ExcelEntradasSemanas(DetallesSemanalDTO detalles)
        {
            try
            {
                var cc = _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == detalles.cc);
                List<DetallesKubrixPorCtaDTO> detallesFinal = detalles.eliminados;
                detallesFinal.ForEach(x => { x.concepto = "**REGISTRO ELIMINADO** " + x.concepto; x.cargos = x.cargos * (-1); x.abonos = x.abonos * (-1); x.saldo = x.saldo * (-1); });
                detallesFinal.AddRange(detalles.entrantes);

                List<DetallesKubrixPorCtaDTO> detallesFinalIntereses = detallesFinal.Where(x => (x.cta == "5900" && x.scta == "1") || (x.cta == "4900" && x.scta == "1")).ToList();
                detallesFinal = detallesFinal.Where(x => !(x.cta == "5900" && x.scta == "1") && !(x.cta == "4900" && x.scta == "1")).ToList();

                var cuentasDesc = getCuentasDesc();
                int renglon = 1;

                var ccDesc = cc == null ? "N/A" : cc.cc;
                using (ExcelPackage excel = new ExcelPackage())
                {
                    var hoja1 = excel.Workbook.Worksheets.Add(ccDesc);
                    List<object> auxCell = new List<object>();

                    hoja1.Cells.Style.Font.Name = "Arial";
                    hoja1.Cells.Style.Font.Size = 10;
                    hoja1.Column(1).Style.Font.Size = hoja1.Column(2).Style.Font.Size = hoja1.Column(3).Style.Font.Size = hoja1.Column(4).Style.Font.Size = 11;
                    hoja1.Column(1).Style.Font.Bold = hoja1.Column(2).Style.Font.Bold = hoja1.Column(3).Style.Font.Bold = hoja1.Column(4).Style.Font.Bold = true;

                    var auxHeaders = new List<string> { "Cta", "SCta", "SSCta", "Descripción", "Año", "Mes", "Póliza", "TP", "Fecha", "C.C.", "Referencia", "Concepto", "Monto" };
                    List<string[]> headerRow = new List<string[]>() { auxHeaders.ToArray() };
                    string headerRange = "A1:M1";
                    hoja1.Cells[headerRange].LoadFromArrays(headerRow);
                    renglon++;
                    hoja1.Cells[headerRange].Style.WrapText = true;
                    hoja1.Cells[headerRange].Style.Font.Size = 12;
                    hoja1.Cells[headerRange].Style.Font.Bold = true;
                    hoja1.Cells[headerRange].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[headerRange].Style.Fill.BackgroundColor.SetColor(1, 56, 103, 143);
                    hoja1.Cells[headerRange].Style.Font.Color.SetColor(System.Drawing.Color.White);
                    hoja1.Cells[headerRange].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Cells[headerRange].Style.Border.BorderAround(ExcelBorderStyle.Thick, System.Drawing.Color.FromArgb(1, 56, 103, 143));

                    ComboDTO cuentaDesc = new ComboDTO();

                    var grupoCta = detallesFinal.GroupBy(x => x.cta).OrderBy(x => x.Key.ParseInt()).ToList();
                    //var cellData = new List<object[]>();
                    foreach (var itemCta in grupoCta)
                    {
                        var renglonCta = renglon;
                        cuentaDesc = cuentasDesc.FirstOrDefault(x => x.Value == (itemCta.Key.ToString() + "-0-0"));
                        auxCell = new List<object> { itemCta.Key, 0, 0 };
                        if (cuentasDesc != null) auxCell.Add(cuentaDesc.Text);
                        hoja1.Cells[renglon, 1].LoadFromArrays(new List<object[]>() { auxCell.ToArray() });
                        hoja1.Cells[renglon, 1, renglon, 13].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja1.Cells[renglon, 1, renglon, 13].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.SteelBlue);
                        hoja1.Cells[renglon, 1, renglon, 13].Style.Font.Color.SetColor(System.Drawing.Color.White);
                        hoja1.Cells[renglon, 1, renglon, 13].Style.Border.BorderAround(ExcelBorderStyle.Thick, System.Drawing.Color.SteelBlue);
                        renglon++;
                        //cellData.Add(auxCell.ToArray());

                        var grupoSCta = itemCta.GroupBy(x => x.scta).OrderBy(x => x.Key.ParseInt()).ToList();
                        foreach (var itemScta in grupoSCta)
                        {
                            var renglonSCta = renglon;
                            cuentaDesc = cuentasDesc.FirstOrDefault(x => x.Value == (itemCta.Key.ToString() + "-" + itemScta.Key.ToString() + "-0"));
                            auxCell = new List<object> { itemCta.Key, itemScta.Key, 0 };
                            if (cuentasDesc != null) auxCell.Add(cuentaDesc.Text);
                            hoja1.Cells[renglon, 1].LoadFromArrays(new List<object[]>() { auxCell.ToArray() });
                            hoja1.Cells[renglon, 1, renglon, 13].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            hoja1.Cells[renglon, 1, renglon, 13].Style.Fill.BackgroundColor.SetColor(1, 140, 169, 207);
                            renglon++;

                            var grupoSScta = itemScta.GroupBy(x => x.sscta).OrderBy(x => x.Key.ParseInt()).ToList();
                            foreach (var itemSScta in grupoSScta)
                            {
                                if (itemSScta.Key != "0")
                                {
                                    cuentaDesc = cuentasDesc.FirstOrDefault(x => x.Value == (itemCta.Key.ToString() + "-" + itemScta.Key.ToString() + "-" + itemSScta.Key.ToString()));
                                    auxCell = new List<object> { itemCta.Key, itemScta.Key, itemSScta.Key };
                                    if (cuentasDesc != null) auxCell.Add(cuentaDesc.Text);
                                    hoja1.Cells[renglon, 1].LoadFromArrays(new List<object[]>() { auxCell.ToArray() });
                                    hoja1.Cells[renglon, 1, renglon, 13].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    hoja1.Cells[renglon, 1, renglon, 13].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightSteelBlue);
                                    renglon++;
                                }
                                var renglonSscta = renglon;
                                foreach (var itemFinal in itemSScta.OrderBy(x => x.anio).ThenBy(x => x.mes).ThenBy(x => x.poliza).ThenBy(x => x.tp))
                                {
                                    auxCell = new List<object> { "", "", "", "", itemFinal.anio, itemFinal.mes, itemFinal.poliza, itemFinal.tp, itemFinal.fecha, ccDesc, 
                                        itemFinal.referencia, itemFinal.concepto, itemFinal.cargos };
                                    hoja1.Cells[renglon, 1].LoadFromArrays(new List<object[]>() { auxCell.ToArray() });
                                    renglon++;
                                }
                                hoja1.Cells[renglonSscta, 1, renglon - 1, 13].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                hoja1.Cells[renglonSscta, 1, renglon - 1, 13].Style.Fill.BackgroundColor.SetColor(1, 212, 223, 237);
                                hoja1.Cells[renglonSscta, 1, renglon - 1, 13].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                //hoja1.Cells[renglon, 12, renglon, 13].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                //hoja1.Cells[renglon, 12, renglon, 13].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                                hoja1.Cells[renglon, 12, renglon, 13].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                auxCell = new List<object> { "", "", "", "", "", "", "", "", "", "", "", "TOTAL", itemSScta.Sum(x => x.cargos) };
                                hoja1.Cells[renglon, 1].LoadFromArrays(new List<object[]>() { auxCell.ToArray() });
                                hoja1.Row(renglon).Style.Font.Bold = true;
                                hoja1.Row(renglon).Style.Font.Size = 11;
                                hoja1.Cells[renglon, 12, renglon, 13].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                hoja1.Cells[renglon, 12, renglon, 13].Style.Fill.BackgroundColor.SetColor(1, 212, 223, 237);
                                renglon++;
                                hoja1.InsertRow(renglon, 1);
                                renglon++;
                            }
                            auxCell = new List<object> { "TOTAL SUBCUENTA", "", "", "", "", "", "", "", "", "", "", "", itemScta.Sum(x => x.cargos) };
                            hoja1.Cells[renglon, 1].LoadFromArrays(new List<object[]>() { auxCell.ToArray() });
                            hoja1.Cells[renglon, 1, renglon, 13].Style.Font.Bold = true;
                            hoja1.Cells[renglon, 1, renglon, 13].Style.Font.Size = 12;
                            hoja1.Cells[renglon, 1, renglon, 13].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            hoja1.Cells[renglon, 1, renglon, 13].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightSteelBlue);
                            renglon++;
                            hoja1.Cells[renglonSCta, 1, renglon - 1, 13].Style.Border.BorderAround(ExcelBorderStyle.Thick, System.Drawing.Color.SteelBlue);
                            hoja1.InsertRow(renglon, 1);
                            renglon++;
                        }
                        //hoja1.Cells[renglonCta, 1, renglon - 2, 13].Style.Border.BorderAround(ExcelBorderStyle.Thick, System.Drawing.Color.SteelBlue);
                    }
                    auxCell = new List<object> { "TOTAL COSTO OPERATIVO", "", "", "", "", "", "", "", "", "", "", "", detallesFinal.Sum(x => x.cargos) };
                    hoja1.Cells[renglon, 1].LoadFromArrays(new List<object[]>() { auxCell.ToArray() });
                    hoja1.Cells[renglon, 12, renglon, 13].Style.Font.Bold = true;
                    hoja1.Cells[renglon, 12, renglon, 13].Style.Font.Size = 12;
                    hoja1.Cells[renglon, 1, renglon, 13].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[renglon, 1, renglon, 13].Style.Fill.BackgroundColor.SetColor(1, 0, 54, 102);
                    hoja1.Cells[renglon, 1, renglon, 13].Style.Font.Color.SetColor(System.Drawing.Color.White);
                    renglon++;
                    hoja1.InsertRow(renglon, 2);
                    renglon++; renglon++;

                    var grupoCtaIntereses = detallesFinalIntereses.GroupBy(x => x.cta).OrderBy(x => x.Key.ParseInt()).ToList();
                    foreach (var itemCta in grupoCtaIntereses)
                    {
                        var renglonCta = renglon;
                        cuentaDesc = cuentasDesc.FirstOrDefault(x => x.Value == (itemCta.Key.ToString() + "-0-0"));
                        auxCell = new List<object> { itemCta.Key, 0, 0 };
                        if (cuentasDesc != null) auxCell.Add(cuentaDesc.Text);
                        hoja1.Cells[renglon, 1].LoadFromArrays(new List<object[]>() { auxCell.ToArray() });
                        hoja1.Cells[renglon, 1, renglon, 13].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja1.Cells[renglon, 1, renglon, 13].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.SteelBlue);
                        hoja1.Cells[renglon, 1, renglon, 13].Style.Font.Color.SetColor(System.Drawing.Color.White);
                        renglon++;

                        var grupoSCta = itemCta.GroupBy(x => x.scta).OrderBy(x => x.Key.ParseInt()).ToList();
                        foreach (var itemScta in grupoSCta)
                        {
                            var renglonSCta = renglon;
                            cuentaDesc = cuentasDesc.FirstOrDefault(x => x.Value == (itemCta.Key.ToString() + "-" + itemScta.Key.ToString() + "-0"));
                            auxCell = new List<object> { itemCta.Key, itemScta.Key, 0 };
                            if (cuentasDesc != null) auxCell.Add(cuentaDesc.Text);
                            hoja1.Cells[renglon, 1].LoadFromArrays(new List<object[]>() { auxCell.ToArray() });
                            hoja1.Cells[renglon, 1, renglon, 13].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            hoja1.Cells[renglon, 1, renglon, 13].Style.Fill.BackgroundColor.SetColor(1, 140, 169, 207);
                            renglon++;

                            var grupoSScta = itemScta.GroupBy(x => x.sscta).OrderBy(x => x.Key.ParseInt()).ToList();
                            foreach (var itemSScta in grupoSScta)
                            {
                                if (itemSScta.Key != "0")
                                {
                                    cuentaDesc = cuentasDesc.FirstOrDefault(x => x.Value == (itemCta.Key.ToString() + "-" + itemScta.Key.ToString() + "-" + itemSScta.Key.ToString()));
                                    auxCell = new List<object> { itemCta.Key, itemScta.Key, itemSScta.Key };
                                    if (cuentasDesc != null) auxCell.Add(cuentaDesc.Text);
                                    hoja1.Cells[renglon, 1].LoadFromArrays(new List<object[]>() { auxCell.ToArray() });
                                    hoja1.Cells[renglon, 1, renglon, 13].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    hoja1.Cells[renglon, 1, renglon, 13].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightSteelBlue);
                                    renglon++;
                                }
                                var renglonSscta = renglon;
                                foreach (var itemFinal in itemSScta.OrderBy(x => x.anio).ThenBy(x => x.mes).ThenBy(x => x.poliza).ThenBy(x => x.tp))
                                {
                                    auxCell = new List<object> { "", "", "", "", itemFinal.anio, itemFinal.mes, itemFinal.poliza, itemFinal.tp, itemFinal.fecha, ccDesc, 
                                        itemFinal.referencia, itemFinal.concepto, itemFinal.cargos };
                                    hoja1.Cells[renglon, 1].LoadFromArrays(new List<object[]>() { auxCell.ToArray() });
                                    renglon++;
                                }
                                hoja1.Cells[renglonSscta, 1, renglon - 1, 13].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                hoja1.Cells[renglonSscta, 1, renglon - 1, 13].Style.Fill.BackgroundColor.SetColor(1, 212, 223, 237);
                                hoja1.Cells[renglonSscta, 1, renglon - 1, 13].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                hoja1.Cells[renglon, 12, renglon, 13].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                auxCell = new List<object> { "", "", "", "", "", "", "", "", "", "", "", "TOTAL", itemSScta.Sum(x => x.cargos) };
                                hoja1.Cells[renglon, 1].LoadFromArrays(new List<object[]>() { auxCell.ToArray() });
                                hoja1.Cells[renglon, 12, renglon, 13].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                hoja1.Cells[renglon, 12, renglon, 13].Style.Fill.BackgroundColor.SetColor(1, 212, 223, 237);
                                hoja1.Row(renglon).Style.Font.Bold = true;
                                hoja1.Row(renglon).Style.Font.Size = 11;
                                renglon++;
                                hoja1.InsertRow(renglon, 1);
                                renglon++;
                            }
                            auxCell = new List<object> { "TOTAL SUBCUENTA", "", "", "", "", "", "", "", "", "", "", "", itemScta.Sum(x => x.cargos) };
                            hoja1.Cells[renglon, 1].LoadFromArrays(new List<object[]>() { auxCell.ToArray() });
                            hoja1.Cells[renglon, 1, renglon, 13].Style.Font.Bold = true;
                            hoja1.Cells[renglon, 1, renglon, 13].Style.Font.Size = 12;
                            hoja1.Cells[renglon, 1, renglon, 13].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            hoja1.Cells[renglon, 1, renglon, 13].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightSteelBlue);
                            renglon++;
                            hoja1.Cells[renglonSCta, 1, renglon - 1, 13].Style.Border.BorderAround(ExcelBorderStyle.Thick, System.Drawing.Color.SteelBlue);
                            hoja1.InsertRow(renglon, 1);
                            renglon++;
                        }
                        //hoja1.Cells[renglonCta, 1, renglon - 2, 13].Style.Border.BorderAround(ExcelBorderStyle.Thick, System.Drawing.Color.SteelBlue);
                    }
                    auxCell = new List<object> { "TOTAL EFECTO CAMBIARIO", "", "", "", "", "", "", "", "", "", "", "", detallesFinalIntereses.Sum(x => x.cargos) };
                    hoja1.Cells[renglon, 1].LoadFromArrays(new List<object[]>() { auxCell.ToArray() });
                    hoja1.Cells[renglon, 12, renglon, 13].Style.Font.Bold = true;
                    hoja1.Cells[renglon, 12, renglon, 13].Style.Font.Size = 12;
                    hoja1.Cells[renglon, 1, renglon, 13].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[renglon, 1, renglon, 13].Style.Fill.BackgroundColor.SetColor(1, 0, 54, 102);
                    hoja1.Cells[renglon, 1, renglon, 13].Style.Font.Color.SetColor(System.Drawing.Color.White);

                    hoja1.Column(1).Style.Numberformat.Format = hoja1.Column(2).Style.Numberformat.Format = hoja1.Column(3).Style.Numberformat.Format =
                        hoja1.Column(5).Style.Numberformat.Format = hoja1.Column(6).Style.Numberformat.Format = hoja1.Column(7).Style.Numberformat.Format = "0";
                    hoja1.Column(1).Style.HorizontalAlignment = hoja1.Column(2).Style.HorizontalAlignment = hoja1.Column(3).Style.HorizontalAlignment =
                        hoja1.Column(5).Style.HorizontalAlignment = hoja1.Column(6).Style.HorizontalAlignment = hoja1.Column(7).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    hoja1.Column(13).Style.Numberformat.Format = hoja1.Column(13).Style.Numberformat.Format = "$#,##0.00";
                    hoja1.Column(13).Style.HorizontalAlignment = hoja1.Column(13).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;


                    //hoja1.Cells[3, 1].LoadFromArrays(cellData);
                    excel.Compression = CompressionLevel.BestSpeed;
                    hoja1.Cells.AutoFitColumns();
                    hoja1.Cells[headerRange].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    byte[] binaryData = null;

                    using (var exportData = new MemoryStream())
                    {
                        excel.SaveAs(exportData);
                        binaryData = exportData.ToArray();
                    }

                    return binaryData;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        #endregion

        #region subirExcelEstimados

        public int CargarExcelEstimados(byte[] bin, int corteID)
        {
            try
            {
                var corte = _context.tblM_KBCorte.FirstOrDefault(x => x.id == corteID);
                if (!corte.costoEstCerrado)
                {
                    List<tblM_KBCorteDet> estimados = new List<tblM_KBCorteDet>();
                    var centrosCosto = _context.tblP_CC.Select(x => new
                    {
                        areaCuenta = x.areaCuenta,
                        cc = x.cc,
                        id = x.id,
                        descripcion = x.descripcion
                    }).ToList();

                    var corteAnterior = _context.tblM_KBCorte.Where(x => x.tipo == corte.tipo && x.fechaCorte < corte.fechaCorte).OrderByDescending(x => x.fechaCorte).FirstOrDefault();
                    List<tblM_KBCorteDet> estimadosGuardados = new List<tblM_KBCorteDet>();
                    List<tblM_KBCorteDet> estimadosCorte2019 = new List<tblM_KBCorteDet>();
                    if (corteAnterior != null)
                    {
                        estimadosGuardados = _context.tblM_KBCorteDet.Where(x => x.corteID == corteAnterior.id && (x.cuenta == "1-1-0" || x.cuenta == "1-2-3" || x.cuenta == "1-4-0")).ToList();
                        estimadosCorte2019 = _context.tblM_KBCorteDet.Where(x => x.corteID == 20 && (x.cuenta == "1-1-0" || x.cuenta == "1-2-3" || x.cuenta == "1-4-0")).ToList();

                    }
                    using (MemoryStream stream = new MemoryStream(bin))
                    using (ExcelPackage excelPackage = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[1];
                        for (int i = worksheet.Dimension.Start.Row + 5; i <= worksheet.Dimension.End.Row; i++)
                        {
                            if (worksheet.Cells[i, 1].Value != null)
                            {
                                string ccIndex = worksheet.Cells[i, 1].Value.ToString().Split('.')[0];
                                ccIndex = ccIndex.PadLeft(3, '0');
                                var cc = centrosCosto.FirstOrDefault(x => x.cc == ccIndex);
                                if (cc != null)
                                {
                                    var areaCuentaFinal = (cc.areaCuenta == "0" || cc.areaCuenta == "0-0") ? ccIndex : cc.areaCuenta;
                                    var EstimadosAnterior = estimadosGuardados.Where(x => x.areaCuenta == ((cc.areaCuenta == "0" || cc.areaCuenta == "0-0") ? ccIndex : cc.areaCuenta)).ToList();
                                    var Estimados2019 = estimadosCorte2019.Where(x => x.areaCuenta == ((cc.areaCuenta == "0" || cc.areaCuenta == "0-0") ? ccIndex : cc.areaCuenta)).ToList();
                                    var IngresosEstimacionAnterior = EstimadosAnterior.Where(x => x.cuenta == "1-1-0").Sum(x => x.monto);
                                    IngresosEstimacionAnterior += Estimados2019.Where(x => x.cuenta == "1-1-0").Sum(x => x.monto);

                                    if (worksheet.Cells[i, 4].Value != null)
                                    {
                                        decimal monto = 0;
                                        bool exitoParse = decimal.TryParse(worksheet.Cells[i, 4].Value.ToString(), out monto);
                                        tblM_KBCorteDet auxEstimados = new tblM_KBCorteDet();
                                        auxEstimados.areaCuenta = areaCuentaFinal;
                                        auxEstimados.cc = "N/A";
                                        auxEstimados.concepto = "INGRESOS ESTIMADOS";
                                        auxEstimados.corteID = corte.id;
                                        auxEstimados.cuenta = "1-1-0";
                                        auxEstimados.empresa = vSesiones.sesionEmpresaActual;
                                        auxEstimados.fechapol = corte.fechaCorte;
                                        auxEstimados.id = 0;
                                        auxEstimados.linea = 0;
                                        auxEstimados.monto = (exitoParse ? monto : 0) - IngresosEstimacionAnterior;
                                        auxEstimados.poliza = "--";
                                        auxEstimados.referencia = "CONCILIACIÓN " + cc.descripcion;
                                        auxEstimados.tipoEquipo = 7;
                                        estimados.Add(auxEstimados);
                                    }

                                    var IngresosGenerarAnterior = EstimadosAnterior.Where(x => x.cuenta == "1-2-3").Sum(x => x.monto);
                                    IngresosGenerarAnterior += Estimados2019.Where(x => x.cuenta == "1-2-3").Sum(x => x.monto);

                                    if (worksheet.Cells[i, 6].Value != null)
                                    {
                                        decimal monto = 0;
                                        bool exitoParse = decimal.TryParse(worksheet.Cells[i, 6].Value.ToString(), out monto);
                                        tblM_KBCorteDet auxEstimados = new tblM_KBCorteDet();
                                        auxEstimados.areaCuenta = areaCuentaFinal;
                                        auxEstimados.cc = "N/A";
                                        auxEstimados.concepto = "INGRESOS PENDIENTES POR GENERAR";
                                        auxEstimados.corteID = corte.id;
                                        auxEstimados.cuenta = "1-2-3";
                                        auxEstimados.empresa = vSesiones.sesionEmpresaActual;
                                        auxEstimados.fechapol = corte.fechaCorte;
                                        auxEstimados.id = 0;
                                        auxEstimados.linea = 0;
                                        auxEstimados.monto = (exitoParse ? monto : 0) - IngresosGenerarAnterior;
                                        auxEstimados.poliza = "--";
                                        auxEstimados.referencia = cc.descripcion;
                                        auxEstimados.tipoEquipo = 7;
                                        estimados.Add(auxEstimados);
                                    }

                                    var CostoEstimadoAnterior = EstimadosAnterior.Where(x => x.cuenta == "1-4-0").Sum(x => x.monto);
                                    CostoEstimadoAnterior += Estimados2019.Where(x => x.cuenta == "1-4-0").Sum(x => x.monto);

                                    if (worksheet.Cells[i, 16].Value != null)
                                    {
                                        decimal monto = 0;
                                        bool exitoParse = decimal.TryParse(worksheet.Cells[i, 16].Value.ToString(), out monto);
                                        tblM_KBCorteDet auxEstimados = new tblM_KBCorteDet();
                                        auxEstimados.areaCuenta = areaCuentaFinal;
                                        auxEstimados.cc = "N/A";
                                        auxEstimados.concepto = "COSTOS ESTIMADOS";
                                        auxEstimados.corteID = corte.id;
                                        auxEstimados.cuenta = "1-4-0";
                                        auxEstimados.empresa = vSesiones.sesionEmpresaActual;
                                        auxEstimados.fechapol = corte.fechaCorte;
                                        auxEstimados.id = 0;
                                        auxEstimados.linea = 0;
                                        auxEstimados.monto = (exitoParse ? monto : 0) - CostoEstimadoAnterior;
                                        auxEstimados.poliza = "--";
                                        auxEstimados.referencia = cc.descripcion;
                                        auxEstimados.tipoEquipo = 7;
                                        estimados.Add(auxEstimados);
                                    }
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    var cortesPosteriores = _context.tblM_KBCorte.Where(x => x.tipo == corte.tipo && x.fechaCorte > corte.fechaCorte).ToList();
                    _context.tblM_KBCorteDet.AddRange(estimados);
                    _context.SaveChanges();
                    foreach (var item in cortesPosteriores)
                    {
                        foreach (var detalles in estimados) detalles.corteID = item.id;
                        _context.tblM_KBCorteDet.AddRange(estimados);
                        _context.SaveChanges();
                    }
                    //var estimadosABorrar = _context.tblM_KBCorteDet.Where(x => x.corteID == corte.id && (x.cuenta == "1-1-0" || x.cuenta == "1-2-3" || x.cuenta == "1-4-0")).ToList();
                    //_context.tblM_KBCorteDet.RemoveRange(estimadosABorrar);
                    corte.costoEstCerrado = true;
                    _context.SaveChanges();
                    return 1;
                }
                else
                {
                    return 2;
                }
            }
            catch (Exception e)
            {
                return 0;
            }

        }

        public int CargarExcelEstimados(byte[] bin)
        {
            try
            {
                List<tblM_KBCorteDet> estimados = new List<tblM_KBCorteDet>();
                var centrosCosto = _context.tblP_CC.Select(x => new
                {
                    areaCuenta = x.areaCuenta,
                    cc = x.cc,
                    id = x.id,
                    descripcion = x.descripcion
                }).ToList();
                var corte = _context.tblM_KBCorte.FirstOrDefault(x => x.anio == 2018);
                using (MemoryStream stream = new MemoryStream(bin))
                using (ExcelPackage excelPackage = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[1];
                    for (int i = worksheet.Dimension.Start.Row + 6; i <= worksheet.Dimension.End.Row; i++)
                    {
                        if (worksheet.Cells[i, 2].Value != null)
                        {
                            string ccIndex = worksheet.Cells[i, 2].Value.ToString().Split('.')[0];
                            ccIndex = ccIndex.PadLeft(3, '0');
                            var cc = centrosCosto.FirstOrDefault(x => x.cc == ccIndex);
                            if (cc != null)
                            {
                                if (worksheet.Cells[i, 6].Value != null)
                                {
                                    decimal monto = 0;
                                    bool exitoParse = decimal.TryParse(worksheet.Cells[i, 6].Value.ToString(), out monto);
                                    tblM_KBCorteDet auxEstimados = new tblM_KBCorteDet();
                                    auxEstimados.areaCuenta = cc.areaCuenta;
                                    auxEstimados.cc = "N/A";
                                    auxEstimados.concepto = "INGRESOS ESTIMADOS";
                                    auxEstimados.corteID = corte.id;
                                    auxEstimados.cuenta = "1-1-0";
                                    auxEstimados.empresa = vSesiones.sesionEmpresaActual;
                                    auxEstimados.fechapol = new DateTime(2019, 12, 31);
                                    auxEstimados.id = 0;
                                    auxEstimados.linea = 0;
                                    auxEstimados.monto = exitoParse ? monto : 0;
                                    auxEstimados.poliza = "--";
                                    auxEstimados.referencia = "CONCILIACIÓN " + cc.descripcion;
                                    auxEstimados.tipoEquipo = 7;
                                    estimados.Add(auxEstimados);
                                }
                                if (worksheet.Cells[i, 8].Value != null)
                                {
                                    decimal monto = 0;
                                    bool exitoParse = decimal.TryParse(worksheet.Cells[i, 8].Value.ToString(), out monto);
                                    tblM_KBCorteDet auxEstimados = new tblM_KBCorteDet();
                                    auxEstimados.areaCuenta = cc.areaCuenta;
                                    auxEstimados.cc = "N/A";
                                    auxEstimados.concepto = "INGRESOS PENDIENTES POR GENERAR";
                                    auxEstimados.corteID = corte.id;
                                    auxEstimados.cuenta = "1-2-3";
                                    auxEstimados.empresa = vSesiones.sesionEmpresaActual;
                                    auxEstimados.fechapol = new DateTime(2019, 12, 31);
                                    auxEstimados.id = 0;
                                    auxEstimados.linea = 0;
                                    auxEstimados.monto = exitoParse ? monto : 0;
                                    auxEstimados.poliza = "--";
                                    auxEstimados.referencia = cc.descripcion;
                                    auxEstimados.tipoEquipo = 7;
                                    estimados.Add(auxEstimados);
                                }
                                if (worksheet.Cells[i, 11].Value != null)
                                {
                                    decimal monto = 0;
                                    bool exitoParse = decimal.TryParse(worksheet.Cells[i, 11].Value.ToString(), out monto);
                                    tblM_KBCorteDet auxEstimados = new tblM_KBCorteDet();
                                    auxEstimados.areaCuenta = cc.areaCuenta;
                                    auxEstimados.cc = "N/A";
                                    auxEstimados.concepto = "COSTOS ESTIMADOS";
                                    auxEstimados.corteID = corte.id;
                                    auxEstimados.cuenta = "1-4-0";
                                    auxEstimados.empresa = vSesiones.sesionEmpresaActual;
                                    auxEstimados.fechapol = new DateTime(2019, 12, 31);
                                    auxEstimados.id = 0;
                                    auxEstimados.linea = 0;
                                    auxEstimados.monto = exitoParse ? monto : 0;
                                    auxEstimados.poliza = "--";
                                    auxEstimados.referencia = cc.descripcion;
                                    auxEstimados.tipoEquipo = 7;
                                    estimados.Add(auxEstimados);
                                }
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                _context.tblM_KBCorteDet.AddRange(estimados);
                var estimadosGuardados = _context.tblM_KBCorteDet.Where(x => x.corteID == corte.id && (x.cuenta == "1-1-0" || x.cuenta == "1-2-3" || x.cuenta == "1-4-0")).ToList();
                _context.tblM_KBCorteDet.RemoveRange(estimadosGuardados);
                _context.SaveChanges();
                return 1;
            }
            catch (Exception e)
            {
                return 0;
            }

        }

        #endregion

        #region Flujo Efectivo
        public List<tblM_KBCorteDet> GetEstimadosArrendadora()
        {
            List<tblM_KBCorteDet> lst = new List<tblM_KBCorteDet>();
            try
            {

                var lstParametros = new List<OdbcParameterDTO>();
                var odbc = new OdbcConsultaDTO();
                odbc.consulta = "SELECT cc AS Value, descripcion AS Text FROM cc";

                var CCs = _contextEnkontrolRentabilidad.Select<ComboDTO>(EnkontrolEnum.ArrenProd, odbc);

                tblM_KBCorte corteAnterior = new tblM_KBCorte();

                DateTime fechaCorte = DateTime.Today.AddDays(1);

                while (fechaCorte.DayOfWeek != DayOfWeek.Tuesday)
                    fechaCorte = fechaCorte.AddDays(-1);

                var cortesAnteriores = _context.tblM_KBCorte.Where(x => x.fechaCorte < fechaCorte && x.tipo == 0).ToList();
                if (cortesAnteriores.Count > 0) corteAnterior = cortesAnteriores.OrderByDescending(x => x.fechaCorte).FirstOrDefault();

                BusqKubrixDTO busq = new BusqKubrixDTO();
                busq.fechaInicio = new DateTime();
                busq.fechaFin = fechaCorte;
                busq.obra = new List<string>();

                var IngresosEstimados = getLstKubrixIngresosEstimacion(busq, true).Select(x => new tblM_KBCorteDet
                {
                    poliza = x.poliza,
                    cuenta = x.grupoInsumo,
                    concepto = x.insumo_Desc,
                    monto = x.importe,
                    cc = x.noEco,
                    areaCuenta = x.areaCuenta,
                    fechapol = x.fecha,
                    referencia = x.referencia,
                    linea = 0
                });
                List<tblM_KBCorteDet> ingresosEstimadosManuales = new List<tblM_KBCorteDet>();
                if (corteAnterior != null)
                {
                    ingresosEstimadosManuales = _context.tblM_KBCorteDet.Where(x => x.corteID == corteAnterior.id && x.cuenta == "1-2-3").ToList();
                }
                var pendientesGenerar = getLstKubrixIngresosPendientesGenerar(busq, -1, true).Select(x => new tblM_KBCorteDet
                {
                    poliza = x.poliza,
                    cuenta = x.grupoInsumo,
                    concepto = x.insumo_Desc,
                    monto = x.importe,
                    cc = x.noEco,
                    areaCuenta = x.areaCuenta,
                    fechapol = x.fecha,
                    referencia = x.referencia,
                    linea = 0
                });
                List<tblM_KBCorteDet> pendientesGenerarManuales = new List<tblM_KBCorteDet>();
                if (corteAnterior != null)
                {
                    pendientesGenerarManuales = _context.tblM_KBCorteDet.Where(x => x.corteID == corteAnterior.id && x.cuenta == "1-3-2").ToList();
                }
                List<tblM_KBCorteDet> costoEstimado = new List<tblM_KBCorteDet>();
                if (corteAnterior != null)
                {
                    costoEstimado = _context.tblM_KBCorteDet.Where(x => x.corteID == corteAnterior.id && x.cuenta == "1-4-0").ToList();
                }

                lst.AddRange(IngresosEstimados);
                lst.AddRange(ingresosEstimadosManuales);
                lst.AddRange(pendientesGenerar);
                lst.AddRange(pendientesGenerarManuales);
                lst.AddRange(costoEstimado);

                GetTipoMaquinaCorte(lst);

                lst.ForEach(x =>
                {
                    var cc = CCs.FirstOrDefault(y => y.Text == x.cc);
                    x.cc = cc == null ? "N/A" : cc.Value;
                });
            }
            catch (Exception o_O)
            {

            }
            return lst;
        }

        public int GuardarEstimadosArrendadora()
        {
            List<tblM_KBCorteDet> lst = new List<tblM_KBCorteDet>();
            try
            {

                var lstParametros = new List<OdbcParameterDTO>();
                var odbc = new OdbcConsultaDTO();
                odbc.consulta = "SELECT cc AS Value, descripcion AS Text FROM cc";

                var CCs = _contextEnkontrolRentabilidad.Select<ComboDTO>(EnkontrolEnum.ArrenProd, odbc);

                tblM_KBCorte corteAnterior = new tblM_KBCorte();

                DateTime fechaCorte = DateTime.Today.AddDays(1);

                while (fechaCorte.DayOfWeek != DayOfWeek.Tuesday)
                    fechaCorte = fechaCorte.AddDays(-1);

                var cortesAnteriores = _context.tblM_KBCorte.Where(x => x.fechaCorte < fechaCorte && x.tipo == 0).ToList();
                if (cortesAnteriores.Count > 0) corteAnterior = cortesAnteriores.OrderByDescending(x => x.fechaCorte).FirstOrDefault();

                BusqKubrixDTO busq = new BusqKubrixDTO();
                busq.fechaInicio = new DateTime();
                busq.fechaFin = fechaCorte;
                busq.obra = new List<string>();

                var IngresosEstimados = getLstKubrixIngresosEstimacion(busq, true).Select(x => new tblM_KBCorteDet
                {
                    poliza = x.poliza,
                    cuenta = x.grupoInsumo,
                    concepto = x.insumo_Desc,
                    monto = x.importe,
                    cc = x.noEco,
                    areaCuenta = x.areaCuenta,
                    fechapol = x.fecha,
                    referencia = x.referencia,
                    linea = 0
                });
                List<tblM_KBCorteDet> ingresosEstimadosManuales = new List<tblM_KBCorteDet>();
                if (corteAnterior != null)
                {
                    ingresosEstimadosManuales = _context.tblM_KBCorteDet.Where(x => x.corteID == corteAnterior.id && x.cuenta == "1-2-3").ToList();
                }
                var pendientesGenerar = getLstKubrixIngresosPendientesGenerar(busq, -1, true).Select(x => new tblM_KBCorteDet
                {
                    poliza = x.poliza,
                    cuenta = x.grupoInsumo,
                    concepto = x.insumo_Desc,
                    monto = x.importe,
                    cc = x.noEco,
                    areaCuenta = x.areaCuenta,
                    fechapol = x.fecha,
                    referencia = x.referencia,
                    linea = 0
                });
                List<tblM_KBCorteDet> pendientesGenerarManuales = new List<tblM_KBCorteDet>();
                if (corteAnterior != null)
                {
                    pendientesGenerarManuales = _context.tblM_KBCorteDet.Where(x => x.corteID == corteAnterior.id && x.cuenta == "1-3-2").ToList();
                }
                List<tblM_KBCorteDet> costoEstimado = new List<tblM_KBCorteDet>();
                if (corteAnterior != null)
                {
                    costoEstimado = _context.tblM_KBCorteDet.Where(x => x.corteID == corteAnterior.id && x.cuenta == "1-4-0").ToList();
                }

                lst.AddRange(IngresosEstimados);
                lst.AddRange(ingresosEstimadosManuales);
                lst.AddRange(pendientesGenerar);
                lst.AddRange(pendientesGenerarManuales);
                lst.AddRange(costoEstimado);

                GetTipoMaquinaCorte(lst);

                //lst.ForEach(x =>
                //{
                //    var cc = CCs.FirstOrDefault(y => y.Text == x.cc);
                //    x.cc = cc == null ? "N/A" : cc.Value;
                //});

                var corteID = saveCorte(lst, 13, fechaCorte, 10, 2);
                return corteID;
            }
            catch (Exception o_O)
            {
                return 0;
            }
        }
        #endregion

        public string GetCCByAC(string AC)
        {
            string data = "";
            try
            {
                var cc = _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == AC);
                data = cc.cc;
            }
            catch (Exception e)
            {

            }
            return data;
        }



        public List<CortesDetDTO> obtenerCortesArrendadora(int corteID, DateTime fechaFin)
        {
            List<CortesDetDTO> modResultado = new List<CortesDetDTO>(); ;
            DynamicParameters lstParametros = new DynamicParameters();
            var corte = getCorteByID(corteID);

            lstParametros.Add("@tipoCorte", corte.tipo, DbType.Int32);
            lstParametros.Add("@fechaFin", fechaFin, DbType.DateTime);
            
            try
            {
                var sql = @"spM_KB_ListaCorteAcumulado";
                using (var conexion = new SqlConnection(ConextSigoDapper.conexionSIGOPLANARRENDADORA()))
                {
                    conexion.Open();
                    modResultado = conexion.Query<CortesDetDTO>(sql, lstParametros, null, true, 300, commandType: CommandType.StoredProcedure).ToList();

                    conexion.Close();
                }
                //sql = @"spM_KB_ListaCorteAcumuladoArrendadora";
                //    using (var conexion = new SqlConnection(ConextSigoDapper.conexionSIGOPLAN()))
                //{
                //    conexion.Open();
                //    var modResultadoCplan = conexion.Query<CortesDetDTO>(sql, lstParametros, commandType: CommandType.StoredProcedure).ToList();
                //    modResultado.AddRange(modResultadoCplan);
                //    conexion.Close();
                //}
            }
            catch (Exception ex)
            {
                LogError(0, 0, "RentabilidadController", "obtenerCortesArrendadora", ex, AccionEnum.CONSULTA, 0, modResultado);
            }
            modResultado = modResultado.GroupBy(x => new { x.areaCuenta, x.cuenta, x.semana }).Select(x => new CortesDetDTO
            {
                areaCuenta = x.Key.areaCuenta,
                cuenta = x.Key.cuenta,
                semana = x.Key.semana,
                montoActual = x.Sum(y => y.montoActual),
                montoAcumulado = x.Sum(y => y.montoAcumulado)
            }).ToList();
            

            return modResultado;

        }

        public List<tblP_CC> obtenerCentrosCostos()
        {
            return _context.tblP_CC.Where(r => r.estatus).ToList();
        }

        #region Stored Procedures Kubrix
        public List<CorteDetDTO> getLstKubrixCortesArrendadora(int corteID, List<string> areaCuenta, List<int> modelos, string economico, DateTime fechaFin, bool reporteCostos, int acumulado)
        {
            List<CorteDetDTO> detalles = new List<CorteDetDTO>();
            DynamicParameters lstParametros = new DynamicParameters();
            // --> Cargar corte actual
            tblM_KBCorte corte = new tblM_KBCorte();

            corte = getCorteByID(corteID);

            // --> Calcular fecha inicio
            var fechaInicio = new DateTime(2020, 1, 1);
            switch (acumulado)
            {
                case 1: fechaInicio = new DateTime(fechaFin.Year, 1, 1); break;
                case 2: fechaInicio = new DateTime(2020, 1, 1); break;
            }
            // --> Revisar si la búsqueda aplica fletes u OTR
            bool aplicaFletes = (areaCuenta == null || areaCuenta.Count() < 1 || areaCuenta.Contains("6-5"));
            bool aplicaOTR = (areaCuenta == null || areaCuenta.Count() < 1 || areaCuenta.Contains("9-27"));
            // --> Si la búsqueda aplica modelos, cargar maquinas de los correspondientes modelos
            List<string> maquinasModelo = (modelos == null ? new List<string>() : _context.tblM_CatMaquina.Where(x => modelos.Contains(x.modeloEquipoID)).Select(x => x.noEconomico).ToList());
            // --> Asegurar que no se carguen listas en nulo
            if (areaCuenta == null) areaCuenta = new List<string>();
            if (maquinasModelo == null) maquinasModelo = new List<string>();

            // --> Agregar parametros
            lstParametros.Add("@tipoCorte", corte.tipo, DbType.Int32);
            lstParametros.Add("@areaCuenta", String.Join(",", areaCuenta.ToArray()), DbType.String);
            lstParametros.Add("@maquinas", String.Join(",", maquinasModelo.ToArray()), DbType.String);
            lstParametros.Add("@economico", economico, DbType.String);
            lstParametros.Add("@fechaInicio", fechaInicio, DbType.DateTime);
            lstParametros.Add("@fechaFin", fechaFin, DbType.DateTime);
            lstParametros.Add("@reporteCostos", reporteCostos, DbType.Boolean);
            lstParametros.Add("@aplicaFletes", aplicaFletes, DbType.Boolean);
            lstParametros.Add("@aplicaOTR", aplicaOTR, DbType.Boolean);
            // --> Llamado a stored procedure
            
            try
            {
                var sql = @"spM_KB_ListaCorte";
                using (var conexion = new SqlConnection(ConextSigoDapper.conexionSIGOPLANARRENDADORA()))
                {
                    conexion.Open();
                    detalles = conexion.Query<CorteDetDTO>(sql, lstParametros, null, true, 300, commandType: CommandType.StoredProcedure).ToList();
                    var Array1 = detalles.Where(r => r.acumulado == false).ToList();
                    conexion.Close();
                }
                sql = @"spM_KB_ListaCorteArrendadora";
                using (var conexion = new SqlConnection(ConextSigoDapper.conexionSIGOPLAN()))
                {
                    conexion.Open();
                    var lst = conexion.Query<CorteDetDTO>(sql, lstParametros, null, true, 300, commandType: CommandType.StoredProcedure).ToList();
                    var Array2 = lst.Where(r => r.acumulado == true).ToList();
                    detalles.AddRange(lst);
                    conexion.Close();
                }
            }
            catch (Exception ex)
            {
                LogError(0, 0, "RentabilidadController", "getLstKubrixCorte", ex, AccionEnum.CONSULTA, 0, detalles);
            }
            return detalles;
        }

        public List<CorteDTO> getLstKubrixCortesArrendadoraDetalle(int corteID, List<string> areaCuenta, List<int> modelos, string economico, DateTime fechaFin, bool reporteCostos, int acumulado, int concepto, int semana, bool aplicaSinDivision, bool aplicaCompacto)
        {
            List<CorteDTO> detalles = new List<CorteDTO>();
            DynamicParameters lstParametros = new DynamicParameters();     
            var corte = getCorteByID(corteID);
            var corteAnteriorID = 0;
            // --> Calcular fecha inicio
            var fechaInicio = new DateTime(1900, 1, 1);
            switch (acumulado)
            {
                case 1: fechaInicio = new DateTime(fechaFin.Year, 1, 1); break;
                case 2: fechaInicio = new DateTime(2020, 1, 1); break;
            }
            // --> Revisar si la búsqueda aplica fletes u OTR
            bool aplicaFletes = (areaCuenta == null || areaCuenta.Count() < 1 || areaCuenta.Contains("6-5"));
            bool aplicaOTR = (areaCuenta == null || areaCuenta.Count() < 1 || areaCuenta.Contains("9-27"));
            // --> Si la búsqueda aplica modelos, cargar maquinas de los correspondientes modelos
            List<string> maquinasModelo = (modelos == null ? new List<string>() : _context.tblM_CatMaquina.Where(x => modelos.Contains(x.modeloEquipoID)).Select(x => x.noEconomico).ToList());
            // --> Asegurar que no se carguen listas en nulo
            if (areaCuenta == null) areaCuenta = new List<string>();
            if (maquinasModelo == null) maquinasModelo = new List<string>();

            var corteAnterior = getCortesAnt(corte.fechaCorte, corte.tipo).OrderByDescending(x => x.fechaCorte).FirstOrDefault();
            if (corteAnterior != null) corteAnteriorID = corteAnterior.id;

            // --> Agregar parametros
            lstParametros.Add("@corteID", corteID, DbType.Int32);
            lstParametros.Add("@corteAnteriorID", corteAnteriorID, DbType.Int32);
            lstParametros.Add("@areaCuenta", String.Join(",", areaCuenta.ToArray()), DbType.String);
            lstParametros.Add("@maquinas", String.Join(",", maquinasModelo.ToArray()), DbType.String);
            lstParametros.Add("@economico", economico, DbType.String);
            lstParametros.Add("@fechaInicio", fechaInicio, DbType.DateTime);
            lstParametros.Add("@fechaFin", fechaFin, DbType.DateTime);
            lstParametros.Add("@reporteCostos", reporteCostos, DbType.Boolean);
            lstParametros.Add("@aplicaFletes", aplicaFletes, DbType.Boolean);
            lstParametros.Add("@aplicaOTR", aplicaOTR, DbType.Boolean);
            lstParametros.Add("@concepto", concepto, DbType.Int32);
            lstParametros.Add("@semana", semana, DbType.Int32);
            lstParametros.Add("@aplicaSinDivision", aplicaSinDivision, DbType.Int32);
            lstParametros.Add("@aplicaCompacto", aplicaCompacto, DbType.Boolean);
            // --> Llamado a stored procedure

            try
            {
                var sql = @"spM_KB_ListaCorteDet";
                using (var conexion = new SqlConnection(vSesiones.sesionEmpresaActual == 1 ? ConextSigoDapper.conexionSIGOPLAN() : ConextSigoDapper.conexionSIGOPLANARRENDADORA()))
                {
                    conexion.Open();
                    detalles = conexion.Query<CorteDTO>(sql, lstParametros, null, true, 300, commandType: CommandType.StoredProcedure).ToList();
                    conexion.Close();
                }

                if ((aplicaCompacto && concepto == 1) || (!aplicaCompacto && (concepto == 5 || concepto == 8 || concepto == 10)))
                {
                    DynamicParameters lstParametrosSIGOPLAN = new DynamicParameters();
                    // --> Agregar parametros
                    lstParametrosSIGOPLAN.Add("@tipoCorte", corte.tipo, DbType.Int32);
                    lstParametrosSIGOPLAN.Add("@areaCuenta", String.Join(",", areaCuenta.ToArray()), DbType.String);
                    lstParametrosSIGOPLAN.Add("@maquinas", String.Join(",", maquinasModelo.ToArray()), DbType.String);
                    lstParametrosSIGOPLAN.Add("@economico", economico, DbType.String);
                    lstParametrosSIGOPLAN.Add("@fechaInicio", fechaInicio, DbType.DateTime);
                    lstParametrosSIGOPLAN.Add("@fechaFin", fechaFin, DbType.DateTime);
                    lstParametrosSIGOPLAN.Add("@reporteCostos", reporteCostos, DbType.Boolean);
                    lstParametrosSIGOPLAN.Add("@concepto", concepto, DbType.Int32);
                    lstParametrosSIGOPLAN.Add("@semana", semana, DbType.Int32);
                    lstParametrosSIGOPLAN.Add("@aplicaSinDivision", aplicaSinDivision, DbType.Int32);
                    lstParametrosSIGOPLAN.Add("@aplicaCompacto", aplicaCompacto, DbType.Boolean);
                    
                    sql = @"spM_KB_ListaCorteArrendadoraDet";
                    using (var conexion = new SqlConnection(ConextSigoDapper.conexionSIGOPLAN()))
                    {
                        conexion.Open();
                        var detallesSIGOPLAN = conexion.Query<CorteDTO>(sql, lstParametrosSIGOPLAN, null, true, 300, commandType: CommandType.StoredProcedure).ToList();
                        detalles.AddRange(detallesSIGOPLAN);
                        conexion.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(0, 0, "RentabilidadController", "getLstKubrixCorte", ex, AccionEnum.CONSULTA, 0, detalles);
            }
            return detalles;
        }

        public List<CorteDetDTO> getLstKubrixCortesConstruplan(int corteID, List<string> areaCuenta, List<int> modelos, string economico, DateTime fechaFin, bool reporteCostos, int acumulado)
        {
            List<CorteDetDTO> detalles = new List<CorteDetDTO>();
            DynamicParameters lstParametros = new DynamicParameters();
            DynamicParameters lstParametrosHistorial = new DynamicParameters();
            // --> Cargar corte actual
            var corte = getCorteByID(corteID);
            // --> Calcular fecha inicio
            var fechaInicio = new DateTime(1900, 1, 1);
            switch (acumulado)
            {
                case 1: fechaInicio = new DateTime(fechaFin.Year, 1, 1); break;
                case 2: fechaInicio = new DateTime(2020, 1, 1); break;
            }
            // --> Revisar si la búsqueda aplica fletes u OTR
            bool aplicaFletes = (areaCuenta == null || areaCuenta.Count() < 1 || areaCuenta.Contains("6-5"));
            bool aplicaOTR = (areaCuenta == null || areaCuenta.Count() < 1 || areaCuenta.Contains("9-27"));
            // --> Si la búsqueda aplica modelos, cargar maquinas de los correspondientes modelos
            List<string> maquinasModelo = (modelos == null ? new List<string>() : _context.tblM_CatMaquina.Where(x => modelos.Contains(x.modeloEquipoID)).Select(x => x.noEconomico).ToList());
            // --> Asegurar que no se carguen listas en nulo
            if (areaCuenta == null) areaCuenta = new List<string>();
            if (maquinasModelo == null) maquinasModelo = new List<string>();

            // --> Agregar parametros
            lstParametros.Add("@tipoCorte", corte.tipo, DbType.Int32);
            lstParametros.Add("@areaCuenta", String.Join(",", areaCuenta.ToArray()), DbType.String);
            lstParametros.Add("@maquinas", String.Join(",", maquinasModelo.ToArray()), DbType.String);
            lstParametros.Add("@economico", economico, DbType.String);
            lstParametros.Add("@fechaInicio", fechaInicio, DbType.DateTime);
            lstParametros.Add("@fechaFin", fechaFin, DbType.DateTime);

            lstParametrosHistorial.Add("@areaCuenta", String.Join(",", areaCuenta.ToArray()), DbType.String);
            lstParametrosHistorial.Add("@maquinas", String.Join(",", maquinasModelo.ToArray()), DbType.String);
            lstParametrosHistorial.Add("@economico", economico, DbType.String);
            // --> Llamado a stored procedure

            try
            {
                var sql = @"spM_KB_ListaCorte";
                using (var conexion = new SqlConnection(ConextSigoDapper.conexionSIGOPLAN()))
                {
                    conexion.Open();
                    detalles = conexion.Query<CorteDetDTO>(sql, lstParametros, null, true, 300, commandType: CommandType.StoredProcedure).ToList();
                    conexion.Close();
                }
                sql = @"spM_KB_ListaCorteHistorial";
                using (var conexion = new SqlConnection(ConextSigoDapper.conexionSIGOPLAN()))
                {
                    conexion.Open();
                    var detallesHistorial = conexion.Query<CorteDetDTO>(sql, lstParametrosHistorial, null, true, 300, commandType: CommandType.StoredProcedure).ToList();
                    detalles.AddRange(detallesHistorial);
                    conexion.Close();
                }
            }
            catch (Exception ex)
            {
                LogError(0, 0, "RentabilidadController", "getLstKubrixCorte", ex, AccionEnum.CONSULTA, 0, detalles);
            }
            return detalles;
        }



        public List<CorteDetDTO> getLstKubrixCortesArrendadoraCentrosCostos(int corteID, List<string> areaCuenta, List<int> modelos, string economico, DateTime fechaFin, bool reporteCostos, int acumulado)
        {
            List<CorteDetDTO> detalles = new List<CorteDetDTO>();
            DynamicParameters lstParametros = new DynamicParameters();
            // --> Cargar corte actual
            tblM_KBCorte corte = new tblM_KBCorte();

            corte = getCorteByIDArrendadora();

            // --> Calcular fecha inicio
            var fechaInicio = new DateTime(2020, 1, 1);
            switch (acumulado)
            {
                case 1: fechaInicio = new DateTime(fechaFin.Year, 1, 1); break;
                case 2: fechaInicio = new DateTime(2020, 1, 1); break;
            }
            // --> Revisar si la búsqueda aplica fletes u OTR
            bool aplicaFletes = (areaCuenta == null || areaCuenta.Count() < 1 || areaCuenta.Contains("6-5"));
            bool aplicaOTR = (areaCuenta == null || areaCuenta.Count() < 1 || areaCuenta.Contains("9-27"));
            // --> Si la búsqueda aplica modelos, cargar maquinas de los correspondientes modelos
            List<string> maquinasModelo = (modelos == null ? new List<string>() : _context.tblM_CatMaquina.Where(x => modelos.Contains(x.modeloEquipoID)).Select(x => x.noEconomico).ToList());
            // --> Asegurar que no se carguen listas en nulo
            if (areaCuenta == null) areaCuenta = new List<string>();
            if (maquinasModelo == null) maquinasModelo = new List<string>();

            // --> Agregar parametros
            lstParametros.Add("@tipoCorte", corte.tipo, DbType.Int32);
            lstParametros.Add("@areaCuenta", String.Join(",", areaCuenta.ToArray()), DbType.String);
            lstParametros.Add("@maquinas", String.Join(",", maquinasModelo.ToArray()), DbType.String);
            lstParametros.Add("@economico", economico, DbType.String);
            lstParametros.Add("@fechaInicio", fechaInicio, DbType.DateTime);
            lstParametros.Add("@fechaFin", fechaFin, DbType.DateTime);
            lstParametros.Add("@reporteCostos", reporteCostos, DbType.Boolean);
            lstParametros.Add("@aplicaFletes", aplicaFletes, DbType.Boolean);
            lstParametros.Add("@aplicaOTR", aplicaOTR, DbType.Boolean);
            // --> Llamado a stored procedure

            try
            {
                var sql = @"spM_KB_ListaCorteCentrosDeCostos";
                using (var conexion = new SqlConnection(ConextSigoDapper.conexionSIGOPLANARRENDADORA()))
                {
                    conexion.Open();
                    detalles = conexion.Query<CorteDetDTO>(sql, lstParametros, null, true, 300, commandType: CommandType.StoredProcedure).ToList();
                    var Array1 = detalles.Where(r => r.acumulado == false).ToList();
                    conexion.Close();
                }
                sql = @"spM_KB_ListaCorteArrendadoraCentrosDeCostos";
                using (var conexion = new SqlConnection(ConextSigoDapper.conexionSIGOPLAN()))
                {
                    conexion.Open();
                    var lst = conexion.Query<CorteDetDTO>(sql, lstParametros, null, true, 300, commandType: CommandType.StoredProcedure).ToList();
                    var Array2 = lst.Where(r => r.acumulado == true).ToList();
                    detalles.AddRange(lst);
                    conexion.Close();
                }
            }
            catch (Exception ex)
            {
                LogError(0, 0, "RentabilidadController", "getLstKubrixCorte", ex, AccionEnum.CONSULTA, 0, detalles);
            }
            return detalles;
        }



        #endregion

        #region Nuevas Funciones para Kubrix Cplan
        public List<ComboDTO> getListaCCConstruplan(int usuarioID)
        {
            try
            {
                List<ComboDTO> listaCCFinal = new List<ComboDTO>();
                //var ccResponsable = _context.tblM_KBAreaCuentaResponsable.Where(x => x.usuarioResponsableID == usuarioID && x.estatus).ToList();
                
                //if (ccResponsable.Count() > 0)
                //{
                //    var ccResponsableIDs = ccResponsable.Select(x => x.areaCuentaID).ToList();
                //    listaCCFinal = _context.tblP_CC.Where(x => ccResponsableIDs.Contains(x.id)).Select(x => new ComboDTO
                //    {
                //        Value = x.cc,
                //        Text = "[" + x.cc + " / " + x.areaCuenta + "] - " + x.descripcion,
                //        Prefijo = x.cc,
                //        TextoOpcional = x.estatus.ToString()
                //    }).ToList();
                //}

                //else 
                //{
                //    var listaCCEnCorte = _context.tblM_KB_CorteDet.Where(x => x.registroActivo).Select(x => x.ccSIGOPLAN).Distinct().ToList();
                //    var listaCCEnCorteBD = _context.tblP_CC.Where(x => listaCCEnCorte.Contains(x.cc)).ToList();
                //    var listaCCEnCorteBDIDs = listaCCEnCorteBD.Select(x => x.cc).ToList();
                //    var listaCCEnCorteSinBD = listaCCEnCorte.Where(x => !listaCCEnCorteBDIDs.Contains(x)).ToList();
                //    listaCCFinal.AddRange(listaCCEnCorteBD.Select(x => new ComboDTO
                //    {
                //        Value = x.cc,
                //        Text = "[" + x.cc + " / " + x.areaCuenta + "] - " + x.descripcion,
                //        Prefijo = x.cc,
                //        TextoOpcional = x.estatus.ToString()
                //    }));
                //    listaCCFinal.AddRange(listaCCEnCorteSinBD.Select(x => new ComboDTO
                //    {
                //        Value = x,
                //        Text = "[" + x + "]",
                //        Prefijo = x,
                //        TextoOpcional = true.ToString()
                //    }));
                //}

                return listaCCFinal.OrderBy(x => x.Value).ToList();               

            }
            catch (Exception) { return new List<ComboDTO>(); }
        }

        public List<ComboDTO> getListaFechasConstruplan(int tipoCorte)
        {
            try
            {
                List<ComboDTO> listaFechasFinal = new List<ComboDTO>();
                //var listaFechasBD = _context.tblM_KB_Corte.Where(x => x.tipoCorte == tipoCorte && x.estatus).OrderByDescending(x => x.fechaCorte).ToList();
                
                //foreach(var item in listaFechasBD)
                //{
                //    ComboDTO _combo = new ComboDTO();
                //    _combo.Value = item.id.ToString();
                //    _combo.Text = "PERIODO #" + item.periodo + " AL " + item.fechaCorte.ToShortDateString();
                //    _combo.Prefijo = item.corteAnteriorID.ToString();
                //    listaFechasFinal.Add(_combo);
                //}

                return listaFechasFinal;
            }
            catch (Exception) { return new List<ComboDTO>(); }
        }

        public Dictionary<string, object> cargarInformacionNivel1(int corteID, List<string> listaCC, int usuarioID)
        {
            try
            {
                //if(listaCC == null) listaCC = new List<string>();
                //List<KB_TotalesSemanaDTO> listaNivel1 = new List<KB_TotalesSemanaDTO>();
                //var corte = _context.tblM_KB_Corte.FirstOrDefault(x => x.id == corteID);
                //if (corte != null)
                //{
                //    var cortesAnteriores = _context.tblM_KB_Corte.Where(x => x.fechaCorte < corte.fechaCorte && x.estatus).OrderByDescending(x => x.fechaCorte).ToList();
                //    var cortesAnterioresIDs = cortesAnteriores.Select(x => x.id).ToList();
                //    var detalles = _context.tblM_KB_CorteDet.Where(x => cortesAnterioresIDs.Contains(x.corteID) && (listaCC.Count() > 0 ? listaCC.Contains(x.ccSIGOPLAN) : true)).ToList();

                //    var corte1 = cortesAnterioresIDs[0];
                //    var corte2 = cortesAnterioresIDs[1];
                //    var corte3 = cortesAnterioresIDs[2];
                //    var corte4 = cortesAnterioresIDs[3];
                //    var corte5 = cortesAnterioresIDs[4];
                //    var corte6 = cortesAnterioresIDs[5];

                //    KB_TotalesSemanaDTO avance = new KB_TotalesSemanaDTO();
                //    KB_TotalesSemanaDTO costo = new KB_TotalesSemanaDTO();
                //    KB_TotalesSemanaDTO resultadoTotal = new KB_TotalesSemanaDTO();
                //    avance.descripcion = "AVANCE";
                //    avance.semana1 = detalles.Where(x => ((x.cta >= 4000 && x.cta < 5000) || x.cta == 1) && x.corteID == corte1).Select(x => x.monto).Sum();
                //    avance.semana2 = detalles.Where(x => ((x.cta >= 4000 && x.cta < 5000) || x.cta == 1) && x.corteID == corte2).Select(x => x.monto).Sum();
                //    avance.semana3 = detalles.Where(x => ((x.cta >= 4000 && x.cta < 5000) || x.cta == 1) && x.corteID == corte3).Select(x => x.monto).Sum();
                //    avance.semana4 = detalles.Where(x => ((x.cta >= 4000 && x.cta < 5000) || x.cta == 1) && x.corteID == corte4).Select(x => x.monto).Sum();
                //    avance.semana5 = detalles.Where(x => ((x.cta >= 4000 && x.cta < 5000) || x.cta == 1) && x.corteID == corte5).Select(x => x.monto).Sum();
                //    avance.semana6 = detalles.Where(x => ((x.cta >= 4000 && x.cta < 5000) || x.cta == 1) && x.corteID == corte6).Select(x => x.monto).Sum();
                //    listaNivel1.Add(avance);

                //    costo.descripcion = "COSTO";
                //    costo.semana1 = detalles.Where(x => ((x.cta >= 5000 && x.cta < 6000) || x.cta == 2) && x.corteID == corte1).Select(x => x.monto).Sum();
                //    costo.semana2 = detalles.Where(x => ((x.cta >= 5000 && x.cta < 6000) || x.cta == 2) && x.corteID == corte2).Select(x => x.monto).Sum();
                //    costo.semana3 = detalles.Where(x => ((x.cta >= 5000 && x.cta < 6000) || x.cta == 2) && x.corteID == corte3).Select(x => x.monto).Sum();
                //    costo.semana4 = detalles.Where(x => ((x.cta >= 5000 && x.cta < 6000) || x.cta == 2) && x.corteID == corte4).Select(x => x.monto).Sum();
                //    costo.semana5 = detalles.Where(x => ((x.cta >= 5000 && x.cta < 6000) || x.cta == 2) && x.corteID == corte5).Select(x => x.monto).Sum();
                //    costo.semana6 = detalles.Where(x => ((x.cta >= 5000 && x.cta < 6000) || x.cta == 2) && x.corteID == corte6).Select(x => x.monto).Sum();
                //    listaNivel1.Add(costo);

                //    resultadoTotal.descripcion = "RESULTADO";
                //    resultadoTotal.semana1 = avance.semana1 + costo.semana1;
                //    resultadoTotal.semana2 = avance.semana2 + costo.semana2;
                //    resultadoTotal.semana3 = avance.semana3 + costo.semana3;
                //    resultadoTotal.semana4 = avance.semana4 + costo.semana4;
                //    resultadoTotal.semana5 = avance.semana5 + costo.semana5;
                //    resultadoTotal.semana6 = avance.semana6 + costo.semana6;
                //    listaNivel1.Add(resultadoTotal);

                //    resultado.Add("listaNivel1", listaNivel1);
                //    resultado.Add(SUCCESS, true);
                //}

                //else 
                //{
                //    resultado.Add(SUCCESS, false);
                //    resultado.Add(MESSAGE, "No se encontró el corte solicitado. Favor de recargar e intentar de nuevo");
                //}                
            }
            catch (Exception e)
            {
                LogError(3, 0, NombreControlador, "cargarInformacionNivel1", e, AccionEnum.CONSULTA, 0, corteID);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        #endregion
    }
}
