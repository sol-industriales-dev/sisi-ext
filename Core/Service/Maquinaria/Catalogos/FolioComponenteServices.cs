using Core.DAO.Maquinaria.Catalogos;
using Core.Entity.Maquinaria.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Catalogos
{
    public class FolioComponenteServices : IFolioComponenteDAO
    {
         #region Atributos
        private IFolioComponenteDAO m_folioComponenteDAO;
        #endregion

        #region Propiedades
        public IFolioComponenteDAO FolioComponenteDAO
        {
            get { return m_folioComponenteDAO; }
            set { m_folioComponenteDAO = value; }
        }
        #endregion

        #region Constructores
        public FolioComponenteServices(IFolioComponenteDAO folioComponenteDAO)
        {
            this.FolioComponenteDAO = folioComponenteDAO;
        }
        #endregion

        public void Guardar(tblM_FolioComponente obj)
        {
            FolioComponenteDAO.Guardar(obj);
        }
        public tblM_FolioComponente getFolio(tblM_FolioComponente obj)
        {
            return FolioComponenteDAO.getFolio(obj);
        }
        public bool Exists(tblM_FolioComponente obj)
        {
            return FolioComponenteDAO.Exists(obj);
        }

    }
}
