using Core.DAO.Administracion.Seguridad;
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
using System.Web;

namespace Core.Service.Administracion.Seguridad
{
    public class CapacitacionSeguridadService : ICapacitacionSeguridadDAO
    {
        public ICapacitacionSeguridadDAO capacitacionSeguridadDAO { get; set; }

        public CapacitacionSeguridadService(ICapacitacionSeguridadDAO capacitacionSeguridadDAO)
        {
            this.capacitacionSeguridadDAO = capacitacionSeguridadDAO;
        }

        #region Cursos
        public Dictionary<string, object> getCursos(List<int> clasificaciones, List<int> puestos, int estatus)
        {
            return capacitacionSeguridadDAO.getCursos(clasificaciones, puestos, estatus);
        }
        public Dictionary<string, object> getCursoById(int id)
        {
            return capacitacionSeguridadDAO.getCursoById(id);
        }
        public Dictionary<string, object> getExamenesCursoById(int id)
        {
            return capacitacionSeguridadDAO.getExamenesCursoById(id);
        }
        public byte[] descargarArchivo(long examen_id)
        {
            return capacitacionSeguridadDAO.descargarArchivo(examen_id);
        }
        public string getFileName(long examen_id)
        {
            return capacitacionSeguridadDAO.getFileName(examen_id);
        }
        public Dictionary<string, object> getTipoExamen(int curso_id)
        {
            return capacitacionSeguridadDAO.getTipoExamen(curso_id);
        }
        public Dictionary<string, object> getPuestosEnkontrol()
        {
            return capacitacionSeguridadDAO.getPuestosEnkontrol();
        }
        public Dictionary<string, object> getClasificacionCursos()
        {
            return capacitacionSeguridadDAO.getClasificacionCursos();
        }
        public Dictionary<string, object> guardarCurso(tblS_CapacitacionSeguridadCursos curso)
        {
            return capacitacionSeguridadDAO.guardarCurso(curso);
        }
        public Dictionary<string, object> guardarExamenes(List<tblS_CapacitacionSeguridadCursosExamen> examenes, List<HttpPostedFileBase> archivos)
        {
            return capacitacionSeguridadDAO.guardarExamenes(examenes, archivos);
        }
        public Dictionary<string, object> actualizarCurso(tblS_CapacitacionSeguridadCursos curso, List<tblS_CapacitacionSeguridadCursosMando> mandos, List<tblS_CapacitacionSeguridadCursosPuestos> puestosNuevos, List<tblS_CapacitacionSeguridadCursosPuestosAutorizacion> puestosAutorizacionNuevos, List<tblS_CapacitacionSeguridadCursosCC> centrosCosto)
        {
            return capacitacionSeguridadDAO.actualizarCurso(curso, mandos, puestosNuevos, puestosAutorizacionNuevos, centrosCosto);
        }
        public Dictionary<string, object> eliminarExamen(int examen_id)
        {
            return capacitacionSeguridadDAO.eliminarExamen(examen_id);
        }

        public Dictionary<string, object> GetEstatusCursos()
        {
            return capacitacionSeguridadDAO.GetEstatusCursos();
        }

        public Dictionary<string, object> EliminarCurso(int cursoID)
        {
            return capacitacionSeguridadDAO.EliminarCurso(cursoID);
        }

        public Dictionary<string, object> getPuestosEnkontrolMandos(List<int> mandos)
        {
            return capacitacionSeguridadDAO.getPuestosEnkontrolMandos(mandos);
        }
        #endregion

        #region Control de Asistencia
        public Dictionary<string, object> ObtenerComboCC()
        {
            return capacitacionSeguridadDAO.ObtenerComboCC();
        }

        public Dictionary<string, object> ObtenerListaControlesAsistencia(string cc, int estado, DateTime fechaInicio, DateTime fechaFin)
        {
            return capacitacionSeguridadDAO.ObtenerListaControlesAsistencia(cc, estado, fechaInicio, fechaFin);
        }

        public object GetCursosAutocomplete(string term, bool porClave)
        {
            return capacitacionSeguridadDAO.GetCursosAutocomplete(term, porClave);
        }

        public object GetUsuariosAutocomplete(string term, bool porClave)
        {
            return capacitacionSeguridadDAO.GetUsuariosAutocomplete(term, porClave);
        }

        public object GetLugarCursoAutocomplete(string term)
        {
            return capacitacionSeguridadDAO.GetLugarCursoAutocomplete(term);
        }

        public object GetEmpleadoEnKontrolAutocomplete(string term, bool porClave)
        {
            return capacitacionSeguridadDAO.GetEmpleadoEnKontrolAutocomplete(term, porClave);
        }

