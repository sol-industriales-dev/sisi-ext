using Core.DAO.Administracion.Seguridad.CatDepartamentos;
using Core.DTO.Administracion.Seguridad.CatDepartamentos;
using Core.DTO.Principal.Generales;
using Core.Entity.Administrativo.Seguridad.Indicadores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Administracion.Seguridad
{
    public class CatDepartamentosService : ICatDepartamentosDAO
    {
        private ICatDepartamentosDAO m_ICatDepartamentosDAO;
        private ICatDepartamentosDAO CatDepartamentosDAO
        {
            get { return m_ICatDepartamentosDAO; }
            set { m_ICatDepartamentosDAO = value; }
        }
        public CatDepartamentosService(ICatDepartamentosDAO CatDepartamentosDAO){
            this.CatDepartamentosDAO = CatDepartamentosDAO;
        }

        public Dictionary<string, object> getClaveDepto()
        {
            return CatDepartamentosDAO.getClaveDepto();
        }

        public List<ComboDTO> getAreaOperativa()
        {
            return CatDepartamentosDAO.getAreaOperativa();
        }

        public bool CrearDepartamento(tblS_CatDepartamentos objDepartamento)
        {
            return CatDepartamentosDAO.CrearDepartamento(objDepartamento);
        }

        public List<CatDepartamentosDTO> GetCatDepartamentos(CatDepartamentosDTO objCatDepartamentos)
        {
            return CatDepartamentosDAO.GetCatDepartamentos(objCatDepartamentos);
        }

        public List<string> ObtenerDepartamento(int clave_depto)
        {
            return CatDepartamentosDAO.ObtenerDepartamento(clave_depto);
        }

        public List<string> ObtenerAreaOperativa(int idAreaOperativa)
        {
            return CatDepartamentosDAO.ObtenerAreaOperativa(idAreaOperativa);
        }

        public bool ActivarDesactivarDepartamento(int id)
        {
            return CatDepartamentosDAO.ActivarDesactivarDepartamento(id);
        }

        public bool ActualizarDepartamento(tblS_CatDepartamentos objDepartamento)
        {
            return CatDepartamentosDAO.ActualizarDepartamento(objDepartamento);
        }

        public bool EsRegistroUnico(tblS_CatDepartamentos objDepartamento)
        {
            return CatDepartamentosDAO.EsRegistroUnico(objDepartamento);
        }

        public List<ComboDTO> getCC()
        {
            return CatDepartamentosDAO.getCC();
        }
        
        public Dictionary<string, object> ObtenerCCporDepartamento(string claveDepto, int idEmpresa)
        {
            return CatDepartamentosDAO.ObtenerCCporDepartamento(claveDepto, idEmpresa);
        }
        public Dictionary<string, object> ObtenerComboCCAmbasEmpresas()
        {
            return CatDepartamentosDAO.ObtenerComboCCAmbasEmpresas();
        }
        public Dictionary<string, object> ObtenerCCporDepartamentoEditar(string claveDepto, int idEmpresa)
        {
            return CatDepartamentosDAO.ObtenerCCporDepartamentoEditar(claveDepto, idEmpresa);
        }
     
    }
}
