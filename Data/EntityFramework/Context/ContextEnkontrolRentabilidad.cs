using Core.DTO;
using Core.DTO.Utils.Data;
using Core.Enum.Multiempresa;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Data.EntityFramework.Context;
using Core.Entity.Maquinaria.Rentabilidad;
using Core.DTO.Maquinaria.Reporte.Rentabilidad;
using Core.DTO.Maquinaria.Rentabilidad;

namespace Data.EntityFramework.Context
{
    public class _contextEnkontrolRentabilidad : _contextEnkontrol
    {
        /// <summary>
        /// Executa la consula a Enkontrol Operativo
        /// </summary>
        /// <typeparam name="T">Tipo de consula</typeparam>
        /// <param name="ek">Enum de ambiente</param>
        /// <param name="odbc">DTO de consulta</param>
        /// <returns>Resultado tipado</returns>
        public static List<tblM_KBCorteDet> SelectCorte(EnkontrolEnum ek, OdbcConsultaDTO odbc, List<ConciliacionKubrixDTO> conciliaciones)
        {
            var empresa = 2;
            if (ek == EnkontrolEnum.ArrenProd) { empresa = 2; }
            else empresa = 1;
            List<tblM_KBCorteDet> data = new List<tblM_KBCorteDet>();
            using (var con = conectar(ek))
            {
                //foreach (var odbc in lstOdbc)
                //{
                    using (var cmd = new OdbcCommand(odbc.consulta, con))
                    {
                        var parameters = cmd.Parameters;
                        odbc.parametros.ForEach(p =>
                        {
                            parameters.Add("@" + p.nombre, p.tipo).Value = p.valor;
                        });
                        data.AddRange(getReaderCorte(cmd, empresa, conciliaciones));
                    }
                //}
            }
            return data;
        }
        /// <summary>
        /// Lee los resultados de OdbcCommand
        /// </summary>
        /// <typeparam name="T">Tipo de consula</typeparam>
        /// <param name="cmd">Comando a leer</param>
        /// <returns>Resultado tipado</returns>
        public static List<tblM_KBCorteDet> getReaderCorte(OdbcCommand cmd, int empresa, List<ConciliacionKubrixDTO> conciliaciones)
        {
            cmd.CommandTimeout = 6000;
            using (var dt = cmd.ExecuteReader())
            {
                var dtEntity = new DataTable();
                dtEntity.Columns.Add("poliza", System.Type.GetType("System.String"));
                dtEntity.Columns.Add("cuenta", System.Type.GetType("System.String"));
                dtEntity.Columns.Add("concepto", System.Type.GetType("System.String"));
                dtEntity.Columns.Add("monto", System.Type.GetType("System.Decimal"));
                dtEntity.Columns.Add("cc", System.Type.GetType("System.String"));
                dtEntity.Columns.Add("areaCuenta", System.Type.GetType("System.String"));
                dtEntity.Columns.Add("fechapol", System.Type.GetType("System.DateTime"));
                dtEntity.Columns.Add("referencia", System.Type.GetType("System.String"));
                dtEntity.Columns.Add("linea", System.Type.GetType("System.Int32"));
                dtEntity.Load(dt);
                
                var auxdata = dtEntity.AsEnumerable();

                var EncConciliacionesIDs = conciliaciones.Select(x => x.factura).ToList();

                if (empresa == 2 || empresa == 12)
                {
                    var data = auxdata.Select(dataRow =>
                    {
                        var auxFacturaString = string.Format(@"""{0}""", (string)dataRow["referencia"]);
                        var caratula = ((string)dataRow["cuenta"]).Contains("4000-") ? (conciliaciones.FirstOrDefault(y => y.factura.Contains(auxFacturaString) && y.estatus == 1) ?? null) : null;
                        var auxReferencia = ((string)dataRow["cuenta"]).Contains("4000-") ? (caratula == null ? "SIN CONCILIACIÓN" : "CONCILIACION " + caratula.cc + " " + caratula.fechaInicio.ToString("dd/MM/yyyy") + " - " + caratula.fechaFin.ToString("dd/MM/yyyy")) : (string)dataRow["referencia"];
                        return new tblM_KBCorteDet
                        {
                            poliza = (string)dataRow["poliza"],
                            cuenta = (string)dataRow["cuenta"],
                            concepto = (string)dataRow["concepto"],
                            monto = (decimal)dataRow["monto"],
                            cc = (string)dataRow["cc"],
                            areaCuenta = (string)dataRow["areaCuenta"],
                            fechapol = (DateTime)dataRow["fechapol"],
                            referencia = auxReferencia,
                            linea = (int)dataRow["linea"],
                        };
                    }).ToList();
                    return data;
                }
                else 
                {
                    var data = auxdata.Select(dataRow =>
                    {
                        return new tblM_KBCorteDet
                        {
                            poliza = (string)dataRow["poliza"],
                            cuenta = (string)dataRow["cuenta"],
                            concepto = (string)dataRow["concepto"],
                            monto = (decimal)dataRow["monto"],
                            cc = (string)dataRow["cc"],
                            areaCuenta = (string)dataRow["areaCuenta"],
                            fechapol = (DateTime)dataRow["fechapol"],
                            referencia = (string)dataRow["referencia"],
                            linea = (int)dataRow["linea"],
                        };
                    }).ToList();
                    return data;
                }                
            }
        }

