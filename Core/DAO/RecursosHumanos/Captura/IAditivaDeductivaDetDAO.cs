using Core.Entity.RecursosHumanos.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.RecursosHumanos.Captura;
namespace Core.DAO.RecursosHumanos.Captura
{       //raguilar 23/11/17
    public  interface IAditivaDeductivaDetDAO
    {
        tblRH_AditivaDeductivaDet GuardarAditivaDeducDet(tblRH_AditivaDeductivaDet objAditivaDEductivaDet);
        List<tblRH_AditivaDeductivaDet> getAditivaDeductivaDet(int idAditiva);
        void eliminarDetalle(int formatoDetalleID);
    }
}
