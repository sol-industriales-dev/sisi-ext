using Core.DAO.Administracion.Seguridad;
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
using System.Web;

namespace Core.Service.Administracion.Seguridad
{
    public class CapacitacionService : ICapacitacionDAO
    {
        public ICapacitacionDAO capacitacionDAO { get; set; }

        public CapacitacionService(ICapacitacionDAO capacitacionDAO)
        {
            this.capacitacionDAO = capacitacionDAO;
        }

        #region Cursos
        public Dictionary<string, object> getCursos(List<int> clasificaciones, List<int> puestos, int estatus)
        {
            return capacitacionDAO.getCursos(clasificaciones, puestos, estatus);
        }
        public Dictionary<string, object> getCursoById(int id)
        {
            return capacitacionDAO.getCursoById(id);
        }
        public Dictionary<string, object> getExamenesCursoById(int id)
        {
            return capacitacionDAO.getExamenesCursoById(id);
        }
        public byte[] descargarArchivo(long examen_id)
        {
            return capacitacionDAO.descargarArchivo(examen_id);
        }
        public string getFileName(long examen_id)
        {
            return capacitacionDAO.getFileName(examen_id);
        }
        public Dictionary<string, object> getTipoExamen(int curso_id)
        {
            return capacitacionDAO.getTipoExamen(curso_id);
        }
        public Dictionary<string, object> getPuestosEnkontrol()
        {
            return capacitacionDAO.getPuestosEnkontrol();
        }
        public Dictionary<string, object> getClasificacionCursos()
        {
            return capacitacionDAO.getClasificacionCursos();
        }
        public Dictionary<string, object> guardarCurso(tblS_CapacitacionCursos curso)
        {
            return capacitacionDAO.guardarCurso(curso);
        }
        public Dictionary<string, object> guardarExamenes(List<tblS_CapacitacionCursosExamen> examenes, List<HttpPostedFileBase> archivos)
        {
            return capacitacionDAO.guardarExamenes(examenes, archivos);
        }
        public Dictionary<string, object> actualizarCurso(tblS_CapacitacionCursos curso, List<tblS_CapacitacionCursosMando> mandos, List<tblS_CapacitacionCursosPuestos> puestosNuevos, List<tblS_CapacitacionCursosPuestosAutorizacion> puestosAutorizacionNuevos, List<tblS_CapacitacionCursosCC> centrosCosto)
        {
            return capacitacionDAO.actualizarCurso(curso, mandos, puestosNuevos, puestosAutorizacionNuevos, centrosCosto);
        }
        public Dictionary<string, object> eliminarExamen(int examen_id)
        {
            return capacitacionDAO.eliminarExamen(examen_id);
        }

        public Dictionary<string, object> GetEstatusCursos()
        {
            return capacitacionDAO.GetEstatusCursos();
        }

        public Dictionary<string, object> EliminarCurso(int cursoID)
        {
            return capacitacionDAO.EliminarCurso(cursoID);
        }

        public Dictionary<string, object> getPuestosEnkontrolMandos(List<int> mandos)
        {
            return capacitacionDAO.getPuestosEnkontrolMandos(mandos);
        }
        #endregion

        #region Control de Asistencia
        public Dictionary<string, object> ObtenerComboCC()
        {
            return capacitacionDAO.ObtenerComboCC();
        }

        public Dictionary<string, object> ObtenerListaControlesAsistencia(string cc, int estado, DateTime fechaInicio, DateTime fechaFin)
        {
            return capacitacionDAO.ObtenerListaControlesAsistencia(cc, estado, fechaInicio, fechaFin);
        }

        public object GetCursosAutocomplete(string term, bool porClave)
        {
            return capacitacionDAO.GetCursosAutocomplete(term, porClave);
        }

        public object GetUsuariosAutocomplete(string term, bool porClave)
        {
            return capacitacionDAO.GetUsuariosAutocomplete(term, porClave);
        }

        public object GetLugarCursoAutocomplete(string term)
        {
            return capacitacionDAO.GetLugarCursoAutocomplete(term);
        }

        public object GetEmpleadoEnKontrolAutocomplete(string term, bool porClave)
        {
            return capacitacionDAO.GetEmpleadoEnKontrolAutocomplete(term, porClave);
        }

        public Dictionary<string, object> CrearControlAsistencia(tblS_CapacitacionControlAsistencia controlAsistencia)
        {
            return capacitacionDAO.CrearControlAsistencia(controlAsistencia);
        }

        public Dictionary<string, object> SubirArchivoControlAsistencia(HttpPostedFileBase archivo, int controlAsistenciaID)
        {
            return capacitacionDAO.SubirArchivoControlAsistencia(archivo, controlAsistenciaID);
        }

        public Tuple<Stream, string> DescargarListaControlAsistencia(int controlAsistenciaID)
        {
            return capacitacionDAO.DescargarListaControlAsistencia(controlAsistenciaID);
        }

        public Tuple<Stream, string> DescargarFormatoAutorizacion(int controlAsistenciaID)
        {
            return capacitacionDAO.DescargarFormatoAutorizacion(controlAsistenciaID);
        }

        public Dictionary<string, object> CargarDatosControlAsistencia(int controlAsistenciaID)
        {
            return capacitacionDAO.CargarDatosControlAsistencia(controlAsistenciaID);
        }

        public Dictionary<string, object> CargarAsistentesCapacitacion(int controlAsistenciaID)
        {
            return capacitacionDAO.CargarAsistentesCapacitacion(controlAsistenciaID);
        }

