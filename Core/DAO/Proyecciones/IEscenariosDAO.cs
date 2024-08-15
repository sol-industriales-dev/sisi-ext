using Core.Entity.Administrativo.Proyecciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Proyecciones
{
    public interface IEscenariosDAO
    {
        void Guardar(tblPro_CatEscenarios obj);

        List<tblPro_CatEscenarios> GetListaEscenarios();

        List<tblPro_CatEscenarios> GetListaEscenariosPrincipales();

        List<tblPro_CatEscenarios> GetListEscenariosTable(int id,string descripcion);
        tblPro_CatEscenarios CatEscenarioByID(int id);
    }
}
