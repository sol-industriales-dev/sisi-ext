using Core.DAO.Administracion.Contratistas;
using Core.DTO.Administracion.Cotnratistas;
using Core.DTO.Principal.Generales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.Service.Administracion.Contratistas
{
    public class EmpleadosService : IEmpleadosDAO
    {
        private IEmpleadosDAO m_IEmpleadosDAO;
        private IEmpleadosDAO EmpleadosDAO
        {
            get { return m_IEmpleadosDAO; }
            set { m_IEmpleadosDAO = value; }
        }
        public EmpleadosService(IEmpleadosDAO EmpleadosDAO)
        {
            this.EmpleadosDAO = EmpleadosDAO;
        }
        public List<ComboDTO> ObtenerPais()
        {
            return EmpleadosDAO.ObtenerPais();
        }
        public List<ComboDTO> ObtenerEstado(int idPais)
        {
            return EmpleadosDAO.ObtenerEstado(idPais);
        }
        public List<ComboDTO> ObtenerMunicipio(int idEstado)
        {
            return EmpleadosDAO.ObtenerMunicipio(idEstado);
        }
        public EmpleadosDTO CrearEditar(EmpleadosDTO _objEmpleados)
        {
            return EmpleadosDAO.CrearEditar(_objEmpleados);
        }
        public EmpleadosDTO ActivarDesactivar(int id, bool esActiv)
        {
            return EmpleadosDAO.ActivarDesactivar(id, esActiv);
        }
        public List<EmpleadosDTO> getListadoDeEmpleados(int idEmpresa, DateTime FechaAlta, bool esActivo)
        {
            return EmpleadosDAO.getListadoDeEmpleados(idEmpresa,FechaAlta, esActivo);
        }
        public EmpleadosDTO CargaMasivaContratistas(HttpPostedFileBase archivo, int idEmpresa)
        {
            return EmpleadosDAO.CargaMasivaContratistas(archivo, idEmpresa);
        }
    }
}
