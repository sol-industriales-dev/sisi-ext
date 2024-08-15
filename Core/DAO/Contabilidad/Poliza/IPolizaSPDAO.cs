using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Contabilidad.Poliza
{
    public interface IPolizaSPDAO
    {
        void SetContext(object context);
        void SetTransaccion(object transaccion);
        object GetTransaccion();
        string GuardarPoliza<Tpoliza, Tmovimientos>(Tpoliza poliza, List<Tmovimientos> movimientos);
        int GetNumeroPolizaNueva(int year, int mes, string tp);
        bool ReferenciaDisponible(DateTime fechapol, int cta, int referencia);
        bool GuardarParaConciliar<T>(T chequera);
        string EstatusPoliza(int year, int mes, int poliza, string tp);
    }
}