        public Dictionary<string, object> CrearControlAsistencia(tblS_CapacitacionSeguridadControlAsistencia controlAsistencia)
        {
            return capacitacionSeguridadDAO.CrearControlAsistencia(controlAsistencia);
        }

        public Dictionary<string, object> SubirArchivoControlAsistencia(HttpPostedFileBase archivo, int controlAsistenciaID)
        {
            return capacitacionSeguridadDAO.SubirArchivoControlAsistencia(archivo, controlAsistenciaID);
        }

        public Tuple<Stream, string> DescargarListaControlAsistencia(int controlAsistenciaID)
        {
            return capacitacionSeguridadDAO.DescargarListaControlAsistencia(controlAsistenciaID);
        }

        public Tuple<Stream, string> DescargarFormatoAutorizacion(int controlAsistenciaID)
        {
            return capacitacionSeguridadDAO.DescargarFormatoAutorizacion(controlAsistenciaID);
        }

        public Dictionary<string, object> CargarDatosControlAsistencia(int controlAsistenciaID)
        {
            return capacitacionSeguridadDAO.CargarDatosControlAsistencia(controlAsistenciaID);
        }

        public Dictionary<string, object> CargarAsistentesCapacitacion(int controlAsistenciaID)
        {
            return capacitacionSeguridadDAO.CargarAsistentesCapacitacion(controlAsistenciaID);
        }

        public Dictionary<string, object> GuardarExamenesAsistentes(List<ExamenesAsistenteDTO> listaExamenesAsistentes, int jefeID, int coordinadorID, int secretarioID, int gerenteID, string rfc, string razonSocial)
        {
            return capacitacionSeguridadDAO.GuardarExamenesAsistentes(listaExamenesAsistentes, jefeID, coordinadorID, secretarioID, gerenteID, rfc, razonSocial);
        }

        public Tuple<Stream, string> DescargarExamenAsistente(int controlAsistenciaDetalleID, int tipoExamen)
        {
            return capacitacionSeguridadDAO.DescargarExamenAsistente(controlAsistenciaDetalleID, tipoExamen);
        }

        public ControlAsistenciaDTO ObtenerDatosControlAsistenciaReporte(int controlAsistenciaID)
        {
            return capacitacionSeguridadDAO.ObtenerDatosControlAsistenciaReporte(controlAsistenciaID);
        }

        public Dictionary<string, object> GuardarEvaluacionAsistentes(List<AsistenteCursoDTO> listaAsistentes)
        {
            return capacitacionSeguridadDAO.GuardarEvaluacionAsistentes(listaAsistentes);
        }

        public Dictionary<string, object> EliminarControlAsistencia(int controlAsistenciaID)
        {
            return capacitacionSeguridadDAO.EliminarControlAsistencia(controlAsistenciaID);
        }

        public Dictionary<string, object> guardarArchivosDC3(HttpPostedFileBase archivoDC3, int controlAsistenciaDetalleID)
        {
            return capacitacionSeguridadDAO.guardarArchivosDC3(archivoDC3, controlAsistenciaDetalleID);
        }

        public Tuple<Stream, string> DescargarDC3(int controlAsistenciaDetalleID)
        {
            return capacitacionSeguridadDAO.DescargarDC3(controlAsistenciaDetalleID);
        }
        #endregion

        #region Autorización Capacitación
        public Dictionary<string, object> ObtenerComboEstatusAutorizacionCapacitacion()
        {
            return capacitacionSeguridadDAO.ObtenerComboEstatusAutorizacionCapacitacion();
        }

        public Dictionary<string, object> ObtenerAutorizaciones(string cc, int curso, int estatus)
        {
            return capacitacionSeguridadDAO.ObtenerAutorizaciones(cc, curso, estatus);
        }

        public Dictionary<string, object> ObtenerAutorizantes(int capacitacionID)
        {
            return capacitacionSeguridadDAO.ObtenerAutorizantes(capacitacionID);
        }

        public FormatoAutorizacionDTO ObtenerDatosFormatoAutorizacion(int controlAsistenciaID)
        {
            return capacitacionSeguridadDAO.ObtenerDatosFormatoAutorizacion(controlAsistenciaID);
        }

        public Dictionary<string, object> AutorizarControlAsistencia(int controlAsistenciaID)
        {
            return capacitacionSeguridadDAO.AutorizarControlAsistencia(controlAsistenciaID);
        }

        public Dictionary<string, object> RechazarControlAsistencia(int controlAsistenciaID, string comentario)
        {
            return capacitacionSeguridadDAO.RechazarControlAsistencia(controlAsistenciaID, comentario);
        }

        public Dictionary<string, object> EnviarCorreoRechazo(int controlAsistenciaID, List<byte[]> pdf)
        {
            return capacitacionSeguridadDAO.EnviarCorreoRechazo(controlAsistenciaID, pdf);
        }

