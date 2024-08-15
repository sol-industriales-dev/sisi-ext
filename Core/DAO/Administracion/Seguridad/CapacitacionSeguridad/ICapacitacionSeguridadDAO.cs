using Core.DTO.Administracion.Seguridad.CapacitacionSeguridad;
using Core.DTO.Principal.Generales;
using Core.DTO.RecursosHumanos.Capacitacion;
using Core.Entity.Administrativo.Seguridad.CapacitacionSeguridad;
using Core.Entity.Principal.Usuarios;
using Core.Entity.RecursosHumanos.Captura;
using Core.Enum.Administracion.Seguridad.CapacitacionSeguridad;
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
    public interface ICapacitacionSeguridadDAO
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
        Dictionary<string, object> guardarCurso(tblS_CapacitacionSeguridadCursos curso);
        Dictionary<string, object> guardarExamenes(List<tblS_CapacitacionSeguridadCursosExamen> examenes, List<HttpPostedFileBase> archivos);
        Dictionary<string, object> actualizarCurso(tblS_CapacitacionSeguridadCursos curso, List<tblS_CapacitacionSeguridadCursosMando> mandos, List<tblS_CapacitacionSeguridadCursosPuestos> puestosNuevos, List<tblS_CapacitacionSeguridadCursosPuestosAutorizacion> puestosAutorizacionNuevos, List<tblS_CapacitacionSeguridadCursosCC> centrosCosto);
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
        Dictionary<string, object> CrearControlAsistencia(tblS_CapacitacionSeguridadControlAsistencia controlAsistencia);

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
        #endregion

        #region Dashboard
        /// <summary>
        /// Retorna los datos generales a mostrar en el dashboard de capacitaciones.
        /// </summary>
        /// <param name="ccsCplan"></param>
        /// <param name="ccsArr"></param>
        /// <returns></returns>
        Dictionary<string, object> CargarDatosGeneralesDashboard(List<string> ccsCplan, List<string> ccsArr, DateTime fechaInicio, DateTime fechaFin, List<string> clasificacion);
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
        #endregion

        #region Privilegios
        /// <summary>
        /// Consulta el privilegio del usuario Actual
        /// </summary>
        /// <returns></returns>
        tblS_CapacitacionSeguridadEmpleadoPrivilegio getPrivilegioActual();
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
        Dictionary<string, object> guardarEmpleadosPrivilegios(List<tblS_CapacitacionSeguridadEmpleadoPrivilegio> lst);
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

        #region Personal Autorizado
        Dictionary<string, object> getListasAutorizacion(List<int> listaCursos, List<tblS_CapacitacionSeguridadListaAutorizacionCC> listaCC);
        Dictionary<string, object> getListasAutorizacionCombo();
        Dictionary<string, object> guardarListaAutorizacion(tblS_CapacitacionSeguridadListaAutorizacion listaAutorizacion, List<tblS_CapacitacionSeguridadListaAutorizacionRFC> listaRFC, List<tblS_CapacitacionSeguridadListaAutorizacionCC> listaCentrosCosto);
        Dictionary<string, object> editarListaAutorizacion(tblS_CapacitacionSeguridadListaAutorizacion listaAutorizacion, List<tblS_CapacitacionSeguridadListaAutorizacionRFC> listaRFC, List<tblS_CapacitacionSeguridadListaAutorizacionCC> listaCentrosCosto);
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
        Dictionary<string, object> guardarNuevoCiclo(tblS_CapacitacionSeguridadDNCicloTrabajo ciclo, List<tblS_CapacitacionSeguridadDNCicloTrabajoAreas> listaAreas, List<tblS_CapacitacionSeguridadDNCicloTrabajoCriterio> listaCriterios);
        Dictionary<string, object> getCicloByID(int cicloID);
        Dictionary<string, object> guardarRegistroCiclo(tblS_CapacitacionSeguridadDNCicloTrabajoRegistro registroCiclo, List<tblS_CapacitacionSeguridadDNCicloTrabajoRegistroRevisiones> listaRevisiones, List<tblS_CapacitacionSeguridadDNCicloTrabajoRegistroPropuestas> listaPropuestas, List<tblS_CapacitacionSeguridadDNCicloTrabajoRegistroAreas> listaAreas);
        Dictionary<string, object> getRegistrosCiclos(FiltrosRegistrosCiclo filtros);
        Dictionary<string, object> getListaSeguimiento(List<string> listaCC, TipoSeguimientoEnum tipoSeguimiento, DateTime fechaInicio, DateTime fechaFin);
        Dictionary<string, object> guardarSeguimientoAcciones(List<tblS_CapacitacionSeguridadDNCicloTrabajoRegistro> capturaEvidencias, List<HttpPostedFileBase> evidencias, List<tblS_CapacitacionSeguridadDNCicloTrabajoRegistro> capturaEvaluaciones);
        Dictionary<string, object> guardarSeguimientoPropuestas(List<tblS_CapacitacionSeguridadDNCicloTrabajoRegistroPropuestas> capturaEvidencias, List<HttpPostedFileBase> evidencias, List<tblS_CapacitacionSeguridadDNCicloTrabajoRegistroPropuestas> capturaEvaluaciones);
        Dictionary<string, object> cargarDatosArchivoEvidenciaSeguimientoAcciones(int id);
        Tuple<Stream, string> descargarArchivoEvidenciaAccion(int id);
        Dictionary<string, object> cargarDatosArchivoEvidenciaSeguimientoPropuestas(int id);
        Tuple<Stream, string> descargarArchivoEvidenciaPropuesta(int id);
        Dictionary<string, object> cargarDashboardCiclos(List<string> listaCC, List<AreaDTO> listaAreas, DateTime fechaInicio, DateTime fechaFin);
        Dictionary<string, object> getRegistroCicloTrabajoByID(int id);

        List<CicloTrabajoDTO> GetTablaCicloTrabajo();

        List<CicloTrabajoCriterioDTO> GetTablaCriterioTrabajo(int id);


        bool EliminarCicloTrabajo(int id);

        bool EditarCicloTrabajo(CicloTrabajoDTO parametros, List<tblS_CapacitacionSeguridadDNCicloTrabajoCriterio> criterio, List<tblS_CapacitacionSeguridadDNCicloTrabajoAreas> lstAreass);

        Dictionary<string, object> getListaDepartamientos(int listaAutorizacionID);


        List<tblP_Usuario> llenarCorreos(int _IdUsuario);
 



        #endregion

        #region Detecciones Primarias
        Dictionary<string, object> getRegistrosDeteccionesPrimarias(List<string> listaCC, List<AreaDTO> listaAreas, DateTime fecha);
        Dictionary<string, object> guardarDeteccionPrimaria(tblS_CapacitacionSeguridadDNDeteccionPrimaria deteccionPrimaria, List<tblS_CapacitacionSeguridadDNDeteccionPrimariaNecesidad> listaNecesidades, List<tblS_CapacitacionSeguridadDNDeteccionPrimariaAreas> listaAreas);
        NecesidadPrimariaReporteDTO getDeteccionPrimariaReporte(int deteccionPrimariaID);
        #endregion

        #region Recorridos
        Dictionary<string, object> getRegistrosRecorridos(List<string> listaCC, List<AreaDTO> listaAreas, DateTime fecha, int realizador);
        Dictionary<string, object> guardarNuevoRecorrido(tblS_CapacitacionSeguridadDNRecorrido recorrido, List<RecorridoHallazgoDTO> listaHallazgos, List<tblS_CapacitacionSeguridadDNRecorridoAreas> listaAreas, List<HttpPostedFileBase> evidencias);
        Dictionary<string, object> editarRecorrido(tblS_CapacitacionSeguridadDNRecorrido recorrido, List<RecorridoHallazgoDTO> listaHallazgos, List<tblS_CapacitacionSeguridadDNRecorridoAreas> listaAreas, List<HttpPostedFileBase> evidencias);
        Dictionary<string, object> getRecorridoByID(int recorridoID);
        Dictionary<string, object> guardarSeguimientoRecorrido(List<tblS_CapacitacionSeguridadDNRecorridoHallazgo> listaSeguimiento);
        Dictionary<string, object> cargarDashboardRecorridos(List<string> listaCC, List<AreaDTO> listaAreas, DateTime fecha, int realizador);
        RecorridoReporteDTO getRecorridoReporte(int recorridoID);
        List<string> enviarCorreoRecorrido(int recorridoID, List<int> usuarios, List<Byte[]> downloadPDF);
        bool enviarCorreoi(int recorridoID, List<Byte[]> archivo);


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
        List<tblS_CapacitacionSeguridadCO_GCArchivos> getArchivosFormatoCambio(int formatoCambioID);
        #endregion

        #region Indicador Hrs-Hombre
        Dictionary<string, object> getEquiposCombo();
        Dictionary<string, object> getEmpleadoPorClave(int claveEmpleado);
        Dictionary<string, object> cargarHorasAdiestramiento(List<string> listaCC, DateTime fechaInicial, DateTime fechaFinal);
        Dictionary<string, object> guardarNuevoColaboradorCapacitacion(tblS_CapacitacionSeguridadIHHColaboradorCapacitacion colaboradorCapacitacion);
        Dictionary<string, object> getInfoColaboradorCapacitacion(int colaboradorCapacitacionID);
        Dictionary<string, object> guardarNuevoControlHoras(List<tblS_CapacitacionSeguridadIHHControlHoras> listaControlHoras);
        Dictionary<string, object> guardarLiberados(List<tblS_CapacitacionSeguridadIHHColaboradorCapacitacion> captura, List<HttpPostedFileBase> archivos);
        Dictionary<string, object> cargarDatosArchivoSoporteAdiestramiento(int id);
        Tuple<Stream, string> descargarArchivoSoporteAdiestramiento(int id);
        ColaboradorCapacitacionReporteDTO getColaboradorCapacitacionReporte(int colaboradorCapacitacionID);
        #endregion
        #region ADMINISTRACIONINSTRUCTORES
        Dictionary<string, object> GetInstructores();
        List<ComboDTO> GetRolesCombo();
        Dictionary<string, object> PostGuardarInstructor(tblS_CapacitacionSeguridad_PCAdministracionInstructores parametros, bool AddEdit);
        List<ComboDTO> GetInstructoresCombo(string cc);
        AdministracionInstructoresDTO getFechaInicio(string cveEmpleado);
        List<ComboDTO> GetCC();
        bool EliminarInstructor(int id);
        AdministracionInstructoresDTO ObtenerCCUnico(string cveEmpleado);
        List<HorasHombreDTO> postObtenerTablaHorasHombre(HorasHombreDTO parametros,bool ActivarHeader);
        HorasHombreDTO obtenerInputPromedios(HorasHombreDTO parametros);

        MemoryStream crearExcelHorasHombreCapacitacion(HorasHombreDTO parametros);
        #endregion

        #region Plan de Capacitación
        Dictionary<string, object> cargarCalendarioPlanCapacitacion(string cc, List<TematicaCursoEnum> listaTematicas, int empresa, DateTime mesCalendario);
        #endregion

        Dictionary<string, object> GetEmpleadoCursosActos(int claveEmpleado);
    }
}
