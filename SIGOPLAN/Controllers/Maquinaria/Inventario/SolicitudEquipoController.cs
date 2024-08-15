using Core.DAO.Maquinaria.Captura;
using Core.DAO.Maquinaria.Catalogos;
using Core.DAO.Maquinaria.Inventario;
using Core.DAO.Principal.Alertas;
using Core.DAO.Principal.Usuarios;
using Core.DTO;
using Core.DTO.Maquinaria.Catalogos;
using Core.DTO.Maquinaria.Inventario;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Maquinaria.Inventario;
using Core.Entity.Principal.Alertas;
using Core.Enum.Principal.Alertas;
using Data.Factory.Maquinaria.Captura;
using Data.Factory.Maquinaria.Catalogos;
using Data.Factory.Maquinaria.Inventario;
using Data.Factory.Principal.Alertas;
using Data.Factory.Principal.Archivos;
using Data.Factory.Principal.Usuarios;
using Infrastructure.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Controllers.Maquinaria.Inventario
{
    public class SolicitudEquipoController : BaseController
    {
        public struct stAutoriza
        {
            public int usuarioID { get; set; }
            public string correo { get; set; }
            public string estatus { get; set; }
        }
        #region Factory
        IAlertasDAO alertasFS;
        ICapturaHorometroDAO capturaHorometroFS;
        IAutorizacionSolicitudReemplazoDAO authSolicitudReemplazoFS;
        ISolicitudEquipoReemplazoDetDAO solicitudEquipoReemplazoDetFS;
        ICentroCostosDAO centroCostosFS;
        ISolicitudEquipoDAO solicitudEquipoFS;
        ISolicitudEquipoDetDAO solicitudEquipoDetFS;
        IAutorizacionSolicitudesDAO authSolicitudesFS;
        IUsuarioDAO usuarioFS;
        IAsignacionEquiposDAO asignacionEquiposFS;
        IMaquinaDAO maquinaFS;
        ISolicitudEquipoReemplazo solicitudReemplazoFS;
        IenvioCorreosDAO envioCorreosFS;
        ArchivoFactoryServices ArchivoFS;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            alertasFS = new AlertaFactoryServices().getAlertaService();
            capturaHorometroFS = new CapturaHorometroFactoryServices().getCapturaHorometroServices();
            authSolicitudReemplazoFS = new AutorizacionSolicitudReemplazoFactoryServices().getAutorizacionSolicitudReemplazoFactoryServices();
            solicitudEquipoReemplazoDetFS = new SolicitudEquipoReemplazoDetFactoryServices().getSolicitudEquipoReemplazoDetFactoryServices();
            solicitudReemplazoFS = new SolicitudReemplazoFactoryServices().getSolicitudReemplazoFactoryServices();
            maquinaFS = new MaquinaFactoryServices().getMaquinaServices();
            centroCostosFS = new CentroCostosFactoryServices().getCentroCostosService();
            usuarioFS = new UsuarioFactoryServices().getUsuarioService();
            authSolicitudesFS = new AutorizacionSolicitudesFactoryServices().getAutorizacionSolicitudesServices();
            solicitudEquipoFS = new SolicitudEquipoFactoryServices().getSolicitudEquipoServices();
            solicitudEquipoDetFS = new SolicitudEquipoDetFactoryServices().getSolicitudEquipoDetServices();
            asignacionEquiposFS = new AsignacionEquiposFactoryServices().getAsignacionEquiposFactoryServices();
            envioCorreosFS = new EnvioCorreosFactoryServices().getEnvioCorreosFactoryServices();
            ArchivoFS = new ArchivoFactoryServices();

            base.OnActionExecuting(filterContext);
        }
        #endregion      
        // GET: SolicitudMaquinaria
        #region Llamado de Vistas
        public ActionResult ElaboracionSolicitudesEquipo()
        {
            Session["rptCadenaAutorizacion"] = null;
            return View();
        }

        public ActionResult Autorizaciones()
        {
            return View();
        }

        public ActionResult AsignacionDeEquipo()
        {
            return View();
        }

        public ActionResult SustitucionMaquinaria()
        {
            return View();
        }

        public ActionResult AutorizacionesReemplazo()
        {
            return View();
        }

        public ActionResult VisorSolicitudes()
        {
            return View();
        }
        public ActionResult AsignacionMaquinaria()
        {

            if (base.getAction("Asignar"))
            {
                ViewBag.PermisoVistas = true;
            }
            else
            {
                ViewBag.PermisoVistas = false;
            }

            return View();
        }
        public ActionResult AsignacionAutorizantes()
        {
            return View();
        }
        #endregion
     
        #region Metodos Visor Solicitudes
        /// <summary>
        /// Este método se encarga de obtener las solicitudes generadas en el sistema.
        /// Pertenece a VisorSolicitudes;
        /// </summary>
        /// <param name="CentroCostos"></param>
        /// <returns></returns>
        public ActionResult GetlistaSolicitudes(List<string> CentroCostos)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var res = solicitudEquipoFS.GetListSolicitudesCC(CentroCostos);

                var DataSend = res.Select(x => new
                {
     
                    Folio = x.folio,
                    CentroCostosName = centroCostosFS.getNombreCCFix(x.SolicitudEquipo.CC),
                    UsuarioElabora = x.usuarioElaboro != 0 ? getNombreUsuario(x.usuarioElaboro) : "",
                    FechaCreacion = x.SolicitudEquipo.fechaElaboracion.ToShortDateString(),
                    Estatus = GetEstatus(x),
                    btnDetalle = "<button class='btn btn-xs' title='Ver Detalle' onclick='GetDetalle(" + x.solicitudEquipoID + ")'><span class='glyphicon glyphicon-eye-open'></span></button>" +
                                 "<button class='btn btn-xs btn-primary' title='Ver Solicitud' onclick='LoadReporte(" + x.solicitudEquipoID + ",\"" + x.SolicitudEquipo.CC + "\")'><span class='glyphicon glyphicon-eye-open'></span></button>"
                });

                result.Add("DataSend", DataSend);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Este método se encarga de obtener las solicitudes generadas en el sistema mas a detalle por cada solicitud
        /// Pertenece a VisorSolicitudes;
        /// </summary>
        /// <param name="CentroCostos"></param>
        /// <returns></returns>
        public ActionResult GetDetalleSolicitudes(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var asignacion = asignacionEquiposFS.getAsignacionesByID(id);

                var data = asignacion.Select(x => new
                {
                    Economico = maquinaFS.EconomicoNotNull(x.noEconomicoID).noEconomico,
                    Equipo = maquinaFS.EconomicoNotNull(x.noEconomicoID).descripcion,
                    FechaPromesa = x.FechaPromesa.ToShortDateString(),
                    btnCalidadEnvio = "<button class='btn btn-xs' title='Control de Calidad Envio' onclick='GetReporteCalidad(" + x.id + "," + 1 + ")'><span class='glyphicon glyphicon-eye-open'></span></button>",
                    btnControlEnvio = "<button class='btn btn-xs' title='Control de Envio' onclick='GetReporteControl(" + x.id + "," + 1 + "," + x.solicitudEquipoID + ")'><span class='glyphicon glyphicon-eye-open'></span></button>",
                    btnCalidadRecepcion = "<button class='btn btn-xs' title='Control de Calidad Recepcion' onclick='GetReporteCalidad(" + x.id + "," + 2 + ")'><span class='glyphicon glyphicon-eye-open'></span></button>",
                    btnControlRecepcion = "<button class='btn btn-xs' title='Control de Control Recepcion' onclick='GetReporteControl(" + x.id + "," + 2 + "," + x.solicitudEquipoID + ")'><span class='glyphicon glyphicon-eye-open'></span></button>",
                });

                result.Add("data", data);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Metodo privado para saber en que esta se encuentra una solicitud
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private string GetEstatus(tblM_AutorizacionSolicitudes x)
        {
            string Estatus = "";


            if (!getDetEstatus(x.cadenaFirmaGerenteObra, x.firmaGerenteObra))
            {
                return "Pendiente Gerente";
            }
            else
            {
                if (!getDetEstatus(x.cadenaFirmaGerenteDirector, x.firmaGerenteDirector))
                {
                    return "Pendiente Director Area";
                }
                else
                {
                    if (!getDetEstatus(x.cadenaFirmaDirector, x.firmaDirectorDivision))
                    {
                        return "Pendiente Director División";
                    }
                    else
                    {
                        if (!getDetEstatus(x.cadenaFirmaDireccion, x.firmaAltaDireccion))
                        {
                            return "Pendiente Alta Direccion";
                        }
                        else
                        {
                            if (x.firmaAltaDireccion)
                            {
                                return "Solicitud Apropada";
                            }
                            else
                            {
                                return "Solicitud Rechazada";
                            }
                        }
                    }
                }


            }

            throw new NotImplementedException();
        }

        private bool getDetEstatus(string Cadena, bool firma)
        {
            bool res = false;
            if (Cadena != null)
            {
                res = true;
            }

            return res;
        }

        private string getNombreUsuario(int p)
        {

            string nombreUsuario = "";
            if (p != 0)
            {
                var nUsuario = usuarioFS.ListUsersById(p).FirstOrDefault();

                if (nUsuario != null)
                {
                    nombreUsuario = nUsuario.nombre + " " + nUsuario.apellidoPaterno;
                }
            }

            return nombreUsuario;
        }
        #endregion

        public ActionResult cboEconomicosByGrupo(int idGrupo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<tblM_CatMaquina> raw = maquinaFS.FillCboEconomicos(idGrupo);
                var select = StringCboEconomicos(raw);
                result.Add("stringCboEconomico", select);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private string StringCboEconomicos(List<tblM_CatMaquina> raw)
        {
            var select = "<select class='form-control EconomicoDat'>";
            select += "<option>Seleccione:</option>";
            select += "<option value='0'>RENTA</option>";
            select += "<option value='9999'>COMPRA</option>";
            foreach (var item in raw)
            {

                var Descripciones = "";
                string centro_costos = "";
                var Asignaciones = asignacionEquiposFS.getEconomicoAsignado(item.id);
                var horometros = capturaHorometroFS.GetHorometroFinal(item.noEconomico);
                try
                {
                    centro_costos = centroCostosFS.getNombreCC(Convert.ToInt32(item.centro_costos));

                }
                catch (Exception)
                {
                    centro_costos = "";
                }

                if (Asignaciones != null)
                {
                    var solicitudesDetalle = solicitudEquipoDetFS.DetSolicitud(Asignaciones.SolicitudDetalleId);

                    if (solicitudesDetalle != null)
                    {

                        Descripciones = "Horometro Actual: " + horometros + "'; Fecha Fin: " + solicitudesDetalle.fechaFin.ToShortDateString() + "; Centro Costos: " + centro_costos;
                    }
                }
                else
                {
                    Descripciones = "Horometro Actual: " + horometros + "; Fecha Fin: No Asignado ; Centro Costos: " + centro_costos;
                }
                select += "<option value='" + item.id + "' title='" + Descripciones + "'>" + item.noEconomico + "(" + item.centro_costos + ")</option>";
            }

            select += "</select>";

            return select;
        }

        public ActionResult getDataFromTableSolicitudElaboracion(List<SolicitudEquipoDTO> array, AutorizadoresIDDTO obj, List<SolicitudEquipoJustificacionDTO> arrayJustificacion)
        {

            var result = new Dictionary<string, object>();
            try
            {

                Session["rptSolicitudEquipo"] = "";

                if (array != null)
                {
                    Session["rptAutorizadores"] = obj;
                    Session["rptSolicitudEquipo"] = array;
                    Session["rptSolicitudEquipoJustificacion"] = arrayJustificacion;
                    result.Add(SUCCESS, true);
                }
                else
                {
                    result.Add(SUCCESS, false);
                }
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveOrUpdate(List<SolicitudEquipoDTO> array, tblM_SolicitudEquipo obj, tblM_AutorizacionSolicitudes autoriza, int Actualizacion, List<tblM_SM_Justificacion> arrayJustificacion)
        {
            var result = new Dictionary<string, object>();
            try
            {
                tblM_AutorizacionSolicitudes getDataAutorizadores = new tblM_AutorizacionSolicitudes();

                //array = (List<SolicitudEquipoDTO>)Session["rptSolicitudEquipo"];
                
                if (array.Count > 0 || array != null)
                {

                    obj.Estatus = false;
                    if (Actualizacion == 0)
                    {
                        obj.Estatus = false;
                    }
                    string folio = "";
                    if (obj.id == 0)
                    {
                        folio = obj.CC.ToString() + "-" + solicitudEquipoFS.GetFolio(obj.CC).PadLeft(6, '0'); folio = obj.CC.ToString() + "-" + solicitudEquipoFS.GetFolio(obj.CC).PadLeft(6, '0');
                        obj.folio = folio;

                    }
                    else
                    {
                        getDataAutorizadores = authSolicitudesFS.getAutorizadores(obj.id);
                        autoriza.id = getDataAutorizadores.id;
                    }
                    obj.condicionInicial = array.FirstOrDefault().condicionInicial;
                    obj.condicionActual = array.FirstOrDefault().condicionActual;
                    obj.justificacion = array.FirstOrDefault().justificacion;
                    obj.link = "";
                    obj.cantidad = array.Count;
                    solicitudEquipoFS.Guardar(obj);
                    arrayJustificacion.ForEach(x=>x.solicitudID = obj.id);
                    solicitudEquipoFS.GuardarJustificaciones(obj.id,arrayJustificacion);

                    var data = array.Select(x => new tblM_SolicitudEquipoDet
                    {
                        id = x.id,
                        folio = obj.folio,
                        solicitudEquipoID = obj.id,
                        tipoMaquinariaID = x.TipoId,
                        grupoMaquinariaID = x.Grupoid,
                        Comentario = x.Descripcion,
                        modeloEquipoID = x.Modeloid,
                        fechaInicio = Convert.ToDateTime(x.pFechaInicio),
                        fechaFin = Convert.ToDateTime(x.pFechaFin),
                        horas = x.pHoras,
                        prioridad = getPrioridadDescripcion(x.pTipoPrioridad),
                        tipoUtilizacion = x.tipoUtilizacion

                    }).ToList();

                    if (autoriza.id == 0)
                    {
                        autoriza.folio = obj.folio;
                        autoriza.solicitudEquipoID = obj.id;
                        autoriza.firmaElaboro = true;

                    }
                    else
                    {
                        autoriza.id = getDataAutorizadores.id;
                        autoriza.folio = getDataAutorizadores.folio;
                        autoriza.solicitudEquipoID = getDataAutorizadores.solicitudEquipoID;
                        autoriza.firmaElaboro = true;
                    }
                    int idUsuarioEnvia = autoriza.id;
                    int idUsuarioRecibe = autoriza.gerenteObra;

                    DateTime fecha = DateTime.Now;
                    string f = fecha.ToString("ddMMyyyy") + fecha.Hour + "" + fecha.Minute;
                    autoriza.cadenaFirmaElabora = autoriza.solicitudEquipoID + f + "" + base.getUsuario().id + "A";
                    autoriza.FechaDireccion = fecha;
                    autoriza.FechaDirectorDivision = fecha;
                    autoriza.FechaElabora = fecha;
                    autoriza.FechaGerenteDirector = fecha;
                    autoriza.FechaGerenteObra = fecha;


                    solicitudEquipoDetFS.Guardar(data);
                    authSolicitudesFS.Guardar(autoriza);
                    var AletaVisto = alertasFS.getAlertasByUsuario(idUsuarioRecibe).FirstOrDefault(x => x.objID.Equals(obj.id));



                    if (AletaVisto != null)
                    {
                        AletaVisto.userEnviaID = getUsuario().id;
                        alertasFS.updateAlerta(AletaVisto);
                    }
                    else
                    {

                        tblP_Alerta objAlerta = new tblP_Alerta();

                        objAlerta.msj = "Solicitud Pendiente de Autorizar " + obj.folio;
                        objAlerta.sistemaID = 1;
                        objAlerta.tipoAlerta = (int)AlertasEnum.REDIRECCION;
                        objAlerta.url = "/SolicitudEquipo/Autorizaciones/?Solicitud=" + obj.id;
                        objAlerta.objID = obj.id;
                        objAlerta.userEnviaID = idUsuarioEnvia;
                        objAlerta.userRecibeID = idUsuarioRecibe;
                        alertasFS.saveAlerta(objAlerta);

                    }

                    string DesCentroCostos = centroCostosFS.getNombreCCFix(obj.CC);
                    MandarCorreo(obj.id, "Se Generó nueva solicitud de maquinaria y equipo con el folio " + obj.folio + "(" + DesCentroCostos + ")", false, "", obj.CC);
                    result.Add("folio", folio);
                    result.Add("solicitudID", obj.id);
                    result.Add(MESSAGE, GlobalUtils.getMensaje(Actualizacion));
                    result.Add(SUCCESS, true);
                }

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SubirEvidenciaSolicitud()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var solicitudID = int.Parse(Request.Form["solicitudID"]);
                HttpPostedFileBase file = Request.Files["fuEvidencia"];

                string FileName = "";
                string ruta = "";
                bool pathExist = false;
                DateTime fecha = DateTime.Now;
                string f = fecha.ToString("ddMMyyyy") + fecha.Hour + "" + fecha.Minute;
                FileName = file.FileName;

                ruta = ArchivoFS.getArchivo().getUrlDelServidor(1016) + f + FileName;
                pathExist = GuardarDocumentos(file, ruta);

                if (pathExist)
                {
                    solicitudEquipoFS.GuardarSolicitudEvidencia(solicitudID, ruta);
                }

                result.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getFileRuta(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var Archivo = solicitudEquipoFS.loadSolicitudById(id).link;
                var esSuccess = Archivo.Length > 0;
                if (esSuccess)
                {
                    var ruta = Archivo.Replace("C:\\", "\\\\REPOSITORIO\\");
                    result.Add("ruta", ruta);
                }
                result.Add(SUCCESS, esSuccess);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public FileResult getFileDownload()
        {
            try
            {
                int id = Convert.ToInt32(Request.QueryString["id"]);
                var Archivo = solicitudEquipoFS.loadSolicitudById(id).link;
                var ruta = Archivo.Replace("C:\\", "\\\\REPOSITORIO\\");
                return File(ruta, "multipart/form-data", Archivo);
            }
            catch (Exception)
            {

                return null;
            }

        }

        private bool GuardarDocumentos(HttpPostedFileBase archivo, string ruta)
        {
            bool result = false;
            byte[] data;
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
            ruta = ruta.Replace("C:\\", "\\REPOSITORIO\\");
            System.IO.File.WriteAllBytes(ruta, data);
            result = System.IO.File.Exists(ruta);
            return result;
        }

        private void MandarCorreoAsigNormal(int solicitudID, string folio, bool needAdjunto, string comentario, string cc, string noEconomico, string origen, string fecha)
        {
            needAdjunto = true;

            var Usuario = authSolicitudesFS.getAutorizadores(solicitudID);
     
            var AsuntoCorreo = @"<style>
                                    table {
                                        font-family: arial, sans-serif;
                                        border-collapse: collapse;
                                        width: 100%;
                                    }

                                    body {
                                        color: #000000; 
                                        font-family: arial, helvetica, sans-serif; 
                                        font-size: 16px; 
                                        font-style: normal; 
                                        font-variant-ligatures: normal; 
                                        font-variant-caps: normal; 
                                        font-weight: 400; 
                                        letter-spacing: normal; 
                                        orphans: 2; 
                                        text-align: start; 
                                        text-indent: 0px; 
                                        text-transform: none; 
                                        white-space: normal; 
                                        widows: 2; 
                                        word-spacing: 0px; 
                                        -webkit-text-stroke-width: 0px; 
                                        text-decoration-style: initial; 
                                        text-decoration-color: initial; 
                                        background-color: #ffffff;
                                    }
                                    th{
                                        background-color: #da6a1a;
                                    }
                                    tD{
                                        text-align: center;
                                        font-size: 14px;
                                    }
                                    </style>
                                    <p>Buen d&iacute;a</p>" +
                                    "<p>Se asignó el siguiente equipo correspondiente a la solicitud " + folio + " al &aacute;rea de cuenta " + centroCostosFS.getNombreCCFix(cc) + "</p>" +
                                    "<table>" +
                                    "<thead>" +
                                    "<tr>" +
                                    "<th>Económico</th>" +
                                    "<th>Origen</th>" +
                                    "<th>Fecha</th>" +
                                    "</tr>" +
                                    "</thead>" +
                                    "<tbody>";

            AsuntoCorreo += "<tr>" +
                "<td>" + noEconomico + "</td>" +
                "<td>" + centroCostosFS.getNombreCCFix(origen) + "</td>" +
                "<td>" + fecha + "</td>" +
                "</tr>";

            AsuntoCorreo +=
                "</tbody>" +
                "</table>" +
                "<p>Mensaje autogenerado por el sistema SIGOPLAN</p>";


            List<string> Corres = new List<string>();

            //var correoElvia = usuarioFS.ListUsersById(1072).FirstOrDefault().correo;
            //Corres.Add(correoElvia);

            var correoAna = usuarioFS.ListUsersById(1197).FirstOrDefault().correo;
            Corres.Add(correoAna);

            var correoElia = usuarioFS.ListUsersById(1123).FirstOrDefault().correo;
            Corres.Add(correoElia);
            //var correoLuara = usuarioFS.ListUsersById(8016).FirstOrDefault().correo;
            //Corres.Add(correoLuara);

            if (!needAdjunto)
            {
                GlobalUtils.sendEmail("Asignación de equipo - Notificación cobranza " + folio, AsuntoCorreo, Corres);
            }
            else
            {
                var downloadPDF = (List<Byte[]>)Session["downloadPDF"];
                GlobalUtils.sendEmailAdjuntoInMemorySolicitudes("Asignación de equipo - Notificación cobranza " + folio, AsuntoCorreo, Corres, downloadPDF);
            }
        }

        private void MandarCorreoAsigArranObra(int solicitudID, string folio, bool needAdjunto, string comentario, string cc, List<tblM_AsignacionEquipos> equiposAsignados)
        {
            needAdjunto = true;

            var Usuario = authSolicitudesFS.getAutorizadores(solicitudID);


            var AsuntoCorreo = @"   <style>
                                    table {
                                        font-family: arial, sans-serif;
                                        border-collapse: collapse;
                                        width: 100%;
                                    }

                                    body {
                                        color: #000000; 
                                        font-family: arial, helvetica, sans-serif; 
                                        font-size: 16px; 
                                        font-style: normal; 
                                        font-variant-ligatures: normal; 
                                        font-variant-caps: normal; 
                                        font-weight: 400; 
                                        letter-spacing: normal; 
                                        orphans: 2; 
                                        text-align: start; 
                                        text-indent: 0px; 
                                        text-transform: none; 
                                        white-space: normal; 
                                        widows: 2; 
                                        word-spacing: 0px; 
                                        -webkit-text-stroke-width: 0px; 
                                        text-decoration-style: initial; 
                                        text-decoration-color: initial; 
                                        background-color: #ffffff;
                                    }
                                    th{
                                        background-color: #da6a1a;
                                    }
                                    tD{
                                        text-align: center;
                                        font-size: 14px;
                                    }
                                    </style>
                                    <p>Buen d&iacute;a</p>" +
                                    "<p>Se asignaron los siguientes equipos correspondientes a la solicitud " + folio + " al &aacute;rea de cuenta " + centroCostosFS.getNombreCCFix(equiposAsignados.FirstOrDefault().cc) + "</p>" +
                                    "<table>" +
                                    "<thead>" +
                                    "<tr>" +
                                    "<th>Económico</th>" +
                                    "<th>Origen</th>" +
                                    "<th>Fecha asignación</th>" +
                                    "</tr>" +
                                    "</thead>" +
                                    "<tbody>";

            foreach (var maquina in equiposAsignados)
            {
                AsuntoCorreo += "<tr>" +
                    "<td>" + maquina.Economico + "</td>" +
                    "<td>" + centroCostosFS.getNombreCCFix(maquina.CCOrigen) + "</td>" +
                    "<td>" + maquina.fechaAsignacion.ToString("dd/MM/yyyy") + "</td>" +
                    "</tr>";
            }
            AsuntoCorreo +=
                "</tbody>" +
                "</table>" +
                "<p>Mensaje autogenerado por el sistema SIGOPLAN</p>";


            List<string> Corres = new List<string>();

            var correoElvia = usuarioFS.ListUsersById(1072).FirstOrDefault().correo;
            Corres.Add(correoElvia);

            var correoElia = usuarioFS.ListUsersById(1123).FirstOrDefault().correo;
            Corres.Add(correoElia);
            //var correoLaura = usuarioFS.ListUsersById(8016).FirstOrDefault().correo;
            //Corres.Add(correoLaura);

            if (!needAdjunto)
            {
                GlobalUtils.sendEmail("Asignación de equipos - Notificación cobranza " + folio, AsuntoCorreo, Corres);
            }
            else
            {
                var downloadPDF = (List<Byte[]>)Session["downloadPDF"];
                GlobalUtils.sendEmailAdjuntoInMemorySolicitudes("Asignación de equipos - Notificación cobranza " + folio, AsuntoCorreo, Corres, downloadPDF);
            }
        }

        private void MandarCorreo(int solicitudID, string folio, bool needAdjunto, string comentario, string cc)
        {
            needAdjunto = true;

            var Auth = authSolicitudesFS.getAutorizadores(solicitudID);
            var Elaboro = usuarioFS.ListUsersById(Auth.usuarioElaboro).FirstOrDefault().correo;
            var Gerente = usuarioFS.ListUsersById(Auth.gerenteObra).FirstOrDefault().correo;
            var DirectorArea = usuarioFS.ListUsersById(Auth.GerenteDirector).FirstOrDefault().correo;
            var Director = usuarioFS.ListUsersById(Auth.directorDivision).FirstOrDefault().correo;
            var Servicios = usuarioFS.ListUsersById(Auth.directorServicios).FirstOrDefault().correo;
            var AltaDireccion = usuarioFS.ListUsersById(Auth.altaDireccion).FirstOrDefault().correo;
            //var Asigna = usuarioFS.ListUsersById(3314).FirstOrDefault().correo;

            var listaAutoriza = new List<stAutoriza>();
            listaAutoriza.Add(new stAutoriza { usuarioID = Auth.usuarioElaboro, correo = Elaboro, estatus = getEstatus(Auth.firmaElaboro, Auth.cadenaFirmaElabora) });
            listaAutoriza.Add(new stAutoriza { usuarioID = Auth.gerenteObra, correo = Gerente, estatus = getEstatus(Auth.firmaGerenteObra, Auth.cadenaFirmaGerenteObra) });
            listaAutoriza.Add(new stAutoriza { usuarioID = Auth.GerenteDirector, correo = DirectorArea, estatus = getEstatus(Auth.firmaGerenteDirector, Auth.cadenaFirmaGerenteDirector) });
            listaAutoriza.Add(new stAutoriza { usuarioID = Auth.directorDivision, correo = Director, estatus = getEstatus(Auth.firmaDirectorDivision, Auth.cadenaFirmaDirector) });
            listaAutoriza.Add(new stAutoriza { usuarioID = Auth.directorServicios, correo = Servicios, estatus = getEstatus(Auth.firmaServicios, Auth.cadenaFirmaServicios) });
            listaAutoriza.Add(new stAutoriza { usuarioID = Auth.altaDireccion, correo = AltaDireccion, estatus = getEstatus(Auth.firmaAltaDireccion, Auth.cadenaFirmaDireccion) });

            var Autorizando = listaAutoriza.FirstOrDefault(x => x.estatus.Contains("PENDIENTE"));
            var proceso = (listaAutoriza.Any(x => x.estatus.Contains("PENDIENTE")) ? Autorizando.usuarioID : 0 );
            var AsuntoCorreo = @"<html>    <head>
                                    <style>
                                    table {
                                        font-family: arial, sans-serif;
                                        border-collapse: collapse;
                                        width: 100%;
                                    }

                                    td, th {
                                        border: 1px solid #dddddd;
                                        text-align: left;
                                        padding: 8px;
                                    }

                                    tr:nth-child(even) {
                                        background-color: #dddddd;
                                    }
                                    </style>
                                </head>
                                <body>
                                <p>Buen día </p>" +
                                folio + @"</p>
                                <table>
<thead>
                                  <tr>
                                    <th>Nombre Autorizador </th>
                                    <th>Descripción Puesto</th>
                                    <th>Autorizó</th>
                                  </tr></thead>
<tbody>
                                  <tr>
                                    <td>" + getUsuarioNombre(Auth.usuarioElaboro) + "</td>" +
                                    "<td>Administrador de Maquinaria</td>" +
                                     getEstatusNuevo(Auth.firmaElaboro, Auth.cadenaFirmaElabora, proceso, Auth.usuarioElaboro) +
                                  "</tr>" +
                                                                   " <tr>" +
                                    "<td>" + getUsuarioNombre(Auth.gerenteObra) + "</td>" +
                                    "<td>Gerente de Obra</td>" +
                                   getEstatusNuevo(Auth.firmaGerenteObra, Auth.cadenaFirmaGerenteObra, proceso, Auth.gerenteObra) +
                                  "</tr>" +
                                                                    "<tr>" +
                                    "<td>" + getUsuarioNombre(Auth.GerenteDirector) + "</td>" +
                                    "<td>Director de Area</td>" +
                                    getEstatusNuevo(Auth.firmaGerenteDirector, Auth.cadenaFirmaGerenteDirector, proceso, Auth.GerenteDirector) +
                                  "</tr>" +
                                  "<tr>" +
                                    "<td>" + getUsuarioNombre(Auth.directorDivision) + "</td>" +
                                    "<td>Director de División</td>" +
                                     getEstatusNuevo(Auth.firmaDirectorDivision, Auth.cadenaFirmaDirector, proceso, Auth.directorDivision) +
                                  "</tr>" +
                                   "<tr>" +
                                    "<td>" + getUsuarioNombre(Auth.directorServicios) + "</td>" +
                                    "<td>Director de División</td>" +
                                     getEstatusNuevo(Auth.firmaServicios, Auth.cadenaFirmaServicios, proceso, Auth.directorServicios) +
                                  "</tr>" +
                                  "<tr>" +
                                    "<td>" + getUsuarioNombre(Auth.altaDireccion) + "</td>" +
                                    "<td>Alta Dirección</td>" +
                                        getEstatusNuevo(Auth.firmaAltaDireccion, Auth.cadenaFirmaDireccion, proceso, Auth.altaDireccion) +
                                  "</tr></tbody>" +
                                 "</table>" +
                                 (!string.IsNullOrEmpty(comentario) ? "Motivo de Rechazo de solicitud : " + comentario : "") +

                                "<p>Mensaje autogenerado por el sistema SIGOPLAN</p>" +
                                "</body>" +
                               " </html>";


            List<string> Correo = new List<string>();


            Correo.Add(Elaboro);
            Correo.Add(Gerente);
            Correo.Add(DirectorArea);
            Correo.Add(Director);
            Correo.Add(Servicios);
            Correo.Add(AltaDireccion);
            //Correo.Add(Asigna);

            var correoGerenteMaquinari = usuarioFS.correoPerfil(8, cc).Distinct();
            var correosAuxiliares = usuarioFS.correoPerfil(9, cc).Distinct();

            Correo.AddRange(correoGerenteMaquinari);
            Correo.AddRange(correosAuxiliares);
            var Elia = usuarioFS.ListUsersById(1123).FirstOrDefault().correo;

            Correo.Add(Elia);
            //var Laura = usuarioFS.ListUsersById(8016).FirstOrDefault().correo;
            //Correo.Add(Laura);

            var excepcionesCorreo = usuarioFS.getPermisosAutorizaCorreo(1);

            List<int> excepcionesCorreoIDs = new List<int>();

            if (excepcionesCorreo.Count > 0)
            {
                excepcionesCorreoIDs.AddRange(excepcionesCorreo.Select(x => x.usuarioID));
            }

            foreach (var ex in excepcionesCorreoIDs)
            {
                var correo = usuarioFS.ListUsersById(ex).FirstOrDefault().correo;
                var existe = listaAutoriza.Any(x=>x.usuarioID == ex);
                if (ex != Autorizando.usuarioID && existe) {
                    Correo.Remove(correo);
                }

  
            }
            //List<string> Correos = new List<string>();
            //Correos.Add("angel.devora@construplan.com.mx");
            if (!needAdjunto)
            {
                GlobalUtils.sendEmail("Notificación Autorizacion Solicitudes " + folio, AsuntoCorreo, Correo.Distinct().ToList());
            }
            else
            {
                var downloadPDF = (List<Byte[]>)Session["downloadPDF"];
                GlobalUtils.sendEmailAdjuntoInMemorySolicitudes("Notificación Autorizacion Solicitudes " + folio, AsuntoCorreo, Correo.Distinct().ToList(), downloadPDF);
            }


        }

        private string getUsuarioNombre(int idUsuario)
        {
            var usuario = usuarioFS.ListUsersById(idUsuario).FirstOrDefault();

            return usuario.nombre + " " + usuario.apellidoPaterno + " " + usuario.apellidoMaterno;

        }

        private string getEstatus(bool firma, string cadena)
        {
            cadena = string.IsNullOrEmpty(cadena) ? "" : cadena;
            if (firma)
            {
                return "<td style='background-color: #82E0AA;'>AUTORIZADO</td>";
            }
            else if (!firma && !string.IsNullOrEmpty(cadena))
            {
                if (cadena.Contains("R") && !string.IsNullOrEmpty(cadena))
                {
                    return "<td style='background-color: #EC7063;'>RECHAZADO</td>";
                }
                else 
                {
                    return "<td style='background-color: #FAE5D3;'>PENDIENTE</td>";
                }
            }

            return "<td style='background-color: #FAE5D3;'>NO FIRMÓ</td>";
        }
        private string getEstatusNuevo(bool firma, string cadena, int autorizando, int autoriza)
        {
            cadena = cadena ?? "";
            if (autorizando == autoriza)
            {
                return "<td style='background-color: yellow;'>AUTORIZANDO</td>";
            }
            else if (firma)
            {
                return "<td style='background-color: #82E0AA;'>AUTORIZADO</td>";
            }
            else if (cadena.Contains("R"))
            {
                return "<td style='background-color: #EC7063;'>RECHAZADO</td>";
            }
            else
            {
                return "<td style='background-color: #FAE5D3;'>PENDIENTE</td>";
            }
        }

        public ActionResult SaveOrUpdateArranqueObra(List<SolicitudEquipoDTO> array, tblM_SolicitudEquipo obj, tblM_AutorizacionSolicitudes autoriza, int Actualizacion, int CentroCostos)
        {
            var result = new Dictionary<string, object>();
            try
            {
                tblM_AutorizacionSolicitudes getDataAutorizadores = new tblM_AutorizacionSolicitudes();

                array = (List<SolicitudEquipoDTO>)Session["rptSolicitudEquipo"];
                autoriza.usuarioElaboro = base.getUsuario().id;
                if (array.Count > 0 || array != null)
                {

                    obj.Estatus = false;
                    if (Actualizacion == 0)
                    {
                        obj.Estatus = false;
                    }
                    string folio = "";
                    if (obj.id == 0)
                    {
                        folio = obj.CC.ToString() + "-" + solicitudEquipoFS.GetFolio(obj.CC).PadLeft(6, '0'); folio = obj.CC.ToString() + "-" + solicitudEquipoFS.GetFolio(obj.CC).PadLeft(6, '0');
                        obj.folio = folio;

                    }
                    else
                    {
                        getDataAutorizadores = authSolicitudesFS.getAutorizadores(obj.id);
                        autoriza.id = getDataAutorizadores.id;
                    }


                    obj.cantidad = array.Count;
                    solicitudEquipoFS.Guardar(obj);

                    var data = array.Select(x => new tblM_SolicitudEquipoDet
                    {
                        id = x.id,
                        folio = obj.folio,
                        solicitudEquipoID = obj.id,
                        tipoMaquinariaID = x.TipoId,
                        grupoMaquinariaID = x.Grupoid,
                        Comentario = x.Descripcion,
                        modeloEquipoID = x.Modeloid,
                        fechaInicio = Convert.ToDateTime(x.pFechaInicio),
                        fechaFin = Convert.ToDateTime(x.pFechaFin),
                        horas = x.pHoras,
                        prioridad = getPrioridadDescripcion(x.pTipoPrioridad),
                        tipoUtilizacion = x.tipoUtilizacion,
                        estatus = true

                    }).ToList();



                    if (autoriza.id == 0)
                    {
                        autoriza.folio = obj.folio;
                        autoriza.solicitudEquipoID = obj.id;
                        autoriza.firmaElaboro = true;

                    }
                    else
                    {

                        autoriza.id = getDataAutorizadores.id;
                        autoriza.folio = getDataAutorizadores.folio;
                        autoriza.solicitudEquipoID = getDataAutorizadores.solicitudEquipoID;
                        autoriza.firmaElaboro = true;
                    }



                    DateTime fecha = DateTime.Now;
                    string f = fecha.ToString("ddMMyyyy") + fecha.Hour + "" + fecha.Minute;
                    autoriza.cadenaFirmaElabora = autoriza.solicitudEquipoID + f + "" + base.getUsuario().id + "A";

                    autoriza.FechaDireccion = fecha;
                    autoriza.FechaDirectorDivision = fecha;
                    autoriza.FechaElabora = fecha;
                    autoriza.FechaGerenteDirector = fecha;
                    autoriza.FechaGerenteObra = fecha;


                    solicitudEquipoDetFS.Guardar(data);
                    authSolicitudesFS.Guardar(autoriza);


                    List<tblM_AsignacionEquipos> lista = new List<tblM_AsignacionEquipos>();
                    List<tblM_SolicitudEquipoDet> listEquiposDet = new List<tblM_SolicitudEquipoDet>();
                    List<tblM_AsignacionEquipos> listaAsignados = new List<tblM_AsignacionEquipos>();
                    int c = 0;
                    foreach (var item in array)
                    {
                        int idAsiganacion = 0;
                        var tempData = data[c];
                        tblM_AsignacionEquipos dato = new tblM_AsignacionEquipos
                        {
                            id = idAsiganacion,
                            cc = obj.CC,
                            CCOrigen = item.idNoEconomico != 0 ? maquinaFS.GetMaquinaByID(item.idNoEconomico).First().centro_costos : "997",
                            estatus = 1,
                            fechaAsignacion = DateTime.Now,
                            fechaFin = Convert.ToDateTime(item.pFechaFin),
                            fechaInicio = Convert.ToDateTime(item.pFechaInicio),
                            folio = folio,
                            Horas = Convert.ToInt32(item.pHoras),
                            noEconomicoID = item.idNoEconomico,
                            solicitudEquipoID = obj.id,
                            SolicitudDetalleId = tempData.id,
                            FechaPromesa = DateTime.Now,
                            Economico = item.Economico
                        };
                        c++;
                        lista.Add(dato);
                    }
                    foreach (var maquina in lista)
                    {
                        if (maquina.noEconomicoID != 0) { listaAsignados.Add(maquina); }
                    }
                    int idUsuarioEnvia = autoriza.id;
                    int idUsuarioRecibe = autoriza.gerenteObra;
                    asignacionEquiposFS.SaveOrUpdate(lista);




                    var AletaVisto = alertasFS.getAlertasByUsuario(idUsuarioEnvia).FirstOrDefault(x => x.objID.Equals(obj.id));

                    if (AletaVisto != null)
                    {
                        alertasFS.updateAlerta(AletaVisto);
                    }
                    else
                    {

                        tblP_Alerta objAlerta = new tblP_Alerta();

                        objAlerta.msj = "Solicitud Pendiente de Autorizar " + obj.folio;
                        objAlerta.sistemaID = 1;
                        objAlerta.tipoAlerta = (int)AlertasEnum.REDIRECCION;
                        objAlerta.url = "/SolicitudEquipo/Autorizaciones/?Solicitud=" + obj.id;
                        objAlerta.objID = obj.id;
                        objAlerta.userEnviaID = idUsuarioEnvia;
                        objAlerta.userRecibeID = idUsuarioRecibe;

                        alertasFS.saveAlerta(objAlerta);

                    }
                    MandarCorreo(obj.id, obj.folio, true, "", obj.CC);

                    if (listaAsignados.Count > 0) { MandarCorreoAsigArranObra(obj.id, obj.folio, true, "", obj.CC, listaAsignados); }

                    result.Add("folio", "Se genero nueva solicitud de maquinaria y equipo con el folio " + folio);
                    result.Add(MESSAGE, GlobalUtils.getMensaje(Actualizacion));
                    result.Add(SUCCESS, true);
                }

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public int getPrioridadDescripcion(string obj)
        {
            switch (obj)
            {

                case "NORMAL":
                    return 2;
                case "URGENTE":
                    return 1;
                case "PROGRAMADA":
                    return 0;
                default:
                    break;
            }
            return 0;
        }

        public ActionResult GetCentroCostosAutorizacion(int idSolicitud)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var solicitudObj = solicitudEquipoFS.listaSolicitudEquipo(idSolicitud);

                if (solicitudObj != null)
                {

                    result.Add("CentroCostos", solicitudObj.CC);
                }

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDataSolicitudesPendientes(int filtro)
        {
            var result = new Dictionary<string, object>();
            //try
            //{
                var idusuario = getUsuario().id;

                var listResult = solicitudEquipoFS.getListPendientes(idusuario, filtro).ToList();

                var lista = new List<SolicitudesPendientesDTO>();
                foreach (var item in listResult)
                {

                    var obj = new SolicitudesPendientesDTO();

                    var usuario = usuarioFS.ListUsersById(item.usuarioID).FirstOrDefault();
                    string nombre = "";
                    if (usuario != null)
                    {
                        nombre = usuario.nombre;
                    }
                    obj.id = item.id;
                    obj.Folio = item.folio;
                    obj.UsuarioSolicitud = nombre;
                    obj.CCName = centroCostosFS.getNombreCCFix(item.CC);
                    obj.Fecha = (item.fechaElaboracion).ToString("dd/MM/yyyy");
                    obj.cc = item.CC;
                    obj.Comentario = solicitudEquipoFS.obtenerComentarioSolicitud(item.id);
                    lista.Add(obj);
                }
                result.Add("Autorizadas", lista);

                result.Add(SUCCESS, true);
            //}
            //catch (Exception e)
            //{
            //    result.Add(MESSAGE, e.Message);
            //    result.Add(SUCCESS, false);

            //}
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //Autorizacion Guardar
        public ActionResult SaveOrUpdateAutorizacion(int obj, string Autoriza)
        {
            var result = new Dictionary<string, object>();
            try
            {
                bool ActualizarHoras = false;
                DateTime fecha = DateTime.Now;
                string f = fecha.ToString("ddMMyyyy") + fecha.Hour + "" + fecha.Minute;
                int idUsuarioRecibe = 0;
                int idUsuarioEnvia = 0;
                var AutorizacionSolicitudes = authSolicitudesFS.GetAutorizacionSolicitudes(obj);
                bool Alerta = true;
                int usuarioActual = getUsuario().id;

                if (AutorizacionSolicitudes.usuarioElaboro == usuarioActual)
                {

                    AutorizacionSolicitudes.firmaElaboro = true;
                    AutorizacionSolicitudes.cadenaFirmaElabora = AutorizacionSolicitudes.solicitudEquipoID + f + "" + base.getUsuario().id + "A";
                    AutorizacionSolicitudes.FechaElabora = fecha;
                    idUsuarioEnvia = AutorizacionSolicitudes.usuarioElaboro;
                    idUsuarioRecibe = AutorizacionSolicitudes.usuarioElaboro;

                }

                if (AutorizacionSolicitudes.gerenteObra == usuarioActual)
                {
                    AutorizacionSolicitudes.firmaGerenteObra = true;
                    AutorizacionSolicitudes.cadenaFirmaGerenteObra = AutorizacionSolicitudes.solicitudEquipoID + f + "" + base.getUsuario().id + "A";
                    idUsuarioEnvia = AutorizacionSolicitudes.gerenteObra;
                    idUsuarioRecibe = AutorizacionSolicitudes.GerenteDirector;
                    AutorizacionSolicitudes.FechaGerenteObra = fecha;
                }

                if (AutorizacionSolicitudes.GerenteDirector == usuarioActual)
                {
                    AutorizacionSolicitudes.firmaGerenteDirector = true;
                    AutorizacionSolicitudes.cadenaFirmaGerenteDirector = AutorizacionSolicitudes.solicitudEquipoID + f + "" + base.getUsuario().id + "A";
                    idUsuarioRecibe = AutorizacionSolicitudes.directorDivision;
                    idUsuarioEnvia = AutorizacionSolicitudes.GerenteDirector;
                    AutorizacionSolicitudes.FechaGerenteDirector = fecha;
                }
                if (AutorizacionSolicitudes.directorDivision == usuarioActual)
                {
                    AutorizacionSolicitudes.firmaDirectorDivision = true;
                    AutorizacionSolicitudes.cadenaFirmaDirector = AutorizacionSolicitudes.solicitudEquipoID + f + "" + base.getUsuario().id + "A";
                    idUsuarioRecibe = AutorizacionSolicitudes.directorServicios;
                    idUsuarioEnvia = AutorizacionSolicitudes.directorDivision;
                    AutorizacionSolicitudes.FechaDirectorDivision = fecha;
                }
                if (AutorizacionSolicitudes.directorServicios == usuarioActual)
                {
                    AutorizacionSolicitudes.firmaServicios = true;
                    AutorizacionSolicitudes.cadenaFirmaServicios = AutorizacionSolicitudes.solicitudEquipoID + f + "" + base.getUsuario().id + "A";
                    idUsuarioRecibe = AutorizacionSolicitudes.altaDireccion;
                    idUsuarioEnvia = AutorizacionSolicitudes.directorServicios;
                    AutorizacionSolicitudes.FechaServicios = fecha;
                }
                if (AutorizacionSolicitudes.altaDireccion == usuarioActual)
                {
                    AutorizacionSolicitudes.firmaAltaDireccion = true;
                    AutorizacionSolicitudes.cadenaFirmaDireccion = AutorizacionSolicitudes.solicitudEquipoID + f + "" + base.getUsuario().id + "A";
                    idUsuarioEnvia = AutorizacionSolicitudes.altaDireccion;
                    AutorizacionSolicitudes.FechaDireccion = fecha;
                    ActualizarHoras = true;
                    Alerta = false;
                }

                authSolicitudesFS.Guardar(AutorizacionSolicitudes);


                var AletaVisto = alertasFS.getAlertasByUsuario(idUsuarioEnvia).FirstOrDefault(x => x.objID.Equals(AutorizacionSolicitudes.solicitudEquipoID));

                if (AletaVisto != null)
                {
                    AletaVisto.visto = true;
                    alertasFS.updateAlerta(AletaVisto);
                }
                if (Alerta == true)
                {
                    tblP_Alerta objAlerta = new tblP_Alerta();

                    objAlerta.msj = "Solicitud Pendiente de Autorizar " + AutorizacionSolicitudes.folio;
                    objAlerta.sistemaID = 1;
                    objAlerta.tipoAlerta = (int)AlertasEnum.REDIRECCION;
                    objAlerta.url = "/SolicitudEquipo/Autorizaciones/?Solicitud=" + AutorizacionSolicitudes.solicitudEquipoID;
                    objAlerta.objID = AutorizacionSolicitudes.solicitudEquipoID;
                    objAlerta.userEnviaID = idUsuarioEnvia;
                    objAlerta.userRecibeID = idUsuarioRecibe;

                    alertasFS.saveAlerta(objAlerta);

                    var usuarioCorreo = usuarioFS.ListUsersById(idUsuarioRecibe).FirstOrDefault();
                    var usuarioEnvia = usuarioFS.ListUsersById(idUsuarioEnvia).FirstOrDefault();

                    List<string> CorreoEnviar = new List<string>();
                    var res = solicitudEquipoFS.loadSolicitudById(AutorizacionSolicitudes.solicitudEquipoID);
                    MandarCorreo(AutorizacionSolicitudes.solicitudEquipoID, "Se Autorizo el folio " + AutorizacionSolicitudes.folio, false, "", res.CC);
                }
                else
                {
                    var res = solicitudEquipoFS.loadSolicitudById(AutorizacionSolicitudes.solicitudEquipoID);
                    MandarCorreo(AutorizacionSolicitudes.solicitudEquipoID, "Se Autorizo el folio " + AutorizacionSolicitudes.folio, true, "", res.CC);
                }

                if (ActualizarHoras)
                {
                    var res = solicitudEquipoFS.loadSolicitudById(AutorizacionSolicitudes.solicitudEquipoID);
                    var ListaDetalleSolicitud = solicitudEquipoDetFS.listaDetalleSolicitud(res.id);
                    List<tblM_SolicitudEquipoDet> SolicitudEquipoDet = new List<tblM_SolicitudEquipoDet>();

                    TimeSpan restaTiempo = DateTime.Now - res.fechaElaboracion;
                    int diasAumento = restaTiempo.Days;
                    foreach (var item in ListaDetalleSolicitud)
                    {
                        item.fechaInicio = item.fechaInicio.AddDays(diasAumento);
                        item.fechaFin = item.fechaFin.AddDays(diasAumento);
                        SolicitudEquipoDet.Add(item);
                    }

                    solicitudEquipoDetFS.Guardar(SolicitudEquipoDet);

                }

                if (AutorizacionSolicitudes.firmaAltaDireccion == true && AutorizacionSolicitudes.firmaServicios == true && AutorizacionSolicitudes.firmaDirectorDivision == true && AutorizacionSolicitudes.firmaElaboro == true && AutorizacionSolicitudes.firmaGerenteDirector == true && AutorizacionSolicitudes.firmaGerenteObra == true) 
                {
                    solicitudEquipoFS.insertEnvioGestor(AutorizacionSolicitudes.solicitudEquipoID);
                }


                result.Add("idSolicitud", AutorizacionSolicitudes.solicitudEquipoID);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult SaveOrUpdateSolicitudReemplazo(int obj, string Autoriza)
        {
            var result = new Dictionary<string, object>();
            try
            {
                bool ActualizarHoras = false;
                DateTime fecha = DateTime.Now;
                tblM_SolicitudReemplazoEquipo existe = new tblM_SolicitudReemplazoEquipo();


                string f = fecha.ToString("ddMMyyyy") + fecha.Hour + "" + fecha.Minute;
                var AutorizacionSolicitudReemplazo = authSolicitudReemplazoFS.GetAutorizacionReemplazoByID(obj);
                int idUsuarioRecibe = 0;
                int idUsuarioEnvia = 0;
                existe = AutorizacionSolicitudReemplazo.SolicitudReemplazoEquipo;
                bool Alerta = false;
                switch (Autoriza)
                {
                    case "Elaboro":
                        AutorizacionSolicitudReemplazo.AutorizaElabora = true;
                        AutorizacionSolicitudReemplazo.CadenaElabora = AutorizacionSolicitudReemplazo.solicitudReemplazoEquipoID + f + "" + base.getUsuario().id + "A";
                        idUsuarioRecibe = AutorizacionSolicitudReemplazo.idAutorizaGerente;
                        idUsuarioEnvia = AutorizacionSolicitudReemplazo.idAutorizaElabora;
                        break;
                    case "gerenteObra":
                        AutorizacionSolicitudReemplazo.AutorizaGerente = true;
                        AutorizacionSolicitudReemplazo.CadenaGerente = AutorizacionSolicitudReemplazo.solicitudReemplazoEquipoID + f + "" + base.getUsuario().id + "A";
                        idUsuarioRecibe = AutorizacionSolicitudReemplazo.idAutorizaAsigna;
                        idUsuarioEnvia = AutorizacionSolicitudReemplazo.idAutorizaGerente;

                        Alerta = true;
                        break;
                    case "asigna":
                        AutorizacionSolicitudReemplazo.AutorizaAsigna = true;
                        AutorizacionSolicitudReemplazo.CadenaAsigna = AutorizacionSolicitudReemplazo.solicitudReemplazoEquipoID + f + "" + base.getUsuario().id + "A";

                        idUsuarioRecibe = AutorizacionSolicitudReemplazo.idAutorizaGerente;
                        idUsuarioEnvia = AutorizacionSolicitudReemplazo.idAutorizaAsigna; //AutorizacionSolicitudReemplazo.idAutorizaGerente;
                        Alerta = false;
                        break;
                    default:
                        break;
                }

                authSolicitudReemplazoFS.Guardar(AutorizacionSolicitudReemplazo);

                var AletaVisto = alertasFS.getAlertasByUsuario(idUsuarioEnvia).FirstOrDefault(x => x.objID.Equals(AutorizacionSolicitudReemplazo.id));

                if (AletaVisto != null)
                {
                    AletaVisto.visto = true;
                    alertasFS.updateAlerta(AletaVisto);
                }
                if (Alerta == true)
                {
                    tblP_Alerta objAlerta = new tblP_Alerta();
                    var Folio = AutorizacionSolicitudReemplazo.SolicitudReemplazoEquipo.folio;
                    objAlerta.msj = "Solicitud Pendiente de Autorizar " + Folio;
                    objAlerta.sistemaID = 1;
                    objAlerta.tipoAlerta = (int)AlertasEnum.REDIRECCION;
                    objAlerta.url = "/SolicitudEquipo/AutorizacionesReemplazo/?Solicitud=" + AutorizacionSolicitudReemplazo.id;
                    objAlerta.objID = AutorizacionSolicitudReemplazo.id;
                    objAlerta.userEnviaID = idUsuarioEnvia;
                    objAlerta.userRecibeID = idUsuarioRecibe;

                    alertasFS.saveAlerta(objAlerta);



                }
                var usuarioCorreo = usuarioFS.ListUsersById(idUsuarioRecibe).FirstOrDefault();
                var usuarioEnvia = usuarioFS.ListUsersById(idUsuarioEnvia).FirstOrDefault();

                mandarcorreoreemplazo(AutorizacionSolicitudReemplazo.idAutorizaGerente, existe, AutorizacionSolicitudReemplazo);

                result.Add("idSolicitud", AutorizacionSolicitudReemplazo.solicitudReemplazoEquipoID);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult RechazoSolicitud(int obj, string Autoriza, string comentario)
        {
            var result = new Dictionary<string, object>();
            try
            {
                if (comentario == null || comentario.Trim().Length < 10)
                {
                    result.Add(MESSAGE, "No se rechazó la solicitud. El comentario viene vacío.");
                    result.Add(SUCCESS, false);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                DateTime fecha = DateTime.Now;
                string f = fecha.ToString("ddmmyyyy") + fecha.Hour + "" + fecha.Minute;
                int idUsuarioRechaza = 0;
                var AutorizacionSolicitudes = authSolicitudesFS.GetAutorizacionSolicitudes(obj);

                switch (Autoriza)
                {
                    case "Elaboro":

                        AutorizacionSolicitudes.cadenaFirmaElabora = AutorizacionSolicitudes.solicitudEquipoID + f + "" + base.getUsuario().id + "R";
                        AutorizacionSolicitudes.cadenaFirmaGerenteObra = " ";
                        AutorizacionSolicitudes.cadenaFirmaGerenteDirector = " ";
                        AutorizacionSolicitudes.cadenaFirmaDirector = " ";
                        AutorizacionSolicitudes.cadenaFirmaDireccion = " ";
                        idUsuarioRechaza = AutorizacionSolicitudes.usuarioElaboro;
                        AutorizacionSolicitudes.FechaElabora = fecha;
                        AutorizacionSolicitudes.firmaElaboro = false;
                        break;
                    case "gerenteObra":
                        AutorizacionSolicitudes.cadenaFirmaGerenteObra = AutorizacionSolicitudes.solicitudEquipoID + f + "" + base.getUsuario().id + "R";
                        AutorizacionSolicitudes.cadenaFirmaGerenteDirector = " ";
                        AutorizacionSolicitudes.cadenaFirmaDirector = " ";
                        AutorizacionSolicitudes.cadenaFirmaDireccion = " ";
                        idUsuarioRechaza = AutorizacionSolicitudes.gerenteObra;
                        AutorizacionSolicitudes.FechaGerenteObra = fecha;
                        AutorizacionSolicitudes.firmaGerenteObra = false;
                        break;
                    case "GerenteDirector":
                        AutorizacionSolicitudes.cadenaFirmaGerenteDirector = AutorizacionSolicitudes.solicitudEquipoID + f + "" + base.getUsuario().id + "R";
                        AutorizacionSolicitudes.cadenaFirmaDirector = " ";
                        AutorizacionSolicitudes.cadenaFirmaDireccion = " ";
                        idUsuarioRechaza = AutorizacionSolicitudes.GerenteDirector;
                        AutorizacionSolicitudes.FechaGerenteDirector = fecha;
                        AutorizacionSolicitudes.firmaGerenteDirector = false;
                        break;
                    case "directorDivision":
                        AutorizacionSolicitudes.cadenaFirmaDirector = AutorizacionSolicitudes.solicitudEquipoID + f + "" + base.getUsuario().id + "R";
                        AutorizacionSolicitudes.cadenaFirmaServicios = " ";
                        AutorizacionSolicitudes.cadenaFirmaDireccion = " ";
                        idUsuarioRechaza = AutorizacionSolicitudes.directorDivision;
                        AutorizacionSolicitudes.FechaDirectorDivision = fecha;
                        AutorizacionSolicitudes.firmaDirectorDivision = false;
                        break;
                    case "directorServicios":
                        AutorizacionSolicitudes.cadenaFirmaServicios = AutorizacionSolicitudes.solicitudEquipoID + f + "" + base.getUsuario().id + "R";
                        AutorizacionSolicitudes.cadenaFirmaDireccion = " ";
                        idUsuarioRechaza = AutorizacionSolicitudes.directorServicios;
                        AutorizacionSolicitudes.FechaServicios = fecha;
                        AutorizacionSolicitudes.firmaServicios = false;
                        break;
                    case "altaDireccion":
                        AutorizacionSolicitudes.cadenaFirmaDireccion = AutorizacionSolicitudes.solicitudEquipoID + f + "" + base.getUsuario().id + "R";
                        idUsuarioRechaza = AutorizacionSolicitudes.altaDireccion;
                        AutorizacionSolicitudes.FechaDireccion = fecha;
                        AutorizacionSolicitudes.firmaAltaDireccion = false;
                        break;
                    default:
                        break;
                }            
                AutorizacionSolicitudes.observaciones = comentario.Trim();
                authSolicitudesFS.Guardar(AutorizacionSolicitudes);
                var AletaVisto = alertasFS.getAlertasByUsuario(idUsuarioRechaza).FirstOrDefault(x => x.objID.Equals(AutorizacionSolicitudes.solicitudEquipoID));
                if (AletaVisto != null)
                {
                    AletaVisto.visto = true;
                    alertasFS.updateAlerta(AletaVisto);
                }
                var res = solicitudEquipoFS.loadSolicitudById(AutorizacionSolicitudes.solicitudEquipoID);
                MandarCorreo(AutorizacionSolicitudes.solicitudEquipoID, AutorizacionSolicitudes.folio, false, comentario, res.CC);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RechazoSolicitudReemplazo(int obj, string Autoriza, string comentario)
        {
            var result = new Dictionary<string, object>();
            try
            {
                if (comentario == null || comentario.Trim().Length < 10)
                {
                    result.Add(MESSAGE, "No se rechazó la solicitud. El comentario viene vacío.");
                    result.Add(SUCCESS, false);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                DateTime fecha = DateTime.Now;
                string f = fecha.ToString("ddmmyyyy") + fecha.Hour + "" + fecha.Minute;
                int idUsuarioRechaza = getUsuario().id;
                string TipoAutoriza = "";

                var AutorizacionSolicitudReemplazo = authSolicitudReemplazoFS.GetAutorizacionReemplazoByID(obj);

                if (AutorizacionSolicitudReemplazo != null)
                {
                    if (AutorizacionSolicitudReemplazo.idAutorizaElabora == idUsuarioRechaza)
                    {
                        AutorizacionSolicitudReemplazo.AutorizaElabora = false;
                        AutorizacionSolicitudReemplazo.CadenaElabora = AutorizacionSolicitudReemplazo.solicitudReemplazoEquipoID + f + "" + base.getUsuario().id + "R";

                    }
                    if (AutorizacionSolicitudReemplazo.idAutorizaAsigna == idUsuarioRechaza)
                    {
                        AutorizacionSolicitudReemplazo.AutorizaAsigna = false;
                        AutorizacionSolicitudReemplazo.CadenaAsigna = AutorizacionSolicitudReemplazo.solicitudReemplazoEquipoID + f + "" + base.getUsuario().id + "R";
                    }
                    if (AutorizacionSolicitudReemplazo.idAutorizaGerente == idUsuarioRechaza)
                    {
                        AutorizacionSolicitudReemplazo.AutorizaGerente = false;
                        AutorizacionSolicitudReemplazo.CadenaGerente = AutorizacionSolicitudReemplazo.solicitudReemplazoEquipoID + f + "" + base.getUsuario().id + "R";
                    }
                }

                AutorizacionSolicitudReemplazo.Comentarios = comentario.Trim();

                authSolicitudReemplazoFS.Guardar(AutorizacionSolicitudReemplazo);

                var AletaVisto = alertasFS.getAlertasByUsuario(idUsuarioRechaza).FirstOrDefault(x => x.objID.Equals(AutorizacionSolicitudReemplazo.id));

                if (AletaVisto != null)
                {
                    AletaVisto.visto = true;
                    alertasFS.updateAlerta(AletaVisto);
                }
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDataAutorizacion(int obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var raw = authSolicitudesFS.getAutorizadores(obj);

                int usuarioActual = base.getUsuario().id;
                string AutorizadorActual = "";
                bool flagEntro = true;

                var bandera = multiPuestos(usuarioActual, raw);

                if (raw.usuarioElaboro.Equals(usuarioActual))
                {
                    AutorizadorActual = "Elaboro";
                }
                if (raw.gerenteObra.Equals(usuarioActual) && flagEntro)
                {

                    if (!raw.firmaGerenteObra)
                    {
                        AutorizadorActual = "gerenteObra";
                        flagEntro = false;
                    }
                    else
                    {
                        AutorizadorActual = "gerenteObra";
                    }
                }
                if (raw.GerenteDirector.Equals(usuarioActual) && flagEntro)
                {

                    if (!raw.firmaGerenteDirector)
                    {
                        AutorizadorActual = "GerenteDirector";
                        flagEntro = false;
                    }
                    else
                    {
                        AutorizadorActual = "GerenteDirector";
                    }
                }
                if (raw.directorDivision.Equals(usuarioActual) && flagEntro)
                {
                    if (!raw.firmaDirectorDivision)
                    {
                        AutorizadorActual = "directorDivision";
                        flagEntro = false;
                    }
                    else
                    {
                        AutorizadorActual = "directorDivision";
                    }
                }
                if (raw.directorServicios.Equals(usuarioActual) && flagEntro)
                {

                    if (!raw.firmaServicios)
                    {
                        AutorizadorActual = "directorServicios";
                        flagEntro = false;
                    }
                    else
                    {
                        AutorizadorActual = "directorServicios";
                    }
                }
                if (raw.altaDireccion.Equals(usuarioActual) && flagEntro)
                {

                    if (!raw.firmaAltaDireccion)
                    {
                        AutorizadorActual = "altaDireccion";
                        flagEntro = false;
                    }
                    else
                    {
                        AutorizadorActual = "altaDireccion";
                    }
                }

                var AutorizadorElabora = usuarioFS.ListUsersById(raw.usuarioElaboro).Select(x => new AutorizadoresDTO
                {
                    idUsuario = x.id,
                    nombreUsuario = x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno,
                    firma = raw.firmaElaboro,
                    firmaCadena = raw.cadenaFirmaElabora
                }).FirstOrDefault();

                var AutorizadorGerente = usuarioFS.ListUsersById(raw.gerenteObra).Select(x => new AutorizadoresDTO
                {
                    idUsuario = x.id,
                    nombreUsuario = x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno,
                    firma = raw.firmaGerenteObra,
                    firmaCadena = raw.cadenaFirmaGerenteObra
                }).FirstOrDefault();
                //Cuando se ajuste el guardado de directores hay que asignarse correctamente
                var AutorizadorGerenteDirector = usuarioFS.ListUsersById(raw.GerenteDirector).Select(x => new AutorizadoresDTO
                {
                    idUsuario = x.id,
                    nombreUsuario = x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno,
                    firma = raw.firmaGerenteDirector,
                    firmaCadena = raw.cadenaFirmaGerenteDirector
                }).FirstOrDefault();

                var AutorizadorDirector = usuarioFS.ListUsersById(raw.directorDivision).Select(x => new AutorizadoresDTO
                {
                    idUsuario = x.id,
                    nombreUsuario = x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno,
                    firma = raw.firmaDirectorDivision,
                    firmaCadena = raw.cadenaFirmaDirector
                }).FirstOrDefault();

                var AutorizadorServicios = usuarioFS.ListUsersById(raw.directorServicios).Select(x => new AutorizadoresDTO
                {
                    idUsuario = x.id,
                    nombreUsuario = x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno,
                    firma = raw.firmaServicios,
                    firmaCadena = raw.cadenaFirmaServicios
                }).FirstOrDefault();

                var AutorizadorDireccion = usuarioFS.ListUsersById(raw.altaDireccion).Select(x => new AutorizadoresDTO
                {
                    idUsuario = x.id,
                    nombreUsuario = x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno,
                    firma = raw.firmaAltaDireccion,
                    firmaCadena = raw.cadenaFirmaDireccion
                }).FirstOrDefault();

                


                var solicitudObj = solicitudEquipoFS.listaSolicitudEquipo(obj);

                result.Add("Centro_costos", solicitudObj.CC);
                result.Add("AutorizadorActual", AutorizadorActual);
                result.Add("AutorizadorElabora", AutorizadorElabora);
                result.Add("AutorizadorGerente", AutorizadorGerente);
                result.Add("AutorizadorGerenteDirector", AutorizadorGerenteDirector);
                result.Add("AutorizadorDirector", AutorizadorDirector);
                result.Add("AutorizadorDireccion", AutorizadorDireccion);
                result.Add("AutorizadorServicios", AutorizadorServicios);
                result.Add("idAutorizacion", raw.id);
                result.Add("observaciones", raw.observaciones);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public bool multiPuestos(int usuarioActual, tblM_AutorizacionSolicitudes raw)
        {
            string AutorizadorActual = "";

            if (raw.usuarioElaboro.Equals(usuarioActual) && raw.firmaElaboro == false)
            {
                AutorizadorActual = "Elaboro";
            }
            if (raw.gerenteObra.Equals(usuarioActual))
            {
                AutorizadorActual = "gerenteObra";
            }
            if (raw.GerenteDirector.Equals(usuarioActual))
            {
                AutorizadorActual = "GerenteDirector";
            }
            if (raw.directorDivision.Equals(usuarioActual))
            {
                AutorizadorActual = "directorDivision";
            }
            if (raw.altaDireccion.Equals(usuarioActual))
            {
                AutorizadorActual = "altaDireccion";
            }

            return false;
        }

        public ActionResult GetDataAutorizadoresReemplazo(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var raw = authSolicitudReemplazoFS.getAutorizadores(id); //.getAutorizadores(obj);

                var reporte = solicitudEquipoReemplazoDetFS.GetSolicitudReemplazoDetByIdSolicitud(id);
                List<rptSolicitudEquipoReemplazoDTO> listaDetalle = new List<rptSolicitudEquipoReemplazoDTO>();
                foreach (var item in reporte)
                {
                    rptSolicitudEquipoReemplazoDTO objReporte = new rptSolicitudEquipoReemplazoDTO();

                    var EquipoMaquinaria = maquinaFS.GetMaquinaByID(item.AsignacionEquipos.noEconomicoID).FirstOrDefault();

                    objReporte.Tipo = EquipoMaquinaria.grupoMaquinaria.tipoEquipo.descripcion;
                    objReporte.Modelo = EquipoMaquinaria.modeloEquipo.descripcion;
                    objReporte.Economico = EquipoMaquinaria.noEconomico;
                    objReporte.FechaEntrega = DateTime.Now.ToShortDateString();
                    listaDetalle.Add(objReporte);
                }

                Session["FolioSolReemplazo"] = raw.SolicitudReemplazoEquipo.folio;
                Session["rptSolicitudEquipoReemplazoDTO"] = listaDetalle;

                AutorizadoresReemplazoDTO autorizadores = new AutorizadoresReemplazoDTO();
                AutorizadoresReemplazoDTO firmasAutorizadores = new AutorizadoresReemplazoDTO();

                int usuarioActual = base.getUsuario().id;
                string AutorizadorActual = "";

                if (raw.idAutorizaElabora.Equals(usuarioActual))
                {
                    AutorizadorActual = "Elaboro";
                }
                if (raw.idAutorizaGerente.Equals(usuarioActual))
                {
                    AutorizadorActual = "gerenteObra";
                }
                if (raw.idAutorizaAsigna.Equals(usuarioActual))
                {
                    AutorizadorActual = "asigna";
                }

                var AutorizadorElabora = usuarioFS.ListUsersById(raw.idAutorizaElabora).Select(x => new AutorizadoresDTO
                {
                    idUsuario = x.id,
                    nombreUsuario = x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno,
                    firma = raw.AutorizaElabora,
                    firmaCadena = raw.CadenaElabora
                }).FirstOrDefault();

                var AutorizadorGerente = usuarioFS.ListUsersById(raw.idAutorizaGerente).Select(x => new AutorizadoresDTO
                {
                    idUsuario = x.id,
                    nombreUsuario = x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno,
                    firma = raw.AutorizaGerente,
                    firmaCadena = raw.CadenaGerente
                }).FirstOrDefault();

                var AutorizadorAsigna = usuarioFS.ListUsersById(raw.idAutorizaAsigna).Select(x => new AutorizadoresDTO
                {
                    idUsuario = x.id,
                    nombreUsuario = x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno,
                    firma = raw.AutorizaAsigna,
                    firmaCadena = raw.CadenaAsigna
                }).FirstOrDefault();

                autorizadores.nombreasigna = AutorizadorAsigna.nombreUsuario;
                autorizadores.nombreElabora = AutorizadorElabora.nombreUsuario;
                autorizadores.nombreGerente = AutorizadorGerente.nombreUsuario;

                if (AutorizadorAsigna.firmaCadena != null)
                    firmasAutorizadores.nombreasigna = AutorizadorAsigna.firmaCadena;
                if (AutorizadorElabora.firmaCadena != null)
                    firmasAutorizadores.nombreElabora = AutorizadorElabora.firmaCadena;
                if (AutorizadorGerente.firmaCadena != null)
                    firmasAutorizadores.nombreGerente = AutorizadorGerente.firmaCadena;

                Session["rptCC"] = raw.SolicitudReemplazoEquipo.CC;

                Session["rptAutorizadores"] = autorizadores;
                Session["firmasAutorizadores"] = firmasAutorizadores;

                result.Add("AutorizadorActual", AutorizadorActual);
                result.Add("AutorizadorElabora", AutorizadorElabora);
                result.Add("AutorizadorGerente", AutorizadorGerente);
                result.Add("AutorizadorAsigna", AutorizadorAsigna);
                result.Add("idAutorizacion", raw.id);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetInfoUsuarios(string term, int gerente, int director, int direccion, int Elabora)
        {
            List<int> usuarios = new List<int>();
            usuarios.Add(gerente);
            usuarios.Add(director);
            usuarios.Add(direccion);
            usuarios.Add(Elabora);
            var items = usuarioFS.ListUsuariosAutoComplete(term, 0).Where(x => !usuarios.Contains(x.id));

            var filteredItems = items.Select(x => new { id = x.id, label = x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno });
            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetInfoSolicitud(string obj)
        {
            var result = new Dictionary<string, object>();
            try
            {

                var res = centroCostosFS.getNombreAreaCuent(obj);
                var folio = solicitudEquipoFS.GetFolio(obj).PadLeft(6, '0');
                var folioReemplazo = solicitudReemplazoFS.GetFolio(obj).PadLeft(6, '0');//.GetFolio(obj).PadLeft(6, '0');


                result.Add("folioReemplazo", folioReemplazo);
                result.Add("folio", folio);
                result.Add("descripcionCC", res);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);

            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetSolicitudesReemplazoPendientes(int filtro, string folio)
        {
            var result = new Dictionary<string, object>();
            try
            {

                var idusuario = getUsuario().id;
                var listResult = authSolicitudReemplazoFS.getListPendientes(idusuario, filtro, folio).ToList()
                    .Select(x => new
                    {
                        Folio = x.folio,
                        CCName = centroCostosFS.getNombreCCFix(x.CC),
                        UsuarioSolicitud = usuarioFS.ListUsersById(x.usuarioID).FirstOrDefault().nombre,
                        Fecha = (x.fechaElaboracion).ToString("dd/MM/yyyy"),
                        id = x.id,
                        cc = x.CC,
                        comentario = authSolicitudReemplazoFS.obtenerComentarioSolicitudReemplazo(x.id)
                    });

                result.Add("Autorizadas", listResult);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetReporte(int obj)
        {
            var result = new Dictionary<string, object>();
            try
            {

                Session["rptCadenaAutorizacion"] = null;
                var objSolicitud = solicitudEquipoFS.listaSolicitudEquipo(obj);

                if (objSolicitud != null)
                {

                    if (!objSolicitud.ArranqueObra)
                    {

                        List<SolicitudEquipoDTO> array = solicitudEquipoFS.getListaDetalleSolicitudAutorizacion(obj);

                        var res = authSolicitudesFS.getAutorizadores(obj);

                        if (res != null)
                        {
                            CadenaAutorizacionDTO cadenaAutorizacionDTO = new CadenaAutorizacionDTO
                            {
                                CadenaDireccion = res.cadenaFirmaDireccion,
                                CadenaDirector = res.cadenaFirmaDirector,
                                CadenaElabora = res.cadenaFirmaElabora,
                                CadenaGerente = res.cadenaFirmaGerenteObra,
                                CadenaGerenteDirector = res.cadenaFirmaGerenteDirector,
                                CadenaServicios = res.cadenaFirmaServicios
                            };

                            AutorizadoresIDDTO autorizadores = new AutorizadoresIDDTO
                            {
                                altaDireccion = res.altaDireccion,
                                directorDivision = res.directorDivision,
                                gerenteObra = res.gerenteObra,
                                usuarioElaboro = res.usuarioElaboro,
                                GerenteDirector = res.GerenteDirector,
                                directorServicios = res.directorServicios
                            };
                            List<SolicitudEquipoJustificacionDTO> arrayJustificacion = new List<SolicitudEquipoJustificacionDTO>();
                            arrayJustificacion.AddRange(solicitudEquipoFS.getListaJustificacionSolicitud(obj));
                            Session["rptSolicitudEquipo"] = array;
                            Session["rptSolicitudEquipoJustificacion"] = arrayJustificacion;
                            if (array != null)
                            {
                                Session["rptAutorizadores"] = autorizadores;
                                Session["rptSolicitudEquipo"] = array;
                                if (cadenaAutorizacionDTO != null)
                                {
                                    Session["rptCadenaAutorizacion"] = cadenaAutorizacionDTO;

                                }

                                var idReporte = 30;
                                result.Add("idReporte", idReporte);
                                result.Add(SUCCESS, true);
                            }
                            else
                            {
                                result.Add(SUCCESS, false);
                            }
                        }
                    }
                    else
                    {
                        var res = authSolicitudesFS.getAutorizadores(obj);
                        var getDetalle = solicitudEquipoDetFS.listaDetalleSolicitud(obj);

                        var Asginacion = asignacionEquiposFS.getAsignacionesByID(obj);

                        var array = Asginacion.ToList().Select(x => new SolicitudEquipoDTO
                        {
                            id = 0,
                            Folio = res.folio,
                            TipoId = 0,
                            Grupoid = 0,
                            Modeloid = 0,
                            Tipo = getDetalle.FirstOrDefault(d => d.id.Equals(x.SolicitudDetalleId)) != null ? getDetalle.FirstOrDefault(d => d.id.Equals(x.SolicitudDetalleId)).TipoMaquinaria.descripcion : "",
                            Grupo = getDetalle.FirstOrDefault(d => d.id.Equals(x.SolicitudDetalleId)) != null ? getDetalle.FirstOrDefault(d => d.id.Equals(x.SolicitudDetalleId)).GrupoMaquinaria.descripcion : "",
                            Modelo = getDetalle.FirstOrDefault(d => d.id.Equals(x.SolicitudDetalleId)) != null ? getDetalle.FirstOrDefault(d => d.id.Equals(x.SolicitudDetalleId)).ModeloEquipo.descripcion : "",
                            Descripcion = getDetalle.FirstOrDefault(d => d.id.Equals(x.SolicitudDetalleId)) != null ? getDetalle.FirstOrDefault(d => d.id.Equals(x.SolicitudDetalleId)).Comentario : "",
                            pFechaInicio = x.fechaInicio.ToShortDateString(),
                            pFechaFin = x.fechaFin.ToShortDateString(),
                            pHoras = Convert.ToInt32(x.Horas),
                            pTipoPrioridad = getDetalle.FirstOrDefault(d => d.id.Equals(x.SolicitudDetalleId)) != null ? (getDetalle.FirstOrDefault(d => d.id.Equals(x.SolicitudDetalleId)).prioridad == 0 ? "A" : getDetalle.FirstOrDefault(d => d.id.Equals(x.SolicitudDetalleId)).prioridad == 2 ? "B" : "C") : "C",
                            Economico = x.Economico,
                            EconomicoRenta = "",
                            FechaPromesa = x.FechaPromesa.ToShortDateString()
                        });

                        AutorizadoresIDDTO autorizadores = new AutorizadoresIDDTO
                        {
                            altaDireccion = res.altaDireccion,
                            directorDivision = res.directorDivision,
                            gerenteObra = res.gerenteObra,
                            usuarioElaboro = res.usuarioElaboro,
                            GerenteDirector = res.GerenteDirector,
                            directorServicios = res.directorServicios
                        };

                        CadenaAutorizacionDTO cadenaAutorizacionDTO = new CadenaAutorizacionDTO
                        {
                            CadenaDireccion = res.cadenaFirmaDireccion,
                            CadenaDirector = res.cadenaFirmaDirector,
                            CadenaElabora = res.cadenaFirmaElabora,
                            CadenaGerente = res.cadenaFirmaGerenteObra,
                            CadenaGerenteDirector = res.cadenaFirmaGerenteDirector,
                            CadenaServicios = res.cadenaFirmaServicios
                        };

                        var c = base.getUsuario();
                        Session["rptSolicitudEquipo"] = "";

                        if (array != null)
                        {
                            Session["rptAutorizadores"] = autorizadores;
                            Session["rptSolicitudEquipo"] = array.ToList();
                            if (cadenaAutorizacionDTO != null)
                            {
                                Session["rptCadenaAutorizacion"] = cadenaAutorizacionDTO;
                                Session["rptAsigna"] = "Oscar Manuel Roman Ruiz";
                            }
                            result.Add(SUCCESS, true);
                            var idReporte = 12;
                            result.Add("idReporte", idReporte);
                        }
                        else
                        {
                            result.Add(SUCCESS, false);
                        }
                    }
                    result.Add("descarga", (string.IsNullOrEmpty(objSolicitud.link)?false:true));
                }
                else
                {
                    var idReporte = 30;
                    result.Add("idReporte", idReporte);
                    result.Add(SUCCESS, false);
                }
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetReporte2(int obj)
        {
            var result = new Dictionary<string, object>();
            try
            {

                Session["rptCadenaAutorizacion"] = null;
                var objSolicitud = solicitudEquipoFS.listaSolicitudEquipo(obj);

                if (objSolicitud != null)
                {
                    var res = authSolicitudesFS.getAutorizadores(obj);
                    var getDetalle = solicitudEquipoDetFS.listaDetalleSolicitud(obj);

                    var Asginacion = asignacionEquiposFS.getAsignacionesByID(obj);

                    var array = Asginacion.ToList().Select(x => new SolicitudEquipoDTO
                    {
                        id = 0,
                        Folio = res.folio,
                        TipoId = 0,
                        Grupoid = 0,
                        Modeloid = 0,
                        Tipo = getDetalle.FirstOrDefault(d => d.id.Equals(x.SolicitudDetalleId)) != null ? getDetalle.FirstOrDefault(d => d.id.Equals(x.SolicitudDetalleId)).TipoMaquinaria.descripcion : "",
                        Grupo = getDetalle.FirstOrDefault(d => d.id.Equals(x.SolicitudDetalleId)) != null ? getDetalle.FirstOrDefault(d => d.id.Equals(x.SolicitudDetalleId)).GrupoMaquinaria.descripcion : "",
                        Modelo = getDetalle.FirstOrDefault(d => d.id.Equals(x.SolicitudDetalleId)) != null ? getDetalle.FirstOrDefault(d => d.id.Equals(x.SolicitudDetalleId)).ModeloEquipo.descripcion : "",
                        Descripcion = getDetalle.FirstOrDefault(d => d.id.Equals(x.SolicitudDetalleId)) != null ? getDetalle.FirstOrDefault(d => d.id.Equals(x.SolicitudDetalleId)).Comentario : "",
                        pFechaInicio = x.fechaInicio.ToShortDateString(),
                        pFechaFin = x.fechaFin.ToShortDateString(),
                        pHoras = Convert.ToInt32(x.Horas),
                        pTipoPrioridad = getDetalle.FirstOrDefault(d => d.id.Equals(x.SolicitudDetalleId)) != null ? (getDetalle.FirstOrDefault(d => d.id.Equals(x.SolicitudDetalleId)).prioridad == 0 ? "A" : getDetalle.FirstOrDefault(d => d.id.Equals(x.SolicitudDetalleId)).prioridad == 2 ? "B" : "C") : "C",
                        Economico = x.Economico,
                        EconomicoRenta = "",
                        FechaPromesa = x.FechaPromesa.ToShortDateString()
                    });

                    AutorizadoresIDDTO autorizadores = new AutorizadoresIDDTO
                    {
                        altaDireccion = res.altaDireccion,
                        directorDivision = res.directorDivision,
                        gerenteObra = res.gerenteObra,
                        usuarioElaboro = res.usuarioElaboro,
                        GerenteDirector = res.GerenteDirector,
                        directorServicios = res.directorServicios
                    };

                    CadenaAutorizacionDTO cadenaAutorizacionDTO = new CadenaAutorizacionDTO
                    {
                        CadenaDireccion = res.cadenaFirmaDireccion,
                        CadenaDirector = res.cadenaFirmaDirector,
                        CadenaElabora = res.cadenaFirmaElabora,
                        CadenaGerente = res.cadenaFirmaGerenteObra,
                        CadenaGerenteDirector = res.cadenaFirmaGerenteDirector,
                        CadenaServicios = res.cadenaFirmaServicios
                    };

                    var c = base.getUsuario();
                    Session["rptSolicitudEquipo"] = "";

                    if (array.ToList().Count > 0)
                    {
                        Session["rptAutorizadores"] = autorizadores;
                        Session["rptSolicitudEquipo"] = array.ToList();
                        if (cadenaAutorizacionDTO != null)
                        {
                            Session["rptCadenaAutorizacion"] = cadenaAutorizacionDTO;
                            Session["rptAsigna"] = "Oscar Manuel Roman Ruiz";
                        }
                        result.Add(SUCCESS, true);
                        var idReporte = 12;
                        result.Add("idReporte", idReporte);
                    }
                    else
                    {
                        Session["rptAutorizadores"] = autorizadores;


                        array = getDetalle.ToList().Select(x => new SolicitudEquipoDTO
                          {
                              id = 0,
                              Folio = res.folio,
                              TipoId = 0,
                              Grupoid = 0,
                              Modeloid = 0,
                              Tipo = x.TipoMaquinaria.descripcion,
                              Grupo = x.GrupoMaquinaria.descripcion,
                              Modelo = x.ModeloEquipo.descripcion,
                              Descripcion = x.Comentario,
                              pFechaInicio = x.fechaInicio.ToShortDateString(),
                              pFechaFin = x.fechaFin.ToShortDateString(),
                              pHoras = Convert.ToInt32(x.horas),
                              pTipoPrioridad = x.prioridad == 0 ? "A" : x.prioridad == 2 ? "B" : "C",
                              Economico = "",
                              EconomicoRenta = "",
                              FechaPromesa = ""
                          });
                        Session["rptSolicitudEquipo"] = array.ToList();
                        if (cadenaAutorizacionDTO != null)
                        {
                            Session["rptCadenaAutorizacion"] = cadenaAutorizacionDTO;
                            Session["rptAsigna"] = "Oscar Manuel Roman Ruiz";
                        }

                        var idReporte = 12;
                        result.Add("idReporte", idReporte);
                        result.Add(SUCCESS, true);
                    }
                    
                }

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetReporteNuevAutorizacion(int obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                Session["rptCadenaAutorizacion"] = null;
                var objSolicitud = solicitudEquipoFS.listaSolicitudEquipo(obj);

                var res = authSolicitudesFS.getAutorizadores(obj);
                var getDetalle = solicitudEquipoDetFS.listaDetalleSolicitud(obj);

                var Asginacion = asignacionEquiposFS.getAsignacionesByID(obj);
                var array = Asginacion.ToList().Select(x => new SolicitudEquipoDTO
                {
                    id = 0,
                    Folio = res.folio,
                    TipoId = 0,
                    Grupoid = 0,
                    Modeloid = 0,
                    Tipo = getDetalle.FirstOrDefault(d => d.id.Equals(x.SolicitudDetalleId)) != null ? getDetalle.FirstOrDefault(d => d.id.Equals(x.SolicitudDetalleId)).TipoMaquinaria.descripcion : "",
                    Grupo = getDetalle.FirstOrDefault(d => d.id.Equals(x.SolicitudDetalleId)) != null ? getDetalle.FirstOrDefault(d => d.id.Equals(x.SolicitudDetalleId)).GrupoMaquinaria.descripcion : "",
                    Modelo = getDetalle.FirstOrDefault(d => d.id.Equals(x.SolicitudDetalleId)) != null ? getDetalle.FirstOrDefault(d => d.id.Equals(x.SolicitudDetalleId)).ModeloEquipo.descripcion : "",
                    Descripcion = getDetalle.FirstOrDefault(d => d.id.Equals(x.SolicitudDetalleId)) != null ? getDetalle.FirstOrDefault(d => d.id.Equals(x.SolicitudDetalleId)).Comentario : "",
                    pFechaInicio = x.fechaInicio.ToShortDateString(),
                    pFechaFin = x.fechaFin.ToShortDateString(),
                    pHoras = Convert.ToInt32(x.Horas),
                    pTipoPrioridad = getDetalle.FirstOrDefault(d => d.id.Equals(x.SolicitudDetalleId)) != null ? (getDetalle.FirstOrDefault(d => d.id.Equals(x.SolicitudDetalleId)).prioridad == 0 ? "A" : getDetalle.FirstOrDefault(d => d.id.Equals(x.SolicitudDetalleId)).prioridad == 2 ? "B" : "C") : "C",
                    Economico = x.Economico,
                    EconomicoRenta = "",
                    FechaPromesa = x.FechaPromesa.ToShortDateString()
                });

                AutorizadoresIDDTO autorizadores = new AutorizadoresIDDTO
                {
                    altaDireccion = res.altaDireccion,
                    directorDivision = res.directorDivision,
                    gerenteObra = res.gerenteObra,
                    usuarioElaboro = res.usuarioElaboro,
                    GerenteDirector = res.GerenteDirector,
                    directorServicios = res.directorServicios
                };

                CadenaAutorizacionDTO cadenaAutorizacionDTO = new CadenaAutorizacionDTO
                {
                    CadenaDireccion = res.cadenaFirmaDireccion,
                    CadenaDirector = res.cadenaFirmaDirector,
                    CadenaElabora = res.cadenaFirmaElabora,
                    CadenaGerente = res.cadenaFirmaGerenteObra,
                    CadenaGerenteDirector = res.cadenaFirmaGerenteDirector,
                    CadenaServicios = res.cadenaFirmaServicios
                };

                var c = base.getUsuario();
                Session["rptSolicitudEquipo"] = "";

                if (array != null)
                {
                    Session["rptAutorizadores"] = autorizadores;
                    Session["rptSolicitudEquipo"] = array.ToList();
                    if (cadenaAutorizacionDTO != null)
                    {
                        Session["rptCadenaAutorizacion"] = cadenaAutorizacionDTO;
                        Session["rptAsigna"] = "Oscar Manuel Roman Ruiz";
                    }
                    result.Add(SUCCESS, true);
                    var idReporte = 12;
                    result.Add("idReporte", idReporte);
                }
                else
                {
                    result.Add(SUCCESS, false);
                }
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDataSolicitudesAutorizadas()
        {
            var result = new Dictionary<string, object>();
            try
            {

                var idusuario = getUsuario().id;
                var listResult = solicitudEquipoFS.getListAutorizadas(idusuario).ToList()
                      .Select(x => new
                      {
                          Folio = x.folio,
                          CCName = centroCostosFS.getNombreCCFix(x.CC.ToString()),
                          UsuarioSolicitud = usuarioFS.ListUsersById(x.usuarioID).FirstOrDefault().nombre,
                          Fecha = (x.fechaElaboracion).ToString("dd/MM/yyyy"),
                          id = x.id,
                          CentroCostos = x.CC
                      });
                result.Add("Pendientes", listResult);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDataSolicitudesAutorizadasAsignacion(int filtro)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var objFiltro = false;
                var lista = new List<SolicitudesPendientesDTO>();
                var idusuario = getUsuario().id;
                if (filtro == 1 || filtro == 2)
                {
                    if (filtro == 2)
                    {
                        objFiltro = true;
                    }
                    var listResult = solicitudEquipoFS.GetListaSolicitudesAutorizadas(idusuario).ToList();
                    var listaCentroCostos = new List<string>();
                    if (base.getAction("LibreCC"))
                    {
                        listaCentroCostos = null;
                    }
                    else
                    {

                        listaCentroCostos = base.GetListaCentroCostos().Select(x => x.cc).ToList();

                        listResult = listResult.Where(x => listaCentroCostos.Contains(x.CC)).ToList();
                    }
                    foreach (var item in listResult.Where(x => x.Estatus.Equals(objFiltro)))
                    {
                        var HasPreasignadas = asignacionEquiposFS.getAsignacionesByIDs(item.id).Where(x => x.estatus == 0).Count();
                        var hasAsignacion = 0;

                        if (HasPreasignadas > 0)
                        {
                            hasAsignacion = 1;
                        }

                        var obj = new SolicitudesPendientesDTO();
                        var usuario = usuarioFS.ListUsersById(item.usuarioID).FirstOrDefault();
                        string nombre = "";
                        if (usuario != null)
                        {
                            nombre = usuario.nombre;
                        }
                        obj.id = item.id;
                        obj.Folio = item.folio;
                        obj.UsuarioSolicitud = nombre;
                        obj.CCName = centroCostosFS.getNombreCCFix(item.CC);
                        obj.Fecha = (item.fechaElaboracion).ToString("dd/MM/yyyy");
                        obj.CentroCostos = item.CC;
                        obj.hasAsignacion = hasAsignacion;
                        obj.tipoAsignacion = 1;
                        lista.Add(obj);

                    }
                }
                else
                {
                    if (filtro == 4)
                    {
                        filtro = 2;
                    }
                    else
                    {
                        filtro = 1;
                    }

                    var SolicitudesReemplazo = solicitudEquipoReemplazoDetFS.GetSolicitudesPendientes(filtro);
                    foreach (var item in SolicitudesReemplazo.Distinct())
                    {
                        var obj = new SolicitudesPendientesDTO();

                        obj.id = item.id;
                        obj.Folio = item.folio;
                        obj.UsuarioSolicitud = getUsuarioNombre(item.usuarioID);
                        obj.CCName = centroCostosFS.getNombreCCFix(item.CC);
                        obj.Fecha = (item.fechaElaboracion).ToString("dd/MM/yyyy");
                        obj.CentroCostos = item.CC;
                        obj.hasAsignacion = 0;
                        obj.tipoAsignacion = 2;
                        lista.Add(obj);
                    }
                }
                result.Add("Pendientes", lista);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        tblM_SolicitudEquipoDet objEquipoDetalle;

        public ActionResult FillDetalleSolictudAsignacion(int obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<SolicitudEquipoDTO> tblDetalleSolicitud = new List<SolicitudEquipoDTO>();
                var rawAsignacion = asignacionEquiposFS.getAsignacionesByID(obj);

                var ListaAsignadosParciales = rawAsignacion.Where(x => x.estatus == 0).Select(y => y.SolicitudDetalleId).ToList();

                var solicitud = solicitudEquipoFS.loadSolicitudById(obj);

                var raw = solicitudEquipoDetFS.listaDetalleSolicitud(obj).Where(x => x.estatus == false || ListaAsignadosParciales.Contains(x.id));
                var ListaGrupos = raw.Select(x => x.grupoMaquinariaID).Distinct().ToList();

                var ListaMaquinaria = maquinaFS.ListaEquiposGrupo(ListaGrupos);

                if (rawAsignacion != null)
                {
                    var AsignacionesRastreo = rawAsignacion.Where(x => x.estatus >= 1).ToList();
                    List<RastreoAsignacionesDTO> listaAsignacionesRastreo = new List<RastreoAsignacionesDTO>();

                    foreach (var x in AsignacionesRastreo)
                    {
                        RastreoAsignacionesDTO objAsignacionesRastreo = new RastreoAsignacionesDTO();

                        objAsignacionesRastreo.LugarRecepcion = GetOrigenCC(x.cc);
                        objAsignacionesRastreo.LugarOrigen = GetOrigenCC(x.CCOrigen);
                        string EconomicoRastreo = "";
                        string TipoEquipo = "PROPIO";
                        if (x.noEconomicoID != 0)
                        {
                            EconomicoRastreo = maquinaFS.GetMaquinaByID(x.noEconomicoID).FirstOrDefault().noEconomico;
                        }

                        if (x.Economico == "RENTA" || x.Economico == "COMPRA" || x.Economico == "RENTA COMPRA")
                        {
                            TipoEquipo = x.Economico;
                        }

                        objAsignacionesRastreo.Economico = EconomicoRastreo;
                        objAsignacionesRastreo.TipoEconomico = TipoEquipo;
                        objAsignacionesRastreo.FechaPromesa = x.FechaPromesa.ToShortDateString();
                        objAsignacionesRastreo.Estado = x.estatus.ToString();

                        listaAsignacionesRastreo.Add(objAsignacionesRastreo);

                    }
                    result.Add("AsignacionesRastreo", listaAsignacionesRastreo);
                }
                tblDetalleSolicitud = raw.Select(x => new SolicitudEquipoDTO
                {
                    id = x.id,
                    Folio = x.folio,
                    Tipo = x.TipoMaquinaria.descripcion,
                    Grupo = x.GrupoMaquinaria.descripcion,
                    Modelo = x.ModeloEquipo.descripcion,
                    pFechaInicio = x.fechaInicio.ToShortDateString(),
                    pFechaFin = x.fechaFin.ToShortDateString(),
                    Comentario = x.Comentario,
                    Economico = getCboEconomicos(x.grupoMaquinariaID, ListaMaquinaria, x.id),
                    idNoEconomico = ReturnIdEconomico(rawAsignacion, x.id),
                    pFechaObra = returnFechaPromesa(rawAsignacion, x.id),
                    tipoAsignacion = 1
                }).ToList();


                var tblListaAsiganados = rawAsignacion.Where(x => x.estatus >= 1).Select(x => new
                {
                    id = x.SolicitudDetalleId,
                    Folio = x.folio,
                    noEconomicoID = x.noEconomicoID,
                    Economico = x.noEconomicoID != 0 ? maquinaFS.GetMaquinaByID(x.noEconomicoID).FirstOrDefault().noEconomico : "PENDIENTE",
                    TipoEquipo = x.noEconomicoID == 0 ? x.Economico : "PROPIO",
                    FechaPromesa = x.FechaPromesa.ToShortDateString(),
                    pFechaInicio = x.fechaInicio.ToShortDateString(),
                    pFechaFin = x.fechaFin.ToShortDateString(),
                    Comentario = ReturnObjDet(x.SolicitudDetalleId) == null ? "" : objEquipoDetalle.Comentario,
                    Tipo = ReturnObjDet(x.SolicitudDetalleId) == null ? "" : objEquipoDetalle.TipoMaquinaria.descripcion,
                    Grupo = ReturnObjDet(x.SolicitudDetalleId) == null ? "" : objEquipoDetalle.GrupoMaquinaria.descripcion,
                    Modelo = ReturnObjDet(x.SolicitudDetalleId) == null ? "" : objEquipoDetalle.ModeloEquipo.descripcion,
                    tipoAsignacion = 1
                });


                result.Add("folio", solicitud.folio);
                result.Add("CC", solicitud.CC);
                result.Add("AsignadosRaw", tblListaAsiganados);

                result.Add("dataDetalleSolicitud", tblDetalleSolicitud);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult FillDetalleReemplazo(int obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<SolicitudEquipoDTO> tblDetalleSolicitud = new List<SolicitudEquipoDTO>();
                List<AsignadosDTO> tblListaAsiganados = new List<AsignadosDTO>();
                var solicitud = solicitudEquipoReemplazoDetFS.GetSolicitudReemplazoDetByIdSolicitud(obj);

                foreach (var item in solicitud.Where(x => x.estatus == 0))
                {
                    var x = solicitudEquipoDetFS.DetSolicitud(item.AsignacionEquipos.SolicitudDetalleId);
                    var rawAsignacion = item.AsignacionEquipos;
                    List<int> gp = new List<int>();
                    gp.Add(x.grupoMaquinariaID);
                    var ListaMaquinaria = maquinaFS.ListaEquiposGrupo(gp);

                    var objItem = new SolicitudEquipoDTO
                        {
                            id = x.id,
                            Folio = x.folio,
                            Tipo = x.TipoMaquinaria.descripcion,
                            Grupo = x.GrupoMaquinaria.descripcion,
                            Modelo = x.ModeloEquipo.descripcion,
                            pFechaInicio = item.fechaInicio==null?x.fechaInicio.ToShortDateString():((DateTime)item.fechaInicio).ToShortDateString(),
                            pFechaFin = item.fechaFin==null?x.fechaFin.ToShortDateString():((DateTime)item.fechaFin).ToShortDateString(),
                            Comentario = x.Comentario,
                            Economico = getCboEconomicos(x.grupoMaquinariaID, ListaMaquinaria, x.id),
                            idNoEconomico = item.AsignacionEquipos.noEconomicoID,
                            pFechaObra = item.fechaInicio == null ? item.AsignacionEquipos.FechaPromesa.ToShortDateString() : ((DateTime)item.fechaInicio).ToShortDateString(),
                            tipoAsignacion = 2
                        };
                    tblDetalleSolicitud.Add(objItem);
                }

                foreach (var item in solicitud.Where(x => x.estatus != 0))
                {
                    var equipo = asignacionEquiposFS.getAsignacionesbySDet(item.AsignacionEquipos.SolicitudDetalleId).OrderByDescending(x => x.id).FirstOrDefault();
                    var sol = solicitudEquipoDetFS.DetSolicitud(item.AsignacionEquipos.SolicitudDetalleId);


                    var AsignacionDTOd = new AsignadosDTO
                    {
                        id = sol.id,
                        Folio = sol.folio,
                        noEconomicoID = equipo.noEconomicoID,
                        Economico = equipo.noEconomicoID != 0 ? maquinaFS.GetMaquinaByID(equipo.noEconomicoID).FirstOrDefault().noEconomico : equipo.Economico,
                        TipoEquipo = equipo.Economico == "RENTA" ? "RENTA" : equipo.Economico == "COMPRA" ? "COMPRA" : "PROPIO",
                        FechaPromesa = item.fechaInicio == null ? item.AsignacionEquipos.FechaPromesa.ToShortDateString() : ((DateTime)item.fechaInicio).ToShortDateString(),
                        pFechaInicio = item.fechaInicio == null ? sol.fechaInicio.ToShortDateString() : ((DateTime)item.fechaInicio).ToShortDateString(),
                        pFechaFin = item.fechaFin == null ? sol.fechaFin.ToShortDateString() : ((DateTime)item.fechaFin).ToShortDateString(),
                        Comentario = string.IsNullOrEmpty(sol.Comentario) ? "" : sol.Comentario,
                        Tipo = sol.tipoMaquinariaID == null ? "" : sol.TipoMaquinaria.descripcion,
                        Grupo = sol.grupoMaquinariaID == null ? "" : sol.GrupoMaquinaria.descripcion,
                        Modelo = sol.modeloEquipoID == null ? "" : sol.ModeloEquipo.descripcion,
                        tipoAsignacion = 2
                    };

                    tblListaAsiganados.Add(AsignacionDTOd);

                }

                result.Add("folio", solicitud.FirstOrDefault().SolicitudEquipoReemplazo.folio);
                result.Add("CC", solicitud.FirstOrDefault().AsignacionEquipos.cc);
                result.Add("AsignadosRaw", tblListaAsiganados);
                result.Add("dataDetalleSolicitud", tblDetalleSolicitud);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        private string GetOrigenCC(string x)
        {
            string s = "";
            if (!string.IsNullOrEmpty(x))
            {
                switch (x)
                {
                    case "1010":
                        return "TMC HERMOSILLO";
                    case "1015":
                        return "PATIO MAQUINARIA HERMOSILLO";
                    default:
                        s = centroCostosFS.getNombreCCFix(x);
                        return s;
                }
            }
            return s;
        }

        public tblM_SolicitudEquipoDet ReturnObjDet(int id)
        {
            objEquipoDetalle = solicitudEquipoDetFS.DetSolicitud(id);


            return objEquipoDetalle;
        }

        public int ReturnIdEconomico(List<tblM_AsignacionEquipos> objLista, int idSolicitudDEt)
        {
            int value = 0;
            var obj = objLista.Where(x => x.SolicitudDetalleId.Equals(idSolicitudDEt)).FirstOrDefault();
            if (obj != null)
            {
                value = obj.noEconomicoID;
            }
            return value;
        }

        public string returnFechaPromesa(List<tblM_AsignacionEquipos> objLista, int idSolicitudDEt)
        {
            string value = "";
            var obj = objLista.Where(x => x.SolicitudDetalleId.Equals(idSolicitudDEt)).FirstOrDefault();
            if (obj != null)
            {
                value = obj.FechaPromesa.ToShortDateString();
            }
            return value;
        }

        public string getCboEconomicos(int idGrupo, List<tblM_CatMaquina> raw, int idSolicitud)
        {

            var select = "<select class='form-control clsEconomico' data-id='" + idSolicitud + "'>";
            select += "<option value='0'>Seleccione:</option>";
            foreach (var item in raw.Where(x => x.grupoMaquinariaID.Equals(idGrupo)))
            {
                select += "<option value='" + item.id + "'>" + item.noEconomico + "</option>";
            }

            select += "<option value='" + 1 + "'>RENTA</option>";
            select += "<option value='" + 2 + "'>COMPRA</option>";
            select += "<option value='" + 3 + "'>RENTA OPCION COMPRA</option>";

            select += "</select>";

            return select;
        }

        public ActionResult GetListaDetalleSolicitud(int obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<SolicitudEquipoDTO> array = solicitudEquipoFS.getListaDetalleSolicitud(obj);

                var lista = array.Where(x => x.estatus == false).GroupBy(x => new { x.Tipo, x.Grupo, x.Modelo }).ToList();
                var listAdd = array.Where(x => x.estatus == true).ToList();

                var objEconomico = new List<object>();

                foreach (var item in listAdd)
                {

                    string Tipo = "";
                    string Modelo = "";
                    string Grupo = "";

                    string Localizacion = "";
                    string Marca = "";
                    string Serie = "";
                    string nombreCC = "";
                    string Economico = "";
                    string CCOrigen = "";
                    int idEconomico = 0;

                    var DetAsignacion = asignacionEquiposFS.GetAsiganacionBySolicitud(item.id);

                    if (DetAsignacion.estatus < 2)
                    {
                        if (DetAsignacion.noEconomicoID != 0)
                        {
                            var DetMaquina = maquinaFS.GetMaquina(DetAsignacion.noEconomicoID);

                            nombreCC = centroCostosFS.getNombreCC(Convert.ToInt32(DetMaquina.centro_costos));

                            Tipo = DetMaquina.grupoMaquinaria.tipoEquipo.descripcion;
                            Modelo = DetMaquina.modeloEquipo.descripcion;
                            Grupo = DetMaquina.grupoMaquinaria.descripcion;
                            Economico = DetMaquina.noEconomico;
                            Marca = DetMaquina.marca.descripcion;
                            Serie = DetMaquina.noSerie;
                            CCOrigen = DetMaquina.centro_costos;
                            idEconomico = DetAsignacion.noEconomicoID;
                        }
                        else
                        {
                            Tipo = item.Tipo;
                            Grupo = item.Grupo;
                            Modelo = item.Modelo;
                            Economico = DetAsignacion.Economico;
                        }

                        objEconomico.Add(new
                        {
                            pFechaInicio = item.pFechaInicio,
                            pFechaFin = item.pFechaFin,
                            pHoras = item.pHoras,
                            CC = DetAsignacion.cc,
                            pTipoPrioridad = item.pTipoPrioridad,
                            id = item.id,

                            idEconomico = idEconomico,
                            Tipo = Tipo,
                            Modelo = Modelo,
                            Grupo = Grupo,
                            Descripcion = "",
                            localizacion = nombreCC,
                            Economico = Economico,
                            Marca = Marca,
                            Serie = Serie,
                            CCOrigen = CCOrigen,
                            CCDestino = DetAsignacion.cc,
                            idsolicitud = DetAsignacion.solicitudEquipoID,
                            Folio = DetAsignacion.folio,
                            estatus = DetAsignacion.estatus,
                            FechaPromesa = DetAsignacion.FechaPromesa.ToString("dd/MM/yyyy"),
                            Comentario = DetAsignacion.SolicitudDetalleId
                        });
                    }

                }
                result.Add("Agregados", objEconomico);
                result.Add("DetalleSolicitud", array.Where(x => x.estatus == false).ToList());


                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getListaEconomicos(SolicitudEquipoDTO obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                MaquinaFiltrosDTO filtro = new MaquinaFiltrosDTO();
                var listResult = maquinaFS.ListaEquiposSolicitud(obj.Grupoid);
                var data = solicitudEquipoDetFS.DetSolicitud(obj.id);
                var asignacion = asignacionEquiposFS.GetAsiganacionBySolicitud(obj.id);
                int idEconomico = 0;
                if (asignacion != null)
                {
                    idEconomico = asignacion.noEconomicoID;
                }
                var res = authSolicitudesFS.ListaEconomicos(data.grupoMaquinariaID, data.modeloEquipoID, data.tipoMaquinariaID, idEconomico);
                var r = res.Select(y => new InventarioDTO
                {
                    id = y.id,
                    CC = centroCostosFS.getNombreCC(Convert.ToInt32(y.centro_costos)),
                    Economico = y.noEconomico,
                    Grupo = y.grupoMaquinaria == null ? "" : y.grupoMaquinaria.descripcion,
                    Marca = y.marca == null ? "" : y.marca.descripcion,
                    Modelo = y.modeloEquipo == null ? "" : y.modeloEquipo.descripcion,
                    Serie = y.noSerie,
                    Tipo = y.grupoMaquinaria == null ? "" : y.grupoMaquinaria.tipoEquipo.descripcion,
                    idEconomico = y.id,
                    CCOrigen = y.centro_costos

                });

                result.Add("listaEconomicos", r.OrderByDescending(x => x.id));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getListaEconomicosNo(SolicitudEquipoDTO objeto, List<AsignacionDTO> obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var data = solicitudEquipoDetFS.DetSolicitud(objeto.id);

                if (obj != null)
                {
                    var listaEconomicos = obj.Where(x => x.idEconomico != 0).Select(x => x.idEconomico);
                    var res = authSolicitudesFS.ListaEconomicos(data.grupoMaquinariaID, data.modeloEquipoID, data.tipoMaquinariaID, 0);

                    var r = res.Where(x => !listaEconomicos.Contains(x.id) && x.centro_costos != data.SolicitudEquipo.CC.ToString()).Select(y => new InventarioDTO
                    {
                        id = y.id,
                        CC = centroCostosFS.getNombreCCFix(y.centro_costos),
                        Economico = y.noEconomico,
                        Grupo = y.grupoMaquinaria == null ? "" : y.grupoMaquinaria.descripcion,
                        Marca = y.marca == null ? "" : y.marca.descripcion,
                        Modelo = y.modeloEquipo == null ? "" : y.modeloEquipo.descripcion,
                        Serie = y.noSerie,
                        Tipo = y.grupoMaquinaria == null ? "" : y.grupoMaquinaria.tipoEquipo.descripcion,
                        idEconomico = y.id,
                        CCOrigen = y.centro_costos

                    });
                    result.Add("listaEconomicos", r);
                }
                else
                {
                    var res = authSolicitudesFS.ListaEconomicos(data.grupoMaquinariaID, data.modeloEquipoID, data.tipoMaquinariaID, 0);

                    var r = res.Select(y => new InventarioDTO
                    {
                        id = y.id,
                        CC = centroCostosFS.getNombreCCFix(y.centro_costos),
                        Economico = y.noEconomico,
                        Grupo = y.grupoMaquinaria == null ? "" : y.grupoMaquinaria.descripcion,
                        Marca = y.marca == null ? "" : y.marca.descripcion,
                        Modelo = y.modeloEquipo == null ? "" : y.modeloEquipo.descripcion,
                        Serie = y.noSerie,
                        Tipo = y.grupoMaquinaria == null ? "" : y.grupoMaquinaria.tipoEquipo.descripcion,
                        idEconomico = y.id,
                        CCOrigen = y.centro_costos

                    });
                    result.Add("listaEconomicos", r);
                }
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult getDataForReporteAsignacion(List<SolicitudEquipoDTO> array, AutorizadoresIDDTO obj)
        {
            var result = new Dictionary<string, object>();
            try
            {

                Session["rptSolicitudEquipo"] = "";

                var c = base.getUsuario();
                obj.usuarioElaboro = c.id;

                if (array != null)
                {
                    Session["rptAutorizadores"] = obj;
                    Session["rptSolicitudEquipo"] = array;
                    //  Session["rptAsigna"] = c.nombre;
                    Session["rptAsigna"] = "Oscar Manuel Roman Ruiz";

                    result.Add(SUCCESS, true);
                }
                else
                {
                    result.Add(SUCCESS, false);
                }
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetReporteAsignacion(List<AsignacionDTO> obj, int idSolicitud)
        {
            var result = new Dictionary<string, object>();
            try
            {

                var res = authSolicitudesFS.getAutorizadores(idSolicitud);
                var array = obj.ToList().Select(x => new SolicitudEquipoDTO
                {
                    id = 0,
                    Folio = res.folio,
                    TipoId = 0,
                    Grupoid = 0,
                    Modeloid = 0,
                    Tipo = x.Tipo,
                    Grupo = x.Grupo,
                    Modelo = x.Modelo,
                    Descripcion = "",
                    pFechaInicio = x.pFechaInicio,
                    pFechaFin = x.pFechaFin,
                    pHoras = Convert.ToInt32(x.pHoras),
                    pTipoPrioridad = x.pTipoPrioridad,
                    Economico = x.Economico,
                    EconomicoRenta = "",
                    FechaPromesa = x.FechaPromesa
                });

                AutorizadoresIDDTO autorizadores = new AutorizadoresIDDTO
                {
                    altaDireccion = res.altaDireccion,
                    directorDivision = res.directorDivision,
                    gerenteObra = res.gerenteObra,
                    usuarioElaboro = res.usuarioElaboro,
                    GerenteDirector = res.GerenteDirector
                };

                CadenaAutorizacionDTO cadenaAutorizacionDTO = new CadenaAutorizacionDTO
                {
                    CadenaDireccion = res.cadenaFirmaDireccion,
                    CadenaDirector = res.cadenaFirmaDirector,
                    CadenaElabora = res.cadenaFirmaElabora,
                    CadenaGerente = res.cadenaFirmaGerenteObra,
                    CadenaGerenteDirector = res.cadenaFirmaGerenteDirector
                };

                var c = base.getUsuario();
                Session["rptSolicitudEquipo"] = "";

                if (array != null)
                {
                    Session["rptAutorizadores"] = autorizadores;
                    Session["rptSolicitudEquipo"] = array.ToList();
                    if (cadenaAutorizacionDTO != null)
                    {
                        Session["rptCadenaAutorizacion"] = cadenaAutorizacionDTO;
                        Session["rptAsigna"] = "Oscar Manuel Roman Ruiz";//c.nombre;
                    }
                    result.Add(SUCCESS, true);
                }
                else
                {
                    result.Add(SUCCESS, false);
                }
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveOrUpdateAsignacion(List<AsignacionDTO> obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<tblM_AsignacionEquipos> lista = new List<tblM_AsignacionEquipos>();
                List<tblM_SolicitudEquipoDet> listEquiposDet = new List<tblM_SolicitudEquipoDet>();
                int idSolicitud = obj.FirstOrDefault().idsolicitud;


                foreach (var item in obj)
                {

                    var getIdAsiganacion = asignacionEquiposFS.GetAsiganacionBySolicitud(item.id);

                    int idAsiganacion = 0;
                    if (getIdAsiganacion != null)
                    {
                        idAsiganacion = getIdAsiganacion.id;
                    }

                    tblM_AsignacionEquipos dato = new tblM_AsignacionEquipos
                    {
                        id = idAsiganacion,
                        cc = item.CCDestino,
                        CCOrigen = item.CCOrigen == "0" ? "997" : item.CCOrigen,
                        estatus = 1,
                        fechaAsignacion = DateTime.Now,
                        fechaFin = Convert.ToDateTime(item.pFechaFin),
                        fechaInicio = Convert.ToDateTime(item.pFechaInicio),
                        folio = item.folio,
                        Horas = Convert.ToInt32(item.pHoras),
                        noEconomicoID = item.idEconomico,
                        solicitudEquipoID = item.idsolicitud,
                        SolicitudDetalleId = item.id,
                        FechaPromesa = Convert.ToDateTime(item.FechaPromesa),
                        Economico = item.Economico
                    };

                    var det = solicitudEquipoFS.GetSolicitudEquipoDet(item.id);
                    det.estatus = true;
                    listEquiposDet.Add(det);
                    lista.Add(dato);
                }

                solicitudEquipoDetFS.Guardar(listEquiposDet);

                var SolicitudesDetalle = solicitudEquipoFS.getListaDetalleSolicitud(idSolicitud);
                if (SolicitudesDetalle.Count.Equals(0))
                {
                    var solicitud = solicitudEquipoFS.listaSolicitudEquipo(idSolicitud);
                    solicitud.Estatus = true;
                    solicitudEquipoFS.Guardar(solicitud);
                }
                asignacionEquiposFS.SaveOrUpdate(lista);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult LoadSolicitud(string Folio)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var res = solicitudEquipoFS.LoadDataSolicitud(Folio);

                var resDet = solicitudEquipoFS.getListaSolicitudesPendientes(res.id);

                var resGrup = resDet.GroupBy(x => new
                {
                    x.FechaInicio,
                    x.FechaFin,
                    x.idTipoPrioridad
                }).ToList();

                var programa = new List<AsignacionDTO>();
                var id = 1;
                var ListaPrograma = new List<SolicitudDetalleDTO>();
                var ListaTable = new List<SolicitudDetalleDTO>();
                var ListaEconomicosFillCbo = new List<ListaEconomicosFillCboDTO>();

                foreach (var listaPrograma in resGrup)
                {
                    var idTemp = 1;
                    foreach (var detPograma in listaPrograma)
                    {
                        if (!res.ArranqueObra)
                        {
                            ListaTable.Add(new SolicitudDetalleDTO
                            {
                                id = idTemp,
                                idPrincipal = id,
                                TipoId = detPograma.TipoId,
                                GrupoId = detPograma.GrupoId,
                                Modeloid = detPograma.Modeloid,
                                Tipo = detPograma.Tipo,
                                Comentario = detPograma.Comentario == null ? "" : detPograma.Comentario,
                                Grupo = detPograma.Grupo,
                                Modelo = detPograma.Modelo,
                                pHoras = detPograma.pHoras,
                                pFechaInicio = detPograma.pFechaInicio,
                                pFechaFin = detPograma.pFechaFin,
                                pTipoPrioridad = detPograma.TipoPrioriad,
                                idTempSolicitudes = detPograma.id,
                                HorasTotales = listaPrograma.Where(x => x.GrupoId.Equals(detPograma.GrupoId)).Sum(x => x.pHoras),
                                tipoUtilizacion = detPograma.tipoUtilizacion
                            });


                        }
                        else
                        {
                            int idEconomico = 0;
                            string noEconomico = "";

                            var ObjAsignacion = asignacionEquiposFS.GetAsiganacionBySolicitud(detPograma.id);
                            if (ObjAsignacion != null)
                            {

                                if (ObjAsignacion.noEconomicoID != 0)
                                {
                                    var Economico = maquinaFS.GetMaquinaByID(ObjAsignacion.noEconomicoID).FirstOrDefault();

                                    if (Economico != null)
                                    {
                                        idEconomico = Economico.id;
                                        noEconomico = Economico.noEconomico;
                                    }
                                }
                                else
                                {
                                    noEconomico = ObjAsignacion.Economico;
                                    if (noEconomico == "COMPRA")
                                    {
                                        idEconomico = 9999;
                                    }
                                    else
                                    {
                                        idEconomico = 0;
                                    }
                                }


                            }


                            ListaTable.Add(new SolicitudDetalleDTO
                            {
                                id = idTemp,
                                idPrincipal = id,
                                TipoId = detPograma.TipoId,
                                GrupoId = detPograma.GrupoId,
                                Modeloid = detPograma.Modeloid,
                                Tipo = detPograma.Tipo,
                                Comentario = detPograma.Comentario == null ? "" : detPograma.Comentario,
                                Grupo = detPograma.Grupo,
                                Modelo = detPograma.Modelo,
                                pHoras = detPograma.pHoras,
                                pFechaInicio = detPograma.pFechaInicio,
                                pFechaFin = detPograma.pFechaFin,
                                pTipoPrioridad = detPograma.TipoPrioriad,
                                idTempSolicitudes = detPograma.id,
                                HorasTotales = listaPrograma.Where(x => x.GrupoId.Equals(detPograma.GrupoId)).Sum(x => x.pHoras),
                                tipoUtilizacion = detPograma.tipoUtilizacion,
                                idNoEconomico = idEconomico,
                                Economico = noEconomico
                            });

                            var Dato = GetObjetoListaEconomico(id, detPograma.GrupoId, ListaEconomicosFillCbo);
                            if (Dato != null)
                            {
                                ListaEconomicosFillCbo.Add(Dato);
                            }


                        }

                        idTemp += 1;
                    }
                    var objeto = new SolicitudDetalleDTO
                    {
                        id = id,
                        FechaInicio = listaPrograma.Key.FechaInicio,
                        FechaFin = listaPrograma.Key.FechaFin,
                        TipoPrioriad = getDescripcionPrioridad(listaPrograma.Key.idTipoPrioridad)// == 1 ? "URGENTE" : "PROGRAMADA"
                    };
                    id += 1;

                    ListaPrograma.Add(objeto);
                }


                var nombreCentroCostos = centroCostosFS.getNombreCCFix(res.CC);
                var resAuto = authSolicitudesFS.getAutorizadores(res.id);

                var UsuarioElabora = usuarioFS.ListUsersById(resAuto.usuarioElaboro).FirstOrDefault();
                var UsuarioGerente = usuarioFS.ListUsersById(resAuto.gerenteObra).FirstOrDefault();
                var UsuarioGerenteDirector = usuarioFS.ListUsersById(resAuto.GerenteDirector).FirstOrDefault();
                var UsuarioDirector = usuarioFS.ListUsersById(resAuto.directorDivision).FirstOrDefault();
                var UsuarioDireccion = usuarioFS.ListUsersById(resAuto.altaDireccion).FirstOrDefault();
                var UsuarioServicios = usuarioFS.ListUsersById(resAuto.directorServicios).FirstOrDefault();

                var autorizadores = new
                {

                    Elabora = UsuarioElabora.nombre + " " + UsuarioElabora.apellidoPaterno + " " + UsuarioElabora.apellidoMaterno,
                    ElaboraId = UsuarioElabora.id,
                    Gerente = UsuarioGerente.nombre + " " + UsuarioGerente.apellidoPaterno + " " + UsuarioGerente.apellidoMaterno,
                    GerenteId = UsuarioGerente.id,
                    GerenteDirector = UsuarioGerenteDirector.nombre + " " + UsuarioGerenteDirector.apellidoPaterno + " " + UsuarioGerenteDirector.apellidoMaterno,
                    GerenteDirectorId = UsuarioGerenteDirector.id,
                    Director = UsuarioDirector.nombre + " " + UsuarioDirector.apellidoPaterno + " " + UsuarioDirector.apellidoMaterno,
                    DirectorId = UsuarioDirector.id,

                    Direccion = UsuarioDireccion.nombre + " " + UsuarioDireccion.apellidoPaterno + " " + UsuarioDireccion.apellidoMaterno,
                    DireccionId = UsuarioDireccion.id,

                    Servicios = UsuarioServicios.nombre + " " + UsuarioServicios.apellidoPaterno + " " + UsuarioServicios.apellidoMaterno,
                    ServiciosId = UsuarioServicios.id
                };

                result.Add("ListaTable", ListaTable);
                result.Add("ListaPrograma", ListaPrograma.Select(x => new
                {
                    id = x.id,
                    FechaInicio = x.FechaInicio.ToString("dd/MM/yyyy"),
                    FechaFin = x.FechaFin.ToString("dd/MM/yyyy"),
                    TipoPrioridad = x.TipoPrioriad

                }));

                result.Add("ListaEconomicosFillCbo", ListaEconomicosFillCbo);
                result.Add("DatosSolicitud", res);
                result.Add("EsArranque", res.ArranqueObra);
                result.Add("FechaElaboracion", res.fechaElaboracion.ToString("dd/MM/yyyy"));
                result.Add("nombreCentroCostos", nombreCentroCostos);
                result.Add("DatosSolicitudDet", resDet);
                result.Add("DatosAutorizadores", autorizadores);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private ListaEconomicosFillCboDTO GetObjetoListaEconomico(int grupo, int key, List<ListaEconomicosFillCboDTO> ListaFinal)
        {

            ListaEconomicosFillCboDTO objetoReturn = new ListaEconomicosFillCboDTO();

            var data = ListaFinal.Where(X => X.idGrupo.Equals(key));
            if (data.Count() == 0 || data == null)
            {
                List<tblM_CatMaquina> raw = maquinaFS.FillCboEconomicos(key);
                var select = StringCboEconomicos(raw);

                objetoReturn.idGrupo = key;
                objetoReturn.idPrincipal = grupo;
                objetoReturn.stringCombo = select;
                return objetoReturn;
            }
            else
            {
                return null;
            }


        }

        private string getDescripcionPrioridad(int obj)
        {
            switch (obj)
            {
                case 1:
                    return "URGENTE";
                case 0:
                    return "PROGRAMADA";
                case 2:
                    return "NORMAL";
                default:
                    break;
            }
            return "NORMAL";
        }

        public ActionResult DeleteDetSolicitud(int id, int idSolicitud)
        {
            var result = new Dictionary<string, object>();
            try
            {

                solicitudEquipoDetFS.delete(id);

                var getDataDetalle = solicitudEquipoDetFS.listaDetalleSolicitud(idSolicitud);

                if (getDataDetalle.Count == 0)
                {
                    var objSolicitud = solicitudEquipoFS.loadSolicitudById(idSolicitud);
                    objSolicitud.Estatus = true;
                    solicitudEquipoFS.Guardar(objSolicitud);
                }

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboSolicitudes(string CC)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var res = solicitudEquipoDetFS.ListaSolicitudes(CC).Select(x => new { Value = x.folio, Text = x.folio });
                //maquinaFactoryServices.FillCboFiltroGrupoMaquinaria(estatus).Select(x => new { Value = x.id, Text = x.descripcion })
                result.Add(ITEMS, res);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboAutorizadores(string CC, int autorizador)
        {
            var result = new Dictionary<string, object>();
            try
            {
                string centroCotos = CC.ToString().PadLeft(3, '0');
                var res = solicitudEquipoFS.GetAutorizadores(centroCotos).Where(x => x.perfilAutorizaID.Equals(autorizador)).Select(x => new { Value = x.usuarioID, Text = x.usuario.nombre + " " + x.usuario.apellidoPaterno + " " + x.usuario.apellidoMaterno });
                //maquinaFactoryServices.FillCboFiltroGrupoMaquinaria(estatus).Select(x => new { Value = x.id, Text = x.descripcion })


                //   var Autorizadores = solicitudEquipoFactoryServices.GetAutorizadores(centroCotos).Select(x => x.usuario.nombre);
                result.Add(ITEMS, res);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboAutorizadoresArranque(int autorizador)
        {
            var result = new Dictionary<string, object>();
            try
            {
                // string centroCotos = CC.ToString().PadLeft(3, '0');
                var res = solicitudEquipoFS.GetAutorizadores("").Where(x => x.perfilAutorizaID.Equals(autorizador)).Select(x => new { Value = x.usuarioID, Text = x.usuario.nombre + " " + x.usuario.apellidoPaterno + " " + x.usuario.apellidoMaterno }).Distinct();
                //maquinaFactoryServices.FillCboFiltroGrupoMaquinaria(estatus).Select(x => new { Value = x.id, Text = x.descripcion })


                //   var Autorizadores = solicitudEquipoFactoryServices.GetAutorizadores(centroCotos).Select(x => x.usuario.nombre);
                result.Add(ITEMS, res);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CancelacionSolicitudes(int idSolicitud, string descripcion)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var SolicitudObj = solicitudEquipoFS.listaSolicitudEquipo(idSolicitud);

                SolicitudObj.Estatus = true;
                SolicitudObj.cantidad = 0;
                SolicitudObj.descripcion = descripcion;
                solicitudEquipoFS.Guardar(SolicitudObj);
                //     result.Add("Agregados", objEconomico);
                //   result.Add("DetalleSolicitud", array.Where(x => x.estatus == false).ToList());


                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListaEquiposAsignados(string obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var res = asignacionEquiposFS.getAsignacionesByCC(obj).Where(x => x.noEconomicoID != 0);

                var rp = res.Select(x => new
                 {
                     id = x.id,
                     Tipo = maquinaFS.GetMaquinaByID(x.noEconomicoID).FirstOrDefault().grupoMaquinaria.tipoEquipo.descripcion,
                     Grupo = maquinaFS.GetMaquinaByID(x.noEconomicoID).FirstOrDefault().grupoMaquinaria.descripcion,
                     Modelo = maquinaFS.GetMaquinaByID(x.noEconomicoID).FirstOrDefault().modeloEquipo.descripcion,
                     Economico = maquinaFS.GetMaquinaByID(x.noEconomicoID).FirstOrDefault().noEconomico,
                     FechaInicio = DateTime.Now.ToShortDateString(),
                     FechaFin = DateTime.Now.ToShortDateString()
                 });

                result.Add("tbllist", rp);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboSolicitudesCambio(string CC)
        {
            var result = new Dictionary<string, object>();
            try
            {
                //   var res = solicitudEquipoDetFactoryServices.ListaSolicitudes(CC).Select(x => new { Value = x.folio, Text = x.folio });

                //maquinaFactoryServices.FillCboFiltroGrupoMaquinaria(estatus).Select(x => new { Value = x.id, Text = x.descripcion })
                //result.Add(ITEMS, res);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SetRptSustitucion(List<rptSolicitudEquipoReemplazoDTO> obj, string CC, string Folio)
        {
            var result = new Dictionary<string, object>();
            try
            {
                foreach (var item in obj)
                {
                    item.FechaEntrega = DateTime.Now.ToShortDateString();
                }

                Session["rptSolicitudEquipoReemplazoDTO"] = "";

                Session["rptAutorizadores"] = "";

                List<SolicitudEquipoDTO> lista = new List<SolicitudEquipoDTO>();
                AutorizadoresReemplazoDTO autorizadores = new AutorizadoresReemplazoDTO();

                var res = solicitudEquipoFS.GetAutorizadores(CC.ToString()).Where(x => x.perfilAutorizaID.Equals(1));

                string usuarioGerente = "";
                if (res != null)
                {
                    var usuario = res.FirstOrDefault(x => x.perfilAutorizaID.Equals(1)).usuario;
                    usuarioGerente = usuario.nombre + " " + usuario.apellidoPaterno + " " + usuario.apellidoMaterno;

                }

                foreach (var item in obj)
                {
                    SolicitudEquipoDTO objeto = new SolicitudEquipoDTO();

                    var Asignacion = asignacionEquiposFS.GetAsiganacionById(item.id);

                    var Det = solicitudEquipoDetFS.DetSolicitud(Asignacion.SolicitudDetalleId);

                    objeto.Grupo = Det.GrupoMaquinaria.descripcion;
                    objeto.Modelo = Det.ModeloEquipo.descripcion;
                    objeto.Comentario = Det.Comentario;
                    objeto.Descripcion = item.Comentario;
                    objeto.Economico = item.Economico;
                    objeto.pFechaInicio = item.fechaInicio;
                    objeto.pFechaFin = item.fechaFin;
                    objeto.pFechaInicio = item.fechaInicio;
                    objeto.pHoras = Det.horas;



                    lista.Add(objeto);
                }

                autorizadores.nombreElabora = CadenaNombre(getUsuario().id);
                autorizadores.nombreGerente = usuarioGerente;
                autorizadores.nombreasigna = CadenaNombre(4);
                Session["rptCC"] = CC;

                Session["FolioSolReemplazo"] = CC + "-R" + Folio; ;
                Session["rptAutorizadores"] = "";

                Session["rptAutorizadores"] = autorizadores;

                Session["rptSolicitudEquipoReemplazoDTO"] = lista;
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private string CadenaNombre(int id)
        {
            var res = usuarioFS.ListUsersById(id).FirstOrDefault();
            return res.nombre + " " + res.apellidoPaterno;
        }

        public ActionResult SetInfoSave(tblM_SolicitudReemplazoEquipo obj, List<rptSolicitudEquipoReemplazoDTO> listaDetalle)
        {
            var result = new Dictionary<string, object>();
            try
            {
                Session["tblM_SolicitudReemplazoEquipo"] = "";
                Session["tblM_SolicitudReemplazoEquipo"] = obj;

                Session["rptSolicitudEquipoReemplazoDTO"] = "";

                foreach (var item in listaDetalle)
                {
                    item.FechaEntrega = DateTime.Now.ToShortDateString();
                }
                Session["rptSolicitudEquipoReemplazoDTO"] = listaDetalle;



                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GuardarDetalleSolicitudReemplazo()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var idSolicitud = JsonConvert.DeserializeObject<int>(Request.Form["idSolicitud"]);
                var CC = JsonConvert.DeserializeObject<string>(Request.Form["CC"]);


                var RowSolicitud = JsonConvert.DeserializeObject<List<rptSolicitudEquipoReemplazoDTO>>(Request.Form["RowSolicitud"]);
                tblM_AutorizacionSolicitudReemplazo objautorizacion = new tblM_AutorizacionSolicitudReemplazo();

                foreach (string fileName in Request.Files)
                {

                    HttpPostedFileBase file = Request.Files[fileName];
                    BinaryReader b = new BinaryReader(file.InputStream);
                    byte[] binData = b.ReadBytes(file.ContentLength);
                    if (file != null && file.ContentLength > 0)
                    {

                        List<tblM_SolicitudReemplazoDet> listaObjetos = new List<tblM_SolicitudReemplazoDet>();
                        tblM_SolicitudReemplazoDet objeto = new tblM_SolicitudReemplazoDet();
                        foreach (var item in RowSolicitud)
                        {
                            objeto.id = 0;
                            objeto.AsignacionEquiposID = item.id;
                            objeto.SolicitudEquipoReemplazoID = idSolicitud;
                            objeto.Comentario = item.Comentario;
                            objeto.estatus = 0;
                            objeto.fechaInicio = Convert.ToDateTime(item.fechaInicio);
                            objeto.fechaFin = Convert.ToDateTime(item.fechaFin);
                        }
                        solicitudEquipoReemplazoDetFS.GuardarDetalleArchivos(objeto, file);
                    }
                }

                var objAutorizadores = solicitudEquipoFS.GetAutorizadores(CC.ToString());

                var existe = solicitudReemplazoFS.GetSolicitudReemplazobyID(idSolicitud);
                var AutorizantesReemplazo = authSolicitudReemplazoFS.GetAutorizacionReemplazoByID(idSolicitud);

                if (AutorizantesReemplazo == null)
                {

                    var Gerente = objAutorizadores.Where(x => x.perfilAutorizaID.Equals(1));
                    var GerenteID = Gerente.FirstOrDefault().usuarioID;

                    objautorizacion.solicitudReemplazoEquipoID = idSolicitud;
                    objautorizacion.idAutorizaGerente = GerenteID;
                    objautorizacion.idAutorizaElabora = getUsuario().id;
                    objautorizacion.idAutorizaAsigna = 3314;
                    objautorizacion.FechaElaboracion = DateTime.Now;
                    objautorizacion.FechaAutorizacion = DateTime.Now;
                    objautorizacion.id = 0;
                    DateTime fechaNow = DateTime.Now;
                    string fe = fechaNow.ToString("ddMMyyyy") + fechaNow.Hour + "" + fechaNow.Minute;
                    objautorizacion.CadenaElabora = idSolicitud + fe + "" + base.getUsuario().id + "A";
                    objautorizacion.AutorizaElabora = true;

                    authSolicitudReemplazoFS.Guardar(objautorizacion);


                    var AletaVisto = alertasFS.getAlertasByUsuario(getUsuario().id).FirstOrDefault(x => x.objID.Equals(objautorizacion.id));

                    if (AletaVisto != null)
                    {
                        alertasFS.updateAlerta(AletaVisto);
                    }
                    else
                    {

                        tblP_Alerta objAlerta = new tblP_Alerta();

                        string folioSolicitud = "";
                        if (objautorizacion.SolicitudReemplazoEquipo == null)
                        {
                            folioSolicitud = existe.folio;
                        }
                        else
                        {
                            folioSolicitud = objautorizacion.SolicitudReemplazoEquipo.folio;
                        }

                        objAlerta.msj = "Solicitud Pendiente de Autorizar " + folioSolicitud;
                        objAlerta.sistemaID = 1;
                        objAlerta.tipoAlerta = (int)AlertasEnum.REDIRECCION;
                        objAlerta.url = "/SolicitudEquipo/AutorizacionesReemplazo/?Solicitud=" + objautorizacion.id;
                        objAlerta.objID = objautorizacion.id;
                        objAlerta.userEnviaID = objautorizacion.idAutorizaElabora;
                        objAlerta.userRecibeID = objautorizacion.idAutorizaGerente;

                        alertasFS.saveAlerta(objAlerta);

                    }

                    mandarcorreoreemplazo(GerenteID, existe, objautorizacion);
                }
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public void mandarcorreoreemplazo(int GerenteID, tblM_SolicitudReemplazoEquipo existe, tblM_AutorizacionSolicitudReemplazo objautorizacion)
        {
            List<string> correos = new List<string>();

            var getCorreosGerenteMaquinaria = usuarioFS.correoPerfil(8, existe.CC);

            var getCorreosAdminMaquinaria = usuarioFS.correoPerfil(5, existe.CC);

            correos.AddRange(getCorreosGerenteMaquinaria);
            correos.AddRange(getCorreosAdminMaquinaria);


            correos.Add(GetCorreo(getUsuario().id));
            correos.Add(GetCorreo(GerenteID));
            //correos.Add(GetCorreo(4));
            var AsuntoCorreo = @"<html>    <head>
                                    <style>
                                    table {
                                        font-family: arial, sans-serif;
                                        border-collapse: collapse;
                                        width: 100%;
                                    }

                                    td, th {
                                        border: 1px solid #dddddd;
                                        text-align: left;
                                        padding: 8px;
                                    }

                                    tr:nth-child(even) {
                                        background-color: #dddddd;
                                    }
                                    </style>
                                </head>
                                <body>
                                <p>Buen día </p>" +
                  "Se ha realizado una nueva solicitud de reemplazo " + existe.folio + @"</p>
                                <table>
<thead>
                                  <tr>
                                    <th>Nombre Autorizador </th>
                                    <th>Descripción Puesto</th>
                                    <th>Autorizó</th>
                                  </tr></thead>
<tbody>
                                  <tr>
                                    <td>" + getUsuarioNombre(objautorizacion.idAutorizaElabora) + "</td>" +
                "<td>Administrador de Maquinaria</td>" +
                   GetEstatusReemplazo(objautorizacion.AutorizaElabora, objautorizacion.CadenaElabora) +
              "</tr>" +
                                               " <tr>" +
                "<td>" + getUsuarioNombre(objautorizacion.idAutorizaGerente) + "</td>" +
                "<td>Gerente de Obra</td>" +
               GetEstatusReemplazo(objautorizacion.AutorizaGerente, objautorizacion.CadenaGerente) +
              "</tr>" +
                                                "<tr>" +
                "<td>" + getUsuarioNombre(objautorizacion.idAutorizaAsigna) + "</td>" +
                "<td>Director de Maquinaria y Equipo / Subdirector de Maquinaria y Equipo</td>" +
              GetEstatusReemplazo(objautorizacion.AutorizaAsigna, objautorizacion.CadenaAsigna) +
              "</tr>" +

                                                                                 "</tbody>" +
             @"</table>
<p>Mensaje autogenerado por el sistema SIGOPLAN</p>
                                </body>
                                </html>";
            GlobalUtils.sendEmail("Nueva Solicitud Reemplazo : " + existe.folio, AsuntoCorreo, correos);
        }

        public string GetEstatusReemplazo(bool firma, string cadena)
        {

            if (firma)
            {
                return "<td style='background-color: #82E0AA;'>AUTORIZADO</td>";
            }
            else if (!firma && string.IsNullOrEmpty(cadena))
            {
                return "<td style='background-color: #FAE5D3;'>PENDIENTE</td>";
            }
            else
            {
                return "<td style='background-color: #EC7063;'>RECHAZADO</td>";
            }

        }

        private string GetCorreo(int id)
        {
            return usuarioFS.ListUsersById(id).FirstOrDefault().correo;
        }

        public ActionResult SetRptAutorizacionReemplazo(int idSolicitudReemplazo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var data = solicitudReemplazoFS.GetSolicitudReemplazobyID(idSolicitudReemplazo);
                var dataDet = solicitudEquipoReemplazoDetFS.GetSolicitudReemplazoDetByIdSolicitud(idSolicitudReemplazo);
                var dataAutorizadores = authSolicitudReemplazoFS.GetAutorizacionReemplazoByIDReemplazo(idSolicitudReemplazo);

                var lst = new List<SolicitudEquipoDTO>();
                foreach (var item in dataDet)
                {

                    var obj = new SolicitudEquipoDTO()
                    {
                        Comentario = item.Comentario,
                        Economico = maquinaFS.GetMaquinaByID(item.AsignacionEquipos.noEconomicoID).FirstOrDefault().noEconomico,
                        Grupo = maquinaFS.GetMaquinaByID(item.AsignacionEquipos.noEconomicoID).FirstOrDefault().grupoMaquinaria.descripcion,
                        Modelo = maquinaFS.GetMaquinaByID(item.AsignacionEquipos.noEconomicoID).FirstOrDefault().modeloEquipo.descripcion,
                        Tipo = maquinaFS.GetMaquinaByID(item.AsignacionEquipos.noEconomicoID).FirstOrDefault().grupoMaquinaria.tipoEquipo.descripcion,
                        EconomicoRenta = item.SolicitudEquipoReemplazo.id.ToString(),
                        pFechaInicio = item.AsignacionEquipos.fechaInicio.ToShortDateString(),
                        pFechaFin = item.AsignacionEquipos.fechaFin.ToShortDateString(),
                        Descripcion = item.Comentario,
                        pHoras = item.AsignacionEquipos.Horas,
                    };
                    lst.Add(obj);
                }

                Session["rptSolicitudEquipoReemplazoDTO"] = "";
                Session["rptSolicitudEquipoReemplazoDTO"] = lst;
                Session["rptAutorizadores"] = "";

                Session["firmasAutorizadores"] = "";

                AutorizadoresReemplazoDTO autorizadores = new AutorizadoresReemplazoDTO();
                AutorizadoresReemplazoDTO FirmasAutorizadores = new AutorizadoresReemplazoDTO();


                var res = solicitudEquipoFS.GetAutorizadores(data.CC.ToString()).Where(x => x.perfilAutorizaID.Equals(1));

                var res2 = authSolicitudReemplazoFS.getAutorizadores(idSolicitudReemplazo);

                string usuarioGerente = "";
                if (res != null)
                {
                    var usuario = res.FirstOrDefault(x => x.perfilAutorizaID.Equals(1)).usuario;
                    usuarioGerente = usuario.nombre + " " + usuario.apellidoPaterno + " " + usuario.apellidoMaterno;
                }
                string nombreElabora = CadenaNombre(res2.idAutorizaElabora);

                if (dataAutorizadores != null)
                {
                    FirmasAutorizadores.nombreasigna = dataAutorizadores.CadenaAsigna;
                    FirmasAutorizadores.nombreElabora = dataAutorizadores.CadenaElabora;
                    FirmasAutorizadores.nombreGerente = dataAutorizadores.CadenaGerente;
                }
                else
                {
                    FirmasAutorizadores.nombreasigna = "";
                    FirmasAutorizadores.nombreElabora = "";
                    FirmasAutorizadores.nombreGerente = "";
                }
                autorizadores.nombreElabora = CadenaNombre(res2.idAutorizaElabora);
                autorizadores.nombreGerente = CadenaNombre(res2.idAutorizaGerente);
                autorizadores.nombreasigna = CadenaNombre(res2.idAutorizaAsigna);
                Session["rptCC"] = data.CC;
                Session["rptAutorizadores"] = "";
                Session["rptAutorizadores"] = autorizadores;
                Session["firmasAutorizadores"] = FirmasAutorizadores;
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SetRptAutorizacionReemplazo2(int idSolicitudReemplazo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<rptSolicitudEquipoReemplazoDTO> obj = new List<rptSolicitudEquipoReemplazoDTO>();
                var data = solicitudReemplazoFS.GetSolicitudReemplazobyID(idSolicitudReemplazo);
                var dataDet = solicitudEquipoReemplazoDetFS.GetSolicitudReemplazoDetByIdSolicitud(idSolicitudReemplazo);

                obj = dataDet.Select(x => new rptSolicitudEquipoReemplazoDTO
                {
                    Comentario = x.Comentario,
                    Economico = maquinaFS.GetMaquinaByID(x.AsignacionEquipos.noEconomicoID).FirstOrDefault().noEconomico,
                    FechaEntrega = DateTime.Now.ToShortDateString(),
                    Grupo = maquinaFS.GetMaquinaByID(x.AsignacionEquipos.noEconomicoID).FirstOrDefault().grupoMaquinaria.descripcion,
                    Modelo = maquinaFS.GetMaquinaByID(x.AsignacionEquipos.noEconomicoID).FirstOrDefault().modeloEquipo.descripcion,
                    Tipo = maquinaFS.GetMaquinaByID(x.AsignacionEquipos.noEconomicoID).FirstOrDefault().grupoMaquinaria.tipoEquipo.descripcion,

                }).ToList();

                Session["rptSolicitudEquipoReemplazoDTO"] = "";
                Session["rptSolicitudEquipoReemplazoDTO"] = obj;
                Session["rptAutorizadores"] = "";
                AutorizadoresReemplazoDTO autorizadores = new AutorizadoresReemplazoDTO();

                var res = solicitudEquipoFS.GetAutorizadores(data.CC.ToString()).Where(x => x.perfilAutorizaID.Equals(1));

                string usuarioGerente = "";
                if (res != null)
                {
                    var usuario = res.FirstOrDefault(x => x.perfilAutorizaID.Equals(1)).usuario;
                    usuarioGerente = usuario.nombre + " " + usuario.apellidoPaterno + " " + usuario.apellidoMaterno;
                }

                autorizadores.nombreElabora = CadenaNombre(getUsuario().id);
                autorizadores.nombreGerente = usuarioGerente;
                autorizadores.nombreasigna = CadenaNombre(4);
                Session["rptCC"] = data.CC;
                Session["rptAutorizadores"] = "";
                Session["rptAutorizadores"] = autorizadores;
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveSolicitudReemplazo(tblM_SolicitudReemplazoEquipo obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                obj.usuarioID = getUsuario().id;
                obj.fechaElaboracion = DateTime.Now;

                obj.folio = obj.CC + "-R" + obj.folio;
                solicitudReemplazoFS.Guardar(obj);

                result.Add("idSolicitud", obj.id);
                result.Add("folioSolicitud", obj.folio);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /*Guardado de Asignacion Parcial*/

        public ActionResult GuardarParcialAsignaciones(List<AsignacionParcialDTO> objParcial, int solicitudID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                string Economico = "";
                string centroCotos = "";
                var listAsignaciones = asignacionEquiposFS.getAsignacionesByID(solicitudID);
                tblM_AsignacionEquipos objSolictudesDetP = new tblM_AsignacionEquipos();
                foreach (AsignacionParcialDTO itemParcial in objParcial)
                {

                    tblM_SolicitudEquipoDet objSolicitudesDet = new tblM_SolicitudEquipoDet();
                    objSolictudesDetP = listAsignaciones.FirstOrDefault(x => x.SolicitudDetalleId.Equals(itemParcial.id));
                    var objMaquina = maquinaFS.GetMaquina(itemParcial.idNoEconomico);

                    if (objMaquina != null)
                    {
                        Economico = objMaquina.noEconomico;
                        centroCotos = objMaquina.centro_costos;
                    }
                    else
                    {
                        switch (itemParcial.idNoEconomico)
                        {
                            case 1:
                                Economico = "RENTA";
                                break;
                            case 2:
                                Economico = "COMPRA";
                                break;
                            case 3:
                                Economico = "RENTA COMPRA";
                                break;
                            default:
                                break;
                        }
                    }

                    if (objSolictudesDetP == null)
                    {
                        objSolicitudesDet = solicitudEquipoDetFS.DetSolicitud(itemParcial.id);
                        tblM_AsignacionEquipos objSaveInfo = new tblM_AsignacionEquipos();
                        objSaveInfo.id = 0;
                        objSaveInfo.solicitudEquipoID = solicitudID;
                        objSaveInfo.folio = objSolicitudesDet.folio;
                        objSaveInfo.noEconomicoID = itemParcial.idNoEconomico;
                        objSaveInfo.fechaInicio = objSolicitudesDet.fechaInicio;
                        objSaveInfo.fechaFin = objSolicitudesDet.fechaFin;
                        objSaveInfo.Horas = objSolicitudesDet.horas;
                        objSaveInfo.estatus = 0;
                        objSaveInfo.fechaAsignacion = DateTime.Now;
                        objSaveInfo.cc = objSolicitudesDet.SolicitudEquipo.CC;
                        objSaveInfo.CCOrigen = centroCotos;
                        objSaveInfo.SolicitudDetalleId = itemParcial.id;
                        objSaveInfo.FechaPromesa = itemParcial.pFechaObra;
                        objSaveInfo.Economico = Economico;
                        asignacionEquiposFS.SaveOrUpdate(objSaveInfo);
                    }
                    else
                    {
                        objSolictudesDetP.noEconomicoID = itemParcial.idNoEconomico;
                        objSolictudesDetP.FechaPromesa = itemParcial.pFechaObra;
                        objSolictudesDetP.Economico = Economico;
                        asignacionEquiposFS.SaveOrUpdate(objSolictudesDetP);
                    }
                }
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //Guardado de Asignaciones Autorizadas.
        public ActionResult GuardarAsignaciones(List<AsignacionParcialDTO> objAsignados, int solicitudID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                string Economico = "";
                string centroCostosMaquina = "";
                bool isCompra = false;

                var listAsignaciones = asignacionEquiposFS.getAsignacionesByID(solicitudID);
                var flagTipo = false;

                foreach (AsignacionParcialDTO itemParcial in objAsignados)
                {
                    List<CorreoCRDTO> CorreosCR = new List<CorreoCRDTO>();
                    List<string> CorresUsuario = new List<string>();
                    var folioRemp = "";
                    var ccRemp = "";
                    if (itemParcial.tipoAsignacion == 2)
                    {
                        flagTipo = true;

                        var solicitudDetalleobj = solicitudEquipoReemplazoDetFS.GetSolicitudReemplazoDetByIdSolicitud(solicitudID).Where(x => x.estatus == 0);

                        var objtemp = solicitudDetalleobj.FirstOrDefault(x => x.AsignacionEquipos.SolicitudDetalleId == itemParcial.id);
                        if (objtemp != null)
                        {
                            solicitudID = solicitudDetalleobj.FirstOrDefault().AsignacionEquipos.solicitudEquipoID;
                            var ObtenerData = solicitudEquipoReemplazoDetFS.GetSolicitudReemplazoDetById(objtemp.id);
                            ObtenerData.estatus = 1;

                            solicitudEquipoReemplazoDetFS.GuardarSingle(ObtenerData);
                            folioRemp = ObtenerData.SolicitudEquipoReemplazo.folio;
                            ccRemp = ObtenerData.SolicitudEquipoReemplazo.CC;
                            
                        }
                    }
                    string centro_costos = "";
                    tblM_SolicitudEquipoDet objSolicitudesDet = new tblM_SolicitudEquipoDet();

                    var objSolictudesDetP = listAsignaciones.FirstOrDefault(x => x.SolicitudDetalleId.Equals(itemParcial.id));
                    var objDetsolicitud = solicitudEquipoDetFS.DetSolicitud(itemParcial.id);
                    var objMaquina = maquinaFS.GetMaquina(itemParcial.idNoEconomico);

                    if (objMaquina != null)
                    {
                        Economico = objMaquina.noEconomico;
                        centroCostosMaquina = objMaquina.centro_costos;
                    }
                    else
                    {
                        switch (itemParcial.idNoEconomico)
                        {
                            case 1:
                                Economico = "RENTA";
                                break;
                            case 2:
                                Economico = "COMPRA";
                                break;
                            case 3:
                                Economico = "RENTA COMPRA";
                                break;
                            default:

                                break;
                        }
                        itemParcial.idNoEconomico = 0;

                        CorresUsuario.Add(GetCorreo(1049));//Javier Elias
                        CorresUsuario.Add(GetCorreo(1050));//Cesar Vega
                        if (objDetsolicitud.tipoMaquinariaID == 2)
                        {
                            CorresUsuario.Add(GetCorreo(1175));//Victor Esquer
                        }

                        isCompra = true;

                    }
                    if (objSolictudesDetP == null || itemParcial.tipoAsignacion == 2)
                    {
                        objSolicitudesDet = solicitudEquipoDetFS.DetSolicitud(itemParcial.id);
                        tblM_AsignacionEquipos objSaveInfo = new tblM_AsignacionEquipos();
                        objSaveInfo.id = 0;
                        objSaveInfo.solicitudEquipoID = solicitudID;
                        objSaveInfo.folio = objSolicitudesDet.folio;
                        objSaveInfo.noEconomicoID = itemParcial.idNoEconomico;
                        objSaveInfo.fechaInicio = itemParcial.pFechaInicio;
                        objSaveInfo.fechaFin = itemParcial.pFechaFin;
                        objSaveInfo.Horas = objSolicitudesDet.horas;
                        objSaveInfo.estatus = 1;
                        objSaveInfo.fechaAsignacion = DateTime.Now;
                        objSaveInfo.cc = objSolicitudesDet.SolicitudEquipo.CC;
                        objSaveInfo.CCOrigen = centroCostosMaquina;
                        objSaveInfo.SolicitudDetalleId = itemParcial.id;
                        objSaveInfo.FechaPromesa = itemParcial.pFechaObra;
                        objSaveInfo.Economico = Economico;
                        asignacionEquiposFS.SaveOrUpdate(objSaveInfo);


                        centro_costos = objSaveInfo.cc;
                        objSolicitudesDet.estatus = true;
                        solicitudEquipoDetFS.Guardar(objSolicitudesDet);

                        var responsableEnvio = usuarioFS.getPerfilesUsuario(1, objSaveInfo.CCOrigen);
                        var responsableRecepcion = usuarioFS.getPerfilesUsuario(1, objSaveInfo.cc);

                        foreach (var item in responsableEnvio)
                        {
                            CorresUsuario.Add(GetCorreo(item.usuarioID));
                        }

                        foreach (var item in responsableRecepcion)
                        {
                            CorresUsuario.Add(GetCorreo(item.usuarioID));
                        }

                        CorreoCRDTO data = new CorreoCRDTO();

                        data.Economico = Economico;
                        data.tipo = objSolicitudesDet.TipoMaquinaria.descripcion;
                        data.descripcion = objSolicitudesDet.GrupoMaquinaria.descripcion;
                        data.modelo = objSolicitudesDet.ModeloEquipo.descripcion;
                        data.solicitud = objSolicitudesDet.folio;
                        data.fechaObra = objSaveInfo.FechaPromesa.ToShortDateString();

                        CorresUsuario.Add(GetCorreo(1123));
                        //CorresUsuario.Add(GetCorreo(4));
                        CorresUsuario.Add(GetCorreo(1126));

                        CorreosCR.Add(data);
                        MandarCorreoAsigNormal(solicitudID, folioRemp, true, "", ccRemp, Economico, centroCostosMaquina, objSaveInfo.fechaAsignacion.ToString("dd/MM/yyyy"));

                    }
                    else
                    {
                        if (objSolictudesDetP.estatus == 0)
                        {
                            objSolictudesDetP.estatus = 1;
                            objSolictudesDetP.Economico = Economico;
                            objSolictudesDetP.noEconomicoID = itemParcial.idNoEconomico;
                            objSolictudesDetP.fechaInicio = itemParcial.pFechaInicio;
                            objSolictudesDetP.fechaFin = itemParcial.pFechaFin;
                            objSolictudesDetP.FechaPromesa = itemParcial.pFechaObra;
                            asignacionEquiposFS.SaveOrUpdate(objSolictudesDetP);

                            objSolicitudesDet = solicitudEquipoDetFS.DetSolicitud(itemParcial.id);
                            objSolicitudesDet.estatus = true;
                            solicitudEquipoDetFS.Guardar(objSolicitudesDet);
                        }
                        else
                        {
                            objSolicitudesDet = solicitudEquipoDetFS.DetSolicitud(itemParcial.id);


                            tblM_AsignacionEquipos objSaveInfo = new tblM_AsignacionEquipos();

                            objSaveInfo.id = 0;
                            objSaveInfo.solicitudEquipoID = solicitudID;
                            objSaveInfo.folio = objSolicitudesDet.folio;
                            objSaveInfo.noEconomicoID = itemParcial.idNoEconomico;
                            objSaveInfo.fechaInicio = itemParcial.pFechaInicio;
                            objSaveInfo.fechaFin = itemParcial.pFechaFin;
                            objSaveInfo.Horas = objSolicitudesDet.horas;
                            objSaveInfo.estatus = 1;
                            objSaveInfo.fechaAsignacion = DateTime.Now;
                            objSaveInfo.cc = objSolicitudesDet.SolicitudEquipo.CC;
                            objSaveInfo.CCOrigen = centroCostosMaquina;
                            objSaveInfo.SolicitudDetalleId = itemParcial.id;
                            objSaveInfo.FechaPromesa = itemParcial.pFechaObra;
                            objSaveInfo.Economico = Economico;
                            asignacionEquiposFS.SaveOrUpdate(objSaveInfo);

                            objSolicitudesDet.estatus = true;
                            solicitudEquipoDetFS.Guardar(objSolicitudesDet);

                            var responsableEnvio = usuarioFS.getPerfilesUsuario(1, objSaveInfo.CCOrigen);
                            var responsableRecepcion = usuarioFS.getPerfilesUsuario(1, objSaveInfo.cc);

                            centro_costos = objSaveInfo.cc;

                            foreach (var item in responsableEnvio)
                            {
                                CorresUsuario.Add(GetCorreo(item.usuarioID));
                            }
                            foreach (var item in responsableRecepcion)
                            {
                                CorresUsuario.Add(GetCorreo(item.usuarioID));
                            }


                            CorreoCRDTO data = new CorreoCRDTO();

                            data.Economico = Economico;
                            data.tipo = objSolicitudesDet.TipoMaquinaria.descripcion;
                            data.descripcion = objSolicitudesDet.GrupoMaquinaria.descripcion;
                            data.modelo = objSolicitudesDet.ModeloEquipo.descripcion;
                            data.solicitud = objSolicitudesDet.folio;
                            data.fechaObra = objSaveInfo.FechaPromesa.ToShortDateString();
                            CorreosCR.Add(data);

                            MandarCorreoAsigNormal(solicitudID, objSolicitudesDet.folio, true, "", centro_costos, Economico, centroCostosMaquina, objSaveInfo.fechaAsignacion.ToString("dd/MM/yyyy"));

                        }
                    }
                    var vista = vSesiones.sesionCurrentView;

                    var RawCorreos = envioCorreosFS.GetListaCorreos(vSesiones.sesionCurrentView, centro_costos);

                    foreach (var item in RawCorreos)
                    {
                        CorresUsuario.Add(GetCorreo(item.usuarioID));
                    }
                    var Gerardo = CorresUsuario.FirstOrDefault(x => x.Equals("g.reina@construplan.com.mx"));
                    if (Gerardo != null)
                    {
                        CorresUsuario.Remove(Gerardo);
                    }
                    if (isCompra)
                    {
                        enviarCorreoCompraRenta(CorreosCR, CorresUsuario, "Nueva solicitud de equipo para Compra/Renta : ");
                    }
                    else
                    {
                        enviarCorreoCompraRenta(CorreosCR, CorresUsuario, "Nueva Asignación de equipo : ");
                    }
                }


                if (!flagTipo)
                {
                    var listaDetalle = solicitudEquipoDetFS.listaDetalleSolicitud(solicitudID);

                    var ContunDetalle = listaDetalle.Where(x => x.estatus == false).Count();

                    if (ContunDetalle == 0)
                    {
                        var objSolicitud = solicitudEquipoFS.loadSolicitudById(solicitudID);

                        objSolicitud.Estatus = true;
                        solicitudEquipoFS.Guardar(objSolicitud);
                    }
                }

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarAsignacion(int objDetalleID, int solicitudID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                string Economico = "";
                string centroCostosMaquina = "";
                var listAsignaciones = asignacionEquiposFS.getAsignacionesByID(solicitudID);

                var objAsignacion = listAsignaciones.OrderByDescending(x => x.id).FirstOrDefault(x => objDetalleID.Equals(x.SolicitudDetalleId));

                if (objAsignacion != null)
                {
                    if (objAsignacion.estatus == 1 || objAsignacion.noEconomicoID == 0)
                    {
                        asignacionEquiposFS.delete(objAsignacion);
                        var objSoldet = solicitudEquipoDetFS.DetSolicitud(objDetalleID);
                        objSoldet.estatus = false;

                        solicitudEquipoDetFS.Guardar(objSoldet);
                        var listaDetalle = solicitudEquipoDetFS.listaDetalleSolicitud(solicitudID);

                        var ContunDetalle = listaDetalle.Where(x => x.estatus == false).Count();

                        if (ContunDetalle > 0)
                        {
                            var objSolicitud = solicitudEquipoFS.loadSolicitudById(solicitudID);

                            objSolicitud.Estatus = false;
                            solicitudEquipoFS.Guardar(objSolicitud);
                        }
                        else
                        {
                            var objSolicitud = solicitudEquipoFS.loadSolicitudById(solicitudID);

                            objSolicitud.Estatus = true;
                            solicitudEquipoFS.Guardar(objSolicitud);
                        }

                        result.Add(SUCCESS, true);
                    }
                }
                else
                {
                    result.Add(SUCCESS, false);
                }


            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarRegistroSolicitud(int objDetalleID, int solicitudID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                string Economico = "";
                string centroCostosMaquina = "";
                var listAsignaciones = asignacionEquiposFS.getAsignacionesByID(solicitudID);

                var objAsignacion = listAsignaciones.FirstOrDefault(x => objDetalleID.Equals(x.SolicitudDetalleId));

                if (objAsignacion != null)
                {
                    asignacionEquiposFS.delete(objAsignacion);
                    solicitudEquipoDetFS.delete(objDetalleID);

                    var countData = solicitudEquipoDetFS.listaDetalleSolicitud(solicitudID).Count();

                    var objSolicitud = solicitudEquipoFS.loadSolicitudById(solicitudID);

                    objSolicitud.cantidad = countData;
                    solicitudEquipoFS.Guardar(objSolicitud);
                }
                else
                {
                    solicitudEquipoDetFS.delete(objDetalleID);

                    var countData = solicitudEquipoDetFS.listaDetalleSolicitud(solicitudID).Count();

                    var objSolicitud = solicitudEquipoFS.loadSolicitudById(solicitudID);

                    objSolicitud.cantidad = countData;
                    solicitudEquipoFS.Guardar(objSolicitud);
                }

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public void enviarCorreoCompraRenta(List<CorreoCRDTO> lista, List<string> CorreosUsuarios, string anexoAdjunto)
        {
            List<string> correos = new List<string>();


            correos.AddRange(CorreosUsuarios);
            //correos.Add(   GetCorreo(1049));//Javier Elias
            //correos.Add(GetCorreo(1050));//Cesar Vega
            var AsuntoCorreo = @"<html><head>
                                <style>
                                    table {
                                        font-family: arial, sans-serif;
                                        border-collapse: collapse;
                                        width: 100%;
                                    }

                                    td, th {
                                        border: 1px solid #dddddd;
                                        text-align: left;
                                        padding: 8px;
                                    }

                                    tr:nth-child(even) {
                                        background-color: #dddddd;
                                    }
                                </style>
                                </head>
                                <body>
                                    <p>Buen día </p><br/>
                                    <p>Se ha realizado una nueva solicitud una nueva Asignación</p><br/>
                                    <table>
                                        <thead>
                                            <tr>
                                                <th>Solicitud</th>
                                                <th>Tipo Solicitud</th>
                                                <th>Descripcion</th>
                                                <th>Modelo</th>
                                                <th>no Economico</th>
                                                <th>Fecha en Obra</th>
                                            </tr>
                                        </thead>
                                        <tbody>";
            foreach (var i in lista)
            {
                AsuntoCorreo += @"<tr>
                                                <td>" + i.solicitud + @"</td>
                                                <td>" + i.tipo + @"</td>
                                                <td>" + i.descripcion + @"</td>
                                                <td>" + i.modelo + @"</td>
                                                <td>" + i.Economico + @"</td>
                                                <td>" + i.fechaObra + @"</td>
                                        </tr>";
            }
            AsuntoCorreo += "</tbody>" +
                        @"</table><br/>
                                    <p>Mensaje autogenerado por el sistema SIGOPLAN</p>
                                </body>
                                </html>";
            GlobalUtils.sendEmail(anexoAdjunto, AsuntoCorreo, correos);
        }

        public ActionResult GetReportePorEmpresa(int obj, int empresa)
        {
            var result = new Dictionary<string, object>();
            try
            {

                Session["rptCadenaAutorizacion"] = null;
                var objSolicitud = solicitudEquipoFS.listaSolicitudEquipoPorEmpresa(obj, empresa);
                var CC = objSolicitud.CC;

                if (objSolicitud != null)
                {

                    if (!objSolicitud.ArranqueObra)
                    {

                        List<SolicitudEquipoDTO> array = solicitudEquipoFS.getListaDetalleSolicitudAutorizacionPorEmpresa(obj, empresa);

                        var res = solicitudEquipoFS.getAutorizadoresPorEmpresa(obj, empresa);

                        if (res != null)
                        {
                            CadenaAutorizacionDTO cadenaAutorizacionDTO = new CadenaAutorizacionDTO
                            {
                                CadenaDireccion = res.cadenaFirmaDireccion,
                                CadenaDirector = res.cadenaFirmaDirector,
                                CadenaElabora = res.cadenaFirmaElabora,
                                CadenaGerente = res.cadenaFirmaGerenteObra,
                                CadenaGerenteDirector = res.cadenaFirmaGerenteDirector,
                                CadenaServicios = res.cadenaFirmaServicios
                            };

                            AutorizadoresIDDTO autorizadores = new AutorizadoresIDDTO
                            {
                                altaDireccion = res.altaDireccion,
                                directorDivision = res.directorDivision,
                                gerenteObra = res.gerenteObra,
                                usuarioElaboro = res.usuarioElaboro,
                                GerenteDirector = res.GerenteDirector,
                                directorServicios = res.directorServicios
                            };

                            Session["rptSolicitudEquipo"] = null;

                            if (array != null)
                            {
                                Session["rptAutorizadores"] = autorizadores;
                                Session["rptSolicitudEquipo"] = array;
                                if (cadenaAutorizacionDTO != null)
                                {
                                    Session["rptCadenaAutorizacion"] = cadenaAutorizacionDTO;

                                }

                                var idReporte = 30;
                                result.Add("idReporte", idReporte);
                                result.Add("CC", CC);
                                result.Add(SUCCESS, true);
                            }
                            else
                            {
                                result.Add(SUCCESS, false);
                            }
                        }
                    }
                    else
                    {
                        var res = solicitudEquipoFS.getAutorizadoresPorEmpresa(obj, empresa);
                        var getDetalle = solicitudEquipoFS.listaDetalleSolicitudPorEmpresa(obj, empresa).ToList();

                        var Asginacion = solicitudEquipoFS.getAsignacionesByIDPorEmpresa(obj, empresa).ToList();

                        var array = Asginacion.ToList().Select(x => new SolicitudEquipoDTO
                        {
                            id = 0,
                            Folio = res.folio,
                            TipoId = 0,
                            Grupoid = 0,
                            Modeloid = 0,
                            Tipo = getDetalle.FirstOrDefault(d => d.id.Equals(x.SolicitudDetalleId)) != null ? getDetalle.FirstOrDefault(d => d.id.Equals(x.SolicitudDetalleId)).TipoMaquinaria.descripcion : "",
                            Grupo = getDetalle.FirstOrDefault(d => d.id.Equals(x.SolicitudDetalleId)) != null ? getDetalle.FirstOrDefault(d => d.id.Equals(x.SolicitudDetalleId)).GrupoMaquinaria.descripcion : "",
                            Modelo = getDetalle.FirstOrDefault(d => d.id.Equals(x.SolicitudDetalleId)) != null ? getDetalle.FirstOrDefault(d => d.id.Equals(x.SolicitudDetalleId)).ModeloEquipo.descripcion : "",
                            Descripcion = getDetalle.FirstOrDefault(d => d.id.Equals(x.SolicitudDetalleId)) != null ? getDetalle.FirstOrDefault(d => d.id.Equals(x.SolicitudDetalleId)).Comentario : "",
                            pFechaInicio = x.fechaInicio.ToShortDateString(),
                            pFechaFin = x.fechaFin.ToShortDateString(),
                            pHoras = Convert.ToInt32(x.Horas),
                            pTipoPrioridad = getDetalle.FirstOrDefault(d => d.id.Equals(x.SolicitudDetalleId)) != null ? (getDetalle.FirstOrDefault(d => d.id.Equals(x.SolicitudDetalleId)).prioridad == 0 ? "A" : getDetalle.FirstOrDefault(d => d.id.Equals(x.SolicitudDetalleId)).prioridad == 2 ? "B" : "C") : "C",
                            Economico = x.Economico,
                            EconomicoRenta = "",
                            FechaPromesa = x.FechaPromesa.ToShortDateString()
                        }).ToList();

                        AutorizadoresIDDTO autorizadores = new AutorizadoresIDDTO
                        {
                            altaDireccion = res.altaDireccion,
                            directorDivision = res.directorDivision,
                            gerenteObra = res.gerenteObra,
                            usuarioElaboro = res.usuarioElaboro,
                            GerenteDirector = res.GerenteDirector,
                            directorServicios = res.directorServicios
                        };

                        CadenaAutorizacionDTO cadenaAutorizacionDTO = new CadenaAutorizacionDTO
                        {
                            CadenaDireccion = res.cadenaFirmaDireccion,
                            CadenaDirector = res.cadenaFirmaDirector,
                            CadenaElabora = res.cadenaFirmaElabora,
                            CadenaGerente = res.cadenaFirmaGerenteObra,
                            CadenaGerenteDirector = res.cadenaFirmaGerenteDirector,
                            CadenaServicios = res.cadenaFirmaServicios
                        };

                        var c = base.getUsuario();
                        Session["rptSolicitudEquipo"] = null;

                        if (array != null)
                        {
                            var idReporte = 12;
                            if (cadenaAutorizacionDTO != null)
                            {
                                Session["rptCadenaAutorizacion"] = cadenaAutorizacionDTO;
                                Session["rptAsigna"] = "Oscar Manuel Roman Ruiz";
                            }
                            Session["rptAutorizadores"] = autorizadores;
                            Session["rptSolicitudEquipo"] = array;
                            result.Add("idReporte", idReporte);
                            result.Add("CC", CC);
                            result.Add(SUCCESS, true);
                        }
                        else
                        {
                            result.Add(SUCCESS, false);
                        }
                    }
                }
                else
                {
                    var idReporte = 30;
                    result.Add("idReporte", idReporte);
                    result.Add("CC", idReporte);
                    result.Add(SUCCESS, false);
                }
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

         [HttpGet]
        public ActionResult getAsginaciones(int economicoID)
        {
          
            return Json(solicitudEquipoFS.getAsginaciones(economicoID), JsonRequestBehavior.AllowGet);
        }
         public ActionResult GetAutorizadoresAC()
         {
             return Json(solicitudEquipoFS.GetAutorizadoresAC(), JsonRequestBehavior.AllowGet);
         }
         public void SetAutorizadorAC(int usarioID, string ac,int perfil)
         {
             solicitudEquipoFS.SetAutorizadorAC(usarioID, ac, perfil);
         }
    }
}