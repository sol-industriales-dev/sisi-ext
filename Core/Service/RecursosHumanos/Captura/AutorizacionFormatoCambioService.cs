using Core.DAO.RecursosHumanos.Captura;
using Core.Entity.RecursosHumanos.Captura;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.RecursosHumanos.Captura
{
    public class AutorizacionFormatoCambioService : IAutorizacionFormatoCambio
    {
        #region Atributos
        private IAutorizacionFormatoCambio m_Autorizacion;
        #endregion
        #region Propiedades
        public IAutorizacionFormatoCambio autorizacionformatoCambio
        {
            get { return m_Autorizacion; }
            set { m_Autorizacion = value; }
        }
        #endregion
        #region Constructores
        #endregion

        public AutorizacionFormatoCambioService(IAutorizacionFormatoCambio AutorizacionFormatoCambio)
        {
            this.autorizacionformatoCambio = AutorizacionFormatoCambio;
        }

        public tblRH_AutorizacionFormatoCambio SaveChangesAutorizacionCambios(tblRH_AutorizacionFormatoCambio objAutorizacion)
        {
            return autorizacionformatoCambio.SaveChangesAutorizacionCambios(objAutorizacion);
        }

        public List<tblRH_AutorizacionFormatoCambio> getAutorizacion(int idFormato)
        {
            return autorizacionformatoCambio.getAutorizacion(idFormato);
        }

        public void EliminarAutorizador(tblRH_AutorizacionFormatoCambio objAutorizacion)
        {
            autorizacionformatoCambio.EliminarAutorizador(objAutorizacion);
        }

        public void EliminarAutorizadores(int id)
        {
            autorizacionformatoCambio.EliminarAutorizadores(id);
        }

        public int getFormatoIDByAutorizacion(int id)
        {
            return autorizacionformatoCambio.getFormatoIDByAutorizacion(id);
        }


        public bool EsUsuarioMismoCC(int usuarioID)
        {
            return autorizacionformatoCambio.EsUsuarioMismoCC(usuarioID);
        }

        public Dictionary<string, object> CancelarFormatoCambioPorTiempo()
        {
            return autorizacionformatoCambio.CancelarFormatoCambioPorTiempo();
        }

        public DataTable getInfoEnca(string nombreReporte, string area)
        {
            return autorizacionformatoCambio.getInfoEnca(nombreReporte, area);
        }
    }
}
