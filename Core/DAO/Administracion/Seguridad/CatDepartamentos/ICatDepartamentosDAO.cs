using Core.DTO.Administracion.Seguridad.CatDepartamentos;
using Core.DTO.Principal.Generales;
using Core.Entity.Administrativo.Seguridad.Indicadores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Administracion.Seguridad.CatDepartamentos
{
    public interface ICatDepartamentosDAO
    {
        Dictionary<string, object> getClaveDepto();
        List<ComboDTO> getCC();

        List<ComboDTO> getAreaOperativa();

        bool CrearDepartamento(tblS_CatDepartamentos objDepartamento);

        List<CatDepartamentosDTO> GetCatDepartamentos(CatDepartamentosDTO objCatDepartamentos);

        List<string> ObtenerDepartamento(int clave_depto);

        List<string> ObtenerAreaOperativa(int idAreaOperativa);

        bool ActivarDesactivarDepartamento(int id);

        bool ActualizarDepartamento(tblS_CatDepartamentos objDepartamento);

        bool EsRegistroUnico(tblS_CatDepartamentos objDepartamento);
        Dictionary<string, object> ObtenerComboCCAmbasEmpresas();
        Dictionary<string, object> ObtenerCCporDepartamento(string claveDepto, int idEmpresa);
        Dictionary<string, object> ObtenerCCporDepartamentoEditar(string claveDepto, int idEmpresa);

    }
}
