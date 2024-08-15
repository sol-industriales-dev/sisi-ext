using Core.Entity.RecursosHumanos.Captura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos
{
   public class CatFormatoAditivaDeductivaDTO
    {

       //raguilar 04/12/17
       public tblRH_AditivaDeductiva objAditivaDeductiva { get; set; }
       public List<tblRH_AditivaDeductivaDet> objAditivaDeductivaDet { get; set; }
       public List<tblRH_AutorizacionAditivaDeductiva> objAditivaDeducAut { get; set; }
    }
}
