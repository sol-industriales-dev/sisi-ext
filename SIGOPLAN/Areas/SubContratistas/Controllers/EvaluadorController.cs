using Core.DAO.Subcontratistas;
using Core.DTO.ControlObra;
using Core.DTO.ControlObra.EvaluacionSubcontratista;
using Core.DTO.Principal.Generales;
using Core.DTO.Subcontratistas.Evaluacion;
using Data.Factory.Subcontratistas;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.SubContratistas.Controllers
{
    public class EvaluadorController : BaseController
    {
        private EvaluadorFactoryService ControlObraFactoryService;
        private IEvaluadorDAO evaluacionSubcontratistaService;
        Dictionary<string, object> result;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            evaluacionSubcontratistaService = new EvaluadorFactoryService().GetEvaluacionSubcontratistaService();
            ControlObraFactoryService = new EvaluadorFactoryService();
            result = new Dictionary<string, object>();
            base.OnActionExecuting(filterContext);
        }

        public ViewResult EvaluadorSubContratista()
        {
            return View();
        }

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
                result.Add(ITEMS, ControlObraFactoryService.GetEvaluacionSubcontratistaService().getProyecto(idUsuario));
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
                result.Add(ITEMS, ControlObraFactoryService.GetEvaluacionSubcontratistaService().getSubContratistas(AreaCuenta));
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
                result.Add(ITEMS, ControlObraFactoryService.GetEvaluacionSubcontratistaService().getTblSubContratista(parametros));
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
        public JsonResult addEditSubContratista(List<HttpPostedFileBase> Archivo, List<SubContratistasDTO> parametros, bool subcontratista = false)
        {
            try
            {
                List<SubContratistasDTO> parameters = new List<SubContratistasDTO>();
                foreach (var item in parametros)
                {
                    item.idSubContratista = getUsuario().id;
                    parameters.Add(item);
                }
                tblPUsuarioDTO tblp = new tblPUsuarioDTO();
                tblp.id = getUsuario().id;
                result.Add(ITEMS, ControlObraFactoryService.GetEvaluacionSubcontratistaService().addEditSubContratista(Archivo, parameters, tblp, subcontratista));
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
                result.Add(ITEMS, ControlObraFactoryService.GetEvaluacionSubcontratistaService().CargarArchivosXSubcontratista(parametros));
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
                //parametros.RFC = getUsuario()._user;
                //result.Add(ITEMS, ControlObraFactoryService.GetEvaluacionSubcontratistaService().CargarArchivosSubcontratista(parametros, getUsuario()));
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
                result.Add(ITEMS, ControlObraFactoryService.GetEvaluacionSubcontratistaService().CargarArchivosXSubcontratistaEvaluacion(parametros));
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

                result.Add(ITEMS, ControlObraFactoryService.GetEvaluacionSubcontratistaService().ObtenerTblAutorizacion(parametros));
                result.Add(SUCCESS, "SUCCESS");
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, "ERROR");
                throw;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult obtenerPlantillas()
        {
            return Json(ControlObraFactoryService.GetEvaluacionSubcontratistaService().obtenerPlantillas(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult addEditPlantilla(DivicionesMenuDTO parametros)
        {
            try
            {
                result.Add(ITEMS, ControlObraFactoryService.GetEvaluacionSubcontratistaService().addEditPlantilla(parametros));
                result.Add(SUCCESS, "SUCCESS");
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, "ERROR");
                throw;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult cboObtenerContratos()
        {
            try
            {
                var rt = ControlObraFactoryService.GetEvaluacionSubcontratistaService().cboObtenerContratos();
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
                var rt = ControlObraFactoryService.GetEvaluacionSubcontratistaService().cboObtenerContratosInclu(idPlantilla);
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
        public ActionResult eliminarPlantilla(int id)
        {
            return Json(ControlObraFactoryService.GetEvaluacionSubcontratistaService().eliminarPlantilla(id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult obtenerDiviciones(int idPlantilla, int idAsignacion)
        {
            try
            {
                result.Add(ITEMS, ControlObraFactoryService.GetEvaluacionSubcontratistaService().obtenerDiviciones(idPlantilla, idAsignacion));
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

                result.Add(ITEMS, ControlObraFactoryService.GetEvaluacionSubcontratistaService().addEditDiviciones(parametros));
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
                result.Add(ITEMS, ControlObraFactoryService.GetEvaluacionSubcontratistaService().eliminarDiviciones(id));
                result.Add(SUCCESS, "SUCCESS");
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, "ERROR");
                throw;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult obtenerRequerimientos(int idDiv, int? idAsignacion)
        {
            try
            {
                result.Add(ITEMS, ControlObraFactoryService.GetEvaluacionSubcontratistaService().obtenerRequerimientos(idDiv, idAsignacion));
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
                result.Add(ITEMS, ControlObraFactoryService.GetEvaluacionSubcontratistaService().obtenerDivicionesEvaluador());
                result.Add(SUCCESS, "SUCCESS");
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, "ERROR");
                throw;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult obtenerDivicionesEvaluadorArchivos(int idPlantilla, int idAsignacion)
        {
            try
            {
                result.Add(ITEMS, ControlObraFactoryService.GetEvaluacionSubcontratistaService().obtenerDivicionesEvaluadorArchivos(idPlantilla, idAsignacion));
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
            var array = ControlObraFactoryService.GetEvaluacionSubcontratistaService().DescargarArchivos(idDet);
            string pathExamen = ControlObraFactoryService.GetEvaluacionSubcontratistaService().getFileName(idDet);

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
                result.Add(ITEMS, ControlObraFactoryService.GetEvaluacionSubcontratistaService().obtenerEvaluacionxReq(parametros));
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
                result.Add(ITEMS, ControlObraFactoryService.GetEvaluacionSubcontratistaService().GuardarEvaluacion(parametros));
                result.Add(SUCCESS, "SUCCESS");
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, "ERROR");
                throw;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult obtenerPromegioEvaluacion(SubContratistasDTO parametros)
        {
            try
            {
                result.Add(ITEMS, ControlObraFactoryService.GetEvaluacionSubcontratistaService().obtenerPromegioEvaluacion(parametros));
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
            var result = ControlObraFactoryService.GetEvaluacionSubcontratistaService().ObtenerGraficaDeBarras(parametros);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult obtenerPromediosxSubcontratista(SubContratistasDTO parametros)
        {
            try
            {
                result.Add(ITEMS, ControlObraFactoryService.GetEvaluacionSubcontratistaService().obtenerPromediosxSubcontratista(parametros));
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
                tblPUsuarioDTO tblp = new tblPUsuarioDTO();
                tblp.id = getUsuario().id;
                result.Add(ITEMS, ControlObraFactoryService.GetEvaluacionSubcontratistaService().ObtenerEvaluacionPendiente(tblp));
                result.Add(SUCCESS, "SUCCESS");
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, "ERROR");
                throw;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult obtenerContratistasConContrato(string AreaCuenta, int subcontratista, int Estatus)
        {
            try
            {
                tblPUsuarioDTO tblp = new tblPUsuarioDTO();
                tblp.id = getUsuario().id;
                result.Add(ITEMS, ControlObraFactoryService.GetEvaluacionSubcontratistaService().obtenerContratistasConContrato(AreaCuenta, subcontratista, Estatus, tblp));
                result.Add(SUCCESS, "SUCCESS");
            }
            catch (Exception ex)
            {
                result.Add(SUCCESS, "ERROR");
                throw;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GuardarEvaluacionSubContratista(SubContratistasDTO parametros)
        {
            try
            {
                result.Add(ITEMS, ControlObraFactoryService.GetEvaluacionSubcontratistaService().GuardarEvaluacionSubContratista(parametros));
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
                result.Add(ITEMS, ControlObraFactoryService.GetEvaluacionSubcontratistaService().GuardarAsignacion(parametros));
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
                var items = ControlObraFactoryService.GetEvaluacionSubcontratistaService().getUsuariosAutorizantes(term);
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
        public ActionResult AutorizarEvaluacion(SubContratistasDTO parametros)
        {
            var result = new Dictionary<string, object>();
            try
            {
                int idUsuario = getUsuario().id;
                var items = ControlObraFactoryService.GetEvaluacionSubcontratistaService().AutorizarEvaluacion(parametros, idUsuario);
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
        public ActionResult AutorizarAsignacion(SubContratistasDTO parametros)
        {
            var result = new Dictionary<string, object>();
            try
            {
                int idUsuario = getUsuario().id;
                var items = ControlObraFactoryService.GetEvaluacionSubcontratistaService().AutorizarAsignacion(parametros, idUsuario);
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

        public ActionResult obtenerTodosLosElementosConSuRequerimiento()
        {
            var result = new Dictionary<string, object>();
            try
            {
                int idUsuario = getUsuario().id;
                var items = ControlObraFactoryService.GetEvaluacionSubcontratistaService().obtenerTodosLosElementosConSuRequerimiento();
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
                var items = ControlObraFactoryService.GetEvaluacionSubcontratistaService().guardarRelacion(lstRelacion);
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
                tblPUsuarioDTO tblp = new tblPUsuarioDTO();
                tblp.id = getUsuario().id;
                var items = ControlObraFactoryService.GetEvaluacionSubcontratistaService().tblObtenerDashBoardSubContratista(tblp);
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
            var stream = ControlObraFactoryService.GetEvaluacionSubcontratistaService().realizarExcel(idAsignacion);

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
                var items = ControlObraFactoryService.GetEvaluacionSubcontratistaService().EvaluarDetalle(id, parametros, userEnvia);
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

                var items = ControlObraFactoryService.GetEvaluacionSubcontratistaService().obtenerPromedioxElemento(parametros);
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

        public ActionResult evaluadroxProyectos()
        {
            return View();
        }
        public ActionResult getEvaluadoresxCC()
        {
            var result = new Dictionary<string, object>();
            try
            {

                var items = ControlObraFactoryService.GetEvaluacionSubcontratistaService().getEvaluadoresxCC();
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

                var items = ControlObraFactoryService.GetEvaluacionSubcontratistaService().AgregarEditarEvaluadores(parametros);
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

                var items = ControlObraFactoryService.GetEvaluacionSubcontratistaService().ActivarDesactivarEvaluadores(parametros);
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

                var items = ControlObraFactoryService.GetEvaluacionSubcontratistaService().getSubContratistasRestantes();
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

                var items = ControlObraFactoryService.GetEvaluacionSubcontratistaService().getProyectoRestantes(Agregar);
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

                var items = ControlObraFactoryService.GetEvaluacionSubcontratistaService().getEstrellas();
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
                var items = ControlObraFactoryService.GetEvaluacionSubcontratistaService().obtenerTodolosElementos();
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
                int tipoUsuario = 10;
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

        public ActionResult obtenerElementosEvaluar(int idPlantilla)
        {
            var result = new Dictionary<string, object>();
            try
            {
                int idUsuarioLogeado = getUsuario().id;
                var items = ControlObraFactoryService.GetEvaluacionSubcontratistaService().obtenerElementosEvaluar(idUsuarioLogeado, idPlantilla);
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
        public ActionResult ObtenerGraficaDeEvaluacionPorCentroDeCosto(SubContratistasDTO parametros)
        {
            return Json(ControlObraFactoryService.GetEvaluacionSubcontratistaService().ObtenerGraficaDeEvaluacionPorCentroDeCosto(parametros), JsonRequestBehavior.AllowGet);
        }
        public ActionResult ObtenerGraficaDeEvaluacionPorDivisionElemento(SubContratistasDTO parametros)
        {
            return Json(ControlObraFactoryService.GetEvaluacionSubcontratistaService().ObtenerGraficaDeEvaluacionPorDivisionElemento(parametros), JsonRequestBehavior.AllowGet);
        }
        public ActionResult ObtenerGraficaDeEvaluacionPorTendenciaMedianteEvaluaciones(SubContratistasDTO parametros)
        {
            return Json(ControlObraFactoryService.GetEvaluacionSubcontratistaService().ObtenerGraficaDeEvaluacionPorTendenciaMedianteEvaluaciones(parametros), JsonRequestBehavior.AllowGet);
        }


        //#region Gestión
        //public ViewResult Gestion()
        //{
        //    return View();
        //}

        //[HttpGet]
        //public JsonResult ObtenerProyectosParaFiltro()
        //{
        //    return Json(evaluacionSubcontratistaService.ObtenerProyectosParaFiltro(), JsonRequestBehavior.AllowGet);
        //}

        //[HttpGet]
        //public JsonResult ObtenerSubcontratistasParaFiltro(string proyecto)
        //{
        //    return Json(evaluacionSubcontratistaService.ObtenerSubcontratistasParaFiltro(proyecto), JsonRequestBehavior.AllowGet);
        //}

        //[HttpGet]
        //public JsonResult ObtenerEvaluacionesSubcontratistas(string proyecto, int? subcontratistaId)
        //{
        //    return Json(evaluacionSubcontratistaService.ObtenerEvaluacionesSubcontratistas(proyecto, subcontratistaId), JsonRequestBehavior.AllowGet);
        //}

        //[HttpGet]
        //public JsonResult ObtenerEstatusFirmantes(int evaluacionId)
        //{
        //    return Json(evaluacionSubcontratistaService.ObtenerEstatusFirmantes(evaluacionId), JsonRequestBehavior.AllowGet);
        //}

        //[HttpGet]
        //public JsonResult ObtenerFirmante(int evaluacionId)
        //{
        //    return Json(evaluacionSubcontratistaService.ObtenerFirmante(evaluacionId), JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //public JsonResult GuardarFirma(InformacionFirmaDigitalDTO firma)
        //{
        //    return Json(evaluacionSubcontratistaService.GuardarFirma(firma));
        //}
        //#endregion

        public JsonResult cambiarDeColor(int idPlantilla, int idAsignacion)
        {
            return Json(evaluacionSubcontratistaService.cambiarDeColor(idPlantilla, idAsignacion));
        }
        public ActionResult obtenerPromedioGeneral(int id)
        {
            return Json(evaluacionSubcontratistaService.obtenerPromedioGeneral(id), JsonRequestBehavior.AllowGet);
        }
    }
}