        public Dictionary<string, object> GuardarExamenesAsistentes(List<ExamenesAsistenteDTO> listaExamenesAsistentes, int jefeID, int coordinadorID, int secretarioID, int gerenteID, string rfc, string razonSocial)
        {
            return capacitacionDAO.GuardarExamenesAsistentes(listaExamenesAsistentes, jefeID, coordinadorID, secretarioID, gerenteID, rfc, razonSocial);
        }

        public Tuple<Stream, string> DescargarExamenAsistente(int controlAsistenciaDetalleID, int tipoExamen)
        {
            return capacitacionDAO.DescargarExamenAsistente(controlAsistenciaDetalleID, tipoExamen);
        }

        public ControlAsistenciaDTO ObtenerDatosControlAsistenciaReporte(int controlAsistenciaID)
        {
            return capacitacionDAO.ObtenerDatosControlAsistenciaReporte(controlAsistenciaID);
        }

        public Dictionary<string, object> GuardarEvaluacionAsistentes(List<AsistenteCursoDTO> listaAsistentes)
        {
            return capacitacionDAO.GuardarEvaluacionAsistentes(listaAsistentes);
        }

        public Dictionary<string, object> EliminarControlAsistencia(int controlAsistenciaID)
        {
            return capacitacionDAO.EliminarControlAsistencia(controlAsistenciaID);
        }

        public Dictionary<string, object> guardarArchivosDC3(HttpPostedFileBase archivoDC3, int controlAsistenciaDetalleID)
        {
            return capacitacionDAO.guardarArchivosDC3(archivoDC3, controlAsistenciaDetalleID);
        }

        public Tuple<Stream, string> DescargarDC3(int controlAsistenciaDetalleID)
        {
            return capacitacionDAO.DescargarDC3(controlAsistenciaDetalleID);
        }

        public Dictionary<string, object> GuardarMigracionEmpleado(int claveEmpleado, string cc, int empresa)
        {
            return capacitacionDAO.GuardarMigracionEmpleado(claveEmpleado, cc, empresa);
        }
        #endregion

        #region Autorización Capacitación
        public Dictionary<string, object> ObtenerComboEstatusAutorizacionCapacitacion()
        {
            return capacitacionDAO.ObtenerComboEstatusAutorizacionCapacitacion();
        }

        public Dictionary<string, object> ObtenerAutorizaciones(string cc, int curso, int estatus)
        {
            return capacitacionDAO.ObtenerAutorizaciones(cc, curso, estatus);
        }

        public Dictionary<string, object> ObtenerAutorizantes(int capacitacionID)
        {
            return capacitacionDAO.ObtenerAutorizantes(capacitacionID);
        }

        public FormatoAutorizacionDTO ObtenerDatosFormatoAutorizacion(int controlAsistenciaID)
        {
            return capacitacionDAO.ObtenerDatosFormatoAutorizacion(controlAsistenciaID);
        }

        public Dictionary<string, object> AutorizarControlAsistencia(int controlAsistenciaID)
        {
            return capacitacionDAO.AutorizarControlAsistencia(controlAsistenciaID);
        }

        public Dictionary<string, object> RechazarControlAsistencia(int controlAsistenciaID, string comentario)
        {
            return capacitacionDAO.RechazarControlAsistencia(controlAsistenciaID, comentario);
        }

        public Dictionary<string, object> EnviarCorreoRechazo(int controlAsistenciaID, List<byte[]> pdf)
        {
            return capacitacionDAO.EnviarCorreoRechazo(controlAsistenciaID, pdf);
        }

        public Dictionary<string, object> EnviarCorreoAutorizacion(int controlAsistenciaID, List<byte[]> pdf)
        {
            return capacitacionDAO.EnviarCorreoAutorizacion(controlAsistenciaID, pdf);
        }

        public Dictionary<string, object> EnviarCorreoAutorizacionCompleta(int controlAsistenciaID, List<byte[]> pdf)
        {
            return capacitacionDAO.EnviarCorreoAutorizacionCompleta(controlAsistenciaID, pdf);
        }

        public Dictionary<string, object> guardarCargaMasiva(HttpPostedFileBase archivo)
        {
            return capacitacionDAO.guardarCargaMasiva(archivo);
        }
        #endregion

        #region Matriz de empleados

        public Dictionary<string, object> ObtenerComboCCEnKontrol(EmpresaEnum empresa)
        {
            return capacitacionDAO.ObtenerComboCCEnKontrol(empresa);
        }

        public List<EmpleadoPuestoDTO> ObtenerEmpleados(List<string> ccsCplan, List<string> ccsArr, List<string> puestos)
        {
            return capacitacionDAO.ObtenerEmpleados(ccsCplan, ccsArr, puestos);
        }

        public Dictionary<string, object> ObtenerCursosEmpleado(int claveEmpleado, int puestoID)
        {
            return capacitacionDAO.ObtenerCursosEmpleado(claveEmpleado, puestoID);
        }

        public Tuple<Stream, string> DescargarExpedienteEmpleado(int claveEmpleado, byte[] licencia)
        {
            return capacitacionDAO.DescargarExpedienteEmpleado(claveEmpleado, licencia);
        }

        public LicenciaHabilidadesDTO ObtenerLicenciaEmpleado(int claveEmpleado, int empresa)
        {
            return capacitacionDAO.ObtenerLicenciaEmpleado(claveEmpleado, empresa);
        }

        public Dictionary<string, object> ObtenerExtracurricularesEmpleado(int claveEmpleado)
        {
            return capacitacionDAO.ObtenerExtracurricularesEmpleado(claveEmpleado);
        }

        public Dictionary<string, object> SubirEvidenciaExtracurricular(int claveEmpleado, string nombre, decimal duracion, string fecha, string fechaFin, HttpPostedFileBase evidencia)
        {
            return capacitacionDAO.SubirEvidenciaExtracurricular(claveEmpleado, nombre, duracion, fecha, fechaFin, evidencia);
        }

