using Core.DAO.Administracion.Contratistas;
using Core.DTO.Administracion.Cotnratistas;
using Core.DTO.Principal.Generales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Administracion.Contratistas
{
    public class EmpresasService : IEmpresasDAO
    {
        private IEmpresasDAO m_IEmpresasDAO;
        private IEmpresasDAO EmpresasDAO
        {
            get { return m_IEmpresasDAO; }
            set { m_IEmpresasDAO = value; }
        }
        public EmpresasService(IEmpresasDAO EmpresasDAO)
        {
            this.EmpresasDAO = EmpresasDAO;
        }

        public List<ComboDTO> ObtenerEmpresasCombo()
        {
            return EmpresasDAO.ObtenerEmpresasCombo();
        }
        public List<EmpresasDTO> ObtenerEmpresas(int nombreEmpresa, bool esActivo)
        {
            return EmpresasDAO.ObtenerEmpresas(nombreEmpresa, esActivo);
        }
        public EmpresasDTO AgregarEmpresa(EmpresasDTO objEmpresas)
        {
            return EmpresasDAO.AgregarEmpresa(objEmpresas);
        }
        public EmpresasDTO EditarEmpresa(EmpresasDTO objEmpresas)
        {
            return EmpresasDAO.EditarEmpresa(objEmpresas);
        }
        public EmpresasDTO ActivarDesactivarEmpresa(int idEmpresa, bool esActivo)
        {
            return EmpresasDAO.ActivarDesactivarEmpresa(idEmpresa,esActivo);
        }
    }
}
