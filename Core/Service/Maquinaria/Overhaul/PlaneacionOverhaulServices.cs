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

namespace Core.Service.Maquinaria.Overhaul
{
    public class PlaneacionOverhaulServices : IPlaneacionOverhaulDAO
    {
        #region Atributos
        private IPlaneacionOverhaulDAO m_interfazDAO;
        #endregion Atributos

        #region Propiedades
        private IPlaneacionOverhaulDAO interfazDAO
        {
            get { return m_interfazDAO; }
            set { m_interfazDAO = value; }
        }
        #endregion Propiedades

        #region Constructores
        public PlaneacionOverhaulServices(IPlaneacionOverhaulDAO InterfazDAO)
        {
            this.interfazDAO = InterfazDAO;
        }
        #endregion Constructores

        public int GuardarCalendario(tblM_CalendarioPlaneacionOverhaul calendario, List<tblM_CapPlaneacionOverhaul> listaOverhauls) 
        {
            return interfazDAO.GuardarCalendario(calendario, listaOverhauls);
        }
        public bool GuardarNuevoCalendario(tblM_CalendarioPlaneacionOverhaul calendario, List<tblM_CapPlaneacionOverhaul> listaOverhauls)
        {
            return interfazDAO.GuardarNuevoCalendario(calendario, listaOverhauls);
        }

        public List<tblM_CapPlaneacionOverhaul> getEventosOverhaul(int grupoMaquina, List<int> modeloMaquina, string obra, string subconjunto, int tipoSubConjunto) 
        {
            return interfazDAO.getEventosOverhaul(grupoMaquina, modeloMaquina, obra, subconjunto, tipoSubConjunto);
        }
        public List<tblM_CalendarioPlaneacionOverhaul> CargarCalendariosGuardados()
        {
            return interfazDAO.CargarCalendariosGuardados();
        }
        public List<tblM_CapPlaneacionOverhaul> getEventosOverhaulGuardado(int idCalendario)
        {
            return interfazDAO.getEventosOverhaulGuardado(idCalendario);
        }
        public tblM_CalendarioPlaneacionOverhaul getCalendarioByID(int idCalendario)
        {
            return interfazDAO.getCalendarioByID(idCalendario);
        }
        public List<tblM_CalendarioPlaneacionOverhaul> getCalendarioByObra(string obra, int anio)
        {
            return interfazDAO.getCalendarioByObra(obra, anio);
        }
        public void ActualizarComponenteRemovido(int idMaquina, int idComponente, DateTime fecha)
        {
            interfazDAO.ActualizarComponenteRemovido(idMaquina, idComponente, fecha);
        }
        //public List<string> getComponentes(List<ComponentePlaneacionDTO> arrComponentes)
        //{
        //    return interfazDAO.getComponentes(arrComponentes);
        //}
        //public string CargarActividadesOverhaul(string idEvento)
        //{
        //    return interfazDAO.CargarActividadesOverhaul(idEvento);
        //}
        //public List<tblM_CatActividadOverhaul> CargarDatosDiagramaGantt(string idEvento)
        //{
        //    return interfazDAO.CargarDatosDiagramaGantt(idEvento);
        //}
        //public bool GuardarDiagramaGantt(string idEvento, string actividadesID)
        //{
        //    return interfazDAO.GuardarDiagramaGantt(idEvento, actividadesID);
        //}
        //public bool IniciarActividadesOverhaul(string idEvento)
        //{
        //    return interfazDAO.IniciarActividadesOverhaul(idEvento);
        //}
        public List<tblM_CapPlaneacionOverhaul> CargarTblInversion(int calendarioID)
        {
            return interfazDAO.CargarTblInversion(calendarioID);
        }
        public List<tblM_CalendarioPlaneacionOverhaul> CargarCalendariosEjec(string obra, int anio)
        {
            return interfazDAO.CargarCalendariosEjec(obra, anio);
        }
        public List<ReporteKPIDTO> GetReporteDisponibilidad(List<string> cc, int anio)
        {
            return interfazDAO.GetReporteDisponibilidad(cc, anio);
        }
        public List<rptCalendarioEjecutadoDTO> GetReporteCalenEjecOverhaul(string cc, int anio)
        {
            return interfazDAO.GetReporteCalenEjecOverhaul(cc, anio);
        }
        public List<tblM_CapPlaneacionOverhaul> GetReportePrecisionOverhaul(DateTime fechaInicio, DateTime fechaFin, int tipo)
        {
            return interfazDAO.GetReportePrecisionOverhaul(fechaInicio, fechaFin, tipo);
        }
        public tblM_CapPlaneacionOverhaul getEventoByID(int id)
        {
            return interfazDAO.getEventoByID(id);
        }
        public decimal CalculoHrsPromDiarioPub(string economicoID)
        {
            return interfazDAO.CalculoHrsPromDiarioPub(economicoID);
        }
        public List<tblM_CalendarioPlaneacionOverhaul> CargarCalendarios(int anio)
        {
            return interfazDAO.CargarCalendarios(anio);
        }
    }
}