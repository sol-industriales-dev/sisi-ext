using Core.DTO.Maquinaria.Inventario;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Maquinaria.Inventario;
using Core.Enum.Maquinaria.StandBy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Maquinaria.Inventario
{
    public interface IStandByNuevoDAO
    {
        bool GuardarCaptura(List<tblM_STB_CapturaStandBy> lst);
        bool GuardarValidacion(List<StandByNuevoDTO> lst);
        bool GuardarLibracion(List<StandByNuevoDTO> lst);
        List<tblM_CatMaquina> getListaDisponible(string cc);
        List<tblM_STB_CapturaStandBy> getListaByEstatus(int estatus,string noAC, string noEconomico);
        List<tblM_STB_CapturaStandBy> getListaByEstatusConDepreciacion(int estatus, string noAC, string noEconomico, DateTime fechaInicio, DateTime fechaFin, int tipo);

        List<DepreciacionLugarDTO> getDepreciacionPorStandBy(string ac, string economico, DateTime fechaInicio, DateTime fechaFin, bool corteSemanal);
        List<DepreciacionLugarDTO> getDepreciacionPorNoasignado(string economico, DateTime fechaInicio, DateTime fechaFin, bool corteSemanal);
        
        List<DateTime> getDiasMartes(DateTime inicio, DateTime fin);
        bool ActivarEconomicoPorAccionRealizada(string numeroEconomico, int? idEconomico, AccionActivacionEconomicoEnum accion, object objeto, bool buscarEnEnkontrol = false);

        Dictionary<string, object> GetUsuarioTipoAutorizacion();

        Dictionary<string, object> GuardarVoBo(List<StandByNuevoDTO> lstStandByDTO);
    }
}