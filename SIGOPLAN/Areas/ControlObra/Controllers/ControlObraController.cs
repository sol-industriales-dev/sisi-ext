using Newtonsoft.Json;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Converters;
using Core.Entity.ControlObra;
using Data.Factory.ControlObra;
using Core.DAO.ControlObra;
using Core.DTO.ControlObra;
using Core.DAO.Principal.Usuarios;
using Data.Factory.Principal.Usuarios;
using System.Data.OleDb;
using System.Data;
using Core.DTO.ControlObra.Gestion;
using Core.Entity.ControlObra.GestionDeCambio;
using Infrastructure.Utils;
using Core.DTO.ControlObra.MatrizDeRiesgo;
using Core.Entity.ControlObra.MatrizDeRiesgo;
using Core.DTO.ControlObra.EvaluacionSubcontratista;

namespace SIGOPLAN.Areas.ControlObra.Controllers
{
    public class ControlObraController : BaseController
    {
        IUsuarioDAO usuarioService;
        private GestionDeProyectoFactoryService gestionDeProyectoFactoryService;
        private ControlObraFactoryService ControlObraFactoryService;
        private MatrizDeRiesgoFactoryService getMatrizDeRiesgoFactoryService;

        Dictionary<string, object> result;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            getMatrizDeRiesgoFactoryService = new MatrizDeRiesgoFactoryService();
            usuarioService = new UsuarioFactoryServices().getUsuarioService();
            ControlObraFactoryService = new ControlObraFactoryService();
            gestionDeProyectoFactoryService = new GestionDeProyectoFactoryService();
            result = new Dictionary<string, object>();
            base.OnActionExecuting(filterContext);
        }

        // GET: ControlObra/ControlObra
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CapitulosCatalogo()
        {
            return View();
        }
        public ActionResult SubCapitulosICatalogo()
        {
            return View();
        }
        public ActionResult SubCapitulosIICatalogo()
        {
            return View();
        }
        public ActionResult SubCapitulosIIICatalogo()
        {
            return View();
        }
        public ActionResult ActividadesCatalogo()
        {
            return View();
        }
        public ActionResult UnidadesCatalogo()
        {
            return View();
        }
        public ActionResult ActividadAvance()
        {
            return View();
        }
        public ActionResult ReporteAvances()
        {
            return View();
        }
        public ActionResult Autorizacion()
        {
            return View();
        }
        public ActionResult CapturaFacturado()
        {
            return View();
        }
        public ActionResult CapturaActividadesDiarias()
        {
            return View();
        }
        public ActionResult PlantillaInforme()
        {
            return View();
        }
        public ActionResult PlantillaInformeEditor()
        {
            return View();
        }
        public ActionResult InformeSemanal()
        {
            return View();
        }
        public ActionResult InformeSemanalGestor()
        {
            return View();
        }




