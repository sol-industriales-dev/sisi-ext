using Core.DTO.Administracion.Seguridad.Capacitacion;
using Core.DTO.Principal.Generales;
using Core.DTO.RecursosHumanos.Capacitacion;
using Core.Entity.Administrativo.Seguridad.Capacitacion;
using Core.Entity.Principal.Usuarios;
using Core.Entity.RecursosHumanos.Captura;
using Core.Enum.Administracion.Seguridad.Capacitacion;
using Core.Enum.Multiempresa;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.DAO.Administracion.Seguridad
{
    public interface ICapacitacionDAO
    {

        #region Cursos
        Dictionary<string, object> getCursos(List<int> clasificaciones, List<int> puestos, int estatus);
        Dictionary<string, object> getCursoById(int id);
        Dictionary<string, object> getExamenesCursoById(int id);
        byte[] descargarArchivo(long examen_id);
        string getFileName(long examen_id);
        Dictionary<string, object> getTipoExamen(int curso_id);
        Dictionary<string, object> getClasificacionCursos();
        Dictionary<string, object> getPuestosEnkontrol();
        Dictionary<string, object> guardarCurso(tblS_CapacitacionCursos curso);
        Dictionary<string, object> guardarExamenes(List<tblS_CapacitacionCursosExamen> examenes, List<HttpPostedFileBase> archivos);
        Dictionary<string, object> actualizarCurso(tblS_CapacitacionCursos curso, List<tblS_CapacitacionCursosMando> mandos, List<tblS_CapacitacionCursosPuestos> puestosNuevos, List<tblS_CapacitacionCursosPuestosAutorizacion> puestosAutorizacionNuevos, List<tblS_CapacitacionCursosCC> centrosCosto);
        Dictionary<string, object> eliminarExamen(int examen_id);

        /// <summary>
        /// Obtiene los estatus que puede tener un curso de capacitación.
        /// </summary>
        /// <returns></returns>
        Dictionary<string, object> GetEstatusCursos();

        /// <summary>
        /// Elimina un curso y provoca un borrado en cascada con todas las entidades relacionadas.
        /// </summary>
        /// <param name="cursoID"></param>
        /// <returns></returns>
        Dictionary<string, object> EliminarCurso(int cursoID);

        Dictionary<string, object> getPuestosEnkontrolMandos(List<int> mandos);
        #endregion

        #region Control de Asistencia
        /// <summary>
        /// Obtiene un listado de los centros de costos en SIGOPLAN en forma de combo.
        /// </summary>
        /// <returns></returns>
        Dictionary<string, object> ObtenerComboCC();

        /// <summary>
        /// Obtiene una lista de controles de asistencia existentes y su información relacioanda.
        /// </summary>
        /// <param name="cc"></param>
        /// <param name="estado"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        Dictionary<string, object> ObtenerListaControlesAsistencia(string cc, int estado, DateTime fechaInicio, DateTime fechaFin);

        /// <summary>
        /// Obtiene una lista en formato autocomplete sobre cursos, ya sea buscando por su nombre o por la clave.
        /// </summary>
        /// <param name="term"></param>
        /// <param name="porClave"></param>
        /// <returns></returns>
        object GetCursosAutocomplete(string term, bool porClave);


        /// <summary>
        /// Obtiene una lista en formato autocomplete sobre usuarios de SIGOPLAN, ya sea buscando por su nombre o por su clave de empleado.
        /// </summary>
        /// <param name="term"></param>
        /// <param name="porClave"></param>
        /// <returns></returns>
        object GetUsuariosAutocomplete(string term, bool porClave);

        /// <summary>
        /// Obtiene una lista en formato autocomplete sobre lugares en los que se haya impartido algún curso.
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        object GetLugarCursoAutocomplete(string term);

        /// <summary>
        /// Obtiene una lista en formato autocomplete sobre empleados registrados en EnKontrol Cplan y Arrendadora.
        /// </summary>
        /// <param name="term"></param>
        /// <param name="porClave"></param>
        /// <returns></returns>
        object GetEmpleadoEnKontrolAutocomplete(string term, bool porClave);

        /// <summary>
        /// Crea una nueva lista de asistencia.
        /// </summary>
        /// <param name="controlAsistencia"></param>
        /// <returns></returns>
        Dictionary<string, object> CrearControlAsistencia(tblS_CapacitacionControlAsistencia controlAsistencia);

        /// <summary>
        /// Sube un archivo de lista de control de asistencia al servidor.
        /// </summary>
        /// <param name="archivo"></param>
        /// <param name="controlAsistenciaID"></param>
        /// <returns></returns>
        Dictionary<string, object> SubirArchivoControlAsistencia(HttpPostedFileBase archivo, int controlAsistenciaID);

        /// <summary>
        /// Retorna un stream con el archivo de control asistencia a descargar.
        /// </summary>
        /// <param name="controlAsistenciaID"></param>
        /// <returns></returns>
        Tuple<Stream, string> DescargarListaControlAsistencia(int controlAsistenciaID);


        /// <summary>
        /// Retorna un stream con el archivo de formato de autorización a descargar.
        /// </summary>
        /// <param name="controlAsistenciaID"></param>
        /// <returns></returns>
        Tuple<Stream, string> DescargarFormatoAutorizacion(int controlAsistenciaID);

        /// <summary>
        /// Retorna todos los datos sobre un control de asistencia.
        /// </summary>
        /// <param name="controlAsistenciaID"></param>
        /// <returns></returns>
        Dictionary<string, object> CargarDatosControlAsistencia(int controlAsistenciaID);

        /// <summary>
        /// Carga la lista de asistenes de de una capacitación.
        /// </summary>
        /// <param name="controlAsistenciaID"></param>
        /// <returns></returns>
        Dictionary<string, object> CargarAsistentesCapacitacion(int controlAsistenciaID);

        /// <summary>
        /// Guarda los exámenes diagnóstico y final de cada asistente de un control de asistencia.
        /// </summary>
        /// <param name="listaExamenesAsistentes"></param>
        /// <returns></returns>
        Dictionary<string, object> GuardarExamenesAsistentes(List<ExamenesAsistenteDTO> listaExamenesAsistentes, int jefeID, int coordinadorID, int secretarioID, int gerenteID, string rfc, string razonSocial);

        /// <summary>
        /// Descarga un examen específico de un asistente.
        /// </summary>
        /// <param name="controlAsistenciaDetalleID"></param>
        /// <param name="tipoExamen"></param>
        /// <returns></returns>
        Tuple<Stream, string> DescargarExamenAsistente(int controlAsistenciaDetalleID, int tipoExamen);

        /// <summary>
        /// Obtiene un objeto con la información necesaria para generar el reporte de control de asistencia de un curso.
        /// </summary>
        /// <param name="controlAsistenciaID"></param>
        /// <returns></returns>
        ControlAsistenciaDTO ObtenerDatosControlAsistenciaReporte(int controlAsistenciaID);

        /// <summary>
        /// Guarda una lista de evaluaciones de asistentes para un curso general (que no lleva examen).
        /// </summary>
        /// <param name="listaAsistentes"></param>
        /// <returns></returns>
        Dictionary<string, object> GuardarEvaluacionAsistentes(List<AsistenteCursoDTO> listaAsistentes);

        /// <summary>
        /// Elimina una capacitación, autorizaciones, alertas y detalles relacionados a ella.
        /// </summary>
        /// <param name="controlAsistenciaID"></param>
        /// <returns></returns>
        Dictionary<string, object> EliminarControlAsistencia(int controlAsistenciaID);

        Dictionary<string, object> guardarArchivosDC3(HttpPostedFileBase archivoDC3, int controlAsistenciaDetalleID);

        Tuple<Stream, string> DescargarDC3(int controlAsistenciaDetalleID);

        Dictionary<string, object> GuardarMigracionEmpleado(int claveEmpleado, string cc, int empresa);
        #endregion

        #region Autorización Capacitación

        /// <summary>
        /// Obtiene los estatus pertinentes a la autorización de un control de asistencia.
        /// </summary>
        /// <returns></returns>
        Dictionary<string, object> ObtenerComboEstatusAutorizacionCapacitacion();

        /// <summary>
        /// Obtiene un listado de autorizaciones de capacitaciones según los filtros indicados.
        /// </summary>
        /// <param name="cc"></param>
        /// <param name="estatus"></param>
        /// <returns></returns>
        Dictionary<string, object> ObtenerAutorizaciones(string cc, int curso, int estatus);

        /// <summary>
        /// Obtiene un listado con las personas responsables de autorizar un curso de capacitación.
        /// </summary>
        /// <param name="capacitacionID"></param>
        /// <returns></returns>
        Dictionary<string, object> ObtenerAutorizantes(int capacitacionID);

        /// <summary>
        /// Obtiene los datos necesarios para generar el reporte de formato de autorización de un curso de capacitación.
        /// </summary>
        /// <param name="controlAsistenciaID"></param>
        /// <returns></returns>
        FormatoAutorizacionDTO ObtenerDatosFormatoAutorizacion(int controlAsistenciaID);

        /// <summary>
        /// Firma electrónicamente autorizando un formato de autorización de un curso de capacitación.
        /// </summary>
        /// <param name="controlAsistenciaID"></param>
        /// <returns></returns>
        Dictionary<string, object> AutorizarControlAsistencia(int controlAsistenciaID);

        /// <summary>
        /// Firma electrónicamente rechazando un formato de autorización de un curso de capacitación.
        /// </summary>
        /// <param name="controlAsistenciaID"></param>
        /// <param name="comentario"></param>
        /// <returns></returns>
        Dictionary<string, object> RechazarControlAsistencia(int controlAsistenciaID, string comentario);

        /// <summary>
        /// Envía un correo para el usuario siguiente en en el proceso de autorización electrónica de una capacitación.
        /// </summary>
        /// <param name="controlAsistenciaID"></param>
        /// <param name="pdf"></param>
        /// <returns></returns>
        Dictionary<string, object> EnviarCorreoAutorizacion(int controlAsistenciaID, List<Byte[]> pdf);

        /// <summary>
        /// Envía un correo notificando a todos los autorizantes de una capacitación que el proceso de autorización ha sido completado.
        /// </summary>
        /// <param name="controlAsistenciaID"></param>
        /// <param name="pdf"></param>
        /// <returns></returns>
        Dictionary<string, object> EnviarCorreoAutorizacionCompleta(int controlAsistenciaID, List<Byte[]> pdf);

        /// <summary>
        /// Envía un correo a los autorizantes de una capacitación con el comentario de rechazo y el formato adjunto.
        /// </summary>
        /// <param name="controlAsistenciaID"></param>
        /// <param name="pdf"></param>
        /// <returns></returns>
        Dictionary<string, object> EnviarCorreoRechazo(int controlAsistenciaID, List<Byte[]> pdf);
        Dictionary<string, object> guardarCargaMasiva(HttpPostedFileBase archivo);
        #endregion

        #region Matriz de empleados

        /// <summary>
        /// Obtiene la lista de CCs en formato combo de EnKontrol de una empresa específica.
        /// </summary>
        /// <param name="empresa"></param>
        /// <returns></returns>
        Dictionary<string, object> ObtenerComboCCEnKontrol(EmpresaEnum empresa);

        /// <summary>
        /// Obtiene a todos los empleados de EnKontrol filtrado por cc y puesto.
        /// </summary>
        /// <param name="ccs"></param>
        /// <param name="puestos"></param>
        /// <returns></returns>
        List<EmpleadoPuestoDTO> ObtenerEmpleados(List<string> ccsCplan, List<string> ccsArr, List<string> puestos);

        /// <summary>
        /// Obtiene todos los cursos a los que haya asistido un empleado (En donde aprobó y si aplicó para autorización, en donde esté autorizado).
        /// </summary>
        /// <param name="claveEmpleado"></param>
        /// <param name="puestoID"></param>
        /// <returns></returns>
        Dictionary<string, object> ObtenerCursosEmpleado(int claveEmpleado, int puestoID);

        /// <summary>
        /// Descarga todos los archivos relacionados a un empleado en específico.
        /// </summary>
        /// <param name="claveEmpleado"></param>
        /// <param name="licencia"></param>
        /// <returns></returns>
        Tuple<Stream, string> DescargarExpedienteEmpleado(int claveEmpleado, byte[] licencia);

        /// <summary>
        /// Obtiene los datos necesarios para imprimir una licencia de habilidades en base a la clave de un empleado.
        /// </summary>
        /// <param name="claveEmpleado"></param>
        /// <returns></returns>
        LicenciaHabilidadesDTO ObtenerLicenciaEmpleado(int claveEmpleado, int empresa);

        /// <summary>
        /// Obtiene un listado con toda la información extracurricular de un empleado.
        /// </summary>
        /// <param name="claveEmpleado"></param>
        /// <returns></returns>
        Dictionary<string, object> ObtenerExtracurricularesEmpleado(int claveEmpleado);

        /// <summary>
        /// Registra un nuevo logro extracurricular para un empleado.
        /// </summary>
        /// <param name="claveEmpleado"></param>
        /// <param name="nombre"></param>
        /// <param name="duracion"></param>
        /// <param name="fecha"></param>
        /// <param name="fechaFin"></param>
        /// <param name="evidencia"></param>
        /// <returns></returns>
        Dictionary<string, object> SubirEvidenciaExtracurricular(int claveEmpleado, string nombre, decimal duracion, string fecha, string fechaFin, HttpPostedFileBase evidencia);

        /// <summary>
        /// Descarga un archivo de tipo evidencia extracurricular de un empleado.
        /// </summary>
        /// <param name="extracurricularID"></param>
        /// <returns></returns>
        Tuple<Stream, string> DescargarEvidenciaExtracurricular(int extracurricularID);

        /// <summary>
        /// Elimina un logro extracurricular de un empleado.
        /// </summary>
        /// <param name="extracurricularID"></param>
        /// <returns></returns>
        Dictionary<string, object> EliminarEvidenciaExtracurricular(int extracurricularID);

        Dictionary<string, object> GetArchivosMatrizEmpleados(int claveEmpleado);
        #endregion

        #region Dashboard
        /// <summary>
        /// Retorna los datos generales a mostrar en el dashboard de capacitaciones.
        /// </summary>
        /// <param name="ccsCplan"></param>
        /// <param name="ccsArr"></param>
        /// <returns></returns>
        Dictionary<string, object> CargarDatosGeneralesDashboard(List<string> ccsCplan, List<string> ccsArr, List<string> departamentosIDs, DateTime fechaInicio, DateTime fechaFin, List<string> clasificacion);
        MemoryStream DescargarExcelExpirados();
        #endregion

        #region Matriz de Necesidades
        /// <summary>
        /// Carga los departamentos correspondientes por cada CC.
        /// </summary>
        /// <param name="ccsCplan"></param>
        /// <param name="ccsArr"></param>
        /// <returns></returns>
        Dictionary<string, object> ObtenerAreasPorCC(List<string> ccsCplan, List<string> ccsArr);

        /// <summary>
        /// Consulta las capacitaciones impartidas en base a los filtros y cálcula varios valores para mostrar en la Matriz de Necesidades.
        /// </summary>
        /// <param name="ccsCplan"></param>
        /// <param name="ccsArr"></param>
        /// <param name="departamentosIDs"></param>
        /// <param name="clasificaciones"></param>
        /// <returns></returns>
        Dictionary<string, object> CargarDatosMatrizNecesidades(List<string> ccsCplan, List<string> ccsArr, List<string> departamentosIDs, List<ClasificacionCursoEnum> clasificaciones);

        Dictionary<string, object> CargarDatosSeccionMatriz(List<string> ccsCplan, List<string> ccsArr, List<string> departamentosIDs, List<ClasificacionCursoEnum> clasificaciones, SeccionMatrizEnum seccion);

        /// <summary>
        /// Exporta la tabla de personal activo mostrado en la vista Matriz de Necesidades a un archivo Excel.
        /// </summary>
        /// <returns></returns>
        Tuple<MemoryStream, string> DescargarExcelPersonalActivo();

        Dictionary<string, object> GetFormatoCambioID(int claveEmpleado);
        #endregion

        #region Privilegios
        /// <summary>
        /// Consulta el privilegio del usuario Actual
        /// </summary>
        /// <returns></returns>
        tblS_CapacitacionEmpleadoPrivilegio getPrivilegioActual();
        /// <summary>
        /// Consuta los empleados y su privilegio
        /// </summary>
        /// <param name="busq">Filtros</param>
        /// <returns></returns>
        Dictionary<string, object> ObtenerEmpleadosPrivilegios(BusqPrivilegiosDTO busq);
        /// <summary>
        /// Guardar Empleados privilegio
        /// </summary>
        /// <param name="lst">empleados con privilegios</param>
        /// <returns></returns>
        Dictionary<string, object> guardarEmpleadosPrivilegios(List<tblS_CapacitacionEmpleadoPrivilegio> lst);
        #endregion

        #region Catálogos
        Dictionary<string, object> getRelacionesCCAutorizantes();
        Dictionary<string, object> guardarNuevaRelacionCCAutorizante(RelacionCCAutorizanteDTO relacion);
        Dictionary<string, object> editarRelacionCCAutorizante(RelacionCCAutorizanteDTO relacion);
        Dictionary<string, object> eliminarRelacionCCAutorizante(RelacionCCAutorizanteDTO relacion);
        Dictionary<string, object> getUsuarioPorClave(int claveEmpleado);
        Dictionary<string, object> getRelacionesCCDepartamentoRazonSocial();
        Dictionary<string, object> guardarNuevaRelacionCCDepartamentoRazonSocial(List<RelacionCCDepartamentoRazonSocialDTO> relaciones);
        Dictionary<string, object> editarRelacionCCDepartamentoRazonSocial(RelacionCCDepartamentoRazonSocialDTO relacion);
        Dictionary<string, object> eliminarRelacionCCDepartamentoRazonSocial(RelacionCCDepartamentoRazonSocialDTO relacion);
        Dictionary<string, object> getDepartamentosCombo();
        #endregion

        List<ComboDTO> getRazonSocialCombo();
        MemoryStream crearExcelRelacionCursosPuestos();
        List<ComboDTO> obtenerComboCursos();
        Dictionary<string, object> guardarCargaMasivaControlAsistencia(HttpPostedFileBase controlAsistencia);
        Dictionary<string, object> guardarCargaMasivaRelacionCursosPuestosAutorizacion(HttpPostedFileBase archivo);
        Dictionary<string, object> ObtenerComboCCAmbasEmpresas();
        Dictionary<string, object> ObtenerComboCCMigracion();

        #region Personal Autorizado
        Dictionary<string, object> getListasAutorizacion(List<int> listaCursos, List<tblS_CapacitacionListaAutorizacionCC> listaCC);
        Dictionary<string, object> getListasAutorizacionCombo();
        Dictionary<string, object> guardarListaAutorizacion(tblS_CapacitacionListaAutorizacion listaAutorizacion, List<tblS_CapacitacionListaAutorizacionRFC> listaRFC, List<tblS_CapacitacionListaAutorizacionCC> listaCentrosCosto);
        Dictionary<string, object> editarListaAutorizacion(tblS_CapacitacionListaAutorizacion listaAutorizacion, List<tblS_CapacitacionListaAutorizacionRFC> listaRFC, List<tblS_CapacitacionListaAutorizacionCC> listaCentrosCosto);
        Dictionary<string, object> eliminarListaAutorizacion(int listaAutorizacionID);
        Dictionary<string, object> getListaAutorizacionByID(int listaAutorizacionID);
        object getAutorizanteEnkontrolAutocomplete(string term);
        Dictionary<string, object> guardarInformacionAutorizados(ListaAutorizacionDTO listaAutorizacion);
        ListaAutorizacionReporteDTO getListaAutorizacionReporte(int listaAutorizacionID, int razonSocialID, int departamento, string cc, int empresa);
        Dictionary<string, object> cargarDashboardPersonalAutorizado(FiltrosDashboardPersonalAutorizadoDTO filtros);
        Dictionary<string, object> getCorreosListaAutorizacion(int listaAutorizacionID);
        bool enviarCorreoListaAutorizacion(int listaAutorizacionID, List<Byte[]> archivoListaAutorizacion, List<string> listaCorreos);
        #endregion

        #region Detección de Necesidades
        #region Ciclos de Trabajo
        List<ComboDTO> getCiclosTrabajoCombo();
        Dictionary<string, object> guardarNuevoCiclo(tblS_CapacitacionDNCicloTrabajo ciclo, List<tblS_CapacitacionDNCicloTrabajoAreas> listaAreas, List<tblS_CapacitacionDNCicloTrabajoCriterio> listaCriterios);
        Dictionary<string, object> getCicloByID(int cicloID);
        Dictionary<string, object> guardarRegistroCiclo(tblS_CapacitacionDNCicloTrabajoRegistro registroCiclo, List<tblS_CapacitacionDNCicloTrabajoRegistroRevisiones> listaRevisiones, List<tblS_CapacitacionDNCicloTrabajoRegistroPropuestas> listaPropuestas, List<tblS_CapacitacionDNCicloTrabajoRegistroAreas> listaAreas, List<int> areasSeguimiento, List<int> interesados);
        Dictionary<string, object> getRegistrosCiclos(FiltrosRegistrosCiclo filtros);
        Dictionary<string, object> getListaSeguimiento(List<string> listaCC, TipoSeguimientoEnum tipoSeguimiento, DateTime fechaInicio, DateTime fechaFin);
        MemoryStream DescargarExcelSeguimientoAcciones();
        Dictionary<string, object> guardarSeguimientoAcciones(List<tblS_CapacitacionDNCicloTrabajoRegistro> capturaEvidencias, List<HttpPostedFileBase> evidencias, List<tblS_CapacitacionDNCicloTrabajoRegistro> capturaEvaluaciones);
        Dictionary<string, object> guardarSeguimientoPropuestas(List<tblS_CapacitacionDNCicloTrabajoRegistroPropuestas> capturaEvidencias, List<HttpPostedFileBase> evidencias, List<tblS_CapacitacionDNCicloTrabajoRegistroPropuestas> capturaEvaluaciones);
        Dictionary<string, object> cargarDatosArchivoEvidenciaSeguimientoAcciones(int id);
        Tuple<Stream, string> descargarArchivoEvidenciaAccion(int id);
        Dictionary<string, object> cargarDatosArchivoEvidenciaSeguimientoPropuestas(int id);
        Tuple<Stream, string> descargarArchivoEvidenciaPropuesta(int id);
        Dictionary<string, object> cargarDashboardCiclos(List<string> listaCC, List<AreaDTO> listaAreas, DateTime fechaInicio, DateTime fechaFin);
        Dictionary<string, object> getRegistroCicloTrabajoByID(int id);
        List<CicloTrabajoDTO> GetTablaCicloTrabajo();
        List<CicloTrabajoCriterioDTO> GetTablaCriterioTrabajo(int id);
        bool EliminarCicloTrabajo(int id);
        bool EditarCicloTrabajo(CicloTrabajoDTO parametros, List<tblS_CapacitacionDNCicloTrabajoCriterio> criterio,List<tblS_CapacitacionDNCicloTrabajoAreas> lstAreass);
        Dictionary<string, object> getListaDepartamientos(int listaAutorizacionID);
        List<tblP_Usuario> llenarCorreos(int _IdUsuario);
        Dictionary<string, object> GetAreaSeguimiento();
        Dictionary<string, object> CargarDatosSeccionSeguimientoAcciones(List<string> listaCC, TipoSeguimientoEnum tipoSeguimiento, DateTime fechaInicio, DateTime fechaFin, List<int> listaAreasSeguimientoHistorial, SeccionSeguimientoAccionesEnum seccion);
        Dictionary<string, object> EliminarRegistroCicloTrabajo(tblS_CapacitacionDNCicloTrabajoRegistro ciclo);
        Dictionary<string, object> CargarCiclosRequeridos();
        Dictionary<string, object> GuardarCiclosRequeridos(List<tblS_CapacitacionDNCiclosRequeridos> listaCiclosRequeridos);
        #endregion

        #region Detecciones Primarias
        Dictionary<string, object> getRegistrosDeteccionesPrimarias(List<string> listaCC, List<AreaDTO> listaAreas, DateTime fecha);
        Dictionary<string, object> guardarDeteccionPrimaria(tblS_CapacitacionDNDeteccionPrimaria deteccionPrimaria, List<tblS_CapacitacionDNDeteccionPrimariaNecesidad> listaNecesidades, List<tblS_CapacitacionDNDeteccionPrimariaAreas> listaAreas);
        NecesidadPrimariaReporteDTO getDeteccionPrimariaReporte(int deteccionPrimariaID);
        #endregion

        #region Recorridos
        Dictionary<string, object> getRegistrosRecorridos(List<string> listaCC, List<AreaDTO> listaAreas, DateTime fechaInicio, DateTime fechaFin, int realizador);
        Dictionary<string, object> guardarNuevoRecorrido(tblS_CapacitacionDNRecorrido recorrido, List<RecorridoHallazgoDTO> listaHallazgos, List<tblS_CapacitacionDNRecorridoAreas> listaAreas, List<HttpPostedFileBase> evidencias);
        Dictionary<string, object> editarRecorrido(tblS_CapacitacionDNRecorrido recorrido, List<RecorridoHallazgoDTO> listaHallazgos, List<tblS_CapacitacionDNRecorridoAreas> listaAreas, List<HttpPostedFileBase> evidencias);
        Dictionary<string, object> EliminarRegistroRecorrido(int recorrido_id);
        Dictionary<string, object> getRecorridoByID(int recorridoID);
        Dictionary<string, object> guardarSeguimientoRecorrido(List<tblS_CapacitacionDNRecorridoHallazgo> listaSeguimiento);
        Dictionary<string, object> cargarDashboardRecorridos(List<string> listaCC, List<AreaDTO> listaAreas, DateTime fechaInicio, DateTime fechaFin, int realizador);
        RecorridoReporteDTO getRecorridoReporte(int recorridoID);
        List<string> enviarCorreoRecorrido(int recorridoID, List<int> usuarios, List<Byte[]> downloadPDF);
        bool enviarCorreoi(int recorridoID, List<Byte[]> archivo);
        Dictionary<string, object> VerificarCorreosValidos(List<int> listaUsuariosID);

        #region CARGAR EVIDENCIAS EN RECORRIDOS
        Dictionary<string, object> GuardarEvidenciaRecorrido(List<HttpPostedFileBase> lstArchivos, int FK_Recorrido);
        Dictionary<string, object> GetEvidenciasRecorrido(int FK_Recorrido);
        Dictionary<string, object> VisualizarEvidenciaRecorrido(int FK_Recorrido);
        Dictionary<string, object> EliminarEvidenciaRecorrido(int idArchivo);
        #endregion
        #endregion
        #endregion

        #region Competencias Operativas
        Dictionary<string, object> getPromedioEvaluaciones(List<string> listaCC, List<AreaDTO> listaAreas, DateTime fecha);
        #endregion

        #region CAMBIOS DE CATEGORIAS
        List<tblRH_AutorizacionFormatoCambio> getAutorizacion(int idFormato);
        List<resultCapacitacionDTO> TablaFormatosPendientes(FiltrosCapacitacionDTO parametros);

        Dictionary<string, object> postSubirArchivos(int id, EmpresaEnum empresa, List<HttpPostedFileBase> archivo);
        byte[] descargarArchivoCO(int id);
        string getFileNameCO(int id);
        Dictionary<string, object> obtenerArchivoCODescargas(int idFormatoCambio);
        List<tblS_CapacitacionCO_GCArchivos> getArchivosFormatoCambio(int formatoCambioID);
        Dictionary<string, object> EliminarArchivosFormatoCambio(int formatoCambio_id);
        #endregion

        #region Indicador Hrs-Hombre
        Dictionary<string, object> getEquiposCombo();
        Dictionary<string, object> GetEquipoAdiestramientoCombo();
        Dictionary<string, object> getEmpleadoPorClave(int claveEmpleado);
        Dictionary<string, object> cargarHorasAdiestramiento(List<string> listaCC, DateTime fechaInicial, DateTime fechaFinal, List<int> equipos, List<int> equiposGrafica, List<int> actividades);
        Dictionary<string, object> cargarHorasAdiestramientoColaborador(List<string> listaCC, DateTime fechaInicial, DateTime fechaFinal, List<int> equipos, List<int> actividades, int colaborador);
        Dictionary<string, object> guardarNuevoColaboradorCapacitacion(tblS_CapacitacionIHHColaboradorCapacitacion colaboradorCapacitacion, List<int> interesados);
        Dictionary<string, object> EditarColaboradorCapacitacion(tblS_CapacitacionIHHColaboradorCapacitacion colaboradorCapacitacion);
        Dictionary<string, object> EliminarColaboradorCapacitacion(tblS_CapacitacionIHHColaboradorCapacitacion colaboradorCapacitacion);
        Dictionary<string, object> getInfoColaboradorCapacitacion(int colaboradorCapacitacionID);
        Dictionary<string, object> GetInfoColaboradorCapacitacionActividad(int colaboradorCapacitacionID);
        Dictionary<string, object> guardarNuevoControlHoras(List<tblS_CapacitacionIHHControlHoras> listaControlHoras);
        Dictionary<string, object> GuardarNuevoControlActividad(List<tblS_CapacitacionIHHControlActividad> listaControlActividad);
        Dictionary<string, object> guardarLiberados(List<tblS_CapacitacionIHHColaboradorCapacitacion> captura, List<HttpPostedFileBase> archivos);
        Dictionary<string, object> GuardarLiberacionAdministrador(List<tblS_CapacitacionIHHColaboradorCapacitacion> captura);
        Dictionary<string, object> cargarDatosArchivoSoporteAdiestramiento(int id);
        Tuple<Stream, string> descargarArchivoSoporteAdiestramiento(int id);
        Tuple<Stream, string> descargarGlobal(int id, string gfx1, string gfx2);
        ColaboradorCapacitacionReporteDTO getColaboradorCapacitacionReporte(int colaboradorCapacitacionID);
        Dictionary<string, object> GetActividades();
        Dictionary<string, object> GetInteresados();
        Dictionary<string, object> GetMtto(int colaboradorCapacitacionId);
        Dictionary<string, object> GetMttoDetalle(int clave_empleado);

        MemoryStream DescargarExcelAdiestramientoActividades();
        MemoryStream DescargarExcelAdiestramientoHoras();
        #endregion
        #region ADMINISTRACIONINSTRUCTORES
        Dictionary<string, object> GetInstructores();
        List<ComboDTO> GetRolesCombo();
        Dictionary<string, object> PostGuardarInstructor(tblS_Capacitacion_PCAdministracionInstructores parametros, bool AddEdit);
        List<ComboDTO> GetInstructoresCombo(string cc);
        AdministracionInstructoresDTO getFechaInicio(string cveEmpleado);
        List<ComboDTO> GetCC();
        bool EliminarInstructor(int id);
        AdministracionInstructoresDTO ObtenerCCUnico(string cveEmpleado);
        Dictionary<string, object> CargarInformacionHorasHombre(HorasHombreDTO filtros);
        List<HorasHombreDTO> postObtenerTablaHorasHombre(HorasHombreDTO parametros,bool ActivarHeader);
        HorasHombreDTO obtenerInputPromedios(HorasHombreDTO parametros);

        MemoryStream crearExcelHorasHombreCapacitacion(HorasHombreDTO parametros);
        #endregion

        #region Plan de Capacitación
        Dictionary<string, object> cargarCalendarioPlanCapacitacion(string cc, List<TematicaCursoEnum> listaTematicas, int empresa, DateTime mesCalendario);
        #endregion

        Dictionary<string, object> GetEmpleadoCursosActos(int claveEmpleado);

        #region Factor Capacitación
        Dictionary<string, object> CargarFactorCapacitacion(int division, List<string> listaCentroCosto, DateTime fechaInicial, DateTime fechaFinal);
        Dictionary<string, object> CargarFactorCapacitacionDetalle(int division, List<string> listaCentroCosto, DateTime fechaInicial, DateTime fechaFinal);
        Dictionary<string, object> CargarEstadisticas(int division, List<string> listaCentroCosto, int anio, SeccionEstadisticaEnum seccion);
        Dictionary<string, object> CargarEfectividadCiclosDetalle(int division, List<string> listaCentroCosto, int anio);
        Dictionary<string, object> GetCentrosCostoDivision(int division);
        Dictionary<string, object> fillComboDivision();        
        Dictionary<string, object> FillAnios();
        #endregion

        #region CATALOGO FIRMAS INSTRUCTORES
        Dictionary<string, object> GetFirmasInstructores();

        Dictionary<string, object> CEFirmaInstructor(FirmaInstructorDTO objParamDTO, List<HttpPostedFileBase> lstArchivos);

        Dictionary<string, object> EliminarFirmaInstructor(FirmaInstructorDTO objParamDTO);

        Dictionary<string, object> FillCboUsuarios();

        ReporteCertificadoTrabajoDTO GetInformacionCertificadoTrabajo(ReporteCertificadoTrabajoDTO objParamDTO);
        #endregion

        bool AccesoPermitidoPrivilegioDivision(int division);
    }
}
