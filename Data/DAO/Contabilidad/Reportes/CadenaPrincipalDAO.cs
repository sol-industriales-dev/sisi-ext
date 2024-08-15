using Core.DAO.Contabilidad.Reportes;
using Core.DTO;
using Core.DTO.Contabilidad.Propuesta;
using Core.DTO.Utils.Data;
using Core.Entity.Administrativo.Contabilidad;
using Core.Entity.Principal.Alertas;
using Core.Enum.Administracion.CadenaProductiva;
using Core.Enum.Multiempresa;
using Core.Enum.Principal;
using Core.Enum.Principal.Bitacoras;
using Data.DAO.Principal.Usuarios;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Contabilidad.Reportes
{
    public class CadenaPrincipalDAO : GenericDAO<tblC_CadenaPrincipal>, ICadenaPrincipalDAO
    {

        private const string PERMISO_VOBO = "PermisoVoBo";

        public tblC_CadenaPrincipal Guardar(tblC_CadenaPrincipal obj)
        {
            if (obj.id == 0)
                SaveEntity(obj, (int)BitacoraEnum.CadenaPrincipal);
            else
                Update(obj, obj.id, (int)BitacoraEnum.CadenaPrincipal);
            return obj;
        }
        public List<tblC_CadenaPrincipal> GetDocumentosGuardados()
        {
            return _context.tblC_CadenaPrincipal.Where(x => x.estatus == true).ToList();
        }

        public bool TienePermisoVoBo()
        {
            return new UsuarioDAO().getViewAction(vSesiones.sesionCurrentView, PERMISO_VOBO);
        }

        public Dictionary<string, object> AsignarVoBoCadena(int cadenaID)
        {
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                var result = new Dictionary<string, object>();
                try
                {
                    var cadenaPrincipal = _context.tblC_CadenaPrincipal.FirstOrDefault(x => x.id == cadenaID);

                    if (cadenaPrincipal == null)
                    {
                        result.Add(SUCCESS, false);
                        result.Add(MESSAGE, "No se encontró la cadena principal en el servidor.");
                    }

                    cadenaPrincipal.estadoAutorizacion = EstadoAutorizacionCadenaEnum.VoBo;
                    cadenaPrincipal.firmaVoBo = GlobalUtils.CrearFirmaDigital(cadenaID, DocumentosEnum.Reporte_CadenaProductiva, vSesiones.sesionUsuarioDTO.id, TipoFirmaEnum.Autorizacion);

                    result.Add(SUCCESS, true);
                    _context.SaveChanges();

                    ActualizarAlertaDocumentosPendientes();

                    dbTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    LogError(0, 0, "ReportesController", "AsignarVoBoCadena", e, AccionEnum.ACTUALIZAR, cadenaID, null);
                    result.Add(SUCCESS, false);
                    result.Add(MESSAGE, "Ocurrió un error al momento de tratar de asigar el VoBo.");
                }
                return result;
            }
        }

        public EstadoAutorizacionCadenaEnum ObtenerEstadoAutorizacionCadena(int cadenaID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cadenaPrincipal = _context.tblC_CadenaPrincipal.FirstOrDefault(x => x.id == cadenaID);
                return cadenaPrincipal != null ? cadenaPrincipal.estadoAutorizacion : EstadoAutorizacionCadenaEnum.INDEFINIDO;
            }
            catch (Exception)
            {
                return EstadoAutorizacionCadenaEnum.INDEFINIDO;
            }
        }
        public string ObtenerFirmaAutorizacionCadena(int cadenaID, int tipo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                switch (tipo)
                {
                    case 1:
                        {
                            var cadenaPrincipal = _context.tblC_CadenaPrincipal.FirstOrDefault(x => x.id == cadenaID);
                            return cadenaPrincipal != null ? cadenaPrincipal.firma : String.Empty;
                        }
                    case 2:
                        {
                            var cadenaPrincipal = _context.tblC_CadenaPrincipal.FirstOrDefault(x => x.id == cadenaID);
                            return cadenaPrincipal != null ? cadenaPrincipal.firmaVoBo : String.Empty;
                        }
                    case 3:
                        {
                            var cadenaPrincipal = _context.tblC_CadenaPrincipal.FirstOrDefault(x => x.id == cadenaID);
                            return cadenaPrincipal != null ? cadenaPrincipal.firmaValidado : String.Empty;
                        }
                    default:
                        return String.Empty;
                }

            }
            catch (Exception)
            {
                return String.Empty;
            }
        }




        public Dictionary<string, object> GetDocumentosPorAutorizar()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cadenasPorAutorizar = _context.tblC_CadenaPrincipal.Where(x => x.estatus && x.estadoAutorizacion == EstadoAutorizacionCadenaEnum.VoBo).ToList();
                var documentosPorAutorizar = cadenasPorAutorizar.Select(x => new
                {
                    id = x.id,
                    x.numProveedor,
                    numNafin = AsignarNumNAFIN(Convert.ToInt32(x.numProveedor)),
                    x.proveedor,
                    factoraje = x.factoraje.Equals("V") ? "Vencido" : "Normal",
                    saldoFactura = x.total.ToString("C2"),
                    fechaS = x.fecha.ToShortDateString(),
                    fechaVencimientoS = x.fechaVencimiento.ToShortDateString()
                }).ToList();
                result.Add(SUCCESS, true);
                result.Add("data", documentosPorAutorizar);
            }
            catch (Exception e)
            {
                LogError(0, 0, "ReportesController", "GetDocumentosPorAutorizar", e, AccionEnum.CONSULTA, 0, null);
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, "Ocurrió un error al intentar obtener las cadenas pendientes por autorizar.");
            }
            return result;
        }

        public Dictionary<string, object> AutorizarDocumentos(List<int> idsDocumentosPorAutorizar)
        {
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                var result = new Dictionary<string, object>();
                try
                {
                    if (idsDocumentosPorAutorizar.Count == 0)
                    {
                        result.Add(SUCCESS, false);
                        result.Add(MESSAGE, "La lista de documentos viene vacía.");
                        return result;
                    }

                    foreach (var cadenaID in idsDocumentosPorAutorizar)
                    {
                        var cadenaPrincipal = _context.tblC_CadenaPrincipal.FirstOrDefault(x => x.id == cadenaID);

                        if (cadenaPrincipal == null)
                        {
                            result.Add(SUCCESS, false);
                            result.Add(MESSAGE, "No se encontró un documento en base de datos.");
                            return result;
                        }

                        cadenaPrincipal.estadoAutorizacion = EstadoAutorizacionCadenaEnum.AUTORIZADA;
                        cadenaPrincipal.firma = GlobalUtils.CrearFirmaDigital(cadenaID, DocumentosEnum.Reporte_CadenaProductiva, vSesiones.sesionUsuarioDTO.id, TipoFirmaEnum.Autorizacion);
                    }

                    // Enviar correo a Genaro Araujo
                    bool correoEnviado = EnviarCorreoAlertaCadenasAutorizadas(idsDocumentosPorAutorizar.Count);
                    if (correoEnviado == false)
                    {
                        dbTransaction.Rollback();
                        result.Add(SUCCESS, false);
                        result.Add(MESSAGE, "Ocurrió un error al intentar enviar el correo de notificación.");
                        return result;
                    }

                    result.Add(SUCCESS, true);
                    _context.SaveChanges();

                    ActualizarAlertaDocumentosPendientes();

                    dbTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    LogError(0, 0, "ReportesController", "AutorizarDocumentos", e, AccionEnum.ACTUALIZAR, 0, idsDocumentosPorAutorizar);
                    result.Add(SUCCESS, false);
                    result.Add(MESSAGE, "Ocurrió un error al momento de autorizar los documentos.");
                }
                return result;
            }
        }

        public Dictionary<string, object> ObtenerFacturasCadena(int cadenaID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var facturasCadena = _context.tblC_CadenaProductiva.Where(x => x.idPrincipal == cadenaID)
                    .ToList()
                    .Select(x => new
                    {
                        x.id,
                        x.factura,
                        x.monto,
                        x.proveedor,
                        x.numProveedor,
                        x.centro_costos,
                        fecha = x.fecha.ToShortDateString(),
                        tienePDF = TienePDFFactura(x.numProveedor, x.factura, x.centro_costos),
                        tieneXML = TieneXMLFactura(x.numProveedor, x.factura, x.centro_costos),
                    }).ToList();

                result.Add(SUCCESS, true);
                result.Add(ITEMS, facturasCadena);
            }
            catch (Exception e)
            {
                LogError(0, 0, "ReportesController", "ObtenerFacturasCadena", e, AccionEnum.CONSULTA, 0, null);
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, "Ocurrió un error al intentar obtener las facturas de la cadena indicada.");
            }
            return result;
        }

        private bool TienePDFFactura(string numProveedor, string factura, string cc)
        {
            try
            {
                var odbc = new OdbcConsultaDTO()
                {
                    consulta = @"
                    SELECT gas.ruta_rec_pdf
                    FROM sp_movprov mov
                    INNER JOIN sp_gastos_prov gas ON mov.numpro = gas.numpro AND mov.cc = gas.cc AND mov.factura = gas.factura
                    WHERE mov.cc = ? AND mov.numpro = ? AND mov.factura = ?",
                    parametros = new List<OdbcParameterDTO>()
                };
                odbc.parametros.Add(new OdbcParameterDTO { nombre = "cc", tipo = OdbcType.VarChar, valor = cc });
                odbc.parametros.Add(new OdbcParameterDTO { nombre = "numProveedor", tipo = OdbcType.VarChar, valor = numProveedor });
                odbc.parametros.Add(new OdbcParameterDTO { nombre = "factura", tipo = OdbcType.VarChar, valor = factura });

                var consultaArchivos = _contextEnkontrol.Select<sp_genera_movprovDTO>(EnkontrolAmbienteEnum.Prod, odbc);

                string urlFile = consultaArchivos.Select(x => x.ruta_rec_pdf).FirstOrDefault();

                return !String.IsNullOrEmpty(urlFile);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool TieneXMLFactura(string numProveedor, string factura, string cc)
        {
            try
            {
                var odbc = new OdbcConsultaDTO()
                {
                    consulta = @"
                    SELECT gas.ruta_rec_xml
                    FROM sp_movprov mov
                    INNER JOIN sp_gastos_prov gas ON mov.numpro = gas.numpro AND mov.cc = gas.cc AND mov.factura = gas.factura
                    WHERE mov.cc = ? AND mov.numpro = ? AND mov.factura = ?",
                    parametros = new List<OdbcParameterDTO>()
                };
                odbc.parametros.Add(new OdbcParameterDTO { nombre = "cc", tipo = OdbcType.VarChar, valor = cc });
                odbc.parametros.Add(new OdbcParameterDTO { nombre = "numProveedor", tipo = OdbcType.VarChar, valor = numProveedor });
                odbc.parametros.Add(new OdbcParameterDTO { nombre = "factura", tipo = OdbcType.VarChar, valor = factura });

                var consultaArchivos = _contextEnkontrol.Select<sp_genera_movprovDTO>(EnkontrolAmbienteEnum.Prod, odbc);

                string urlFile = consultaArchivos.Select(x => x.ruta_rec_xml).FirstOrDefault();

                return !String.IsNullOrEmpty(urlFile);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Dictionary<string, object> ObtenerRutaPDFFactura(string numProveedor, string factura, string cc)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var odbc = new OdbcConsultaDTO()
                {
                    consulta = @"
                    SELECT gas.ruta_rec_pdf
                    FROM sp_movprov mov
                    INNER JOIN sp_gastos_prov gas ON mov.numpro = gas.numpro AND mov.cc = gas.cc AND mov.factura = gas.factura
                    WHERE mov.cc = ? AND mov.numpro = ? AND mov.factura = ?",
                    parametros = new List<OdbcParameterDTO>()
                };
                odbc.parametros.Add(new OdbcParameterDTO { nombre = "cc", tipo = OdbcType.VarChar, valor = cc });
                odbc.parametros.Add(new OdbcParameterDTO { nombre = "numProveedor", tipo = OdbcType.VarChar, valor = numProveedor });
                odbc.parametros.Add(new OdbcParameterDTO { nombre = "factura", tipo = OdbcType.VarChar, valor = factura });

                var consultaArchivos = _contextEnkontrol.Select<sp_genera_movprovDTO>(EnkontrolAmbienteEnum.Prod, odbc);

                string urlFile = consultaArchivos.Select(x => x.ruta_rec_pdf).FirstOrDefault();

                if (String.IsNullOrEmpty(urlFile))
                {
                    result.Add(SUCCESS, false);
                    result.Add(MESSAGE, "Ocurrió un error al intentar descargar el archivo PDF de la factura.");
                }

                result.Add(SUCCESS, true);
                result.Add("url", urlFile.Replace(@"C:\", @"\\10.1.0.125\"));
            }
            catch (Exception)
            {
                result.Clear();
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, "Ocurrió un error al intentar descargar el archivo PDF de la factura.");
            }
            return result;
        }

        public Dictionary<string, object> ObtenerRutaXMLFactura(string numProveedor, string factura, string cc)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var odbc = new OdbcConsultaDTO()
                {
                    consulta = @"
                    SELECT gas.ruta_rec_xml
                    FROM sp_movprov mov
                    INNER JOIN sp_gastos_prov gas ON mov.numpro = gas.numpro AND mov.cc = gas.cc AND mov.factura = gas.factura
                    WHERE mov.cc = ? AND mov.numpro = ? AND mov.factura = ?",
                    parametros = new List<OdbcParameterDTO>()
                };
                odbc.parametros.Add(new OdbcParameterDTO { nombre = "cc", tipo = OdbcType.VarChar, valor = cc });
                odbc.parametros.Add(new OdbcParameterDTO { nombre = "numProveedor", tipo = OdbcType.VarChar, valor = numProveedor });
                odbc.parametros.Add(new OdbcParameterDTO { nombre = "factura", tipo = OdbcType.VarChar, valor = factura });

                var consultaArchivos = _contextEnkontrol.Select<sp_genera_movprovDTO>(EnkontrolAmbienteEnum.Prod, odbc);

                string urlFile = consultaArchivos.Select(x => x.ruta_rec_xml).FirstOrDefault();

                if (String.IsNullOrEmpty(urlFile))
                {
                    result.Add(SUCCESS, false);
                    result.Add(MESSAGE, "Ocurrió un error al intentar descargar el archivo XML de la factura.");
                }

                result.Add(SUCCESS, true);
                result.Add("url", urlFile.Replace(@"C:\", @"\\10.1.0.125\"));
            }
            catch (Exception)
            {
                result.Clear();
                result.Add(SUCCESS, false);
                result.Add(MESSAGE, "Ocurrió un error al intentar descargar el archivo XML de la factura.");
            }
            return result;
        }

        private bool EnviarCorreoAlertaCadenasAutorizadas(int cantidadCadenasAutorizadas)
        {
            int usuarioRecibeID = 1068; // Genaro Araujo

            var usuario = _context.tblP_Usuario.FirstOrDefault(x => x.id == usuarioRecibeID);

            string nombreCompletoUsuario = usuario.nombre + " " + usuario.apellidoPaterno + " " + usuario.apellidoMaterno;

            DateTime fechaActual = DateTime.Now;

            string cadenaFechaActual = fechaActual.ToLongDateString();
            string cadenaHoraActual = fechaActual.ToLongTimeString();

            var usuarioEnvia = vSesiones.sesionUsuarioDTO;
            string cuerpoCorreo = @"<html>
                                        <head>
                                            <style>
                                                p {
                                                    font-family: arial, sans-serif;
                                                }
                                            </style>
                                        </head>
                                        <body lang=ES-MX link='#0563C1' vlink='#954F72'>
                                            <div class=WordSection1>" +
                                                @"<p class=MsoNormal>Hola " + nombreCompletoUsuario + @"<o:p></o:p></p>" +
                                                (cantidadCadenasAutorizadas == 1 ? @"<p class=MsoNormal>Se le informa que se autorizó una cadena productiva.<o:p></o:p></p>" :
                                                @"<p class=MsoNormal>Se le informa que se autorizaron " + cantidadCadenasAutorizadas + @" cadenas productivas.<o:p></o:p></p>") +
                                                @"<p class=MsoNormal>Fecha: " + cadenaFechaActual + @"<o:p></o:p></p>" +
                                                @"<p class=MsoNormal>Hora: " + cadenaHoraActual + @"<o:p></o:p></p>" +
                                                @"<p class=MsoNormal>
                                                    PD. Esta notificación es autogenerada por el sistema SIGOPLAN y no es necesario dar una respuesta.<o:p></o:p>
                                                </p>
                                                <p class=MsoNormal>
                                                    Gracias.<o:p></o:p>
                                                </p>
                                            </div>
                                        </body>
                                    </html>";

            return GlobalUtils.sendEmail(string.Format("{0}: Autorización Cadenas Productivas", PersonalUtilities.GetNombreEmpresa()), cuerpoCorreo, new List<string> { usuario.correo });
        }

        public Dictionary<string, object> RechazarDocumento(int cadenaID, string comentarioRechazo)
        {
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                var result = new Dictionary<string, object>();
                try
                {
                    if (cadenaID == 0)
                    {
                        result.Add(SUCCESS, false);
                        result.Add(MESSAGE, "No se pudo identificar a la cadena por eliminar.");
                        return result;
                    }

                    var cadenaPrincipal = _context.tblC_CadenaPrincipal.FirstOrDefault(x => x.id == cadenaID);

                    if (cadenaPrincipal == null)
                    {
                        result.Add(SUCCESS, false);
                        result.Add(MESSAGE, "No se encontró el documento en base de datos.");
                        return result;
                    }

                    cadenaPrincipal.estadoAutorizacion = EstadoAutorizacionCadenaEnum.RECHAZADA;
                    cadenaPrincipal.comentarioRechazo = String.IsNullOrEmpty(comentarioRechazo) ? null : comentarioRechazo.Trim();
                    cadenaPrincipal.firma = String.Empty;

                    result.Add(SUCCESS, true);
                    _context.SaveChanges();

                    ActualizarAlertaDocumentosPendientes();

                    dbTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    LogError(0, 0, "ReportesController", "RechazarDocumento", e, AccionEnum.ACTUALIZAR, cadenaID, null);
                    result.Add(SUCCESS, false);
                    result.Add(MESSAGE, "Ocurrió un error al momento de rechazar el documentos.");
                }
                return result;
            }
        }

        private void ActualizarAlertaDocumentosPendientes()
        {
            int documentosPendientesPorAutorizar = _context.tblC_CadenaPrincipal.Where(x => x.estadoAutorizacion == EstadoAutorizacionCadenaEnum.VoBo).ToList().Count;

            var alertaExistente = _context.tblP_Alerta.FirstOrDefault(x => x.userRecibeID == 1164 && x.url == @"/Administrativo/Reportes/AutorizacionCadenaPrincipal");
            string mensajeAlerta = String.Format("Cadenas productivas por autorizar: {0}", documentosPendientesPorAutorizar);

            // Si la alerta ya existe, se actualiza.
            if (alertaExistente != null)
            {
                // Si ya no hay documentos pendientes, se marca como visto.
                if (documentosPendientesPorAutorizar == 0)
                {
                    alertaExistente.visto = true;
                }
                // Si no, se actualiza la cantidad
                else
                {
                    alertaExistente.msj = mensajeAlerta;
                    alertaExistente.visto = false;
                }
            }
            // Si no existe, la crea
            else
            {
                var nuevaAlerta = new tblP_Alerta
                {
                    userEnviaID = 13,
                    userRecibeID = 1164,
                    url = @"/Administrativo/Reportes/AutorizacionCadenaPrincipal",
                    sistemaID = 7,
                    msj = mensajeAlerta,
                    tipoAlerta = 2
                };
                _context.tblP_Alerta.Add(nuevaAlerta);
            }

            _context.SaveChanges();
        }

        private string AsignarNumNAFIN(int numProveedor)
        {
            var listaNAFIN = new CatNumNafinDAO().GetLstHanilitadosNumNafin();
            var proveedorNAFIN = listaNAFIN.Where(x => x.NumProveedor.Equals(numProveedor.ToString())).FirstOrDefault();
            return proveedorNAFIN != null ?
                proveedorNAFIN.NumNafin.Replace("\r\n", string.Empty) :
                numProveedor.ToString();
        }

        public List<tblC_CadenaPrincipal> GetDocumentosAplicados()
        {
            var lstAnt = _context.tblC_Anticipo.ToList().Where(w => !w.estatus).ToList();
            var lstPal = _context.tblC_CadenaPrincipal.Where(x => !x.estatus).ToList();
            lstPal.AddRange(lstAnt.Select(a => new tblC_CadenaPrincipal
            {
                banco = a.banco,
                centro_costos = a.centro_costos,
                estatus = false,
                factoraje = a.factoraje,
                fecha = a.fecha,
                fechaVencimiento = a.fechaVencimiento,
                id = 0,
                nombCC = a.nombCC,
                numNafin = a.numNafin,
                numProveedor = a.numProveedor,
                pagado = false,
                proveedor = a.proveedor,
                total = a.anticipo
            }));
            return lstPal;
        }
        public List<tblC_CadenaPrincipal> GetAllDocumentos()
        {
            return _context.tblC_CadenaPrincipal.ToList();
        }
        public tblC_CadenaPrincipal GetDocumento(int id)
        {
            return _context.tblC_CadenaPrincipal.Where(x => x.id == id).FirstOrDefault();
        }

        public bool Eliminar(tblC_CadenaPrincipal obj)
        {
            try
            {
                Delete(obj, (int)BitacoraEnum.CadenaPrincipal);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
