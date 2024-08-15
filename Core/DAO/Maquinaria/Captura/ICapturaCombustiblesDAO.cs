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

namespace Core.DAO.Maquinaria.Captura
{
    public interface ICapturaCombustiblesDAO
    {
        List<capCombusitbleDTO> getDataTable(string cc, int turno, DateTime fecha, int idTipo);
        void Guardar(List<tblM_CapCombustible> obj);
        List<EcoPipaDTO> FillCboPipa(string cc);
        string getNombreCC(string cc);
        List<InfoCombustibleDTO> getDataReporteCombustibleMensual(string cc, DateTime pfecha);
        List<dataRepRendimientoCombustible> getReporteRendimientoComb(string cc, DateTime fInicio, DateTime fFin);
        List<tblM_CapCombustible> getTableInfoCombustibles(string cc, int turno, DateTime fechaInicia, DateTime fechaFinal, string economico);

        List<tblM_CapCombustible> getConsumoCombustibles(string cc, DateTime fechaFin, DateTime FechaInicio, string economico, List<int> lstTipoMaquinaria);

        decimal getTotalMaquinaEnkontrolConsumoDiesel(string cc, string economico);

        List<totalDieselEnkontrolDTO> getTotalEnkontrolConsumoDiesel(string cc);
        decimal getTotalContratistaConsumoDiesel(string cc, DateTime fechaFin, DateTime fechaInicio);
    }
}
