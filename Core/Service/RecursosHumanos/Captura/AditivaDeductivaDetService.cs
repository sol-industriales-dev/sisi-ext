using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DAO.RecursosHumanos.Captura;
using Core.Entity.RecursosHumanos.Captura;
namespace Core.Service.RecursosHumanos.Captura
{
    public class AditivaDeductivaDetService: IAditivaDeductivaDetDAO
    {
        public  IAditivaDeductivaDetDAO aditivaDeductiva { get; set; }

        public AditivaDeductivaDetService(IAditivaDeductivaDetDAO AditivaDeducDet) 
        {
            this.aditivaDeductiva = AditivaDeducDet;
        }

        public tblRH_AditivaDeductivaDet GuardarAditivaDeducDet(tblRH_AditivaDeductivaDet objAditivaDeductivaDET)
        {
            return aditivaDeductiva.GuardarAditivaDeducDet(objAditivaDeductivaDET);
        }
        public List<tblRH_AditivaDeductivaDet> getAditivaDeductivaDet(int idAditiva) {
            return aditivaDeductiva.getAditivaDeductivaDet(idAditiva);
        }

        public void eliminarDetalle(int formatoDetalleID)
        {
             aditivaDeductiva.eliminarDetalle(formatoDetalleID);
        }
    }
}

