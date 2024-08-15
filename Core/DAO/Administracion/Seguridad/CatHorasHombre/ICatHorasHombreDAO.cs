using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Entity.Administrativo.Seguridad.CatHorasHombre;
using Core.DTO.Principal.Generales;
using Core.DTO.Administracion.Seguridad.CatHorasHombre;
using Core.Entity.Principal.Multiempresa;

namespace Core.DAO.Administracion.Seguridad
{
    public interface ICatHorasHombreDAO
    {
        List<ComboDTO> getCC();

        List<ComboDTO> getRoles();

        bool ActualizarHorasHombre(tblS_CatHorasHombre objHorasHombre);

        bool CrearHorasHombre(tblS_CatHorasHombre objHorasHombre);

        bool ValidarRegistroUnico(tblS_CatHorasHombre objHorasHombre);

        List<tblS_CatHorasHombre> GetHorasHombre(tblS_CatHorasHombre objHorasHombre);

        bool EliminarHorasHombre(int id);

        List<string> ObtenerDepartamento(int clave_depto);

        Dictionary<string, object> ObtenerComboCCAmbasEmpresas();

        //List<tblP_CC> GetCCHorasHombre(int idCC);
        Dictionary<string, object> ObtenerAreasPorCC(List<string> ccsCplan, int idEmpresa);
    }
}