using Core.DAO.Maquinaria.Catalogos;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Maquinaria.Overhaul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Catalogos
{
    public class subConjuntoModeloServices : IsubConjuntoModeloDAO
    {
        #region Atributos
        private IsubConjuntoModeloDAO m_interfazDAO;
        #endregion Atributos

        #region Propiedades
        private IsubConjuntoModeloDAO interfazDAO
        {
            get { return m_interfazDAO; }
            set { m_interfazDAO = value; }
        }
        #endregion Propiedades

        #region Constructores
        public subConjuntoModeloServices(IsubConjuntoModeloDAO InterfazDAO)
        {
            this.interfazDAO = InterfazDAO;
        }
        #endregion Constructores
        public void Guardar(tblM_SubConjuntoModelo obj)
        {
            interfazDAO.Guardar(obj);
        }
        public List<tblM_SubConjuntoModelo> getDataSubConjuntoModelo(int idModelo)
        {
            return interfazDAO.getDataSubConjuntoModelo(idModelo);
        }
        public List<tblM_CatConjunto> FillCboConjunto()
        {
            return interfazDAO.FillCboConjunto();
        }
        public List<tblM_CatSubConjunto> FillCboSubConjunto(int idConjunto)
        {
            return interfazDAO.FillCboSubConjunto(idConjunto);
        }

        public List<tblM_CatModeloEquipotblM_CatSubConjunto> FillGridSubConjunto(int idModelo) 
        {
            return interfazDAO.FillGridSubConjunto(idModelo);
        }
    }
}
