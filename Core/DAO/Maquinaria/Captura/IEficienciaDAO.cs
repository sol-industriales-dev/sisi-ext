using Core.DTO.Maquinaria.Captura;
using Core.Entity.Maquinaria.Captura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Maquinaria.Captura
{
    public interface IEficienciaDAO
    {
        tblM_Eficiencia GuardaEficiencia(tblM_Eficiencia obj);
        List<tblM_Eficiencia> getTablaEficiencia(DateTime FechaInicioFiltro, DateTime FechaUltimoFiltro, string cc);
        List<tblM_Eficiencia> getEficienciaObraInfo(DateTime FechaInicioFiltro, DateTime FechaUltimoFiltro, string cc);
        List<RepEficienciaGeneralDTO> getEficienciaGeneralInfo(DateTime FechaInicioFiltro, DateTime FechaUltimoFiltro);
    }
}
