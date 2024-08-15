using Core.DAO.Maquinaria.Overhaul;
using Core.DTO.Maquinaria.Overhaul;
using Core.Entity.Maquinaria.Overhaul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Maquinaria.Inventario;
using Core.DTO.Principal.Generales;
using System.IO;
using Core.DTO.Maquinaria.Mantenimiento.Correctivo.ReporteFalla;

namespace Core.Service.Maquinaria.Overhaul
{
    public class ReporteFallaOverhaulServices : IReporteFallaOverhaulDAO
    {

        #region Atributos
        private IReporteFallaOverhaulDAO m_interfazDAO;
        #endregion Atributos

        #region Propiedades
        private IReporteFallaOverhaulDAO interfazDAO
        {
            get { return m_interfazDAO; }
            set { m_interfazDAO = value; }
        }
        #endregion Propiedades

        #region Constructores
        public ReporteFallaOverhaulServices(IReporteFallaOverhaulDAO InterfazDAO)
        {
            this.interfazDAO = InterfazDAO;
        }
        #endregion Constructores

        public bool Guardar(tblM_ReporteFalla obj)
        {
            return interfazDAO.Guardar(obj);
        }
        public bool Guardar(tblM_ReporteFalla_Componente obj)
        {
            return interfazDAO.Guardar(obj);
        }
        public bool Guardar(tblM_ReporteFalla_Reparacion obj)
        {
            return interfazDAO.Guardar(obj);
        }
        public List<tblM_ReporteFalla> getReporteFallas(FiltrosReporteFallaDTO obj)
        {
            return interfazDAO.getReporteFallas(obj);
        }

        public void Eliminar(int id)
        {
            interfazDAO.Eliminar(id);
        }
        public bool EliminarReparacionDesdeComponente(tblM_ReporteFalla_Componente obj)
        {
            return interfazDAO.EliminarReparacionDesdeComponente(obj);
        }
        public bool EliminarComponenteDesdeReparacion(tblM_ReporteFalla_Reparacion obj)
        {
            return interfazDAO.EliminarComponenteDesdeReparacion(obj);
        }
        public List<ReporteFallaDTO> cargarReportes(int estatus, /*string descripcionComponente,*/ int cc)
        {
            return interfazDAO.cargarReportes(estatus, /*descripcionComponente,*/ cc);
        }
        public tblM_ReporteFalla aprobarReporte(int idReporte)
        {
            return interfazDAO.aprobarReporte(idReporte);
        }
        public tblM_ReporteFalla getReporte(int idReporte)
        {
            return interfazDAO.getReporte(idReporte);
        }
        public tblM_ReporteFalla_Componente getRptComoponente(int idReporte)
        {
            return interfazDAO.getRptComoponente(idReporte);
        }
        public tblM_ReporteFalla_Reparacion getRptReparacion(int idReporte)
        {
            return interfazDAO.getRptReparacion(idReporte);
        }
        public List<tblM_ReporteFalla_Archivo> getLstImagenes(int idReporte)
        {
            return interfazDAO.getLstImagenes(idReporte);
        }
        public int getIdComponenteFromIdReporteFalla(int idReporte)
        {
            return interfazDAO.getIdComponenteFromIdReporteFalla(idReporte);
        }
        //public int getConjuntoComponente(int idSubconjunto) 
        //{
        //    return interfazDAO.getConjuntoComponente(idSubconjunto);
        //}
        public List<tblM_trackComponentes> fillCboComponentes(int idModelo, int idSubconjunto)
        {
            return interfazDAO.fillCboComponentes(idModelo, idSubconjunto);
        }
        public tblM_CatSubConjunto getSubconjuntoComponente(int idComponente)
        {
            return interfazDAO.getSubconjuntoComponente(idComponente);
        }
        public tblM_AsignacionEquipos getProcedenciaMaquina(int idMaquina)
        {
            return interfazDAO.getProcedenciaMaquina(idMaquina);
        }
        public string getCCNameByAC(string areaCuenta)
        {
            return interfazDAO.getCCNameByAC(areaCuenta);
        }
        public tblM_CatComponente getComponente(int idComponente)
        {
            return interfazDAO.getComponente(idComponente);
        }
        public tblM_trackComponentes getTrackingActualComponente(int idComponente)
        {
            return interfazDAO.getTrackingActualComponente(idComponente);
        }
        public string getCCNameByID(int idCC)
        {
            return interfazDAO.getCCNameByID(idCC);
        }
        public List<ComboDTO> fillVistoBueno(string CentroCostos)
        {
            return interfazDAO.fillVistoBueno(CentroCostos);
        }
        public List<ComboDTO> fillCboRevisa(string CentroCostos)
        {
            return interfazDAO.fillCboRevisa(CentroCostos);
        }
        public tblM_ReporteFalla getReporteByID(int idReporte)
        {
            return interfazDAO.getReporteByID(idReporte);
        }

        public Dictionary<string, object> GetArchivosReporteFalla(int _idReporteFalla)
        {
            return m_interfazDAO.GetArchivosReporteFalla(_idReporteFalla);
        }

        public Tuple<Stream, string> DescargarArchivoReporteFalla(int _idArchivo)
        {
            return m_interfazDAO.DescargarArchivoReporteFalla(_idArchivo);
        }
    }
}
