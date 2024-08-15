using Core.DTO;
using Core.DTO.ControlObra;
using Core.DTO.ControlObra.EvaluacionSubcontratista;
using Core.DTO.Principal.Generales;
using Core.DTO.Subcontratistas.Evaluacion;
using Core.Entity.ControlObra;
using Core.Entity.Encuestas;
using Core.Entity.SubContratistas;
using Core.Enum.ControlObra;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.DAO.Subcontratistas
{
    public interface IEvaluacionSubcontratistaDAO
    {
        #region SUB
        #region Captura
        #endregion
        #region Gestión
        /// <summary>
        /// Obtiene todos los proyectos para mostrarlos en el comboBox de proyectos en el filtro de firmas de evaluación de subcontratistas
        /// </summary>
        /// <returns></returns>
        Dictionary<string, object> ObtenerProyectosParaFiltro();

        /// <summary>
        /// Obtiene todos los subcontratistas que se encuentren en el proyecto enviado para mostrarlos en el
        /// comboBox de subcontratistas en el filtro de firmas de evaluación de subcontratistass
        /// </summary>
        /// <returns></returns>
        Dictionary<string, object> ObtenerSubcontratistasParaFiltro(string proyecto);

        /// <summary>
        /// Obtiene todas las evaluaciones segun el proyecto y subcontratista seleccionados para mostrar en la tabla principal
        /// de firmas de evaluaciones de subcontratistas
        /// </summary>
        /// <param name="proyecto"></param>
        /// <param name="subcontratistaId"></param>
        /// <returns></returns>
        Dictionary<string, object> ObtenerEvaluacionesSubcontratistas(string proyecto, int? subcontratistaId);

        /// <summary>
        /// Obtiene los usuario que firman las evaluaciones de forma escalonada segun la evaluacion.
        /// </summary>
        /// <param name="evaluacionId"></param>
        /// <returns></returns>
        Dictionary<string, object> ObtenerEstatusFirmantes(int evaluacionId);

        /// <summary>
        /// Obtiene información del firmante pendiente a firmar segun la evaluacion.
        /// </summary>
        /// <param name="evaluacionId"></param>
        /// <returns></returns>
        Dictionary<string, object> ObtenerFirmante(int evaluacionId);

        /// <summary>
        /// Se recibe la firma en formato base64, se guarda en formato jpeg y se guarda la informacion en base de datos
        /// </summary>
        /// <param name="firma"></param>
        /// <returns></returns>
        Dictionary<string, object> GuardarFirma(InformacionFirmaDigitalDTO firma);
        #endregion
        #region Dashboard
        #endregion
        List<excelDTO> obtenerListado(int idAsignacion);
        tblCO_ADP_EvalSubConAsignacion traermeDatosPrincipales(int idAsignacion);
        List<tblCO_ADP_EvalSubConAsignacion> obtenerTodasLasASinaciones(int idAsignacion);
        Dictionary<string, object> cambiarDeColor(int idPlantilla, int idAsignacion);
        #region SUBCONTRATISTA

        List<ComboDTO> getProyecto(int idUsuario);
        List<ComboDTO> getSubContratistas(string AreaCuenta);
        Dictionary<string, object> FillCboProyectosFacultamientos();
        SubContratistasDTO getTblSubContratista(SubContratistasDTO parametros);
        SubContratistasDTO addEditSubContratista(List<HttpPostedFileBase> Archivo, List<SubContratistasDTO> parametros);
        List<SubContratistasDTO> CargarArchivosXSubcontratista(SubContratistasDTO parametros);

        List<SubContratistasDTO> CargarArchivosSubcontratista(SubContratistasDTO parametros);
        List<SubContratistasDTO> CargarArchivosXSubcontratistaEvaluacion(SubContratistasDTO parametros);
        List<SubContratistasDTO> ObtenerTblAutorizacion(SubContratistasDTO parametros);
        Dictionary<string, object> GetPlantillasCreadas(int plantilla_id, int contrato_id);
        Dictionary<string, object> addEditPlantilla(DivicionesMenuDTO objDTO);
        List<ComboDTO> cboObtenerContratos();
        List<ComboDTO> cboObtenerContratosInclu(int idPlantilla);
        Dictionary<string, object> eliminarPlantilla(int id);
        List<DivicionesMenuDTO> obtenerDiviciones(int idPlantilla);
        Dictionary<string, object> obtenerDivicionesEvaluador();
        List<DivicionesMenuDTO> obtenerDivicionesEvaluadorArchivos(int idPlantilla, int idAsignacion);
        List<DivReqDTO> obtenerLst(int idPlantilla, int idAsignacion);
        List<tblCOES_Firma> obtenerFirmas(int evaluacion_id);
        DivicionesMenuDTO addEditDiviciones(DivicionesMenuDTO parametros);
        DivicionesMenuDTO eliminarDiviciones(int id);
        List<RequerimientosDTO> obtenerRequerimientos(int idDiv);
        byte[] DescargarArchivos(long idDet, int idEvaluacion);
        string getFileName(long idDet, int idEvaluacion);

        SubContratistasDTO obtenerEvaluacionxReq(SubContratistasDTO parametros);
        SubContratistasDTO GuardarEvaluacion(SubContratistasDTO parametros);
        SubContratistasDTO obtenerPromegioEvaluacion(SubContratistasDTO objDTO);

        Dictionary<string, object> ObtenerGraficaDeBarras(SubContratistasDTO parametros);
        List<SubContratistasDTO> obtenerPromediosxSubcontratista(SubContratistasDTO parametros);
        bool ObtenerEvaluacionPendiente(string idUsuario);
        List<ContratoSubContratistaDTO> obtenerContratistasConContrato(string AreaCuenta, int subcontratista, int Estatus, int tipoUsuario, string cc);
        SubContratistasDTO GuardarAsignacion(SubContratistasDTO parametros);
        List<SubContratistasDTO> getUsuariosAutorizantes(string term);
        //SubContratistasDTO AutorizarEvaluacion(SubContratistasDTO parametros, int idUsuario);
        Dictionary<string, object> AutorizarAsignacion(SubContratistasDTO objDTO, int idUsuario);

        List<ElementosDTO> obtenerTodosLosElementosConSuRequerimiento();
        ElementosDTO guardarRelacion(List<ElementosDTO> lstRelacion);
        List<ContratoSubContratistaDTO> tblObtenerDashBoardSubContratista(string RFC);
        MemoryStream realizarExcel(int idAsignacion);
        ContratoSubContratistaDTO EvaluarDetalle(int id, List<SubContratistasDTO> parametros, int userEnvia);
        List<SubContratistasDTO> obtenerPromedioxElemento(List<SubContratistasDTO> parametros);
        #endregion
        #region  ELEMENTOS
        List<evaluadorXccDTO> getEvaluadoresxCC(string cc, string elemento, int evaluadores);
        evaluadorXccDTO AgregarEditarEvaluadores(evaluadorXccDTO parametros);
        evaluadorXccDTO ActivarDesactivarEvaluadores(evaluadorXccDTO parametros);
        List<ComboDTO> getSubContratistasRestantes();
        List<ComboDTO> getProyectoRestantes(bool Agregar);
        List<tblEN_Estrellas> getEstrellas();
        List<ComboDTO> obtenerTodolosElementos();
        Dictionary<string, object> obtenerElementosEvaluar(int idUsuario, int idPlantilla, int idAsignacion);
        Dictionary<string, object> ObtenerGraficaDeEvaluacionPorCentroDeCosto(SubContratistasDTO parametros);
        Dictionary<string, object> ObtenerGraficaDeEvaluacionPorDivisionElemento(SubContratistasDTO parametros);
        Dictionary<string, object> ObtenerGraficaDeEvaluacionPorTendenciaMedianteEvaluaciones(SubContratistasDTO parametros);

        #endregion
        #region CATALOGOS NUEVOS
        Dictionary<string, object> fillComboPrestadoresDeServicio();
        Dictionary<string, object> AccionesUsuariosExpediente(tblPUsuarioDTO objUsuario, int Accion);
        #endregion
        #region combos
        Dictionary<string, object> cboProyecto();
        Dictionary<string, object> cboProyecto3(int idSubcontratista);
        Dictionary<string, object> cboContratosBuscar3(int idSubcontratista);
        Dictionary<string, object> cboEvaluador();
        Dictionary<string, object> cboSubcontratistas();
        Dictionary<string, object> cboElementos();
        Dictionary<string, object> FillComboPlantillas();
        Dictionary<string, object> cboContratosBuscar();
        #endregion
        #region CATALOGO DE FACULTAMIENTOS
        Dictionary<string, object> AccionesFacultamientos(facultamientosCODTO objUsuario, int Accion);
        #endregion
        Dictionary<string, object> cboUsuarios();
        Dictionary<string, object> obtenerPromedioGeneral(int id);
        Respuesta PermisoVista(int vistaID);
        Dictionary<string, object> VerificarTipoUsuario();
        Dictionary<string, object> FillCboEstados();
        Dictionary<string, object> FillCboMunicipios(int idEstado);
        Dictionary<string, object> GetListadoCCRelUsuarioFacultamientos(int facultamiento_id);
        #endregion

        #region ADMINISTRACIÓN DE EVALUACIONES
        Dictionary<string, object> VerificarElementoTerminado(EvaluacionDTO objDTO);
        #endregion

        #region CATÁLOGO DE EVALUADORES
        Dictionary<string, object> FillCboFiltroEvaluadores();

        Dictionary<string, object> FillCboCEEvaluadores();
        #endregion

        #region CATALOGOS DE FACULTAMIENTOS
        Dictionary<string, object> GetListadoUsuarioRelCC(int id);

        Dictionary<string, object> GetCCActualizarFacultamiento(int id);

        Dictionary<string, object> FillTipoUsuarioFacultamientos();
        #endregion

        #region CATALOGO FIRMASTES ENCARGADOS DE CIERTOS CONTRATOS
        Dictionary<string, object> GetUsuariosRelSubcontratistas(UsuarioRelSubcontratistaDTO objDTO);

        Dictionary<string, object> CEUsuarioRelSubcontratista(UsuarioRelSubcontratistaDTO objDTO);

        Dictionary<string, object> EliminarUsuarioRelSubcontratista(UsuarioRelSubcontratistaDTO objDTO);

        Dictionary<string, object> MandarUsuarioComoHistorial(UsuarioRelSubcontratistaDTO objDTO);

        Dictionary<string, object> FillComboSubcontratistas();

        Dictionary<string, object> GetDatosActualizarUsuarioRelSubcontratista(UsuarioRelSubcontratistaDTO objDTO);

        Dictionary<string, object> FillCboContratosRelSubcontratistas(int idSubcontratista);
        #endregion

        #region CALENDARIO
        Dictionary<string, object> FillCboEvaluacionesActivas(SubContratistasDTO objDTO);

        Dictionary<string, object> GetFechasEvaluaciones(UsuarioRelSubcontratistaDTO objDTO);

        Dictionary<string, object> ActualizarFechasActualizacion(CalendarioDTO objDTO);

        Dictionary<string, object> GetFechasActualizar(CalendarioDTO objDTO);

        Dictionary<string, object> GetTipoUsuario();
        #endregion

        #region CATÁLOGO DE PLANTILLAS
        Dictionary<string, object> FillComboContratos();
        #endregion

        #region REPORTE CRYSTAL REPORT
        List<tblCO_ADP_EvalSubConAsignacion> GetEvaluaciones(int idAsignacion);
        #endregion

        object GetUsuariosAutocomplete(string term, bool porClave);
        Dictionary<string, object> FillComboElementos();
        Dictionary<string, object> FillComboRequerimientos(int elemento_id);
        Dictionary<string, object> GuardarNuevoElemento(tblCOES_Elemento elemento);
        Dictionary<string, object> GuardarNuevoRequerimiento(tblCOES_Requerimiento requerimiento);
        Dictionary<string, object> GetRequerimientosElemento(int elemento_id);
        Dictionary<string, object> GuardarNuevaPlantilla(tblCOES_Plantilla plantilla, List<int> contratos, List<RequerimientoDTO> requerimientos);
        Dictionary<string, object> EditarPlantilla(tblCOES_Plantilla plantilla, List<int> contratos, List<RequerimientoDTO> requerimientos);
        Dictionary<string, object> EliminarRequerimientoElemento(int requerimiento_id);
        Dictionary<string, object> GetPlantilla(int plantilla_id);
        Dictionary<string, object> CopiarPlantillaBase(int plantilla_id);
        Dictionary<string, object> EliminarPlantilla(int plantilla_id);
        Dictionary<string, object> FillComboProyectos();

        Dictionary<string, object> GetFacultamientoUsuario();

        #region Evaluadores
        Dictionary<string, object> GetEvaluadores(string cc, int elemento);
        Dictionary<string, object> GuardarNuevoEvaluador(tblCOES_Evaluador evaluador, List<tblCOES_Evaluador_Proyecto> proyectos, List<tblCOES_Evaluador_Elemento> elementos);
        Dictionary<string, object> EditarEvaluador(tblCOES_Evaluador evaluador, List<tblCOES_Evaluador_Proyecto> proyectos, List<tblCOES_Evaluador_Elemento> elementos);
        Dictionary<string, object> EliminarEvaluador(int evaluador_id);
        Dictionary<string, object> GetEvaluador(int evaluador_id);
        #endregion

        #region Facultamientos
        Dictionary<string, object> GetFacultamientos(string cc, TipoFacultamientoEnum tipo);
        Dictionary<string, object> GuardarNuevoFacultamiento(tblCOES_Facultamiento facultamiento, List<tblCOES_Facultamiento_CentroCosto> proyectos);
        Dictionary<string, object> EditarFacultamiento(tblCOES_Facultamiento facultamiento, List<tblCOES_Facultamiento_CentroCosto> proyectos);
        Dictionary<string, object> EliminarFacultamiento(int facultamiento_id);
        Dictionary<string, object> GetFacultamiento(int facultamiento_id);
        #endregion

        #region Firmas Subcontratistas
        Dictionary<string, object> GetFirmaSubcontratistas(int subcontratista_id);
        Dictionary<string, object> GuardarNuevaFirmaSubcontratista(tblCOES_FirmaSubcontratista firma, List<tblCOES_FirmaSubcontratistatblX_Contrato> contratos);
        Dictionary<string, object> EditarFirmaSubcontratista(tblCOES_FirmaSubcontratista firma, List<tblCOES_FirmaSubcontratistatblX_Contrato> contratos);
        Dictionary<string, object> EliminarFirmaSubcontratista(int firma_id);
        Dictionary<string, object> GetFirmaSubcontratista(int firma_id);
        Dictionary<string, object> EnviarCorreoNotificacionFirma(int firma_id);
        #endregion

        #region Firmas Gerentes
        Dictionary<string, object> GetFirmaGerentes(string cc);
        Dictionary<string, object> GuardarNuevaFirmaGerente(tblCOES_FirmaGerente firma);
        Dictionary<string, object> EditarFirmaGerente(tblCOES_FirmaGerente firma);
        Dictionary<string, object> EliminarFirmaGerente(int firma_id);
        Dictionary<string, object> GetFirmaGerente(int firma_id);
        #endregion

        #region Especialidades
        Dictionary<string, object> FillComboEspecialidades();
        Dictionary<string, object> GetSubcontratistasEspecialidad(string cc);
        Dictionary<string, object> GuardarEspecialidadesSubcontratista(int subcontratista_id, List<tblCOES_EspecialidadtblX_SubContratista> especialidades);
        #endregion

        #region Administración Evaluaciones
        Dictionary<string, object> CargarEvaluacionesSubcontratistas(string cc, int subcontratista_id, EstatusEvaluacionEnum estatus);
        Dictionary<string, object> GuardarAsignacionEvaluacion(tblCOES_Asignacion asignacion, List<tblCOES_Asignacion_Evaluacion> evaluaciones);
        Dictionary<string, object> GetContratoInformacion(int contrato_id);
        Dictionary<string, object> GetElementosEvaluacion(int contrato_id, int evaluacion_id);
        Dictionary<string, object> GuardarRetroalimentacionEvaluador(tblCOES_Evidencia evidencia);
        Dictionary<string, object> GuardarEvaluacionSubcontratista();
        tblCOES_Evidencia GetArchivoEvidencia(int evidencia_id);
        Dictionary<string, object> EnviarGestionFirmas(int evaluacion_id, int contrato_id, int subcontratista_id);
        Dictionary<string, object> GetSeguimientoFirmas(int evaluacion_id, int contrato_id);
        Dictionary<string, object> AutorizarEvaluacion(int firma_id, HttpPostedFileBase archivoFirma);
        Dictionary<string, object> RechazarEvaluacion(int firma_id);
        Dictionary<string, object> GetAsignacionContrato(int asignacion_id);
        Dictionary<string, object> GuardarCambioEvaluacion(tblCOES_CambioEvaluacion cambio);
        Dictionary<string, object> GetCambioEvaluacion(int cambioEvaluacion_id);
        Dictionary<string, object> GuardarAutorizacionCambioEvaluacion(tblCOES_CambioEvaluacion cambio);
        Dictionary<string, object> CargarGraficasSubcontratista(string cc, int subcontratista_id, int contrato_id, EstatusEvaluacionEnum estatus);
        ReporteEvaluacionSubcontratistaDTO GetReporteEvaluacionSubcontratista(int evaluacion_id);
        #endregion

        #region Calendario Evaluaciones
        Dictionary<string, object> llenarCalendarioEvaluaciones(List<string> lstFiltroCC, List<int?> lstFiltroSubC);

        Dictionary<string, object> buscarEvaluaciones(List<string> cc, List<string> subContratistas);
	    #endregion

        #region DASHBOARD
         Dictionary<string, object> FillComboEspecialidad();

        #region GENERAL
         Dictionary<string, object> GetGraficaCumplimientoPorSubContratista(List<string> lstFiltroCC, List<int> lstFiltroSubC, DateTime fechaFiltroInicio, DateTime fechaFiltroFin, int estado_id, int municipio_id, List<int> listaEspecialidades);

         Dictionary<string, object> GetGraficaCumplimientoPorElementos(List<string> lstFiltroCC, List<int> lstFiltroSubC, DateTime fechaFiltroInicio, DateTime fechaFiltroFin, int estado_id, int municipio_id, List<int> listaEspecialidades);

         Dictionary<string, object> GetGraficaCumplimientoPorEvaluacion(List<string> lstFiltroCC, List<int> lstFiltroSubC, DateTime fechaFiltroInicio, DateTime fechaFiltroFin, int estado_id, int municipio_id, List<int> listaEspecialidades);

         MemoryStream crearReporte(List<string> lstFiltroCC, List<int> lstFiltroSubC, DateTime fechaFiltroInicio, DateTime fechaFiltroFin, int estado_id, int municipio_id, List<int> listaEspecialidades);

        #endregion

        #region DETALLES
         Dictionary<string, object> GetCumplimientosElementos(List<string> lstfiltroCC, List<int> lstfiltroSubC, DateTime fechaFiltroInicio, DateTime fechaFiltroFin, int idElemento, int estado_id, int municipio_id, List<int> listaEspecialidades);
        #endregion        

        #region REPORTE EJECUTIVO
         Dictionary<string, object> GetReporteEjecutivo(List<string> lstfiltroCC, List<int> lstfiltroSubC, DateTime fechaFiltroInicio, DateTime fechaFiltroFin, int estado_id, int municipio_id, List<int> listaEspecialidades);

        Tuple<MemoryStream, string> DescargarExcelPersonalActivo();
        #endregion
        
          #endregion

        Dictionary<string, object> FillComboEstados();
        Dictionary<string, object> FillComboMunicipios(int estado_id);
    }
}