        public Dictionary<string, object> EnviarCorreoAutorizacion(int controlAsistenciaID, List<byte[]> pdf)
        {
            return capacitacionSeguridadDAO.EnviarCorreoAutorizacion(controlAsistenciaID, pdf);
        }

        public Dictionary<string, object> EnviarCorreoAutorizacionCompleta(int controlAsistenciaID, List<byte[]> pdf)
        {
            return capacitacionSeguridadDAO.EnviarCorreoAutorizacionCompleta(controlAsistenciaID, pdf);
        }

        public Dictionary<string, object> guardarCargaMasiva(HttpPostedFileBase archivo)
        {
            return capacitacionSeguridadDAO.guardarCargaMasiva(archivo);
        }
        #endregion

        #region Matriz de empleados

        public Dictionary<string, object> ObtenerComboCCEnKontrol(EmpresaEnum empresa)
        {
            return capacitacionSeguridadDAO.ObtenerComboCCEnKontrol(empresa);
        }

        public List<EmpleadoPuestoDTO> ObtenerEmpleados(List<string> ccsCplan, List<string> ccsArr, List<string> puestos)
        {
            return capacitacionSeguridadDAO.ObtenerEmpleados(ccsCplan, ccsArr, puestos);
        }

        public Dictionary<string, object> ObtenerCursosEmpleado(int claveEmpleado, int puestoID)
        {
            return capacitacionSeguridadDAO.ObtenerCursosEmpleado(claveEmpleado, puestoID);
        }

        public Tuple<Stream, string> DescargarExpedienteEmpleado(int claveEmpleado, byte[] licencia)
        {
            return capacitacionSeguridadDAO.DescargarExpedienteEmpleado(claveEmpleado, licencia);
        }

        public LicenciaHabilidadesDTO ObtenerLicenciaEmpleado(int claveEmpleado, int empresa)
        {
            return capacitacionSeguridadDAO.ObtenerLicenciaEmpleado(claveEmpleado, empresa);
        }

        public Dictionary<string, object> ObtenerExtracurricularesEmpleado(int claveEmpleado)
        {
            return capacitacionSeguridadDAO.ObtenerExtracurricularesEmpleado(claveEmpleado);
        }

        public Dictionary<string, object> SubirEvidenciaExtracurricular(int claveEmpleado, string nombre, decimal duracion, string fecha, string fechaFin, HttpPostedFileBase evidencia)
        {
            return capacitacionSeguridadDAO.SubirEvidenciaExtracurricular(claveEmpleado, nombre, duracion, fecha, fechaFin, evidencia);
        }

        public Tuple<Stream, string> DescargarEvidenciaExtracurricular(int extracurricularID)
        {
            return capacitacionSeguridadDAO.DescargarEvidenciaExtracurricular(extracurricularID);
        }

        public Dictionary<string, object> EliminarEvidenciaExtracurricular(int extracurricularID)
        {
            return capacitacionSeguridadDAO.EliminarEvidenciaExtracurricular(extracurricularID);
        }
        #endregion

        #region Dashboard
        public Dictionary<string, object> CargarDatosGeneralesDashboard(List<string> ccsCplan, List<string> ccsArr, DateTime FechaInicio, DateTime fechaFin, List<string> clasificacion)
        {
            return capacitacionSeguridadDAO.CargarDatosGeneralesDashboard(ccsCplan, ccsArr, FechaInicio, fechaFin, clasificacion);
        }
        #endregion

        #region Matriz de Necesidades
        public Dictionary<string, object> ObtenerAreasPorCC(List<string> ccsCplan, List<string> ccsArr)
        {
            return capacitacionSeguridadDAO.ObtenerAreasPorCC(ccsCplan, ccsArr);
        }

        public Dictionary<string, object> CargarDatosMatrizNecesidades(List<string> ccsCplan, List<string> ccsArr, List<string> departamentosIDs, List<ClasificacionCursoEnum> clasificaciones)
        {
            return capacitacionSeguridadDAO.CargarDatosMatrizNecesidades(ccsCplan, ccsArr, departamentosIDs, clasificaciones);
        }
        public Dictionary<string, object> CargarDatosSeccionMatriz(List<string> ccsCplan, List<string> ccsArr, List<string> departamentosIDs, List<ClasificacionCursoEnum> clasificaciones, SeccionMatrizEnum seccion)
        {
            return capacitacionSeguridadDAO.CargarDatosSeccionMatriz(ccsCplan, ccsArr, departamentosIDs, clasificaciones, seccion);
        }
        public Tuple<MemoryStream, string> DescargarExcelPersonalActivo()
        {
            return capacitacionSeguridadDAO.DescargarExcelPersonalActivo();
        }

        #endregion

