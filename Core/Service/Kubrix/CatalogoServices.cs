using Core.DAO.Kubrix;
using Core.DTO.Facturacion;
using Core.Entity.Kubrix;
using Core.Entity.Kubrix.Analisis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Kubrix
{
    public class CatalogoServices : ICatalogoDAO
    {
        #region Atributos
        private ICatalogoDAO k_CatalogoDAO;
        #endregion
        #region Propiedades
        public ICatalogoDAO CatalogoDAO
        {
            get { return k_CatalogoDAO; }
            set { k_CatalogoDAO = value; }
        }
        #endregion
        #region Constructores
        public CatalogoServices(ICatalogoDAO catalogoDAO)
        {
            this.CatalogoDAO = catalogoDAO;
        }
        #endregion
        public List<ComboDTO> getCboDivision()
        {
            return this.CatalogoDAO.getCboDivision();
        }
        public List<tblK_catDivision> getLstDiv()
        {
            return this.CatalogoDAO.getLstDiv();
        }
        public List<tblK_catCcDiv> getlstCcDiv(string cc, int idDiv)
        {
            return this.CatalogoDAO.getlstCcDiv(cc, idDiv);
        }
        public List<tblK_Bal12> getlstBal12(string cc)
        {
            return this.CatalogoDAO.getlstBal12(cc);
        }
    }
}
