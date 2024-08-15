using Core.DTO.ControlObra;
using Core.DTO.ControlObra.EvaluacionSubcontratista;
using Core.DTO.Principal.Generales;
using Core.DTO.Subcontratistas.Evaluacion;
using Core.Entity.ControlObra;
using Core.Entity.Encuestas;
using Core.Entity.SubContratistas;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.DAO.Subcontratistas
{
    public interface IEvaluadorDAO
    {
        #region SUBCONTRATISTA

        List<ComboDTO> getProyecto(int idUsuario);
        List<ComboDTO> getSubContratistas(string AreaCuenta);
        SubContratistasDTO getTblSubContratista(SubContratistasDTO parametros);
        SubContratistasDTO addEditSubContratista(List<HttpPostedFileBase> Archivo, List<SubContratistasDTO> parametros, tblPUsuarioDTO ObjUsuario, bool subcontratista);
        List<SubContratistasDTO> CargarArchivosXSubcontratista(SubContratistasDTO parametros);
        List<SubContratistasDTO> CargarArchivosSubcontratista(SubContratistasDTO parametros, tblPUsuarioDTO ObjUsuario);
        List<SubContratistasDTO> CargarArchivosXSubcontratistaEvaluacion(SubContratistasDTO parametros);
        List<SubContratistasDTO> ObtenerTblAutorizacion(SubContratistasDTO parametros);
        Dictionary<string, object> obtenerPlantillas();
        DivicionesMenuDTO addEditPlantilla(DivicionesMenuDTO parametros);
        List<ComboDTO> cboObtenerContratos();
        List<ComboDTO> cboObtenerContratosInclu(int idPlantilla);
        Dictionary<string, object> eliminarPlantilla(int id);
        List<DivicionesMenuDTO> obtenerDiviciones(int idPlantilla, int idAsignacion);
        List<DivicionesMenuDTO> obtenerDivicionesEvaluador();
        List<DivicionesMenuDTO> obtenerDivicionesEvaluadorArchivos(int idPlantilla, int idAsignacion);

        DivicionesMenuDTO addEditDiviciones(DivicionesMenuDTO parametros);
        DivicionesMenuDTO eliminarDiviciones(int id);
        List<RequerimientosDTO> obtenerRequerimientos(int idDiv, int? idAsignacion);
        byte[] DescargarArchivos(long idDet);
        string getFileName(long idDet);

        SubContratistasDTO obtenerEvaluacionxReq(SubContratistasDTO parametros);
        SubContratistasDTO GuardarEvaluacion(SubContratistasDTO parametros);
        SubContratistasDTO obtenerPromegioEvaluacion(SubContratistasDTO parametros);

        Dictionary<string, object> ObtenerGraficaDeBarras(SubContratistasDTO parametros);
        List<SubContratistasDTO> obtenerPromediosxSubcontratista(SubContratistasDTO parametros);
        bool ObtenerEvaluacionPendiente(tblPUsuarioDTO ObjUsuario);
        List<ContratoSubContratistaDTO> obtenerContratistasConContrato(string AreaCuenta, int subcontratista, int Estatus, tblPUsuarioDTO ObjUsuario);
        SubContratistasDTO GuardarEvaluacionSubContratista(SubContratistasDTO parametros);
        SubContratistasDTO GuardarAsignacion(SubContratistasDTO parametros);
        List<SubContratistasDTO> getUsuariosAutorizantes(string term);
        SubContratistasDTO AutorizarEvaluacion(SubContratistasDTO parametros, int idUsuario);
        SubContratistasDTO AutorizarAsignacion(SubContratistasDTO parametros, int idUsuario);

        List<ElementosDTO> obtenerTodosLosElementosConSuRequerimiento();
        ElementosDTO guardarRelacion(List<ElementosDTO> lstRelacion);
        List<ContratoSubContratistaDTO> tblObtenerDashBoardSubContratista(tblPUsuarioDTO ObjUsuario);
        MemoryStream realizarExcel(int idAsignacion);
        ContratoSubContratistaDTO EvaluarDetalle(int id, List<SubContratistasDTO> parametros, int userEnvia);
        List<SubContratistasDTO> obtenerPromedioxElemento(List<SubContratistasDTO> parametros);
        #endregion

        List<evaluadorXccDTO> getEvaluadoresxCC();
        evaluadorXccDTO AgregarEditarEvaluadores(evaluadorXccDTO parametros);
        evaluadorXccDTO ActivarDesactivarEvaluadores(evaluadorXccDTO parametros);
        List<ComboDTO> getSubContratistasRestantes();
        List<ComboDTO> getProyectoRestantes(bool Agregar);
        List<tblEN_Estrellas> getEstrellas();
        List<ComboDTO> obtenerTodolosElementos();
        Dictionary<string, object> obtenerElementosEvaluar(int idUsuario, int idPlantilla);
        Dictionary<string, object> ObtenerGraficaDeEvaluacionPorCentroDeCosto(SubContratistasDTO parametros);
        Dictionary<string, object> ObtenerGraficaDeEvaluacionPorDivisionElemento(SubContratistasDTO parametros);
        Dictionary<string, object> ObtenerGraficaDeEvaluacionPorTendenciaMedianteEvaluaciones(SubContratistasDTO parametros);


        //#region Gestión
        ///// <summary>
        ///// Obtiene todos los proyectos para mostrarlos en el comboBox de proyectos en el filtro de firmas de evaluación de subcontratistas
        ///// </summary>
        ///// <returns></returns>
        //Dictionary<string, object> ObtenerProyectosParaFiltro();

        ///// <summary>
        ///// Obtiene todos los subcontratistas que se encuentren en el proyecto enviado para mostrarlos en el
        ///// comboBox de subcontratistas en el filtro de firmas de evaluación de subcontratistass
        ///// </summary>
        ///// <returns></returns>
        //Dictionary<string, object> ObtenerSubcontratistasParaFiltro(string proyecto);

        ///// <summary>
        ///// Obtiene todas las evaluaciones segun el proyecto y subcontratista seleccionados para mostrar en la tabla principal
        ///// de firmas de evaluaciones de subcontratistas
        ///// </summary>
        ///// <param name="proyecto"></param>
        ///// <param name="subcontratistaId"></param>
        ///// <returns></returns>
        //Dictionary<string, object> ObtenerEvaluacionesSubcontratistas(string proyecto, int? subcontratistaId);

        ///// <summary>
        ///// Obtiene los usuario que firman las evaluaciones de forma escalonada segun la evaluacion.
        ///// </summary>
        ///// <param name="evaluacionId"></param>
        ///// <returns></returns>
        //Dictionary<string, object> ObtenerEstatusFirmantes(int evaluacionId);

        ///// <summary>
        ///// Obtiene información del firmante pendiente a firmar segun la evaluacion.
        ///// </summary>
        ///// <param name="evaluacionId"></param>
        ///// <returns></returns>
        //Dictionary<string, object> ObtenerFirmante(int evaluacionId);

        ///// <summary>
        ///// Se recibe la firma en formato base64, se guarda en formato jpeg y se guarda la informacion en base de datos
        ///// </summary>
        ///// <param name="firma"></param>
        ///// <returns></returns>
        //Dictionary<string, object> GuardarFirma(InformacionFirmaDigitalDTO firma);
        //#endregion

        /// <summary>
        /// Se recibe la firma en formato base64, se guarda en formato jpeg y se guarda la informacion en base de datos
        /// </summary>
        /// <param name="firma"></param>
        /// <returns></returns>
        Dictionary<string, object> cambiarDeColor(int idPlantilla, int idAsignacion);

        Dictionary<string, object> obtenerPromedioGeneral(int id);

        List<DivReqDTO> obtenerLst(int idPlantilla, int idAsignacion);
        List<tblX_FirmaEvaluacion> obtenerFirmas(int idAsignacion);
        List<excelDTO> obtenerListado2(int idAsignacion);
        tblCO_ADP_EvalSubConAsignacion traermeDatosPrincipales(int idAsignacion);
        List<tblCO_ADP_EvalSubConAsignacion> obtenerTodasLasASinaciones(int idAsignacion);
    }
}
