using Core.DAO.Maquinaria.Catalogos;
using Core.Entity.Maquinaria.Catalogo;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Maquinaria.Catalogos
{
    public class TipoBajaDAO : GenericDAO<tblM_CatTipoBaja>, ITipoBajaDAO
    {
        public List<tblM_CatTipoBaja> FillCboTipoBaja()
        {
            return _context.tblM_CatTipoBaja.ToList();
        }
    }
}
