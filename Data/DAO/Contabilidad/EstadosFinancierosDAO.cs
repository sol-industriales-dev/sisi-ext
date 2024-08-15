using Core.DAO.Contabilidad;
using Core.DTO;
using Core.DTO.Contabilidad;
using Core.DTO.Principal.Generales;
using Core.DTO.Utils.Data;
using Core.Entity.Administrativo.Contabilidad;
using Core.Entity.Principal.Multiempresa;
using Core.Entity.Principal.Usuarios;
using Core.Enum.Contabilidad;
using Core.Enum.Multiempresa;
using Core.Enum.Principal;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Utils;
using System.Globalization;
using Core.Enum.Contabilidad.EstadoFinanciero;
using Core.DTO.Contabilidad.EstadoFinanciero;
using Core.Entity.Administrativo.Contabilidad.EstadoFinanciero;
using Core.Enum.Contabilidad.Moneda;
using Core.DTO.Highcharts.Line;
using Core.DTO.Highcharts;
using System.Data.Odbc;

namespace Data.DAO.Contabilidad
{
    public class EstadosFinancierosDAO : GenericDAO<tblP_Usuario>, IEstadosFinancierosDAO
    {
        private Dictionary<string, object> resultado = new Dictionary<string, object>();

        public EstadosFinancierosDAO()
        {
            resultado.Clear();
        }

        public Dictionary<string, object> calcularBalanza(DateTime fechaAnioMes)
        {
            try
            {
                resultado.Add("data", obtenerBalanzaEnkontrol(fechaAnioMes));
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                LogError(0, 0, "EstadosFinancierosController", "calcularBalanza", e, AccionEnum.CONSULTA, 0, fechaAnioMes);
            }

            return resultado;
        }

        private List<BalanzaDTO> obtenerBalanzaEnkontrol(DateTime fechaAnioMes)
        {
            var anio = fechaAnioMes.Year;
            var mes = fechaAnioMes.Month;

            #region Obtener el string del mes abreviado.
            var mesString = "";

            switch (mes)
            {
                case 1:
                    mesString = "ene";
                    break;
                case 2:
                    mesString = "feb";
                    break;
                case 3:
                    mesString = "mar";
                    break;
                case 4:
                    mesString = "abr";
                    break;
                case 5:
                    mesString = "may";
                    break;
                case 6:
                    mesString = "jun";
                    break;
                case 7:
                    mesString = "jul";
                    break;
                case 8:
                    mesString = "ago";
                    break;
                case 9:
                    mesString = "sep";
                    break;
                case 10:
                    mesString = "oct";
                    break;
                case 11:
                    mesString = "nov";
                    break;
                case 12:
                    mesString = "dic";
                    break;
            }
            #endregion

            string[] _mesesContables = {
                                           "ene",
                                           "feb",
                                           "mar",
                                           "abr",
                                           "may",
                                           "jun",
                                           "jul",
                                           "ago",
                                           "sep",
                                           "oct",
                                           "nov",
                                           "dic"
                                       };

            var mesesCargo = "+ ";
            var mesesAbono = "+ ";
            var hayMeses = false;
            for (int i = 0; i < fechaAnioMes.Month - 1; i++)
            {
                hayMeses = true;
                mesesCargo += "sum(sal." + _mesesContables[i] + "cargos)";
                mesesAbono += "sum(sal." + _mesesContables[i] + "abonos)";
                if (i < fechaAnioMes.Month - 2)
                {
                    mesesCargo += " + ";
                    mesesAbono += " + ";
                }
            }
            mesesCargo = mesesCargo.Trim();
            mesesAbono = mesesAbono.Trim();
            if (!hayMeses)
            {
                mesesCargo = "";
                mesesAbono = "";
            }

            var data = _contextEnkontrol.Select<BalanzaDTO>(getEnkontrolEnumADM(), new OdbcConsultaDTO()
            {
                consulta = string.Format(@"
                    SELECT
                        sal.cta,
                        sal.scta,
                        sal.sscta,
                        cue.descripcion,
                        (CASE WHEN sal.cta BETWEEN 4000 AND 5999 THEN 0 ELSE SUM(sal.salini) END) {2} {3} AS saldoInicial,
                        SUM(sal.{0}cargos) AS cargos,
                        SUM(sal.{0}abonos) AS abonos,
                        (saldoInicial + cargos + abonos) AS saldoActual
                    FROM sc_salcont sal
                        INNER JOIN catcta cue ON sal.cta = cue.cta AND cue.scta = 0 AND cue.sscta = 0
                    WHERE sal.year = {1} AND sal.cta > 0 AND sal.scta = 0 AND sal.sscta = 0
                    GROUP BY sal.cta, sal.scta, sal.sscta, cue.descripcion
                    ORDER BY sal.cta, sal.scta, sal.sscta", mesString, anio, mesesCargo, mesesAbono)
            }).ToList();

            return data;
        }

        public Dictionary<string, object> guardarBalanzaCorte(DateTime fechaAnioMes)
        {
            try
            {
                var anio = fechaAnioMes.Year;
                var mes = fechaAnioMes.Month;
                var ultimoDiaMes = new DateTime(anio, mes, DateTime.DaysInMonth(anio, mes));

                #region Validación para verificar si ya se guardó el mes indicado.
                var corteMesSIGOPLAN = _context.tblEF_CorteMes.FirstOrDefault(x => x.estatus && x.anio == anio && x.mes == mes);

                if (corteMesSIGOPLAN != null)
                {
                    throw new Exception("Ya existe información guardada para el mes indicado.");
                }
                #endregion

                #region Guardar Registro Corte Mes
                var corteMes = new tblEF_CorteMes
                {
                    anio = anio,
                    mes = mes,
                    usuarioCapturaID = vSesiones.sesionUsuarioDTO.id,
                    fechaCaptura = DateTime.Now,
                    estatus = false
                };

                _context.tblEF_CorteMes.Add(corteMes);
                _context.SaveChanges();
                #endregion

                #region Guardar Registros Balanza
                var balanza = obtenerBalanzaEnkontrol(fechaAnioMes);

                foreach (var bal in balanza)
                {
                    _context.tblEF_Balanza.Add(new tblEF_Balanza
                    {
                        cta = bal.cta,
                        scta = bal.scta,
                        sscta = bal.sscta,
                        saldoInicial = bal.saldoInicial,
                        cargos = bal.cargos,
                        abonos = bal.abonos,
                        saldoActual = bal.saldoActual,
                        corteMesID = corteMes.id,
                        estatus = true
                    });
                    _context.SaveChanges();
                }
                #endregion

                #region Guardar Registros Cuentas por Cobrar
                if (vSesiones.sesionEmpresaActual != (int)EmpresaEnum.Arrendadora)
                {
                    var movimientos = _contextEnkontrol.Select<tblEF_MovimientoCliente>(getEnkontrolEnumADM(), new OdbcConsultaDTO()
                    {
                        consulta = string.Format(@"
                            SELECT
                                cli.numcte AS numeroCliente,
                                cli.tm AS tipoMovimiento,
                                cli.cc,
                                --area,
                                --cuenta,
                                --areaCuenta,
                                SUM(cli.total) AS total,
                                {1} AS corteMesID,
                                1 AS estatus
                            FROM sx_movcltes cli
                            WHERE cli.fecha <= '{0}'
                            GROUP BY cli.numcte, cli.tm, cli.cc", (ultimoDiaMes.Year + "-" + ultimoDiaMes.Month + "-" + ultimoDiaMes.Day), corteMes.id)
                    }).ToList();

                    _context.tblEF_MovimientoCliente.AddRange(movimientos);
                    _context.SaveChanges();
                }
                else
                {
                    var movimientos = _contextEnkontrol.Select<tblEF_MovimientoCliente>(getEnkontrolEnumADM(), new OdbcConsultaDTO()
                    {
                        consulta = string.Format(@"
                            SELECT
                                cli.numcte AS numeroCliente,
                                cli.tm AS tipoMovimiento,
                                '' AS cc,
                                pol.area,
                                pol.cuenta_oc AS cuenta,
                                CASE
                                 WHEN pol.area IS NOT NULL THEN CONVERT(varchar, pol.area) + '-' + CONVERT(varchar, pol.cuenta_oc)
                                 ELSE NULL
                                END as areaCuenta,
                                SUM(cli.total) AS total,
                                {1} AS corteMesID,
                                1 AS estatus
                            FROM sx_movcltes cli
                                LEFT JOIN sc_movpol pol ON cli.year = pol.year AND cli.mes = pol.mes AND cli.poliza = pol.poliza AND cli.tp = pol.tp AND cli.linea = pol.linea
                            WHERE cli.fecha <= '{0}'
                            GROUP BY cli.numcte, cli.tm, pol.area, pol.cuenta_oc", (ultimoDiaMes.Year + "-" + ultimoDiaMes.Month + "-" + ultimoDiaMes.Day), corteMes.id)
                    }).ToList();

                    _context.tblEF_MovimientoCliente.AddRange(movimientos);
                    _context.SaveChanges();
                }
                #endregion

                #region Guardar Registros Cuentas por Pagar
                if (vSesiones.sesionEmpresaActual != (int)EmpresaEnum.Arrendadora)
                {
                    var movimientos = _contextEnkontrol.Select<tblEF_MovimientoProveedor>(getEnkontrolEnumADM(), new OdbcConsultaDTO()
                    {
                        consulta = string.Format(@"
                            SELECT
                                prov.numpro AS numeroProveedor,
                                prov.tm AS tipoMovimiento,
                                prov.cc,
                                --area,
                                --cuenta,
                                --areaCuenta,
                                SUM(prov.total) AS total,
                                {1} AS corteMesID,
                                1 AS estatus
                            FROM sp_movprov prov
                            WHERE prov.fecha <= '{0}'
                            GROUP BY prov.numpro, prov.tm, prov.cc", (ultimoDiaMes.Year + "-" + ultimoDiaMes.Month + "-" + ultimoDiaMes.Day), corteMes.id)
                    }).ToList();

                    _context.tblEF_MovimientoProveedor.AddRange(movimientos);
                    _context.SaveChanges();
                }
                else
                {
                    var movimientos = _contextEnkontrol.Select<tblEF_MovimientoProveedor>(getEnkontrolEnumADM(), new OdbcConsultaDTO()
                    {
                        consulta = string.Format(@"
                            SELECT
                                prov.numpro AS numeroProveedor,
                                prov.tm AS tipoMovimiento,
                                '' AS cc,
                                pol.area,
                                pol.cuenta_oc AS cuenta,
                                CASE
                                 WHEN pol.area IS NOT NULL THEN CONVERT(varchar, pol.area) + '-' + CONVERT(varchar, pol.cuenta_oc)
                                 ELSE NULL
                                END as areaCuenta,
                                SUM(prov.total) AS total,
                                {1} AS corteMesID,
                                1 AS estatus
                            FROM sp_movprov prov
                                LEFT JOIN sc_movpol pol ON prov.year = pol.year AND prov.mes = pol.mes AND prov.poliza = pol.poliza AND prov.tp = pol.tp AND prov.linea = pol.linea
                            WHERE prov.fecha <= '{0}'
                            GROUP BY prov.numpro, prov.tm, pol.area, pol.cuenta_oc", (ultimoDiaMes.Year + "-" + ultimoDiaMes.Month + "-" + ultimoDiaMes.Day), corteMes.id)
                    }).ToList();

                    _context.tblEF_MovimientoProveedor.AddRange(movimientos);
                    _context.SaveChanges();
                }
                #endregion

                #region Guardar Registros Estimación
                var kubrixCorte = _context.tblM_KBCorte.Where(x => x.tipo == 1).ToList().FirstOrDefault(x => x.fechaCorte.Year == anio && x.fechaCorte.Month == mes);

                if (kubrixCorte != null)
                {
                    var kubrixCorteDetalle = _context.tblM_KBCorteDet.Where(x => x.corteID == kubrixCorte.id && x.cuenta.StartsWith("1-")).ToList();

                    if (kubrixCorteDetalle.Count > 0)
                    {
                        var listaCC = _context.tblP_CC.ToList();
                        var saldoKubrix = kubrixCorteDetalle.GroupBy(x => new { x.fechapol.Year, x.cuenta, x.cc, x.areaCuenta }).Select(x => new tblEF_SaldoCC
                        {
                            anio = x.Key.Year,
                            cta = x.Key.cuenta.Split('-').Count() == 3 ? Int32.Parse(x.Key.cuenta.Split('-')[0]) : 0,
                            scta = x.Key.cuenta.Split('-').Count() == 3 ? Int32.Parse(x.Key.cuenta.Split('-')[1]) : 0,
                            sscta = x.Key.cuenta.Split('-').Count() == 3 ? Int32.Parse(x.Key.cuenta.Split('-')[2]) : 0,
                            saldoInicial = x.Sum(y => x.Key.Year < anio ? y.monto : 0),
                            cargosMes =
                                x.Sum(y => (x.Key.Year == anio && x.Key.cuenta.StartsWith("1-4")) ?
                                    x.Sum(z => z.fechapol.Month == mes ? z.monto : 0)
                                : 0),
                            abonosMes =
                                x.Sum(y => (x.Key.Year == anio && (x.Key.cuenta.StartsWith("1-1") || x.Key.cuenta.StartsWith("1-2") || x.Key.cuenta.StartsWith("1-3"))) ?
                                    x.Sum(z => z.fechapol.Month == mes ? z.monto : 0)
                                : 0),
                            cargosAcumulados =
                                x.Sum(y => (x.Key.Year == anio && x.Key.cuenta.StartsWith("1-4")) ?
                                    x.Sum(z => z.fechapol.Month >= 1 && z.fechapol.Month <= mes ? z.monto : 0)
                                : 0),
                            abonosAcumulados =
                                x.Sum(y => (x.Key.Year == anio && (x.Key.cuenta.StartsWith("1-1") || x.Key.cuenta.StartsWith("1-2") || x.Key.cuenta.StartsWith("1-3"))) ?
                                    x.Sum(z => z.fechapol.Month >= 1 && z.fechapol.Month <= mes ? z.monto : 0)
                                : 0),
                            cc = listaCC.FirstOrDefault(y => y.areaCuenta == x.Key.areaCuenta) != null ? listaCC.FirstOrDefault(y => y.areaCuenta == x.Key.areaCuenta).cc : "",
                            area = x.Key.areaCuenta.Split('-').Count() == 2 ? Int32.Parse(x.Key.areaCuenta.Split('-')[0]) : 0,
                            cuenta = x.Key.areaCuenta.Split('-').Count() == 2 ? Int32.Parse(x.Key.areaCuenta.Split('-')[1]) : 0,
                            areaCuenta = x.Key.areaCuenta,
                            corteMesID = corteMes.id,
                            estatus = true
                        }).ToList().Where(x => x.anio == anio).ToList();

                        _context.tblEF_SaldoCC.AddRange(saldoKubrix);
                        _context.SaveChanges();
                    }
                }
                #endregion

                #region Guardar Registros Saldos por SqlBulk (después del commit de la transacción)
                string stringCon = "";
                switch (vSesiones.sesionEmpresaActual)
                {
                    case (int)EmpresaEnum.Construplan:
                        stringCon = System.Configuration.ConfigurationManager.ConnectionStrings["MainContext"].ConnectionString;
                        break;
                    case (int)EmpresaEnum.Arrendadora:
                        stringCon = System.Configuration.ConfigurationManager.ConnectionStrings["MainContextArrendadora"].ConnectionString;
                        break;
                    case (int)EmpresaEnum.EICI:
                        stringCon = System.Configuration.ConfigurationManager.ConnectionStrings["MainContextEICI"].ConnectionString;
                        break;
                    case (int)EmpresaEnum.Integradora:
                        stringCon = System.Configuration.ConfigurationManager.ConnectionStrings["MainContextIntegradora"].ConnectionString;
                        break;
                }

                var saldos = new List<tblEF_SaldoCC>();
                var listaCuentas = _context.tblEF_CuentaConcepto.Where(x => x.estatus && x.cta >= 1000).GroupBy(x => x.cta).Select(x => x.Key).ToList();
                var listaCuentasBalance = _context.tblEF_BalanceCuenta.Where(x => x.registroActivo && x.cta >= 1000 && x.tipoCuentaId == TipoCuentaEnum.CUENTA).Select(x => x.cta).Distinct().ToList();
                foreach (var item in listaCuentasBalance)
                {
                    listaCuentas.RemoveAll(x => x == item);
                }
                listaCuentas.AddRange(listaCuentasBalance);

                var listaCuentasString = string.Join(", ", listaCuentas);

                DataTable dt = new DataTable();

                dt.Columns.Add("id", System.Type.GetType("System.Int32"));
                dt.Columns.Add("anio", System.Type.GetType("System.Int32"));
                dt.Columns.Add("cta", System.Type.GetType("System.Int32"));
                dt.Columns.Add("scta", System.Type.GetType("System.Int32"));
                dt.Columns.Add("sscta", System.Type.GetType("System.Int32"));
                dt.Columns.Add("saldoInicial", System.Type.GetType("System.Decimal"));
                dt.Columns.Add("cargosMes", System.Type.GetType("System.Decimal"));
                dt.Columns.Add("abonosMes", System.Type.GetType("System.Decimal"));
                dt.Columns.Add("cargosAcumulados", System.Type.GetType("System.Decimal"));
                dt.Columns.Add("abonosAcumulados", System.Type.GetType("System.Decimal"));
                dt.Columns.Add("cc", System.Type.GetType("System.String"));
                dt.Columns.Add("area", System.Type.GetType("System.Int32"));
                dt.Columns.Add("cuenta", System.Type.GetType("System.Int32"));
                dt.Columns.Add("areaCuenta", System.Type.GetType("System.String"));
                dt.Columns.Add("corteMesID", System.Type.GetType("System.Int32"));
                dt.Columns.Add("itm", System.Type.GetType("System.Int32"));
                dt.Columns.Add("estatus", System.Type.GetType("System.Boolean"));

                if (vSesiones.sesionEmpresaActual != (int)EmpresaEnum.Arrendadora)
                {
                    var odbc = new OdbcConsultaDTO();
                    //CASE WHEN MOV.cta BETWEEN 4000 AND 5999 THEN 0 ELSE SUM(CASE WHEN MOV.year < 2022 THEN MOV.monto ELSE 0 END) END AS saldoInicial,
                    odbc.consulta = string.Format(@"
                            SELECT
                                Y.year AS anio,
                                Y.cta,
                                Y.scta,
                                Y.sscta,
                                SUM(Y.saldoInicial) AS saldoInicial,
                                SUM(y.cargosMes) AS cargosMes,
                                SUM(Y.abonosMes) AS abonosMes,
                                SUM(Y.cargosAcumulados) AS cargosAcumulados,
                                SUM(Y.abonosAcumulados) AS abonosAcumulados,
                                Y.cc,
                                Y.itm,
                                NULL AS area,
                                NULL AS cuenta,
                                NULL as areaCuenta,
                                {2} AS corteMesID,
                                1 AS estatus
                            FROM
                                (
                                    SELECT                                            
                                        {0} AS year,
                                        MOV.cta,
                                        MOV.scta,
                                        MOV.sscta,
                                        MOV.cc,
                                        MOV.itm,
                                        SUM(CASE WHEN MOV.year < 2022 THEN MOV.monto ELSE 0 END) AS saldoInicial,
                                        SUM(CASE WHEN MOV.year = {0} AND MOV.tm IN (1, 3) AND MOV.mes = {1} THEN MOV.monto ELSE 0 END) AS cargosMes,
                                        SUM(CASE WHEN MOV.year = {0} AND MOV.tm IN (2, 4) AND MOV.mes = {1} THEN MOV.monto ELSE 0 END) AS abonosMes,
                                        SUM(CASE WHEN MOV.year = {0} AND MOV.tm IN (1, 3) AND MOV.mes <= {1} THEN MOV.monto ELSE 0 END) AS cargosAcumulados,
                                        SUM(CASE WHEN MOV.year = {0} AND MOV.tm IN (2, 4) AND MOV.mes <= {1} THEN MOV.monto ELSE 0 END) AS abonosAcumulados
                                    FROM
                                        sc_movpol AS MOV
                                    WHERE
                                        MOV.cta IN {3} AND
                                        (
                                            (MOV.year < {0}) OR
                                            (MOV.year = {0} AND MOV.mes <= {1})
                                        ) AND
                                        (SELECT TOP 1 status FROM sc_polizas POL WHERE MOV.year = POL.year AND MOV.mes = POL.mes AND MOV.tp = POL.tp AND MOV.poliza = POL.poliza) = 'A'
                                    GROUP BY
                                        MOV.cta, MOV.scta, MOV.sscta, MOV.cc, MOV.itm
                                    ORDER BY
                                        MOV.cta, MOV.scta, MOV.sscta, MOV.cc, MOV.itm
                                ) AS Y
                            GROUP BY
                                anio, Y.cta, Y.scta, Y.sscta, Y.cc, Y.itm", anio, mes, corteMes.id, listaCuentas.ToParamInValue());
                    odbc.parametros.AddRange(listaCuentas.Select(s => new OdbcParameterDTO() { nombre = "cta", tipo = OdbcType.Int, valor = s }));
                    saldos = _contextEnkontrol.Select<tblEF_SaldoCC>(getEnkontrolEnumADM(), odbc);
                }
                else
                {
                    //CASE WHEN MOV.cta BETWEEN 4000 AND 5999 THEN 0 ELSE SUM(CASE WHEN MOV.year < 2022 THEN MOV.monto ELSE 0 END) END AS saldoInicial,
                    var odbc = new OdbcConsultaDTO();
                    odbc.consulta = string.Format(@"
                            SELECT
                                Y.year AS anio,
                                Y.cta,
                                Y.scta,
                                Y.sscta,
                                SUM(Y.saldoInicial) AS saldoInicial,
                                SUM(y.cargosMes) AS cargosMes,
                                SUM(Y.abonosMes) AS abonosMes,
                                SUM(Y.cargosAcumulados) AS cargosAcumulados,
                                SUM(Y.abonosAcumulados) AS abonosAcumulados,
                                NULL AS cc,
                                Y.area,
                                Y.cuenta_oc AS cuenta,
                                CASE
                                    WHEN Y.area IS NOT NULL THEN CONVERT(varchar, Y.area) + '-' + CONVERT(varchar, Y.cuenta_oc)
                                    ELSE NULL
                                END as areaCuenta,
                                Y.itm,
                                {2} AS corteMesID,
                                1 AS estatus
                            FROM
                                (
                                    SELECT                                            
                                        {0} AS year,
                                        MOV.cta,
                                        MOV.scta,
                                        MOV.sscta,
                                        MOV.area,
                                        MOV.cuenta_oc,
                                        MOV.itm,
                                        SUM(CASE WHEN MOV.year < 2022 THEN MOV.monto ELSE 0 END) AS saldoInicial,
                                        SUM(CASE WHEN MOV.year = {0} AND MOV.tm IN (1, 3) AND MOV.mes = {1} THEN MOV.monto ELSE 0 END) AS cargosMes,
                                        SUM(CASE WHEN MOV.year = {0} AND MOV.tm IN (2, 4) AND MOV.mes = {1} THEN MOV.monto ELSE 0 END) AS abonosMes,
                                        SUM(CASE WHEN MOV.year = {0} AND MOV.tm IN (1, 3) AND MOV.mes <= {1} THEN MOV.monto ELSE 0 END) AS cargosAcumulados,
                                        SUM(CASE WHEN MOV.year = {0} AND MOV.tm IN (2, 4) AND MOV.mes <= {1} THEN MOV.monto ELSE 0 END) AS abonosAcumulados
                                    FROM
                                        sc_movpol AS MOV
                                    WHERE
                                        MOV.cta IN {3} AND
                                        (
                                            (MOV.year < {0}) OR
                                            (MOV.year = {0} AND MOV.mes <= {1})
                                        ) AND
                                        (SELECT TOP 1 status FROM sc_polizas POL WHERE MOV.year = POL.year AND MOV.mes = POL.mes AND MOV.tp = POL.tp AND MOV.poliza = POL.poliza) = 'A'
                                    GROUP BY
                                        MOV.cta, MOV.scta, MOV.sscta, MOV.area, MOV.cuenta_oc, MOV.itm
                                    ORDER BY
                                        MOV.cta, MOV.scta, MOV.sscta, MOV.area, MOV.cuenta_oc, MOV.itm
                                ) AS Y
                            GROUP BY
                                anio, Y.cta, Y.scta, Y.sscta, Y.area, cuenta, Y.itm", anio, mes, corteMes.id, listaCuentas.ToParamInValue());
                    odbc.parametros.AddRange(listaCuentas.Select(s => new OdbcParameterDTO() { nombre = "cta", tipo = OdbcType.Int, valor = s }));

                    saldos = _contextEnkontrol.Select<tblEF_SaldoCC>(getEnkontrolEnumADM(), odbc);
                }

                foreach (var sal in saldos)
                {
                    DataRow row = dt.NewRow();

                    row["id"] = sal.id;
                    row["anio"] = sal.anio;
                    row["cta"] = sal.cta;
                    row["scta"] = sal.scta;
                    row["sscta"] = sal.sscta;
                    row["saldoInicial"] = sal.saldoInicial;
                    row["cargosMes"] = sal.cargosMes;
                    row["abonosMes"] = sal.abonosMes;
                    row["cargosAcumulados"] = sal.cargosAcumulados;
                    row["abonosAcumulados"] = sal.abonosAcumulados;
                    row["cc"] = sal.cc ?? (object)DBNull.Value;
                    row["area"] = sal.area ?? (object)DBNull.Value;
                    row["cuenta"] = sal.cuenta ?? (object)DBNull.Value;
                    row["areaCuenta"] = sal.areaCuenta ?? (object)DBNull.Value;
                    row["corteMesID"] = sal.corteMesID;
                    row["itm"] = sal.itm;
                    row["estatus"] = sal.estatus;

                    dt.Rows.Add(row);
                }

                using (SqlConnection cn = new SqlConnection(stringCon))
                {
                    cn.Open();

                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(cn))
                    {
                        bulkCopy.DestinationTableName = "dbo.tblEF_SaldoCC";
                        bulkCopy.WriteToServer(dt);
                    }

                    cn.Close();
                }

                //
                DataTable dtIndicadores = new DataTable();

                dtIndicadores.Columns.Add("id", System.Type.GetType("System.Int32"));
                dtIndicadores.Columns.Add("corteMesId", System.Type.GetType("System.Int32"));
                dtIndicadores.Columns.Add("cc", System.Type.GetType("System.String"));
                dtIndicadores.Columns.Add("area", System.Type.GetType("System.Int32"));
                dtIndicadores.Columns.Add("cuenta", System.Type.GetType("System.Int32"));
                dtIndicadores.Columns.Add("areaCuenta", System.Type.GetType("System.String"));
                dtIndicadores.Columns.Add("activoCirculante", System.Type.GetType("System.Decimal"));
                dtIndicadores.Columns.Add("pasivoCirculante", System.Type.GetType("System.Decimal"));
                dtIndicadores.Columns.Add("pasivoTotal", System.Type.GetType("System.Decimal"));
                dtIndicadores.Columns.Add("activoTotal", System.Type.GetType("System.Decimal"));
                dtIndicadores.Columns.Add("capitalContable", System.Type.GetType("System.Decimal"));
                dtIndicadores.Columns.Add("inventario", System.Type.GetType("System.Decimal"));
                dtIndicadores.Columns.Add("ebitda", System.Type.GetType("System.Decimal"));
                dtIndicadores.Columns.Add("registroActivo", System.Type.GetType("System.Boolean"));

                if (vSesiones.sesionEmpresaActual != (int)EmpresaEnum.Arrendadora)
                {
                    foreach (var sal in saldos.GroupBy(x => x.cc))
                    {
                        var depreciaciones = sal
                            .Where(x =>
                                ((x.cta == 5000 && x.scta == 10) ||
                                (x.cta == 5280 && x.scta == 10)) && x.anio >= fechaAnioMes.Year)
                            .Sum(x => (decimal?)x.cargosMes + (decimal?)x.abonosMes);

                        var perdidaOperacion = sal
                            .Where(x =>
                                (
                                    (x.cta == 4000) ||
                                    (x.cta == 4001) ||
                                    (x.cta == 5000 && !((x.cta == 5000 && x.scta == 18) || (x.cta == 5000 && x.scta == 10) || (x.cta == 5000 && x.scta == 6 && x.sscta == 50))) ||
                                    (x.cta == 5000 && x.scta == 18 && !(x.cta == 5000 && x.scta == 18 && x.cc == "994")) ||
                                    (x.cta == 5002) ||
                                    (x.cta == 5000 && x.scta == 10) ||
                                    (x.cta == 5280 && !((x.cta == 5280 && x.scta == 18) || (x.cta == 5280 && x.cc == "A03") || (x.cta == 5280 && x.scta == 10))) ||
                                    (x.cta == 5280 && x.scta == 18) ||
                                    (x.cta == 5280 && x.cc == "A03") ||
                                    (x.cta == 5280 && x.scta == 10)
                                ) && x.anio >= fechaAnioMes.Year)
                            .Sum(x => (decimal?)x.cargosMes + (decimal?)x.abonosMes) * -1;

                        var ebitda = depreciaciones + perdidaOperacion;

                        DataRow rowIndicador = dtIndicadores.NewRow();

                        rowIndicador["id"] = 0;
                        rowIndicador["corteMesId"] = corteMes.id;
                        rowIndicador["cc"] = sal.Key;
                        rowIndicador["area"] = DBNull.Value;
                        rowIndicador["cuenta"] = DBNull.Value;
                        rowIndicador["areaCuenta"] = DBNull.Value;
                        rowIndicador["activoCirculante"] = 0M;
                        rowIndicador["pasivoCirculante"] = 0M;
                        rowIndicador["pasivoTotal"] = 0M;
                        rowIndicador["activoTotal"] = 0M;
                        rowIndicador["capitalContable"] = 0M;
                        rowIndicador["inventario"] = 0M;
                        rowIndicador["ebitda"] = ebitda;
                        rowIndicador["registroActivo"] = true;

                        dtIndicadores.Rows.Add(rowIndicador);
                    }
                }
                else
                {
                    foreach (var sal in saldos.GroupBy(x => x.areaCuenta))
                    {
                        var depreciaciones = sal
                            .Where(x =>
                                (x.cta == 5000 && x.scta == 10) && x.anio >= fechaAnioMes.Year)
                            .Sum(x => (decimal?)x.cargosMes + (decimal?)x.abonosMes);

                        var perdidaOperacion = sal
                            .Where(x =>
                                (x.anio >= fechaAnioMes.Year && x.cta == 4000 && !(x.cta == 4000 && (x.scta == 4 || x.scta == 8))) ||
                                (x.anio >= fechaAnioMes.Year && x.cta == 5000 && !(x.cta == 5000 && x.scta == 10)) ||
                                (x.anio >= fechaAnioMes.Year && x.cta == 5000 && x.scta == 10) ||
                                (x.anio >= fechaAnioMes.Year && x.cta == 5280) ||
                                (x.anio >= fechaAnioMes.Year && x.cta == 5901 && x.scta == 6) ||
                                (x.cta == 1127))
                            .Sum(x => (decimal?)x.cargosMes + (decimal?)x.abonosMes);

                        DataRow rowIndicador = dtIndicadores.NewRow();

                        rowIndicador["id"] = 0;
                        rowIndicador["corteMesId"] = corteMes.id;
                        rowIndicador["cc"] = DBNull.Value;
                        rowIndicador["area"] = DBNull.Value;
                        rowIndicador["cuenta"] = DBNull.Value;
                        rowIndicador["areaCuenta"] = sal.Key;
                        rowIndicador["activoCirculante"] = 0M;
                        rowIndicador["pasivoCirculante"] = 0M;
                        rowIndicador["pasivoTotal"] = 0M;
                        rowIndicador["activoTotal"] = 0M;
                        rowIndicador["capitalContable"] = 0M;
                        rowIndicador["inventario"] = 0M;
                        rowIndicador["ebitda"] = depreciaciones + perdidaOperacion;
                        rowIndicador["registroActivo"] = true;

                        dtIndicadores.Rows.Add(rowIndicador);
                    }
                }

                using (SqlConnection cn = new SqlConnection(stringCon))
                {
                    cn.Open();

                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(cn))
                    {
                        bulkCopy.DestinationTableName = "dbo.tblEF_Indicadores";
                        bulkCopy.WriteToServer(dtIndicadores);
                    }

                    cn.Close();
                }
                //
                #endregion

                corteMes.estatus = true;
                _context.SaveChanges();

                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, "EstadosFinancierosController", "guardarBalanza", e, AccionEnum.AGREGAR, 0, fechaAnioMes);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);

            }

            return resultado;
        }

        private EnkontrolEnum getEnkontrolEnumADM()
        {
            var baseDatos = new EnkontrolEnum();

            if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan)
            {
                baseDatos = EnkontrolEnum.CplanProd;
            }
            else if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
            {
                baseDatos = EnkontrolEnum.ArrenProd;
            }
            else if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.EICI)
            {
                baseDatos = EnkontrolEnum.CplanEici;
            }
            else if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Integradora)
            {
                baseDatos = EnkontrolEnum.CplanIntegradora;
            }
            else
            {
                throw new Exception("Empresa distinta a Construplan, Arrendadora, EICI e INTEGRADORA.");
            }

            return baseDatos;
        }

