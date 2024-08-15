using Core.DAO.Administracion.Seguridad;
using Core.DTO;
using Core.DTO.Administracion.Cotnratistas;
using Core.DTO.Administracion.Seguridad.Indicadores;
using Core.DTO.Principal.Generales;
using Core.Entity.Administrativo.Seguridad.Indicadores;
using Core.Enum.Administracion.Seguridad.Indicadores;
using Core.Service.Administracion.Seguridad;
using Data.Factory.Administracion.Contratistas;
using Data.Factory.Administracion.Seguridad.Capacitacion;
using Data.Factory.Administracion.Seguridad.Incidencias;
using Data.Factory.Principal.Usuarios;
using DotnetDaddy.DocumentViewer;
using Infrastructure.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Administrativo.Controllers.Seguridad
{
    public class IndicadoresSeguridadController : BaseController
    {
        private ICapacitacionDAO _capacitacionService;
        private SeguridadIncidentesFactoryService SeguridadIncidentesServices;
        private UsuarioFactoryServices usuarioFactoryServices;
        private EmpresasFactoryService Empresas;
        private EmpleadosFactoryService Empleados;
        Dictionary<string, object> result;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            vSesiones.sesionCapacitacionOperativa = vSesiones.sesionSistemaActual == 18 ? true : false;

            Empleados = new EmpleadosFactoryService();
            Empresas = new EmpresasFactoryService();
            _capacitacionService = new CapacitacionFactoryService().GetCapacitacionService();
            SeguridadIncidentesServices = new SeguridadIncidentesFactoryService();
            usuarioFactoryServices = new UsuarioFactoryServices();
            result = new Dictionary<string, object>();
            base.OnActionExecuting(filterContext);
        }

        // GET: Administrativo/IndicadoresSeguridad
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CapturaIncidente()
        {
            return View();
        }
        public ActionResult Dashboard()
        {
            return View();
        }
        public ActionResult CapturaColaboradores()
        {
            return View();
        }
        public ActionResult CapturaInformePreliminar()
        {
            return View();
        }

        #region CAPTURA INFORME PRELIMINAR
        public ActionResult GetDatosGeneralesIncidentes(int agrupacionID, int empresa, DateTime fechaInicio, DateTime fechaFin)
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().GetDatosGeneralesIncidentes(agrupacionID, empresa, fechaInicio, fechaFin);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetInformesPreliminares(List<int> listaDivisiones, List<int> listaLineasNegocio, int idAgrupacion, int idEmpresa, DateTime fechaInicio, DateTime fechaFin, int tipoAccidente = -1, int supervisor = -1, int departamento = -1, int estatus = -1)
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().getInformesPreliminares(listaDivisiones, listaLineasNegocio, idAgrupacion, idEmpresa, fechaInicio, fechaFin, tipoAccidente, supervisor, departamento, estatus);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetInformePreliminarByID(int id)
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().getInformePreliminarByID(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetUsuariosCCSigoPlan(int idEmpresa, int idAgrupacion)
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().getUsuariosCCSigoPlan(idEmpresa, idAgrupacion);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetFolio(string cc)
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().getFolio(cc);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetEvaluacionesRiesgo()
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().getEvaluacionesRiesgo();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarInforme(List<HttpPostedFileBase> evidencias)
        {
            var informe = JsonConvert.DeserializeObject<tblS_IncidentesInformePreliminar>(Request.Form["informe"], new IsoDateTimeConverter
            {
                DateTimeFormat = "dd/MM/yyyy hh:mm tt",
                Culture = System.Globalization.CultureInfo.InvariantCulture
            });
            var captura = new InformeDTO { informe = informe, evidencias = evidencias };

            result = SeguridadIncidentesServices.getSeguridadIncidenteService().guardarInforme(captura);

            Session["capturaInformePreliminar"] = captura;
            Session["flagEnviarCorreoInformePreliminarFormatoRIA"] = true;

            //#region Determinar si se envía correo.
            ////Incidente de riesgo crítico o intolerable.
            //if (captura.informe.riesgo == 6 || captura.informe.riesgo == 9)
            //{
            //    Session["flagEnviarCorreoInformePreliminarFormatoRIA"] = true;
            //}
            //else
            //{
            //    //Incidente de daños materiales (PD) clasificado como Mala Operación.
            //    if (captura.informe.tipoAccidente_id == 5 && captura.informe.subclasificacionID == 1)
            //    {
            //        Session["flagEnviarCorreoInformePreliminarFormatoRIA"] = true;
            //    }
            //    else
            //    {
            //        //Incidente de lesión al personal: FATAL, LTI, MTI, MDI, FAI.
            //        switch (captura.informe.tipoAccidente_id)
            //        {
            //            case 1:
            //            case 2:
            //            case 3:
            //            case 4:
            //            case 8:
            //                Session["flagEnviarCorreoInformePreliminarFormatoRIA"] = true;
            //                break;
            //            default:
            //                break;
            //        }
            //    }
            //}
            //#endregion

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateInforme(tblS_IncidentesInformePreliminar informe)
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().updateInforme(informe);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EnviarCorreo(int informe_id, List<int> usuarios)
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().enviarCorreo(informe_id, usuarios);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ObtenerEvidenciasInforme(int informeID)
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().ObtenerEvidenciasInforme(informeID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ObtenerEvidenciasRIA(int informeID)
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().ObtenerEvidenciasRIA(informeID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GuardarEvidencias(List<HttpPostedFileBase> evidencias, int informeID)
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().GuardarEvidencias(evidencias, informeID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DescargarEvidenciaInforme(int evidenciaID)
        {
            var resultadoTupla = SeguridadIncidentesServices.getSeguridadIncidenteService().DescargarEvidenciaInforme(evidenciaID);

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
                return View(RUTA_VISTA_ERROR_DESCARGA);
            }
        }

        [HttpGet]
        public ActionResult DescargarEvidenciaRIA(int evidenciaID)
        {
            var resultadoTupla = SeguridadIncidentesServices.getSeguridadIncidenteService().DescargarEvidenciaRIA(evidenciaID);

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
                return View(RUTA_VISTA_ERROR_DESCARGA);
            }
        }

        [HttpPost]
        public ActionResult EliminarEvidencia(int evidenciaID)
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().EliminarEvidencia(evidenciaID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CargarDatosEvidencia(int evidenciaID)
        {
            var resultado = new Dictionary<string, object>();

            resultado = SeguridadIncidentesServices.getSeguridadIncidenteService().CargarDatosEvidencia(evidenciaID);

            if (Convert.ToBoolean(resultado["success"]))
            {
                var bytesArchivo = resultado["archivo"] as byte[];
                var extension = resultado["extension"] as string;

                var fileData = Tuple.Create(bytesArchivo, extension);

                Session["archivoVisor"] = fileData;
            }
            else
            {
                Session["archivoVisor"] = null;
            }

            resultado.Remove("archivo");
            resultado.Remove("extension");

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CargarDatosEvidenciaRIA(int evidenciaID)
        {
            var resultado = new Dictionary<string, object>();

            resultado = SeguridadIncidentesServices.getSeguridadIncidenteService().CargarDatosEvidenciaRIA(evidenciaID);

            if (Convert.ToBoolean(resultado["success"]))
            {
                var bytesArchivo = resultado["archivo"] as byte[];
                var extension = resultado["extension"] as string;

                var fileData = Tuple.Create(bytesArchivo, extension);

                Session["archivoVisor"] = fileData;
            }
            else
            {
                Session["archivoVisor"] = null;
            }

            resultado.Remove("archivo");
            resultado.Remove("extension");

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult _visorGrid()
        {
            var viewer = new DocViewer { ID = "ctlDoc", IncludeJQuery = false, DebugMode = false, BasePath = "/", ResourcePath = "/", FitType = "", Zoom = 40, TimeOut = 20 };

            ViewBag.ViewerScripts = viewer.ReferenceScripts();
            ViewBag.ViewerCSS = viewer.ReferenceCss();
            ViewBag.ViewerID = viewer.ClientID;
            ViewBag.ViewerObject = viewer.JsObject;
            ViewBag.ViewerInit = viewer.GetAjaxInitArguments("");

            return PartialView();
        }

        [HttpPost]
        public ActionResult SubirReporteIncidente(HttpPostedFileBase archivo, int informeID, bool esRIA)
        {
            return Json(SeguridadIncidentesServices.getSeguridadIncidenteService().SubirReporteIncidente(archivo, informeID, esRIA), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DescargarReporte(int informeID, bool esRIA)
        {
            var resultadoTupla = SeguridadIncidentesServices.getSeguridadIncidenteService().DescargarReporte(informeID, esRIA);

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
                return View(RUTA_VISTA_ERROR_DESCARGA);
            }
        }

        [HttpPost]
        public ActionResult EliminarIncidente(int id)
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().EliminarIncidente(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region CAPTURA INCIDENTE
        public ActionResult GetTiposAccidentesList()
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().getTiposAccidentesList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSubclasificacionesAccidente()
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().GetSubclasificacionesAccidente();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getTipoProcedimientosVioladosList()
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().getTipoProcedimientosVioladosList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDepartamentosList()
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().getDepartamentosList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getSupervisoresList()
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().getSupervisoresList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getSupervisoresIncidentesList()
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().getSupervisoresIncidentesList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetTiposLesionList()
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().getTiposLesionList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPartesCuerposList()
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().getPartesCuerposList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetTiposContactoList()
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().getTiposContactoList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAgentesImplicadosList()
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().getAgentesImplicados();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetExperienciaEmpleadosList()
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().getExperienciaEmpleados();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAntiguedadEmpleadosList()
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().getAntiguedadEmpleados();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetTurnosEmpleadoList()
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().getTurnosEmpleado();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetTecnicasInvestigacionList()
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().getTecnicasInvestigacion();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetProtocolosTrabajoList()
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().getProtocolosTrabajoList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetEmpleadosContratistasList(int cc)
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().getEmpleadosContratistasList(cc);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LlenarComboCC()
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().obtenerCentrosCostos();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LlenarComboCCUsuario()
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().ObtenerCentrosCostosUsuario();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSubcontratistas()
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().getSubcontratistas();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetEmpleadosCCList(string cc)
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().getEmpleadosCCList(cc);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetInfoEmpleado(int claveEmpleado, bool esContratista, int idEmpresaContratista)
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().getInfoEmpleado(claveEmpleado, esContratista, idEmpresaContratista);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetInfoEmpleadoContratista(int empleado_id)
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().getInfoEmpleadoContratista(empleado_id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPrioridadesActividad()
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().getPrioridadesActividad();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetUsuarioSelectWithException(string term)
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().getUsersEnkontrol(term);
            return Json(result["empleadoInfo"], JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUsuarioSelectByClave(string clave)
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().getUsersEnkontrolByClave(clave);
            return Json(result["empleadoInfo"], JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarEmpleadoSubcontratista(tblS_IncidentesEmpleadosContratistas empleado)
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().guardarEmpleadoSubcontratista(empleado);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarIncidente(List<HttpPostedFileBase> evidencias, DateTime fechaAccidente, DateTime fechaJunta)
        {
            var incidente = JsonConvert.DeserializeObject<tblS_Incidentes>(Request.Form["incidente"], new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });

            //Se separaron las fechas y se asignaron aquí porque el deserializador causa problemas con los formatos que llevan horas.
            incidente.fechaAccidente = fechaAccidente;
            incidente.fechaJunta = fechaJunta;

            var grupoTrabajo = JsonConvert.DeserializeObject<List<tblS_IncidentesGrupoInvestigacion>>(Request.Form["grupoTrabajo"], new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            var ordenCronologico = JsonConvert.DeserializeObject<List<tblS_IncidentesOrdenCronologico>>(Request.Form["ordenCronologico"], new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            var eventoDetonador = JsonConvert.DeserializeObject<List<tblS_IncidentesEventoDetonador>>(Request.Form["eventosDetonador"], new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            var causasInmediatas = JsonConvert.DeserializeObject<List<tblS_IncidentesCausasInmediatas>>(Request.Form["causasInmediatas"], new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            var causasBasicas = JsonConvert.DeserializeObject<List<tblS_IncidentesCausasBasicas>>(Request.Form["causasBasicas"], new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            var causasRaiz = JsonConvert.DeserializeObject<List<tblS_IncidentesCausasRaiz>>(Request.Form["causasRaiz"], new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            var medidasControl = JsonConvert.DeserializeObject<List<tblS_IncidentesMedidasControl>>(Request.Form["medidasControl"], new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            var tipoEvidenciaRIA = JsonConvert.DeserializeObject<List<int>>(Request.Form["tipoEvidenciaRIA"]);

            var captura = new IncidenteDTO
            {
                incidente = incidente,
                grupoTrabajo = grupoTrabajo,
                ordenCronologico = ordenCronologico,
                eventoDetonador = eventoDetonador,
                causasInmediatas = causasInmediatas,
                causasBasicas = causasBasicas,
                causasRaiz = causasRaiz,
                medidasControl = medidasControl,
                evidencias = evidencias,
                tipoEvidenciaRIA = tipoEvidenciaRIA
            };

            result = SeguridadIncidentesServices.getSeguridadIncidenteService().guardarIncidente(captura);

            Session["capturaIncidente"] = captura;
            Session["flagEnviarCorreoIncidenteFormatoRIA"] = true;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboTipoEvidenciaRIA()
        {
            result.Add(ITEMS, GlobalUtils.ParseEnumToCombo<TipoEvidenciaRIAEnum>());
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ObtenerIncidentePorInformeID(int informeID)
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().ObtenerIncidentePorInformeID(informeID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUsuariosAutocomplete(string term)
        {
            var result = SeguridadIncidentesServices.getSeguridadIncidenteService().GetUsuariosAutocomplete(term);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LlenarComboEstatusIncidente()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cbo = EnumExtensions.ToCombo<EstatusIncidenteEnum>();
                result.Add(ITEMS, cbo);
                result.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }
        public ActionResult LlenarComboSupervisorIncidente()
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().LlenarComboSupervisorIncidente();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LlenarComboDepartamentoIncidente()
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().LlenarComboDepartamentoIncidente();
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region CAPTURA INFORMACION COLABORADORES
        public ActionResult GetInformacionColaboradores(int idAgrupacion, DateTime fechaInicio, DateTime fechaFin, int idEmpresa)
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().getInformacionColaboradores(idAgrupacion, fechaInicio, fechaFin, idEmpresa);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetInformacionColaboradoresByID(int id)
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().getInformacionColaboradoresByID(id);
            bool isSuccess = (bool)result["success"];
            if (isSuccess)
            {
                foreach (var item in SeguridadIncidentesServices.getSeguridadIncidenteService().getInformacionColaboradoresByIDDetalle(id))
                {
                    result.Add(item.Key, item.Value);
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetFechasUltimoCorte(int idEmpresa, int idAgrupacion)
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().getFechasUltimoCorte(idEmpresa, idAgrupacion);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarRegistroInformacion(
            tblS_IncidentesInformacionColaboradores registroInformacion,
            List<tblS_IncidentesInformacionColaboradoresDetalle> lstDetalle,
            List<tblS_IncidentesInformacionColaboradoresClasificacion> listaClasificacion
        )
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().GuardarRegistroInformacion(registroInformacion, lstDetalle, listaClasificacion);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateRegistroInformacion(
            tblS_IncidentesInformacionColaboradores registroInformacion,
            List<tblS_IncidentesInformacionColaboradoresDetalle> lstDetalle,
            List<tblS_IncidentesInformacionColaboradoresClasificacion> listaClasificacion
        )
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().UpdateRegistroInformacion(registroInformacion, lstDetalle, listaClasificacion);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EliminarHHT(int id)
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().EliminarHHT(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetClasificacionHHTCombo()
        {
            result.Add(ITEMS, GlobalUtils.ParseEnumToCombo<ClasificacionHHTEnum>());
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region DASHBOARD
        public ActionResult GetIncidentesRegistrables(busqDashboardDTO busq)
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().getIncidentesRegistrables(busq);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetIncidentesReportables(busqDashboardDTO busq)
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().getIncidentesReportables(busq);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetHorasHombreLostDay(busqDashboardDTO busq)
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().getHorasHombreLostDay(busq);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPotencialSeveridad(busqDashboardDTO busq)
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().getPotencialSeveridad(busq);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetIncidentesMes(busqDashboardDTO busq)
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().getIncidentesPorMes(busq);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetIncidentesRegistrablesXmesAnterior(busqDashboardDTO busq)
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().getIncidentesRegistrablesXmes(busq, TipoCargaGraficaEnum.Anterior);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetIncidentesRegistrablesXmesActual(busqDashboardDTO busq)
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().getIncidentesRegistrablesXmes(busq, TipoCargaGraficaEnum.Actual);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDanoInstalacionEquipo(busqDashboardDTO busq)
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().getDanoInstalacionEquipo(busq);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetIncidentesDepartamento(busqDashboardDTO busq)
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().getIncidentesDepartamento(busq);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetTasaIncidencias(busqDashboardDTO busq, TipoCargaGraficaEnum tipoCarga = TipoCargaGraficaEnum.Anterior)
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().getTasaIncidencias(busq, tipoCarga);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetTIFR(busqDashboardDTO busq, TipoCargaGraficaEnum tipoCarga = TipoCargaGraficaEnum.Anterior)
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().getTIFR(busq, tipoCarga);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetTPDFR(busqDashboardDTO busq, TipoCargaGraficaEnum tipoCarga = TipoCargaGraficaEnum.Anterior)
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().getTPDFR(busq, tipoCarga);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetIncidenciasPresentadas(busqDashboardDTO busq)
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().getIncidenciasPresentadas(busq);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetIncidenciasPresentadasTipo(string tipo, busqDashboardDTO busq)
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().getIncidenciasPresentadasTipo(tipo, busq);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAccidentabilidad(busqDashboardDTO busq)
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().getAccidentabilidad(busq);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAccidentabilidadTop(busqDashboardDTO busq)
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().getAccidentabilidadTop(busq);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCausasIncidencias(busqDashboardDTO busq)
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().getCausasIncidencias(busq);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTiposCargaTasaAnual()
        {
            result.Add(ITEMS, GlobalUtils.ParseEnumToCombo<TipoCargaGraficaEnum>());
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTiposGraficaMeta()
        {
            result.Add(ITEMS, GlobalUtils.ParseEnumToCombo<TipoGraficaEnum>());
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ObtenerMetasGrafica()
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().ObtenerMetasGrafica();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AgregarMetaGrafica(tblS_IncidentesMetasGrafica meta)
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().AgregarMetaGrafica(meta);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarMetaGrafica(int id)
        {
            result = SeguridadIncidentesServices.getSeguridadIncidenteService().EliminarMetaGrafica(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPermisoBotonMetas()
        {
            bool permisoBotonMetas = false;

            if (vSesiones.sesionUsuarioDTO.idPerfil == 1 || vSesiones.sesionUsuarioDTO.nombreUsuario == "diego.cardenas" || vSesiones.sesionUsuarioDTO.nombreUsuario == "jose.iribe" || vSesiones.sesionUsuarioDTO.nombreUsuario == "ruben.salguero" || vSesiones.sesionUsuarioDTO.nombreUsuario == "emanuel.montes")
            {
                permisoBotonMetas = true;
            }

            result.Add("permisoBotonMetas", permisoBotonMetas);
            result.Add(SUCCESS, true);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDatosLesionesPersonal(busqDashboardDTO busq)
        {
            return Json(SeguridadIncidentesServices.getSeguridadIncidenteService().GetDatosLesionesPersonal(busq), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDatosDañosMateriales(busqDashboardDTO busq)
        {
            return Json(SeguridadIncidentesServices.getSeguridadIncidenteService().GetDatosDañosMateriales(busq), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region "INDICADORES"
        public ActionResult Estadisticas()
        {
            return View();
        }
        #endregion

        #region Reporte Global
        public ActionResult EnviarCorreoReporteGlobal()
        {
            List<Byte[]> pdf;
            try
            {
                pdf = (List<Byte[]>)Session["downloadPDF"];
            }
            catch (Exception)
            {
                pdf = null;
            }
            finally
            {
                Session["downloadPDF"] = null;
            }

            if (pdf == null)
            {
                result.Add(SUCCESS, false);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(SeguridadIncidentesServices.getSeguridadIncidenteService().EnviarCorreoReporteGlobal(pdf), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region CALCULO DE HORAS TRABAJADAS - HORAS HOMBRE
        public ActionResult GetDatos(CalculosHorasHombreDTO objSelected)
        {
            return Json(SeguridadIncidentesServices.getSeguridadIncidenteService().GetDatos(objSelected), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ObtenerComboCCAmbasEmpresas(bool incContratista, int? division)
        {
            return Json(SeguridadIncidentesServices.getSeguridadIncidenteService().ObtenerComboCCAmbasEmpresas(incContratista, division), JsonRequestBehavior.AllowGet);
        }
        public ActionResult ObtenerComboCCAmbasEmpresas_SoloGrupos(bool incContratista, int? division)
        {
            return Json(SeguridadIncidentesServices.getSeguridadIncidenteService().ObtenerComboCCAmbasEmpresas_SoloGrupos(incContratista, division), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ObtenerComboCCAmbasEmpresasDivisionesLineas(bool incContratista, List<int> listaDivisiones, List<int> listaLineasNegocio)
        {
            return Json(SeguridadIncidentesServices.getSeguridadIncidenteService().ObtenerComboCCAmbasEmpresasDivisionesLineas(incContratista, listaDivisiones, listaLineasNegocio), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region CONTRATISTAS
        public ActionResult EmpresasEmpleados()
        {
            return View();
        }
        public ActionResult ObtenerEmpresasCombo()
        {
            try
            {
                List<ComboDTO> lstAgrupacion = Empresas.getEmpresasService().ObtenerEmpresasCombo();
                result.Add(ITEMS, lstAgrupacion);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ObtenerEmpresas(int nombreEmpresa, bool esActivo)
        {
            try
            {
                List<EmpresasDTO> lstEmpresas = Empresas.getEmpresasService().ObtenerEmpresas(nombreEmpresa, esActivo);
                result.Add(ITEMS, lstEmpresas);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AgregarEmpresa(EmpresasDTO _objEmpresa)
        {
            try
            {
                EmpresasDTO objEmpresa = Empresas.getEmpresasService().AgregarEmpresa(_objEmpresa);
                result.Add(ITEMS, objEmpresa);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditarEmpresa(EmpresasDTO _objEmpresa)
        {
            try
            {
                EmpresasDTO objEmpresa = Empresas.getEmpresasService().EditarEmpresa(_objEmpresa);
                result.Add(ITEMS, objEmpresa);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ActivarDesactivarEmpresa(int idEmpresa, bool esActivo)
        {
            try
            {
                EmpresasDTO objEmpresa = Empresas.getEmpresasService().ActivarDesactivarEmpresa(idEmpresa, esActivo);
                result.Add(ITEMS, objEmpresa);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ObtenerPais()
        {
            try
            {
                List<ComboDTO> lstAgrupacion = Empleados.getEmpleadosService().ObtenerPais();
                result.Add(ITEMS, lstAgrupacion);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ObtenerEstado(int idPais)
        {
            try
            {
                List<ComboDTO> lstAgrupacion = Empleados.getEmpleadosService().ObtenerEstado(idPais);
                result.Add(ITEMS, lstAgrupacion);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ObtenerMunicipio(int idEstado)
        {
            try
            {
                List<ComboDTO> lstAgrupacion = Empleados.getEmpleadosService().ObtenerMunicipio(idEstado);
                result.Add(ITEMS, lstAgrupacion);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CrearEditar(EmpleadosDTO _objEmpleados)
        {
            try
            {
                EmpleadosDTO objEmpleados = Empleados.getEmpleadosService().CrearEditar(_objEmpleados);
                result.Add(ITEMS, objEmpleados);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ActivarDesactivar(int id, bool esActivo)
        {
            try
            {
                EmpleadosDTO objEmpleados = Empleados.getEmpleadosService().ActivarDesactivar(id, esActivo);
                result.Add(ITEMS, objEmpleados);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getListadoDeEmpleados(int idEmpresa, DateTime FechaAlta, bool esActivo)
        {
            try
            {
                List<EmpleadosDTO> objEmpleados = Empleados.getEmpleadosService().getListadoDeEmpleados(idEmpresa, FechaAlta, esActivo);
                result.Add(ITEMS, objEmpleados);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CargaMasivaContratistas(HttpPostedFileBase archivo)
        {

            try
            {
                int idEmpresa = 0;
                var resultado = Empleados.getEmpleadosService().CargaMasivaContratistas(archivo, idEmpresa);
                result.Add(ITEMS, resultado);
                result.Add(SUCCESS, true);

            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);

                throw;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCC()
        {

            try
            {
                var resultado = _capacitacionService.ObtenerComboCCAmbasEmpresas();
                result.Add(ITEMS, resultado);
                result.Add(SUCCESS, true);

            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);

                throw;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ValidarAccesoContratista()
        {
            var result = new Dictionary<string, object>();
            try
            {
                bool esContratista = SeguridadIncidentesServices.getSeguridadIncidenteService().ValidarAccesoContratista();
                if (esContratista)
                    result.Add(SUCCESS, true);
                else
                    result.Add(SUCCESS, false);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region CATÁLOGO AGRUPACIÓN CONTRATISTAS
        public ActionResult AgrupacionContratistas()
        {
            return View();
        }

        public ActionResult GetAgrupacionesContratistas(IncidentesAgrupacionesContratistasDTO objFiltro)
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<IncidentesAgrupacionesContratistasDTO> lstAgrupaciones = SeguridadIncidentesServices.getSeguridadIncidenteService().GetAgrupacionesContratistas(objFiltro);
                result.Add("lstAgrupaciones", lstAgrupaciones);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearEditarAgrupacion(IncidentesAgrupacionesContratistasDTO objAgrupacion)
        {
            var result = new Dictionary<string, object>();
            try
            {
                if (objAgrupacion != null)
                {
                    bool disponibleContratistaEnAgrupacionNomAgrupacion = SeguridadIncidentesServices.getSeguridadIncidenteService().existeNomAgrupacion(objAgrupacion);
                    if (objAgrupacion.id > 0 /*&& disponibleContratistaEnAgrupacionNomAgrupacion*/)
                    {
                        bool esEditar = SeguridadIncidentesServices.getSeguridadIncidenteService().ActualizarAgrupacion(objAgrupacion);
                        if (esEditar)
                            result.Add(SUCCESS, true);
                        else
                            result.Add(SUCCESS, false);
                    }
                    else if (disponibleContratistaEnAgrupacionNomAgrupacion)
                    {
                        bool esCrear = SeguridadIncidentesServices.getSeguridadIncidenteService().CrearAgrupacion(objAgrupacion);
                        if (esCrear)
                            result.Add(SUCCESS, true);
                        else
                            result.Add(SUCCESS, false);
                    }
                }
            }
            catch (Exception ex)
            {
                result.Add(MESSAGE, ex.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarAgrupacion(int idAgrupacion)
        {
            var result = new Dictionary<string, object>();
            try
            {
                if (idAgrupacion > 0)
                {
                    bool EliminarAgrupacion = SeguridadIncidentesServices.getSeguridadIncidenteService().EliminarAgrupacion(idAgrupacion);
                    if (EliminarAgrupacion)
                        result.Add(SUCCESS, true);
                    else
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

        public ActionResult FillCboAgrupaciones()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cboAgrupaciones = SeguridadIncidentesServices.getSeguridadIncidenteService().FillCboAgrupaciones();
                result.Add(ITEMS, cboAgrupaciones);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetContratistas(int idAgrupacion)
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<IncidentesAgrupacionesContratistasDTO> lstContratistas = SeguridadIncidentesServices.getSeguridadIncidenteService().GetContratistas(idAgrupacion);
                result.Add("lstContratistas", lstContratistas);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboContratistas()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cboContratistas = SeguridadIncidentesServices.getSeguridadIncidenteService().FillCboContratistas();
                result.Add(ITEMS, cboContratistas);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearContratistaEnAgrupacion(IncidentesAgrupacionesContratistasDTO objAgrupacion)
        {
            var result = new Dictionary<string, object>();
            try
            {
                if (objAgrupacion != null && objAgrupacion.idAgruContratista > 0 && objAgrupacion.idContratista > 0)
                {
                    bool disponibleContratistaEnAgrupacion = SeguridadIncidentesServices.getSeguridadIncidenteService().existeContratistaEnAgrupacion(objAgrupacion);
                    if (disponibleContratistaEnAgrupacion)
                    {
                        bool esCrear = SeguridadIncidentesServices.getSeguridadIncidenteService().CrearContratistaEnAgrupacion(objAgrupacion);
                        if (esCrear)
                            result.Add(SUCCESS, true);
                        else
                            result.Add(SUCCESS, false);
                    }
                    else
                        throw new Exception("El contratista seleccionado, ya existe en la agrupación");
                }
                else
                    throw new Exception("Ocurrió un error al crear el contratista en la agrupación");
            }
            catch (Exception ex)
            {
                result.Add(MESSAGE, ex.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarContratistaEnAgrupacion(int idAgrupacionDet)
        {
            var result = new Dictionary<string, object>();
            try
            {
                if (idAgrupacionDet > 0)
                {
                    bool EliminarContratista = SeguridadIncidentesServices.getSeguridadIncidenteService().EliminarContratistaEnAgrupacion(idAgrupacionDet);
                    if (EliminarContratista)
                        result.Add(SUCCESS, true);
                    else
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
        #endregion

        #region CATÁLOGO RELACIÓN CONTRATISTA - EMPRESA
        public ActionResult EmpresaRelContratistas()
        {
            return View();
        }

        public ActionResult GetEmpresaRelContratistas(IncidentesRelEmpresaContratistasDTO objFiltro)
        {
            var result = new Dictionary<string, object>();
            try
            {
                List<IncidentesRelEmpresaContratistasDTO> lstContratistas = SeguridadIncidentesServices.getSeguridadIncidenteService().GetEmpresaRelContratistas(objFiltro);
                result.Add("lstContratistas", lstContratistas);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearEditarRelEmpresaContratista(IncidentesRelEmpresaContratistasDTO objRel)
        {
            var result = new Dictionary<string, object>();
            try
            {
                if (objRel != null)
                {
                    bool disponibleContratista = SeguridadIncidentesServices.getSeguridadIncidenteService().DisponibleRelEmpresaContratista(objRel);
                    if (disponibleContratista)
                    {
                        bool esCrear = SeguridadIncidentesServices.getSeguridadIncidenteService().CrearRelEmpresaContratista(objRel);
                        if (esCrear)
                            result.Add(SUCCESS, true);
                        else
                            result.Add(SUCCESS, false);
                    }
                    else
                        throw new Exception("Ya éxiste en esta empresa el contratista seleccionado.");
                }
            }
            catch (Exception ex)
            {
                result.Add(MESSAGE, ex.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarRelEmpresaContratista(int idRel)
        {
            var result = new Dictionary<string, object>();
            try
            {
                if (idRel > 0)
                {
                    bool EliminarRel = SeguridadIncidentesServices.getSeguridadIncidenteService().EliminarRelEmpresaContratista(idRel);
                    if (EliminarRel)
                        result.Add(SUCCESS, true);
                    else
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

        public ActionResult FillCboContratistasSP()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var cboContratistas = SeguridadIncidentesServices.getSeguridadIncidenteService().FillCboContratistasSP();
                result.Add(ITEMS, cboContratistas);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}