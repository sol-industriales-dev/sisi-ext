using Core.DAO.Proyecciones;
using Core.Entity.Administrativo.Proyecciones;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Proyecciones
{
    public class TESTDATADAO : GenericDAO<tbl_TESTDATA>, ITESTDATADAO
    {
        public List<tbl_TESTDATA> getListaDATA(int id)
        {
            return _context.tbl_TESTDATA.Where(x => x.tipo == id).ToList();
        }
    }
}
