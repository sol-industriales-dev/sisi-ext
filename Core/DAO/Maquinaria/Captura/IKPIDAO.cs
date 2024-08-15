using Core.DTO.Maquinaria;
using Core.DTO.Maquinaria.Captura;
using Core.DTO.Maquinaria.Captura.OT;
using Core.Entity.Maquinaria.Captura;
using Core.Entity.Maquinaria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Principal.Usuarios;
using Core.Entity.Maquinaria.Catalogo;

namespace Core.DAO.Maquinaria.Captura
{
    public interface IKPIDAO
    {
        IList<KPIDTO> getKPIGeneral(List<string> cc, int tipo, int modelo, DateTime fechainicio, DateTime fechaFin);
        void Guardar(tblM_KPI obj);
        kpiTipoMantenimientoDTO getMDTipoMantenimiento(int id, List<string> cc, DateTime fechainicio, DateTime fechaFin, List<tblM_CatMaquina> _lstCatMaquinas = null, 
                                                        List<tblM_CatCriteriosCausaParo> _lstCatCriteriosCausaParo = null, List<tblM_CapOrdenTrabajo> _lstCapOrdenTrabajo = null);
        kpiInfoGeneralDTO getInfoGeneral(int id, List<string> cc, DateTime fechainicio, DateTime fechaFin, List<tblM_CatMaquina> _lstCatMaquinas = null, List<tblM_CapOrdenTrabajo> _lstCapOrdenTrabajo = null,
                                                    List<tblM_CapHorometro> _lstCapHorometro = null, List<tblM_DetOrdenTrabajo> _lstDetOrdenTrabajo = null, List<tblM_CatCriteriosCausaParo> _lstCatCriteriosCausaParo = null);
        IList<kpiMTTOyParoDTO> getMTTOyParo(int id, List<string> cc, DateTime fechainicio, DateTime fechaFin, List<tblM_CatMaquina> _lstCatMaquinas, List<tblM_CapOrdenTrabajo> _lstCapOrdenTrabajo,
                                                    List<tblM_DetOrdenTrabajo> _lstDetOrdenTrabajo, List<tblM_CatCriteriosCausaParo> _lstCatCriteriosCausaParo);
        KPIReporteEquipoDTO getKPIReporteEquipo(int id, List<string> cc, DateTime fechainicio, DateTime fechaFin);
        kpiRepMetricasDTO getKPIRepMetricasDTO(List<string> cc, int tipo, int modelo, DateTime fechainicio, DateTime fechaFin);
        kpiRepGraficas getKPIRepGraficas(List<string> cc, int tipo, int modelo, DateTime fechainicio, DateTime fechaFin, List<KPIDTO> data = null);
        List<tblHorasHombreDTO> ConsultaFiltrosOT(FiltrosRtpHorasHombre obj);
        List<tblHorasHombreDetDTO> ConsultaFiltrosOTDET(tblHorasHombreDetalleDTO obj);
        List<tblDetallePersonalDTO> GetOTEmpleado(int EmpleadoID, DateTime FechaInicio, DateTime FechaFin);
        List<frecuenciaParoDTO> getDataFrecuenciasParo(FiltrosRtpHorasHombre obj, decimal horaIncio, decimal horaFinal);
        List<detFrecuenciaParoDTO> DetalleTiposParo(int TipoParoID, DateTime FechaInicio, DateTime FechaFin);
        List<tblHorasHombreDetDTO> ConsultaRptPersonal(List<int> Listapuestos, List<int> listaPersonalID);
        List<tblM_DetOrdenTrabajo> getDetOrdenTrabajo(FiltrosRtpHorasHombre obj, decimal horaIncio, decimal horaFinal);
        List<tblM_CatCriteriosCausaParo> getCatCriteriosCausa();

        List<tblM_DisponibilidadMaquina> indiceDisponibilidad(List<tblP_CC_Usuario> listObj);
        List<tblM_RendimientoMaquina> alertasRendimiento(List<tblP_CC_Usuario> listObj);
        decimal promedioRendimiento(string cc, int modelo);
        IList<KPINoFormatDTO> getKPIGeneralNoFormat(List<string> cc, int tipo, int modelo, DateTime Fechainicio, DateTime FechaFin);
    }

}
