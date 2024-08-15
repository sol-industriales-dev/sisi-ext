using Core.DAO.Administracion.TransferenciasBancarias;
using Core.DTO;
using Core.DTO.Administracion;
using Core.DTO.Contabilidad.Propuesta;
using Core.Entity.Principal.Usuarios;
using Core.Enum.Multiempresa;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Utils;
using Core.DTO.Administracion.TransferenciasBancarias;
using System.Web.Mvc;
using System.Web;
using Core.Enum.Administracion.TransferenciasBancarias;
using System.IO;
using System.IO.Compression;
using Core.DTO.Utils.Data;
using System.Data.Odbc;
using Core.Entity.Administrativo.TransferenciasBancarias;
using Data.EntityFramework;
using Core.DTO.Enkontrol.OrdenCompra;
using System.Globalization;

namespace Data.DAO.Administracion.TransferenciasBancarias
{
    public class TransferenciasBancariasDAO : GenericDAO<tblP_Usuario>, ITransferenciasBancariasDAO
    {
        private Dictionary<string, object> resultado = new Dictionary<string, object>();
        private string NombreControlador = "TransferenciasBancariasController";
        private readonly string RutaBase = @"\\REPOSITORIO\Proyecto\SIGOPLAN\TRANSFERENCIAS_BANCARIAS";
        private const string RutaLocal = @"C:\Proyecto\SIGOPLAN\TRANSFERENCIAS_BANCARIAS";
        private readonly string RutaTemp;
        private bool productivo = Convert.ToString(System.Web.Configuration.WebConfigurationManager.AppSettings["enkontrolProductivo"]) == "1";

        public TransferenciasBancariasDAO()
        {
#if DEBUG
            RutaBase = RutaLocal;
#endif

            RutaTemp = RutaBase;

            resultado.Clear();
        }

        public Dictionary<string, object> CargarMovimientosProveedorAutorizados(int proveedorInicial, int proveedorFinal, DateTime fechaInicial, DateTime fechaFinal)
        {
            try
            {
                var converted = new List<sp_genera_movprovDTO>();
                CultureInfo provider = CultureInfo.InvariantCulture;
                string format = "yyyyMMdd";
                fechaFinal = fechaFinal.AddHours(23).AddMinutes(59).AddSeconds(59);

                var listaBancos = _contextEnkontrol.Select<BancosDTO>(getEnkontrolAmbienteConsulta(), @"
                    SELECT
                        cta.numpro, cta.cuenta, cta.clabe, ban.banco, ban.descripcion AS bancoDesc
                    FROM " + vSesiones.sesionEmpresaDBPregijo + @"sp_proveedores_cta_dep cta
                        INNER JOIN " + vSesiones.sesionEmpresaDBPregijo + @"sb_bancos ban ON cta.banco = ban.banco
                    WHERE cta.ind_cuenta_def = 1 AND cta.ind_cuenta_activa = 1"
                );
                var listaProveedores = _contextEnkontrol.Select<ComboDTO>(getEnkontrolAmbienteConsulta(), string.Format(@"SELECT numpro as Value, nombre as Text FROM {0}sp_proveedores", vSesiones.sesionEmpresaDBPregijo));
                var listaTipoMovimiento = _contextEnkontrol.Select<ComboDTO>(getEnkontrolAmbienteConsulta(), "SELECT tm AS Value, descripcion AS Text FROM " + vSesiones.sesionEmpresaDBPregijo + @"sp_tm");

                var auxlistaMovimientos = _context.tblC_sp_gastos_prov.Where(x =>
                    x.estatus == "A" && x.numpro >= proveedorInicial && x.numpro <= proveedorFinal && x.fechaPropuesta >= fechaInicial && x.fechaPropuesta <= fechaFinal && x.autorizada
                ).OrderBy(x => x.numpro).ToList();

                var listaMovimientos = auxlistaMovimientos.Select(x =>
                {
                    DateTime aux_fecha_timbrado = new DateTime();
                    bool aplicaFecha = DateTime.TryParseExact(x.fecha_timbrado, format, provider, DateTimeStyles.None, out aux_fecha_timbrado);                    
                    return new FacturaDTO
                        {
                            factura = Int32.Parse(x.factura),
                            estado = x.fechaAutorizacion == null ? "PENDIENTE" : "AUTORIZADO",
                            tm = x.tm,
                            cc = x.cc,
                            monto = x.total, //monto = x.monto,
                            monto_plan = x.monto_plan,
                            programo = x.programo,
                            autorizo = x.autorizo,
                            proveedor = listaProveedores.Where(w => w.Value.Equals(x.numpro)).Select(z => z.Text).FirstOrDefault(),
                            cuenta = listaBancos.Where(y => y.numpro == x.numpro).Select(z => z.cuenta).FirstOrDefault(),
                            clabe = listaBancos.Where(y => y.numpro == x.numpro).Select(z => z.clabe).FirstOrDefault(),
                            banco = listaBancos.Where(y => y.numpro == x.numpro).Select(z => z.banco).FirstOrDefault(),
                            bancoDesc = listaBancos.Where(y => y.numpro == x.numpro).Select(z => z.bancoDesc).FirstOrDefault(),
                            numpro = x.numpro,
                            referenciaoc = x.referenciaoc,
                            chequeGenerado = false,
                            cuentaBanorte = vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? "0157323241" : vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora ? "1016805896" : "",
                            cuentaSantander = vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? "65507209813" : vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora ? "65506595661" : "",
                            fecha_timbrado = (aplicaFecha ? aux_fecha_timbrado : DateTime.Today).ToShortDateString(),
                            fecha_validacion = (x.fecha_autoriza_factura ?? DateTime.Today).ToShortDateString()
                        };
                }).ToList();

                //Se cruza la información con los registros pendientes de la tabla sp_movprov para mostrar las mismas facturas que en Enkontrol en la pantalla de "Generación de Cheques Automáticos".
                var hoyAnioSiguiente = DateTime.Now.AddDays(365).ToString("yyyy-MM-dd");
                var listaFacturasEnkontrol = _contextEnkontrol.Select<FacturaDTO>(getEnkontrolAmbienteConsulta(), string.Format(@"
                    SELECT
                        t1.numpro, t1.factura, t1.fechavenc, t1.tm, t1.cc, t1.referenciaoc, t1.concepto, t1.total, t1.fecha,
                        (SELECT SUM(t2.total) FROM sp_movprov t2 WHERE t2.numpro = t1.numpro AND t2.factura = t1.factura AND (t2.fecha != t1.fecha OR t2.tm != t1.tm OR t2.poliza != t1.poliza OR t2.linea != t1.linea)) AS total2,
                        (t1.total + ISNULL(total2, 0)) AS saldo
                    FROM sp_movprov t1
                    WHERE (t1.autorizapago = 'A' OR t1.autorizapago = 'E') AND t1.es_factura = 'S' AND saldo > 0 AND t1.fechavenc <= '{0}'
                    ORDER BY t1.numpro, t1.factura, t1.cc, t1.referenciaoc", hoyAnioSiguiente)
                );

                foreach (var mov in listaMovimientos)
                {
                    var facturaPendienteEK = listaFacturasEnkontrol.FirstOrDefault(x => x.numpro == mov.numpro && x.factura == mov.factura);

                    if (facturaPendienteEK == null)
                    {
                        mov.chequeGenerado = true;
                    }
                    else
                    {
                        mov.fecha_validacion = facturaPendienteEK.fecha.ToShortDateString();
                    }
                }

                resultado.Add("data", listaMovimientos);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(11, 0, NombreControlador, "CargarMovimientosProveedorAutorizados", e, AccionEnum.CONSULTA, 0, new { proveedorInicial = proveedorInicial, proveedorFinal = proveedorFinal });
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Tuple<Stream, string> CargarArchivoComprimido(List<RegistroArchivoDTO> registros)
        {
            var rutaFolderTemp = "";

            try
            {
                var nombreFolderTemp = String.Format("{0} {1}", "tmp", ObtenerFormatoCarpetaFechaActual());

                rutaFolderTemp = Path.Combine(RutaTemp, nombreFolderTemp);

                List<FormatoBancoEnum> formatosSeleccionados = registros.GroupBy(x => (FormatoBancoEnum)x.bancoOrigen).Select(x => x.Key).ToList();

                var proveedoresEK = _contextEnkontrol.Select<ProveedorDTO>(vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? EnkontrolEnum.CplanProd : EnkontrolEnum.ArrenProd, new OdbcConsultaDTO()
                {
                    consulta = @"
                        SELECT
                            prov.numpro,
                            REPLACE(REPLACE(prov.nombre, CHAR(10), ''), CHAR(13), '') AS nombre,
                            prov.rfc,
                            prov.responsable AS beneficiario,
                            ban.banco,
                            ban.descripcion AS bancoDesc,
                            cta_dep.moneda,
                            cta_dep.cuenta,
                            cta_dep.sucursal,
                            cta_dep.plaza,
                            cta_dep.clabe,
                            prov.email
                        FROM sp_proveedores prov
                            LEFT JOIN sp_proveedores_cta_dep cta_dep ON prov.numpro = cta_dep.numpro AND cta_dep.ind_cuenta_def = 1 AND cta_dep.ind_cuenta_activa = 1
                            LEFT JOIN sb_bancos ban ON cta_dep.banco = ban.banco"
                });

                foreach (var prov in proveedoresEK)
                {
                    if (prov.nombre.Contains("Ñ"))
                    {
                        prov.nombre = prov.nombre.Replace("Ñ", "N");
                    }

                    if (prov.nombre.Contains("ñ"))
                    {
                        prov.nombre = prov.nombre.Replace("ñ", "n");
                    }
                }

                var proveedoresBanorte = new List<tblTB_ProveedoresBanorte>();

                using (MainContext _contextConstruplan = new MainContext(EmpresaEnum.Construplan))
                {
                    proveedoresBanorte = _context.tblTB_ProveedoresBanorte.Where(x => x.registroActivo).ToList();
                }

                foreach (var formato in formatosSeleccionados)
                {
                    var rutaCarpetaFormato = Path.Combine(rutaFolderTemp, formato.GetDescription());
                    Directory.CreateDirectory(rutaCarpetaFormato);

                    foreach (var operacion in Enum.GetValues(typeof(OperacionEnum)).Cast<OperacionEnum>())
                    {
                        var registrosFormatoOperacion = registros.Where(x => (FormatoBancoEnum)x.bancoOrigen == formato && x.operacion == operacion).ToList();

                        if (registrosFormatoOperacion.Count() > 0)
                        {
                            var rutaArchivoTexto = Path.Combine(rutaCarpetaFormato, formato.GetDescription() + "_" + operacion.GetDescription() + ".txt");

                            using (FileStream fs = File.Create(rutaArchivoTexto))
                            {
                                foreach (var registro in registrosFormatoOperacion)
                                {
                                    if (registro.referencia == null)
                                    {
                                        registro.referencia = "";
                                    }

                                    if (registro.referencia.Count() > 10)
                                    {
                                        throw new Exception("La referencia \"" + registro.referencia + "\" sobrepasa el límite de 10 caracteres.");
                                    }

                                    //if (registro.descripcion.Count() > 30)
                                    //{
                                    //    throw new Exception("La descripción \"" + registro.descripcion + "\" sobrepasa el límite de 30 caracteres.");
                                    //}

                                    var proveedorEK = proveedoresEK.FirstOrDefault(x => x.numpro == registro.numpro);

                                    if (proveedorEK == null)
                                    {
                                        throw new Exception("No se encuentra la información del proveedor \"" + registro.numpro + "\".");
                                    }

                                    var textoLinea = "";
                                    var rfc = proveedorEK.rfc ?? "";
                                    var fechaAplicacion = DateTime.Now.AddDays(1).ToShortDateString().Replace("/", string.Empty);
                                    var instruccionPago = proveedorEK.beneficiario ?? ""; //Debe ir el beneficiario
                                    var beneficiario = (proveedorEK.nombre ?? "").Replace(",", "").Replace(".", "");
                                    var sucursal = proveedorEK.sucursal ?? "";
                                    var emailBeneficiario = proveedorEK.email ?? "";

                                    switch (formato)
                                    {
                                        #region SANTANDER
                                        case FormatoBancoEnum.SANTANDER:
                                            #region Banco Receptor
                                            var bancoReceptor = "";

                                            //Los cases son los ID's de Enkontrol. Los códigos de los bancos son del catálogo de SANTANDER.
                                            switch (proveedorEK.banco)
                                            {
                                                case 1: //BANAMEX S.A.
                                                    bancoReceptor = "BANAM";
                                                    break;
                                                case 2: //HSBC
                                                    bancoReceptor = "BITAL";
                                                    break;
                                                case 3: //BANORTE
                                                    bancoReceptor = "BBANO";
                                                    break;
                                                case 4: //SANTANDER
                                                    bancoReceptor = "BANME";
                                                    break;
                                                case 5: //BBVA BANCOMER SA
                                                    bancoReceptor = "BACOM";
                                                    break;
                                                case 6: //SCOTIABANK INVERLAT SA
                                                    bancoReceptor = "COMER";
                                                    break;
                                                case 7: //BANCO REGIONAL DE MONTERREY SA
                                                    bancoReceptor = "BANRE";
                                                    break;
                                                case 8: //INBURSA GRUPO FINANCIERO
                                                    bancoReceptor = "BINBU";
                                                    break;
                                                case 11: //BANCO DEL BAJIO SA
                                                    bancoReceptor = "BAJIO";
                                                    break;
                                                case 19: //BANCA AFIRME
                                                    bancoReceptor = "BAFIR";
                                                    break;
                                                case 23: //BANCO MONEX SA
                                                    bancoReceptor = "CMCA";
                                                    break;
                                                case null:
                                                    throw new Exception("No se encuentra la información del banco del proveedor, verifique que el proveedor tenga una cuenta activa relacionada. Proveedor: " + proveedorEK.numpro + " - " + proveedorEK.nombre);
                                                default:
                                                    throw new Exception("El banco destino se encuentra fuera de las opciones establecidas. Comuníquese con el departamento de tecnologías. Banco: " + proveedorEK.banco.ToString());
                                            }
                                            #endregion

                                            switch (operacion)
                                            {
                                                case OperacionEnum.terceros:
                                                    var fechaActual = DateTime.Now.Date;
                                                    var cuentaSantanderTerceros = "";

                                                    if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan)
                                                    {
                                                        cuentaSantanderTerceros = "65507209813";
                                                    }
                                                    else if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                                                    {
                                                        cuentaSantanderTerceros = "65506595661";
                                                    }

                                                    //Los espacios en blanco son específicos e irregulares según el formato que mandaron de Santander.
                                                    textoLinea += cuentaSantanderTerceros + "     "; //textoLinea += registro.cuentaOrigen + "     ";
                                                    textoLinea += registro.cuentaDestino.PadRight(16, ' ');

                                                    var montoString = !((registro.monto % 1) == 0) ? registro.monto.ToString("0.00") : registro.monto.ToString() + ".00";

                                                    textoLinea += montoString.PadLeft(13, '0') + "                                        ";
                                                    textoLinea += fechaActual.ToShortDateString().Replace("/", "");
                                                    break;
                                                case OperacionEnum.SPEI:
                                                    var cuentaSantanderSPEI = ""; //Va la cuenta en el origen a pesar de ser SPEI porque así lo pide el formato que maneja Santander.

                                                    if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan)
                                                    {
                                                        cuentaSantanderSPEI = "65507209813";
                                                    }
                                                    else if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                                                    {
                                                        cuentaSantanderSPEI = "65506595661";
                                                    }

                                                    //Los espacios en blanco son específicos e irregulares según el formato que mandaron de Santander.
                                                    textoLinea += "LTX05" + cuentaSantanderSPEI + "       ";
                                                    textoLinea += registro.clabeDestino + "  ";
                                                    textoLinea += bancoReceptor;
                                                    textoLinea += beneficiario.Count() > 40 ? beneficiario.Substring(0, 40) : beneficiario.PadRight(40, ' ');
                                                    textoLinea += "0101";
                                                    textoLinea += (registro.monto.ToString("0.00").Replace(".", "")).PadLeft(18, '0');

                                                    var espaciosFinalCantidad = 56;

                                                    if (registro.referencia.Count() > 0)
                                                    {
                                                        espaciosFinalCantidad -= registro.referencia.Count();
                                                    }

                                                    textoLinea += ("01001").PadRight(45, ' ') + registro.referencia;
                                                    textoLinea += new string(' ', espaciosFinalCantidad);
                                                    break;
                                                default:
                                                    break;
                                            }
                                            break;
                                        #endregion
                                        #region BANORTE
                                        case FormatoBancoEnum.BANORTE:
                                            var proveedorSIGOPLAN = proveedoresBanorte.LastOrDefault(x => x.rfc.Trim() == proveedorEK.rfc.Trim()
                                                //&& x.cuenta_clabe_celular.Trim() == registro.cuentaDestino.Trim()
                                            );

                                            if (proveedorSIGOPLAN == null)
                                            {
                                                throw new Exception("No se encuentra la información del proveedor con el RFC \"" + proveedorEK.rfc.Trim() + "\" en la base de datos de SIGOPLAN.");
                                            }

                                            var claveIDProveedor = proveedorSIGOPLAN.banorte_id;
                                            var descripcion = registro.descripcion.Length <= 30 ? registro.descripcion : registro.descripcion.Substring(0, 30);

                                            switch (operacion)
                                            {
                                                case OperacionEnum.terceros:
                                                    var cuentaBanorteTerceros = "";

                                                    if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan)
                                                    {
                                                        cuentaBanorteTerceros = "0157323241";
                                                    }
                                                    else if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                                                    {
                                                        cuentaBanorteTerceros = "1016805896";
                                                    }

                                                    textoLinea += "02" + "\t"; //OPERACIÓN
                                                    textoLinea += claveIDProveedor + "\t"; //CLAVE ID PROVEEDOR
                                                    textoLinea += cuentaBanorteTerceros + "\t"; //CUENTA ORIGEN
                                                    textoLinea += registro.cuentaDestino + "\t"; //CUENTA/CLABE DESTINO
                                                    textoLinea += registro.monto.ToString("0.00") + "\t"; //IMPORTE
                                                    textoLinea += registro.referencia + "\t"; //REFERENCIA
                                                    textoLinea += claveIDProveedor + "\t"; //DESCRIPCIÓN
                                                    textoLinea += "GCP800324FJ1" + "\t"; //RFC ORDENANTE
                                                    textoLinea += "0" + "\t"; //IVA
                                                    textoLinea += DateTime.Now.ToShortDateString().Replace("/", string.Empty) + "\t"; //FECHA DE APLICACIÓN
                                                    textoLinea += "X"; //INSTRUCCIÓN DE PAGO
                                                    break;
                                                case OperacionEnum.SPEI:
                                                    var cuentaBanorteSPEI = ""; //Va la cuenta en el origen a pesar de ser SPEI porque así lo pide el formato que maneja Banorte.

                                                    if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan)
                                                    {
                                                        cuentaBanorteSPEI = "0157323241";
                                                    }
                                                    else if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                                                    {
                                                        cuentaBanorteSPEI = "1016805896";
                                                    }

                                                    textoLinea += "04" + "\t"; //OPERACIÓN
                                                    textoLinea += claveIDProveedor + "\t"; //CLAVE ID PROVEEDOR
                                                    textoLinea += cuentaBanorteSPEI + "\t"; //CUENTA ORIGEN
                                                    textoLinea += registro.clabeDestino + "\t"; //CUENTA/CLABE DESTINO
                                                    textoLinea += registro.monto.ToString("0.00") + "\t"; //IMPORTE
                                                    textoLinea += registro.referencia + "\t"; //REFERENCIA
                                                    textoLinea += claveIDProveedor + "\t"; //textoLinea += descripcion.Length <= 30 ? descripcion : descripcion.Substring(0, 30) + "\t"; //DESCRIPCIÓN
                                                    textoLinea += "GCP800324FJ1" + "\t"; //textoLinea += rfc + "\t"; //RFC ORDENANTE
                                                    textoLinea += "0" + "\t"; //textoLinea += (Math.Truncate(100 * (registro.monto * 0.16m)) / 100) + "\t"; //IVA
                                                    textoLinea += DateTime.Now.ToShortDateString().Replace("/", string.Empty) + "\t"; //FECHA DE APLICACIÓN
                                                    textoLinea += instruccionPago.Length <= 70 ? instruccionPago : instruccionPago.Substring(0, 70); //INSTRUCCIÓN DE PAGO
                                                    break;
                                                case OperacionEnum.SPID:
                                                    #region Validaciones
                                                    int numericValue;
                                                    if (!int.TryParse(registro.referencia, out numericValue))
                                                    {
                                                        throw new Exception("La referencia debe ser un número mayor a cero para las transferencias SPID.");
                                                    }
                                                    else
                                                    {
                                                        if (numericValue <= 0)
                                                        {
                                                            throw new Exception("La referencia debe ser un número mayor a cero para las transferencias SPID.");
                                                        }
                                                    }

                                                    //if (registro.descripcion.Count() > 40)
                                                    //{
                                                    //    throw new Exception("La descripción no debe sobrepasar los 40 caracteres.");
                                                    //}
                                                    #endregion

                                                    //Longitud Fija - 312 caracteres

                                                    textoLinea += "13" + "\t"; //OPERACIÓN
                                                    textoLinea += claveIDProveedor.PadRight(13, ' ') + "\t"; //CLAVE ID PROVEEDOR
                                                    textoLinea += registro.cuentaOrigen.PadLeft(20, '0') + "\t"; //CUENTA ORIGEN
                                                    textoLinea += registro.cuentaDestino.PadLeft(30, '0') + "\t"; //CUENTA/CLABE DESTINO
                                                    textoLinea += registro.monto.ToString().PadLeft(14, '0') + "\t"; //IMPORTE
                                                    textoLinea += registro.referencia.PadLeft(10, '0') + "\t"; //REFERENCIA
                                                    textoLinea += "0000000" + "\t"; //CLAVE TIPO CAMBIO
                                                    textoLinea += descripcion.PadRight(40, ' ') + "\t"; //DESCRIPCIÓN
                                                    textoLinea += "2" + "\t"; //MONEDA ORIGEN
                                                    textoLinea += "2" + "\t"; //MONEDA DESTINO
                                                    textoLinea += rfc.PadRight(13, ' ') + "\t"; //RFC ORDENANTE
                                                    textoLinea += (Math.Truncate(100 * (registro.monto * 0.16m)) / 100).ToString().PadLeft(14, '0') + "\t"; //IVA
                                                    textoLinea += string.Empty.PadRight(39, ' ') + "\t"; //E-MAIL BENEFICIARIO
                                                    textoLinea += fechaAplicacion + "\t"; //FECHA APLICACIÓN
                                                    textoLinea += instruccionPago.PadRight(99, ' ') + "\t"; //INSTRUCCIÓN DE PAGO
                                                    textoLinea += "1"; //TIPO DE OPERACIÓN
                                                    break;
                                            }
                                            break;
                                        #endregion
                                    }

                                    byte[] title = new UTF8Encoding(true).GetBytes(textoLinea);
                                    fs.Write(title, 0, title.Length);
                                    byte[] newline = Encoding.ASCII.GetBytes(Environment.NewLine);
                                    fs.Write(newline, 0, newline.Length);
                                }
                            }
                        }
                    }
                }

                string rutaNuevoZip = Path.Combine(RutaTemp, nombreFolderTemp + ".zip");
                GlobalUtils.ComprimirCarpeta(rutaFolderTemp, rutaNuevoZip);

                Directory.Delete(rutaFolderTemp, true);
                var zipStream = GlobalUtils.GetFileAsStream(rutaNuevoZip);

                File.Delete(rutaNuevoZip);

                return Tuple.Create(zipStream, "PAGOS_MASIVOS_PROVEEDORES.zip");
            }
            catch (Exception e)
            {
                try
                {
                    Directory.Delete(rutaFolderTemp);
                }
                catch (Exception ex)
                {
                    LogError(11, 0, NombreControlador, "CargarArchivoComprimido_Directory_Delete", ex, AccionEnum.ELIMINAR, 0, rutaFolderTemp);
                }

                LogError(11, 0, NombreControlador, "CargarArchivoComprimido", e, AccionEnum.DESCARGAR, 0, registros);

                throw new Exception(e.Message);
            }
        }