        #region Privilegios
        public tblS_CapacitacionSeguridadEmpleadoPrivilegio getPrivilegioActual()
        {
            return capacitacionSeguridadDAO.getPrivilegioActual();
        }
        public Dictionary<string, object> ObtenerEmpleadosPrivilegios(BusqPrivilegiosDTO busq)
        {
            return capacitacionSeguridadDAO.ObtenerEmpleadosPrivilegios(busq);
        }
        public Dictionary<string, object> guardarEmpleadosPrivilegios(List<tblS_CapacitacionSeguridadEmpleadoPrivilegio> lst)
        {
            return capacitacionSeguridadDAO.guardarEmpleadosPrivilegios(lst);
        }

        #endregion

        #region Catálogos
        public Dictionary<string, object> getRelacionesCCAutorizantes()
        {
            return capacitacionSeguridadDAO.getRelacionesCCAutorizantes();
        }

        public Dictionary<string, object> guardarNuevaRelacionCCAutorizante(RelacionCCAutorizanteDTO relacion)
        {
            return capacitacionSeguridadDAO.guardarNuevaRelacionCCAutorizante(relacion);
        }

        public Dictionary<string, object> editarRelacionCCAutorizante(RelacionCCAutorizanteDTO relacion)
        {
            return capacitacionSeguridadDAO.editarRelacionCCAutorizante(relacion);
        }

        public Dictionary<string, object> eliminarRelacionCCAutorizante(RelacionCCAutorizanteDTO relacion)
        {
            return capacitacionSeguridadDAO.eliminarRelacionCCAutorizante(relacion);
        }

        public Dictionary<string, object> getUsuarioPorClave(int claveEmpleado)
        {
            return capacitacionSeguridadDAO.getUsuarioPorClave(claveEmpleado);
        }

        public Dictionary<string, object> getRelacionesCCDepartamentoRazonSocial()
        {
            return capacitacionSeguridadDAO.getRelacionesCCDepartamentoRazonSocial();
        }

        public Dictionary<string, object> guardarNuevaRelacionCCDepartamentoRazonSocial(List<RelacionCCDepartamentoRazonSocialDTO> relaciones)
        {
            return capacitacionSeguridadDAO.guardarNuevaRelacionCCDepartamentoRazonSocial(relaciones);
        }

        public Dictionary<string, object> editarRelacionCCDepartamentoRazonSocial(RelacionCCDepartamentoRazonSocialDTO relacion)
        {
            return capacitacionSeguridadDAO.editarRelacionCCDepartamentoRazonSocial(relacion);
        }

        public Dictionary<string, object> eliminarRelacionCCDepartamentoRazonSocial(RelacionCCDepartamentoRazonSocialDTO relacion)
        {
            return capacitacionSeguridadDAO.eliminarRelacionCCDepartamentoRazonSocial(relacion);
        }

        public Dictionary<string, object> getDepartamentosCombo()
        {
            return capacitacionSeguridadDAO.getDepartamentosCombo();
        }
        #endregion

        public List<ComboDTO> getRazonSocialCombo()
        {
            return capacitacionSeguridadDAO.getRazonSocialCombo();
        }

        public MemoryStream crearExcelRelacionCursosPuestos()
        {
            return capacitacionSeguridadDAO.crearExcelRelacionCursosPuestos();
        }

        public List<ComboDTO> obtenerComboCursos()
        {
            return capacitacionSeguridadDAO.obtenerComboCursos();
        }

        public Dictionary<string, object> guardarCargaMasivaControlAsistencia(HttpPostedFileBase controlAsistencia)
        {
            return capacitacionSeguridadDAO.guardarCargaMasivaControlAsistencia(controlAsistencia);
        }

        public Dictionary<string, object> guardarCargaMasivaRelacionCursosPuestosAutorizacion(HttpPostedFileBase archivo)
        {
            return capacitacionSeguridadDAO.guardarCargaMasivaRelacionCursosPuestosAutorizacion(archivo);
        }

        public Dictionary<string, object> ObtenerComboCCAmbasEmpresas()
        {
            return capacitacionSeguridadDAO.ObtenerComboCCAmbasEmpresas();
        }

        #region Personal Autorizado
        public Dictionary<string, object> getListasAutorizacion(List<int> listaCursos, List<tblS_CapacitacionSeguridadListaAutorizacionCC> listaCC)
        {
            return capacitacionSeguridadDAO.getListasAutorizacion(listaCursos, listaCC);
        }

        public Dictionary<string, object> getListasAutorizacionCombo()
        {
            return capacitacionSeguridadDAO.getListasAutorizacionCombo();
        }

