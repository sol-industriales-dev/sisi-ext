using Core.Entity.Administrativo.Proyecciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Proyecciones
{
    public interface ITESTDATADAO
    {

        List<tbl_TESTDATA> getListaDATA(int id);
    }
}
