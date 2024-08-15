using Core.DAO.Maquinaria.Catalogos;
using Core.Entity.Maquinaria.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Catalogos
{
    public class ConjuntoServices : IConjuntoDAO
    {
        #region Atributos
        private IConjuntoDAO m_conjuntoDAO;
        #endregion
        #region Propiedades
        public IConjuntoDAO ConjuntoDAO
        {
            get { return m_conjuntoDAO; }
            set { m_conjuntoDAO = value; }
        }
        #endregion
        #region Constructores
        public ConjuntoServices(IConjuntoDAO conjuntoDAO)
        {
            this.ConjuntoDAO = conjuntoDAO;
        }
        #endregion
        public void Guardar(tblM_CatConjunto obj)
        {
            ConjuntoDAO.Guardar(obj);
        }
        public List<tblM_CatConjunto> FillGridConjunto(tblM_CatConjunto obj)
        {
            return ConjuntoDAO.FillGridConjunto(obj);
        }

    }
}