        #region CAPITULOS
        public ActionResult GetCapitulosList()
        {
            result = ControlObraFactoryService.getControlObraService().getCapitulosList(0);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCapitulosCatalogo()
        {
            result = ControlObraFactoryService.getControlObraService().getCapitulosCatalogo();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCapitulo(int capituloID)
        {
            result = ControlObraFactoryService.getControlObraService().getCapitulo(capituloID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarCapitulo()
        {
            var capitulo = JsonConvert.DeserializeObject<tblCO_Capitulos>(Request.Form["capitulo"]);
            result = ControlObraFactoryService.getControlObraService().guardarCapitulo(capitulo);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateCapitulo(int capituloID, string capitulo, DateTime fechaInicio, DateTime fechaFin, int? cc_id, int? autorizante_id, int? periodoFacturacion)
        {
            result = ControlObraFactoryService.getControlObraService().updateCapitulo(capituloID, capitulo, fechaInicio, fechaFin, cc_id, autorizante_id, periodoFacturacion);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RemoveCapitulo(int capituloID)
        {
            result = ControlObraFactoryService.getControlObraService().removeCapitulo(capituloID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LlenarComboCC()
        {
            result = ControlObraFactoryService.getControlObraService().obtenerCentrosCostos();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LlenarComboAutorizante(string cc)
        {
            var usuarios = usuarioService.ListUsersAll().Where(x => x.cc == cc).Select(y => new
            {
                Value = y.id,
                Text = y.nombre + ' ' + y.apellidoPaterno + ' ' + y.apellidoMaterno
            }).ToList();

            if (usuarios.Count > 0)
            {
                result.Add("items", usuarios);
                result.Add(SUCCESS, true);
            }
            else
            {
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPeriodoFacturacion()
        {

            result = ControlObraFactoryService.getControlObraService().getPeriodoFacturacion();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region SUBCAPITULOS NIVEL I
        public ActionResult GetSubcapitulosN1List(int capituloID)
        {
            result = ControlObraFactoryService.getControlObraService().getSubcapitulosN1List(capituloID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSubcapitulosN1Catalogo(List<int> listCapitulosID)
        {
            result = ControlObraFactoryService.getControlObraService().getSubcapitulosN1Catalogo(listCapitulosID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSubcapituloN1(int subcapituloID)
        {
            result = ControlObraFactoryService.getControlObraService().getSubcapituloN1(subcapituloID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarSubcapituloN1()
        {
            var subcapitulo = JsonConvert.DeserializeObject<tblCO_Subcapitulos_Nivel1>(Request.Form["subcapituloN1"]);
            result = ControlObraFactoryService.getControlObraService().guardarSubcapituloN1(subcapitulo);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateSubcapituloN1(int subcapituloID, string subcapitulo, DateTime fechaInicio, DateTime fechaFin, int capituloID)
        {
            result = ControlObraFactoryService.getControlObraService().updateSubcapituloN1(subcapituloID, subcapitulo, fechaInicio, fechaFin, capituloID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RemoveSubcapituloN1(int subcapituloID)
        {
            result = ControlObraFactoryService.getControlObraService().removeSubcapituloN1(subcapituloID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region SUBCAPITULOS NIVEL II
        public ActionResult GetSubcapitulosN2List(int subcapituloN1_id)
        {
            result = ControlObraFactoryService.getControlObraService().getSubcapitulosN2List(subcapituloN1_id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSubcapitulosN2Catalogo(int subcapituloN1_id)
        {
            result = ControlObraFactoryService.getControlObraService().getSubcapitulosN2Catalogo(subcapituloN1_id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSubcapituloN2(int subcapituloID)
        {
            result = ControlObraFactoryService.getControlObraService().getSubcapituloN2(subcapituloID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarSubcapituloN2()
        {
            var subcapituloN2 = JsonConvert.DeserializeObject<tblCO_Subcapitulos_Nivel2>(Request.Form["subcapituloN2"]);
            result = ControlObraFactoryService.getControlObraService().guardarSubcapituloN2(subcapituloN2);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateSubcapituloN2(int subcapituloID, string subcapitulo, DateTime fechaInicio, DateTime fechaFin, int subcapituloN1_id)
        {
            result = ControlObraFactoryService.getControlObraService().updateSubcapituloN2(subcapituloID, subcapitulo, fechaInicio, fechaFin, subcapituloN1_id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RemoveSubcapituloN2(int subcapituloID)
        {
            result = ControlObraFactoryService.getControlObraService().removeSubcapituloN2(subcapituloID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region SUBCAPITULOS NIVEL III
        public ActionResult GetSubcapitulosN3List(int subcapituloN2_id)
        {
            result = ControlObraFactoryService.getControlObraService().getSubcapitulosN3List(subcapituloN2_id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSubcapitulosN3Catalogo(int subcapituloN2_id)
        {
            result = ControlObraFactoryService.getControlObraService().getSubcapitulosN3Catalogo(subcapituloN2_id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSubcapituloN3(int subcapituloID)
        {
            result = ControlObraFactoryService.getControlObraService().getSubcapituloN3(subcapituloID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarSubcapituloN3()
        {
            var subcapituloN3 = JsonConvert.DeserializeObject<tblCO_Subcapitulos_Nivel3>(Request.Form["subcapituloN3"]);
            result = ControlObraFactoryService.getControlObraService().guardarSubcapituloN3(subcapituloN3);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateSubcapituloN3(int subcapituloID, string subcapitulo, DateTime fechaInicio, DateTime fechaFin, int subcapituloN2_id)
        {
            result = ControlObraFactoryService.getControlObraService().updateSubcapituloN3(subcapituloID, subcapitulo, fechaInicio, fechaFin, subcapituloN2_id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RemoveSubcapituloN3(int subcapituloID)
        {
            result = ControlObraFactoryService.getControlObraService().removeSubcapituloN3(subcapituloID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ACTIVIDADES
        public ActionResult GetActividadesList(int subcapitulosN1_id, int subcapitulosN2_id, int subcapitulosN3_id)
        {
            result = ControlObraFactoryService.getControlObraService().getActividadesList(subcapitulosN1_id, subcapitulosN2_id, subcapitulosN3_id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getActividadLigadaSiguiente(int subcapitulosN1_id, int subcapitulosN2_id, int subcapitulosN3_id)
        {
            result = ControlObraFactoryService.getControlObraService().getActividadLigadaSiguiente(subcapitulosN1_id, subcapitulosN2_id, subcapitulosN3_id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetActividadesCatalogo(int subcapitulosN1_id, int subcapitulosN2_id, int subcapitulosN3_id)
        {
            result = ControlObraFactoryService.getControlObraService().getActividadesCatalogo(subcapitulosN1_id, subcapitulosN2_id, subcapitulosN3_id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetActividad(int actividadID)
        {
            result = ControlObraFactoryService.getControlObraService().getActividad(actividadID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarActividad(string actividad, decimal cantidad, int unidad_id, DateTime fechaInicio, DateTime fechaFin, int? subcapitulosN1_id, int? subcapitulosN2_id, int? subcapitulosN3_id, bool estatus, int? actividadPadre_id, bool actividadPadreRequerida, bool actividadTerminada)
        {
            result = ControlObraFactoryService.getControlObraService().guardarActividad(actividad, cantidad, unidad_id, fechaInicio, fechaFin, subcapitulosN1_id, subcapitulosN2_id, subcapitulosN3_id, estatus, actividadPadre_id, actividadPadreRequerida, actividadTerminada);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateActividad(int actividadID, string actividad, decimal cantidad, int unidad_id, DateTime fechaInicio, DateTime fechaFin, int? subcapitulosN1_id, int? subcapitulosN2_id, int? subcapitulosN3_id, int? actividadPadre_id, bool actividadPadreRequerida)
        {
            result = ControlObraFactoryService.getControlObraService().updateActividad(actividadID, actividad, cantidad, unidad_id, fechaInicio, fechaFin, subcapitulosN1_id, subcapitulosN2_id, subcapitulosN3_id, actividadPadre_id, actividadPadreRequerida);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RemoveActividad(int actividadID)
        {
            result = ControlObraFactoryService.getControlObraService().removeActividad(actividadID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateActividadPeriodoValor()
        {
            var actividad = JsonConvert.DeserializeObject<List<ActividadPeriodoAvanceDTO>>(Request.Form["actividad"]);

            result = ControlObraFactoryService.getControlObraService().updateActividadPeriodoValor(actividad);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region UNIDADES
        public ActionResult GetUnidadesList()
        {
            result = ControlObraFactoryService.getControlObraService().getUnidadesList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetUnidadesCatalogo()
        {
            result = ControlObraFactoryService.getControlObraService().getUnidadesCatalogo();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetUnidad(int unidadID)
        {
            result = ControlObraFactoryService.getControlObraService().getUnidad(unidadID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarUnidad(string unidad)
        {
            result = ControlObraFactoryService.getControlObraService().guardarUnidad(unidad);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditarUnidad(int unidadID, string unidad)
        {
            result = ControlObraFactoryService.getControlObraService().editarUnidad(unidadID, unidad);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RemoveUnidad(int unidadID)
        {
            result = ControlObraFactoryService.getControlObraService().removeUnidad(unidadID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ACTIVIDAD AVANCE
        public ActionResult GuardarAvanceActividad()
        {
            var avance = JsonConvert.DeserializeObject<ActividadAvanceDTO>(Request.Form["avance"]);
            var avanceDetalle = JsonConvert.DeserializeObject<List<ActividadAvanceDetalleDTO>>(Request.Form["avanceDetalle"]);

            result = ControlObraFactoryService.getControlObraService().guardarAvance(avance, avanceDetalle);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarFacturadoActividad()
        {
            var facturado = JsonConvert.DeserializeObject<FacturadoDTO>(Request.Form["facturado"]);
            var facturadoDetalle = JsonConvert.DeserializeObject<List<FacturadoDetalleDTO>>(Request.Form["facturadoDetalle"]);

            result = ControlObraFactoryService.getControlObraService().guardarAvanceFacturado(facturado, facturadoDetalle);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetFechasUltimoAvance(int capituloID)
        {
            result = ControlObraFactoryService.getControlObraService().getFechasUltimoAvance(capituloID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetActividadAvanceDetalleAutorizar(int subcapitulosN1_id, int subcapitulosN2_id, int subcapitulosN3_id, int actividadAvance_id)
        {
            result = ControlObraFactoryService.getControlObraService().getActividadAvanceDetalleAutorizar(subcapitulosN1_id, subcapitulosN2_id, subcapitulosN3_id, actividadAvance_id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarAutorizacion(bool autorizacion, int avance_id)
        {
            result = ControlObraFactoryService.getControlObraService().guardarAutorizacion(autorizacion, avance_id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPeriodoAvance()
        {
            result = ControlObraFactoryService.getControlObraService().getPeriodoAvance();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult UpdateActividadAvance(int actividadAvanceID, int actividadID, decimal cantidad, DateTime fechaInicio, DateTime fechaFin)
        //{
        //    result = ControlObraFactoryService.getControlObraService().validaActividadAvanceEdit(actividadAvanceID, actividadID, cantidad, fechaInicio, fechaFin);
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}
        //public ActionResult RemoveActividadAvance(int actividadAvanceID, int actividadID)
        //{
        //    result = ControlObraFactoryService.getControlObraService().removeActividadAvance(actividadAvanceID, actividadID);
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}
        #endregion

        #region REPORTE AVANCES
        public ActionResult GetActividadAvanceReporte(int subcapitulosN1_id, int subcapitulosN2_id, int subcapitulosN3_id, DateTime fechaInicio, DateTime fechaFin)
        {
            result = ControlObraFactoryService.getControlObraService().getActividadAvanceReporte(subcapitulosN1_id, subcapitulosN2_id, subcapitulosN3_id, fechaInicio, fechaFin);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAvancesAutorizar(int capituloID)
        {
            result = ControlObraFactoryService.getControlObraService().getAvancesAutorizar(capituloID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetConcentradoReporte(int capitulo_id, DateTime fechaInicio, DateTime fechaFin)
        {
            result = ControlObraFactoryService.getControlObraService().getConcentradoReporte(capitulo_id, fechaInicio, fechaFin);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region IMPORTAR ARCHIVO OPUS
        public ActionResult GuardarArchivos()
        {
            var nombreObra = JsonConvert.DeserializeObject<string>(Request.Form["nombreObra"]);
            var periodoFacturacion = JsonConvert.DeserializeObject<string>(Request.Form["periodoFacturacion"]);
            var cc_id = JsonConvert.DeserializeObject<string>(Request.Form["cc_id"]);
            var autorizande_id = JsonConvert.DeserializeObject<string>(Request.Form["autorizande_id"]);

            int ptemp;
            int? periodo = int.TryParse(periodoFacturacion, out ptemp) ? ptemp : default(int?);

            int cctemp;
            int? cc = int.TryParse(cc_id, out cctemp) ? cctemp : default(int?);

            int atemp;
            int? autorizante = int.TryParse(autorizande_id, out atemp) ? atemp : default(int?);

            string nombreSinEspacio = nombreObra.Replace(" ", "");
            List<HttpPostedFileBase> archivos = new List<HttpPostedFileBase>();

            foreach (string fileName in Request.Files)
            {
                archivos.Add(Request.Files[fileName]);
            }

            result = ControlObraFactoryService.getControlObraService().guardarInfoOpus(nombreObra.ToString(), nombreSinEspacio, archivos, periodo, cc, autorizante);
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region INFORME SEMANAL
        public ActionResult ObtenerDivisiones()
        {
            result = ControlObraFactoryService.getControlObraService().ObtenerDivisiones();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ObtenerDivisionCC()
        {
            result = ControlObraFactoryService.getControlObraService().obtenerDivisionCC();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetInformesSemanal(int division_id)
        {
            result = ControlObraFactoryService.getControlObraService().getInformesSemanal(division_id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetInformeSemanalContenido(int informe_id)
        {
            result = ControlObraFactoryService.getControlObraService().getInformeSemanalContenido(informe_id);
            var a = new JsonResult();
            a.MaxJsonLength = int.MaxValue;
            a.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            a.Data = result;
            return a;
        }
        public ActionResult GetUltimoInforme()
        {
            result = ControlObraFactoryService.getControlObraService().getUltimoInforme();
            var a = new JsonResult();
            a.MaxJsonLength = int.MaxValue;
            a.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            a.Data = result;
            return a;
        }
        public ActionResult GetInformeSemanal(int informe_id)
        {
            result = ControlObraFactoryService.getControlObraService().getInformeSemanal(informe_id);
            var a = new JsonResult();
            a.MaxJsonLength = int.MaxValue;
            a.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            a.Data = result;
            return a;
        }
        public ActionResult GetPlantillaInformeDetalle(int plantilla_id)
        {
            result = ControlObraFactoryService.getControlObraService().getPlantillaInformeDetalle(plantilla_id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPlantillaInformeDetalleCC()
        {
            result = ControlObraFactoryService.getControlObraService().getPlantillaInformeDetalleCC();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPlantillaDivision(int divisionID)
        {
            result = ControlObraFactoryService.getControlObraService().getPlantillaDivision(divisionID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarPlantilla(PlantillaInformeDTO plantilla, List<PlantillaInforme_detalleDTO> plantilla_detalle)
        {
            result = ControlObraFactoryService.getControlObraService().guardarPlantilla(plantilla, plantilla_detalle);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarPlantillaContenido(int plantilla_id, List<tblCO_PlantillaInforme_detalle> plantilla_contenido)
        {
            result = ControlObraFactoryService.getControlObraService().guardarPlantillaContenido(plantilla_id, plantilla_contenido);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarInforme(informeSemanalDTO informe, List<informeSemanal_detalleDTO> informe_detalle)
        {
            result = ControlObraFactoryService.getControlObraService().guardarInforme(informe, informe_detalle);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion



        #region CONTROL OBRA SUB CONTRATISTA
        public ActionResult DashboardSubContratista()
        {
            return View();
        }
        public ActionResult AutorizacionSubContratista()
        {
            return View();
        }
        public ActionResult SubContratista()
        {
            return View();
        }
        public ActionResult CatalogoDeDiviciones()
        {
            return View();
        }
        public ActionResult DashboardEvaluador()
        {
            return View();
        }
        public ActionResult getProyecto()
        {
            try
            {
                result.Add(ITEMS, ControlObraFactoryService.getControlObraService().getProyecto());
                result.Add(SUCCESS, "SUCCESS");
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, "ERROR");
                throw;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getSubContratistas()
        {
            try
            {
                result.Add(ITEMS, ControlObraFactoryService.getControlObraService().getSubContratistas());
                result.Add(SUCCESS, "SUCCESS");
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, "ERROR");
                throw;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getTblSubContratista(SubContratistasDTO parametros)
        {
            try
            {
                result.Add(ITEMS, ControlObraFactoryService.getControlObraService().getTblSubContratista(parametros));
                result.Add(SUCCESS, "SUCCESS");
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, "ERROR");
                throw;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult addEditSubContratista(List<HttpPostedFileBase> Archivo, List<SubContratistasDTO> parametros)
        {
            try
            {
                List<SubContratistasDTO> parameters = new List<SubContratistasDTO>();
                foreach (var item in parametros)
                {
                    item.idSubContratista = getUsuario().id;
                    parameters.Add(item);
                }
                result.Add(ITEMS, ControlObraFactoryService.getControlObraService().addEditSubContratista(Archivo, parameters));
                result.Add(SUCCESS, "SUCCESS");
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, "ERROR");
                throw;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CargarArchivosXSubcontratista(SubContratistasDTO parametros)
        {
            try
            {
                parametros.idSubContratista = getUsuario().id;
                result.Add(ITEMS, ControlObraFactoryService.getControlObraService().CargarArchivosXSubcontratista(parametros));
                result.Add(SUCCESS, "SUCCESS");
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, "ERROR");
                throw;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ObtenerTblAutorizacion(SubContratistasDTO parametros)
        {
            try
            {

                result.Add(ITEMS, ControlObraFactoryService.getControlObraService().ObtenerTblAutorizacion(parametros));
                result.Add(SUCCESS, "SUCCESS");
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, "ERROR");
                throw;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult obtenerDiviciones()
        {
            try
            {
                result.Add(ITEMS, ControlObraFactoryService.getControlObraService().obtenerDiviciones());
                result.Add(SUCCESS, "SUCCESS");
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, "ERROR");
                throw;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult addEditDiviciones(DivicionesMenuDTO parametros)
        {
            try
            {

                result.Add(ITEMS, ControlObraFactoryService.getControlObraService().addEditDiviciones(parametros));
                result.Add(SUCCESS, "SUCCESS");
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, "ERROR");
                throw;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult eliminarDiviciones(int id)
        {
            try
            {
                result.Add(ITEMS, ControlObraFactoryService.getControlObraService().eliminarDiviciones(id));
                result.Add(SUCCESS, "SUCCESS");
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, "ERROR");
                throw;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult obtenerRequerimientos(int idDiv)
        {
            try
            {
                result.Add(ITEMS, ControlObraFactoryService.getControlObraService().obtenerRequerimientos(idDiv));
                result.Add(SUCCESS, "SUCCESS");
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, "ERROR");
                throw;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult obtenerDivicionesEvaluador()
        {
            try
            {
                result.Add(ITEMS, ControlObraFactoryService.getControlObraService().obtenerDivicionesEvaluador());
                result.Add(SUCCESS, "SUCCESS");
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, "ERROR");
                throw;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DescargarArchivo(long idDet)
        {
            var array = ControlObraFactoryService.getControlObraService().DescargarArchivos(idDet);
            string pathExamen = ControlObraFactoryService.getControlObraService().getFileName(idDet);

            if (array != null)
            {
                return File(array, System.Net.Mime.MediaTypeNames.Application.Octet, pathExamen);
            }
            else
            {
                return View("ErrorDescarga");
            }

        }


        #endregion

        #region PRESTADORES DE SERVICIO ORDEN DE CAMBIO


        #region GESTION DE ORDEN DE CAMBIO
        public ActionResult IndexOrdenCambio()
        {
            return View();
        }
        public ActionResult gestionFacultamientos()
        {
            return View();
        }
        public ActionResult GestionOrden()
        {
            return View();
        }
        public ActionResult DashboardOrdenCambio()
        {
            return View();
        }
        public ActionResult getProyectoOrdenCambio()
        {
            try
            {
                result.Add(ITEMS, gestionDeProyectoFactoryService.getGestionDeProyectoService().getProyecto());
                result.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, false);
                result.Add(ITEMS, "");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult obtenerOrdenesDeCambio(string cc)
        {
            try
            {
                int idUsuario = getUsuario().id;
                result.Add(ITEMS, gestionDeProyectoFactoryService.getGestionDeProyectoService().obtenerOrdenesDeCambio(cc, idUsuario));
                result.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, false);
                result.Add(ITEMS, "");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult comboObtenerContratosyUltimasOrdenesDeCambio(List<string> filtroCC)
        {
            try
            {
                result.Add(ITEMS, gestionDeProyectoFactoryService.getGestionDeProyectoService().comboObtenerContratosyUltimasOrdenesDeCambio(filtroCC));
                result.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, false);
                result.Add(ITEMS, "");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult comboObtenerContratosyUltimasOrdenesDeCambioEditar()
        {
            try
            {
                result.Add(ITEMS, gestionDeProyectoFactoryService.getGestionDeProyectoService().comboObtenerContratosyUltimasOrdenesDeCambioEditar());
                result.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, false);
                result.Add(ITEMS, "");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult obtenerCamposDeOrdenDeCambio(ordenesDeCambioDTO parametros)
        {
            try
            {
                result.Add(ITEMS, gestionDeProyectoFactoryService.getGestionDeProyectoService().obtenerCamposDeOrdenDeCambio(parametros));
                result.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, false);
                result.Add(ITEMS, "");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult nuevoEditarOrdenesDeCambio(ordenesDeCambioDTO parametros)
        {
            try
            {
                if (parametros.editar == false)
                {
                    var lstFirmas = JsonUtils.convertJsonToNetObject<List<tblCO_OC_Firmas>>(Request.Form["lstFirmas"], "es-MX");
                    parametros.lstFirmas = lstFirmas;
                }
                var cveEmpleados = JsonUtils.convertJsonToNetObject<List<cveEmpleadosDTO>>(Request.Form["cveEmpleados"], "es-MX");
                var lstMontos = JsonUtils.convertJsonToNetObject<List<tblCO_OC_Montos>>(Request.Form["lstMontos"], "es-MX");
                parametros.lstMontos = lstMontos;
                var lstSoportesEvidencia = JsonUtils.convertJsonToNetObject<SoportesEvidenciaDTO>(Request.Form["lstSoportesEvidencia"], "es-MX");

                //parametros.lstMontos = lstMontos;
                parametros.lstSoportesEvidencia = lstSoportesEvidencia;
                parametros.cveEmpleados = cveEmpleados;
                parametros.representanteLegal = Request.Form["representanteLegal"] as string;
               
                if(parametros.fechaAmpliacion != null)
                {
                    parametros.fechaAmpliacion = Convert.ToDateTime(Request.Form["FechaAmpliacion"], CultureInfo.InvariantCulture);
                }
       
                
                result.Add(ITEMS, gestionDeProyectoFactoryService.getGestionDeProyectoService().nuevoEditarOrdenesDeCambio(parametros, getUsuario().id));
                result.Add(SUCCESS, true);

            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, false);
                result.Add(ITEMS, "");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EliminarRenglon(int id)
        {
            try
            {
                result.Add(ITEMS, gestionDeProyectoFactoryService.getGestionDeProyectoService().EliminarRenglon(id));
                result.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, false);
                result.Add(ITEMS, "");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GenerandoFirmas(List<firmasDTO> lstFirmas)
        {
            try
            {
                int idUsuario = getUsuario().id;
                result.Add(ITEMS, gestionDeProyectoFactoryService.getGestionDeProyectoService().GenerandoFirmas(lstFirmas, idUsuario));
                result.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, false);
                result.Add(ITEMS, "");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AutorizarOrdenDeCambio(int id)
        {
            try
            {

                result.Add(ITEMS, gestionDeProyectoFactoryService.getGestionDeProyectoService().AutorizarOrdenDeCambio(id, 2));
                result.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, false);
                result.Add(ITEMS, "");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RechazarOrdenDeCambio(int id)
        {
            try
            {
                result.Add(ITEMS, gestionDeProyectoFactoryService.getGestionDeProyectoService().RechazarOrdenDeCambio(id));
                result.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, false);
                result.Add(ITEMS, "");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getUsuarioSelectWithExceptionSubContratista(string term, string cc)
        {
            var items = gestionDeProyectoFactoryService.getGestionDeProyectoService().ListUsersByNameWithException(term, cc);
            var filteredItems = items.Select(x => new { id = x.id, label = x.nombre_completo, puesto = "" });
            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getUsuarioSelectWithExceptionConstruplan(string term, string cc)
        {
            var items = gestionDeProyectoFactoryService.getGestionDeProyectoService().ListUsersByNameWithExceptionConstruplan(term, cc);
            var filteredItems = items.Select(x => new { id = x.id, label = x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno, puesto = "" });
            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Autorizar(int idOrdenDeCambio, string firma)
        {
            try
            {
                result.Add(ITEMS, gestionDeProyectoFactoryService.getGestionDeProyectoService().Autorizar(idOrdenDeCambio, getUsuario().id, firma));
                result.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, false);
                result.Add(ITEMS, "");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Rechazar(int idOrdenDeCambio, string firma)
        {
            try
            {
                result.Add(ITEMS, gestionDeProyectoFactoryService.getGestionDeProyectoService().Rechazar(idOrdenDeCambio, getUsuario().id, firma));
                result.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, false);
                result.Add(ITEMS, "");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult obtenerTodasLasFirmas(string filtroCC, int filtroOrdenCambioID)
        {
            return Json(gestionDeProyectoFactoryService.getGestionDeProyectoService().obtenerTodasLasFirmas(filtroCC, filtroOrdenCambioID), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEstatusGlobalOrdenesCambio(string cc)
        {
            return Json(gestionDeProyectoFactoryService.getGestionDeProyectoService().GetEstatusGlobalOrdenesCambio(cc), JsonRequestBehavior.AllowGet);
        }

        public ActionResult AutorizarRechazarOrdenCambio(bool esAutorizar, int idOrdenCambio, string comentarioRechazo)
        {
            return Json(gestionDeProyectoFactoryService.getGestionDeProyectoService().AutorizarRechazarOrdenCambio(esAutorizar, idOrdenCambio, comentarioRechazo), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarDocumentoFirmado(firmasDTO parametros)
        {
            return Json(gestionDeProyectoFactoryService.getGestionDeProyectoService().GuardarDocumentoFirmado(parametros), JsonRequestBehavior.AllowGet);
        }
        public ActionResult autorizarArchivoFirmado(ordenesDeCambioDTO parametros)
        {
            return Json(gestionDeProyectoFactoryService.getGestionDeProyectoService().autorizarArchivoFirmado(parametros), JsonRequestBehavior.AllowGet);
        }

        public ActionResult obtenerOrdenesDeCambiabosPorAutorizar(string cc, int estatus, int idUsuario = 0)
        {
            try
            {
                result.Add(ITEMS, gestionDeProyectoFactoryService.getGestionDeProyectoService().obtenerOrdenesDeCambiabosPorAutorizar(cc, estatus, idUsuario, 2));
                result.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, false);
                result.Add(ITEMS, "");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult obteniendoTipoYIDDeUsuario()
        {
            try
            {
                int Tipo = 2;
                int id = getUsuario().id;
                var lst = new
                {
                    tipo = Tipo,
                    id = id
                };
                result.Add(ITEMS, lst);
                result.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, false);
                result.Add(ITEMS, "");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult obtenerArchivos(int id)
        {
            try
            {
                result.Add(ITEMS, gestionDeProyectoFactoryService.getGestionDeProyectoService().obtenerArchivos(id));
                result.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, false);
                result.Add(ITEMS, "");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult DescargarArchivos(long idDet, int Tipo)
        {
            var array = gestionDeProyectoFactoryService.getGestionDeProyectoService().DescargarArchivos(idDet, Tipo);
            string pathExamen = gestionDeProyectoFactoryService.getGestionDeProyectoService().getFileName(idDet, Tipo);

            if (array != null && pathExamen != "")
            {
                return File(array, System.Net.Mime.MediaTypeNames.Application.Octet, pathExamen);
            }
            else
            {
                return View("ErrorDescarga");
            }

        }
        public ActionResult obtenerPuestos()
        {
            try
            {
                result.Add(ITEMS, gestionDeProyectoFactoryService.getGestionDeProyectoService().obtenerPuestos());
                result.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, false);
                result.Add(ITEMS, "");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EnviarCorreo(int idOrdenDeCambio, Byte[] archivo, int tipoCorreo)
        {
            try
            {
                result.Add(ITEMS, gestionDeProyectoFactoryService.getGestionDeProyectoService().EnviarCorreo(idOrdenDeCambio, archivo, tipoCorreo));
                result.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, false);
                result.Add(ITEMS, "");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult obtenerOrdenDeCambioByID(int idOrdenDeCambio)
        {
            try
            {
                result.Add(ITEMS, gestionDeProyectoFactoryService.getGestionDeProyectoService().obtenerOrdenDeCambioByID(idOrdenDeCambio));
                result.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, false);
                result.Add(ITEMS, "");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult obtenerPermisos()
        {
            try
            {
                int idUsuario = getUsuario().id;
                result.Add(ITEMS, gestionDeProyectoFactoryService.getGestionDeProyectoService().obtenerPermisos(idUsuario));
                result.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, false);
                result.Add(ITEMS, "");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDashboardOrdenCambio(string cc, int contrato_id, DateTime fechaInicio, DateTime fechaFin)
        {
            return Json(gestionDeProyectoFactoryService.getGestionDeProyectoService().GetDashboardOrdenCambio(cc, contrato_id, fechaInicio, fechaFin), JsonRequestBehavior.AllowGet);
        }

        public ActionResult fillComboContratistasByContrato(int idContrato)
        {
            return Json(gestionDeProyectoFactoryService.getGestionDeProyectoService().fillComboContratistasByContrato(idContrato), JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult obtenerLstFacultamientos(string cc)
        {
            try
            {
                result.Add(ITEMS, gestionDeProyectoFactoryService.getGestionDeProyectoService().obtenerLstFacultamientos(cc));
                result.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, false);
                result.Add(ITEMS, "");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult agregarEditarFacultamientos(facultamientosDTO parametros)
        {
            try
            {
                result.Add(ITEMS, gestionDeProyectoFactoryService.getGestionDeProyectoService().agregarEditarFacultamientos(parametros));
                result.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, false);
                result.Add(ITEMS, "");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EliminarFacultamiento(int idFacultamiento)
        {
            try
            {
                result.Add(ITEMS, gestionDeProyectoFactoryService.getGestionDeProyectoService().EliminarFacultamiento(idFacultamiento));
                result.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, false);
                result.Add(ITEMS, "");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult obtenerFacultamiento(int idUsuario)
        {
            try
            {
                result.Add(ITEMS, gestionDeProyectoFactoryService.getGestionDeProyectoService().obtenerFacultamiento(idUsuario));
                result.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, false);
                result.Add(ITEMS, "");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult obtenerCC()
        {
            try
            {
                result.Add(ITEMS, gestionDeProyectoFactoryService.getGestionDeProyectoService().obtenerCC());
                result.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, false);
                result.Add(ITEMS, "");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult obtenerUsuarios()
        {
            try
            {
                result.Add(ITEMS, gestionDeProyectoFactoryService.getGestionDeProyectoService().obtenerUsuarios());
                result.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, false);
                result.Add(ITEMS, "");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetInsumosSISUNAutocomplete(string term, bool busquedaPorNumero, string cc)
        {
            return Json(gestionDeProyectoFactoryService.getGestionDeProyectoService().GetInsumosSISUNAutocomplete(term, busquedaPorNumero, cc), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetUsuariosAutocomplete(string term, bool porClave)
        {
            return Json(gestionDeProyectoFactoryService.getGestionDeProyectoService().GetUsuariosAutocomplete(term, porClave), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboEstados()
        {
            try
            {
                result.Add(ITEMS, gestionDeProyectoFactoryService.getGestionDeProyectoService().FillComboEstados());
                result.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, false);
                result.Add(ITEMS, "");
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult FillComboMunicipios(int estado_id)
        {
            try
            {
                result.Add(ITEMS, gestionDeProyectoFactoryService.getGestionDeProyectoService().FillComboMunicipios(estado_id));
                result.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, false);
                result.Add(ITEMS, "");
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }


        #endregion


        #region MATRIZ DE RIESGO

        public ActionResult MatrizDeRiesgo()
        {
            return View();
        }
        public ActionResult CatalogoDeRepuestas()
        {
            return View();
        }
        public ActionResult TiposDeRespuestas()
        {
            return View();
        }

        public ActionResult obtenerMatrizesDeRiesgo(string variable)
        {
            try
            {
                result.Add(ITEMS, getMatrizDeRiesgoFactoryService.getMatrizDeRiesgoService().obtenerMatrizesDeRiesgo(variable));
                result.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, false);
                result.Add(ITEMS, "");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult obtenerMatrizesDeRiesgoxID(int idMatrizDeRiesgo, List<int> lstFiltro)
        {
            try
            {
                result.Add(ITEMS, getMatrizDeRiesgoFactoryService.getMatrizDeRiesgoService().obtenerMatrizesDeRiesgoxID(idMatrizDeRiesgo, lstFiltro));
                result.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, false);
                result.Add(ITEMS, "");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarEditarMatriz(MatrizDTO parametros, bool editar)
        {
            try
            {
                result.Add(ITEMS, getMatrizDeRiesgoFactoryService.getMatrizDeRiesgoService().GuardarEditarMatriz(parametros, editar));
                result.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, false);
                result.Add(ITEMS, "");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult eliminarMatrizDeRiesgoDet(int idMatrizDeRiesgoDet)
        {
            try
            {
                //result.Add(ITEMS, FactoryService.getService().obtenerLstFacultamientos(idMatrizDeRiesgoDet));
                result.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, false);
                result.Add(ITEMS, "");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        #endregion


        public ActionResult obtenerContratos()
        {
            try
            {
                result.Add(ITEMS, getMatrizDeRiesgoFactoryService.getMatrizDeRiesgoService().obtenerContratos());
                result.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, false);
                result.Add(ITEMS, "");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult obtenerContratosByCC(string cc)
        {
            try
            {
                var lstContratos = getMatrizDeRiesgoFactoryService.getMatrizDeRiesgoService().obtenerContratos();

                lstContratos = lstContratos.Where(e => e.TextoOpcional == cc).ToList();

                result.Add(ITEMS, lstContratos);
                result.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, false);
                result.Add(ITEMS, "");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TraermeTodosLosCC()
        {
            try
            {
                result.Add(ITEMS, getMatrizDeRiesgoFactoryService.getMatrizDeRiesgoService().TraermeTodosLosCC());
                result.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, false);
                result.Add(ITEMS, "");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult QuienElaboro()
        {
            try
            {
                int idUsuario = getUsuario().id;
                result.Add(ITEMS, getMatrizDeRiesgoFactoryService.getMatrizDeRiesgoService().QuienElaboro(idUsuario));
                result.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, false);
                result.Add(ITEMS, "");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult lstMrCategorias()
        {
            try
            {
                result.Add(ITEMS, getMatrizDeRiesgoFactoryService.getMatrizDeRiesgoService().lstMrCategorias());
                result.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, false);
                result.Add(ITEMS, "");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AgregarEditarCategoria(tblCO_MR_CategoriaDeRiesgo parametros)
        {
            try
            {
                int idUsuario = getUsuario().id;
                result.Add(ITEMS, getMatrizDeRiesgoFactoryService.getMatrizDeRiesgoService().AgregarEditarCategoria(parametros));
                result.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, false);
                result.Add(ITEMS, "");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EliminarCategoria(tblCO_MR_CategoriaDeRiesgo parametros)
        {
            try
            {
                int idUsuario = getUsuario().id;
                result.Add(ITEMS, getMatrizDeRiesgoFactoryService.getMatrizDeRiesgoService().EliminarCategoria(parametros));
                result.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, false);
                result.Add(ITEMS, "");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult cbolstMrCategorias()
        {
            try
            {
                int idUsuario = getUsuario().id;
                result.Add(ITEMS, getMatrizDeRiesgoFactoryService.getMatrizDeRiesgoService().cbolstMrCategorias());
                result.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, false);
                result.Add(ITEMS, "");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult cboTiposDeRespuestas(int idTipo)
        {
            try
            {
                result.Add(ITEMS, getMatrizDeRiesgoFactoryService.getMatrizDeRiesgoService().cboTiposDeRespuestas(idTipo));
                result.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, false);
                result.Add(ITEMS, "");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult cboResponsables()
        {
            try
            {
                result.Add(ITEMS, getMatrizDeRiesgoFactoryService.getMatrizDeRiesgoService().cboResponsables());
                result.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, false);
                result.Add(ITEMS, "");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult lstMrTiposDeRespuestas()
        {
            try
            {
                result.Add(ITEMS, getMatrizDeRiesgoFactoryService.getMatrizDeRiesgoService().lstMrTiposDeRespuestas());
                result.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, false);
                result.Add(ITEMS, "");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AgregarEditarTiposDeRespuestas(tblCO_MR_TipoDeRespuestas parametros)
        {
            try
            {
                int idUsuario = getUsuario().id;
                result.Add(ITEMS, getMatrizDeRiesgoFactoryService.getMatrizDeRiesgoService().AgregarEditarTiposDeRespuestas(parametros));
                result.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, false);
                result.Add(ITEMS, "");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EliminarTiposDeRespuestas(tblCO_MR_TipoDeRespuestas parametros)
        {
            try
            {
                int idUsuario = getUsuario().id;
                result.Add(ITEMS, getMatrizDeRiesgoFactoryService.getMatrizDeRiesgoService().EliminarTiposDeRespuestas(parametros));
                result.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, false);
                result.Add(ITEMS, "");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public int GetPrivilegioUsuario()
        {
            var f = new GestionDeProyectoFactoryService();
            int privilegio = f.getGestionDeProyectoService().GetPrivilegioUsuario();
            return privilegio;
		}
        public MemoryStream DescargarArchivo(int id)
        {
            var lstAreaCuenta = new List<string>();
            var stream = gestionDeProyectoFactoryService.getGestionDeProyectoService().DescargarArchivo(id);

            if (stream != null)
            {
                this.Response.Clear();
                //this.Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //this.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", "Reporte_Kubrix_Subcuenta.xlsx"));
                this.Response.BinaryWrite(stream.ToArray());
                this.Response.End();

                return stream;
            }
            else
            {
                return null;
            }
        }
    }
}