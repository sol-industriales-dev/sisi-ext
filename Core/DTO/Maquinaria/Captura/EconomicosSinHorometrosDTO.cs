using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura
{
    public class EconomicosSinHorometrosDTO
    {
        public string cc { get; set; }
        public string ccDesc { get; set; }
        public string economico { get; set; }
        public decimal horometroAcumulado { get; set; }
        public string horometroAcumuladoDesc { get; set; }
        public DateTime fecha { get; set; }
        public string fechaString { get; set; }
        public decimal diasTranscurridos { get; set; }
    }
}
