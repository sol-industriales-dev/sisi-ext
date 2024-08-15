using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Contabilidad.Cuenta
{
    public interface ICuentaDAO
    {
        object BuscarCuenta(string term);
        object GetCuenta(int cta, int scta, int sscta);
        object GetCuenta(int cta, int scta, string descripcion);
        object GetCuentas(List<int> ctas);
    }
}
