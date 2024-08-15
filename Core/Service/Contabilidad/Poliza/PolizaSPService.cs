using Core.DAO.Contabilidad.Poliza;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Contabilidad.Poliza
{
    public class PolizaSPService : IPolizaSPDAO
    {
        private IPolizaSPDAO _polizaDAO;

        private IPolizaSPDAO PolizaDAO
        {
            get { return _polizaDAO; }
            set { _polizaDAO = value; }
        }

        public PolizaSPService(IPolizaSPDAO poliza)
        {
            this.PolizaDAO = poliza;
        }

        public void SetContext(object context)
        {
            this.PolizaDAO.SetContext(context);
        }

        public void SetTransaccion(object transaccion)
        {
            this.PolizaDAO.SetTransaccion(transaccion);
        }

        public object GetTransaccion()
        {
            return this.PolizaDAO.GetTransaccion();
        }

        public string GuardarPoliza<Tpoliza, Tmovimientos>(Tpoliza poliza, List<Tmovimientos> movimientos)
        {
            return this.PolizaDAO.GuardarPoliza(poliza, movimientos);
        }

        public int GetNumeroPolizaNueva(int year, int mes, string tp)
        {
            return this.PolizaDAO.GetNumeroPolizaNueva(year, mes, tp);
        }

        public bool ReferenciaDisponible(DateTime fechapol, int cta, int referencia)
        {
            return this.PolizaDAO.ReferenciaDisponible(fechapol, cta, referencia);
        }

        public bool GuardarParaConciliar<T>(T chequera)
        {
            return this.PolizaDAO.GuardarParaConciliar(chequera);
        }

        public string EstatusPoliza(int year, int mes, int poliza, string tp)
        {
            return this.PolizaDAO.EstatusPoliza(year, mes, poliza, tp);
        }
    }
}
