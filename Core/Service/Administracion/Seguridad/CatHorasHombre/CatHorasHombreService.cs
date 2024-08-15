using Core.Entity.Administrativo.Seguridad.CatHorasHombre;
using Core.DTO.Administracion.Seguridad.CatHorasHombre;
using Core.DAO.Administracion.Seguridad;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTO.Principal.Generales;
using Core.Entity.Principal.Multiempresa;

namespace Core.Service.Administracion.Seguridad.CatHorasHombre
{
    public class CatHorasHombreService : ICatHorasHombreDAO
    {
        private ICatHorasHombreDAO m_ICatHorasHombreDAO;

        private ICatHorasHombreDAO CatHorasHombreDAO
        {
            get { return m_ICatHorasHombreDAO; }
            set { m_ICatHorasHombreDAO = value; }
        }

        public CatHorasHombreService(ICatHorasHombreDAO CatHorasHombreDAO)
        {
            this.CatHorasHombreDAO = CatHorasHombreDAO;
        }

        public List<ComboDTO> getCC()
        {
            return CatHorasHombreDAO.getCC();
        }

        public List<ComboDTO> getRoles()
        {
            return CatHorasHombreDAO.getRoles();
        }

        public bool ActualizarHorasHombre(tblS_CatHorasHombre objHorasHombre)
        {
            return CatHorasHombreDAO.ActualizarHorasHombre(objHorasHombre);
        }

        public bool CrearHorasHombre(tblS_CatHorasHombre objHorasHombre)
        {
            return CatHorasHombreDAO.CrearHorasHombre(objHorasHombre);
        }

        public bool ValidarRegistroUnico(tblS_CatHorasHombre objHorasHombre)
        {
            return CatHorasHombreDAO.ValidarRegistroUnico(objHorasHombre);
        }

        public List<tblS_CatHorasHombre> GetHorasHombre(tblS_CatHorasHombre objHorasHombre)
        {
            return CatHorasHombreDAO.GetHorasHombre(objHorasHombre);
        }

        public bool EliminarHorasHombre(int id)
        {
            return CatHorasHombreDAO.EliminarHorasHombre(id);
        }

        public List<string> ObtenerDepartamento(int clave_depto)
        {
            return CatHorasHombreDAO.ObtenerDepartamento(clave_depto);
        }

        //public List<tblP_CC> GetCCHorasHombre(int idCC)
        //{
        //    return CatHorasHombreDAO.GetCCHorasHombre(idCC);
        //}
        public Dictionary<string, object> ObtenerComboCCAmbasEmpresas()
        {
            return CatHorasHombreDAO.ObtenerComboCCAmbasEmpresas();
        }
        public Dictionary<string, object> ObtenerAreasPorCC(List<string> ccsCplan, int idEmpresa)
        {
            return CatHorasHombreDAO.ObtenerAreasPorCC(ccsCplan, idEmpresa);
        }
    }
}