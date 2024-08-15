using Core.DAO.Administracion.Seguridad.AgrupacionCC;
using Core.DTO.Administracion.Seguridad.AgrupacionCC;
using Core.DTO.Principal.Generales;
using Core.Entity.Administrativo.Seguridad.Indicadores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Administracion.Seguridad.AgrupacionCC
{
    public class AgrupacionCCService : IAgrupacionCCDAO
    {
        private IAgrupacionCCDAO m_IAgrupacionCCDAO;
        private IAgrupacionCCDAO AgrupacionCCDAO
        {
            get { return m_IAgrupacionCCDAO; }
            set { m_IAgrupacionCCDAO = value; }
        }
        public AgrupacionCCService(IAgrupacionCCDAO AgrupacionCCDAO)
        {
            this.AgrupacionCCDAO = AgrupacionCCDAO;
        }
        public List<ComboDTO> getCC()
        {
            return AgrupacionCCDAO.getCC();
        }
        public List<ComboDTO> getCCTodos(int idAgrupacionCC)
        {
            return AgrupacionCCDAO.getCCTodos(idAgrupacionCC);
        }
        public List<ComboDTO> ObtnerAgrupacion()
        {
            return AgrupacionCCDAO.ObtnerAgrupacion();
        }
        public List<AgrupacionCCDTO> GetDetalleAgrupacion(int idAgrupacionCC)
        {
            return AgrupacionCCDAO.GetDetalleAgrupacion(idAgrupacionCC);
        }
        public AgrupacionCCDTO CrearAgrupacion(AgrupacionCCDTO objAgrupaciones, List<tblS_IncidentesAgrupacionCCDet> lstAgrupaciones)
        {
            return AgrupacionCCDAO.CrearAgrupacion(objAgrupaciones,lstAgrupaciones);
        }
        public bool EditarAgrupacion(int id, string NuevoNombre, string[] lstAgrupacion)
        {
            return AgrupacionCCDAO.EditarAgrupacion(id,NuevoNombre, lstAgrupacion);
        }
        public bool EliminarAgrupacion(int id, int esActivo)
        {
            return AgrupacionCCDAO.EliminarAgrupacion(id,esActivo);
        }
        public List<ComboDTO> obtenerAgrupacionCombo()
        {
            return AgrupacionCCDAO.obtenerAgrupacionCombo();
        }
    }
}
