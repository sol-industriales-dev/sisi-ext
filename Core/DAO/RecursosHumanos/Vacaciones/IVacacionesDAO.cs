using Core.DTO.Principal.Generales;
using Core.DTO.RecursosHumanos.Vacaciones;
using Core.Entity.RecursosHumanos.Vacaciones;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.DAO.RecursosHumanos.Vacaciones
{
    public interface IVacacionesDAO
    {

        #region FILL COMBOS
        List<ComboDTO> FillComboPeriodos();
        List<ComboDTO> FillComboCC();
        Dictionary<string, object> FillComboAutorizantes(int clave_empleado);
        Dictionary<string, object> FillCboUsuarios();

        #endregion

        #region FNC GRALES

        Dictionary<string, object> GetDatosPersona(int? claveEmpleado, string nombre);
        int? GetNumDias(string claveEmpleado);
        int? GetNumDias(string claveEmpleado, DateTime fecha);
        int? GetNumDiasPermisos(int tipoPermiso);
        Dictionary<string, object> SetNotificada(int id);
        Dictionary<string, object> GetResponsable(int clvEmpleado);
        DateTime? GetFechaIngreso(int claveEmpleado);
        #endregion

        #region CRUD VACACIONES
        Dictionary<string, object> GetVacaciones(VacacionesDTO objFiltro);
        Dictionary<string, object> GetVacacionesById(VacacionesDTO objFiltro);
        Dictionary<string, object> GetVacacionesIncidencias(VacacionesDTO objFiltro);
        Dictionary<string, object> GuardarVacacionesIncidencias(List<int> fechasIDs);
        Dictionary<string, object> CrearEditarVacaciones(VacacionesDTO objVacacion);
        Dictionary<string, object> EliminarVacacion(int id);
        Dictionary<string, object> GuardarArchivoActa(int vacacion_id, HttpPostedFileBase archivoActa);
        Tuple<Stream, string> DescargarArchivoActa(int id);
        #endregion

        #region GESTION VACACIONES

        Dictionary<string, object> AutorizarVacacion(int id, int estado, string msg);
        Dictionary<string, object> GetVacacionesGestion(VacacionesDTO objFiltro);
        #endregion

        #region CRUD FECHAS

        Dictionary<string, object> GetFechas(int idReg, int tipoPermiso, int? clave_empleado, bool esGestion = false);
        Dictionary<string, object> CrearEditarFechas(int idVacacion, List<DateTime> lstFechas, int tipoPermiso, bool esSobreEscribir, bool esEditar, int diasPermitidos = 0);
        Dictionary<string, object> GetFechasByClaveEmpleado(int clave_empleado, int tipoPermiso, bool esGestion = false);

        #endregion

        #region CRUD PERIODOS
        Dictionary<string, object> GetPeriodos(PeriodosDTO objFiltro);
        Dictionary<string, object> CrearEditarPeriodo(PeriodosDTO objPeriodo);
        Dictionary<string, object> EliminarPeriodo(int id);
        #endregion

        #region RESPONSABLES
        Dictionary<string, object> GetResponsables(string cc, int claveEmpleado);
        Dictionary<string, object> VerificarAntiguedadEmpleado(int claveEmpleado);
        Dictionary<string, object> CrearEditarResponsable(VacacionesDTO objDTO);
        Dictionary<string, object> EliminarResponsable(int id);
        Dictionary<string, object> GetHistorialDias(int id);
        #endregion

        #region PENDIENTES

        Dictionary<string, object> GetVacacionesPendientes(VacacionesDTO objFiltro);

        #endregion

        #region PERMISOS
        Dictionary<string,object> GetDiasDispPermisos(int cveUsuario, int tipoPermiso);
        bool GetEsPermisoSoloConsultaVacaciones();
        bool GetEsPermisoSoloConsultaPermisos();
        #endregion

        #region INCAPACIDADES
        Dictionary<string, object> GetIncapacidades(int? estatus, List<string> ccs, DateTime? fechaInicio, DateTime? fechaTerminacion, int? claveEmpleado, string nombreEmpleado);
        Dictionary<string, object> GetHistorialIncapacidades(int clave_empleado);
        Dictionary<string, object> CrearEditarIncapacidades(IncapacidadesDTO objIncaps);
        Dictionary<string, object> DeleteIncapacidades(int id_incap);
        Dictionary<string, object> NotificarIncapacidades(int clave_empleado);
        Dictionary<string, object> GuardarArchivo(int id_incap, int tipoArchivo, HttpPostedFileBase archivo);
        tblRH_Vacaciones_Archivos GetArchivo(int id_archivo);
        Dictionary<string, object> GetIncapacidadesVencer();

        #endregion

        #region DASHBOARD INCAPS
        Dictionary<string, object> GetDashboard(List<string> ccs, DateTime? fechaInicio, DateTime? fechaFin);

        #endregion

        #region SALDOS
        Dictionary<string, object> GetSaldos(FiltroSaldosDTO objFiltro);
        Dictionary<string, object> GetSaldosDet(int clave_empleado);
        Dictionary<string, object> CrearEditarSaldo(SaldosDTO objFiltro);
        Dictionary<string, object> DeleteSaldoDet(int id);

        #endregion

        #region REPORTE DIAS
        Dictionary<string, object> GetVacacionesReporte(VacacionesDTO objFiltro);
        List<string[]> GetExcelReporte(VacacionesDTO objFiltro);
        
        #endregion

        #region DASHBOARD

        Dictionary<string, object> GetDashboardVacaciones(VacacionesDTO objFiltro);

        #endregion

        #region RETARDOS
        Dictionary<string, object> GetRetardos(RetardosDTO objFiltro);
        Dictionary<string, object> CrearEditarRetardo(RetardosDTO objRetardo);
        Dictionary<string, object> RemoveRetardo(int idRetardo);
        Dictionary<string, object> FillComboMotivosByTipo(int tipoRetardo);
        Dictionary<string, object> GuardarArchivoRetardo(int idRetardo, HttpPostedFileBase archivoActa);
        Tuple<Stream, string> DescargarArchivoRetardo(int id);
        Dictionary<string, object> GetRetardoById(RetardosDTO objFiltro);
        
        #endregion

        #region GESTION RETARDOS
        Dictionary<string, object> AutorizarRetardo(int id, int estado, string msg);
        Dictionary<string, object> GetRetardosGestion(RetardosDTO objFiltro);
        
        #endregion
    }
}
