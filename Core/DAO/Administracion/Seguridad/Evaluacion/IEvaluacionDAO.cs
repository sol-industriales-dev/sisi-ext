using Core.DTO.Administracion.Seguridad.Evaluacion;
using Core.DTO.Principal.Generales;
using Core.Entity.Administrativo.Seguridad.Evaluacion;
using System;
using System.Collections.Generic;
using System.IO;

namespace Core.DAO.Administracion.Seguridad.Evaluacion
{
    public interface IEvaluacionDAO
    {
        #region Catálogos
        Dictionary<string, object> getActividades();
        Dictionary<string, object> guardarNuevaActividad(tblSED_Actividad actividad);
        Dictionary<string, object> editarActividad(tblSED_Actividad actividad);
        Dictionary<string, object> eliminarActividad(tblSED_Actividad actividad);

        Dictionary<string, object> getPuestos();
        Dictionary<string, object> getActividadesPuesto(int puestoID);
        Dictionary<string, object> guardarNuevoPuesto(tblSED_Puesto puesto, List<ActividadDTO> actividades);
        Dictionary<string, object> editarPuesto(tblSED_Puesto puesto, List<ActividadDTO> actividades);
        Dictionary<string, object> eliminarPuesto(tblSED_Puesto puesto);

        Dictionary<string, object> getEmpleados();
        Dictionary<string, object> getEvaluadores();
        Dictionary<string, object> getEvaluadoresEmpleado(int empleadoID);
        Dictionary<string, object> guardarNuevoEmpleado(tblSED_Empleado empleado, List<EmpleadoDTO> evaluadores);
        Dictionary<string, object> editarEmpleado(tblSED_Empleado empleado, List<EmpleadoDTO> evaluadores);
        Dictionary<string, object> eliminarEmpleado(tblSED_Empleado empleado);
        Dictionary<string, object> getEmpleadoPorClave(int claveEmpleado, bool esContratista, int idEmpresaContratista);
        #endregion

        Dictionary<string, object> getActividadesCapturaCombo();
        Dictionary<string, object> guardarCaptura(EvaluacionDTO captura);
        Dictionary<string, object> getCapturasEmpleado(DateTime fechaInicio, DateTime fechaFin, int evaluadorID, int estatus);
        Dictionary<string, object> cargarDatosArchivoEvidencia(int capturaID);
        Tuple<Stream, string> descargarArchivoEvidencia(int capturaID);
        Dictionary<string, object> getCapturasEvaluador(int empleadoID, int estatus);
        Dictionary<string, object> guardarEvaluacion(EvaluacionDTO evaluacion);
        List<ComboDTO> FillComboCc();
        Dictionary<string, object> cargarDashboard(DateTime mes, int idEmpresa, int idAgrupador, List<int> categorias, int evaluadorID);
        Dictionary<string, object> getAgendaActividades(DateTime mes);
    }
}
