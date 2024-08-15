using Core.DAO.RecursosHumanos.Evaluacion360;
using Core.DTO.RecursosHumanos.Evaluacion360;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.RecursosHumanos.Evaluacion360
{
    public class Evaluacion360Service : IEvaluacion360DAO
    {
        #region CONSTRUCTOR
        public IEvaluacion360DAO e_iEvaluacion360DAO { get; set; }
        private IEvaluacion360DAO Evaluacion360DAO
        {
            get { return e_iEvaluacion360DAO; }
            set { e_iEvaluacion360DAO = value; }
        }

        public Evaluacion360Service(IEvaluacion360DAO iEvaluacion360DAO)
        {
            this.e_iEvaluacion360DAO = iEvaluacion360DAO;
        }
        #endregion

        #region CATALOGO DE PERSONAL
        public Dictionary<string, object> GetCatalogoPersonal(CatPersonalDTO objPersonalDTO)
        {
            return e_iEvaluacion360DAO.GetCatalogoPersonal(objPersonalDTO);
        }

        public Dictionary<string, object> CECatalogoPersonal(CatPersonalDTO objPersonalDTO)
        {
            return e_iEvaluacion360DAO.CECatalogoPersonal(objPersonalDTO);
        }

        public Dictionary<string, object> EliminarCatalogoPersonal(int idCatalogoPersonal)
        {
            return e_iEvaluacion360DAO.EliminarCatalogoPersonal(idCatalogoPersonal);
        }

        public Dictionary<string, object> GetDatosActualizarPersonal(int id)
        {
            return e_iEvaluacion360DAO.GetDatosActualizarPersonal(id);
        }
        #endregion

        #region CATALOGO DE CONDUCTAS
        public Dictionary<string, object> GetConductas(CatConductasDTO objFiltroDTO)
        {
            return e_iEvaluacion360DAO.GetConductas(objFiltroDTO);
        }

        public Dictionary<string, object> CEConducta(CatConductasDTO objConductaDTO)
        {
            return e_iEvaluacion360DAO.CEConducta(objConductaDTO);
        }

        public Dictionary<string, object> EliminarConducta(int idConducta)
        {
            return e_iEvaluacion360DAO.EliminarConducta(idConducta);
        }

        public Dictionary<string, object> GetDatosActualizarConducta(int id)
        {
            return e_iEvaluacion360DAO.GetDatosActualizarConducta(id);
        }

        #region CATALOGO DE GRUPOS
        public Dictionary<string, object> GetGrupos()
        {
            return e_iEvaluacion360DAO.GetGrupos();
        }

        public Dictionary<string, object> CEGrupo(CatGrupoDTO objGrupoDTO)
        {
            return e_iEvaluacion360DAO.CEGrupo(objGrupoDTO);
        }

        public Dictionary<string, object> EliminarGrupo(int idGrupo)
        {
            return e_iEvaluacion360DAO.EliminarGrupo(idGrupo);
        }

        public Dictionary<string, object> GetDatosActualizarGrupo(int id)
        {
            return e_iEvaluacion360DAO.GetDatosActualizarGrupo(id);
        }
        #endregion

        #region CATALOGO DE COMPETENCIAS
        public Dictionary<string, object> GetCompetencias(int idGrupo = 0)
        {
            return e_iEvaluacion360DAO.GetCompetencias(idGrupo);
        }

        public Dictionary<string, object> CECompetencia(CatCompetenciaDTO objCompetenciaDTO)
        {
            return e_iEvaluacion360DAO.CECompetencia(objCompetenciaDTO);
        }

        public Dictionary<string, object> EliminarCompetencia(int idCompetencia)
        {
            return e_iEvaluacion360DAO.EliminarCompetencia(idCompetencia);
        }

        public Dictionary<string, object> GetDatosActualizarCompetencia(int id)
        {
            return e_iEvaluacion360DAO.GetDatosActualizarCompetencia(id);
        }
        #endregion

        #endregion

        #region CATALOGO DE PLANTILLAS
        public Dictionary<string, object> GetPlantillas()
        {
            return e_iEvaluacion360DAO.GetPlantillas();
        }

        public Dictionary<string, object> CEPlantilla(CatPlantillaDTO objPlantillaDTO)
        {
            return e_iEvaluacion360DAO.CEPlantilla(objPlantillaDTO);
        }

        public Dictionary<string, object> EliminarPlantilla(int idPlantilla)
        {
            return e_iEvaluacion360DAO.EliminarPlantilla(idPlantilla);
        }

        public Dictionary<string, object> GetDatosActualizarPlantilla(int id)
        {
            return e_iEvaluacion360DAO.GetDatosActualizarPlantilla(id);
        }

        #region CATALOGO DE CRITERIOS
        public Dictionary<string, object> GetCriterios(int idPlantilla)
        {
            return e_iEvaluacion360DAO.GetCriterios(idPlantilla);
        }

        public Dictionary<string, object> CECriterio(CatCriterioDTO objCriterioDTO)
        {
            return e_iEvaluacion360DAO.CECriterio(objCriterioDTO);
        }

        public Dictionary<string, object> EliminarCriterio(int idCriterio)
        {
            return e_iEvaluacion360DAO.EliminarCriterio(idCriterio);
        }

        public Dictionary<string, object> GetDatosActualizarCriterio(int id)
        {
            return e_iEvaluacion360DAO.GetDatosActualizarCriterio(id);
        }
        #endregion
        #endregion

        #region CATALOGO DE PERIODOS
        public Dictionary<string, object> GetPeriodos()
        {
            return e_iEvaluacion360DAO.GetPeriodos();
        }

        public Dictionary<string, object> CEPeriodo(CatPeriodosDTO objPeriodoDTO)
        {
            return e_iEvaluacion360DAO.CEPeriodo(objPeriodoDTO);
        }

        public Dictionary<string, object> EliminarPeriodo(int idPeriodo)
        {
            return e_iEvaluacion360DAO.EliminarPeriodo(idPeriodo);
        }

        public Dictionary<string, object> GetDatosActualizarPeriodo(int id)
        {
            return e_iEvaluacion360DAO.GetDatosActualizarPeriodo(id);
        }
        #endregion

        #region CUESTIONARIOS
        public Dictionary<string, object> GetCuestionarios()
        {
            return e_iEvaluacion360DAO.GetCuestionarios();
        }

        public Dictionary<string, object> CECuestionario(CuestionarioDTO objCuestionarioDTO)
        {
            return e_iEvaluacion360DAO.CECuestionario(objCuestionarioDTO);
        }

        public Dictionary<string, object> EliminarCuestionario(int idCuestionario)
        {
            return e_iEvaluacion360DAO.EliminarCuestionario(idCuestionario);
        }

        public Dictionary<string, object> GetDatosActualizarCuestionario(int id)
        {
            return e_iEvaluacion360DAO.GetDatosActualizarCuestionario(id);
        }

        public Dictionary<string, object> FillCboCuestionarios()
        {
            return e_iEvaluacion360DAO.FillCboCuestionarios();
        }

        #region CONDUCTAS REL CUESTIONARIOS
        public Dictionary<string, object> GetConductasRelCuestionario(CuestionarioDetDTO objCuestionarioDetDTO)
        {
            return e_iEvaluacion360DAO.GetConductasRelCuestionario(objCuestionarioDetDTO);
        }

        public Dictionary<string, object> CrearConductaRelCuestionario(CuestionarioDetDTO objCuestionarioDetDTO)
        {
            return e_iEvaluacion360DAO.CrearConductaRelCuestionario(objCuestionarioDetDTO);
        }

        public Dictionary<string, object> EliminarConductaRelCuestionario(int idConductaRelCuestonario)
        {
            return e_iEvaluacion360DAO.EliminarConductaRelCuestionario(idConductaRelCuestonario);
        }
        #endregion

        #region CONDUCTAS DISPONIBLES PARA LIGARLOS A UN CUESTIONARIO
        public Dictionary<string, object> GetConductasDisponibles(CuestionarioDetDTO objCuestionarioDetDTO)
        {
            return e_iEvaluacion360DAO.GetConductasDisponibles(objCuestionarioDetDTO);
        }
        #endregion
        #endregion

        #region RELACIONES
        public Dictionary<string, object> GetRelaciones(RelacionDTO objRelacionDTO)
        {
            return e_iEvaluacion360DAO.GetRelaciones(objRelacionDTO);
        }

        public Dictionary<string, object> CERelacion(RelacionDTO objRelacionDTO)
        {
            return e_iEvaluacion360DAO.CERelacion(objRelacionDTO);
        }

        public Dictionary<string, object> EliminarRelacion(int id)
        {
            return e_iEvaluacion360DAO.EliminarRelacion(id);
        }

        public Dictionary<string, object> FillCboPersonalRelRelacionDisponibles(int idPeriodo)
        {
            return e_iEvaluacion360DAO.FillCboPersonalRelRelacionDisponibles(idPeriodo);
        }

        public Dictionary<string, object> FillCboEvaluadores(List<int> lstEvaluadores_ID)
        {
            return e_iEvaluacion360DAO.FillCboEvaluadores(lstEvaluadores_ID);
        }

        public Dictionary<string, object> GetListadoEvaluadoresRelEvaluador(int idPersonalEvaluado, int tipoRelacion, int idPeriodo)
        {
            return e_iEvaluacion360DAO.GetListadoEvaluadoresRelEvaluador(idPersonalEvaluado, tipoRelacion, idPeriodo);
        }

        public Dictionary<string, object> CE_edicionEvaluado(RelacionDetDTO objRelacionDetDTO)
        {
            return e_iEvaluacion360DAO.CE_edicionEvaluado(objRelacionDetDTO);
        }

        public Dictionary<string, object> EliminarEvaluadorRelEvaluado(RelacionDetDTO objRelacionDetDTO)
        {
            return e_iEvaluacion360DAO.EliminarEvaluadorRelEvaluado(objRelacionDetDTO);
        }
        #endregion

        #region EVALUACIONES EVALUADOR
        public Dictionary<string, object> GetEvaluaciones(EvaluacionEvaluadorDTO objDTO)
        {
            return e_iEvaluacion360DAO.GetEvaluaciones(objDTO);
        }

        public Dictionary<string, object> GetEvaluacionEvaluadoRelEvaluador(EvaluacionEvaluadorDTO objDTO)
        {
            return e_iEvaluacion360DAO.GetEvaluacionEvaluadoRelEvaluador(objDTO);
        }

        public Dictionary<string, object> GuardarRespuestaConducta(EvaluacionEvaluadorDetDTO objDTO)
        {
            return e_iEvaluacion360DAO.GuardarRespuestaConducta(objDTO);
        }
        #endregion

        #region REPORTE 360
        public Dictionary<string, object> GetEstatusEvaluados(Reporte360DTO objFiltro)
        {
            return e_iEvaluacion360DAO.GetEstatusEvaluados(objFiltro);
        }

        public Dictionary<string, object> GetEstatusCuestionariosEvaluadores(Reporte360DTO objDTO)
        {
            return e_iEvaluacion360DAO.GetEstatusCuestionariosEvaluadores(objDTO);
        }

        public Dictionary<string, object> GenerarReporte360(Reporte360DTO objDTO)
        {
            return e_iEvaluacion360DAO.GenerarReporte360(objDTO);
        }

        public Dictionary<string, object> GetCompetenciasRelEvaluado(Reporte360DTO objDTO)
        {
            return e_iEvaluacion360DAO.GetCompetenciasRelEvaluado(objDTO);
        }
        #endregion

        #region AVANCES
        public Dictionary<string, object> GetEstatusEvaluadores(EstatusEvaluadorDTO objFiltroDTO)
        {
            return e_iEvaluacion360DAO.GetEstatusEvaluadores(objFiltroDTO);
        }

        public Dictionary<string, object> EnviarCorreo(List<int> lstPersonalEvaluadorID, int idPeriodo)
        {
            return e_iEvaluacion360DAO.EnviarCorreo(lstPersonalEvaluadorID, idPeriodo);
        }
        #endregion

        #region METODOS GENERALES
        public Dictionary<string, object> FillCboCC()
        {
            return e_iEvaluacion360DAO.FillCboCC();
        }

        public Dictionary<string, object> FillCboDepartamentos()
        {
            return e_iEvaluacion360DAO.FillCboDepartamentos();
        }

        public Dictionary<string, object> FillCboTipoUsuarios()
        {
            return e_iEvaluacion360DAO.FillCboTipoUsuarios();
        }

        public Dictionary<string, object> FillCboGrupos()
        {
            return e_iEvaluacion360DAO.FillCboGrupos();
        }

        public Dictionary<string, object> FillCboCompetencias(int idGrupo)
        {
            return e_iEvaluacion360DAO.FillCboCompetencias(idGrupo);
        }

        public Dictionary<string, object> FillCboUsuarios()
        {
            return e_iEvaluacion360DAO.FillCboUsuarios();
        }

        public Dictionary<string, object> GetInformacionUsuario(int idUsuario, int idEmpresa)
        {
            return e_iEvaluacion360DAO.GetInformacionUsuario(idUsuario, idEmpresa);
        }

        public Dictionary<string, object> FillCboPuestos()
        {
            return e_iEvaluacion360DAO.FillCboPuestos();
        }

        public Dictionary<string, object> FillCboPeriodos()
        {
            return e_iEvaluacion360DAO.FillCboPeriodos();
        }

        public Dictionary<string, object> FillCboTipoRelacionEvaluado()
        {
            return e_iEvaluacion360DAO.FillCboTipoRelacionEvaluado();
        }

        public Dictionary<string, object> GetNivelAcceso()
        {
            return e_iEvaluacion360DAO.GetNivelAcceso();
        }
        #endregion
    }
}