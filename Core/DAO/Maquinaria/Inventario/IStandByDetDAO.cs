using Core.DTO.Maquinaria.Inventario;
using Core.Entity.Maquinaria.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Maquinaria.Inventario
{
    public interface IStandByDetDAO
    {

        void GuardarStandByDet(List<standByDetDTO> obj, standByDTO standbyCliente, int StandByID);

        List<tblM_DetStandby> getListaDetStandBy(int StandByID);

        void DeleteRow(tblM_DetStandby objDetSingle);
    }
}