        public Tuple<Stream, string> DescargarEvidenciaExtracurricular(int extracurricularID)
        {
            return capacitacionDAO.DescargarEvidenciaExtracurricular(extracurricularID);
        }

        public Dictionary<string, object> EliminarEvidenciaExtracurricular(int extracurricularID)
        {
            return capacitacionDAO.EliminarEvidenciaExtracurricular(extracurricularID);
        }

        public Dictionary<string, object> GetArchivosMatrizEmpleados(int claveEmpleado)
        {
            return capacitacionDAO.GetArchivosMatrizEmpleados(claveEmpleado);
        }
        #endregion

        #region Dashboard
        public Dictionary<string, object> CargarDatosGeneralesDashboard(List<string> ccsCplan, List<string> ccsArr, List<string> departamentosIDs, DateTime FechaInicio, DateTime fechaFin, List<string> clasificacion)
        {
            return capacitacionDAO.CargarDatosGeneralesDashboard(ccsCplan, ccsArr, departamentosIDs, FechaInicio, fechaFin, clasificacion);
        }

        public MemoryStream DescargarExcelExpirados()
        {
            return capacitacionDAO.DescargarExcelExpirados();
        }
        #endregion

        #region Matriz de Necesidades
        public Dictionary<string, object> ObtenerAreasPorCC(List<string> ccsCplan, List<string> ccsArr)
        {
            return capacitacionDAO.ObtenerAreasPorCC(ccsCplan, ccsArr);
        }

        public Dictionary<string, object> CargarDatosMatrizNecesidades(List<string> ccsCplan, List<string> ccsArr, List<string> departamentosIDs, List<ClasificacionCursoEnum> clasificaciones)
        {
            return capacitacionDAO.CargarDatosMatrizNecesidades(ccsCplan, ccsArr, departamentosIDs, clasificaciones);
        }
        public Dictionary<string, object> CargarDatosSeccionMatriz(List<string> ccsCplan, List<string> ccsArr, List<string> departamentosIDs, List<ClasificacionCursoEnum> clasificaciones, SeccionMatrizEnum seccion)
        {
            return capacitacionDAO.CargarDatosSeccionMatriz(ccsCplan, ccsArr, departamentosIDs, clasificaciones, seccion);
        }
        public Tuple<MemoryStream, string> DescargarExcelPersonalActivo()
        {
            return capacitacionDAO.DescargarExcelPersonalActivo();
        }

        public Dictionary<string, object> GetFormatoCambioID(int claveEmpleado)
        {
            return capacitacionDAO.GetFormatoCambioID(claveEmpleado);
        }
        #endregion

        #region Privilegios
        public tblS_CapacitacionEmpleadoPrivilegio getPrivilegioActual()
        {
            return capacitacionDAO.getPrivilegioActual();
        }
        public Dictionary<string, object> ObtenerEmpleadosPrivilegios(BusqPrivilegiosDTO busq)
        {
            return capacitacionDAO.ObtenerEmpleadosPrivilegios(busq);
        }
        public Dictionary<string, object> guardarEmpleadosPrivilegios(List<tblS_CapacitacionEmpleadoPrivilegio> lst)
        {
            return capacitacionDAO.guardarEmpleadosPrivilegios(lst);
        }

        #endregion

        #region Catálogos
        public Dictionary<string, object> getRelacionesCCAutorizantes()
        {
            return capacitacionDAO.getRelacionesCCAutorizantes();
        }

        public Dictionary<string, object> guardarNuevaRelacionCCAutorizante(RelacionCCAutorizanteDTO relacion)
        {
            return capacitacionDAO.guardarNuevaRelacionCCAutorizante(relacion);
        }

        public Dictionary<string, object> editarRelacionCCAutorizante(RelacionCCAutorizanteDTO relacion)
        {
            return capacitacionDAO.editarRelacionCCAutorizante(relacion);
        }

        public Dictionary<string, object> eliminarRelacionCCAutorizante(RelacionCCAutorizanteDTO relacion)
        {
            return capacitacionDAO.eliminarRelacionCCAutorizante(relacion);
        }

        public Dictionary<string, object> getUsuarioPorClave(int claveEmpleado)
        {
            return capacitacionDAO.getUsuarioPorClave(claveEmpleado);
        }

        public Dictionary<string, object> getRelacionesCCDepartamentoRazonSocial()
        {
            return capacitacionDAO.getRelacionesCCDepartamentoRazonSocial();
        }

        public Dictionary<string, object> guardarNuevaRelacionCCDepartamentoRazonSocial(List<RelacionCCDepartamentoRazonSocialDTO> relaciones)
        {
            return capacitacionDAO.guardarNuevaRelacionCCDepartamentoRazonSocial(relaciones);
        }

        public Dictionary<string, object> editarRelacionCCDepartamentoRazonSocial(RelacionCCDepartamentoRazonSocialDTO relacion)
        {
            return capacitacionDAO.editarRelacionCCDepartamentoRazonSocial(relacion);
        }

        public Dictionary<string, object> eliminarRelacionCCDepartamentoRazonSocial(RelacionCCDepartamentoRazonSocialDTO relacion)
        {
            return capacitacionDAO.eliminarRelacionCCDepartamentoRazonSocial(relacion);
        }

        public Dictionary<string, object> getDepartamentosCombo()
        {
            return capacitacionDAO.getDepartamentosCombo();
        }
        #endregion

        public List<ComboDTO> getRazonSocialCombo()
        {
            return capacitacionDAO.getRazonSocialCombo();
        }

        public MemoryStream crearExcelRelacionCursosPuestos()
        {
            return capacitacionDAO.crearExcelRelacionCursosPuestos();
        }

