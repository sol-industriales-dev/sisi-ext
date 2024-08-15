using Core.DAO.Subcontratistas;
using Core.DTO;
using Core.DTO.ControlObra;
using Core.DTO.ControlObra.EvaluacionSubcontratista;
using Core.DTO.Principal.Generales;
using Core.DTO.Subcontratistas.Evaluacion;
using Core.Entity.ControlObra;
using Core.Enum.ControlObra;
using Core.Enum.Subcontratistas;
using Data.Factory.Subcontratistas;
using Infrastructure.Utils;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.SubContratistas.Controllers
{
    public class EvaluacionSubcontratistaController : BaseController
    {
        #region SUB
        private IEvaluacionSubcontratistaDAO evaluacionSubcontratistaService;

        Dictionary<string, object> result;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            evaluacionSubcontratistaService = new EvaluacionSubcontratistaFactoryService().GetEvaluacionSubcontratistaService();
            result = new Dictionary<string, object>();
            base.OnActionExecuting(filterContext);
        }

        #region Captura
        public ViewResult AdministracionEvaluacion()
        {
            return evaluacionSubcontratistaService.PermisoVista((int)VistasEnum.administracionEvaluacion).Success ? View() : View("ErrorPermisoVista");
        }
        #endregion
        #region Gestión
        //public ViewResult Gestion()
        //{
        //    Respuesta respuesta = evaluacionSubcontratistaService.PermisoVista((int)VistasEnum.gestionFirmas);
        //    if (respuesta.Success)
        //        return View();
        //    else
        //        return View("ErrorPermisoVista");
        //}

        [HttpGet]
        public JsonResult ObtenerProyectosParaFiltro()
        {
            return Json(evaluacionSubcontratistaService.ObtenerProyectosParaFiltro(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerSubcontratistasParaFiltro(string proyecto)
        {
            return Json(evaluacionSubcontratistaService.ObtenerSubcontratistasParaFiltro(proyecto), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerEvaluacionesSubcontratistas(string proyecto, int? subcontratistaId)
        {
            return Json(evaluacionSubcontratistaService.ObtenerEvaluacionesSubcontratistas(proyecto, subcontratistaId), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerEstatusFirmantes(int evaluacionId)
        {
            return Json(evaluacionSubcontratistaService.ObtenerEstatusFirmantes(evaluacionId), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerFirmante(int evaluacionId)
        {
            return Json(evaluacionSubcontratistaService.ObtenerFirmante(evaluacionId), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarFirma(InformacionFirmaDigitalDTO firma)
        {
            return Json(evaluacionSubcontratistaService.GuardarFirma(firma));
        }
        #endregion
        #region Dashboard
        public ViewResult Dashboard()
        {
            Respuesta respuesta = evaluacionSubcontratistaService.PermisoVista((int)VistasEnum.dashboard);
            if (respuesta.Success)
                return View();
            else
                return View("ErrorPermisoVista");
        }
        #endregion
        #region Catalogo Alta Elementos
        //public ViewResult CatalogoAltaElementos()
        //{
        //    Respuesta respuesta = evaluacionSubcontratistaService.PermisoVista((int)VistasEnum.plantillas);
        //    if (respuesta.Success)
        //        return View();
        //    else
        //        return View("ErrorPermisoVista");
        //}
        public ActionResult cboProyecto()
        {
            return Json(evaluacionSubcontratistaService.cboProyecto(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult cboProyecto3(int idSubcontratista)
        {
            return Json(evaluacionSubcontratistaService.cboProyecto3(idSubcontratista), JsonRequestBehavior.AllowGet);
        }
        public ActionResult cboContratosBuscar3(int idSubcontratista)
        {
            return Json(evaluacionSubcontratistaService.cboContratosBuscar3(idSubcontratista), JsonRequestBehavior.AllowGet);
        }
        public ActionResult cboEvaluador()
        {
            return Json(evaluacionSubcontratistaService.cboEvaluador(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult cboSubcontratistas()
        {
            return Json(evaluacionSubcontratistaService.cboSubcontratistas(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult cboElementos()
        {
            return Json(evaluacionSubcontratistaService.cboElementos(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillComboPlantillas()
        {
            return Json(evaluacionSubcontratistaService.FillComboPlantillas(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult cboContratosBuscar()
        {
            return Json(evaluacionSubcontratistaService.cboContratosBuscar(), JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region Catalogo Alta Evaluadores
        //public ViewResult CatalogoAltaEvaluadores()
        //{
        //    Respuesta respuesta = evaluacionSubcontratistaService.PermisoVista((int)VistasEnum.evaluadores);
        //    if (respuesta.Success)
        //        return View();
        //    else
        //        return View("ErrorPermisoVista");
        //}
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
                int idUsuario = getUsuario().id;
                result.Add(ITEMS, evaluacionSubcontratistaService.getProyecto(idUsuario));
                result.Add(SUCCESS, "SUCCESS");
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, "ERROR");
                throw;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getSubContratistas(string AreaCuenta)
        {
            try
            {
                result.Add(ITEMS, evaluacionSubcontratistaService.getSubContratistas(AreaCuenta));
                result.Add(SUCCESS, "SUCCESS");
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, "ERROR");
                throw;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillCboProyectosFacultamientos()
        {
            return Json(evaluacionSubcontratistaService.FillCboProyectosFacultamientos(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult getTblSubContratista(SubContratistasDTO parametros)
        {
            try
            {
                result.Add(ITEMS, evaluacionSubcontratistaService.getTblSubContratista(parametros));
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
                    item.idSubContratista = 1;
                    parameters.Add(item);
                }
                result.Add(ITEMS, evaluacionSubcontratistaService.addEditSubContratista(Archivo, parameters));
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
                result.Add(ITEMS, evaluacionSubcontratistaService.CargarArchivosXSubcontratista(parametros));
                result.Add(SUCCESS, "SUCCESS");
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, "ERROR");
                throw;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarArchivosSubcontratista(SubContratistasDTO parametros)
        {
            try
            {
                parametros.RFC = "";
                result.Add(ITEMS, evaluacionSubcontratistaService.CargarArchivosSubcontratista(parametros));
                result.Add(SUCCESS, "SUCCESS");
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, "ERROR");
                throw;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult CargarArchivosXSubcontratistaEvaluacion(SubContratistasDTO parametros)
        {
            try
            {
                result.Add(ITEMS, evaluacionSubcontratistaService.CargarArchivosXSubcontratistaEvaluacion(parametros));
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

                result.Add(ITEMS, evaluacionSubcontratistaService.ObtenerTblAutorizacion(parametros));
                result.Add(SUCCESS, "SUCCESS");
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, "ERROR");
                throw;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPlantillasCreadas(int plantilla_id, int contrato_id)
        {
            return Json(evaluacionSubcontratistaService.GetPlantillasCreadas(plantilla_id, contrato_id), JsonRequestBehavior.AllowGet);
        }
        public ActionResult addEditPlantilla(DivicionesMenuDTO objDTO)
        {
            //try
            //{
            //    result.Add(ITEMS, evaluacionSubcontratistaService.addEditPlantilla(parametros));
            //    result.Add(SUCCESS, "SUCCESS");
            //}
            //catch (Exception ex)
            //{
            //    result.Add(SUCCESS, "ERROR");
            //    throw;
            //}
            //return Json(result, JsonRequestBehavior.AllowGet);

            return Json(evaluacionSubcontratistaService.addEditPlantilla(objDTO), JsonRequestBehavior.AllowGet);
        }
        public ActionResult cboObtenerContratos()
        {
            try
            {
                var rt = evaluacionSubcontratistaService.cboObtenerContratos();
                var lst = rt.Select(y => new ComboDTO
                {
                    Value = y.Value,
                    Text = y.Text,
                }).ToList();
                result.Add(ITEMS, lst);
                result.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, false);
                throw;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult cboObtenerContratosInclu(int idPlantilla)
        {
            try
            {
                var rt = evaluacionSubcontratistaService.cboObtenerContratosInclu(idPlantilla);
                var lst = rt.Select(y => new ComboDTO
                {
                    Value = y.Value,
                    Text = y.Text,
                }).ToList();
                result.Add(ITEMS, lst);
                result.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, false);
                throw;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult eliminarPlantilla(int id)
        //{
        //    return Json(evaluacionSubcontratistaService.eliminarPlantilla(id), JsonRequestBehavior.AllowGet);
        //}

        public ActionResult obtenerDiviciones(int idPlantilla)
        {
            try
            {
                result.Add(ITEMS, evaluacionSubcontratistaService.obtenerDiviciones(idPlantilla));
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

                result.Add(ITEMS, evaluacionSubcontratistaService.addEditDiviciones(parametros));
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
                result.Add(ITEMS, evaluacionSubcontratistaService.eliminarDiviciones(id));
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
                result.Add(ITEMS, evaluacionSubcontratistaService.obtenerRequerimientos(idDiv));
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
            #region v1 ADAN
            //try
            //{
            //    result.Add(ITEMS, evaluacionSubcontratistaService.obtenerDivicionesEvaluador());
            //    result.Add(SUCCESS, "SUCCESS");
            //}
            //catch (Exception ex)
            //{
            //    result.Add(SUCCESS, "ERROR");
            //    throw;
            //}
            //return Json(result, JsonRequestBehavior.AllowGet);
            #endregion

            // v2
            return Json(evaluacionSubcontratistaService.obtenerDivicionesEvaluador(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult obtenerDivicionesEvaluadorArchivos(int idPlantilla, int idAsignacion)
        {
            try
            {
                result.Add(ITEMS, evaluacionSubcontratistaService.obtenerDivicionesEvaluadorArchivos(idPlantilla, idAsignacion));
                result.Add(SUCCESS, "SUCCESS");
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, "ERROR");
                throw;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DescargarArchivo(string p_idDet)
        {
            string[] objParametros = p_idDet.Split('|');
            long idDet = Convert.ToInt32(objParametros[0]);
            int idEvaluacion = Convert.ToInt32(objParametros[1]);

            var array = evaluacionSubcontratistaService.DescargarArchivos(idDet, idEvaluacion);
            string pathExamen = evaluacionSubcontratistaService.getFileName(idDet, idEvaluacion);

            if (array != null)
            {
                return File(array, System.Net.Mime.MediaTypeNames.Application.Octet, pathExamen);
            }
            else
            {
                return View("ErrorDescarga");
            }

        }

        public ActionResult obtenerEvaluacionxReq(SubContratistasDTO parametros)
        {
            try
            {
                result.Add(ITEMS, evaluacionSubcontratistaService.obtenerEvaluacionxReq(parametros));
                result.Add(SUCCESS, "SUCCESS");
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, "ERROR");
                throw;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarEvaluacion(SubContratistasDTO parametros)
        {
            try
            {
                result.Add(ITEMS, evaluacionSubcontratistaService.GuardarEvaluacion(parametros));
                result.Add(SUCCESS, "SUCCESS");
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, "ERROR");
                throw;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult obtenerPromegioEvaluacion(SubContratistasDTO objDTO)
        {
            try
            {
                result.Add(ITEMS, evaluacionSubcontratistaService.obtenerPromegioEvaluacion(objDTO));
                result.Add(SUCCESS, "SUCCESS");
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, "ERROR");
                throw;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ObtenerGraficaDeBarras(SubContratistasDTO parametros)
        {
            var result = evaluacionSubcontratistaService.ObtenerGraficaDeBarras(parametros);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult obtenerPromediosxSubcontratista(SubContratistasDTO parametros)
        {
            try
            {
                result.Add(ITEMS, evaluacionSubcontratistaService.obtenerPromediosxSubcontratista(parametros));
                result.Add(SUCCESS, "SUCCESS");
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, "ERROR");
                throw;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ObtenerEvaluacionPendiente()
        {
            try
            {
                result.Add(ITEMS, evaluacionSubcontratistaService.ObtenerEvaluacionPendiente(""));
                result.Add(SUCCESS, "SUCCESS");
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, "ERROR");
                throw;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult obtenerContratistasConContrato(string AreaCuenta, int subcontratista, int Estatus, string cc)
        {
            try
            {
                result.Add(ITEMS, evaluacionSubcontratistaService.obtenerContratistasConContrato(AreaCuenta, subcontratista, Estatus, 1004, cc));
                result.Add(SUCCESS, "SUCCESS");
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, "ERROR");
                throw;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarAsignacion(SubContratistasDTO parametros)
        {
            try
            {
                result.Add(ITEMS, evaluacionSubcontratistaService.GuardarAsignacion(parametros));
                result.Add(SUCCESS, "SUCCESS");
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, "ERROR");
                throw;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getUsuariosAutorizantes(string term)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var items = evaluacionSubcontratistaService.getUsuariosAutorizantes(term);
                var lst = items.Select(y => new ComboDTO
                {
                    Value = y.id.ToString(),
                    Text = y.nombre,
                }).ToList();
                result.Add(ITEMS, lst);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult AutorizarEvaluacion(SubContratistasDTO parametros)
        //{
        //    var result = new Dictionary<string, object>();
        //    try
        //    {
        //        int idUsuario = getUsuario().id;
        //        var items = evaluacionSubcontratistaService.AutorizarEvaluacion(parametros, idUsuario);
        //        result.Add(ITEMS, items);
        //        result.Add(SUCCESS, true);
        //    }
        //    catch (Exception e)
        //    {
        //        result.Add(MESSAGE, e.Message);
        //        result.Add(SUCCESS, false);
        //    }
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult AutorizarAsignacion(SubContratistasDTO objDTO)
        {
            #region VERSIÓN ADAN
            //var result = new Dictionary<string, object>();
            //try
            //{
            //    int idUsuario = getUsuario().id;
            //    var items = evaluacionSubcontratistaService.AutorizarAsignacion(parametros, idUsuario);
            //    result.Add(ITEMS, items);
            //    result.Add(SUCCESS, true);
            //}
            //catch (Exception e)
            //{
            //    result.Add(MESSAGE, e.Message);
            //    result.Add(SUCCESS, false);
            //}
            //return Json(result, JsonRequestBehavior.AllowGet);
            #endregion
            return Json(evaluacionSubcontratistaService.AutorizarAsignacion(objDTO, (int)vSesiones.sesionUsuarioDTO.id), JsonRequestBehavior.AllowGet);
            //return Json(service.nombreMetodo(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult obtenerTodosLosElementosConSuRequerimiento()
        {
            var result = new Dictionary<string, object>();
            try
            {
                int idUsuario = getUsuario().id;
                var items = evaluacionSubcontratistaService.obtenerTodosLosElementosConSuRequerimiento();
                result.Add(ITEMS, items);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult guardarRelacion(List<ElementosDTO> lstRelacion)
        {
            var result = new Dictionary<string, object>();
            try
            {
                int idUsuario = getUsuario().id;
                var items = evaluacionSubcontratistaService.guardarRelacion(lstRelacion);
                result.Add(ITEMS, items);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult tblObtenerDashBoardSubContratista()
        {
            var result = new Dictionary<string, object>();
            try
            {
                string idUsuario = "";
                var items = evaluacionSubcontratistaService.tblObtenerDashBoardSubContratista(idUsuario);
                result.Add(ITEMS, items);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult creaExcelito(int idAsignacion)
        {
            Session["idAsignacion"] = idAsignacion;
            result.Add(SUCCESS, true);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public MemoryStream realizarExcel()
        {
            int idAsignacion = (int)Session["idAsignacion"];
            var stream = evaluacionSubcontratistaService.realizarExcel(idAsignacion);

            if (stream != null)
            {
                this.Response.Clear();
                this.Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                this.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", "reporteExcel.xlsx"));
                this.Response.BinaryWrite(stream.ToArray());
                this.Response.End();

                Session["idAsignacion"] = null;
                return stream;
            }
            else
            {
                return null;
            }
        }
        public ActionResult EvaluarDetalle(int id, List<SubContratistasDTO> parametros)
        {
            var result = new Dictionary<string, object>();
            try
            {
                int userEnvia = getUsuario().id;
                var items = evaluacionSubcontratistaService.EvaluarDetalle(id, parametros, userEnvia);
                result.Add(ITEMS, items);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult obtenerPromedioxElemento(List<SubContratistasDTO> parametros)
        {
            var result = new Dictionary<string, object>();
            try
            {

                var items = evaluacionSubcontratistaService.obtenerPromedioxElemento(parametros);
                result.Add(ITEMS, items);
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
        #region Evaluadores Proyectos
        public ActionResult evaluadroxProyectos()
        {
            return View();
        }
        public ActionResult cambiarDeColor(int idPlantilla, int idAsignacion)
        {
            return Json(evaluacionSubcontratistaService.cambiarDeColor(idPlantilla, idAsignacion), JsonRequestBehavior.AllowGet);
        }
        public ActionResult getEvaluadoresxCC(string cc, string elemento, int evaluadores)
        {
            var result = new Dictionary<string, object>();
            try
            {

                var items = evaluacionSubcontratistaService.getEvaluadoresxCC(cc, elemento, evaluadores);
                result.Add(ITEMS, items);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AgregarEditarEvaluadores(evaluadorXccDTO parametros)
        {
            var result = new Dictionary<string, object>();
            try
            {

                var items = evaluacionSubcontratistaService.AgregarEditarEvaluadores(parametros);
                result.Add(ITEMS, items);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ActivarDesactivarEvaluadores(evaluadorXccDTO parametros)
        {
            var result = new Dictionary<string, object>();
            try
            {

                var items = evaluacionSubcontratistaService.ActivarDesactivarEvaluadores(parametros);
                result.Add(ITEMS, items);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getSubContratistasRestantes()
        {
            var result = new Dictionary<string, object>();
            try
            {

                var items = evaluacionSubcontratistaService.getSubContratistasRestantes();
                result.Add(ITEMS, items);
                result.Add(SUCCESS, "SUCCESS");
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getProyectoRestantes(bool Agregar)
        {
            var result = new Dictionary<string, object>();
            try
            {

                var items = evaluacionSubcontratistaService.getProyectoRestantes(Agregar);
                result.Add(ITEMS, items);
                result.Add(SUCCESS, "SUCCESS");
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getEstrellas()
        {
            var result = new Dictionary<string, object>();
            try
            {

                var items = evaluacionSubcontratistaService.getEstrellas();
                result.Add(ITEMS, items);
                result.Add(SUCCESS, "SUCCESS");
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult obtenerTodolosElementos()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var items = evaluacionSubcontratistaService.obtenerTodolosElementos();
                result.Add(ITEMS, items);
                result.Add(SUCCESS, "SUCCESS");
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult cargarTipoUsuarios()
        {
            var result = new Dictionary<string, object>();
            try
            {
                int tipoUsuario = 1004;
                result.Add(ITEMS, tipoUsuario);
                result.Add(SUCCESS, "SUCCESS");
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult obtenerElementosEvaluar(int idPlantilla, int idAsignacion)
        {
            var result = new Dictionary<string, object>();
            try
            {
                int idUsuarioLogeado = getUsuario().id;
                var items = evaluacionSubcontratistaService.obtenerElementosEvaluar(idUsuarioLogeado, idPlantilla, idAsignacion);
                //var elementoVerde = Json(evaluacionSubcontratistaService.obtenerElementosEvaluar(idUsuarioLogeado, idPlantilla, idAsignacion), JsonRequestBehavior.AllowGet);
                result.Add(ITEMS, items);
                //result.Add("elementoVerde", elementoVerde);
                result.Add(SUCCESS, "SUCCESS");
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ObtenerGraficaDeEvaluacionPorCentroDeCosto(SubContratistasDTO parametros)
        {
            return Json(evaluacionSubcontratistaService.ObtenerGraficaDeEvaluacionPorCentroDeCosto(parametros), JsonRequestBehavior.AllowGet);
        }
        public ActionResult ObtenerGraficaDeEvaluacionPorDivisionElemento(SubContratistasDTO parametros)
        {
            return Json(evaluacionSubcontratistaService.ObtenerGraficaDeEvaluacionPorDivisionElemento(parametros), JsonRequestBehavior.AllowGet);
        }
        public ActionResult ObtenerGraficaDeEvaluacionPorTendenciaMedianteEvaluaciones(SubContratistasDTO parametros)
        {
            return Json(evaluacionSubcontratistaService.ObtenerGraficaDeEvaluacionPorTendenciaMedianteEvaluaciones(parametros), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region CATALOGO DE NOTIFICANTES
        //public ActionResult CatalogoUsuariosExpediente()
        //{
        //    Respuesta respuesta = evaluacionSubcontratistaService.PermisoVista((int)VistasEnum.usuarioExpediente);
        //    if (respuesta.Success)
        //        return View();
        //    else
        //        return View("ErrorPermisoVista");
        //}
        public ActionResult CatalogoAltaDeNotificantes()
        {
            return View();
        }

        public ActionResult fillComboPrestadoresDeServicio()
        {
            return Json(evaluacionSubcontratistaService.fillComboPrestadoresDeServicio(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult AccionesUsuariosExpediente(tblPUsuarioDTO objUsuario, int Accion)
        {
            return Json(evaluacionSubcontratistaService.AccionesUsuariosExpediente(objUsuario, Accion), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region CATALAOGO DE FACULTAMIENTOS

        //public ViewResult CatalogoDeFacultamientos()
        //{
        //    Respuesta respuesta = evaluacionSubcontratistaService.PermisoVista((int)VistasEnum.facultamiento);
        //    if (respuesta.Success)
        //        return View();
        //    else
        //        return View("ErrorPermisoVista");
        //}
        public ActionResult AccionesFacultamientos(facultamientosCODTO objUsuario, int Accion)
        {
            return Json(evaluacionSubcontratistaService.AccionesFacultamientos(objUsuario, Accion), JsonRequestBehavior.AllowGet);
        }


        #endregion
        public ActionResult cboUsuarios()
        {
            return Json(evaluacionSubcontratistaService.cboUsuarios(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult VerificarTipoUsuario()
        {
            return Json(evaluacionSubcontratistaService.VerificarTipoUsuario(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboEstados()
        {
            return Json(evaluacionSubcontratistaService.FillCboEstados(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult FillCboMunicipios(int idEstado)
        {
            return Json(evaluacionSubcontratistaService.FillCboMunicipios(idEstado), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region VIEWS
        //public ActionResult UsuariosRelSubcontratistas()
        //{
        //    Respuesta respuesta = evaluacionSubcontratistaService.PermisoVista((int)VistasEnum.firmantes);
        //    if (respuesta.Success)
        //        return View();
        //    else
        //        return View("ErrorPermisoVista");
        //}

        public ActionResult Calendario()
        {
            Respuesta respuesta = evaluacionSubcontratistaService.PermisoVista((int)VistasEnum.calendario);
            if (respuesta.Success)
                return View();
            else
                return View("ErrorPermisoVista");
        }

        public ActionResult Catalogos()
        {
            return View();
        }
        #endregion

        #region ADMINISTRACIÓN DE EVALUACIONES
        public ActionResult VerificarElementoTerminado(EvaluacionDTO objDTO)
        {
            return Json(evaluacionSubcontratistaService.VerificarElementoTerminado(objDTO), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region CATÁLOGO DE EVALUADORES
        public ActionResult FillCboFiltroEvaluadores()
        {
            return Json(evaluacionSubcontratistaService.FillCboFiltroEvaluadores(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboCEEvaluadores()
        {
            return Json(evaluacionSubcontratistaService.FillCboCEEvaluadores(), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region CATALOGO DE FACULTAMIENTOS
        public ActionResult GetListadoUsuarioRelCC(int id)
        {
            return Json(evaluacionSubcontratistaService.GetListadoUsuarioRelCC(id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCCActualizarFacultamiento(int id)
        {
            return Json(evaluacionSubcontratistaService.GetCCActualizarFacultamiento(id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillTipoUsuarioFacultamientos()
        {
            return Json(evaluacionSubcontratistaService.FillTipoUsuarioFacultamientos(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListadoCCRelUsuarioFacultamientos(int facultamiento_id)
        {
            return Json(evaluacionSubcontratistaService.GetListadoCCRelUsuarioFacultamientos(facultamiento_id), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region CATALOGO FIRMASTES ENCARGADOS DE CIERTOS CONTRATOS
        public ActionResult GetUsuariosRelSubcontratistas(UsuarioRelSubcontratistaDTO objDTO)
        {
            return Json(evaluacionSubcontratistaService.GetUsuariosRelSubcontratistas(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CEUsuarioRelSubcontratista(UsuarioRelSubcontratistaDTO objDTO)
        {
            return Json(evaluacionSubcontratistaService.CEUsuarioRelSubcontratista(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarUsuarioRelSubcontratista(UsuarioRelSubcontratistaDTO objDTO)
        {
            return Json(evaluacionSubcontratistaService.EliminarUsuarioRelSubcontratista(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult MandarUsuarioComoHistorial(UsuarioRelSubcontratistaDTO objDTO)
        {
            return Json(evaluacionSubcontratistaService.MandarUsuarioComoHistorial(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDatosActualizarUsuarioRelSubcontratista(UsuarioRelSubcontratistaDTO objDTO)
        {
            return Json(evaluacionSubcontratistaService.GetDatosActualizarUsuarioRelSubcontratista(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboContratosRelSubcontratistas(int idSubcontratista)
        {
            return Json(evaluacionSubcontratistaService.FillCboContratosRelSubcontratistas(idSubcontratista), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region CALENDARIO
        public ActionResult FillCboEvaluacionesActivas(SubContratistasDTO objDTO)
        {
            return Json(evaluacionSubcontratistaService.FillCboEvaluacionesActivas(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFechasEvaluaciones(UsuarioRelSubcontratistaDTO objDTO)
        {
            return Json(evaluacionSubcontratistaService.GetFechasEvaluaciones(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ActualizarFechasActualizacion(CalendarioDTO objDTO)
        {
            return Json(evaluacionSubcontratistaService.ActualizarFechasActualizacion(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFechasActualizar(CalendarioDTO objDTO)
        {
            return Json(evaluacionSubcontratistaService.GetFechasActualizar(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTipoUsuario()
        {
            return Json(evaluacionSubcontratistaService.GetTipoUsuario(), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region CATÁLOGO DE PLANTILLAS
        public ActionResult FillComboContratos()
        {
            return Json(evaluacionSubcontratistaService.FillComboContratos(), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region REPORTE CRYSTAL REPORT
        public ActionResult GetEvaluaciones(int idAsignacion)
        {
            return Json(evaluacionSubcontratistaService.GetEvaluaciones(idAsignacion), JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult GetUsuariosAutocomplete(string term, bool porClave)
        {
            return Json(evaluacionSubcontratistaService.GetUsuariosAutocomplete(term, porClave), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboElementos()
        {
            return Json(evaluacionSubcontratistaService.FillComboElementos(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboRequerimientos(int elemento_id)
        {
            return Json(evaluacionSubcontratistaService.FillComboRequerimientos(elemento_id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarNuevoElemento(tblCOES_Elemento elemento)
        {
            return Json(evaluacionSubcontratistaService.GuardarNuevoElemento(elemento), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarNuevoRequerimiento(tblCOES_Requerimiento requerimiento)
        {
            return Json(evaluacionSubcontratistaService.GuardarNuevoRequerimiento(requerimiento), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRequerimientosElemento(int elemento_id)
        {
            return Json(evaluacionSubcontratistaService.GetRequerimientosElemento(elemento_id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarNuevaPlantilla(tblCOES_Plantilla plantilla, List<int> contratos, List<RequerimientoDTO> requerimientos)
        {
            return Json(evaluacionSubcontratistaService.GuardarNuevaPlantilla(plantilla, contratos, requerimientos), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarPlantilla(tblCOES_Plantilla plantilla, List<int> contratos, List<RequerimientoDTO> requerimientos)
        {
            return Json(evaluacionSubcontratistaService.EditarPlantilla(plantilla, contratos, requerimientos), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarRequerimientoElemento(int requerimiento_id)
        {
            return Json(evaluacionSubcontratistaService.EliminarRequerimientoElemento(requerimiento_id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPlantilla(int plantilla_id)
        {
            return Json(evaluacionSubcontratistaService.GetPlantilla(plantilla_id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CopiarPlantillaBase(int plantilla_id)
        {
            return Json(evaluacionSubcontratistaService.CopiarPlantillaBase(plantilla_id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarPlantilla(int plantilla_id)
        {
            return Json(evaluacionSubcontratistaService.EliminarPlantilla(plantilla_id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboProyectos()
        {
            return Json(evaluacionSubcontratistaService.FillComboProyectos(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFacultamientoUsuario()
        {
            return Json(evaluacionSubcontratistaService.GetFacultamientoUsuario(), JsonRequestBehavior.AllowGet);
        }

        #region Evaluadores
        public ActionResult GetEvaluadores(string cc, int elemento)
        {
            return Json(evaluacionSubcontratistaService.GetEvaluadores(cc, elemento), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarNuevoEvaluador(tblCOES_Evaluador evaluador, List<tblCOES_Evaluador_Proyecto> proyectos, List<tblCOES_Evaluador_Elemento> elementos)
        {
            return Json(evaluacionSubcontratistaService.GuardarNuevoEvaluador(evaluador, proyectos, elementos), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarEvaluador(tblCOES_Evaluador evaluador, List<tblCOES_Evaluador_Proyecto> proyectos, List<tblCOES_Evaluador_Elemento> elementos)
        {
            return Json(evaluacionSubcontratistaService.EditarEvaluador(evaluador, proyectos, elementos), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarEvaluador(int evaluador_id)
        {
            return Json(evaluacionSubcontratistaService.EliminarEvaluador(evaluador_id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEvaluador(int evaluador_id)
        {
            return Json(evaluacionSubcontratistaService.GetEvaluador(evaluador_id), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Facultamientos
        public ActionResult GetFacultamientos(string cc, TipoFacultamientoEnum tipo)
        {
            return Json(evaluacionSubcontratistaService.GetFacultamientos(cc, tipo), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarNuevoFacultamiento(tblCOES_Facultamiento facultamiento, List<tblCOES_Facultamiento_CentroCosto> proyectos)
        {
            return Json(evaluacionSubcontratistaService.GuardarNuevoFacultamiento(facultamiento, proyectos), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarFacultamiento(tblCOES_Facultamiento facultamiento, List<tblCOES_Facultamiento_CentroCosto> proyectos)
        {
            return Json(evaluacionSubcontratistaService.EditarFacultamiento(facultamiento, proyectos), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarFacultamiento(int facultamiento_id)
        {
            return Json(evaluacionSubcontratistaService.EliminarFacultamiento(facultamiento_id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFacultamiento(int facultamiento_id)
        {
            return Json(evaluacionSubcontratistaService.GetFacultamiento(facultamiento_id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTipoFacultamientoCombo()
        {
            var listaPrivilegios = GlobalUtils.ParseEnumToCombo<TipoFacultamientoEnum>().ToList();

            listaPrivilegios.RemoveAt(0);

            result.Add(ITEMS, listaPrivilegios);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Firmas Subcontratistas
        public ActionResult GetFirmaSubcontratistas(int subcontratista_id)
        {
            return Json(evaluacionSubcontratistaService.GetFirmaSubcontratistas(subcontratista_id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarNuevaFirmaSubcontratista(tblCOES_FirmaSubcontratista firma, List<tblCOES_FirmaSubcontratistatblX_Contrato> contratos)
        {
            return Json(evaluacionSubcontratistaService.GuardarNuevaFirmaSubcontratista(firma, contratos), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarFirmaSubcontratista(tblCOES_FirmaSubcontratista firma, List<tblCOES_FirmaSubcontratistatblX_Contrato> contratos)
        {
            return Json(evaluacionSubcontratistaService.EditarFirmaSubcontratista(firma, contratos), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarFirmaSubcontratista(int firma_id)
        {
            return Json(evaluacionSubcontratistaService.EliminarFirmaSubcontratista(firma_id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFirmaSubcontratista(int firma_id)
        {
            return Json(evaluacionSubcontratistaService.GetFirmaSubcontratista(firma_id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboSubcontratistas()
        {
            return Json(evaluacionSubcontratistaService.FillComboSubcontratistas(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EnviarCorreoNotificacionFirma(int firma_id)
        {
            return Json(evaluacionSubcontratistaService.EnviarCorreoNotificacionFirma(firma_id), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Firmas Gerentes
        public ActionResult GetFirmaGerentes(string cc)
        {
            return Json(evaluacionSubcontratistaService.GetFirmaGerentes(cc), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarNuevaFirmaGerente(tblCOES_FirmaGerente firma)
        {
            return Json(evaluacionSubcontratistaService.GuardarNuevaFirmaGerente(firma), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarFirmaGerente(tblCOES_FirmaGerente firma)
        {
            return Json(evaluacionSubcontratistaService.EditarFirmaGerente(firma), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarFirmaGerente(int firma_id)
        {
            return Json(evaluacionSubcontratistaService.EliminarFirmaGerente(firma_id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFirmaGerente(int firma_id)
        {
            return Json(evaluacionSubcontratistaService.GetFirmaGerente(firma_id), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Especialidades
        public ActionResult FillComboEspecialidades()
        {
            return Json(evaluacionSubcontratistaService.FillComboEspecialidades(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSubcontratistasEspecialidad(string cc)
        {
            return Json(evaluacionSubcontratistaService.GetSubcontratistasEspecialidad(cc), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarEspecialidadesSubcontratista(int subcontratista_id, List<tblCOES_EspecialidadtblX_SubContratista> especialidades)
        {
            return Json(evaluacionSubcontratistaService.GuardarEspecialidadesSubcontratista(subcontratista_id, especialidades), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Administración Evaluaciones
        public ActionResult CargarEvaluacionesSubcontratistas(string cc, int subcontratista_id, EstatusEvaluacionEnum estatus)
        {
            return Json(evaluacionSubcontratistaService.CargarEvaluacionesSubcontratistas(cc, subcontratista_id, estatus), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarAsignacionEvaluacion(tblCOES_Asignacion asignacion, List<tblCOES_Asignacion_Evaluacion> evaluaciones)
        {
            return Json(evaluacionSubcontratistaService.GuardarAsignacionEvaluacion(asignacion, evaluaciones), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetContratoInformacion(int contrato_id)
        {
            return Json(evaluacionSubcontratistaService.GetContratoInformacion(contrato_id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetElementosEvaluacion(int contrato_id, int evaluacion_id)
        {
            return Json(evaluacionSubcontratistaService.GetElementosEvaluacion(contrato_id, evaluacion_id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarRetroalimentacionEvaluador(tblCOES_Evidencia evidencia)
        {
            return Json(evaluacionSubcontratistaService.GuardarRetroalimentacionEvaluador(evidencia), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarEvaluacionSubcontratista()
        {
            return Json(evaluacionSubcontratistaService.GuardarEvaluacionSubcontratista(), JsonRequestBehavior.AllowGet);
        }

        public FileResult GetArchivoEvidencia()
        {
            try
            {
                int evidencia_id = Convert.ToInt32(Request.QueryString["id"]);
                var archivo = evaluacionSubcontratistaService.GetArchivoEvidencia(evidencia_id);

#if DEBUG
                archivo.rutaArchivo = archivo.rutaArchivo.Replace("\\\\10.1.0.112", "C:");
#endif

                return File(archivo.rutaArchivo, "multipart/form-data", Path.GetFileName(archivo.rutaArchivo));
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult EnviarGestionFirmas(int evaluacion_id, int contrato_id, int subcontratista_id)
        {
            return Json(evaluacionSubcontratistaService.EnviarGestionFirmas(evaluacion_id, contrato_id, subcontratista_id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSeguimientoFirmas(int evaluacion_id, int contrato_id)
        {
            return Json(evaluacionSubcontratistaService.GetSeguimientoFirmas(evaluacion_id, contrato_id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult AutorizarEvaluacion(HttpPostedFileBase archivoFirma)
        {
            var firma_id = Convert.ToInt32(Request.Form["firma_id"]);

            return Json(evaluacionSubcontratistaService.AutorizarEvaluacion(firma_id, archivoFirma), JsonRequestBehavior.AllowGet);
        }

        public ActionResult RechazarEvaluacion(int firma_id)
        {
            return Json(evaluacionSubcontratistaService.RechazarEvaluacion(firma_id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAsignacionContrato(int asignacion_id)
        {
            return Json(evaluacionSubcontratistaService.GetAsignacionContrato(asignacion_id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarCambioEvaluacion(tblCOES_CambioEvaluacion cambio)
        {
            return Json(evaluacionSubcontratistaService.GuardarCambioEvaluacion(cambio), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCambioEvaluacion(int cambioEvaluacion_id)
        {
            return Json(evaluacionSubcontratistaService.GetCambioEvaluacion(cambioEvaluacion_id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarAutorizacionCambioEvaluacion(tblCOES_CambioEvaluacion cambio)
        {
            return Json(evaluacionSubcontratistaService.GuardarAutorizacionCambioEvaluacion(cambio), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarGraficasSubcontratista(string cc, int subcontratista_id, int contrato_id, EstatusEvaluacionEnum estatus)
        {
            return Json(evaluacionSubcontratistaService.CargarGraficasSubcontratista(cc, subcontratista_id, contrato_id, estatus), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Calendario Evaluaciones
        public ActionResult llenarCalendarioEvaluaciones(List<string> lstFiltroCC, List<int?> lstFiltroSubC)
        {
            return Json(evaluacionSubcontratistaService.llenarCalendarioEvaluaciones(lstFiltroCC, lstFiltroSubC), JsonRequestBehavior.AllowGet);
        }
        public ActionResult buscarEvaluaciones(List<string> cc, List<string> subContratistas)
        {
            return Json(evaluacionSubcontratistaService.buscarEvaluaciones(cc,subContratistas), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboSubcontratistasSorted()
        {
            var dictFillCombo = evaluacionSubcontratistaService.FillComboSubcontratistas();

            var lstSubC = dictFillCombo["items"] as List<ComboDTO>;

            dictFillCombo["items"] = lstSubC.OrderBy(e => e.Text).ToList();

            return Json(dictFillCombo, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveImgSession(string img){

            var dictResult = new Dictionary<string,object>();
            dictResult.Add(SUCCESS, true);

            Session["imgCalendarioSubcontratistas"] = img;

            return Json(dictResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboProyectosUnicos()
        {
            var dictFillCombo = evaluacionSubcontratistaService.FillComboProyectos();

            var lstCC = dictFillCombo["items"] as List<ComboDTO>;
            var lstIDsCCs = lstCC.Where(e => e.Value != "0").Select(e => e.Value.Trim()).Distinct().ToList();
            var lsCCU = new List<ComboDTO>();

            foreach (var item in lstIDsCCs)
	        {
                var objProyecto = lsCCU.FirstOrDefault(e => e.Value.Trim() == item);
                var objProyectoU = lstCC.FirstOrDefault(e => e.Value.Trim() == item);

                if (objProyecto == null)
	            {
		            lsCCU.Add(objProyectoU);
	            }
	        }


            dictFillCombo["items"] = lsCCU;

            //return Json(dictFillCombo, JsonRequestBehavior.AllowGet);

            return Json(dictFillCombo, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region DASHBOARD

        #region GENERAL
        public ActionResult GetGraficaCumplimientoPorSubContratista(List<string> lstFiltroCC, List<int> lstFiltroSubC, DateTime fechaFiltroInicio, DateTime fechaFiltroFin, int estado_id, int municipio_id, List<int> listaEspecialidades)
        {
            return Json(evaluacionSubcontratistaService.GetGraficaCumplimientoPorSubContratista(lstFiltroCC, lstFiltroSubC, fechaFiltroInicio, fechaFiltroFin, estado_id, municipio_id, listaEspecialidades), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetGraficaCumplimientoPorElementos(List<string> lstFiltroCC, List<int> lstFiltroSubC, DateTime fechaFiltroInicio, DateTime fechaFiltroFin, int estado_id, int municipio_id, List<int> listaEspecialidades)
        {
            return Json(evaluacionSubcontratistaService.GetGraficaCumplimientoPorElementos(lstFiltroCC, lstFiltroSubC, fechaFiltroInicio, fechaFiltroFin, estado_id, municipio_id, listaEspecialidades), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetGraficaCumplimientoPorEvaluacion(List<string> lstFiltroCC, List<int> lstFiltroSubC, DateTime fechaFiltroInicio, DateTime fechaFiltroFin, int estado_id, int municipio_id, List<int> listaEspecialidades)
        {
            return Json(evaluacionSubcontratistaService.GetGraficaCumplimientoPorEvaluacion(lstFiltroCC, lstFiltroSubC, fechaFiltroInicio, fechaFiltroFin, estado_id, municipio_id, listaEspecialidades), JsonRequestBehavior.AllowGet);
        }

        public ActionResult creaVariableDeSesion(List<string> lstFiltroCC, List<int> lstFiltroSubC, DateTime fechaFiltroInicio, DateTime fechaFiltroFin, int estado_id, int municipio_id, List<int> listaEspecialidades)
        {
            var result = new Dictionary<string, object>();
            Session["Documento"] = evaluacionSubcontratistaService.crearReporte(lstFiltroCC, lstFiltroSubC, fechaFiltroInicio, fechaFiltroFin, estado_id, municipio_id, listaEspecialidades);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public MemoryStream crearReporte()
        {
            var result = new Dictionary<string, object>();
            MemoryStream stream = (MemoryStream)Session["Documento"];

            if (stream != null)
            {
                this.Response.Clear();
                this.Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                this.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", "Reporte evaluacion subcontratistas.xlsx"));
                this.Response.BinaryWrite(stream.ToArray());
                this.Response.End();

                Session["Documento"] = null;
                return stream;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #endregion

        #region Dashboard
        public ActionResult FillComboEspecialidad()
        {
            return Json(evaluacionSubcontratistaService.FillComboEspecialidad(),JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCumplimientosElementos(List<string> lstfiltroCC, List<int> lstfiltroSubC, DateTime fechaFiltroInicio, DateTime fechaFiltroFin, int idElemento, int estado_id, int municipio_id, List<int> listaEspecialidades)
        {
            return Json(evaluacionSubcontratistaService.GetCumplimientosElementos(lstfiltroCC, lstfiltroSubC, fechaFiltroInicio, fechaFiltroFin,idElemento, estado_id, municipio_id, listaEspecialidades), JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveImgSessionDashboard(string graficaSubcontratista, string graficaElementos, string graficaEvaluacion)
        {
            var dictResult = new Dictionary<string, object>();
            dictResult.Add(SUCCESS, true);
            Session["graficaSubcontratistaDashboard"] = graficaSubcontratista;
            Session["graficaElementosDashboard"] = graficaElementos;
            Session["graficaEvaluacionDashboard"] = graficaEvaluacion;

            return Json(dictResult, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult FillComboEstados()
        {
            return Json(evaluacionSubcontratistaService.FillComboEstados(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillComboMunicipios(int estado_id)
        {
            return Json(evaluacionSubcontratistaService.FillComboMunicipios(estado_id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetReporteEjecutivo(List<string> lstfiltroCC, List<int> lstfiltroSubC, DateTime fechaFiltroInicio, DateTime fechaFiltroFin, int estado_id, int municipio_id, List<int> listaEspecialidades)
        {
            return Json(evaluacionSubcontratistaService.GetReporteEjecutivo(lstfiltroCC, lstfiltroSubC, fechaFiltroInicio, fechaFiltroFin, estado_id, municipio_id, listaEspecialidades), JsonRequestBehavior.AllowGet);
        }
    }
}