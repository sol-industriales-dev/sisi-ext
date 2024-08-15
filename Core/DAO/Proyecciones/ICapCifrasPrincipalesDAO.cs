using Core.Entity.Administrativo.Proyecciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Proyecciones
{
    public interface ICapCifrasPrincipalesDAO
    {
        void Guardar(tblPro_CapCifrasPrincipales obj);

        void BorrarEscenarios(int mes, int anio, string escenarios);

        tblPro_CapCifrasPrincipales getOBJCifrasPrincipales(int mes, int anio, string escenario, int tipo);

        List<string> getEscenariosConfiguraciones(int mes, int anio);
    }
}