        public static List<RentabilidadDTO> SelectKubrix(EnkontrolAmbienteEnum ek, OdbcConsultaDTO odbc)
        {
            using (var con = conectar(ek))
            using (var cmd = new OdbcCommand(odbc.consulta, con))
            {
                var parameters = cmd.Parameters;
                odbc.parametros.ForEach(p =>
                {
                    parameters.Add("@" + p.nombre, p.tipo).Value = p.valor;
                });
                return getReaderKubrix(cmd, 2);
            }
        }

        public static List<RentabilidadDTO> SelectKubrix(EnkontrolEnum ek, OdbcConsultaDTO odbc)
        {
            var empresa = 2;
            if (ek == EnkontrolEnum.ArrenProd) { empresa = 2; }
            else empresa = 1;
            using (var con = conectar(ek))
            using (var cmd = new OdbcCommand(odbc.consulta, con))
            {
                var parameters = cmd.Parameters;
                odbc.parametros.ForEach(p =>
                {
                    parameters.Add("@" + p.nombre, p.tipo).Value = p.valor;
                });
                return getReaderKubrix(cmd, empresa);
            }
        }

        /// <summary>
        /// Lee los resultados de OdbcCommand
        /// </summary>
        /// <typeparam name="T">Tipo de consula</typeparam>
        /// <param name="cmd">Comando a leer</param>
        /// <returns>Resultado tipado</returns>
        public static List<RentabilidadDTO> getReaderKubrix(OdbcCommand cmd, int empresa)
        {
            cmd.CommandTimeout = 6000;
            using (var dt = cmd.ExecuteReader())
            {
                var dsEntity = new DataSet();
                dsEntity.Tables.Add("");
                dsEntity.Tables[0].Load(dt);
                return dsEntity.Tables[0].AsEnumerable().Select(dataRow => new RentabilidadDTO
                {
                    noEco = empresa == 2 ? dataRow.Field<string>("noEco") : (Decimal.ToInt32(dataRow.Field<decimal>("cta")) == 5000 ? dataRow.Field<string>("referencia") : "N/A"),
                    cta = Decimal.ToInt32(dataRow.Field<decimal>("cta")),
                    tipoInsumo = dataRow.Field<string>("tipoInsumo"),
                    tipoInsumo_Desc = dataRow.Field<string>("tipoInsumo_Desc"),
                    grupoInsumo = dataRow.Field<string>("grupoInsumo"),
                    grupoInsumo_Desc = dataRow.Field<string>("grupoInsumo_Desc"),
                    insumo = Decimal.ToInt32(dataRow.Field<decimal>("insumo")),
                    insumo_Desc = dataRow.Field<string>("insumo_Desc"),
                    tipo_mov = Decimal.ToInt32(dataRow.Field<decimal>("tipo_mov")),
                    importe = dataRow.Field<decimal>("importe"),
                    fecha = dataRow.Field<DateTime>("fecha"),
                    poliza = dataRow.Field<string>("poliza"),
                    referencia = dataRow.Field<string>("referencia"),
                    areaCuenta = empresa == 2 ? dataRow.Field<string>("areaCuenta") : dataRow.Field<string>("noEco"),
                    empresa = empresa,
                    linea = Decimal.ToInt32(dataRow.Field<decimal>("linea")),
                }).ToList();
            }
        }

