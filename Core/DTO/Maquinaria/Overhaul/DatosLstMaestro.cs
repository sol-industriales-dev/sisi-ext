using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Overhaul
{
    public class DatosLstMaestro
    {
        public string obra { get; set; }
        public string periodo { get; set; }
        public DateTime fecha { get; set; }
        public string obraMaquina { get; set; }
        public string noEconomico { get; set; }
        public decimal ritmo { get; set; }
        public decimal horasComponente { get; set; }
        public decimal target { get; set; }
        public DateTime proximoPCR { get; set; }
        public string cause { get; set; }
        public string elaboro { get; set; }
        public string facilitador { get; set; }
        public string reviso { get; set; }
    }
}
