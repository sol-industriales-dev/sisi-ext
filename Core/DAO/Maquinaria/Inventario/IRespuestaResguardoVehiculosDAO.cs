using Core.DTO.Maquinaria.Inventario;
using Core.Entity.Maquinaria.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Maquinaria.Inventario
{
    public interface IRespuestaResguardoVehiculosDAO
    {
        void Guardar(List<tblM_RespuestaResguardoVehiculos> obj);

        List<RespuestasDTO> GetResguardoRespuestas(int id);
        List<DocumentoGridDTO> getDocumentos(int id);
        List<RespuestasDTO> GetResguardoRespuestasLiberado(int id);
    }
}
