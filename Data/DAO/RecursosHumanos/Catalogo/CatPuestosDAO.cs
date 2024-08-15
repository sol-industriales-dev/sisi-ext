using Core.Entity.RecursosHumanos.Catalogos;
using Data.EntityFramework.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.RecursosHumanos.Catalogo
{
    public class CatPuestosDAO
    {
        public IList<tblRH_catPuestos> FillGridTaller()
        {
            var result = (IList<tblRH_catPuestos>)_contextEnkontrol.Where("SELECT * FROM 'DBA'.'si_puestos'").ToObject<IList<tblRH_catPuestos>>();

            return result.ToList();
        }

    }
}
