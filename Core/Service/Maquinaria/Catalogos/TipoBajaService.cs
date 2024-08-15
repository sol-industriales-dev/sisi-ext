using Core.DAO.Maquinaria.Catalogos;
using Core.Entity.Maquinaria.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Catalogos
{
    public class TipoBajaService : ITipoBajaDAO
    {
        #region Atributos
        private ITipoBajaDAO m_baja;
        #endregion
        #region Propiedades
        public ITipoBajaDAO ITipoBajaDAO
        {
            get { return m_baja; }
            set { m_baja = value; }
        }
        #endregion
        #region Constructores
        public TipoBajaService(ITipoBajaDAO tipoBajaDAO)
        {
            this.ITipoBajaDAO = tipoBajaDAO;
        }
        #endregion
        public List<tblM_CatTipoBaja> FillCboTipoBaja()
        {
            return m_baja.FillCboTipoBaja();
        }
    }
}
