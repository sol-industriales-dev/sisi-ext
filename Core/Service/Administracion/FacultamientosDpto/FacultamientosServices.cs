using Core.DAO.Administracion.FacultamientosDpto;
using Core.DTO.Administracion.Facultamiento;
using Core.DTO.Principal.Generales;
using System.Collections.Generic;

namespace Core.Service.Administracion.FacultamientosDpto
{
    public class FacultamientosServices : IFacultamientosDAO
    {
        #region Atributos
        private IFacultamientosDAO facultamientos;
        #endregion

        #region Propiedades
        public IFacultamientosDAO FacultamientosDAO
        {
            get { return facultamientos; }
            set { facultamientos = value; }
        }
        #endregion

        #region Constructores
        public FacultamientosServices(IFacultamientosDAO facultamientosDAO)
        {
            this.FacultamientosDAO = facultamientosDAO;
        }
        #endregion

        #region metodos catalogo
        public List<ComboDTO> ObtenerDepartamentos()
        {
            return this.FacultamientosDAO.ObtenerDepartamentos();
        }

        public Dictionary<string, object> GuardarPlantilla(string titulo, List<int> listaDepartamentos, List<ConceptoDTO> listaConceptos)
        {
            return this.FacultamientosDAO.GuardarPlantilla(titulo, listaDepartamentos, listaConceptos);
        }

        public Dictionary<string, object> ObtenerCatalogo(int departamentoID)
        {
            return this.FacultamientosDAO.ObtenerCatalogo(departamentoID);
        }

        public Dictionary<string, object> ObtenerPlantilla(int plantillaID)
        {
            return this.FacultamientosDAO.ObtenerPlantilla(plantillaID);
        }

        public Dictionary<string, object> ActualizarPlantilla(string nuevoTitulo, List<int> nuevosDepartamentos, List<ConceptoDTO> nuevosConceptos, int plantillaID, bool esActualizar)
        {
            return this.FacultamientosDAO.ActualizarPlantilla(nuevoTitulo, nuevosDepartamentos, nuevosConceptos, plantillaID, esActualizar);
        }
        #endregion

        #region metodos asignacion
        public List<ComboDTO> ObtenerCentrosCostos()
        {
            return this.FacultamientosDAO.ObtenerCentrosCostos();
        }

        public Dictionary<string, object> CargarPlantillasCC(int centroCostosID)
        {
            return this.FacultamientosDAO.CargarPlantillasCC(centroCostosID);
        }

        public Dictionary<string, object> AsignarFacultamientos(int centroCostosID, List<FacultamientoDTO> listaFacultamientos, List<EmpleadoAutorizanteDTO> listaAutorizantes, bool todoCompleto)
        {
            return this.FacultamientosDAO.AsignarFacultamientos(centroCostosID, listaFacultamientos, listaAutorizantes, todoCompleto);
        }

        public Dictionary<string, object> ObtenerPaquetes(int departamentoID, int centroCostosID, int estado)
        {
            return this.FacultamientosDAO.ObtenerPaquetes(departamentoID, centroCostosID, estado);
        }

        public List<ComboDTO> ObtenerObras()
        {
            return this.FacultamientosDAO.ObtenerObras();
        }

        public Dictionary<string, object> ObtenerPaqueteActualizar(int paqueteID, bool esReporte = false)
        {
            return this.FacultamientosDAO.ObtenerPaqueteActualizar(paqueteID, esReporte);
        }

        public Dictionary<string, object> ActualizarFacultamientos(int paqueteID, List<FacultamientoDTO> listaFacultamientos, List<EmpleadoAutorizanteDTO> listaAutorizantes, bool todoCompleto)
        {
            return this.FacultamientosDAO.ActualizarFacultamientos(paqueteID, listaFacultamientos, listaAutorizantes, todoCompleto);
        }
        #endregion

        #region metodos autorizacion
        public Dictionary<string, object> ObtenerPaquetesPorAutorizar()
        {
            return this.FacultamientosDAO.ObtenerPaquetesPorAutorizar();
        }

        public Dictionary<string, object> ObtenerAutorizantes(int paqueteID)
        {
            return this.FacultamientosDAO.ObtenerAutorizantes(paqueteID);
        }

        public Dictionary<string, object> AutorizarPaquete(int paqueteID)
        {
            return this.FacultamientosDAO.AutorizarPaquete(paqueteID);
        }

        public Dictionary<string, object> RechazarPaquete(int paqueteID, string comentario)
        {
            return this.FacultamientosDAO.RechazarPaquete(paqueteID, comentario);
        }

        public Dictionary<string, object> EnviarCorreoAutorizacion(int paqueteID, int ordenVoBo, List<byte[]> pdf)
        {
            return this.FacultamientosDAO.EnviarCorreoAutorizacion(paqueteID, ordenVoBo, pdf);
        }

        public Dictionary<string, object> EnviarCorreoAutorizacionCompleta(int paqueteID, List<byte[]> pdf)
        {
            return this.FacultamientosDAO.EnviarCorreoAutorizacionCompleta(paqueteID, pdf);
        }

        public Dictionary<string, object> EnviarCorreoRechazo(int paqueteID, string comentario, List<byte[]> pdf)
        {
            return this.FacultamientosDAO.EnviarCorreoRechazo(paqueteID, comentario, pdf);
        }
        #endregion

        #region metodos historicos
        public Dictionary<string, object> ObtenerHistorico(int ccID)
        {
            return this.FacultamientosDAO.ObtenerHistorico(ccID);

        }
        #endregion

        #region metodos por empleado
        public Dictionary<string, object> ObtenerFacultamientosEmpleado(int claveEmpleado, int centroCostosID)
        {
            return this.FacultamientosDAO.ObtenerFacultamientosEmpleado(claveEmpleado, centroCostosID);

        }

        public Dictionary<string, object> ObtenerFacultamiento(int facultamientoID)
        {
            return this.FacultamientosDAO.ObtenerFacultamiento(facultamientoID);
        }

        public string ObtenerNombreEmpleadoPorClave(int claveEmpleado)
        {
            return this.FacultamientosDAO.ObtenerNombreEmpleadoPorClave(claveEmpleado);
        }
        #endregion

        #region metodos catalogo grupos
        public Dictionary<string, object> ObtenerCCGrupo(int grupoID)
        {
            return this.FacultamientosDAO.ObtenerCCGrupo(grupoID);
        }
        public Dictionary<string, object> GuardarCCGrupo(int ccID,int? grupoID)
        {
            return this.FacultamientosDAO.GuardarCCGrupo(ccID,grupoID);
        }
        public Dictionary<string, object> getTblGrupo()
        {
            return this.FacultamientosDAO.getTblGrupo();
        }
        public Dictionary<string, object> delGrupo(int id)
        {
            return this.FacultamientosDAO.delGrupo(id);
        }
        public Dictionary<string, object> GuardarGrupo(string grupo)
        {
            return this.FacultamientosDAO.GuardarGrupo(grupo);
        }
        public Dictionary<string, object> delPuesto(int id)
        {
            return this.FacultamientosDAO.delPuesto(id);
        }
        #endregion
    }
}
