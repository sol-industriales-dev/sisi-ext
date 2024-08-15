using Core.DTO.Maquinaria.Inventario;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Maquinaria.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Maquinaria.Inventario
{
    public interface IStandByDAO
    {
        void GuardarStandBy(tblM_CapStandBy obj);

        List<tblM_CapStandBy> getListaStandBy(List<string> listCC, DateTime fechainicio, DateTime fechaFin, int filtro);
        List<StandbyGridDTO> GetListMaquinaria(List<string> listCC, DateTime fechaInicio, DateTime fechaFin);
        tblM_CapStandBy getStandByID(int id);

    }
}
