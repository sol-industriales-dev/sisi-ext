using Core.DTO.Maquinaria.Reporte;
using Core.DTO.Reportes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Maquinaria.Reporte
{
    public interface IRepComparativaTiposDAO 
    {
        IList<RepComparativaTiposDTO> getAmountbyType(RepGastosFiltrosDTO obj);
        IList<RepComparativaTiposDTO> getAmountbyGroup(RepGastosFiltrosDTO obj,string cc);
        IList<RepComparativaTiposDTO> getAmountbyTypeNoOverhaul(RepGastosFiltrosDTO obj);
        IList<RepComparativaTiposDTO> getAmountbyTypeNoOverhaulByTipo(RepGastosFiltrosDTO obj,string cc);
        
        IList<RepGastosMaquinariaGrid> getGrupoInsumos(RepGastosFiltrosDTO obj);
        IList<RepGastosMaquinariaGrid> getInsumos(RepGastosFiltrosDTO obj);
        IList<area_cuentaDTO> getEconomicosXCentroCostos(string centroCostos);
        IList<pruebaDto> getDataPrueba();
        double getTotalImporte(string obj);

    }
}
