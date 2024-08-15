using Core.DAO.Subcontratistas;
using Core.DTO;
using Core.DTO.ControlObra;
using Core.DTO.ControlObra.EvaluacionSubcontratista;
using Core.DTO.Principal.Generales;
using Core.DTO.Subcontratistas.Evaluacion;
using Core.Entity.ControlObra;
using Core.Entity.ControlObra.Evaluacion;
using Core.Entity.Encuestas;
using Core.Entity.Principal.Alertas;
using Core.Entity.SubContratistas;
using Core.Enum.Principal;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Infrastructure.Utils;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Dapper;
using OfficeOpenXml.Drawing;
using Core.Enum.Principal.Bitacoras;
using System.Data;
using Core.DTO.Utils.Firmas;
using Core.Enum.ControlObra;
using Core.Entity.Principal.Menus;
using Core.DTO.Utils.Data;
using Core.Entity.Principal.Usuarios;
using Core.Enum;
using Core.Enum.Subcontratistas;
using Core.Entity.Principal.Multiempresa;
using System.Text.RegularExpressions;
using Core.Enum.Multiempresa;
using Core.Entity.Administrativo.Contratistas;
using Core.DTO.RecursosHumanos;
using Core.DTO.Subcontratistas;
using Core.DTO.Utils.ChartJS;
using Core.Enum.Principal.Alertas;
using Core.DTO.ControlObra.Dashboard;
using OfficeOpenXml.Table;
using OfficeOpenXml.Drawing.Chart;
using Core.DAO.ControlObra.Dashboard;

namespace Data.DAO.Subcontratistas
{
    public class EvaluacionSubcontratistaDAO : GenericDAO<tblCO_ADP_EvalSubConAsignacion>, IEvaluacionSubcontratistaDAO
    {
        #region SUB
        contextSigoplan db = new contextSigoplan();
        DynamicParameters lstParametros = new DynamicParameters();
        List<tblPUsuarioDTO> lstUsuariosExpediente = new List<tblPUsuarioDTO>();
        tblPUsuarioDTO objUsuariosExpediente = new tblPUsuarioDTO();

        private readonly string RutaBase = @"\\10.1.0.112\Proyecto\SUBCONTRATISTAS\CONTROL_OBRA";
        string NombreControlador = "EvaluacionSubcontratistaController";
        private const string RutaLocal = @"C:\Proyecto\SUBCONTRATISTAS\CONTROL_OBRA";
        private readonly string RutaControlObra;
        private readonly string RutaControlObraFirmas;
        private const string _NOMBRE_CONTROLADOR = "EvaluacionSubcontratistaController";
        private const int _SISTEMA = 0;

        private Dictionary<string, object> resultado = new Dictionary<string, object>();
        public EvaluacionSubcontratistaDAO()
        {
            resultado.Clear();

#if DEBUG
            RutaBase = RutaLocal;
#endif

            RutaControlObra = Path.Combine(RutaBase, @"EVALUACION_SUBCONTRATISTAS");
        }
        #region Captura
        #endregion

        #region Gestión
        public Dictionary<string, object> ObtenerProyectosParaFiltro()
        {
            using (var _ctx = new MainContext())
            {
                try
                {
                    #region VERSIÓN ADAN
                    bool todosLosCC = false;
                    string strQuery = string.Format(@"SELECT * FROM tblCO_ADP_Facultamientos WHERE idUsuario = {0} AND esActivo = {1}", (int)vSesiones.sesionUsuarioDTO.id, 1);
                    List<tblCO_ADP_Facultamientos> lstUsuarioFacultamientos = _context.Select<tblCO_ADP_Facultamientos>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                        consulta = strQuery
                    }).ToList();
                    List<string> lstCC = new List<string>();
                    List<string> lstUsuarioRelCC = lstUsuarioFacultamientos.Select(s => s.cc).ToList();
                    foreach (var item in lstUsuarioRelCC)
                    {
                        string[] SplitCC = item.Split(',');
                        foreach (var itemCC in SplitCC)
                        {
                            if (!string.IsNullOrEmpty(itemCC) && itemCC != ",")
                            {
                                string cc = itemCC.Replace(",", "");
                                lstCC.Add(string.Format("{0}", cc));
                            }
                        }
                    }

                    List<ComboBoxDTO> proyectos;
                    if (!todosLosCC)
                    {
                        proyectos = _context.tblP_CC.Where(w => lstCC.Contains(w.cc) && w.estatus).Select(x => new ComboBoxDTO
                        {
                            valor = x.cc,
                            texto = "[" + x.cc + "] " + x.descripcion.Trim(),
                            datas = new List<ComboBoxDataDTO>
                            {
                                new ComboBoxDataDTO {
                                    data = "id",
                                    valor = x.id.ToString()
                                }
                            }
                        }).OrderBy(x => x.valor).ToList();
                    }
                    else
                    {
                        proyectos = _context.tblP_CC.Where(w => w.estatus).Select(x => new ComboBoxDTO
                        {
                            valor = x.cc,
                            texto = "[" + x.cc + "] " + x.descripcion.Trim(),
                            datas = new List<ComboBoxDataDTO>
                            {
                                new ComboBoxDataDTO {
                                    data = "id",
                                    valor = x.id.ToString()
                                }
                            }
                        }).OrderBy(x => x.valor).ToList();
                    }

                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, proyectos);
                    #endregion
                }
                catch (Exception ex)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, ex.Message);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> ObtenerSubcontratistasParaFiltro(string proyecto)
        {
            using (var _ctx = new contextSigoplan())
            {
                try
                {
#if DEBUG
                    var subcontratistas = _ctx.tblX_Contrato.Where(x =>
                    x.cc == proyecto &&
                    x.estatus).Select(x => new ComboBoxDTO
                    {
                        valor = x.subcontratistaID.ToString(),
                        texto = x.subcontratista.nombre.Trim()
                    }).Distinct().OrderBy(x => x.texto).ToList();
#else
                    var subcontratistas = _ctx.tblX_Contrato.Where(x =>
                    x.cc == proyecto &&
                    x.estatus &&
                    x.subcontratista.estatus).Select(x => new ComboBoxDTO
                    {
                        valor = x.subcontratistaID.ToString(),
                        texto = x.subcontratista.nombre.Trim()
                    }).Distinct().OrderBy(x => x.texto).ToList();
#endif

                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, subcontratistas);
                }
                catch (Exception ex)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, ex.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> ObtenerEvaluacionesSubcontratistas(string proyecto, int? subcontratistaId)
        {
            using (var _ctx = new contextSigoplan())
            {
                try
                {
                    bool todosLosCC = false;
                    string strQuery = string.Format(@"SELECT * FROM tblCO_ADP_Facultamientos WHERE idUsuario = {0} AND esActivo = {1}", (int)vSesiones.sesionUsuarioDTO.id, 1);
                    List<tblCO_ADP_Facultamientos> lstUsuarioFacultamientos = _context.Select<tblCO_ADP_Facultamientos>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                        consulta = strQuery
                    }).ToList();
                    List<string> lstCC = new List<string>();
                    List<string> lstUsuarioRelCC = lstUsuarioFacultamientos.Select(s => s.cc).ToList();
                    foreach (var item in lstUsuarioRelCC)
                    {
                        string[] SplitCC = item.Split(',');
                        foreach (var itemCC in SplitCC)
                        {
                            if (!string.IsNullOrEmpty(itemCC) && itemCC != ",")
                            {
                                string cc = itemCC.Replace(",", "");
                                lstCC.Add(string.Format("{0}", cc));
                            }
                        }
                    }

                    List<EvaluacionParaFirmarDTO> evaluaciones = new List<EvaluacionParaFirmarDTO>();
                    if (!todosLosCC)
                    {
                        evaluaciones = _ctx.tblCO_ADP_EvalSubConAsignacion
                            .Where(x => !x.esActivo && x.statusAutorizacion == 3 && lstCC.Contains(x.cc) && (subcontratistaId.HasValue ? x.idSubContratista == subcontratistaId.Value : true) &&
                            (!string.IsNullOrEmpty(proyecto) ? x.cc == proyecto : true)).Select(x => new EvaluacionParaFirmarDTO
                            {
                                evaluacionId = x.id,
                                contratoId = x.idContrato,
                                numeroContrato = x.contrato.numeroContrato,
                                subcontratistaId = x.idSubContratista,
                                nombreSubcontratista = x.subcontratista.nombre,
                                proyecto = x.cc,
                                nombreProyecto = x.contrato.proyecto.nombre,
                                nombreEvaluacion = x.nombreEvaluacion,
                                firma = x.firma.FirstOrDefault(y => y.esActivo)
                            }).ToList();
                    }
                    else
                    {
                        evaluaciones = _ctx.tblCO_ADP_EvalSubConAsignacion
                            .Where(x => !x.esActivo && x.statusAutorizacion == 3 && (subcontratistaId.HasValue ? x.idSubContratista == subcontratistaId.Value : true) &&
                            (!string.IsNullOrEmpty(proyecto) ? x.cc == proyecto : true)).Select(x => new EvaluacionParaFirmarDTO
                            {
                                evaluacionId = x.id,
                                contratoId = x.idContrato,
                                numeroContrato = x.contrato.numeroContrato,
                                subcontratistaId = x.idSubContratista,
                                nombreSubcontratista = x.subcontratista.nombre,
                                proyecto = x.cc,
                                nombreProyecto = x.contrato.proyecto.nombre,
                                nombreEvaluacion = x.nombreEvaluacion,
                                firma = x.firma.FirstOrDefault(y => y.esActivo)
                            }).ToList();
                    }

                    var ccEvaluaciones = evaluaciones.Select(x => x.proyecto).ToList();
                    var firmantes = _ctx.tblX_Firmante.Where(x => x.esActivo && (x.orden == 0 || ccEvaluaciones.Contains(x.cc))).OrderBy(x => x.orden).ToList();

                    foreach (var evaluacion in evaluaciones)
                    {
                        if (evaluacion.firma == null)
                        {
                            evaluacion.elUsuarioPuedeFirmar = firmantes.First().esSubcontratista ? false : firmantes.First().usuarioId == vSesiones.sesionUsuarioDTO.id;
                        }
                        else
                        {
                            evaluacion.firmado = evaluacion.firma.tieneTodasLasFirmas;
                            var firmante = evaluacion.firma.firmantePendiente;
                            evaluacion.elUsuarioPuedeFirmar = evaluacion.firmado ? false : !firmante.esSubcontratista ? evaluacion.firma.firmantePendiente.usuarioId == vSesiones.sesionUsuarioDTO.id : false;
                        }

                        evaluacion.firma = null;
                    }

                    resultado.Add(SUCCESS, true);
                    resultado.Add(ITEMS, evaluaciones);
                }
                catch (Exception ex)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, ex.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> ObtenerEstatusFirmantes(int evaluacionId)
        {
            using (var _ctx = new contextSigoplan())
            {
                try
                {
                    var asignacion = _ctx.tblCO_ADP_EvalSubConAsignacion.FirstOrDefault(x => x.id == evaluacionId && !x.esActivo && x.statusAutorizacion == 3);
                    if (asignacion != null)
                    {
                        var firmantes = _ctx.tblX_Firmante.Where(x => x.esActivo && (x.orden == 0 || asignacion.cc == x.cc)).OrderBy(x => x.orden).ToList();

                        var firmaEvaluacion = _ctx.tblX_FirmaEvaluacion.FirstOrDefault(x => x.evaluacionId == evaluacionId && x.esActivo);

                        var estatusFirmas = new List<EstatusFirmasEvaluacionDTO>();

                        foreach (var firmante in firmantes)
                        {
                            var estatusFirma = new EstatusFirmasEvaluacionDTO();
                            estatusFirma.puesto = firmante.puesto;

                            if (firmante.esSubcontratista)
                            {
                                estatusFirma.nombreCompleto = asignacion.contrato.subcontratista.nombre.ToUpper();
                            }
                            else
                            {
                                estatusFirma.nombreCompleto = firmante.nombre + " " + firmante.apellidoPaterno +
                                (firmante.apellidoMaterno != null ? " " + firmante.apellidoMaterno : "");
                            }

                            if (firmaEvaluacion == null)
                            {
                                estatusFirma.estatusFirma = false;
                            }
                            else if (firmaEvaluacion.tieneTodasLasFirmas)
                            {
                                estatusFirma.estatusFirma = true;
                            }
                            else
                            {
                                estatusFirma.estatusFirma = firmante.orden < firmaEvaluacion.numeroOrdenFirmantePendiente.Value;
                            }

                            if (firmaEvaluacion != null)
                            {
                                var fechaFirma = firmaEvaluacion.detalle.FirstOrDefault(x => x.firmanteId == firmante.id);
                                if (fechaFirma != null)
                                {
                                    estatusFirma.fechaAutorizacion = fechaFirma.fechaCreacion;
                                }
                            }

                            estatusFirmas.Add(estatusFirma);
                        }

                        resultado.Add(SUCCESS, true);
                        resultado.Add(ITEMS, estatusFirmas);
                    }
                    else
                    {
                        throw new Exception("No existe la evaluación");
                    }
                }
                catch (Exception ex)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, ex.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> ObtenerFirmante(int evaluacionId)
        {
            using (var _ctx = new contextSigoplan())
            {
                try
                {
                    var asignacion = _ctx.tblCO_ADP_EvalSubConAsignacion.FirstOrDefault(x => x.id == evaluacionId && !x.esActivo && x.statusAutorizacion == 3);
                    if (asignacion != null)
                    {
                        var firmaPendiente = _ctx.tblX_FirmaEvaluacion.FirstOrDefault(x => x.evaluacionId == evaluacionId && x.esActivo);

                        var infoParaFirmar = new InfoParaFirmarDTO();

                        if (firmaPendiente != null)
                        {
                            if (firmaPendiente.tieneTodasLasFirmas)
                            {
                                throw new Exception("La evaluación ya tiene todas las firmas correspondientes");
                            }
                            else
                            {
                                if (firmaPendiente.firmantePendiente.esSubcontratista)
                                {
                                    throw new Exception("Favor de ingresar al modulo de subcontratistas para realizar la firma de subcontratista");
                                }
                                else
                                {
                                    infoParaFirmar.firmanteId = firmaPendiente.firmantePendienteId.Value;
                                    infoParaFirmar.puestoDelFirmante = firmaPendiente.firmantePendiente.puesto.ToUpper();
                                    infoParaFirmar.nombreCompletoFirmante = (firmaPendiente.firmantePendiente.nombre + " " +
                                        firmaPendiente.firmantePendiente.apellidoPaterno +
                                        (firmaPendiente.firmantePendiente.apellidoMaterno != null ?
                                        " " + firmaPendiente.firmantePendiente.apellidoMaterno : "")).ToUpper();
                                    infoParaFirmar.evaluacionId = evaluacionId;
                                }
                            }
                        }
                        else
                        {
                            var firmante = _ctx.tblX_Firmante.Where(x => x.esActivo && (x.orden == 0 || asignacion.cc == x.cc)).OrderBy(x => x.orden).First();
                            if (firmante.esSubcontratista)
                            {
                                throw new Exception("Favor de ingresar al modulo de subcontratistas para realizar la firma de subcontratista");
                            }
                            else
                            {
                                infoParaFirmar.firmanteId = firmante.id;
                                infoParaFirmar.puestoDelFirmante = firmante.puesto.ToUpper();
                                infoParaFirmar.nombreCompletoFirmante = (firmante.nombre + " " + firmante.apellidoPaterno +
                                    (firmante.apellidoMaterno != null ? " " + firmante.apellidoMaterno : "")).ToUpper();
                                infoParaFirmar.evaluacionId = evaluacionId;
                            }
                        }

                        resultado.Add(SUCCESS, true);
                        resultado.Add(ITEMS, infoParaFirmar);
                    }
                    else
                    {
                        throw new Exception("No existe la evaluación");
                    }
                }
                catch (Exception ex)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, ex.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> GuardarFirma(InformacionFirmaDigitalDTO firma)
        {
            using (var _ctx = new contextSigoplan())
            {
                using (var transaccion = _ctx.Database.BeginTransaction())
                {
                    try
                    {
                        var evaluacion = _ctx.tblCO_ADP_EvalSubConAsignacion.FirstOrDefault(x => x.id == firma.evaluacionId && !x.esActivo && x.statusAutorizacion == 3);
                        if (evaluacion != null)
                        {
                            var firmantes = _ctx.tblX_Firmante.Where(x => x.esActivo && (x.orden == 0 || evaluacion.cc == x.cc)).OrderBy(x => x.orden).ToList();

                            if (firmantes.Count == 0)
                            {
                                throw new Exception("No existen usuarios para firmar");
                            }
                            else
                            {
                                var nuevaFirma = new tblX_FirmaEvaluacion();
                                var nuevaFirmaDetalle = new tblX_FirmaEvaluacionDetalle();

                                var firmaPendiente = _ctx.tblX_FirmaEvaluacion.FirstOrDefault(x => x.evaluacionId == firma.evaluacionId && x.esActivo);
                                if (firmaPendiente != null)
                                {
                                    if (firmaPendiente.tieneTodasLasFirmas)
                                    {
                                        throw new Exception("La evaluación ya cuenta con todas las firmas correspondientes");
                                    }
                                    else
                                    {
                                        if (firmaPendiente.firmantePendiente.usuarioId != vSesiones.sesionUsuarioDTO.id)
                                        {
                                            throw new Exception("Usted no es el usuario correspondiente a esta firma");
                                        }

                                        if (firmaPendiente.firmantePendiente.esSubcontratista)
                                        {
                                            throw new Exception("Usted no es el usuario subcontratista que puede realizar esta firma");
                                        }

                                        firmaPendiente.tieneTodasLasFirmas = firma.firmanteId == firmantes.Last().id;
                                        firmaPendiente.firmantePendienteId = firmaPendiente.tieneTodasLasFirmas ? (int?)null :
                                            firmantes.First(x => x.orden > firmantes.First(y => y.id == firma.firmanteId).orden).id;
                                        firmaPendiente.numeroOrdenFirmantePendiente = firmaPendiente.tieneTodasLasFirmas ? (int?)null :
                                            firmantes.First(x => x.id == firmaPendiente.firmantePendienteId).orden;
                                        firmaPendiente.esActivo = true;
                                        firmaPendiente.fechaModificacion = DateTime.Now;
                                        firmaPendiente.usuarioModificacionId = vSesiones.sesionUsuarioDTO.id;
                                        nuevaFirma = firmaPendiente;
                                    }
                                }
                                else
                                {
                                    if (firmantes.First().usuarioId != vSesiones.sesionUsuarioDTO.id)
                                    {
                                        throw new Exception("Usted no es el usuario correspondiente a esta firma");
                                    }

                                    if (firmantes.First().esSubcontratista)
                                    {
                                        throw new Exception("Favor de ingresar al modulo de subcontratista para realizar la firma de subcontratista");
                                    }

                                    nuevaFirma.evaluacionId = evaluacion.id;
                                    nuevaFirma.evaluacion = evaluacion;
                                    nuevaFirma.tieneTodasLasFirmas = firmantes.Count == 1;
                                    if (firmantes.Count == 1)
                                    {
                                        nuevaFirma.firmantePendienteId = null;
                                    }
                                    else
                                    {
                                        var firmanteSiguiente = firmantes.Skip(1).First();
                                        nuevaFirma.firmantePendienteId = firmanteSiguiente.id;
                                        nuevaFirma.numeroOrdenFirmantePendiente = firmanteSiguiente.orden;
                                    }

                                    nuevaFirma.esActivo = true;
                                    nuevaFirma.fechaCreacion = DateTime.Now;
                                    nuevaFirma.fechaModificacion = nuevaFirma.fechaCreacion;
                                    nuevaFirma.usuarioCreacionId = vSesiones.sesionUsuarioDTO.id;
                                    nuevaFirma.usuarioModificacionId = nuevaFirma.usuarioCreacionId;

                                    _ctx.tblX_FirmaEvaluacion.Add(nuevaFirma);
                                }
                                _ctx.SaveChanges();

                                nuevaFirmaDetalle.firmaEvaluacionId = nuevaFirma.id;
                                nuevaFirmaDetalle.firmanteId = firma.firmanteId;
                                nuevaFirmaDetalle.esActivo = true;
                                nuevaFirmaDetalle.fechaCreacion = nuevaFirma.fechaModificacion;
                                nuevaFirmaDetalle.usuarioCreacionId = nuevaFirma.usuarioCreacionId;
                                nuevaFirmaDetalle.urlFirma = "";

                                _ctx.tblX_FirmaEvaluacionDetalle.Add(nuevaFirmaDetalle);
                                _ctx.SaveChanges();

                                #region archivo firma
                                var pathCompleto = "";
                                byte[] imagenBytes = Convert.FromBase64String(firma.firmaDigitalBase64.Split(',')[1]);
                                using (var ms = new MemoryStream(imagenBytes, 0, imagenBytes.Length))
                                {
                                    Image image = Image.FromStream(ms, true);

                                    var bmpImage = (System.Drawing.Bitmap)image;
                                    var bmp = new System.Drawing.Bitmap(bmpImage.Size.Width, bmpImage.Size.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                                    var gpx = System.Drawing.Graphics.FromImage(bmp);
                                    gpx.Clear(System.Drawing.Color.White);
                                    gpx.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
                                    gpx.DrawImage(bmpImage, 0, 0);

                                    Bitmap myBitmap;
                                    ImageCodecInfo myImageCodecInfo;
                                    System.Drawing.Imaging.Encoder myEncoder;
                                    EncoderParameter myEncoderParameter;
                                    EncoderParameters myEncoderParameters;

                                    myBitmap = new Bitmap(bmp, 800, 800);
                                    myImageCodecInfo = GetEncoderInfo("image/jpeg");
                                    myEncoder = System.Drawing.Imaging.Encoder.Quality;
                                    myEncoderParameters = new EncoderParameters(1);
                                    myEncoderParameter = new EncoderParameter(myEncoder, 90L);
                                    myEncoderParameters.Param[0] = myEncoderParameter;

                                    using (var memStream = new MemoryStream())
                                    {
                                        myBitmap.Save(memStream, myImageCodecInfo, myEncoderParameters);
                                        Image newImage = Image.FromStream(memStream);
                                        ImageAttributes imageAttributes = new ImageAttributes();
                                        using (Graphics g = Graphics.FromImage(newImage))
                                        {
                                            g.InterpolationMode =
                                                System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                                            g.DrawImage(newImage, new Rectangle(Point.Empty, newImage.Size), 0, 0,
                                                newImage.Width, newImage.Height, GraphicsUnit.Pixel, imageAttributes);
                                        }

#if DEBUG
                                        var folderFima = nuevaFirma.evaluacionId.ToString() + "_" + nuevaFirma.evaluacion.contrato.id.ToString();
                                        var nombreArchivo = "firmaDigital_" + nuevaFirmaDetalle.firmanteId.ToString() + nuevaFirma.fechaModificacion.ToString("yyyyMMddTHHmmssfff") + ".jpeg";
                                        pathCompleto = @"C:\Proyecto\SUBCONTRATISTAS\FIRMAS_EVALUACIONES\" + folderFima + @"\";
#else
                                        var folderFima = nuevaFirma.evaluacionId.ToString() + "_" + nuevaFirma.evaluacion.contrato.id.ToString();
                                        var nombreArchivo = "firmaDigital_" + nuevaFirmaDetalle.firmanteId.ToString() + nuevaFirma.fechaModificacion.ToString("yyyyMMddTHHmmssfff") + ".jpeg";
                                        pathCompleto = @"\\10.1.0.49\Proyecto\SUBCONTRATISTAS\FIRMAS_EVALUACIONES\" + folderFima + @"\";
#endif
                                        var directorio = new DirectoryInfo(pathCompleto);
                                        if (!directorio.Exists)
                                        {
                                            directorio.Create();
                                        }

                                        pathCompleto = Path.Combine(pathCompleto, nombreArchivo);

                                        newImage.Save(pathCompleto);
                                    }
                                }
                                #endregion

                                nuevaFirmaDetalle.urlFirma = pathCompleto;
                                _ctx.SaveChanges();

                                #region NOTIFICAR SIGUIENTE FIRMANTE
                                if (!nuevaFirmaDetalle.firmaEvaluacion.tieneTodasLasFirmas)
                                {
                                    if (nuevaFirmaDetalle.firmaEvaluacion.firmantePendienteId.HasValue)
                                    {
                                        var correo = new Infrastructure.DTO.CorreoDTO();
                                        correo.asunto = "Se ha realizado la evaluación " + nuevaFirmaDetalle.firmaEvaluacion.evaluacion.nombreEvaluacion + " y necesita su firma";
                                        correo.cuerpo = "<p>Se ha realizado la evaluación " + nuevaFirmaDetalle.firmaEvaluacion.evaluacion.nombreEvaluacion + " del contrato " +
                                            nuevaFirmaDetalle.firmaEvaluacion.evaluacion.contrato.numeroContrato + " correspondiente al subcontratista " +
                                            nuevaFirmaDetalle.firmaEvaluacion.evaluacion.contrato.subcontratista.nombre + " y necesita su firma digital</p>";
                                        correo.cuerpo += "<br>";
                                        correo.cuerpo += "<p>Puede ingresar al siguiente enlace para dirigirse al modulo de gestión de firmas de evaluaciones:</p>";
                                        correo.cuerpo += "http://sigoplan.construplan.com.mx/Subcontratistas/EvaluacionSubcontratista/Gestion";
                                        correo.cuerpo += "<br>";
                                        correo.cuerpo += "<p>O ingresar manualmente desde el sistema SIGOPLAN a:</p>";
                                        correo.cuerpo += "EMPRESA: CONSTRUPLAN / MODULO: ADMINISTRACIÓN DE PROYECTOS / MENU: CONTROL DE OBRA / EVALUACIÓN SUBCONTRATISTA / GESTIÓN DE FIRMA.";
#if DEBUG
                                        correo.correos = new List<string> { "adan.gonzalez@construplan.com.mx", "martin.zayas@construplan.com.mx" };
#else
                                        correo.correos = new List<string> { nuevaFirmaDetalle.firmaEvaluacion.firmantePendiente.correo };
#endif
                                        correo.Enviar();
                                    }
                                }
                                #endregion

                                if (nuevaFirma.tieneTodasLasFirmas)
                                {
                                    #region COMENTADO
                                    //var notificantes = _ctx.tblCO_ADP_Notificante.Where(x => x.esFijo || (x.cc == evaluacion.cc)).ToList();

                                    //                                    if (notificantes.Count > 0)
                                    //                                    {
                                    //                                        var correoNotificantes = new Infrastructure.DTO.CorreoDTO();
                                    //                                        correoNotificantes.asunto = "Evaluación " + evaluacion.nombreEvaluacion + " del subcontratista " + evaluacion.contrato.subcontratista.nombre + " completa";
                                    //                                        correoNotificantes.cuerpo = "<P>Se ha concluido la evaluación " + evaluacion.nombreEvaluacion + " del subcontratista " + evaluacion.contrato.subcontratista.nombre + " del contrato " + evaluacion.contrato.numeroContrato + "</p>";
                                    //#if DEBUG
                                    //                                        correoNotificantes.correos = new List<string> { "adan.gonzalez@construplan.com.mx", "martin.zayas@construplan.com.mx" };
                                    //#else
                                    //                                        correoNotificantes.correos = notificantes.Select(x => x.correo).ToList();
                                    //#endif
                                    //                                        correoNotificantes.Enviar();
                                    //}
                                    #endregion

                                    #region SE NOTIFICA A LOS USUARIOS NIVEL CONSULTA QUE TENGAN EL FACULTAMIENTO DEL CC DE LA EVALUACIÓN
                                    // SE OBTIENE EL ID DE LOS USUARIOS NIVEL CONSULTA
                                    string strQuery = string.Format(@"SELECT idUsuario FROM tblCO_ADP_Facultamientos WHERE tipo = {0} AND esActivo = {1} AND cc LIKE '{2}'", (int)TipoUsuariosEnum.consulta, 1, evaluacion.cc);
                                    List<int> lstUsuariosNivelConsulta = _context.Select<int>(new DapperDTO
                                    {
                                        baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                                        consulta = strQuery
                                        //parametros = new { tipo = (int)TipoUsuariosEnum.consulta, esActivo = true, cc = evaluacion.cc }
                                    }).ToList();

                                    // SE OBTIENE LOS CORREOS DE LOS USUARIOS NIVEL CONSULTA
                                    List<string> lstCorreos = _context.Select<string>(new DapperDTO
                                    {
                                        baseDatos = MainContextEnum.Construplan,
                                        consulta = @"SELECT correo FROM tblP_Usuario WHERE id IN (@id)",
                                        parametros = new { id = string.Join(",", lstUsuariosNivelConsulta) }
                                    }).ToList();
#if DEBUG
                                    lstCorreos = new List<string>();
                                    lstCorreos.Add("omar.nunez@construplan.com.mx");
#else
                                    lstCorreos = new List<string>();
                                    lstCorreos.Add("maricela.ortiz@construplan.com.mx");
                                    lstCorreos.Add("omar.nunez@construplan.com.mx");
#endif
                                    // ENVIAR CORREO
                                    List<FirmantesDTO> lstFirmantes = new List<FirmantesDTO>();
                                    string msg = GlobalUtils.cuerpoDeCorreoFormato("Prestadores de servicio", "Se ha concluido la evaluación", "", string.Format("Se notifica que se ha concluido la evaluación: {0}.", evaluacion.nombreEvaluacion), "Gracias por su atención.", "http://expediente.construplan.com.mx/", lstFirmantes);
                                    GlobalUtils.sendEmail(string.Format("{0}: Se ha concluido la evaluación", PersonalUtilities.GetNombreEmpresa()), msg, lstCorreos);
                                    #endregion
                                }

                                transaccion.Commit();

                                resultado.Add(SUCCESS, true);
                            }
                        }
                        else
                        {
                            throw new Exception("No existe la evaluación");
                        }
                    }
                    catch (Exception ex)
                    {
                        transaccion.Rollback();

                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, ex.Message);
                    }
                }
            }

            return resultado;
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }
        #endregion

        #region Dashboard

        public Dictionary<string, object> FillComboEspecialidad()
        {
            resultado = new Dictionary<string, object>();

            try
            {
                var listaEspecialidad = db.tblCOES_Especialidad.Where(x => x.registroActivo).ToList().Select(x => new ComboDTO
                {
                    Value = x.id.ToString(),
                    Text = x.descripcion
                }).OrderBy(x => x.Text).ToList();
                resultado.Add(ITEMS, listaEspecialidad);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                LogError(0, 0, NombreControlador, "FillComboEspecialidad", ex, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(MESSAGE, ex.Message);
                resultado.Add(SUCCESS, false);
            }

            return resultado;

        }


        //public Dictionary<string, object> CargarDatosGeneralesDashboard(List<string> lstfiltroCC, List<int> lstfiltroSubC, DateTime fechaFiltroInicio, DateTime fechaFiltroFin)
        //{

        //    resultado.Clear();

        //    try
        //    {
        //        var lstContratos = db.tblX_Contrato.Where(e => lstfiltroSubC.Contains(e.subcontratistaID) && lstfiltroCC.Contains(e.cc)).ToList();
        //        var lstIdsContratos = lstContratos.Select(e => e.id);

        //        //var lstEvaluaciones = db.tblCOES_Asignacion.Where(e => lstContratos.Contains(e.contrato_id)).ToList();





        //        resultado.Add(SUCCESS, true);
        //    }
        //    catch (Exception ex)
        //    {
        //        resultado.Add(SUCCESS, false);
        //    }


        //    return resultado;
        //}
        //public Dictionary<string, object> CargarDatosSecciones(List<string> cc, List<string> subContratistas, List<string> estados, List<string> municipios, List<string> especialidades, SeccionGeneralEvalSubEnum seccionGen, SeccionDetalleEvalSubEnum seccionDet)
        //{
        //    try
        //    {
        //        #region Filtros

        //        if ((cc == null || cc.Count == 0))
        //        {
        //            resultado.Add(SUCCESS, false);
        //            resultado.Add(MESSAGE, "La lista de centros de costos viene vacía.");
        //            return resultado;
        //        }

        //        var listaCCs = new List<string>();

        //        if (cc != null)
        //        {
        //            listaCCs.AddRange(cc);
        //        }

        //        #endregion


        //        switch (seccionGen)
        //        {

        //            case SeccionGeneralEvalSubEnum.MANDO_CUMPLIMIENTO_GLOBAL_SUBCONTRATISTA:
        //                AgregarEstadisticaCumplimientoGlobalSubcontratista(ref resultado);
        //                break;
        //            case SeccionGeneralEvalSubEnum.MANDO_CUMPLIMIENTO_GLOBAL_ELEMENTOS:
        //                AgregarEstadisticaCumplimientoGlobalElementos(ref resultado);
        //                break;
        //            case SeccionGeneralEvalSubEnum.MANDO_CUMPLIMIENTO_GLOBAL_EVALUACION:
        //                AgregarEstadisticaCumplimientoGlobalEvaluacion(ref resultado);
        //                break;

        //        }
        //        switch (seccionDet)
        //        {

        //            case SeccionDetalleEvalSubEnum.MANDO_CUMPLIMIENTO_CALIDAD:
        //                AgregarEstadisticasCumplimientoCalidad(ref resultado);
        //                break;

        //        }

        //    resultado.Add(SUCCESS, true);
        //    }
        //    catch (Exception e)
        //    {
        //        //LogError(0, 0, NombreControlador, "CargarDatosSecciones", e, AccionEnum.CONSULTA, 0, new { ccsCplan = ccsCplan, ccsArr = ccsArr, departamentosIDs = departamentosIDs, clasificaciones = clasificaciones, seccion = seccion });
        //        resultado.Clear();
        //        resultado.Add(SUCCESS, false);
        //        resultado.Add(MESSAGE, "Ocurrió un error al consultar la información para el dashboard.");
        //    }

        //    return resultado;
        //}

        #region Agregar Seccion General

        private void AgregarEstadisticaCumplimientoGlobalSubcontratista(ref Dictionary<string, object> resultado)
        {

            //var porcentajeProtocolo = new { labels = indicadoresProtocolos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            //var datosBarras = ObtenerPorcentajeClasificacionPorMando(indicadoresProtocolos, MandoEnum.Administrativo, false);
            //List<decimal> listaLinea = new List<decimal>();

            //foreach (var x in indicadoresProtocolos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList())
            //{
            //    listaLinea.Add(90m);
            //}

            //porcentajeProtocolo.datasets.Add(new DatasetDTO
            //{
            //    data = listaLinea,
            //    label = "Porcentaje Mínimo Requerido",
            //    type = "line"
            //});
            //porcentajeProtocolo.datasets.Add(datosBarras);
            //resultado.Add("porcentajeMandoAdministrativoProtocoloFatalidad", porcentajeProtocolo);

            //var cantidadProtocolo = new { labels = indicadoresProtocolos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            //cantidadProtocolo.datasets.AddRange(ObtenerCantidadClasificacionPorMando(indicadoresProtocolos, MandoEnum.Administrativo));
            //resultado.Add("chartCantidadMandoAdministrativoProtocoloFatalidad", cantidadProtocolo);
        }

        private void AgregarEstadisticaCumplimientoGlobalElementos(ref Dictionary<string, object> resultado)
        {
            //var porcentajeProtocolo = new { labels = indicadoresProtocolos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            //var datosBarras = ObtenerPorcentajeClasificacionPorMando(indicadoresProtocolos, MandoEnum.Administrativo, false);
            //List<decimal> listaLinea = new List<decimal>();

            //foreach (var x in indicadoresProtocolos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList())
            //{
            //    listaLinea.Add(90m);
            //}

            //porcentajeProtocolo.datasets.Add(new DatasetDTO
            //{
            //    data = listaLinea,
            //    label = "Porcentaje Mínimo Requerido",
            //    type = "line"
            //});
            //porcentajeProtocolo.datasets.Add(datosBarras);
            //resultado.Add("porcentajeMandoAdministrativoProtocoloFatalidad", porcentajeProtocolo);

            //var cantidadProtocolo = new { labels = indicadoresProtocolos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            //cantidadProtocolo.datasets.AddRange(ObtenerCantidadClasificacionPorMando(indicadoresProtocolos, MandoEnum.Administrativo));
            //resultado.Add("chartCantidadMandoAdministrativoProtocoloFatalidad", cantidadProtocolo);
        }

        private void AgregarEstadisticaCumplimientoGlobalEvaluacion(ref Dictionary<string, object> resultado)
        {
            //var porcentajeProtocolo = new { labels = indicadoresProtocolos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            //var datosBarras = ObtenerPorcentajeClasificacionPorMando(indicadoresProtocolos, MandoEnum.Administrativo, false);
            //List<decimal> listaLinea = new List<decimal>();

            //foreach (var x in indicadoresProtocolos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList())
            //{
            //    listaLinea.Add(90m);
            //}

            //porcentajeProtocolo.datasets.Add(new DatasetDTO
            //{
            //    data = listaLinea,
            //    label = "Porcentaje Mínimo Requerido",
            //    type = "line"
            //});
            //porcentajeProtocolo.datasets.Add(datosBarras);
            //resultado.Add("porcentajeMandoAdministrativoProtocoloFatalidad", porcentajeProtocolo);

            //var cantidadProtocolo = new { labels = indicadoresProtocolos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            //cantidadProtocolo.datasets.AddRange(ObtenerCantidadClasificacionPorMando(indicadoresProtocolos, MandoEnum.Administrativo));
            //resultado.Add("chartCantidadMandoAdministrativoProtocoloFatalidad", cantidadProtocolo);
        }

        #endregion



        #region Agregar Seccion Detalles


        private void AgregarEstadisticasCumplimientoCalidad(ref Dictionary<string, object> resultado)
        {
            //var indicadoresCumplimientoCalidad = ObtenerEstadisticasCalidad(); 
            var porcentajeCumplimientoCalidad = "";
            var datosBarras = "";

            //var indicadoresProtocolos = ObtenerIndicadoresPorClasificacion(indicadores, ClasificacionCursoEnum.ProtocoloFatalidad);
            //var porcentajeProtocolo = new { labels = indicadoresProtocolos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            //var datosBarras = ObtenerPorcentajeClasificacionPorMando(indicadoresProtocolos, MandoEnum.Administrativo, false);
            //List<decimal> listaLinea = new List<decimal>();

            //foreach (var x in indicadoresProtocolos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList())
            //{
            //    listaLinea.Add(90m);
            //}

            //porcentajeProtocolo.datasets.Add(new DatasetDTO
            //{
            //    data = listaLinea,
            //    label = "Porcentaje Mínimo Requerido",
            //    type = "line"
            //});
            //porcentajeProtocolo.datasets.Add(datosBarras);
            //resultado.Add("porcentajeMandoAdministrativoProtocoloFatalidad", porcentajeProtocolo);

            //var cantidadProtocolo = new { labels = indicadoresProtocolos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            //cantidadProtocolo.datasets.AddRange(ObtenerCantidadClasificacionPorMando(indicadoresProtocolos, MandoEnum.Administrativo));
            //resultado.Add("chartCantidadMandoAdministrativoProtocoloFatalidad", cantidadProtocolo);
        }

        private void AgregarEstadisticasCumplimientoPlaneacion(ref Dictionary<string, object> resultado)
        {
            var indicadoresCumplimientoPlaneacion = "";
            var porcentajeCumplimientoPlaneacion = "";
            var datosBarras = "";
            //var porcentajeProtocolo = new { labels = indicadoresProtocolos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            //var datosBarras = ObtenerPorcentajeClasificacionPorMando(indicadoresProtocolos, MandoEnum.Administrativo, false);
            //List<decimal> listaLinea = new List<decimal>();

            //foreach (var x in indicadoresProtocolos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList())
            //{
            //    listaLinea.Add(90m);
            //}

            //porcentajeProtocolo.datasets.Add(new DatasetDTO
            //{
            //    data = listaLinea,
            //    label = "Porcentaje Mínimo Requerido",
            //    type = "line"
            //});
            //porcentajeProtocolo.datasets.Add(datosBarras);
            //resultado.Add("porcentajeMandoAdministrativoProtocoloFatalidad", porcentajeProtocolo);

            //var cantidadProtocolo = new { labels = indicadoresProtocolos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            //cantidadProtocolo.datasets.AddRange(ObtenerCantidadClasificacionPorMando(indicadoresProtocolos, MandoEnum.Administrativo));
            //resultado.Add("chartCantidadMandoAdministrativoProtocoloFatalidad", cantidadProtocolo);
        }

        private void AgregarEstadisticasCumplimientoFacturacion(ref Dictionary<string, object> resultado)
        {
            var indicadoresCumplimientoFacturacion = "";
            var porcentajeCumplimientoFacturacion = "";
            var datosBarras = "";
            //var porcentajeProtocolo = new { labels = indicadoresProtocolos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            //var datosBarras = ObtenerPorcentajeClasificacionPorMando(indicadoresProtocolos, MandoEnum.Administrativo, false);
            //List<decimal> listaLinea = new List<decimal>();

            //foreach (var x in indicadoresProtocolos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList())
            //{
            //    listaLinea.Add(90m);
            //}

            //porcentajeProtocolo.datasets.Add(new DatasetDTO
            //{
            //    data = listaLinea,
            //    label = "Porcentaje Mínimo Requerido",
            //    type = "line"
            //});
            //porcentajeProtocolo.datasets.Add(datosBarras);
            //resultado.Add("porcentajeMandoAdministrativoProtocoloFatalidad", porcentajeProtocolo);

            //var cantidadProtocolo = new { labels = indicadoresProtocolos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            //cantidadProtocolo.datasets.AddRange(ObtenerCantidadClasificacionPorMando(indicadoresProtocolos, MandoEnum.Administrativo));
            //resultado.Add("chartCantidadMandoAdministrativoProtocoloFatalidad", cantidadProtocolo);
        }

        private void AgregarEstadisticasCumplimientoSeguridad(ref Dictionary<string, object> resultado)
        {
            var indicadoresCumplimientoSeguridad = "";
            var porcentajeCumplimientoSeguridad = "";
            var datosBarras = "";
            //var porcentajeProtocolo = new { labels = indicadoresProtocolos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            //var datosBarras = ObtenerPorcentajeClasificacionPorMando(indicadoresProtocolos, MandoEnum.Administrativo, false);
            //List<decimal> listaLinea = new List<decimal>();

            //foreach (var x in indicadoresProtocolos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList())
            //{
            //    listaLinea.Add(90m);
            //}

            //porcentajeProtocolo.datasets.Add(new DatasetDTO
            //{
            //    data = listaLinea,
            //    label = "Porcentaje Mínimo Requerido",
            //    type = "line"
            //});
            //porcentajeProtocolo.datasets.Add(datosBarras);
            //resultado.Add("porcentajeMandoAdministrativoProtocoloFatalidad", porcentajeProtocolo);

            //var cantidadProtocolo = new { labels = indicadoresProtocolos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            //cantidadProtocolo.datasets.AddRange(ObtenerCantidadClasificacionPorMando(indicadoresProtocolos, MandoEnum.Administrativo));
            //resultado.Add("chartCantidadMandoAdministrativoProtocoloFatalidad", cantidadProtocolo);
        }

        private void AgregarEstadisticasCumplimientoAmbiental(ref Dictionary<string, object> resultado)
        {
            var indicadoresCumplimientoAmbiental = "";
            var porcentajeCumplimientoAmbiental = "";
            var datosBarras = "";
            //var porcentajeProtocolo = new { labels = indicadoresProtocolos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            //var datosBarras = ObtenerPorcentajeClasificacionPorMando(indicadoresProtocolos, MandoEnum.Administrativo, false);
            //List<decimal> listaLinea = new List<decimal>();

            //foreach (var x in indicadoresProtocolos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList())
            //{
            //    listaLinea.Add(90m);
            //}

            //porcentajeProtocolo.datasets.Add(new DatasetDTO
            //{
            //    data = listaLinea,
            //    label = "Porcentaje Mínimo Requerido",
            //    type = "line"
            //});
            //porcentajeProtocolo.datasets.Add(datosBarras);
            //resultado.Add("porcentajeMandoAdministrativoProtocoloFatalidad", porcentajeProtocolo);

            //var cantidadProtocolo = new { labels = indicadoresProtocolos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            //cantidadProtocolo.datasets.AddRange(ObtenerCantidadClasificacionPorMando(indicadoresProtocolos, MandoEnum.Administrativo));
            //resultado.Add("chartCantidadMandoAdministrativoProtocoloFatalidad", cantidadProtocolo);
        }

        private void AgregarEstadisticasCumplimientoEfectividad(ref Dictionary<string, object> resultado)
        {
            var indicadoresCumplimientoEfectividad = "";
            var porcentajeCumplimientoEfectividad = "";
            var datosBarras = "";
            //var porcentajeProtocolo = new { labels = indicadoresProtocolos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            //var datosBarras = ObtenerPorcentajeClasificacionPorMando(indicadoresProtocolos, MandoEnum.Administrativo, false);
            //List<decimal> listaLinea = new List<decimal>();

            //foreach (var x in indicadoresProtocolos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList())
            //{
            //    listaLinea.Add(90m);
            //}

            //porcentajeProtocolo.datasets.Add(new DatasetDTO
            //{
            //    data = listaLinea,
            //    label = "Porcentaje Mínimo Requerido",
            //    type = "line"
            //});
            //porcentajeProtocolo.datasets.Add(datosBarras);
            //resultado.Add("porcentajeMandoAdministrativoProtocoloFatalidad", porcentajeProtocolo);

            //var cantidadProtocolo = new { labels = indicadoresProtocolos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            //cantidadProtocolo.datasets.AddRange(ObtenerCantidadClasificacionPorMando(indicadoresProtocolos, MandoEnum.Administrativo));
            //resultado.Add("chartCantidadMandoAdministrativoProtocoloFatalidad", cantidadProtocolo);
        }

        private void AgregarEstadisticasCumplimientoFuerza(ref Dictionary<string, object> resultado)
        {
            //var porcentajeProtocolo = new { labels = indicadoresProtocolos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            //var datosBarras = ObtenerPorcentajeClasificacionPorMando(indicadoresProtocolos, MandoEnum.Administrativo, false);
            //List<decimal> listaLinea = new List<decimal>();

            //foreach (var x in indicadoresProtocolos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList())
            //{
            //    listaLinea.Add(90m);
            //}

            //porcentajeProtocolo.datasets.Add(new DatasetDTO
            //{
            //    data = listaLinea,
            //    label = "Porcentaje Mínimo Requerido",
            //    type = "line"
            //});
            //porcentajeProtocolo.datasets.Add(datosBarras);
            //resultado.Add("porcentajeMandoAdministrativoProtocoloFatalidad", porcentajeProtocolo);

            //var cantidadProtocolo = new { labels = indicadoresProtocolos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            //cantidadProtocolo.datasets.AddRange(ObtenerCantidadClasificacionPorMando(indicadoresProtocolos, MandoEnum.Administrativo));
            //resultado.Add("chartCantidadMandoAdministrativoProtocoloFatalidad", cantidadProtocolo);
        }

        #endregion







        #endregion


        #region SUB CONTRATISTA

        public List<ComboDTO> getProyecto(int idUsuario)
        {
            List<ComboDTO> lstProyecto = new List<ComboDTO>();
            try
            {
                bool todosLosCC = false;
                tblCO_ADP_EvaluadorXcc objEvaluadoresPorCC = db.tblCO_ADP_EvaluadorXcc.Where(r => r.evaluador == idUsuario && r.esActivo).FirstOrDefault();
                string strQuery = string.Format(@"SELECT * FROM tblCO_ADP_Facultamientos WHERE idUsuario = {0} AND esActivo = {1}", (int)vSesiones.sesionUsuarioDTO.id, 1);
                List<tblCO_ADP_Facultamientos> lstUsuarioFacultamientos = _context.Select<tblCO_ADP_Facultamientos>(new DapperDTO
                {
                    baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                    consulta = strQuery
                }).ToList();

                if (lstUsuarioFacultamientos != null)
                {
                    List<string> lstCC = new List<string>();
                    List<string> lstUsuarioRelCC = lstUsuarioFacultamientos.Select(s => s.cc).ToList();
                    foreach (var item in lstUsuarioRelCC)
                    {
                        string[] SplitCC = item.Split(',');
                        foreach (var itemCC in SplitCC)
                        {
                            if (!string.IsNullOrEmpty(itemCC) && itemCC != ",")
                            {
                                string cc = itemCC.Replace(",", "");
                                lstCC.Add(string.Format("'{0}'", cc));
                            }
                        }
                    }

                    strQuery = string.Empty;
                    if (!todosLosCC)
                        strQuery = string.Format(@"SELECT id, cc, descripcion FROM tblP_CC WHERE cc IN ({0})", string.Join(",", lstCC));
                    else
                        strQuery = string.Format(@"SELECT id, cc, descripcion FROM tblP_CC WHERE estatus = {0}", 1);

                    List<tblP_CC> listaCC = _context.Select<tblP_CC>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Construplan,
                        consulta = strQuery
                    }).ToList();

                    lstProyecto = new List<ComboDTO>();
                    foreach (var item in listaCC)
                    {
                        ComboDTO objDTO = new ComboDTO();
                        objDTO.Value = item.id.ToString();
                        objDTO.Text = string.Format("[{0}] {1}", !string.IsNullOrEmpty(item.cc) ? item.cc.Trim().ToUpper() : string.Empty, !string.IsNullOrEmpty(item.descripcion) ? item.descripcion.Trim().ToUpper() : string.Empty);
                        objDTO.Prefijo = !string.IsNullOrEmpty(item.cc) ? item.cc.Trim().ToUpper() : string.Empty;
                        lstProyecto.Add(objDTO);
                    }
                }
                else if (objEvaluadoresPorCC != null)
                {
                    var lst = obtenerTodolosCC(objEvaluadoresPorCC.cc);
                    lstProyecto = _context.tblP_CC.Where(r => r.estatus && lst.Contains(r.cc)).ToList().Select(y => new ComboDTO
                    {
                        Value = y.cc.ToString(),
                        Text = y.descripcion
                    }).ToList();
                }
                else
                {
                    lstProyecto = _context.tblP_CC.Where(r => r.estatus).ToList().Select(y => new ComboDTO
                    {
                        Value = y.cc.ToString(),
                        Text = y.descripcion
                    }).ToList();
                }

                //if (lstProyecto.Count() > 0)
                //{
                //    string cc = string.Empty;
                //    string descCC = string.Empty;
                //    foreach (var item in lstProyecto)
                //    {
                //        cc = string.Empty; 
                //        descCC = string.Empty;
                //        if (!string.IsNullOrEmpty(item.Value) && !string.IsNullOrEmpty(item.Text))
                //        {
                //            cc = item.Value.Trim().ToUpper();
                //            descCC = item.Text.Trim().ToUpper();

                //            item.Value = cc;
                //            item.Text = "[" + cc + "] " + descCC;
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                //LogError(0, 0, NombreControlador, "getProyecto", "", AccionEnum.FILLCOMBO, 0, 0);
                throw;
            }
            return lstProyecto.OrderBy(o => o.Text).ToList();
        }
        public List<ComboDTO> getSubContratistas(string idCC)
        {
            #region VERSIÓN ADAN
            //var lstProyecto = new List<ComboDTO>();
            #endregion
            try
            {
                #region VERSIÓN ADAN
                //if (AreaCuenta == null)
                //    AreaCuenta = "";

                //List<ContratoSubContratistaDTO> lstContrato = new List<ContratoSubContratistaDTO>();

                //List<tblX_SubContratista> lstSubContratistas = db.tblX_SubContratista.Where(r => r.estatus).ToList();
                //var lstContratos = db.tblX_Contrato.Where(r => r.estatus).ToList().Select(y => y.subcontratistaID).ToList();
                //lstContrato = lstSubContratistas.Where(y => lstContratos.Contains(y.id)).Select(y => new ContratoSubContratistaDTO
                //{
                //    id = y.id,
                //    nombre = y.nombre,
                //    direccion = y.direccion,
                //}).ToList();

                //lstProyecto = lstContrato.ToList().Select(y => new ComboDTO
                //{
                //    Value = y.id.ToString(),
                //    Text = y.nombre
                //}).ToList();
                #endregion

                #region VERSIÓN v2
                if (string.IsNullOrEmpty(idCC))
                    return null;

                // SE OBTIENE EL CC SELECCIONADO
                tblP_CC objCC = _context.Select<tblP_CC>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Construplan,
                    consulta = @"SELECT cc FROM tblP_CC WHERE id = @idCC",
                    parametros = new { idCC = idCC }
                }).FirstOrDefault();

                string strQuery = string.Format(@"SELECT t1.id AS Value, t1.nombre AS Text
	                                                    FROM tblX_SubContratista AS t1
	                                                    INNER JOIN tblX_Contrato AS t2 ON t1.id = t2.subcontratistaID
		                                                    WHERE t1.estatus = 1 AND t2.estatus = 1 AND t2.cc = '{0}'
			                                                    GROUP BY t1.id, t1.nombre", objCC.cc);
                List<ComboDTO> lstSubcontratistas = _context.Select<ComboDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                    consulta = strQuery,
                    parametros = new { cc = objCC.cc }
                }).ToList();

                List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                foreach (var item in lstSubcontratistas)
                {
                    ComboDTO obj = new ComboDTO();
                    obj.Value = item.Value;
                    obj.Text = !string.IsNullOrEmpty(item.Text) ? item.Text.Trim().ToUpper() : string.Empty;
                    lstComboDTO.Add(obj);
                }

                return lstComboDTO;
                #endregion
            }
            catch (Exception ex)
            {
                throw;
            }
            #region VERSIÓN ADAN
            //return lstProyecto.OrderBy(o => o.Text).ToList();
            #endregion
        }
        public SubContratistasDTO getTblSubContratista(SubContratistasDTO parametros)
        {
            return null;
        }
        public SubContratistasDTO addEditSubContratista(List<HttpPostedFileBase> Archivo, List<SubContratistasDTO> parametros)
        {
            string urlArchivo = RutaControlObra;
            string FechaActual = Convert.ToString(parametros.Select(y => y.idSubContratista).FirstOrDefault().ToString() + "CO" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute);
            urlArchivo = Path.Combine(urlArchivo, FechaActual);
            if (Archivo != null)
            {
                foreach (var Arch in Archivo)
                {
                    foreach (var param in parametros)
                    {
                        if (Arch.FileName == param.Archivo)
                        {
                            param.File = Arch;
                        }
                    }
                }

            }

            var listaRutaArchivos = new List<Tuple<HttpPostedFileBase, string, string>>();
            tblCO_ADP_EvalSubContratistaDet objAdd = new tblCO_ADP_EvalSubContratistaDet();

            foreach (var item in parametros)
            {
                #region ELIMINADO
                //if (objAdd == null)
                //{
                //    objAdd = new tblCO_ADP_EvalSubContratista();
                //    objAdd.tipoEvaluacion = parametros.Select(y => y.tipoEvaluacion).FirstOrDefault();
                //    objAdd.idAreaCuenta = parametros.Select(y => y.AreaCuenta).FirstOrDefault();
                //    objAdd.idSubContratista = parametros.Select(y => y.idSubContratista).FirstOrDefault();
                //    objAdd.Calificacion = parametros.Select(y => y.Calificacion).FirstOrDefault();

                //    _context.tblCO_ADP_EvalSubContratista.Add(objAdd);
                //    _context.SaveChanges();


                //    foreach (var item in parametros)
                //    {
                //        tblCO_ADP_EvalSubContratistaDet objAddDet = new tblCO_ADP_EvalSubContratistaDet();
                //        objAddDet = _context.tblCO_ADP_EvalSubContratistaDet.Where(r => r.id == item.id).FirstOrDefault();
                //        if (objAddDet == null)
                //        {
                //            objAddDet = new tblCO_ADP_EvalSubContratistaDet();
                //            objAddDet.idEvaluacion = objAdd.id;
                //            objAddDet.fechaDocumento = DateTime.Now;
                //            objAddDet.idRow = item.idRow;
                //            objAddDet.calificacion = 0;
                //            objAddDet.tipoEvaluacion = item.tipoEvaluacion;
                //            if (item.Archivo != null)
                //            {
                //                objAddDet.rutaArchivo = Path.Combine(urlArchivo, item.Archivo);
                //            }
                //            _context.tblCO_ADP_EvalSubContratistaDet.Add(objAddDet);
                //            _context.SaveChanges();

                //            if (item.File != null)
                //            {
                //                listaRutaArchivos.Add(Tuple.Create(item.File, Path.Combine(urlArchivo, item.Archivo), urlArchivo));

                //            }
                //        }
                //    }

                //}
                //else
                #endregion
                objAdd = db.tblCO_ADP_EvalSubContratistaDet.Where(r => r.idRow == item.idRow && r.id == item.idEvaluacion).FirstOrDefault();
                if (objAdd != null)
                {
                    objAdd.fechaDocumento = DateTime.Now;
                    if (item.Archivo != null)
                    {
                        objAdd.rutaArchivo = Path.Combine(urlArchivo, item.Archivo);
                    }
                    _context.SaveChanges();

                    if (item.File != null)
                    {
                        listaRutaArchivos.Add(Tuple.Create(item.File, Path.Combine(urlArchivo, item.Archivo), urlArchivo));

                    }
                }
            }

            foreach (var arch in listaRutaArchivos)
            {
                if (SaveHTTPPostedFile(arch.Item1, arch.Item2, arch.Item3) == false)
                {
                    resultado.Clear();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Ocurrió un error al guardar los archivos en el servidor.");
                }
            }
            int idSubContratista = parametros.Select(y => y.idSubContratista).FirstOrDefault();
            int tipoEvaluacion = parametros.Select(y => y.tipoEvaluacion).FirstOrDefault();
            int id = parametros.Select(y => y.id).FirstOrDefault();
            var objSubContra = db.tblX_SubContratista.Where(r => r.id == idSubContratista).FirstOrDefault();
            var objTipoEvaluacion = db.tblCO_ADP_EvaluacionDiv.Where(r => r.id == tipoEvaluacion).FirstOrDefault();
            if (objSubContra != null)
            {
                List<string> emails = new List<string>();
                List<tblPUsuarioDTO> lstUsurio = new List<tblPUsuarioDTO>();
                string sql = @"SELECT * FROM tblP_Usuario WHERE tipo=3 AND estatus='true'";
                using (var ctx = new SqlConnection(ConextSigoDapper.conexionSUBCONTRATISTAS()))
                {
                    ctx.Open();
                    lstUsurio = ctx.Query<tblPUsuarioDTO>(sql, null, null, true, 300).ToList();
                    ctx.Close();
                }
                List<tblPUsuarioDTO> lstUsurio2 = new List<tblPUsuarioDTO>();
                string sql2 = @"SELECT * FROM tblP_Usuario WHERE tipo=14 AND estatus='true'";
                using (var ctx = new SqlConnection(ConextSigoDapper.conexionSUBCONTRATISTAS()))
                {
                    ctx.Open();
                    lstUsurio2 = ctx.Query<tblPUsuarioDTO>(sql2, null, null, true, 300).ToList();
                    ctx.Close();
                }

                var lstUsusariosPrestadores = lstUsurio2.Where(r => r.idPadre == idSubContratista).ToList();
                foreach (var item in lstUsusariosPrestadores)
                {
                    emails.Add(item.correo);
                }
                var objAsignacion = db.tblCO_ADP_EvalSubConAsignacion.FirstOrDefault(r => r.id == id);
                var objUsuario = lstUsurio.Where(r => r._user == objSubContra.rfc).FirstOrDefault();
                emails.Add(objUsuario.correo);
                string Subject = "CONTESTACION PARA LA EVALUACION.";
                string msg2 = CuerpoCorreo(objAsignacion, objUsuario);
                List<FirmantesDTO> lstFirmantes = new List<FirmantesDTO>();
                string msg = GlobalUtils.cuerpoDeCorreoFormato("Prestadores de servicio", "Evaluaciones", "Asginacion de la evaluacion para prestadores de servicio", msg2, "Gracias por su atencion.", "http://expediente.construplan.com.mx/", lstFirmantes);

#if DEBUG
                emails = new List<string>();
                emails.Add("omar.nunez@construplan.com.mx");
#else
                emails = new List<string>();
                emails.Add("maricela.ortiz@construplan.com.mx");
                emails.Add("omar.nunez@construplan.com.mx");
#endif
                GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), Subject), msg, emails);
            }

            return null;
        }
        public List<tblCO_ADP_EvalSubConAsignacion> obtenerTodasLasASinaciones(int idAsignacion)
        {
            List<tblCO_ADP_EvalSubConAsignacion> lst = new List<tblCO_ADP_EvalSubConAsignacion>();
            try
            {
                var objAsignacion = db.tblCO_ADP_EvalSubConAsignacion.Where(r => r.id == idAsignacion).FirstOrDefault();
                lst = db.tblCO_ADP_EvalSubConAsignacion.Where(r => r.idContrato == objAsignacion.idContrato).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return lst;
        }
        public static bool SaveHTTPPostedFile(HttpPostedFileBase archivo, string ruta, string Archivo)
        {
            try
            {
                byte[] data;

                if (!Directory.Exists(Archivo))
                {
                    Directory.CreateDirectory(Archivo);
                }

                using (Stream inputStream = archivo.InputStream)
                {
                    MemoryStream memoryStream = inputStream as MemoryStream;
                    if (memoryStream == null)
                    {
                        memoryStream = new MemoryStream();
                        inputStream.CopyTo(memoryStream);
                    }
                    data = memoryStream.ToArray();
                }

                File.WriteAllBytes(ruta, data);
            }
            catch (Exception)
            {
                return false;
            }

            return File.Exists(ruta);
        }
        public List<SubContratistasDTO> CargarArchivosXSubcontratista(SubContratistasDTO parametros)
        {
            List<SubContratistasDTO> lstDetalle = new List<SubContratistasDTO>();
            try
            {
                var idButtonElemento = db.tblCO_ADP_EvaluacionDiv.First(x => x.id == parametros.tipoEvaluacion).idbutton;
                var objSubAsig = db.tblCO_ADP_EvalSubConAsignacion.Where(r => r.idSubContratista == parametros.idSubContratista && r.cc == parametros.cc && r.esActivo == true && r.statusAutorizacion == 2).FirstOrDefault();
                if (objSubAsig != null)
                {
                    var objSubContratistas = db.tblCO_ADP_EvalSubContratista.Where(r => r.idSubConAsignacion == objSubAsig.id && r.tipoEvaluacion == parametros.tipoEvaluacion).FirstOrDefault();
                    if (objSubContratistas != null)
                    {
                        var lstDatos = db.tblCO_ADP_EvalSubContratistaDet.Where(r => r.idEvaluacion == objSubContratistas.id && r.tipoEvaluacion == parametros.tipoEvaluacion).ToList();
                        if (lstDatos.Count() != 0)
                        {
                            lstDetalle = lstDatos.Select(y => new SubContratistasDTO
                            {
                                id = y.id,
                                idRow = y.idRow,
                                tipoEvaluacion = y.tipoEvaluacion,
                                idEvaluacion = y.idEvaluacion,
                                evaluacionPendiente = y.evaluacionPendiente,
                                rutaArchivo = obtenerNombreRuta(y.rutaArchivo),
                                Calificacion = db.tblCO_ADP_EvalSubContratista.Where(n => n.id == y.idEvaluacion).Select(P => P.Calificacion).FirstOrDefault(),
                                idSubContratista = db.tblCO_ADP_EvalSubContratista.Where(n => n.id == y.idEvaluacion).Select(P => P.idSubContratista).FirstOrDefault(),
                                estaEvaluado = y.calificacion != 0,
                                btnElemento = idButtonElemento
                            }).ToList();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            return lstDetalle;
        }
        public List<SubContratistasDTO> CargarArchivosSubcontratista(SubContratistasDTO parametros)
        {
            List<SubContratistasDTO> lstDetalle = new List<SubContratistasDTO>();
            try
            {
                int idSubcontratista = db.tblX_SubContratista.Where(r => r.rfc == parametros.RFC).FirstOrDefault().id;
                parametros.idSubContratista = idSubcontratista;

                var objAsignacion = db.tblCO_ADP_EvalSubConAsignacion.Where(r => r.idSubContratista == parametros.idSubContratista && r.id == parametros.idAsignacion && r.esActivo).FirstOrDefault();

                if (objAsignacion != null)
                {
                    var objSubContratista = db.tblCO_ADP_EvalSubContratista.Where(r => r.idSubContratista == parametros.idSubContratista && r.idSubConAsignacion == objAsignacion.id && r.tipoEvaluacion == parametros.tipoEvaluacion).FirstOrDefault();

                    if (objSubContratista != null)
                    {
                        var lstDatos = db.tblCO_ADP_EvalSubContratistaDet.Where(r => r.tipoEvaluacion == parametros.tipoEvaluacion && r.idEvaluacion == objSubContratista.id).ToList();
                        if (lstDatos.Count() != 0)
                        {
                            lstDetalle = lstDatos.Select(y => new SubContratistasDTO
                            {
                                id = y.id,
                                idRow = y.idRow,
                                tipoEvaluacion = y.tipoEvaluacion,
                                idEvaluacion = y.idEvaluacion,
                                evaluacionPendiente = y.evaluacionPendiente,
                                rutaArchivo = obtenerNombreRuta(y.rutaArchivo),
                                Calificacion = db.tblCO_ADP_EvalSubContratista.Where(n => n.id == y.idEvaluacion).Select(P => P.Calificacion).FirstOrDefault(),
                                idSubContratista = db.tblCO_ADP_EvalSubContratista.Where(n => n.id == y.idEvaluacion).Select(P => P.idSubContratista).FirstOrDefault(),
                                opcional = y.opcional
                            }).ToList();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            return lstDetalle;
        }
        public string obtenerNombreRuta(string rutaArchivo)
        {
            string r = "";
#if DEBUG
            r = rutaArchivo == null ? "Ningún archivo seleccionado" : rutaArchivo.Split('\\')[6];
#else
            r = rutaArchivo == null ? "Ningún archivo seleccionado" : rutaArchivo.Split('\\')[8];
#endif
            return r;
        }
        public List<SubContratistasDTO> CargarArchivosXSubcontratistaEvaluacion(SubContratistasDTO parametros)
        {
            List<SubContratistasDTO> lstDetalle = new List<SubContratistasDTO>();
            try
            {
                var objSubContratista = db.tblCO_ADP_EvalSubContratista.Where(r => r.idSubContratista == parametros.idSubContratista).FirstOrDefault();
                if (objSubContratista != null)
                {
                    var lstDatos = db.tblCO_ADP_EvalSubContratistaDet.Where(r => r.tipoEvaluacion == parametros.tipoEvaluacion).ToList();
                    if (lstDatos.Count() != 0)
                    {
                        lstDetalle = lstDatos.Select(y => new SubContratistasDTO
                        {
                            id = y.id,
                            idRow = y.idRow,
                            tipoEvaluacion = y.tipoEvaluacion,
                            idEvaluacion = y.idEvaluacion,
                            rutaArchivo = obtenerNombreRuta(y.rutaArchivo),
                            Calificacion = db.tblCO_ADP_EvalSubContratista.Where(n => n.id == y.idEvaluacion).Select(P => P.Calificacion).FirstOrDefault(),
                            idSubContratista = db.tblCO_ADP_EvalSubContratista.Where(n => n.id == y.idEvaluacion).Select(P => P.idSubContratista).FirstOrDefault(),
                        }).ToList();
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            return lstDetalle;
        }
        public List<SubContratistasDTO> ObtenerTblAutorizacion(SubContratistasDTO parametros)
        {
            List<SubContratistasDTO> lstDatos = new List<SubContratistasDTO>();
            try
            {
                lstDatos = db.tblCO_ADP_EvalSubContratista.Where(r => r.idSubContratista == parametros.idSubContratista).ToList().Select(y => new SubContratistasDTO
                {
                    id = y.id,
                    Calificacion = y.Calificacion,
                    fechaAutorizacion = y.fechaAutorizacion,
                    tipoEvaluacionDesc = obtenerTipoEvaluacion(db.tblCO_ADP_EvalSubContratistaDet.Where(r => r.idEvaluacion == y.id).Select(n => n.tipoEvaluacion).FirstOrDefault())
                }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return lstDatos;
        }
        public string obtenerTipoEvaluacion(int tipoEvaluacion)
        {
            string Desc = "";
            switch (tipoEvaluacion)
            {
                case 1:
                    Desc = "Calidad ";
                    break;
                case 2:
                    Desc = "Planeacion/Programa";
                    break;
                case 3:
                    Desc = "Facturacion";
                    break;
                case 4:
                    Desc = "Seguridad";
                    break;
                case 5:
                    Desc = "Ambiental";
                    break;
                case 6:
                    Desc = "Efectivo";
                    break;
                case 7:
                    Desc = "Fuerza de trabajo y Atencion al cliente";
                    break;
            }
            return Desc;
        }
        public List<DivicionesMenuDTO> obtenerDiviciones(int idPlantilla)
        {
            List<DivicionesMenuDTO> lstDatos = new List<DivicionesMenuDTO>();
            try
            {
                lstDatos = db.tblCO_ADP_EvaluacionDiv.Where(r => r.esActivo && r.SubContratista == false && r.idPlantilla == idPlantilla).ToList().Select(y => new DivicionesMenuDTO
                {
                    id = y.id,
                    idbutton = y.idbutton,
                    idsection = y.idsection,
                    toltips = y.toltips,
                    descripcion = y.descripcion,
                    esActivo = y.esActivo,
                    orden = y.orden,
                    important = y.important,
                    idEvaluador = y.idEvaluador,
                }).OrderBy(x => x.orden).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
            return lstDatos;
        }
        public Dictionary<string, object> GetPlantillasCreadas(int plantilla_id, int contrato_id)
        {
            resultado = new Dictionary<string, object>();

            try
            {
                var listaRelacionPlantillaContrato = db.tblCOES_PlantillatblX_Contrato.Where(x => x.registroActivo && (plantilla_id > 0 ? x.plantilla_id == plantilla_id : true) && (contrato_id > 0 ? x.contrato_id == contrato_id : true)).ToList();

                var listaRelacion = (
                  from rel in listaRelacionPlantillaContrato
                  join plant in db.tblCOES_Plantilla.Where(x => x.registroActivo).ToList() on rel.plantilla_id equals plant.id
                  join cont in db.tblX_Contrato.Where(x => x.estatus).Select(x => new { id = x.id, numeroContrato = x.numeroContrato }).ToList() on rel.contrato_id equals cont.id
                  select new { rel, plant, cont }
                ).ToList();

                var listaPlantillas = listaRelacion.GroupBy(x => x.plant.id).Select(x => new
                {
                    id = x.Key,
                    nombre = x.First().plant.nombre,
                    tipo = x.First().plant.tipo,
                    tipoDesc = x.First().plant.tipo.GetDescription(),
                    plantillaBase = x.First().plant.plantillaBase,
                    contratos = string.Join(",", x.Select(y => y.cont.numeroContrato).ToList()),
                    grp = x.ToList()
                });

                resultado.Add("data", listaPlantillas);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "GetPlantillasCreadas", e, AccionEnum.FILLCOMBO, 0, new { plantilla_id = plantilla_id, contrato_id = contrato_id });
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }
        public List<string> obtenerNumeroContratosID(string rcon)
        {
            List<string> rt = new List<string>();
            if (rcon != "")
            {
                var r = rcon.Split(',');
                foreach (var item in r)
                {
                    if (item != "")
                    {
                        int id = Convert.ToInt32(item);
                        var contrato = Convert.ToString(db.tblX_Contrato.Where(f => f.id == id).FirstOrDefault().id);
                        rt.Add(contrato);
                    }
                }
            }
            return rt;
        }
        public string obtenerNumeroContratos(string rcon)
        {
            string rt = "";
            if (rcon != "")
            {
                var r = rcon.Split(',');
                foreach (var item in r)
                {
                    if (item != "")
                    {
                        int id = Convert.ToInt32(item);
                        var contrato = db.tblX_Contrato.Where(f => f.id == id).FirstOrDefault().numeroContrato;
                        rt += contrato + ",";
                    }
                }
            }
            return rt.TrimEnd(',');
        }
        public Dictionary<string, object> eliminarPlantilla(int id)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                var objPlantilla = db.tblCO_ADP_EvaluacionPlantilla.Where(r => r.id == id).FirstOrDefault();
                var lstDiv = db.tblCO_ADP_EvaluacionDiv.Where(r => r.idPlantilla == id).ToList();



                foreach (var item in lstDiv)
                {
                    var lstReq = db.tblCO_ADP_EvaluacionReq.Where(r => r.idDiv == item.id).ToList();

                    db.tblCO_ADP_EvaluacionReq.RemoveRange(lstReq);
                    db.SaveChanges();
                }
                db.tblCO_ADP_EvaluacionPlantilla.Remove(objPlantilla);
                db.SaveChanges();

                db.tblCO_ADP_EvaluacionDiv.RemoveRange(lstDiv);
                db.SaveChanges();

                resultado.Add(ITEMS, "Registro borrado con exito.");
                resultado.Add(SUCCESS, true);

            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                throw;
            }

            return resultado;
        }
        public Dictionary<string, object> addEditPlantilla(DivicionesMenuDTO objDTO)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region VALIDACIONES
                    if (objDTO.nombrePlantilla == null) { throw new Exception("Es necesario indicar el nombre de la plantilla."); }
                    if (objDTO.contratos == null) { throw new Exception("Es necesario seleccionar un contrato."); }
                    #endregion

                    #region SE VERIFICA SI EL CONTRATO SE ENCUENTRA CON UNA EVALUACIÓN ASIGNADA
                    string[] splitContratosID = objDTO.contratos.Split(',');
                    List<int> lstContratosID = new List<int>();
                    foreach (var item in splitContratosID)
                    {
                        if (Regex.IsMatch(item, @"^[0-9]+$"))
                            lstContratosID.Add(Convert.ToInt32(item));
                    }

                    // SE OBTIENE LISTADO DE EVALUACIONES ASIGNADAS
                    List<tblCO_ADP_EvalSubConAsignacion> lstEvaluacionesAsignada = db.tblCO_ADP_EvalSubConAsignacion.Where(w => w.esActivo).ToList();
                    objDTO.contratos = string.Empty;
                    foreach (var item in lstContratosID)
                    {
                        tblCO_ADP_EvalSubConAsignacion contratoTieneEvaluacionAsignada = lstEvaluacionesAsignada.Where(w => w.idContrato == item).FirstOrDefault();
                        if (contratoTieneEvaluacionAsignada == null)
                            objDTO.contratos += string.Format("{0},", item.ToString());
                        else
                            throw new Exception("El contrato seleccionado, ya cuenta con evaluaciones activas. Por lo que no se le puede crear nueva plantilla.");
                    }
                    #endregion

                    #region SE REGISTRA PLANTILLA DEL CONTRATO SELECCIONADO
                    tblCO_ADP_EvaluacionPlantilla objCE = new tblCO_ADP_EvaluacionPlantilla();
                    if (objDTO.idPlantilla <= 0)
                    {
                        objCE.nombrePlantilla = objDTO.nombrePlantilla.Trim().ToUpper();
                        objCE.contratos = objDTO.contratos;
                        objCE.esActivo = true;
                        db.tblCO_ADP_EvaluacionPlantilla.Add(objCE);
                        db.SaveChanges();
                    }
                    else
                    {
                        objCE = db.tblCO_ADP_EvaluacionPlantilla.Where(w => w.id == objDTO.idPlantilla).FirstOrDefault();
                        objCE.nombrePlantilla = objDTO.nombrePlantilla.Trim().ToUpper();
                        objCE.contratos = objDTO.contratos;
                        _context.SaveChanges();
                    }
                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, objDTO.idPlantilla <= 0 ? "Se ha registrado con éxito" : "Se ha actualizado con éxito.");
                    dbContextTransaction.Commit();
                    #endregion
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, NombreControlador, "addEditPlantilla", ex, objDTO.idPlantilla <= 0 ? AccionEnum.AGREGAR : AccionEnum.ACTUALIZAR, objDTO.idPlantilla, objDTO);
                    resultado.Add(MESSAGE, ex.Message);
                    resultado.Add(SUCCESS, false);
                }
                return resultado;
            }
        }

        public List<ComboDTO> cboObtenerContratos()
        {
            List<ComboDTO> lstCombo = new List<ComboDTO>();
            try
            {
                var lstPlantillasContrato = db.tblCO_ADP_EvaluacionPlantilla.ToList();
                List<int> lstTodosLosContratos = new List<int>();
                foreach (var item in lstPlantillasContrato)
                {
                    var lst = obtenerListas(item.contratos);
                    lstTodosLosContratos.AddRange(lst);
                }

                lstCombo = db.tblX_Contrato.Where(r => !lstTodosLosContratos.Contains(r.id) && r.estatus).ToList().Select(y => new ComboDTO
                {
                    Value = y.id.ToString(),
                    Text = y.numeroContrato
                }).ToList();

            }
            catch (Exception ex)
            {
                throw;
            }
            return lstCombo;

        }
        public List<ComboDTO> cboObtenerContratosInclu(int idPlantilla)
        {
            List<ComboDTO> lstCombo = new List<ComboDTO>();
            try
            {
                var lstPlantillasContrato = db.tblCO_ADP_EvaluacionPlantilla.Where(r => r.id != idPlantilla).ToList();
                List<int> lstTodosLosContratos = new List<int>();
                foreach (var item in lstPlantillasContrato)
                {
                    var lst = obtenerListas(item.contratos);
                    lstTodosLosContratos.AddRange(lst);
                }

                lstCombo = db.tblX_Contrato.Where(r => !lstTodosLosContratos.Contains(r.id) && r.estatus).ToList().Select(y => new ComboDTO
                {
                    Value = y.id.ToString(),
                    Text = y.numeroContrato
                }).ToList();

            }
            catch (Exception ex)
            {
                throw;
            }
            return lstCombo;
        }
        public List<int> obtenerListas(string rt)
        {
            List<int> devolverLst = new List<int>();
            if (rt != "")
            {
                var arr = rt.Split(',');
                if (arr.Count() > 0)
                {
                    foreach (var item in arr)
                    {
                        if (item != "")
                        {
                            devolverLst.Add(Convert.ToInt32(item));

                        }
                    }
                }
            }
            return devolverLst;
        }
        public DivicionesMenuDTO addEditDiviciones(DivicionesMenuDTO parametros)
        {
            DivicionesMenuDTO objDatos = new DivicionesMenuDTO();
            try
            {
                var obj = db.tblCO_ADP_EvaluacionDiv.Where(y => y.id == parametros.id).FirstOrDefault();
                if (obj == null)
                {
                    int id = 0;
                    int orden = 0;
                    var objid = (from i in db.tblCO_ADP_EvaluacionDiv orderby i.id descending select i).FirstOrDefault();
                    if (objid == null)
                    {
                        id = 1;
                        orden = 1;
                    }
                    else
                    {
                        id = 1 + db.tblCO_ADP_EvaluacionDiv.OrderByDescending(n => n.id).FirstOrDefault().id;
                        orden = 1 + db.tblCO_ADP_EvaluacionDiv.OrderByDescending(n => n.orden).FirstOrDefault().id;
                    }
                    obj = new tblCO_ADP_EvaluacionDiv();
                    obj.idPlantilla = parametros.idPlantilla;
                    obj.esActivo = true;
                    obj.descripcion = parametros.descripcion;
                    obj.idEvaluador = parametros.idEvaluador;
                    obj.toltips = parametros.toltips;
                    obj.idbutton = "btnEvaluacion" + id;
                    obj.idsection = "sectionDiv" + id;
                    obj.orden = parametros.orden;
                    obj.important = parametros.important;
                    db.tblCO_ADP_EvaluacionDiv.Add(obj);
                    db.SaveChanges();
                    var objUltimoid = (from i in db.tblCO_ADP_EvaluacionDiv orderby i.id descending select i).FirstOrDefault();

                    if (parametros.lstRequerimientos != null)
                    {
                        foreach (var item in parametros.lstRequerimientos)
                        {
                            tblCO_ADP_EvaluacionReq objReq = new tblCO_ADP_EvaluacionReq();
                            int id2 = 0;
                            var objid2 = (from i in db.tblCO_ADP_EvaluacionReq orderby i.id descending select i).FirstOrDefault();
                            if (objid2 == null)
                            {
                                id2 = 1;
                            }
                            else
                            {
                                id2 = 1 + db.tblCO_ADP_EvaluacionReq.OrderByDescending(n => n.id).FirstOrDefault().id;
                            }
                            objReq.idDiv = objUltimoid.id;
                            objReq.texto = item.texto;
                            objReq.inputFile = "inputFile" + id2;
                            objReq.lblInput = "lblInput" + id2;
                            objReq.txtAComentario = "txtArea" + id2;
                            objReq.txtPlaneacion = "txtPlaneacion" + id2;
                            objReq.txtResponsable = "txtResponsable" + id2;
                            objReq.txtFechaCompromiso = "txtFechaCompromiso" + id2;
                            objReq.preguntarEvaluacion = item.important;
                            db.tblCO_ADP_EvaluacionReq.Add(objReq);
                            db.SaveChanges();
                        }
                    }

                }
                else
                {
                    obj.idPlantilla = parametros.idPlantilla;
                    obj.idEvaluador = parametros.idEvaluador;
                    obj.descripcion = parametros.descripcion;
                    obj.toltips = parametros.toltips;
                    obj.orden = parametros.orden;
                    obj.important = parametros.important;
                    db.SaveChanges();
                    var lstReq = db.tblCO_ADP_EvaluacionReq.Where(r => r.idDiv == parametros.id).ToList();
                    if (lstReq.Count() != 0)
                    {
                        db.tblCO_ADP_EvaluacionReq.RemoveRange(lstReq);
                        db.SaveChanges();
                    }
                    if (parametros.lstRequerimientos != null)
                    {
                        foreach (var item in parametros.lstRequerimientos)
                        {
                            tblCO_ADP_EvaluacionReq objReq = new tblCO_ADP_EvaluacionReq();
                            int id2 = 0;
                            var objid2 = (from i in db.tblCO_ADP_EvaluacionReq orderby i.id descending select i).FirstOrDefault();
                            if (objid2 == null)
                            {
                                id2 = 1;
                            }
                            else
                            {
                                id2 = 1 + db.tblCO_ADP_EvaluacionReq.OrderByDescending(n => n.id).FirstOrDefault().id;
                            }
                            objReq.idDiv = parametros.id;
                            objReq.texto = item.texto;
                            objReq.inputFile = "inputFile" + id2;
                            objReq.lblInput = "lblInput" + id2;
                            objReq.txtAComentario = "txtArea" + id2;
                            objReq.txtPlaneacion = "txtPlaneacion" + id2;
                            objReq.txtResponsable = "txtResponsable" + id2;
                            objReq.txtFechaCompromiso = "txtFechaCompromiso" + id2;
                            objReq.preguntarEvaluacion = item.important;
                            db.tblCO_ADP_EvaluacionReq.Add(objReq);
                            db.SaveChanges();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return objDatos;
        }
        public DivicionesMenuDTO eliminarDiviciones(int id)
        {
            DivicionesMenuDTO objDatos = new DivicionesMenuDTO();
            try
            {
                var obj = db.tblCO_ADP_EvaluacionDiv.Where(y => y.id == id).FirstOrDefault();
                if (obj != null)
                {
                    db.tblCO_ADP_EvaluacionDiv.Remove(obj);
                    db.SaveChanges();
                    var lstDatos = _context.tblCO_ADP_EvaluacionReq.Where(r => r.idDiv == id).ToList();
                    if (lstDatos.Count() != 0)
                    {
                        db.tblCO_ADP_EvaluacionReq.RemoveRange(lstDatos);
                        db.SaveChanges();
                    }

                    objDatos.estatus = 1;
                    objDatos.mensaje = "Se ha eliminado con exito!";
                }
                else
                {
                    objDatos.estatus = 2;
                    objDatos.mensaje = "No se ha podido eliminar!";
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            return objDatos;
        }
        public List<RequerimientosDTO> obtenerRequerimientos(int idDiv)
        {
            List<RequerimientosDTO> objDatos = new List<RequerimientosDTO>();
            try
            {

                objDatos = db.tblCO_ADP_EvaluacionReq.Where(r => r.idDiv == idDiv).Select(y => new RequerimientosDTO
                {
                    id = y.id,
                    idDiv = y.idDiv,
                    texto = y.texto,
                    inputFile = y.inputFile,
                    lblInput = y.lblInput,
                    tipoFile = y.tipoFile,
                    txtAComentario = y.txtAComentario,
                    txtPlaneacion = y.txtPlaneacion,
                    txtResponsable = y.txtResponsable,
                    txtFechaCompromiso = y.txtFechaCompromiso,
                    important = y.preguntarEvaluacion,
                }).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
            return objDatos;
        }

        public Dictionary<string, object> obtenerDivicionesEvaluador()
        {
            #region v1 ADAN
            List<DivicionesMenuDTO> lstDatos = new List<DivicionesMenuDTO>();
            List<DivicionesMenuDTO> lstDatos2 = new List<DivicionesMenuDTO>();
            try
            {
                lstDatos = db.tblCO_ADP_EvaluacionDiv.Where(r => r.esActivo && r.SubContratista == true).ToList().Select(y => new DivicionesMenuDTO
                {
                    id = y.id,
                    idbutton = y.idbutton,
                    idsection = y.idsection,
                    toltips = y.toltips,
                    descripcion = y.descripcion,
                    esActivo = y.esActivo,
                    orden = y.orden,
                    important = y.important
                }).ToList();
                lstDatos2 = lstDatos.OrderBy(n => n.orden).ToList();

                resultado.Add(ITEMS, lstDatos2);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, ex.Message);
                LogError(0, 0, NombreControlador, "obtenerDivicionesEvaluador", ex, AccionEnum.CONSULTA, 0, 0);
            }
            return resultado;
            #endregion

            #region v2
            //            resultado = new Dictionary<string, object>();
            //            try
            //            {
            //                List<DivicionesMenuDTO> lstEvaluacionesDIV = _context.Select<DivicionesMenuDTO>(new DapperDTO
            //                {
            //                    baseDatos = MainContextEnum.Construplan,
            //                    consulta = @"SELECT id, idbutton, idsection, toltips, descripcion, esActivo, orden, important 
            //	                                    FROM tblCO_ADP_EvaluacionDiv
            //		                                    WHERE esActivo = @esActivo AND SubContratista = @SubContratista
            //			                                    ORDER BY orden",
            //                    parametros = new { esActivo = true, SubContratista = true }
            //                }).ToList();

            //                resultado.Add(ITEMS, lstEvaluacionesDIV);
            //                resultado.Add(SUCCESS, true);
            //            }
            //            catch (Exception e)
            //            {
            //                resultado.Add(SUCCESS, false);
            //                resultado.Add(MESSAGE, e.Message);
            //                LogError(0, 0, NombreControlador, "obtenerDivicionesEvaluador", e, AccionEnum.CONSULTA, 0, 0);
            //            }
            #endregion
        }

        public List<DivicionesMenuDTO> obtenerDivicionesEvaluadorArchivos(int idPlantilla, int idAsignacion)
        {
            List<DivicionesMenuDTO> lstDatos = new List<DivicionesMenuDTO>();
            List<DivicionesMenuDTO> lstDatos2 = new List<DivicionesMenuDTO>();
            List<int> lstElementosID = new List<int>();
            try
            {
                var lstIdDiviciones = db.tblCO_ADP_EvalSubContratista.Where(r => r.idSubConAsignacion == idAsignacion).Select(y => y.tipoEvaluacion).ToList();
                string cc = db.tblCO_ADP_EvalSubContratista.Where(r => r.idSubConAsignacion == idAsignacion).Select(y => y.idAreaCuenta).FirstOrDefault();

                // SE OBTIENE A LOS ELEMENTOS QUE PUEDE EVALUAR EL EVALUADOR LOGUEADO
                string strQuery = string.Format(@"SELECT elementos FROM tblCO_ADP_EvaluadorXcc WHERE evaluador = {0} AND cc LIKE '%{1}%' AND esActivo = {2}", (int)vSesiones.sesionUsuarioDTO.id, cc, 1);
                string stringElementos = _context.Select<string>(new DapperDTO
                {
                    baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                    consulta = strQuery
                }).FirstOrDefault();


                string[] arrElementos;
                if (!string.IsNullOrEmpty(stringElementos))
                {
                    arrElementos = stringElementos.Split(',');
                    lstElementosID = new List<int>();
                    foreach (var item in arrElementos)
                    {
                        if (item != ",")
                            lstElementosID.Add(Convert.ToInt32(item));
                    }
                }

                lstDatos = db.tblCO_ADP_EvaluacionDiv.Where(r => r.esActivo && r.idPlantilla == idPlantilla && r.SubContratista == false && lstIdDiviciones.Contains(r.id)).ToList().Select(y => new DivicionesMenuDTO
                {
                    id = y.id,
                    idbutton = y.idbutton,
                    idsection = y.idsection,
                    toltips = y.toltips,
                    descripcion = y.descripcion,
                    esActivo = y.esActivo,
                    orden = y.orden,
                    important = y.important
                }).ToList();
                lstDatos2 = lstDatos.OrderBy(n => n.orden).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }

            if (lstElementosID.Count() > 0)
                return lstDatos2.Where(w => lstElementosID.Contains(w.id)).ToList();
            else
                return lstDatos2.ToList();
        }
        public List<DivReqDTO> obtenerLst(int idPlantilla, int idAsignacion)
        {
            List<DivReqDTO> lstResultado = new List<DivReqDTO>();
            DivReqDTO objDivReq = new DivReqDTO();
            try
            {
                var lstDiviciones = db.tblCO_ADP_EvalSubContratista.Where(r => r.idSubConAsignacion == idAsignacion).ToList().Select(n => new
                {
                    id = n.id,
                    tipoEvaluacion = n.tipoEvaluacion,
                    descripcion = !string.IsNullOrEmpty(db.tblCO_ADP_EvaluacionDiv.Where(r => r.id == n.tipoEvaluacion).Select(s => s.descripcion).FirstOrDefault()) ? db.tblCO_ADP_EvaluacionDiv.Where(r => r.id == n.tipoEvaluacion).Select(s => s.descripcion).FirstOrDefault() : string.Empty,
                    toltips = !string.IsNullOrEmpty(db.tblCO_ADP_EvaluacionDiv.Where(r => r.id == n.tipoEvaluacion).Select(s => s.toltips).FirstOrDefault()) ? db.tblCO_ADP_EvaluacionDiv.Where(r => r.id == n.tipoEvaluacion).Select(s => s.toltips).FirstOrDefault() : string.Empty,
                }).ToList();
                foreach (var item in lstDiviciones)
                {
                    List<tblCO_ADP_EvaluacionReq> cantRequerimientos = db.tblCO_ADP_EvaluacionReq.Where(r => r.idDiv == item.tipoEvaluacion).ToList();
                    var cantRequerimientosRegistrados = lstResultado.Where(w => w.DivicionesORequerimiento == item.descripcion && w.Titulo != "").ToList();

                    if (cantRequerimientos.Count() != cantRequerimientosRegistrados.Count())
                    {
                        List<int> lstEvaluaciones = new List<int>();
                        lstEvaluaciones = db.tblCO_ADP_EvaluacionReq.Where(w => w.idDiv == item.tipoEvaluacion).Select(s => s.id).ToList();
                        var total = obtenerCalificacionPorDiv(idAsignacion, item.tipoEvaluacion, lstEvaluaciones);
                        var numero = Math.Round(total, 2);
                        objDivReq = new DivReqDTO();
                        objDivReq.DivicionesORequerimiento = item.descripcion;
                        objDivReq.TituloP = item.toltips;
                        objDivReq.Titulo = "";
                        if (total <= 25) { objDivReq.Pesimo = numero == 0 ? "" : numero.ToString(); } else { objDivReq.Pesimo = ""; }
                        if (total >= 25.01 && total <= 50) { objDivReq.Malo = numero == 0 ? "" : numero.ToString(); } else { objDivReq.Malo = ""; }
                        if (total >= 50.01 && total <= 70) { objDivReq.Regular = numero == 0 ? "" : numero.ToString(); } else { objDivReq.Regular = ""; }
                        if (total >= 70.01 && total <= 90) { objDivReq.Aceptable = numero == 0 ? "" : numero.ToString(); } else { objDivReq.Aceptable = ""; }
                        if (total >= 90.01 && total <= 100) { objDivReq.Excdediendo = numero == 0 ? "" : numero.ToString(); } else { objDivReq.Excdediendo = ""; }
                        objDivReq.Calificacion = numero == 0 ? "" : numero.ToString();
                        lstResultado.Add(objDivReq);

                        var lstReq = db.tblCO_ADP_EvaluacionReq.Where(r => r.idDiv == item.tipoEvaluacion).ToList();
                        foreach (var item2 in lstReq)
                        {
                            objDivReq = new DivReqDTO();
                            objDivReq.DivicionesORequerimiento = item.descripcion;
                            objDivReq.TituloP = item.toltips;
                            objDivReq.Titulo = item2.texto;
                            objDivReq.Pesimo = "";
                            objDivReq.Malo = "";
                            objDivReq.Regular = "";
                            objDivReq.Aceptable = "";
                            objDivReq.Excdediendo = "";
                            objDivReq.Calificacion = "";
                            lstResultado.Add(objDivReq);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return lstResultado.ToList();
        }
        public List<tblCOES_Firma> obtenerFirmas(int evaluacion_id)
        {
            List<tblCOES_Firma> resultado = new List<tblCOES_Firma>();

            var listaFirmas = db.tblCOES_Firma.Where(x => x.registroActivo && x.evaluacion_id == evaluacion_id && x.rutaArchivoFirma != null && x.rutaArchivoFirma != "").ToList();

            if (listaFirmas.Count() > 0)
            {
                resultado = listaFirmas;
            }

            return resultado;
        }
        public double obtenerCalificacionPorDiv(int idAsignacion, int idTipoDiv, List<int> lstEvaluaciones)
        {
            double total = 0;
            try
            {
                var lstDetalle = db.tblCO_ADP_EvalSubContratistaDet.Where(x => lstEvaluaciones.Contains(x.tipoEvaluacion) && x.idEvaluacion == idAsignacion).ToList();
                foreach (var item in lstDetalle)
                {
                    total += item.calificacion;
                }
                if (total == 0)
                {
                    total = 0;
                }
                else
                {
                    total = (total / lstDetalle.Count());
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return total;
        }
        public byte[] DescargarArchivos(long idDet, int idEvaluacion)
        {
            var resultado = new Dictionary<string, object>();
            Stream fileStream;
            try
            {
                string pathExamen = db.tblCO_ADP_EvalSubContratistaDet.Where(x => x.tipoEvaluacion == idDet && x.idEvaluacion == idEvaluacion).FirstOrDefault().rutaArchivo;
                fileStream = GlobalUtils.GetFileAsStream(pathExamen);

            }
            catch (Exception e)
            {
                fileStream = null;
            }

            //resultado.Add("nombreDescarga", version.nombre);
            //resultado.Add(SUCCESS, true);
            return ReadFully(fileStream);
        }
        public string getFileName(long idDet, int idEvaluacion)
        {
            string fileName = "";
            try
            {
                string pathExamen = db.tblCO_ADP_EvalSubContratistaDet.Where(x => x.tipoEvaluacion == idDet && x.idEvaluacion == idEvaluacion).FirstOrDefault().rutaArchivo;
                if (pathExamen != null)
                {
                    fileName = pathExamen.Split('\\').Last();
                }
            }
            catch (Exception e)
            {
                fileName = "";
            }

            return fileName;
        }
        public static byte[] ReadFully(Stream input)
        {
            MemoryStream ms = new MemoryStream();
            if (input != null)
            {
                byte[] buffer = new byte[16 * 1024];
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
            }
            return ms.ToArray();
        }
        public SubContratistasDTO obtenerEvaluacionxReq(SubContratistasDTO objDTO)
        {
            var obj = new SubContratistasDTO();
            try
            {
                // SE OBTIENE EL ID SUBCONTRATISTA
                string strQuery = string.Format("SELECT * FROM tblX_SubContratista WHERE nombre LIKE '%{0}%'", objDTO.nombreSubcontratista);
                int idSubcontratista = _context.Select<int>(new DapperDTO
                {
                    baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                    consulta = strQuery
                }).FirstOrDefault();

                int idEvaluacion = 0;
                if (idSubcontratista > 0)
                {
                    strQuery = string.Format("SELECT * FROM tblCO_ADP_EvalSubConAsignacion WHERE idSubContratista = {0} AND cc = '{1}' AND fechaInicial = '{2}' AND fechaFinal = '{3}'",
                                                idSubcontratista, objDTO.cc, objDTO.strFechaInicial, objDTO.strFechaFinal);
                    idEvaluacion = _context.Select<int>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                        consulta = strQuery
                    }).FirstOrDefault();
                }

                // SE OBTIENE el IDROW DE LA EVALUACIÓN
                strQuery = string.Format("SELECT texto FROM tblCO_ADP_EvaluacionReq WHERE id = {0}", objDTO.id);
                string strRow = _context.Select<string>(new DapperDTO
                {
                    baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                    consulta = strQuery
                }).FirstOrDefault();

                string idRow = strRow.Substring(0, 1);
                int row = Convert.ToInt32(idRow);

                //obj = db.tblCO_ADP_EvalSubContratistaDet.Where(r => r.id == objDTO.id).Select(y => new SubContratistasDTO
                //obj = db.tblCO_ADP_EvalSubContratistaDet.Where(r => r.id == idEvaluacion).Select(y => new SubContratistasDTO
                //obj = db.tblCO_ADP_EvalSubContratistaDet.Where(r => r.id == idEvaluacion && r.idRow == row).Select(y => new SubContratistasDTO
                //{
                //    id = y.id,
                //    Calificacion = y.calificacion,
                //    Comentario = y.comentario,
                //    fechaCompromiso = y.fechaCompromiso,
                //    planesDeAccion = y.planesDeAccion,
                //    responsable = y.responsable
                //}).FirstOrDefault();

                strQuery = string.Format("SELECT * FROM tblCO_ADP_EvalSubContratistaDet WHERE idEvaluacion = {0} AND idRow = {1} AND tipoEvaluacion = {2}", idEvaluacion, row, objDTO.id);
                obj = _context.Select<SubContratistasDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                    consulta = strQuery
                }).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
            return obj;
        }
        public SubContratistasDTO GuardarEvaluacion(SubContratistasDTO objDTO)
        {
            SubContratistasDTO objretu = new SubContratistasDTO();
            try
            {

                // SE OBTIENE EL ID SUBCONTRATISTA
                string strQuery = string.Format("SELECT * FROM tblX_SubContratista WHERE nombre LIKE '%{0}%'", objDTO.nombreSubcontratista);
                int idSubcontratista = _context.Select<int>(new DapperDTO
                {
                    baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                    consulta = strQuery
                }).FirstOrDefault();

                int idEvaluacion = 0;
                if (idSubcontratista > 0)
                {
                    strQuery = string.Format("SELECT * FROM tblCO_ADP_EvalSubConAsignacion WHERE idSubContratista = {0} AND cc = '{1}' AND fechaInicial = '{2}' AND fechaFinal = '{3}'",
                                                idSubcontratista, objDTO.cc, objDTO.strFechaInicial, objDTO.strFechaFinal);
                    idEvaluacion = _context.Select<int>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                        consulta = strQuery
                    }).FirstOrDefault();
                }

                // SE OBTIENE el IDROW DE LA EVALUACIÓN
                strQuery = string.Format("SELECT texto FROM tblCO_ADP_EvaluacionReq WHERE id = {0}", objDTO.id);
                string strRow = _context.Select<string>(new DapperDTO
                {
                    baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                    consulta = strQuery
                }).FirstOrDefault();

                string idRow = strRow.Substring(0, 1);
                int row = Convert.ToInt32(idRow);

                //var obj = db.tblCO_ADP_EvalSubContratistaDet.Where(r => r.id == objDTO.id).FirstOrDefault();
                //var obj = db.tblCO_ADP_EvalSubContratistaDet.Where(r => r.id == idEvaluacion).FirstOrDefault();
                var obj = db.tblCO_ADP_EvalSubContratistaDet.Where(w => w.idEvaluacion == idEvaluacion && w.idRow == row && w.tipoEvaluacion == objDTO.id).FirstOrDefault();
                if (obj != null)
                {
                    obj.calificacion = objDTO.Calificacion;
                    obj.comentario = objDTO.Comentario;
                    //obj.evaluacionPendiente = true;
                    db.SaveChanges();
                    objretu.mensaje = "Se ha calificado con exito";
                    objretu.status = 1;

                    #region SE VERIFICA SI YA SE EVALUO TODO LOS REQUERIMIENTOS DEL ELEMENTO

                    // SE OBTIENE LOS ELEMENTOS
                    strQuery = string.Format("SELECT idDiv FROM tblCO_ADP_EvaluacionReq WHERE id = {0}", obj.tipoEvaluacion);
                    List<int> lstDiv = _context.Select<int>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                        consulta = strQuery
                    }).ToList();

                    strQuery = string.Format("SELECT id FROM tblCO_ADP_EvaluacionReq WHERE idDiv IN ({0})", string.Join(",", lstDiv));
                    List<int> lstElementos = _context.Select<int>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                        consulta = strQuery
                    }).ToList();

                    // SE OBTIENES LAS CALIFICACIONES DE LOS ELEMENTOS
                    strQuery = string.Format("SELECT calificacion FROM tblCO_ADP_EvalSubContratistaDet WHERE idEvaluacion = {0} AND tipoEvaluacion IN ({1})", idEvaluacion, string.Join(",", lstElementos));
                    List<tblCO_ADP_EvalSubContratistaDet> lstCalificaciones = _context.Select<tblCO_ADP_EvalSubContratistaDet>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                        consulta = strQuery
                    }).ToList();

                    int cantEvaluaciones = lstElementos.Count();
                    int cantEvaluacionesRealizadas = lstCalificaciones.Where(w => w.calificacion > 0).Count();
                    int cantEvaluacionesNoRealizadas = lstCalificaciones.Where(w => w.calificacion <= 0).Count();

                    if (cantEvaluacionesNoRealizadas >= 1)
                    {
                        objretu.elementoVerde = false;
                    }
                    else if (cantEvaluacionesRealizadas == cantEvaluaciones)
                    {
                        objretu.elementoVerde = true;
                        objretu.idElemento = lstDiv[0];
                    }
                    #endregion
                }
            }
            catch (Exception)
            {
                objretu.mensaje = "algo a ocurrido mal comuniquese con el departamento de TI";
                objretu.status = 2;
            }
            return objretu;
        }
        public SubContratistasDTO obtenerPromegioEvaluacion(SubContratistasDTO objDTO)
        {
            try
            {
                #region VERSIÓN ADAN
                //SubContratistasDTO promedio = new SubContratistasDTO();
                //int calculo = 0;

                //var obj = db.tblCO_ADP_EvalSubConAsignacion.Where(r => r.cc == parametros.cc && r.idSubContratista == parametros.idSubContratista).FirstOrDefault();
                //string strQuery = string.Format(@"SELECT * FROM tblCO_ADP_EvalSubConAsignacion WHERE id = {0}", objDTO.id);
                //var obj = _context.Select<tblCO_ADP_EvalSubConAsignacion>(new DapperDTO
                //{
                //    baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                //    consulta = strQuery
                //}).FirstOrDefault();

                //if (obj != null)
                //{
                //    //var objInicio = db.tblCO_ADP_EvalSubContratista.Where(r => r.idSubConAsignacion == obj.id && r.tipoEvaluacion == objDTO.tipoEvaluacion).FirstOrDefault();

                //    //if (objInicio != null)
                //    //{
                //    strQuery = string.Format(@"SELECT * FROM tblCO_ADP_EvalSubContratistaDet WHERE calificacion > 0 AND tipoEvaluacion = {0} AND idEvaluacion = {1}", objDTO.tipoEvaluacionDet, objDTO.id);
                //    List<tblCO_ADP_EvalSubContratistaDet> lstCalificaciones = _context.Select<tblCO_ADP_EvalSubContratistaDet>(new DapperDTO
                //    {
                //        baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                //        consulta = strQuery
                //    }).ToList();

                //    //var lstDeCalificaciones = db.tblCO_ADP_EvalSubContratistaDet.Where(w => w.calificacion > 0 && w.tipoEvaluacion == objDTO.tipoEvaluacion && w.idEvaluacion == objDTO.idEvaluacion).ToList();

                //    var lstReq = db.tblCO_ADP_EvaluacionReq.Where(r => r.idDiv == objDTO.tipoEvaluacion).ToList();
                //    if (lstCalificaciones.Count() != 0)
                //    {
                //        foreach (var item in lstCalificaciones)
                //        {
                //            calculo += item.calificacion;
                //        }
                //        promedio.promedio = Convert.ToDouble(calculo / lstReq.Count());
                //    }
                //    else
                //    {
                //        promedio = new SubContratistasDTO();
                //        promedio.promedio = 0;
                //    }

                //    //}
                //    //else
                //    //{
                //    //    promedio = new SubContratistasDTO();
                //    //    promedio.promedio = 0;
                //    //}
                //}
                //return promedio;
                #endregion

                #region VERSIÓN v1
                SubContratistasDTO objPromedio = new SubContratistasDTO();

                #region PROMEDIO POR ELEMENTO
                // SE OBTIENE LOS ELEMENTOS
                string strQuery = string.Format("SELECT id FROM tblCO_ADP_EvaluacionReq WHERE idDiv = {0}", objDTO.tipoEvaluacion);
                List<tblCO_ADP_EvaluacionReq> lstElementos = _context.Select<tblCO_ADP_EvaluacionReq>(new DapperDTO
                {
                    baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                    consulta = strQuery
                }).ToList();
                decimal CantElementos = lstElementos.Count();
                if (lstElementos.Count() <= 0)
                    throw new Exception("Ocurrió un error al obtener los elementos para poder obtener el promedio.");

                List<int> lstTipoEvaluaciones = new List<int>();
                foreach (var item in lstElementos)
                {
                    lstTipoEvaluaciones.Add(item.id);
                }

                // SE OBTIENE LAS CALIFICACIONES
                strQuery = string.Format("SELECT SUM(calificacion) AS SumaCalificaciones FROM tblCO_ADP_EvalSubContratistaDet WHERE idEvaluacion = {0} AND tipoEvaluacion IN ({1})", objDTO.id, string.Join(",", lstTipoEvaluaciones));
                decimal SumaCalificaciones = _context.Select<decimal>(new DapperDTO
                {
                    baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                    consulta = strQuery
                }).FirstOrDefault();

                // SE OBTIENE EL PROMEDIO
                if (CantElementos > 0 && SumaCalificaciones > 0)
                    objPromedio.promedio = (double)SumaCalificaciones / (double)CantElementos;
                #endregion

                #region PROMEDIO GLOBAL
                // SE OBTIENE LISTADO DE TIPO DE EVALUACIONES
                strQuery = string.Format("SELECT calificacion, tipoEvaluacion FROM tblCO_ADP_EvalSubContratistaDet WHERE idEvaluacion = {0}", objDTO.id);
                List<tblCO_ADP_EvalSubContratistaDet> lstTipoEvaluaciones_Global = _context.Select<tblCO_ADP_EvalSubContratistaDet>(new DapperDTO
                {
                    baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                    consulta = strQuery
                }).ToList();
                if (lstTipoEvaluaciones_Global == null)
                    throw new Exception("Ocurrió un error al obtener todas las calificaciones de todos los elementos.");

                List<int> lstTipoEvaluacionesID_Global = new List<int>();
                foreach (var item in lstTipoEvaluaciones_Global)
                {
                    lstTipoEvaluacionesID_Global.Add(item.tipoEvaluacion);
                }

                strQuery = string.Format("SELECT COUNT(id) CantElementos FROM tblCO_ADP_EvaluacionReq WHERE id IN ({0})", string.Join(",", lstTipoEvaluacionesID_Global));
                int CantElementos_Global = _context.Select<int>(new DapperDTO
                {
                    baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                    consulta = strQuery
                }).FirstOrDefault();
                if (CantElementos_Global <= 0)
                    throw new Exception("Ocurrió un error al obtener la cantidad de elementos existentes.");

                // SE OBTIENE PROMEDIO GLOBAL
                decimal SumaCalificaciones_Global = lstTipoEvaluaciones_Global.Where(w => w.calificacion > 0).Select(w => w.calificacion).Sum(s => s);
                if (SumaCalificaciones_Global > 0 && CantElementos_Global > 0)
                    objPromedio.promedioGlobal = (double)SumaCalificaciones_Global / (double)CantElementos_Global;
                #endregion

                return objPromedio;
                #endregion
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Dictionary<string, object> ObtenerGraficaDeBarras(SubContratistasDTO parametros)
        {
            Dictionary<string, object> lstDatos = new Dictionary<string, object>();
            List<SubContratistasDTO> lstSubContratistas = new List<SubContratistasDTO>();
            var lstGrafica = db.tblCO_ADP_EvalSubConAsignacion.Where(r => (r.statusAutorizacion == 2 || r.statusAutorizacion == 3)).ToList();
            //var lstGrafica = _context.tblCO_ADP_EvalSubContratista.ToList().Select(y => y.idSubContratista).Distinct();
            //var lst = lstGrafica.Distinct();

            if (lstGrafica.Count() != 0)
            {
                List<tblPUsuarioDTO> lstUsuario = new List<tblPUsuarioDTO>();
                string sql = @"SELECT * FROM tblP_Usuario WHERE tipo=3 AND estatus='true'";
                using (var ctx = new SqlConnection(ConextSigoDapper.conexionSUBCONTRATISTAS()))
                {
                    ctx.Open();
                    lstUsuario = ctx.Query<tblPUsuarioDTO>(sql, null, null, true, 300).ToList();
                    ctx.Close();
                }
                lstSubContratistas = lstGrafica.Select(y => new SubContratistasDTO
                {
                    responsable = lstUsuario.Where(r => r.id == y.idSubContratista).Select(n => n.nombre_completo).FirstOrDefault(),
                    promedio = obtenerPromedio(y.id),
                    Indicador = IndicadorEvaluacion(obtenerPromedio(y.id)),
                }).ToList();
            }

            var lstResultado22 = lstSubContratistas.GroupBy(r => r.Indicador).ToList();
            List<object> lstResutlado = new List<object>();
            foreach (var item in lstResultado22)
            {
                if (item.Select(y => y.Indicador).FirstOrDefault() == (int)EstatusControlObraEnum.Pesimo)
                {
                    var obj = new
                    {
                        name = "PESIMO",
                        y = item.Count(),
                        color = "#FA0101",
                    };
                    lstResutlado.Add(obj);
                }
                if (item.Select(y => y.Indicador).FirstOrDefault() == (int)EstatusControlObraEnum.Malo)
                {
                    var obj = new
                    {
                        name = "MALO",
                        y = item.Count(),
                        color = "#FA8001",
                    };
                    lstResutlado.Add(obj);
                }
                if (item.Select(y => y.Indicador).FirstOrDefault() == (int)EstatusControlObraEnum.Regular)
                {
                    var obj = new
                    {
                        name = "REGULAR",
                        y = item.Count(),
                        color = "#FAFF01",
                    };
                    lstResutlado.Add(obj);
                }
                if (item.Select(y => y.Indicador).FirstOrDefault() == (int)EstatusControlObraEnum.Aceptable)
                {
                    var obj = new
                    {
                        name = "ACEPTABLE",
                        y = item.Count(),
                        color = "#018001",
                    };
                    lstResutlado.Add(obj);
                }
                if (item.Select(y => y.Indicador).FirstOrDefault() == (int)EstatusControlObraEnum.Excediendo)
                {
                    var obj = new
                    {
                        name = "EXCEDIENDO LAS EXPECTATIVAS",
                        y = item.Count(),
                        color = "#0180FF",
                    };
                    lstResutlado.Add(obj);
                }
            }



            var listado = new
            {
                name = "",
                colorByPoint = true,
                data = lstResutlado,
            };
            List<object> lstResult2 = new List<object>();
            lstResult2.Add(listado);

            lstDatos.Add("gpxGraficaDeBarras", lstResult2);

            return lstDatos;

        }
        public int IndicadorEvaluacion(double total)
        {
            int tipo = 0;
            if (total >= 0 && total <= 25)
            {
                tipo = (int)EstatusControlObraEnum.Pesimo;
            }
            if (total >= 26 && total <= 50)
            {
                tipo = (int)EstatusControlObraEnum.Malo;
            }
            if (total >= 51 && total <= 70)
            {
                tipo = (int)EstatusControlObraEnum.Regular;
            }
            if (total >= 71 && total <= 90)
            {
                tipo = (int)EstatusControlObraEnum.Aceptable;
            }
            if (total >= 91 && total <= 100)
            {
                tipo = (int)EstatusControlObraEnum.Excediendo;
            }
            return tipo;
        }
        public double obtenerPromedio(int idAsignacion)
        {
            //var total = lstDatos.Sum(r => r.Calificacion) / lstDatos.Count();

            var lstDatos = obtenerListado(idAsignacion);
            var total = lstDatos.Sum(r => r.Calificacion) / lstDatos.Count();
            return (double)total;

            //double stTotal = 0;
            //double promedio2 = 0;
            //double total = 0;
            //var Promedio = db.tblCO_ADP_EvalSubContratista.Where(r => r.idSubConAsignacion == idAsignacion).ToList();
            //var TotalEvaluaciones = db.tblCO_ADP_EvaluacionDiv.Where(r => r.SubContratista == false).ToList().Count();
            //if (Promedio.Count() != 0)
            //{
            //    foreach (var item in Promedio)
            //    {
            //        var TotalRequerimientos = db.tblCO_ADP_EvaluacionReq.Where(r => r.idDiv == item.tipoEvaluacion).ToList().Count();
            //        var lstObjetos = db.tblCO_ADP_EvalSubContratistaDet.Where(r => r.idEvaluacion == item.id && r.calificacion != 0).ToList();
            //        if (lstObjetos.Count() != 0)
            //        {
            //            foreach (var item2 in lstObjetos)
            //            {
            //                total += item2.calificacion;
            //            }
            //        }
            //        if (total == 0)
            //        {
            //            total = 0;
            //        }
            //        else
            //        {
            //            promedio2 += total / TotalRequerimientos > 0 ? TotalRequerimientos : 1;
            //            stTotal += promedio2;
            //        }
            //        promedio2 = 0;
            //        total = 0;
            //    }
            //    if (stTotal == 0)
            //    {
            //        stTotal = 0;
            //    }
            //    else
            //    {
            //        stTotal = stTotal / TotalEvaluaciones;
            //    }
            //}
            //return stTotal;
        }
        public List<SubContratistasDTO> obtenerPromediosxSubcontratista(SubContratistasDTO parametros)
        {
            List<SubContratistasDTO> lstSubContratistas = new List<SubContratistasDTO>();
            var lstGraficaAsig = db.tblCO_ADP_EvalSubConAsignacion.Where(r => (r.statusAutorizacion == 2 || r.statusAutorizacion == 3)).ToList();

            if (lstGraficaAsig.Count() != 0)
            {
                lstSubContratistas = lstGraficaAsig.Select(y => new SubContratistasDTO
                {
                    id = y.id,
                    idSubContratista = y.idSubContratista,
                    centroCostos = y.cc,
                    cc = y.cc + " " + _context.tblP_CC.Where(r => r.cc == y.cc).FirstOrDefault().descripcion,
                    responsable = db.tblX_SubContratista.Where(r => r.id == y.idSubContratista).Select(n => n.nombre).FirstOrDefault(),
                    promedio = obtenerPromedio(y.id),
                    Indicador = IndicadorEvaluacion(obtenerPromedio(y.id)),
                }).ToList();
            }
            return lstSubContratistas;
        }
        public List<ContratoSubContratistaDTO> obtenerTblSubContratista(string AreaCuenta)
        {
            List<ContratoSubContratistaDTO> lstContrato = new List<ContratoSubContratistaDTO>();
            if (AreaCuenta == null)
            {
                AreaCuenta = "";
            }
            var lstSubContratistas = db.tblX_SubContratista.Where(r => r.estatus).ToList();
            var lstContratos = db.tblX_Contrato.Where(r => r.estatus
                                                                                    && (AreaCuenta == "" ? r.cc == r.cc : r.cc == AreaCuenta)
                                                                                    ).ToList().Select(y => y.subcontratistaID).ToList();
            lstContrato = lstSubContratistas.Where(y => lstContratos.Contains(y.id)).Select(y => new ContratoSubContratistaDTO
            {
                id = y.id,
                nombre = y.nombre,
                direccion = y.direccion,
            }).ToList();



            return lstContrato;
        }
        public bool ObtenerEvaluacionPendiente(string idUsuario)
        {
            bool EvaluacionPendiente = false;
            try
            {
                var objSubContratistaID = db.tblX_SubContratista.Where(r => r.rfc == idUsuario).FirstOrDefault();
                int _idUsuario = objSubContratistaID.id;
                var obj = db.tblCO_ADP_EvalSubConAsignacion.Where(r => r.idSubContratista == _idUsuario && r.esActivo == true && r.statusAutorizacion == 2).FirstOrDefault();
                if (obj != null)
                {
                    EvaluacionPendiente = true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return EvaluacionPendiente;
        }
        public List<ContratoSubContratistaDTO> obtenerContratistasConContrato(string AreaCuenta, int subcontratista, int Estatus, int tipoUsuario, string _cc)
        {
            List<ContratoSubContratistaDTO> lstRetornar = new List<ContratoSubContratistaDTO>();
            List<ContratoSubContratistaDTO> lstRetornarFiltrandoPasado = new List<ContratoSubContratistaDTO>();

            //sigoplanGenericDAO _cont = new sigoplanGenericDAO();
            if (AreaCuenta == null)
                AreaCuenta = "";

            using (var _ctx = new contextSigoplan())
            {
                try
                {
                    if (Estatus == 0)
                    {
                        #region optimización
                        var lstEstatusNo2 = new List<int> { 2, 5 };

                        // SE OBTIENE A LOS CC QUE TIENE PERMISO EL USUARIO
                        bool todosLosCC = false;
                        string strQuery = string.Format(@"SELECT * FROM tblCO_ADP_Facultamientos WHERE idUsuario = {0} AND esActivo = {1}", (int)vSesiones.sesionUsuarioDTO.id, 1);
                        List<tblCO_ADP_Facultamientos> lstUsuarioFacultamientos = _context.Select<tblCO_ADP_Facultamientos>(new DapperDTO
                        {
                            baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                            consulta = strQuery
                        }).ToList();
                        List<string> lstUsuarioRelCC = lstUsuarioFacultamientos.Select(s => s.cc).ToList();
                        List<string> lstCC = new List<string>();
                        List<int> lstCC_i = new List<int>();

                        foreach (var item in lstUsuarioRelCC)
                        {
                            string[] SplitCC = item.Split(',');
                            foreach (var itemCC in SplitCC)
                            {
                                if (!string.IsNullOrEmpty(itemCC) && itemCC != ",")
                                {
                                    string cc = itemCC.Replace(",", "");
                                    lstCC.Add(string.Format("{0}", cc));
                                }
                            }
                        }

                        var lstAsignaciones2 = _ctx.tblCO_ADP_EvalSubConAsignacion.Where(x => lstEstatusNo2.Contains(x.statusAutorizacion) && x.esActivo).Select(x => x.id).ToList();

                        var lstContratos2 = _ctx.tblX_Contrato
                            .Where(x =>
                                ((!string.IsNullOrEmpty(_cc) ? x.cc == _cc : true)) &&
                                (subcontratista == 0 ? true : x.subcontratistaID == subcontratista) &&
                                x.estatus)
                            .Select(x => new ContratoSubContratistaDTO
                            {
                                numeroProveedor = x.subcontratista.numeroProveedor,
                                nombre = x.subcontratista.nombre,
                                direccion = x.subcontratista.direccion,
                                nombreCorto = x.subcontratista.nombreCorto,
                                codigoPostal = x.subcontratista.codigoPostal,
                                cc = x.cc,
                                idSubContratista = x.subcontratistaID,
                                tipoUsuario = tipoUsuario,
                                numeroContrato = x.numeroContrato,
                                idContrato = x.id,
                                emails = x.subcontratista.correo,
                                evaluaciones = x.evaluaciones.Select(y => new EvaluacionesDTO
                                {
                                    nombreEvaluacion = y.nombreEvaluacion,
                                    cc = y.cc,
                                    esActivo = y.esActivo,
                                    id = y.id,
                                    statusAutorizacion = y.statusAutorizacion,
                                    evaluacionAnteriorid = y.evaluacionAnteriorid,
                                    statusVobo = y.statusVobo,
                                    evaluacionesConAsignacion = y.evalSubContratista.Select(z => new EvaluacionConAsignacionDTO
                                    {
                                        id = z.id,
                                        evaluacionPendiente = z.evaluacionPendiente,
                                        evaluacionesPendiente = z.detalleEvaluaciones.Select(m => m.evaluacionPendiente).ToList()
                                    }).ToList()
                                }).ToList()
                            }).ToList();

                        var ccContratos = lstContratos2.Select(x => x.cc).Distinct().ToList();
                        var _ccs = _context.tblP_CC.Where(x => ccContratos.Contains(x.cc)).ToList();

                        var lstPlantillas = _ctx.tblCO_ADP_EvaluacionPlantilla.Where(x => !string.IsNullOrEmpty(x.contratos)).ToList();
                        var plantillaDefault = _ctx.tblCO_ADP_EvaluacionPlantilla.First(x => x.nombrePlantilla == "default").id;

                        foreach (var contrato in lstContratos2)
                        {
                            var cc = _ccs.FirstOrDefault(x => x.cc == contrato.cc);

                            var evaluacion = contrato.evaluaciones.FirstOrDefault(x => x.esActivo);
                            var asignacion = contrato.evaluaciones.Where(x => x.esActivo).OrderByDescending(x => x.id).FirstOrDefault();
                            var estatusAutorizacion = contrato.evaluaciones.Where(x => !x.esActivo).ToList();
                            var status = evaluacion != null ? 2 : estatusAutorizacion.Count != 0 ? 3 : 0;

                            var idPlantilla = 0;
                            foreach (var plantilla in lstPlantillas)
                            {
                                var contratosPlantilla = plantilla.contratos.Split(',');
                                if (contratosPlantilla.Count() > 0)
                                {
                                    var plantillaAsignada = contratosPlantilla.FirstOrDefault(x => !string.IsNullOrEmpty(x) && Convert.ToInt32(x) == contrato.idContrato);
                                    if (plantillaAsignada != null)
                                    {
                                        idPlantilla = plantilla.id;
                                        break;
                                    }
                                }
                            }
                            if (idPlantilla == 0)
                            {
                                idPlantilla = plantillaDefault;
                            }

                            int sta = 0;
                            if (evaluacion != null)
                            {
                                var objSubCon = evaluacion.evaluacionesConAsignacion;
                                if (objSubCon.Count != 0)
                                {
                                    foreach (var item in objSubCon)
                                    {
                                        if (item.evaluacionesPendiente.Any(x => x))
                                        {
                                            sta = 1;
                                        }
                                        else
                                        {
                                            sta = 0;
                                            break;
                                        }
                                    }
                                }
                            }

                            contrato.id = evaluacion != null ? evaluacion.id : 0;
                            contrato.promedio = Math.Round(obtenerPromedio(asignacion != null ? asignacion.id : 0), 2);
                            contrato.idAsignacion = asignacion != null ? asignacion.id : 0;
                            contrato.fechaPeriodo = obtenerPeriodoFechas(asignacion != null ? asignacion.id : 0);
                            contrato.descripcion = cc != null ? cc.descripcion : "";
                            contrato.descripcioncc = contrato.cc + (!string.IsNullOrEmpty(contrato.descripcion) ? " " + cc.descripcion : "");
                            contrato.estatusAutorizacion = estatusAutorizacion.Count > 0 ? estatusAutorizacion.First().statusAutorizacion : 0;
                            contrato.evaluacionAnteriorid = evaluacion != null ? evaluacion.evaluacionAnteriorid : 0;
                            contrato.evaluacionActual = sta;
                            contrato.existeEvaluacionAnterior = estatusAutorizacion.Count != 0 ? true : false;
                            contrato.existeEvaluacionPendiente = evaluacion == null ? false : true;
                            contrato.status = status;
                            contrato.statusVobo = evaluacion == null ? false : evaluacion.statusVobo;
                            contrato.idPlantilla = idPlantilla;
                            contrato.emails = contrato.emails;
                        }

                        //lstRetornar = lstContratos2.Where(x => !lstAsignaciones2.Contains(x.id)).ToList();
                        if (string.IsNullOrEmpty(_cc) && lstCC == null)
                            lstContratos2 = null;
                        else if (!todosLosCC)
                            lstRetornar = lstContratos2.Where(w => lstCC.Contains(w.cc) && !lstAsignaciones2.Contains(w.id)).ToList();
                        else
                            lstRetornar = lstContratos2.ToList();
                        #endregion
                    }
                    else if (Estatus == 2)
                    {
                        #region VERSIÓN v1
                        string strQuery = string.Format(@"SELECT t1.id, t1.id AS idAsignacion, t2.numeroContrato, t1.cc AS descripcioncc, t3.nombre, t1.fechaInicial, t1.fechaFinal, t1.statusAutorizacion AS estatusAutorizacion, 
                                                                 t3.correo AS emails, t1.idPlantilla, t1.nombreEvaluacion
	                                                                FROM tblCO_ADP_EvalSubConAsignacion AS t1
	                                                                INNER JOIN tblX_Contrato AS t2 ON t1.idContrato = t2.id
	                                                                INNER JOIN tblX_SubContratista AS t3 ON t1.idSubContratista = t1.idSubContratista
		                                                                WHERE t1.cc = '{0}' AND t3.id = {1} AND t1.idSubContratista = {1} AND t1.statusAutorizacion = 2", _cc, subcontratista);

                        List<ContratoSubContratistaDTO> lstEvaluaciones = _context.Select<ContratoSubContratistaDTO>(new DapperDTO
                        {
                            baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                            consulta = strQuery
                        }).ToList();

                        lstRetornar = lstEvaluaciones.ToList();

                        List<tblP_CC> lstCC = _context.Select<tblP_CC>(new DapperDTO
                        {
                            baseDatos = MainContextEnum.Construplan,
                            consulta = @"SELECT * FROM tblP_CC WHERE estatus = 1"
                        }).ToList();

                        foreach (var item in lstEvaluaciones)
                        {
                            tblP_CC objCC = lstCC.Where(w => w.cc == item.cc).FirstOrDefault();
                            if (objCC != null)
                            {
                                string cc = !string.IsNullOrEmpty(objCC.cc) ? objCC.cc.Trim().ToUpper() : string.Empty;
                                string descripcionCC = !string.IsNullOrEmpty(objCC.descripcion) ? objCC.descripcion.Trim().ToUpper() : string.Empty;
                                item.descripcioncc = string.Format("[{0}] {1}", cc, descripcionCC);
                            }
                        }
                        #endregion

                        #region VERSIÓN ADAN
                        #region optimización
                        //var lstContratos2 = _ctx.tblX_Contrato
                        //    .Where(x =>
                        //        (string.IsNullOrEmpty(AreaCuenta) ? true : x.cc == AreaCuenta) &&
                        //        (subcontratista == 0 ? true : x.subcontratistaID == subcontratista) &&
                        //        x.estatus)
                        //    .Select(x => new ContratoSubContratistaDTO
                        //    {
                        //        numeroProveedor = x.subcontratista.numeroProveedor,
                        //        nombre = x.subcontratista.nombre,
                        //        direccion = x.subcontratista.direccion,
                        //        nombreCorto = x.subcontratista.nombreCorto,
                        //        codigoPostal = x.subcontratista.codigoPostal,
                        //        cc = x.cc,
                        //        idSubContratista = x.subcontratistaID,
                        //        tipoUsuario = tipoUsuario,
                        //        numeroContrato = x.numeroContrato,
                        //        idContrato = x.id,
                        //        emails = x.subcontratista.correo,
                        //        evaluaciones = x.evaluaciones.Select(y => new EvaluacionesDTO
                        //        {
                        //            cc = y.cc,
                        //            esActivo = y.esActivo,
                        //            id = y.id,
                        //            statusAutorizacion = y.statusAutorizacion,
                        //            evaluacionAnteriorid = y.evaluacionAnteriorid,
                        //            statusVobo = y.statusVobo,
                        //            evaluacionesConAsignacion = y.evalSubContratista.Select(z => new EvaluacionConAsignacionDTO
                        //            {
                        //                id = z.id,
                        //                evaluacionPendiente = z.evaluacionPendiente,
                        //                evaluacionesPendiente = z.detalleEvaluaciones.Select(m => m.evaluacionPendiente).ToList()
                        //            }).ToList()
                        //        }).ToList()
                        //    }).ToList();

                        ////var ccContratos = lstContratos2.Select(x => x.cc).Distinct().ToList();
                        //var ccContratos = lstContratos2.Select(x => x.cc).ToList();
                        //var _ccs = _context.tblP_CC.Where(x => ccContratos.Contains(x.cc)).ToList();

                        //var lstPlantillas = _ctx.tblCO_ADP_EvaluacionPlantilla.Where(x => !string.IsNullOrEmpty(x.contratos)).ToList();
                        //var plantillaDefault = _ctx.tblCO_ADP_EvaluacionPlantilla.First(x => x.nombrePlantilla == "default").id;

                        //foreach (var contrato in lstContratos2)
                        //{
                        //    var cc = _ccs.FirstOrDefault(x => x.cc == contrato.cc);

                        //    var evaluacion = contrato.evaluaciones.FirstOrDefault(x => x.esActivo);
                        //    var asignacion = contrato.evaluaciones.Where(x => x.esActivo).OrderByDescending(x => x.id).FirstOrDefault();
                        //    var estatusAutorizacion = contrato.evaluaciones.Where(x => x.esActivo).ToList();
                        //    var status = evaluacion != null ? 2 : contrato.evaluaciones.Where(x => !x.esActivo).Count() != 0 ? 3 : 0;

                        //    var idPlantilla = 0;
                        //    foreach (var plantilla in lstPlantillas)
                        //    {
                        //        var contratosPlantilla = plantilla.contratos.Split(',');
                        //        if (contratosPlantilla.Count() > 0)
                        //        {
                        //            var plantillaAsignada = contratosPlantilla.FirstOrDefault(x => !string.IsNullOrEmpty(x) && Convert.ToInt32(x) == contrato.idContrato);
                        //            if (plantillaAsignada != null)
                        //            {
                        //                idPlantilla = plantilla.id;
                        //                break;
                        //            }
                        //        }
                        //    }
                        //    if (idPlantilla == 0)
                        //    {
                        //        idPlantilla = plantillaDefault;
                        //    }

                        //    int sta = 0;
                        //    if (evaluacion != null)
                        //    {
                        //        var objSubCon = evaluacion.evaluacionesConAsignacion;
                        //        if (objSubCon.Count != 0)
                        //        {
                        //            foreach (var item in objSubCon)
                        //            {
                        //                if (item.evaluacionesPendiente.Any(x => x))
                        //                {
                        //                    sta = 1;
                        //                }
                        //                else
                        //                {
                        //                    sta = 0;
                        //                    break;
                        //                }
                        //            }
                        //        }
                        //    }

                        //    contrato.id = evaluacion != null ? evaluacion.id : 0;
                        //    contrato.promedio = Math.Round(obtenerPromedio(asignacion != null ? asignacion.id : 0), 2);
                        //    contrato.emails = contrato.emails;
                        //    contrato.fechaPeriodo = obtenerPeriodoFechas(asignacion != null ? asignacion.id : 0);
                        //    contrato.idAsignacion = asignacion != null ? asignacion.id : 0;
                        //    contrato.descripcion = cc != null ? cc.descripcion : "";
                        //    contrato.descripcioncc = contrato.cc + (!string.IsNullOrEmpty(contrato.descripcion) ? " " + cc.descripcion : "");
                        //    contrato.estatusAutorizacion = estatusAutorizacion.Count > 0 ? estatusAutorizacion.First().statusAutorizacion : 0;
                        //    contrato.evaluacionAnteriorid = evaluacion != null ? evaluacion.evaluacionAnteriorid : 0;
                        //    contrato.evaluacionActual = sta;
                        //    contrato.existeEvaluacionAnterior = estatusAutorizacion.Count != 0 ? true : false;
                        //    contrato.existeEvaluacionPendiente = evaluacion == null ? false : true;
                        //    contrato.status = status;
                        //    contrato.statusVobo = evaluacion == null ? false : evaluacion.statusVobo;
                        //    contrato.idPlantilla = idPlantilla;
                        //}

                        //lstRetornar = lstContratos2;
                        #endregion
                        #endregion
                    }
                    else if (Estatus == 4)
                    {
                        #region SE OBTIENE EVALUACIONES YA FINALIZADAS
                        string strQuery = string.Format(@"SELECT t1.id, t1.id AS idAsignacion, t2.numeroContrato, t1.cc AS descripcioncc, t3.nombre, t1.fechaInicial, t1.fechaFinal, t1.statusAutorizacion AS estatusAutorizacion, 
                                                                 t3.correo AS emails, t1.idPlantilla, t1.nombreEvaluacion
	                                                                FROM tblCO_ADP_EvalSubConAsignacion AS t1
	                                                                INNER JOIN tblX_Contrato AS t2 ON t1.idContrato = t2.id
	                                                                INNER JOIN tblX_SubContratista AS t3 ON t1.idSubContratista = t1.idSubContratista
		                                                                WHERE t1.cc = '{0}' AND t3.id = {1} AND t1.idSubContratista = {1} AND t1.statusAutorizacion = 2", _cc, subcontratista);

                        List<ContratoSubContratistaDTO> lstEvaluaciones = _context.Select<ContratoSubContratistaDTO>(new DapperDTO
                        {
                            baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                            consulta = strQuery
                        }).ToList();

                        lstRetornar = lstEvaluaciones.ToList();

                        List<tblP_CC> lstCC = _context.Select<tblP_CC>(new DapperDTO
                        {
                            baseDatos = MainContextEnum.Construplan,
                            consulta = @"SELECT * FROM tblP_CC WHERE estatus = 1"
                        }).ToList();

                        foreach (var item in lstEvaluaciones)
                        {
                            tblP_CC objCC = lstCC.Where(w => w.cc == item.cc).FirstOrDefault();
                            if (objCC != null)
                            {
                                string cc = !string.IsNullOrEmpty(objCC.cc) ? objCC.cc.Trim().ToUpper() : string.Empty;
                                string descripcionCC = !string.IsNullOrEmpty(objCC.descripcion) ? objCC.descripcion.Trim().ToUpper() : string.Empty;
                                item.descripcioncc = string.Format("[{0}] {1}", cc, descripcionCC);
                            }
                        }
                        #endregion
                    }
                    else if (Estatus == 3)
                    {
                        #region SE OBTIENE LAS EVALUACIONES QUE CUENTA CON TODAS LAS FIRMAS.
                        string strQuery = string.Format(@"SELECT t1.id, t1.id AS idAsignacion, t2.numeroContrato, t1.cc AS descripcioncc, t3.nombre, t1.fechaInicial, t1.fechaFinal, t1.statusAutorizacion AS estatusAutorizacion, 
                                                                 t3.correo AS emails, t1.idPlantilla, t1.nombreEvaluacion
	                                                                FROM tblCO_ADP_EvalSubConAsignacion AS t1
	                                                                INNER JOIN tblX_Contrato AS t2 ON t1.idContrato = t2.id
	                                                                INNER JOIN tblX_SubContratista AS t3 ON t1.idSubContratista = t1.idSubContratista
		                                                                WHERE t1.cc = '{0}' AND t3.id = {1} AND t1.idSubContratista = {1} AND t1.statusAutorizacion = 3", _cc, subcontratista);

                        List<ContratoSubContratistaDTO> lstEvaluaciones = _context.Select<ContratoSubContratistaDTO>(new DapperDTO
                        {
                            baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                            consulta = strQuery
                        }).ToList();

                        lstRetornar = lstEvaluaciones.ToList();

                        List<tblP_CC> lstCC = _context.Select<tblP_CC>(new DapperDTO
                        {
                            baseDatos = MainContextEnum.Construplan,
                            consulta = @"SELECT * FROM tblP_CC WHERE estatus = 1"
                        }).ToList();

                        foreach (var item in lstEvaluaciones)
                        {
                            tblP_CC objCC = lstCC.Where(w => w.cc == item.cc).FirstOrDefault();
                            if (objCC != null)
                            {
                                string cc = !string.IsNullOrEmpty(objCC.cc) ? objCC.cc.Trim().ToUpper() : string.Empty;
                                string descripcionCC = !string.IsNullOrEmpty(objCC.descripcion) ? objCC.descripcion.Trim().ToUpper() : string.Empty;
                                item.descripcioncc = string.Format("[{0}] {1}", cc, descripcionCC);
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        #region optimización
                        lstRetornar = _ctx.tblCO_ADP_EvalSubConAsignacion
                            .Where(x =>
                                (string.IsNullOrEmpty(AreaCuenta) ? true : x.cc == AreaCuenta) &&
                                (subcontratista == 0 ? true : x.idSubContratista == subcontratista) &&
                                x.statusAutorizacion == Estatus)
                            .Select(x => new ContratoSubContratistaDTO
                            {
                                id = x.id,
                                numeroProveedor = x.subcontratista.numeroProveedor,
                                nombre = x.subcontratista.nombre,
                                direccion = x.subcontratista.direccion,
                                nombreCorto = x.subcontratista.nombreCorto,
                                codigoPostal = x.subcontratista.codigoPostal,
                                idAsignacion = x.id,
                                cc = x.cc,
                                fechaCreacion = (DateTime)x.fechaCreacion,
                                estatusAutorizacion = x.statusAutorizacion,
                                idSubContratista = x.idSubContratista,
                                emails = x.subcontratista.correo,
                                status = 3,
                                numeroContrato = "",
                                tipoUsuario = tipoUsuario,
                                statusVobo = x.statusVobo,
                                idContrato = x.idContrato
                            }).ToList();

                        foreach (var item in lstRetornar)
                        {
                            item.numeroContrato = db.tblX_Contrato.Where(w => w.id == item.idContrato).Select(s => s.numeroContrato).FirstOrDefault();
                        }

                        var ccContratos = lstRetornar.Select(x => x.cc).Distinct().ToList();
                        var _ccs = _context.tblP_CC.Where(x => ccContratos.Contains(x.cc)).ToList();

                        var lstPlantillas = _ctx.tblCO_ADP_EvaluacionPlantilla.Where(x => !string.IsNullOrEmpty(x.contratos)).ToList();
                        var plantillaDefault = _ctx.tblCO_ADP_EvaluacionPlantilla.First(x => x.nombrePlantilla == "default").id;

                        foreach (var asignacion in lstRetornar)
                        {
                            var cc = _ccs.FirstOrDefault(x => x.cc == asignacion.cc);

                            var evaluaciones = _ctx.tblCO_ADP_EvalSubConAsignacion.Where(x => x.idContrato == asignacion.idContrato).ToList();
                            var evaluacionAnterior = evaluaciones.FirstOrDefault(x => x.esActivo);

                            int sta = 0;
                            foreach (var item in evaluaciones.Where(x => x.esActivo))
                            {
                                if (item.evalSubContratista.Any(x => x.evaluacionPendiente))
                                {
                                    sta = 1;
                                }
                                else
                                {
                                    sta = 0;
                                }
                            }

                            var idPlantilla = 0;
                            foreach (var plantilla in lstPlantillas)
                            {
                                var contratosPlantilla = plantilla.contratos.Split(',');
                                if (contratosPlantilla.Count() > 0)
                                {
                                    var plantillaAsignada = contratosPlantilla.FirstOrDefault(x => !string.IsNullOrEmpty(x) && Convert.ToInt32(x) == asignacion.idContrato);
                                    if (plantillaAsignada != null)
                                    {
                                        idPlantilla = plantilla.id;
                                        break;
                                    }
                                }
                            }
                            if (idPlantilla == 0)
                            {
                                idPlantilla = plantillaDefault;
                            }

                            asignacion.emails = asignacion.emails;
                            asignacion.descripcion = cc != null ? cc.descripcion : "";
                            asignacion.promedio = Math.Round(obtenerPromedio(asignacion != null ? asignacion.id : 0), 2);
                            asignacion.descripcioncc = asignacion.cc + (!string.IsNullOrEmpty(asignacion.descripcion) ? " " + cc.descripcion : "");
                            asignacion.evaluacionAnteriorid = evaluacionAnterior != null ? evaluacionAnterior.evaluacionAnteriorid : 0;
                            asignacion.evaluacionActual = sta;
                            asignacion.fechaPeriodo = obtenerPeriodoFechas(asignacion != null ? asignacion.id : 0);
                            asignacion.existeEvaluacionAnterior = evaluaciones.Where(x => x.esActivo).Count() != 0 ? true : false;
                            asignacion.existeEvaluacionPendiente = evaluacionAnterior == null ? false : true;
                            asignacion.idPlantilla = idPlantilla;
                        }
                        #endregion
                    }

                    lstRetornarFiltrandoPasado = lstRetornar.Where(r => r.estatusAutorizacion == Estatus).ToList();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            if (Estatus == 2)
            {
                foreach (var item in lstRetornarFiltrandoPasado)
                {
                    #region SE VERIFICA SI LA PRIMERA EVALUACIÓN SE LLEVO ACABO.

                    // FECHA EVALUABLE PRIMER EVALUACIÓN
                    DateTime fechaInicialPrimerEvaluacion = lstRetornar.OrderBy(o => o.fechaInicial).Select(s => s.fechaInicial).FirstOrDefault();
                    DateTime fechaFinalPrimerEvaluacion = lstRetornar.OrderBy(o => o.fechaFinal).Select(s => s.fechaFinal).FirstOrDefault();

                    // FECHA EVALUABLE EVALUACIÓN
                    DateTime fechaInicialEvaluable = item.fechaInicial;
                    DateTime fechaFinalEvaluable = item.fechaFinal;

                    if (fechaInicialEvaluable <= fechaInicialPrimerEvaluacion)
                        item.evaluacionActiva = true;
                    else if (fechaInicialEvaluable == fechaInicialPrimerEvaluacion)
                        item.evaluacionActiva = true;
                    else
                        item.evaluacionActiva = false;
                    #endregion
                }
            }

            int tipoUsuarioID = GetTipoFacultamientosUsuarioLogueado();
            foreach (var item in lstRetornarFiltrandoPasado)
            {
                item.tipoUsuarioID = tipoUsuarioID;
            }

            return lstRetornarFiltrandoPasado;
        }
        public string obtenerPeriodoFechas(int idAsignacion)
        {
            string r = "";
            try
            {
                var objAsignacion = traermeDatosPrincipales(idAsignacion);
                var lstReqDivicion = obtenerLst(objAsignacion.idPlantilla, idAsignacion);
                var lstAsignacion = obtenerTodasLasASinaciones(idAsignacion);

                DateTime FechaInicialPeriodo = new DateTime();
                DateTime FechaInicialFinal = new DateTime();
                DateTime fechaActual = DateTime.Now;
                var sumaDeDias = (objAsignacion.numFreq * lstAsignacion.Count());
                var sumaDeDiasInicial = ((lstAsignacion.Count() - 1) * objAsignacion.numFreq);

                FechaInicialPeriodo = objAsignacion.fechaInicial.AddDays(sumaDeDiasInicial);
                FechaInicialFinal = objAsignacion.fechaFinal.AddDays(sumaDeDias);

                r = FechaInicialPeriodo.ToShortDateString() + " - " + FechaInicialFinal.ToShortDateString();
                //r = string.Format("{0} - {1}", objAsignacion.fechaInicial.ToShortDateString(), objAsignacion.fechaFinal.ToShortDateString();
            }
            catch (Exception)
            {
                return r = "";
            }
            return r;
        }
        public int retornarEvaluacionActual(int id)
        {
            int sta = 0;
            var objSubCon = db.tblCO_ADP_EvalSubContratista.Where(r => r.idSubConAsignacion == id).ToList();
            if (objSubCon.Count() != 0)
            {
                foreach (var item in objSubCon)
                {
                    var objdet = db.tblCO_ADP_EvalSubContratistaDet.Where(r => r.idEvaluacion == item.id).FirstOrDefault();
                    if (objdet != null)
                    {
                        if (objdet.evaluacionPendiente == true)
                        {
                            sta = 1;
                        }
                        else
                        {
                            return sta = 0;
                        }
                    }
                }
            }

            return sta;
        }
        //public SubContratistasDTO GuardarEvaluacionSubContratista(SubContratistasDTO parametros)
        //{
        //    SubContratistasDTO objretu = new SubContratistasDTO();
        //    try
        //    {
        //        var obj = db.tblCO_ADP_EvalSubContratistaDet.Where(r => r.id == parametros.id).FirstOrDefault();
        //        if (obj != null)
        //        {
        //            obj.planesDeAccion = parametros.planesDeAccion;
        //            obj.responsable = parametros.responsable;
        //            obj.fechaCompromiso = parametros.fechaCompromiso;
        //            obj.evaluacionPendiente = true;
        //            db.SaveChanges();
        //            objretu.mensaje = "Se ha calificado con exito";
        //            objretu.status = 1;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        objretu.mensaje = "Ocurrio algun error comuniquese con el departamento de TI.";
        //        objretu.status = 1;
        //    }
        //    return objretu;
        //}
        public SubContratistasDTO GuardarAsignacion(SubContratistasDTO parametros)
        {
            SubContratistasDTO objretu = new SubContratistasDTO();
            try
            {
                tblCO_ADP_EvalSubConAsignacion objAsignacion = db.tblCO_ADP_EvalSubConAsignacion.Where(r => r.id == parametros.idAsignacion).FirstOrDefault();
                if (objAsignacion != null)
                {
                    objAsignacion.statusAutorizacion = 2;
                    objAsignacion.statusVobo = true;
                    db.SaveChanges();
                    tblCO_ADP_EvaluacionRel objRel;
                    foreach (var item in parametros.lstRelacion)
                    {
                        objRel = new tblCO_ADP_EvaluacionRel();
                        objRel.idSubContratista = item.idSubContratista;
                        objRel.idReq = item.idReq;
                        objRel.Preguntar = item.Preguntar;
                        objRel.idAsignacion = objAsignacion.id;
                        db.tblCO_ADP_EvaluacionRel.Add(objRel);
                        db.SaveChanges();

                        var objDiv = db.tblCO_ADP_EvaluacionReq.Where(r => r.id == item.idReq).FirstOrDefault().idDiv;
                        var lstRequeriemientos = db.tblCO_ADP_EvaluacionReq.Where(r => r.idDiv == objDiv).ToList();
                        var Contratista = db.tblCO_ADP_EvalSubContratista.Where(r => r.idSubConAsignacion == objAsignacion.id && r.tipoEvaluacion == objDiv).FirstOrDefault();
                        var lstDetalle = db.tblCO_ADP_EvalSubContratistaDet.Where(r => r.tipoEvaluacion == objDiv && r.idEvaluacion == Contratista.id).ToList();
                        int idRowReq = 0;

                        foreach (var item3 in lstRequeriemientos)
                        {
                            idRowReq++;
                            var objReq = db.tblCO_ADP_EvaluacionReq.Where(r => r.id == item3.id).FirstOrDefault();
                            if (objReq.id == item.idReq)
                                break;
                        }

                        var objDetalle = db.tblCO_ADP_EvalSubContratistaDet.Where(r => r.tipoEvaluacion == objDiv && r.idEvaluacion == Contratista.id && r.idRow == idRowReq).FirstOrDefault();
                        if (objDetalle != null)
                        {
                            objDetalle.opcional = item.Preguntar;
                            db.SaveChanges();
                        }
                    }
                }
                else
                {
                    if (parametros.cantEvaluaciones <= 0)
                        throw new Exception("Es necesario indicar la cantidad de evaluaciones.");

                    for (int i = 0; i < parametros.cantEvaluaciones; i++)
                    {
                        int contador = i + 1;
                        objAsignacion = new tblCO_ADP_EvalSubConAsignacion();
                        objAsignacion.cc = parametros.cc;
                        objAsignacion.idSubContratista = parametros.idSubContratista;
                        objAsignacion.statusAutorizacion = 0;
                        objAsignacion.idPadre = 0;
                        objAsignacion.evaluacionActual = 0;
                        objAsignacion.fechaCreacion = DateTime.Now;
                        objAsignacion.evaluacionAnteriorid = 0;
                        objAsignacion.idContrato = parametros.idContrato;
                        objAsignacion.idPlantilla = parametros.idPlantilla;
                        objAsignacion.nombreEvaluacion = string.Format("{0} - {1}", parametros.nombreEvaluacion, contador);
                        objAsignacion.servicioContratado = parametros.inpServicioContratado;
                        objAsignacion.esActivo = true;

                        DateTime fechaInicial = new DateTime(2000, 01, 01);
                        DateTime fechaFinal = new DateTime(2000, 01, 01);
                        //objAsignacion.fechaInicial = contador == 1 ? (DateTime)parametros.fechaInicial : (DateTime)parametros.fechaInicial.Value.AddDays(10); //EN CALENDARIO SE DEFINEN
                        //objAsignacion.fechaFinal = contador == 1 ? (DateTime)parametros.fechaFinal : (DateTime)parametros.fechaFinal.Value.AddDays(10); //EN CALENDARIO SE DEFINEN
                        objAsignacion.fechaInicial = fechaInicial; //EN CALENDARIO SE DEFINEN
                        objAsignacion.fechaFinal = fechaFinal; //EN CALENDARIO SE DEFINEN
                        objAsignacion.fechaInicialEjecutable = (DateTime)parametros.fechaInicial;
                        objAsignacion.fechaFinalEjecutable = (DateTime)parametros.fechaFinal;

                        //objAsignacion.numFreq = (int)parametros.freqEval > 0 ? (int)parametros.freqEval : 0;
                        objAsignacion.idEstado = parametros.idEstado;
                        objAsignacion.idMunicipio = parametros.idMunicipio;
                        objAsignacion.cantEvaluaciones = parametros.cantEvaluaciones;
                        objAsignacion.numFreq = 0;
                        objAsignacion.statusVobo = true;
                        db.tblCO_ADP_EvalSubConAsignacion.Add(objAsignacion);
                        db.SaveChanges();
                        objAsignacion = db.tblCO_ADP_EvalSubConAsignacion.OrderByDescending(r => r.id).FirstOrDefault();
                        tblCO_ADP_EvaluacionRel objRel;

                        int idSubContratista = db.tblCO_ADP_EvalSubConAsignacion.OrderByDescending(r => r.id).FirstOrDefault().id;

                        tblCO_ADP_EvalSubContratista objSubContratista = new tblCO_ADP_EvalSubContratista();
                        tblCO_ADP_EvalSubContratistaDet objSubContratistaDet = new tblCO_ADP_EvalSubContratistaDet();
                        var lstEvaluaciones = db.tblCO_ADP_EvaluacionDiv.Where(r => r.SubContratista == false && r.idPlantilla == parametros.idPlantilla).ToList();
                        foreach (var item in lstEvaluaciones)
                        {
                            objSubContratista = new tblCO_ADP_EvalSubContratista();
                            objSubContratista.idSubConAsignacion = idSubContratista;
                            objSubContratista.idAreaCuenta = parametros.cc;
                            objSubContratista.idSubContratista = parametros.idSubContratista;
                            objSubContratista.tipoEvaluacion = item.id;
                            objSubContratista.evaluacionPendiente = true;
                            db.tblCO_ADP_EvalSubContratista.Add(objSubContratista);
                            db.SaveChanges();

                            int idRow = 0;
                            var lstRequerimiento = db.tblCO_ADP_EvaluacionReq.Where(r => r.idDiv == item.id).ToList();
                            int idEvalu = db.tblCO_ADP_EvalSubContratista.OrderByDescending(r => r.id).FirstOrDefault().id;
                            foreach (var item2 in lstRequerimiento)
                            {
                                idRow++;
                                objSubContratistaDet = new tblCO_ADP_EvalSubContratistaDet();
                                objSubContratistaDet.tipoEvaluacion = item2.id;
                                objSubContratistaDet.idRow = idRow;
                                //objSubContratistaDet.idEvaluacion = idEvalu;
                                objSubContratistaDet.idEvaluacion = objAsignacion.id;
                                objSubContratistaDet.fechaDocumento = DateTime.Now;
                                db.tblCO_ADP_EvalSubContratistaDet.Add(objSubContratistaDet);
                                db.SaveChanges();
                            }
                            idRow = 0;
                        }
                    }

                    #region COMENTADO
                    //foreach (var item in parametros.lstRelacion)
                    //{
                    //    objRel = new tblCO_ADP_EvaluacionRel();
                    //    objRel.idSubContratista = item.idSubContratista;
                    //    objRel.idReq = item.idReq;
                    //    objRel.Preguntar = item.Preguntar;
                    //    objRel.idAsignacion = objAsignacion.id;
                    //    db.tblCO_ADP_EvaluacionRel.Add(objRel);
                    //    db.SaveChanges();

                    //    var objDiv = db.tblCO_ADP_EvaluacionReq.Where(r => r.id == item.idReq).FirstOrDefault().idDiv;
                    //    var lstRequeriemientos = db.tblCO_ADP_EvaluacionReq.Where(r => r.idDiv == objDiv).ToList();

                    //    var Contratista = db.tblCO_ADP_EvalSubContratista.Where(r => r.idSubConAsignacion == objAsignacion.id && r.tipoEvaluacion == objDiv).FirstOrDefault();
                    //    var lstDetalle = db.tblCO_ADP_EvalSubContratistaDet.Where(r => r.tipoEvaluacion == objDiv && r.idEvaluacion == Contratista.id).ToList();
                    //    int idRowReq = 0;
                    //    foreach (var item3 in lstRequeriemientos)
                    //    {
                    //        idRowReq++;
                    //        var objReq = db.tblCO_ADP_EvaluacionReq.Where(r => r.id == item3.id).FirstOrDefault();
                    //        if (objReq.id == item.idReq)
                    //        {
                    //            break;
                    //        }
                    //    }

                    //    var objDetalle = db.tblCO_ADP_EvalSubContratistaDet.Where(r => r.tipoEvaluacion == objDiv && r.idEvaluacion == Contratista.id && r.idRow == idRowReq).FirstOrDefault();
                    //    if (objDetalle != null)
                    //    {
                    //        objDetalle.opcional = item.Preguntar;
                    //        db.SaveChanges();
                    //    }
                    //}
                    #endregion

                    var objSubContra = db.tblX_SubContratista.Where(r => r.id == parametros.idSubContratista).FirstOrDefault();
                    if (objSubContra != null)
                    {
                        List<string> emails = new List<string>();
                        List<tblPUsuarioDTO> lstUsuario = new List<tblPUsuarioDTO>();
                        string sql = @"SELECT * FROM tblP_Usuario WHERE tipo = 3 AND estatus = 'true'";
                        using (var ctx = new SqlConnection(ConextSigoDapper.conexionSUBCONTRATISTAS()))
                        {
                            ctx.Open();
                            lstUsuario = ctx.Query<tblPUsuarioDTO>(sql, null, null, true, 300).ToList();
                            ctx.Close();
                        }

                        List<tblPUsuarioDTO> lstUsurio2 = new List<tblPUsuarioDTO>();
                        string sql2 = @"SELECT * FROM tblP_Usuario WHERE tipo = 14 AND estatus = 'true'";
                        using (var ctx = new SqlConnection(ConextSigoDapper.conexionSUBCONTRATISTAS()))
                        {
                            ctx.Open();
                            lstUsurio2 = ctx.Query<tblPUsuarioDTO>(sql2, null, null, true, 300).ToList();
                            ctx.Close();
                        }

                        var objUsuario = lstUsuario.Where(r => r._user == objSubContra.rfc).FirstOrDefault();
                        var lstUsusariosPrestadores = lstUsurio2.Where(r => r.idPadre == objUsuario.id).ToList();
                        foreach (var item in lstUsusariosPrestadores)
                        {
                            emails.Add(item.correo);
                        }
                        GuardarAlerta(10, objUsuario.id, objAsignacion.id);
                        emails.Add(objUsuario.correo);
                        string Subject = "El nombre de la evaluacion : " + objAsignacion.nombreEvaluacion + ".";
                        string msg2 = CuerpoCorreo(objAsignacion, objUsuario);
                        List<FirmantesDTO> lstFirmantes = new List<FirmantesDTO>();
                        string msg = GlobalUtils.cuerpoDeCorreoFormato("Prestadores de servicio", "Evaluaciones", "Asginacion de la evaluacion para prestadores de servicio", msg2, "Gracias por su atencion.", "http://expediente.construplan.com.mx/", lstFirmantes);
#if DEBUG
                        emails = new List<string>();
                        emails.Add("omar.nunez@construplan.com.mx");
#else
                        emails = new List<string>();
                        emails.Add("maricela.ortiz@construplan.com.mx");
                        emails.Add("omar.nunez@construplan.com.mx");
#endif
                        GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), Subject), msg, emails);
                    }
                }
            }
            catch (Exception ex)
            {
                objretu.mensaje = "Ocurrio algun error comuniquese con el departamento de TI.";
                objretu.status = 1;
            }
            return objretu;
        }
        public bool GuardarAlerta(int userEnvia, int userRecibe, int id)
        {
            tblP_Alerta objAlerta = new tblP_Alerta();
            objAlerta.userEnviaID = userEnvia;
            objAlerta.userRecibeID = userRecibe;
            objAlerta.msj = "Tienes una evaluacion pendiente";
            objAlerta.visto = false;
            objAlerta.tipoAlerta = 2;
            objAlerta.sistemaID = 2;
            objAlerta.objID = id;
            objAlerta.url = "/ControlObra/ControlObra/DashboardSubContratista";
            db.tblP_Alerta.Add(objAlerta);
            db.SaveChanges();
            return true;
        }
        public string CuerpoCorreo(tblCO_ADP_EvalSubConAsignacion objAsignacion, tblPUsuarioDTO objUsuario)
        {
            string textoContrato = objAsignacion.contrato == null ? "No cuentas con numero de contrato" : objAsignacion.contrato.numeroContrato;
            string textoProyecto = objAsignacion.contrato == null ? "No cuentas con un proyecto asignado" : objAsignacion.contrato.proyecto.nombre;
            string a = "Tiene una evaluacion pendiente por realizar <br>"
                                      + " La evaluacion con el nombre : " + objAsignacion.nombreEvaluacion + "<br>"
                                      + " El numero del contrato " + textoContrato + "<br>"
                                      + " El nombre del proyecto asignado " + textoProyecto + "<br>"
                                      + " Usuario : " + objUsuario._user + "<br>"
                                      + " Password : " + objUsuario._pass + "<br>"
                                      + " Gracias por su atencion";
            return a;
        }
        public List<tblPUsuarioDTO> obtenerUsuariosTipo10()
        {
            List<tblPUsuarioDTO> obj = new List<tblPUsuarioDTO>();
            string sql = @"SELECT * FROM tblP_Usuario WHERE tipo=10 AND estatus='true'";
            using (var ctx = new SqlConnection(ConextSigoDapper.conexionSUBCONTRATISTAS()))
            {
                ctx.Open();
                obj = ctx.Query<tblPUsuarioDTO>(sql, null, null, true, 300).ToList();
                ctx.Close();
            }
            return obj;
        }
        public List<SubContratistasDTO> getUsuariosAutorizantes(string term)
        {
            List<SubContratistasDTO> lstUsuarios = new List<SubContratistasDTO>();
            try
            {
                string sql = @"SELECT * FROM tblP_Usuario WHERE tipo=2 AND estatus='true'";
                using (var ctx = new SqlConnection(ConextSigoDapper.conexionSUBCONTRATISTAS()))
                {
                    ctx.Open();
                    lstUsuarios = ctx.Query<SubContratistasDTO>(sql, null, null, true, 300).ToList().Select(y => new SubContratistasDTO
                    {
                        id = y.id,
                        nombre = y.nombre_completo
                    }).ToList();
                    ctx.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return lstUsuarios;
        }
        //        public SubContratistasDTO AutorizarEvaluacion(SubContratistasDTO parametros, int idUsuario)
        //        {
        //            SubContratistasDTO objReturn = new SubContratistasDTO();
        //            try
        //            {
        //                var obj = db.tblCO_ADP_EvalSubConAsignacion.Where(r => r.id == parametros.id).FirstOrDefault();
        //                if (obj != null)
        //                {
        //                    //obj.firmaAutorizacion = GlobalUtils.CrearFirmaDigital(obj.id, DocumentosEnum.AutorizacionEvaluacion, idUsuario);
        //                    obj.FechaAutorizacion = DateTime.Now;
        //                    obj.esActivo = false;
        //                    obj.statusAutorizacion = 3;
        //                    db.SaveChanges();

        //                    List<string> emails = new List<string>();
        //                    tblPUsuarioDTO lstUsurio3 = new tblPUsuarioDTO();
        //                    List<tblPUsuarioDTO> lstUsurio2 = new List<tblPUsuarioDTO>();
        //                    string sql2 = @"SELECT * FROM tblP_Usuario WHERE tipo=14 AND estatus='true'";
        //                    using (var ctx = new SqlConnection(ConextSigoDapper.conexionSUBCONTRATISTAS()))
        //                    {
        //                        ctx.Open();
        //                        lstUsurio2 = ctx.Query<tblPUsuarioDTO>(sql2, null, null, true, 300).ToList();
        //                        ctx.Close();
        //                    }
        //                    var objUsuarioSubContratista = db.tblX_SubContratista.FirstOrDefault(r => r.id == obj.idSubContratista);
        //                    if (objUsuarioSubContratista != null)
        //                    {
        //                        string sql3 = @"SELECT * FROM tblP_Usuario WHERE tipo=3 AND estatus='true' AND _user='" + objUsuarioSubContratista.rfc + "'";
        //                        using (var ctx = new SqlConnection(ConextSigoDapper.conexionSUBCONTRATISTAS()))
        //                        {
        //                            ctx.Open();
        //                            lstUsurio3 = ctx.Query<tblPUsuarioDTO>(sql3, null, null, true, 300).FirstOrDefault();
        //                            ctx.Close();
        //                        }
        //                        var lstUsusariosPrestadores = lstUsurio2.Where(r => r.idPadre == lstUsurio3.id).ToList();

        //                        foreach (var item in lstUsusariosPrestadores)
        //                        {
        //                            emails.Add(item.correo);
        //                        }
        //                    }
        //                    string Subject = "El nombre de la evaluacion : " + obj.nombreEvaluacion + ".";
        //                    string msg2 = CuerpoCorreoPMO(obj);
        //                    //string msg2 = CuerpoCorreo(objAsignacion, objUsuario);
        //                    List<FirmantesDTO> lstFirmantes = new List<FirmantesDTO>();
        //                    FirmantesDTO objFirmas = new FirmantesDTO();
        //                    objFirmas.nombreCompleto = lstUsurio3.nombre_completo;
        //                    objFirmas.Fecha = db.tblX_FirmaEvaluacionDetalle.Where(r => r.firmanteId == lstUsurio3.id).FirstOrDefault() == null ? "" : db.tblX_FirmaEvaluacionDetalle.Where(r => r.firmanteId == lstUsurio3.id).FirstOrDefault().fechaCreacion.ToShortDateString();
        //                    objFirmas.puesto = "SUBCONTRATISTA";
        //                    objFirmas.estado = false;
        //                    objFirmas.Firma = db.tblX_FirmaEvaluacionDetalle.Where(r => r.firmanteId == lstUsurio3.id).FirstOrDefault() == null ? "" : db.tblX_FirmaEvaluacionDetalle.Where(r => r.firmanteId == lstUsurio3.id).FirstOrDefault().firmaEvaluacion.ToString();
        //                    lstFirmantes.Add(objFirmas);
        //                    objFirmas = new FirmantesDTO();
        //                    var objUsuarioFirmantes = db.tblX_Firmante.Where(r => r.cc == obj.cc).FirstOrDefault();
        //                    if (objUsuarioFirmantes != null)
        //                    {
        //                        var objUsuarioSigo = _context.tblP_Usuario.Where(r => r.id == objUsuarioFirmantes.usuarioId).FirstOrDefault();
        //                        if (objUsuarioSigo != null)
        //                        {
        //                            objFirmas.nombreCompleto = objUsuarioSigo.nombre + " " + objUsuarioSigo.apellidoPaterno + " " + objUsuarioSigo.apellidoMaterno;
        //                            objFirmas.Fecha = db.tblX_FirmaEvaluacionDetalle.Where(r => r.firmanteId == objUsuarioSigo.id).FirstOrDefault() == null ? "" : db.tblX_FirmaEvaluacionDetalle.Where(r => r.firmanteId == objUsuarioSigo.id).FirstOrDefault().fechaCreacion.ToShortDateString();
        //                            objFirmas.puesto = "ADMINISTRADOR PMO";
        //                            objFirmas.estado = false;
        //                            objFirmas.Firma = db.tblX_FirmaEvaluacionDetalle.Where(r => r.firmanteId == objUsuarioSigo.id).FirstOrDefault() == null ? "" : db.tblX_FirmaEvaluacionDetalle.Where(r => r.firmanteId == objUsuarioSigo.id).FirstOrDefault().firmaEvaluacion.ToString();
        //                            lstFirmantes.Add(objFirmas);
        //                        }
        //                    }

        //                    string msg = GlobalUtils.cuerpoDeCorreoFormato("Prestadores de servicio", "Evaluaciones", "Correo para la gestion de firmas.", msg2, "Gracias por su atencion.", "http://expediente.construplan.com.mx/", lstFirmantes);

        //#if DEBUG
        //                    emails = new List<string>();
        //                    emails.Add("omar.nunez@construplan.com.mx");
        //#else
        //                    emails = new List<string>();
        //                    emails.Add("maricela.ortiz@construplan.com.mx");
        //                    emails.Add("omar.nunez@construplan.com.mx");
        //#endif
        //                    GlobalUtils.sendEmail(Subject, msg, emails);
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                throw;
        //            }

        //            return objReturn;
        //        }
        public string CuerpoCorreoPMO(tblCO_ADP_EvalSubConAsignacion objAsignacion)
        {
            string textoContrato = objAsignacion.contrato == null ? "No cuentas con numero de contrato" : objAsignacion.contrato.numeroContrato;
            string textoProyecto = objAsignacion.contrato == null ? "No cuentas con un proyecto asignado" : objAsignacion.contrato.proyecto.nombre;
            string a = "Se Autorizo una evaluacion para gestion de firmas <br>"
                            + " La evaluacion con el nombre : " + objAsignacion.nombreEvaluacion + "<br>"
                            + " El numero del contrato " + textoContrato + "<br>"
                            + " El nombre del proyecto asignado " + textoProyecto + "<br>"
                            + " Gracias por su atencion";
            return a;
        }
        //public SubContratistasDTO AutorizarAsignacion(SubContratistasDTO objDTO, int idUsuario)
        public Dictionary<string, object> AutorizarAsignacion(SubContratistasDTO objDTO, int idUsuario)
        {
            //SubContratistasDTO objReturn = new SubContratistasDTO();
            resultado = new Dictionary<string, object>();
            try
            {
                #region SE VERIFICA SI CUENTA CON ALGUN FIRMANTE ACTIVO, EN BASE AL SUBCONTRATISTA Y CONTRATO
                // SE OBTIENE ID_SUBCONTRATISTA Y ID_CONTRATO.
                string strQuery = string.Format("SELECT idSubContratista, idContrato FROM tblCO_ADP_EvalSubConAsignacion WHERE id = {0}", objDTO.id);
                tblCO_ADP_EvalSubConAsignacion objEvaluacion = _context.Select<tblCO_ADP_EvalSubConAsignacion>(new DapperDTO
                {
                    baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                    consulta = strQuery
                }).FirstOrDefault();
                if (objEvaluacion == null)
                    throw new Exception("Ocurrió un error al generar la asignación de la evaluación.");

                // SE VERIFICA EN BASE AL ID_SUBCONTRATISTA Y ID_CONTRATO, QUE CONTENGA ALGUN FIRMANTE ACTIVO.
                strQuery = string.Format("SELECT correo FROM tblCO_ADP_UsuariosFirmantesRelSubcontratistas WHERE idUsuarioSubcontratista = {0} AND idContrato LIKE '%{1}%' AND registroActivo = {2}",
                    objEvaluacion.idSubContratista, objEvaluacion.idContrato, 1);
                tblCO_ADP_UsuariosFirmantesRelSubcontratistas objFirmante = _context.Select<tblCO_ADP_UsuariosFirmantesRelSubcontratistas>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Construplan,
                    consulta = strQuery
                }).FirstOrDefault();
                if (objFirmante == null)
                    throw new Exception("Es necesario indicar un firmante al Subcontratista y Contrato seleccionado.");
                #endregion

                // SE OBTIENE EL CC
                string cc = db.tblCO_ADP_EvalSubConAsignacion.Where(w => w.id == objDTO.id).Select(s => s.cc).FirstOrDefault();
                List<int> lstEvalConAsignacionID = db.tblCO_ADP_EvalSubConAsignacion.Where(w => w.cc == cc).Select(s => s.id).ToList();

                List<tblCO_ADP_EvalSubConAsignacion> lstActualizar = db.tblCO_ADP_EvalSubConAsignacion.Where(r => lstEvalConAsignacionID.Contains(r.id)).ToList();
                if (lstActualizar.Count() > 0)
                {
                    for (int i = 0; i < lstActualizar.Count(); i++)
                    {
                        lstActualizar[i].esActivo = true;
                        lstActualizar[i].statusAutorizacion = 2;
                        lstActualizar[i].statusVobo = false;
                        db.SaveChanges();
                    }
                }

                // CORREO GENERAL DEL SUBCONTRATISTA.
                List<string> lstCorreos = new List<string>();
                lstCorreos.Add(objDTO.correo);
                // END

                // SE OBTIENE CORREO DEL FIRMANTE DEL SUBCONTRATISTA Y CONTRATO ASIGNADO.
                lstCorreos = new List<string>();
                lstCorreos.Add(objFirmante.correo);
#if DEBUG
                lstCorreos = new List<string>();
                lstCorreos.Add("omar.nunez@construplan.com.mx");
#else
                lstCorreos = new List<string>();
                lstCorreos.Add("maricela.ortiz@construplan.com.mx");
                lstCorreos.Add("omar.nunez@construplan.com.mx");
#endif
                List<FirmantesDTO> lstFirmantes = new List<FirmantesDTO>();
                string msg = GlobalUtils.cuerpoDeCorreoFormato("Prestadores de servicio", "Evaluaciones asignadas", "", "Se notifica que se ha asignado evaluaciones", "Gracias por su atención.", "http://expediente.construplan.com.mx/", lstFirmantes);
                GlobalUtils.sendEmail(string.Format("{0}: Se ha asignado evaluaciones", PersonalUtilities.GetNombreEmpresa()), msg, lstCorreos);

                resultado.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                LogError(0, 0, NombreControlador, "AutorizarAsignacion", ex, AccionEnum.AGREGAR, objDTO.id, objDTO);
                resultado.Add(MESSAGE, ex.Message);
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }
        public List<ElementosDTO> obtenerTodosLosElementosConSuRequerimiento()
        {
            List<ElementosDTO> lstRequerimiento = new List<ElementosDTO>();
            try
            {
                var lstReq = db.tblCO_ADP_EvaluacionReq.Where(n => n.preguntarEvaluacion == true).ToList();
                if (lstReq.Count() != 0)
                {
                    lstRequerimiento = db.tblCO_ADP_EvaluacionDiv.Where(r => r.SubContratista == false).ToList().Select(y => new ElementosDTO
                    {
                        id = y.id,
                        idbutton = y.idbutton,
                        idsection = y.idsection,
                        orden = y.orden,
                        toltips = y.toltips,
                        descripcion = y.descripcion,
                        esActivo = y.esActivo,
                        SubContratista = y.SubContratista,
                        important = y.important,
                        idEvaluador = y.idEvaluador,
                        Aparece = db.tblCO_ADP_EvaluacionReq.Where(n => n.idDiv == y.id && n.preguntarEvaluacion == true).ToList().Count() >= 1 ? true : false,
                        lstRequerimientos = db.tblCO_ADP_EvaluacionReq.Where(n => n.idDiv == y.id && n.preguntarEvaluacion == true).ToList(),
                    }).ToList(); ;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return lstRequerimiento;
        }
        public ElementosDTO guardarRelacion(List<ElementosDTO> lstRelacion)
        {
            ElementosDTO obj = new ElementosDTO();
            try
            {
                tblCO_ADP_EvaluacionRel objRel;
                foreach (var item in lstRelacion)
                {
                    objRel = new tblCO_ADP_EvaluacionRel();
                    objRel.idSubContratista = item.idSubContratista;
                    objRel.idReq = item.idReq;
                    objRel.Preguntar = item.Preguntar;
                    db.tblCO_ADP_EvaluacionRel.Add(objRel);
                    db.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return obj;
        }
        public List<ContratoSubContratistaDTO> tblObtenerDashBoardSubContratista(string RFC)
        {
            #region CODIGO ANTERIOR
            //List<ContratoSubContratistaDTO> lstRetornar = new List<ContratoSubContratistaDTO>();
            //var lstContratos = db.tblX_Contrato.Where(r => r.estatus == true && r.subcontratistaID == objUsuario).Select(x => x.subcontratistaID).Distinct().ToList();
            #endregion

            List<ContratoSubContratistaDTO> lstRetornarFiltrandoPasado = new List<ContratoSubContratistaDTO>();
            try
            {
                var objUsuario = db.tblX_SubContratista.Where(r => r.rfc == RFC).FirstOrDefault().id;

                lstRetornarFiltrandoPasado = db.tblX_Contrato
                    .Where(x =>
                        x.subcontratista.rfc == RFC &&
                        x.estatus)
                    .Select(x => new ContratoSubContratistaDTO
                    {
                        id = x.subcontratistaID,
                        numeroProveedor = x.subcontratista.numeroProveedor,
                        nombre = x.subcontratista.nombre,
                        direccion = x.subcontratista.direccion,
                        nombreCorto = x.subcontratista.nombreCorto,
                        codigoPostal = x.subcontratista.codigoPostal,
                        cc = x.cc,
                        idSubContratista = x.subcontratistaID,
                        idContrato = x.id,
                        evaluaciones = x.subcontratista.evaluaciones.Select(y => new EvaluacionesDTO
                        {
                            cc = y.cc,
                            esActivo = y.esActivo,
                            id = y.id,
                            statusAutorizacion = y.statusAutorizacion
                        }).ToList()
                    }).ToList();

                var ccContratos = lstRetornarFiltrandoPasado.Select(x => x.cc).ToList();
                var ccs = _context.tblP_CC.Where(x => ccContratos.Contains(x.cc)).ToList();

                foreach (var item in lstRetornarFiltrandoPasado)
                {
                    var idAsignacion = item.evaluaciones.Where(x => x.cc == item.cc && x.esActivo).OrderByDescending(x => x.id).FirstOrDefault();
                    var estatusAutorizacion = item.evaluaciones.Where(x => x.cc == item.cc && x.esActivo).FirstOrDefault();
                    var estatus = item.evaluaciones.Where(x => x.esActivo).Count() != 0 ? 2 : item.evaluaciones.Where(x => !x.esActivo).Count() != 0 ? 3 : 0;

                    item.descripcion = ccs.First(x => x.cc == item.cc).descripcion;
                    item.descripcioncc = item.cc + " " + item.descripcion;
                    item.idAsignacion = idAsignacion != null ? idAsignacion.id : 0;
                    item.estatusAutorizacion = estatusAutorizacion != null ? estatusAutorizacion.statusAutorizacion : 0;
                    item.existeEvaluacionAnterior = item.evaluaciones.Any(x => !x.esActivo);
                    item.existeEvaluacionPendiente = item.evaluaciones.Any(x => x.esActivo);
                    item.status = estatus;
                    item.evaluaciones = null;
                    item.idPlantilla = obtenerElIdDeLaPlantilla(item.idContrato);
                }

                lstRetornarFiltrandoPasado.RemoveAll(x => x.status != 2 || x.idAsignacion == 0);

                #region CODIGO ANTERIOR
                //var lstSubContratista = db.tblX_Contrato.Where(r => r.estatus == true).ToList().Select(y => new ContratoSubContratistaDTO
                //{
                //    id = db.tblX_SubContratista.Where(r => r.id == y.subcontratistaID).FirstOrDefault() == null ? 0 : db.tblX_SubContratista.Where(r => r.id == y.subcontratistaID).FirstOrDefault().id,
                //    numeroProveedor = db.tblX_SubContratista.Where(r => r.id == y.subcontratistaID).FirstOrDefault() == null ? 0 : db.tblX_SubContratista.Where(r => r.id == y.subcontratistaID).FirstOrDefault().numeroProveedor,
                //    nombre = db.tblX_SubContratista.Where(r => r.id == y.subcontratistaID).FirstOrDefault() == null ? "" : db.tblX_SubContratista.Where(r => r.id == y.subcontratistaID).FirstOrDefault().nombre,
                //    direccion = db.tblX_SubContratista.Where(r => r.id == y.subcontratistaID).FirstOrDefault() == null ? "" : db.tblX_SubContratista.Where(r => r.id == y.subcontratistaID).FirstOrDefault().direccion,
                //    nombreCorto = db.tblX_SubContratista.Where(r => r.id == y.subcontratistaID).FirstOrDefault() == null ? "" : db.tblX_SubContratista.Where(r => r.id == y.subcontratistaID).FirstOrDefault().nombreCorto,
                //    codigoPostal = db.tblX_SubContratista.Where(r => r.id == y.subcontratistaID).FirstOrDefault() == null ? "" : db.tblX_SubContratista.Where(r => r.id == y.subcontratistaID).FirstOrDefault().codigoPostal,
                //    cc = y.cc,
                //    descripcion = _cont.dbSigoplan.tblP_CC.Where(r => r.cc == y.cc).FirstOrDefault() == null ? "" : _cont.dbSigoplan.tblP_CC.Where(r => r.cc == y.cc).FirstOrDefault().descripcion

                //}).ToList();


                //lstRetornar = lstSubContratista.Where(y => lstContratos.Contains(y.id)).ToList().Select(y => new ContratoSubContratistaDTO
                //{
                //    id = y.id,
                //    numeroProveedor = y.numeroProveedor,
                //    nombre = y.nombre,
                //    direccion = y.direccion,
                //    nombreCorto = y.nombreCorto,
                //    codigoPostal = y.codigoPostal,
                //    idAsignacion = db.tblCO_ADP_EvalSubConAsignacion.Where(n => n.idSubContratista == y.id && n.cc == y.cc && n.esActivo).FirstOrDefault() == null ? 0 : db.tblCO_ADP_EvalSubConAsignacion.OrderByDescending(f => f.id).Where(n => n.idSubContratista == y.id && n.cc == y.cc && n.esActivo).FirstOrDefault().id,
                //    cc = y.cc,
                //    descripcioncc = y.cc + " " + y.descripcion,
                //    estatusAutorizacion = db.tblCO_ADP_EvalSubConAsignacion.Where(n => n.idSubContratista == y.id && n.esActivo && n.cc == y.cc).FirstOrDefault() == null ? 0 : db.tblCO_ADP_EvalSubConAsignacion.Where(n => n.idSubContratista == y.id && n.esActivo && n.cc == y.cc).FirstOrDefault().statusAutorizacion,
                //    idSubContratista = y.id,
                //    existeEvaluacionAnterior = db.tblCO_ADP_EvalSubConAsignacion.Where(n => n.idSubContratista == y.id && n.esActivo == false).ToList().Count() != 0 ? true : false,
                //    existeEvaluacionPendiente = db.tblCO_ADP_EvalSubConAsignacion.Where(n => n.idSubContratista == y.id && n.esActivo == true).FirstOrDefault() == null ? false : true,
                //    status = db.tblCO_ADP_EvalSubConAsignacion.Where(n => n.idSubContratista == y.id && n.esActivo == true).ToList().Count() != 0 ? 2 : db.tblCO_ADP_EvalSubConAsignacion.Where(n => n.idSubContratista == y.id && n.esActivo == false).ToList().Count() != 0 ? 3 : 0
                //}).ToList();

                //lstRetornarFiltrandoPasado = lstRetornar.Where(r => r.status == 2 && r.idAsignacion != 0).ToList();
                #endregion
            }
            catch (Exception ex)
            {
                throw;
            }
            return lstRetornarFiltrandoPasado;
        }
        public int obtenerElIdDeLaPlantilla(int idContrato)
        {
            try
            {
                List<DivicionesMenuDTO> lstTodosLosContratos = new List<DivicionesMenuDTO>();
                var lstPlantillas = db.tblCO_ADP_EvaluacionPlantilla.ToList();
                foreach (var item in lstPlantillas)
                {
                    var lst = lstPlantillasPor(item.id, item.contratos);
                    lstTodosLosContratos.AddRange(lst);
                }

                return lstTodosLosContratos.Where(r => r.id == idContrato).FirstOrDefault().idPlantilla;

            }
            catch (Exception)
            {
                return db.tblCO_ADP_EvaluacionPlantilla.Where(r => r.nombrePlantilla == "default").FirstOrDefault().id;
            }
        }
        public List<DivicionesMenuDTO> lstPlantillasPor(int idPlantilla, string lstPlan)
        {
            List<DivicionesMenuDTO> lst = new List<DivicionesMenuDTO>();
            DivicionesMenuDTO obj = new DivicionesMenuDTO();
            if (lstPlan != "")
            {
                var arr = lstPlan.Split(',');
                if (arr.Count() > 0)
                {
                    foreach (var item in arr)
                    {
                        if (item != "")
                        {
                            obj = new DivicionesMenuDTO();
                            var a = Convert.ToInt32(item);
                            //var objContrato = db.tblX_Contrato.Where(r => r.id == a).FirstOrDefault();
                            //obj.id = objContrato.id;
                            obj.id = a;
                            obj.idPlantilla = idPlantilla;
                            lst.Add(obj);
                        }
                    }
                }
            }
            return lst;
        }
        public MemoryStream realizarExcel(int idAsignacion)
        {
            var bytes = new MemoryStream();
            try
            {
                if (idAsignacion != 0)
                {
                    using (ExcelPackage excel = new ExcelPackage())
                    {
                        Color colorDeNaranja = ColorTranslator.FromHtml("#fabf8f");
                        Color colorDeCelda = ColorTranslator.FromHtml("#fff");
                        Color ColorNegro = ColorTranslator.FromHtml("#000");

                        var hoja1 = excel.Workbook.Worksheets.Add("EvaluacionSubContratista");
                        hoja1.View.ShowGridLines = false;

                        string startupPath = AppDomain.CurrentDomain.BaseDirectory;
                        string targetPath = startupPath + "Content\\img\\logo\\logo0.png";

                        //C:\Proyecto\SIGOPLAN\SEGURIDAD_SALUD_OCUPACIONAL
                        //C:\Users\Programador\Documents\SUBPRODUCTIVO\SIGOPLAN\Content\img\logo\logo.jpg
                        #region ENCABEZADO

#if DEBUG
                        System.Drawing.Image image = System.Drawing.Image.FromFile((targetPath));
#else
                         System.Drawing.Image image = System.Drawing.Image.FromFile((targetPath));
#endif

                        var obASignacion = db.tblCO_ADP_EvalSubConAsignacion.Where(r => r.id == idAsignacion).FirstOrDefault();
                        var objContratista = db.tblX_SubContratista.Where(r => r.id == obASignacion.idSubContratista).FirstOrDefault();
                        var firmantes = db.tblX_Firmante.Where(x => x.esActivo && (x.orden == 0 || obASignacion.cc == x.cc)).ToList();
                        var firmas = db.tblX_FirmaEvaluacion.FirstOrDefault(x => x.evaluacionId == idAsignacion);
                        var notificantes = db.tblCO_ADP_Notificante.Where(x => x.registroActivo && (x.esFijo || (!x.esFijo && x.cc == obASignacion.cc))).ToList();

                        var img = hoja1.Drawings.AddPicture("my logo", image);
                        img.From.Column = 1;
                        img.From.Row = 1;
                        img.SetSize(140, 70);

                        hoja1.Column(1).Width = 12;
                        hoja1.Column(2).Width = 38;
                        hoja1.Column(3).Width = 27;
                        hoja1.Column(4).Width = 32;
                        hoja1.Column(5).Width = 22;
                        hoja1.Column(6).Width = 20;
                        hoja1.Row(9).Height = 6;
                        hoja1.Row(11).Height = 6;
                        hoja1.Row(13).Height = 6;
                        hoja1.Row(15).Height = 6;

                        string TituloRangoConA = "A1:" + "A1";
                        hoja1.Cells[TituloRangoConA].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja1.Cells[TituloRangoConA].Merge = true;
                        hoja1.Cells[TituloRangoConA].AutoFitColumns(4);
                        hoja1.Cells[TituloRangoConA].Style.Font.Size = 4;
                        hoja1.Cells[TituloRangoConA].Style.Font.Color.SetColor(colorDeCelda);
                        hoja1.Cells[TituloRangoConA].Style.Fill.BackgroundColor.SetColor(colorDeCelda);

                        List<string[]> headerRow = new List<string[]>() { new string[] { 
                        "Sistema de Gestion de Proyectos"  ,
                    } };
                        string TituloRango = "B2:" + "F2";
                        hoja1.Cells[TituloRango].Merge = true;
                        hoja1.Cells[TituloRango].LoadFromArrays(headerRow);
                        //hoja1.Cells[TituloRango].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja1.Cells[TituloRango].Style.Font.Color.SetColor(ColorNegro);
                        hoja1.Cells[TituloRango].Style.Font.Bold = true;
                        //hoja1.Cells[TituloRango].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        //hoja1.Cells[TituloRango].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        //hoja1.Cells[TituloRango].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        //hoja1.Cells[TituloRango].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        //hoja1.Cells[TituloRango].Style.Fill.BackgroundColor.SetColor(colorDeCelda);
                        hoja1.Cells[TituloRango].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        hoja1.Cells[TituloRango].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        List<string[]> headerRow2 = new List<string[]>() { new string[] { 
                        "Evaluación a Subcontratistas"  ,
                    } };
                        //string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

                        string TituloRango2 = "B4:" + "F4";
                        hoja1.Cells[TituloRango2].Merge = true;
                        hoja1.Cells[TituloRango2].LoadFromArrays(headerRow2);
                        //hoja1.Cells[TituloRango2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja1.Cells[TituloRango2].Style.Font.Color.SetColor(ColorNegro);
                        hoja1.Cells[TituloRango2].Style.Font.Bold = true;
                        //hoja1.Cells[TituloRango2].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        //hoja1.Cells[TituloRango2].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        //hoja1.Cells[TituloRango2].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        //hoja1.Cells[TituloRango2].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        //hoja1.Cells[TituloRango2].Style.Fill.BackgroundColor.SetColor(colorDeCelda);
                        hoja1.Cells[TituloRango2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        hoja1.Cells[TituloRango2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        List<string[]> headerRow3 = new List<string[]>() { new string[] { 
                        "Nombre del Subcontratista: "  ,
                        objContratista.nombre == null ?"":objContratista.nombre
                    } };
                        //string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

                        string TituloRango3 = "B6:" + "B6";
                        hoja1.Cells[TituloRango3].Merge = true;
                        hoja1.Cells[TituloRango3].LoadFromArrays(headerRow3);
                        //hoja1.Cells[TituloRango3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja1.Cells[TituloRango3].Style.Font.Color.SetColor(ColorNegro);
                        hoja1.Cells[TituloRango3].Style.Font.Bold = true;
                        //hoja1.Cells[TituloRango3].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        //hoja1.Cells[TituloRango3].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        //hoja1.Cells[TituloRango3].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        //hoja1.Cells[TituloRango3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        //hoja1.Cells[TituloRango3].Style.Fill.BackgroundColor.SetColor(colorDeCelda);
                        hoja1.Cells[TituloRango3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        hoja1.Cells[TituloRango3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        List<string[]> headerRow4 = new List<string[]>() { new string[] { 
                        "Especialidad: "  ,
                        obASignacion.contrato.numeroContrato
                    } };
                        //string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

                        string TituloRango4 = "B8:" + "B8";
                        hoja1.Cells[TituloRango4].Merge = true;
                        hoja1.Cells[TituloRango4].LoadFromArrays(headerRow4);
                        //hoja1.Cells[TituloRango4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja1.Cells[TituloRango4].Style.Font.Color.SetColor(ColorNegro);
                        hoja1.Cells[TituloRango4].Style.Font.Bold = true;
                        //hoja1.Cells[TituloRango4].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        //hoja1.Cells[TituloRango4].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        //hoja1.Cells[TituloRango4].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        //hoja1.Cells[TituloRango4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        //hoja1.Cells[TituloRango4].Style.Fill.BackgroundColor.SetColor(colorDeCelda);
                        hoja1.Cells[TituloRango4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        hoja1.Cells[TituloRango4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                        List<string[]> headerRow5 = new List<string[]>() { new string[] { 
                        "Proyecto: "  ,
                        obASignacion.contrato.proyecto.nombre
                    } };
                        //string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

                        string TituloRango5 = "B10:" + "B10";
                        hoja1.Cells[TituloRango5].Merge = true;
                        hoja1.Cells[TituloRango5].LoadFromArrays(headerRow5);
                        //hoja1.Cells[TituloRango5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja1.Cells[TituloRango5].Style.Font.Color.SetColor(ColorNegro);
                        hoja1.Cells[TituloRango5].Style.Font.Bold = true;
                        //hoja1.Cells[TituloRango5].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        //hoja1.Cells[TituloRango5].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        //hoja1.Cells[TituloRango5].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        //hoja1.Cells[TituloRango5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        //hoja1.Cells[TituloRango5].Style.Fill.BackgroundColor.SetColor(colorDeCelda);
                        hoja1.Cells[TituloRango5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        hoja1.Cells[TituloRango5].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        List<string[]> headerRow6 = new List<string[]>() { new string[] { 
                        "Fecha de evaluacion: "  ,
                        obASignacion.fechaCreacion == null ? "" :obASignacion.fechaCreacion.ToString()
                    } };
                        //string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

                        string TituloRango6 = "B12:" + "B12";
                        hoja1.Cells[TituloRango6].Merge = true;
                        hoja1.Cells[TituloRango6].LoadFromArrays(headerRow6);
                        //hoja1.Cells[TituloRango6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja1.Cells[TituloRango6].Style.Font.Color.SetColor(ColorNegro);
                        hoja1.Cells[TituloRango6].Style.Font.Bold = true;
                        //hoja1.Cells[TituloRango6].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        //hoja1.Cells[TituloRango6].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        //hoja1.Cells[TituloRango6].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        //hoja1.Cells[TituloRango6].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        //hoja1.Cells[TituloRango6].Style.Fill.BackgroundColor.SetColor(colorDeCelda);
                        hoja1.Cells[TituloRango6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        hoja1.Cells[TituloRango6].Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                        List<string[]> headerRow7 = new List<string[]>() { new string[] { 
                        "Periodo de evaluacion: "  ,
                        obASignacion.fechaInicial.ToString("dd/MM/yyyy") + " AL " + obASignacion.fechaFinal.ToString("dd/MM/yyyy")
                    } };
                        //string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

                        string TituloRango7 = "D12:" + "D12";
                        hoja1.Cells[TituloRango7].Merge = true;
                        hoja1.Cells[TituloRango7].LoadFromArrays(headerRow7);
                        //hoja1.Cells[TituloRango7].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja1.Cells[TituloRango7].Style.Font.Color.SetColor(ColorNegro);
                        hoja1.Cells[TituloRango7].Style.Font.Bold = true;
                        //hoja1.Cells[TituloRango7].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        //hoja1.Cells[TituloRango7].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        //hoja1.Cells[TituloRango7].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        //hoja1.Cells[TituloRango7].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        //hoja1.Cells[TituloRango7].Style.Fill.BackgroundColor.SetColor(colorDeCelda);
                        hoja1.Cells[TituloRango7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        hoja1.Cells[TituloRango7].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        List<string[]> headerRow8 = new List<string[]>() { new string[] { 
                        "PROCESO DE RETROALIMENTACIÓN"  ,
                    } };
                        //string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

                        string TituloRango8 = "B14:" + "F14";
                        hoja1.Cells[TituloRango8].Merge = true;
                        hoja1.Cells[TituloRango8].LoadFromArrays(headerRow8);
                        hoja1.Cells[TituloRango8].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja1.Cells[TituloRango8].Style.Font.Color.SetColor(ColorNegro);
                        hoja1.Cells[TituloRango8].Style.Font.Bold = true;
                        hoja1.Cells[TituloRango8].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[TituloRango8].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[TituloRango8].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[TituloRango8].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[TituloRango8].Style.Fill.BackgroundColor.SetColor(colorDeNaranja);
                        hoja1.Cells[TituloRango8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        hoja1.Cells[TituloRango8].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        List<string[]> headerRow9 = new List<string[]>() { new string[] { 
                        "ELEMENTO CLAVE"  ,
                        "DESVIACIONES" ,
                        "PLANES DE ACCION" ,
                        "RESPONSABLE" ,
                        "FECHA COMPROMISO" ,
                    } };
                        //string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

                        string TituloRango9 = "B16:" + "F16";
                        hoja1.Cells[TituloRango9].LoadFromArrays(headerRow9);
                        hoja1.Cells[TituloRango9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja1.Cells[TituloRango9].Style.Font.Color.SetColor(colorDeCelda);
                        hoja1.Cells[TituloRango9].Style.Font.Bold = true;
                        hoja1.Cells[TituloRango9].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[TituloRango9].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[TituloRango9].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[TituloRango9].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        hoja1.Cells[TituloRango9].Style.Fill.BackgroundColor.SetColor(ColorNegro);
                        hoja1.Cells[TituloRango9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        hoja1.Cells[TituloRango9].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        #endregion


                        var obtenerListadoImprimible = obtenerListado(idAsignacion);


                        //string headerRange = "B18:" + "F18";



                        var cellData = new List<object[]>();
                        var cellDataC = new List<object[]>();

                        int cont = 17;
                        int contC = 17;
                        int contP = 17;
                        int contR = 17;
                        int contF = 17;
                        foreach (var item in obtenerListadoImprimible)
                        {
                            cellData = new List<object[]>();
                            cellData.Add(new object[]{
                                      item.Divicion.Trim(),
                                    });

                            cont++;
                            if (item.numeroMayor != 0)
                            {
                                if (!item.Desviaciones.Any(x => x.texto != ""))
                                {
                                    cont--;
                                    continue;
                                }
                                string headAling = "B" + cont + ":" + "F" + cont;
                                int valid = item.numeroMayor == 0 ? 1 : item.numeroMayor;
                                string headA = "B" + cont + ":" + "B" + ((cont + valid) - 1).ToString();
                                hoja1.Cells[headAling].LoadFromArrays(cellData);
                                hoja1.Cells[headA].Merge = true;
                                hoja1.Cells[headA].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                hoja1.Cells[headA].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                hoja1.Cells[headA].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                hoja1.Cells[headA].Style.Fill.BackgroundColor.SetColor(colorDeCelda);
                                hoja1.Cells[headA].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                hoja1.Cells[headA].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                hoja1.Cells[headA].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                hoja1.Cells[headA].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                hoja1.Cells[headAling].Style.Font.Bold = true;
                                hoja1.Cells[headAling].Style.WrapText = true;
                            }

                            foreach (var item2 in item.Desviaciones)
                            {
                                cellDataC = new List<object[]>();
                                cellDataC.Add(new object[]{
                                      item2.texto== null ?"":item2.texto.Trim() ,
                                    });
                                contC++;
                                string headAlingC = "C" + contC + ":" + "C" + contC;
                                hoja1.Cells[headAlingC].LoadFromArrays(cellDataC);
                                hoja1.Cells[headAlingC].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                hoja1.Cells[headAlingC].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                hoja1.Cells[headAlingC].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                hoja1.Cells[headAlingC].Style.Fill.BackgroundColor.SetColor(colorDeCelda);
                                hoja1.Cells[headAlingC].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                hoja1.Cells[headAlingC].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                hoja1.Cells[headAlingC].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                hoja1.Cells[headAlingC].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            }

                            foreach (var item2 in item.PlanesDeAccion)
                            {
                                cellDataC = new List<object[]>();
                                cellDataC.Add(new object[]{
                                      item2.texto== null ?"":item2.texto.Trim() ,
                                    });
                                contP++;
                                string headAlingC = "D" + contP + ":" + "D" + contP;
                                hoja1.Cells[headAlingC].LoadFromArrays(cellDataC);
                                hoja1.Cells[headAlingC].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                hoja1.Cells[headAlingC].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                hoja1.Cells[headAlingC].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                hoja1.Cells[headAlingC].Style.Fill.BackgroundColor.SetColor(colorDeCelda);
                                hoja1.Cells[headAlingC].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                hoja1.Cells[headAlingC].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                hoja1.Cells[headAlingC].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                hoja1.Cells[headAlingC].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            }
                            foreach (var item2 in item.Responsable)
                            {
                                cellDataC = new List<object[]>();
                                cellDataC.Add(new object[]{
                                      item2.texto== null ?"":item2.texto.Trim() ,
                                    });
                                contR++;
                                string headAlingC = "E" + contR + ":" + "E" + contR;
                                hoja1.Cells[headAlingC].LoadFromArrays(cellDataC);
                                hoja1.Cells[headAlingC].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                hoja1.Cells[headAlingC].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                hoja1.Cells[headAlingC].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                hoja1.Cells[headAlingC].Style.Fill.BackgroundColor.SetColor(colorDeCelda);
                                hoja1.Cells[headAlingC].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                hoja1.Cells[headAlingC].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                hoja1.Cells[headAlingC].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                hoja1.Cells[headAlingC].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            }
                            foreach (var item2 in item.FechaCompromiso)
                            {
                                cellDataC = new List<object[]>();
                                cellDataC.Add(new object[]{
                                      item2.texto== null ?"":item2.texto.Trim() ,
                                    });
                                contF++;
                                string headAlingC = "F" + contF + ":" + "F" + contF;
                                hoja1.Cells[headAlingC].LoadFromArrays(cellDataC);
                                hoja1.Cells[headAlingC].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                hoja1.Cells[headAlingC].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                hoja1.Cells[headAlingC].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                hoja1.Cells[headAlingC].Style.Fill.BackgroundColor.SetColor(colorDeCelda);
                                hoja1.Cells[headAlingC].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                hoja1.Cells[headAlingC].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                hoja1.Cells[headAlingC].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                hoja1.Cells[headAlingC].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            }



                            cont = (cont + item.numeroMayor) - 1;
                        }

                        for (int i = 18; i < cont; i++)
                        {
                            hoja1.Row(i).Height = 68;
                        }
                        cont = cont + 5;
                        cellDataC = new List<object[]>();
                        cellDataC.Add(new object[]{
                                      "EVALUACION GENERAL:" ,
                                       obtenerListadoImprimible.Sum(r=>r.Calificacion) / obtenerListadoImprimible.Count(),
                                    });

                        string headAlingD = "B" + cont + ":" + "B" + cont;
                        hoja1.Cells[headAlingD].LoadFromArrays(cellDataC);
                        //hoja1.Cells[headAlingD].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja1.Cells[headAlingD].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        hoja1.Cells[headAlingD].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        //hoja1.Cells[headAlingD].Style.Fill.BackgroundColor.SetColor(colorDeCelda);
                        //hoja1.Cells[headAlingD].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        //hoja1.Cells[headAlingD].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        //hoja1.Cells[headAlingD].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        //hoja1.Cells[headAlingD].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        cont += 3;
                        #region seccion firmas
                        var colFirmante = 2;
                        var cantidadDeFirmasEnUnRenglon = 0;
                        foreach (var firmante in firmantes)
                        {
                            hoja1.Cells[cont, colFirmante, cont + 4, colFirmante].Merge = true;
                            hoja1.Column(colFirmante).Width = 38D;

                            tblX_FirmaEvaluacionDetalle firmado = firmas != null ? firmas.detalle.FirstOrDefault(x => x.firmanteId == firmante.id) : null;
                            if (firmado != null)
                            {
                                var firmaDigital = Image.FromFile(firmado.urlFirma);
                                var excelPicture = hoja1.Drawings.AddPicture("firma" + colFirmante.ToString() + cont.ToString(), firmaDigital);
                                excelPicture.SetSize(250, 80);
                                excelPicture.SetPosition(cont - 1, 10, colFirmante - 1, 10);
                            }

                            var cont2 = cont + 5;

                            hoja1.Cells[cont2, colFirmante, cont2 + 1, colFirmante].Merge = true;
                            hoja1.Cells[cont2, colFirmante].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                            hoja1.Cells[cont2, colFirmante].Value = firmante.puesto + " " + firmante.apellidoPaterno + (!string.IsNullOrEmpty(firmante.apellidoMaterno) ? " " + firmante.apellidoMaterno : "");
                            hoja1.Cells[cont2, colFirmante].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            hoja1.Cells[cont2, colFirmante].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            hoja1.Cells[cont2, colFirmante].Style.WrapText = true;

                            colFirmante++;
                            cantidadDeFirmasEnUnRenglon++;

                            if (cantidadDeFirmasEnUnRenglon == 4)
                            {
                                cont += 9;
                                colFirmante = 2;
                            }
                        }
                        foreach (var notificante in notificantes)
                        {
                            hoja1.Cells[cont, colFirmante, cont + 4, colFirmante].Merge = true;
                            hoja1.Column(colFirmante).Width = 38D;

                            var cont2 = cont + 5;

                            hoja1.Cells[cont2, colFirmante, cont2 + 1, colFirmante].Merge = true;
                            hoja1.Cells[cont2, colFirmante].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                            hoja1.Cells[cont2, colFirmante].Value = "ccp. " + notificante.puesto + " " + notificante.nombre;
                            hoja1.Cells[cont2, colFirmante].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            hoja1.Cells[cont2, colFirmante].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            hoja1.Cells[cont2, colFirmante].Style.WrapText = true;

                            colFirmante++;
                            cantidadDeFirmasEnUnRenglon++;

                            if (cantidadDeFirmasEnUnRenglon == 4)
                            {
                                cont += 9;
                                colFirmante = 2;
                            }
                        }
                        //cont += 3;
                        //hoja1.Cells[cont, 2, cont + 4, 2].Merge = true;
                        //var firma = hoja1.MergedCells[cont, 1];
                        ////hoja1.Row(cont).Height = 70D;
                        //hoja1.Column(2).Width = 38D;
                        //var imgFirma = Image.FromFile(firmas.detalle.First().urlFirma);
                        //var asd = hoja1.Drawings.AddPicture(cont.ToString(), imgFirma);
                        //asd.SetSize(250, 80);
                        //asd.Locked = false;
                        //asd.SetPosition(cont - 1, 10, 1, 10);

                        //hoja1.Cells[cont + 5, 2, cont + 6, 2].Merge = true;
                        //hoja1.Cells[cont + 5, 2].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                        //hoja1.Cells[cont + 5, 2].Value = firmas.detalle.First().firmante.puesto + " " + firmas.detalle.First().firmante.nombre + " " + firmas.detalle.First().firmante.apellidoPaterno + " " + firmas.detalle.First().firmante.apellidoMaterno;
                        //hoja1.Cells[cont + 5, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //hoja1.Cells[cont + 5, 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        //hoja1.Cells[cont + 5, 2].Style.WrapText = true;
                        #endregion

                        excel.Compression = OfficeOpenXml.CompressionLevel.BestSpeed;

                        List<byte[]> lista = new List<byte[]>();


                        using (var exportData = new MemoryStream())
                        {
                            excel.SaveAs(exportData);
                            bytes = exportData;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return bytes;
        }
        public tblCO_ADP_EvalSubConAsignacion traermeDatosPrincipales(int idAsignacion)
        {
            try
            {
                if (idAsignacion > 0)
                    return db.tblCO_ADP_EvalSubConAsignacion.Where(r => r.id == idAsignacion).FirstOrDefault();

                return null;
            }
            catch (Exception ex)
            {
                return null;
                throw;
            }
        }
        public List<excelDTO> obtenerListado(int idAsignacion)
        {
            List<excelDTO> obtenerListado = new List<excelDTO>();

            var lstSubContratista = db.tblCO_ADP_EvalSubContratista.Where(r => r.idSubConAsignacion == idAsignacion).ToList();
            List<tblCO_ADP_EvalSubContratistaDet> lstSubContratistaDet = new List<tblCO_ADP_EvalSubContratistaDet>();
            foreach (var item in lstSubContratista)
            {
                var lst = db.tblCO_ADP_EvalSubContratistaDet.Where(r => r.idEvaluacion == item.id).ToList();
                lstSubContratistaDet.AddRange(lst);
            }

            obtenerListado = db.tblCO_ADP_EvaluacionDiv.Where(r => r.SubContratista == false).ToList().Select(y => new excelDTO
            {
                id = y.id,
                Divicion = y.descripcion,
                Desviaciones = lstSubContratistaDet.Where(r => r.tipoEvaluacion == y.id && r.opcional == false && (r.comentario != null || r.fechaCompromiso != null || r.responsable != null || r.planesDeAccion != null)).ToList().Select(n => new resultDTO { texto = n.comentario }).ToList(),
                PlanesDeAccion = lstSubContratistaDet.Where(r => r.tipoEvaluacion == y.id && r.opcional == false && (r.comentario != null || r.fechaCompromiso != null || r.responsable != null || r.planesDeAccion != null)).ToList().Select(n => new resultDTO { texto = n.planesDeAccion }).ToList(),
                Responsable = lstSubContratistaDet.Where(r => r.tipoEvaluacion == y.id && r.opcional == false && (r.comentario != null || r.fechaCompromiso != null || r.responsable != null || r.planesDeAccion != null)).ToList().Select(n => new resultDTO { texto = n.responsable }).ToList(),
                FechaCompromiso = lstSubContratistaDet.Where(r => r.tipoEvaluacion == y.id && r.opcional == false && (r.comentario != null || r.fechaCompromiso != null || r.responsable != null || r.planesDeAccion != null)).ToList().Select(n => new resultDTO { texto = n.fechaCompromiso }).ToList(),
                numeroMayor = lstSubContratistaDet.Where(r => r.tipoEvaluacion == y.id && r.opcional == false && (r.comentario != null || r.fechaCompromiso != null || r.responsable != null || r.planesDeAccion != null)).ToList().Count(),
                Calificacion = lstSubContratistaDet.Where(r => r.tipoEvaluacion == y.id && r.opcional == false && (r.comentario != null || r.fechaCompromiso != null || r.responsable != null || r.planesDeAccion != null)).ToList().Select(n => new resultDTO { calificacion = n.calificacion }).ToList().Sum(r => r.calificacion) == 0 ? 1 : lstSubContratistaDet.Where(r => r.tipoEvaluacion == y.id && r.opcional == false).ToList().Select(n => new resultDTO { calificacion = n.calificacion }).ToList().Sum(r => r.calificacion) / lstSubContratistaDet.Where(r => r.tipoEvaluacion == y.id && r.opcional == false).ToList().Count(),
            }).ToList();

            return obtenerListado;
        }
        public List<tblEN_Estrellas> getEstrellas()
        {
            return _context.Select<tblEN_Estrellas>(new DapperDTO
            {
                baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                consulta = @"SELECT * FROM tblEN_Estrellas"
            }).ToList();
            //return db.tblEN_Estrellas.ToList();
        }
        public ContratoSubContratistaDTO EvaluarDetalle(int id, List<SubContratistasDTO> objDTO, int userEnvia)
        {
            ContratoSubContratistaDTO objre = new ContratoSubContratistaDTO();
            try
            {
                #region EVALUAR DETALLE

                #region SE OBTIENE INFORMACIÓN DEL SUBCONTRATISTA
                string strQuery = string.Format("SELECT * FROM tblX_SubContratista WHERE nombre LIKE '%{0}%'", objDTO[0].nombreSubcontratista);
                int idSubcontratista = _context.Select<int>(new DapperDTO
                {
                    baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                    consulta = strQuery
                }).FirstOrDefault();

                int idEvaluacion = 0;
                if (idSubcontratista > 0)
                {
                    strQuery = string.Format("SELECT * FROM tblCO_ADP_EvalSubConAsignacion WHERE idSubContratista = {0} AND cc = '{1}' AND fechaInicial = '{2}' AND fechaFinal = '{3}'",
                                                idSubcontratista, objDTO[0].cc, objDTO[0].strFechaInicial, objDTO[0].strFechaFinal);
                    idEvaluacion = _context.Select<int>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                        consulta = strQuery
                    }).FirstOrDefault();
                }
                #endregion

                #region SE OBTIENE el IDROW DE LA EVALUACIÓN
                strQuery = string.Format("SELECT texto FROM tblCO_ADP_EvaluacionReq WHERE id = {0}", objDTO[0].id);
                string strRow = _context.Select<string>(new DapperDTO
                {
                    baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                    consulta = strQuery
                }).FirstOrDefault();

                string idRow = strRow.Substring(0, 1);
                int row = Convert.ToInt32(idRow);
                #endregion

                #region SE CREA ALERTA AL SUBCONTRATISTA
                tblP_Alerta objAlerta = new tblP_Alerta();
                objAlerta.userEnviaID = userEnvia;
                objAlerta.userRecibeID = idSubcontratista;
#if DEBUG
                objAlerta.userRecibeID = 7939;
#endif
                objAlerta.msj = "Tienes una evaluación pendiente";
                objAlerta.visto = false;
                objAlerta.tipoAlerta = 2;
                objAlerta.sistemaID = 2;
                objAlerta.objID = id;
                objAlerta.url = "/ControlObra/ControlObra/DashboardSubContratista";
                db.tblP_Alerta.Add(objAlerta);
                db.SaveChanges();
                #endregion

                tblX_SubContratista objSubContra = db.tblX_SubContratista.Where(r => r.id == idSubcontratista).FirstOrDefault();
                if (objSubContra != null)
                {
                    List<tblPUsuarioDTO> lstUsuario = new List<tblPUsuarioDTO>();
                    string sql = @"SELECT * FROM tblP_Usuario WHERE tipo=3 AND estatus='true'";
                    using (var ctx = new SqlConnection(ConextSigoDapper.conexionSUBCONTRATISTAS()))
                    {
                        ctx.Open();
                        lstUsuario = ctx.Query<tblPUsuarioDTO>(sql, null, null, true, 300).ToList();
                        ctx.Close();
                    }
                    var objAsignacion = db.tblCO_ADP_EvalSubConAsignacion.FirstOrDefault(r => r.id == idEvaluacion);

                    var objUsuario = lstUsuario.Where(r => r._user == objSubContra.rfc).FirstOrDefault();

                    #region SE VERIFICA SI CUENTA CON ALGUN FIRMANTE ACTIVO, EN BASE AL SUBCONTRATISTA Y CONTRATO
                    // SE OBTIENE ID_SUBCONTRATISTA Y ID_CONTRATO.
                    strQuery = string.Format("SELECT nombreEvaluacion, idSubContratista, idContrato FROM tblCO_ADP_EvalSubConAsignacion WHERE id = {0}", idEvaluacion);
                    tblCO_ADP_EvalSubConAsignacion objEvaluacion = _context.Select<tblCO_ADP_EvalSubConAsignacion>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                        consulta = strQuery
                    }).FirstOrDefault();
                    if (objEvaluacion == null)
                        throw new Exception("Ocurrió un error al generar la asignación de la evaluación.");

                    // SE VERIFICA EN BASE AL ID_SUBCONTRATISTA Y ID_CONTRATO, QUE CONTENGA ALGUN FIRMANTE ACTIVO.
                    strQuery = string.Format("SELECT correo FROM tblCO_ADP_UsuariosFirmantesRelSubcontratistas WHERE idUsuarioSubcontratista = {0} AND idContrato LIKE '%{1}%' AND registroActivo = {2}",
                        objEvaluacion.idSubContratista, objEvaluacion.idContrato, 1);
                    tblCO_ADP_UsuariosFirmantesRelSubcontratistas objFirmante = _context.Select<tblCO_ADP_UsuariosFirmantesRelSubcontratistas>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Construplan,
                        consulta = strQuery
                    }).FirstOrDefault();
                    if (objFirmante == null)
                        throw new Exception("Es necesario indicar un firmante al Subcontratista y Contrato seleccionado.");
                    #endregion

                    #region SE OBTIENE EL ELEMENTO A NOTIFICAR
                    strQuery = string.Format("SELECT idDiv FROM tblCO_ADP_EvaluacionReq WHERE id = {0}", objDTO[0].id);
                    int idDiv = _context.Select<int>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                        consulta = strQuery
                    }).FirstOrDefault();
                    if (idDiv <= 0)
                        throw new Exception("Ocurrió un error al obtener el nombre del elemento evaluado.");

                    strQuery = string.Format("SELECT descripcion FROM tblCO_ADP_EvaluacionDiv WHERE id = {0}", idDiv);
                    string elementoEvaluado = _context.Select<string>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                        consulta = strQuery
                    }).FirstOrDefault();
                    #endregion

                    #region SE OBTIENE EL CORREO DE LOS ADMINISTRADORES DEL CC Y EVALUADORES
                    List<string> lstCorreos = new List<string>();
                    List<tblCO_ADP_Facultamientos> lstUsuariosID = new List<tblCO_ADP_Facultamientos>();
                    strQuery = string.Format("SELECT idUsuario, tipo FROM tblCO_ADP_Facultamientos WHERE (tipo = {0} OR tipo = {1}) AND cc LIKE '%{2}%'", (int)TipoUsuariosEnum.administrador, (int)TipoUsuariosEnum.evaluador, objDTO[0].cc);
                    lstUsuariosID = _context.Select<tblCO_ADP_Facultamientos>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                        consulta = strQuery
                    }).ToList();

                    // SE OBTIENE A LOS USUARIOS EVALUADORES DEL ELEMENTO A NOTIFICAR
                    string whereElementoID = string.Empty;
                    whereElementoID = idDiv.ToString();
                    strQuery = string.Format("SELECT evaluador FROM tblCO_ADP_EvaluadorXcc WHERE (elementos LIKE '%{0}%') AND evaluador IN ({1})", whereElementoID, string.Join(",", lstUsuariosID.Where(w => w.tipo == (int)TipoUsuariosEnum.evaluador).Select(s => s.idUsuario)));
                    List<int> lstUsuariosEvaluadoresRelElementoNotificar = _context.Select<int>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                        consulta = strQuery
                    }).ToList();


                    List<int> _lstUsuariosID = lstUsuariosID.Where(w => w.tipo == (int)TipoUsuariosEnum.administrador).Select(s => s.idUsuario).ToList();
                    foreach (var item in lstUsuariosEvaluadoresRelElementoNotificar.Select(s => s).ToList())
                    {
                        _lstUsuariosID.Add(item);
                    }
                    List<tblP_Usuario> lstUsuarios = _context.tblP_Usuario.Where(w => _lstUsuariosID.Contains(w.id)).ToList();
                    foreach (var item in lstUsuarios)
                    {
                        lstCorreos.Add(item.correo);
                    }
                    #endregion

                    //string Subject = "El nombre de la evaluacion : " + objAsignacion.nombreEvaluacion + " El elemento a revisar es :" + Evaluacion;
                    string msg2 = CuerpoCorreo(objAsignacion, objUsuario);
                    List<FirmantesDTO> lstFirmantes = new List<FirmantesDTO>();
                    string msg = GlobalUtils.cuerpoDeCorreoFormato("Prestadores de servicio", "Evaluaciones", string.Format("Elemento evaluado: {0}", elementoEvaluado), msg2, "Gracias por su atención.", "http://expediente.construplan.com.mx/", lstFirmantes);

                    lstCorreos.Add(objFirmante.correo);

#if DEBUG
                    lstCorreos = new List<string>();
                    lstCorreos.Add("omar.nunez@construplan.com.mx");
#else
                    lstCorreos = new List<string>();
                    lstCorreos.Add("maricela.ortiz@construplan.com.mx");
                    lstCorreos.Add("omar.nunez@construplan.com.mx");
#endif
                    string asunto = "Se evaluó: " + objEvaluacion.nombreEvaluacion;
                    GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), asunto), msg, lstCorreos);
                }
                #endregion

            }
            catch (Exception e)
            {
                objre.status = 0;
                objre.mensaje = "Algo ocurrio mal comuniquese con el departamento de ti.";
            }
            return objre;
        }

        public List<SubContratistasDTO> obtenerPromedioxElemento(List<SubContratistasDTO> parametros)
        {
            try
            {
                List<SubContratistasDTO> lstProm = new List<SubContratistasDTO>();
                int calculo = 0;

                foreach (var item in parametros)
                {
                    SubContratistasDTO promedio = new SubContratistasDTO();

                    var obj = db.tblCO_ADP_EvalSubConAsignacion.Where(r => r.id == item.idAsignacion).FirstOrDefault();
                    if (obj != null)
                    {
                        var objInicio = db.tblCO_ADP_EvalSubContratista.Where(r => r.idSubConAsignacion == obj.id && r.tipoEvaluacion == item.tipoEvaluacion).FirstOrDefault();

                        if (objInicio != null)
                        {
                            var lstDeCalificaciones = db.tblCO_ADP_EvalSubContratistaDet.Where(r => r.calificacion > 0 && r.tipoEvaluacion == item.tipoEvaluacion && r.idEvaluacion == objInicio.id).ToList();
                            var lstReq = db.tblCO_ADP_EvaluacionReq.Where(r => r.idDiv == item.tipoEvaluacion).ToList();
                            if (lstDeCalificaciones.Count() != 0)
                            {
                                foreach (var item2 in lstDeCalificaciones)
                                {
                                    calculo += item2.calificacion;
                                }
                                promedio.promedio = Convert.ToDouble(calculo / lstReq.Count());
                                promedio.tipoEvaluacion = item.tipoEvaluacion;
                            }
                            else
                            {
                                promedio = new SubContratistasDTO();
                                promedio.promedio = 0;
                                promedio.tipoEvaluacion = item.tipoEvaluacion;
                            }

                        }
                        else
                        {
                            promedio = new SubContratistasDTO();
                            promedio.promedio = 0;
                            promedio.tipoEvaluacion = item.tipoEvaluacion;
                        }

                        var tieneCargaDeArchivo = db.tblCO_ADP_EvalSubContratistaDet.Any(x => x.tipoEvaluacion == item.tipoEvaluacion && x.idEvaluacion == objInicio.id && x.rutaArchivo != null);
                        promedio.tieneCargaDeArchivo = tieneCargaDeArchivo;
                    }
                    lstProm.Add(promedio);
                }
                return lstProm;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
        #region funciones Generales
        public List<evaluadorXccDTO> getEvaluadoresxCC(string cc, string elemento, int evaluadores)
        {
            #region SE OBTIENE LISTADO DE CC LIGADOS AL USUARIO LOGUEADO (DE FACULTAMIENTOS)
            string strQuery = string.Format(@"SELECT * FROM tblCO_ADP_Facultamientos WHERE idUsuario = {0}", (int)vSesiones.sesionUsuarioDTO.id);
            List<tblCO_ADP_Facultamientos> lstUsuarioFacultamientos = _context.Select<tblCO_ADP_Facultamientos>(new DapperDTO
            {
                baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                consulta = strQuery
            }).ToList();
            List<string> lstCC = new List<string>();
            List<string> lstUsuarioRelCC = lstUsuarioFacultamientos.Select(s => s.cc).ToList();
            foreach (var item in lstUsuarioRelCC)
            {
                string[] SplitCC = item.Split(',');
                foreach (var itemCC in SplitCC)
                {
                    if (!string.IsNullOrEmpty(itemCC) && itemCC != ",")
                    {
                        string _cc = itemCC.Replace(",", "");
                        lstCC.Add(string.Format("{0}", _cc));
                    }
                }
            }
            #endregion

            List<evaluadorXccDTO> lst = new List<evaluadorXccDTO>();
            //var lstUsuarios = obtenerUsuariosTipo10();

            strQuery = string.Format("SELECT * FROM tblP_Usuario WHERE estatus = {0}", 1);
            List<tblP_Usuario> lstUsuarios = _context.Select<tblP_Usuario>(new DapperDTO
            {
                baseDatos = MainContextEnum.Construplan,
                consulta = strQuery,
                parametros = new { registroActivo = true }
            }).ToList();

            lst = db.tblCO_ADP_EvaluadorXcc.Where(r => lstCC.Contains(r.cc) && r.esActivo).ToList().Select(y => new evaluadorXccDTO
            {
                id = y.id,
                evaluador = y.evaluador,
                //nombreEvaluador = lstUsuarios.Where(r => r.id == y.evaluador).FirstOrDefault() == null ? "" : lstUsuarios.Where(r => r.id == y.evaluador).FirstOrDefault().nombre,
                nombreEvaluador = string.Empty,
                esActivo = y.esActivo,
                cc = y.cc,
                lstCC = obtenerlst(y.cc),
                lstElem = y.elementos,
                lstElementos = obtenerElementos(y.elementos),
                lstElementos2 = obtenerFormatoLista(y.elementos),
                lstCC2 = obtenerFormatoLista(y.cc),
            }).ToList();

            foreach (var item in lst)
            {
                string nombreCompleto = string.Empty;
                tblP_Usuario objUsuario = lstUsuarios.Where(w => w.id == item.evaluador).FirstOrDefault();
                if (objUsuario != null)
                {
                    if (!string.IsNullOrEmpty(objUsuario.nombre))
                        nombreCompleto += objUsuario.nombre.Trim().ToUpper();
                    if (!string.IsNullOrEmpty(objUsuario.apellidoPaterno))
                        nombreCompleto += !string.IsNullOrEmpty(objUsuario.nombre) ? " " + objUsuario.apellidoPaterno.Trim().ToUpper() : objUsuario.apellidoPaterno.Trim().ToUpper();
                    if (!string.IsNullOrEmpty(objUsuario.apellidoMaterno))
                        nombreCompleto += !string.IsNullOrEmpty(objUsuario.apellidoPaterno) ? " " + objUsuario.apellidoMaterno.Trim().ToUpper() : objUsuario.apellidoMaterno.Trim().ToUpper();

                    item.nombreEvaluador = nombreCompleto;
                }
            }

            var resultado = lst.Where(r => (cc == "" ? r.cc == r.cc : r.lstCC2.Contains(cc))
                                        && (elemento == "" ? r.lstElem == r.lstElem : r.lstElementos2.Contains(elemento))
                                        && (evaluadores == 0 ? r.evaluador == r.evaluador : r.evaluador == evaluadores)
                                        ).ToList();
            return resultado;

        }
        public List<string> obtenerFormatoLista(string cadena)
        {
            var a = cadena.Split(',');
            List<string> b = new List<string>();
            if (a != null)
            {
                foreach (var item in a)
                {
                    if (item != "")
                    {
                        b.Add(item);
                    }
                }
            }
            return b;
        }
        public List<string> obtenerElementos(string cadena)
        {
            List<string> lst = new List<string>();
            if (cadena == null)
            {
                lst.Add("");
            }
            else
            {
                var Arr = cadena.Split(',');
                foreach (var item in Arr)
                {
                    if (item != "")
                    {
                        int id = Convert.ToInt32(item);
                        string nombreCC = db.tblCO_ADP_EvaluacionDiv.Where(r => r.id == id).FirstOrDefault() == null ? "" : db.tblCO_ADP_EvaluacionDiv.Where(r => r.id == id).FirstOrDefault().descripcion;
                        lst.Add(item + " - " + nombreCC);
                    }
                }
            }
            return lst;
        }
        public List<string> obtenerlst(string cadena)
        {
            List<string> lst = new List<string>();
            if (cadena == null)
            {
                lst.Add("");
            }
            else
            {
                var Arr = cadena.Split(',');
                foreach (var item in Arr)
                {
                    string nombreCC = _context.tblP_CC.Where(r => r.cc == item).FirstOrDefault() == null ? "" : _context.tblP_CC.Where(r => r.cc == item).FirstOrDefault().descripcion;
                    lst.Add(item + " - " + nombreCC);
                }
            }
            return lst;
        }
        public evaluadorXccDTO AgregarEditarEvaluadores(evaluadorXccDTO parametros)
        {
            evaluadorXccDTO obj = new evaluadorXccDTO();
            try
            {
                tblCO_ADP_EvaluadorXcc agregar = db.tblCO_ADP_EvaluadorXcc.Where(r => r.id == parametros.id && r.esActivo).FirstOrDefault();
                if (agregar == null)
                {
                    var Existe = db.tblCO_ADP_EvaluadorXcc.Where(r => r.evaluador == parametros.evaluador && r.esActivo).FirstOrDefault();
                    if (Existe == null)
                    {
                        Existe = new tblCO_ADP_EvaluadorXcc();
                        Existe.evaluador = parametros.evaluador;
                        Existe.esActivo = true;
                        Existe.elementos = parametros.lstElem;
                        Existe.cc = parametros.cc;
                        db.tblCO_ADP_EvaluadorXcc.Add(Existe);
                        db.SaveChanges();
                        obj.estatus = 1;
                        obj.mensaje = "Evaluador Agregado Con Exito";
                    }
                    else
                    {
                        obj.estatus = 2;
                        obj.mensaje = "Ya existe un evaluador revise porfavor";
                    }
                }
                else
                {
                    agregar.elementos = parametros.lstElem;
                    agregar.cc = parametros.cc;
                    db.SaveChanges();
                    obj.estatus = 1;
                    obj.mensaje = "Evaluador Modificado Con Exito";
                }

            }
            catch (Exception ex)
            {

            }
            return obj;
        }
        public evaluadorXccDTO ActivarDesactivarEvaluadores(evaluadorXccDTO parametros)
        {
            evaluadorXccDTO obj = new evaluadorXccDTO();
            try
            {
                var Activar = db.tblCO_ADP_EvaluadorXcc.Where(r => r.id == parametros.id).FirstOrDefault();
                if (Activar != null)
                {
                    Activar.esActivo = false;
                    db.SaveChanges();
                    obj.estatus = 1;
                    obj.mensaje = "Evaluador Eliminado Con Exito";
                }
                else
                {
                    obj.estatus = 2;
                    obj.mensaje = "Algo ocurrio al quere eliminar el registro comuniquese con el departamento de TI.";
                }

            }
            catch (Exception ex)
            {

            }
            return obj;
        }
        public List<ComboDTO> getProyectoRestantes(bool Agregar)
        {
            List<ComboDTO> obj = new List<ComboDTO>();
            try
            {
                List<string> todosLosCCActivos = new List<string>();
                List<string> todosLosCCActivosx = new List<string>();

                #region SE OBTIENE LISTADO DE CC LIGADOS AL USUARIO LOGUEADO (DE FACULTAMIENTOS)
                string strQuery = string.Format(@"SELECT * FROM tblCO_ADP_Facultamientos WHERE idUsuario = {0}", (int)vSesiones.sesionUsuarioDTO.id);
                List<tblCO_ADP_Facultamientos> lstUsuarioFacultamientos = _context.Select<tblCO_ADP_Facultamientos>(new DapperDTO
                {
                    baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                    consulta = strQuery
                }).ToList();
                List<string> lstCC = new List<string>();
                List<string> lstUsuarioRelCC = lstUsuarioFacultamientos.Select(s => s.cc).ToList();
                foreach (var item in lstUsuarioRelCC)
                {
                    string[] SplitCC = item.Split(',');
                    foreach (var itemCC in SplitCC)
                    {
                        if (!string.IsNullOrEmpty(itemCC) && itemCC != ",")
                        {
                            string _cc = itemCC.Replace(",", "");
                            lstCC.Add(string.Format("{0}", _cc));
                        }
                    }
                }
                #endregion

                if (Agregar == true)
                {
                    var lst = db.tblCO_ADP_EvaluadorXcc.Where(r => r.esActivo).Select(y => y.cc).ToList();
                    var lst2 = db.tblCO_ADP_EvaluadorXcc.Where(r => r.esActivo).ToList();
                    if (lst2.Count() > 0)
                    {
                        foreach (var item in lst)
                        {
                            todosLosCCActivos = obtenerTodolosCC(item);
                            foreach (var item2 in todosLosCCActivos)
                            {
                                todosLosCCActivosx.Add(item2);
                            }
                        }
                    }

                    //obj = _context.tblP_CC.Where(r => !todosLosCCActivosx.Contains(r.cc)).ToList().Select(y => new ComboDTO
                    obj = _context.tblP_CC.Where(r => lstCC.Contains(r.cc)).ToList().Select(y => new ComboDTO
                    {
                        Value = y.cc.ToString(),
                        Text = y.cc.ToString() + " - " + y.descripcion
                    }).ToList();
                }
                else
                {
                    var lst = db.tblCO_ADP_EvaluadorXcc.Where(r => r.esActivo).Select(y => y.cc).ToList();
                    var lst2 = db.tblCO_ADP_EvaluadorXcc.Where(r => r.esActivo).ToList();
                    if (lst2.Count() > 0)
                    {
                        foreach (var item in lst)
                        {
                            todosLosCCActivos = obtenerTodolosCC(item);
                            foreach (var item2 in todosLosCCActivos)
                            {
                                todosLosCCActivosx.Add(item2);
                            }
                        }
                    }

                    //obj = _context.tblP_CC.ToList().Select(y => new ComboDTO
                    obj = _context.tblP_CC.Where(w => lstCC.Contains(w.cc)).ToList().Select(y => new ComboDTO
                    {
                        Value = y.cc.ToString(),
                        Text = y.cc.ToString() + " - " + y.descripcion
                    }).ToList();
                }


            }
            catch (Exception ex)
            {

            }
            return obj;
        }
        public List<ComboDTO> obtenerTodolosElementos()
        {
            List<ComboDTO> lstobtenerElementos = new List<ComboDTO>();
            try
            {
                lstobtenerElementos = db.tblCO_ADP_EvaluacionDiv.Where(r => !r.SubContratista).Select(y => new ComboDTO
                {
                    Value = y.id.ToString(),
                    Text = y.descripcion
                }).ToList();
            }
            catch (Exception ex)
            {
            }
            return lstobtenerElementos;
        }
        public List<string> obtenerTodolosCC(string cc)
        {
            List<string> lst = new List<string>();
            if (cc == null)
            {
                lst.Add("");
            }
            else
            {
                var array = cc.Split(',');
                foreach (var item in array)
                {
                    lst.Add(item);
                }
            }
            return lst;
        }

        public List<ComboDTO> getSubContratistasRestantes()
        {
            List<ComboDTO> obj = new List<ComboDTO>();
            try
            {
                string sql = @"SELECT * FROM tblP_Usuario WHERE tipo=10 AND estatus='true'";
                using (var ctx = new SqlConnection(ConextSigoDapper.conexionSUBCONTRATISTAS()))
                {
                    ctx.Open();
                    obj = ctx.Query<tblPUsuarioDTO>(sql, null, null, true, 300).ToList().Select(y => new ComboDTO
                    {
                        Value = y.id.ToString(),
                        Text = y.nombre_completo
                    }).ToList();
                    ctx.Close();
                }
            }
            catch (Exception ex)
            {

            }
            return obj;
        }
        public string formatearStrign(List<int> datos)
        {
            string n = "";
            foreach (var item in datos)
            {
                n += item + ",";
            }
            return n;
        }
        public List<tblPUsuarioDTO> obtenerSubcontratistas()
        {
            List<tblPUsuarioDTO> lst = new List<tblPUsuarioDTO>();


            return lst;

        }
        public Dictionary<string, object> obtenerElementosEvaluar(int idUsuario, int idPlantilla, int idAsignacion)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                List<tblPUsuarioDTO> lstUsuario = new List<tblPUsuarioDTO>();
                string sql = @"SELECT * FROM tblP_Usuario WHERE estatus='true'";
                using (var ctx = new SqlConnection(ConextSigoDapper.conexionSUBCONTRATISTAS()))
                {
                    ctx.Open();
                    lstUsuario = ctx.Query<tblPUsuarioDTO>(sql, null, null, true, 300).ToList();
                    ctx.Close();
                }
                var obtener = lstUsuario.Where(r => r.id == 1).FirstOrDefault();
                bool elementoVerde = false;
                if (obtener != null)
                {
                    if (obtener.tipo == 1002)
                    {
                        var l = db.tblCO_ADP_EvaluacionDiv.Where(x => !x.SubContratista && x.idPlantilla == idPlantilla).ToList().Select(n => n.id).ToList();
                        var s = formatearStrign(l);
                        resultado.Add(ITEMS, s);
                        resultado.Add(SUCCESS, true);
                    }
                    else if (obtener.tipo == 10)
                    {
                        var n = db.tblCO_ADP_EvaluadorXcc.Where(r => r.evaluador == idUsuario && r.esActivo).FirstOrDefault();
                        if (n != null)
                        {
                            resultado.Add(ITEMS, n.elementos);
                            resultado.Add(SUCCESS, true);
                        }
                    }
                    else
                    {
                        var l = db.tblCO_ADP_EvaluacionDiv.Where(x => !x.SubContratista && x.idPlantilla == idPlantilla).ToList().Select(n => n.id).ToList();
                        var s = formatearStrign(l);

                        #region SE VERIFICA SI LAS EVALUACIONES YA FUERON REALIZAS PARA COLOREAR EL ELEMENTO EN VERDE
                        //foreach (var item in l)
                        //{
                        //    int idEvaluacion = db.tblCO_ADP_EvalSubConAsignacion.Where(w => w.id == idAsignacion).Select(k => k.id).FirstOrDefault();

                        //    int idElemento = item;
                        //    List<tblCO_ADP_EvalSubContratistaDet> lstEvaluaciones = db.tblCO_ADP_EvalSubContratistaDet.Where(w => w.idEvaluacion == idEvaluacion && w.tipoEvaluacion == idElemento).ToList();
                        //    int cantEvaluaciones = lstEvaluaciones.Count();
                        //    int cantEvaluacionesRealizadas = lstEvaluaciones.Where(w => w.calificacion > 0).Count();
                        //    if (cantEvaluaciones == cantEvaluacionesRealizadas)
                        //        elementoVerde = true;
                        //}
                        #endregion

                        resultado.Add(ITEMS, s);
                        resultado.Add("elementoVerde", elementoVerde);
                        resultado.Add(SUCCESS, true);
                    }
                }

            }
            catch (Exception ex)
            {
                resultado.Add(ITEMS, "");
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public double ObtenerPromedioPorElemento(tblCO_ADP_EvalSubContratista objContratista)
        {
            double total = 0;
            var lstDetalle = db.tblCO_ADP_EvalSubContratistaDet.Where(r => r.idEvaluacion == objContratista.id).ToList();
            foreach (var item in lstDetalle)
            {
                total += item.calificacion;
            }
            if (total == 0)
            {
                return total;
            }
            else
            {
                total = total / lstDetalle.Count();
            }
            return total;
        }
        #endregion
        #region GRAFICAS
        // ESTA ES LA GRAFICA DE CENTROS DE COSTOS
        public Dictionary<string, object> ObtenerGraficaDeEvaluacionPorCentroDeCosto(SubContratistasDTO parametros)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                //AQUI ESTOY TRABAJANDO
                var lstCentrosDeCostosAsignados = db.tblCO_ADP_EvalSubConAsignacion.Select(y => y.cc).ToList().Distinct();
                var lstCentrosDeCostos = _context.tblP_CC.Where(r => lstCentrosDeCostosAsignados.Contains(r.cc)).Select(y => new
                {
                    cc = y.cc,
                    descripcion = y.cc + " - " + y.descripcion
                }).ToList();
                var lstDatoPendientes = db.tblCO_ADP_EvalSubConAsignacion.Where(r => (r.statusAutorizacion == 2 || r.statusAutorizacion == 1) && r.esActivo).ToList();
                var lstDatoRealizadas = db.tblCO_ADP_EvalSubConAsignacion.Where(r => (r.statusAutorizacion == 3 || r.statusAutorizacion == 4) && !r.esActivo).ToList();
                var ListaDeDatosPendientesFiltrados = lstCentrosDeCostos.Select(y => new
                {
                    y = lstDatoPendientes.Where(r => r.cc == y.cc).ToList().Count(),
                    name = y.descripcion,
                }).ToList();
                var ListaDeDatosRealizadasFiltrados = lstCentrosDeCostos.Select(y => new
                {
                    y = lstDatoRealizadas.Where(r => r.cc == y.cc).ToList().Count(),
                    name = y.descripcion,
                }).ToList();

                var ObjDatoPendiente = new
                {
                    name = "Pendientes",
                    colorByPoint = true,
                    data = ListaDeDatosPendientesFiltrados,
                };
                var ObjDatoRealizado = new
                {
                    name = "Realizadas",
                    colorByPoint = true,
                    data = ListaDeDatosRealizadasFiltrados,
                };
                List<object> lstResultadoDeGRaficas = new List<object>();
                lstResultadoDeGRaficas.Add(ObjDatoPendiente);
                lstResultadoDeGRaficas.Add(ObjDatoRealizado);

                int lista1 = ListaDeDatosPendientesFiltrados.Select(y => y.y).Max();
                int lista2 = ListaDeDatosRealizadasFiltrados.Select(y => y.y).Max();
                int numeroMaximo = 0;
                if (lista1 > lista2)
                {
                    numeroMaximo = lista1 + 1;
                }
                else
                {
                    numeroMaximo = lista2 + 1;
                }

                resultado.Add("numMaximo", numeroMaximo);
                resultado.Add("gpxGrafica", lstResultadoDeGRaficas);
                resultado.Add(SUCCESS, true);

            }
            catch (Exception ex)
            {

            }
            return resultado;
        }
        // GRAFICA DE SUBCONTRATISTAS POR DIVISION/ELEMENTO
        public Dictionary<string, object> ObtenerGraficaDeEvaluacionPorDivisionElemento(SubContratistasDTO parametros)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                //AQUI ESTOY TRABAJANDO
                var LstElementos = db.tblCO_ADP_EvaluacionDiv.Where(r => !r.SubContratista).ToList();

                var lstContratista = db.tblCO_ADP_EvalSubContratista.Where(r => r.idSubConAsignacion == parametros.id).ToList();
                if (lstContratista != null)
                {
                    var lstSubContratistas = LstElementos.Select(y => new
                    {
                        name = y.descripcion,
                        y = ObtenerPromedioPorElemento(lstContratista.Where(r => r.tipoEvaluacion == y.id).FirstOrDefault()),
                    }).ToList();


                    var ObjDatoRealizado = new
                    {
                        name = "Calificacion",
                        colorByPoint = true,
                        data = lstSubContratistas,
                    };
                    List<object> lstResultadoDeGRaficas = new List<object>();
                    lstResultadoDeGRaficas.Add(ObjDatoRealizado);

                    resultado.Add("gpxGrafica", lstResultadoDeGRaficas);
                    resultado.Add(SUCCESS, true);

                }

            }
            catch (Exception ex)
            {

            }
            return resultado;
        }
        //nnnn

        //POR CENTRO DE COSTO SUBCONTRATISTA MOSTRARIA SU AVANCE EN CUANTO A TRAYECTIRIA DE EVALUACION EN UNA GRAFICA DE TENDENCIA
        public Dictionary<string, object> ObtenerGraficaDeEvaluacionPorTendenciaMedianteEvaluaciones(SubContratistasDTO parametros)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                //AQUI ESTOY TRABAJANDO
                List<gpGraficaDTO> lstFormateada = traerResultadoMensual(/*parametros.cc, parametros.idSubContratista*/parametros.idContrato);
                var objResultado = lstFormateada.Select(y => new
                {
                    name = y.mes,
                    y = y.calificacion,
                }).ToList();
                var obj = new
                {
                    name = "Calificacion Mensual",
                    colorByPoint = true,
                    data = objResultado
                };
                List<object> lstResultado = new List<object>();
                lstResultado.Add(obj);

                resultado.Add("gpxGrafica", lstResultado);
                resultado.Add(SUCCESS, true);

            }
            catch (Exception ex)
            {

            }
            return resultado;
        }

        public List<gpGraficaDTO> traerResultadoMensual(int contratoId)
        {
            #region codigo nuevo
            var lstRetornar = new List<gpGraficaDTO>();

            var asignaciones = db.tblCO_ADP_EvalSubConAsignacion.Where(x => x.idContrato == contratoId).ToList();

            foreach (var asignacion in asignaciones)
            {
                var objRetornar = new gpGraficaDTO();
                objRetornar.mes = asignacion.fechaCreacion.ToString("dd/MM/yyyy");
                objRetornar.calificacion = obtenerPromedio(asignacion.id);
                lstRetornar.Add(objRetornar);
            }
            #endregion

            #region codigo anterior
            //List<gpGraficaDTO> lstRetornar = new List<gpGraficaDTO>();
            //gpGraficaDTO objRetornar = new gpGraficaDTO();
            //var Asignacion = db.tblCO_ADP_EvalSubConAsignacion.Where(r => r.cc == cc && r.idSubContratista == idSubcontratista).ToList();



            //for (int i = 1; i <= 12; i++)
            //{
            //    var obtenerMes = Asignacion.Where(r => r.fechaCreacion.Year == DateTime.Now.Year && r.fechaCreacion.Month == i).FirstOrDefault();
            //    if (obtenerMes != null)
            //    {
            //        objRetornar = new gpGraficaDTO();
            //        objRetornar.mes = retornarNombreMes(i);
            //        objRetornar.calificacion = obtenerPromedio(obtenerMes.id);
            //        lstRetornar.Add(objRetornar);
            //    }
            //    else
            //    {
            //        objRetornar = new gpGraficaDTO();
            //        objRetornar.mes = retornarNombreMes(i);
            //        objRetornar.calificacion = 0;
            //        lstRetornar.Add(objRetornar);
            //    }
            //}
            #endregion

            return lstRetornar;
        }
        public string retornarNombreMes(int mes)
        {
            string rt = "";
            switch (mes)
            {
                case 1:
                    rt = "ENERO";
                    break;
                case 2:
                    rt = "FEBRERO";
                    break;
                case 3:
                    rt = "MARZO";
                    break;
                case 4:
                    rt = "ABRIL";
                    break;
                case 5:
                    rt = "MAYO";
                    break;
                case 6:
                    rt = "JUNIO";
                    break;
                case 7:
                    rt = "JULIO";
                    break;
                case 8:
                    rt = "AGOSTO";
                    break;
                case 9:
                    rt = "SEPTIEMBRE";
                    break;
                case 10:
                    rt = "OCTUBRE";
                    break;
                case 11:
                    rt = "NOVIEMBRE";
                    break;
                case 12:
                    rt = "DICIEMBRE";
                    break;
            }

            return rt;
        }



        //hiiii

        #endregion
        public bool retornarValor(int idAsignacion, int tipoEvaluacion)
        {
            bool ns = false;
            var objAsignacion = db.tblCO_ADP_EvalSubContratista.Where(r => r.idSubConAsignacion == idAsignacion && r.tipoEvaluacion == tipoEvaluacion).FirstOrDefault();
            var lst = db.tblCO_ADP_EvalSubContratistaDet.Where(r => r.idEvaluacion == objAsignacion.id && r.tipoEvaluacion == tipoEvaluacion).ToList();
            int contador = 0;
            var registros = lst.Count();
            foreach (var item in lst)
            {
                contador++;
                if (item.evaluacionPendiente == false && contador <= registros)
                {
                    return ns = false;
                }
                else
                {
                    ns = true;
                }
            }
            return ns;
        }
        public Dictionary<string, object> cambiarDeColor(int idPlantilla, int idAsignacion)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                var lst = db.tblCO_ADP_EvaluacionDiv.Where(r => r.idPlantilla == idPlantilla).ToList().Select(y => new
                {
                    idButton = y.idbutton,
                    classe = retornarValor(idAsignacion, y.id) == false ? "btn-primary" : "btn-success"
                }).ToList();

                resultado.Add(ITEMS, lst);
                resultado.Add(SUCCESS, true);
                resultado.Add(MESSAGE, "CONSULTA REALIZADA CON EXITO");
            }
            catch (Exception ex)
            {
                resultado.Add(ITEMS, "");
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, ex.Message.ToString());
                throw;
            }
            return resultado;
        }
        #region CATALOGO DE NOTIFICANTES
        public Dictionary<string, object> fillComboPrestadoresDeServicio()
        {
            resultado = new Dictionary<string, object>();
            List<tblPUsuarioDTO> lstUsuariosExpedienteTodos = new List<tblPUsuarioDTO>();
            string sqlBusqueda = @"sp_getTablas";
            try
            {
                lstParametros = new DynamicParameters();
                lstParametros.Add("@tabla", " tblP_Usuario WHERE tipo=3", DbType.String);
                using (var ctx = new SqlConnection(ConextSigoDapper.conexionSUBCONTRATISTAS()))
                {
                    ctx.Open();
                    lstUsuariosExpedienteTodos = ctx.Query<tblPUsuarioDTO>(sqlBusqueda, lstParametros, null, true, 300, commandType: CommandType.StoredProcedure).ToList();
                    ctx.Close();
                }
                List<ComboDTO> lstUsuariosCombo = lstUsuariosExpedienteTodos.Select(r => new ComboDTO
                {
                    Value = r.id.ToString(),
                    Text = r.nombre_completo
                }).ToList();
                resultado.Add(ITEMS, lstUsuariosCombo);
                resultado.Add(MESSAGE, "");
                resultado.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                LogError(0, 0, NombreControlador, "fillComboPrestadoresDeServicio", ex, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(ITEMS, null);
                resultado.Add(MESSAGE, ex.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }
        public Dictionary<string, object> AccionesUsuariosExpediente(tblPUsuarioDTO objUsuario, int Accion)
        {
            resultado = new Dictionary<string, object>();
            objUsuariosExpediente = new tblPUsuarioDTO();
            lstUsuariosExpediente = new List<tblPUsuarioDTO>();
            List<tblPUsuarioDTO> lstUsuariosExpedienteTodos = new List<tblPUsuarioDTO>();
            string sql = @"sp_AddEditDeletUsuarios";
            string sqlBusqueda = @"sp_getTablas";
            try
            {
                switch (Accion)
                {
                    #region CONSULTA
                    case (int)AccionesEnum.CONSULTA:
                        lstParametros = new DynamicParameters();
                        lstParametros.Add("@tabla", "tblP_Usuario WHERE tipo=14 AND estatus=1", DbType.String);
                        using (var ctx = new SqlConnection(ConextSigoDapper.conexionSUBCONTRATISTAS()))
                        {
                            ctx.Open();
                            lstUsuariosExpediente = ctx.Query<tblPUsuarioDTO>(sqlBusqueda, lstParametros, null, true, 300, commandType: CommandType.StoredProcedure).ToList();
                            ctx.Close();
                        }
                        lstParametros = new DynamicParameters();
                        lstParametros.Add("@tabla", "tblP_Usuario WHERE tipo=3 AND estatus=1", DbType.String);
                        using (var ctx = new SqlConnection(ConextSigoDapper.conexionSUBCONTRATISTAS()))
                        {
                            ctx.Open();
                            lstUsuariosExpedienteTodos = ctx.Query<tblPUsuarioDTO>(sqlBusqueda, lstParametros, null, true, 300, commandType: CommandType.StoredProcedure).ToList();
                            ctx.Close();
                        }
                        var lstNuevaLista = lstUsuariosExpediente.Select(r => new
                        {
                            id = r.id,
                            _user = r._user,
                            nombre_completo = r.nombre_completo,
                            estatus = r.estatus,
                            _pass = r._pass,
                            correo = r.correo,
                            tipo = r.tipo,
                            idPadre = r.idPadre,
                            nombrePadre = lstUsuariosExpedienteTodos.Where(x => x.id == r.idPadre).FirstOrDefault() == null ? "" : lstUsuariosExpedienteTodos.Where(x => x.id == r.idPadre).FirstOrDefault().nombre_completo,
                            cc = r.cc,
                            idContrato = r.idContrato,
                            numeroContrato = db.tblX_Contrato.Where(x => x.id == r.idContrato).FirstOrDefault() == null ? "" : db.tblX_Contrato.Where(x => x.id == r.idContrato).FirstOrDefault().numeroContrato
                        }).ToList();

                        var lstResu = lstNuevaLista.Where(x =>
                                                            (objUsuario.cc == null ? x.cc == x.cc : x.cc == objUsuario.cc) &&
                                                            (objUsuario.idPadre == 0 ? x.idPadre == x.idPadre : x.idPadre == objUsuario.idPadre) &&
                                                            (objUsuario.idContrato == 0 ? x.idContrato == x.idContrato : x.idContrato == objUsuario.idContrato)
                            ).ToList();

                        resultado.Add(ITEMS, lstResu);
                        resultado.Add(MESSAGE, "");
                        resultado.Add(SUCCESS, true);
                        break;
                    #endregion
                    #region AGREGAR EDITAR ELIMINAR
                    default:
                        lstParametros = new DynamicParameters();
                        lstParametros.Add("@Accion", Accion, DbType.Int32);
                        lstParametros.Add("@id", objUsuario.id, DbType.Int32);
                        lstParametros.Add("@_user", objUsuario._user, DbType.String);
                        lstParametros.Add("@_pass", objUsuario._pass, DbType.String);
                        lstParametros.Add("@nombre_completo", objUsuario.nombre_completo, DbType.String);
                        lstParametros.Add("@tipo", objUsuario.tipo, DbType.Int32);
                        lstParametros.Add("@estatus", objUsuario.estatus, DbType.Boolean);
                        lstParametros.Add("@correo", objUsuario.correo, DbType.String);
                        lstParametros.Add("@idPadre", objUsuario.idPadre, DbType.Int32);
                        lstParametros.Add("@cc", objUsuario.cc, DbType.String);
                        lstParametros.Add("@contrato", objUsuario.idContrato, DbType.Int32);
                        using (var ctx = new SqlConnection(ConextSigoDapper.conexionSUBCONTRATISTAS()))
                        {
                            ctx.Open();
                            var objResultado = ctx.Query<resultadoSPDTO>(sql, lstParametros, null, true, 300, commandType: CommandType.StoredProcedure).FirstOrDefault();
                            resultado.Add(ITEMS, null);
                            resultado.Add(MESSAGE, objResultado.mensaje);
                            resultado.Add(SUCCESS, objResultado.exito);
                            ctx.Close();
                        }


                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogError(0, 0, NombreControlador, "AccionesUsuariosExpediente", ex, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(ITEMS, null);
                resultado.Add(MESSAGE, ex.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }
        #endregion
        #region COMBOS

        public Dictionary<string, object> cboProyecto()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                bool todosLosCC = false;
                string strQuery = string.Format(@"SELECT * FROM tblCO_ADP_Facultamientos WHERE idUsuario = {0} AND esActivo = {1}", (int)vSesiones.sesionUsuarioDTO.id, 1);
                List<tblCO_ADP_Facultamientos> lstUsuarioFacultamientos = _context.Select<tblCO_ADP_Facultamientos>(new DapperDTO
                {
                    baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                    consulta = strQuery
                }).ToList();
                List<string> lstCC = new List<string>();
                List<string> lstUsuarioRelCC = lstUsuarioFacultamientos.Select(s => s.cc).ToList();
                foreach (var item in lstUsuarioRelCC)
                {
                    string[] SplitCC = item.Split(',');
                    foreach (var itemCC in SplitCC)
                    {
                        if (!string.IsNullOrEmpty(itemCC) && itemCC != ",")
                        {
                            string cc = itemCC.Replace(",", "");
                            lstCC.Add(string.Format("{0}", cc));
                        }
                    }
                }

                List<tblP_CC> _lstCC = new List<tblP_CC>();
                if (!todosLosCC)
                    _lstCC = _context.tblP_CC.Where(w => lstCC.Contains(w.cc) && w.estatus).ToList();
                else
                    _lstCC = _context.tblP_CC.Where(w => w.estatus).ToList();

                List<ComboDTO> lstCCDTO = new List<ComboDTO>();
                foreach (var item in _lstCC)
                {
                    ComboDTO obj = new ComboDTO();
                    obj.Value = !string.IsNullOrEmpty(item.cc) ? item.cc.Trim().ToUpper() : string.Empty;
                    obj.Text = !string.IsNullOrEmpty(item.cc) && !string.IsNullOrEmpty(item.descripcion) ? string.Format("[{0}] {1}", item.cc.Trim().ToUpper(), item.descripcion.Trim().ToUpper()) : string.Empty;
                    lstCCDTO.Add(obj);
                }

                resultado.Add(ITEMS, lstCCDTO.OrderBy(o => o.Value));
                resultado.Add(MESSAGE, "");
                resultado.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                LogError(0, 0, NombreControlador, "cboProyecto", ex, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(ITEMS, null);
                resultado.Add(MESSAGE, ex.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboProyectosFacultamientos()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region SE OBTIENE LOS CC ASIGNADOS AL USUARIO LOGUEADO
                string strQuery = string.Format(@"SELECT * FROM tblCO_ADP_Facultamientos WHERE idUsuario = {0} AND esActivo = {1}", (int)vSesiones.sesionUsuarioDTO.id, 1);
                List<tblCO_ADP_Facultamientos> lstUsuarioFacultamientos = _context.Select<tblCO_ADP_Facultamientos>(new DapperDTO
                {
                    baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                    consulta = strQuery
                }).ToList();
                List<string> lstCC_Facultamiento = new List<string>();
                List<string> lstUsuarioRelCC = lstUsuarioFacultamientos.Select(s => s.cc).ToList();
                foreach (var item in lstUsuarioRelCC)
                {
                    string[] SplitCC = item.Split(',');
                    foreach (var itemCC in SplitCC)
                    {
                        if (!string.IsNullOrEmpty(itemCC) && itemCC != ",")
                        {
                            string cc = itemCC.Replace(",", "");
                            lstCC_Facultamiento.Add(string.Format("'{0}'", cc));
                        }
                    }
                }
                #endregion

                strQuery = string.Format(@"SELECT id, cc, descripcion FROM tblP_CC WHERE cc IN ({0}) AND estatus = 1", string.Join(",", lstCC_Facultamiento));
                List<tblP_CC> lstCC = _context.Select<tblP_CC>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Construplan,
                    consulta = strQuery,
                    parametros = new { estatus = true }
                }).ToList();

                List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                foreach (var item in lstCC)
                {
                    ComboDTO obj = new ComboDTO();
                    obj.Value = !string.IsNullOrEmpty(item.cc) ? item.cc.Trim().ToUpper() : string.Empty;
                    obj.Text = !string.IsNullOrEmpty(item.cc) && !string.IsNullOrEmpty(item.descripcion) ? string.Format("[{0}] {1}", item.cc.Trim().ToUpper(), item.descripcion.Trim().ToUpper()) : string.Empty;
                    lstComboDTO.Add(obj);
                }

                resultado.Add(ITEMS, lstComboDTO);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "FillCboProyectosFacultamientos", e, AccionEnum.FILLCOMBO, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> cboProyecto3(int idSubcontratista)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                tblPUsuarioDTO objUsuario = new tblPUsuarioDTO();
                string sql = @"SELECT * FROM tblP_Usuario WHERE tipo=3 AND estatus='true' AND id=" + idSubcontratista;
                using (var ctx = new SqlConnection(ConextSigoDapper.conexionSUBCONTRATISTAS()))
                {
                    ctx.Open();
                    objUsuario = ctx.Query<tblPUsuarioDTO>(sql, null, null, true, 300).FirstOrDefault();
                    ctx.Close();
                }
                if (objUsuario != null)
                {
                    var objSubcontratista = db.tblX_SubContratista.Where(r => r.rfc == objUsuario._user).FirstOrDefault();
                    if (objSubcontratista != null)
                    {
                        var lst = db.tblX_Contrato.Where(r => r.subcontratistaID == objSubcontratista.id).ToList().Select(y => y.cc).ToList();
                        var lstCC = _context.tblP_CC.Where(r => r.estatus && lst.Contains(r.cc)).ToList().Select(y => new ComboDTO
                        {
                            Text = y.cc + " - " + y.descripcion,
                            Value = y.cc
                        }).ToList();
                        resultado.Add(ITEMS, lstCC);
                        resultado.Add(MESSAGE, "");
                        resultado.Add(SUCCESS, true);
                    }
                    else
                    {
                        resultado.Add(ITEMS, null);
                        resultado.Add(MESSAGE, "");
                        resultado.Add(SUCCESS, false);
                    }
                }
                else
                {
                    resultado.Add(ITEMS, null);
                    resultado.Add(MESSAGE, "");
                    resultado.Add(SUCCESS, false);
                }
            }
            catch (Exception ex)
            {
                LogError(0, 0, NombreControlador, "cboProyecto", ex, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(ITEMS, null);
                resultado.Add(MESSAGE, ex.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }
        public Dictionary<string, object> cboContratosBuscar3(int idSubcontratista)
        {
            resultado = new Dictionary<string, object>();
            try
            {

                tblPUsuarioDTO objUsuario = new tblPUsuarioDTO();
                string sql = @"SELECT * FROM tblP_Usuario WHERE tipo=3 AND estatus='true' AND id=" + idSubcontratista;
                using (var ctx = new SqlConnection(ConextSigoDapper.conexionSUBCONTRATISTAS()))
                {
                    ctx.Open();
                    objUsuario = ctx.Query<tblPUsuarioDTO>(sql, null, null, true, 300).FirstOrDefault();
                    ctx.Close();
                }
                if (objUsuario != null)
                {
                    var objSubcontratista = db.tblX_SubContratista.Where(r => r.rfc == objUsuario._user).FirstOrDefault();
                    if (objSubcontratista != null)
                    {
                        var lstcboElementos = db.tblX_Contrato.Where(r => r.estatus == true && r.subcontratistaID == objSubcontratista.id).ToList().Select(y => new ComboDTO
                        {
                            Text = y.cc + " - " + y.numeroContrato,
                            Value = y.id.ToString()
                        }).ToList();
                        resultado.Add(ITEMS, lstcboElementos);
                        resultado.Add(MESSAGE, "");
                        resultado.Add(SUCCESS, true);

                    }
                    else
                    {
                        resultado.Add(ITEMS, null);
                        resultado.Add(MESSAGE, "");
                        resultado.Add(SUCCESS, false);
                    }
                }
                else
                {
                    resultado.Add(ITEMS, null);
                    resultado.Add(MESSAGE, "");
                    resultado.Add(SUCCESS, false);
                }

            }
            catch (Exception ex)
            {
                LogError(0, 0, NombreControlador, "cboContratosBuscar", ex, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(ITEMS, null);
                resultado.Add(MESSAGE, ex.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }
        public Dictionary<string, object> cboEvaluador()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VERSIÓN ADAN
                //List<tblPUsuarioDTO> lstUsurio = new List<tblPUsuarioDTO>();
                //string sql = @"SELECT * FROM tblP_Usuario WHERE tipo=10 AND estatus='true'";
                //using (var ctx = new SqlConnection(ConextSigoDapper.conexionSUBCONTRATISTAS()))
                //{
                //    ctx.Open();
                //    lstUsurio = ctx.Query<tblPUsuarioDTO>(sql, null, null, true, 300).ToList();
                //    ctx.Close();
                //}
                //var lstCombo = lstUsurio.Select(y => new ComboDTO
                //{
                //    Value = y.id.ToString(),
                //    Text = y.nombre_completo,

                //}).ToList();
                //resultado.Add(ITEMS, lstCombo);
                //resultado.Add(MESSAGE, "");
                //resultado.Add(SUCCESS, true);
                #endregion

                #region VERSIÓN v1
                string strQuery = string.Format("SELECT * FROM tblP_Usuario WHERE estatus = {0}", 1);
                List<tblP_Usuario> lstUsuarios = _context.Select<tblP_Usuario>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = strQuery,
                }).ToList();

                List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                foreach (var item in lstUsuarios)
                {
                    ComboDTO obj = new ComboDTO();
                    obj.Value = item.id.ToString();

                    string nombreCompleto = string.Empty;
                    if (!string.IsNullOrEmpty(item.nombre))
                        nombreCompleto += item.nombre.Trim().ToUpper();
                    if (!string.IsNullOrEmpty(item.apellidoPaterno))
                        nombreCompleto += !string.IsNullOrEmpty(item.nombre) ? " " + item.apellidoPaterno.Trim().ToUpper() : item.apellidoPaterno.Trim().ToUpper();
                    if (!string.IsNullOrEmpty(item.apellidoMaterno))
                        nombreCompleto += !string.IsNullOrEmpty(item.apellidoPaterno) ? " " + item.apellidoMaterno.Trim().ToUpper() : item.apellidoMaterno.Trim().ToUpper();

                    obj.Text = nombreCompleto;
                    lstComboDTO.Add(obj);
                }

                resultado.Add(ITEMS, lstComboDTO);
                resultado.Add(SUCCESS, true);
                #endregion
            }
            catch (Exception ex)
            {
                LogError(0, 0, NombreControlador, "cboEvaluador", ex, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(ITEMS, null);
                resultado.Add(MESSAGE, ex.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> cboSubcontratistas()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                List<tblPUsuarioDTO> lstUsurio = new List<tblPUsuarioDTO>();
                string sql = @"SELECT * FROM tblP_Usuario WHERE tipo=3 AND estatus='true'";
                using (var ctx = new SqlConnection(ConextSigoDapper.conexionSUBCONTRATISTAS()))
                {
                    ctx.Open();
                    lstUsurio = ctx.Query<tblPUsuarioDTO>(sql, null, null, true, 300).ToList();
                    ctx.Close();
                }
                var lstCombo = lstUsurio.Select(y => new ComboDTO
                {
                    Value = y.id.ToString(),
                    Text = y.nombre_completo,

                }).ToList();
                resultado.Add(ITEMS, lstCombo);
                resultado.Add(MESSAGE, "");
                resultado.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                LogError(0, 0, NombreControlador, "cboEvaluador", ex, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(ITEMS, null);
                resultado.Add(MESSAGE, ex.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> cboElementos()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                var lstcboElementos = db.tblCO_ADP_EvaluacionDiv.Where(r => r.SubContratista == false).ToList().Select(y => new ComboDTO
                {
                    Text = nombrePlantilla(y.idPlantilla) + " - " + y.id + " - " + y.descripcion,
                    Value = y.id.ToString()
                }).ToList();
                resultado.Add(ITEMS, lstcboElementos);
                resultado.Add(MESSAGE, "");
                resultado.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                LogError(0, 0, NombreControlador, "cboElementos", ex, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(ITEMS, null);
                resultado.Add(MESSAGE, ex.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public string nombrePlantilla(int idPlantilla)
        {
            string nombre = "";
            nombre = db.tblCO_ADP_EvaluacionPlantilla.Where(r => r.id == idPlantilla).FirstOrDefault().nombrePlantilla;
            return nombre;
        }

        public Dictionary<string, object> FillComboPlantillas()
        {
            resultado = new Dictionary<string, object>();

            try
            {
                var listaPlantillas = db.tblCOES_Plantilla.Where(x => x.registroActivo).ToList().Select(x => new ComboDTO
                {
                    Value = x.id.ToString(),
                    Text = x.nombre
                }).ToList();
                resultado.Add(ITEMS, listaPlantillas);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                LogError(0, 0, NombreControlador, "FillComboPlantillas", ex, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(MESSAGE, ex.Message);
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }

        public Dictionary<string, object> FillComboProyectos()
        {
            resultado = new Dictionary<string, object>();

            try
            {
                var facultamiento = db.tblCOES_Facultamiento.FirstOrDefault(x => x.registroActivo && x.usuario_id == vSesiones.sesionUsuarioDTO.id);
                var listaProyectos = new List<ComboDTO>();

                if (facultamiento != null)
                {
                    listaProyectos = _context.tblP_CC.Where(x => x.estatus).ToList().Select(x => new ComboDTO
                    {
                        Value = x.cc,
                        Text = string.Format(@"[{0}] {1}", x.cc, x.descripcion)
                    }).OrderBy(x => x.Text).ToList();

                    if (facultamiento.tipo != TipoFacultamientoEnum.ADMINISTRADOR_PMO) //Los administradores PMO ven todos los centros de costos.
                    {
                        var listaCentroCostoFacultamiento = db.tblCOES_Facultamiento_CentroCosto.Where(x => x.registroActivo && x.facultamiento_id == facultamiento.id).Select(x => x.cc).ToList();

                        listaProyectos = listaProyectos.Where(x => listaCentroCostoFacultamiento.Contains(x.Value)).ToList();
                    }
                }

                resultado.Add(ITEMS, listaProyectos);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                LogError(0, 0, NombreControlador, "FillComboProyectos", ex, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(MESSAGE, ex.Message);
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }

        public Dictionary<string, object> cboContratosBuscar()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                var lstcboElementos = db.tblX_Contrato.Where(r => r.estatus == true).ToList().Select(y => new ComboDTO
                {
                    Text = y.cc + " - " + y.numeroContrato,
                    Value = y.id.ToString()
                }).ToList();
                resultado.Add(ITEMS, lstcboElementos);
                resultado.Add(MESSAGE, "");
                resultado.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                LogError(0, 0, NombreControlador, "cboContratosBuscar", ex, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(ITEMS, null);
                resultado.Add(MESSAGE, ex.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        #endregion
        public Dictionary<string, object> AccionesFacultamientos(facultamientosCODTO objUsuario, int Accion)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                if (objUsuario.lstcc != null)
                {
                    if (objUsuario.lstcc.Count() > 0)
                        objUsuario.lstcc.Remove("Todos");
                }

                var objRegistro = db.tblCO_ADP_Facultamientos.Where(r => r.id == objUsuario.id).FirstOrDefault();

                tblP_MenutblP_Usuario objRelMenu = new tblP_MenutblP_Usuario();
                switch (Accion)
                {
                    #region CONSULTA
                    case (int)AccionesEnum.CONSULTA:

                        #region VALIDACIONES
                        if (objUsuario.cc == null) throw new Exception("Es necesario seleccionar un proyecto.");
                        #endregion

                        List<tblP_Usuario> tblP_Usuario = _context.Select<tblP_Usuario>(new DapperDTO
                        {
                            baseDatos = MainContextEnum.Construplan,
                            consulta = @"SELECT * FROM tblP_Usuario WHERE estatus = 1"
                        }).ToList();

                        var lst = db.tblCO_ADP_Facultamientos.Where(r => r.esActivo).ToList().Select(y => new facultamientosCODTO
                        {
                            id = y.id,
                            tipo = y.tipo,
                            tipoNombre = regresarNombretipo(y.tipo),
                            lstcc = obtenerlst(y.cc),
                            ccProyu = !string.IsNullOrEmpty(y.cc) ? obtenerFormatoLista(y.cc) : null,
                            usuarioCreacion = y.usuarioCreacion,
                            fechaCrecion = y.fechaCrecion,
                            usuarioModificacion = y.usuarioModificacion,
                            fechaModificacion = y.fechaModificacion,
                            esActivo = y.esActivo,
                            idUsuario = y.idUsuario,
                            nombreUsuario = tblP_Usuario.Where(r => r.id == y.idUsuario).FirstOrDefault() == null ? "" :
                                            tblP_Usuario.Where(r => r.id == y.idUsuario).Select(n => n.nombre + " " + n.apellidoPaterno + " " + n.apellidoMaterno).FirstOrDefault()
                        }).ToList();

                        List<facultamientosCODTO> lstResulta = new List<facultamientosCODTO>();
                        int tipoUsuario = GetTipoFacultamientosUsuarioLogueado();

                        if (tipoUsuario == (int)TipoUsuariosEnum.administradorPMO)
                            lstResulta = lst.Where(r => (objUsuario.cc == null ? r.cc == r.cc : r.ccProyu.Contains(objUsuario.cc))).ToList();
                        else if (tipoUsuario == (int)TipoUsuariosEnum.administrador)
                            lstResulta = lst.Where(w => (objUsuario.cc == null ? w.cc == w.cc : w.ccProyu.Contains(objUsuario.cc)) && (w.tipo == (int)TipoUsuariosEnum.consulta || w.tipo == (int)TipoUsuariosEnum.evaluador)).ToList();

                        resultado.Add(ITEMS, lstResulta);
                        resultado.Add(MESSAGE, "");
                        resultado.Add(SUCCESS, true);
                        break;
                    #endregion
                    #region AGREGAR
                    case (int)AccionesEnum.AGREGAR:
                        if (objRegistro == null)
                        {
                            objRegistro = new tblCO_ADP_Facultamientos();
                            objRegistro.idUsuario = objUsuario.idUsuario;
                            objRegistro.tipo = objUsuario.tipo;
                            objRegistro.cc = objUsuario.cc;
                            objRegistro.usuarioCreacion = (string)vSesiones.sesionUsuarioDTO.nombreUsuario;
                            objRegistro.fechaCrecion = DateTime.Now;
                            objRegistro.esActivo = true;
                            db.tblCO_ADP_Facultamientos.Add(objRegistro);
                            db.SaveChanges();
                            resultado.Add(SUCCESS, true);
                            resultado.Add(MESSAGE, "Se ha registrado con éxito.");
                        }
                        break;
                    #endregion
                    #region MODIFICAR
                    case (int)AccionesEnum.EDITAR:
                        if (objRegistro != null)
                        {
                            objRegistro.idUsuario = objUsuario.idUsuario;
                            objRegistro.cc = objUsuario.cc;
                            objRegistro.tipo = objUsuario.tipo;
                            objRegistro.usuarioModificacion = (string)vSesiones.sesionUsuarioDTO.nombreUsuario;
                            objRegistro.fechaModificacion = DateTime.Now;
                            db.SaveChanges();
                            resultado.Add(SUCCESS, true);
                            resultado.Add(MESSAGE, "Se ha actualizado con éxito.");
                        }
                        break;
                    #endregion
                    #region ELIMINAR
                    case (int)AccionesEnum.ELIMINAR:
                        if (objRegistro != null)
                        {
                            objRegistro.esActivo = objUsuario.esActivo;
                            db.SaveChanges();
                            resultado.Add(ITEMS, null);
                            resultado.Add(SUCCESS, true);
                            resultado.Add(MESSAGE, "Se ha eliminado con éxito.");
                        }
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogError(0, 0, NombreControlador, "AccionesFacultamientos", ex, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(ITEMS, null);
                resultado.Add(MESSAGE, ex.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public string regresarNombretipo(int tipo)
        {
            string nombre = "";
            switch (tipo)
            {
                case (int)tipoFacultamiento.ADMINISTRADORPMO:
                    nombre = EnumHelper.GetDescription((tipoFacultamiento)tipo);
                    break;
                case (int)tipoFacultamiento.ADMINISTRADOR:
                    nombre = EnumHelper.GetDescription((tipoFacultamiento)tipo);
                    break;
                case (int)tipoFacultamiento.EVALUADOR:
                    nombre = EnumHelper.GetDescription((tipoFacultamiento)tipo);
                    break;
                case (int)tipoFacultamiento.CONSULTA:
                    nombre = EnumHelper.GetDescription((tipoFacultamiento)tipo);
                    break;
            }
            return nombre;
        }

        public Dictionary<string, object> cboUsuarios()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                List<ComboDTO> lst = _context.tblP_Usuario.Where(r => r.estatus).ToList().Select(s => new ComboDTO
                {
                    Value = s.id.ToString(),
                    Text = string.Format("{0}{1}{2}{3}{4}",
                        !string.IsNullOrEmpty(s.nombre) ? s.nombre.Trim().ToUpper() : "", " ",
                        !string.IsNullOrEmpty(s.apellidoPaterno) ? s.apellidoPaterno.Trim().ToUpper() : "", " ",
                        !string.IsNullOrEmpty(s.apellidoMaterno) ? s.apellidoMaterno.Trim().ToUpper() : "")
                }).ToList();

                resultado.Add(ITEMS, lst);
                resultado.Add(MESSAGE, "");
                resultado.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                LogError(0, 0, NombreControlador, "cboUsuarios", ex, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(ITEMS, null);
                resultado.Add(MESSAGE, ex.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> obtenerPromedioGeneral(int id)
        {
            resultado = new Dictionary<string, object>();
            resultado.Add(ITEMS, Math.Round(obtenerPromedio(id), 2));
            resultado.Add(SUCCESS, true);
            return resultado;
        }

        public Respuesta PermisoVista(int vistaID)
        {
            Respuesta respuesta = new Respuesta();

            try
            {
                var facultamientoUsuario = db.tblCOES_Facultamiento.FirstOrDefault(x => x.registroActivo && x.usuario_id == vSesiones.sesionUsuarioDTO.id);

                if (facultamientoUsuario == null)
                {
                    throw new Exception("Este usuario no cuenta con facultamientos, ponerse en contacto con el Administrador PMO.");
                }

                bool tienePermiso = false;

                switch ((VistasEnum)vistaID)
                {
                    case VistasEnum.administracionEvaluacion:
                        tienePermiso = (facultamientoUsuario.tipo == TipoFacultamientoEnum.ADMINISTRADOR_PMO || facultamientoUsuario.tipo == TipoFacultamientoEnum.ADMINISTRADOR || facultamientoUsuario.tipo == TipoFacultamientoEnum.EVALUADOR || facultamientoUsuario.tipo == TipoFacultamientoEnum.CONSULTA);
                        break;
                    case VistasEnum.dashboard:
                        tienePermiso = (facultamientoUsuario.tipo == TipoFacultamientoEnum.ADMINISTRADOR_PMO || facultamientoUsuario.tipo == TipoFacultamientoEnum.ADMINISTRADOR || facultamientoUsuario.tipo == TipoFacultamientoEnum.EVALUADOR || facultamientoUsuario.tipo == TipoFacultamientoEnum.CONSULTA);
                        break;
                    case VistasEnum.catalogos:
                        tienePermiso = (facultamientoUsuario.tipo == TipoFacultamientoEnum.ADMINISTRADOR_PMO || facultamientoUsuario.tipo == TipoFacultamientoEnum.ADMINISTRADOR);
                        break;
                    case VistasEnum.calendario:
                        tienePermiso = (facultamientoUsuario.tipo == TipoFacultamientoEnum.ADMINISTRADOR_PMO || facultamientoUsuario.tipo == TipoFacultamientoEnum.ADMINISTRADOR || facultamientoUsuario.tipo == TipoFacultamientoEnum.EVALUADOR || facultamientoUsuario.tipo == TipoFacultamientoEnum.CONSULTA);
                        break;
                }

                respuesta.Success = tienePermiso;
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "PermisoVistas", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }

            return respuesta;
        }

        public Dictionary<string, object> VerificarTipoUsuario()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region SE OBTIENE QUE TIPO DE USUARIO TIENE EL USUARIO LOGUEADO
                int idUsuario = (int)vSesiones.sesionUsuarioDTO.id;
                tblCO_ADP_Facultamientos objTipoUsuario = db.tblCO_ADP_Facultamientos.Where(w => w.idUsuario == idUsuario && w.esActivo).FirstOrDefault();
                if (objTipoUsuario == null)
                    throw new Exception("Ocurrió un error al obtener que tipo de usuario es el logueado.");

                resultado.Add("objTipoUsuario", objTipoUsuario);
                resultado.Add(SUCCESS, true);
                #endregion
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboEstados()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                List<tblP_Estado> lstEstados = _context.tblP_Estado.OrderBy(o => o.Estado).ToList();
                List<ComboDTO> lstEstadosDTO = new List<ComboDTO>();
                foreach (var item in lstEstados)
                {
                    ComboDTO obj = new ComboDTO();
                    obj.Value = item.idEstado.ToString();
                    obj.Text = !string.IsNullOrEmpty(item.Estado) ? item.Estado.Trim().ToUpper() : string.Empty;
                    lstEstadosDTO.Add(obj);
                }
                resultado.Add(ITEMS, lstEstadosDTO);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "FillCboEstados", e, AccionEnum.FILLCOMBO, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboMunicipios(int idEstado)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                List<tblP_Municipio> lstMunicipios = _context.tblP_Municipio.Where(w => idEstado > 0 ? w.idEstado == idEstado : true).OrderBy(o => o.Municipio).ToList();
                List<ComboDTO> lstMunicipiosDTO = new List<ComboDTO>();
                foreach (var item in lstMunicipios)
                {
                    ComboDTO obj = new ComboDTO();
                    obj.Value = item.idMunicipio.ToString();
                    obj.Text = !string.IsNullOrEmpty(item.Municipio) ? item.Municipio.Trim().ToUpper() : string.Empty;
                    lstMunicipiosDTO.Add(obj);
                }
                resultado.Add(ITEMS, lstMunicipiosDTO);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "FillCboMunicipios", e, AccionEnum.FILLCOMBO, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }
        #endregion

        #region ADMINISTRACIÓN DE EVALUACIONES
        public Dictionary<string, object> VerificarElementoTerminado(EvaluacionDTO objDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                // SE OBTIENE LOS ELEMENTOS
                string strQuery = string.Format("SELECT id FROM tblCO_ADP_EvaluacionReq WHERE idDiv = {0}", objDTO.idDiv);
                List<int> lstElementos = _context.Select<int>(new DapperDTO
                {
                    baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                    consulta = strQuery
                }).ToList();

                // SE OBTIENES LAS CALIFICACIONES DE LOS ELEMENTOS
                strQuery = string.Format("SELECT calificacion FROM tblCO_ADP_EvalSubContratistaDet WHERE idEvaluacion = {0} AND tipoEvaluacion IN ({1})", objDTO.idEvaluacion, string.Join(",", lstElementos));
                List<tblCO_ADP_EvalSubContratistaDet> lstCalificaciones = _context.Select<tblCO_ADP_EvalSubContratistaDet>(new DapperDTO
                {
                    baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                    consulta = strQuery
                }).ToList();

                int cantEvaluaciones = lstElementos.Count();
                int cantEvaluacionesRealizadas = lstCalificaciones.Where(w => w.calificacion > 0).Count();
                int cantEvaluacionesNoRealizadas = lstCalificaciones.Where(w => w.calificacion <= 0).Count();

                if (cantEvaluacionesNoRealizadas >= 1)
                {
                    resultado.Add(SUCCESS, false);
                }
                else if (cantEvaluacionesRealizadas == cantEvaluaciones)
                {
                    resultado.Add(SUCCESS, true);
                }
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "VerificarElementoTerminado", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }
        #endregion

        #region CATÁLOGO DE EVALUADORES
        public Dictionary<string, object> FillCboFiltroEvaluadores()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                List<tblCO_ADP_Facultamientos> lstCC = GetListadoCCRelUsuario();
                List<tblP_Usuario> lstUsuariosSIGOPLAN = GetListadoUsuariosSIGOPLAN();

                string strQuery = string.Format("SELECT * FROM tblCO_ADP_EvaluadorXcc WHERE cc IN ({0})", string.Join(",", lstCC.Select(s => s.cc)));
                List<tblCO_ADP_EvaluadorXcc> lstEvaluadores = _context.Select<tblCO_ADP_EvaluadorXcc>(new DapperDTO
                {
                    baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                    consulta = strQuery
                }).ToList();

                List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                ComboDTO objComboDTO = new ComboDTO();
                foreach (var item in lstEvaluadores)
                {
                    tblP_Usuario objUsuario = lstUsuariosSIGOPLAN.Where(w => w.id == item.evaluador).FirstOrDefault();
                    string nombre = string.Empty;
                    string apellidoPaterno = string.Empty;
                    string apellidoMaterno = string.Empty;

                    objComboDTO = new ComboDTO();
                    objComboDTO.Value = item.id.ToString();

                    string nombreCompleto = string.Empty;
                    if (!string.IsNullOrEmpty(objUsuario.nombre))
                        nombreCompleto += objUsuario.nombre.Trim().ToUpper();
                    if (!string.IsNullOrEmpty(objUsuario.apellidoPaterno))
                        nombreCompleto += !string.IsNullOrEmpty(objUsuario.nombre) ? " " + objUsuario.apellidoPaterno.Trim().ToUpper() : objUsuario.apellidoPaterno.Trim().ToUpper();
                    if (!string.IsNullOrEmpty(objUsuario.apellidoMaterno))
                        nombreCompleto += !string.IsNullOrEmpty(objUsuario.apellidoPaterno) ? " " + objUsuario.apellidoMaterno.Trim().ToUpper() : objUsuario.apellidoMaterno.Trim().ToUpper();

                    objComboDTO.Text = nombreCompleto;
                    lstComboDTO.Add(objComboDTO);
                }

                resultado.Add(ITEMS, lstComboDTO);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                LogError(0, 0, NombreControlador, "FillCboFiltroEvaluadores", ex, AccionEnum.FILLCOMBO, 0, 0);
                resultado.Add(ITEMS, null);
                resultado.Add(MESSAGE, ex.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboCEEvaluadores()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                List<tblCO_ADP_Facultamientos> lstCC = GetListadoCCRelUsuario();
                List<tblP_Usuario> lstUsuariosSIGOPLAN = GetListadoUsuariosSIGOPLAN();

                // SE OBTIENE A LOS EVALUADORES ACTUALES DEL CC DEL USUARIO LOGUEADO
                string strQuery = string.Format("SELECT evaluador FROM tblCO_ADP_EvaluadorXcc WHERE cc IN ({0}) AND esActivo = 1", string.Join(",", lstCC.Select(s => s.cc)));
                List<int> lstEvaluadores = _context.Select<int>(new DapperDTO
                {
                    baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                    consulta = strQuery
                }).ToList();

                // SE QUITA DE LA LISTA DE USUARIOS A LOS USUARIOS QUE YA SE ENCUENTRAN REGISTRADOS COMO EVALUADORES
                List<tblP_Usuario> lstUsuarios = lstUsuariosSIGOPLAN.Where(w => !lstEvaluadores.Contains(w.id)).ToList();

                // SE OBTIENE A LOS USUARIOS CON FACULTAMIENTOS DEL USUARIO LOGUEADO
                string whereCC = string.Empty;
                foreach (var item in lstCC)
                {
                    if (string.IsNullOrEmpty(whereCC))
                        whereCC = string.Format("cc LIKE '%{0}%'", item.cc);
                    else
                        whereCC += string.Format("OR cc LIKE '%{0}%'", item.cc);
                }
                strQuery = string.Format("SELECT idUsuario FROM tblCO_ADP_Facultamientos WHERE ({0}) AND esActivo = 1 AND (tipo = {1} OR tipo = {2})", whereCC, (int)TipoUsuariosEnum.consulta, (int)TipoUsuariosEnum.evaluador);
                List<int> lstUsuariosRelCCFacultamientos = _context.Select<int>(new DapperDTO
                {
                    baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                    consulta = strQuery
                }).ToList();

                // SE OBTIENE LOS USUARIOS CON EL CC QUE TENGA EL USUARIO LOGUEADO
                lstUsuarios = lstUsuarios.Where(w => lstUsuariosRelCCFacultamientos.Contains(w.id)).ToList();

                List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                ComboDTO objComboDTO = new ComboDTO();
                foreach (var item in lstUsuarios)
                {
                    objComboDTO = new ComboDTO();
                    objComboDTO.Value = item.id.ToString();

                    string nombreCompleto = string.Empty;
                    if (!string.IsNullOrEmpty(item.nombre))
                        nombreCompleto += item.nombre.Trim().ToUpper();
                    if (!string.IsNullOrEmpty(item.apellidoPaterno))
                        nombreCompleto += !string.IsNullOrEmpty(item.nombre) ? " " + item.apellidoPaterno.Trim().ToUpper() : item.apellidoPaterno.Trim().ToUpper();
                    if (!string.IsNullOrEmpty(item.apellidoMaterno))
                        nombreCompleto += !string.IsNullOrEmpty(item.apellidoPaterno) ? " " + item.apellidoMaterno.Trim().ToUpper() : item.apellidoMaterno.Trim().ToUpper();

                    objComboDTO.Text = nombreCompleto;
                    lstComboDTO.Add(objComboDTO);
                }

                resultado.Add(ITEMS, lstComboDTO);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                LogError(0, 0, NombreControlador, "FillCboCEEvaluadores", ex, AccionEnum.FILLCOMBO, 0, 0);
                resultado.Add(ITEMS, null);
                resultado.Add(MESSAGE, ex.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }
        #endregion

        #region CATALOGOS DE FACULTAMIENTOS
        public Dictionary<string, object> GetListadoUsuarioRelCC(int id)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region SE OBTIENE LOS CC QUE CONTENGA EL USUARIO, REGISTRADOS
                string strUsuarioRelCC = _context.Select<string>(new DapperDTO
                {
                    baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                    consulta = @"SELECT cc FROM tblCO_ADP_Facultamientos WHERE esActivo = 1 AND id = @id",
                    parametros = new { id = id }
                }).FirstOrDefault();

                string[] SplitCC = strUsuarioRelCC.Split(',');
                List<string> lstUsuarioRelCC = new List<string>();
                foreach (var item in SplitCC)
                {
                    if (item != "," && item != "0" && item != "032")
                        lstUsuarioRelCC.Add(string.Format("'{0}'", item));
                }
                #endregion

                #region SE OBTIENE LOS CC QUE TENGA RELACIONADOS EL USUARIO
                string strQuery = string.Format(@"SELECT cc, descripcion FROM tblP_CC WHERE estatus = {0} AND cc IN ({1})", 1, string.Join(",", lstUsuarioRelCC));
                List<tblP_CC> lstCC = _context.Select<tblP_CC>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Construplan,
                    consulta = strQuery,
                    parametros = new { estatus = true, lstCC = string.Join(",", lstUsuarioRelCC) }
                }).ToList();

                List<CCDTO> lstCCDTO = new List<CCDTO>();
                foreach (var item in lstCC)
                {
                    CCDTO objDTO = new CCDTO();
                    if (!string.IsNullOrEmpty(item.cc) && !string.IsNullOrEmpty(item.descripcion))
                    {
                        objDTO.descripcion = string.Format("[{0}] {1}", item.cc.Trim().ToUpper(), item.descripcion.Trim().ToUpper());
                        lstCCDTO.Add(objDTO);
                    }
                }
                #endregion

                resultado.Add("lstCC", lstCCDTO);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "GetListadoUsuarioRelCC", e, AccionEnum.CONSULTA, id, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> GetCCActualizarFacultamiento(int id)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region SE OBTIENE LOS CC QUE CONTENGA EL USUARIO, REGISTRADOS
                string strUsuarioRelCC = _context.Select<string>(new DapperDTO
                {
                    baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                    consulta = @"SELECT cc FROM tblCO_ADP_Facultamientos WHERE esActivo = 1 AND id = @id",
                    parametros = new { id = id }
                }).FirstOrDefault();

                string[] SplitCC = strUsuarioRelCC.Split(',');
                List<string> lstUsuarioRelCC = new List<string>();
                foreach (var item in SplitCC)
                {
                    if (item != "," && item != "0" && item != "032")
                        lstUsuarioRelCC.Add(string.Format("'{0}'", item));
                }
                #endregion

                #region SE OBTIENE LOS CC QUE TENGA RELACIONADOS EL USUARIO
                string strQuery = string.Format(@"SELECT cc, descripcion FROM tblP_CC WHERE estatus = {0} AND cc IN ({1})", 1, string.Join(",", lstUsuarioRelCC));
                List<tblP_CC> lstCC = _context.Select<tblP_CC>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Construplan,
                    consulta = strQuery,
                    parametros = new { estatus = true, lstCC = string.Join(",", lstUsuarioRelCC) }
                }).ToList();

                List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                ComboDTO objComboDTO = new ComboDTO();
                foreach (var item in lstCC)
                {
                    objComboDTO = new ComboDTO();
                    if (!string.IsNullOrEmpty(item.cc) && !string.IsNullOrEmpty(item.descripcion))
                    {
                        objComboDTO.Value = item.cc;
                        objComboDTO.Text = string.Format("[{0}] {1}", item.cc.Trim().ToUpper(), item.descripcion.Trim().ToUpper());
                        lstComboDTO.Add(objComboDTO);
                    }
                }
                #endregion

                resultado.Add(ITEMS, lstComboDTO);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "GetCCActualizarFacultamiento", e, AccionEnum.FILLCOMBO, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> FillTipoUsuarioFacultamientos()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region FILL COMBO TIPO USUARIOS FACULTAMIENTOS
                // SE OBTIENE EL TIPO DE FACULTAMIENTO DEL USUARIO LOGUEADO
                int tipoUsuarioFacultamiento = GetTipoFacultamientosUsuarioLogueado();
                List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                ComboDTO objDTO = new ComboDTO();
                int tipoUsuario = 0;

                if (tipoUsuarioFacultamiento == (int)TipoUsuariosEnum.administradorPMO)
                {
                    // ADMINISTRADOR PMO
                    tipoUsuario = (int)TipoUsuariosEnum.administradorPMO;
                    objDTO = new ComboDTO();
                    objDTO.Value = tipoUsuario.ToString();
                    objDTO.Text = EnumHelper.GetDescription((TipoUsuariosEnum.administradorPMO));
                    lstComboDTO.Add(objDTO);

                    // ADMINISTRADOR
                    tipoUsuario = (int)TipoUsuariosEnum.administrador;
                    objDTO = new ComboDTO();
                    objDTO.Value = tipoUsuario.ToString();
                    objDTO.Text = EnumHelper.GetDescription((TipoUsuariosEnum.administrador));
                    lstComboDTO.Add(objDTO);
                }

                // EVALUADOR
                tipoUsuario = (int)TipoUsuariosEnum.evaluador;
                objDTO = new ComboDTO();
                objDTO.Value = tipoUsuario.ToString();
                objDTO.Text = EnumHelper.GetDescription((TipoUsuariosEnum.evaluador));
                lstComboDTO.Add(objDTO);

                // CONSULTA
                tipoUsuario = (int)TipoUsuariosEnum.consulta;
                objDTO = new ComboDTO();
                objDTO.Value = tipoUsuario.ToString();
                objDTO.Text = EnumHelper.GetDescription((TipoUsuariosEnum.consulta));
                lstComboDTO.Add(objDTO);

                resultado.Add(ITEMS, lstComboDTO);
                resultado.Add(SUCCESS, true);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "FillTipoUsuarioFacultamientos", e, AccionEnum.FILLCOMBO, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }
        #endregion

        #region CATALOGO FIRMASTES ENCARGADOS DE CIERTOS CONTRATOS
        public Dictionary<string, object> GetUsuariosRelSubcontratistas(UsuarioRelSubcontratistaDTO objDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region SE OBTIENE LISTADO DE USUARIOS REL SUBCONTRATISTAS
                string strQuery = string.Empty;
                if (objDTO.idUsuarioSubcontratista > 0)
                {
                    strQuery = string.Format(@"SELECT t2.id AS idSubcontratista, t2.nombre AS subcontratista, t1.id AS idContrato_ID, t1.numeroContrato
	                                                    FROM tblX_Contrato AS t1
	                                                    INNER JOIN tblX_SubContratista AS t2 ON t1.subcontratistaID = t2.id
		                                                    WHERE t2.estatus = 1 AND t2.id = {0}
			                                                    ORDER BY t2.nombre", objDTO.idUsuarioSubcontratista);
                    List<UsuarioRelSubcontratistaDTO> lstUsuariosRelSubcontratistas = _context.Select<UsuarioRelSubcontratistaDTO>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                        consulta = strQuery
                    }).ToList();

                    // SE OBTIENE NOMBRE DEL FIRMANTE Y SU CORREO, DEL SUBCONTRATISTA SELECCIONADO EN EL FILTRO
                    strQuery = string.Format("SELECT id, idUsuarioSubcontratista, idContrato, nombreFirmante, correo FROM tblCO_ADP_UsuariosFirmantesRelSubcontratistas WHERE registroActivo = 1 AND registroHistorial = 0 AND idUsuarioSubcontratista = {0}", objDTO.idUsuarioSubcontratista);
                    List<UsuarioRelSubcontratistaDTO> lstFirmantes = _context.Select<UsuarioRelSubcontratistaDTO>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Construplan,
                        consulta = strQuery
                    }).ToList();

                    foreach (var item in lstFirmantes)
                    {
                        List<int> lstContratos_ID = new List<int>();
                        string[] SplitContratos = item.idContrato.Split(',');
                        foreach (var itemContrato in SplitContratos)
                        {
                            if (!string.IsNullOrEmpty(itemContrato) && itemContrato != ",")
                            {
                                string contrato = itemContrato.Replace(",", "");
                                lstContratos_ID.Add(Convert.ToInt32(contrato));
                            }
                        }

                        item.subcontratista = lstUsuariosRelSubcontratistas.Where(w => w.idSubcontratista == item.idUsuarioSubcontratista && lstContratos_ID.Contains(w.idContrato_ID)).Select(s => s.subcontratista).FirstOrDefault();
                        List<UsuarioRelSubcontratistaDTO> lstUsuarioRelSubcontratista = lstUsuariosRelSubcontratistas.Where(w => w.idSubcontratista == item.idUsuarioSubcontratista && lstContratos_ID.Contains(w.idContrato_ID)).ToList();
                        string contratos = string.Empty;
                        foreach (var item2 in lstUsuarioRelSubcontratista)
                        {
                            if (string.IsNullOrEmpty(contratos))
                                contratos = item2.numeroContrato;
                            else
                                contratos += string.Format(",{0}", item2.numeroContrato);
                        }
                        item.idContrato = contratos;
                    }

                    resultado.Add("lstUsuariosRelSubcontratistas", lstFirmantes);
                    resultado.Add(SUCCESS, true);
                }
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "GetUsuariosRelSubcontratistas", e, AccionEnum.CONSULTA, objDTO.id, objDTO);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> CEUsuarioRelSubcontratista(UsuarioRelSubcontratistaDTO objDTO)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region VALIDACIONES
                    if (objDTO.idUsuarioSubcontratista <= 0) throw new Exception("Es necesario indicar el subcontratista.");
                    if (string.IsNullOrEmpty(objDTO.nombreFirmante)) throw new Exception("Es necesario indicar el nombre del responsable.");
                    if (string.IsNullOrEmpty(objDTO.correo)) throw new Exception("Es necesario indicar el correo del responsable.");

                    #region SE VERIFICA SI YA EXISTE UN FIRMANTE ACTIVO PARA EL SUBCONTRATISTA SELECCIONADO. NOTA: NOMAS DEBE HABER UN FIRMANTE ACTIVO O SI NO, PASARLO A HISTORIAL.
                    if (objDTO.id <= 0)
                    {
                        List<tblCO_ADP_UsuariosFirmantesRelSubcontratistas> lstFirmantes = _context.tblCO_ADP_UsuariosFirmantesRelSubcontratistas.Where(w => w.registroActivo && !w.registroHistorial).ToList();

                        List<string> lstContratos_ID = new List<string>();
                        foreach (var item in lstFirmantes)
                        {
                            string[] SplitContratos = item.idContrato.Split(',');
                            foreach (var itemContrato in SplitContratos)
                            {
                                if (!string.IsNullOrEmpty(itemContrato) && itemContrato != ",")
                                {
                                    string contrato = itemContrato.Replace(",", "");
                                    lstContratos_ID.Add(contrato);
                                }
                            }
                        }

                        List<tblX_Contrato> lstContratos = _context.Select<tblX_Contrato>(new DapperDTO
                        {
                            baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                            consulta = @"SELECT id, numeroContrato FROM tblX_Contrato WHERE estatus = 1"
                        }).ToList();
                        foreach (var item in objDTO.lstContratos)
                        {
                            int ContratoEnUso = lstContratos_ID.Where(w => w == item).Count();
                            if (ContratoEnUso > 0)
                            {
                                string strMensajeError = string.Format("El contrato: {0}, ya se encuentra asignado.", lstContratos.Where(w => w.id == Convert.ToInt32(item)).Select(s => s.numeroContrato).FirstOrDefault());
                                throw new Exception(strMensajeError);
                            }
                        }
                    }
                    #endregion
                    #endregion

                    string contratosSeleccionados = string.Empty;
                    foreach (var item in objDTO.lstContratos)
                    {
                        if (string.IsNullOrEmpty(contratosSeleccionados))
                            contratosSeleccionados = item;
                        else
                            contratosSeleccionados += string.Format(",{0}", item);
                    }

                    tblCO_ADP_UsuariosFirmantesRelSubcontratistas objCE = new tblCO_ADP_UsuariosFirmantesRelSubcontratistas();
                    if (objDTO.id > 0)
                    {
                        #region ACTUALIZAR REGISTRO
                        objCE = _context.tblCO_ADP_UsuariosFirmantesRelSubcontratistas.Where(w => w.id == objDTO.id).FirstOrDefault();
                        if (objCE == null)
                            throw new Exception("Ocurrió un error al actualizar el registro.");

                        objCE.idContrato = contratosSeleccionados;
                        objCE.idUsuarioSubcontratista = objDTO.idUsuarioSubcontratista;
                        objCE.nombreFirmante = objDTO.nombreFirmante;
                        objCE.correo = objDTO.correo;
                        objCE.registroHistorial = false;
                        objCE.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objCE.fechaModificacion = DateTime.Now;
                        _context.SaveChanges();
                        #endregion
                    }
                    else
                    {
                        #region NUEVO REGISTRO
                        objCE = new tblCO_ADP_UsuariosFirmantesRelSubcontratistas();
                        objCE.idContrato = contratosSeleccionados;
                        objCE.idUsuarioSubcontratista = objDTO.idUsuarioSubcontratista;
                        objCE.nombreFirmante = objDTO.nombreFirmante;
                        objCE.correo = objDTO.correo;
                        objCE.registroHistorial = objDTO.registroHistorial;
                        objCE.idUsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objCE.fechaCreacion = DateTime.Now;
                        objCE.registroActivo = true;
                        _context.tblCO_ADP_UsuariosFirmantesRelSubcontratistas.Add(objCE);
                        _context.SaveChanges();
                        #endregion
                    }
                    resultado.Add(MESSAGE, objDTO.id > 0 ? "Se ha actualizado con éxito." : "Se ha registrado con éxito.");
                    resultado.Add(SUCCESS, true);
                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, NombreControlador, "CEUsuarioRelSubcontratista", e, objDTO.id > 0 ? AccionEnum.ACTUALIZAR : AccionEnum.AGREGAR, objDTO.id, objDTO);
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> EliminarUsuarioRelSubcontratista(UsuarioRelSubcontratistaDTO objDTO)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region VALIDACIONES
                    if (objDTO.id <= 0) throw new Exception("Ocurrió un error al eliminar el responsable.");
                    #endregion

                    #region SE ELIMINA EL USUARIO RELACIONADO AL SUBCONTRATISTA
                    tblCO_ADP_UsuariosFirmantesRelSubcontratistas objEliminar = _context.tblCO_ADP_UsuariosFirmantesRelSubcontratistas.Where(w => w.id == objDTO.id).FirstOrDefault();
                    if (objEliminar == null)
                        throw new Exception("Ocurrió un error al eliminar el responsable.");

                    objEliminar.registroActivo = false;
                    objEliminar.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                    objEliminar.fechaModificacion = DateTime.Now;
                    _context.SaveChanges();
                    #endregion

                    resultado.Add(MESSAGE, "Se ha eliminado con éxito.");
                    resultado.Add(SUCCESS, true);
                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, NombreControlador, "EliminarUsuarioRelSubcontratista", e, AccionEnum.ELIMINAR, objDTO.id, objDTO);
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> MandarUsuarioComoHistorial(UsuarioRelSubcontratistaDTO objDTO)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region VALIDACIONES
                    if (objDTO.id <= 0) throw new Exception("Ocurrió un error al mandar el usuario al historial.");
                    #endregion

                    #region SE DESHABILITA EL USUARIO Y SE INDICA AL USUARIO COMO HISTORIAL PARA FUTURAS CONSULTAS
                    tblCO_ADP_UsuariosFirmantesRelSubcontratistas objUsuarioHistorial = _context.tblCO_ADP_UsuariosFirmantesRelSubcontratistas.Where(w => w.id == objDTO.id).FirstOrDefault();
                    if (objUsuarioHistorial == null)
                        throw new Exception("Ocurrió un error al mover el usuario a historial.");

                    objUsuarioHistorial.registroActivo = false;
                    objUsuarioHistorial.registroHistorial = true;
                    objUsuarioHistorial.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                    objUsuarioHistorial.fechaModificacion = DateTime.Now;
                    _context.SaveChanges();
                    #endregion

                    resultado.Add(MESSAGE, "Se ha registrado con éxito en historial.");
                    resultado.Add(SUCCESS, true);
                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, NombreControlador, "MandarUsuarioComoHistorial", e, AccionEnum.ACTUALIZAR, objDTO.id, objDTO);
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> GetDatosActualizarUsuarioRelSubcontratista(UsuarioRelSubcontratistaDTO objDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                if (objDTO.id <= 0) throw new Exception("Ocurrió un error al obtener la información del registro seleccionado.");
                #endregion

                #region SE OBTIENE LA INFORMACIÓN DEL USUARIO A ACTUALIZAR
                tblCO_ADP_UsuariosFirmantesRelSubcontratistas objUsuarioRelSubcontratista = _context.tblCO_ADP_UsuariosFirmantesRelSubcontratistas.Where(w => w.id == objDTO.id).FirstOrDefault();
                if (objUsuarioRelSubcontratista == null)
                    throw new Exception("Ocurrió un error al obtener la información del registro seleccionado.");

                resultado.Add("objUsuarioRelSubcontratista", objUsuarioRelSubcontratista);
                resultado.Add(SUCCESS, true);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "GetDatosActualizarUsuarioRelSubcontratista", e, AccionEnum.CONSULTA, objDTO.id, objDTO);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboContratosRelSubcontratistas(int idSubcontratista)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region SE OBTIENE LISTADO DE CONTRATOS
                List<dynamic> lstContratos = new List<dynamic>(); ;
                string strQuery = string.Format(@"SELECT id, numeroContrato, cc FROM tblX_Contrato WHERE estatus = {0} AND subcontratistaID = {1}", 1, idSubcontratista);
                using (var ctx = new SqlConnection(ConextSigoDapper.conexionSUBCONTRATISTAS()))
                {
                    ctx.Open();
                    lstContratos = ctx.Query<dynamic>(strQuery, null, null, true, 300).ToList();
                    ctx.Close();
                }

                List<ComboDTO> lstContratosDTO = new List<ComboDTO>();
                foreach (var item in lstContratos)
                {
                    ComboDTO obj = new ComboDTO();
                    obj.Value = item.id.ToString();
                    obj.Text = "[" + item.cc + "] " + item.numeroContrato;
                    lstContratosDTO.Add(obj);
                }

                resultado.Add(ITEMS, lstContratosDTO);
                resultado.Add(SUCCESS, true);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "FillCboContratosRelSubcontratistas", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }
        #endregion

        #region CALENDARIO
        public Dictionary<string, object> FillCboEvaluacionesActivas(SubContratistasDTO objDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region SE OBTIENE LOS CC QUE TIENE ASIGNADO EL USUARIO LOGUEADO (FACULTAMIENTOS)
                bool todosLosCC = false;
                string strQuery = string.Format(@"SELECT * FROM tblCO_ADP_Facultamientos WHERE idUsuario = {0} AND esActivo = {1}", (int)vSesiones.sesionUsuarioDTO.id, 1);
                List<tblCO_ADP_Facultamientos> lstUsuarioFacultamientos = _context.Select<tblCO_ADP_Facultamientos>(new DapperDTO
                {
                    baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                    consulta = strQuery
                }).ToList();
                List<string> lstCC = new List<string>();
                List<string> lstUsuarioRelCC = lstUsuarioFacultamientos.Select(s => s.cc).ToList();
                foreach (var item in lstUsuarioRelCC)
                {
                    string[] SplitCC = item.Split(',');
                    foreach (var itemCC in SplitCC)
                    {
                        if (!string.IsNullOrEmpty(itemCC) && itemCC != ",")
                        {
                            string cc = itemCC.Replace(",", "");
                            lstCC.Add(string.Format("{0}", cc));
                        }
                    }
                }
                #endregion

                #region SE OBTIENE LAS EVALUACIONES
                List<tblX_SubContratista> lstSubcontratistas = db.tblX_SubContratista.Where(w => w.estatus).ToList();
                List<tblCO_ADP_EvalSubConAsignacion> lstEvaluaciones = new List<tblCO_ADP_EvalSubConAsignacion>();
                if (!todosLosCC)
                    lstEvaluaciones = db.tblCO_ADP_EvalSubConAsignacion.Where(w => lstCC.Contains(w.cc)).ToList();
                else
                    lstEvaluaciones = db.tblCO_ADP_EvalSubConAsignacion.ToList();

                if (objDTO.id > 0)
                    lstEvaluaciones = lstEvaluaciones.Where(w => lstCC.Contains(w.cc) && w.idSubContratista == objDTO.id).ToList();

                List<ComboDTO> lstComboDTO = new List<ComboDTO>();
                foreach (var item in lstEvaluaciones.Where(w => w.idSubContratista == objDTO.id).ToList())
                {
                    string value = item.id.ToString();
                    string text = string.Format("[{0}] {1} - {2}", item.cc, lstSubcontratistas.Where(w => w.id == item.idSubContratista).Select(s => s.nombre).FirstOrDefault(), item.nombreEvaluacion);
                    string prefijo = string.Format("[{0}] {1}", item.cc, lstSubcontratistas.Where(w => w.id == item.idSubContratista).Select(s => s.nombre).FirstOrDefault());

                    ComboDTO obj = new ComboDTO();
                    obj.Value = value;
                    obj.Text = text;
                    obj.Prefijo = prefijo;
                    lstComboDTO.Add(obj);
                }

                resultado.Add(ITEMS, lstComboDTO);
                resultado.Add(SUCCESS, true);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "FillCboEvaluacionesActivas", e, AccionEnum.FILLCOMBO, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> GetFechasEvaluaciones(UsuarioRelSubcontratistaDTO objDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region SE OBTIENE LOS CC QUE TIENE ASIGNADO EL USUARIO LOGUEADO (FACULTAMIENTOS)
                string strQuery = string.Format(@"SELECT * FROM tblCO_ADP_Facultamientos WHERE idUsuario = {0} AND esActivo = {1}", (int)vSesiones.sesionUsuarioDTO.id, 1);
                List<tblCO_ADP_Facultamientos> lstUsuarioFacultamientos = _context.Select<tblCO_ADP_Facultamientos>(new DapperDTO
                {
                    baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                    consulta = strQuery
                }).ToList();
                List<string> lstCC = new List<string>();
                List<string> lstUsuarioRelCC = lstUsuarioFacultamientos.Select(s => s.cc).ToList();
                foreach (var item in lstUsuarioRelCC)
                {
                    string[] SplitCC = item.Split(',');
                    foreach (var itemCC in SplitCC)
                    {
                        if (!string.IsNullOrEmpty(itemCC) && itemCC != ",")
                        {
                            string cc = itemCC.Replace(",", "");
                            lstCC.Add(string.Format("{0}", cc));
                        }
                    }
                }
                #endregion

                #region SE OBTIENE LAS EVALUACIONES
                List<tblX_SubContratista> lstSubcontratistas = db.tblX_SubContratista.Where(w => w.estatus).ToList();
                List<tblCO_ADP_EvalSubConAsignacion> lstEvaluacionesAsignadas = new List<tblCO_ADP_EvalSubConAsignacion>();

                if (objDTO.id > 0)
                    lstEvaluacionesAsignadas = db.tblCO_ADP_EvalSubConAsignacion.Where(w => lstCC.Contains(w.cc) && w.id == objDTO.id).ToList();
                else
                    lstEvaluacionesAsignadas = db.tblCO_ADP_EvalSubConAsignacion.Where(w => lstCC.Contains(w.cc)).ToList();

                List<int> lstEvaluacionesID = new List<int>();
                foreach (var item in lstEvaluacionesAsignadas)
                {
                    lstEvaluacionesID.Add(item.id);
                }
                #endregion

                #region SE OBTIENE LA FECHA INICIAL Y FECHA FINAL DE LA EVALUACIÓN SELECCIONADA
                List<tblCO_ADP_EvalSubConAsignacion> lstEvaluaciones = db.tblCO_ADP_EvalSubConAsignacion.Where(w => lstEvaluacionesID.Contains(w.id)).ToList();
                if (lstEvaluaciones == null)
                    throw new Exception("Ocurrió un error al obtener la información de las evaluaciones.");

                List<CalendarioDTO> lstCalentarioDTO = new List<CalendarioDTO>();
                CalendarioDTO objCalendarioDTO = new CalendarioDTO();
                foreach (var item in lstEvaluaciones)
                {
                    objCalendarioDTO = new CalendarioDTO();
                    objCalendarioDTO.id = item.id;
                    objCalendarioDTO.nombreEvaluacion = item.nombreEvaluacion;
                    objCalendarioDTO.fechaInicial = item.fechaInicial;
                    objCalendarioDTO.fechaFinal = item.fechaFinal;
                    objCalendarioDTO.fecha7Dias = item.fechaFinal.AddDays(07);
                    objCalendarioDTO.fechaEvaluacion = item.fechaEvaluacion;
                    objCalendarioDTO.esEvaluacionActual = false;
                    objCalendarioDTO.color = "#f1ce11";
                    lstCalentarioDTO.Add(objCalendarioDTO);
                }

                strQuery = "SELECT evaluacionId AS id FROM tblX_FirmaEvaluacion WHERE esActivo = 1";
                List<tblX_FirmaEvaluacion> lstFirmasEvaluaciones = _context.Select<tblX_FirmaEvaluacion>(new DapperDTO
                {
                    baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                    consulta = strQuery
                }).ToList();

                foreach (var item in lstCalentarioDTO)
                {
                    tblX_FirmaEvaluacion objFirmaEvaluacion = lstFirmasEvaluaciones.Where(w => w.id == item.id).FirstOrDefault();

                    if (objFirmaEvaluacion == null)
                    {
                        item.esEvaluacionActual = true;
                        item.color = "green";
                        break;
                    }
                    else
                        item.color = "gray";
                }

                resultado.Add("lstCalentarioDTO", lstCalentarioDTO);
                resultado.Add(SUCCESS, true);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "GetFechasEvaluaciones", e, AccionEnum.CONSULTA, objDTO.idEvaluacion, objDTO);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> ActualizarFechasActualizacion(CalendarioDTO objDTO)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region VALIDACIONES
                    if (string.IsNullOrEmpty(objDTO.nombreEvaluacion)) throw new Exception("Es necesario indicar el nombre de evaluación.");
                    if (objDTO.fechaInicial == null) throw new Exception("Es necesario indicar la fecha inicial.");
                    if (objDTO.fechaFinal == null) throw new Exception("Es necesario indicar la fecha final.");
                    if (objDTO.fechaEvaluacion == null) throw new Exception("Es necesario indicar la fecha de evaluación.");
                    #endregion

                    #region SE ACTUALIZA LA FECHA INICIAL Y FECHA FINAL DE LA ACTUALIZACIÓN
                    tblCO_ADP_EvalSubConAsignacion objActualizar = db.tblCO_ADP_EvalSubConAsignacion.Where(w => w.id == objDTO.id).FirstOrDefault();
                    if (objActualizar == null)
                        throw new Exception("Ocurrió un error al actualizar las fechas.");

                    objActualizar.nombreEvaluacion = objDTO.nombreEvaluacion.Trim().ToUpper();
                    objActualizar.fechaInicial = objDTO.fechaInicial;
                    objActualizar.fechaFinal = objDTO.fechaFinal;
                    objActualizar.fechaEvaluacion = objDTO.fechaEvaluacion;
                    db.SaveChanges();

                    resultado.Add(MESSAGE, "Se ha actualizado con éxito.");
                    resultado.Add(SUCCESS, true);
                    dbContextTransaction.Commit();
                    #endregion
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, NombreControlador, "ActualizarFechasActualizacion", e, AccionEnum.CONSULTA, objDTO.id, objDTO);
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> GetFechasActualizar(CalendarioDTO objDTO)
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region VALIDACIONES
                if (objDTO.id <= 0)
                    throw new Exception("Ocurrió un error al obtener las fechas a actualizar.");
                #endregion

                #region SE OBTIENE LAS FECHAS A ACTUALIZAR
                string strQuery = string.Format("SELECT * FROM tblCO_ADP_EvalSubConAsignacion WHERE id = {0}", objDTO.id);
                tblCO_ADP_EvalSubConAsignacion objEvaluacion = _context.Select<tblCO_ADP_EvalSubConAsignacion>(new DapperDTO
                {
                    baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                    consulta = strQuery
                }).FirstOrDefault();

                if (objEvaluacion == null)
                    throw new Exception("Ocurrió un error al obtener las fechas a actualizar.");

                CalendarioDTO obj = new CalendarioDTO();
                obj.nombreEvaluacion = objEvaluacion.nombreEvaluacion;
                obj.fechaInicial = objEvaluacion.fechaInicial;
                obj.fechaFinal = objEvaluacion.fechaFinal;
                obj.fechaInicialEjecutable = objEvaluacion.fechaInicialEjecutable;
                obj.fechaFinalEjecutable = objEvaluacion.fechaFinalEjecutable;
                obj.fechaEvaluacion = objEvaluacion.fechaEvaluacion;

                resultado.Add("objEvaluacion", obj);
                resultado.Add(SUCCESS, true);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "GetFechasActualizar", e, AccionEnum.CONSULTA, objDTO.id, objDTO);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> GetTipoUsuario()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region SE VERIFICA QUE TIPO DE USUARIO ES EL QUE SE ENCUENTRA LOGUEADO
                tblCO_ADP_Facultamientos objFacultamientosUsuario = db.tblCO_ADP_Facultamientos.Where(w => w.idUsuario == (int)vSesiones.sesionUsuarioDTO.id && w.esActivo).FirstOrDefault();
                if (objFacultamientosUsuario == null)
                    throw new Exception("Este usuario no cuenta con ningún facultamiento.");

                resultado.Add("tipoUsuario", objFacultamientosUsuario.tipo);
                resultado.Add(SUCCESS, true);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "GetTipoUsuario", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }
        #endregion

        #region CATÁLOGO DE PLANTILLAS
        public Dictionary<string, object> FillComboContratos()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                List<tblX_Contrato> listaContratos = _context.Select<tblX_Contrato>(new DapperDTO
                {
                    baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                    consulta = @"SELECT * FROM tblX_Contrato WHERE estatus = @estatus",
                    parametros = new { estatus = true }
                }).ToList();

                List<ComboDTO> listaComboDTO = new List<ComboDTO>();

                foreach (var contrato in listaContratos)
                {
                    listaComboDTO.Add(new ComboDTO
                    {
                        Value = contrato.id.ToString(),
                        Text = string.Format("[{0}] {1}", contrato.cc.Trim().ToUpper(), contrato.numeroContrato.Trim().ToUpper())
                    });
                }

                resultado.Add(ITEMS, listaComboDTO.OrderBy(x => x.Text).ToList());
                resultado.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                LogError(0, 0, NombreControlador, "FillComboContratos", ex, AccionEnum.FILLCOMBO, 0, 0);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }
        #endregion

        #region REPORTE CRYSTAL REPORT
        public List<tblCO_ADP_EvalSubConAsignacion> GetEvaluaciones(int idAsignacion)
        {
            List<tblCO_ADP_EvalSubConAsignacion> lstEvaluacionesToImprimir = new List<tblCO_ADP_EvalSubConAsignacion>();
            try
            {
                #region SE OBTIENE LAS EVALUACIONES
                List<tblCO_ADP_EvalSubConAsignacion> lstEvaluaciones = db.tblCO_ADP_EvalSubConAsignacion.ToList();
                tblCO_ADP_EvalSubConAsignacion objEvaluacion = lstEvaluaciones.Where(w => w.id == idAsignacion).FirstOrDefault();
                if (objEvaluacion == null)
                    throw new Exception("Ocurrió un error al obtener el reporte.");

                lstEvaluacionesToImprimir = lstEvaluaciones.Where(w => w.cc == objEvaluacion.cc && w.idSubContratista == objEvaluacion.idSubContratista && w.idPlantilla == objEvaluacion.idPlantilla &&
                                                                       w.idContrato == objEvaluacion.idContrato && w.servicioContratado == objEvaluacion.servicioContratado && w.idEstado == objEvaluacion.idEstado &&
                                                                       w.idMunicipio == objEvaluacion.idMunicipio && w.cantEvaluaciones == objEvaluacion.cantEvaluaciones).ToList();
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "GetEvaluaciones", e, AccionEnum.CONSULTA, idAsignacion, 0);
                return null;
            }
            return lstEvaluacionesToImprimir;
        }
        #endregion

        #region METODOS GENERALES
        public int GetTipoFacultamientosUsuarioLogueado()
        {
            tblCO_ADP_Facultamientos objFacultamiento = new tblCO_ADP_Facultamientos();
            try
            {
                #region SE OBTIENE EL TIPO DE FACULTAMIENTO EL USUARIO LOGUEADO
                string strQuery = string.Format("SELECT * FROM tblCO_ADP_Facultamientos WHERE idUsuario = {0} AND esActivo = 1", (int)vSesiones.sesionUsuarioDTO.id);
                objFacultamiento = _context.Select<tblCO_ADP_Facultamientos>(new DapperDTO
                {
                    baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                    consulta = strQuery
                }).FirstOrDefault();
                if (objFacultamiento == null)
                    throw new Exception("Ocurrió un error al obtener los facultamientos del usuario logueado.");
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "GetTipoFacultamientosUsuarioLogueado", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return objFacultamiento.tipo;
        }

        public List<tblCO_ADP_Facultamientos> GetListadoCCRelUsuario()
        {
            List<tblCO_ADP_Facultamientos> lstCC = new List<tblCO_ADP_Facultamientos>();
            try
            {
                #region SE OBTIENE LISTADO DE CC QUE CONTENGA EN FACULTAMIENTOS EL USUARIO LOGUEADO
                string strQuery = string.Format(@"SELECT * FROM tblCO_ADP_Facultamientos WHERE idUsuario = {0} AND esActivo = 1", (int)vSesiones.sesionUsuarioDTO.id);
                List<tblCO_ADP_Facultamientos> lstUsuarioFacultamientos = _context.Select<tblCO_ADP_Facultamientos>(new DapperDTO
                {
                    baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                    consulta = strQuery
                }).ToList();

                List<tblCO_ADP_Facultamientos> lstUsuarioRelCC = lstUsuarioFacultamientos.ToList();
                foreach (var item in lstUsuarioRelCC)
                {
                    string[] SplitCC = item.cc.Split(',');
                    foreach (var itemCC in SplitCC)
                    {
                        if (!string.IsNullOrEmpty(itemCC) && itemCC != ",")
                        {
                            tblCO_ADP_Facultamientos objFacultamiento = new tblCO_ADP_Facultamientos();
                            int existeCCEnLista = lstCC.Where(w => w.cc == itemCC).Count();
                            if (existeCCEnLista <= 0)
                            {
                                string cc = itemCC.Replace(",", "");
                                objFacultamiento.idUsuario = item.idUsuario;
                                objFacultamiento.cc = cc;
                                lstCC.Add(objFacultamiento);
                            }
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                LogError(0, 0, NombreControlador, "GetListadoCCRelUsuario", ex, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(MESSAGE, ex.Message);
                resultado.Add(SUCCESS, false);
            }
            return lstCC.ToList();
        }

        public List<string> GetListadoCCRelUsuarioTipoEvaluador()
        {
            List<string> lstCC = new List<string>();
            try
            {
                #region SE OBTIENE LISTADO DE CC QUE CONTENGA EN FACULTAMIENTOS EL USUARIO LOGUEADO
                string strQuery = string.Format(@"SELECT * FROM tblCO_ADP_EvaluadorXcc WHERE esActivo = 1");
                List<tblCO_ADP_Facultamientos> lstUsuariosRelCCTipoEvaluador = _context.Select<tblCO_ADP_Facultamientos>(new DapperDTO
                {
                    baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                    consulta = strQuery
                }).ToList();

                List<string> lstUsuarioRelCC = lstUsuariosRelCCTipoEvaluador.Select(s => s.cc).ToList();
                foreach (var item in lstUsuarioRelCC)
                {
                    string[] SplitCC = item.Split(',');
                    foreach (var itemCC in SplitCC)
                    {
                        if (!string.IsNullOrEmpty(itemCC) && itemCC != ",")
                        {
                            string cc = itemCC.Replace(",", "");
                            lstCC.Add(cc);
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                LogError(0, 0, NombreControlador, "GetListadoCCRelUsuarioTipoEvaluador", ex, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(MESSAGE, ex.Message);
                resultado.Add(SUCCESS, false);
            }
            return lstCC.ToList();
        }

        public List<tblP_Usuario> GetListadoUsuariosSIGOPLAN()
        {
            List<tblP_Usuario> lstUsuarios = new List<tblP_Usuario>();
            try
            {
                #region SE OBTIENE LISTADO DE USUARIOS DE SIGOPLAN
                lstUsuarios = _context.Select<tblP_Usuario>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Construplan,
                    consulta = @"SELECT * FROM tblP_Usuario WHERE estatus = @estatus",
                    parametros = new { estatus = true }
                }).ToList();
                if (lstUsuarios == null)
                    throw new Exception("Ocurrió un error al obtener listado de usuarios.");
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "GetListadoUsuariosSIGOPLAN", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return lstUsuarios.ToList();
        }

        public object GetUsuariosAutocomplete(string term, bool porClave)
        {
            var usuarios = _context.tblP_Usuario
                .Where(x =>
                    x.estatus &&
                    (porClave ? x.cveEmpleado.Contains(term) : (x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno).Contains(term))
                ).OrderBy(x => x.id).Take(12).ToList();

            return porClave ?
                usuarios.Select(x => new
                {
                    id = x.id,
                    value = x.cveEmpleado,
                    claveEmpleado = x.cveEmpleado,
                    nombre = GlobalUtils.ObtenerNombreCompletoUsuario(x)
                })
                :
                usuarios.Select(x => new
                {
                    id = x.id,
                    value = GlobalUtils.ObtenerNombreCompletoUsuario(x),
                    claveEmpleado = x.cveEmpleado,
                    nombre = GlobalUtils.ObtenerNombreCompletoUsuario(x)
                });
        }
        #endregion

        public Dictionary<string, object> FillComboElementos()
        {
            resultado = new Dictionary<string, object>();

            try
            {
                var listaElementos = db.tblCOES_Elemento.Where(x => x.registroActivo).ToList().Select(x => new ComboDTO
                {
                    Value = x.id.ToString(),
                    Text = x.descripcion,
                    Prefijo = x.mensaje
                }).ToList();

                resultado.Add(ITEMS, listaElementos);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "FillComboElementos", e, AccionEnum.FILLCOMBO, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }

        public Dictionary<string, object> FillComboRequerimientos(int elemento_id)
        {
            resultado = new Dictionary<string, object>();

            try
            {
                var listaRequermientos = db.tblCOES_Requerimiento.Where(x => x.registroActivo && x.elemento_id == elemento_id).ToList().Select(x => new ComboDTO
                {
                    Value = x.id.ToString(),
                    Text = x.descripcion,
                    Prefijo = x.elemento_id.ToString()
                }).ToList();

                resultado.Add(ITEMS, listaRequermientos);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "FillComboRequerimientos", e, AccionEnum.FILLCOMBO, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }

        public Dictionary<string, object> GuardarNuevoElemento(tblCOES_Elemento elemento)
        {
            resultado = new Dictionary<string, object>();

            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    elemento.usuarioCreacion_id = vSesiones.sesionUsuarioDTO.id;
                    elemento.fechaCreacion = DateTime.Now;
                    elemento.registroActivo = true;

                    db.tblCOES_Elemento.Add(elemento);
                    db.SaveChanges();

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                    SaveBitacora(0, (int)AccionEnum.AGREGAR, elemento.id, JsonUtils.convertNetObjectToJson(elemento));
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, NombreControlador, "GuardarNuevoElemento", e, AccionEnum.AGREGAR, 0, 0);
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> GuardarNuevoRequerimiento(tblCOES_Requerimiento requerimiento)
        {
            resultado = new Dictionary<string, object>();

            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    requerimiento.usuarioCreacion_id = vSesiones.sesionUsuarioDTO.id;
                    requerimiento.fechaCreacion = DateTime.Now;
                    requerimiento.registroActivo = true;

                    db.tblCOES_Requerimiento.Add(requerimiento);
                    db.SaveChanges();

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                    SaveBitacora(0, (int)AccionEnum.AGREGAR, requerimiento.id, JsonUtils.convertNetObjectToJson(requerimiento));
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, NombreControlador, "GuardarNuevoRequerimiento", e, AccionEnum.AGREGAR, 0, 0);
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> GetRequerimientosElemento(int elemento_id)
        {
            resultado = new Dictionary<string, object>();

            try
            {
                var listaRequerimientos = (
                    from plan in db.tblCOES_Plantilla.Where(x => x.registroActivo && x.plantillaBase).ToList()
                    join relPE in db.tblCOES_PlantillatblCOES_Elemento.Where(x => x.registroActivo && x.elemento_id == elemento_id).ToList() on plan.id equals relPE.plantilla_id
                    join req in db.tblCOES_Requerimiento.Where(x => x.registroActivo).ToList() on relPE.elemento_id equals req.elemento_id
                    join ele in db.tblCOES_Elemento.Where(x => x.registroActivo).ToList() on relPE.elemento_id equals ele.id
                    join relPER in db.tblCOES_PlantillatblCOES_ElementotblCOES_Requerimiento.Where(x => x.registroActivo).ToList() on new { relID = relPE.id, reqID = req.id } equals new { relID = relPER.relacionPlantillaElemento_id, reqID = relPER.requerimiento_id }
                    select new RequerimientoDTO
                    {
                        requerimiento_id = req.id,
                        elemento_id = req.elemento_id,
                        elementoDesc = ele.descripcion,
                        mensaje = ele.mensaje,
                        critico = relPE.critico,
                        criticoDesc = relPE.critico ? "SÍ" : "NO",
                        ponderacion = relPE.ponderacion,
                        requerimientoDesc = req.descripcion,
                        tipoRequerimiento = relPER.tipo,
                        tipoRequerimientoDesc = relPER.tipo.GetDescription()
                    }
                ).ToList();

                var elementoRegistro = db.tblCOES_Elemento.FirstOrDefault(x => x.id == elemento_id);

                resultado.Add("data", listaRequerimientos);
                resultado.Add("dataElemento", elementoRegistro);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "GetRequerimientosElemento", e, AccionEnum.FILLCOMBO, 0, elemento_id);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }

        public Dictionary<string, object> GuardarNuevaPlantilla(tblCOES_Plantilla plantilla, List<int> contratos, List<RequerimientoDTO> requerimientos)
        {
            resultado = new Dictionary<string, object>();

            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    plantilla.usuarioCreacion_id = vSesiones.sesionUsuarioDTO.id;
                    plantilla.fechaCreacion = DateTime.Now;
                    plantilla.registroActivo = true;

                    db.tblCOES_Plantilla.Add(plantilla);
                    db.SaveChanges();

                    foreach (var con in contratos)
                    {
                        db.tblCOES_PlantillatblX_Contrato.Add(new tblCOES_PlantillatblX_Contrato
                        {
                            plantilla_id = plantilla.id,
                            contrato_id = con,
                            usuarioCreacion_id = vSesiones.sesionUsuarioDTO.id,
                            fechaCreacion = DateTime.Now,
                            registroActivo = true
                        });
                        db.SaveChanges();
                    }

                    var listaElementos = requerimientos.GroupBy(x => x.elemento_id).Select(x => new
                    {
                        elemento_id = x.Key,
                        critico = x.First().critico,
                        ponderacion = x.First().ponderacion,
                        grp = x.ToList()
                    }).ToList();

                    foreach (var elemento in listaElementos)
                    {
                        var registroRelacionPlantillaElemento = new tblCOES_PlantillatblCOES_Elemento
                        {
                            plantilla_id = plantilla.id,
                            elemento_id = elemento.elemento_id,
                            critico = elemento.critico,
                            ponderacion = elemento.ponderacion,
                            usuarioCreacion_id = vSesiones.sesionUsuarioDTO.id,
                            fechaCreacion = DateTime.Now,
                            registroActivo = true
                        };

                        db.tblCOES_PlantillatblCOES_Elemento.Add(registroRelacionPlantillaElemento);
                        db.SaveChanges();

                        foreach (var requerimiento in elemento.grp)
                        {
                            db.tblCOES_PlantillatblCOES_ElementotblCOES_Requerimiento.Add(new tblCOES_PlantillatblCOES_ElementotblCOES_Requerimiento
                            {
                                relacionPlantillaElemento_id = registroRelacionPlantillaElemento.id,
                                requerimiento_id = requerimiento.requerimiento_id,
                                tipo = requerimiento.tipoRequerimiento,
                                usuarioCreacion_id = vSesiones.sesionUsuarioDTO.id,
                                fechaCreacion = DateTime.Now,
                                registroActivo = true
                            });
                            db.SaveChanges();
                        }
                    }

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                    SaveBitacora(0, (int)AccionEnum.AGREGAR, plantilla.id, JsonUtils.convertNetObjectToJson(new { plantilla = plantilla, contratos = contratos, requerimientos = requerimientos }));
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, NombreControlador, "GuardarNuevaPlantilla", e, AccionEnum.AGREGAR, 0, new { plantilla = plantilla, contratos = contratos, requerimientos = requerimientos });
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> EditarPlantilla(tblCOES_Plantilla plantilla, List<int> contratos, List<RequerimientoDTO> requerimientos)
        {
            resultado = new Dictionary<string, object>();

            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    var registroPlantilla = db.tblCOES_Plantilla.FirstOrDefault(x => x.id == plantilla.id);

                    registroPlantilla.nombre = plantilla.nombre;
                    registroPlantilla.colaborador_id = plantilla.colaborador_id;
                    registroPlantilla.fecha = plantilla.fecha;
                    registroPlantilla.tipo = plantilla.tipo;
                    registroPlantilla.plantillaBase = plantilla.plantillaBase;
                    registroPlantilla.usuarioModificacion_id = vSesiones.sesionUsuarioDTO.id;
                    registroPlantilla.fechaModificacion = DateTime.Now;

                    db.SaveChanges();

                    var listaContratosAnteriores = db.tblCOES_PlantillatblX_Contrato.Where(x => x.registroActivo && x.plantilla_id == plantilla.id).ToList();

                    foreach (var rel in listaContratosAnteriores)
                    {
                        rel.usuarioModificacion_id = vSesiones.sesionUsuarioDTO.id;
                        rel.fechaModificacion = DateTime.Now;
                        rel.registroActivo = false;
                        db.SaveChanges();
                    }

                    foreach (var con in contratos)
                    {
                        db.tblCOES_PlantillatblX_Contrato.Add(new tblCOES_PlantillatblX_Contrato
                        {
                            plantilla_id = plantilla.id,
                            contrato_id = con,
                            usuarioCreacion_id = vSesiones.sesionUsuarioDTO.id,
                            fechaCreacion = DateTime.Now,
                            registroActivo = true
                        });
                        db.SaveChanges();
                    }

                    #region Registros de relación
                    #region Eliminar registros anteriores no incluidos
                    var listaRelacionPlantillaElementoAnteriores = db.tblCOES_PlantillatblCOES_Elemento.Where(x => x.registroActivo && x.plantilla_id == plantilla.id).ToList();
                    var listaRelacionPlantillaElementoAnteriores_id = listaRelacionPlantillaElementoAnteriores.Select(x => x.id).ToList();
                    var listaRelacionPlantillaElementoRequerimientoAnteriores = db.tblCOES_PlantillatblCOES_ElementotblCOES_Requerimiento.Where(x => x.registroActivo && listaRelacionPlantillaElementoAnteriores_id.Contains(x.relacionPlantillaElemento_id)).ToList();
                    var listaRelacionPlantillaElementoRequerimientoAnteriores_id = listaRelacionPlantillaElementoRequerimientoAnteriores.Select(x => x.id).ToList();

                    var registrosRelacionPlantillaElementoAnterioresNoIncluidos = listaRelacionPlantillaElementoAnteriores_id.Where(x => !requerimientos.Select(y => y.relPE_id).Contains(x)).ToList();

                    foreach (var r in registrosRelacionPlantillaElementoAnterioresNoIncluidos)
                    {
                        var registro = db.tblCOES_PlantillatblCOES_Elemento.FirstOrDefault(x => x.id == r);

                        registro.usuarioModificacion_id = vSesiones.sesionUsuarioDTO.id;
                        registro.fechaModificacion = DateTime.Now;
                        registro.registroActivo = false;
                        db.SaveChanges();
                    }

                    var registrosRelacionPlantillaElementoRequerimientoAnterioresNoIncluidos = listaRelacionPlantillaElementoRequerimientoAnteriores_id.Where(x => !requerimientos.Select(y => y.relPER_id).Contains(x)).ToList();

                    foreach (var r in registrosRelacionPlantillaElementoRequerimientoAnterioresNoIncluidos)
                    {
                        var registro = db.tblCOES_PlantillatblCOES_ElementotblCOES_Requerimiento.FirstOrDefault(x => x.id == r);

                        registro.usuarioModificacion_id = vSesiones.sesionUsuarioDTO.id;
                        registro.fechaModificacion = DateTime.Now;
                        registro.registroActivo = false;
                        db.SaveChanges();
                    }
                    #endregion

                    #region Agregar registros nuevos
                    var listaRequerimientosNuevos = requerimientos.Where(x => x.relPE_id == 0 && x.relPER_id == 0).ToList();

                    var listaElementos = listaRequerimientosNuevos.GroupBy(x => x.elemento_id).Select(x => new
                    {
                        elemento_id = x.Key,
                        critico = x.First().critico,
                        ponderacion = x.First().ponderacion,
                        grp = x.ToList()
                    }).ToList();

                    foreach (var elemento in listaElementos)
                    {
                        var registroRelacionPlantillaElementoExistente = db.tblCOES_PlantillatblCOES_Elemento.FirstOrDefault(x => x.registroActivo && x.plantilla_id == plantilla.id && x.elemento_id == elemento.elemento_id);
                        var registroRelacionPlantillaElemento_id = 0;

                        if (registroRelacionPlantillaElementoExistente == null)
                        {
                            var registroRelacionPlantillaElemento = new tblCOES_PlantillatblCOES_Elemento
                            {
                                plantilla_id = plantilla.id,
                                elemento_id = elemento.elemento_id,
                                critico = elemento.critico,
                                ponderacion = elemento.ponderacion,
                                usuarioCreacion_id = vSesiones.sesionUsuarioDTO.id,
                                fechaCreacion = DateTime.Now,
                                registroActivo = true
                            };

                            db.tblCOES_PlantillatblCOES_Elemento.Add(registroRelacionPlantillaElemento);
                            db.SaveChanges();

                            registroRelacionPlantillaElemento_id = registroRelacionPlantillaElemento.id;
                        }
                        else
                        {
                            registroRelacionPlantillaElemento_id = registroRelacionPlantillaElementoExistente.id;
                        }

                        foreach (var requerimiento in elemento.grp)
                        {
                            db.tblCOES_PlantillatblCOES_ElementotblCOES_Requerimiento.Add(new tblCOES_PlantillatblCOES_ElementotblCOES_Requerimiento
                            {
                                relacionPlantillaElemento_id = registroRelacionPlantillaElemento_id,
                                requerimiento_id = requerimiento.requerimiento_id,
                                tipo = requerimiento.tipoRequerimiento,
                                usuarioCreacion_id = vSesiones.sesionUsuarioDTO.id,
                                fechaCreacion = DateTime.Now,
                                registroActivo = true
                            });
                            db.SaveChanges();
                        }
                    }
                    #endregion
                    #endregion

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                    SaveBitacora(0, (int)AccionEnum.ACTUALIZAR, plantilla.id, JsonUtils.convertNetObjectToJson(new { plantilla = plantilla, contratos = contratos }));
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, NombreControlador, "EditarPlantilla", e, AccionEnum.ACTUALIZAR, 0, new { plantilla = plantilla, contratos = contratos });
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> EliminarRequerimientoElemento(int requerimiento_id)
        {
            resultado = new Dictionary<string, object>();

            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    var registroRequerimiento = db.tblCOES_Requerimiento.FirstOrDefault(x => x.id == requerimiento_id);

                    if (registroRequerimiento != null)
                    {
                        registroRequerimiento.usuarioModificacion_id = vSesiones.sesionUsuarioDTO.id;
                        registroRequerimiento.fechaModificacion = DateTime.Now;
                        registroRequerimiento.registroActivo = false;
                        db.SaveChanges();
                    }

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                    SaveBitacora(0, (int)AccionEnum.ELIMINAR, requerimiento_id, JsonUtils.convertNetObjectToJson(requerimiento_id));
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, NombreControlador, "EliminarRequerimientoElemento", e, AccionEnum.ELIMINAR, 0, requerimiento_id);
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> GetPlantilla(int plantilla_id)
        {
            resultado = new Dictionary<string, object>();

            try
            {
                var registroPlantilla = db.tblCOES_Plantilla.FirstOrDefault(x => x.id == plantilla_id);
                var listaRelacionPlantillaContrato = db.tblCOES_PlantillatblX_Contrato.Where(x => x.registroActivo && x.plantilla_id == plantilla_id).Select(x => x.contrato_id).ToList();
                var registroUsuarioColaborador = _context.tblP_Usuario.FirstOrDefault(x => x.id == registroPlantilla.colaborador_id);
                var data = new PlantillaDTO
                {
                    id = registroPlantilla.id,
                    nombre = registroPlantilla.nombre,
                    colaborador_id = registroPlantilla.colaborador_id,
                    colaboradorNombre = GlobalUtils.ObtenerNombreCompletoUsuario(registroUsuarioColaborador),
                    fecha = registroPlantilla.fecha,
                    fechaString = registroPlantilla.fecha.ToShortDateString(),
                    tipo = registroPlantilla.tipo,
                    tipoDesc = registroPlantilla.tipo.GetDescription(),
                    plantillaBase = registroPlantilla.plantillaBase,
                    contratos = listaRelacionPlantillaContrato
                };

                data.requerimientos = (
                    //from rel in db.tblCOES_PlantillatblCOES_Elemento.Where(x => x.registroActivo && x.plantilla_id == plantilla_id).ToList()
                    //join req in db.tblCOES_Requerimiento.Where(x => x.registroActivo).ToList() on rel.elemento_id equals req.elemento_id
                    //join ele in db.tblCOES_Elemento.Where(x => x.registroActivo).ToList() on req.elemento_id equals ele.id
                    from plan in db.tblCOES_Plantilla.Where(x => x.registroActivo && x.id == plantilla_id).ToList()
                    join relPE in db.tblCOES_PlantillatblCOES_Elemento.Where(x => x.registroActivo && x.plantilla_id == plantilla_id).ToList() on plan.id equals relPE.plantilla_id
                    join req in db.tblCOES_Requerimiento.Where(x => x.registroActivo).ToList() on relPE.elemento_id equals req.elemento_id
                    join ele in db.tblCOES_Elemento.Where(x => x.registroActivo).ToList() on relPE.elemento_id equals ele.id
                    join relPER in db.tblCOES_PlantillatblCOES_ElementotblCOES_Requerimiento.Where(x => x.registroActivo).ToList() on new { relID = relPE.id, reqID = req.id } equals new { relID = relPER.relacionPlantillaElemento_id, reqID = relPER.requerimiento_id }
                    select new { req, ele, plan, relPE, relPER }
                ).Select(x => new RequerimientoDTO
                {
                    relPE_id = x.relPE.id,
                    relPER_id = x.relPER.id,
                    requerimiento_id = x.req.id,
                    elemento_id = x.req.elemento_id,
                    elementoDesc = x.ele.descripcion,
                    mensaje = x.ele.mensaje,
                    critico = x.relPE.critico,
                    criticoDesc = x.relPE.critico ? "SÍ" : "NO",
                    ponderacion = x.relPE.ponderacion,
                    requerimientoDesc = x.req.descripcion,
                    tipoRequerimiento = x.relPER.tipo,
                    tipoRequerimientoDesc = x.relPER.tipo.GetDescription()
                }).ToList();

                resultado.Add("data", data);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "GetPlantilla", e, AccionEnum.CONSULTA, 0, plantilla_id);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }

        public Dictionary<string, object> CopiarPlantillaBase(int plantilla_id)
        {
            resultado = new Dictionary<string, object>();

            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    var registroPlantilla = db.tblCOES_Plantilla.FirstOrDefault(x => x.id == plantilla_id);
                    var nuevoRegistroPlantilla = new tblCOES_Plantilla
                    {
                        nombre = "COPIA " + registroPlantilla.nombre,
                        colaborador_id = registroPlantilla.colaborador_id,
                        fecha = registroPlantilla.fecha,
                        tipo = registroPlantilla.tipo,
                        plantillaBase = false,
                        usuarioCreacion_id = vSesiones.sesionUsuarioDTO.id,
                        fechaCreacion = DateTime.Now,
                        registroActivo = true
                    };

                    db.tblCOES_Plantilla.Add(nuevoRegistroPlantilla);
                    db.SaveChanges();

                    var listaRelacionPlantillaContrato = db.tblCOES_PlantillatblX_Contrato.Where(x => x.registroActivo && x.plantilla_id == registroPlantilla.id).ToList();

                    foreach (var rel in listaRelacionPlantillaContrato)
                    {
                        db.tblCOES_PlantillatblX_Contrato.Add(new tblCOES_PlantillatblX_Contrato
                        {
                            plantilla_id = nuevoRegistroPlantilla.id,
                            contrato_id = rel.contrato_id,
                            usuarioCreacion_id = vSesiones.sesionUsuarioDTO.id,
                            fechaCreacion = DateTime.Now,
                            registroActivo = true
                        });
                        db.SaveChanges();
                    }

                    var listaRelacionPlantillaElemento = db.tblCOES_PlantillatblCOES_Elemento.Where(x => x.registroActivo && x.plantilla_id == registroPlantilla.id).ToList();

                    foreach (var rel in listaRelacionPlantillaElemento)
                    {
                        var nuevoRegistroRelacion = new tblCOES_PlantillatblCOES_Elemento
                        {
                            plantilla_id = nuevoRegistroPlantilla.id,
                            elemento_id = rel.elemento_id,
                            usuarioCreacion_id = vSesiones.sesionUsuarioDTO.id,
                            fechaCreacion = DateTime.Now,
                            registroActivo = true
                        };

                        db.tblCOES_PlantillatblCOES_Elemento.Add(nuevoRegistroRelacion);
                        db.SaveChanges();

                        var listaRelacionRelacionPlantillaElementoRequerimiento = db.tblCOES_PlantillatblCOES_ElementotblCOES_Requerimiento.Where(x => x.registroActivo && x.relacionPlantillaElemento_id == rel.id).ToList();

                        foreach (var relrel in listaRelacionRelacionPlantillaElementoRequerimiento)
                        {
                            db.tblCOES_PlantillatblCOES_ElementotblCOES_Requerimiento.Add(new tblCOES_PlantillatblCOES_ElementotblCOES_Requerimiento
                            {
                                relacionPlantillaElemento_id = nuevoRegistroRelacion.id,
                                requerimiento_id = relrel.requerimiento_id,
                                tipo = relrel.tipo,
                                usuarioCreacion_id = vSesiones.sesionUsuarioDTO.id,
                                fechaCreacion = DateTime.Now,
                                registroActivo = true
                            });
                            db.SaveChanges();
                        }
                    }

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                    SaveBitacora(0, (int)AccionEnum.AGREGAR, plantilla_id, JsonUtils.convertNetObjectToJson(plantilla_id));
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, NombreControlador, "CopiarPlantillaBase", e, AccionEnum.AGREGAR, 0, plantilla_id);
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> EliminarPlantilla(int plantilla_id)
        {
            resultado = new Dictionary<string, object>();

            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    var registroPlantilla = db.tblCOES_Plantilla.FirstOrDefault(x => x.id == plantilla_id);

                    if (registroPlantilla != null)
                    {
                        registroPlantilla.usuarioModificacion_id = vSesiones.sesionUsuarioDTO.id;
                        registroPlantilla.fechaModificacion = DateTime.Now;
                        registroPlantilla.registroActivo = false;
                        db.SaveChanges();
                    }

                    var listaRelacionPlantillaContrato = db.tblCOES_PlantillatblX_Contrato.Where(x => x.registroActivo && x.plantilla_id == plantilla_id).ToList();

                    foreach (var rel in listaRelacionPlantillaContrato)
                    {
                        rel.usuarioModificacion_id = vSesiones.sesionUsuarioDTO.id;
                        rel.fechaModificacion = DateTime.Now;
                        rel.registroActivo = false;
                        db.SaveChanges();
                    }

                    var listaRelacionPlantillaElemento = db.tblCOES_PlantillatblCOES_Elemento.Where(x => x.registroActivo && x.plantilla_id == plantilla_id).ToList();

                    foreach (var rel in listaRelacionPlantillaElemento)
                    {
                        rel.usuarioModificacion_id = vSesiones.sesionUsuarioDTO.id;
                        rel.fechaModificacion = DateTime.Now;
                        rel.registroActivo = false;
                        db.SaveChanges();
                    }

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                    SaveBitacora(0, (int)AccionEnum.ELIMINAR, plantilla_id, JsonUtils.convertNetObjectToJson(plantilla_id));
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, NombreControlador, "EliminarPlantilla", e, AccionEnum.ELIMINAR, 0, plantilla_id);
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> GetFacultamientoUsuario()
        {
            resultado = new Dictionary<string, object>();

            try
            {
                var registroFacultamiento = db.tblCOES_Facultamiento.FirstOrDefault(x => x.registroActivo && x.usuario_id == vSesiones.sesionUsuarioDTO.id);
                TipoFacultamientoEnum facultamiento = TipoFacultamientoEnum.NO_ASIGNADO;

                if (registroFacultamiento != null)
                {
                    facultamiento = registroFacultamiento.tipo;
                }

                resultado.Add("data", facultamiento);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "GetFacultamientoUsuario", e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }

        #region Evaluadores
        public Dictionary<string, object> GetEvaluadores(string cc, int elemento)
        {
            try
            {
                var listaEvaluadores = db.tblCOES_Evaluador.Where(x => x.registroActivo).ToList();
                var listaEvaluadores_id = listaEvaluadores.Select(x => x.id).ToList();
                var listaProyectos = db.tblCOES_Evaluador_Proyecto.Where(x => x.registroActivo && listaEvaluadores_id.Contains(x.evaluador_id)).ToList().Join(
                    _context.tblP_CC.Where(x => x.estatus).ToList(),
                    rel => rel.cc,
                    centroCosto => centroCosto.cc,
                    (rel, centroCosto) => new { rel = rel, centroCosto = centroCosto }
                ).ToList();
                var listaElementos = db.tblCOES_Evaluador_Elemento.Where(x => x.registroActivo && listaEvaluadores_id.Contains(x.evaluador_id)).ToList().Join(
                    db.tblCOES_Elemento.Where(x => x.registroActivo).ToList(),
                    rel => rel.elemento_id,
                    ele => ele.id,
                    (rel, ele) => new { rel = rel, ele = ele }
                ).ToList();

                if (cc != "" && cc != null)
                {
                    listaProyectos = listaProyectos.Where(x => x.centroCosto.cc == cc).ToList();
                }

                if (elemento > 0)
                {
                    listaElementos = listaElementos.Where(x => x.rel.elemento_id == elemento).ToList();
                }

                var listaProyectos_evaluador_id = listaProyectos.Select(y => y.rel.evaluador_id).ToList();
                var listaElementos_evaluador_id = listaElementos.Select(y => y.rel.evaluador_id).ToList();
                var listaEvaluadoresFiltrados = db.tblCOES_Evaluador.Where(x => x.registroActivo && listaProyectos_evaluador_id.Contains(x.id) && listaElementos_evaluador_id.Contains(x.id)).ToList();
                var data = listaEvaluadoresFiltrados.Select(x => new
                {
                    evaluador_id = x.id,
                    usuario_id = x.usuario_id,
                    evaluadorNombre = _context.tblP_Usuario.Where(y => y.id == x.usuario_id).Select(z => z.nombre + " " + z.apellidoPaterno + " " + z.apellidoMaterno).FirstOrDefault(),
                    proyectos = string.Join(",", listaProyectos.Where(y => y.rel.evaluador_id == x.id).Select(z => "[" + z.centroCosto.cc + "] " + z.centroCosto.descripcion).ToList()),
                    elementos = string.Join(",", listaElementos.Where(y => y.rel.evaluador_id == x.id).Select(z => "[" + z.ele.clave + "] " + z.ele.descripcion).ToList()),
                }).ToList();

                resultado.Add("data", data);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                LogError(0, 0, NombreControlador, "GetEvaluadores", e, AccionEnum.CONSULTA, 0, new { cc = cc, elemento = elemento });
            }

            return resultado;
        }

        public Dictionary<string, object> GuardarNuevoEvaluador(tblCOES_Evaluador evaluador, List<tblCOES_Evaluador_Proyecto> proyectos, List<tblCOES_Evaluador_Elemento> elementos)
        {
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    evaluador.usuarioCreacion_id = vSesiones.sesionUsuarioDTO.id;
                    evaluador.fechaCreacion = DateTime.Now;
                    evaluador.registroActivo = true;

                    db.tblCOES_Evaluador.Add(evaluador);
                    db.SaveChanges();

                    foreach (var pro in proyectos)
                    {
                        pro.evaluador_id = evaluador.id;
                        pro.usuarioCreacion_id = vSesiones.sesionUsuarioDTO.id;
                        pro.fechaCreacion = DateTime.Now;
                        pro.registroActivo = true;
                        db.tblCOES_Evaluador_Proyecto.Add(pro);
                        db.SaveChanges();
                    }

                    foreach (var ele in elementos)
                    {
                        ele.evaluador_id = evaluador.id;
                        ele.usuarioCreacion_id = vSesiones.sesionUsuarioDTO.id;
                        ele.fechaCreacion = DateTime.Now;
                        ele.registroActivo = true;
                        db.tblCOES_Evaluador_Elemento.Add(ele);
                        db.SaveChanges();
                    }

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                    LogError(0, 0, NombreControlador, "GuardarNuevoEvaluador", e, AccionEnum.AGREGAR, 0, new { evaluador = evaluador, proyectos = proyectos, elementos = elementos });
                }
            }

            return resultado;
        }

        public Dictionary<string, object> EditarEvaluador(tblCOES_Evaluador evaluador, List<tblCOES_Evaluador_Proyecto> proyectos, List<tblCOES_Evaluador_Elemento> elementos)
        {
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    var registroEvaluador = db.tblCOES_Evaluador.FirstOrDefault(x => x.id == evaluador.id);

                    registroEvaluador.usuario_id = evaluador.usuario_id;
                    registroEvaluador.usuarioModificacion_id = vSesiones.sesionUsuarioDTO.id;
                    registroEvaluador.fechaModificacion = DateTime.Now;
                    db.SaveChanges();

                    var listaProyectosAnteriores = db.tblCOES_Evaluador_Proyecto.Where(x => x.registroActivo && x.evaluador_id == registroEvaluador.id).ToList();

                    foreach (var pro in listaProyectosAnteriores)
                    {
                        pro.usuarioModificacion_id = vSesiones.sesionUsuarioDTO.id;
                        pro.fechaModificacion = DateTime.Now;
                        pro.registroActivo = false;
                        db.SaveChanges();
                    }

                    foreach (var pro in proyectos)
                    {
                        pro.evaluador_id = registroEvaluador.id;
                        pro.usuarioCreacion_id = vSesiones.sesionUsuarioDTO.id;
                        pro.fechaCreacion = DateTime.Now;
                        pro.registroActivo = true;
                        db.tblCOES_Evaluador_Proyecto.Add(pro);
                        db.SaveChanges();
                    }

                    var listaElementosAnteriores = db.tblCOES_Evaluador_Elemento.Where(x => x.registroActivo && x.evaluador_id == registroEvaluador.id).ToList();

                    foreach (var ele in listaElementosAnteriores)
                    {
                        ele.usuarioModificacion_id = vSesiones.sesionUsuarioDTO.id;
                        ele.fechaModificacion = DateTime.Now;
                        ele.registroActivo = false;
                        db.SaveChanges();
                    }

                    foreach (var ele in elementos)
                    {
                        ele.evaluador_id = registroEvaluador.id;
                        ele.usuarioCreacion_id = vSesiones.sesionUsuarioDTO.id;
                        ele.fechaCreacion = DateTime.Now;
                        ele.registroActivo = true;
                        db.tblCOES_Evaluador_Elemento.Add(ele);
                        db.SaveChanges();
                    }

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                    LogError(0, 0, NombreControlador, "EditarEvaluador", e, AccionEnum.ACTUALIZAR, 0, new { evaluador = evaluador, proyectos = proyectos, elementos = elementos });
                }
            }

            return resultado;
        }

        public Dictionary<string, object> EliminarEvaluador(int evaluador_id)
        {
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    var registroEvaluador = db.tblCOES_Evaluador.FirstOrDefault(x => x.id == evaluador_id);

                    registroEvaluador.usuarioModificacion_id = vSesiones.sesionUsuarioDTO.id;
                    registroEvaluador.fechaModificacion = DateTime.Now;
                    registroEvaluador.registroActivo = false;
                    db.SaveChanges();

                    var listaProyectosAnteriores = db.tblCOES_Evaluador_Proyecto.Where(x => x.registroActivo && x.evaluador_id == registroEvaluador.id).ToList();

                    foreach (var pro in listaProyectosAnteriores)
                    {
                        pro.usuarioModificacion_id = vSesiones.sesionUsuarioDTO.id;
                        pro.fechaModificacion = DateTime.Now;
                        pro.registroActivo = false;
                        db.SaveChanges();
                    }

                    var listaElementosAnteriores = db.tblCOES_Evaluador_Elemento.Where(x => x.registroActivo && x.evaluador_id == registroEvaluador.id).ToList();

                    foreach (var ele in listaElementosAnteriores)
                    {
                        ele.usuarioModificacion_id = vSesiones.sesionUsuarioDTO.id;
                        ele.fechaModificacion = DateTime.Now;
                        ele.registroActivo = false;
                        db.SaveChanges();
                    }

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                    LogError(0, 0, NombreControlador, "EliminarEvaluador", e, AccionEnum.ACTUALIZAR, 0, evaluador_id);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> GetEvaluador(int evaluador_id)
        {
            try
            {
                var registroEvaluador = db.tblCOES_Evaluador.FirstOrDefault(x => x.id == evaluador_id);
                var listaProyectos = db.tblCOES_Evaluador_Proyecto.Where(x => x.registroActivo && x.evaluador_id == evaluador_id).ToList().Join(
                    _context.tblP_CC.Where(x => x.estatus).ToList(),
                    rel => rel.cc,
                    centroCosto => centroCosto.cc,
                    (rel, centroCosto) => new { rel = rel, centroCosto = centroCosto }
                ).ToList();
                var listaElementos = db.tblCOES_Evaluador_Elemento.Where(x => x.registroActivo && x.evaluador_id == evaluador_id).ToList().Join(
                    db.tblCOES_Elemento.Where(x => x.registroActivo).ToList(),
                    rel => rel.elemento_id,
                    ele => ele.id,
                    (rel, ele) => new { rel = rel, ele = ele }
                ).ToList();

                var data = new
                {
                    evaluador_id = registroEvaluador.id,
                    usuario_id = registroEvaluador.usuario_id,
                    evaluadorNombre = _context.tblP_Usuario.Where(y => y.id == registroEvaluador.usuario_id).Select(z => z.nombre + " " + z.apellidoPaterno + " " + z.apellidoMaterno).FirstOrDefault(),
                    proyectos = listaProyectos.Select(x => x.rel.cc).ToList(),
                    elementos = listaElementos.Select(x => x.rel.elemento_id).ToList()
                };

                resultado.Add("data", data);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                LogError(0, 0, NombreControlador, "GetEvaluador", e, AccionEnum.CONSULTA, 0, evaluador_id);
            }

            return resultado;
        }
        #endregion

        #region Facultamientos
        public Dictionary<string, object> GetFacultamientos(string cc, TipoFacultamientoEnum tipo)
        {
            try
            {
                // SE VERIFICA LA CANTIDAD DE CC ACTIVOS
                int cantCCActivos = _context.tblP_CC.Where(w => w.estatus).Count();

                var listaFacultamientos = db.tblCOES_Facultamiento.Where(x => x.registroActivo).ToList();
                var listaFacultamientos_id = listaFacultamientos.Select(x => x.id).ToList();
                var listaProyectos = db.tblCOES_Facultamiento_CentroCosto.Where(x => x.registroActivo && listaFacultamientos_id.Contains(x.facultamiento_id)).ToList().Join(
                    _context.tblP_CC.Where(x => x.estatus).ToList(),
                    rel => rel.cc,
                    centroCosto => centroCosto.cc,
                    (rel, centroCosto) => new { rel = rel, centroCosto = centroCosto }
                ).ToList();

                if (cc != "" && cc != null)
                {
                    listaProyectos = listaProyectos.Where(x => x.centroCosto.cc == cc).ToList();
                }

                var listaProyectos_facultamiento_id = listaProyectos.Select(y => y.rel.facultamiento_id).ToList();
                var listaFacultamientosFiltrados = db.tblCOES_Facultamiento.Where(x => x.registroActivo && listaProyectos_facultamiento_id.Contains(x.id)).ToList();
                var data = listaFacultamientosFiltrados.Select(x => new FacultamientosDTO
                {
                    facultamiento_id = x.id,
                    usuario_id = x.usuario_id,
                    facultamientoNombre = _context.tblP_Usuario.Where(y => y.id == x.usuario_id).Select(z => z.nombre + " " + z.apellidoPaterno + " " + z.apellidoMaterno).FirstOrDefault(),
                    proyectos = string.Join(",", listaProyectos.Where(y => y.rel.facultamiento_id == x.id).Select(z => "[" + z.centroCosto.cc + "] " + z.centroCosto.descripcion).ToList()),
                    tipo = x.tipo,
                    tipoDesc = x.tipo.GetDescription(),
                    mostrarModalCC = false
                }).ToList();

                if (tipo != TipoFacultamientoEnum.NO_ASIGNADO)
                    data = data.Where(x => x.tipo == tipo).ToList();

                foreach (var item in data)
                {
                    int cantCC = listaProyectos.Where(w => w.rel.facultamiento_id == item.facultamiento_id).Count();
                    if (cantCC > 4)
                        item.mostrarModalCC = true;
                }

                resultado.Add("data", data);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                LogError(0, 0, NombreControlador, "GetFacultamientos", e, AccionEnum.CONSULTA, 0, new { cc = cc, tipo = tipo });
            }

            return resultado;
        }

        public Dictionary<string, object> GetListadoCCRelUsuarioFacultamientos(int facultamiento_id)
        {
            try
            {
                #region SE OBTIENE LISTADO DE CC DEL USUARIO

                #region CATALOGOS
                List<tblP_CC> lstCC = _context.tblP_CC.Where(w => w.estatus).ToList();
                #endregion

                List<tblCOES_Facultamiento_CentroCosto> lstCCRelUsuario = db.tblCOES_Facultamiento_CentroCosto.Where(w => w.facultamiento_id == facultamiento_id && w.registroActivo).ToList();

                List<UsuarioRelCCDTO> lstUsuarioRelCC = new List<UsuarioRelCCDTO>();
                UsuarioRelCCDTO objUsuarioRelCC = new UsuarioRelCCDTO();
                foreach (var item in lstCCRelUsuario)
                {
                    tblP_CC objCC = lstCC.Where(w => w.cc == item.cc).FirstOrDefault();
                    if (objCC != null)
                    {
                        if (!string.IsNullOrEmpty(objCC.cc) && !string.IsNullOrEmpty(objCC.descripcion))
                        {
                            objUsuarioRelCC = new UsuarioRelCCDTO();
                            objUsuarioRelCC.cc = string.Format("[{0}] {1}", objCC.cc.Trim().ToUpper(), objCC.descripcion.Trim().ToUpper());
                            lstUsuarioRelCC.Add(objUsuarioRelCC);
                        }
                    }
                }

                resultado.Add(SUCCESS, true);
                resultado.Add("lstUsuarioRelCC", lstUsuarioRelCC);
                #endregion
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "GetListadoCCRelUsuario", e, AccionEnum.CONSULTA, facultamiento_id, new { facultamiento_id = facultamiento_id });
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Dictionary<string, object> GuardarNuevoFacultamiento(tblCOES_Facultamiento facultamiento, List<tblCOES_Facultamiento_CentroCosto> proyectos)
        {
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    facultamiento.usuarioCreacion_id = vSesiones.sesionUsuarioDTO.id;
                    facultamiento.fechaCreacion = DateTime.Now;
                    facultamiento.registroActivo = true;

                    db.tblCOES_Facultamiento.Add(facultamiento);
                    db.SaveChanges();

                    foreach (var pro in proyectos)
                    {
                        pro.facultamiento_id = facultamiento.id;
                        pro.usuarioCreacion_id = vSesiones.sesionUsuarioDTO.id;
                        pro.fechaCreacion = DateTime.Now;
                        pro.registroActivo = true;
                        db.tblCOES_Facultamiento_CentroCosto.Add(pro);
                        db.SaveChanges();
                    }

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                    LogError(0, 0, NombreControlador, "GuardarNuevoFacultamiento", e, AccionEnum.AGREGAR, 0, new { facultamiento = facultamiento, proyectos = proyectos });
                }
            }

            return resultado;
        }

        public Dictionary<string, object> EditarFacultamiento(tblCOES_Facultamiento facultamiento, List<tblCOES_Facultamiento_CentroCosto> proyectos)
        {
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    var registroFacultamiento = db.tblCOES_Facultamiento.FirstOrDefault(x => x.id == facultamiento.id);

                    registroFacultamiento.usuario_id = facultamiento.usuario_id;
                    registroFacultamiento.tipo = facultamiento.tipo;
                    registroFacultamiento.usuarioModificacion_id = vSesiones.sesionUsuarioDTO.id;
                    registroFacultamiento.fechaModificacion = DateTime.Now;
                    db.SaveChanges();

                    var listaProyectosAnteriores = db.tblCOES_Facultamiento_CentroCosto.Where(x => x.registroActivo && x.facultamiento_id == registroFacultamiento.id).ToList();

                    foreach (var pro in listaProyectosAnteriores)
                    {
                        pro.usuarioModificacion_id = vSesiones.sesionUsuarioDTO.id;
                        pro.fechaModificacion = DateTime.Now;
                        pro.registroActivo = false;
                        db.SaveChanges();
                    }

                    foreach (var pro in proyectos)
                    {
                        pro.facultamiento_id = registroFacultamiento.id;
                        pro.usuarioCreacion_id = vSesiones.sesionUsuarioDTO.id;
                        pro.fechaCreacion = DateTime.Now;
                        pro.registroActivo = true;
                        db.tblCOES_Facultamiento_CentroCosto.Add(pro);
                        db.SaveChanges();
                    }

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                    LogError(0, 0, NombreControlador, "EditarFacultamiento", e, AccionEnum.ACTUALIZAR, 0, new { facultamiento = facultamiento, proyectos = proyectos });
                }
            }

            return resultado;
        }

        public Dictionary<string, object> EliminarFacultamiento(int facultamiento_id)
        {
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    var registroFacultamiento = db.tblCOES_Facultamiento.FirstOrDefault(x => x.id == facultamiento_id);

                    registroFacultamiento.usuarioModificacion_id = vSesiones.sesionUsuarioDTO.id;
                    registroFacultamiento.fechaModificacion = DateTime.Now;
                    registroFacultamiento.registroActivo = false;
                    db.SaveChanges();

                    var listaProyectosAnteriores = db.tblCOES_Facultamiento_CentroCosto.Where(x => x.registroActivo && x.facultamiento_id == registroFacultamiento.id).ToList();

                    foreach (var pro in listaProyectosAnteriores)
                    {
                        pro.usuarioModificacion_id = vSesiones.sesionUsuarioDTO.id;
                        pro.fechaModificacion = DateTime.Now;
                        pro.registroActivo = false;
                        db.SaveChanges();
                    }

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                    LogError(0, 0, NombreControlador, "EliminarFacultamiento", e, AccionEnum.ACTUALIZAR, 0, facultamiento_id);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> GetFacultamiento(int facultamiento_id)
        {
            try
            {
                var registroFacultamiento = db.tblCOES_Facultamiento.FirstOrDefault(x => x.id == facultamiento_id);
                var listaProyectos = db.tblCOES_Facultamiento_CentroCosto.Where(x => x.registroActivo && x.facultamiento_id == facultamiento_id).ToList().Join(
                    _context.tblP_CC.Where(x => x.estatus).ToList(),
                    rel => rel.cc,
                    centroCosto => centroCosto.cc,
                    (rel, centroCosto) => new { rel = rel, centroCosto = centroCosto }
                ).ToList();

                var data = new
                {
                    facultamiento_id = registroFacultamiento.id,
                    usuario_id = registroFacultamiento.usuario_id,
                    facultamientoNombre = _context.tblP_Usuario.Where(y => y.id == registroFacultamiento.usuario_id).Select(z => z.nombre + " " + z.apellidoPaterno + " " + z.apellidoMaterno).FirstOrDefault(),
                    proyectos = listaProyectos.Select(x => x.rel.cc).ToList(),
                    tipo = (int)registroFacultamiento.tipo
                };

                resultado.Add("data", data);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                LogError(0, 0, NombreControlador, "GetFacultamiento", e, AccionEnum.CONSULTA, 0, facultamiento_id);
            }

            return resultado;
        }
        #endregion

        #region Firmas Subcontratistas
        public Dictionary<string, object> GetFirmaSubcontratistas(int subcontratista_id)
        {
            try
            {
                var listaFirmaSubcontratistas = db.tblCOES_FirmaSubcontratista.Where(x => x.registroActivo).ToList();

                if (subcontratista_id > 0)
                {
                    listaFirmaSubcontratistas = listaFirmaSubcontratistas.Where(x => x.subcontratista_id == subcontratista_id).ToList();
                }
                var listaContratos = db.tblCOES_FirmaSubcontratistatblX_Contrato.Where(x => x.registroActivo).ToList().Join(
                    db.tblX_Contrato.Where(x => x.estatus).ToList(),
                    rel => rel.contrato_id,
                    con => con.id,
                    (rel, con) => new { rel, con }
                ).ToList();
                var listaSubcontratistas = db.tblX_SubContratista.Where(x => x.estatus).ToList();

                var data = listaFirmaSubcontratistas.Select(x => new
                {
                    firma_id = x.id,
                    subcontratista_id = x.subcontratista_id,
                    subcontratistaDesc = listaSubcontratistas.Where(y => y.id == x.subcontratista_id).Select(z => z.nombre).FirstOrDefault(),
                    nombre = x.nombre,
                    correo = x.correo,
                    contratos = string.Join(",", listaContratos.Where(y => y.rel.firma_id == x.id).Select(z => "[" + z.con.cc + "] " + z.con.numeroContrato).ToList())
                }).ToList();

                resultado.Add("data", data);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                LogError(0, 0, NombreControlador, "GetFirmaSubcontratistas", e, AccionEnum.CONSULTA, 0, subcontratista_id);
            }

            return resultado;
        }

        public Dictionary<string, object> GuardarNuevaFirmaSubcontratista(tblCOES_FirmaSubcontratista firma, List<tblCOES_FirmaSubcontratistatblX_Contrato> contratos)
        {
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    firma.usuarioCreacion_id = vSesiones.sesionUsuarioDTO.id;
                    firma.fechaCreacion = DateTime.Now;
                    firma.registroActivo = true;

                    db.tblCOES_FirmaSubcontratista.Add(firma);
                    db.SaveChanges();

                    foreach (var con in contratos)
                    {
                        con.firma_id = firma.id;
                        con.usuarioCreacion_id = vSesiones.sesionUsuarioDTO.id;
                        con.fechaCreacion = DateTime.Now;
                        con.registroActivo = true;
                        db.tblCOES_FirmaSubcontratistatblX_Contrato.Add(con);
                        db.SaveChanges();
                    }

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                    LogError(0, 0, NombreControlador, "GuardarNuevaFirmaSubcontratista", e, AccionEnum.AGREGAR, 0, new { firma = firma, contratos = contratos });
                }
            }

            return resultado;
        }

        public Dictionary<string, object> EditarFirmaSubcontratista(tblCOES_FirmaSubcontratista firma, List<tblCOES_FirmaSubcontratistatblX_Contrato> contratos)
        {
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    var registroFirma = db.tblCOES_FirmaSubcontratista.FirstOrDefault(x => x.id == firma.id);

                    registroFirma.subcontratista_id = firma.subcontratista_id;
                    registroFirma.nombre = firma.nombre;
                    registroFirma.correo = firma.correo;
                    registroFirma.usuarioModificacion_id = vSesiones.sesionUsuarioDTO.id;
                    registroFirma.fechaModificacion = DateTime.Now;
                    db.SaveChanges();

                    var listaContratosAnteriores = db.tblCOES_FirmaSubcontratistatblX_Contrato.Where(x => x.registroActivo && x.firma_id == registroFirma.id).ToList();

                    foreach (var con in listaContratosAnteriores)
                    {
                        con.usuarioModificacion_id = vSesiones.sesionUsuarioDTO.id;
                        con.fechaModificacion = DateTime.Now;
                        con.registroActivo = false;
                        db.SaveChanges();
                    }

                    foreach (var con in contratos)
                    {
                        con.firma_id = registroFirma.id;
                        con.usuarioCreacion_id = vSesiones.sesionUsuarioDTO.id;
                        con.fechaCreacion = DateTime.Now;
                        con.registroActivo = true;
                        db.tblCOES_FirmaSubcontratistatblX_Contrato.Add(con);
                        db.SaveChanges();
                    }

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                    LogError(0, 0, NombreControlador, "EditarFirmaSubcontratista", e, AccionEnum.ACTUALIZAR, 0, new { firma = firma, contratos = contratos });
                }
            }

            return resultado;
        }

        public Dictionary<string, object> EliminarFirmaSubcontratista(int firma_id)
        {
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    var registroFirma = db.tblCOES_FirmaSubcontratista.FirstOrDefault(x => x.id == firma_id);

                    registroFirma.usuarioModificacion_id = vSesiones.sesionUsuarioDTO.id;
                    registroFirma.fechaModificacion = DateTime.Now;
                    registroFirma.registroActivo = false;
                    db.SaveChanges();

                    var listaContratosAnteriores = db.tblCOES_FirmaSubcontratistatblX_Contrato.Where(x => x.registroActivo && x.firma_id == registroFirma.id).ToList();

                    foreach (var con in listaContratosAnteriores)
                    {
                        con.usuarioModificacion_id = vSesiones.sesionUsuarioDTO.id;
                        con.fechaModificacion = DateTime.Now;
                        con.registroActivo = false;
                        db.SaveChanges();
                    }

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                    LogError(0, 0, NombreControlador, "EliminarFirmaSubcontratista", e, AccionEnum.ACTUALIZAR, 0, firma_id);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> GetFirmaSubcontratista(int firma_id)
        {
            try
            {
                var registroFirma = db.tblCOES_FirmaSubcontratista.FirstOrDefault(x => x.id == firma_id);
                var listaContratos = db.tblCOES_FirmaSubcontratistatblX_Contrato.Where(x => x.registroActivo && x.firma_id == firma_id).Select(x => x.contrato_id).ToList();

                var data = new
                {
                    id = registroFirma.id,
                    subcontratista_id = registroFirma.subcontratista_id,
                    nombre = registroFirma.nombre,
                    correo = registroFirma.correo,
                    contratos = listaContratos
                };

                resultado.Add("data", data);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                LogError(0, 0, NombreControlador, "GetFirmaSubcontratista", e, AccionEnum.CONSULTA, 0, firma_id);
            }

            return resultado;
        }

        public Dictionary<string, object> FillComboSubcontratistas()
        {
            resultado = new Dictionary<string, object>();

            try
            {
                var listaSubcontratistas = db.tblX_SubContratista.Where(x => x.estatus).ToList().Select(x => new ComboDTO
                {
                    Value = x.id.ToString(),
                    Text = x.nombre
                }).ToList();

                resultado.Add(ITEMS, listaSubcontratistas);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "FillComboSubcontratistas", e, AccionEnum.FILLCOMBO, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }

        public Dictionary<string, object> EnviarCorreoNotificacionFirma(int firma_id)
        {
            resultado = new Dictionary<string, object>();

            try
            {
                var correos = new List<string>();
                var registroFirma = db.tblCOES_FirmaSubcontratista.FirstOrDefault(x => x.id == firma_id);

                correos.Add(registroFirma.correo);

                var facultamientosAdministrador = db.tblCOES_Facultamiento.Where(x => x.registroActivo && (x.tipo == TipoFacultamientoEnum.ADMINISTRADOR_PMO || x.tipo == TipoFacultamientoEnum.ADMINISTRADOR)).ToList();
                var facultamientosAdministrador_usuario_id = facultamientosAdministrador.Select(x => x.usuario_id).ToList();
                var usuariosCorreos = _context.tblP_Usuario.Where(x => x.estatus && facultamientosAdministrador_usuario_id.Contains(x.id)).Select(x => x.correo).ToList();

                correos.AddRange(usuariosCorreos);

#if DEBUG
                correos = new List<string> { "oscar.valencia@construplan.com.mx" };
#endif

                var mensaje = @"
                    <h3>Estás recibiendo este email debido a que se ha registrado tu correo como firmante de las evaluaciones ligadas a contratista.</h3><br><br>
                    Favor de no contestar este correo ya que es generado de forma automática por nuestros sistemas de cómputo.<br><br><br><br><br><br>
                    Enviado automáticamente por http://expediente.construplan.com.mx/
                ";

                var correoEnviado = GlobalUtils.sendEmail(string.Format("{0}: Se ha asignado como firmante - SIGOPLAN CONSTRUPLAN", PersonalUtilities.GetNombreEmpresa()), mensaje, correos);

                resultado.Add(SUCCESS, correoEnviado);
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "EnviarCorreoNotificacionFirma", e, AccionEnum.CORREO, 0, 0);
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }
        #endregion

        #region Firmas Gerentes
        public Dictionary<string, object> GetFirmaGerentes(string cc)
        {
            try
            {
                var listaFirmaGerentes = db.tblCOES_FirmaGerente.Where(x => x.registroActivo).ToList();

                if (cc != null && cc != "")
                {
                    listaFirmaGerentes = listaFirmaGerentes.Where(x => x.cc == cc).ToList();
                }

                var listaUsuarios = _context.tblP_Usuario.Where(x => x.estatus).ToList();
                var listaCentroCostos = _context.tblP_CC.Where(x => x.estatus).ToList();

                var data = listaFirmaGerentes.Select(x => new
                {
                    firma_id = x.id,
                    usuario_id = x.usuario_id,
                    nombre = listaUsuarios.Where(y => y.id == x.usuario_id).Select(z => z.nombre + " " + z.apellidoPaterno + " " + z.apellidoMaterno).FirstOrDefault(),
                    cc = x.cc,
                    ccDesc = listaCentroCostos.Where(y => y.cc == x.cc).Select(z => "[" + z.cc + "] " + z.descripcion).FirstOrDefault()
                }).ToList();

                resultado.Add("data", data);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                LogError(0, 0, NombreControlador, "GetFirmaGerentes", e, AccionEnum.CONSULTA, 0, cc);
            }

            return resultado;
        }

        public Dictionary<string, object> GuardarNuevaFirmaGerente(tblCOES_FirmaGerente firma)
        {
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    firma.usuarioCreacion_id = vSesiones.sesionUsuarioDTO.id;
                    firma.fechaCreacion = DateTime.Now;
                    firma.registroActivo = true;

                    db.tblCOES_FirmaGerente.Add(firma);
                    db.SaveChanges();

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                    LogError(0, 0, NombreControlador, "GuardarNuevaFirmaGerente", e, AccionEnum.AGREGAR, 0, firma);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> EditarFirmaGerente(tblCOES_FirmaGerente firma)
        {
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    var registroFirma = db.tblCOES_FirmaGerente.FirstOrDefault(x => x.id == firma.id);

                    registroFirma.usuario_id = firma.usuario_id;
                    registroFirma.cc = firma.cc;
                    registroFirma.usuarioModificacion_id = vSesiones.sesionUsuarioDTO.id;
                    registroFirma.fechaModificacion = DateTime.Now;
                    db.SaveChanges();

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                    LogError(0, 0, NombreControlador, "EditarFirmaGerente", e, AccionEnum.ACTUALIZAR, 0, firma);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> EliminarFirmaGerente(int firma_id)
        {
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    var registroFirma = db.tblCOES_FirmaGerente.FirstOrDefault(x => x.id == firma_id);

                    registroFirma.usuarioModificacion_id = vSesiones.sesionUsuarioDTO.id;
                    registroFirma.fechaModificacion = DateTime.Now;
                    registroFirma.registroActivo = false;
                    db.SaveChanges();

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                    LogError(0, 0, NombreControlador, "EliminarFirmaGerente", e, AccionEnum.ACTUALIZAR, 0, firma_id);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> GetFirmaGerente(int firma_id)
        {
            try
            {
                var registroFirma = db.tblCOES_FirmaGerente.FirstOrDefault(x => x.id == firma_id);
                var registroUsuario = _context.tblP_Usuario.FirstOrDefault(x => x.id == registroFirma.usuario_id);

                var data = new
                {
                    id = registroFirma.id,
                    usuario_id = registroFirma.usuario_id,
                    nombre = registroUsuario.nombre + " " + registroUsuario.apellidoPaterno + " " + registroUsuario.apellidoMaterno,
                    cc = registroFirma.cc
                };

                resultado.Add("data", data);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                LogError(0, 0, NombreControlador, "GetFirmaGerente", e, AccionEnum.CONSULTA, 0, firma_id);
            }

            return resultado;
        }
        #endregion

        #region Especialidades
        public Dictionary<string, object> FillComboEspecialidades()
        {
            resultado = new Dictionary<string, object>();

            try
            {
                var listaEspecialidades = db.tblCOES_Especialidad.Where(x => x.registroActivo).ToList().Select(x => new ComboDTO
                {
                    Value = x.id.ToString(),
                    Text = x.descripcion
                }).ToList();

                resultado.Add(ITEMS, listaEspecialidades);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "FillComboSubcontratistas", e, AccionEnum.FILLCOMBO, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }

        public Dictionary<string, object> GetSubcontratistasEspecialidad(string cc)
        {
            resultado = new Dictionary<string, object>();

            try
            {
                var listaEspecialidades = db.tblCOES_EspecialidadtblX_SubContratista.Where(x => x.registroActivo).ToList();
                var listaSubcontratistas = db.tblX_SubContratista.Where(x => x.estatus).ToList().Select(x => new
                {
                    id = x.id,
                    nombre = x.nombre,
                    especialidades = listaEspecialidades.Where(y => y.subcontratista_id == x.id).Select(z => z.especialidad_id).ToList()
                }).ToList();

                if (cc != "" && cc != null)
                {
                    var listaContratosActivos = db.tblX_Contrato.Where(x => x.estatus && x.estatusContrato.id == 1 && x.cc == cc).Select(x => x.subcontratistaID).ToList();

                    listaSubcontratistas = listaSubcontratistas.Where(x => listaContratosActivos.Contains(x.id)).ToList();
                }

                resultado.Add("data", listaSubcontratistas);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "GetSubcontratistasEspecialidad", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }

        public Dictionary<string, object> GuardarEspecialidadesSubcontratista(int subcontratista_id, List<tblCOES_EspecialidadtblX_SubContratista> especialidades)
        {
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    var listaEspecialidadesAnteriores = db.tblCOES_EspecialidadtblX_SubContratista.Where(x => x.registroActivo && x.subcontratista_id == subcontratista_id).ToList();

                    foreach (var esp in listaEspecialidadesAnteriores)
                    {
                        esp.usuarioModificacion_id = vSesiones.sesionUsuarioDTO.id;
                        esp.fechaModificacion = DateTime.Now;
                        esp.registroActivo = false;
                        db.SaveChanges();
                    }

                    if (especialidades != null && especialidades.Count() > 0)
                    {
                        foreach (var esp in especialidades)
                        {
                            esp.usuarioCreacion_id = vSesiones.sesionUsuarioDTO.id;
                            esp.fechaCreacion = DateTime.Now;
                            esp.registroActivo = true;
                            db.tblCOES_EspecialidadtblX_SubContratista.Add(esp);
                            db.SaveChanges();
                        }
                    }

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                    LogError(0, 0, NombreControlador, "GuardarEspecialidadesSubcontratista", e, AccionEnum.AGREGAR, 0, especialidades);
                }
            }

            return resultado;
        }
        #endregion

        #region Administración Evaluaciones
        public Dictionary<string, object> CargarEvaluacionesSubcontratistas(string cc, int subcontratista_id, EstatusEvaluacionEnum estatus)
        {
            try
            {
                if (estatus == EstatusEvaluacionEnum.PENDIENTE_POR_ASIGNAR || estatus == EstatusEvaluacionEnum.EVALUACION_ASIGNADA || estatus == EstatusEvaluacionEnum.ESTATUS_DE_EVALUACION)
                {
                    #region PENDIENTE POR ASIGNAR - EVALUACIÓN ASIGNADA - ESTATUS DE EVALUACIÓN
                    var listaContratos = db.tblX_Contrato.Where(x => x.estatus).ToList().Where(x => x.fechaVigencia != null ? ((DateTime)x.fechaVigencia).Date >= DateTime.Now.Date : false).ToList();
                    var listaAsignaciones = db.tblCOES_Asignacion.Where(x => x.registroActivo).ToList();
                    var listaSubcontratistas = db.tblX_SubContratista.Where(x => x.estatus).ToList();
                    var listaCentroCosto = _context.tblP_CC.Where(x => x.estatus).ToList();
                    var data = new List<ContratoEvaluacionDTO>();

                    foreach (var con in listaContratos)
                    {
                        var asignacion = listaAsignaciones.FirstOrDefault(x => x.contrato_id == con.id);
                        var subcontratista = listaSubcontratistas.FirstOrDefault(x => x.id == con.subcontratistaID);
                        var centroCosto = listaCentroCosto.FirstOrDefault(x => x.cc == con.cc);

                        if (asignacion != null)
                        {
                            var evaluacionesAsignacion = db.tblCOES_Asignacion_Evaluacion.Where(x => x.registroActivo && x.asignacion_id == asignacion.id).ToList();

                            foreach (var eva in evaluacionesAsignacion)
                            {
                                var listaEvidencias = db.tblCOES_Evidencia.Where(x => x.registroActivo && x.evaluacion_id == eva.id).ToList();
                                var fechaCargaLimite = eva.fecha.AddDays(2);
                                var evaluacionFirmada = db.tblCOES_Firma.Where(x => x.registroActivo && x.evaluacion_id == eva.id && x.estadoFirma == EstadoFirmaEnum.AUTORIZADO).ToList().Count() == 2;
                                var registroCambioEvaluacion = db.tblCOES_CambioEvaluacion.FirstOrDefault(x => x.registroActivo && x.evaluacion_id == eva.id && x.estatus == EstatusCambioEvaluacionEnum.PENDIENTE);

                                #region String Carga Soportes
                                var cargaSoportes = "";

                                if (listaEvidencias.Count() == 0)
                                {
                                    //var hoy = DateTime.Now.Date;

                                    //if (fechaCargaLimite.Date >= hoy.Date)
                                    //{
                                    //    cargaSoportes = "CARGA PENDIENTE";
                                    //}
                                    //else
                                    //{
                                    //    var diferenciaDias = (hoy.Date - fechaCargaLimite.Date).TotalDays;

                                    //    cargaSoportes = string.Format("FUERA DE TIEMPO \"{0} DÍAS\"", diferenciaDias);
                                    //}
                                }
                                else
                                {
                                    var ultimaEvidenciaCargada = listaEvidencias.Max(x => x.fechaCreacion);

                                    if (fechaCargaLimite.Date >= ultimaEvidenciaCargada.Date)
                                    {
                                        cargaSoportes = "EN TIEMPO \"OK\"";
                                    }
                                    else
                                    {
                                        var diferenciaDias = (ultimaEvidenciaCargada.Date - fechaCargaLimite.Date).TotalDays;

                                        cargaSoportes = string.Format("FUERA DE TIEMPO \"{0} DÍAS\"", diferenciaDias);
                                    }
                                }
                                #endregion

                                #region String Estatus Firmas
                                var estatusFirmas = "";
                                var firmaSubcontratista = db.tblCOES_Firma.FirstOrDefault(x => x.registroActivo && x.evaluacion_id == eva.id && x.tipo == TipoFirmanteEnum.SUBCONTRATISTA);
                                var firmaGerente = db.tblCOES_Firma.FirstOrDefault(x => x.registroActivo && x.evaluacion_id == eva.id && x.tipo == TipoFirmanteEnum.GERENTE);

                                if (firmaSubcontratista != null && firmaGerente != null)
                                {
                                    if (firmaSubcontratista.estadoFirma == EstadoFirmaEnum.AUTORIZADO && firmaGerente.estadoFirma == EstadoFirmaEnum.AUTORIZADO)
                                    {
                                        estatusFirmas = "AUTORIZADA";
                                    }
                                    else
                                    {
                                        estatusFirmas = "PENDIENTE";
                                    }
                                }
                                #endregion

                                data.Add(new ContratoEvaluacionDTO
                                {
                                    contrato_id = con.id,
                                    asignacion_id = asignacion.id,
                                    evaluacion_id = eva.id,
                                    numeroContrato = con.numeroContrato,
                                    cc = con.cc,
                                    ccDesc = centroCosto != null ? ("[" + centroCosto.cc + "] " + centroCosto.descripcion) : "",
                                    subcontratista_id = con.subcontratistaID,
                                    subcontratistaDesc = subcontratista != null ? subcontratista.nombre : "",
                                    periodoEvaluable = asignacion.fechaInicial.ToShortDateString() + " - " + asignacion.fechaFinal.ToShortDateString(),
                                    fechaEvaluacion = eva.fecha,
                                    fechaEvaluacionString = eva.fecha.ToShortDateString(),
                                    cargaSoportes = cargaSoportes,
                                    estatusEvaluacion = eva.estatus,
                                    estatusFirmas = estatusFirmas,
                                    evaluacionFirmada = evaluacionFirmada,
                                    cambioEvaluacion_id = registroCambioEvaluacion != null ? registroCambioEvaluacion.id : 0,
                                    flagGestionFirmas = listaEvidencias.Count() > 0
                                });
                            }
                        }
                        else
                        {
                            data.Add(new ContratoEvaluacionDTO
                            {
                                contrato_id = con.id,
                                asignacion_id = 0,
                                numeroContrato = con.numeroContrato,
                                cc = con.cc,
                                ccDesc = centroCosto != null ? ("[" + centroCosto.cc + "] " + centroCosto.descripcion) : "",
                                subcontratista_id = con.subcontratistaID,
                                subcontratistaDesc = subcontratista != null ? subcontratista.nombre : "",
                                periodoEvaluable = "",
                                fechaEvaluacion = null,
                                fechaEvaluacionString = "",
                                cargaSoportes = "",
                                estatusEvaluacion = EstatusEvaluacionEnum.PENDIENTE_POR_ASIGNAR,
                                estatusFirmas = "",
                                evaluacionFirmada = false,
                                cambioEvaluacion_id = 0,
                                flagGestionFirmas = false
                            });
                        }
                    }

                    #region Filtros
                    if (cc != "" && cc != null)
                    {
                        data = data.Where(x => x.cc == cc).ToList();
                    }

                    if (subcontratista_id > 0)
                    {
                        data = data.Where(x => x.subcontratista_id == subcontratista_id).ToList();
                    }
                    #endregion

                    var pendientesAsignar = data.Where(x => x.asignacion_id == 0).ToList().Count();
                    var asignadas = data.Where(x => x.asignacion_id > 0).ToList().GroupBy(x => x.asignacion_id).ToList().Count();
                    decimal autorizadas = data.Where(x => x.evaluacionFirmada).ToList().Count();
                    var cumplimiento = autorizadas > 0 ? ((Math.Truncate(100 * (autorizadas * 100 / (pendientesAsignar > 0 ? pendientesAsignar : 1))) / 100).ToString() + "%") : "0%";

                    data = data.Where(x => x.estatusEvaluacion == estatus).ToList();

                    #region Facultamientos Centros de Costo
                    var facultamiento = db.tblCOES_Facultamiento.FirstOrDefault(x => x.registroActivo && x.usuario_id == vSesiones.sesionUsuarioDTO.id);

                    if (facultamiento != null)
                    {
                        if (facultamiento.tipo != TipoFacultamientoEnum.ADMINISTRADOR_PMO) //Los administradores PMO ven todos los centros de costos.
                        {
                            var listaCentroCostoFacultamiento = db.tblCOES_Facultamiento_CentroCosto.Where(x => x.registroActivo && x.facultamiento_id == facultamiento.id).Select(x => x.cc).ToList();

                            data = data.Where(x => listaCentroCostoFacultamiento.Contains(x.cc)).ToList();
                        }
                    }
                    else
                    {
                        data = new List<ContratoEvaluacionDTO>();
                    }
                    #endregion

                    resultado.Add("data", data);
                    resultado.Add("pendientesAsignar", pendientesAsignar);
                    resultado.Add("asignadas", asignadas);
                    resultado.Add("autorizadas", autorizadas);
                    resultado.Add("cumplimiento", cumplimiento);
                    #endregion
                }
                else if (estatus == EstatusEvaluacionEnum.COMPROMISOS)
                {
                    #region COMPROMISOS
                    var listaContratos = db.tblX_Contrato.Where(x => x.estatus).ToList().Where(x => x.fechaVigencia != null ? ((DateTime)x.fechaVigencia).Date >= DateTime.Now.Date : false).ToList();
                    var listaAsignaciones = db.tblCOES_Asignacion.Where(x => x.registroActivo).ToList();
                    var listaSubcontratistas = db.tblX_SubContratista.Where(x => x.estatus).ToList();
                    var listaCentroCosto = _context.tblP_CC.Where(x => x.estatus).ToList();
                    var data = new List<ContratoEvaluacionDTO>();
                    var listaRequerimientos = new List<RequerimientoDTO>();

                    foreach (var con in listaContratos)
                    {
                        var asignacion = listaAsignaciones.FirstOrDefault(x => x.contrato_id == con.id);
                        var subcontratista = listaSubcontratistas.FirstOrDefault(x => x.id == con.subcontratistaID);
                        var centroCosto = listaCentroCosto.FirstOrDefault(x => x.cc == con.cc);

                        if (asignacion != null)
                        {
                            var evaluacionesAsignacion = db.tblCOES_Asignacion_Evaluacion.Where(x => x.registroActivo && x.asignacion_id == asignacion.id).ToList();

                            foreach (var eva in evaluacionesAsignacion)
                            {
                                var listaEvidenciasCompromiso = db.tblCOES_Evidencia.Where(x => x.registroActivo && x.evaluacion_id == eva.id && x.tipo == TipoEvidenciaEnum.COMPROMISO).ToList();

                                foreach (var evi in listaEvidenciasCompromiso)
                                {
                                    data.Add(new ContratoEvaluacionDTO
                                    {
                                        contrato_id = con.id,
                                        asignacion_id = asignacion.id,
                                        evaluacion_id = eva.id,
                                        evidencia_id = evi.id,
                                        numeroContrato = con.numeroContrato,
                                        cc = con.cc,
                                        ccDesc = centroCosto != null ? ("[" + centroCosto.cc + "] " + centroCosto.descripcion) : "",
                                        subcontratista_id = con.subcontratistaID,
                                        subcontratistaDesc = subcontratista != null ? subcontratista.nombre : "",
                                        periodoEvaluable = asignacion.fechaInicial.ToShortDateString() + " - " + asignacion.fechaFinal.ToShortDateString(),
                                        fechaEvaluacion = eva.fecha,
                                        fechaEvaluacionString = eva.fecha.ToShortDateString(),
                                        estatusEvaluacion = EstatusEvaluacionEnum.COMPROMISOS,
                                        comentarioEvaluacion = evi.comentarioEvaluacion,
                                        planAccion = evi.planAccion,
                                        responsable = evi.responsable,
                                        fechaCompromiso = evi.fechaCompromiso,
                                        fechaCompromisoString = evi.fechaCompromiso != null ? ((DateTime)evi.fechaCompromiso).ToShortDateString() : "",
                                        calificacion = evi.calificacion,
                                        ponderacion = evi.ponderacion,
                                        estatusEvidencia = evi.estatus
                                    });
                                }

                                #region Indicadores Requerimientos
                                var relacionPlantillaContrato = db.tblCOES_PlantillatblX_Contrato.FirstOrDefault(x => x.registroActivo && x.contrato_id == con.id);

                                if (relacionPlantillaContrato != null)
                                {
                                    var listaRelacionPlantillaElemento = db.tblCOES_PlantillatblCOES_Elemento.Where(x => x.registroActivo && x.plantilla_id == relacionPlantillaContrato.plantilla_id).ToList();

                                    foreach (var relPE in listaRelacionPlantillaElemento)
                                    {
                                        var listaRelacionPlantillaElementoRequerimiento = db.tblCOES_PlantillatblCOES_ElementotblCOES_Requerimiento.Where(x => x.registroActivo && x.relacionPlantillaElemento_id == relPE.id).ToList();

                                        foreach (var relPER in listaRelacionPlantillaElementoRequerimiento)
                                        {
                                            var listaEvidencias = db.tblCOES_Evidencia.Where(x => x.evaluacion_id == eva.id && x.requerimiento_id == relPER.requerimiento_id).ToList();
                                            var requerimiento = new RequerimientoDTO
                                            {
                                                evaluacion_id = eva.id,
                                                requerimiento_id = relPER.requerimiento_id,
                                                elemento_id = relPE.elemento_id,
                                                evidencias = new List<EvidenciaDTO>(),
                                                cc = con.cc,
                                                subcontratista_id = con.subcontratistaID
                                            };

                                            foreach (var evi in listaEvidencias)
                                            {
                                                requerimiento.evidencias.Add(new EvidenciaDTO
                                                {
                                                    evidencia_id = evi.id,
                                                    evaluacion_id = evi.evaluacion_id,
                                                    requerimiento_id = evi.requerimiento_id,
                                                    rutaArchivo = evi.rutaArchivo,
                                                    tipo = evi.tipo,
                                                    tipoDesc = evi.tipo.GetDescription(),
                                                    evidenciaInicial_id = evi.evidenciaInicial_id,
                                                    estatus = evi.estatus,
                                                    estatusDesc = evi.estatus.GetDescription(),
                                                    calificacion = evi.calificacion,
                                                    ponderacion = evi.ponderacion,
                                                    comentarioEvaluacion = evi.comentarioEvaluacion,
                                                    planAccion = evi.planAccion,
                                                    responsable = evi.responsable,
                                                    fechaCompromiso = evi.fechaCompromiso,
                                                    fechaCompromisoString = evi.fechaCompromiso != null ? ((DateTime)evi.fechaCompromiso).ToShortDateString() : ""
                                                });
                                            }

                                            if (requerimiento.evidencias.Count() > 0)
                                            {
                                                var ultimaEvidencia = requerimiento.evidencias.Last();
                                                var ultimaEvidenciaInicial = requerimiento.evidencias.Where(x => x.tipo == TipoEvidenciaEnum.INICIAL).ToList().Last();

                                                requerimiento.estatusUltimaEvidencia = ultimaEvidencia.estatus;
                                                requerimiento.estatusUltimaEvidenciaDesc = ultimaEvidencia.estatusDesc;
                                                requerimiento.estatusUltimaEvidenciaInicial = ultimaEvidenciaInicial.estatus;
                                                requerimiento.estatusUltimaEvidenciaInicialDesc = ultimaEvidenciaInicial.estatusDesc;
                                                requerimiento.calificacionUltimaEvidencia = ultimaEvidencia.calificacion;
                                                requerimiento.ponderacionUltimaEvidencia = ultimaEvidencia.ponderacion;
                                                requerimiento.nombreUltimaEvidencia = Path.GetFileName(ultimaEvidencia.rutaArchivo);
                                            }

                                            listaRequerimientos.Add(requerimiento);
                                        }
                                    }
                                }
                                #endregion
                            }
                        }
                    }

                    #region Filtros
                    if (cc != "" && cc != null)
                    {
                        data = data.Where(x => x.cc == cc).ToList();
                        listaRequerimientos = listaRequerimientos.Where(x => x.cc == cc).ToList();
                    }

                    if (subcontratista_id > 0)
                    {
                        data = data.Where(x => x.subcontratista_id == subcontratista_id).ToList();
                        listaRequerimientos = listaRequerimientos.Where(x => x.subcontratista_id == subcontratista_id).ToList();
                    }
                    #endregion

                    var totalRequerimientosEvaluados = listaRequerimientos.Count();
                    var requerimientosAprobados = listaRequerimientos.Where(x => x.estatusUltimaEvidencia == EstatusEvidenciaEnum.APROBADO).Count();
                    var requerimientosNoAprobados = listaRequerimientos.Where(x => x.estatusUltimaEvidencia != EstatusEvidenciaEnum.APROBADO).Count();
                    var totalCompromisos = data.Count();
                    var promedioCumplimiento = data.Count() > 0 ? (Math.Truncate(100 * data.Average(x => x.calificacion)) / 100) : 0;

                    data = data.Where(x => x.estatusEvaluacion == estatus).ToList();

                    #region Facultamientos Centros de Costo
                    var facultamiento = db.tblCOES_Facultamiento.FirstOrDefault(x => x.registroActivo && x.usuario_id == vSesiones.sesionUsuarioDTO.id);

                    if (facultamiento != null)
                    {
                        if (facultamiento.tipo != TipoFacultamientoEnum.ADMINISTRADOR_PMO) //Los administradores PMO ven todos los centros de costos.
                        {
                            var listaCentroCostoFacultamiento = db.tblCOES_Facultamiento_CentroCosto.Where(x => x.registroActivo && x.facultamiento_id == facultamiento.id).Select(x => x.cc).ToList();

                            data = data.Where(x => listaCentroCostoFacultamiento.Contains(x.cc)).ToList();
                        }
                    }
                    else
                    {
                        data = new List<ContratoEvaluacionDTO>();
                    }
                    #endregion

                    resultado.Add("data", data);
                    resultado.Add("totalRequerimientosEvaluados", totalRequerimientosEvaluados);
                    resultado.Add("requerimientosAprobados", requerimientosAprobados);
                    resultado.Add("requerimientosNoAprobados", requerimientosNoAprobados);
                    resultado.Add("totalCompromisos", totalCompromisos);
                    resultado.Add("promedioCumplimiento", promedioCumplimiento + "%");
                    #endregion
                }

                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                LogError(0, 0, NombreControlador, "CargarEvaluacionesSubcontratistas", e, AccionEnum.CONSULTA, 0, new { cc = cc, subcontratista_id = subcontratista_id, estatus = estatus });
            }

            return resultado;
        }

        public Dictionary<string, object> GuardarAsignacionEvaluacion(tblCOES_Asignacion asignacion, List<tblCOES_Asignacion_Evaluacion> evaluaciones)
        {
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    asignacion.usuarioCreacion_id = vSesiones.sesionUsuarioDTO.id;
                    asignacion.fechaCreacion = DateTime.Now;
                    asignacion.registroActivo = true;

                    db.tblCOES_Asignacion.Add(asignacion);
                    db.SaveChanges();

                    foreach (var eva in evaluaciones)
                    {
                        eva.asignacion_id = asignacion.id;
                        eva.estatus = EstatusEvaluacionEnum.EVALUACION_ASIGNADA;
                        eva.usuarioCreacion_id = vSesiones.sesionUsuarioDTO.id;
                        eva.fechaCreacion = DateTime.Now;
                        eva.registroActivo = true;
                        db.tblCOES_Asignacion_Evaluacion.Add(eva);
                        db.SaveChanges();
                    }

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                    LogError(0, 0, NombreControlador, "GuardarAsignacionEvaluacion", e, AccionEnum.AGREGAR, 0, new { asignacion = asignacion, evaluaciones = evaluaciones });
                }
            }

            return resultado;
        }

        public Dictionary<string, object> GetContratoInformacion(int contrato_id)
        {
            try
            {
                var contrato = db.tblX_Contrato.FirstOrDefault(x => x.id == contrato_id);
                var listaEspecialidades = db.tblCOES_Especialidad.Where(x => x.registroActivo).ToList();
                var especialidadesSubcontratista = db.tblCOES_EspecialidadtblX_SubContratista.Where(x => x.registroActivo && x.subcontratista_id == contrato.subcontratistaID).ToList().Join(
                    listaEspecialidades,
                    rel => rel.especialidad_id,
                    esp => esp.id,
                    (rel, esp) => new { esp.descripcion }
                ).ToList();
                var data = new
                {
                    especialidad = string.Join(", ", especialidadesSubcontratista.Select(x => x.descripcion).ToList()),
                    fechaInicialString = contrato.fechaInicial != null ? ((DateTime)contrato.fechaInicial).ToShortDateString() : "",
                    fechaFinalString = contrato.fechaFinal != null ? ((DateTime)contrato.fechaFinal).ToShortDateString() : ""
                };

                resultado.Add("data", data);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                LogError(0, 0, NombreControlador, "GetContratoInformacion", e, AccionEnum.CONSULTA, 0, contrato_id);
            }

            return resultado;
        }

        public Dictionary<string, object> GetElementosEvaluacion(int contrato_id, int evaluacion_id)
        {
            try
            {
                //var usuario = _context.tblP_Usuario.FirstOrDefault(x => x.id == vSesiones.sesionUsuarioDTO.id);

                var relacionPlantillaContrato = db.tblCOES_PlantillatblX_Contrato.FirstOrDefault(x => x.registroActivo && x.contrato_id == contrato_id);

                if (relacionPlantillaContrato == null)
                {
                    throw new Exception("No hay una plantilla asignada a ese contrato.");
                }

                var listaElementosPlantilla = db.tblCOES_PlantillatblCOES_Elemento.Where(x => x.registroActivo && x.plantilla_id == relacionPlantillaContrato.plantilla_id).ToList();

                if (listaElementosPlantilla.Count() == 0)
                {
                    throw new Exception("La plantilla asignada no contiene elementos.");
                }

                var listaElementosPlantilla_elemento_id = listaElementosPlantilla.Select(x => x.elemento_id).ToList();
                var listaElementos = db.tblCOES_Elemento.Where(x => x.registroActivo == listaElementosPlantilla_elemento_id.Contains(x.id)).ToList();
                var listaElementosPlantilla_id = listaElementosPlantilla.Select(x => x.id).ToList();
                var listaRequerimientosPlantilla = db.tblCOES_PlantillatblCOES_ElementotblCOES_Requerimiento.Where(x => x.registroActivo && listaElementosPlantilla_id.Contains(x.relacionPlantillaElemento_id)).ToList();
                var listaRequerimientosPlantilla_requerimiento_id = listaRequerimientosPlantilla.Select(x => x.requerimiento_id).ToList();
                var listaRequerimientos = db.tblCOES_Requerimiento.Where(x => x.registroActivo && listaRequerimientosPlantilla_requerimiento_id.Contains(x.id)).ToList();
                var registroEvaluacion = db.tblCOES_Asignacion_Evaluacion.FirstOrDefault(x => x.id == evaluacion_id);
                var data = new List<ElementoDTO>();

                foreach (var ele in listaElementosPlantilla)
                {
                    var registroElemento = listaElementos.FirstOrDefault(x => x.id == ele.elemento_id);
                    var listaRequerimientosElemento = listaRequerimientosPlantilla.Where(x => x.relacionPlantillaElemento_id == ele.id).ToList();
                    var elemento = new ElementoDTO()
                    {
                        contrato_id = contrato_id,
                        asignacion_id = registroEvaluacion.asignacion_id,
                        evaluacion_id = evaluacion_id,
                        elemento_id = ele.elemento_id,
                        clave = registroElemento.clave,
                        descripcion = registroElemento.descripcion,
                        mensaje = registroElemento.mensaje,
                        requerimientos = new List<RequerimientoDTO>()
                    };

                    foreach (var req in listaRequerimientosElemento)
                    {
                        var registroRequerimiento = listaRequerimientos.FirstOrDefault(x => x.id == req.requerimiento_id);
                        var listaEvidencias = db.tblCOES_Evidencia.Where(x => x.evaluacion_id == evaluacion_id && x.requerimiento_id == req.requerimiento_id).ToList();
                        var requerimiento = new RequerimientoDTO
                        {
                            evaluacion_id = evaluacion_id,
                            requerimiento_id = req.requerimiento_id,
                            descripcion = registroRequerimiento.descripcion,
                            elemento_id = ele.elemento_id,
                            evidencias = new List<EvidenciaDTO>()
                        };

                        tblCOES_Evidencia evidenciaAnterior = null;

                        foreach (var evi in listaEvidencias)
                        {
                            requerimiento.evidencias.Add(new EvidenciaDTO
                            {
                                evidencia_id = evi.id,
                                evaluacion_id = evi.evaluacion_id,
                                requerimiento_id = evi.requerimiento_id,
                                rutaArchivo = evi.rutaArchivo,
                                tipo = evi.tipo,
                                tipoDesc = evi.tipo.GetDescription(),
                                evidenciaInicial_id = evi.evidenciaInicial_id,
                                estatus = evi.estatus,
                                estatusDesc = evi.estatus.GetDescription(),
                                calificacion = evi.calificacion,
                                ponderacion = evi.ponderacion,
                                comentarioEvaluacion = evi.comentarioEvaluacion,
                                planAccion = evidenciaAnterior != null ? evidenciaAnterior.planAccion : evi.planAccion,
                                responsable = evidenciaAnterior != null ? evidenciaAnterior.responsable : evi.responsable,
                                fechaCompromiso = evidenciaAnterior != null ? evidenciaAnterior.fechaCompromiso : evi.fechaCompromiso,
                                fechaCompromisoString =
                                    evidenciaAnterior != null ?
                                        (evidenciaAnterior.fechaCompromiso != null ? ((DateTime)evidenciaAnterior.fechaCompromiso).ToShortDateString() : "") :
                                        (evi.fechaCompromiso != null ? ((DateTime)evi.fechaCompromiso).ToShortDateString() : "")
                            });

                            evidenciaAnterior = evi;
                        }

                        if (requerimiento.evidencias.Count() > 0)
                        {
                            var ultimaEvidencia = requerimiento.evidencias.Last();
                            var ultimaEvidenciaInicial = requerimiento.evidencias.Where(x => x.tipo == TipoEvidenciaEnum.INICIAL).ToList().Last();

                            requerimiento.estatusUltimaEvidencia = ultimaEvidencia.estatus;
                            requerimiento.estatusUltimaEvidenciaDesc = ultimaEvidencia.estatusDesc;
                            requerimiento.estatusUltimaEvidenciaInicial = ultimaEvidenciaInicial.estatus;
                            requerimiento.estatusUltimaEvidenciaInicialDesc = ultimaEvidenciaInicial.estatusDesc;
                            requerimiento.calificacionUltimaEvidencia = ultimaEvidencia.calificacion;
                            requerimiento.ponderacionUltimaEvidencia = ultimaEvidencia.ponderacion;
                            requerimiento.nombreUltimaEvidencia = Path.GetFileName(ultimaEvidencia.rutaArchivo);
                        }

                        elemento.requerimientos.Add(requerimiento);
                    }

                    data.Add(elemento);
                }

                var listaCombo = data.Select(x => new ComboDTO
                {
                    Value = x.elemento_id.ToString(),
                    Text = x.descripcion
                }).ToList();

                resultado.Add("data", data);
                resultado.Add("combo", listaCombo);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                LogError(0, 0, NombreControlador, "GetElementosEvaluacion", e, AccionEnum.CONSULTA, 0, new { contrato_id = contrato_id, evaluacion_id = evaluacion_id });
            }

            return resultado;
        }

        public Dictionary<string, object> GuardarRetroalimentacionEvaluador(tblCOES_Evidencia evidencia)
        {
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    var registroEvidencia = db.tblCOES_Evidencia.FirstOrDefault(x => x.id == evidencia.id);

                    registroEvidencia.estatus = evidencia.calificacion > 75 ? EstatusEvidenciaEnum.APROBADO : EstatusEvidenciaEnum.REPROBADO;
                    registroEvidencia.comentarioEvaluacion = evidencia.comentarioEvaluacion ?? "";
                    registroEvidencia.ponderacion = evidencia.ponderacion;
                    registroEvidencia.calificacion = evidencia.calificacion;
                    db.SaveChanges();

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                    LogError(0, 0, NombreControlador, "GuardarRetroalimentacionEvaluador", e, AccionEnum.ACTUALIZAR, 0, evidencia);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> GuardarEvaluacionSubcontratista()
        {
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {


                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                    LogError(0, 0, NombreControlador, "GuardarEvaluacionSubcontratista", e, AccionEnum.AGREGAR, 0, null);
                }
            }

            return resultado;
        }

        public tblCOES_Evidencia GetArchivoEvidencia(int evidencia_id)
        {
            return db.tblCOES_Evidencia.FirstOrDefault(x => x.id == evidencia_id);
        }

        public Dictionary<string, object> EnviarGestionFirmas(int evaluacion_id, int contrato_id, int subcontratista_id)
        {
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    #region Agregar Registros Firmantes
                    var registroContrato = db.tblX_Contrato.FirstOrDefault(x => x.id == contrato_id);
                    var registroFirmanteSubcontratista = db.tblCOES_FirmaSubcontratista.FirstOrDefault(x => x.registroActivo && x.subcontratista_id == subcontratista_id);

                    if (registroFirmanteSubcontratista == null)
                    {
                        throw new Exception("No hay firmantes dados de alta para el contrato seleccionado.");
                    }

                    var registroFirmaSubcontratista = new tblCOES_Firma
                    {
                        evaluacion_id = evaluacion_id,
                        contrato_id = contrato_id,
                        firmante_id = registroFirmanteSubcontratista.id,
                        tipo = TipoFirmanteEnum.SUBCONTRATISTA,
                        estadoFirma = EstadoFirmaEnum.PENDIENTE,
                        rutaArchivoFirma = null,
                        fechaAutorizacion = null,
                        usuarioCreacion_id = vSesiones.sesionUsuarioDTO.id,
                        fechaCreacion = DateTime.Now,
                        registroActivo = true
                    };

                    db.tblCOES_Firma.Add(registroFirmaSubcontratista);
                    db.SaveChanges();

                    var registroFirmanteGerente = db.tblCOES_FirmaGerente.FirstOrDefault(x => x.registroActivo && x.cc == registroContrato.cc);

                    if (registroFirmanteGerente == null)
                    {
                        throw new Exception("No hay un gerente asignado al centro de costos " + registroContrato.cc + ".");
                    }

                    var registroFirmaGerente = new tblCOES_Firma
                    {
                        evaluacion_id = evaluacion_id,
                        contrato_id = contrato_id,
                        firmante_id = registroFirmanteGerente.id,
                        tipo = TipoFirmanteEnum.GERENTE,
                        estadoFirma = EstadoFirmaEnum.PENDIENTE,
                        rutaArchivoFirma = null,
                        fechaAutorizacion = null,
                        usuarioCreacion_id = vSesiones.sesionUsuarioDTO.id,
                        fechaCreacion = DateTime.Now,
                        registroActivo = true
                    };

                    db.tblCOES_Firma.Add(registroFirmaGerente);
                    db.SaveChanges();
                    #endregion

                    var registroEvaluacion = db.tblCOES_Asignacion_Evaluacion.FirstOrDefault(x => x.id == evaluacion_id);

                    registroEvaluacion.estatus = EstatusEvaluacionEnum.ESTATUS_DE_EVALUACION;
                    registroEvaluacion.usuarioModificacion_id = vSesiones.sesionUsuarioDTO.id;
                    registroEvaluacion.fechaModificacion = DateTime.Now;
                    db.SaveChanges();

                    #region Notificar al firmante, administradores PMO y gerente
                    var listaCorreos = new List<string>();

                    listaCorreos.Add(registroFirmanteSubcontratista.correo);

                    var usuarioGerente = _context.tblP_Usuario.FirstOrDefault(x => x.id == registroFirmanteGerente.usuario_id);

                    if (usuarioGerente.correo != null)
                    {
                        listaCorreos.Add(usuarioGerente.correo);
                    }

                    var listaAdministradoresPMO = db.tblCOES_Facultamiento.Where(x => x.registroActivo && x.tipo == TipoFacultamientoEnum.ADMINISTRADOR_PMO).ToList().Join(
                        db.tblCOES_Facultamiento_CentroCosto.Where(x => x.registroActivo).ToList(),
                        f => f.id,
                        cc => cc.facultamiento_id,
                        (f, cc) => new { f, cc }
                    ).Where(x => x.cc.cc == registroContrato.cc).GroupBy(x => x.f.usuario_id).Select(x => new { usuario_id = x.Key, grp = x.ToList() }).ToList();

                    foreach (var admin in listaAdministradoresPMO)
                    {
                        var usuarioAdministradorPMO = _context.tblP_Usuario.FirstOrDefault(x => x.id == admin.usuario_id);

                        if (usuarioAdministradorPMO.correo != null)
                        {
                            listaCorreos.Add(usuarioAdministradorPMO.correo);
                        }
                    }

#if DEBUG
                    listaCorreos = new List<string> { "oscar.valencia@construplan.com.mx" };
#endif

                    var mensaje = @"Se ha enviado a gestión de firmas la evaluación del " + registroEvaluacion.fecha.ToShortDateString() + " para el contrato " + registroContrato.numeroContrato + ".";

                    var correoEnviado = GlobalUtils.sendEmail(string.Format("{0}: Evaluación Gestión de Firmas", PersonalUtilities.GetNombreEmpresa()), mensaje, listaCorreos);

                    if (!correoEnviado)
                    {
                        throw new Exception("No se pudo enviar el correo.");
                    }
                    #endregion

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                    LogError(0, 0, NombreControlador, "EnviarGestionFirmas", e, AccionEnum.ACTUALIZAR, 0, evaluacion_id);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> GetSeguimientoFirmas(int evaluacion_id, int contrato_id)
        {
            try
            {
                var listaFirmantesSubcontratistas = db.tblCOES_FirmaSubcontratista.Where(x => x.registroActivo).ToList();
                var listaFirmantesGerentes = db.tblCOES_FirmaGerente.Where(x => x.registroActivo).ToList().Join(
                    _context.tblP_Usuario.ToList(),
                    f => f.usuario_id,
                    u => u.id,
                    (f, u) => new { f, u }
                ).ToList();
                var listaFirmas = db.tblCOES_Firma.Where(x => x.registroActivo && x.evaluacion_id == evaluacion_id && x.contrato_id == contrato_id).ToList();
                var data = new List<FirmaDTO>();

                foreach (var firma in listaFirmas)
                {
                    var nombre = "";

                    #region Flags para firmar y rechazar
                    var flagPuedeFirmar = false;
                    var flagPuedeRechazar = false;

                    switch (firma.tipo)
                    {
                        case TipoFirmanteEnum.SUBCONTRATISTA:
                            nombre = listaFirmantesSubcontratistas.Where(y => y.id == firma.firmante_id).Select(z => z.nombre).FirstOrDefault();
                            flagPuedeFirmar = false;
                            break;
                        case TipoFirmanteEnum.GERENTE:
                            nombre = listaFirmantesGerentes.Where(y => y.f.id == firma.firmante_id).Select(z => z.u.nombre + " " + z.u.apellidoPaterno + " " + z.u.apellidoMaterno).FirstOrDefault();
                            flagPuedeFirmar = listaFirmantesGerentes.Where(y => y.f.id == firma.firmante_id).Select(x => x.f.usuario_id).FirstOrDefault() == vSesiones.sesionUsuarioDTO.id;
                            break;
                    }

                    if (vSesiones.sesionUsuarioDTO.id == 3807) //Usuario Oscar Valencia
                    {
                        flagPuedeFirmar = true;
                    }

                    if (firma.tipo == TipoFirmanteEnum.SUBCONTRATISTA && firma.estadoFirma == EstadoFirmaEnum.AUTORIZADO && flagPuedeFirmar) //El gerente que autoriza puede rechazar la autorización del subcontratista.
                    {
                        flagPuedeRechazar = true;
                    }

                    if (firma.estadoFirma != EstadoFirmaEnum.PENDIENTE) //Se quita el flag de firmar cuando ya fue autorizada la firma.
                    {
                        flagPuedeFirmar = false;
                    }
                    #endregion

                    data.Add(new FirmaDTO
                    {
                        firma_id = firma.id,
                        firmante_id = firma.firmante_id,
                        tipo = firma.tipo,
                        tipoDesc = firma.tipo.GetDescription(),
                        nombre = nombre,
                        fechaAutorizacion = firma.fechaAutorizacion,
                        fechaAutorizacionString = firma.fechaAutorizacion != null ? ((DateTime)firma.fechaAutorizacion).ToShortDateString() : "",
                        estadoFirma = firma.estadoFirma,
                        estadoFirmaDesc = firma.estadoFirma.GetDescription(),
                        flagPuedeFirmar = flagPuedeFirmar,
                        flagPuedeRechazar = flagPuedeRechazar
                    });
                }

                //Si el subcontratista no ha firmado, el gerente todavía no puede firmar.
                if (data.Where(x => x.tipo == TipoFirmanteEnum.SUBCONTRATISTA).Select(x => x.estadoFirma).FirstOrDefault() == EstadoFirmaEnum.PENDIENTE)
                {
                    var registroFirmaGerente = data.FirstOrDefault(x => x.tipo == TipoFirmanteEnum.GERENTE);

                    registroFirmaGerente.flagPuedeFirmar = false;
                }

                //Si todas las firmas fueron autorizadas, se quita el permiso de rechazar.
                if (data.All(x => x.estadoFirma == EstadoFirmaEnum.AUTORIZADO))
                {
                    foreach (var d in data)
                    {
                        d.flagPuedeRechazar = false;
                    }
                }

                resultado.Add("data", data);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                LogError(0, 0, NombreControlador, "GetSeguimientoFirmas", e, AccionEnum.CONSULTA, 0, new { evaluacion_id = evaluacion_id, contrato_id = contrato_id });
            }

            return resultado;
        }

        public Dictionary<string, object> AutorizarEvaluacion(int firma_id, HttpPostedFileBase archivoFirma)
        {
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    var registroFirma = db.tblCOES_Firma.FirstOrDefault(x => x.id == firma_id);
                    var registroContrato = db.tblX_Contrato.FirstOrDefault(x => x.id == registroFirma.contrato_id);
                    var registroSubcontratista = db.tblX_SubContratista.FirstOrDefault(x => x.id == registroContrato.subcontratistaID);
                    var registroEvaluacion = db.tblCOES_Asignacion_Evaluacion.FirstOrDefault(x => x.id == registroFirma.evaluacion_id);

                    #region Crear Carpetas
                    var carpetaSubcontratista = string.Format(@"{0}_{1}", registroSubcontratista.numeroProveedor, registroSubcontratista.rfc);
                    var rutaCarpetaEvaluacionSubcontratista = Path.Combine(RutaControlObra, carpetaSubcontratista);

                    if (verificarExisteCarpeta(rutaCarpetaEvaluacionSubcontratista, true) == false)
                    {
                        throw new Exception("No se pudo crear la carpeta del subcontratista.");
                    }

                    var rutaCarpetaEvaluacion = Path.Combine(rutaCarpetaEvaluacionSubcontratista, string.Format(@"EVALUACION_ID_{0}_TIPO_{1}_FECHA_{2}", registroFirma.evaluacion_id, registroEvaluacion.tipo.GetDescription(), (registroEvaluacion.fecha.ToShortDateString()).Replace("/", "_")));

                    if (verificarExisteCarpeta(rutaCarpetaEvaluacion, true) == false)
                    {
                        throw new Exception("No se pudo crear la carpeta de la evaluación.");
                    }

                    var rutaCarpetaFirmas = Path.Combine(rutaCarpetaEvaluacion, "FIRMAS");

                    if (verificarExisteCarpeta(rutaCarpetaFirmas, true) == false)
                    {
                        throw new Exception("No se pudo crear la carpeta de las firmas.");
                    }
                    #endregion

                    string nombreArchivo = ObtenerFormatoNombreArchivo("ArchivoFirma_" + registroFirma.id + "_" + registroFirma.firmante_id + "_" + registroFirma.tipo, archivoFirma.FileName);
                    string rutaArchivo = Path.Combine(rutaCarpetaFirmas, nombreArchivo);

                    #region Archivo Existente
                    if (File.Exists(rutaArchivo))
                    {
                        int count = 1;

                        string fileNameOnly = Path.GetFileNameWithoutExtension(rutaArchivo);
                        string extension = Path.GetExtension(rutaArchivo);
                        string path = Path.GetDirectoryName(rutaArchivo);
                        string newFullPath = rutaArchivo;

                        while (File.Exists(newFullPath))
                        {
                            string tempFileName = string.Format("{0} ({1})", fileNameOnly, count++);
                            newFullPath = Path.Combine(path, tempFileName + extension);
                        }

                        rutaArchivo = newFullPath;
                    }
                    #endregion

                    if (GlobalUtils.SaveHTTPPostedFile(archivoFirma, rutaArchivo) == false)
                    {
                        dbContextTransaction.Rollback();
                        resultado.Clear();
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "Ocurrió un error al guardar los archivos en el servidor.");
                        return resultado;
                    }

                    registroFirma.estadoFirma = EstadoFirmaEnum.AUTORIZADO;
                    registroFirma.rutaArchivoFirma = rutaArchivo;
                    registroFirma.fechaAutorizacion = DateTime.Now;
                    registroFirma.usuarioModificacion_id = vSesiones.sesionUsuarioDTO.id;
                    registroFirma.fechaModificacion = DateTime.Now;
                    db.SaveChanges();

                    #region Notificar a los administradores PMO
                    var listaCorreos = new List<string>();
                    var listaAdministradoresPMO = db.tblCOES_Facultamiento.Where(x => x.registroActivo && x.tipo == TipoFacultamientoEnum.ADMINISTRADOR_PMO).ToList().Join(
                        db.tblCOES_Facultamiento_CentroCosto.Where(x => x.registroActivo).ToList(),
                        f => f.id,
                        cc => cc.facultamiento_id,
                        (f, cc) => new { f, cc }
                    ).Where(x => x.cc.cc == registroContrato.cc).GroupBy(x => x.f.usuario_id).Select(x => new { usuario_id = x.Key, grp = x.ToList() }).ToList();

                    foreach (var admin in listaAdministradoresPMO)
                    {
                        var usuarioAdministradorPMO = _context.tblP_Usuario.FirstOrDefault(x => x.id == admin.usuario_id);

                        if (usuarioAdministradorPMO.correo != null)
                        {
                            listaCorreos.Add(usuarioAdministradorPMO.correo);
                        }
                    }

#if DEBUG
                    listaCorreos = new List<string> { "oscar.valencia@construplan.com.mx" };
#endif

                    var mensaje = string.Format(@"
                        Se autorizó una evaluación para gestión de firmas.<br/>
                        Número de contrato: {0}<br/>
                        Proyecto: {1}<br/><br/>", registroContrato.numeroContrato, _context.tblP_CC.Where(x => x.cc == registroContrato.cc).Select(x => x.descripcion).FirstOrDefault()
                    );

                    var listaFirmantesSubcontratistas = db.tblCOES_FirmaSubcontratista.Where(x => x.registroActivo).ToList();
                    var listaFirmantesGerentes = db.tblCOES_FirmaGerente.Where(x => x.registroActivo).ToList().Join(
                        _context.tblP_Usuario.ToList(),
                        f => f.usuario_id,
                        u => u.id,
                        (f, u) => new { f, u }
                    ).ToList();
                    var listaFirmas = db.tblCOES_Firma.Where(x => x.registroActivo && x.evaluacion_id == registroFirma.evaluacion_id && x.contrato_id == registroFirma.contrato_id).ToList();
                    var data = new List<FirmaDTO>();
                    var renglonesString = "";

                    foreach (var firma in listaFirmas)
                    {
                        var nombre = "";

                        switch (firma.tipo)
                        {
                            case TipoFirmanteEnum.SUBCONTRATISTA:
                                nombre = listaFirmantesSubcontratistas.Where(y => y.id == firma.firmante_id).Select(z => z.nombre).FirstOrDefault();
                                break;
                            case TipoFirmanteEnum.GERENTE:
                                nombre = listaFirmantesGerentes.Where(y => y.f.id == firma.firmante_id).Select(z => z.u.nombre + " " + z.u.apellidoPaterno + " " + z.u.apellidoMaterno).FirstOrDefault();
                                break;
                        }

                        data.Add(new FirmaDTO
                        {
                            firma_id = firma.id,
                            firmante_id = firma.firmante_id,
                            tipo = firma.tipo,
                            tipoDesc = firma.tipo.GetDescription(),
                            nombre = nombre,
                            fechaAutorizacion = firma.fechaAutorizacion,
                            fechaAutorizacionString = firma.fechaAutorizacion != null ? ((DateTime)firma.fechaAutorizacion).ToShortDateString() : "",
                            estadoFirma = firma.estadoFirma,
                            estadoFirmaDesc = firma.estadoFirma.GetDescription()
                        });

                        renglonesString += string.Format(@"
                            <tr>
                                <td>{0}</td>
                                <td>{1}</td>
                                <td>{2}</td>
                                <td>{3}</td>
                            </tr>", firma.tipo.GetDescription(), nombre, firma.fechaAutorizacion != null ? ((DateTime)firma.fechaAutorizacion).ToShortDateString() : "", firma.estadoFirma.GetDescription()
                        );
                    }

                    mensaje += @"
                        <table>
                            <thead>
                                <tr>
                                    <th>Tipo</th>
                                    <th>Nombre</th>
                                    <th>Fecha Autorización</th>
                                    <th>Estado Firma</th>
                                </tr>
                            </thead>
                            <tbody>" + renglonesString + @"</tbody>
                        </table>";

                    var correoEnviado = GlobalUtils.sendEmail(string.Format("{0}: Autorización Evaluación [{1} - {2}]", PersonalUtilities.GetNombreEmpresa(), registroContrato.numeroContrato, registroEvaluacion.fecha.ToShortDateString()), mensaje, listaCorreos);

                    if (!correoEnviado)
                    {
                        throw new Exception("No se pudo enviar el correo.");
                    }
                    #endregion

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                    LogError(0, 0, "ControlObraController", "AutorizarEvaluacion", e, AccionEnum.AGREGAR, 0, firma_id);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> RechazarEvaluacion(int firma_id)
        {
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    var registroFirmaGerente = db.tblCOES_Firma.FirstOrDefault(x => x.id == firma_id);

                    var registroFirmaSubcontratistaAutorizada = db.tblCOES_Firma.FirstOrDefault(x =>
                        x.registroActivo &&
                        x.evaluacion_id == registroFirmaGerente.evaluacion_id &&
                        x.tipo == TipoFirmanteEnum.SUBCONTRATISTA &&
                        x.estadoFirma == EstadoFirmaEnum.AUTORIZADO
                    );

                    if (registroFirmaSubcontratistaAutorizada == null)
                    {
                        throw new Exception("No se encuentra la firma del subcontratista autorizada.");
                    }

                    registroFirmaSubcontratistaAutorizada.estadoFirma = EstadoFirmaEnum.PENDIENTE;
                    registroFirmaSubcontratistaAutorizada.rutaArchivoFirma = null;
                    registroFirmaSubcontratistaAutorizada.fechaAutorizacion = null;
                    registroFirmaSubcontratistaAutorizada.usuarioModificacion_id = vSesiones.sesionUsuarioDTO.id;
                    registroFirmaSubcontratistaAutorizada.fechaModificacion = DateTime.Now;
                    db.SaveChanges();

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                    LogError(0, 0, "ControlObraController", "RechazarEvaluacion", e, AccionEnum.AGREGAR, 0, firma_id);
                }
            }

            return resultado;
        }

        private static bool verificarExisteCarpeta(string path, bool crear = false)
        {
            bool existe = false;

            try
            {
                existe = Directory.Exists(path);

                if (!existe && crear)
                {
                    Directory.CreateDirectory(path);
                    existe = true;
                }
            }
            catch (Exception e)
            {
                existe = false;
            }

            return existe;
        }

        private string ObtenerFormatoNombreArchivo(string nombreBase, string fileName)
        {
            return String.Format("{0} {1}{2}", nombreBase, DateTime.Now.ToString("dd-MM-y HH:mm:ss").Replace(":", "-"), Path.GetExtension(fileName));
        }

        public Dictionary<string, object> GetAsignacionContrato(int asignacion_id)
        {
            try
            {
                var registroAsignacion = db.tblCOES_Asignacion.FirstOrDefault(x => x.id == asignacion_id);
                var listaEvaluaciones = db.tblCOES_Asignacion_Evaluacion.Where(x => x.registroActivo && x.asignacion_id == registroAsignacion.id).ToList();
                var data = new AsignacionDTO
                {
                    id = registroAsignacion.id,
                    contrato_id = registroAsignacion.contrato_id,
                    fechaInicial = registroAsignacion.fechaInicial,
                    fechaInicialString = registroAsignacion.fechaInicial.ToShortDateString(),
                    fechaFinal = registroAsignacion.fechaFinal,
                    fechaFinalString = registroAsignacion.fechaFinal.ToShortDateString(),
                    evaluaciones = listaEvaluaciones.Select(x => new EvaluacionAsignacionDTO
                    {
                        id = x.id,
                        asignacion_id = x.asignacion_id,
                        fecha = x.fecha,
                        fechaString = x.fecha.ToShortDateString(),
                        tipo = x.tipo,
                        tipoDesc = x.tipo.GetDescription(),
                        estatus = x.estatus,
                        estatusDesc = x.estatus.GetDescription()
                    }).ToList()
                };

                resultado.Add("data", data);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                LogError(0, 0, NombreControlador, "GetContratoInformacion", e, AccionEnum.CONSULTA, 0, asignacion_id);
            }

            return resultado;
        }

        public Dictionary<string, object> GuardarCambioEvaluacion(tblCOES_CambioEvaluacion cambio)
        {
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    var registroEvaluacion = db.tblCOES_Asignacion_Evaluacion.FirstOrDefault(x => x.id == cambio.evaluacion_id);

                    cambio.fechaAnterior = registroEvaluacion.fecha;
                    cambio.estatus = EstatusCambioEvaluacionEnum.PENDIENTE;
                    cambio.comentarioAutorizante = "";
                    cambio.usuarioCreacion_id = vSesiones.sesionUsuarioDTO.id;
                    cambio.fechaCreacion = DateTime.Now;
                    cambio.registroActivo = true;

                    db.tblCOES_CambioEvaluacion.Add(cambio);
                    db.SaveChanges();

                    #region Notificar Administradores PMO
                    var listaAdministradoresPMO = db.tblCOES_Facultamiento.Where(x => x.registroActivo && x.tipo == TipoFacultamientoEnum.ADMINISTRADOR_PMO).ToList();
                    var registroAsignacion = db.tblCOES_Asignacion.FirstOrDefault(x => x.id == registroEvaluacion.asignacion_id);
                    var registroContrato = db.tblX_Contrato.FirstOrDefault(x => x.id == registroAsignacion.contrato_id);
                    var registroSubcontratista = db.tblX_SubContratista.FirstOrDefault(x => x.id == registroContrato.subcontratistaID);
                    var mensajeAlerta = string.Format(@"Gestión de cambio de fecha de evaluación: [{0} - {1}] {2}", registroContrato.numeroContrato, registroEvaluacion.fecha.ToShortDateString(), registroSubcontratista.nombre);

                    foreach (var admin in listaAdministradoresPMO)
                    {
                        var usuarioRecibe_id = admin.usuario_id;

#if DEBUG
                        usuarioRecibe_id = 3807; //Usuario Oscar Valencia
#endif

                        var alertaFacultamiento = new tblP_Alerta
                        {
                            msj = mensajeAlerta,
                            sistemaID = 3,
                            tipoAlerta = (int)AlertasEnum.REDIRECCION,
                            url = "/Subcontratistas/EvaluacionSubcontratista/AdministracionEvaluacion",
                            userEnviaID = vSesiones.sesionUsuarioDTO.id,
                            userRecibeID = usuarioRecibe_id,
                            objID = cambio.evaluacion_id
                        };
                        _context.tblP_Alerta.Add(alertaFacultamiento);
                        _context.SaveChanges();
                    }
                    #endregion

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                    LogError(0, 0, NombreControlador, "GuardarCambioEvaluacion", e, AccionEnum.AGREGAR, 0, null);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> GetCambioEvaluacion(int cambioEvaluacion_id)
        {
            try
            {
                var registroCambio = db.tblCOES_CambioEvaluacion.FirstOrDefault(x => x.id == cambioEvaluacion_id);
                var data = new
                {
                    id = registroCambio.id,
                    evaluacion_id = registroCambio.evaluacion_id,
                    fechaAnterior = registroCambio.fechaAnterior,
                    fechaAnteriorString = registroCambio.fechaAnterior.ToShortDateString(),
                    fechaNueva = registroCambio.fechaNueva,
                    fechaNuevaString = registroCambio.fechaNueva.ToShortDateString(),
                    usuarioSolicitante_id = registroCambio.usuarioSolicitante_id,
                    usuarioSolicitanteNombre = _context.tblP_Usuario.Where(x => x.id == registroCambio.usuarioSolicitante_id).Select(y => y.nombre + " " + y.apellidoPaterno + " " + y.apellidoMaterno).FirstOrDefault(),
                    motivoCambio = registroCambio.motivoCambio,
                    estatus = registroCambio.estatus,
                    estatusDesc = registroCambio.estatus.GetDescription(),
                    comentarioAutorizante = registroCambio.comentarioAutorizante
                };

                resultado.Add("data", data);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                LogError(0, 0, NombreControlador, "GetCambioEvaluacion", e, AccionEnum.CONSULTA, 0, cambioEvaluacion_id);
            }

            return resultado;
        }

        public Dictionary<string, object> GuardarAutorizacionCambioEvaluacion(tblCOES_CambioEvaluacion cambio)
        {
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    var registroCambio = db.tblCOES_CambioEvaluacion.FirstOrDefault(x => x.id == cambio.id);

                    registroCambio.estatus = cambio.estatus;
                    registroCambio.comentarioAutorizante = cambio.comentarioAutorizante ?? "";
                    db.SaveChanges();

                    #region Recorrer las evaluaciones posteriores si el cambio fue autorizado.
                    if (cambio.estatus == EstatusCambioEvaluacionEnum.AUTORIZADO)
                    {
                        var registroEvaluacion = db.tblCOES_Asignacion_Evaluacion.FirstOrDefault(x => x.id == registroCambio.evaluacion_id);
                        var listaEvaluacionesPeriodicasAsignacion = db.tblCOES_Asignacion_Evaluacion.Where(x => x.registroActivo && x.asignacion_id == registroEvaluacion.asignacion_id && x.tipo == TipoEvaluacionEnum.PERIODICA).ToList();
                        var registroEvaluacionFinal = db.tblCOES_Asignacion_Evaluacion.FirstOrDefault(x => x.registroActivo && x.asignacion_id == registroEvaluacion.asignacion_id && x.tipo == TipoEvaluacionEnum.FINAL);
                        var cantidadDias = (registroCambio.fechaNueva.Date - registroCambio.fechaAnterior.Date).TotalDays;

                        foreach (var evaluacionPeriodica in listaEvaluacionesPeriodicasAsignacion)
                        {
                            var nuevaFechaEvaluacion = evaluacionPeriodica.fecha.AddDays(cantidadDias);

                            evaluacionPeriodica.fecha = nuevaFechaEvaluacion;

                            if ((registroEvaluacionFinal.fecha.Date - nuevaFechaEvaluacion.Date).TotalDays < 30) //Si la última evaluación periódica termina a menos de 30 días de la evaluación final, se elimina la evaluación.
                            {
                                evaluacionPeriodica.registroActivo = false;
                            }

                            db.SaveChanges();
                        }
                    }
                    #endregion

                    #region Notificar Solicitante
                    var mensaje = cambio.estatus == EstatusCambioEvaluacionEnum.AUTORIZADO ? "Gestión de cambio aprobada" : "Gestión de cambio no aprobada";
                    var alertaFacultamiento = new tblP_Alerta
                    {
                        msj = mensaje,
                        sistemaID = 3,
                        tipoAlerta = (int)AlertasEnum.MENSAJE_VISTO_CLICK,
                        url = "#",
                        userEnviaID = vSesiones.sesionUsuarioDTO.id,
                        userRecibeID = registroCambio.usuarioSolicitante_id,
                        objID = registroCambio.id
                    };
                    _context.tblP_Alerta.Add(alertaFacultamiento);
                    _context.SaveChanges();
                    #endregion

                    #region Quitar alertas de autorización
                    var alertasAutorizacion = _context.tblP_Alerta.Where(x => x.url.Contains("/Subcontratistas/EvaluacionSubcontratista/AdministracionEvaluacion") && x.objID == registroCambio.evaluacion_id).ToList();

                    if (alertasAutorizacion.Count > 0)
                    {
                        _context.tblP_Alerta.RemoveRange(alertasAutorizacion);
                        _context.SaveChanges();
                    }
                    #endregion

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                    LogError(0, 0, NombreControlador, "GuardarAutorizacionCambioEvaluacion", e, AccionEnum.ACTUALIZAR, 0, cambio);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> CargarGraficasSubcontratista(string cc, int subcontratista_id, int contrato_id, EstatusEvaluacionEnum estatus)
        {
            try
            {
                var listaContratos = db.tblX_Contrato.Where(x => x.estatus).ToList().Where(x => x.fechaVigencia != null ? ((DateTime)x.fechaVigencia).Date >= DateTime.Now.Date : false).ToList();
                var listaAsignaciones = db.tblCOES_Asignacion.Where(x => x.registroActivo).ToList();
                var listaSubcontratistas = db.tblX_SubContratista.Where(x => x.estatus).ToList();
                var listaCentroCosto = _context.tblP_CC.Where(x => x.estatus).ToList();
                var data = new List<ContratoEvaluacionDTO>();

                foreach (var con in listaContratos)
                {
                    var asignacion = listaAsignaciones.FirstOrDefault(x => x.contrato_id == con.id);
                    var subcontratista = listaSubcontratistas.FirstOrDefault(x => x.id == con.subcontratistaID);
                    var centroCosto = listaCentroCosto.FirstOrDefault(x => x.cc == con.cc);

                    if (asignacion != null)
                    {
                        var evaluacionesAsignacion = db.tblCOES_Asignacion_Evaluacion.Where(x => x.registroActivo && x.asignacion_id == asignacion.id).ToList();

                        foreach (var eva in evaluacionesAsignacion)
                        {
                            var listaEvidencias = db.tblCOES_Evidencia.Where(x => x.registroActivo && x.evaluacion_id == eva.id).ToList();
                            var fechaCargaLimite = eva.fecha.AddDays(2);
                            var evaluacionFirmada = db.tblCOES_Firma.Where(x => x.registroActivo && x.evaluacion_id == eva.id && x.estadoFirma == EstadoFirmaEnum.AUTORIZADO).ToList().Count() == 2;
                            var registroCambioEvaluacion = db.tblCOES_CambioEvaluacion.FirstOrDefault(x => x.registroActivo && x.evaluacion_id == eva.id && x.estatus == EstatusCambioEvaluacionEnum.PENDIENTE);
                            var flagCargaSoportesEnTiempo = false;
                            DateTime? fechaCargaSoporte = null;

                            #region String Carga Soportes
                            var cargaSoportes = "";

                            if (listaEvidencias.Count() == 0)
                            {
                                var hoy = DateTime.Now.Date;

                                if (fechaCargaLimite.Date >= hoy.Date)
                                {
                                    flagCargaSoportesEnTiempo = true;
                                }
                                else
                                {
                                    flagCargaSoportesEnTiempo = false;
                                }
                            }
                            else
                            {
                                var ultimaEvidenciaCargada = listaEvidencias.Max(x => x.fechaCreacion);

                                if (fechaCargaLimite.Date >= ultimaEvidenciaCargada.Date)
                                {
                                    cargaSoportes = "EN TIEMPO \"OK\"";
                                    flagCargaSoportesEnTiempo = true;
                                }
                                else
                                {
                                    var diferenciaDias = (ultimaEvidenciaCargada.Date - fechaCargaLimite.Date).TotalDays;

                                    cargaSoportes = string.Format("FUERA DE TIEMPO \"{0} DÍAS\"", diferenciaDias);
                                    flagCargaSoportesEnTiempo = false;
                                }

                                fechaCargaSoporte = ultimaEvidenciaCargada.Date;
                            }
                            #endregion

                            #region String Estatus Firmas
                            var estatusFirmas = "";
                            var firmaSubcontratista = db.tblCOES_Firma.FirstOrDefault(x => x.registroActivo && x.evaluacion_id == eva.id && x.tipo == TipoFirmanteEnum.SUBCONTRATISTA);
                            var firmaGerente = db.tblCOES_Firma.FirstOrDefault(x => x.registroActivo && x.evaluacion_id == eva.id && x.tipo == TipoFirmanteEnum.GERENTE);

                            if (firmaSubcontratista != null && firmaGerente != null)
                            {
                                if (firmaSubcontratista.estadoFirma == EstadoFirmaEnum.AUTORIZADO && firmaGerente.estadoFirma == EstadoFirmaEnum.AUTORIZADO)
                                {
                                    estatusFirmas = "AUTORIZADA";
                                }
                                else
                                {
                                    estatusFirmas = "PENDIENTE";
                                }
                            }
                            #endregion

                            data.Add(new ContratoEvaluacionDTO
                            {
                                contrato_id = con.id,
                                asignacion_id = asignacion.id,
                                evaluacion_id = eva.id,
                                numeroContrato = con.numeroContrato,
                                cc = con.cc,
                                ccDesc = centroCosto != null ? ("[" + centroCosto.cc + "] " + centroCosto.descripcion) : "",
                                subcontratista_id = con.subcontratistaID,
                                subcontratistaDesc = subcontratista != null ? subcontratista.nombre : "",
                                periodoEvaluable = asignacion.fechaInicial.ToShortDateString() + " - " + asignacion.fechaFinal.ToShortDateString(),
                                fechaEvaluacion = eva.fecha,
                                fechaEvaluacionString = eva.fecha.ToShortDateString(),
                                flagCargaSoportesEnTiempo = flagCargaSoportesEnTiempo,
                                cargaSoportes = cargaSoportes,
                                fechaCargaSoporte = fechaCargaSoporte,
                                estatusEvaluacion = eva.estatus,
                                estatusFirmas = estatusFirmas,
                                evaluacionFirmada = evaluacionFirmada,
                                cambioEvaluacion_id = registroCambioEvaluacion != null ? registroCambioEvaluacion.id : 0
                            });
                        }
                    }
                    else
                    {
                        data.Add(new ContratoEvaluacionDTO
                        {
                            contrato_id = con.id,
                            asignacion_id = 0,
                            numeroContrato = con.numeroContrato,
                            cc = con.cc,
                            ccDesc = centroCosto != null ? ("[" + centroCosto.cc + "] " + centroCosto.descripcion) : "",
                            subcontratista_id = con.subcontratistaID,
                            subcontratistaDesc = subcontratista != null ? subcontratista.nombre : "",
                            periodoEvaluable = "",
                            fechaEvaluacion = null,
                            fechaEvaluacionString = "",
                            flagCargaSoportesEnTiempo = false,
                            cargaSoportes = "",
                            fechaCargaSoporte = null,
                            estatusEvaluacion = EstatusEvaluacionEnum.PENDIENTE_POR_ASIGNAR,
                            estatusFirmas = "",
                            evaluacionFirmada = false,
                            cambioEvaluacion_id = 0
                        });
                    }
                }

                #region Filtros
                if (cc != "" && cc != null)
                {
                    data = data.Where(x => x.cc == cc).ToList();
                }

                if (subcontratista_id > 0)
                {
                    data = data.Where(x => x.subcontratista_id == subcontratista_id).ToList();
                }
                #endregion

                data = data.Where(x => x.estatusEvaluacion == estatus).ToList();

                #region Gráfica Cumplimiento
                var chartCumplimientoCargaSoportes = new GraficaDTO();

                chartCumplimientoCargaSoportes.categorias.Add(data.First().subcontratistaDesc);
                chartCumplimientoCargaSoportes.serie1Descripcion = "En Tiempo";
                chartCumplimientoCargaSoportes.serie1.Add(data.Where(x => x.flagCargaSoportesEnTiempo).ToList().Count());
                chartCumplimientoCargaSoportes.serie2Descripcion = "Fuera de periodo";
                chartCumplimientoCargaSoportes.serie2.Add(data.Where(x => !x.flagCargaSoportesEnTiempo).ToList().Count());
                chartCumplimientoCargaSoportes.serie3Descripcion = "Óptimo";
                chartCumplimientoCargaSoportes.serie3.Add(data.Count());
                #endregion

                #region Gráfica Histórico
                var chartHistoricoCargaSoportes = new GraficaDTO();
                var contador = 1;

                foreach (var d in data)
                {
                    chartHistoricoCargaSoportes.categorias.Add("EV-" + contador);
                    chartHistoricoCargaSoportes.serie1Descripcion = "REQUERIDA";
                    chartHistoricoCargaSoportes.serie1String.Add(d.fechaEvaluacionString);
                    chartHistoricoCargaSoportes.serie2Descripcion = "ENVIADA";
                    chartHistoricoCargaSoportes.serie2String.Add(d.fechaCargaSoporte != null ? ((DateTime)d.fechaCargaSoporte).ToShortDateString() : "");

                    contador++;
                }
                #endregion

                resultado.Add("chartCumplimientoCargaSoportes", chartCumplimientoCargaSoportes);
                resultado.Add("chartHistoricoCargaSoportes", chartHistoricoCargaSoportes);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                LogError(0, 0, NombreControlador, "CargarGraficasSubcontratista", e, AccionEnum.CONSULTA, 0, new { cc = cc, subcontratista_id = subcontratista_id, estatus = estatus });
            }

            return resultado;
        }

        public ReporteEvaluacionSubcontratistaDTO GetReporteEvaluacionSubcontratista(int evaluacion_id)
        {
            var registroEvaluacion = db.tblCOES_Asignacion_Evaluacion.FirstOrDefault(x => x.id == evaluacion_id);
            var registroAsignacion = db.tblCOES_Asignacion.FirstOrDefault(x => x.id == registroEvaluacion.asignacion_id);
            var registroContrato = db.tblX_Contrato.FirstOrDefault(x => x.id == registroAsignacion.contrato_id);
            var registroSubcontratista = db.tblX_SubContratista.FirstOrDefault(x => x.id == registroContrato.subcontratistaID);
            var registroCentroCosto = _context.tblP_CC.FirstOrDefault(x => x.cc == registroContrato.cc);
            var listaEvidencias = db.tblCOES_Evidencia.Where(x => x.registroActivo && x.evaluacion_id == evaluacion_id).ToList();
            var relacionPlantillaContrato = db.tblCOES_PlantillatblX_Contrato.FirstOrDefault(x => x.registroActivo && x.contrato_id == registroAsignacion.contrato_id);
            var listaElementosPlantilla = db.tblCOES_PlantillatblCOES_Elemento.Where(x => x.registroActivo && x.plantilla_id == relacionPlantillaContrato.plantilla_id).ToList();
            var listaElementosPlantilla_elemento_id = listaElementosPlantilla.Select(x => x.elemento_id).ToList();
            var listaElementos = db.tblCOES_Elemento.Where(x => x.registroActivo == listaElementosPlantilla_elemento_id.Contains(x.id)).ToList();
            var listaElementosPlantilla_id = listaElementosPlantilla.Select(x => x.id).ToList();
            var listaRequerimientosPlantilla = db.tblCOES_PlantillatblCOES_ElementotblCOES_Requerimiento.Where(x => x.registroActivo && listaElementosPlantilla_id.Contains(x.relacionPlantillaElemento_id)).ToList();
            var listaRequerimientosPlantilla_requerimiento_id = listaRequerimientosPlantilla.Select(x => x.requerimiento_id).ToList();
            var listaRequerimientos = db.tblCOES_Requerimiento.Where(x => x.registroActivo && listaRequerimientosPlantilla_requerimiento_id.Contains(x.id)).ToList();
            var listaElementosRequerimientos = new List<DivReqDTO>();
            var listaRetroalimentacion = new List<reporteCristal>();

            foreach (var ele in listaElementosPlantilla)
            {
                var registroElemento = listaElementos.FirstOrDefault(x => x.id == ele.elemento_id);
                var listaRequerimientosElemento = listaRequerimientosPlantilla.Where(x => x.relacionPlantillaElemento_id == ele.id).ToList();
                var listaCalificacionesElemento = new List<decimal>();

                foreach (var req in listaRequerimientosElemento)
                {
                    var registroRequerimiento = listaRequerimientos.FirstOrDefault(x => x.id == req.requerimiento_id);
                    var listaEvidenciasRequerimiento = db.tblCOES_Evidencia.Where(x => x.evaluacion_id == evaluacion_id && x.requerimiento_id == req.requerimiento_id).ToList();

                    if (listaEvidenciasRequerimiento.Count() > 0)
                    {
                        listaCalificacionesElemento.Add(listaEvidenciasRequerimiento.Where(x => x.calificacion > 0).LastOrDefault().calificacion);

                        var evidenciasReprobadas = listaEvidenciasRequerimiento.Where(x => x.calificacion <= 75).ToList();

                        foreach (var eviR in evidenciasReprobadas)
                        {
                            listaRetroalimentacion.Add(new reporteCristal
                            {
                                Divicion = registroElemento.clave,
                                Desviaciones = registroRequerimiento.descripcion,
                                PlanesDeAccion = eviR.planAccion ?? "",
                                Responsable = eviR.responsable ?? "",
                                FechaCompromiso = eviR.fechaCompromiso != null ? ((DateTime)eviR.fechaCompromiso).ToShortDateString() : ""
                            });
                        }
                    }
                    else
                    {
                        listaCalificacionesElemento.Add(0m);
                    }
                }

                foreach (var req in listaRequerimientosElemento)
                {
                    var registroRequerimiento = listaRequerimientos.FirstOrDefault(x => x.id == req.requerimiento_id);
                    var calificacionElemento = listaCalificacionesElemento.Count() > 0 ? listaCalificacionesElemento.Average() : 0m;

                    listaElementosRequerimientos.Add(new DivReqDTO
                    {
                        DivicionesORequerimiento = registroElemento.clave,
                        TituloP = registroElemento.descripcion,
                        Titulo = registroRequerimiento.descripcion,
                        Pesimo = calificacionElemento >= 0 && calificacionElemento <= 25 ? calificacionElemento.ToString() : "",
                        Malo = calificacionElemento >= 26 && calificacionElemento <= 50 ? calificacionElemento.ToString() : "",
                        Regular = calificacionElemento >= 51 && calificacionElemento <= 70 ? calificacionElemento.ToString() : "",
                        Aceptable = calificacionElemento >= 71 && calificacionElemento <= 90 ? calificacionElemento.ToString() : "",
                        Excdediendo = calificacionElemento >= 91 ? calificacionElemento.ToString() : "",
                        Calificacion = "",
                        CalificacionNumero = calificacionElemento
                    });

                    //foreach (var evi in listaEvidencias)
                    //{
                    //    requerimiento.evidencias.Add(new EvidenciaDTO
                    //    {
                    //        evidencia_id = evi.id,
                    //        evaluacion_id = evi.evaluacion_id,
                    //        requerimiento_id = evi.requerimiento_id,
                    //        rutaArchivo = evi.rutaArchivo,
                    //        tipo = evi.tipo,
                    //        tipoDesc = evi.tipo.GetDescription(),
                    //        evidenciaInicial_id = evi.evidenciaInicial_id,
                    //        estatus = evi.estatus,
                    //        estatusDesc = evi.estatus.GetDescription(),
                    //        calificacion = evi.calificacion,
                    //        ponderacion = evi.ponderacion,
                    //        comentarioEvaluacion = evi.comentarioEvaluacion,
                    //        planAccion = evidenciaAnterior != null ? evidenciaAnterior.planAccion : evi.planAccion,
                    //        responsable = evidenciaAnterior != null ? evidenciaAnterior.responsable : evi.responsable,
                    //        fechaCompromiso = evidenciaAnterior != null ? evidenciaAnterior.fechaCompromiso : evi.fechaCompromiso,
                    //        fechaCompromisoString =
                    //            evidenciaAnterior != null ?
                    //                (evidenciaAnterior.fechaCompromiso != null ? ((DateTime)evidenciaAnterior.fechaCompromiso).ToShortDateString() : "") :
                    //                (evi.fechaCompromiso != null ? ((DateTime)evi.fechaCompromiso).ToShortDateString() : "")
                    //    });

                    //    evidenciaAnterior = evi;
                    //}
                }
            }

            var listaEspecialidadContrato = db.tblX_ContratotblCOES_Especialidad.Where(x => x.registroActivo && x.contrato_id == registroContrato.id).ToList().Join(
                db.tblCOES_Especialidad.Where(x => x.registroActivo).ToList(),
                r => r.especialidad_id,
                e => e.id,
                (r, e) => new { r, e }
            ).ToList();
            var servicioContratado = string.Join(", ", listaEspecialidadContrato.Select(x => x.e.descripcion).ToList());
            var listaEvaluadoresCentroCosto = db.tblCOES_Evaluador_Proyecto.Where(x => x.registroActivo && x.cc == registroContrato.cc).ToList().Join(
                db.tblCOES_Evaluador.Where(x => x.registroActivo).ToList(),
                r => r.evaluador_id,
                e => e.id,
                (r, e) => new { r, e }
            ).Join(
                _context.tblP_Usuario.Where(x => x.estatus).ToList(),
                re => re.e.usuario_id,
                u => u.id,
                (re, u) => new { re.r, re.e, u }
            ).ToList();
            var evaluadorNombre = string.Join(", ", listaEvaluadoresCentroCosto.Select(x => x.u.nombre + " " + x.u.apellidoPaterno + " " + x.u.apellidoMaterno).ToList());

            var data = new ReporteEvaluacionSubcontratistaDTO
            {
                fechaEvaluacion = registroEvaluacion.fecha.ToShortDateString(),
                periodoEvaluacion = registroAsignacion.fechaInicial.ToShortDateString() + " - " + registroAsignacion.fechaFinal.ToShortDateString(),
                periodoEjecucion = (registroContrato.fechaInicial != null ? ((DateTime)registroContrato.fechaInicial).ToShortDateString() : "") + " - " + (registroContrato.fechaFinal != null ? ((DateTime)registroContrato.fechaFinal).ToShortDateString() : ""),
                subcontratistaNombre = registroSubcontratista.nombre,
                numeroContrato = registroContrato.numeroContrato,
                servicioContratado = servicioContratado,
                proyectoNombre = registroCentroCosto.descripcion,
                evaluadorNombre = evaluadorNombre,
                calificacionEvaluacion = listaEvidencias.Average(x => x.calificacion).ToString(),
                listaElementosRequerimientos = listaElementosRequerimientos,
                listaRetroalimentacion = listaRetroalimentacion
            };

            return data;
        }
        #endregion

        #region Calendario Evaluaciones
        public Dictionary<string, object> llenarCalendarioEvaluaciones(List<string> lstFiltroCC, List<int?> lstFiltroSubC)
        {
            try
            {

                if (lstFiltroCC != null && lstFiltroCC.Count == 1 && lstFiltroCC.Contains(null))
                {
                    lstFiltroCC = null;
                }

                if (lstFiltroSubC != null && lstFiltroSubC.Count == 1 && lstFiltroSubC.Contains(null))
                {
                    lstFiltroSubC = null;
                }
                // var listaEvaluacionesFecha = db.tblCOES_Asignacion_Evaluacion.Where(x => x.registroActivo == true).Select(x => new
                //{
                //    id = x.asignacion_id,
                //    fecha = x.fecha,

                //}).ToList();

                //var listaAsignacion = db.tblCOES_Asignacion.Select(x => x.contrato_id).FirstOrDefault();
                //var listaContrato = _context.tblX_Contrato.Where(x => x.id == listaAsignacion);

                //                var listaEvaluaciones = _context.Select<CalendarioEvaluacionDTO>(new DapperDTO
                //                {
                //                    baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                //                    consulta = @"
                //                          	 SELECT 								
                //								contrato.id as idContrato,
                //								contrato.cc as centroCosto,
                //								asignacion_id as idAsignacion,
                //                                fecha as Fecha,
                //                                contrato.subcontratistaID as idSubContratista,
                //								nombre AS SubContratista
                //								FROM tblCOES_Asignacion as asignacion                                
                //                                INNER JOIN tblCOES_Asignacion_Evaluacion as asigEval ON asignacion.id= asigEval.asignacion_id
                //                                INNER JOIN tblX_Contrato as contrato on asignacion.contrato_id = contrato.id
                //								LEFT JOIN tblX_SubContratista AS nomSubcontratista on nomSubcontratista.id = contrato.subcontratistaID",
                //                    //            where centroCosto in @cc AND idSubContratista in @subContratista",
                //                    //parametros = new { cc,subContratista }


                //                }).ToList();

                var listaEvaluacionesFecha = db.tblCOES_Asignacion_Evaluacion.Where(x => x.registroActivo == true).ToList();
                var listaAsignacion = db.tblCOES_Asignacion.Where(x => x.registroActivo).ToList();
                var listaContrato = db.tblX_Contrato.Where(e => e.estatus).ToList();
                var listaSubcontratistas = db.tblX_SubContratista.Where(e => e.estatus).ToList();

                List<CalendarioEvaluacionDTO> listaEvaluaciones = (from eval in listaEvaluacionesFecha
                                                                   join asig in listaAsignacion on eval.asignacion_id equals asig.id
                                                                   join contrato in listaContrato on asig.contrato_id equals contrato.id
                                                                   join subcontratista in listaSubcontratistas on contrato.subcontratistaID equals subcontratista.id
                                                                   select new CalendarioEvaluacionDTO
                                                                   {
                                                                       idContrato = contrato.id,
                                                                       centroCosto = contrato.cc,
                                                                       idAsignacion = asig.id,
                                                                       fecha = eval.fecha,
                                                                       idSubContratista = subcontratista.id,
                                                                       SubContratista = subcontratista.nombre,
                                                                       registroActivo = eval.registroActivo,
                                                                   }).ToList();

                List<CalendarioEvaluacionDTO> listFiltrada = new List<CalendarioEvaluacionDTO>();
                listFiltrada.AddRange(listaEvaluaciones.Where(e => e.registroActivo
                    && (lstFiltroCC != null ? lstFiltroCC.Contains(e.centroCosto) : true)
                    && (lstFiltroSubC != null ? lstFiltroSubC.Contains(e.idSubContratista) : true)
                     ).ToList());

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, listFiltrada);


            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> buscarEvaluaciones(List<string> cc, List<string> subContratistas)
        {
            try
            {
                var listaEvaluaciones = _context.Select<CalendarioEvaluacionDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.SUBCONTRATISTAS_GESTOR,
                    consulta = @"
                          	 SELECT 								
								contrato.id as idContrato,
								contrato.cc as centroCosto,
								asignacion_id as idAsignacion,
                                fecha as Fecha,
                                contrato.subcontratistaID as idSubContratista,
								nomSubcontratista.nombre AS SubContratista
								FROM tblCOES_Asignacion as asignacion                                
                                INNER JOIN tblCOES_Asignacion_Evaluacion as asigEval ON asignacion.id= asigEval.asignacion_id
                                INNER JOIN tblX_Contrato as contrato on asignacion.contrato_id = contrato.id
								LEFT JOIN tblX_SubContratista AS nomSubcontratista on nomSubcontratista.id = contrato.subcontratistaID
                                WHERE cc in @cc AND subcontratistaID in @subContratistas",
                    parametros = new { cc, subContratistas }

                }).ToList();

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, listaEvaluaciones);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }
        #endregion

        #region DASHBOARD

        #region GENERAL
        public Dictionary<string, object> GetGraficaCumplimientoPorSubContratista(List<string> lstFiltroCC, List<int> lstFiltroSubC, DateTime fechaFiltroInicio, DateTime fechaFiltroFin, int estado_id, int municipio_id, List<int> listaEspecialidades)
        {
            resultado.Clear();

            try
            {

                //FILTRO SUBCONTRATISTAS
                var lstSubContratistas = db.tblX_SubContratista.Where(e => e.estatus && lstFiltroSubC.Contains(e.id)).ToList();

                List<decimal> lstEvidenciasAprovadasEvaluacion = new List<decimal>();
                List<string> lstConceptosGrafica = new List<string>();

                foreach (var subC in lstSubContratistas)
                {
                    //FILTROS CC
                    //var lstContratos = db.tblX_Contrato.Where(e => e.estatus && subC.id == e.subcontratistaID && (lstFiltroCC != null ? lstFiltroCC.Contains(e.cc) : true)).ToList();
                    var lstContratos = db.tblX_Contrato.Where(e => e.estatus && subC.id == e.subcontratistaID).ToList().Where(e =>
                        (lstFiltroCC != null ? lstFiltroCC.Contains(e.cc) : true) &&
                        (estado_id > 0 ? e.estado_id == estado_id : true) &&
                        (municipio_id > 0 ? e.municipio_id == municipio_id : true)
                    ).ToList();
                    if (listaEspecialidades != null && listaEspecialidades.Count() > 0)
                    {
                        var listaEspecialidadesContratos = db.tblX_ContratotblCOES_Especialidad.Where(x => x.registroActivo && listaEspecialidades.Contains(x.especialidad_id)).Select(x => x.contrato_id).ToList();

                        lstContratos = lstContratos.Where(x => listaEspecialidadesContratos.Contains(x.id)).ToList();
                    }
                    var lstIdsContratos = lstContratos.Select(e => e.id).ToList();

                    int totalEvidenciasAprobadas = 0;
                    int totalRequerimientosACumplir = 0;

                    var lstPlantillas = db.tblCOES_PlantillatblX_Contrato.Where(e => e.registroActivo && lstIdsContratos.Contains(e.contrato_id)).ToList();
                    var lstIdsPlantillas = db.tblCOES_PlantillatblX_Contrato.Where(e => e.registroActivo && lstIdsContratos.Contains(e.contrato_id)).Select(e => e.plantilla_id).ToList();
                    var lstRelElementos = db.tblCOES_PlantillatblCOES_Elemento.Where(e => e.registroActivo && lstIdsPlantillas.Contains(e.plantilla_id)).ToList();
                    var lstIdRelPlantillaEle = lstRelElementos.Select(e => e.id).ToList();
                    var lstRequeremientos = db.tblCOES_PlantillatblCOES_ElementotblCOES_Requerimiento.Where(e => e.registroActivo && lstIdRelPlantillaEle.Contains(e.relacionPlantillaElemento_id)).ToList();

                    totalRequerimientosACumplir = lstRequeremientos.Count();

                    var lstAsignaciones = db.tblCOES_Asignacion.Where(e => e.registroActivo && lstIdsContratos.Contains(e.contrato_id)).ToList();
                    var lstIdsAsignaciones = db.tblCOES_Asignacion.Where(e => e.registroActivo && lstIdsContratos.Contains(e.contrato_id)).Select(e => e.id).ToList();
                    var lstEvaluaciones = db.tblCOES_Asignacion_Evaluacion.Where(e => e.registroActivo && lstIdsAsignaciones.Contains(e.asignacion_id) && (e.fecha >= fechaFiltroInicio && e.fecha <= fechaFiltroFin)).ToList();
                    var lstIdsEvaluaciones = lstEvaluaciones.Select(e => e.id).ToList();
                    var lstEvaluacionesAprobadas = new List<int>();

                    var lstEvalFirmas = db.tblCOES_Firma.Where(e => e.registroActivo && lstIdsContratos.Contains(e.contrato_id) && e.estadoFirma == EstadoFirmaEnum.AUTORIZADO).ToList();

                    foreach (var eval in lstEvaluaciones)
                    {
                        //FILTROS FECHAS
                        var lstEvalFirmasAprobadas = lstEvalFirmas.Where(e => e.evaluacion_id == eval.id).ToList();

                        if (lstEvalFirmasAprobadas != null && lstEvalFirmasAprobadas.Count() == 2)
                        {
                            lstEvaluacionesAprobadas.Add(eval.id);
                        }
                    }

                    var lstEvidencias = db.tblCOES_Evidencia.Where(e => e.registroActivo && lstIdsEvaluaciones.Contains(e.evaluacion_id)).ToList();
                    var lstGroupedEvidencias = lstEvidencias.GroupBy(e => e.evaluacion_id).Select(e => new { evaluacion = e.Key, lstEvidencias = e.ToList() }).ToList();

                    foreach (var eval in lstEvaluaciones)
                    {
                        if (lstEvaluacionesAprobadas.Contains(eval.id))
                        {
                            var lstEvidenciasAprobadas = lstEvidencias.Where(e => e.estatus == EstatusEvidenciaEnum.APROBADO && e.evaluacion_id == eval.id).Select(e => e.requerimiento_id).Distinct().ToList();
                            totalEvidenciasAprobadas += lstEvidenciasAprobadas.Count();
                        }
                    }

                    if (totalRequerimientosACumplir > 0)
                    {
                        int totalReqs = totalRequerimientosACumplir;
                        int totalReqsAprobadas = totalEvidenciasAprobadas;
                        decimal porcEvidenciasAprobadas = ((decimal)totalReqsAprobadas / (decimal)totalReqs) * 100;
                        //var porcEvidenciasAprobadas = Convert.ToInt32((lstEvidenciasAprobadas.Count() / lstRequerimientos.Count()) * 100);

                        lstConceptosGrafica.Add(subC.nombre);
                        lstEvidenciasAprovadasEvaluacion.Add(porcEvidenciasAprobadas);
                    }
                    else
                    {
                        lstConceptosGrafica.Add(subC.nombre);
                        lstEvidenciasAprovadasEvaluacion.Add(0);
                    }

                }



                resultado.Add("lstData", lstEvidenciasAprovadasEvaluacion);
                resultado.Add("lstConcepts", lstConceptosGrafica);
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, null);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> GetGraficaCumplimientoPorElementos(List<string> lstFiltroCC, List<int> lstFiltroSubC, DateTime fechaFiltroInicio, DateTime fechaFiltroFin, int estado_id, int municipio_id, List<int> listaEspecialidades)
        {
            resultado.Clear();

            try
            {
                //var lstContratos = db.tblX_Contrato.Where(e => e.estatus && lstFiltroSubC.Contains(e.subcontratistaID) && lstFiltroCC.Contains(e.cc)).ToList();
                var lstContratos = db.tblX_Contrato.Where(e => e.estatus).ToList().Where(x => (estado_id > 0 ? x.estado_id == estado_id : true) && (municipio_id > 0 ? x.municipio_id == municipio_id : true)).ToList();
                if (listaEspecialidades != null && listaEspecialidades.Count() > 0)
                {
                    var listaEspecialidadesContratos = db.tblX_ContratotblCOES_Especialidad.Where(x => x.registroActivo && listaEspecialidades.Contains(x.especialidad_id)).Select(x => x.contrato_id).ToList();

                    lstContratos = lstContratos.Where(x => listaEspecialidadesContratos.Contains(x.id)).ToList();
                }
                var lstIdsContratos = lstContratos.Select(e => e.id).ToList();

                var lstPlantillas = db.tblCOES_PlantillatblX_Contrato.Where(e => e.registroActivo && lstIdsContratos.Contains(e.contrato_id)).ToList();
                var lstIdsPlantillas = db.tblCOES_PlantillatblX_Contrato.Where(e => e.registroActivo && lstIdsContratos.Contains(e.contrato_id)).Select(e => e.plantilla_id).ToList();
                var lstRelElementos = db.tblCOES_PlantillatblCOES_Elemento.Where(e => e.registroActivo && lstIdsPlantillas.Contains(e.plantilla_id)).ToList();
                var lstIdRelPlantillaEle = lstRelElementos.Select(e => e.id).ToList();
                var lstRequeremientos = db.tblCOES_PlantillatblCOES_ElementotblCOES_Requerimiento.Where(e => e.registroActivo && lstIdRelPlantillaEle.Contains(e.relacionPlantillaElemento_id)).ToList();

                var lstAsignaciones = db.tblCOES_Asignacion.Where(e => e.registroActivo && lstIdsContratos.Contains(e.contrato_id)).ToList();
                var lstIdsAsignaciones = db.tblCOES_Asignacion.Where(e => e.registroActivo && lstIdsContratos.Contains(e.contrato_id)).Select(e => e.id).ToList();
                var lstEvaluaciones = db.tblCOES_Asignacion_Evaluacion.Where(e => e.registroActivo && lstIdsAsignaciones.Contains(e.asignacion_id) && (e.fecha >= fechaFiltroInicio && e.fecha <= fechaFiltroFin)).ToList();
                var lstIdsEvaluaciones = lstEvaluaciones.Select(e => e.id).ToList();
                var lstEvaluacionesAprobadas = new List<int>();

                var lstEvalFirmas = db.tblCOES_Firma.Where(e => e.registroActivo && lstIdsContratos.Contains(e.contrato_id) && e.estadoFirma == EstadoFirmaEnum.AUTORIZADO).ToList();

                foreach (var item in lstEvaluaciones)
                {
                    var lstEvalFirmasAprobadas = lstEvalFirmas.Where(e => e.evaluacion_id == item.id).ToList();

                    if (lstEvalFirmasAprobadas != null && lstEvalFirmasAprobadas.Count() == 2)
                    {
                        lstEvaluacionesAprobadas.Add(item.id);
                    }
                }


                List<decimal> lstEvidenciasAprovadasEvaluacion = new List<decimal>();
                List<string> lstConceptosGrafica = new List<string>();
                List<GraficaCumplimientoElementosDTO> lstDatosGrafica = new List<GraficaCumplimientoElementosDTO>();

                foreach (var eval in lstEvaluaciones)
                {
                    var objAsignacion = lstAsignaciones.FirstOrDefault(e => e.id == eval.asignacion_id);
                    var objPlantilla = lstPlantillas.FirstOrDefault(e => e.contrato_id == objAsignacion.contrato_id);
                    var lstDetRelElementos = db.tblCOES_PlantillatblCOES_Elemento.Where(e => e.registroActivo && objPlantilla.plantilla_id == e.plantilla_id).ToList();

                    var lstEvidencias = db.tblCOES_Evidencia.Where(e => e.registroActivo && eval.id == e.evaluacion_id).ToList();


                    foreach (var item in lstDetRelElementos)
                    {
                        var objCalculo = lstDatosGrafica.FirstOrDefault(e => e.id_elemento == item.elemento_id && e.id_plantilla == item.plantilla_id);

                        var objElemento = db.tblCOES_Elemento.FirstOrDefault(e => e.id == item.elemento_id);
                        var lstGroupedRequerimientos = lstRequeremientos.Where(e => e.relacionPlantillaElemento_id == item.id).Select(e => e.requerimiento_id).ToList();

                        var lstEvidenciasAprobadas = lstEvidencias.Where(e => e.estatus == EstatusEvidenciaEnum.APROBADO && lstGroupedRequerimientos.Contains(e.requerimiento_id)).Select(e => e.requerimiento_id).Distinct().ToList();

                        if (objCalculo != null)
                        {
                            if (lstEvaluacionesAprobadas.Contains(eval.id))
                            {
                                if (lstEvidenciasAprobadas.Count() > 0)
                                {
                                    objCalculo.totalRequeridas += lstGroupedRequerimientos.Count();
                                    objCalculo.totalAutorizadas += lstEvidenciasAprobadas.Count();
                                    //porcEvidenciasAprobadas = ((decimal)totalReqsAprobadas / (decimal)totalReqs) * 100;
                                }
                            }

                        }
                        else
                        {
                            if (lstEvaluacionesAprobadas.Contains(eval.id))
                            {
                                lstDatosGrafica.Add(new GraficaCumplimientoElementosDTO
                                {
                                    id_elemento = item.elemento_id,
                                    id_plantilla = item.plantilla_id,
                                    totalAutorizadas = lstEvidenciasAprobadas.Count(),
                                    totalRequeridas = lstGroupedRequerimientos.Count(),
                                    descElemento = objElemento.descripcion,
                                });
                            }
                            else
                            {
                                lstDatosGrafica.Add(new GraficaCumplimientoElementosDTO
                                {
                                    id_elemento = item.elemento_id,
                                    id_plantilla = item.plantilla_id,
                                    totalAutorizadas = 0,
                                    totalRequeridas = 0,
                                    descElemento = objElemento.descripcion,
                                });
                            }
                        }
                    }
                }

                foreach (var item in lstDatosGrafica)
                {
                    decimal porcEvidenciasAprobadas = 0;
                    if (item.totalRequeridas > 0)
                    {
                        porcEvidenciasAprobadas = ((decimal)item.totalAutorizadas / (decimal)item.totalRequeridas) * 100;

                    }
                    lstEvidenciasAprovadasEvaluacion.Add(porcEvidenciasAprobadas);
                    lstConceptosGrafica.Add(item.descElemento);
                }


                resultado.Add("lstData", lstEvidenciasAprovadasEvaluacion);
                resultado.Add("lstConcepts", lstConceptosGrafica);
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, null);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> GetGraficaCumplimientoPorEvaluacion(List<string> lstFiltroCC, List<int> lstFiltroSubC, DateTime fechaFiltroInicio, DateTime fechaFiltroFin, int estado_id, int municipio_id, List<int> listaEspecialidades)
        {
            resultado.Clear();

            try
            {

                var lstContratos = db.tblX_Contrato.Where(e => e.estatus && lstFiltroSubC.Contains(e.subcontratistaID)).ToList().Where(e =>
                    (lstFiltroCC != null ? lstFiltroCC.Contains(e.cc) : true) &&
                    (estado_id > 0 ? e.estado_id == estado_id : true) &&
                    (municipio_id > 0 ? e.municipio_id == municipio_id : true)
                ).ToList();
                if (listaEspecialidades != null && listaEspecialidades.Count() > 0)
                {
                    var listaEspecialidadesContratos = db.tblX_ContratotblCOES_Especialidad.Where(x => x.registroActivo && listaEspecialidades.Contains(x.especialidad_id)).Select(x => x.contrato_id).ToList();

                    lstContratos = lstContratos.Where(x => listaEspecialidadesContratos.Contains(x.id)).ToList();
                }
                var lstIdsContratos = lstContratos.Select(e => e.id).ToList();

                var lstPlantillas = db.tblCOES_PlantillatblX_Contrato.Where(e => e.registroActivo && lstIdsContratos.Contains(e.contrato_id)).ToList();
                var lstIdsPlantillas = db.tblCOES_PlantillatblX_Contrato.Where(e => e.registroActivo && lstIdsContratos.Contains(e.contrato_id)).Select(e => e.plantilla_id).ToList();
                var lstElementos = db.tblCOES_PlantillatblCOES_Elemento.Where(e => e.registroActivo && lstIdsPlantillas.Contains(e.plantilla_id)).ToList();
                var lstIdRelPlantillaEle = lstElementos.Select(e => e.id).ToList();
                var lstRequeremientos = db.tblCOES_PlantillatblCOES_ElementotblCOES_Requerimiento.Where(e => e.registroActivo && lstIdRelPlantillaEle.Contains(e.relacionPlantillaElemento_id)).ToList();

                var lstAsignaciones = db.tblCOES_Asignacion.Where(e => e.registroActivo && lstIdsContratos.Contains(e.contrato_id)).ToList();
                var lstIdsAsignaciones = db.tblCOES_Asignacion.Where(e => e.registroActivo && lstIdsContratos.Contains(e.contrato_id)).Select(e => e.id).ToList();
                var lstEvaluaciones = db.tblCOES_Asignacion_Evaluacion.Where(e => e.registroActivo && lstIdsAsignaciones.Contains(e.asignacion_id) && (e.fecha >= fechaFiltroInicio && e.fecha <= fechaFiltroFin)).ToList();
                var lstIdsEvaluaciones = lstEvaluaciones.Select(e => e.id).ToList();
                var lstEvaluacionesAprobadas = new List<int>();

                var lstEvalFirmas = db.tblCOES_Firma.Where(e => e.registroActivo && lstIdsContratos.Contains(e.contrato_id) && e.estadoFirma == EstadoFirmaEnum.AUTORIZADO).ToList();

                foreach (var item in lstEvaluaciones)
                {
                    var lstEvalFirmasAprobadas = lstEvalFirmas.Where(e => e.evaluacion_id == item.id).ToList();

                    if (lstEvalFirmasAprobadas != null && lstEvalFirmasAprobadas.Count() == 2)
                    {
                        lstEvaluacionesAprobadas.Add(item.id);
                    }
                }

                var lstEvidencias = db.tblCOES_Evidencia.Where(e => e.registroActivo && lstIdsEvaluaciones.Contains(e.evaluacion_id)).ToList();
                //var lstGroupedEvidencias = lstEvidencias.GroupBy(e => e.evaluacion_id).Select(e => new {evaluacion = e.Key, lstEvidencias = e.ToList()}).ToList();

                List<decimal> lstEvidenciasAprovadasEvaluacion = new List<decimal>();
                List<string> lstConceptosGrafica = new List<string>();

                foreach (var item in lstEvaluaciones)
                {

                    var objEvaluacion = lstEvaluaciones.FirstOrDefault(e => e.id == item.id);
                    var objRelAsig = lstAsignaciones.FirstOrDefault(e => e.id == objEvaluacion.asignacion_id);
                    var objContrato = lstContratos.FirstOrDefault(e => e.id == objRelAsig.contrato_id);
                    var objPlantilla = lstPlantillas.FirstOrDefault(e => e.contrato_id == objContrato.id);
                    var lstElementosEvidencia = lstElementos.Where(e => e.plantilla_id == objPlantilla.plantilla_id).Select(e => e.id).ToList();
                    var lstRequerimientos = lstRequeremientos.Where(e => lstElementosEvidencia.Contains(e.relacionPlantillaElemento_id)).Select(e => e.requerimiento_id).ToList();

                    var lstEvidenciasAprobadas = lstEvidencias.Where(e => e.estatus == EstatusEvidenciaEnum.APROBADO && e.evaluacion_id == item.id).Select(e => e.requerimiento_id).Distinct().ToList();

                    int totalReqs = 0;
                    int totalReqsAprobadas = 0;
                    decimal porcEvidenciasAprobadas = 0;

                    if (lstEvaluacionesAprobadas.Contains(item.id))
                    {
                        if (lstRequerimientos.Count() > 0)
                        {
                            totalReqs = lstRequerimientos.Count();
                            totalReqsAprobadas = lstEvidenciasAprobadas.Count();
                            porcEvidenciasAprobadas = ((decimal)totalReqsAprobadas / (decimal)totalReqs) * 100;

                        }

                    }
                    else
                    {
                        porcEvidenciasAprobadas = 0;
                    }

                    lstConceptosGrafica.Add(objContrato.cc + " " + objContrato.subcontratista.nombre + " " + objEvaluacion.fecha.ToString("dd/MM/yyyy"));
                    lstEvidenciasAprovadasEvaluacion.Add(porcEvidenciasAprobadas);
                }

                resultado.Add("lstData", lstEvidenciasAprovadasEvaluacion);
                resultado.Add("lstConcepts", lstConceptosGrafica);
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, null);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public List<string[]> GetInfoTablaReporteDashboard(List<string> lstFiltroCC, List<int> lstFiltroSubC, DateTime fechaFiltroInicio, DateTime fechaFiltroFin, int estado_id, int municipio_id, List<int> listaEspecialidades)
        {
            List<ReporteDashboardEvaluacionDTO> tablaReporte = new List<ReporteDashboardEvaluacionDTO>();
            List<string[]> tablaStrReporte = new List<string[]>();

            try
            {
                var lstSubContratistas = db.tblX_SubContratista.Where(e => e.estatus && lstFiltroSubC.Contains(e.id)).ToList();
                var lstCC = _context.tblP_CC.Where(e => e.estatus).ToList();

                List<decimal> lstEvidenciasAprovadasEvaluacion = new List<decimal>();
                List<string> lstConceptosGrafica = new List<string>();

                foreach (var subC in lstSubContratistas)
                {
                    //FILTROS CC
                    //var lstContratos = db.tblX_Contrato.Where(e => e.estatus && subC.id == e.subcontratistaID && (lstFiltroCC != null ? lstFiltroCC.Contains(e.cc) : true)).ToList();
                    var lstContratos = db.tblX_Contrato.Where(e => e.estatus && subC.id == e.subcontratistaID).ToList().Where(e =>
                        (lstFiltroCC != null ? lstFiltroCC.Contains(e.cc) : true) &&
                        (estado_id > 0 ? e.estado_id == estado_id : true) &&
                        (municipio_id > 0 ? e.municipio_id == municipio_id : true)
                    ).ToList();
                    if (listaEspecialidades != null && listaEspecialidades.Count() > 0)
                    {
                        var listaEspecialidadesContratos = db.tblX_ContratotblCOES_Especialidad.Where(x => x.registroActivo && listaEspecialidades.Contains(x.especialidad_id)).Select(x => x.contrato_id).ToList();

                        lstContratos = lstContratos.Where(x => listaEspecialidadesContratos.Contains(x.id)).ToList();
                    }
                    var lstIdsContratos = lstContratos.Select(e => e.id).ToList();

                    var lstPlantillas = db.tblCOES_PlantillatblX_Contrato.Where(e => e.registroActivo && lstIdsContratos.Contains(e.contrato_id)).ToList();
                    var lstIdsPlantillas = db.tblCOES_PlantillatblX_Contrato.Where(e => e.registroActivo && lstIdsContratos.Contains(e.contrato_id)).Select(e => e.plantilla_id).ToList();
                    var lstRelElementos = db.tblCOES_PlantillatblCOES_Elemento.Where(e => e.registroActivo && lstIdsPlantillas.Contains(e.plantilla_id)).ToList();
                    var lstIdRelPlantillaEle = lstRelElementos.Select(e => e.id).ToList();
                    var lstRequeremientos = db.tblCOES_PlantillatblCOES_ElementotblCOES_Requerimiento.Where(e => e.registroActivo && lstIdRelPlantillaEle.Contains(e.relacionPlantillaElemento_id)).ToList();

                    var lstAsignaciones = db.tblCOES_Asignacion.Where(e => e.registroActivo && lstIdsContratos.Contains(e.contrato_id)).ToList();
                    var lstIdsAsignaciones = db.tblCOES_Asignacion.Where(e => e.registroActivo && lstIdsContratos.Contains(e.contrato_id)).Select(e => e.id).ToList();
                    var lstEvaluaciones = db.tblCOES_Asignacion_Evaluacion.Where(e => e.registroActivo && lstIdsAsignaciones.Contains(e.asignacion_id) && (e.fecha >= fechaFiltroInicio && e.fecha <= fechaFiltroFin)).ToList();
                    var lstIdsEvaluaciones = lstEvaluaciones.Select(e => e.id).ToList();
                    var lstEvaluacionesAprobadas = new List<int>();

                    var lstEvalFirmas = db.tblCOES_Firma.Where(e => e.registroActivo && lstIdsContratos.Contains(e.contrato_id) && e.estadoFirma == EstadoFirmaEnum.AUTORIZADO).ToList();

                    foreach (var eval in lstEvaluaciones)
                    {
                        //FILTROS FECHAS
                        var lstEvalFirmasAprobadas = lstEvalFirmas.Where(e => e.evaluacion_id == eval.id).ToList();

                        if (lstEvalFirmasAprobadas != null && lstEvalFirmasAprobadas.Count() == 2)
                        {
                            lstEvaluacionesAprobadas.Add(eval.id);
                        }
                    }

                    //var lstEvidencias = db.tblCOES_Evidencia.Where(e => e.registroActivo && lstIdsEvaluaciones.Contains(e.evaluacion_id)).ToList();
                    //var lstGroupedEvidencias = lstEvidencias.GroupBy(e => e.evaluacion_id).Select(e => new { evaluacion = e.Key, lstEvidencias = e.ToList() }).ToList();

                    List<ReporteDashboardEvaluacionesContratoDTO> lstTotalesEvaluacionesContrato = new List<ReporteDashboardEvaluacionesContratoDTO>();
                    List<GraficaCumplimientoElementosDTO> lstEvaluacionesReporteElementos = new List<GraficaCumplimientoElementosDTO>();

                    foreach (var eval in lstEvaluaciones)
                    {

                        var objAsignacion = lstAsignaciones.FirstOrDefault(e => e.id == eval.asignacion_id);
                        var objContrato = lstContratos.FirstOrDefault(e => e.id == objAsignacion.contrato_id);
                        var objPlantilla = lstPlantillas.FirstOrDefault(e => e.contrato_id == objAsignacion.contrato_id);
                        var lstDetRelElementos = db.tblCOES_PlantillatblCOES_Elemento.Where(e => e.registroActivo && objPlantilla.plantilla_id == e.plantilla_id).ToList();

                        var lstEvidencias = db.tblCOES_Evidencia.Where(e => e.registroActivo && eval.id == e.evaluacion_id).ToList();

                        int totalEvidenciasAprobadas = 0;
                        int totalRequerimientosACumplir = 0;

                        int totalEvidenciasTiempo = 0;
                        int totalRequerimientosTiempoACumplir = 0;

                        int totalEvidenciasCompromiso = 0;
                        int totalRequerimientosCompromisoACumplir = 0;

                        foreach (var item in lstDetRelElementos)
                        {
                            var objElemento = db.tblCOES_Elemento.FirstOrDefault(e => e.id == item.elemento_id);
                            var lstIdsRequerimientos = lstRequeremientos.Where(e => e.relacionPlantillaElemento_id == item.id).Select(e => e.requerimiento_id).ToList();

                            var lstEvidenciasTiempo = lstEvidencias.Where(e => lstIdsRequerimientos.Contains(e.requerimiento_id)).ToList();
                            var lstIdsEvidenciasTiempoU = lstEvidencias.Where(e => lstIdsRequerimientos.Contains(e.requerimiento_id)).Select(e => e.requerimiento_id).Distinct().ToList();
                            var lstEvidenciasAprobadas = lstEvidenciasTiempo.Where(e => e.estatus == EstatusEvidenciaEnum.APROBADO).Select(e => e.requerimiento_id).Distinct().ToList();
                            var lstEvidenciasCompromiso = lstEvidenciasTiempo.Where(e => e.tipo == TipoEvidenciaEnum.COMPROMISO).ToList();
                            var lstEvidenciasCompromisoAprobadas = lstEvidenciasCompromiso.Where(e => e.estatus == EstatusEvidenciaEnum.APROBADO).ToList();

                            if (lstEvidenciasCompromiso.Count > 0)
                            {
                                var lstGroupedEvidenciasCompromiso = lstEvidenciasCompromisoAprobadas.GroupBy(e => e.requerimiento_id).Select(e => new { requerimiento = e.Key, lstGroupedReqs = e.OrderBy(ee => ee.fechaCompromiso) }).ToList();

                                int requerimientosCompromisoAprobados = 0;

                                if (lstGroupedEvidenciasCompromiso.Count() > 0)
	                            {
		                            foreach (var group in lstGroupedEvidenciasCompromiso)
	                                {
                                        var objReq = group.lstGroupedReqs.FirstOrDefault();

                                        if (objReq != null && objReq.fechaCompromiso <= objReq.fechaCompromiso)
                                        {
                                            requerimientosCompromisoAprobados++;
                                        }
	                                }
	                            }

                                totalEvidenciasCompromiso += requerimientosCompromisoAprobados;
                                totalRequerimientosCompromisoACumplir += lstIdsRequerimientos.Count();
                            }

                            if (lstEvidenciasTiempo.Count > 0)
                            {
                                totalEvidenciasTiempo += lstEvidenciasTiempo.Count();
                                totalRequerimientosTiempoACumplir += lstIdsRequerimientos.Count();
                            }

                            var objReporteElemento = lstEvaluacionesReporteElementos.FirstOrDefault(e => e.id_elemento == item.elemento_id && e.id_plantilla == item.plantilla_id);

                            if (objReporteElemento != null)
                            {

                                if (lstEvaluacionesAprobadas.Contains(eval.id))
                                {
                                    //TOTALES POR EVALUACION
                                    totalEvidenciasAprobadas += lstEvidenciasAprobadas.Count();
                                    totalRequerimientosACumplir += lstIdsRequerimientos.Count();

                                    int totalReqs = 0;
                                    int totalReqsAprobadas = 0;
                                    decimal porcEvidenciasAprobadas = 0;

                                    if (lstEvidenciasAprobadas.Count() > 0)
                                    {
                                        totalReqs = lstIdsRequerimientos.Count();
                                        totalReqsAprobadas = lstEvidenciasAprobadas.Count();
                                        porcEvidenciasAprobadas = ((decimal)totalReqsAprobadas / (decimal)totalReqs) * 100;
                                    }

                                    objReporteElemento.totalAutorizadas += totalReqsAprobadas;
                                    objReporteElemento.totalRequeridas += totalReqs;
                                }
                            }
                            else
                            {

                                if (lstEvaluacionesAprobadas.Contains(eval.id))
                                {
                                    //TOTALES POR EVALUACION
                                    totalEvidenciasAprobadas += lstEvidenciasAprobadas.Count();
                                    totalRequerimientosACumplir += lstIdsRequerimientos.Count();

                                    int totalReqs = 0;
                                    int totalReqsAprobadas = 0;
                                    decimal porcEvidenciasAprobadas = 0;

                                    if (lstEvidenciasAprobadas.Count() > 0)
                                    {
                                        totalReqs = lstIdsRequerimientos.Count();
                                        totalReqsAprobadas = lstEvidenciasAprobadas.Count();
                                        porcEvidenciasAprobadas = ((decimal)totalReqsAprobadas / (decimal)totalReqs) * 100;
                                    }
                                    else
                                    {
                                        totalReqs = lstIdsRequerimientos.Count();
                                        porcEvidenciasAprobadas = 0;

                                    }

                                    lstEvaluacionesReporteElementos.Add(new GraficaCumplimientoElementosDTO
                                    {
                                        id_elemento = item.elemento_id,
                                        id_plantilla = item.plantilla_id,
                                        id_contrato = objContrato.id,
                                        totalAutorizadas = totalReqsAprobadas,
                                        totalRequeridas = totalReqs,
                                        descElemento = objElemento.descripcion,
                                    });
                                }
                                else
                                {
                                    lstEvaluacionesReporteElementos.Add(new GraficaCumplimientoElementosDTO
                                    {
                                        id_elemento = item.elemento_id,
                                        id_plantilla = item.plantilla_id,
                                        id_contrato = objContrato.id,
                                        totalAutorizadas = 0,
                                        totalRequeridas = 0,
                                        porcAutorizado = 0M,
                                        descElemento = objElemento.descripcion,
                                    });
                                }
                            }
                        }

                        decimal porcEvalEvidenciasAprobadas = 0;
                        //int diffEvalEvidencias = 0;

                        if (totalRequerimientosACumplir > 0)
                        {
                            porcEvalEvidenciasAprobadas = ((decimal)totalEvidenciasAprobadas / (decimal)totalRequerimientosACumplir) * 100;
                            //diffEvalEvidencias = totalEvidenciasAprobadas - totalRequerimientosACumplir;
                        }

                        lstTotalesEvaluacionesContrato.Add(new ReporteDashboardEvaluacionesContratoDTO
                        {
                            id_evaluacion = eval.id,
                            id_contrato = objContrato.id,
                            contratoAutorizadas = totalEvidenciasAprobadas,
                            contratoRequeridas = totalRequerimientosACumplir,
                            contratoCompromisoAutorizadas = totalEvidenciasCompromiso,
                            contratoCompromismoRequeridas = totalRequerimientosCompromisoACumplir,
                            contratoTiempoAutorizadas = totalEvidenciasTiempo,
                            contratoTiempoRequeridas = totalRequerimientosTiempoACumplir,
                        });
                    }

                    foreach (var item in lstAsignaciones)
                    {
                        var objContrato = lstContratos.FirstOrDefault(e => e.id == item.contrato_id);

                        foreach (var elemento in lstRelElementos)
                        {
                            var objElemento = lstEvaluacionesReporteElementos.FirstOrDefault(e => e.id_elemento == elemento.elemento_id && item.contrato_id == e.id_contrato);

                            if (objElemento != null)
                            {
                                int diffElementosContrato = 0;
                                decimal porcElementosContrato = 0;

                                var totalElementosRequerimientos = objElemento.totalRequeridas;
                                var totalElementosAprobadas = objElemento.totalAutorizadas;

                                if (totalElementosRequerimientos > 0)
                                {
                                    diffElementosContrato = totalElementosRequerimientos - totalElementosAprobadas;
                                    porcElementosContrato = ((decimal)objElemento.totalAutorizadas / (decimal)objElemento.totalRequeridas) * 100;
                                }

                                objElemento.porcAutorizado = porcElementosContrato;
                            }
                            else
                            {
                                var objNewElemento = db.tblCOES_Elemento.FirstOrDefault(e => e.id == elemento.elemento_id);
                                lstEvaluacionesReporteElementos.Add(new GraficaCumplimientoElementosDTO
                                {
                                    id_elemento = elemento.elemento_id,
                                    id_plantilla = elemento.plantilla_id,
                                    id_contrato = item.contrato_id,
                                    totalAutorizadas = 0,
                                    totalRequeridas = 0,
                                    porcAutorizado = -1M,
                                    descElemento = objNewElemento.descripcion,
                                });
                            }


                        }

                        int diffReqsContrato = 0;
                        decimal porcReqsContrato = 0;

                        decimal porcReqsCompromisoContrato = 0;
                        decimal porcReqsTiempoContrato = 0;

                        var totalRequerimientos = lstTotalesEvaluacionesContrato.Sum(e => e.contratoRequeridas);
                        var totalAprobadas = lstTotalesEvaluacionesContrato.Sum(e => e.contratoAutorizadas);

                        var totalCompromisoRequerimientos = lstTotalesEvaluacionesContrato.Sum(e => e.contratoCompromismoRequeridas);
                        var totalCompromisoAprobadas = lstTotalesEvaluacionesContrato.Sum(e => e.contratoCompromisoAutorizadas);

                        var totalTiempoRequerimientos = lstTotalesEvaluacionesContrato.Sum(e => e.contratoTiempoRequeridas);
                        var totalTiempoAprobadas = lstTotalesEvaluacionesContrato.Sum(e => e.contratoTiempoAutorizadas);

                        //int diffEvalEvidencias = 0;

                        if (totalRequerimientos > 0)
                        {
                            diffReqsContrato = totalRequerimientos - totalAprobadas;
                            porcReqsContrato = ((decimal)totalAprobadas / (decimal)totalRequerimientos) * 100;
                        }

                        if (totalCompromisoRequerimientos > 0 && totalCompromisoRequerimientos > 0)
                        {
                            porcReqsCompromisoContrato = ((decimal)totalCompromisoAprobadas / (decimal)totalCompromisoRequerimientos) * 100;
                        }
                        else 
                        {
                            porcReqsCompromisoContrato = 100;
                        }

                        if (totalTiempoRequerimientos > 0)
                        {
                            porcReqsTiempoContrato = ((decimal)totalTiempoAprobadas / (decimal)totalTiempoRequerimientos) * 100;
                        }

                        var objCC = lstCC.FirstOrDefault(e => e.cc == objContrato.cc);
                        string ccDesc = "";

                        if (objCC != null)
                        {
                            ccDesc = "[" + objContrato.cc + "] " + objCC.descripcion;
                        }

		                tablaReporte.Add(new ReporteDashboardEvaluacionDTO
                        {
                            cc = ccDesc,
                            contrato = objContrato.numeroContrato,
                            nombreSucC = subC.nombre,
                            periodoEval = objContrato.fechaSuscripcion.Value.ToString("dd/MM/yyyy") + " -> " + objContrato.fechaVigencia.Value.ToString("dd/MM/yyyy"),
                            calificacionGlobal = porcReqsContrato,
                            realizadas = totalRequerimientos,
                            aprobadas = totalAprobadas,
                            noAprobadas = diffReqsContrato,
                            porcAprobadas = porcReqsContrato,
                            lstElementosEvaluados = lstEvaluacionesReporteElementos.Where(e => e.id_contrato == item.contrato_id).OrderBy(e => e.descElemento).ToList(),
                            cumplimientoTiempo = porcReqsTiempoContrato,
                            cumplimientoCompromiso = porcReqsCompromisoContrato,
                        });
                    }


                    foreach (var item in tablaReporte)
                    {
                        //HEADER DINAMICO
                        if (tablaStrReporte.Count() == 0)
                        {
                            tablaStrReporte.Add(item.lstElementosEvaluados.Select(e => e.descElemento).ToArray());
                        }

                        int rowLength = 11 + item.lstElementosEvaluados.Count();
                        int tempRowLength = rowLength;

                        string[] rowRpt = new string[rowLength];
                        rowRpt[0] = item.cc;
                        rowRpt[1] = item.contrato;
                        rowRpt[2] = item.nombreSucC;
                        rowRpt[3] = item.periodoEval;
                        rowRpt[4] = item.calificacionGlobal.ToString() + " %";
                        rowRpt[5] = item.realizadas.ToString();
                        rowRpt[6] = item.aprobadas.ToString();
                        rowRpt[7] = item.noAprobadas.ToString();
                        rowRpt[8] = item.porcAprobadas.ToString("#.#") + " %";
                        rowRpt[tempRowLength - 2] = item.cumplimientoTiempo.ToString() + " %";
                        tempRowLength = rowLength;
                        rowRpt[tempRowLength - 1] = item.cumplimientoCompromiso.ToString() + " %";


                        for (int i = 0; i < item.lstElementosEvaluados.Count(); i++)
                        {
                            int j = i;

                            rowRpt[j + 9] = item.lstElementosEvaluados[i].porcAutorizado < 0 ? " " : (item.lstElementosEvaluados[i].porcAutorizado == 0 ? "0 %" : (item.lstElementosEvaluados[i].porcAutorizado.ToString("#.#") + " %"));

                        }
                        tablaStrReporte.Add(rowRpt);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return tablaStrReporte;
        }

        public MemoryStream crearReporte(List<string> lstFiltroCC, List<int> lstFiltroSubC, DateTime fechaFiltroInicio, DateTime fechaFiltroFin, int estado_id, int municipio_id, List<int> listaEspecialidades)
        {
            try
            {
                using (ExcelPackage excel = new ExcelPackage())
                {

                    var hoja1 = excel.Workbook.Worksheets.Add("Evaluacion");

                    var lstReporte = GetInfoTablaReporteDashboard(lstFiltroCC, lstFiltroSubC, fechaFiltroInicio, fechaFiltroFin, estado_id, municipio_id, listaEspecialidades);

                    #region Titulo

                    switch (vSesiones.sesionEmpresaActual)
                    {
                        case (int)MainContextEnum.Construplan:
                            hoja1.Cells["D1"].Value = "GRUPO CONSTRUCCIONES PLANIFICADAS SA DE CV.";

                            break;
                        case (int)MainContextEnum.Arrendadora:
                            hoja1.Cells["D1"].Value = "ARRENDADORA CONSTRUPLAN SA DE CV";

                            break;
                        case (int)MainContextEnum.EICI:
                            hoja1.Cells["D1"].Value = "EICI";

                            break;
                        case (int)MainContextEnum.INTEGRADORA:
                            hoja1.Cells["D1"].Value = "INTEGRADORA";

                            break;
                    }

                    Color colFromHexBlack = System.Drawing.ColorTranslator.FromHtml("#000000");
                    Color colFromHexOrange = System.Drawing.ColorTranslator.FromHtml("#f99645");

                    string ccDesc = "";
                    string subCDesc = "";

                    if (lstFiltroSubC != null && lstFiltroSubC.Count() == 1)
                    {
                        subCDesc = lstReporte[1][2] ?? "";
                    }

                    if (lstFiltroCC != null && lstFiltroCC.Count() == 1)
                    {
                        ccDesc = lstReporte[1][0] ?? "";
                    }

                    hoja1.Cells["C1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    hoja1.Cells["D1:F1"].Merge = true;
                    hoja1.Cells["D1"].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                    hoja1.Cells["F1"].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                    hoja1.Cells["D1:F1"].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                    hoja1.Cells["D1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Cells["D1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja1.Cells["D1"].Style.Font.Bold = true;
                    hoja1.Cells["D1"].Style.Font.Size = 14;

                    hoja1.Cells["D2:F2"].Merge = true;
                    hoja1.Cells["D2"].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                    hoja1.Cells["F2"].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                    hoja1.Cells["D2:F2"].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                    hoja1.Cells["D2"].Value = "PMO Oficina de Gestión de Proyectos";
                    hoja1.Cells["D2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Cells["D2"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja1.Cells["D2"].Style.Font.Bold = true;
                    hoja1.Cells["D2"].Style.Font.Size = 16;

                    hoja1.Cells["D3:F3"].Merge = true;
                    hoja1.Cells["D3"].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                    hoja1.Cells["F3"].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                    hoja1.Cells["D3:F3"].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                    hoja1.Cells["D3"].Value = "EVALUACION A SUBCONTRATISTAS";
                    hoja1.Cells["D3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Cells["D3"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja1.Cells["D3"].Style.Font.Bold = true;
                    hoja1.Cells["D3"].Style.Font.Size = 16;
                    hoja1.Cells["D3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells["D3"].Style.Fill.BackgroundColor.SetColor(colFromHexOrange);

                    hoja1.Cells["D4:F4"].Merge = true;
                    hoja1.Cells["D4"].Value = "Fecha: " + DateTime.Now.ToString("dd/MM/yyyy");
                    hoja1.Cells["D4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    hoja1.Cells["D8:F8"].Merge = true;
                    hoja1.Cells["D8:F8"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    hoja1.Cells["D8"].Value = ccDesc;
                    hoja1.Cells["C8"].Value = "CC";
                    hoja1.Cells["C8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    hoja1.Cells["D9:F9"].Merge = true;
                    hoja1.Cells["D9:F9"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    hoja1.Cells["D9"].Value = subCDesc;
                    hoja1.Cells["C9"].Value = "Subcontratista";
                    hoja1.Cells["C9"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    hoja1.Cells["D10:F10"].Merge = true;
                    hoja1.Cells["D10:F10"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    hoja1.Cells["D10"].Value = "";
                    hoja1.Cells["C10"].Value = "Estado";
                    hoja1.Cells["C10"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    hoja1.Cells["D11:F11"].Merge = true;
                    hoja1.Cells["D11:F11"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    hoja1.Cells["D11"].Value = "";
                    hoja1.Cells["C11"].Value = "Municipio";
                    hoja1.Cells["C11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    hoja1.Cells["D12:F12"].Merge = true;
                    hoja1.Cells["D12:F12"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    hoja1.Cells["D12"].Value = "";
                    hoja1.Cells["C12"].Value = "Especialidad";
                    hoja1.Cells["C12"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    #endregion

                    List<string> headerRow1 = new List<string>() { "CC", "Contrato", "Subcontratista", "Periodo", "Calificación global", "Evaluaciones", "Elementos evaluados", "Cumplimiento carga de soportes (Tiempo)", "Cumplimiento compromisos", };
                    List<string> headerRow2 = new List<string>() { "Realizadas", "Aprobadas", "NO aprobadas", "%" };
                    int headerLength = lstReporte[1].Length;
                    int tempHeaderLength = headerLength;


                    hoja1.Cells[15, 2].Value = headerRow1[0];
                    hoja1.Cells[15, 3].Value = headerRow1[1];
                    hoja1.Cells[15, 4].Value = headerRow1[2];
                    hoja1.Cells[15, 5].Value = headerRow1[3];
                    hoja1.Cells[15, 6].Value = headerRow1[4];
                    hoja1.Cells[15, 7].Value = headerRow2[0];
                    hoja1.Cells[15, 8].Value = headerRow2[1];
                    hoja1.Cells[15, 9].Value = headerRow2[2];
                    hoja1.Cells[15, 10].Value = headerRow2[3];

                    hoja1.Cells[14, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[14, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[14, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[14, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[14, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[14, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[14, 8].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[14, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[14, 10].Style.Fill.PatternType = ExcelFillStyle.Solid;

                    hoja1.Cells[14, 2].Style.Fill.BackgroundColor.SetColor(colFromHexBlack);
                    hoja1.Cells[14, 3].Style.Fill.BackgroundColor.SetColor(colFromHexBlack);
                    hoja1.Cells[14, 4].Style.Fill.BackgroundColor.SetColor(colFromHexBlack);
                    hoja1.Cells[14, 5].Style.Fill.BackgroundColor.SetColor(colFromHexBlack);
                    hoja1.Cells[14, 6].Style.Fill.BackgroundColor.SetColor(colFromHexBlack);
                    hoja1.Cells[14, 7].Style.Fill.BackgroundColor.SetColor(colFromHexBlack);
                    hoja1.Cells[14, 8].Style.Fill.BackgroundColor.SetColor(colFromHexBlack);
                    hoja1.Cells[14, 9].Style.Fill.BackgroundColor.SetColor(colFromHexBlack);
                    hoja1.Cells[14, 10].Style.Fill.BackgroundColor.SetColor(colFromHexBlack);

                    int j = 0;
                    for (int i = 11; i < lstReporte[0].Length + 11; i++)
                    {
                        headerRow2.Add(lstReporte[0][j]);
                        hoja1.Cells[15, i].Value = lstReporte[0][j];

                        j++;
                    }

                    hoja1.Cells["G14:J14"].Merge = true;
                    hoja1.Cells["G14"].Value = "Evaluaciones";
                    hoja1.Cells["G14"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Cells["G14"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells["G14"].Style.Fill.BackgroundColor.SetColor(colFromHexBlack);
                    hoja1.Cells["G14"].Style.Font.Color.SetColor(Color.White);

                    hoja1.Cells[14, 11, 14, lstReporte[0].Length+10].Merge = true;
                    hoja1.Cells["K14"].Value = "Elementos evaluados";
                    hoja1.Cells["K14"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Cells["K14"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells["K14"].Style.Fill.BackgroundColor.SetColor(colFromHexBlack);
                    hoja1.Cells["K14"].Style.Font.Color.SetColor(Color.White);

                    hoja1.Cells[15, tempHeaderLength].Value = headerRow1[7];
                    hoja1.Cells[14, tempHeaderLength].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[14, tempHeaderLength].Style.Fill.BackgroundColor.SetColor(colFromHexBlack);

                    tempHeaderLength = headerLength;
                    hoja1.Cells[15, tempHeaderLength+1].Value = headerRow1[8];
                    hoja1.Cells[14, tempHeaderLength+1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[14, tempHeaderLength+1].Style.Fill.BackgroundColor.SetColor(colFromHexBlack);

                    int tblWidth = headerLength;
                    int tblHeight = lstReporte.Count + 15;
                    tblWidth++;

                    var cellData = new List<object[]>();

                    hoja1.Cells[16, 2].LoadFromArrays(lstReporte.GetRange(1, lstReporte.Count()-1)); // LOAD 

                    for (int i = 2; i <= tblWidth; i++)
                    {
                        hoja1.Column(i).Width = 15;
                        hoja1.Column(i).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    }

                    hoja1.Column(2).Width = 45;
                    hoja1.Column(3).Width = 45;
                    hoja1.Column(4).Width = 45;
                    hoja1.Column(5).Width = 45;
                    hoja1.Column(6).Width = 25;
                    tempHeaderLength = headerLength;
                    hoja1.Column(tempHeaderLength).Width = 45;
                    tempHeaderLength = headerLength;
                    hoja1.Column(tempHeaderLength + 1).Width = 45;

                    hoja1.Row(5).Style.WrapText = true;
                    hoja1.Row(tblHeight + 1).Style.WrapText = true;

                    //hoja1.Cells[tblHeight + 1, 4, tblHeight + 1, 6].Style.;

                    ExcelRange rangeTable = hoja1.Cells[15, 2, tblHeight - 1, tblWidth];

                    ExcelTable tbl1 = hoja1.Tables.Add(rangeTable, "Evaluacion_a_Subcontratistas");

                    tbl1.TableStyle = TableStyles.Medium15;

                    string graficaSubcontratistaBase64 = (string)HttpContext.Current.Session["graficaSubcontratistaDashboard"];
                    string graficaElementosBase64 = (string)HttpContext.Current.Session["graficaElementosDashboard"];
                    string graficaEvaluacionBase64 = (string)HttpContext.Current.Session["graficaEvaluacionDashboard"];

                    byte[] graficaSubcontratistas = Convert.FromBase64String(graficaSubcontratistaBase64.Split(',')[1]);
                    byte[] graficaElementos = Convert.FromBase64String(graficaElementosBase64.Split(',')[1]);
                    byte[] graficaEvaluacion = Convert.FromBase64String(graficaEvaluacionBase64.Split(',')[1]);

                    var logoPMO = Image.FromFile(HttpContext.Current.Server.MapPath("\\Content\\img\\logo\\PMO.jpeg"));
                    var logoCplan = Image.FromFile(HttpContext.Current.Server.MapPath("\\Content\\img\\logo\\logo.jpg"));

                    var imagenGraficaSubC = ByteArrToImage(graficaSubcontratistas);
                    var imagenGraficaEle = ByteArrToImage(graficaElementos);
                    var imagenGraficaEval = ByteArrToImage(graficaEvaluacion);

                    logoPMO = resizeImage(logoPMO, new Size(105, 136));


                    var picture = hoja1.Drawings.AddPicture("dashboardSubC", imagenGraficaSubC);
                    picture.SetPosition(tblHeight + 1, 0, 2, 0);

                    picture = hoja1.Drawings.AddPicture("dashboardEle", imagenGraficaEle);
                    picture.SetPosition(tblHeight + 21, 0, 2, 0);

                    picture = hoja1.Drawings.AddPicture("dashboardEval", imagenGraficaEval);
                    picture.SetPosition(tblHeight + 42, 0, 2, 0);

                    picture = hoja1.Drawings.AddPicture("logoPMO", logoPMO);
                    picture.SetPosition(0, 0, 6, 5);

                    picture = hoja1.Drawings.AddPicture("logoCplan", logoCplan);
                    picture.SetPosition(0, 0, 2, 66);

                    List<byte[]> lista = new List<byte[]>();

                    var bytes = new MemoryStream();

                    //var pieChart = hoja1.Drawings.AddChart("Plantilla", eChartType.Pie);
                    ////Set top left corner to row 1 column 2
                    //pieChart.SetPosition(tblHeight + 3, 2, 2, tblWidth);
                    //pieChart.SetSize(400, 400);
                    //pieChart.Series.Add(hoja1.Cells[tblHeight + 2, 5, tblHeight + 2, 6], hoja1.Cells[tblHeight + 1, 5, tblHeight + 1, 6]);
                    //pieChart.Title.Text = "Plantilla";

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

        #region Detalles
        public Dictionary<string, object> GetCumplimientosElementos(List<string> lstFiltroCC, List<int> lstFiltroSubC, DateTime fechaFiltroInicio, DateTime fechaFiltroFin, int idElemento, int estado_id, int municipio_id, List<int> listaEspecialidades)
        {
            resultado.Clear();
            try
            {
                List<decimal> lstEvidenciasAprobadasPorElemento = new List<decimal>();
                List<string> lstConceptosGrafica = new List<string>();
                
                //var lstContratos = db.tblX_Contrato.Where(e => e.estatus && lstFiltroSubC.Contains(e.subcontratistaID) && lstFiltroCC.Contains(e.cc)).ToList();
                var lstContratos = db.tblX_Contrato.Where(e => e.estatus && lstFiltroSubC.Contains(e.subcontratistaID)).ToList().Where(e =>
                    (lstFiltroCC != null ? lstFiltroCC.Contains(e.cc) : true) &&
                    (estado_id > 0 ? e.estado_id == estado_id : true) &&
                    (municipio_id > 0 ? e.municipio_id == municipio_id : true)
                ).ToList();
                if (lstContratos.Count() > 0)
                {
                    if (listaEspecialidades != null && listaEspecialidades.Count() > 0)
                    {
                        var listaEspecialidadesContratos = db.tblX_ContratotblCOES_Especialidad.Where(x => x.registroActivo && listaEspecialidades.Contains(x.especialidad_id)).Select(x => x.contrato_id).ToList();

                        lstContratos = lstContratos.Where(x => listaEspecialidadesContratos.Contains(x.id)).ToList();
                    }
                    var lstIdsContratos = lstContratos.Select(e => e.id).ToList();

                    var lstPlantillas = db.tblCOES_PlantillatblX_Contrato.Where(e => e.registroActivo && lstIdsContratos.Contains(e.contrato_id)).ToList();
                    var lstIdsPlantillas = db.tblCOES_PlantillatblX_Contrato.Where(e => e.registroActivo && lstIdsContratos.Contains(e.contrato_id)).Select(e => e.plantilla_id).ToList();
                    var lstRelElementos = db.tblCOES_PlantillatblCOES_Elemento.FirstOrDefault(e => e.registroActivo && lstIdsPlantillas.Contains(e.plantilla_id) && e.elemento_id == idElemento);
                    //var lstIdRelPlantillaEle = lstRelElementos.Select(e => e.id).ToList();
                    var lstRequerimientos = db.tblCOES_PlantillatblCOES_ElementotblCOES_Requerimiento.Where(e => e.registroActivo && e.relacionPlantillaElemento_id == lstRelElementos.id).ToList();

                    var lstAsignaciones = db.tblCOES_Asignacion.Where(e => e.registroActivo && lstIdsContratos.Contains(e.contrato_id)).ToList();
                    var lstIdsAsignaciones = db.tblCOES_Asignacion.Where(e => e.registroActivo && lstIdsContratos.Contains(e.contrato_id)).Select(e => e.id).ToList();
                    var lstEvaluaciones = db.tblCOES_Asignacion_Evaluacion.Where(e => e.registroActivo && lstIdsAsignaciones.Contains(e.asignacion_id) && (e.fecha >= fechaFiltroInicio && e.fecha <= fechaFiltroFin)).ToList();
                    var lstEvaluacionesAprobadas = new List<int>();
                    var lstElementosAprobadas = new List<int>();

                    var lstEvalFirmas = db.tblCOES_Firma.Where(e => e.registroActivo && lstIdsContratos.Contains(e.contrato_id) && e.estadoFirma == EstadoFirmaEnum.AUTORIZADO).ToList();
                    var lstElementosFirmados = db.tblCOES_Firma.Where(e => e.registroActivo && lstIdsContratos.Contains(e.contrato_id) && e.estadoFirma == EstadoFirmaEnum.AUTORIZADO).ToList();
                    foreach (var item in lstEvaluaciones)
                    {
                        var lstEvalFirmasAprobadas = lstEvalFirmas.Where(e => e.evaluacion_id == item.id).ToList();

                        if (lstEvalFirmasAprobadas != null && lstEvalFirmasAprobadas.Count() == 2)
                        {
                            lstEvaluacionesAprobadas.Add(item.id);
                        }
                    }

                    var lstEvidencias = db.tblCOES_Evidencia.Where(e => e.registroActivo && lstEvaluacionesAprobadas.Contains(e.evaluacion_id)).ToList();

                    int totalRequerimientos = lstEvaluacionesAprobadas.Count();

                    decimal porcEvidenciasAprobadas = 0;

                    foreach (var item in lstRequerimientos)
                    {
                        var objDescRequerimiento = db.tblCOES_Requerimiento.FirstOrDefault(x => x.id == item.requerimiento_id);
                        var reqAprobado = 0;

                        foreach (var eval in lstEvaluacionesAprobadas)
                        {

                            var objEvidenciasAprobadas = lstEvidencias.FirstOrDefault(e => e.estatus == EstatusEvidenciaEnum.APROBADO && e.evaluacion_id == eval && e.requerimiento_id == item.requerimiento_id);

                            if (objEvidenciasAprobadas != null)
                            {
                                reqAprobado += 1;
                            }

                            int totalReqs = totalRequerimientos;
                            int totalReqsAprobadas = reqAprobado;
                            porcEvidenciasAprobadas = ((decimal)totalReqsAprobadas / (decimal)totalReqs) * 100;

                            lstConceptosGrafica.Add(objDescRequerimiento.descripcion);
                            lstEvidenciasAprobadasPorElemento.Add(porcEvidenciasAprobadas);
                        }
                    }
                }           

                resultado.Add("lstData", lstEvidenciasAprobadasPorElemento);
                resultado.Add("lstConcepts", lstConceptosGrafica);
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, null);

            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        #endregion

        #region Reporte Ejecutivo

        public Dictionary<string, object> GetReporteEjecutivo(List<string> lstfiltroCC, List<int> lstfiltroSubC, DateTime fechaFiltroInicio, DateTime fechaFiltroFin, int estado_id, int municipio_id, List<int> listaEspecialidades)
        {
            resultado.Clear();
            List<ReporteEjecutivoDTO> lstReporteEjecutivo = new List<ReporteEjecutivoDTO>();
            try
            {

                var lstContratos = db.tblX_Contrato.Where(e => e.estatus && lstfiltroSubC.Contains(e.subcontratistaID)).ToList().Where(e =>
                    (lstfiltroCC != null ? lstfiltroCC.Contains(e.cc) : true) &&
                    (estado_id > 0 ? e.estado_id == estado_id : true) &&
                    (municipio_id > 0 ? e.municipio_id == municipio_id : true)
                ).ToList();
                if (listaEspecialidades != null && listaEspecialidades.Count() > 0)
                {
                    var listaEspecialidadesContratos = db.tblX_ContratotblCOES_Especialidad.Where(x => x.registroActivo && listaEspecialidades.Contains(x.especialidad_id)).Select(x => x.contrato_id).ToList();

                    lstContratos = lstContratos.Where(x => listaEspecialidadesContratos.Contains(x.id)).ToList();
                }
                var lstIdsContratos = lstContratos.Select(e => e.id).ToList();


                var lstPlantillas = db.tblCOES_PlantillatblX_Contrato.Where(e => e.registroActivo && lstIdsContratos.Contains(e.contrato_id)).ToList();
                var lstIdsPlantillas = db.tblCOES_PlantillatblX_Contrato.Where(e => e.registroActivo && lstIdsContratos.Contains(e.contrato_id)).Select(e => e.plantilla_id).ToList();
                var lstRelElementos = db.tblCOES_PlantillatblCOES_Elemento.Where(e => e.registroActivo && lstIdsPlantillas.Contains(e.plantilla_id)).ToList();
                var lstIDRelElemento = lstRelElementos.Select(x => x.id).ToList();
                //var lstIdRelPlantillaEle = lstRelElementos.Select(e => e.id).ToList();
                var lstRequerimientos = db.tblCOES_PlantillatblCOES_ElementotblCOES_Requerimiento.Where(e => e.registroActivo && lstIDRelElemento.Contains(e.relacionPlantillaElemento_id)).ToList();

                var lstIdRequerimientos = lstRequerimientos.Select(x => x.id).ToList();
                var lstAsignaciones = db.tblCOES_Asignacion.Where(e => e.registroActivo && lstIdsContratos.Contains(e.contrato_id)).ToList();
                var lstIdsAsignaciones = db.tblCOES_Asignacion.Where(e => e.registroActivo && lstIdsContratos.Contains(e.contrato_id)).Select(e => e.id).ToList();

                var lstEvaluaciones = db.tblCOES_Asignacion_Evaluacion.Where(e => e.registroActivo && lstIdsAsignaciones.Contains(e.asignacion_id) && (e.fecha >= fechaFiltroInicio && e.fecha <= fechaFiltroFin)).ToList();

                //var lstIdsEvaluaciones = lstEvaluaciones.Select(e => e.id).ToList();
                var lstEvaluacionesAprobadas = new List<int>();

                var lstSubcontratistas = db.tblX_SubContratista.Where(x => x.estatus).ToList();

                var lstEvalFirmas = db.tblCOES_Firma.Where(e => e.registroActivo && lstIdsContratos.Contains(e.contrato_id) && e.estadoFirma == EstadoFirmaEnum.AUTORIZADO).ToList();

                foreach (var item in lstEvaluaciones)
                {
                    var lstEvalFirmasAprobadas = lstEvalFirmas.Where(e => e.evaluacion_id == item.id).ToList();

                    if (lstEvalFirmasAprobadas != null && lstEvalFirmasAprobadas.Count() == 2)
                    {
                        lstEvaluacionesAprobadas.Add(item.id); //da toda la lista de evaluaciones aprobadas y autorizadas en base al filtro de fecha
                    }
                }

                List<GraficaCumplimientoEvaluacionesDTO> lstEvaluacionesReporteEvaluacion = new List<GraficaCumplimientoEvaluacionesDTO>();

                foreach (var item in lstAsignaciones)
                {
                    var objContrato = lstContratos.FirstOrDefault(x => x.id == item.contrato_id);
                    var objSubcontratista = lstSubcontratistas.FirstOrDefault(x => x.id == objContrato.subcontratistaID);
                    var objPlantillaContrato = lstPlantillas.FirstOrDefault(x => x.contrato_id == item.contrato_id);
                    //Var Globales
                    int totalEvalGlobalAprob = 0;//Conteo de evaluaciones Aprobados
                    int totalEvalGlobalRequerida = 0;      //Conteo de evaluaciones            
                    decimal porcentajeGlobalEval = 0;

                    //var cumplimeinto Sop
                    int totalCumplimientoSoporte = 0;
                    decimal porcentajeCumplimientoSoporte = 0;

                    //var cumplimeinto Compromiso
                    int totalCumplimientoCompromiso = 0;
                    decimal porcentajeCumplimientoCompromiso = 0;


                    //var cumplimeinto Confiabilidad
                    decimal porcentajeCumplimientoConfiablidad = 0;

                    foreach (var eval in lstEvaluaciones)
                    {
                        var objAsignacion = lstAsignaciones.FirstOrDefault(e => e.id == eval.asignacion_id);
                        objContrato = lstContratos.FirstOrDefault(e => e.id == objAsignacion.contrato_id);
                        var objPlantilla = lstPlantillas.FirstOrDefault(e => e.contrato_id == objAsignacion.contrato_id);
                        var lstDetRelElementos = db.tblCOES_PlantillatblCOES_Elemento.Where(e => e.registroActivo && objPlantilla.plantilla_id == e.plantilla_id).ToList();
                        var lstIdRelEvalElementos = lstDetRelElementos.Select(e => e.id).ToList();

                        var lstEvidencias = db.tblCOES_Evidencia.Where(e => e.registroActivo && eval.id == e.evaluacion_id).ToList();

                        int totalEvidenciasAprobadas = 0;
                        int totalRequerimientosACumplir = 0;

                        int totalEvidenciasTiempo = 0;
                        int totalRequerimientosTiempoACumplir = 0;

                        int totalEvidenciasCompromiso = 0;
                        int totalRequerimientosCompromisoACumplir = 0;

                        //var objElemento = db.tblCOES_Elemento.FirstOrDefault(e => e.id == item.elemento_id);
                        var lstIdsRequerimientos = lstRequerimientos.Where(e => lstIdRelEvalElementos.Contains(e.relacionPlantillaElemento_id)).Select(e => e.requerimiento_id).ToList();

                        var lstEvidenciasTiempo = lstEvidencias.Where(e => lstIdsRequerimientos.Contains(e.requerimiento_id)).ToList();
                        var lstIdsEvidenciasTiempoU = lstEvidencias.Where(e => lstIdsRequerimientos.Contains(e.requerimiento_id)).Select(e => e.requerimiento_id).Distinct().ToList();
                        var lstEvidenciasAprobadas = lstEvidenciasTiempo.Where(e => e.estatus == EstatusEvidenciaEnum.APROBADO).Select(e => e.requerimiento_id).Distinct().ToList();
                        var lstEvidenciasCompromiso = lstEvidenciasTiempo.Where(e => e.tipo == TipoEvidenciaEnum.COMPROMISO).ToList();
                        var lstEvidenciasCompromisoAprobadas = lstEvidenciasCompromiso.Where(e => e.estatus == EstatusEvidenciaEnum.APROBADO).ToList();

                        if (lstEvidenciasCompromiso.Count > 0)
                        {
                            var lstGroupedEvidenciasCompromiso = lstEvidenciasCompromisoAprobadas.GroupBy(e => e.requerimiento_id).Select(e => new { requerimiento = e.Key, lstGroupedReqs = e.OrderBy(ee => ee.fechaCompromiso) }).ToList();

                            int requerimientosCompromisoAprobados = 0;

                            if (lstGroupedEvidenciasCompromiso.Count() > 0)
                            {
                                foreach (var group in lstGroupedEvidenciasCompromiso)
                                {
                                    var objReq = group.lstGroupedReqs.FirstOrDefault();

                                    if (objReq != null && objReq.fechaCompromiso <= objReq.fechaCompromiso)
                                    {
                                        requerimientosCompromisoAprobados++;
                                    }
                                }
                            }

                            totalEvidenciasCompromiso += requerimientosCompromisoAprobados;
                            totalRequerimientosCompromisoACumplir += lstIdsRequerimientos.Count();
                        }

                        if (lstEvidenciasTiempo.Count > 0)
                        {
                            totalEvidenciasTiempo += lstEvidenciasTiempo.Count();
                            totalRequerimientosTiempoACumplir += lstIdsRequerimientos.Count();
                        }

                        var objReporteEvaluacion = lstEvaluacionesReporteEvaluacion.FirstOrDefault(e => e.id_evaluacion == eval.id && e.id_plantilla == objPlantilla.plantilla_id);

                        if (objReporteEvaluacion != null)
                        {

                            if (lstEvaluacionesAprobadas.Contains(eval.id))
                            {
                                //TOTALES POR EVALUACION
                                totalEvidenciasAprobadas += lstEvidenciasAprobadas.Count();
                                totalRequerimientosACumplir += lstIdsRequerimientos.Count();

                                int totalReqs = 0;
                                int totalReqsAprobadas = 0;
                                decimal porcEvidenciasAprobadas = 0;

                                if (lstEvidenciasAprobadas.Count() > 0)
                                {
                                    totalReqs = lstIdsRequerimientos.Count();
                                    totalReqsAprobadas = lstEvidenciasAprobadas.Count();
                                    porcEvidenciasAprobadas = ((decimal)totalReqsAprobadas / (decimal)totalReqs) * 100;
                                }

                                objReporteEvaluacion.totalAutorizadas += totalReqsAprobadas;
                                objReporteEvaluacion.totalRequeridas += totalReqs;
                            }
                        }
                        else
                        {

                            if (lstEvaluacionesAprobadas.Contains(eval.id))
                            {
                                //TOTALES POR EVALUACION
                                totalEvidenciasAprobadas += lstEvidenciasAprobadas.Count();
                                totalRequerimientosACumplir += lstIdsRequerimientos.Count();

                                int totalReqs = 0;
                                int totalReqsAprobadas = 0;
                                decimal porcEvidenciasAprobadas = 0;

                                if (lstEvidenciasAprobadas.Count() > 0)
                                {
                                    totalReqs = lstIdsRequerimientos.Count();
                                    totalReqsAprobadas = lstEvidenciasAprobadas.Count();
                                    porcEvidenciasAprobadas = ((decimal)totalReqsAprobadas / (decimal)totalReqs) * 100;
                                }
                                else
                                {
                                    totalReqs = lstIdsRequerimientos.Count();
                                    porcEvidenciasAprobadas = 0;

                                }

                                lstEvaluacionesReporteEvaluacion.Add(new GraficaCumplimientoEvaluacionesDTO
                                {
                                    id_elemento = eval.id,
                                    id_plantilla = objPlantilla.plantilla_id,
                                    id_contrato = objContrato.id,
                                    id_evaluacion = eval.id,
                                    totalAutorizadas = totalReqsAprobadas,
                                    totalRequeridas = totalReqs,
                                    contratoCompromisoAutorizadas = totalEvidenciasCompromiso,
                                    contratoCompromismoRequeridas = totalRequerimientosCompromisoACumplir,
                                    contratoTiempoAutorizadas = totalEvidenciasTiempo,
                                    contratoTiempoRequeridas = totalRequerimientosTiempoACumplir,
                                    descElemento = "",
                                });
                            }
                            else
                            {
                                lstEvaluacionesReporteEvaluacion.Add(new GraficaCumplimientoEvaluacionesDTO
                                {
                                    id_elemento = eval.id,
                                    id_plantilla = objPlantilla.plantilla_id,
                                    id_contrato = objContrato.id,
                                    id_evaluacion = eval.id,
                                    totalAutorizadas = 0,
                                    totalRequeridas = 0,
                                    contratoCompromisoAutorizadas = 0,
                                    contratoCompromismoRequeridas = 0,
                                    contratoTiempoAutorizadas = 0,
                                    contratoTiempoRequeridas = 0,
                                    porcAutorizado = 0M,
                                    descElemento = "",
                                });
                            }
                        }


                    }
                    //if (totalEvalGlobalRequerida > 0)
                    //{
                    //    porcentajeGlobalEval = ((decimal)totalEvalGlobalAprob / (decimal)totalEvalGlobalRequerida) * 100;
                    //    porcentajeCumplimientoSoporte = ((decimal)totalCumplimientoSoporte / (decimal)totalEvalGlobalRequerida) * 100;
                    //    porcentajeCumplimientoCompromiso = ((decimal)totalCumplimientoCompromiso / (decimal)totalEvalGlobalRequerida) * 100;

                    //    decimal totalSuma = porcentajeGlobalEval + porcentajeCumplimientoSoporte + porcentajeCumplimientoCompromiso;

                    //    porcentajeCumplimientoConfiablidad = totalSuma / 3;
                    //}
                }

                foreach (var item in lstAsignaciones)
                {
                    var objContrato = lstContratos.FirstOrDefault(e => e.id == item.contrato_id);
                    var objSubcontratista = lstSubcontratistas.FirstOrDefault(x => x.id == objContrato.subcontratistaID);
                    var lstEvaluacionesContrato = lstEvaluacionesReporteEvaluacion.Where(e => e.id_contrato == objContrato.id);

                    var lstGroupedEvalReporte = lstEvaluacionesContrato.GroupBy(e => e.id_evaluacion).Select(e => new { evaluacion = e.Key, lstEvaluaciones = e }).ToList();

                    int diffReqsContrato = 0;
                    decimal porcReqsContrato = 0;

                    decimal porcReqsCompromisoContrato = 0;
                    decimal porcReqsTiempoContrato = 0;

                    var totalRequerimientos = 0;
                    var totalAprobadas = 0;

                    var totalCompromisoRequerimientos = 0;
                    var totalCompromisoAprobadas = 0;

                    var totalTiempoRequerimientos = 0;
                    var totalTiempoAprobadas = 0;

                    foreach (var group in lstGroupedEvalReporte)
                    {
                        totalRequerimientos += group.lstEvaluaciones.Sum(e => e.totalRequeridas);
                        totalAprobadas += group.lstEvaluaciones.Sum(e => e.totalAutorizadas);

                        totalCompromisoRequerimientos += group.lstEvaluaciones.Sum(e => e.contratoCompromismoRequeridas);
                        totalCompromisoAprobadas += group.lstEvaluaciones.Sum(e => e.contratoCompromisoAutorizadas);

                        totalTiempoRequerimientos += group.lstEvaluaciones.Sum(e => e.contratoTiempoRequeridas);
                        totalTiempoAprobadas += group.lstEvaluaciones.Sum(e => e.contratoTiempoAutorizadas);
                    }

                    if (totalRequerimientos > 0)
                    {
                        diffReqsContrato = totalRequerimientos - totalAprobadas;
                        porcReqsContrato = ((decimal)totalAprobadas / (decimal)totalRequerimientos) * 100;
                    }

                    if (totalCompromisoRequerimientos > 0 && totalCompromisoRequerimientos > 0)
                    {
                        porcReqsCompromisoContrato = ((decimal)totalCompromisoAprobadas / (decimal)totalCompromisoRequerimientos) * 100;
                    }
                    else
                    {
                        porcReqsCompromisoContrato = 100;
                    }

                    if (totalTiempoRequerimientos > 0)
                    {
                        porcReqsTiempoContrato = ((decimal)totalTiempoAprobadas / (decimal)totalTiempoRequerimientos) * 100;
                    }

                    decimal porcentajeCumplimientoConfiablidad = (porcReqsContrato + porcReqsCompromisoContrato + porcReqsTiempoContrato) / 3;


                    //tablaReporte.Add(new ReporteDashboardEvaluacionDTO
                    //{
                    //    cc = objContrato.cc,
                    //    contrato = objContrato.numeroContrato,
                    //    nombreSucC = subC.nombre,
                    //    periodoEval = objContrato.fechaSuscripcion.Value.ToString("dd/MM/yyyy") + " -> " + objContrato.fechaVigencia.Value.ToString("dd/MM/yyyy"),
                    //    calificacionGlobal = porcReqsContrato,
                    //    realizadas = totalRequerimientos,
                    //    aprobadas = totalAprobadas,
                    //    noAprobadas = diffReqsContrato,
                    //    porcAprobadas = porcReqsContrato,
                    //    lstElementosEvaluados = lstEvaluacionesReporteElementos.Where(e => e.id_contrato == item.contrato_id).OrderBy(e => e.descElemento).ToList(),
                    //    cumplimientoTiempo = porcReqsTiempoContrato,
                    //    cumplimientoCompromiso = porcReqsCompromisoContrato,
                    //})

                    lstReporteEjecutivo.Add(new ReporteEjecutivoDTO
                    {
                        cc = objContrato.cc,
                        numeroContrato = objContrato.numeroContrato,
                        subContratista = objSubcontratista.nombre,
                        confiabilidad = Math.Round(porcentajeCumplimientoConfiablidad, 2),
                        calificacionEval = Math.Round(porcReqsContrato, 2),
                        cumplimientoSoporte = Math.Round(porcReqsTiempoContrato, 2),
                        cumplimientoCompromisos = Math.Round(porcReqsCompromisoContrato, 2)
                    });
                }
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, lstReporteEjecutivo);

            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Tuple<MemoryStream, string> DescargarExcelPersonalActivo()
        {
            try
            {
                var listaEmpleadosRelacionCursos = HttpContext.Current.Session["ListaReporteEjecutivo"] as List<Dictionary<string, object>>;
                var columnasCursosSesion = HttpContext.Current.Session["columnasReporteEjecutivo"] as List<Tuple<string, string>>;

                if (listaEmpleadosRelacionCursos == null || columnasCursosSesion == null)
                {
                    return null;
                }

                // Se agregan las columnas.
                var columnasCursos = new List<Tuple<string, string>>();
                columnasCursos.AddRange(new List<Tuple<string, string>>
                {
                    Tuple.Create("claveEmpleado","Clave"),
                    Tuple.Create("curp", "CURP"),
                    Tuple.Create("nombre","Nombre"),
                    Tuple.Create("puesto","Puesto"),
                    Tuple.Create("departamentoEmpleado","Área"),
                });
                columnasCursos.AddRange(columnasCursosSesion);

                var headersExcel = columnasCursos.Select(x => x.Item2).ToArray();

                using (ExcelPackage excel = new ExcelPackage())
                {
                    var hoja = excel.Workbook.Worksheets.Add("Personal Activo");

                    List<string[]> headerRow = new List<string[]>() { headersExcel };

                    string headerRange = "A1:" + ExcelUtilities.GetExcelColumnName(headersExcel.Count()) + "1";

                    hoja.Cells[headerRange].LoadFromArrays(headerRow);

                    var cellData = new List<object[]>();

                    foreach (var empleadoCursos in listaEmpleadosRelacionCursos)
                    {
                        var cellDataEmployee = new object[empleadoCursos.Count];

                        int counter = 0;

                        foreach (var dict in empleadoCursos)
                        {
                            //cellDataEmployee[counter++] = ObtenerStringEstatusCurso(dict.Value.ToString());
                        }

                        cellData.Add(cellDataEmployee);
                    }

                    hoja.Cells[2, 1].LoadFromArrays(cellData);

                    excel.Compression = OfficeOpenXml.CompressionLevel.BestSpeed;

                    List<byte[]> lista = new List<byte[]>();

                    var bytes = new MemoryStream();

                    using (var exportData = new MemoryStream())
                    {
                        excel.SaveAs(exportData);
                        bytes = exportData;
                    }

                    return Tuple.Create(bytes, "Personal Activo.xlsx");
                }
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "DescargarExcelPersonalActivo", e, AccionEnum.DESCARGAR, 0, 0);
                return null;
            }
        }

        #endregion

        public Image ByteArrToImage(byte[] array)
        {
            // Convert base 64 string to byte[]
            using (var ms = new MemoryStream(array))
            {
                return Image.FromStream(ms);
            }
        }

        public static Image resizeImage(Image imgToResize, Size size)
        {
            return (Image)(new Bitmap(imgToResize, size));
        }

        #endregion

        public Dictionary<string, object> FillComboEstados()
        {
            try
            {
                var data = _context.tblRH_EK_Estados.Where(x => x.clave_pais == 1).ToList().Select(x => new ComboDTO
                {
                    Value = x.id.ToString(),
                    Text = x.descripcion
                }).ToList();

                resultado.Add(ITEMS, data);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "FillComboEstados", e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> FillComboMunicipios(int estado_id)
        {
            try
            {
                var data = _context.tblRH_EK_Cuidades.Where(x => x.clave_pais == 1).ToList().Where(x => estado_id > 0 ? x.clave_estado == estado_id : true).Select(x => new ComboDTO
                {
                    Value = x.id.ToString(),
                    Text = x.descripcion
                }).ToList();

                resultado.Add(ITEMS, data);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "FillComboMunicipios", e, AccionEnum.CONSULTA, 0, estado_id);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }
        #endregion
    }
}