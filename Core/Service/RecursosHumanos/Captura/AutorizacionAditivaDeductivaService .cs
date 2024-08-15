using Core.DAO.RecursosHumanos.Captura;
using Core.Entity.RecursosHumanos.Captura;
using Core.Entity.RecursosHumanos.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Data.Factory.RecursosHumanos.Captura
{
    //reaguilar 23/11/17
    public class AutorizacionAditivaDeductivaService: IAutorizacionAditivaDeductivaDAO
    {
        public IAutorizacionAditivaDeductivaDAO aditivaDeductivaDAO { get; set; }

        public AutorizacionAditivaDeductivaService(IAutorizacionAditivaDeductivaDAO AutAditivaDeductiva)
        {
            this.aditivaDeductivaDAO = AutAditivaDeductiva;
        }
        public tblRH_AutorizacionAditivaDeductiva GuardarAutorizacion(tblRH_AutorizacionAditivaDeductiva objAutorizacion)
        {
            return aditivaDeductivaDAO.GuardarAutorizacion(objAutorizacion);
        }
        public List<tblRH_AutorizacionAditivaDeductiva> getAutorizacion(int idAditiva)
        {
            return aditivaDeductivaDAO.getAutorizacion(idAditiva);
        }
        public tblRH_AutorizacionAditivaDeductiva SaveChangesAutorizacionCambios(tblRH_AutorizacionAditivaDeductiva objAutorizacion) 
        {
            return aditivaDeductivaDAO.SaveChangesAutorizacionCambios(objAutorizacion);
        }

        public tblRH_AutorizacionAditivaDeductiva getAutorizacionIndividual(int idFirma) 
        {
            return aditivaDeductivaDAO.getAutorizacionIndividual(idFirma);
        }
        public void EliminarAutorizador(tblRH_AutorizacionAditivaDeductiva objAutorizacion)
        {
            aditivaDeductivaDAO.EliminarAutorizador(objAutorizacion);
        }

        public bool EsUsuarioMismoCC(int usuarioCapturoID)
        {
            return aditivaDeductivaDAO.EsUsuarioMismoCC(usuarioCapturoID);
        }
    }
}