        public Dictionary<string, object> guardarListaAutorizacion(tblS_CapacitacionSeguridadListaAutorizacion listaAutorizacion, List<tblS_CapacitacionSeguridadListaAutorizacionRFC> listaRFC, List<tblS_CapacitacionSeguridadListaAutorizacionCC> listaCentrosCosto)
        {
            return capacitacionSeguridadDAO.guardarListaAutorizacion(listaAutorizacion, listaRFC, listaCentrosCosto);
        }

        public Dictionary<string, object> editarListaAutorizacion(tblS_CapacitacionSeguridadListaAutorizacion listaAutorizacion, List<tblS_CapacitacionSeguridadListaAutorizacionRFC> listaRFC, List<tblS_CapacitacionSeguridadListaAutorizacionCC> listaCentrosCosto)
        {
            return capacitacionSeguridadDAO.editarListaAutorizacion(listaAutorizacion, listaRFC, listaCentrosCosto);
        }

        public Dictionary<string, object> eliminarListaAutorizacion(int listaAutorizacionID)
        {
            return capacitacionSeguridadDAO.eliminarListaAutorizacion(listaAutorizacionID);
        }

        public Dictionary<string, object> getListaAutorizacionByID(int listaAutorizacionID)
        {
            return capacitacionSeguridadDAO.getListaAutorizacionByID(listaAutorizacionID);
        }

        public object getAutorizanteEnkontrolAutocomplete(string term)
        {
            return capacitacionSeguridadDAO.getAutorizanteEnkontrolAutocomplete(term);
        }

        public Dictionary<string, object> guardarInformacionAutorizados(ListaAutorizacionDTO listaAutorizacion)
        {
            return capacitacionSeguridadDAO.guardarInformacionAutorizados(listaAutorizacion);
        }

        public ListaAutorizacionReporteDTO getListaAutorizacionReporte(int listaAutorizacionID, int razonSocialID, int departamento, string cc, int empresa)
        {
            return capacitacionSeguridadDAO.getListaAutorizacionReporte(listaAutorizacionID, razonSocialID, departamento, cc, empresa);
        }

        public Dictionary<string, object> cargarDashboardPersonalAutorizado(FiltrosDashboardPersonalAutorizadoDTO filtros)
        {
            return capacitacionSeguridadDAO.cargarDashboardPersonalAutorizado(filtros);
        }

        public Dictionary<string, object> getCorreosListaAutorizacion(int listaAutorizacionID)
        {
            return capacitacionSeguridadDAO.getCorreosListaAutorizacion(listaAutorizacionID);
        }

        public bool enviarCorreoListaAutorizacion(int listaAutorizacionID, List<Byte[]> archivoListaAutorizacion, List<string> listaCorreos)
        {
            return capacitacionSeguridadDAO.enviarCorreoListaAutorizacion(listaAutorizacionID, archivoListaAutorizacion, listaCorreos);
        }
        #endregion

        #region Detección de Necesidades
        #region Ciclos de Trabajo
        public List<ComboDTO> getCiclosTrabajoCombo()
        {
            return capacitacionSeguridadDAO.getCiclosTrabajoCombo();
        }

        public Dictionary<string, object> guardarNuevoCiclo(tblS_CapacitacionSeguridadDNCicloTrabajo ciclo, List<tblS_CapacitacionSeguridadDNCicloTrabajoAreas> listaAreas, List<tblS_CapacitacionSeguridadDNCicloTrabajoCriterio> listaCriterios)
        {
            return capacitacionSeguridadDAO.guardarNuevoCiclo(ciclo, listaAreas, listaCriterios);
        }

        public Dictionary<string, object> getCicloByID(int cicloID)
        {
            return capacitacionSeguridadDAO.getCicloByID(cicloID);
        }

        public Dictionary<string, object> guardarRegistroCiclo(tblS_CapacitacionSeguridadDNCicloTrabajoRegistro registroCiclo, List<tblS_CapacitacionSeguridadDNCicloTrabajoRegistroRevisiones> listaRevisiones, List<tblS_CapacitacionSeguridadDNCicloTrabajoRegistroPropuestas> listaPropuestas, List<tblS_CapacitacionSeguridadDNCicloTrabajoRegistroAreas> listaAreas)
        {
            return capacitacionSeguridadDAO.guardarRegistroCiclo(registroCiclo, listaRevisiones, listaPropuestas, listaAreas);
        }

        public Dictionary<string, object> getRegistrosCiclos(FiltrosRegistrosCiclo filtros)
        {
            return capacitacionSeguridadDAO.getRegistrosCiclos(filtros);
        }

        public Dictionary<string, object> getListaSeguimiento(List<string> listaCC, TipoSeguimientoEnum tipoSeguimiento, DateTime fechaInicio, DateTime fechaFin)
        {
            return capacitacionSeguridadDAO.getListaSeguimiento(listaCC, tipoSeguimiento, fechaInicio, fechaFin);
        }

