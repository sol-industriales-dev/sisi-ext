using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.RecursosHumanos.Captura;


namespace Core.DAO.RecursosHumanos.Captura
{       //raguilar 23/11/17

    public  interface IAutorizacionAditivaDeductivaDAO
    {
        tblRH_AutorizacionAditivaDeductiva GuardarAutorizacion(tblRH_AutorizacionAditivaDeductiva objAutorizacion);
        List<tblRH_AutorizacionAditivaDeductiva> getAutorizacion(int idAditiva);
        tblRH_AutorizacionAditivaDeductiva SaveChangesAutorizacionCambios(tblRH_AutorizacionAditivaDeductiva objAutorizacion);
        tblRH_AutorizacionAditivaDeductiva getAutorizacionIndividual(int idFirma);
        void EliminarAutorizador(tblRH_AutorizacionAditivaDeductiva autorizador);
        bool EsUsuarioMismoCC(int usuarioCapturoID);
    }
}
