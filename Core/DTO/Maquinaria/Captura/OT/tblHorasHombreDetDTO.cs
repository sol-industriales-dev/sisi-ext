using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura.OT
{
    public class tblHorasHombreDetDTO
    {
        public int personalID { get; set; }
        public string personalNombre { get; set; }
        public decimal hrasPreventivo { get; set; }
        public decimal hrasPredictivo { get; set; }
        public decimal hrasCorrectivo { get; set; }
        public int cantidadOT { get; set; }
        public decimal promedioHrasOT { get; set; }
        public string puesto { get; set; }
    }
}