        public Dictionary<string, object> guardarSeguimientoAcciones(List<tblS_CapacitacionSeguridadDNCicloTrabajoRegistro> capturaEvidencias, List<HttpPostedFileBase> evidencias, List<tblS_CapacitacionSeguridadDNCicloTrabajoRegistro> capturaEvaluaciones)
        {
            return capacitacionSeguridadDAO.guardarSeguimientoAcciones(capturaEvidencias, evidencias, capturaEvaluaciones);
        }

        public Dictionary<string, object> guardarSeguimientoPropuestas(List<tblS_CapacitacionSeguridadDNCicloTrabajoRegistroPropuestas> capturaEvidencias, List<HttpPostedFileBase> evidencias, List<tblS_CapacitacionSeguridadDNCicloTrabajoRegistroPropuestas> capturaEvaluaciones)
        {
            return capacitacionSeguridadDAO.guardarSeguimientoPropuestas(capturaEvidencias, evidencias, capturaEvaluaciones);
        }

        public Dictionary<string, object> cargarDatosArchivoEvidenciaSeguimientoAcciones(int id)
        {
            return capacitacionSeguridadDAO.cargarDatosArchivoEvidenciaSeguimientoAcciones(id);
        }

        public Tuple<Stream, string> descargarArchivoEvidenciaAccion(int id)
        {
            return capacitacionSeguridadDAO.descargarArchivoEvidenciaAccion(id);
        }

        public Dictionary<string, object> cargarDatosArchivoEvidenciaSeguimientoPropuestas(int id)
        {
            return capacitacionSeguridadDAO.cargarDatosArchivoEvidenciaSeguimientoPropuestas(id);
        }

        public Tuple<Stream, string> descargarArchivoEvidenciaPropuesta(int id)
        {
            return capacitacionSeguridadDAO.descargarArchivoEvidenciaPropuesta(id);
        }

        public Dictionary<string, object> cargarDashboardCiclos(List<string> listaCC, List<AreaDTO> listaAreas, DateTime fechaInicio, DateTime fechaFin)
        {
            return capacitacionSeguridadDAO.cargarDashboardCiclos(listaCC, listaAreas, fechaInicio, fechaFin);
        }

        public Dictionary<string, object> getRegistroCicloTrabajoByID(int id)
        {
            return capacitacionSeguridadDAO.getRegistroCicloTrabajoByID(id);
        }

        public List<CicloTrabajoDTO> GetTablaCicloTrabajo()
        {
            return capacitacionSeguridadDAO.GetTablaCicloTrabajo();
            
        }

        public List<CicloTrabajoCriterioDTO> GetTablaCriterioTrabajo(int id)
        {
            return capacitacionSeguridadDAO.GetTablaCriterioTrabajo(id);

        }

        public bool EliminarCicloTrabajo(int id)
        {
            return capacitacionSeguridadDAO.EliminarCicloTrabajo(id);
        }

        public bool EditarCicloTrabajo(CicloTrabajoDTO parametros,  List<tblS_CapacitacionSeguridadDNCicloTrabajoCriterio> criterio, List<tblS_CapacitacionSeguridadDNCicloTrabajoAreas> lstAreass)
        {
            return capacitacionSeguridadDAO.EditarCicloTrabajo(parametros, criterio, lstAreass);
        }

        public Dictionary<string, object> getListaDepartamientos(int listaAutorizacionID)
        {
            return capacitacionSeguridadDAO.getListaDepartamientos(listaAutorizacionID);
        }

        public List<tblP_Usuario> llenarCorreos(int _IdUsuario)
        {
            return capacitacionSeguridadDAO.llenarCorreos(_IdUsuario);
        }
        #endregion

        #region Detecciones Primarias
        public Dictionary<string, object> getRegistrosDeteccionesPrimarias(List<string> listaCC, List<AreaDTO> listaAreas, DateTime fecha)
        {
            return capacitacionSeguridadDAO.getRegistrosDeteccionesPrimarias(listaCC, listaAreas, fecha);
        }

        public Dictionary<string, object> guardarDeteccionPrimaria(tblS_CapacitacionSeguridadDNDeteccionPrimaria deteccionPrimaria, List<tblS_CapacitacionSeguridadDNDeteccionPrimariaNecesidad> listaNecesidades, List<tblS_CapacitacionSeguridadDNDeteccionPrimariaAreas> listaAreas)
        {
            return capacitacionSeguridadDAO.guardarDeteccionPrimaria(deteccionPrimaria, listaNecesidades, listaAreas);
        }

        public NecesidadPrimariaReporteDTO getDeteccionPrimariaReporte(int deteccionPrimariaID)
        {
            return capacitacionSeguridadDAO.getDeteccionPrimariaReporte(deteccionPrimariaID);
        }
        #endregion

