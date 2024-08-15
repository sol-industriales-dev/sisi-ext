using Core.DTO.Maquinaria.Inventario;
using Core.Entity.Maquinaria.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Maquinaria.Inventario
{
    public interface IAutorizacionStandByDAO
    {
         void Guardar(tblM_AutorizacionStandBy obj);

         tblM_AutorizacionStandBy GetAutorizacion(int idAsignacion, int idEconomico);
         tblM_AutorizacionStandBy GetAutorizacionByID(int idAutorizacion);
         List<rptConciliacionDTO> GetReporte(string cc, DateTime inicio, DateTime fin);
         List<int> GetAutorizadoresStandby(string cc, DateTime inicio, DateTime fin);

         tblM_AutorizaStandby GetAutorizacionByIDStanby(int idStandby);
    }
}
