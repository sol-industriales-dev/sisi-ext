using Core.DAO.RecursosHumanos.Vacaciones;
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

namespace Core.Service.RecursosHumanos.Vacaciones
{
    public class VacacionesService : IVacacionesDAO
    {

        public IVacacionesDAO vacacionesInterfaz { get; set; }

        public VacacionesService(IVacacionesDAO vacacionesTemp)
        {
            this.vacacionesInterfaz = vacacionesTemp;
        }

        #region FILL COMBOS

        public List<ComboDTO> FillComboPeriodos()
        {
            return vacacionesInterfaz.FillComboPeriodos();
        }

        public List<ComboDTO> FillComboCC()
        {
            return vacacionesInterfaz.FillComboCC();
        }

        public Dictionary<string, object> FillComboAutorizantes(int clave_empleado)
        {
            return vacacionesInterfaz.FillComboAutorizantes(clave_empleado);
        }

        public Dictionary<string, object> FillCboUsuarios()
        {
            return vacacionesInterfaz.FillCboUsuarios();
        }
        #endregion

        #region FNC GRALES

        public Dictionary<string, object> GetDatosPersona(int? claveEmpleado, string nombre)
        {
            return vacacionesInterfaz.GetDatosPersona(claveEmpleado, nombre);
        }

        public int? GetNumDias(string claveEmpleado)
        {
            return vacacionesInterfaz.GetNumDias(claveEmpleado);
        }

        public int? GetNumDias(string claveEmpleado, DateTime fecha)
        {
            return vacacionesInterfaz.GetNumDias(claveEmpleado, fecha);
        }

        public int? GetNumDiasPermisos(int tipoPermiso)
        {
            return vacacionesInterfaz.GetNumDiasPermisos(tipoPermiso);
        }

        public Dictionary<string, object> SetNotificada(int id)
        {
            return vacacionesInterfaz.SetNotificada(id);
        }

        public Dictionary<string, object> GetResponsable(int clvEmpleado)
        {
            return vacacionesInterfaz.GetResponsable(clvEmpleado);
        }

        public DateTime? GetFechaIngreso(int claveEmpleado)
        {
            return vacacionesInterfaz.GetFechaIngreso(claveEmpleado);
        }


        #endregion

        #region CRUD VACACIONES

        public Dictionary<string, object> GetVacaciones(VacacionesDTO objFiltro)
        {
            return vacacionesInterfaz.GetVacaciones(objFiltro);
        }
        public Dictionary<string, object> GetVacacionesById(VacacionesDTO objFiltro)
        {
            return vacacionesInterfaz.GetVacacionesById(objFiltro);
        }
        public Dictionary<string, object> GetVacacionesIncidencias(VacacionesDTO objFiltro)
        {
            return vacacionesInterfaz.GetVacacionesIncidencias(objFiltro);
        }
        public Dictionary<string, object> GuardarVacacionesIncidencias(List<int> fechasIDs)
        {
            return vacacionesInterfaz.GuardarVacacionesIncidencias(fechasIDs);
        }
        public Dictionary<string, object> CrearEditarVacaciones(VacacionesDTO objVacaciones)
        {
            return vacacionesInterfaz.CrearEditarVacaciones(objVacaciones);
        }
        public Dictionary<string, object> EliminarVacacion(int id)
        {
            return vacacionesInterfaz.EliminarVacacion(id);
        }
        public Dictionary<string, object> GuardarArchivoActa(int vacacion_id, HttpPostedFileBase archivoActa)
        {
            return vacacionesInterfaz.GuardarArchivoActa(vacacion_id, archivoActa);
        }

        public Tuple<Stream, string> DescargarArchivoActa(int id)
        {
            return vacacionesInterfaz.DescargarArchivoActa(id);
        }
        #endregion

        #region GESTION VACACIONES

        public Dictionary<string, object> AutorizarVacacion(int id, int estado, string msg)
        {
            return vacacionesInterfaz.AutorizarVacacion(id, estado, msg);
        }
        public Dictionary<string, object> GetVacacionesGestion(VacacionesDTO objFiltro) {
            return vacacionesInterfaz.GetVacacionesGestion(objFiltro);
        }
        #endregion

