using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura.OT
{
    public class FiltrosRtpHorasHombre
    {

        public List<string> CC { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int MotivoParo { get; set; }
        public int TipoParo { get; set; }
        public int TipoMatto { get; set; }
        public int grupo { get; set; }
        public int modelo { get; set; }
        public int economico { get; set; }
        public int Estatus { get; set; }
        public int CondicionParo { get; set; }
        
    }
}