        private string ObtenerFormatoCarpetaFechaActual()
        {
            return DateTime.Now.ToString("dd-MM-y HH:mm:ss").Replace(":", "-");
        }

        public Dictionary<string, object> GenerarCheques(List<FacturaDTO> facturas, int cuentaBancaria)
        {
            resultado = new Dictionary<string, object>();

            using (var dbSigoplanTransaction = _context.Database.BeginTransaction())
            {
                using (var con = getEnkontrolConexion())
                {
                    using (var trans = con.BeginTransaction())
                    {
                        try
                        {
                            var relacionUsuarioSIGOPLANEK = _context.tblP_Usuario_Enkontrol.FirstOrDefault(x => x.idUsuario == vSesiones.sesionUsuarioDTO.id);
                            var cuentaBancariaEK = _contextEnkontrol.Select<CuentaBancariaDTO>(getEnkontrolAmbienteConsulta(), new OdbcConsultaDTO { consulta = @"SELECT * FROM sb_cuenta WHERE cuenta = " + cuentaBancaria }).FirstOrDefault();
                            var bancoEK = _contextEnkontrol.Select<BancosDTO>(getEnkontrolAmbienteConsulta(), new OdbcConsultaDTO { consulta = @"SELECT banco, descripcion AS bancoDesc FROM sb_bancos WHERE banco = " + cuentaBancariaEK.banco }).FirstOrDefault();
                            var listaAreasCuentas = _contextEnkontrol.Select<AreaCuentaEKDTO>(getEnkontrolAmbienteConsulta(), new OdbcConsultaDTO { consulta = string.Format(@"SELECT * FROM si_area_cuenta WHERE cc_activo = 1") });
                            var listaRelacionTM = _contextEnkontrol.Select<TmDTO>(getEnkontrolAmbienteConsulta(), new OdbcConsultaDTO { consulta = string.Format(@"SELECT * FROM sp_tm") });

                            var count = 0;
                            var facturasAgrupadas = facturas.GroupBy(x => x.numpro).Select(x => new { numpro = x.Key, grp = x }).ToList();

                            var stringListaProveedores = string.Join(", ", facturasAgrupadas.Select(x => x.numpro).ToList());
                            var fechaActual = DateTime.Today;
                            var fechaMesesAtras = DateTime.Today.AddMonths(-2);
                            var anioAtras = fechaMesesAtras.Year;
                            var anioActual = fechaActual.Year;

                            var auxConsulta = "";
                            var auxConsulta2 = "";
                            for (int i = anioAtras; i <= anioActual; i++) 
                            {
                                if (i == anioActual) 
                                {
                                    auxConsulta += string.Format(@"SELECT referencia, numpro, poliza FROM sc_movpol WHERE tp = '07' AND year = {0} AND mes <= {1} AND numpro IN ({2})", i, fechaActual.Month, stringListaProveedores);
                                    auxConsulta2 += string.Format(@"SELECT referencia, numpro, poliza, cta, scta, sscta, monto FROM sc_movpol WHERE tp = '07' AND cta = 1146 AND year = {0} AND mes <= {1}", i, fechaActual.Month);    
                                }
                                else if (i == anioAtras)
                                {
                                    auxConsulta += string.Format(@"SELECT referencia, numpro, poliza FROM sc_movpol WHERE tp = '07' AND year = {0} AND mes >= {1} AND numpro IN ({2}) UNION ", i, fechaMesesAtras.Month, stringListaProveedores);
                                    auxConsulta2 += string.Format(@"SELECT referencia, numpro, poliza, cta, scta, sscta, monto FROM sc_movpol WHERE tp = '07' AND cta = 1146 AND year = {0} AND mes >= {1} UNION ", i, fechaMesesAtras.Month);

                                }
                                else 
                                {
                                    auxConsulta += string.Format(@"SELECT referencia, numpro, poliza FROM sc_movpol WHERE tp = '07' AND year = {0} AND numpro IN ({2}) UNION ", i, stringListaProveedores);
                                    auxConsulta2 += string.Format(@"SELECT referencia, numpro, poliza, cta, scta, sscta, monto FROM sc_movpol WHERE tp = '07' AND cta = 1146 AND year = {0} UNION", i);
                                }
                            }


                            var listaMovimientoPolizaFactura = _contextEnkontrol.Select<MovimientoPolizaDTO>(getEnkontrolAmbienteConsulta(), new OdbcConsultaDTO
                            {
                                consulta = auxConsulta //string.Format(@"SELECT referencia, numpro, poliza FROM sc_movpol WHERE tp = '07' AND year >= {0} AND mes >= {1} AND numpro IN ({2})", fechaMesesAtras.Year, fechaMesesAtras.Month, stringListaProveedores)
                            });
                            var listaMovimientoPolizaFacturaIVA = _contextEnkontrol.Select<MovimientoPolizaDTO>(getEnkontrolAmbienteConsulta(), new OdbcConsultaDTO
                            {
                                consulta = auxConsulta2 //string.Format(@"SELECT referencia, numpro, poliza, cta, scta, sscta, monto FROM sc_movpol WHERE tp = '07' AND cta = 1146 AND year >= {0} AND mes >= {1}", fechaMesesAtras.Year, fechaMesesAtras.Month)
                            });

                            foreach (var grpFac in facturasAgrupadas)
                            {
                                var proveedor = _contextEnkontrol.Select<ProveedorDTO>(getEnkontrolAmbienteConsulta(), new OdbcConsultaDTO { consulta = string.Format(@"SELECT TOP 1 nombre, rfc FROM sp_proveedores WHERE numpro = " + grpFac.numpro) }).FirstOrDefault();
                                var listaFacturas = grpFac.grp.ToList();
                                var ultimoCheque = _contextEnkontrol.Select<dynamic>(getEnkontrolAmbienteConsulta(), new OdbcConsultaDTO { consulta = string.Format(@"SELECT TOP 1 numero FROM sb_cheques WHERE cuenta = {0} ORDER BY numero DESC", cuentaBancaria) }).FirstOrDefault();
                                var siguienteNumeroCheque = ultimoCheque != null ? (ultimoCheque.numero + 1) : 1;

                                //Los cheques electrónicos empiezan en el número 500,000
                                if (siguienteNumeroCheque < 500000)
                                {
                                    siguienteNumeroCheque = 500000;
                                }

                                var tm_banco = listaRelacionTM.FirstOrDefault(x => x.tm == listaFacturas[0].tm).tm_banco;
                                var tm_pago = listaRelacionTM.FirstOrDefault(x => x.tm == listaFacturas[0].tm).tm_pago;

                                #region Obtener los campos cpto1, cpto2, y cpto3
                                var cpto1 = (string.Format(@"Pago de la(s) Factura(s):  {0} (07:{1})", listaFacturas[0].factura, siguienteNumeroCheque)).PadRight(50, ' ');
                                var cpto2 = " ";
                                var cpto3 = "";

                                if (listaFacturas.Count() > 1)
                                {
                                    var facturasString = string.Join(" ", listaFacturas.Skip(1).Select(x => x.factura.ToString() + " (07:" + siguienteNumeroCheque + ")").ToList());

                                    if (facturasString.Count() <= 50)
                                    {
                                        if (facturasString.Count() < 48)
                                        {
                                            cpto2 = "  " + facturasString;
                                        }
                                        else
                                        {
                                            cpto2 = "  " + facturasString.Substring(0, 48);
                                        }
                                    }
                                    else
                                    {
                                        cpto2 = "  " + facturasString.Substring(0, 48);
                                        cpto3 = facturasString.Substring(48);

                                        if (cpto3.Count() > 50)
                                        {
                                            cpto3 = Truncate(cpto3, 50);
                                        }
                                    }
                                }
                                #endregion

                                if (cuentaBancariaEK.moneda == 1)
                                {
                                    #region PESOS
                                    var cargos = listaFacturas.Sum(x => x.monto);
                                    var abonos = cargos * -1;

                                    #region sc_polizas
                                    using (var cmd = new OdbcCommand(@"
                                    INSERT INTO sc_polizas 
                                    (year, mes, poliza, tp, fechapol, cargos, abonos, generada, status, error, status_lock, fec_hora_movto, usuario_movto, fecha_hora_crea, usuario_crea, socio_inversionista, status_carga_pol, concepto)
                                    VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)"))
                                    {
                                        OdbcParameterCollection parameters = cmd.Parameters;

                                        parameters.Add("@year", OdbcType.Numeric).Value = DateTime.Now.Year;
                                        parameters.Add("@mes", OdbcType.Numeric).Value = DateTime.Now.Month;
                                        parameters.Add("@poliza", OdbcType.Numeric).Value = siguienteNumeroCheque;
                                        parameters.Add("@tp", OdbcType.Char).Value = cuentaBancariaEK.tp;
                                        parameters.Add("@fechapol", OdbcType.Date).Value = DateTime.Now;
                                        parameters.Add("@cargos", OdbcType.Numeric).Value = cargos;
                                        parameters.Add("@abonos", OdbcType.Numeric).Value = abonos;
                                        parameters.Add("@generada", OdbcType.Char).Value = "B";
                                        parameters.Add("@status", OdbcType.Char).Value = "C";
                                        parameters.Add("@error", OdbcType.Char).Value = (object)DBNull.Value;
                                        parameters.Add("@status_lock", OdbcType.Char).Value = "N";
                                        parameters.Add("@fec_hora_movto", OdbcType.DateTime).Value = DateTime.Now;
                                        parameters.Add("@usuario_movto", OdbcType.Char).Value = (object)DBNull.Value;
                                        parameters.Add("@fecha_hora_crea", OdbcType.DateTime).Value = DateTime.Now;
                                        parameters.Add("@usuario_crea", OdbcType.Char).Value = relacionUsuarioSIGOPLANEK.empleado.ToString();
                                        parameters.Add("@socio_inversionista", OdbcType.Numeric).Value = (object)DBNull.Value;
                                        parameters.Add("@status_carga_pol", OdbcType.Char).Value = (object)DBNull.Value;
                                        parameters.Add("@concepto", OdbcType.VarChar).Value = (object)DBNull.Value;

                                        cmd.Connection = trans.Connection;
                                        cmd.Transaction = trans;
                                        count += cmd.ExecuteNonQuery();
                                    }
                                    #endregion

                                    #region Obtener el centro de costo para el registro general del cheque.
                                    var listaFacturasCC = listaFacturas.Select(x => x.cc).ToList();
                                    var ccCheque = "";
                                    var flagValorDistinto = false;
                                    var primerCC = listaFacturasCC[0];

                                    foreach (var cc in listaFacturasCC)
                                    {
                                        if (cc != primerCC)
                                        {
                                            flagValorDistinto = true;
                                        }
                                    }

                                    if (flagValorDistinto)
                                    {
                                        ccCheque = "*";
                                    }
                                    else
                                    {
                                        ccCheque = primerCC;
                                    }
                                    #endregion

                                    #region sb_cheques
                                    using (var cmd = new OdbcCommand(@"
                                    INSERT INTO sb_cheques (cuenta, fecha_mov, tm, numero, tipocheque, descripcion, cc, monto, hecha_por, status_bco, status_lp, num_pro_emp, cpto1, cpto2, cpto3, iyear, imes, ipoliza, itp, ilinea, tp,
                                    fecha_reten, [desc], status_transf_cash, id_empleado_firma, id_empleado_firma2, fecha_reten_fin, firma1, fecha_firma1, firma2, fecha_firma2, firma3, fecha_firma3, clave_sub_tm, ruta_comprobantebco_pdf)
                                    VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)"))
                                    {
                                        OdbcParameterCollection parameters = cmd.Parameters;

                                        parameters.Add("@cuenta", OdbcType.Numeric).Value = cuentaBancaria;
                                        parameters.Add("@fecha_mov", OdbcType.Date).Value = DateTime.Now;
                                        parameters.Add("@tm", OdbcType.Numeric).Value = tm_banco;
                                        parameters.Add("@numero", OdbcType.Numeric).Value = siguienteNumeroCheque;
                                        parameters.Add("@tipocheque", OdbcType.Char).Value = "P";
                                        parameters.Add("@descripcion", OdbcType.Char).Value = proveedor.nombre;
                                        parameters.Add("@cc", OdbcType.Char).Value = ccCheque;
                                        parameters.Add("@monto", OdbcType.Numeric).Value = cargos;
                                        parameters.Add("@hecha_por", OdbcType.Char).Value = "scp";
                                        parameters.Add("@status_bco", OdbcType.Char).Value = "";
                                        parameters.Add("@status_lp", OdbcType.Char).Value = "";
                                        parameters.Add("@num_pro_emp", OdbcType.Numeric).Value = grpFac.numpro;
                                        parameters.Add("@cpto1", OdbcType.Char).Value = cpto1;
                                        parameters.Add("@cpto2", OdbcType.Char).Value = cpto2;
                                        parameters.Add("@cpto3", OdbcType.Char).Value = cpto3;
                                        parameters.Add("@iyear", OdbcType.Numeric).Value = DateTime.Now.Year;
                                        parameters.Add("@imes", OdbcType.Numeric).Value = DateTime.Now.Month;
                                        parameters.Add("@ipoliza", OdbcType.Numeric).Value = siguienteNumeroCheque;
                                        parameters.Add("@itp", OdbcType.Char).Value = cuentaBancariaEK.tp;
                                        parameters.Add("@ilinea", OdbcType.Numeric).Value = 1;
                                        parameters.Add("@tp", OdbcType.Char).Value = cuentaBancariaEK.tp;
                                        parameters.Add("@fecha_reten", OdbcType.Date).Value = (object)DBNull.Value;
                                        parameters.Add("@desc", OdbcType.Char).Value = (object)DBNull.Value;
                                        parameters.Add("@status_transf_cash", OdbcType.Char).Value = "N";
                                        parameters.Add("@id_empleado_firma", OdbcType.Numeric).Value = (object)DBNull.Value;
                                        parameters.Add("@id_empleado_firma2", OdbcType.Numeric).Value = (object)DBNull.Value;
                                        parameters.Add("@fecha_reten_fin", OdbcType.Date).Value = (object)DBNull.Value;
                                        parameters.Add("@firma1", OdbcType.Numeric).Value = (object)DBNull.Value;
                                        parameters.Add("@fecha_firma1", OdbcType.Date).Value = (object)DBNull.Value;
                                        parameters.Add("@firma2", OdbcType.Numeric).Value = (object)DBNull.Value;
                                        parameters.Add("@fecha_firma2", OdbcType.Date).Value = (object)DBNull.Value;
                                        parameters.Add("@firma3", OdbcType.Numeric).Value = (object)DBNull.Value;
                                        parameters.Add("@fecha_firma3", OdbcType.Date).Value = (object)DBNull.Value;
                                        parameters.Add("@clave_sub_tm", OdbcType.Numeric).Value = (object)DBNull.Value;
                                        parameters.Add("@ruta_comprobantebco_pdf", OdbcType.VarChar).Value = (object)DBNull.Value;

                                        cmd.Connection = trans.Connection;
                                        cmd.Transaction = trans;
                                        count += cmd.ExecuteNonQuery();
                                    }
                                    #endregion

                                    #region Actualizar registro de la tabla sb_cuenta para el campo ult_cheq_electronico
                                    using (var cmd = new OdbcCommand(@"
                                        UPDATE sb_cuenta
                                        SET ult_cheq_electronico = ?
                                        WHERE cuenta = ?"))
                                    {
                                        OdbcParameterCollection parameters = cmd.Parameters;

                                        parameters.Add("@ult_cheq_electronico", OdbcType.Numeric).Value = siguienteNumeroCheque;

                                        parameters.Add("@cuenta", OdbcType.Numeric).Value = cuentaBancaria;

                                        cmd.Connection = trans.Connection;
                                        cmd.Transaction = trans;
                                        count += cmd.ExecuteNonQuery();
                                    }
                                    #endregion

                                    var contadorLineaMovPol = 1;

                                    var listaFacturasPorCC = listaFacturas.GroupBy(x => x.cc).Select(x => new { cc = x.Key, grp = x }).ToList();

                                    foreach (var grupoCC in listaFacturasPorCC)
                                    {
                                        var facturasCC = grupoCC.grp.ToList();
                                        var listaComprasEK = _contextEnkontrol.Select<OrdenCompraDTO>(getEnkontrolAmbienteConsulta(), new OdbcConsultaDTO
                                        {
                                            consulta = string.Format(@"SELECT cc, numero, tipo_cambio FROM so_orden_compra WHERE cc = '{0}'", grupoCC.cc)
                                        });

                                        #region sc_movpol Registro Abono (Sumatoria - Monto Negativo)
                                        using (var cmd = new OdbcCommand(@"
                                    INSERT INTO sc_movpol 
                                    (year, mes, poliza, tp, linea, cta, scta, sscta, digito, tm, referencia, cc, concepto, monto, iclave, itm, st_par, orden_compra, numpro, socio_inversionista, istm, folio_imp, linea_imp, num_emp, folio_gxc,
                                    cfd_ruta_pdf, cfd_ruta_xml, UUID, cfd_rfc, cfd_tipocambio, cfd_total, cfd_moneda, metodo_pago_sat, ruta_comp_ext, factura_comp_ext, taxid, forma_pago, cfd_fecha_expedicion, cfd_tipocomprobante, cfd_metodo_pago_sat, area, cuenta_oc)
                                    VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)"))
                                        {
                                            OdbcParameterCollection parameters = cmd.Parameters;

                                            parameters.Add("@year", OdbcType.Numeric).Value = DateTime.Now.Year;
                                            parameters.Add("@mes", OdbcType.Numeric).Value = DateTime.Now.Month;
                                            parameters.Add("@poliza", OdbcType.Numeric).Value = siguienteNumeroCheque;
                                            parameters.Add("@tp", OdbcType.Char).Value = cuentaBancariaEK.tp;
                                            parameters.Add("@linea", OdbcType.Numeric).Value = contadorLineaMovPol;
                                            parameters.Add("@cta", OdbcType.Numeric).Value = cuentaBancariaEK.cta;
                                            parameters.Add("@scta", OdbcType.Numeric).Value = cuentaBancariaEK.scta;
                                            parameters.Add("@sscta", OdbcType.Numeric).Value = cuentaBancariaEK.sscta;
                                            parameters.Add("@digito", OdbcType.Numeric).Value = 0;
                                            parameters.Add("@tm", OdbcType.Numeric).Value = 2;
                                            parameters.Add("@referencia", OdbcType.Char).Value = siguienteNumeroCheque;
                                            parameters.Add("@cc", OdbcType.Char).Value = grupoCC.cc;
                                            parameters.Add("@concepto", OdbcType.Char).Value = proveedor.nombre;
                                            parameters.Add("@monto", OdbcType.Numeric).Value = facturasCC.Sum(x => x.monto) * -1;
                                            parameters.Add("@iclave", OdbcType.Numeric).Value = 0;
                                            parameters.Add("@itm", OdbcType.Numeric).Value = 59;
                                            parameters.Add("@st_par", OdbcType.Char).Value = (object)DBNull.Value;
                                            parameters.Add("@orden_compra", OdbcType.Numeric).Value = facturasCC[0].referenciaoc;
                                            parameters.Add("@numpro", OdbcType.Numeric).Value = 0;
                                            parameters.Add("@socio_inversionista", OdbcType.Numeric).Value = (object)DBNull.Value;
                                            parameters.Add("@istm", OdbcType.Numeric).Value = 0;
                                            parameters.Add("@folio_imp", OdbcType.Numeric).Value = (object)DBNull.Value;
                                            parameters.Add("@linea_imp", OdbcType.Numeric).Value = (object)DBNull.Value;
                                            parameters.Add("@num_emp", OdbcType.Numeric).Value = (object)DBNull.Value;
                                            parameters.Add("@folio_gxc", OdbcType.Numeric).Value = (object)DBNull.Value;
                                            parameters.Add("@cfd_ruta_pdf", OdbcType.VarChar).Value = (object)DBNull.Value;
                                            parameters.Add("@cfd_ruta_xml", OdbcType.VarChar).Value = (object)DBNull.Value;
                                            parameters.Add("@UUID", OdbcType.VarChar).Value = (object)DBNull.Value;
                                            parameters.Add("@cfd_rfc", OdbcType.VarChar).Value = (object)DBNull.Value;
                                            parameters.Add("@cfd_tipocambio", OdbcType.Numeric).Value = (object)DBNull.Value;
                                            parameters.Add("@cfd_total", OdbcType.Numeric).Value = (object)DBNull.Value;
                                            parameters.Add("@cfd_moneda", OdbcType.Char).Value = (object)DBNull.Value;
                                            parameters.Add("@metodo_pago_sat", OdbcType.Numeric).Value = (object)DBNull.Value;
                                            parameters.Add("@ruta_comp_ext", OdbcType.Char).Value = (object)DBNull.Value;
                                            parameters.Add("@factura_comp_ext", OdbcType.Char).Value = (object)DBNull.Value;
                                            parameters.Add("@taxid", OdbcType.Char).Value = (object)DBNull.Value;
                                            parameters.Add("@forma_pago", OdbcType.VarChar).Value = "TRANSFERENCIA BANCARIA";
                                            parameters.Add("@cfd_fecha_expedicion", OdbcType.DateTime).Value = (object)DBNull.Value;
                                            parameters.Add("@cfd_tipocomprobante", OdbcType.VarChar).Value = (object)DBNull.Value;
                                            parameters.Add("@cfd_metodo_pago_sat", OdbcType.VarChar).Value = (object)DBNull.Value;

                                            if (vSesiones.sesionEmpresaActual == 1)
                                            {
                                                parameters.Add("@area", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                parameters.Add("@cuenta_oc", OdbcType.Numeric).Value = (object)DBNull.Value;
                                            }
                                            else
                                            {
                                                //var areaCuenta = listaAreasCuentas.FirstOrDefault(x => x.centro_costo == grupoCC.cc);

                                                //if (areaCuenta == null)
                                                //{
                                                //    throw new Exception("No se encuentra la relación de área-cuenta con el cc \"" + grupoCC.cc + "\".");
                                                //}
                                                var ordenCompraEkDetalle = _contextEnkontrol.Select<OrdenCompraDTO>(getEnkontrolAmbienteConsulta(), new OdbcConsultaDTO
                                                {
                                                    consulta = string.Format(@"SELECT TOP 1 area, cuenta FROM so_orden_compra_det WHERE cc = '{0}' AND numero = {1}", grupoCC.cc, facturasCC[0].referenciaoc)
                                                }).FirstOrDefault();

                                                if (ordenCompraEkDetalle == null)
                                                {
                                                    throw new Exception(string.Format(@"No se encuentra el detalle de la orden de compra {0}-{1}", grupoCC.cc, facturasCC[0].referenciaoc));
                                                }

                                                //parameters.Add("@area", OdbcType.Numeric).Value = areaCuenta.area;
                                                //parameters.Add("@cuenta_oc", OdbcType.Numeric).Value = areaCuenta.cuenta;
                                                parameters.Add("@area", OdbcType.Numeric).Value = ordenCompraEkDetalle.area;
                                                parameters.Add("@cuenta_oc", OdbcType.Numeric).Value = ordenCompraEkDetalle.cuenta;
                                            }

                                            cmd.Connection = trans.Connection;
                                            cmd.Transaction = trans;
                                            count += cmd.ExecuteNonQuery();
                                        }

                                        contadorLineaMovPol++;
                                        #endregion

                                        foreach (var fac in facturasCC)
                                        {
                                            var ordenCompraEK = listaComprasEK.FirstOrDefault(x => x.numero.ToString() == fac.referenciaoc);

                                            if (ordenCompraEK == null)
                                            {
                                                throw new Exception(string.Format(@"No se encuentra la orden de compra {0}-{1} para la factura {2}", fac.cc, fac.referenciaoc, fac.factura));
                                            }

                                            var ordenCompraEkDetalle = _contextEnkontrol.Select<OrdenCompraDTO>(getEnkontrolAmbienteConsulta(), new OdbcConsultaDTO
                                            {
                                                consulta = string.Format(@"SELECT TOP 1 area, cuenta FROM so_orden_compra_det WHERE cc = '{0}' AND numero = {1}", ordenCompraEK.cc, ordenCompraEK.numero)
                                            }).FirstOrDefault();

                                            if (ordenCompraEkDetalle == null)
                                            {
                                                throw new Exception(string.Format(@"No se encuentra el detalle de la orden de compra {0}-{1} para la factura {2}", fac.cc, fac.referenciaoc, fac.factura));
                                            }

                                            #region sc_movpol Registro Cargo (Monto Positivo)
                                            var contadorMovPolCargoLinea = contadorLineaMovPol;

                                            using (var cmd = new OdbcCommand(@"
                                            INSERT INTO sc_movpol 
                                            (year, mes, poliza, tp, linea, cta, scta, sscta, digito, tm, referencia, cc, concepto, monto, iclave, itm, st_par, orden_compra, numpro, socio_inversionista, istm, folio_imp, linea_imp, num_emp, folio_gxc,
                                            cfd_ruta_pdf, cfd_ruta_xml, UUID, cfd_rfc, cfd_tipocambio, cfd_total, cfd_moneda, metodo_pago_sat, ruta_comp_ext, factura_comp_ext, taxid, forma_pago, cfd_fecha_expedicion, cfd_tipocomprobante, cfd_metodo_pago_sat, area, cuenta_oc)
                                            VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)"))
                                            {
                                                OdbcParameterCollection parameters = cmd.Parameters;

                                                parameters.Add("@year", OdbcType.Numeric).Value = DateTime.Now.Year;
                                                parameters.Add("@mes", OdbcType.Numeric).Value = DateTime.Now.Month;
                                                parameters.Add("@poliza", OdbcType.Numeric).Value = siguienteNumeroCheque;
                                                parameters.Add("@tp", OdbcType.Char).Value = cuentaBancariaEK.tp;
                                                parameters.Add("@linea", OdbcType.Numeric).Value = contadorLineaMovPol;
                                                parameters.Add("@cta", OdbcType.Numeric).Value = 2105;
                                                parameters.Add("@scta", OdbcType.Numeric).Value = 1;
                                                parameters.Add("@sscta", OdbcType.Numeric).Value = 0;
                                                parameters.Add("@digito", OdbcType.Numeric).Value = 0;
                                                parameters.Add("@tm", OdbcType.Numeric).Value = 1;
                                                parameters.Add("@referencia", OdbcType.Char).Value = fac.factura;
                                                parameters.Add("@cc", OdbcType.Char).Value = fac.cc;
                                                parameters.Add("@concepto", OdbcType.Char).Value = string.Format(@"Ch:{0} Cta:{1}", siguienteNumeroCheque, cuentaBancariaEK.descripcion);
                                                parameters.Add("@monto", OdbcType.Numeric).Value = fac.monto;
                                                parameters.Add("@iclave", OdbcType.Numeric).Value = 0;
                                                parameters.Add("@itm", OdbcType.Numeric).Value = 51;
                                                parameters.Add("@st_par", OdbcType.Char).Value = (object)DBNull.Value;
                                                parameters.Add("@orden_compra", OdbcType.Numeric).Value = fac.referenciaoc;
                                                parameters.Add("@numpro", OdbcType.Numeric).Value = fac.numpro;
                                                parameters.Add("@socio_inversionista", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                parameters.Add("@istm", OdbcType.Numeric).Value = 0;
                                                parameters.Add("@folio_imp", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                parameters.Add("@linea_imp", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                parameters.Add("@num_emp", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                parameters.Add("@folio_gxc", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                parameters.Add("@cfd_ruta_pdf", OdbcType.VarChar).Value = (object)DBNull.Value;
                                                parameters.Add("@cfd_ruta_xml", OdbcType.VarChar).Value = (object)DBNull.Value;
                                                parameters.Add("@UUID", OdbcType.VarChar).Value = (object)DBNull.Value;
                                                parameters.Add("@cfd_rfc", OdbcType.VarChar).Value = (object)DBNull.Value;
                                                parameters.Add("@cfd_tipocambio", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                parameters.Add("@cfd_total", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                parameters.Add("@cfd_moneda", OdbcType.Char).Value = (object)DBNull.Value;
                                                parameters.Add("@metodo_pago_sat", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                parameters.Add("@ruta_comp_ext", OdbcType.Char).Value = (object)DBNull.Value;
                                                parameters.Add("@factura_comp_ext", OdbcType.Char).Value = (object)DBNull.Value;
                                                parameters.Add("@taxid", OdbcType.Char).Value = (object)DBNull.Value;
                                                parameters.Add("@forma_pago", OdbcType.VarChar).Value = "TRANSFERENCIA BANCARIA";
                                                parameters.Add("@cfd_fecha_expedicion", OdbcType.DateTime).Value = (object)DBNull.Value;
                                                parameters.Add("@cfd_tipocomprobante", OdbcType.VarChar).Value = (object)DBNull.Value;
                                                parameters.Add("@cfd_metodo_pago_sat", OdbcType.VarChar).Value = (object)DBNull.Value;

                                                if (vSesiones.sesionEmpresaActual == 1)
                                                {
                                                    parameters.Add("@area", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                    parameters.Add("@cuenta_oc", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                }
                                                else
                                                {
                                                    //var areaCuenta = listaAreasCuentas.FirstOrDefault(x => x.centro_costo == fac.cc);

                                                    //parameters.Add("@area", OdbcType.Numeric).Value = areaCuenta.area;
                                                    //parameters.Add("@cuenta_oc", OdbcType.Numeric).Value = areaCuenta.cuenta;
                                                    parameters.Add("@area", OdbcType.Numeric).Value = ordenCompraEkDetalle.area;
                                                    parameters.Add("@cuenta_oc", OdbcType.Numeric).Value = ordenCompraEkDetalle.cuenta;
                                                }

                                                cmd.Connection = trans.Connection;
                                                cmd.Transaction = trans;
                                                count += cmd.ExecuteNonQuery();
                                            }

                                            contadorLineaMovPol++;
                                            #endregion

                                            #region sp_movprov
                                            var autoincremento = _contextEnkontrol.Select<sp_movprovDTO>(getEnkontrolAmbienteConsulta(), new OdbcConsultaDTO
                                            {
                                                consulta = string.Format(@"SELECT TOP 1 autoincremento FROM sp_movprov ORDER BY autoincremento DESC")
                                            }).FirstOrDefault();

                                            using (var cmd = new OdbcCommand(@"
                                            INSERT INTO sp_movprov 
                                            (numpro, factura, fecha, tm, fechavenc, concepto, cc, referenciaoc, monto, tipocambio, iva, year, mes, poliza, tp, linea, generado, es_factura, moneda, autorizapago, total, st_pago, folio, autoincremento,
                                            tipocambio_oc, empleado_modifica, fecha_modifica, hora_modifica, pago_factoraje, inst_factoraje, st_factoraje, socio_inversionista, bit_factoraje, bit_autoriza, bit_transferida, bit_pagada, empleado,
                                            folio_gxc, cfd_serie, cfd_folio, cfd_fecha, cfd_certificado, cfd_ano_aprob, cfd_num_aprob, ruta_rec_xml, ruta_rec_pdf, afectacompra, val_ref, suma_o_resta, pide_iva, valida_recibido, valida_almacen,
                                            valida_recibido_autorizar, empleado_autorizo, fecha_autoriza, empleado_vobo, fecha_vobo, tipo_factoraje, UUID, folio_retencion, cfd_metodo_pago, moneda_oc_nom, moneda_oc)
                                            VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)"))
                                            {
                                                OdbcParameterCollection parameters = cmd.Parameters;

                                                parameters.Add("@numpro", OdbcType.Numeric).Value = fac.numpro;
                                                parameters.Add("@factura", OdbcType.Numeric).Value = fac.factura;
                                                parameters.Add("@fecha", OdbcType.Date).Value = DateTime.Now;
                                                parameters.Add("@tm", OdbcType.Numeric).Value = 51; //parameters.Add("@tm", OdbcType.Numeric).Value = fac.tm;
                                                parameters.Add("@fechavenc", OdbcType.Date).Value = DateTime.Now;
                                                parameters.Add("@concepto", OdbcType.Char).Value = string.Format(@"CHEQUE: {0} : {1}", siguienteNumeroCheque, bancoEK.bancoDesc);
                                                parameters.Add("@cc", OdbcType.Char).Value = fac.cc;
                                                parameters.Add("@referenciaoc", OdbcType.Char).Value = fac.referenciaoc;
                                                parameters.Add("@monto", OdbcType.Numeric).Value = fac.monto * -1;
                                                parameters.Add("@tipocambio", OdbcType.Numeric).Value = ordenCompraEK.tipo_cambio;
                                                parameters.Add("@iva", OdbcType.Numeric).Value = 0;
                                                parameters.Add("@year", OdbcType.Numeric).Value = DateTime.Now.Year;
                                                parameters.Add("@mes", OdbcType.Numeric).Value = DateTime.Now.Month;
                                                parameters.Add("@poliza", OdbcType.Numeric).Value = siguienteNumeroCheque;
                                                parameters.Add("@tp", OdbcType.Char).Value = cuentaBancariaEK.tp;
                                                parameters.Add("@linea", OdbcType.Numeric).Value = contadorMovPolCargoLinea;
                                                parameters.Add("@generado", OdbcType.Char).Value = "B";
                                                parameters.Add("@es_factura", OdbcType.Char).Value = "N";
                                                parameters.Add("@moneda", OdbcType.Char).Value = cuentaBancariaEK.moneda;
                                                parameters.Add("@autorizapago", OdbcType.Char).Value = "";
                                                parameters.Add("@total", OdbcType.Numeric).Value = fac.monto * -1;
                                                parameters.Add("@st_pago", OdbcType.Char).Value = "";
                                                parameters.Add("@folio", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                parameters.Add("@autoincremento", OdbcType.Numeric).Value = autoincremento.autoincremento + 1;
                                                parameters.Add("@tipocambio_oc", OdbcType.Numeric).Value = ordenCompraEK.tipo_cambio;
                                                parameters.Add("@empleado_modifica", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                parameters.Add("@fecha_modifica", OdbcType.Date).Value = (object)DBNull.Value;
                                                parameters.Add("@hora_modifica", OdbcType.Time).Value = (object)DBNull.Value;
                                                parameters.Add("@pago_factoraje", OdbcType.Char).Value = (object)DBNull.Value;
                                                parameters.Add("@inst_factoraje", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                parameters.Add("@st_factoraje", OdbcType.Char).Value = (object)DBNull.Value;
                                                parameters.Add("@socio_inversionista", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                parameters.Add("@bit_factoraje", OdbcType.Char).Value = "N";
                                                parameters.Add("@bit_autoriza", OdbcType.Char).Value = "N";
                                                parameters.Add("@bit_transferida", OdbcType.Char).Value = "N";
                                                parameters.Add("@bit_pagada", OdbcType.Char).Value = "N";
                                                parameters.Add("@empleado", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                parameters.Add("@folio_gxc", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                parameters.Add("@cfd_serie", OdbcType.VarChar).Value = (object)DBNull.Value;
                                                parameters.Add("@cfd_folio", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                parameters.Add("@cfd_fecha", OdbcType.DateTime).Value = (object)DBNull.Value;
                                                parameters.Add("@cfd_certificado", OdbcType.Char).Value = (object)DBNull.Value;
                                                parameters.Add("@cfd_ano_aprob", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                parameters.Add("@cfd_num_aprob", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                parameters.Add("@ruta_rec_xml", OdbcType.VarChar).Value = (object)DBNull.Value;
                                                parameters.Add("@ruta_rec_pdf", OdbcType.VarChar).Value = (object)DBNull.Value;
                                                parameters.Add("@afectacompra", OdbcType.Char).Value = "N";
                                                parameters.Add("@val_ref", OdbcType.Char).Value = " ";
                                                parameters.Add("@suma_o_resta", OdbcType.Numeric).Value = 0;
                                                parameters.Add("@pide_iva", OdbcType.Char).Value = "N";
                                                parameters.Add("@valida_recibido", OdbcType.Char).Value = " ";
                                                parameters.Add("@valida_almacen", OdbcType.Char).Value = "N";
                                                parameters.Add("@valida_recibido_autorizar", OdbcType.Char).Value = "N";
                                                parameters.Add("@empleado_autorizo", OdbcType.Numeric).Value = 0;
                                                parameters.Add("@fecha_autoriza", OdbcType.Date).Value = (object)DBNull.Value;
                                                parameters.Add("@empleado_vobo", OdbcType.Numeric).Value = 0;
                                                parameters.Add("@fecha_vobo", OdbcType.Date).Value = (object)DBNull.Value;
                                                parameters.Add("@tipo_factoraje", OdbcType.Char).Value = "N";
                                                parameters.Add("@UUID", OdbcType.VarChar).Value = (object)DBNull.Value;
                                                parameters.Add("@folio_retencion", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                parameters.Add("@cfd_metodo_pago", OdbcType.Char).Value = (object)DBNull.Value;
                                                parameters.Add("@moneda_oc_nom", OdbcType.Char).Value = (object)DBNull.Value;
                                                parameters.Add("@moneda_oc", OdbcType.Char).Value = (object)DBNull.Value;

                                                cmd.Connection = trans.Connection;
                                                cmd.Transaction = trans;
                                                count += cmd.ExecuteNonQuery();
                                            }
                                            #endregion
                                        }

                                        #region sb_edo_cta_chequera
                                        using (var cmd = new OdbcCommand(@"
                                            INSERT INTO sb_edo_cta_chequera 
                                            (cuenta, fecha_mov, tm, numero, cc, descripcion, monto, tc, origen_mov, generada, st_consilia, num_consilia, st_che, ref_che_inverso, ref_tm_inverso, motivo_cancelado, iyear, imes, ipoliza, itp, ilinea,
                                            banco, tp, [desc], folio, tipo_iva, porc_iva, monto_iva, folio_iva, fecha_reten, fecha_reten_fin, id_num_credito, prototipo, consecutivo_minist, acredita_iva, clave_sub_tm, folio_imp, linea_imp)
                                            VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)"))
                                        {
                                            OdbcParameterCollection parameters = cmd.Parameters;

                                            parameters.Add("@cuenta", OdbcType.Numeric).Value = cuentaBancaria;
                                            parameters.Add("@fecha_mov", OdbcType.Date).Value = DateTime.Now;
                                            parameters.Add("@tm", OdbcType.Numeric).Value = tm_banco;
                                            parameters.Add("@numero", OdbcType.Numeric).Value = siguienteNumeroCheque;
                                            parameters.Add("@cc", OdbcType.Char).Value = grupoCC.cc;
                                            parameters.Add("@descripcion", OdbcType.Char).Value = proveedor.nombre;
                                            parameters.Add("@monto", OdbcType.Numeric).Value = facturasCC.Sum(x => x.monto) * -1;
                                            parameters.Add("@tc", OdbcType.Numeric).Value = 1;
                                            parameters.Add("@origen_mov", OdbcType.Char).Value = "P";
                                            parameters.Add("@generada", OdbcType.Char).Value = "B";
                                            parameters.Add("@st_consilia", OdbcType.Char).Value = "";
                                            parameters.Add("@num_consilia", OdbcType.Numeric).Value = 0;
                                            parameters.Add("@st_che", OdbcType.Char).Value = "";
                                            parameters.Add("@ref_che_inverso", OdbcType.Numeric).Value = 0;
                                            parameters.Add("@ref_tm_inverso", OdbcType.Numeric).Value = 0;
                                            parameters.Add("@motivo_cancelado", OdbcType.Char).Value = "";
                                            parameters.Add("@iyear", OdbcType.Numeric).Value = DateTime.Now.Year;
                                            parameters.Add("@imes", OdbcType.Numeric).Value = DateTime.Now.Month;
                                            parameters.Add("@ipoliza", OdbcType.Numeric).Value = siguienteNumeroCheque;
                                            parameters.Add("@itp", OdbcType.Char).Value = cuentaBancariaEK.tp;
                                            parameters.Add("@ilinea", OdbcType.Numeric).Value = 1;
                                            parameters.Add("@banco", OdbcType.Numeric).Value = cuentaBancariaEK.banco;
                                            parameters.Add("@tp", OdbcType.Char).Value = (object)DBNull.Value;
                                            parameters.Add("@desc", OdbcType.Char).Value = (object)DBNull.Value;
                                            parameters.Add("@folio", OdbcType.Numeric).Value = (object)DBNull.Value;
                                            parameters.Add("@tipo_iva", OdbcType.Char).Value = (object)DBNull.Value;
                                            parameters.Add("@porc_iva", OdbcType.Numeric).Value = (object)DBNull.Value;
                                            parameters.Add("@monto_iva", OdbcType.Numeric).Value = (object)DBNull.Value;
                                            parameters.Add("@folio_iva", OdbcType.Numeric).Value = (object)DBNull.Value;
                                            parameters.Add("@fecha_reten", OdbcType.Date).Value = (object)DBNull.Value;
                                            parameters.Add("@fecha_reten_fin", OdbcType.Date).Value = (object)DBNull.Value;
                                            parameters.Add("@id_num_credito", OdbcType.Numeric).Value = (object)DBNull.Value;
                                            parameters.Add("@prototipo", OdbcType.Char).Value = (object)DBNull.Value;
                                            parameters.Add("@consecutivo_minist", OdbcType.Numeric).Value = (object)DBNull.Value;
                                            parameters.Add("@acredita_iva", OdbcType.Char).Value = (object)DBNull.Value;
                                            parameters.Add("@clave_sub_tm", OdbcType.Numeric).Value = (object)DBNull.Value;
                                            parameters.Add("@folio_imp", OdbcType.Numeric).Value = (object)DBNull.Value;
                                            parameters.Add("@linea_imp", OdbcType.Numeric).Value = (object)DBNull.Value;

                                            cmd.Connection = trans.Connection;
                                            cmd.Transaction = trans;
                                            count += cmd.ExecuteNonQuery();
                                        }
                                        #endregion
                                    }
                                    #endregion

                                    #region Registros de IVA
                                    var listaMovimientosPolizaIVA = new List<MovimientoPolizaDTO>();

                                    foreach (var fac in listaFacturas)
                                    {
                                        var movimientoPolizaFactura = listaMovimientoPolizaFactura.FirstOrDefault(x => x.referencia == fac.factura.ToString() && x.numpro == grpFac.numpro);

                                        if (movimientoPolizaFactura == null) //Si no encuentra un registro en la lista precargada lo busca directamente en la base de datos.
                                        {
                                            movimientoPolizaFactura = _contextEnkontrol.Select<MovimientoPolizaDTO>(getEnkontrolAmbienteConsulta(), new OdbcConsultaDTO
                                            {
                                                consulta = string.Format(@"SELECT referencia, numpro, poliza FROM sc_movpol WHERE tp = '07' AND numpro = {0} AND referencia = '{1}'", grpFac.numpro, fac.factura)
                                            }).FirstOrDefault();
                                        }

                                        if (movimientoPolizaFactura != null)
                                        {
                                            var movimientosPolizaIVA = listaMovimientoPolizaFacturaIVA.Where(x => x.referencia == fac.factura.ToString() && x.poliza == movimientoPolizaFactura.poliza).ToList();

                                            if (movimientosPolizaIVA.Count() == 0) //Si no encuentra un registro en la lista precargada lo busca directamente en la base de datos.
                                            {
                                                movimientosPolizaIVA = _contextEnkontrol.Select<MovimientoPolizaDTO>(getEnkontrolAmbienteConsulta(), new OdbcConsultaDTO
                                                {
                                                    consulta = string.Format(@"SELECT * FROM sc_movpol WHERE tp = '07' AND cta = 1146 AND referencia = '{0}' AND poliza = {1}", fac.factura, movimientoPolizaFactura.poliza)
                                                });
                                            }

                                            if (movimientosPolizaIVA.Count() > 0)
                                            {
                                                //var listaMovimientosProveedorFactura = _contextEnkontrol.Select<MovimientoProveedorDTO>(getEnkontrolAmbienteConsulta(), new OdbcConsultaDTO
                                                //{
                                                //    consulta = string.Format(@"SELECT * FROM sp_movprov WHERE numpro = {0} AND factura = {1} AND poliza != {2}", grpFac.numpro, fac.factura, siguienteNumeroCheque) //Se filtran los movimientos que tengan el "siguienteNumeroCheque" ya que es el registro que se acaba de guardar en el código de arriba.
                                                //});
                                                //decimal facturaNuevoMonto = listaMovimientosProveedorFactura.Sum(x => x.total);

                                                //if (facturaNuevoMonto > 0)
                                                //{
                                                    foreach (var mov in movimientosPolizaIVA)
                                                    {
                                                        //decimal montoIva = 0m;

                                                        //switch (mov.scta)
                                                        //{
                                                        //    case 1: //scta de IVA 15%
                                                        //        montoIva = facturaNuevoMonto * 0.15m;
                                                        //        break;
                                                        //    case 2: //scta de IVA 10%
                                                        //        montoIva = facturaNuevoMonto * 0.1m;
                                                        //        break;
                                                        //    case 4: //scta de IVA 16%
                                                        //        montoIva = facturaNuevoMonto * 0.16m;
                                                        //        break;
                                                        //    case 5: //scta de IVA 11%
                                                        //        montoIva = facturaNuevoMonto * 0.11m;
                                                        //        break;
                                                        //    case 9: //scta de IVA 8%
                                                        //        montoIva = facturaNuevoMonto * 0.08m;
                                                        //        break;
                                                        //    default:
                                                        //        throw new Exception("No se encuentra la scta del IVA. scta = " + mov.scta);
                                                        //}

                                                        //listaMovimientosPolizaIVA.Add(new MovimientoPolizaDTO
                                                        //{
                                                        //    cta = 0, //El campo cta no se ocupa en la lista ya que se coloca manualmente 1146 y 1147 en el guardado de los registros más adelante.
                                                        //    scta = mov.scta,
                                                        //    sscta = 0, //El campo sscta no se ocupa ya que el scta determina el tipo de IVA.
                                                        //    monto = (Math.Truncate(100 * montoIva) / 100)
                                                        //});

                                                        listaMovimientosPolizaIVA.Add(new MovimientoPolizaDTO
                                                        {
                                                            cta = 0, //El campo cta no se ocupa en la lista ya que se coloca manualmente 1146 y 1147 en el guardado de los registros más adelante.
                                                            scta = mov.scta,
                                                            sscta = mov.sscta,
                                                            monto = mov.monto
                                                        });
                                                    }
                                                //}
                                            }
                                        }
                                    }

                                    if (listaMovimientosPolizaIVA.Count() > 0)
                                    {
                                        var ultimoCC = listaFacturas.Last().cc;
                                        var listaMovimientosPolizaIVAAgrupada = listaMovimientosPolizaIVA.GroupBy(x => new { x.cta, x.scta, x.sscta }).Select(x => new
                                        {
                                            cta = x.Key.cta,
                                            scta = x.Key.scta,
                                            sscta = x.Key.sscta,
                                            montoTotal = x.Sum(y => y.monto)
                                        }).ToList();

                                        foreach (var cuenta in listaMovimientosPolizaIVAAgrupada)
                                        {
                                            if (cuenta.montoTotal > 0)
                                            {
                                                #region sc_movpol Registro RFC e IVA Cargo y Abono
                                                #region Cargo
                                                OrdenCompraDTO ordenCompraEkDetalle = null;
                                                using (var cmd = new OdbcCommand(@"
                                                        INSERT INTO sc_movpol 
                                                        (year, mes, poliza, tp, linea, cta, scta, sscta, digito, tm, referencia, cc, concepto, monto, iclave, itm, st_par, orden_compra, numpro, socio_inversionista, istm, folio_imp, linea_imp, num_emp, folio_gxc,
                                                        cfd_ruta_pdf, cfd_ruta_xml, UUID, cfd_rfc, cfd_tipocambio, cfd_total, cfd_moneda, metodo_pago_sat, ruta_comp_ext, factura_comp_ext, taxid, forma_pago, cfd_fecha_expedicion, cfd_tipocomprobante, cfd_metodo_pago_sat, area, cuenta_oc)
                                                        VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)"))
                                                {
                                                    OdbcParameterCollection parameters = cmd.Parameters;

                                                    parameters.Add("@year", OdbcType.Numeric).Value = DateTime.Now.Year;
                                                    parameters.Add("@mes", OdbcType.Numeric).Value = DateTime.Now.Month;
                                                    parameters.Add("@poliza", OdbcType.Numeric).Value = siguienteNumeroCheque;
                                                    parameters.Add("@tp", OdbcType.Char).Value = cuentaBancariaEK.tp;
                                                    parameters.Add("@linea", OdbcType.Numeric).Value = contadorLineaMovPol;
                                                    parameters.Add("@cta", OdbcType.Numeric).Value = 1147;
                                                    parameters.Add("@scta", OdbcType.Numeric).Value = cuenta.scta;
                                                    parameters.Add("@sscta", OdbcType.Numeric).Value = cuenta.sscta;
                                                    parameters.Add("@digito", OdbcType.Numeric).Value = 0;
                                                    parameters.Add("@tm", OdbcType.Numeric).Value = 1;
                                                    parameters.Add("@referencia", OdbcType.Char).Value = siguienteNumeroCheque;
                                                    parameters.Add("@cc", OdbcType.Char).Value = ultimoCC;
                                                    parameters.Add("@concepto", OdbcType.Char).Value = proveedor.rfc ?? "";
                                                    parameters.Add("@monto", OdbcType.Numeric).Value = cuenta.montoTotal;
                                                    parameters.Add("@iclave", OdbcType.Numeric).Value = 0;
                                                    parameters.Add("@itm", OdbcType.Numeric).Value = 0;
                                                    parameters.Add("@st_par", OdbcType.Char).Value = (object)DBNull.Value;
                                                    parameters.Add("@orden_compra", OdbcType.Numeric).Value = 0;
                                                    parameters.Add("@numpro", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                    parameters.Add("@socio_inversionista", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                    parameters.Add("@istm", OdbcType.Numeric).Value = 0;
                                                    parameters.Add("@folio_imp", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                    parameters.Add("@linea_imp", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                    parameters.Add("@num_emp", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                    parameters.Add("@folio_gxc", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                    parameters.Add("@cfd_ruta_pdf", OdbcType.VarChar).Value = (object)DBNull.Value;
                                                    parameters.Add("@cfd_ruta_xml", OdbcType.VarChar).Value = (object)DBNull.Value;
                                                    parameters.Add("@UUID", OdbcType.VarChar).Value = (object)DBNull.Value;
                                                    parameters.Add("@cfd_rfc", OdbcType.VarChar).Value = (object)DBNull.Value;
                                                    parameters.Add("@cfd_tipocambio", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                    parameters.Add("@cfd_total", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                    parameters.Add("@cfd_moneda", OdbcType.Char).Value = (object)DBNull.Value;
                                                    parameters.Add("@metodo_pago_sat", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                    parameters.Add("@ruta_comp_ext", OdbcType.Char).Value = (object)DBNull.Value;
                                                    parameters.Add("@factura_comp_ext", OdbcType.Char).Value = (object)DBNull.Value;
                                                    parameters.Add("@taxid", OdbcType.Char).Value = (object)DBNull.Value;
                                                    parameters.Add("@forma_pago", OdbcType.VarChar).Value = (object)DBNull.Value;
                                                    parameters.Add("@cfd_fecha_expedicion", OdbcType.DateTime).Value = (object)DBNull.Value;
                                                    parameters.Add("@cfd_tipocomprobante", OdbcType.VarChar).Value = (object)DBNull.Value;
                                                    parameters.Add("@cfd_metodo_pago_sat", OdbcType.VarChar).Value = (object)DBNull.Value;

                                                    if (vSesiones.sesionEmpresaActual == 1)
                                                    {
                                                        parameters.Add("@area", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                        parameters.Add("@cuenta_oc", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                    }
                                                    else
                                                    {
                                                        //var areaCuenta = listaAreasCuentas.FirstOrDefault(x => x.centro_costo == ultimoCC);

                                                        //parameters.Add("@area", OdbcType.Numeric).Value = areaCuenta.area;
                                                        //parameters.Add("@cuenta_oc", OdbcType.Numeric).Value = areaCuenta.cuenta;

                                                        ordenCompraEkDetalle = _contextEnkontrol.Select<OrdenCompraDTO>(getEnkontrolAmbienteConsulta(), new OdbcConsultaDTO
                                                        {
                                                            consulta = string.Format(@"SELECT TOP 1 area, cuenta FROM so_orden_compra_det WHERE cc = '{0}' AND numero = {1}", listaFacturas.Last().cc, listaFacturas.Last().referenciaoc)
                                                        }).FirstOrDefault();

                                                        if (ordenCompraEkDetalle == null)
                                                        {
                                                            throw new Exception(string.Format(@"No se encuentra el detalle de la orden de compra {0}-{1}", listaFacturas.Last().cc, listaFacturas.Last().referenciaoc));
                                                        }

                                                        parameters.Add("@area", OdbcType.Numeric).Value = ordenCompraEkDetalle.area;
                                                        parameters.Add("@cuenta_oc", OdbcType.Numeric).Value = ordenCompraEkDetalle.cuenta;
                                                    }

                                                    cmd.Connection = trans.Connection;
                                                    cmd.Transaction = trans;
                                                    count += cmd.ExecuteNonQuery();
                                                }

                                                contadorLineaMovPol++;
                                                #endregion

                                                #region Abono
                                                using (var cmd = new OdbcCommand(@"
                                                        INSERT INTO sc_movpol 
                                                        (year, mes, poliza, tp, linea, cta, scta, sscta, digito, tm, referencia, cc, concepto, monto, iclave, itm, st_par, orden_compra, numpro, socio_inversionista, istm, folio_imp, linea_imp, num_emp, folio_gxc,
                                                        cfd_ruta_pdf, cfd_ruta_xml, UUID, cfd_rfc, cfd_tipocambio, cfd_total, cfd_moneda, metodo_pago_sat, ruta_comp_ext, factura_comp_ext, taxid, forma_pago, cfd_fecha_expedicion, cfd_tipocomprobante, cfd_metodo_pago_sat, area, cuenta_oc)
                                                        VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)"))
                                                {
                                                    OdbcParameterCollection parameters = cmd.Parameters;

                                                    parameters.Add("@year", OdbcType.Numeric).Value = DateTime.Now.Year;
                                                    parameters.Add("@mes", OdbcType.Numeric).Value = DateTime.Now.Month;
                                                    parameters.Add("@poliza", OdbcType.Numeric).Value = siguienteNumeroCheque;
                                                    parameters.Add("@tp", OdbcType.Char).Value = cuentaBancariaEK.tp;
                                                    parameters.Add("@linea", OdbcType.Numeric).Value = contadorLineaMovPol;
                                                    parameters.Add("@cta", OdbcType.Numeric).Value = 1146;
                                                    parameters.Add("@scta", OdbcType.Numeric).Value = cuenta.scta;
                                                    parameters.Add("@sscta", OdbcType.Numeric).Value = cuenta.sscta;
                                                    parameters.Add("@digito", OdbcType.Numeric).Value = 0;
                                                    parameters.Add("@tm", OdbcType.Numeric).Value = 2;
                                                    parameters.Add("@referencia", OdbcType.Char).Value = siguienteNumeroCheque;
                                                    parameters.Add("@cc", OdbcType.Char).Value = ultimoCC;
                                                    parameters.Add("@concepto", OdbcType.Char).Value = proveedor.rfc ?? "";
                                                    parameters.Add("@monto", OdbcType.Numeric).Value = cuenta.montoTotal * -1;
                                                    parameters.Add("@iclave", OdbcType.Numeric).Value = 0;
                                                    parameters.Add("@itm", OdbcType.Numeric).Value = 0;
                                                    parameters.Add("@st_par", OdbcType.Char).Value = (object)DBNull.Value;
                                                    parameters.Add("@orden_compra", OdbcType.Numeric).Value = 0;
                                                    parameters.Add("@numpro", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                    parameters.Add("@socio_inversionista", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                    parameters.Add("@istm", OdbcType.Numeric).Value = 0;
                                                    parameters.Add("@folio_imp", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                    parameters.Add("@linea_imp", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                    parameters.Add("@num_emp", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                    parameters.Add("@folio_gxc", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                    parameters.Add("@cfd_ruta_pdf", OdbcType.VarChar).Value = (object)DBNull.Value;
                                                    parameters.Add("@cfd_ruta_xml", OdbcType.VarChar).Value = (object)DBNull.Value;
                                                    parameters.Add("@UUID", OdbcType.VarChar).Value = (object)DBNull.Value;
                                                    parameters.Add("@cfd_rfc", OdbcType.VarChar).Value = (object)DBNull.Value;
                                                    parameters.Add("@cfd_tipocambio", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                    parameters.Add("@cfd_total", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                    parameters.Add("@cfd_moneda", OdbcType.Char).Value = (object)DBNull.Value;
                                                    parameters.Add("@metodo_pago_sat", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                    parameters.Add("@ruta_comp_ext", OdbcType.Char).Value = (object)DBNull.Value;
                                                    parameters.Add("@factura_comp_ext", OdbcType.Char).Value = (object)DBNull.Value;
                                                    parameters.Add("@taxid", OdbcType.Char).Value = (object)DBNull.Value;
                                                    parameters.Add("@forma_pago", OdbcType.VarChar).Value = (object)DBNull.Value;
                                                    parameters.Add("@cfd_fecha_expedicion", OdbcType.DateTime).Value = (object)DBNull.Value;
                                                    parameters.Add("@cfd_tipocomprobante", OdbcType.VarChar).Value = (object)DBNull.Value;
                                                    parameters.Add("@cfd_metodo_pago_sat", OdbcType.VarChar).Value = (object)DBNull.Value;

                                                    if (vSesiones.sesionEmpresaActual == 1)
                                                    {
                                                        parameters.Add("@area", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                        parameters.Add("@cuenta_oc", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                    }
                                                    else
                                                    {
                                                        //var areaCuenta = listaAreasCuentas.FirstOrDefault(x => x.centro_costo == ultimoCC);

                                                        //parameters.Add("@area", OdbcType.Numeric).Value = areaCuenta.area;
                                                        //parameters.Add("@cuenta_oc", OdbcType.Numeric).Value = areaCuenta.cuenta;

                                                        parameters.Add("@area", OdbcType.Numeric).Value = ordenCompraEkDetalle.area;
                                                        parameters.Add("@cuenta_oc", OdbcType.Numeric).Value = ordenCompraEkDetalle.cuenta;
                                                    }

                                                    cmd.Connection = trans.Connection;
                                                    cmd.Transaction = trans;
                                                    count += cmd.ExecuteNonQuery();
                                                }

                                                contadorLineaMovPol++;
                                                #endregion
                                                #endregion
                                            }
                                        }
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region DÓlARES
                                    var ultimoTipo_cambioEK = _contextEnkontrol.Select<dynamic>(getEnkontrolAmbienteConsulta(), new OdbcConsultaDTO
                                    {
                                        consulta = @"SELECT TOP 1 tipo_cambio FROM tipo_cambio WHERE moneda = 2 ORDER BY fecha DESC"
                                    }).FirstOrDefault();

                                    var cargos = listaFacturas.Sum(x => x.monto) * ultimoTipo_cambioEK.tipo_cambio;
                                    var abonos = (cargos * -1) * ultimoTipo_cambioEK.tipo_cambio;

                                    #region sc_polizas
                                    using (var cmd = new OdbcCommand(@"
                                    INSERT INTO sc_polizas 
                                    (year, mes, poliza, tp, fechapol, cargos, abonos, generada, status, error, status_lock, fec_hora_movto, usuario_movto, fecha_hora_crea, usuario_crea, socio_inversionista, status_carga_pol, concepto)
                                    VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)"))
                                    {
                                        OdbcParameterCollection parameters = cmd.Parameters;

                                        parameters.Add("@year", OdbcType.Numeric).Value = DateTime.Now.Year;
                                        parameters.Add("@mes", OdbcType.Numeric).Value = DateTime.Now.Month;
                                        parameters.Add("@poliza", OdbcType.Numeric).Value = siguienteNumeroCheque;
                                        parameters.Add("@tp", OdbcType.Char).Value = cuentaBancariaEK.tp;
                                        parameters.Add("@fechapol", OdbcType.Date).Value = DateTime.Now;
                                        parameters.Add("@cargos", OdbcType.Numeric).Value = cargos;
                                        parameters.Add("@abonos", OdbcType.Numeric).Value = abonos;
                                        parameters.Add("@generada", OdbcType.Char).Value = "B";
                                        parameters.Add("@status", OdbcType.Char).Value = "C";
                                        parameters.Add("@error", OdbcType.Char).Value = (object)DBNull.Value;
                                        parameters.Add("@status_lock", OdbcType.Char).Value = "N";
                                        parameters.Add("@fec_hora_movto", OdbcType.DateTime).Value = DateTime.Now;
                                        parameters.Add("@usuario_movto", OdbcType.Char).Value = (object)DBNull.Value;
                                        parameters.Add("@fecha_hora_crea", OdbcType.DateTime).Value = DateTime.Now;
                                        parameters.Add("@usuario_crea", OdbcType.Char).Value = relacionUsuarioSIGOPLANEK.empleado.ToString();
                                        parameters.Add("@socio_inversionista", OdbcType.Numeric).Value = (object)DBNull.Value;
                                        parameters.Add("@status_carga_pol", OdbcType.Char).Value = (object)DBNull.Value;
                                        parameters.Add("@concepto", OdbcType.VarChar).Value = (object)DBNull.Value;

                                        cmd.Connection = trans.Connection;
                                        cmd.Transaction = trans;
                                        count += cmd.ExecuteNonQuery();
                                    }
                                    #endregion

                                    #region Obtener el centro de costo para el registro general del cheque.
                                    var listaFacturasCC = listaFacturas.Select(x => x.cc).ToList();
                                    var ccCheque = "";
                                    var flagValorDistinto = false;
                                    var primerCC = listaFacturasCC[0];

                                    foreach (var cc in listaFacturasCC)
                                    {
                                        if (cc != primerCC)
                                        {
                                            flagValorDistinto = true;
                                        }
                                    }

                                    if (flagValorDistinto)
                                    {
                                        ccCheque = "*";
                                    }
                                    else
                                    {
                                        ccCheque = primerCC;
                                    }
                                    #endregion

                                    #region sb_cheques
                                    using (var cmd = new OdbcCommand(@"
                                    INSERT INTO sb_cheques (cuenta, fecha_mov, tm, numero, tipocheque, descripcion, cc, monto, hecha_por, status_bco, status_lp, num_pro_emp, cpto1, cpto2, cpto3, iyear, imes, ipoliza, itp, ilinea, tp,
                                    fecha_reten, [desc], status_transf_cash, id_empleado_firma, id_empleado_firma2, fecha_reten_fin, firma1, fecha_firma1, firma2, fecha_firma2, firma3, fecha_firma3, clave_sub_tm, ruta_comprobantebco_pdf)
                                    VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)"))
                                    {
                                        OdbcParameterCollection parameters = cmd.Parameters;

                                        parameters.Add("@cuenta", OdbcType.Numeric).Value = cuentaBancaria;
                                        parameters.Add("@fecha_mov", OdbcType.Date).Value = DateTime.Now;
                                        parameters.Add("@tm", OdbcType.Numeric).Value = tm_banco;
                                        parameters.Add("@numero", OdbcType.Numeric).Value = siguienteNumeroCheque;
                                        parameters.Add("@tipocheque", OdbcType.Char).Value = "P";
                                        parameters.Add("@descripcion", OdbcType.Char).Value = proveedor.nombre;
                                        parameters.Add("@cc", OdbcType.Char).Value = ccCheque;
                                        parameters.Add("@monto", OdbcType.Numeric).Value = cargos;
                                        parameters.Add("@hecha_por", OdbcType.Char).Value = "scp";
                                        parameters.Add("@status_bco", OdbcType.Char).Value = "";
                                        parameters.Add("@status_lp", OdbcType.Char).Value = "";
                                        parameters.Add("@num_pro_emp", OdbcType.Numeric).Value = grpFac.numpro;
                                        parameters.Add("@cpto1", OdbcType.Char).Value = cpto1;
                                        parameters.Add("@cpto2", OdbcType.Char).Value = cpto2;
                                        parameters.Add("@cpto3", OdbcType.Char).Value = cpto3;
                                        parameters.Add("@iyear", OdbcType.Numeric).Value = DateTime.Now.Year;
                                        parameters.Add("@imes", OdbcType.Numeric).Value = DateTime.Now.Month;
                                        parameters.Add("@ipoliza", OdbcType.Numeric).Value = siguienteNumeroCheque;
                                        parameters.Add("@itp", OdbcType.Char).Value = cuentaBancariaEK.tp;
                                        parameters.Add("@ilinea", OdbcType.Numeric).Value = 1;
                                        parameters.Add("@tp", OdbcType.Char).Value = cuentaBancariaEK.tp;
                                        parameters.Add("@fecha_reten", OdbcType.Date).Value = (object)DBNull.Value;
                                        parameters.Add("@desc", OdbcType.Char).Value = (object)DBNull.Value;
                                        parameters.Add("@status_transf_cash", OdbcType.Char).Value = "N";
                                        parameters.Add("@id_empleado_firma", OdbcType.Numeric).Value = (object)DBNull.Value;
                                        parameters.Add("@id_empleado_firma2", OdbcType.Numeric).Value = (object)DBNull.Value;
                                        parameters.Add("@fecha_reten_fin", OdbcType.Date).Value = (object)DBNull.Value;
                                        parameters.Add("@firma1", OdbcType.Numeric).Value = (object)DBNull.Value;
                                        parameters.Add("@fecha_firma1", OdbcType.Date).Value = (object)DBNull.Value;
                                        parameters.Add("@firma2", OdbcType.Numeric).Value = (object)DBNull.Value;
                                        parameters.Add("@fecha_firma2", OdbcType.Date).Value = (object)DBNull.Value;
                                        parameters.Add("@firma3", OdbcType.Numeric).Value = (object)DBNull.Value;
                                        parameters.Add("@fecha_firma3", OdbcType.Date).Value = (object)DBNull.Value;
                                        parameters.Add("@clave_sub_tm", OdbcType.Numeric).Value = (object)DBNull.Value;
                                        parameters.Add("@ruta_comprobantebco_pdf", OdbcType.VarChar).Value = (object)DBNull.Value;

                                        cmd.Connection = trans.Connection;
                                        cmd.Transaction = trans;
                                        count += cmd.ExecuteNonQuery();
                                    }
                                    #endregion

                                    var contadorLineaMovPol = 1;

                                    var listaFacturasPorCC = listaFacturas.GroupBy(x => x.cc).Select(x => new { cc = x.Key, grp = x }).ToList();

                                    foreach (var grupoCC in listaFacturasPorCC)
                                    {
                                        var facturasCC = grupoCC.grp.ToList();
                                        var sumatoriaMontoEnPesos = (facturasCC.Sum(x => x.monto) * ultimoTipo_cambioEK.tipo_cambio) * -1;

                                        #region sc_movpol Registro Abono (Sumatoria - Monto Negativo)
                                        using (var cmd = new OdbcCommand(@"
                                        INSERT INTO sc_movpol 
                                        (year, mes, poliza, tp, linea, cta, scta, sscta, digito, tm, referencia, cc, concepto, monto, iclave, itm, st_par, orden_compra, numpro, socio_inversionista, istm, folio_imp, linea_imp, num_emp, folio_gxc,
                                        cfd_ruta_pdf, cfd_ruta_xml, UUID, cfd_rfc, cfd_tipocambio, cfd_total, cfd_moneda, metodo_pago_sat, ruta_comp_ext, factura_comp_ext, taxid, forma_pago, cfd_fecha_expedicion, cfd_tipocomprobante, cfd_metodo_pago_sat, area, cuenta_oc)
                                        VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)"))
                                        {
                                            OdbcParameterCollection parameters = cmd.Parameters;

                                            parameters.Add("@year", OdbcType.Numeric).Value = DateTime.Now.Year;
                                            parameters.Add("@mes", OdbcType.Numeric).Value = DateTime.Now.Month;
                                            parameters.Add("@poliza", OdbcType.Numeric).Value = siguienteNumeroCheque;
                                            parameters.Add("@tp", OdbcType.Char).Value = cuentaBancariaEK.tp;
                                            parameters.Add("@linea", OdbcType.Numeric).Value = contadorLineaMovPol;
                                            parameters.Add("@cta", OdbcType.Numeric).Value = cuentaBancariaEK.cta;
                                            parameters.Add("@scta", OdbcType.Numeric).Value = cuentaBancariaEK.scta;
                                            parameters.Add("@sscta", OdbcType.Numeric).Value = cuentaBancariaEK.sscta;
                                            parameters.Add("@digito", OdbcType.Numeric).Value = 4;
                                            parameters.Add("@tm", OdbcType.Numeric).Value = 2;
                                            parameters.Add("@referencia", OdbcType.Char).Value = siguienteNumeroCheque;
                                            parameters.Add("@cc", OdbcType.Char).Value = grupoCC.cc;
                                            parameters.Add("@concepto", OdbcType.Char).Value = proveedor.nombre;
                                            parameters.Add("@monto", OdbcType.Numeric).Value = sumatoriaMontoEnPesos;
                                            parameters.Add("@iclave", OdbcType.Numeric).Value = 0;
                                            parameters.Add("@itm", OdbcType.Numeric).Value = 57;
                                            parameters.Add("@st_par", OdbcType.Char).Value = (object)DBNull.Value;
                                            parameters.Add("@orden_compra", OdbcType.Numeric).Value = facturasCC[0].referenciaoc;
                                            parameters.Add("@numpro", OdbcType.Numeric).Value = 0;
                                            parameters.Add("@socio_inversionista", OdbcType.Numeric).Value = (object)DBNull.Value;
                                            parameters.Add("@istm", OdbcType.Numeric).Value = 0;
                                            parameters.Add("@folio_imp", OdbcType.Numeric).Value = (object)DBNull.Value;
                                            parameters.Add("@linea_imp", OdbcType.Numeric).Value = (object)DBNull.Value;
                                            parameters.Add("@num_emp", OdbcType.Numeric).Value = (object)DBNull.Value;
                                            parameters.Add("@folio_gxc", OdbcType.Numeric).Value = (object)DBNull.Value;
                                            parameters.Add("@cfd_ruta_pdf", OdbcType.VarChar).Value = (object)DBNull.Value;
                                            parameters.Add("@cfd_ruta_xml", OdbcType.VarChar).Value = (object)DBNull.Value;
                                            parameters.Add("@UUID", OdbcType.VarChar).Value = (object)DBNull.Value;
                                            parameters.Add("@cfd_rfc", OdbcType.VarChar).Value = (object)DBNull.Value;
                                            parameters.Add("@cfd_tipocambio", OdbcType.Numeric).Value = (object)DBNull.Value;
                                            parameters.Add("@cfd_total", OdbcType.Numeric).Value = (object)DBNull.Value;
                                            parameters.Add("@cfd_moneda", OdbcType.Char).Value = (object)DBNull.Value;
                                            parameters.Add("@metodo_pago_sat", OdbcType.Numeric).Value = (object)DBNull.Value;
                                            parameters.Add("@ruta_comp_ext", OdbcType.Char).Value = (object)DBNull.Value;
                                            parameters.Add("@factura_comp_ext", OdbcType.Char).Value = (object)DBNull.Value;
                                            parameters.Add("@taxid", OdbcType.Char).Value = (object)DBNull.Value;
                                            parameters.Add("@forma_pago", OdbcType.VarChar).Value = "TRANSFERENCIA BANCARIA";
                                            parameters.Add("@cfd_fecha_expedicion", OdbcType.DateTime).Value = (object)DBNull.Value;
                                            parameters.Add("@cfd_tipocomprobante", OdbcType.VarChar).Value = (object)DBNull.Value;
                                            parameters.Add("@cfd_metodo_pago_sat", OdbcType.VarChar).Value = (object)DBNull.Value;

                                            if (vSesiones.sesionEmpresaActual == 1)
                                            {
                                                parameters.Add("@area", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                parameters.Add("@cuenta_oc", OdbcType.Numeric).Value = (object)DBNull.Value;
                                            }
                                            else
                                            {
                                                //var areaCuenta = listaAreasCuentas.FirstOrDefault(x => x.centro_costo == grupoCC.cc);

                                                //parameters.Add("@area", OdbcType.Numeric).Value = areaCuenta.area;
                                                //parameters.Add("@cuenta_oc", OdbcType.Numeric).Value = areaCuenta.cuenta;

                                                var ordenCompraEkDetalle = _contextEnkontrol.Select<OrdenCompraDTO>(getEnkontrolAmbienteConsulta(), new OdbcConsultaDTO
                                                {
                                                    consulta = string.Format(@"SELECT TOP 1 area, cuenta FROM so_orden_compra_det WHERE cc = '{0}' AND numero = {1}", facturasCC[0].cc, facturasCC[0].referenciaoc)
                                                }).FirstOrDefault();

                                                if (ordenCompraEkDetalle == null)
                                                {
                                                    throw new Exception(string.Format(@"No se encuentra el detalle de la orden de compra {0}-{1}", facturasCC[0].cc, facturasCC[0].referenciaoc));
                                                }

                                                parameters.Add("@area", OdbcType.Numeric).Value = ordenCompraEkDetalle.area;
                                                parameters.Add("@cuenta_oc", OdbcType.Numeric).Value = ordenCompraEkDetalle.cuenta;
                                            }

                                            cmd.Connection = trans.Connection;
                                            cmd.Transaction = trans;
                                            count += cmd.ExecuteNonQuery();
                                        }

                                        contadorLineaMovPol++;
                                        #endregion

                                        var sumatoriaMontoDiferenciaCambiaria = 0m;

                                        foreach (var fac in facturasCC)
                                        {
                                            var ordenCompraEK = _contextEnkontrol.Select<OrdenCompraDTO>(getEnkontrolAmbienteConsulta(), new OdbcConsultaDTO
                                            {
                                                consulta = string.Format(@"SELECT tipo_cambio FROM so_orden_compra WHERE cc = '{0}' AND numero = {1}", fac.cc, fac.referenciaoc)
                                            }).FirstOrDefault();

                                            if (ordenCompraEK == null)
                                            {
                                                throw new Exception(string.Format(@"No se encuentra la orden de compra {0}-{1} para la factura {2}", fac.cc, fac.referenciaoc, fac.factura));
                                            }

                                            #region sc_movpol Primer Registro Registro Cargo (Monto Positivo)
                                            var contadorMovPolCargoLinea = contadorLineaMovPol;

                                            sumatoriaMontoDiferenciaCambiaria += fac.monto;

                                            using (var cmd = new OdbcCommand(@"
                                            INSERT INTO sc_movpol 
                                            (year, mes, poliza, tp, linea, cta, scta, sscta, digito, tm, referencia, cc, concepto, monto, iclave, itm, st_par, orden_compra, numpro, socio_inversionista, istm, folio_imp, linea_imp, num_emp, folio_gxc,
                                            cfd_ruta_pdf, cfd_ruta_xml, UUID, cfd_rfc, cfd_tipocambio, cfd_total, cfd_moneda, metodo_pago_sat, ruta_comp_ext, factura_comp_ext, taxid, forma_pago, cfd_fecha_expedicion, cfd_tipocomprobante, cfd_metodo_pago_sat, area, cuenta_oc)
                                            VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)"))
                                            {
                                                OdbcParameterCollection parameters = cmd.Parameters;

                                                parameters.Add("@year", OdbcType.Numeric).Value = DateTime.Now.Year;
                                                parameters.Add("@mes", OdbcType.Numeric).Value = DateTime.Now.Month;
                                                parameters.Add("@poliza", OdbcType.Numeric).Value = siguienteNumeroCheque;
                                                parameters.Add("@tp", OdbcType.Char).Value = cuentaBancariaEK.tp;
                                                parameters.Add("@linea", OdbcType.Numeric).Value = contadorLineaMovPol;
                                                parameters.Add("@cta", OdbcType.Numeric).Value = 2105;
                                                parameters.Add("@scta", OdbcType.Numeric).Value = 2;
                                                parameters.Add("@sscta", OdbcType.Numeric).Value = 0;
                                                parameters.Add("@digito", OdbcType.Numeric).Value = 0;
                                                parameters.Add("@tm", OdbcType.Numeric).Value = 1;
                                                parameters.Add("@referencia", OdbcType.Char).Value = fac.factura;
                                                parameters.Add("@cc", OdbcType.Char).Value = fac.cc;
                                                parameters.Add("@concepto", OdbcType.Char).Value = string.Format(@"Ch:{0} Cta:{1}", siguienteNumeroCheque, cuentaBancariaEK.descripcion);
                                                parameters.Add("@monto", OdbcType.Numeric).Value = fac.monto;
                                                parameters.Add("@iclave", OdbcType.Numeric).Value = 0;
                                                parameters.Add("@itm", OdbcType.Numeric).Value = 51;
                                                parameters.Add("@st_par", OdbcType.Char).Value = (object)DBNull.Value;
                                                parameters.Add("@orden_compra", OdbcType.Numeric).Value = fac.referenciaoc;
                                                parameters.Add("@numpro", OdbcType.Numeric).Value = fac.numpro;
                                                parameters.Add("@socio_inversionista", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                parameters.Add("@istm", OdbcType.Numeric).Value = 0;
                                                parameters.Add("@folio_imp", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                parameters.Add("@linea_imp", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                parameters.Add("@num_emp", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                parameters.Add("@folio_gxc", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                parameters.Add("@cfd_ruta_pdf", OdbcType.VarChar).Value = (object)DBNull.Value;
                                                parameters.Add("@cfd_ruta_xml", OdbcType.VarChar).Value = (object)DBNull.Value;
                                                parameters.Add("@UUID", OdbcType.VarChar).Value = (object)DBNull.Value;
                                                parameters.Add("@cfd_rfc", OdbcType.VarChar).Value = (object)DBNull.Value;
                                                parameters.Add("@cfd_tipocambio", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                parameters.Add("@cfd_total", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                parameters.Add("@cfd_moneda", OdbcType.Char).Value = (object)DBNull.Value;
                                                parameters.Add("@metodo_pago_sat", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                parameters.Add("@ruta_comp_ext", OdbcType.Char).Value = (object)DBNull.Value;
                                                parameters.Add("@factura_comp_ext", OdbcType.Char).Value = (object)DBNull.Value;
                                                parameters.Add("@taxid", OdbcType.Char).Value = (object)DBNull.Value;
                                                parameters.Add("@forma_pago", OdbcType.VarChar).Value = "TRANSFERENCIA BANCARIA";
                                                parameters.Add("@cfd_fecha_expedicion", OdbcType.DateTime).Value = (object)DBNull.Value;
                                                parameters.Add("@cfd_tipocomprobante", OdbcType.VarChar).Value = (object)DBNull.Value;
                                                parameters.Add("@cfd_metodo_pago_sat", OdbcType.VarChar).Value = (object)DBNull.Value;

                                                if (vSesiones.sesionEmpresaActual == 1)
                                                {
                                                    parameters.Add("@area", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                    parameters.Add("@cuenta_oc", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                }
                                                else
                                                {
                                                    //var areaCuenta = listaAreasCuentas.FirstOrDefault(x => x.centro_costo == fac.cc);

                                                    //parameters.Add("@area", OdbcType.Numeric).Value = areaCuenta.area;
                                                    //parameters.Add("@cuenta_oc", OdbcType.Numeric).Value = areaCuenta.cuenta;

                                                    var ordenCompraEkDetalle = _contextEnkontrol.Select<OrdenCompraDTO>(getEnkontrolAmbienteConsulta(), new OdbcConsultaDTO
                                                    {
                                                        consulta = string.Format(@"SELECT TOP 1 area, cuenta FROM so_orden_compra_det WHERE cc = '{0}' AND numero = {1}", fac.cc, fac.referenciaoc)
                                                    }).FirstOrDefault();

                                                    if (ordenCompraEkDetalle == null)
                                                    {
                                                        throw new Exception(string.Format(@"No se encuentra el detalle de la orden de compra {0}-{1}", fac.cc, fac.referenciaoc));
                                                    }

                                                    parameters.Add("@area", OdbcType.Numeric).Value = ordenCompraEkDetalle.area;
                                                    parameters.Add("@cuenta_oc", OdbcType.Numeric).Value = ordenCompraEkDetalle.cuenta;
                                                }

                                                cmd.Connection = trans.Connection;
                                                cmd.Transaction = trans;
                                                count += cmd.ExecuteNonQuery();
                                            }

                                            contadorLineaMovPol++;
                                            #endregion

                                            #region sc_movpol Segundo Registro Cargo (Monto Positivo Calculado)
                                            var montoResultado = (fac.monto * ordenCompraEK.tipo_cambio) - fac.monto;

                                            sumatoriaMontoDiferenciaCambiaria += montoResultado;

                                            using (var cmd = new OdbcCommand(@"
                                            INSERT INTO sc_movpol 
                                            (year, mes, poliza, tp, linea, cta, scta, sscta, digito, tm, referencia, cc, concepto, monto, iclave, itm, st_par, orden_compra, numpro, socio_inversionista, istm, folio_imp, linea_imp, num_emp, folio_gxc,
                                            cfd_ruta_pdf, cfd_ruta_xml, UUID, cfd_rfc, cfd_tipocambio, cfd_total, cfd_moneda, metodo_pago_sat, ruta_comp_ext, factura_comp_ext, taxid, forma_pago, cfd_fecha_expedicion, cfd_tipocomprobante, cfd_metodo_pago_sat, area, cuenta_oc)
                                            VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)"))
                                            {
                                                OdbcParameterCollection parameters = cmd.Parameters;

                                                parameters.Add("@year", OdbcType.Numeric).Value = DateTime.Now.Year;
                                                parameters.Add("@mes", OdbcType.Numeric).Value = DateTime.Now.Month;
                                                parameters.Add("@poliza", OdbcType.Numeric).Value = siguienteNumeroCheque;
                                                parameters.Add("@tp", OdbcType.Char).Value = cuentaBancariaEK.tp;
                                                parameters.Add("@linea", OdbcType.Numeric).Value = contadorLineaMovPol;
                                                parameters.Add("@cta", OdbcType.Numeric).Value = 2105;
                                                parameters.Add("@scta", OdbcType.Numeric).Value = 200;
                                                parameters.Add("@sscta", OdbcType.Numeric).Value = 0;
                                                parameters.Add("@digito", OdbcType.Numeric).Value = 0;
                                                parameters.Add("@tm", OdbcType.Numeric).Value = 1;
                                                parameters.Add("@referencia", OdbcType.Char).Value = fac.factura;
                                                parameters.Add("@cc", OdbcType.Char).Value = fac.cc;
                                                parameters.Add("@concepto", OdbcType.Char).Value = string.Format(@"Ch:{0} Cta:{1}", siguienteNumeroCheque, cuentaBancariaEK.descripcion);
                                                parameters.Add("@monto", OdbcType.Numeric).Value = montoResultado;
                                                parameters.Add("@iclave", OdbcType.Numeric).Value = 0;
                                                parameters.Add("@itm", OdbcType.Numeric).Value = 0;
                                                parameters.Add("@st_par", OdbcType.Char).Value = (object)DBNull.Value;
                                                parameters.Add("@orden_compra", OdbcType.Numeric).Value = fac.referenciaoc;
                                                parameters.Add("@numpro", OdbcType.Numeric).Value = fac.numpro;
                                                parameters.Add("@socio_inversionista", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                parameters.Add("@istm", OdbcType.Numeric).Value = 0;
                                                parameters.Add("@folio_imp", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                parameters.Add("@linea_imp", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                parameters.Add("@num_emp", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                parameters.Add("@folio_gxc", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                parameters.Add("@cfd_ruta_pdf", OdbcType.VarChar).Value = (object)DBNull.Value;
                                                parameters.Add("@cfd_ruta_xml", OdbcType.VarChar).Value = (object)DBNull.Value;
                                                parameters.Add("@UUID", OdbcType.VarChar).Value = (object)DBNull.Value;
                                                parameters.Add("@cfd_rfc", OdbcType.VarChar).Value = (object)DBNull.Value;
                                                parameters.Add("@cfd_tipocambio", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                parameters.Add("@cfd_total", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                parameters.Add("@cfd_moneda", OdbcType.Char).Value = (object)DBNull.Value;
                                                parameters.Add("@metodo_pago_sat", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                parameters.Add("@ruta_comp_ext", OdbcType.Char).Value = (object)DBNull.Value;
                                                parameters.Add("@factura_comp_ext", OdbcType.Char).Value = (object)DBNull.Value;
                                                parameters.Add("@taxid", OdbcType.Char).Value = (object)DBNull.Value;
                                                parameters.Add("@forma_pago", OdbcType.VarChar).Value = "TRANSFERENCIA BANCARIA";
                                                parameters.Add("@cfd_fecha_expedicion", OdbcType.DateTime).Value = (object)DBNull.Value;
                                                parameters.Add("@cfd_tipocomprobante", OdbcType.VarChar).Value = (object)DBNull.Value;
                                                parameters.Add("@cfd_metodo_pago_sat", OdbcType.VarChar).Value = (object)DBNull.Value;
                                                parameters.Add("@area", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                parameters.Add("@cuenta_oc", OdbcType.Numeric).Value = (object)DBNull.Value;

                                                cmd.Connection = trans.Connection;
                                                cmd.Transaction = trans;
                                                count += cmd.ExecuteNonQuery();
                                            }

                                            contadorLineaMovPol++;
                                            #endregion

                                            #region sp_movprov
                                            var autoincremento = _contextEnkontrol.Select<sp_movprovDTO>(getEnkontrolAmbienteConsulta(), new OdbcConsultaDTO
                                            {
                                                consulta = string.Format(@"SELECT TOP 1 autoincremento FROM sp_movprov ORDER BY autoincremento DESC")
                                            }).FirstOrDefault();

                                            using (var cmd = new OdbcCommand(@"
                                            INSERT INTO sp_movprov 
                                            (numpro, factura, fecha, tm, fechavenc, concepto, cc, referenciaoc, monto, tipocambio, iva, year, mes, poliza, tp, linea, generado, es_factura, moneda, autorizapago, total, st_pago, folio, autoincremento,
                                            tipocambio_oc, empleado_modifica, fecha_modifica, hora_modifica, pago_factoraje, inst_factoraje, st_factoraje, socio_inversionista, bit_factoraje, bit_autoriza, bit_transferida, bit_pagada, empleado,
                                            folio_gxc, cfd_serie, cfd_folio, cfd_fecha, cfd_certificado, cfd_ano_aprob, cfd_num_aprob, ruta_rec_xml, ruta_rec_pdf, afectacompra, val_ref, suma_o_resta, pide_iva, valida_recibido, valida_almacen,
                                            valida_recibido_autorizar, empleado_autorizo, fecha_autoriza, empleado_vobo, fecha_vobo, tipo_factoraje, UUID, folio_retencion, cfd_metodo_pago, moneda_oc_nom, moneda_oc)
                                            VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)"))
                                            {
                                                OdbcParameterCollection parameters = cmd.Parameters;

                                                parameters.Add("@numpro", OdbcType.Numeric).Value = fac.numpro;
                                                parameters.Add("@factura", OdbcType.Numeric).Value = fac.factura;
                                                parameters.Add("@fecha", OdbcType.Date).Value = DateTime.Now;
                                                parameters.Add("@tm", OdbcType.Numeric).Value = 51; //parameters.Add("@tm", OdbcType.Numeric).Value = tm_pago;
                                                parameters.Add("@fechavenc", OdbcType.Date).Value = DateTime.Now;
                                                parameters.Add("@concepto", OdbcType.Char).Value = string.Format(@"CHEQUE: {0} : {1}", siguienteNumeroCheque, bancoEK.bancoDesc);
                                                parameters.Add("@cc", OdbcType.Char).Value = fac.cc;
                                                parameters.Add("@referenciaoc", OdbcType.Char).Value = fac.referenciaoc;
                                                parameters.Add("@monto", OdbcType.Numeric).Value = fac.monto * -1;
                                                parameters.Add("@tipocambio", OdbcType.Numeric).Value = ordenCompraEK.tipo_cambio;
                                                parameters.Add("@iva", OdbcType.Numeric).Value = 0;
                                                parameters.Add("@year", OdbcType.Numeric).Value = DateTime.Now.Year;
                                                parameters.Add("@mes", OdbcType.Numeric).Value = DateTime.Now.Month;
                                                parameters.Add("@poliza", OdbcType.Numeric).Value = siguienteNumeroCheque;
                                                parameters.Add("@tp", OdbcType.Char).Value = cuentaBancariaEK.tp;
                                                parameters.Add("@linea", OdbcType.Numeric).Value = contadorMovPolCargoLinea;
                                                parameters.Add("@generado", OdbcType.Char).Value = "B";
                                                parameters.Add("@es_factura", OdbcType.Char).Value = "N";
                                                parameters.Add("@moneda", OdbcType.Char).Value = cuentaBancariaEK.moneda;
                                                parameters.Add("@autorizapago", OdbcType.Char).Value = "";
                                                parameters.Add("@total", OdbcType.Numeric).Value = fac.monto * -1;
                                                parameters.Add("@st_pago", OdbcType.Char).Value = "";
                                                parameters.Add("@folio", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                parameters.Add("@autoincremento", OdbcType.Numeric).Value = autoincremento.autoincremento + 1;
                                                parameters.Add("@tipocambio_oc", OdbcType.Numeric).Value = ordenCompraEK.tipo_cambio;
                                                parameters.Add("@empleado_modifica", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                parameters.Add("@fecha_modifica", OdbcType.Date).Value = (object)DBNull.Value;
                                                parameters.Add("@hora_modifica", OdbcType.Time).Value = (object)DBNull.Value;
                                                parameters.Add("@pago_factoraje", OdbcType.Char).Value = (object)DBNull.Value;
                                                parameters.Add("@inst_factoraje", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                parameters.Add("@st_factoraje", OdbcType.Char).Value = (object)DBNull.Value;
                                                parameters.Add("@socio_inversionista", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                parameters.Add("@bit_factoraje", OdbcType.Char).Value = "N";
                                                parameters.Add("@bit_autoriza", OdbcType.Char).Value = "N";
                                                parameters.Add("@bit_transferida", OdbcType.Char).Value = "N";
                                                parameters.Add("@bit_pagada", OdbcType.Char).Value = "N";
                                                parameters.Add("@empleado", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                parameters.Add("@folio_gxc", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                parameters.Add("@cfd_serie", OdbcType.VarChar).Value = (object)DBNull.Value;
                                                parameters.Add("@cfd_folio", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                parameters.Add("@cfd_fecha", OdbcType.DateTime).Value = (object)DBNull.Value;
                                                parameters.Add("@cfd_certificado", OdbcType.Char).Value = (object)DBNull.Value;
                                                parameters.Add("@cfd_ano_aprob", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                parameters.Add("@cfd_num_aprob", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                parameters.Add("@ruta_rec_xml", OdbcType.VarChar).Value = (object)DBNull.Value;
                                                parameters.Add("@ruta_rec_pdf", OdbcType.VarChar).Value = (object)DBNull.Value;
                                                parameters.Add("@afectacompra", OdbcType.Char).Value = "N";
                                                parameters.Add("@val_ref", OdbcType.Char).Value = " ";
                                                parameters.Add("@suma_o_resta", OdbcType.Numeric).Value = 0;
                                                parameters.Add("@pide_iva", OdbcType.Char).Value = "N";
                                                parameters.Add("@valida_recibido", OdbcType.Char).Value = " ";
                                                parameters.Add("@valida_almacen", OdbcType.Char).Value = "N";
                                                parameters.Add("@valida_recibido_autorizar", OdbcType.Char).Value = "N";
                                                parameters.Add("@empleado_autorizo", OdbcType.Numeric).Value = 0;
                                                parameters.Add("@fecha_autoriza", OdbcType.Date).Value = (object)DBNull.Value;
                                                parameters.Add("@empleado_vobo", OdbcType.Numeric).Value = 0;
                                                parameters.Add("@fecha_vobo", OdbcType.Date).Value = (object)DBNull.Value;
                                                parameters.Add("@tipo_factoraje", OdbcType.Char).Value = "N";
                                                parameters.Add("@UUID", OdbcType.VarChar).Value = (object)DBNull.Value;
                                                parameters.Add("@folio_retencion", OdbcType.Numeric).Value = (object)DBNull.Value;
                                                parameters.Add("@cfd_metodo_pago", OdbcType.Char).Value = (object)DBNull.Value;
                                                parameters.Add("@moneda_oc_nom", OdbcType.Char).Value = (object)DBNull.Value;
                                                parameters.Add("@moneda_oc", OdbcType.Char).Value = (object)DBNull.Value;

                                                cmd.Connection = trans.Connection;
                                                cmd.Transaction = trans;
                                                count += cmd.ExecuteNonQuery();
                                            }
                                            #endregion
                                        }

                                        #region sb_edo_cta_chequera
                                        using (var cmd = new OdbcCommand(@"
                                            INSERT INTO sb_edo_cta_chequera 
                                            (cuenta, fecha_mov, tm, numero, cc, descripcion, monto, tc, origen_mov, generada, st_consilia, num_consilia, st_che, ref_che_inverso, ref_tm_inverso, motivo_cancelado, iyear, imes, ipoliza, itp, ilinea,
                                            banco, tp, [desc], folio, tipo_iva, porc_iva, monto_iva, folio_iva, fecha_reten, fecha_reten_fin, id_num_credito, prototipo, consecutivo_minist, acredita_iva, clave_sub_tm, folio_imp, linea_imp)
                                            VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)"))
                                        {
                                            OdbcParameterCollection parameters = cmd.Parameters;

                                            parameters.Add("@cuenta", OdbcType.Numeric).Value = cuentaBancaria;
                                            parameters.Add("@fecha_mov", OdbcType.Date).Value = DateTime.Now;
                                            parameters.Add("@tm", OdbcType.Numeric).Value = tm_pago;
                                            parameters.Add("@numero", OdbcType.Numeric).Value = siguienteNumeroCheque;
                                            parameters.Add("@cc", OdbcType.Char).Value = grupoCC.cc;
                                            parameters.Add("@descripcion", OdbcType.Char).Value = proveedor.nombre;
                                            parameters.Add("@monto", OdbcType.Numeric).Value = (facturasCC.Sum(x => x.monto) * -1) * ultimoTipo_cambioEK.tipo_cambio;
                                            parameters.Add("@tc", OdbcType.Numeric).Value = 1;
                                            parameters.Add("@origen_mov", OdbcType.Char).Value = "P";
                                            parameters.Add("@generada", OdbcType.Char).Value = "B";
                                            parameters.Add("@st_consilia", OdbcType.Char).Value = "";
                                            parameters.Add("@num_consilia", OdbcType.Numeric).Value = 0;
                                            parameters.Add("@st_che", OdbcType.Char).Value = "";
                                            parameters.Add("@ref_che_inverso", OdbcType.Numeric).Value = 0;
                                            parameters.Add("@ref_tm_inverso", OdbcType.Numeric).Value = 0;
                                            parameters.Add("@motivo_cancelado", OdbcType.Char).Value = "";
                                            parameters.Add("@iyear", OdbcType.Numeric).Value = DateTime.Now.Year;
                                            parameters.Add("@imes", OdbcType.Numeric).Value = DateTime.Now.Month;
                                            parameters.Add("@ipoliza", OdbcType.Numeric).Value = siguienteNumeroCheque;
                                            parameters.Add("@itp", OdbcType.Char).Value = cuentaBancariaEK.tp;
                                            parameters.Add("@ilinea", OdbcType.Numeric).Value = 1;
                                            parameters.Add("@banco", OdbcType.Numeric).Value = cuentaBancariaEK.banco;
                                            parameters.Add("@tp", OdbcType.Char).Value = (object)DBNull.Value;
                                            parameters.Add("@desc", OdbcType.Char).Value = (object)DBNull.Value;
                                            parameters.Add("@folio", OdbcType.Numeric).Value = (object)DBNull.Value;
                                            parameters.Add("@tipo_iva", OdbcType.Char).Value = (object)DBNull.Value;
                                            parameters.Add("@porc_iva", OdbcType.Numeric).Value = (object)DBNull.Value;
                                            parameters.Add("@monto_iva", OdbcType.Numeric).Value = (object)DBNull.Value;
                                            parameters.Add("@folio_iva", OdbcType.Numeric).Value = (object)DBNull.Value;
                                            parameters.Add("@fecha_reten", OdbcType.Date).Value = (object)DBNull.Value;
                                            parameters.Add("@fecha_reten_fin", OdbcType.Date).Value = (object)DBNull.Value;
                                            parameters.Add("@id_num_credito", OdbcType.Numeric).Value = (object)DBNull.Value;
                                            parameters.Add("@prototipo", OdbcType.Char).Value = (object)DBNull.Value;
                                            parameters.Add("@consecutivo_minist", OdbcType.Numeric).Value = (object)DBNull.Value;
                                            parameters.Add("@acredita_iva", OdbcType.Char).Value = (object)DBNull.Value;
                                            parameters.Add("@clave_sub_tm", OdbcType.Numeric).Value = (object)DBNull.Value;
                                            parameters.Add("@folio_imp", OdbcType.Numeric).Value = (object)DBNull.Value;
                                            parameters.Add("@linea_imp", OdbcType.Numeric).Value = (object)DBNull.Value;

                                            cmd.Connection = trans.Connection;
                                            cmd.Transaction = trans;
                                            count += cmd.ExecuteNonQuery();
                                        }
                                        #endregion

                                        #region sc_movpol Registro Diferencia Cambiaria Cargo (Monto Positivo)
                                        var montoDiferenciaCambiaria = (sumatoriaMontoEnPesos * -1) - sumatoriaMontoDiferenciaCambiaria;

                                        using (var cmd = new OdbcCommand(@"
                                            INSERT INTO sc_movpol 
                                            (year, mes, poliza, tp, linea, cta, scta, sscta, digito, tm, referencia, cc, concepto, monto, iclave, itm, st_par, orden_compra, numpro, socio_inversionista, istm, folio_imp, linea_imp, num_emp, folio_gxc,
                                            cfd_ruta_pdf, cfd_ruta_xml, UUID, cfd_rfc, cfd_tipocambio, cfd_total, cfd_moneda, metodo_pago_sat, ruta_comp_ext, factura_comp_ext, taxid, forma_pago, cfd_fecha_expedicion, cfd_tipocomprobante, cfd_metodo_pago_sat, area, cuenta_oc)
                                            VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)"))
                                        {
                                            OdbcParameterCollection parameters = cmd.Parameters;

                                            parameters.Add("@year", OdbcType.Numeric).Value = DateTime.Now.Year;
                                            parameters.Add("@mes", OdbcType.Numeric).Value = DateTime.Now.Month;
                                            parameters.Add("@poliza", OdbcType.Numeric).Value = siguienteNumeroCheque;
                                            parameters.Add("@tp", OdbcType.Char).Value = cuentaBancariaEK.tp;
                                            parameters.Add("@linea", OdbcType.Numeric).Value = contadorLineaMovPol;
                                            parameters.Add("@cta", OdbcType.Numeric).Value = 5900;
                                            parameters.Add("@scta", OdbcType.Numeric).Value = 1;
                                            parameters.Add("@sscta", OdbcType.Numeric).Value = 0;
                                            parameters.Add("@digito", OdbcType.Numeric).Value = 6;
                                            parameters.Add("@tm", OdbcType.Numeric).Value = 1;
                                            parameters.Add("@referencia", OdbcType.Char).Value = siguienteNumeroCheque;
                                            parameters.Add("@cc", OdbcType.Char).Value = grupoCC.cc;
                                            parameters.Add("@concepto", OdbcType.Char).Value = string.Format(@"Diferencia cambiaria en el Cheque:{0}", siguienteNumeroCheque);
                                            parameters.Add("@monto", OdbcType.Numeric).Value = montoDiferenciaCambiaria;
                                            parameters.Add("@iclave", OdbcType.Numeric).Value = 0;
                                            parameters.Add("@itm", OdbcType.Numeric).Value = 0;
                                            parameters.Add("@st_par", OdbcType.Char).Value = (object)DBNull.Value;
                                            parameters.Add("@orden_compra", OdbcType.Numeric).Value = 0;
                                            parameters.Add("@numpro", OdbcType.Numeric).Value = 0;
                                            parameters.Add("@socio_inversionista", OdbcType.Numeric).Value = (object)DBNull.Value;
                                            parameters.Add("@istm", OdbcType.Numeric).Value = 0;
                                            parameters.Add("@folio_imp", OdbcType.Numeric).Value = (object)DBNull.Value;
                                            parameters.Add("@linea_imp", OdbcType.Numeric).Value = (object)DBNull.Value;
                                            parameters.Add("@num_emp", OdbcType.Numeric).Value = (object)DBNull.Value;
                                            parameters.Add("@folio_gxc", OdbcType.Numeric).Value = (object)DBNull.Value;
                                            parameters.Add("@cfd_ruta_pdf", OdbcType.VarChar).Value = (object)DBNull.Value;
                                            parameters.Add("@cfd_ruta_xml", OdbcType.VarChar).Value = (object)DBNull.Value;
                                            parameters.Add("@UUID", OdbcType.VarChar).Value = (object)DBNull.Value;
                                            parameters.Add("@cfd_rfc", OdbcType.VarChar).Value = (object)DBNull.Value;
                                            parameters.Add("@cfd_tipocambio", OdbcType.Numeric).Value = (object)DBNull.Value;
                                            parameters.Add("@cfd_total", OdbcType.Numeric).Value = (object)DBNull.Value;
                                            parameters.Add("@cfd_moneda", OdbcType.Char).Value = (object)DBNull.Value;
                                            parameters.Add("@metodo_pago_sat", OdbcType.Numeric).Value = (object)DBNull.Value;
                                            parameters.Add("@ruta_comp_ext", OdbcType.Char).Value = (object)DBNull.Value;
                                            parameters.Add("@factura_comp_ext", OdbcType.Char).Value = (object)DBNull.Value;
                                            parameters.Add("@taxid", OdbcType.Char).Value = (object)DBNull.Value;
                                            parameters.Add("@forma_pago", OdbcType.VarChar).Value = "TRANSFERENCIA BANCARIA";
                                            parameters.Add("@cfd_fecha_expedicion", OdbcType.DateTime).Value = (object)DBNull.Value;
                                            parameters.Add("@cfd_tipocomprobante", OdbcType.VarChar).Value = (object)DBNull.Value;
                                            parameters.Add("@cfd_metodo_pago_sat", OdbcType.VarChar).Value = (object)DBNull.Value;
                                            parameters.Add("@area", OdbcType.Numeric).Value = (object)DBNull.Value;
                                            parameters.Add("@cuenta_oc", OdbcType.Numeric).Value = (object)DBNull.Value;

                                            cmd.Connection = trans.Connection;
                                            cmd.Transaction = trans;
                                            count += cmd.ExecuteNonQuery();
                                        }

                                        contadorLineaMovPol++;
                                        #endregion
                                    }
                                    #endregion
                                }
                            }

                            trans.Commit();
                            dbSigoplanTransaction.Commit();
                            SaveBitacora(0, (int)AccionEnum.AGREGAR, 0, JsonUtils.convertNetObjectToJson(facturas));

                            resultado.Add(SUCCESS, true);
                        }
                        catch (Exception e)
                        {
                            trans.Rollback();
                            dbSigoplanTransaction.Rollback();

                            LogError(0, 0, NombreControlador, "GenerarCheques", e, AccionEnum.AGREGAR, 0, facturas);

                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, e.Message);
                        }
                    }
                }
            }

            return resultado;
        }

        private string Truncate(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        public Dictionary<string, object> FillComboCuentasBancarias()
        {
            try
            {
                var listaCuentas = _contextEnkontrol.Select<ComboDTO>(getEnkontrolAmbienteConsulta(), @"SELECT cuenta AS Value, (CAST(cuenta AS varchar) + ' - ' + descripcion) AS Text FROM sb_cuenta ORDER BY cuenta");

                resultado.Add(ITEMS, listaCuentas);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(11, 0, NombreControlador, "FillComboCuentasBancarias", e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        private OdbcConnection getEnkontrolConexion()
        {
            if (productivo)
            {
                if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan)
                {
                    return new Conexion().ConnectCplanProductivo();
                }
                else
                {
                    return new Conexion().ConnectArrendarora();
                }
            }
            else
            {
                return new Conexion().ConnectPrueba();
            }
        }

        private EnkontrolAmbienteEnum getEnkontrolAmbienteConsulta()
        {
            if (productivo)
            {
                if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan)
                {
                    return EnkontrolAmbienteEnum.ProdCPLAN;
                }
                else
                {
                    return EnkontrolAmbienteEnum.ProdARREND;
                }
            }
            else
            {
                if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan)
                {
                    return EnkontrolAmbienteEnum.PruebaCPLAN;
                }
                else
                {
                    return EnkontrolAmbienteEnum.PruebaARREND;
                }
            }
        }
    }
}