        #region CRUD FECHAS

        public Dictionary<string, object> GetFechas(int idReg, int tipoPermiso, int? clave_empleado, bool esGestion = false)
        {
            return vacacionesInterfaz.GetFechas(idReg, tipoPermiso, clave_empleado, esGestion);
        }

        public Dictionary<string, object> CrearEditarFechas(int idVacacion, List<DateTime> lstFechas, int tipoPermiso, bool esSobreEscribir, bool esEditar, int diasPermitidos = 0)
        {
            return vacacionesInterfaz.CrearEditarFechas(idVacacion, lstFechas, tipoPermiso, esSobreEscribir, esEditar, diasPermitidos);
        }

        public Dictionary<string, object> GetFechasByClaveEmpleado(int clave_empleado, int tipoPermiso, bool esGestion = false)
        {
            return vacacionesInterfaz.GetFechasByClaveEmpleado(clave_empleado, tipoPermiso, esGestion);
        }
        #endregion

        #region CRUD PERIODOS

        public Dictionary<string, object> GetPeriodos(PeriodosDTO objFiltro)
        {
            return vacacionesInterfaz.GetPeriodos(objFiltro);
        }
        public Dictionary<string, object> CrearEditarPeriodo(PeriodosDTO objPeriodo)
        {
            return vacacionesInterfaz.CrearEditarPeriodo(objPeriodo);
        }
        public Dictionary<string, object> EliminarPeriodo(int id)
        {
            return vacacionesInterfaz.EliminarPeriodo(id);
        }

        #endregion

        #region RESPONSABLES
        public Dictionary<string, object> GetResponsables(string cc, int claveEmpleado)
        {
            return vacacionesInterfaz.GetResponsables(cc, claveEmpleado);
        }

        public Dictionary<string, object> VerificarAntiguedadEmpleado(int claveEmpleado)
        {
            return vacacionesInterfaz.VerificarAntiguedadEmpleado(claveEmpleado);
        }

        public Dictionary<string, object> CrearEditarResponsable(VacacionesDTO objDTO)
        {
            return vacacionesInterfaz.CrearEditarResponsable(objDTO);
        }

        public Dictionary<string, object> EliminarResponsable(int id)
        {
            return vacacionesInterfaz.EliminarResponsable(id);
        }

        public Dictionary<string, object> GetHistorialDias(int id)
        {
            return vacacionesInterfaz.GetHistorialDias(id);
        }
        #endregion

        #region PENDIENTES

        public Dictionary<string, object> GetVacacionesPendientes(VacacionesDTO objFiltro)
        {
            return vacacionesInterfaz.GetVacacionesPendientes(objFiltro);
        }

        #endregion

        #region PERMISOS
        public Dictionary<string, object> GetDiasDispPermisos(int cveUsuario, int tipoPermiso)
        {
            return vacacionesInterfaz.GetDiasDispPermisos(cveUsuario, tipoPermiso);
        }
        public bool GetEsPermisoSoloConsultaVacaciones()
        {
            return vacacionesInterfaz.GetEsPermisoSoloConsultaVacaciones();
        }
        public bool GetEsPermisoSoloConsultaPermisos()
        {
            return vacacionesInterfaz.GetEsPermisoSoloConsultaPermisos();
        }

        #endregion

        #region INCAPACIDADES
        public Dictionary<string, object> GetIncapacidades(int? estatus, List<string> ccs, DateTime? fechaInicio, DateTime? fechaTerminacion, int? claveEmpleado, string nombreEmpleado)
        {
            return vacacionesInterfaz.GetIncapacidades(estatus, ccs, fechaInicio, fechaTerminacion, claveEmpleado, nombreEmpleado);
        }
        public Dictionary<string, object> GetHistorialIncapacidades(int clave_empleado)
        {
            return vacacionesInterfaz.GetHistorialIncapacidades(clave_empleado);
        }
        public Dictionary<string, object> CrearEditarIncapacidades(IncapacidadesDTO objIncaps)
        {
            return vacacionesInterfaz.CrearEditarIncapacidades(objIncaps);
        }
        public Dictionary<string, object> DeleteIncapacidades(int id_incap)
        {
            return vacacionesInterfaz.DeleteIncapacidades(id_incap);
        }
        public Dictionary<string, object> NotificarIncapacidades(int clave_empleado)
        {
            return vacacionesInterfaz.NotificarIncapacidades(clave_empleado);
        }
        public Dictionary<string, object> GuardarArchivo(int id_incap, int tipoArchivo, HttpPostedFileBase archivo)
        {
            return vacacionesInterfaz.GuardarArchivo(id_incap, tipoArchivo, archivo);
        }
        public tblRH_Vacaciones_Archivos GetArchivo(int id_archivo)
        {
            return vacacionesInterfaz.GetArchivo(id_archivo);
        }
        public Dictionary<string,object> GetIncapacidadesVencer()
        {
            return vacacionesInterfaz.GetIncapacidadesVencer();
        }
        #endregion