        public static List<BalanzaKubrixDTO> SelectBalanza(EnkontrolEnum ek, OdbcConsultaDTO odbc)
        {
            using (var con = conectar(ek))
            using (var cmd = new OdbcCommand(odbc.consulta, con))
            {
                var parameters = cmd.Parameters;
                odbc.parametros.ForEach(p =>
                {
                    parameters.Add("@" + p.nombre, p.tipo).Value = p.valor;
                });
                return getReaderBalanza(cmd);
            }
        }
        /// <summary>
        /// Lee los resultados de OdbcCommand
        /// </summary>
        /// <typeparam name="T">Tipo de consula</typeparam>
        /// <param name="cmd">Comando a leer</param>
        /// <returns>Resultado tipado</returns>
        public static List<BalanzaKubrixDTO> getReaderBalanza(OdbcCommand cmd)
        {
            cmd.CommandTimeout = 6000;
            using (var dt = cmd.ExecuteReader())
            {
                var dsEntity = new DataSet();
                dsEntity.Tables.Add("");
                dsEntity.Tables[0].Load(dt);
                return dsEntity.Tables[0].AsEnumerable().Select(dataRow => new BalanzaKubrixDTO
                {
                    cta = Decimal.ToInt32(dataRow.Field<decimal>("cta")),
                    scta = Decimal.ToInt32(dataRow.Field<decimal>("scta")),
                    sscta = Decimal.ToInt32(dataRow.Field<decimal>("sscta")),
                    abonos = dataRow.Field<decimal>("abonos"),
                    cargos = dataRow.Field<decimal>("cargos"),
                    saldoActual = dataRow.Field<decimal>("saldoActual"),
                    saldoInicial = 0
                }).ToList();
            }
        }


        public static List<SaldosKubrixDTO> SelectSaldos(EnkontrolEnum ek, OdbcConsultaDTO odbc)
        {
            var empresa = 2;
            if (ek == EnkontrolEnum.ArrenProd) { empresa = 2; }
            else empresa = 1;
            List<SaldosKubrixDTO> data = new List<SaldosKubrixDTO>();
            using (var con = conectar(ek))
            {
                using (var cmd = new OdbcCommand(odbc.consulta, con))
                {
                    var parameters = cmd.Parameters;
                    odbc.parametros.ForEach(p =>
                    {
                        parameters.Add("@" + p.nombre, p.tipo).Value = p.valor;
                    });
                    data.AddRange(getReaderSaldos(cmd, empresa));
                }
            }
            return data;
        }

