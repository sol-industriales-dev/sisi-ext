using Core.Entity.Maquinaria.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Maquinaria.Catalogos
{
    public interface ITipoBajaDAO
    {
        List<tblM_CatTipoBaja> FillCboTipoBaja();
    }
}