        public List<ComboDTO> obtenerComboCursos()
        {
            return capacitacionDAO.obtenerComboCursos();
        }

        public Dictionary<string, object> guardarCargaMasivaControlAsistencia(HttpPostedFileBase controlAsistencia)
        {
            return capacitacionDAO.guardarCargaMasivaControlAsistencia(controlAsistencia);
        }

        public Dictionary<string, object> guardarCargaMasivaRelacionCursosPuestosAutorizacion(HttpPostedFileBase archivo)
        {
            return capacitacionDAO.guardarCargaMasivaRelacionCursosPuestosAutorizacion(archivo);
        }

        public Dictionary<string, object> ObtenerComboCCAmbasEmpresas()
        {
            return capacitacionDAO.ObtenerComboCCAmbasEmpresas();
        }

        public Dictionary<string, object> ObtenerComboCCMigracion()
        {
            return capacitacionDAO.ObtenerComboCCMigracion();
        }

        #region Personal Autorizado
        public Dictionary<string, object> getListasAutorizacion(List<int> listaCursos, List<tblS_CapacitacionListaAutorizacionCC> listaCC)
        {
            return capacitacionDAO.getListasAutorizacion(listaCursos, listaCC);
        }

        public Dictionary<string, object> getListasAutorizacionCombo()
        {
            return capacitacionDAO.getListasAutorizacionCombo();
        }

        public Dictionary<string, object> guardarListaAutorizacion(tblS_CapacitacionListaAutorizacion listaAutorizacion, List<tblS_CapacitacionListaAutorizacionRFC> listaRFC, List<tblS_CapacitacionListaAutorizacionCC> listaCentrosCosto)
        {
            return capacitacionDAO.guardarListaAutorizacion(listaAutorizacion, listaRFC, listaCentrosCosto);
        }

        public Dictionary<string, object> editarListaAutorizacion(tblS_CapacitacionListaAutorizacion listaAutorizacion, List<tblS_CapacitacionListaAutorizacionRFC> listaRFC, List<tblS_CapacitacionListaAutorizacionCC> listaCentrosCosto)
        {
            return capacitacionDAO.editarListaAutorizacion(listaAutorizacion, listaRFC, listaCentrosCosto);
        }

        public Dictionary<string, object> eliminarListaAutorizacion(int listaAutorizacionID)
        {
            return capacitacionDAO.eliminarListaAutorizacion(listaAutorizacionID);
        }

        public Dictionary<string, object> getListaAutorizacionByID(int listaAutorizacionID)
        {
            return capacitacionDAO.getListaAutorizacionByID(listaAutorizacionID);
        }

        public object getAutorizanteEnkontrolAutocomplete(string term)
        {
            return capacitacionDAO.getAutorizanteEnkontrolAutocomplete(term);
        }

        public Dictionary<string, object> guardarInformacionAutorizados(ListaAutorizacionDTO listaAutorizacion)
        {
            return capacitacionDAO.guardarInformacionAutorizados(listaAutorizacion);
        }

        public ListaAutorizacionReporteDTO getListaAutorizacionReporte(int listaAutorizacionID, int razonSocialID, int departamento, string cc, int empresa)
        {
            return capacitacionDAO.getListaAutorizacionReporte(listaAutorizacionID, razonSocialID, departamento, cc, empresa);
        }

        public Dictionary<string, object> cargarDashboardPersonalAutorizado(FiltrosDashboardPersonalAutorizadoDTO filtros)
        {
            return capacitacionDAO.cargarDashboardPersonalAutorizado(filtros);
        }

        public Dictionary<string, object> getCorreosListaAutorizacion(int listaAutorizacionID)
        {
            return capacitacionDAO.getCorreosListaAutorizacion(listaAutorizacionID);
        }

        public bool enviarCorreoListaAutorizacion(int listaAutorizacionID, List<Byte[]> archivoListaAutorizacion, List<string> listaCorreos)
        {
            return capacitacionDAO.enviarCorreoListaAutorizacion(listaAutorizacionID, archivoListaAutorizacion, listaCorreos);
        }
        #endregion

        #region Detección de Necesidades
        #region Ciclos de Trabajo
        public List<ComboDTO> getCiclosTrabajoCombo()
        {
            return capacitacionDAO.getCiclosTrabajoCombo();
        }

        public Dictionary<string, object> guardarNuevoCiclo(tblS_CapacitacionDNCicloTrabajo ciclo, List<tblS_CapacitacionDNCicloTrabajoAreas> listaAreas, List<tblS_CapacitacionDNCicloTrabajoCriterio> listaCriterios)
        {
            return capacitacionDAO.guardarNuevoCiclo(ciclo, listaAreas, listaCriterios);
        }

        public Dictionary<string, object> getCicloByID(int cicloID)
        {
            return capacitacionDAO.getCicloByID(cicloID);
        }

        public Dictionary<string, object> guardarRegistroCiclo(tblS_CapacitacionDNCicloTrabajoRegistro registroCiclo, List<tblS_CapacitacionDNCicloTrabajoRegistroRevisiones> listaRevisiones, List<tblS_CapacitacionDNCicloTrabajoRegistroPropuestas> listaPropuestas, List<tblS_CapacitacionDNCicloTrabajoRegistroAreas> listaAreas, List<int> areasSeguimiento, List<int> interesados)
        {
            return capacitacionDAO.guardarRegistroCiclo(registroCiclo, listaRevisiones, listaPropuestas, listaAreas, areasSeguimiento, interesados);
        }

        public Dictionary<string, object> getRegistrosCiclos(FiltrosRegistrosCiclo filtros)
        {
            return capacitacionDAO.getRegistrosCiclos(filtros);
        }