        public static List<SaldosKubrixDTO> getReaderSaldos(OdbcCommand cmd, int empresa)
        {
            cmd.CommandTimeout = 6000;
            using (var dt = cmd.ExecuteReader())
            {
                var dtEntity = new DataTable();
                dtEntity.Columns.Add("year", System.Type.GetType("System.Int32"));
                dtEntity.Columns.Add("cta", System.Type.GetType("System.Int32"));
                dtEntity.Columns.Add("scta", System.Type.GetType("System.Int32"));
                dtEntity.Columns.Add("sscta", System.Type.GetType("System.Int32"));
                dtEntity.Columns.Add("salini", System.Type.GetType("System.Decimal"));
                dtEntity.Columns.Add("enecargos", System.Type.GetType("System.Decimal"));
                dtEntity.Columns.Add("eneabonos", System.Type.GetType("System.Decimal"));
                dtEntity.Columns.Add("febcargos", System.Type.GetType("System.Decimal"));
                dtEntity.Columns.Add("febabonos", System.Type.GetType("System.Decimal"));
                dtEntity.Columns.Add("marcargos", System.Type.GetType("System.Decimal"));
                dtEntity.Columns.Add("marabonos", System.Type.GetType("System.Decimal"));
                dtEntity.Columns.Add("abrcargos", System.Type.GetType("System.Decimal"));
                dtEntity.Columns.Add("abrabonos", System.Type.GetType("System.Decimal"));
                dtEntity.Columns.Add("maycargos", System.Type.GetType("System.Decimal"));
                dtEntity.Columns.Add("mayabonos", System.Type.GetType("System.Decimal"));
                dtEntity.Columns.Add("juncargos", System.Type.GetType("System.Decimal"));
                dtEntity.Columns.Add("junabonos", System.Type.GetType("System.Decimal"));
                dtEntity.Columns.Add("julcargos", System.Type.GetType("System.Decimal"));
                dtEntity.Columns.Add("julabonos", System.Type.GetType("System.Decimal"));
                dtEntity.Columns.Add("agocargos", System.Type.GetType("System.Decimal"));
                dtEntity.Columns.Add("agoabonos", System.Type.GetType("System.Decimal"));
                dtEntity.Columns.Add("sepcargos", System.Type.GetType("System.Decimal"));
                dtEntity.Columns.Add("sepabonos", System.Type.GetType("System.Decimal"));
                dtEntity.Columns.Add("octcargos", System.Type.GetType("System.Decimal"));
                dtEntity.Columns.Add("octabonos", System.Type.GetType("System.Decimal"));
                dtEntity.Columns.Add("novcargos", System.Type.GetType("System.Decimal"));
                dtEntity.Columns.Add("novabonos", System.Type.GetType("System.Decimal"));
                dtEntity.Columns.Add("diccargos", System.Type.GetType("System.Decimal"));
                dtEntity.Columns.Add("dicabonos", System.Type.GetType("System.Decimal"));
                dtEntity.Columns.Add("cc", System.Type.GetType("System.String"));
                dtEntity.Columns.Add("descripcion", System.Type.GetType("System.String"));
                dtEntity.Columns.Add("descripcion2", System.Type.GetType("System.String"));

                dtEntity.Load(dt);

                var auxdata = dtEntity.AsEnumerable();

                var data = auxdata.Select(dataRow => new SaldosKubrixDTO
                {
                    year = (int)dataRow["year"],
                    cta = (int)dataRow["cta"],
                    scta = (int)dataRow["scta"],
                    sscta = (int)dataRow["sscta"],
                    salini = (decimal)dataRow["salini"],
                    enecargos = (decimal)dataRow["enecargos"],
                    eneabonos = (decimal)dataRow["eneabonos"],
                    febcargos = (decimal)dataRow["febcargos"],
                    febabonos = (decimal)dataRow["febabonos"],
                    marcargos = (decimal)dataRow["marcargos"],
                    marabonos = (decimal)dataRow["marabonos"],
                    abrcargos = (decimal)dataRow["abrcargos"],
                    abrabonos = (decimal)dataRow["abrabonos"],
                    maycargos = (decimal)dataRow["maycargos"],
                    mayabonos = (decimal)dataRow["mayabonos"],
                    juncargos = (decimal)dataRow["juncargos"],
                    junabonos = (decimal)dataRow["junabonos"],
                    julcargos = (decimal)dataRow["julcargos"],
                    julabonos = (decimal)dataRow["julabonos"],
                    agocargos = (decimal)dataRow["agocargos"],
                    agoabonos = (decimal)dataRow["agoabonos"],
                    sepcargos = (decimal)dataRow["sepcargos"],
                    sepabonos = (decimal)dataRow["sepabonos"],
                    octcargos = (decimal)dataRow["octcargos"],
                    octabonos = (decimal)dataRow["octabonos"],
                    novcargos = (decimal)dataRow["novcargos"],
                    novabonos = (decimal)dataRow["novabonos"],
                    diccargos = (decimal)dataRow["diccargos"],
                    dicabonos = (decimal)dataRow["dicabonos"],
                    cc = (string)dataRow["cc"],
                    descripcion = (string)dataRow["descripcion"],
                    descripcion2 = (string)dataRow["descripcion2"],

                }).ToList();

                return data;
            }
        }

        public static List<ClientesKubrixDTO> SelectClientes(EnkontrolEnum ek, OdbcConsultaDTO odbc)
        {
            var empresa = 2;
            if (ek == EnkontrolEnum.ArrenProd) { empresa = 2; }
            else empresa = 1;
            List<ClientesKubrixDTO> data = new List<ClientesKubrixDTO>();
            using (var con = conectar(ek))
            {
                using (var cmd = new OdbcCommand(odbc.consulta, con))
                {
                    var parameters = cmd.Parameters;
                    odbc.parametros.ForEach(p =>
                    {
                        parameters.Add("@" + p.nombre, p.tipo).Value = p.valor;
                    });
                    data.AddRange(getReaderClientes(cmd, empresa));
                }
            }
            return data;
        }

