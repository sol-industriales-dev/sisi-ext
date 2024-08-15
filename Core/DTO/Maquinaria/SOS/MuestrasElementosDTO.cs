using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.SOS
{
    public class MuestrasElementosDTO
    {
        public string al { get; set; }
        public string fe { get; set; }
        public string si { get; set; }
        public string cu { get; set; }
        public DateTime  fecha { get; set; }
        public string name { get; set; }
        
        public string hora_Aceite { get; set; }
        public string hora_equipo { get; set; }
        public string description { get; set; }

    }
}