        public Dictionary<string, object> getListaSeguimiento(List<string> listaCC, TipoSeguimientoEnum tipoSeguimiento, DateTime fechaInicio, DateTime fechaFin)
        {
            return capacitacionDAO.getListaSeguimiento(listaCC, tipoSeguimiento, fechaInicio, fechaFin);
        }

        public MemoryStream DescargarExcelSeguimientoAcciones()
        {
            return capacitacionDAO.DescargarExcelSeguimientoAcciones();
        }

        public Dictionary<string, object> guardarSeguimientoAcciones(List<tblS_CapacitacionDNCicloTrabajoRegistro> capturaEvidencias, List<HttpPostedFileBase> evidencias, List<tblS_CapacitacionDNCicloTrabajoRegistro> capturaEvaluaciones)
        {
            return capacitacionDAO.guardarSeguimientoAcciones(capturaEvidencias, evidencias, capturaEvaluaciones);
        }

        public Dictionary<string, object> guardarSeguimientoPropuestas(List<tblS_CapacitacionDNCicloTrabajoRegistroPropuestas> capturaEvidencias, List<HttpPostedFileBase> evidencias, List<tblS_CapacitacionDNCicloTrabajoRegistroPropuestas> capturaEvaluaciones)
        {
            return capacitacionDAO.guardarSeguimientoPropuestas(capturaEvidencias, evidencias, capturaEvaluaciones);
        }

        public Dictionary<string, object> cargarDatosArchivoEvidenciaSeguimientoAcciones(int id)
        {
            return capacitacionDAO.cargarDatosArchivoEvidenciaSeguimientoAcciones(id);
        }

        public Tuple<Stream, string> descargarArchivoEvidenciaAccion(int id)
        {
            return capacitacionDAO.descargarArchivoEvidenciaAccion(id);
        }

        public Dictionary<string, object> cargarDatosArchivoEvidenciaSeguimientoPropuestas(int id)
        {
            return capacitacionDAO.cargarDatosArchivoEvidenciaSeguimientoPropuestas(id);
        }

        public Tuple<Stream, string> descargarArchivoEvidenciaPropuesta(int id)
        {
            return capacitacionDAO.descargarArchivoEvidenciaPropuesta(id);
        }

        public Dictionary<string, object> cargarDashboardCiclos(List<string> listaCC, List<AreaDTO> listaAreas, DateTime fechaInicio, DateTime fechaFin)
        {
            return capacitacionDAO.cargarDashboardCiclos(listaCC, listaAreas, fechaInicio, fechaFin);
        }

        public Dictionary<string, object> getRegistroCicloTrabajoByID(int id)
        {
            return capacitacionDAO.getRegistroCicloTrabajoByID(id);
        }

        public List<CicloTrabajoDTO> GetTablaCicloTrabajo()
        {
            return capacitacionDAO.GetTablaCicloTrabajo();
            
        }

        public List<CicloTrabajoCriterioDTO> GetTablaCriterioTrabajo(int id)
        {
            return capacitacionDAO.GetTablaCriterioTrabajo(id);

        }

        public bool EliminarCicloTrabajo(int id)
        {
            return capacitacionDAO.EliminarCicloTrabajo(id);
        }

        public bool EditarCicloTrabajo(CicloTrabajoDTO parametros,  List<tblS_CapacitacionDNCicloTrabajoCriterio> criterio, List<tblS_CapacitacionDNCicloTrabajoAreas> lstAreass)
        {
            return capacitacionDAO.EditarCicloTrabajo(parametros, criterio, lstAreass);
        }

        public Dictionary<string, object> getListaDepartamientos(int listaAutorizacionID)
        {
            return capacitacionDAO.getListaDepartamientos(listaAutorizacionID);
        }

        public List<tblP_Usuario> llenarCorreos(int _IdUsuario)
        {
            return capacitacionDAO.llenarCorreos(_IdUsuario);
        }

        public Dictionary<string, object> GetAreaSeguimiento()
        {
            return capacitacionDAO.GetAreaSeguimiento();
        }

        public Dictionary<string, object> CargarDatosSeccionSeguimientoAcciones(List<string> listaCC, TipoSeguimientoEnum tipoSeguimiento, DateTime fechaInicio, DateTime fechaFin, List<int> listaAreasSeguimientoHistorial, SeccionSeguimientoAccionesEnum seccion)
        {
            return capacitacionDAO.CargarDatosSeccionSeguimientoAcciones(listaCC, tipoSeguimiento, fechaInicio, fechaFin, listaAreasSeguimientoHistorial, seccion);
        }

        public Dictionary<string, object> EliminarRegistroCicloTrabajo(tblS_CapacitacionDNCicloTrabajoRegistro ciclo)
        {
            return capacitacionDAO.EliminarRegistroCicloTrabajo(ciclo);
        }

        public Dictionary<string, object> CargarCiclosRequeridos()
        {
            return capacitacionDAO.CargarCiclosRequeridos();
        }

        public Dictionary<string, object> GuardarCiclosRequeridos(List<tblS_CapacitacionDNCiclosRequeridos> listaCiclosRequeridos)
        {
            return capacitacionDAO.GuardarCiclosRequeridos(listaCiclosRequeridos);
        }
        #endregion

        #region Detecciones Primarias
        public Dictionary<string, object> getRegistrosDeteccionesPrimarias(List<string> listaCC, List<AreaDTO> listaAreas, DateTime fecha)
        {
            return capacitacionDAO.getRegistrosDeteccionesPrimarias(listaCC, listaAreas, fecha);
        }