        public static List<ClientesKubrixDTO> getReaderClientes(OdbcCommand cmd, int empresa)
        {
            cmd.CommandTimeout = 6000;
            using (var dt = cmd.ExecuteReader()) 
            {
                var dtEntity = new DataTable();
                dtEntity.Columns.Add("numcte", System.Type.GetType("System.Int32"));
                dtEntity.Columns.Add("nombre", System.Type.GetType("System.String"));
                dtEntity.Columns.Add("factura", System.Type.GetType("System.Double"));
                dtEntity.Columns.Add("tm", System.Type.GetType("System.Int32"));
                dtEntity.Columns.Add("fecha", System.Type.GetType("System.DateTime"));
                dtEntity.Columns.Add("vencimiento", System.Type.GetType("System.DateTime"));
                dtEntity.Columns.Add("id_segmento", System.Type.GetType("System.Int32"));
                dtEntity.Columns.Add("desc_segmento", System.Type.GetType("System.String"));
                dtEntity.Columns.Add("tipo_cliente", System.Type.GetType("System.Int32"));
                dtEntity.Columns.Add("desc_tipo_cliente", System.Type.GetType("System.String"));
                //dtEntity.Columns.Add("moneda", System.Type.GetType("System.Int32"));
                dtEntity.Columns.Add("tipocambio", System.Type.GetType("System.Decimal"));
                dtEntity.Columns.Add("desc_moneda", System.Type.GetType("System.String"));
                //dtEntity.Columns.Add("cc", System.Type.GetType("System.String"));
                dtEntity.Columns.Add("desc_cc", System.Type.GetType("System.String"));
                dtEntity.Columns.Add("corto", System.Type.GetType("System.String"));
                dtEntity.Columns.Add("corte", System.Type.GetType("System.DateTime"));
                dtEntity.Columns.Add("plazo1", System.Type.GetType("System.Int32"));
                dtEntity.Columns.Add("plazo2", System.Type.GetType("System.Int32"));
                dtEntity.Columns.Add("plazo3", System.Type.GetType("System.Int32"));
                dtEntity.Columns.Add("dif1", System.Type.GetType("System.DateTime"));
                dtEntity.Columns.Add("dif2", System.Type.GetType("System.DateTime"));
                dtEntity.Columns.Add("dif3", System.Type.GetType("System.DateTime"));
                dtEntity.Columns.Add("movs_x_factura", System.Type.GetType("System.Int32"));
                dtEntity.Columns.Add("sum_venc_x_fact", System.Type.GetType("System.Decimal"));
                dtEntity.Columns.Add("sum_por_vencer", System.Type.GetType("System.Decimal"));
                dtEntity.Columns.Add("sum_vencido_1", System.Type.GetType("System.Decimal"));
                dtEntity.Columns.Add("sum_vencido_2", System.Type.GetType("System.Decimal"));
                dtEntity.Columns.Add("sum_vencido_3", System.Type.GetType("System.Decimal"));
                dtEntity.Columns.Add("sum_vencido_4", System.Type.GetType("System.Decimal"));
                dtEntity.Columns.Add("sum_vencido", System.Type.GetType("System.Decimal"));
                dtEntity.Columns.Add("saldo_x_factura", System.Type.GetType("System.Decimal"));
                dtEntity.Columns.Add("monto_factura", System.Type.GetType("System.Decimal"));
                //dtEntity.Columns.Add("fact_abono_no_apli", System.Type.GetType("System.Int32"));
                //dtEntity.Columns.Add("monto_fact_abono_no_apli", System.Type.GetType("System.Decimal"));
                //dtEntity.Columns.Add("fecha_max", System.Type.GetType("System.DateTime"));
                //dtEntity.Columns.Add("ultimo_tipocambio", System.Type.GetType("System.Decimal"));
                dtEntity.Columns.Add("descrip_tm", System.Type.GetType("System.String"));
                dtEntity.Columns.Add("concep_mov", System.Type.GetType("System.String"));
                //dtEntity.Columns.Add("ls_telefono", System.Type.GetType("System.String"));

                dtEntity.Load(dt);

                var auxdata = dtEntity.AsEnumerable();

                var data = auxdata.Select(dataRow => new ClientesKubrixDTO
                {
                    numcte = (int)dataRow["numcte"],
                    nombre = (string)dataRow["nombre"],
                    factura = (double)dataRow["factura"],
                    tm = (int)dataRow["tm"],
                    fecha = (DateTime)dataRow["fecha"],
                    vencimiento = (DateTime)dataRow["vencimiento"],
                    id_segmento = (int)dataRow["id_segmento"],
                    desc_segmento = (string)dataRow["desc_segmento"],
                    tipo_cliente = (int)dataRow["tipo_cliente"],
                    desc_tipo_cliente = (string)dataRow["desc_tipo_cliente"],
                    //moneda = (int)dataRow["moneda"],
                    tipocambio = (decimal)dataRow["tipocambio"],
                    desc_moneda = (string)dataRow["desc_moneda"],
                    //cc = (string)dataRow["cc"],
                    desc_cc = (string)dataRow["desc_cc"],
                    corto = (string)dataRow["corto"],
                    corte = (DateTime)dataRow["corte"],
                    plazo1 = (int)dataRow["plazo1"],
                    plazo2 = (int)dataRow["plazo2"],
                    plazo3 = (int)dataRow["plazo3"],
                    dif1 = (DateTime)dataRow["dif1"],
                    dif2 = (DateTime)dataRow["dif2"],
                    dif3 = (DateTime)dataRow["dif3"],
                    movs_x_factura = (int)dataRow["movs_x_factura"],
                    sum_venc_x_fact = (decimal)dataRow["sum_venc_x_fact"],
                    sum_por_vencer = (decimal)dataRow["sum_por_vencer"],
                    sum_vencido_1 = (decimal)dataRow["sum_vencido_1"],
                    sum_vencido_2 = (decimal)dataRow["sum_vencido_2"],
                    sum_vencido_3 = (decimal)dataRow["sum_vencido_3"],
                    sum_vencido_4 = (decimal)dataRow["sum_vencido_4"],
                    sum_vencido = (decimal)dataRow["sum_vencido"],
                    saldo_x_factura = (decimal)dataRow["saldo_x_factura"],
                    monto_factura = (decimal)dataRow["monto_factura"],
                    //fact_abono_no_apli = (int)dataRow["fact_abono_no_apli"],
                    //monto_fact_abono_no_apli = (decimal)dataRow["monto_fact_abono_no_apli"],
                    //fecha_max = (DateTime)dataRow["fecha_max"],
                    //ultimo_tipocambio = (decimal)dataRow["ultimo_tipocambio"],
                    descrip_tm = (string)dataRow["descrip_tm"],
                    concep_mov = (string)dataRow["concep_mov"],
                    //ls_telefono = (string)dataRow["ls_telefono"]
                }).ToList();

                return data;
            }
        }

