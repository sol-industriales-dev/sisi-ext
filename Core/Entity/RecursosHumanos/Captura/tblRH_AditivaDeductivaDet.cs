using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Captura
{
    //raguilar 23/11/17
   public class tblRH_AditivaDeductivaDet
    {
        public int id { get; set; }
        public int id_AditivaDeductiva { get; set; }
        public string puesto { get; set; }
        public string  categoria { get; set; }
        public int personalNecesario { get; set; }
        public int  personalExistente { get; set; }
        public int personalFaltante { get; set; }
        public int  lugaresPlantilla { get; set; }
        public int numPersTotal { get; set; }
        public int aditiva { get; set; }
        public int  deductiva { get; set; }
        public string justificacion { get; set; }   
       //raguilar nueva variable 19/12/17
        public bool estado { get; set; }
        public bool nuevo { get; set; }
        public int? idPuesto { get; set; }
   }
}
