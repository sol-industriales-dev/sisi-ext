using Core.DAO.Maquinaria.Overhaul;
using Core.Entity.Maquinaria.Overhaul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Maquinaria.Catalogo;
using Core.DTO.Maquinaria.Overhaul;
using Core.DTO.Principal.Generales;
using Core.Entity.RecursosHumanos.Catalogo;

namespace Core.Service.Maquinaria.Overhaul
{
    public class RemocionComponenteServices : IRemocionComponenteDAO
    {
        #region Atributos
        private IRemocionComponenteDAO m_interfazDAO;
        #endregion Atributos

        #region Propiedades
        private IRemocionComponenteDAO interfazDAO
        {
            get { return m_interfazDAO; }
            set { m_interfazDAO = value; }
        }
        #endregion Propiedades

        #region Constructores
        public RemocionComponenteServices(IRemocionComponenteDAO InterfazDAO)
        {
            this.interfazDAO = InterfazDAO;
        }
        #endregion Constructores

        public RemocionDTO cargarDatosRemocionComponente(int idComponente) 
        {
            return interfazDAO.cargarDatosRemocionComponente(idComponente);
        }
        public List<ComboDTO> cargarCboComponenteInstalado(int idModelo, int idSubconjunto) 
        {
            return interfazDAO.cargarCboComponenteInstalado(idModelo, idSubconjunto);
        }
        public List<ComboDTO> cargarCboPersonal(string cc) 
        {
            return interfazDAO.cargarCboPersonal(cc);
        }
        public List<ComboDTO> getCatEmpleados() 
        {
            return interfazDAO.getCatEmpleados();
        }
        public string getPuestoDescripcion(int idPuesto) 
        {
            return interfazDAO.getPuestoDescripcion(idPuesto);
        }
        public List<ComboDTO> getMaquinasByModelo(int idModelo, int estatus) 
        {
            return interfazDAO.getMaquinasByModelo(idModelo, estatus);
        }
        public string getDescripcionCC(string cc) {
            return interfazDAO.getDescripcionCC(cc);
        }
        public void Guardar(tblM_ReporteRemocionComponente obj, int tipo) 
        {
            interfazDAO.Guardar(obj, tipo);
        }
        public decimal CalcularHrsCicloComponente(int idComponente, DateTime fecha)
        {
            return interfazDAO.CalcularHrsCicloComponente(idComponente, fecha);
        }
        public List<horometrosComponentesDTO> GetHrsCicloActualComponentes(List<int> componentesIDs)
        {
            return interfazDAO.GetHrsCicloActualComponentes(componentesIDs);
        }
        public decimal GetHrsCicloActualComponente(int componenteID, DateTime fechaActual, int id = 0, bool esHistorial = false)
        {
            return interfazDAO.GetHrsCicloActualComponente(componenteID, fechaActual, id, esHistorial);
        }
        public List<horometrosComponentesDTO> GetHrsAcumuladasComponentes(List<int> componentesIDs)
        {
            return interfazDAO.GetHrsAcumuladasComponentes(componentesIDs);
        }
        public decimal GetHrsAcumuladasComponente(int componenteID, DateTime fechaActual, bool esHistorial)
        {
            return interfazDAO.GetHrsAcumuladasComponente(componenteID, fechaActual, esHistorial);
        }
        public bool GuardarReporteActualizacion(tblM_ReporteRemocionComponente obj)
        {
            return interfazDAO.GuardarReporteActualizacion(obj);
        }
        public List<tblM_ReporteRemocionComponente> cargarReportes(int estatus, string descripcionComponente, string noEconomico, int motivoRemocion, DateTime? fechaInicio, DateTime? fechaFinal, List<string> cc, List<int> modelos, string noComponente) 
        {
            return interfazDAO.cargarReportes(estatus, descripcionComponente, noEconomico, motivoRemocion, fechaInicio, fechaFinal, cc, modelos, noComponente);
        }
        public void Eliminar(int idComponente)
        {
            interfazDAO.Eliminar(idComponente);
        }

        public tblM_ReporteRemocionComponente getReporteRemocionByID(int idReporte) 
        {
            return interfazDAO.getReporteRemocionByID(idReporte);
        }

        public string getCC(string areaCuenta)
        {
            return interfazDAO.getCC(areaCuenta);
        }

        public bool verificarReporte(int idReporte) 
        {
            return interfazDAO.verificarReporte(idReporte);
        }

        public bool enviarReporte(int idReporte, int trackID)
        {
            return interfazDAO.enviarReporte(idReporte, trackID);
        }

        public bool aprobarReporte(int idReporte)
        {
            return interfazDAO.aprobarReporte(idReporte);
        }
        public decimal GetHorasMaquina(string noEconomico)
        {
            return interfazDAO.GetHorasMaquina(noEconomico);
        }
        public bool UpdateReporteDesecho(int idReporte)
        {
            return interfazDAO.UpdateReporteDesecho(idReporte);
        }
        public decimal GetHorasMaquinaPorFecha(string noEconomico, DateTime fecha)
        {
            return interfazDAO.GetHorasMaquinaPorFecha(noEconomico, fecha);
        }
        public DateTime fechaInstalacion(int idComponente)
        {
            return interfazDAO.fechaInstalacion(idComponente);
        }
        public bool EliminarReporteRemocionByID(int idReporte)
        {
            return interfazDAO.EliminarReporteRemocionByID(idReporte);
        }
        public decimal GetHorasMaquinaIDPorFecha(int idMaquina, DateTime fecha)
        {
            return interfazDAO.GetHorasMaquinaIDPorFecha(idMaquina, fecha);
        }
        public List<ComboDTO> getEmpleadosRemocion(string term)
        {
            return interfazDAO.getEmpleadosRemocion(term);
        }

        public bool EliminarArchivoTrackComponentes(int idArchivo, int idComponente)
        {
            return interfazDAO.EliminarArchivoTrackComponentes(idArchivo, idComponente);
        }
    }
}