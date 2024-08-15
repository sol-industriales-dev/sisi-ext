using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Maquinaria.Overhaul;
using Core.DTO.Principal.Generales;
using Core.Entity.Maquinaria.Catalogo;
using Core.DTO.Maquinaria.Overhaul;

namespace Core.DAO.Maquinaria.Overhaul
{
    public interface IPlaneacionOverhaulDAO 
    {
        int GuardarCalendario(tblM_CalendarioPlaneacionOverhaul calendario, List<tblM_CapPlaneacionOverhaul> listaOverhauls);
        bool GuardarNuevoCalendario(tblM_CalendarioPlaneacionOverhaul calendario, List<tblM_CapPlaneacionOverhaul> listaOverhauls);
        List<tblM_CapPlaneacionOverhaul> getEventosOverhaul(int grupoMaquina, List<int> modeloMaquina, string obra, string subconjunto, int tipoSubConjunto);
        List<tblM_CalendarioPlaneacionOverhaul> CargarCalendariosGuardados();
        List<tblM_CapPlaneacionOverhaul> getEventosOverhaulGuardado(int idCalendario);
        tblM_CalendarioPlaneacionOverhaul getCalendarioByID(int idCalendario);
        List<tblM_CalendarioPlaneacionOverhaul> getCalendarioByObra(string obra, int anio);
        void ActualizarComponenteRemovido(int idMaquina, int idComponente, DateTime fecha);
        //List<string> getComponentes(List<ComponentePlaneacionDTO> arrComponentes);
        //string CargarActividadesOverhaul(string idEvento);
        //List<tblM_CatActividadOverhaul> CargarDatosDiagramaGantt(string idEvento);
        //bool GuardarDiagramaGantt(string idEvento, string actividadesID);
        //bool IniciarActividadesOverhaul(string idEvento);
        List<tblM_CapPlaneacionOverhaul> CargarTblInversion(int calendarioID);
        List<tblM_CalendarioPlaneacionOverhaul> CargarCalendariosEjec(string obra, int anio);
        List<ReporteKPIDTO> GetReporteDisponibilidad(List<string> cc, int anio);
        List<rptCalendarioEjecutadoDTO> GetReporteCalenEjecOverhaul(string cc, int anio);
        List<tblM_CapPlaneacionOverhaul> GetReportePrecisionOverhaul(DateTime fechaInicio, DateTime fechaFin, int tipo);
        tblM_CapPlaneacionOverhaul getEventoByID(int id);
        decimal CalculoHrsPromDiarioPub(string economicoID);
        List<tblM_CalendarioPlaneacionOverhaul> CargarCalendarios(int anio);
    }
}
