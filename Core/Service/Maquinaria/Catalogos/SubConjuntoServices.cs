using Core.DAO.Maquinaria.Catalogos;
using Core.Entity.Maquinaria.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Catalogos
{
    public class SubConjuntoServices : ISubConjuntoDAO
    {
          #region Atributos
        private ISubConjuntoDAO m_subConjuntoDAO;
        #endregion
        #region Propiedades
        public ISubConjuntoDAO SubConjuntoDAO
        {
            get { return m_subConjuntoDAO; }
            set { m_subConjuntoDAO = value; }
        }
        #endregion
        #region Constructores
        public SubConjuntoServices(ISubConjuntoDAO subConjuntoDAO)
        {
            this.SubConjuntoDAO = subConjuntoDAO;
        }
        #endregion
        public void Guardar(tblM_CatSubConjunto obj)
        {
            SubConjuntoDAO.Guardar(obj);
        }

        public List<tblM_CatSubConjunto> FillGridSubConjunto(tblM_CatSubConjunto obj)
        {
            return SubConjuntoDAO.FillGridSubConjunto(obj);
        }
        public List<tblM_CatConjunto> FillCboConjuntos(bool estatus)
        {
            return SubConjuntoDAO.FillCboConjuntos(estatus);
        }


    }
}