        #region DASHBOARD INCAPS
        public Dictionary<string, object> GetDashboard(List<string> ccs, DateTime? fechaInicio, DateTime? fechaFin)
        {
            return vacacionesInterfaz.GetDashboard(ccs, fechaInicio, fechaFin);
        }
        
        #endregion

        #region SALDOS
        public Dictionary<string, object> GetSaldos(FiltroSaldosDTO objFiltro)
        {
            return vacacionesInterfaz.GetSaldos(objFiltro);
        }

        public Dictionary<string, object> GetSaldosDet(int clave_empleado)
        {
            return vacacionesInterfaz.GetSaldosDet(clave_empleado);
        }

        public Dictionary<string, object> CrearEditarSaldo(SaldosDTO objFiltro)
        {
            return vacacionesInterfaz.CrearEditarSaldo(objFiltro);
        }

        public Dictionary<string, object> DeleteSaldoDet(int id)
        {
            return vacacionesInterfaz.DeleteSaldoDet(id);
        }
        #endregion

        #region REPORTE DIAS
        public Dictionary<string, object> GetVacacionesReporte(VacacionesDTO objFiltro)
        {
            return vacacionesInterfaz.GetVacacionesReporte(objFiltro);
        }

        public List<string[]> GetExcelReporte(VacacionesDTO objFiltro)
        {
            return vacacionesInterfaz.GetExcelReporte(objFiltro);
        }

        #endregion

        #region DASHBOARD
        public Dictionary<string, object> GetDashboardVacaciones(VacacionesDTO objFiltro)
        {
            return vacacionesInterfaz.GetDashboardVacaciones(objFiltro);
        }
        #endregion

        #region RETARDOS
        public Dictionary<string, object> GetRetardos(RetardosDTO objFiltro)
        {
            return vacacionesInterfaz.GetRetardos(objFiltro);
        }
        public Dictionary<string, object> CrearEditarRetardo(RetardosDTO objRetardo)
        {
            return vacacionesInterfaz.CrearEditarRetardo(objRetardo);
        }
        public Dictionary<string, object> RemoveRetardo(int idRetardo)
        {
            return vacacionesInterfaz.RemoveRetardo(idRetardo);
        }
        public Dictionary<string, object> FillComboMotivosByTipo(int tipoRetardo)
        {
            return vacacionesInterfaz.FillComboMotivosByTipo(tipoRetardo);
        }
        public Dictionary<string, object> GuardarArchivoRetardo(int idRetardo, HttpPostedFileBase archivoActa)
        {
            return vacacionesInterfaz.GuardarArchivoRetardo(idRetardo, archivoActa);
        }
        public Tuple<Stream, string> DescargarArchivoRetardo(int id)
        {
            return vacacionesInterfaz.DescargarArchivoRetardo(id);
        }
        #endregion

        #region GESTION PRESTAMO
        public Dictionary<string, object> AutorizarRetardo(int id, int estado, string msg)
        {
            return vacacionesInterfaz.AutorizarRetardo(id, estado, msg);
        }
        public Dictionary<string, object> GetRetardosGestion(RetardosDTO objFiltro)
        {
            return vacacionesInterfaz.GetRetardosGestion(objFiltro);
        }
        public Dictionary<string, object> GetRetardoById(RetardosDTO objFiltro)
        {
            return vacacionesInterfaz.GetRetardoById(objFiltro);
        }
        #endregion
    }
}