        #region Recorridos
        public Dictionary<string, object> getRegistrosRecorridos(List<string> listaCC, List<AreaDTO> listaAreas, DateTime fecha, int realizador)
        {
            return capacitacionSeguridadDAO.getRegistrosRecorridos(listaCC, listaAreas, fecha, realizador);
        }

        public Dictionary<string, object> guardarNuevoRecorrido(tblS_CapacitacionSeguridadDNRecorrido recorrido, List<RecorridoHallazgoDTO> listaHallazgos, List<tblS_CapacitacionSeguridadDNRecorridoAreas> listaAreas, List<HttpPostedFileBase> evidencias)
        {
            return capacitacionSeguridadDAO.guardarNuevoRecorrido(recorrido, listaHallazgos, listaAreas, evidencias);
        }

        public Dictionary<string, object> editarRecorrido(tblS_CapacitacionSeguridadDNRecorrido recorrido, List<RecorridoHallazgoDTO> listaHallazgos, List<tblS_CapacitacionSeguridadDNRecorridoAreas> listaAreas, List<HttpPostedFileBase> evidencias)
        {
            return capacitacionSeguridadDAO.editarRecorrido(recorrido, listaHallazgos, listaAreas, evidencias);
        }

        public Dictionary<string, object> getRecorridoByID(int recorridoID)
        {
            return capacitacionSeguridadDAO.getRecorridoByID(recorridoID);
        }

        public Dictionary<string, object> guardarSeguimientoRecorrido(List<tblS_CapacitacionSeguridadDNRecorridoHallazgo> listaSeguimiento)
        {
            return capacitacionSeguridadDAO.guardarSeguimientoRecorrido(listaSeguimiento);
        }

        public Dictionary<string, object> cargarDashboardRecorridos(List<string> listaCC, List<AreaDTO> listaAreas, DateTime fecha, int realizador)
        {
            return capacitacionSeguridadDAO.cargarDashboardRecorridos(listaCC, listaAreas, fecha, realizador);
        }

        public RecorridoReporteDTO getRecorridoReporte(int recorridoID)
        {
            return capacitacionSeguridadDAO.getRecorridoReporte(recorridoID);
        }

        public List<string> enviarCorreoRecorrido(int recorridoID, List<int> usuarios, List<Byte[]> downloadPDF)
        {
            return capacitacionSeguridadDAO.enviarCorreoRecorrido(recorridoID, usuarios, downloadPDF);
        }

        public bool enviarCorreoi(int recorridoID, List<Byte[]> archivo)
        {
            return capacitacionSeguridadDAO.enviarCorreoi(recorridoID, archivo);
        }
        #endregion
        #endregion

        #region CAMBIOS DE CATEGORIAS
        public List<tblRH_AutorizacionFormatoCambio> getAutorizacion(int idFormato)
        {
            return capacitacionSeguridadDAO.getAutorizacion(idFormato);
        }
        public List<resultCapacitacionDTO> TablaFormatosPendientes(FiltrosCapacitacionDTO parametros)
        {
            return capacitacionSeguridadDAO.TablaFormatosPendientes(parametros);
        }
        public Dictionary<string, object> postSubirArchivos(int id, EmpresaEnum empresa, List<HttpPostedFileBase> archivo)
        {
            return capacitacionSeguridadDAO.postSubirArchivos(id, empresa, archivo);
        }
        public byte[] descargarArchivoCO(int id)
        {
            return capacitacionSeguridadDAO.descargarArchivoCO(id);
        }
        public string getFileNameCO(int id)
        {
            return capacitacionSeguridadDAO.getFileNameCO(id);
        }
        public Dictionary<string, object> obtenerArchivoCODescargas(int idFormatoCambio)
        {
            return capacitacionSeguridadDAO.obtenerArchivoCODescargas(idFormatoCambio);
        }
        public List<tblS_CapacitacionSeguridadCO_GCArchivos> getArchivosFormatoCambio(int formatoCambioID)
        {
            return capacitacionSeguridadDAO.getArchivosFormatoCambio(formatoCambioID);
        }
        #endregion

        #region Competencias Operativas
        public Dictionary<string, object> getPromedioEvaluaciones(List<string> listaCC, List<AreaDTO> listaAreas, DateTime fecha)
        {
            return capacitacionSeguridadDAO.getPromedioEvaluaciones(listaCC, listaAreas, fecha);
        }
        #endregion

        #region Indicador Hrs-Hombre
        public Dictionary<string, object> getEquiposCombo()
        {
            return capacitacionSeguridadDAO.getEquiposCombo();
        }

        public Dictionary<string, object> cargarHorasAdiestramiento(List<string> listaCC, DateTime fechaInicial, DateTime fechaFinal)
        {
            return capacitacionSeguridadDAO.cargarHorasAdiestramiento(listaCC, fechaInicial, fechaFinal);
        }

