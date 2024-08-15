using Core.DAO.Maquinaria.Captura;
using Core.DTO.Captura;
using Core.DTO.Maquinaria.Captura;
using Core.DTO.Maquinaria.Reporte;
using Core.Entity.Maquinaria.Captura;
using Core.Entity.Maquinaria.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Capturas
{
    public class CapturaCombustibleService : ICapturaCombustiblesDAO
    {
        #region Atributos
        private ICapturaCombustiblesDAO m_CapturaCombustiblesDAO;
        #endregion
        #region Propiedades
        public ICapturaCombustiblesDAO CapturaCombustiblesDAO
        {
            get { return m_CapturaCombustiblesDAO; }
            set { m_CapturaCombustiblesDAO = value; }
        }
        #endregion
        #region Constructores
        #endregion
        public CapturaCombustibleService(ICapturaCombustiblesDAO capturaCombustiblesDAO)
        {
            this.CapturaCombustiblesDAO = capturaCombustiblesDAO;
        }
        public List<capCombusitbleDTO> getDataTable(string cc, int turno, DateTime fecha, int idTipo)
        {
            return CapturaCombustiblesDAO.getDataTable(cc, turno, fecha, idTipo);
        }
        public List<EcoPipaDTO> FillCboPipa(string cc)
        {
            return CapturaCombustiblesDAO.FillCboPipa(cc);
        }
        public void Guardar(List<tblM_CapCombustible> obj)
        {
            CapturaCombustiblesDAO.Guardar(obj);
        }
        public string getNombreCC(string cc)
        {
            return CapturaCombustiblesDAO.getNombreCC(cc);
        }
        public List<InfoCombustibleDTO> getDataReporteCombustibleMensual(string cc, DateTime pfecha)
        {
            return CapturaCombustiblesDAO.getDataReporteCombustibleMensual(cc, pfecha);
        }

        public List<dataRepRendimientoCombustible> getReporteRendimientoComb(string cc, DateTime fInicio, DateTime fFin)
        {
            return CapturaCombustiblesDAO.getReporteRendimientoComb(cc, fInicio, fFin);
        }

        public List<tblM_CapCombustible> getTableInfoCombustibles(string cc, int turno, DateTime fechaInicia, DateTime fechaFinal, string economico)
        {
            return CapturaCombustiblesDAO.getTableInfoCombustibles(cc, turno, fechaInicia, fechaFinal, economico);
        }


        public List<tblM_CapCombustible> getConsumoCombustibles(string cc, DateTime fechaFin, DateTime fechaInicio, string economico, List<int> lstTipoMaquinaria)
        {
            return CapturaCombustiblesDAO.getConsumoCombustibles(cc, fechaFin, fechaInicio, economico, lstTipoMaquinaria);
        }


        public List<totalDieselEnkontrolDTO> getTotalEnkontrolConsumoDiesel(string cc)
        {
            return CapturaCombustiblesDAO.getTotalEnkontrolConsumoDiesel(cc);
        }


        public decimal getTotalContratistaConsumoDiesel(string cc, DateTime fechaFin, DateTime fechaInicio)
        {
            return CapturaCombustiblesDAO.getTotalContratistaConsumoDiesel(cc, fechaFin, fechaInicio);
        }


        public decimal getTotalMaquinaEnkontrolConsumoDiesel(string cc, string economico)
        {
            return CapturaCombustiblesDAO.getTotalMaquinaEnkontrolConsumoDiesel(cc, economico);
        }
    }
}
