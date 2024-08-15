using Core.DAO.Principal.Usuarios;
using Core.DTO.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Principal.Usuarios
{
    public class OrganigramaService : IOrganigramaDAO
    {
        #region Atributos
        private IOrganigramaDAO m_organigramaDAO;
        #endregion

        #region Propiedades
        public IOrganigramaDAO OrganigramaDAO
        {
            get { return m_organigramaDAO; }
            set { m_organigramaDAO = value; }
        }
        #endregion

        #region Constructores
        public OrganigramaService(IOrganigramaDAO organigramaDAO)
        {
            this.OrganigramaDAO = organigramaDAO;
        }
        #endregion
        public List<OrganigramaDTO> getByUserID(int id)
        {
            return OrganigramaDAO.getByUserID(id);
        }
    }
}
