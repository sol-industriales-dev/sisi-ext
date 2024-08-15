using Core.DAO.Maquinaria.Captura;
using Core.DTO.Maquinaria.Captura;
using Core.DTO.Maquinaria.Captura.OT;
using Core.DTO.Maquinaria;
using Core.Entity.Maquinaria.Captura;
using Core.Entity.Maquinaria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Principal.Usuarios;
using Core.Entity.Maquinaria.Catalogo;

namespace Core.Service.Maquinaria.Capturas
{
    public class KPIService : IKPIDAO
    {
        #region Atributos
        private IKPIDAO m_KPIDAO;
        #endregion
        #region Propiedades
        public IKPIDAO KPIDAO
        {
            get { return m_KPIDAO; }
            set { m_KPIDAO = value; }
        }
        #endregion
        #region Constructor
        public KPIService(IKPIDAO kpiDAO)
        {
            this.KPIDAO = kpiDAO;
        }
        #endregion

        public IList<KPIDTO> getKPIGeneral(List<string> cc, int tipo, int modelo, DateTime fechainicio, DateTime fechaFin)
        {
            return this.KPIDAO.getKPIGeneral(cc, tipo, modelo, fechainicio, fechaFin);
        }
        public void Guardar(tblM_KPI obj)
        {
            this.KPIDAO.Guardar(obj);
        }

        public kpiTipoMantenimientoDTO getMDTipoMantenimiento(int id, List<string> cc, DateTime fechainicio, DateTime fechaFin, List<tblM_CatMaquina> _lstCatMaquinas = null, 
                                                                List<tblM_CatCriteriosCausaParo> _lstCatCriteriosCausaParo = null, List<tblM_CapOrdenTrabajo> _lstCapOrdenTrabajo = null)
        {
            return this.KPIDAO.getMDTipoMantenimiento(id, cc, fechainicio, fechaFin, _lstCatMaquinas, _lstCatCriteriosCausaParo, _lstCapOrdenTrabajo);
        }

        public kpiInfoGeneralDTO getInfoGeneral(int id, List<string> cc, DateTime fechainicio, DateTime fechaFin, List<tblM_CatMaquina> _lstCatMaquinas = null, List<tblM_CapOrdenTrabajo> _lstCapOrdenTrabajo = null,
                                                    List<tblM_CapHorometro> _lstCapHorometro = null, List<tblM_DetOrdenTrabajo> _lstDetOrdenTrabajo = null, List<tblM_CatCriteriosCausaParo> _lstCatCriteriosCausaParo = null)
        {
            return this.KPIDAO.getInfoGeneral(id, cc, fechainicio, fechaFin, _lstCatMaquinas, _lstCapOrdenTrabajo, _lstCapHorometro, _lstDetOrdenTrabajo, _lstCatCriteriosCausaParo);
        }
        public IList<kpiMTTOyParoDTO> getMTTOyParo(int id, List<string> cc, DateTime fechainicio, DateTime fechaFin, List<tblM_CatMaquina> _lstCatMaquinas, List<tblM_CapOrdenTrabajo> _lstCapOrdenTrabajo, 
                                                    List<tblM_DetOrdenTrabajo> _lstDetOrdenTrabajo, List<tblM_CatCriteriosCausaParo> _lstCatCriteriosCausaParo)
        {
            return this.KPIDAO.getMTTOyParo(id, cc, fechainicio, fechaFin, _lstCatMaquinas, _lstCapOrdenTrabajo, _lstDetOrdenTrabajo, _lstCatCriteriosCausaParo);
        }
        public KPIReporteEquipoDTO getKPIReporteEquipo(int id, List<string> cc, DateTime fechainicio, DateTime fechaFin)
        {
            return this.KPIDAO.getKPIReporteEquipo(id, cc, fechainicio, fechaFin);
        }

        public kpiRepMetricasDTO getKPIRepMetricasDTO(List<string> cc, int tipo, int modelo, DateTime fechainicio, DateTime fechaFin)
        {
            return this.KPIDAO.getKPIRepMetricasDTO(cc, tipo, modelo, fechainicio, fechaFin);
        }
        public kpiRepGraficas getKPIRepGraficas(List<string> cc, int tipo, int modelo, DateTime fechainicio, DateTime fechaFin, List<KPIDTO> data = null)
        {
            return this.KPIDAO.getKPIRepGraficas(cc, tipo, modelo, fechainicio, fechaFin, data);
        }

        public List<tblHorasHombreDTO> ConsultaFiltrosOT(FiltrosRtpHorasHombre obj)
        {
            return this.KPIDAO.ConsultaFiltrosOT(obj);
        }

        public List<tblHorasHombreDetDTO> ConsultaFiltrosOTDET(tblHorasHombreDetalleDTO obj)
        {
            return this.KPIDAO.ConsultaFiltrosOTDET(obj);
        }
        public List<tblDetallePersonalDTO> GetOTEmpleado(int EmpleadoID, DateTime FechaInicio, DateTime FechaFin)
        {
            return this.KPIDAO.GetOTEmpleado(EmpleadoID, FechaInicio, FechaFin);
        }

        public List<frecuenciaParoDTO> getDataFrecuenciasParo(FiltrosRtpHorasHombre obj, decimal horaIncio, decimal horaFinal)
        {
            return this.KPIDAO.getDataFrecuenciasParo(obj, horaIncio, horaFinal);
        }
        public List<detFrecuenciaParoDTO> DetalleTiposParo(int TipoParoID, DateTime FechaInicio, DateTime FechaFin)
        {
            return this.KPIDAO.DetalleTiposParo(TipoParoID, FechaInicio, FechaFin);
        }

        public List<tblHorasHombreDetDTO> ConsultaRptPersonal(List<int> Listapuestos, List<int> listaPersonalID)
        {
            return this.KPIDAO.ConsultaRptPersonal(Listapuestos, listaPersonalID);
        }

        public List<tblM_DetOrdenTrabajo> getDetOrdenTrabajo(FiltrosRtpHorasHombre obj, decimal horaIncio, decimal horaFinal)
        {
            return this.KPIDAO.getDetOrdenTrabajo(obj, horaIncio, horaFinal);
        }

        public List<tblM_CatCriteriosCausaParo> getCatCriteriosCausa()
        {
            return this.KPIDAO.getCatCriteriosCausa();
        }

        public List<tblM_DisponibilidadMaquina> indiceDisponibilidad(List<tblP_CC_Usuario> listObj)
        {
            return this.KPIDAO.indiceDisponibilidad(listObj);
        }

        public List<tblM_RendimientoMaquina> alertasRendimiento(List<tblP_CC_Usuario> listObj)
        {
            return this.KPIDAO.alertasRendimiento(listObj);
        }
        public decimal promedioRendimiento(string cc, int modelo)
        {
            return this.KPIDAO.promedioRendimiento(cc, modelo);
        }

        public IList<KPINoFormatDTO> getKPIGeneralNoFormat(List<string> cc, int tipo, int modelo, DateTime Fechainicio, DateTime FechaFin)
        {
            return this.KPIDAO.getKPIGeneralNoFormat(cc, tipo, modelo, Fechainicio, FechaFin);
        }


    }
}