        public Dictionary<string, object> guardarDeteccionPrimaria(tblS_CapacitacionDNDeteccionPrimaria deteccionPrimaria, List<tblS_CapacitacionDNDeteccionPrimariaNecesidad> listaNecesidades, List<tblS_CapacitacionDNDeteccionPrimariaAreas> listaAreas)
        {
            return capacitacionDAO.guardarDeteccionPrimaria(deteccionPrimaria, listaNecesidades, listaAreas);
        }

        public NecesidadPrimariaReporteDTO getDeteccionPrimariaReporte(int deteccionPrimariaID)
        {
            return capacitacionDAO.getDeteccionPrimariaReporte(deteccionPrimariaID);
        }
        #endregion

        #region Recorridos
        public Dictionary<string, object> getRegistrosRecorridos(List<string> listaCC, List<AreaDTO> listaAreas, DateTime fechaInicio, DateTime fechaFin, int realizador)
        {
            return capacitacionDAO.getRegistrosRecorridos(listaCC, listaAreas, fechaInicio, fechaFin, realizador);
        }

        public Dictionary<string, object> guardarNuevoRecorrido(tblS_CapacitacionDNRecorrido recorrido, List<RecorridoHallazgoDTO> listaHallazgos, List<tblS_CapacitacionDNRecorridoAreas> listaAreas, List<HttpPostedFileBase> evidencias)
        {
            return capacitacionDAO.guardarNuevoRecorrido(recorrido, listaHallazgos, listaAreas, evidencias);
        }

        public Dictionary<string, object> editarRecorrido(tblS_CapacitacionDNRecorrido recorrido, List<RecorridoHallazgoDTO> listaHallazgos, List<tblS_CapacitacionDNRecorridoAreas> listaAreas, List<HttpPostedFileBase> evidencias)
        {
            return capacitacionDAO.editarRecorrido(recorrido, listaHallazgos, listaAreas, evidencias);
        }

        public Dictionary<string, object> EliminarRegistroRecorrido(int recorrido_id)
        {
            return capacitacionDAO.EliminarRegistroRecorrido(recorrido_id);
        }

        public Dictionary<string, object> getRecorridoByID(int recorridoID)
        {
            return capacitacionDAO.getRecorridoByID(recorridoID);
        }

        public Dictionary<string, object> guardarSeguimientoRecorrido(List<tblS_CapacitacionDNRecorridoHallazgo> listaSeguimiento)
        {
            return capacitacionDAO.guardarSeguimientoRecorrido(listaSeguimiento);
        }

        public Dictionary<string, object> cargarDashboardRecorridos(List<string> listaCC, List<AreaDTO> listaAreas, DateTime fechaInicio, DateTime fechaFin, int realizador)
        {
            return capacitacionDAO.cargarDashboardRecorridos(listaCC, listaAreas, fechaInicio, fechaFin, realizador);
        }

        public RecorridoReporteDTO getRecorridoReporte(int recorridoID)
        {
            return capacitacionDAO.getRecorridoReporte(recorridoID);
        }

        public List<string> enviarCorreoRecorrido(int recorridoID, List<int> usuarios, List<Byte[]> downloadPDF)
        {
            return capacitacionDAO.enviarCorreoRecorrido(recorridoID, usuarios, downloadPDF);
        }

        public bool enviarCorreoi(int recorridoID, List<Byte[]> archivo)
        {
            return capacitacionDAO.enviarCorreoi(recorridoID, archivo);
        }

        public Dictionary<string, object> VerificarCorreosValidos(List<int> listaUsuariosID)
        {
            return capacitacionDAO.VerificarCorreosValidos(listaUsuariosID);
        }

        #region CARGAR EVIDENCIAS EN RECORRIDOS
        public Dictionary<string, object> GuardarEvidenciaRecorrido(List<HttpPostedFileBase> lstArchivos, int FK_Recorrido)
        {
            return capacitacionDAO.GuardarEvidenciaRecorrido(lstArchivos, FK_Recorrido);
        }

        public Dictionary<string, object> GetEvidenciasRecorrido(int FK_Recorrido)
        {
            return capacitacionDAO.GetEvidenciasRecorrido(FK_Recorrido);
        }

        public Dictionary<string, object> VisualizarEvidenciaRecorrido(int FK_Recorrido)
        {
            return capacitacionDAO.VisualizarEvidenciaRecorrido(FK_Recorrido);
        }

        public Dictionary<string, object> EliminarEvidenciaRecorrido(int idArchivo)
        {
            return capacitacionDAO.EliminarEvidenciaRecorrido(idArchivo);
        }
        #endregion
        #endregion
        #endregion

        #region CAMBIOS DE CATEGORIAS
        public List<tblRH_AutorizacionFormatoCambio> getAutorizacion(int idFormato)
        {
            return capacitacionDAO.getAutorizacion(idFormato);
        }
        public List<resultCapacitacionDTO> TablaFormatosPendientes(FiltrosCapacitacionDTO parametros)
        {
            return capacitacionDAO.TablaFormatosPendientes(parametros);
        }
        public Dictionary<string, object> postSubirArchivos(int id, EmpresaEnum empresa, List<HttpPostedFileBase> archivo)
        {
            return capacitacionDAO.postSubirArchivos(id, empresa, archivo);
        }
        public byte[] descargarArchivoCO(int id)
        {
            return capacitacionDAO.descargarArchivoCO(id);
        }
        public string getFileNameCO(int id)
        {
            return capacitacionDAO.getFileNameCO(id);
        }
        public Dictionary<string, object> obtenerArchivoCODescargas(int idFormatoCambio)
        {
            return capacitacionDAO.obtenerArchivoCODescargas(idFormatoCambio);
        }
        public List<tblS_CapacitacionCO_GCArchivos> getArchivosFormatoCambio(int formatoCambioID)
        {
            return capacitacionDAO.getArchivosFormatoCambio(formatoCambioID);
        }
        public Dictionary<string, object> EliminarArchivosFormatoCambio(int formatoCambio_id)
        {
            return capacitacionDAO.EliminarArchivosFormatoCambio(formatoCambio_id);
        }
        #endregion

