using Core.DTO.Administracion.Cotnratistas;
using Core.DTO.Principal.Generales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.DAO.Administracion.Contratistas
{
    public interface IEmpleadosDAO
    {
        List<ComboDTO> ObtenerPais();
        List<ComboDTO> ObtenerEstado(int idPais);
        List<ComboDTO> ObtenerMunicipio(int idEstado);
        EmpleadosDTO CrearEditar(EmpleadosDTO _objEmpleados);
        EmpleadosDTO ActivarDesactivar(int id, bool esActiv);
        List<EmpleadosDTO> getListadoDeEmpleados(int idEmpresa, DateTime FechaAlta, bool esActivo);
        EmpleadosDTO CargaMasivaContratistas(HttpPostedFileBase archivo, int idEmpresa);
    }
}
