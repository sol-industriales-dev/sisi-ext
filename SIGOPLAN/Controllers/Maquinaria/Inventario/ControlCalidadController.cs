using Core.DTO;
using Core.DTO.Maquinaria.Captura;
using Core.DTO.Maquinaria.Inventario;
using Core.DTO.Maquinaria.Inventario.Controles;
using Core.DTO.Utils;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Maquinaria.Inventario;
using Core.Entity.Maquinaria.Inventario.ControlCalidad;
using Core.Entity.Principal.Alertas;
using Core.Entity.Principal.Usuarios;
using Core.Enum.Principal.Alertas;
using Data.Factory.Maquinaria.Captura;
using Data.Factory.Maquinaria.Catalogos;
using Data.Factory.Maquinaria.Inventario;
using Data.Factory.Maquinaria.Inventario.ControlCalidad;
using Data.Factory.Maquinaria.Reporte.ActivoFijo;
using Data.Factory.Principal.Alertas;
using Data.Factory.Principal.Archivos;
using Data.Factory.Principal.Usuarios;
using Infrastructure.Utils;
using Newtonsoft.Json;
using Reportes.Reports.Inventario;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Controllers.Calidad
{
    public class ControlCalidadController : BaseController
    {
        #region Factory
        private ResguardoEquipoFactoryServices resguardoEquipoFactoryServices = new ResguardoEquipoFactoryServices();
        private ControlEnvioyRecepcionFactoryServices controlEnvioyRecepcionFactoryService = new ControlEnvioyRecepcionFactoryServices();
        private CentroCostosFactoryServices centroCostosFactoryServices = new CentroCostosFactoryServices();
        private MaquinaFactoryServices maquinaFactoryServices = new MaquinaFactoryServices();
        private CapturaHorometroFactoryServices horometroServices = new CapturaHorometroFactoryServices();
        private CentroCostosFactoryServices CCs = new CentroCostosFactoryServices();
        private GrupoPreguntasFactoryServices GrupoPreguntasCalidad = new GrupoPreguntasFactoryServices();
        private PreguntasCalidadFactoryServices PreguntasCalidad = new PreguntasCalidadFactoryServices();
        private ControlCalidadFactoryServices ControlCalidadService = new ControlCalidadFactoryServices();
        private RespuestasCalidadFactoryServices RespuestasCalidadService = new RespuestasCalidadFactoryServices();
        private AsignacionEquiposFactoryServices asignacionEquiposFactoryServices = new AsignacionEquiposFactoryServices();
        private SolicitudEquipoFactoryServices solicitudEquipoFactoryServices = new SolicitudEquipoFactoryServices();
        private SolicitudEquipoDetFactoryServices solicitudEquipoDetFactoryServices = new SolicitudEquipoDetFactoryServices();
        private UsuarioFactoryServices usuarioFactoryServices = new UsuarioFactoryServices();
        private AlertaFactoryServices alertasFactoryServices = new AlertaFactoryServices();
        private ArchivoFactoryServices archivoFs = new ArchivoFactoryServices();
        private ActivoFijoFactoryServices afFS = new ActivoFijoFactoryServices();
        #endregion

        private List<string> pathTempArchivos = new List<string>();

        // GET: ControlCalidad
        public ActionResult Index()
        {
            ViewBag.pagina = "Control de Calidad";
            ViewBag.PolizasCapturadas = afFS.getActivoFijoServices().PolizasCapturadas();
            return View();
        }
        tblM_CatMaquina maquinatemp = new tblM_CatMaquina();
        //Obj  = Tipo de Documento
        //Filtro = Equipos Pendientes de hacer
        //Filtro = Equipos Con Controles ya hechos.

        public ActionResult GetMaquinariasPendientesEnvios(int obj, int tipoFiltro, DateTime? fechaInicio, DateTime? fechaFin, string cc, int? numEconomico)
        {
            var result = new Dictionary<string, object>();
            //try
            //{
            var idUsuario = getUsuario().id;
            var usuario = usuarioFactoryServices.getUsuarioService().getCCsUsuario(idUsuario);
            //List<tblM_AsignacionEquipos> raw = controlEnvioyRecepcionFactoryService.getControlEnvioyRecepcionFactoryServices().GetListaControlesPendientesRecepcion();
            List<tblM_AsignacionEquipos> raw = controlEnvioyRecepcionFactoryService.getControlEnvioyRecepcionFactoryServices().GetListaControlesCalidad(obj, usuario, tipoFiltro, fechaInicio, fechaFin, cc, numEconomico).Where(x => x.noEconomicoID != 0).ToList();
            //List<tblM_AsignacionEquipos> raw = controlEnvioyRecepcionFactoryService.getControlEnvioyRecepcionFactoryServices().GetListaControlesCalidad(obj, usuario, tipoFiltro).Where(x => x.noEconomicoID != 0 && x.noEconomicoID==1391).ToList();

            if (raw != null)
            {
                var ListaEconomicos = new List<dynamic>();
                foreach (var x in raw)
                {
                    if (x.noEconomicoID == 28141)
                    {
                        var a = 1;
                    }
                    var m = maquinaFactoryServices.getMaquinaServices().GetMaquina(x.noEconomicoID);
                    var CCName = "";
                    if (x.noEconomicoID == 0)
                    {
                        CCName = centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(x.cc);
                    }
                    else
                    {

                        if (m != null)
                        {
                            if (m.centro_costos.Equals("1010") || m.centro_costos.Equals("1015"))
                            {
                                CCName = centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(m.centro_costos);
                            }
                            else
                            {
                                CCName = centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(x.cc);
                            }
                        }
                        else
                        {
                            CCName = centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(x.cc);
                        }
                    }
                    var doc = ControlCalidadService.getControlCalidadFactoryServices().getControlCalidadById(x.id, obj, tipoFiltro);
                    
                    var o = new
                    {
                        id = x.solicitudEquipoID,
                        Folio = x.folio ?? "",
                        CCName = CCName ?? "",
                        Fecha = x.fechaInicio.ToString("dd/MM/yyyy") ?? "",
                        Economico = x.noEconomicoID,
                        nomb = x.noEconomicoID == 0 ? x.Economico : m != null ? m.noEconomico ?? "" : "",
                        isrenta = x.noEconomicoID == 0 ? false : m != null ? m.renta : false,
                        mostrar = ValidMaquina(x.noEconomicoID) != "" ? true : false,
                        idAsigancion = x.id,
                        cc = x.cc ?? "",
                        estatus = x.noEconomicoID == 0 ? x.estatus : GetMostrar(x.estatus, x.cc, x.noEconomicoID),
                        reporte = doc != null ? doc.id : 0,
                        fechaFormato = x.fechaInicio,
                        CCOrigen = centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(x.CCOrigen),
                        ccor = x.cc,
                        contieneArchivos = controlEnvioyRecepcionFactoryService.getControlEnvioyRecepcionFactoryServices().asignacionContieneArchivos(x.id)
                    };

                    ListaEconomicos.Add(o);
                }


                if (tipoFiltro == 1)
                {
                    result.Add("EquiposPendientes", ListaEconomicos.Where(x => x.nomb != "").OrderBy(x => x.Economico).ToList());
                }
                else
                {
                    result.Add("EquiposPendientes", ListaEconomicos.Where(x => x.reporte != 0 && x.nomb != "").OrderBy(x => x.Economico).ToList());

                }
            }
            result.Add(SUCCESS, true);
            //}
            //catch (Exception e)
            //{
            //    result.Add(MESSAGE, e.Message);
            //    result.Add(SUCCESS, false);

            //}
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetMaquinariasPendientesRecepcion()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var idUsuario = getUsuario().id;
                var usuario = usuarioFactoryServices.getUsuarioService().getCCsUsuario(idUsuario);
                List<tblM_AsignacionEquipos> raw = controlEnvioyRecepcionFactoryService.getControlEnvioyRecepcionFactoryServices().GetListaControlesPendientesRecepcion();


                if (raw != null)
                {
                    var ListaEconomicos = new List<dynamic>();
                    foreach (var x in raw)
                    {
                        var m = maquinaFactoryServices.getMaquinaServices().GetMaquina(x.noEconomicoID);
                        var CCName = "";
                        if (x.noEconomicoID == 0)
                        {
                            CCName = centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(x.cc);
                        }
                        else
                        {

                            if (m != null)
                            {
                                if (m.centro_costos.Equals("1010") || m.centro_costos.Equals("1015"))
                                {
                                    CCName = centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(m.centro_costos);
                                }
                                else
                                {
                                    CCName = centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(x.cc);
                                }
                            }
                            else
                            {
                                CCName = centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(x.cc);
                            }
                        }
                        var doc = ControlCalidadService.getControlCalidadFactoryServices().getByIDAsignacionTipo(x.id, (x.estatus - 1));
                        var o = new
                        {
                            id = x.solicitudEquipoID,
                            Folio = x.folio,
                            CCName = doc != null ? doc.CcDestino : x.cc,
                            Fecha = doc != null ? doc.FechaCaptura.ToShortDateString() : x.fechaInicio.ToString("dd/MM/yyyy"),
                            Economico = x.noEconomicoID,
                            nomb = x.noEconomicoID == 0 ? x.Economico : m != null ? m.noEconomico : "",
                            isrenta = x.noEconomicoID == 0 ? false : m != null ? m.renta : false,
                            mostrar = ValidMaquina(x.noEconomicoID) != "" ? true : false,
                            idAsigancion = x.id,
                            cc = x.cc,
                            estatus = x.noEconomicoID == 0 ? x.estatus : GetMostrar(x.estatus, x.cc, x.noEconomicoID),
                            reporte = 0,
                            fechaFormato = x.fechaInicio,
                            CCOrigen = doc != null ? doc.CcOrigen : centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(x.CCOrigen),
                            ccor = doc != null ? doc.CcDestino : x.cc
                        };

                        ListaEconomicos.Add(o);
                    }

                    if (true)
                    {
                        result.Add("EquiposPendientes", ListaEconomicos.Where(x => x.nomb != "").OrderBy(x => x.Economico).ToList());
                    }
                    else
                    {
                        result.Add("EquiposPendientes", ListaEconomicos.Where(x => x.reporte != 0 && x.nomb != "").OrderBy(x => x.Economico).ToList());

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
        private int GetMostrar(int estatus, string cc, int idEconomico)
        {
            var listaResguardos = resguardoEquipoFactoryServices.getResguardoEquipoFactoryServices().getListaResguardosPendientesAutorizacion(cc, idEconomico);

            if (listaResguardos.Count() == 0)
            {
                return estatus;
            }
            else
            {
                return 9;
            }

        }
        public ActionResult GetMaquinariaEnvio(int TipoFiltro)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var idUsuario = getUsuario().id;
                var usuario = usuarioFactoryServices.getUsuarioService().getCCsUsuario(idUsuario);

                List<tblM_AsignacionEquipos> raw = controlEnvioyRecepcionFactoryService.getControlEnvioyRecepcionFactoryServices().GetPendientesEnvio(usuario);

                if (raw != null)
                {
                    if (TipoFiltro == 1)
                    {
                        raw = raw.Where(x => x.estatus.Equals(1)).ToList();
                    }
                    else
                    {
                        raw = raw.Where(x => x.estatus > 1).ToList();
                    }

                    var ListaEconomicos = raw.Select(x => new
                    {
                        id = x.solicitudEquipoID,
                        Folio = x.folio,
                        CCName = centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(x.cc),
                        Fecha = x.fechaInicio.ToString("dd/MM/yyyy"),
                        Economico = x.noEconomicoID,
                        nomb = x.noEconomicoID == 0 ? x.Economico : (maquinaFactoryServices.getMaquinaServices().GetMaquina(x.noEconomicoID) != null ? maquinaFactoryServices.getMaquinaServices().GetMaquina(x.noEconomicoID).noEconomico : ""),
                        isrenta = x.noEconomicoID == 0 ? false : maquinaFactoryServices.getMaquinaServices().GetMaquina(x.noEconomicoID) != null ? maquinaFactoryServices.getMaquinaServices().GetMaquina(x.noEconomicoID).renta : false,
                        mostrar = ValidMaquina(x.noEconomicoID) != "" ? true : false,
                        idAsigancion = x.id,
                        cc = x.cc,
                        estatus = x.estatus,
                        reporte = ControlCalidadService.getControlCalidadFactoryServices().getControlCalidadById(x.id, 1).id == null ? 0 : ControlCalidadService.getControlCalidadFactoryServices().getControlCalidadById(x.id, 1).id
                    }).OrderBy(x => x.Economico).ToList();

                    if (TipoFiltro == 1)
                    {
                        result.Add("EquiposPendientes", ListaEconomicos);
                    }
                    else
                    {
                        result.Add("EquiposPendientes", ListaEconomicos.Where(r => r.reporte != 0));
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
        public ActionResult GetMaquinariaRecepcion(int TipoFiltro)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var idUsuario = getUsuario().id;
                var usuario = usuarioFactoryServices.getUsuarioService().getCCsUsuario(idUsuario);

                List<tblM_AsignacionEquipos> raw = controlEnvioyRecepcionFactoryService.getControlEnvioyRecepcionFactoryServices().GetPendientesEnvio(usuario);

                if (raw != null)
                {
                    if (TipoFiltro == 1)
                    {
                        raw = raw.Where(x => x.estatus <= 2).ToList();

                    }
                    else if (TipoFiltro == 2)
                    {
                        raw = raw.Where(x => x.estatus > 2).ToList();
                    }
                    else if (TipoFiltro == 4)
                    {
                        raw = raw.Where(x => x.estatus.Equals(4)).ToList();
                    }

                    var ListaEconomicos = raw.Select(x => new
                    {
                        id = x.solicitudEquipoID,
                        Folio = x.folio,
                        CCName = centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(x.cc),
                        Fecha = x.fechaInicio.ToString("dd/MM/yyyy"),
                        Economico = x.noEconomicoID,
                        nomb = x.noEconomicoID == 0 ? x.Economico : (maquinaFactoryServices.getMaquinaServices().GetMaquina(x.noEconomicoID) != null ? maquinaFactoryServices.getMaquinaServices().GetMaquina(x.noEconomicoID).noEconomico : ""),
                        isrenta = x.noEconomicoID == 0 ? false : maquinaFactoryServices.getMaquinaServices().GetMaquina(x.noEconomicoID) != null ? maquinaFactoryServices.getMaquinaServices().GetMaquina(x.noEconomicoID).renta : false,
                        mostrar = ValidMaquina(x.noEconomicoID) != "" ? true : false,
                        idAsigancion = x.id,
                        cc = x.cc,
                        estatus = x.estatus,
                        reporte = ControlCalidadService.getControlCalidadFactoryServices().getControlCalidadById(x.id, 1).id == null ? 0 : ControlCalidadService.getControlCalidadFactoryServices().getControlCalidadById(x.id, 1).id
                    }).OrderBy(x => x.Economico).ToList();

                    if (TipoFiltro == 1)
                    {
                        result.Add("EquiposPendientes", ListaEconomicos);
                    }
                    else
                    {
                        result.Add("EquiposPendientes", ListaEconomicos.Where(r => r.reporte != 0));
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
        public ActionResult GetRecepcionTMC(int Filtro)
        {
            var result = new Dictionary<string, object>();
            try
            { }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        private string ValidMaquina(int maquina)
        {
            string Economico = "";
            if (maquina != 0)
            {
                var c = maquinaFactoryServices.getMaquinaServices().EconomicoNotNull(maquina);
                if (c != null)
                {
                    if (!string.IsNullOrEmpty(c.noEconomico))
                    {
                        Economico = c.noEconomico;
                    }
                }
            }

            return Economico;
        }
        public ActionResult Preguntas(int obj, int Tipo, int CCal)
        {
            ControlCalidadDTO objControlCalidad = new ControlCalidadDTO();

            string origenObra = "";
            string DestinoObra = "";

            var objAsignacion = controlEnvioyRecepcionFactoryService.getControlEnvioyRecepcionFactoryServices().getInfoAsignacion(obj);
            var objMaquina = controlEnvioyRecepcionFactoryService.getControlEnvioyRecepcionFactoryServices().GetInfoMaquinaria(objAsignacion.noEconomicoID);
            var horometro = horometroServices.getCapturaHorometroServices().getUltimoHorometro(objMaquina.noEconomico);

            if (CCal > 0)
            {
                var objCalidad = ControlCalidadService.getControlCalidadFactoryServices().getControlCalidadById(obj, Tipo);
                //modificaion roberto agregado raguilar solo linea de abajo
                objControlCalidad.solicitudID = objAsignacion.solicitudEquipoID;
                objControlCalidad.objControlCalidad = objCalidad;
            }
            else
            {


                if (Tipo <= 2)
                {
                    if (objAsignacion.CCOrigen.Equals("0"))
                    {
                        origenObra = "PROVEEDOR";
                    }
                    else
                    {
                        objControlCalidad.areaCuentaOrigen = objAsignacion.CCOrigen;
                        origenObra = (objAsignacion.CCOrigen + "-" + GetCentroCostosObra(objAsignacion.CCOrigen)).Trim();
                    }
                }
                else
                {
                    objControlCalidad.areaCuentaOrigen = objAsignacion.cc;
                    //origenObra = objAsignacion.cc + "-" + GetCentroCostosObra(objAsignacion.cc);
                    origenObra = (objAsignacion.CCOrigen + "-" + GetCentroCostosObra(objAsignacion.CCOrigen)).Trim();
                }

                if (Tipo <= 2)
                {
                    objControlCalidad.areaCuentaDestino = objAsignacion.cc;
                    DestinoObra = (objAsignacion.cc + "-" + GetCentroCostosObra(objAsignacion.cc)).Trim();
                }
                else
                {
                    objControlCalidad.areaCuentaDestino = objAsignacion.cc;
                    DestinoObra = (objAsignacion.cc + "-" + GetCentroCostosObra(objAsignacion.cc)).Trim();
                }
                if (objAsignacion.estatus == 3)
                {
                    if (objMaquina.centro_costos.Equals("1010") || objMaquina.centro_costos.Equals("1015"))
                    {
                        objControlCalidad.areaCuentaOrigen = objMaquina.centro_costos;
                        origenObra = (objMaquina.centro_costos + "-" + GetCentroCostosObra(objMaquina.centro_costos)).Trim();
                    }

                }

                switch (Tipo)
                {
                    case 3:
                        {
                            origenObra = (objAsignacion.cc + "-" + GetCentroCostosObra(objAsignacion.cc)).Trim();
                        }
                        break;
                }

                objControlCalidad.solicitudID = objAsignacion.solicitudEquipoID;
                objControlCalidad.objControlCalidad.IdAsignacion = objAsignacion.id;
                objControlCalidad.objControlCalidad.TipoControl = Tipo;
                objControlCalidad.objControlCalidad.Folio = objAsignacion.folio;
                objControlCalidad.objControlCalidad.IdEconomico = objAsignacion.noEconomicoID;
                objControlCalidad.objControlCalidad.NoEconomico = objMaquina.noEconomico;
                objControlCalidad.objControlCalidad.FechaCaptura = DateTime.Now;
                objControlCalidad.objControlCalidad.Horometro = horometro == null ? 0 : horometro.Horometro;
                objControlCalidad.objControlCalidad.Obra = DestinoObra;

                objControlCalidad.objControlCalidad.CcOrigen = origenObra;//OrigenObra2;
                objControlCalidad.objControlCalidad.CcDestino = DestinoObra;//objAsignacion.cc + " - " + CCs.getCentroCostosService().getNombreCCFix(objAsignacion.cc);

            }

            objControlCalidad.lstGrupos = GrupoPreguntasCalidad.getGrupoPreguntasFactoryServices().getListGrupoPreguntas();
            objControlCalidad.lstPreguntas = PreguntasCalidad.getPreguntasFactoryServices().getListPreguntasCalidad();


            objControlCalidad.objInfoMaquina = objMaquina;

            ViewBag.pagina = "Preguntas";
            return View(objControlCalidad);

        }
        private string GetCentroCostosObra(string cc)
        {
            return CCs.getCentroCostosService().getNombreCCFix(cc);
        }
        //public ActionResult Guardar(tblM_CatControlCalidad objCalidad, List<tblM_RelPreguntaControlCalidad> lstRespuestas, tblM_ControlEnvioMaquinaria objControl, string Destino, int EnvioNormal)


        private string saveLocalFiles(HttpPostedFileBase archivo, string folder, string requestFile)
        {

            try
            {
                // HttpPostedFileBase archivo = null;
                byte[] archivoBytes = null;

                string pathCompleto = "";
                var nombreArchivo = archivo.FileName;
                var nombreArchivoSinExtension = System.IO.Path.GetFileNameWithoutExtension(nombreArchivo);
                var extension = System.IO.Path.GetExtension(nombreArchivo);
                var fechaArchivo = DateTime.Now.ToString("yyyy-MM-ddTHHmmssfff");
#if DEBUG
                var ruta = @"c:/C/" + folder + "/";
#else
             var ruta = archivoFs.getArchivo().getUrlDelServidor(10) + folder + @"\";
#endif
                if (archivo != null)
                {
                    //archivo = Request.Files["ArchivoPagare"];
                    nombreArchivo = archivo.FileName;
                    nombreArchivoSinExtension = System.IO.Path.GetFileNameWithoutExtension(nombreArchivo);
                    extension = System.IO.Path.GetExtension(nombreArchivo);
                    fechaArchivo = DateTime.Now.ToString("yyyy-mm-ddTHHmmssfff") + "0";
                    pathCompleto = System.IO.Path.Combine(ruta, nombreArchivoSinExtension + "_" + fechaArchivo + extension);
                    archivo.SaveAs(pathCompleto);
                    pathTempArchivos.Add(pathCompleto);
                    Stream stream = archivo.InputStream;
                    stream.Seek(0, SeekOrigin.Begin);
                    using (var streamReader = new MemoryStream())
                    {
                        stream.CopyTo(streamReader);
                        archivoBytes = streamReader.ToArray();
                        List<adjuntoCorreoDTO> files = (List<adjuntoCorreoDTO>)Session["lstArchivosControles"];
                        files.Add(new adjuntoCorreoDTO { archivo = archivoBytes, nombreArchivo = requestFile, extArchivo = extension });
                        Session["lstArchivosControles"] = files;
                    }

                    return pathCompleto;
                }
                else
                    return null;
            }
            catch (Exception)
            {

                return "false";
            }

        }

        [HttpPost]
        public ActionResult Guardar()
        {
            var result = new Dictionary<string, object>();

            string asuntoTipoControl = "";
            List<adjuntoCorreoDTO> files = new List<adjuntoCorreoDTO>();
            Session["lstArchivosControles"] = files;

            tblM_CatControlCalidad objCalidad = new tblM_CatControlCalidad();
            pathTempArchivos = new List<string>();
            try
            {

                List<string> Correos = new List<string>();

                objCalidad = JsonConvert.DeserializeObject<tblM_CatControlCalidad>(Request.Form["objCalidad"]);
                var objControl = JsonConvert.DeserializeObject<tblM_ControlEnvioMaquinaria>(Request.Form["objControl"]);
                var lstRespuestas = JsonConvert.DeserializeObject<List<tblM_RelPreguntaControlCalidad>>(Request.Form["lstRespuestas"]);
                var Destino = Request.Form["Destino"].ToString();
                var EnvioNormal = Request.Form["EnvioNormal"].ToString();
                var DestinoCBOX = Request.Form["DestinoCBOX"].ToString();

                //Se cambio esta parte de codigo hacia arriba para poder tener la validacion en caso de que se desee enviar el equipo de taller o patio de regreso a obra, Devolucion de equipo.
                var EquipoMaquina = maquinaFactoryServices.getMaquinaServices().GetMaquina(objCalidad.IdEconomico);

                #region Guardado de Archivos
                var folderSOS = "DOCUMENTOS_SOS";
                var folderDN = "DOCUMENTOS_DN";
                var folderRehabilitacion = "DOCUMENTOS_REHABILITACION";
                var folderSetFotografico = "DOCUMENTOS_SETFOTOGRAFICO";
                var folderBitacora = "DOCUMENTOS_BITACORA";
                var folderCheckList = "DOCUMENTOS_CHECKLIST";
                var folderVidaAceites = "DOCUMENTOS_VIDAACEITES";

                HttpPostedFileBase archivoSetFotografico = Request.Files["archivoSetFotografico"];
                HttpPostedFileBase archivoRehabilitacion = Request.Files["archivoRehabilitacion"];
                HttpPostedFileBase archivoDN = Request.Files["archivoDN"];
                HttpPostedFileBase archivoSOS = Request.Files["archivoSOS"];
                HttpPostedFileBase archivoBitacora = Request.Files["archivoBitacora"];
                HttpPostedFileBase archivoChecklist = Request.Files["archivoChecklist"];
                HttpPostedFileBase archivoVidaAceites = Request.Files["archivoVidaAceites"];

                string pathSetFotografico = archivoSetFotografico != null ? saveLocalFiles(archivoSetFotografico, folderSetFotografico, "archivoSetFotografico") : "";
                string pathRehabilitacion = archivoRehabilitacion != null ? saveLocalFiles(archivoRehabilitacion, folderRehabilitacion, "archivoRehabilitacion") : "";
                string pathDN = archivoDN != null ? saveLocalFiles(archivoDN, folderDN, "archivoDN") : "";
                string pathSOS = archivoSOS != null ? saveLocalFiles(archivoSOS, folderSOS, "archivoSOS") : "";
                string pathBitacora = archivoBitacora != null ? saveLocalFiles(archivoBitacora, folderBitacora, "archivoBitacora") : "";
                string pathVidaAceites = archivoVidaAceites != null ? saveLocalFiles(archivoVidaAceites, folderVidaAceites, "archivoVidaAceites") : "";

                //Aplica Solamente para la recepcion de equipos.
                string pathChecklist = archivoChecklist != null ? saveLocalFiles(archivoChecklist, folderCheckList, "archivoChecklist") : "";

                if (pathBitacora != "false")
                    objCalidad.archivoBitacora = pathBitacora;
                if (pathDN != "false")
                    objCalidad.archivoDN = pathDN;

                if (pathRehabilitacion != "false")
                    objCalidad.archivoRehabilitacion = pathRehabilitacion;

                if (pathSetFotografico != "false")
                    objCalidad.archivoSetFotografico = pathSetFotografico;

                if (pathSOS != "false")
                    objCalidad.archivoSOS = pathSOS;

                if (pathChecklist != "false")
                    objCalidad.archivoCheckList = pathChecklist;

                if (pathVidaAceites != "false")
                    objCalidad.archivoVidaAceites = pathVidaAceites;

                #endregion

                tblM_AsignacionEquipos objAsignacion = controlEnvioyRecepcionFactoryService.getControlEnvioyRecepcionFactoryServices().getInfoAsignacion(objCalidad.IdAsignacion);
                string economico = " ";
                if (EquipoMaquina != null)
                {
                    economico = EquipoMaquina.noEconomico;
                }

                if (objAsignacion.estatus == 3)
                {
                    //Se verifica que el equipo se encuentre en TMC o en Patio con estatus 3 para poder hacer el cambio de los nombres y el envio y Recepción del equipo.
                    if (EquipoMaquina.centro_costos.Equals("1015") || EquipoMaquina.centro_costos.Equals("1010"))
                    {
                        objCalidad.CcDestino = Destino;
                        objCalidad.CcOrigen = GetCentroCostosObra(EquipoMaquina.centro_costos);
                        string responsableE = objControl.nombreResponsableRecepcion;
                        string responsableR = objControl.nombreResponsableEnvio;

                        objControl.nombreResponsableEnvio = responsableE;
                        objControl.nombreResponsableRecepcion = responsableR;
                    }
                }

                objControl.fechaElaboracion = DateTime.Now;
                objCalidad.usuarioID = vSesiones.sesionUsuarioDTO.id;
                objControl.usuarioID = vSesiones.sesionUsuarioDTO.id;
                objCalidad = ControlCalidadService.getControlCalidadFactoryServices().saveControlCalidad(objCalidad);

                foreach (var aux in lstRespuestas)
                {
                    aux.IdControl = objCalidad.id;
                }

                RespuestasCalidadService.getRespuestasCalidadFactoryServices().saveRespuestasCalidad(lstRespuestas);

                string AsuntoCorreo = "";
                if (objCalidad.TipoControl == 1)
                {
                    objControl.lugar = objAsignacion.CCOrigen;
                    AsuntoCorreo = AsuntoCorreoString(objCalidad.TipoControl, objCalidad.Folio, economico, objAsignacion.CCOrigen, objAsignacion.cc);

                    var listaCorreos = GetlistaCorreosSend(objAsignacion.CCOrigen);
                    var listaCorreos2 = GetlistaCorreosSend(objAsignacion.cc);
                    Correos.AddRange(listaCorreos);
                    Correos.AddRange(listaCorreos2);

                    string correoGerente = GetGerenteRecepcion(objAsignacion.cc);

                    Correos.Add(correoGerente);

                    objAsignacion.estatus = 2;

                    Correos.AddRange(usuarioFactoryServices.getUsuarioService().getListaCorreosEnvioGlobal(25, objAsignacion.CCOrigen));
                    if (objAsignacion.CCOrigen == "1010" || objAsignacion.CCOrigen == "1015")
                    {
                        Correos.Add("luis.gracia@construplan.com.mx");
                        Correos.Add("patiodemaquinaria@construplan.com.mx");
                        //Correos.Add("juan.felix@construplan.com.mx");
                    }
                    asuntoTipoControl = "Control Envío";

                    removePiezas(objAsignacion.noEconomicoID);

                    var cboResponsables = usuarioFactoryServices.getUsuarioService().getPerfilesUsuario(5, objAsignacion.CCOrigen);
                    foreach (var item in cboResponsables)
                    {
                        Correos.Add(GetCorreo(item.usuarioID));
                    }
                }
                else if (objCalidad.TipoControl == 2)
                {

                    objControl.lugar = objAsignacion.cc;
                    AsuntoCorreo = AsuntoCorreoString(objCalidad.TipoControl, objCalidad.Folio, economico, objAsignacion.CCOrigen, objAsignacion.cc);
                    objAsignacion.estatus = 3;

                    var listaCorreos = GetlistaCorreosSend(objAsignacion.CCOrigen);
                    var listaCorreos2 = GetlistaCorreosSend(objAsignacion.cc);
                    Correos.AddRange(listaCorreos);
                    Correos.AddRange(listaCorreos2);

                    string correoGerente = GetGerenteRecepcion(objAsignacion.cc);

                    Correos.Add(correoGerente);
                    Correos.AddRange(usuarioFactoryServices.getUsuarioService().getListaCorreosEnvioGlobal(25, objAsignacion.cc));

                    //Correos.Add("e.salazar@construplan.com.mx");
                    Correos.Add("ana.mendez@construplan.com.mx");
                    if (objAsignacion.CCOrigen == "1010" || objAsignacion.CCOrigen == "1015")
                    {
                        Correos.Add("luis.gracia@construplan.com.mx");
                        //Correos.Add("juan.felix@construplan.com.mx");
                        Correos.Add("patiodemaquinaria@construplan.com.mx");
                    }
                    Correos.Add("antoniocastro@construplan.com.mx");
                    asuntoTipoControl = "Control Recepción";

                }
                else if (objCalidad.TipoControl == 3)
                {

                    //TRABAJANDO EN ESTE PUNTO.
                    objControl.lugar = objAsignacion.cc;
                    string envio = "";
                    if (EquipoMaquina.centro_costos.Equals("1010") || EquipoMaquina.centro_costos.Equals("1015"))
                    {
                        envio = EquipoMaquina.centro_costos;
                        //objAsignacion.CCOrigen = EquipoMaquina.centro_costos;
                    }
                    else
                        envio = objAsignacion.cc;

                    objAsignacion.CCOrigen = EquipoMaquina.centro_costos;

                    if (DestinoCBOX != "1")
                    {
                        objAsignacion.cc = DestinoCBOX;
                    }

                    Session["origenTMCPRV"] = controlEnvioyRecepcionFactoryService.getControlEnvioyRecepcionFactoryServices().getAC(objAsignacion.CCOrigen);
                    Session["destinoTMCPRV"] = controlEnvioyRecepcionFactoryService.getControlEnvioyRecepcionFactoryServices().getAC(objAsignacion.cc);

                    AsuntoCorreo = AsuntoCorreoString(objCalidad.TipoControl, objCalidad.Folio, economico, envio, Destino);

                    switch (objCalidad.CcDestino)
                    {
                        case "TALLER MECANICO CENTRAL":
                        case "PATIO MAQUINARIA HERMOSILLO":
                            objAsignacion.estatus = 4;//CUANDO SE DEL TIPO 4 OTRO MODULO PODRA VOLVERLO AL TRES  PARA QUE PUEDA REUBICARLO A SU ULTIMA OBRA ASIGNADA
                            break;
                        case "ENVIO PROVEEDOR":
                        case "VENTA EQUIPO":
                            objAsignacion.estatus = 10;
                            break;
                        //case "OBRA":
                        default:
                            objAsignacion.estatus = 2;//SE REGRESA A LA ULTIMA ASIGNACÍON DEL CC.
                            objCalidad.TipoControl = 1;
                            break;
                    }
                    var listaCorreos = GetlistaCorreosSend(objAsignacion.cc);

                    Correos.AddRange(usuarioFactoryServices.getUsuarioService().getListaCorreosEnvioGlobal(25, objAsignacion.cc));
                    if (Destino != "997")
                    {
                        var listaCorreos2 = GetlistaCorreosSend(Destino);
                        Correos.AddRange(listaCorreos2);
                    }

                    Correos.AddRange(listaCorreos);

                    var cboResponsables = usuarioFactoryServices.getUsuarioService().getPerfilesUsuario(5, objAsignacion.CCOrigen);
                    foreach (var item in cboResponsables)
                    {
                        Correos.Add(GetCorreo(item.usuarioID));
                    }

                    //Correos.Add("e.salazar@construplan.com.mx");
                    Correos.Add("ana.mendez@construplan.com.mx");
                    Correos.Add("luis.gracia@construplan.com.mx");
                    Correos.Add("patiodemaquinaria@construplan.com.mx");
                    //Correos.Add("juan.felix@construplan.com.mx");
                    asuntoTipoControl = "Control Envío";
                }
                else if (objCalidad.TipoControl == 4)
                {
                    objAsignacion.CCOrigen = EquipoMaquina.centro_costos;
                    if (DestinoCBOX != "1")
                    {
                        objAsignacion.cc = DestinoCBOX;
                    }

                    Session["origenTMCPRV"] = controlEnvioyRecepcionFactoryService.getControlEnvioyRecepcionFactoryServices().getAC(objAsignacion.CCOrigen);
                    Session["destinoTMCPRV"] = controlEnvioyRecepcionFactoryService.getControlEnvioyRecepcionFactoryServices().getAC(objAsignacion.cc);

                    switch (Destino)
                    {
                        case "TALLER MECANICO CENTRAL":
                            {
                                objControl.lugar = "1010";

                                //Correos.Add("juan.felix@construplan.com.mx");
                                Correos.Add("patiodemaquinaria@construplan.com.mx");
                            }
                            break;
                        case "PATIO MAQUINARIA HERMOSILLO":
                            {
                                Correos.Add("luis.gracia@construplan.com.mx");
                                Correos.Add("patiodemaquinaria@construplan.com.mx");
                                objControl.lugar = "1015";
                            }
                            break;
                        case "1010":
                            {
                                Correos.Add("luis.gracia@construplan.com.mx");
                                Correos.Add("patiodemaquinaria@construplan.com.mx");
                                objControl.lugar = "997";
                                objCalidad.CcDestino = Destino;

                            }
                            break;
                        case "1015":
                            {
                                //Correos.Add("juan.felix@construplan.com.mx");
                                Correos.Add("patiodemaquinaria@construplan.com.mx");
                                objControl.lugar = "1015";
                                objCalidad.CcDestino = Destino;

                            }
                            break;
                        default:
                            {
                                Correos.Add("luis.gracia@construplan.com.mx");
                                Correos.Add("patiodemaquinaria@construplan.com.mx");
                                objControl.lugar = "997";

                            }
                            break;
                    }

                    AsuntoCorreo = AsuntoCorreoString(objCalidad.TipoControl, objCalidad.Folio, economico, objAsignacion.cc, Destino);
                    objAsignacion.estatus = 5;// estos son tipo 4 raguilar
                    //objAsignacion.estatus = 4;// raguilar prueba

                    var listaCorreos = GetlistaCorreosSend(objAsignacion.cc);
                    var listaCorreos2 = GetlistaCorreosSend(Destino);
                    Correos.AddRange(listaCorreos);
                    Correos.AddRange(listaCorreos2);

                    Correos.AddRange(usuarioFactoryServices.getUsuarioService().getListaCorreosEnvioGlobal(25, Destino));
                    Correos.Add("luis.gracia@construplan.com.mx");
                    Correos.Add("patiodemaquinaria@construplan.com.mx");
                    //Correos.Add("juan.felix@construplan.com.mx");
                    asuntoTipoControl = "Control Recepción";
                }

                objControl.tipoControl = objCalidad.TipoControl;
                objControl.nota = objCalidad.Observaciones;
                var idAsignacion = objAsignacion.id;
                objControl.asignacionEquipoId = idAsignacion;
                objControl.solicitudEquipoID = objAsignacion.solicitudEquipoID;
                objControl.noEconomico = objCalidad.IdEconomico;
                controlEnvioyRecepcionFactoryService.getControlEnvioyRecepcionFactoryServices().SaveOrUpdate(objControl, null);

                if (objAsignacion.estatus == 3)
                {
                    var maquina = maquinaFactoryServices.getMaquinaServices().GetMaquina(objAsignacion.noEconomicoID);
                    maquina.centro_costos = objAsignacion.cc;
                    maquinaFactoryServices.getMaquinaServices().Guardar(maquina);
                    var asignacionesPendientes = asignacionEquiposFactoryServices.getAsignacionEquiposFactoryServices().getAsignacionesByEconomicos(maquina.id).Where(x => x.id != objAsignacion.id && x.estatus != 10).ToList();

                    foreach (var asignacion in asignacionesPendientes.Where(x => objAsignacion.id < x.id))
                    {
                        asignacion.estatus = 10;
                        asignacionEquiposFactoryServices.getAsignacionEquiposFactoryServices().SaveOrUpdate(asignacion);
                    }

                    //Aqui se debe agregar la opcion para revisar si existen otras asignaciones al momento de recibir el equipo cualquiera excepto tipo 1.

                }
                else if (objAsignacion.estatus == 4)
                {

                    if (EnvioNormal == "3")
                    {
                        var maquina = maquinaFactoryServices.getMaquinaServices().GetMaquina(objAsignacion.noEconomicoID);

                        switch (objCalidad.CcDestino)
                        {
                            case "ENVIO PROVEEDOR":
                            case "VENTA EQUIPO":
                                maquina.centro_costos = "0";
                                maquina.TipoBajaID = 6;
                                maquina.estatus = 9;
                                maquinaFactoryServices.getMaquinaServices().Guardar(maquina);
                                objAsignacion.estatus = 10;
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {

                        if (EnvioNormal == "4")
                        {
                            tblP_Alerta objAlerta = new tblP_Alerta();

                            objAlerta.msj = "Se agregó un equipo para baja.";
                            objAlerta.sistemaID = 1;
                            objAlerta.tipoAlerta = (int)AlertasEnum.REDIRECCION;
                            objAlerta.url = "/CatMaquina/index?idMaquinaria=" + objAsignacion.noEconomicoID;
                            objAlerta.objID = objAsignacion.noEconomicoID;
                            objAlerta.userEnviaID = getUsuario().id;
                            objAlerta.userRecibeID = 1123;
                            alertasFactoryServices.getAlertaService().saveAlerta(objAlerta);
                        }
                    }

                }
                else if (objAsignacion.estatus >= 5)
                {

                    switch (objCalidad.CcDestino)
                    {
                        case "TALLER MECANICO CENTRAL":
                            {
                                var maquina = maquinaFactoryServices.getMaquinaServices().GetMaquina(objAsignacion.noEconomicoID);
                                maquina.centro_costos = "1010";
                                maquinaFactoryServices.getMaquinaServices().Guardar(maquina);
                            }
                            break;
                        case "PATIO MAQUINARIA HERMOSILLO":
                            {
                                var maquina = maquinaFactoryServices.getMaquinaServices().GetMaquina(objAsignacion.noEconomicoID);
                                maquina.centro_costos = "1015";
                                maquinaFactoryServices.getMaquinaServices().Guardar(maquina);
                                objAsignacion.estatus = 3;
                            }
                            break;
                        case "ENVIO PROVEEDOR":
                        case "VENTA EQUIPO":
                            {
                                var maquina = maquinaFactoryServices.getMaquinaServices().GetMaquina(objAsignacion.noEconomicoID);
                                maquina.centro_costos = "997";
                                maquina.TipoBajaID = 6;
                                maquina.estatus = 0;
                                maquinaFactoryServices.getMaquinaServices().Guardar(maquina);
                            }
                            break;
                        default:
                            {
                                var maquina = maquinaFactoryServices.getMaquinaServices().GetMaquina(objAsignacion.noEconomicoID);
                                maquina.centro_costos = "997";
                                maquina.TipoBajaID = 6;
                                maquina.estatus = 0;
                                maquinaFactoryServices.getMaquinaServices().Guardar(maquina);
                            }
                            break;
                    }
                }
                //else if (objAsignacion.estatus == 2)//obra se debe guardar con estatus dos lista para Recepción en obra de solicitud
                //{
                //    var maquina = maquinaFactoryServices.getMaquinaServices().GetMaquina(objAsignacion.noEconomicoID);
                //    maquinaFactoryServices.getMaquinaServices().Guardar(maquina);
                //}
                //sendEmailAdjuntoInMemorySend
                List<tblM_AsignacionEquipos> lstAsignada = new List<tblM_AsignacionEquipos>();
                lstAsignada.Add(objAsignacion);
                asignacionEquiposFactoryServices.getAsignacionEquiposFactoryServices().SaveOrUpdate(lstAsignada);

                alertasFactoryServices.getAlertaService().updateAlertaByModulo(objAsignacion.id, 24);

                string asuntoCorreo = CuerpoCorreo(objCalidad.TipoControl, objCalidad.Folio, economico, objAsignacion.CCOrigen, objAsignacion.cc);
                Session["AsuntoCorreosControles"] = asuntoCorreo;
                Session["TituloCorreoControles"] = "Notificación: " + asuntoTipoControl + ". Folio Solicitud " + objAsignacion.folio;
                Correos.Add("e.encinas@construplan.com.mx");
                
                #region Coordinadores de Seguridad y Salud en el Trabajo
                Correos.AddRange(usuarioFactoryServices.getUsuarioService().getListaCorreosEnvioGlobal(objAsignacion.cc));
                Correos.AddRange(usuarioFactoryServices.getUsuarioService().getListaCorreosEnvioGlobal(Destino));
                #endregion

                //Agregar correos de administradores de maquinaria de las obras origen y destino.
                Correos.AddRange(controlEnvioyRecepcionFactoryService.getControlEnvioyRecepcionFactoryServices().getCorreosAdministradoresMaquinaria(objCalidad));

                Session["CorreosControles"] = Correos.Distinct().ToList();

                result.Add(ITEMS, objCalidad);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                asignacionEquiposFactoryServices.getAsignacionEquiposFactoryServices().logErrores(1, 24, "ControlCalidad", "GetIDControl", e, Core.Enum.Principal.Bitacoras.AccionEnum.CONSULTA, 0, 0);
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private void removePiezas(int p)
        {
            try
            {

            }
            catch (Exception)
            {


            }
        }

        public ActionResult Senf()
        {

            var result = new Dictionary<string, object>();
            try
            {
                var downloadPDF = (List<Byte[]>)Session["downloadPDF"];
                string titulo = Session["TituloCorreoControles"].ToString();
                string AsuntoCorreo = Session["AsuntoCorreosControles"].ToString();
                List<string> Corres = (List<string>)Session["CorreosControles"];
                //Corres.Clear();
                //Corres.Add("jesus.matus@construplan.com.mx");

                GlobalUtils.sendEmailAdjuntoInMemorySolicitudes(titulo, AsuntoCorreo, Corres, downloadPDF);

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
                result.Add("ControlID", 0);

            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult enviarCorreosCalidad()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var downloadPDF = (List<Byte[]>)Session["PDFControles"];
                var archivosAdjuntos = (List<adjuntoCorreoDTO>)Session["lstArchivosControles"];

                for (int i = 0; i < downloadPDF.Count; i++)
                {
                    if (i == 0)
                    {
                        archivosAdjuntos.Add(new adjuntoCorreoDTO
                        {
                            nombreArchivo = "Control Equipo",
                            archivo = downloadPDF[i],
                            extArchivo = ".pdf"
                        });
                    }
                    else
                    {
                        archivosAdjuntos.Add(new adjuntoCorreoDTO
                        {
                            nombreArchivo = "Control Calidad",
                            archivo = downloadPDF[i],
                            extArchivo = ".pdf"
                        });
                    }
                }

                string titulo = Session["TituloCorreoControles"].ToString();
                string AsuntoCorreo = Session["AsuntoCorreosControles"].ToString();
                List<string> Corres = (List<string>)Session["CorreosControles"];
                //List<string> Corres = new List<string> { "martin.zayas@construplan.com.mx" };
                Session["lstArchivosControles"] = null;
                Session["PDFControles"] = null;

                GlobalUtils.sendMailWithFiles(titulo, AsuntoCorreo, Corres, archivosAdjuntos);

            }
            catch (Exception e)
            {
                asignacionEquiposFactoryServices.getAsignacionEquiposFactoryServices().logErrores(1, 24, "ControlCalidad", "enviarCorreosCalidad", e, Core.Enum.Principal.Bitacoras.AccionEnum.CORREO, 0, 0);
                Session["PDFControles"] = null;
                result.Add(MESSAGE, "No se envió el correo. " + e.Message);
                result.Add(SUCCESS, false);
                result.Add("ControlID", 0);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        private string GetGerenteRecepcion(string centroCostos)
        {
            string correo = "";

            correo = controlEnvioyRecepcionFactoryService.getControlEnvioyRecepcionFactoryServices().getCorreoGerente(centroCostos);

            return correo;
        }
        private string GetCorreo(int id)
        {
            return usuarioFactoryServices.getUsuarioService().ListUsersById(id).FirstOrDefault().correo;
        }
        private List<string> GetlistaCorreosSend(string cc)
        {

            List<cboDTO> cboDTOList = new List<cboDTO>();
            List<tblP_Autoriza> cboResponsables = new List<tblP_Autoriza>();
            List<string> ListaCorreos = new List<string>();
            cboResponsables = usuarioFactoryServices.getUsuarioService().getPerfilesUsuario(1, cc);
            cboResponsables.AddRange(usuarioFactoryServices.getUsuarioService().getPerfilesUsuario(8, cc));

            foreach (var item in cboResponsables)
            {
                ListaCorreos.Add(GetCorreo(item.usuarioID));
            }

            return ListaCorreos;
        }
        public string AsuntoCorreoString(int tipoCorreo, string folio, string economico, string CCOrigen, string CcDestino)
        {

            string textoAsuntoCorreo = "";
            switch (tipoCorreo)
            {
                case 1:
                    textoAsuntoCorreo = "Se Generó un control de envío del equipo ";
                    break;
                case 2:
                    textoAsuntoCorreo = "Se Generó un control de recepción del equipo ";
                    break;
                case 3:
                    textoAsuntoCorreo = "Se Generó un control de envío del equipo ";
                    break;
                case 4:
                    textoAsuntoCorreo = "Se Generó un control de recepción del equipo ";
                    break;
                default:
                    break;
            }

            if (string.IsNullOrEmpty(CcDestino))
            {
                CcDestino = "Envío a Externo";
            }

            textoAsuntoCorreo += "con el económico  " + economico + " con el origen " + CCOrigen + " con destino " + CcDestino;

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
                                <body>"
                                    + textoAsuntoCorreo +
                                "</body>" +
                               " </html>";


            return AsuntoCorreo;
        }

        private string CuerpoCorreo(int tipoCorreo, string folio, string economico, string CCOrigen, string CcDestino)
        {

            string textoAsuntoCorreo = "";
            switch (tipoCorreo)
            {
                case 1:
                    textoAsuntoCorreo = "Se Generó un control de envío del equipo ";
                    break;
                case 2:
                    textoAsuntoCorreo = "Se Generó un control de recepción del equipo ";
                    break;
                case 3:
                    textoAsuntoCorreo = "Se Generó un control de envío del equipo ";
                    break;
                case 4:
                    textoAsuntoCorreo = "Se Generó un control de recepción del equipo ";
                    break;
                default:
                    break;
            }

            string texto = @"   <p>Buen d&iacute;a.</p>
                                <p>" + textoAsuntoCorreo + @"</p>
                                <table style='height: 126px; width: 691px; border=1'>
                                <tbody>
                                <tr style='height: 38px; background-color: #f39c12;'>
                                <td style='width: 110px; height: 38px;text-align: center;'> <strong>Solicitud</strong></td>
                                <td style='width: 135px; height: 38px;text-align: center;'><strong>Equipo</strong></td>
                                <td style='width: 205px; height: 38px;text-align: center;'><strong>Lugar Envío</strong></td>
                                <td style='width: 205px; height: 38px;text-align: center;'><strong>Lugar Recepción</strong></td>
                                </tr>
                                <tr style='height: 70px;'>
                                <td style='width: 114px; height: 70px;text-align: center;'>" + folio + @"</td>
                                <td style='width: 127px; height: 70px;text-align: center;'>" + economico + @"</td>
                                <td style='width: 198px; height: 70px;text-align: center;'>" + CCOrigen + @"</td>
                                <td style='width: 224px; height: 70px;text-align: center;'>" + CcDestino + @"</td>
                                </tr>
                                </tbody>
                                </table>
                                <p>&nbsp;</p>
                                <p>Saludos.</p>
                                <hr />
                                <p>Este es un correo autogenerado por el sistema SIGOPLAN, favor de contestar el correo.</p>";

            return texto;
        }
        public ActionResult GetRespuestas(int id)
        {
            var lstRespuestas = RespuestasCalidadService.getRespuestasCalidadFactoryServices().getListRespuestasCalidad(id);

            return Json(lstRespuestas, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetIDControl(int asignacionID, int TipoControl, int solicitudID)
        {

            var result = new Dictionary<string, object>();
            try
            {
                var ControlID = controlEnvioyRecepcionFactoryService.getControlEnvioyRecepcionFactoryServices().getInfoControl(asignacionID, TipoControl, solicitudID);
                if (ControlID != null)
                {
                    result.Add("ControlID", ControlID.id);
                }
                else
                {
                    result.Add("ControlID", 0);
                }
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                asignacionEquiposFactoryServices.getAsignacionEquiposFactoryServices().logErrores(1, 24, "ControlCalidad", "GetIDControl", e, Core.Enum.Principal.Bitacoras.AccionEnum.CONSULTA, 0, 0);
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
                result.Add("ControlID", 0);

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GETidControlCalidad(int asignacionID, int TipoControl, int solicitudID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var ControlID = ControlCalidadService.getControlCalidadFactoryServices().getByIDAsignacionTipo(asignacionID, TipoControl);

                if (ControlID != null)
                {
                    result.Add("ControlExiste", true);
                }
                else
                {
                    result.Add("ControlID", false);
                }
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
                result.Add("ControlID", 0);

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GuardarControlCalidad(tblM_CatControlCalidad objCalidad,
                                                  List<tblM_RelPreguntaControlCalidad> lstRespuestas,
                                                  tblM_ControlMovimientoMaquinaria objControl,
                                                  string areaCuentaDestino, string areaCuentaOrigen,
                                                  int tipoControl, int envioEspecial)
        {
            List<string> Correos = new List<string>();

            //Se cambio esta parte de codigo hacia arriba para poder tener la validacion en caso de que se desee enviar el equipo de taller o patio de regreso a obra, Devolucion de equipo.
            var EquipoMaquina = maquinaFactoryServices.getMaquinaServices().GetMaquina(objCalidad.IdEconomico);
            tblM_AsignacionEquipos objAsignacion = controlEnvioyRecepcionFactoryService.getControlEnvioyRecepcionFactoryServices().getInfoAsignacion(objCalidad.IdAsignacion);
            string economico = " ";
            objControl.fechaElaboracion = DateTime.Now;
            if (EquipoMaquina != null)
            {
                economico = EquipoMaquina.noEconomico;
            }

            if (objAsignacion.estatus == 3)
            {
                //Se verifica que el equipo se encuentre en TMC o en Patio con estatus 3 para poder hacer el cambio de los nombres y el envio y Recepción del equipo.
                if (EquipoMaquina.centro_costos.Equals("1015") || EquipoMaquina.centro_costos.Equals("1010"))
                {
                    objCalidad.CcDestino = areaCuentaDestino;
                    objCalidad.CcOrigen = GetCentroCostosObra(EquipoMaquina.centro_costos);
                    string responsableE = objControl.nombreResponsableRecepcion;
                    string responsableR = objControl.nombreResponsableEnvio;

                    objControl.nombreResponsableEnvio = responsableE;
                    objControl.nombreResponsableRecepcion = responsableR;
                }
            }
            var result = ControlCalidadService.getControlCalidadFactoryServices().guardarControlMovimientoMaquinaria(objCalidad, lstRespuestas, objControl, areaCuentaDestino, areaCuentaOrigen, tipoControl, envioEspecial);

            result.Add("asignacionID", objCalidad.IdAsignacion);
            result.Add("tipoControl", tipoControl);
            result.Add("solicitudID", objAsignacion.solicitudEquipoID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult DescargarArchivos(int idAsignacion, int solicitudID)
        {
            var resultadoTupla = controlEnvioyRecepcionFactoryService.getControlEnvioyRecepcionFactoryServices().descargarArchivos(idAsignacion, solicitudID);

            if (resultadoTupla != null)
            {
                string nombreArchivo = resultadoTupla.Item2;
                string tipo = MimeMapping.GetMimeMapping(nombreArchivo);

                var fileStreamResult = new FileStreamResult(resultadoTupla.Item1, tipo);
                fileStreamResult.FileDownloadName = nombreArchivo;

                return fileStreamResult;
            }
            else
            {
                return View("ErrorDescarga");
            }
        }

        #region FILL COMBOS
        public ActionResult GetCCs()
        {
            return Json(controlEnvioyRecepcionFactoryService.getControlEnvioyRecepcionFactoryServices().GetCCs(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEconomicos()
        {
            return Json(controlEnvioyRecepcionFactoryService.getControlEnvioyRecepcionFactoryServices().GetEconomicos(), JsonRequestBehavior.AllowGet);
        }
        
        #endregion
    }
}