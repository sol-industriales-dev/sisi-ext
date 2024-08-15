using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Overhaul
{
    public class rptInventarioDTO
    {
        public string subconjunto { get; set; }
        public int total { get; set; }
        public int almacenMLC { get; set; }
        public int almacenMNB { get; set; }
        public int almacenMSA { get; set; }
        public int almacenTMC { get; set; }
        public int almacenMTAK { get; set; }
        public int komatsu { get; set; }
        public int madisa { get; set; }
        public int matco { get; set; }
        public int piinsa { get; set; }
        public int soluciones { get; set; }
        public int tmcCRC { get; set; }
    }
}
