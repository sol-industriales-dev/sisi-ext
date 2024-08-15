using Core.Entity.RecursosHumanos.Captura;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.RecursosHumanos.Captura
{
    public interface IAutorizacionFormatoCambio
    {

     
        tblRH_AutorizacionFormatoCambio SaveChangesAutorizacionCambios(tblRH_AutorizacionFormatoCambio objAutorizacion);
        List<tblRH_AutorizacionFormatoCambio> getAutorizacion(int idFormato);
        void EliminarAutorizador(tblRH_AutorizacionFormatoCambio objAutorizacion);
        void EliminarAutorizadores(int id);
        int getFormatoIDByAutorizacion(int id);
        bool EsUsuarioMismoCC(int usuarioID);
        Dictionary<string, object> CancelarFormatoCambioPorTiempo();
        DataTable getInfoEnca(string nombreReporte, string area);

    }
}