        private MainContextEnum getSIGOPLANEnum()
        {
            var baseDatos = new MainContextEnum();

            if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan)
            {
                baseDatos = MainContextEnum.Construplan;
            }
            else if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
            {
                baseDatos = MainContextEnum.Arrendadora;
            }
            else if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.EICI)
            {
                baseDatos = MainContextEnum.EICI;
            }
            else if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Integradora)
            {
                baseDatos = MainContextEnum.INTEGRADORA;
            }
            else
            {
                throw new Exception("Empresa distinta a Construplan, Arrendadora, EICI e INTEGRADORA.");
            }

            return baseDatos;
        }

        public Dictionary<string, object> FillComboCC()
        {
            try
            {
                var listaCC = new List<ComboDTO>
                {
                    new ComboDTO { Value = "Todoss", Text = "Todos los CC" }
                };
                //Se cargan nomás los centros de costo de construplan por ser los proyectos.
                listaCC.AddRange(_contextEnkontrol.Select<ComboDTO>(EnkontrolEnum.CplanProd, new OdbcConsultaDTO()
                {
                    consulta = @"
                    SELECT
                        cc as Value,
                        (cc + ' - ' + descripcion) as Text
                    FROM cc
                    ORDER BY cc"
                }));

                resultado.Add(ITEMS, listaCC);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                LogError(0, 0, "EstadosFinancierosController", "FillComboCC", e, AccionEnum.CONSULTA, 0, null);
            }

            return resultado;
        }

        public Dictionary<string, object> calcularEstadoResultados(List<EmpresaEnum> listaEmpresas, DateTime fechaAnioMes, List<string> listaCC)
        {
            try
            {
                List<CentroCostoAreaCuentaDTO> listaAreaCuenta = null;
                //List<string> listaAC = null;

                if (listaCC == null)
                {
                    listaCC = new List<string>();
                    listaAreaCuenta = new List<CentroCostoAreaCuentaDTO>();
                    //listaAC = new List<string>();
                }
                else
                {
                    using (var _contextConstruplan = new MainContext(EmpresaEnum.Construplan))
                    {
                        listaAreaCuenta = _contextConstruplan.tblP_CC.ToList()
                            .Where(x =>
                                (listaCC != null ? listaCC.Contains(x.cc) : true))
                            .Select(x => new CentroCostoAreaCuentaDTO
                            {
                                area = x.area,
                                cuenta = x.cuenta,
                                areaCuenta = x.area.ToString() + "-" + x.cuenta
                            }).ToList();

                        //listaAC = _contextConstruplan.tblP_CC.ToList()
                        //    .Where(x => listaCC.Contains(x.cc))
                        //    .Select(x => x.area.ToString() + "-" + x.cuenta.ToString()).ToList();
                    }
                }

                var anio = fechaAnioMes.Year;
                var mes = fechaAnioMes.Month;
                var nombreMes = (new DateTime(anio, mes, 1).ToString("MMMM", CultureInfo.CreateSpecificCulture("es"))).ToUpper();
                var listaDatosDataTable1 = new List<Dictionary<string, object>>();

                if (listaEmpresas == null)
                {
                    throw new Exception("Debe seleccionar por lo menos una empresa.");
                }

                #region Columnas
                var tipoColumnas = new Dictionary<string, int>() { { "Otro", 0 }, { "Monto", 1 }, { "Porcentaje", 2 } };
                var listaColumnas = new List<Tuple<string, string, int>> { new Tuple<string, string, int>("ordenReporte", "", tipoColumnas["Otro"]), new Tuple<string, string, int>("concepto", "CONCEPTOS", tipoColumnas["Otro"]) };
                int contadorColumna = 1;

                switch (listaEmpresas.Count)
                {
                    case 1:
                        #region Year actual
                        foreach (var empresa in listaEmpresas)
                        {
                            listaColumnas.Add(Tuple.Create<string, string, int>(
                                "columna" + contadorColumna, //data
                                nombreMes + " " + anio, //title
                                tipoColumnas["Monto"] //render
                            ));

                            contadorColumna++;
                        }

                        foreach (var empresa in listaEmpresas)
                        {
                            listaColumnas.Add(Tuple.Create<string, string, int>(
                                "columna" + contadorColumna, //data
                                "%", //title
                                tipoColumnas["Porcentaje"] //render
                            ));

                            contadorColumna++;
                        }
                        #endregion

                        #region Year anterior
                        foreach (var empresa in listaEmpresas)
                        {
                            listaColumnas.Add(Tuple.Create<string, string, int>(
                                "columna" + contadorColumna, //data
                                nombreMes + " " + (anio - 1), //title
                                tipoColumnas["Monto"] //render
                            ));

                            contadorColumna++;
                        }

                        foreach (var empresa in listaEmpresas)
                        {
                            listaColumnas.Add(Tuple.Create<string, string, int>(
                                "columna" + contadorColumna, //data
                                "%", //title
                                tipoColumnas["Porcentaje"] //render
                            ));

                            contadorColumna++;
                        }
                        #endregion

                        #region Variaciones
                        foreach (var empresa in listaEmpresas)
                        {
                            listaColumnas.Add(Tuple.Create<string, string, int>(
                                "columna" + contadorColumna, //data
                                "VARIACIÓN", //title
                                tipoColumnas["Monto"] //render
                            ));

                            contadorColumna++;
                        }
                        #endregion

                        #region Year actual acumulado
                        foreach (var empresa in listaEmpresas)
                        {
                            listaColumnas.Add(Tuple.Create<string, string, int>(
                                "columna" + contadorColumna, //data
                                nombreMes + " " + anio, //title
                                tipoColumnas["Monto"] //render
                            ));

                            contadorColumna++;
                        }

                        foreach (var empresa in listaEmpresas)
                        {
                            listaColumnas.Add(Tuple.Create<string, string, int>(
                                "columna" + contadorColumna, //data
                                "%", //title
                                tipoColumnas["Porcentaje"] //render
                            ));

                            contadorColumna++;
                        }
                        #endregion

                        #region Year anterior acumulado
                        foreach (var empresa in listaEmpresas)
                        {
                            listaColumnas.Add(Tuple.Create<string, string, int>(
                                "columna" + contadorColumna, //data
                                nombreMes + " " + (anio - 1), //title
                                tipoColumnas["Monto"] //render
                            ));

                            contadorColumna++;
                        }

                        foreach (var empresa in listaEmpresas)
                        {
                            listaColumnas.Add(Tuple.Create<string, string, int>(
                                "columna" + contadorColumna, //data
                                "%", //title
                                tipoColumnas["Porcentaje"] //render
                            ));

                            contadorColumna++;
                        }
                        #endregion

                        #region Variaciones acumulado
                        foreach (var empresa in listaEmpresas)
                        {
                            listaColumnas.Add(Tuple.Create<string, string, int>(
                                "columna" + contadorColumna, //data
                                "VARIACIONES", //title
                                tipoColumnas["Monto"] //render
                            ));

                            contadorColumna++;
                        }
                        #endregion
                        break;
                    default:
                        #region Monto empresa
                        foreach (var empresa in listaEmpresas)
                        {
                            listaColumnas.Add(Tuple.Create<string, string, int>(
                                "columna" + contadorColumna, //data
                                listaEmpresas.Count() > 1 ? empresa.GetDescription() : "", //title
                                tipoColumnas["Monto"] //render
                            ));

                            contadorColumna++;

                            listaColumnas.Add(Tuple.Create<string, string, int>(
                                "columna" + contadorColumna, //data
                                "", //title
                                tipoColumnas["Monto"] //render
                            ));

                            contadorColumna++;
                        }
                        #endregion

                        #region Consolidado
                        listaColumnas.Add(Tuple.Create<string, string, int>(
                                "columna" + contadorColumna, //data
                                "MONTO", //title
                                tipoColumnas["Monto"] //render
                        ));

                        contadorColumna++;

                        listaColumnas.Add(Tuple.Create<string, string, int>(
                                "columna" + contadorColumna, //data
                                "%", //title
                                tipoColumnas["Porcentaje"] //render
                        ));

                        contadorColumna++;
                        #endregion

                        #region Year anterior
                        listaColumnas.Add(Tuple.Create<string, string, int>(
                            "columna" + contadorColumna, //data
                            "MONTO", //title
                            tipoColumnas["Monto"] //render
                        ));

                        contadorColumna++;

                        listaColumnas.Add(Tuple.Create<string, string, int>(
                            "columna" + contadorColumna, //data
                            "%", //title
                            tipoColumnas["Porcentaje"] //render
                        ));

                        contadorColumna++;
                        #endregion
                        break;
                }
                #endregion

                #region Datos
                switch (listaEmpresas.Count)
                {
                    case 1:
                        #region Datos mensuales y acumulados
                        {
                            var empresa = listaEmpresas.First();
                            var tipoResultado = _context.tblEF_TipoResultado.First(x => x.id == (int)TipoResultadoEnum.MENSUAL_Y_ACUMULADO);

                            using (var _ctxEmpresa = new MainContext(empresa))
                            {
                                var grupos = _ctxEmpresa.tblEF_GrupoConceptoFlujo.Where(x => x.estatus).OrderBy(x => x.id).ToList();

                                var renglones = new List<EstadoResultadoDTO>();
                                var orden = 0;

                                var corte = _ctxEmpresa.tblEF_CorteMes.FirstOrDefault(x => x.anio == fechaAnioMes.Year && x.mes == fechaAnioMes.Month && x.estatus);
                                var corteID = corte == null ? 0 : corte.id;

                                var corteAnterior = _ctxEmpresa.tblEF_CorteMes.FirstOrDefault(x => x.anio == (fechaAnioMes.Year - 1) && x.mes == fechaAnioMes.Month && x.estatus);
                                var corteAnteriorID = corteAnterior == null ? 0 : corteAnterior.id;

                                EstadoResultadoDTO renglonTotalGrupoAnterior = null;

                                foreach (var grupo in grupos)
                                {
                                    var renglonesGrupo = new List<EstadoResultadoDTO>();

                                    foreach (var concepto in grupo.conceptos.Where(x => x.estatus).OrderBy(x => x.ordenReporte))
                                    {
                                        if (concepto.id != 38)
                                        {
                                            var montoMesYearActual = 0M;
                                            var montoMesYearAnterior = 0M;
                                            var montoAcumuladoYearActual = 0M;
                                            var montoAcumuladoYearAnterior = 0M;

                                            var cuentasExcepcionConcepto = _ctxEmpresa.tblEF_CuentaExcepcionConcepto.Where(x => x.conceptoID == concepto.id && x.estatus).ToList();

                                            foreach (var cuenta in concepto.cuentas.Where(x => x.estatus))
                                            {
                                                var saldosCC = new List<EFSaldosCCDTO>();

                                                switch (empresa)
                                                {
                                                    case EmpresaEnum.Integradora:
                                                    case EmpresaEnum.EICI:
                                                    case EmpresaEnum.Construplan:
                                                        saldosCC = _ctxEmpresa.tblEF_SaldoCC
                                                            .Select(x => new EFSaldosCCDTO
                                                            {
                                                                anio = x.anio,
                                                                cta = x.cta,
                                                                scta = x.scta,
                                                                sscta = x.sscta,
                                                                ccCtaSctaSscta = (string.IsNullOrEmpty(x.cc) ? "" : x.cc + "-") + x.cta + "-" + x.scta + "-" + x.sscta,
                                                                cargosMes = x.cargosMes * (-1),
                                                                abonosMes = x.abonosMes * (-1),
                                                                cargoAcumulado = x.cargosAcumulados * (-1),
                                                                abonoAcumulado = x.abonosAcumulados * (-1),
                                                                cc = x.cc,
                                                                //areaCuenta = x.area.HasValue ? x.area.Value + "-" + x.cuenta.Value : null,
                                                                corteMesID = x.corteMesID,
                                                                corte = x.corte,
                                                                estatus = x.estatus
                                                            })
                                                            .Where(x =>
                                                                (
                                                                    (x.corte.anio == anio) ||
                                                                    (x.corte.anio == (anio - 1))
                                                                ) &&
                                                                x.corte.mes == mes &&
                                                                x.corte.estatus &&
                                                                x.cta == cuenta.cta &&
                                                                (cuenta.scta > 0 ? x.scta == cuenta.scta : true) &&
                                                                (cuenta.sscta > 0 ? x.sscta == cuenta.sscta : true) &&
                                                                (listaCC.Count > 0 ? listaCC.Contains(x.cc) : true) &&
                                                                (string.IsNullOrEmpty(cuenta.cc) ? true : x.cc == cuenta.cc) &&
                                                                x.estatus
                                                            ).ToList();

                                                        foreach (var item in cuentasExcepcionConcepto)
                                                        {
                                                            saldosCC.RemoveAll(x =>
                                                                (item.cta != 0 ? item.cta == x.cta : true) &&
                                                                (item.scta != 0 ? item.scta == x.scta : true) &&
                                                                (item.sscta != 0 ? item.sscta == x.sscta : true) &&
                                                                (!string.IsNullOrEmpty(item.cc) ? item.cc == x.cc : true));
                                                        }
                                                        break;
                                                    case EmpresaEnum.Arrendadora:
                                                        saldosCC = _ctxEmpresa.tblEF_SaldoCC
                                                            .Select(x => new EFSaldosCCDTO
                                                            {
                                                                anio = x.anio,
                                                                cta = x.cta,
                                                                scta = x.scta,
                                                                sscta = x.sscta,
                                                                ccCtaSctaSscta = (string.IsNullOrEmpty(x.cc) ? "" : x.cc + "-") + x.cta + "-" + x.scta + "-" + x.sscta,
                                                                cargosMes = x.cargosMes * (-1),
                                                                abonosMes = x.abonosMes * (-1),
                                                                cargoAcumulado = x.cargosAcumulados * (-1),
                                                                abonoAcumulado = x.abonosAcumulados * (-1),
                                                                cc = x.cc,
                                                                areaCuenta = x.area.HasValue ? x.area.Value + "-" + x.cuenta.Value : null,
                                                                area = x.area,
                                                                cuenta = x.cuenta,
                                                                corteMesID = x.corteMesID,
                                                                corte = x.corte,
                                                                estatus = x.estatus
                                                            })
                                                            .Where(x =>
                                                                (
                                                                    (x.corte.anio == anio) ||
                                                                    (x.corte.anio == (anio - 1))
                                                                ) &&
                                                                x.corte.mes == mes &&
                                                                x.corte.estatus &&
                                                                x.cta == cuenta.cta &&
                                                                (cuenta.scta > 0 ? x.scta == cuenta.scta : true) &&
                                                                (cuenta.sscta > 0 ? x.sscta == cuenta.sscta : true) &&
                                                                (listaAreaCuenta.Count > 0 ? x.area.HasValue : true) &&
                                                                (string.IsNullOrEmpty(cuenta.cc) ? true : x.areaCuenta == cuenta.cc) &&
                                                                x.estatus
                                                            ).ToList();

                                                        if (listaAreaCuenta.Count > 0)
                                                        {
                                                            saldosCC = (
                                                            from sal in saldosCC
                                                            join ac in listaAreaCuenta on new { area = sal.area.Value, cuenta = sal.cuenta.Value } equals new { area = ac.area, cuenta = ac.cuenta }
                                                            select sal
                                                        ).ToList();
                                                        }

                                                        foreach (var item in cuentasExcepcionConcepto)
                                                        {
                                                            saldosCC.RemoveAll(x =>
                                                                (item.cta != 0 ? item.cta == x.cta : true) &&
                                                                (item.scta != 0 ? item.scta == x.scta : true) &&
                                                                (item.sscta != 0 ? item.sscta == x.sscta : true) &&
                                                                (!string.IsNullOrEmpty(item.cc) ? item.cc == x.areaCuenta : true));
                                                        }
                                                        break;
                                                }

                                                montoMesYearActual += saldosCC.Where(x => x.corte.anio == anio && x.corte.mes == mes).Sum(x => (decimal?)x.cargosMes + (decimal?)x.abonosMes) ?? 0;
                                                montoMesYearAnterior += saldosCC.Where(x => x.corte.anio < anio && x.corte.mes == mes).Sum(x => (decimal?)x.cargosMes + (decimal?)x.abonosMes) ?? 0;
                                                montoAcumuladoYearActual += saldosCC.Where(x => x.corte.anio == anio).Sum(x => (decimal?)x.cargoAcumulado + (decimal?)x.abonoAcumulado) ?? 0;
                                                montoAcumuladoYearAnterior += saldosCC.Where(x => x.corte.anio < anio).Sum(x => (decimal?)x.cargoAcumulado + (decimal?)x.abonoAcumulado) ?? 0;

                                                if (concepto.invertirSigno && concepto.id == 2 && empresa == EmpresaEnum.Arrendadora)
                                                {
                                                    montoMesYearActual *= -1;
                                                    montoMesYearAnterior *= -1;
                                                    montoAcumuladoYearActual *= -1;
                                                    montoAcumuladoYearAnterior *= -1;
                                                }
                                            }

                                            var renglon = new EstadoResultadoDTO();
                                            renglon.concepto = concepto.concepto;
                                            renglon.tipoOperacion = (TipoOperacionEnum)concepto.tipoOperacion;
                                            renglon.montoMesAnioActual = concepto.tipoOperacion == TipoOperacionEnum.resta ? (Math.Truncate(Math.Round(montoMesYearActual / 1000))) * (-1) : (Math.Truncate(Math.Round(montoMesYearActual / 1000)));
                                            renglon.montoMesAnioAnterior = concepto.tipoOperacion == TipoOperacionEnum.resta ? (Math.Truncate(Math.Round(montoMesYearAnterior / 1000))) * (-1) : (Math.Truncate(Math.Round(montoMesYearAnterior / 1000)));
                                            renglon.montoMesAcumuladoAnioActual = concepto.tipoOperacion == TipoOperacionEnum.resta ? (Math.Truncate(Math.Round(montoAcumuladoYearActual / 1000))) * (-1) : (Math.Truncate(Math.Round(montoAcumuladoYearActual / 1000)));
                                            renglon.montoMesAcumuladoAnioAnterior = concepto.tipoOperacion == TipoOperacionEnum.resta ? (Math.Truncate(Math.Round(montoAcumuladoYearAnterior / 1000))) * (-1) : (Math.Truncate(Math.Round(montoAcumuladoYearAnterior / 1000)));
                                            renglon.variaciones = renglon.montoMesAnioActual - renglon.montoMesAnioAnterior;
                                            renglon.variacionesAcumulado = renglon.montoMesAcumuladoAnioActual - renglon.montoMesAcumuladoAnioAnterior;
                                            renglon.grupoReporteID = grupo.id;
                                            renglon.ordenReporte = ++orden;
                                            renglon.detalleID = concepto.tipoEnlaceId;
                                            renglonesGrupo.Add(renglon);
                                        }
                                        else 
                                        {
                                            var resultadoEjercicio = _ctxEmpresa.tblEF_Balanza.Where(x => x.corteMesID == corteID && x.cta == 3110 && x.scta == 0 && x.sscta == 0).ToList();
                                            var resultadoEjercicioAnterior = _ctxEmpresa.tblEF_Balanza.Where(x => x.corteMesID == corteAnteriorID && x.cta == 3110 && x.scta == 0 && x.sscta == 0).ToList();
                                            var montoMesYearActual = (resultadoEjercicio == null || resultadoEjercicio.Count() < 1) ? 0 : resultadoEjercicio.Sum(x => x.cargos + x.abonos);
                                            var montoMesYearAnterior = (resultadoEjercicioAnterior == null || resultadoEjercicioAnterior.Count() < 1) ? 0 : resultadoEjercicioAnterior.Sum(x => x.cargos + x.abonos);
                                            var montoAcumuladoYearActual = (resultadoEjercicio == null || resultadoEjercicio.Count() < 1) ? 0 : resultadoEjercicio.Sum(x => x.saldoActual);
                                            var montoAcumuladoYearAnterior = (resultadoEjercicioAnterior == null || resultadoEjercicioAnterior.Count() < 1) ? 0 : resultadoEjercicioAnterior.Sum(x => x.saldoActual);
                                            
                                            var renglon = new EstadoResultadoDTO();
                                            renglon.concepto = concepto.concepto;
                                            renglon.tipoOperacion = (TipoOperacionEnum)concepto.tipoOperacion;
                                            renglon.montoMesAnioActual = concepto.tipoOperacion == TipoOperacionEnum.resta ? (Math.Truncate(Math.Round(montoMesYearActual / 1000))) * (-1) : (Math.Truncate(Math.Round(montoMesYearActual / 1000)));
                                            renglon.montoMesAnioAnterior = concepto.tipoOperacion == TipoOperacionEnum.resta ? (Math.Truncate(Math.Round(montoMesYearAnterior / 1000))) * (-1) : (Math.Truncate(Math.Round(montoMesYearAnterior / 1000)));
                                            renglon.montoMesAcumuladoAnioActual = concepto.tipoOperacion == TipoOperacionEnum.resta ? (Math.Truncate(Math.Round(montoAcumuladoYearActual / 1000))) * (-1) : (Math.Truncate(Math.Round(montoAcumuladoYearActual / 1000)));
                                            renglon.montoMesAcumuladoAnioAnterior = concepto.tipoOperacion == TipoOperacionEnum.resta ? (Math.Truncate(Math.Round(montoAcumuladoYearAnterior / 1000))) * (-1) : (Math.Truncate(Math.Round(montoAcumuladoYearAnterior / 1000)));
                                            renglon.variaciones = renglon.montoMesAnioActual - renglon.montoMesAnioAnterior;
                                            renglon.variacionesAcumulado = renglon.montoMesAcumuladoAnioActual - renglon.montoMesAcumuladoAnioAnterior;
                                            renglon.grupoReporteID = grupo.id;
                                            renglon.ordenReporte = ++orden;
                                            renglon.detalleID = concepto.tipoEnlaceId;
                                            renglonesGrupo.Add(renglon);
                                        }
                                    }

                                    var renglonTotal = new EstadoResultadoDTO();
                                    renglonTotal.concepto = grupo.descripcion;
                                    renglonTotal.tipoOperacion = TipoOperacionEnum.total;

                                    if (renglonTotalGrupoAnterior != null)
                                    {
                                        renglonTotal.montoMesAnioActual = renglonTotalGrupoAnterior.montoMesAnioActual;
                                        renglonTotal.montoMesAnioAnterior = renglonTotalGrupoAnterior.montoMesAnioAnterior;
                                        renglonTotal.montoMesAcumuladoAnioActual = renglonTotalGrupoAnterior.montoMesAcumuladoAnioActual;
                                        renglonTotal.montoMesAcumuladoAnioAnterior = renglonTotalGrupoAnterior.montoMesAcumuladoAnioAnterior;

                                        renglonTotal.porcentajeMesAnioActual = renglonTotalGrupoAnterior.porcentajeMesAnioActual;
                                        renglonTotal.porcentajeMesAnioAnterior = renglonTotalGrupoAnterior.porcentajeMesAnioAnterior;
                                        renglonTotal.porcentajeMesAcumuladoAnioActual = renglonTotalGrupoAnterior.porcentajeMesAcumuladoAnioActual;
                                        renglonTotal.porcentajeMesAcumuladoAnioAnterior = renglonTotalGrupoAnterior.porcentajeMesAcumuladoAnioAnterior;

                                        renglonTotal.variaciones = renglonTotalGrupoAnterior.variaciones;
                                        renglonTotal.variacionesAcumulado = renglonTotalGrupoAnterior.variacionesAcumulado;
                                    }

                                    foreach (var item in renglonesGrupo)
                                    {
                                        renglonTotal.montoMesAnioActual += item.tipoOperacion == TipoOperacionEnum.suma ? item.montoMesAnioActual : (item.montoMesAnioActual * -1);
                                        renglonTotal.montoMesAnioAnterior += item.tipoOperacion == TipoOperacionEnum.suma ? item.montoMesAnioAnterior : (item.montoMesAnioAnterior * -1);
                                        renglonTotal.montoMesAcumuladoAnioActual += item.tipoOperacion == TipoOperacionEnum.suma ? item.montoMesAcumuladoAnioActual : (item.montoMesAcumuladoAnioActual * -1);
                                        renglonTotal.montoMesAcumuladoAnioAnterior += item.tipoOperacion == TipoOperacionEnum.suma ? item.montoMesAcumuladoAnioAnterior : (item.montoMesAcumuladoAnioAnterior * -1);
                                    }
                                    renglonTotal.variaciones = renglonTotal.montoMesAnioActual - renglonTotal.montoMesAnioAnterior;
                                    renglonTotal.variacionesAcumulado = renglonTotal.montoMesAcumuladoAnioActual - renglonTotal.montoMesAcumuladoAnioAnterior;
                                    renglonTotal.grupoReporteID = grupo.id;
                                    renglonTotal.ordenReporte = ++orden;
                                    renglonTotal.flagGrupoReporte = true;
                                    foreach (var item in renglonesGrupo)
                                    {
                                        if (renglonTotalGrupoAnterior == null)
                                        {
                                            item.porcentajeMesAnioActual = Math.Round((item.montoMesAnioActual * 100) / (renglonTotal.montoMesAnioActual != 0 ? renglonTotal.montoMesAnioActual : 1), 2);
                                            item.porcentajeMesAnioAnterior = Math.Round((item.montoMesAnioAnterior * 100) / (renglonTotal.montoMesAnioAnterior != 0 ? renglonTotal.montoMesAnioAnterior : 1), 2);
                                            item.porcentajeMesAcumuladoAnioActual = Math.Round((item.montoMesAcumuladoAnioActual * 100) / (renglonTotal.montoMesAcumuladoAnioActual != 0 ? renglonTotal.montoMesAcumuladoAnioActual : 1), 2);
                                            item.porcentajeMesAcumuladoAnioAnterior = Math.Round((item.montoMesAcumuladoAnioAnterior * 100) / (renglonTotal.montoMesAcumuladoAnioAnterior != 0 ? renglonTotal.montoMesAcumuladoAnioAnterior : 1), 2);
                                        }
                                        else
                                        {
                                            item.porcentajeMesAnioActual = Math.Round((item.montoMesAnioActual * renglonTotalGrupoAnterior.porcentajeMesAnioActual) / (renglonTotalGrupoAnterior.montoMesAnioActual != 0 ? renglonTotalGrupoAnterior.montoMesAnioActual : 1), 2);
                                            item.porcentajeMesAnioAnterior = Math.Round((item.montoMesAnioAnterior * renglonTotalGrupoAnterior.porcentajeMesAnioAnterior) / (renglonTotalGrupoAnterior.montoMesAnioAnterior != 0 ? renglonTotalGrupoAnterior.montoMesAnioAnterior : 1), 2);
                                            item.porcentajeMesAcumuladoAnioActual = Math.Round((item.montoMesAcumuladoAnioActual * renglonTotalGrupoAnterior.porcentajeMesAcumuladoAnioActual) / (renglonTotalGrupoAnterior.montoMesAcumuladoAnioActual != 0 ? renglonTotalGrupoAnterior.montoMesAcumuladoAnioActual : 1), 2);
                                            item.porcentajeMesAcumuladoAnioAnterior = Math.Round((item.montoMesAcumuladoAnioAnterior * renglonTotalGrupoAnterior.porcentajeMesAcumuladoAnioAnterior) / (renglonTotalGrupoAnterior.montoMesAcumuladoAnioAnterior != 0 ? renglonTotalGrupoAnterior.montoMesAcumuladoAnioAnterior : 1), 2);
                                        }

                                        renglonTotal.porcentajeMesAnioActual += item.tipoOperacion == TipoOperacionEnum.suma ? item.porcentajeMesAnioActual : item.porcentajeMesAnioActual * -1;
                                        renglonTotal.porcentajeMesAnioAnterior += item.tipoOperacion == TipoOperacionEnum.suma ? item.porcentajeMesAnioAnterior : item.porcentajeMesAnioAnterior * -1;
                                        renglonTotal.porcentajeMesAcumuladoAnioActual += item.tipoOperacion == TipoOperacionEnum.suma ? item.porcentajeMesAcumuladoAnioActual : item.porcentajeMesAcumuladoAnioActual * -1;
                                        renglonTotal.porcentajeMesAcumuladoAnioAnterior += item.tipoOperacion == TipoOperacionEnum.suma ? item.porcentajeMesAcumuladoAnioAnterior : item.porcentajeMesAcumuladoAnioAnterior * -1;

                                        List<string> tiposModal = new List<string> { "", "Ingreso", "Costo", "Gasto" };

                                        var renglonDataTable = new Dictionary<string, object>();
                                        renglonDataTable.Add("ordenReporte", item.ordenReporte);
                                        renglonDataTable.Add("concepto", (item.detalleID > 0 && item.detalleID < 4) ? "<a class='modal" + tiposModal[item.detalleID] + "' data-target='#modal" + tiposModal[item.detalleID] + "' data-toggle='modal" + tiposModal[item.detalleID] + "' href='#modal" + tiposModal[item.detalleID] + "' nombre-detalle='" + tiposModal[item.detalleID] + "' style='font-weight: bold;'>" + item.concepto + "</a>" : item.concepto);
                                        renglonDataTable.Add("flagGrupoReporte", false);
                                        renglonDataTable.Add("columna" + 1, item.montoMesAnioActual);
                                        renglonDataTable.Add("columna" + 2, item.porcentajeMesAnioActual);
                                        renglonDataTable.Add("columna" + 3, item.montoMesAnioAnterior);
                                        renglonDataTable.Add("columna" + 4, item.porcentajeMesAnioAnterior);
                                        renglonDataTable.Add("columna" + 5, item.variaciones);
                                        renglonDataTable.Add("columna" + 6, item.montoMesAcumuladoAnioActual);
                                        renglonDataTable.Add("columna" + 7, item.porcentajeMesAcumuladoAnioActual);
                                        renglonDataTable.Add("columna" + 8, item.montoMesAcumuladoAnioAnterior);
                                        renglonDataTable.Add("columna" + 9, item.porcentajeMesAcumuladoAnioAnterior);
                                        renglonDataTable.Add("columna" + 10, item.variacionesAcumulado);
                                        renglonDataTable.Add("detalleID", item.detalleID);
                                        listaDatosDataTable1.Add(renglonDataTable);
                                    }
                                    renglonTotalGrupoAnterior = renglonTotal;

                                    {
                                        var renglonDataTable = new Dictionary<string, object>();
                                        renglonDataTable.Add("ordenReporte", renglonTotal.ordenReporte);
                                        renglonDataTable.Add("concepto", renglonTotal.concepto);
                                        renglonDataTable.Add("flagGrupoReporte", true);
                                        renglonDataTable.Add("columna" + 1, renglonTotal.montoMesAnioActual);
                                        renglonDataTable.Add("columna" + 2, renglonTotal.porcentajeMesAnioActual);
                                        renglonDataTable.Add("columna" + 3, renglonTotal.montoMesAnioAnterior);
                                        renglonDataTable.Add("columna" + 4, renglonTotal.porcentajeMesAnioAnterior);
                                        renglonDataTable.Add("columna" + 5, renglonTotal.variaciones);
                                        renglonDataTable.Add("columna" + 6, renglonTotal.montoMesAcumuladoAnioActual);
                                        renglonDataTable.Add("columna" + 7, renglonTotal.porcentajeMesAcumuladoAnioActual);
                                        renglonDataTable.Add("columna" + 8, renglonTotal.montoMesAcumuladoAnioAnterior);
                                        renglonDataTable.Add("columna" + 9, renglonTotal.porcentajeMesAcumuladoAnioAnterior);
                                        renglonDataTable.Add("columna" + 10, renglonTotal.variacionesAcumulado);
                                        renglonDataTable.Add("detalleID", 0);
                                        listaDatosDataTable1.Add(renglonDataTable);
                                    }

                                    renglones.AddRange(renglonesGrupo);
                                    renglones.Add(renglonTotal);
                                }
                            }
                        }
                        #endregion
                        break;
                    default:
                        #region Datos consolidados
                        {
                            var tipoResultado = _context.tblEF_TipoResultado.First(x => x.id == (int)TipoResultadoEnum.MENSUAL_Y_ACUMULADO);

                            using (var _ctxCplan = new MainContext((int)EmpresaEnum.Construplan))
                            {
                                var gruposGenerales = _ctxCplan.tblEF_GrupoConsolidado.Where(x => x.estatus).OrderBy(x => x.id).ToList();

                                var conceptosCruzados = new List<tblEF_ConceptoClienteProveedor>();

                                if (listaEmpresas.Contains(EmpresaEnum.Construplan) && listaEmpresas.Contains(EmpresaEnum.Arrendadora))
                                {
                                    conceptosCruzados = _ctxCplan.tblEF_ConceptoClienteProveedor.Where(x => x.estatus).ToList();
                                }

                                var renglones = new List<EstadoResultadoDTO>();
                                var orden = 0;

                                EstadoResultadoDTO renglonTotalGrupoAnterior = null;

                                foreach (var grupoGeneral in gruposGenerales)
                                {
                                    var renglonesGrupo = new List<EstadoResultadoDTO>();

                                    foreach (var concepto in grupoGeneral.conceptos.Where(x => x.estatus).OrderBy(x => x.ordenReporte))
                                    {
                                        tblEF_ConceptoClienteProveedor conceptoCruzado = null;

                                        var resultadosPorEmpresa = new List<EstadoResultadoPorEmpresaDTO>();

                                        foreach (var empresa in listaEmpresas)
                                        {
                                            conceptoCruzado = null;
                                            using (var _ctxEmpresa = new MainContext(empresa))
                                            {
                                                tblEF_EdoFinancieroConcepto conceptoEmpresa = null;
                                                switch (empresa)
                                                {
                                                    case EmpresaEnum.Construplan:
                                                        conceptoEmpresa = _ctxEmpresa.tblEF_EdoFinancieroConcepto.FirstOrDefault(x => x.id == concepto.conceptoConstruplanId);
                                                        conceptoCruzado = conceptosCruzados.FirstOrDefault(x => x.empresaPrincipalId == (int)empresa && x.conceptoPrincipalId == concepto.conceptoConstruplanId && x.estatus);
                                                        break;
                                                    case EmpresaEnum.Arrendadora:
                                                        conceptoEmpresa = _ctxEmpresa.tblEF_EdoFinancieroConcepto.FirstOrDefault(x => x.id == concepto.conceptoArrendadoraId);
                                                        conceptoCruzado = conceptosCruzados.FirstOrDefault(x => x.empresaPrincipalId == (int)empresa && x.conceptoPrincipalId == concepto.conceptoConstruplanId && x.estatus);
                                                        break;
                                                    case EmpresaEnum.EICI:
                                                        conceptoEmpresa = _ctxEmpresa.tblEF_EdoFinancieroConcepto.FirstOrDefault(x => x.id == concepto.conceptoEiciId);
                                                        break;
                                                    case EmpresaEnum.Integradora:
                                                        conceptoEmpresa = _ctxEmpresa.tblEF_EdoFinancieroConcepto.FirstOrDefault(x => x.id == concepto.conceptoIntegradoraId);
                                                        break;
                                                }
                                                if (conceptoEmpresa == null)
                                                {
                                                    conceptoEmpresa = new tblEF_EdoFinancieroConcepto();
                                                    conceptoEmpresa.cuentas = new List<tblEF_CuentaConcepto>();
                                                }

                                                var cuentasExcepcionConcepto = _ctxEmpresa.tblEF_CuentaExcepcionConcepto.Where(x => x.conceptoID == conceptoEmpresa.id && x.estatus).ToList();

                                                var montoEmpresa = 0M;
                                                var montoYearAnterior = 0M;

                                                foreach (var cuenta in conceptoEmpresa.cuentas.Where(x => x.estatus))
                                                {
                                                    var saldosCC = new List<EFSaldosCCDTO>();

                                                    switch (empresa)
                                                    {
                                                        case EmpresaEnum.Construplan:
                                                            saldosCC = _ctxEmpresa.tblEF_SaldoCC
                                                                .Select(x => new EFSaldosCCDTO
                                                                {
                                                                    anio = x.anio,
                                                                    cta = x.cta,
                                                                    scta = x.scta,
                                                                    sscta = x.sscta,
                                                                    ccCtaSctaSscta = (string.IsNullOrEmpty(x.cc) ? "" : x.cc + "-") + x.cta + "-" + x.scta + "-" + x.sscta,
                                                                    cargosMes = x.cargosMes * (-1),
                                                                    abonosMes = x.abonosMes * (-1),
                                                                    cc = x.cc,
                                                                    //areaCuenta = x.area.HasValue ? x.area.Value + "-" + x.cuenta.Value : null,
                                                                    corteMesID = x.corteMesID,
                                                                    corte = x.corte,
                                                                    estatus = x.estatus
                                                                })
                                                                .Where(x =>
                                                                    (
                                                                        (x.corte.anio == anio) ||
                                                                        (x.corte.anio == (anio - 1))
                                                                    ) &&
                                                                    x.corte.mes == mes &&
                                                                    x.corte.estatus &&
                                                                    x.cta == cuenta.cta &&
                                                                    (cuenta.scta > 0 ? x.scta == cuenta.scta : true) &&
                                                                    (cuenta.sscta > 0 ? x.sscta == cuenta.sscta : true) &&
                                                                    (listaCC.Count > 0 ? listaCC.Contains(x.cc) : true) &&
                                                                    (string.IsNullOrEmpty(cuenta.cc) ? true : x.cc == cuenta.cc) &&
                                                                    x.estatus
                                                                ).ToList();

                                                            foreach (var item in cuentasExcepcionConcepto)
                                                            {
                                                                saldosCC.RemoveAll(x =>
                                                                    (item.cta != 0 ? item.cta == x.cta : true) &&
                                                                    (item.scta != 0 ? item.scta == x.scta : true) &&
                                                                    (item.sscta != 0 ? item.sscta == x.sscta : true) &&
                                                                    (!string.IsNullOrEmpty(item.cc) ? item.cc == x.cc : true));
                                                            }
                                                            break;
                                                        case EmpresaEnum.Arrendadora:
                                                            saldosCC = _ctxEmpresa.tblEF_SaldoCC
                                                                .Select(x => new EFSaldosCCDTO
                                                                {
                                                                    anio = x.anio,
                                                                    cta = x.cta,
                                                                    scta = x.scta,
                                                                    sscta = x.sscta,
                                                                    ccCtaSctaSscta = (string.IsNullOrEmpty(x.cc) ? "" : x.cc + "-") + x.cta + "-" + x.scta + "-" + x.sscta,
                                                                    cargosMes = x.cargosMes * (-1),
                                                                    abonosMes = x.abonosMes * (-1),
                                                                    cc = x.cc,
                                                                    areaCuenta = x.area.HasValue ? x.area.Value + "-" + x.cuenta.Value : null,
                                                                    area = x.area,
                                                                    cuenta = x.cuenta,
                                                                    corteMesID = x.corteMesID,
                                                                    corte = x.corte,
                                                                    estatus = x.estatus
                                                                })
                                                                .Where(x =>
                                                                    (
                                                                        (x.corte.anio == anio) ||
                                                                        (x.corte.anio == (anio - 1))
                                                                    ) &&
                                                                    x.corte.mes == mes &&
                                                                    x.corte.estatus &&
                                                                    x.cta == cuenta.cta &&
                                                                    (cuenta.scta > 0 ? x.scta == cuenta.scta : true) &&
                                                                    (cuenta.sscta > 0 ? x.sscta == cuenta.sscta : true) &&
                                                                    (string.IsNullOrEmpty(cuenta.cc) ? true : x.cc == cuenta.cc) &&
                                                                    (listaCC.Count > 0 ? x.area.HasValue : true) &&
                                                                    x.estatus
                                                                ).ToList();

                                                            if (listaAreaCuenta.Count > 0)
                                                            {
                                                                saldosCC = (
                                                                    from sal in saldosCC
                                                                    join ac in listaAreaCuenta on new { area = sal.area.Value, cuenta = sal.cuenta.Value } equals new { area = ac.area, cuenta = ac.cuenta }
                                                                    select sal
                                                                ).ToList();
                                                            }

                                                            foreach (var item in cuentasExcepcionConcepto)
                                                            {
                                                                saldosCC.RemoveAll(x =>
                                                                    (item.cta != 0 ? item.cta == x.cta : true) &&
                                                                    (item.scta != 0 ? item.scta == x.scta : true) &&
                                                                    (item.sscta != 0 ? item.sscta == x.sscta : true) &&
                                                                    (!string.IsNullOrEmpty(item.cc) ? item.cc == x.cc : true));
                                                            }
                                                            break;
                                                    }

                                                    montoEmpresa += saldosCC.Where(x => x.corte.anio == anio).Sum(x => (decimal?)x.cargosMes + (decimal?)x.abonosMes) ?? 0;
                                                    montoYearAnterior += saldosCC.Where(x => x.corte.anio < anio).Sum(x => (decimal?)x.cargosMes + (decimal?)x.abonosMes) ?? 0;
                                                }

                                                var resultadoPorEmpresa = new EstadoResultadoPorEmpresaDTO();
                                                resultadoPorEmpresa.montoEmpresa = concepto.tipoOperacion == TipoOperacionEnum.resta ? (Math.Truncate(Math.Round(montoEmpresa / 1000))) * (-1) : (Math.Truncate(Math.Round(montoEmpresa / 1000)));
                                                resultadoPorEmpresa.montoYearAnteriorEmpresa = concepto.tipoOperacion == TipoOperacionEnum.resta ? (Math.Truncate(Math.Round(montoYearAnterior / 1000))) * (-1) : (Math.Truncate(Math.Round(montoYearAnterior / 1000)));
                                                resultadoPorEmpresa.sumarConsolidado = true;

                                                #region cuentasCruzadas
                                                if (conceptoCruzado != null)
                                                {
                                                    using (var _ctxEmpresaCruzada = new MainContext(conceptoCruzado.empresaSecundariaId))
                                                    {
                                                        var cuentasCruzadas = _ctxEmpresaCruzada.tblEF_EdoFinancieroConcepto.FirstOrDefault(x => x.id == conceptoCruzado.conceptoId && x.estatus);
                                                        var cuentasCruzadasExcpciones = _ctxEmpresaCruzada.tblEF_CuentaExcepcionConcepto.Where(x => x.conceptoID == cuentasCruzadas.id && x.estatus).ToList();

                                                        var montoCruzado = 0M;
                                                        var montoCruzadoAnterior = 0M;

                                                        foreach (var cuenta in cuentasCruzadas.cuentas.Where(x => x.estatus))
                                                        {
                                                            var saldosCC = new List<EFSaldosCCDTO>();

                                                            switch (conceptoCruzado.empresaSecundariaId)
                                                            {
                                                                case (int)EmpresaEnum.Construplan:
                                                                    saldosCC = _ctxEmpresaCruzada.tblEF_SaldoCC
                                                                        .Select(x => new EFSaldosCCDTO
                                                                        {
                                                                            anio = x.anio,
                                                                            cta = x.cta,
                                                                            scta = x.scta,
                                                                            sscta = x.sscta,
                                                                            ccCtaSctaSscta = (string.IsNullOrEmpty(x.cc) ? "" : x.cc + "-") + x.cta + "-" + x.scta + "-" + x.sscta,
                                                                            cargosMes = x.cargosMes * (-1),
                                                                            abonosMes = x.abonosMes * (-1),
                                                                            cc = x.cc,
                                                                            //areaCuenta = x.area.HasValue ? x.area.Value + "-" + x.cuenta.Value : null,
                                                                            corteMesID = x.corteMesID,
                                                                            corte = x.corte,
                                                                            estatus = x.estatus
                                                                        })
                                                                        .Where(x =>
                                                                            (
                                                                                (x.corte.anio == anio) ||
                                                                                (x.corte.anio == (anio - 1))
                                                                            ) &&
                                                                            x.corte.mes == mes &&
                                                                            x.corte.estatus &&
                                                                            x.cta == cuenta.cta &&
                                                                            (cuenta.scta > 0 ? x.scta == cuenta.scta : true) &&
                                                                            (cuenta.sscta > 0 ? x.sscta == cuenta.sscta : true) &&
                                                                            (listaCC.Count > 0 ? listaCC.Contains(x.cc) : true) &&
                                                                            (string.IsNullOrEmpty(cuenta.cc) ? true : x.cc == cuenta.cc) &&
                                                                            x.estatus
                                                                        ).ToList();

                                                                    foreach (var item in cuentasCruzadasExcpciones)
                                                                    {
                                                                        saldosCC.RemoveAll(x =>
                                                                            (item.cta != 0 ? item.cta == x.cta : true) &&
                                                                            (item.scta != 0 ? item.scta == x.scta : true) &&
                                                                            (item.sscta != 0 ? item.sscta == x.sscta : true) &&
                                                                            (!string.IsNullOrEmpty(item.cc) ? item.cc == x.cc : true));
                                                                    }
                                                                    break;
                                                                case (int)EmpresaEnum.Arrendadora:
                                                                    saldosCC = _ctxEmpresaCruzada.tblEF_SaldoCC
                                                                        .Select(x => new EFSaldosCCDTO
                                                                        {
                                                                            anio = x.anio,
                                                                            cta = x.cta,
                                                                            scta = x.scta,
                                                                            sscta = x.sscta,
                                                                            ccCtaSctaSscta = (string.IsNullOrEmpty(x.cc) ? "" : x.cc + "-") + x.cta + "-" + x.scta + "-" + x.sscta,
                                                                            cargosMes = x.cargosMes * (-1),
                                                                            abonosMes = x.abonosMes * (-1),
                                                                            cc = x.cc,
                                                                            areaCuenta = x.area.HasValue ? x.area.Value + "-" + x.cuenta.Value : null,
                                                                            corteMesID = x.corteMesID,
                                                                            area = x.area,
                                                                            cuenta = x.cuenta,
                                                                            corte = x.corte,
                                                                            estatus = x.estatus
                                                                        })
                                                                        .Where(x =>
                                                                            (
                                                                                (x.corte.anio == anio) ||
                                                                                (x.corte.anio == (anio - 1))
                                                                            ) &&
                                                                            x.corte.mes == mes &&
                                                                            x.corte.estatus &&
                                                                            x.cta == cuenta.cta &&
                                                                            (cuenta.scta > 0 ? x.scta == cuenta.scta : true) &&
                                                                            (cuenta.sscta > 0 ? x.sscta == cuenta.sscta : true) &&
                                                                            (string.IsNullOrEmpty(cuenta.cc) ? true : x.cc == cuenta.cc) &&
                                                                            (listaCC.Count > 0 ? x.area.HasValue : true) &&
                                                                            x.estatus
                                                                        ).ToList();

                                                                    if (listaAreaCuenta.Count > 0)
                                                                    {
                                                                        saldosCC = (
                                                                            from sal in saldosCC
                                                                            join ac in listaAreaCuenta on new { area = sal.area.Value, cuenta = sal.cuenta.Value } equals new { area = ac.area, cuenta = ac.cuenta }
                                                                            select sal
                                                                        ).ToList();
                                                                    }

                                                                    foreach (var item in cuentasCruzadasExcpciones)
                                                                    {
                                                                        saldosCC.RemoveAll(x =>
                                                                            (item.cta != 0 ? item.cta == x.cta : true) &&
                                                                            (item.scta != 0 ? item.scta == x.scta : true) &&
                                                                            (item.sscta != 0 ? item.sscta == x.sscta : true) &&
                                                                            (!string.IsNullOrEmpty(item.cc) ? item.cc == x.cc : true));
                                                                    }
                                                                    break;
                                                            }

                                                            montoCruzado += saldosCC.Where(x => x.corte.anio == anio).Sum(x => (decimal?)x.cargosMes + (decimal?)x.abonosMes) ?? 0;
                                                            montoCruzadoAnterior += saldosCC.Where(x => x.corte.anio < anio).Sum(x => (decimal?)x.cargosMes + (decimal?)x.abonosMes) ?? 0;

                                                            if (cuentasCruzadas.invertirSigno)
                                                            {
                                                                montoCruzado *= -1;
                                                                montoCruzadoAnterior *= -1;
                                                            }
                                                        }

                                                        resultadoPorEmpresa.montoCruzado = concepto.tipoOperacion == TipoOperacionEnum.resta ? (Math.Truncate(montoCruzado) / 1000) * (-1) : (Math.Truncate(montoCruzado) / 1000);
                                                        resultadoPorEmpresa.montoCruzadoAnterior = concepto.tipoOperacion == TipoOperacionEnum.resta ? (Math.Truncate(montoCruzadoAnterior) / 1000) * (-1) : (Math.Truncate(montoCruzadoAnterior) / 1000);
                                                        //renglon.montoClienteProveedor = Math.Abs(montoCruzado) * -1;
                                                        //renglon.montoClienteProveedorAnterior = Math.Abs(montoCruzadoAnterior) * -1;
                                                    }
                                                }
                                                #endregion

                                                if (empresa == EmpresaEnum.Arrendadora)
                                                {
                                                    if (conceptoEmpresa.id == 1 || conceptoEmpresa.id == 2)
                                                    {
                                                        resultadoPorEmpresa.sumarConsolidado = false;
                                                    }
                                                }

                                                resultadosPorEmpresa.Add(resultadoPorEmpresa);
                                            }
                                        } // fin ciclo empresa

                                        var renglon = new EstadoResultadoDTO();
                                        renglon.grupoReporteID = grupoGeneral.id;
                                        renglon.ordenReporte = ++orden;
                                        renglon.concepto = concepto.concepto;
                                        renglon.tipoOperacion = (TipoOperacionEnum)concepto.tipoOperacion;
                                        renglon.resultadosPorEmpresa.AddRange(resultadosPorEmpresa);
                                        renglon.detalleID = concepto.tipoEnlaceId;
                                        renglonesGrupo.Add(renglon);
                                    } // fin ciclo concepto

                                    var renglonTotal = new EstadoResultadoDTO();
                                    renglonTotal.concepto = grupoGeneral.descripcion;
                                    renglonTotal.tipoOperacion = TipoOperacionEnum.total;
                                    renglonTotal.grupoReporteID = grupoGeneral.id;
                                    renglonTotal.ordenReporte = ++orden;
                                    renglonTotal.flagGrupoReporte = true;

                                    if (renglonTotalGrupoAnterior != null)
                                    {
                                        renglonTotal.montoConsolidado = renglonTotalGrupoAnterior.montoConsolidado;
                                        renglonTotal.porcentajeConsolidado = renglonTotalGrupoAnterior.porcentajeConsolidado;
                                        renglonTotal.montoConsolidadoYearAnterior = renglonTotalGrupoAnterior.montoConsolidadoYearAnterior;
                                        renglonTotal.porcentajeConsolidadoYearAnterior = renglonTotalGrupoAnterior.porcentajeConsolidadoYearAnterior;
                                        renglonTotal.montoClienteProveedor = renglonTotalGrupoAnterior.montoClienteProveedor;
                                        renglonTotal.montoClienteProveedorAnterior = renglonTotalGrupoAnterior.montoClienteProveedorAnterior;
                                    }

                                    foreach (var item in renglonesGrupo)
                                    {
                                        item.montoConsolidado += item.resultadosPorEmpresa.Where(x => x.sumarConsolidado).Sum(x => x.montoEmpresa + x.montoCruzado);
                                        item.montoConsolidadoYearAnterior += item.resultadosPorEmpresa.Where(x => x.sumarConsolidado).Sum(x => x.montoYearAnteriorEmpresa + x.montoCruzadoAnterior);

                                        renglonTotal.montoConsolidado += item.tipoOperacion == TipoOperacionEnum.suma ? item.montoConsolidado : (item.montoConsolidado * -1);
                                        renglonTotal.montoConsolidadoYearAnterior += item.tipoOperacion == TipoOperacionEnum.suma ? item.montoConsolidadoYearAnterior : (item.montoConsolidadoYearAnterior * -1);
                                        renglonTotal.montoClienteProveedor += item.montoClienteProveedor;
                                    }

                                    for (int i = 0; i < listaEmpresas.Count; i++)
                                    {
                                        var resultadoTotalEmpresa = new EstadoResultadoPorEmpresaDTO();

                                        if (renglonTotalGrupoAnterior != null)
                                        {
                                            resultadoTotalEmpresa.montoEmpresa = renglonTotalGrupoAnterior.resultadosPorEmpresa[i].montoEmpresa;
                                            resultadoTotalEmpresa.montoCruzado = renglonTotalGrupoAnterior.resultadosPorEmpresa[i].montoCruzado;
                                        }

                                        foreach (var renglon in renglonesGrupo)
                                        {
                                            resultadoTotalEmpresa.montoEmpresa += renglon.tipoOperacion == TipoOperacionEnum.suma ? renglon.resultadosPorEmpresa[i].montoEmpresa : (renglon.resultadosPorEmpresa[i].montoEmpresa * -1);
                                            resultadoTotalEmpresa.montoCruzado += renglon.resultadosPorEmpresa[i].montoCruzado;
                                        }

                                        renglonTotal.resultadosPorEmpresa.Add(resultadoTotalEmpresa);
                                    }

                                    foreach (var item in renglonesGrupo)
                                    {
                                        if (renglonTotalGrupoAnterior == null)
                                        {
                                            item.porcentajeConsolidado = Math.Round((item.montoConsolidado * 100) / (renglonTotal.montoConsolidado != 0 ? renglonTotal.montoConsolidado : 1), 2);
                                            item.porcentajeConsolidadoYearAnterior = Math.Round((item.montoConsolidadoYearAnterior * 100) / (renglonTotal.montoConsolidadoYearAnterior != 0 ? renglonTotal.montoConsolidadoYearAnterior : 1), 2);
                                        }
                                        else
                                        {
                                            item.porcentajeConsolidado = Math.Round((item.montoConsolidado * renglonTotalGrupoAnterior.porcentajeConsolidado) / (renglonTotalGrupoAnterior.montoConsolidado != 0 ? renglonTotalGrupoAnterior.montoConsolidado : 1), 2);
                                            item.porcentajeConsolidadoYearAnterior = Math.Round((item.montoConsolidadoYearAnterior * renglonTotalGrupoAnterior.porcentajeConsolidadoYearAnterior) / (renglonTotalGrupoAnterior.montoConsolidadoYearAnterior != 0 ? renglonTotalGrupoAnterior.montoConsolidadoYearAnterior : 1), 2);
                                        }

                                        renglonTotal.porcentajeConsolidado += item.tipoOperacion == TipoOperacionEnum.suma ? item.porcentajeConsolidado : item.porcentajeConsolidado * -1; ;
                                        renglonTotal.porcentajeConsolidadoYearAnterior += item.tipoOperacion == TipoOperacionEnum.suma ? item.porcentajeConsolidadoYearAnterior : item.porcentajeConsolidadoYearAnterior * -1;
                                    }

                                    renglonTotalGrupoAnterior = renglonTotal;

                                    renglones.AddRange(renglonesGrupo);
                                    renglones.Add(renglonTotal);
                                }
                                List<string> tiposModal = new List<string> { "", "Ingreso", "Costo", "Gasto" };
                                foreach (var renglon in renglones)
                                {
                                    var renglonDataTable = new Dictionary<string, object>();
                                    renglonDataTable.Add("ordenReporte", renglon.ordenReporte);
                                    //renglonDataTable.Add("concepto", renglon.concepto);
                                    renglonDataTable.Add("concepto", (renglon.detalleID > 0 && renglon.detalleID < 4) ? "<a class='modal" + tiposModal[renglon.detalleID] + "' data-target='#modal" + tiposModal[renglon.detalleID] + "' data-toggle='modal" + tiposModal[renglon.detalleID] + "' href='#modal" + tiposModal[renglon.detalleID] + "' nombre-detalle='" + tiposModal[renglon.detalleID] + "' style='font-weight: bold;'>" + renglon.concepto + "</a>" : renglon.concepto);
                                    renglonDataTable.Add("flagGrupoReporte", renglon.flagGrupoReporte);

                                    int numeroColumna = 1;
                                    foreach (var empresa in renglon.resultadosPorEmpresa)
                                    {
                                        renglonDataTable.Add("columna" + numeroColumna, empresa.montoEmpresa);
                                        numeroColumna++;
                                        renglonDataTable.Add("columna" + numeroColumna, empresa.montoCruzado);
                                        numeroColumna++;
                                    }

                                    renglonDataTable.Add("columna" + numeroColumna, renglon.montoConsolidado);
                                    renglonDataTable.Add("columna" + ++numeroColumna, renglon.porcentajeConsolidado);
                                    renglonDataTable.Add("columna" + ++numeroColumna, renglon.montoConsolidadoYearAnterior);
                                    renglonDataTable.Add("columna" + ++numeroColumna, renglon.porcentajeConsolidadoYearAnterior);
                                    renglonDataTable.Add("detalleID", renglon.detalleID);
                                    listaDatosDataTable1.Add(renglonDataTable);
                                }
                            }
                        }
                        #endregion
                        break;
                }
                #endregion

                resultado.Add("listaColumnas", listaColumnas);
                resultado.Add("listaDatosDataTable", listaDatosDataTable1);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                LogError(0, 0, "EstadosFinancierosController", "calcularEstadoResultados", e, AccionEnum.CONSULTA, 0, new { fechaAnioMes = fechaAnioMes, listaCC = listaCC });
            }

            return resultado;
        }

        public Dictionary<string, object> GetEstadoResultadoDetalle(List<EmpresaEnum> listaEmpresas, DateTime fechaAnioMes, List<string> listaCC, int tipoBusqueda)
        {
            try
            {
                if (listaCC == null)
                {
                    listaCC = new List<string>();
                }
                else
                {
                    if (listaCC.Contains("Todoss"))
                    {
                        listaCC = new List<string>();
                    }
                }

                var listaAC = new List<string>();
                

                if (listaCC.Count > 0)
                {
                    using (var _contextConstruplan = new MainContext(EmpresaEnum.Construplan))
                    {
                        listaAC = _contextConstruplan.tblP_CC
                            .Where(x => (listaCC.Count > 0 ? listaCC.Contains(x.cc) : true))
                            .Select(x => x.area + "-" + x.areaCuenta)
                            .Distinct().ToList();
                    }
                }

                var anio = fechaAnioMes.Year;
                var mes = fechaAnioMes.Month;
                var nombreMes = (new DateTime(anio, mes, 1).ToString("MMMM", CultureInfo.CreateSpecificCulture("es"))).ToUpper();

                var listaDatosDataTable1 = new List<Dictionary<string, object>>();

                List<EFIngresosDetDTO> detalles = new List<EFIngresosDetDTO>();
                List<EFIngresosDetDTO> auxDetalles = new List<EFIngresosDetDTO>();
                List<EFGraficaDetallesDTO> detallesGrafica = new List<EFGraficaDetallesDTO>();
                List<EFGraficaDetallesDTO> auxDetallesGrafica = new List<EFGraficaDetallesDTO>();

                int numEmpresas = listaEmpresas.Count();
                bool consultaCplan = listaEmpresas.Contains(EmpresaEnum.Construplan);
                bool consultaArrendadora = listaEmpresas.Contains(EmpresaEnum.Arrendadora);

                if (listaEmpresas == null)
                {
                    throw new Exception("Debe seleccionar por lo menos una empresa.");
                }

                #region datosTablaDetalle
                switch (tipoBusqueda)
                {
                    case 1:
                        foreach (var empresa in listaEmpresas)
                        {
                            using (var _ctxEmpresa = new MainContext(empresa))
                            {
                                var divisionesDet = _ctxEmpresa.tblAF_DxP_Divisiones_Proyecto.Where(x => listaCC.Count() > 0 ? listaCC.Contains(x.cc) : x.esActivo).ToList();
                                var divisionesIDs = divisionesDet.Select(x => x.divisionID).ToList();
                                var divisiones = _ctxEmpresa.tblAF_DxP_Divisiones.Where(x => divisionesIDs.Contains(x.id)).ToList();
                                auxDetalles = _ctxEmpresa.tblEF_SaldoCC.Where(x =>
                                        ((x.corte.anio == anio) || (x.corte.anio == (anio - 1))) &&
                                        x.corte.mes == mes &&
                                        (x.anio >= fechaAnioMes.Year && (x.cta == 4000 || x.cta == 4001)) &&
                                        ((numEmpresas > 1 && consultaCplan && consultaArrendadora && empresa == EmpresaEnum.Arrendadora) ? (x.anio >= fechaAnioMes.Year && (x.cta != 4000 && x.cta != 4001)) : true) &&
                                        (x.scta > 0 ? x.scta == x.scta : true) &&
                                        (x.sscta > 0 ? x.sscta == x.sscta : true) &&
                                        (empresa != EmpresaEnum.Arrendadora ? (listaCC.Count > 0 ? listaCC.Contains(x.cc) : true) : (listaAC.Count > 0 ? listaAC.Contains(x.areaCuenta) : true)) &&
                                        (string.IsNullOrEmpty(x.cc) ? true : x.cc == x.cc) &&
                                        x.estatus
                                    ).ToList()
                                    .Select(x =>
                                    {
                                        var auxdivisionesDet = divisionesDet.FirstOrDefault(y => y.cc == x.cc);
                                        var auxDivisionDescr = "OTROS";
                                        var auxDivisionID = 0;
                                        if (auxdivisionesDet != null)
                                        {
                                            var auxDivision = divisiones.FirstOrDefault(y => y.id == auxdivisionesDet.divisionID);
                                            if (auxDivision != null)
                                            {
                                                auxDivisionDescr = auxDivision.nombre;
                                                auxDivisionID = auxDivision.id;
                                            }
                                        }
                                        return new EFIngresosDetDTO
                                        {
                                            divisionID = auxDivisionID,
                                            divisionDescr = auxDivisionDescr,
                                            mensualActual = x.abonosMes + x.cargosMes,
                                            acumuladoActual = x.abonosAcumulados + x.cargosAcumulados,
                                            esActual = x.anio == anio
                                        };
                                    }).ToList();

                                //Datos Grafica
                                auxDetallesGrafica = _ctxEmpresa.tblEF_SaldoCC.Where(x =>
                                    x.corte.anio == anio &&
                                    x.corte.mes <= mes &&
                                    (x.anio >= fechaAnioMes.Year && (x.cta == 4000 || x.cta == 4001)) &&
                                    ((numEmpresas > 1 && consultaCplan && consultaArrendadora && empresa == EmpresaEnum.Arrendadora) ? (x.anio >= fechaAnioMes.Year && (x.cta != 4000 && x.cta != 4001)) : true) &&
                                    (x.scta > 0 ? x.scta == x.scta : true) &&
                                    (x.sscta > 0 ? x.sscta == x.sscta : true) &&
                                    (empresa != EmpresaEnum.Arrendadora ? (listaCC.Count > 0 ? listaCC.Contains(x.cc) : true) : (listaAC.Count > 0 ? listaAC.Contains(x.areaCuenta) : true)) &&
                                    (string.IsNullOrEmpty(x.cc) ? true : x.cc == x.cc) &&
                                    x.estatus
                                ).ToList()
                                .Select(x =>
                                {
                                    var auxdivisionesDet = divisionesDet.FirstOrDefault(y => y.cc == x.cc);
                                    var auxDivisionDescr = "OTROS";
                                    var auxDivisionID = 0;
                                    if (auxdivisionesDet != null)
                                    {
                                        var auxDivision = divisiones.FirstOrDefault(y => y.id == auxdivisionesDet.divisionID);
                                        if (auxDivision != null)
                                        {
                                            auxDivisionDescr = auxDivision.nombre;
                                            auxDivisionID = auxDivision.id;
                                        }
                                    }
                                    return new EFGraficaDetallesDTO
                                    {
                                        concepto = auxDivisionDescr,
                                        monto = x.abonosMes + x.cargosMes,
                                        anio = x.corte.anio,
                                        mes = x.corte.mes,
                                    };
                                }).ToList();
                            }
                            detalles.AddRange(auxDetalles);
                            detallesGrafica.AddRange(auxDetallesGrafica);
                        }
                        var totalMensualActual = detalles.Where(x => x.esActual).Sum(x => x.mensualActual) * (-1);
                        var totalAcumuladoActual = detalles.Where(x => x.esActual).Sum(x => x.acumuladoActual) * (-1);
                        var totalMensualAnterior = detalles.Where(x => !x.esActual).Sum(x => x.mensualActual) * (-1);
                        var totalAcumuladoAnterior = detalles.Where(x => !x.esActual).Sum(x => x.acumuladoActual) * (-1);
                        detalles = detalles
                        .GroupBy(x => new { x.divisionID, x.divisionDescr })
                        .Select(x => new EFIngresosDetDTO
                        {
                            divisionID = x.Key.divisionID,
                            divisionDescr = x.Key.divisionDescr,
                            mensualActual = x.Where(y => y.esActual).Sum(y => y.mensualActual) * (-1),
                            porcentajeMensualActual = totalMensualActual == 0 ? (x.Where(y => y.esActual).Sum(y => y.mensualActual) * (-100)) : (x.Where(y => y.esActual).Sum(y => y.mensualActual) * (-100)) / totalMensualActual,
                            acumuladoActual = x.Where(y => y.esActual).Sum(y => y.acumuladoActual) * (-1),
                            porcentajeAcumuladoActual = totalAcumuladoActual == 0 ? (x.Where(y => y.esActual).Sum(y => y.acumuladoActual) * (-100)) : (x.Where(y => y.esActual).Sum(y => y.acumuladoActual) * (-100)) / totalAcumuladoActual,
                            mensualAnterior = x.Where(y => !y.esActual).Sum(y => y.mensualActual) * (-1),
                            porcentajeMensualAnterior = totalMensualAnterior == 0 ? (x.Where(y => !y.esActual).Sum(y => y.mensualActual) * (-100)) : (x.Where(y => !y.esActual).Sum(y => y.mensualActual) * (-100)) / totalMensualAnterior,
                            acumuladoAnterior = x.Where(y => !y.esActual).Sum(y => y.acumuladoActual) * (-1),
                            porcentajeAcumuladoAnterior = totalAcumuladoAnterior == 0 ? (x.Where(y => !y.esActual).Sum(y => y.acumuladoActual) * (-100)) : (x.Where(y => !y.esActual).Sum(y => y.acumuladoActual) * (-100)) / totalAcumuladoAnterior
                        }).OrderByDescending(x => x.mensualActual).ThenByDescending(x => x.acumuladoActual).ThenByDescending(x => x.mensualAnterior).ThenByDescending(x => x.acumuladoAnterior).ToList();
                        //Detalles grafica agrupacion
                        detallesGrafica = detallesGrafica
                        .GroupBy(x => new { x.concepto, x.anio, x.mes })
                        .Select(x => new EFGraficaDetallesDTO
                        {
                            concepto = x.Key.concepto,
                            monto = x.Sum(y => y.monto),
                            anio = x.Key.anio,
                            mes = x.Key.mes
                        }).OrderByDescending(x => x.anio).ThenBy(x => x.mes).ThenBy(x => x.concepto).ToList();
                        break;
                    case 2:
                        var cuentasContables = getCuentasDesc();
                        foreach (var empresa in listaEmpresas)
                        {
                            using (var _ctxEmpresa = new MainContext(empresa))
                            {
                                var divisionesDet = _ctxEmpresa.tblAF_DxP_Divisiones_Proyecto.Where(x => listaCC.Count() > 0 ? listaCC.Contains(x.cc) : x.esActivo).ToList();
                                var divisionesIDs = divisionesDet.Select(x => x.divisionID).ToList();
                                var divisiones = _ctxEmpresa.tblAF_DxP_Divisiones.Where(x => divisionesIDs.Contains(x.id)).ToList();

                                auxDetalles = _ctxEmpresa.tblEF_SaldoCC.Where(x =>
                                        ((x.corte.anio == anio) || (x.corte.anio == (anio - 1))) &&
                                        x.corte.mes == mes &&
                                        (x.anio >= fechaAnioMes.Year && x.cta == 5000 /*|| (x.cta == 1 && x.scta != 4)*/) &&
                                        ((numEmpresas > 1 && consultaCplan && consultaArrendadora && empresa == EmpresaEnum.Construplan) ? (x.scta != 18 && x.cta != 1) : true) &&
                                        !(x.cta == 5000 && x.scta == 18 && x.cc == "994") &&
                                        (x.scta > 0 ? x.scta == x.scta : true) &&
                                        (x.sscta > 0 ? x.sscta == x.sscta : true) &&
                                        (empresa != EmpresaEnum.Arrendadora ? (listaCC.Count > 0 ? listaCC.Contains(x.cc) : true) : (listaAC.Count > 0 ? listaAC.Contains(x.areaCuenta) : true)) &&
                                        (string.IsNullOrEmpty(x.cc) ? true : x.cc == x.cc) &&
                                        x.estatus
                                    ).OrderBy(x => new { x.cta, x.scta }).ToList()
                                    .Select(x =>
                                    {
                                        return new EFIngresosDetDTO
                                        {
                                            divisionID = 0,
                                            divisionDescr = x.cta.ToString() + "-" + x.scta.ToString(),
                                            mensualActual = x.abonosMes + x.cargosMes,
                                            acumuladoActual = x.abonosAcumulados + x.cargosAcumulados,
                                            esActual = x.anio == anio
                                        };
                                    }).ToList();
                                auxDetallesGrafica = _ctxEmpresa.tblEF_SaldoCC.Where(x =>
                                    x.corte.anio == anio &&
                                    x.corte.mes <= mes &&
                                    (x.anio >= fechaAnioMes.Year && x.cta == 5000 /*|| (x.cta == 1 && x.scta != 4)*/) &&
                                    ((numEmpresas > 1 && consultaCplan && consultaArrendadora && empresa == EmpresaEnum.Construplan) ? (x.scta != 18 && x.cta != 1) : true) &&
                                    !(x.cta == 5000 && x.scta == 18 && x.cc == "994") &&
                                    (x.scta > 0 ? x.scta == x.scta : true) &&
                                    (x.sscta > 0 ? x.sscta == x.sscta : true) &&
                                    (empresa != EmpresaEnum.Arrendadora ? (listaCC.Count > 0 ? listaCC.Contains(x.cc) : true) : (listaAC.Count > 0 ? listaAC.Contains(x.areaCuenta) : true)) &&
                                    (string.IsNullOrEmpty(x.cc) ? true : x.cc == x.cc) &&
                                    x.estatus
                                ).ToList()
                                .Select(x =>
                                {
                                    return new EFGraficaDetallesDTO
                                    {
                                        concepto = x.cta.ToString() + "-" + x.scta.ToString(),
                                        monto = x.abonosMes + x.cargosMes,
                                        anio = x.corte.anio,
                                        mes = x.corte.mes,
                                    };
                                }).ToList();
                            }
                            detalles.AddRange(auxDetalles);
                            detallesGrafica.AddRange(auxDetallesGrafica);
                        }
                        detalles = detalles
                        .GroupBy(x => new { x.divisionID, x.divisionDescr })
                        .Select(x =>
                        {
                            var cuentaContableDescr = cuentasContables.FirstOrDefault(y => y.Value == x.Key.divisionDescr + "-0");
                            var keyDescr = x.Key.divisionDescr;
                            if (cuentaContableDescr != null) keyDescr = cuentaContableDescr.Text;
                            return new EFIngresosDetDTO
                            {
                                divisionID = x.Key.divisionID,
                                divisionDescr = keyDescr,
                                mensualActual = x.Where(y => y.esActual).Sum(y => y.mensualActual),
                                acumuladoActual = x.Where(y => y.esActual).Sum(y => y.acumuladoActual),
                                mensualAnterior = x.Where(y => !y.esActual).Sum(y => y.mensualActual),
                                acumuladoAnterior = x.Where(y => !y.esActual).Sum(y => y.acumuladoActual)
                            };
                        }).OrderByDescending(x => x.mensualActual).ThenByDescending(x => x.acumuladoActual).ThenByDescending(x => x.mensualAnterior).ThenByDescending(x => x.acumuladoAnterior).ToList();
                        detallesGrafica = detallesGrafica
                        .GroupBy(x => new { x.concepto, x.anio, x.mes })
                        .Select(x =>
                        {
                            var cuentaContableDescr = cuentasContables.FirstOrDefault(y => y.Value == x.Key.concepto + "-0");
                            var keyDescr = x.Key.concepto;
                            if (cuentaContableDescr != null) keyDescr = cuentaContableDescr.Text;
                            return new EFGraficaDetallesDTO
                            {
                                concepto = keyDescr,
                                monto = x.Sum(y => y.monto),
                                anio = x.Key.anio,
                                mes = x.Key.mes
                            };
                        }).OrderByDescending(x => x.anio).ThenBy(x => x.mes).ThenBy(x => x.concepto).ToList();
                        break;
                    case 3:
                        cuentasContables = getCuentasDesc();
                        foreach (var empresa in listaEmpresas)
                        {
                            using (var _ctxEmpresa = new MainContext(empresa))
                            {
                                var divisionesDet = _ctxEmpresa.tblAF_DxP_Divisiones_Proyecto.Where(x => listaCC.Count() > 0 ? listaCC.Contains(x.cc) : x.esActivo).ToList();
                                var divisionesIDs = divisionesDet.Select(x => x.divisionID).ToList();
                                var divisiones = _ctxEmpresa.tblAF_DxP_Divisiones.Where(x => divisionesIDs.Contains(x.id)).ToList();

                                auxDetalles = _ctxEmpresa.tblEF_SaldoCC.Where(x =>
                                        ((x.corte.anio == anio) || (x.corte.anio == (anio - 1))) &&
                                        x.corte.mes == mes &&
                                        x.cta == 5280 &&
                                        (x.scta > 0 ? x.scta == x.scta : true) &&
                                        (x.sscta > 0 ? x.sscta == x.sscta : true) &&
                                        (empresa != EmpresaEnum.Arrendadora ? (listaCC.Count > 0 ? listaCC.Contains(x.cc) : true) : (listaAC.Count > 0 ? listaAC.Contains(x.areaCuenta) : true)) &&
                                        (string.IsNullOrEmpty(x.cc) ? true : x.cc == x.cc) &&
                                        x.estatus
                                    ).OrderBy(x => new { x.cta, x.scta }).ToList()
                                    .Select(x =>
                                    {
                                        return new EFIngresosDetDTO
                                        {
                                            divisionID = 0,
                                            divisionDescr = x.cc == "A03" ? "GOBIERNO CORPORATIVO" : x.cta.ToString() + "-" + x.scta.ToString(),
                                            mensualActual = x.abonosMes + x.cargosMes,
                                            acumuladoActual = x.abonosAcumulados + x.cargosAcumulados,
                                            esActual = x.anio == anio
                                        };
                                    }).ToList();
                            }
                            detalles.AddRange(auxDetalles);
                        }
                        detalles = detalles
                        .GroupBy(x => new { x.divisionID, x.divisionDescr })
                        .Select(x =>
                        {
                            var cuentaContableDescr = cuentasContables.FirstOrDefault(y => y.Value == x.Key.divisionDescr + "-0");
                            var keyDescr = x.Key.divisionDescr;
                            if (cuentaContableDescr != null) keyDescr = cuentaContableDescr.Text;
                            return new EFIngresosDetDTO
                            {
                                divisionID = x.Key.divisionID,
                                divisionDescr = keyDescr,
                                mensualActual = x.Where(y => y.esActual).Sum(y => y.mensualActual),
                                acumuladoActual = x.Where(y => y.esActual).Sum(y => y.acumuladoActual),
                                mensualAnterior = x.Where(y => !y.esActual).Sum(y => y.mensualActual),
                                acumuladoAnterior = x.Where(y => !y.esActual).Sum(y => y.acumuladoActual)
                            };
                        }).OrderByDescending(x => x.mensualActual).ThenByDescending(x => x.acumuladoActual).ThenByDescending(x => x.mensualAnterior).ThenByDescending(x => x.acumuladoAnterior).ToList();
                        break;
                }
                #endregion
                resultado.Add("data", detalles);
                resultado.Add("dataGraficas", detallesGrafica);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                LogError(0, 0, "EstadosFinancierosController", "GetEstadoResultadoDetalle", e, AccionEnum.CONSULTA, 0, new { fechaAnioMes = fechaAnioMes, listaCC = listaCC });
            }

            return resultado;
        }

        private List<ComboDTO> getCuentasDesc()
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

        private decimal reglaTresPorcentaje(decimal montoRenglonActual, decimal montoRenglonAnterior, decimal porcentajeRenglonAnterior)
        {
            return Math.Truncate(100 * ((montoRenglonActual * porcentajeRenglonAnterior) / (montoRenglonAnterior > 0 ? montoRenglonAnterior : 1))) / 100;
        }

        public Dictionary<string, object> EliminarBalanza(DateTime fechaCorte)
        {
            using (var transaccion = _context.Database.BeginTransaction())
            {
                try
                {
                    var corte = _context.tblEF_CorteMes.FirstOrDefault(x => x.estatus && x.anio == fechaCorte.Year && x.mes == fechaCorte.Month);

                    if (corte != null)
                    {
                        var balanza = _context.tblEF_Balanza.Where(x => x.estatus && x.corteMesID == corte.id).ToList();
                        var indicadores = _context.tblEF_Indicadores.Where(x => x.registroActivo && x.corteMesId == corte.id).ToList();
                        var clientes = _context.tblEF_MovimientoCliente.Where(x => x.estatus && x.corteMesID == corte.id).ToList();
                        var proveedores = _context.tblEF_MovimientoProveedor.Where(x => x.estatus && x.corteMesID == corte.id).ToList();
                        var saldos = _context.tblEF_SaldoCC.Where(x => x.estatus && x.corteMesID == corte.id).ToList();

                        foreach (var item in balanza)
                        {
                            item.estatus = false;
                        }

                        foreach (var item in indicadores)
                        {
                            item.registroActivo = false;
                        }

                        foreach (var item in clientes)
                        {
                            item.estatus = false;
                        }

                        foreach (var item in proveedores)
                        {
                            item.estatus = false;
                        }

                        foreach (var item in saldos)
                        {
                            item.estatus = false;
                        }

                        corte.estatus = false;

                        _context.SaveChanges();
                        transaccion.Commit();

                        resultado.Add(SUCCESS, true);
                    }
                    else
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "No se encuentra información guardada para el periodo seleccionado");
                    }
                }
                catch (Exception ex)
                {
                    transaccion.Rollback();
                    LogError(0, 0, "EstadosFinancierosController", "EliminarBalanza", ex, AccionEnum.ELIMINAR, 0, fechaCorte);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, ex.Message);
                }
            }

            return resultado;
        }

        #region Balance
        public Dictionary<string, object> CalcularBalanceGeneral(List<EmpresaEnum> listaEmpresas, DateTime fechaAnioMes, List<string> listaCC, TipoBalanceEnum tipoBalance)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                var listaAC = new List<string>();

                if (listaCC == null)
                {
                    listaCC = new List<string>();
                }
                else
                {
                    if (listaCC.Contains("Todoss"))
                    {
                        listaCC = new List<string>();
                    }
                }

                if (listaCC.Count > 0)
                {
                    using (var _contextConstruplan = new MainContext(EmpresaEnum.Construplan))
                    {
                        listaAC = _contextConstruplan.tblP_CC
                            .Where(x => (listaCC.Count > 0 ? listaCC.Contains(x.cc) : true))
                            .Select(x => x.area + "-" + x.areaCuenta)
                            .Distinct().ToList();
                    }
                }

                var year = fechaAnioMes.Year;
                var yearAnterior = year - 1;
                var mes = fechaAnioMes.Month;

                fechaAnioMes = new DateTime(year, mes, DateTime.DaysInMonth(year, mes));
                var tipoCambio = GetTipoCambio(fechaAnioMes);
                var tipoCambioMesAnterior = GetTipoCambio(fechaAnioMes.AddMonths(-1));

                List<BalanceDTO> datosTabla = new List<BalanceDTO>();

                var pesosCirculanteLP = 0M;
                var dolaresCirculanteLP = 0M;
                var pesosCirculanteLPAnterior = 0M;
                var dolaresCirculanteLPAnterior = 0M;

                switch (listaEmpresas.Count)
                {
                    case 1:
                        #region Balance para una empresa
                        {
                            var empresa = listaEmpresas.First();

                            using (var _ctx = new MainContext(empresa))
                            {
                                var balance = _ctx.tblEF_BalanceTipoBalance.First(x => x.id == (int)tipoBalance && !x.esConsolidado && x.registroActivo);

                                foreach (var grupo in balance.grupos.Where(x => x.registroActivo))
                                {
                                    var datosGrupo = new List<BalanceDTO>();

                                    foreach (var concepto in grupo.conceptos.Where(x => x.registroActivo).OrderBy(x => x.orden))
                                    {
                                        BalanceDTO datoTabla = new BalanceDTO();
                                        datoTabla.concepto = concepto.descripcion;
                                        datoTabla.tipoDetalle = concepto.tipoDetalleId;
                                        datoTabla.empresa = empresa;
                                        datoTabla.mes = mes;

                                        if (concepto.esSubtitulo)
                                        {
                                            datoTabla.renglonSubTitulo = true;
                                        }
                                        if (concepto.tieneEnlace)
                                        {
                                            datoTabla.renglonEnlace = true;
                                        }

                                        var corte = 0M;
                                        var corteAnterior = 0M;
                                        var dolares = 0M;

                                        if (concepto.dxpLp)
                                        {
                                            corte -= pesosCirculanteLP;
                                            dolares -= dolaresCirculanteLP;
                                            corteAnterior -= pesosCirculanteLPAnterior;
                                        }

                                        if (concepto.dxpLpCirculante)
                                        {
                                            var circulanteLP = CalcularDxpCirculanteLP(_ctx, fechaAnioMes, empresa, tipoCambio, tipoCambioMesAnterior);

                                            pesosCirculanteLP = (decimal)circulanteLP["pesos"];
                                            dolaresCirculanteLP = (decimal)circulanteLP["dolares"];
                                            pesosCirculanteLPAnterior = (decimal)circulanteLP["pesosAnterior"];
                                            dolaresCirculanteLPAnterior = (decimal)circulanteLP["dolaresAnterior"];


                                            corte += pesosCirculanteLP;
                                            dolares += dolaresCirculanteLP;
                                            corteAnterior += pesosCirculanteLPAnterior;
                                        }
                                        else
                                        {
                                            foreach (var cuenta in concepto.cuentas.Where(x => x.registroActivo))
                                            {
                                                var filtroSaldo = new FiltroSaldoDTO();
                                                filtroSaldo.empresa = empresa;
                                                filtroSaldo.year = year;
                                                filtroSaldo.mes = mes;
                                                filtroSaldo.cta = cuenta.cta;
                                                filtroSaldo.scta = cuenta.scta;
                                                filtroSaldo.sscta = cuenta.sscta;
                                                filtroSaldo.cc = cuenta.cc;
                                                filtroSaldo.areaCuenta = cuenta.areaCuenta;
                                                filtroSaldo.listaCC = listaCC;
                                                filtroSaldo.listaAC = listaAC;

                                                var saldosCC = new List<tblEF_SaldoCC>();
                                                var clientes = new List<tblEF_MovimientoCliente>();
                                                var proveedores = new List<tblEF_MovimientoProveedor>();

                                                switch (cuenta.tipoCuentaId)
                                                {
                                                    case TipoCuentaEnum.CUENTA:
                                                        {
                                                            saldosCC = SaldoBalance(_ctx, filtroSaldo);

                                                            saldosCC.ForEach(x => x.saldoInicial = (x.cta >= 4000 && x.cta < 6000) ? 0 : x.saldoInicial);

                                                            if (cuenta.tipoOperacion == TipoOperacionEnum.suma && !cuenta.invertirSigno)
                                                            {
                                                                if (!cuenta.esCuentaDolar)
                                                                {
                                                                    if (!concepto.esAcumulado)
                                                                    {
                                                                        corte += saldosCC
                                                                            .Where(x => x.corte.anio == year && x.corte.mes == mes)
                                                                            .Sum(x => (decimal?)x.saldoInicial + (decimal?)x.cargosAcumulados + (decimal?)x.abonosAcumulados) ?? 0;
                                                                        corteAnterior += saldosCC
                                                                            .Where(x => x.corte.mes != mes)
                                                                            .Sum(x => (decimal?)x.saldoInicial + (decimal?)x.cargosAcumulados + (decimal?)x.abonosAcumulados) ?? 0;
                                                                    }
                                                                    else
                                                                    {
                                                                        corte += saldosCC
                                                                            .Where(x => x.corte.anio == year && x.corte.mes == mes)
                                                                            .Sum(x => (decimal?)x.cargosAcumulados + (decimal?)x.abonosAcumulados) ?? 0;
                                                                        corteAnterior += saldosCC
                                                                            .Where(x => x.corte.mes != mes)
                                                                            .Sum(x => (decimal?)x.cargosAcumulados + (decimal?)x.abonosAcumulados) ?? 0;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    dolares += saldosCC
                                                                        .Where(x => x.corte.anio == year && x.corte.mes == mes)
                                                                        .Sum(x => (decimal?)x.saldoInicial + (decimal?)x.cargosAcumulados + (decimal?)x.abonosAcumulados) ?? 0;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (!cuenta.esCuentaDolar)
                                                                {
                                                                    if (!concepto.esAcumulado)
                                                                    {
                                                                        corte -= saldosCC
                                                                            .Where(x => x.corte.anio == year && x.corte.mes == mes)
                                                                            .Sum(x => (decimal?)x.saldoInicial + (decimal?)x.cargosAcumulados + (decimal?)x.abonosAcumulados) ?? 0;
                                                                        corteAnterior -= saldosCC
                                                                            .Where(x => x.corte.mes != mes)
                                                                            .Sum(x => (decimal?)x.saldoInicial + (decimal?)x.cargosAcumulados + (decimal?)x.abonosAcumulados) ?? 0;
                                                                    }
                                                                    else
                                                                    {
                                                                        corte -= saldosCC
                                                                            .Where(x => x.corte.anio == year && x.corte.mes == mes)
                                                                            .Sum(x => (decimal?)x.cargosAcumulados + (decimal?)x.abonosAcumulados) ?? 0;
                                                                        corteAnterior -= saldosCC
                                                                            .Where(x => x.corte.mes != mes)
                                                                            .Sum(x => (decimal?)x.cargosAcumulados + (decimal?)x.abonosAcumulados) ?? 0;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    dolares -= saldosCC
                                                                        .Where(x => x.corte.anio == year && x.corte.mes == mes)
                                                                        .Sum(x => (decimal?)x.saldoInicial + (decimal?)x.cargosAcumulados + (decimal?)x.abonosAcumulados) ?? 0;
                                                                }
                                                            }
                                                        }
                                                        break;
                                                    case TipoCuentaEnum.CLIENTE:
                                                        {
                                                            clientes = SaldoCliente(_ctx, filtroSaldo);

                                                            if (cuenta.tipoOperacion == TipoOperacionEnum.suma && !cuenta.invertirSigno)
                                                            {
                                                                if (!cuenta.esCuentaDolar)
                                                                {
                                                                    corte += clientes
                                                                        .Where(x => x.corte.anio == year && x.corte.mes == mes)
                                                                        .Sum(x => (decimal?)x.total) ?? 0;
                                                                    corteAnterior += clientes
                                                                        .Where(x => x.corte.mes != mes)
                                                                        .Sum(x => (decimal?)x.total) ?? 0;
                                                                }
                                                                else
                                                                {
                                                                    corte += (clientes
                                                                        .Where(x => x.corte.anio == year && x.corte.mes == mes)
                                                                        .Sum(x => (decimal?)x.total) ?? 0) * tipoCambio;
                                                                    corteAnterior += (clientes
                                                                        .Where(x => x.corte.mes != mes)
                                                                        .Sum(x => (decimal?)x.total) ?? 0) * tipoCambioMesAnterior;

                                                                    dolares += clientes
                                                                        .Where(x => x.corte.anio == year && x.corte.mes == mes)
                                                                        .Sum(x => (decimal?)x.total) ?? 0;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (!cuenta.esCuentaDolar)
                                                                {
                                                                    corte -= clientes
                                                                        .Where(x => x.corte.anio == year && x.corte.mes == mes)
                                                                        .Sum(x => (decimal?)x.total) ?? 0;
                                                                    corteAnterior -= clientes
                                                                        .Where(x => x.corte.mes != mes)
                                                                        .Sum(x => (decimal?)x.total) ?? 0;
                                                                }
                                                                else
                                                                {
                                                                    corte -= (clientes
                                                                        .Where(x => x.corte.anio == year && x.corte.mes == mes)
                                                                        .Sum(x => (decimal?)x.total) ?? 0) * tipoCambio;
                                                                    corteAnterior -= (clientes
                                                                        .Where(x => x.corte.mes != mes)
                                                                        .Sum(x => (decimal?)x.total) ?? 0) * tipoCambioMesAnterior;

                                                                    dolares -= clientes
                                                                        .Where(x => x.corte.anio == year && x.corte.mes == mes)
                                                                        .Sum(x => (decimal?)x.total) ?? 0;
                                                                }
                                                            }
                                                        }
                                                        break;
                                                    case TipoCuentaEnum.PROVEEDOR:
                                                        {
                                                            proveedores = SaldoProveedor(_ctx, filtroSaldo);

                                                            if (cuenta.tipoOperacion == TipoOperacionEnum.suma && !cuenta.invertirSigno)
                                                            {
                                                                if (!cuenta.esCuentaDolar)
                                                                {
                                                                    corte += proveedores
                                                                        .Where(x => x.corte.anio == year && x.corte.mes == mes)
                                                                        .Sum(x => (decimal?)x.total) ?? 0;
                                                                    corteAnterior += proveedores
                                                                        .Where(x => x.corte.mes != mes)
                                                                        .Sum(x => (decimal?)x.total) ?? 0;
                                                                }
                                                                else
                                                                {
                                                                    corte += (proveedores
                                                                        .Where(x => x.corte.anio == year && x.corte.mes == mes)
                                                                        .Sum(x => (decimal?)x.total) ?? 0) * tipoCambio;
                                                                    corteAnterior += (proveedores
                                                                        .Where(x => x.corte.mes != mes)
                                                                        .Sum(x => (decimal?)x.total) ?? 0) * tipoCambioMesAnterior;

                                                                    dolares += proveedores
                                                                        .Where(x => x.corte.anio == year && x.corte.mes == mes)
                                                                        .Sum(x => (decimal?)x.total) ?? 0;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (!cuenta.esCuentaDolar)
                                                                {
                                                                    corte -= proveedores
                                                                        .Where(x => x.corte.anio == year && x.corte.mes == mes)
                                                                        .Sum(x => (decimal?)x.total) ?? 0;
                                                                    corteAnterior -= proveedores
                                                                        .Where(x => x.corte.mes != mes)
                                                                        .Sum(x => (decimal?)x.total) ?? 0;
                                                                }
                                                                else
                                                                {
                                                                    corte -= (proveedores
                                                                        .Where(x => x.corte.anio == year && x.corte.mes == mes)
                                                                        .Sum(x => (decimal?)x.total) ?? 0) * tipoCambio;
                                                                    corteAnterior -= (proveedores
                                                                        .Where(x => x.corte.mes != mes)
                                                                        .Sum(x => (decimal?)x.total) ?? 0) * tipoCambioMesAnterior;

                                                                    dolares -= proveedores
                                                                        .Where(x => x.corte.anio == year && x.corte.mes == mes)
                                                                        .Sum(x => (decimal?)x.total) ?? 0;
                                                                }
                                                            }
                                                        }
                                                        break;
                                                    default:
                                                        break;
                                                }
                                            }
                                        }

                                        if (concepto.calcularDolaresClientes)
                                        {
                                            var filtroSaldo = new FiltroSaldoDTO();
                                            filtroSaldo.empresa = empresa;
                                            filtroSaldo.year = year;
                                            filtroSaldo.mes = mes;
                                            filtroSaldo.listaCC = listaCC;
                                            filtroSaldo.listaAC = listaAC;

                                            var clientes = SaldoClientesDolares(_ctx, filtroSaldo);

                                            dolares = clientes.Sum(x => x.total);
                                        }

                                        if (concepto.calcularDolaresProveedores)
                                        {
                                            var filtroSaldo = new FiltroSaldoDTO();
                                            filtroSaldo.empresa = empresa;
                                            filtroSaldo.year = year;
                                            filtroSaldo.mes = mes;
                                            filtroSaldo.listaCC = listaCC;
                                            filtroSaldo.listaAC = listaAC;

                                            var proveedores = SaldoProveedoresDolares(_ctx, filtroSaldo);

                                            dolares += proveedores.Sum(x => x.total);
                                        }

                                        datoTabla.corte = Math.Truncate(Math.Round(corte / 1000));
                                        datoTabla.corteAnterior = Math.Truncate(Math.Round(corteAnterior / 1000));
                                        datoTabla.dolares = Math.Truncate(Math.Round(dolares / 1000));
                                        datoTabla.variacion = Math.Truncate(datoTabla.corte - datoTabla.corteAnterior);
                                        datosGrupo.Add(datoTabla);
                                    }

                                    var datoTablaGrupo = new BalanceDTO();
                                    datoTablaGrupo.concepto = grupo.descripcion;
                                    datoTablaGrupo.tipoDetalle = TipoDetalleEnum.NOMBRE;
                                    datoTablaGrupo.renglonGrupo = true;
                                    datoTablaGrupo.sumarTotal = grupo.sumarTotal;

                                    if (grupo.esGranTotal)
                                    {
                                        datoTablaGrupo.corte = datosTabla.Where(x => x.sumarTotal).Sum(x => x.corte);
                                        datoTablaGrupo.corteAnterior = datosTabla.Where(x => x.sumarTotal).Sum(x => x.corteAnterior);
                                        datoTablaGrupo.dolares = datosTabla.Where(x => x.sumarTotal).Sum(x => x.dolares);
                                    }
                                    else
                                    {
                                        datoTablaGrupo.corte = datosGrupo.Sum(x => x.corte);
                                        datoTablaGrupo.corteAnterior = datosGrupo.Sum(x => x.corteAnterior);
                                        datoTablaGrupo.dolares = datosGrupo.Sum(x => x.dolares);
                                    }
                                    datoTablaGrupo.variacion = datoTablaGrupo.corte - datoTablaGrupo.corteAnterior;

                                    datosTabla.AddRange(datosGrupo);
                                    datosTabla.Add(datoTablaGrupo);
                                }
                            }
                        }
                        #endregion
                        break;
                    default:
                        #region Balance consolidado
                        {
                            using (var _ctxCplan = new MainContext(EmpresaEnum.Construplan))
                            {
                                var tB = (int)tipoBalance + 2;

                                var balance = _ctxCplan.tblEF_BalanceTipoBalance.First(x => x.id == tB && x.esConsolidado && x.registroActivo);

                                var gruposGenerales = _ctxCplan.tblEF_BalanceGrupoConsolidado
                                    .Where(x => x.registroActivo &&  x.tipoBalanceId == balance.id).OrderBy(x => x.orden).ToList();

                                foreach (var grupoGeneral in gruposGenerales)
                                {
                                    var datosGrupo = new List<BalanceDTO>();

                                    foreach (var conceptoGeneral in grupoGeneral.conceptos.Where(x => x.registroActivo).OrderBy(x => x.orden))
                                    {
                                        var datoTabla = new BalanceDTO();
                                        datoTabla.concepto = conceptoGeneral.descripcion;
                                        datoTabla.tipoDetalle = (TipoDetalleEnum)conceptoGeneral.tipoDetalleId;
                                        if (conceptoGeneral.esSubtitulo)
                                        {
                                            datoTabla.renglonSubTitulo = true;
                                        }
                                        if (conceptoGeneral.tieneEnlace)
                                        {
                                            datoTabla.renglonEnlace = true;
                                        }

                                        var corte = 0M;
                                        var corteAnterior = 0M;
                                        var dolares = 0M;

                                        foreach (var relacionConceptoEmpresa in conceptoGeneral.conceptosEmpresa)
                                        {
                                            foreach (var empresa in listaEmpresas)
                                            {
                                                using (var _ctxEmpresa = new MainContext(empresa))
                                                {
                                                    tblEF_BalanceConcepto conceptoEmpresa = null;

                                                    switch (empresa)
                                                    {
                                                        case EmpresaEnum.Construplan:
                                                            conceptoEmpresa = _ctxEmpresa.tblEF_BalanceConcepto
                                                                .FirstOrDefault(x => x.id == relacionConceptoEmpresa.conceptoConstruplanId);
                                                            break;
                                                        case EmpresaEnum.Arrendadora:
                                                            conceptoEmpresa = _ctxEmpresa.tblEF_BalanceConcepto
                                                                .FirstOrDefault(x => x.id == relacionConceptoEmpresa.conceptoArrendadoraId);
                                                            break;
                                                        case EmpresaEnum.EICI:
                                                            conceptoEmpresa = _ctxEmpresa.tblEF_BalanceConcepto
                                                                .FirstOrDefault(x => x.id == relacionConceptoEmpresa.conceptoEiciId);
                                                            break;
                                                        case EmpresaEnum.Integradora:
                                                            conceptoEmpresa = _ctxEmpresa.tblEF_BalanceConcepto
                                                                .FirstOrDefault(x => x.id == relacionConceptoEmpresa.conceptoIntegradoraId);
                                                            break;
                                                    }

                                                    if (conceptoEmpresa != null)
                                                    {
                                                        if (conceptoEmpresa.dxpLp)
                                                        {
                                                            corte -= pesosCirculanteLP / listaEmpresas.Count;
                                                            dolares -= dolaresCirculanteLP / listaEmpresas.Count;

                                                            corteAnterior -= pesosCirculanteLPAnterior / listaEmpresas.Count;
                                                        }

                                                        if (conceptoEmpresa.dxpLpCirculante)
                                                        {
                                                            var circulanteLP = CalcularDxpCirculanteLP(_ctxEmpresa, fechaAnioMes, empresa, tipoCambio, tipoCambioMesAnterior);

                                                            var pesosLP = (decimal)circulanteLP["pesos"];
                                                            var dolaresLP = (decimal)circulanteLP["dolares"];
                                                            var pesosLPAnterior = (decimal)circulanteLP["pesosAnterior"];
                                                            var dolaresLPAnterior = (decimal)circulanteLP["dolaresAnterior"];

                                                            corte += pesosLP;
                                                            dolares += dolaresLP;

                                                            pesosCirculanteLP += pesosLP;
                                                            dolaresCirculanteLP += dolaresLP;

                                                            corteAnterior += pesosLPAnterior;

                                                            pesosCirculanteLPAnterior += pesosLPAnterior;
                                                        }
                                                        else
                                                        {
                                                            foreach (var cuenta in conceptoEmpresa.cuentas.Where(x => x.registroActivo))
                                                            {
                                                                var filtroSaldo = new FiltroSaldoDTO();
                                                                filtroSaldo.empresa = empresa;
                                                                filtroSaldo.year = year;
                                                                filtroSaldo.mes = mes;
                                                                filtroSaldo.cta = cuenta.cta;
                                                                filtroSaldo.scta = cuenta.scta;
                                                                filtroSaldo.sscta = cuenta.sscta;
                                                                filtroSaldo.cc = cuenta.cc;
                                                                filtroSaldo.areaCuenta = cuenta.areaCuenta;
                                                                filtroSaldo.listaCC = listaCC;
                                                                filtroSaldo.listaAC = listaAC;

                                                                var saldosCC = new List<tblEF_SaldoCC>();
                                                                var clientes = new List<tblEF_MovimientoCliente>();
                                                                var proveedores = new List<tblEF_MovimientoProveedor>();

                                                                switch (cuenta.tipoCuentaId)
                                                                {
                                                                    case TipoCuentaEnum.CUENTA:
                                                                        {
                                                                            saldosCC = SaldoBalance(_ctxEmpresa, filtroSaldo);

                                                                            if (cuenta.tipoOperacion == TipoOperacionEnum.suma && !cuenta.invertirSigno)
                                                                            {
                                                                                if (!cuenta.esCuentaDolar)
                                                                                {
                                                                                    corte += saldosCC
                                                                                        .Where(x => x.corte.anio == year && x.corte.mes == mes)
                                                                                        .Sum(x => (decimal?)x.saldoInicial + (decimal?)x.cargosAcumulados + (decimal?)x.abonosAcumulados) ?? 0;
                                                                                    corteAnterior += saldosCC
                                                                                        .Where(x => x.corte.mes != mes)
                                                                                        .Sum(x => (decimal?)x.saldoInicial + (decimal?)x.cargosAcumulados + (decimal?)x.abonosAcumulados) ?? 0;
                                                                                }
                                                                                else
                                                                                {
                                                                                    dolares += saldosCC
                                                                                        .Where(x => x.corte.anio == year && x.corte.mes == mes)
                                                                                        .Sum(x => (decimal?)x.saldoInicial + (decimal?)x.cargosAcumulados + (decimal?)x.abonosAcumulados) ?? 0;
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                if (!cuenta.esCuentaDolar)
                                                                                {
                                                                                    corte -= saldosCC
                                                                                        .Where(x => x.corte.anio == year && x.corte.mes == mes)
                                                                                        .Sum(x => (decimal?)x.saldoInicial + (decimal?)x.cargosAcumulados + (decimal?)x.abonosAcumulados) ?? 0;
                                                                                    corteAnterior -= saldosCC
                                                                                        .Where(x => x.corte.mes != mes)
                                                                                        .Sum(x => (decimal?)x.saldoInicial + (decimal?)x.cargosAcumulados + (decimal?)x.abonosAcumulados) ?? 0;
                                                                                }
                                                                                else
                                                                                {
                                                                                    dolares -= saldosCC
                                                                                        .Where(x => x.corte.anio == year && x.corte.mes == mes)
                                                                                        .Sum(x => (decimal?)x.saldoInicial + (decimal?)x.cargosAcumulados + (decimal?)x.abonosAcumulados) ?? 0;
                                                                                }
                                                                            }
                                                                        }
                                                                        break;
                                                                    case TipoCuentaEnum.CLIENTE:
                                                                        {
                                                                            clientes = SaldoCliente(_ctxEmpresa, filtroSaldo);

                                                                            if (cuenta.tipoOperacion == TipoOperacionEnum.suma && !cuenta.invertirSigno)
                                                                            {
                                                                                if (!cuenta.esCuentaDolar)
                                                                                {
                                                                                    corte += clientes
                                                                                        .Where(x => x.corte.anio == year && x.corte.mes == mes)
                                                                                        .Sum(x => (decimal?)x.total) ?? 0;
                                                                                    corteAnterior += clientes
                                                                                        .Where(x => x.corte.mes != mes)
                                                                                        .Sum(x => (decimal?)x.total) ?? 0;
                                                                                }
                                                                                else
                                                                                {
                                                                                    corte += (clientes
                                                                                        .Where(x => x.corte.anio == year && x.corte.mes == mes)
                                                                                        .Sum(x => (decimal?)x.total) ?? 0) * tipoCambio;
                                                                                    corteAnterior += (clientes
                                                                                        .Where(x => x.corte.mes != mes)
                                                                                        .Sum(x => (decimal?)x.total) ?? 0) * tipoCambio;
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                if (!cuenta.esCuentaDolar)
                                                                                {
                                                                                    corte -= clientes
                                                                                        .Where(x => x.corte.anio == year && x.corte.mes == mes)
                                                                                        .Sum(x => (decimal?)x.total) ?? 0;
                                                                                    corteAnterior -= clientes
                                                                                        .Where(x => x.corte.mes != mes)
                                                                                        .Sum(x => (decimal?)x.total) ?? 0;
                                                                                }
                                                                                else
                                                                                {
                                                                                    corte -= (clientes
                                                                                        .Where(x => x.corte.anio == year && x.corte.mes == mes)
                                                                                        .Sum(x => (decimal?)x.total) ?? 0) * tipoCambio;
                                                                                    corteAnterior -= (clientes
                                                                                        .Where(x => x.corte.mes != mes)
                                                                                        .Sum(x => (decimal?)x.total) ?? 0) * tipoCambio;
                                                                                }
                                                                            }
                                                                        }
                                                                        break;
                                                                    case TipoCuentaEnum.PROVEEDOR:
                                                                        {
                                                                            proveedores = SaldoProveedor(_ctxEmpresa, filtroSaldo);

                                                                            if (cuenta.tipoOperacion == TipoOperacionEnum.suma && !cuenta.invertirSigno)
                                                                            {
                                                                                if (!cuenta.esCuentaDolar)
                                                                                {
                                                                                    corte += proveedores
                                                                                        .Where(x => x.corte.anio == year && x.corte.mes == mes)
                                                                                        .Sum(x => (decimal?)x.total) ?? 0;
                                                                                    corteAnterior += proveedores
                                                                                        .Where(x => x.corte.mes != mes)
                                                                                        .Sum(x => (decimal?)x.total) ?? 0;
                                                                                }
                                                                                else
                                                                                {
                                                                                    corte += (proveedores
                                                                                        .Where(x => x.corte.anio == year && x.corte.mes == mes)
                                                                                        .Sum(x => (decimal?)x.total) ?? 0) * tipoCambio;
                                                                                    corteAnterior += (proveedores
                                                                                        .Where(x => x.corte.mes != mes)
                                                                                        .Sum(x => (decimal?)x.total) ?? 0) * tipoCambio;
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                if (!cuenta.esCuentaDolar)
                                                                                {
                                                                                    corte -= proveedores
                                                                                        .Where(x => x.corte.anio == year && x.corte.mes == mes)
                                                                                        .Sum(x => (decimal?)x.total) ?? 0;
                                                                                    corteAnterior -= proveedores
                                                                                        .Where(x => x.corte.mes != mes)
                                                                                        .Sum(x => (decimal?)x.total) ?? 0;
                                                                                }
                                                                                else
                                                                                {
                                                                                    corte -= (proveedores
                                                                                        .Where(x => x.corte.anio == year && x.corte.mes == mes)
                                                                                        .Sum(x => (decimal?)x.total) ?? 0) * tipoCambio;
                                                                                    corteAnterior -= (proveedores
                                                                                        .Where(x => x.corte.mes != mes)
                                                                                        .Sum(x => (decimal?)x.total) ?? 0) * tipoCambio;
                                                                                }
                                                                            }
                                                                        }
                                                                        break;
                                                                }
                                                            }
                                                        }
                                                    }

                                                    if (conceptoGeneral != null && conceptoGeneral.calcularDolaresClientes)
                                                    {
                                                        var filtroSaldo = new FiltroSaldoDTO();
                                                        filtroSaldo.empresa = empresa;
                                                        filtroSaldo.year = year;
                                                        filtroSaldo.mes = mes;
                                                        filtroSaldo.listaCC = listaCC;
                                                        filtroSaldo.listaAC = listaAC;

                                                        var clientes = SaldoClientesDolares(_ctxEmpresa, filtroSaldo);

                                                        dolares += clientes.Sum(x => x.total);
                                                    }

                                                    if (conceptoEmpresa != null && conceptoEmpresa.calcularDolaresProveedores)
                                                    {
                                                        var filtroSaldo = new FiltroSaldoDTO();
                                                        filtroSaldo.empresa = empresa;
                                                        filtroSaldo.year = year;
                                                        filtroSaldo.mes = mes;
                                                        filtroSaldo.listaCC = listaCC;
                                                        filtroSaldo.listaAC = listaAC;

                                                        var proveedores = SaldoProveedoresDolares(_ctxEmpresa, filtroSaldo);

                                                        dolares += proveedores.Sum(x => x.total);
                                                    }

                                                    if (conceptoGeneral != null && conceptoGeneral.esProveedoresRelacionados)
                                                    {
                                                        var relacionProveedorDolares = _ctxCplan.tblEF_ClienteProveedorRelacion
                                                            .Where(x =>
                                                                x.empresaId == empresa &&
                                                                x.numeroRelacionado >= 9000 &&
                                                                x.empresaRelacionadaId != empresa &&
                                                                listaEmpresas.Contains(x.empresaRelacionadaId) &&
                                                                x.tipoRelacion == TipoRelacionEnum.PROVEEDOR &&
                                                                x.registroActivo
                                                            ).ToList();

                                                        foreach (var clienteProveedor in relacionProveedorDolares)
                                                        {
                                                            var filtro = new FiltroSaldoDTO();
                                                            filtro.empresa = empresa;
                                                            filtro.year = year;
                                                            filtro.mes = mes;
                                                            filtro.cta = clienteProveedor.numeroRelacionado;
                                                            filtro.listaCC = listaCC;
                                                            filtro.listaAC = listaAC;

                                                            var proveedor = SaldoProveedorUnPeriodo(_ctxEmpresa, filtro);

                                                            dolares -= proveedor.Sum(x => x.total);
                                                        }
                                                    }

                                                    if (conceptoGeneral != null && conceptoGeneral.esClientesRelacionados)
                                                    {
                                                        var relacionClienteDolares = _ctxCplan.tblEF_ClienteProveedorRelacion
                                                            .Where(x =>
                                                                x.empresaId == empresa &&
                                                                x.numeroRelacionado >= 9000 &&
                                                                x.empresaRelacionadaId != empresa &&
                                                                listaEmpresas.Contains(x.empresaRelacionadaId) &&
                                                                x.tipoRelacion == TipoRelacionEnum.CLIENTE &&
                                                                x.registroActivo
                                                            ).ToList();

                                                        foreach (var clienteProveedor in relacionClienteDolares)
                                                        {
                                                            var filtro = new FiltroSaldoDTO();
                                                            filtro.empresa = empresa;
                                                            filtro.year = year;
                                                            filtro.mes = mes;
                                                            filtro.cta = clienteProveedor.numeroRelacionado;
                                                            filtro.listaCC = listaCC;
                                                            filtro.listaAC = listaAC;

                                                            var cliente = SaldoClienteUnPeriodo(_ctxEmpresa, filtro);

                                                            dolares -= cliente.Sum(x => x.total);
                                                        }

                                                        //var relacionCliente = _ctxCplan.tblEF_ClienteProveedorRelacion
                                                        //    .Where(x =>
                                                        //        x.empresaId == empresa &&
                                                        //        x.numeroRelacionado < 9000 &&
                                                        //        x.empresaRelacionadaId != empresa &&
                                                        //        listaEmpresas.Contains(x.empresaRelacionadaId) &&
                                                        //        x.tipoRelacion == TipoRelacionEnum.CLIENTE &&
                                                        //        x.registroActivo
                                                        //    ).ToList();

                                                        //foreach (var clienteProveedor in relacionCliente)
                                                        //{
                                                        //    var filtro = new FiltroSaldoDTO();
                                                        //    filtro.empresa = empresa;
                                                        //    filtro.year = year;
                                                        //    filtro.mes = mes;
                                                        //    filtro.cta = clienteProveedor.numeroRelacionado;
                                                        //    filtro.listaAC = listaAC;
                                                        //    filtro.listaCC = listaCC;

                                                        //    var cliente = SaldoCliente(_ctxEmpresa, filtro);

                                                        //    corte -= cliente
                                                        //        .Where(x => x.corte.mes == mes)
                                                        //        .Sum(x => x.total);
                                                        //    corteAnterior -= cliente
                                                        //        .Where(x => x.corte.mes != mes)
                                                        //        .Sum(x => x.total);
                                                        //}
                                                    }
                                                }
                                            }//End ciclo empresas
                                        }//End ciclo conceptoEmpresa

                                        datoTabla.corte = Math.Truncate(Math.Round(corte / 1000));
                                        datoTabla.corteAnterior = Math.Truncate(Math.Round(corteAnterior / 1000));
                                        datoTabla.dolares = Math.Truncate(Math.Round(dolares / 1000));
                                        datoTabla.variacion = Math.Truncate(datoTabla.corte - datoTabla.corteAnterior);
                                        datosGrupo.Add(datoTabla);
                                    }//End ciclo conceptosGenerales

                                    var datoTablaGrupo = new BalanceDTO();
                                    datoTablaGrupo.concepto = grupoGeneral.descripcion;
                                    datoTablaGrupo.tipoDetalle = TipoDetalleEnum.NOMBRE;
                                    datoTablaGrupo.renglonGrupo = true;
                                    datoTablaGrupo.sumarTotal = grupoGeneral.sumarTotal;

                                    if (grupoGeneral.esGranTotal)
                                    {
                                        datoTablaGrupo.corte = datosTabla.Where(x => x.sumarTotal).Sum(x => x.corte);
                                        datoTablaGrupo.corteAnterior = datosTabla.Where(x => x.sumarTotal).Sum(x => x.corteAnterior);
                                        datoTablaGrupo.dolares = datosTabla.Where(x => x.sumarTotal).Sum(x => x.dolares);
                                    }
                                    else
                                    {
                                        datoTablaGrupo.corte = datosGrupo.Sum(x => x.corte);
                                        datoTablaGrupo.corteAnterior = datosGrupo.Sum(x => x.corteAnterior);
                                        datoTablaGrupo.dolares = datosGrupo.Sum(x => x.dolares);
                                    }
                                    datoTablaGrupo.variacion = datoTablaGrupo.corte - datoTablaGrupo.corteAnterior;

                                    datosTabla.AddRange(datosGrupo);
                                    datosTabla.Add(datoTablaGrupo);
                                }//End ciclo gruposGenerales
                            }//End using
                        }
                        #endregion
                        break;
                }

                resultado.Add(SUCCESS, true);
                resultado.Add("data", datosTabla);
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, ex.Message);
                LogError(0, 0, "EstadosFinancierosController", "calcularBalanceGeneral", ex, AccionEnum.CONSULTA, 0, new { fechaAnioMes = fechaAnioMes, listaCC = listaCC, listaEmpresas = listaEmpresas });
            }

            return resultado;
        }

        private List<tblEF_SaldoCC> SaldoBalance(MainContext ctx, FiltroSaldoDTO filtro)
        {
            var saldosCC = new List<tblEF_SaldoCC>();

            var mesAnterior = filtro.mes - 1;
            var yearAnterior = filtro.year;

            if (mesAnterior <= 0)
            {
                yearAnterior -= 1;
                mesAnterior = 12;
            }

            switch (filtro.empresa)
            {
                case EmpresaEnum.Integradora:
                case EmpresaEnum.EICI:
                case EmpresaEnum.Construplan:
                    saldosCC = ctx.tblEF_SaldoCC
                        .Where(x =>
                            (
                                (x.corte.anio == yearAnterior && x.corte.mes == mesAnterior) ||
                                (x.corte.anio == filtro.year && x.corte.mes == filtro.mes)
                            ) &&
                            x.corte.estatus &&
                            x.cta == filtro.cta &&
                            (filtro.scta > 0 ? x.scta == filtro.scta : true) &&
                            (filtro.sscta > 0 ? x.sscta == filtro.sscta : true) &&
                            (filtro.listaCC.Count > 0 ? filtro.listaCC.Contains(x.cc) : true) &&
                            (string.IsNullOrEmpty(filtro.cc) ? true : x.cc == filtro.cc) &&
                            x.estatus
                        ).ToList();
                    saldosCC.ForEach(x => x.saldoInicial = (x.cta >= 4000 && x.cta < 6000) ? 0 : x.saldoInicial);
                    break;
                case EmpresaEnum.Arrendadora:
                    saldosCC = ctx.tblEF_SaldoCC
                        .Where(x =>
                            (
                                (x.corte.anio == yearAnterior && x.corte.mes == mesAnterior) ||
                                (x.corte.anio == filtro.year && x.corte.mes == filtro.mes)
                            ) &&
                            x.corte.estatus &&
                            x.cta == filtro.cta &&
                            (filtro.scta > 0 ? x.scta == filtro.scta : true) &&
                            (filtro.sscta > 0 ? x.sscta == filtro.sscta : true) &&
                            (filtro.listaAC.Count > 0 ? filtro.listaAC.Contains(x.areaCuenta) : true) &&
                            (string.IsNullOrEmpty(filtro.areaCuenta) ? true : x.areaCuenta == filtro.areaCuenta) &&
                            x.estatus
                        ).ToList();
                    saldosCC.ForEach(x => x.saldoInicial = (x.cta >= 4000 && x.cta < 6000) ? 0 : x.saldoInicial);
                    break;
            }

            return saldosCC;
        }

        private List<tblEF_SaldoCC> SaldoBalanceUnPeriodo(MainContext ctx, FiltroSaldoDTO filtro)
        {
            var saldosCC = new List<tblEF_SaldoCC>();

            switch (filtro.empresa)
            {
                case EmpresaEnum.Integradora:
                case EmpresaEnum.EICI:
                case EmpresaEnum.Construplan:
                    saldosCC = ctx.tblEF_SaldoCC
                        .Where(x =>
                            x.corte.anio == filtro.year &&
                            x.corte.mes == filtro.mes &&
                            x.corte.estatus &&
                            x.cta == filtro.cta &&
                            (filtro.scta > 0 ? x.scta == filtro.scta : true) &&
                            (filtro.sscta > 0 ? x.sscta == filtro.sscta : true) &&
                            (filtro.listaCC.Count > 0 ? filtro.listaCC.Contains(x.cc) : true) &&
                            (string.IsNullOrEmpty(filtro.cc) ? true : x.cc == filtro.cc) &&
                            x.estatus)
                        .ToList();
                    saldosCC.ForEach(x => x.saldoInicial = (x.cta >= 4000 && x.cta < 6000) ? 0 : x.saldoInicial);
                    break;
                case EmpresaEnum.Arrendadora:
                    saldosCC = ctx.tblEF_SaldoCC
                        .Where(x =>
                            x.corte.anio == filtro.year &&
                            x.corte.mes == filtro.mes &&
                            x.corte.estatus &&
                            x.cta == filtro.cta &&
                            (filtro.scta > 0 ? x.scta == filtro.scta : true) &&
                            (filtro.sscta > 0 ? x.sscta == filtro.sscta : true) &&
                            (string.IsNullOrEmpty(filtro.areaCuenta) ? true : x.areaCuenta == filtro.areaCuenta) &&
                            (filtro.listaAC.Count > 0 ? filtro.listaAC.Contains(x.areaCuenta) : true) &&
                            x.estatus
                        ).ToList();
                    saldosCC.ForEach(x => x.saldoInicial = (x.cta >= 4000 && x.cta < 6000) ? 0 : x.saldoInicial);
                    break;
            }

            return saldosCC;
        }

        public Dictionary<string, object> GetBalanceDetalle(List<EmpresaEnum> listaEmpresas, DateTime fechaMesCorte, List<string> listaCC, TipoDetalleEnum tipoDetalle, int tipoTablaGeneral)
        {
            try
            {
                fechaMesCorte = new DateTime(fechaMesCorte.Year, fechaMesCorte.Month, DateTime.DaysInMonth(fechaMesCorte.Year, fechaMesCorte.Month));

                List<CentroCostoAreaCuentaDTO> listaAreaCuenta = null;
                List<string> listaAC = new List<string>();

                if (listaCC == null)
                {
                    listaCC = new List<string>();
                    listaAreaCuenta = new List<CentroCostoAreaCuentaDTO>();
                }
                else
                {
                    using (var _contextConstruplan = new MainContext(EmpresaEnum.Construplan))
                    {
                        listaAreaCuenta = _contextConstruplan.tblP_CC.ToList()
                            .Where(x =>
                                (listaCC != null ? listaCC.Contains(x.cc) : true))
                            .Select(x => new CentroCostoAreaCuentaDTO
                            {
                                area = x.area,
                                cuenta = x.cuenta,
                                areaCuenta = x.area.ToString() + "-" + x.cuenta
                            }).ToList();
                    }
                }

                if (listaEmpresas.Contains(EmpresaEnum.Arrendadora))
                {
                    listaAC = listaAreaCuenta.Select(x => x.areaCuenta).ToList();
                }

                var data = new List<dynamic>();
                var corteMes = _context.tblEF_CorteMes.FirstOrDefault(x => x.estatus && x.anio == fechaMesCorte.Year && x.mes == fechaMesCorte.Month);

                if (corteMes == null)
                {
                    throw new Exception("No se encuentra la información del corte.");
                }

                switch (tipoDetalle)
                {
                    case TipoDetalleEnum.NOMBRE:
                        break;
                    case TipoDetalleEnum.BANCO:
                        #region BANCO
                        if (listaEmpresas.Count() == 1)
                        {
                            var listaConceptos = _context.tblEF_BancoConcepto.Where(x => x.registroActivo && !x.consolidado).ToList();
                            var listaGrupos = _context.tblEF_GrupoBancoConcepto.Where(x => x.registroActivo && !x.consolidado).ToList();
                            var listaTipoMovimiento = _context.tblEF_BancoConceptoDetalle.Where(x => x.registroActivo && !x.consolidado).ToList();

                            var saldos = new List<tblEF_SaldoCC>();

                            using (var _contextEmpresa = new MainContext(listaEmpresas[0]))
                            {
                                saldos = _contextEmpresa.tblEF_SaldoCC.Where(x => x.estatus && x.corteMesID == corteMes.id && (x.cta == 1105 || x.cta == 1110 || x.cta == 1115)).ToList();

                                #region Filtro de cc/área-cuenta.
                                if (listaEmpresas[0] != EmpresaEnum.Arrendadora)
                                {
                                    if (listaCC.Count > 0)
                                    {
                                        saldos = saldos.Where(x => (listaCC != null ? listaCC.Contains(x.cc) : true)).ToList();
                                    }
                                }
                                else
                                {
                                    if (listaCC.Count > 0)
                                    {
                                        saldos = saldos.Where(x => x.area.HasValue && (listaAC.Count > 0 ? listaAC.Contains(x.areaCuenta) : true)).ToList();

                                    //    saldos = (
                                    //    from sal in saldos.Where(x => x.area != null && x.cuenta != null).ToList()
                                    //    join ac in listaAreaCuenta on new { area = (int)sal.area, cuenta = (int)sal.cuenta } equals new { area = ac.area, cuenta = ac.cuenta }
                                    //    select sal
                                    //).ToList();
                                    }
                                }
                                #endregion
                            }

                            var mesMontoInicial = 0m;
                            var acumuladoMontoInicial = 0m;

                            foreach (var grupo in listaGrupos)
                            {
                                var conceptosGrupo = listaConceptos.Where(x => x.grupoReporteID == grupo.id).OrderBy(x => x.ordenReporte).ToList();

                                #region Agregar renglones normales
                                var renglones = new List<DetalleBancoDTO>();

                                foreach (var concepto in conceptosGrupo)
                                {
                                    var listaTipoMovimientoConcepto = listaTipoMovimiento.Where(x => x.conceptoID == concepto.id).ToList();
                                    var saldosConcepto = saldos.Where(x => listaTipoMovimientoConcepto.Select(y => y.tm).Contains(x.itm)).ToList();
                                    var mesMonto = saldosConcepto.Sum(x => x.cargosMes + x.abonosMes);
                                    var acumuladoMonto = saldosConcepto.Sum(x => x.cargosAcumulados + x.abonosAcumulados);

                                    if (concepto.ordenReporte == 1)
                                    {
                                        mesMontoInicial = mesMonto;
                                        acumuladoMontoInicial = acumuladoMonto;
                                    }

                                    renglones.Add(new DetalleBancoDTO
                                    {
                                        concepto = concepto.concepto,
                                        mesMonto = mesMonto,
                                        mesMontoResultado = 0,
                                        mesPorcentaje = (concepto.ordenReporte > 1) ? Math.Round(((mesMonto * 100m) / (mesMontoInicial > 0 ? mesMontoInicial : 1)), 2) : 100m,
                                        mesPorcentajeResultado = 0,
                                        acumuladoMonto = acumuladoMonto,
                                        acumuladoMontoResultado = 0,
                                        acumuladoPorcentaje = (concepto.ordenReporte > 1) ? Math.Round(((acumuladoMonto * 100m) / (acumuladoMontoInicial > 0 ? acumuladoMontoInicial : 1)), 2) : 100m,
                                        acumuladoPorcentajeResultado = 0,
                                        renglonGrupo = false
                                    });
                                }

                                data.AddRange(renglones);
                                #endregion

                                #region Agregar renglones grupo
                                if (!grupo.sumaGrupos)
                                {
                                    data.Add(new DetalleBancoDTO
                                    {
                                        concepto = grupo.descripcion,
                                        mesMonto = 0,
                                        mesMontoResultado = renglones.Sum(x => x.mesMonto),
                                        mesPorcentaje = 0,
                                        mesPorcentajeResultado = renglones.Sum(x => x.mesPorcentaje),
                                        acumuladoMonto = 0,
                                        acumuladoMontoResultado = renglones.Sum(x => x.acumuladoMonto),
                                        acumuladoPorcentaje = 0,
                                        acumuladoPorcentajeResultado = renglones.Sum(x => x.acumuladoPorcentaje),
                                        renglonGrupo = true
                                    });
                                }
                                else
                                {
                                    var gruposAgregados = data.Where(x => x.renglonGrupo).ToList();
                                    var ultimosDosGrupos = gruposAgregados.Skip(Math.Max(0, gruposAgregados.Count() - 2)).ToList();

                                    data.Add(new DetalleBancoDTO
                                    {
                                        concepto = grupo.descripcion,
                                        mesMonto = 0,
                                        mesMontoResultado = ultimosDosGrupos[0].mesMontoResultado + ultimosDosGrupos[1].mesMontoResultado,
                                        mesPorcentaje = 0,
                                        mesPorcentajeResultado = ultimosDosGrupos[0].mesPorcentajeResultado + ultimosDosGrupos[1].mesPorcentajeResultado,
                                        acumuladoMonto = 0,
                                        acumuladoMontoResultado = ultimosDosGrupos[0].acumuladoMontoResultado + ultimosDosGrupos[1].acumuladoMontoResultado,
                                        acumuladoPorcentaje = 0,
                                        acumuladoPorcentajeResultado = ultimosDosGrupos[0].acumuladoPorcentajeResultado + ultimosDosGrupos[1].acumuladoPorcentajeResultado,
                                        renglonGrupo = true
                                    });
                                }
                                #endregion
                            }

                            #region Agregar renglones especiales
                            if (listaEmpresas[0] != EmpresaEnum.Arrendadora)
                            {
                                var saldosDividendos = saldos.Where(x => x.itm == 77).ToList();
                                var mesMontoDividendos = saldosDividendos.Sum(x => x.cargosMes + x.abonosMes);
                                var mesPorcentajeDividendos = (data.Count() > 0) ? Math.Round(((mesMontoDividendos * 100m) / (mesMontoInicial > 0 ? mesMontoInicial : 1)), 2) : 100m;
                                var acumuladoMontoDividendos = saldosDividendos.Sum(x => x.cargosAcumulados + x.abonosAcumulados);
                                var acumuladoPorcentajeDividendos = (data.Count() > 0) ? Math.Round(((acumuladoMontoDividendos * 100m) / (acumuladoMontoInicial > 0 ? acumuladoMontoInicial : 1)), 2) : 100m;

                                data.Add(new DetalleBancoDTO
                                {
                                    concepto = "DIVIDENDOS PAGADOS",
                                    mesMonto = mesMontoDividendos,
                                    mesMontoResultado = 0,
                                    mesPorcentaje = mesPorcentajeDividendos,
                                    mesPorcentajeResultado = 0,
                                    acumuladoMonto = acumuladoMontoDividendos,
                                    acumuladoMontoResultado = 0,
                                    acumuladoPorcentaje = acumuladoPorcentajeDividendos,
                                    acumuladoPorcentajeResultado = 0,
                                    renglonGrupo = false
                                });

                                var mesMontoGastos = saldos.Where(x => x.cc == "A03").Sum(x => x.cargosMes + x.abonosMes);
                                var mesPorcentajeGastos = Math.Round(((mesMontoGastos * 100m) / (mesMontoInicial > 0 ? mesMontoInicial : 1)), 2);
                                var acumuladoMontoGastos = saldos.Where(x => x.cc == "A03").Sum(x => x.cargosAcumulados + x.abonosAcumulados);
                                var acumuladoPorcentajeGastos = Math.Round(((acumuladoMontoGastos * 100m) / (acumuladoMontoInicial > 0 ? acumuladoMontoInicial : 1)), 2);

                                data.Add(new DetalleBancoDTO
                                {
                                    concepto = "GASTOS CORPORATIVOS",
                                    mesMonto = mesMontoGastos,
                                    mesMontoResultado = mesMontoDividendos + mesMontoGastos,
                                    mesPorcentaje = mesPorcentajeGastos,
                                    mesPorcentajeResultado = mesPorcentajeDividendos + mesPorcentajeGastos,
                                    acumuladoMonto = acumuladoMontoGastos,
                                    acumuladoMontoResultado = acumuladoMontoDividendos + acumuladoMontoGastos,
                                    acumuladoPorcentaje = acumuladoPorcentajeGastos,
                                    acumuladoPorcentajeResultado = acumuladoPorcentajeDividendos + acumuladoPorcentajeGastos,
                                    renglonGrupo = false
                                });

                                var gruposAgregados = data.Where(x => x.renglonGrupo).ToList();
                                var ultimoGrupo = gruposAgregados.Skip(Math.Max(0, gruposAgregados.Count() - 1)).ToList();
                                var mesMontoResultadoSinNombre = ultimoGrupo[0].mesMontoResultado + data.Last().mesMontoResultado;
                                var acumuladoMontoResultadoSinNombre = ultimoGrupo[0].acumuladoMontoResultado + data.Last().acumuladoMontoResultado;

                                data.Add(new DetalleBancoDTO
                                {
                                    concepto = "",
                                    mesMonto = 0,
                                    mesMontoResultado = mesMontoResultadoSinNombre,
                                    mesPorcentaje = 0,
                                    mesPorcentajeResultado = ultimoGrupo[0].mesPorcentajeResultado + data.Last().mesPorcentajeResultado,
                                    acumuladoMonto = 0,
                                    acumuladoMontoResultado = acumuladoMontoResultadoSinNombre,
                                    acumuladoPorcentaje = 0,
                                    acumuladoPorcentajeResultado = ultimoGrupo[0].acumuladoPorcentajeResultado + data.Last().acumuladoPorcentajeResultado,
                                    renglonGrupo = true
                                });

                                var saldosIncremento = saldos.Where(x => x.itm == 9 || x.itm == 73).ToList();
                                var mesMontoResultadoIncremento = saldosIncremento.Sum(x => x.cargosMes + x.abonosMes);
                                var mesPorcentajeResultadoIncremento = (data.Count() > 0) ? Math.Round(((mesMontoResultadoIncremento * 100m) / (mesMontoInicial > 0 ? mesMontoInicial : 1)), 2) : 100m;
                                var acumuladoMontoResultadoIncremento = saldosIncremento.Sum(x => x.cargosAcumulados + x.abonosAcumulados);
                                var acumuladoPorcentajeResultadoIncremento = (data.Count() > 0) ? Math.Round(((acumuladoMontoResultadoIncremento * 100m) / (acumuladoMontoInicial > 0 ? acumuladoMontoInicial : 1)), 2) : 100m;

                                data.Add(new DetalleBancoDTO
                                {
                                    concepto = "INCREMENTO (DISMINUCIÓN) POR TIPO CAMBIO",
                                    mesMonto = 0,
                                    mesMontoResultado = mesMontoResultadoIncremento,
                                    mesPorcentaje = 0,
                                    mesPorcentajeResultado = mesPorcentajeResultadoIncremento,
                                    acumuladoMonto = 0,
                                    acumuladoMontoResultado = acumuladoMontoResultadoIncremento,
                                    acumuladoPorcentaje = 0,
                                    acumuladoPorcentajeResultado = acumuladoPorcentajeResultadoIncremento,
                                    renglonGrupo = true
                                });

                                var mesMontoResultadoInicio = 0m;
                                var acumuladoMontoResultadoInicio = 0m;

                                data.Add(new DetalleBancoDTO
                                {
                                    concepto = "EFECTIVO AL INICIO DEL PERIODO",
                                    mesMonto = 0,
                                    mesMontoResultado = mesMontoResultadoInicio,
                                    mesPorcentaje = 0,
                                    mesPorcentajeResultado = 0,
                                    acumuladoMonto = 0,
                                    acumuladoMontoResultado = acumuladoMontoResultadoInicio,
                                    acumuladoPorcentaje = 0,
                                    acumuladoPorcentajeResultado = 0,
                                    renglonGrupo = true
                                });

                                var mesMontoResultadoFinal = mesMontoResultadoSinNombre + mesMontoResultadoIncremento + mesMontoResultadoInicio;
                                var acumuladoMontoResultadoFinal = acumuladoMontoResultadoSinNombre + acumuladoMontoResultadoIncremento + acumuladoMontoResultadoInicio;

                                data.Add(new DetalleBancoDTO
                                {
                                    concepto = "EFECTIVO AL FINAL DEL PERIODO",
                                    mesMonto = 0,
                                    mesMontoResultado = mesMontoResultadoFinal,
                                    mesPorcentaje = 0,
                                    mesPorcentajeResultado = 0,
                                    acumuladoMonto = 0,
                                    acumuladoMontoResultado = acumuladoMontoResultadoFinal,
                                    acumuladoPorcentaje = 0,
                                    acumuladoPorcentajeResultado = 0,
                                    renglonGrupo = true
                                });
                            }
                            else
                            {
                                var gruposAgregados = data.Where(x => x.renglonGrupo).ToList();
                                var ultimoGrupo = gruposAgregados.Skip(Math.Max(0, gruposAgregados.Count() - 1)).ToList();
                                var mesMontoResultadoUltimoGrupo = ultimoGrupo[0].mesMontoResultado + data.Last().mesMontoResultado;
                                var acumuladoMontoResultadoUltimoGrupo = ultimoGrupo[0].acumuladoMontoResultado + data.Last().acumuladoMontoResultado;

                                var saldosIncremento = saldos.Where(x => x.itm == 9 || x.itm == 73).ToList();
                                var mesMontoResultadoIncremento = saldosIncremento.Sum(x => x.cargosMes + x.abonosMes);
                                var mesPorcentajeResultadoIncremento = (data.Count() > 0) ? Math.Round(((mesMontoResultadoIncremento * 100m) / (mesMontoInicial > 0 ? mesMontoInicial : 1)), 2) : 100m;
                                var acumuladoMontoResultadoIncremento = saldosIncremento.Sum(x => x.cargosAcumulados + x.abonosAcumulados);
                                var acumuladoPorcentajeResultadoIncremento = (data.Count() > 0) ? Math.Round(((acumuladoMontoResultadoIncremento * 100m) / (acumuladoMontoInicial > 0 ? acumuladoMontoInicial : 1)), 2) : 100m;

                                data.Add(new DetalleBancoDTO
                                {
                                    concepto = "INCREMENTO (DISMINUCIÓN) POR TIPO CAMBIO",
                                    mesMonto = 0,
                                    mesMontoResultado = mesMontoResultadoIncremento,
                                    mesPorcentaje = 0,
                                    mesPorcentajeResultado = mesPorcentajeResultadoIncremento,
                                    acumuladoMonto = 0,
                                    acumuladoMontoResultado = acumuladoMontoResultadoIncremento,
                                    acumuladoPorcentaje = 0,
                                    acumuladoPorcentajeResultado = acumuladoPorcentajeResultadoIncremento,
                                    renglonGrupo = true
                                });

                                var mesMontoResultadoInicio = 0m;
                                var acumuladoMontoResultadoInicio = 0m;

                                data.Add(new DetalleBancoDTO
                                {
                                    concepto = "EFECTIVO AL INICIO DEL PERIODO",
                                    mesMonto = 0,
                                    mesMontoResultado = mesMontoResultadoInicio,
                                    mesPorcentaje = 0,
                                    mesPorcentajeResultado = 0,
                                    acumuladoMonto = 0,
                                    acumuladoMontoResultado = acumuladoMontoResultadoInicio,
                                    acumuladoPorcentaje = 0,
                                    acumuladoPorcentajeResultado = 0,
                                    renglonGrupo = true
                                });

                                var mesMontoResultadoFinal = mesMontoResultadoUltimoGrupo + mesMontoResultadoIncremento + mesMontoResultadoInicio;
                                var acumuladoMontoResultadoFinal = acumuladoMontoResultadoUltimoGrupo + acumuladoMontoResultadoIncremento + acumuladoMontoResultadoInicio;

                                data.Add(new DetalleBancoDTO
                                {
                                    concepto = "EFECTIVO AL FINAL DEL PERIODO",
                                    mesMonto = 0,
                                    mesMontoResultado = mesMontoResultadoFinal,
                                    mesPorcentaje = 0,
                                    mesPorcentajeResultado = 0,
                                    acumuladoMonto = 0,
                                    acumuladoMontoResultado = acumuladoMontoResultadoFinal,
                                    acumuladoPorcentaje = 0,
                                    acumuladoPorcentajeResultado = 0,
                                    renglonGrupo = true
                                });
                            }
                            #endregion
                        }
                        else if (listaEmpresas.Count() > 1)
                        {
                            var listaConceptos = _context.tblEF_BancoConcepto.Where(x => x.registroActivo && x.consolidado).ToList();
                            var listaGrupos = _context.tblEF_GrupoBancoConcepto.Where(x => x.registroActivo && x.consolidado).ToList();
                            var listaTipoMovimiento = _context.tblEF_BancoConceptoDetalle.Where(x => x.registroActivo && x.consolidado).ToList();

                            var mesMontoInicial = 0m;
                            var acumuladoMontoInicial = 0m;

                            foreach (var grupo in listaGrupos)
                            {
                                var conceptosGrupo = listaConceptos.Where(x => x.grupoReporteID == grupo.id).OrderBy(x => x.ordenReporte).ToList();

                                #region Agregar renglones normales
                                var renglones = new List<DetalleBancoDTO>();

                                foreach (var concepto in conceptosGrupo)
                                {
                                    var mesMontoConcepto = 0m;
                                    var acumuladoMontoConcepto = 0m;

                                    foreach (var empresa in listaEmpresas)
                                    {
                                        var saldos = new List<tblEF_SaldoCC>();

                                        using (var _contextEmpresa = new MainContext(empresa))
                                        {
                                            saldos = _contextEmpresa.tblEF_SaldoCC.Where(x => x.estatus && x.corteMesID == corteMes.id && (x.cta == 1105 || x.cta == 1110 || x.cta == 1115)).ToList();

                                            #region Filtro de cc/área-cuenta.
                                            if (empresa != EmpresaEnum.Arrendadora)
                                            {
                                                saldos = saldos.Where(x => (listaCC.Count > 0 ? listaCC.Contains(x.cc) : true)).ToList();
                                            }
                                            else
                                            {
                                                saldos = saldos.Where(x => (listaAC.Count > 0 ? listaAC.Contains(x.areaCuenta) : true)).ToList();
                                                //saldos = (
                                                //    from sal in saldos.Where(x => x.area != null && x.cuenta != null).ToList()
                                                //    join ac in listaAreaCuenta on new { area = (int)sal.area, cuenta = (int)sal.cuenta } equals new { area = ac.area, cuenta = ac.cuenta }
                                                //    select sal
                                                //).ToList();
                                            }
                                            #endregion
                                        }

                                        var listaTipoMovimientoConcepto = listaTipoMovimiento.Where(x => x.conceptoID == concepto.id).ToList();
                                        var saldosConcepto = saldos.Where(x => listaTipoMovimientoConcepto.Select(y => y.tm).Contains(x.itm)).ToList();

                                        mesMontoConcepto += saldosConcepto.Sum(x => x.cargosMes + x.abonosMes);
                                        acumuladoMontoConcepto += saldosConcepto.Sum(x => x.cargosAcumulados + x.abonosAcumulados);
                                    }

                                    if (concepto.ordenReporte == 1)
                                    {
                                        mesMontoInicial = mesMontoConcepto;
                                        acumuladoMontoInicial = acumuladoMontoConcepto;
                                    }

                                    renglones.Add(new DetalleBancoDTO
                                    {
                                        concepto = concepto.concepto,
                                        mesMonto = mesMontoConcepto,
                                        mesMontoResultado = 0,
                                        mesPorcentaje = (concepto.ordenReporte > 1) ? Math.Round(((mesMontoConcepto * 100m) / (mesMontoInicial > 0 ? mesMontoInicial : 1)), 2) : 100m,
                                        mesPorcentajeResultado = 0,
                                        acumuladoMonto = acumuladoMontoConcepto,
                                        acumuladoMontoResultado = 0,
                                        acumuladoPorcentaje = (concepto.ordenReporte > 1) ? Math.Round(((acumuladoMontoConcepto * 100m) / (acumuladoMontoInicial > 0 ? acumuladoMontoInicial : 1)), 2) : 100m,
                                        acumuladoPorcentajeResultado = 0,
                                        renglonGrupo = false
                                    });
                                }

                                data.AddRange(renglones);
                                #endregion

                                #region Agregar renglones grupo
                                if (!grupo.sumaGrupos)
                                {
                                    data.Add(new DetalleBancoDTO
                                    {
                                        concepto = grupo.descripcion,
                                        mesMonto = 0,
                                        mesMontoResultado = renglones.Sum(x => x.mesMonto),
                                        mesPorcentaje = 0,
                                        mesPorcentajeResultado = renglones.Sum(x => x.mesPorcentaje),
                                        acumuladoMonto = 0,
                                        acumuladoMontoResultado = renglones.Sum(x => x.acumuladoMonto),
                                        acumuladoPorcentaje = 0,
                                        acumuladoPorcentajeResultado = renglones.Sum(x => x.acumuladoPorcentaje),
                                        renglonGrupo = true
                                    });
                                }
                                else
                                {
                                    var gruposAgregados = data.Where(x => x.renglonGrupo).ToList();
                                    var ultimosDosGrupos = gruposAgregados.Skip(Math.Max(0, gruposAgregados.Count() - 2)).ToList();

                                    data.Add(new DetalleBancoDTO
                                    {
                                        concepto = grupo.descripcion,
                                        mesMonto = 0,
                                        mesMontoResultado = ultimosDosGrupos[0].mesMontoResultado + ultimosDosGrupos[1].mesMontoResultado,
                                        mesPorcentaje = 0,
                                        mesPorcentajeResultado = ultimosDosGrupos[0].mesPorcentajeResultado + ultimosDosGrupos[1].mesPorcentajeResultado,
                                        acumuladoMonto = 0,
                                        acumuladoMontoResultado = ultimosDosGrupos[0].acumuladoMontoResultado + ultimosDosGrupos[1].acumuladoMontoResultado,
                                        acumuladoPorcentaje = 0,
                                        acumuladoPorcentajeResultado = ultimosDosGrupos[0].acumuladoPorcentajeResultado + ultimosDosGrupos[1].acumuladoPorcentajeResultado,
                                        renglonGrupo = true
                                    });
                                }
                                #endregion
                            }

                            #region Agregar renglones especiales
                            var mesMontoDividendos = 0m;
                            var mesPorcentajeDividendos = 0m;
                            var acumuladoMontoDividendos = 0m;
                            var acumuladoPorcentajeDividendos = 0m;

                            var mesMontoGastos = 0m;
                            var mesPorcentajeGastos = 0m;
                            var acumuladoMontoGastos = 0m;
                            var acumuladoPorcentajeGastos = 0m;

                            var gruposAgregadosFinal = data.Where(x => x.renglonGrupo).ToList();
                            var ultimosGrupos = gruposAgregadosFinal.Skip(Math.Max(0, gruposAgregadosFinal.Count() - 2)).ToList();
                            var mesMontoResultadoIncrementoNeta = ultimosGrupos[0].mesMontoResultado + ultimosGrupos[1].mesMontoResultado + data.Last().mesMontoResultado;
                            var mesPorcentajeResultadoIncrementoNeta = ultimosGrupos[0].mesPorcentajeResultado + ultimosGrupos[1].mesPorcentajeResultado + data.Last().mesPorcentajeResultado;
                            var acumuladoMontoResultadoIncrementoNeta = ultimosGrupos[0].acumuladoMontoResultado + ultimosGrupos[1].acumuladoMontoResultado + data.Last().acumuladoMontoResultado;
                            var acumuladoPorcentajeResultadoIncrementoNeta = ultimosGrupos[0].acumuladoPorcentajeResultado + ultimosGrupos[1].acumuladoPorcentajeResultado + data.Last().acumuladoPorcentajeResultado;

                            var mesMontoResultadoIncremento = 0m;
                            var mesPorcentajeResultadoIncremento = 0m;
                            var acumuladoMontoResultadoIncremento = 0m;
                            var acumuladoPorcentajeResultadoIncremento = 0m;

                            var mesMontoResultadoInicio = 0m;
                            var acumuladoMontoResultadoInicio = 0m;

                            var mesMontoResultadoFinal = 0m;
                            var acumuladoMontoResultadoFinal = 0m;

                            foreach (var empresa in listaEmpresas)
                            {
                                var saldos = new List<tblEF_SaldoCC>();

                                using (var _contextEmpresa = new MainContext(empresa))
                                {
                                    saldos = _contextEmpresa.tblEF_SaldoCC.Where(x => x.estatus && x.corteMesID == corteMes.id && (x.cta == 1105 || x.cta == 1110 || x.cta == 1115)).ToList();

                                    #region Filtro de cc/área-cuenta.
                                    if (empresa != EmpresaEnum.Arrendadora)
                                    {
                                        saldos = saldos.Where(x => (listaCC.Count > 0 ? listaCC.Contains(x.cc) : true)).ToList();
                                    }
                                    else
                                    {
                                        saldos = saldos.Where(x => (listaAC.Count > 0 ? listaAC.Contains(x.areaCuenta) : true)).ToList();
                                        //saldos = (
                                        //    from sal in saldos.Where(x => x.area != null && x.cuenta != null).ToList()
                                        //    join ac in listaAreaCuenta on new { area = (int)sal.area, cuenta = (int)sal.cuenta } equals new { area = ac.area, cuenta = ac.cuenta }
                                        //    select sal
                                        //).ToList();
                                    }
                                    #endregion
                                }

                                var saldosDividendos = saldos.Where(x => x.itm == 77).ToList();

                                mesMontoDividendos = saldosDividendos.Sum(x => x.cargosMes + x.abonosMes);
                                mesPorcentajeDividendos = (data.Count() > 0) ? Math.Round(((mesMontoDividendos * 100m) / (mesMontoInicial > 0 ? mesMontoInicial : 1)), 2) : 100m;
                                acumuladoMontoDividendos = saldosDividendos.Sum(x => x.cargosAcumulados + x.abonosAcumulados);
                                acumuladoPorcentajeDividendos = (data.Count() > 0) ? Math.Round(((acumuladoMontoDividendos * 100m) / (acumuladoMontoInicial > 0 ? acumuladoMontoInicial : 1)), 2) : 100m;

                                mesMontoGastos = saldos.Where(x => x.cc == "A03").Sum(x => x.cargosMes + x.abonosMes);
                                mesPorcentajeGastos = Math.Round(((mesMontoGastos * 100m) / (mesMontoInicial > 0 ? mesMontoInicial : 1)), 2);
                                acumuladoMontoGastos = saldos.Where(x => x.cc == "A03").Sum(x => x.cargosAcumulados + x.abonosAcumulados);
                                acumuladoPorcentajeGastos = Math.Round(((acumuladoMontoGastos * 100m) / (acumuladoMontoInicial > 0 ? acumuladoMontoInicial : 1)), 2);

                                var saldosIncremento = saldos.Where(x => x.itm == 9 || x.itm == 73).ToList();

                                mesMontoResultadoIncremento = saldosIncremento.Sum(x => x.cargosMes + x.abonosMes);
                                mesPorcentajeResultadoIncremento = (data.Count() > 0) ? Math.Round(((mesMontoResultadoIncremento * 100m) / (mesMontoInicial > 0 ? mesMontoInicial : 1)), 2) : 100m;
                                acumuladoMontoResultadoIncremento = saldosIncremento.Sum(x => x.cargosAcumulados + x.abonosAcumulados);
                                acumuladoPorcentajeResultadoIncremento = (data.Count() > 0) ? Math.Round(((acumuladoMontoResultadoIncremento * 100m) / (acumuladoMontoInicial > 0 ? acumuladoMontoInicial : 1)), 2) : 100m;

                                mesMontoResultadoInicio = 0m;
                                acumuladoMontoResultadoInicio = 0m;

                                mesMontoResultadoFinal = mesMontoResultadoIncrementoNeta + mesMontoResultadoIncremento + mesMontoResultadoInicio;
                                acumuladoMontoResultadoFinal = acumuladoMontoResultadoIncrementoNeta + acumuladoMontoResultadoIncremento + acumuladoMontoResultadoInicio;
                            }

                            data.Add(new DetalleBancoDTO
                            {
                                concepto = "DIVIDENDOS PAGADOS",
                                mesMonto = mesMontoDividendos,
                                mesMontoResultado = 0,
                                mesPorcentaje = mesPorcentajeDividendos,
                                mesPorcentajeResultado = 0,
                                acumuladoMonto = acumuladoMontoDividendos,
                                acumuladoMontoResultado = 0,
                                acumuladoPorcentaje = acumuladoPorcentajeDividendos,
                                acumuladoPorcentajeResultado = 0,
                                renglonGrupo = false
                            });

                            data.Add(new DetalleBancoDTO
                            {
                                concepto = "GASTOS CORPORATIVOS",
                                mesMonto = mesMontoGastos,
                                mesMontoResultado = mesMontoDividendos + mesMontoGastos,
                                mesPorcentaje = mesPorcentajeGastos,
                                mesPorcentajeResultado = mesPorcentajeDividendos + mesPorcentajeGastos,
                                acumuladoMonto = acumuladoMontoGastos,
                                acumuladoMontoResultado = acumuladoMontoDividendos + acumuladoMontoGastos,
                                acumuladoPorcentaje = acumuladoPorcentajeGastos,
                                acumuladoPorcentajeResultado = acumuladoPorcentajeDividendos + acumuladoPorcentajeGastos,
                                renglonGrupo = false
                            });

                            data.Add(new DetalleBancoDTO
                            {
                                concepto = "INCREMENTO (DISMINUCIÓN) NETA DE EFECTIVO",
                                mesMonto = 0,
                                mesMontoResultado = mesMontoResultadoIncrementoNeta,
                                mesPorcentaje = 0,
                                mesPorcentajeResultado = mesPorcentajeResultadoIncrementoNeta,
                                acumuladoMonto = 0,
                                acumuladoMontoResultado = acumuladoMontoResultadoIncrementoNeta,
                                acumuladoPorcentaje = 0,
                                acumuladoPorcentajeResultado = acumuladoPorcentajeResultadoIncrementoNeta,
                                renglonGrupo = true
                            });

                            data.Add(new DetalleBancoDTO
                            {
                                concepto = "INCREMENTO (DISMINUCIÓN) POR TIPO CAMBIO",
                                mesMonto = 0,
                                mesMontoResultado = mesMontoResultadoIncremento,
                                mesPorcentaje = 0,
                                mesPorcentajeResultado = mesPorcentajeResultadoIncremento,
                                acumuladoMonto = 0,
                                acumuladoMontoResultado = acumuladoMontoResultadoIncremento,
                                acumuladoPorcentaje = 0,
                                acumuladoPorcentajeResultado = acumuladoPorcentajeResultadoIncremento,
                                renglonGrupo = true
                            });

                            data.Add(new DetalleBancoDTO
                            {
                                concepto = "EFECTIVO AL INICIO DEL PERIODO",
                                mesMonto = 0,
                                mesMontoResultado = mesMontoResultadoInicio,
                                mesPorcentaje = 0,
                                mesPorcentajeResultado = 0,
                                acumuladoMonto = 0,
                                acumuladoMontoResultado = acumuladoMontoResultadoInicio,
                                acumuladoPorcentaje = 0,
                                acumuladoPorcentajeResultado = 0,
                                renglonGrupo = true
                            });

                            data.Add(new DetalleBancoDTO
                            {
                                concepto = "EFECTIVO AL FINAL DEL PERIODO",
                                mesMonto = 0,
                                mesMontoResultado = mesMontoResultadoFinal,
                                mesPorcentaje = 0,
                                mesPorcentajeResultado = 0,
                                acumuladoMonto = 0,
                                acumuladoMontoResultado = acumuladoMontoResultadoFinal,
                                acumuladoPorcentaje = 0,
                                acumuladoPorcentajeResultado = 0,
                                renglonGrupo = true
                            });
                            #endregion
                        }
                        #endregion
                        break;
                    case TipoDetalleEnum.CUENTA:
                        #region CUENTA
                        var clientesEK = _contextEnkontrol.Select<dynamic>(getEnkontrolEnumADM(), new OdbcConsultaDTO()
                        {
                            consulta = string.Format(@"SELECT * FROM sx_clientes")
                        }).ToList();

                        if (listaEmpresas.Count() == 1)
                        {
                            #region Datos de una empresa
                            EmpresaEnum empresa = vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? listaEmpresas[0] : (EmpresaEnum)vSesiones.sesionEmpresaActual;
                            var listaMovimientosCliente = new List<tblEF_MovimientoCliente>();

                            using (var _contextEmpresa = new MainContext(empresa))
                            {
                                listaMovimientosCliente = _contextEmpresa.tblEF_MovimientoCliente.Where(x => x.estatus && x.corteMesID == corteMes.id).ToList();
                            }

                            if (empresa != EmpresaEnum.Arrendadora)
                            {
                                listaMovimientosCliente = listaMovimientosCliente.Where(x => (listaCC.Count > 0 ? listaCC.Contains(x.cc) : true)).ToList();
                            }
                            else
                            {
                                listaMovimientosCliente = listaMovimientosCliente.Where(x => listaAC.Count > 0 ? listaAC.Contains(x.areaCuenta) : true).ToList();
                                //listaMovimientosCliente = (
                                //    from mov in listaMovimientosCliente
                                //    join ac in listaAreaCuenta on new { area = (int)mov.area, cuenta = (int)mov.cuenta } equals new { area = ac.area, cuenta = ac.cuenta }
                                //    select mov
                                //).ToList();
                            }

                            var movimientosPorCliente = listaMovimientosCliente.GroupBy(x => x.numeroCliente).Select(x => new
                            {
                                cliente = x.Key,
                                clienteDesc = clientesEK.Where(y => Convert.ToInt32(y.numcte) == x.Key).Select(z => (string)z.nombre).FirstOrDefault(),
                                moneda = clientesEK.Where(y => Convert.ToInt32(y.numcte) == x.Key).Select(z => Convert.ToInt32(z.moneda)).FirstOrDefault(),
                                monto = (x.Sum(y => y.total)) / 1000
                            }).OrderByDescending(x => x.monto).ToList();

                            var pesos = movimientosPorCliente.Where(x => x.moneda == 1).OrderByDescending(x => x.monto).Take(10).Select(x => new
                            {
                                cliente = x.cliente,
                                clienteDesc = x.clienteDesc,
                                moneda = x.moneda,
                                montoPesos = x.monto,
                                montoDolares = 0
                            }).ToList();
                            var dolares = movimientosPorCliente.Where(x => x.moneda == 2).OrderByDescending(x => x.monto).Take(10).Select(x => new
                            {
                                cliente = x.cliente,
                                clienteDesc = x.clienteDesc,
                                moneda = x.moneda,
                                montoPesos = 0,
                                montoDolares = x.monto
                            }).ToList();

                            data.AddRange(pesos);
                            data.AddRange(dolares);
                            #endregion
                        }
                        else if (listaEmpresas.Count() > 1)
                        {
                            #region Datos de múltiples empresas
                            var datosEmpresasMixtas = new List<dynamic>();

                            foreach (var empresa in listaEmpresas)
                            {
                                var listaMovimientosCliente = new List<tblEF_MovimientoCliente>();

                                using (var _contextEmpresa = new MainContext(listaEmpresas[0]))
                                {
                                    listaMovimientosCliente = _contextEmpresa.tblEF_MovimientoCliente.Where(x => x.estatus && x.corteMesID == corteMes.id).ToList();
                                }

                                if (empresa != EmpresaEnum.Arrendadora)
                                {
                                    listaMovimientosCliente = listaMovimientosCliente.Where(x => (listaCC.Count > 0 ? listaCC.Contains(x.cc) : true)).ToList();
                                }
                                else
                                {
                                    listaMovimientosCliente = listaMovimientosCliente.Where(x => listaAC.Count > 0 ? listaAC.Contains(x.areaCuenta) : true).ToList();
                                    //listaMovimientosCliente = (
                                    //    from mov in listaMovimientosCliente
                                    //    join ac in listaAreaCuenta on new { area = (int)mov.area, cuenta = (int)mov.cuenta } equals new { area = ac.area, cuenta = ac.cuenta }
                                    //    select mov
                                    //).ToList();
                                }

                                var movimientosPorCliente = listaMovimientosCliente.GroupBy(x => x.numeroCliente).Select(x => new
                                {
                                    cliente = x.Key,
                                    clienteDesc = clientesEK.Where(y => Convert.ToInt32(y.numcte) == x.Key).Select(z => (string)z.nombre).FirstOrDefault(),
                                    moneda = clientesEK.Where(y => Convert.ToInt32(y.numcte) == x.Key).Select(z => Convert.ToInt32(z.moneda)).FirstOrDefault(),
                                    monto = x.Sum(y => y.total)
                                }).OrderByDescending(x => x.monto).ToList();

                                var pesos = movimientosPorCliente.Where(x => x.moneda == 1).OrderByDescending(x => x.monto).Take(10).Select(x => new
                                {
                                    cliente = x.cliente,
                                    clienteDesc = x.clienteDesc,
                                    moneda = x.moneda,
                                    montoPesos = x.monto,
                                    montoDolares = 0
                                }).ToList();
                                var dolares = movimientosPorCliente.Where(x => x.moneda == 2).OrderByDescending(x => x.monto).Take(10).Select(x => new
                                {
                                    cliente = x.cliente,
                                    clienteDesc = x.clienteDesc,
                                    moneda = x.moneda,
                                    montoPesos = 0,
                                    montoDolares = x.monto
                                }).ToList();

                                datosEmpresasMixtas.AddRange(pesos);
                                datosEmpresasMixtas.AddRange(dolares);
                            }

                            data.AddRange(datosEmpresasMixtas.Where(x => x.moneda == 1).OrderByDescending(x => x.montoPesos).Take(10).ToList());
                            data.AddRange(datosEmpresasMixtas.Where(x => x.moneda == 2).OrderByDescending(x => x.montoDolares).Take(10).ToList());
                            #endregion
                        }
                        #endregion
                        break;
                    case TipoDetalleEnum.OBRA:
                        #region OBRA
                        var listaCentroCosto = new List<tblP_CC>();

                        using (var _contextConstruplan = new MainContext(EmpresaEnum.Construplan))
                        {
                            listaCentroCosto = _contextConstruplan.tblP_CC.ToList();
                        }

                        if (listaEmpresas.Count() == 1)
                        {
                            #region Datos de una empresa
                            EmpresaEnum empresa = vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? listaEmpresas[0] : (EmpresaEnum)vSesiones.sesionEmpresaActual;

                            var saldos = new List<tblEF_SaldoCC>();

                            using (var _contextEmpresa = new MainContext(empresa))
                            {
                                saldos = _contextEmpresa.tblEF_SaldoCC.Where(x => x.estatus && x.corteMesID == corteMes.id && ((x.cta == 2125 && x.scta == 10) || (x.cta == 1127 && x.scta == 1))).ToList();

                                #region Filtro de cc/área-cuenta.
                                if (empresa != EmpresaEnum.Arrendadora)
                                {
                                    if (listaCC.Count > 0)
                                    {
                                        saldos = saldos.Where(x => (listaCC != null ? listaCC.Contains(x.cc) : true)).ToList();
                                    }
                                }
                                else
                                {
                                    if (listaAreaCuenta.Count > 0)
                                    {
                                        saldos = saldos.Where(x => x.area.HasValue).ToList();

                                        saldos = saldos.Where(x => listaAC.Count > 0 ? listaAC.Contains(x.areaCuenta) : true).ToList();

                                    //    saldos = (
                                    //    from sal in saldos
                                    //    join ac in listaAreaCuenta on new { area = sal.area.Value, cuenta = sal.cuenta.Value } equals new { area = ac.area, cuenta = ac.cuenta }
                                    //    select sal
                                    //).ToList();
                                    }
                                }
                                #endregion
                            }

                            var obras100 = _context.tblEF_Obra100Porcentaje.Where(x => x.registroActivo).ToList();
                            var obrasSaldos = saldos.GroupBy(x => x.cc).Select(x => new
                            {
                                cc = x.Key,
                                ccDesc = listaCentroCosto.Where(y => y.cc == x.Key).Select(z => z.descripcion).FirstOrDefault(),
                                estimacion = x.Where(y => y.cta == 2125 && y.scta == 10).Sum(z => z.saldoInicial + z.cargosAcumulados + z.abonosAcumulados),
                                grp = x//.Where(y => y.cta == 1127 && y.scta == 1).ToList()
                            }).ToList();
                            var revision70Total = 0m;
                            var revision30Total = 0m;

                            foreach (var obra in obrasSaldos)
                            {
                                var revision70 = obra.grp.Where(x => x.cta == 1127 && x.scta == 1).Sum(x => x.saldoInicial + x.cargosAcumulados + x.abonosAcumulados);

                                var revision30 = 0m;

                                if (obras100.Select(x => x.cc).Contains(obra.cc))
                                {
                                    data.Add(new
                                    {
                                        obra = obra.cc,
                                        obraDesc = obra.ccDesc,
                                        estimacion = obra.estimacion,
                                        revision70 = revision70,
                                        revision30 = revision30
                                    });
                                }
                                else
                                {
                                    var revision100 = Math.Round(((revision70 * 100) / 70), 2);

                                    revision30 = Math.Round(((revision100 * 30) / 100), 2);

                                    data.Add(new
                                    {
                                        obra = obra.cc,
                                        obraDesc = obra.ccDesc,
                                        estimacion = obra.estimacion,
                                        revision70 = revision70,
                                        revision30 = revision30
                                    });
                                }

                                revision70Total += revision70;
                                revision30Total += revision30;
                            }

                            data = data.Where(x => (x.estimacion > 0 || x.estimacion < 0) || x.revision70 > 0 || x.revision30 > 0).OrderBy(x => x.obraDesc).ToList();

                            var estimacionTotal = obrasSaldos.Sum(x => x.estimacion);

                            data.Add(new
                            {
                                obra = "",
                                obraDesc = "TOTALES",
                                estimacion = estimacionTotal,
                                revision70 = revision70Total,
                                revision30 = revision30Total,
                                renglonGrupo = true
                            });

                            data.Add(new
                            {
                                obra = "",
                                obraDesc = "TOTAL OBRA",
                                estimacion = estimacionTotal + revision70Total,
                                revision70 = 0,
                                revision30 = 0,
                                renglonGrupo = true
                            });
                            #endregion
                        }
                        else if (listaEmpresas.Count() > 1)
                        {
                            #region Datos de múltiples empresas
                            foreach (var empresa in listaEmpresas)
                            {
                                var saldos = new List<tblEF_SaldoCC>();

                                using (var _contextEmpresa = new MainContext(empresa))
                                {
                                    saldos = _contextEmpresa.tblEF_SaldoCC.Where(x => x.estatus && x.corteMesID == corteMes.id && ((x.cta == 2125 && x.scta == 10) || (x.cta == 1127 && x.scta == 1))).ToList();

                                    #region Filtro de cc/área-cuenta.
                                    if (empresa != EmpresaEnum.Arrendadora)
                                    {
                                        if (listaCC.Count > 0)
                                        {
                                            saldos = saldos.Where(x => (listaCC != null ? listaCC.Contains(x.cc) : true)).ToList();
                                        }
                                    }
                                    else
                                    {
                                        if (listaAreaCuenta.Count > 0)
                                        {
                                            saldos = saldos.Where(x => x.area.HasValue).ToList();

                                            saldos = saldos.Where(x => listaAC.Count > 0 ? listaAC.Contains(x.areaCuenta) : true).ToList();

                                        //    saldos = (
                                        //        from sal in saldos
                                        //        join ac in listaAreaCuenta on new { area = sal.area.Value, cuenta = sal.cuenta.Value } equals new { area = ac.area, cuenta = ac.cuenta }
                                        //    select sal
                                        //).ToList();
                                        }
                                    }
                                    #endregion
                                }

                                var obras100 = _context.tblEF_Obra100Porcentaje.Where(x => x.registroActivo).ToList();
                                var obrasSaldos = saldos.GroupBy(x => x.cc).Select(x => new
                                {
                                    cc = x.Key,
                                    ccDesc = listaCentroCosto.Where(y => y.cc == x.Key).Select(z => z.descripcion).FirstOrDefault(),
                                    estimacion = x.Where(y => y.cta == 2125 && y.scta == 10).Sum(z => z.saldoInicial + z.cargosAcumulados + z.abonosAcumulados),
                                    grp = x.Where(y => y.cta == 1127 && y.scta == 1).ToList()
                                });

                                foreach (var obra in obrasSaldos)
                                {
                                    var revision70 = obra.grp.Where(x => x.cta == 1127 && x.scta == 1).Sum(x => x.saldoInicial + x.cargosAcumulados + x.abonosAcumulados);
                                    var revision30 = 0m;

                                    if (obras100.Select(x => x.cc).Contains(obra.cc))
                                    {
                                        data.Add(new
                                        {
                                            obra = obra.cc,
                                            obraDesc = obra.ccDesc,
                                            estimacion = (decimal)obra.estimacion,
                                            revision70 = (decimal)revision70,
                                            revision30 = (decimal)revision30
                                        });
                                    }
                                    else
                                    {
                                        var revision100 = Math.Round(((revision70 * 100) / 70), 2);

                                        revision30 = Math.Round(((revision100 * 30) / 100), 2);

                                        data.Add(new
                                        {
                                            obra = obra.cc,
                                            obraDesc = obra.ccDesc,
                                            estimacion = (decimal)obra.estimacion,
                                            revision70 = (decimal)revision70,
                                            revision30 = (decimal)revision30
                                        });
                                    }
                                }
                            }

                            data = data.Where(x => (x.estimacion > 0 || x.estimacion < 0) || x.revision70 > 0 || x.revision30 > 0).OrderBy(x => x.obraDesc).ToList();

                            var estimacionTotal = data.Sum(x => ((decimal)x.estimacion));
                            var revision70Total = data.Sum(x => ((decimal)x.revision70));
                            var revision30Total = data.Sum(x => ((decimal)x.revision30));

                            data.Add(new
                            {
                                obra = "",
                                obraDesc = "TOTALES",
                                estimacion = (decimal)estimacionTotal,
                                revision70 = (decimal)revision70Total,
                                revision30 = (decimal)revision30Total,
                                renglonGrupo = true
                            });

                            data.Add(new
                            {
                                obra = "",
                                obraDesc = "TOTAL OBRA",
                                estimacion = (decimal)estimacionTotal + revision70Total,
                                revision70 = 0M,
                                revision30 = 0M,
                                renglonGrupo = true
                            });
                            #endregion
                        }
                        #endregion
                        break;
                    case TipoDetalleEnum.PARTE_RELACIONADA:
                        #region PARTE RELACIONADA
                        var partesRelacionadas = new List<tblEF_ParteRelacionada>();
                        var partesRelacionadasDetalle = new List<tblEF_ParteRelacionadaDetalle>();

                        //Se toma la información nomás de Construplan (esto puede cambiar)
                        using (var _contextConstruplan = new MainContext(EmpresaEnum.Construplan))
                        {
                            partesRelacionadas = _contextConstruplan.tblEF_ParteRelacionada.Where(x => x.registroActivo).ToList();
                            partesRelacionadasDetalle = _contextConstruplan.tblEF_ParteRelacionadaDetalle.Where(x => x.registroActivo).ToList().Where(x => partesRelacionadas.Select(y => y.id).Contains(x.parteRelacionadaID)).ToList();
                        }

                        partesRelacionadas = partesRelacionadas.Where(x => x.tipo == tipoTablaGeneral).ToList();
                        partesRelacionadasDetalle = partesRelacionadasDetalle.Where(x => partesRelacionadas.Select(y => y.id).Contains(x.parteRelacionadaID)).ToList();

                        if (listaEmpresas.Count() == 1)
                        {
                            #region Datos de una empresa
                            EmpresaEnum empresa = vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? listaEmpresas[0] : (EmpresaEnum)vSesiones.sesionEmpresaActual;

                            foreach (var grupo in partesRelacionadas.GroupBy(x => x.grupo).Select(x => x.Key).ToList())
                            {
                                var partesRelacionadasGrupo = partesRelacionadas.Where(x => x.grupo == grupo).ToList();
                                var montoTotalGrupo = 0m;

                                foreach (var parteRelacionada in partesRelacionadasGrupo)
                                {
                                    var partesRelacionadasDetalleGrupo = partesRelacionadasDetalle.Where(x => x.parteRelacionadaID == parteRelacionada.id).ToList();
                                    var montoTotalParte = 0m;

                                    foreach (var detalle in partesRelacionadasDetalleGrupo)
                                    {
                                        switch (detalle.tipo)
                                        {
                                            case TipoCuentaEnum.CUENTA:
                                                var saldos = new List<tblEF_SaldoCC>();

                                                using (var _contextEmpresa = new MainContext(empresa))
                                                {
                                                    saldos = _contextEmpresa.tblEF_SaldoCC.Where(x => x.estatus && x.corteMesID == corteMes.id && x.cta == detalle.cta && x.scta == detalle.scta && x.sscta == detalle.sscta).ToList();

                                                    #region Filtro de cc/área-cuenta.
                                                    if (empresa != EmpresaEnum.Arrendadora)
                                                    {
                                                        saldos = saldos.Where(x => (listaCC.Count > 0 ? listaCC.Contains(x.cc) : true)).ToList();
                                                    }
                                                    else
                                                    {
                                                        saldos = saldos.Where(x => listaAC.Count > 0 ? listaAC.Contains(x.areaCuenta) : true).ToList();
                                                        //saldos = (
                                                        //    from sal in saldos
                                                        //    join ac in listaAreaCuenta on new { area = (int)sal.area, cuenta = (int)sal.cuenta } equals new { area = ac.area, cuenta = ac.cuenta }
                                                        //    select sal
                                                        //).ToList();
                                                    }
                                                    #endregion
                                                }

                                                montoTotalParte += saldos.Sum(x => x.saldoInicial + x.cargosAcumulados + x.abonosAcumulados);
                                                break;
                                            case TipoCuentaEnum.CLIENTE:
                                                var movimientosCliente = new List<tblEF_MovimientoCliente>();

                                                using (var _contextEmpresa = new MainContext(empresa))
                                                {
                                                    movimientosCliente = _contextEmpresa.tblEF_MovimientoCliente.Where(x => x.estatus && x.corteMesID == corteMes.id && x.numeroCliente == detalle.cta).ToList();

                                                    #region Filtro de cc/área-cuenta.
                                                    if (empresa != EmpresaEnum.Arrendadora)
                                                    {
                                                        movimientosCliente = movimientosCliente.Where(x => (listaCC.Count > 0 ? listaCC.Contains(x.cc) : true)).ToList();
                                                    }
                                                    else
                                                    {
                                                        movimientosCliente = movimientosCliente.Where(x => listaAC.Count > 0 ? listaAC.Contains(x.areaCuenta) : true).ToList();
                                                        //movimientosCliente = (
                                                        //    from mov in movimientosCliente
                                                        //    join ac in listaAreaCuenta on new { area = (int)mov.area, cuenta = (int)mov.cuenta } equals new { area = ac.area, cuenta = ac.cuenta }
                                                        //    select mov
                                                        //).ToList();
                                                    }
                                                    #endregion
                                                }

                                                montoTotalParte += movimientosCliente.Sum(x => x.total);
                                                break;
                                            case TipoCuentaEnum.PROVEEDOR:
                                                var movimientosProveedor = new List<tblEF_MovimientoProveedor>();

                                                using (var _contextEmpresa = new MainContext(empresa))
                                                {
                                                    movimientosProveedor = _contextEmpresa.tblEF_MovimientoProveedor.Where(x => x.estatus && x.corteMesID == corteMes.id && x.numeroProveedor == detalle.cta).ToList();

                                                    #region Filtro de cc/área-cuenta.
                                                    if (empresa != EmpresaEnum.Arrendadora)
                                                    {
                                                        movimientosProveedor = movimientosProveedor.Where(x => (listaCC.Count > 0 ? listaCC.Contains(x.cc) : true)).ToList();
                                                    }
                                                    else
                                                    {
                                                        movimientosProveedor = movimientosProveedor.Where(x => listaAC.Count > 0 ? listaAC.Contains(x.areaCuenta) : true).ToList();
                                                        //movimientosProveedor = (
                                                        //    from mov in movimientosProveedor
                                                        //    join ac in listaAreaCuenta on new { area = (int)mov.area, cuenta = (int)mov.cuenta } equals new { area = ac.area, cuenta = ac.cuenta }
                                                        //    select mov
                                                        //).ToList();
                                                    }
                                                    #endregion
                                                }

                                                montoTotalParte += movimientosProveedor.Sum(x => x.total);
                                                break;
                                        }
                                    }

                                    data.Add(new
                                    {
                                        parteRelacionada = parteRelacionada.descripcion,
                                        saldo = montoTotalParte,
                                        grupo = parteRelacionada.grupo,
                                        renglonGrupo = false
                                    });

                                    montoTotalGrupo += montoTotalParte;
                                }

                                if (grupo == 1)
                                {
                                    data.Add(new
                                    {
                                        parteRelacionada = "CONSOLIDADO",
                                        saldo = montoTotalGrupo,
                                        grupo = 1,
                                        renglonGrupo = true
                                    });
                                }
                                else if (grupo == 2)
                                {
                                    var saldoTotal = 0m;

                                    foreach (var d in data)
                                    {
                                        if (!d.renglonGrupo)
                                        {
                                            saldoTotal += Convert.ToDecimal(d.saldo);
                                        }
                                    }

                                    data.Add(new
                                    {
                                        parteRelacionada = "",
                                        saldo = saldoTotal,
                                        grupo = 2,
                                        renglonGrupo = true
                                    });
                                }
                            }
                            #endregion
                        }
                        else if (listaEmpresas.Count() > 1)
                        {
                            #region Datos de múltiples empresas
                            foreach (var grupo in partesRelacionadas.GroupBy(x => x.grupo).Select(x => x.Key).ToList())
                            {
                                var partesRelacionadasGrupo = partesRelacionadas.Where(x => x.grupo == grupo).ToList();
                                var montoTotalGrupo = 0m;

                                foreach (var parteRelacionada in partesRelacionadasGrupo)
                                {
                                    var partesRelacionadasDetalleGrupo = partesRelacionadasDetalle.Where(x => x.parteRelacionadaID == parteRelacionada.id).ToList();
                                    var montoTotalParte = 0m;

                                    foreach (var detalle in partesRelacionadasDetalleGrupo)
                                    {
                                        foreach (var empresa in listaEmpresas)
                                        {
                                            switch (detalle.tipo)
                                            {
                                                case TipoCuentaEnum.CUENTA:
                                                    var saldos = new List<tblEF_SaldoCC>();

                                                    using (var _contextEmpresa = new MainContext(empresa))
                                                    {
                                                        saldos = _contextEmpresa.tblEF_SaldoCC.Where(x => x.estatus && x.corteMesID == corteMes.id && x.cta == detalle.cta && x.scta == detalle.scta && x.sscta == detalle.sscta).ToList();

                                                        #region Filtro de cc/área-cuenta.
                                                        if (empresa != EmpresaEnum.Arrendadora)
                                                        {
                                                            saldos = saldos.Where(x => (listaCC.Count > 0 ? listaCC.Contains(x.cc) : true)).ToList();
                                                        }
                                                        else
                                                        {
                                                            saldos = saldos.Where(x => listaAC.Count > 0 ? listaAC.Contains(x.areaCuenta) : true).ToList();

                                                            //saldos = (
                                                            //    from sal in saldos
                                                            //    join ac in listaAreaCuenta on new { area = (int)sal.area, cuenta = (int)sal.cuenta } equals new { area = ac.area, cuenta = ac.cuenta }
                                                            //    select sal
                                                            //).ToList();
                                                        }
                                                        #endregion
                                                    }

                                                    montoTotalParte += saldos.Sum(x => x.saldoInicial + x.cargosAcumulados + x.abonosAcumulados);
                                                    break;
                                                case TipoCuentaEnum.CLIENTE:
                                                    var movimientosCliente = new List<tblEF_MovimientoCliente>();

                                                    using (var _contextEmpresa = new MainContext(empresa))
                                                    {
                                                        movimientosCliente = _contextEmpresa.tblEF_MovimientoCliente.Where(x => x.estatus && x.corteMesID == corteMes.id && x.numeroCliente == detalle.cta).ToList();

                                                        #region Filtro de cc/área-cuenta.
                                                        if (empresa != EmpresaEnum.Arrendadora)
                                                        {
                                                            movimientosCliente = movimientosCliente.Where(x => (listaCC.Count > 0 ? listaCC.Contains(x.cc) : true)).ToList();
                                                        }
                                                        else
                                                        {
                                                            movimientosCliente = movimientosCliente.Where(x => listaAC.Count > 0 ? listaAC.Contains(x.areaCuenta) : true).ToList();
                                                            //movimientosCliente = (
                                                            //    from mov in movimientosCliente
                                                            //    join ac in listaAreaCuenta on new { area = (int)mov.area, cuenta = (int)mov.cuenta } equals new { area = ac.area, cuenta = ac.cuenta }
                                                            //    select mov
                                                            //).ToList();
                                                        }
                                                        #endregion
                                                    }

                                                    montoTotalParte += movimientosCliente.Sum(x => x.total);
                                                    break;
                                                case TipoCuentaEnum.PROVEEDOR:
                                                    var movimientosProveedor = new List<tblEF_MovimientoProveedor>();

                                                    using (var _contextEmpresa = new MainContext(empresa))
                                                    {
                                                        movimientosProveedor = _contextEmpresa.tblEF_MovimientoProveedor.Where(x => x.estatus && x.corteMesID == corteMes.id && x.numeroProveedor == detalle.cta).ToList();

                                                        #region Filtro de cc/área-cuenta.
                                                        if (empresa != EmpresaEnum.Arrendadora)
                                                        {
                                                            movimientosProveedor = movimientosProveedor.Where(x => (listaCC.Count > 0 ? listaCC.Contains(x.cc) : true)).ToList();
                                                        }
                                                        else
                                                        {
                                                            movimientosProveedor = movimientosProveedor.Where(x => listaAC.Count > 0 ? listaAC.Contains(x.areaCuenta) : true).ToList();
                                                            //movimientosProveedor = (
                                                            //    from mov in movimientosProveedor
                                                            //    join ac in listaAreaCuenta on new { area = (int)mov.area, cuenta = (int)mov.cuenta } equals new { area = ac.area, cuenta = ac.cuenta }
                                                            //    select mov
                                                            //).ToList();
                                                        }
                                                        #endregion
                                                    }

                                                    montoTotalParte += movimientosProveedor.Sum(x => x.total);
                                                    break;
                                            }
                                        }
                                    }

                                    data.Add(new
                                    {
                                        parteRelacionada = parteRelacionada.descripcion,
                                        saldo = montoTotalParte,
                                        grupo = parteRelacionada.grupo,
                                        renglonGrupo = false
                                    });

                                    montoTotalGrupo += montoTotalParte;
                                }

                                if (grupo == 1)
                                {
                                    data.Add(new
                                    {
                                        parteRelacionada = "CONSOLIDADO",
                                        saldo = montoTotalGrupo,
                                        grupo = 1,
                                        renglonGrupo = true
                                    });
                                }
                                else if (grupo == 2)
                                {
                                    var saldoTotal = 0m;

                                    foreach (var d in data)
                                    {
                                        if (!d.renglonGrupo)
                                        {
                                            saldoTotal += Convert.ToDecimal(d.saldo);
                                        }
                                    }

                                    data.Add(new
                                    {
                                        parteRelacionada = "",
                                        saldo = saldoTotal,
                                        grupo = 2,
                                        renglonGrupo = true
                                    });
                                }
                            }
                            #endregion
                        }
                        #endregion
                        break;
                    case TipoDetalleEnum.ALMACEN:
                        #region ALMACEN
                        if (listaEmpresas.Count() == 1)
                        {
                            #region SE OBTIENE LA SUMA DE: SALDO INICIAL + CARGOS ACUMULADOS + ABONO ACUMULADOS. (ALMACEN DE MATERIALES (CTA: 1155))
                            using (var _contextEmpresa = new MainContext(listaEmpresas[0]))
                            {
                                decimal almacenMateriales = 0, reservaInsumosObsoletos = 0, saldoNetoInventarios = 0;
                                List<tblEF_SaldoCC> lstAlmacenMateriales = new List<tblEF_SaldoCC>();
                                List<tblEF_SaldoCC> lstReservaInsumosObsoletos = new List<tblEF_SaldoCC>();

                                if (listaCC.Count > 0)
                                {
                                    lstAlmacenMateriales = _contextEmpresa.tblEF_SaldoCC.Where(w => w.cta == 1155 && w.corteMesID == corteMes.id && w.estatus && listaCC.Contains(w.cc)).ToList();
                                    lstReservaInsumosObsoletos = _contextEmpresa.tblEF_SaldoCC.Where(w => w.cta == 1156 && w.corteMesID == corteMes.id && w.estatus && listaCC.Contains(w.cc)).ToList();
                                }
                                else
                                {
                                    lstAlmacenMateriales = _contextEmpresa.tblEF_SaldoCC.Where(w => w.cta == 1155 && w.corteMesID == corteMes.id && w.estatus).ToList();
                                    lstReservaInsumosObsoletos = _contextEmpresa.tblEF_SaldoCC.Where(w => w.cta == 1156 && w.corteMesID == corteMes.id && w.estatus).ToList();
                                }

                                foreach (var item in lstAlmacenMateriales)
                                {
                                    almacenMateriales += item.saldoInicial;
                                    almacenMateriales += item.cargosAcumulados;
                                    almacenMateriales += item.abonosAcumulados;
                                }

                                foreach (var item in lstReservaInsumosObsoletos)
                                {
                                    reservaInsumosObsoletos += item.saldoInicial;
                                    reservaInsumosObsoletos += item.cargosAcumulados;
                                    reservaInsumosObsoletos += item.abonosAcumulados;
                                }

                                if ((decimal)almacenMateriales < 0)
                                    almacenMateriales *= -1;

                                if ((decimal)reservaInsumosObsoletos < 0)
                                    reservaInsumosObsoletos *= -1;

                                if (almacenMateriales <= 0)
                                    saldoNetoInventarios = (decimal)reservaInsumosObsoletos * (-1);
                                else
                                    saldoNetoInventarios = (decimal)almacenMateriales - (decimal)reservaInsumosObsoletos;

                                #region DATA
                                data.Add(new
                                {
                                    concepto = "ALMACEN DE MATERIALES",
                                    saldo = Math.Truncate(Math.Round(((decimal)almacenMateriales) / 1000)),
                                    grupo = 2,
                                    renglonGrupo = false
                                });

                                data.Add(new
                                {
                                    concepto = "MENOS",
                                    saldo = "MENOS",
                                    grupo = 1,
                                    renglonGrupo = false
                                });

                                data.Add(new
                                {
                                    concepto = "RESERVA INSUMOS OBSOLETOS",
                                    saldo = Math.Truncate(Math.Round(((decimal)reservaInsumosObsoletos) / 1000)),
                                    grupo = 2,
                                    renglonGrupo = false
                                });

                                data.Add(new
                                {
                                    concepto = "SALDO NETO INVENTARIOS",
                                    saldo = Math.Truncate(Math.Round(((decimal)saldoNetoInventarios) / 1000)),
                                    grupo = 2,
                                    renglonGrupo = true
                                });
                                #endregion
                            }
                            #endregion
                        }
                        else if (listaEmpresas.Count() > 1)
                        {
                            #region MULTIEMPRESAS
                            decimal almacenMateriales = 0, reservaInsumosObsoletos = 0, saldoNetoInventarios = 0;
                            List<tblEF_SaldoCC> lstAlmacenMateriales = new List<tblEF_SaldoCC>();
                            List<tblEF_SaldoCC> lstReservaInsumosObsoletos = new List<tblEF_SaldoCC>();
                            foreach (var objEmpresa in listaEmpresas)
                            {
                                decimal almacenMaterialesFE = 0;
                                decimal reservaInsumosObsoletosFE = 0;
                                decimal saldoNetoInventariosFE = 0;
                                lstAlmacenMateriales = new List<tblEF_SaldoCC>();
                                lstReservaInsumosObsoletos = new List<tblEF_SaldoCC>();
                                using (var _contextEmpresa = new MainContext(objEmpresa))
                                {
                                    if (listaCC.Count > 0)
                                    {
                                        lstAlmacenMateriales = _contextEmpresa.tblEF_SaldoCC.Where(w => w.cta == 1155 && w.corteMesID == corteMes.id && w.estatus && listaCC.Contains(w.cc)).ToList();
                                        lstReservaInsumosObsoletos = _contextEmpresa.tblEF_SaldoCC.Where(w => w.cta == 1156 && w.corteMesID == corteMes.id && w.estatus && listaCC.Contains(w.cc)).ToList();
                                    }
                                    else
                                    {
                                        lstAlmacenMateriales = _contextEmpresa.tblEF_SaldoCC.Where(w => w.cta == 1155 && w.corteMesID == corteMes.id && w.estatus).ToList();
                                        lstReservaInsumosObsoletos = _contextEmpresa.tblEF_SaldoCC.Where(w => w.cta == 1156 && w.corteMesID == corteMes.id && w.estatus).ToList();
                                    }

                                    foreach (var item in lstAlmacenMateriales)
                                    {
                                        almacenMaterialesFE += item.saldoInicial;
                                        almacenMaterialesFE += item.cargosAcumulados;
                                        almacenMaterialesFE += item.abonosAcumulados;
                                    }

                                    foreach (var item in lstReservaInsumosObsoletos)
                                    {
                                        reservaInsumosObsoletosFE += item.saldoInicial;
                                        reservaInsumosObsoletosFE += item.cargosAcumulados;
                                        reservaInsumosObsoletosFE += item.abonosAcumulados;
                                    }

                                    if ((decimal)almacenMaterialesFE < 0)
                                        almacenMaterialesFE *= -1;

                                    if ((decimal)reservaInsumosObsoletosFE < 0)
                                        reservaInsumosObsoletosFE *= -1;

                                    saldoNetoInventariosFE = (decimal)almacenMaterialesFE - (decimal)reservaInsumosObsoletosFE;

                                    almacenMateriales += (decimal)almacenMaterialesFE;
                                    reservaInsumosObsoletos += (decimal)reservaInsumosObsoletosFE;
                                    saldoNetoInventarios += (decimal)saldoNetoInventariosFE;
                                }
                            }

                            if (almacenMateriales <= 0)
                                saldoNetoInventarios = (decimal)reservaInsumosObsoletos * (-1);
                            else
                                saldoNetoInventarios = (decimal)almacenMateriales - (decimal)reservaInsumosObsoletos;
                            #endregion

                            #region DATA
                            data.Add(new
                            {
                                concepto = "ALMACEN DE MATERIALES",
                                saldo = Math.Truncate(Math.Round(((decimal)almacenMateriales) / 1000)),
                                grupo = 2,
                                renglonGrupo = false
                            });

                            data.Add(new
                            {
                                concepto = "MENOS",
                                saldo = "MENOS",
                                grupo = 1,
                                renglonGrupo = false
                            });

                            data.Add(new
                            {
                                concepto = "RESERVA INSUMOS OBSOLETOS",
                                saldo = Math.Truncate(Math.Round(((decimal)reservaInsumosObsoletos) / 1000)),
                                grupo = 2,
                                renglonGrupo = false
                            });

                            data.Add(new
                            {
                                concepto = "SALDO NETO INVENTARIOS",
                                saldo = Math.Truncate(Math.Round(((decimal)saldoNetoInventarios) / 1000)),
                                grupo = 2,
                                renglonGrupo = true
                            });
                            #endregion
                        }
                        #endregion
                        break;
                    case TipoDetalleEnum.INVERSION:
                        #region INVERSION
                        var inversiones = new List<tblEF_Inversion>();
                        var inversionesDetalle = new List<tblEF_InversionDetalle>();

                        //Se toma la información nomás de Construplan (esto puede cambiar)
                        using (var _contextConstruplan = new MainContext(EmpresaEnum.Construplan))
                        {
                            inversiones = _contextConstruplan.tblEF_Inversion.Where(x => x.registroActivo).ToList();
                            inversionesDetalle = _contextConstruplan.tblEF_InversionDetalle.Where(x => x.registroActivo).ToList().Where(x => inversiones.Select(y => y.id).Contains(x.inversionID)).ToList();
                        }

                        if (listaEmpresas.Count() == 1)
                        {
                            #region Datos de una empresa
                            EmpresaEnum empresa = vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? listaEmpresas[0] : (EmpresaEnum)vSesiones.sesionEmpresaActual;

                            foreach (var grupo in inversiones.GroupBy(x => x.grupo).Select(x => x.Key).ToList())
                            {
                                var inversionesGrupo = inversiones.Where(x => x.grupo == grupo).ToList();
                                var montoTotalGrupo = 0m;

                                foreach (var inversion in inversionesGrupo)
                                {
                                    var inversionesDetalleGrupo = inversionesDetalle.Where(x => x.inversionID == inversion.id).ToList();
                                    var montoTotalInversion = 0m;

                                    foreach (var detalle in inversionesDetalleGrupo)
                                    {
                                        var saldos = new List<tblEF_SaldoCC>();

                                        using (var _contextEmpresa = new MainContext(empresa))
                                        {
                                            saldos = _contextEmpresa.tblEF_SaldoCC.Where(x => x.estatus && x.corteMesID == corteMes.id && x.cta == detalle.cta && x.scta == detalle.scta && x.sscta == detalle.sscta).ToList();

                                            #region Filtro de cc/área-cuenta.
                                            if (empresa != EmpresaEnum.Arrendadora)
                                            {
                                                saldos = saldos.Where(x => (listaCC.Count > 0 ? listaCC.Contains(x.cc) : true)).ToList();
                                            }
                                            else
                                            {
                                                saldos = saldos.Where(x => listaAC.Count > 0 ? listaAC.Contains(x.areaCuenta) : true).ToList();
                                                //saldos = (
                                                //    from sal in saldos
                                                //    join ac in listaAreaCuenta on new { area = (int)sal.area, cuenta = (int)sal.cuenta } equals new { area = ac.area, cuenta = ac.cuenta }
                                                //    select sal
                                                //).ToList();
                                            }
                                            #endregion
                                        }

                                        montoTotalInversion += saldos.Sum(x => x.saldoInicial + x.cargosAcumulados + x.abonosAcumulados);
                                    }

                                    data.Add(new
                                    {
                                        inversion = inversion.descripcion,
                                        saldo = montoTotalInversion,
                                        grupo = inversion.grupo,
                                        renglonGrupo = false
                                    });

                                    montoTotalGrupo += montoTotalInversion;
                                }

                                if (grupo == 1)
                                {
                                    data.Add(new
                                    {
                                        inversion = "CONSOLIDADO",
                                        saldo = montoTotalGrupo,
                                        grupo = 1,
                                        renglonGrupo = true
                                    });
                                }
                                else if (grupo == 2)
                                {
                                    var saldoTotal = 0m;

                                    foreach (var d in data)
                                    {
                                        if (!d.renglonGrupo)
                                        {
                                            saldoTotal += Convert.ToDecimal(d.saldo);
                                        }
                                    }

                                    data.Add(new
                                    {
                                        inversion = "",
                                        saldo = saldoTotal,
                                        grupo = 2,
                                        renglonGrupo = true
                                    });
                                }
                            }
                            #endregion
                        }
                        else if (listaEmpresas.Count() > 1)
                        {
                            #region Datos de múltiples empresas
                            foreach (var grupo in inversiones.GroupBy(x => x.grupo).Select(x => x.Key).ToList())
                            {
                                var inversionesGrupo = inversiones.Where(x => x.grupo == grupo).ToList();
                                var montoTotalGrupo = 0m;

                                foreach (var inversion in inversionesGrupo)
                                {
                                    var inversionesDetalleGrupo = inversionesDetalle.Where(x => x.inversionID == inversion.id).ToList();
                                    var montoTotalInversion = 0m;

                                    foreach (var detalle in inversionesDetalleGrupo)
                                    {
                                        foreach (var empresa in listaEmpresas)
                                        {
                                            var saldos = new List<tblEF_SaldoCC>();

                                            using (var _contextEmpresa = new MainContext(empresa))
                                            {
                                                saldos = _contextEmpresa.tblEF_SaldoCC.Where(x => x.estatus && x.corteMesID == corteMes.id && x.cta == detalle.cta && x.scta == detalle.scta && x.sscta == detalle.sscta).ToList();

                                                #region Filtro de cc/área-cuenta.
                                                if (empresa != EmpresaEnum.Arrendadora)
                                                {
                                                    saldos = saldos.Where(x => (listaCC.Count > 0 ? listaCC.Contains(x.cc) : true)).ToList();
                                                }
                                                else
                                                {
                                                    saldos = saldos.Where(x => listaAC.Count > 0 ? listaAC.Contains(x.areaCuenta) : true).ToList();
                                                    //saldos = (
                                                    //    from sal in saldos
                                                    //    join ac in listaAreaCuenta on new { area = (int)sal.area, cuenta = (int)sal.cuenta } equals new { area = ac.area, cuenta = ac.cuenta }
                                                    //    select sal
                                                    //).ToList();
                                                }
                                                #endregion
                                            }

                                            montoTotalInversion += saldos.Sum(x => x.saldoInicial + x.cargosAcumulados + x.abonosAcumulados);
                                        }
                                    }

                                    data.Add(new
                                    {
                                        inversion = inversion.descripcion,
                                        saldo = montoTotalInversion,
                                        grupo = inversion.grupo,
                                        renglonGrupo = false
                                    });

                                    montoTotalGrupo += montoTotalInversion;
                                }

                                if (grupo == 1)
                                {
                                    data.Add(new
                                    {
                                        inversion = "CONSOLIDADO",
                                        saldo = montoTotalGrupo,
                                        grupo = 1,
                                        renglonGrupo = true
                                    });
                                }
                                else if (grupo == 2)
                                {
                                    var saldoTotal = 0m;

                                    foreach (var d in data)
                                    {
                                        if (!d.renglonGrupo)
                                        {
                                            saldoTotal += Convert.ToDecimal(d.saldo);
                                        }
                                    }

                                    data.Add(new
                                    {
                                        inversion = "",
                                        saldo = saldoTotal,
                                        grupo = 2,
                                        renglonGrupo = true
                                    });
                                }
                            }
                            #endregion
                        }
                        #endregion
                        break;
                    case TipoDetalleEnum.DOCUMENTO:
                        #region DOCUMENTO
                        var year = fechaMesCorte.Year;
                        var mes = fechaMesCorte.Month;

                        var dataPesos = new List<DxPDetalleDTO>();
                        var dataDolares = new List<DxPDetalleDTO>();

                        var dataPuros = new List<DxPDetallePuroDTO>();
                        var dataPurosPesos = new List<DxPDetallePuroDTO>();
                        var dataPurosDolares = new List<DxPDetallePuroDTO>();

                        fechaMesCorte = new DateTime(year, mes, DateTime.DaysInMonth(year, mes));
                        var tipoCambio = GetTipoCambio(fechaMesCorte);

                        if (listaEmpresas.Count() == 1)
                        {
                            #region Datos de una empresa
                            var empresa = listaEmpresas.First();

                            using (var _ctx = new MainContext(empresa))
                            {
                                if (empresa == EmpresaEnum.Construplan)
                                {
                                    var institucionesPQ = _ctx.tblAF_DxP_Instituciones
                                        .Where(x => x.esPQ && x.Estatus)
                                        .Select(x => new DxPCuentaDTO
                                        {
                                            institucionId = x.Id,
                                            institucion = x.Nombre + " (REVOLVENTES)"
                                        }).ToList();

                                    for (int i = 1; i <= 2; i++)
                                    {
                                        foreach (var institucion in institucionesPQ)
                                        {
                                            var movimientoPQ = _ctx.tblAF_DxP_PQ
                                                    .Where(x =>
                                                        x.estatus &&
                                                        x.bancoId == institucion.institucionId &&
                                                        x.monedaId == i)
                                                    .GroupBy(x => x.interes).OrderByDescending(x => x.Count()).ToList();

                                            if (movimientoPQ.Count > 0)
                                            {
                                                institucion.tasa = movimientoPQ.First().Key;
                                                institucion.ctaAbono = movimientoPQ.First().First().ctaAbonoBanco;
                                                institucion.sctaAbono = movimientoPQ.First().First().sctaAbonoBanco;
                                                institucion.ssctaAbono = movimientoPQ.First().First().ssctaAbonoBanco;
                                                institucion.ctaCargo = movimientoPQ.First().First().ctaCargoBanco;
                                                institucion.sctaCargo = movimientoPQ.First().First().sctaCargoBanco;
                                                institucion.ssctaCargo = movimientoPQ.First().First().ssctaCargoBanco;

                                                var filtroSaldo = new FiltroSaldoDTO();
                                                filtroSaldo.empresa = empresa;
                                                filtroSaldo.year = year;
                                                filtroSaldo.mes = mes;
                                                filtroSaldo.cta = institucion.ctaAbono;
                                                filtroSaldo.scta = institucion.sctaAbono;
                                                filtroSaldo.sscta = institucion.ssctaAbono;
                                                filtroSaldo.listaCC = listaCC;
                                                filtroSaldo.listaAC = listaAC;

                                                var pqCortoPlazo = SaldoBalance(_ctx, filtroSaldo);
                                                var pqDetalle = new DxPDetalleDTO();
                                                pqDetalle.documento = institucion.institucion;
                                                pqDetalle.tasa = institucion.tasa;
                                                pqDetalle.totales = Math.Truncate(Math.Round(((pqCortoPlazo.Where(x => x.corte.anio == year && x.corte.mes == mes).Sum(x => (decimal?)x.saldoInicial + (decimal?)x.cargosAcumulados + (decimal?)x.abonosAcumulados) ?? 0) / 1000) * -1));
                                                pqDetalle.anio = pqDetalle.totales;

                                                if (i == (int)TipoMonedaEnum.MN)
                                                {
                                                    dataPesos.Add(pqDetalle);
                                                }
                                                else
                                                {
                                                    dataDolares.Add(pqDetalle);
                                                }
                                            }
                                        }
                                    }
                                }

                                var institucionesDxP = _ctx.tblAF_DxP_Instituciones
                                    .Where(x => !x.esPQ && x.Estatus)
                                    .Select(x => new DxPCuentaDTO
                                    {
                                        institucionId = x.Id,
                                        institucion = x.Nombre
                                    }).ToList();

                                for (int i = 1; i <= 2; i++)
                                {
                                    foreach (var institucion in institucionesDxP)
                                    {
                                        var contratos = _ctx.tblAF_DxP_Contratos
                                            .Where(x =>
                                                x.InstitucionId == institucion.institucionId &&
                                                x.monedaContrato == i &&
                                                x.fechaFirma <= fechaMesCorte &&
                                                !x.Terminado &&
                                                !x.arrendamientoPuro &&
                                                x.Estatus).ToList();

                                        if (contratos.Count > 0)
                                        {
                                            institucion.tasa = contratos.GroupBy(x => x.TasaInteres)
                                                .OrderByDescending(x => x.Count()).First().Key;

                                            var dxpCP = 0M;
                                            var dxpLPAnioMas1 = 0M;
                                            var dxpLPAnioMas2 = 0M;
                                            var dxpLPAnioMas3 = 0M;
                                            var dxpLPAnioMas4 = 0M;

                                            foreach (var contrato in contratos)
                                            {
                                                var filtroSaldo = new FiltroSaldoDTO();
                                                filtroSaldo.empresa = empresa;
                                                filtroSaldo.year = year;
                                                filtroSaldo.mes = mes;
                                                filtroSaldo.cta = contrato.cta;
                                                filtroSaldo.scta = contrato.scta;
                                                filtroSaldo.sscta = contrato.sscta;
                                                filtroSaldo.listaCC = listaCC;
                                                filtroSaldo.listaAC = listaAC;

                                                var dxpSaldoCP = SaldoBalance(_ctx, filtroSaldo);
                                                dxpCP += Math.Truncate(Math.Round(((dxpSaldoCP.Where(x => x.corte.anio == year && x.corte.mes == mes).Sum(x => (decimal?)x.saldoInicial + (decimal?)x.cargosAcumulados + (decimal?)x.abonosAcumulados) ?? 0) / 1000) * -1));

                                                var contratoDetalles = _ctx.tblAF_DxP_ContratosDetalle
                                                    .Where(x =>
                                                        x.ContratoId == contrato.Id &&
                                                        x.FechaVencimiento.Year > fechaMesCorte.Year &&
                                                        x.Estatus).ToList();

                                                dxpLPAnioMas1 += Math.Truncate(Math.Round((contratoDetalles.Where(x => x.FechaVencimiento.Year == (year + 1)).Sum(x => (decimal?)x.Importe) ?? 0) / 1000));
                                                dxpLPAnioMas2 += Math.Truncate(Math.Round((contratoDetalles.Where(x => x.FechaVencimiento.Year == (year + 2)).Sum(x => (decimal?)x.Importe) ?? 0) / 1000));
                                                dxpLPAnioMas3 += Math.Truncate(Math.Round((contratoDetalles.Where(x => x.FechaVencimiento.Year == (year + 3)).Sum(x => (decimal?)x.Importe) ?? 0) / 1000));
                                                dxpLPAnioMas4 += Math.Truncate(Math.Round((contratoDetalles.Where(x => x.FechaVencimiento.Year >= (year + 4)).Sum(x => (decimal?)x.Importe) ?? 0) / 1000));
                                            }

                                            var dxpDetalle = new DxPDetalleDTO();
                                            dxpDetalle.documento = institucion.institucion;
                                            dxpDetalle.tasa = institucion.tasa;
                                            dxpDetalle.totales = dxpCP + dxpLPAnioMas1 + dxpLPAnioMas2 + dxpLPAnioMas3 + dxpLPAnioMas4;
                                            dxpDetalle.anio = dxpCP;
                                            dxpDetalle.anioMas1 = dxpLPAnioMas1;
                                            dxpDetalle.anioMas2 = dxpLPAnioMas2;
                                            dxpDetalle.anioMas3 = dxpLPAnioMas3;
                                            dxpDetalle.anioMas4 = dxpLPAnioMas4;

                                            if (i == (int)TipoMonedaEnum.MN)
                                            {
                                                dataPesos.Add(dxpDetalle);
                                            }
                                            else
                                            {
                                                dataDolares.Add(dxpDetalle);
                                            }
                                        }
                                    }
                                }

                                var dxpTotal = new DxPDetalleDTO();
                                dxpTotal.documento = "TOTAL MONEDA NACIONAL";
                                dxpTotal.totales = dataPesos.Sum(x => x.totales);
                                dxpTotal.anio = dataPesos.Sum(x => x.anio);
                                dxpTotal.anioMas1 = dataPesos.Sum(x => x.anioMas1);
                                dxpTotal.anioMas2 = dataPesos.Sum(x => x.anioMas2);
                                dxpTotal.anioMas3 = dataPesos.Sum(x => x.anioMas3);
                                dxpTotal.anioMas4 = dataPesos.Sum(x => x.anioMas4);
                                dxpTotal.renglonGrupo = true;
                                dataPesos.Add(dxpTotal);
                                var dxpTotalDLL = new DxPDetalleDTO();
                                dxpTotalDLL.documento = "TOTAL DOLARES";
                                dxpTotalDLL.totales = dataDolares.Sum(x => x.totales);
                                dxpTotalDLL.anio = dataDolares.Sum(x => x.anio);
                                dxpTotalDLL.anioMas1 = dataDolares.Sum(x => x.anioMas1);
                                dxpTotalDLL.anioMas2 = dataDolares.Sum(x => x.anioMas2);
                                dxpTotalDLL.anioMas3 = dataDolares.Sum(x => x.anioMas3);
                                dxpTotalDLL.anioMas4 = dataDolares.Sum(x => x.anioMas4);
                                dxpTotalDLL.renglonGrupo = true;
                                dataDolares.Add(dxpTotalDLL);
                                var dxpGranTotal = new DxPDetalleDTO();
                                dxpGranTotal.documento = "ADEUDO TOTAL EN M.N.";
                                dxpGranTotal.totales = Math.Truncate(Math.Round(dxpTotal.totales + (dxpTotalDLL.totales * tipoCambio)));
                                dxpGranTotal.anio = Math.Truncate(Math.Round(dxpTotal.anio + (dxpTotalDLL.anio * tipoCambio)));
                                dxpGranTotal.anioMas1 = dxpTotal.anioMas1 + (dxpTotalDLL.anioMas1 * tipoCambio);
                                dxpGranTotal.anioMas2 = dxpTotal.anioMas2 + (dxpTotalDLL.anioMas2 * tipoCambio);
                                dxpGranTotal.anioMas3 = dxpTotal.anioMas3 + (dxpTotalDLL.anioMas3 * tipoCambio);
                                dxpGranTotal.anioMas4 = dxpTotal.anioMas4 + (dxpTotalDLL.anioMas4 * tipoCambio);
                                dxpGranTotal.renglonGrupo = true;
                                dataDolares.Add(dxpGranTotal);
                                var dxpTipoCambio = new DxPDetalleDTO();
                                dxpTipoCambio.documento = "TIPO CAMBIO: " + tipoCambio;
                                dxpTipoCambio.renglonGrupo = true;
                                dataDolares.Add(dxpTipoCambio);
                            }

                            data.AddRange(dataPesos);
                            data.AddRange(dataDolares);

                            if (empresa == EmpresaEnum.Arrendadora)
                            {
                                using (var _ctx = new MainContext(empresa))
                                {
                                    for (int i = 1; i <= 2; i++)
                                    {
                                        var contratos = _ctx.tblAF_DxP_Contratos
                                            .Where(x =>
                                                x.monedaContrato == i &&
                                                x.fechaFirma <= fechaMesCorte &&
                                                !x.Terminado &&
                                                x.arrendamientoPuro &&
                                                x.Estatus).ToList();

                                        if (contratos.Count > 0)
                                        {
                                            foreach (var contrato in contratos)
                                            {
                                                var institucion = _ctx.tblAF_DxP_Instituciones.First(x => x.Id == contrato.InstitucionId);
                                                var ContratoMaquina = _ctx.tblAF_DxP_ContratoMaquinas.First(x => x.ContratoId == contrato.Id);
                                                var detalleContratoMaquina = _ctx.tblAF_DxP_ContratoMaquinasDetalle.Where(x => x.ContratoMaquinaId == ContratoMaquina.Id && x.Estatus).ToList();
                                                var maquina = _ctx.tblM_CatMaquina.First(x => x.id == ContratoMaquina.MaquinaId);

                                                var puro = new DxPDetallePuroDTO();
                                                puro.documento = institucion.Nombre;
                                                puro.equipo = maquina.noEconomico;
                                                puro.inicio = contrato.FechaInicio.ToString("dd/MM/yyyy");
                                                puro.fin = contrato.FechaInicio.AddMonths(contrato.Plazo).ToString("dd/MM/yyyy");
                                                puro.renta = detalleContratoMaquina.First().Importe;
                                                puro.pendientes = detalleContratoMaquina.Where(x => !x.Pagado).Count();
                                                puro.anio = Math.Truncate(Math.Round((detalleContratoMaquina.Where(x => x.FechaVencimiento.Year == year && !x.Pagado).Sum(x => (decimal?)x.Importe) ?? 0) / 1000));
                                                puro.anio2 = Math.Truncate(Math.Round((detalleContratoMaquina.Where(x => x.FechaVencimiento.Year > year).Sum(x => (decimal?)x.Importe) ?? 0) / 1000));

                                                //var filtro = new FiltroSaldoDTO();
                                                //filtro.empresa = empresa;
                                                //filtro.year = year;
                                                //filtro.mes = mes;
                                                //filtro.cta = contrato.cta;
                                                //filtro.scta = contrato.scta;
                                                //filtro.sscta = contrato.sscta;
                                                //filtro.listaCC = listaCC;
                                                //filtro.listaAC = listaAreaCuenta;

                                                //var saldo = SaldoBalance(_ctx, filtro);
                                                //puro.anio = Math.Truncate(Math.Round((saldo
                                                //    .Where(x => x.corte.anio == year && x.corte.mes == mes)
                                                //    .Sum(x => (decimal?)x.saldoInicial + (decimal?)x.cargosAcumulados + (decimal?)x.abonosAcumulados) ?? 0) / 1000));

                                                if (i == (int)TipoMonedaEnum.MN)
                                                {
                                                    dataPurosPesos.Add(puro);
                                                }
                                                else
                                                {
                                                    dataPurosDolares.Add(puro);
                                                }
                                            }
                                        }
                                    }
                                }

                                dataPuros.AddRange(dataPurosDolares);

                                var dxpTotalDolares = new DxPDetallePuroDTO();
                                dxpTotalDolares.documento = "TOTAL ADEUDO USD";
                                dxpTotalDolares.equipo = "";
                                dxpTotalDolares.inicio = "";
                                dxpTotalDolares.fin = "";
                                dxpTotalDolares.anio = dataPurosDolares.Sum(x => x.anio);
                                dxpTotalDolares.anio2 = dataPurosDolares.Sum(x => x.anio2);
                                dxpTotalDolares.renglonGrupo = true;
                                dataPuros.Add(dxpTotalDolares);

                                var dxpTotalPesos = new DxPDetallePuroDTO();
                                dxpTotalPesos.documento = "ADEUDO TOTAL EN M.N.";
                                dxpTotalPesos.equipo = "";
                                dxpTotalPesos.inicio = "";
                                dxpTotalPesos.fin = "";
                                dxpTotalPesos.anio = Math.Truncate(Math.Round(dxpTotalDolares.anio * tipoCambio));
                                dxpTotalPesos.anio2 = Math.Truncate(Math.Round(dxpTotalDolares.anio2 * tipoCambio));
                                dxpTotalPesos.renglonGrupo = true;
                                dataPuros.Add(dxpTotalPesos);

                                var dxpTipoCambio = new DxPDetallePuroDTO();
                                dxpTipoCambio.documento = "TIPO CAMBIO: " + tipoCambio;
                                dxpTipoCambio.renglonGrupo = true;
                                dataPuros.Add(dxpTipoCambio);
                            }
                            #endregion
                        }
                        else
                        {
                            #region Datos de múltiples empresas
                            foreach (var empresa in listaEmpresas)
                            {
                                using (var _ctx = new MainContext(empresa))
                                {
                                    if (empresa == EmpresaEnum.Construplan)
                                    {
                                        var institucionesPQ = _ctx.tblAF_DxP_Instituciones
                                            .Where(x => x.esPQ && x.Estatus)
                                            .Select(x => new DxPCuentaDTO
                                            {
                                                institucionId = x.Id,
                                                institucion = x.Nombre + " (REVOLVENTES)"
                                            }).ToList();

                                        for (int i = 1; i <= 2; i++)
                                        {
                                            foreach (var institucion in institucionesPQ)
                                            {
                                                var movimientoPQ = _ctx.tblAF_DxP_PQ
                                                        .Where(x =>
                                                            x.estatus &&
                                                            x.bancoId == institucion.institucionId &&
                                                            x.monedaId == i)
                                                        .GroupBy(x => x.interes).OrderByDescending(x => x.Count()).ToList();

                                                if (movimientoPQ.Count > 0)
                                                {
                                                    institucion.tasa = movimientoPQ.First().Key;
                                                    institucion.ctaAbono = movimientoPQ.First().First().ctaAbonoBanco;
                                                    institucion.sctaAbono = movimientoPQ.First().First().sctaAbonoBanco;
                                                    institucion.ssctaAbono = movimientoPQ.First().First().ssctaAbonoBanco;
                                                    institucion.ctaCargo = movimientoPQ.First().First().ctaCargoBanco;
                                                    institucion.sctaCargo = movimientoPQ.First().First().sctaCargoBanco;
                                                    institucion.ssctaCargo = movimientoPQ.First().First().ssctaCargoBanco;

                                                    var filtroSaldo = new FiltroSaldoDTO();
                                                    filtroSaldo.empresa = empresa;
                                                    filtroSaldo.year = year;
                                                    filtroSaldo.mes = mes;
                                                    filtroSaldo.cta = institucion.ctaAbono;
                                                    filtroSaldo.scta = institucion.sctaAbono;
                                                    filtroSaldo.sscta = institucion.ssctaAbono;
                                                    filtroSaldo.listaCC = listaCC;
                                                    filtroSaldo.listaAC = listaAC;

                                                    var pqCortoPlazo = SaldoBalance(_ctx, filtroSaldo);
                                                    var pqDetalle = new DxPDetalleDTO();
                                                    pqDetalle.documento = institucion.institucion;
                                                    pqDetalle.tasa = institucion.tasa;
                                                    pqDetalle.totales = ((pqCortoPlazo.Where(x => x.corte.anio == year && x.corte.mes == mes).Sum(x => (decimal?)x.saldoInicial + (decimal?)x.cargosAcumulados + (decimal?)x.abonosAcumulados) ?? 0) / 1000) * -1;
                                                    pqDetalle.anio = pqDetalle.totales;

                                                    if (i == (int)TipoMonedaEnum.MN)
                                                    {
                                                        dataPesos.Add(pqDetalle);
                                                    }
                                                    else
                                                    {
                                                        dataDolares.Add(pqDetalle);
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    var institucionesDxP = _ctx.tblAF_DxP_Instituciones
                                        .Where(x => !x.esPQ && x.Estatus)
                                        .Select(x => new DxPCuentaDTO
                                        {
                                            institucionId = x.Id,
                                            institucion = x.Nombre
                                        }).ToList();

                                    for (int i = 1; i <= 2; i++)
                                    {
                                        foreach (var institucion in institucionesDxP)
                                        {
                                            var contratos = _ctx.tblAF_DxP_Contratos
                                                .Where(x =>
                                                    x.InstitucionId == institucion.institucionId &&
                                                    x.monedaContrato == i &&
                                                    x.fechaFirma <= fechaMesCorte &&
                                                    !x.Terminado &&
                                                    !x.arrendamientoPuro &&
                                                    x.Estatus).ToList();

                                            if (contratos.Count > 0)
                                            {
                                                institucion.tasa = contratos.GroupBy(x => x.TasaInteres)
                                                    .OrderByDescending(x => x.Count()).First().Key;

                                                var dxpCP = 0M;
                                                var dxpLPAnioMas1 = 0M;
                                                var dxpLPAnioMas2 = 0M;
                                                var dxpLPAnioMas3 = 0M;
                                                var dxpLPAnioMas4 = 0M;

                                                foreach (var contrato in contratos)
                                                {
                                                    var filtroSaldo = new FiltroSaldoDTO();
                                                    filtroSaldo.empresa = empresa;
                                                    filtroSaldo.year = year;
                                                    filtroSaldo.mes = mes;
                                                    filtroSaldo.cta = contrato.cta;
                                                    filtroSaldo.scta = contrato.scta;
                                                    filtroSaldo.sscta = contrato.sscta;
                                                    filtroSaldo.listaCC = listaCC;
                                                    filtroSaldo.listaAC = listaAC;

                                                    var dxpSaldoCP = SaldoBalance(_ctx, filtroSaldo);
                                                    dxpCP += ((dxpSaldoCP.Where(x => x.corte.anio == year && x.corte.mes == mes).Sum(x => (decimal?)x.saldoInicial + (decimal?)x.cargosAcumulados + (decimal?)x.abonosAcumulados) ?? 0) / 1000) * -1;

                                                    var contratoDetalles = _ctx.tblAF_DxP_ContratosDetalle
                                                        .Where(x =>
                                                            x.ContratoId == contrato.Id &&
                                                            x.FechaVencimiento.Year > fechaMesCorte.Year &&
                                                            x.Estatus).ToList();

                                                    dxpLPAnioMas1 += (contratoDetalles.Where(x => x.FechaVencimiento.Year == (year + 1)).Sum(x => (decimal?)x.Importe) ?? 0) / 1000;
                                                    dxpLPAnioMas2 += (contratoDetalles.Where(x => x.FechaVencimiento.Year == (year + 2)).Sum(x => (decimal?)x.Importe) ?? 0) / 1000;
                                                    dxpLPAnioMas3 += (contratoDetalles.Where(x => x.FechaVencimiento.Year == (year + 3)).Sum(x => (decimal?)x.Importe) ?? 0) / 1000;
                                                    dxpLPAnioMas4 += (contratoDetalles.Where(x => x.FechaVencimiento.Year >= (year + 4)).Sum(x => (decimal?)x.Importe) ?? 0) / 1000;
                                                }

                                                var dxpDetalle = new DxPDetalleDTO();
                                                dxpDetalle.documento = institucion.institucion;
                                                dxpDetalle.tasa = institucion.tasa;
                                                dxpDetalle.totales = dxpCP + dxpLPAnioMas1 + dxpLPAnioMas2 + dxpLPAnioMas3 + dxpLPAnioMas4;
                                                dxpDetalle.anio = dxpCP;
                                                dxpDetalle.anioMas1 = dxpLPAnioMas1;
                                                dxpDetalle.anioMas2 = dxpLPAnioMas2;
                                                dxpDetalle.anioMas3 = dxpLPAnioMas3;
                                                dxpDetalle.anioMas4 = dxpLPAnioMas4;

                                                if (i == (int)TipoMonedaEnum.MN)
                                                {
                                                    dataPesos.Add(dxpDetalle);
                                                }
                                                else
                                                {
                                                    dataDolares.Add(dxpDetalle);
                                                }
                                            }
                                        }
                                    }

                                    if (empresa == EmpresaEnum.Arrendadora)
                                    {
                                        var contratos = _ctx.tblAF_DxP_Contratos
                                            .Where(x =>
                                                x.monedaContrato == (int)TipoMonedaEnum.USD &&
                                                x.fechaFirma <= fechaMesCorte &&
                                                !x.Terminado &&
                                                x.arrendamientoPuro &&
                                                x.Estatus).ToList();

                                        if (contratos.Count > 0)
                                        {
                                            foreach (var contrato in contratos)
                                            {
                                                var institucion = _ctx.tblAF_DxP_Instituciones.First(x => x.Id == contrato.InstitucionId);
                                                var ContratoMaquina = _ctx.tblAF_DxP_ContratoMaquinas.First(x => x.ContratoId == contrato.Id);
                                                var detalleContratoMaquina = _ctx.tblAF_DxP_ContratoMaquinasDetalle.Where(x => x.ContratoMaquinaId == ContratoMaquina.Id && x.Estatus).ToList();
                                                var maquina = _ctx.tblM_CatMaquina.First(x => x.id == ContratoMaquina.MaquinaId);

                                                var puro = new DxPDetallePuroDTO();
                                                puro.documento = institucion.Nombre;
                                                puro.equipo = maquina.noEconomico;
                                                puro.inicio = contrato.FechaInicio.ToString("dd/MM/yyyy");
                                                puro.fin = contrato.FechaInicio.AddMonths(contrato.Plazo).ToString("dd/MM/yyyy");
                                                puro.renta = detalleContratoMaquina.First().Importe;
                                                puro.pendientes = detalleContratoMaquina.Where(x => !x.Pagado).Count();
                                                puro.anio = Math.Truncate(Math.Round((detalleContratoMaquina.Where(x => x.FechaVencimiento.Year == year && !x.Pagado).Sum(x => (decimal?)x.Importe) ?? 0) / 1000));
                                                puro.anio2 = Math.Truncate(Math.Round((detalleContratoMaquina.Where(x => x.FechaVencimiento.Year > year).Sum(x => (decimal?)x.Importe) ?? 0) / 1000));

                                                //var filtro = new FiltroSaldoDTO();
                                                //filtro.empresa = empresa;
                                                //filtro.year = year;
                                                //filtro.mes = mes;
                                                //filtro.cta = contrato.cta;
                                                //filtro.scta = contrato.scta;
                                                //filtro.sscta = contrato.sscta;
                                                //filtro.listaCC = listaCC;
                                                //filtro.listaAC = listaAreaCuenta;

                                                //var saldo = SaldoBalance(_ctx, filtro);
                                                //puro.anio = Math.Truncate(Math.Round((saldo
                                                //    .Where(x => x.corte.anio == year && x.corte.mes == mes)
                                                //    .Sum(x => (decimal?)x.saldoInicial + (decimal?)x.cargosAcumulados + (decimal?)x.abonosAcumulados) ?? 0) / 1000));

                                                dataPurosDolares.Add(puro);
                                            }
                                        }

                                        dataPuros.AddRange(dataPurosDolares);

                                        var dxpTotalDolares = new DxPDetallePuroDTO();
                                        dxpTotalDolares.documento = "TOTAL ADEUDO USD";
                                        dxpTotalDolares.equipo = "";
                                        dxpTotalDolares.inicio = "";
                                        dxpTotalDolares.fin = "";
                                        dxpTotalDolares.anio = dataPurosDolares.Sum(x => x.anio);
                                        dxpTotalDolares.anio2 = dataPurosDolares.Sum(x => x.anio2);
                                        dxpTotalDolares.renglonGrupo = true;
                                        dataPuros.Add(dxpTotalDolares);

                                        var dxpTotalPesos = new DxPDetallePuroDTO();
                                        dxpTotalPesos.documento = "ADEUDO TOTAL EN M.N.";
                                        dxpTotalPesos.equipo = "";
                                        dxpTotalPesos.inicio = "";
                                        dxpTotalPesos.fin = "";
                                        dxpTotalPesos.anio = dxpTotalDolares.anio * tipoCambio;
                                        dxpTotalPesos.anio2 = dxpTotalDolares.anio2 * tipoCambio;
                                        dxpTotalPesos.renglonGrupo = true;
                                        dataPuros.Add(dxpTotalPesos);

                                        var dxpTipoCambioPuro = new DxPDetallePuroDTO();
                                        dxpTipoCambioPuro.documento = "TIPO CAMBIO: " + tipoCambio;
                                        dxpTipoCambioPuro.renglonGrupo = true;
                                        dataPuros.Add(dxpTipoCambioPuro);
                                    }
                                }
                            }

                            var dxpTotal = new DxPDetalleDTO();
                            dxpTotal.documento = "TOTAL MONEDA NACIONAL";
                            dxpTotal.totales = dataPesos.Sum(x => x.totales);
                            dxpTotal.anio = dataPesos.Sum(x => x.anio);
                            dxpTotal.anioMas1 = dataPesos.Sum(x => x.anioMas1);
                            dxpTotal.anioMas2 = dataPesos.Sum(x => x.anioMas2);
                            dxpTotal.anioMas3 = dataPesos.Sum(x => x.anioMas3);
                            dxpTotal.anioMas4 = dataPesos.Sum(x => x.anioMas4);
                            dxpTotal.renglonGrupo = true;
                            dataPesos.Add(dxpTotal);
                            var dxpTotalDLL = new DxPDetalleDTO();
                            dxpTotalDLL.documento = "TOTAL DOLARES";
                            dxpTotalDLL.totales = dataDolares.Sum(x => x.totales);
                            dxpTotalDLL.anio = dataDolares.Sum(x => x.anio);
                            dxpTotalDLL.anioMas1 = dataDolares.Sum(x => x.anioMas1);
                            dxpTotalDLL.anioMas2 = dataDolares.Sum(x => x.anioMas2);
                            dxpTotalDLL.anioMas3 = dataDolares.Sum(x => x.anioMas3);
                            dxpTotalDLL.anioMas4 = dataDolares.Sum(x => x.anioMas4);
                            dxpTotalDLL.renglonGrupo = true;
                            dataDolares.Add(dxpTotalDLL);
                            var dxpGranTotal = new DxPDetalleDTO();
                            dxpGranTotal.documento = "ADEUDO TOTAL EN M.N.";
                            dxpGranTotal.totales = dxpTotal.totales + (dxpTotalDLL.totales * tipoCambio);
                            dxpGranTotal.anio = dxpTotal.anio + (dxpTotalDLL.anio * tipoCambio);
                            dxpGranTotal.anioMas1 = dxpTotal.anioMas1 + (dxpTotalDLL.anioMas1 * tipoCambio);
                            dxpGranTotal.anioMas2 = dxpTotal.anioMas2 + (dxpTotalDLL.anioMas2 * tipoCambio);
                            dxpGranTotal.anioMas3 = dxpTotal.anioMas3 + (dxpTotalDLL.anioMas3 * tipoCambio);
                            dxpGranTotal.anioMas4 = dxpTotal.anioMas4 + (dxpTotalDLL.anioMas4 * tipoCambio);
                            dxpGranTotal.renglonGrupo = true;
                            dataDolares.Add(dxpGranTotal);
                            var dxpTipoCambio = new DxPDetalleDTO();
                            dxpTipoCambio.documento = "TIPO CAMBIO: " + tipoCambio;
                            dxpTipoCambio.renglonGrupo = true;
                            dataDolares.Add(dxpTipoCambio);

                            foreach (var pesos in dataPesos.GroupBy(x => x.documento))
                            {
                                var info = new DxPDetalleDTO();
                                info.documento = pesos.Key;
                                info.tasa = pesos.First().tasa;
                                info.totales = pesos.Sum(x => x.totales);
                                info.anio = pesos.Sum(x => x.anio);
                                info.anioMas1 = pesos.Sum(x => x.anioMas1);
                                info.anioMas2 = pesos.Sum(x => x.anioMas2);
                                info.anioMas3 = pesos.Sum(x => x.anioMas3);
                                info.anioMas4 = pesos.Sum(x => x.anioMas4);
                                info.renglonGrupo = pesos.First().renglonGrupo;
                                data.Add(info);
                            }

                            foreach (var dolares in dataDolares.GroupBy(x => x.documento))
                            {
                                var info = new DxPDetalleDTO();
                                info.documento = dolares.Key;
                                info.tasa = dolares.First().tasa;
                                info.totales = dolares.Sum(x => x.totales);
                                info.anio = dolares.Sum(x => x.anio);
                                info.anioMas1 = dolares.Sum(x => x.anioMas1);
                                info.anioMas2 = dolares.Sum(x => x.anioMas2);
                                info.anioMas3 = dolares.Sum(x => x.anioMas3);
                                info.anioMas4 = dolares.Sum(x => x.anioMas4);
                                info.renglonGrupo = dolares.First().renglonGrupo;
                                data.Add(info);
                            }
                            #endregion
                        }

                        resultado.Add("dxpPuro", dataPuros);
                        #endregion
                        break;
                    case TipoDetalleEnum.GFX_INDICADORES:
                        #region GFX INDICADORES
                        {
                            List<BalanceDTO> infoBalanceActivo = new List<BalanceDTO>();
                            List<BalanceDTO> infoBalancePasivo = new List<BalanceDTO>();
                            List<BalanceDTO> infoBalanceActivoConsolidado = new List<BalanceDTO>();
                            List<BalanceDTO> infoBalancePasivoConsolidado = new List<BalanceDTO>();
                            List<InfoIndicadoresDTO> Ebitdas = new List<InfoIndicadoresDTO>();
                            List<InfoIndicadoresDTO> deudaEbitdas = new List<InfoIndicadoresDTO>();

                            #region EBITDA
                            var fechaAnterior = fechaMesCorte.AddMonths(-12);

                            if (listaEmpresas.Count == 1)
                            {
                                #region Datos de una empresa
                                var empresa = listaEmpresas.First();

                                for (int _mes = fechaAnterior.Month; _mes <= 12; _mes++)
                                {
                                    using (var _ctx = new MainContext(empresa))
                                    {
                                        var filtro = new FiltroSaldoDTO();
                                        filtro.empresa = empresa;
                                        filtro.year = fechaAnterior.Year;
                                        filtro.mes = _mes;
                                        filtro.listaCC = listaCC;
                                        filtro.listaAC = listaAC;

                                        var indicadores = SaldoIndicadores(_ctx, filtro);

                                        var ebitda = indicadores.Sum(x => x.ebitda);

                                        var ebitdaAnterior = Ebitdas.LastOrDefault(x => x.empresa == empresa);
                                        if (ebitdaAnterior != null)
                                        {
                                            ebitda += ebitdaAnterior.ebitda;
                                        }

                                        var infoIndicadores = new InfoIndicadoresDTO();
                                        infoIndicadores.empresa = empresa;
                                        infoIndicadores.year = fechaAnterior.Year;
                                        infoIndicadores.mes = _mes;
                                        infoIndicadores.ebitda = ebitda;
                                        Ebitdas.Add(infoIndicadores);
                                    }
                                }

                                for (int _mes = 1; _mes <= fechaMesCorte.Month; _mes++)
                                {
                                    using (var _ctx = new MainContext(empresa))
                                    {
                                        var filtro = new FiltroSaldoDTO();
                                        filtro.empresa = empresa;
                                        filtro.year = fechaMesCorte.Year;
                                        filtro.mes = _mes;
                                        filtro.listaCC = listaCC;
                                        filtro.listaAC = listaAC;

                                        var indicadores = SaldoIndicadores(_ctx, filtro);

                                        var ebitda = indicadores.Sum(x => x.ebitda);

                                        var ebitdaAnterior = Ebitdas.LastOrDefault(x => x.empresa == empresa);
                                        if (ebitdaAnterior != null)
                                        {
                                            ebitda += ebitdaAnterior.ebitda;
                                        }

                                        var infoIndicadores = new InfoIndicadoresDTO();
                                        infoIndicadores.empresa = empresa;
                                        infoIndicadores.year = fechaMesCorte.Year;
                                        infoIndicadores.mes = _mes;
                                        infoIndicadores.ebitda = ebitda;
                                        Ebitdas.Add(infoIndicadores);
                                    }
                                }
                            #endregion
                            }
                            else
                            {
                                #region Datos consolidados
                                for (int _mes = fechaAnterior.Month; _mes <= 12; _mes++)
                                {
                                    var mesConsolidado = 0M;

                                    foreach (var empresa in listaEmpresas)
                                    {
                                        using (var _ctx = new MainContext(empresa))
                                        {
                                            var filtro = new FiltroSaldoDTO();
                                            filtro.empresa = empresa;
                                            filtro.year = fechaAnterior.Year;
                                            filtro.mes = _mes;
                                            filtro.listaCC = listaCC;
                                            filtro.listaAC = listaAC;

                                            var indicadores = SaldoIndicadores(_ctx, filtro);

                                            var ebitda = indicadores.Sum(x => x.ebitda);

                                            var ebitdaAnterior = Ebitdas.LastOrDefault(x => x.empresa == null);
                                            if (ebitdaAnterior != null)
                                            {
                                                ebitda += ebitdaAnterior.ebitda;
                                            }

                                            mesConsolidado += ebitda;
                                        }
                                    }

                                    var infoIndicadores = new InfoIndicadoresDTO();
                                    infoIndicadores.empresa = null;
                                    infoIndicadores.year = fechaAnterior.Year;
                                    infoIndicadores.mes = _mes;
                                    infoIndicadores.ebitda = mesConsolidado;
                                    Ebitdas.Add(infoIndicadores);
                                }

                                for (int _mes = 1; _mes <= fechaMesCorte.Month; _mes++)
                                {
                                    var mesConsolidado = 0M;

                                    foreach (var empresa in listaEmpresas)
                                    {
                                        using (var _ctx = new MainContext(empresa))
                                        {
                                            var filtro = new FiltroSaldoDTO();
                                            filtro.empresa = empresa;
                                            filtro.year = fechaMesCorte.Year;
                                            filtro.mes = _mes;
                                            filtro.listaCC = listaCC;
                                            filtro.listaAC = listaAC;

                                            var indicadores = SaldoIndicadores(_ctx, filtro);

                                            var ebitda = indicadores.Sum(x => x.ebitda);

                                            var ebitdaAnterior = Ebitdas.LastOrDefault(x => x.empresa == null);
                                            if (ebitdaAnterior != null)
                                            {
                                                ebitda += ebitdaAnterior.ebitda;
                                            }

                                            mesConsolidado += ebitda;
                                        }
                                    }

                                    var infoIndicadores = new InfoIndicadoresDTO();
                                    infoIndicadores.empresa = null;
                                    infoIndicadores.year = fechaMesCorte.Year;
                                    infoIndicadores.mes = _mes;
                                    infoIndicadores.ebitda = mesConsolidado;
                                    Ebitdas.Add(infoIndicadores);
                                }
                                #endregion
                            }

                            #region GFX
                            HighchartDTO gfxEbitda = new HighchartDTO();
                            xAxisDTO xAxis = new xAxisDTO();
                            xAxis.crosshair = true;

                            for (int _mes = 1; _mes <= fechaMesCorte.Month; _mes++)
                            {
                                xAxis.categories.Add(new DateTime(fechaMesCorte.Year, _mes, 1).ToString("MMMM yyyy"));
                            }

                            gfxEbitda.xAxis = xAxis;

                            for (int empresaNum = 0; empresaNum <= listaEmpresas.Count; empresaNum++)
                            {
                                var serieEbitda = new SerieDTO();
                                if (empresaNum < listaEmpresas.Count)
                                {
                                    var nombreEmpresa = listaEmpresas[empresaNum].GetDescription();

                                    serieEbitda.name = nombreEmpresa;

                                    for (int _mes = 1; _mes <= fechaMesCorte.Month; _mes++)
                                    {
                                        var resultadoEbitda = Ebitdas
                                            .FirstOrDefault(x =>
                                                x.empresa == listaEmpresas[empresaNum] &&
                                                x.year == fechaMesCorte.Year &&
                                                x.mes == _mes
                                            );

                                        serieEbitda.data.Add(resultadoEbitda != null ? (resultadoEbitda.ebitda / 1000) : 0);
                                    }
                                }
                                else
                                {
                                    var nombreEmpresa = "CONSOLIDADO";

                                    serieEbitda.name = nombreEmpresa;

                                    for (int _mes = 1; _mes <= fechaMesCorte.Month; _mes++)
                                    {
                                        var resultadoEbitda = Ebitdas
                                            .FirstOrDefault(x =>
                                                x.empresa == null &&
                                                x.year == fechaMesCorte.Year &&
                                                x.mes == _mes
                                            );

                                        serieEbitda.data.Add(resultadoEbitda != null ? (resultadoEbitda.ebitda / 1000) : 0);
                                    }
                                }

                                gfxEbitda.series.Add(serieEbitda);
                            }

                            resultado.Add("gfxEbitda", gfxEbitda);
                            #endregion
                            #endregion

                            #region DEUDA/EBITDA
                            #region Datos de una empresa
                            {
                                var empresa = listaEmpresas.First();

                                for (int _mes = 1; _mes <= fechaMesCorte.Month; _mes++)
                                {
                                    using (var _ctx = new MainContext(empresa))
                                    {
                                        var filtro = new FiltroSaldoDTO();
                                        filtro.empresa = empresa;
                                        filtro.year = fechaMesCorte.Year;
                                        filtro.mes = _mes;
                                        filtro.cta = 2120;
                                        filtro.listaAC = listaAC;
                                        filtro.listaCC = listaCC;

                                        var saldoCortoPlazo = SaldoBalanceUnPeriodo(_ctx, filtro);

                                        var cortoPlazo = saldoCortoPlazo.Sum(x => x.saldoInicial + x.cargosAcumulados + x.abonosAcumulados) * -1;

                                        filtro.cta = 2135;

                                        var saldoLargoPlazo = SaldoBalanceUnPeriodo(_ctx, filtro);

                                        var largoPlazo = saldoLargoPlazo.Sum(x => x.saldoInicial + x.cargosAcumulados + x.abonosAcumulados) * -1;

                                        var infoDeudaEbitda = new InfoIndicadoresDTO();
                                        infoDeudaEbitda.empresa = empresa;
                                        infoDeudaEbitda.year = fechaMesCorte.Year;
                                        infoDeudaEbitda.mes = _mes;
                                        infoDeudaEbitda.dxpCortoPlazo = cortoPlazo;
                                        infoDeudaEbitda.dxpLargoPlazo = largoPlazo;
                                        deudaEbitdas.Add(infoDeudaEbitda);
                                    }
                                }
                            }
                            #endregion

                            #region Datos consolidados
                            {
                                for (int _mes = 1; _mes <= fechaMesCorte.Month; _mes++)
                                {
                                    var mesCortoPlazo = 0M;
                                    var mesLargoPlazo = 0M;

                                    foreach (var empresa in listaEmpresas)
                                    {
                                        using (var _ctx = new MainContext(empresa))
                                        {
                                            var filtro = new FiltroSaldoDTO();
                                            filtro.empresa = empresa;
                                            filtro.year = fechaMesCorte.Year;
                                            filtro.mes = _mes;
                                            filtro.cta = 2120;
                                            filtro.listaAC = listaAC;
                                            filtro.listaCC = listaCC;

                                            var saldoCortoPlazo = SaldoBalanceUnPeriodo(_ctx, filtro);

                                            mesCortoPlazo += saldoCortoPlazo.Sum(x => x.saldoInicial + x.cargosAcumulados + x.abonosAcumulados) * -1;

                                            filtro.cta = 2135;

                                            var saldoLargoPlazo = SaldoBalanceUnPeriodo(_ctx, filtro);

                                            mesLargoPlazo += saldoLargoPlazo.Sum(x => x.saldoInicial + x.cargosAcumulados + x.abonosAcumulados) * -1;
                                        }
                                    }

                                    var infoDeudaEbitda = new InfoIndicadoresDTO();
                                    infoDeudaEbitda.empresa = null;
                                    infoDeudaEbitda.year = fechaMesCorte.Year;
                                    infoDeudaEbitda.mes = _mes;
                                    infoDeudaEbitda.dxpCortoPlazo = mesCortoPlazo;
                                    infoDeudaEbitda.dxpLargoPlazo = mesLargoPlazo;
                                    deudaEbitdas.Add(infoDeudaEbitda);
                                }
                            }
                            #endregion

                            #region GFX
                            HighchartDTO gfxDeudaEbitda = new HighchartDTO();
                            gfxDeudaEbitda.xAxis = xAxis;

                            for (int empresaNum = 0; empresaNum <= listaEmpresas.Count; empresaNum++)
                            {
                                var serieDeudaEbitda = new SerieDTO();
                                if (empresaNum < listaEmpresas.Count)
                                {
                                    var nombreEmpresa = listaEmpresas[empresaNum].GetDescription();

                                    serieDeudaEbitda.name = nombreEmpresa;

                                    for (int _mes = 0; _mes <= fechaMesCorte.Month; _mes++)
                                    {
                                        var resultadoDeudaEbitda = deudaEbitdas
                                            .FirstOrDefault(x =>
                                                x.empresa == null &&
                                                x.year == fechaMesCorte.Year &&
                                                x.mes == _mes
                                            );

                                        var ebitdaDelPeriodo = Ebitdas
                                            .FirstOrDefault(x =>
                                                x.empresa == listaEmpresas[empresaNum] &&
                                                x.year == fechaMesCorte.Year &&
                                                x.mes == _mes
                                            );

                                        if (resultadoDeudaEbitda != null && ebitdaDelPeriodo != null && ebitdaDelPeriodo.ebitda != 0)
                                        {
                                            var valor = ((resultadoDeudaEbitda.dxpCortoPlazo + resultadoDeudaEbitda.dxpLargoPlazo / 1000)) / ebitdaDelPeriodo.ebitda;
                                            serieDeudaEbitda.data.Add(valor);
                                        }
                                        else
                                        {
                                            serieDeudaEbitda.data.Add(0);
                                        }
                                    }
                                }
                                else
                                {
                                    var nombreEmpresa = "CONSOLIDADO";

                                    serieDeudaEbitda.name = nombreEmpresa;

                                    for (int _mes = 0; _mes <= fechaMesCorte.Month; _mes++)
                                    {
                                        var resultadoDeudaEbitda = deudaEbitdas
                                            .FirstOrDefault(x =>
                                                x.empresa == null &&
                                                x.year == fechaMesCorte.Year &&
                                                x.mes == _mes
                                            );

                                        var ebitdaDelPeriodo = Ebitdas
                                            .FirstOrDefault(x =>
                                                x.empresa == null &&
                                                x.year == fechaMesCorte.Year &&
                                                x.mes == _mes
                                            );

                                        if (resultadoDeudaEbitda != null && ebitdaDelPeriodo != null && ebitdaDelPeriodo.ebitda != 0)
                                        {
                                            var valor = ((resultadoDeudaEbitda.dxpCortoPlazo + resultadoDeudaEbitda.dxpLargoPlazo / 1000))  / ebitdaDelPeriodo.ebitda;
                                            serieDeudaEbitda.data.Add(valor);
                                        }
                                        else
                                        {
                                            serieDeudaEbitda.data.Add(0);
                                        }
                                    }
                                }

                                gfxDeudaEbitda.series.Add(serieDeudaEbitda);
                            }

                            resultado.Add("gfxDeudaEbitda", gfxDeudaEbitda);
                            #endregion
                            #endregion


                            #region comentado
                            //for (int _mes = fechaMesCorte.Month; _mes >= 1; _mes--)
                            //{
                            //    if (listaEmpresas.Count == 1)
                            //    {
                            //        #region Datos de una empresa
                            //        var empresa = listaEmpresas.First();

                            //        using (var _ctx = new MainContext(empresa))
                            //        {
                            //            for (int _tipoBalance = 1; _tipoBalance <= 2; _tipoBalance++)
                            //            {
                            //                switch ((TipoBalanceEnum)_tipoBalance)
                            //                {
                            //                    case TipoBalanceEnum.ACTIVO:
                            //                        infoBalanceActivo.AddRange(BalancePorEmpresa(empresa, fechaMesCorte, listaCC, listaAC, (TipoBalanceEnum)_tipoBalance));
                            //                        break;
                            //                    case TipoBalanceEnum.PASIVO:
                            //                        infoBalancePasivo.AddRange(BalancePorEmpresa(empresa, fechaMesCorte, listaCC, listaAC, (TipoBalanceEnum)_tipoBalance));
                            //                        break;
                            //                }
                            //            }
                            //        }
                            //        #endregion
                            //    }
                            //    else
                            //    {
                            //        #region Datos consolidados
                            //        for (int _tipoBalance = 1; _tipoBalance <= 2; _tipoBalance++)
                            //        {
                            //            foreach (var empresa in listaEmpresas)
                            //            {
                            //                using (var _ctx = new MainContext(empresa))
                            //                {
                            //                    switch ((TipoBalanceEnum)_tipoBalance)
                            //                    {
                            //                        case TipoBalanceEnum.ACTIVO:
                            //                            infoBalanceActivo.AddRange(BalancePorEmpresa(empresa, fechaMesCorte, listaCC, listaAC, (TipoBalanceEnum)_tipoBalance));
                            //                            break;
                            //                        case TipoBalanceEnum.PASIVO:
                            //                            infoBalancePasivo.AddRange(BalancePorEmpresa(empresa, fechaMesCorte, listaCC, listaAC, (TipoBalanceEnum)_tipoBalance));
                            //                            break;
                            //                    }
                            //                }
                            //            }

                            //            switch ((TipoBalanceEnum)_tipoBalance)
                            //            {
                            //                case TipoBalanceEnum.ACTIVO:
                            //                    infoBalanceActivoConsolidado.AddRange(BalanceConsolidado(listaEmpresas, fechaMesCorte, listaCC, listaAC, (TipoBalanceEnum)(_tipoBalance + 2)));
                            //                    break;
                            //                case TipoBalanceEnum.PASIVO:
                            //                    infoBalancePasivoConsolidado.AddRange(BalanceConsolidado(listaEmpresas, fechaMesCorte, listaCC, listaAC, (TipoBalanceEnum)(_tipoBalance + 2)));
                            //                    break;
                            //            }
                            //        }
                            //        #endregion
                            //    }
                            //}//End ciclo mes

                            //#region GFX RAZON DEL CIRCULANTE
                            //HighchartDTO razonCirculante = new HighchartDTO();
                            //HighchartDTO deudaCapitalNeto = new HighchartDTO();
                            //HighchartDTO pruebaAcido = new HighchartDTO();
                            //HighchartDTO razonEndeudamiento = new HighchartDTO();
                            //xAxisDTO xAxis = new xAxisDTO();
                            //xAxis.crosshair = true;

                            //for (int _mes = 1; _mes <= fechaMesCorte.Month; _mes++)
                            //{
                            //    xAxis.categories.Add(new DateTime(fechaMesCorte.Year, _mes, 1).ToString("MMMM yyyy"));
                            //}

                            //razonCirculante.xAxis = xAxis;
                            //deudaCapitalNeto.xAxis = xAxis;
                            //pruebaAcido.xAxis = xAxis;
                            //razonEndeudamiento.xAxis = xAxis;

                            //for (int empresa = 0; empresa <= listaEmpresas.Count; empresa++)
                            //{
                            //    var serieRazonCirculante = new SerieDTO();
                            //    var serieDeudaCapitalNeto = new SerieDTO();
                            //    var seriePruebaAcido = new SerieDTO();
                            //    var serieEndeudamiento = new SerieDTO();

                            //    if (empresa < listaEmpresas.Count)
                            //    {
                            //        var nombreEmpresa = listaEmpresas[empresa].GetDescription();

                            //        serieRazonCirculante.name = nombreEmpresa;
                            //        serieDeudaCapitalNeto.name = nombreEmpresa;
                            //        seriePruebaAcido.name = nombreEmpresa;
                            //        serieEndeudamiento.name = nombreEmpresa;

                            //        for (int _mes = 1; _mes <= fechaMesCorte.Month; _mes++)
                            //        {
                            //            var activoCirculante = infoBalanceActivo
                            //                .Where(x =>
                            //                    x.empresa == listaEmpresas[empresa] &&
                            //                    x.mes == _mes)
                            //                .Sum(x => x.activoCirculante);
                            //            var pasivoCirculante = infoBalancePasivo
                            //                .Where(x =>
                            //                    x.empresa == listaEmpresas[empresa] &&
                            //                    x.mes == _mes)
                            //                .Sum(x => x.pasivoCirculante);
                            //            var pasivoTotal = infoBalancePasivo
                            //                .Where(x =>
                            //                    x.empresa == listaEmpresas[empresa] &&
                            //                    x.mes == _mes)
                            //                .Sum(x => x.pasivoTotal);
                            //            var capitalContableCirculante = infoBalancePasivo
                            //                .Where(x =>
                            //                    x.empresa == listaEmpresas[empresa] &&
                            //                    x.mes == _mes)
                            //                .Sum(x => x.capitalContableCirculante);
                            //            var inventario = infoBalanceActivo
                            //                .Where(x =>
                            //                    x.empresa == listaEmpresas[empresa] &&
                            //                    x.mes == _mes)
                            //                .Sum(x => x.inventario);
                            //            var activoTotal = infoBalancePasivo
                            //                .Where(x =>
                            //                    x.empresa == listaEmpresas[empresa] &&
                            //                    x.mes == _mes)
                            //                .Sum(x => x.activoTotal);

                            //            serieRazonCirculante.data.Add(activoCirculante / (pasivoCirculante != 0 ? pasivoCirculante : 1));
                            //            serieDeudaCapitalNeto.data.Add(pasivoTotal / (capitalContableCirculante != 0 ? capitalContableCirculante : 1));
                            //            seriePruebaAcido.data.Add((activoCirculante - inventario) / (pasivoCirculante != 0 ? pasivoCirculante : 1));
                            //            serieEndeudamiento.data.Add((pasivoTotal / (activoTotal != 0 ? activoTotal : 1)) * 100);
                            //        }
                            //    }
                            //    else
                            //    {
                            //        var nombreEmpresa = "CONSOLIDADO";

                            //        serieRazonCirculante.name = nombreEmpresa;
                            //        serieDeudaCapitalNeto.name = nombreEmpresa;
                            //        seriePruebaAcido.name = nombreEmpresa;
                            //        serieEndeudamiento.name = nombreEmpresa;

                            //        for (int _mes = 1; _mes <= fechaMesCorte.Month; _mes++)
                            //        {
                            //            var activoCirculante = infoBalanceActivoConsolidado.Where(x => x.mes == _mes).Sum(x => x.activoCirculante);
                            //            var pasivoCirculante = infoBalancePasivoConsolidado.Where(x => x.mes == _mes).Sum(x => x.pasivoCirculante);
                            //            var pasivoTotal = infoBalancePasivoConsolidado.Where(x => x.mes == _mes).Sum(x => x.pasivoTotal);
                            //            var capitalContableCirculante = infoBalancePasivoConsolidado.Where(x => x.mes == _mes).Sum(x => x.capitalContableCirculante);
                            //            var inventario = infoBalanceActivoConsolidado.Where(x => x.mes == _mes).Sum(x => x.inventario);
                            //            var activoTotal = infoBalancePasivoConsolidado.Where(x => x.mes == _mes).Sum(x => x.activoTotal);

                            //            serieRazonCirculante.data.Add(activoCirculante / (pasivoCirculante != 0 ? pasivoCirculante : 1));
                            //            serieDeudaCapitalNeto.data.Add(pasivoTotal / (capitalContableCirculante != 0 ? capitalContableCirculante : 1));
                            //            seriePruebaAcido.data.Add((activoCirculante - inventario) / (pasivoCirculante != 0 ? pasivoCirculante : 1));
                            //            serieEndeudamiento.data.Add((pasivoTotal / (activoTotal != 0 ? activoTotal : 1)) * 100);
                            //        }
                            //    }

                            //    razonCirculante.series.Add(serieRazonCirculante);
                            //    deudaCapitalNeto.series.Add(serieDeudaCapitalNeto);
                            //    pruebaAcido.series.Add(seriePruebaAcido); //Andale, pruebalo!
                            //    razonEndeudamiento.series.Add(serieEndeudamiento);
                            //}

                            //resultado.Add("gfxRazonCirculante", razonCirculante);
                            //resultado.Add("gfxDeudaCapitalNeto", deudaCapitalNeto);
                            //resultado.Add("gfxPruebaAcido", pruebaAcido);
                            //resultado.Add("gfxRazonEndeudamiento", razonEndeudamiento);
                            //#endregion
                            #endregion
                        }
                        #endregion
                        break;
                }

                resultado.Add(SUCCESS, true);
                resultado.Add("data", data);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                LogError(0, 0, "EstadosFinancierosController", "CalcularBalanceGeneral", e, AccionEnum.CONSULTA, 0, new { listaEmpresas = listaEmpresas, fechaMesCorte = fechaMesCorte, listaCC = listaCC });
            }

            return resultado;
        }

        private List<BalanceDTO> BalancePorEmpresa(EmpresaEnum empresa, DateTime fechaCorte, List<string> listaCC, List<string> listaAC, TipoBalanceEnum tipoBalance)
        {
            int year = fechaCorte.Year;
            int mes = fechaCorte.Month;

            decimal tipoCambio = GetTipoCambio(fechaCorte);

            List<BalanceDTO> datosTabla = new List<BalanceDTO>();

            using (var _ctx = new MainContext(empresa))
            {
                var balance = _ctx.tblEF_BalanceTipoBalance.First(x => x.id == (int)tipoBalance && !x.esConsolidado && x.registroActivo);

                foreach (var grupo in balance.grupos.Where(x => x.registroActivo).OrderBy(x => x.orden))
                {
                    var datosGrupo = new List<BalanceDTO>();

                    foreach (var concepto in grupo.conceptos.Where(x => x.registroActivo).OrderBy(x => x.orden))
                    {
                        BalanceDTO datoTabla = new BalanceDTO();
                        datoTabla.concepto = concepto.descripcion;
                        datoTabla.tipoDetalle = concepto.tipoDetalleId;
                        datoTabla.empresa = empresa;
                        datoTabla.mes = mes;

                        if (concepto.esSubtitulo)
                        {
                            datoTabla.renglonSubTitulo = true;
                        }
                        if (concepto.tieneEnlace)
                        {
                            datoTabla.renglonEnlace = true;
                        }

                        var corte = 0M;

                        foreach (var cuenta in concepto.cuentas.Where(x => x.registroActivo))
                        {
                            var filtroSaldo = new FiltroSaldoDTO();
                            filtroSaldo.empresa = empresa;
                            filtroSaldo.year = year;
                            filtroSaldo.mes = mes;
                            filtroSaldo.cta = cuenta.cta;
                            filtroSaldo.scta = cuenta.scta;
                            filtroSaldo.sscta = cuenta.sscta;
                            filtroSaldo.cc = cuenta.cc;
                            filtroSaldo.areaCuenta = cuenta.areaCuenta;
                            filtroSaldo.listaCC = listaCC;
                            filtroSaldo.listaAC = listaAC;

                            var saldosCC = new List<tblEF_SaldoCC>();
                            var clientes = new List<tblEF_MovimientoCliente>();
                            var proveedores = new List<tblEF_MovimientoProveedor>();

                            switch (cuenta.tipoCuentaId)
                            {
                                case TipoCuentaEnum.CUENTA:
                                    #region CALCULAR CUENTA
                                    {
                                        saldosCC = SaldoBalanceUnPeriodo(_ctx, filtroSaldo);

                                        if (cuenta.tipoOperacion == TipoOperacionEnum.suma && !cuenta.invertirSigno)
                                        {
                                            if (!cuenta.esCuentaDolar)
                                            {
                                                corte += saldosCC
                                                    .Sum(x =>
                                                        ((x.cta >= 4000 && x.cta < 6000) ? (decimal?)0 : (decimal?)x.saldoInicial) +
                                                        (decimal?)x.cargosAcumulados +
                                                        (decimal?)x.abonosAcumulados) ?? 0;
                                            }
                                        }
                                        else
                                        {
                                            if (!cuenta.esCuentaDolar)
                                            {
                                                corte -= saldosCC
                                                    .Sum(x =>
                                                        ((x.cta >= 4000 && x.cta < 6000) ? (decimal?)0 : (decimal?)x.saldoInicial) +
                                                        (decimal?)x.cargosAcumulados +
                                                        (decimal?)x.abonosAcumulados) ?? 0;
                                            }
                                        }
                                    }
                                    #endregion
                                    break;
                                case TipoCuentaEnum.CLIENTE:
                                    #region CALCULAR CLIENTE
                                    {
                                        clientes = SaldoClienteUnPeriodo(_ctx, filtroSaldo);

                                        if (cuenta.tipoOperacion == TipoOperacionEnum.suma && !cuenta.invertirSigno)
                                        {
                                            if (!cuenta.esCuentaDolar)
                                            {
                                                corte += clientes.Sum(x => (decimal?)x.total) ?? 0;
                                            }
                                            else
                                            {
                                                corte += (clientes.Sum(x => (decimal?)x.total) ?? 0) * tipoCambio;
                                            }
                                        }
                                        else
                                        {
                                            if (!cuenta.esCuentaDolar)
                                            {
                                                corte -= clientes.Sum(x => (decimal?)x.total) ?? 0;
                                            }
                                            else
                                            {
                                                corte -= (clientes.Sum(x => (decimal?)x.total) ?? 0) * tipoCambio;
                                            }
                                        }
                                    }
                                    #endregion
                                    break;
                                case TipoCuentaEnum.PROVEEDOR:
                                    #region CALCULAR PROVEEDOR
                                    {
                                        proveedores = SaldoProveedorUnPeriodo(_ctx, filtroSaldo);

                                        if (cuenta.tipoOperacion == TipoOperacionEnum.suma && !cuenta.invertirSigno)
                                        {
                                            if (!cuenta.esCuentaDolar)
                                            {
                                                corte += proveedores.Sum(x => (decimal?)x.total) ?? 0;
                                            }
                                            else
                                            {
                                                corte += (proveedores.Sum(x => (decimal?)x.total) ?? 0) * tipoCambio;
                                            }
                                        }
                                        else
                                        {
                                            if (!cuenta.esCuentaDolar)
                                            {
                                                corte -= proveedores.Sum(x => (decimal?)x.total) ?? 0;
                                            }
                                            else
                                            {
                                                corte -= (proveedores.Sum(x => (decimal?)x.total) ?? 0) * tipoCambio;
                                            }
                                        }
                                    }
                                    #endregion
                                    break;
                            }//End switch tipoCuenta
                        }//End ciclo cuenta

                        datoTabla.corte = corte / 1000;

                        if (concepto.tipoIndicador.HasValue)
                        {
                            switch (concepto.tipoIndicador.Value)
                            {
                                case TipoIndicadorEnum.INVENTARIO:
                                    datoTabla.inventario = datoTabla.corte;
                                    break;
                            }
                        }

                        datosGrupo.Add(datoTabla);
                    }//End ciclo conceptos

                    var datoTablaGrupo = new BalanceDTO();
                    datoTablaGrupo.concepto = grupo.descripcion;
                    datoTablaGrupo.tipoDetalle = TipoDetalleEnum.NOMBRE;
                    datoTablaGrupo.renglonGrupo = true;
                    datoTablaGrupo.sumarTotal = grupo.sumarTotal;
                    datoTablaGrupo.empresa = empresa;
                    datoTablaGrupo.mes = mes;

                    if (grupo.esGranTotal)
                    {
                        datoTablaGrupo.corte = datosTabla.Where(x => x.sumarTotal).Sum(x => x.corte);
                    }
                    else
                    {
                        datoTablaGrupo.corte = datosGrupo.Sum(x => x.corte);
                    }

                    if (grupo.tipoIndicador.HasValue)
                    {
                        switch (grupo.tipoIndicador.Value)
                        {
                            case TipoIndicadorEnum.ACTIVO_CIRCULANTE:
                                datoTablaGrupo.activoCirculante = datoTablaGrupo.corte;
                                break;
                            case TipoIndicadorEnum.PASIVO_CIRCULANTE:
                                datoTablaGrupo.pasivoCirculante = datoTablaGrupo.corte;
                                break;
                            case TipoIndicadorEnum.PASIVO_TOTAL:
                                datoTablaGrupo.pasivoTotal = datoTablaGrupo.corte;
                                break;
                            case TipoIndicadorEnum.CAPITAL_CONTABLE_CIRCULANTE:
                                datoTablaGrupo.capitalContableCirculante = datoTablaGrupo.corte;
                                break;
                            case TipoIndicadorEnum.ACTIVO_TOTAL:
                                datoTablaGrupo.activoTotal = datoTablaGrupo.corte;
                                break;
                        }
                    }

                    datosTabla.AddRange(datosGrupo);
                    datosTabla.Add(datoTablaGrupo);
                }//End ciclo grupos

                return datosTabla;
            }//End context
        }

        private List<BalanceDTO> BalanceConsolidado(List<EmpresaEnum> empresas, DateTime fechaCorte, List<string> listaCC, List<string> listaAC, TipoBalanceEnum tipoBalance)
        {
            int year = fechaCorte.Year;
            int mes = fechaCorte.Month;

            decimal tipoCambio = GetTipoCambio(fechaCorte);

            List<BalanceDTO> datosTabla = new List<BalanceDTO>();

            using (var _ctxCplan = new MainContext(EmpresaEnum.Construplan))
            {
                var balance = _ctxCplan.tblEF_BalanceTipoBalance.First(x => x.id == (int)tipoBalance && x.esConsolidado && x.registroActivo);

                var gruposGenerales = _ctxCplan.tblEF_BalanceGrupoConsolidado
                    .Where(x => x.registroActivo && x.tipoBalanceId == balance.id).OrderBy(x => x.orden).ToList();

                foreach (var grupoGeneral in gruposGenerales)
                {
                    var datosGrupo = new List<BalanceDTO>();

                    foreach (var conceptoGeneral in grupoGeneral.conceptos.Where(x => x.registroActivo).OrderBy(x => x.orden))
                    {
                        var datoTabla = new BalanceDTO();
                        datoTabla.concepto = conceptoGeneral.descripcion;
                        datoTabla.tipoDetalle = (TipoDetalleEnum)conceptoGeneral.tipoDetalleId;
                        datoTabla.empresa = null;
                        datoTabla.mes = mes;
                        if (conceptoGeneral.esSubtitulo)
                        {
                            datoTabla.renglonSubTitulo = true;
                        }
                        if (conceptoGeneral.tieneEnlace)
                        {
                            datoTabla.renglonEnlace = true;
                        }

                        var corte = 0M;

                        foreach (var relacionConceptoEmpresa in conceptoGeneral.conceptosEmpresa.Where(x => x.registroActivo))
                        {
                            foreach (var empresa in empresas)
                            {
                                using (var _ctxEmpresa = new MainContext(empresa))
                                {
                                    tblEF_BalanceConcepto conceptoEmpresa = null;

                                    switch (empresa)
                                    {
                                        case EmpresaEnum.Construplan:
                                            conceptoEmpresa = _ctxEmpresa.tblEF_BalanceConcepto
                                                .FirstOrDefault(x => x.id == relacionConceptoEmpresa.conceptoConstruplanId);
                                            break;
                                        case EmpresaEnum.Arrendadora:
                                            conceptoEmpresa = _ctxEmpresa.tblEF_BalanceConcepto
                                                .FirstOrDefault(x => x.id == relacionConceptoEmpresa.conceptoArrendadoraId);
                                            break;
                                        case EmpresaEnum.EICI:
                                            conceptoEmpresa = _ctxEmpresa.tblEF_BalanceConcepto
                                                .FirstOrDefault(x => x.id == relacionConceptoEmpresa.conceptoEiciId);
                                            break;
                                        case EmpresaEnum.Integradora:
                                            conceptoEmpresa = _ctxEmpresa.tblEF_BalanceConcepto
                                                .FirstOrDefault(x => x.id == relacionConceptoEmpresa.conceptoIntegradoraId);
                                            break;
                                    }//End switch empresa

                                    if (conceptoEmpresa != null)
                                    {
                                        foreach (var cuenta in conceptoEmpresa.cuentas.Where(x => x.registroActivo))
                                        {
                                            var filtroSaldo = new FiltroSaldoDTO();
                                            filtroSaldo.empresa = empresa;
                                            filtroSaldo.year = year;
                                            filtroSaldo.mes = mes;
                                            filtroSaldo.cta = cuenta.cta;
                                            filtroSaldo.scta = cuenta.scta;
                                            filtroSaldo.sscta = cuenta.sscta;
                                            filtroSaldo.cc = cuenta.cc;
                                            filtroSaldo.listaCC = listaCC;
                                            filtroSaldo.listaAC = listaAC;
                                            filtroSaldo.areaCuenta = cuenta.areaCuenta;

                                            var saldosCC = new List<tblEF_SaldoCC>();
                                            var clientes = new List<tblEF_MovimientoCliente>();
                                            var proveedores = new List<tblEF_MovimientoProveedor>();

                                            switch (cuenta.tipoCuentaId)
                                            {
                                                case TipoCuentaEnum.CUENTA:
                                                    #region CUENTA
                                                    {
                                                        saldosCC = SaldoBalanceUnPeriodo(_ctxEmpresa, filtroSaldo);

                                                        if (cuenta.tipoOperacion == TipoOperacionEnum.suma && !cuenta.invertirSigno)
                                                        {
                                                            if (!cuenta.esCuentaDolar)
                                                            {
                                                                corte += saldosCC
                                                                    .Sum(x => ((x.cta >= 4000 && x.cta < 6000) ? (decimal?)0 : (decimal?)x.saldoInicial) + (decimal?)x.cargosAcumulados + (decimal?)x.abonosAcumulados) ?? 0;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (!cuenta.esCuentaDolar)
                                                            {
                                                                corte -= saldosCC
                                                                    .Sum(x => ((x.cta >= 4000 && x.cta < 6000) ? (decimal?)0 : (decimal?)x.saldoInicial) + (decimal?)x.cargosAcumulados + (decimal?)x.abonosAcumulados) ?? 0;
                                                            }
                                                        }
                                                    }
                                                    #endregion
                                                    break;
                                                case TipoCuentaEnum.CLIENTE:
                                                    #region CLIENTE
                                                    {
                                                        clientes = SaldoCliente(_ctxEmpresa, filtroSaldo);

                                                        if (cuenta.tipoOperacion == TipoOperacionEnum.suma && !cuenta.invertirSigno)
                                                        {
                                                            if (!cuenta.esCuentaDolar)
                                                            {
                                                                corte += clientes.Sum(x => (decimal?)x.total) ?? 0;
                                                            }
                                                            else
                                                            {
                                                                corte += (clientes.Sum(x => (decimal?)x.total) ?? 0) * tipoCambio;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (!cuenta.esCuentaDolar)
                                                            {
                                                                corte -= clientes.Sum(x => (decimal?)x.total) ?? 0;
                                                            }
                                                            else
                                                            {
                                                                corte -= (clientes.Sum(x => (decimal?)x.total) ?? 0) * tipoCambio;
                                                            }
                                                        }
                                                    }
                                                    #endregion
                                                    break;
                                                case TipoCuentaEnum.PROVEEDOR:
                                                    #region PROVEEDOR
                                                    {
                                                        proveedores = SaldoProveedor(_ctxEmpresa, filtroSaldo);

                                                        if (cuenta.tipoOperacion == TipoOperacionEnum.suma && !cuenta.invertirSigno)
                                                        {
                                                            if (!cuenta.esCuentaDolar)
                                                            {
                                                                corte += proveedores.Sum(x => (decimal?)x.total) ?? 0;
                                                            }
                                                            else
                                                            {
                                                                corte += (proveedores.Sum(x => (decimal?)x.total) ?? 0) * tipoCambio;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (!cuenta.esCuentaDolar)
                                                            {
                                                                corte -= proveedores.Sum(x => (decimal?)x.total) ?? 0;
                                                            }
                                                            else
                                                            {
                                                                corte -= (proveedores.Sum(x => (decimal?)x.total) ?? 0) * tipoCambio;
                                                            }
                                                        }
                                                    }
                                                    #endregion
                                                    break;
                                            }//End switch tipoCuenta
                                        }//End ciclo cuenta
                                    }//End verifica si la empresa tiene el información del concepto
                                }//End contexto empresa
                            }//End ciclo empresas
                        }//End ciclo relacion concepto empresa

                        datoTabla.corte = corte / 1000;

                        if (conceptoGeneral.tipoIndicador.HasValue)
                        {
                            switch (conceptoGeneral.tipoIndicador.Value)
                            {
                                case TipoIndicadorEnum.INVENTARIO:
                                    datoTabla.inventario = datoTabla.corte;
                                    break;
                            }
                        }

                        datosGrupo.Add(datoTabla);
                    }//End ciclo conceptoGeneral

                    var datoTablaGrupo = new BalanceDTO();
                    datoTablaGrupo.concepto = grupoGeneral.descripcion;
                    datoTablaGrupo.tipoDetalle = TipoDetalleEnum.NOMBRE;
                    datoTablaGrupo.renglonGrupo = true;
                    datoTablaGrupo.sumarTotal = grupoGeneral.sumarTotal;
                    datoTablaGrupo.empresa = null;
                    datoTablaGrupo.mes = mes;

                    if (grupoGeneral.esGranTotal)
                    {
                        datoTablaGrupo.corte = datosTabla.Where(x => x.sumarTotal).Sum(x => x.corte);
                    }
                    else
                    {
                        datoTablaGrupo.corte = datosGrupo.Sum(x => x.corte);
                    }

                    if (grupoGeneral.tipoIndicador.HasValue)
                    {
                        switch (grupoGeneral.tipoIndicador.Value)
                        {
                            case TipoIndicadorEnum.ACTIVO_CIRCULANTE:
                                datoTablaGrupo.activoCirculante = datoTablaGrupo.corte;
                                break;
                            case TipoIndicadorEnum.PASIVO_CIRCULANTE:
                                datoTablaGrupo.pasivoCirculante = datoTablaGrupo.corte;
                                break;
                            case TipoIndicadorEnum.PASIVO_TOTAL:
                                datoTablaGrupo.pasivoTotal = datoTablaGrupo.corte;
                                break;
                            case TipoIndicadorEnum.CAPITAL_CONTABLE_CIRCULANTE:
                                datoTablaGrupo.capitalContableCirculante = datoTablaGrupo.corte;
                                break;
                            case TipoIndicadorEnum.ACTIVO_TOTAL:
                                datoTablaGrupo.activoTotal = datoTablaGrupo.corte;
                                break;
                        }
                    }

                    datosTabla.AddRange(datosGrupo);
                    datosTabla.Add(datoTablaGrupo);
                }//End ciclo grupoGeneral
            }//End context cplan

            return datosTabla;
        }

        private List<tblEF_MovimientoCliente> SaldoCliente(MainContext ctx, FiltroSaldoDTO filtro)
        {
            var saldosCliente = new List<tblEF_MovimientoCliente>();

            var mesAnterior = filtro.mes - 1;
            var yearAnterior = filtro.year;

            if (mesAnterior <= 0)
            {
                yearAnterior -= 1;
                mesAnterior = 12;
            }

            switch (filtro.empresa)
            {
                case EmpresaEnum.Integradora:
                case EmpresaEnum.EICI:
                case EmpresaEnum.Construplan:
                    saldosCliente = ctx.tblEF_MovimientoCliente
                        .Where(x =>
                            (
                                (x.corte.anio == yearAnterior && x.corte.mes == mesAnterior) ||
                                (x.corte.anio == filtro.year && x.corte.mes == filtro.mes)
                            ) &&
                            x.corte.estatus &&
                            x.numeroCliente == filtro.cta &&
                            (filtro.listaCC.Count > 0 ? filtro.listaCC.Contains(x.cc) : true) &&
                            (string.IsNullOrEmpty(filtro.cc) ? true : x.cc == filtro.cc) &&
                            x.estatus
                        ).ToList();
                    break;
                case EmpresaEnum.Arrendadora:
                    saldosCliente = ctx.tblEF_MovimientoCliente
                        .Where(x =>
                            (
                                (x.corte.anio == yearAnterior && x.corte.mes == mesAnterior) ||
                                (x.corte.anio == filtro.year && x.corte.mes == filtro.mes)
                            ) &&
                            x.corte.estatus &&
                            x.numeroCliente == filtro.cta &&
                            (filtro.listaAC.Count > 0 ? filtro.listaAC.Contains(x.areaCuenta) : true) &&
                            (string.IsNullOrEmpty(filtro.areaCuenta) ? true : x.areaCuenta == filtro.areaCuenta) &&
                            x.estatus
                        ).ToList();
                    break;
            }

            return saldosCliente;
        }

        public List<tblEF_MovimientoCliente> SaldoClienteUnPeriodo(MainContext ctx, FiltroSaldoDTO filtro)
        {
            var saldosCliente = new List<tblEF_MovimientoCliente>();

            switch (filtro.empresa)
            {
                case EmpresaEnum.EICI:
                case EmpresaEnum.Integradora:
                case EmpresaEnum.Construplan:
                    saldosCliente = ctx.tblEF_MovimientoCliente
                        .Where(x =>
                            x.corte.anio == filtro.year &&
                            x.corte.mes == filtro.mes &&
                            x.corte.estatus &&
                            x.numeroCliente == filtro.cta &&
                            (filtro.listaCC.Count > 0 ? filtro.listaCC.Contains(x.cc) : true) &&
                            (string.IsNullOrEmpty(filtro.cc) ? true : x.cc == filtro.cc) &&
                            x.estatus
                        ).ToList();
                    break;
                case EmpresaEnum.Arrendadora:
                    saldosCliente = ctx.tblEF_MovimientoCliente
                        .Where(x =>
                            x.corte.anio == filtro.year &&
                            x.corte.mes == filtro.mes &&
                            x.corte.estatus &&
                            x.numeroCliente == filtro.cta &&
                            (filtro.listaAC.Count > 0 ? filtro.listaAC.Contains(x.areaCuenta) : true) &&
                            (string.IsNullOrEmpty(filtro.areaCuenta) ? true : x.areaCuenta == filtro.areaCuenta) &&
                            x.estatus
                        ).ToList();
                    break;
            }

            return saldosCliente;
        }

        public List<tblEF_Indicadores> SaldoIndicadores(MainContext ctx, FiltroSaldoDTO filtro)
        {
            var saldoIndicador = new List<tblEF_Indicadores>();

            switch (filtro.empresa)
            {
                case EmpresaEnum.EICI:
                case EmpresaEnum.Integradora:
                case EmpresaEnum.Construplan:
                    saldoIndicador = ctx.tblEF_Indicadores
                        .Where(x =>
                            x.corte.anio == filtro.year &&
                            x.corte.mes == filtro.mes &&
                            x.corte.estatus &&
                            (filtro.listaCC.Count > 0 ? filtro.listaCC.Contains(x.cc) : true) &&
                            x.registroActivo
                        ).ToList();
                    break;
                case EmpresaEnum.Arrendadora:
                    saldoIndicador = ctx.tblEF_Indicadores
                        .Where(x =>
                            x.corte.anio == filtro.year &&
                            x.corte.mes == filtro.mes &&
                            x.corte.estatus &&
                            (filtro.listaAC.Count > 0 ? filtro.listaAC.Contains(x.areaCuenta) : true) &&
                            x.registroActivo
                        ).ToList();
                    break;
            }

            return saldoIndicador;
        }

        public List<tblEF_MovimientoCliente> SaldoClientesDolares(MainContext ctx, FiltroSaldoDTO filtro)
        {
            var saldosClientes = new List<tblEF_MovimientoCliente>();

            switch (filtro.empresa)
            {
                case EmpresaEnum.EICI:
                case EmpresaEnum.Integradora:
                case EmpresaEnum.Construplan:
                    saldosClientes = ctx.tblEF_MovimientoCliente
                        .Where(x =>
                            x.corte.anio == filtro.year &&
                            x.corte.mes == filtro.mes &&
                            x.corte.estatus &&
                            x.numeroCliente >= 9000 &&
                            (filtro.listaCC.Count > 0 ? filtro.listaCC.Contains(x.cc) : true) &&
                            (string.IsNullOrEmpty(filtro.cc) ? true : x.cc == filtro.cc) &&
                            x.estatus
                        ).ToList();
                    break;
                case EmpresaEnum.Arrendadora:
                    saldosClientes = ctx.tblEF_MovimientoCliente
                        .Where(x =>
                            x.corte.anio == filtro.year &&
                            x.corte.mes == filtro.mes &&
                            x.corte.estatus &&
                            x.numeroCliente >= 9000 &&
                            (filtro.listaAC.Count > 0 ? filtro.listaAC.Contains(x.areaCuenta) : true) &&
                            (string.IsNullOrEmpty(filtro.areaCuenta) ? true : x.areaCuenta == filtro.areaCuenta) &&
                            x.estatus
                        ).ToList();
                    break;
            }

            return saldosClientes;
        }

        public List<tblEF_MovimientoProveedor> SaldoProveedoresDolares(MainContext ctx, FiltroSaldoDTO filtro)
        {
            var saldosProveedores = new List<tblEF_MovimientoProveedor>();

            switch (filtro.empresa)
            {
                case EmpresaEnum.EICI:
                case EmpresaEnum.Integradora:
                case EmpresaEnum.Construplan:
                    saldosProveedores = ctx.tblEF_MovimientoProveedor
                        .Where(x =>
                            x.corte.anio == filtro.year &&
                            x.corte.mes == filtro.mes &&
                            x.corte.estatus &&
                            x.numeroProveedor >= 9000 &&
                            (filtro.listaCC.Count > 0 ? filtro.listaCC.Contains(x.cc) : true) &&
                            (string.IsNullOrEmpty(filtro.cc) ? true : x.cc == filtro.cc) &&
                            x.estatus
                        ).ToList();
                    break;
                case EmpresaEnum.Arrendadora:
                    saldosProveedores = ctx.tblEF_MovimientoProveedor
                        .Where(x =>
                            x.corte.anio == filtro.year &&
                            x.corte.mes == filtro.mes &&
                            x.corte.estatus &&
                            x.numeroProveedor >= 9000 &&
                            (filtro.listaAC.Count > 0 ? filtro.listaAC.Contains(x.areaCuenta) : true) &&
                            (string.IsNullOrEmpty(filtro.areaCuenta) ? true : x.areaCuenta == filtro.areaCuenta) &&
                            x.estatus
                        ).ToList();
                    break;
            }

            return saldosProveedores;
        }

        private List<tblEF_MovimientoProveedor> SaldoProveedor(MainContext ctx, FiltroSaldoDTO filtro)
        {
            var saldosProveedores = new List<tblEF_MovimientoProveedor>();

            var mesAnterior = filtro.mes - 1;
            var yearAnterior = filtro.year;

            if (mesAnterior <= 0)
            {
                yearAnterior -= 1;
                mesAnterior = 12;
            }

            switch (filtro.empresa)
            {
                case EmpresaEnum.Integradora:
                case EmpresaEnum.EICI:
                case EmpresaEnum.Construplan:
                    saldosProveedores = ctx.tblEF_MovimientoProveedor
                        .Where(x =>
                            (
                                (x.corte.anio == yearAnterior && x.corte.mes == mesAnterior) ||
                                (x.corte.anio == filtro.year && x.corte.mes == filtro.mes)
                            ) &&
                            x.corte.estatus &&
                            x.numeroProveedor == filtro.cta &&
                            (filtro.listaCC.Count > 0 ? filtro.listaCC.Contains(x.cc) : true) &&
                            (string.IsNullOrEmpty(filtro.cc) ? true : x.cc == filtro.cc) &&
                            x.estatus
                        ).ToList();
                    break;
                case EmpresaEnum.Arrendadora:
                    saldosProveedores = ctx.tblEF_MovimientoProveedor
                        .Where(x =>
                            (
                                (x.corte.anio == yearAnterior && x.corte.mes == mesAnterior) ||
                                (x.corte.anio == filtro.year && x.corte.mes == filtro.mes)
                            ) &&
                            x.corte.estatus &&
                            x.numeroProveedor == filtro.cta &&
                            (filtro.listaAC.Count > 0 ? filtro.listaAC.Contains(x.areaCuenta) : true) &&
                            (string.IsNullOrEmpty(filtro.areaCuenta) ? true : x.areaCuenta == filtro.areaCuenta) &&
                            x.estatus
                        ).ToList();
                    break;
            }

            return saldosProveedores;
        }

        private List<tblEF_MovimientoProveedor> SaldoProveedorUnPeriodo(MainContext ctx, FiltroSaldoDTO filtro)
        {
            var saldosProveedores = new List<tblEF_MovimientoProveedor>();

            switch (filtro.empresa)
            {
                case EmpresaEnum.Integradora:
                case EmpresaEnum.EICI:
                case EmpresaEnum.Construplan:
                    saldosProveedores = ctx.tblEF_MovimientoProveedor
                        .Where(x =>
                            x.corte.anio == filtro.year &&
                            x.corte.mes == filtro.mes &&
                            x.corte.estatus &&
                            x.numeroProveedor == filtro.cta &&
                            (filtro.listaCC.Count > 0 ? filtro.listaCC.Contains(x.cc) : true) &&
                            (string.IsNullOrEmpty(filtro.cc) ? true : x.cc == filtro.cc) &&
                            x.estatus
                        ).ToList();
                    break;
                case EmpresaEnum.Arrendadora:
                    saldosProveedores = ctx.tblEF_MovimientoProveedor
                        .Where(x =>
                            x.corte.anio == filtro.year &&
                            x.corte.mes == filtro.mes &&
                            x.corte.estatus &&
                            x.numeroProveedor == filtro.cta &&
                            (filtro.listaAC.Count > 0 ? filtro.listaAC.Contains(x.areaCuenta) : true) &&
                            (string.IsNullOrEmpty(filtro.areaCuenta) ? true : x.areaCuenta == filtro.areaCuenta) &&
                            x.estatus
                        ).ToList();
                    break;
            }

            return saldosProveedores;
        }

        private Dictionary<string, decimal> CalcularDxpCirculanteLP(MainContext _ctx, DateTime fechaCorte, EmpresaEnum empresa, decimal tipoCambio, decimal tipoCambioAnterior)
        {
            var resultadoDxPLP = new Dictionary<string, decimal>();

            var institucionesDxP = _ctx.tblAF_DxP_Instituciones.Where(x => !x.esPQ && x.Estatus).Select(x => x.Id).ToList();

            var pesos = 0M;
            var dolares = 0M;
            var pesosAnterior = 0M;
            var dolaresAnterior = 0M;

            var contratos = _ctx.tblAF_DxP_Contratos
                .Where(x =>
                    x.fechaFirma <= fechaCorte &&
                    !x.Terminado &&
                    !x.arrendamientoPuro &&
                    x.Estatus).ToList();

            foreach (var contrato in contratos)
            {
                switch ((TipoMonedaEnum)contrato.monedaContrato)
                {
                    case TipoMonedaEnum.MN:
                        pesos += contrato.detalles
                            .Where(x =>
                                x.FechaVencimiento.Year == (fechaCorte.Year + 1) &&
                                x.FechaVencimiento.Month <= fechaCorte.Month &&
                                x.Estatus)
                            .Sum(x => (decimal?)x.Importe) ?? 0;
                        pesosAnterior += contrato.detalles
                            .Where(x =>
                                x.FechaVencimiento.Year == (fechaCorte.Year + 1) &&
                                (fechaCorte.Month - 1 <= 0 ? true : x.FechaVencimiento.Month <= (fechaCorte.Month - 1)) &&
                                x.Estatus)
                            .Sum(x => (decimal?)x.Importe) ?? 0;
                        break;
                    case TipoMonedaEnum.USD:
                        dolares += contrato.detalles
                            .Where(x =>
                                x.FechaVencimiento.Year == (fechaCorte.Year + 1) &&
                                x.FechaVencimiento.Month <= fechaCorte.Month &&
                                x.Estatus)
                            .Sum(x => (decimal?)x.Importe) ?? 0;
                        dolaresAnterior += contrato.detalles
                            .Where(x =>
                                x.FechaVencimiento.Year == (fechaCorte.Year + 1) &&
                                (fechaCorte.Month - 1 <= 0 ? true : x.FechaVencimiento.Month <= (fechaCorte.Month - 1)) &&
                                x.Estatus)
                            .Sum(x => (decimal?)x.Importe) ?? 0;
                        break;
                }
            }

            pesos += (dolares * tipoCambio);
            pesosAnterior += (dolaresAnterior * tipoCambioAnterior);

            resultadoDxPLP.Add("pesos", pesos);
            resultadoDxPLP.Add("dolares", dolares);
            resultadoDxPLP.Add("pesosAnterior", pesosAnterior);
            resultadoDxPLP.Add("dolaresAnterior", dolaresAnterior);

            return resultadoDxPLP;
        }

        private decimal GetTipoCambio(DateTime fecha)
        {
            var query_tipo_cambio = new OdbcConsultaDTO();
            query_tipo_cambio.consulta = string.Format
                (
                    @"SELECT
                        tipo_cambio
                    FROM
                        tipo_cambio
                    WHERE
                        fecha BETWEEN ? AND ?"
                );
            query_tipo_cambio.parametros.Add(new OdbcParameterDTO
            {
                nombre = "fecha",
                tipo = System.Data.Odbc.OdbcType.NVarChar,
                valor = fecha.ToString("yyyyMMdd")
            });
            query_tipo_cambio.parametros.Add(new OdbcParameterDTO
            {
                nombre = "fecha",
                tipo = System.Data.Odbc.OdbcType.NVarChar,
                valor = fecha.ToString("yyyyMMdd")
            });

            var tipoCambio = _contextEnkontrol.Select<decimal>(vSesiones.sesionAmbienteEnkontrolAdm, query_tipo_cambio).FirstOrDefault();

            if (tipoCambio == null)
            {
                throw new Exception("No se ha capturado el tipo de cambio de la fecha " + fecha.ToString("dd/MM/yyyy"));
            }

            return tipoCambio;
        }
        #endregion
    }
}
 