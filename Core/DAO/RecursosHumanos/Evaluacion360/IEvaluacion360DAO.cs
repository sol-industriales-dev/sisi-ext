using Core.DTO.RecursosHumanos.Evaluacion360;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.RecursosHumanos.Evaluacion360
{
    public interface IEvaluacion360DAO
    {
        #region CATALOGO DE PERSONAL
        Dictionary<string, object> GetCatalogoPersonal(CatPersonalDTO objPersonalDTO);

        Dictionary<string, object> CECatalogoPersonal(CatPersonalDTO objPersonalDTO);

        Dictionary<string, object> EliminarCatalogoPersonal(int idCatalogoPersonal);

        Dictionary<string, object> GetDatosActualizarPersonal(int id);
        #endregion

        #region CATALOGO DE CONDUCTAS
        Dictionary<string, object> GetConductas(CatConductasDTO objFiltroDTO);

        Dictionary<string, object> CEConducta(CatConductasDTO objConductaDTO);

        Dictionary<string, object> EliminarConducta(int idConducta);

        Dictionary<string, object> GetDatosActualizarConducta(int id);

        #region CATALOGO DE GRUPOS
        Dictionary<string, object> GetGrupos();

        Dictionary<string, object> CEGrupo(CatGrupoDTO objGrupoDTO);

        Dictionary<string, object> EliminarGrupo(int idGrupo);

        Dictionary<string, object> GetDatosActualizarGrupo(int id);
        #endregion

        #region CATALOGO DE COMPETENCIAS
        Dictionary<string, object> GetCompetencias(int idGrupo = 0);

        Dictionary<string, object> CECompetencia(CatCompetenciaDTO objCompetenciaDTO);

        Dictionary<string, object> EliminarCompetencia(int idCompetencia);

        Dictionary<string, object> GetDatosActualizarCompetencia(int id);
        #endregion

        #endregion

        #region CATALOGO DE PLANTILLAS
        Dictionary<string, object> GetPlantillas();

        Dictionary<string, object> CEPlantilla(CatPlantillaDTO objPlantillaDTO);

        Dictionary<string, object> EliminarPlantilla(int idPlantilla);

        Dictionary<string, object> GetDatosActualizarPlantilla(int id);

        #region CATALOGO DE CRITERIOS
        Dictionary<string, object> GetCriterios(int idPlantilla);

        Dictionary<string, object> CECriterio(CatCriterioDTO objCriterioDTO);

        Dictionary<string, object> EliminarCriterio(int idCriterio);

        Dictionary<string, object> GetDatosActualizarCriterio(int id);
        #endregion
        #endregion

        #region CATALOGO DE PERIODOS
        Dictionary<string, object> GetPeriodos();

        Dictionary<string, object> CEPeriodo(CatPeriodosDTO objPeriodoDTO);

        Dictionary<string, object> EliminarPeriodo(int idPeriodo);

        Dictionary<string, object> GetDatosActualizarPeriodo(int id);
        #endregion

        #region CUESTIONARIOS
        Dictionary<string, object> GetCuestionarios();

        Dictionary<string, object> CECuestionario(CuestionarioDTO objCuestionarioDTO);

        Dictionary<string, object> EliminarCuestionario(int idCuestionario);

        Dictionary<string, object> GetDatosActualizarCuestionario(int id);

        Dictionary<string, object> FillCboCuestionarios();

        #region CONDUCTAS REL CUESTIONARIOS
        Dictionary<string, object> GetConductasRelCuestionario(CuestionarioDetDTO objCuestionarioDetDTO);

        Dictionary<string, object> CrearConductaRelCuestionario(CuestionarioDetDTO objCuestionarioDetDTO);

        Dictionary<string, object> EliminarConductaRelCuestionario(int idConductaRelCuestonario);
        #endregion

        #region CONDUCTAS DISPONIBLES PARA LIGARLOS A UN CUESTIONARIO
        Dictionary<string, object> GetConductasDisponibles(CuestionarioDetDTO objCuestionarioDetDTO);
        #endregion
        #endregion

        #region RELACIONES
        Dictionary<string, object> GetRelaciones(RelacionDTO objRelacionDTO);

        Dictionary<string, object> CERelacion(RelacionDTO objRelacionDTO);

        Dictionary<string, object> EliminarRelacion(int id);

        Dictionary<string, object> FillCboPersonalRelRelacionDisponibles(int idPeriodo);

        Dictionary<string, object> FillCboEvaluadores(List<int> lstEvaluadores_ID);

        Dictionary<string, object> GetListadoEvaluadoresRelEvaluador(int idPersonalEvaluado, int tipoRelacion, int idPeriodo);

        Dictionary<string, object> CE_edicionEvaluado(RelacionDetDTO objRelacionDetDTO);

        Dictionary<string, object> EliminarEvaluadorRelEvaluado(RelacionDetDTO objRelacionDetDTO);
        #endregion

        #region EVALUACIONES EVALUADOR
        Dictionary<string, object> GetEvaluaciones(EvaluacionEvaluadorDTO objDTO);

        Dictionary<string, object> GetEvaluacionEvaluadoRelEvaluador(EvaluacionEvaluadorDTO objDTO);

        Dictionary<string, object> GuardarRespuestaConducta(EvaluacionEvaluadorDetDTO objDTO);
        #endregion

        #region REPORTE 360
        Dictionary<string, object> GetEstatusEvaluados(Reporte360DTO objFiltro);

        Dictionary<string, object> GetEstatusCuestionariosEvaluadores(Reporte360DTO objDTO);

        Dictionary<string, object> GenerarReporte360(Reporte360DTO objDTO);

        Dictionary<string, object> GetCompetenciasRelEvaluado(Reporte360DTO objDTO);
        #endregion

        #region AVANCES
        Dictionary<string, object> GetEstatusEvaluadores(EstatusEvaluadorDTO objFiltroDTO);

        Dictionary<string, object> EnviarCorreo(List<int> lstPersonalEvaluadorID, int idPeriodo);
        #endregion

        #region METODOS GENERALES
        Dictionary<string, object> FillCboCC();

        Dictionary<string, object> FillCboDepartamentos();

        Dictionary<string, object> FillCboTipoUsuarios();

        Dictionary<string, object> FillCboGrupos();

        Dictionary<string, object> FillCboCompetencias(int idGrupo);

        Dictionary<string, object> FillCboUsuarios();

        Dictionary<string, object> GetInformacionUsuario(int idUsuario, int idEmpresa);

        Dictionary<string, object> FillCboPuestos();

        Dictionary<string, object> FillCboPeriodos();

        Dictionary<string, object> FillCboTipoRelacionEvaluado();

        Dictionary<string, object> GetNivelAcceso();
        #endregion
    }
}