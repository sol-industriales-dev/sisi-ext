using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Desempeno
{
    public class EventoCalendarioDTO
    {
        public string title { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string classNames { get; set; }
        public string backgroundColor { get; set; }
        public bool inicio { get; set; }
    }
}