using Core.DAO.RecursosHumanos.Evaluacion360;
using Core.DTO.RecursosHumanos.Evaluacion360;
using Core.Enum.RecursosHumanos.Evaluacion360;
using Data.Factory.RecursosHumanos.Evaluacion360;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Areas.Administrativo.Controllers.RecursosHumanos
{
    public class Evaluacion360Controller : BaseController
    {
        #region CONSTRUCTOR
        IEvaluacion360DAO iEvaluacion360DAO;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            iEvaluacion360DAO = new Evaluacion360FactoryServices().getEvaluacion360();
            base.OnActionExecuting(filterContext);
        }
        #endregion

        #region VISTAS
        public ActionResult Personal()
        {
            return View();
        }

        public ActionResult Conductas()
        {
            return View();
        }

        public ActionResult Criterios()
        {
            return View();
        }

        public ActionResult Periodos()
        {
            return View();
        }

        public ActionResult Cuestionarios()
        {
            return View();
        }

        public ActionResult Relaciones()
        {
            return View();
        }

        public ActionResult Evaluaciones()
        {
            return View();
        }

        public ActionResult Reporte360()
        {
            return View();
        }

        public ActionResult Avances()
        {
            return View();
        }
        #endregion

        #region CATALOGO DE PERSONAL
        public ActionResult GetCatalogoPersonal(CatPersonalDTO objPersonalDTO)
        {
            return Json(iEvaluacion360DAO.GetCatalogoPersonal(objPersonalDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CECatalogoPersonal(CatPersonalDTO objPersonalDTO)
        {
            return Json(iEvaluacion360DAO.CECatalogoPersonal(objPersonalDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarCatalogoPersonal(int idCatalogoPersonal)
        {
            return Json(iEvaluacion360DAO.EliminarCatalogoPersonal(idCatalogoPersonal), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDatosActualizarPersonal(int id)
        {
            return Json(iEvaluacion360DAO.GetDatosActualizarPersonal(id), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region CATALOGO DE CONDUCTAS
        public ActionResult GetConductas(CatConductasDTO objFiltroDTO)
        {
            return Json(iEvaluacion360DAO.GetConductas(objFiltroDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CEConducta(CatConductasDTO objConductaDTO)
        {
            return Json(iEvaluacion360DAO.CEConducta(objConductaDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarConducta(int idConducta)
        {
            return Json(iEvaluacion360DAO.EliminarConducta(idConducta), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDatosActualizarConducta(int id)
        {
            return Json(iEvaluacion360DAO.GetDatosActualizarConducta(id), JsonRequestBehavior.AllowGet);
        }

        #region CATALOGO DE GRUPOS
        public ActionResult GetGrupos()
        {
            return Json(iEvaluacion360DAO.GetGrupos(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CEGrupo(CatGrupoDTO objGrupoDTO)
        {
            return Json(iEvaluacion360DAO.CEGrupo(objGrupoDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarGrupo(int idGrupo)
        {
            return Json(iEvaluacion360DAO.EliminarGrupo(idGrupo), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDatosActualizarGrupo(int id)
        {
            return Json(iEvaluacion360DAO.GetDatosActualizarGrupo(id), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region CATALOGO DE COMPETENCIAS
        public ActionResult GetCompetencias(int idGrupo = 0)
        {
            return Json(iEvaluacion360DAO.GetCompetencias(idGrupo), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CECompetencia(CatCompetenciaDTO objCompetenciaDTO)
        {
            return Json(iEvaluacion360DAO.CECompetencia(objCompetenciaDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarCompetencia(int idCompetencia)
        {
            return Json(iEvaluacion360DAO.EliminarCompetencia(idCompetencia), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDatosActualizarCompetencia(int id)
        {
            return Json(iEvaluacion360DAO.GetDatosActualizarCompetencia(id), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region CATALOGO DE PLANTILLAS
        public ActionResult GetPlantillas()
        {
            return Json(iEvaluacion360DAO.GetPlantillas(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CEPlantilla(CatPlantillaDTO objPlantillaDTO)
        {
            return Json(iEvaluacion360DAO.CEPlantilla(objPlantillaDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarPlantilla(int idPlantilla)
        {
            return Json(iEvaluacion360DAO.EliminarPlantilla(idPlantilla), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDatosActualizarPlantilla(int id)
        {
            return Json(iEvaluacion360DAO.GetDatosActualizarPlantilla(id), JsonRequestBehavior.AllowGet);
        }

        #region CATALOGO DE CRITERIOS
        public ActionResult GetCriterios(int idPlantilla)
        {
            return Json(iEvaluacion360DAO.GetCriterios(idPlantilla), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CECriterio(CatCriterioDTO objCriterioDTO)
        {
            return Json(iEvaluacion360DAO.CECriterio(objCriterioDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarCriterio(int idCriterio)
        {
            return Json(iEvaluacion360DAO.EliminarCriterio(idCriterio), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDatosActualizarCriterio(int id)
        {
            return Json(iEvaluacion360DAO.GetDatosActualizarCriterio(id), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region CATALOGO DE PERIODOS
        public ActionResult GetPeriodos()
        {
            return Json(iEvaluacion360DAO.GetPeriodos(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CEPeriodo(CatPeriodosDTO objPeriodoDTO)
        {
            return Json(iEvaluacion360DAO.CEPeriodo(objPeriodoDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarPeriodo(int idPeriodo)
        {
            return Json(iEvaluacion360DAO.EliminarPeriodo(idPeriodo), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDatosActualizarPeriodo(int id)
        {
            return Json(iEvaluacion360DAO.GetDatosActualizarPeriodo(id), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region CUESTIONARIOS
        public ActionResult GetCuestionarios()
        {
            return Json(iEvaluacion360DAO.GetCuestionarios(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CECuestionario(CuestionarioDTO objCuestionarioDTO)
        {
            return Json(iEvaluacion360DAO.CECuestionario(objCuestionarioDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarCuestionario(int idCuestionario)
        {
            return Json(iEvaluacion360DAO.EliminarCuestionario(idCuestionario), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDatosActualizarCuestionario(int id)
        {
            return Json(iEvaluacion360DAO.GetDatosActualizarCuestionario(id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboCuestionarios()
        {
            return Json(iEvaluacion360DAO.FillCboCuestionarios(), JsonRequestBehavior.AllowGet);
        }

        #region CONDUCTAS REL CUESTIONARIOS
        public ActionResult GetConductasRelCuestionario(CuestionarioDetDTO objCuestionarioDetDTO)
        {
            return Json(iEvaluacion360DAO.GetConductasRelCuestionario(objCuestionarioDetDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearConductaRelCuestionario(CuestionarioDetDTO objCuestionarioDetDTO)
        {
            return Json(iEvaluacion360DAO.CrearConductaRelCuestionario(objCuestionarioDetDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarConductaRelCuestionario(int idConductaRelCuestonario)
        {
            return Json(iEvaluacion360DAO.EliminarConductaRelCuestionario(idConductaRelCuestonario), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region CONDUCTAS DISPONIBLES PARA LIGARLOS A UN CUESTIONARIO
        public ActionResult GetConductasDisponibles(CuestionarioDetDTO objCuestionarioDetDTO)
        {
            return Json(iEvaluacion360DAO.GetConductasDisponibles(objCuestionarioDetDTO), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region RELACIONES
        public ActionResult GetRelaciones(RelacionDTO objRelacionDTO)
        {
            return Json(iEvaluacion360DAO.GetRelaciones(objRelacionDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CERelacion(RelacionDTO objRelacionDTO)
        {
            return Json(iEvaluacion360DAO.CERelacion(objRelacionDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarRelacion(int id)
        {
            return Json(iEvaluacion360DAO.EliminarRelacion(id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboPersonalRelRelacionDisponibles(int idPeriodo)
        {
            return Json(iEvaluacion360DAO.FillCboPersonalRelRelacionDisponibles(idPeriodo), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboEvaluadores(List<int> lstEvaluadores_ID)
        {
            return Json(iEvaluacion360DAO.FillCboEvaluadores(lstEvaluadores_ID), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListadoEvaluadoresRelEvaluador(int idPersonalEvaluado, int tipoRelacion, int idPeriodo)
        {
            return Json(iEvaluacion360DAO.GetListadoEvaluadoresRelEvaluador(idPersonalEvaluado, tipoRelacion, idPeriodo), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CE_edicionEvaluado(RelacionDetDTO objRelacionDetDTO)
        {
            return Json(iEvaluacion360DAO.CE_edicionEvaluado(objRelacionDetDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarEvaluadorRelEvaluado(RelacionDetDTO objRelacionDetDTO)
        {
            return Json(iEvaluacion360DAO.EliminarEvaluadorRelEvaluado(objRelacionDetDTO), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region EVALUACIONES EVALUADOR
        public ActionResult GetEvaluaciones(EvaluacionEvaluadorDTO objDTO)
        {
            return Json(iEvaluacion360DAO.GetEvaluaciones(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEvaluacionEvaluadoRelEvaluador(EvaluacionEvaluadorDTO objDTO)
        {
            return Json(iEvaluacion360DAO.GetEvaluacionEvaluadoRelEvaluador(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarRespuestaConducta(EvaluacionEvaluadorDetDTO objDTO)
        {
            return Json(iEvaluacion360DAO.GuardarRespuestaConducta(objDTO), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region REPORTE 360
        public ActionResult GetEstatusEvaluados(Reporte360DTO objFiltro)
        {
            return Json(iEvaluacion360DAO.GetEstatusEvaluados(objFiltro), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEstatusCuestionariosEvaluadores(Reporte360DTO objDTO)
        {
            return Json(iEvaluacion360DAO.GetEstatusCuestionariosEvaluadores(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GenerarReporte360(Reporte360DTO objDTO)
        {
            return Json(iEvaluacion360DAO.GenerarReporte360(objDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCompetenciasRelEvaluado(Reporte360DTO objDTO)
        {
            return Json(iEvaluacion360DAO.GetCompetenciasRelEvaluado(objDTO), JsonRequestBehavior.AllowGet);
        }

        public bool CargarVariableSesion(Reporte360DTO objDTO)
        {
            Session["img"] = objDTO.grafica;
            return true;
        }
        #endregion

        #region AVANCES
        public ActionResult GetEstatusEvaluadores(EstatusEvaluadorDTO objFiltroDTO)
        {
            return Json(iEvaluacion360DAO.GetEstatusEvaluadores(objFiltroDTO), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EnviarCorreo(List<int> lstPersonalEvaluadorID, int idPeriodo)
        {
            return Json(iEvaluacion360DAO.EnviarCorreo(lstPersonalEvaluadorID, idPeriodo), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region METODOS GENERALES
        public ActionResult FillCboCC()
        {
            return Json(iEvaluacion360DAO.FillCboCC(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboDepartamentos()
        {
            return Json(iEvaluacion360DAO.FillCboDepartamentos(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboTipoUsuarios()
        {
            return Json(iEvaluacion360DAO.FillCboTipoUsuarios(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboGrupos()
        {
            return Json(iEvaluacion360DAO.FillCboGrupos(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboCompetencias(int idGrupo)
        {
            return Json(iEvaluacion360DAO.FillCboCompetencias(idGrupo), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboUsuarios()
        {
            return Json(iEvaluacion360DAO.FillCboUsuarios(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetInformacionUsuario(int idUsuario, int idEmpresa)
        {
            return Json(iEvaluacion360DAO.GetInformacionUsuario(idUsuario, idEmpresa), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboPuestos()
        {
            return Json(iEvaluacion360DAO.FillCboPuestos(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboPeriodos()
        {
            return Json(iEvaluacion360DAO.FillCboPeriodos(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboTipoRelacionEvaluado()
        {
            return Json(iEvaluacion360DAO.FillCboTipoRelacionEvaluado(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetNivelAcceso()
        {
            return Json(iEvaluacion360DAO.GetNivelAcceso(), JsonRequestBehavior.AllowGet);
        }

        private bool GetNivelAccesoUsuario()
        {
            Dictionary<string, object> objNivelAcceso = iEvaluacion360DAO.GetNivelAcceso();
            if ((int)objNivelAcceso["nivelAcceso"] == (int)NivelAccesoEnum.ADMINISTRADOR)
                return true;
            else
                return false;
        }
        #endregion
    }
}