        public static List<VencimientoKubrixDTO> SelectVencimientos(EnkontrolEnum ek, OdbcConsultaDTO odbc)
        {
            var empresa = 2;
            if (ek == EnkontrolEnum.ArrenProd) { empresa = 2; }
            else empresa = 1;
            List<VencimientoKubrixDTO> data = new List<VencimientoKubrixDTO>();
            using (var con = conectar(ek))
            {
                using (var cmd = new OdbcCommand(odbc.consulta, con))
                {
                    var parameters = cmd.Parameters;
                    odbc.parametros.ForEach(p =>
                    {
                        parameters.Add("@" + p.nombre, p.tipo).Value = p.valor;
                    });
                    data.AddRange(getReaderVencimientos(cmd, empresa));
                }
            }
            return data;
        }

        public static List<VencimientoKubrixDTO> getReaderVencimientos(OdbcCommand cmd, int empresa)
        {
            cmd.CommandTimeout = 6000;
            using (var dt = cmd.ExecuteReader())
            {
                var dtEntity = new DataTable();
                dtEntity.Columns.Add("numpro", System.Type.GetType("System.Int32"));
                dtEntity.Columns.Add("nombre", System.Type.GetType("System.String"));
                dtEntity.Columns.Add("por_vencer", System.Type.GetType("System.Decimal"));
                dtEntity.Columns.Add("plazo_1", System.Type.GetType("System.Decimal"));
                dtEntity.Columns.Add("plazo_2", System.Type.GetType("System.Decimal"));
                dtEntity.Columns.Add("plazo_3", System.Type.GetType("System.Decimal"));
                dtEntity.Columns.Add("plazo_4", System.Type.GetType("System.Decimal"));
                dtEntity.Columns.Add("factura", System.Type.GetType("System.Int32"));
                dtEntity.Columns.Add("moneda", System.Type.GetType("System.String"));
                dtEntity.Columns.Add("fecha", System.Type.GetType("System.DateTime"));
                dtEntity.Columns.Add("fechavenc", System.Type.GetType("System.DateTime"));
                dtEntity.Columns.Add("tm", System.Type.GetType("System.Int32"));
                dtEntity.Columns.Add("autorizapago", System.Type.GetType("System.String"));
                dtEntity.Columns.Add("saldo_factura", System.Type.GetType("System.Decimal"));
                dtEntity.Columns.Add("plazo1", System.Type.GetType("System.Int32"));
                dtEntity.Columns.Add("plazo2", System.Type.GetType("System.Int32"));
                dtEntity.Columns.Add("plazo3", System.Type.GetType("System.Int32"));
                dtEntity.Columns.Add("plazo4", System.Type.GetType("System.Int32"));
                dtEntity.Columns.Add("cc", System.Type.GetType("System.String"));
                dtEntity.Columns.Add("descripcion", System.Type.GetType("System.String"));
                dtEntity.Columns.Add("tipocambio", System.Type.GetType("System.Decimal"));
                dtEntity.Columns.Add("nomcorto", System.Type.GetType("System.String"));
                dtEntity.Columns.Add("corto", System.Type.GetType("System.String"));
                dtEntity.Columns.Add("telefono1", System.Type.GetType("System.String"));
                dtEntity.Columns.Add("saldo_factura_dlls", System.Type.GetType("System.Decimal"));
                dtEntity.Columns.Add("concepto", System.Type.GetType("System.String"));
                dtEntity.Columns.Add("moneda_id", System.Type.GetType("System.Int32"));

                dtEntity.Load(dt);

                var auxdata = dtEntity.AsEnumerable();

                var data = auxdata.Select(dataRow => new VencimientoKubrixDTO
                {
                    numpro = (int)dataRow["numpro"],
                    nombre = (string)dataRow["nombre"],
                    por_vencer = (decimal)dataRow["por_vencer"],
                    plazo_1 = (decimal)dataRow["plazo_1"],
                    plazo_2 = (decimal)dataRow["plazo_2"],
                    plazo_3 = (decimal)dataRow["plazo_3"],
                    plazo_4 = (decimal)dataRow["plazo_4"],
                    factura = (int)dataRow["factura"],
                    moneda = (string)dataRow["moneda"],
                    fecha = (DateTime)dataRow["fecha"],
                    fechavenc = (DateTime)dataRow["fechavenc"],
                    tm = (int)dataRow["tm"],
                    autorizapago = (string)dataRow["autorizapago"],
                    saldo_factura = (decimal)dataRow["saldo_factura"],
                    plazo1 = (int)dataRow["plazo1"],
                    plazo2 = (int)dataRow["plazo2"],
                    plazo3 = (int)dataRow["plazo3"],
                    plazo4 = (int)dataRow["plazo4"],
                    cc = (string)dataRow["cc"],
                    descripcion = (string)dataRow["descripcion"],
                    tipocambio = (decimal)dataRow["tipocambio"],
                    nomcorto = (string)dataRow["nomcorto"],
                    corto = (string)dataRow["corto"],
                    telefono1 = (string)dataRow["telefono1"],
                    saldo_factura_dlls = (decimal)dataRow["saldo_factura_dlls"],
                    concepto = (string)dataRow["concepto"],
                    moneda_id = (int)dataRow["moneda_id"]
                }).ToList();

                return data;
            }
        }

    }
}
