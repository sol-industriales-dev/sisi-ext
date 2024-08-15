using Core.DAO.Maquinaria.Catalogos;
using Core.Entity.Maquinaria.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Catalogos
{
    public class TipoAceitesServices : ITiposAceitesDAO
    {
        #region Atributos
        private ITiposAceitesDAO m_interfazDAO;
        #endregion

        #region Propiedades
        private ITiposAceitesDAO interfazDAO
        {
            get { return m_interfazDAO; }
            set { m_interfazDAO = value; }
        }
        #endregion

        #region Constructores
        public TipoAceitesServices(ITiposAceitesDAO InterfazDAO)
        {
            this.interfazDAO = InterfazDAO;
        }
        #endregion

        public void Guardar(tblM_CatTiposAceites obj)
        {
            interfazDAO.Guardar(obj);
        }

        public List<tblM_CatTiposAceites> GetListaAceites(string descripcion, bool estatus)
        {
            return interfazDAO.GetListaAceites(descripcion, estatus);
        }


    }
}
