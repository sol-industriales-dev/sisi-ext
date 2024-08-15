using Core.DTO.Administracion.Cotnratistas;
using Core.DTO.Principal.Generales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Administracion.Contratistas
{
    public interface IEmpresasDAO
    {
        List<ComboDTO> ObtenerEmpresasCombo();
        List<EmpresasDTO> ObtenerEmpresas(int nombreEmpresa, bool esActivo);
        EmpresasDTO AgregarEmpresa(EmpresasDTO objEmpresas);
        EmpresasDTO EditarEmpresa(EmpresasDTO objEmpresas);
        EmpresasDTO ActivarDesactivarEmpresa(int idEmpresa, bool esActivo);

    }
}
