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
    public class TipoMantenimientoDAO : GenericDAO<tblM_CatTipoMantenimiento>, ITiposMantenimientosDAO
    {
    }
}