        public Dictionary<string, object> guardarNuevoColaboradorCapacitacion(tblS_CapacitacionSeguridadIHHColaboradorCapacitacion colaboradorCapacitacion)
        {
            return capacitacionSeguridadDAO.guardarNuevoColaboradorCapacitacion(colaboradorCapacitacion);
        }

        public Dictionary<string, object> getInfoColaboradorCapacitacion(int colaboradorCapacitacionID)
        {
            return capacitacionSeguridadDAO.getInfoColaboradorCapacitacion(colaboradorCapacitacionID);
        }

        public Dictionary<string, object> guardarNuevoControlHoras(List<tblS_CapacitacionSeguridadIHHControlHoras> listaControlHoras)
        {
            return capacitacionSeguridadDAO.guardarNuevoControlHoras(listaControlHoras);
        }

        public Dictionary<string, object> guardarLiberados(List<tblS_CapacitacionSeguridadIHHColaboradorCapacitacion> captura, List<HttpPostedFileBase> archivos)
        {
            return capacitacionSeguridadDAO.guardarLiberados(captura, archivos);
        }

        public Dictionary<string, object> cargarDatosArchivoSoporteAdiestramiento(int id)
        {
            return capacitacionSeguridadDAO.cargarDatosArchivoSoporteAdiestramiento(id);
        }

        public Tuple<Stream, string> descargarArchivoSoporteAdiestramiento(int id)
        {
            return capacitacionSeguridadDAO.descargarArchivoSoporteAdiestramiento(id);
        }

        public ColaboradorCapacitacionReporteDTO getColaboradorCapacitacionReporte(int colaboradorCapacitacionID)
        {
            return capacitacionSeguridadDAO.getColaboradorCapacitacionReporte(colaboradorCapacitacionID);
        }
        public List<HorasHombreDTO> postObtenerTablaHorasHombre(HorasHombreDTO parametros, bool ActivarHeader)
        {
            return capacitacionSeguridadDAO.postObtenerTablaHorasHombre(parametros, ActivarHeader);
        }
        public HorasHombreDTO obtenerInputPromedios(HorasHombreDTO parametros)
        {
            return capacitacionSeguridadDAO.obtenerInputPromedios(parametros);
        }
        
        public MemoryStream crearExcelHorasHombreCapacitacion(HorasHombreDTO parametros)
        {
            return capacitacionSeguridadDAO.crearExcelHorasHombreCapacitacion(parametros);
        }

        #endregion
        public Dictionary<string, object> getEmpleadoPorClave(int claveEmpleado)
        {
            return capacitacionSeguridadDAO.getEmpleadoPorClave(claveEmpleado);
        }
        #region ADMINISTRACIONINSTRUCTORES
        public Dictionary<string, object> GetInstructores()
        {
            return capacitacionSeguridadDAO.GetInstructores();
        }
        public List<ComboDTO> GetRolesCombo()
        {
            return capacitacionSeguridadDAO.GetRolesCombo();
        }
        public Dictionary<string, object> PostGuardarInstructor(tblS_CapacitacionSeguridad_PCAdministracionInstructores parametros, bool AddEdit)
        {
            return capacitacionSeguridadDAO.PostGuardarInstructor(parametros, AddEdit);
        }
        public List<ComboDTO> GetInstructoresCombo(string cc)
        {
            return capacitacionSeguridadDAO.GetInstructoresCombo(cc);
        }
        public AdministracionInstructoresDTO getFechaInicio(string cveEmpleado)
        {
            return capacitacionSeguridadDAO.getFechaInicio(cveEmpleado);
        }

        public List<ComboDTO> GetCC()
        {
            return capacitacionSeguridadDAO.GetCC();
        }
        public bool EliminarInstructor(int id)
        {
            return capacitacionSeguridadDAO.EliminarInstructor(id);
        }
        public AdministracionInstructoresDTO ObtenerCCUnico(string cveEmpleado){
            return capacitacionSeguridadDAO.ObtenerCCUnico(cveEmpleado);
        }
       
        #endregion

        #region Plan de Capacitación
        public Dictionary<string, object> cargarCalendarioPlanCapacitacion(string cc, List<TematicaCursoEnum> listaTematicas, int empresa, DateTime mesCalendario)
        {
            return capacitacionSeguridadDAO.cargarCalendarioPlanCapacitacion(cc, listaTematicas, empresa, mesCalendario);
        }
        #endregion

        public Dictionary<string, object> GetEmpleadoCursosActos(int claveEmpleado)
        {
            return capacitacionSeguridadDAO.GetEmpleadoCursosActos(claveEmpleado);
        }
    }
}