        #region Competencias Operativas
        public Dictionary<string, object> getPromedioEvaluaciones(List<string> listaCC, List<AreaDTO> listaAreas, DateTime fecha)
        {
            return capacitacionDAO.getPromedioEvaluaciones(listaCC, listaAreas, fecha);
        }
        #endregion

        #region Indicador Hrs-Hombre
        public Dictionary<string, object> getEquiposCombo()
        {
            return capacitacionDAO.getEquiposCombo();
        }

        public Dictionary<string, object> GetEquipoAdiestramientoCombo()
        {
            return capacitacionDAO.GetEquipoAdiestramientoCombo();
        }

        public Dictionary<string, object> cargarHorasAdiestramiento(List<string> listaCC, DateTime fechaInicial, DateTime fechaFinal, List<int> equipos, List<int> equiposGrafica, List<int> actividades)
        {
            return capacitacionDAO.cargarHorasAdiestramiento(listaCC, fechaInicial, fechaFinal, equipos, equiposGrafica, actividades);
        }

        public Dictionary<string, object> cargarHorasAdiestramientoColaborador(List<string> listaCC, DateTime fechaInicial, DateTime fechaFinal, List<int> equipos, List<int> actividades, int colaborador)
        {
            return capacitacionDAO.cargarHorasAdiestramientoColaborador(listaCC, fechaInicial, fechaFinal, equipos, actividades, colaborador);
        }

        public Dictionary<string, object> guardarNuevoColaboradorCapacitacion(tblS_CapacitacionIHHColaboradorCapacitacion colaboradorCapacitacion, List<int> interesados)
        {
            return capacitacionDAO.guardarNuevoColaboradorCapacitacion(colaboradorCapacitacion, interesados);
        }

        public Dictionary<string, object> EditarColaboradorCapacitacion(tblS_CapacitacionIHHColaboradorCapacitacion colaboradorCapacitacion)
        {
            return capacitacionDAO.EditarColaboradorCapacitacion(colaboradorCapacitacion);
        }

        public Dictionary<string, object> EliminarColaboradorCapacitacion(tblS_CapacitacionIHHColaboradorCapacitacion colaboradorCapacitacion)
        {
            return capacitacionDAO.EliminarColaboradorCapacitacion(colaboradorCapacitacion);
        }

        public Dictionary<string, object> getInfoColaboradorCapacitacion(int colaboradorCapacitacionID)
        {
            return capacitacionDAO.getInfoColaboradorCapacitacion(colaboradorCapacitacionID);
        }

        public Dictionary<string, object> GetInfoColaboradorCapacitacionActividad(int colaboradorCapacitacionID)
        {
            return capacitacionDAO.GetInfoColaboradorCapacitacionActividad(colaboradorCapacitacionID);
        }

        public Dictionary<string, object> guardarNuevoControlHoras(List<tblS_CapacitacionIHHControlHoras> listaControlHoras)
        {
            return capacitacionDAO.guardarNuevoControlHoras(listaControlHoras);
        }

        public Dictionary<string, object> GuardarNuevoControlActividad(List<tblS_CapacitacionIHHControlActividad> listaControlActividad)
        {
            return capacitacionDAO.GuardarNuevoControlActividad(listaControlActividad);
        }

        public Dictionary<string, object> guardarLiberados(List<tblS_CapacitacionIHHColaboradorCapacitacion> captura, List<HttpPostedFileBase> archivos)
        {
            return capacitacionDAO.guardarLiberados(captura, archivos);
        }

        public Dictionary<string, object> GuardarLiberacionAdministrador(List<tblS_CapacitacionIHHColaboradorCapacitacion> captura)
        {
            return capacitacionDAO.GuardarLiberacionAdministrador(captura);
        }

        public Dictionary<string, object> cargarDatosArchivoSoporteAdiestramiento(int id)
        {
            return capacitacionDAO.cargarDatosArchivoSoporteAdiestramiento(id);
        }

        public Tuple<Stream, string> descargarArchivoSoporteAdiestramiento(int id)
        {
            return capacitacionDAO.descargarArchivoSoporteAdiestramiento(id);
        }

        public Tuple<Stream, string> descargarGlobal(int id, string gfx1, string gfx2)
        {
            return capacitacionDAO.descargarGlobal(id, gfx1, gfx2);
        }

        public ColaboradorCapacitacionReporteDTO getColaboradorCapacitacionReporte(int colaboradorCapacitacionID)
        {
            return capacitacionDAO.getColaboradorCapacitacionReporte(colaboradorCapacitacionID);
        }

        public Dictionary<string, object> CargarInformacionHorasHombre(HorasHombreDTO filtros)
        {
            return capacitacionDAO.CargarInformacionHorasHombre(filtros);
        }

        public List<HorasHombreDTO> postObtenerTablaHorasHombre(HorasHombreDTO parametros, bool ActivarHeader)
        {
            return capacitacionDAO.postObtenerTablaHorasHombre(parametros, ActivarHeader);
        }
        public HorasHombreDTO obtenerInputPromedios(HorasHombreDTO parametros)
        {
            return capacitacionDAO.obtenerInputPromedios(parametros);
        }
        
        public MemoryStream crearExcelHorasHombreCapacitacion(HorasHombreDTO parametros)
        {
            return capacitacionDAO.crearExcelHorasHombreCapacitacion(parametros);
        }

