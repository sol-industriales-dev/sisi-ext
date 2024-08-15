using Core.DTO.Maquinaria.Inventario;
using Core.DTO.Maquinaria.Inventario.Resguardo;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Maquinaria.Inventario;
using Data.Factory.Maquinaria.Catalogos;
using Data.Factory.Maquinaria.Inventario;
using Data.Factory.Principal.Alertas;
using Data.Factory.Principal.Archivos;
using Data.Factory.Principal.Usuarios;
using Data.Factory.RecursosHumanos.Captura;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Controllers.Maquinaria.Inventario
{
    public class ResguardoEquipoController : BaseController
    {
        DocumentosMaquinariaFactoryServices documentosMaquinariaFactoryServices;
        UsuarioFactoryServices usuarioFactoryServices = new UsuarioFactoryServices();
        DocumentosResguardoFactoryServices documentosResguardoFactoryServices;
        AutorizacionResguardoFactoryServices autorizacionResguardoFactoryServices;
        FormatoCambioFactoryService capturaFormatoCambioFactoryServices;
        ResguardoEquipoFactoryServices resguardoEquipoFactoryServices;
        MaquinaFactoryServices maquinaFactoryServices;
        CentroCostosFactoryServices centroCostosFactoryServices;
        RespuestaResguardoVehiculosFactoryServices respuestaResguardoVehiculosFactoryServices;
        AlertaFactoryServices alertaFactoryServices = new AlertaFactoryServices();
        ArchivoFactoryServices archivofs;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            documentosResguardoFactoryServices = new DocumentosResguardoFactoryServices();
            respuestaResguardoVehiculosFactoryServices = new RespuestaResguardoVehiculosFactoryServices();
            centroCostosFactoryServices = new CentroCostosFactoryServices();
            autorizacionResguardoFactoryServices = new AutorizacionResguardoFactoryServices();
            capturaFormatoCambioFactoryServices = new FormatoCambioFactoryService();
            resguardoEquipoFactoryServices = new ResguardoEquipoFactoryServices();
            maquinaFactoryServices = new MaquinaFactoryServices();
            archivofs = new ArchivoFactoryServices();
            documentosMaquinariaFactoryServices = new DocumentosMaquinariaFactoryServices();
            base.OnActionExecuting(filterContext);
        }
        // GET: ResguardoEquipo
        public ActionResult AsignacionResguardo()
        {
            return View();
        }

        public ActionResult AutorizacionResguardo()
        {
            return View();
        }

        public ActionResult cboEconomicos(string obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<int> Resguardo = resguardoEquipoFactoryServices.getResguardoEquipoFactoryServices().GetMaquinariaAsignada();
                List<string> ListaEconomicos = usuarioFactoryServices.getUsuarioService().getCCsUsuario(base.getUsuario().id).Select(x => x.cc).ToList();


                var cboEconomicos = maquinaFactoryServices.getMaquinaServices().getCboMaquinaria("0").Where(x => (x.grupoMaquinaria.tipoEquipoID == 3 || ((x.centro_costos == "18-1" || x.centro_costos == "18-2") && x.modeloEquipoID == 3763)) && ListaEconomicos.Contains(x.centro_costos));

                result.Add(ITEMS, cboEconomicos.Select(x => new { Value = x.id, Text = x.noEconomico }).OrderBy(x => x.Text));

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult cboEconomicosSinFiltro(string obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                //  List<int> Resguardo = resguardoEquipoFactoryServices.getResguardoEquipoFactoryServices().GetMaquinariaAsignada();
                List<string> ListaEconomicos = usuarioFactoryServices.getUsuarioService().getCCsUsuario(base.getUsuario().id).Select(x => x.cc).ToList();


                var cboEconomicos = maquinaFactoryServices.getMaquinaServices().getCboMaquinaria("0").Where(x => x.grupoMaquinaria.tipoEquipoID == 3); //&& ListaEconomicos.Contains(x.centro_costos));

                result.Add(ITEMS, cboEconomicos.Select(x => new { Value = x.id, Text = x.noEconomico }).OrderBy(x => x.Text));

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
        public ActionResult EditarLicencia()
        {
            var result = new Dictionary<string, object>();
            try
            {
                HttpPostedFileBase archivoLicencia = null;
                HttpPostedFileBase archivoPoliza = null;
                HttpPostedFileBase archivoCurso = null;
                DateTime fechaVencimientoLicencia = new DateTime();
                DateTime fechaVencimientoPoliza = new DateTime();
                tblM_CatMaquina maquinaEquipo = null;

                bool actualizaLicencia;
                bool actualizaPoliza;
                bool actualizaCurso;

                int resguardoID = Convert.ToInt32(Request.Params["resguardoID"].ToString());

                var resguardoEquipo = resguardoEquipoFactoryServices.getResguardoEquipoFactoryServices().getResguardoBYID(resguardoID);

                actualizaLicencia = Convert.ToBoolean(Request.Params["actualizaLicencia"].ToString());
                actualizaPoliza = Convert.ToBoolean(Request.Params["actualizaPoliza"].ToString());
                actualizaCurso = Convert.ToBoolean(Request.Params["actualizaCurso"].ToString());

                if (actualizaLicencia)
                {
                    fechaVencimientoLicencia = Convert.ToDateTime(Request.Form["fechaVencimientoLicencia"].ToString());
                    archivoLicencia = Request.Files["archivoLicencia"];
                    resguardoEquipo.fechaVencimiento = fechaVencimientoLicencia;
                }

                if (actualizaPoliza)
                {
                    maquinaEquipo = maquinaFactoryServices.getMaquinaServices().GetMaquinaByID(resguardoEquipo.MaquinariaID).FirstOrDefault();
                    fechaVencimientoPoliza = Convert.ToDateTime(Request.Form["fechaVencimientoPoliza"].ToString());
                    resguardoEquipo.fechaVencimientoPoliza = fechaVencimientoPoliza;
                    maquinaEquipo.fechaPoliza = fechaVencimientoPoliza;
                    archivoPoliza = Request.Files["archivoPoliza"];
                }

                if (actualizaCurso)
                {
                    archivoCurso = Request.Files["archivoCurso"];
                }

                if (Request.Form["fechaVigencia"] != "")
                {
                    var fechaVigencia = JsonConvert.DeserializeObject<DateTime>(Request.Form["fechaVigencia"], new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });

                    resguardoEquipo.fechaVigenciaCurso = fechaVigencia;
                }

                string fecha = DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.Hour + "" + DateTime.Now.Minute;
                string rutaBase = archivofs.getArchivo().getUrlDelServidor(3) + fecha;

                documentosResguardoFactoryServices.getDocumentosResguardoFactoryServices().ActualizarArchivosResguardo(resguardoID, archivoLicencia, archivoPoliza, archivoCurso, rutaBase);
                resguardoEquipoFactoryServices.getResguardoEquipoFactoryServices().GuardarResguardoVehiculos(resguardoEquipo);

                if (actualizaPoliza)
                {
                    maquinaFactoryServices.getMaquinaServices().Guardar(maquinaEquipo);
                }

                alertaFactoryServices.getAlertaService().updateAlertaByModulo(resguardoID, 45);

                result.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarPoliza(int obj, DateTime fechaVencimiento)
        {
            var result = new Dictionary<string, object>();
            try
            {
                DateTime fecha = DateTime.Now;
                int idActual = getUsuario().id;
                var ObjResguardo = resguardoEquipoFactoryServices.getResguardoEquipoFactoryServices().getResguardoBYID(obj);
                ObjResguardo.fechaVencimientoPoliza = fechaVencimiento;

                resguardoEquipoFactoryServices.getResguardoEquipoFactoryServices().GuardarResguardoVehiculos(ObjResguardo);

                //alertaFactoryServices.getAlertaService().updateAlertaByModulo(obj, 45);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetInfoEconomico(int idEconomico)
        {
            var result = new Dictionary<string, object>();
            try
            {
                // result.Add(ITEMS, maquinaFactoryServices.getMaquinaServices().getCboMaquinaria(0).Select(x => new { Value = x.id, Text = x.noEconomico }).OrderBy(x => x.Text));

                var objEconomico = maquinaFactoryServices.getMaquinaServices().GetMaquinaByID(idEconomico).Select(x => new
                {
                    Descripcion = x.descripcion,
                    Marca = x.marca.descripcion,
                    Modelo = x.modeloEquipo.descripcion,
                    Serie = x.noSerie,
                    TipoEncierro = x.tipoEncierro,
                    Placas = x.placas
                }).FirstOrDefault();

                var documentos = documentosMaquinariaFactoryServices.getDocumentosMaquinariaFactoryServices().listaDocumentos(idEconomico).Where(r => r.tipoArchivo >= 3 && r.tipoArchivo <= 5).ToList();

                result.Add("documentos", documentos);
                result.Add("dataEconomico", objEconomico);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        //raguilar obteber si tiene evidencia el resguardo
        public ActionResult GetEvidenciaByID(int idEvidencia)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var archivo = documentosResguardoFactoryServices.getDocumentosResguardoFactoryServices().GetObjRutaDocumentobyID(idEvidencia);
                result.Add("idDocumento", archivo);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetTablesPreguntas()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var raw = resguardoEquipoFactoryServices.getResguardoEquipoFactoryServices().GetListaPreguntas();
                var Interiores = raw.Where(x => x.GrupoID.Equals(1));
                var Exterior = raw.Where(x => x.GrupoID.Equals(2));
                var Documentos = raw.Where(x => x.GrupoID.Equals(3));


                result.Add("Interiores", Interiores.Select(x => new
                {
                    id = x.id,
                    Concepto = x.Pregunta,
                    Bueno = 0,
                    Regular = 0,
                    Malo = 0,
                    NA = 0,
                    Observaciones = ""

                }));
                result.Add("Exterior", Exterior.Select(x => new
                {
                    id = x.id,
                    Concepto = x.Pregunta,
                    Bueno = 0,
                    Regular = 0,
                    Malo = 0,
                    NA = 0,
                    Observaciones = ""

                }));
                result.Add("Documentos", Documentos);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult SaveOrUpdate(int obj, int tipo)
        //{
        //    var result = new Dictionary<string, object>();
        //    try
        //    {
        //        DateTime fecha = DateTime.Now;
        //        int idActual = getUsuario().id;
        //        string f = fecha.ToString("ddMMyyyy") + fecha.Hour + "" + fecha.Minute;
        //        var GetObj = autorizacionResguardoFactoryServices.getAutorizacionResguardoFactoryServices().GetObjAutorizaciones(obj);
        //        var ObjResguardo = resguardoEquipoFactoryServices.getResguardoEquipoFactoryServices().getResguardoBYID(obj);
        //        if (GetObj.usuarioSeguridadID == 0)
        //        {
        //            GetObj.usuarioSeguridadID = idActual;

        //        }

        //        if (tipo == 1)
        //        {
        //            GetObj.usuarioSeguridadFirma = idActual + f + "" + base.getUsuario().id + "A";
        //            GetObj.estatus = true;

        //            ObjResguardo.estado = 2;
        //        }
        //        else
        //        {
        //            GetObj.usuarioSeguridadFirma = idActual + f + "" + base.getUsuario().id + "R";
        //            ObjResguardo.estado = 4;
        //        }

        //        autorizacionResguardoFactoryServices.getAutorizacionResguardoFactoryServices().Guardar(GetObj);




        //        resguardoEquipoFactoryServices.getResguardoEquipoFactoryServices().GuardarResguardoVehiculos(ObjResguardo);

        //        result.Add(SUCCESS, true);
        //    }
        //    catch (Exception e)
        //    {
        //        result.Add(MESSAGE, e.Message);
        //        result.Add(SUCCESS, false);
        //    }
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public ActionResult SaveOrUpdate()
        {
            HttpPostedFileBase file1 = Request.Files["fResguardoManejo"];
            string FileName = "";
            string ruta = "";
            var idDoc = JsonConvert.DeserializeObject<int>(Request.Form["idresguardoManejo"]);
            string comentario = JsonConvert.DeserializeObject<string>(Request.Form["Comentario"]);
            var obj = JsonConvert.DeserializeObject<int>(Request.Form["obj"]);
            var tipo = JsonConvert.DeserializeObject<int>(Request.Form["tipo"]);
            var result = new Dictionary<string, object>();
            bool pathExist = false;
            try
            {
                DateTime fecha = DateTime.Now;
                int idActual = getUsuario().id;
                string f = fecha.ToString("ddMMyyyy") + fecha.Hour + "" + fecha.Minute;
                var GetObj = autorizacionResguardoFactoryServices.getAutorizacionResguardoFactoryServices().GetObjAutorizaciones(obj);
                var ObjResguardo = resguardoEquipoFactoryServices.getResguardoEquipoFactoryServices().getResguardoBYID(obj);
                if (GetObj.usuarioSeguridadID == 0)
                {
                    GetObj.usuarioSeguridadID = idActual;

                }

                if (tipo == 1)
                {
                    GetObj.usuarioSeguridadFirma = idActual + f + "" + base.getUsuario().id + "A";
                    GetObj.estatus = true;

                    if (Request.Form["fechaVigencia"] != "")
                    {
                        var fechaVigencia = JsonConvert.DeserializeObject<DateTime>(Request.Form["fechaVigencia"], new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });

                        ObjResguardo.fechaVigenciaCurso = fechaVigencia;
                    }

                    ObjResguardo.estado = 2;
                    result.Add(MESSAGE, "Se ha autorizado con éxito.");
                }
                else if (tipo == 3)
                {
                    GetObj.usuarioSeguridadFirma = idActual + f + "" + base.getUsuario().id + "R";
                    ObjResguardo.estado = 3;
                    result.Add(MESSAGE, "Se ha rechazado con éxito.");
                }

                autorizacionResguardoFactoryServices.getAutorizacionResguardoFactoryServices().Guardar(GetObj);

                resguardoEquipoFactoryServices.getResguardoEquipoFactoryServices().GuardarResguardoVehiculos(ObjResguardo);

                resguardoEquipoFactoryServices.getResguardoEquipoFactoryServices().QuitarNotificacionSSOMA(ObjResguardo.id);

                if (file1 != null && file1.ContentLength > 0)
                {
                    FileName = file1.FileName;
                    ruta = archivofs.getArchivo().getUrlDelServidor(3) + f + FileName;
                    pathExist = GuardarDocumentos(file1, ruta);
                    if (pathExist)
                    {
                        tblM_DocumentosResguardos objDoc = new tblM_DocumentosResguardos();
                        objDoc.nombreArchivo = FileName;
                        objDoc.nombreRuta = ruta;
                        objDoc.tipoArchivo = 9;
                        objDoc.ResguardoID = idDoc;
                        objDoc.fechaSubido = DateTime.Now;
                        objDoc.tipoResguardo = 1;
                        documentosResguardoFactoryServices.getDocumentosResguardoFactoryServices().Guardar(objDoc);
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

        public ActionResult getEmpleados(string term)
        {
            var CentroCostosUsuario = GetListaCentroCostos().Select(x => x.cc).ToList();

            //List<int> arrayUsuariosResguardo = new List<int>();


            //arrayUsuariosResguardo = resguardoEquipoFactoryServices.getResguardoEquipoFactoryServices().GetEmpleadosResguardo();


            var items = resguardoEquipoFactoryServices.getResguardoEquipoFactoryServices().getCatEmpleados(term, CentroCostosUsuario).Take(10);

            var filteredItems = items.Select(x => new { id = x.clave_empleado, label = x.Nombre });

            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListaAutorizacionesPendientes(string cc, int tipoDocumento)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var ListaCCUsuarios = base.GetListaCentroCostos().Select(x => x.cc).ToList();
                List<dynamic> lstDocumentos = resguardoEquipoFactoryServices.getResguardoEquipoFactoryServices().GetDocumentosResguardos();

                //if (tipoDocumento != 4)
                //{
                var listAutorizaciones = resguardoEquipoFactoryServices.getResguardoEquipoFactoryServices().GetListaAutorizacionesPendientes(cc, tipoDocumento).Where(x => ListaCCUsuarios.Contains(x.Obra));
                var data = listAutorizaciones.Select(x => new
                {
                    id = x.id,
                    Economico = maquinaFactoryServices.getMaquinaServices().GetMaquinaByID(x.MaquinariaID).FirstOrDefault().noEconomico,
                    CCName = x.Obra.ToString(),
                    UsuarioSolicitud = x.nombEmpleado,
                    Grupo = maquinaFactoryServices.getMaquinaServices().GetMaquinaByID(x.MaquinariaID).FirstOrDefault().grupoMaquinaria.descripcion,
                    Vencimiento = Convert.ToString(x.fechaVencimiento.Subtract(DateTime.Today).Days),
                    fechaCaptura = x.Fecha.ToShortDateString(),
                    documentos = infoDocumentos(maquinaFactoryServices.getMaquinaServices().GetMaquinaByID(x.MaquinariaID).FirstOrDefault().id),
                    tieneResguardo = lstDocumentos.Count() > 0 ? VerificarCantResguardos(lstDocumentos, x.id) : false,
                    resguardoFirmadoId = lstDocumentos.Where(y => y.ResguardoID == x.id).Count() > 0 ? lstDocumentos.Where(y => y.ResguardoID == x.id).First().id : 0,
                    resguardoFirmadoUrl = lstDocumentos.Where(y => y.ResguardoID == x.id).Count() > 0 ? lstDocumentos.Where(y => y.ResguardoID == x.id).First().nombreRuta.Replace("C:", @"\\REPOSITORIO") : "",
                    FechaVigencia = resguardoEquipoFactoryServices.getResguardoEquipoFactoryServices().GetFechaVigenciaResguardo(x.id)
                }).ToList();

                result.Add("listAutorizaciones", data);
                //}
                //else //Sin Resguardos
                //{
                //    var listaCentroCostos = resguardoEquipoFactoryServices.getResguardoEquipoFactoryServices().GetCentrosCostos();
                //    var listaMaquinasSinResguardo = resguardoEquipoFactoryServices.getResguardoEquipoFactoryServices().getEquipoSinResguardo(cc);
                //    var data = listaMaquinasSinResguardo.Select(x => new
                //    {
                //        id = x.id,
                //        Economico = x.noEconomico,
                //        CCName = listaCentroCostos.Where(y => y.cc == x.centro_costos).Select(z => z.descripcion).FirstOrDefault(),
                //        UsuarioSolicitud = "",
                //        //Grupo = x.grupoMaquinaria.descripcion,
                //        Vencimiento = 0,
                //        fechaCaptura = "",
                //        //documentos = infoDocumentos(x.id),
                //        tieneResguardo = false,
                //        resguardoFirmadoId = 0,
                //        resguardoFirmadoUrl = ""
                //    }).ToList();

                //    result.Add("listAutorizaciones", data);
                //}

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private bool VerificarCantResguardos(List<dynamic> p_lstDocumentos, int p_idResguardo)
        {
            #region SE VERIFICA SI EL USUARIO DEL RESGUARDO, CUENTA CON UN RESGUARDO YA CARGADO. (ARCHIVO TIPO 7)
            bool tieneResguardo = false;
            if (p_lstDocumentos.Count() > 0)
            {
                int cantResguardos = p_lstDocumentos.Where(w => w.ResguardoID == p_idResguardo).Count();
                if (cantResguardos > 0)
                    tieneResguardo = true;
            }
            return tieneResguardo;
            #endregion
        }

        private List<documentosResguardoDTO> infoDocumentos(int economicoID)
        {
            var documentos = documentosMaquinariaFactoryServices
                            .getDocumentosMaquinariaFactoryServices()
                            .listaDocumentos(economicoID)
                            .Where(x => x.tipoArchivo >= 3 && x.tipoArchivo <= 5);
            List<documentosResguardoDTO> result = new List<documentosResguardoDTO>();

            foreach (var d in documentos)
            {
                documentosResguardoDTO obj = new documentosResguardoDTO();
                obj.existe = true;
                obj.idDocumento = d.id;
                obj.tipoDocumento = d.tipoArchivo;
                result.Add(obj);
            }
            return result;
        }

        [HttpPost]
        public ActionResult SaveSendEspecial()
        {
            var result = new Dictionary<string, object>();
            try
            {
                DateTime fecha = DateTime.Now;
                string FileName = "";
                string ruta = "";
                string f = fecha.ToString("ddMMyyyy") + fecha.Hour + "" + fecha.Minute;
                bool pathExist = false;
                //format fecha
                tblM_ResguardoVehiculosServicio obj = new tblM_ResguardoVehiculosServicio();

                obj = JsonConvert.DeserializeObject<tblM_ResguardoVehiculosServicio>(Request.Form["obj"], new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                var datacheckList = JsonConvert.DeserializeObject<List<tblM_RespuestaResguardoVehiculos>>(Request.Form["datacheckList"]);
                var getEconomico = maquinaFactoryServices.getMaquinaServices().GetMaquinaByID(obj.MaquinariaID);
                string economicoNombre = "";
                if (getEconomico.Count > 0)
                {
                    obj.Obra = getEconomico.FirstOrDefault().centro_costos;
                    economicoNombre = getEconomico.First().noEconomico;
                }

                var TipoResguardo = JsonConvert.DeserializeObject<int>(Request.Form["TipoResguardo"]);

                if (TipoResguardo == 2)
                {
                    obj = resguardoEquipoFactoryServices.getResguardoEquipoFactoryServices().getResguardoBYID(obj.id);
                    obj.estado = 3;
                }
                else
                {
                    obj.estado = 1;
                }

                obj.Fecha = fecha;
                resguardoEquipoFactoryServices.getResguardoEquipoFactoryServices().GuardarResguardoVehiculos(obj);
                resguardoEquipoFactoryServices.getResguardoEquipoFactoryServices().NotificarCoordinadorSSOMA(obj.Obra, obj.id, economicoNombre);
                HttpPostedFileBase file1 = Request.Files["fLicenciaConducir"];
                HttpPostedFileBase file2 = Request.Files["fTarjetaCirculacion"];
                HttpPostedFileBase file3 = Request.Files["fPolizaSeguro"];
                HttpPostedFileBase file4 = Request.Files["fcheckAsignacion"];
                HttpPostedFileBase file5 = Request.Files["fFormatoMMtoPreventivo"];
                HttpPostedFileBase file6 = Request.Files["fPermisoCarga"];
                HttpPostedFileBase file7 = Request.Files["fcheckLiberacion"];

                tblM_RespuestaResguardoVehiculos objLicencia = new tblM_RespuestaResguardoVehiculos();

                objLicencia.RespuestaID = 52;
                objLicencia.HasDocumento = 0;
                objLicencia.Bueno = 0;
                objLicencia.Malo = 0;
                objLicencia.NA = 0;
                objLicencia.Regular = 0;

                tblM_RespuestaResguardoVehiculos tarjetaCirculacion = new tblM_RespuestaResguardoVehiculos();
                tarjetaCirculacion.RespuestaID = 44;
                tarjetaCirculacion.Bueno = 0;
                tarjetaCirculacion.Malo = 0;
                tarjetaCirculacion.NA = 0;
                tarjetaCirculacion.Regular = 0;

                tblM_RespuestaResguardoVehiculos PolizaSeguro = new tblM_RespuestaResguardoVehiculos();
                PolizaSeguro.RespuestaID = 45;
                PolizaSeguro.Bueno = 0;
                PolizaSeguro.Malo = 0;
                PolizaSeguro.NA = 0;
                PolizaSeguro.Regular = 0;

                tblM_RespuestaResguardoVehiculos ChekLiberacion = new tblM_RespuestaResguardoVehiculos();
                PolizaSeguro.RespuestaID = 46;
                PolizaSeguro.Bueno = 0;
                PolizaSeguro.Malo = 0;
                PolizaSeguro.NA = 0;
                PolizaSeguro.Regular = 0;

                tblM_RespuestaResguardoVehiculos ProgramaMantenimiento = new tblM_RespuestaResguardoVehiculos();
                ProgramaMantenimiento.RespuestaID = 47;
                ProgramaMantenimiento.Bueno = 0;
                ProgramaMantenimiento.Malo = 0;
                ProgramaMantenimiento.NA = 0;
                ProgramaMantenimiento.Regular = 0;

                if (file1 != null && file1.ContentLength > 0)
                {

                    objLicencia.HasDocumento = 1;
                    FileName = file1.FileName;
                    ruta = archivofs.getArchivo().getUrlDelServidor(3) + f + FileName;
                    pathExist = GuardarDocumentos(file1, ruta);

                    if (pathExist)
                    {
                        tblM_DocumentosResguardos objDoc = new tblM_DocumentosResguardos();
                        objDoc.nombreArchivo = FileName;
                        objDoc.nombreRuta = ruta;
                        objDoc.tipoArchivo = 9;
                        objDoc.ResguardoID = obj.id;
                        objDoc.fechaSubido = DateTime.Now;
                        objDoc.tipoResguardo = TipoResguardo;
                        documentosResguardoFactoryServices.getDocumentosResguardoFactoryServices().Guardar(objDoc);
                    }
                }

                datacheckList.Add(objLicencia);
                //Guarda la tarjeta de circulacion
                if (file2 != null && file2.ContentLength > 0)
                {
                    tarjetaCirculacion.HasDocumento = 1;

                    FileName = file2.FileName;
                    ruta = archivofs.getArchivo().getUrlDelServidor(3) + f + FileName;
                    pathExist = GuardarDocumentos(file2, ruta);

                    if (pathExist)
                    {
                        tblM_DocumentosMaquinaria tarjetaCiculacionSave = new tblM_DocumentosMaquinaria();
                        tblM_DocumentosResguardos objDoc = new tblM_DocumentosResguardos();
                        objDoc.nombreArchivo = FileName;
                        objDoc.nombreRuta = ruta;
                        objDoc.tipoArchivo = 2;
                        objDoc.ResguardoID = obj.id;
                        objDoc.fechaSubido = DateTime.Now;
                        objDoc.tipoResguardo = TipoResguardo;
                        documentosResguardoFactoryServices.getDocumentosResguardoFactoryServices().Guardar(objDoc);
                        int tipoArchivo = 4;
                        documentosMaquinariaFactoryServices.getDocumentosMaquinariaFactoryServices().Guardar(guardarDocumento(0, FileName, ruta, tipoArchivo, getUsuario().id, getEconomico.FirstOrDefault().id, DateTime.Now));
                    }
                }
                datacheckList.Add(tarjetaCirculacion);
                //Guarda La poliza de seguro
                if (file3 != null && file3.ContentLength > 0)
                {
                    PolizaSeguro.HasDocumento = 1;
                    FileName = file3.FileName;
                    ruta = archivofs.getArchivo().getUrlDelServidor(3) + f + FileName;
                    pathExist = GuardarDocumentos(file3, ruta);

                    if (pathExist)
                    {
                        tblM_DocumentosResguardos objDoc = new tblM_DocumentosResguardos();
                        objDoc.nombreArchivo = FileName;
                        objDoc.nombreRuta = ruta;
                        objDoc.tipoArchivo = 3;
                        objDoc.ResguardoID = obj.id;
                        objDoc.fechaSubido = DateTime.Now;
                        objDoc.tipoResguardo = TipoResguardo;
                        documentosResguardoFactoryServices.getDocumentosResguardoFactoryServices().Guardar(objDoc);
                        int tipoArchivo = 3;
                        documentosMaquinariaFactoryServices.getDocumentosMaquinariaFactoryServices().Guardar(guardarDocumento(0, FileName, ruta, tipoArchivo, getUsuario().id, getEconomico.FirstOrDefault().id, DateTime.Now));
                    }
                }

                datacheckList.Add(PolizaSeguro);
                if (file4 != null && file4.ContentLength > 0)
                {
                    FileName = file4.FileName;
                    ruta = archivofs.getArchivo().getUrlDelServidor(3) + f + FileName;
                    pathExist = GuardarDocumentos(file4, ruta);

                    if (pathExist)
                    {
                        tblM_DocumentosResguardos objDoc = new tblM_DocumentosResguardos();
                        objDoc.nombreArchivo = FileName;
                        objDoc.nombreRuta = ruta;
                        objDoc.tipoArchivo = 4;
                        objDoc.ResguardoID = obj.id;
                        objDoc.fechaSubido = DateTime.Now;
                        objDoc.tipoResguardo = TipoResguardo;
                        documentosResguardoFactoryServices.getDocumentosResguardoFactoryServices().Guardar(objDoc);
                    }
                }
                if (file5 != null && file5.ContentLength > 0)
                {
                    FileName = file5.FileName;
                    ruta = archivofs.getArchivo().getUrlDelServidor(3) + f + FileName;
                    pathExist = GuardarDocumentos(file5, ruta);


                    if (pathExist)
                    {
                        tblM_DocumentosResguardos objDoc = new tblM_DocumentosResguardos();
                        objDoc.nombreArchivo = FileName;
                        objDoc.nombreRuta = ruta;
                        objDoc.tipoArchivo = 5;
                        objDoc.ResguardoID = obj.id;
                        objDoc.fechaSubido = DateTime.Now;
                        objDoc.tipoResguardo = TipoResguardo;
                        documentosResguardoFactoryServices.getDocumentosResguardoFactoryServices().Guardar(objDoc);
                    }
                }
                if (file6 != null && file6.ContentLength > 0)
                {
                    ProgramaMantenimiento.HasDocumento = 1;
                    FileName = file6.FileName;
                    ruta = archivofs.getArchivo().getUrlDelServidor(3) + f + FileName;
                    pathExist = GuardarDocumentos(file6, ruta);
                    if (pathExist)
                    {
                        tblM_DocumentosResguardos objDoc = new tblM_DocumentosResguardos();
                        objDoc.nombreArchivo = FileName;
                        objDoc.nombreRuta = ruta;
                        objDoc.tipoArchivo = 6;
                        objDoc.ResguardoID = obj.id;
                        objDoc.fechaSubido = DateTime.Now;
                        objDoc.tipoResguardo = TipoResguardo;
                        documentosResguardoFactoryServices.getDocumentosResguardoFactoryServices().Guardar(objDoc);
                        int tipoArchivo = 5;
                        documentosMaquinariaFactoryServices.getDocumentosMaquinariaFactoryServices().Guardar(guardarDocumento(0, FileName, ruta, tipoArchivo, getUsuario().id, getEconomico.FirstOrDefault().id, DateTime.Now));
                    }
                }

                if (file7 != null && file7.ContentLength > 0)
                {
                    ProgramaMantenimiento.HasDocumento = 1;
                    FileName = file7.FileName;
                    ruta = archivofs.getArchivo().getUrlDelServidor(3) + f + FileName;
                    pathExist = GuardarDocumentos(file7, ruta);
                    if (pathExist)
                    {
                        tblM_DocumentosResguardos objDoc = new tblM_DocumentosResguardos();
                        objDoc.nombreArchivo = FileName;
                        objDoc.nombreRuta = ruta;
                        objDoc.tipoArchivo = 7;
                        objDoc.ResguardoID = obj.id;
                        objDoc.fechaSubido = DateTime.Now;
                        objDoc.tipoResguardo = TipoResguardo;
                        documentosResguardoFactoryServices.getDocumentosResguardoFactoryServices().Guardar(objDoc);
                    }
                }

                datacheckList.Add(ProgramaMantenimiento);

                resguardoEquipoFactoryServices.getResguardoEquipoFactoryServices().GuardarResguardoVehiculos(obj);

                foreach (var item in datacheckList)
                {
                    item.ResguardoID = obj.id;
                }

                respuestaResguardoVehiculosFactoryServices.getRespuestaResguardoVehiculosServices().Guardar(datacheckList);
                if (TipoResguardo == 1)
                {

                    List<int> listaPuestos = new List<int>();
                    listaPuestos.Add(57);

                    var usuarioAsignan = usuarioFactoryServices.getUsuarioService().GetUsuariosByPuesto(listaPuestos);
                    List<int> listaPuestos2 = new List<int>();
                    listaPuestos2.Add(65);
                    var usuarioSeguridad = usuarioFactoryServices.getUsuarioService().GetUsuariosByPuesto(listaPuestos2).FirstOrDefault();


                    tblM_AutorizacionResguardo objAutorizacion = new tblM_AutorizacionResguardo();

                    objAutorizacion.id = 0;
                    objAutorizacion.fechaRegistro = DateTime.Now;
                    objAutorizacion.estatus = false;
                    objAutorizacion.ResguardoVehiculoID = obj.id;
                    objAutorizacion.usuarioElaboroFirma = obj.id + f + "" + base.getUsuario().id + "A";
                    objAutorizacion.usuarioElaboroID = base.getUsuario().id;
                    objAutorizacion.usuarioReguardoIDEK = obj.noEmpleado;
                    objAutorizacion.usuarioSeguridadFirma = "";
                    objAutorizacion.usuarioSeguridadID = 0;

                    autorizacionResguardoFactoryServices.getAutorizacionResguardoFactoryServices().Guardar(objAutorizacion);

                }
                result.Add("idControl", obj.id);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private tblM_DocumentosMaquinaria guardarDocumento(int id, string FileName, string ruta, int tipoArchivo, int usuarioID, int economicoID, DateTime dateTime)
        {
            tblM_DocumentosMaquinaria objSave = new tblM_DocumentosMaquinaria();
            objSave.id = id;
            objSave.nombreArchivo = FileName;
            objSave.nombreRuta = ruta;
            objSave.tipoArchivo = tipoArchivo;
            objSave.usuarioSubeArchivo = usuarioID;
            objSave.economicoID = economicoID;
            objSave.fechaCarga = DateTime.Now;
            return objSave;
        }

        [HttpPost]
        public ActionResult SubirArchivoResguardo()
        {
            var result = new Dictionary<string, object>();
            try
            {
                DateTime fecha = DateTime.Now;
                string FileName = "";
                string ruta = "";
                bool pathExist = false;
                string f = fecha.ToString("ddMMyyyy") + fecha.Hour + "" + fecha.Minute;

                var idResguardo = JsonConvert.DeserializeObject<int>(Request.Form["idResguardo"]);
                var obj = resguardoEquipoFactoryServices.getResguardoEquipoFactoryServices().getResguardoBYID(idResguardo);

                HttpPostedFileBase file1 = Request.Files["fResguardoFirmado"];
                HttpPostedFileBase file2 = Request.Files["fAnexos"];

                if (file1 != null && file1.ContentLength > 0)
                {
                    FileName = file1.FileName;

                    ruta = archivofs.getArchivo().getUrlDelServidor(3) + f + FileName;
                    pathExist = GuardarDocumentos(file1, ruta);

                    if (pathExist)
                    {
                        tblM_DocumentosResguardos objDoc = new tblM_DocumentosResguardos();
                        objDoc.nombreArchivo = FileName;
                        objDoc.nombreRuta = ruta;
                        objDoc.tipoArchivo = 7;
                        objDoc.ResguardoID = obj.id;
                        objDoc.fechaSubido = DateTime.Now;
                        objDoc.tipoResguardo = 1;
                        documentosResguardoFactoryServices.getDocumentosResguardoFactoryServices().Guardar(objDoc);
                    }
                }

                if (file2 != null && file2.ContentLength > 0)
                {
                    FileName = file2.FileName;

                    ruta = archivofs.getArchivo().getUrlDelServidor(3) + f + FileName;
                    pathExist = GuardarDocumentos(file2, ruta);

                    if (pathExist)
                    {
                        tblM_DocumentosResguardos objDoc = new tblM_DocumentosResguardos();
                        objDoc.nombreArchivo = FileName;
                        objDoc.nombreRuta = ruta;
                        objDoc.tipoArchivo = 8;
                        objDoc.ResguardoID = obj.id;
                        objDoc.fechaSubido = DateTime.Now;
                        objDoc.tipoResguardo = 1;
                        documentosResguardoFactoryServices.getDocumentosResguardoFactoryServices().Guardar(objDoc);
                    }
                }

                // resguardoEquipoFactoryServices.getResguardoEquipoFactoryServices().GuardarResguardoVehiculos(obj);


                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
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

#if DEBUG
            ruta = ruta.Replace("\\\\REPOSITORIO\\", "C:\\");
#endif

            System.IO.File.WriteAllBytes(ruta, data);
            result = System.IO.File.Exists(ruta);
            return result;
        }


        //raguilar 11/04/18 
        public FileResult getFileDownload(int id)
        {
            var a = id;
            var archivo = documentosResguardoFactoryServices.getDocumentosResguardoFactoryServices().GetObjRutaDocumentobyID(id);
            var nombre = " ";
            var Ruta = " ";
            if (archivo != null)
            {
                nombre = archivo.nombreArchivo;
                Ruta = archivo.nombreRuta;
            }

            return File(Ruta, "multipart/form-data", nombre);
        }

        public FileResult getFileDownloadGeneral(int id)
        {
            var nombre = "";
            var Ruta = "";

            var archivo = documentosResguardoFactoryServices.getDocumentosResguardoFactoryServices().GetObjRutaDocumentoByIDGeneral(id);
            if (archivo != null)
            {
                nombre = archivo.nombreArchivo;
                Ruta = archivo.nombreRuta;
            }

            return File(Ruta, "multipart/form-data", nombre);
        }



        public ActionResult GetReporte(int obj)
        {
            var result = new Dictionary<string, object>();
            try
            {

                AutorizacionResguardoDTO SetDato = new AutorizacionResguardoDTO();
                AutorizantesDTO autorizantesDTO = new AutorizantesDTO();

                Session["rptAutorizantesDTO"] = null;
                Session["rptAutoriacionResguardos"] = null;

                var objSolicitud = autorizacionResguardoFactoryServices.getAutorizacionResguardoFactoryServices().GetObjAutorizaciones(obj);
                var objResguardoVehiculo = resguardoEquipoFactoryServices.getResguardoEquipoFactoryServices().getResguardoBYID(obj);
                var GetFirmas = autorizacionResguardoFactoryServices.getAutorizacionResguardoFactoryServices().GetObjAutorizaciones(obj);
                if (objResguardoVehiculo != null)
                {
                    var ObjMaquina = maquinaFactoryServices.getMaquinaServices().GetMaquinaByID(objResguardoVehiculo.MaquinariaID).FirstOrDefault();

                    SetDato.NombreEmpleado = objResguardoVehiculo.nombEmpleado;
                    SetDato.Puesto = objResguardoVehiculo.Puesto;
                    try
                    {
                        SetDato.Obra = centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(objResguardoVehiculo.Obra);
                    }
                    catch (Exception)
                    {
                        SetDato.Obra = "";
                    }

                    if (ObjMaquina != null)
                    {
                        SetDato.Descripcion = ObjMaquina.grupoMaquinaria.descripcion;
                        SetDato.Encierro = objResguardoVehiculo.TipoEncierro == 1 ? "A" : ObjMaquina.tipoEncierro == 2 ? "B" : "C";
                        SetDato.NoEconomico = ObjMaquina.noEconomico;
                        SetDato.Serie = ObjMaquina.noSerie;
                        SetDato.Modelo = ObjMaquina.modeloEquipo.descripcion;
                        SetDato.Marca = ObjMaquina.marca.descripcion;
                        SetDato.noPlaca = ObjMaquina.placas;
                        SetDato.KM = objResguardoVehiculo.Kilometraje;
                        SetDato.Comentario = objResguardoVehiculo.Comentario;
                        SetDato.FechaResguardo = objResguardoVehiculo.Fecha.ToShortDateString();
                    }
                    if (GetFirmas != null)
                    {
                        autorizantesDTO.ElaboraCadena = GetFirmas.usuarioElaboroFirma;
                        var Elabora = usuarioFactoryServices.getUsuarioService().ListUsersById(GetFirmas.usuarioElaboroID).FirstOrDefault();
                        autorizantesDTO.ElaboraNombre = Elabora.nombre + " " + Elabora.apellidoPaterno;
                        if (GetFirmas.usuarioSeguridadID != 0)
                        {
                            var Seguridad = usuarioFactoryServices.getUsuarioService().ListUsersById(GetFirmas.usuarioSeguridadID).FirstOrDefault();
                            autorizantesDTO.SeguridadNombre = Seguridad.nombre + " " + Seguridad.apellidoPaterno;
                        }
                        else
                        {
                            autorizantesDTO.SeguridadNombre = "Seguridad y Medio Ambiente";
                        }

                        //  var Empleado = resguardoEquipoFactoryServices.getResguardoEquipoFactoryServices().getCatEmpleado(GetFirmas.usuarioReguardoIDEK.ToString());

                        autorizantesDTO.nombreResguardo = objResguardoVehiculo.nombEmpleado;
                        autorizantesDTO.SeguridadCadena = GetFirmas.usuarioSeguridadFirma;
                    }


                }
                Session["rptAutorizantesDTO"] = autorizantesDTO;
                Session["rptAutoriacionResguardos"] = SetDato;

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
                var usuarioTemp = getUsuario();
                var raw = autorizacionResguardoFactoryServices.getAutorizacionResguardoFactoryServices().GetObjAutorizaciones(obj);

                int usuarioActual = base.getUsuario().id;
                string AutorizadorActual = "";

                if (raw.usuarioElaboroID.Equals(usuarioActual))
                {
                    AutorizadorActual = "Elaboro";
                }
                else if (raw.usuarioSeguridadID == 0 ? true : raw.usuarioSeguridadID.Equals(usuarioActual))
                {
                    if (usuarioTemp.departamento.id == 13)
                    {
                        AutorizadorActual = "Seguridad";
                    }
                    else
                    {
                        /*Quitar cuando se acepte la modificacion.*/
                        AutorizadorActual = "Seguridad";
                    }

                }
                var AutorizadorElabora = usuarioFactoryServices.getUsuarioService().ListUsersById(raw.usuarioElaboroID).Select(x => new AutorizadoresDTO
                {
                    idUsuario = x.id,
                    nombreUsuario = x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno,
                    firmaCadena = raw.usuarioSeguridadFirma,
                    firma = true
                }).FirstOrDefault();


                // if (raw.usuarioSeguridadID != 0) quitar cuando se acepte la modificacion. 
                {

                    if (usuarioTemp.departamento.id == 13)
                    {


                        var AutorizadorSeguridad = usuarioFactoryServices.getUsuarioService().ListUsersById(usuarioTemp.id).Select(x => new AutorizadoresDTO
                        {
                            idUsuario = x.id,
                            nombreUsuario = x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno,
                            firma = raw.estatus,
                            firmaCadena = raw.usuarioSeguridadFirma
                        }).FirstOrDefault();

                        result.Add("AutorizadorSeguridad", AutorizadorSeguridad);
                    }
                    else
                    {
                        AutorizadoresDTO AutorizadorSeguridad = new AutorizadoresDTO();

                        AutorizadorSeguridad.idUsuario = 0;
                        AutorizadorSeguridad.nombreUsuario = "Seguridad y Medio Ambiente";
                        AutorizadorSeguridad.firma = false;

                        result.Add("AutorizadorSeguridad", AutorizadorSeguridad);
                    }
                }
                result.Add("AutorizadorActual", AutorizadorActual);
                result.Add("AutorizadorElabora", AutorizadorElabora);
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

        public ActionResult GetSingleUsuario(string id)
        {
            var result = new Dictionary<string, object>();
            try
            {

                if (id != "89898989")
                {
                    var items = resguardoEquipoFactoryServices.getResguardoEquipoFactoryServices().getCatEmpleado(id);
                    var CentroCostos = centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(items.cc);
                    string txtPuesto = "";
                    if (items != null)
                    {
                        var puesto = items.Puesto.Split(' ');
                        for (int i = 0; i < puesto.Length; i++)
                        {
                            if (puesto[i].Length != 1)
                            {
                                txtPuesto += puesto[i] + " ";
                            }
                        }
                    }
                    result.Add("Puesto", txtPuesto);
                    result.Add("CCEmpleado", items.cc);
                    result.Add("Centro_Costos", CentroCostos);
                    result.Add("CvePuesto", items.cvePuesto);
                }
                else
                {
                    result.Add("Puesto", "GERENTE");
                    result.Add("CCEmpleado", "070");
                    result.Add("Centro_Costos", "CANAL DE DESCARGA");
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

        public ActionResult LoadInfoResguardo(int resguardoID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var objResguardo = resguardoEquipoFactoryServices.getResguardoEquipoFactoryServices().getResguardoBYID(resguardoID);
                var nameCC = centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(objResguardo.Obra);
                result.Add("nameCC", nameCC);
                result.Add("objResguardo", objResguardo);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getEquiposSinResguardo(string noAC)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var dataRaw = resguardoEquipoFactoryServices.getResguardoEquipoFactoryServices().getEquipoSinResguardo(noAC);
                var data = dataRaw.Select(x => new
                {
                    id = x.id,
                    economico = x.noEconomico,
                    grupo = x.grupoMaquinaria.descripcion,
                    modelo = x.modeloEquipo.descripcion,
                    ccActual = x.centro_costos + " - " + centroCostosFactoryServices.getCentroCostosService().getNombreCcFromSIGOPLAN(x.centro_costos)
                }).ToList();
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
        public ActionResult GetDatosAutorizacion(int obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var usuarioTemp = getUsuario();
                var raw = autorizacionResguardoFactoryServices.getAutorizacionResguardoFactoryServices().GetObjAutorizaciones(obj);

                int usuarioActual = base.getUsuario().id;
                string AutorizadorActual = "";

                if (raw.usuarioElaboroID.Equals(usuarioActual))
                {
                    AutorizadorActual = "Elaboro";
                }
                else if (raw.usuarioSeguridadID == 0 ? true : raw.usuarioSeguridadID.Equals(usuarioActual))
                {
                    if (usuarioTemp.departamento.id == 13)
                    {
                        AutorizadorActual = "Seguridad";
                    }
                    else
                    {
                        /*Quitar cuando se acepte la modificacion.*/
                        AutorizadorActual = "Seguridad";
                    }

                }
                var AutorizadorElabora = usuarioFactoryServices.getUsuarioService().ListUsersById(raw.usuarioElaboroID).Select(x => new AutorizadoresDTO
                {
                    idUsuario = x.id,
                    nombreUsuario = x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno,
                    firmaCadena = raw.usuarioSeguridadFirma,
                    firma = true
                }).FirstOrDefault();


                // if (raw.usuarioSeguridadID != 0) quitar cuando se acepte la modificacion. 
                {

                    if (usuarioTemp.departamento.id == 13 || getUsuario().id == 79467) //NIKOLAY PERU) 
                    {
                        if (getUsuario().id == 79467)
                            usuarioTemp.id = 79467;

                        var AutorizadorSeguridad = usuarioFactoryServices.getUsuarioService().ListUsersById(usuarioTemp.id).Select(x => new AutorizadoresDTO
                        {
                            idUsuario = x.id,
                            nombreUsuario = x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno,
                            firma = raw.estatus,
                            firmaCadena = raw.usuarioSeguridadFirma
                        }).FirstOrDefault();

                        result.Add("AutorizadorSeguridad", AutorizadorSeguridad);
                    }
                    else
                    {
                        AutorizadoresDTO AutorizadorSeguridad = new AutorizadoresDTO();

                        AutorizadorSeguridad.idUsuario = 0;
                        AutorizadorSeguridad.nombreUsuario = "Seguridad y Medio Ambiente";
                        AutorizadorSeguridad.firma = false;

                        result.Add("AutorizadorSeguridad", AutorizadorSeguridad);
                    }
                }
                result.Add("AutorizadorActual", AutorizadorActual);
                result.Add("AutorizadorElabora", AutorizadorElabora);
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

    }
}