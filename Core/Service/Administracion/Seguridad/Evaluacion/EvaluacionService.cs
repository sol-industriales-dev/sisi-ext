using Core.DAO.Administracion.Seguridad.Evaluacion;
using Core.DTO.Administracion.Seguridad.Evaluacion;
using Core.DTO.Principal.Generales;
using Core.Entity.Administrativo.Seguridad.Evaluacion;
using System;
using System.Collections.Generic;
using System.IO;

namespace Core.Service.Administracion.Seguridad.Evaluacion
{
    public class EvaluacionService : IEvaluacionDAO
    {
        public IEvaluacionDAO evaluacionDAO { get; set; }

        public EvaluacionService(IEvaluacionDAO evaluacionDAO)
        {
            this.evaluacionDAO = evaluacionDAO;
        }

        #region Catálogos
        public Dictionary<string, object> getActividades()
        {
            return evaluacionDAO.getActividades();
        }

        public Dictionary<string, object> guardarNuevaActividad(tblSED_Actividad actividad)
        {
            return evaluacionDAO.guardarNuevaActividad(actividad);
        }

        public Dictionary<string, object> editarActividad(tblSED_Actividad actividad)
        {
            return evaluacionDAO.editarActividad(actividad);
        }

        public Dictionary<string, object> eliminarActividad(tblSED_Actividad actividad)
        {
            return evaluacionDAO.eliminarActividad(actividad);
        }

        public Dictionary<string, object> getPuestos()
        {
            return evaluacionDAO.getPuestos();
        }

        public Dictionary<string, object> getActividadesPuesto(int puestoID)
        {
            return evaluacionDAO.getActividadesPuesto(puestoID);
        }

        public Dictionary<string, object> guardarNuevoPuesto(tblSED_Puesto puesto, List<ActividadDTO> actividades)
        {
            return evaluacionDAO.guardarNuevoPuesto(puesto, actividades);
        }

        public Dictionary<string, object> editarPuesto(tblSED_Puesto puesto, List<ActividadDTO> actividades)
        {
            return evaluacionDAO.editarPuesto(puesto, actividades);
        }

        public Dictionary<string, object> eliminarPuesto(tblSED_Puesto puesto)
        {
            return evaluacionDAO.eliminarPuesto(puesto);
        }

        public Dictionary<string, object> getEmpleados()
        {
            return evaluacionDAO.getEmpleados();
        }

        public Dictionary<string, object> getEvaluadores()
        {
            return evaluacionDAO.getEvaluadores();
        }

        public Dictionary<string, object> getEvaluadoresEmpleado(int empleadoID)
        {
            return evaluacionDAO.getEvaluadoresEmpleado(empleadoID);
        }

        public Dictionary<string, object> guardarNuevoEmpleado(tblSED_Empleado empleado, List<EmpleadoDTO> evaluadores)
        {
            return evaluacionDAO.guardarNuevoEmpleado(empleado, evaluadores);
        }

        public Dictionary<string, object> editarEmpleado(tblSED_Empleado empleado, List<EmpleadoDTO> evaluadores)
        {
            return evaluacionDAO.editarEmpleado(empleado, evaluadores);
        }

        public Dictionary<string, object> eliminarEmpleado(tblSED_Empleado empleado)
        {
            return evaluacionDAO.eliminarEmpleado(empleado);
        }

        public Dictionary<string, object> getEmpleadoPorClave(int claveEmpleado, bool esContratista, int idEmpresaContratista)
        {
            return evaluacionDAO.getEmpleadoPorClave(claveEmpleado, esContratista, idEmpresaContratista);
        }
        #endregion

        public Dictionary<string, object> getActividadesCapturaCombo()
        {
            return evaluacionDAO.getActividadesCapturaCombo();
        }

        public Dictionary<string, object> guardarCaptura(EvaluacionDTO captura)
        {
            return evaluacionDAO.guardarCaptura(captura);
        }

        public Dictionary<string, object> getCapturasEmpleado(DateTime fechaInicio, DateTime fechaFin, int evaluadorID, int estatus)
        {
            return evaluacionDAO.getCapturasEmpleado(fechaInicio, fechaFin, evaluadorID, estatus);
        }

        public Dictionary<string, object> cargarDatosArchivoEvidencia(int capturaID)
        {
            return evaluacionDAO.cargarDatosArchivoEvidencia(capturaID);
        }

        public Tuple<Stream, string> descargarArchivoEvidencia(int capturaID)
        {
            return evaluacionDAO.descargarArchivoEvidencia(capturaID);
        }

        public Dictionary<string, object> getCapturasEvaluador(int empleadoID, int estatus)
        {
            return evaluacionDAO.getCapturasEvaluador(empleadoID, estatus);
        }

        public Dictionary<string, object> guardarEvaluacion(EvaluacionDTO evaluacion)
        {
            return evaluacionDAO.guardarEvaluacion(evaluacion);
        }

        public List<ComboDTO> FillComboCc()
        {
            return evaluacionDAO.FillComboCc();
        }

        public Dictionary<string, object> cargarDashboard(DateTime mes, int idEmpresa, int idAgrupador, List<int> categorias, int evaluadorID)
        {
            return evaluacionDAO.cargarDashboard(mes, idEmpresa, idAgrupador, categorias, evaluadorID);
        }

        public Dictionary<string, object> getAgendaActividades(DateTime mes)
        {
            return evaluacionDAO.getAgendaActividades(mes);
        }
    }
}