        public Dictionary<string, object> GetActividades()
        {
            return capacitacionDAO.GetActividades();
        }

        public Dictionary<string, object> GetInteresados()
        {
            return capacitacionDAO.GetInteresados();
        }

        public Dictionary<string, object> GetMtto(int colaboradorCapacitacionId)
        {
            return capacitacionDAO.GetMtto(colaboradorCapacitacionId);
        }

        public Dictionary<string, object> GetMttoDetalle(int clave_empleado)
        {
            return capacitacionDAO.GetMttoDetalle(clave_empleado);
        }

        public MemoryStream DescargarExcelAdiestramientoActividades()
        {
            return capacitacionDAO.DescargarExcelAdiestramientoActividades();
        }
        public MemoryStream DescargarExcelAdiestramientoHoras()
        {
            return capacitacionDAO.DescargarExcelAdiestramientoHoras();
        }
        #endregion
        public Dictionary<string, object> getEmpleadoPorClave(int claveEmpleado)
        {
            return capacitacionDAO.getEmpleadoPorClave(claveEmpleado);
        }
        #region ADMINISTRACIONINSTRUCTORES
        public Dictionary<string, object> GetInstructores()
        {
            return capacitacionDAO.GetInstructores();
        }
        public List<ComboDTO> GetRolesCombo()
        {
            return capacitacionDAO.GetRolesCombo();
        }
        public Dictionary<string, object> PostGuardarInstructor(tblS_Capacitacion_PCAdministracionInstructores parametros, bool AddEdit)
        {
            return capacitacionDAO.PostGuardarInstructor(parametros, AddEdit);
        }
        public List<ComboDTO> GetInstructoresCombo(string cc)
        {
            return capacitacionDAO.GetInstructoresCombo(cc);
        }
        public AdministracionInstructoresDTO getFechaInicio(string cveEmpleado)
        {
            return capacitacionDAO.getFechaInicio(cveEmpleado);
        }

        public List<ComboDTO> GetCC()
        {
            return capacitacionDAO.GetCC();
        }
        public bool EliminarInstructor(int id)
        {
            return capacitacionDAO.EliminarInstructor(id);
        }
        public AdministracionInstructoresDTO ObtenerCCUnico(string cveEmpleado){
            return capacitacionDAO.ObtenerCCUnico(cveEmpleado);
        }
       
        #endregion

        #region Plan de Capacitación
        public Dictionary<string, object> cargarCalendarioPlanCapacitacion(string cc, List<TematicaCursoEnum> listaTematicas, int empresa, DateTime mesCalendario)
        {
            return capacitacionDAO.cargarCalendarioPlanCapacitacion(cc, listaTematicas, empresa, mesCalendario);
        }
        #endregion

        public Dictionary<string, object> GetEmpleadoCursosActos(int claveEmpleado)
        {
            return capacitacionDAO.GetEmpleadoCursosActos(claveEmpleado);
        }

        #region Factor Capacitación
        public Dictionary<string, object> CargarFactorCapacitacion(int division, List<string> listaCentroCosto, DateTime fechaInicial, DateTime fechaFinal)
        {
            return capacitacionDAO.CargarFactorCapacitacion(division, listaCentroCosto, fechaInicial, fechaFinal);
        }

        public Dictionary<string, object> CargarFactorCapacitacionDetalle(int division, List<string> listaCentroCosto, DateTime fechaInicial, DateTime fechaFinal)
        {
            return capacitacionDAO.CargarFactorCapacitacionDetalle(division, listaCentroCosto, fechaInicial, fechaFinal);
        }

        public Dictionary<string, object> CargarEstadisticas(int division, List<string> listaCentroCosto, int anio, SeccionEstadisticaEnum seccion)
        {
            return capacitacionDAO.CargarEstadisticas(division, listaCentroCosto, anio, seccion);
        }

        public Dictionary<string, object> CargarEfectividadCiclosDetalle(int division, List<string> listaCentroCosto, int anio)
        {
            return capacitacionDAO.CargarEfectividadCiclosDetalle(division, listaCentroCosto, anio);
        }

        public Dictionary<string, object> GetCentrosCostoDivision(int division)
        {
            return capacitacionDAO.GetCentrosCostoDivision(division);
        }
        public Dictionary<string, object> fillComboDivision()
        {
            return capacitacionDAO.fillComboDivision();
        }
        public Dictionary<string, object> FillAnios()
        {
            return capacitacionDAO.FillAnios();
        }

        #endregion

        #region CATALOGO FIRMAS INSTRUCTORES
        public Dictionary<string, object> GetFirmasInstructores()
        {
            return capacitacionDAO.GetFirmasInstructores();
        }

        public Dictionary<string, object> CEFirmaInstructor(FirmaInstructorDTO objParamDTO, List<HttpPostedFileBase> lstArchivos)
        {
            return capacitacionDAO.CEFirmaInstructor(objParamDTO, lstArchivos);
        }

        public Dictionary<string, object> EliminarFirmaInstructor(FirmaInstructorDTO objParamDTO)
        {
            return capacitacionDAO.EliminarFirmaInstructor(objParamDTO);
        }

        public Dictionary<string, object> FillCboUsuarios()
        {
            return capacitacionDAO.FillCboUsuarios();
        }

        public ReporteCertificadoTrabajoDTO GetInformacionCertificadoTrabajo(ReporteCertificadoTrabajoDTO objParamDTO)
        {
            return capacitacionDAO.GetInformacionCertificadoTrabajo(objParamDTO);
        }
        #endregion

        public bool AccesoPermitidoPrivilegioDivision(int division)
        {
            return capacitacionDAO.AccesoPermitidoPrivilegioDivision(division);
        }
    }
}
