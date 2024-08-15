using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos
{
    public class RepAditivaDeductivaDTO
    {
        public string cC { get; set; }
        public DateTime fechaCaptura { get; set; }
        public string puesto { get; set; }
        public string categoria { get; set; }
        public int personalNecesario { get; set; }
        public int personalExistente { get; set; }
        public int personalFaltante { get; set; }
        public int lugaresPlantilla { get; set; }
        public int numPersTotal { get; set; }
        public int aditiva { get; set; }
        public int deductiva { get; set; }
        public string justificacion { get; set; }   
    }
}
