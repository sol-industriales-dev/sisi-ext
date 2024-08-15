using Core.DAO.Maquinaria.Inventario;
using Core.Entity.Maquinaria.Catalogo;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Maquinaria.Catalogos
{
    public class HistInventarioDAO : GenericDAO<tblM_HistInventario>, IHistInventarioDAO
    {

        public string GetInfoHistorial(DateTime fecha)
        {
            string JsonString = "";

            var obj = _context.tblM_HistInventario.ToList().FirstOrDefault(x => x.Fecha.Date == fecha.Date).CadenaInv;

            if(obj!=null)
            {
                JsonString = obj;
            }
            return JsonString;
        }
    }
}
