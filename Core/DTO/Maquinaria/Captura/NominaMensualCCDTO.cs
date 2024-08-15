using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura
{
    public class NominaMensualCCDTO
    {
        public int id { get; set; }
        public int nominaID { get; set; }
        public string mes { get; set; }
        public int numeroMes { get; set; }
        public string año { get; set; }
        public decimal nominaIMSS { get; set; }
        public decimal nominaInfonavit { get; set; }
        public decimal ISN { get; set; }
        public decimal ISR { get; set; }
        public string estatus { get; set; }
        public string areaCuenta { get; set; }
        public string proyecto { get; set; }
        public decimal horasHombreTotales { get; set; }
    }
}
