using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTO.RecursosHumanos;
namespace Core.DTO.RecursosHumanos
{
    public class rptAditivaDeductivaDTO
    {
        //public int id { get; set; }
        //public int id_AditivaDeductiva { get; set; }
        public int id { get; set; }
        //obj1
        public string puesto { get; set; }
        public int personalFaltante { get; set; }
        public int lugaresPlantilla { get; set; }
        public int numPersTotal { get; set; }
        public int aditiva { get; set; }
        public int deductiva { get; set; }
        public string justificacion { get; set; }
        //obj2
        //public List<int> personalNecesario { get; set; }
        //public List<int> personalExistente { get; set; }
        //public List<string> categoria { get; set; }

        //public List<rptAditivaDeductivadetDTO> rptAditivaDeductivadetDTO { get; set; }

    }
}
